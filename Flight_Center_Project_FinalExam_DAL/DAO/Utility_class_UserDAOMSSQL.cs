using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Center_Project_FinalExam_DAL
{
    public class Utility_class_UserDAOMSSQL<T> : UserBaseMSSQLDAO<T>, IUtility_class_UserDAO<T> where T : Utility_class_User, IPoco, new()
    {
        public Utility_class_UserDAOMSSQL() : base() { }

        public T GetUserByIdentifier2(IPoco anotherTypePoco)
        {
            List<T> users = new List<T>();
            users.Add(base.GetRegisteredUserInOneTableBySomethingInAnotherInternal(anotherTypePoco.GetType().GetProperty("USER_ID").GetValue(anotherTypePoco), (int)CustomerPropertyNumber.USER_ID, typeof(Customer)));
            users.Add(base.GetRegisteredUserInOneTableBySomethingInAnotherInternal(anotherTypePoco.GetType().GetProperty("USER_ID").GetValue(anotherTypePoco), (int)AirlineCompanyPropertyNumber.USER_ID, typeof(AirlineCompany)));
            users.Add(base.GetRegisteredUserInOneTableBySomethingInAnotherInternal(anotherTypePoco.GetType().GetProperty("USER_ID").GetValue(anotherTypePoco), (int)AdministratorPropertyNumber.USER_ID, typeof(Administrator)));
            /*users.Add(base.GetSomethingBySomethingInternal(anotherTypePoco.GetType().GetProperties()[0].GetValue(anotherTypePoco), (int)Utility_class_UserPropertyNumber.AIRLINE_ID));
            users.Add(base.GetSomethingBySomethingInternal(anotherTypePoco.GetType().GetProperties()[0].GetValue(anotherTypePoco), (int)Utility_class_UserPropertyNumber.CUSTOMER_ID));
            users.Add(base.GetSomethingBySomethingInternal(anotherTypePoco.GetType().GetProperties()[0].GetValue(anotherTypePoco), (int)Utility_class_UserPropertyNumber.ADMINISTRATOR_ID));*/

            T toReturn = null;
            foreach(var s in users)
                if (s != new T()) toReturn = s;
            
            return toReturn;
        }
        public T GetUserByIdentifier(IPoco anotherTypePoco)
        {
            return GetRegisteredUserInOneTableBySomethingInAnotherInternal(anotherTypePoco.GetType().GetProperty("USER_ID").GetValue(anotherTypePoco), anotherTypePoco.GetType().GetProperties().Length - 1, anotherTypePoco.GetType());
        }

    }
}
