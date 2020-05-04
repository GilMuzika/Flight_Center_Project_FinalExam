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
            if (!isUsernamePasswordValid) return Unauthorized();
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
