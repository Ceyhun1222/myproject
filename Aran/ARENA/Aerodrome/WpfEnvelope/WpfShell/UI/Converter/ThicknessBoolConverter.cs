using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace WpfEnvelope.WpfShell.UI.Converter
{
	[ValueConversion(typeof (bool), typeof (Thickness))]
	public class ThicknessBoolConverter : IValueConverter
	{
		public Thickness TrueThickness { get; set; }
		public Thickness FalseThickness { get; set; }

		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var b = (bool)value;
			if (b)
				return TrueThickness;
			else
				return FalseThickness;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}