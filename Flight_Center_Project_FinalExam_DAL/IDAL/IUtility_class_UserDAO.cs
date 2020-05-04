using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Center_Project_FinalExam_DAL
{
    public interface IUtility_class_UserDAO<T> : IUserBaseDAO<T> where T : class, IPoco, new()
    {
    }
}
