using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Aran.Temporality.CommonUtil.Control
{
    public class AnimatedScrollViewer : ScrollViewer
    {
        public static DependencyProperty CurrentVerticalOffsetProperty = DependencyProperty.Register("CurrentVerticalOffset", typeof(double), typeof(AnimatedScrollViewer), new PropertyMetadata(OnVerticalChanged));
        public static DependencyProperty CurrentHorizontalOffsetProperty = DependencyProperty.Register("CurrentHorizontalOffset", typeof(double), typeof(AnimatedScrollViewer), new PropertyMetadata(OnHorizontalChanged));

        private static void OnVerticalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewer = d as AnimatedScrollViewer;
            if (viewer != null) viewer.ScrollToVerticalOffset((double)e.NewValue);
        }

        private static void OnHorizontalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewer = d as AnimatedScrollViewer;
            if (viewer != null) viewer.ScrollToHorizontalOffset((double)e.NewValue);
        }

        public double CurrentHorizontalOffset
        {
            get { return (double)GetValue(CurrentHorizontalOffsetProperty); }
            set { SetValue(CurrentHorizontalOffsetProperty, value); }
        }

        public double CurrentVerticalOffset
        {
            get { return (double)GetValue(CurrentVerticalOffsetProperty); }
            set { SetValue(CurrentVerticalOffsetProperty, value); }
        }

        public void ScrollToPositionAnimated(double x, double y, int ms)
        {
            var vertAnim = new DoubleAnimation
            {
                From = VerticalOffset,
                To = y,
                DecelerationRatio = .2,
                Duration = new Duration(TimeSpan.FromMilliseconds(ms))
            };
            var horzAnim = new DoubleAnimation
            {
                From = HorizontalOffset,
                To = x,
                DecelerationRatio = .2,
                Duration = new Duration(TimeSpan.FromMilliseconds(ms))
            };
            var storyboard = new Storyboard();
            storyboard.Children.Add(vertAnim);
            storyboard.Children.Add(horzAnim);
            Storyboard.SetTarget(vertAnim, this);
            Storyboard.SetTargetProperty(vertAnim, new PropertyPath(CurrentVerticalOffsetProperty));
            Storyboard.SetTarget(horzAnim, this);
            Storyboard.SetTargetProperty(horzAnim, new PropertyPath(CurrentHorizontalOffsetProperty));
            storyboard.Begin();
        }

        public void ScrollToRightEndAnimated(int ms)
        {
            ScrollToPositionAnimated(ScrollableWidth, 0, ms);
        }

    }
}
