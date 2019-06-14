using System;
using System.Windows;
using System.Windows.Data;

namespace PVT.UI.Converters
{
    public class EnumToVisibilityConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string parameterString = parameter as string;
            if (parameterString == null)
                return Visibility.Collapsed;

            if (Enum.IsDefined(value.GetType(), value) == false)
                return Visibility.Collapsed;

            object parameterValue = Enum.Parse(value.GetType(), parameterString);
            
            return parameterValue.Equals(value)?Visibility.Visible:Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}