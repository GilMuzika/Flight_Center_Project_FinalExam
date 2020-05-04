using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Api_interface
{
    public class Utility_class_UserRepository : IDisposable
    {
        //encripting phrase for encripting and decripting usernames and passwords
        //const string ENCRIPTION_PHRASE = "4r8rjfnklefjkljghggGKJHnif5r5242";

        //
        Utility_class_UserDAOMSSQL<Utility_class_User> _utility_class_UserDAO = new Utility_class_UserDAOMSSQL<Utility_class_User>();
        List<Utility_class_User> _registeredUsersLst;

        Utility_class_User _registeredUser = new Utility_class_User();

        public Utility_class_UserRepository()
        {
            _registeredUsersLst = _utility_class_UserDAO.GetAll();
        }


        public Utility_class_User ValidateUser(string username, string password)
        {
            
            foreach (var s in _registeredUsersLst)
            {
                if (s.USER_NAME.Length > 50 && s.PASSWORD.Length > 50)
                {
                    if (username == EncryptionProvider.Decryprt(s.USER_NAME) && password == EncryptionProvider.Decryprt(s.PASSWORD))
                    {
                        _registeredUser.PASSWORD = password;
                        _registeredUser.USER_NAME = username;
                        _registeredUser.USER_KIND = s.USER_KIND;
                        break;
                    }
                }
            }
            return _registeredUser;
        }




        public void Dispose()
        {
            _registeredUser.Dispose();
        }





    }
}