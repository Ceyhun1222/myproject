using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TOSSM.Control
{
    public class ChildListView : ListView
    {
        public ChildListView()
        {
            PreviewMouseWheel += (sender, e) =>
            {
                if (e.Handled) return;

                var listView = sender as ListView;
                if (listView == null) return;


                var parent = listView.Parent as UIElement;

                if (parent == null) return;

                e.Handled = true;

                var eventArg = new MouseWheelEventArgs(e.MouseDevice,
                    e.Timestamp, e.Delta)
                {
                    RoutedEvent = MouseWheelEvent,
                    Source = parent
                };

                parent.RaiseEvent(eventArg);
            };

        }
    }
}
