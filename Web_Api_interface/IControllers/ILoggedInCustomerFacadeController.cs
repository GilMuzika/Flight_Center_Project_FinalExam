using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Web_Api_interface.IControllers
{
    interface ILoggedInCustomerFacadeController
    {
        IHttpActionResult PurchaseTicket([FromBody]Flight flight);

        IHttpActionResult CancelTicket([FromBody] Ticket ticket);
    }
}
