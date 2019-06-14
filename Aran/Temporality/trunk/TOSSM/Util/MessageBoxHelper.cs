using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using TOSSM.View;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace TOSSM.Util
{
    
    public class MessageBoxHelper
    {
       

        public static MessageBoxResult Show(string message, string caption, MessageBoxButton button,MessageBoxImage image)
        {
            try
            {
                if (Application.Current != null)
                {
                    if (Application.Current.Dispatcher.CheckAccess())
                    {
                        return MessageBox.Show(Application.Current.MainWindow, message, caption, button, image);
                    }

                    var result = MessageBoxResult.Cancel;

                    try
                    {
                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            result = MessageBox.Show(Application.Current.MainWindow, message, caption, button, image);
                        }));

                    }
                    catch//some times problem occurs before main window is created, so can not use Application.Current.MainWindow
                    {
                    }
                   
                    return result;
                }
            }
            catch (Exception exception)
            {

            }

            var result2 = MessageBoxResult.Cancel;
            var thread = new Thread(() =>
            {
               
                var dummyWindow = new DummyWindow();
                //show it
                dummyWindow.Show();
                result2 = MessageBox.Show(dummyWindow, message, caption, button, image);
                dummyWindow.Hide();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            return result2;
        }

        public static MessageBoxResult Show(string message)
        {
            return Show(message, "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}
