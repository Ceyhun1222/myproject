using Aran.AimEnvironment.Tools.View;
using Aran.AimEnvironment.Tools.ViewModel;
using Aran.AranEnvironment;
using System;
using System.Windows.Forms.Integration;
using System.Windows.Interop;

namespace Aran.AimEnvironment.Tools
{
    public sealed class MeasureAngleToolPlugin : AranPlugin
    {
        private readonly AranTool _aranTool;
        private readonly MeasureAngleToolForm _form;
        private IAranEnvironment _aranEnv;
        private AngleMeasureView _angleMeasureView;
        private AngleMeasureViewModel _angleMeasureViewModel;
        private bool _isClosedFromToolbar = false;
        System.Windows.Forms.ToolStripButton _toolStripButton;

        public MeasureAngleToolPlugin()
        {
            Id = new Guid("8a2faebf-3a11-4e1c-b822-7276da9861d0");

            _aranTool = new AranTool
            {
                Name = "Angle Meausure",
                Image = null,
                Cursor = System.Windows.Forms.Cursors.Cross
            };
            IsSystemPlugin = true;

            _form = new MeasureAngleToolForm();
            _form.Clicked += _form_Clicked;

        }

        private void _form_Clicked(object sender, EventArgs e)
        {
             _toolStripButton = sender as System.Windows.Forms.ToolStripButton;
            _aranEnv.AranUI.SetCurrentTool(_toolStripButton != null && _toolStripButton.Checked ? _aranTool : null);
            if (_toolStripButton != null && !_toolStripButton.Checked)
            {
                _isClosedFromToolbar = true;
                _angleMeasureViewModel.AngleMeasure.Stop();
                Close();
            }
            else
                Show();
        }


        public override Guid Id { get; }

        public override string Name
        {
            get { return "Angle Measure Tool"; }
        }

        public override void Startup(IAranEnvironment aranEnv)
        {
            _aranEnv = aranEnv;
            Context.Load(aranEnv);
            aranEnv.AranUI.AddMapTool(_aranTool);

            Toolbar = _form.Toolbar;

        }

        private void Show()
        {
            _angleMeasureView = new AngleMeasureView();
            _angleMeasureViewModel = new AngleMeasureViewModel();
            _angleMeasureView.DataContext = _angleMeasureViewModel;

            _aranTool.MouseClickedOnMap += _angleMeasureViewModel.AngleMeasure.OnMouseClickedOnMap;
            _aranTool.MouseMoveOnMap += _angleMeasureViewModel.AngleMeasure.OnMouseMoveOnMap;

            _angleMeasureView.Closing += _angleMeasureView_Closing;
            _angleMeasureView.Topmost = true;

            var helper = new WindowInteropHelper(_angleMeasureView) { Owner = _aranEnv.Win32Window.Handle };
            ElementHost.EnableModelessKeyboardInterop(_angleMeasureView);
            _angleMeasureView.ShowInTaskbar = false;
            _angleMeasureView.Show();
        }

        private void _angleMeasureView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_isClosedFromToolbar)
            {
                _toolStripButton.Checked = false;
                _aranEnv.AranUI.SetCurrentTool(null);
            }

            Dispose();
        }

        private void Close()
        {
            if (_isClosedFromToolbar)
                _angleMeasureView.Close();
        }

        private void Dispose()
        {
            _aranTool.MouseClickedOnMap -= _angleMeasureViewModel.AngleMeasure.OnMouseClickedOnMap;
            _aranTool.MouseMoveOnMap -= _angleMeasureViewModel.AngleMeasure.OnMouseMoveOnMap;
            _angleMeasureViewModel.Dispose();
            _angleMeasureView = null;
            _angleMeasureViewModel = null;
            _isClosedFromToolbar = false;
        }
    }
}
