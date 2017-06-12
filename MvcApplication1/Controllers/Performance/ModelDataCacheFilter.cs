using System;
using System.Collections.Generic;
using System.Data.Metadata.Edm;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

namespace MvcApplication1.Controllers.Performance
{
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ModelDataCacheFilterAttribute : ActionFilterAttribute
{
    //request paraméter nevek.
    private readonly string[] _splittedRequestparams;
    //Elkülönítés actionönként vagy modelnevenként
    private string _cacheKeyPrefix;

    public ModelDataCacheFilterAttribute(string modelName, string requestParams)
    {
        if (string.IsNullOrWhiteSpace(requestParams))
            throw new ArgumentException("Legalább egy request paramétert meg kéne adni...");
        _splittedRequestparams = requestParams.Split(new[] { ',', ';' },
            StringSplitOptions.RemoveEmptyEntries);
        if (!string.IsNullOrWhiteSpace(modelName))
            _cacheKeyPrefix = modelName + ".";
    }

    //Model megszerzése a cache-ből
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        base.OnActionExecuting(filterContext);
        var cachekey = GetCacheKey(filterContext);
        var model = filterContext.HttpContext.Cache[cachekey];
        if (model == null) return;

        //Post esetén a cache érvénytelenítése. 
        if (filterContext.HttpContext.Request.HttpMethod == "POST")
        {
            filterContext.HttpContext.Cache.Remove(cachekey);
            filterContext.Controller.ViewData["originalModel"] = model;
            return;
        }

        /*Így a teljes View kimaradhat:
        var result = new ViewResult {
            ViewData = new ViewDataDictionary(model)
        };
        filterContext.Result = result; */

        //Modell átadása a View számára.
        filterContext.Controller.ViewData.Model = model;
    }

    //Model tárolása a cache-ben
    public override void OnResultExecuted(ResultExecutedContext filterContext)
    {
        base.OnResultExecuted(filterContext);
        var result = filterContext.Result as ViewResultBase;
        if (result == null || result.Model == null) return;

        var cacheKey = GetCacheKey(filterContext);
        var model = filterContext.HttpContext.Cache[cacheKey];
        if (model != null) return; //A cache elem még érvényes

        //Öt perc sliding expiration
        filterContext.HttpContext.Cache.Insert(cacheKey, result.Model, null,
            Cache.NoAbsoluteExpiration, new TimeSpan(1, 10, 10));
    }

    private string GetCacheKey(ControllerContext ccontext)
    {
        if (_cacheKeyPrefix == null) //Nincs modellnév->kontroller+action
        {
            _cacheKeyPrefix = string.Join("+", ccontext.RouteData.Values
                .Where(r => r.Key == "controller" || r.Key == "action")
                .Select(s => (s.Value ?? s.Key).ToString())) + ".";
        }

        var hcontext = ccontext.HttpContext;
        var q = _splittedRequestparams.Where(s => ccontext.RouteData.Values.ContainsKey(s))
            .Select(s => (ccontext.RouteData.Values[s] ?? "").ToString());
        var q2 = _splittedRequestparams.Where(s => hcontext.Request[s] != null)
            .Select(s => hcontext.Request[s].ToString());

        return _cacheKeyPrefix + string.Join(".", q.Union(q2));

    }
}
}