using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flight_Center_Project_FinalExam_BL
{
    interface ILoginService<T> where T : class, IUser, IPoco
    {
        //bool TryAdminLogin(string userName, string password, out LoginToken<Administrator> token);
        //bool TryAirlineLogin(string userName, string password, out LoginToken<AirlineCompany> token);
        bool TryUserLogin(string userName, string password, out LoginToken<T> token);
    }
}
