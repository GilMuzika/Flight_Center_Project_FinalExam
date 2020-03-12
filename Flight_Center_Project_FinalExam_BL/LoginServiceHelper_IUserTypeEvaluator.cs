using System;
using System.Collections.Generic;
using System.Text;
using Flight_Center_Project_FinalExam_DAL;

namespace Flight_Center_Project_FinalExam_BL
{
    public class LoginServiceHelper_IUserTypeEvaluator
    {       
        /// <summary>
        /// This method has much of functionality of LoginService.TryUserLogin(string userName, string password, out LoginToken<T> token)     
        /// </summary>
        /// <returns></returns>
        public static Type Evaluate(string userName, string password)
        {
            Dictionary<string, Type> iUserTypeCorrelation = new Dictionary<string, Type>();
            iUserTypeCorrelation.Add(typeof(Customer).Name, typeof(Customer));
            iUserTypeCorrelation.Add(typeof(Administrator).Name, typeof(Administrator));
            iUserTypeCorrelation.Add(typeof(Utility_class_User).Name, typeof(Utility_class_User));

            List<Utility_class_User> allTheusers = new Utility_class_UserDAOMSSQL<Utility_class_User>().GetAll();

            Type type = null;
            foreach (var s in allTheusers)
            {
                if (userName == s.USER_NAME)
                {
                    if (password == s.PASSWORD)
                    {
                        type = iUserTypeCorrelation[s.USER_KIND];

                    }
                    else throw new WrongPasswordException(password);
                }
            }
            return type;
        }




    }




}
