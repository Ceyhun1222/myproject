using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfEnvelope.WpfShell.UI.Converter
{
    public class NullableToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is null)
                return null;

            var properties = value.GetType().GetProperties();
            var nilValue = properties[0].GetValue(value);
            if (nilValue != null)
                return nilValue.ToString();


            return properties[1].GetValue(value)?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    //TODO:NullableToDateTimeConverter
    //TODO:NullableToDoubleConverter
}
