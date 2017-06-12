using System;
using System.Web.Mvc;
using System.Web.UI;
using MvcApplication1.Models;
using System.Web.Caching;

namespace MvcApplication1.Controllers
{
    public class CacheDemoController : Controller
    {
        public ActionResult ClearCache()
        {
            //Normál, éles alkalmazásnál ilyet ne csináljunk:
            System.Web.HttpRuntime.UnloadAppDomain();
            return Content("Cache törölve. Frissítsd az oldalt.");
        }

        public ActionResult Index(int? id)
        {
            return View();
        }

        [HttpGet]
        [OutputCache(Duration = 20, VaryByParam = "none")]
        public ActionResult CacheTest(int? id)
        {
            return View(CacheDemoModel.GetModell(id ?? 1));
        }

        [HttpPost]
        [ActionName("CacheTest")]
        public ActionResult CacheTestPost(int? id)
        {
            if (!id.HasValue) return RedirectToAction("Index");
            var model = CacheDemoModel.GetModell(id.Value);
            TryUpdateModel(model);
            return View(model);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult CacheTestParent(int? id)
        {
            return View();
        }

        //[OutputCache(Duration = 10, VaryByParam = "Id")] 
        [OutputCache(Duration = 10)] //Nincs szükség a VaryByParam = "Id"-re mert action paraméter
        //[OutputCache(Duration = 10, VaryByParam = "none")] //Ebben az esetben nem jó működés, mert nem veszi figyelembe az Id-t
        public ActionResult CacheTestChild1(int? id)
        {
            return PartialView("CacheTestChild", CacheDemoModel.GetModell(id ?? 1));
        }


        [HttpGet]
        [OutputCache(Duration = 120, VaryByHeader = "User-Agent")]
        public ActionResult CacheTestHeader(int? id)
        {
            return View(CacheDemoModel.GetModell(id ?? 1));
        }

        [HttpGet]
        [OutputCache(Duration = 120, VaryByCustom = "minorversion")]
        public ActionResult CacheTestCustom(int? id)
        {
            return View("CacheTestHeader", CacheDemoModel.GetModell(id ?? 1));
        }

        [HttpGet]
        [OutputCache(Duration = 120, VaryByParam = "none", Location = OutputCacheLocation.None)]
        public ActionResult CacheTestLocation(int? id)
        {
            return View("CacheTestHeader", CacheDemoModel.GetModell(id ?? 1));
        }

        [HttpGet]
        [OutputCache(CacheProfile = "OtPercVaryById")]
        public ActionResult CacheTestProfile(int? id)
        {
            return View("CacheTestHeader", CacheDemoModel.GetModell(id ?? 1));
        }


        [HttpGet]
        [OutputCache(Duration = 15)]
        public ActionResult CacheTestParentSlow()
        {
            return View();
        }

        [OutputCache(Duration = 5)]
        public ActionResult CacheTestChildFast()
        {
            return PartialView("CacheTestChildFast");
        }

    }
}
