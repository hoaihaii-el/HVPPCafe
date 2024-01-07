using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HVPPCafeDesktop.Converters
{
    class BoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool IsFullTime)
            {
                if (IsFullTime) return "Full-time";
            }
            return "Part-time";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string Type)
            {
                if (Type == "Full-time") return true;
            }

            return false;
        }
    }
}
