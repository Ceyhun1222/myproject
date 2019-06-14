using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aran.PANDA.Common;

namespace Aran.PANDA.RNAV.Arrival
{
	public partial class FIXInfo : Form
	{
		#region Form

		public FIXInfo()
		{
			InitializeComponent();

			label02.Text = GlobalVars.unitConverter.HeightUnit;
			label04.Text = GlobalVars.unitConverter.SpeedUnit;
			label06.Text = GlobalVars.unitConverter.SpeedUnit;
			label10.Text = GlobalVars.unitConverter.DistanceUnit;

		}

		void FillFixInfo(WayPoint fix)
		{
			textBox01.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fix.ConstructAltitude, eRoundMode.NEAREST).ToString();
			//textBox01.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fix.NomLineAltitude, eRoundMode.NEAREST).ToString();

			textBox02.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(fix.IAS, eRoundMode.NEAREST).ToString();

			textBox03.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(fix.ConstructTAS, eRoundMode.NEAREST).ToString();
			//textBox03.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(fix.NomLineTAS, eRoundMode.NEAREST).ToString();

			textBox04.Text = ARANMath.RadToDeg(fix.BankAngle).ToString("0.00");

			textBox05.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(fix.ConstructTurnRadius, eRoundMode.NEAREST).ToString();
			//textBox05.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(fix.NomLineTurnRadius, eRoundMode.NEAREST).ToString();

			//=================================================

		}

		private void LegInfo_Deactivate(object sender, EventArgs e)
		{
			Hide();
		}
		#endregion

		#region Interaction

		static FIXInfo _fixInfoForm;
		public static void ShowFixInfo(int X, int Y, WayPoint LegPoint)
		{
			if (_fixInfoForm == null)
				_fixInfoForm = new FIXInfo();

			_fixInfoForm.FillFixInfo(LegPoint);
			_fixInfoForm .Left = X;
			_fixInfoForm.Top = Y;

			_fixInfoForm.Show();
		}
		#endregion

	}
}
