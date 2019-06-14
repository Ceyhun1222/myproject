using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace WpfEnvelope.WpfShell.UI.Converter
{
	[ValueConversion(typeof (DateTime), typeof (string))]
	public class DateTimeCultureConverter : IValueConverter
	{
		public bool UseShortDateString { get; set; }

		#region IValueConverter Members

		public object Convert(object value,
		                      Type targetType,
		                      object parameter,
		                      CultureInfo culture)
		{
			var date = (DateTime)value;
			if (UseShortDateString)
				return date.ToShortDateString();
			else
				return date.ToString();
		}

		public object ConvertBack(object value,
		                          Type targetType,
		                          object parameter,
		                          CultureInfo culture)
		{
			var strValue = value.ToString();
			DateTime resultDateTime;
			if (DateTime.TryParse(strValue, out resultDateTime))
				return resultDateTime;
			return value;
		}

		#endregion
	}
}