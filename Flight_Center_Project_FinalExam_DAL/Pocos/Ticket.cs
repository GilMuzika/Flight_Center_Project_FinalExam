using System;

namespace Flight_Center_Project_FinalExam_DAL
{

  public enum TicketPropertyNumber
  {
           ID = 0,
           FLIGHT_ID = 1,
           CUSTOMER_ID = 2
  }

  public class Ticket : PocoBase, IPoco
   {
       public Int64 ID { get; set; }
       public Int64 FLIGHT_ID { get; set; }
       public Int64 CUSTOMER_ID { get; set; }


       public Ticket( Int64 fLIGHT_ID, Int64 cUSTOMER_ID)
       {
           FLIGHT_ID = fLIGHT_ID;
           CUSTOMER_ID = cUSTOMER_ID;
       }
       public Ticket()
       {
           FLIGHT_ID = -9999;
           CUSTOMER_ID = -9999;
       }



        public static bool operator ==(Ticket c1, Ticket c2)
        {
            if (ReferenceEquals(c1, null) && ReferenceEquals(c2, null)) return true;

            if (ReferenceEquals(c1, null) || ReferenceEquals(c2, null)) return false;

            return c1.ID == c2.ID;
        }

        public static bool operator !=(Ticket c1, Ticket c2)
        {
            return !(c1 == c2);
        }
        public override bool Equals(object obj)
        {
            var thisType = obj as Ticket;
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
