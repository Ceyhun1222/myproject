using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.PANDA.Common;

namespace Aran.PANDA.RNAV.SGBAS
{
	public partial class InfoForm : Form
	{
		int THRFields;
		int TurnFields;

		double[] THRValuses;
		double[] TurnValuses;

		public InfoForm()
		{
			InitializeComponent();

			THRValuses = new double[5];
			TurnValuses = new double[5];
		}

		private void InfoForm_Deactivate(object sender, EventArgs e)
		{
			this.Hide();
		}

		public void SetLocAbeamDist(double LocAbeamDist)
		{
			THRValuses[0] = LocAbeamDist;
			THRFields = THRFields | 1;
		}

		public void SetLocAlongDist(double LocAlongDist)
		{
			THRValuses[1] = LocAlongDist;
			THRFields = THRFields | 2;
		}

		public void SetDeltaAngle(double dAngle)
		{
			THRValuses[2] = dAngle;
			THRFields = THRFields | 4;
		}

		public void SetIntersectDistance(double IntersectDistance)
		{
			THRValuses[3] = IntersectDistance;
			THRFields = THRFields | 8;
		}

		public void SetOCHLimit(double OCHLimit)
		{
			THRValuses[4] = OCHLimit;
			THRFields = THRFields | 16;
		}

		public void ResetTHRFields()
		{
			THRFields = 0;
		}

		public void ShowTHRInfo(int X, int Y)
		{
			string[] FieldNam = new string[] { "THR abeam distance from Localizer course: ", "THR along distance from Localizer: ", "Offset angle: ", "Intersect point distance from THR: ", "OCH lower limit: " };
			string[] FieldUnit = new string[] { " m", " m", " °", " m", " " + GlobalVars.unitConverter.HeightUnit };

			string strTmp, strRes = "";
			//txtInfo.WordWrap = true;

			for (int i = 0, flg = 1; i < 5; i++, flg += flg)
			{
				if ((THRFields & flg) != 0)
				{
					if (i == 4)
						strTmp = FieldNam[i] + GlobalVars.unitConverter.HeightToDisplayUnits(THRValuses[i], eRoundMode.NEAREST).ToString() + FieldUnit[i];
					else
						strTmp = FieldNam[i] + THRValuses[i].ToString("0.00") + FieldUnit[i];

					strRes = strRes + strTmp + "\r\n";
				}
			}

			txtInfo.Text = strRes;
			txtInfo.Select(0, 0);

			lblInfo.Text = strRes;
			lblInfo.AutoSize = false;
			lblInfo.AutoSize = true;

			this.Width = lblInfo.Width + 2;
			this.Height = lblInfo.Height + 2;

			this.Show(GlobalVars.Win32Window);
			this.Left = X;
			this.Top = Y + Height;
			//this.Activate();
		}

		public void SetTAS(double TAS)
		{
			TurnValuses[0] = TAS;
			TurnFields = TurnFields | 1;
		}

		public void SetWindSpeed(double WindSpeed)
		{
			TurnValuses[1] = WindSpeed;
			TurnFields = TurnFields | 2;
		}

		public void SetRadius(double Radius)
		{
			TurnValuses[2] = Radius;
			TurnFields = TurnFields | 4;
		}

		public void SetE(double E)
		{
			TurnValuses[3] = E;
			TurnFields = TurnFields | 8;
		}

		public void SetAltitude(double Altitude)
		{
			TurnValuses[4] = Altitude;
			TurnFields = TurnFields | 16;
		}

		public void ResetTurnFields()
		{
			TurnFields = 0;
		}

		public void ShowTurnInfo(int X, int Y)
		{
			string[] FieldNam = new string[] { "TAS: ", "Wind speed: ", "Radius: ", "E: ", "Altitude: " };
			string[] FieldUnit = new string[] { " " + GlobalVars.unitConverter.SpeedUnit, " " + GlobalVars.unitConverter.SpeedUnit, " " + GlobalVars.unitConverter.DistanceUnit, " " + GlobalVars.unitConverter.DistanceUnit + "/°", " " + GlobalVars.unitConverter.HeightUnit };

			string strTmp, strRes = "";

			for (int i = 0, flg = 1; i < 5; i++, flg += flg)
			{
				if ((TurnFields & flg) != 0)
				{
					if (i < 2)
						strTmp = FieldNam[i] + GlobalVars.unitConverter.SpeedToDisplayUnits(TurnValuses[i], eRoundMode.NEAREST).ToString() + FieldUnit[i];
					else if (i < 4)
						strTmp = FieldNam[i] + GlobalVars.unitConverter.DistanceToDisplayUnits(TurnValuses[i], eRoundMode.NEAREST).ToString() + FieldUnit[i];
					else
						strTmp = FieldNam[i] + GlobalVars.unitConverter.HeightToDisplayUnits(TurnValuses[i], eRoundMode.NEAREST).ToString() + FieldUnit[i];

					strRes = strRes + strTmp + "\r\n";
				}
			}

			txtInfo.Text = strRes;
			txtInfo.Select(0, 0);

			lblInfo.Text = strRes;
			lblInfo.AutoSize = false;
			lblInfo.AutoSize = true;

			this.Width = lblInfo.Width + 2;
			this.Height = lblInfo.Height + 1;

			this.Show(GlobalVars.Win32Window);
			this.Left = X;
			this.Top = Y + Height;
			//this.Activate();
		}

	}
}
