using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.AranEnvironment;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.Data;
//using Aran.PANDA.RNAV.SGBAS.Properties;

namespace Aran.PANDA.RNAV.SGBAS
{
	public class DeparturePlugin : AranPlugin
	{
		const string PlaginName = "PANDA RNAV - AdvancedPBN";
		const string GuidString = "5F6AECCF-12EE-4440-A52F-64B099A41046";

		private int _currCmd;
		private Guid _id;
		private ToolStripMenuItem _tsmiSBAS;
		private ToolStripMenuItem _tsmiGBAS;
		private ToolStripMenuItem _tsmiRNP_AR;
		private ToolStripMenuItem _tsmiOASCalc;

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

			list.Add(FeatureType.AirportHeliport);
			list.Add(FeatureType.RunwayCentrelinePoint);
			list.Add(FeatureType.DesignatedPoint);
			list.Add(FeatureType.VOR);
			list.Add(FeatureType.DME);
			list.Add(FeatureType.VerticalStructure);

			return list;
		}

		public static void ShowPandaBox()
		{
			Aran.PANDA.Common.NativeMethods.ShowPandaBox(GlobalVars.gAranEnv.Win32Window.Handle.ToInt32());
		}

		private static void HidePandaBox()
		{
			Aran.PANDA.Common.NativeMethods.HidePandaBox();
		}

		public override void Startup(IAranEnvironment aranEnv)
		{
			GlobalVars.gAranEnv = aranEnv;
			GlobalVars.gAranGraphics = aranEnv.Graphics;

			//GlobalVars.gHookHelper = new ESRI.ArcGIS.Controls.HookHelper();
			//GlobalVars.gHookHelper.Hook = GlobalVars.gAranEnv.HookObject;

			_currCmd = -1;
			_id = new Guid(GuidString);

			_tsmiSBAS = new ToolStripMenuItem();
			_tsmiSBAS.Text = "SBAS";
			_tsmiSBAS.Tag = 0;
			_tsmiSBAS.Click += new EventHandler(cmdSBAS_Click);

			_tsmiGBAS = new ToolStripMenuItem();
			_tsmiGBAS.Text = "GBAS";
			_tsmiGBAS.Tag = 1;
			_tsmiGBAS.Click += new EventHandler(cmdGBAS_Click);

			_tsmiRNP_AR = new ToolStripMenuItem();
			_tsmiRNP_AR.Text = "RNP AR APCH";
			_tsmiRNP_AR.Tag = 2;
			_tsmiRNP_AR.Click += new EventHandler(cmdRNP_AR_Click);

			_tsmiOASCalc = new ToolStripMenuItem();
			_tsmiOASCalc.Text = "OAS Calculator";
			_tsmiOASCalc.Tag = 3;
			_tsmiOASCalc.Click += new EventHandler(cmdOASCalc_Click);

			ToolStripMenuItem menuItem = new ToolStripMenuItem();
			menuItem.Text = "Advanced PBN";
			menuItem.DropDownItems.Add(_tsmiSBAS);
			menuItem.DropDownItems.Add(_tsmiGBAS);
			menuItem.DropDownItems.Add(_tsmiRNP_AR);
			menuItem.DropDownItems.Add(_tsmiOASCalc);

			aranEnv.AranUI.AddMenuItem(AranMapMenu.Applications, menuItem);
		}

		//AltimeterSource
		private void AllForms_FormClosed(object sender, FormClosedEventArgs e)
		{
			_currCmd = -1;
			Toolbar = null;

			_tsmiSBAS.Checked = false;
			_tsmiGBAS.Checked = false;
			_tsmiRNP_AR.Checked = false;
			_tsmiOASCalc.Checked = false;

			_tsmiSBAS.Enabled = true;
			_tsmiGBAS.Enabled = true;
			_tsmiRNP_AR.Enabled = true;
			_tsmiOASCalc.Enabled = true;

			if (GlobalVars.VisibilityBar != null)
			{
				GlobalVars.VisibilityBar.Close();
				GlobalVars.VisibilityBar = null;
			}

			sender = null;
			GC.Collect();
		}

		void cmdSBAS_Click(object sender, EventArgs e)
		{
			if (_currCmd == (int)((ToolStripMenuItem)sender).Tag)
				return;

			_currCmd = -1;

			try
			{
                if (!SlotSelector())
                    return;
				ShowPandaBox();

				_currCmd = (int)((ToolStripMenuItem)sender).Tag;
				GlobalVars.InitCommand();

				if (GlobalVars.VisibilityBar == null || GlobalVars.VisibilityBar.IsDisposed)
					GlobalVars.VisibilityBar = new ToolbarForm();

				Toolbar = GlobalVars.VisibilityBar.RNAVToolStrip;

				SBASForm SBASForm = new SBASForm();

				SBASForm.FormClosed += AllForms_FormClosed;
				SBASForm.Show(GlobalVars.Win32Window);
				SBASForm.ComboBox0001.Focus();


				_tsmiSBAS.Checked = true;
				_tsmiGBAS.Checked = false;
				_tsmiRNP_AR.Checked = false;
				//_tsmiOASCalc.Checked = false;

				_tsmiSBAS.Enabled = false;
				_tsmiGBAS.Enabled = false;
				_tsmiRNP_AR.Enabled = false;
				//_tsmiOASCalc.Enabled = false;

				HidePandaBox();
				SBASForm = null;
			}
			catch(Exception ex)
			{
				HidePandaBox();
				_currCmd = -1;
				MessageBox.Show(GlobalVars.Win32Window, ex.Message, ((ToolStripMenuItem)sender).Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		void cmdGBAS_Click(object sender, EventArgs e)
		{
			if (_currCmd == (int)((ToolStripMenuItem)sender).Tag)
				return;

			_currCmd = -1;

			try
			{
                if (!SlotSelector())
                    return;
                ShowPandaBox();

				_currCmd = (int)((ToolStripMenuItem)sender).Tag;
				GlobalVars.InitCommand();

				if (GlobalVars.VisibilityBar == null || GlobalVars.VisibilityBar.IsDisposed)
					GlobalVars.VisibilityBar = new ToolbarForm();

				Toolbar = GlobalVars.VisibilityBar.RNAVToolStrip;

				GBASForm GBASForm = new GBASForm();
				GBASForm.FormClosed += AllForms_FormClosed;
				GBASForm.Show(GlobalVars.Win32Window);
				GBASForm.ComboBox0001.Focus();

				_tsmiSBAS.Checked = false;
				_tsmiGBAS.Checked = true;
				_tsmiRNP_AR.Checked = false;
				//_tsmiOASCalc.Checked = false;

				_tsmiSBAS.Enabled = false;
				_tsmiGBAS.Enabled = false;
				_tsmiRNP_AR.Enabled = false;
				//_tsmiOASCalc.Enabled = false;

				HidePandaBox();
				GBASForm = null;
			}
			catch (Exception ex)
			{
				HidePandaBox();
				_currCmd = -1;
				MessageBox.Show(GlobalVars.Win32Window, ex.Message, ((ToolStripMenuItem)sender).Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		void cmdRNP_AR_Click(object sender, EventArgs e)
		{
			if (_currCmd == (int)((ToolStripMenuItem)sender).Tag)
				return;

			_currCmd = -1;

			try
			{
                if (!SlotSelector())
                    return;
                ShowPandaBox();

				_currCmd = (int)((ToolStripMenuItem)sender).Tag;
				GlobalVars.InitCommand();

				bool havePrimary = false;

				for (int i = 0; i < GlobalVars.CurrADHP.AltimeterSource.Count; i++)
				{
					if (GlobalVars.CurrADHP.pAirportHeliport.AltimeterSource[i] != null)
					{
						Aim.DataTypes.FeatureRef fr = GlobalVars.CurrADHP.pAirportHeliport.AltimeterSource[i].Feature;
						//Aran.Aim.Features.AltimeterSource als = (Aran.Aim.Features.AltimeterSource)fr.GetFeature();
						Aran.Aim.Features.AltimeterSource als = (Aran.Aim.Features.AltimeterSource)Aran.Queries.ExtensionFeature.GetFeature(fr);

						if (als.IsPrimary != null && als.IsPrimary == true)
						{
							havePrimary = true;
							break;
						}
					}
				}

				havePrimary = true;

				if (!havePrimary)
				{
					HidePandaBox();
					_currCmd = -1;
					MessageBox.Show("RNP AR APCH procedure shall not be promulgated for use with remote altimeter setting sources.",
						((ToolStripMenuItem)sender).Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				if (GlobalVars.VisibilityBar == null || GlobalVars.VisibilityBar.IsDisposed)
					GlobalVars.VisibilityBar = new ToolbarForm();

				Toolbar = GlobalVars.VisibilityBar.RNAVToolStrip;

				RNP_ARForm RNP_ARForm = new RNP_ARForm();
				RNP_ARForm.FormClosed += AllForms_FormClosed;
				RNP_ARForm.Show(GlobalVars.Win32Window);
				RNP_ARForm.ComboBox0001.Focus();
				//GlobalVars.VisibilityBar.Show(GlobalVars.Win32Window);


				_tsmiSBAS.Checked = false;
				_tsmiGBAS.Checked = false;
				_tsmiRNP_AR.Checked = true;
				//_tsmiOASCalc.Checked = false;

				_tsmiSBAS.Enabled = false;
				_tsmiGBAS.Enabled = false;
				_tsmiRNP_AR.Enabled = false;
				//_tsmiOASCalc.Enabled = false;

				HidePandaBox();
				RNP_ARForm = null;
			}
			catch (Exception ex)
			{
				HidePandaBox();
				_currCmd = -1;
				MessageBox.Show(GlobalVars.Win32Window, ex.Message, ((ToolStripMenuItem)sender).Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		void cmdOASCalc_Click(object sender, EventArgs e)
		{
			if (_currCmd == (int)((ToolStripMenuItem)sender).Tag)
				return;

			_currCmd = -1;

			try
			{
                if (!SlotSelector())
                    return;
                _currCmd = (int)((ToolStripMenuItem)sender).Tag;

				OASDlg oasDlg = new OASDlg();

				oasDlg.FormClosed += AllForms_FormClosed;
				oasDlg.ShowDialog(GlobalVars.Win32Window);
				oasDlg = null;
			}
			finally
			{
				_currCmd = -1;
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
