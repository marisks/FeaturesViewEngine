using System.Linq;

namespace FeaturesViewEngine
{
    public sealed class DefaultControllerFeaturesViewEngine : ControllerFeaturesViewEngine
    {
        public DefaultControllerFeaturesViewEngine()
        {
            var paths = new[]
            {
                $"{FeaturePlaceholder}/{{0}}.cshtml",
                $"{FeaturePlaceholder}/Views/{{0}}.cshtml",
                $"{FeaturePlaceholder}/Views/{{1}}{{0}}.cshtml"
            };

            ViewLocationFormats =
                paths
                    .Union(ViewLocationFormats)
                    .ToArray();

            PartialViewLocationFormats =
                paths
                    .Union(PartialViewLocationFormats)
                    .ToArray();
        }
    }
}