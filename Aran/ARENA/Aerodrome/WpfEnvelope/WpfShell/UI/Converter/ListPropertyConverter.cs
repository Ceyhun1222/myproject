using Aerodrome.Features;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfEnvelope.WpfShell.UI.Converter
{
    public class ListPropertyConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<AM_AbstractFeature> result = new List<AM_AbstractFeature>();
            foreach (var item in (IList)value)
            {
                result.Add((AM_AbstractFeature)item);
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
