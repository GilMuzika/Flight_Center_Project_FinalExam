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
        private FlyingCenterSystem() 
        {
            _threadTimeDelatyMilliseconds = int.Parse(ConfigurationManager.AppSettings["ThreadDelayTime"]);

            _wakeUpAndSing = new Thread(() => 
            {
                Thread.Sleep(_threadTimeDelatyMilliseconds);
                var _currentFlightDAO = this.CreateApproptiateDAOInstance<Flight>();
                var  _currentFlightHistoryDAO = this.CreateApproptiateDAOInstance<FlightsHistory>();
                var _currentTicketsDAO = this.CreateApproptiateDAOInstance<Ticket>();
                var _currentTicketHistoryDAO = this.CreateApproptiateDAOInstance<TicketsHistory>();

                foreach(var s_Flight in _currentFlightDAO.GetAll())
                {
                    if(s_Flight.LANDING_TIME.Hour - DateTime.Now.Hour > 3)
                    {
                        FlightsHistory historyItemFlight = new FlightsHistory();
                        for(int i = 0; i < s_Flight.GetType().GetProperties().Length; i++)
                        {
                            historyItemFlight.GetType().GetProperties()[i].SetValue(historyItemFlight, s_Flight.GetType().GetProperties()[i].GetValue(s_Flight));
                        }
                        _currentFlightHistoryDAO.Add(historyItemFlight);
                        _currentFlightDAO.Remove(s_Flight);

                        Ticket ticketFromFlight = _currentTicketsDAO.GetSomethingBySomethingInternal(s_Flight.ID, (int)TicketPropertyNumber.FLIGHT_ID);
                        TicketsHistory historyItemTicket = new TicketsHistory();
                        for(int i = 0; i < ticketFromFlight.GetType().GetProperties().Length; i++)
                        {
                            historyItemTicket.GetType().GetProperties()[i].SetValue(historyItemTicket, ticketFromFlight.GetType().GetProperties()[i].GetValue(ticketFromFlight));
                        }
                        _currentTicketHistoryDAO.Add(historyItemTicket);
                        _currentTicketsDAO.Remove(ticketFromFlight);
                    }

                    
                }



            });
            _wakeUpAndSing.Start();
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
