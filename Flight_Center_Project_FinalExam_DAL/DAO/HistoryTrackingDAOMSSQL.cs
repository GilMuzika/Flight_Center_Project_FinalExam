using Flight_Center_Project_FinalExam_DAL.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Center_Project_FinalExam_DAL
{
    public class HistoryTrackingDAOMSSQL<T> : DAO<T>, IHistoryTrackingDAO<T> where T : HistoryTracking, IPoco, new()
    {
        public HistoryTrackingDAOMSSQL() : base() { }
    }
}
