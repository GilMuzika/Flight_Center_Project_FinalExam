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
    public class TicketDAOMSSQL<T> : DAO<T>, ITicketDAO<T> where T : Ticket, IPoco, new()
    {
        public TicketDAOMSSQL(): base() { }

        public Ticket GetByFlightID(long flightId)
        {
            return GetSomethingBySomethingInternal(flightId, (int)TicketPropertyNumber.FLIGHT_ID);
        }

    }
}
