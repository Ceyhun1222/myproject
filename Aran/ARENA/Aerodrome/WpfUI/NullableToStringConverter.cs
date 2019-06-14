using Aerodrome.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfUI
{
    //[ValueConversion(typeof(string), typeof(AM_Nullable<string>))]
    public class NullableToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value is AM_Nullable<string> t)
            {
                return t.Value;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!value.ToString().Equals(string.Empty))
            {
                return new AM_Nullable<string>() { Value = value.ToString() };
            }
            return null;
        }
    }
}
