using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aran.AranEnvironment;

namespace Aran.Aim.Data.LocalDbLoader
{
    public class DbLoaderPlugin : AranPlugin
    {
        private Guid _id;

        public DbLoaderPlugin()
        {
            _id = new Guid("ee3161c8-0ac6-4a9a-a694-03e4e9165e98");
            //IsSystemPlugin = true;
        }

        public override Guid Id
        {
            get { return _id; }
        }

        public override void Startup(IAranEnvironment aranEnv)
        {
            Global.AranEnv = aranEnv;

            var tsmi = new ToolStripMenuItem("Cache Loader", null, OnLoaderItemClicked);
            aranEnv.AranUI.AddMenuItem(AranMapMenu.Other, tsmi, "ui_cadasMenuItem");
        }

        public override string Name
        {
            get { return "Cache Loader"; }
        }

        private void OnLoaderItemClicked(object sender, EventArgs e)
        {
            if (Global.LoaderForm == null)
            {
                var lf = new LoaderForm();
                Global.LoaderForm = lf;
                lf.Show(Global.AranEnv.Win32Window);
            }

            Global.LoaderForm.Visible = true;
            Global.LoaderForm.WindowState = FormWindowState.Normal;
        }
    }
}
