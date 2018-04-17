using System.Web.Mvc;
using FeaturesViewEngine.Demo.Models;

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

        [Route(nameof(IndexWithPartialByName))]
        public ActionResult IndexWithPartialByName()
        {
            return View("~/Views/Default/Index.cshtml", new ViewConfig {Partial = "Partial"});
        }

        [Route(nameof(IndexWithPartialBySpecificName))]
        public ActionResult IndexWithPartialBySpecificName()
        {
            return View("~/Views/Default/Index.cshtml", new ViewConfig {Partial = "~/Views/Shared/Partial.cshtml"});
        }

        [Route(nameof(IndexWithLayoutBySpecificName))]
        public ActionResult IndexWithLayoutBySpecificName()
        {
            return View("~/Views/Default/Index.cshtml", new ViewConfig {Layout = "~/Views/Shared/_Layout.cshtml"});
        }
    }
}