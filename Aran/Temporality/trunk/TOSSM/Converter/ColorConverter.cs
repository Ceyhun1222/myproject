using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace TOSSM.Converter
{
    public class ColorConverter :  IValueConverter
    {

        public static Color Blend(Color color, Color backColor, double amount)
        {
            byte a = (byte)((color.A * amount) + backColor.A * (1 - amount));
            byte r = (byte)((color.R * amount) + backColor.R * (1 - amount));
            byte g = (byte)((color.G * amount) + backColor.G * (1 - amount));
            byte b = (byte)((color.B * amount) + backColor.B * (1 - amount));
            return Color.FromArgb(a, r, g, b);
        }

        private static readonly SolidColorBrush Normal = new SolidColorBrush(Colors.Lavender);
        private static readonly SolidColorBrush Warning = new SolidColorBrush(Colors.Yellow);
        private static readonly SolidColorBrush Critical = new SolidColorBrush(Colors.Red);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = System.Convert.ToDouble(value);
            //return new SolidColorBrush(Blend(Colors.Yellow, Colors.Transparent, val));
            if (val < 33) return Normal;
            if (val < 66) return Warning;
            return Critical;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
