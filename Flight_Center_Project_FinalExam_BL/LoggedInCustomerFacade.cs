using System;
using System.Collections.Generic;
using System.Text;
using Flight_Center_Project_FinalExam_DAL;

namespace Flight_Center_Project_FinalExam_BL
{
    public class LoggedInCustomerFacade : AnonimousUserFacade, ILoggedInCustomerFacade
    {
        public bool CancelTicket(LoginToken<Customer> token, Ticket ticket)
        {
            if (CheckToken(token))
            {
                if (IsSomethingExists(ticket))
                {

                    _ticketHistoryDAO.Add(ticket.ConvertToHistoryObject<Ticket, TicketsHistory>());

                    HistoryTracking thisTicketTrackingInfo = new HistoryTracking();
                    thisTicketTrackingInfo.HISTORY_ENTRY_ID = ticket.ID;
                    thisTicketTrackingInfo.HISTORY_ENTRY_KIND = ticket.GetType().Name;
                    thisTicketTrackingInfo.HISTORY_ENTRY_TIME = DateTime.UtcNow;

                    _historyTrackingDAO.Add(thisTicketTrackingInfo);

                    _ticketDAO.Remove(ticket);
                    return true;
                }
            }
            return false;
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
                //throw new TicketNotFoundException(flight.ID);
                return null;
            }
            else throw new UserDoesntExistsException<Customer>(token.ActualUser);
        }

        public List<Ticket> PurchaseTickets(LoginToken<Customer> token, Flight flight)
        {
            List<Ticket> ticketsLst = new List<Ticket>();
            if (CheckToken(token))
            {
                foreach (var s in _ticketDAO.GetAll())
                {
                    if (s.FLIGHT_ID == flight.ID) { ticketsLst.Add(s); }
                }
                //throw new TicketNotFoundException(flight.ID);
                if (ticketsLst.Count == 0) return null;

                return ticketsLst;
            }
            else throw new UserDoesntExistsException<Customer>(token.ActualUser);
        }
    }
}
