using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flight_Center_Project_FinalExam_BL
{
    public class LoggedInAirlineFacade : AnonimousUserFacade, ILoggedInAirlineFacade
    {
        public bool CancelFlight(LoginToken<AirlineCompany> token, Flight flight)
        {

            if (CheckToken(token))
            {
                if (IsSomethingExists(flight))
                {
                    _flightHistoryDAO.Add(flight.ConvertToHistoryObject<Flight, FlightsHistory>());

                    HistoryTracking thisFlightTrackingInfo = new HistoryTracking();
                    thisFlightTrackingInfo.HISTORY_ENTRY_ID = flight.ID;
                    thisFlightTrackingInfo.HISTORY_ENTRY_KIND = flight.GetType().Name;
                    thisFlightTrackingInfo.HISTORY_ENTRY_TIME = DateTime.UtcNow;

                    _historyTrackingDAO.Add(thisFlightTrackingInfo);

                    _flightDAO.Remove(flight);
                    return true;
                }
            }
            return false;
        }
        public bool CancelAllFlights(LoginToken<AirlineCompany> token)
        {
            if(CheckToken(token))
            {
                List<Flight> allFlights = _flightDAO.GetAll();
                _ticketDAO.DeleteAll();
                foreach(Flight flight in allFlights)
                {
                    _flightHistoryDAO.Add(flight.ConvertToHistoryObject<Flight, FlightsHistory>());

                    HistoryTracking thisFlightTrackingInfo = new HistoryTracking();
                    thisFlightTrackingInfo.HISTORY_ENTRY_ID = flight.ID;
                    thisFlightTrackingInfo.HISTORY_ENTRY_KIND = flight.GetType().Name;
                    thisFlightTrackingInfo.HISTORY_ENTRY_TIME = DateTime.UtcNow;

                    _historyTrackingDAO.Add(thisFlightTrackingInfo);

                    _flightDAO.Remove(flight);
                }

                if (_ticketDAO.GetAll().Count == 0 && _flightDAO.GetAll().Count == 0) return true;
            }
            return false;
        }

        public bool ChangeMyPassword(LoginToken<AirlineCompany> token, string oldPassword, string newPassword, out bool isPasswordWrong)
        {
            bool isChanged = false;            
            if(CheckToken(token))
            {
                var utility_user = _utility_Class_UserDAO.GetUserByIdentifier(token.ActualUser);
                string utility_user_PASSWORD = string.Empty;
                if (utility_user.PASSWORD.Length > 50) utility_user_PASSWORD = EncryptionProvider.Decryprt(utility_user.PASSWORD);
                else utility_user_PASSWORD = utility_user.PASSWORD;
                

                if (utility_user_PASSWORD.Equals(oldPassword))
                    _airlineDAO.Update(token.ActualUser, utility_user.USER_NAME, newPassword);
                else 
                {                    
                    isPasswordWrong = true;
                    //throw new WrongPasswordException(oldPassword); 
                }

                var utility_userForChecking = _utility_Class_UserDAO.GetUserByIdentifier(token.ActualUser);
                if (utility_userForChecking.PASSWORD.Equals(newPassword)) isChanged = true;
            }
            isPasswordWrong = false;
            return isChanged;
        }

        public bool CreateFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            bool isAddded = false;
            if (CheckToken(token))
            {
                long flightId = _flightDAO.Add(flight);
                Flight flightForChecking = _flightDAO.Get(flightId);
                isAddded = Statics.BulletprofComparsion(flight, flightForChecking);
                if (!isAddded) _flightDAO.Remove(flightForChecking);
            }            
            else throw new UserDoesntExistsException<AirlineCompany>(token.ActualUser);

            return isAddded;
        }

        public IList<Flight> GetAllFlights(LoginToken<AirlineCompany> token)
        {
            if (CheckToken(token))
            {
                return _flightDAO.GetAll();
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

        public bool MofidyAirlineDetails(LoginToken<AirlineCompany> token, AirlineCompany airline)
        {
            bool isUpdated = false;
            if (CheckToken(token))
            {
                _airlineDAO.Update(airline);
                AirlineCompany airlineForChecking = _airlineDAO.Get(airline.ID);
                isUpdated = Statics.BulletprofComparsion(airlineForChecking, airline);
            }
            else throw new UserDoesntExistsException<AirlineCompany>(token.ActualUser);
            return isUpdated;
        }

        public bool UpdateFlight(LoginToken<AirlineCompany> token, Flight flight, out bool isFound)
        {
            bool isFoundInternal = true;
            bool isUpdated = false;
            if (CheckToken(token))
            {
                List<long> allFlightsIDs = _flightDAO.GetAll().Select(x => x.ID).ToList();
                if (!allFlightsIDs.Contains(flight.ID)) isFoundInternal = false;

                if (isFoundInternal)
                {
                    _flightDAO.Update(flight);
                    Flight flightForChecking = _flightDAO.Get(flight.ID);
                    isUpdated = Statics.BulletprofComparsion(flight, flightForChecking);
                }
            }
            else throw new UserDoesntExistsException<AirlineCompany>(token.ActualUser);

            isFound = isFoundInternal;
            return isUpdated;
        }

    }
}
