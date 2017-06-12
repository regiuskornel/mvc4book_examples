using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;

namespace MvcApplication1.Controllers
{
    public class AjaxDemoController : Controller
    {

        private void LongTimeDBAccess()
        {
            System.Threading.Thread.Sleep(2000);
        }

        public ActionResult Index()
        {
            var model = TemplateDemoModel.GetList(100).Take(10);
            return View(model.ToList());
        }

        [HttpPost]
        public ActionResult IndexListPartial(string findName, string findAddress)
        {
            this.LongTimeDBAccess();
            bool filtered = false;
            var query = TemplateDemoModel.GetList();
            if (!string.IsNullOrEmpty(findName))
            {
                query = query.Where(l => l.FullName.Contains(findName));
                filtered = true;
            }

            if (!string.IsNullOrEmpty(findAddress))
            {
                query = query.Where(l => l.Address.Contains(findAddress));
                filtered = true;
            }

            if (filtered) //volt szűrési feltétel
            {
                var list = query.ToList();
                if (list.Count > 0) //van keresési eredmény.
                    return PartialView(list);

                this.SendNotFount();
                return null;
            }
            return PartialView(TemplateDemoModel.GetList().Take(10).ToList());
        }

        private void SendNotFount()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            //Response.Status = "Nincs találat";
            Response.Write("Nincs találat");
            Response.End();
        }

        [HttpPost]
        public ActionResult DetailListPartial(int id)
        {
            var item = TemplateDemoModel.GetModell(id);
            return PartialView(item.PurchasesList);
        }

        public ActionResult Details(int id)
        {
            this.LongTimeDBAccess();
            return PartialView(TemplateDemoModel.GetModell(id));
        }

        public ActionResult Edit(int id)
        {
            this.LongTimeDBAccess();
            return PartialView(TemplateDemoModel.GetModell(id));
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection coll)
        {
            var model = TemplateDemoModel.GetModell(id);
            if (this.TryUpdateModel(model))
                return new EmptyResult();

            return PartialView(model);
        }

    }
}
