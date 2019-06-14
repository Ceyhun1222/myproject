using System.Windows;

namespace Aran.Temporality.CommonUtil.Extender
{
    public static class ToolBarExtender
    {
        public static bool GetPreventOverflow(DependencyObject obj)
        {
            return (bool)obj.GetValue(CanOverflowProperty);
        }


        public static void SetPreventOverflow(DependencyObject obj, bool value)
        {
            obj.SetValue(CanOverflowProperty, value);
        }


        public static readonly DependencyProperty CanOverflowProperty =
            DependencyProperty.RegisterAttached(
             "PreventOverflow", typeof(bool), typeof(ToolBarExtender),
             new UIPropertyMetadata(false, OnPreventOverflowPropertyChanged));


        private static void OnPreventOverflowPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uie = (UIElement)d;
            if ((bool)e.NewValue)
            {
               
            }
        }
    }
}
