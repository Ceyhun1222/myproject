using System;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace EnrouteChartCompare.Helper
{
    public class AutoScrollBehavior : Behavior<ScrollViewer>
    {
        private ScrollViewer _scrollViewer = null;
        private double _height = 0.0d;

        protected override void OnAttached()
        {
            base.OnAttached();

            _scrollViewer = AssociatedObject;
            _scrollViewer.LayoutUpdated += new EventHandler(_scrollViewer_LayoutUpdated);
        }

        private void _scrollViewer_LayoutUpdated(object sender, EventArgs e)
        {
            if (_scrollViewer.ExtentHeight != _height)
            {
                _scrollViewer.ScrollToVerticalOffset(_scrollViewer.ExtentHeight);
                _height = _scrollViewer.ExtentHeight;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (_scrollViewer != null)
                _scrollViewer.LayoutUpdated -= new EventHandler(_scrollViewer_LayoutUpdated);
        }
    }
}
