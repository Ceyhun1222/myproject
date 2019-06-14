using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Aran.Aim;
using Aran.AranEnvironment;
using Aran.PANDA.RNAV.EnRoute.Properties;
using Aran.Aim.Data;

namespace Aran.PANDA.RNAV.EnRoute
{
	public class EnRoutePlugin : AranPlugin
	{
		const string PlaginName = "PANDA RNAV - En-Route";
		const string GuidString = "89B13BF0-FEAD-4C86-8AC3-7C1126C72887";

		private Guid _id = new Guid(GuidString);
		private int CurrCmd = -1;
		private ToolStripMenuItem _tsmiRouteConstruct;
		//ToolStripMenuItem _tsmiProtectionConstruct;

		public override Guid Id
		{
			get { return _id; }
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

			_tsmiRouteConstruct = new ToolStripMenuItem();
			_tsmiRouteConstruct.Text = Resources.str00030;
			_tsmiRouteConstruct.Tag = 0;

			_tsmiRouteConstruct.Click += new EventHandler(command1_Click);
			menuItem.DropDownItems.Add(_tsmiRouteConstruct);

			//_tsmiProtectionConstruct = new ToolStripMenuItem();
			//_tsmiProtectionConstruct.Text = Resources.str00031;
			//_tsmiProtectionConstruct.Tag = 1;

			//_tsmiProtectionConstruct.Click += new EventHandler(command2_Click);
			//menuItem.DropDownItems.Add(_tsmiProtectionConstruct);

			aranEnv.AranUI.AddMenuItem(AranMapMenu.Applications, menuItem);
		}

		private void AllForms_FormClosed(object sender, FormClosedEventArgs e)
		{
			CurrCmd = -1;
			Toolbar = null;

			_tsmiRouteConstruct.Checked = false;
			//_tsmiProtectionConstruct.Checked = false;

			_tsmiRouteConstruct.Enabled = true;
			//_tsmiProtectionConstruct.Enabled = true;

			sender = null;
			GC.Collect();
		}

		void command1_Click(object sender, EventArgs e)
		{
			if (CurrCmd == (int)((ToolStripMenuItem)sender).Tag)
				return;

			CurrCmd = -1;

			try
			{
                if (!SlotSelector())
                    return;
				RouteForm routeForm;
				ShowPandaBox();

				CurrCmd = (int)((ToolStripMenuItem)sender).Tag;
				GlobalVars.InitCommand();

				//if (routeForm == null || routeForm.IsDisposed)
				routeForm = new RouteForm();

				routeForm.FormClosed += AllForms_FormClosed;

				routeForm.Show(GlobalVars.Win32Window);
				//routeForm.ComboBox001.Focus();
				HidePandaBox();
				routeForm = null;
			}
			catch (Exception ex)
			{
				CurrCmd = -1;
				HidePandaBox();
				//throw ex;
				MessageBox.Show(GlobalVars.Win32Window, ex.Message, ((ToolStripMenuItem)sender).Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        //void command2_Click(object sender, EventArgs e)
        //{
        //	if (CurrCmd == (int)((ToolStripMenuItem)sender).Tag)
        //		return;

        //	CurrCmd = -1;

        //	try
        //	{
        //		ProtectionForm protectionForm;
        //		ShowPandaBox();

        //		CurrCmd = (int)((ToolStripMenuItem)sender).Tag;
        //		GlobalVars.InitCommand();

        //		//if (protectionForm == null || protectionForm.IsDisposed)
        //			protectionForm = new ProtectionForm();

        //		protectionForm.FormClosed += AllForms_FormClosed;

        //		protectionForm.Show(GlobalVars.Win32Window);
        //		//protectionForm.ComboBox001.Focus();

        //		HidePandaBox();
        //		protectionForm = null;
        //	}
        //	catch (Exception ex)
        //	{
        //		CurrCmd = -1;
        //		HidePandaBox();
        //		//throw ex;
        //		MessageBox.Show(GlobalVars.Win32Window, ex.Message, ((ToolStripMenuItem)sender).Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //	}
        //}
    }
}
