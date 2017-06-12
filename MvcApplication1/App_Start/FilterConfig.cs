using System;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute()
              {
                  Order = 10,
                  View = "HandleException",
                  ExceptionType = typeof(InvalidOperationException)
              });
            filters.Add(new HandleErrorAttribute() {Order = 1});
            //filters.Add(new Services.SillyLoggerActionFilterAttribute());
            //int i = 1;
        }
    }
}