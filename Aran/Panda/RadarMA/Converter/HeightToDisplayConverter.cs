using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Aran.PANDA.Common;

namespace Aran.Panda.RadarMA.Converter
{
    class HeightToDisplayConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GlobalParams.UnitConverter.HeightToDisplayUnits((double)value,eRoundMode.CEIL);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.TryParse(value.ToString(),out double diplayHeight))
                return GlobalParams.UnitConverter.HeightToInternalUnits(diplayHeight);
            return 0;
        }
    }
}
