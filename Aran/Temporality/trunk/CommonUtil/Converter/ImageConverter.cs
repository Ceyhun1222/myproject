using System;
using System.Windows.Data;

namespace TOSSM.Converter
{
    public class ImageConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter,
                                                            System.Globalization.CultureInfo culture)
        {
           return  new Uri(@"pack://application:,,,/Aran.Temporality.Resources;component\Resources\Images\"+ value + ".png", UriKind.RelativeOrAbsolute);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                                                 System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    } 
}
