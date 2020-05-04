using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Flight_Center_Project_FinalExam_WPF_interface
{
    class NumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string message = string.Empty;
            if(value is String)
            {
                if (!Int32.TryParse((string)value, out int numValue)) return "Sorry, but the property value isn't a number";
                else value = numValue;
            }

            if ((int)value == 0) message = "Full";
            else if ((int)value > 0 && (int)value <= 30) message = "Almost full";
            else message = "Vacancy";

            return message;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
