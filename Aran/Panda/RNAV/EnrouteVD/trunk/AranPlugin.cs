using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Aran.Aim;
using Aran.AranEnvironment;
using Aran.PANDA.RNAV.Enroute.VD.Properties;
using Aran.Aim.Data;

namespace Aran.PANDA.RNAV.Enroute.VD
{
	public class EnrouteVD : AranPlugin
	{
		const string _name = "PANDA RNAV - Enroute VOR-DME";
		const string guidString = "C3E0E289-9707-4940-AECD-226A8B75DB97";

		private Guid _id = new Guid(guidString);
		private int _currCmd = -1;
		private ToolStripMenuItem _tsmiEnrouteVD;

		public override Guid Id
		{
			get { return _id; }
		}

		public override string Name
		{
			get { return _name; }
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

			_tsmiEnrouteVD = new ToolStripMenuItem();
			_tsmiEnrouteVD.Text = Resources.str00031;
			_tsmiEnrouteVD.Tag = 0;
			_tsmiEnrouteVD.Click += new EventHandler(enroute_Click);

			ToolStripMenuItem menuItem = new ToolStripMenuItem();
			menuItem.Text = Resources.str00030;
			menuItem.DropDownItems.Add(_tsmiEnrouteVD);

			aranEnv.AranUI.AddMenuItem(AranMapMenu.Applications, menuItem);
		}

		private void AllForms_FormClosed(object sender, FormClosedEventArgs e)
		{
			_currCmd = -1;

			_tsmiEnrouteVD.Checked = false;
			_tsmiEnrouteVD.Enabled = true;

			sender = null;
			GC.Collect();
		}

		void enroute_Click(object sender, EventArgs e)
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

				MainForm enrouteVDForm = new MainForm();
				enrouteVDForm.FormClosed += AllForms_FormClosed;
				enrouteVDForm.Show(GlobalVars.Win32Window);

				HidePandaBox();
				enrouteVDForm = null;
			}
			catch (Exception ex)
			{
				_currCmd = -1;
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
    }
}
