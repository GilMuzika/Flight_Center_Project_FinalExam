using Flight_Center_Project_FinalExam_BL;
using Flight_Center_Project_FinalExam_DAL;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using System.Web.UI.WebControls;
using Web_Api_interface.Controllers.JWTAutenticationAuxiliaries;
using Web_Api_interface.Models;

namespace Web_Api_interface.Controllers
{
    [RoutePrefix("api/jwt")]
    public class JwtController : ApiController
    {
        private JsonToDictionaryConverter _jsonToDictionaryConverter = new JsonToDictionaryConverter();
         

        /// <summary>
        /// "SecretKey" string needed to initialize the "JWTService" service that creates the JWT token
        /// </summary>
        private string Secretkey = Convert.ToBase64String(Encoding.Default.GetBytes(ConfigurationManager.AppSettings["JWT_SecretKey"]));
        private JWTService _jwtService;

        public JwtController()
        {
            _jwtService = new JWTService(Secretkey);
        }

        private UserValidator _userValidator = new UserValidator();

        [HttpGet]
        [Authorize]
        [Route("ok")]
        public IHttpActionResult Authenticated() => Ok("Authenticated");

        [HttpGet]
        [Route("notok")]
        public IHttpActionResult NotAuthorized() => Ok("Unauthorized");


        /// <summary>
        /// This method takes argument as JObject in thuis format:
        /// {
        ///  "username": "actual username",
        ///  "password": "actual password",
        /// }
        /// </summary>
        /// <param name="credentials">JObject in format: ["username" : "actual username", "password": "actual password"]</param>
        /// <returns></returns>
        [HttpPost]
        [Route("createJwtToken")]
        public IHttpActionResult Authenticate([FromBody] JObject credentials)
        {
            if (credentials == null) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, $"Sorry, but your credentials came in unsupported format."));

            Dictionary<string, object> credentialsData = _jsonToDictionaryConverter.ProvideAPIDataFromJSON(credentials);

            string username = string.Empty;
            string password = string.Empty;

            foreach(var s in credentialsData)
            {
                if (s.Key.Contains("username")) username = s.Value.ToString();
                if (s.Key.Contains("password")) password = s.Value.ToString();
            }


            var loginResponse = new LoginResponseVM();

            bool isUsernamePasswordValid = _userValidator.ValidateUser(username, password, out Utility_class_User validatedUserModel);

            //if credentials are invalid
            if (!isUsernamePasswordValid)
            {
                FailedAttemptsFacade failedFacade = FlyingCenterSystem.GetInstance().getFacede<FailedAttemptsFacade>();

                FailedLoginAttempt attemptByPassword = failedFacade.GetByPassword(password);
                FailedLoginAttempt attempByUsername = failedFacade.GetByUserName(username);
                bool attemptsComparsion = Statics.BulletprofComparsion(attemptByPassword, attempByUsername);                
                if(!attemptsComparsion) return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Your username or password is incorrect, also there is no consistency between them! Acsess denied."));

                long failedAttemptNum = 0;
                long failedAttemptNumToDisplay = 1;

                bool isTheAttemptIsFirts = attemptByPassword.Equals(new FailedLoginAttempt());

                if(isTheAttemptIsFirts)
                {
                    failedFacade.AddBlackUser(new FailedLoginAttempt(username, password, 2, Guid.NewGuid().ToString(), DateTime.Now));
                }
                else
                {
                    //long.TryParse(ConfigurationManager.AppSettings["Permitted_Login_Attempts_Num"], out long permittedLOginAttempts);
                    if(attemptByPassword.FAILED_ATTEMPTS_NUM <= 3)
                    {
                        failedAttemptNum = attemptByPassword.FAILED_ATTEMPTS_NUM;
                        failedAttemptNumToDisplay = failedAttemptNum;
                        failedAttemptNum++;                        
                        attemptByPassword.FAILED_ATTEMPTS_NUM = failedAttemptNum;
                        bool isUpdated = failedFacade.UpdateBlackUser(attemptByPassword);
                    }
                    else
                    {                        
                        if (DateTime.Now.AddDays(-1) < attemptByPassword.FAILURE_TIME) 
                        {
                            TimeSpan timeRemainder = new TimeSpan(24, 0, 0) - DateTime.Now.Subtract(attemptByPassword.FAILURE_TIME);
                            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, $"Sorry, but the system didn't regocnyzed you as registered user. Your accsess is denied. You're had tried to aouthorize more tham 3 times. Wait {timeRemainder.Hours} hours and {timeRemainder.Minutes} minutes until new attempt!"));
                        }
                        else
                        {
                            failedAttemptNum = 1;                            
                            attemptByPassword.FAILED_ATTEMPTS_NUM = failedAttemptNum;
                            attemptByPassword.FAILURE_TIME = DateTime.Now;
                            bool updated = failedFacade.UpdateBlackUser(attemptByPassword);
                        }
                        
                    }
                }



                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, $"Sorry, but the system didn't regocnyzed you as registered user. Your accsess is denied. You're had tried to aouthorize {failedAttemptNumToDisplay} times."));
            }
            //if credentials are valid
            if (isUsernamePasswordValid)
            {
                var token = _jwtService.CreateToken(validatedUserModel);
                return Ok(token);
            }
            //if credentials are nt valid send unathorized status code in response
            loginResponse.responseMsg.StatusCode = HttpStatusCode.Unauthorized;
            IHttpActionResult response = ResponseMessage(loginResponse.responseMsg);
            return response;
        }


    }
}
