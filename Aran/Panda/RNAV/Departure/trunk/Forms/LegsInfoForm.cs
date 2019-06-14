using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;

namespace Aran.PANDA.RNAV.Departure
{
	public partial class LegsInfoForm : Form
	{
		#region Form

		double _windSpeed;
		//List<WayPoint> _legPoints;

		public LegsInfoForm()
		{
			InitializeComponent();

			label03.Text = GlobalVars.unitConverter.HeightUnit;
			label05.Text = GlobalVars.unitConverter.SpeedUnit;
			label07.Text = GlobalVars.unitConverter.DistanceUnit;
			label09.Text = GlobalVars.unitConverter.SpeedUnit;

			label12.Text = GlobalVars.unitConverter.HeightUnit;
			label14.Text = GlobalVars.unitConverter.SpeedUnit;
			label16.Text = GlobalVars.unitConverter.DistanceUnit;
			label18.Text = GlobalVars.unitConverter.DistanceUnit;

			_windSpeed = GlobalVars.constants.Pansops[ePANSOPSData.dpWind_Speed].Value;
			textBox04.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(_windSpeed, eRoundMode.NEAREST).ToString();
		}

		void FillFixInfo(WayPoint fix)
		{
			//double RTurn, lPt, EWindSpiral, Rv;

			double RTurn = fix.ConstructTurnRadius;
			textBox01.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fix.ConstructAltitude, eRoundMode.NEAREST).ToString();
			textBox02.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(fix.ConstructTAS, eRoundMode.NEAREST).ToString();
			textBox03.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(RTurn, eRoundMode.NEAREST).ToString();

			double lPt = fix.LPT * (1 - ((fix.FlyMode == eFlyMode.Flyby ? 1 : 0) << 1));
			double Rv = 1765.27777777777777777 * Math.Tan(fix.BankAngle) / (Math.PI * fix.ConstructTAS);
			if (Rv > 3) Rv = 3;
			double EWindSpiral = RTurn + 90.0 * _windSpeed / Rv;

			textBox08.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(EWindSpiral, eRoundMode.NEAREST).ToString();
			//=================================================

			textBox05.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fix.NomLineAltitude, eRoundMode.NEAREST).ToString();
			textBox06.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(fix.NomLineTAS, eRoundMode.NEAREST).ToString();
			textBox07.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(fix.NomLineTurnRadius, eRoundMode.NEAREST).ToString();
		}

		void InitInfoForm(int X, int Y, List<WayPoint> LegPoints)
		{
			Left = X;
			Top = Y;
			//_legPoints = LegPoints;

			DataGrid.RowCount = 0;

			if (LegPoints.Count == 0)
			{
				textBox01.Text = "";
				textBox02.Text = "";
				textBox05.Text = "";
				textBox06.Text = "";
				textBox03.Text = "";
				textBox08.Text = "";

				return;
			}

			WayPoint ReferenceFIX = null;

			for (int i = 0; i < LegPoints.Count; i++)
			{
				WayPoint AddedFIX = LegPoints[i];
				AddedFIX.ISAtC = GlobalVars.CurrADHP.ISAtC;

				DataGridViewRow row = new DataGridViewRow();
				row.Tag = AddedFIX;

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = (DataGrid.Rows.Count + 1).ToString();
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = AddedFIX.Name;
				row.Cells.Add(cell);

				double Val = ARANFunctions.DirToAzimuth(AddedFIX.PrjPt, AddedFIX.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
				cell = new DataGridViewTextBoxCell();
				cell.Value = Math.Round(Val, 2).ToString();
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = Math.Round(ARANMath.RadToDeg(AddedFIX.TurnAngle), 2).ToString();
				row.Cells.Add(cell);

				//cell = new DataGridViewTextBoxCell();
				//if (ReferenceFIX != null)
				//{
				//	//ReferenceFIX.ISAtC = GlobalVars.CurrADHP.ISAtC;//-15

				//	double fDist = ARANMath.Hypot(ReferenceFIX.PrjPt.X - AddedFIX.PrjPt.X, ReferenceFIX.PrjPt.Y - AddedFIX.PrjPt.Y);
				//	Val = GlobalVars.unitConverter.DistanceToDisplayUnits(fDist, eRoundMode.NERAEST);
				//	cell.Value = Val.ToString();
				//}

				//row.Cells.Add(cell);
				//Val = GlobalVars.unitConverter.HeightToDisplayUnits(AddedFIX.ConstructAltitude, eRoundMode.NERAEST);
				//cell = new DataGridViewTextBoxCell();
				//cell.Value = Val.ToString();
				//row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = AddedFIX.FlyMode.ToString();
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = AddedFIX.PBNType.ToString();
				row.Cells.Add(cell);

				DataGrid.Rows.Add(row);
				ReferenceFIX = AddedFIX;
			}

			FillFixInfo(LegPoints[0]);
		}

		private void FIXInfoForm_Deactivate(object sender, EventArgs e)
		{
			Hide();
		}

		private void dataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0)
				return;

			FillFixInfo((WayPoint)(DataGrid.Rows[e.RowIndex].Tag));
			//FillFixInfo(_LegPoints[e.RowIndex]);
		}
		#endregion

		#region Interaction

		static LegsInfoForm _fixInfoForm;
		public static void ShowFixInfo(int X, int Y, List<WayPoint> LegPoints)
		{
			if (_fixInfoForm == null)
				_fixInfoForm = new LegsInfoForm();

			_fixInfoForm.InitInfoForm(X, Y, LegPoints);
			_fixInfoForm.Show();
		}
		#endregion
	}
}
