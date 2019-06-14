using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Aran.PANDA.RNAV.SGBAS
{
	public partial class ToolbarForm : Form
	{
		private ToolStripButton[] StripButtons;
		//public readonly int OASCat23 = 0;
		public readonly int OASCat1 = 0;
		public readonly int BasicILS = 1;
		public readonly int SBASOAS = 2;
		public readonly int OFZ = 3;

		public ToolbarForm()
		{
			InitializeComponent();

			StripButtons = new ToolStripButton[] { tsBtnOASCat1, tsBtnBasicILS, tsBtnSBASOAS, tsBtnOFZ };
			RefresBar(0);
		}

		public bool GetEnabled(int index)
		{
			//int n = RNAVSBASToolStrip.Items.Count;
			int n = StripButtons.Length;
			if (index < 0 || index >= n) return false;

			//return RNAVSBASToolStrip.Items[index].Enabled;
			return StripButtons[index].Enabled;
		}

		public void SetEnabled(int index, bool val)
		{
			//int n = RNAVSBASToolStrip.Items.Count;
			int n = StripButtons.Length;
			if (index < 0 || index >= n) return;
			//RNAVSBASToolStrip.Items[index].Enabled = val;
			StripButtons[index].Enabled = val;
		}

		public void RefresBar(int enables)
		{
			//int n = RNAVSBASToolStrip.Items.Count;
			int n = StripButtons.Length;

			for (int i = 0, m = 1; i < n; i++, m += m)
				//RNAVSBASToolStrip.Items[i].Enabled = (enables & m) != 0;
				StripButtons[i].Enabled = (enables & m) != 0;
		}

		private void tsBtnOASCat1_Click(object sender, EventArgs e)
		{
			int n = GlobalVars.OASPlanesCat1Element.Length;
			GlobalVars.OASPlanesCat1State = tsBtnOASCat1.Checked;

			try
			{
				for (int i = 0; i < n; i++)
					GlobalVars.gAranGraphics.SetVisible(GlobalVars.OASPlanesCat1Element[i], GlobalVars.OASPlanesCat1State);
				//GlobalVars.gAranGraphics.Refresh();
			}
			catch { }
		}

		private void tsBtnBasicILS_Click(object sender, EventArgs e)
		{
			int n = GlobalVars.ILSPlanesElement.Length;
			GlobalVars.ILSPlanesState = tsBtnBasicILS.Checked;

			try
			{
				for (int i = 0; i < n; i++)
					GlobalVars.gAranGraphics.SetVisible(GlobalVars.ILSPlanesElement[i], GlobalVars.ILSPlanesState);
				//GlobalVars.gAranGraphics.Refresh();
			}
			catch { }
		}

		private void tsBtnSBASOAS_Click(object sender, EventArgs e)
		{
			int n = GlobalVars.SBASOASPlanesElement.Length;
			GlobalVars.SBASOASPlanesState = tsBtnSBASOAS.Checked;

			try
			{
				for (int i = 0; i < n; i++)
					GlobalVars.gAranGraphics.SetVisible(GlobalVars.SBASOASPlanesElement[i], GlobalVars.SBASOASPlanesState);
				//GlobalVars.gAranGraphics.Refresh();
			}
			catch { }
		}

		private void tsBtnOFZ_Click(object sender, EventArgs e)
		{
			GlobalVars.OFZPlanesState = tsBtnOFZ.Checked;
			int n = GlobalVars.OFZPlanesElement.Length;

			try
			{
				for (int i = 0; i < n; i++)
					GlobalVars.gAranGraphics.SetVisible(GlobalVars.OFZPlanesElement[i], GlobalVars.OFZPlanesState);
				//GlobalVars.gAranGraphics.Refresh();
			}
			catch { }
		}
	}
}
