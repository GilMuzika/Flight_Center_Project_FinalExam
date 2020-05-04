using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase_Generator_WPF
{
    interface IDataInterface
    {
        void Add(long from, long to, long fixeedNumber);
    }
}
