using System;

namespace Flight_Center_Project_FinalExam_DAL
{

  public enum FlightsHistoryPropertyNumber
  {
           ID = 0,
           AIRLINECOMPANY_ID = 1,
           ORIGIN_COUNTRY_CODE = 2,
           DESTINATION_COUNTRY_CODE = 3,
           DEPARTURE_TIME = 4,
           LANDING_TIME = 5,
           REMAINING_TICKETS = 6
  }

  public class FlightsHistory : IPoco
   {
       public Int64 ID { get; set; }
       public Int64 AIRLINECOMPANY_ID { get; set; }
       public Int64 ORIGIN_COUNTRY_CODE { get; set; }
       public Int64 DESTINATION_COUNTRY_CODE { get; set; }
       public DateTime DEPARTURE_TIME { get; set; }
       public DateTime LANDING_TIME { get; set; }
       public Int32 REMAINING_TICKETS { get; set; }


       public FlightsHistory( Int64 aIRLINECOMPANY_ID, Int64 oRIGIN_COUNTRY_CODE, Int64 dESTINATION_COUNTRY_CODE, DateTime dEPARTURE_TIME, DateTime lANDING_TIME, Int32 rEMAINING_TICKETS)
       {
           AIRLINECOMPANY_ID = aIRLINECOMPANY_ID;
           ORIGIN_COUNTRY_CODE = oRIGIN_COUNTRY_CODE;
           DESTINATION_COUNTRY_CODE = dESTINATION_COUNTRY_CODE;
           DEPARTURE_TIME = dEPARTURE_TIME;
           LANDING_TIME = lANDING_TIME;
           REMAINING_TICKETS = rEMAINING_TICKETS;
       }
       public FlightsHistory()
       {
           AIRLINECOMPANY_ID = -9999;
           ORIGIN_COUNTRY_CODE = -9999;
           DESTINATION_COUNTRY_CODE = -9999;
           DEPARTURE_TIME = DateTime.MinValue;
           LANDING_TIME = DateTime.MinValue;
           REMAINING_TICKETS = -9999;
       }



        public static bool operator ==(FlightsHistory c1, FlightsHistory c2)
        {
            if (ReferenceEquals(c1, null) && ReferenceEquals(c2, null)) return true;

            if (ReferenceEquals(c1, null) || ReferenceEquals(c2, null)) return false;

            return c1.ID == c2.ID;
        }

        public static bool operator !=(FlightsHistory c1, FlightsHistory c2)
        {
            return !(c1 == c2);
        }
        public override bool Equals(object obj)
        {
            var thisType = obj as FlightsHistory;
            return this == thisType;
        }
        public override int GetHashCode()
        {
            return Convert.ToInt32(this.ID);
        }

        public override string ToString()
        {
            string str = string.Empty;
            foreach(var s in this.GetType().GetProperties())
               str += $"{ s.Name}: { s.GetValue(this)}\n";

            return str;
        }
   }
}
