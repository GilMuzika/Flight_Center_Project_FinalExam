using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flight_Center_Project_FinalExam_BL
{
    public class LoggedInAdministratorFacade : AnonimousUserFacade, ILoggedAdministratorFacade
    {
        public bool CreateNewAirline(LoginToken<Administrator> token, AirlineCompany airline, string airlineUserName, string airlinePassword, out bool isAirlineExists)
        {
            bool isCreated = false;
            bool isAirlineExistsInternal = true;
            if (CheckToken(token))
            {
                if (!IsUserExists(airline))
                {
                    isAirlineExistsInternal = false;
                    long addedAirlineId = _airlineDAO.Add(airline, airlineUserName, airlinePassword);
                    AirlineCompany airlineForhecking = _airlineDAO.Get(addedAirlineId);
                    isCreated = Statics.BulletprofComparsion(airline, airlineForhecking);
                }

                else throw new UserAlreadyExistsException<AirlineCompany>(airline);
                
            }
            isAirlineExists = isAirlineExistsInternal;
            return isCreated;
        }

        public bool CreateNewCustomer(LoginToken<Administrator> token, Customer customer, string customeruserName, string customerPassword, out bool isCustomerAlreadyExists)
        {
            bool isCreated = false;
            bool isCustomerAlreadyExistsInternal = true;
            if (CheckToken(token))
            {
                if (!IsUserExists(customer))
                {
                    isCustomerAlreadyExistsInternal = false;
                    long addedCustomerId =  _customerDAO.Add(customer, customeruserName, customerPassword);
                    Customer customerForChecking = _customerDAO.Get(addedCustomerId);
                    isCreated = Statics.BulletprofComparsion(customer, customerForChecking);
                }
                else throw new UserAlreadyExistsException<Customer>(customer);
            }
            isCustomerAlreadyExists = isCustomerAlreadyExistsInternal;
            return isCreated;
        }

        public bool RemoveAirline(LoginToken<Administrator> token, AirlineCompany airline, out bool isAirlineExists)
        {
            bool isRemoved = false;
            bool IsExistsInternal = false;
            if (CheckToken(token))
            {
                if (IsUserExists(airline))
                {
                    IsExistsInternal = true;
                    _airlineDAO.Remove(airline);
                    if (!IsUserExists(airline)) isRemoved = true;
                }
                else throw new UserDoesntExistsException<AirlineCompany>(airline);
            }
            isAirlineExists = IsExistsInternal;
            return isRemoved;
        }

        public bool RemoveCustomer(LoginToken<Administrator> token, Customer customer, out bool isCustomerExists)
        {
            bool isRemoved = false;
            bool IsExistsInternal = false;
            if (CheckToken(token))
            {
                if (IsUserExists(customer))
                {
                    IsExistsInternal = true;
                    List<Ticket> ticketsOfThisCustomer = _ticketDAO.GetAll().Where(x => x.CUSTOMER_ID == customer.ID).ToList();
                    ticketsOfThisCustomer.ForEach(x => _ticketDAO.Remove(x.ID));
                  
                    _customerDAO.Remove(customer);
                    if (!IsUserExists(customer)) isRemoved = true;
                }
                else throw new UserDoesntExistsException<Customer>(customer);
            }
            isCustomerExists = IsExistsInternal;
            return isRemoved;
        }

        public bool UpdateAirlineDetails(LoginToken<Administrator> token, AirlineCompany airline, string airlineUserName, string airlinePassword, out bool isAirlineExists)
        {
            bool isUpdated = false;
            bool isAirlineExistsInternal = false;
            if (CheckToken(token))
            {
                if (IsUserExists(airline))                
                {
                    isAirlineExistsInternal = true;
                    _airlineDAO.Update(airline, airlineUserName, airlinePassword);
                    AirlineCompany airlineForChecking = _airlineDAO.Get(airline.ID);
                    isUpdated = Statics.BulletprofComparsion(airline, airlineForChecking);
                }
                else throw new UserDoesntExistsException<AirlineCompany>(airline);
            }
            isAirlineExists = isAirlineExistsInternal;
            return isUpdated;
        }

        public bool UpdateCustomerDetails(LoginToken<Administrator> token, Customer customer, string customeruserName, string customerPassword, out bool isCustomerExists)
        {
            bool isUpdated = false;
            bool isCustomerExistsInternal = false;
            if (CheckToken(token))
            {
                if (IsUserExists(customer))
                {
                    isCustomerExistsInternal = true;
                    _customerDAO.Update(customer, customeruserName, customerPassword);
                    Customer customerForChecking = _customerDAO.Get(customer.ID);
                    isUpdated = Statics.BulletprofComparsion(customer, customerForChecking);
                }
                else throw new UserDoesntExistsException<Customer>(customer);
            }
            isCustomerExists = isCustomerExistsInternal;
            return isUpdated;
        }




    }
}
