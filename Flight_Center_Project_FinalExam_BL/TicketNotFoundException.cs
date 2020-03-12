using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flight_Center_Project_FinalExam_BL
{
    class TicketNotFoundException : Exception
    {
        public TicketNotFoundException(long ticketID): base($"the ticket with the ID \"{ticketID}\" is NOT found.")
        {

        }
    }
}
