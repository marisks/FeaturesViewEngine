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
            return base.FindView(controllerContext, viewName, masterName, false);
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            return base.FindPartialView(controllerContext, partialViewName, false);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return base.CreateView(
                controllerContext,
                viewPath.Replace(FeaturePlaceholder, ResolveFeaturePath(controllerContext)),
                masterPath.Replace(FeaturePlaceholder, ResolveFeaturePath(controllerContext)));
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return base.CreatePartialView(
                controllerContext,
                partialPath.Replace(FeaturePlaceholder, ResolveFeaturePath(controllerContext)));
        }

        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            return base.FileExists(
                controllerContext,
                virtualPath.Replace(FeaturePlaceholder, ResolveFeaturePath(controllerContext)));
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

        private string ResolveFeaturePath(ControllerContext controllerContext)
        {
            if (controllerContext.Controller == null) return string.Empty;
            var controllerType = controllerContext.Controller.GetType();
            var fullNamespace = controllerType.Namespace;
            if (fullNamespace == null) return string.Empty;
            var prefixToRemove = NamespacePrefixToRemove(controllerContext);
            return $"~{fullNamespace.Replace(prefixToRemove, string.Empty).Replace(".", "/")}";
        }
    }
}
