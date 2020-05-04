using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flight_Center_Project_FinalExam_BL
{
    interface ILoggedInAirlineFacade : IAnonymousUserFacade
    {
        IList<Ticket> GetAllTickets(LoginToken<AirlineCompany> token);
        IList<Flight> GetAllFlights(LoginToken<AirlineCompany> token);
        bool CancelFlight(LoginToken<AirlineCompany> token, Flight flight);
        bool CreateFlight(LoginToken<AirlineCompany> token, Flight flight);
        bool UpdateFlight(LoginToken<AirlineCompany> token, Flight flight, out bool isFound);
        bool ChangeMyPassword(LoginToken<AirlineCompany> token, string oldPassword, string newPassword, out bool isPasswordWrong);
        bool MofidyAirlineDetails(LoginToken<AirlineCompany> token, AirlineCompany airline);

    }
}
