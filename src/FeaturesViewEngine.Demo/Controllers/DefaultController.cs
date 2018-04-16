using System.Web.Mvc;

namespace FeaturesViewEngine.Demo.Controllers
{
    [RoutePrefix("Default")]
    public class DefaultController : Controller
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
            return View("~/Views/Default/Index.cshtml");
        }

        [Route(nameof(IndexWithLayoutByName))]
        public ActionResult IndexWithLayoutByName()
        {
            return View("Index", "_Layout");
        }

        [Route(nameof(IndexWithLayoutBySpecificName))]
        public ActionResult IndexWithLayoutBySpecificName()
        {
            return View("Index", "~/Views/Shared/_Layout.cshtml");
        }

        [Route(nameof(IndexWithLayoutWithPartialByName))]
        public ActionResult IndexWithLayoutWithPartialByName()
        {
            return View("IndexWithPartialByName", "_Layout");
        }

        [Route(nameof(IndexWithLayoutWithPartialBySpecificName))]
        public ActionResult IndexWithLayoutWithPartialBySpecificName()
        {
            return View("IndexWithPartialBySpecificName", "_Layout");
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
            return PartialView("~/Views/Shared/Partial.cshtml");
        }
    }
}