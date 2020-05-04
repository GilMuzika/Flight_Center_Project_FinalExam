using Flight_Center_Project_FinalExam_BL;
using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Web_Api_interface.IControllers;

namespace Web_Api_interface.Controllers
{
    [RoutePrefix("api/AnonimousUserFacade")]
    public class AnonimousUserFacadeController : ApiController, IAnonimousUserFacadeController
    {
        FlyingCenterSystem _fsc = FlyingCenterSystem.GetInstance();

        AnonimousUserFacade _anonimousUserFacade;

        public AnonimousUserFacadeController()
        {
            _anonimousUserFacade = _fsc.GetAnonimousFacade();
        }

        [ResponseType(typeof(AirlineCompany))]
        [HttpGet]
        [Route("GetAllAirlineCompanies", Name = "AnonimousGetAllAirlineCompanies")]
        public IHttpActionResult GetAllAirlineCompanies()
        {
            List<AirlineCompany> airlineCompanies =  _anonimousUserFacade.GetAllAirlineCompanies();
            if (airlineCompanies.Count == 0) return NotFound();

            return Ok(airlineCompanies);
        }

        [ResponseType(typeof(AirlineCompany))]
        [HttpGet]
        [Route("GetAllCustomers", Name = "GetAllCustomers")]
        public IHttpActionResult GetAllCustomers()
        {
            List<Customer> customers = _anonimousUserFacade.GetAll<Customer>();
            if (customers.Count == 0) return NotFound();

            return Ok(customers);
        }

        [ResponseType(typeof(Flight))]
        [HttpGet]
        [Route("GetAllFlights", Name = "AnonimousGetAllFlights")]
        public virtual IHttpActionResult GetAllFlights()
        {

            List<Flight> flights = _anonimousUserFacade.GetAllFlights();
            if (flights.Count == 0) return NotFound();

            return Ok(flights);
        }

        [ResponseType(typeof(Flight))]
        [HttpGet]
        [Route("GetAllFlightsAndRemainingTichketsForEachFly", Name = "AnonimousGetAllFlightsAndRemainingTichketsForEachFly")]
        public IHttpActionResult GetAllFlightsVacancy()
        {

            Dictionary<Flight, int> flightsVacancy = _anonimousUserFacade.GetAllFlightsVacancy();
            if (flightsVacancy.Count == 0) return NotFound();

            return Ok(flightsVacancy);            
        }

        [ResponseType(typeof(Flight))]
        [HttpPost]
        [Route("GetFlightByDepartureDate", Name = "AnonimousGetFlightByDepartureDate")]
        public IHttpActionResult GetFlightByDepartureDate([FromBody] DateTime departureTime)
        {
            Flight flight =  _anonimousUserFacade.GetFlightByDepartureDate(departureTime);
            if (flight == null) return NotFound();

            return Ok(flight);           
        }

        [ResponseType(typeof(Flight))]
        [HttpPost]
        [Route("GetFlightByLandingDate", Name = "AnonimousGetFlightByLandingDate")]
        public IHttpActionResult GetFlightByLandingDate([FromBody] DateTime landingDate)
        {
            Flight flight = _anonimousUserFacade.GetFlightByLandingDate(landingDate);
            if (flight == null) return NotFound();

            return Ok(flight);
        }

        [ResponseType(typeof(Flight))]
        [HttpPost]
        [Route("GetFlightByDestinationCountry", Name = "AnonimousGetFlightByDestinationCountry")]
        public IHttpActionResult GetFlightByDestinationCountry([FromBody] Country  destinationCountry)
        {
            Flight flight = _anonimousUserFacade.GetFlightByDestinationCountry(destinationCountry);
            if (flight == null) return NotFound();

            return Ok(flight);
        }

        [ResponseType(typeof(Flight))]
        [HttpGet]
        [Route("GetFlightByDestinationCountry/{destinationCountryCode}", Name = "AnonimousGetFlightByDestinationCountryCode")]
        public IHttpActionResult GetFlightByDestinationCountry([FromUri] int destinationCountryCode)
        {
            Flight flight = _anonimousUserFacade.GetFlightByDestinationCountry(destinationCountryCode);
            if (flight == null) return NotFound();

            return Ok(flight);
        }

        [ResponseType(typeof(Flight))]
        [HttpPost]
        [Route("GetFlightByOriginCountry", Name = "AnonimousGetFlightByOriginCountry")]
        public IHttpActionResult GetFlightByOriginCountry([FromBody] Country originCountry)
        {
            Flight flight = _anonimousUserFacade.GetFlightByOriginCountry(originCountry);
            if (flight == null) return NotFound();

            return Ok(flight);
        }

        [ResponseType(typeof(Flight))]
        [HttpGet]
        [Route("GetFlightByOriginCountry/{originCountryCode}", Name = "AnonimousGetFlightByOriginCountryCode")]
        public IHttpActionResult GetFlightByOriginCountry([FromUri] int originCountryCode)
        {
            Flight flight = _anonimousUserFacade.GetFlightByOriginCountry(originCountryCode);
            if (flight == null) return NotFound();

            return Ok(flight);            
        }






    }
}
