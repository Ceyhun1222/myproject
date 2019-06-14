using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Aran.Panda.RadarMA.Converter
{
    class DistanceDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GlobalParams.UnitConverter.DistanceToDisplayUnits((double) value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
             if (double.TryParse(value.ToString(),out double displayDistance))
                return GlobalParams.UnitConverter.DistanceToInternalUnits(displayDistance);

            return 0;
        }
    }
}
