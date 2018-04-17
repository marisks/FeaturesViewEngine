using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;

namespace FeaturesViewEngine.Demo
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            RouteTable.Routes.MapMvcAttributeRoutes();

            ViewEngines.Engines.Insert(0, new DefaultControllerFeaturesViewEngine());

            DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("Tablet")
            {
                ContextCondition = ctx => ctx.Request.Headers["DisplayMode"] == "Tablet"
            });

            DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("Mobile")
            {
                ContextCondition = ctx => ctx.Request.Headers["DisplayMode"] == "Mobile"
            });
        }
    }
}