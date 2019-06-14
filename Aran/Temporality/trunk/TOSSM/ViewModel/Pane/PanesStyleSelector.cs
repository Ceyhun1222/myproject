using System.Windows;
using System.Windows.Controls;
using TOSSM.ViewModel.Pane.Base;

namespace TOSSM.ViewModel.Pane
{
    class PanesStyleSelector : StyleSelector
    {
        public Style ToolStyle { get; set; }

        public Style DocumentStyle { get; set; }
        
        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is ToolViewModel)
                return ToolStyle;

            if (item is DocViewModel)
                return DocumentStyle;

            return base.SelectStyle(item, container);
        }
    }
}
