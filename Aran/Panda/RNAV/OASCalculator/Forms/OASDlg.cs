using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using OASCalculator;

namespace OASCalculator
{
	public partial class OASDlg : Form
	{
		double GPAvalue;
		double MAGvalue;
		double LLZTHRDist;
		double rdhValue;

		D3Plane _PlaneW, _PlaneW1, _PlaneX, _PlaneY, _PlaneZ;
		D3Point _PointC, _PointD, _PointE/*, _PointC2*/, _PointD2/*, _PointE2*/;

		public OASDlg()
		{
			InitializeComponent();

			AircraftCatBox.SelectedIndex = 0;
			ApproachCatBox.SelectedIndex = 0;
		}

		private void OASDlg_Load(object sender, EventArgs e)
		{
			GPAvalue = (double)GPAUpDwn.Value;
			MAGvalue = (double)MAGUpDwn.Value;
			LLZTHRDist = double.Parse(kpmBox.Text);
			rdhValue = double.Parse(rdhBox.Text);

			CalcPlanes();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void fillBoxes()
		{
			D3Plane oPlaneW, oPlaneW1, oPlaneX, oPlaneY, oPlaneZ;
			D3Point oPointC, oPointD, oPointE, oPointC2, oPointD2, oPointE2;

			double APVCorr = 0.0;

			if (ApproachCatBox.SelectedIndex == 3)
				APVCorr = 38.0;
			else if (ApproachCatBox.SelectedIndex == 4)
				APVCorr = 8.0;

			oPlaneW = _PlaneW; oPlaneW1 = _PlaneW1; oPlaneX = _PlaneX; oPlaneY = _PlaneY; oPlaneZ = _PlaneZ;

			double dRDH = rdhValue - Common.baseRDHHeight;
			oPlaneW.C += dRDH;
			oPlaneW1.C += dRDH;
			oPlaneX.C += dRDH;
			oPlaneY.C += dRDH;

			oPlaneX.C -= APVCorr;
			oPlaneY.C -= APVCorr;

			//==========================================

			PointC22X.Text = "";
			PointC22Y.Text = "";
			PointC22Z.Text = "";
			PointC22X.Enabled = false;
			PointC22Y.Enabled = false;
			PointC22Z.Enabled = false;

			if (ApproachCatBox.SelectedIndex > 2)
			{
				double WW1InterDist = Common.WW1Inter(oPlaneW, oPlaneW1);
				WPrimA.Enabled = WW1InterDist > 0.0;
				WPrimB.Enabled = WW1InterDist > 0.0;
				WPrimC.Enabled = WW1InterDist > 0.0;

				WPrimA.Text = oPlaneW1.A.ToString("0.000000");
				WPrimB.Text = oPlaneW1.B.ToString("0.000000");
				WPrimC.Text = oPlaneW1.C.ToString("0.000");

				if (WW1InterDist > 0.0)
				{
					oPointC = Common.IntersectPlanes(oPlaneW1, oPlaneX, 0.0);

					double ZC4 = oPlaneW1.A * WW1InterDist + oPlaneW1.C;
					D3Point oPointC4 = Common.IntersectPlanes(oPlaneW1, oPlaneX, ZC4);

					PointC22X.Text = oPointC4.X.ToString("0.00");
					PointC22Y.Text = oPointC4.Y.ToString("0.00");
					PointC22Z.Text = oPointC4.Z.ToString("0.00");

					PointC22X.Enabled = ZC4 > 0.0;
					PointC22Y.Enabled = ZC4 > 0.0;
					PointC22Z.Enabled = ZC4 > 0.0;
				}
				else
					oPointC = Common.IntersectPlanes(oPlaneW, oPlaneX, 0.0);

				D3Line ee1 = Common.IntersectPlanes(oPlaneY, oPlaneZ);
				oPointE2.Y = 1759.4;					//1852;// 
				oPointE2.X = -(oPointE2.Y * ee1.B + ee1.C) / ee1.A;
				oPointE2.Z = oPlaneZ.A * oPointE2.X + oPlaneZ.C;
			}
			else
			{
				WPrimA.Text = "";
				WPrimB.Text = "";
				WPrimC.Text = "";

				WPrimA.Enabled = false;
				WPrimB.Enabled = false;
				WPrimC.Enabled = false;
				oPointC = Common.IntersectPlanes(oPlaneW, oPlaneX, 0.0);
				oPointE2 = Common.IntersectPlanes(oPlaneY, oPlaneZ, Common.catOASTermHeight);
			}

			oPointD = Common.IntersectPlanes(oPlaneX, oPlaneY, 0.0);
			oPointE = Common.IntersectPlanes(oPlaneY, oPlaneZ, 0.0);

			oPointC2 = Common.IntersectPlanes(oPlaneW, oPlaneX, Common.catOASTermHeight);
			oPointD2 = Common.IntersectPlanes(oPlaneX, oPlaneY, Common.catOASTermHeight);

			WA.Text = oPlaneW.A.ToString("0.000000");
			WB.Text = oPlaneW.B.ToString("0.000000");
			WC.Text = oPlaneW.C.ToString("0.000");

			XA.Text = oPlaneX.A.ToString("0.000000");
			XB.Text = oPlaneX.B.ToString("0.000000");
			XC.Text = oPlaneX.C.ToString("0.000");

			YA.Text = oPlaneY.A.ToString("0.000000");
			YB.Text = oPlaneY.B.ToString("0.000000");
			YC.Text = oPlaneY.C.ToString("0.000");

			ZA.Text = oPlaneZ.A.ToString("0.000000");
			ZB.Text = oPlaneZ.B.ToString("0.000000");
			ZC.Text = oPlaneZ.C.ToString("0.000");

			PointCX.Text = oPointC.X.ToString("0.00");
			PointCY.Text = oPointC.Y.ToString("0.00");

			PointDX.Text = oPointD.X.ToString("0.00");
			PointDY.Text = oPointD.Y.ToString("0.00");

			PointEX.Text = oPointE.X.ToString("0.00");
			PointEY.Text = oPointE.Y.ToString("0.00");

			PointC2X.Text = oPointC2.X.ToString("0.00");
			PointC2Y.Text = oPointC2.Y.ToString("0.00");
			PointC2Z.Text = oPointC2.Z.ToString("0.00");

			PointD2X.Text = oPointD2.X.ToString("0.00");
			PointD2Y.Text = oPointD2.Y.ToString("0.00");
			PointD2Z.Text = oPointD2.Z.ToString("0.00");

			PointE2X.Text = oPointE2.X.ToString("0.00");
			PointE2Y.Text = oPointE2.Y.ToString("0.00");
			PointE2Z.Text = oPointE2.Z.ToString("0.00");
		}

		private void CalcPlanes()
		{
			if (ApproachCatBox.SelectedIndex == 1)
			{

			}
			else if (ApproachCatBox.SelectedIndex == 2)
			{
			}
			//else
			{
				_PointC = Common.LagrangeI(GPAvalue, Common.GPNodes, Common.CSNodesI);

				_PlaneW.A = Common.LagrangeI(GPAvalue, Common.GPNodes, Common.WANodes);
				_PlaneW.B = 0.0;
				_PlaneW.C = -_PointC.X * _PlaneW.A;

				_PointD = Common.LagrangeI(GPAvalue, Common.GPNodes, Common.DSNodesI);

				_PlaneX.B = Common.LagrangeII(GPAvalue, LLZTHRDist, Common.XBNodesI, Common.GPNodes, Common.LocTHRDists);
				_PlaneX.A = -_PlaneX.B * (_PointD.Y - _PointC.Y) / (_PointD.X - _PointC.X);
				_PlaneX.C = _PlaneX.B * (_PointD.Y * _PointC.X - _PointC.Y * _PointD.X) / (_PointD.X - _PointC.X);

				//_PointC2.Z = Common.catOASTermHeight;
				//_PointC2.X = (_PointC2.Z - _PlaneW.C) / _PlaneW.A;
				//_PointC2.Y = (_PointC2.Z - _PlaneX.A * _PointC2.X - _PlaneX.C) / _PlaneX.B;

				_PointD2.Z = Common.catOASTermHeight;
				_PointD2.X = (_PointD2.Z - Common.baseRDHHeight) / Math.Tan(GPAvalue * Math.PI * (1.0 / 180.0));
				_PointD2.Y = (_PointD2.Z - _PlaneX.A * _PointD2.X - _PlaneX.C) / _PlaneX.B;

				_PointE = Common.LagrangeII(GPAvalue, MAGvalue, Common.ESNodesI, Common.GPNodes, Common.MisAprGr);
				_PlaneY = Common.PlaneFrom3Pt(_PointD, _PointE, _PointD2);

				_PlaneZ.A = -0.01 * MAGvalue;
				_PlaneZ.B = 0.0;
				_PlaneZ.C = -_PlaneZ.A * _PointE.X;

				//_PointE2 = Common.Calc2Equation(_PlaneY, _PlaneZ, Common.catOASTermHeight);
			}

			if (ApproachCatBox.SelectedIndex > 2)
			{
				double dd = 50.0;
				if (ApproachCatBox.SelectedIndex == 4)
					dd = 20.0;

				_PlaneW1.A = Math.Tan(0.75 * GPAvalue * Math.PI * (1.0 / 180.0));
				_PlaneW1.B = 0.0;
				_PlaneW1.C = _PlaneW1.A * rdhValue / Math.Tan(GPAvalue * Math.PI * (1.0 / 180.0)) - dd;
			}

			double APVCorr = 38.0;
			if (ApproachCatBox.SelectedIndex == 4)
				APVCorr = 8.0;

			_PointE.X = -900.0 - (APVCorr / Math.Tan(GPAvalue * Math.PI * (1.0 / 180.0)));
			_PlaneZ.C = -_PlaneZ.A * _PointE.X;

			fillBoxes();
		}

		private void kpmBox_Validating(object sender, CancelEventArgs e)
		{
			LLZTHRDist = double.Parse(kpmBox.Text);
			CalcPlanes();
		}

		private void rdhBox_Validating(object sender, CancelEventArgs e)
		{
			rdhValue = double.Parse(rdhBox.Text);
			CalcPlanes();
		}

		private void GPAUpDwn_ValueChanged(object sender, EventArgs e)
		{
			GPAvalue = (double)GPAUpDwn.Value;
			CalcPlanes();
		}

		private void MAGUpDwn_ValueChanged(object sender, EventArgs e)
		{
			MAGvalue = (double)MAGUpDwn.Value;
			CalcPlanes();
		}

		private void ApproachCatBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			CalcPlanes();
		}

		private void btnGeoCalc_Click(object sender, EventArgs e)
		{
			CalcDlg clcdlg = new CalcDlg();
			clcdlg.ShowDialog(this);
		}

	}
}
