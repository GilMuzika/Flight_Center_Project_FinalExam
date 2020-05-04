using Flight_Center_Project_FinalExam_BL;
using Flight_Center_Project_FinalExam_DAL;
using Flight_Center_Project_FinalExam_DAL.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Description;
using System.Windows;
using Web_Api_interface.Controllers.JWTAutenticationAuxiliaries;
using Web_Api_interface.IControllers;
using Web_Api_interface.Models;

namespace Web_Api_interface.Controllers
{
    [RoutePrefix("api/LoggedInCustomerFacade")]
    public class LoggedInCustomerFacadeController : ApiController, ILoggedInCustomerFacadeController
    {        
        FlyingCenterSystem _fsc = FlyingCenterSystem.GetInstance();

        LoggedInCustomerFacade _loggedInCustomerFacade;

        public LoggedInCustomerFacadeController()
        {
            _loggedInCustomerFacade = _fsc.getFacede<LoggedInCustomerFacade>();
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

        #endregion
        #region Public Methods

        [ResponseType(typeof(LoginToken<Customer>))]
        [Route("GetCustomerLoginToken", Name = "GetCustomerLoginToken")]
        [HttpGet]
        [Authorize(Roles = "Customer")]
        public IHttpActionResult GetCustomerLoginToken()
        {
            bool isAuthorized = false;
            LoginToken<Customer> customerToken = null;
            Action act = () => 
            {
                isAuthorized = this.GetInternalLoginTokenInternal<Customer>(out LoginToken<Customer> customerTokenInternal);
                customerToken = customerTokenInternal;
            };
            ProcessExceptions(act);

            if (!isAuthorized)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, $"Sorry, but you're not a customer. Ypur accsess is denied."));
            }

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, customerToken));
        }

        [ResponseType(typeof(Flight))]
        [Route("GetFlightById/{flightId}", Name = "GetFlightById")]
        [HttpGet]
        [Authorize(Roles = "Customer")]
        public IHttpActionResult GetFlightById(long flightId)
        {
            Flight flight = null;
            Action act = () =>
            {
                flight = _loggedInCustomerFacade.GetFlightById(flightId);
            };
            ProcessExceptions(act);
            if (flight == null) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, $"There is no flights for the flight with the ID {flightId} in the system."));

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, flight));
        }

        [ResponseType(typeof(Ticket))]
        [Route("PurchaseTicket", Name = "CustomerPurchaseTicket")]
        [HttpPost]        
        [Authorize(Roles = "Customer")]        
        public IHttpActionResult PurchaseTicket([FromBody]Flight flight)
        {            
            List<Ticket> ticketLst = null;
            bool isAuthorized = false;
            Action act = () => 
            {
                isAuthorized = GetInternalLoginTokenInternal<Customer>(out LoginToken<Customer> loginTokenCustomer);
                if(isAuthorized) ticketLst = _loggedInCustomerFacade.PurchaseTickets(loginTokenCustomer, flight);
            };
            ProcessExceptions(act);
            if (!isAuthorized) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, $"Sorry, but you're not a customer. Ypur accsess is denied."));
            if(ticketLst == null) return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, $"There is no tickets for the flight with the ID {flight.ID} in the system."));

            return Ok(ticketLst);
        }

        [Route("CancelTicket", Name = "CancelTicket")]
        [HttpDelete]
        [Authorize(Roles = "Customer")]
        public IHttpActionResult CancelTicket([FromBody] Ticket ticket)
        {
            bool isAuthorized = false;
            bool isCanceled = false;
            Action act = () =>
            {
                isAuthorized = GetInternalLoginTokenInternal<Customer>(out LoginToken<Customer> loginTokenCustomer);

                if (isAuthorized)
                {
                    isCanceled = _loggedInCustomerFacade.CancelTicket(loginTokenCustomer, ticket);
                }
            };
            ProcessExceptions(act);
            if (!isAuthorized) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, $"Sorry, but you're not a customer. Ypur accsess is denied."));

            if (!isCanceled) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, $"The ticket number \"{ticket.ID}\" alredy not exists, so can't be deleted"));

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, $"The ticket number \"{ticket.ID}\" has been deleted"));

        }

        #endregion

        /*
         
         * for TokenBasedAutentication (NOT JWT!)
         
        [ResponseType(typeof(Ticket))]
        [Route("PurchaseTicketDeme/{flightId}", Name = "CustomerPurchaseTicketDeme")]
        [HttpGet]
        //This resource is For all types of role
        [Authorize(Roles = "Customer")] //Only if the USER_KIND of the registered user is "Customer" the method will continue execution, otherwise the accsess will be denied
        public IHttpActionResult PurchaseTicketDeme(long flightId)
        {
            Ticket ticket = null;
            Action act = () =>
            {
                var identity = (ClaimsIdentity)User.Identity;
                var role = identity.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).FirstOrDefault();
                var password = identity.Claims.Where(x => x.Type == "Password").Select(x => x.Value).FirstOrDefault();
                var username = identity.Claims.Where(x => x.Type == ClaimTypes.Name).Select(x => x.Value).FirstOrDefault();


                Flight flight = _loggedInCustomerFacade.GetFlightById(flightId);



                LoginService<Customer> loginService = _fsc.GetLoginService(new Customer());

                loginService.TryUserLogin(username, password, out LoginToken<Customer> loginToken);

                ticket = _loggedInCustomerFacade.PurchaseTicket(loginToken, flight);
            };

            ProcessExceptions(act);

            if (ticket == null) return NotFound();
            return Ok(ticket);

        }
        
        */

    }
}
