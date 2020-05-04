using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Web_Api_interface.Controllers
{
    public class BasicAutenticationAttribute : AuthorizationFilterAttribute
    {

        //encripting phrase for encripting and decripting usernames and passwords
        const string ENCRIPTION_PHRASE = "4r8rjfnklefjkljghggGKJHnif5r5242";

        public override void OnAuthorization(HttpActionContext actionContext)
        {            
            //does the request has username + password?
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized, "you must sent username and password");
                return;
            }

            //got username and password here in server;

            //how to retrive username and password:
            string autenticationToken = actionContext.Request.Headers.Authorization.Parameter;

            string decodedAutenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(autenticationToken));

            string[] usernamepasswordArr = decodedAutenticationToken.Split(':');
            string userName = usernamepasswordArr[0];
            string password = usernamepasswordArr[1];


            Utility_class_UserDAOMSSQL<Utility_class_User> utility_class_UserDAO = new Utility_class_UserDAOMSSQL<Utility_class_User>();
            List<Utility_class_User> registeredSystemUsersLst = utility_class_UserDAO.GetAll();

            bool isUserLegal = false;
            Utility_class_User registeredUser = new Utility_class_User();
            foreach (var s in registeredSystemUsersLst)
            {
                if (s.USER_NAME.Length > 50 && s.PASSWORD.Length > 50)
                {
                    if (userName == Statics.Decrypt(s.USER_NAME, ENCRIPTION_PHRASE) && password == Statics.Decrypt(s.PASSWORD, ENCRIPTION_PHRASE))
                    {
                        isUserLegal = true;
                        registeredUser.PASSWORD = password;
                        registeredUser.USER_NAME = userName;
                        registeredUser.USER_KIND = s.USER_KIND;
                        break;
                    }
                }
            }

            
            if (isUserLegal)
            {
               /*
               //Also there is an option to put the information in the bag on the Request itself, not on the Principal.
               //There is how to put a data on the Request's bag:                 
               */
                actionContext.Request.Properties["registered_user"] = registeredUser;
                return;
            }


            //stop the request = will not arive to web api controller
            actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized, "you are not allowed");
        }
    }
}