using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Aran.Omega.Enums;

namespace Aran.Omega.UIConverters
{
    class UIDistanceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var val = System.Convert.ToDouble(value);
                return Common.ConvertDistance(val, RoundType.ToNearest).ToString();
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var val = System.Convert.ToDouble(value);
                return Common.DeConvertDistance(val);
            }

            return 0;
        }
    }
}
