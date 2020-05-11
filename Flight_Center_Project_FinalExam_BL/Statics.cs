using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flight_Center_Project_FinalExam_BL
{
    static class Statics
    {
        /// <summary>
        /// This method supposed to work appropriately only with Poco objects that have a corresponding "[pocoobject]History" object.
        /// So, in this project there only the "Ticket <=> TicketsHistory"
        /// and "Flight <=> FlightsHistory" pairs.
        /// The only difference between the poco objects in such a pairs is only their names, 
        /// the "inside" must be the same.
        /// </summary>
        /// <typeparam name="T">input poco object type</typeparam>
        /// <typeparam name="THistory">corresponding "[pocoobject]History" object type (output type)</typeparam>
        /// <param name="inputObj">input poco object</param>
        /// <returns></returns>
        public static THistory ConvertToHistoryObject<T, THistory>(this T inputObj) where THistory : class, IPoco, new()
        {
            if (typeof(T).GetProperties().Length != typeof(THistory).GetProperties().Length) return null;

            THistory historyObj = new THistory();
            for(int i = 0; i < typeof(THistory).GetProperties().Length; i++)
            {
                typeof(THistory).GetProperties()[i].SetValue(historyObj, typeof(T).GetProperties()[i].GetValue(inputObj));
            }

            return historyObj;
        }

        /// <summary>
        /// This method compares a two Poco objects of the same type by all the properties except ID
        /// </summary>
        /// <typeparam name="T">type of the objects</typeparam>
        /// <param name="o1">The first compared object</param>
        /// <param name="o2">The second compared object</param>
        /// <returns></returns>
        public static bool BulletprofComparsion<T>(T o1, T o2) where T : IPoco
        {
            bool isEqual = true;
            for (int i = 1; i < typeof(T).GetProperties().Length; i++)
            {
                var value1 = typeof(T).GetProperties()[i].GetValue(o1);
                var value2 = typeof(T).GetProperties()[i].GetValue(o2);

                if (!value1.Equals(value2))
                {
                    isEqual = false;
                    break;
                }
            }
            return isEqual;
        }


    }
}
