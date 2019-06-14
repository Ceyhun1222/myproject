using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TOSSM.Control;
using TOSSM.ViewModel.Document;
using TOSSM.ViewModel.Pane.Base;

namespace TOSSM.ViewModel.Tool.PropertyPrecision.Util
{
    public abstract class HierarchyToolViewerModel : ToolViewModel
    {
        #region Ctor

        protected HierarchyToolViewerModel(string name) : base(name)
        {
        }

        #endregion

        #region UI references


        public static HierarchyControl GetRootParent(HierarchyControl control)
        {
            if (control == null) return null;
            var model = control.DataContext as HierarchyDocViewerModel;
            if (model == null) return control;
            if (model.ParentViewer == null) return control;
            return GetRootParent(model.ParentViewer);
        }

        public void MoveHorisontalScrollToRight()
        {
            var parent = GetRootParent(ParentViewer);
            if (parent==null) return;
            if (parent.HorisontalScrollViewer==null) return;
            parent.HorisontalScrollViewer.ScrollToRightEnd();
        }

        public abstract void OnCurrentViewerSet();

        private HierarchyControl _currentViewer;
        public HierarchyControl CurrentViewer
        {
            get => _currentViewer;
            set
            {
                _currentViewer = value;
                OnCurrentViewerSet();
            }
        }

        private HierarchyControl _complexContent;
        public HierarchyControl ComplexContent
        {
            get => _complexContent;
            set
            {
                _complexContent = value;
                if (ComplexContent!=null)
                {
                    if (ComplexContent.Visibility==Visibility.Visible)
                    {
                        MoveHorisontalScrollToRight();
                    }
                }
            }
        }

        private HierarchyControl _parentViewer;
        public HierarchyControl ParentViewer
        {
            get => _parentViewer;
            set
            {
                _parentViewer = value;
                if (ParentViewer==null)
                {
                    ParentModel = null;
                }
                else
                {
                    ParentModel = ParentViewer.DataContext as HierarchyDocViewerModel;
                    //ParentViewer.DataContextChanged += (sender, b) =>
                    //                                       {
                    //                                           var control = sender as HierarchyControl;
                    //                                           if (control != null)
                    //                                           {
                    //                                                ParentModel = control.DataContext as HierarchyDocViewerModel;
                    //                                           } 
                    //                                       };
                }
            }
        }

        public HierarchyDocViewerModel ParentModel { get; set; }

        #endregion

        #region Focus operations

        public void FocusRight()
        {
            if (ComplexContent == null) return;
            SetFocusToDatagrid(ComplexContent.FocusableDataGrid);
        }

        public void FocusLeft()
        {
            if (ParentViewer == null) return;
            SetFocusToDatagrid(ParentViewer.FocusableDataGrid);
        }

        public static void SetFocusToDatagrid(DataGrid myDataGrid)
        {
            if (myDataGrid == null) return;
            if (!myDataGrid.IsVisible) return;
            if (myDataGrid.Items == null) return;
            if (myDataGrid.Items.Count <= 0) return;

            try
            {
                if (myDataGrid.SelectedIndex == -1)
                {
                    myDataGrid.SelectedIndex = 0;
                }

                myDataGrid.SelectedItem = myDataGrid.Items[myDataGrid.SelectedIndex];
                myDataGrid.ScrollIntoView(myDataGrid.Items[myDataGrid.SelectedIndex]);
                var dgrow = (DataGridRow)myDataGrid.ItemContainerGenerator.ContainerFromItem(myDataGrid.Items[myDataGrid.SelectedIndex]);
                dgrow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
            catch
            {
            }
        }

        #endregion

        public abstract void UpdateByChildren();
    }
}
