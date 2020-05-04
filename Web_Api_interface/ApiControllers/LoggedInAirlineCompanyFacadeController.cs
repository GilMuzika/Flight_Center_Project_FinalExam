using Flight_Center_Project_FinalExam_BL;
using Flight_Center_Project_FinalExam_DAL;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;
using System.Windows;
using Web_Api_interface.IControllers;

namespace Web_Api_interface.Controllers
{ 
    [RoutePrefix("api/LoggedInAirlineCompanyFacade")]    
    public class LoggedInAirlineCompanyFacadeController : ApiController, ILoggedInAirlineCompanyFacadeController
    {
        private JsonToDictionaryConverter _jsonToDictionaryConverter = new JsonToDictionaryConverter();

        private FlyingCenterSystem _fsc = FlyingCenterSystem.GetInstance();

        private LoggedInAirlineFacade _loggedInAirlineFacade;

        public LoggedInAirlineCompanyFacadeController()
        {
            _loggedInAirlineFacade = _fsc.getFacede<LoggedInAirlineFacade>();
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

        [ResponseType(typeof(Flight))]
        [HttpGet]
        [Route("GetAllFlights", Name = "GetAllFlights")]
        public IHttpActionResult GetAllFlights()
        {

            List<Flight> flights = _loggedInAirlineFacade.GetAllFlights();
            if (flights.Count == 0) return NotFound();

            return Ok(flights);
        }

        [ResponseType(typeof(Ticket))]
        [Route("GetAllTickets", Name = "GetAllTickets")]
        [HttpGet]
        [Authorize(Roles = "AirlineCompany")]
        public IHttpActionResult GetAllTickets()
        {
            IList<Ticket> ticketLst = null;
            bool isAuthorized = false;
            Action act = () =>
            {
                isAuthorized = GetInternalLoginTokenInternal<AirlineCompany>(out LoginToken<AirlineCompany> loginTokenAirline);
                if (isAuthorized) ticketLst = _loggedInAirlineFacade.GetAllTickets(loginTokenAirline);
            };
            ProcessExceptions(act);
            if (!isAuthorized) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, $"Sorry, but you're not an Airline Company. Your accsess is denied."));
            if (ticketLst == null) return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, $"There is no tickets in the system."));

            return Ok(ticketLst);
        }

        [Route("CancelFlight", Name = "CancelFlight")]
        [HttpDelete]
        [Authorize(Roles = "AirlineCompany")]
        public IHttpActionResult CancelFlight([FromBody]Flight flight)
        {
            bool isAuthorized = false;
            bool isCanceled = false;
            Action act = () =>
            {
                isAuthorized = GetInternalLoginTokenInternal<AirlineCompany>(out LoginToken<AirlineCompany> loginTokenAirline);

                if (isAuthorized)
                {
                    isCanceled = _loggedInAirlineFacade.CancelFlight(loginTokenAirline, flight);
                }
            };
            ProcessExceptions(act);
            if (!isAuthorized) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, $"Sorry, but you're not an Airline Company. Ypur accsess is denied."));

            if (!isCanceled) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, $"The flight number \"{flight.ID}\" alredy not exists, so can't be deleted"));

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, $"The flight number \"{flight.ID}\" has been deleted"));
        }

        [Route("CreateFlight", Name = "CreateFlight")]
        [HttpPost]
        [Authorize(Roles = "AirlineCompany")]
        public IHttpActionResult CreateFlight([FromBody]Flight flight)
        {
            bool isAuthorized = false;
            bool isCreated = false;
            Action act = () =>
            {
                isAuthorized = GetInternalLoginTokenInternal<AirlineCompany>(out LoginToken<AirlineCompany> loginTokenAirline);

                if (isAuthorized)
                {
                    isCreated = _loggedInAirlineFacade.CreateFlight(loginTokenAirline, flight);
                }
            };
            ProcessExceptions(act);
            if (!isAuthorized) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, $"Sorry, but you're not an Airline Company. Your accsess is denied."));

            if (!isCreated) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, $"Sorry, but the flight number \"{flight.ID}\" doesn't created."));

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, $"The flight number \"{flight.ID}\" has been created"));
        }

        [Route("UpdateFlight", Name = "UpdateFlight")]
        [HttpPost]
        [Authorize(Roles = "AirlineCompany")]
        public IHttpActionResult UpdateFlight([FromBody]Flight flight)
        {
            bool isAuthorized = false;
            bool isUpdated = false;
            bool isFound = false;
            Action act = () =>
            {
                isAuthorized = GetInternalLoginTokenInternal<AirlineCompany>(out LoginToken<AirlineCompany> loginTokenAirline);

                if (isAuthorized)
                {
                    isUpdated = _loggedInAirlineFacade.UpdateFlight(loginTokenAirline, flight, out isFound);
                }
            };
            ProcessExceptions(act);
            if (!isAuthorized) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, $"Sorry, but you're not an Airline Company. Your accsess is denied."));

            if (!isFound) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Sorry, but the flight number \"{flight.ID}\" can't be update beause it don't exists in the system in the first place."));

            if (!isUpdated) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotModified, $"Sorry, but the flight number \"{flight.ID}\" doesn't updated."));            

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, $"The flight number \"{flight.ID}\" has been updated. Now it seems like that: \n\n {_loggedInAirlineFacade.GetFlightById(flight.ID)}\n\nEnjoy it!"));
        }

        /// <summary>
        /// This method takes argument as JObject in thuis format:
        /// {
        ///  "oldpass": "pld password",
        ///  "newpass": "new password",
        /// }
        /// </summary>
        /// <param name="credentials">JObject in format: ["oldpass" : "old password", "newpass": "new password"]</param>
        /// <returns></returns>
        [Route("ChangeMypassword", Name = "ChangeMypassword")]
        [HttpPost]
        [Authorize(Roles = "AirlineCompany")]
        public IHttpActionResult ChangeMypassword([FromBody] JObject olsAndNewPassword)
        {
            if (olsAndNewPassword == null) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, $"Sorry, but your credentials came in unsupported format."));

            Dictionary<string, object> credentialsDataDict = _jsonToDictionaryConverter.ProvideAPIDataFromJSON(olsAndNewPassword);

            string oldPass = string.Empty;
            string newPass = string.Empty;
            int count = 0;
            bool isContainsRequiredWords = true;
            foreach(var s in credentialsDataDict)
            {
                if (s.Key.Contains("oldpass")) oldPass = s.Value.ToString();
                else isContainsRequiredWords = false;
                if (s.Key.Contains("newpass")) newPass = s.Value.ToString();
                else isContainsRequiredWords = false;

                count++;
            }

            if (isContainsRequiredWords == false) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, $"Your credentials must contain words \"oldpass\" and \"newpass\" "));
            if(count != 2) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, $"Your credentials must be a JObject with 2 properties (no more no less)"));

            bool isAuthorized = false;
            bool isChanged = false;
            bool isPasswordWrong = true;
            Action act = () =>
            {
                isAuthorized = GetInternalLoginTokenInternal<AirlineCompany>(out LoginToken<AirlineCompany> loginTokenAirline);

                if (isAuthorized)
                {
                    isChanged = _loggedInAirlineFacade.ChangeMyPassword(loginTokenAirline, oldPass, newPass, out isPasswordWrong);
                }
            };
            ProcessExceptions(act);
            if (!isAuthorized) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, $"Sorry, but you're not an Airline Company. Your accsess is denied."));

            if (isPasswordWrong) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, $"Sorry, but the password \"{oldPass}\" is wrong. You need to feed the right password in order to change it."));

            if (!isChanged) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "Sorry, but your password does not changed"));

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, $"your password changed to \"{newPass}\" "));
        }

        [Route("MofidyAirlineDetails", Name = "MofidyAirlineDetails")]
        [HttpPut]
        [Authorize(Roles = "AirlineCompany")]
        public IHttpActionResult MofidyAirlineDetails([FromBody]AirlineCompany airline)
        {
            bool isAuthorized = false;
            bool isModified = false;
            Action act = () =>
            {
                isAuthorized = GetInternalLoginTokenInternal<AirlineCompany>(out LoginToken<AirlineCompany> loginTokenAirline);

                if (isAuthorized)
                {
                    isModified = _loggedInAirlineFacade.MofidyAirlineDetails(loginTokenAirline, airline);
                }
            };
            ProcessExceptions(act);
            if (!isAuthorized) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, $"Sorry, but you're not an Airline Company. Your accsess is denied."));

            if(!isModified) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, $"Sorry, but the Airline isn't modified"));

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, $"The Airline number {airline.ID} is modified"));
        }

        #endregion




    }
}
