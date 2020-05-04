using System;

namespace Flight_Center_Project_FinalExam_DAL
{

  public enum HistoryTrackingPropertyNumber
  {
           ID = 0,
           HISTORY_ENTRY_TIME = 1,
           HISTORY_ENTRY_KIND = 2,
           HISTORY_ENTRY_ID = 3
  }

  public class HistoryTracking : IDisposable, IPoco
   {
       public Int64 ID { get; set; }
       public DateTime HISTORY_ENTRY_TIME { get; set; }
       public String HISTORY_ENTRY_KIND { get; set; }
       public Int64 HISTORY_ENTRY_ID { get; set; }


       public HistoryTracking( DateTime hISTORY_ENTRY_TIME, String hISTORY_ENTRY_KIND, Int64 hISTORY_ENTRY_ID)
       {
           HISTORY_ENTRY_TIME = hISTORY_ENTRY_TIME;
           HISTORY_ENTRY_KIND = hISTORY_ENTRY_KIND;
           HISTORY_ENTRY_ID = hISTORY_ENTRY_ID;
       }
       public HistoryTracking()
       {
           HISTORY_ENTRY_TIME = DateTime.MinValue;
           HISTORY_ENTRY_KIND = "-=DEFAULT_STRING=-";
           HISTORY_ENTRY_ID = -9999;
       }



        public static bool operator ==(HistoryTracking c1, HistoryTracking c2)
        {
            if (ReferenceEquals(c1, null) && ReferenceEquals(c2, null)) return true;

            if (ReferenceEquals(c1, null) || ReferenceEquals(c2, null)) return false;

            return c1.ID == c2.ID;
        }

        public static bool operator !=(HistoryTracking c1, HistoryTracking c2)
        {
            return !(c1 == c2);
        }
        public override bool Equals(object obj)
        {
            var thisType = obj as HistoryTracking;
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
