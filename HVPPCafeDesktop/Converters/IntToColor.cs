using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace HVPPCafeDesktop.Converters
{
    public class IntToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int So)
            {
                if (So % 2 == 0)
                {
                    return new SolidColorBrush(Color.FromRgb(57, 153, 62));
                }
            }

            if (value is string Str)
            {
                if (int.Parse(Str) % 2 == 0)
                {
                    return new SolidColorBrush(Color.FromRgb(57, 153, 62));
                }
            }

            return new SolidColorBrush(Color.FromRgb(39, 137, 186));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
