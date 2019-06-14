using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
using Aran.Geometries;
using Aran.PANDA.RNAV.AdvancedPBN.Properties;
using Aran.Geometries.Operators;
using Aran.AranEnvironment.Symbols;
using Aran.Aim.Enums;

namespace Aran.PANDA.RNAV.SGBAS
{
	public partial class GBASForm : Form
	{
		#region Variable declarations

		private Label[] Label1;
		private InfoForm infoForm;

		private GBASReportForm reportFrm;
		private ProfileForm ArrivalProfile;

		private bool bFormInitialised = false;
		private bool bUnloadByOk;
		private bool _haveReport;

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
					//Label1[i].Visible = MultiPage1.TabPages.Item[i].Visible
					Label1[i].ForeColor = System.Drawing.Color.Silver;
					Label1[i].Font = new System.Drawing.Font(Label1[i].Font, System.Drawing.FontStyle.Regular);
				}

				Label1[value].ForeColor = System.Drawing.Color.FromArgb(0XFF8000);
				Label1[value].Font = new System.Drawing.Font(Label1[value].Font, System.Drawing.FontStyle.Bold);

				//this.Text = Resources.str00033 + "   " + MultiPage1.TabPages[StIndex].Text;

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

		private int PrevCmbRWY;
		private int Category;

		#region Page I

		private int ComboBox0002SelectedIndex;
		private double ArDir;
		private double RWYDir;
		//private double RWYCourse;
		private double GBASDir;
		private double GBASCourse;
		private double GPAngle;
		private double TanGPA;
		private double m_fMOC;
		private double GP_RDH;

		private double AlignP_THRMin;
		private double AlignP_THRMax;

		private double _zSurfaceOrigin;
		private double fMissAprPDG;

		//private double FPAP_THR;
		private double GARP_THR;
		private double AlignP_THR;
		private double FPAP_THR;

		private double fRDHOCH;
		//private double OCHMin;
		private double DistLLZ2THR;
		private double deltaRangeOffset;

		private eSBASCat _flightCategory;
		private double Ss;
		private double Vs;
		private bool bHaveSolution;

		private RWYType SelectedRWY;
		private RWYType[] SelctedRWYs;

		private ILSType ILS;

		private int FPAPElement;
		private int GARPElement;
		private int InterElement;

		private Aran.Geometries.Point FicTHRgeo;
		private Aran.Geometries.Point FicTHRprj;

		private Aran.Geometries.Point FPAPprj;
		private Aran.Geometries.Point GARPprj;

		private Aran.Geometries.Point ptInterGeo;
		private Aran.Geometries.Point ptInterPrj;

		private Polygon pCircle;

		private NavaidType[] InSectList;

		//private ObstacleContainer ObstacleList;
		private ObstacleContainer ILSObstacleList;
		private ObstacleContainer OFZObstacleList;
		private ObstacleContainer OASObstacleList;
		private ObstacleContainer WorkObstacleList;

		#endregion

		#region Page II

		private int Plane15Elem;
		private int FAP15FIXElem;
		private int FAPElem;
		private int IFFIXElem;
		private int IntermediateFullAreaElem;
		private int IntermediatePrimAreaElem;

		private ExcludeObstForm excludeObstFrm;
		private WPT_FIXType[] FixAngl;
		private NavaidType[] FAPNavDat;
		private D3DPolygone Plane15;

		private bool bNavFlg;
		private bool HaveExcluded;
		//FAP
		private MultiPolygon pFAPTolerArea;
		private MultiPolygon FAP15FIX;

		private MultiPolygon IntermediateFullArea;
		private MultiPolygon IntermediateWorkArea;
		private MultiPolygon IntermediatePrimArea;

		private MultiPolygon pIFPoly;
		private int IxOCH;

		private double FAPEarlierToler;
		private double fFAPDist;
		private double fMinFAPDist;
		private double fMaxFAPDist;
		private double fNearDist;
		private double fFarDist;

		private double arMinISlen;
		private double _hFAP;
		private double _CurrFAPOCH;
		private double fHalfFAPWidth;
		private Point ptFAP;

		#endregion

		#region Page III

		//=============================================
		private NavaidType[] IFNavDat;
		private Point IFprj;
		private double arImDescent_PDG;
		private double hDis;	//horizontalDist
		private double hIFFix;
		//=============================================
		private int ZContinuedFullElem;
		private int ZContinuedPrimElem;
		private int SOCElem;
		private int pMAPtElem;
		//private int MAHFElem;
		//private int MAHFTolerAreaElem;
		//private MAPt maptFix;

		private LegApch _zLeg;
		private MATF mahfFix;
		private FIX fictFix;
		
		private LineString LeftLine;
		private LineString RightLine;
		private LineString pFAPLine;
		private LineString pIFLine;

		private MultiPolygon pIFTolerArea;
		private MultiPolygon pIFFIXPoly;
		private MultiPolygon ZContinuedFull;
		private MultiPolygon ZContinuedPrim;
		private MultiPolygon pFullPoly;
		private MultiPolygon MAHFTolerArea;

		private Point PtCoordCntr;
		private Point PtSOC;
		private Point pMAPt;
		private Point ptMAHF;

		private ObstacleContainer MAObstacleList;
		private double fMisAprOCH;
		private double _MAPt2THRDist;
		private double _SOC2THRDist;
		private double XptLH;
		private double XptSOC;
		private double _straightMissedTermHeight;
		private double _minMAHFDist;
		private double _maxMAHFDist;
		private double _MAHF2THRDist;

		private double _minTermAlt;
		private double _maxTermAlt;
		private double _MAHFTerminationAlt;
		private double _EnRoteMOC;

		#endregion

		#region Page IV

		//private MATF matfFix;
		private Point TurnFixPnt;
		private ObstacleContainer DetTNHObstacles;
		private Interval[] m_TurnIntervals;
		private bool _TurnMoreThan15;
		private int TurnDir;

		private double _BankAngle;
		private double _TurnIAS;
		private double _TurnTAS;
		private double _TurnTurnR;
		private double _PlannedTurnAtMATF;

		private double fTNH;
		private double fHTurn;	//height
		private double hTurn;	//Altitude

		private int m_IxMinOCH;
		private int IxMaxOCH;

		#endregion

		#region Page V variables

		private CodeSegmentPath[] FlyOverPathAndTerminations =
				new CodeSegmentPath[] { CodeSegmentPath.TF, CodeSegmentPath.DF };
		private CodeSegmentPath[] FlyByPathAndTerminations =
				new CodeSegmentPath[] { CodeSegmentPath.TF };

		private ePBNClass[] firstTurnPBNClass = new ePBNClass[] { ePBNClass.RNP_APCH };
		private ePBNClass[] regularTurnPBNClass = new ePBNClass[] { ePBNClass.RNAV1, ePBNClass.RNAV2, ePBNClass.RNP1, ePBNClass.RNP4, ePBNClass.RNAV5 };

		List<WayPoint> _SignificantPoints;
		private Interval UpDown401Range;
		private Transitions transitions;
		//private WayPoint _AddedFIX;

		#endregion

		#endregion

		#region MainForm

		public GBASForm()
		{
			InitializeComponent();

			Label1 = new Label[] { Label01, Label02, Label03, Label04};
			infoForm = new InfoForm();

			reportFrm = new GBASReportForm();
			reportFrm.Init(ReportButton);
			ArrivalProfile = new ProfileForm();

			excludeObstFrm = new ExcludeObstForm();
			//reportForm.SetUnVisible(-1);

			bFormInitialised = true;
			pCircle = new Polygon();
			bUnloadByOk = false;

			GlobalVars.OASPlanesCat1State = GlobalVars.ILSPlanesState = true;
			//GlobalVars.OASPlanesCat1State = true;
			//GlobalVars.ILSPlanesState = false;

			arImDescent_PDG = GlobalVars.constants.Pansops[ePANSOPSData.arImDescent_Max].Value;
			TextBox0203.Text = (100.0 * arImDescent_PDG).ToString("0.0");

			TextBox0206.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value, eRoundMode.NEAREST).ToString();

			//RModel = MinCalcAreaRadius
			//hCons = GlobalVars.constants.Pansops[ePANSOPSData.arMATurnAlt].Value;

			int i;
			for (i = 0; i < Label1.Length; i++)
				Label1[i].Text = MultiPage1.TabPages[i].Text;

			PrevCmbRWY = -1;

			//=====================================================================
			Label0001_02.Text = GlobalVars.unitConverter.HeightUnit;
			Label0001_06.Text = GlobalVars.unitConverter.HeightUnitM;
			Label0001_08.Text = GlobalVars.unitConverter.HeightUnitM;
			Label0001_10.Text = GlobalVars.unitConverter.HeightUnitM;
			Label0001_12.Text = GlobalVars.unitConverter.HeightUnitM;
			Label0001_17.Text = GlobalVars.unitConverter.HeightUnit;
			Label0001_21.Text = GlobalVars.unitConverter.HeightUnitM;
			Label0001_23.Text = GlobalVars.unitConverter.HeightUnit;
			Label0001_25.Text = GlobalVars.unitConverter.HeightUnitM;
			Label0001_27.Text = GlobalVars.unitConverter.HeightUnitM;
			Label0001_31.Text = GlobalVars.unitConverter.HeightUnitM;

			//=====================================================================
			_Label0101_8.Text = GlobalVars.unitConverter.DistanceUnit;
			_Label0101_9.Text = GlobalVars.unitConverter.HeightUnit;
			_Label0101_4.Text = GlobalVars.unitConverter.HeightUnit;
			_Label0101_7.Text = GlobalVars.unitConverter.HeightUnit;
			_Label0101_10.Text = GlobalVars.unitConverter.DistanceUnit;
			//=====================================================================
			_Label0201_8.Text = GlobalVars.unitConverter.HeightUnit;
			_Label0201_10.Text = GlobalVars.unitConverter.DistanceUnit;
			_Label0201_11.Text = GlobalVars.unitConverter.DistanceUnit;
			_Label0201_13.Text = GlobalVars.unitConverter.DistanceUnit;
			_Label0201_15.Text = GlobalVars.unitConverter.HeightUnit;

			//=====================================================================
			Label0301_02.Text = GlobalVars.unitConverter.DistanceUnit;
			Label0301_04.Text = GlobalVars.unitConverter.DistanceUnit;
			Label0301_06.Text = GlobalVars.unitConverter.HeightUnit;
			Label0301_09.Text = GlobalVars.unitConverter.HeightUnit;
			Label0301_12.Text = GlobalVars.unitConverter.HeightUnit;
			Label0301_14.Text = GlobalVars.unitConverter.HeightUnit;
			Label0301_16.Text = GlobalVars.unitConverter.HeightUnit;
			Label0301_18.Text = GlobalVars.unitConverter.DistanceUnit;
			//=====================================================================

			//_Label0901_1.Text = GlobalVars.unitConverter.DistanceUnit;
            //_Label0901_3.Text = GlobalVars.unitConverter.HeightUnit;
            //_Label0901_5.Text = GlobalVars.unitConverter.HeightUnit;
            //_Label0901_7.Text = GlobalVars.unitConverter.HeightUnit;
            //_Label0901_10.Text = GlobalVars.unitConverter.HeightUnit;
            //_Label0901_13.Text = GlobalVars.unitConverter.HeightUnit;
            //_Label0901_21.Text = GlobalVars.unitConverter.DistanceUnit;
            //_Label0901_22.Text = GlobalVars.unitConverter.HeightUnit;

			GP_RDH = GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value;
			TextBox0004.Text = GlobalVars.unitConverter.HeightToDisplayUnits(GP_RDH, eRoundMode.NEAREST).ToString();
			TextBox0011.Text = "210";

			//CreateLog(My.Resources.str2)
			ShowPanelBtn.Checked = false;

			MultiPage1.Top = -21;
			Frame1.Top = Frame1.Top - 21;
			Height = Height - 21;

			CurrPage = 0;

			///=====================================
			int n = GlobalVars.RWYList.Length;

			ComboBox0001.Items.Clear();
			SelctedRWYs = new RWYType[n];

			for (i = 0; i < n; i++)
			{
				SelctedRWYs[i] = GlobalVars.RWYList[i];
				ComboBox0001.Items.Add(SelctedRWYs[i].Name);
			}

			NextBtn.Enabled = n >= 0;

			TextBox0010_Validating(TextBox0010, null);

			if (n > 0)
				ComboBox0001.SelectedIndex = 0;
			else
			{
				//Array.Resize<RWYType>(ref SelctedRWYs , 0);
				//		throw new Exception("There is no ILS for the specified ADHP.");

				MessageBox.Show("There is no RWY for the specified ADHP.", "PANDA",
					System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				Close();
				return;
			}

			//double maxRange = GlobalVars.ModellingRadius + Functions.CalcMaxRadius();
			DBModule.GetObstaclesByDist(out GlobalVars.ObstacleList, GlobalVars.CurrADHP.pPtPrj, GlobalVars.ModellingRadius);

			ComboBox0002SelectedIndex = 0;
			_flightCategory = eSBASCat.CategoryI;

			ComboBox0003.SelectedIndex = 0;

			ComboBox0301.Items.Clear();
			//ComboBox0402.Items.Clear();

			for (i = 0; i < GlobalVars.EnrouteMOCValues.Length; i++)
			{
				ComboBox0301.Items.Add((GlobalVars.unitConverter.HeightToDisplayUnits(GlobalVars.EnrouteMOCValues[i], eRoundMode.SPECIAL_CEIL)));
				//comboBox0402.Items.Add((GlobalVars.unitConverter.HeightToDisplayUnits(GlobalVars.EnrouteMOCValues[i], eRoundMode.SPECIAL_CEIL)));
			}

			ComboBox0301.SelectedIndex = 0;
			//comboBox0402.SelectedIndex = 0;

			//ComboBox0301.SelectedIndex = 0;
			//ComboBox0303.SelectedIndex = 1;
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			DBModule.CloseDB();
			ClearGraphics();
			reportFrm.Close();
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

		#region Common Form Events
		private void TextBoxes_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				e.IsInputKey = true;
		}

		private void ShowPanelBtn_CheckedChanged(object sender, EventArgs e)
		{
			if (!bFormInitialised)
				return;

			if (ShowPanelBtn.Checked)
			{
				this.Width = 758;
				ShowPanelBtn.Image = Resources.bmpHIDE_INFO;
			}
			else
			{
				this.Width = 571;
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

			switch (MultiPage1.SelectedIndex)
			{
				case 1:
					preparePageI();
					break;
				case 2:
					break;
				case 3:
					break;
				case 4:
					//leavePageV();
					break;
				case 5:
					break;
			}

			this.CurrPage = MultiPage1.SelectedIndex - 1;

			NativeMethods.HidePandaBox();
			//this.HelpContextID = 4000 + 100 * (MultiPage1.SelectedIndex + 1);
		}

		private void NextBtn_Click(object sender, EventArgs e)
		{
			NativeMethods.ShowPandaBox(this.Handle.ToInt32());
			try
			{
				switch (MultiPage1.SelectedIndex)
				{
					case 0:
						if (!preparePageII())
						{
							NativeMethods.HidePandaBox();
							return;
						}
						break;

					case 1:
						if (!preparePageIII())
						{
							NativeMethods.HidePandaBox();
							return;
						}
						break;

					case 2:
						preparePageIV();
						break;

					case 3:
						//preparePageV();
						break;
				}

				this.CurrPage = MultiPage1.SelectedIndex + 1;
			}
			finally
			{
				NativeMethods.HidePandaBox();
			}
		}

		private void OkBtn_Click(object sender, EventArgs e)
		{

		}

		private void CancelBtn_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void ReportButton_CheckedChanged(object sender, EventArgs e)
		{
			if (!bFormInitialised) return;

			if (ReportButton.Checked)
				reportFrm.Show(GlobalVars.Win32Window);
			else
				reportFrm.Hide();
		}

		private void ProfileBtn_CheckedChanged(object sender, EventArgs e)
		{

		}

		#endregion

		#region Utilities

		private void ClearGraphics()
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(FPAPElement);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(GARPElement);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(InterElement);
			//GlobalVars.gAranGraphics.SafeDeleteGraphic(FAPElem);
			//GlobalVars.gAranGraphics.SafeDeleteGraphic(IFElem);

			//GlobalVars.gAranGraphics.SafeDeleteGraphic(IntermSecondaryAreaElem);
			//GlobalVars.gAranGraphics.SafeDeleteGraphic(IntermPrimaryAreaElem);

			GlobalVars.gAranGraphics.SafeDeleteGraphic(ZContinuedFullElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(ZContinuedPrimElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(SOCElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(pMAPtElem);

			if (mahfFix != null)
				mahfFix.DeleteGraphics();

			int n = GlobalVars.OFZPlanesElement.Length;
			for (int i = 0; i < n; i++)
				GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.OFZPlanesElement[i]);

			n = GlobalVars.OASPlanesCat1Element.Length;
			for (int i = 0; i < n; i++)
				GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.OASPlanesCat1Element[i]);

			n = GlobalVars.ILSPlanesElement.Length;
			for (int i = 0; i < n; i++)
				GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.ILSPlanesElement[i]);
		}

		double FAPDist2hFAP(double Dist)
		{
			return GP_RDH + TanGPA * Dist + 0.0785 * 0.000001 * Dist * Dist;
			//0.0000000785;
			//7.85e-008
		}

		double hFAP2FAPDist(double Hrel)
		{
			const double kA = 2.0 * 0.0785 * 0.000001;

			double dD = GlobalVars.constants.Pansops[ePANSOPSData.arCurvatureCoeff].Value *
							(GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value - GP_RDH) / TanGPA;

			return (System.Math.Sqrt(TanGPA * TanGPA + 2.0 * kA * (Hrel - GP_RDH)) - TanGPA) / kA - dD;
		}

		void WPTInSector(Point pRefPt, WPT_FIXType[] InList, out NavaidType[] OutList)
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

		private void preparePageI()
		{

		}

		private bool CalculatePageI()
		{
			infoForm.ResetTHRFields();

			if (ComboBox0001.SelectedIndex < 0)
				return false;

			double BaseDist;
			//infoForm.SetDeltaAngle(0);

			deltaRangeOffset = 0.0;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(FPAPElement);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(GARPElement);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(InterElement);
			//GlobalVars.gAranGraphics.SafeDeleteGraphic(FAPElem);

			//GlobalVars.gAranGraphics.SafeDeleteGraphic(IFElem);
			//GlobalVars.gAranGraphics.SafeDeleteGraphic(IntermSecondaryAreaElem);
			//GlobalVars.gAranGraphics.SafeDeleteGraphic(IntermPrimaryAreaElem);

			FPAPElement = GARPElement = InterElement = -1;

			if (cbIgnoreILS.Checked)
			{
				BaseDist = AlignP_THR;

				ptInterPrj = ARANFunctions.LocalToPrj(SelectedRWY.pPtPrj[eRWY.ptTHR], RWYDir + Math.PI, BaseDist);

				FicTHRprj = ARANFunctions.LocalToPrj(ptInterPrj, GBASDir, BaseDist);
				//ARANFunctions.PrjToLocal(pPtAlign, SBASDir, SelectedRWY.pPtPrj[eRWY.PtTHR], out BaseDist, out y);
				//FicTHRprj = ARANFunctions.LocalToPrj(SelectedRWY.pPtPrj[eRWY.PtStart], SBASDir, BaseDist);

				FPAPprj = ARANFunctions.LocalToPrj(FicTHRprj, GBASDir, FPAP_THR);
				GARPprj = ARANFunctions.LocalToPrj(FPAPprj, GBASDir, 305.0);

				GARP_THR = FPAP_THR + 305.0;

				TextBox0013.ReadOnly = false;
				TextBox0010.ReadOnly = false;

				fRDHOCH = GP_RDH + BaseDist * TanGPA + GlobalVars.LocOffsetOCHAdd;

				//InterElement = GlobalVars.gAranGraphics.DrawPointWithText(ptInterPrj, ARANFunctions.RGB(0, 250, 255), "Intersection");
			}
			else
			{
				GBASDir = ILS.pPtPrj.M;
				GBASCourse = ILS.pPtGeo.M;

				GPAngle = ILS.GPAngle;
				TanGPA = System.Math.Tan(GPAngle);

				//GARP_THR = ILS.LLZ_THR;
				double MaxCLShift = 65.0;	//1.0;

				TextBox0013.ReadOnly = true;
				TextBox0010.ReadOnly = true;

				TextBox0013.Text = GBASCourse.ToString("0.00");
				TextBox0010.Text = System.Math.Round(ARANMath.RadToDegValue * GPAngle, 2).ToString("0.00");

				Geometry geom = ARANFunctions.LineLineIntersect(SelectedRWY.pPtPrj[eRWY.ptTHR], RWYDir, ILS.pPtPrj, GBASDir);
				double x = 0.0, y;
				double fTmp = GBASDir - RWYDir;
				if (fTmp < 0.0) fTmp = fTmp + 2.0 * Math.PI;
				if (fTmp > Math.PI) fTmp = 2.0 * Math.PI - fTmp;

				infoForm.SetDeltaAngle(fTmp);

				if (geom == null || geom.Type != GeometryType.Point || fTmp < ARANMath.DegToRadValue)
				{
					ARANFunctions.PrjToLocal(ILS.pPtPrj, GBASDir, SelectedRWY.pPtPrj[eRWY.ptTHR], out x, out y);
					if (Math.Abs(y) > MaxCLShift)
					{
						MessageBox.Show("LOC bearing is parallel to RWY bearing.");
						ComboBox0001.SelectedIndex = PrevCmbRWY;
						return false;
					}

					geom = new Point();
				}

				ptInterPrj = (Point)geom;

				//for(int i = 0 ; i< ILS.MkrList.Lenght; i++)
				//{
				//    ILS.MkrList[i].DistFromTHR =  ARANFunctions.Point2LineDistancePrj(SelectedRWY.pPtPrj[eRWY.PtTHR], ILS.MkrList[i].pPtPrj, ILSDir + 0.5*Math.PI);
				//    ILS.MkrList[i].Height = FAPDist2hFAP(ILS.MkrList[i].DistFromTHR);
				//    ILS.MkrList[i].Altitude = SelectedRWY.pPtGeo[eRWY.PtTHR].Z + ILS.MkrList[i].Height;
				//}

				//if (fTmp >= ARANMath.DegToRadValue && pPtAlign.IsEmpty)
				//{
				//    MessageBox.Show("Invalid ILS");
				//    ComboBox0001.SelectedIndex = PrevCmbRWY;
				//    return false;
				//}

				if (ptInterPrj.IsEmpty)
				{
					BaseDist = AlignP_THRMin;
					FicTHRprj = ARANFunctions.LocalToPrj(ILS.pPtPrj, GBASDir, x);
					ptInterPrj = FicTHRprj;
					fRDHOCH = GP_RDH;
				}
				else
				{
					ARANFunctions.PrjToLocal(ptInterPrj, RWYDir, SelectedRWY.pPtPrj[eRWY.ptTHR], out BaseDist, out y);
					if (BaseDist < AlignP_THRMin || BaseDist > AlignP_THRMax)
					{
						MessageBox.Show("LOC and RWY bearings intersection point is out of permissible range.");
						ComboBox0001.SelectedIndex = PrevCmbRWY;
						return false;
					}

					FicTHRprj = ARANFunctions.LocalToPrj(ptInterPrj, GBASDir, BaseDist);
					fRDHOCH = GP_RDH + BaseDist * TanGPA + GlobalVars.LocOffsetOCHAdd;

					infoForm.SetIntersectDistance(BaseDist);
					InterElement = GlobalVars.gAranGraphics.DrawPointWithText(ptInterPrj, "Intersection", ARANFunctions.RGB(0, 250, 255));
				}

				double Loc_THR = ARANFunctions.ReturnDistanceInMeters(ILS.pPtPrj, FicTHRprj);

				if (Loc_THR >= FPAP_THR + 305.0)
				{
					GARPprj = ILS.pPtPrj;
					FPAPprj = ARANFunctions.LocalToPrj(GARPprj, GBASDir + Math.PI, 305.0);
					deltaRangeOffset = Loc_THR - FPAP_THR - 305.0;
				}
				else
				{
					FPAPprj = ARANFunctions.LocalToPrj(FicTHRprj, GBASDir, FPAP_THR);
					GARPprj = ARANFunctions.LocalToPrj(FPAPprj, GBASDir, 305.0);
				}

				GARP_THR = ARANFunctions.ReturnDistanceInMeters(GARPprj, FicTHRprj);
				//?????????????????????????????????????
				ARANFunctions.PrjToLocal(SelectedRWY.pPtPrj[eRWY.ptTHR], GBASDir, GARPprj, out x, out y);
				infoForm.SetLocAlongDist(x);
				infoForm.SetLocAbeamDist(y);
			}
			//=====================================================================

			//if (ComboBox0005.SelectedIndex < 0)
			//{
			//    ComboBox0005.SelectedIndex = 1;
			//    return false;
			//}

			FicTHRprj.Z = SelectedRWY.pPtPrj[eRWY.ptTHR].Z;
			FicTHRprj.M = GBASDir;											//Azt2Dir(ptLH, ptLH.M)
			ArDir = GBASDir;

			ComboBox0003_SelectedIndexChanged(ComboBox0003, null);
			ComboBox0002_SelectedIndexChanged(ComboBox0002SelectedIndex, null);

			FPAPElement = GlobalVars.gAranGraphics.DrawPointWithText(FPAPprj, "FPAP", ARANFunctions.RGB(0, 250, 255));
			GARPElement = GlobalVars.gAranGraphics.DrawPointWithText(GARPprj, "GARP", ARANFunctions.RGB(0, 250, 255));

			FicTHRgeo = GlobalVars.pspatialReferenceOperation.ToGeo<Point>(FicTHRprj);
			FicTHRgeo.M = GBASCourse;

			ptInterGeo = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(ptInterPrj);

			//ILS.SecWidth = ILS.LLZ_THR * Tan(DegToRad(ILS.AngleWidth))

			DistLLZ2THR = GARP_THR;
			TextBox0003.Text = Math.Round(DistLLZ2THR).ToString();
			TextBox0012.Text = Math.Round(deltaRangeOffset).ToString();

			double FPAP2THR = ARANFunctions.ReturnDistanceInMeters(FicTHRprj, FPAPprj);
			TextBox0009.Text = Math.Round(FPAP2THR).ToString();

			infoForm.SetOCHLimit(fRDHOCH);

			//CreateILS23Planes(FicTHRprj, ArDir, 45.0, ILS23Planes)
			//n = UBound(ILS23Planes)
			//For i = 0 To n
			//	If Not ILS23PlanesElement(i) Is Nothing Then pGraphics.DeleteElement(ILS23PlanesElement(i))
			//	ILS23PlanesElement(i) = DrawPolygon(ILS23Planes(i).Poly, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSHollow, ILS23PlaneState)
			//	ILS23PlanesElement(i).Locked = True
			//Next i
			//CommandBar.isEnabled(CommandBar.ILS23) = True

			return true;
		}

		private void THRInfoBtn_Click(object sender, EventArgs e)
		{
			infoForm.ShowTHRInfo(this.Left + MultiPage1.Left + THRInfoBtn.Left, this.Top + MultiPage1.Top + THRInfoBtn.Top + THRInfoBtn.Height);
		}

		private void ComboBox0001_SelectedIndexChanged(object sender, EventArgs e)
		{
			int RWYIndex = ComboBox0001.SelectedIndex;
			if (RWYIndex < 0)
				return;

			SelectedRWY = GlobalVars.RWYList[RWYIndex];

			TextBox0001.Text = GlobalVars.unitConverter.HeightToDisplayUnits(SelectedRWY.pPtPrj[eRWY.ptTHR].Z, eRoundMode.NEAREST).ToString();
			TextBox0002.Text = SelectedRWY.pPtGeo[eRWY.ptTHR].M.ToString("0.00");
			TextBox0008.Text = Math.Round(SelectedRWY.ASDA).ToString();

			RWYDir = SelectedRWY.pPtPrj[eRWY.ptTHR].M;

			double x, y;
			ARANFunctions.PrjToLocal(SelectedRWY.pPtPrj[eRWY.ptStart], RWYDir, SelectedRWY.pPtPrj[eRWY.ptTHR], out x, out y);
			FPAPprj = ARANFunctions.LocalToPrj(SelectedRWY.pPtPrj[eRWY.ptStart], RWYDir, SelectedRWY.ASDA, y);

			ARANFunctions.PrjToLocal(SelectedRWY.pPtPrj[eRWY.ptTHR], RWYDir, FPAPprj, out FPAP_THR, out y);

			DBModule.GetILS(SelectedRWY, GlobalVars.CurrADHP, out ILS);

			cbIgnoreILS.Enabled = (ILS.index & 1) == 1;

			if (!cbIgnoreILS.Enabled)
				cbIgnoreILS.Checked = true;

			if (cbIgnoreILS.Checked)
			{
				GBASDir = SelectedRWY.pPtPrj[eRWY.ptTHR].M;
				GBASCourse = SelectedRWY.pPtGeo[eRWY.ptTHR].M;
				TextBox0013.Text = GBASCourse.ToString("0.00");

				TextBox0010.Text = (3.0).ToString();
				GPAngle = 3.0 * ARANMath.DegToRadValue;

				FicTHRprj = SelectedRWY.pPtPrj[eRWY.ptTHR];
				FicTHRgeo = SelectedRWY.pPtGeo[eRWY.ptTHR];
			}
			else
				GPAngle = ILS.GPAngle;

			TanGPA = System.Math.Tan(GPAngle);

			AlignP_THRMin = (GlobalVars.MinGPIntersectHeight - GP_RDH) / TanGPA;
			AlignP_THRMax = (GlobalVars.MaxGPIntersectHeight - GP_RDH) / TanGPA;

			if (CalculatePageI())
				PrevCmbRWY = RWYIndex;

			//pCircle.Clear();
			//pCircle.ExteriorRing = ARANFunctions.CreateCirclePrj(FicTHRprj, GlobalVars.RModel);

			////DBModule.GetObstaclesByDist(out ObstacleList, GlobalVars.CurrADHP.pPtPrj, GlobalVars.RModel, FicTHRprj.Z);

			//ComboBox0002.SelectedIndex = 0;
			////ComboBox0103.SelectedIndex = 0;
			////ComboBox0202.SelectedIndex = 0;
			////ComboBox0301.SelectedIndex = 0;
		}

		private void ComboBox0002_SelectedIndexChanged(int ComboBox0002SelectedIndex, EventArgs e)
		{
			if (ComboBox0002SelectedIndex < 0)
				return;

			_flightCategory = (eSBASCat)ComboBox0002SelectedIndex;

			string ErrorStr = "The value is outside the range of GP angle.";
			//Dim ILS23ObstacleList As ObstacleContainer

			ComboBox0004.Items.Clear();

			TextBox0010.ForeColor = System.Drawing.SystemColors.WindowText;
			//ToolTip1.SetToolTip(TextBox0010, "")
			if (GPAngle + ARANMath.EpsilonRadian < GlobalVars.constants.Pansops[ePANSOPSData.arMinGPAngle].Value)
			{
				//ToolTip1.SetToolTip(TextBox0010, ErrorStr);
				TextBox0010.ForeColor = System.Drawing.Color.Red;
				MessageBox.Show(ErrorStr);
			}

			switch (ComboBox0002SelectedIndex)
			{
				case 0:
					if (GPAngle - ARANMath.EpsilonRadian > GlobalVars.constants.Pansops[ePANSOPSData.arMaxGPAngleCat1].Value)
					{
						//ToolTip1.SetToolTip(TextBox0010, ErrorStr);
						TextBox0010.ForeColor = System.Drawing.Color.Red;
						MessageBox.Show(ErrorStr);
					}

					ComboBox0004.Items.Add("Radio altimeter");
					ComboBox0004.Items.Add("Pressure altimeter");
					ComboBox0004.SelectedIndex = 1;
					break;

				case 1:
				case 2:
					if (GPAngle - ARANMath.EpsilonRadian > GlobalVars.constants.Pansops[ePANSOPSData.arMaxGPAngleCat2].Value)
					{
						TextBox0010.ForeColor = System.Drawing.Color.Red;
						//ToolTip1.SetToolTip(TextBox0010, ErrorStr);
						MessageBox.Show(ErrorStr);
					}

					ComboBox0004.Items.Add("Radio altimeter");
					Functions.CreateOFZPlanes(FicTHRprj, ArDir, 45.0, ref GlobalVars.OFZPlanes);

					int n = GlobalVars.OFZPlanes.Length;

					for (int i = 0; i < n; i++)
					{
						GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.OFZPlanesElement[i]);
						GlobalVars.OFZPlanesElement[i] = GlobalVars.gAranGraphics.DrawMultiPolygon(GlobalVars.OFZPlanes[i].Poly, AranEnvironment.Symbols.eFillStyle.sfsHollow, -1, GlobalVars.OFZPlanesState);
					}

					GlobalVars.VisibilityBar.SetEnabled(GlobalVars.VisibilityBar.OFZ, true);

					if (Functions.AnaliseObstacles(GlobalVars.ObstacleList, out OFZObstacleList, FicTHRprj, ArDir, GlobalVars.OFZPlanes) > 0)
					{
						MessageBox.Show("The obstacle penetrate Obstacle Free Zone.\r\n" + "You can obtain detailed information in the report.");
						//        reportForm.FillPage8 ILS23ObstacleList
						//        ComboBox0002.ListIndex = PrevCmb002
						//        Return
					}

					//PrecReportFrm.FillPage01(OFZObstacleList);
					ComboBox0004.SelectedIndex = 0;
					break;
			}

			//PrevCmb002 = ComboBox0002.SelectedIndex
			//TextBox0404.Text = ComboBox0002.Text;
		}

		private void ComboBox0003_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!bFormInitialised)
				return;

			double fMOCCorrH, fMOCCorrGP, fMargin;
			int k = ComboBox0003.SelectedIndex;
			if (k < 0) return;

			Category = k;

			//TextBox0403.Text = ComboBox0003.Text;

			Ss = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arSemiSpan].Value[Category];
			Vs = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arVerticalSize].Value[Category];

			TextBox0005.Text = Ss.ToString("0.00");
			TextBox0006.Text = Vs.ToString("0.00");

			if (ComboBox0004.SelectedIndex == 0)						//Radio	//	fMargin = 0.096 / 0.277777777777778 * cVatMax.Values(k) - 3.2
				fMargin = 0.34406047516199 * GlobalVars.constants.AircraftCategory[aircraftCategoryData.VatMax].Value[k] - 3.2;					// 0.3456
			else														//Baro	//	fMargin = 0.068 / 0.277777777777778 * cVatMax.Values(k) + 28.3
				fMargin = 0.24298056155508 * GlobalVars.constants.AircraftCategory[aircraftCategoryData.VatMax].Value[k] + 28.3;					// 0.2448

			if (GlobalVars.CurrADHP.pPtGeo.Z > 900.0)
				fMOCCorrH = GlobalVars.CurrADHP.pPtGeo.Z * fMargin / 1500.0;
			else
				fMOCCorrH = 0.0;

			if (GPAngle > ARANMath.DegToRad(3.2))
				fMOCCorrGP = (GPAngle - ARANMath.DegToRad(3.2)) * fMargin * 0.5;
			else
				fMOCCorrGP = 0.0;

			m_fMOC = fMargin + fMOCCorrGP + fMOCCorrH;

			TextBox0007.Text = GlobalVars.unitConverter.HeightToDisplayUnits(m_fMOC, eRoundMode.NEAREST).ToString();
		}

		private void ComboBox0004_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox0003_SelectedIndexChanged(ComboBox0003, null);
			//???????? ??????????? ?????????? ??????????
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

			GPAngle = fTmp;
			if (GPAngle < 3.0)
				GPAngle = 3.0;

			if (GPAngle > 3.5)
				GPAngle = 3.5;

			if (fTmp != GPAngle)
				TextBox0010.Text = System.Math.Round(GPAngle, 2).ToString("0.00");

			GPAngle *= ARANMath.DegToRadValue;
			TanGPA = System.Math.Tan(GPAngle);

			TextBox0010.Tag = TextBox0010.Text;

			AlignP_THRMin = (GlobalVars.MinGPIntersectHeight - GP_RDH) / TanGPA;
			AlignP_THRMax = (GlobalVars.MaxGPIntersectHeight - GP_RDH) / TanGPA;

			if (!double.TryParse(TextBox0014.Text, out fTmp))
				TextBox0014.Text = Math.Round(AlignP_THRMin + 0.004999, 1).ToString();

			TextBox0014_Validating(TextBox0014, null);	//Calls ParamChanged();
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

			GBASCourse = fTmp;

			while (GBASCourse < 0.0)
				GBASCourse += 360.0;

			while (GBASCourse >= 360.0)
				GBASCourse = GBASCourse - 360.0;

			double DirInDeg0, DirInDeg1, rAngle;
			TurnDirection res;

			DirInDeg0 = SelectedRWY.pPtGeo[eRWY.ptTHR].M - 5.0;
			DirInDeg1 = GBASCourse;
			rAngle = ARANMath.SubtractAnglesInDegs(DirInDeg0, DirInDeg1);

			if (360.0 - rAngle < ARANMath.EpsilonDegree || rAngle < ARANMath.EpsilonDegree)
				res = TurnDirection.NONE;
			else
			{
				rAngle = ARANMath.Modulus(DirInDeg1 - DirInDeg0);

				if (rAngle < 180.0)
					res = TurnDirection.CW;
				else
					res = TurnDirection.CCW;
			}

			if (res == TurnDirection.CCW)
				GBASCourse = SelectedRWY.pPtGeo[eRWY.ptTHR].M - 5.0;
			else
			{
				DirInDeg0 = SelectedRWY.pPtGeo[eRWY.ptTHR].M + 5.0;
				rAngle = ARANMath.SubtractAnglesInDegs(DirInDeg0, DirInDeg1);

				if (360.0 - rAngle < ARANMath.EpsilonDegree || rAngle < ARANMath.EpsilonDegree)
					res = TurnDirection.NONE;
				else
				{
					rAngle = ARANMath.Modulus(DirInDeg1 - DirInDeg0);

					if (rAngle < 180.0)
						res = TurnDirection.CW;
					else
						res = TurnDirection.CCW;
				}

				if (res == TurnDirection.CW)
					GBASCourse = SelectedRWY.pPtGeo[eRWY.ptTHR].M + 5.0;
			}

			if (fTmp != GBASCourse)
				TextBox0013.Text = GBASCourse.ToString("0.00");

			//pPtBase = LineLineIntersect(SelectedRWY.pPtPrj(eRWY.PtTHR), RWYDir, ILS.pPtPrj, ILSDir);
			if (ptInterGeo.IsEmpty)
				GBASDir = ARANFunctions.AztToDirection(SelectedRWY.pPtGeo[eRWY.ptTHR], GBASCourse, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);
			else
				GBASDir = ARANFunctions.AztToDirection(ptInterGeo, GBASCourse, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);

			TextBox0013.Tag = TextBox0013.Text;

			CalculatePageI();
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

			AlignP_THR = fTmp;

			if (AlignP_THR < AlignP_THRMin)
				AlignP_THR = AlignP_THRMin;

			if (AlignP_THR > AlignP_THRMax)
				AlignP_THR = AlignP_THRMax;

			if (fTmp != AlignP_THR)
				TextBox0014.Text = System.Math.Round(AlignP_THR, 2).ToString("0.00");

			TextBox0014.Tag = TextBox0014.Text;

			//OCHMin = GP_RDH + AlignP_THR * TanGPA;

			CalculatePageI();
		}

		private void cbIgnoreILS_CheckedChanged(object sender, EventArgs e)
		{
			if (cbIgnoreILS.Checked)
			{
				GBASDir = SelectedRWY.pPtPrj[eRWY.ptTHR].M;
				GBASCourse = SelectedRWY.pPtGeo[eRWY.ptTHR].M;
				TextBox0013.Text = GBASCourse.ToString("0.00");

				TextBox0010.Text = (3.0).ToString();
				GPAngle = 3.0 * ARANMath.DegToRadValue;

				double x, y;
				ARANFunctions.PrjToLocal(SelectedRWY.pPtPrj[eRWY.ptStart], RWYDir, SelectedRWY.pPtPrj[eRWY.ptTHR], out x, out y);
				FPAPprj = ARANFunctions.LocalToPrj(SelectedRWY.pPtPrj[eRWY.ptStart], RWYDir, SelectedRWY.ASDA, y);

				ARANFunctions.PrjToLocal(SelectedRWY.pPtPrj[eRWY.ptTHR], RWYDir, FPAPprj, out x, out y);
				FPAP_THR = x;

				FicTHRprj = SelectedRWY.pPtPrj[eRWY.ptTHR];
				FicTHRgeo = SelectedRWY.pPtGeo[eRWY.ptTHR];
			}
			else
				GPAngle = ILS.GPAngle;

			TanGPA = System.Math.Tan(GPAngle);

			AlignP_THRMin = (GlobalVars.MinGPIntersectHeight - GP_RDH) / TanGPA;
			AlignP_THRMax = (GlobalVars.MaxGPIntersectHeight - GP_RDH) / TanGPA;

			CalculatePageI();
		}

		#endregion

		#region Page II
		private bool preparePageII()
		{
			if (_flightCategory == eSBASCat.CategoryI)
				_zSurfaceOrigin = GlobalVars.Cat1OASZOrigin;
			else if (_flightCategory == eSBASCat.APVI)
				_zSurfaceOrigin = GlobalVars.Cat1OASZOrigin + 38 / TanGPA;
			else
				_zSurfaceOrigin = GlobalVars.Cat1OASZOrigin + 8 / TanGPA;

			if (GPAngle > GlobalVars.MaxRefGPAngle)
				_zSurfaceOrigin += 500.0 * (GPAngle - GlobalVars.MaxRefGPAngle);

			int n = GlobalVars.WPTList.Length;
			if (n >= 0)
			{
				FixAngl = new WPT_FIXType[n];
				int j = 0;
				for (int i = 0; i < n; i++)

					if (GlobalVars.WPTList[i].TypeCode == eNavaidType.VOR || GlobalVars.WPTList[i].TypeCode == eNavaidType.NDB ||
						GlobalVars.WPTList[i].TypeCode == eNavaidType.TACAN)
					{
						FixAngl[j] = GlobalVars.WPTList[i];
						j++;
					}

				if (j > 0)
					Array.Resize<WPT_FIXType>(ref FixAngl, j);
				else
				{
					FixAngl = new WPT_FIXType[0];
					//OptionButton0602.Enabled = false;
				}
			}
			else
			{
				FixAngl = new WPT_FIXType[0];
                //OptionButton0603.Enabled = false;
                //OptionButton0602.Enabled = false;
                //ComboBox0601.Enabled = false;
			}

			//==============================================================
			ArrivalProfile.InitWOFAF(SelectedRWY.Length, NativeMethods.Modulus(SelectedRWY.TrueBearing) > 180.0 ? -1 : 1, FicTHRprj.Z, FicTHRprj.Z, ProfileBtn);
			ProfileBtn.Enabled = true;

			CheckBox0101.Checked = false;
			//ClearScr()

			Functions.CreateILSPlanes(FicTHRprj, ArDir, ref GlobalVars.ILSPlanes);
			Functions.AnaliseObstacles(GlobalVars.ObstacleList, out ILSObstacleList, FicTHRprj, ArDir, GlobalVars.ILSPlanes);

			fMissAprPDG = 0.01 * (double)MAGUpDwn.Value;
			Common.CalcOASPlanes(_flightCategory, DistLLZ2THR, GPAngle, fMissAprPDG, GP_RDH, Ss, Vs, ref GlobalVars.wOASPlanes);

			double hMax = GlobalVars.constants.Pansops[ePANSOPSData.arFAPMaxRange].Value * TanGPA + GP_RDH - GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value;
			Common.CreateOASPlanes(FicTHRprj, ArDir, hMax, ref GlobalVars.wOASPlanes, 1);

			Functions.CalcEffectiveHeights(ref ILSObstacleList, GPAngle, fMissAprPDG, GP_RDH, _zSurfaceOrigin, GlobalVars.wOASPlanes, true); //arMAS_Climb_Nom.Value

			n = GlobalVars.wOASPlanes.Length;

			Functions.AnaliseObstacles(GlobalVars.ObstacleList, out OASObstacleList, FicTHRprj, ArDir, GlobalVars.wOASPlanes);
			if (GlobalVars.VisibilityBar == null || GlobalVars.VisibilityBar.IsDisposed)
			{
				GlobalVars.VisibilityBar = new ToolbarForm();
				GlobalVars.VisibilityBar.Show(GlobalVars.Win32Window);
			}

			/*
			GlobalVars.VisibilityBar.SetEnabled(GlobalVars.VisibilityBar.OASCat23, false);
			if (ComboBox0002SelectedIndex > 0)
			{
				//OAS_DATABase(DistLLZ2THR, GPAngle, fMissAprPDG, 2, GP_RDH, Ss, Vs, OASPlanesCat23)
				Common.CreateOASPlanes(FicTHRprj, ArDir, GlobalVars.arHOASPlaneCat23, ref GlobalVars.OASPlanesCat23, 2);
				Functions.AnaliseCat23Obstacles(OASObstacleList, FicTHRprj, ArDir, GlobalVars.OASPlanesCat23);

				n = GlobalVars.OASPlanesCat23.Length;
				for (int i = 0; i < n; i++)
				{
					GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.OASPlanesCat23Element[i]);
					GlobalVars.OASPlanesCat23Element[i] = GlobalVars.gAranGraphics.DrawMultiPolygon(GlobalVars.OASPlanesCat23Element[i].Poly, -1, AranEnvironment.Symbols.eFillStyle.sfsHollow, GlobalVars.OASPlanesCat23State);

				}
				GlobalVars.VisibilityBar.SetEnabled(GlobalVars.VisibilityBar.OASCat23, true);
			}
			*/

			Functions.CalcEffectiveHeights(ref OASObstacleList, GPAngle, fMissAprPDG, GP_RDH, _zSurfaceOrigin, GlobalVars.wOASPlanes);

			int m = OASObstacleList.Obstacles.Length;
			WorkObstacleList.Obstacles = new Obstacle[m];

			n = OASObstacleList.Parts.Length;
			WorkObstacleList.Parts = new ObstacleData[n];

			Array.Copy(OASObstacleList.Obstacles, WorkObstacleList.Obstacles, m);
			Array.Copy(OASObstacleList.Parts, WorkObstacleList.Parts, n);

			Functions.Sort(ref WorkObstacleList, 2);

			WPTInSector(FicTHRprj, GlobalVars.WPTList, out InSectList);
			bNavFlg = false;

			fFAPDist = GlobalVars.constants.Pansops[ePANSOPSData.arOptimalFAFRang].Value;
			_hFAP = FAPDist2hFAP(fFAPDist);

			TextBox0102.Tag = _hFAP.ToString();

			ComboBox0103_SelectedIndexChanged(ComboBox0103, null);
			ComboBox0102_SelectedIndexChanged(ComboBox0102, null);

			//if (ComboBox0103.SelectedIndex == 0)
			//	TextBox0102.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fTmp + FicTHRprj.Z, eRoundMode.NERAEST).ToString();
			//else
			//	TextBox0102.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fTmp, eRoundMode.NERAEST).ToString();

			//if (ComboBox0102.SelectedIndex == 0)
			//	ComboBox0102_SelectedIndexChanged(ComboBox0102, null);
			//else
			//	ComboBox0102.SelectedIndex = 0;

			if (!bHaveSolution)
				return false;

			fMinFAPDist = Functions.StartPointDist(GlobalVars.wOASPlanes[(int)eOAS.XlPlane], GlobalVars.wOASPlanes[(int)eOAS.YlPlane], GlobalVars.arHOASPlaneCat1, 0.0);
			fMaxFAPDist = GlobalVars.constants.Pansops[ePANSOPSData.arMaxRangeFAS].Value;

			ComboBox0105.Items.Clear();
			n = InSectList.Length;

			for (int i = 0; i < n; i++)
			{
				if (InSectList[i].TypeCode != eNavaidType.NONE)
					continue;

				double X, Y;
				ARANFunctions.PrjToLocal(FicTHRprj, ArDir + Math.PI, InSectList[i].pPtPrj, out X, out Y);

				if (X < fMinFAPDist) continue;
				if (X > fMaxFAPDist) continue;

				ComboBox0105.Items.Add(InSectList[i]);
			}
			if (ComboBox0105.Items.Count > 0)
			{
				CheckBox0102.Enabled = true;
				ComboBox0105.SelectedIndex = 0;
			}
			else
			{
				CheckBox0102.Enabled = false;
				CheckBox0102.Checked = false;
			}

			return true;
		}

		private D3DPolygone CreatePlane15(NavaidType NavDat, out double Dist)
		{
			double fDis;
			double hFix = ptFAP.Z + FicTHRprj.Z;

			Point pPtNAV = NavDat.pPtPrj;
			Polygon pTmpPoly;
			MultiPolygon pTmpMultiPoly;
			GeometryOperators pTopo = new GeometryOperators();
			LineString pCutter = new LineString();

			if (NavDat.IntersectionType == eIntersectionType.OnNavaid)
			{
				if (NavDat.TypeCode == eNavaidType.VOR)
					Functions.VORFIXTolerArea(pPtNAV, ArDir, hFix, out pTmpPoly);
				else
					Functions.NDBFIXTolerArea(pPtNAV, ArDir, hFix, out pTmpPoly);

				pTmpMultiPoly = new MultiPolygon();
				pTmpMultiPoly.Add(pTmpPoly);
			}
			else
			{
				SideDirection Side = ARANMath.SideDef(pPtNAV, ArDir + ARANMath.C_PI_2, ptFAP);

				fDis = ARANFunctions.ReturnDistanceInMeters(pPtNAV, ptFAP);
				double d0 = System.Math.Sqrt(fDis * fDis + (hFix - pPtNAV.Z) * (hFix - pPtNAV.Z));

				d0 = d0 * GlobalVars.constants.NavaidConstants.DME.ErrorScalingUp + GlobalVars.constants.NavaidConstants.DME.MinimalError;

				Polygon pSect0 = new Polygon(), pSect1 = new Polygon();
				double D = fDis + d0;

				Ring pTmpRing = ARANFunctions.CreateCirclePrj(pPtNAV, D);
				pSect0.ExteriorRing = pTmpRing;

				D = fDis - d0;
				pTmpRing = ARANFunctions.CreateCirclePrj(pPtNAV, D);
				pSect1.ExteriorRing = pTmpRing;

				pTmpMultiPoly = (MultiPolygon)pTopo.Difference(pSect0, pSect1);

				pCutter.Add(ARANFunctions.LocalToPrj(pPtNAV, ArDir - ARANMath.C_PI_2, GlobalVars.MaxModelRadius + 20.0 * fDis));
				pCutter.Add(ARANFunctions.LocalToPrj(pPtNAV, ArDir + ARANMath.C_PI_2, GlobalVars.MaxModelRadius + 20.0 * fDis));

				Geometry geom0, geom1;
				if (Side == SideDirection.sideRight)
					pTopo.Cut((Geometry)pTmpMultiPoly, pCutter, out geom1, out geom0);
				else
					pTopo.Cut((Geometry)pTmpMultiPoly, pCutter, out geom0, out geom1);

				Ring pTrackRing = new Ring();

				pTrackRing.Add(GARPprj);
				pTrackRing.Add(ARANFunctions.LocalToPrj(GARPprj, ArDir - GlobalVars.constants.NavaidConstants.LLZ.TrackingTolerance + Math.PI, 3.0 * GlobalVars.MaxModelRadius));
				pTrackRing.Add(ARANFunctions.LocalToPrj(GARPprj, ArDir + GlobalVars.constants.NavaidConstants.LLZ.TrackingTolerance + Math.PI, 3.0 * GlobalVars.MaxModelRadius));

				Polygon pTrackPoly = new Polygon();
				pTrackPoly.ExteriorRing = pTrackRing;

				pTmpMultiPoly = (MultiPolygon)pTopo.Intersect(pTrackPoly, geom0);
			}
			//================================================================

			pFAPTolerArea = pTmpMultiPoly;

			const double FarDist = 100000.0;

			pCutter.Clear();
			pCutter.Add(ARANFunctions.LocalToPrj(ptFAP, ArDir, -FarDist, -GlobalVars.MaxModelRadius));
			pCutter.Add(ARANFunctions.LocalToPrj(ptFAP, ArDir, -FarDist, GlobalVars.MaxModelRadius));

			fDis = FarDist - pTopo.GetDistance(pCutter, pTmpMultiPoly);

			if (NavDat.IntersectionType == eIntersectionType.ByDistance || NavDat.IntersectionType == eIntersectionType.RadarFIX)
				Dist = ARANFunctions.ReturnDistanceInMeters(FicTHRprj, ptFAP);
			else
				Dist = ARANFunctions.ReturnDistanceInMeters(FicTHRprj, pPtNAV);

			FAPEarlierToler = Dist + fDis;
			FAP15FIX = pTmpMultiPoly;
			//================================================================

			double Tan15 = GlobalVars.constants.Pansops[ePANSOPSData.arFixMaxIgnorGrd].Value;

			Point ptTmp = ARANFunctions.LocalToPrj(pPtNAV, ArDir + Math.PI, fDis);
			ptTmp.Z = FAPDist2hFAP(Dist) - 150.0;

			D3DPolygone result;
			result.Plane.pPt = ptTmp;
			result.Plane.A = Tan15;
			result.Plane.B = 0.0;
			result.Plane.C = -1.0;
			result.Plane.D = (ptTmp.Z - Tan15 * FAPEarlierToler);

			result.Plane.X = result.Plane.Y = result.Plane.Z = 0.0;

			double Hinter = Common.Det2(result.Plane.A, result.Plane.D, GlobalVars.wOASPlanes[(int)eOAS.WPlane].Plane.A, GlobalVars.wOASPlanes[(int)eOAS.WPlane].Plane.D) /
				(result.Plane.A - GlobalVars.wOASPlanes[(int)eOAS.WPlane].Plane.A);

			LineSegment pLeftLine = Common.IntersectPlanes(result.Plane, GlobalVars.wOASPlanes[(int)eOAS.XlPlane].Plane, ptTmp.Z, Hinter);
			LineSegment pRightLine = Common.IntersectPlanes(result.Plane, GlobalVars.wOASPlanes[(int)eOAS.XrPlane].Plane, ptTmp.Z, Hinter);

			Common.RotateAndOffset(ArDir + Math.PI, FicTHRprj, pLeftLine);
			Common.RotateAndOffset(ArDir + Math.PI, FicTHRprj, pRightLine);

			Ring resultRing = new Ring();
			resultRing.Add(pLeftLine.Start);
			resultRing.Add(pLeftLine.End);
			resultRing.Add(pRightLine.End);
			resultRing.Add(pRightLine.Start);
			resultRing.Add(pLeftLine.Start);

			pTmpPoly = new Polygon();
			pTmpPoly.ExteriorRing = resultRing;
			result.Poly = new MultiPolygon();
			result.Poly.Add(pTmpPoly);

			return result;
		}

		private double CalcFAPOCH(ref double fhFAP, double f_MOC, out int Ix)
		{
			int i, n = WorkObstacleList.Parts.Length;
			double result = f_MOC > fRDHOCH ? f_MOC : fRDHOCH;

			Ix = -1;

			for (i = n - 1; i >= 0; i--)
			{
				if (WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].IgnoredByUser ||
					(CheckBox0101.Checked && WorkObstacleList.Parts[i].Dist > FAPEarlierToler))
					continue;

				if (WorkObstacleList.Parts[i].hPenet > 0.0 && WorkObstacleList.Parts[i].ReqH <= fhFAP)  //&& (WorkObstacleList(i).Dist >= -ZSurfaceOrigin)
				{
					WorkObstacleList.Parts[i].ReqOCH = Math.Min(WorkObstacleList.Parts[i].Height, WorkObstacleList.Parts[i].EffectiveHeight) + f_MOC;

					int bUnder15 = 0;
					if (CheckBox0101.Checked)
					{
						double fTmp = FAPEarlierToler - WorkObstacleList.Parts[i].Dist;
						if (fTmp < GlobalVars.constants.Pansops[ePANSOPSData.arFIX15PlaneRang].Value)
						{
							fTmp = fTmp * GlobalVars.constants.Pansops[ePANSOPSData.arFixMaxIgnorGrd].Value;
							bUnder15 = fhFAP - GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value - fTmp > WorkObstacleList.Parts[i].Height ? 16 : 0;
						}
					}

					WorkObstacleList.Parts[i].Plane = (((int)WorkObstacleList.Parts[i].Plane & (~16)) | bUnder15);

					if (bUnder15 == 0)
					{
						if (WorkObstacleList.Parts[i].ReqOCH > result)
						{
							result = WorkObstacleList.Parts[i].ReqOCH;
							Ix = i;
						}

						if (WorkObstacleList.Parts[i].ReqOCH > fhFAP)
							fhFAP = WorkObstacleList.Parts[i].ReqOCH;
					}
				}
				else
					WorkObstacleList.Parts[i].ReqOCH = 0.0;
			}

			return result;
		}

		private Interval[] CalcImIntervals(double fhFAP, out ObstacleContainer IntermObstacleList, out int Ix)
		{
			Common.CreateOASPlanes(FicTHRprj, ArDir, fhFAP - GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value, ref GlobalVars.wOASPlanes, 1);
			LineSegment pLPolyLine = Common.IntersectPlanes(GlobalVars.wOASPlanes[(int)eOAS.XlPlane].Plane, GlobalVars.wOASPlanes[(int)eOAS.YlPlane].Plane, fhFAP - GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value, fhFAP);
			LineSegment pRPolyLine = Common.IntersectPlanes(GlobalVars.wOASPlanes[(int)eOAS.YrPlane].Plane, GlobalVars.wOASPlanes[(int)eOAS.XrPlane].Plane, fhFAP - GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value, fhFAP);

			Common.RotateAndOffset(ArDir + Math.PI, FicTHRprj, pLPolyLine);
			Common.RotateAndOffset(ArDir + Math.PI, FicTHRprj, pRPolyLine);
			//Common.CreateOASPlanes(FicTHRprj, ArDir, fhFAP - GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value, ref GlobalVars.wOASPlanes, 1);

			//======================================================

			double fDist = hFAP2FAPDist(fhFAP);
			Point ptTmpFAP = ARANFunctions.LocalToPrj(FicTHRprj, ArDir + Math.PI, fDist);

			LineString pCutLine = new LineString();
			pCutLine.Add(ARANFunctions.LocalToPrj(ptTmpFAP, ArDir + ARANMath.C_PI_2, 100000.0));
			pCutLine.Add(ARANFunctions.LocalToPrj(ptTmpFAP, ArDir - ARANMath.C_PI_2, 100000.0));

			Point pt1 = pCutLine[0];
			Point pt2 = pCutLine[1];

			Point pt3 = pRPolyLine.Start;
			Point pt4 = pRPolyLine.End;

			Point ptRight = (Point)pRPolyLine.Start.Clone();
			Point ptLeft = (Point)pLPolyLine.Start.Clone();

			int i = 1;

			//double Va = (pt4.X - pt3.X) * (pt1.Y - pt3.Y) - (pt4.Y - pt3.Y) * (pt1.X - pt3.X);
			double Vb = (pt2.X - pt1.X) * (pt1.Y - pt3.Y) - (pt2.Y - pt1.Y) * (pt1.X - pt3.X);
			double Nd = (pt4.Y - pt3.Y) * (pt2.X - pt1.X) - (pt4.X - pt3.X) * (pt2.Y - pt1.Y);
			double Ub = Vb / Nd;

			bool bFlg = Ub >= 0 && Ub <= 1;

			if (bFlg)
			{
				double Xr = pt3.X + Ub * (pt4.X - pt3.X);
				double Yr = pt3.Y + Ub * (pt4.Y - pt3.Y);
				ptRight.SetCoords(Xr, Yr);

				pt3 = pLPolyLine.Start;
				pt4 = pLPolyLine.End;

				//Va = (pt4.X - pt3.X) * (pt1.Y - pt3.Y) - (pt4.Y - pt3.Y) * (pt1.X - pt3.X);
				Vb = (pt2.X - pt1.X) * (pt1.Y - pt3.Y) - (pt2.Y - pt1.Y) * (pt1.X - pt3.X);
				Nd = (pt4.Y - pt3.Y) * (pt2.X - pt1.X) - (pt4.X - pt3.X) * (pt2.Y - pt1.Y);
				Ub = Vb / Nd;

				Xr = pt3.X + Ub * (pt4.X - pt3.X);
				Yr = pt3.Y + Ub * (pt4.Y - pt3.Y);

				ptLeft.SetCoords(Xr, Yr);
				i = 2;
			}

			//double MaxLocDist = Math.Max(46000.0, fDist + 28000.0);
			double MaxLocDist = fDist + GlobalVars.constants.Pansops[ePANSOPSData.arImRange_Max].Value; //28000.0

			Ring imFullAreaRing = new Ring();
			Point pPoint = ARANFunctions.LocalToPrj(FicTHRprj, ArDir + Math.PI, fDist + arMinISlen);

			if (bFlg)
				imFullAreaRing.Add(pRPolyLine.Start);

			imFullAreaRing.Add(ptRight);
			imFullAreaRing.Add(ARANFunctions.LocalToPrj(pPoint, ArDir - ARANMath.C_PI_2, GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value));
			imFullAreaRing.Add(ARANFunctions.LocalToPrj(imFullAreaRing[i], ArDir - Math.PI, MaxLocDist - fDist - arMinISlen - GARP_THR));

			double fMaxInterLenght = ARANFunctions.Point2LineDistancePrj(ptTmpFAP, imFullAreaRing[i + 1], ArDir + ARANMath.C_PI_2);

			imFullAreaRing.Add(ARANFunctions.LocalToPrj(imFullAreaRing[i + 1], ArDir + ARANMath.C_PI_2, 2.0 * GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value));
			imFullAreaRing.Add(ARANFunctions.LocalToPrj(pPoint, ArDir + ARANMath.C_PI_2, GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value));
			imFullAreaRing.Add(ptLeft);
			if (bFlg)
				imFullAreaRing.Add(pLPolyLine.Start);

			//============================================================================
			Geometry leftGeom, rightGeom;
			GeometryOperators pTopo = new GeometryOperators();

			MultiPolygon pTmpPolygon = null, pPolygon;

			if (CheckBox0101.Enabled && CheckBox0101.Checked)
			{
				pCutLine.Clear();
				pPoint = ARANFunctions.LocalToPrj(FicTHRprj, ArDir + Math.PI, FAPEarlierToler);
				pCutLine.Add(ARANFunctions.LocalToPrj(pPoint, ArDir + ARANMath.C_PI_2, 100000.0));
				pCutLine.Add(ARANFunctions.LocalToPrj(pPoint, ArDir - ARANMath.C_PI_2, 100000.0));

				try
				{
					pTopo.Cut(GlobalVars.wOASPlanes[(int)eOAS.CommonPlane].Poly, pCutLine, out leftGeom, out rightGeom);
					pTmpPolygon = (MultiPolygon)leftGeom;
					pPolygon = (MultiPolygon)rightGeom;
				}
				catch
				{ }

				if (pTmpPolygon == null)
					pTmpPolygon = GlobalVars.wOASPlanes[(int)eOAS.CommonPlane].Poly;
			}
			else
				pTmpPolygon = GlobalVars.wOASPlanes[(int)eOAS.CommonPlane].Poly;

			Polygon pTmpPoly = new Polygon();
			pTmpPoly.ExteriorRing = imFullAreaRing;

			IntermediateFullArea = new MultiPolygon();
			IntermediateFullArea.Add(pTmpPoly);

			IntermediateWorkArea = (MultiPolygon)pTopo.Difference(IntermediateFullArea, pTmpPolygon);

			fHalfFAPWidth = 0.5 * ARANFunctions.ReturnDistanceInMeters(ptRight, ptLeft);
			Functions.GetIntermObstacleList(GlobalVars.ObstacleList, out IntermObstacleList, FicTHRprj, ArDir, IntermediateWorkArea);

			int n = IntermObstacleList.Parts.Length;
			int m = IntermObstacleList.Obstacles.Length;
			i = 0;

			while (i < n)
			{
				double fTmp = System.Math.Round(IntermObstacleList.Parts[i].Dist - fDist + 0.4999) + 20.0;
				if (fTmp > arMinISlen)
				{
					IntermObstacleList.Parts[i].Rmin = fTmp;
					IntermObstacleList.Parts[i].MOC = 2.0 * GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value * (1.0 - IntermObstacleList.Parts[i].DistStar / GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value);
				}
				else if (fTmp > 0.0)
				{
					IntermObstacleList.Parts[i].Rmin = arMinISlen;
					IntermObstacleList.Parts[i].MOC = 2.0 * GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value * (GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value - fHalfFAPWidth - arMinISlen * (IntermObstacleList.Parts[i].DistStar - fHalfFAPWidth) / fTmp) / GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value;
				}
				else
				{
					IntermObstacleList.Parts[i].Rmin = arMinISlen;
					IntermObstacleList.Parts[i].MOC = GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value;
				}

				if (IntermObstacleList.Parts[i].MOC > GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value)
					IntermObstacleList.Parts[i].MOC = GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value;

				IntermObstacleList.Parts[i].hPenet = System.Math.Round(IntermObstacleList.Parts[i].Height +
					IntermObstacleList.Parts[i].MOC - fhFAP + 0.4999);

				if (IntermObstacleList.Parts[i].hPenet > 0.0)
				{
					double fTmp1 = IntermObstacleList.Parts[i].DistStar - fHalfFAPWidth;
					if (fTmp <= 0.0 || fTmp1 <= 0.0)
						IntermObstacleList.Parts[i].Rmax = fMaxInterLenght;
					else
					{
						double fDh = fhFAP - IntermObstacleList.Parts[i].Height;

						if (fDh < 0.0)
							IntermObstacleList.Parts[i].Rmax = fTmp * (GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value - fHalfFAPWidth) / fTmp1;
						else
							IntermObstacleList.Parts[i].Rmax = fTmp * (GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value *
								(1.0 - 0.5 * fDh / GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value) - fHalfFAPWidth) / fTmp1;

						if (IntermObstacleList.Parts[i].Rmax > fMaxInterLenght)
							IntermObstacleList.Parts[i].Rmax = fMaxInterLenght;
					}
					i++;
				}
				else
				{
					int l = IntermObstacleList.Parts[i].Owner;
					IntermObstacleList.Obstacles[l].PartsNum--;

					//int k = IntermObstacleList.Parts[i].Index;
					//IntermObstacleList.Obstacles[l].Parts[k] = IntermObstacleList.Obstacles[l].Parts[IntermObstacleList.Obstacles[l].PartsNum];
					//IntermObstacleList.Parts[IntermObstacleList.Obstacles[l].Parts[k]].Index = k;

					n--;
					IntermObstacleList.Parts[i] = IntermObstacleList.Parts[n];
					//IntermObstacleList.Obstacles[IntermObstacleList.Parts[i].Owner].Parts[IntermObstacleList.Parts[i].Index] = i;
				}
			}

			Interval[] ImRange = new Interval[1];
			Interval[] ResRange;
			Interval ObsRange;

			ImRange[0].Min = arMinISlen;
			ImRange[0].Max = fMaxInterLenght;
			ImRange[0].Tag = -1;
			Ix = -1;

			if (n < 0)
			{
				IntermObstacleList.Parts = new ObstacleData[0];
				return ImRange;
			}

			Array.Resize<ObstacleData>(ref IntermObstacleList.Parts, n);

			Functions.Sort(ref IntermObstacleList, 0);
			ObsRange = default(Interval);

			for (i = 0; i < n; i++)
			{
				if (IntermObstacleList.Parts[i].hPenet <= 0.0)
					continue;

				int p = ImRange.Length;

				ObsRange.Min = IntermObstacleList.Parts[i].Rmin;
				ObsRange.Max = IntermObstacleList.Parts[i].Rmax;

				int k = 0;
				while (k < p)
				{
					ResRange = Functions.IntervalsDifference(ImRange[k], ObsRange);
					int l = ResRange.Length;
					if (l > 0)
					{
						if (ImRange[k].Min != ResRange[0].Min || ImRange[k].Max != ResRange[0].Max)
						{
							ImRange[k] = ResRange[0];
							ImRange[k].Tag = i;
						}

						if (l > 1)
						{
							p++;
							Array.Resize<Interval>(ref ImRange, p);
							for (int o = p - 1; o >= k + 2; o--)
								ImRange[o] = ImRange[o - 1];

							ImRange[k + 1] = ResRange[1];
							ImRange[k + 1].Tag = i;
							k += 2;
						}
						else
							k++;

					}
					else if (l == 0)
					{
						for (int o = k; o < p - 1; o++)
							ImRange[o] = ImRange[o + 1];

						p--;

						if (p == 0)
						{
							ImRange = new Interval[0];
							Ix = i;
							break;
						}
						else
							Array.Resize<Interval>(ref ImRange, p);
					}
				}
			}
			return ImRange;
		}

		private double CalcNewHFAP(int Ix, ObstacleContainer ObstList, double hFAP)
		{
			double CoTanGPA = 1.0 / TanGPA;
			double Coef300 = 0.5 / GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value;
			double OldFAPRange = hFAP2FAPDist(hFAP);

			double MinNewRange = 40000000.0, fTmp = 0;
			double Ap = GlobalVars.wOASPlanes[(int)eOAS.XrPlane].Plane.A;
			double Bp = GlobalVars.wOASPlanes[(int)eOAS.XrPlane].Plane.B;
			double Cp = GlobalVars.wOASPlanes[(int)eOAS.XrPlane].Plane.D;

			for (int i = 0; i <= Ix; i++)
			{
				double Zx = GlobalVars.wOASPlanes[(int)eOAS.XrPlane].Plane.A * ObstList.Parts[i].Dist + GlobalVars.wOASPlanes[(int)eOAS.XrPlane].Plane.B * ObstList.Parts[i].DistStar + GlobalVars.wOASPlanes[(int)eOAS.XrPlane].Plane.D;
				double Zw = GlobalVars.wOASPlanes[(int)eOAS.WPlane].Plane.A * ObstList.Parts[i].Dist + GlobalVars.wOASPlanes[(int)eOAS.WPlane].Plane.B * ObstList.Parts[i].DistStar + GlobalVars.wOASPlanes[(int)eOAS.WPlane].Plane.D;
				double X0, X1, X4, X5, A, B, C;

				if (Zx > Zw)
					X4 = OldFAPRange + (Zx + GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value - hFAP) * CoTanGPA;
				else
					X4 = OldFAPRange + (Zw + GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value - hFAP) * CoTanGPA;

				X5 = OldFAPRange + (ObstList.Parts[i].MOC + ObstList.Parts[i].Height - hFAP) * CoTanGPA;

				if (hFAP > ObstList.Parts[i].Height)
				{
					A = TanGPA * Coef300 + (TanGPA - Ap) / (Bp * GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value);
					B = ObstList.Parts[i].Rmin * (TanGPA - Ap) / (Bp * GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value) -
						ObstList.Parts[i].Dist * A + (GP_RDH - ObstList.Parts[i].Height) * Coef300 + (GP_RDH - Cp) / (Bp * GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value) - 1.0;
					C = -ObstList.Parts[i].Rmin * (ObstList.Parts[i].DistStar - (GP_RDH - Cp) / Bp) / GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value -
						ObstList.Parts[i].Dist * ((GP_RDH - ObstList.Parts[i].Height) * Coef300 + (GP_RDH - Cp) / (Bp * GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value) - 1.0);
				}
				else
				{
					A = (TanGPA - Ap) / (Bp * GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value);
					B = (ObstList.Parts[i].Dist - ObstList.Parts[i].Rmin) * A + (GP_RDH - Cp) / (Bp * GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value) - 1.0;
					C = ObstList.Parts[i].Dist - ObstList.Parts[i].Rmin * ObstList.Parts[i].DistStar / GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value -
						(ObstList.Parts[i].Dist - ObstList.Parts[i].Rmin) * (GP_RDH - Cp) / (Bp * GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value);
				}

				int Sol1 = Functions.Quadric(A, B, C, out X0, out X1);

				if (Sol1 == 2 && X0 <= OldFAPRange)
					Sol1 = 0;
				int Sol2 = 0;
				//double X2, X3;

				switch (3 * Sol2 + Sol1)
				{
					case 0:
					case 1:
					case 3:
					case 4:
						fTmp = Math.Min(X4, X5);
						break;
					case 2:
					case 5:
						fTmp = Math.Min(X0, Math.Min(X4, X5));
						break;
					case 6:
					case 7:
						//fTmp = Math.Min(X2, Math.Min(X4, X5));
						fTmp = Math.Min(X4, X5);
						break;
					case 8:
						//fTmp = Math.Min(X0, Math.Min(X2, Math.Min(X4, X5)));
						fTmp = Math.Min(X0, Math.Min(X4, X5));
						break;
				}

				if (fTmp <= OldFAPRange)
					fTmp = OldFAPRange + 0.5 * CoTanGPA; //0.5

				if (MinNewRange > fTmp)
					MinNewRange = fTmp + 4.0;
			}

			//return  MinNewRange * TanGPA + GP_RDH;
			return FAPDist2hFAP(MinNewRange);
		}

		private void Caller(ref double OCH, ref double hFAP)
		{
			NativeMethods.ShowPandaBox(this.Handle.ToInt32());

			//================ Initial
			ObstacleContainer IntermObstacleList;
			double fTmp, FAPDist = hFAP2FAPDist(hFAP);

			ptFAP = ARANFunctions.LocalToPrj(FicTHRprj, ArDir + Math.PI, FAPDist);
			ptFAP.Z = hFAP;
			ptFAP.M = ArDir;					//DrawPointWithText PtFAP, "ptFAP"

			NavaidType OldNav = default(NavaidType);
			OldNav.CallSign = "";

			if (CheckBox0101.Enabled && CheckBox0101.Checked && ComboBox0101.SelectedIndex >= 0)
			{
				OldNav = FAPNavDat[ComboBox0101.SelectedIndex];
				CreatePlane15(OldNav, out fTmp);
			}

			IxOCH = -1;
			OCH = CalcFAPOCH(ref hFAP, m_fMOC, out IxOCH);

			ExcludeBtn.Enabled = (IxOCH >= 0) || HaveExcluded;
			//================ Initial

			double distEps2 = ARANMath.Epsilon_2Distance;
			Interval hRange = default(Interval);
			Interval[] ImIntervals;
			hRange.Min = 0.0;
			bHaveSolution = true;
			int ix, i, n = WorkObstacleList.Parts.Length;

			do
			{
				ix = -1;

				for (i = n - 1; i >= 0; i--)
					if (WorkObstacleList.Parts[i].ReqH > hFAP && WorkObstacleList.Parts[i].hPenet > 0.0)
					{
						ix = i;
						break;
					}


				if (ix < 0)
					hRange.Max = FAPDist2hFAP(GlobalVars.constants.Pansops[ePANSOPSData.arFAPMaxRange].Value);
				else
					hRange.Max = WorkObstacleList.Parts[ix].ReqH;

				ImIntervals = CalcImIntervals(hFAP, out IntermObstacleList, out ix);

				if (ix >= 0)
				{
					fTmp = CalcNewHFAP(ix, IntermObstacleList, hFAP);
					//fTmp = CalcNewHFAP(Ix, WorkObstacleList, hFAP)

					if (System.Math.Abs(fTmp - hFAP) < distEps2)
					{
						bHaveSolution = false;
						NativeMethods.HidePandaBox();

						MessageBox.Show("Solution does not exist.", "Error",
							System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
						return;
					}

					hFAP = fTmp;

					FAPDist = hFAP2FAPDist(hFAP);
					ptFAP = ARANFunctions.LocalToPrj(FicTHRprj, ArDir + 180.0, FAPDist);
					ptFAP.Z = hFAP;
					ptFAP.M = ArDir;

					if (CheckBox0101.Enabled && CheckBox0101.Checked && ComboBox0101.SelectedIndex >= 0)
					{
						OldNav = FAPNavDat[ComboBox0101.SelectedIndex];
						CreatePlane15(OldNav, out fTmp);
					}

					OCH = CalcFAPOCH(ref hFAP, m_fMOC, out IxOCH);
				}
			}
			while (ix >= 0);

			//========================================================

			ComboBox0104_SelectedIndexChanged(ComboBox0104, null);

			if (IxOCH > -1)
			{
				TextBox0104.Text = WorkObstacleList.Obstacles[WorkObstacleList.Parts[IxOCH].Owner].UnicalName;
				ToolTip1.SetToolTip(_Label0101_5, WorkObstacleList.Obstacles[WorkObstacleList.Parts[IxOCH].Owner].UnicalName);
			}
			else
			{
				TextBox0104.Text = "";
				ToolTip1.SetToolTip(_Label0101_5, "");
			}
			//========================================================

			if (ImIntervals.Length == 1 && ImIntervals[0].Max < ImIntervals[0].Min)
			{
				bHaveSolution = false;
				NativeMethods.HidePandaBox();

				MessageBox.Show("Solution does not exist.", "Error",
					System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				return;
			}

			FAPDist = hFAP2FAPDist(hFAP);
			fFAPDist = FAPDist;

			//========================================================

			if (ix >= 0)
				TextBox0105.Text = GlobalVars.unitConverter.HeightToDisplayUnits(hRange.Max, eRoundMode.NEAREST).ToString();
			else
				TextBox0105.Text = "";

			//============================================================================
			GlobalVars.gAranGraphics.SafeDeleteGraphic(IntermediateFullAreaElem);
			IntermediateFullAreaElem = GlobalVars.gAranGraphics.DrawMultiPolygon(IntermediateWorkArea, eFillStyle.sfsHollow, 255);

			GeometryOperators pTopo = new GeometryOperators();
			//MultiPolygon pTmpPolygon;

			n = GlobalVars.wOASPlanes.Length;
			for (i = 0; i < n; i++)
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.OASPlanesCat1Element[i]);

				if (GlobalVars.wOASPlanes[i].Poly.IsEmpty)
					continue;

				MultiPolygon pTmpPolygon = (MultiPolygon)pTopo.Difference(GlobalVars.wOASPlanes[i].Poly, IntermediateWorkArea);

				if (!pTmpPolygon.IsEmpty)
					GlobalVars.OASPlanesCat1Element[i] = GlobalVars.gAranGraphics.DrawMultiPolygon(pTmpPolygon, eFillStyle.sfsHollow, -1, GlobalVars.OASPlanesCat1State);
			}

			GlobalVars.VisibilityBar.SetEnabled(GlobalVars.VisibilityBar.OASCat1, true);

			//==============================================================================================================================

			LineString pCutPoly = new LineString();
			pCutPoly.Add(ARANFunctions.LocalToPrj(ptFAP, ArDir + ARANMath.C_PI_2, 2 * GlobalVars.RModel + 10000.0));
			pCutPoly.Add(ARANFunctions.LocalToPrj(ptFAP, ArDir - ARANMath.C_PI_2, 2 * GlobalVars.RModel + 10000.0));

			n = GlobalVars.ILSPlanes.Length;
			for (i = 0; i < n; i++)
			{
				Geometry pRightPolygon = null;
				Geometry pTmpPolygon = null;

				GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.ILSPlanesElement[i]);

				//GlobalVars.gAranGraphics.DrawLineString(pCutPoly, 255, 2);
				//GlobalVars.gAranGraphics.DrawMultiPolygon(GlobalVars.ILSPlanes[i].Poly, -1, eFillStyle.sfsBackwardDiagonal);
				//while(true)
				//Application.DoEvents();

				try
				{
					pTmpPolygon = null;
					pTopo.Cut(GlobalVars.ILSPlanes[i].Poly, pCutPoly, out pTmpPolygon, out pRightPolygon);
				}
				catch { }

				if (pTmpPolygon == null || pTmpPolygon.IsEmpty)
					pTmpPolygon = GlobalVars.ILSPlanes[i].Poly;

				GlobalVars.ILSPlanesElement[i] = GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pTmpPolygon, eFillStyle.sfsHollow, -1, GlobalVars.ILSPlanesState);
			}

			GlobalVars.VisibilityBar.SetEnabled(GlobalVars.VisibilityBar.BasicILS, true);

			//==============================================================================================================================
			GlobalVars.gAranGraphics.SafeDeleteGraphic(FAPElem);
			FAPElem = GlobalVars.gAranGraphics.DrawPointWithText(ptFAP, "FAP", GlobalVars.WPTColor);

			// ILS Obstacles ========================================================

			ObstacleContainer tmpObstacleList;

			int m = ILSObstacleList.Obstacles.Length;
			n = ILSObstacleList.Parts.Length;

			if (n > 0)
			{
				tmpObstacleList.Obstacles = new Obstacle[m];
				tmpObstacleList.Parts = new ObstacleData[n];
			}
			else
			{
				tmpObstacleList.Obstacles = new Obstacle[0];
				tmpObstacleList.Parts = new ObstacleData[0];
			}

			for (i = 0; i < m; i++)
				ILSObstacleList.Obstacles[i].NIx = -1;

			int k = 0, l = 0;

			for (i = 0; i < n; i++)
			{
				if (ILSObstacleList.Parts[i].ReqH > hFAP)
					continue;

				tmpObstacleList.Parts[k] = ILSObstacleList.Parts[i];

				if (ILSObstacleList.Obstacles[ILSObstacleList.Parts[i].Owner].NIx < 0)
				{
					tmpObstacleList.Obstacles[l] = ILSObstacleList.Obstacles[ILSObstacleList.Parts[i].Owner];
					tmpObstacleList.Obstacles[l].PartsNum = 0;
					//tmpObstacleList.Obstacles[l].Parts = new int[ILSObstacleList.Obstacles[ILSObstacleList.Parts[i].Owner].PartsNum];
					ILSObstacleList.Obstacles[ILSObstacleList.Parts[i].Owner].NIx = l;
					l++;
				}

				tmpObstacleList.Parts[k].Owner = ILSObstacleList.Obstacles[ILSObstacleList.Parts[i].Owner].NIx;
				tmpObstacleList.Parts[k].Index = tmpObstacleList.Obstacles[tmpObstacleList.Parts[k].Owner].PartsNum;
				//tmpObstacleList.Obstacles[tmpObstacleList.Parts[k].Owner].Parts[tmpObstacleList.Obstacles[tmpObstacleList.Parts[k].Owner].PartsNum] = k;
				tmpObstacleList.Obstacles[tmpObstacleList.Parts[k].Owner].PartsNum++;
				k++;
			}

			if (k > 0)
			{
				Array.Resize<Obstacle>(ref tmpObstacleList.Obstacles, l);
				Array.Resize<ObstacleData>(ref tmpObstacleList.Parts, k);
			}
			else
			{
				tmpObstacleList.Obstacles = new Obstacle[0];
				tmpObstacleList.Parts = new ObstacleData[0];
			}

			reportFrm.FillPage01(tmpObstacleList);

			//OAS Obstacles ========================================================

			m = OASObstacleList.Obstacles.Length;
			n = OASObstacleList.Parts.Length;

			if (n >= 0)
			{
				tmpObstacleList.Obstacles = new Obstacle[m];
				tmpObstacleList.Parts = new ObstacleData[n];
			}
			else
			{
				tmpObstacleList.Obstacles = new Obstacle[0];
				tmpObstacleList.Parts = new ObstacleData[0];
			}

			for (i = 0; i < m; i++)
				OASObstacleList.Obstacles[i].NIx = -1;

			k = l = 0;

			for (i = 0; i < n; i++)
			{
				if (OASObstacleList.Parts[i].ReqH > hFAP)
					continue;

				tmpObstacleList.Parts[k] = OASObstacleList.Parts[i];

				if (OASObstacleList.Obstacles[OASObstacleList.Parts[i].Owner].NIx < 0)
				{
					tmpObstacleList.Obstacles[l] = OASObstacleList.Obstacles[OASObstacleList.Parts[i].Owner];
					tmpObstacleList.Obstacles[l].PartsNum = 0;
					//tmpObstacleList.Obstacles[l].Parts = new int[OASObstacleList.Obstacles[OASObstacleList.Parts[i].Owner].PartsNum];
					OASObstacleList.Obstacles[OASObstacleList.Parts[i].Owner].NIx = l;
					l++;
				}

				tmpObstacleList.Parts[k].Owner = OASObstacleList.Obstacles[OASObstacleList.Parts[i].Owner].NIx;
				tmpObstacleList.Parts[k].Index = tmpObstacleList.Obstacles[tmpObstacleList.Parts[k].Owner].PartsNum;
				//tmpObstacleList.Obstacles[tmpObstacleList.Parts[k].Owner].Parts[tmpObstacleList.Obstacles[tmpObstacleList.Parts[k].Owner].PartsNum] = k;
				tmpObstacleList.Obstacles[tmpObstacleList.Parts[k].Owner].PartsNum++;
				k++;
			}

			if (k > 0)
			{
				Array.Resize<Obstacle>(ref tmpObstacleList.Obstacles, l);
				Array.Resize<ObstacleData>(ref tmpObstacleList.Parts, k);
			}
			else
			{
				tmpObstacleList.Obstacles = new Obstacle[0];
				tmpObstacleList.Parts = new ObstacleData[0];
			}
			//PrecReportFrm.FillPage02(OASObstacleList);
			reportFrm.FillPage02(tmpObstacleList);

			// Work Obstacles ========================================================

			m = WorkObstacleList.Obstacles.Length;
			n = WorkObstacleList.Parts.Length;

			if (n >= 0)
			{
				tmpObstacleList.Obstacles = new Obstacle[m];
				tmpObstacleList.Parts = new ObstacleData[n];
			}
			else
			{
				tmpObstacleList.Obstacles = new Obstacle[0];
				tmpObstacleList.Parts = new ObstacleData[0];
			}

			for (i = 0; i < m; i++)
				WorkObstacleList.Obstacles[i].NIx = -1;

			k = l = 0;

			for (i = 0; i < n; i++)
			{
				bool fFlg = !(CheckBox0101.Enabled && CheckBox0101.Checked) || (WorkObstacleList.Parts[i].Dist < FAPEarlierToler);

				if (WorkObstacleList.Parts[i].ReqH <= hFAP && fFlg) { }
				else
					continue;

				tmpObstacleList.Parts[k] = WorkObstacleList.Parts[i];

				if (WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].NIx < 0)
				{
					tmpObstacleList.Obstacles[l] = WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner];
					tmpObstacleList.Obstacles[l].PartsNum = 0;
					//tmpObstacleList.Obstacles[l].Parts = new int[WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].PartsNum];
					WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].NIx = l;
					l++;
				}

				tmpObstacleList.Parts[k].Owner = WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].NIx;
				tmpObstacleList.Parts[k].Index = tmpObstacleList.Obstacles[tmpObstacleList.Parts[k].Owner].PartsNum;
				//tmpObstacleList.Obstacles[tmpObstacleList.Parts[k].Owner].Parts[tmpObstacleList.Obstacles[tmpObstacleList.Parts[k].Owner].PartsNum] = k;
				tmpObstacleList.Obstacles[tmpObstacleList.Parts[k].Owner].PartsNum += 1;
				k++;
			}

			if (k > 0)
			{
				Array.Resize<Obstacle>(ref tmpObstacleList.Obstacles, l);
				Array.Resize<ObstacleData>(ref tmpObstacleList.Parts, k);
			}
			else
			{
				tmpObstacleList.Obstacles = new Obstacle[0];
				tmpObstacleList.Parts = new ObstacleData[0];
			}

			reportFrm.FillPage03(tmpObstacleList);

			//========================================================

			string tmpStr;
			System.Windows.Forms.ListViewItem itmX;
			n = ImIntervals.Length;

			ListView0101.Items.Clear();

			for (i = 0; i < n; i++)
			{
				tmpStr = GlobalVars.unitConverter.DistanceToDisplayUnits(ImIntervals[i].Min, eRoundMode.CEIL).ToString();
				itmX = ListView0101.Items.Add(tmpStr);
				itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.DistanceToDisplayUnits(ImIntervals[i].Max, eRoundMode.FLOOR).ToString()));
				if (ImIntervals[i].Tag >= 0)
					itmX.SubItems.Insert(2, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, IntermObstacleList.Obstacles[IntermObstacleList.Parts[ImIntervals[i].Tag].Owner].UnicalName));
			}

			if (n >= 0)
			{
				fNearDist = ImIntervals[0].Min;
				fFarDist = ImIntervals[0].Max;

				itmX = ListView0101.Items[0];
				itmX.Checked = true;

				//?????????????????????????????????????????????????????????????????????????????????????
				ListView0101.SelectedIndices.Clear();
				ListView0101.SelectedIndices.Add(itmX.Index);
				//		ListView0101_ItemClick(itmX)
				ListView0101_ItemChecked(ListView0101, new ItemCheckedEventArgs(itmX));
			}

			//==================================================================================================

			FAPNavDat = Functions.GetValidFAPNavs(FicTHRprj, 2.0 * fFAPDist, ArDir, ptFAP, ptFAP.Z, eNavaidType.LLZ, GARPprj);
			k = 0;

			m = FAPNavDat.Length;
			ComboBox0101.Items.Clear();
			for (i = 0; i < m; i++)
			{
				ComboBox0101.Items.Add(FAPNavDat[i]);
				if (FAPNavDat[i].CallSign == OldNav.CallSign && FAPNavDat[i].TypeCode == OldNav.TypeCode)
					k = i;
			}

			n = InSectList.Length;
			if (n > 0)
			{
				if (m > 0)
					Array.Resize<NavaidType>(ref FAPNavDat, n + m);
				else
					FAPNavDat = new NavaidType[n];

				double MinFAPDist = (GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value - GP_RDH) / TanGPA;

				for (i = 0; i < n; i++)
				{
					if (InSectList[i].TypeCode != eNavaidType.NDB && InSectList[i].TypeCode != eNavaidType.VOR && InSectList[i].TypeCode != eNavaidType.TACAN)
						continue;

					SideDirection Side = ARANMath.SideDef(FicTHRprj, ArDir - ARANMath.C_PI_2, InSectList[i].pPtPrj);
					fTmp = ARANFunctions.Point2LineDistancePrj(FicTHRprj, InSectList[i].pPtPrj, ArDir + ARANMath.C_PI_2);

					if (Side > 0 && fTmp < GlobalVars.constants.Pansops[ePANSOPSData.arFAPMaxRange].Value && fTmp > MinFAPDist)
					{
						NavaidType lNavDat = InSectList[i];

						lNavDat.ValCnt = -2;
						lNavDat.IntersectionType = eIntersectionType.OnNavaid;

						FAPNavDat[m] = lNavDat;
						m++;
						ComboBox0101.Items.Add(lNavDat);

						if (lNavDat.CallSign == OldNav.CallSign && lNavDat.TypeCode == OldNav.TypeCode)
							k = m;
					}
				}
			}

			CheckBox0101.Enabled = m > 0;
			if (m > 0)
			{
				Array.Resize<NavaidType>(ref FAPNavDat, m);
				ComboBox0101.SelectedIndex = k;
			}
			else
				FAPNavDat = new NavaidType[0];

			ArrivalProfile.ClearPoints();
			ArrivalProfile.AddPoint(FAPDist, hFAP, GBASCourse, -GPAngle, CodeProcedureDistance.PFAF);
			double fDist = FAPDist - (hFAP - OCH) / GPAngle;
			ArrivalProfile.AddPoint(fDist, OCH, GBASCourse, ARANMath.RadToDeg(System.Math.Atan(fMissAprPDG)), CodeProcedureDistance.MAP);

			bNavFlg = false;
			NativeMethods.HidePandaBox();
		}

		private void MarkerBtn_Click(object sender, EventArgs e)
		{
			//MrkInfoForm.ShowMrkInfo(ILS.MkrList);
		}

		private void ExcludeBtn_Click(object sender, EventArgs e)
		{
			int i, m = WorkObstacleList.Obstacles.Length, n = WorkObstacleList.Parts.Length;

			ObstacleContainer LocalObstacleList;
			LocalObstacleList.Obstacles = new Obstacle[m];
			LocalObstacleList.Parts = new ObstacleData[n];

			for (i = 0; i < m; i++)
			{
				WorkObstacleList.Obstacles[i].NIx = -1;
				WorkObstacleList.Obstacles[i].Index = i;
			}

			int k = 0, l = 0;

			for (i = 0; i < n; i++)
			{
				//double Dist = ARANFunctions.Point2LineDistancePrj(WorkObstacleList.Parts[i].pPtPrj, FicTHRprj, ArDir + ARANMath.C_PI_2);
				//double Dist1 = ARANFunctions.Point2LineDistancePrj(WorkObstacleList.Parts[i].pPtPrj, FicTHRprj, ArDir);
				//SideDirection Side = ARANMath.SideDef(FicTHRprj, ArDir + ARANMath.C_PI_2, WorkObstacleList.Parts[i].pPtPrj);
				//If (Side = 1) And (Dist < 900.0) And (Dist1 < 150.0) And (WorkObstacleList.Parts(i).ReqOCH > 0.0) Then

				if (WorkObstacleList.Parts[i].ReqOCH > 0.0)
				{
					LocalObstacleList.Parts[k] = WorkObstacleList.Parts[i];

					if (WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].NIx < 0)
					{
						LocalObstacleList.Obstacles[l] = WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner];
						LocalObstacleList.Obstacles[l].PartsNum = 0;
						//LocalObstacleList.Obstacles[l].Parts = new int[WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].PartsNum];
						WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].NIx = l;
						l++;
					}

					LocalObstacleList.Parts[k].Owner = WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].NIx;
					LocalObstacleList.Parts[k].Index = LocalObstacleList.Obstacles[LocalObstacleList.Parts[k].Owner].PartsNum;
					//LocalObstacleList.Obstacles[LocalObstacleList.Parts[k].Owner].Parts[LocalObstacleList.Obstacles[LocalObstacleList.Parts[k].Owner].PartsNum] = k;
					LocalObstacleList.Obstacles[LocalObstacleList.Parts[k].Owner].PartsNum++;
					k++;
				}
			}

			if (k == 0)
			{
				LocalObstacleList.Obstacles = new Obstacle[0];
				LocalObstacleList.Parts = new ObstacleData[0];
			}
			else
			{
				LocalObstacleList.Obstacles = new Obstacle[l];
				LocalObstacleList.Parts = new ObstacleData[k];

				if (excludeObstFrm.ExcludeOstacles(ref LocalObstacleList, this))
				{
					HaveExcluded = false;

					for (i = 0; i < l; i++)
					{
						WorkObstacleList.Obstacles[LocalObstacleList.Obstacles[i].Index].IgnoredByUser = LocalObstacleList.Obstacles[i].IgnoredByUser;
						if (LocalObstacleList.Obstacles[i].IgnoredByUser)
							HaveExcluded = true;
					}

					ComboBox0102_SelectedIndexChanged(ComboBox0102, null);
				}
			}
		}

		private void ComboBox0101_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!bFormInitialised || !ComboBox0101.Enabled)
				return;

			int k = ComboBox0101.SelectedIndex;
			if (k < 0) return;

			double D;
			Plane15 = CreatePlane15(FAPNavDat[k], out D);

			if (FAPNavDat[k].IntersectionType == eIntersectionType.OnNavaid)
			{
				TextBox0101.ReadOnly = true;
				TextBox0102.ReadOnly = true;
				TextBox0101.BackColor = System.Drawing.SystemColors.Control;
				TextBox0102.BackColor = System.Drawing.SystemColors.Control;
			}
			else
			{
				TextBox0101.ReadOnly = false;
				TextBox0102.ReadOnly = false;
				TextBox0101.BackColor = System.Drawing.SystemColors.Window;
				TextBox0102.BackColor = System.Drawing.SystemColors.Window;
			}

			if (!bNavFlg)
			{
				if (FAPNavDat[k].IntersectionType == eIntersectionType.OnNavaid)
				{
					TextBox0101.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(D, eRoundMode.NEAREST).ToString();
					TextBox0102.Tag = Plane15.Plane.pPt.Z;
					ComboBox0103_SelectedIndexChanged(ComboBox0103, null);
				}

				bNavFlg = true;
				TextBox0101.Tag = "";
				TextBox0101_Validating(TextBox0101, null);
			}

			GlobalVars.gAranGraphics.SafeDeleteGraphic(Plane15Elem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(FAP15FIXElem);

			Plane15Elem = GlobalVars.gAranGraphics.DrawMultiPolygon(Plane15.Poly, eFillStyle.sfsHollow, 255 * 256);
			FAP15FIXElem = GlobalVars.gAranGraphics.DrawMultiPolygon(FAP15FIX, eFillStyle.sfsHollow, 128);
		}

		private void ComboBox0102_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!bFormInitialised)
				return;
			if (ComboBox0102.SelectedIndex < 0)
			{
				ComboBox0102.SelectedIndex = 0;
				return;
			}

			switch (ComboBox0102.SelectedIndex)
			{
				case 0:
					arMinISlen = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arMinISlen00_15p].Value[Category];
					break;
				case 1:
					arMinISlen = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arMinISlen16_30p].Value[Category];
					break;
				case 2:
					arMinISlen = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arMinISlen31_60p].Value[Category];
					break;
				case 3:
					arMinISlen = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arMinISlen61_90p].Value[Category];
					break;
			}
			CheckBox0101.Checked = false;
			TextBox0106.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(arMinISlen, eRoundMode.NEAREST).ToString();

			_hFAP = -100.0;
			TextBox0102.Tag = null;
			TextBox0102_Validating(TextBox0102, null);
		}

		private void ComboBox0103_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!bFormInitialised)
				return;
			if (ComboBox0103.SelectedIndex < 0)
			{
				ComboBox0103.SelectedIndex = 0;
				return;
			}

			double fTmp;
			if (!double.TryParse(TextBox0102.Tag.ToString(), out fTmp))
				return;

			if (ComboBox0103.SelectedIndex == 0)
				TextBox0102.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fTmp + FicTHRprj.Z, eRoundMode.NEAREST).ToString();
			else
				TextBox0102.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fTmp, eRoundMode.NEAREST).ToString();
		}

		private void ComboBox0104_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!bFormInitialised)
				return;
			if (ComboBox0104.SelectedIndex < 0)
			{
				ComboBox0104.SelectedIndex = 0;
				return;
			}

			if (ComboBox0104.SelectedIndex == 0)
				TextBox0103.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_CurrFAPOCH + FicTHRprj.Z, eRoundMode.NEAREST).ToString();
			else
				TextBox0103.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_CurrFAPOCH, eRoundMode.NEAREST).ToString();
		}

		private void ComboBox0105_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!bFormInitialised)
				return;

			if (!CheckBox0102.Checked)
				return;

			if (ComboBox0105.SelectedIndex < 0)
				return;

			NavaidType WPT = (NavaidType)ComboBox0105.SelectedItem;

			double fTmpDist = Functions.StartPointDist(GlobalVars.wOASPlanes[(int)eOAS.XlPlane], GlobalVars.wOASPlanes[(int)eOAS.YlPlane], GlobalVars.arHOASPlaneCat1, 0.0);
			fFAPDist = ARANFunctions.Point2LineDistancePrj(WPT.pPtPrj, FicTHRprj, ArDir - ARANMath.C_PI_2);
			// * SideDef(RWYTHRPrj, RWYTHRPrj.M - 90.0, FAFNavDat(i).pPtGeo)

			if (fFAPDist < fTmpDist)
				fFAPDist = fTmpDist;

			_hFAP = FAPDist2hFAP(fFAPDist);

			//fTmpDist = StartPointDist(OASPlanes(eOAS.XlPlane), OASPlanes(eOAS.YlPlane), m_fhFAP, fFAPDist)
			//FAPto150Distance = arMinISlen + fTmpDist

			_CurrFAPOCH = m_fMOC > fRDHOCH ? m_fMOC : fRDHOCH;
			bNavFlg = true;

			Caller(ref _CurrFAPOCH, ref _hFAP);

			TextBox0102.Tag = _hFAP;
			ComboBox0103_SelectedIndexChanged(ComboBox0103, null);

			TextBox0101.Tag = GlobalVars.unitConverter.DistanceToDisplayUnits(fFAPDist, eRoundMode.FLOOR).ToString();
			TextBox0101.Text = TextBox0101.Tag.ToString();
		}

		private void CheckBox0101_CheckedChanged(object sender, EventArgs e)
		{
			if (!bFormInitialised)
				return;

			ComboBox0101.Enabled = CheckBox0101.Checked;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(Plane15Elem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(FAP15FIXElem);
			//GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

			if (CheckBox0101.Checked)
			{
				bNavFlg = false;
				ComboBox0101_SelectedIndexChanged(ComboBox0101, null);
			}
			else
			{
				TextBox0101.ReadOnly = false;
				TextBox0102.ReadOnly = false;
				TextBox0101.BackColor = System.Drawing.SystemColors.Window;
				TextBox0102.BackColor = System.Drawing.SystemColors.Window;

				_CurrFAPOCH = m_fMOC;
				bNavFlg = true;
				Caller(ref _CurrFAPOCH, ref _hFAP);
			}
		}

		private void CheckBox0102_CheckedChanged(object sender, EventArgs e)
		{
			ComboBox0105.Enabled = CheckBox0102.Checked;
			TextBox0101.ReadOnly = CheckBox0102.Checked;
			TextBox0102.ReadOnly = CheckBox0102.Checked;

			if (CheckBox0102.Checked)
			{
				TextBox0101.BackColor = System.Drawing.SystemColors.ButtonFace;
				TextBox0102.BackColor = System.Drawing.SystemColors.ButtonFace;
				ComboBox0105_SelectedIndexChanged(ComboBox0105, null);
			}
			else
			{
				TextBox0101.BackColor = System.Drawing.SystemColors.Window;
				TextBox0102.BackColor = System.Drawing.SystemColors.Window;
			}
		}

		private bool ListView0101_InUse = false;

		private void ListView0101_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			if (ListView0101_InUse || !bFormInitialised)
				return;

			ListView0101_InUse = true;
			System.Windows.Forms.ListViewItem itmX;
			System.Windows.Forms.ListViewItem Item = e.Item;

			try
			{
				int n = ListView0101.Items.Count;

				if (Item.Checked)
				{
					fNearDist = GlobalVars.unitConverter.DistanceToInternalUnits(double.Parse(Item.Text));
					if (Item.SubItems.Count < 2)
						return;
					fFarDist = GlobalVars.unitConverter.DistanceToInternalUnits(double.Parse(Item.SubItems[1].Text));

					for (int i = 0; i < n; i++)
						if (i != Item.Index)
						{
							itmX = ListView0101.Items[i];
							itmX.Checked = false;
						}
				}
				else
				{
					for (int i = 0; i < n; i++)
						if (i != Item.Index)
						{
							itmX = ListView0101.Items[i];
							if (itmX.Checked)
								return;
						}

					ListView0101_InUse = false;
					Item.Checked = true;
				}
			}
			finally
			{
				ListView0101_InUse = false;
			}
		}

		private void ListView0101_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ListView0101.SelectedItems.Count == 0)
				return;

			System.Windows.Forms.ListViewItem Item = ListView0101.SelectedItems[0];

			double fNear = GlobalVars.unitConverter.DistanceToInternalUnits(double.Parse(Item.Text));
			double fFar = GlobalVars.unitConverter.DistanceToInternalUnits(double.Parse(Item.SubItems[1].Text));

			pIFPoly = (MultiPolygon)IntermediateFullArea.Clone();

			LineString pLineTmp = new LineString();

			Point ptTmp = ARANFunctions.LocalToPrj(ptFAP, ArDir + Math.PI, fNear);
			pLineTmp.Add(ARANFunctions.LocalToPrj(ptTmp, ArDir + ARANMath.C_PI_2, 2.0 * GlobalVars.RModel));
			pLineTmp.Add(ARANFunctions.LocalToPrj(ptTmp, ArDir - ARANMath.C_PI_2, 2.0 * GlobalVars.RModel));

			GeometryOperators geomOper = new GeometryOperators();
			Geometry geomLeft, geomRight;

			geomOper.Cut(pIFPoly, pLineTmp, out geomLeft, out geomRight);
			MultiPolygon pTmpPoly = (MultiPolygon)geomRight;
			//ClipByLine(pIFPoly, pLineTmp, Nothing, pTmpPoly, Nothing);

			if (!pTmpPoly.IsEmpty)
				pIFPoly = pTmpPoly;

			ptTmp = ARANFunctions.LocalToPrj(ptFAP, ArDir + Math.PI, fFar);

			pLineTmp.Clear();
			pLineTmp.Add(ARANFunctions.LocalToPrj(ptTmp, ArDir + ARANMath.C_PI_2, 2.0 * GlobalVars.RModel));
			pLineTmp.Add(ARANFunctions.LocalToPrj(ptTmp, ArDir - ARANMath.C_PI_2, 2.0 * GlobalVars.RModel));

			geomOper.Cut(pIFPoly, pLineTmp, out geomLeft, out geomRight);
			pTmpPoly = (MultiPolygon)geomLeft;
			//ClipByLine(pIFPoly, pLineTmp, pTmpPoly, Nothing, Nothing);

			if (!pTmpPoly.IsEmpty)
				pIFPoly = pTmpPoly;
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

			double fTmpDist = Functions.StartPointDist(GlobalVars.wOASPlanes[(int)eOAS.XlPlane], GlobalVars.wOASPlanes[(int)eOAS.YlPlane], GlobalVars.arHOASPlaneCat1, 0.0);
			fTmp = fFAPDist = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

			if (fFAPDist < fTmpDist)
				fFAPDist = fTmpDist;

			_hFAP = FAPDist2hFAP(fFAPDist);

			_CurrFAPOCH = m_fMOC > fRDHOCH ? m_fMOC : fRDHOCH;
			bNavFlg = true;

			Caller(ref _CurrFAPOCH, ref _hFAP);

			TextBox0102.Tag = _hFAP;
			ComboBox0103_SelectedIndexChanged(ComboBox0103, null);

			if (fTmp != fFAPDist)
			{
				TextBox0101.Tag = GlobalVars.unitConverter.DistanceToDisplayUnits(fFAPDist, eRoundMode.FLOOR);
				TextBox0101.Text = TextBox0101.Tag.ToString();
			}
		}

		private void TextBox0102_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0102_Validating(TextBox0102, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0102.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0102_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(TextBox0102.Text, out fTmp))
				return;

			if (TextBox0102.Tag != null && TextBox0102.Tag.ToString() == TextBox0102.Text)
				return;

			double newH = fTmp = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);

			if (ComboBox0103.SelectedIndex == 0)
				fTmp -= FicTHRprj.Z;

			if (fTmp < GlobalVars.arHOASPlaneCat1)
			{
				fTmp = GlobalVars.arHOASPlaneCat1;			//'arISegmentMOC.Value'arISegmentMOC.Value 
				TextBox0102.Tag = _hFAP;
				ComboBox0103_SelectedIndexChanged(ComboBox0103, null);
			}

			if (_hFAP == fTmp)
				return;

			_hFAP = fTmp;

			fFAPDist = hFAP2FAPDist(_hFAP);

			_CurrFAPOCH = m_fMOC > fRDHOCH ? m_fMOC : fRDHOCH;
			bNavFlg = true;
			Caller(ref _CurrFAPOCH, ref _hFAP);

			TextBox0102.Tag = _hFAP;
			ComboBox0103_SelectedIndexChanged(ComboBox0103, null);

			TextBox0101.Tag = GlobalVars.unitConverter.DistanceToDisplayUnits(fFAPDist, eRoundMode.FLOOR);
			TextBox0101.Text = TextBox0101.Tag.ToString();
		}
		#endregion

		#region Page III
		private bool preparePageIII()
		{
			ComboBox0203.Items.Clear();
			ComboBox0201.Items.Clear();

			IFNavDat = Functions.GetValidIFNavs(ptFAP, FicTHRprj.Z, fNearDist, fFarDist, ArDir, GlobalVars.constants.Pansops[ePANSOPSData.arImDescent_Max].Value, eNavaidType.LLZ, GARPprj);

			int n = IFNavDat.Length;
			int m = InSectList.Length;

			if (n > 0)
			{
				if (m > 0)
					Array.Resize<NavaidType>(ref IFNavDat, n + m);
			}
			else if (m > 0)
				IFNavDat = new NavaidType[m];
			else
			{
				IFNavDat = new NavaidType[0];
				MessageBox.Show("Solution does not exist.", "PANDA",
					System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

				return false;
			}

			for (int i = 0; i < m; i++)
			{
				SideDirection Side = ARANMath.SideDef(ptFAP, ArDir - ARANMath.C_PI_2, InSectList[i].pPtPrj);
				double fTmp = ARANFunctions.Point2LineDistancePrj(ptFAP, InSectList[i].pPtPrj, ArDir + ARANMath.C_PI_2);

				if (InSectList[i].TypeCode == eNavaidType.NONE && Side == SideDirection.sideRight && fTmp >= fNearDist && fTmp <= fFarDist)
					ComboBox0203.Items.Add(InSectList[i]);

				if (InSectList[i].TypeCode != eNavaidType.NDB && InSectList[i].TypeCode != eNavaidType.VOR && InSectList[i].TypeCode != eNavaidType.TACAN)
					continue;

				if (Side == SideDirection.sideRight && fTmp >= fNearDist && fTmp <= fFarDist)
				{
					IFNavDat[n] = InSectList[i];
					IFNavDat[n].IntersectionType = eIntersectionType.OnNavaid;
					IFNavDat[n].ValCnt = -2;
					n++;
				}
			}

			if (n == 0)
			{
				IFNavDat = new NavaidType[0];
				MessageBox.Show("Solution does not exist.", "PANDA",
					System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

				return false;
			}

			Array.Resize<NavaidType>(ref IFNavDat, n);

			for (int i = 0; i < n; i++)
				ComboBox0201.Items.Add(IFNavDat[i]);

			ComboBox0201.SelectedIndex = 0;
			if (ComboBox0203.Items.Count > 0)
			{
				CheckBox0201.Enabled = true;
				ComboBox0203.SelectedIndex = 0;
			}
			else
			{
				CheckBox0201.Checked = false;
				CheckBox0201.Enabled = false;
			}

			return true;
		}

		private void ComboBox0202_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!bFormInitialised)
				return;

			double fTmp;
			if (!double.TryParse(TextBox0202.Tag.ToString(), out fTmp))
				return;

			if (ComboBox0202.SelectedIndex < 0)
			{
				ComboBox0202.SelectedIndex = 0;
				return;
			}

			if (ComboBox0202.SelectedIndex == 0)
				fTmp += FicTHRprj.Z;

			TextBox0202.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fTmp, eRoundMode.NEAREST).ToString();
		}

		private void ComboBox0203_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!bFormInitialised)
				return;

			if (!CheckBox0201.Checked) return;
			if (ComboBox0203.SelectedIndex < 0) return;
			if (ComboBox0201.SelectedIndex < 0) return;

			NavaidType WPT = (NavaidType)ComboBox0203.SelectedItem;

			int n = IFNavDat.Length;
			if (n == 0) return;

			NavaidType navaid = (NavaidType)ComboBox0201.SelectedItem;
			//Label0401_25.Text = GetNavTypeName(WPT.TypeCode)
			//PtFAF

			//GlobalVars.gAranGraphics.DrawPointWithText(WPT.pPtPrj, -1, WPT.CallSign);
			//Application.DoEvents();

			if (navaid.IntersectionType == eIntersectionType.ByDistance || navaid.IntersectionType == eIntersectionType.RadarFIX)
			{
				double fDist = ARANFunctions.ReturnDistanceInMeters(navaid.pPtPrj, WPT.pPtPrj);
				TextBox0201.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(fDist - navaid.Disp, eRoundMode.NONE).ToString("0.####");
				if (ARANMath.SideDef(navaid.pPtPrj, ArDir + ARANMath.C_PI_2, WPT.pPtPrj) < 0)
					OptionButton0201.Checked = true;
				else
					OptionButton0202.Checked = true;
			}
			else
			{
				double fDir = ARANFunctions.ReturnAngleInRadians(navaid.pPtPrj, WPT.pPtPrj);
				double fAzt = ARANFunctions.DirToAzimuth(navaid.pPtPrj, fDir, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
				if (navaid.TypeCode == eNavaidType.NDB)
					TextBox0201.Text = NativeMethods.Modulus(fAzt + 180.0 - navaid.MagVar, 360.0).ToString("0.##");
				else
					TextBox0201.Text = NativeMethods.Modulus(fAzt - navaid.MagVar, 360.0).ToString("0.##");
			}

			TextBox0201.Tag = null;
			TextBox0201_Validating(TextBox0201, null);
		}

		private void ComboBox0201_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!bFormInitialised || ComboBox0201.SelectedIndex < 0)
				return;

			NavaidType navaid = (NavaidType)ComboBox0201.SelectedItem;

			_Label0201_3.Text = Functions.GetNavTypeName(navaid.TypeCode);
			TextBox0201.Visible = true;
			string tipStr;

			if (navaid.IntersectionType == eIntersectionType.ByDistance)
			{
				int n = navaid.ValMin.Length;
				_Label0201_12.Text = GlobalVars.unitConverter.DistanceUnit;
				OptionButton0201.Enabled = n > 1;
				OptionButton0202.Enabled = n > 1;

				if (!CheckBox0201.Checked)
				{
					if (OptionButton0202.Checked || n == 1)
						TextBox0201.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(navaid.ValMin[0] - navaid.Disp, eRoundMode.NONE).ToString("0.####");
					else
						TextBox0201.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(navaid.ValMin[1] - navaid.Disp, eRoundMode.NONE).ToString("0.####");
				}

				if (n == 1)
				{
					if (navaid.ValCnt > 0)
						OptionButton0201.Checked = true;
					else
						OptionButton0202.Checked = true;
				}

				_Label0201_1.Text = "DME distance:";
				tipStr = "Recomended range:\n\r";	 //"Рекомендуемый интервал расстояний:"

				for (int i = n - 1; i >= 0; i--)
				{
					tipStr = tipStr + " from " + GlobalVars.unitConverter.DistanceToDisplayUnits(navaid.ValMin[i] - navaid.Disp, eRoundMode.NEAREST).ToString()
						+ " " + GlobalVars.unitConverter.DistanceUnit
										+ " to " + GlobalVars.unitConverter.DistanceToDisplayUnits(navaid.ValMax[i] - navaid.Disp, eRoundMode.NEAREST).ToString()
						+ " " + GlobalVars.unitConverter.DistanceUnit;

					if (i > 0)
						tipStr += "\n\r";

				}
			}
			else
			{
				OptionButton0201.Enabled = false;
				OptionButton0202.Enabled = false;

				if (navaid.IntersectionType == eIntersectionType.OnNavaid)
				{
					CheckBox0201.Checked = false;
					_Label0201_1.Text = "over facility";	 //"Над средством"
					tipStr = "";
					_Label0201_12.Text = "";
					TextBox0201.Visible = false;
				}
				else
				{
					double Kmax, Kmin;
					if (navaid.TypeCode == eNavaidType.NDB)
					{
						_Label0201_1.Text = "Bearing:";			//"Пеленг:"
						Kmax = NativeMethods.Modulus(navaid.ValMax[0] + 180.0 - navaid.MagVar, 360.0);
						Kmin = NativeMethods.Modulus(navaid.ValMin[0] + 180.0 - navaid.MagVar, 360.0);
						tipStr = "Recomended range:\n\r" + " from ";	 //"Рекомендуемый интервал пеленгов:"
					}
					else
					{
						_Label0201_1.Text = "Radial:";			//"Радиал:"
						Kmax = NativeMethods.Modulus(navaid.ValMax[0] - navaid.MagVar, 360.0);
						Kmin = NativeMethods.Modulus(navaid.ValMin[0] - navaid.MagVar, 360.0);
						tipStr = "Recomended range:\n\r" + " from ";	 //"Рекомендуемый интервал радиалов:"
					}

					_Label0201_12.Text = "°";
					tipStr = tipStr + Kmin.ToString("0.##") + " °" + " to " + Kmax.ToString("0.##") + " °";

					if (!CheckBox0201.Checked)
					{
						if (navaid.ValCnt > 0)
							TextBox0201.Text = Kmin.ToString("0.##");
						else
							TextBox0201.Text = Kmax.ToString("0.##");
					}
				}
			}

			TextBox0201.Visible = navaid.IntersectionType != eIntersectionType.OnNavaid;

			_Label0201_2.Text = tipStr;
			tipStr = tipStr.Replace("\n\r", "   ");
			ToolTip1.SetToolTip(TextBox0201, tipStr);

			TextBox0201.Tag = null;
			TextBox0201_Validating(TextBox0201, null);
		}

		private void CheckBox0201_CheckedChanged(object sender, EventArgs e)
		{
			ComboBox0203.Enabled = CheckBox0201.Checked;
			TextBox0201.ReadOnly = CheckBox0201.Checked;

			if (CheckBox0201.Checked)
			{
				//TextBox0201.BackColor = System.Drawing.SystemColors.ButtonFace;
				ComboBox0203_SelectedIndexChanged(ComboBox0203, null);
			}
			//else
			//	TextBox0201.BackColor = System.Drawing.SystemColors.Window;
		}

		private void OptionButton0201_CheckedChanged(object sender, EventArgs e)
		{
			if (!bFormInitialised || !OptionButton0201.Enabled)
				return;

			TextBox0201_Validating(TextBox0201, null);
		}

		private void TextBox0202_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0202_Validating(TextBox0202, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0202.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0202_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(TextBox0202.Text, out fTmp))
				return;

			if (TextBox0202.Tag != null && TextBox0202.Tag.ToString() == TextBox0202.Text)
				return;

			double D = ARANFunctions.Point2LineDistancePrj(ptFAP, IFprj, ArDir + ARANMath.C_PI_2);

			double hIFMin = ptFAP.Z;
			double hIFMax = (D - hDis) * arImDescent_PDG + ptFAP.Z;

			if (ComboBox0202.SelectedIndex == 0)
				fTmp = GlobalVars.unitConverter.HeightToInternalUnits(fTmp) - FicTHRprj.Z;
			else
				fTmp = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);

			hIFFix = fTmp;

			if (fTmp < hIFMin) fTmp = hIFMin;
			if (fTmp > hIFMax) fTmp = hIFMax;

			if (fTmp != hIFFix)
			{
				hIFFix = fTmp;

				TextBox0202.Tag = hIFFix;
				ComboBox0202_SelectedIndexChanged(ComboBox0202, null);
			}
			IFprj.Z = hIFFix;

			if (arImDescent_PDG == 0.0)
				hDis = D;
			else
				hDis = D - (hIFFix - ptFAP.Z) / arImDescent_PDG;

			TextBox0204.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(hDis, eRoundMode.NEAREST).ToString();

			//======================================
			if (ArrivalProfile.PointsNo == 4)
			{
				ArrivalProfile.RemovePointByIndex(0);
				ArrivalProfile.RemovePointByIndex(0);
			}

			double fDis = ARANFunctions.Point2LineDistancePrj(ptFAP, FicTHRprj, ArDir + ARANMath.C_PI_2);
			ArrivalProfile.InsertPoint(D + fDis, hIFFix, GBASCourse, -ARANMath.RadToDeg(System.Math.Atan(arImDescent_PDG)), (CodeProcedureDistance)(-1), 0);

			if (arImDescent_PDG != 0.0)
				ArrivalProfile.InsertPoint(D + fDis - (hIFFix - ptFAP.Z) / arImDescent_PDG, ptFAP.Z, GBASCourse, 0, (CodeProcedureDistance)(-1), 1);
			else
				ArrivalProfile.InsertPoint(D + fDis, ptFAP.Z, GBASCourse, 0, (CodeProcedureDistance)(-1), 1);
		}

		private void TextBox0201_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0201_Validating(TextBox0201, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0201.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0201_Validating(object sender, CancelEventArgs e)
		{
			NavaidType navaid = (NavaidType)ComboBox0201.SelectedItem;

			double fDirl = 0.0, fTmp;
			double Kmin, Kmax;
			int i, n;

			if (navaid.IntersectionType != eIntersectionType.OnNavaid && (!CheckBox0201.Checked || ComboBox0203.SelectedIndex < 0))
			{
				if (!double.TryParse(TextBox0201.Text, out fTmp))
					return;

				if (TextBox0201.Tag != null && TextBox0201.Tag.ToString() == TextBox0201.Text)
					return;

				fDirl = fTmp;
				switch (navaid.IntersectionType)
				{
					case eIntersectionType.ByAngle:

						if (navaid.TypeCode == eNavaidType.NDB)
						{
							Kmax = NativeMethods.Modulus(navaid.ValMax[0] + 180.0 - navaid.MagVar, 360.0);
							Kmin = NativeMethods.Modulus(navaid.ValMin[0] + 180.0 - navaid.MagVar, 360.0);
						}
						else
						{
							Kmax = NativeMethods.Modulus(navaid.ValMax[0] - navaid.MagVar, 360.0);
							Kmin = NativeMethods.Modulus(navaid.ValMin[0] - navaid.MagVar, 360.0);
						}

						if (navaid.ValCnt > 0)
							if (!Functions.AngleInSectorDeg(fDirl, Kmin, Kmax)) fDirl = Kmin;
							else if (!Functions.AngleInSectorDeg(fDirl, Kmin, Kmax)) fDirl = Kmax;

						if (fDirl != fTmp)
							TextBox0201.Text = fDirl.ToString("0.##");

						break;
					case eIntersectionType.ByDistance:
						fDirl = GlobalVars.unitConverter.DistanceToInternalUnits(fDirl) + navaid.Disp;
						fTmp = fDirl;

						n = navaid.ValMin.Length;

						if (OptionButton0202.Checked || n == 1)
						{
							if (fDirl < navaid.ValMin[0]) fDirl = navaid.ValMin[0];
							if (fDirl > navaid.ValMax[0]) fDirl = navaid.ValMax[0];
						}
						else
						{
							if (fDirl < navaid.ValMin[1]) fDirl = navaid.ValMin[1];
							if (fDirl > navaid.ValMax[1]) fDirl = navaid.ValMax[1];
						}

						if (fDirl != fTmp)
							TextBox0201.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(fDirl - navaid.Disp, eRoundMode.NONE).ToString("0.####");

						break;
				}
			}

			GlobalVars.gAranGraphics.SafeDeleteGraphic(IFFIXElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(IntermediateFullAreaElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(IntermediatePrimAreaElem);

			double TrackToler = GlobalVars.constants.NavaidConstants.LLZ.TrackingTolerance;
			Polygon pTmpPoly = new Polygon();
			Ring pTmpRing = new Ring();
			GeometryOperators pTopo = new GeometryOperators();

			pTmpRing.Add(GARPprj );
			pTmpRing.Add(ARANFunctions.LocalToPrj(GARPprj, ArDir - TrackToler + Math.PI, 3.0 * GlobalVars.RModel));
			pTmpRing.Add(ARANFunctions.LocalToPrj(GARPprj, ArDir + TrackToler + Math.PI, 3.0 * GlobalVars.RModel));
			pTmpPoly.ExteriorRing = pTmpRing;

			MultiPolygon pPolyClone = new MultiPolygon();
			pPolyClone.Add(pTmpPoly);
			pTopo.CurrentGeometry = pPolyClone;

			MultiPolygon pSect0, pSect1, pTmpMultiPoly;
			Geometry geomLeft, geomRight;

			Point pt1, pt2, pt3;
			LineString pCutter = new LineString();
			double fDis, Dl, InterToler;

			switch (navaid.IntersectionType)
			{
				case eIntersectionType.OnNavaid:
					Dl = ARANFunctions.Point2LineDistancePrj(ptFAP, navaid.pPtPrj, ArDir + ARANMath.C_PI_2);

					IFprj = (Point)navaid.pPtPrj.Clone();

					hIFFix = Dl * arImDescent_PDG + ptFAP.Z + FicTHRprj.Z - navaid.pPtPrj.Z;
					if (navaid.TypeCode == eNavaidType.NDB)
						Functions.NDBFIXTolerArea(navaid.pPtPrj, ArDir, hIFFix, out pTmpPoly);
					else
						Functions.VORFIXTolerArea(navaid.pPtPrj, ArDir, hIFFix, out pTmpPoly);

					break;

					//pIFTolerArea = new MultiPolygon();
					//pIFTolerArea.Add(pTmpPoly);

				case eIntersectionType.ByAngle:
					if (navaid.TypeCode == eNavaidType.NDB)
					{
						InterToler = GlobalVars.constants.NavaidConstants.NDB.IntersectingTolerance;
						fDis = GlobalVars.constants.NavaidConstants.NDB.Range;
						fDirl = fDirl + Math.PI;
					}
					else
					{
						InterToler = GlobalVars.constants.NavaidConstants.VOR.IntersectingTolerance;
						fDis = GlobalVars.constants.NavaidConstants.VOR.Range;
					}

					if (CheckBox0201.Checked)
					{
						pt3 = ((NavaidType)ComboBox0203.SelectedItem).pPtPrj;
						IFprj = (Point)pt3.Clone();
						fDirl = ARANFunctions.ReturnAngleInRadians(navaid.pPtPrj, IFprj);

						fTmp = ARANFunctions.DirToAzimuth(navaid.pPtPrj, fDirl, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo) - navaid.MagVar;
						if (navaid.TypeCode == eNavaidType.NDB) fTmp += 180.0;
						fTmp = NativeMethods.Modulus(fTmp);
						TextBox0201.Text = fTmp.ToString("0.##");
						TextBox0201.Tag = TextBox0201.Text;
					}
					else
					{
						fDirl = ARANFunctions.AztToDirection(navaid.pPtGeo, fDirl + navaid.MagVar, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);
						Geometry tmpGeom = ARANFunctions.LineLineIntersect(navaid.pPtPrj, fDirl, ptFAP, ArDir);
						IFprj = (Point)tmpGeom;
					}

					pt1 = ARANFunctions.PointAlongPlane(navaid.pPtPrj, fDirl + InterToler, fDis);
					pt2 = ARANFunctions.PointAlongPlane(navaid.pPtPrj, fDirl - InterToler, fDis);

					//DrawPoint IFPnt, 0
					//DrawPoint ptFAP, 255
					pTmpRing = new Ring();

					pTmpRing.Add(navaid.pPtPrj);
					pTmpRing.Add(pt1);
					pTmpRing.Add(pt2);
					pTmpRing.Add(navaid.pPtPrj);

					pTmpPoly = new Polygon();
					pTmpPoly.ExteriorRing = pTmpRing;

					pSect0 = new MultiPolygon();
					pSect0.Add(pTmpPoly);

					//DrawPolygon pPolyClone, 0
					//DrawPolygon pSect0, 255

					pIFTolerArea = (MultiPolygon)pTopo.Intersect(pSect0);
					//DrawPolygon pTmpPoly, RGB(128, 128, 0)
					break;

				case eIntersectionType.ByDistance:
					if (CheckBox0201.Checked && ComboBox0203.SelectedIndex >= 0)
					{
						pt3 = ((NavaidType)ComboBox0203.SelectedItem).pPtPrj;
						IFprj = (Point)pt3.Clone();
						fDirl = ARANFunctions.ReturnAngleInRadians(navaid.pPtPrj, IFprj);

						TextBox0201.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(fDirl - navaid.Disp, eRoundMode.NONE).ToString("0.####");
						TextBox0201.Tag = TextBox0201.Text;
					}
					else
					{
						if (navaid.ValCnt < 0 || (OptionButton0202.Enabled && OptionButton0202.Checked))
							IFprj = ARANFunctions.CircleVectorIntersect(navaid.pPtPrj, fDirl, ptFAP, ArDir);
						else
							IFprj = ARANFunctions.CircleVectorIntersect(navaid.pPtPrj, fDirl, ptFAP, ArDir + Math.PI);
					}

					//fDis = Point2LineDistancePrj(IFPnt, ptPlaneFAP, ArDir + 90.0)
					fDis = ARANFunctions.Point2LineDistancePrj(IFprj, ptFAP, ArDir + ARANMath.C_PI_2);
					hIFFix = fDis * GlobalVars.constants.Pansops[ePANSOPSData.arImDescent_Max].Value + ptFAP.Z + FicTHRprj.Z - navaid.pPtPrj.Z;

					double d0 = System.Math.Sqrt(fDirl * fDirl + hIFFix * hIFFix) * GlobalVars.constants.NavaidConstants.DME.ErrorScalingUp + GlobalVars.constants.NavaidConstants.DME.MinimalError;

					Dl = fDirl + d0;
					pTmpPoly = new Polygon();
					pTmpPoly.ExteriorRing = ARANFunctions.CreateCirclePrj(navaid.pPtPrj, Dl);

					pSect0 = new MultiPolygon();
					pSect0.Add(pTmpPoly);

					pCutter.Add(ARANFunctions.PointAlongPlane(navaid.pPtPrj, ArDir - ARANMath.C_PI_2, Dl + Dl));
					pCutter.Add(ARANFunctions.PointAlongPlane(navaid.pPtPrj, ArDir + ARANMath.C_PI_2, Dl + Dl));

					Dl = fDirl - d0;
					pTmpPoly = new Polygon();
					pTmpPoly.ExteriorRing = ARANFunctions.CreateCirclePrj(navaid.pPtPrj, Dl);

					pSect1 = new MultiPolygon();
					pSect1.Add(pTmpPoly);

					pTmpMultiPoly = (MultiPolygon)pTopo.Difference(pSect0, pSect1);

					//pTopo = pTmpMultiPoly 

					if (ARANMath.SideDef(pCutter[0], ArDir, pCutter[1]) == SideDirection.sideRight) pCutter.Reverse();

					pTopo.Cut(pTmpMultiPoly, pCutter, out geomLeft, out geomRight);

					if (navaid.ValCnt < 0 || (OptionButton0202.Enabled && OptionButton0202.Checked))
						pSect0 = (MultiPolygon)geomRight;
					else
						pSect0 = (MultiPolygon)geomLeft;

					//GlobalVars.gAranGraphics.DrawMultiPolygon(pSect0, -1, eFillStyle.sfsForwardDiagonal);
					//GlobalVars.gAranGraphics.DrawMultiPolygon(pPolyClone, -1, eFillStyle.sfsHorizontal);
					//System.Windows.Forms.Application.DoEvents();
					////while(true)
					//	System.Windows.Forms.Application.DoEvents();

					pIFTolerArea = (MultiPolygon)pTopo.Intersect(pSect0);
					//DrawPolygon pTmpPoly, 0
					break;
			}

			pTmpMultiPoly = pIFTolerArea;
			//while (true)
			//	System.Windows.Forms.Application.DoEvents();

			//GlobalVars.gAranGraphics.DrawMultiPolygon(IntermediateFullArea, 255, eFillStyle.sfsBackwardDiagonal);
			//GlobalVars.gAranGraphics.DrawMultiPolygon(pIFTolerArea, 0, eFillStyle.sfsBackwardDiagonal);
			//System.Windows.Forms.Application.DoEvents();

			//===================================================================
			pTopo.CurrentGeometry = IntermediateFullArea;
			if (pTopo.Disjoint(ptFAP))
			{
				pCutter.Clear();
				pCutter.Add(ARANFunctions.LocalToPrj(ptFAP, ArDir + ARANMath.C_PI_2, 50000.0));
				pCutter.Add(ARANFunctions.LocalToPrj(ptFAP, ArDir - ARANMath.C_PI_2, 50000.0));
				pFAPLine = (LineString)pTopo.Intersect(pCutter);
			}
			else
			{
				LineSegment pLPolyLine = Common.IntersectPlanes(GlobalVars.wOASPlanes[(int)eOAS.XlPlane].Plane, GlobalVars.wOASPlanes[(int)eOAS.YlPlane].Plane, _hFAP - GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value, _hFAP);
				LineSegment pRPolyLine = Common.IntersectPlanes(GlobalVars.wOASPlanes[(int)eOAS.YrPlane].Plane, GlobalVars.wOASPlanes[(int)eOAS.XrPlane].Plane, _hFAP - GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value, _hFAP);

				Common.RotateAndOffset(ArDir + Math.PI, FicTHRprj, pLPolyLine);
				Common.RotateAndOffset(ArDir + Math.PI, FicTHRprj, pRPolyLine);

				pFAPLine = new LineString();
				pFAPLine.Add(pLPolyLine.Start);
				pFAPLine.Add(pRPolyLine.Start);
			}

			if (ARANMath.SideDef(pFAPLine[0], ArDir, pFAPLine[1]) == SideDirection.sideLeft)
				pFAPLine.Reverse();
			//================================================

			//DrawPolyLine pCutter, RGB(0, 0, 255), 2
			//DrawPoint ptFAP, 255

			//GlobalVars.gAranGraphics.DrawLineString(pFAPLine, ARANFunctions.RGB(0, 0, 255), 2);
			//System.Windows.Forms.Application.DoEvents();

			pIFLine = new LineString();
			pIFLine.Add(ARANFunctions.LocalToPrj(IFprj, ArDir + ARANMath.C_PI_2, GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value));
			pIFLine.Add(ARANFunctions.LocalToPrj(IFprj, ArDir - ARANMath.C_PI_2, GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value));

			//GlobalVars.gAranGraphics.DrawLineString(pIFLine, ARANFunctions.RGB(0, 0, 255), 2);
			//System.Windows.Forms.Application.DoEvents();
			//===================================================================

			pCutter.Clear();
			pCutter.Add(ARANFunctions.LocalToPrj(pFAPLine[0], ArDir, 5000.0));
			pCutter.Add(ARANFunctions.LocalToPrj(pFAPLine[0], ArDir + Math.PI, 100000.0));

			//GlobalVars.gAranGraphics.DrawLineString(pCutter, ARANFunctions.RGB(255, 0, 255), 2);
			//System.Windows.Forms.Application.DoEvents();

			pTopo.Cut(IntermediateFullArea, pCutter, out geomLeft, out geomRight);
			//pSect0 = (MultiPolygon)geomLeft;

			pCutter.Clear();
			pCutter.Add(ARANFunctions.LocalToPrj(pFAPLine[1], ArDir, 5000.0));
			pCutter.Add(ARANFunctions.LocalToPrj(pFAPLine[1], ArDir + Math.PI, 100000.0));

			pTopo.Cut(geomLeft, pCutter, out geomLeft, out geomRight);
			pIFFIXPoly = (MultiPolygon)geomRight;

			pCutter.Clear();
			pCutter.Add(ARANFunctions.LocalToPrj(IFprj, ArDir + ARANMath.C_PI_2, 50000.0));
			pCutter.Add(ARANFunctions.LocalToPrj(IFprj, ArDir - ARANMath.C_PI_2, 50000.0));

			//GlobalVars.gAranGraphics.DrawLineString(pCutter, 255, 2);
			//GlobalVars.gAranGraphics.DrawMultiPolygon(pIFFIXPoly, 0, eFillStyle.sfsHorizontal);
			//GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)geomRight, -1, eFillStyle.sfsVertical);
			//while(true)
			//System.Windows.Forms.Application.DoEvents();

			pTopo.Cut(geomRight, pCutter, out geomLeft, out geomRight);
			pTmpRing = new Ring();

			pTmpRing.Add(pFAPLine[0]);
			pTmpRing.Add(pIFLine[0]);
			pTmpRing.Add(pIFLine[1]);
			pTmpRing.Add(pFAPLine[1]);
			pTmpPoly = new Polygon();
			pTmpPoly.ExteriorRing = pTmpRing;

			pSect1 = new MultiPolygon();
			pSect1.Add(pTmpPoly);

			pIFFIXPoly = (MultiPolygon)pTopo.UnionGeometry(geomLeft, pSect1);
			pIFFIXPoly = (MultiPolygon)pTopo.UnionGeometry(pIFFIXPoly, pTmpMultiPoly);

			//GlobalVars.gAranGraphics.DrawMultiPolygon(pIFFIXPoly, 255, eFillStyle.sfsVertical);
			////while(true)
			//System.Windows.Forms.Application.DoEvents();

			//===================================================================

			LineString pTmpLine = new LineString();
			pTmpLine.Add(ARANFunctions.LocalToPrj(pFAPLine[0], ARANFunctions.ReturnAngleInRadians(pIFLine[0], pFAPLine[0]) + ARANMath.C_PI_2, 500.0));
			pTmpLine.Add(pFAPLine[0]);
			pTmpLine.Add(ARANFunctions.LocalToPrj(pIFLine[0], ArDir - ARANMath.C_PI_2, 0.25 * pIFLine.Length));
			pTmpLine.Add(ARANFunctions.LocalToPrj(pTmpLine[2], ArDir + Math.PI, 500.0));

			pTopo.Cut(pIFFIXPoly, pTmpLine, out geomLeft, out geomRight);
			//===================================================================
			pTmpLine.Clear();
			pTmpLine.Add(ARANFunctions.LocalToPrj(pFAPLine[1], ARANFunctions.ReturnAngleInRadians(pIFLine[1], pFAPLine[1]) - ARANMath.C_PI_2, 500.0));
			pTmpLine.Add(pFAPLine[1]);
			pTmpLine.Add(ARANFunctions.LocalToPrj(pIFLine[1], ArDir + ARANMath.C_PI_2, 0.25 * pIFLine.Length));
			pTmpLine.Add(ARANFunctions.LocalToPrj(pTmpLine[2], ArDir + Math.PI, 500.0));

			//while(true)
			//System.Windows.Forms.Application.DoEvents();

			//LineString ls = new LineString();
			//ls.Add(KKhMax.Start);
			//ls.Add(KKhMax.End);
			//GlobalVars.gAranGraphics.DrawLineString(ls, -1, 2);

			//GlobalVars.gAranGraphics.DrawLineString(pTmpLine, -1, 2);
			//GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)geomLeft, -1, eFillStyle.sfsDiagonalCross);
			////while (true)
			//System.Windows.Forms.Application.DoEvents();

			pTopo.Cut(geomLeft, pTmpLine, out geomLeft, out geomRight);//pSect1, IntermediatePrimArea
			IntermediatePrimArea = (MultiPolygon)geomRight;

			//GlobalVars.gAranGraphics.DrawMultiPolygon(IntermediatePrimArea, 0, eFillStyle.sfsDiagonalCross);
			////while (true)
			//System.Windows.Forms.Application.DoEvents();

			//===================================================================
			MultiPolygon pTmpPoly1;

			if (CheckBox0101.Enabled && CheckBox0101.Checked)
			{
				pCutter.Clear();
				pt1 = ARANFunctions.LocalToPrj(FicTHRprj, ArDir + Math.PI, FAPEarlierToler);
				pCutter.Add(ARANFunctions.LocalToPrj(pt1, ArDir + ARANMath.C_PI_2, 100000.0));
				pCutter.Add(ARANFunctions.LocalToPrj(pt1, ArDir - ARANMath.C_PI_2, 100000.0));
				//DrawPolyLine pCutter, 0, 2

				try
				{
					pTopo.Cut(GlobalVars.wOASPlanes[(int)eOAS.CommonPlane].Poly, pCutter, out geomLeft, out geomRight);
					pTmpPoly1 = (MultiPolygon)geomLeft;
				}
				catch
				{
					pTmpPoly1 = GlobalVars.wOASPlanes[(int)eOAS.CommonPlane].Poly;
				}
			}
			else
				pTmpPoly1 = GlobalVars.wOASPlanes[(int)eOAS.CommonPlane].Poly;

			pTmpPoly1 = (MultiPolygon)pTopo.Difference(pTmpPoly1, GlobalVars.wOASPlanes[(int)eOAS.YrPlane].Poly);
			pTmpPoly1 = (MultiPolygon)pTopo.Difference(pTmpPoly1, GlobalVars.wOASPlanes[(int)eOAS.YlPlane].Poly);
			pTmpPoly1 = Functions.RemoveAgnails(pTmpPoly1);

			//ClipByLine(wOASPlanes(eOAS.CommonPlane).Poly, pCutter, pTmpPoly, Nothing, Nothing)
			try
			{
				pTopo.Cut(GlobalVars.wOASPlanes[(int)eOAS.CommonPlane].Poly, pCutter, out geomLeft, out geomRight);
			}
			catch
			{
				geomLeft = null;
			}

			if (geomLeft == null)
			{
				pIFFIXPoly = (MultiPolygon)pTopo.Difference(pIFFIXPoly, pTmpPoly1);
				IntermediatePrimArea = (MultiPolygon)pTopo.Difference(IntermediatePrimArea, pTmpPoly1);
			}
			else
			{
				pTmpMultiPoly = (MultiPolygon)geomLeft;
				pIFFIXPoly = (MultiPolygon)pTopo.Difference(pIFFIXPoly, pTmpMultiPoly);
				IntermediatePrimArea = (MultiPolygon)pTopo.Difference(IntermediatePrimArea, pTmpPoly);
			}

			IntermediateFullAreaElem = GlobalVars.gAranGraphics.DrawMultiPolygon(pIFFIXPoly, eFillStyle.sfsHollow, 255);
			IntermediatePrimAreaElem = GlobalVars.gAranGraphics.DrawMultiPolygon(IntermediatePrimArea, eFillStyle.sfsHollow, 0);
			IFFIXElem = GlobalVars.gAranGraphics.DrawPointWithText(IFprj, "IF", GlobalVars.WPTColor);

			n = GlobalVars.wOASPlanes.Length;

			for (i = 0; i < n; i++)
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.OASPlanesCat1Element[i]);

				if (!GlobalVars.wOASPlanes[i].Poly.IsEmpty)
				{
					geomLeft = null;
					try
					{
						pTopo.Cut(GlobalVars.wOASPlanes[i].Poly, pCutter, out geomLeft, out geomRight);
					}
					catch
					{
					}

					if (geomLeft == null)
						pTmpPoly1 = GlobalVars.wOASPlanes[i].Poly;
					else
						pTmpPoly1 = (MultiPolygon)geomLeft;

					geomLeft = pTopo.Difference(pTmpPoly1, pIFFIXPoly);

					if (!geomLeft.IsEmpty)
					{
						pTmpMultiPoly = (MultiPolygon)geomLeft;
						GlobalVars.OASPlanesCat1Element[i] = GlobalVars.gAranGraphics.DrawMultiPolygon(pTmpMultiPoly, eFillStyle.sfsHollow, 0, GlobalVars.OASPlanesCat1State);
					}
				}
			}

			GlobalVars.VisibilityBar.SetEnabled(GlobalVars.VisibilityBar.OASCat1, true);

			//====================================================================================================
			ObstacleContainer tmpObstacleList;
			double fCutDist;
			int k = 0, l = 0;

			int m = WorkObstacleList.Obstacles.Length;
			n = WorkObstacleList.Parts.Length;

			tmpObstacleList.Obstacles = new Obstacle[m];
			tmpObstacleList.Parts = new ObstacleData[n];

			if (n > 0)
			{
				if (CheckBox0101.Enabled && CheckBox0101.Checked)
					fCutDist = FAPEarlierToler;
				else
					fCutDist = ARANFunctions.ReturnDistanceInMeters(IFprj, FicTHRprj);

				for (i = 0; i < m; i++)
					WorkObstacleList.Obstacles[i].NIx = -1;

				for (i = 0; i < n; i++)
				{
					if (WorkObstacleList.Parts[i].ReqH <= _hFAP && WorkObstacleList.Parts[i].Dist < fCutDist)
					{
						tmpObstacleList.Parts[k] = WorkObstacleList.Parts[i];

						if (WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].NIx < 0)
						{
							tmpObstacleList.Obstacles[l] = WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner];
							tmpObstacleList.Obstacles[l].PartsNum = 0;
							//tmpObstacleList.Obstacles[l].Parts = new int[WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].PartsNum];
							WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].NIx = l;
							l++;
						}

						tmpObstacleList.Parts[k].Owner = WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].NIx;
						tmpObstacleList.Parts[k].Index = tmpObstacleList.Obstacles[tmpObstacleList.Parts[k].Owner].PartsNum;
						//tmpObstacleList.Obstacles[tmpObstacleList.Parts[k].Owner].Parts[tmpObstacleList.Obstacles[tmpObstacleList.Parts[k].Owner].PartsNum] = k;
						tmpObstacleList.Obstacles[tmpObstacleList.Parts[k].Owner].PartsNum++;
						k++;
					}
				}
			}

			if (k > 0)
			{
				Array.Resize<Obstacle>(ref tmpObstacleList.Obstacles, l);
				Array.Resize<ObstacleData>(ref tmpObstacleList.Parts, k);
			}
			else
			{
				tmpObstacleList.Obstacles = new Obstacle[0];
				tmpObstacleList.Parts = new ObstacleData[0];
			}

			reportFrm.FillPage03(tmpObstacleList);
			//====================================================================================================
			TextBox0201.Tag = TextBox0201.Text;
			ObstacleContainer lIntermObstacleList;
			double fDist = ARANFunctions.Point2LineDistancePrj(FicTHRprj, ptFAP, ArDir + ARANMath.C_PI_2);	//hFAP2FAPDist(fhFAP) //(fhFAP - GP_RDH) / TanGPA
			fDis = ARANFunctions.Point2LineDistancePrj(IFprj, ptFAP, ArDir + ARANMath.C_PI_2);

			Functions.GetIntermObstacleList(GlobalVars.ObstacleList, out lIntermObstacleList, FicTHRprj, ArDir, pIFFIXPoly);
			n = lIntermObstacleList.Parts.Length;
			double fMaxReqH = _hFAP;

			for (i = 0; i < n; i++)
			{
				fTmp = lIntermObstacleList.Parts[i].Dist - fDist;

				if (fTmp > 0.0)
				{
					lIntermObstacleList.Parts[i].Rmin = arMinISlen;
					lIntermObstacleList.Parts[i].MOC = 2.0 * GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value *
						(GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value - fHalfFAPWidth -
						fDis * (lIntermObstacleList.Parts[i].DistStar - fHalfFAPWidth) / fTmp) / GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value;
				}
				else
				{
					lIntermObstacleList.Parts[i].Rmin = arMinISlen;
					lIntermObstacleList.Parts[i].MOC = GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value;
				}

				if (lIntermObstacleList.Parts[i].MOC > GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value)
					lIntermObstacleList.Parts[i].MOC = GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value;

				lIntermObstacleList.Parts[i].hPenet = lIntermObstacleList.Parts[i].Height + lIntermObstacleList.Parts[i].MOC - _hFAP;

				lIntermObstacleList.Parts[i].ReqH = lIntermObstacleList.Parts[i].Height + lIntermObstacleList.Parts[i].MOC;
				lIntermObstacleList.Parts[i].Flags = lIntermObstacleList.Parts[i].MOC >= GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value ? -1 : 0;
				if (fMaxReqH < lIntermObstacleList.Parts[i].ReqH)
					fMaxReqH = lIntermObstacleList.Parts[i].ReqH;
			}

			reportFrm.FillPage04(lIntermObstacleList);
			NextBtn.Enabled = fMaxReqH <= _hFAP;
			if (fMaxReqH > _hFAP)
				MessageBox.Show("A minimum of obstacle clearance in the intermediate approach segment cannot be provided due to low FAP height.");

			IFprj.M = ArDir;

			Dl = ARANFunctions.Point2LineDistancePrj(ptFAP, IFprj, ArDir + ARANMath.C_PI_2);
			hDis = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arImHorSegLen].Value[Category];
			hIFFix = (Dl - hDis) * arImDescent_PDG + ptFAP.Z;
			//======================================
			if (ArrivalProfile.PointsNo == 4)
			{
				ArrivalProfile.RemovePointByIndex(0);
				ArrivalProfile.RemovePointByIndex(0);
			}

			fDis = ARANFunctions.Point2LineDistancePrj(ptFAP, FicTHRprj, ArDir + ARANMath.C_PI_2);
			ArrivalProfile.InsertPoint(Dl + fDis, hIFFix, GBASCourse, -ARANMath.RadToDeg(System.Math.Atan(arImDescent_PDG)), (CodeProcedureDistance)(-1), 0);
			if (arImDescent_PDG != 0.0)
				ArrivalProfile.InsertPoint(Dl + fDis - (hIFFix - ptFAP.Z) / arImDescent_PDG, ptFAP.Z, GBASCourse, 0, (CodeProcedureDistance)(-1), 1);
			else
				ArrivalProfile.InsertPoint(Dl + fDis, ptFAP.Z, GBASCourse, 0, (CodeProcedureDistance)(-1), 1);

			//======================================
			TextBox0202.Tag = hIFFix;
			ComboBox0202_SelectedIndexChanged(ComboBox0202, null);
			TextBox0204.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(hDis, eRoundMode.NEAREST).ToString();
			TextBox0205.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(Dl, eRoundMode.NEAREST).ToString();
			IFprj.Z = hIFFix;
			TextBox0206_Validating(TextBox0206, null);
		}

		private void TextBox0203_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0203_Validating(TextBox0203, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0203.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0203_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(TextBox0203.Text, out fTmp))
			{
				if (double.TryParse(TextBox0203.Tag.ToString(), out fTmp))
					TextBox0203.Text = TextBox0203.Tag.ToString();
				return;
			}

			fTmp *= 0.01;

			if (fTmp > GlobalVars.constants.Pansops[ePANSOPSData.arImDescent_Max].Value)
			{
				arImDescent_PDG = GlobalVars.constants.Pansops[ePANSOPSData.arImDescent_Max].Value;
				TextBox0203.Text = (100.0 * arImDescent_PDG).ToString();
			}
			else if (fTmp < 0.0)
			{
				arImDescent_PDG = 0.0;
				TextBox0203.Text = "0";
			}
			else
				arImDescent_PDG = fTmp;


			TextBox0203.Tag = TextBox0203.Text;

			double D = ARANFunctions.Point2LineDistancePrj(ptFAP, IFprj, ArDir + ARANMath.C_PI_2);
			hIFFix = (D - hDis) * arImDescent_PDG + ptFAP.Z;
			IFprj.Z = hIFFix;

			TextBox0202.Tag = hIFFix;
			ComboBox0202_SelectedIndexChanged(ComboBox0202, null);
		}

		private void TextBox0204_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0204_Validating(TextBox0204, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0204.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0204_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;
			if (!double.TryParse(TextBox0204.Text, out fTmp))
				return;

			double D = ARANFunctions.Point2LineDistancePrj(ptFAP, IFprj, ArDir + ARANMath.C_PI_2);
			double hDisMin = 0.0;
			double hDisMax = D;

			hDis = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);
			fTmp = hDis;

			if (fTmp < hDisMin) fTmp = hDisMin;
			if (fTmp > hDisMax) fTmp = hDisMax;

			if (fTmp != hDis)
			{
				hDis = fTmp;
				TextBox0204.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(hDis, eRoundMode.NEAREST).ToString();
			}

			hIFFix = (D - hDis) * arImDescent_PDG + ptFAP.Z;
			IFprj.Z = hIFFix;

			TextBox0202.Tag = hIFFix;
			ComboBox0202_SelectedIndexChanged(ComboBox0202, null);
		}

		private void TextBox0208_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0208_Validating(TextBox0208, null);
			else
				Functions.TextBoxLimitCount(ref eventChar, TextBox0208.Text, 5);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0208_Validating(object sender, CancelEventArgs e)
		{
			//
		}

		private void TextBox0206_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0206_Validating(TextBox0206, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0206.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0206_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;
			if (!double.TryParse(TextBox0206.Text, out fTmp))
				return;

			double fDist = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

			if (fDist < GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value)
			{
				fDist = GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value;
				TextBox0206.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(fDist, eRoundMode.NEAREST).ToString();
			}

			if (fDist > 60000.0)
			{
				fDist = 60000.0;
				TextBox0206.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(fDist, eRoundMode.NEAREST).ToString();
			}

			Point pPtTmp = ARANFunctions.PointAlongPlane(IFprj, ArDir + Math.PI, fDist);
			Ring pTmpRing = new Ring();

			pTmpRing.Add(ARANFunctions.LocalToPrj(IFprj, ArDir + ARANMath.C_PI_2, GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value));
			pTmpRing.Add(ARANFunctions.LocalToPrj(IFprj, ArDir - ARANMath.C_PI_2, GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value));

			pTmpRing.Add(ARANFunctions.LocalToPrj(pPtTmp, ArDir - ARANMath.C_PI_2, GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value));
			pTmpRing.Add(ARANFunctions.LocalToPrj(pPtTmp, ArDir + ARANMath.C_PI_2, GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value));

			Polygon pInitialFullPoly = new Polygon();
			pInitialFullPoly.ExteriorRing = pTmpRing;

			pTmpRing = new Ring();
			pTmpRing.Add(ARANFunctions.LocalToPrj(IFprj, ArDir + ARANMath.C_PI_2, 0.5 * GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value));
			pTmpRing.Add(ARANFunctions.LocalToPrj(IFprj, ArDir - ARANMath.C_PI_2, 0.5 * GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value));

			pTmpRing.Add(ARANFunctions.LocalToPrj(pPtTmp, ArDir - ARANMath.C_PI_2, 0.5 * GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value));
			pTmpRing.Add(ARANFunctions.LocalToPrj(pPtTmp, ArDir + ARANMath.C_PI_2, 0.5 * GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value));

			Polygon pInitialPrimPoly = new Polygon();
			pInitialPrimPoly.ExteriorRing = pTmpRing;

			GeometryOperators pFullRelat = new GeometryOperators();
			pFullRelat.CurrentGeometry = pInitialFullPoly;

			GeometryOperators pPrimRelat = new GeometryOperators();
			pPrimRelat.CurrentGeometry = pInitialPrimPoly;

			int n = GlobalVars.ObstacleList.Obstacles.Length;
			double fMinHeight = GlobalVars.constants.Pansops[ePANSOPSData.arIASegmentMOC].Value;

			for (int i = 0; i < n; i++)
			{
				if (!pFullRelat.Disjoint(GlobalVars.ObstacleList.Obstacles[i].pGeomPrj))
				{
					double k;
					if (!pPrimRelat.Disjoint(GlobalVars.ObstacleList.Obstacles[i].pGeomPrj))
						k = 1.0;
					else
						k = 2 * (GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value -
							ARANFunctions.Geometry2LineDistancePrj(GlobalVars.ObstacleList.Obstacles[i].pGeomPrj, IFprj, ArDir)) /
							GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value;

					double fObstHeight = GlobalVars.ObstacleList.Obstacles[i].Height + GlobalVars.constants.Pansops[ePANSOPSData.arIASegmentMOC].Value * k;
					if (fObstHeight > fMinHeight)
						fMinHeight = fObstHeight;
				}
			}

			TextBox0207.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fMinHeight + FicTHRprj.Z, eRoundMode.SPECIAL_CEIL).ToString();
		}
		#endregion

		#region Page IV
		private bool preparePageIV()
		{
			_maxMAHFDist = GlobalVars.MATMinRange;
			_MAHF2THRDist = _maxMAHFDist;

			ptMAHF = ARANFunctions.LocalToPrj(FicTHRprj, ArDir, _MAHF2THRDist);

			mahfFix = new MATF(GlobalVars.gAranEnv);
			mahfFix.TurnAt = eTurnAt.TP;
			mahfFix.SensorType = eSensorType.GNSS;
			mahfFix.PBNType = ePBNClass.RNP_APCH;
			mahfFix.FlightPhase = eFlightPhase.MApLT28;
			mahfFix.Role = eFIXRole.MAHF_LE_56;
			mahfFix.ISAtC = GlobalVars.CurrADHP.ISAtC;

			mahfFix.EntryDirection = ArDir;
			mahfFix.OutDirection = ArDir;
			mahfFix.PrjPt = ptMAHF;
			mahfFix.Visible = !CheckBox0301.Checked;

			fictFix = new FIX(GlobalVars.gAranEnv);
			fictFix.Visible = false;
			fictFix.FlyMode = eFlyMode.Flyby;
			fictFix.FlightPhase = eFlightPhase.MApGE28;

			fictFix.IAS = _TurnIAS;
			fictFix.ISAtC = GlobalVars.CurrADHP.ISAtC;

			fictFix.EntryDirection = ArDir;
			fictFix.OutDirection = ArDir;

			_zLeg = new LegApch(fictFix, mahfFix, GlobalVars.gAranEnv);
			_zLeg.Altitude = _hFAP;

			TextBox0302.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_CurrFAPOCH, eRoundMode.NEAREST).ToString();
			TextBox0305.Text = TextBox0104.Text;

			TextBox0301.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_maxMAHFDist).ToString();
			TextBox0301.Tag = TextBox0301.Text;

			CreateMAPolygon();
			ComboBox0301_SelectedIndexChanged(ComboBox0301, null);

			ArrivalProfile.MAPtIndex = ArrivalProfile.PointsNo;

			return true;
		}

		private void CreateLegGeom()
		{
			Ring pRing = Functions.ReArrangeRing(GlobalVars.wOASPlanes[(int)eOAS.ZPlane].Poly[0].ExteriorRing, FicTHRprj, ArDir + Math.PI);
			double ASW = ARANFunctions.ReturnDistanceInMeters(pRing[1], pRing[2]);

			/************************************/
			fictFix.PrjPt = new Point(0.5 * (pRing[1].X + pRing[2].X), 0.5 * (pRing[1].Y + pRing[2].Y));

			fictFix.NomLineAltitude = _CurrFAPOCH + FicTHRprj.Z;
			fictFix.SetSemiWidth(0.5 * ASW);

			fictFix.ASW_L = fictFix.SemiWidth;
			fictFix.ASW_R = fictFix.SemiWidth;

			fictFix.ASW_2_L = fictFix.ASW_L;
			fictFix.ASW_2_R = fictFix.ASW_R;

			double DistanceNarToARP = ARANFunctions.ReturnDistanceInMeters(fictFix.PrjPt, GlobalVars.CurrADHP.pPtPrj);
			double DistanceFarToARP = ARANFunctions.ReturnDistanceInMeters(mahfFix.PrjPt, GlobalVars.CurrADHP.pPtPrj);

			bool HaveTransition = DistanceNarToARP < PANSOPSConstantList.PBNInternalTriggerDistance && DistanceFarToARP > PANSOPSConstantList.PBNInternalTriggerDistance;

			WayPoint TransitionFix = null;
			if (HaveTransition)
			{
				TransitionFix = new WayPoint(GlobalVars.gAranEnv);
				TransitionFix.Visible = false;
				TransitionFix.SensorType = eSensorType.GNSS;
				TransitionFix.PBNType = ePBNClass.RNP_APCH;
				TransitionFix.FlightPhase = eFlightPhase.MApLT28;
				TransitionFix.Role = eFIXRole.MAHF_LE_56;
				TransitionFix.ISAtC = GlobalVars.CurrADHP.ISAtC;

				TransitionFix.EntryDirection = ArDir;
				TransitionFix.OutDirection = ArDir;

				TransitionFix.PrjPt = ARANFunctions.CircleVectorIntersect(GlobalVars.CurrADHP.pPtPrj, PANSOPSConstantList.PBNInternalTriggerDistance, FicTHRprj, ArDir);
			}

			double SplayAngle15, DivergenceAngle30, intersectAngle, currWidth = fictFix.ASW_L;
			double x, y;

			DivergenceAngle30 = GlobalVars.constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;
			SplayAngle15 = GlobalVars.constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
			Point ptBaseR, ptBaseL, ptArm, ptTmp;

			/*********************************************************************************/
			MultiPoint LeftPoints = new MultiPoint(), RightPoints = new MultiPoint();

			ptBaseR = pRing[1];
			ptBaseL = pRing[2];

			RightPoints.Add(ptBaseR);
			LeftPoints.Add(ptBaseL);

			if (HaveTransition)
			{
				if (currWidth == TransitionFix.ASW_L)
				{
					ptArm = ARANFunctions.LocalToPrj(TransitionFix.PrjPt, ArDir, -TransitionFix.ATT, -TransitionFix.ASW_L);
					RightPoints.Add(ptArm);
					ptArm = ARANFunctions.LocalToPrj(TransitionFix.PrjPt, ArDir, -TransitionFix.ATT, TransitionFix.ASW_L);
					LeftPoints.Add(ptArm);
				}
				else
				{
					if (currWidth > TransitionFix.ASW_L)
						intersectAngle = DivergenceAngle30;
					else //if (currWidth < TransitionFix.ASW_L)
						intersectAngle = -SplayAngle15;

					ptArm = ARANFunctions.LocalToPrj(TransitionFix.PrjPt, ArDir, -TransitionFix.ATT, -TransitionFix.ASW_L);
					ptTmp = (Point)ARANFunctions.LineLineIntersect(ptBaseR, ArDir + intersectAngle, ptArm, ArDir);

					ARANFunctions.PrjToLocal(fictFix.PrjPt, ArDir, ptTmp, out x, out y);
					if (x > _zLeg.Length)
						ptTmp = (Point)ARANFunctions.LineLineIntersect(ptBaseR, ArDir + intersectAngle, mahfFix.PrjPt, ArDir + ARANMath.C_PI_2);

					RightPoints.Add(ptTmp);
					RightPoints.Add(ptArm);
					ptBaseR = ptArm;

					ptArm = ARANFunctions.LocalToPrj(TransitionFix.PrjPt, ArDir, -TransitionFix.ATT, TransitionFix.ASW_L);
					ptTmp = (Point)ARANFunctions.LineLineIntersect(ptBaseL, ArDir - intersectAngle, ptArm, ArDir);

					ARANFunctions.PrjToLocal(fictFix.PrjPt, ArDir, ptTmp, out x, out y);
					if (x > _zLeg.Length)
						ptTmp = (Point)ARANFunctions.LineLineIntersect(ptBaseL, ArDir - intersectAngle, mahfFix.PrjPt, ArDir + ARANMath.C_PI_2);

					LeftPoints.Add(ptTmp);
					LeftPoints.Add(ptArm);
					ptBaseL = ptArm;

					currWidth = TransitionFix.ASW_L;
				}
			}

			if (currWidth == mahfFix.ASW_L)
			{
				ptArm = ARANFunctions.LocalToPrj(mahfFix.PrjPt, ArDir, 0.0, -mahfFix.ASW_L);
				RightPoints.Add(ptArm);
				ptArm = ARANFunctions.LocalToPrj(mahfFix.PrjPt, ArDir, 0.0, mahfFix.ASW_L);
				LeftPoints.Add(ptArm);
			}
			else
			{
				if (currWidth > mahfFix.ASW_L)
					intersectAngle = DivergenceAngle30;
				else //if (currWidth < mahfFix.ASW_L)
					intersectAngle = -SplayAngle15;

				ptArm = ARANFunctions.LocalToPrj(mahfFix.PrjPt, ArDir, 0.0, -mahfFix.ASW_L);
				ptTmp = (Point)ARANFunctions.LineLineIntersect(ptBaseR, ArDir + intersectAngle, ptArm, ArDir);

				ARANFunctions.PrjToLocal(fictFix.PrjPt, ArDir, ptTmp, out x, out y);
				if (x > _zLeg.Length)
					ptTmp = (Point)ARANFunctions.LineLineIntersect(ptBaseR, ArDir + intersectAngle, mahfFix.PrjPt, ArDir + ARANMath.C_PI_2);

				RightPoints.Add(ptTmp);
				RightPoints.Add(ptArm);

				ptArm = ARANFunctions.LocalToPrj(mahfFix.PrjPt, ArDir, 0.0, mahfFix.ASW_L);
				ptTmp = (Point)ARANFunctions.LineLineIntersect(ptBaseL, ArDir - intersectAngle, ptArm, ArDir);

				ARANFunctions.PrjToLocal(fictFix.PrjPt, ArDir, ptTmp, out x, out y);
				if (x > _zLeg.Length)
					ptTmp = (Point)ARANFunctions.LineLineIntersect(ptBaseL, ArDir - intersectAngle, mahfFix.PrjPt, ArDir + ARANMath.C_PI_2);

				LeftPoints.Add(ptTmp);
				LeftPoints.Add(ptArm);
			}

			/***********************************/
			LeftPoints.Reverse();
			Ring ZContRing = new Ring();
			ZContRing.AddMultiPoint(RightPoints);
			ZContRing.AddMultiPoint(LeftPoints);

			//GlobalVars.gAranGraphics.DrawRing(ZContRing, -1, eFillStyle.sfsVertical);
			//Application.DoEvents();

			Polygon pPolygon = new Polygon();
			pPolygon.ExteriorRing = ZContRing;
			ZContinuedFull = new MultiPolygon();
			ZContinuedFull.Add(pPolygon);

			/*********************************************************************************/
			currWidth = fictFix.ASW_2_L;
			LeftPoints = new MultiPoint(); RightPoints = new MultiPoint();
			ptBaseR = pRing[1];
			ptBaseL = pRing[2];

			RightPoints.Add(ptBaseR);
			LeftPoints.Add(ptBaseL);

			if (HaveTransition)
			{
				if (currWidth == TransitionFix.ASW_2_L)
				{
					ptArm = ARANFunctions.LocalToPrj(TransitionFix.PrjPt, ArDir, -TransitionFix.ATT, -TransitionFix.ASW_2_L);
					RightPoints.Add(ptArm);
					ptArm = ARANFunctions.LocalToPrj(TransitionFix.PrjPt, ArDir, -TransitionFix.ATT, TransitionFix.ASW_2_L);
					LeftPoints.Add(ptArm);
				}
				else
				{
					if (currWidth > TransitionFix.ASW_2_L)
						intersectAngle = DivergenceAngle30;
					else //if (currWidth < TransitionFix.ASW_L)
						intersectAngle = -SplayAngle15;

					ptArm = ARANFunctions.LocalToPrj(TransitionFix.PrjPt, ArDir, -TransitionFix.ATT, -TransitionFix.ASW_2_L);
					ptTmp = (Point)ARANFunctions.LineLineIntersect(ptBaseR, ArDir + intersectAngle, ptArm, ArDir);

					ARANFunctions.PrjToLocal(fictFix.PrjPt, ArDir, ptTmp, out x, out y);
					if (x > _zLeg.Length)
						ptTmp = (Point)ARANFunctions.LineLineIntersect(ptBaseR, ArDir + intersectAngle, mahfFix.PrjPt, ArDir + ARANMath.C_PI_2);

					RightPoints.Add(ptTmp);
					RightPoints.Add(ptArm);
					ptBaseR = ptArm;

					ptArm = ARANFunctions.LocalToPrj(TransitionFix.PrjPt, ArDir, -TransitionFix.ATT, TransitionFix.ASW_2_L);
					ptTmp = (Point)ARANFunctions.LineLineIntersect(ptBaseL, ArDir - intersectAngle, ptArm, ArDir);

					ARANFunctions.PrjToLocal(fictFix.PrjPt, ArDir, ptTmp, out x, out y);
					if (x > _zLeg.Length)
						ptTmp = (Point)ARANFunctions.LineLineIntersect(ptBaseL, ArDir - intersectAngle, mahfFix.PrjPt, ArDir + ARANMath.C_PI_2);

					LeftPoints.Add(ptTmp);
					LeftPoints.Add(ptArm);
					ptBaseL = ptArm;
				}
				currWidth = TransitionFix.ASW_2_L;
			}

			if (currWidth == mahfFix.ASW_2_L)
			{
				ptArm = ARANFunctions.LocalToPrj(mahfFix.PrjPt, ArDir, 0.0, -mahfFix.ASW_2_L);
				RightPoints.Add(ptArm);
				ptArm = ARANFunctions.LocalToPrj(mahfFix.PrjPt, ArDir, 0.0, mahfFix.ASW_2_L);
				LeftPoints.Add(ptArm);
			}
			else
			{
				if (currWidth > mahfFix.ASW_2_L)
					intersectAngle = DivergenceAngle30;
				else //if (fictFix.ASW_2_L < 0.5 * mahfFix.ASW_L)
					intersectAngle = -SplayAngle15;

				ptArm = ARANFunctions.LocalToPrj(mahfFix.PrjPt, ArDir, 0.0, -mahfFix.ASW_2_L);
				ptTmp = (Point)ARANFunctions.LineLineIntersect(ptBaseR, ArDir + intersectAngle, ptArm, ArDir);

				ARANFunctions.PrjToLocal(fictFix.PrjPt, ArDir, ptTmp, out x, out y);
				if (x > _zLeg.Length)
					ptTmp = (Point)ARANFunctions.LineLineIntersect(ptBaseR, ArDir + intersectAngle, mahfFix.PrjPt, ArDir + ARANMath.C_PI_2);

				RightPoints.Add(ptTmp);
				RightPoints.Add(ptArm);

				ptArm = ARANFunctions.LocalToPrj(mahfFix.PrjPt, ArDir, 0.0, mahfFix.ASW_2_L);
				ptTmp = (Point)ARANFunctions.LineLineIntersect(ptBaseL, ArDir - intersectAngle, ptArm, ArDir);
				ARANFunctions.PrjToLocal(fictFix.PrjPt, ArDir, ptTmp, out x, out y);
				if (x > _zLeg.Length)
					ptTmp = (Point)ARANFunctions.LineLineIntersect(ptBaseL, ArDir - intersectAngle, mahfFix.PrjPt, ArDir + ARANMath.C_PI_2);

				LeftPoints.Add(ptTmp);
				LeftPoints.Add(ptArm);
			}

			//while(true)
			//Application.DoEvents();
			/***********************************/
			LeftPoints.Reverse();
			ZContRing = new Ring();
			ZContRing.AddMultiPoint(RightPoints);
			ZContRing.AddMultiPoint(LeftPoints);

			//GlobalVars.gAranGraphics.DrawRing(ZContRing, -1, eFillStyle.sfsHorizontal);
			//Application.DoEvents();

			pPolygon = new Polygon();
			pPolygon.ExteriorRing = ZContRing;
			ZContinuedPrim = new MultiPolygon();
			ZContinuedPrim.Add(pPolygon);
		}

		private double CreateMAPolygon()
		{
			CreateLegGeom();

			double CoTanGPA = 1.0 / TanGPA;
			double CoTanZ = 1.0 / fMissAprPDG;

			/*********************************************************************************/

			Ring pRing = Functions.ReArrangeRing(ZContinuedFull[0].ExteriorRing, FicTHRprj, ArDir);
			Point ptTmp = ARANFunctions.LocalToPrj(FicTHRprj, ArDir, GlobalVars.ModellingRadius);
			Ring pRing1 = Functions.ReArrangeRing(GlobalVars.wOASPlanes[(int)eOAS.CommonPlane].Poly[0].ExteriorRing, FicTHRprj, ArDir + Math.PI);

			LeftLine = new LineString();
			RightLine = new LineString();

			LeftLine.Add(pRing1[1]);
			LeftLine.Add(pRing[3]);
			LeftLine.Add(pRing[2]);

			RightLine.Add(pRing1[pRing1.Count - 2]);
			RightLine.Add(pRing[0]);
			RightLine.Add(pRing[1]);
			//============================================================
			Ring pFullPolyRing = new Ring();

			pFullPolyRing.AddMultiPoint(RightLine);
			pFullPolyRing.Add(LeftLine[2]);
			pFullPolyRing.Add(LeftLine[1]);
			pFullPolyRing.Add(LeftLine[0]);

			Polygon pPolygon = new Polygon();
			pPolygon.ExteriorRing = pFullPolyRing;
			pFullPoly = new MultiPolygon();
			pFullPoly.Add(pPolygon);

			PtCoordCntr = new Point(0.5 * (LeftLine[0].X + RightLine[0].X), 0.5 * (LeftLine[0].Y + RightLine[0].Y));

			XptLH = ARANFunctions.ReturnDistanceInMeters(FicTHRprj, PtCoordCntr);
			//============================================================
			GlobalVars.gAranGraphics.SafeDeleteGraphic(ZContinuedFullElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(ZContinuedPrimElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(SOCElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(pMAPtElem);

			ZContinuedFullElem = GlobalVars.gAranGraphics.DrawMultiPolygon(ZContinuedFull, eFillStyle.sfsHollow, GlobalVars.SecRElemColor);
			ZContinuedPrimElem = GlobalVars.gAranGraphics.DrawMultiPolygon(ZContinuedPrim, eFillStyle.sfsHollow, GlobalVars.PrimElemColor);

			Functions.GetIntermObstacleList(GlobalVars.ObstacleList, out MAObstacleList, FicTHRprj, ArDir, ZContinuedFull, ZContinuedPrim);

			int i, MAPrCnt = MAObstacleList.Parts.Length,
				MAObCnt = MAObstacleList.Obstacles.Length;

			int ix = -1;
			fMisAprOCH = _CurrFAPOCH;

			for (i = 0; i < MAPrCnt; i++)
			{
				MAObstacleList.Parts[i].Plane = (int)eOAS.NonPrec;
				MAObstacleList.Parts[i].Dist = -MAObstacleList.Parts[i].Dist;
				MAObstacleList.Parts[i].EffectiveHeight = (MAObstacleList.Parts[i].Height * CoTanZ + (_zSurfaceOrigin + MAObstacleList.Parts[i].Dist)) / (CoTanZ + CoTanGPA);
				MAObstacleList.Parts[i].hSurface = -(MAObstacleList.Parts[i].Dist + _zSurfaceOrigin) * fMissAprPDG;
				MAObstacleList.Parts[i].hPenet = MAObstacleList.Parts[i].Height - MAObstacleList.Parts[i].hSurface;
				MAObstacleList.Parts[i].Flags = 1;
				MAObstacleList.Parts[i].ReqOCH = Math.Min(MAObstacleList.Parts[i].Height, MAObstacleList.Parts[i].EffectiveHeight) + m_fMOC * MAObstacleList.Parts[i].fSecCoeff;

				if (MAObstacleList.Parts[i].ReqOCH > fMisAprOCH && MAObstacleList.Parts[i].hPenet > 0.0 && MAObstacleList.Parts[i].ReqH <= _hFAP)
				{
					fMisAprOCH = MAObstacleList.Parts[i].ReqOCH;
					ix = i;
				}
			}

			//===========================================
			int WrObCnt = WorkObstacleList.Obstacles.Length;
			int WrPrCnt = WorkObstacleList.Parts.Length;

			if (MAPrCnt > 0)
			{
				Array.Resize<ObstacleData>(ref MAObstacleList.Parts, WrPrCnt + MAPrCnt);
				Array.Resize<Obstacle>(ref MAObstacleList.Obstacles, WrObCnt + MAObCnt);
			}
			else if (WrPrCnt > 0)
			{
				MAObstacleList.Obstacles = new Obstacle[WrObCnt];
				MAObstacleList.Parts = new ObstacleData[WrPrCnt];
			}
			else
			{
				MAObstacleList.Obstacles = new Obstacle[0];
				MAObstacleList.Parts = new ObstacleData[0];
			}

			for (i = 0; i < WrObCnt; i++)
				WorkObstacleList.Obstacles[i].NIx = -1;

			for (i = 0; i < WrPrCnt; i++)
			{
				if (WorkObstacleList.Parts[i].EffectiveHeight >= WorkObstacleList.Parts[i].Height)
					continue;

				MAObstacleList.Parts[MAPrCnt] = WorkObstacleList.Parts[i];

				if (WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].NIx < 0)
				{
					MAObstacleList.Obstacles[MAObCnt] = WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner];
					MAObstacleList.Obstacles[MAObCnt].PartsNum = 0;
					//MAObstacleList.Obstacles[MAObCnt].Parts = new int[WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].PartsNum];
					WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].NIx = MAObCnt;
					MAObCnt++;
				}

				MAObstacleList.Parts[MAPrCnt].Owner = WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].NIx;
				MAObstacleList.Parts[MAPrCnt].Index = MAObstacleList.Obstacles[MAObstacleList.Parts[MAPrCnt].Owner].PartsNum;
				//MAObstacleList.Obstacles[MAObstacleList.Parts[MAPrCnt].Owner].Parts[MAObstacleList.Obstacles[MAObstacleList.Parts[MAPrCnt].Owner].PartsNum] = MAPrCnt;
				MAObstacleList.Obstacles[MAObstacleList.Parts[MAPrCnt].Owner].PartsNum++;

				MAObstacleList.Parts[MAPrCnt].EffectiveHeight = (MAObstacleList.Parts[MAPrCnt].Height * CoTanZ + (_zSurfaceOrigin + MAObstacleList.Parts[MAPrCnt].Dist)) / (CoTanZ + CoTanGPA);
				MAObstacleList.Parts[MAPrCnt].ReqOCH = Math.Min(MAObstacleList.Parts[MAPrCnt].Height, MAObstacleList.Parts[MAPrCnt].EffectiveHeight) + m_fMOC * MAObstacleList.Parts[i].fSecCoeff;

				if (MAObstacleList.Parts[MAPrCnt].ReqOCH > fMisAprOCH && MAObstacleList.Parts[MAPrCnt].hPenet > 0.0 && MAObstacleList.Parts[MAPrCnt].ReqH <= _hFAP)
				{
					fMisAprOCH = MAObstacleList.Parts[MAPrCnt].ReqOCH;
					ix = MAPrCnt;
				}

				MAPrCnt++;
			}

			if (MAPrCnt > 0)
			{
				Array.Resize<ObstacleData>(ref MAObstacleList.Parts, MAPrCnt);
				Array.Resize<Obstacle>(ref MAObstacleList.Obstacles, MAObCnt);
			}
			else
			{
				MAObstacleList.Obstacles = new Obstacle[0];
				MAObstacleList.Parts = new ObstacleData[0];
			}
			//===========================================

			_SOC2THRDist = (fMisAprOCH - m_fMOC) * CoTanGPA - _zSurfaceOrigin;
			PtSOC = ARANFunctions.LocalToPrj(FicTHRprj, ArDir, -_SOC2THRDist);
			PtSOC.Z = fMisAprOCH - m_fMOC;
			PtSOC.M = ArDir;

			double YptSOC;
			ARANFunctions.PrjToLocal(PtCoordCntr, ArDir, PtSOC, out XptSOC, out YptSOC);

			TextBox0304.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_SOC2THRDist, eRoundMode.NEAREST).ToString();

			_MAPt2THRDist = (fMisAprOCH - GP_RDH) * CoTanGPA;
			pMAPt = ARANFunctions.LocalToPrj(FicTHRprj, ArDir, -_MAPt2THRDist);
			pMAPt.Z = fMisAprOCH;
			pMAPt.M = ArDir;

			//maptFix.TurnAltitude = fMisAprOCH + FicTHRprj.Z;
			//maptFix.PrjPt = pMAPt;

			SOCElem = GlobalVars.gAranGraphics.DrawPointWithText(PtSOC, "SOC", GlobalVars.WPTColor);
			pMAPtElem = GlobalVars.gAranGraphics.DrawPointWithText(pMAPt, "MAPt", GlobalVars.WPTColor);

			TextBox0303.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fMisAprOCH, eRoundMode.NEAREST).ToString();

			if (ix >= 0)
				TextBox0306.Text = MAObstacleList.Obstacles[MAObstacleList.Parts[ix].Owner].UnicalName;
			else
				TextBox0306.Text = "-";

			if (fMisAprOCH > _CurrFAPOCH ||  fMisAprOCH > _hFAP) 
				TextBox0303.ForeColor = System.Drawing.Color.Red;
			else
				TextBox0303.ForeColor = System.Drawing.Color.Black;

			reportFrm.FillPage05(MAObstacleList);

			return fMisAprOCH;
		}

		private void CreateMAHF(double MAHFDistance, NavaidType nav = default(NavaidType ), bool ByDistance = true)
		{
			if (ByDistance)
				ptMAHF = ARANFunctions.LocalToPrj(FicTHRprj, ArDir, MAHFDistance);
			else
				ptMAHF = (Point)nav.pPtPrj.Clone();

			mahfFix.PrjPt = ptMAHF;
			//*********************************************************************//

			double fDist = ARANMath.Hypot(ptMAHF.X - GlobalVars.CurrADHP.pPtPrj.X, ptMAHF.Y - GlobalVars.CurrADHP.pPtPrj.Y);

			if (fDist >= PANSOPSConstantList.PBNInternalTriggerDistance)
				mahfFix.FlightPhase = eFlightPhase.MApGE28;
			else
				mahfFix.FlightPhase = eFlightPhase.MApLT28;

			//*******************************************************************//

			CreateMAPolygon();

			ptMAHF.Z = PtSOC.Z + (MAHFDistance - _SOC2THRDist) * fMissAprPDG;

			_maxTermAlt = ptMAHF.Z + FicTHRprj.Z;
			if (_maxTermAlt < _minTermAlt)
				_maxTermAlt = _minTermAlt;

			mahfFix.NomLineAltitude = _maxTermAlt;

			mahfFix.DeleteGraphics();
			mahfFix.RefreshGraphics();

			TextBox0308.Tag = null;
			TextBox0308_Validating(TextBox0308, null);
		}

		private void CheckBox0301_CheckedChanged(object sender, EventArgs e)
		{
			if (!bFormInitialised)
				return;

			mahfFix.Visible = !CheckBox0301.Checked;
			Label0301_20.Enabled = !CheckBox0301.Checked;
			Frame0301.Enabled = !CheckBox0301.Checked;
			OkBtn.Enabled = !CheckBox0301.Checked;
			NextBtn.Enabled = CheckBox0301.Checked;

			TextBox0301.Tag = null;
			TextBox0301_Validating(TextBox0301, null);
		}

		private void ComboBox0301_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (MultiPage1.SelectedIndex < 1 || !bFormInitialised)
				return;

			if (ComboBox0301.SelectedIndex < 0)
			{
				ComboBox0301.SelectedIndex = 0;
				return;
			}

			_EnRoteMOC = GlobalVars.unitConverter.HeightToInternalUnits(double.Parse(ComboBox0301.Text));
			_straightMissedTermHeight = _EnRoteMOC;

			int n = MAObstacleList.Parts.Length;

			for (int i = 0; i < n; i++)
			{
				double fReqH = MAObstacleList.Parts[i].Height + _EnRoteMOC;

				if (fReqH > _straightMissedTermHeight)
					_straightMissedTermHeight = fReqH;
			}

			_minTermAlt = GlobalVars.unitConverter.HeightToDisplayUnits(_straightMissedTermHeight + FicTHRprj.Z, eRoundMode.SPECIAL_CEIL);
			TextBox0307.Text = _minTermAlt.ToString();
			_minTermAlt = GlobalVars.unitConverter.HeightToInternalUnits(_minTermAlt);

			_minMAHFDist = _SOC2THRDist + (_straightMissedTermHeight - PtSOC.Z) / fMissAprPDG;
			double fTmp;
			if (!double.TryParse(TextBox0309.Text, out fTmp))
				TextBox0309.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_minMAHFDist).ToString();

			if (!double.TryParse(TextBox0308.Text, out fTmp))
				TextBox0308.Text = TextBox0307.Text;
			else
			{
				TextBox0308.Tag = null;
				TextBox0308_Validating(TextBox0308, null);
			}

			Guid prevPt = default(Guid);
			if (ComboBox0302.SelectedIndex >= 0)
				prevPt = ((NavaidType)ComboBox0302.SelectedItem).Identifier;

			int previ = 0;
			n = InSectList.Length;
			ComboBox0302.Items.Clear();

			for (int i = 0; i < n; i++)
			{
				double X, Y;
				ARANFunctions.PrjToLocal(FicTHRprj, ArDir, InSectList[i].pPtPrj, out X, out Y);

				if (X < _minMAHFDist) continue;
				if (X > _maxMAHFDist) continue;

				ComboBox0302.Items.Add(InSectList[i]);
				if (InSectList[i].Identifier == prevPt)
					previ = i;
			}

			if (ComboBox0302.Items.Count > 0)
			{
				RadioButton0302.Enabled = true;
				ComboBox0302.SelectedIndex = previ;
			}
			else
			{
				RadioButton0302.Enabled = false;
				RadioButton0302.Checked = false;
			}

			if (!RadioButton0302.Checked)
			{
				TextBox0309.Tag = null;
				TextBox0309_Validating(TextBox0309, null);
			}
		}

		private void RadioButton0301_CheckedChanged(object sender, EventArgs e)
		{
			if (!bFormInitialised) return;

			if (!((RadioButton)sender).Checked)
				return;

			if (RadioButton0301.Checked)
			{
				TextBox0309.ReadOnly = false;
				ComboBox0302.Enabled = false;
				TextBox0309_Validating(TextBox0309, null);
			}
			else
			{
				TextBox0309.ReadOnly = true;
				ComboBox0302.Enabled = true;
				ComboBox0302_SelectedIndexChanged(ComboBox0302, null);
			}
		}

		private void ComboBox0302_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!bFormInitialised)
				return;

			int k = ComboBox0302.SelectedIndex;
			if (k < 0)
				return;

			NavaidType newMAHF = (NavaidType)ComboBox0302.SelectedItem;

			if (newMAHF.TypeCode == eNavaidType.NONE)
				Label0301_19.Text = "WPT/FIX";
			else
				Label0301_19.Text = newMAHF.TypeCode.ToString();

			CreateMAHF(0, newMAHF, false);
		}

		private void TextBox0301_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0301_Validating(TextBox0301, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0301.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0301_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(TextBox0301.Text, out fTmp))
				return;

			if (TextBox0301.Tag != null && TextBox0301.Tag.ToString() == TextBox0301.Text)
				return;

			_maxMAHFDist = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

			fTmp = _maxMAHFDist;

			if (_maxMAHFDist < GlobalVars.MATMinRange)
				_maxMAHFDist = GlobalVars.MATMinRange;
			if (_maxMAHFDist > GlobalVars.ModellingRadius)
				_maxMAHFDist = GlobalVars.ModellingRadius;

			if (_maxMAHFDist != fTmp)
				TextBox0301.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_maxMAHFDist).ToString();

			TextBox0301.Tag = TextBox0301.Text;
			if (CheckBox0301.Checked)
				CreateMAHF(_maxMAHFDist);
			else
			{
				TextBox0309.Tag = null;
				TextBox0309_Validating(TextBox0309, null);
			}
		}

		private void TextBox0308_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0308_Validating(TextBox0308, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0308.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0308_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(TextBox0308.Text, out fTmp))
				return;

			if (TextBox0308.Tag != null && TextBox0308.Tag.ToString() == TextBox0308.Text)
				return;

			_MAHFTerminationAlt = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);

			fTmp = _MAHFTerminationAlt;
			if (_MAHFTerminationAlt < _minTermAlt)
				_MAHFTerminationAlt = _minTermAlt;
			if (_MAHFTerminationAlt > _maxTermAlt)
				_MAHFTerminationAlt = _maxTermAlt;

			if (fTmp != _MAHFTerminationAlt)
				TextBox0308.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_MAHFTerminationAlt).ToString();

			TextBox0308.Tag = TextBox0308.Text;
		}

		private void TextBox0309_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0309_Validating(TextBox0309, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0309.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0309_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(TextBox0309.Text, out fTmp))
				return;

			if (TextBox0309.Tag != null && TextBox0309.Tag.ToString() == TextBox0309.Text)
				return;

			_MAHF2THRDist = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

			fTmp = _MAHF2THRDist;
			if (_MAHF2THRDist < _minMAHFDist)
				_MAHF2THRDist = _minMAHFDist;
			if (_MAHF2THRDist > _maxMAHFDist)
				_MAHF2THRDist = _maxMAHFDist;

			if (fTmp != _MAHF2THRDist)
				TextBox0309.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_MAHF2THRDist).ToString();

			TextBox0309.Tag = TextBox0309.Text;

			CreateMAHF(_MAHF2THRDist);
		}

		#endregion

	}
}
