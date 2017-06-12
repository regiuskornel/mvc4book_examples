using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcApplication1.Models;

namespace MvcApplication1.Controllers.Multilanguage
{
public class MultilanguageController : Controller
{
    public ActionResult Index()
    {
        return View(ValidationMaxModel.GetModell(1));
    }

    public ActionResult ChangeLang(string langcode)
    {
        var validlangcode = LanguageModel.GetAvailableOrFallback(langcode);
        Response.Cookies.Remove("lang");
        var langcookie = new HttpCookie("lang", validlangcode)
            {
                Expires = DateTime.Today.AddYears(1)
            };
        Response.Cookies.Add(langcookie);
        if (Request.UrlReferrer != null)
        {
            this.HttpContext.RewritePath(Request.UrlReferrer.LocalPath);
            var routeData = RouteTable.Routes.GetRouteData(this.HttpContext);
            if (routeData != null && routeData.Values.Count != 0)
            {
                routeData.Values["lang"] = validlangcode;
                return RedirectToRoute(routeData.Values);
            }
        }

        return RedirectToRoute(new { lang = validlangcode, controller = "Home", action = "Index" });
    }
}
}
