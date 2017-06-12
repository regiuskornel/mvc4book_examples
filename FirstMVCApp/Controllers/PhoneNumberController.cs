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
    public class PhoneNumberController : Controller
    {
        private CardRegisterDb db = new CardRegisterDb();

        //
        // GET: /PhoneNumber/

        public ActionResult Index()
        {
            var phonenumbers = db.PhoneNumbers.Include(p => p.CardRegister);
            return View(phonenumbers.ToList());
        }

        //
        // GET: /PhoneNumber/Details/5

        public ActionResult Details(int id = 0)
        {
            PhoneNumber phonenumber = db.PhoneNumbers.Find(id);
            if (phonenumber == null)
            {
                return HttpNotFound();
            }
            return View(phonenumber);
        }

        //
        // GET: /PhoneNumber/Create

        public ActionResult Create(int? cardid)
        {
            ViewBag.CardRegisterId = new SelectList(db.CardRegisters, "Id", "FullName", cardid);
            return View(new PhoneNumber() { CardRegisterId = cardid ?? 0 });
        }

        //
        // POST: /PhoneNumber/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PhoneNumber phonenumber)
        {
            if (ModelState.IsValid)
            {
                db.PhoneNumbers.Add(phonenumber);
                db.SaveChanges();
                return RedirectToAction("Details", "CardRegister", new { id = phonenumber.CardRegisterId });
                return RedirectToAction("Index");
            }

            ViewBag.CardRegisterId = new SelectList(db.CardRegisters, "Id", "FullName", phonenumber.CardRegisterId);
            return View(phonenumber);
        }

        //
        // GET: /PhoneNumber/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PhoneNumber phonenumber = db.PhoneNumbers.Find(id);
            if (phonenumber == null)
            {
                return HttpNotFound();
            }
            ViewBag.CardRegisterId = new SelectList(db.CardRegisters, "Id", "FullName", phonenumber.CardRegisterId);
            return View(phonenumber);
        }

        //
        // POST: /PhoneNumber/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PhoneNumber phonenumber)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phonenumber).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "CardRegister", new { id = phonenumber.CardRegisterId });
                return RedirectToAction("Index");
            }
            ViewBag.CardRegisterId = new SelectList(db.CardRegisters, "Id", "FullName", phonenumber.CardRegisterId);
            return View(phonenumber);
        }

        //
        // GET: /PhoneNumber/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PhoneNumber phonenumber = db.PhoneNumbers.Find(id);
            if (phonenumber == null)
            {
                return HttpNotFound();
            }
            return View(phonenumber);
        }

        //
        // POST: /PhoneNumber/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhoneNumber phonenumber = db.PhoneNumbers.Find(id);
            db.PhoneNumbers.Remove(phonenumber);
            db.SaveChanges();
            return RedirectToAction("Details", "CardRegister", new { id = phonenumber.CardRegisterId });
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}