using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Converters;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Geometries.SpatialReferences;
using Aran.Panda.Common;
using Aran.Panda.Constants;

namespace Aran.Panda.EnrouteStar
{
	public struct Parametr
	{
		double minVal;
		double maxVal;
		double nomVal;
		double currVal;
	}

	public partial class MainForm : Form
	{
		const double minFixAltitude = 300.0;
		const double maxFixAltitude = 25000.0 * 0.3048;
		const double EarthCurvVisibility = 4.11;

		int graphic_StartFIX = -1;

		FIX _StartFIX, _ReferenceFIX, _CurrentFIX;

		SensorType _SensorType;

		List<FIX> LegFIXList = new List<FIX>();
		List<Leg> LegList = new List<Leg>();

		double _CourseDirection;
		double _DistanceFromPrev;

		//WPT_FIXType[] WPTList;
		NavaidType[] NavaidList1;
		NavaidType[] NavaidList2;

		Interval[] intervalList1;
		Interval[] intervalList2;

		NavaidType Navaid1;
		NavaidType Navaid2;

		Interval interval1;
		Interval interval2;

		double minAltitude;
		double maxAltitude;
		double minDistance = 0.0;
		double maxDistance = 120000.0;

		//double minVORDistance = 0.0;
		//double maxVORDistance = 120000.0;
		//double minNDBDistance = 0.0;
		//double maxNDBDistance = 120000.0;

		double StPtAltitude;
		double slantDistMin1, slantDistMax1;

		Parametr Radial_RadialIntersectionAngle;
		Parametr ArcRadius_ArcRadiusIntersectionAngle;
		Parametr Radial_ArcRadiusIntersectionAngle;

		Parametr prmMaxAngle;
		Parametr prmCourse;
		Parametr prmDistance;
		Parametr prmIAS;
		Parametr prmAltitude;
		Parametr prmGradient;

		public MainForm()
		{
			InitializeComponent();
		}

		private void SetUnitTexts()
		{
			label10.Text = GlobalVars.unitConverter.HeightUnit;
			label23.Text = GlobalVars.unitConverter.HeightUnit;

			label17.Text = GlobalVars.unitConverter.DistanceUnit;

			label19.Text = GlobalVars.unitConverter.SpeedUnit;
			label20.Text = GlobalVars.unitConverter.SpeedUnit;
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			Functions.SetFormParented(Handle);

			SetUnitTexts();

			minAltitude = minFixAltitude + GlobalVars.CurrADHP.Elev;
			maxAltitude = maxFixAltitude;

			StPtAltitude = minAltitude;

			txtbAltitude_00_01.Text = GlobalVars.unitConverter.HeightToDisplayUnits(StPtAltitude, eRoundMode.SPECIAL_CEIL).ToString();

			DBModule.FillWPT_FIXList(out GlobalVars.WPTList, GlobalVars.CurrADHP, GlobalVars.ModellingRadius);
			DBModule.FillNavaidList(out GlobalVars.NavaidList, out GlobalVars.DMEList, GlobalVars.CurrADHP, GlobalVars.ModellingRadius);

			cmbFromList_00_01.Items.Clear();

			int n = GlobalVars.WPTList.Length;

			for (int i = 0, j = -1; i < n; i++)
			{
				if (GlobalVars.WPTList[i].CallSign != null)
				{
					j++;
					cmbFromList_00_01.Items.Add(new CBWPT_FixItem(GlobalVars.WPTList[i]));
				}
			}

			rbByCoords_00_04_CheckedChanged(rbByCoords_00_04, new EventArgs());
		}

		#region "I Layer"

		#region "ByCoordinates"
		void DisableByCoordinates()
		{

		}

		void EnableByCoordinates()
		{

		}
		#endregion

		#region "FromList"
		void DisableFromList()
		{
			cmbFromList_00_01.Enabled = false;
			label1.Enabled = false;
		}

		void EnableFromList()
		{
			cmbFromList_00_01.Enabled = true;
			label1.Enabled = true;
		}
		#endregion

		#region "ByIntersection"
		void DisableByIntersection()
		{
			groupBox3.Enabled = false;
		}

		void EnableByIntersection()
		{
			groupBox3.Enabled = true;
		}
		#endregion
		#endregion

		#region "II Layer"

		#region "Common"
		private void txtbAltitude_00_01_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				txtbAltitude_00_01_Leave(txtbAltitude_00_01, new EventArgs());
				e.Handled = true;
			}
		}

		private void txtbAltitude_00_01_Leave(object sender, EventArgs e)
		{
			double fTmp;
			if (!double.TryParse(txtbAltitude_00_01.Text, out fTmp))
				return;

			double StPtAltitude1 = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);
			fTmp = StPtAltitude1;

			double fTmp1 = GlobalVars.unitConverter.HeightToDisplayUnits(StPtAltitude1, eRoundMode.SPECIAL_CEIL);
			StPtAltitude = GlobalVars.unitConverter.HeightToInternalUnits(fTmp1);

			if (StPtAltitude < minAltitude)
				StPtAltitude = minAltitude;
			else if (StPtAltitude > maxAltitude)
				StPtAltitude = maxAltitude;

			if (StPtAltitude != fTmp)
				txtbAltitude_00_01.Text = GlobalVars.unitConverter.HeightToDisplayUnits(StPtAltitude, eRoundMode.SPECIAL_CEIL).ToString();

			rbByRadial1_00_07_CheckedChanged(rbByRadial1_00_07, new EventArgs());
		}

		private void rbByCoords_00_04_CheckedChanged(object sender, EventArgs e)
		{
			if (rbByCoords_00_04.Checked)
				ByCoordinates();
			else if (rbFromList_00_05.Checked)
				FromList();
			else if (rbByIntersect_00_06.Checked)
				ByIntersection();
		}
		#endregion

		#region "ByCoordinates"
		void ByCoordinates()
		{
			EnableByCoordinates();
			DisableFromList();
			DisableByIntersection();

			//			GlobalVars.CurrADHP.Elev + minFixAltitude;
			//Ot (Aerodrom elevation + 300 metre) okruglennuyu v bolshuyu storonu,
			//			kratnuyu 50 metram ili 100 feet
			//Do FL 250 = 25000 feet vey a metr ile kratnuyu 50 metram

		}
		#endregion

		#region "FromList"
		void FromList()
		{
			DisableByCoordinates();
			EnableFromList();
			DisableByIntersection();

			if (cmbFromList_00_01.Items.Count > 0)
			{
				if (cmbFromList_00_01.SelectedIndex < 0)
					cmbFromList_00_01.SelectedIndex = 0;
				else
					cmbFromList_00_01_SelectedIndexChanged(cmbFromList_00_01, new EventArgs());
			}
		}
		#endregion

		#region "ByIntersection"
		void ByIntersection()
		{
			DisableByCoordinates();
			DisableFromList();
			EnableByIntersection();
			rbByRadial1_00_07_CheckedChanged(rbByRadial1_00_07, new EventArgs());
		}
		#endregion

		#endregion

		#region "III Layer"

		#region "ByCoordinates"
		#endregion

		#region "FromList"

		#endregion

		//    if (comboBox3.Items.Count > 0)
		//        comboBox3.SelectedIndex = 0;

		#region "ByIntersection"

		private void fillNavaidList1()
		{
			int i, j = -1, n;
			NavaidType Navaid;
			Interval interval = new Interval();

			cmbReference1_00_02.Items.Clear();
			if (rbByRadial1_00_07.Checked)	//By radial
			{
				label3.Text = "Radial:";
				label5.Text = "°";

				n = GlobalVars.NavaidList.Length;
				NavaidList1 = new NavaidType[n];
				intervalList1 = new Interval[n];

				for (i = 0; i < n; i++)
				{
					Navaid = GlobalVars.NavaidList[i];

					if (Navaid.CallSign != null)
					{
						double dHeight = StPtAltitude - Navaid.pPtPrj.Z;
						double ConAngle;
						if (dHeight < 0.0)
							dHeight = 0.0;

						if (Navaid.TypeCode == eNavaidType.CodeNDB)
							ConAngle = Navaids_DataBase.NDB.ConeAngle;
						else
							ConAngle = Navaids_DataBase.VOR.ConeAngle;

						interval.Min = dHeight * Math.Tan(ARANMath.DegToRad(ConAngle));
						interval.Max = 1000.0 * EarthCurvVisibility * Math.Sqrt(dHeight);

						NavaidList1[++j] = Navaid;
						intervalList1[j] = interval;

						cmbReference1_00_02.Items.Add(Navaid.CallSign);
					}
				}
			}
			else
			{
				label3.Text = "Distance:";

				label5.Text = GlobalVars.unitConverter.DistanceUnit;

				n = GlobalVars.DMEList.Length;
				NavaidList1 = new NavaidType[n];
				intervalList1 = new Interval[n];

				for (i = 0; i < n; i++)
				{
					Navaid = GlobalVars.DMEList[i];

					if (Navaid.CallSign != null)
					{
						double dHeight = StPtAltitude - Navaid.pPtPrj.Z;
						if (dHeight < 0.0)
							dHeight = 0.0;

						interval.Min = dHeight / Math.Cos(ARANMath.DegToRad(Navaids_DataBase.DME.SlantAngle));
						interval.Max = 1000.0 * EarthCurvVisibility * Math.Sqrt(dHeight);

						if (interval.Min < interval.Max)
						{
							NavaidList1[++j] = Navaid;
							intervalList1[j] = interval;

							cmbReference1_00_02.Items.Add(Navaid.CallSign);
						}
					}
				}
			}

			System.Array.Resize<NavaidType>(ref NavaidList1, ++j);
			System.Array.Resize<Interval>(ref intervalList1, j);
		}

		private void fillNavaidList2()
		{
			int i, j = -1, n;

			cmbReference2_00_03.Items.Clear();

			if (rbByRadial2_00_09.Checked)
			{
				label7.Text = "Radial:";
				label9.Text = "°";

				n = GlobalVars.NavaidList.Length;
				NavaidList2 = new NavaidType[n];

				if (rbByRadial1_00_07.Checked)
				{
					for (i = 0; i < n; i++)
					{
						if (GlobalVars.NavaidList[i].CallSign != null)
						{
							//intervalList1.

							NavaidList2[++j] = GlobalVars.NavaidList[i];
							cmbReference2_00_03.Items.Add(NavaidList2[j].CallSign);
						}
					}
				}
				else
				{

				}
			}
			else
			{
				label7.Text = "Distance:";
				label9.Text = GlobalVars.unitConverter.DistanceUnit;

				n = GlobalVars.DMEList.Length;
				NavaidList2 = new NavaidType[n];

				if (rbByRadial1_00_07.Checked)
				{
				}
				else
				{
				}
			}
			System.Array.Resize<NavaidType>(ref NavaidList2, j + 1);
		}

		private void rbByRadial1_00_07_CheckedChanged(object sender, EventArgs e)
		{
			fillNavaidList1();

			//minDistance = 

			if (cmbReference1_00_02.Items.Count > 0)
				cmbReference1_00_02.SelectedIndex = 0;
		}

		private void cmbFromList_00_01_SelectedIndexChanged(object sender, EventArgs e)
		{
			_StartFIX = new FIX(eFIXRole.IF_, ((CBWPT_FixItem)cmbFromList_00_01.SelectedItem).WPT_FIX, GlobalVars.gAranEnv);
			if (graphic_StartFIX > -1)
				GlobalVars.gAranGraphics.DeleteGraphic(graphic_StartFIX);

			graphic_StartFIX = GlobalVars.gAranGraphics.DrawPointWithText(_StartFIX.PrjPt, 255, _StartFIX.Name);
		}

		private void cmbReference1_00_02_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void cmbReference2_00_03_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void rbByRadial2_00_09_CheckedChanged(object sender, EventArgs e)
		{
			fillNavaidList2();
			if (cmbReference2_00_03.Items.Count > 0)
				cmbReference2_00_03.SelectedIndex = 0;

			if (rbByRadial1_00_07.Checked && rbByRadial2_00_09.Checked)
			{
				label15.Text = "Min. radial/radial intersection angle:";
			}
			else if (rbByDistance1_00_08.Checked && rbByDistance2_00_10.Checked)
			{
				label15.Text = "Min. arc radius/arc radius intersection angle:";
			}
			else
			{
				label15.Text = "Max. radial/arc radius intersection angle:";
			}
		}

		private void txtbAngleDistance1_00_04_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				txtbAngleDistance1_00_04_Leave(txtbAngleDistance1_00_04, new EventArgs());
				e.Handled = true;
			}
		}

		private void txtbAngleDistance1_00_04_Leave(object sender, EventArgs e)
		{
			double fTmp;

			if (!double.TryParse(txtbAngleDistance1_00_04.Text, out fTmp))
				return;

			double NewValue = fTmp;

			if (rbByRadial1_00_07.Checked)	//By radial
			{

			}
			else
			{
				NewValue = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);
				fTmp = NewValue;

				if (NewValue < minDistance)
					NewValue = minDistance;
				else if (NewValue > maxDistance)
					NewValue = maxDistance;

				if (NewValue != fTmp)
					txtbAngleDistance1_00_04.Text = GlobalVars.unitConverter.HeightToDisplayUnits(NewValue, eRoundMode.NERAEST).ToString();
			}

			fillNavaidList2();

			//Navaid1.
			//fTmp
		}

		#endregion

		private void txtbAngleDistance2_00_05_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				txtbAngleDistance2_00_05_Leave(txtbAngleDistance2_00_05, new EventArgs());
				e.Handled = true;
			}
		}

		private void txtbAngleDistance2_00_05_Leave(object sender, EventArgs e)
		{

		}

		#endregion

		private void txtbMinIntersect_00_02_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				txtbMinIntersect_00_02_Leave(txtbMinIntersect_00_02, new EventArgs());
				e.Handled = true;
			}
		}

		private void txtbMinIntersect_00_02_Leave(object sender, EventArgs e)
		{

		}

		private void txtbMaxIntersect_00_03_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				txtbMaxIntersect_00_03_Leave(txtbMaxIntersect_00_03, new EventArgs());
				e.Handled = true;
			}
		}

		private void txtbMaxIntersect_00_03_Leave(object sender, EventArgs e)
		{

		}

		//===============================
		private void btnNext_Click(object sender, EventArgs e)
		{
			int i = tabPages.SelectedIndex;
			if (i < 1)
			{
				LegFIXList.Clear();

				_ReferenceFIX = (FIX)_StartFIX.Clone();
				_CurrentFIX = (FIX)_StartFIX.Clone();

				LegFIXList.Add(_CurrentFIX);

				cmbDirList_01_01.Items.Clear();
				cmbDirList_01_01.Items.AddRange(Utility.FillDirectionWPTList(_CurrentFIX, 0, ARANMath.C_PI, 500.0, 40000.0).ToArray());

				if (cmbDirList_01_01.Items.Count > 0)
					cmbDirList_01_01.SelectedIndex = 0;
				else
					cmbDistList_01_02.Items.Clear();
			}

			i++;
			tabPages.SelectedIndex = i;
		}

		#region  Segment Direction

		private void rbTrueCourse_01_01_CheckedChanged(object sender, EventArgs e)
		{
			updTrueCourse_01_01.Enabled = rbTrueCourse_01_01.Checked;
			cmbDirList_01_01.Enabled = !rbTrueCourse_01_01.Checked;
		}

		private void updTrueCourse_01_01_ValueChanged(object sender, EventArgs e)
		{
			if (updTrueCourse_01_01.Enabled)
			{
				double newCourse = ARANFunctions.AztToDirection(_CurrentFIX.GeoPt, (double)updTrueCourse_01_01.Value, GlobalVars.pSpRefShp, GlobalVars.pSpRefPrj);
				CourseChanged(newCourse);
			}
		}

		private void cmbDirList_01_01_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cmbDirList_01_01.Enabled)
			{
				double newCourse = ARANFunctions.ReturnAngleInRadians(_CurrentFIX.PrjPt, ((CBWPT_FixItem)cmbDirList_01_01.SelectedItem).WPT_FIX.pPtPrj);

				double Axis = ARANFunctions.DirToAzimuth(_CurrentFIX.PrjPt, _CourseDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefShp);
				updTrueCourse_01_01.Value = (decimal)Axis;

				CourseChanged(newCourse);
			}
		}

		#endregion

		#region  WPT Position

		private void rbByDistance_01_03_CheckedChanged(object sender, EventArgs e)
		{
			txtbDistance_01_01.Enabled = rbByDistance_01_03.Checked;
			cmbDistList_01_02.Enabled = !rbByDistance_01_03.Checked;
			if (cmbDistList_01_02.Enabled)
				cmbDistList_01_02_SelectedIndexChanged(cmbDistList_01_02, new EventArgs());
		}

		private void txtbDistance_01_01_Leave(object sender, EventArgs e)
		{
			if (txtbDistance_01_01.Enabled)
			{
				double fDistance;
				if (double.TryParse(txtbDistance_01_01.Text, out fDistance))
				{
					double newDistance = GlobalVars.unitConverter.DistanceToInternalUnits(fDistance);
					DistanceChanged(newDistance);
				}
			}
		}

		private void cmbDistList_01_02_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cmbDistList_01_02.Enabled)
			{
				_DistanceFromPrev = ARANFunctions.ReturnDistanceInMeters(_CurrentFIX.PrjPt, ((CBWPT_FixItem)cmbDistList_01_02.SelectedItem).WPT_FIX.pPtPrj);
				double newDistance = GlobalVars.unitConverter.DistanceToDisplayUnits(_DistanceFromPrev, eRoundMode.NERAEST);
				txtbDistance_01_01.Text = newDistance.ToString();
				DistanceChanged(newDistance);
			}
		}

		#endregion

		private void CourseChanged(double newCorse)
		{
			_CourseDirection = newCorse;

			cmbDistList_01_02.Items.Clear();

			cmbDistList_01_02.Items.AddRange(Utility.FillPositionWPTList(_CurrentFIX, newCorse, 500.0, 40000.0).ToArray());

			if (cmbDistList_01_02.Items.Count > 0)
				cmbDistList_01_02.SelectedIndex = 0;

			Double 	fAngle2_0, fAngle50_0, MinDist, MaxDist, //	fTurnAngle, //	MinStabNext, //	MinStabCurr,
				fDist, fAngle,	fDistTreshol1, fDistTreshol2;

			Point LocalSigPoint;
			FIX  SigPoint;
			int I;

			LineString BridgeLinePart;
			MultiLineString BridgeLine;

		}

		private void DistanceChanged(double newDistance)
		{
			_DistanceFromPrev = newDistance;
			UpdateFIX();
		}

		private void UpdateFIX()
		{
			Point FIXPoint;
			double fDist, fDistTreshol;

			FIXPoint = ARANFunctions.LocalToPrj(_ReferenceFIX.PrjPt, _CurrentFIX.OutDirection + Math.PI, _DistanceFromPrev, 0.0);
			_CurrentFIX.PrjPt = FIXPoint;

			//if (_SensorType == SensorType.stGNSS )	fDistTreshol = GNSSTriggerDistance;
			//else										fDistTreshol = SBASTriggerDistance + GNSSTriggerDistance;
			fDistTreshol = Functions.PBNTerminalTriggerDistance;

			fDist = ARANMath.Hypot(FIXPoint.X - GlobalVars.CurrADHP.pPtPrj.X, FIXPoint.Y - GlobalVars.CurrADHP.pPtPrj.Y);

			if (fDist <= fDistTreshol )
				_CurrentFIX.Role = eFIXRole.IAF_LE_56_;
			else
				_CurrentFIX.Role = eFIXRole.IAF_GT_56_;

			//if(rgSensorType.ItemIndex == 0 && cbPBNType.ItemIndex == 0 && FCurrFIX.Role == IAF_GT_56_) 
			//{
			//    cbPBNType.ItemIndex := 1;
			//    cbPBNTypeChange(cbPBNType);
			//}
			//else
			//    FCurrFIX.RefreshGraphics;
		}

		private void btnAdd_01_01_Click(object sender, EventArgs e)
		{
			Boolean bExisting = false;
			Interval Range;
			Double fLeft, fRight;
			Double IAS, Gradient;

			Leg leg;
			FIX sigPoint;

			//	PolyLine TmpPolyLine;
			//	prmCourse.Reset;
			//	FDistanceParam.Reset;
			//	prmIAS.Reset;
			//	prmAltitude.Reset;
			//	prmGradient.Reset;

			_CurrentFIX.Name = txtbWPTName_01_05.Text;

			if (rbOnWPT_01_04.Checked)
			{
				sigPoint = new FIX(eFIXRole.IF_, ((CBWPT_FixItem)cmbDistList_01_02.SelectedItem).WPT_FIX, GlobalVars.gAranEnv);

				//_CurrentFIX.CallSign = sigPoint.CallSign;
				//_CurrentFIX.Name = sigPoint.Name;
				bExisting = true;
			}

			/*
				if (!bExisting && !CheckName(_CurrentFIX.Name)) 
				{
					MessageDlg('Invalid name.', mtError, [mbOk], 0);
					exit;
				}
			*/

			/*
				prmMaxAngle.BeginUpdate;
				prmMaxAngle.MaxValue := GPANSOPSConstants.Constant[enrMaxTurnAngle].Value;	//120
				prmMaxAngle.EndUpdate;
			*/

			//	sigPoint := TSignificantPoint(cbDistList.Items.Objects [cbDistList.ItemIndex]);
			//	_CurrentFIX.Name

			/*
				IAS = prmIAS.Value;
				Gradient = prmGradient.Value;

				_CurrentFIX.IAS = IAS;
				_CurrentFIX.Gradient = Gradient;

				_CurrentFIX.MultiCoverage = cbMoreDME.Checked;
				_CurrentFIX.FlyMode = FlyMode(cbFlyMode.ItemIndex);
				_CurrentFIX.SensorType = FSensorType;
				_CurrentFIX.PBNType = TPBNType(cbPBNType.ItemIndex + cbPBNType.Tag);

				_CurrentFIX.EntryDirection = prmCourse.Value;

				prmCourse.BeginUpdate;
				prmDistance.BeginUpdate;
				prmIAS.BeginUpdate;
				prmAltitude.BeginUpdate;
				prmGradient.BeginUpdate;

				Range.Left = Modulus(prmCourse.Value - FlightPhases[FFlightProcedure].TurnRange.Right, 2*PI);
				Range.Right = Modulus(prmCourse.Value + FlightPhases[FFlightProcedure].TurnRange.Right, 2*PI);

				prmCourse.Range = Range;

				prmCourse.OutConvertionPoint = _CurrentFIX.PrjPt.Clone;
				prmCourse.InConvertionPoint = _CurrentFIX.GeoPt.Clone;

				Range.Left = GPANSOPSConstants.Constant[rnvImMinDist].Value;
				Range.Right = 500000.0;

				prmDistance.Range = Range;		//FlightPhases[FFlightProcedure].DistRange;
				prmDistance.Value = 9300.0;	//prmDistance.Range.Right;

				prmIAS.Range = FlightPhases[FFlightProcedure].IASRange;
				prmIAS.Value = prmIAS.MaxValue;

			//	Range.Left := _CurrentFIX.Altitude;
			//	Range.Right := _CurrentFIX.Altitude + prmGradient.Value*prmDistance.Value;;
			//	prmAltitude.Range := Range;
			//	prmAltitude.Value := Range.Left;

				prmAltitude.MinValue = _CurrentFIX.Altitude;
				prmAltitude.MaxValue = prmAltitude.MinValue + prmGradient.Value*prmDistance.Value;
				prmAltitude.Value = prmAltitude.MaxValue;

				fLeft = CConverters[puAltitude].ConvertFunction(Range.Left, cdToOuter, nil);
				fRight = CConverters[puAltitude].ConvertFunction(Range.Right, cdToOuter, nil);

				editAltitude.Hint = 'Range: ' + RoundToStr(fLeft, GAltitudeAccuracy, rtCeil) +
										' .. ' + RoundToStr(fRight, GAltitudeAccuracy, rtFloor);

				prmGradient.Range = FlightPhases[FFlightProcedure].GradientRange;
				prmGradient.Value = prmGradient.MinValue;
				prmGradient.EndUpdate;

				spBtnMinus.Enabled = True;

				InitNewPoint(_CurrentFIX);

				leg = new Leg(_CurrentFIX, FReferenceFIX, FFlightProcedure, GUI);
				leg.Gradient = Gradient;
				leg.IAS = IAS;

				FLegs.Insert(0, leg);//	FLegs.Add(Leg);

				prmMaxAngle.BeginUpdate;
				prmMaxAngle.MaxValue = GPANSOPSConstants.Constant[enrMaxTurnAngle].Value;	//120
				prmMaxAngle.EndUpdate;
			*/
		}
	}
}
