using System;
using System.Globalization;
using System.Windows.Data;

namespace TOSSM.Converter
{
    public class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            Enum enumValue = default(Enum);
            if (parameter is Type)
            {
                enumValue = (Enum)Enum.Parse((Type)parameter, value.ToString());
            }
            return enumValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            int returnValue = 0;
            if (parameter is Type)
            {
                returnValue = (int)Enum.Parse((Type)parameter, value.ToString());
            }
            return returnValue;
        }
    }
    public class ReverseEnumConverter : IValueConverter
    {
        private readonly EnumConverter _converter = new EnumConverter();

        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            return _converter.ConvertBack(value, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            return _converter.Convert(value, targetType, parameter, culture);
        }
    }
}
