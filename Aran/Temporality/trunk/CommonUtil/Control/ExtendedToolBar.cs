using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Aran.Temporality.CommonUtil.Extender;

namespace Aran.Temporality.CommonUtil.Control
{
    public class ExtendedToolBar : ToolBar
    {
        public ExtendedToolBar()
        {
            Loaded += (sender, e) =>
                          {
                              var toolBar = sender as ToolBar;
                              if (toolBar==null) return;

                              if (!ToolBarExtender.GetPreventOverflow(toolBar)) return;

                              if (Items!=null)
                              {
                                  foreach (var item in Items.OfType<UIElement>())
                                  {
                                      item.SetValue(OverflowModeProperty, OverflowMode.Never);
                                  }
                              }
                             
                              
                              var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
                              if (overflowGrid != null)
                              {
                                  overflowGrid.Visibility = Visibility.Collapsed;
                              }
                              

                              var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
                              if (mainPanelBorder != null)
                              {
                                  mainPanelBorder.Margin = new Thickness();
                              }
                          };
        }
    }
}
