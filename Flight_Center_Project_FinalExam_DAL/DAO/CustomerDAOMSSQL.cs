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
    public class CustomerDAOMSSQL<T> : UserBaseMSSQLDAO<T>, ICustomerDAO<T> where T : Customer, IPoco,  new()
    {
        public CustomerDAOMSSQL(): base() { }

        public Customer GetCustomerByUsername(string customerUsername)
        {
            //return GetSomethingBySomethingInternal(customerUsername, (int)CustomerPropertyNumber.USER_NAME);
            return GetRegisteredUserInOneTableBySomethingInAnotherInternal(customerUsername, (int)Utility_class_UserPropertyNumber.USER_NAME, typeof(Utility_class_User));
        }
        public Customer GetCustomerByUserID(long userID)
        {
            return base.GetSomethingBySomethingInternal(userID, (int)CustomerPropertyNumber.USER_ID);
        }
    }
}
