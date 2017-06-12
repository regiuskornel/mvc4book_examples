using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Services
{
    public class SillyLogger
    {
        static SillyLogger()
        {
            System.Diagnostics.Debug.WriteLine("Started : {0:D}", DateTime.Now);
        }

        public static void EnterMethod(string methodname)
        {
            System.Diagnostics.Debug.WriteLine("{0:HH:mm:ss.fff} Enter '{1}' action", DateTime.Now, methodname);
        }

        public static void Store(string message)
        {
            System.Diagnostics.Debug.WriteLine("   " + message);
        }

        public static void ExitMethod(string methodname)
        {
            System.Diagnostics.Debug.WriteLine("{0:HH:mm:ss.fff} Exit '{1}' action", DateTime.Now, methodname);
        }
    }


    public class SillyLoggerActionFilterAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            SillyLogger.EnterMethod(filterContext.ActionDescriptor.ActionName);
        }

        public override void OnActionExecuted(System.Web.Mvc.ActionExecutedContext filterContext)
        {
            SillyLogger.ExitMethod(filterContext.RouteData.Values["action"].ToString());
        }

        public override void OnResultExecuted(System.Web.Mvc.ResultExecutedContext filterContext)
        {
            ViewResult result = filterContext.Result as ViewResult;
            if (result != null)
                SillyLogger.Store("Használt view neve: " + result.ViewName);

            RazorView razor = result.View as RazorView;
            if (razor != null)
                SillyLogger.Store("Használt view template: " + razor.ViewPath);

            //var response = filterContext.HttpContext.Response;
            //response.Filter = new LogFilter(response.Filter, filterContext.RouteData.Values["action"].ToString());

            SillyLogger.Store(string.Format("{0} action and view processed", filterContext.RouteData.Values["action"]));
        }

        /// <summary>
        /// Based on: http://arranmaclean.wordpress.com/2010/08/10/minify-html-with-net-mvc-actionfilter/
        /// Stream láncolás
        /// </summary>
        public class LogFilter : Stream
        {
            /// <summary>
            /// Eredeti stream
            /// </summary>
            private Stream _shrinkStream;
            /// <summary>
            /// Action neve
            /// </summary>
            private string _action;

            public LogFilter(Stream shrink, string action)
            {
                _shrinkStream = shrink;
                _action = action;
            }

            public override void Flush()
            {
                _shrinkStream.Flush();
                SillyLogger.Store(string.Format("{0} view rendered", _action));
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return _shrinkStream != null ? _shrinkStream.Seek(offset, origin) : 0;
            }

            public override void SetLength(long value)
            {
                _shrinkStream.SetLength(value);
            }

            public override void Close()
            {
                _shrinkStream.Close();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return _shrinkStream.Read(buffer, offset, count);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                //A renderelt html:
                string html = System.Text.Encoding.Default.GetString(buffer);
                SillyLogger.Store(html); //Loggolása
                _shrinkStream.Write(buffer, offset, count);
            }

            public override bool CanRead
            {
                get { return _shrinkStream.CanRead; }
            }

            public override bool CanSeek
            {
                get { return _shrinkStream.CanSeek; }
            }

            public override bool CanWrite
            {
                get { return _shrinkStream.CanWrite; }
            }

            public override long Length
            {
                get { return _shrinkStream.Length; }
            }

            public override long Position { get; set; }
        }
    }
}