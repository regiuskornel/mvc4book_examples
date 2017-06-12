using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MvcApplication1.Infrastructure;
using MvcApplication1.Models;

namespace MvcApplication1.Controllers
{
    public class AjaxJSONDemoController : Controller
    {
        private void NagyAdatLekeres()
        {
            System.Threading.Thread.Sleep(2000);
        }

        public ActionResult Index()
        {
            var model = TemplateDemoModel.GetList(1000).Take(10);
            return View(model.ToList());
        }

        [HttpPost]
        public ActionResult IndexListPartial(string findName, string findAddress)
        {
            this.NagyAdatLekeres();
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

        [HttpGet]
        public ActionResult AutoComplete(string term, string field)
        {
            if (string.IsNullOrEmpty(term)) return Json(null);
            var query = TemplateDemoModel.GetList();
            IEnumerable<string> response = null;
            switch (field)
            {
                case "findName":
                    response = query.Where(l => l.FullName.Contains(term)).Select(l => l.FullName);
                    break;

                case "findAddress":
                    response = query.Where(l => l.Address.Contains(term)).Select(l => l.Address);
                    break;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
            return new JsonResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public ActionResult AutoCompletePost(string term, string field)
        {
            if (string.IsNullOrEmpty(term)) return Json(null);
            var query = TemplateDemoModel.GetList();
            IEnumerable<string> response = null;
            switch (field)
            {
                case "nev":
                    response = query.Where(l => l.FullName.Contains(term)).Select(l => l.FullName);
                    break;

                case "cim":
                    response = query.Where(l => l.Address.Contains(term)).Select(l => l.Address);
                    break;
            }


            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public ActionResult ServerData()
        {
            return View();
        }

        public ActionResult GetServerData()
        {
            var sorositando = new
                {
                    Message = "Szerver idő",
                    CurrentTime = DateTime.Now,
                    EniacFinished = new DateTime(1946, 2, 14),
                };

            return Json(sorositando, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetServerData2()
        {
            var sorositando = new
            {
                Message = "Szerver idő",
                CurrentTime = DateTime.Now,
                EniacFinished = new DateTime(1946, 2, 14),
            };

            return new MyJsonResult(sorositando);

            /* Bedrótozott sorosítás:
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new[] { new Infrastructure.DateTimeJsonConverter() });

            var serialized = serializer.Serialize(sorositando);
            
            HttpResponseBase response = this.HttpContext.Response;
            response.ContentType = "application/json";
            response.Write(serialized);
            response.End();
            return null;
            */
        }

        [AjaxPost]
        public ActionResult SetServerData(MyJsonModell modell)
        {
            //var serializer = new JavaScriptSerializer();
            //serializer.RegisterConverters(new[] { new DateTimeJsonConverter() });

            //return new MyJsonResult(deserialized);

            modell.Id++;
            modell.ClientUTCTime = modell.ClientUTCTime.AddDays(1200);
            return new MyJsonResult(modell);
        }

    }

    public class MyJsonModell
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime ClientUTCTime { get; set; }

        public MyJsonModell Internal { get; set; }
    }


    public class AjaxPostAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            HttpRequestBase request = filterContext.RequestContext.HttpContext.Request;
            string actionname = filterContext.RouteData.GetRequiredString("action");
            if (request.HttpMethod.ToLowerInvariant() != "post" || !request.IsAjaxRequest())
            {
                throw new InvalidOperationException(actionname + " csak AJAX POST requesttel hívható!");
            }
        }
    }

    /// <summary>
    /// Egy másik variáció az ajax+post kikényszerítésére
    /// </summary>
    public class AjaxPost1Attribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(ControllerContext context, System.Reflection.MethodInfo methodInfo)
        {
            return context.RequestContext.HttpContext.Request.HttpMethod.ToLowerInvariant() == "post"
                && context.RequestContext.HttpContext.Request.IsAjaxRequest();
        }
    }


}
