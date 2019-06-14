using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace WpfEnvelope.WpfShell.UI.Controls
{
    [StyleTypedProperty(Property = "PropertyDescriptionStyle", StyleTargetType = typeof(TextBlock))]
    public class KeyValueGrid : Panel
    {
        static KeyValueGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(KeyValueGrid),
                new FrameworkPropertyMetadata(typeof(KeyValueGrid)));
        }

        #region PropertyDescriptionStyle

        public Style PropertyDescriptionStyle
        {
            get { return (Style)GetValue(PropertyDescriptionStyleProperty); }
            set { SetValue(PropertyDescriptionStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PropertyDescriptionStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropertyDescriptionStyleProperty =
            DependencyProperty.Register("PropertyDescriptionStyle", typeof(Style), typeof(KeyValueGrid), new UIPropertyMetadata(new Style()));

        #endregion

        #region Orientation

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(KeyValueGrid), new UIPropertyMetadata(Orientation.Vertical));

        #endregion

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            Point lastPoint = new Point(0, 0);

            // H
            double keyColumnWidth = 0.0;
            double valueColumnWidth = 0.0;

            // V
            double keyRowHeight = 0.0;
            double valueRowHeight = 0.0;

            // determine the width of the key and value columns
            for (int i = 0; i < Children.Count; i = i + 2)
            {
                var key = Children[i];
                var value = Children[i + 1];

                // H
                if (key.DesiredSize.Width > keyColumnWidth)
                    keyColumnWidth = key.DesiredSize.Width;
                if (value.DesiredSize.Width > valueColumnWidth)
                    valueColumnWidth = value.DesiredSize.Width;

                // V
                if (key.DesiredSize.Height > keyRowHeight)
                    keyRowHeight = key.DesiredSize.Height;
                if (value.DesiredSize.Height > valueRowHeight)
                    valueRowHeight = value.DesiredSize.Height;
            }

            for (int i = 0; i < Children.Count; i = i + 2)
            {
                var key = Children[i];
                var value = Children[i + 1];

                if (Orientation == Orientation.Horizontal)
                {
                    //newPoint.X = lastPoint.X + key.DesiredSize.Width + value.DesiredSize.Width;
                    throw new NotImplementedException("Horizontal alignment is not implemented.");
                }
                else
                {
                    // IMP: Ich zentriere IMMER vertikal. Besser: Komplettes Layout mir Grid machen, um die Layout-Funktionalitäten des Grid zu nutzen.
                    double keyCenterOffset = 0.0;
                    double valueCenterOffset = 0.0;
                    double desiredHeight = 0.0;
                    if (key.DesiredSize.Height > value.DesiredSize.Height)
                    {
                        desiredHeight = key.DesiredSize.Height;
                        valueCenterOffset = (desiredHeight - value.DesiredSize.Height) / 2.0;
                    }
                    else
                    {
                        desiredHeight = value.DesiredSize.Height;
                        keyCenterOffset = (desiredHeight - key.DesiredSize.Height) / 2.0;
                    }

                    // Umbruch checken
                    if (lastPoint.Y + desiredHeight > arrangeSize.Height)
                    {
                        lastPoint.Y = 0.0;
                        lastPoint.X += keyColumnWidth + valueColumnWidth;
                    }

                    key.Arrange(new Rect(
                        new Point(
                            lastPoint.X,
                            lastPoint.Y + keyCenterOffset),
                        new Point(
                            lastPoint.X + keyColumnWidth,
                            lastPoint.Y + desiredHeight)));
                    value.Arrange(new Rect(
                        new Point(
                            lastPoint.X + keyColumnWidth,
                            lastPoint.Y + valueCenterOffset),
                        new Point(
                            lastPoint.X + keyColumnWidth + valueColumnWidth,
                            lastPoint.Y + desiredHeight)));

                    lastPoint.Y = lastPoint.Y + desiredHeight;
                }

                if (key is TextBlock)
                    ((TextBlock)key).Style = PropertyDescriptionStyle;
            }

            return base.ArrangeOverride(arrangeSize);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            foreach (UIElement it in Children)
                it.Measure(new Size(constraint.Width, constraint.Height));

            return base.MeasureOverride(constraint);
        }
    }
}
