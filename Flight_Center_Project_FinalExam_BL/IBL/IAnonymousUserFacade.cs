using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flight_Center_Project_FinalExam_BL
{
    public interface IAnonymousUserFacade
    {
        List<AirlineCompany> GetAllAirlineCompanies();
        List<Flight> GetAllFlights();
        Dictionary<Flight, int> GetAllFlightsVacancy();
        Flight GetFlightById(long ID);
        Flight GetFlightByDepartureDate(DateTime departureDate);
        Flight GetFlightByDestinationCountry(int countryCode);
        Flight GetFlightByLandingDate(DateTime landingDate);
        Flight GetFlightByOriginCountry(int countryCode);

    }
}
