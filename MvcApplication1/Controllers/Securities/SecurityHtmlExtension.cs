using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace MvcApplication1.Controllers.Securities
{
    public static class SecurityHtmlExtension
    {
        public const string SecurityHiddenFieldNamePrefix = "hobj-";
        public static MvcHtmlString EncodedHidden(this HtmlHelper htmlHelper, string name, object value)
        {
            if (value == null) return MvcHtmlString.Empty;

            var encoded = StringEncoderHelper.GetEncodedString(value);
            return InputExtensions.Hidden(htmlHelper, SecurityHiddenFieldNamePrefix + name, encoded);
        }

        public static MvcHtmlString EncodedActionLink(this HtmlHelper htmlHelper, string title, string action, string controller, int id)
        {
            var routeValues = new RouteValueDictionary();
            routeValues.Add("id", StringEncoderHelper.GetEncodedString(id));

            return MvcHtmlString.Create(HtmlHelper.GenerateLink(htmlHelper.ViewContext.RequestContext,
                htmlHelper.RouteCollection, title, (string)null, action, controller,
                routeValues, null));
        }

        public static MvcHtmlString EncodedActionLink(this HtmlHelper htmlHelper, string title, string action, string controller, Dictionary<string, object> routeData)
        {
            var routeValues = new RouteValueDictionary();
            routeValues.Add("id", StringEncoderHelper.GetEncodedString(routeData));

            return MvcHtmlString.Create(HtmlHelper.GenerateLink(htmlHelper.ViewContext.RequestContext,
                htmlHelper.RouteCollection, title, (string)null, action, controller,
                routeValues, null));
        }

    }

}