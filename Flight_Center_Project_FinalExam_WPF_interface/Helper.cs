using System;
using System.Collections.Generic;
using System.Text;
using Flight_Center_Project_FinalExam_BL;
using Flight_Center_Project_FinalExam_DAL;

namespace Flight_Center_Project_FinalExam_WPF_interface
{
    class Helper
    {
        public static bool TryUserLoginGeneric<T>(string userName, string pasword) where T : class, IPoco, IUser, new()
        {
            LoginService<T> loginService = new LoginService<T>();
            bool flag = loginService.TryUserLogin(userName, pasword, out LoginToken<T> token);
            //token2 = token;
            return flag;
        }
    }
}
