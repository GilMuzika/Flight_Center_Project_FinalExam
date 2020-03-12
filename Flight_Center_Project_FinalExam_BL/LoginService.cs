using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flight_Center_Project_FinalExam_BL
{
    public class LoginService<T> : ILoginService<T> where T : class, IUser, IPoco, new()
    {
        private UserBaseMSSQLDAO<T> _currentUserBaseMSSQLDAO = new UserBaseMSSQLDAO<T>();
        private Utility_class_UserDAOMSSQL<Utility_class_User> _currentUtility_Class_UserDAOMSSQL = new Utility_class_UserDAOMSSQL<Utility_class_User>();
        private AdministratorDAOMSSQL<Administrator> _currentAdministratorDAOMSSQL = new AdministratorDAOMSSQL<Administrator>();
        private CustomerDAOMSSQL<Customer> _currentCustomerDAOMSSQL = new CustomerDAOMSSQL<Customer>();
        private AirlineDAOMSSQL<AirlineCompany> _currentAirlineDAOMSSQL = new AirlineDAOMSSQL<AirlineCompany>();
        //private Utility_class_UserDAOMSSQL<Utility_class_User> _currentUserBaseMSSQLDAO = new Utility_class_UserDAOMSSQL<Utility_class_User>();

        public bool TryUserLogin(string userName, string password, out LoginToken<T> token)
        {
            bool isUserExists = false;
            LoginToken<T> loginToken = null;
            //List<Utility_class_User> allTheusers = (_currentUserBaseMSSQLDAO as Utility_class_UserDAOMSSQL<Utility_class_User>).GetAll();
            List<Utility_class_User> allTheusers = _currentUtility_Class_UserDAOMSSQL.GetAll();

            Dictionary<string, Func<Utility_class_User, IUser>> correlation = new Dictionary<string, Func<Utility_class_User, IUser>>();
            correlation.Add(typeof(Administrator).Name, (utility_user) => {  return _currentAdministratorDAOMSSQL.GetAdministratorByUserID(utility_user.ID); });
            correlation.Add(typeof(Customer).Name, (utility_user) => { return _currentCustomerDAOMSSQL.GetCustomerByUserID(utility_user.ID); });
            correlation.Add(typeof(AirlineCompany).Name, (utility_user) => { return _currentAirlineDAOMSSQL.GerAirlineCompanyByUserID(utility_user.ID); });
            foreach(var s in allTheusers)
            {
                if (userName == s.USER_NAME)
                {
                    isUserExists = true;
                    if (password == s.PASSWORD)
                    {
                        var actualUser = correlation[s.USER_KIND](s);
                        loginToken = new LoginToken<T>();
                        loginToken.ActualUser = actualUser as T;
                        loginToken.UserAsUser = s;
                    }
                    else throw new WrongPasswordException(password);
                } 
            }
            if (!isUserExists) throw new UserNotFoundException(userName);
            token = loginToken;
            return isUserExists;
        }

    }
}
