using System.Web.Mvc;

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

        [Route(nameof(IndexWithLayoutByName))]
        public ActionResult IndexWithLayoutByName()
        {
            return View("Index", "_Layout");
        }

        [Route(nameof(IndexWithLayoutBySpecificName))]
        public ActionResult IndexWithLayoutBySpecificName()
        {
            return View("Index.cshtml", "~/Views/Shared/_Layout.cshtml");
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
    }
}