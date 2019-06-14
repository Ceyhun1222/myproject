using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Aran.Temporality.CommonUtil.Util
{
    public class WindowStarter
    {
        public Window CurrentWindow;

        public event RoutedEventHandler startupEvent;
        public event EventHandler exitEvent;

        public void LaunchWindow<T>() where T : Window, new()
        {
            var currentThread = new Thread(ThreadStartingPoint);
            currentThread.SetApartmentState(ApartmentState.STA);
            currentThread.IsBackground = true;
            Func<Window> f = () => new T();
            currentThread.Start(f);
        }

        private void ThreadStartingPoint(object t)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            var f = (Func<Window>)t;
            CurrentWindow = f();
            CurrentWindow.Loaded += startupEvent;
            CurrentWindow.Show();
            Dispatcher.Run();
        }

        public void ExitCurrentWindow()
        {
            CurrentWindow.Hide();
        }
    }
}
