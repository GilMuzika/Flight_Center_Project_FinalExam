using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Center_Project_FinalExam_DAL
{
    public interface IBasicDB<T> where T : class, IPoco, new()
    {
        long Add(T poco);
        T Get(long ID);
        List<T> GetAll();
        void Remove(T poco);
        void Update(T poco);

    }
}
