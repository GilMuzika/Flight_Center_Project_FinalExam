using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Web_Api_interface.IControllers
{    
    interface IAnonimousUserFacadeController
    {
        [ResponseType(typeof(AirlineCompany))]
        [HttpGet]
        IHttpActionResult GetAllAirlineCompanies();

        [ResponseType(typeof(Flight))]
        [HttpGet]
        IHttpActionResult GetAllFlights();

        [ResponseType(typeof(Flight))]
        [HttpGet]
        IHttpActionResult GetAllFlightsVacancy();

        [ResponseType(typeof(Flight))]
        [HttpPost]
        IHttpActionResult GetFlightByDepartureDate([FromBody] DateTime departureTime);

        [ResponseType(typeof(Flight))]
        [HttpPost]
        IHttpActionResult GetFlightByLandingDate([FromBody] DateTime landingDate);

        [ResponseType(typeof(Flight))]
        [HttpPost]
        IHttpActionResult GetFlightByDestinationCountry([FromBody] Country destinationCountry);

        [ResponseType(typeof(Flight))]
        [HttpGet]
        IHttpActionResult GetFlightByDestinationCountry([FromUri] int destinationCountryCode);

        [ResponseType(typeof(Flight))]
        [HttpPost]
        IHttpActionResult GetFlightByOriginCountry([FromBody] Country originCountry);

        [ResponseType(typeof(Flight))]
        [HttpGet]
        IHttpActionResult GetFlightByOriginCountry([FromUri] int originCountryCode);

    }
}
