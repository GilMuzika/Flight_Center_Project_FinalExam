using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flight_Center_Project_FinalExam_BL
{
    interface ILoggedAdministratorFacade: IAnonymousUserFacade
    {
        bool CreateNewAirline(LoginToken<Administrator> token, AirlineCompany airline, string airlineUserName, string airlinePassword, out bool isAirlineExists);
        bool CreateNewCustomer(LoginToken<Administrator> token, Customer customer, string customeruserName, string customerPassword, out bool isCustomerExists);
        bool RemoveAirline(LoginToken<Administrator> token, AirlineCompany airline, out bool isAirlineExists);
        bool RemoveCustomer(LoginToken<Administrator> token, Customer customer, out bool isCustomerExists);
        bool UpdateAirlineDetails(LoginToken<Administrator> token, AirlineCompany airline, string airlineUserName, string airlinePassword, out bool isAirlineExists);
        bool UpdateCustomerDetails(LoginToken<Administrator> token, Customer customer, string customeruserName, string customerPassword, out bool isCustomerExists);
    }
}
