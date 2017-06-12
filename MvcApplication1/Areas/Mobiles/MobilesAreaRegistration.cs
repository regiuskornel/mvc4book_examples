using System.Web.Mvc;

namespace MvcApplication1.Areas.Mobiles
{
    public class MobilesAreaRegistration : AreaRegistration
    {
        public const string Area = "Mobiles";
        public override string AreaName { get { return Area; } }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                AreaName + "_default",
                AreaName + "/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "MvcApplication1.Areas.Mobiles.Controllers" }
            );
        }
    }


}
