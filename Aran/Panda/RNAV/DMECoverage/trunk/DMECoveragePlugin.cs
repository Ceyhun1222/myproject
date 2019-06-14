using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Aran.Aim;
using Aran.AranEnvironment;
using Aran.PANDA.RNAV.DMECoverage.Properties;
using Aran.Aim.Data;

namespace Aran.PANDA.RNAV.DMECoverage
{
	public class DMECoveragePlugin : AranPlugin
	{
		const string PlaginName = "PANDA RNAV - DMECoverage";
		const string guidString = "E92A18E6-376C-4DA8-8CAD-D97B1794BC87";
		public static int CurrCmd = -1;
		ToolStripMenuItem _tsmiArrival;

		public override Guid Id
		{
			get { return new Guid(guidString); }
		}

		public override string Name
		{
			get { return PlaginName; }
		}

		public override List<FeatureType> GetLayerFeatureTypes()
		{
			var list = new List<FeatureType>();

			//list.Add(FeatureType.AirportHeliport);
			//list.Add(FeatureType.RunwayCentrelinePoint);
			//list.Add(FeatureType.VOR);
			//list.Add(FeatureType.NDB);
			//list.Add(FeatureType.DME);

			list.Add(FeatureType.DesignatedPoint);
			list.Add(FeatureType.Navaid);
			list.Add(FeatureType.VerticalStructure);

			return list;
		}

		public static void ShowPandaBox()
		{
			Aran.PANDA.Common.NativeMethods.ShowPandaBox(GlobalVars.Win32Window.Handle.ToInt32());
		}

		private static void HidePandaBox()
		{
			Aran.PANDA.Common.NativeMethods.HidePandaBox();
		}

		public override void Startup(IAranEnvironment aranEnv)
		{
			GlobalVars.gAranEnv = aranEnv;
			GlobalVars.gAranGraphics = aranEnv.Graphics;

			ToolStripMenuItem menuItem = new ToolStripMenuItem();
			menuItem.Text = Resources.str00030;

			_tsmiArrival = new ToolStripMenuItem();
			_tsmiArrival.Text = Resources.str00031;
			_tsmiArrival.Tag = 0;

			_tsmiArrival.Click += new EventHandler(arrival_Click);
			menuItem.DropDownItems.Add(_tsmiArrival);

			aranEnv.AranUI.AddMenuItem(AranMapMenu.Applications, menuItem);
		}

		private void AllForms_FormClosed(object sender, FormClosedEventArgs e)
		{
			CurrCmd = -1;

			_tsmiArrival.Checked = false;
			_tsmiArrival.Enabled = true;
			Toolbar = null;
			sender = null;
			GC.Collect();
		}

		void arrival_Click(object sender, EventArgs e)
		{
			if (CurrCmd == (int)((ToolStripMenuItem)sender).Tag)
				return;

			CurrCmd = -1;

			try
			{
                if (!SlotSelector())
                    return;
				ShowPandaBox();

				CurrCmd = (int)((ToolStripMenuItem)sender).Tag;
				GlobalVars.InitCommand();

				MainForm DMECoverageForm = new MainForm();

				DMECoverageForm.FormClosed += AllForms_FormClosed;
				DMECoverageForm.Show(GlobalVars.Win32Window);
				//routeForm.ComboBox001.Focus();

				HidePandaBox();
				DMECoverageForm = null;
			}
			catch (Exception ex)
			{
				CurrCmd = -1;
				HidePandaBox();
				throw ex;
				//MessageBox.Show(GlobalVars.Win32Window, ex.Message, ((ToolStripMenuItem)sender).Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
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
