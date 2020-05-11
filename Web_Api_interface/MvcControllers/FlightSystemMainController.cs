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
            ViewBag.PageKindIconSrc = "departures.gif";
            return View("FlightSystemMainView");
            //return View();
        }

        public ActionResult Departures(string str)
        {
            ViewBag.Title = "Departures";
            ViewBag.PageKind = "Departures";
            ViewBag.PageKindIconSrc = "departures.gif";
            return View("FlightSystemMainView");
        }

        public ActionResult Landings(string str)
        {
            ViewBag.Title = "Landings";
            ViewBag.PageKind = "Landings";
            ViewBag.PageKindIconSrc = "arrivings.gif";
            return View("FlightSystemMainView");
        }
    }
}