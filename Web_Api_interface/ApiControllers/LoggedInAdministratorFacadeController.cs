using Flight_Center_Project_FinalExam_BL;
using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Windows;

namespace Web_Api_interface.Controllers
{
    [RoutePrefix("api/LoggedInAdministratorFacade")]
    public class LoggedInAdministratorFacadeController : ApiController
    {
        private Random _rnd = new Random();
        private FlyingCenterSystem _fsc = FlyingCenterSystem.GetInstance();

        private LoggedInAdministratorFacade _loggedInAdministratorFacade;

        public LoggedInAdministratorFacadeController()
        {
            _loggedInAdministratorFacade = _fsc.getFacede<LoggedInAdministratorFacade>();
        }

        #region Private Methods

        private bool GetInternalLoginTokenInternal<T>(out LoginToken<T> loginToken) where T : class, IPoco, new()
        {
            bool isAuthorized = false;
            LoginToken<T> loginTokenInternal = null;
            Action act = () =>
            {
                var identity = (ClaimsIdentity)User.Identity;
                var role = identity.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).FirstOrDefault();
                var password = identity.Claims.Where(x => x.Type == "Password").Select(x => x.Value).FirstOrDefault();
                var username = identity.Claims.Where(x => x.Type == ClaimTypes.Name).Select(x => x.Value).FirstOrDefault();

                LoginService<T> loginService = _fsc.GetLoginService(new T());
                isAuthorized = loginService.TryUserLogin(username, password, out LoginToken<T> loginTokenInternalInternal);
                loginTokenInternal = loginTokenInternalInternal;
            };
            ProcessExceptions(act);
            loginToken = loginTokenInternal;
            return isAuthorized;
        }

        private void ProcessExceptions(Action act)
        {
            try
            {
                act();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.GetType().Name}\n\n{ex.Message}\n\n\n{ex.StackTrace}");
            }
        }

        private void GenerateUtility_class_UserPasswordAndName(out string nameCrypt, out string passwordCrypt)
        {
            string nameForEncription = Statics.GetUniqueKeyOriginal_BIASED(_rnd.Next(5, 8));
            string passForEncription = Statics.GetUniqueKeyOriginal_BIASED(_rnd.Next(5, 15));

            string encryptedName = EncryptionProvider.Encrypt(nameForEncription);                
            string encryptedPassword = EncryptionProvider.Encrypt(passForEncription);            
            nameCrypt = encryptedName;
            passwordCrypt = encryptedPassword;
        }

        #endregion
        #region Public Methods

        [Route("CreateNewAirline", Name = "CreateNewAirline")]
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult CreateNewAirline([FromBody] AirlineCompany airline)
        {
            bool isAuthorized = false;
            bool isCreated = false;
            bool isAirlineAlreadyExists = false;
            Action act = () =>
            {
                isAuthorized = GetInternalLoginTokenInternal<Administrator>(out LoginToken<Administrator> loginTokenAdministrator);

                if (isAuthorized)
                {
                    GenerateUtility_class_UserPasswordAndName(out string nameCrypt, out string passCrypt);
                    isCreated = _loggedInAdministratorFacade.CreateNewAirline(loginTokenAdministrator, airline, nameCrypt, passCrypt, out isAirlineAlreadyExists);
                }
            };
            ProcessExceptions(act);
            if (!isAuthorized) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, $"Sorry, but you're not an Administrator. Your accsess is denied."));

            if(isAirlineAlreadyExists) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, $"This Airline didn't added to the system because it's already exists in it."));

            if(!isCreated) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, $"Sorry, but this Airline didn't added to the system"));

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, "The Airline has been added to the system sucsessfully"));
        }

        [Route("UpdayeAirlineDetails", Name = "UpdayeAirlineDetails")]
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult UpdayeAirlineDetails([FromBody]AirlineCompany airline)
        {
            bool isAuthorized = false;
            bool isUpdated = false;
            bool isAirlineExists = false;
            Action act = () =>
            {
                isAuthorized = GetInternalLoginTokenInternal<Administrator>(out LoginToken<Administrator> loginTokenAdministrator);

                if (isAuthorized)
                {
                    Utility_class_User airlineAsUser = _loggedInAdministratorFacade.GetRegisteredUserDetails(airline.USER_ID);
                    isUpdated = _loggedInAdministratorFacade.UpdateAirlineDetails(loginTokenAdministrator, airline, airlineAsUser.USER_NAME, airlineAsUser.PASSWORD, out isAirlineExists);
                }
            };
            ProcessExceptions(act);
            if (!isAuthorized) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, $"Sorry, but you're not an Administrator. Your accsess is denied."));

            if (!isAirlineExists) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Sorry, but this Airline doesn't exists in the system"));

            if (!isUpdated) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotModified, $"Sorry, but this Airline (number {airline.ID}) didn't modified"));

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, $"The details of the Airline (number {airline.ID}) was updated sucsessfully."));
        }

        [Route("RemoveAirline", Name = "RemoveAirline")]
        [HttpDelete]
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult RemoveAirline([FromBody]AirlineCompany airline)
        {
            bool isAuthorized = false;
            bool isRemoved = false;
            bool isAirlineExists = false;
            Action act = () =>
            {
                isAuthorized = GetInternalLoginTokenInternal<Administrator>(out LoginToken<Administrator> loginTokenAdministrator);

                if (isAuthorized)
                {
                    isRemoved = _loggedInAdministratorFacade.RemoveAirline(loginTokenAdministrator, airline, out isAirlineExists);
                }
            };
            ProcessExceptions(act);
            if (!isAuthorized) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, $"Sorry, but you're not an Administrator. Your accsess is denied."));

            if (!isAirlineExists) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, $"THis Airline can't be removed because it isn't exists in the systen in the first place"));

            if(!isRemoved) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, $"Sorry, but this Airline(number {airline.ID}) didn't removed."));

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, $"The Airline number {airline.ID} has been removed sucsessfully."));
        }

        [Route("Administrator", Name = "Administrator")]
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult CreateNewCustomer([FromBody] Customer customer)
        {
            bool isAuthorized = false;
            bool isCreated = false;
            bool isAirlineExists = false;
            Action act = () =>
            {
                isAuthorized = GetInternalLoginTokenInternal<Administrator>(out LoginToken<Administrator> loginTokenAdministrator);

                if (isAuthorized)
                {
                    GenerateUtility_class_UserPasswordAndName(out string nameCrypt, out string passCrypt);
                    isCreated = _loggedInAdministratorFacade.CreateNewCustomer(loginTokenAdministrator, customer, nameCrypt, passCrypt, out isAirlineExists);
                }
            };
            ProcessExceptions(act);
            if (!isAuthorized) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, $"Sorry, but you're not an Administrator. Your accsess is denied."));

            if (isAirlineExists) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, $"Such a customer (number {customer.ID}) can't be created because it's already exists in the system."));

            if (!isCreated) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, $"Sorry but the customer with the number {customer.ID} didn't created"));

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, $"The customer number {customer.ID} has been created sucsessfully."));
        }

        [Route("UpdateCustomerDetails", Name = "UpdateCustomerDetails")]
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult UpdateCustomerDetails([FromBody]Customer customer)
        {
            bool isAuthorized = false;
            bool isUpdated = false;
            bool isCustomerExists = false;
            Action act = () =>
            {
                isAuthorized = GetInternalLoginTokenInternal<Administrator>(out LoginToken<Administrator> loginTokenAdministrator);

                if (isAuthorized)
                {
                    Utility_class_User customerAsUser = _loggedInAdministratorFacade.GetRegisteredUserDetails(customer.USER_ID);
                    isUpdated = _loggedInAdministratorFacade.UpdateCustomerDetails(loginTokenAdministrator, customer, customerAsUser.USER_NAME, customerAsUser.PASSWORD, out isCustomerExists);
                }
            };
            ProcessExceptions(act);
            if (!isAuthorized) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, $"Sorry, but you're not an Administrator. Your accsess is denied."));

            if (!isCustomerExists) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Sorry, but this Customer doesn't exists in the system"));

            if (!isUpdated) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotModified, $"Sorry, but this Customer (number {customer.ID}) didn't modified"));

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, $"The details of the Customer (number {customer.ID}) was updated sucsessfully."));
        }

        [Route("RemoveCustomer", Name = "RemoveCustomer")]
        [HttpDelete]
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult RemoveCustomer([FromBody]Customer customer)
        {
            bool isAuthorized = false;
            bool isRemoved = false;
            bool isCustomerExists = false;
            Action act = () =>
            {
                isAuthorized = GetInternalLoginTokenInternal<Administrator>(out LoginToken<Administrator> loginTokenAdministrator);

                if (isAuthorized)
                {
                    isRemoved = _loggedInAdministratorFacade.RemoveCustomer(loginTokenAdministrator, customer, out isCustomerExists);
                }
            };
            ProcessExceptions(act);
            if (!isAuthorized) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, $"Sorry, but you're not an Administrator. Your accsess is denied."));

            if (!isCustomerExists) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, $"THis customer can't be removed because it isn't exists in the systen in the first place"));

            if (!isRemoved) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, $"Sorry, but this customer(number {customer.ID}) didn't removed."));

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, $"The customer number {customer.ID} has been removed sucsessfully."));
        }

        #endregion
    }
}
