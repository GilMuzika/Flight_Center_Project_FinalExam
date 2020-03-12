using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Center_Project_FinalExam_DAL
{
    class RecentlyDeletedRecordStillExistsException<T>: Exception
    {
        public RecentlyDeletedRecordStillExistsException(T poco): base($"The recently deleted record that comprised of {poco} still exists in the database")
        {

        }
    }
}
