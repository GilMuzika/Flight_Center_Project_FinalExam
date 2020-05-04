
using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flight_Center_Project_FinalExam_BL
{
    class UserAlreadyExistsException<T> : Exception where T : class, IPoco, new()
    {
        public UserAlreadyExistsException(T user) : base($"the {typeof(T).Name.ToLower()} with the parameters:\n {user} \nALREADY exists in the system") { }
    }
}
