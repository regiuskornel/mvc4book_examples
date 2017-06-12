using System.Web.Mvc;

namespace MvcApplication1.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public const string Area = "Admin";
        public override string AreaName { get { return Area; } }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            object fogadottparameter = context.State;
            context.MapRoute(
                AreaName + "_default",
                AreaName + "/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "MvcApplication1.Areas.Admin.Controllers" }
            );
        }
    }
}
