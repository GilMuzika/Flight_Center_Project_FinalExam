using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading;

namespace Flight_Center_Project_FinalExam_BL
{
    public class FlyingCenterSystem
    {
        private int _threadTimeDelatyMilliseconds;
        private readonly Thread _wakeUpAndSing;


        private static object _key = new object();
        private static FlyingCenterSystem Instance;
        /// <summary>
        /// Singleton - private constructor, 
        /// acsess the clas by it's name and the method GetInstance instead.
        /// </summary>
        private FlyingCenterSystem() 
        {
            try
            {
                _threadTimeDelatyMilliseconds = int.Parse(ConfigurationManager.AppSettings["ThreadDelayTime"]);
            }
            catch(ArgumentNullException)
            {
                _threadTimeDelatyMilliseconds = 86400000;
            }

            _wakeUpAndSing = new Thread(() => 
            {
                Thread.Sleep(_threadTimeDelatyMilliseconds);
                var _currentFlightDAO = this.CreateApproptiateDAOInstance<Flight>();
                var  _currentFlightHistoryDAO = this.CreateApproptiateDAOInstance<FlightsHistory>();
                var _currentTicketsDAO = this.CreateApproptiateDAOInstance<Ticket>();
                var _currentTicketHistoryDAO = this.CreateApproptiateDAOInstance<TicketsHistory>();
                var _currentHistoryTrackingDAO = this.CreateApproptiateDAOInstance<HistoryTracking>();

                foreach(var s_Flight in _currentFlightDAO.GetAll())
                {
                    if(s_Flight.LANDING_TIME.Hour - DateTime.Now.Hour > 4)
                    {
                        FlightsHistory historyItemFlight = new FlightsHistory();
                        for(int i = 0; i < s_Flight.GetType().GetProperties().Length; i++)
                        {
                            historyItemFlight.GetType().GetProperties()[i].SetValue(historyItemFlight, s_Flight.GetType().GetProperties()[i].GetValue(s_Flight));
                        }
                        _currentFlightHistoryDAO.Add(historyItemFlight);

                        HistoryTracking thisFlightTrackingInfo = new HistoryTracking();
                        thisFlightTrackingInfo.HISTORY_ENTRY_ID = s_Flight.ID;
                        thisFlightTrackingInfo.HISTORY_ENTRY_KIND = s_Flight.GetType().Name;
                        thisFlightTrackingInfo.HISTORY_ENTRY_TIME = DateTime.UtcNow;

                        _currentHistoryTrackingDAO.Add(thisFlightTrackingInfo);

                        _currentFlightDAO.Remove(s_Flight);

                        Ticket ticketFromFlight = _currentTicketsDAO.GetSomethingBySomethingInternal(s_Flight.ID, (int)TicketPropertyNumber.FLIGHT_ID);
                        TicketsHistory historyItemTicket = new TicketsHistory();
                        for(int i = 0; i < ticketFromFlight.GetType().GetProperties().Length; i++)
                        {
                            historyItemTicket.GetType().GetProperties()[i].SetValue(historyItemTicket, ticketFromFlight.GetType().GetProperties()[i].GetValue(ticketFromFlight));
                        }
                        _currentTicketHistoryDAO.Add(historyItemTicket);

                        HistoryTracking thisTicketTrackingInfo = new HistoryTracking();
                        thisTicketTrackingInfo.HISTORY_ENTRY_ID = ticketFromFlight.ID;
                        thisTicketTrackingInfo.HISTORY_ENTRY_KIND = ticketFromFlight.GetType().Name;
                        thisTicketTrackingInfo.HISTORY_ENTRY_TIME = DateTime.UtcNow;

                        _currentHistoryTrackingDAO.Add(thisTicketTrackingInfo);

                        _currentTicketsDAO.Remove(ticketFromFlight);
                    }

                    
                }



            });
            _wakeUpAndSing.Start();
        }
        //loginService
        private LoginService<PocoBase> _loginService = new LoginService<PocoBase>();

        /// <summary>
        /// creating an appropriate facade basing on the logged user type
        /// </summary>
        /// <param name="type">Logged user type</param>
        /// <returns></returns>
        public IAnonymousUserFacade GetProperFacade(Type type)
        {
            IAnonymousUserFacade facade = null;
            Dictionary<Type, Action<Type>> correlation = new Dictionary<Type, Action<Type>>();
            correlation.Add(typeof(Customer), (Customer) => { facade = this.getFacede<LoggedInCustomerFacade>(); });
            correlation.Add(typeof(AirlineCompany), (AirlineCompany) => { facade = this.getFacede<LoggedInAirlineFacade>(); });
            correlation.Add(typeof(Administrator), (Administrator) => { facade = this.getFacede<LoggedInAdministratorFacade>(); });

            correlation[type](type);
            return facade;
        }
        /// <summary>
        /// returns AnonimousFacade for not logged anonimous user. 
        /// </summary>
        /// <returns></returns>
        public AnonimousUserFacade GetAnonimousFacade()
        {
            return this.getFacede<AnonimousUserFacade>();
        }

        public LoginService<T> GetLoginService<T>(T registeredUserPocoType) where T : class, IPoco, new()
        {                              
            return new LoginService<T>();
        }



        public static FlyingCenterSystem GetInstance()
        {
            if (Instance == null)
            {
                lock (_key)
                {
                    if(Instance == null)
                    {
                        Instance = new FlyingCenterSystem(); 
                    }
                }                
            }

            return Instance;
        }

        public T getFacede<T>() where T : class, IAnonymousUserFacade, new()
        {
            return new T();
        }
        private DAO<T> CreateApproptiateDAOInstance<T>() where T : class, IPoco, new()
        {
            Dictionary<Type, Func<DAO<T>>> correlation = new Dictionary<Type, Func<DAO<T>>>();

            correlation.Add(typeof(Flight), () => { return new FlightDAOMSSQL<Flight>() as DAO<T>; });
            correlation.Add(typeof(FlightsHistory), () => { return new FlightHistoryDAOMSSQL<FlightsHistory>() as DAO<T>; });
            correlation.Add(typeof(Ticket), () => { return new TicketDAOMSSQL<Ticket>() as DAO<T>; });
            correlation.Add(typeof(TicketsHistory), () => { return new TicketHistoryDAOMSSQL<TicketsHistory>() as DAO<T>; });

            return correlation[typeof(T)]();
        }





    }
}
