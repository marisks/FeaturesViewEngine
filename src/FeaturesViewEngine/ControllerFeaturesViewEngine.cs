using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

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

        public static string FeaturePlaceholder = "%feature%";

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            var resolved = ResolveViewPath(controllerContext, viewName, ViewLocationFormats, useCache);
            if (string.IsNullOrEmpty(resolved.Path))
            {
                return new ViewEngineResult(resolved.SearchLocations);
            }
            return base.FindView(controllerContext, resolved.Path, masterName, useCache);
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            var resolved = ResolveViewPath(controllerContext, partialViewName, PartialViewLocationFormats, useCache);
            if (string.IsNullOrEmpty(resolved.Path))
            {
                return new ViewEngineResult(resolved.SearchLocations);
            }
            return base.FindPartialView(controllerContext, resolved.Path, useCache);
        }

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

        private ViewPathResult ResolveViewPath(
            ControllerContext controllerContext,
            string viewName,
            IEnumerable<string> formats,
            bool useCache)
        {
            if (controllerContext.Controller == null) return ViewPathResult.Empty;

            var featurePath = GetFeaturePath(controllerContext);
            var controllerName = GetControllerName(controllerContext);
            var displayModes = GetAvailableDisplayModesForContext(controllerContext);
            var cacheKey = CreateCacheKey(featurePath, viewName, controllerName, displayModes);

            if (useCache)
            {
                var cachedLocation = ViewLocationCache.GetViewLocation(controllerContext.HttpContext, cacheKey);
                if (cachedLocation != null) return ViewPathResult.Create(cachedLocation);
            }

            var resolved = ResolveViewPath(controllerContext, viewName, formats);

            ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, resolved.Path);

            return resolved;
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

        private ViewPathResult ResolveViewPath(ControllerContext controllerContext, string viewName, IEnumerable<string> formats)
        {
            if (IsSpecificPath(viewName)) return ViewPathResult.Create(viewName);

            var featurePath = GetFeaturePath(controllerContext);
            if (string.IsNullOrEmpty(featurePath)) return ViewPathResult.Empty;

            var controllerName = GetControllerName(controllerContext);
            var displayModes = GetAvailableDisplayModesForContext(controllerContext);

            var searchLocations = formats
                .SelectMany(path => displayModes.Select(mode => FormatViewPath(path, featurePath, viewName, mode, controllerName)))
                .ToArray();
            var resolved = searchLocations.FirstOrDefault(viewPath => FileExists(controllerContext, viewPath)) ?? string.Empty;

            return ViewPathResult.Create(resolved, searchLocations);
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

        private class ViewPathResult
        {
            public string Path { get; }
            public string[] SearchLocations { get; }

            public static readonly ViewPathResult Empty = new ViewPathResult(string.Empty, new string[0]);

            private ViewPathResult(string path, string[] searchLocations)
            {
                Path = path ?? throw new ArgumentNullException(nameof(path));
                SearchLocations = searchLocations ?? throw new ArgumentNullException(nameof(searchLocations));
            }

            public static ViewPathResult Create(string path)
            {
                return new ViewPathResult(path, new string[0]);
            }

            public static ViewPathResult Create(string path, string[] searchLocations)
            {
                return new ViewPathResult(path, searchLocations);
            }
        }
    }
}
