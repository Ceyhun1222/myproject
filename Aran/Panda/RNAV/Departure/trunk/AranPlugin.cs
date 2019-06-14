using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Aran.Aim;
using Aran.AranEnvironment;
using Aran.PANDA.RNAV.Departure.Properties;
using Aran.Aim.Data;

namespace Aran.PANDA.RNAV.Departure
{
	public class DeparturePlugin : AranPlugin
	{
		const string _plaginName = "PANDA RNAV - Departure";
		private readonly Guid _guid = new Guid("8a709113-d7ca-443a-8648-f8fc8ad33200");

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

		void command1_Click(object sender, EventArgs e)
		{
			if (GlobalVars.CurrCmd == (int)((ToolStripMenuItem)sender).Tag)
				return;

			GlobalVars.CurrCmd = -1;
			DepartureForm mainForm;

			try
			{
                if (!SlotSelector())
                    return;
				ShowPandaBox();

				GlobalVars.CurrCmd = (int)((ToolStripMenuItem)sender).Tag;
				GlobalVars.InitCommand();

				mainForm = new DepartureForm();
				mainForm.FormClosed += AllForms_FormClosed;
				mainForm.Show(GlobalVars.Win32Window);
				mainForm.ComboBox001.Focus();
			}
			catch (Exception ex)
			{
				GlobalVars.CurrCmd = -1;
				GC.Collect();
				throw ex;
				//MessageBox.Show(ex.Message, ((ToolStripMenuItem)sender).Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				HidePandaBox();
				mainForm = null;
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

        public bool SlotSelector()
        {

            var slotSelector = true;

            var dbProvider = GlobalVars.gAranEnv.DbProvider as DbProvider;
            if (dbProvider != null && dbProvider.ProviderType == DbProviderType.TDB)
            {
                dynamic methodResult = new System.Dynamic.ExpandoObject();
                dbProvider.CallSpecialMethod("SelectSlot", methodResult);
                slotSelector = methodResult.Result;
            }

            if (!slotSelector)
            {
                MessageBox.Show("Please first select slot!", "Slot Selector", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return slotSelector;
            }
            return slotSelector;
        }

    }
}
