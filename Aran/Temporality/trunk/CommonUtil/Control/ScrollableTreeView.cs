using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Aran.Temporality.CommonUtil.Control
{
    public class ScrollableTreeView : TreeView
    {
        private ScrollViewer m_scrollViewer;
        private ScrollViewer ScrollViewer
        {
            get
            {
                if (m_scrollViewer == null)
                {
                    DependencyObject border = VisualTreeHelper.GetChild(this, 0);
                    if (border != null)
                    {
                        m_scrollViewer = VisualTreeHelper.GetChild(border, 0) as ScrollViewer;
                    }
                }

                return m_scrollViewer;
            }
        }

        public void ScrollIfNeeded(Point mouseLocation)
        {
            if (this.ScrollViewer != null)
            {
                double scrollOffset = 0.0;
                double speed = 8;

                // See if we need to scroll down 
                if (this.ScrollViewer.ViewportHeight - mouseLocation.Y < 20.0)
                {
                    scrollOffset = speed;
                }
                else if (mouseLocation.Y < 20.0)
                {
                    scrollOffset = -speed;
                }

                // Scroll the tree down or up 
                if (Math.Abs(scrollOffset - 0.0) > 0.1)
                {
                    scrollOffset += this.ScrollViewer.VerticalOffset;

                    if (scrollOffset < 0.0)
                    {
                        scrollOffset = 0.0;
                    }
                    else if (scrollOffset > this.ScrollViewer.ScrollableHeight)
                    {
                        scrollOffset = this.ScrollViewer.ScrollableHeight;
                    }

                    this.ScrollViewer.ScrollToVerticalOffset(scrollOffset);
                }
            }
        }
    }
}
