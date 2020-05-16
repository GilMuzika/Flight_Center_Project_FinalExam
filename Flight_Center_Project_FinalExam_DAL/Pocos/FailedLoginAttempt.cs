using System;

namespace Flight_Center_Project_FinalExam_DAL
{

  public enum FailedLoginAttemptPropertyNumber
  {
           ID = 0,
           FAILED_USERNAME = 1,
           FAILED_PASSWORD = 2,
           FAILED_ATTEMPTS_NUM = 3,
           IDENTIFIER = 4,
           FAILURE_TIME = 5
  }

  public class FailedLoginAttempt : IDisposable, IPoco
   {
       public Int64 ID { get; set; }
       public String FAILED_USERNAME { get; set; }
       public String FAILED_PASSWORD { get; set; }
       public Int64 FAILED_ATTEMPTS_NUM { get; set; }
       public String IDENTIFIER { get; set; }
       public DateTime FAILURE_TIME { get; set; }


       public FailedLoginAttempt( String fAILED_USERNAME, String fAILED_PASSWORD, Int64 fAILED_ATTEMPTS_NUM, String iDENTIFIER, DateTime fAILURE_TIME)
       {
           FAILED_USERNAME = fAILED_USERNAME;
           FAILED_PASSWORD = fAILED_PASSWORD;
           FAILED_ATTEMPTS_NUM = fAILED_ATTEMPTS_NUM;
           IDENTIFIER = iDENTIFIER;
           FAILURE_TIME = fAILURE_TIME;
       }
       public FailedLoginAttempt()
       {
           FAILED_USERNAME = "-=DEFAULT_STRING=-";
           FAILED_PASSWORD = "-=DEFAULT_STRING=-";
           FAILED_ATTEMPTS_NUM = -9999;
           IDENTIFIER = "-=DEFAULT_STRING=-";
           FAILURE_TIME = DateTime.MinValue;
       }



        public static bool operator ==(FailedLoginAttempt c1, FailedLoginAttempt c2)
        {
            if (ReferenceEquals(c1, null) && ReferenceEquals(c2, null)) return true;

            if (ReferenceEquals(c1, null) || ReferenceEquals(c2, null)) return false;

            return c1.ID == c2.ID;
        }

        public static bool operator !=(FailedLoginAttempt c1, FailedLoginAttempt c2)
        {
            return !(c1 == c2);
        }
        public override bool Equals(object obj)
        {
            var thisType = obj as FailedLoginAttempt;
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
