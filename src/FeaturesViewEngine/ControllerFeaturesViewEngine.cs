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
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            var resolved = ResolveViewPath(controllerContext, viewName, ViewLocationFormats);
            return base.FindView(controllerContext, resolved, masterName, useCache);
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            var resolved = ResolveViewPath(controllerContext, partialViewName, PartialViewLocationFormats);
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

        private string ResolveViewPath(ControllerContext controllerContext, string viewName, IEnumerable<string> formats)
        {
            var featurePath = ResolveFeaturePath(controllerContext);
            if (string.IsNullOrEmpty(featurePath)) return viewName;
            string controllerName = controllerContext.RouteData.GetRequiredString("controller");
            return formats
                .Select(path => FormatViewPath(path, featurePath, viewName, controllerName))
                .Where(viewPath => FileExists(controllerContext, viewPath))
                .DefaultIfEmpty(viewName)
                .First();
        }

        private string ResolveFeaturePath(ControllerContext controllerContext)
        {
            if (controllerContext.Controller == null) return string.Empty;
            var controllerType = controllerContext.Controller.GetType();
            var fullNamespace = controllerType.Namespace;
            if (fullNamespace == null) return string.Empty;
            var prefixToRemove = NamespacePrefixToRemove(controllerContext);
            return $"~{fullNamespace.Replace(prefixToRemove, string.Empty).Replace(".", "/")}";
        }

        private static string FormatViewPath(string formatString, string featurePath, string viewName, string controllerName)
        {
            var format = formatString.Replace(FeaturePlaceholder, featurePath);
            return string.Format(CultureInfo.InvariantCulture, format, viewName, controllerName);
        }
    }
}
