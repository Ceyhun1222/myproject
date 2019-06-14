using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace BRuleManager
{
    public interface IWindowService
    {
        void ShowWindow(object viewModel, Window owner = null);
    }

    public static class WindowService
    {
        static WindowService()
        {
            Instance = new WindowDataTemplateService();
        }

        public static IWindowService Instance { get; private set; }
    }

    public class WindowDataTemplateService : IWindowService
    {
        public void ShowWindow(object viewModel, Window owner)
        {
            var win = new Window
            {
                Content = viewModel
            };

            bool isModal = false;

            if (viewModel is ExpandoObject eo)
            {
                dynamic eoViewModel = viewModel as dynamic;
                var modelType = eoViewModel._type;

                win.ContentTemplate = Application.Current.TryFindResource("model_" + modelType) as DataTemplate;

                var size = eo.GetValue<Size>("_defaultSize");
                if (!size.IsEmpty)
                {
                    if (size.Width == 1.1)
                    {
                        win.WindowState = WindowState.Maximized;
                    }
                    else
                    {
                        win.Width = size.Width;
                        win.Height = size.Height;
                    }
                }

                win.Title = eoViewModel._windowTitle;


                eoViewModel.CloseRequested = new Action<object>((arg) => win.Close());

                eoViewModel._windowIsLoadingChanged = new Action<object>((arg) =>
                {
                    win.Cursor = (arg as dynamic)._windowIsLoading ? Cursors.Wait : Cursors.Arrow;
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                                          new Action(delegate { }));
                });


                eoViewModel._window = win;
                if (owner != null)
                    win.Owner = owner;

                win.Closed += (sender, e) => eoViewModel._window = null;

                win.ShowInTaskbar = eo.GetValue<bool>("_showInTaskbar", true);
                win.ResizeMode = eo.GetValue<ResizeMode>("_resizeMode", ResizeMode.CanResizeWithGrip);

                isModal = eo.GetValue<bool>("_isModal", isModal);
            }

            if (isModal)
                win.ShowDialog();
            else
                win.Show();
        }
    }
}
