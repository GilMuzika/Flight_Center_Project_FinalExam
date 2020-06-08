using Flight_Center_Project_FinalExam_DAL.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Center_Project_FinalExam_DAL.DAO
{
    public class WebPageSearchResultsHTMLModelDAOMSSQL<T> : DAO<T>, IWebPageSearchResultsHTMLModelDAOMSSQL<T> where T : WebPageSearchResultsHTMLModel, IPoco, new()
    {        
        public WebPageSearchResultsHTMLModelDAOMSSQL() : base() { }


    }
}
