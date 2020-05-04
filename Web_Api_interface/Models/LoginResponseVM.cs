using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Web_Api_interface.Models
{
    public class LoginResponseVM
    {
        public LoginResponseVM()
        {

            this.Token = "";
            this.responseMsg = new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.Unauthorized };
        }

        public LoginResponseVM(HttpResponseMessage responseMsg)
        {
            this.Token = "";
            this.responseMsg = responseMsg;
        }

        public string Token { get; set; }
        public HttpResponseMessage responseMsg { get; set; }
    }
}