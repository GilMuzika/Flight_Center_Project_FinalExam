using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.IO;
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
    public class ExampleBasicAutenticationAttribute : AuthorizationFilterAttribute
    {
        //implementation of "ThreadStatic" mechanism
        //"ThreadStatic" attribute means that despite the field is static, each thread is given its own copy od this field,
        //so it exists only within one specific thread, so 
        //the mechanism is thread-safe
        [ThreadStatic]
        public static Utility_class_User _registeredUser = null;

        //encripting phrase for encripting and decripting usernames and passwords
        const string ENCRIPTION_PHRASE = "4r8rjfnklefjkljghggGKJHnif5r5242";

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //got username and password here in server;


            //does the request has username + password?
            if(actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized, "you must sent username and password");
                return;
            }
            //how to retrive username and password:
            string autenticationToken = actionContext.Request.Headers.Authorization.Parameter;

            string decodedAutenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(autenticationToken));

            string[] usernamepasswordArr = decodedAutenticationToken.Split(':');
            string userName = usernamepasswordArr[0];
            string password = usernamepasswordArr[1];



            /*
            //Example of using ThreadStatic fields:
            */
            Utility_class_UserDAOMSSQL<Utility_class_User> utility_class_UserDAO = new Utility_class_UserDAOMSSQL<Utility_class_User>();
            List<Utility_class_User> registeredSystemUsersLst = utility_class_UserDAO.GetAll();

            bool isUserLegal = false;
            Utility_class_User registeredUser = new Utility_class_User();
            foreach(var s in registeredSystemUsersLst)
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


            


            //Principle


            //if username and pasword are legal stop the function and prevent it to return Unauthorized response
            if (isUserLegal)
            {
                /*
                // Passing information (aka username) through current thread by putting it in the Principal of the thread.
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(userName), null);
                */

                /*
                // Passing information (aka username) through the request by putting it in the Principal of the request.
                 
                actionContext.Request.GetRequestContext().Principal = new GenericPrincipal(new GenericIdentity(userName), null);
                */

                /*
                 //Also there is an option to putte information in the bag on the Request itself, not on the Principal.
                 //There is how to put a data on the Request's bag:                 
                 */

                actionContext.Request.Properties["registered_user"] = registeredUser;
                _registeredUser = registeredUser;

                actionContext.Request.Properties["arbitrary_key"] = usernamepasswordArr; //
                                                                                         //"actionContext.Request.Properties" is a dictionary of objects (Dictionary<string, object>), you can put inside any object with an arbitrary string key key



                


                return;
            }


            //stop the request = will not arive to web api controller
            actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized, "you are not allowed");
        }
    }
}