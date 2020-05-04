
using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flight_Center_Project_FinalExam_BL
{
    class UserDoesntExistsException<T> : Exception where T : class, IPoco, new()
    {
        public UserDoesntExistsException(T user) : base($"the {typeof(T).Name.ToLower()} with the parameters:\n {user} \nDON'T exists in the system") { }
    }
}
