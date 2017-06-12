using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;

namespace MvcApplication1.Controllers.Performance
{
    public class DataCacheController : Controller
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

        //[ModelDataCacheFilter(null, "id")]
        [ModelDataCacheFilter("CacheDemoModel-forTest", "id,category")]
        public ActionResult CacheTest(int? id)
        {
            if (ViewData.Model == null) //nem volt a cache-ben
                ViewData.Model = CacheDemoModel.GetModell(id ?? 1);
            return View();
        }

        [HttpPost]
        //[ModelDataCacheFilter(null, "id")]
        [ModelDataCacheFilter("CacheDemoModel-forTest", "id")]
        [ActionName("CacheTest")]
        public ActionResult CacheTestPost(int? id)
        {
            if (!id.HasValue) return RedirectToAction("Index");
            CacheDemoModel originalModel = ViewData["originalModel"] as CacheDemoModel;
            if (originalModel == null)
                originalModel = CacheDemoModel.GetModell(id.Value);

            if (TryUpdateModel<CacheDemoModel>(originalModel))
                return RedirectToAction("Index");

            return View(originalModel);
        }

        //public ActionResult TipicalSearchAction(string category, string name)
        //{
        //    var model = dataService.GetModelByCategory_Name(category, name);
        //    return View(model);
        //}

    }
}
