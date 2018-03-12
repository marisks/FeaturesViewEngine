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
        private const string CacheKeyFormat = ":FeatureViewCacheEntry:{0}:{1}:{2}:{3}:";

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            var (resolved, searchLocations) = ResolveViewPath(controllerContext, viewName, ViewLocationFormats, useCache);
            if (string.IsNullOrEmpty(resolved))
            {
                return new ViewEngineResult(searchLocations);
            }
            return base.FindView(controllerContext, resolved, masterName, useCache);
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            var (resolved, searchLocations) = ResolveViewPath(controllerContext, partialViewName, PartialViewLocationFormats, useCache);
            if (string.IsNullOrEmpty(resolved))
            {
                return new ViewEngineResult(searchLocations);
            }
            return base.FindPartialView(controllerContext, resolved, useCache);
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

        private (string resolved, string[] searchLocations) ResolveViewPath(
            ControllerContext controllerContext,
            string viewName,
            IEnumerable<string> formats,
            bool useCache)
        {
            var featurePath = GetFeaturePath(controllerContext);
            var controllerName = GetControllerName(controllerContext);
            var cacheKey = CreateCacheKey(featurePath, viewName, controllerName);

            if (useCache)
            {
                var cachedLocation = ViewLocationCache.GetViewLocation(controllerContext.HttpContext, cacheKey);
                if (cachedLocation != null) return (cachedLocation, new string[0]);
            }

            var (location, searchLocations) = ResolveViewPath(controllerContext, viewName, formats);
            if (!string.IsNullOrEmpty(location))
            {
                ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, location);
            }
            return (location, searchLocations);
        }

        private string CreateCacheKey(string featurePath, string viewName, string controllerName)
        {
            return string.Format(CultureInfo.InvariantCulture, CacheKeyFormat,
                GetType().AssemblyQualifiedName, featurePath, viewName, controllerName);
        }

        private (string resolved, string[] searchLocations) ResolveViewPath(ControllerContext controllerContext, string viewName, IEnumerable<string> formats)
        {
            if (IsSpecificPath(viewName)) return (viewName, new string[0]);
            var featurePath = GetFeaturePath(controllerContext);
            if (string.IsNullOrEmpty(featurePath)) return (null, new string[0]);
            var controllerName = GetControllerName(controllerContext);
            var searchLocations = formats
                .Select(path => FormatViewPath(path, featurePath, viewName, controllerName))
                .ToArray();
            var resolved = searchLocations.FirstOrDefault(viewPath => FileExists(controllerContext, viewPath));
            return (resolved, searchLocations);
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

        protected virtual string FormatViewPath(string formatString, string featurePath, string viewName, string controllerName)
        {
            var format = formatString.Replace(FeaturePlaceholder, featurePath);
            return string.Format(CultureInfo.InvariantCulture, format, viewName, controllerName);
        }

        private static bool IsSpecificPath(string name)
        {
            var c = name[0];
            return c == '~' || c == '/';
        }
    }
}
