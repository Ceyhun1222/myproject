using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Aran.Temporality.CommonUtil.Control;
using TOSSM.ViewModel.Document;
using TOSSM.ViewModel.Tool.PropertyPrecision.Util;

namespace TOSSM.Control
{

    public class HierarchyControl : UserControl
    {
        protected HierarchyControl()
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }
#endif

            Initialized += (a, b) =>
                               {
                                   DataContextChanged += (sender, e) =>
                                   {
                                       var model = DataContext as HierarchyDocViewerModel;
                                       if (model != null)
                                       {
                                           model.CurrentViewer = this;
                                       }

                                       var model2 = DataContext as HierarchyToolViewerModel;
                                       if (model2 != null)
                                       {
                                           model2.CurrentViewer = this;
                                       }
                                   };

                                   if (FocusableDataGrid!=null)
                                   {
                                       FocusableDataGrid.KeyUp += (sender, e) =>
                                       {
                                           var model1 = DataContext as HierarchyDocViewerModel;
                                           if (model1 != null)
                                           {
                                               if (e.Key == Key.Right)
                                               {
                                                   model1.FocusRight();
                                               }
                                               else if (e.Key == Key.Left)
                                               {
                                                   model1.FocusLeft();
                                               }
                                           }

                                           var model2 = DataContext as HierarchyToolViewerModel;
                                           if (model2 != null)
                                           {
                                               if (e.Key == Key.Right)
                                               {
                                                   model2.FocusRight();
                                               }
                                               else if (e.Key == Key.Left)
                                               {
                                                   model2.FocusLeft();
                                               }
                                           }

                                           
                                       };

                                       FocusableDataGrid.PreviewMouseWheel += (sender, e) =>
                                       {
                                           if (!(sender is DataGrid) || e.Handled) return;

                                           var model = DataContext as HierarchyDocViewerModel;
                                           if (model != null)
                                           {
                                               var parent = ((DataGrid)sender).Parent as UIElement;

                                               //if (model.ParentViewer != null)
                                               //{
                                               //    parent = model.ParentViewer.FocusableDataGrid;
                                               //}

                                               if (parent == null) return;

                                               e.Handled = true;

                                               var eventArg = new MouseWheelEventArgs(e.MouseDevice,
                                                                                      e.Timestamp, e.Delta)
                                               {
                                                   RoutedEvent = MouseWheelEvent,
                                                   Source = parent
                                               };

                                               parent.RaiseEvent(eventArg);
                                               return;
                                           }


                                           var model2 = DataContext as HierarchyToolViewerModel;
                                           if (model2 != null)
                                           {
                                               var parent = ((DataGrid)sender).Parent as UIElement;

                                               //if (model.ParentViewer != null)
                                               //{
                                               //    parent = model.ParentViewer.FocusableDataGrid;
                                               //}

                                               if (parent == null) return;

                                               e.Handled = true;

                                               var eventArg = new MouseWheelEventArgs(e.MouseDevice,
                                                                                      e.Timestamp, e.Delta)
                                               {
                                                   RoutedEvent = MouseWheelEvent,
                                                   Source = parent
                                               };

                                               parent.RaiseEvent(eventArg);
                                           }
                                          
                                       };
                                   }
                                  
                               };

            
        }

        #region Implementation of dummy properties

        public virtual DataGrid FocusableDataGrid => null;

        public virtual Panel SubHeaderPanel => null;

        public virtual AnimatedScrollViewer HorisontalScrollViewer => null;

        #endregion
    }
}
