using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Aran.Omega.Models
{
    class YesNoBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool) value)
                    return "Yes";
                return "No";
            }
            return "No";

            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (value?.ToString().ToUpper())
            {
                case "YES":
                    return true;
                case "NO":
                    return false;
            }
            return false;
        }
    }
}
