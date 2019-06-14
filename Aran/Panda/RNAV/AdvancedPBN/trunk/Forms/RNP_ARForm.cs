using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
using Aran.Geometries;
using Aran.PANDA.RNAV.AdvancedPBN.Properties;
using Aran.Geometries.Operators;
using Aran.AranEnvironment.Symbols;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;
using Aran.Aim.Objects;
using Aran.Queries;
using Aran.Aim.Enums;

namespace Aran.PANDA.RNAV.SGBAS
{
	public partial class RNP_ARForm : Form
	{
		struct TWC
		{
			public double height;
			public double speed;
		}

		public enum LegType
		{
			Straight,
			FixedRadius,
			FlyBy
		}

		public struct RFLeg
		{
			public Point StartPrj;
			public Point StartGeo;
			public double StartDir;
			public double StartAltitude;

			public Point RollOutPrj;
			public Point RollOutGeo;
			public double RollOutDir;
			public double RollOutAltitude;

			//public Point EndPrj;
			//public Point EndGeo;
			//public double EndDir;
			//public double EndAltitude;

			public TurnDirection TurnDir;
			public Point Center;
			public double Radius;
			public double BankAngle;
			public double TAS;
			public double IAS;

			public double MOC;
			public double RNPvalue;
			public double DescentGR;
			public double DistToNext;

			public MultiLineString Nominal;
			public MultiPolygon Protection;
			public ObstacleContainer ObstaclesList;

			public LegType legType;
			public int FixElem;
			public int NominalElem;
			public int ProtectionElem;
		}

		const double FASRNPvalMin = 185.2;
		const double FASRNPvalMax = 926;

		const double MARNPvalMin = 185.2;
		const double MARNPvalMax = 1852;

		const double coTan15 = 3.7320508075688772935274463415059;
		const double rdhToler = 1.524;

		//new TWC { height =	   0.00, speed =	 28.000},
		TWC[] TWCTable = new TWC[]
		{
			new TWC { height =   152.40, speed =     46.300},
			new TWC { height =   304.80, speed =     70.376},
			new TWC { height =   457.20, speed =     92.600},
			new TWC { height =   609.60, speed =     92.600},
			new TWC { height =   762.00, speed =     92.600},
			new TWC { height =   914.40, speed =     92.600},
			new TWC { height =  1066.80, speed =    101.860},
			new TWC { height =  1219.20, speed =    111.120},
			new TWC { height =  1371.60, speed =    120.380},
			new TWC { height =  1524.00, speed =    129.640},
			new TWC { height =  1676.40, speed =    138.900},
			new TWC { height =  1828.80, speed =    148.160},
			new TWC { height =  1981.20, speed =    157.420},
			new TWC { height =  2133.60, speed =    166.680},
			new TWC { height =  2286.00, speed =    175.940},
			new TWC { height =  2438.40, speed =    185.200},
			new TWC { height =  2590.80, speed =    194.460},
			new TWC { height =  2743.20, speed =    203.720},
			new TWC { height =  2895.60, speed =    212.980},
			new TWC { height =  3048.00, speed =    222.240},
			new TWC { height =  3200.40, speed =    231.500},
			new TWC { height =  3352.80, speed =    240.760}
		};

		#region Variable declarations

		private Label[] Label1;
		//private InfoForm infoForm;
		private RNP_ARReportForm reportForm;

		private bool FormInitialised;
		private bool bUnloadByOk;

		private UomSpeed mUomSpeed;
		private UomDistance mUomHDistance;
		private UomDistanceVertical mUomVDistance;

		private int CurrPage
		{
			get { return MultiPage1.SelectedIndex; }
			set
			{
				if (value < 0)
					value = 0;

				if (value >= MultiPage1.TabPages.Count)
					value = MultiPage1.TabPages.Count - 1;

				int n = Label1.Length;
				for (int i = 0; i < n; i++)
				{
					//Label1[i].Visible = MultiPage1.TabPages[i].Visible;
					Label1[i].ForeColor = System.Drawing.Color.Silver;
					Label1[i].Font = new System.Drawing.Font(Label1[i].Font, System.Drawing.FontStyle.Regular);
				}

				Label1[value].ForeColor = System.Drawing.Color.FromArgb(0xFF8000);
				Label1[value].Font = new System.Drawing.Font(Label1[value].Font, System.Drawing.FontStyle.Bold);

				//this.Text = Resources.str00033 + "  "  + MultiPage1.TabPages[StIndex].Text;

				PrevBtn.Enabled = value > 0;
				NextBtn.Enabled = value < MultiPage1.TabPages.Count - 1;

				if (value == MultiPage1.TabPages.Count - 1)
					OkBtn.Enabled = true;
				else
					OkBtn.Enabled = false;

				//this.HelpContextID = 4000 + 100 * (value + 1);
				MultiPage1.SelectedIndex = value;
			}
		}

		#region Page I Variables

		private ObstacleContainer OFZObstacleList;
		private ObstacleContainer OASObstacleList;

		private MultiPolygon _MASegmentPolygon;
		private MultiLineString _pOasLine;
		private MultiLineString _pZorigin;

		private double _ArCourse;
		private double _RWYDir;
		private double _ArDir;

		private double _CoTanGPA;
		private double _GP_RDH;
		private double _TanGPA;

		private double _AlignP_THRMax;
		private double _AlignP_THRMin;
		private double _FapToThrDist;
		private double _AlignP_THR;

		private double _OCHbyAlignment;
		private double _OCHbyObctacle;
		private double _OASgradient;
		private double _HeightLoss;
		private double _OASorigin;
		private double _GPAngle;
		private double _OCHMin;
		private double _MOCfap;
		private double _MOC75;
		private double _Tmin;

		private double _zOrigin;
		private double _TrD;
		private double _IAS;

		private double _MaxTWC;
		private double _CurrTWC;

		private double _PrelFAPalt;
		private double _FASRNPval;
		private double _PrelDHval;
		private double _MARNPval;

		private Point ptTHRgeo;
		private Point ptTHRprj;
		private Point ptFAPprj;
		private Point ptSOCprj;

		private RWYType _SelectedRWY;

		private int _ZoriginLineElem;
		private int _MASegmentElem;
		private int _OasLineElem;
		private int _Category;
		private int _FAPElem;
		private int _SOCElem;

		#endregion

		#region Page II variables

		private ObstacleContainer FASObstaclesList;

		private int _FASturndir;
		private int _FROPElem;

		private double _MinDistFROPtoNEXT;
		private double _MinFROPAltitude;
		private double _FASminRadius;
		private double _FASmaxRadius;
		private double _FASmaxBank;
		private double _totalLenght;
		private double _hNext;

		private Point _ptNextprj;
		RFLeg _CurrFASLeg;
		List<RFLeg> _FASLegs;

		#endregion

		#region Page III variables

		private ObstacleContainer CurrlegObsatclesList;
		private double _ImASminRadius;
		private double _ImASmaxRadius;

		private int _ImASturndir;
		RFLeg _CurrImASLeg;
		List<RFLeg> _ImASLegs;

		#endregion

		#endregion

		#region MainForm

		public RNP_ARForm()
		{
			InitializeComponent();

			FormInitialised = false;

            // Track type on initial/arrival segments
			this.radioButton0203.Tag = Aran.PANDA.RNAV.SGBAS.RNP_ARForm.LegType.Straight;
			this.radioButton0201.Tag = Aran.PANDA.RNAV.SGBAS.RNP_ARForm.LegType.FixedRadius;
			this.radioButton0202.Tag = Aran.PANDA.RNAV.SGBAS.RNP_ARForm.LegType.FlyBy;

			Label1 = new Label[] { Label01, Label02, Label03 };
			//infoForm = new InfoForm();

			reportForm = new RNP_ARReportForm();
			reportForm.Init(ReportButton);

			//LegArray = new RFLeg[100];
			_FASLegs = new List<RFLeg>();

			//excludeObstFrm = new ExcludeObstForm();

			UomDistance[] uomDistHorzTab = new UomDistance[] { UomDistance.KM, UomDistance.NM, UomDistance.M, UomDistance.FT, UomDistance.MI };
			UomDistanceVertical[] uomDistVerTab = new UomDistanceVertical[] { UomDistanceVertical.M, UomDistanceVertical.FT }; //, UomDistanceVertical.OTHER, UomDistanceVertical.OTHER, UomDistanceVertical.OTHER
			UomSpeed[] uomSpeedTab = new UomSpeed[] { UomSpeed.KM_H, UomSpeed.KT, UomSpeed.M_SEC, UomSpeed.MACH, UomSpeed.M_MIN, UomSpeed.FT_MIN, UomSpeed.FT_SEC };

			mUomHDistance = uomDistHorzTab[GlobalVars.unitConverter.DistanceUnitIndex];
			mUomVDistance = uomDistVerTab[GlobalVars.unitConverter.HeightUnitIndex];
			mUomSpeed = uomSpeedTab[GlobalVars.unitConverter.SpeedUnitIndex];

			bUnloadByOk = false;

			GlobalVars.OFZPlanesState = true;

			int i;
			for (i = 0; i < Label1.Length; i++)
				Label1[i].Text = MultiPage1.TabPages[i].Text;

			//=====================================================================

			Label0001_03.Text = GlobalVars.unitConverter.HeightUnit;
			Label0001_07.Text = GlobalVars.unitConverter.HeightUnit;
			Label0001_13.Text = GlobalVars.unitConverter.HeightUnit;
			Label0001_17.Text = GlobalVars.unitConverter.HeightUnitM;
			Label0001_21.Text = GlobalVars.unitConverter.HeightUnit;
			Label0001_23.Text = GlobalVars.unitConverter.DistanceUnit;
			Label0001_25.Text = GlobalVars.unitConverter.DistanceUnit;
			Label0001_27.Text = GlobalVars.unitConverter.HeightUnit;
			Label0001_29.Text = GlobalVars.unitConverter.HeightUnit;
			Label0001_35.Text = GlobalVars.unitConverter.SpeedUnit;
			Label0001_37.Text = GlobalVars.unitConverter.SpeedUnit;
			//=====================================================================
			Label0102.Text = GlobalVars.unitConverter.DistanceUnit;
			Label0104.Text = GlobalVars.unitConverter.HeightUnit;
			Label0108.Text = GlobalVars.unitConverter.DistanceUnit;
			Label0115.Text = GlobalVars.unitConverter.DistanceUnit;
			Label0117.Text = GlobalVars.unitConverter.HeightUnit;
			Label0121.Text = GlobalVars.unitConverter.DistanceUnit;
			Label0125.Text = GlobalVars.unitConverter.DistanceUnit;
			Label0127.Text = GlobalVars.unitConverter.DistanceUnit;
			Label0129.Text = GlobalVars.unitConverter.DistanceUnit;
			//=====================================================================
			label0202.Text = GlobalVars.unitConverter.HeightUnit;
			label0204.Text = GlobalVars.unitConverter.DistanceUnitNM;
			label0206.Text = GlobalVars.unitConverter.HeightUnit;
			label0211.Text = GlobalVars.unitConverter.DistanceUnit;
			label0215.Text = GlobalVars.unitConverter.DistanceUnit;
			label0217.Text = GlobalVars.unitConverter.SpeedUnit;
			//=====================================================================

			//=====================================================================

			_AlignP_THRMin = 450.0; // RNPAR 4.5.5
            _AlignP_THRMax = 1200.0; // RNPAR ?.?.? 
            _OCHbyObctacle = 75.0; // RNPAR 2.2.3
			_OCHbyAlignment = 0.0; // RNPAR ?.?.?

            _GP_RDH = GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value; // RNPAR 4.5.22 - ILS VGSI VPA implement 
            TextBox0003.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_GP_RDH, eRoundMode.NEAREST).ToString();

			_AlignP_THR = _AlignP_THRMin;
			TextBox0005.Text = System.Math.Round(_AlignP_THR, 2).ToString("0.0");

			_FASRNPval = 555.6; // RNPAR 4.1.8
            TextBox0008.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_FASRNPval, eRoundMode.NEAREST).ToString();

			_MARNPval = 1852.0; // RNPAR 4.1.8
            TextBox0009.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_MARNPval, eRoundMode.NEAREST).ToString();

			_PrelDHval = _OCHbyObctacle;
			TextBox0010.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_PrelDHval, eRoundMode.NEAREST).ToString();

            //  ?.?.? need to be calculated
            TextBox0011.Text = GlobalVars.unitConverter.HeightToDisplayUnits(600, eRoundMode.NEAREST).ToString("0");
			_Tmin = GlobalVars.CurrADHP.ISAtC ;

			TextBox0012.Text = _Tmin.ToString();
			//CreateLog("NAME")
			//=====================================================================
			int n = GlobalVars.RWYList.Length;

			ComboBox0001.Items.Clear();

			for (i = 0; i < n; i++)
				ComboBox0001.Items.Add(GlobalVars.RWYList[i]);

			if (n <= 0)
			{
				System.Windows.Forms.MessageBox.Show("There is no RWY for the specified ADHP.", "PANDA",
					System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				Close();
				return;
			}

			//NextBtn.Enabled = true;
			//DBModule.GetObstaclesByDist(out GlobalVars.ObstacleList, GlobalVars.CurrADHP.pPtPrj, GlobalVars.ModellingRadius);

			nUpDown0001_ValueChanged(nUpDown0001, null);

			ComboBox0001.SelectedIndex = 0;

			//double maxRange = GlobalVars.ModellingRadius + Functions.CalcMaxRadius();
			FormInitialised = true;

			ComboBox0002.SelectedIndex = 0;
			ComboBox0003.SelectedIndex = 1;

			TextBox0011_Validating(TextBox0011, null);

			//=================================================================================
			//ComboBox0301.Items.Clear();

			//for (i = 0; i < GlobalVars.EnrouteMOCValues.Length; i++)
			//{
			//    ComboBox0301.Items.Add((GlobalVars.unitConverter.HeightToDisplayUnits(GlobalVars.EnrouteMOCValues[i], eRoundMode.SPECIAL_CEIL)));
			//    comboBox0508.Items.Add((GlobalVars.unitConverter.HeightToDisplayUnits(GlobalVars.EnrouteMOCValues[i], eRoundMode.SPECIAL_CEIL)));
			//}

			//ComboBox0301.SelectedIndex = 0;
			//ComboBox0401.SelectedIndex = 0;
			//ComboBox0403.SelectedIndex = 1;
			//comboBox0508.SelectedIndex = 0;

			//==============================================
			ShowPanelBtn.Checked = false;

			MultiPage1.Top = -21;
			MainControlsFrame.Top = MainControlsFrame.Top - 21;
			Height = Height - 21;

			CurrPage = 0;
		}

		private void RNP_ARForm_KeyUp(object sender, KeyEventArgs e)
		{
			//if (e.KeyCode == Keys.F1)
			//{
			//	NativeMethods.HtmlHelp(0, GlobalVars.HelpFile, GlobalVars.HH_HELP_CONTEXT, HelpContextID);
			//	e.Handled = true;
			//}
		}

		private void RNP_ARForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			DBModule.CloseDB();
			ClearGraphics();
			reportForm.Close();
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
		#endregion

		#region Common Controls Events
		private void TextBoxes_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				e.IsInputKey = true;
		}

		private void ShowPanelBtn_CheckedChanged(object sender, EventArgs e)
		{
			if (!FormInitialised)
				return;

			if (ShowPanelBtn.Checked)
			{
				this.Width = TabInfoFrame.Left + TabInfoFrame.Width + 16;
				ShowPanelBtn.Image = Resources.bmpHIDE_INFO;
			}
			else
			{
				this.Width = TabInfoFrame.Left + 16;
				ShowPanelBtn.Image = Resources.bmpSHOW_INFO;
			}

			if (NextBtn.Enabled)
				NextBtn.Focus();
			else
				PrevBtn.Focus();
		}

		private void PrevBtn_Click(object sender, EventArgs e)
		{
			Aran.PANDA.Common.NativeMethods.ShowPandaBox(this.Handle.ToInt32());

			this.CurrPage = MultiPage1.SelectedIndex - 1;

			switch (MultiPage1.SelectedIndex)
			{
				case 0:
					leavePageII();
					break;
				case 1:
					//leavePageIII();
					break;
				case 2:
					break;
				case 3:
					//leavePageV();

					break;
				case 4:
					break;
			}

			NativeMethods.HidePandaBox();
		}

		private void NextBtn_Click(object sender, EventArgs e)
		{
			NativeMethods.ShowPandaBox(this.Handle.ToInt32());

			int nextCase = MultiPage1.SelectedIndex;

			switch (this.CurrPage)
			{
				case 0:
					if (!preparePageII())
					{
						NativeMethods.HidePandaBox();
						return;
					}
					break;

				case 1:
					preparePageIII();
					break;

				case 2:
					//preparePageIV();
					break;

				case 3:
					//preparePageV();
					break;
			}

			this.CurrPage = MultiPage1.SelectedIndex + 1;
			NextBtn.Enabled = nextCase != 0 && nextCase != 1;

			NativeMethods.HidePandaBox();
		}

		ReportPoint[] RoutsPoints;

		private void ConvertTracToPoints()
		{
			int n = _ImASLegs.Count;
			int m = _FASLegs.Count;

			RoutsPoints = new ReportPoint[n + m];

			//RoutsPoints[0].Description = Resources.str512;
			//RoutsPoints[0].Lat = DER.pPtGeo[eRWY.PtEnd].Y;
			//RoutsPoints[0].Lon = DER.pPtGeo[eRWY.PtEnd].X;
			//RoutsPoints[0].Direction = NativeMethods.Modulus(Functions.Dir2Azt(DER.pPtPrj[eRWY.PtEnd], RWYDir));
			//RoutsPoints[0].Height = PANS_OPS_DataBase.dpH_abv_DER.Value + DER.pPtPrj[eRWY.PtEnd].Z;
			//RoutsPoints[0].Radius = -1;

			Point pPtCurr;//= ((ESRI.ArcGIS.Geometry.IPoint)(DER.pPtPrj[eRWY.PtEnd]));
						  //pPtCurr.M = RWYDir;

			//double fE = GlobalVars.DegToRadValue * 0.5;

			//IPoint pPtCross = new ESRI.ArcGIS.Geometry.Point();
			//IConstructPoint ptConstr = (ESRI.ArcGIS.Geometry.IConstructPoint)pPtCross;
			int j = 0;
			//double fZLen = 0.0, OverallLength = 0.0;
			for (int i = n - 1; i >= 0; i--)
			{
				Point pPtGeo;//, pPtNext = null;
				pPtCurr = _ImASLegs[i].StartPrj;
				pPtGeo = GlobalVars.pspatialReferenceOperation.ToGeo<Point>(pPtCurr);
				RoutsPoints[j].Lat = pPtGeo.Y;
				RoutsPoints[j].Lon = pPtGeo.X;
				//pPtNext = _ImASLegs[i+1].StartPrj;

				RoutsPoints[i].Radius = -1;
				if (_ImASLegs[i].legType != LegType.Straight)
				{
					RoutsPoints[j].Radius = _ImASLegs[i].Radius;
					RoutsPoints[j].Turn = (int)_ImASLegs[i].TurnDir;

					RoutsPoints[j].turnAngle = NativeMethods.Modulus(ARANMath.RadToDeg((_ImASLegs[i].StartDir - _ImASLegs[i].RollOutDir) * (int)_ImASLegs[i].TurnDir));
					RoutsPoints[j].TurnArcLen = RoutsPoints[j].turnAngle * ARANMath.DegToRadValue * RoutsPoints[j].Radius;

					pPtGeo = GlobalVars.pspatialReferenceOperation.ToGeo<Point>(_ImASLegs[i].Center);
					RoutsPoints[j].CenterLat = pPtGeo.Y;
					RoutsPoints[j].CenterLon = pPtGeo.X;

					RoutsPoints[j].Direction = -1;
					// =========================================================================
					//RoutsPoints[i].Description = Resources.str513 + GlobalVars.RomanFigures[(i + 1) / 2 - 1] + Resources.str515;
				}
				else
				{
					RoutsPoints[j].Radius = -1;

					//pPtCurr = TracPoly.get_Point(TracPoly.PointCount - 1);
					//pPtCurr.M = pPtPrev.M;
					//RoutsPoints[i].Description = Resources.str516;
					////RoutsPoints[i].Direction = RoutsPoints[i - 1].Direction
					//RoutsPoints[i].Direction = -1;
				}

				RoutsPoints[j].DistToNext = 0.0;
				j++;
			}

			for (int i = m - 1; i >= 0; i--)
			{
				Point pPtGeo;//, pPtNext;
				pPtCurr = _FASLegs[i].StartPrj;
				pPtGeo = GlobalVars.pspatialReferenceOperation.ToGeo<Point>(pPtCurr);
				RoutsPoints[j].Lat = pPtGeo.Y;
				RoutsPoints[j].Lon = pPtGeo.X;
				//pPtNext = _ImASLegs[i+1].StartPrj;

				RoutsPoints[i].Radius = -1;
				if (_FASLegs[i].legType != LegType.Straight)
				{


					RoutsPoints[j].Radius = _FASLegs[i].Radius;
					RoutsPoints[j].Turn = (int)_FASLegs[i].TurnDir;

					RoutsPoints[j].turnAngle = NativeMethods.Modulus(ARANMath.RadToDeg((_FASLegs[i].StartDir - _FASLegs[i].RollOutDir) * (int)_FASLegs[i].TurnDir));
					RoutsPoints[j].TurnArcLen = RoutsPoints[j].turnAngle * ARANMath.DegToRadValue * RoutsPoints[j].Radius;

					pPtGeo = GlobalVars.pspatialReferenceOperation.ToGeo<Point>(_FASLegs[i].Center);
					RoutsPoints[j].CenterLat = pPtGeo.Y;
					RoutsPoints[j].CenterLon = pPtGeo.X;

					RoutsPoints[j].Direction = -1;
					// =========================================================================
					//RoutsPoints[i].Description = Resources.str513 + GlobalVars.RomanFigures[(i + 1) / 2 - 1] + Resources.str515;
				}
				else
				{
					RoutsPoints[j].Radius = -1;
					//pPtCurr = TracPoly.get_Point(TracPoly.PointCount - 1);
					//pPtCurr.M = pPtPrev.M;
					//RoutsPoints[i].Description = Resources.str516;
					////RoutsPoints[i].Direction = RoutsPoints[i - 1].Direction
					//RoutsPoints[i].Direction = -1;
				}

				RoutsPoints[j].DistToNext = 0.0;
				j++;
			}

			//ZRSegment = OverallLength - fZLen;
			//RoutsAllLen = OverallLength;
		}

		private void OkBtn_Click(object sender, EventArgs e)
		{
			string RepFileName, RepFileTitle;

			if (Functions.ShowSaveDialog(out RepFileName, out RepFileTitle))
			{
				ConvertTracToPoints();
				ReportHeader pReport;

				string sProcName = "RNP AR APCH " + _SelectedRWY.Name;
				pReport.Procedure = sProcName;

				//pReport.EffectiveDate = _Procedure.TimeSlice.ValidTime.BeginPosition;
				pReport.Database = GlobalVars.gAranEnv.ConnectionInfo.Database;
				pReport.Aerodrome = GlobalVars.CurrADHP.Name;
				//pReport.RWY = ComboBox001.Text;

				pReport.Category = ComboBox0002.Text;

				//CReportFile.SaveLog(RepFileName, RepFileTitle, pReport);
				//CReportFile.SaveProtocol(RepFileName, RepFileTitle, pReport);
				// SaveGeometry(RepFileName, RepFileTitle, pReport);

				CReportFile.SaveGeometry(RepFileName, RepFileTitle, pReport, _SelectedRWY, RoutsPoints, 0.0, false);

				if (SaveProcedure())
				{
					//saveReportToDB()
					//saveScreenshotToDB()
					//bUnloadByOk = true

					Close();
				}
			}
		}

		private void CancelBtn_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void ReportButton_CheckedChanged(object sender, EventArgs e)
		{
			if (!FormInitialised) return;

			if (ReportButton.Checked)
				reportForm.Show(GlobalVars.Win32Window);
			else
				reportForm.Hide();
		}

		private void ProfileBtn_CheckedChanged(object sender, EventArgs e)
		{
			if (!FormInitialised) return;

		}

		#endregion

		#region Utilities

		private void ClearGraphics()
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_FAPElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_MASegmentElem);

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_OasLineElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_ZoriginLineElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_SOCElem);

			/*
			GlobalVars.gAranGraphics.SafeDeleteGraphic(IntermSecondaryAreaElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(IntermPrimaryAreaElem);

			GlobalVars.gAranGraphics.SafeDeleteGraphic(ZContinuedElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(SOCElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(pMAPtElem);
			if (mahfFix != null)
				mahfFix.DeleteGraphics();
			*/

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_FROPElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_CurrFASLeg.NominalElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_CurrFASLeg.ProtectionElem);

			int n = _FASLegs.Count;

			for (int i = 0; i < n; i++)
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_FASLegs[i].NominalElem);
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_FASLegs[i].ProtectionElem);
			}

			n = GlobalVars.OFZPlanesElement.Length;
			for (int i = 0; i < n; i++)
				GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.OFZPlanesElement[i]);
		}

		private double FAPDist2hFAP(double Dist, double hStart)
		{
			return Math.Exp(Dist * _TanGPA / GlobalVars.MeanEarthRadius) * (GlobalVars.MeanEarthRadius + hStart) - GlobalVars.MeanEarthRadius;
		}

		private double hFAP2FAPDist(double Hrel)
		{
			return GlobalVars.MeanEarthRadius * Math.Log((GlobalVars.MeanEarthRadius + Hrel + ptTHRgeo.Z) / (GlobalVars.MeanEarthRadius + _GP_RDH + ptTHRgeo.Z)) * _CoTanGPA;
		}

		private double FASMOCatAltitude(double elev, bool isStraight)
		{
			double dISA = _Tmin - (15.0 - 0.0065 * ptTHRprj.Z);

			double bg = isStraight ? 8.0 : 12.360679774997896964091736687313;   //40.0 * sin(18.0);
			double fte = 23.0;
			double atis = 6.0;

			double anpe = 1.225 * _FASRNPval * _TanGPA;
			double wpr = 18.0 * _TanGPA;
			double ase = -2.887e-7 * elev * elev + 6.5e-3 * elev + 15;
			double tan001 = Math.Tan(0.01 * ARANMath.DegToRadValue);
			double vae = (elev - ptTHRprj.Z) * _CoTanGPA * (_TanGPA - (_TanGPA - tan001) / (1.0 + _TanGPA * tan001));
			double isad = (elev - ptTHRprj.Z) * dISA / (288 + dISA - 0.5 * 0.0065 * elev);

			double result = bg - isad + 4.0 / 3.0 * Math.Sqrt(anpe * anpe + wpr * wpr + fte * fte + ase * ase + vae * vae + atis * atis);
			return result;
		}

		private void WPTInSector(Point pRefPt, WPT_FIXType[] InList, out NavaidType[] OutList)
		{
			int n = InList.Length;
			OutList = new NavaidType[n];

			if (n == 0)
				return;

			//DrawPolygon(pSector, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSHorizontal)
			//Application.DoEvents()

			int j = 0;

			for (int i = 0; i < n; i++)
			{
				double X, Y, tmpDist, d0;
				ARANFunctions.PrjToLocal(pRefPt, pRefPt.M, InList[i].pPtPrj, out X, out Y);
				tmpDist = Math.Abs(Y);

				if (InList[i].TypeCode == eNavaidType.NDB)
					d0 = GlobalVars.constants.NavaidConstants.NDB.OnNAVRadius;
				else if (InList[i].TypeCode == eNavaidType.VOR || InList[i].TypeCode == eNavaidType.TACAN)
					d0 = GlobalVars.constants.NavaidConstants.VOR.OnNAVRadius;
				else
					d0 = 20.0;

				if (tmpDist > d0)
					continue;

				//If X > 0 Then Continue For

				if (InList[i].TypeCode == eNavaidType.VOR || InList[i].TypeCode == eNavaidType.NDB || InList[i].TypeCode == eNavaidType.TACAN)
					NavaidsDatabase.FindNavaid(InList[i].Name, InList[i].TypeCode, out OutList[j]);
				else
					OutList[j] = InList[i].ToNavaid();

				//OutList[j].Distance = -X;
				j++;
			}

			if (j < 0)
				OutList = new NavaidType[0];
			else
				Array.Resize<NavaidType>(ref OutList, j);
		}

		#endregion

		#region Page I

		private void CreateOFZPlanes()
		{
			Functions.CreateOFZPlanes(ptTHRprj, _RWYDir, 45.0, ref GlobalVars.OFZPlanes);

			int n = GlobalVars.OFZPlanes.Length;

			for (int i = 0; i < n; i++)
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.OFZPlanesElement[i]);
				GlobalVars.OFZPlanesElement[i] = GlobalVars.gAranGraphics.DrawMultiPolygon(GlobalVars.OFZPlanes[i].Poly, AranEnvironment.Symbols.eFillStyle.sfsHollow, -1, GlobalVars.OFZPlanesState);
			}

			_OCHbyObctacle = 75.0;
			if (Functions.AnaliseObstacles(GlobalVars.ObstacleList, out OFZObstacleList, ptTHRprj, _ArDir, GlobalVars.OFZPlanes) > 0)
				_OCHbyObctacle = 90.0; // RNPAR 2.2.2 ?.?.? old edition 

            GlobalVars.VisibilityBar.SetEnabled(GlobalVars.VisibilityBar.OFZ, true);
			reportForm.FillPage01(OFZObstacleList);

			double OldOCHMin = _OCHMin;
			if (_OCHbyAlignment > _OCHbyObctacle)
				_OCHMin = _OCHbyAlignment;
			else
				_OCHMin = _OCHbyObctacle;

			if (_OCHMin != OldOCHMin)
			{
				TextBox0007.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_OCHMin, eRoundMode.NEAREST).ToString();
				TextBox0010.Tag = null;
				TextBox0010_Validating(TextBox0010, null);
			}

			//TextBox0005_Validating(TextBox0005, null);
		}

		private void FillFAMAObstaclesFields(double FAPaltitude, bool isStraight = true)
		{
			double _MAMOC = 0.0;

			CalcAreaParams(FAPaltitude, isStraight);

			int n = OASObstacleList.Parts.Length;
			double MAG = 0.01 * (double)MAGUpDwn.Value;
			double CoTanZ = 1.0 / MAG;

			double InvMeanEarthRadius = 1.0 / GlobalVars.MeanEarthRadius;

			for (int i = 0; i < n; i++)
			{
				double x, y;
				ARANFunctions.PrjToLocal(ptTHRprj, _ArDir, OASObstacleList.Parts[i].pPtPrj, out x, out y);
				OASObstacleList.Parts[i].Dist = -x;
				OASObstacleList.Parts[i].DistStar = y;
				OASObstacleList.Parts[i].EffectiveHeight = 0.0;

				if (OASObstacleList.Parts[i].Dist > _OASorigin)
				{
					OASObstacleList.Parts[i].Plane = (int)eOAS.WPlane;
					double p = _OASgradient * (OASObstacleList.Parts[i].Dist - _OASorigin) * InvMeanEarthRadius;
					double q = _TanGPA * OASObstacleList.Parts[i].Dist * InvMeanEarthRadius;
					OASObstacleList.Parts[i].hSurface = (GlobalVars.MeanEarthRadius + ptTHRprj.Z) * (Math.Exp(p) - 1.0);
					OASObstacleList.Parts[i].MOC = (GlobalVars.MeanEarthRadius + ptTHRprj.Z + _GP_RDH) * Math.Exp(p) - GlobalVars.MeanEarthRadius - OASObstacleList.Parts[i].hSurface;

					OASObstacleList.Parts[i].hPenet = OASObstacleList.Parts[i].Height - OASObstacleList.Parts[i].hSurface;
					if (OASObstacleList.Parts[i].hPenet > 0.0)
						OASObstacleList.Parts[i].ReqOCH = OASObstacleList.Parts[i].Height + _HeightLoss;
				}
				else if (x > _zOrigin)
				{
					OASObstacleList.Parts[i].Plane = (int)eOAS.XlPlane;
					OASObstacleList.Parts[i].hSurface = MAG * (x - _zOrigin);
					OASObstacleList.Parts[i].hPenet = OASObstacleList.Parts[i].Height - OASObstacleList.Parts[i].hSurface;

					if (OASObstacleList.Parts[i].hPenet > 0.0)
					{
						double fTmp = ((OASObstacleList.Parts[i].Height + _MAMOC) * CoTanZ + (OASObstacleList.Parts[i].Dist + _zOrigin)) / (CoTanZ + _CoTanGPA);
						OASObstacleList.Parts[i].EffectiveHeight = fTmp;
						OASObstacleList.Parts[i].ReqOCH = fTmp + _HeightLoss;
					}
				}
				else
				{
					OASObstacleList.Parts[i].Plane = (int)eOAS.ZeroPlane;
					OASObstacleList.Parts[i].hSurface = 0.0;
					OASObstacleList.Parts[i].hPenet = OASObstacleList.Parts[i].Height - OASObstacleList.Parts[i].hSurface;

					if (OASObstacleList.Parts[i].hPenet > 0.0)
						OASObstacleList.Parts[i].ReqOCH = OASObstacleList.Parts[i].Height + _HeightLoss;
				}
			}

			reportForm.FillPage02(OASObstacleList);
		}

		private void CalcAreaParams(double FAPaltitude, bool isStraight)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_OasLineElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_ZoriginLineElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_SOCElem);

			_MOC75 = FASMOCatAltitude(75.0 + ptTHRprj.Z, isStraight);
			_MOCfap = FASMOCatAltitude(FAPaltitude, isStraight);

			_OASgradient = ((FAPaltitude - ptTHRprj.Z - _MOCfap) - (75.0 - _MOC75)) / ((FAPaltitude - ptTHRprj.Z - 75.0) * _CoTanGPA);
			_OASorigin = (75.0 - _GP_RDH) * _CoTanGPA - (75.0 - _MOC75) / _OASgradient;

			double anpe = 1.225 * _MARNPval;
			double wpr = 18.288;
			double fte = 22.86 * _CoTanGPA;
			//double dISA = _Tmin - 15.0;
			double Vtas = ARANMath.IASToTAS(_IAS, GlobalVars.CurrADHP.Elev, 15.0);
			double t = 15;

			_TrD = t * (Vtas + 18.52 / 3.6) + 4.0 / 3.0 * Math.Sqrt(anpe * anpe + wpr * wpr + fte * fte);
			_zOrigin = _TrD - (_HeightLoss - _GP_RDH) * _CoTanGPA;

			double xSOC = (_PrelDHval - _GP_RDH) * _CoTanGPA - _TrD;
			ptSOCprj = ARANFunctions.LocalToPrj(ptTHRprj, _ArDir, -xSOC);

			_SOCElem = GlobalVars.gAranGraphics.DrawPointWithText(ptSOCprj, "SOC", GlobalVars.WPTColor);

			GeometryOperators geomOp = new GeometryOperators();
			geomOp.CurrentGeometry = _MASegmentPolygon;

			LineString ls = new LineString();
			ls.Add(ARANFunctions.LocalToPrj(ptTHRprj, _ArDir, -_OASorigin, 100000.0));
			ls.Add(ARANFunctions.LocalToPrj(ptTHRprj, _ArDir, -_OASorigin, -100000.0));

			_pOasLine = (MultiLineString)geomOp.Intersect(ls);
			_OasLineElem = GlobalVars.gAranGraphics.DrawMultiLineString(_pOasLine, 2, ARANFunctions.RGB(0, 255, 0));

			ls.Clear();
			ls.Add(ARANFunctions.LocalToPrj(ptTHRprj, _ArDir, _zOrigin, 100000.0));
			ls.Add(ARANFunctions.LocalToPrj(ptTHRprj, _ArDir, _zOrigin, -100000.0));

			_pZorigin = (MultiLineString)geomOp.Intersect(ls);
			_ZoriginLineElem = GlobalVars.gAranGraphics.DrawMultiLineString(_pZorigin, 2, ARANFunctions.RGB(0, 0, 255));

			TextBox0110.Text = GlobalVars.unitConverter.GradientToDisplayUnits(_OASgradient, eRoundMode.NEAREST).ToString();
			TextBox0111.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_OASorigin, eRoundMode.NEAREST).ToString();
			TextBox0112.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_zOrigin, eRoundMode.NEAREST).ToString();
			TextBox0113.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_TrD, eRoundMode.NEAREST).ToString();
		}

		private void CreateMAProtArea()
		{
			double FASsemiWidth = 2.0 * _FASRNPval; //RNPAR 4.1.7
			double ThrToSplStartDist = (_PrelDHval - _GP_RDH) * _CoTanGPA;

			double MAsemiWidth = 2.0 * _MARNPval; //RNPAR 4.1.7
            double SplStartToEndDist = coTan15 * 2.0 * (_MARNPval - _FASRNPval);

			_FapToThrDist = hFAP2FAPDist(_PrelFAPalt - ptTHRgeo.Z);

			ptFAPprj = ARANFunctions.LocalToPrj(ptTHRprj, _ArDir, -_FapToThrDist);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_FAPElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_MASegmentElem);

			_FAPElem = GlobalVars.gAranGraphics.DrawPointWithText(ptFAPprj, "FAP", GlobalVars.WPTColor);

			Point pt0l = ARANFunctions.LocalToPrj(ptTHRprj, _ArDir, -_FapToThrDist - 5.0 * 1852.0, FASsemiWidth);
			Point pt0r = ARANFunctions.LocalToPrj(ptTHRprj, _ArDir, -_FapToThrDist - 5.0 * 1852.0, -FASsemiWidth);

			Point pt1l = ARANFunctions.LocalToPrj(ptTHRprj, _ArDir, -ThrToSplStartDist, FASsemiWidth);
			Point pt1r = ARANFunctions.LocalToPrj(ptTHRprj, _ArDir, -ThrToSplStartDist, -FASsemiWidth);

			Point pt2l = ARANFunctions.LocalToPrj(pt1l, _ArDir, SplStartToEndDist, MAsemiWidth - FASsemiWidth);
			Point pt2r = ARANFunctions.LocalToPrj(pt1r, _ArDir, SplStartToEndDist, -MAsemiWidth + FASsemiWidth);

			Point pt3l = ARANFunctions.LocalToPrj(pt2l, _ArDir, 27780.0);
			Point pt3r = ARANFunctions.LocalToPrj(pt2r, _ArDir, 27780.0);

			Ring MASegmentRing = new Ring();
			MASegmentRing.Add(pt0l);
			MASegmentRing.Add(pt1l);
			MASegmentRing.Add(pt2l);
			MASegmentRing.Add(pt3l);

			MASegmentRing.Add(pt3r);
			MASegmentRing.Add(pt2r);
			MASegmentRing.Add(pt1r);
			MASegmentRing.Add(pt0r);

			Polygon pPolygon = new Polygon();
			pPolygon.ExteriorRing = MASegmentRing;
			_MASegmentPolygon = new MultiPolygon();
			_MASegmentPolygon.Add(pPolygon);
			_MASegmentElem = GlobalVars.gAranGraphics.DrawMultiPolygon(_MASegmentPolygon, eFillStyle.sfsHollow, ARANFunctions.RGB(0, 245, 245));

			Functions.GetObstaclesByPolygonWithDecomposition(GlobalVars.ObstacleList, out OASObstacleList, _MASegmentPolygon);

			FillFAMAObstaclesFields(_PrelFAPalt);
		}

		private void ComboBox0001_SelectedIndexChanged(object sender, EventArgs e)
		{
			int RWYIndex = ComboBox0001.SelectedIndex;
			if (RWYIndex < 0)
				return;

			_SelectedRWY = (RWYType)ComboBox0001.SelectedItem;

			ptTHRprj = _SelectedRWY.pPtPrj[eRWY.ptTHR];
			ptTHRgeo = _SelectedRWY.pPtGeo[eRWY.ptTHR];
			_RWYDir = ptTHRprj.M;

			DBModule.GetObstaclesByDist(out GlobalVars.ObstacleList, GlobalVars.CurrADHP.pPtPrj, GlobalVars.ModellingRadius, ptTHRgeo.Z);

			TextBox0001.Text = GlobalVars.unitConverter.HeightToDisplayUnits(ptTHRgeo.Z, eRoundMode.NEAREST).ToString();
			TextBox0002.Text = ptTHRgeo.M.ToString("0.00");
			nUpDown0002_ValueChanged(nUpDown0002, null);

			CreateOFZPlanes();
			CreateMAProtArea();
		}

		private void ComboBox0002_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!FormInitialised)
				return;

			int k = ComboBox0002.SelectedIndex;
			if (k < 0) return;

			_Category = k;
			_IAS = GlobalVars.constants.AircraftCategory[aircraftCategoryData.VfafMax].Value[_Category];
			TextBox0013.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(_IAS).ToString();
			TextBox0013.Tag = TextBox0013.Text;

			decimal oldv = nUpDown0001.Value;
			double oldPrelDH = _PrelDHval;
			//===============================================================;
			decimal[] MaxGPAngle = new decimal[] { 6.4m, 4.2m, 3.6m, 3.1m, 3.1m };

			nUpDown0001.Minimum = (decimal)ARANMath.RadToDeg(GlobalVars.constants.Pansops[ePANSOPSData.arOptGPAngle].Value);
			nUpDown0001.Maximum = MaxGPAngle[_Category];

			//===============================================================;
			//if (oldv != nUpDown0001.Value)
			ComboBox0003_SelectedIndexChanged(ComboBox0003, null);

			TextBox0003.Tag = null;
			TextBox0003_Validating(TextBox0003, null);

			if (oldPrelDH == _PrelDHval)
				CreateMAProtArea();
		}

		private void ComboBox0003_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ComboBox0003.SelectedIndex < 0)
			{
				ComboBox0003.SelectedIndex = 0;
				return;
			}

			double fMOCCorrH, fMOCCorrGP, fMargin;

			if (ComboBox0003.SelectedIndex == 0)                        //Radio	//	fMargin = 0.096 / 0.277777777777778 * cVatMax.Values(k) - 3.2
				fMargin = 0.34406047516199 * GlobalVars.constants.AircraftCategory[aircraftCategoryData.VatMax].Value[_Category] - 3.2;                 // 0.3456
			else                                                        //Baro	//	fMargin = 0.068 / 0.277777777777778 * cVatMax.Values(k) + 28.3
				fMargin = 0.24298056155508 * GlobalVars.constants.AircraftCategory[aircraftCategoryData.VatMax].Value[_Category] + 28.3;                    // 0.2448

			if (GlobalVars.CurrADHP.pPtGeo.Z > 900.0)
				fMOCCorrH = GlobalVars.CurrADHP.pPtGeo.Z * fMargin / 1500.0;
			else
				fMOCCorrH = 0.0;

			if (_GPAngle > ARANMath.DegToRad(3.2))
				fMOCCorrGP = (_GPAngle - ARANMath.DegToRad(3.2)) * fMargin * 0.5;
			else
				fMOCCorrGP = 0.0;

			_HeightLoss = fMargin + fMOCCorrGP + fMOCCorrH;
			TextBox0004.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_HeightLoss, eRoundMode.NEAREST).ToString();
		}

		private void nUpDown0001_ValueChanged(object sender, EventArgs e)
		{
			_GPAngle = (double)nUpDown0001.Value * ARANMath.DegToRadValue; //RNPAR 4.5.19 RNPAR 4.5.20 RNPAR 4.5.21
            _TanGPA = System.Math.Tan(_GPAngle); 
            _CoTanGPA = 1.0 / _TanGPA;

			ComboBox0003_SelectedIndexChanged(ComboBox0003, null);
		}

		private void nUpDown0002_ValueChanged(object sender, EventArgs e)
		{
			_ArCourse = ptTHRgeo.M + (double)nUpDown0002.Value; //RNPAR 4.5.5
			_ArDir = ARANFunctions.AztToDirection(ptTHRgeo, _ArCourse, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);
			TextBox0006.Text = _ArCourse.ToString("0.00");
			CreateMAProtArea();
		}

		private void MAGUpDwn_ValueChanged(object sender, EventArgs e)
		{
			FillFAMAObstaclesFields(_PrelFAPalt);
		}

		private void TextBox0003_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0003_Validating(TextBox0003, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0003.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0003_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(TextBox0003.Text, out fTmp))
				return;

			if (TextBox0003.Tag != null && TextBox0003.Tag.ToString() == TextBox0003.Text)
				return;

			double[] _RDHNom = new double[] { 12.192, 13.716, 15.24, 15.24, 15.24 };

			fTmp = _GP_RDH = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);

			if (_GP_RDH < _RDHNom[_Category] - rdhToler)
				_GP_RDH = _RDHNom[_Category] - rdhToler;

			if (_GP_RDH > _RDHNom[_Category] + rdhToler)
				_GP_RDH = _RDHNom[_Category] + rdhToler;

			if (fTmp != _GP_RDH)
				TextBox0003.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_GP_RDH, eRoundMode.NEAREST).ToString();
			TextBox0003.Tag = TextBox0003.Text;

			double oldPrelDH = _PrelDHval;

			TextBox0005_Validating(TextBox0005, null);

			if (oldPrelDH != _PrelDHval)
				CreateMAProtArea();
		}

		private void TextBox0005_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0005_Validating(TextBox0005, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0005.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0005_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(TextBox0005.Text, out fTmp))
				return;

			if (TextBox0005.Tag != null && TextBox0005.Tag.ToString() == TextBox0005.Text)
				return;

			_AlignP_THR = fTmp;

			if (_AlignP_THR < _AlignP_THRMin)
				_AlignP_THR = _AlignP_THRMin;

			if (_AlignP_THR > _AlignP_THRMax)
				_AlignP_THR = _AlignP_THRMax;

			if (fTmp != _AlignP_THR)
				TextBox0005.Text = System.Math.Round(_AlignP_THR, 2).ToString("0.0");
			TextBox0005.Tag = TextBox0005.Text;

			_OCHbyAlignment = _GP_RDH + _AlignP_THR * _TanGPA;

			double OldOCHMin = _OCHMin;
			if (_OCHbyAlignment > _OCHbyObctacle)
				_OCHMin = _OCHbyAlignment;
			else
				_OCHMin = _OCHbyObctacle;

			if (_OCHMin != OldOCHMin)
			{
				TextBox0007.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_OCHMin, eRoundMode.CEIL).ToString();
				TextBox0010.Tag = null;
				TextBox0010_Validating(TextBox0010, null);
			}
		}

		private void TextBox0008_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0008_Validating(TextBox0008, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0008.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0008_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(TextBox0008.Text, out fTmp))
				return;

			if (TextBox0008.Tag != null && TextBox0008.Tag.ToString() == TextBox0008.Text)
				return;

			fTmp = _FASRNPval = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

			if (_FASRNPval < FASRNPvalMin)
				_FASRNPval = FASRNPvalMin;

			if (_FASRNPval > FASRNPvalMax)
				_FASRNPval = FASRNPvalMax;

			if (fTmp != _FASRNPval)
				TextBox0008.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_FASRNPval, eRoundMode.NEAREST).ToString();
			TextBox0008.Tag = TextBox0008.Text;

			CreateMAProtArea();
		}

		private void TextBox0009_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0009_Validating(TextBox0009, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0009.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0009_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(TextBox0009.Text, out fTmp))
				return;

			if (TextBox0009.Tag != null && TextBox0009.Tag.ToString() == TextBox0009.Text)
				return;

			fTmp = _MARNPval = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

			if (_MARNPval < MARNPvalMin)
				_MARNPval = MARNPvalMin;

			if (_MARNPval > MARNPvalMax)
				_MARNPval = MARNPvalMax;

			if (fTmp != _MARNPval)
				TextBox0009.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_MARNPval, eRoundMode.NEAREST).ToString();
			TextBox0009.Tag = TextBox0009.Text;

			CreateMAProtArea();
		}

		private void TextBox0010_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0010_Validating(TextBox0010, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0010.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0010_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(TextBox0010.Text, out fTmp))
				return;

			if (TextBox0010.Tag != null && TextBox0010.Tag.ToString() == TextBox0010.Text)
				return;

			double oldPrelDHval = _PrelDHval;
			fTmp = _PrelDHval = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);

			if (_PrelDHval < _OCHMin)
				_PrelDHval = _OCHMin;

			if (_PrelDHval > 4.0 * 75.0)
				_PrelDHval = 4.0 * 75.0;

			if (fTmp != _PrelDHval)
				TextBox0010.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_PrelDHval, eRoundMode.NEAREST).ToString();
			TextBox0010.Tag = TextBox0010.Text;

			if (oldPrelDHval != _PrelDHval)
			{
				TextBox0011.Tag = null;
				TextBox0011_Validating(TextBox0011, null);
			}
			else
				CreateMAProtArea();
		}

		private void TextBox0011_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0011_Validating(TextBox0011, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0011.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0011_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(TextBox0011.Text, out fTmp))
				return;

			if (TextBox0011.Tag != null && TextBox0011.Tag.ToString() == TextBox0011.Text)
				return;

			fTmp = _PrelFAPalt = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);
			double minval = GlobalVars.unitConverter.HeightToInternalUnits(GlobalVars.unitConverter.HeightToDisplayUnits(2.0 * _PrelDHval + ptTHRprj.Z, eRoundMode.SPECIAL_CEIL));

			if (_PrelFAPalt < minval)
				_PrelFAPalt = minval;

			//if (_PrelFAPalt > 4.0 * 75.0)
			//	_PrelFAPalt = 4.0 * 75.0;

			if (fTmp != _PrelFAPalt)
				TextBox0011.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_PrelFAPalt, eRoundMode.NEAREST).ToString();
			TextBox0011.Tag = TextBox0010.Text;

			int ix = (int)Math.Ceiling((_PrelFAPalt - 152.4) / 152.4);
			if (ix < 0) ix = 0;
			else if (ix > 21) ix = 21;

			_CurrTWC = _MaxTWC = TWCTable[ix].speed / 3.6; //RNPAR 3.2.4
            TextBox0014.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(_MaxTWC).ToString();
			TextBox0014.Tag = TextBox0014.Text;

			CreateMAProtArea();
		}

		private void TextBox0012_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0012_Validating(TextBox0012, null);
			else
				Functions.TextBoxFloatWithMinus(ref eventChar, TextBox0012.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0012_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(TextBox0012.Text, out fTmp))
				return;

			if (TextBox0012.Tag != null && TextBox0012.Tag.ToString() == TextBox0012.Text)
				return;

			_Tmin = fTmp;

			if (_Tmin < -100.0)
				_Tmin = -100.0;

			if (_Tmin > 15.0)
				_Tmin = 15.0;

			if (fTmp != _Tmin)
				TextBox0012.Text = _Tmin.ToString();
			TextBox0012.Tag = TextBox0012.Text;

			FillFAMAObstaclesFields(_PrelFAPalt);
		}

		private void TextBox0013_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0013_Validating(TextBox0013, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0013.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0013_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(TextBox0013.Text, out fTmp))
				return;

			if (TextBox0013.Tag != null && TextBox0013.Tag.ToString() == TextBox0013.Text)
				return;

			_IAS = GlobalVars.unitConverter.SpeedToInternalUnits(fTmp);
			fTmp = _IAS;

			if (_IAS < GlobalVars.constants.AircraftCategory[aircraftCategoryData.VfafMin].Value[_Category])
				_IAS = GlobalVars.constants.AircraftCategory[aircraftCategoryData.VfafMin].Value[_Category];

			if (_IAS > GlobalVars.constants.AircraftCategory[aircraftCategoryData.VfafMax].Value[_Category])
				_IAS = GlobalVars.constants.AircraftCategory[aircraftCategoryData.VfafMax].Value[_Category];

			if (fTmp != _IAS)
				TextBox0013.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(_IAS).ToString();
			TextBox0013.Tag = TextBox0013.Text;
		}

		private void TextBox0014_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0014_Validating(TextBox0014, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0014.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0014_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(TextBox0014.Text, out fTmp))
				return;

			if (TextBox0014.Tag != null && TextBox0014.Tag.ToString() == TextBox0014.Text)
				return;

			_CurrTWC = GlobalVars.unitConverter.SpeedToInternalUnits(fTmp);
			fTmp = _CurrTWC;

			if (_CurrTWC < 0.0)
				_CurrTWC = 0.0;

			if (_CurrTWC > _MaxTWC)
				_CurrTWC = _MaxTWC;

			if (fTmp != _CurrTWC)
				TextBox0014.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(_CurrTWC).ToString();
			TextBox0014.Tag = TextBox0014.Text;
		}

		#endregion

		#region Page II

		private void leavePageII()
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_FROPElem);

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_CurrFASLeg.NominalElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_CurrFASLeg.ProtectionElem);
			int n = _FASLegs.Count;

			for (int i = 0; i < n; i++)
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_FASLegs[i].NominalElem);
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_FASLegs[i].ProtectionElem);

			}

			if (_FASLegs.Count > 0)                 //??????????????????????????????????????
				_FASLegs.RemoveAt(_FASLegs.Count - 1);

			//NextBtn.Enabled = false;
		}

		private bool preparePageII()
		{
			_FASLegs.Clear();

			//DBModule.GetObstaclesByDist(out GlobalVars.ObstacleList, GlobalVars.CurrADHP.pPtPrj, GlobalVars.ModellingRadius, ptTHRgeo.Z);

			double dt, Vtas = 3.6 * ARANMath.IASToTAS(_IAS, GlobalVars.CurrADHP.Elev, 0.0);

            //RNPAR 4.5.12
			if (_MARNPval >= 1852.0)
			{
				double d15 = (_PrelDHval - _GP_RDH) * _CoTanGPA + 4.1666666666666666 * Vtas + 4.1666666666666666 * 27.78;       //115.75
				dt = d15;
			}
			else
			{
				double d50 = (_PrelDHval - _GP_RDH) * _CoTanGPA + (Vtas + 27.78) * 13.89;
				dt = d50;
			}

            //RNPAR 4.5.11
            double D150 = (150.0 - _GP_RDH) * _CoTanGPA;

			_MinDistFROPtoNEXT = Math.Max(D150, dt);
            //================================================================================

            //RNPAR 3.2.7 ?? what about 3.2.11 
            double Vwind = 3.6 * _CurrTWC;
			double V = Vtas + Vwind;

			_FASmaxBank = ARANMath.RadToDeg(Math.Atan(3.0 * Math.PI * V / 6355.0));

			//if (_FASmaxBank > 18.0)				_FASmaxBank = 18.0;		//??????????????????????
			//================================================================================
			_hNext = _GP_RDH + ptTHRgeo.Z;
			_MinFROPAltitude = FAPDist2hFAP(_MinDistFROPtoNEXT, _hNext);
            
            //RNPAR 4.1 ???? 
			_FASminRadius = CalcRadius(_MinFROPAltitude - GlobalVars.CurrADHP.Elev, _FASmaxBank);
			if (_FASminRadius <= 2.0 * _FASRNPval)
				_FASminRadius = Math.Ceiling(2.01 * _FASRNPval);

			_FASmaxRadius = CalcRadius(_PrelFAPalt - GlobalVars.CurrADHP.Elev, 1.0);

			//=========================================================================================
			_totalLenght = 0.0;
			_FASLegs.Clear();
			_ptNextprj = ptTHRprj;

			//_hNext = ptTHRprj.Z;
			//_FROPtoTHRdistance = _MinDistFROPtoTHR;

			_CurrFASLeg = new RFLeg();

			_CurrFASLeg.DescentGR = _TanGPA;
			_CurrFASLeg.RNPvalue = _FASRNPval;
			_CurrFASLeg.BankAngle = _FASmaxBank;
			_CurrFASLeg.DistToNext = _MinDistFROPtoNEXT;
			_CurrFASLeg.RollOutAltitude = _MinFROPAltitude;
			_CurrFASLeg.Radius = CalcRadius(_PrelFAPalt - GlobalVars.CurrADHP.Elev, _FASmaxBank);
			_CurrFASLeg.RollOutDir = _ArDir;

			_CurrFASLeg.Nominal = new MultiLineString();
			_CurrFASLeg.Protection = new MultiPolygon();

			TextBox0101.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_CurrFASLeg.DistToNext).ToString();
			TextBox0102.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_CurrFASLeg.RollOutAltitude).ToString();
			TextBox0103.Text = _CurrFASLeg.BankAngle.ToString("0.0");
			TextBox0104.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_CurrFASLeg.Radius).ToString();

			TextBox0107.Text = TextBox0101.Text;
			TextBox0108.Text = TextBox0102.Text;

			TextBox0101.Tag = null;
			TextBox0105.Tag = null;

			TextBox0101_Validating(TextBox0101, null);
			TextBox0105_Validating(TextBox0105, null);

			ComboBox0101_SelectedIndexChanged(ComboBox0101, null);
			radioButton0101_CheckedChanged(radioButton0101, null);

			CheckRadius();
			return true;
		}

		private void CreateStraightLeg(RFLeg nextLeg, ref RFLeg currLeg)
		{
			//currLeg.StartPrj = new Point(currLeg.RollOutPrj);
			//currLeg.StartDir = currLeg.RollOutDir;

			Ring ring = new Ring();
			ring.Add(ARANFunctions.LocalToPrj(currLeg.RollOutPrj, nextLeg.StartDir, 0.0, 2.0 * currLeg.RNPvalue));
			ring.Add(ARANFunctions.LocalToPrj(nextLeg.StartPrj, nextLeg.StartDir, 0.0, 2.0 * currLeg.RNPvalue));
			ring.Add(ARANFunctions.LocalToPrj(nextLeg.StartPrj, nextLeg.StartDir, 0.0, -2.0 * currLeg.RNPvalue));
			ring.Add(ARANFunctions.LocalToPrj(currLeg.RollOutPrj, nextLeg.StartDir, 0.0, -2.0 * currLeg.RNPvalue));

			Polygon pPoly = new Polygon();
			pPoly.ExteriorRing = ring;

			currLeg.Protection.Add(pPoly);
			//===================================================
			LineString ls = new LineString();
			ls.Add(currLeg.RollOutPrj);
			ls.Add(nextLeg.StartPrj);
			currLeg.Nominal.Add(ls);
		}

		private void CreateRFLeg(int turnDir, ref RFLeg currLeg)
		{
			currLeg.Center = ARANFunctions.LocalToPrj(currLeg.RollOutPrj, currLeg.RollOutDir, 0.0, turnDir * currLeg.Radius);
			currLeg.StartPrj = ARANFunctions.LocalToPrj(currLeg.Center, currLeg.StartDir, 0.0, -turnDir * currLeg.Radius);

			Ring ring = new Ring();

			Point from = ARANFunctions.LocalToPrj(currLeg.StartPrj, currLeg.StartDir, 0.0, 2.0 * currLeg.RNPvalue);
			Point to = ARANFunctions.LocalToPrj(currLeg.RollOutPrj, currLeg.RollOutDir, 0.0, 2.0 * currLeg.RNPvalue);

			LineString ls = ARANFunctions.CreateArcAsPartPrj(currLeg.Center, from, to, currLeg.TurnDir);
			ring.AddMultiPoint(ls);

			from = ARANFunctions.LocalToPrj(currLeg.StartPrj, currLeg.StartDir, 0.0, -2.0 * currLeg.RNPvalue);
			to = ARANFunctions.LocalToPrj(currLeg.RollOutPrj, currLeg.RollOutDir, 0.0, -2.0 * currLeg.RNPvalue);

			ls = ARANFunctions.CreateArcAsPartPrj(currLeg.Center, from, to, currLeg.TurnDir);
			ring.AddReverse(ls);

			Polygon pPoly = new Polygon();
			pPoly.ExteriorRing = ring;
			currLeg.Protection.Add(pPoly);
			//===================================================

			ls = ARANFunctions.CreateArcAsPartPrj(currLeg.Center, currLeg.StartPrj, currLeg.RollOutPrj, currLeg.TurnDir);
			currLeg.Nominal.Add(ls);

			currLeg.StartAltitude = currLeg.RollOutAltitude + ls.Length * currLeg.DescentGR;
		}

		private double CalcRadius(double height, double bank)
		{
            //RNPAR 3.2.7 ?? what about 3.2.11 
            double Vtas = ARANMath.IASToTAS(_IAS, GlobalVars.CurrADHP.Elev + height, 0.0);

			//int ix = (int)Math.Ceiling((height - 152.4) / 152.4);
			//if (ix < 0) ix = 0;
			//else if (ix > 21) ix = 21;

			double Vwind = _CurrTWC;
			double V = 3.6 * (Vtas + Vwind);
			double R = (6355.0 * Math.Tan(ARANMath.DegToRad(bank))) / (Math.PI * V);

			if (R > 3.0)
				R = 3.0;

			return 1000.0 * V / (20 * Math.PI * R);
		}

		private double CalcBank(double height, double radius)
		{
            //RNPAR 4.1 3.2.8  ?? what about 3.2.11 
            double Vtas = ARANMath.IASToTAS(_IAS, GlobalVars.CurrADHP.Elev + height, 0.0);

			//int ix = (int)Math.Ceiling((height - 152.4) / 152.4);
			//if (ix < 0) ix = 0;
			//else if (ix > 21) ix = 21;

			double Vwind = _CurrTWC;
			double V = 3.6 * (Vtas + Vwind);

			//double R = (6355.0 * Math.Tan(ARANMath.DegToRad(bank))) / (Math.PI * V);
			//		bank = ARANMath.RadToDeg(Math.Atan(3.0 * Math.PI * V / 6355.0));
			//double bank = ARANMath.RadToDeg(Math.Atan(V * V / (127094.0 * radius)));

			double R = 50.0 * V / (radius * Math.PI);

			if (R > 3.0)
				R = 3.0;

			double bank = ARANMath.RadToDeg(Math.Atan(R * Math.PI * V / 6355.0));

			//if (bank > 18.0)				bank = 18.0;		//???????????????????????
			return bank;
		}

		private void CheckRadius()
		{
			double fTmp = _CurrFASLeg.Radius;

			if (_CurrFASLeg.Radius < _FASminRadius)
				_CurrFASLeg.Radius = _FASminRadius;

			if (_CurrFASLeg.Radius > _FASmaxRadius)
				_CurrFASLeg.Radius = _FASmaxRadius;

			if (Math.Abs(fTmp - _CurrFASLeg.Radius) > ARANMath.Epsilon_2Distance)       //			if (fTmp != _CurrFASLeg.Radius)
			{
				TextBox0104.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_CurrFASLeg.Radius).ToString();
				TextBox0104.Tag = TextBox0104.Text;
				CreateFASLeg();
			}

			fTmp = _CurrFASLeg.BankAngle;
			_CurrFASLeg.BankAngle = CalcBank(_CurrFASLeg.RollOutAltitude - GlobalVars.CurrADHP.Elev, _CurrFASLeg.Radius);

			if (Math.Abs(fTmp - _CurrFASLeg.BankAngle) > ARANMath.EpsilonDegree)
			{
				CheckBank();
				TextBox0103.Text = _CurrFASLeg.BankAngle.ToString("0.0");
				TextBox0103.Tag = TextBox0103.Text;
			}
		}

		private void CheckBank()
		{
			double fTmp = _CurrFASLeg.BankAngle;

			if (_CurrFASLeg.BankAngle < 1.0)
				_CurrFASLeg.BankAngle = 1.0;

			if (_CurrFASLeg.BankAngle > _FASmaxBank)
				_CurrFASLeg.BankAngle = _FASmaxBank;

			if (fTmp != _CurrFASLeg.BankAngle)
			{
				TextBox0103.Text = _CurrFASLeg.BankAngle.ToString("0.0");
				TextBox0103.Tag = TextBox0103.Text;
			}

			fTmp = _CurrFASLeg.Radius;
			_CurrFASLeg.Radius = CalcRadius(_CurrFASLeg.RollOutAltitude - GlobalVars.CurrADHP.Elev, _CurrFASLeg.BankAngle);

			if (Math.Abs(fTmp - _CurrFASLeg.Radius) > ARANMath.Epsilon_2Distance)
			{
				TextBox0104.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_CurrFASLeg.Radius).ToString();
				TextBox0104.Tag = TextBox0104.Text;

				CreateFASLeg();
			}
		}

		private void CreateFASLeg()
		{
			if (_FASturndir == 0)
				return;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_CurrFASLeg.NominalElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_CurrFASLeg.ProtectionElem);

			//==================================================
			_CurrFASLeg.Nominal.Clear();
			_CurrFASLeg.Protection.Clear();

			if (_FASLegs.Count != 0)
				CreateStraightLeg(_FASLegs[_FASLegs.Count - 1], ref _CurrFASLeg);

			TextBox0106.Text = "-";

			if (radioButton0101.Checked)
			{
				CreateRFLeg(_FASturndir, ref _CurrFASLeg);
				TextBox0106.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_CurrFASLeg.StartAltitude).ToString();
			}

			//===================================================
			_CurrFASLeg.ProtectionElem = GlobalVars.gAranGraphics.DrawMultiPolygon(_CurrFASLeg.Protection, eFillStyle.sfsHollow, ARANFunctions.RGB(0, 0, 255));
			_CurrFASLeg.NominalElem = GlobalVars.gAranGraphics.DrawMultiLineString(_CurrFASLeg.Nominal, 2, 255);

			double commomL = _totalLenght + _CurrFASLeg.Nominal.Length;

			if (_FASLegs.Count == 0)
				commomL += _CurrFASLeg.DistToNext;

			TextBox0109.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(commomL).ToString();
		}

		private void ComboBox0101_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ComboBox0101.SelectedIndex < 0)
			{
				ComboBox0101.SelectedIndex = 0;
				return;
			}

			_FASturndir = 1 - 2 * ComboBox0101.SelectedIndex;
			_CurrFASLeg.TurnDir = (TurnDirection)_FASturndir;

			TextBox0105.Tag = null;
			TextBox0105_Validating(TextBox0105, null);
		}

		private void radioButton0101_CheckedChanged(object sender, EventArgs e)
		{
			if (!((RadioButton)sender).Checked)
				return;

			//=============================================
			Label0101.Enabled = radioButton0101.Checked;
			Label0102.Enabled = radioButton0101.Checked;
			Label0103.Enabled = radioButton0101.Checked;
			Label0104.Enabled = radioButton0101.Checked;
			Label0107.Enabled = radioButton0101.Checked;
			Label0105.Enabled = radioButton0101.Checked;
			Label0106.Enabled = radioButton0101.Checked;
			Label0108.Enabled = radioButton0101.Checked;
			Label0109.Enabled = radioButton0101.Checked;
			Label0110.Enabled = radioButton0101.Checked;
			Label0111.Enabled = radioButton0101.Checked;
			Label0112.Enabled = radioButton0101.Checked;
			Label0113.Enabled = radioButton0101.Checked;

			TextBox0101.Enabled = radioButton0101.Checked;
			TextBox0102.Enabled = radioButton0101.Checked;
			TextBox0103.Enabled = radioButton0101.Checked;
			TextBox0104.Enabled = radioButton0101.Checked;
			TextBox0105.Enabled = radioButton0101.Checked;
			TextBox0106.Enabled = radioButton0101.Checked;

			ComboBox0101.Enabled = radioButton0101.Checked;
			button0101.Enabled = radioButton0101.Checked;
			//=============================================
			Label0114.Enabled = radioButton0102.Checked;
			Label0115.Enabled = radioButton0102.Checked;
			Label0116.Enabled = radioButton0102.Checked;
			Label0117.Enabled = radioButton0102.Checked;

			TextBox0107.Enabled = radioButton0102.Checked;
			TextBox0108.Enabled = radioButton0102.Checked;

			NextBtn.Enabled = radioButton0102.Checked;
			//=============================================
			if (radioButton0101.Checked)
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_FAPElem);
				_FAPElem = GlobalVars.gAranGraphics.DrawPointWithText(ptFAPprj, "FAP", GlobalVars.WPTColor);

				TextBox0101.Text = TextBox0107.Text;
				TextBox0102.Text = TextBox0108.Text;

				TextBox0101.Tag = null;
				TextBox0101_Validating(TextBox0101, null);
			}
			else
			{
				if (_FASLegs.Count == 0)
					GlobalVars.gAranGraphics.SafeDeleteGraphic(_FROPElem);

				TextBox0107.Text = TextBox0101.Text;
				TextBox0108.Text = TextBox0102.Text;

				TextBox0107.Tag = null;
				TextBox0107_Validating(TextBox0107, null);
			}
		}

		private void button0101_Click(object sender, EventArgs e)
		{
			RFLeg nextLeg = _CurrFASLeg;
			_ptNextprj = nextLeg.StartPrj;
			_ptNextprj.Z = nextLeg.StartAltitude;
			_hNext = nextLeg.StartAltitude;

			MultiPolygon mlp = (MultiPolygon)nextLeg.Protection.Clone();		//  new MultiPolygon();
			MultiLineString mls = (MultiLineString)nextLeg.Nominal;				// new MultiLineString();
			GeometryOperators geoOp = new GeometryOperators();
			double InvMeanEarthRadius = 1.0 / GlobalVars.MeanEarthRadius;

			foreach (var leg in _FASLegs)
			{
				mlp = (MultiPolygon)geoOp.UnionGeometry(mlp, leg.Protection);
				mls = (MultiLineString)geoOp.UnionGeometry(mls, leg.Nominal);
			}

			Functions.GetObstaclesByPolygonWithDecomposition(GlobalVars.ObstacleList, out FASObstaclesList, mlp);

			geoOp.CurrentGeometry = mls;

			int n = FASObstaclesList.Parts.Length;
			for (int i = 0; i < n; i++)
			{
				//while (true)
				//	Application.DoEvents();

				if (mls.IsEmpty)
					FASObstaclesList.Parts[i].Dist = 0.0;
				else
				{
					Point ptTan = geoOp.GetNearestPoint(FASObstaclesList.Parts[i].pPtPrj);

					//GlobalVars.gAranGraphics.DrawPointWithText(ls[0],-1,"p-0");
					//GlobalVars.gAranGraphics.DrawPointWithText(ls[ls.Count-1 ], -1, "p-n");
					//GlobalVars.gAranGraphics.DrawPointWithText(ptTan, -1, "ptTan-1");
					//Application.DoEvents();
					LineString ls = (LineString)mls[0].Clone();

					double x0, y0;
					double x1, y1;
					double distance = 0.0;

					while (ls.Count > 1)
					{
						Point pt0 = ls[0];
						Point pt1 = ls[1];
						double dir = ARANFunctions.ReturnAngleInRadians(pt0, pt1);
						double dist = ARANFunctions.ReturnDistanceInMeters (pt0, pt1);

						ARANFunctions.PrjToLocal(pt0, dir, ptTan, out x0, out y0);
						if (y0 * y0 < ARANMath.Epsilon_2Distance )
						{
							ARANFunctions.PrjToLocal(pt1, dir, ptTan, out x1, out y1);
							if (x0 * x1 < 0.0)
							{
								ls.Remove(0);
								ls.Insert(0, ptTan);

								//ls[0] = ptTan;
								distance = ls.Length;
								//distance  = Math.Sqrt(ls.CalculateLengthSquare());

								//GlobalVars.gAranGraphics.DrawLineString(ls, -1,2);
								//do
								//	Application.DoEvents();
								//while (true);
								break;
							}
						}
						ls.Remove(0);
					}

					//double x, y;
					//ARANFunctions.PrjToLocal(ptTHRprj, _ArDir, OASObstacleList.Parts[i].pPtPrj, out x, out y);
					//FASObstaclesList.Parts[i].Dist = -x;
					double commomL = _totalLenght + distance;

					if (_FASLegs.Count == 0)
						commomL += _CurrFASLeg.DistToNext;


					FASObstaclesList.Parts[i].Dist = commomL;// _totalLenght += nextLeg.Nominal.Length;	//??????????????????????
				}

				double p = _OASgradient * (FASObstaclesList.Parts[i].Dist - _OASorigin) * InvMeanEarthRadius;
				double q = _TanGPA * FASObstaclesList.Parts[i].Dist * InvMeanEarthRadius;

				FASObstaclesList.Parts[i].hSurface = (GlobalVars.MeanEarthRadius + ptTHRprj.Z) * (Math.Exp(p) - 1.0);
				//FASObstaclesList.Parts[i].MOC = (GlobalVars.MeanEarthRadius + ptTHRprj.Z + _GP_RDH) * Math.Exp(p) - GlobalVars.MeanEarthRadius - FASObstaclesList.Parts[i].hSurface;

				FASObstaclesList.Parts[i].hPenet = FASObstaclesList.Parts[i].Height - FASObstaclesList.Parts[i].hSurface;
				//if (FASObstaclesList.Parts[i].hPenet > 0.0)
				//	FASObstaclesList.Parts[i].ReqOCH = FASObstaclesList.Parts[i].Height + _HeightLoss;
			}

			_CurrFASLeg.ObstaclesList = FASObstaclesList;
			_FASLegs.Add(_CurrFASLeg);

			reportForm.FillPage03(FASObstaclesList);

			_CurrFASLeg = new RFLeg();
			_CurrFASLeg.Nominal = new MultiLineString();
			_CurrFASLeg.Protection = new MultiPolygon();

			_CurrFASLeg.DescentGR = _TanGPA;
			_CurrFASLeg.RNPvalue = _FASRNPval;
			_CurrFASLeg.BankAngle = nextLeg.BankAngle;
			_CurrFASLeg.Radius = nextLeg.Radius;
			_CurrFASLeg.TurnDir = nextLeg.TurnDir;

			_CurrFASLeg.RollOutDir = nextLeg.StartDir;
			_CurrFASLeg.RollOutAltitude = nextLeg.StartAltitude;

			TextBox0101.Tag = null;
			TextBox0105.Tag = null;

			TextBox0101_Validating(TextBox0101, null);
			TextBox0105_Validating(TextBox0105, null);

			_totalLenght += nextLeg.Nominal.Length;
			ComboBox0101_SelectedIndexChanged(ComboBox0101, null);
		}

		private void button0102_Click(object sender, EventArgs e)
		{

		}

		private void TextBox0101_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0101_Validating(TextBox0101, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0101.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0101_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(TextBox0101.Text, out fTmp))
				return;

			if (TextBox0101.Tag != null && TextBox0101.Tag.ToString() == TextBox0101.Text)
				return;

			fTmp = _CurrFASLeg.DistToNext = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

			double minDist = _FASLegs.Count > 0 ? 0 : _MinDistFROPtoNEXT;
			if (_CurrFASLeg.DistToNext < minDist)
				_CurrFASLeg.DistToNext = minDist;

			if (_CurrFASLeg.DistToNext > _FapToThrDist)
				_CurrFASLeg.DistToNext = _FapToThrDist;

			if (fTmp != _CurrFASLeg.DistToNext)
				TextBox0101.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_CurrFASLeg.DistToNext).ToString();
			TextBox0101.Tag = TextBox0101.Text;

			_CurrFASLeg.RollOutPrj = ARANFunctions.LocalToPrj(_ptNextprj, _CurrFASLeg.RollOutDir, -_CurrFASLeg.DistToNext);
			_CurrFASLeg.RollOutGeo = GlobalVars.pspatialReferenceOperation.ToGeo<Point>(_CurrFASLeg.RollOutPrj);
			_CurrFASLeg.RollOutAltitude = FAPDist2hFAP(_CurrFASLeg.DistToNext, _hNext);

			TextBox0102.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_CurrFASLeg.RollOutAltitude).ToString();

			_CurrFASLeg.RollOutPrj.Z = _CurrFASLeg.RollOutAltitude;
			_CurrFASLeg.RollOutGeo.Z = _CurrFASLeg.RollOutAltitude;

			if (_FASLegs.Count == 0)
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_FROPElem);
				_FROPElem = GlobalVars.gAranGraphics.DrawPointWithText(_CurrFASLeg.RollOutPrj, "FROP", GlobalVars.WPTColor);
			}

			CreateFASLeg();
			FillFAMAObstaclesFields(_CurrFASLeg.StartAltitude, false);
		}

		private void TextBox0103_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0103_Validating(TextBox0103, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0103.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0103_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(TextBox0103.Text, out fTmp))
				return;

			if (TextBox0103.Tag != null && TextBox0103.Tag.ToString() == TextBox0103.Text)
				return;

			_CurrFASLeg.BankAngle = fTmp;
			CheckBank();
			TextBox0103.Tag = TextBox0103.Text;
		}

		private void TextBox0104_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0104_Validating(TextBox0104, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0104.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0104_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(TextBox0104.Text, out fTmp))
				return;

			if (TextBox0104.Tag != null && TextBox0104.Tag.ToString() == TextBox0104.Text)
				return;

			_CurrFASLeg.Radius = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);
			CheckRadius();
			TextBox0104.Tag = TextBox0104.Text;

			CreateFASLeg();
		}

		private void TextBox0105_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0105_Validating(TextBox0105, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0105.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0105_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(TextBox0105.Text, out fTmp))
				return;

			if (TextBox0105.Tag != null && TextBox0105.Tag.ToString() == TextBox0105.Text)
				return;

			double InCourse = NativeMethods.Modulus(fTmp);
			double ExitCourse = ARANFunctions.DirToAzimuth(_CurrFASLeg.RollOutPrj, _CurrFASLeg.RollOutDir, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			double dAzt = NativeMethods.Modulus(_FASturndir * (fTmp - ExitCourse));

			if (dAzt > 240)
				fTmp = NativeMethods.Modulus(ExitCourse + _FASturndir * 240.0);

			if (fTmp != InCourse)
				TextBox0105.Text = fTmp.ToString("0.00");

			_CurrFASLeg.StartDir = ARANFunctions.AztToDirection(_CurrFASLeg.RollOutGeo, fTmp, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);
			TextBox0105.Tag = TextBox0105.Text;

			CreateFASLeg();
		}

		private void TextBox0107_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0107_Validating(TextBox0107, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0107.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0107_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(TextBox0107.Text, out fTmp))
				return;

			if (TextBox0107.Tag != null && TextBox0107.Tag.ToString() == TextBox0107.Text)
				return;

			fTmp = _CurrFASLeg.DistToNext = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

			if (_CurrFASLeg.DistToNext < _MinDistFROPtoNEXT)
				_CurrFASLeg.DistToNext = _MinDistFROPtoNEXT;

			if (_CurrFASLeg.DistToNext > _FapToThrDist)
				_CurrFASLeg.DistToNext = _FapToThrDist;

			if (fTmp != _CurrFASLeg.DistToNext)
				TextBox0107.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_CurrFASLeg.DistToNext).ToString();
			TextBox0107.Tag = TextBox0107.Text;

			_CurrFASLeg.StartAltitude = FAPDist2hFAP(_CurrFASLeg.DistToNext, _hNext);
			_CurrFASLeg.StartPrj = ARANFunctions.LocalToPrj(_ptNextprj, _CurrFASLeg.RollOutDir, -_CurrFASLeg.DistToNext);
			_CurrFASLeg.StartPrj.Z = _CurrFASLeg.StartAltitude;

			_CurrFASLeg.StartGeo = GlobalVars.pspatialReferenceOperation.ToGeo<Point>(_CurrFASLeg.StartPrj);
			_CurrFASLeg.StartGeo.Z = _CurrFASLeg.StartAltitude;

			_CurrFASLeg.RollOutPrj = _CurrFASLeg.StartPrj;
			_CurrFASLeg.RollOutGeo = _CurrFASLeg.StartGeo;
			_CurrFASLeg.RollOutAltitude = _CurrFASLeg.StartAltitude;

			TextBox0108.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_CurrFASLeg.StartAltitude).ToString();


			_CurrFASLeg.StartPrj = new Point(_CurrFASLeg.RollOutPrj);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_FAPElem);
			_FAPElem = GlobalVars.gAranGraphics.DrawPointWithText(_CurrFASLeg.RollOutPrj, "FAP", GlobalVars.WPTColor);

			CreateFASLeg();

			FillFAMAObstaclesFields(_CurrFASLeg.StartAltitude, false);
		}

		#endregion

		#region Page III

		private void leavePageIII()
		{
			//NextBtn.Enabled = false;
		}

		private bool preparePageIII()
		{
			_CurrFASLeg.StartDir = _CurrFASLeg.RollOutDir;
			_ptNextprj = _CurrFASLeg.StartPrj;
			_ptNextprj.Z = _hNext = _CurrFASLeg.StartAltitude;
			_FASLegs.Add(_CurrFASLeg);

			//=========================================================================================
			//GlobalVars.constants.AircraftCategory[aircraftCategoryData.VatMax].Value[_Category]
			//GlobalVars.constants.AircraftCategory[aircraftCategoryData.VatMax].Value[_Category]
			//ViafMin, ViafMax
			double ias = GlobalVars.constants.AircraftCategory[aircraftCategoryData.ViafMax].Value[_Category];
			double tas = 3.6 * ARANMath.IASToTAS(ias, GlobalVars.CurrADHP.Elev + _hNext, 0.0);

			_ImASminRadius = CalcImASRadius(tas, _hNext - GlobalVars.CurrADHP.Elev, 20.0);
			if (_ImASminRadius <= 2.0 * _FASRNPval)
				_ImASminRadius = Math.Ceiling(2.01 * _FASRNPval);

			_ImASmaxRadius = CalcRadius(_hNext - GlobalVars.CurrADHP.Elev, 1.0);

			//=========================================================================================
			_ImASLegs = new List<RFLeg>();
			_CurrImASLeg = new RFLeg();

			_CurrImASLeg.DescentGR = _TanGPA;
			_CurrImASLeg.IAS = ias;
			_CurrImASLeg.TAS = tas;

			_CurrImASLeg.BankAngle = _FASmaxBank;
			_CurrImASLeg.DistToNext = 0.0;
			_CurrImASLeg.RollOutAltitude = _hNext;
			_CurrImASLeg.Radius = _ImASmaxRadius;
			_CurrImASLeg.RollOutDir = _CurrFASLeg.StartDir;
			_CurrImASLeg.RollOutPrj = new Point(_CurrFASLeg.StartPrj);
			_CurrImASLeg.RNPvalue = GlobalVars.unitConverter.DistanceFromNM(double.Parse(textBox0202.Text));
			_CurrImASLeg.DistToNext = _CurrImASLeg.RNPvalue;
			_CurrImASLeg.MOC = GlobalVars.unitConverter.HeightToInternalUnits(double.Parse(textBox0201.Text));
			_CurrImASLeg.RollOutGeo = GlobalVars.pspatialReferenceOperation.ToGeo<Point>(_CurrImASLeg.RollOutPrj);

			_CurrImASLeg.Nominal = new MultiLineString();
			_CurrImASLeg.Protection = new MultiPolygon();

			textBox0201.Text = GlobalVars.unitConverter.HeightToDisplayUnits(150.0).ToString();
			textBox0203.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_CurrImASLeg.RollOutAltitude).ToString();
			textBox0204.Text = _CurrImASLeg.BankAngle.ToString("0.0");
			textBox0205.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_CurrImASLeg.Radius).ToString();
			textBox0207.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_CurrImASLeg.DistToNext).ToString();
			textBox0208.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(_CurrImASLeg.IAS).ToString();

			//textBox0207.Tag = textBox0207.Text;

			textBox0206.Tag = null;
			textBox0206_Validating(textBox0206, null);

			comboBox0201_SelectedIndexChanged(comboBox0201, null);
			radioButton0201_CheckedChanged(radioButton0201, null);

			_CurrImASLeg.RNPvalue = GlobalVars.unitConverter.DistanceFromNM(double.Parse(textBox0202.Text));
			_CurrImASLeg.MOC = GlobalVars.unitConverter.HeightToInternalUnits(double.Parse(textBox0201.Text));
			CheckImASRadius();

			//CreateImASLeg();
			return true;
		}

		private void CreateNextLeg(RFLeg nextLeg, ref RFLeg currLeg)    //CreateNextPart
		{
			Ring ring;
			Polygon pPoly;
			LineString ls;
			Point from, to;

			if (nextLeg.legType == LegType.Straight)
			{
				ring = new Ring();
				//ring.Add(ARANFunctions.LocalToPrj(nextLeg.StartPrj, currLeg.RollOutDir, currLeg.RNPvalue, 2.0 * currLeg.RNPvalue));
				//ring.Add(ARANFunctions.LocalToPrj(nextLeg.StartPrj, nextLeg.StartDir, 0.0, 2.0 * currLeg.RNPvalue));
				//ring.Add(ARANFunctions.LocalToPrj(nextLeg.StartPrj, nextLeg.StartDir, 0.0, -2.0 * currLeg.RNPvalue));
				//ring.Add(ARANFunctions.LocalToPrj(nextLeg.StartPrj, currLeg.RollOutDir, currLeg.RNPvalue, -2.0 * currLeg.RNPvalue));

				ring.Add(ARANFunctions.LocalToPrj(nextLeg.StartPrj, nextLeg.RollOutDir, currLeg.RNPvalue, 2.0 * currLeg.RNPvalue));
				ring.Add(ARANFunctions.LocalToPrj(nextLeg.StartPrj, nextLeg.RollOutDir, 0.0, 2.0 * currLeg.RNPvalue));
				ring.Add(ARANFunctions.LocalToPrj(nextLeg.StartPrj, nextLeg.RollOutDir, 0.0, -2.0 * currLeg.RNPvalue));
				ring.Add(ARANFunctions.LocalToPrj(nextLeg.StartPrj, nextLeg.RollOutDir, currLeg.RNPvalue, -2.0 * currLeg.RNPvalue));
				//ring.IsExterior = true;

				pPoly = new Polygon();
				pPoly.ExteriorRing = ring;
				//GlobalVars.gAranGraphics.DrawPolygon(pPoly, -1, eFillStyle.sfsBackwardDiagonal);
				///GlobalVars.gAranGraphics.DrawPointWithText(nextLeg.StartPrj, -1, "nextLeg.StartPrj");
				//while(true)
				//Application.DoEvents();

				currLeg.Protection.Add(pPoly);

				//GlobalVars.gAranGraphics.DrawMultiPolygon(currLeg.Protection, -1, eFillStyle.sfsBackwardDiagonal);
				//Application.DoEvents();
				//===================================================
				ls = new LineString();
				ls.Add(nextLeg.StartPrj);
				ls.Add(ARANFunctions.LocalToPrj(nextLeg.StartPrj, nextLeg.StartDir, currLeg.RNPvalue));

				//GlobalVars.gAranGraphics.DrawLineString(ls, -1, 2);
				//Application.DoEvents();

				currLeg.Nominal.Add(ls);
			}
			else if (nextLeg.legType == LegType.FixedRadius)
			{
				ring = new Ring();

				double alpha = currLeg.RNPvalue / (nextLeg.Radius - 2.0 * currLeg.RNPvalue);
				from = ARANFunctions.LocalToPrj(nextLeg.StartPrj, nextLeg.StartDir, 0.0, -2.0 * currLeg.RNPvalue);
				to = ARANFunctions.LocalToPrj(nextLeg.Center, nextLeg.StartDir - (int)nextLeg.TurnDir * (ARANMath.C_PI_2 - alpha), nextLeg.Radius - 2.0 * currLeg.RNPvalue, 0.0);

				ls = ARANFunctions.CreateArcAsPartPrj(nextLeg.Center, from, to, nextLeg.TurnDir);
				ring.AddMultiPoint(ls);

				alpha = currLeg.RNPvalue / (nextLeg.Radius + 2.0 * currLeg.RNPvalue);
				from = ARANFunctions.LocalToPrj(nextLeg.StartPrj, nextLeg.StartDir, 0.0, 2.0 * currLeg.RNPvalue);
				to = ARANFunctions.LocalToPrj(nextLeg.Center, nextLeg.StartDir - (int)nextLeg.TurnDir * (ARANMath.C_PI_2 - alpha), nextLeg.Radius + 2.0 * currLeg.RNPvalue, 0.0);

				ls = ARANFunctions.CreateArcAsPartPrj(nextLeg.Center, from, to, nextLeg.TurnDir);
				ring.AddReverse(ls);

				pPoly = new Polygon();
				pPoly.ExteriorRing = ring;

				currLeg.Protection.Add(pPoly);

				//===================================================
				alpha = currLeg.RNPvalue / nextLeg.Radius;

				to = ARANFunctions.LocalToPrj(nextLeg.Center, nextLeg.StartDir - (int)nextLeg.TurnDir * (ARANMath.C_PI_2 - alpha), nextLeg.Radius, 0.0);
				ls = ARANFunctions.CreateArcAsPartPrj(nextLeg.Center, nextLeg.StartPrj, to, nextLeg.TurnDir);

				currLeg.Nominal.Add(ls);
			}
			else
			{
				int turnDir = (int)nextLeg.TurnDir;
				Point ptFIX = (Point)ARANFunctions.LineLineIntersect(nextLeg.StartPrj, nextLeg.StartDir, nextLeg.RollOutPrj, nextLeg.RollOutDir);

				double turnangle = turnDir * (nextLeg.RollOutDir - nextLeg.StartDir);
				if (turnangle < 0.0)
					turnangle += 2.0 * Math.PI;

				double bisect = nextLeg.RollOutDir + 0.5 * turnDir * (Math.PI - turnangle);

				double rIn = nextLeg.Radius + nextLeg.RNPvalue;
				double l1 = (nextLeg.Radius + 3.0 * currLeg.RNPvalue) / Math.Cos(0.5 * turnangle);
				Point ptCnt1 = ARANFunctions.LocalToPrj(ptFIX, bisect, l1);

				//GlobalVars.gAranGraphics.DrawPointWithText(ptCnt1, -1, "Cnt-1");
				//Application.DoEvents();

				//================================================================================================
				Point ptInStart = ARANFunctions.LocalToPrj(ptCnt1, nextLeg.StartDir, 0, -turnDir * rIn);

				//GlobalVars.gAranGraphics.DrawPointWithText(ptInStart, -1, "Inner Start");
				//Application.DoEvents();
				//ptInStart = ARANFunctions.LocalToPrj(ptCnt1, currLeg.StartDir, 0, -turnDir * rIn);

				double x = Math.Sqrt(rIn * rIn - nextLeg.RNPvalue * nextLeg.RNPvalue);
				Point ptInEnd = ARANFunctions.LocalToPrj(ptCnt1, bisect, -x, -turnDir * currLeg.RNPvalue);

				//GlobalVars.gAranGraphics.DrawPointWithText(ptInStart, -1, "Inner Start");
				//GlobalVars.gAranGraphics.DrawPointWithText(ptInEnd, -1, "Inner End");
				////while(true)
				//Application.DoEvents();

				//Point ptOutStart = ARANFunctions.LocalToPrj(ptFIX, currLeg.ExitDir + Math.PI, 2.0 * currLeg.RNPvalue);
				Point ptOutStart = ARANFunctions.LocalToPrj(ptFIX, currLeg.StartDir, 0, -2.0 * turnDir * currLeg.RNPvalue);
				//GlobalVars.gAranGraphics.DrawPointWithText(ptOutStart, -1, "Outer Start");
				//Application.DoEvents();

				x = currLeg.RNPvalue * ARANMath.C_SQRT3;
				Point ptOutEnd = ARANFunctions.LocalToPrj(ptFIX, bisect, -x, -turnDir * currLeg.RNPvalue);
				//GlobalVars.gAranGraphics.DrawPointWithText(ptOutEnd, -1, "Outer End");
				//Application.DoEvents();

				Point ptOutPrev = ARANFunctions.LocalToPrj(ptCnt1, currLeg.StartDir, 0, -turnDir * (rIn + 4.0 * currLeg.RNPvalue));
				//GlobalVars.gAranGraphics.DrawPointWithText(ptOutPrev, -1, "Outer Prev");
				//Application.DoEvents();

				ring = new Ring();
				ls = ARANFunctions.CreateArcAsPartPrj(ptCnt1, ptInStart, ptInEnd, nextLeg.TurnDir);

				//GlobalVars.gAranGraphics.DrawLineString(ls, -1, 2);
				//Application.DoEvents();

				ring.AddMultiPoint(ls);

				ls = ARANFunctions.CreateArcAsPartPrj(ptFIX, ptOutStart, ptOutEnd, nextLeg.TurnDir);

				//GlobalVars.gAranGraphics.DrawLineString(ls, -1, 2);
				//Application.DoEvents();

				ring.AddReverse(ls);
				ring.Add(ptOutPrev);


				pPoly = new Polygon();
				pPoly.ExteriorRing = ring;
				currLeg.Protection.Add(pPoly);

				//GlobalVars.gAranGraphics.DrawPolygon(pPoly, -1, eFillStyle.sfsBackwardDiagonal);
				////while(true)
				//Application.DoEvents();

				// cutter Polygon ==================================

				Ring cutRing = new Ring();
				cutRing.Add(ARANFunctions.LocalToPrj(ptFIX, nextLeg.StartDir, currLeg.RNPvalue, 2.0 * currLeg.RNPvalue));
				cutRing.Add(ARANFunctions.LocalToPrj(ptFIX, nextLeg.StartDir, currLeg.RNPvalue, -2.0 * currLeg.RNPvalue));
				cutRing.Add(ARANFunctions.LocalToPrj(ptFIX, nextLeg.StartDir, 5.0 * currLeg.RNPvalue, -2.0 * currLeg.RNPvalue));
				cutRing.Add(ARANFunctions.LocalToPrj(ptFIX, nextLeg.StartDir, 5.0 * currLeg.RNPvalue, 2.0 * currLeg.RNPvalue));

				Polygon cutPoly = new Polygon();
				cutPoly.ExteriorRing = cutRing;

				//GlobalVars.gAranGraphics.DrawPolygon(cutPoly, -1, eFillStyle.sfsHorizontal);
				//Application.DoEvents();

				// cutter Line ==================================
				LineString cutLine = new LineString();
				cutLine.Add(ptCnt1);
				cutLine.Add(ARANFunctions.LocalToPrj(ptFIX, bisect + Math.PI, 5.0 * nextLeg.RNPvalue));

				//GlobalVars.gAranGraphics.DrawLineString(cutLine, 255, 2);
				//Application.DoEvents();

				GeometryOperators geoOps = new GeometryOperators();
				//geoOps.CurrentGeometry = cutPoly;

				if (geoOps.Crosses(cutPoly, cutLine))
				{
					Geometry geomLeft, geomRight;
					geoOps.Cut(cutPoly, cutLine, out geomLeft, out geomRight);

					if (nextLeg.TurnDir == TurnDirection.CCW)
						cutPoly = ((MultiPolygon)geomLeft)[0];
					else
						cutPoly = ((MultiPolygon)geomRight)[0];
				}

				//GlobalVars.gAranGraphics.DrawPolygon(cutPoly, -1, eFillStyle.sfsVertical);
				//Application.DoEvents();

				//geoOps.CurrentGeometry = cutPoly;
				if (geoOps.Crosses(cutPoly, currLeg.Protection))
				{
					//MultiPolygon result = (MultiPolygon)geoOps.Difference(cutPoly, currLeg.Protection);
					//GlobalVars.gAranGraphics.DrawMultiPolygon(result, -1, eFillStyle.sfsCross);
					//while(true)
					//Application.DoEvents();

					MultiPolygon result1 = (MultiPolygon)geoOps.Difference(currLeg.Protection, cutPoly);
					//GlobalVars.gAranGraphics.DrawMultiPolygon(result1 , -1, eFillStyle.sfsDiagonalCross);
					//Application.DoEvents();

					currLeg.Protection = result1;
				}

				//===================================================

				ls = ARANFunctions.CreateArcAsPartPrj(nextLeg.Center, nextLeg.StartPrj, nextLeg.RollOutPrj, nextLeg.TurnDir);

				//GlobalVars.gAranGraphics.DrawLineString(ls, ARANFunctions.RGB(0, 255,0), 2);
				//Application.DoEvents();

				currLeg.Nominal.Add(ls);
				currLeg.StartAltitude = currLeg.RollOutAltitude + ls.Length * currLeg.DescentGR;
			}
		}

		private void CreatePrevLeg(ref RFLeg currLeg, RFLeg prevLeg)    //CreatePrevPart
		{
			Ring ring;
			Polygon pPoly;
			LineString ls;
			Point from, to;

			if (prevLeg.legType == LegType.Straight)
			{
				ring = new Ring();
				ring.Add(ARANFunctions.LocalToPrj(currLeg.StartPrj, prevLeg.RollOutDir, 0.0, 2.0 * currLeg.RNPvalue));
				ring.Add(ARANFunctions.LocalToPrj(currLeg.StartPrj, prevLeg.RollOutDir, -prevLeg.RNPvalue, 2.0 * currLeg.RNPvalue));
				ring.Add(ARANFunctions.LocalToPrj(currLeg.StartPrj, prevLeg.RollOutDir, -prevLeg.RNPvalue, -2.0 * currLeg.RNPvalue));
				ring.Add(ARANFunctions.LocalToPrj(currLeg.StartPrj, prevLeg.RollOutDir, 0.0, -2.0 * currLeg.RNPvalue));

				pPoly = new Polygon();
				pPoly.ExteriorRing = ring;

				//GlobalVars.gAranGraphics.DrawPolygon(pPoly, -1, eFillStyle.sfsBackwardDiagonal);
				//Application.DoEvents();

				currLeg.Protection.Add(pPoly);
				//===================================================
				ls = new LineString();
				ls.Add(ARANFunctions.LocalToPrj(currLeg.StartPrj, prevLeg.RollOutDir, -prevLeg.RNPvalue));
				ls.Add(currLeg.StartPrj);
				currLeg.Nominal.Add(ls);
			}
			else if (prevLeg.legType == LegType.FixedRadius)
			{
				ring = new Ring();


				//GlobalVars.gAranGraphics.DrawPointWithText(currLeg.StartPrj, -1, "currLeg.StartPrj");
				//GlobalVars.gAranGraphics.DrawPointWithText(prevLeg.RollOutPrj, -1, "prevLeg.RollOutPrj");
				////while(true)
				//Application.DoEvents();

				double alpha = prevLeg.RNPvalue / (prevLeg.Radius - 2.0 * currLeg.RNPvalue);
				double fTurnDir = (int)prevLeg.TurnDir;

				to = ARANFunctions.LocalToPrj(currLeg.StartPrj, currLeg.StartDir, 0.0, 2.0 * fTurnDir * currLeg.RNPvalue);
				from = ARANFunctions.LocalToPrj(prevLeg.Center, currLeg.StartDir - fTurnDir * (ARANMath.C_PI_2 + alpha), prevLeg.Radius - 2.0 * currLeg.RNPvalue, 0.0);
				//from = ARANFunctions.LocalToPrj(prevLeg.RollOutPrj, prevLeg.StartDir, 0.0, 2.0 * prevLeg.RNPvalue);
				//to = ARANFunctions.LocalToPrj(prevLeg.Center, prevLeg.StartDir + (int)prevLeg.TurnDir * (alpha + ARANMath.C_PI_2), prevLeg.Radius + 2.0 * prevLeg.RNPvalue, 0.0);

				//GlobalVars.gAranGraphics.DrawPointWithText(from, -1, "from");
				//GlobalVars.gAranGraphics.DrawPointWithText(to, -1, "to");
				//Application.DoEvents();

				//double alpha = currLeg.RNPvalue / prevLeg.Radius;


				ls = ARANFunctions.CreateArcAsPartPrj(prevLeg.Center, from, to, prevLeg.TurnDir);
				ring.AddMultiPoint(ls);


				alpha = prevLeg.RNPvalue / (prevLeg.Radius + 2.0 * currLeg.RNPvalue);

				to = ARANFunctions.LocalToPrj(currLeg.StartPrj, currLeg.StartDir, 0.0, -2.0 * fTurnDir * currLeg.RNPvalue);
				from = ARANFunctions.LocalToPrj(prevLeg.Center, currLeg.StartDir - fTurnDir * (ARANMath.C_PI_2 + alpha), prevLeg.Radius + 2.0 * currLeg.RNPvalue, 0.0);
				//from = ARANFunctions.LocalToPrj(prevLeg.RollOutPrj, prevLeg.StartDir, 0.0, -2.0 * prevLeg.RNPvalue);
				//to = ARANFunctions.LocalToPrj(prevLeg.Center, prevLeg.StartDir + (int)prevLeg.TurnDir * (alpha + ARANMath.C_PI_2), prevLeg.Radius - 2.0 * prevLeg.RNPvalue, 0.0);

				//while(true)
				//Application.DoEvents();

				//GlobalVars.gAranGraphics.DrawPointWithText(from, -1, "from");
				//GlobalVars.gAranGraphics.DrawPointWithText(to, -1, "to");
				//Application.DoEvents();

				ls = ARANFunctions.CreateArcAsPartPrj(prevLeg.Center, from, to, prevLeg.TurnDir);
				ring.AddReverse(ls);

				pPoly = new Polygon();
				pPoly.ExteriorRing = ring;

				//GlobalVars.gAranGraphics.DrawPolygon(pPoly, -1, eFillStyle.sfsForwardDiagonal);
				//Application.DoEvents();

				currLeg.Protection.Add(pPoly);
				//===================================================
				alpha = prevLeg.RNPvalue / prevLeg.Radius;

				from = ARANFunctions.LocalToPrj(prevLeg.Center, currLeg.StartDir - fTurnDir * (ARANMath.C_PI_2 + alpha), prevLeg.Radius, 0.0);
				ls = ARANFunctions.CreateArcAsPartPrj(prevLeg.Center, from, prevLeg.RollOutPrj, prevLeg.TurnDir);

				//GlobalVars.gAranGraphics.DrawLineString(ls, -1, 2);
				//Application.DoEvents();

				currLeg.Nominal.Add(ls);
			}
			else
			{
				int turnDir = (int)prevLeg.TurnDir;
				Point ptFIX = currLeg.StartPrj;

				//GlobalVars.gAranGraphics.DrawPointWithText(ptFIX, -1, "FIX");
				//while(true)
				//Application.DoEvents();

				double turnangle = turnDir * (prevLeg.RollOutDir - prevLeg.StartDir);
				if (turnangle < 0.0)
					turnangle += 2.0 * Math.PI;

				double bisect = prevLeg.RollOutDir + 0.5 * turnDir * (Math.PI - turnangle);

				double rIn = prevLeg.Radius + currLeg.RNPvalue;
				double l1 = (prevLeg.Radius + 3.0 * currLeg.RNPvalue) / Math.Cos(0.5 * turnangle);
				Point ptCnt1 = ARANFunctions.LocalToPrj(ptFIX, bisect, l1);
				//================================================================================================
				//GlobalVars.gAranGraphics.DrawPointWithText(ptCnt1, -1, "Cnt-1");
				//Application.DoEvents();

				//ls = new LineString();
				//ls.Add(currLeg.RollOutPrj);
				//ls.Add(ARANFunctions.LocalToPrj(currLeg.RollOutPrj, currLeg.RollOutDir, 10000.0));
				//GlobalVars.gAranGraphics.DrawLineString(ls, -1, 2);
				//Application.DoEvents();

				//ls = new LineString();
				//ls.Add(currLeg.StartPrj);
				//ls.Add(ARANFunctions.LocalToPrj(currLeg.StartPrj, prevLeg.StartDir, -10000.0));
				//GlobalVars.gAranGraphics.DrawLineString(ls, -1, 2);
				//Application.DoEvents();

				//ls = new LineString();
				//ls.Add(ptFIX);
				//ls.Add(ARANFunctions.LocalToPrj(ptFIX, bisect, 10000.0));
				//GlobalVars.gAranGraphics.DrawLineString(ls, -1, 2);
				//Application.DoEvents();

				Point ptInEnd = ARANFunctions.LocalToPrj(ptCnt1, currLeg.StartDir, 0, -turnDir * rIn);

				//GlobalVars.gAranGraphics.DrawPointWithText(ptInEnd, -1, "Inner End");
				//Application.DoEvents();

				double x = Math.Sqrt(rIn * rIn - prevLeg.RNPvalue * prevLeg.RNPvalue);
				Point ptInStart = ARANFunctions.LocalToPrj(ptCnt1, bisect, -x, turnDir * prevLeg.RNPvalue);
				//GlobalVars.gAranGraphics.DrawPointWithText(ptInStart, -1, "Inner Start-1");
				//while(true)
				//Application.DoEvents();

				//Point ptOutStart = ARANFunctions.LocalToPrj(ptFIX, currLeg.ExitDir + Math.PI, 2.0 * currLeg.RNPvalue);

				Point ptOutEnd = ARANFunctions.LocalToPrj(ptFIX, currLeg.StartDir, 0, -2.0 * turnDir * currLeg.RNPvalue);
				//GlobalVars.gAranGraphics.DrawPointWithText(ptOutEnd, -1, "Outer End");
				//Application.DoEvents();

				x = Math.Sqrt(4 * currLeg.RNPvalue * currLeg.RNPvalue - prevLeg.RNPvalue * prevLeg.RNPvalue);
				Point ptOutStart = ARANFunctions.LocalToPrj(ptFIX, bisect, -x, turnDir * prevLeg.RNPvalue);
				//GlobalVars.gAranGraphics.DrawPointWithText(ptOutStart, -1, "Outer Start-1");
				//Application.DoEvents();

				Point ptOutPrev = ARANFunctions.LocalToPrj(ptCnt1, currLeg.StartDir, 0, -turnDir * (rIn + 4.0 * currLeg.RNPvalue));
				//GlobalVars.gAranGraphics.DrawPointWithText(ptOutPrev, -1, "Outer Prev");
				//Application.DoEvents();

				//GlobalVars.gAranGraphics.DrawPointWithText(ptCnt1, -1, "ptCnt1");
				//Application.DoEvents();

				ring = new Ring();
				ls = ARANFunctions.CreateArcAsPartPrj(ptCnt1, ptInStart, ptInEnd, prevLeg.TurnDir);
				ring.AddReverse(ls);

				ls = ARANFunctions.CreateArcAsPartPrj(ptFIX, ptOutStart, ptOutEnd, prevLeg.TurnDir);
				ring.AddMultiPoint(ls);
				ring.Add(ptOutPrev);

				pPoly = new Polygon();
				pPoly.ExteriorRing = ring;

				//GlobalVars.gAranGraphics.DrawPolygon(pPoly, -1, eFillStyle.sfsBackwardDiagonal);
				////while(true)
				//Application.DoEvents();

				currLeg.Protection.Add(pPoly);

				//GlobalVars.gAranGraphics.DrawPolygon(pPoly, -1, eFillStyle.sfsBackwardDiagonal);
				////while(true)
				//Application.DoEvents();

				// cutter Line ==================================

				Point lineStart = ARANFunctions.LocalToPrj(ptCnt1, bisect, 0, turnDir * prevLeg.RNPvalue);
				Point lineEnd = ARANFunctions.LocalToPrj(ptFIX, bisect, -2.0 * (prevLeg.Radius + prevLeg.RNPvalue + currLeg.RNPvalue), turnDir * prevLeg.RNPvalue);

				//GlobalVars.gAranGraphics.DrawPointWithText(lineStart, -1, "line Start");
				//GlobalVars.gAranGraphics.DrawPointWithText(lineEnd, -1, "line End ");
				///while(true)
				//Application.DoEvents();

				LineString cutLine = new LineString();
				cutLine.Add(lineStart);
				cutLine.Add(lineEnd);

				//GlobalVars.gAranGraphics.DrawLineString(cutLine, 255, 2);
				//Application.DoEvents();

				GeometryOperators geoOps = new GeometryOperators();
				//geoOps.CurrentGeometry = currLeg.Protection;

				//if (!geoOps.Disjoint(currLeg.Protection, cutLine))
				if (geoOps.Crosses(currLeg.Protection, cutLine))
				{
					MultiPolygon cutPoly, cutPoly0, cutPoly1;
					Geometry geomLeft, geomRight;

					geoOps.Cut(currLeg.Protection, cutLine, out geomLeft, out geomRight);
					cutPoly0 = ((MultiPolygon)geomLeft);
					cutPoly1 = ((MultiPolygon)geomRight);

					if (prevLeg.TurnDir == TurnDirection.CCW)
						cutPoly = cutPoly0;
					else
						cutPoly = cutPoly1;

					//if (cutPoly0.Area > cutPoly1.Area)
					//    cutPoly = cutPoly0;
					//else
					//    cutPoly = cutPoly1;

					//GlobalVars.gAranGraphics.DrawMultiPolygon(cutPoly, -1, eFillStyle.sfsHorizontal);
					//Application.DoEvents();

					currLeg.Protection = cutPoly;
				}

				// appendix ==================================

				//GlobalVars.gAranGraphics.DrawPointWithText(ptOutStart, -1, "Outer Start");
				//GlobalVars.gAranGraphics.DrawPointWithText(ptOutEnd, -1, "Outer End");
				//Application.DoEvents();

				//ptOutEnd = ptOutStart;
				//GlobalVars.gAranGraphics.DrawPointWithText(ptOutEnd, -1, "Outer End");
				//Application.DoEvents();
				ptOutStart = ARANFunctions.LocalToPrj(ptFIX, prevLeg.StartDir, 0, -2.0 * turnDir * currLeg.RNPvalue);
				//GlobalVars.gAranGraphics.DrawPointWithText(ptOutStart, -1, "Outer Start-2");
				////while(true)
				//Application.DoEvents();

				//x = currLeg.RNPvalue * ARANMath.C_SQRT3;
				ring = new Ring();

				ls = ARANFunctions.CreateArcAsPartPrj(ptFIX, ptOutStart, ptOutEnd, prevLeg.TurnDir);
				//GlobalVars.gAranGraphics.DrawLineString(ls, -1, 2);
				//Application.DoEvents();
				ring.AddMultiPoint(ls);
				ring.Add(ptFIX);
				ring.Add(ARANFunctions.LocalToPrj(ptFIX, prevLeg.StartDir, 0.0, turnDir * 2.0 * currLeg.RNPvalue));

				ring.Add(ARANFunctions.LocalToPrj(ptFIX, prevLeg.StartDir, -prevLeg.RNPvalue, turnDir * 2.0 * currLeg.RNPvalue));
				ring.Add(ARANFunctions.LocalToPrj(ptFIX, prevLeg.StartDir, -prevLeg.RNPvalue, -turnDir * 2.0 * currLeg.RNPvalue));

				//GlobalVars.gAranGraphics.DrawPointWithText(ARANFunctions.LocalToPrj(ptFIX, prevLeg.StartDir, -prevLeg.RNPvalue, -turnDir * 2.0 * currLeg.RNPvalue), -1, "ptr");
				//Application.DoEvents();
				//GlobalVars.gAranGraphics.DrawRing(ring, -1, eFillStyle.sfsHorizontal);
				//while(true)
				//Application.DoEvents();

				Polygon appendix = new Polygon();
				appendix.ExteriorRing = ring;

				currLeg.Protection = (MultiPolygon)geoOps.UnionGeometry(currLeg.Protection, appendix);

				//Point ptOutStart = ARANFunctions.LocalToPrj(ptFIX, bisect, -x, turnDir * currLeg.RNPvalue);

				//GlobalVars.gAranGraphics.DrawPolygon(cutPoly, -1, eFillStyle.sfsVertical);
				//Application.DoEvents();

				//geoOps.CurrentGeometry = currLeg.Protection;
				//if (!geoOps.Disjoint(currLeg.Protection))
				//{
				//MultiPolygon result = (MultiPolygon)geoOps.Difference(cutPoly, currLeg.Protection);
				//GlobalVars.gAranGraphics.DrawMultiPolygon(result, -1, eFillStyle.sfsCross);
				//while(true)
				//Application.DoEvents();

				//MultiPolygon result1 = (MultiPolygon)geoOps.Difference(currLeg.Protection, cutPoly);
				//currLeg.Protection = result1;
				//GlobalVars.gAranGraphics.DrawMultiPolygon(result1 , -1, eFillStyle.sfsDiagonalCross);
				//Application.DoEvents();
				//currLeg.Protection
				//}

				//===================================================

				ls = currLeg.Nominal[1];

				//GlobalVars.gAranGraphics.DrawLineString(ls, -1, 2);
				//Application.DoEvents();

				//currLeg.Nominal.Remove(1);
				//if (prevLeg.TurnDir== TurnDirection.CCW)
				ls[0] = prevLeg.RollOutPrj;
				//else
				//	ls[1] = prevLeg.RollOutPrj;

				//GlobalVars.gAranGraphics.DrawLineString(ls, -1, 2);
				//Application.DoEvents();

				currLeg.Nominal.Clear();
				currLeg.Nominal.Add(ls);

				//GlobalVars.gAranGraphics.DrawMultiLineString(currLeg.Nominal, 255, 2);
				//Application.DoEvents();
				x = Math.Sqrt(prevLeg.Radius * prevLeg.Radius - currLeg.RNPvalue * currLeg.RNPvalue);
				Point ptStart = ARANFunctions.LocalToPrj(prevLeg.Center, bisect, -x, turnDir * currLeg.RNPvalue);

				//GlobalVars.gAranGraphics.DrawPointWithText(ptStart, -1, "");
				//Application.DoEvents();

				ls = ARANFunctions.CreateArcAsPartPrj(prevLeg.Center, prevLeg.StartPrj, prevLeg.RollOutPrj, prevLeg.TurnDir);

				//GlobalVars.gAranGraphics.DrawLineString(ls, 255, 2);
				//Application.DoEvents();

				currLeg.Nominal.Add(ls);
				currLeg.StartAltitude = currLeg.RollOutAltitude + ls.Length * currLeg.DescentGR;

				//===================================================

			}
		}

		private void CreateImASStraightLeg(RFLeg nextLeg, ref RFLeg currLeg, RFLeg prevLeg = default(RFLeg), bool prevIsValid = false)
		{
			CreateNextLeg(nextLeg, ref currLeg);

			Ring ring;
			Polygon pPoly;
			LineString ls;

			ring = new Ring();
			ring.Add(ARANFunctions.LocalToPrj(currLeg.RollOutPrj, currLeg.RollOutDir, 0.0, 2.0 * currLeg.RNPvalue));
			ring.Add(ARANFunctions.LocalToPrj(nextLeg.StartPrj, nextLeg.StartDir, 0.0, 2.0 * currLeg.RNPvalue));
			ring.Add(ARANFunctions.LocalToPrj(nextLeg.StartPrj, nextLeg.StartDir, 0.0, -2.0 * currLeg.RNPvalue));
			ring.Add(ARANFunctions.LocalToPrj(currLeg.RollOutPrj, currLeg.RollOutDir, 0.0, -2.0 * currLeg.RNPvalue));

			pPoly = new Polygon();
			pPoly.ExteriorRing = ring;
			currLeg.Protection.Add(pPoly);

			//===================================================
			ls = new LineString();
			ls.Add(currLeg.RollOutPrj);
			ls.Add(nextLeg.StartPrj);
			currLeg.Nominal.Add(ls);

			if (!prevIsValid)
				return;

			CreatePrevLeg(ref currLeg, prevLeg);
		}

		private void CreateImASRFLeg(RFLeg nextLeg, ref RFLeg currLeg, RFLeg prevLeg = default(RFLeg), bool prevIsValid = false)
		{
			CreateNextLeg(nextLeg, ref currLeg);

			Ring ring;
			Polygon pPoly;
			LineString ls;
			Point from, to;
			int turnDir = (int)currLeg.TurnDir;
			//===================================================
			currLeg.Center = ARANFunctions.LocalToPrj(currLeg.RollOutPrj, currLeg.RollOutDir, 0.0, turnDir * currLeg.Radius);
			currLeg.StartPrj = ARANFunctions.LocalToPrj(currLeg.Center, currLeg.StartDir, 0.0, -turnDir * currLeg.Radius);

			ring = new Ring();

			from = ARANFunctions.LocalToPrj(currLeg.StartPrj, currLeg.StartDir, 0.0, 2.0 * currLeg.RNPvalue);
			to = ARANFunctions.LocalToPrj(currLeg.RollOutPrj, currLeg.RollOutDir, 0.0, 2.0 * currLeg.RNPvalue);

			//GlobalVars.gAranGraphics.DrawPointWithText(from, -1, "from");
			//GlobalVars.gAranGraphics.DrawPointWithText(to, -1, "_|_");
			//GlobalVars.gAranGraphics.DrawMultiPolygon(currLeg.Protection , -1, eFillStyle.sfsBackwardDiagonal);
			//Application.DoEvents();

			ls = ARANFunctions.CreateArcAsPartPrj(currLeg.Center, from, to, currLeg.TurnDir);
			ring.AddMultiPoint(ls);

			from = ARANFunctions.LocalToPrj(currLeg.StartPrj, currLeg.StartDir, 0.0, -2.0 * currLeg.RNPvalue);
			to = ARANFunctions.LocalToPrj(currLeg.RollOutPrj, currLeg.RollOutDir, 0.0, -2.0 * currLeg.RNPvalue);

			ls = ARANFunctions.CreateArcAsPartPrj(currLeg.Center, from, to, currLeg.TurnDir);
			ring.AddReverse(ls);

			//ring.IsExterior = true;

			pPoly = new Polygon();
			pPoly.ExteriorRing = ring;

			//GlobalVars.gAranGraphics.DrawPolygon(pPoly, -1, eFillStyle.sfsBackwardDiagonal);
			//Application.DoEvents();

			currLeg.Protection.Add(pPoly);

			//===================================================

			ls = ARANFunctions.CreateArcAsPartPrj(currLeg.Center, currLeg.StartPrj, currLeg.RollOutPrj, currLeg.TurnDir);

			//GlobalVars.gAranGraphics.DrawLineString(ls, -1, 2);
			//Application.DoEvents();

			currLeg.Nominal.Add(ls);
			currLeg.StartAltitude = currLeg.RollOutAltitude + ls.Length * currLeg.DescentGR;
			//===================================================
			if (!prevIsValid)
				return;

			//GlobalVars.gAranGraphics.DrawMultiPolygon(currLeg.Protection, -1, eFillStyle.sfsForwardDiagonal);
			//Application.DoEvents();

			CreatePrevLeg(ref currLeg, prevLeg);
		}

		private void CreateIImASFlyByLeg(RFLeg nextLeg, ref RFLeg currLeg, RFLeg prevLeg = default(RFLeg), bool prevIsValid = false)
		{
			//CreateNextLeg(nextLeg, ref currLeg);
			Ring ring;
			Polygon pPoly;
			LineString ls;

			int turnDir = (int)currLeg.TurnDir;
			//===================================================
			currLeg.Center = ARANFunctions.LocalToPrj(currLeg.RollOutPrj, currLeg.RollOutDir, 0.0, turnDir * currLeg.Radius);
			currLeg.StartPrj = ARANFunctions.LocalToPrj(currLeg.Center, currLeg.StartDir, 0.0, -turnDir * currLeg.Radius);

			//ls = new LineString();
			//ls.Add(currLeg.RollOutPrj);
			//ls.Add(ARANFunctions.LocalToPrj(currLeg.RollOutPrj, currLeg.RollOutDir, 10000.0));
			//GlobalVars.gAranGraphics.DrawLineString(ls, -1, 2);

			//ls = new LineString();
			//ls.Add(currLeg.StartPrj);
			//ls.Add(ARANFunctions.LocalToPrj(currLeg.StartPrj, currLeg.StartDir, -10000.0));
			//GlobalVars.gAranGraphics.DrawLineString(ls, -1, 2);
			//Application.DoEvents();


			//Geometry geom = ARANFunctions.LineLineIntersect(currLeg.RollOutPrj, currLeg.RollOutDir, currLeg.StartPrj, currLeg.StartDir);

			//if (geom.Type != GeometryType.Point)
			//{
			//	throw new Exception("Turn angle is not allowed.");
			//	//MessageBox.Show("Turn angle is not allowed.");
			//	//return;
			//}

			Point ptFIX = nextLeg.StartPrj;

			double turnangle = turnDir * (currLeg.RollOutDir - currLeg.StartDir);// +2 * Math.PI;
			if (turnangle < 0.0)
				turnangle += 2.0 * Math.PI;

			double bisect = currLeg.RollOutDir + 0.5 * turnDir * (Math.PI - turnangle);

			//double bisect = 0.5 * (currLeg.StartDir + currLeg.RollOutDir + turnDir * Math.PI);
			//bisect = 0.5 * (currLeg.StartDir + currLeg.RollOutDir + Math.PI) + Math.PI;
			//bisect = currLeg.RollOutDir + 0.5 * turnDir * (Math.PI - turnangle);
			//ls = new LineString();
			//ls.Add(ptFIX);
			//ls.Add(ARANFunctions.LocalToPrj(ptFIX, bisect, 10000.0));
			//GlobalVars.gAranGraphics.DrawLineString(ls, -1, 2);
			//GlobalVars.gAranGraphics.DrawPointWithText(ptFIX, -1, "FIX");
			////while(true)
			//Application.DoEvents();

			double rIn = currLeg.Radius + currLeg.RNPvalue;
			double l1 = (currLeg.Radius + 3.0 * currLeg.RNPvalue) / Math.Cos(0.5 * turnangle);
			Point ptCnt1 = ARANFunctions.LocalToPrj(ptFIX, bisect, l1);

			//MultiPolygon innerCircle = ARANFunctions.CreateCircleAsMultiPolyPrj(ptCnt1, currLeg.Radius + currLeg.RNPvalue);
			//GlobalVars.gAranGraphics.DrawMultiPolygon(innerCircle, -1, eFillStyle.sfsForwardDiagonal);
			//MultiPolygon outerCircle = ARANFunctions.CreateCircleAsMultiPolyPrj(ptFIX, 2.0 * currLeg.RNPvalue);
			//GlobalVars.gAranGraphics.DrawMultiPolygon(outerCircle, -1, eFillStyle.sfsBackwardDiagonal);

			//GlobalVars.gAranGraphics.DrawPointWithText(currLeg.EndPrj, -1, "End");
			//GlobalVars.gAranGraphics.DrawPointWithText(currLeg.StartPrj, -1, "Start");

			//GlobalVars.gAranGraphics.DrawPointWithText(ptFIX, -1, "FIX");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptCnt1, -1, "Cnt-1");
			////while(true)
			//Application.DoEvents();

			Point ptInStart = ARANFunctions.LocalToPrj(ptCnt1, currLeg.StartDir, 0, -turnDir * rIn);

			double x = Math.Sqrt(rIn * rIn - currLeg.RNPvalue * currLeg.RNPvalue);
			Point ptInEnd = ARANFunctions.LocalToPrj(ptCnt1, bisect, -x, -turnDir * currLeg.RNPvalue);

			//GlobalVars.gAranGraphics.DrawPointWithText(ptInStart, -1, "Inner Start");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptInEnd, -1, "Inner End");
			////while(true)
			//Application.DoEvents();

			//Point ptOutStart = ARANFunctions.LocalToPrj(ptFIX, currLeg.ExitDir + Math.PI, 2.0 * currLeg.RNPvalue);
			Point ptOutStart = ARANFunctions.LocalToPrj(ptFIX, currLeg.StartDir, 0, -2.0 * turnDir * currLeg.RNPvalue);
			//GlobalVars.gAranGraphics.DrawPointWithText(ptOutStart, -1, "Outer Start");
			//Application.DoEvents();

			x = currLeg.RNPvalue * ARANMath.C_SQRT3;
			Point ptOutEnd = ARANFunctions.LocalToPrj(ptFIX, bisect, -x, -turnDir * currLeg.RNPvalue);
			//GlobalVars.gAranGraphics.DrawPointWithText(ptOutEnd, -1, "Outer End");
			//Application.DoEvents();

			Point ptOutPrev = ARANFunctions.LocalToPrj(ptCnt1, currLeg.StartDir, 0, -turnDir * (rIn + 4.0 * currLeg.RNPvalue));
			//GlobalVars.gAranGraphics.DrawPointWithText(ptOutPrev, -1, "Outer Prev");
			//Application.DoEvents();

			ring = new Ring();
			ls = ARANFunctions.CreateArcAsPartPrj(ptCnt1, ptInStart, ptInEnd, currLeg.TurnDir);

			//GlobalVars.gAranGraphics.DrawLineString(ls, -1, 2);
			//Application.DoEvents();

			ring.AddMultiPoint(ls);

			ls = ARANFunctions.CreateArcAsPartPrj(ptFIX, ptOutStart, ptOutEnd, currLeg.TurnDir);

			//GlobalVars.gAranGraphics.DrawLineString(ls, -1, 2);
			//Application.DoEvents();

			ring.AddReverse(ls);
			ring.Add(ptOutPrev);


			pPoly = new Polygon();
			pPoly.ExteriorRing = ring;
			currLeg.Protection.Add(pPoly);

			//GlobalVars.gAranGraphics.DrawPolygon(pPoly, -1, eFillStyle.sfsBackwardDiagonal);
			//while(true)
			//Application.DoEvents();

			// cutter Polygon ==================================

			Ring cutRing = new Ring();
			cutRing.Add(ARANFunctions.LocalToPrj(ptFIX, currLeg.StartDir, currLeg.RNPvalue, 2.0 * currLeg.RNPvalue));
			cutRing.Add(ARANFunctions.LocalToPrj(ptFIX, currLeg.StartDir, currLeg.RNPvalue, -2.0 * currLeg.RNPvalue));
			cutRing.Add(ARANFunctions.LocalToPrj(ptFIX, currLeg.StartDir, 5.0 * currLeg.RNPvalue, -2.0 * currLeg.RNPvalue));
			cutRing.Add(ARANFunctions.LocalToPrj(ptFIX, currLeg.StartDir, 5.0 * currLeg.RNPvalue, 2.0 * currLeg.RNPvalue));

			Polygon cutPoly = new Polygon();
			cutPoly.ExteriorRing = cutRing;

			//GlobalVars.gAranGraphics.DrawPolygon(cutPoly, -1, eFillStyle.sfsHorizontal);
			//Application.DoEvents();

			// cutter Line ==================================
			LineString cutLine = new LineString();
			cutLine.Add(ptCnt1);
			cutLine.Add(ARANFunctions.LocalToPrj(ptFIX, bisect + Math.PI, 5.0 * currLeg.RNPvalue));

			//GlobalVars.gAranGraphics.DrawLineString(cutLine, 255, 2);
			//Application.DoEvents();

			GeometryOperators geoOps = new GeometryOperators();
			//geoOps.CurrentGeometry = cutPoly;

			if (geoOps.Crosses(cutPoly, cutLine))
			{
				Geometry geomLeft, geomRight;
				geoOps.Cut(cutPoly, cutLine, out geomLeft, out geomRight);

				if (currLeg.TurnDir == TurnDirection.CCW)
					cutPoly = ((MultiPolygon)geomLeft)[0];
				else
					cutPoly = ((MultiPolygon)geomRight)[0];
			}

			//GlobalVars.gAranGraphics.DrawPolygon(cutPoly, -1, eFillStyle.sfsVertical);
			//Application.DoEvents();

			//geoOps.CurrentGeometry = cutPoly;
			if (geoOps.Crosses(cutPoly, currLeg.Protection))
			{
				//MultiPolygon result = (MultiPolygon)geoOps.Difference(cutPoly, currLeg.Protection);
				//GlobalVars.gAranGraphics.DrawMultiPolygon(result, -1, eFillStyle.sfsCross);
				//while(true)
				//Application.DoEvents();

				MultiPolygon result1 = (MultiPolygon)geoOps.Difference(currLeg.Protection, cutPoly);
				currLeg.Protection = result1;
				//GlobalVars.gAranGraphics.DrawMultiPolygon(result1 , -1, eFillStyle.sfsDiagonalCross);
				//Application.DoEvents();
				//currLeg.Protection
			}

			//Ring eraliestBisectorRing = new Ring();
			//eraliestBisectorRing.Add(ARANFunctions.LocalToPrj() ); 

			//Polygon eraliestBisectorPoly = new Polygon();

			//GlobalVars.gAranGraphics.DrawMultiPolygon(innerCircle, -1, eFillStyle.sfsForwardDiagonal);
			//GlobalVars.gAranGraphics.DrawMultiPolygon(outerCircle, -1, eFillStyle.sfsBackwardDiagonal);
			//Application.DoEvents();

			//ring = new Ring();

			//from = ARANFunctions.LocalToPrj(currLeg.StartPrj, currLeg.EntryDir, 0.0, 2.0 * currLeg.RNPvalue);
			//to = ARANFunctions.LocalToPrj(currLeg.EndPrj, currLeg.ExitDir, 0.0, 2.0 * currLeg.RNPvalue);

			//ls = ARANFunctions.CreateArcAsPartPrj(currLeg.Center, from, to, currLeg.TurnDir);
			//ring.AddMultiPoint(ls);

			//from = ARANFunctions.LocalToPrj(currLeg.StartPrj, currLeg.EntryDir, 0.0, -2.0 * currLeg.RNPvalue);
			//to = ARANFunctions.LocalToPrj(currLeg.EndPrj, currLeg.ExitDir, 0.0, -2.0 * currLeg.RNPvalue);

			//ls = ARANFunctions.CreateArcAsPartPrj(currLeg.Center, from, to, currLeg.TurnDir);
			//ring.AddReverse(ls);


			//===================================================

			ls = ARANFunctions.CreateArcAsPartPrj(currLeg.Center, currLeg.StartPrj, currLeg.RollOutPrj, currLeg.TurnDir);

			//GlobalVars.gAranGraphics.DrawLineString(ls, 255, 2);
			//Application.DoEvents();

			currLeg.Nominal.Add(ls);
			currLeg.StartAltitude = currLeg.RollOutAltitude + ls.Length * currLeg.DescentGR;

			//===================================================
			//if (!prevIsValid)
			//	return;
			//CreatePrevLeg(ref currLeg, prevLeg);
		}

		private double CalcImASRadius(double Vtas, double height, double bank)
		{
			//double Vtas = 3.6 * ARANMath.IASToTAS(IAS, GlobalVars.CurrADHP.Elev + height, 0.0);

			//int ix = (int)Math.Ceiling((height - 152.4) / 152.4);
			//if (ix < 0) ix = 0;
			//else if (ix > 21) ix = 21;

			double Vwind = 3.6 * _CurrTWC;
			double V = Vtas + Vwind;
			double R = (6355.0 * Math.Tan(ARANMath.DegToRad(bank))) / (Math.PI * V);

			if (R > 3.0)
				R = 3.0;

			return 1000.0 * V / (20 * Math.PI * R);
		}

		private double CalcImASBank(double Vtas, double height, double radius)
		{
			//double Vtas = 3.6 * ARANMath.IASToTAS(IAS, GlobalVars.CurrADHP.Elev + height, 0.0);

			//int ix = (int)Math.Ceiling((height - 152.4) / 152.4);
			//if (ix < 0) ix = 0;
			//else if (ix > 21) ix = 21;

			double Vwind = 3.6 * _CurrTWC;
			double V = Vtas + Vwind;

			//double R = (6355.0 * Math.Tan(ARANMath.DegToRad(bank))) / (Math.PI * V);
			//		bank = ARANMath.RadToDeg(Math.Atan(3.0 * Math.PI * V / 6355.0));
			//double bank = ARANMath.RadToDeg(Math.Atan(V * V / (127094.0 * radius)));

			double R = 50.0 * V / (radius * Math.PI);

			if (R > 3.0)
				R = 3.0;

			double bank = ARANMath.RadToDeg(Math.Atan(R * Math.PI * V / 6355.0));

			if (bank > 20.0)
				bank = 20.0;
			return bank;
		}

		private void CheckImASRadius()
		{
			double fTmp = _CurrImASLeg.Radius;

			if (_CurrImASLeg.Radius < _ImASminRadius)
				_CurrImASLeg.Radius = _ImASminRadius;

			if (_CurrImASLeg.Radius > _ImASmaxRadius)
				_CurrImASLeg.Radius = _ImASmaxRadius;

			if (fTmp != _CurrImASLeg.Radius)
			{
				textBox0205.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_CurrImASLeg.Radius).ToString();
				textBox0205.Tag = textBox0205.Text;
				CreateImASLeg();
			}

			fTmp = _CurrImASLeg.BankAngle;
			_CurrImASLeg.BankAngle = CalcImASBank(_CurrImASLeg.TAS, _CurrImASLeg.RollOutAltitude - GlobalVars.CurrADHP.Elev, _CurrImASLeg.Radius);

			if (fTmp != _CurrImASLeg.BankAngle)
			{
				CheckImASBank();
				textBox0204.Text = _CurrImASLeg.BankAngle.ToString("0.0");
				textBox0204.Tag = textBox0204.Text;
			}
		}

		private void CheckImASBank()
		{
			double fTmp = _CurrImASLeg.BankAngle;

			if (_CurrImASLeg.BankAngle < 1.0)
				_CurrImASLeg.BankAngle = 1.0;

			if (_CurrImASLeg.BankAngle > _FASmaxBank)   //_ImASmaxBank
				_CurrImASLeg.BankAngle = _FASmaxBank;

			if (fTmp != _CurrImASLeg.BankAngle)
			{
				textBox0204.Text = _CurrImASLeg.BankAngle.ToString("0.0");
				textBox0204.Tag = textBox0204.Text;
			}

			fTmp = _CurrImASLeg.Radius;
			_CurrImASLeg.Radius = CalcImASRadius(_CurrImASLeg.TAS, _CurrImASLeg.RollOutAltitude - GlobalVars.CurrADHP.Elev, _CurrImASLeg.BankAngle);

			if (fTmp != _CurrImASLeg.Radius)
			{
				textBox0205.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_CurrImASLeg.Radius).ToString();
				textBox0205.Tag = textBox0205.Text;

				CreateImASLeg();
			}
		}

		private void CreateImASLeg()
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_CurrImASLeg.FixElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_CurrImASLeg.NominalElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_CurrImASLeg.ProtectionElem);

			//==================================================
			_CurrImASLeg.Nominal.Clear();
			_CurrImASLeg.Protection.Clear();

			if (radioButton0201.Checked)
			{
				_CurrImASLeg.legType = LegType.FixedRadius;

				_CurrImASLeg.RollOutPrj = (Point)_ptNextprj.Clone();
				_CurrImASLeg.RollOutGeo = GlobalVars.pspatialReferenceOperation.ToGeo<Point>(_CurrImASLeg.RollOutPrj);

				_CurrImASLeg.RollOutPrj.Z = _CurrImASLeg.RollOutAltitude;
				_CurrImASLeg.RollOutGeo.Z = _CurrImASLeg.RollOutAltitude;
				//===

				if (_ImASLegs.Count == 0)
				{
					//RFLeg next = _CurrFASLeg;// _FASLegs[_FASLegs.Count - 1];
					CreateImASRFLeg(_CurrFASLeg, ref _CurrImASLeg);
				}
				else
				{
					RFLeg next = _ImASLegs[_ImASLegs.Count - 1];
					CreateImASRFLeg(next, ref _CurrImASLeg);

					RFLeg nextNext;
					if (_ImASLegs.Count == 1)
						nextNext = _CurrFASLeg;
					else
						nextNext = _ImASLegs[_ImASLegs.Count - 2];

					next.Protection.Clear();
					next.Nominal.Clear();

					if (next.legType == LegType.FixedRadius)
						CreateImASRFLeg(nextNext, ref next, _CurrImASLeg, true);
					else if (next.legType == LegType.FlyBy)
						CreateIImASFlyByLeg(nextNext, ref next, _CurrImASLeg, true);
					else
						CreateImASStraightLeg(nextNext, ref next, _CurrImASLeg, true);

					GlobalVars.gAranGraphics.SafeDeleteGraphic(next.FixElem);
					GlobalVars.gAranGraphics.SafeDeleteGraphic(next.NominalElem);
					GlobalVars.gAranGraphics.SafeDeleteGraphic(next.ProtectionElem);

					next.ProtectionElem = GlobalVars.gAranGraphics.DrawMultiPolygon(next.Protection, eFillStyle.sfsHollow, ARANFunctions.RGB(0, 0, 255));
					next.NominalElem = GlobalVars.gAranGraphics.DrawMultiLineString(next.Nominal, 2, 255);
					next.FixElem = GlobalVars.gAranGraphics.DrawPointWithText(next.StartPrj, "WPT", ARANFunctions.RGB(0, 127, 255));

					_ImASLegs[_ImASLegs.Count - 1] = next;
				}

				textBox0207.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_CurrImASLeg.Nominal.Length - _CurrImASLeg.RNPvalue).ToString();
			}
			else if (radioButton0202.Checked)
			{
				_CurrImASLeg.legType = LegType.FlyBy;

				double turnangle = (_CurrImASLeg.RollOutDir - _CurrImASLeg.StartDir) * (int)_CurrImASLeg.TurnDir;
				double ptDist = _CurrImASLeg.Radius * Math.Tan(0.5 * turnangle);

				_CurrImASLeg.RollOutPrj = ARANFunctions.LocalToPrj(_ptNextprj, _CurrImASLeg.RollOutDir, ptDist);        // (Point)_ptNextprj.Clone();
				_CurrImASLeg.RollOutGeo = GlobalVars.pspatialReferenceOperation.ToGeo<Point>(_CurrImASLeg.RollOutPrj);

				_CurrImASLeg.RollOutPrj.Z = _CurrImASLeg.RollOutAltitude;
				_CurrImASLeg.RollOutGeo.Z = _CurrImASLeg.RollOutAltitude;
				//===

				if (_ImASLegs.Count == 0)
					CreateIImASFlyByLeg(_CurrFASLeg, ref _CurrImASLeg);
				else
				{
					RFLeg next = _ImASLegs[_ImASLegs.Count - 1];
					CreateIImASFlyByLeg(next, ref _CurrImASLeg);

					//_CurrImASLeg.ProtectionElem = GlobalVars.gAranGraphics.DrawMultiPolygon(_CurrImASLeg.Protection, -1, eFillStyle.sfsForwardDiagonal);
					//Application.DoEvents();

					RFLeg nextNext;
					if (_ImASLegs.Count == 1)
						nextNext = _CurrFASLeg;
					else
						nextNext = _ImASLegs[_ImASLegs.Count - 2];

					next.Protection.Clear();
					next.Nominal.Clear();

					if (next.legType == LegType.FixedRadius)
						CreateImASRFLeg(nextNext, ref next, _CurrImASLeg, true);
					else if (next.legType == LegType.FlyBy)
						CreateIImASFlyByLeg(nextNext, ref next, _CurrImASLeg, true);
					else
						CreateImASStraightLeg(nextNext, ref next, _CurrImASLeg, true);

					GlobalVars.gAranGraphics.SafeDeleteGraphic(next.FixElem);
					GlobalVars.gAranGraphics.SafeDeleteGraphic(next.NominalElem);
					GlobalVars.gAranGraphics.SafeDeleteGraphic(next.ProtectionElem);

					next.ProtectionElem = GlobalVars.gAranGraphics.DrawMultiPolygon(next.Protection, eFillStyle.sfsHollow, ARANFunctions.RGB(0, 0, 255));
					next.NominalElem = GlobalVars.gAranGraphics.DrawMultiLineString(next.Nominal, 2, 255);
					next.FixElem = GlobalVars.gAranGraphics.DrawPointWithText(next.StartPrj, "WPT", ARANFunctions.RGB(0, 127, 255));

					_ImASLegs[_ImASLegs.Count - 1] = next;
				}

				textBox0207.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_CurrImASLeg.Nominal.Length - _CurrImASLeg.RNPvalue).ToString();
			}
			else
			{
				_CurrImASLeg.legType = LegType.Straight;

				_CurrImASLeg.RollOutPrj = ARANFunctions.LocalToPrj(_ptNextprj, _CurrImASLeg.RollOutDir, -_CurrImASLeg.DistToNext);
				_CurrImASLeg.RollOutGeo = GlobalVars.pspatialReferenceOperation.ToGeo<Point>(_CurrImASLeg.RollOutPrj);
				_CurrImASLeg.StartPrj = new Point(_CurrImASLeg.RollOutPrj);

				_CurrImASLeg.StartDir = _CurrImASLeg.RollOutDir;

				//_CurrImASLeg.EndAltitude = FAPDist2hFAP(_CurrFASLeg.DistToNext, _hNext);
				//textBox0203.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_CurrFASLeg.EndAltitude).ToString();

				_CurrImASLeg.RollOutPrj.Z = _CurrImASLeg.RollOutAltitude;
				_CurrImASLeg.RollOutGeo.Z = _CurrImASLeg.RollOutAltitude;

				if (_ImASLegs.Count == 0)
					CreateImASStraightLeg(_CurrFASLeg, ref _CurrImASLeg);
				else
				{
					RFLeg next = _ImASLegs[_ImASLegs.Count - 1];
					CreateImASStraightLeg(next, ref _CurrImASLeg);

					//GlobalVars.gAranGraphics.DrawMultiPolygon(_CurrImASLeg.Protection, -1, eFillStyle.sfsBackwardDiagonal);
					//Application.DoEvents();

					RFLeg nextNext;
					if (_ImASLegs.Count == 1)
						nextNext = _CurrFASLeg;
					else
						nextNext = _ImASLegs[_ImASLegs.Count - 2];

					next.Protection.Clear();
					next.Nominal.Clear();

					if (next.legType == LegType.FixedRadius)
						CreateImASRFLeg(nextNext, ref next, _CurrImASLeg, true);
					else if (next.legType == LegType.FlyBy)
						CreateIImASFlyByLeg(nextNext, ref next, _CurrImASLeg, true);
					else
						CreateImASStraightLeg(nextNext, ref next, _CurrImASLeg, true);

					GlobalVars.gAranGraphics.SafeDeleteGraphic(next.FixElem);
					GlobalVars.gAranGraphics.SafeDeleteGraphic(next.NominalElem);
					GlobalVars.gAranGraphics.SafeDeleteGraphic(next.ProtectionElem);
					//Application.DoEvents();

					next.ProtectionElem = -1;
					next.NominalElem = -1;
					next.FixElem = -1;

					if (next.legType != LegType.FlyBy)
					{
						next.ProtectionElem = GlobalVars.gAranGraphics.DrawMultiPolygon(next.Protection, eFillStyle.sfsHollow, ARANFunctions.RGB(0, 0, 255));
						next.NominalElem = GlobalVars.gAranGraphics.DrawMultiLineString(next.Nominal, 2, 255);
						next.FixElem = GlobalVars.gAranGraphics.DrawPointWithText(next.StartPrj, "WPT", ARANFunctions.RGB(0, 127, 255));
					}
					//Application.DoEvents();

					_ImASLegs[_ImASLegs.Count - 1] = next;
				}
			}

			_CurrImASLeg.ProtectionElem = GlobalVars.gAranGraphics.DrawMultiPolygon(_CurrImASLeg.Protection, eFillStyle.sfsHollow, ARANFunctions.RGB(0, 0, 255));
			_CurrImASLeg.NominalElem = GlobalVars.gAranGraphics.DrawMultiLineString(_CurrImASLeg.Nominal, 2, 255);
			if (_CurrImASLeg.legType != LegType.FlyBy)
				_CurrImASLeg.FixElem = GlobalVars.gAranGraphics.DrawPointWithText(_CurrImASLeg.StartPrj, "WPT", ARANFunctions.RGB(0, 127, 255));

			/*********************************************************************************************/

			Functions.GetObstaclesByPolygonWithDecomposition(GlobalVars.ObstacleList, out CurrlegObsatclesList, _CurrImASLeg.Protection);

			int n = CurrlegObsatclesList.Parts.Length;
			for (int i = 0; i < n; i++)
			{
				CurrlegObsatclesList.Parts[i].MOC = _CurrImASLeg.MOC;
				CurrlegObsatclesList.Parts[i].ReqH = CurrlegObsatclesList.Parts[i].Height + _CurrImASLeg.MOC;
				CurrlegObsatclesList.Parts[i].Elev = CurrlegObsatclesList.Parts[i].Height + ptTHRprj.Z;
			}

			reportForm.FillPage07(CurrlegObsatclesList);

			/************************************************************************************************************/
		}

		private void button0201_Click(object sender, EventArgs e)
		{
			RFLeg nextLeg = _CurrImASLeg;
			_ptNextprj = nextLeg.StartPrj;
			_ptNextprj.Z = nextLeg.StartAltitude;
			_hNext = _ptNextprj.Z;

			_ImASLegs.Add(_CurrImASLeg);

			_CurrImASLeg = new RFLeg();
			_CurrImASLeg.Nominal = new MultiLineString();
			_CurrImASLeg.Protection = new MultiPolygon();

			_CurrImASLeg.DescentGR = _TanGPA; //nextLeg.DescentGR;
			_CurrImASLeg.RNPvalue = nextLeg.RNPvalue;
			_CurrImASLeg.BankAngle = nextLeg.BankAngle;
			_CurrImASLeg.DistToNext = nextLeg.DistToNext;
			_CurrImASLeg.Radius = nextLeg.Radius;
			_CurrImASLeg.TurnDir = nextLeg.TurnDir;
			_CurrImASLeg.IAS = nextLeg.IAS;
			_CurrImASLeg.TAS = nextLeg.TAS;

			_CurrImASLeg.RollOutDir = nextLeg.StartDir;
			_CurrImASLeg.RollOutAltitude = nextLeg.StartAltitude;

			label0203.Enabled = nextLeg.legType != LegType.FlyBy;
			label0204.Enabled = nextLeg.legType != LegType.FlyBy;
			textBox0202.Enabled = nextLeg.legType != LegType.FlyBy;

			radioButton0201.Enabled = nextLeg.legType != LegType.FlyBy;
			radioButton0202.Enabled = nextLeg.legType == LegType.Straight;

			LegType lt = nextLeg.legType;
			if (radioButton0202.Checked && !radioButton0202.Enabled)
			{
				radioButton0202.Checked = false;
				if (radioButton0201.Enabled)
					lt = LegType.FixedRadius;
				else
					lt = LegType.Straight;
			}

			if (radioButton0201.Checked && !radioButton0201.Enabled)
			{
				radioButton0201.Checked = false;
				lt = LegType.Straight;
			}

			_CurrImASLeg.legType = lt;

			if (lt == LegType.FixedRadius)
				radioButton0201.Checked = true;
			else if (lt == LegType.FlyBy)
				radioButton0202.Checked = true;
			else if (lt == LegType.Straight)
				radioButton0203.Checked = true;

			textBox0206.Tag = null;
			textBox0206_Validating(textBox0206, null);
			comboBox0201_SelectedIndexChanged(comboBox0201, null);
		}

		private void button0202_Click(object sender, EventArgs e)
		{

		}

		private void radioButton0201_CheckedChanged(object sender, EventArgs e)
		{
			if (!((RadioButton)sender).Checked)
				return;

			_CurrImASLeg.legType = (LegType)((RadioButton)sender).Tag;

			comboBox0201.Enabled = _CurrImASLeg.legType != LegType.Straight;

			textBox0204.Enabled = _CurrImASLeg.legType != LegType.Straight;
			textBox0205.Enabled = _CurrImASLeg.legType != LegType.Straight;
			textBox0206.Enabled = _CurrImASLeg.legType != LegType.Straight;

			label0207.Enabled = _CurrImASLeg.legType != LegType.Straight;
			label0208.Enabled = _CurrImASLeg.legType != LegType.Straight;
			label0209.Enabled = _CurrImASLeg.legType != LegType.Straight;
			label0210.Enabled = _CurrImASLeg.legType != LegType.Straight;
			label0211.Enabled = _CurrImASLeg.legType != LegType.Straight;
			label0212.Enabled = _CurrImASLeg.legType != LegType.Straight;
			label0213.Enabled = _CurrImASLeg.legType != LegType.Straight;

			textBox0207.ReadOnly = _CurrImASLeg.legType != LegType.Straight;
			//label0214.Enabled = _CurrImASLeg.legType == LegType.Straight;
			//label0215.Enabled = _CurrImASLeg.legType == LegType.Straight;

			if (_CurrImASLeg.legType == LegType.FlyBy)
			{
				textBox0206.Tag = null;
				textBox0206_Validating(textBox0206, null);

				textBox0204.Text = "18";
				textBox0204_Validating(textBox0204, null);

				//double turnangle = (_CurrImASLeg.ExitDir - _CurrImASLeg.EntryDir) * (int)_CurrImASLeg.TurnDir;
				//if (turnangle > ARANMath.C_PI_2)
				//{
				//	_CurrImASLeg.EntryDir = _CurrImASLeg.ExitDir + ARANMath.C_PI_2 * (int)_CurrImASLeg.TurnDir;
				//}
			}

			CreateImASLeg();
		}

		private void comboBox0201_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox0201.SelectedIndex < 0)
			{
				comboBox0201.SelectedIndex = 0;
				return;
			}

			_ImASturndir = 1 - 2 * comboBox0201.SelectedIndex;
			_CurrImASLeg.TurnDir = (TurnDirection)_ImASturndir;

			textBox0206.Tag = null;
			textBox0206_Validating(textBox0206, null);
		}

		private void textBox0201_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox0201_Validating(textBox0201, null);
			else
				Functions.TextBoxFloat(ref eventChar, textBox0201.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox0201_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox0201.Text, out fTmp))
				return;

			if (textBox0201.Tag != null && textBox0201.Tag.ToString() == textBox0201.Text)
				return;
			fTmp = _CurrImASLeg.MOC = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);

			if (_CurrImASLeg.MOC < 150.0)
				_CurrImASLeg.MOC = 150.0;

			if (_CurrImASLeg.MOC > 300.0)
				_CurrImASLeg.MOC = 300.0;

			if (fTmp != _CurrImASLeg.MOC)
				textBox0201.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_CurrImASLeg.MOC).ToString();

			textBox0201.Tag = textBox0201.Text;
		}

		private void textBox0202_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox0202_Validating(textBox0202, null);
			else
				Functions.TextBoxFloat(ref eventChar, textBox0202.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox0202_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;
			if (textBox0202.ReadOnly)
				return;

			if (!double.TryParse(textBox0202.Text, out fTmp))
				return;

			if (textBox0202.Tag != null && textBox0202.Tag.ToString() == textBox0202.Text)
				return;

			double RNPval = fTmp;

			if (RNPval < 0.1)
				RNPval = 0.1;

			if (RNPval > 1.0)
				RNPval = 1.0;

			if (fTmp != RNPval)
				textBox0202.Text = RNPval.ToString();
			textBox0202.Tag = textBox0202.Text;

			_CurrImASLeg.RNPvalue = GlobalVars.unitConverter.DistanceFromNM(RNPval);
			if (_CurrImASLeg.DistToNext < _CurrImASLeg.RNPvalue)
			{
				_CurrImASLeg.DistToNext = _CurrImASLeg.RNPvalue;
				textBox0207.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_CurrImASLeg.DistToNext).ToString();
				textBox0207.Tag = textBox0207.Text;
			}

			CreateImASLeg();
		}

		private void textBox0203_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox0203_Validating(textBox0203, null);
			else
				Functions.TextBoxFloat(ref eventChar, textBox0203.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox0203_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox0203.Text, out fTmp))
				return;

			if (textBox0203.Tag != null && textBox0203.Tag.ToString() == textBox0203.Text)
				return;

			fTmp = _CurrImASLeg.StartAltitude = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);

			double maxAlt = _CurrImASLeg.RollOutAltitude + _CurrImASLeg.Nominal.Length * _CurrImASLeg.DescentGR;

			if (_CurrImASLeg.StartAltitude < _hNext)
				_CurrImASLeg.StartAltitude = _hNext;

			if (_CurrImASLeg.StartAltitude > maxAlt)
				_CurrImASLeg.StartAltitude = maxAlt;

			if (fTmp != _CurrImASLeg.StartAltitude)
				textBox0203.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_CurrImASLeg.StartAltitude).ToString();
			textBox0203.Tag = textBox0203.Text;

			_CurrImASLeg.StartPrj.Z = _CurrImASLeg.StartAltitude;
			//_CurrImASLeg.StartGeo.Z = _CurrImASLeg.StartAltitude;

			_ImASminRadius = CalcImASRadius(_CurrImASLeg.TAS, _CurrImASLeg.StartAltitude - GlobalVars.CurrADHP.Elev, 20.0);
			if (_ImASminRadius <= 2.0 * _FASRNPval)
				_ImASminRadius = Math.Ceiling(2.01 * _FASRNPval);

			_ImASmaxRadius = CalcRadius(_CurrImASLeg.StartAltitude - GlobalVars.CurrADHP.Elev, 1.0);
			CheckImASRadius();
			CreateImASLeg();
		}

		private void textBox0204_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox0204_Validating(textBox0204, null);
			else
				Functions.TextBoxFloat(ref eventChar, textBox0204.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox0204_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox0204.Text, out fTmp))
				return;

			if (textBox0204.Tag != null && textBox0204.Tag.ToString() == textBox0204.Text)
				return;

			_CurrImASLeg.BankAngle = fTmp;
			CheckImASBank();
			textBox0204.Tag = textBox0204.Text;
		}

		private void textBox0205_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox0205_Validating(textBox0205, null);
			else
				Functions.TextBoxFloat(ref eventChar, textBox0205.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox0205_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox0205.Text, out fTmp))
				return;

			if (textBox0205.Tag != null && textBox0205.Tag.ToString() == textBox0205.Text)
				return;

			_CurrImASLeg.Radius = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);
			CheckImASRadius();
			textBox0205.Tag = textBox0205.Text;

			CreateImASLeg();
		}

		private void textBox0206_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox0206_Validating(textBox0206, null);
			else
				Functions.TextBoxFloat(ref eventChar, textBox0206.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox0206_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox0206.Text, out fTmp))
				return;

			if (textBox0206.Tag != null && textBox0206.Tag.ToString() == textBox0206.Text)
				return;

			double InCourse = NativeMethods.Modulus(fTmp);
			double ExitCourse = ARANFunctions.DirToAzimuth(_ptNextprj, _CurrImASLeg.RollOutDir, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			double dAzt = NativeMethods.Modulus(_ImASturndir * (fTmp - ExitCourse));

			double dAngle;
			if (radioButton0201.Checked)
				dAngle = 240.0;
			else
			{
				if (_ImASLegs.Count > 0)
					dAngle = 90.0;
				else
					dAngle = 15.0;
			}

			if (dAzt > dAngle)
				fTmp = NativeMethods.Modulus(ExitCourse + _ImASturndir * dAngle);

			if (fTmp != InCourse)
				textBox0206.Text = fTmp.ToString("0.00");

			Point EndGeo = GlobalVars.pspatialReferenceOperation.ToGeo<Point>(_ptNextprj);

			//GlobalVars.gAranGraphics.DrawPointWithText(_ptNextprj, -1, "Next point");
			//GlobalVars.gAranGraphics.DrawPointWithText(_CurrImASLeg.EndPrj, -1, "End point");
			//Application.DoEvents();


			_CurrImASLeg.StartDir = ARANFunctions.AztToDirection(EndGeo, fTmp, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);
			textBox0206.Tag = textBox0206.Text;

			CreateImASLeg();
		}

		private void textBox0207_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox0207_Validating(textBox0207, null);
			else
				Functions.TextBoxFloat(ref eventChar, textBox0207.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox0207_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox0207.Text, out fTmp))
				return;

			if (textBox0207.Tag != null && textBox0207.Tag.ToString() == textBox0207.Text)
				return;

			fTmp = _CurrImASLeg.DistToNext = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);
			if (_CurrImASLeg.DistToNext < _CurrImASLeg.RNPvalue)
				_CurrImASLeg.DistToNext = _CurrImASLeg.RNPvalue;

			if (_CurrImASLeg.DistToNext > 15.0 * 1852.0)
				_CurrImASLeg.DistToNext = 15.0 * 1852.0;

			if (fTmp != _CurrImASLeg.DistToNext)
				textBox0207.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_CurrImASLeg.DistToNext).ToString();
			textBox0207.Tag = textBox0207.Text;

			//_CurrImASLeg.EndPrj = ARANFunctions.LocalToPrj(_ptNextprj, _CurrImASLeg.ExitDir, -_CurrImASLeg.DistToNext);
			//_CurrImASLeg.EndGeo = GlobalVars.pspatialReferenceOperation.ToGeo<Point>(_CurrImASLeg.EndPrj);
			//_CurrImASLeg.StartPrj = new Point(_CurrImASLeg.EndPrj);

			////_CurrImASLeg.EndAltitude = FAPDist2hFAP(_CurrFASLeg.DistToNext, _hNext);
			////textBox0203.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_CurrFASLeg.EndAltitude).ToString();

			//_CurrImASLeg.EndPrj.Z = _CurrImASLeg.EndAltitude;
			//_CurrImASLeg.EndGeo.Z = _CurrImASLeg.EndAltitude;

			CreateImASLeg();
		}

		private void textBox0208_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox0208_Validating(textBox0208, null);
			else
				Functions.TextBoxFloat(ref eventChar, textBox0208.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;

		}

		private void textBox0208_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox0208.Text, out fTmp))
				return;

			if (textBox0208.Tag != null && textBox0208.Tag.ToString() == textBox0208.Text)
				return;

			fTmp = _CurrImASLeg.IAS = GlobalVars.unitConverter.SpeedToInternalUnits(fTmp);

			double iasMin = GlobalVars.constants.AircraftCategory[aircraftCategoryData.ViafMin].Value[_Category];
			double iasMax = GlobalVars.constants.AircraftCategory[aircraftCategoryData.ViafMax].Value[_Category];

			if (_CurrImASLeg.IAS < iasMin)
				_CurrImASLeg.IAS = iasMin;

			if (_CurrImASLeg.IAS > iasMax)
				_CurrImASLeg.IAS = iasMax;

			if (fTmp != _CurrImASLeg.IAS)
				textBox0208.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(_CurrImASLeg.IAS).ToString();
			textBox0208.Tag = textBox0208.Text;

			_CurrImASLeg.TAS = 3.6 * ARANMath.IASToTAS(_CurrImASLeg.IAS, GlobalVars.CurrADHP.Elev + _hNext, 0.0);

			_ImASminRadius = CalcImASRadius(_CurrImASLeg.TAS, _CurrImASLeg.StartAltitude - GlobalVars.CurrADHP.Elev, 20.0);
			if (_ImASminRadius <= 2.0 * _FASRNPval)
				_ImASminRadius = Math.Ceiling(2.01 * _FASRNPval);

			_ImASmaxRadius = CalcRadius(_CurrImASLeg.StartAltitude - GlobalVars.CurrADHP.Elev, 1.0);

			CheckImASRadius();
			CreateImASLeg();
		}
		#endregion

		#region Done

		private bool SaveProcedure()
		{
			ProcedureTransition pTransition;
			LandingTakeoffAreaCollection pLandingTakeoffAreaCollection;
			AircraftCharacteristic IsLimitedTo;
			FeatureRef featureRef;
			FeatureRefObject featureRefObject;
			GuidanceService pGuidanceServiceChose = new GuidanceService();
			SegmentLeg pSegmentLeg;
			ProcedureTransitionLeg ptl;

			string sProcName;

			DBModule.pObjectDir.ClearAllFeatures();

			InstrumentApproachProcedure pProcedure = DBModule.pObjectDir.CreateFeature<InstrumentApproachProcedure>();

			// Procedure =================================================================================================

			pLandingTakeoffAreaCollection = new LandingTakeoffAreaCollection();
			pLandingTakeoffAreaCollection.Runway.Add(_SelectedRWY.GetFeatureRefObject());

			pProcedure.RNAV = true;
			pProcedure.FlightChecked = false;
			pProcedure.CodingStandard = Aran.Aim.Enums.CodeProcedureCodingStandard.PANS_OPS;
			pProcedure.DesignCriteria = Aran.Aim.Enums.CodeDesignStandard.PANS_OPS;
			pProcedure.Landing = pLandingTakeoffAreaCollection;

			sProcName = "RNP AR APCH " + _SelectedRWY.Name;
			pProcedure.Name = sProcName;

			//pProcedure.CommunicationFailureDescription
			//pProcedure.Description =
			//pProcedure.ID
			//pProcedure.Note =
			//pProcedure.ProtectsSafeAltitudeAreaId = `

			IsLimitedTo = new AircraftCharacteristic();

			switch (_Category)
			{
				case 0:
					IsLimitedTo.AircraftLandingCategory = Aran.Aim.Enums.CodeAircraftCategory.A;
					break;
				case 1:

					IsLimitedTo.AircraftLandingCategory = Aran.Aim.Enums.CodeAircraftCategory.B;
					break;
				case 2:

					IsLimitedTo.AircraftLandingCategory = Aran.Aim.Enums.CodeAircraftCategory.C;
					break;
				case 3:
					IsLimitedTo.AircraftLandingCategory = Aran.Aim.Enums.CodeAircraftCategory.D;
					break;
				case 4:
					IsLimitedTo.AircraftLandingCategory = Aran.Aim.Enums.CodeAircraftCategory.E;
					break;
			}

			pProcedure.AircraftCharacteristic.Add(IsLimitedTo);

			featureRefObject = new FeatureRefObject();
			featureRef = new FeatureRef();
			featureRef.Identifier = GlobalVars.CurrADHP.pAirportHeliport.Identifier;
			featureRefObject.Feature = featureRef;
			pProcedure.AirportHeliport.Add(featureRefObject);

			TerminalSegmentPoint pEndPoint = null;

			//Transition 1 =========================================================================================

			pTransition = new ProcedureTransition();

			//	pTransition.Description =
			//	pTransition.ID =
			//	pTransition.Procedure =

			pTransition.Type = Aran.Aim.Enums.CodeProcedurePhase.APPROACH;

			//Initial Legs ===============================================================================================
			for (int NO_SEQ = 1, i = _ImASLegs.Count - 1; i >= 1; NO_SEQ++, i--)
			{
				pSegmentLeg = InitialApproachLeg(i, pProcedure, IsLimitedTo, ref pEndPoint);

				ptl = new ProcedureTransitionLeg();
				ptl.SeqNumberARINC = (uint)NO_SEQ;
				ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef<AbstractSegmentLegRef>();
				pTransition.TransitionLeg.Add(ptl);
			}

			pSegmentLeg = IntermediateApproachLeg(0, pProcedure, IsLimitedTo, ref pEndPoint);
			ptl = new ProcedureTransitionLeg();
			ptl.SeqNumberARINC = (uint)_ImASLegs.Count;
			ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef<AbstractSegmentLegRef>();
			pTransition.TransitionLeg.Add(ptl);

			pProcedure.FlightTransition.Add(pTransition);

			//Transition 2 =========================================================================================

			pTransition = new ProcedureTransition();

			//	pTransition.Description =
			//	pTransition.ID =
			//	pTransition.Procedure =

			pTransition.Type = Aran.Aim.Enums.CodeProcedurePhase.FINAL;

			//Final Legs ===============================================================================================

			for (int NO_SEQ = 1, i = _FASLegs.Count - 1; i >= 0; NO_SEQ++, i--)
			{
				pSegmentLeg = FinalApproachLeg(i, pProcedure, IsLimitedTo, ref pEndPoint);

				ptl = new ProcedureTransitionLeg();
				ptl.SeqNumberARINC = (uint)NO_SEQ;
				ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef<AbstractSegmentLegRef>();
				pTransition.TransitionLeg.Add(ptl);
			}

			pProcedure.FlightTransition.Add(pTransition);

			return true;
		}

		private SegmentLeg InitialApproachLeg(int i, Procedure pProcedure, AircraftCharacteristic IsLimitedTo, ref TerminalSegmentPoint pEndPoint)
		{
			AngleIndication pAngleIndication;

			SegmentPoint pSegmentPoint;
			Feature pFixDesignatedPoint;
			SignificantPoint pFIXSignPt;

			ValDistance pDistance;
			ValDistanceVertical pDistanceVertical;
			ValSpeed pSpeed;

			//int i = (int)NO_SEQ - 1;

			RFLeg leg = _ImASLegs[i];

			InitialLeg pInitialLeg = DBModule.pObjectDir.CreateFeature<InitialLeg>();

			pInitialLeg.AircraftCategory.Add(IsLimitedTo);
			//pArrivalLeg.Approach = pProcedure.GetFeatureRef();

			SegmentLeg pSegmentLeg = pInitialLeg;

			pSegmentLeg.AltitudeInterpretation = Aran.Aim.Enums.CodeAltitudeUse.BETWEEN;
			pSegmentLeg.UpperLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL;
			pSegmentLeg.LowerLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL;
			pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT;
			pSegmentLeg.CourseType = Aran.Aim.Enums.CodeCourse.TRUE_TRACK;
			//pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT;
			pSegmentLeg.SpeedReference = Aran.Aim.Enums.CodeSpeedReference.IAS;

			pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.CF;

			if (leg.TurnDir == TurnDirection.CW)
				pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.RIGHT;
			else if (leg.TurnDir == TurnDirection.CCW)
				pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.LEFT;

			pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO;

			double Course = GlobalVars.pspatialReferenceOperation.DirToAztPrj(leg.RollOutPrj, leg.RollOutDir);
			pSegmentLeg.Course = Course;

			//====================================================================================================

			pDistanceVertical = new ValDistanceVertical(GlobalVars.unitConverter.HeightToDisplayUnits(leg.RollOutAltitude + ptTHRprj.Z, eRoundMode.NEAREST), mUomVDistance);
			pSegmentLeg.LowerLimitAltitude = pDistanceVertical;

			pDistanceVertical = new ValDistanceVertical(GlobalVars.unitConverter.HeightToDisplayUnits(leg.StartAltitude + ptTHRprj.Z, eRoundMode.NEAREST), mUomVDistance);
			pSegmentLeg.UpperLimitAltitude = pDistanceVertical;

			pDistance = new ValDistance(GlobalVars.unitConverter.DistanceToDisplayUnits(leg.Nominal.Length, eRoundMode.NEAREST), mUomHDistance);
			pSegmentLeg.Length = pDistance;

			pSegmentLeg.BankAngle = leg.BankAngle;

			pSegmentLeg.VerticalAngle = -ARANMath.RadToDeg(System.Math.Atan(leg.DescentGR));

			pSpeed = new ValSpeed(GlobalVars.unitConverter.SpeedToDisplayUnits(_IAS, eRoundMode.SPECIAL_NEAREST), mUomSpeed);
			pSegmentLeg.SpeedLimit = pSpeed;

			// End Point ========================
			if (pEndPoint == null)
			{
				pEndPoint = new TerminalSegmentPoint();
				//	pStartPoint.IndicatorFACF =      ??????????
				//	pStartPoint.LeadDME =            ??????????
				//	pStartPoint.LeadRadial =         ??????????
				pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.FAF;

				//==

				pSegmentPoint = pEndPoint;
				pSegmentPoint.FlyOver = false;
				pSegmentPoint.RadarGuidance = false;

				pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT;
				pSegmentPoint.Waypoint = false;

				// ========================
				if (i == _ImASLegs.Count - 1)
					pFixDesignatedPoint = DBModule.CreateDesignatedPoint(leg.StartPrj, "IAF" + (i).ToString("00"), leg.StartDir);
				else
					pFixDesignatedPoint = DBModule.CreateDesignatedPoint(leg.StartPrj, "TP" + (i).ToString("00"), leg.StartDir);

				pFIXSignPt = new SignificantPoint();

				if (pFixDesignatedPoint is DesignatedPoint)
					pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef();
				else if (pFixDesignatedPoint is Navaid)
					pFIXSignPt.NavaidSystem = pFixDesignatedPoint.GetFeatureRef();
				pSegmentPoint.PointChoice = pFIXSignPt;
			}
			//========
			pSegmentLeg.StartPoint = pEndPoint;

			// End Of End Point ========================

			// Angle ========================
			double Angle = NativeMethods.Modulus(Course - GlobalVars.CurrADHP.MagVar, 360.0);

			pAngleIndication = DBModule.CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pEndPoint.PointChoice);
			pAngleIndication.TrueAngle = Course;
			pAngleIndication.IndicationDirection = CodeDirectionReference.FROM;
			pSegmentLeg.Angle = pAngleIndication.GetFeatureRef();

			//=======================================================================================

			// Start Point ========================
			pEndPoint = new TerminalSegmentPoint();
			//pEndPoint.IndicatorFACF =      ??????????
			//pEndPoint.LeadDME =            ??????????
			//pEndPoint.LeadRadial =         ??????????
			pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.FROP;

			pSegmentPoint = pEndPoint;

			pSegmentPoint.FlyOver = true;
			pSegmentPoint.RadarGuidance = false;
			pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT;
			pSegmentPoint.Waypoint = false;

			// ========================
			if (i + 1 == _ImASLegs.Count - 1)
				pFixDesignatedPoint = DBModule.CreateDesignatedPoint(leg.StartPrj, "IAF" + (i + 1).ToString("00"), leg.StartDir);
			else
				pFixDesignatedPoint = DBModule.CreateDesignatedPoint(leg.StartPrj, "TP" + (i + 1).ToString("00"), leg.StartDir);

			pFIXSignPt = new SignificantPoint();

			if (pFixDesignatedPoint is DesignatedPoint)
				pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef();
			else if (pFixDesignatedPoint is Navaid)
				pFIXSignPt.NavaidSystem = pFixDesignatedPoint.GetFeatureRef();
			pSegmentPoint.PointChoice = pFIXSignPt;

			//////////////////////////

			//pSegmentLeg.StartPoint = pEndPoint;
			pSegmentLeg.EndPoint = pEndPoint;

			// End Of EndPoint =====================================================================================

			// Trajectory ===========================================
			Curve pCurve = new Curve();
			MultiLineString ls = GlobalVars.pspatialReferenceOperation.ToGeo<MultiLineString>(leg.Nominal);
			pCurve.Geo.Add(ls);
			pSegmentLeg.Trajectory = pCurve;

			// Trajectory =====================================================
			// protected Area =================================================

			Surface pSurface = new Surface();
			MultiPolygon pl = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(leg.Protection);
			pSurface.Geo.Add(pl);

			ObstacleAssessmentArea pPrimProtectedArea = new ObstacleAssessmentArea();
			pPrimProtectedArea.Surface = pSurface;
			pPrimProtectedArea.SectionNumber = 0;
			pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY;
			pSegmentLeg.DesignSurface.Add(pPrimProtectedArea);

			//  END ======================================================

			// Protection Area Obstructions list ==================================================
			ObstacleContainer ostacles = leg.ObstaclesList;

			Functions.Sort(ref ostacles, 2);
			int saveCount = Math.Min(ostacles.Obstacles.Length, 15);

			for (i = 0; i < ostacles.Parts.Length; i++)
			{
				if (saveCount == 0)
					break;

				double RequiredClearance = 0.0;

				Obstacle tmpObstacle = ostacles.Obstacles[ostacles.Parts[i].Owner];
				if (tmpObstacle.NIx > -1)
					continue;

				tmpObstacle.NIx = 0;

				//MinimumAltitude = Math.Max(MinimumAltitude, ostacles.Parts[ostacles.Obstacles[i].Parts[j]].ReqH)
				RequiredClearance = ostacles.Parts[i].MOC;

				Obstruction obs = new Obstruction();
				obs.VerticalStructureObstruction = new FeatureRef(ostacles.Obstacles[i].Identifier);

				//ReqH
				double MinimumAltitude = ostacles.Obstacles[i].Height + RequiredClearance + ptTHRprj.Z;

				pDistanceVertical = new ValDistanceVertical();
				pDistanceVertical.Uom = mUomVDistance;
				pDistanceVertical.Value = GlobalVars.unitConverter.HeightToDisplayUnits(MinimumAltitude);
				obs.MinimumAltitude = pDistanceVertical;

				// MOC
				pDistance = new ValDistance();
				pDistance.Uom = UomDistance.M;
				pDistance.Value = RequiredClearance;
				obs.RequiredClearance = pDistance;
				pPrimProtectedArea.SignificantObstacle.Add(obs);

				pDistanceVertical = null;
				pDistance = null;
				saveCount -= 1;
			}

			pSegmentLeg.DesignSurface.Add(pPrimProtectedArea);

			//=================================================================================================================
			ApproachCondition pCondition = new ApproachCondition();
			pCondition.AircraftCategory.Add(IsLimitedTo);

			Minima pMinimumSet = new Minima();

			pMinimumSet.AltitudeCode = CodeMinimumAltitude.OCA;
			pMinimumSet.AltitudeReference = CodeVerticalReference.MSL;

			pMinimumSet.HeightCode = CodeMinimumHeight.OCH;
			pMinimumSet.HeightReference = CodeHeightReference.HAT;

			//If CheckBox0301.Checked Then
			//	pMinimumSet.Altitude = New ValDistanceVertical(ConvertHeight(fTA_OCH + FicTHRprj.Z, eRoundMode.NEAREST), mUomVDistance)
			//	pMinimumSet.Height = New ValDistanceVertical(ConvertHeight(fTA_OCH, eRoundMode.NEAREST), mUomVDistance)
			//Else
			//	pMinimumSet.Altitude = New ValDistanceVertical(ConvertHeight(Math.Max(fMisAprOCH, _CurrFAPOCH) + FicTHRprj.Z, eRoundMode.NEAREST), mUomVDistance)
			//	pMinimumSet.Height = New ValDistanceVertical(ConvertHeight(Math.Max(fMisAprOCH, _CurrFAPOCH), eRoundMode.NEAREST), mUomVDistance)
			//End If
			//pCondition.MinimumSet = pMinimumSet;
			//pFinalLeg.Condition.Add(pCondition);

			// protected Area =================================================

			return pInitialLeg;
		}

		private SegmentLeg IntermediateApproachLeg(int i, Procedure pProcedure, AircraftCharacteristic IsLimitedTo, ref TerminalSegmentPoint pEndPoint)
		{
			AngleIndication pAngleIndication;

			SegmentPoint pSegmentPoint;
			Feature pFixDesignatedPoint;
			SignificantPoint pFIXSignPt;

			ValDistance pDistance;
			ValDistanceVertical pDistanceVertical;
			ValSpeed pSpeed;

			//int i = (int)NO_SEQ - 1;

			RFLeg leg = _ImASLegs[i];

			IntermediateLeg pIntermediateLeg = DBModule.pObjectDir.CreateFeature<IntermediateLeg>();

			pIntermediateLeg.AircraftCategory.Add(IsLimitedTo);
			//pArrivalLeg.Approach = pProcedure.GetFeatureRef();

			SegmentLeg pSegmentLeg = pIntermediateLeg;

			pSegmentLeg.AltitudeInterpretation = Aran.Aim.Enums.CodeAltitudeUse.BETWEEN;
			pSegmentLeg.UpperLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL;
			pSegmentLeg.LowerLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL;
			pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT;
			pSegmentLeg.CourseType = Aran.Aim.Enums.CodeCourse.TRUE_TRACK;
			//pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT;
			pSegmentLeg.SpeedReference = Aran.Aim.Enums.CodeSpeedReference.IAS;

			pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.CF;

			if (leg.TurnDir == TurnDirection.CW)
				pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.RIGHT;
			else if (leg.TurnDir == TurnDirection.CCW)
				pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.LEFT;

			pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO;

			double Course = GlobalVars.pspatialReferenceOperation.DirToAztPrj(leg.RollOutPrj, leg.RollOutDir);
			pSegmentLeg.Course = Course;

			//====================================================================================================

			pDistanceVertical = new ValDistanceVertical(GlobalVars.unitConverter.HeightToDisplayUnits(leg.RollOutAltitude + ptTHRprj.Z, eRoundMode.NEAREST), mUomVDistance);
			pSegmentLeg.LowerLimitAltitude = pDistanceVertical;

			pDistanceVertical = new ValDistanceVertical(GlobalVars.unitConverter.HeightToDisplayUnits(leg.StartAltitude + ptTHRprj.Z, eRoundMode.NEAREST), mUomVDistance);
			pSegmentLeg.UpperLimitAltitude = pDistanceVertical;

			pDistance = new ValDistance(GlobalVars.unitConverter.DistanceToDisplayUnits(leg.Nominal.Length, eRoundMode.NEAREST), mUomHDistance);
			pSegmentLeg.Length = pDistance;

			pSegmentLeg.BankAngle = leg.BankAngle;

			pSegmentLeg.VerticalAngle = -ARANMath.RadToDeg(System.Math.Atan(leg.DescentGR));

			pSpeed = new ValSpeed(GlobalVars.unitConverter.SpeedToDisplayUnits(_IAS, eRoundMode.SPECIAL_NEAREST), mUomSpeed);
			pSegmentLeg.SpeedLimit = pSpeed;

			// End Point ========================
			if (pEndPoint == null)
			{
				pEndPoint = new TerminalSegmentPoint();
				//	pStartPoint.IndicatorFACF =      ??????????
				//	pStartPoint.LeadDME =            ??????????
				//	pStartPoint.LeadRadial =         ??????????
				pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.FAF;

				//==

				pSegmentPoint = pEndPoint;
				pSegmentPoint.FlyOver = false;
				pSegmentPoint.RadarGuidance = false;

				pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT;
				pSegmentPoint.Waypoint = false;

				// ========================
				if (i == _ImASLegs.Count - 1)
					pFixDesignatedPoint = DBModule.CreateDesignatedPoint(leg.StartPrj, "IAF" + (i).ToString("00"), leg.StartDir);
				else
					pFixDesignatedPoint = DBModule.CreateDesignatedPoint(leg.StartPrj, "TP" + (i).ToString("00"), leg.StartDir);

				pFIXSignPt = new SignificantPoint();

				if (pFixDesignatedPoint is DesignatedPoint)
					pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef();
				else if (pFixDesignatedPoint is Navaid)
					pFIXSignPt.NavaidSystem = pFixDesignatedPoint.GetFeatureRef();
				pSegmentPoint.PointChoice = pFIXSignPt;
			}
			//========
			pSegmentLeg.StartPoint = pEndPoint;

			// End Of End Point ========================

			// Angle ========================
			double Angle = NativeMethods.Modulus(Course - GlobalVars.CurrADHP.MagVar, 360.0);

			pAngleIndication = DBModule.CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pEndPoint.PointChoice);
			pAngleIndication.TrueAngle = Course;
			pAngleIndication.IndicationDirection = CodeDirectionReference.FROM;
			pSegmentLeg.Angle = pAngleIndication.GetFeatureRef();

			//=======================================================================================

			// Start Point ========================
			pEndPoint = new TerminalSegmentPoint();
			//pEndPoint.IndicatorFACF =      ??????????
			//pEndPoint.LeadDME =            ??????????
			//pEndPoint.LeadRadial =         ??????????
			pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.FROP;

			pSegmentPoint = pEndPoint;

			pSegmentPoint.FlyOver = true;
			pSegmentPoint.RadarGuidance = false;
			pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT;
			pSegmentPoint.Waypoint = false;

			// ========================
			pFixDesignatedPoint = DBModule.CreateDesignatedPoint(leg.StartPrj, "FAF" + (i + 1).ToString("00"), leg.StartDir);

			pFIXSignPt = new SignificantPoint();

			if (pFixDesignatedPoint is DesignatedPoint)
				pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef();
			else if (pFixDesignatedPoint is Navaid)
				pFIXSignPt.NavaidSystem = pFixDesignatedPoint.GetFeatureRef();
			pSegmentPoint.PointChoice = pFIXSignPt;

			//////////////////////////

			//pSegmentLeg.StartPoint = pEndPoint;
			pSegmentLeg.EndPoint = pEndPoint;

			// End Of EndPoint =====================================================================================

			// Trajectory ===========================================
			Curve pCurve = new Curve();
			MultiLineString ls = GlobalVars.pspatialReferenceOperation.ToGeo<MultiLineString>(leg.Nominal);
			pCurve.Geo.Add(ls);
			pSegmentLeg.Trajectory = pCurve;

			// Trajectory =====================================================
			// protected Area =================================================

			Surface pSurface = new Surface();
			MultiPolygon pl = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(leg.Protection);
			pSurface.Geo.Add(pl);

			ObstacleAssessmentArea pPrimProtectedArea = new ObstacleAssessmentArea();
			pPrimProtectedArea.Surface = pSurface;
			pPrimProtectedArea.SectionNumber = 0;
			pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY;
			pSegmentLeg.DesignSurface.Add(pPrimProtectedArea);

			//  END ======================================================

			// Protection Area Obstructions list ==================================================
			ObstacleContainer ostacles = leg.ObstaclesList;

			Functions.Sort(ref ostacles, 2);
			int saveCount = Math.Min(ostacles.Obstacles.Length, 15);

			for (i = 0; i < ostacles.Parts.Length; i++)
			{
				if (saveCount == 0)
					break;

				double RequiredClearance = 0.0;

				Obstacle tmpObstacle = ostacles.Obstacles[ostacles.Parts[i].Owner];
				if (tmpObstacle.NIx > -1)
					continue;

				tmpObstacle.NIx = 0;

				//MinimumAltitude = Math.Max(MinimumAltitude, ostacles.Parts[ostacles.Obstacles[i].Parts[j]].ReqH)
				RequiredClearance = ostacles.Parts[i].MOC;

				Obstruction obs = new Obstruction();
				obs.VerticalStructureObstruction = new FeatureRef(ostacles.Obstacles[i].Identifier);

				//ReqH
				double MinimumAltitude = ostacles.Obstacles[i].Height + RequiredClearance + ptTHRprj.Z;

				pDistanceVertical = new ValDistanceVertical();
				pDistanceVertical.Uom = mUomVDistance;
				pDistanceVertical.Value = GlobalVars.unitConverter.HeightToDisplayUnits(MinimumAltitude);
				obs.MinimumAltitude = pDistanceVertical;

				// MOC
				pDistance = new ValDistance();
				pDistance.Uom = UomDistance.M;
				pDistance.Value = RequiredClearance;
				obs.RequiredClearance = pDistance;
				pPrimProtectedArea.SignificantObstacle.Add(obs);

				pDistanceVertical = null;
				pDistance = null;
				saveCount -= 1;
			}

			pSegmentLeg.DesignSurface.Add(pPrimProtectedArea);

			//=================================================================================================================
			ApproachCondition pCondition = new ApproachCondition();
			pCondition.AircraftCategory.Add(IsLimitedTo);

			Minima pMinimumSet = new Minima();

			pMinimumSet.AltitudeCode = CodeMinimumAltitude.OCA;
			pMinimumSet.AltitudeReference = CodeVerticalReference.MSL;

			pMinimumSet.HeightCode = CodeMinimumHeight.OCH;
			pMinimumSet.HeightReference = CodeHeightReference.HAT;

			//If CheckBox0301.Checked Then
			//	pMinimumSet.Altitude = New ValDistanceVertical(ConvertHeight(fTA_OCH + FicTHRprj.Z, eRoundMode.NEAREST), mUomVDistance)
			//	pMinimumSet.Height = New ValDistanceVertical(ConvertHeight(fTA_OCH, eRoundMode.NEAREST), mUomVDistance)
			//Else
			//	pMinimumSet.Altitude = New ValDistanceVertical(ConvertHeight(Math.Max(fMisAprOCH, _CurrFAPOCH) + FicTHRprj.Z, eRoundMode.NEAREST), mUomVDistance)
			//	pMinimumSet.Height = New ValDistanceVertical(ConvertHeight(Math.Max(fMisAprOCH, _CurrFAPOCH), eRoundMode.NEAREST), mUomVDistance)
			//End If
			//pCondition.MinimumSet = pMinimumSet;
			//pFinalLeg.Condition.Add(pCondition);

			// protected Area =================================================

			return pIntermediateLeg;
		}

		private ApproachLeg FinalApproachLeg(int i, InstrumentApproachProcedure pProcedure, AircraftCharacteristic IsLimitedTo, ref TerminalSegmentPoint pEndPoint)
		{
			SegmentPoint pSegmentPoint;
			AngleIndication pAngleIndication;

			ValSpeed pSpeed;
			ValDistance pDistance;
			ValDistanceVertical pDistanceVertical;

			Feature pFixDesignatedPoint;
			SignificantPoint pFIXSignPt;

			//int i = (int)NO_SEQ - 1;

			RFLeg leg = _FASLegs[i];

			FinalLeg pFinalLeg = DBModule.pObjectDir.CreateFeature<FinalLeg>();
			pFinalLeg.GuidanceSystem = CodeFinalGuidance.LPV;
			pFinalLeg.AircraftCategory.Add(IsLimitedTo);

			//pFinalLeg.Approach = pProcedure.GetFeatureRef();

			SegmentLeg pSegmentLeg = pFinalLeg;
			// pSegmentLeg.AltitudeOverrideATC =
			// pSegmentLeg.AltitudeOverrideReference =
			// pSegmentLeg.Duration = ' ???????????????????????????????? pLegs(I).valDur
			// pSegmentLeg.Note =
			// pSegmentLeg.ProcedureTurnRequired =
			// pSegmentLeg.ReqNavPerformance = 
			// pSegmentLeg.SpeedInterpretation = CodeAltitudeUse.RECOMMENDED;

			pSegmentLeg.AltitudeInterpretation = Aran.Aim.Enums.CodeAltitudeUse.BETWEEN;
			pSegmentLeg.UpperLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL;
			pSegmentLeg.LowerLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL;
			pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT;
			pSegmentLeg.CourseType = Aran.Aim.Enums.CodeCourse.TRUE_TRACK;
			//pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT;
			pSegmentLeg.SpeedReference = Aran.Aim.Enums.CodeSpeedReference.IAS;

			pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.CF;

			if (leg.TurnDir == TurnDirection.CW)
				pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.RIGHT;
			else if (leg.TurnDir == TurnDirection.CCW)
				pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.LEFT;

			pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO;

			double Course = GlobalVars.pspatialReferenceOperation.DirToAztPrj(leg.RollOutPrj, leg.RollOutDir);
			pSegmentLeg.Course = Course;

			//====================================================================================================

			pDistanceVertical = new ValDistanceVertical(GlobalVars.unitConverter.HeightToDisplayUnits(leg.RollOutAltitude + ptTHRprj.Z, eRoundMode.NEAREST), mUomVDistance);
			pSegmentLeg.LowerLimitAltitude = pDistanceVertical;

			pDistanceVertical = new ValDistanceVertical(GlobalVars.unitConverter.HeightToDisplayUnits(leg.StartAltitude + ptTHRprj.Z, eRoundMode.NEAREST), mUomVDistance);
			pSegmentLeg.UpperLimitAltitude = pDistanceVertical;

			pDistance = new ValDistance(GlobalVars.unitConverter.DistanceToDisplayUnits(leg.Nominal.Length, eRoundMode.NEAREST), mUomHDistance);
			pSegmentLeg.Length = pDistance;

			pSegmentLeg.BankAngle = leg.BankAngle;
			pSegmentLeg.VerticalAngle = -ARANMath.RadToDeg(System.Math.Atan(leg.DescentGR));


			pSpeed = new ValSpeed(GlobalVars.unitConverter.SpeedToDisplayUnits(_IAS, eRoundMode.SPECIAL_NEAREST), mUomSpeed);
			pSegmentLeg.SpeedLimit = pSpeed;

			// Start Point ========================
			if (pEndPoint == null)
			{
				pEndPoint = new TerminalSegmentPoint();
				//	pStartPoint.IndicatorFACF =      ??????????
				//	pStartPoint.LeadDME =            ??????????
				//	pStartPoint.LeadRadial =         ??????????
				pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.FAF;

				//==

				pSegmentPoint = pEndPoint;
				pSegmentPoint.FlyOver = false;
				pSegmentPoint.RadarGuidance = false;

				pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT;
				pSegmentPoint.Waypoint = false;

				// ========================

				pFixDesignatedPoint = DBModule.CreateDesignatedPoint(leg.StartPrj, "FAF" + (i).ToString("00"), leg.StartDir);
				pFIXSignPt = new SignificantPoint();

				if (pFixDesignatedPoint is DesignatedPoint)
					pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef();
				else if (pFixDesignatedPoint is Navaid)
					pFIXSignPt.NavaidSystem = pFixDesignatedPoint.GetFeatureRef();
				pSegmentPoint.PointChoice = pFIXSignPt;
			}
			//========

			pSegmentLeg.StartPoint = pEndPoint;
			// End Of Start Point ========================

			// Angle ========================
			double Angle = NativeMethods.Modulus(Course - GlobalVars.CurrADHP.MagVar, 360.0);

			pAngleIndication = DBModule.CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pEndPoint.PointChoice);
			pAngleIndication.TrueAngle = Course;
			pAngleIndication.IndicationDirection = CodeDirectionReference.FROM;
			pSegmentLeg.Angle = pAngleIndication.GetFeatureRef();

			// EndPoint ========================
			pEndPoint = new TerminalSegmentPoint();
			//pEndPoint.IndicatorFACF =      ??????????
			//pEndPoint.LeadDME =            ??????????
			//pEndPoint.LeadRadial =         ??????????
			pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.FROP;

			pSegmentPoint = pEndPoint;

			pSegmentPoint.FlyOver = true;
			pSegmentPoint.RadarGuidance = false;
			pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT;
			pSegmentPoint.Waypoint = false;

			// ========================

			pFixDesignatedPoint = DBModule.CreateDesignatedPoint(leg.StartPrj, "FROP" + (i).ToString("00"), leg.StartDir);
			pFIXSignPt = new SignificantPoint();

			if (pFixDesignatedPoint is DesignatedPoint)
				pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef();
			else if (pFixDesignatedPoint is Navaid)
				pFIXSignPt.NavaidSystem = pFixDesignatedPoint.GetFeatureRef();
			pSegmentPoint.PointChoice = pFIXSignPt;

			//////////////////////////

			pSegmentLeg.EndPoint = pEndPoint;

			// End Of EndPoint =====================================================================================
			// Trajectory =====================================================

			Curve pCurve = new Curve();
			MultiLineString ls = GlobalVars.pspatialReferenceOperation.ToGeo<MultiLineString>(leg.Nominal);
			pCurve.Geo.Add(ls);
			pSegmentLeg.Trajectory = pCurve;

			// Trajectory =====================================================
			// protected Area =================================================

			Surface pSurface = new Surface();
			MultiPolygon pl = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(leg.Protection);
			pSurface.Geo.Add(pl);

			ObstacleAssessmentArea pPrimProtectedArea = new ObstacleAssessmentArea();
			pPrimProtectedArea.Surface = pSurface;
			pPrimProtectedArea.SectionNumber = 0;
			pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY;
			pSegmentLeg.DesignSurface.Add(pPrimProtectedArea);

			//  END ======================================================

			// Protection Area Obstructions list ==================================================
			ObstacleContainer ostacles = leg.ObstaclesList;

			Functions.Sort(ref ostacles, 2);
			int saveCount = Math.Min(ostacles.Obstacles.Length, 15);

			for (i = 0; i < ostacles.Parts.Length; i++)
			{
				if (saveCount == 0)
					break;

				double RequiredClearance = 0.0;

				Obstacle tmpObstacle = ostacles.Obstacles[ostacles.Parts[i].Owner];
				if (tmpObstacle.NIx > -1)
					continue;

				tmpObstacle.NIx = 0;

				//MinimumAltitude = Math.Max(MinimumAltitude, ostacles.Parts[ostacles.Obstacles[i].Parts[j]].ReqH)
				RequiredClearance = ostacles.Parts[i].MOC;

				Obstruction obs = new Obstruction();
				obs.VerticalStructureObstruction = new FeatureRef(ostacles.Obstacles[i].Identifier);

				//ReqH
				double MinimumAltitude = ostacles.Obstacles[i].Height + RequiredClearance + ptTHRprj.Z;

				pDistanceVertical = new ValDistanceVertical();
				pDistanceVertical.Uom = mUomVDistance;
				pDistanceVertical.Value = GlobalVars.unitConverter.HeightToDisplayUnits(MinimumAltitude);
				obs.MinimumAltitude = pDistanceVertical;

				// MOC
				pDistance = new ValDistance();
				pDistance.Uom = UomDistance.M;
				pDistance.Value = RequiredClearance;
				obs.RequiredClearance = pDistance;
				pPrimProtectedArea.SignificantObstacle.Add(obs);

				pDistanceVertical = null;
				pDistance = null;
				saveCount -= 1;
			}

			pSegmentLeg.DesignSurface.Add(pPrimProtectedArea);

			//=================================================================================================================
			ApproachCondition pCondition = new ApproachCondition();
			pCondition.AircraftCategory.Add(IsLimitedTo);

			Minima pMinimumSet = new Minima();

			pMinimumSet.AltitudeCode = CodeMinimumAltitude.OCA;
			pMinimumSet.AltitudeReference = CodeVerticalReference.MSL;

			pMinimumSet.HeightCode = CodeMinimumHeight.OCH;
			pMinimumSet.HeightReference = CodeHeightReference.HAT;

			//If CheckBox0301.Checked Then
			//	pMinimumSet.Altitude = New ValDistanceVertical(ConvertHeight(fTA_OCH + FicTHRprj.Z, eRoundMode.NEAREST), mUomVDistance)
			//	pMinimumSet.Height = New ValDistanceVertical(ConvertHeight(fTA_OCH, eRoundMode.NEAREST), mUomVDistance)
			//Else
			//	pMinimumSet.Altitude = New ValDistanceVertical(ConvertHeight(Math.Max(fMisAprOCH, _CurrFAPOCH) + FicTHRprj.Z, eRoundMode.NEAREST), mUomVDistance)
			//	pMinimumSet.Height = New ValDistanceVertical(ConvertHeight(Math.Max(fMisAprOCH, _CurrFAPOCH), eRoundMode.NEAREST), mUomVDistance)
			//End If
			//pCondition.MinimumSet = pMinimumSet;
			//pFinalLeg.Condition.Add(pCondition);

			// protected Area =================================================

			return pFinalLeg;
		}


		#endregion
	}
}
