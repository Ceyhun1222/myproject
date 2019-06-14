using System;
using System.ComponentModel;
using System.Windows.Forms;
using Aran.Geometries;
using Aran.PANDA.Common;
using Aran.AranEnvironment.Symbols;
using Aran.Geometries.Operators;
using System.Collections.Generic;
using System.IO;

namespace Aran.PANDA.CRMWall
{
	public partial class MainForm : Form
	{
		const double minSemiWidth = 1852.0;
		const double maxSemiWidth = 3000.0;

		const double minAhead = 15000.0;
		const double maxAhead = 28000.0;

		const double minBehind = 10000.0;
		const double maxBehind = 20000.0;

		const double minStep = 30.0;
		const double nomStep = 100.0;
		const double maxStep = 300.0;

		const double MinGPIntersectHeight = 55.0;
		const double MaxGPIntersectHeight = 200.0;	//150.0
		const double OffsetTreshold = 1.0;
		const double LocOffsetOCHAdd = 20.0;

		private RWYType SelectedRWY;
		private Point ptCenter;
		private Obstacle[] ObstacleList;
		private MultiPolygon pAreaPoly;
		private LineString pZeroLine;

		private ILSType ILS;

		//private Point ptLH;
		private Point ptLHPrj;

		double SemiWidth;
		double Ahead;
		double Behind;

		double RWYDir;
		double ILSDir;
		double partitioningStep;
		int PrevCmbRWY;
		int havePolygons;

		InfoForm infoFrm;
		Slicer Splitter;
		List<int> PointObstacles;

		List<int> WallElems;

		private IGeometryOperators _jtsGeoOperators;

		public MainForm()
		{
			InitializeComponent();

			this.Text = "Vertical structure to CRM obstacle model exporter";

			label003.Text = GlobalVars.unitConverter.DistanceUnit;
			label005.Text = GlobalVars.unitConverter.DistanceUnit;
			label007.Text = GlobalVars.unitConverter.DistanceUnit;
			label008.Text = GlobalVars.unitConverter.HeightUnit;

			ComboBox001.Items.Clear();

			int n = GlobalVars.RWYList.Length;
			if (n <= 0)
			{
				MessageBox.Show("RWY data is missing.", "PANDA", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				Close();
				return;
			}

			_jtsGeoOperators = new JtsGeometryOperators();

			Splitter = new Slicer();
			PointObstacles = new List<int>();

			WallElems = new List<int>();

			PrevCmbRWY = -1;
			SemiWidth = minSemiWidth;
			Ahead = minAhead;
			Behind = minBehind;
			partitioningStep = nomStep;

			//GlobalVars.ButtonControl1State = true;

			textBox001.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(SemiWidth, eRoundMode.CEIL).ToString();
			textBox002.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(Ahead, eRoundMode.CEIL).ToString();
			textBox003.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(Behind, eRoundMode.CEIL).ToString();
			textBox004.Text = GlobalVars.unitConverter.HeightToDisplayUnits(partitioningStep, eRoundMode.CEIL).ToString();

			infoFrm = new InfoForm();

			for (int i = 0; i < n; i++)
				ComboBox001.Items.Add(GlobalVars.RWYList[i]);
			ComboBox001.SelectedIndex = 0;
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			// Get a handle to a copy of this form's system (window) menu
			IntPtr hSysMenu = Functions.GetSystemMenu(this.Handle, false);
			// Add a separator
			Functions.AppendMenu(hSysMenu, GlobalVars.MF_SEPARATOR, 0, string.Empty);
			// Add the About menu item
			Functions.AppendMenu(hSysMenu, GlobalVars.MF_STRING, GlobalVars.SYSMENU_ABOUT_ID, "&About…");
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);

			if ((m.Msg == GlobalVars.WM_SYSCOMMAND) && ((int)m.WParam == GlobalVars.SYSMENU_ABOUT_ID))
			{
				AboutForm about = new AboutForm();
				about.ShowDialog(this);
				about = null;
			}
		}

		private void TextBoxes_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				e.IsInputKey = true;
		}

		void CreateArea()
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.pAreaElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.ZeroLineElem);

			if (ptCenter == null)
				return;

			Ring pAreaRing = new Ring();

			Point ptTmp = ARANFunctions.LocalToPrj(ptCenter, ILSDir, -Ahead, SemiWidth);
			pAreaRing.Add(ptTmp);

			ptTmp = ARANFunctions.LocalToPrj(ptCenter, ILSDir, -Ahead, -SemiWidth);
			pAreaRing.Add(ptTmp);

			ptTmp = ARANFunctions.LocalToPrj(ptCenter, ILSDir, Behind, -SemiWidth);
			pAreaRing.Add(ptTmp);

			ptTmp = ARANFunctions.LocalToPrj(ptCenter, ILSDir, Behind, SemiWidth);
			pAreaRing.Add(ptTmp);

			Polygon pAreaPolygon = new Polygon();
			MultiPolygon pFullCircle = new MultiPolygon();

			pAreaPoly = new MultiPolygon();
			pAreaPolygon.ExteriorRing = pAreaRing;
			pAreaPoly.Add(pAreaPolygon);

			pZeroLine = new LineString();
			pZeroLine.Add(ARANFunctions.LocalToPrj(ptCenter, ILSDir, 0, -SemiWidth));
			pZeroLine.Add(ARANFunctions.LocalToPrj(ptCenter, ILSDir, 0, SemiWidth));

			// =====================================================================

			LineSymbol pLineSym = new LineSymbol();
			pLineSym.Color = ARANFunctions.RGB(255, 0, 0);
			pLineSym.Width = 2;

			FillSymbol pEmptyFillSym = new FillSymbol();
			pEmptyFillSym.Style = eFillStyle.sfsNull;		//.sfsCross;	//
			pEmptyFillSym.Outline = pLineSym;

			GlobalVars.pAreaElem = GlobalVars.gAranGraphics.DrawMultiPolygon(pAreaPoly, pEmptyFillSym, true);		//GlobalVars.ButtonControl1State
			GlobalVars.ZeroLineElem = GlobalVars.gAranGraphics.DrawLineString(pZeroLine, 1, 0, true);				//GlobalVars.ButtonControl1State

			// =====================================================================

			NativeMethods.ShowPandaBox((int)(this.Handle));
			Functions.GetObstaclesByPolygon(out ObstacleList, pAreaPoly);
			ClrScr();
			NativeMethods.HidePandaBox();
			havePolygons = -1;
		}

		private void textBox001_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox001_Validating(textBox001, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox001.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox001_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (double.TryParse(textBox001.Text, out fTmp))
			{
				if (textBox001.Tag != null && textBox001.Tag.ToString() == textBox001.Text)
					return;

				double NewSemiWidth = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);
				SemiWidth = NewSemiWidth;

				if (SemiWidth < minSemiWidth)
					SemiWidth = minSemiWidth;

				if (SemiWidth > maxSemiWidth)
					SemiWidth = maxSemiWidth;

				if (SemiWidth != NewSemiWidth)
					textBox001.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(SemiWidth, eRoundMode.CEIL).ToString();

				textBox001.Tag = textBox001.Text;

				CreateArea();
			}
			else if (double.TryParse((string)textBox001.Tag, out fTmp))
				textBox001.Text = (string)textBox001.Tag;
			else
				textBox001.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(SemiWidth, eRoundMode.CEIL).ToString();
		}

		private void textBox002_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox002_Validating(textBox002, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox002.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox002_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (double.TryParse(textBox002.Text, out fTmp))
			{
				if (textBox002.Tag != null && textBox002.Tag.ToString() == textBox002.Text)
					return;

				double NewAhead = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);
				Ahead = NewAhead;

				if (Ahead < minAhead)
					Ahead = minAhead;

				if (Ahead > maxAhead)
					Ahead = maxAhead;

				if (Ahead != NewAhead)
					textBox002.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(Ahead, eRoundMode.CEIL).ToString();

				textBox002.Tag = textBox002.Text;

				CreateArea();
			}
			else if (double.TryParse((string)textBox002.Tag, out fTmp))
				textBox002.Text = (string)textBox002.Tag;
			else
				textBox002.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(Ahead, eRoundMode.CEIL).ToString();
		}

		private void textBox003_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox003_Validating(textBox003, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox003.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox003_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (double.TryParse(textBox003.Text, out fTmp))
			{
				if (textBox003.Tag != null && textBox003.Tag.ToString() == textBox003.Text)
					return;

				double NewBehind = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);
				Behind = NewBehind;

				if (Behind < minBehind)
					Behind = minBehind;

				if (Behind > maxBehind)
					Behind = maxBehind;

				if (Behind != NewBehind)
					textBox003.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(Behind, eRoundMode.CEIL).ToString();

				textBox003.Tag = textBox003.Text;

				CreateArea();
			}
			else if (double.TryParse((string)textBox003.Tag, out fTmp))
				textBox003.Text = (string)textBox003.Tag;
			else
				textBox003.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(Ahead, eRoundMode.CEIL).ToString();
		}

		private void textBox004_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox004_Validating(textBox004, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox004.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox004_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (double.TryParse(textBox004.Text, out fTmp))
			{
				if (textBox004.Tag != null && textBox004.Tag.ToString() == textBox004.Text)
					return;

				double NewStep = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);
				partitioningStep = NewStep;

				if (partitioningStep < minStep)
					partitioningStep = minStep;

				if (partitioningStep > maxStep)
					partitioningStep = maxStep;

				if (partitioningStep != NewStep)
					textBox004.Text = GlobalVars.unitConverter.HeightToDisplayUnits(partitioningStep, eRoundMode.NEAREST).ToString();

				textBox004.Tag = textBox004.Text;

				//CreateArea();
			}
			else if (double.TryParse((string)textBox004.Tag, out fTmp))
				textBox004.Text = (string)textBox004.Tag;
			else
				textBox004.Text = GlobalVars.unitConverter.HeightToDisplayUnits(partitioningStep, eRoundMode.NEAREST).ToString();
		}

		private void ComboBox001_SelectedIndexChanged(object sender, EventArgs e)
		{
			int RWYIndex = ComboBox001.SelectedIndex;
			if (RWYIndex < 0)
				return;

			SelectedRWY = GlobalVars.RWYList[RWYIndex];

			long Index = DBModule.GetILS(SelectedRWY, ref ILS, GlobalVars.CurrADHP);
			if ((Index & 1) != 1)
			{
				MessageBox.Show("Invalid ILS: Localaizer is not found!");
				ComboBox001.SelectedIndex = PrevCmbRWY;
				return;
			}
			//???????????????????????????????????????????????????????
			double fTmp = ILS.pPtGeo.M - SelectedRWY.pPtGeo[eRWY.ptTHR].M;

			if (fTmp < 0.0) fTmp = fTmp + 360.0;
			if (fTmp > 180.0) fTmp = 360.0 - fTmp;
			infoFrm.SetDeltaAngle(fTmp);
			//???????????????????????????????????????????????????????

			RWYDir = SelectedRWY.pPtPrj[eRWY.ptTHR].M;
			ILSDir = ILS.pPtPrj.M;

			double Dist55 = (MinGPIntersectHeight - ILS.GP_RDH) / System.Math.Tan(ARANMath.DegToRad(ILS.GPAngle));
			double Dist200 = (MaxGPIntersectHeight - ILS.GP_RDH) / System.Math.Tan(ARANMath.DegToRad(ILS.GPAngle));

			bool bFlg = true;

			Point pPtBase = (Point)ARANFunctions.LineLineIntersect(SelectedRWY.pPtPrj[eRWY.ptTHR], RWYDir, ILS.pPtPrj, ILSDir);

			int BaseSight;
			double BaseDist, ResX, ResY;

			if (fTmp < OffsetTreshold)
			{
				ARANFunctions.PrjToLocal(SelectedRWY.pPtPrj[eRWY.ptTHR], ILSDir, ILS.pPtPrj, out ResX, out ResY);
				BaseSight = (int)ARANMath.SideDef(ILS.pPtPrj, ILSDir - 0.5 * Math.PI, SelectedRWY.pPtPrj[eRWY.ptTHR]);//-Math.Sign(ResX);	//
				infoFrm.SetLocAlongDist(-ResX);

				if (ResY / ResX < System.Math.Tan(ARANMath.DegToRad(OffsetTreshold)))
				{
					BaseSight = (int)ARANMath.SideDef(SelectedRWY.pPtPrj[eRWY.ptTHR], RWYDir + 0.5 * Math.PI, ILS.pPtPrj);
					BaseDist = ARANFunctions.Point2LineDistancePrj(SelectedRWY.pPtPrj[eRWY.ptTHR], ILS.pPtPrj, ILSDir + 0.5 * Math.PI) * BaseSight;
					ptLHPrj = ARANFunctions.PointAlongPlane(ILS.pPtPrj, ILSDir , BaseDist);

					//GlobalVars.gAranGraphics.DrawPointWithText(ptLHPrj, 255, "ptLH");
					//Application.DoEvents();

					bFlg = false;
				}
				else if (pPtBase == null)
				{
					MessageBox.Show("Loc. beam does not intercept RWY center line!");
					ComboBox001.SelectedIndex = PrevCmbRWY;
					return;
				}
				infoFrm.SetLocAbeamDist(ResY * (int)ARANMath.SideDef(ILS.pPtPrj, ILSDir + Math.PI, SelectedRWY.pPtPrj[eRWY.ptTHR]));
			}

			if (bFlg)
			{
				if (pPtBase == null)
				{
					MessageBox.Show("Interception angle between localizer beam and RWY center line does not meet alignment criteria!");
					ComboBox001.SelectedIndex = PrevCmbRWY;
					return;
				}

				BaseSight = (int)ARANMath.SideDef(SelectedRWY.pPtPrj[eRWY.ptTHR], RWYDir - 0.5 * Math.PI, pPtBase);
				BaseDist = ARANFunctions.ReturnDistanceInMeters(SelectedRWY.pPtPrj[eRWY.ptTHR], pPtBase) * BaseSight;
				infoFrm.SetIntersectDistance(BaseDist);

				if (BaseDist >= Dist55 && BaseDist <= Dist200)
					ptLHPrj = ARANFunctions.PointAlongPlane(pPtBase, ILSDir, BaseDist);
				else
				{
					MessageBox.Show("Interception distance between localizer beam and RWY center line does not meet alignment criteria!");
					ComboBox001.SelectedIndex = PrevCmbRWY;
					return;
				}

				BaseSight = (int)ARANMath.SideDef(ILS.pPtPrj, ILSDir - 0.5 * Math.PI, SelectedRWY.pPtPrj[eRWY.ptTHR]);
				ARANFunctions.PrjToLocal(SelectedRWY.pPtPrj[eRWY.ptTHR], ILSDir, ILS.pPtPrj, out ResX, out ResY);

				ResX *= BaseSight;

				infoFrm.SetLocAlongDist(ResX);
				infoFrm.SetLocAbeamDist(ResY * (int)ARANMath.SideDef(ILS.pPtPrj, ILSDir + Math.PI, SelectedRWY.pPtPrj[eRWY.ptTHR]));
				//double fRDHOCH = ILS.GP_RDH + BaseDist * System.Math.Tan(ARANMath.DegToRad(ILS.GPAngle)) + LocOffsetOCHAdd;
				//infoFrm.SetOCHLimit(fRDHOCH);
				ILS.Category = 1;
			}

			PrevCmbRWY = RWYIndex;

			//ptLH = GlobalVars.pspatialReferenceOperation.ToGeo<Point>(ptLHPrj);
			//ptLH.M = ILS.pPtGeo.M;

			ptLHPrj.Z = SelectedRWY.pPtPrj[eRWY.ptTHR].Z;
			ptLHPrj.M = ILSDir;		//Azt2Dir(ptLH, ptLH.M)
			ILS.LLZ_THR = ARANFunctions.ReturnDistanceInMeters(ILS.pPtPrj, ptLHPrj);

			ptCenter = ptLHPrj;

			CreateArea();
		}

		private void InfoBtn_Click(object sender, EventArgs e)
		{
			infoFrm.ShowMessage(this.Left + InfoBtn.Left, this.Top + InfoBtn.Top + InfoBtn.Height);
		}

		//================================================================

		private void ClrScr()
		{
			foreach (var e in WallElems)
				GlobalVars.gAranGraphics.DeleteGraphic(e);
			WallElems.Clear();
			ExportBtn.Enabled = false;
		}

		private void CreateBtn_Click(object sender, EventArgs e)
		{
			NativeMethods.ShowPandaBox((int)(this.Handle));
			Splitter.Data.Clear();
			PointObstacles.Clear();
			ClrScr();
			havePolygons = 0;

			for (int i = 0; i < ObstacleList.Length; i++)
			{
				Obstacle obst = ObstacleList[i];
				if (obst.pGeomPrj == null || obst.pGeomPrj.IsEmpty)
					continue;

				if (obst.Height<  ptLHPrj.Z)
					continue;

				if (obst.pGeomPrj.Type == GeometryType.MultiPolygon || obst.pGeomPrj.Type == GeometryType.MultiLineString || obst.HorAccuracy > ARANMath.EpsilonDistance)
				{
					Geometry pPoly;
					if (obst.HorAccuracy > ARANMath.EpsilonDistance)
						pPoly = _jtsGeoOperators.Buffer(obst.pGeomPrj, obst.HorAccuracy) as Aran.Geometries.MultiPolygon;
					else
						pPoly = obst.pGeomPrj;

					//GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pPoly, ARANFunctions.RGB(0,255,255) , eFillStyle.sfsDiagonalCross, true, false);

					if (pPoly != null && !pPoly.IsEmpty)
						Splitter.SliceGeometry(pPoly, ptCenter, ILSDir - 0.5 * Math.PI, obst.Height + Math.Abs(obst.VertAccuracy), partitioningStep, i);
					havePolygons++;
				}
				else
					PointObstacles.Add(i);
			}

			LineString pPolyline = new LineString();

			foreach (SlicerBase.Span spn in Splitter.Data)
			{
				pPolyline.Clear();
				pPolyline.Add(spn.p0);
				pPolyline.Add(spn.p1);

				var ge = GlobalVars.gAranGraphics.DrawLineString(pPolyline, 2, 255);
				WallElems.Add(ge);
			}

			NativeMethods.HidePandaBox();
			ExportBtn.Enabled = true;
		}

		//================================================================
		private void TestBtn_Click(object sender, EventArgs e)
		{
			if (saveFileDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
			    return;

			//SavePolyline(saveFileDlg.FileName);
			SavePolygon(saveFileDlg.FileName);
		}

		private void SavePolyline(string FileName)
		{
			FileStream strm = new FileStream(FileName, FileMode.Create);
			StreamWriter tw = new StreamWriter(strm);
			tw.WriteLine("file testPolylines");

			foreach (Obstacle obst in ObstacleList)
			{
				//RIX_L_27	RIX_L_42
				//if (obst.Name == null || obst.Name != "RIX_L_27") continue;

				if (obst.pGeomPrj.Type == GeometryType.MultiLineString)
				{
					tw.WriteLine("multipolyline");

					for (int j = 0; j < ((MultiLineString)obst.pGeomPrj).Count; j++)
					{
						LineString lineStr = ((MultiLineString)obst.pGeomPrj)[j];
						int n = lineStr.Count;
						if (n < 2)
							continue;
						tw.WriteLine("polyline");

						for (int i = 0; i < n; i++)
							tw.WriteLine(string.Format("Point X:{0}, Y:{1} end point", lineStr[i].X, lineStr[i].Y));

						tw.WriteLine("end polyline");
					}
					tw.WriteLine("end multipolyline");
					tw.WriteLine();
				}
			}

			tw.Close();
			strm.Close();
		}

		private void SavePolygon(string FileName)
		{
			FileStream strm = new FileStream(FileName, FileMode.Create);
			StreamWriter tw = new StreamWriter(strm);
			tw.WriteLine("file testPolygons");

			foreach (Obstacle obst in ObstacleList)
			{
				//Geometry pPrjGeometry = _topoGeoOperators.Buffer(obst.pPrjGeometry, obst.HorAccuracy) as Aran.Geometries.MultiPolygon;
				Geometry pPrjGeometry = obst.pGeomPrj;

				if (pPrjGeometry.Type == GeometryType.MultiPolygon)
				{
					Geometry pPoly;
					if (obst.HorAccuracy > ARANMath.EpsilonDistance)
						pPoly = _jtsGeoOperators.Buffer(obst.pGeomPrj, obst.HorAccuracy) as Aran.Geometries.MultiPolygon;
					else
						pPoly = (MultiPolygon)obst.pGeomPrj;

					GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pPoly, eFillStyle.sfsDiagonalCross, ARANFunctions.RGB(0, 255, 255), true, false);

					tw.WriteLine("multipolygon");
					foreach (Polygon ppoly in pPoly)		//obst.pPrjGeometry
					{
						tw.WriteLine("polygon");

						for (int j = 0; j <= ppoly.InteriorRingList.Count; j++)
						{
							Ring ring = j == 0 ? ppoly.ExteriorRing : ppoly.InteriorRingList[j - 1];

							int n = ring.Count;
							if (n < 3)
								continue;

							tw.WriteLine("ring");

							for (int i = 0; i < n; i++)
								tw.WriteLine(string.Format("Point X:{0}, Y:{1} end point", ring[i].X, ring[i].Y));

							tw.WriteLine("end ring");
						}
						tw.WriteLine("end polygon");
					}
					tw.WriteLine("end multipolygon\n");
				}
			}

			tw.Close();
			strm.Close();
		}

		private void ExportBtn_Click(object sender, EventArgs e)
		{
			if (havePolygons < 0)
			{
				MessageBox.Show("Vertical structures is not devided into parts!", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (saveFileDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
				return;

			NativeMethods.ShowPandaBox((int)(this.Handle));
			FileStream strm = new FileStream(saveFileDlg.FileName, FileMode.Create);
			StreamWriter tw = new StreamWriter(strm);

			Functions.ExportPointObstacles(tw, PointObstacles, ILSDir, ref ObstacleList);
			Functions.ExportPolygonalObstacles(tw, Splitter.Data, ILSDir, ref ObstacleList);

			tw.Close();
			strm.Close();

			NativeMethods.HidePandaBox();
		}

		private void CloseBtn_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.pAreaElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.ZeroLineElem);

			foreach (var el in WallElems)
				GlobalVars.gAranGraphics.DeleteGraphic(el);
			WallElems.Clear();
		}

		//================================================================
	}
}
