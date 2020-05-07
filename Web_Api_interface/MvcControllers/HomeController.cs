using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_Api_interface.MvcControllers
{
    [RoutePrefix("mvc")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";   

            return View(); 
        }
    }
}
