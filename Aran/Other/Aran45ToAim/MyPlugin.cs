using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.AranEnvironment;
using System.Windows.Forms;

namespace Aran45ToAixm
{
    public class MyPlugin : AranPlugin
    {
        private Form _mainForm;


        public override void Startup (IAranEnvironment aranEnv)
        {
            Global.AranEvn = aranEnv;

            aranEnv.AranUI.AddMenuItem (AranMapMenu.Applications,
                new ToolStripMenuItem ("Aran4.5 To AIM", null, new EventHandler (MenuItem_Clicked)));
        }

        public void MenuItem_Clicked (object sender, EventArgs e)
        {
            if (_mainForm == null || _mainForm.IsDisposed)
            {
                _mainForm = new MainForm2 ();
                _mainForm.Show (Global.AranEvn.Win32Window);
            }

            _mainForm.Visible = true;
            _mainForm.Activate ();
        }

        public override Guid Id
        {
            get { return new Guid("52e7ab7a-5770-487f-8d5e-a78794d1995f"); }
        }

        public override string Name
        {
            get { return "Aran-4.5 to IAIM Database"; }
        }
    }
}