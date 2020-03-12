using System;
using System.Collections.Generic;
using System.Text;
using Flight_Center_Project_FinalExam_DAL;

namespace Flight_Center_Project_FinalExam_BL
{
    public class LoggedInCustomerFacade : AnonimousUserFacade, ILoggedInCustomerFacade
    {
        public void CancelTicket(LoginToken<Customer> token, Ticket ticket)
        {
            if (CheckToken(token))
            {
                if (IsSomethingExists(ticket))
                {
                    _ticketDAO.Remove(ticket);
                }
            }
        }

        public IList<Flight> GetAllMyFlights(LoginToken<Customer> token)
        {
            if (CheckToken(token))
            {
                return _flightDAO.GetFlightsByCustomer(token.ActualUser);
            }
            else throw new UserDoesntExistsException<Customer>(token.ActualUser);
        }

        public Ticket PurchaseTicket(LoginToken<Customer> token, Flight flight)
        {
            if (CheckToken(token)) 
            {
                foreach(var s in _ticketDAO.GetAll())
                {
                    if (s.FLIGHT_ID == flight.ID) { return s; }
                }
                throw new TicketNotFoundException(flight.ID);
            }
            else throw new UserDoesntExistsException<Customer>(token.ActualUser);
        }
    }
}
