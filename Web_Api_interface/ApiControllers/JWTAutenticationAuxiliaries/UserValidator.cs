using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Api_interface
{
    public class UserValidator
    {
        Utility_class_UserDAOMSSQL<Utility_class_User> _utility_class_UserDAO = new Utility_class_UserDAOMSSQL<Utility_class_User>();
        List<Utility_class_User> _registeredUsersLst;

        Utility_class_User _registeredUser = new Utility_class_User();

        public UserValidator()
        {
            _registeredUsersLst = _utility_class_UserDAO.GetAll();
        }


        /// <summary>
        /// If the user credantials are valid, returns true by "return" and instance of "Utility_class_User" with validated unencrypted user name nd password and user role by "out",
        /// in the "USER_NAME", "PASSWORD" and "USER_KIND" properties. Rest of the properties doesn't matter.
        /// In this methos "Utility_class_User" used differently, here it's just a data bearing model for validated user credentials.
        /// If the user credantials are not valid, returns false by "return" and null by "out".        
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <param name="validatedUserModel">instance of "Utility_class_User" class with validated unecrypted user credentials. In this context used merely as a data bearing model for validated user credentials, only properties "USER_NAME", "PASSWORD" and "USER_KIND" are matters. "USER_KIND" bears user role.</param>        
        /// <returns></returns>
        public bool ValidateUser(string username, string password, out Utility_class_User validatedUserModel)
        {
            bool isUserValid = false;
            string s_USER_NAME = string.Empty;
            string s_PASSWORD = string.Empty;

            foreach (var s in _registeredUsersLst)
            {

                if (s.USER_NAME.Length > 50) s_USER_NAME = EncryptionProvider.Decryprt(s.USER_NAME);
                else s_USER_NAME = s.USER_NAME;

                if (s.PASSWORD.Length > 50) s_PASSWORD = EncryptionProvider.Decryprt(s.PASSWORD);
                else s_PASSWORD = s.PASSWORD;


                if (username == s_USER_NAME && password == s_PASSWORD)
                {
                    _registeredUser.PASSWORD = password;
                    _registeredUser.USER_NAME = username;
                    _registeredUser.USER_KIND = s.USER_KIND;
                    isUserValid = true;
                    break;
                }                
            }
            if (!isUserValid) _registeredUser = null;
            validatedUserModel = _registeredUser;
            return isUserValid;
        }
    }
}