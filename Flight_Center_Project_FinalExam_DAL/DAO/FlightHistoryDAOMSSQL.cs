using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Center_Project_FinalExam_DAL
{
    public class FlightHistoryDAOMSSQL<T> : DAO<T>, IFlightDAO<T> where T : FlightsHistory, IPoco, new()
    {
        public FlightHistoryDAOMSSQL(): base() { }

        public Dictionary<Flight, int> GetAllFlightVacancty()
        {
            throw new NotImplementedException();
        }

        public Flight GetFlightByCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Flight GetFlightByDepartureDate(DateTime departureDate)
        {
            throw new NotImplementedException();
        }

        public Flight getFlightByDestinationCountry(Country destinationCountry)
        {
            throw new NotImplementedException();
        }

        public Flight GetFlightById(long flightID)
        {
            throw new NotImplementedException();
        }

        public Flight GetFlightByLandingDate(DateTime landingDate)
        {
            throw new NotImplementedException();
        }

        public Flight GetFlightByOriginCountry(Country originCountry)
        {
            throw new NotImplementedException();
        }
    }
}
