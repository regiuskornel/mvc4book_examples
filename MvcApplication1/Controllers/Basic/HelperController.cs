using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;

namespace MvcApplication1.Controllers
{
    public partial class HelperController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        #region Html.Raw
        public ActionResult Hraw()
        {
            return View();
        }
        #endregion

        #region ActionLink, RouteLink
        public ActionResult Hlink()
        {
            return View();
        }

        public ActionResult Hlink2(string honnan)
        {
            ViewBag.Honnan = honnan;
            return View();
        }
        #endregion

        #region Form és Textbox

        public ActionResult Hform()
        {
            return View();
        }


        public ActionResult Hinput()
        {
            return View(ActionDemoModel.GetModell(1));
        }

        [HttpPost]
        public ActionResult Hinput(int? id, FormCollection fcoll, string FullNameOrig)
        {
            if (!id.HasValue) return RedirectToAction("Hinput");

            var model = ActionDemoModel.GetModell(id.Value);
            if (TryUpdateModel(model))
            {
                return View(model);
            }
            return View(ActionDemoModel.GetModell(id.Value));
        }

        [HttpPost]
        public ActionResult Hinput2(int? id, string FullName, String Address)
        {
            if (!id.HasValue) return RedirectToAction("Hinput");

            var model = ActionDemoModel.GetModell(id.Value);
            model.FullName = FullName;
            model.Address = Address;
            return View("Hinput", model);
        }

        public ActionResult Hlabel()
        {
            return View(ActionDemoModel.GetModell(1));
        }
        #endregion

        #region Listák és combók
        public ActionResult Hlist()
        {
            return View(ActionDemoModel.GetModell(1));
        }

        [HttpGet]
        public ActionResult Hcombo(int? id)
        {
            return View(ActionDemoModel.GetModell(id ?? 1));
        }

        [HttpPost]
        public ActionResult Hcombo(ActionDemoModel inputmodel)
        {
            var model = ActionDemoModel.GetModell(inputmodel.Id);
            if (inputmodel.KeyPurchase.Id != model.KeyPurchase.Id)
            {
                model.KeyPurchase = model.PurchasesList
                    .FirstOrDefault(v => v.Id == inputmodel.KeyPurchase.Id);
            }
            return View("Hcombo", model);
        }

        [HttpPost]
        public ActionResult Hlist(ActionDemoModel inputmodel)
        {
            var model = ActionDemoModel.GetModell(inputmodel.Id);
            model.KeyPurchaseIds = inputmodel.KeyPurchaseIds;
            //return RedirectToAction("Hcombo", new { id = model.Id });
            return View("Hcombo", model);
        }

        [HttpGet]
        public ActionResult Hcombo1(int? id)
        {
            var model = ActionDemoModel.GetModell(id ?? 1);
            ViewData["KeyPurchase.Id"] = new List<SelectListItem>()
                {
                    new SelectListItem() {Selected = false,Text = "Alma",Value = "1"},
                    new SelectListItem() {Selected = true,Text = "Körte",Value = "2"},
                    new SelectListItem() {Selected = false,Text = "Szilva",Value = "3"},
                };

            return View(model);
        }
        #endregion

        #region Checkbox és rádió

        [HttpGet]
        public ActionResult Hcheck(int? id)
        {
            return View(ActionDemoModel.GetModell(id ?? 1));
        }

        [HttpPost]
        public ActionResult Hcheck(ActionDemoModel inputmodel, FormCollection formcoll)
        {
            var model = ActionDemoModel.GetModell(inputmodel.Id);
            model.VIP = inputmodel.VIP;
            if (inputmodel.KeyPurchase.Id != model.KeyPurchase.Id)
            {
                model.KeyPurchase = model.PurchasesList
                    .FirstOrDefault(v => v.Id == inputmodel.KeyPurchase.Id);
            }

            return View(model);
        }
        #endregion

        #region Validáció kijelzése
        [HttpGet]
        public ActionResult Hvalid(int? id)
        {
            return View(ValidationDemoModel.GetModell(id ?? 1));
        }

        [HttpPost]
        public ActionResult Hvalid(int? id, FormCollection coll)
        {
            if (!id.HasValue) return RedirectToAction("Hvalid");
            bool elotte = ModelState.IsValid;

            var model = ValidationDemoModel.GetModell(id.Value);
            if (this.TryUpdateModel(model))
            {
                return View("Hvalid", model);
            }
            bool utana = ModelState.IsValid;

            return View(model);
        }

        [HttpGet]
        public ActionResult Hvalid2(int? id)
        {
            return View("Hvalid", ValidationDemoModel.GetModell(id ?? 1));
        }

        [HttpPost]
        public ActionResult Hvalid2(ValidationDemoModel inputmodel)
        {
            if (ModelState.IsValid && inputmodel.Id > 0)
            {
                var model = ValidationDemoModel.GetModell(inputmodel.Id);
                model.Address = inputmodel.Address;
                model.FullName = inputmodel.FullName;
                return View("Hvalid", model);
            }

            return View("Hvalid", inputmodel);
        }

        [HttpGet]
        public ActionResult Hvalid3(int? id)
        {
            return View("Hvalid", ValidationDemoModel.GetModell(id ?? 1));
        }

        [HttpPost]
        public ActionResult Hvalid3(int? id, string FullName, string Address)
        {
            if (!id.HasValue) return RedirectToAction("Hvalid");
            var model = ValidationDemoModel.GetModell(id.Value);
            bool elotte = ModelState.IsValid;
            //model.Email = null; //Hogy a modell szintű customValidation attributum működjön.
            if (TryUpdateModel(model))
            {
                model.Address = Address;
                model.FullName = FullName;
                return View("Hvalid", model);
            }
            bool utana = ModelState.IsValid;
            return View("Hvalid", model);
        }

        #endregion

public ActionResult HnameCollision(int? id, string Name, string name, string NAME)
{

    var model = new WrongModel
        {
            Id = 1,
            Name = "Tanuló 1",
            NAME = "Tanuló 2",
            name = "Tanuló 3",
            Checked = true,
            Disabled = true,
            Form = "HnameCollision",
            Value = "text"
        };
    return View(model);
}

    }

    public partial class HelperController
    {
public ActionResult HHtml5Textbox()
{
    var model = TemplateDemoModel.GetModell(1);
    model.FullName = string.Empty;
    return View();
}
    }
}
