using MvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class ViewTestController : Controller
    {
        //Ennek az action-nak nincs View párja.
        public ActionResult NonexistentPage()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            return View(ActionDemoModel.GetModell(id));
        }

        public ActionResult Egyben()
        {
            return View("Egyben", "_LayoutDemoSub");
        }

        public ActionResult ViewContext()
        {
            return View(new { Alma = 1, Korte = 2 });
        }

        public ActionResult Bongeszo()
        {
            switch(Request.Browser.Browser)
            {
                case "Chrome":
                    return View("BongeszoChrome");
                case "IE":
                    return View("BongeszoIE");
                case "Firefox":
                    return View("BongeszoFireFox");
                default:
                    return View("Bongeszo");
            }
        }
    }
}
