using System;
using System.Web.Mvc;
using MvcApplication1.Models;

namespace MvcApplication1.Controllers
{
    public class ValidationsController : Controller
    {
        public ActionResult Index(int? id)
        {
            return View();
        }

        #region ModelState
        [HttpGet]
        public ActionResult ModelStateTest(int? id)
        {
            return View(ValidationMaxModel.GetModell(id ?? 1));
        }

        [HttpPost]
        [ActionName("ModelStateTest")]
        public ActionResult ModelStateTestPost(int? id, ValidationMaxModel inputModel)
        {
            bool isValid;
            if (!id.HasValue) return RedirectToAction("Index");

            isValid = this.ModelState.IsValid; // = false

            this.ModelState.Clear();

            var model = ValidationMaxModel.GetModell(id.Value);
            inputModel.LastPurchaseDate = model.LastPurchaseDate;

            //Exception-t generáló változat: this.ValidateModel(inputModel);
            if (this.TryValidateModel(inputModel))
            {
                //Igen mostmár valid.
                isValid = this.ModelState.IsValid; // = true
            }

            if (this.TryUpdateModel(model))
            {
                return RedirectToAction("ModelStateTest");
            }


            return View(model);
        }
        #endregion

        #region Validációs üzenetek
        [HttpGet]
        public ActionResult ShowValid(int? id)
        {
            return View(ValidationMaxModel.GetModell(id ?? 1));
        }

        [HttpPost]
        [ActionName("ShowValid")]
        public ActionResult ShowValidPost(int? id, ValidationMaxModel inputModel)
        {
            if (!id.HasValue) return RedirectToAction("Index");

            ModelState.AddModelError("FullName", "Manuális hibaüzenet a Felhasználó névhez");
            ModelState.AddModelError("", "Modell szintű hibaüzenet");
            ModelState.AddModelError("", new InvalidOperationException("Modell szintű Exception"));

            return View(inputModel);
        }
        #endregion

        #region IValidatableObject
        [HttpGet]
        public ActionResult IValidatableObjectTest()
        {
            return View(ValidationMaxIVOModel.GetModell(1));
        }

        [HttpPost]
        [ActionName("IValidatableObjectTest")]
        public ActionResult IValidatableObjectTestPost(int? id)
        {
            if (!id.HasValue) return RedirectToAction("Index");
            var model = ValidationMaxIVOModel.GetModell(id.Value);
            this.TryUpdateModel(model);
            return View(model);
        }
        #endregion

        #region Saját, csak szerver oldali validácios attribútum
        [HttpGet]
        public ActionResult RelativeDateTest()
        {
            return View(ValidationMaxRelativeModel.GetModell(1));
        }

        [HttpPost]
        [ActionName("RelativeDateTest")]
        public ActionResult RelativeDateTestPost(int? id)
        {
            if (!id.HasValue) return RedirectToAction("Index");
            var model = ValidationMaxRelativeModel.GetModell(id.Value);
            this.TryUpdateModel(model);
            return View(model);
        }

        #endregion

        #region Kliens oldali relatív dátum validáció
        [HttpGet]
        public ActionResult RelativeDateClient()
        {
            return View(ValidationMaxRelativeModel.GetModell(1));
        }

        [HttpPost]
        [ActionName("RelativeDateClient")]
        public ActionResult RelativeDateClientPost(int? id)
        {
            if (!id.HasValue) return RedirectToAction("Index");
            var model = ValidationMaxRelativeModel.GetModell(id.Value);
            this.TryUpdateModel(model);
            return View(model);
        }
        #endregion

        #region Kompoziv validáció Remote
        [HttpGet]
        public ActionResult RemoteName(int? id)
        {
            for (int i = 0; i < 5; i++)
                ValidationMaxRemoteModel.GetModell(i);
            return View(ValidationMaxRemoteModel.GetModell(id ?? 1));
        }

        [HttpPost]
        [ActionName("RemoteName")]
        public ActionResult RemoteNamePost(int? id, FormCollection coll)
        {
            if (!id.HasValue) return RedirectToAction("Index");
            var model = ValidationMaxRemoteModel.GetModell(id.Value);
            bool result = this.TryUpdateModel(model);
            return View(model);
        }

        [HttpPost]
        public JsonResult RemoteNameValidator(string FullName, string Address, int id)
        {
            System.Threading.Thread.Sleep(1000);
            if (ValidationMaxRemoteModel.IsNameReserved(FullName))
            {
                //1. változat az attribútum hibaüzenete
                return Json(false);
                //2. változat szerver oldali hibaüzenet
                return Json("Sajnos a név már foglalt (controller message)");
            }
            return Json(true);
            return Json("true");
        }

        #endregion

    }
}
