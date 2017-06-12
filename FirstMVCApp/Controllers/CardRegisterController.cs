using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FirstMVCApp.Models;

namespace FirstMVCApp.Controllers
{
    public class CardRegisterController : Controller
    {
        private CardRegisterDb db = new CardRegisterDb();

        //
        // GET: /CardRegister/

        public ActionResult Index()
        {
            return View(db.CardRegisters.ToList());
        }

        //
        // GET: /CardRegister/Details/5

        public ActionResult Details(int id = 0)
        {
            CardRegister cardregister = db.CardRegisters.Find(id);
            if (cardregister == null)
            {
                return HttpNotFound();
            }
            return View(cardregister);
        }

        //
        // GET: /CardRegister/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /CardRegister/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CardRegister cardregister)
        {
            if (ModelState.IsValid)
            {
                db.CardRegisters.Add(cardregister);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cardregister);
        }

        //
        // GET: /CardRegister/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CardRegister cardregister = db.CardRegisters.Find(id);
            if (cardregister == null)
            {
                return HttpNotFound();
            }
            return View(cardregister);
        }

        //
        // POST: /CardRegister/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CardRegister cardregister)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cardregister).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cardregister);
        }

        //
        // GET: /CardRegister/Delete/5

        public ActionResult Delete(int id = 0)
        {
            CardRegister cardregister = db.CardRegisters.Find(id);
            if (cardregister == null)
            {
                return HttpNotFound();
            }
            return View(cardregister);
        }

        //
        // POST: /CardRegister/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CardRegister cardregister = db.CardRegisters.Find(id);
            db.CardRegisters.Remove(cardregister);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}