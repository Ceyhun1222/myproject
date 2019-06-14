using System;
using System.Windows.Data;
using TOSSM.ViewModel.Pane.Base;

namespace TOSSM.Converter
{
    class ActiveDocumentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DocViewModel)
                return value;


            if (value is ToolViewModel)
                return value;


            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DocViewModel)
                return value;

            if (value is ToolViewModel)
                return value;

            return Binding.DoNothing;
        }
    }
}
