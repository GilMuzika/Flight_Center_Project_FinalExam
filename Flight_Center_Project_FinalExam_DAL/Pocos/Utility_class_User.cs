using System;

namespace Flight_Center_Project_FinalExam_DAL
{

  public enum Utility_class_UserPropertyNumber
  {
           ID = 0,
           USER_NAME = 1,
           PASSWORD = 2,
           USER_KIND = 3,
           AIRLINE_ID = 4,
           CUSTOMER_ID = 5,
           ADMINISTRATOR_ID = 6,
           IDENTIFIER = 7
  }

  public class Utility_class_User : IDisposable, IPoco
   {
       public Int64 ID { get; set; }
       public String USER_NAME { get; set; }
       public String PASSWORD { get; set; }
       public String USER_KIND { get; set; }
       public Int64 AIRLINE_ID { get; set; }
       public Int64 CUSTOMER_ID { get; set; }
       public Int64 ADMINISTRATOR_ID { get; set; }
       public String IDENTIFIER { get; set; }


       public Utility_class_User( String uSER_NAME, String pASSWORD, String uSER_KIND, Int64 aIRLINE_ID, Int64 cUSTOMER_ID, Int64 aDMINISTRATOR_ID, String iDENTIFIER)
       {
           USER_NAME = uSER_NAME;
           PASSWORD = pASSWORD;
           USER_KIND = uSER_KIND;
           AIRLINE_ID = aIRLINE_ID;
           CUSTOMER_ID = cUSTOMER_ID;
           ADMINISTRATOR_ID = aDMINISTRATOR_ID;
           IDENTIFIER = iDENTIFIER;
       }
       public Utility_class_User()
       {
           USER_NAME = "-=DEFAULT_STRING=-";
           PASSWORD = "-=DEFAULT_STRING=-";
           USER_KIND = "-=DEFAULT_STRING=-";
           AIRLINE_ID = -9999;
           CUSTOMER_ID = -9999;
           ADMINISTRATOR_ID = -9999;
           IDENTIFIER = "-=DEFAULT_STRING=-";
       }



        public static bool operator ==(Utility_class_User c1, Utility_class_User c2)
        {
            if (ReferenceEquals(c1, null) && ReferenceEquals(c2, null)) return true;

            if (ReferenceEquals(c1, null) || ReferenceEquals(c2, null)) return false;

            return c1.ID == c2.ID;
        }

        public static bool operator !=(Utility_class_User c1, Utility_class_User c2)
        {
            return !(c1 == c2);
        }
        public override bool Equals(object obj)
        {
            var thisType = obj as Utility_class_User;
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


       public void Dispose() { }

   }
}
