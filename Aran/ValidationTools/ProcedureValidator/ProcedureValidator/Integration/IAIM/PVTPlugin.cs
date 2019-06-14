using System;
using Aran.AranEnvironment;
using System.Windows.Forms;
using PVT.Engine.IAIM;
using Aran.PANDA.Common;
using PVT.UI.View;
using PVT.UI.ViewModel;
using System.Windows.Forms.Integration;
using System.Windows.Interop;

namespace PVT.Integration.IAIM
{
    public class PVTPlugin : AranPlugin
    {
        private Engine.Environment CurrentEnvironment;
        private IAranEnvironment aranEnv;
        public override Guid Id
        {
            get
            {
                return new Guid("620f77dc-f6f1-43bf-9dec-d75d7c5f79f5");
            }
        }

        public override string Name
        {
            get
            {
                return "Procedure Validation Tool";
            }
        }

        public override void Startup(IAranEnvironment aranEnv)
        {
            this.aranEnv = aranEnv;
            Engine.Environment.Current = new IAIMEnvironment(aranEnv);
            CurrentEnvironment = Engine.Environment.Current;

            var validationMenuItem = new ToolStripMenuItem {Text = "Validation Tools"};

            ToolStripItem procedureValidationMenuItem = new ToolStripMenuItem();
            procedureValidationMenuItem.Text = "Procedure Validation";
            procedureValidationMenuItem.Tag = 0;
            procedureValidationMenuItem.Click += ProcedureValidationClick;
            validationMenuItem.DropDownItems.Add(procedureValidationMenuItem);

            aranEnv.AranUI.AddMenuItem(AranMapMenu.Applications, validationMenuItem);
        }

        MainView _mainView;
        private void ProcedureValidationClick(object sender, EventArgs e)
        {
            if (CurrentEnvironment.CurrentCMD == 1)
                return;
            CurrentEnvironment.CurrentCMD = 0;

            try
            {
                CurrentEnvironment.Initialize();
                NativeMethods.ShowPandaBox(CurrentEnvironment.EnvWin32Window.ToInt32());

                if (_mainView == null)
                {
                    _mainView = new MainView();
                    _mainView.Closed += MainView_Closed;
                    _mainView.DataContext = new MainViewModel();
                    ElementHost.EnableModelessKeyboardInterop(_mainView);
                    var helper = new WindowInteropHelper(_mainView) { Owner = aranEnv.Win32Window.Handle };
                    ((IAIMEnvironment)CurrentEnvironment).SetWin32Window(helper.Handle);
                    _mainView.ShowInTaskbar = false;

                }

                Engine.Environment.Current.Logger.Info("Validation tool starting", null);
                _mainView.Show();
                CurrentEnvironment.CurrentCMD = 1;
                NativeMethods.HidePandaBox();
            }
            catch (Exception ex)
            {
                Engine.Environment.Current.Logger.Error(ex, "Error on opening validation tool");
                NativeMethods.HidePandaBox();
                _mainView = null;
                var tsmi = sender as ToolStripMenuItem;
                MessageBox.Show(ex.Message, tsmi.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainView_Closed(object sender, EventArgs e)
        {
            Engine.Environment.Current.Logger.Info("Validation tool closed", null);
            CurrentEnvironment.CurrentCMD = 0;
            ((MainViewModel)_mainView.DataContext).Destroy();
            _mainView = null;
            CurrentEnvironment.Delete();
        }
    }
}
;