using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flight_Center_Project_FinalExam_BL
{
    interface ILoggedAdministratorFacade: IAnonymousUserFacade
    {
        void CreateNewAirline(LoginToken<Administrator> token, AirlineCompany airline, string airlineUserName, string airlinePassword);
        void CreateNewCustomer(LoginToken<Administrator> token, Customer customer, string customeruserName, string customerPassword);
        void RemoveAirline(LoginToken<Administrator> token, AirlineCompany airline);
        void RemoveCustomer(LoginToken<Administrator> token, Customer customer);
        void UpdateAirlineDetails(LoginToken<Administrator> token, AirlineCompany airline, string airlineUserName, string airlinePassword);
        void UpdateCustomerDetails(LoginToken<Administrator> token, Customer customer, string customeruserName, string customerPassword);
    }
}
