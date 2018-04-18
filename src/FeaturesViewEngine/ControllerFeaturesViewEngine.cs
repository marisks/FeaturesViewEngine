using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;

namespace FeaturesViewEngine
{
    /// <inheritdoc />
    /// <summary>
    /// Razor view engine which resolves views by the controller namespace.
    /// It replaces the placeholder '%feature%' with the namespace path to the controller excluding the namespace prefix to remove (by default assembly name).
    /// </summary>
    public abstract class ControllerFeaturesViewEngine : RazorViewEngine
    {
        // format is ":FeatureViewCacheEntry:{cacheType}:{featurePath}:{viewName}:{controllerName}:"
        private const string CacheKeyFormat = ":FeatureViewCacheEntry:{0}:{1}:{2}:{3}:{4}:";

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            var resolved = ResolveViewPath(controllerContext, viewName, ViewLocationFormats, useCache);
            if (string.IsNullOrEmpty(resolved.Item1))
            {
                return new ViewEngineResult(resolved.Item2);
            }
            return base.FindView(controllerContext, resolved.Item1, masterName, useCache);
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            var resolved = ResolveViewPath(controllerContext, partialViewName, PartialViewLocationFormats, useCache);
            if (string.IsNullOrEmpty(resolved.Item1))
            {
                return new ViewEngineResult(resolved.Item2);
            }
            return base.FindPartialView(controllerContext, resolved.Item1, useCache);
        }

        public static string FeaturePlaceholder = "%feature%";

        /// <summary>
        /// Returns namespace prefix to remove before building view path. Default implementation returns assembly name.
        /// </summary>
        public virtual string NamespacePrefixToRemove(ControllerContext controllerContext)
        {
            if (controllerContext.Controller == null) return string.Empty;
            var controllerType = controllerContext.Controller.GetType();
            var fullNamespace = controllerType.Namespace;
            return fullNamespace != null ? controllerType.Assembly.GetName().Name : string.Empty;
        }

        private Tuple<string, string[]> ResolveViewPath(
            ControllerContext controllerContext,
            string viewName,
            IEnumerable<string> formats,
            bool useCache)
        {
            var featurePath = GetFeaturePath(controllerContext);
            var controllerName = GetControllerName(controllerContext);
            var displayModes = GetAvailableDisplayModesForContext(controllerContext);
            var cacheKey = CreateCacheKey(featurePath, viewName, controllerName, displayModes);

            if (useCache)
            {
                var cachedLocation = ViewLocationCache.GetViewLocation(controllerContext.HttpContext, cacheKey);
                if (cachedLocation != null) return Tuple.Create(cachedLocation, new string[0]);
            }

            var resolved = ResolveViewPath(controllerContext, viewName, formats);
            if (!string.IsNullOrEmpty(resolved.Item1))
            {
                ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, resolved.Item1);
            }
            return Tuple.Create(resolved.Item1, resolved.Item2);
        }

        private string CreateCacheKey(string featurePath, string viewName, string controllerName, string[] displayModes)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                CacheKeyFormat,
                GetType().AssemblyQualifiedName,
                featurePath,
                viewName,
                controllerName,
                string.Join(",", displayModes)
            );
        }

        private Tuple<string, string[]> ResolveViewPath(ControllerContext controllerContext, string viewName, IEnumerable<string> formats)
        {
            if (IsSpecificPath(viewName)) return Tuple.Create(viewName, new string[0]);
            var featurePath = GetFeaturePath(controllerContext);
            if (string.IsNullOrEmpty(featurePath)) return Tuple.Create((string)null, new string[0]);
            var controllerName = GetControllerName(controllerContext);
            var displayModes = GetAvailableDisplayModesForContext(controllerContext);

            var searchLocations = formats
                .SelectMany(path => displayModes.Select(mode => FormatViewPath(path, featurePath, viewName, mode, controllerName)))
                .ToArray();
            var resolved = searchLocations.FirstOrDefault(viewPath => FileExists(controllerContext, viewPath));
            return Tuple.Create(resolved, searchLocations);
        }

        /// <summary>
        /// Method returns display modes that are valid for current request context
        /// </summary>
        /// <param name="controllerContext">Current controller context</param>
        /// <returns>Valid display modes for current request ordered by their priority (e.g. ["Tablet", "Mobile", ""])</returns>
        private string[] GetAvailableDisplayModesForContext(ControllerContext controllerContext)
        {
            return DisplayModeProvider.GetAvailableDisplayModesForContext(controllerContext.HttpContext, controllerContext.DisplayMode)
                .Select(mode => mode.DisplayModeId)
                .ToArray();
        }

        private static string GetControllerName(ControllerContext controllerContext)
        {
            return controllerContext.RouteData.GetRequiredString("controller");
        }

        private string GetFeaturePath(ControllerContext controllerContext)
        {
            if (controllerContext.Controller == null) return string.Empty;
            var controllerType = controllerContext.Controller.GetType();
            var fullNamespace = controllerType.Namespace;
            if (fullNamespace == null) return string.Empty;
            var prefixToRemove = NamespacePrefixToRemove(controllerContext);
            return fullNamespace.StartsWith(prefixToRemove)
                ? $"~{fullNamespace.Replace(prefixToRemove, string.Empty).Replace(".", "/")}"
                : string.Empty;
        }

        protected virtual string FormatViewPath(string formatString, string featurePath, string viewName, string displayMode, string controllerName)
        {
            var format = formatString.Replace(FeaturePlaceholder, featurePath);

            if (!string.IsNullOrEmpty(displayMode))
            {
                viewName = $"{viewName}.{displayMode}";
            }

            return string.Format(CultureInfo.InvariantCulture, format, viewName, controllerName);
        }

        private static bool IsSpecificPath(string name)
        {
            var c = name[0];
            return c == '~' || c == '/';
        }
    }
}
