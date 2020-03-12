using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flight_Center_Project_FinalExam_BL
{
    public class LoggedInAdministratorFacade : AnonimousUserFacade, ILoggedAdministratorFacade
    {
        public void CreateNewAirline(LoginToken<Administrator> token, AirlineCompany airline, string airlineUserName, string airlinePassword)
        {
            if (CheckToken(token))
            {
                if (!IsUserExists(airline)) _airlineDAO.Add(airline, airlineUserName, airlinePassword);
                else throw new UserAlreadyExistsException<AirlineCompany>(airline);
            }
        }

        public void CreateNewCustomer(LoginToken<Administrator> token, Customer customer, string customeruserName, string customerPassword)
        {
            if (CheckToken(token))
            {
                if (!IsUserExists(customer)) _customerDAO.Add(customer, customeruserName, customerPassword);
                else throw new UserAlreadyExistsException<Customer>(customer);
            }
        }

        public void RemoveAirline(LoginToken<Administrator> token, AirlineCompany airline)
        {
            if (CheckToken(token))
            {
                if (IsUserExists(airline)) _airlineDAO.Remove(airline);
                else throw new UserDoesntExistsException<AirlineCompany>(airline);
            }
        }

        public void RemoveCustomer(LoginToken<Administrator> token, Customer customer)
        {
            if (CheckToken(token))
            {
                if (IsUserExists(customer)) _customerDAO.Remove(customer);
                else throw new UserDoesntExistsException<Customer>(customer);
            }
        }

        public void UpdateAirlineDetails(LoginToken<Administrator> token, AirlineCompany airline, string airlineUserName, string airlinePassword)
        {
            if (CheckToken(token))
            {
                if (IsUserExists(airline)) _airlineDAO.Update(airline, airlineUserName, airlinePassword);
                else throw new UserDoesntExistsException<AirlineCompany>(airline);
            }
        }

        public void UpdateCustomerDetails(LoginToken<Administrator> token, Customer customer, string customeruserName, string customerPassword)
        {
            if (CheckToken(token))
            {
                if (IsUserExists(customer)) _customerDAO.Update(customer, customeruserName, customerPassword);
                else throw new UserDoesntExistsException<Customer>(customer);
            }
        }


    }
}
