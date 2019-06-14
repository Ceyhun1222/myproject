using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.PANDA.Common;

namespace Aran.PANDA.CRMWall
{
	public partial class InfoForm : Form
	{
		private int Fields;
		private double[] Valuses;

		private string[] FieldNam;
		private string[] FieldUnit;

		public InfoForm()
		{
			InitializeComponent();
			FieldNam = new string[] { "THR abeam distance from Localizer course: ", "THR along distance from Localizer: ", "Offset angle: ", "Intersect point distance from THR: ", "OCH lower limit: " };
			FieldUnit = new string[] { " m", " m", " °", " m", " " + GlobalVars.unitConverter.HeightUnit };
			Valuses = new double[4];
			Fields = 0;
		}

		public void SetLocAbeamDist(double LocAbeamDist)
		{
			Valuses[0] = LocAbeamDist;
			Fields |= 1;
		}

		public void SetLocAlongDist(double LocAlongDist)
		{
			Valuses[1] = LocAlongDist;
			Fields |= 2;
		}

		public void SetDeltaAngle(double dAngle)
		{
			Valuses[2] = dAngle;
			Fields |= 4;
		}

		public void SetIntersectDistance(double IntersectDistance)
		{
			Valuses[3] = IntersectDistance;
			Fields |= 8;
		}

		public void SetOCHLimit(double OCHLimit)
		{
			Valuses[4] = OCHLimit;
			Fields |= 16;
		}

		public void ResetFields()
		{
			Fields = 0;
		}

		public void ShowMessage(int X, int Y)
		{
			string strRes = "";

			for (int i = 0, f = 1; i < 5; i++, f += f)
			{
				if ((Fields & f) != 0)
				{
					if (i == 4)
						strRes += FieldNam[i] + GlobalVars.unitConverter.HeightToDisplayUnits(Valuses[i], eRoundMode.NEAREST).ToString() + FieldUnit[i] + "\n";
					else
						strRes += FieldNam[i] + System.Math.Round(Valuses[i], 2).ToString() + FieldUnit[i] + "\n";
				}
			}

			lblInfo.Text = strRes;
			lblInfo.AutoSize = false;
			lblInfo.AutoSize = true;

			this.Width = lblInfo.Width + 2;
			this.Height = lblInfo.Height + 1;

			this.Show(GlobalVars.Win32Window);
			this.Left = X;
			this.Top = Y + Height;
			//Me.Activate()
		}

		private void InfoForm_Deactivate(object sender, EventArgs e)
		{
			this.Hide();
		}
	}
}
