namespace FeaturesViewEngine
{
    public sealed class DefaultControllerFeaturesViewEngine : ControllerFeaturesViewEngine
    {
        public DefaultControllerFeaturesViewEngine()
        {
            // clean out default formats
            AreaMasterLocationFormats = new string[0];
            AreaPartialViewLocationFormats = new string[0];
            AreaViewLocationFormats = new string[0];
            MasterLocationFormats = new string[0];

            var paths = new[]
            {
                $"{FeaturePlaceholder}/{{0}}.cshtml",
                $"{FeaturePlaceholder}/Views/{{0}}.cshtml",
                $"{FeaturePlaceholder}/Views/{{1}}{{0}}.cshtml"
            };

            ViewLocationFormats = paths;
            PartialViewLocationFormats = paths;
        }
    }
}
