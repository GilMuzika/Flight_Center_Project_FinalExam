using System;
using System.Collections.Generic;
using System.Text;

namespace Flight_Center_Project_FinalExam_BL
{
    class WrongPasswordException: Exception
    {
        public WrongPasswordException(string password) : base($"The password \"{password}\" is WRONG.") { }
    }
}
