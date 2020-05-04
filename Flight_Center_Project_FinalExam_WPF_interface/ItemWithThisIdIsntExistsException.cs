using System;
using Flight_Center_Project_FinalExam_DAL;
using System.Collections.Generic;
using System.Text;

namespace Flight_Center_Project_FinalExam_WPF_interface
{
    class ItemWithThisIdIsntExistsException<T> : Exception where T : class, IPoco, new()
    {
        public ItemWithThisIdIsntExistsException(T item, int allegedID) : base($"The {typeof(T).Name.ToLower()} with the ID \"{allegedID}\" doesn't exist in the system") {}


        public static string GetMessageWithoutThrowing(int allegedID)
        {
            return $"The {typeof(T).Name.ToLower()} with the ID \"{allegedID}\" doesn't exist in the system";
        }
    }
}
