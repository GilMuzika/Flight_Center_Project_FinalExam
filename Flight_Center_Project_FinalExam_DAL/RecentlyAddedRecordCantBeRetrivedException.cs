using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Center_Project_FinalExam_DAL
{
    class RecentlyAddedRecordCantBeRetrivedException<T>: Exception
    {
        public RecentlyAddedRecordCantBeRetrivedException(T poco): base($"The recently added record that comprised of {poco} can't be retrived from the database")
        {

        }
    }
}
