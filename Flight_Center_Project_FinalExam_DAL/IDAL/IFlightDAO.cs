using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Center_Project_FinalExam_DAL
{
    public interface IFlightDAO<T>: IBasicDB<T> where T : class, IPoco, new()
    {
        Dictionary<Flight, int> GetAllFlightVacancty();
        Flight GetFlightById(long flightID);
        Flight GetFlightByCustomer(Customer customer);
        Flight GetFlightByDepartureDate(DateTime departureDate);
        Flight getFlightByDestinationCountry(Country destinationCountry);
        Flight GetFlightByLandingDate(DateTime landingDate);
        Flight GetFlightByOriginCountry(Country originCountry);

    }
}
