using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;

namespace MvcApplication1.Controllers
{
    public class TemplateDemoController : Controller
    {
        public ActionResult Index()
        {
            var t = typeof(int).Name;
            return View(TemplateDemoModel.GetList(5));
        }

        public ActionResult Details(int id)
        {
            return View(TemplateDemoModel.GetModell(id));
        }

        public ActionResult Edit(int id)
        {
            return View(TemplateDemoModel.GetModell(id));
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection coll)
        {
            var model = TemplateDemoModel.GetModell(id);
            if (this.TryUpdateModel(model))
                return RedirectToAction("Index");

            return View(model);
        }
    }
}
