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
    public class CountryDAOMSSQL<T> : DAO<T>, ICountryDAO<T> where T : Country, IPoco, new()
    {
        public CountryDAOMSSQL(): base() { }

    }
}
