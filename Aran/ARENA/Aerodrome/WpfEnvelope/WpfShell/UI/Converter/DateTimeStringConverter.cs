using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace WpfEnvelope.WpfShell.UI.Converter
{
	[ValueConversion(typeof (DateTime), typeof (string))]
	public class DateTimeStringConverter : IValueConverter
	{
		public DateTimeStringConverter()
		{
			UseThreadCulture = false;
		}

		public bool UseThreadCulture { get; set; }
		public string Format { get; set; }

		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var dtime = (DateTime)value;
			if (!UseThreadCulture)
				return dtime.ToString(Format);
			else
				return dtime.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}