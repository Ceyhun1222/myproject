using System.Windows;
using System.Windows.Controls;
using TOSSM.ViewModel.Tool.PropertyPrecision.Single;

namespace TOSSM.ViewModel.Tool.PropertyPrecision.Util
{
    class PrecisionEditorPropertyTemplateSelector : DataTemplateSelector
    {
        public static PrecisionEditorPropertyTemplateSelector Instance=new PrecisionEditorPropertyTemplateSelector();

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            // ReSharper disable HeuristicUnreachableCode
            if (item == null) return null;
            // ReSharper restore HeuristicUnreachableCode
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            var element = container as ContentPresenter;
            if (element != null)
            {
                if (item is DoublePropertyPrecisionViewModel)
                {
                    return element.FindResource("DoublePrecisionFormatTemplate") as DataTemplate;
                }
                if (item is ValPropertyPrecisionViewModel)
                {
                    return element.FindResource("ValPrecisionFormatTemplate") as DataTemplate;
                }
                if (item is ComplexPropertyPrecisionViewModel)
                {
                    return element.FindResource("EmptyTemplate") as DataTemplate;
                }
                return null;
            }
            return null;
        }
    }
}
