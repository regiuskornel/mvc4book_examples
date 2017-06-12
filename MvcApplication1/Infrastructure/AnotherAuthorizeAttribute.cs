using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcApplication1.Infrastructure
{
public class AnotherAuthorizeAttribute : AuthorizeAttribute
{
    public override void OnAuthorization(AuthorizationContext filterContext)
    {
        if (filterContext == null)
        {
            throw new ArgumentNullException("filterContext");
        }

        var roles = SplitString(Roles);
        IPrincipal user = filterContext.HttpContext.User;

        if (user.Identity.IsAuthenticated && roles.Length > 0 && !roles.Any(user.IsInRole))
        {
            var url = new UrlHelper(filterContext.RequestContext);
            filterContext.Result = new RedirectResult(
                url.Action("YourAreNotInRole", "Security", new {rolenames = Roles}));
            return;
        }
        base.OnAuthorization(filterContext);
    }

    private static string[] SplitString(string original)
    {
        if (String.IsNullOrEmpty(original))
        {
            return new string[0];
        }

        var split = from piece in original.Split(',')
                    let trimmed = piece.Trim()
                    where !String.IsNullOrEmpty(trimmed)
                    select trimmed;
        return split.ToArray();
    }
}


}