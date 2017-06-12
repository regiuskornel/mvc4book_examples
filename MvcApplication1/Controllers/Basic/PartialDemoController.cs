using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class PartialDemoController : Controller
    {
        public ActionResult Index()
        {
            //Azonos tartalom. Csak property kivezetés.
            var session = this.ControllerContext.HttpContext.Session;
            var session2 = this.Session;

            //ViewData tartalma:
            ViewData["Szemelynev"] = "J. Gipsz";
            ViewData["Címe"] = "9999 Salátahegye 1.";

            //ViewBag tartalma
            ViewBag.Telefonszama = "+99 99-999-999";
            ViewBag.EmailCime = "kukac@kukac.kc";

            TempData["Tempadat"] = "Elérhető!";
            return View();
        }

        public ActionResult IndexRedir()
        {
            TempData["Tempadat"] = "IndexRedir Elérhető!";
            return RedirectToAction("DemoTempData");
        }

        public ActionResult DemoTempData()
        {
            TempData.Keep("Tempadat");
            return View();
        }

        public ActionResult DemoPartial()
        {
            return PartialView("/Views/PartialDemo/DemoPartial.cshtml");
        }

        //Első szintű action
        [ChildActionOnly]
        public ActionResult ChildAction()
        {
            ViewData["Szemelynev"] = "St. II. Gipsz";
            ViewData["Címe"] = "1111 Paradicsomvölgy 2.";
            return PartialView();
        }

        //Második szintű action
        [ChildActionOnly]
        public ActionResult SecondLevelChildAction()
        {
            ViewData["Szemelynev"] = "Mr. III. Gipsz";
            ViewData["Címe"] = "2222 Káposztafelföld 2.";
            return PartialView();
        }

    }
}
