using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Center_Project_FinalExam_DAL
{
    public class FlightDAOMSSQL<T> : DAO<T>, IFlightDAO<T> where T : Flight, IPoco, new()
    {
        public FlightDAOMSSQL(): base() { }

        /// <summary>
        /// returns dictionary with all the flights as keys and their respective numbers of remaining tickets as values
        /// </summary>
        /// <returns></returns>
        public Dictionary<Flight, int> GetAllFlightVacancty()
        {            
            Dictionary<Flight, int> allFlightVacancty = new Dictionary<Flight, int>();
            foreach(var s in this.GetAll()) allFlightVacancty.Add(s, s.REMAINING_TICKETS);
            return allFlightVacancty;
        }

        public List<Flight> GetFlightsByCustomer1(Customer customer)
        {
            try
            {
                SetAnotherDAOInstance(typeof(Utility_class_User));
                var customerAsUser = _currentUserDAOMSSQL.GetUserByIdentifier(customer);
                SetAnotherDAOInstance(typeof(AirlineCompany));
                AirlineCompany company = _currentAirlineDAOMSSQL.GetAirlineByUsername(customerAsUser.USER_NAME);                
                _connection.Open();
                List<Flight> flightsByCustomer = new List<Flight>();
                
                _command.CommandText = $"SELECT * FROM Flights WHERE AIRLINECOMPANY_ID = {company.ID}";
                using (SqlDataReader reader = _command.ExecuteReader())
                {                    
                    while (reader.Read())
                    {
                        Flight flightByCustomer = new Flight();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            typeof(Flight).GetProperties()[i].SetValue(flightByCustomer, reader.GetValue(i));
                        }
                        flightsByCustomer.Add(flightByCustomer);
                    }                    
                }
                return flightsByCustomer;

            }
            finally { _connection.Close(); }

        }
        /// <summary>
        /// getting a list of 
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public List<Flight> GetFlightsByCustomer(Customer customer)
        {
            SetAnotherDAOInstance(typeof(Ticket));
            var ticketWithThisCustomerId  = _currentTicketDAOMSSQL.GetManyBySomethingInternal(customer.ID, (int)TicketPropertyNumber.CUSTOMER_ID);
            SetAnotherDAOInstance(typeof(Flight));
            return ticketWithThisCustomerId.Select(x => _currentFlightDAOMSSQL.GetSomethingBySomethingInternal(x.FLIGHT_ID, (int)FlightPropertyNumber.ID)).ToList();
        }




        public Flight GetFlightByCustomer(Customer customer)
        {
            try
            {
                SetAnotherDAOInstance(typeof(Utility_class_User));
                var customerAsUser = _currentUserDAOMSSQL.GetUserByIdentifier(customer);
                SetAnotherDAOInstance(typeof(AirlineCompany));
                AirlineCompany company = _currentAirlineDAOMSSQL.GetAirlineByUsername(customerAsUser.USER_NAME);
                Flight flightByCustomer = GetSomethingBySomethingInternal(company.ID, (int)FlightPropertyNumber.AIRLINECOMPANY_ID);
                return flightByCustomer;
            }
            finally { _connection.Close(); }

        }

        public Flight GetFlightByDepartureDate(DateTime departureDate)
        {
            return GetSomethingBySomethingInternal(departureDate, (int)FlightPropertyNumber.DEPARTURE_TIME);
        }

        
        public List<T> GetAllFlightsThatTakeOffInSomeTimeFromNow(TimeSpan sometime)
        {
            try
            {
                List<T> toReturn = new List<T>();
                _connection.Open();
                _command.CommandType = CommandType.Text;
                var dt = DateTime.Now;
                DateTime selectionDateTime = DateTime.Now.Add(sometime);
                _command.CommandText = $"select * from {GetTableName(typeof(T))} WHERE DEPARTURE_TIME <= '{selectionDateTime}'";
                using (SqlDataReader reader = _command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        T poco = new T();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            object value = reader[i];
                            if (reader[i] is DBNull && typeof(T).GetProperties()[i].GetType().Name.ToLower().Equals("string"))
                            {
                                typeof(T).GetProperties()[i].SetValue(poco, string.Empty);
                            }
                            if (reader[i] is DBNull && typeof(T).GetProperties()[i].GetType().Name.ToLower().Contains("int"))
                            {
                                typeof(T).GetProperties()[i].SetValue(poco, 0);
                            }

                            if (!(reader[i] is DBNull)) { typeof(T).GetProperties()[i].SetValue(poco, value); }

                        }
                        toReturn.Add(poco);
                    }
                }
                return toReturn;

            }
            finally { _connection.Close(); }


        }

        public Flight getFlightByDestinationCountry(Country destinationCountry)
        {
            return GetSomethingBySomethingInternal(destinationCountry.ID, (int)FlightPropertyNumber.DESTINATION_COUNTRY_CODE);
        }

        public Flight GetFlightById(long flightID)
        {
            return Get(flightID);
        }

        public Flight GetFlightByLandingDate(DateTime landingDate)
        {
            return GetSomethingBySomethingInternal(landingDate, (int)FlightPropertyNumber.LANDING_TIME);
        }

        public Flight GetFlightByOriginCountry(Country originCountry)
        {
            return GetSomethingBySomethingInternal(originCountry.ID, (int)FlightPropertyNumber.ORIGIN_COUNTRY_CODE);
        }


    }
}
