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
        public List<Flight> GetAllFlightsThatTakeOffInSomeTimeFromNow(TimeSpan someTime, RazorViewStatus status)
        {
            if (status == RazorViewStatus.Landings)
            {
                List<Flight> oveallLandingFlights;
                List<Flight> willLandFlights;

                //first of all, flights that already landed
                oveallLandingFlights = _flightDAO.GetAllFlightsThatTakeOffInSomeTimeFromNow(someTime, status, RazorViewStatus.Future);                
                //second of all, flights tht will land in 4 hours
                willLandFlights = _flightDAO.GetAllFlightsThatTakeOffInSomeTimeFromNow(new TimeSpan(4, 0, 0), status, RazorViewStatus.Past);                

                oveallLandingFlights.AddRange(willLandFlights);

                return oveallLandingFlights;
            }

                return _flightDAO.GetAllFlightsThatTakeOffInSomeTimeFromNow(someTime, status, RazorViewStatus.Future);            
        }



        /// <summary>
        /// Builds infrastructure for Get() and GetAll() generic types
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private Dictionary<string, DAO<T>> BuildCorrelationDictionary<T>() where T : class, IPoco, new()
        {
            Dictionary<string, DAO<T>> correlation = new Dictionary<string, DAO<T>>();
            correlation.Add(typeof(Administrator).Name, _administratorDAO as DAO<T>);
            correlation.Add(typeof(AirlineCompany).Name, _airlineDAO as DAO<T>);
            correlation.Add(typeof(Country).Name, _countryDAO as DAO<T>);
            correlation.Add(typeof(Customer).Name, _customerDAO as DAO<T>);
            correlation.Add(typeof(Flight).Name, _flightDAO as DAO<T>);
            correlation.Add(typeof(Ticket).Name, _ticketDAO as DAO<T>);

            return correlation;
        }

        /// <summary>
        /// Generic GetAll
        /// </summary>
        /// <typeparam name="T">type argument</typeparam>
        /// <returns></returns>
        public List<T> GetAll<T>() where T : class, IPoco, new()
        {
            Dictionary<string, DAO<T>> correlation = BuildCorrelationDictionary<T>();          

            return correlation[typeof(T).Name].GetAll();
        }

        /// <summary>
        /// Generic Get
        /// </summary>
        /// <typeparam name="T">type argument</typeparam>
        /// <returns></returns>
        public T Get<T>(long Id) where T : class, IPoco, new()
        {
            Dictionary<string, DAO<T>> correlation = BuildCorrelationDictionary<T>();

            return correlation[typeof(T).Name].Get(Id);
        }

        public Dictionary<Flight, int> GetAllFlightsVacancy()
        {
            return _flightDAO.GetAllFlightVacancty();
        }

        public Flight GetFlightByDepartureDate(DateTime departureDate)
        {
            return _flightDAO.GetFlightByDepartureDate(departureDate);
        }

        /// <summary>
        /// returns dictionary with all the flights as keys and their respective numbers of remaining tickets as values
        /// </summary>
        /// <returns></returns>
        public Flight GetFlightByDestinationCountry(Country destinationCountry)
        {
            return _flightDAO.getFlightByDestinationCountry(destinationCountry);
        }

        public Flight GetFlightByDestinationCountry(int countryCode)
        {
            var r = _countryDAO.Get(countryCode);
            return _flightDAO.getFlightByDestinationCountry(_countryDAO.Get(countryCode));
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

        protected bool CheckToken<T>(LoginToken<T> token) where T : class, IPoco, new()
        {
            if (token == null || token.ActualUser == null) return false;

            if (!(token.ActualUser is IPoco)) return false;            

            if (!IsUserExists(token.ActualUser)) throw new UserDoesntExistsException<T>(token.ActualUser);

            var allUsers = _utility_Class_UserDAO.GetAll();

            foreach(var s_AsUtilityUser in allUsers)
            {
                if (s_AsUtilityUser.PASSWORD.Equals(token.UserAsUser.PASSWORD)) return true; 
            }            
            throw new WrongPasswordException(token.UserAsUser.PASSWORD);
        }

        protected bool IsUserExists<T>(T user) where T : class, IPoco, new()
        {
            bool isExists = false;

            Dictionary<string, UserBaseMSSQLDAO<T>> correlation = new Dictionary<string, UserBaseMSSQLDAO<T>>();
            correlation.Add(typeof(AirlineCompany).Name, _airlineDAO as UserBaseMSSQLDAO<T>);
            correlation.Add(typeof(Customer).Name, _customerDAO as UserBaseMSSQLDAO<T>);
            correlation.Add(typeof(Administrator).Name, _administratorDAO as UserBaseMSSQLDAO<T>);


            //var users2Lst = correlation[user.GetType().Name].GetAll();
            var user2 = correlation[user.GetType().Name].Get((long)user.GetType().GetProperty("ID").GetValue(user));           
            if (user2.Equals(user)) isExists = true;
            //isExists = Statics.BulletprofComparsion(user, user2);

            return isExists;
        }
        protected bool IsSomethingExists<T>(T user) where T : class, IPoco, new()
        {           
            bool isExists = false;

            Dictionary<string, DAO<T>> correlation = new Dictionary<string, DAO<T>>();
            correlation.Add(typeof(Ticket).Name, _ticketDAO as DAO<T>);
            correlation.Add(typeof(Country).Name, _countryDAO as DAO<T>);
            correlation.Add(typeof(Flight).Name, _flightDAO as DAO<T>);
            correlation.Add(typeof(FailedLoginAttempt).Name, _failedLoginAttemptsDAO as DAO<T>);

            var user2 = correlation[user.GetType().Name].Get((long)user.GetType().GetProperty("ID").GetValue(user));
            if (user2.Equals(user)) isExists = true;

            return isExists;            
        }

        /// <summary>
        /// Gets registered user credentials as Utility"_clas_User object by appropriate ID,
        /// which is USER_ID property within registered user poco objects
        /// (Administrator, AirlineCompany and Customer)
        /// </summary>
        /// <param name="registeredUserId">Utility_class_user ID and also registered user USER_ID</param>
        /// <returns></returns>
        public Utility_class_User GetRegisteredUserDetails(long registeredUserId)
        {
            return _utility_Class_UserDAO.Get(registeredUserId);
        }



    }
}
