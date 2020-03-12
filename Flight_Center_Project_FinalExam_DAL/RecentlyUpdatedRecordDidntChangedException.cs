using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Center_Project_FinalExam_DAL
{
    class RecentlyUpdatedRecordDidntChangedException<T>: Exception
    {
        public RecentlyUpdatedRecordDidntChangedException(T poco): base($"The recently updated record that comprised of {poco} didn't changed")
        {

        }
    }
}
