using Flight_Center_Project_FinalExam_DAL;
using Flight_Center_Project_FinalExam_DAL.DAO;
using System;

namespace Flight_Center_Project_FinalExam_BL
{
    public abstract class FacadeBase
    {
        protected AirlineDAOMSSQL<AirlineCompany> _airlineDAO = new AirlineDAOMSSQL<AirlineCompany>();
        protected CountryDAOMSSQL<Country> _countryDAO = new CountryDAOMSSQL<Country>();
        protected CustomerDAOMSSQL<Customer> _customerDAO = new CustomerDAOMSSQL<Customer>();
        protected AdministratorDAOMSSQL<Administrator> _administratorDAO = new AdministratorDAOMSSQL<Administrator>();
        protected FlightDAOMSSQL<Flight> _flightDAO = new FlightDAOMSSQL<Flight>();
        protected FlightHistoryDAOMSSQL<FlightsHistory> _flightHistoryDAO = new FlightHistoryDAOMSSQL<FlightsHistory>();
        protected TicketDAOMSSQL<Ticket> _ticketDAO = new TicketDAOMSSQL<Ticket>();
        protected TicketHistoryDAOMSSQL<TicketsHistory> _ticketHistoryDAO = new TicketHistoryDAOMSSQL<TicketsHistory>();
        protected HistoryTrackingDAOMSSQL<HistoryTracking> _historyTrackingDAO = new HistoryTrackingDAOMSSQL<HistoryTracking>();
        protected FailedLoginAttemptsDAOMSSQL<FailedLoginAttempt> _failedLoginAttemptsDAO = new FailedLoginAttemptsDAOMSSQL<FailedLoginAttempt>();
        protected WebPageSearchResultsHTMLModelDAOMSSQL<WebPageSearchResultsHTMLModel> _webPageSearchResultsHTMLModelDAO = new WebPageSearchResultsHTMLModelDAOMSSQL<WebPageSearchResultsHTMLModel>();

        protected Utility_class_UserDAOMSSQL<Utility_class_User> _utility_Class_UserDAO = new Utility_class_UserDAOMSSQL<Utility_class_User>();
      

    }

}
