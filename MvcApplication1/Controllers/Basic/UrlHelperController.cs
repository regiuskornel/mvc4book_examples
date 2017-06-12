using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class UrlHelperController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AnotherAction()
        {
            return Content("Belső action oldala");
        }
    }
}
