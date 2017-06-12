using System;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers.Securities
{

    public class SecurityController : Controller
    {
        public SecurityController()
        {
            //System.Web.Helpers.AntiForgeryConfig.AdditionalDataProvider =
            //    new MyAntiForgeryAdditionalDataProvider();
            //System.Web.Helpers.AntiForgeryConfig.AdditionalDataProvider =
            //    new OnlyLastAFDataProvider();
            System.Web.Helpers.AntiForgeryConfig.AdditionalDataProvider =
                new HiddenAFDataProvider();

            System.Web.Helpers.AntiForgeryConfig.CookieName = "_lastproduct";
            //System.Web.Helpers.AntiForgeryConfig.RequireSsl = true;
        }

        public ActionResult Index()
        {
            this.RouteData.Values.Add("hiddenid", 12);
            return View(SecurityModel.CreteNewModel());
        }

        [RequireHttps]
        public ActionResult HttpsOnly()
        {
            return null;
        }

        [ChildActionOnly]
        public ActionResult OnlyChildAction()
        {
            return null;
        }

        [BrowserOnly("Chrome")]
        public ActionResult ChromeBrowserOnly()
        {
            return Content("Hello Chrome!");
        }

        [HttpPost]
        public ActionResult AntiForgeryNelkul()
        {
            return Content("Nem volt ellenőrzés...");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AntiForgeryServed(int hiddenid)
        {
            return Content("Minden rendben " + hiddenid);
        }

        [HttpPost]
        public ActionResult EncodedHidden(SecurityModel model)
        {
            return Content(model.ToString());
        }

        [HttpPost]
        public ActionResult EncodedInternalHidden(SecurityStorageModel model)
        {
            return Content(model.SecurityModelInternal.ToString());
        }

        [EncryptedRouteAttribute]
        public ActionResult EncodedUrl(int Id)
        {
            return Content("Id: " + Id);
        }

        [EncryptedRouteAttribute]
        public ActionResult EncodedUrlQuery(int Id, string mokusnev)
        {
            return Content("Id: " + Id + " Név: " + mokusnev);
        }


        public ActionResult YourAreNotInRole(string rolenames)
        {
            return View("YourAreNotInRole", "_Layout", rolenames);
        }
    }

    public class BrowserOnlyAttribute : FilterAttribute, IAuthorizationFilter
    {
        private readonly string _browserName;
        public BrowserOnlyAttribute(string browserName)
        {
            this._browserName = browserName;
        }
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext == null) return;
            if (filterContext.HttpContext.Request.Browser.Browser != _browserName)
                throw new InvalidOperationException(
                    "Ez az action csak " + _browserName + " böngészővel érhető el!");
        }
    }

    public class MyAntiForgeryAdditionalDataProvider : System.Web.Helpers.IAntiForgeryAdditionalDataProvider
    {
        public string GetAdditionalData(HttpContextBase context)
        {
            return "Sós mókus";
        }

        public bool ValidateAdditionalData(HttpContextBase context, string additionalData)
        {
            return additionalData == "Sós mókus";
        }
    }

    public class OnlyLastAFDataProvider : System.Web.Helpers.IAntiForgeryAdditionalDataProvider
    {
        public string GetAdditionalData(HttpContextBase context)
        {
            object pO = context.Session["PageIdentity"];
            if (pO == null || !(pO is int))
                pO = 0;
            int pIdentity = (int)pO + 1;
            context.Session["PageIdentity"] = pIdentity;
            return pIdentity.ToString();
        }

        public bool ValidateAdditionalData(HttpContextBase context, string additionalData)
        {
            if (string.IsNullOrEmpty(additionalData)) return false;
            object pO = context.Session["PageIdentity"];
            if (pO == null || !(pO is int)) return false;
            int pIdentity = (int)pO;
            int addIdentity;
            return Int32.TryParse(additionalData, out addIdentity) && pIdentity == addIdentity;
        }
    }

    public class HiddenAFDataProvider : System.Web.Helpers.IAntiForgeryAdditionalDataProvider
    {
        public string GetAdditionalData(HttpContextBase context)
        {
            var mvchandler = context.Handler as MvcHandler;
            if (mvchandler == null ||
                !mvchandler.RequestContext.RouteData.Values.ContainsKey("hiddenid")) return String.Empty;
            return mvchandler.RequestContext.RouteData.Values["hiddenid"].ToString();
        }

        public bool ValidateAdditionalData(HttpContextBase context, string additionalData)
        {
            var mvchandler = context.Handler as MvcHandler;
            if (mvchandler == null) return false;

            mvchandler.RequestContext.RouteData.Values.Add("hiddenid", additionalData);
            return true;
        }
    }

}
