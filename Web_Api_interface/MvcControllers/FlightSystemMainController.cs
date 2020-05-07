using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_Api_interface.MvcControllers
{
    public class FlightSystemMainController : Controller
    {
        // GET: FlightSystemMain
        public ActionResult FlightSystemMainView()
        {
            ViewBag.PageKind = "Departures";
            //return View("../Views/FlightSystemMain/FlightsSystemMainView.cshtml");
            return View();
        }
    }
}