using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aran.Aim;
using Aran.AranEnvironment;
using Aran.PANDA.RNAV.Approach.Properties;

namespace Aran.PANDA.RNAV.Approach
{
	public class ApproachPlugin : AranPlugin
	{
		const string _plaginName = "PANDA RNAV - Approach";
		private readonly Guid _guid = new Guid("B7511633-0F4F-4A0A-B9EB-5CD9D2EC4B36");

		public override Guid Id
		{
			get { return _guid; }
		}

		public override string Name
		{
			get { return _plaginName; }
		}

		public override void Startup(IAranEnvironment aranEnv)
		{
			GlobalVars.gAranEnv = aranEnv;
			GlobalVars.gAranGraphics = aranEnv.Graphics;

			//GlobalVars.gHookHelper = new ESRI.ArcGIS.Controls.HookHelper();
			//GlobalVars.gHookHelper.Hook = GlobalVars.gAranEnv.HookObject;

			ToolStripMenuItem menuItem = new ToolStripMenuItem();
			menuItem.Text = Resources.str00033;

			ToolStripMenuItem command1 = new ToolStripMenuItem();
			command1.Text = Resources.str00033;
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

		private void command1_Click(object sender, EventArgs e)
		{
			if (GlobalVars.CurrCmd == (int)((ToolStripMenuItem)sender).Tag)
				return;

			GlobalVars.CurrCmd = -1;

			try
			{
				ShowPandaBox();

				GlobalVars.CurrCmd = (int)((ToolStripMenuItem)sender).Tag;
				GlobalVars.InitCommand();

				ApproachForm mainForm = new ApproachForm();
				mainForm.FormClosed += AllForms_FormClosed;
				mainForm.Show(GlobalVars.Win32Window);
				//mainForm.ComboBox001.Focus();

				HidePandaBox();
				mainForm = null;
			}
			catch (Exception ex)
			{
				HidePandaBox();
				GlobalVars.CurrCmd = -1;
				throw ex;
				//MessageBox.Show(ex.Message, ((ToolStripMenuItem)sender).Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public override List<FeatureType> GetLayerFeatureTypes()
		{
			var list = new List<FeatureType>();

			list.Add(FeatureType.AirportHeliport);
			list.Add(FeatureType.RunwayCentrelinePoint);
			list.Add(FeatureType.DesignatedPoint);
			list.Add(FeatureType.VOR);
			list.Add(FeatureType.DME);
			list.Add(FeatureType.VerticalStructure);

			return list;
		}

		private void AllForms_FormClosed(object sender, FormClosedEventArgs e)
		{
			GlobalVars.CurrCmd = -1;
			sender = null;
			GC.Collect();
		}

		public static void ShowPandaBox()
		{
			Aran.PANDA.Common.NativeMethods.ShowPandaBox(GlobalVars.gAranEnv.Win32Window.Handle.ToInt32());
		}

		private static void HidePandaBox()
		{
			Aran.PANDA.Common.NativeMethods.HidePandaBox();
		}

	}
}
