using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Glimpse.AspNet.Tab;

namespace MvcApplication1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(name: "Kategoriak",
            //    url: "{controller}/{action}/{id}/{category}/{format}",
            //    defaults: new
            //    {
            //        controller = "Home",
            //        action = "Index",
            //        format = UrlParameter.Optional,
            //        category = UrlParameter.Optional,
            //        id = UrlParameter.Optional
            //    }
            //    );

            //Route constraints regex:
            //routes.MapRoute(name: "Kategoriak",
            //    url: "{controller}/{action}/{category}/{id}",
            //    defaults: new { controller = "Home", action = "Index", category = UrlParameter.Optional, id = UrlParameter.Optional },
            //    constraints: new { id = @"^\d+$", category = @"(Butorok|Textil|Vilagitas)" }
            //    );

            //Route constraints IRouteConstraint:
            //routes.MapRoute(name: "Kategoriak",
            //    url: "{controller}/{action}/{category}/{id}",
            //    defaults: new { controller = "Home", action = "Index", category = UrlParameter.Optional, id = UrlParameter.Optional },
            //    constraints: new { id_akarmi = new MultiConstraint() },
            //    namespaces: new string[] { "MvcApplication1.Controllers" }
            //    );

            // Html.RouteLink kipróbálásához:
            //routes.MapRoute(
            //    name: "complains",
            //    url: "complains/{controller}/{action}/{id}",
            //    defaults: new { id = UrlParameter.Optional }
            //);

            //PageRoute
            routes.MapPageRoute("staticPages", "forms/{webform}", "~/AspPages/{webform}.aspx");

            //Multilanguage:
            var langroute = new LocalizedRoute("{lang}/{controller}/{action}/{id}",
                new { lang = "en", controller = "Home", action = "Index", id = UrlParameter.Optional });
            langroute.DataTokens = new RouteValueDictionary
                {
                    {"Namespaces", new string[] {"MvcApplication1.Controllers"}}
                };
            routes.Add("LocalizedRoute", langroute);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "MvcApplication1.Controllers" }
            );
        }
    }

    public class MultiConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string paramName,
        RouteValueDictionary valuesDict, RouteDirection routeDirection)
        {

            object idobject;
            if (!valuesDict.TryGetValue("id", out idobject) || idobject == null)
                return false;
            int id;
            if (!Int32.TryParse(idobject.ToString(), out id))
                return false;
            if (id < 1 && id > 10000)
                return false;

            object categobject;
            if (!valuesDict.TryGetValue("category", out categobject) || categobject == null)
                return false;
            switch (categobject.ToString())
            {
                case "Butorok":
                    return id < 100;
                case "Textil":
                    return id < 10;
                case "Vilagitas":
                    return id < 5000;
                default:
                    return false;
            }
        }
    }

    public class LocalizedRoute : Route
    {
        public LocalizedRoute(string url, object defaults)
            : base(url, new RouteValueDictionary(defaults),
                            new RouteValueDictionary(new { lang = "[a-z]{2}" }), new MvcRouteHandler()) { }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            if (!values.ContainsKey("lang"))
                values["lang"] = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

            return base.GetVirtualPath(requestContext, values);
        }
    }
}