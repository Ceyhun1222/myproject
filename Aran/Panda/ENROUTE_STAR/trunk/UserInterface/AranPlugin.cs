using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.AranEnvironment;
using System.Windows.Forms;
using Aran.Panda.EnrouteStar.Properties;

namespace Aran.Panda.EnrouteStar
{
    public class EnrouteStarPlugin : AranPlugin
    {
        const string PlaginName = "Enroute Star plugin";

		public EnrouteStarPlugin()
		{
			//_isInitCommands = false;
		}

		public override Guid Id { get { return new Guid("4412c852-98b4-4eba-ac3b-7b9c6c8315d8"); } }

        public override string Name { get { return PlaginName; } }

        public override void Startup(IAranEnvironment aranEnv)
        {
            GlobalVars.gAranEnv = aranEnv;
			GlobalVars.gHookHelper = new ESRI.ArcGIS.Controls.HookHelper();
			GlobalVars.gHookHelper.Hook = GlobalVars.gAranEnv.HookObject;
			GlobalVars.gAranGraphics = aranEnv.Graphics;

			//int LangCode = Functions.RegRead<int>(Registry.CurrentUser, GlobalVars.PandaRegKey, "LanguageCode", 1033);
			//NativeMethods.SetThreadLocale(LangCode);
			//Resources.Culture = new System.Globalization.CultureInfo(LangCode);

            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            menuItem.Text = "Enroute-STAR";

            ToolStripMenuItem command1 = new ToolStripMenuItem();
            command1.Text = "Enroute-STAR";
            command1.Tag = 0;
            command1.Click += new EventHandler(command1_Click);
            menuItem.DropDownItems.Add(command1);

            //ToolStripMenuItem command2 = new ToolStripMenuItem ();
            //command2.Text = "Command - 2";
            //command2.Tag = 1;
            //command2.Click += new EventHandler (command2_Click);
            //menuItem.DropDownItems.Add (command2);

            aranEnv.AranUI.AddMenuItem(AranMapMenu.Applications, menuItem);
        }

        void command1_Click(object sender, EventArgs e)
        {
            if (GlobalVars.CurrCmd == (int)((ToolStripMenuItem)sender).Tag)
                return;

            GlobalVars.CurrCmd = -1;

            try {
                GlobalVars.InitCommand();
                MainForm mainForm = new MainForm();
				mainForm.Show(GlobalVars.Win32Window);
				//mainForm.ComboBox001.Focus();
				GlobalVars.CurrCmd = (int)((ToolStripMenuItem)sender).Tag;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, ((ToolStripMenuItem)sender).Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
