using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Aran.Aim;
using TOSSM.Control;
using TOSSM.ViewModel.Pane.Base;

namespace TOSSM.ViewModel.Document
{
    public abstract class HierarchyDocViewerModel : DocViewModel
    {
        #region Ctor
        
        protected HierarchyDocViewerModel()
        {
        }

        protected HierarchyDocViewerModel(FeatureType type, Guid id, DateTime date)
            : base(type, id, date)
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
            if (parent?.HorisontalScrollViewer == null) return;

            var worker = new BackgroundWorker();
            worker.DoWork += (a, b) => { Thread.Sleep(300); };
            worker.RunWorkerCompleted += (a, b) =>
            {
               

                Application.Current.Dispatcher.Invoke(
                    DispatcherPriority.Background,
                    (Action)(
                        () =>
                        {
                            parent.HorisontalScrollViewer.ScrollToRightEndAnimated(150);
                        }));
            };
        
            worker.RunWorkerAsync();
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
                if (ComplexContent?.Visibility==Visibility.Visible)
                {
                    MoveHorisontalScrollToRight();
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
                // ignored
            }
        }

        #endregion

        public abstract void UpdateByChildren();

        //public abstract void UpdateListByChildren();
    }
}
