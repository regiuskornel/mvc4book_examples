
namespace ASP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Optimization;
    using System.Web.Routing;
    using WebMatrix.Data;
    using WebMatrix.WebData;
    using Microsoft.Web.WebPages.OAuth;
    using DotNetOpenAuth.AspNet;


    public class _Page_Views_Nezet_ViewContext_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        /*
        protected ASP.global_asax ApplicationInstance
        {
            get { return ((ASP.global_asax)(Context.ApplicationInstance)); }
        }
        */

        public override void Execute()
        {
            Type viewTipusa = this.GetType();
            WriteLiteral("\r\n\r\n<h2>ViewContext felfedése</h2>\r\nA View típusa: ");
            Write(viewTipusa);
            WriteLiteral(" <br />\r\nA View dll fájlja: ");
            Write(viewTipusa.Assembly.Location);
            WriteLiteral("\r\n");
        }
    }


}


namespace ASP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Optimization;
    using System.Web.Routing;
    using WebMatrix.Data;
    using WebMatrix.WebData;
    using Microsoft.Web.WebPages.OAuth;
    using DotNetOpenAuth.AspNet;


    public class _Page_Views_Shared__Layout_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {

#line hidden

        public _Page_Views_Shared__Layout_cshtml()
        {
        }
        /*
        protected ASP.global_asax ApplicationInstance
        {
            get
            {
                return ((ASP.global_asax)(Context.ApplicationInstance));
            }
        }*/

        public override void Execute()
        {
            WriteLiteral("<!DOCTYPE html>\r\n<html");
            WriteLiteral(" lang=\"en\"");
            WriteLiteral(">\r\n    <head>\r\n        <meta");
            //...
            WriteLiteral("\r\n    </head>\r\n    <body>");
            //...
            Write(RenderSection("featured", required: false));
            //...
            Write(RenderBody());
            //...
            WriteLiteral("\r\n    </body>\r\n</html>\r\n");
        }
    }
}

