using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.WebPages;
using MvcApplication1.Models;

namespace MvcApplication1
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private void Application_Start()
        {
            
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new ConciseViewEngine());

            AreaRegistration.RegisterAllAreas("Areának átadott típusmentes paraméter");

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            //Régi módszer a binder regisztrálására:
            //            ModelBinders.Binders.Add(typeof(CategoryModel), new CategoryModelBinder());

            //Új módszer a binder regisztrálására:
            //            ModelBinderProviders.BinderProviders.Add(new CategoryModelBinderProvider());

            ValueProviderFactories.Factories.Insert(0, new Controllers.Securities.EncryptedValueProviderFactory());

            /* View variánsok
            //1. iPhon vizsgálattal
            DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("Iphone")
            {
                ContextCondition = (context => context.GetOverriddenUserAgent().IndexOf
                            ("iPhone", StringComparison.OrdinalIgnoreCase) >= 0)
            });
            //2. Android vizsgálattal
            DisplayModeProvider.Instance.Modes.Insert(1, new DefaultDisplayMode("Android")
            {
                ContextCondition = (context =>
                {
                    var browser = context.GetOverriddenUserAgent();
                    return browser.IndexOf("android", StringComparison.OrdinalIgnoreCase) >= 0;
                })
            });
            //3. Chrome böngésző vizsgálattal
            DisplayModeProvider.Instance.Modes.Insert(2, new DefaultDisplayMode("Chrome")
            {
                ContextCondition = (context =>
                {
                    var browser = context.GetOverriddenBrowser();
                    return browser.Browser == "Chrome";
                })
            });
            */

            /*Saját Metadata provider html helper és attributum kapcsolatához*/
            //ModelMetadataProviders.Current = new ExtendedMetaDataProvider();
        }

        private void Session_Start()
        {
            //Started
        }

        private void Session_End()
        {
            //End
        }

        public void Application_BeginRequest(object sender, EventArgs e)
        {
            var currentContext = new HttpContextWrapper(HttpContext.Current);
            var routeData = RouteTable.Routes.GetRouteData(currentContext);
            if (routeData == null || routeData.Values.Count == 0) return;

            string usableLang;
            var languageCode = (string)routeData.Values["lang"];
            if (languageCode == null)
            {
                var langcookie = HttpContext.Current.Request.Cookies["lang"];
                languageCode = langcookie != null ?
                    langcookie.Value :
                    System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToLower();
                routeData.Values["lang"] =
                    usableLang = LanguageModel.GetAvailableOrFallback(languageCode);
            }
            else
                usableLang = LanguageModel.GetAvailableOrFallback(languageCode);

            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(usableLang);
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(usableLang);
        }



        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            switch (custom)
            {
                case "browser": //<- az Application ős is ezt csinálja
                    return context.Request.Browser.Type;
                case "minorversion":
                    return "BrowserFoVer=" + context.Request.Browser.MinorVersion;
                case "majorversion":
                    return "BrowserAlVer=" + context.Request.Browser.MajorVersion;
                case "mobiledevice":
                    return "BrowserMobile=" + context.Request.Browser.IsMobileDevice;
                default:
                    return base.GetVaryByCustomString(context, custom);
            }
        }

    }

    public class ConciseViewEngine : RazorViewEngine
    {
        public ConciseViewEngine()
        {
            this.AreaViewLocationFormats = new[] { "~/Areas/{2}/Views/{1}/{0}.cshtml",
                                                "~/Areas/{2}/Views/Shared/{0}.cshtml"};
            this.AreaMasterLocationFormats = new[] {"~/Areas/{2}/Views/{1}/{0}.cshtml",
                                                "~/Areas/{2}/Views/Shared/{0}.cshtml"};
            this.AreaPartialViewLocationFormats = new[] {"~/Areas/{2}/Views/{1}/{0}.cshtml",
                                                    "~/Areas/{2}/Views/Shared/{0}.cshtml"};
            this.ViewLocationFormats = new[] {"~/Views/{1}/{0}.cshtml",
                                        "~/Views/Shared/{0}.cshtml"};
            this.MasterLocationFormats = new[] {"~/Views/{1}/{0}.cshtml",
                                        "~/Views/Shared/{0}.cshtml"};
            this.PartialViewLocationFormats = new[] {"~/Views/{1}/{0}.cshtml",
                                        "~/Views/Shared/{0}.cshtml"};
            this.FileExtensions = new[] { "cshtml" };
        }
    }
}