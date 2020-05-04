using System;

namespace Flight_Center_Project_FinalExam_DAL
{

  public enum CountryPropertyNumber
  {
           ID = 0,
           COUNTRY_NAME = 1,
           COUNTRY_IDENTIFIER = 2
  }

  public class Country : PocoBase, IPoco
   {
       public Int64 ID { get; set; }
       public String COUNTRY_NAME { get; set; }
       public Int64 COUNTRY_IDENTIFIER { get; set; }


       public Country( String cOUNTRY_NAME, Int64 cOUNTRY_IDENTIFIER)
       {
           COUNTRY_NAME = cOUNTRY_NAME;
           COUNTRY_IDENTIFIER = cOUNTRY_IDENTIFIER;
       }
       public Country()
       {
           COUNTRY_NAME = "-=DEFAULT_STRING=-";
           COUNTRY_IDENTIFIER = -9999;
       }



        public static bool operator ==(Country c1, Country c2)
        {
            if (ReferenceEquals(c1, null) && ReferenceEquals(c2, null)) return true;

            if (ReferenceEquals(c1, null) || ReferenceEquals(c2, null)) return false;

            return c1.ID == c2.ID;
        }

        public static bool operator !=(Country c1, Country c2)
        {
            return !(c1 == c2);
        }
        public override bool Equals(object obj)
        {
            var thisType = obj as Country;
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
