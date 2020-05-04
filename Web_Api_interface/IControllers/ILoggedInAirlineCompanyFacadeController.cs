using Flight_Center_Project_FinalExam_DAL;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Web_Api_interface.IControllers
{
    interface ILoggedInAirlineCompanyFacadeController
    {
        IHttpActionResult GetAllFlights();

        IHttpActionResult GetAllTickets();

        IHttpActionResult CancelFlight([FromBody]Flight flight);

        IHttpActionResult CreateFlight([FromBody]Flight flight);

        IHttpActionResult UpdateFlight([FromBody]Flight flight);

        IHttpActionResult ChangeMypassword([FromBody] JObject olsAndNewPassword);

        IHttpActionResult MofidyAirlineDetails([FromBody]AirlineCompany airline);








    }
}
