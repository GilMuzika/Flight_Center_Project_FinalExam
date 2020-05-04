using System;

namespace Flight_Center_Project_FinalExam_DAL
{

  enum AirlineCompanyPropertyNumber
  {
           ID = 0,
           AIRLINE_NAME = 1,
           COUNTRY_CODE = 2,
           USER_ID = 3
  }

  public class AirlineCompany : PocoBase, IPoco
   {
       public Int64 ID { get; set; }
       public String AIRLINE_NAME { get; set; }
       public Int64 COUNTRY_CODE { get; set; }
       public Int64 USER_ID { get; set; }


       public AirlineCompany( String aIRLINE_NAME, Int64 cOUNTRY_CODE, Int64 uSER_ID)
       {
           AIRLINE_NAME = aIRLINE_NAME;
           COUNTRY_CODE = cOUNTRY_CODE;
           USER_ID = uSER_ID;
       }
       public AirlineCompany()
       {
           AIRLINE_NAME = "-=DEFAULT_STRING=-";
           COUNTRY_CODE = -9999;
           USER_ID = -9999;
       }



        public static bool operator ==(AirlineCompany c1, AirlineCompany c2)
        {
            if (ReferenceEquals(c1, null) && ReferenceEquals(c2, null)) return true;

            if (ReferenceEquals(c1, null) || ReferenceEquals(c2, null)) return false;

            return c1.ID == c2.ID;
        }

        public static bool operator !=(AirlineCompany c1, AirlineCompany c2)
        {
            return !(c1 == c2);
        }
        public override bool Equals(object obj)
        {
            var thisType = obj as AirlineCompany;
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
