using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstMVCApp.Controllers
{
    public class FirstController : Controller
    {
        //
        // GET: /First/
        public ActionResult Index()
        {
            var listmodel = new List<Models.FirstModel>();
listmodel.Add(new Models.FirstModel()
{
    Id = 1,
    FullName = "Karcsi",
    Address = "Hosszú utca 1."
});
listmodel.Add(new Models.FirstModel()
{
    Id = 2,
    FullName = "Pista",
    Address = "Hosszú utca 3."
});
            //Adjuk át a modellt a View-nak
            return View(listmodel);
        }

        //
        // GET: /First/Details/5
        public ActionResult Details(int id)
        {
            var model = new Models.FirstModel()
            {
                Id = 1,
                FullName = "Karcsi",
                Address = "Hosszú utca 1."
            };

            return View(model);
        }

        //
        // GET: /First/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /First/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /First/Edit/5
public ActionResult Edit(int id)
{
    var model = new Models.FirstModel()
    {
        Id = 1,
        FullName = "Karcsi",
        Address = "Hosszú utca 1."
    };
    return View(model);
}

        //
        // POST: /First/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Models.FirstModel model)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /First/Delete/5
        public ActionResult Delete(int id)
        {

            return View();
        }

        //
        // POST: /First/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
