using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MvcApplication1.Infrastructure
{
    public class MyJsonResult : ActionResult
    {
        private readonly object _model;

        public MyJsonResult(object modeltoJson)
        {
            this._model = modeltoJson;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new[] { new DateTimeJsonConverter() });

            var serialized = serializer.Serialize(_model);

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "application/json";
            response.Write(serialized);
            response.End();
        }
    }
}