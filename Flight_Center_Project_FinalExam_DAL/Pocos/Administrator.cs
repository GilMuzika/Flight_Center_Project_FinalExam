using System;

namespace Flight_Center_Project_FinalExam_DAL
{

  enum AdministratorPropertyNumber
  {
           ID = 0,
           NAME = 1,
           USER_ID = 2
  }

  public class Administrator : IPoco, IUser
   {
       public Int64 ID { get; set; }
       public String NAME { get; set; }
       public Int64 USER_ID { get; set; }


       public Administrator( String nAME, Int64 uSER_ID)
       {
           NAME = nAME;
           USER_ID = uSER_ID;
       }
       public Administrator()
       {
           NAME = "-=DEFAULT_STRING=-";
           USER_ID = -9999;
       }



        public static bool operator ==(Administrator c1, Administrator c2)
        {
            if (ReferenceEquals(c1, null) && ReferenceEquals(c2, null)) return true;

            if (ReferenceEquals(c1, null) || ReferenceEquals(c2, null)) return false;

            return c1.ID == c2.ID;
        }

        public static bool operator !=(Administrator c1, Administrator c2)
        {
            return !(c1 == c2);
        }
        public override bool Equals(object obj)
        {
            var thisType = obj as Administrator;
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
