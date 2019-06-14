using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Media;

namespace Aran.Aim.FeatureInfo
{
    internal class TextLengerConverter : IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            TextBlock tb = value as TextBlock;
            
            if (tb == null)
                return Binding.DoNothing;

            var formattedText = new FormattedText (
                tb.Text, culture, tb.FlowDirection,
                new Typeface (
                    tb.FontFamily, tb.FontStyle, tb.FontWeight, tb.FontStretch),
                tb.FontSize, Brushes.Black);

            return (formattedText.Width > tb.Width - 10);
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException ();
        }
    }

    internal class DD2DMSConverter : IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.IsNaN((double)value))
                return "<null>";

            double x = (double) value;
            double xDeg, xMin, xSec;
            int sign = Math.Sign (x);
            Global.DD2DMS (x, out xDeg, out xMin, out xSec, sign);
			
            string degStr = xDeg.ToString ();
            int strLen = 2;
            string signSymb = "SN";

            if ("x".Equals (parameter))
            {
                strLen = 3;
                signSymb = "WE";
            }

            while (degStr.Length < strLen)
                degStr = "0" + degStr;

            return string.Format ("{0}°{1}'{2}\" {3}", degStr, xMin, xSec, signSymb [(sign + 1) >> 1]);
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException ();
        }
    }

    internal class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isVisible = (bool)value;
            if ("not".Equals(parameter))
                isVisible = !isVisible;

            return isVisible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
