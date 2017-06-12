using System.Web.Optimization;
using dotless.Core;

namespace MvcApplication1
{
    public class LessTransform : IBundleTransform
    {
        public void Process(BundleContext context, BundleResponse response)
        {
            response.ContentType = "text/css";
            response.Content = Less.Parse(response.Content);
        }
    }
}