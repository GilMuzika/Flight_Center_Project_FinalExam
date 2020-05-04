using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Center_Project_FinalExam_DAL
{
    public class AdministratorDAOMSSQL<T> : UserBaseMSSQLDAO<T>, IAdministratorDAO<T> where T : Administrator, IPoco, new()
    {
        public AdministratorDAOMSSQL() : base() { }    
        
        public Administrator GetAdministratorByUserID(long userID)
        {
            return base.GetSomethingBySomethingInternal(userID, (int)AdministratorPropertyNumber.USER_ID);
        }
    }
}
