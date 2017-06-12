
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcApplication1.Models;

namespace MvcApplication1.Controllers
{
    [Services.SillyLoggerActionFilter()] //Minden Action-ön működni fog.
    public class ActionDemoController : Controller
    {
        public ActionDemoController()
        {
            //Környezeti beállítások. Adatbázis/WCF kapcsolat. 
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        #region Action és kimenete, adatok
        //
        // GET: /ActionDemo/
        public ActionResult Index()
        {
            //1. változat
            return View();

            var model = new List<int>();

            //2. változat
            return View(model);

            //3. változat
            return View("Index", model);

            //4. változat
            return View("Index", "_Layout", model);

            return new ViewResult() { };
        }

        public ActionResult Detail(int id)
        {
            return View(ActionDemoModel.GetModell(id));
        }

        //public ActionResult Details(int id = 0)
        //{
        //    ViewData["kategoria"] = Request["category"];
        //    ViewData["formátum"] = Request["format"];
        //    return View(id);
        //}

        //
        // GET: /ActionDemo/Details/5
        public ActionResult Details(int id = 0, string category = "nincs", string format = "nincs")
        {
            ViewBag.kategoria = "WBag";
            bool egyforma1 = ViewBag.kategoria == ViewData["kategoria"];

            ViewData["formátum"] = "Forma";
            bool egyforma2 = ViewBag.formátum == ViewData["formátum"];

            ViewBag.referenciaTipus = new EmptyResult();
            bool egyforma3 = ViewBag.referenciaTipus == ViewData["referenciaTipus"];

            ViewData["kategoria"] = category;
            ViewData["formátum"] = format;
            return View(id);
        }

        [ChildActionOnly]
        public ActionResult DetailPartial()
        {
            ViewBag.kategoria = "Ez egy alszekvencia";
            return View();

            var model = new List<int>();
            return PartialView("DetailPartial", model);

            return new PartialViewResult() { };
        }

        //
        // GET: /ActionDemo/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ActionDemo/Create
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

        // GET: /ActionDemo/Edit/5
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Put | HttpVerbs.Head)]
        //[ActionName("Szerkesztes")]
        public ActionResult Edit(int id)
        {
            return View(ActionDemoModel.GetModell(id));
        }

        // POST: /ActionDemo/Edit/5
        [HttpPost]
        //[ValidateInput(false)] //Potenciális veszély !
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                ActionDemoModel fpdm = ActionDemoModel.GetModell(id);
                if (TryUpdateModel(fpdm))
                    return RedirectToAction("Index");
                return View(fpdm);
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /ActionDemo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ActionDemo/Delete/5

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

        ////Azonos nevek, csak a HTTP method neve különböztet meg
        //[HttpGet]
        //public ActionResult Edit(int id) { }

        //[HttpPost]
        //public ActionResult Edit(int id, Model model) { }

        //[HttpDelete]
        //public ActionResult Edit(int id) { }

        ////Értelmes action nevek
        //[HttpGet]
        //public ActionResult EditTermekek(int id) { }

        //[HttpPost]
        //public ActionResult EditTermekek(int id, Model model) { }

        //[HttpDelete]
        //public ActionResult DeleteTermekek(int id) { }


        #endregion

        #region ActionResult típusok
        public ActionResult GetContentResult()
        {
            var txt = "Ez kerül ki a kimenetre.";

            //1. változat
            return Content(txt);

            //2. változat
            return new ContentResult() { Content = txt };

            //3. változat
            Response.Write(txt);
            return null;
        }

        public ActionResult GetEmptyResult()
        {
            //1. változat
            return new EmptyResult();

            //2. változat
            return null;
        }

        public ActionResult GetFileContentResult()
        {
            byte[] filedata = System.IO.File.ReadAllBytes(Server.MapPath("~/Images/heroAccent.png"));
            FileContentResult filecontent = new FileContentResult(filedata, "image/PNG");
            //Ha megadjuk 'download to file' lesz és nem megjelenítés a böngészőben.
            filecontent.FileDownloadName = "heroAccent.png";
            return filecontent;
        }

        public ActionResult GetFilePathResult()
        {
            FilePathResult filePathResult = new FilePathResult(Server.MapPath("~/Images/heroAccent.png"), "image/PNG");
            //filePathResult.FileDownloadName = "heroAccent.png";
            return filePathResult;
        }

        public ActionResult GetFileStreamResult()
        {
            byte[] filedata = System.IO.File.ReadAllBytes(Server.MapPath("~/Images/heroAccent.png"));
            System.IO.MemoryStream ms = new MemoryStream(filedata);
            FileStreamResult fileStreamResult = new FileStreamResult(ms, "image/PNG");
            //fileStreamResult.FileDownloadName = "heroAccent.png";
            return fileStreamResult;
        }

        public ActionResult GetJavaScriptResult()
        {
            var jr = new JavaScriptResult();
            jr.Script = "messageYes='Igen'; messageNo='Nem';";
            return jr;
        }

        public ActionResult GetHttpUnauthorizedResult()
        {
            return new HttpUnauthorizedResult();
        }

        public ActionResult GetJsonResult()
        {
            var data = new JsonModelClass() { Id = 1, FullName = "Pista" };
            return new JsonResult()
            {
                Data = data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

            //2. variáció.
            return Json(data);
        }

        public class JsonModelClass
        {
            public int Id { get; set; }
            public string FullName { get; set; }
        }
        #endregion

        #region Redirekciók
        public ActionResult GetRedirectResult()
        {
            return new RedirectResult("http://index.hu", true);
        }

        public ActionResult GetRelativeRedirectResult()
        {
            return new RedirectResult("GetContentResult", false);
        }

        public ActionResult GetRedirectToActon()
        {
            //1. változat
            return RedirectToAction("GetContentResult", "ActionDemo");

            //2. változat
            return new RedirectToRouteResult("Default",
                new RouteValueDictionary(new { action = "GetContentResult", controller = "ActionDemo" }));
        }
        #endregion

        #region IAuthorizationFilter

        [RequireHttps]
        public ActionResult CsakHttps()
        {
            return Content("Ezt ne szaglászd!");
        }

        #endregion

        #region IActionFilter, Logging
        public ActionResult BusinessCritical()
        {
            Services.SillyLogger.EnterMethod("BusinessCritical");
            //..
            //Itt sok kód lesz
            //..
            Services.SillyLogger.Store("Ez egy szöveg, amit naplóztunk");
            //..
            //Itt sok kód lesz
            //..
            ViewResult result = View();
            Services.SillyLogger.Store("A result: " + result.View);
            Services.SillyLogger.ExitMethod("BusinessCritical");
            return result;
        }

        [Services.SillyLoggerActionFilter]
        public ActionResult BusinessCriticalA()
        {
            //..
            //Itt sok kód lesz
            //..
            Services.SillyLogger.Store("Ez egy szöveg, amit naplóztunk");
            //..
            //Itt sok kód lesz
            //..
            return View("BusinessCritical");
        }
        #endregion

        #region IExceptionFilter
        //[HandleError(View = "HandleException", ExceptionType = typeof(InvalidOperationException))]
        public ActionResult MakeException()
        {
            throw new InvalidOperationException("Opps. Hiba történt.");
        }

        public ActionResult MakeGeneralException()
        {
            throw new Exception("Opps. Generális hiba történt.");
        }

        public ActionResult HandleException(HandleErrorInfo info)
        {
            return View(info);
        }

        #endregion

    }
}
