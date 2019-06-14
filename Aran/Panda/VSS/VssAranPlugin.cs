using Aran.AranEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aran.PANDA.Common;

namespace Aran.PANDA.Vss
{
    public class VssAranPlugin : AranPlugin
    {
        private Guid _id;
        private string _name;
		private MainForm _mainForm;

		public VssAranPlugin()
        {
            _id = new Guid("b7e6ea77-720d-4360-9839-c9f378f9e213");
            _name = "PANDA - VSS";
        }

        public override Guid Id { get { return _id; } }

        public override void Startup(IAranEnvironment aranEnv)
        {
            Globals.AranEnv = aranEnv;
			Globals.SpatRefOperation = new SpatialReferenceOperation ( aranEnv );

			var tsmi = new ToolStripMenuItem();
            tsmi.Text = "PANDA - Visual Segment Surface";
            tsmi.Click += MenuItem_Click;

            aranEnv.AranUI.AddMenuItem(AranMapMenu.Applications, tsmi);
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            if (_mainForm == null || _mainForm.IsDisposed) {
                Globals.ClearSingletoneValues();
                _mainForm = new MainForm();
                _mainForm.Show(Globals.AranEnv.Win32Window);
            }
            else {
                _mainForm.Visible = true;
                _mainForm.WindowState = FormWindowState.Normal;
                _mainForm.Activate();
            }
        }

        public override string Name
        {
            get { return _name; }
        }
    }
}
