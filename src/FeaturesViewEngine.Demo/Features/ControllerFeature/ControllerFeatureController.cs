using System.Web.Mvc;
using FeaturesViewEngine.Demo.Models;

namespace FeaturesViewEngine.Demo.Features.ControllerFeature
{
    [RoutePrefix("ControllerFeature")]
    public class ControllerFeatureController : Controller
    {
        [Route(nameof(Index))]
        public ActionResult Index()
        {
            return View();
        }

        [Route(nameof(IndexByName))]
        public ActionResult IndexByName()
        {
            return View("Index");
        }

        [Route(nameof(IndexBySpecificName))]
        public ActionResult IndexBySpecificName()
        {
            return View("~/Features/ControllerFeature/Index.cshtml");
        }

        [Route(nameof(Partial))]
        public ActionResult Partial()
        {
            return PartialView();
        }

        [Route(nameof(PartialByName))]
        public ActionResult PartialByName()
        {
            return PartialView("Partial");
        }

        [Route(nameof(PartialBySpecificName))]
        public ActionResult PartialBySpecificName()
        {
            return PartialView("~/Features/ControllerFeature/Partial.cshtml");
        }

        [Route(nameof(IndexWithLayoutBySpecificName))]
        public ActionResult IndexWithLayoutBySpecificName()
        {
            return View("~/Features/ControllerFeature/Index.cshtml",
                new ViewConfig {Layout = "~/Features/ControllerFeature/_Layout.cshtml"});
        }
    }
}