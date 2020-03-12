using System;
using System.Collections.Generic;
using System.Text;
using Flight_Center_Project_FinalExam_DAL;

namespace Flight_Center_Project_FinalExam_BL
{
    public class LoginToken<T> where T : IPoco
    {
        /// <summary>
        /// USER_ID property of "User" is "ID" of "UserAsUser"
        /// </summary>
        public T ActualUser { get; set; }
        public Utility_class_User UserAsUser { get; set; }


    }
}
