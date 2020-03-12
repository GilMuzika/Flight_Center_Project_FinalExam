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
    public class TicketHistoryDAOMSSQL<T> : DAO<T>, ITicketDAO<T> where T : TicketsHistory, IPoco, new()
    {
        public TicketHistoryDAOMSSQL(): base() { }

    }
}
