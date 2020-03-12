using Flight_Center_Project_FinalExam_DAL;
using System;

namespace Flight_Center_Project_FinalExam_BL
{
    public abstract class FacadeBase
    {
        protected AirlineDAOMSSQL<AirlineCompany> _airlineDAO = new AirlineDAOMSSQL<AirlineCompany>();
        protected CountryDAOMSSQL<Country> _countryDAO = new CountryDAOMSSQL<Country>();
        protected CustomerDAOMSSQL<Customer> _customerDAO = new CustomerDAOMSSQL<Customer>();
        protected FlightDAOMSSQL<Flight> _flightDAO = new FlightDAOMSSQL<Flight>();
        protected TicketDAOMSSQL<Ticket> _ticketDAO = new TicketDAOMSSQL<Ticket>();
        protected Utility_class_UserDAOMSSQL<Utility_class_User> _utility_Class_UserDAO = new Utility_class_UserDAOMSSQL<Utility_class_User>();




        private void Initialize()
        {

        }
    }

}
