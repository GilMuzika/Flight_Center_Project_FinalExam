
using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flight_Center_Project_FinalExam_BL
{
    class UserNotFoundException: Exception
    {
        public UserNotFoundException(string username) : base($"The user with the parameters:\n{username}\n is NOT FOUND") { }
    }
}
