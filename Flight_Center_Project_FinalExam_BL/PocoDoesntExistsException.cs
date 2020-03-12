
using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flight_Center_Project_FinalExam_BL
{
    class PocoDoesntExistsException<T> : Exception where T : class, IPoco, new()
    {
        public PocoDoesntExistsException(T poco) : base($"the {typeof(T).Name.ToLower()} with the parameters:\n {poco} \nDOESN'T exists in the system") { }
    }
}
