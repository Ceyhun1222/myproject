using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace WpfEnvelope.WpfShell.UI.Converter
{
	[ValueConversion(typeof(ResizeMode), typeof(bool))]
	public class CanMaximizeConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var rm = (ResizeMode)value;
			if (rm == ResizeMode.CanResize || rm == ResizeMode.CanResizeWithGrip)
				return true;
			else
				return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value;
		}

		#endregion
	}
}
