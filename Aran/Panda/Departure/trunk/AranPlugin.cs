using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Aran.AranEnvironment;
using Aran.PANDA.Departure.Properties;
using Microsoft.Win32;
using Aran.Aim;
using Aran.Aim.Data;

namespace Aran.PANDA.Departure
{
	public class DeparaturePlugin : AranPlugin
	{
		private const string _name = "PANDA Conventional - Departure";
		//private bool _isInitCommands;
		private ToolStripMenuItem _tsmiDepartOmniDirect;
		private ToolStripMenuItem _tsmiDepartRouts;
		private ToolStripMenuItem _tsmiDepartGuidance;

		#region IAranPlugin Members

		public DeparaturePlugin()
		{
			//_isInitCommands = false;
		}

		public override Guid Id { get { return new Guid("c3860b1d-89c8-4fc5-9477-c105d223e775"); } }
		public override string Name { get { return _name; } }

		public override List<FeatureType> GetLayerFeatureTypes()
		{
			var list = new List<FeatureType>();

			list.Add(FeatureType.AirportHeliport);
			list.Add(FeatureType.RunwayCentrelinePoint);
			list.Add(FeatureType.DesignatedPoint);
			list.Add(FeatureType.VOR);
			list.Add(FeatureType.NDB);
			list.Add(FeatureType.DME);
			list.Add(FeatureType.VerticalStructure);

			return list;
		}

		public override void Startup(IAranEnvironment aranEnv)
		{
			GlobalVars.gAranEnv = aranEnv;
			GlobalVars.gHookHelper = new ESRI.ArcGIS.Controls.HookHelper();
			GlobalVars.gHookHelper.Hook = GlobalVars.gAranEnv.HookObject;

			int LangCode = 1033;
			NativeMethods.SetThreadLocale(LangCode);
			Resources.Culture = new System.Globalization.CultureInfo(LangCode);

			ToolStripMenuItem menuItem = new ToolStripMenuItem();
			menuItem.Text = Resources.str00080;

			_tsmiDepartOmniDirect = new ToolStripMenuItem();
			_tsmiDepartOmniDirect.Text = Resources.str15063;
			_tsmiDepartOmniDirect.Tag = 0;
			_tsmiDepartOmniDirect.Click += DepartOmniDirect_Click;
			menuItem.DropDownItems.Add(_tsmiDepartOmniDirect);

			_tsmiDepartRouts = new ToolStripMenuItem();
			_tsmiDepartRouts.Text = Resources.str15270;
			_tsmiDepartOmniDirect.Tag = 1;
			_tsmiDepartRouts.Click += DepartRouts_Click;
			menuItem.DropDownItems.Add(_tsmiDepartRouts);

			_tsmiDepartGuidance = new ToolStripMenuItem();
			_tsmiDepartGuidance.Text = Resources.str15478;
			_tsmiDepartOmniDirect.Tag = 2;
			_tsmiDepartGuidance.Click += DepartGuidance_Click;
			menuItem.DropDownItems.Add(_tsmiDepartGuidance);

			aranEnv.AranUI.AddMenuItem(AranMapMenu.Applications, menuItem);
		}

		#endregion

		private void InitCommands()
		{
			//if (_isInitCommands)
			//   return;

			GlobalVars.InitCommand();

			//_isInitCommands = true;
		}

		#region Event handlers
		private void DepartOmniDirect_Click(object sender, EventArgs e)
		{
			if (GlobalVars.CurrCmd == 1)
				return;
			GlobalVars.CurrCmd = 0;

			try
			{
                if (!SlotSelector())
                    return;

				NativeMethods.ShowPandaBox(GlobalVars.gAranEnv.Win32Window.Handle.ToInt32());

				InitCommands();

				CDepartOmniDirect DepartOmniDirectFrm = new CDepartOmniDirect();

				DepartOmniDirectFrm.FormClosed += Form_FormClosed;

				_tsmiDepartOmniDirect.Checked = true;
				_tsmiDepartRouts.Enabled = false;
				_tsmiDepartGuidance.Enabled = false;

				DepartOmniDirectFrm.Show(GlobalVars.Win32Window);
				DepartOmniDirectFrm.ComboBox001.Focus();
				GlobalVars.CurrCmd = 1;

				NativeMethods.HidePandaBox();
				DepartOmniDirectFrm = null;
			}
			catch (Exception ex)
			{
				NativeMethods.HidePandaBox();

				var tsmi = sender as ToolStripMenuItem;
				MessageBox.Show(ex.Message, tsmi.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

		}

		private void DepartRouts_Click(object sender, EventArgs e)
		{
			if (GlobalVars.CurrCmd == 2)
				return;
			GlobalVars.CurrCmd = 0;

			try
			{
                if (!SlotSelector())
                    return;

                NativeMethods.ShowPandaBox(GlobalVars.gAranEnv.Win32Window.Handle.ToInt32());

				InitCommands();

				CDepartRouts DepartRoutsFrm = new CDepartRouts();

				DepartRoutsFrm.FormClosed += Form_FormClosed;

				_tsmiDepartOmniDirect.Enabled = false;
				_tsmiDepartRouts.Checked = true;
				_tsmiDepartGuidance.Enabled = false;

				DepartRoutsFrm.Show(GlobalVars.Win32Window);
				DepartRoutsFrm.ComboBox001.Focus();
				GlobalVars.CurrCmd = 2;

				NativeMethods.HidePandaBox();
				DepartRoutsFrm = null;
			}
			catch (Exception ex)
			{
				NativeMethods.HidePandaBox();

				var tsmi = sender as ToolStripMenuItem;
				MessageBox.Show(ex.Message, tsmi.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void DepartGuidance_Click(object sender, EventArgs e)
		{
			if (GlobalVars.CurrCmd == 3)
				return;
			GlobalVars.CurrCmd = 0;

			try
			{
                if (!SlotSelector())
                    return;

                NativeMethods.ShowPandaBox(GlobalVars.gAranEnv.Win32Window.Handle.ToInt32());

				InitCommands();

				CDepartGuidanse DepartGuidanseFrm = new CDepartGuidanse();

				DepartGuidanseFrm.FormClosed += Form_FormClosed;

				_tsmiDepartOmniDirect.Enabled = false;
				_tsmiDepartRouts.Checked = true;
				_tsmiDepartGuidance.Enabled = false;

				DepartGuidanseFrm.Show(GlobalVars.Win32Window);
				DepartGuidanseFrm.ComboBox001.Focus();
				GlobalVars.CurrCmd = 3;
				NativeMethods.HidePandaBox();
				DepartGuidanseFrm = null;
			}
			catch (Exception ex)
			{
				NativeMethods.HidePandaBox();

				var tsmi = sender as ToolStripMenuItem;
				MessageBox.Show(ex.Message, tsmi.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		void Form_FormClosed(Object sender, EventArgs e)
		{
			_tsmiDepartOmniDirect.Checked = false;
			_tsmiDepartRouts.Checked = false;
			_tsmiDepartGuidance.Checked = false;

			_tsmiDepartOmniDirect.Enabled = true;
			_tsmiDepartRouts.Enabled = true;
			_tsmiDepartGuidance.Enabled = true;

			GlobalVars.CurrCmd = 0;
			sender = null;
			GC.Collect();
		}
        #endregion

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
