using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace MvcApplication1.Controllers
{
    public class MobileTestController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Other()
        {

            return View();
        }

        private const string ChromeDesktop = @"Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/28.0.1500.52 Safari/537.36";
        private const string OperaMobile = @"Opera/9.80 (Android 2.3.7; Linux; Opera Mobi/35779; U; hu) Presto/2.10.254 Version/12.00";
        private const string IpadTablet = "Mozilla/5.0 (iPhone; CPU iPhone OS 6_0 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A403 Safari/8536.25";
        private const string AndroidPhone = "Mozilla/5.0 (Linux; U; Android 4.0.3; hu-hu; LG-P700 Build/IML74K) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Mobile Safari/534.30";

        public RedirectResult ChangeBrowserMode(bool mobile, string returnUrl)
        {
            if (this.Request.Browser.IsMobileDevice == mobile)
                this.HttpContext.ClearOverriddenBrowser();
            else
            {
                this.HttpContext.SetOverriddenBrowser(mobile ? BrowserOverride.Mobile : BrowserOverride.Desktop);
                //Más User-Agent-ek használata:
                //this.HttpContext.SetOverriddenBrowser(mobile ? OperaMobile : ChromeDesktop);
            }
            return this.Redirect(returnUrl);
        }
    }

}
