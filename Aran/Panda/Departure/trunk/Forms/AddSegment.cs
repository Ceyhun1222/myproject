using System;
using System.ComponentModel;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
//using ESRI.ArcGIS.esriSystem;
using Aran.PANDA.Departure.Properties;
using Aran.AranEnvironment;

namespace Aran.PANDA.Departure
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public partial class CAddSegment : System.Windows.Forms.Form
	{
		#region declerations
		const double RadiusCoeff = 0.0000644666;
		const double FootCoeff = 0.3048;
		const int HelpContextID = 2600;
		const int NavSeeCoeff = 2500;
		const int NavMinAngle = 0;
		const int WPTFarness = 100;

		const double AreaMinSeminwidth = 2.0 * 1852.0;
		const double AreaMaxSeminwidth = 4.0 * 1852.0;

		const double RMin = 9.0 * 1852.0;
		const double RMax = 100.0 * 1852.0;
		const double cMaxH = 12800.0;
		const double fMinDist = 500.0;
		const double fMaxDist = 300000.0;

		const double MinTurnAngle = 1.0;
		const double MaxTurnAngle = 120.0;

		public bool IsSectorized;
		public double SectorRightDir;
		public double SectorLeftDir;
		public double SectorHeight;
		public IPointCollection SectorPoly;

		ADHPType CurrADHP;
		RWYType m_DER;
		CDepartOmniDirect m_MasterForm;

		bool bVTextBox003;
		bool bVTextBox004;
		bool bVTextBox005;
		bool bVTextBox006;

		double m_IAS;
		double m_PDG;
		double m_AztDir;
		double m_CurrDir;
		ESRI.ArcGIS.Geometry.IPoint m_CurrPnt;

		double m_CurrAzt;
		double m_HFinish;
		double m_BankAngle;
		double m_seminWidth;
		double m_MagVar;
		double m_TurnR;

		TraceSegment PrevSegment;
		TraceSegment CurrSegment;

		ESRI.ArcGIS.Carto.IElement pNominalTrackElem;
		ESRI.ArcGIS.Carto.IElement pProtectAreaElem;

		Interval Type1_RadInterval;
		NavaidType Type1_CurNavaid;
		WPT_FIXType Type1_CurWPtFix;

		double Type1_CurDist;
		double Type1_CurDir;

		double Type2_CurDir;
		int Type2_CurTurnDir;

		WPT_FIXType Type3_CurNav;

		double Type3_CurDir;
		int Type3_CurTurnDir;

		Interval Type3_CurrInterval;

		Interval[] ComboBox301_LIntervals;
		Interval[] ComboBox301_RIntervals;

		WPT_FIXType Type4_CurFix;
		int Type4_CurTurnDir;

		WPT_FIXType Type5_CurNav;
		Interval[] Type5_Intervals;
		double Type5_CurDir;
		int Type5_CurTurnDir;
		double Type5_SnapAngle;

		NavaidType Type6_CurNav;
		Interval[] Type6_Intervals;

		WPT_FIXType Type7_CurFix;
		NavaidType Type7_CurNav;
		Interval[] Type7_Intervals;

		//WPT_FIXType[] ComboBox101List;
		NavaidType[] ComboBox102List;
		//WPT_FIXType[] ComboBox301List;
		//WPT_FIXType[] ComboBox303List;
		//WPT_FIXType[] ComboBox401List;
		//WPT_FIXType[] ComboBox501List;
		//WPT_FIXType[] ComboBox503List;
		//NavaidType[] ComboBox603List;
		//NavaidType[] ComboBox701List;
		//WPT_FIXType[] ComboBox703List;

		bool IsLoaded;

		IScreenCapture screencapture;
		#endregion

		#region Form
		public CAddSegment(IScreenCapture screencapture)
		{
			InitializeComponent();

			this.screencapture = screencapture;
			IsLoaded = false;
			//SSTab1.Enabled = false;

			SSTab1.SelectedIndex = 0;

			//Application.HelpFile = "Panda.chm";

			m_seminWidth = AreaMinSeminwidth;
			m_BankAngle = 15.0;

			TextBox005.Text = Functions.ConvertDistance(m_seminWidth, eRoundMode.NEAREST).ToString();
			TextBox006.Text = m_BankAngle.ToString();

			Label004.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			Label006.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			Label014.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;

			Label010.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
			Label103.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
			Label310.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
			Label506.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
			Label607.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
			Label610.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
			Label613.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;

			ComboBox704.Left = TextBox703.Left;
			ComboBox704.Top = TextBox703.Top;

			//=================================================================
			SSTab1.TabPages[0].Text = Resources.str13003;
			SSTab1.TabPages[1].Text = Resources.str13004;
			SSTab1.TabPages[2].Text = Resources.str13005;
			SSTab1.TabPages[3].Text = Resources.str13006;
			SSTab1.TabPages[4].Text = Resources.str13007;
			SSTab1.TabPages[5].Text = Resources.str13008;
			SSTab1.TabPages[6].Text = Resources.str13009;

			OkBtn.Text = Resources.str00004;
			CancelBtn.Text = Resources.str00005;

			Label001.Text = Resources.str13506;

			ComboBox001.Items.Add(Resources.str13106);
			ComboBox001.Items.Add(Resources.str13107);

			ComboBox002.Items.Add(Resources.str13106);
			ComboBox002.Items.Add(Resources.str13107);

			//Label005.Caption = Resources.str13106;
			//Label007.Caption = Resources.str13107;
			ComboBox001.SelectedIndex = 0;
			ComboBox002.SelectedIndex = 0;

			//=================================================================
			Label102.Text = Resources.str00578;
			Label101.Text = Resources.str13103;
			Label106.Text = Resources.str13512;

			OptionButton101.Text = Resources.str13104;
			OptionButton102.Text = Resources.str13105;
			OptionButton103.Text = Resources.str13108;

			//=================================================================
			Label201.Text = Resources.str13203;
			Label203.Text = Resources.str13206;
			Label213.Text = Resources.str13306; //Resources.str13204

			ComboBox201.Items.Add(Resources.str13207);
			ComboBox201.Items.Add(Resources.str13208);
			ComboBox201.SelectedIndex = 0;
			//=================================================================
			Label301.Text = Resources.str13302;
			Label303.Text = Resources.str13303;
			Label305.Text = Resources.str13306;

			Label307.Text = Resources.str13506;
			Label309.Text = Resources.str13309;
			Label311.Text = Resources.str13310;

			CheckBox301.Text = Resources.str13305;

			ComboBox302.Items.Add(Resources.str13315);
			ComboBox302.Items.Add(Resources.str13316);
			ComboBox302.SelectedIndex = 0;

			//=================================================================
			Label401.Text = Resources.str13203;
			Label403.Text = Resources.str13306;             //Resources.str13404;
			Label405.Text = Resources.str03613;
			Label407.Text = Resources.str13407;

			ComboBox402.Items.Add(Resources.str13408);
			ComboBox402.Items.Add(Resources.str13409);
			ComboBox402.SelectedIndex = 0;
			//=================================================================
			Label503.Text = Resources.str13511;
			Label508.Text = Resources.str13302;             //Resources.str13502;
			Label509.Text = Resources.str13517;
			Label510.Text = Resources.str13508;
			Label517.Text = Resources.str13503;
			Label518.Text = Resources.str13512;

			CheckBox501.Text = Resources.str13305;          //13507 xxx

			ComboBox502.Items.Add(Resources.str13504);
			ComboBox502.Items.Add(Resources.str13505);
			ComboBox502.SelectedIndex = 0;
			//=================================================================
			TextBox603.Text = Functions.ConvertDistance(RMin, eRoundMode.NEAREST).ToString();
			TextBox604.Text = Functions.ConvertDistance(RMax, eRoundMode.NEAREST).ToString();
			TextBox605.Text = Functions.ConvertDistance(RMin, eRoundMode.NEAREST).ToString();

			ComboBox601.SelectedIndex = 0;
			ComboBox602.SelectedIndex = 0;
			//=================================================================
			ComboBox702.SelectedIndex = 0;

			//Help Context Number: 2600
			//Topic ID: Dep_Omnidir_Route_Modelling
			//Anchor ID: Route_Modelling_Rt_Segm_DialogForm

			this.Text = Resources.str13001;
			IsLoaded = true;
		}

		private void CAddSegment_FormClosing(object sender, FormClosingEventArgs e)
		{
			ClearSegmentDrawings(true);

			if (e.CloseReason == System.Windows.Forms.CloseReason.UserClosing)
			{
				if (m_MasterForm.IsClosing)
					return;

				Hide();
				e.Cancel = true;

				TraceSegment tmpSegment = new TraceSegment();
				m_MasterForm.DialogHook(0, ref tmpSegment);
			}
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

		#region initialisations

		public TraceSegment CreateInitialSegment(RWYType DER, IPolygon pPolygon, double fPDG, double DepDir, double TNAH)
		{
			double ModellingTNA = TNAH + DER.pPtPrj[eRWY.PtDER].Z;

			TraceSegment result = new TraceSegment();

			result.HStart = DER.pPtPrj[eRWY.PtDER].Z + PANS_OPS_DataBase.dpH_abv_DER.Value;
			result.HFinish = ModellingTNA;
			result.PDG = fPDG;
			result.DirIn = DepDir;
			result.DirOut = result.DirIn;

			result.ptIn = DER.pPtPrj[eRWY.PtDER];
			result.ptOut = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir, (result.HFinish - result.HStart) / fPDG);

			//Functions.DrawPointWithText(result.ptIn, "ptIn");
			//Functions.DrawPointWithText(result.ptOut, "ptOut");
			//Application.DoEvents();

			result.PathPrj = (IPolyline)new Polyline();
			((IPointCollection)result.PathPrj).AddPoint(result.ptIn);
			((IPointCollection)result.PathPrj).AddPoint(result.ptOut);

			IPolyline pPolyline = result.PathPrj as IPolyline;
			result.Length = pPolyline.Length;

			//Functions.DrawPolyline(result.PathPrj, 255,2);
			//Application.DoEvents();

			ESRI.ArcGIS.esriSystem.IClone pClone = (ESRI.ArcGIS.esriSystem.IClone)pPolygon;
			result.pProtectArea = (IPolygon)pClone.Clone();
			ITopologicalOperator2 pTopo = (ITopologicalOperator2)result.pProtectArea;
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();
			result.Comment = Resources.str00153; // "Начальный участок"
			result.RepComment = result.Comment;

			//Functions.DrawPolygon(result.pProtectArea, -1, esriSimpleFillStyle.esriSFSCross);
			//Application.DoEvents();
			NavaidType GuidNav = default(NavaidType);
			GuidNav.pPtPrj = DER.pPtPrj[eRWY.PtDER];
			GuidNav.Identifier = new Guid("C3E1A55A-490B-4230-AB1E-D60DB17C7E7C");
			GuidNav.TypeCode = eNavaidType.NONE;
			GuidNav.HorAccuracy = Functions.CalDERcHorisontalAccuracy(DER);

			result.SegmentCode = eSegmentType.straight;
			result.LegType = Aim.Enums.CodeSegmentPath.VA;
			result.GuidanceNav = GuidNav;
			result.InterceptionNav.TypeCode = eNavaidType.NONE;

			IPoint PtGeoSt = Functions.ToGeo(result.ptIn) as IPoint;
			IPoint PtGeoFin = Functions.ToGeo(result.ptOut) as IPoint;

			result.StCoords = Functions.Degree2String(System.Math.Abs(PtGeoSt.Y), Degree2StringMode.DMSLat) + ", " + Functions.Degree2String(System.Math.Abs(PtGeoSt.X), Degree2StringMode.DMSLon);
			result.FinCoords = Functions.Degree2String(System.Math.Abs(PtGeoFin.Y), Degree2StringMode.DMSLat) + ", " + Functions.Degree2String(System.Math.Abs(PtGeoFin.X), Degree2StringMode.DMSLon);

			return result;
		}

		public void CreateNextSegment(ADHPType ADHP, RWYType DER, double fIAS, double fPDG, double fAztDir, TraceSegment PSegment, CDepartOmniDirect masterForm)
		{
			double fTASl;
			m_MasterForm = masterForm;

			PrevSegment = PSegment;
			pNominalTrackElem = null;
			pProtectAreaElem = null;
			//FormRes = false;

			m_IAS = fIAS;
			m_AztDir = fAztDir;
			m_PDG = fPDG;

			CurrADHP = ADHP;
			m_MagVar = ADHP.MagVar;     //0.0;	//

			m_DER = DER;

			CurrSegment.HStart = PrevSegment.HFinish;
			m_HFinish = Math.Min(CurrSegment.HStart, cMaxH);

			m_CurrDir = PrevSegment.DirOut;
			m_CurrPnt = PrevSegment.ptOut;
			m_CurrPnt.M = m_CurrDir;

			m_CurrAzt = Functions.Dir2Azt(m_CurrPnt, m_CurrDir);

			TextBox001.Text = m_CurrAzt.ToString("0.00");

			if (ComboBox001.SelectedIndex == 0)
				TextBox002.Text = Functions.ConvertHeight(CurrSegment.HStart, eRoundMode.NEAREST).ToString();
			else
				TextBox002.Text = Functions.ConvertHeight(CurrSegment.HStart - m_DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.NEAREST).ToString();

			ComboBox002_SelectedIndexChanged(ComboBox002, new System.EventArgs());

			TextBox004.Text = (100.0 * m_PDG).ToString();

			fTASl = Functions.IAS2TAS(m_IAS, CurrSegment.HStart, CurrADHP.ISAtC);
			m_TurnR = Functions.Bank2Radius(m_BankAngle, fTASl);
			TextBox007.Text = Functions.ConvertDistance(m_TurnR, eRoundMode.NEAREST).ToString();

			//SSTab1.Enabled = false;

			//if (PrevSegment.SegmentCode == eSegmentType.arcIntercept)
			//{
			//    SSTab1.TabPages[6].Visible = true;
			//    SSTab1.TabPages[0].Visible = false;
			//    SSTab1.TabPages[1].Visible = false;
			//    SSTab1.TabPages[2].Visible = false;
			//    SSTab1.TabPages[3].Visible = false;
			//    SSTab1.TabPages[4].Visible = false;
			//    SSTab1.TabPages[5].Visible = false;
			//}
			//else
			//{
			//    SSTab1.TabPages[6].Visible = false;
			//    SSTab1.TabPages[0].Visible = true;
			//    SSTab1.TabPages[1].Visible = true;
			//    SSTab1.TabPages[2].Visible = true;
			//    SSTab1.TabPages[3].Visible = true;
			//    SSTab1.TabPages[4].Visible = true;
			//    SSTab1.TabPages[5].Visible = true;
			//}

			//SSTab1.TabMaxWidth = 1105;
			//SSTab1.Enabled = true;

			if (PrevSegment.SegmentCode == eSegmentType.arcIntercept)
			{
				SSTab1.TabPages[0].Enabled = false;
				SSTab1.TabPages[1].Enabled = false;
				SSTab1.TabPages[2].Enabled = false;
				SSTab1.TabPages[3].Enabled = false;
				SSTab1.TabPages[4].Enabled = false;
				SSTab1.TabPages[5].Enabled = false;

				SSTab1.TabPages[6].Enabled = true;
				SSTab1.TabPages[6].Visible = true;

				if (SSTab1.SelectedIndex == 6)
					Do_Option007();
				else
					SSTab1.SelectedIndex = 6;
			}
			else
			{
				SSTab1.TabPages[0].Enabled = true;
				SSTab1.TabPages[1].Enabled = true;
				SSTab1.TabPages[2].Enabled = FillComboBox301Stations() > 0;
				SSTab1.TabPages[3].Enabled = FillComboBox401Stations() > 0;
				SSTab1.TabPages[4].Enabled = FillComboBox501Stations() > 0;
				SSTab1.TabPages[5].Enabled = FillComboBox603DMEStations() > 0;
				SSTab1.TabPages[6].Enabled = false;

				if (SSTab1.SelectedIndex == 0)
					Do_Option001();
				else
					SSTab1.SelectedIndex = 0;
			}

			//SetFormParented(Handle.ToInt32)
			Show(GlobalVars.Win32Window);
		}

		void ConstructNextSegment()
		{
			CurrSegment.pProtectArea = null;
			if (m_seminWidth == 0)
				return;

			ClearSegmentDrawings(false);
			bool Assigned = false;

			if (PrevSegment.SegmentCode == eSegmentType.arcIntercept)
				Assigned = Type7Segment(Type7_CurNav, ref CurrSegment);
			else if (SSTab1.SelectedIndex == 0)
			{
				if (OptionButton101.Checked)
					Assigned = Type1SegmentOnDistance(Type1_CurDist, ref CurrSegment);
				else if (OptionButton102.Checked)
					Assigned = Type1SegmentOnWpt(Type1_CurWPtFix, ref CurrSegment);
				else if (OptionButton103.Checked)
					Assigned = Type1SegmentOnNewFIX(Type1_CurNavaid, Type1_CurDir, ref CurrSegment);
			}

			else if (SSTab1.SelectedIndex == 1)
				Assigned = Type2Segment(Type2_CurDir, Type2_CurTurnDir, ref CurrSegment);
			else if (SSTab1.SelectedIndex == 2)
				Assigned = Type3Segment(Type3_CurNav, Type3_CurDir, ref CurrSegment);
			else if (SSTab1.SelectedIndex == 3)
				Assigned = Type4Segment(Type4_CurFix, Type4_CurTurnDir, ref CurrSegment);
			else if (SSTab1.SelectedIndex == 4)
				Assigned = Type5Segment(Type5_CurNav, ref Type5_CurDir, Type5_CurTurnDir, Type5_SnapAngle, ref CurrSegment);
			else if (SSTab1.SelectedIndex == 5)
				Assigned = Type6Segment(Type6_CurNav, ref CurrSegment);

			bool bRefreshNeeded = true;

			if (Assigned)
			{
				ESRI.ArcGIS.Geometry.IPoint PtGeoSt = (IPoint)Functions.ToGeo(CurrSegment.ptIn);
				ESRI.ArcGIS.Geometry.IPoint PtGeoFin = (IPoint)Functions.ToGeo(CurrSegment.ptOut);

				CurrSegment.HFinish = m_HFinish;
				CurrSegment.BankAngle = m_BankAngle;
				CurrSegment.PDG = m_PDG;
				CurrSegment.StCoords = Functions.Degree2String(System.Math.Abs(PtGeoSt.Y), Degree2StringMode.DMSLat) + ", " + Functions.Degree2String(System.Math.Abs(PtGeoSt.X), Degree2StringMode.DMSLon);
				CurrSegment.FinCoords = Functions.Degree2String(System.Math.Abs(PtGeoFin.Y), Degree2StringMode.DMSLat) + ", " + Functions.Degree2String(System.Math.Abs(PtGeoFin.X), Degree2StringMode.DMSLon);

				ComboBox002_SelectedIndexChanged(ComboBox002, new System.EventArgs());

				//        TextBox003_Validate False

				if (CurrSegment.PathPrj != null)
				{
					ITopologicalOperator2 pTopo = (ITopologicalOperator2)CurrSegment.PathPrj;
					pTopo.IsKnownSimple_2 = false;
					pTopo.Simplify();

					if (!CurrSegment.PathPrj.IsEmpty)
					{
						CurrSegment.pProtectArea = (IPolygon)pTopo.Buffer(m_seminWidth);
						CurrSegment.SeminWidth = m_seminWidth;
						pTopo = (ITopologicalOperator2)CurrSegment.pProtectArea;
						pTopo.IsKnownSimple_2 = false;
						pTopo.Simplify();

						pProtectAreaElem = Functions.DrawPolygon(CurrSegment.pProtectArea, Functions.RGB(0, 255, 0));
						pNominalTrackElem = Functions.DrawPolyline(CurrSegment.PathPrj, Functions.RGB(255, 0, 0));
						bRefreshNeeded = false;
					}
				}
			}

			if (bRefreshNeeded)
				GlobalVars.GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, null, null);
		}

		void ClearSegmentDrawings(bool bRefresh = true)
		{
			ESRI.ArcGIS.Carto.IGraphicsContainer pGraphics;
			pGraphics = GlobalVars.GetActiveView().GraphicsContainer;

			Functions.DeleteGraphicsElement(pNominalTrackElem);
			Functions.DeleteGraphicsElement(pProtectAreaElem);

			if (bRefresh)
				GlobalVars.GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, null, null);
		}

		bool RadialConflict(TraceSegment Segment)
		{
			if (Segment.HStart >= SectorHeight)
				return false;

			ITopologicalOperator2 pTopo = (ITopologicalOperator2)Segment.PathPrj;
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();

			IPointCollection PathPts = (IPointCollection)Segment.PathPrj;
			int n = PathPts.PointCount;

			for (int i = 0; i < n - 1; i++)
			{
				double Ang = NativeMethods.Modulus(90.0 - Functions.ReturnAngleInDegrees(PathPts.Point[i], PathPts.Point[i + 1]));
				double SegmOutDir = NativeMethods.Modulus(Ang - NativeMethods.Modulus(90.0 - m_AztDir));

				if (SegmOutDir > 180.0)
					SegmOutDir = SegmOutDir - 360.0;

				if (SegmOutDir < SectorLeftDir || SegmOutDir > SectorRightDir)
					return true;
			}

			return false;
		}

		IPoint CenterOfTurn(IPoint FromPt, IPoint ToPt)
		{
			double fTmp;
			IPoint pPtX;

			fTmp = GlobalVars.DegToRadValue * (FromPt.M - ToPt.M);
			pPtX = new ESRI.ArcGIS.Geometry.Point();
			IConstructPoint ptConstr = (IConstructPoint)pPtX;

			if (System.Math.Abs(System.Math.Sin(fTmp)) > GlobalVars.DegToRadValue * 0.5)
				ptConstr.ConstructAngleIntersection(FromPt, GlobalVars.DegToRadValue * NativeMethods.Modulus(FromPt.M + 90.0), ToPt, GlobalVars.DegToRadValue * NativeMethods.Modulus(ToPt.M + 90.0));
			else
				pPtX.PutCoords(0.5 * (FromPt.X + ToPt.X), 0.5 * (FromPt.Y + ToPt.Y));

			return pPtX;
		}

		string FormatInterval(Interval I)
		{
			string result = "";

			if (I.Right == -1)
				result = I.Left.ToString() + "°";
			else
				result = Resources.str13313 + I.Left.ToString("0.0") + Resources.str13314 + I.Right.ToString("0.0") + "°";

			return result;
		}

		int FillComboBox101Stations()
		{
			ComboBox101.Items.Clear();
			int n = GlobalVars.WPTList.Length;

			for (int i = 0; i < n; i++)
			{
				bool Type1PossibleWpt = (Functions.SideDef(m_CurrPnt, m_CurrDir + 90.0, GlobalVars.WPTList[i].pPtPrj) > 0) &&
					(Functions.Point2LineDistancePrj(GlobalVars.WPTList[i].pPtPrj, m_CurrPnt, m_CurrDir) < WPTFarness) &&
					(Functions.ReturnDistanceInMeters(m_CurrPnt, GlobalVars.WPTList[i].pPtPrj) > 1);

				if (Type1PossibleWpt)
					ComboBox101.Items.Add(GlobalVars.WPTList[i]);
			}

			return ComboBox101.Items.Count;
		}

		int FillComboBox102Stations()
		{
			int i, j, l, n, m;

			double fTmp, navX, navY, navL, ConDist, interLeft, interRight, cotan23,
				VORConDist, NDBConDist, minInterceptAngle, maxInterceptAngle;

			minInterceptAngle = 30.0;
			maxInterceptAngle = 180.0 - minInterceptAngle;

			VORConDist = CurrSegment.HStart * Math.Tan(Functions.DegToRad(Navaids_DataBase.VOR.ConeAngle));
			NDBConDist = CurrSegment.HStart * Math.Tan(Functions.DegToRad(Navaids_DataBase.NDB.ConeAngle));

			ComboBox102.Items.Clear();

			n = GlobalVars.NavaidList.Length;
			m = GlobalVars.DMEList.Length;
			j = -1;
			ComboBox102List = new NavaidType[n + m];

			for (i = 0; i < n; i++)
			{
				IPoint pPtNav = GlobalVars.NavaidList[i].pPtPrj;
				Functions.PrjToLocal(m_CurrPnt, m_CurrDir, pPtNav.X, pPtNav.Y, out navX, out navY);

				if (GlobalVars.NavaidList[i].TypeCode == eNavaidType.VOR || GlobalVars.NavaidList[i].TypeCode == eNavaidType.TACAN)
					ConDist = VORConDist;
				else
					ConDist = NDBConDist;

				//'DrawPointWithText pPtNav, "SL"
				//'DrawPointWithText m_CurrPnt, "CurrPnt"
				//'DrawPointWithText PointAlongPlane(m_CurrPnt, m_CurrDir + 90#, navY), "navY"

				if (navX > GlobalVars.MaxNAVDist) continue;
				if (navX < -0.5 * GlobalVars.MaxNAVDist) continue;

				if (System.Math.Abs(navY) > GlobalVars.MaxNAVDist) continue;
				if (System.Math.Abs(navY) < ConDist) continue;


				fTmp = NativeMethods.Modulus(90.0 - Functions.RadToDeg(Math.Atan(navX / navY)), 180.0);

				if (navY > 0.0)
				{
					interLeft = Math.Max(fTmp, minInterceptAngle);
					if (interLeft > maxInterceptAngle)
						continue;
					interLeft = interLeft + 180.0;
					interRight = maxInterceptAngle + 180.0;
				}
				else
				{
					interRight = Math.Min(fTmp, maxInterceptAngle);
					if (interRight < minInterceptAngle)
						continue;
					interLeft = minInterceptAngle;
				}

				j++;

				ComboBox102List[j] = GlobalVars.NavaidList[i];
				ComboBox102List[j].ValMax = new double[1];
				ComboBox102List[j].ValMin = new double[1];

				ComboBox102List[j].ValMin[0] = interLeft;
				ComboBox102List[j].ValMax[0] = interRight;

				ComboBox102.Items.Add(ComboBox102List[j]);
			}

			int ii, jj, ll, kk = 0;
			Interval Int23, IntRange;
			Interval[] IntrRes, IntrRes1;
			double Y55, cotan55, L55;

			cotan23 = 1.0 / System.Math.Tan(GlobalVars.DegToRadValue * PANS_OPS_DataBase.dpTP_by_DME_div.Value);
			cotan55 = 1.0 / System.Math.Tan(GlobalVars.DegToRadValue * Navaids_DataBase.DME.SlantAngle);

			for (i = 0; i < m; i++)
			{
				IPoint pPtNav = GlobalVars.DMEList[i].pPtPrj;
				Functions.PrjToLocal(m_CurrPnt, m_CurrDir, pPtNav.X, pPtNav.Y, out navX, out navY);
				navY = System.Math.Abs(navY);

				navL = navY * cotan23;

				Y55 = (CurrSegment.HStart - pPtNav.Z) * cotan55;

				if (Y55 > navY)
				{
					L55 = System.Math.Sqrt(Y55 * Y55 - navY * navY);
					navL = System.Math.Max(navL, L55);
				}

				IntRange.Left = navX - Navaids_DataBase.DME.Range;
				IntRange.Right = navX + Navaids_DataBase.DME.Range;
				if (IntRange.Left < fMinDist) IntRange.Left = fMinDist;
				if (IntRange.Right > GlobalVars.MaxNAVDist) IntRange.Right = GlobalVars.MaxNAVDist;

				Int23.Left = navX - navL;
				Int23.Right = navX + navL;

				IntrRes = Functions.IntervalsDifference(IntRange, Int23);
				l = IntrRes.Length - 1;
				IntrRes1 = new Interval[0];

				ii = 0;
				if (l >= 0)
					do
					{
						if (IntrRes[ii].Left == IntrRes[ii].Right)
						{
							for (jj = ii; jj < l; jj++)
								IntrRes[jj] = IntrRes[jj + 1];

							l--;
						}
						else
							ii++;
					}
					while (ii < l - 1);

				ii = 0;
				while (ii < l - 1)
				{
					if (IntrRes[ii].Right == IntrRes[ii + 1].Left)
					{
						IntrRes[ii].Right = IntrRes[ii + 1].Right;
						for (jj = ii + 1; jj < l; jj++)
							IntrRes[jj] = IntrRes[jj + 1];

						l--;
					}
					else
						ii++;
				}

				if (l < 0) continue;

				IntrRes1 = new Interval[l + 1];
				ll = -1;

				for (ii = 0; ii <= l; ii++)
				{
					Int23.Left = IntrRes[ii].Left - navX;
					Int23.Right = IntrRes[ii].Right - navX;

					if (Int23.Left < Int23.Right)
					{
						ll++;

						if (Int23.Left < 0)
						{
							kk = -1;
							IntrRes1[ll].Right = System.Math.Abs(Int23.Left);
							IntrRes1[ll].Left = System.Math.Abs(Int23.Right);
						}
						else
						{
							kk = 1;
							IntrRes1[ll].Left = Int23.Left;
							IntrRes1[ll].Right = Int23.Right;
						}

						//ptNearD = PointAlongPlane(m_CurrPnt, m_CurrDir, IntrRes(ii).Left)
						//kk = SideDef(KKhMinDME.FromPoint, m_CurrDir + 90#, pPtNav)
						//ComboBox102List(J).ValCnt = SideDef(KKhMinDME.FromPoint, NomDir + 90#, ptFNavPrj)
					}
				}

				if (ll < 0) continue;
				if (ll > 0) kk = 0;

				//if( IntrRes1(0).Right < IntrRes1(0).Left ) GoTo NextDME

				j++;

				ComboBox102List[j] = GlobalVars.DMEList[i];

				ComboBox102List[j].ValCnt = kk;
				ComboBox102List[j].ValMax = new double[ll + 1];
				ComboBox102List[j].ValMin = new double[ll + 1];

				for (ii = 0; ii <= ll; ii++)
				{
					ComboBox102List[j].ValMin[ii] = IntrRes1[ii].Left;
					ComboBox102List[j].ValMax[ii] = IntrRes1[ii].Right;
				}

				ComboBox102.Items.Add(ComboBox102List[j]);
			}

			j++;
			System.Array.Resize<NavaidType>(ref ComboBox102List, j);

			return j;
		}

		int FillComboBox301Stations()
		{
			int i, j, n;
			double fRefX, fRefY, InvTan1Deg, InvTanMaxDeg, fTmp, fMaxInterceptDist;

			IPoint pPtRef;

			Interval LInterval, RInterval;
			bool bYaradi;
			double fT1, fT2, fAlpha1, fAlpha2, fAbsRefY;

			double fDeterOp, fDeterPo;

			InvTan1Deg = 1.0 / System.Math.Tan(GlobalVars.DegToRadValue);
			InvTanMaxDeg = 1.0 / System.Math.Tan(GlobalVars.DegToRadValue * (180.0 - MaxTurnAngle));

			ComboBox301.Items.Clear();

			j = -1;

			n = GlobalVars.WPTList.Length;
			ComboBox301_LIntervals = new Interval[n];
			ComboBox301_RIntervals = new Interval[n];

			for (i = 0; i < n; i++)
			{
				pPtRef = GlobalVars.WPTList[i].pPtPrj;

				Functions.PrjToLocal(m_CurrPnt, m_CurrDir, pPtRef.X, pPtRef.Y, out fRefX, out fRefY);
				fAbsRefY = System.Math.Abs(fRefY);

				if (fRefX < -GlobalVars.MaxNAVDist || fRefX > GlobalVars.MaxNAVDist || fAbsRefY > GlobalVars.MaxNAVDist)
				{
				}
				else
				{
					fDeterOp = System.Math.Sqrt(fRefX * fRefX + fAbsRefY * (fAbsRefY + 2.0 * m_TurnR));
					fDeterPo = fRefX * fRefX + fAbsRefY * (fAbsRefY - 2 * m_TurnR);
					if (fDeterPo < 0)
						fDeterPo = -1;
					else
						fDeterPo = System.Math.Sqrt(fDeterPo);

					RInterval.Left = -1;
					RInterval.Right = -1;
					LInterval.Left = -1;
					LInterval.Right = -1;

					if (fRefY > 0.0)
					{
						//Sola =========================================================
						if (fDeterPo >= 0.0)
						{
							fAlpha2 = MaxTurnAngle;

							if (fAbsRefY > 2.0 * m_TurnR)
							{
								fT1 = (-fRefX + fDeterPo) / (fAbsRefY - 2.0 * m_TurnR);
								fAlpha1 = 2.0 * GlobalVars.RadToDegValue * System.Math.Atan(fT1);

								if (fAlpha1 < fAlpha2)
								{
									LInterval.Left = fAlpha1;
									LInterval.Right = fAlpha2;
								}
							}
							else if (fAbsRefY < 2.0 * m_TurnR)
							{
								fT1 = (-fRefX + fDeterPo) / (fAbsRefY - 2.0 * m_TurnR);
								fAlpha1 = 2.0 * GlobalVars.RadToDegValue * System.Math.Atan(fT1);

								if (fAbsRefY > 0)
								{
									fT2 = (-fRefX - fDeterPo) / (fAbsRefY - 2.0 * m_TurnR);
									fAlpha2 = Math.Min(2.0 * GlobalVars.RadToDegValue * System.Math.Atan(fT2), fAlpha2);

									if (fAlpha2 > MinTurnAngle && fAlpha1 < fAlpha2)
									{
										if (fAlpha1 < MinTurnAngle) fAlpha1 = MinTurnAngle;
										if (fAlpha2 > MaxTurnAngle) fAlpha2 = MaxTurnAngle;
										LInterval.Left = fAlpha1;
										LInterval.Right = fAlpha2;
									}
								}
							}
						}

						//Sag"a =========================================================
						fAlpha1 = MinTurnAngle;
						fMaxInterceptDist = fRefX + fAbsRefY / System.Math.Tan(GlobalVars.DegToRadValue * MinTurnAngle);

						if (fMaxInterceptDist > fMaxDist)
						{
							fTmp = fMaxDist - fRefX;
							if (System.Math.Abs(fTmp) < GlobalVars.distEps)
								fAlpha1 = 90.0;
							else
							{
								fAlpha1 = GlobalVars.RadToDegValue * System.Math.Atan(fAbsRefY / fTmp);
								if (fAlpha1 < 0) fAlpha1 += 180.0;
							}
						}

						fT2 = (fRefX + fDeterOp) / (fAbsRefY + 2.0 * m_TurnR);
						fAlpha2 = 2.0 * GlobalVars.RadToDegValue * System.Math.Atan(fT2);
						if (fAlpha2 > MaxTurnAngle) fAlpha2 = MaxTurnAngle;

						if (fAlpha2 >= MinTurnAngle && fAlpha2 - fAlpha1 > 1.0)
						{
							RInterval.Left = fAlpha1;
							RInterval.Right = fAlpha2;
						}
					}
					else if (fRefY < 0.0)
					{
						//Sola =========================================================
						fAlpha1 = MinTurnAngle;
						fMaxInterceptDist = fRefX + fAbsRefY / System.Math.Tan(GlobalVars.DegToRadValue * MinTurnAngle);

						if (fMaxInterceptDist > fMaxDist)
						{
							fTmp = fMaxDist - fRefX;
							if (System.Math.Abs(fTmp) < GlobalVars.distEps)
								fAlpha1 = 90.0;
							else
							{
								fAlpha1 = GlobalVars.RadToDegValue * System.Math.Atan(fAbsRefY / fTmp);
								if (fAlpha1 < 0) fAlpha1 += 180.0;
							}
						}

						fT2 = (fRefX + fDeterOp) / (fAbsRefY + 2.0 * m_TurnR);
						fAlpha2 = 2.0 * GlobalVars.RadToDegValue * System.Math.Atan(fT2);
						if (fAlpha2 > MaxTurnAngle) fAlpha2 = MaxTurnAngle;

						if (fAlpha2 >= MinTurnAngle && fAlpha2 - fAlpha1 > 1.0)
						{
							LInterval.Left = fAlpha1;
							LInterval.Right = fAlpha2;
						}

						//Sag"a =========================================================
						if (fDeterPo >= 0)
						{
							fAlpha2 = MaxTurnAngle;

							if (fAbsRefY > 2.0 * m_TurnR)
							{
								fT1 = (-fRefX + fDeterPo) / (fAbsRefY - 2.0 * m_TurnR);
								fAlpha1 = 2.0 * GlobalVars.RadToDegValue * System.Math.Atan(fT1);

								if (fAlpha1 < fAlpha2)
								{
									RInterval.Left = fAlpha1;
									RInterval.Right = fAlpha2;
								}
							}
							else if (fAbsRefY < 2.0 * m_TurnR)
							{
								fT1 = (-fRefX + fDeterPo) / (fAbsRefY - 2.0 * m_TurnR);
								fAlpha1 = 2.0 * GlobalVars.RadToDegValue * System.Math.Atan(fT1);

								if (fAbsRefY > 0)
								{
									fT2 = (-fRefX - fDeterPo) / (fAbsRefY - 2.0 * m_TurnR);
									fAlpha2 = Math.Min(2.0 * GlobalVars.RadToDegValue * System.Math.Atan(fT2), fAlpha2);

									if (fAlpha2 > MinTurnAngle && fAlpha1 < fAlpha2)
									{
										if (fAlpha1 < MinTurnAngle) fAlpha1 = MinTurnAngle;
										if (fAlpha2 > MaxTurnAngle) fAlpha2 = MaxTurnAngle;
										RInterval.Left = fAlpha1;
										RInterval.Right = fAlpha2;
									}
								}
							}
						}
					}
					else
					{
						if (fRefX > 0)
						{
							fT1 = fRefX / m_TurnR;
							fAlpha1 = 2 * System.Math.Atan(fT1);

							if (fAlpha1 >= MinTurnAngle + 1.0)
							{
								if (fAlpha1 > MaxTurnAngle) fAlpha1 = MaxTurnAngle;
								LInterval.Left = MinTurnAngle;
								LInterval.Right = fAlpha1;

								RInterval.Left = MinTurnAngle;
								RInterval.Right = fAlpha1;
							}
						}
					}

					bYaradi = (LInterval.Right > 0) || (RInterval.Right > 0);

					if (bYaradi)
					{
						j++;
						ComboBox301_LIntervals[j] = LInterval;
						ComboBox301_RIntervals[j] = RInterval;
						ComboBox301.Items.Add(GlobalVars.WPTList[i]);
					}
				}
			}

			j++;
			Array.Resize<Interval>(ref ComboBox301_LIntervals, j);
			Array.Resize<Interval>(ref ComboBox301_RIntervals, j);

			return j;
		}

		int FillComboBox401Stations()
		{
			int n = GlobalVars.WPTList.Length;
			ComboBox401.Items.Clear();

			for (int i = 0; i < n; i++)
			{
				//IPoint pPtCnt = Functions.PointAlongPlane(GlobalVars.WPTList[i].pPtPrj, m_CurrDir + 90.0, m_TurnR);
				//double fMinDist = Functions.ReturnDistanceInMeters(m_CurrPnt, pPtCnt);

				//pPtCnt = Functions.PointAlongPlane(GlobalVars.WPTList[I].pPtPrj, m_CurrDir - 90.0, m_TurnR);
				//double fDist = Functions.ReturnDistanceInMeters(m_CurrPnt, pPtCnt);
				//if (fDist < fMinDist) fMinDist = fDist;

				//if (fMinDist > m_TurnR)
				//{
				//    j++;
				//    ComboBox401List[j] = GlobalVars.WPTList[i];
				//    ComboBox401.Items.Add(ComboBox401List[j].Name);
				//}

				if (Functions.ReturnDistanceInMeters(m_CurrPnt, GlobalVars.WPTList[i].pPtPrj) > m_TurnR)
					ComboBox401.Items.Add(GlobalVars.WPTList[i]);
			}

			return ComboBox401.Items.Count;
		}

		int FillComboBox501Stations()
		{
			ComboBox503.Enabled = false;
			CheckBox501.Checked = false;

			int n = GlobalVars.WPTList.Length;

			ComboBox501.Items.Clear();

			for (int i = 0; i < n; i++)
				if (Functions.ReturnDistanceInMeters(m_CurrPnt, GlobalVars.WPTList[i].pPtPrj) < GlobalVars.MaxNAVDist)
					ComboBox501.Items.Add(GlobalVars.WPTList[i]);

			return ComboBox501.Items.Count;
		}

		int FillComboBox603DMEStations()
		{
			int I, J, N, Side, TurnDir;

			double minR1R2, maxR1R2, R1, R2, R11, R12, R21, R22, CosA1, CosA2, Bheta, phi, R, fTmp, L;

			CosA1 = -2.0;
			CosA2 = 1.0;
			if (!double.TryParse(TextBox605.Text, out fTmp))
			{
			}

			R = Functions.DeConvertDistance(fTmp);

			J = -1;
			N = GlobalVars.DMEList.Length;
			//ComboBox603List = new NavaidType[N];
			Type6_Intervals = new Interval[N];

			Side = 2 * ComboBox601.SelectedIndex - 1;
			TurnDir = 2 * ComboBox602.SelectedIndex - 1;

			ComboBox603.Items.Clear();

			for (I = 0; I < N; I++)
			{
				IPoint ptDME = GlobalVars.DMEList[I].pPtPrj;
				phi = Functions.ReturnAngleInDegrees(m_CurrPnt, ptDME);
				L = Functions.ReturnDistanceInMeters(m_CurrPnt, ptDME);

				Bheta = m_CurrDir - phi;

				R11 = CosA1 * (Side * m_TurnR - TurnDir * L * System.Math.Sin(Functions.DegToRad(Bheta))) - Side * m_TurnR; //if Cos(A1) = -0.5
				R12 = -1000000 * (Side * m_TurnR - TurnDir * L * System.Math.Sin(Functions.DegToRad(Bheta))) - Side * m_TurnR;  //if Cos(A1) = -0.5

				R21 = CosA2 * (Side * m_TurnR - TurnDir * L * System.Math.Sin(Functions.DegToRad(Bheta))) - Side * m_TurnR; //if Cos(A2) = 1
				R22 = 1000000 * (Side * m_TurnR - TurnDir * L * System.Math.Sin(Functions.DegToRad(Bheta))) - Side * m_TurnR;   //if Cos(A2) = 1

				if (R12 < 0)
				{
					maxR1R2 = Math.Max(R21, R22);
					minR1R2 = Math.Min(R21, R22);
				}
				else
				{
					maxR1R2 = Math.Max(R11, R12);
					minR1R2 = Math.Min(R11, R12);
				}

				//if (B > -90 && B < 90) {
				//}

				if (minR1R2 < RMax && maxR1R2 > RMin)
				{
					R2 = Math.Min(maxR1R2, RMax);
					R1 = Math.Max(minR1R2, RMin);

					if (Side > 0)
					{
						fTmp = L - 1.5 * m_TurnR;
						if (R2 > fTmp) R2 = fTmp;
						if (System.Math.Cos(Functions.DegToRad(Bheta)) < 0) R2 = R1;
					}
					else
					{
						fTmp = L + 1.5 * m_TurnR;
						if (R1 < fTmp) R1 = fTmp;
					}

					if (R1 < R2)
					{
						J++;
						//ComboBox603List[J] = GlobalVars.DMEList[I];
						Type6_Intervals[J].Left = R1;
						Type6_Intervals[J].Right = R2;
						ComboBox603.Items.Add(GlobalVars.DMEList[I]);
					}
				}
			}

			return J + 1;
		}

		void FillComboBox701Stations()
		{
			if (PrevSegment.SegmentCode != eSegmentType.arcIntercept)
				return;

			const double CosaMax = -0.5;

			int ExitSide = 1 - 2 * ComboBox702.SelectedIndex;
			int n = GlobalVars.NavaidList.Length;
			int j = -1;
			//ComboBox701List = new NavaidType[n];
			Type7_Intervals = new Interval[n];
			ComboBox701.Items.Clear();

			for (int i = 0; i < n; i++)
			{
				double minInter, maxInter, MinAngle, MaxAngle;
				double L = Functions.ReturnDistanceInMeters(GlobalVars.NavaidList[i].pPtPrj, PrevSegment.PtCenter2);

				if (PrevSegment.Turn2Dir > 0)
				{
					if (PrevSegment.Turn2R < L)
					{
						minInter = Functions.RadToDeg(Math.Asin(-PrevSegment.Turn2R / L));
						maxInter = 180.0 - minInter;
					}
					else
					{
						minInter = 0.0;
						maxInter = 360.0;
					}

					double fTmp = ExitSide * m_TurnR - (PrevSegment.Turn2R + ExitSide * PrevSegment.Turn2R) * CosaMax;
					if (System.Math.Abs(fTmp) < L)
					{
						MaxAngle = Functions.RadToDeg(Math.Asin(fTmp / L));
						MinAngle = -180.0 - MaxAngle;
					}
					else
					{
						MinAngle = 0.0;
						MaxAngle = 360.0;
					}
				}
				else
				{
					if (PrevSegment.Turn2R < L)
					{
						maxInter = Functions.RadToDeg(Math.Asin(PrevSegment.Turn2R / L));
						minInter = -180.0 - maxInter;
					}
					else
					{
						minInter = 0.0;
						maxInter = 360.0;
					}

					double fTmp = -ExitSide * m_TurnR + (PrevSegment.Turn2R + ExitSide * PrevSegment.Turn2R) * CosaMax;
					if (System.Math.Abs(fTmp) < L)
					{
						MinAngle = Functions.RadToDeg(Math.Asin(fTmp / L));
						MaxAngle = 180.0 - MinAngle;
					}
					else
					{
						MinAngle = 0.0;
						MaxAngle = 360.0;
					}
				}

				if (maxInter < minInter) maxInter = 360.0 + maxInter;
				if (MaxAngle < MinAngle) MaxAngle = 360.0 + MaxAngle;

				double MinOut = Math.Max(minInter, MinAngle);
				double MaxOut = Math.Min(maxInter, MaxAngle);

				if (MinOut < MaxOut)
				{
					double phi = Functions.ReturnAngleInDegrees(GlobalVars.NavaidList[i].pPtPrj, PrevSegment.PtCenter2);
					j++;

					//ComboBox701List[j] = GlobalVars.NavaidList[i];
					ComboBox701.Items.Add(GlobalVars.NavaidList[i]);
					if (MinOut == 0 && MaxOut == 360.0)
					{
						Type7_Intervals[j].Left = 0.0;
						Type7_Intervals[j].Right = 360.0;
					}
					else
					{
						Type7_Intervals[j].Left = NativeMethods.Modulus(phi + MinOut);
						Type7_Intervals[j].Right = NativeMethods.Modulus(phi + MaxOut);
					}
				}
			}

			if (ComboBox701.Items.Count > 0)
				ComboBox701.SelectedIndex = 0;
		}

		void FillComboBox703List()
		{
			int n = GlobalVars.WPTList.Length;
			int k = ComboBox701.SelectedIndex;

			ComboBox703.Items.Clear();

			for (int i = 0; i < n; i++)
			{
				if (GlobalVars.WPTList[i].Name != ComboBox701.Text)
				{
					double NewVal = Functions.ReturnAngleInDegrees(Type7_CurNav.pPtPrj, GlobalVars.WPTList[i].pPtPrj);
					if (Type7_Intervals[k].Left == 0.0 && Type7_Intervals[k].Right == 360.0)
						ComboBox703.Items.Add(GlobalVars.WPTList[i]);
					else if (Functions.AngleInSector(NewVal, Type7_Intervals[k].Left, Type7_Intervals[k].Right))
						ComboBox703.Items.Add(GlobalVars.WPTList[i]);
					else if (Functions.AngleInSector(NewVal + 180.0, Type7_Intervals[k].Left, Type7_Intervals[k].Right))
						ComboBox703.Items.Add(GlobalVars.WPTList[i]);
				}
			}

			OptionButton702.Enabled = ComboBox703.Items.Count > 0;
			if (OptionButton702.Enabled)
				ComboBox703.SelectedIndex = 0;
			else
				OptionButton701.Checked = true;
		}

		#endregion

		#region Common events

		private void HelpBtn_Click(object sender, EventArgs e)
		{
			NativeMethods.HtmlHelp(0, "PANDA.chm", GlobalVars.HH_HELP_CONTEXT, HelpContextID);
		}

		private void OkBtn_Click(object sender, EventArgs e)
		{
			ClearSegmentDrawings(true);

			if (IsSectorized)
				if (RadialConflict(CurrSegment))
					if (MessageBox.Show(Resources.str00907, "", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
						return;
			screencapture.Save(this);
			Hide();
			m_MasterForm.DialogHook(1, ref CurrSegment, m_PDG);
		}

		private void CancelBtn_Click(object sender, EventArgs e)
		{
			ClearSegmentDrawings(true);
			Hide();

			TraceSegment tmpSegment = new TraceSegment();
			m_MasterForm.DialogHook(0, ref tmpSegment);
		}

		private void SSTab1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!IsLoaded)
				return;

			switch (SSTab1.SelectedIndex)
			{
				case 0:
					Do_Option001();
					break;
				case 1:
					Do_Option002();
					break;
				case 2:
					Do_Option003();
					break;
				case 3:
					Do_Option004();
					break;
				case 4:
					Do_Option005();
					break;
				case 5:
					Do_Option006();
					break;
				case 6:
					Do_Option007();
					break;
			}
		}

		private void ComboBox001_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!IsLoaded)
				return;

			if (ComboBox001.SelectedIndex == 0)
				TextBox002.Text = Functions.ConvertHeight(CurrSegment.HStart, eRoundMode.NEAREST).ToString();
			else
				TextBox002.Text = Functions.ConvertHeight(CurrSegment.HStart - m_DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.NEAREST).ToString();
		}

		private void ComboBox002_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!IsLoaded)
				return;

			if (ComboBox002.SelectedIndex == 0)
				TextBox003.Text = Functions.ConvertHeight(m_HFinish, eRoundMode.NEAREST).ToString();
			else
				TextBox003.Text = Functions.ConvertHeight(m_HFinish - m_DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.NEAREST).ToString();
		}

		private void TextBox003_KeyPress(object sender, KeyPressEventArgs e)
		{
			char KeyAscii = e.KeyChar;
			if (KeyAscii == 13)
				TextBox003_Validating(TextBox003, new System.ComponentModel.CancelEventArgs());
			else
			{
				Functions.TextBoxFloat(ref KeyAscii, TextBox003.Text);
				bVTextBox003 = true;
			}

			e.KeyChar = KeyAscii;
			if (KeyAscii == 0)
				e.Handled = true;
		}

		//private void TextBox003_Leave(object sender, EventArgs e)
		//{
		//	TextBox003_Validating(TextBox003, new System.ComponentModel.CancelEventArgs());
		//}

		private void TextBox003_Validating(object sender, CancelEventArgs e)
		{
			if (!bVTextBox003)
				return;

			bVTextBox003 = false;

			double fTmp;
			if (!double.TryParse(TextBox003.Text, out fTmp))
				return;

			double hMax = Math.Min(CurrSegment.HStart + CurrSegment.Length * m_PDG, cMaxH);
			if (SSTab1.SelectedIndex == 0 && OptionButton101.Checked)
				hMax = Math.Min(CurrSegment.HStart + GlobalVars.RModel * m_PDG, cMaxH);

			double h = Functions.DeConvertHeight(fTmp);

			if (ComboBox002.SelectedIndex == 1)
				h += m_DER.pPtPrj[eRWY.PtDER].Z;

			m_HFinish = h;

			if (m_HFinish < CurrSegment.HStart)
			{
				m_HFinish = CurrSegment.HStart;
				ComboBox002_SelectedIndexChanged(ComboBox002, new System.EventArgs());
			}
			else if (m_HFinish > hMax)
			{
				m_HFinish = hMax;
				ComboBox002_SelectedIndexChanged(ComboBox002, new System.EventArgs());
			}

			CurrSegment.HFinish = m_HFinish;

			if (SSTab1.SelectedIndex == 0 && OptionButton101.Checked)
			{
				double legLen = (m_HFinish - CurrSegment.HStart) / m_PDG;
				TextBox101.Text = Functions.ConvertDistance(legLen, eRoundMode.CEIL).ToString();
				TextBox101_Validating(TextBox101, new System.ComponentModel.CancelEventArgs());
			}
		}

		private void TextBox004_KeyPress(object sender, KeyPressEventArgs e)
		{
			char KeyAscii = e.KeyChar;
			if (KeyAscii == 13)
				TextBox004_Validating(TextBox004, new System.ComponentModel.CancelEventArgs());
			else
			{
				Functions.TextBoxFloat(ref KeyAscii, TextBox004.Text);
				bVTextBox004 = true;
			}

			e.KeyChar = KeyAscii;
			if (KeyAscii == 0)
				e.Handled = true;
		}

		private void TextBox004_Leave(object sender, EventArgs e)
		{
			TextBox004_Validating(TextBox004, new System.ComponentModel.CancelEventArgs());
		}

		private void TextBox004_Validating(object sender, CancelEventArgs e)
		{
			if (!bVTextBox004)
				return;

			bVTextBox004 = false;
			double fTmp;
			if (!double.TryParse(TextBox004.Text, out fTmp))
				return;

			fTmp *= 0.01;
			m_PDG = System.Math.Round(fTmp, 3);

			if (m_PDG < PANS_OPS_DataBase.dpPDG_Nom.Value) m_PDG = PANS_OPS_DataBase.dpPDG_Nom.Value;
			if (m_PDG > 50.0 * PANS_OPS_DataBase.dpMaxPosPDG.Value) m_PDG = 50.0 * PANS_OPS_DataBase.dpMaxPosPDG.Value;

			if (m_PDG != fTmp)
				TextBox004.Text = (100.0 * m_PDG).ToString();

			ConstructNextSegment();
		}

		private void TextBox005_KeyPress(object sender, KeyPressEventArgs e)
		{
			char KeyAscii = e.KeyChar;
			if (KeyAscii == 13)
				TextBox005_Validating(TextBox005, new System.ComponentModel.CancelEventArgs());
			else
			{
				Functions.TextBoxFloat(ref KeyAscii, TextBox005.Text);
				bVTextBox005 = true;
			}

			e.KeyChar = KeyAscii;
			if (KeyAscii == 0)
				e.Handled = true;
		}

		private void TextBox005_Leave(object sender, EventArgs e)
		{
			TextBox005_Validating(TextBox005, new System.ComponentModel.CancelEventArgs());
		}

		private void TextBox005_Validating(object sender, CancelEventArgs e)
		{
			if (!bVTextBox005)
				return;
			bVTextBox005 = false;

			double fTmp;
			if (!double.TryParse(TextBox005.Text, out fTmp))
				return;

			m_seminWidth = Functions.DeConvertDistance(fTmp);
			fTmp = m_seminWidth;

			if (m_seminWidth < AreaMinSeminwidth)
				m_seminWidth = AreaMinSeminwidth;
			else if (m_seminWidth > AreaMaxSeminwidth)
				m_seminWidth = AreaMaxSeminwidth;

			if (m_seminWidth != fTmp)
				TextBox005.Text = Functions.ConvertDistance(m_seminWidth, eRoundMode.NEAREST).ToString();

			ConstructNextSegment();
		}

		private void TextBox006_KeyPress(object sender, KeyPressEventArgs e)
		{
			char KeyAscii = e.KeyChar;
			if (KeyAscii == 13)
				TextBox006_Validating(TextBox006, new System.ComponentModel.CancelEventArgs());
			else
			{
				Functions.TextBoxFloat(ref KeyAscii, TextBox006.Text);
				bVTextBox006 = true;
			}

			e.KeyChar = KeyAscii;
			if (KeyAscii == 0)
				e.Handled = true;
		}

		private void TextBox006_Leave(object sender, EventArgs e)
		{
			TextBox006_Validating(TextBox006, new System.ComponentModel.CancelEventArgs());
		}

		private void TextBox006_Validating(object sender, CancelEventArgs e)
		{
			if (!bVTextBox006)
				return;
			bVTextBox006 = false;

			double fTmp;
			if (!double.TryParse(TextBox006.Text, out fTmp))
				return;

			m_BankAngle = fTmp;

			//if (m_BankAngle < PANS_OPS_DataBase.dpT_Bank.Value)
			//	m_BankAngle = PANS_OPS_DataBase.dpT_Bank.Value;
			if (m_BankAngle < 3)
				m_BankAngle = 3;
			else if (m_BankAngle > 25)
				m_BankAngle = 25;

			if (m_BankAngle != fTmp)
				TextBox006.Text = m_BankAngle.ToString("0.0");

			double fTASl = Functions.IAS2TAS(m_IAS, CurrSegment.HStart, CurrADHP.ISAtC);
			m_TurnR = Functions.Bank2Radius(m_BankAngle, fTASl);
			TextBox007.Text = Functions.ConvertDistance(m_TurnR, eRoundMode.NEAREST).ToString();

			SSTab1_SelectedIndexChanged(SSTab1, new System.EventArgs());
		}

		#endregion

		#region Type 1
		private void ComboBox101_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ComboBox101.SelectedIndex < 0)
				return;
			Type1_CurWPtFix = (WPT_FIXType)(ComboBox101.SelectedItem);  // ComboBox101List[ComboBox101.SelectedIndex];
			ConstructNextSegment();
		}

		private void ComboBox102_SelectedIndexChanged(object sender, EventArgs e)
		{
			double fTmp;//, navX, navY;
			Interval inter;
			//IPoint pPtNav;

			int k = ComboBox102.SelectedIndex;
			if (k < 0)
				return;

			Type1_CurNavaid = ComboBox102List[k];
			Label104.Text = Navaids_DataBase.GetNavTypeName(Type1_CurNavaid.TypeCode);

			if (Type1_CurNavaid.TypeCode != eNavaidType.DME)
			{
				Type1_CurNavaid.IntersectionType = eIntersectionType.ByAngle;

				Label105.Text = "°";

				inter.Left = Type1_CurNavaid.ValMin[0];
				inter.Right = Type1_CurNavaid.ValMax[0];

				Type1_RadInterval.Left = m_CurrDir + inter.Left;
				Type1_RadInterval.Right = m_CurrDir + inter.Right;

				inter.Left = Functions.Dir2Azt(Type1_CurNavaid.pPtPrj, Type1_RadInterval.Right);
				inter.Right = Functions.Dir2Azt(Type1_CurNavaid.pPtPrj, Type1_RadInterval.Left);
				Label107.Text = FormatInterval(inter);
				if (!double.TryParse(TextBox102.Text, out fTmp))
					TextBox102.Text = inter.Left.ToString("0.0");
				TextBox102_Validating(TextBox102, new System.ComponentModel.CancelEventArgs());
			}
			else
			{
				Type1_CurNavaid.IntersectionType = eIntersectionType.ByDistance;

				Label105.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;

				int inx = Type1_CurNavaid.ValMin.Length - 1;
				ComboBox103.Enabled = inx > 0;
				if (inx > 0)
				{
					if (ComboBox103.SelectedIndex < 0)
						ComboBox103.SelectedIndex = 0;
					else
						ComboBox103_SelectedIndexChanged(ComboBox103, new System.EventArgs());
				}
				else
				{
					if (ComboBox103.SelectedIndex == 0)
						ComboBox103_SelectedIndexChanged(ComboBox103, new System.EventArgs());
					else
						ComboBox103.SelectedIndex = 0;
				}
			}
		}

		private void ComboBox103_SelectedIndexChanged(object sender, EventArgs e)
		{
			int k, inx;
			Interval inter;
			k = ComboBox102.SelectedIndex;
			inx = ComboBox103.SelectedIndex;

			inter.Left = ComboBox102List[k].ValMin[inx];
			inter.Right = ComboBox102List[k].ValMax[inx];

			Label107.Text = Resources.str13313 +
				Functions.ConvertDistance(inter.Left, eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit + " " +
				Functions.ConvertDistance(inter.Right, eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;

			double fTmp;
			if (!double.TryParse(TextBox102.Text, out fTmp))
				TextBox102.Text = Functions.ConvertDistance(inter.Left, eRoundMode.NEAREST).ToString();

			TextBox102_Validating(TextBox102, new System.ComponentModel.CancelEventArgs());
		}

		private void OptionButton101_CheckedChanged(object sender, EventArgs e)
		{
			if (!((RadioButton)sender).Checked)
				return;

			TextBox101.ReadOnly = false;

			ComboBox101.Enabled = false;
			ComboBox102.Enabled = false;
			TextBox102.Enabled = false;

			Label106.Visible = false;
			Label107.Visible = false;

			double fTmp;
			if (!double.TryParse(TextBox101.Text, out fTmp))
				TextBox101.Text = Functions.ConvertDistance(fMinDist, eRoundMode.CEIL).ToString();

			TextBox101_Validating(TextBox101, new System.ComponentModel.CancelEventArgs());
		}

		private void OptionButton102_CheckedChanged(object sender, EventArgs e)
		{
			if (!((RadioButton)sender).Checked)
				return;

			TextBox101.ReadOnly = true;

			ComboBox101.Enabled = true;
			ComboBox102.Enabled = false;
			TextBox102.Enabled = false;
			Label104.Visible = false;

			Label106.Visible = false;
			Label107.Visible = false;

			if (ComboBox101.SelectedIndex < 0)
				ComboBox101.SelectedIndex = 0;
			else
				ComboBox101_SelectedIndexChanged(ComboBox101, new System.EventArgs());
		}

		private void OptionButton103_CheckedChanged(object sender, EventArgs e)
		{
			if (!((RadioButton)sender).Checked)
				return;

			TextBox101.ReadOnly = true;

			ComboBox101.Enabled = false;
			ComboBox102.Enabled = true;
			TextBox102.Enabled = true;
			Label104.Visible = true;

			Label106.Visible = true;
			Label107.Visible = true;

			if (ComboBox102.SelectedIndex < 0)
				ComboBox102.SelectedIndex = 0;
			else
				ComboBox102_SelectedIndexChanged(ComboBox102, new System.EventArgs());
		}

		private void TextBox101_KeyPress(object sender, KeyPressEventArgs e)
		{
			char KeyAscii = e.KeyChar;
			if (KeyAscii == 13)
				TextBox101_Validating(TextBox101, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref KeyAscii, TextBox101.Text);

			e.KeyChar = KeyAscii;
			if (KeyAscii == 0)
				e.Handled = true;
		}

		private void TextBox101_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;
			if (!double.TryParse(TextBox101.Text, out fTmp))
				return;

			Type1_CurDist = Functions.DeConvertDistance(fTmp);

			if (Type1_CurDist < fMinDist)
			{
				Type1_CurDist = fMinDist;
				TextBox101.Text = Functions.ConvertDistance(Type1_CurDist, eRoundMode.CEIL).ToString();
			}
			else if (Type1_CurDist > GlobalVars.MaxNAVDist)
			{
				Type1_CurDist = GlobalVars.MaxNAVDist;
				TextBox101.Text = Functions.ConvertDistance(Type1_CurDist, eRoundMode.FLOOR).ToString();
			}

			ConstructNextSegment();
		}

		private void TextBox102_KeyPress(object sender, KeyPressEventArgs e)
		{
			char KeyAscii = e.KeyChar;
			if (KeyAscii == 13)
				TextBox102_Validating(TextBox102, new System.ComponentModel.CancelEventArgs());
			else if (Type1_CurNavaid.TypeCode == eNavaidType.DME)
				Functions.TextBoxFloat(ref KeyAscii, TextBox102.Text);
			else
				Functions.TextBoxInteger(ref KeyAscii);

			e.KeyChar = KeyAscii;
			if (KeyAscii == 0)
				e.Handled = true;
		}

		private void TextBox102_Validating(object sender, CancelEventArgs e)
		{
			double NewVal, fTmp;

			if (!double.TryParse(TextBox102.Text, out fTmp))
				return;

			if (Type1_CurNavaid.TypeCode != eNavaidType.DME)
			{
				NewVal = Functions.Azt2Dir(Type1_CurNavaid.pPtGeo, fTmp);
				fTmp = NewVal;

				if (!Functions.AngleInInterval(NewVal, Type1_RadInterval))
				{
					if (Functions.SubtractAngles(Type1_RadInterval.Left, NewVal) < Functions.SubtractAngles(Type1_RadInterval.Right, NewVal))
						NewVal = Type1_RadInterval.Left;
					else
						NewVal = Type1_RadInterval.Right;
				}

				if (fTmp != NewVal)
					TextBox102.Text = Functions.Dir2Azt(Type1_CurNavaid.pPtPrj, NewVal).ToString("0.00");
			}
			else
			{
				int inx = ComboBox103.SelectedIndex;

				NewVal = Functions.DeConvertDistance(fTmp);
				fTmp = NewVal;

				if (NewVal < Type1_CurNavaid.ValMin[inx])
					NewVal = Type1_CurNavaid.ValMin[inx];
				else if (NewVal > Type1_CurNavaid.ValMax[inx])
					NewVal = Type1_CurNavaid.ValMax[inx];

				if (fTmp != NewVal)
					TextBox102.Text = Functions.ConvertDistance(NewVal, eRoundMode.NEAREST).ToString();
			}

			Type1_CurDir = NewVal;
			ConstructNextSegment();
		}

		#endregion

		#region Type 2

		private void ComboBox201_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ComboBox201.SelectedIndex < 0)
				return;

			if (ComboBox201.SelectedIndex == 0)
				Type2_CurTurnDir = 1;
			else
				Type2_CurTurnDir = -1;

			if (IsLoaded)
				TextBox201_Validating(TextBox201, new System.ComponentModel.CancelEventArgs());
		}


		private void TextBox201_KeyPress(object sender, KeyPressEventArgs e)
		{
			char KeyAscii = e.KeyChar;
			if (KeyAscii == 13)
				TextBox201_Validating(TextBox201, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxInteger(ref KeyAscii);

			e.KeyChar = KeyAscii;
			if (KeyAscii == 0)
				e.Handled = true;
		}

		private void TextBox201_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(TextBox201.Text, out fTmp))
				return;

			IPoint pPtGeo = (IPoint)Functions.ToGeo(m_CurrPnt);
			Type2_CurDir = Functions.Azt2Dir(pPtGeo, fTmp);

			ConstructNextSegment();
		}

		#endregion

		#region Type 3

		void Type3_SetNewDirection(double NewDir)
		{
			if (!Functions.AngleInInterval(NewDir, Type3_CurrInterval))
			{
				if (Functions.SubtractAngles(Type3_CurrInterval.Left, NewDir) < Functions.SubtractAngles(Type3_CurrInterval.Right, NewDir))
					NewDir = Type3_CurrInterval.Left;
				else
					NewDir = Type3_CurrInterval.Right;
			}

			Type3_CurDir = NewDir;
			ConstructNextSegment();
		}

		private void ComboBox301_SelectedIndexChanged(object sender, EventArgs e)
		{
			int k = ComboBox301.SelectedIndex;
			if (k < 0)
				return;
			Type3_CurNav = (WPT_FIXType)ComboBox301.SelectedItem; //ComboBox301List[k];

			Label302.Text = Navaids_DataBase.GetNavTypeName(Type3_CurNav.TypeCode);

			int i = ComboBox302.SelectedIndex;
			ComboBox302.Enabled = ComboBox301_LIntervals[k].Right > 0.0 && ComboBox301_RIntervals[k].Right > 0.0;

			if (ComboBox301_LIntervals[k].Right < 0.0 && i == 0)
				ComboBox302.SelectedIndex = 1;
			else if (ComboBox301_RIntervals[k].Right < 0.0 && i == 1)
				ComboBox302.SelectedIndex = 0;

			if (i == ComboBox302.SelectedIndex)
				ComboBox302_SelectedIndexChanged(ComboBox302, new System.EventArgs());
		}

		private void ComboBox302_SelectedIndexChanged(object sender, EventArgs e)
		{
			double NavDir, TurnAngle;

			Interval inter;

			if (!IsLoaded)
				return;

			if (ComboBox302.SelectedIndex < 0)
				return;

			int k = ComboBox301.SelectedIndex;

			if (ComboBox302.SelectedIndex == 0)
			{
				Type3_CurTurnDir = 1;
				inter = ComboBox301_LIntervals[k];

				Type3_CurrInterval.Left = m_CurrDir + inter.Left;
				Type3_CurrInterval.Right = m_CurrDir + inter.Right;
			}
			else
			{
				Type3_CurTurnDir = -1;
				inter = ComboBox301_RIntervals[k];

				Type3_CurrInterval.Left = m_CurrDir - inter.Right;
				Type3_CurrInterval.Right = m_CurrDir - inter.Left;
			}

			int n = GlobalVars.WPTList.Length;

			ComboBox303.Items.Clear();
			for (int i = 0; i < n; i++)
			{
				if (GlobalVars.WPTList[i].Name != ComboBox301.Text)
				{
					if (Functions.SideDef(Type3_CurNav.pPtPrj, m_CurrDir, GlobalVars.WPTList[i].pPtPrj) == Type3_CurTurnDir)
						NavDir = Functions.ReturnAngleInDegrees(GlobalVars.WPTList[i].pPtPrj, Type3_CurNav.pPtPrj);
					else
						NavDir = Functions.ReturnAngleInDegrees(Type3_CurNav.pPtPrj, GlobalVars.WPTList[i].pPtPrj);

					TurnAngle = NativeMethods.Modulus(Type3_CurTurnDir * (NavDir - m_CurrDir));

					if (TurnAngle >= inter.Left && TurnAngle <= inter.Right)
						ComboBox303.Items.Add(GlobalVars.WPTList[i]);
				}
			}

			inter.Left = Functions.Dir2Azt(Type3_CurNav.pPtPrj, Type3_CurrInterval.Right);
			inter.Right = Functions.Dir2Azt(Type3_CurNav.pPtPrj, Type3_CurrInterval.Left);
			Label312.Text = FormatInterval(inter);
			CheckBox301.Enabled = ComboBox303.Items.Count > 0;

			if (CheckBox301.Enabled)
				ComboBox303.SelectedIndex = 0;
			else
				CheckBox301.Checked = false;

			if (!(CheckBox301.Checked && CheckBox301.Enabled))
				TextBox302_Validating(TextBox302, new System.ComponentModel.CancelEventArgs());
		}

		private void ComboBox303_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!(CheckBox301.Checked && CheckBox301.Enabled))
				return;

			int k = ComboBox303.SelectedIndex;
			if (k < 0)
				return;

			IPoint pToPoint = ((WPT_FIXType)ComboBox303.SelectedItem).pPtPrj; //ComboBox303List[K].pPtPrj;
			double NewDir;

			if (Functions.SideDef(Type3_CurNav.pPtPrj, m_CurrDir, pToPoint) == Type3_CurTurnDir)
				NewDir = Functions.ReturnAngleInDegrees(pToPoint, Type3_CurNav.pPtPrj);
			else
				NewDir = Functions.ReturnAngleInDegrees(Type3_CurNav.pPtPrj, pToPoint);

			Type3_SetNewDirection(NewDir);
		}

		private void CheckBox301_CheckedChanged(object sender, EventArgs e)
		{
			if (!CheckBox301.Checked)
			{
				ComboBox303.Enabled = false;
				TextBox302.ReadOnly = false;

				Label304.Visible = true;
			}
			else
			{
				Label304.Visible = true;
				ComboBox303.Enabled = true;

				TextBox302.ReadOnly = true;
				ComboBox303_SelectedIndexChanged(ComboBox303, new System.EventArgs());
			}
		}

		private void TextBox302_KeyPress(object sender, KeyPressEventArgs e)
		{
			char KeyAscii = e.KeyChar;
			if (KeyAscii == 13)
				TextBox302_Validating(TextBox302, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxInteger(ref KeyAscii);

			e.KeyChar = KeyAscii;
			if (KeyAscii == 0)
				e.Handled = true;
		}

		private void TextBox302_Validating(object sender, CancelEventArgs e)
		{
			double fTmp, fDir;

			if (!double.TryParse(TextBox302.Text, out fTmp))
				return;

			fDir = Functions.Azt2Dir(Type3_CurNav.pPtGeo, fTmp);
			Type3_SetNewDirection(fDir);
		}
		#endregion

		#region Type 4

		private void ComboBox401_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ComboBox401.SelectedIndex < 0)
				return;

			Type4_CurFix = (WPT_FIXType)ComboBox401.SelectedItem;   // ComboBox401List[ComboBox401.SelectedIndex];
			ConstructNextSegment();
		}

		private void ComboBox402_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ComboBox402.SelectedIndex < 0)
				return;

			if (ComboBox402.SelectedIndex == 0)
				Type4_CurTurnDir = 1;
			else
				Type4_CurTurnDir = -1;

			if (IsLoaded)
				ConstructNextSegment();
		}

		#endregion

		#region Type 5
		private void CheckBox501_CheckedChanged(object sender, EventArgs e)
		{
			if (!CheckBox501.Checked)
			{
				ComboBox503.Enabled = false;
				TextBox505.ReadOnly = false;

			}
			else
			{
				ComboBox503.Enabled = true;
				Label519.Visible = true;
				TextBox505.ReadOnly = true;


				int n = GlobalVars.WPTList.Length;

				ComboBox503.Items.Clear();
				for (int i = 0; i < n; i++)
					if (GlobalVars.WPTList[i].Name != ComboBox501.Text && Functions.ReturnDistanceInMeters(m_CurrPnt, GlobalVars.WPTList[i].pPtPrj) < GlobalVars.MaxNAVDist)
						ComboBox503.Items.Add(GlobalVars.WPTList[i]);

				if (ComboBox503.Items.Count > 0)
					ComboBox503.SelectedIndex = 0;
			}
		}

		private void ComboBox501_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ComboBox501.SelectedIndex < 0)
				return;

			Type5_CurNav = (WPT_FIXType)ComboBox501.SelectedItem;
			Label512.Text = Navaids_DataBase.GetNavTypeName(Type5_CurNav.TypeCode);

			TextBox506_Validating(TextBox506, new System.ComponentModel.CancelEventArgs());
		}

		private void ComboBox502_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ComboBox502.SelectedIndex < 0)
				return;

			if (ComboBox502.SelectedIndex == 0)
				Type5_CurTurnDir = 1;
			else
				Type5_CurTurnDir = -1;

			if (IsLoaded)
				TextBox506_Validating(TextBox506, new System.ComponentModel.CancelEventArgs());
		}

		private void ComboBox503_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ComboBox503.SelectedIndex < 0)
				return;

			IPoint pToPoint = ((WPT_FIXType)ComboBox503.SelectedItem).pPtPrj;
			Type5_CurDir = Functions.ReturnAngleInDegrees(Type5_CurNav.pPtPrj, pToPoint);
			ConstructNextSegment();
		}

		private void TextBox505_KeyPress(object sender, KeyPressEventArgs e)
		{
			char KeyAscii = e.KeyChar;
			if (KeyAscii == 13)
				TextBox505_Validating(TextBox505, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxInteger(ref KeyAscii);

			e.KeyChar = KeyAscii;
			if (KeyAscii == 0)
				e.Handled = true;
		}

		private void TextBox505_Validating(object sender, CancelEventArgs e)
		{
			double fCourse;
			if (!double.TryParse(TextBox505.Text, out fCourse))
				return;

			Type5_CurDir = Functions.Azt2Dir(Type5_CurNav.pPtGeo, fCourse + m_MagVar);
			ConstructNextSegment();
		}

		private void TextBox506_KeyPress(object sender, KeyPressEventArgs e)
		{
			char KeyAscii = e.KeyChar;
			if (KeyAscii == 13)
				TextBox506_Validating(TextBox506, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxInteger(ref KeyAscii);

			e.KeyChar = KeyAscii;
			if (KeyAscii == 0)
				e.Handled = true;
		}

		private void TextBox506_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;
			if (!double.TryParse(TextBox506.Text, out fTmp))
				return;

			Type5_SnapAngle = fTmp;

			if (Type5_SnapAngle < PANS_OPS_DataBase.dpInterMinAngle.Value)
				Type5_SnapAngle = PANS_OPS_DataBase.dpInterMinAngle.Value;
			else if (Type5_SnapAngle > PANS_OPS_DataBase.dpInterMaxAngle.Value)
				Type5_SnapAngle = PANS_OPS_DataBase.dpInterMaxAngle.Value;

			if (fTmp != Type5_SnapAngle)
				TextBox506.Text = Type5_SnapAngle.ToString();

			Type5UpdateIntervals();
		}
		#endregion

		#region Type 6

		private void ComboBox601_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!IsLoaded)
				return;
			FillComboBox603DMEStations();
			if (ComboBox603.Items.Count > 0)
				ComboBox603.SelectedIndex = 0;
		}

		private void ComboBox602_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!IsLoaded)
				return;
			FillComboBox603DMEStations();
			if (ComboBox603.Items.Count > 0)
				ComboBox603.SelectedIndex = 0;
		}

		private void ComboBox603_SelectedIndexChanged(object sender, EventArgs e)
		{
			int k = ComboBox603.SelectedIndex;
			if (k < 0)
				return;

			Type6_CurNav = (NavaidType)ComboBox603.SelectedItem;
			ToolTip1.SetToolTip(TextBox605, "Min: " + Functions.ConvertDistance(Type6_Intervals[k].Left, eRoundMode.NEAREST).ToString() +
											" Max: " + Functions.ConvertDistance(Type6_Intervals[k].Right, eRoundMode.NEAREST).ToString());

			TextBox605_Validating(TextBox605, new System.ComponentModel.CancelEventArgs());
		}

		private void TextBox605_KeyPress(object sender, KeyPressEventArgs e)
		{
			char KeyAscii = e.KeyChar;
			if (KeyAscii == 13)
				TextBox605_Validating(TextBox605, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref KeyAscii, TextBox605.Text);

			e.KeyChar = KeyAscii;
			if (KeyAscii == 0)
				e.Handled = true;
		}

		private void TextBox605_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(TextBox605.Text, out fTmp))
				return;

			double NewVal = Functions.DeConvertDistance(fTmp);

			fTmp = NewVal;
			int k = ComboBox603.SelectedIndex;
			//if (k < 0) return;
			Type6_CurNav = (NavaidType)ComboBox603.SelectedItem;

			if (NewVal < Type6_Intervals[k].Left)
				NewVal = Type6_Intervals[k].Left;
			else if (NewVal > Type6_Intervals[k].Right)
				NewVal = Type6_Intervals[k].Right;

			if (NewVal != fTmp)
				TextBox605.Text = Functions.ConvertDistance(NewVal, eRoundMode.NEAREST).ToString();

			ConstructNextSegment();
		}
		#endregion

		#region Type 7

		private void ComboBox701_SelectedIndexChanged(object sender, EventArgs e)
		{
			int k = ComboBox701.SelectedIndex;
			if (k < 0)
				return;

			Type7_CurNav = (NavaidType)ComboBox701.SelectedItem;    // ComboBox701List[K];
			if (Type7_Intervals[k].Left != 0 || Type7_Intervals[k].Right != 360)
				ToolTip1.SetToolTip(TextBox703, "Min: " + Functions.Dir2Azt(Type7_CurNav.pPtPrj, Type7_Intervals[k].Right).ToString("0.00") +
					" Max: " + Functions.Dir2Azt(Type7_CurNav.pPtPrj, Type7_Intervals[k].Left).ToString("0.00"));
			else
				ToolTip1.SetToolTip(TextBox703, "Min: 0 Max: 360");

			FillComboBox703List();
			if (OptionButton701.Checked)
			{
				TextBox703.Text = Functions.Dir2Azt(Type7_CurNav.pPtPrj, Type7_Intervals[k].Right).ToString("0.00");
				TextBox703_Validating(TextBox703, new System.ComponentModel.CancelEventArgs());
			}
		}

		private void ComboBox702_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!IsLoaded)
				return;

			FillComboBox701Stations();
		}

		private void ComboBox703_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!OptionButton702.Checked)
				return;

			int k = ComboBox703.SelectedIndex;
			if (k < 0)
				return;

			Type7_CurFix = (WPT_FIXType)ComboBox703.SelectedItem;   ///ComboBox703List[K];

			k = ComboBox701.SelectedIndex;
			if (k < 0)
				return;

			Label702.Text = Navaids_DataBase.GetNavTypeName(Type7_CurFix.TypeCode);

			double fTmp = Functions.ReturnAngleInDegrees(Type7_CurNav.pPtPrj, Type7_CurFix.pPtPrj);

			if (Type7_Intervals[k].Left == 0 && Type7_Intervals[k].Right == 360)
			{
				ComboBox704.Items.Clear();

				ComboBox704.Items.Add(Functions.Dir2Azt(Type7_CurNav.pPtPrj, fTmp).ToString("0.00"));
				ComboBox704.Items.Add(Functions.Dir2Azt(Type7_CurNav.pPtPrj, fTmp + 180.0).ToString("0.00"));

				//	TextBox703.Locked = true;
				//	TextBox703.BackColor = vbButtonFace;
				//	ComboBox704.Enabled = true;

				TextBox703.Visible = false;
				ComboBox704.Visible = true;
				ComboBox704.SelectedIndex = 0;
			}
			else
			{
				//	TextBox703.Locked = false;
				//	TextBox703.BackColor = vbWindowBackground;
				//	ComboBox704.Enabled = false;

				TextBox703.Visible = true;
				ComboBox704.Visible = false;
				double NewVal;

				if (Functions.AngleInSector(fTmp, Type7_Intervals[k].Left, Type7_Intervals[k].Right))
					NewVal = fTmp;
				else
					NewVal = fTmp + 180.0;

				TextBox703.Text = Functions.Dir2Azt(Type7_CurNav.pPtPrj, NewVal).ToString("0.00");
				TextBox703_Validating(TextBox703, new System.ComponentModel.CancelEventArgs());
			}
		}

		private void ComboBox704_SelectedIndexChanged(object sender, EventArgs e)
		{
			TextBox703.Text = ComboBox704.Text;
			TextBox703_Validating(TextBox703, new System.ComponentModel.CancelEventArgs());
		}

		private void OptionButton701_CheckedChanged(object sender, EventArgs e)
		{
			if (!OptionButton701.Checked)
				return;

			ComboBox703.Enabled = false;

			TextBox703.Visible = true;
			ComboBox704.Visible = false;

			TextBox703.ReadOnly = false;

			TextBox703_Validating(TextBox703, new System.ComponentModel.CancelEventArgs());
		}

		private void OptionButton702_CheckedChanged(object sender, EventArgs e)
		{
			if (!OptionButton702.Checked)
				return;

			ComboBox703.Enabled = true;
			ComboBox703_SelectedIndexChanged(ComboBox703, new System.EventArgs());
		}

		private void TextBox703_KeyPress(object sender, KeyPressEventArgs e)
		{
			char KeyAscii = e.KeyChar;
			if (KeyAscii == 13)
				TextBox703_Validating(TextBox703, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref KeyAscii, TextBox703.Text);

			e.KeyChar = KeyAscii;
			if (KeyAscii == 0)
				e.Handled = true;
		}

		private void TextBox703_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(TextBox703.Text, out fTmp))
				return;

			int k = ComboBox701.SelectedIndex;
			//if (K < 0) return;

			Type7_CurNav = (NavaidType)ComboBox701.SelectedItem;

			double NewVal = Functions.Azt2Dir(Type7_CurNav.pPtGeo, fTmp);

			if ((Type7_Intervals[k].Left != 0 || Type7_Intervals[k].Right != 360) && !Functions.AngleInSector(NewVal, Type7_Intervals[k].Left, Type7_Intervals[k].Right))
			{
				if (Functions.AnglesSideDef(NewVal, Type7_Intervals[k].Left) < 0)
					NewVal = Type7_Intervals[k].Left;
				else
					NewVal = Type7_Intervals[k].Right;

				TextBox703.Text = Functions.Dir2Azt(Type7_CurNav.pPtPrj, NewVal).ToString("0.00");
			}

			ConstructNextSegment();
		}

		#endregion

		#region Do :)

		void Do_Option001()
		{
			//OptionButton101.Enabled = true;
			OptionButton102.Enabled = FillComboBox101Stations() > 0;
			OptionButton103.Enabled = FillComboBox102Stations() > 0;

			//TextBox101.Visible = false;
			//Label102.Visible = false;
			//Label103.Visible = false;

			//ComboBox101.Visible = false;
			//ComboBox101.Items.Clear();
			//ComboBox101.Enabled = false;
			//OptionButton101.Checked = true;

			//ComboBox102.Visible = false;
			//TextBox102.Visible = false;
			//Label105.Visible = false;
			//Label106.Visible = false;
			//Label104.Visible = false;
			//Label107.Visible = false;

			OptionButton102.Checked = false;
			OptionButton103.Checked = false;

			if (OptionButton101.Checked)
				OptionButton101_CheckedChanged(OptionButton101, new System.EventArgs());
			else
				OptionButton101.Checked = true;

		}

		void Do_Option002()
		{
			TextBox201.Text = "0";
			TextBox201_Validating(TextBox201, new System.ComponentModel.CancelEventArgs());
		}

		void Do_Option003()
		{
			FillComboBox301Stations();
			if (ComboBox301.Items.Count > 0)
				ComboBox301.SelectedIndex = 0;
		}

		void Do_Option004()
		{
			FillComboBox401Stations();
			if (ComboBox401.Items.Count > 0)
				ComboBox401.SelectedIndex = 0;
		}

		void Do_Option005()
		{
			TextBox506.Text = "30";
			FillComboBox501Stations();

			if (ComboBox501.Items.Count > 0)
				ComboBox501.SelectedIndex = 0;
		}

		void Do_Option006()
		{
			FillComboBox603DMEStations();
			if (ComboBox603.Items.Count > 0)
				ComboBox603.SelectedIndex = 0;
		}

		void Do_Option007()
		{
			FillComboBox701Stations();

			if (OptionButton701.Checked)
				OptionButton701_CheckedChanged(OptionButton701, new System.EventArgs());
			else
				OptionButton701.Checked = true;
		}
		#endregion

		#region Type 1 Implimentation

		bool Type1SegmentOnDistance(double Dist, ref TraceSegment Segment)
		{
			IPoint PtOut = Functions.PointAlongPlane(m_CurrPnt, m_CurrDir, Dist);

			Segment.ptIn = m_CurrPnt;
			Segment.DirIn = m_CurrDir;

			Segment.ptOut = PtOut;
			Segment.DirOut = m_CurrDir;

			Segment.Length = Dist;
			Segment.Turn1R = 0.0;

			Segment.PathPrj = (IPolyline)new ESRI.ArcGIS.Geometry.Polyline();
			((IPointCollection)(Segment.PathPrj)).AddPoint(m_CurrPnt);
			((IPointCollection)(Segment.PathPrj)).AddPoint(PtOut);

			m_HFinish = Math.Min(CurrSegment.HStart + Segment.Length * m_PDG, cMaxH);

			Segment.Comment = Resources.str00500 + Functions.ConvertHeight(m_HFinish, eRoundMode.NEAREST).ToString() + " " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
			Segment.RepComment = Segment.Comment;

			Segment.SegmentCode = eSegmentType.straight;

			if (PrevSegment.SegmentCode == eSegmentType.courseIntercept || PrevSegment.SegmentCode == eSegmentType.turnAndIntercept || PrevSegment.SegmentCode == eSegmentType.arcPath)
				Segment.GuidanceNav = PrevSegment.InterceptionNav;
			else
				Segment.GuidanceNav = PrevSegment.GuidanceNav;

			Segment.InterceptionNav.TypeCode = eNavaidType.NONE;
			//public Navaid InterceptionNav;

			if (PrevSegment.SegmentCode == eSegmentType.courseIntercept || PrevSegment.SegmentCode == eSegmentType.directToFIX || PrevSegment.SegmentCode == eSegmentType.turnAndIntercept)
				Segment.LegType = Aim.Enums.CodeSegmentPath.CA;
			else
				Segment.LegType = Aim.Enums.CodeSegmentPath.VA;

			return true;
		}

		bool Type1SegmentOnWpt(WPT_FIXType WPtFix, ref TraceSegment Segment)
		{
			Segment.ptIn = m_CurrPnt;
			Segment.DirIn = m_CurrDir;

			Segment.ptOut = WPtFix.pPtPrj;
			Segment.DirOut = m_CurrDir;

			Segment.Length = Functions.ReturnDistanceInMeters(m_CurrPnt, WPtFix.pPtPrj);
			TextBox101.Text = Functions.ConvertDistance(Segment.Length, eRoundMode.FLOOR).ToString();

			Segment.Turn1R = 0.0;

			Segment.PathPrj = (IPolyline)new ESRI.ArcGIS.Geometry.Polyline();
			((IPointCollection)Segment.PathPrj).AddPoint(Segment.ptIn);
			((IPointCollection)Segment.PathPrj).AddPoint(Segment.ptOut);

			m_HFinish = Math.Min(CurrSegment.HStart + Segment.Length * m_PDG, cMaxH);

			Segment.Comment = Resources.str00503 + ComboBox101.Text;
			Segment.RepComment = Segment.Comment;

			if (PrevSegment.SegmentCode == eSegmentType.courseIntercept || PrevSegment.SegmentCode == eSegmentType.turnAndIntercept || PrevSegment.SegmentCode == eSegmentType.arcPath)
				Segment.GuidanceNav = PrevSegment.InterceptionNav;
			else
				Segment.GuidanceNav = PrevSegment.GuidanceNav;

			if (WPtFix.TypeCode != eNavaidType.NONE)
			{
				Segment.InterceptionNav = Navaids_DataBase.WPT_FIXToNavaid(WPtFix);
				Segment.InterceptionNav.IntersectionType = eIntersectionType.OnNavaid;
			}
			else
			{
				Segment.InterceptionNav.TypeCode = WPtFix.TypeCode;
				Segment.InterceptionNav.IntersectionType = (eIntersectionType)0;
			}

			Segment.SegmentCode = eSegmentType.straight;
			Segment.LegType = Aim.Enums.CodeSegmentPath.CF;

			return true;
		}

		bool Type1SegmentOnNewFIX(NavaidType Navaid, double NavDirDist, ref TraceSegment Segment)
		{
			IPoint FinalPt;
			double NewAztDist;

			if (Navaid.TypeCode == eNavaidType.DME)
			{
				if (Navaid.ValCnt != 0)
					NewAztDist = 90.0 * (Navaid.ValCnt - 1);
				else
				{
					int inx = 1 - ComboBox103.SelectedIndex;
					NewAztDist = 180.0 * inx;
				}

				Functions.CircleVectorIntersect(Navaid.pPtPrj, NavDirDist, m_CurrPnt, m_CurrDir + NewAztDist, out FinalPt);
			}
			else
			{
				FinalPt = new ESRI.ArcGIS.Geometry.Point();
				IConstructPoint ConstPt = (IConstructPoint)FinalPt;
				ConstPt.ConstructAngleIntersection(m_CurrPnt, Functions.DegToRad(m_CurrDir), Navaid.pPtPrj, Functions.DegToRad(NavDirDist));
			}

			if (FinalPt.IsEmpty)
				return false;

			//DrawPoint (FinalPt, RGB(0, 0, 255));
			//DrawPoint (m_CurrPnt, RGB(0, 255, 0));
			//DrawPoint (Navaid.pPtPrj, RGB(255, 0, 0));

			Segment.Length = Functions.ReturnDistanceInMeters(m_CurrPnt, FinalPt);
			TextBox101.Text = Functions.ConvertDistance(Segment.Length, eRoundMode.FLOOR).ToString();

			Segment.ptIn = m_CurrPnt;
			Segment.DirIn = m_CurrDir;

			Segment.ptOut = FinalPt;
			Segment.DirOut = Segment.DirIn;

			Segment.Turn1R = 0;

			Segment.PathPrj = (IPolyline)new ESRI.ArcGIS.Geometry.Polyline();
			((IPointCollection)Segment.PathPrj).AddPoint(m_CurrPnt);
			((IPointCollection)Segment.PathPrj).AddPoint(FinalPt);

			m_HFinish = Math.Min(CurrSegment.HStart + Segment.Length * m_PDG, cMaxH);

			Navaid.IntersectionType = eIntersectionType.ByAngle;

			if (Navaid.TypeCode == eNavaidType.VOR)
				Segment.Comment = Resources.str00501 + TextBox102.Text + " " + ComboBox102.Text;
			else if (Navaid.TypeCode == eNavaidType.NDB)
				Segment.Comment = Resources.str00502 + TextBox102.Text + " " + ComboBox102.Text;
			else if (Navaid.TypeCode == eNavaidType.DME)
			{
				Segment.Comment = Resources.str00523 + TextBox102.Text + " " + ComboBox102.Text;
				Navaid.IntersectionType = eIntersectionType.ByDistance;
			}

			Segment.RepComment = Segment.Comment;

			Segment.SegmentCode = eSegmentType.straight;

			if (PrevSegment.SegmentCode == eSegmentType.courseIntercept || PrevSegment.SegmentCode == eSegmentType.turnAndIntercept || PrevSegment.SegmentCode == eSegmentType.arcPath)
				Segment.GuidanceNav = PrevSegment.InterceptionNav;
			else
				Segment.GuidanceNav = PrevSegment.GuidanceNav;

			Segment.InterceptionNav = Navaid;

			if (PrevSegment.SegmentCode == eSegmentType.courseIntercept || PrevSegment.SegmentCode == eSegmentType.directToFIX || PrevSegment.SegmentCode == eSegmentType.turnAndIntercept)
			{
				if (Navaid.TypeCode == eNavaidType.DME)
					Segment.LegType = Aim.Enums.CodeSegmentPath.CD;
				else
					Segment.LegType = Aim.Enums.CodeSegmentPath.CR;
			}
			else if (Navaid.TypeCode == eNavaidType.DME)
				Segment.LegType = Aim.Enums.CodeSegmentPath.VD;
			else                            // if (Navaid.TypeCode == eNavaidType.CodeNDB)
				Segment.LegType = Aim.Enums.CodeSegmentPath.VR;

			return true;
		}

		#endregion

		#region Type 2 Implimentation

		bool Type2Segment(double OutDir, int TurnDir, ref TraceSegment Segment)
		{
			double TurnAngle = NativeMethods.Modulus((OutDir - m_CurrDir) * TurnDir);
			TextBox204.Text = TurnAngle.ToString("0.00");

			IPoint pPtCenter = Functions.PointAlongPlane(m_CurrPnt, m_CurrDir + 90.0 * TurnDir, m_TurnR);
			IPoint Pt1 = Functions.PointAlongPlane(pPtCenter, OutDir - 90.0 * TurnDir, m_TurnR);
			Pt1.M = OutDir;

			IPointCollection MyPC = new ESRI.ArcGIS.Geometry.Multipoint();
			MyPC.AddPoint(m_CurrPnt);
			MyPC.AddPoint(Pt1);

			//---------------------------------
			IPoint pPtXGeo = (IPoint)Functions.ToGeo(pPtCenter);
			Segment.Center1Coords = Functions.Degree2String((pPtXGeo.Y), Degree2StringMode.DMSLat) + ", " + Functions.Degree2String((pPtXGeo.X), Degree2StringMode.DMSLon);
			Segment.Turn1Dir = -TurnDir;    // SideDef(Pt0, Pt0.M, Pt1)
			Segment.Turn1Angle = TurnAngle;
			//---------------------------------

			Segment.PathPrj = Functions.CalcTrajectoryFromMultiPoint(MyPC);
			IPolyline MyPoly = (IPolyline)Segment.PathPrj;
			Segment.Length = MyPoly.Length;

			Segment.ptIn = m_CurrPnt;
			Segment.DirIn = m_CurrDir;

			Segment.ptOut = Pt1;
			Segment.DirOut = OutDir;

			Segment.Turn1R = m_TurnR;

			m_HFinish = Math.Min(CurrSegment.HStart + Segment.Length * m_PDG, cMaxH);

			double fTmp = double.Parse(TextBox201.Text);

			Segment.Comment = Resources.str00504 + TextBox001.Text + Resources.str00505 + TextBox201.Text;
			Segment.RepComment = Resources.str00504 + NativeMethods.Modulus(m_CurrAzt + m_MagVar).ToString("0.00") + Resources.str00505 + NativeMethods.Modulus(fTmp + m_MagVar).ToString("0.00");

			Segment.GuidanceNav.TypeCode = eNavaidType.NONE;
			Segment.InterceptionNav.TypeCode = eNavaidType.NONE;

			Segment.LegType = Aim.Enums.CodeSegmentPath.CF;
			Segment.SegmentCode = eSegmentType.toHeading;
			return true;
		}

		#endregion

		#region Type 3 Implimentation

		bool Type3Segment(WPT_FIXType Navaid, double NewDir, ref TraceSegment Segment)
		{
			double alpha, fRefX, fRefY, NewAzt, AddDist, fAbsRefY, TurnAngle;

			//IPoint pPtRef = Navaid.pPtPrj;

			Functions.PrjToLocal(m_CurrPnt, m_CurrDir, Navaid.pPtPrj.X, Navaid.pPtPrj.Y, out fRefX, out fRefY);
			fAbsRefY = System.Math.Abs(fRefY);

			alpha = GlobalVars.DegToRadValue * NativeMethods.Modulus(Type3_CurTurnDir * (NewDir - m_CurrDir));

			if (Type3_CurTurnDir * fRefY > 0.0)
				AddDist = fRefX - fAbsRefY / System.Math.Tan(alpha) - m_TurnR * System.Math.Tan(0.5 * alpha);
			else
				AddDist = fRefX + fAbsRefY / System.Math.Tan(alpha) - m_TurnR * System.Math.Tan(0.5 * alpha);

			IPointCollection MyPC = new ESRI.ArcGIS.Geometry.Multipoint();
			MyPC.AddPoint(m_CurrPnt);

			IPoint pPt0 = Functions.PointAlongPlane(m_CurrPnt, m_CurrDir, AddDist);
			pPt0.M = m_CurrDir;

			IPoint ptCenter = Functions.PointAlongPlane(pPt0, m_CurrDir + 90.0 * Type3_CurTurnDir, m_TurnR);

			IPoint pPt1 = Functions.PointAlongPlane(ptCenter, NewDir - 90.0 * Type3_CurTurnDir, m_TurnR);
			pPt1.M = NewDir;

			MyPC.AddPoint(pPt0);
			MyPC.AddPoint(pPt1);
			//---------------------------------
			TurnAngle = NativeMethods.Modulus(Type3_CurTurnDir * (NewDir - m_CurrDir));
			Segment.Turn1Angle = TurnAngle;
			Segment.Turn1Dir = -Type3_CurTurnDir; //SideDef(pPt0, m_CurrDir, pPt1)

			IPoint pPtTmpGeo = (IPoint)Functions.ToGeo(ptCenter);
			Segment.Center1Coords = Functions.Degree2String((pPtTmpGeo.Y), Degree2StringMode.DMSLat) + ", " + Functions.Degree2String((pPtTmpGeo.X), Degree2StringMode.DMSLon);

			pPtTmpGeo = (IPoint)Functions.ToGeo(pPt0);
			Segment.St1Coords = Functions.Degree2String((pPtTmpGeo.Y), Degree2StringMode.DMSLat) + ", " + Functions.Degree2String((pPtTmpGeo.X), Degree2StringMode.DMSLon);

			Segment.BetweenTurns = Functions.ReturnDistanceInMeters(m_CurrPnt, pPt0);
			Segment.H1 = CurrSegment.HStart + Segment.BetweenTurns * m_PDG;
			//---------------------------------
			Segment.PathPrj = Functions.CalcTrajectoryFromMultiPoint(MyPC);
			IPolyline MyPoly = (IPolyline)Segment.PathPrj;
			Segment.Length = MyPoly.Length;

			Segment.Turn1Length = Segment.Length - Segment.BetweenTurns;

			Segment.ptIn = m_CurrPnt;
			Segment.DirIn = m_CurrDir;

			Segment.ptOut = pPt1;
			Segment.DirOut = NewDir;

			Segment.Turn1R = m_TurnR;

			m_HFinish = Math.Min(CurrSegment.HStart + Segment.Length * m_PDG, cMaxH);

			NewAzt = Functions.Dir2Azt(Navaid.pPtPrj, Type3_CurDir);

			TextBox301.Text = TurnAngle.ToString("0.00");
			TextBox302.Text = NewAzt.ToString("0.00");
			TextBox303.Text = Functions.ConvertDistance(AddDist, eRoundMode.NEAREST).ToString();

			double fTmp = double.Parse(TextBox302.Text);
			Segment.Comment = Resources.str00506 + NativeMethods.Modulus(fTmp - m_MagVar).ToString("0.00") + " " + ComboBox301.Text;
			Segment.RepComment = Resources.str00506 + TextBox302.Text + " " + ComboBox301.Text;

			Segment.SegmentCode = eSegmentType.courseIntercept;

			switch (PrevSegment.LegType)
			{
				case Aim.Enums.CodeSegmentPath.TF:
				case Aim.Enums.CodeSegmentPath.CA:
				case Aim.Enums.CodeSegmentPath.CD:
				case Aim.Enums.CodeSegmentPath.CI:
				case Aim.Enums.CodeSegmentPath.CR:
				case Aim.Enums.CodeSegmentPath.CF:
				case Aim.Enums.CodeSegmentPath.DF:
					Segment.LegType = Aim.Enums.CodeSegmentPath.CI;
					break;
				default:
					Segment.LegType = Aim.Enums.CodeSegmentPath.VI;
					break;
			}

			if (PrevSegment.SegmentCode == eSegmentType.courseIntercept || PrevSegment.SegmentCode == eSegmentType.turnAndIntercept || PrevSegment.SegmentCode == eSegmentType.arcPath)
				Segment.GuidanceNav = PrevSegment.InterceptionNav;
			else
				Segment.GuidanceNav = PrevSegment.GuidanceNav;

			if (Navaid.TypeCode > eNavaidType.NONE)
			{
				Segment.InterceptionNav = Navaids_DataBase.WPT_FIXToNavaid(Navaid);

				if (Navaid.TypeCode == eNavaidType.VOR || Navaid.TypeCode == eNavaidType.NDB)
					Segment.InterceptionNav.IntersectionType = eIntersectionType.ByAngle;
				else if (Navaid.TypeCode == eNavaidType.DME)
					Segment.InterceptionNav.IntersectionType = eIntersectionType.ByDistance;
			}
			else
			{
				Segment.InterceptionNav.TypeCode = eNavaidType.NONE;
				Segment.InterceptionNav.IntersectionType = 0;
			}

			return true;
		}

		#endregion

		#region Type 4 Implimentation

		bool Type4Segment(WPT_FIXType WPtFix, int TurnDir, ref TraceSegment Segment)
		{
			double OutDir, TurnAngle;

			IPoint Pt1, pPtXGeo, pPtCenter;
			IPolyline MyPoly;
			IPointCollection MyPC;

			MyPC = Functions.TurnToFixPrj(m_CurrPnt, m_TurnR, TurnDir, Type4_CurFix.pPtPrj);
			Pt1 = MyPC.Point[1];
			OutDir = Pt1.M;

			//Functions.DrawPointWithText(Pt1, "PtCevr");
			//Application.DoEvents();

			TextBox401.Text = NativeMethods.Modulus(Functions.Dir2Azt(Pt1, OutDir)).ToString("0.00");

			TurnAngle = NativeMethods.Modulus((OutDir - m_CurrDir) * TurnDir);
			TextBox402.Text = TurnAngle.ToString("0.00");
			Label406.Text = Navaids_DataBase.GetNavTypeName(Type4_CurFix.TypeCode);

			//---------------------------------
			pPtCenter = Functions.PointAlongPlane(m_CurrPnt, m_CurrDir + 90.0 * TurnDir, m_TurnR);
			pPtXGeo = (IPoint)Functions.ToGeo(pPtCenter);

			Segment.Center1Coords = Functions.Degree2String((pPtXGeo.Y), Degree2StringMode.DMSLat) + ", " + Functions.Degree2String((pPtXGeo.X), Degree2StringMode.DMSLon);
			Segment.Turn1Dir = -TurnDir;        //	Functions.SideDef(m_CurrPnt, m_CurrPnt.M, Pt1);
			Segment.Turn1Angle = TurnAngle;     //	Modulus((MyPC.Point(0).M - MyPC.Point(1).M) * Segment.Turn1Dir);

			Segment.PathPrj = Functions.CalcTrajectoryFromMultiPoint(MyPC);
			MyPoly = (IPolyline)Segment.PathPrj;
			Segment.Length = MyPoly.Length;

			Segment.ptIn = m_CurrPnt;
			Segment.DirIn = m_CurrDir;

			Segment.ptOut = Pt1;
			Segment.DirOut = OutDir;

			Segment.Turn1R = m_TurnR;
			m_HFinish = Math.Min(CurrSegment.HStart + Segment.Length * m_PDG, cMaxH);

			Segment.Comment = Resources.str00504 + TextBox001.Text + Resources.str00505 + ComboBox401.Text;
			Segment.RepComment = Resources.str00504 + NativeMethods.Modulus(m_CurrAzt + m_MagVar).ToString("0.00") + Resources.str00505 + ComboBox401.Text;
			//---------------------------------

			Segment.SegmentCode = eSegmentType.directToFIX;
			Segment.LegType = Aim.Enums.CodeSegmentPath.DF;

			Segment.InterceptionNav.TypeCode = eNavaidType.NONE;

			if (WPtFix.TypeCode == eNavaidType.NONE)
				Segment.GuidanceNav.TypeCode = eNavaidType.NONE;
			else
				Segment.GuidanceNav = Navaids_DataBase.WPT_FIXToNavaid(WPtFix);

			return true;
		}

		#endregion

		#region Type 5 Implimentation

		void Type5UpdateIntervals()
		{
			//double MinDR;
			double VTotal, lMinDR, tStabl;
			double fTASl, delta, alpha, xMin, xMax;
			double Snap, fTmp, ddr, bAz, dr, d, R;

			tStabl = 6;
			Snap = Type5_SnapAngle;

			IPoint FixPntPrj = Type5_CurNav.pPtPrj;
			fTASl = Functions.IAS2TAS(m_IAS, CurrSegment.HStart, CurrADHP.ISAtC);
			VTotal = fTASl + CurrADHP.WindSpeed;

			//==========================================
			ddr = m_TurnR / (System.Math.Tan(GlobalVars.DegToRadValue * ((180.0 - Snap) * 0.5)));
			dr = PANS_OPS_DataBase.dpT_Gui_dist.Value + ddr;
			lMinDR = VTotal * tStabl * 0.277777777777778 + ddr;
			//MinDR = lMinDR - ddr;
			//TextBox001.ToolTipText = LoadResString(15250) + MinDR.ToString() + LoadResString(15251)     '"Минимальная длина DR = "" метр"
			//==========================================

			IPoint ptCnt = Functions.PointAlongPlane(m_CurrPnt, m_CurrDir + 90.0 * Type5_CurTurnDir, m_TurnR);
			bAz = Functions.ReturnAngleInDegrees(FixPntPrj, ptCnt);
			d = Functions.ReturnDistanceInMeters(ptCnt, FixPntPrj);

			R = System.Math.Sqrt(dr * dr + m_TurnR * m_TurnR);

			delta = GlobalVars.RadToDegValue * (System.Math.Atan(m_TurnR / dr));
			alpha = Snap - Type5_CurTurnDir * delta;

			fTmp = R * System.Math.Sin(GlobalVars.DegToRadValue * alpha) / d;
			if (fTmp > 1.0)
				xMin = 90.0;
			else if (fTmp < -1.0)
				xMin = -90.0;
			else
				xMin = GlobalVars.RadToDegValue * Math.Asin(fTmp);

			alpha = Snap + Type5_CurTurnDir * delta;
			fTmp = R * System.Math.Sin(GlobalVars.DegToRadValue * alpha) / d;
			if (fTmp > 1.0)
				xMax = 90.0;
			else if (fTmp < -1.0)
				xMax = -90.0;
			else
				xMax = GlobalVars.RadToDegValue * Math.Asin(fTmp);

			Type5_Intervals = new Interval[4];

			Type5_Intervals[0].Left = NativeMethods.Modulus(Functions.Dir2Azt(FixPntPrj, bAz + xMin) - m_MagVar);
			Type5_Intervals[1].Right = NativeMethods.Modulus(Functions.Dir2Azt(FixPntPrj, bAz - xMax) - m_MagVar);
			Type5_Intervals[2].Left = NativeMethods.Modulus(Functions.Dir2Azt(FixPntPrj, bAz + xMax) + 180.0 - m_MagVar);
			Type5_Intervals[3].Right = NativeMethods.Modulus(Functions.Dir2Azt(FixPntPrj, bAz - xMin) + 180.0 - m_MagVar);

			R = System.Math.Sqrt(lMinDR * lMinDR + m_TurnR * m_TurnR);

			delta = GlobalVars.RadToDegValue * (System.Math.Atan(m_TurnR / lMinDR));
			alpha = Snap - Type5_CurTurnDir * delta;

			fTmp = R * System.Math.Sin(GlobalVars.DegToRadValue * alpha) / d;
			if (fTmp > 1.0)
				xMin = 90.0;
			else if (fTmp < -1.0)
				xMin = -90.0;
			else
				xMin = GlobalVars.RadToDegValue * Math.Asin(fTmp);

			alpha = Snap + Type5_CurTurnDir * delta;
			fTmp = R * System.Math.Sin(GlobalVars.DegToRadValue * alpha) / d;
			if (fTmp > 1.0)
				xMax = 90.0;
			else if (fTmp < -1.0)
				xMax = -90.0;
			else
				xMax = GlobalVars.RadToDegValue * Math.Asin(fTmp);

			Type5_Intervals[0].Right = NativeMethods.Modulus(Functions.Dir2Azt(FixPntPrj, bAz + xMin) - m_MagVar);
			Type5_Intervals[1].Left = NativeMethods.Modulus(Functions.Dir2Azt(FixPntPrj, bAz - xMax) - m_MagVar);
			Type5_Intervals[2].Right = NativeMethods.Modulus(Functions.Dir2Azt(FixPntPrj, bAz + xMax) + 180.0 - m_MagVar);
			Type5_Intervals[3].Left = NativeMethods.Modulus(Functions.Dir2Azt(FixPntPrj, bAz - xMin) + 180.0 - m_MagVar);

			int i, j = 0, n = 4;//Type5_Intervals.Length;

			//SortIntervals Type5_Intervals

			while (j < n - 1)
			{
				if (Functions.SubtractAngles(Type5_Intervals[j].Right, Type5_Intervals[j + 1].Left) <= 1.0)
				{
					Type5_Intervals[j].Right = Type5_Intervals[j + 1].Right;
					n--;
					for (i = j + 1; i < n; i++)
						Type5_Intervals[i] = Type5_Intervals[i + 1];

				}
				else
					j++;
			}

			if (n > 1)
			{
				if (Functions.SubtractAngles(Type5_Intervals[0].Left, Type5_Intervals[n - 1].Right) <= 1.0)
				{
					//    If Type5_Intervals(0).Left = Type5_Intervals(N).Right Then
					Type5_Intervals[0].Left = Type5_Intervals[n - 1].Left;
					n--;
				}
			}

			Array.Resize<Interval>(ref Type5_Intervals, n);

			for (i = 0; i < n; i++)
			{
				if (Functions.SubtractAngles(System.Math.Round(Type5_Intervals[i].Left), System.Math.Round(Type5_Intervals[i].Right)) < 1.0)
				{
					Type5_Intervals[i].Left = System.Math.Round(Type5_Intervals[i].Left);
					Type5_Intervals[i].Right = Type5_Intervals[i].Left;
				}
				else
				{
					Type5_Intervals[i].Left = System.Math.Round(Type5_Intervals[i].Left + 0.4999);
					Type5_Intervals[i].Right = System.Math.Round(Type5_Intervals[i].Right - 0.4999);
				}
			}

			Functions.SortIntervals(Type5_Intervals);
			string tmpStr = "";

			for (i = 0; i < n; i++)
			{
				if (Functions.SubtractAngles(Type5_Intervals[i].Left, Type5_Intervals[i].Right) <= GlobalVars.degEps)
					tmpStr = Type5_Intervals[i].Left.ToString() + "°";
				else
					tmpStr = tmpStr + Resources.str13513 + Type5_Intervals[i].Left.ToString() + Resources.str13514 + Type5_Intervals[i].Right.ToString() + "°";

				//	if(i = 0) TextBox505.Text = Type5_Intervals[0].Left.ToString();

				if (i != n)
					tmpStr = tmpStr + "\r\n";

			}
			TextBox505_Validating(TextBox505, new System.ComponentModel.CancelEventArgs());
			Label516.Text = tmpStr;
		}

		bool Type5Segment(WPT_FIXType Navaid, ref double DirNav, int TurnDir, double SnapAngle, ref TraceSegment Segment)
		{
			double dDir;
			IPoint FlyBy;

			IPointCollection MyPC = Functions.CalcTouchByFixDir(m_CurrPnt, Navaid.pPtPrj, m_TurnR, m_CurrDir, ref DirNav, TurnDir, SnapAngle, out dDir, out FlyBy);

			int iToFacility = Functions.SideDef(MyPC.Point[MyPC.PointCount - 1], DirNav + 90.0, Navaid.pPtPrj);
			//DrawPointWithText MyPC.Point(MyPC.PointCount - 1), "pt1"

			if (iToFacility > 0)
				Label520.Text = Resources.str13515;
			else
				Label520.Text = Resources.str13516;

			TextBox503.Text = Functions.ConvertDistance(dDir, eRoundMode.NEAREST).ToString();

			double fCourse = Functions.Dir2Azt(Type5_CurNav.pPtPrj, Type5_CurDir);

			TextBox505.Text = NativeMethods.Modulus(fCourse - m_MagVar).ToString("0.00");

			//---------------------------------

			IPoint pPtX = CenterOfTurn(MyPC.Point[0], MyPC.Point[1]);

			IPoint pPtXGeo = (IPoint)Functions.ToGeo(pPtX);
			//Segment.PtCenter1 = pPtX;

			Segment.Center1Coords = Functions.Degree2String(pPtXGeo.Y, Degree2StringMode.DMSLat) + ", " + Functions.Degree2String(pPtXGeo.X, Degree2StringMode.DMSLon);
			Segment.Turn1Dir = Functions.SideDef(MyPC.Point[0], MyPC.Point[0].M, MyPC.Point[1]);
			Segment.Turn1Angle = NativeMethods.Modulus((MyPC.Point[0].M - MyPC.Point[1].M) * Segment.Turn1Dir);

			IPointCollection pPC = new ESRI.ArcGIS.Geometry.Polyline();
			pPC.AddPoint(MyPC.Point[0]);
			pPC.AddPoint(MyPC.Point[1]);
			IPolyline pPoly = Functions.CalcTrajectoryFromMultiPoint(pPC);

			Segment.H1 = CurrSegment.HStart + pPoly.Length * m_PDG;
			Segment.Turn1Length = pPoly.Length;

			if (MyPC.PointCount > 2)
			{
				pPtX = CenterOfTurn(MyPC.Point[2], MyPC.Point[3]);
				pPtXGeo = (IPoint)Functions.ToGeo(pPtX);

				//Segment.PtCenter2 = pPtX;
				Segment.Center2Coords = Functions.Degree2String(pPtXGeo.Y, Degree2StringMode.DMSLat) + ", " + Functions.Degree2String(pPtXGeo.X, Degree2StringMode.DMSLon);
				Segment.Turn2R = m_TurnR;
				Segment.Turn2Dir = Functions.SideDef(MyPC.Point[2], MyPC.Point[2].M, MyPC.Point[3]);
				Segment.Turn2Angle = NativeMethods.Modulus((MyPC.Point[2].M - MyPC.Point[3].M) * Segment.Turn2Dir);

				pPC = new ESRI.ArcGIS.Geometry.Polyline();
				pPC.AddPoint(MyPC.Point[2]);
				pPC.AddPoint(MyPC.Point[3]);
				pPoly = Functions.CalcTrajectoryFromMultiPoint(pPC);

				Segment.H2 = Segment.H1 + (dDir + pPoly.Length) * m_PDG;
				Segment.Turn2Length = pPoly.Length;

				Segment.PtCenter1 = MyPC.Point[1];
				pPtXGeo = (IPoint)Functions.ToGeo(MyPC.Point[1]);
				Segment.Fin1Coords = Functions.Degree2String(pPtXGeo.Y, Degree2StringMode.DMSLat) + ", " + Functions.Degree2String(pPtXGeo.X, Degree2StringMode.DMSLon);

				Segment.PtCenter2 = MyPC.Point[2];
				pPtXGeo = (IPoint)Functions.ToGeo(MyPC.Point[2]);
				Segment.St2Coords = Functions.Degree2String(pPtXGeo.Y, Degree2StringMode.DMSLat) + ", " + Functions.Degree2String(pPtXGeo.X, Degree2StringMode.DMSLon);

				Segment.BetweenTurns = dDir;
				Segment.DirBetween = MyPC.Point[1].M;
			}
			else
			{
				Segment.Turn2R = -1.0;
				//Segment.Turn2Length = 0.0;
				//Segment.BetweenTurns = 0.0;
				//Segment.DirBetween = 0.0;
			}
			//---------------------------------

			Segment.PathPrj = Functions.CalcTrajectoryFromMultiPoint(MyPC);
			pPC = (IPointCollection)Segment.PathPrj;

			Segment.Length = Segment.PathPrj.Length;

			Segment.ptIn = m_CurrPnt;
			Segment.DirIn = m_CurrDir;

			Segment.ptOut = pPC.Point[pPC.PointCount - 1];
			Segment.DirOut = Segment.ptOut.M;   //MyPC.Point[1].M;//

			Segment.Turn1R = m_TurnR;

			m_HFinish = Math.Min(CurrSegment.HStart + Segment.Length * m_PDG, cMaxH);

			if (!CheckBox501.Checked)
			{
				if (iToFacility > 0)
				{
					Segment.Comment = Resources.str00508 + TextBox505.Text + Resources.str00509 + ComboBox501.Text;
					Segment.RepComment = Resources.str00508 + NativeMethods.Modulus(fCourse + m_MagVar).ToString("0.00") + Resources.str00509 + ComboBox501.Text;
				}
				else
				{
					Segment.Comment = Resources.str00508 + TextBox505.Text + Resources.str00510 + ComboBox501.Text;
					Segment.RepComment = Resources.str00508 + NativeMethods.Modulus(fCourse + m_MagVar).ToString("0.00") + Resources.str00510 + ComboBox501.Text;
				}
			}
			else
			{
				Segment.Comment = Resources.str00508 + ComboBox501.Text + Resources.str00522 + ComboBox503.Text;
				Segment.RepComment = Resources.str00508 + NativeMethods.Modulus(fCourse + m_MagVar).ToString("0.00") + Resources.str00522 + ComboBox503.Text;
			}

			Segment.SegmentCode = eSegmentType.turnAndIntercept;
			Segment.LegType = Aim.Enums.CodeSegmentPath.CI;

			//Segment.InterceptionNav.TypeCode = eNavaidType.NONE;

			if (Navaid.TypeCode > eNavaidType.NONE)
			{
				Segment.InterceptionNav = Navaids_DataBase.WPT_FIXToNavaid(Navaid);
				Segment.InterceptionNav.IntersectionType = eIntersectionType.ByAngle;
			}
			else
			{
				Segment.InterceptionNav.TypeCode = eNavaidType.NONE;
				Segment.InterceptionNav.IntersectionType = (eIntersectionType)0;
			}

			Segment.GuidanceNav.TypeCode = eNavaidType.NONE;

			//if (Navaid.TypeCode == eNavaidType.NONE)
			//	Segment.GuidanceNav.TypeCode = eNavaidType.NONE;
			//else
			//	Segment.GuidanceNav = Navaids_DataBase.WPT_FIXToNavaid(Navaid);

			return true;
		}

		#endregion

		#region Type 6 Implimentation

		bool Type6Segment(NavaidType Nav, ref TraceSegment Segment)
		{
			int Side = 2 * ComboBox601.SelectedIndex - 1;
			int TurnDir = 2 * ComboBox602.SelectedIndex - 1;

			IPoint ptNav = Nav.pPtPrj;

			double phi = Functions.ReturnAngleInDegrees(m_CurrPnt, ptNav);
			double L = Functions.ReturnDistanceInMeters(m_CurrPnt, ptNav);
			double R = Functions.DeConvertDistance(double.Parse(TextBox605.Text));

			Segment.Turn2R = R;
			Segment.Turn2Dir = 2 * ComboBox602.SelectedIndex - 1;

			Segment.PtCenter2 = Nav.pPtPrj;

			double Bheta = m_CurrDir - phi;
			double alpha = Functions.RadToDeg(Math.Acos((Side * m_TurnR - TurnDir * L * System.Math.Sin(Functions.DegToRad(Bheta))) / (R + Side * m_TurnR)));
			double Gamma = m_CurrDir + Side * TurnDir * (90.0 - alpha) + 90.0 * (1 + Side);
			double Lp = R + Side * m_TurnR;

			IPoint ptCenter = Functions.PointAlongPlane(ptNav, Gamma, Lp);

			IPoint ptStart = Functions.PointAlongPlane(ptCenter, m_CurrDir + 90.0 * Side * TurnDir, m_TurnR);
			ptStart.M = m_CurrDir;

			IPoint ptEnd = Functions.PointAlongPlane(ptNav, Gamma, Lp - Side * m_TurnR);
			ptEnd.M = Gamma + 90.0 * TurnDir;

			//Xe = (yt - y0) * Sin(t) * Cos(t) + xt * Cos(t) ^ 2 + X0 * Sin(t) ^ 2;
			//Ye = y0 + (Xe - X0) * tg(t);

			IPointCollection MyPC = new ESRI.ArcGIS.Geometry.Multipoint();
			MyPC.AddPoint(m_CurrPnt);
			MyPC.AddPoint(ptStart);
			MyPC.AddPoint(ptEnd);

			IPolyline MyPoly = (IPolyline)Functions.CalcTrajectoryFromMultiPoint(MyPC);
			//---------------------------------
			IPoint pPtX = ptCenter;     //CenterOfTurn(MyPC.Point(0), MyPC.Point(1))
			IPoint pPtXGeo = (IPoint)Functions.ToGeo(pPtX);

			Segment.Center1Coords = Functions.Degree2String(pPtXGeo.Y, Degree2StringMode.DMSLat) + ", " + Functions.Degree2String(pPtXGeo.X, Degree2StringMode.DMSLon);
			Segment.Turn1Dir = Functions.SideDef(MyPC.Point[0], MyPC.Point[0].M, MyPC.Point[1]);
			Segment.Turn1Angle = NativeMethods.Modulus((MyPC.Point[0].M - MyPC.Point[1].M) * Segment.Turn1Dir);

			IPointCollection pPC = new ESRI.ArcGIS.Geometry.Polyline();
			pPC.AddPoint(MyPC.Point[0]);
			pPC.AddPoint(MyPC.Point[1]);

			IPolyline pPoly = Functions.CalcTrajectoryFromMultiPoint(pPC);

			Segment.H1 = CurrSegment.HStart + pPoly.Length * m_PDG;
			Segment.Turn1Length = pPoly.Length;
			//---------------------------------
			MyPC = (IPointCollection)MyPoly;

			Segment.PathPrj = MyPoly;
			Segment.Length = MyPoly.Length;

			Segment.ptIn = m_CurrPnt;
			Segment.DirIn = m_CurrDir;

			Segment.ptOut = ptEnd;
			Segment.DirOut = MyPC.Point[MyPC.PointCount - 1].M;

			Segment.Turn1R = m_TurnR;
			m_HFinish = Math.Min(CurrSegment.HStart + Segment.Length * m_PDG, cMaxH);

			Segment.Comment = "Arc intercept"; //Resources.str608 + ComboBox601.Text + Resources.str522 + ComboBox603.Text;
			Segment.RepComment = Segment.Comment;

			switch (PrevSegment.LegType)
			{
				case Aim.Enums.CodeSegmentPath.TF:
				case Aim.Enums.CodeSegmentPath.CA:
				case Aim.Enums.CodeSegmentPath.CD:
				case Aim.Enums.CodeSegmentPath.CI:
				case Aim.Enums.CodeSegmentPath.CR:
				case Aim.Enums.CodeSegmentPath.CF:
				case Aim.Enums.CodeSegmentPath.DF:
					Segment.LegType = Aim.Enums.CodeSegmentPath.CD;
					break;
				default:
					Segment.LegType = Aim.Enums.CodeSegmentPath.VD;
					break;
			}

			if (PrevSegment.SegmentCode == eSegmentType.courseIntercept || PrevSegment.SegmentCode == eSegmentType.turnAndIntercept || PrevSegment.SegmentCode == eSegmentType.arcPath)
				Segment.GuidanceNav = PrevSegment.InterceptionNav;
			else
				Segment.GuidanceNav = PrevSegment.GuidanceNav;

			Segment.InterceptionNav = Nav;
			if (Nav.TypeCode == eNavaidType.DME)
				Segment.InterceptionNav.IntersectionType = eIntersectionType.ByDistance;
			else
				Segment.InterceptionNav.IntersectionType = eIntersectionType.ByAngle;

			Segment.SegmentCode = eSegmentType.arcIntercept;

			return true;
		}

		#endregion

		#region Type 7 Implimentation

		bool Type7Segment(NavaidType pNav, ref TraceSegment Segment)
		{
			int ExitSide = 1 - 2 * ComboBox702.SelectedIndex;

			double ExitDir = Functions.Azt2Dir(pNav.pPtGeo, double.Parse(TextBox703.Text));
			double DirToArcCenter = Functions.ReturnAngleInDegrees(pNav.pPtPrj, PrevSegment.PtCenter2);
			double DistToArcCenter = Functions.ReturnDistanceInMeters(pNav.pPtPrj, PrevSegment.PtCenter2);

			double betta = Functions.DegToRad(ExitDir - DirToArcCenter);

			double TurnAngle = Functions.RadToDeg(Math.Acos((ExitSide * m_TurnR - PrevSegment.Turn2Dir * DistToArcCenter * System.Math.Sin(betta)) / (PrevSegment.Turn2R + ExitSide * m_TurnR)));
			double startRad = ExitDir - ExitSide * PrevSegment.Turn2Dir * (90.0 - TurnAngle) + 90.0 * (1 - ExitSide);

			IPoint ptStart = Functions.PointAlongPlane(PrevSegment.PtCenter2, startRad, PrevSegment.Turn2R);
			ptStart.M = startRad + PrevSegment.Turn2Dir * 90.0;

			IPoint ptCenter = Functions.PointAlongPlane(ptStart, startRad, ExitSide * m_TurnR);

			IPoint ptEnd = Functions.PointAlongPlane(ptCenter, ExitDir + PrevSegment.Turn2Dir * ExitSide * 90.0, m_TurnR);
			ptEnd.M = ExitDir;

			//DrawPointWithText ptEnd, "ptEnd"

			IPointCollection MyPC = new ESRI.ArcGIS.Geometry.Multipoint();
			MyPC.AddPoint(m_CurrPnt);
			MyPC.AddPoint(ptStart);
			MyPC.AddPoint(ptEnd);
			IPolyline MyPoly = (IPolyline)Functions.CalcTrajectoryFromMultiPoint(MyPC);

			//DrawPointWithText PrevSegment.PtCenter2, "PtCenter2"
			//DrawPointWithText Navaid.pPtPrj, "Nav", RGB(0, 0, 255)
			//ptCenter = PointAlongPlane(ptStart, startRad - 90, m_TurnR)
			//DrawPointWithText ptCenter, "ptCenter2"

			//ExitDir + turn*(90°- TurnAngle) +180 или более универсально,
			//ExitDir + ExitSide*PrevSegment.Turn2Dir*(90- TurnAngle) +90*(1 + ExitSide)
			//? - uchushun direksion istiqametidir. ? - donme bucagidir.
			//? = arcos{(side*r -turn* L*sin?)/(R + side*r)}
			//---------------------------------

			IPoint pPtX = ptCenter;     //CenterOfTurn(MyPC.Point(0), MyPC.Point(1))
			IPoint pPtXGeo = (IPoint)Functions.ToGeo(pPtX);

			Segment.Center1Coords = Functions.Degree2String(pPtXGeo.Y, Degree2StringMode.DMSLat) + ", " + Functions.Degree2String(pPtXGeo.X, Degree2StringMode.DMSLon);
			Segment.Turn1Dir = Functions.SideDef(MyPC.Point[0], MyPC.Point[0].M, MyPC.Point[1]);
			Segment.Turn1Angle = NativeMethods.Modulus((MyPC.Point[0].M - MyPC.Point[1].M) * Segment.Turn1Dir);

			IPointCollection pPC = new ESRI.ArcGIS.Geometry.Polyline();
			pPC.AddPoint(MyPC.Point[0]);
			pPC.AddPoint(MyPC.Point[1]);

			IPolyline pPoly = Functions.CalcTrajectoryFromMultiPoint(pPC);

			Segment.H1 = CurrSegment.HStart + pPoly.Length * m_PDG;
			Segment.Turn1Length = pPoly.Length;
			//---------------------------------

			Segment.PathPrj = MyPoly;
			Segment.Length = MyPoly.Length;

			Segment.ptIn = m_CurrPnt;
			Segment.DirIn = m_CurrDir;

			Segment.ptOut = ptEnd;
			Segment.DirOut = ExitDir;

			Segment.Turn2R = m_TurnR;

			m_HFinish = Math.Min(CurrSegment.HStart + Segment.Length * m_PDG, cMaxH);
			Segment.Comment = "Out from arc";   // LoadResString(608) + ComboBox601.Text + LoadResString(522) + ComboBox603.Text
			Segment.RepComment = Segment.Comment;

			Segment.SegmentCode = eSegmentType.arcPath;

			Segment.GuidanceNav = PrevSegment.InterceptionNav;
			Segment.InterceptionNav = pNav;

			Segment.LegType = Aim.Enums.CodeSegmentPath.AF;


			return true;
		}
		#endregion
	}
}
