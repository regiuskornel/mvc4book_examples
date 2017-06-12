using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{

    public class HomeController : Controller
    {

        public ActionResult Index(string category, string id)
        {
            ViewBag.Message = String.Format("Kategória: {0} Id: {1}", category, id);
            this.HomeSession.PreviousId = 10;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";
            int tiz = this.HomeSession.PreviousId;
            Session.Abandon();
            return View();
        }

        public ActionResult Contact(string id)
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }



        public HomeSessionData HomeSession
        {
            get
            {
                string sessionName = this.GetType().Name;
                return (HomeSessionData)(Session[sessionName] ?? (Session[sessionName] = new HomeSessionData()));
            }
        }
    }

    [Serializable]
    public class HomeSessionData
    {
        public int PreviousId { get; set; }
        public int[] VisitedProducts { get; set; }
    }
}
