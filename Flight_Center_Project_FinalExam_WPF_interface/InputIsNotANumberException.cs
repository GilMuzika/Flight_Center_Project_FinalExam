
using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flight_Center_Project_FinalExam_WPF_interface
{
    class InputIsNotANumberException : Exception
    {
        public InputIsNotANumberException(string input) : base($"Your input \"{input}\" isn't a number") { }
    }
}
