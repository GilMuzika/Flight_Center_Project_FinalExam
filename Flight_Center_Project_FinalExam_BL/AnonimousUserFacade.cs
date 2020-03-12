using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flight_Center_Project_FinalExam_BL
{
    public class AnonimousUserFacade : FacadeBase, IAnonymousUserFacade
    {
        public List<AirlineCompany> GetAllAirlineCompanies()
        {
            return _airlineDAO.GetAll();
        }

        public List<Flight> GetAllFlights()
        {
            return _flightDAO.GetAll();
        }

        public Dictionary<Flight, int> GetAllFlightsVacancy()
        {
            return _flightDAO.GetAllFlightVacancty();
        }

        public Flight GetFlightByDepartureDate(DateTime departureDate)
        {
            return _flightDAO.GetFlightByDepartureDate(departureDate);
        }

        public Flight GetFlightByDestinationCountry(Country destinationCountry)
        {
            return _flightDAO.getFlightByDestinationCountry(destinationCountry);
        }

        public Flight GetFlightByDestinationCountry(int countryCode)
        {
            return _flightDAO.GetFlightByOriginCountry(_countryDAO.Get(countryCode));
        }

        public Flight GetFlightById(long ID)
        {
            return _flightDAO.GetFlightById(ID);
        }

        public Flight GetFlightByLandingDate(DateTime landingDate)
        {
            return _flightDAO.GetFlightByLandingDate(landingDate);
        }

        public Flight GetFlightByOriginCountry(Country originCountry)
        {
            return _flightDAO.GetFlightByOriginCountry(originCountry);
        }

        public Flight GetFlightByOriginCountry(int countryCode)
        {            
            return _flightDAO.GetFlightByOriginCountry(_countryDAO.Get(countryCode));
        }

        protected bool CheckToken<T>(LoginToken<T> token) where T : class, IPoco, IUser, new()
        {
            if (token == null || token.ActualUser == null) return false;

            if (!(token.ActualUser is IPoco)) return false;
            if (!(token.UserAsUser is IUser)) return false;

            if (IsUserExists(token.ActualUser)) throw new UserDoesntExistsException<T>(token.ActualUser);

            var allUsers = _utility_Class_UserDAO.GetAll();

            foreach(var s_AsUtilityUser in allUsers)
            {
                if (s_AsUtilityUser.PASSWORD.Equals(token.UserAsUser.PASSWORD)) return true; 
            }            
            throw new WrongPasswordException(token.UserAsUser.PASSWORD);
        }

        protected bool IsUserExists<T>(T user) where T : class, IPoco, IUser, new()
        {
            bool isExists = false;

            Dictionary<string, UserBaseMSSQLDAO<T>> correlation = new Dictionary<string, UserBaseMSSQLDAO<T>>();
            correlation.Add(typeof(AirlineCompany).Name, _airlineDAO as UserBaseMSSQLDAO<T>);
            correlation.Add(typeof(Customer).Name, _customerDAO as UserBaseMSSQLDAO<T>);
            correlation.Add(typeof(Administrator).Name, _customerDAO as UserBaseMSSQLDAO<T>);

            var user2 = correlation[user.GetType().Name].Get((long)user.GetType().GetProperty("ID").GetValue(user));
            if (user2.Equals(user)) isExists = true;

            return isExists;
        }
        protected bool IsSomethingExists<T>(T user) where T : class, IPoco, new()
        {
            bool isExists = false;

            Dictionary<string, DAO<T>> correlation = new Dictionary<string, DAO<T>>();
            correlation.Add(typeof(Ticket).Name, _airlineDAO as DAO<T>);
            correlation.Add(typeof(Country).Name, _customerDAO as DAO<T>);
            correlation.Add(typeof(Flight).Name, _customerDAO as DAO<T>);

            var user2 = correlation[user.GetType().Name].Get((long)user.GetType().GetProperty("ID").GetValue(user));
            if (user2.Equals(user)) isExists = true;

            return isExists;
        }
    }
}
