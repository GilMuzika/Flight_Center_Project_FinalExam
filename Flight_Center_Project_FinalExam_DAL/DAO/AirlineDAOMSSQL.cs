using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Center_Project_FinalExam_DAL
{
    public class AirlineDAOMSSQL<T> : UserBaseMSSQLDAO<T>, IAirlineDAO<T> where T : AirlineCompany, IPoco, IUser, new()
    {
        public AirlineDAOMSSQL(): base() {  }

        public AirlineCompany GetAirlineByCountry(Country country)
        {
            return GetSomethingBySomethingInternal(country.ID, (int)AirlineCompanyPropertyNumber.ID);
        }
        public AirlineCompany GetAirlineByUsername(string airlineUsername)
        {
            AirlineCompany company = new AirlineCompany();
            this.SetAnotherDAOInstance(typeof(Utility_class_User));
            try
            {
                _connection.Open();
                _command.CommandText = $"SELECT * FROM AirlineCompanies JOIN Utility_class_Users ON AirlineCompanies.USER_ID = Utility_class_Users.ID WHERE Utility_class_Users.USER_NAME = '{airlineUsername}'";
                using (SqlDataReader reader = _command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            typeof(AirlineCompany).GetProperties()[i].SetValue(company, reader.GetValue(i));
                        }
                    }
                }
            }
            finally
            {
                _connection.Close();
            }



            return company;
        }

        public AirlineCompany GerAirlineCompanyByUserID(long userID)
        {
            return base.GetSomethingBySomethingInternal(userID, (int)AirlineCompanyPropertyNumber.USER_ID);
        }

    }
}
