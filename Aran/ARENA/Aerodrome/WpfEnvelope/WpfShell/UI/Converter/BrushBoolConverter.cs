using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfEnvelope.WpfShell.UI.Converter
{
	[ValueConversion(typeof(bool), typeof(Brush))]
	public class BrushBoolConverter : IValueConverter
	{
		public Brush TrueBrush { get; set; }
		public Brush FalseBrush { get; set; }

		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			bool b = (bool)value;
			if (b)
				return TrueBrush;
			else
				return FalseBrush;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
