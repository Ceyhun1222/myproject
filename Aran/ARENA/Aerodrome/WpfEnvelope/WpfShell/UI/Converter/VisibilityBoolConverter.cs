using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace WpfEnvelope.WpfShell.UI.Converter
{
	[ValueConversion(typeof (bool), typeof (Visibility))]
	public class VisibilityBoolConverter : IValueConverter
	{
		public bool Invert { get; set; }

		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var b = (bool)value;
			var result = Visibility.Visible;

			if (Invert)
			{
				if (b)
					result = Visibility.Visible;
				else
					result = Visibility.Hidden;
			}
			else
			{
				if (b)
					result = Visibility.Hidden;
				else
					result = Visibility.Visible;
			}

			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}