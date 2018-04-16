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


            DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("Custom")
            {
                ContextCondition = ctx => ctx.Request.Headers["DisplayMode"] == "Custom"
            });
        }
    }
}