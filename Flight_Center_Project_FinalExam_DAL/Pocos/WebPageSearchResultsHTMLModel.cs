using System;

namespace Flight_Center_Project_FinalExam_DAL
{

  public enum WebPageSearchResultsHTMLModelPropertyNumber
  {
           ID = 0,
           ModelConstantPart = 1,
           ModelRepetitivePart = 2
  }

  public class WebPageSearchResultsHTMLModel : IDisposable, IPoco
   {
       public Int64 ID { get; set; }
       public String ModelConstantPart { get; set; }
       public String ModelRepetitivePart { get; set; }


       public WebPageSearchResultsHTMLModel( String mODELCONSTANTPART, String mODELREPETITIVEPART)
       {
           ModelConstantPart = mODELCONSTANTPART;
           ModelRepetitivePart = mODELREPETITIVEPART;
       }
       public WebPageSearchResultsHTMLModel()
       {
           ModelConstantPart = "-=DEFAULT_STRING=-";
           ModelRepetitivePart = "-=DEFAULT_STRING=-";
       }



        public static bool operator ==(WebPageSearchResultsHTMLModel c1, WebPageSearchResultsHTMLModel c2)
        {
            if (ReferenceEquals(c1, null) && ReferenceEquals(c2, null)) return true;

            if (ReferenceEquals(c1, null) || ReferenceEquals(c2, null)) return false;

            return c1.ID == c2.ID;
        }

        public static bool operator !=(WebPageSearchResultsHTMLModel c1, WebPageSearchResultsHTMLModel c2)
        {
            return !(c1 == c2);
        }
        public override bool Equals(object obj)
        {
            var thisType = obj as WebPageSearchResultsHTMLModel;
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
