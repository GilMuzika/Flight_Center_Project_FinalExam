using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flight_Center_Project_FinalExam_BL
{
    public class LoggedInAirlineFacade : AnonimousUserFacade, ILoggedInAirlineFacade
    {
        public void CancelFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            if(CheckToken(token)) _flightDAO.Remove(flight);
        }

        public void ChangeMyPassword(LoginToken<AirlineCompany> token, string oldPassword, string newPassword)
        {
            if(CheckToken(token))
            {
                var utility_user = _utility_Class_UserDAO.GetUserByIdentifier(token.ActualUser);
                if (utility_user.PASSWORD.Equals(oldPassword)) _utility_Class_UserDAO.Update(utility_user, utility_user.USER_NAME, newPassword);
                else throw new WrongPasswordException(oldPassword);
            }
        }

        public void CreateFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            if (CheckToken(token))
            {
                _flightDAO.Add(flight);
            }
            else throw new UserDoesntExistsException<AirlineCompany>(token.ActualUser);
        }

        public IList<Ticket> GetAllFlights(LoginToken<AirlineCompany> token)
        {
            if (CheckToken(token))
            {
                return _flightDAO.GetAll().Select(x => _ticketDAO.GetSomethingBySomethingInternal(x.ID, (int)TicketPropertyNumber.FLIGHT_ID)).ToList();
            }
            else throw new UserDoesntExistsException<AirlineCompany>(token.ActualUser);
        }

        public IList<Ticket> GetAllTickets(LoginToken<AirlineCompany> token)
        {
            if (CheckToken(token))
            {
                return _ticketDAO.GetAll();
            }
            else throw new UserDoesntExistsException<AirlineCompany>(token.ActualUser);
        }

        public void MofidyAirlineDetails(LoginToken<AirlineCompany> token, AirlineCompany airline)
        {
            if (CheckToken(token))
            {
                _airlineDAO.Update(airline);
            }
            else throw new UserDoesntExistsException<AirlineCompany>(token.ActualUser);
        }

        public void UpdateFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            if (CheckToken(token))
            {
                _flightDAO.Update(flight);
            }
            else throw new UserDoesntExistsException<AirlineCompany>(token.ActualUser);
        }
    }
}
