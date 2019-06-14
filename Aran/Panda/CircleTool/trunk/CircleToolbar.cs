using System;
using System.Windows.Forms;
using Aran.Geometries;
using Aran.PANDA.Common;

namespace Aran.PANDA.CircleTool
{
	public partial class CircleToolbar : Form
	{
		public CircleToolbar()
		{
			InitializeComponent();
		}

		private void tsBtn15nm_Click(object sender, EventArgs e)
		{
			if (tsBtn15nm.Checked)
			{
				if(!GlobalVars.Initialised)
					GlobalVars.InitCommand();

				GlobalVars.FillADHP();

				LineString ls = new LineString();

				MultiPoint circle = ARANFunctions.CreateCircle(GlobalVars.CurrADHP.pPtPrj, 15 * 1852.2);
				ls.AddMultiPoint(circle);

				GlobalVars.p15NMCircleElem = GlobalVars.gAranGraphics.DrawLineString(ls, 1, ARANFunctions.RGB(0, 255, 255));
			}
			else
				GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.p15NMCircleElem);
		}

		private void tsBtn30nm_Click(object sender, EventArgs e)
		{
			if (tsBtn30nm.Checked)
			{
				if (!GlobalVars.Initialised)
					GlobalVars.InitCommand();

				GlobalVars.FillADHP();

				LineString ls = new LineString();

				MultiPoint circle = ARANFunctions.CreateCircle(GlobalVars.CurrADHP.pPtPrj, 30 * 1852.2);
				ls.AddMultiPoint(circle);

				GlobalVars.p30NMCircleElem = GlobalVars.gAranGraphics.DrawLineString(ls, 1, ARANFunctions.RGB(0, 255, 0));
			}
			else
				GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.p30NMCircleElem);
		}

		public ToolStrip Toolbar
		{
			get { return ToolStrip1; }
		}

	}
}
