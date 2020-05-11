using Flight_Center_Project_FinalExam_DAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace DataBase_Generator_WPF
{
    class DataInterface<T> : IDataInterface where T : class, IPoco, new()
    {
        const string ENCRIPTION_PHRASE = "4r8rjfnklefjkljghggGKJHnif5r5242";

        //private RandomUserAPIObject _deserializedRandomUserObject = JsonConvert.DeserializeObject<RandomUserAPIObject>(Statics.ReadFromUrl("https://randomuser.me/api"));
        private string _apiUrl = "https://randomuser.me/api";

        private List<Dictionary<string, string>> _airlineCompaniesFromExternalSource = AirlineGenerator.Airlines;
        private List<Country> _countries;
        private List<AirlineCompany> _airlineCompanies;
        private List<Flight> _flights;
        private List<Customer> _customers;
        private FlightDAOMSSQL<Flight> _flightDAO = new FlightDAOMSSQL<Flight>();
        private CountryDAOMSSQL<Country> _countryDAO = new CountryDAOMSSQL<Country>();
        private AirlineDAOMSSQL<AirlineCompany> _airlineCompanyDAO = new AirlineDAOMSSQL<AirlineCompany>();
        private CustomerDAOMSSQL<Customer> _customerDAO = new CustomerDAOMSSQL<Customer>();

        private Regex _regex = new Regex("('|\"|„|”|«|»)");

        private List<Type> _utility_user_types = new List<Type>();
        private WebClient _webClient = new WebClient();
        private Random _rnd = new Random();

        private UserBaseMSSQLDAO<T> _currentWithUtility_clas_UserDAO;
        private DAO<T> _currentDAO;


        private delegate DAO<T> createAppropriateDAOInstance();
        private delegate UserBaseMSSQLDAO<T> createAppropriateDAOInstanceWithUlitity_class_User();
        private DAO<T> CreateAppropriateDAO()
        {

            Dictionary<Type, createAppropriateDAOInstance> pocoDaoCorrelation = new Dictionary<Type, createAppropriateDAOInstance>();
            pocoDaoCorrelation.Add(typeof(Country), () => { return new CountryDAOMSSQL<Country>() as DAO<T>; });
            pocoDaoCorrelation.Add(typeof(Flight), () => { return new FlightDAOMSSQL<Flight>() as DAO<T>; });
            pocoDaoCorrelation.Add(typeof(Ticket), () => { return new TicketDAOMSSQL<Ticket>() as DAO<T>; });


            return pocoDaoCorrelation[typeof(T)]();

        }
        private UserBaseMSSQLDAO<T> CreateAppropriateDAO_WithUtility_class_User()
        {
            Dictionary<Type, createAppropriateDAOInstanceWithUlitity_class_User> pocoDaoCorrelation = new Dictionary<Type, createAppropriateDAOInstanceWithUlitity_class_User>();
            pocoDaoCorrelation.Add(typeof(Administrator), () => { return new AdministratorDAOMSSQL<Administrator>() as UserBaseMSSQLDAO<T>; });
            pocoDaoCorrelation.Add(typeof(AirlineCompany), () => { return new AirlineDAOMSSQL<AirlineCompany>() as UserBaseMSSQLDAO<T>; });
            pocoDaoCorrelation.Add(typeof(Customer), () => { return new CustomerDAOMSSQL<Customer>() as UserBaseMSSQLDAO<T>; });
            pocoDaoCorrelation.Add(typeof(Utility_class_User), () => { return new Utility_class_UserDAOMSSQL<Utility_class_User>() as UserBaseMSSQLDAO<T>; });
            return pocoDaoCorrelation[typeof(T)]();
        }


        private List<string> GetPropertyKeysForDynamic(dynamic dynamicToGetPropertiesFor)
        {
            JObject attributesAsJObject = dynamicToGetPropertiesFor;
            Dictionary<string, object> values = dynamicToGetPropertiesFor.ToObject<Dictionary<string, object>>();
            List<string> toReturn = new List<string>();
            foreach (string key in values.Keys)
            {
                toReturn.Add(key);
            }
            return toReturn;
        }
        
        public DataInterface()
        {            
            Initialize();
        }
        private Dictionary<Type, Action<long, long, long>> _genericTypeMethodCorrelation = new Dictionary<Type, Action<long, long, long>>();
        /// <summary>
        /// method that selects the appropriate method for adding a a database member depending on the this class Generic parameter ("T")
        /// </summary>
        private void Initialize()
        {
            _countries = _countryDAO.GetAll();
            _airlineCompanies = _airlineCompanyDAO.GetAll();
            _flights = _flightDAO.GetAll();
            _customers = _customerDAO.GetAll();

            _utility_user_types.Add(typeof(Administrator));
            _utility_user_types.Add(typeof(Customer));
            _utility_user_types.Add(typeof(AirlineCompany));



            _genericTypeMethodCorrelation.Add(typeof(Customer), AddCustomers);
            _genericTypeMethodCorrelation.Add(typeof(Administrator), AddAdministrators);
            _genericTypeMethodCorrelation.Add(typeof(Flight), AddFlights);
            _genericTypeMethodCorrelation.Add(typeof(Country), this.AddCountries);
            _genericTypeMethodCorrelation.Add(typeof(AirlineCompany), this.AddAirlineCompanies);
            _genericTypeMethodCorrelation.Add(typeof(Ticket), this.AddTickets);
            //add more "Add["type_name"]" functions to the dictionary when they created
        }
        private void GenerateUtility_class_UserPasswordAndName(out string nameCrypt, out string passwordCrypt)
        {
            string nameForEncription = Statics.GetUniqueKeyOriginal_BIASED(_rnd.Next(5, 8));
            string passForEncription = Statics.GetUniqueKeyOriginal_BIASED(_rnd.Next(5, 15));

            string encryptedName = Statics.Encrypt(nameForEncription, ENCRIPTION_PHRASE);
            string encryptedPassword = Statics.Encrypt(passForEncription, ENCRIPTION_PHRASE);
            nameCrypt = encryptedName;
            passwordCrypt = encryptedPassword;
        }

        public void Add(long from, long to, long fixedNumber)
        {           
            _genericTypeMethodCorrelation[typeof(T)](from, to, fixedNumber);
        }

        //all the "Add["type_name"]" functions must came with the same signature
        private void AddCustomers(long from, long to, long fixedNumber) //this function don't use the parameter "fixedNumber" but it must be there becauase of signature uniformity
        {
            int randomCallingsNum = _rnd.Next(Convert.ToInt32(from), Convert.ToInt32(to));

            for (long i = 0; i < randomCallingsNum; i++) AddOneCustomer();            
        }
        private void AddOneCustomer()
        {
            RandomUserAPIObject deserializedRandomUserObject = JsonConvert.DeserializeObject<RandomUserAPIObject>(Statics.ReadFromUrl(_apiUrl));            

            Customer customer = new Customer();
            customer.FIRST_NAME = deserializedRandomUserObject.results[0].name.first;
            customer.LAST_NAME = deserializedRandomUserObject.results[0].name.last;
            customer.PHONE_NO = deserializedRandomUserObject.results[0].phone;
            customer.ADDRESS = $"{deserializedRandomUserObject.results[0].location.city}, {deserializedRandomUserObject.results[0].location.street} st, {deserializedRandomUserObject.results[0].location.coordinates.latitude} / {deserializedRandomUserObject.results[0].location.coordinates.longitude}";
            customer.CREDIT_CARD_NUMBER = Statics.Encrypt(Statics.DashingString(Statics.GetUniqueKeyOriginal_BIASED(20, Charset.OnlyNumber), 4), ENCRIPTION_PHRASE);
            customer.IMAGE = ImageProvider.GetResizedCustomerImageAs64BaseString(4);


            _currentWithUtility_clas_UserDAO = CreateAppropriateDAO_WithUtility_class_User();
            GenerateUtility_class_UserPasswordAndName(out string nameCrypt, out string passsCrypt);
            _currentWithUtility_clas_UserDAO.Add(customer as T, nameCrypt, passsCrypt);
        }
        private void AddAdministrators(long from, long to, long fixedNumber) //this function don't use the parameters "from" and "to" but they must be there becauase of signature uniformity
        {            
            for (int i = 0; i < fixedNumber; i++) AddOneAdministrator();
        }
        private void AddOneAdministrator()
        {
            RandomUserAPIObject deserializedRandomUserObject = JsonConvert.DeserializeObject<RandomUserAPIObject>(Statics.ReadFromUrl(_apiUrl));

            Administrator administrator = new Administrator();
            administrator.NAME = $"{deserializedRandomUserObject.results[0].name.first} {deserializedRandomUserObject.results[0].name.last}";

            _currentWithUtility_clas_UserDAO = CreateAppropriateDAO_WithUtility_class_User();
            GenerateUtility_class_UserPasswordAndName(out string nameCrypt, out string passsCrypt);
            _currentWithUtility_clas_UserDAO.Add(administrator as T, nameCrypt, passsCrypt);


            var r = deserializedRandomUserObject.results[0].picture.thumbnail;
        }
        private void AddFlights(long from, long to, long fixedNumber) //this function don't use the parameters "from" and "to" but they must be there becauase of signature uniformity
        {
            for (int i = 0; i < fixedNumber; i++) AddOneFlight();
        }
        private void AddOneFlight()
        {              
            Flight flight = new Flight();
            AirlineCompany randomAirline = _airlineCompanies[_rnd.Next(_airlineCompanies.Count)];
            flight.AIRLINECOMPANY_ID = randomAirline.ID;
            flight.ORIGIN_COUNTRY_CODE = _countries[_rnd.Next(_countries.Count)].ID;
            flight.DESTINATION_COUNTRY_CODE = _countries[_rnd.Next(_countries.Count)].ID;
            
            var departureDateTime = Statics.GetRandomDate(DateTime.Now, DateTime.Now.AddHours(Convert.ToDouble(_rnd.Next(1, 3))).AddMinutes(Convert.ToDouble(_rnd.Next(10, 55))));
            flight.DEPARTURE_TIME = departureDateTime;
            //flight.LANDING_TIME = Statics.GetRandomDate(flight.DEPARTURE_TIME, new DateTime(flight.DEPARTURE_TIME.Year, flight.DEPARTURE_TIME.Month, flight.DEPARTURE_TIME.Day, flight.DEPARTURE_TIME.Hour + _rnd.Next(0, 24 - flight.DEPARTURE_TIME.Hour), 0, 0));
            flight.LANDING_TIME = Statics.GetRandomDate(flight.DEPARTURE_TIME, flight.DEPARTURE_TIME.AddHours(_rnd.Next(18, 20)).AddMinutes(_rnd.Next(5, 45)));
            flight.REMAINING_TICKETS = _rnd.Next(0, 500);

            _currentDAO = CreateAppropriateDAO();
            _currentDAO.Add(flight as T);

        }      
        private void AddCountries(long from, long to, long fixedNumber) //this function don't use none of it's parameters, but they still need to be here because of the signature uniformity
        {
            AddCountries();       
        }
        /// <summary>
        /// Fill the Countries table from the Database basing on the AirlineCompanies data source
        /// </summary>
        private void AddCountries()
        {                                 
            _currentDAO = CreateAppropriateDAO();
            _currentDAO.DeleteAllNotRegardingForeignKeys();

            List<string> countries_names = new List<string>();
            foreach(var s_airline in _airlineCompaniesFromExternalSource)
            {
                s_airline.TryGetValue("country", out string s_airline_country_name);
                if (!countries_names.Contains(s_airline_country_name)) countries_names.Add(s_airline_country_name);
            }


            foreach (var s_country_name in countries_names)
            {                
                Country country = new Country();
                country.COUNTRY_NAME = _regex.Replace(s_country_name, string.Empty);                               
                country.COUNTRY_IDENTIFIER = Convert.ToInt64(Statics.GetUniqueKeyOriginal_BIASED(_rnd.Next(4, 9), Charset.OnlyNumber));
                _currentDAO.Add(country as T);
            }
            
        }
        
        private void AddAirlineCompanies(long from, long to, long fixedNumber) //this function don't use the parameter "fixedNumber" but it must be there becauase of signature uniformity
        {
            int randomCallingsNum = _rnd.Next(Convert.ToInt32(from), Convert.ToInt32(to));
            List<long> indexses = new List<long>();

            for (long i = 0; i < randomCallingsNum; i++) indexses.Add(i);

            indexses = Statics.ShuffleList(indexses);

            indexses.ForEach(x => AddOneAirlineCompany(Convert.ToInt32(x)));
        }
        private void AddOneAirlineCompany(int index)
        {
            AirlineCompany airline = new AirlineCompany();
            airline.AIRLINE_NAME = _regex.Replace(_airlineCompaniesFromExternalSource[index]["name"], string.Empty);
            airline.IMAGE = ImageProvider.GetResizedAirlineImageAs64BaseString(4);
            airline.ADORNING = _airlineCompaniesFromExternalSource[index]["adorning"] + " " + Statics.GetUniqueKeyOriginal_BIASED(_rnd.Next(2, 4), Charset.OnlyNumber);
            string airline_country_name = _regex.Replace(_airlineCompaniesFromExternalSource[index]["country"], string.Empty);
            foreach (var s_country in _countries)
            {
                if (airline_country_name.Equals(s_country.COUNTRY_NAME))
                    airline.COUNTRY_CODE = s_country.ID;
            }

            _currentWithUtility_clas_UserDAO = CreateAppropriateDAO_WithUtility_class_User();
            GenerateUtility_class_UserPasswordAndName(out string nameCrypt, out string passsCrypt);
            _currentWithUtility_clas_UserDAO.Add(airline as T, nameCrypt, passsCrypt);

        }
        private void AddTickets(long from, long to, long fixedNumber) //this function don't use the parameter "fixedNumber" but it must be there becauase of signature uniformity
        {
            int randomCallingsNum = _rnd.Next(Convert.ToInt32(from), Convert.ToInt32(to));
            List<long> indexses = new List<long>();

            for (long i = 0; i < randomCallingsNum; i++) indexses.Add(i);

            indexses = Statics.ShuffleList(indexses);

            indexses.ForEach(x => AddOneTicket());
        }
        private void AddOneTicket()
        {
            Ticket ticket = new Ticket();
            Customer randomCustomer = _customers[_rnd.Next(_customers.Count)];
            Flight randomFlight = _flights[_rnd.Next(_flights.Count)];

            ticket.CUSTOMER_ID = randomCustomer.ID;
            ticket.FLIGHT_ID = randomFlight.ID;

            _currentDAO = CreateAppropriateDAO();
            _currentDAO.Add(ticket as T);
        }






    }








}
