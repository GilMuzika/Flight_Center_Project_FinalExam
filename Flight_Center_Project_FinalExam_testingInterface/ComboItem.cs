using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Center_Project_FinalExam_testingInterface
{
    class ComboItem<T>
    {
        public T Item { get; set; } = default(T);

        public ComboItem(T item)
        {
            Item = item;
        }

        public override string ToString()
        {
            return typeof(T).GetProperties()[1].GetValue(Item).ToString();
        }
    }
}
