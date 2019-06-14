using System;
using Aran.AranEnvironment;
using System.Windows.Forms;
using Aran.PANDA.Common;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using System.Windows.Threading;
using Aran.Panda.RNAV.RNPAR;
using Aran.Panda.RNAV.RNPAR.UI.View;
using Aran.Panda.RNAV.RNPAR.UI.ViewModel;

namespace Aran.Panda.RNAV.RNPAR
{
    public class RNPARPlugin : AranPlugin
    {
        private int _currCmd;
        private IAranEnvironment aranEnv;
        public override Guid Id => new Guid("620f77dc-f6f1-43bf-9dec-d75d7c5f7966");

        public override string Name => "RNP AR Approach";

        public override void Startup(IAranEnvironment aranEnv)
        {
            _currCmd = -1;
            this.aranEnv = aranEnv;

            var validationMenuItem = new ToolStripMenuItem {Text = "RNP AR Approach" };

            ToolStripItem rnparValidationMenuItem = new ToolStripMenuItem();
            rnparValidationMenuItem.Text = "RNP AR Approach";
            rnparValidationMenuItem.Tag = 0;
            rnparValidationMenuItem.Click += RNPARClick;
            validationMenuItem.DropDownItems.Add(rnparValidationMenuItem);

            aranEnv.AranUI.AddMenuItem(AranMapMenu.Applications, validationMenuItem);
        }

        MainView _mainView;
        private void RNPARClick(object sender, EventArgs e)
        {

            if (_currCmd == (int)((ToolStripMenuItem)sender).Tag)
                return;
        
            _currCmd = -1;


            try
            {
                Context.AppEnvironment.Init(aranEnv);
                NativeMethods.ShowPandaBox(Context.AppEnvironment.Current.SystemContext.EnvWin32Window.ToInt32());       
                Context.AppEnvironment.Current.DataContext.Load();

                if (_mainView == null)
                {
                    _mainView = new MainView();
                    _mainView.Closed += MainView_Closed;
                    Context.AppEnvironment.Current.ApplicationContext.MainViewModel = new MainViewModel();
                    _mainView.DataContext = Context.AppEnvironment.Current.ApplicationContext.MainViewModel;
                    ElementHost.EnableModelessKeyboardInterop(_mainView);
                    var helper = new WindowInteropHelper(_mainView) { Owner = aranEnv.Win32Window.Handle };
                    Context.AppEnvironment.Current.SystemContext.SetWin32Window(aranEnv.Win32Window);
                    _mainView.ShowInTaskbar = false;
                    _mainView.Closing += RNPClosing;
                }

                Dispatcher.CurrentDispatcher.Invoke(new Action(() => _mainView.Show()));

                
                _currCmd = -1;
                NativeMethods.HidePandaBox();
            }
            catch (Exception ex)
            {
                _currCmd = -1;
                //Context.Environment.Current.Logger.Error(ex, "Error on opening validation tool");
                NativeMethods.HidePandaBox();
                _mainView = null;
                var tsmi = sender as ToolStripMenuItem;
                MessageBox.Show(ex.Message, tsmi.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RNPClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var mainView = sender as MainView;
            var context = mainView?.DataContext as MainViewModel;
            context?.Destroy();
        }

        private void MainView_Closed(object sender, EventArgs e)
        {
            // Engine.Environment.Current.Logger.Info("Validation tool closed", null);
            _currCmd = -1;
            //((MainViewModel)_mainView.DataContext).Destroy();
            _mainView = null;
            //CurrentEnvironment.Delete();
        }
    }
}
;