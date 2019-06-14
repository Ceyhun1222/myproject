using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Aran.Temporality.CommonUtil.Control.TreeViewDragDrop;
using TOSSM.ViewModel.Tool;

namespace TOSSM.View.Tool
{
    /// <summary>
    /// Interaction logic for FeatureSelector.xaml
    /// </summary>
    public partial class FeatureSelector 
    {
        public FeatureSelector()
        {
            InitializeComponent();
        }

        Point _lastMouseDown;
        object _draggedItem, _target;

        private void TreeViewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _lastMouseDown = e.GetPosition(tvParameters);
            }
        }

        private void TreeViewMouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Point currentPosition = e.GetPosition(tvParameters);

                    
                    if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 10.0) || (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 10.0))
                    {
                        _draggedItem = tvParameters.SelectedItem;
                        if (_draggedItem != null)
                        {
                            DragDropEffects finalDropEffect = DragDrop.DoDragDrop(tvParameters, tvParameters.SelectedValue, DragDropEffects.Move);
                            //Checking target is not null and item is dragging(moving)
                            if ((finalDropEffect == DragDropEffects.Move) && (_target != null))
                            {
                                var sourceModel = GetModel(_draggedItem);
                                var targetModel = GetModel(_target);

                                if (sourceModel != null && targetModel != null && sourceModel != targetModel)
                                {
                                    var parentModel=sourceModel.SelectedItemHolder as FeatureSelectorToolViewModel;
                                    if (parentModel!=null)
                                    {
                                        parentModel.PerformDragDrop(sourceModel, targetModel);
                                    }
                                    
                                    _target = null;
                                    _draggedItem = null;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
            }
        }


        private void TreeViewDragOver(object sender, DragEventArgs e)
        {
            try
            { 
                var currentPosition = e.GetPosition(tvParameters);

                if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 10.0) ||
                    (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 10.0))
                {
                    var item = GetNearestContainer(e.OriginalSource as UIElement);
                    e.Effects = CheckDropTarget(_draggedItem, item) ? DragDropEffects.Move : DragDropEffects.None;
                   
                }

                tvParameters.ScrollIfNeeded(currentPosition);
                e.Handled = true;
            }
            catch (Exception)
            {
            }
        }

        private void TreeViewDrop(object sender, DragEventArgs e)
        {
            try
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;

                // Verify that this is a valid drop and then store the drop target
                var targetItem = GetNearestContainer(e.OriginalSource as UIElement);
                if (targetItem != null && _draggedItem != null )
                {
                    _target = targetItem;
                    e.Effects = DragDropEffects.Move;
                }
            }
            catch (Exception)
            {
            }

        }

        private static FeatureTreeViewItemViewModel GetModel(object targetItem)
        {
            if (targetItem is TreeViewItem)
            {
                return (targetItem as TreeViewItem).DataContext as FeatureTreeViewItemViewModel;
            }
            if (targetItem is FeatureTreeViewItemViewModel)
            {
                return targetItem as FeatureTreeViewItemViewModel;
            }
            return null;
        }

        private static bool CheckDropTarget(object sourceItem, object targetItem)
        {
            var targetModel = GetModel(targetItem);
            var sourceModel = GetModel(sourceItem);

            if (sourceModel == null) return false;
            if (targetModel == null) return false;

            if (targetModel == sourceModel) return false;

            return true;
        }

        private static TreeViewItem GetNearestContainer(UIElement element)
        {
            // Walk up the element tree to the nearest tree view item.
            var container = element as TreeViewItem;
            while ((container == null) && (element != null))
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
                container = element as TreeViewItem;
            }
            return container;
        }
    }
}
