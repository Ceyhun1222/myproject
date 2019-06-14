using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
using Aran.Geometries;
using Aran.PANDA.RNAV.AdvancedPBN.Properties;
using Aran.Geometries.Operators;
using Aran.Aim.Enums;

namespace Aran.PANDA.RNAV.SGBAS
{
	public partial class SBASForm : Form
	{
		#region Variable declarations

		private Label[] Label1;
		private InfoForm infoForm;
		private SBASReportForm reportForm;

		private bool FormInitialised;
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
					//Label1[i].Visible = MultiPage1.TabPages[i].Visible
					Label1[i].ForeColor = System.Drawing.Color.Silver;
					Label1[i].Font = new System.Drawing.Font(Label1[i].Font, System.Drawing.FontStyle.Regular);
				}

				Label1[value].ForeColor = System.Drawing.Color.FromArgb(0XFF8000);
				Label1[value].Font = new System.Drawing.Font(Label1[value].Font, System.Drawing.FontStyle.Bold);

				//this.Text = Resources.str00033 + "   " + MultiPage1.TabPages[StIndex].Text;

				PrevBtn.Enabled = value > 0;
				NextBtn.Enabled = value < MultiPage1.TabPages.Count - 2;

				if (value == MultiPage1.TabPages.Count - 2)
					OkBtn.Enabled = true;
				else
					OkBtn.Enabled = false;

				//this.HelpContextID = 4000 + 100 * (value + 1);
				MultiPage1.SelectedIndex = value;
			}
		}

		#region Page I

		private double ArDir;
		private double RWYDir;

		private double SBASDir;
		private double SBASCourse;
		private double GPAngle;
		private double TanGPA;
		private double m_fMOC;
		private double GP_RDH;

		private double AlignP_THRMin;
		private double AlignP_THRMax;

		private double _zSurfaceOrigin;
		private double fMissAprPDG;


		private double GARP_THR;
		private double AlignP_THR;
		private double FPAP_THR;

		private double fRDHOCH;
		//private double OCHMin;
		private double DistLLZ2THR;
		private double deltaRangeOffset;

		eSBASCat _flightCategory;
		double Ss;
		double Vs;
		bool bHaveSolution;

		private RWYType SelectedRWY;
		private ILSType ILS;
		private int PrevCmbRWY;
		private int Category;

		private int FPAPElement;
		private int GARPElement;
		private int InterElement;

		private Aran.Geometries.Point FicTHRgeo;
		private Aran.Geometries.Point FicTHRprj;

		private Aran.Geometries.Point FPAPprj;
		private Aran.Geometries.Point GARPprj;

		private Aran.Geometries.Point ptInterGeo;
		private Aran.Geometries.Point ptInterPrj;

		//private Polygon pCircle;

		private NavaidType[] InSectList;

		//private ObstacleContainer ObstacleList;
		private ObstacleContainer OFZObstacleList;
		private ObstacleContainer OASObstacleList;
		private ObstacleContainer WorkObstacleList;

		#endregion

		#region Page II
		//FAP
		int FAPElem;
		private Aran.Geometries.Point _ptFAPprj;
		private Aran.Geometries.Point _ptFAPgeo;

		private double fMinFAPDist;
		private double fMaxFAPDist;
		private double fFAPDist;

		private double _hFAP;
		//private double _MinFAPOCH;	//????????????????????
		private double _CurrFAPOCH;		//fCurrOCH

		//IF
		private int IFElem;
		private int IFTolerAreaElem;

		private int IntermPrimaryAreaElem;
		private int IntermSecondaryAreaElem;

		private Aran.Geometries.Point IFprj;
		private Aran.Geometries.MultiPolygon IFTolerArea;

		private Aran.Geometries.MultiPolygon IntermPrimaryArea;
		private Aran.Geometries.MultiPolygon IntermSecondaryArea;

		private double _InterDescGrad;
		private double _PlannedTurnAtIF;

		private double _IFIAS;
		private double _IFTAS;
		private double _IFTurnR;

		private double _ImRange_Min;
		//_ImRange_Max = GlobalVars.constants.Pansops[ePANSOPSData.arImRange_Max].Value
		private double _IF2FAPdistance;

		private double _hIF_Min;
		private double _hIF_Max;
		private double _hIF;

		private int IxOCH;
		private bool HaveExcluded;

		ExcludeObstForm excludeObstFrm;

		private ObstacleContainer PrecisionOASObstacles;
		private ObstacleContainer IntermetiatObstacles;

		#endregion

		#region Page III

		private int ZContinuedElem;
		private int SOCElem;
		private int pMAPtElem;
		//private int MAHFElem;
		//private int MAHFTolerAreaElem;
		//private MAPt maptFix;
		private MATF mahfFix;

		private LineString LeftLine;
		private LineString RightLine;
		private MultiPolygon ZContinued;
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

		#region Page V

		private CodeSegmentPath[] FlyOverPathAndTerminations =
				new CodeSegmentPath[] { CodeSegmentPath.TF, CodeSegmentPath.DF };
		private CodeSegmentPath[] FlyByPathAndTerminations =
				new CodeSegmentPath[] { CodeSegmentPath.TF };

		private ePBNClass[] firstTurnPBNClass = new ePBNClass[] { ePBNClass.RNP_APCH};
		private ePBNClass[] regularTurnPBNClass = new ePBNClass[] { ePBNClass.RNAV1, ePBNClass.RNAV2, ePBNClass.RNP1, ePBNClass.RNP4, ePBNClass.RNAV5 };

		List<WayPoint> _SignificantPoints;
		private Interval UpDown401Range;
		private Transitions transitions;
		private WayPoint _AddedFIX;

		#endregion

		#endregion

		#region MainForm

		public SBASForm()
		{
			FormInitialised = false;

			InitializeComponent();

			Label1 = new Label[] { Label01, Label02, Label03, Label04, Label05, Label06};
			infoForm = new InfoForm();

			reportForm = new SBASReportForm();
			reportForm.Init(ReportButton);
			excludeObstFrm = new ExcludeObstForm();
			//reportForm.SetUnVisible(-1);

			FormInitialised = true;

			//pCircle = new Polygon();

			FPAPElement = -1;
			GARPElement = -1;
			InterElement = -1;
			bUnloadByOk = false;

			GlobalVars.SBASOASPlanesState = GlobalVars.OFZPlanesState = true;

			int i;
			for (i = 0; i < Label1.Length; i++)
				Label1[i].Text = MultiPage1.TabPages[i].Text;

			PrevCmbRWY = -1;

			//PrevCmb002 = -1;
			//ReDim TextBox0507Intervals(-1);
			//===========================================================
			//ComboBox0103.Items.Add(My.Resources.str223 + ":")
			//ComboBox0103.Items.Add(My.Resources.str224 + ":")

			//ComboBox0104.Items.Add(My.Resources.str10113)
			//ComboBox0104.Items.Add(My.Resources.str10118)

			//ComboBox0202.Items.Add(My.Resources.str223 + ":")
			//ComboBox0202.Items.Add(My.Resources.str224 + ":")

			//ComboBox0301.Items.Add(My.Resources.str223 + ":")
			//ComboBox0301.Items.Add(My.Resources.str224 + ":")

			//ComboBox0401.Items.Add(My.Resources.str226)
			//ComboBox0401.Items.Add(My.Resources.str225)

			//ComboBox0502.Items.Add(My.Resources.str10113)
			//ComboBox0502.Items.Add(My.Resources.str10118)

			//ComboBox0503.Items.Add(My.Resources.str16008)	//???????
			//ComboBox0503.Items.Add(My.Resources.str16010)

			//ComboBox0903.Items.Add(My.Resources.str10113)
			//ComboBox0903.Items.Add(My.Resources.str10118)

			//FIXElem = Nothing
			//===========================================================

			//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			//===========================================================
			//Text = My.Resources.str2
			//MultiPage1.TabPages.Item(0).Text = My.Resources.str201
			//MultiPage1.TabPages.Item(1).Text = My.Resources.str202
			//MultiPage1.TabPages.Item(2).Text = My.Resources.str203
			//MultiPage1.TabPages.Item(3).Text = My.Resources.str109
			//MultiPage1.TabPages.Item(4).Text = My.Resources.str110
			//MultiPage1.TabPages.Item(5).Text = My.Resources.str111
			//MultiPage1.TabPages.Item(6).Text = My.Resources.str113
			//MultiPage1.TabPages.Item(7).Text = My.Resources.str114
			//MultiPage1.TabPages.Item(8).Text = My.Resources.str115
			//MultiPage1.TabPages.Item(9).Text = My.Resources.str116
			//=======================================================
			//Label0001_0.Text = My.Resources.str20101
			//_Label0001_1.Text = My.Resources.str20102
			//_Label0001_2.Text = My.Resources.str20104
			//_Label0001_3.Text = My.Resources.str20103
			//_Label0001_4.Text = My.Resources.str20105
			//_Label0001_5.Text = My.Resources.str20106
			//_Label0001_6.Text = My.Resources.str20107
			//_Label0001_7.Text = My.Resources.str20115
			//_Label0001_12.Text = My.Resources.str21
			//_Label0001_13.Text = My.Resources.str21
			//_Label0001_14.Text = My.Resources.str20109
			//_Label0001_15.Text = My.Resources.str20116
			//_Label0001_16.Text = My.Resources.str20110
			//_Label0001_17.Text = My.Resources.str20111
			//_Label0001_18.Text = My.Resources.str20112
			//_Label0001_19.Text = My.Resources.str20113
			//_Label0001_20.Text = My.Resources.str20114
			//_Label0001_21.Text = My.Resources.str20108
			//_Label0001_25.Text = My.Resources.str21
			//Label0001_27.Text = My.Resources.str10107

			//'===============================================
			//_Label0101_0.Text = My.Resources.str20201

			//_Label0101_5.Text = My.Resources.str10405
			//CheckBox0101.Text = My.Resources.str20206
			//_Label0101_2.Text = My.Resources.str20207

			//Frame0103.Text = My.Resources.str20203
			//_Label0101_3.Text = My.Resources.str20210
			//_Label0101_6.Text = My.Resources.str20211
			//'==================================================
			//Frame0201.Text = My.Resources.str10509
			//_Label0201_0.Text = My.Resources.str10203

			//OptionButton0201.Text = My.Resources.str230
			//OptionButton0202.Text = My.Resources.str231
			//Frame0202.Text = My.Resources.str20306

			//Frame0203.Text = My.Resources.str20304

			//_Label0201_4.Text = My.Resources.str20305
			//_Label0201_5.Text = My.Resources.str10503
			//_Label0201_6.Text = My.Resources.str20307
			//_Label0201_7.Text = My.Resources.str20308


			//_Label0201_14.Text = My.Resources.str20316
			//_Label0201_16.Text = My.Resources.str10516	'    IF designator:

			//'=================================================
			//_Label0301_1.Text = My.Resources.str20402
			//_Label0301_2.Text = My.Resources.str20403
			//CheckBox0301.Text = My.Resources.str20404
			//_Label0301_4.Text = My.Resources.str10911
			//_Label0301_5.Text = My.Resources.str11057
			//_Label0301_6.Text = My.Resources.str11057
			//_Label0301_7.Text = My.Resources.str20407
			//_Label0301_3.Text = My.Resources.str20408

			//_Label0301_13.Text = My.Resources.str16005
			//'=================================================
			//_Label0401_0.Text = My.Resources.str20501
			//_Label0401_2.Text = My.Resources.str10101
			//_Label0401_4.Text = My.Resources.str20502
			//_Label0401_5.Text = My.Resources.str20102
			//_Label0401_8.Text = My.Resources.str10920

			//CheckBox0401.Text = My.Resources.str20503
			//CheckBox0402.Text = My.Resources.str20507

			//Frame0401.Text = My.Resources.str11005	'20504
			//OptionButton0401.Text = My.Resources.str11006
			//OptionButton0402.Text = My.Resources.str11007
			//'=================================================
			//_Label0501_0.Text = My.Resources.str20606
			//_Label0501_1.Text = My.Resources.str20607
			//'_Label0501_2.Text = My.Resources.str20601
			//_Label0501_3.Text = My.Resources.str20608
			//_Label0501_4.Text = My.Resources.str20609
			//_Label0501_5.Text = My.Resources.str20610
			//_Label0501_6.Text = My.Resources.str20611
			//'_Label0501_7.Text = "THR-TP Dist:"

			//_Label0501_8.Text = My.Resources.str20602
			//_Label0501_9.Text = My.Resources.str10203
			//_Label0501_18.Text = ""

			//Frame0501.Text = My.Resources.str12007
			//OptionButton0501.Text = My.Resources.str230
			//OptionButton0502.Text = My.Resources.str231
			//'=====================================================
			//OptionButton0601.Text = My.Resources.str13001
			//OptionButton0602.Text = My.Resources.str13002
			//OptionButton0603.Text = My.Resources.str13003
			//CheckBox0601.Text = My.Resources.str13007

			//_Label0601_0.Text = My.Resources.str13004
			//_Label0601_1.Text = My.Resources.str13011
			//_Label0601_2.Text = My.Resources.str13010
			//_Label0601_5.Text = My.Resources.str13005
			//_Label0601_6.Text = My.Resources.str13006
			//_Label0601_11.Text = My.Resources.str13012
			//_Label0601_12.Text = My.Resources.str15
			//_Label0601_13.Text = My.Resources.str13016

			//ComboBox0603.Items.Clear()
			//ComboBox0603.Items.Add(My.Resources.str13014)
			//ComboBox0603.Items.Add(My.Resources.str13015)
			//'======================================================
			//Frame0701.Text = My.Resources.str14001
			//Frame0702.Text = My.Resources.str14006
			//_Label0701_0.Text = My.Resources.str20902
			//OptionButton0701.Text = My.Resources.str20903
			//OptionButton0702.Text = My.Resources.str20904
			//OptionButton0703.Text = My.Resources.str20905
			//OptionButton0704.Text = My.Resources.str20903
			//OptionButton0705.Text = My.Resources.str20904
			//OptionButton0706.Text = My.Resources.str20905
			//_Label0701_1.Text = My.Resources.str20907
			//'=======================================================
			//Frame0801.Text = My.Resources.str21001
			//Frame0802.Text = My.Resources.str21005
			//CheckBox0801.Text = My.Resources.str21002
			//CheckBox0802.Text = My.Resources.str21003
			//CheckBox0803.Text = My.Resources.str21004
			//CheckBox0804.Text = My.Resources.str21002
			//CheckBox0805.Text = My.Resources.str21003
			//CheckBox0806.Text = My.Resources.str21006
			//'========================================================
			//_Label0901_0.Text = My.Resources.str16006		'	'
			//_Label0901_2.Text = My.Resources.str16009		'	'
			//_Label0901_6.Text = My.Resources.str16005
			//_Label0901_8.Text = My.Resources.str10720
			//_Label0901_11.Text = My.Resources.str10720
			//_Label0901_12.Text = My.Resources.str16007
			//_Label0901_14.Text = My.Resources.str12002
			//_Label0901_18.Text = My.Resources.str21103
			//_Label0901_23.Text = My.Resources.str13016 'LoadResString(13013)+":"

			//OptionButton0901.Text = My.Resources.str10512
			//OptionButton0902.Text = My.Resources.str10513
			//Frame0901.Text = My.Resources.str16003
			//====================================================================

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

			_Label0101_01.Text = GlobalVars.unitConverter.DistanceUnit;
			_Label0101_02.Text = GlobalVars.unitConverter.HeightUnit;
			_Label0101_03.Text = GlobalVars.unitConverter.HeightUnit;

			_Label0101_06.Text = GlobalVars.unitConverter.DistanceUnit;
			_Label0101_07.Text = GlobalVars.unitConverter.HeightUnit;
			_Label0101_11.Text = GlobalVars.unitConverter.SpeedUnit;
			_Label0101_13.Text = GlobalVars.unitConverter.DistanceUnit;

			//=====================================================================
			_Label0201_02.Text = GlobalVars.unitConverter.DistanceUnit;
			_Label0201_04.Text = GlobalVars.unitConverter.DistanceUnit;
			_Label0201_06.Text = GlobalVars.unitConverter.HeightUnit;
			_Label0201_09.Text = GlobalVars.unitConverter.HeightUnit;
			_Label0201_12.Text = GlobalVars.unitConverter.HeightUnit;
			_Label0201_14.Text = GlobalVars.unitConverter.HeightUnit;
			_Label0201_18.Text = GlobalVars.unitConverter.DistanceUnit;
			//=====================================================================

			_Label0301_04.Text = GlobalVars.unitConverter.SpeedUnit;
			_Label0301_07.Text = GlobalVars.unitConverter.HeightUnit;
			_Label0301_12.Text = GlobalVars.unitConverter.HeightUnit;
			_Label0301_14.Text = GlobalVars.unitConverter.HeightUnit;
			_Label0301_16.Text = GlobalVars.unitConverter.HeightUnit;
			_Label0301_20.Text = GlobalVars.unitConverter.DistanceUnit;
			//=====================================================================

			label403.Text = GlobalVars.unitConverter.DistanceUnit;
			label406.Text = GlobalVars.unitConverter.SpeedUnit;
			label408.Text = GlobalVars.unitConverter.SpeedUnit;
			label410.Text = GlobalVars.unitConverter.HeightUnit;

			//=====================================================================

			//_Label0901_1.Text = GlobalVars.unitConverter.DistanceUnit;
			_Label0901_3.Text = GlobalVars.unitConverter.HeightUnit;
			_Label0901_5.Text = GlobalVars.unitConverter.HeightUnit;
			_Label0901_7.Text = GlobalVars.unitConverter.HeightUnit;
			_Label0901_10.Text = GlobalVars.unitConverter.HeightUnit;
			_Label0901_13.Text = GlobalVars.unitConverter.HeightUnit;
			_Label0901_21.Text = GlobalVars.unitConverter.DistanceUnit;
			_Label0901_22.Text = GlobalVars.unitConverter.HeightUnit;

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

			for (i = 0; i < n; i++)
				ComboBox0001.Items.Add(GlobalVars.RWYList[i]);

			NextBtn.Enabled = n >= 0;

			TextBox0010_Validating(TextBox0010, null);

			if (n > 0)
				ComboBox0001.SelectedIndex = 0;
			else
			{
				System.Windows.Forms.MessageBox.Show("There is no RWY for the specified ADHP.", "PANDA",
					System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				Close();
				return;
			}

			//double maxRange = GlobalVars.ModellingRadius + Functions.CalcMaxRadius();
			DBModule.GetObstaclesByDist(out GlobalVars.ObstacleList, GlobalVars.CurrADHP.pPtPrj, GlobalVars.ModellingRadius);

			ComboBox0002.SelectedIndex = 1;
			ComboBox0003.SelectedIndex = 0;

			ComboBox0201.Items.Clear();

			for (i = 0; i < GlobalVars.EnrouteMOCValues.Length; i++)
			{
				ComboBox0201.Items.Add((GlobalVars.unitConverter.HeightToDisplayUnits(GlobalVars.EnrouteMOCValues[i], eRoundMode.SPECIAL_CEIL)));
				comboBox408.Items.Add((GlobalVars.unitConverter.HeightToDisplayUnits(GlobalVars.EnrouteMOCValues[i], eRoundMode.SPECIAL_CEIL)));
			}

			ComboBox0201.SelectedIndex = 0;
			ComboBox0301.SelectedIndex = 0;
			ComboBox0303.SelectedIndex = 1;
			comboBox408.SelectedIndex = 0;
		}

		private void SBASForm_KeyUp(object sender, KeyEventArgs e)
		{
			//if (e.KeyCode == Keys.F1)
			//{
			//	NativeMethods.HtmlHelp(0, GlobalVars.HelpFile, GlobalVars.HH_HELP_CONTEXT, HelpContextID);
			//	e.Handled = true;
			//}
		}

		private void SBASForm_FormClosed(object sender, FormClosedEventArgs e)
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
				this.Width = 758;
				ShowPanelBtn.Image = Resources.bmpHIDE_INFO;
			}
			else
			{
				this.Width = 578;
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
					//preparePageI();
					break;
				case 2:
					break;
				case 3:
					break;
				case 4:
					leavePageV();

					break;
				case 5:
					break;
			}

			this.CurrPage = MultiPage1.SelectedIndex - 1;

			NativeMethods.HidePandaBox();
		}

		private void NextBtn_Click(object sender, EventArgs e)
		{
			NativeMethods.ShowPandaBox(this.Handle.ToInt32());

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
					preparePageIII();
					break;

				case 2:
					preparePageIV();
					break;

				case 3:
					preparePageV();
					break;
			}

			this.CurrPage = MultiPage1.SelectedIndex + 1;

			NativeMethods.HidePandaBox();
		}

		private void OkBtn_Click(object sender, EventArgs e)
		{
			Close();
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

		private void ClearGraphics()
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(FPAPElement);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(GARPElement);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(InterElement);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(FAPElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(IFElem);

			GlobalVars.gAranGraphics.SafeDeleteGraphic(IntermSecondaryAreaElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(IntermPrimaryAreaElem);

			GlobalVars.gAranGraphics.SafeDeleteGraphic(ZContinuedElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(SOCElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(pMAPtElem);
			if (mahfFix != null)
				mahfFix.DeleteGraphics();

			int n = GlobalVars.OFZPlanesElement.Length;
			for (int i = 0; i < n; i++)
				GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.OFZPlanesElement[i]);

			n = GlobalVars.SBASOASPlanesElement.Length;
			for (int i = 0; i < n; i++)
				GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.SBASOASPlanesElement[i]);
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
			GlobalVars.gAranGraphics.SafeDeleteGraphic(FAPElem);

			GlobalVars.gAranGraphics.SafeDeleteGraphic(IFElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(IntermSecondaryAreaElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(IntermPrimaryAreaElem);

			FPAPElement = GARPElement = InterElement = -1;

			if (cbIgnoreILS.Checked)
			{
				BaseDist = AlignP_THR;

				ptInterPrj = ARANFunctions.LocalToPrj(SelectedRWY.pPtPrj[eRWY.ptTHR], RWYDir + Math.PI, BaseDist);

				FicTHRprj = ARANFunctions.LocalToPrj(ptInterPrj, SBASDir, BaseDist);
				//ARANFunctions.PrjToLocal(pPtAlign, SBASDir, SelectedRWY.pPtPrj[eRWY.PtTHR], out BaseDist, out y);
				//FicTHRprj = ARANFunctions.LocalToPrj(SelectedRWY.pPtPrj[eRWY.PtStart], SBASDir, BaseDist);

				FPAPprj = ARANFunctions.LocalToPrj(FicTHRprj, SBASDir, FPAP_THR);
				GARPprj = ARANFunctions.LocalToPrj(FPAPprj, SBASDir, 305.0);

				GARP_THR = FPAP_THR + 305.0;

				TextBox0013.ReadOnly = false;
				TextBox0010.ReadOnly = false;

				fRDHOCH = GP_RDH + BaseDist * TanGPA + GlobalVars.LocOffsetOCHAdd;

				//InterElement = GlobalVars.gAranGraphics.DrawPointWithText(ptInterPrj, ARANFunctions.RGB(0, 250, 255), "Intersection");
			}
			else
			{
				SBASDir = ILS.pPtPrj.M;
				SBASCourse = ILS.pPtGeo.M;

				GPAngle = ILS.GPAngle;
				TanGPA = System.Math.Tan(GPAngle);

				//GARP_THR = ILS.LLZ_THR;
				double MaxCLShift = 65.0;	//1.0;

				TextBox0013.ReadOnly = true;
				TextBox0010.ReadOnly = true;

				TextBox0013.Text = SBASCourse.ToString("0.00");
				TextBox0010.Text = System.Math.Round(ARANMath.RadToDegValue * GPAngle, 2).ToString("0.00");

				double fTmp = SBASDir - RWYDir;
				if (fTmp < 0.0) fTmp = fTmp + 2.0 * Math.PI;
				if (fTmp > Math.PI) fTmp = 2.0 * Math.PI - fTmp;
				infoForm.SetDeltaAngle(fTmp);

				Geometry geom;
				if (fTmp < ARANMath.DegToRadValue)
					geom = new Point();
				else
					geom = ARANFunctions.LineLineIntersect(SelectedRWY.pPtPrj[eRWY.ptTHR], RWYDir, ILS.pPtPrj, SBASDir);

				double x = 0.0, y;

				if (geom.IsEmpty ||  geom == null || geom.Type != GeometryType.Point)
				{
					ARANFunctions.PrjToLocal(ILS.pPtPrj, SBASDir, SelectedRWY.pPtPrj[eRWY.ptTHR], out x, out y);
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
					FicTHRprj = ARANFunctions.LocalToPrj(ILS.pPtPrj, SBASDir, x);
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

					FicTHRprj = ARANFunctions.LocalToPrj(ptInterPrj, SBASDir, BaseDist);
					fRDHOCH = GP_RDH + BaseDist * TanGPA + GlobalVars.LocOffsetOCHAdd;

					infoForm.SetIntersectDistance(BaseDist);
					InterElement = GlobalVars.gAranGraphics.DrawPointWithText(ptInterPrj, "Intersection", ARANFunctions.RGB(0, 250, 255));
				}

				double Loc_THR = ARANFunctions.ReturnDistanceInMeters(ILS.pPtPrj, FicTHRprj);

				if (Loc_THR >= FPAP_THR + 305.0)
				{
					GARPprj = ILS.pPtPrj;
					FPAPprj = ARANFunctions.LocalToPrj(GARPprj, SBASDir + Math.PI, 305.0);
					deltaRangeOffset = Loc_THR - FPAP_THR - 305.0;
				}
				else
				{
					FPAPprj = ARANFunctions.LocalToPrj(FicTHRprj, SBASDir, FPAP_THR);
					GARPprj = ARANFunctions.LocalToPrj(FPAPprj, SBASDir, 305.0);
				}

				GARP_THR = ARANFunctions.ReturnDistanceInMeters(GARPprj, FicTHRprj);
				//?????????????????????????????????????
				ARANFunctions.PrjToLocal(SelectedRWY.pPtPrj[eRWY.ptTHR], SBASDir, GARPprj, out x, out y);
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
			FicTHRprj.M = SBASDir;											//Azt2Dir(ptLH, ptLH.M)
			ArDir = SBASDir;

			ComboBox0003_SelectedIndexChanged(ComboBox0003, null);
			ComboBox0002_SelectedIndexChanged(ComboBox0002, null);

			FPAPElement = GlobalVars.gAranGraphics.DrawPointWithText(FPAPprj, "FPAP", ARANFunctions.RGB(0, 250, 255));
			GARPElement = GlobalVars.gAranGraphics.DrawPointWithText(GARPprj, "GARP", ARANFunctions.RGB(0, 250, 255));

			FicTHRgeo = GlobalVars.pspatialReferenceOperation.ToGeo<Point>(FicTHRprj);
			FicTHRgeo.M = SBASCourse;

			ptInterGeo = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(ptInterPrj);

			//ILS.SecWidth = ILS.LLZ_THR * Tan(DegToRad(ILS.AngleWidth))

			DistLLZ2THR = GARP_THR;
			TextBox0003.Text = Math.Round(DistLLZ2THR).ToString();
			TextBox0012.Text = Math.Round(deltaRangeOffset).ToString();

			double FPAP2THR = ARANFunctions.ReturnDistanceInMeters(FicTHRprj, FPAPprj);
			TextBox0009.Text = Math.Round(FPAP2THR).ToString();

			infoForm.SetOCHLimit(fRDHOCH);

			//CreateILS23Planes(ptLHPrj, ArDir, 45.0, ILS23Planes)
			//N = UBound(ILS23Planes)
			//For I = 0 To N
			//	If Not ILS23PlanesElement(I) Is Nothing Then pGraphics.DeleteElement(ILS23PlanesElement(I))
			//	ILS23PlanesElement(I) = DrawPolygon(ILS23Planes(I).Poly, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSHollow, ILS23PlaneState)
			//	ILS23PlanesElement(I).Locked = True
			//Next I
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

			SelectedRWY = (RWYType)ComboBox0001.SelectedItem;	//GlobalVars.RWYList[RWYIndex];

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
				SBASDir = SelectedRWY.pPtPrj[eRWY.ptTHR].M;
				SBASCourse = SelectedRWY.pPtGeo[eRWY.ptTHR].M;
				TextBox0013.Text = SBASCourse.ToString("0.00");

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
			//pCircle.ExteriorRing = ARANFunctions.CreateCirclePrj(ptLHPrj, GlobalVars.RModel);
			////DBModule.GetObstaclesByDist(out ObstacleList, GlobalVars.CurrADHP.pPtPrj, GlobalVars.RModel, ptLHPrj.Z);
			//ComboBox0002.SelectedIndex = 0;
			////ComboBox0103.SelectedIndex = 0;
			////ComboBox0202.SelectedIndex = 0;
			////ComboBox0301.SelectedIndex = 0;
		}

		private void ComboBox0002_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ComboBox0002.SelectedIndex < 0)
				return;

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

			switch (ComboBox0002.SelectedIndex)
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

					reportForm.FillPage01(OFZObstacleList);
					ComboBox0004.SelectedIndex = 0;
					break;
			}

			//PrevCmb002 = ComboBox0002.SelectedIndex
			//TextBox0404.Text = ComboBox0002.Text;
		}

		private void ComboBox0003_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!FormInitialised)
				return;

			double fMOCCorrH, fMOCCorrGP, fMargin;
			int k = ComboBox0003.SelectedIndex;
			if (k < 0) return;

			Category = k;

			//TextBox0403.Text = ComboBox0003.Text;

			Ss = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arSemiSpan].Value[k];
			Vs = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arVerticalSize].Value[k];

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

			SBASCourse = fTmp;

			while (SBASCourse < 0.0)
				SBASCourse += 360.0;

			while (SBASCourse >= 360.0)
				SBASCourse = SBASCourse - 360.0;

			double DirInDeg0 = SelectedRWY.pPtGeo[eRWY.ptTHR].M - 5.0;
			double DirInDeg1 = SBASCourse;
			double rAngle = ARANMath.SubtractAnglesInDegs(DirInDeg0, DirInDeg1);

			TurnDirection res;

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
				SBASCourse = SelectedRWY.pPtGeo[eRWY.ptTHR].M - 5.0;
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
					SBASCourse = SelectedRWY.pPtGeo[eRWY.ptTHR].M + 5.0;
			}

			if (fTmp != SBASCourse)
				TextBox0013.Text = SBASCourse.ToString("0.00");

			//pPtBase = LineLineIntersect(SelectedRWY.pPtPrj(eRWY.PtTHR), RWYDir, ILS.pPtPrj, ILSDir);
			if (ptInterGeo.IsEmpty)
				SBASDir = ARANFunctions.AztToDirection(SelectedRWY.pPtGeo[eRWY.ptTHR], SBASCourse, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);
			else
				SBASDir = ARANFunctions.AztToDirection(ptInterGeo, SBASCourse, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);

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
				SBASDir = SelectedRWY.pPtPrj[eRWY.ptTHR].M;
				SBASCourse = SelectedRWY.pPtGeo[eRWY.ptTHR].M;
				TextBox0013.Text = SBASCourse.ToString("0.00");

				TextBox0010.Text = (3.0).ToString();
				GPAngle = 3.0 * ARANMath.DegToRadValue;

				double x, y;
				ARANFunctions.PrjToLocal(SelectedRWY.pPtPrj[eRWY.ptStart], RWYDir, SelectedRWY.pPtPrj[eRWY.ptTHR], out x, out y);
				FPAPprj = ARANFunctions.LocalToPrj(SelectedRWY.pPtPrj[eRWY.ptStart], RWYDir, SelectedRWY.ASDA, y);
				ARANFunctions.PrjToLocal(SelectedRWY.pPtPrj[eRWY.ptTHR], RWYDir, FPAPprj, out FPAP_THR, out y);

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
			HaveExcluded = false;

			_InterDescGrad = TanGPA;
			TextBox0107.Text = (100.0 * _InterDescGrad).ToString("0.0");

			_flightCategory = (eSBASCat)ComboBox0002.SelectedIndex;

			if (_flightCategory == eSBASCat.CategoryI)
				_zSurfaceOrigin = GlobalVars.Cat1OASZOrigin;
			else if (_flightCategory == eSBASCat.APVI)
				_zSurfaceOrigin = GlobalVars.Cat1OASZOrigin + 38 / TanGPA;
			else
				_zSurfaceOrigin = GlobalVars.Cat1OASZOrigin + 8 / TanGPA;

			if (GPAngle > GlobalVars.MaxRefGPAngle)
				_zSurfaceOrigin += 500.0 * (GPAngle - GlobalVars.MaxRefGPAngle);

			fMissAprPDG = 0.01 * (double)MAGUpDwn.Value;
			Common.CalcOASPlanes(_flightCategory, DistLLZ2THR, GPAngle, fMissAprPDG, GP_RDH, Ss, Vs, ref GlobalVars.SBASOASPlanes);

			double hMax = GlobalVars.constants.Pansops[ePANSOPSData.arFAPMaxRange].Value * TanGPA + GP_RDH - GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value;
			Common.CreateOASPlanes(FicTHRprj, ArDir, hMax, ref GlobalVars.SBASOASPlanes);

			if (GlobalVars.VisibilityBar == null || GlobalVars.VisibilityBar.IsDisposed)
			{
				GlobalVars.VisibilityBar = new ToolbarForm();
				GlobalVars.VisibilityBar.Show(GlobalVars.Win32Window);
			}

			GlobalVars.VisibilityBar.SetEnabled(GlobalVars.VisibilityBar.SBASOAS, true);

            int n = GlobalVars.SBASOASPlanes.Length;

            for (int i = 0; i < n; i++)
            {
                GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.SBASOASPlanesElement[i]);
                GlobalVars.SBASOASPlanesElement[i] = GlobalVars.gAranGraphics.DrawMultiPolygon(GlobalVars.SBASOASPlanes[i].Poly, AranEnvironment.Symbols.eFillStyle.sfsHollow, -1, GlobalVars.SBASOASPlanesState);
            }

            //if (!double.TryParse(TextBox0005.Text, out Ss))
			//	Ss = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arSemiSpan].Value[Category];

			//if (!double.TryParse(TextBox0006.Text, out Vs))
			//	Vs = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arVerticalSize].Value[Category];

			//CheckBox0101.Checked = false;
			Functions.AnaliseObstacles(GlobalVars.ObstacleList, out OASObstacleList, FicTHRprj, ArDir, GlobalVars.SBASOASPlanes);
			Functions.CalcEffectiveHeights(ref OASObstacleList, GPAngle, fMissAprPDG, GP_RDH, _zSurfaceOrigin, GlobalVars.SBASOASPlanes);		// arMAS_Climb_Nom.Value

			int m = OASObstacleList.Obstacles.Length;
			WorkObstacleList.Obstacles = new Obstacle[m];

			n = OASObstacleList.Parts.Length;
			WorkObstacleList.Parts = new ObstacleData[n];

			Array.Copy(OASObstacleList.Obstacles, WorkObstacleList.Obstacles, m);
			Array.Copy(OASObstacleList.Parts, WorkObstacleList.Parts, n);

			Functions.Sort(ref WorkObstacleList, 2);

			WPTInSector(FicTHRprj, GlobalVars.WPTList, out InSectList);
			//bNavFlg = false;

			fFAPDist = GlobalVars.constants.Pansops[ePANSOPSData.arOptimalFAFRang].Value;
			TextBox0101.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(fFAPDist, eRoundMode.NEAREST).ToString();

			double fTmp = FAPDist2hFAP(fFAPDist);
			TextBox0102.Tag = fTmp.ToString();
			ComboBox0101_SelectedIndexChanged(ComboBox0101, null);

			//if (ComboBox0103.SelectedIndex == 0)
			//    TextBox0102.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fTmp + FicTHRprj.Z, eRoundMode.NERAEST).ToString();
			//else
			//    TextBox0102.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fTmp, eRoundMode.NERAEST).ToString();

			//if( ComboBox0102.SelectedIndex == 0 )
			//    ComboBox0102_SelectedIndexChanged(ComboBox0102, null);
			//else
			//    ComboBox0102.SelectedIndex = 0;

			//if( ! bHaveSolution )
			//    return false;

			fMinFAPDist = Functions.StartPointDist(GlobalVars.SBASOASPlanes[(int)eOAS.XlPlane], GlobalVars.SBASOASPlanes[(int)eOAS.YlPlane], GlobalVars.arHOASPlaneCat1, 0.0);
			fMaxFAPDist = GlobalVars.constants.Pansops[ePANSOPSData.arMaxRangeFAS].Value;

			n = InSectList.Length;
			ComboBox0103.Items.Clear();

			for (int i = 0; i < n; i++)
			{
				if (InSectList[i].TypeCode != eNavaidType.NONE)
					continue;

				double X, Y;
				ARANFunctions.PrjToLocal(FicTHRprj, ArDir + Math.PI, InSectList[i].pPtPrj, out X, out Y);

				if (X < fMinFAPDist) continue;
				if (X > fMaxFAPDist) continue;

				ComboBox0103.Items.Add(InSectList[i]);
			}

			if (ComboBox0103.Items.Count > 0)
			{
				CheckBox0102.Enabled = true;
				ComboBox0103.SelectedIndex = 0;
			}
			else
			{
				CheckBox0102.Enabled = false;
				CheckBox0102.Checked = false;
			}

			//IF ==============================================================================

			_PlannedTurnAtIF = ARANMath.C_PI_2;
			TextBox0110.Text = ARANMath.RadToDeg(_PlannedTurnAtIF).ToString("0");
			TextBox0109.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(GlobalVars.constants.AircraftCategory[aircraftCategoryData.ViafMax].Value[Category], eRoundMode.NEAREST).ToString();

			_IFIAS = GlobalVars.constants.AircraftCategory[aircraftCategoryData.ViafMax].Value[Category];
			_hFAP = FAPDist2hFAP(fFAPDist);

			Caller(out _CurrFAPOCH, ref _hFAP);

			return true;
		}

		private double CalcFAPOCH(ref double fhFAP, double f_MOC, out int Ix)
		{
			int n = WorkObstacleList.Parts.Length;
			double result = f_MOC > fRDHOCH ? f_MOC : fRDHOCH;

			Ix = -1;

			for (int i = n - 1; i >= 0; i--)
			{
				if (WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].IgnoredByUser)
					continue;

				if (WorkObstacleList.Parts[i].hPenet > 0.0 && WorkObstacleList.Parts[i].ReqH <= fhFAP)	//&& WorkObstacleList[i].Dist >= -ZSurfaceOrigin)
				{
					WorkObstacleList.Parts[i].ReqOCH =
						f_MOC + Math.Min(WorkObstacleList.Parts[i].Height, WorkObstacleList.Parts[i].EffectiveHeight);

					if (WorkObstacleList.Parts[i].ReqOCH > result)
					{
						result = WorkObstacleList.Parts[i].ReqOCH;
						Ix = i;
					}

					if (WorkObstacleList.Parts[i].ReqOCH > fhFAP)
						fhFAP = WorkObstacleList.Parts[i].ReqOCH;
				}
				else
					WorkObstacleList.Parts[i].ReqOCH = 0.0;
			}

			return result;
		}

		private void Caller(out double OCH, ref double hFAP)
		{
			NativeMethods.ShowPandaBox(this.Handle.ToInt32());
			//bNavFlg = true;

			OCH = CalcFAPOCH(ref hFAP, m_fMOC, out IxOCH);

			fFAPDist = hFAP2FAPDist(hFAP);

			_ptFAPprj = ARANFunctions.LocalToPrj(FicTHRprj, ArDir + Math.PI, fFAPDist);
			_ptFAPprj.Z = hFAP;
			_ptFAPprj.M = ArDir;

			_ptFAPgeo = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(_ptFAPprj);

			GlobalVars.gAranGraphics.SafeDeleteGraphic(FAPElem);

			FAPElem = GlobalVars.gAranGraphics.DrawPointWithText(_ptFAPprj, "FAP", GlobalVars.WPTColor);

			TextBox0101.Tag = GlobalVars.unitConverter.DistanceToDisplayUnits(fFAPDist, eRoundMode.NEAREST).ToString();
			TextBox0101.Text = (string)TextBox0101.Tag;

			TextBox0102.Tag = _hFAP.ToString();

			ComboBox0101_SelectedIndexChanged(ComboBox0101, null);
			ComboBox0102_SelectedIndexChanged(ComboBox0102, null);

			if (IxOCH > -1)
			{
				TextBox0104.Text = WorkObstacleList.Obstacles[WorkObstacleList.Parts[IxOCH].Owner].UnicalName;
				//ToolTip1.SetToolTip(_Label0101_5, WorkObstacleList.Obstacles(WorkObstacleList.Parts(IxOCH).Owner).UnicalName)
			}
			else
			{
				TextBox0104.Text = "";
				//ToolTip1.SetToolTip(_Label0101_5, "")
			}

			//======================================================================
			//Common.CalcOASPlanes(_flightCategory, DistLLZ2THR, GPAngle, fMissAprPDG, GP_RDH, Ss, Vs, ref GlobalVars.OASWorkPlanes);
			//double hMax = GlobalVars.constants.Pansops[ePANSOPSData.arFAPMaxRange].Value * TanGPA + ILS.GP_RDH - GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value;

			Common.CreateOASPlanes(FicTHRprj, ArDir, hFAP, ref GlobalVars.SBASOASPlanes);

			//int n = GlobalVars.SBASOASPlanes.Length;
			//int i;

			//for (i = 0; i < n; i++)
			//{
			//	//GlobalVars.OASWorkPlanes[i] = GlobalVars.SBASOASPlanes[i];
			//	GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.SBASOASPlanesElement[i]);
			//	GlobalVars.SBASOASPlanesElement[i] = GlobalVars.gAranGraphics.DrawMultiPolygon(GlobalVars.SBASOASPlanes[i].Poly, -1, AranEnvironment.Symbols.eFillStyle.sfsHollow, GlobalVars.SBASOASPlanesState);
			//}

			//OAS Obstacles ========================================================
			int i;
			int m = OASObstacleList.Obstacles.Length;
			int n = OASObstacleList.Parts.Length;

			ObstacleContainer tmpObstacleList;

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
				OASObstacleList.Obstacles[i].NIx = -1;

			int k = 0, l = 0;

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

			reportForm.FillPage02(tmpObstacleList);

			//==================================================================================================
			ExcludeBtn.Enabled = (IxOCH >= 0) || HaveExcluded;
			CheckAndDrawIF(7);

			NativeMethods.HidePandaBox();
		}

		private void CheckAndDrawIF(int verify)
		{
			_hIF_Min = _hFAP;

			double IFminTAS = ARANMath.IASToTASForRnav(_IFIAS, _hIF_Min, GlobalVars.CurrADHP.ISAtC - GlobalVars.constants.Pansops[ePANSOPSData.arISAmax].Value);
			double IFminTurnR = ARANMath.BankToRadius(ARANMath.DegToRad(25), IFminTAS);
			double ImRange_Min = IFminTurnR * Math.Tan(0.5 * _PlannedTurnAtIF) + GlobalVars.SBASTransitionDistance;	///+  IF.ATT;
			double x, y;

			int previ = 0, i, n = InSectList.Length;

			bool ValidateComboBox0203 = Math.Abs(_ImRange_Min - ImRange_Min) > ARANMath.EpsilonDistance;// || (verify & 4) != 0;

			if (ValidateComboBox0203)
			{
				Guid prevPt = default(Guid);
				if (ComboBox0105.SelectedIndex >= 0)
					prevPt = ((NavaidType)ComboBox0105.SelectedItem).Identifier;

				ComboBox0105.Items.Clear();

				for (i = 0; i < n; i++)
				{
					ARANFunctions.PrjToLocal(_ptFAPprj, ArDir, InSectList[i].pPtPrj, out x, out y);

					if (x >= ImRange_Min && x <= GlobalVars.constants.Pansops[ePANSOPSData.arImRange_Max].Value)
					{
						if (prevPt == InSectList[i].Identifier)
							previ = ComboBox0105.Items.Count;
						ComboBox0105.Items.Add(InSectList[i]);
					}
				}
			}

			_ImRange_Min = ImRange_Min;
			double fTmp = _IF2FAPdistance;
			if (fTmp < _ImRange_Min)
				fTmp = _ImRange_Min;

			_hIF_Max = Functions.PosibleMaxIFaltitude(fTmp, _InterDescGrad, _PlannedTurnAtIF, _hFAP, _IFIAS);
			fTmp = _hIF;

			//if ((verify & 1) != 0)
			{
				if (_hIF < _hIF_Min)
					_hIF = _hIF_Min;
				else if (_hIF > _hIF_Max)
					_hIF = _hIF_Max;
			}

			_IFTAS = ARANMath.IASToTASForRnav(_IFIAS, _hIF, GlobalVars.CurrADHP.ISAtC - GlobalVars.constants.Pansops[ePANSOPSData.arISAmax].Value);
			_IFTurnR = ARANMath.BankToRadius(ARANMath.DegToRad(25), _IFTAS);

			bool validateTextBox0205 = (verify & 4) != 0;
			if ((verify & 2) != 0)
			{
				if (_IF2FAPdistance < _ImRange_Min || validateTextBox0205)
				{
					_IF2FAPdistance = _ImRange_Min;
					TextBox0105.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_IF2FAPdistance).ToString();
					validateTextBox0205 = true;
				}
			}
			else if ((verify & 1) != 0)
			{
				//double fTmp1 = _IFTurnR * Math.Tan(0.5 * _PlannedTurnAngle) + GlobalVars.SBASTransitionDistance;
				//_IF2FAPdistance = _IFTurnR * Math.Tan(0.5 * _PlannedTurnAngle) + GlobalVars.SBASTransitionDistance;
				//TextBox0205.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_IF2FAPdistance).ToString();
				//validateTextBox0205 = true;
			}

			if (ValidateComboBox0203)
			{
				if (ComboBox0105.Items.Count > 0)
					ComboBox0105.SelectedIndex = previ;
				else if (!RadioButton0101.Checked)
				{
					RadioButton0101.Checked = true;
					validateTextBox0205 = false;
				}
			}

			if (validateTextBox0205)
				TextBox0105_Validating(TextBox0105, null);

			if (fTmp != _hIF || (verify & 4) != 0)
			{
				TextBox0106.Tag = _hIF.ToString();
				ComboBox0104_SelectedIndexChanged(ComboBox0104, null);
				//TextBox0202_Validating(TextBox0202, null);
			}

			IFprj = ARANFunctions.LocalToPrj(_ptFAPprj, ArDir + Math.PI, _IF2FAPdistance);
			IFprj.Z = _hIF;
			IFprj.M = ArDir;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(IFElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(IntermSecondaryAreaElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(IntermPrimaryAreaElem);

			IFElem = GlobalVars.gAranGraphics.DrawPointWithText(IFprj, "IF", GlobalVars.WPTColor);

			//Protection area
			MultiPolygon pContur = GlobalVars.SBASOASPlanes[(int)eOAS.CommonPlane].Poly;

			GeometryOperators pTopoOper = new GeometryOperators();
			pTopoOper.CurrentGeometry = pContur;

			Point pt1, pt2;
			LineString mls = new LineString();
			Geometry geomLeft, geomRight;

			if (!pTopoOper.Disjoint(IFprj))
			{
				pt1 = ARANFunctions.LocalToPrj(IFprj, ArDir, 0.0, 100000.0);
				pt2 = ARANFunctions.LocalToPrj(IFprj, ArDir, 0.0, -100000.0);
				mls = new LineString();

				mls.Add(pt1);
				mls.Add(pt2);

				pTopoOper.Cut(pContur, mls, out geomLeft, out geomRight);

				if (geomLeft.Type == GeometryType.MultiPolygon)
					pContur = (MultiPolygon)geomLeft;
				else
				{
					pContur = new MultiPolygon();
					pContur.Add((Polygon)geomLeft);
				}
			}

			// Rectangle
			Ring pRing = new Ring();
			pRing.Add(ARANFunctions.LocalToPrj(FicTHRprj, ArDir, 0.0, GlobalVars.SBASWidth));
			pRing.Add(ARANFunctions.LocalToPrj(FicTHRprj, ArDir, -fFAPDist, GlobalVars.SBASWidth));
			pRing.Add(ARANFunctions.LocalToPrj(FicTHRprj, ArDir, -fFAPDist, -GlobalVars.SBASWidth));
			pRing.Add(ARANFunctions.LocalToPrj(FicTHRprj, ArDir, 0.0, -GlobalVars.SBASWidth));

			Polygon pPoly = new Polygon();
			pPoly.ExteriorRing = pRing;

			MultiPolygon pRectangle = new MultiPolygon();
			pRectangle.Add(pPoly);

			MultiPolygon protectionPoly;
			Geometry proGeom = pTopoOper.UnionGeometry(pContur, pRectangle);
			if (proGeom.Type == GeometryType.MultiPolygon)
				protectionPoly = (MultiPolygon)proGeom;
			else
			{
				protectionPoly = new MultiPolygon();
				protectionPoly.Add((Polygon)proGeom);
			}

			// Trapesoid
			D3Line xyLine = Common.IntersectPlanes(GlobalVars.SBASOASPlanes[(int)eOAS.YlPlane].Plane, GlobalVars.SBASOASPlanes[(int)eOAS.XlPlane].Plane);
			LineSegment InterLine = Common.IntersectPlanes(GlobalVars.SBASOASPlanes[(int)eOAS.YlPlane].Plane, GlobalVars.SBASOASPlanes[(int)eOAS.XlPlane].Plane, 0.0, _hFAP);

			y = (xyLine.A * fFAPDist + xyLine.C) / xyLine.B;
			if (y < GlobalVars.SBASWidth)
				y = GlobalVars.SBASWidth;

			pt1 = ARANFunctions.LocalToPrj(FicTHRprj, ArDir, -InterLine.Start.X, InterLine.Start.Y);
			Point ptLeft = ARANFunctions.LocalToPrj(FicTHRprj, ArDir, -fFAPDist, -y);

			pRing = new Ring();
			pRing.Add(pt1);
			pRing.Add(ptLeft);

			Point ptRight = ARANFunctions.LocalToPrj(FicTHRprj, ArDir, -fFAPDist, y);
			pt1 = ARANFunctions.LocalToPrj(FicTHRprj, ArDir, -InterLine.Start.X, -InterLine.Start.Y);
			pRing.Add(ptRight);
			pRing.Add(pt1);

			pPoly = new Polygon();
			pPoly.ExteriorRing = pRing;

			MultiPolygon pTrapesoid = new MultiPolygon();
			pTrapesoid.Add(pPoly);

			proGeom = pTopoOper.UnionGeometry(protectionPoly, pTrapesoid);

			if (proGeom.Type == GeometryType.MultiPolygon)
				protectionPoly = (MultiPolygon)proGeom;
			else
			{
				protectionPoly = new MultiPolygon();
				protectionPoly.Add((Polygon)proGeom);
			}

			//cutt ================================================================

			pt1 = ARANFunctions.LocalToPrj(_ptFAPprj, ArDir, 0.0, 100000.0);
			pt2 = ARANFunctions.LocalToPrj(_ptFAPprj, ArDir, 0.0, -100000.0);
			mls = new LineString();

			mls.Add(pt1);
			mls.Add(pt2);

			pTopoOper.Cut(protectionPoly, mls, out geomLeft, out geomRight);

			//GlobalVars.gAranGraphics.DrawMultiPolygon(protectionPoly, -1, AranEnvironment.Symbols.eFillStyle.sfsForwardDiagonal);
			//GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)geomLeft, 255, AranEnvironment.Symbols.eFillStyle.sfsNull);
			//Application.DoEvents();

			if (geomLeft.Type == GeometryType.MultiPolygon)
				protectionPoly = (MultiPolygon)geomLeft;
			else
			{
				protectionPoly = new MultiPolygon();
				protectionPoly.Add((Polygon)geomLeft);
			}

			//Secondary =======================================================================
			//GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value

			pRing = new Ring();
			pRing.Add(ptLeft);
			pRing.Add(ARANFunctions.LocalToPrj(_ptFAPprj, ArDir, -2.0 * 1852.0, -2.5 * 1852.0));
			pRing.Add(ARANFunctions.LocalToPrj(_ptFAPprj, ArDir, -_IF2FAPdistance, -2.5 * 1852.0));

			pRing.Add(ARANFunctions.LocalToPrj(_ptFAPprj, ArDir, -_IF2FAPdistance, 2.5 * 1852.0));
			pRing.Add(ARANFunctions.LocalToPrj(_ptFAPprj, ArDir, -2.0 * 1852.0, 2.5 * 1852.0));
			pRing.Add(ptRight);

			pPoly = new Polygon();
			pPoly.ExteriorRing = pRing;

			proGeom = pTopoOper.UnionGeometry(protectionPoly, pPoly);

			if (proGeom.Type == GeometryType.MultiPolygon)
				IntermSecondaryArea = (MultiPolygon)proGeom;
			else
			{
				IntermSecondaryArea = new MultiPolygon();
				IntermSecondaryArea.Add((Polygon)proGeom);
			}

			IntermSecondaryAreaElem = GlobalVars.gAranGraphics.DrawMultiPolygon(IntermSecondaryArea, AranEnvironment.Symbols.eFillStyle.sfsHollow, 255);

			//Primary =======================================================================
			//GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value

			pRing = new Ring();
			pRing.Add(ptLeft);
			pRing.Add(ARANFunctions.LocalToPrj(_ptFAPprj, ArDir, -2.0 * 1852.0, -0.5 * 2.5 * 1852.0));
			pRing.Add(ARANFunctions.LocalToPrj(_ptFAPprj, ArDir, -_IF2FAPdistance, -0.5 * 2.5 * 1852.0));

			pRing.Add(ARANFunctions.LocalToPrj(_ptFAPprj, ArDir, -_IF2FAPdistance, 0.5 * 2.5 * 1852.0));
			pRing.Add(ARANFunctions.LocalToPrj(_ptFAPprj, ArDir, -2.0 * 1852.0, 0.5 * 2.5 * 1852.0));
			pRing.Add(ptRight);

			pPoly = new Polygon();
			pPoly.ExteriorRing = pRing;

			proGeom = pTopoOper.UnionGeometry(protectionPoly, pPoly);

			if (proGeom.Type == GeometryType.MultiPolygon)
				IntermPrimaryArea = (MultiPolygon)proGeom;
			else
			{
				IntermPrimaryArea = new MultiPolygon();
				IntermPrimaryArea.Add((Polygon)proGeom);
			}

			IntermPrimaryAreaElem = GlobalVars.gAranGraphics.DrawMultiPolygon(IntermPrimaryArea, AranEnvironment.Symbols.eFillStyle.sfsHollow, ARANFunctions.RGB(0, 0, 255));

			Functions.GetPrecisionAndIntermetiatObstacles(WorkObstacleList, out PrecisionOASObstacles, GlobalVars.ObstacleList, out IntermetiatObstacles,
				_ptFAPprj, ArDir, FicTHRprj.Z, GlobalVars.SBASOASPlanes[(int)eOAS.CommonPlane].Poly, IntermSecondaryArea, IntermPrimaryArea);

			reportForm.FillPage03(PrecisionOASObstacles);
			reportForm.FillPage04(IntermetiatObstacles);
		}

		private void ExcludeBtn_Click(object sender, EventArgs e)
		{
			int m = WorkObstacleList.Obstacles.Length;
			int n = WorkObstacleList.Parts.Length;

			ObstacleContainer LocalObstacleList;

			LocalObstacleList.Obstacles = new Obstacle[m];
			LocalObstacleList.Parts = new ObstacleData[n];

			int i;
			for (i = 0; i < m; i++)
			{
				WorkObstacleList.Obstacles[i].NIx = -1;
				//WorkObstacleList.Obstacles[I].IgnoredByUser = False;
				WorkObstacleList.Obstacles[i].Index = i;
			}

			int k = -1, l = -1;
			for (i = 0; i < n; i++)
			{
				//SideDirection Side = ARANMath.SideDef(FicTHRprj, ArDir + ARANMath.C_PI_2, WorkObstacleList.Parts[i].pPtPrj);
				//double Dist = ARANFunctions.Point2LineDistancePrj(WorkObstacleList.Parts[i].pPtPrj, FicTHRprj, ArDir + ARANMath.C_PI_2);
				//double Dist1 = ARANFunctions.Point2LineDistancePrj(WorkObstacleList.Parts[i].pPtPrj, FicTHRprj, ArDir);

				//if (Side = 1 && Dist < 900.0 && Dist1 < 150.0 && WorkObstacleList.Parts[i].ReqOCH > 0.0)

				if (WorkObstacleList.Parts[i].ReqOCH > 0.0)
				{
					k++;
					LocalObstacleList.Parts[k] = WorkObstacleList.Parts[i];

					if (WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].NIx < 0)
					{
						l++;
						LocalObstacleList.Obstacles[l] = WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner];
						LocalObstacleList.Obstacles[l].PartsNum = 0;
						WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].NIx = l;
						//LocalObstacleList.Obstacles[l].Parts = new int[WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].PartsNum];
					}

					LocalObstacleList.Parts[k].Owner = WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].NIx;
					LocalObstacleList.Parts[k].Index = LocalObstacleList.Obstacles[LocalObstacleList.Parts[k].Owner].PartsNum;
					LocalObstacleList.Obstacles[LocalObstacleList.Parts[k].Owner].PartsNum++;
					//LocalObstacleList.Obstacles[LocalObstacleList.Parts[k].Owner].Parts[LocalObstacleList.Parts[k].Index].PartsNum] = k;
				}
			}

			if (k < 0)
			{
				LocalObstacleList.Obstacles = new Obstacle[0];
				LocalObstacleList.Parts = new ObstacleData[0];
			}
			else
			{
				Array.Resize<ObstacleData>(ref LocalObstacleList.Parts, k + 1);
				Array.Resize<Obstacle>(ref LocalObstacleList.Obstacles, l + 1);

				if (excludeObstFrm.ExcludeOstacles(ref LocalObstacleList, this))
				{
					HaveExcluded = false;

					for (i = 0; i <= l; i++)
					{
						WorkObstacleList.Obstacles[LocalObstacleList.Obstacles[i].Index].IgnoredByUser = LocalObstacleList.Obstacles[i].IgnoredByUser;
						if (LocalObstacleList.Obstacles[i].IgnoredByUser)
							HaveExcluded = true;
					}

					//ComboBox0102_SelectedIndexChanged(ComboBox0102, null);
				}
			}
		}

		private void ComboBox0101_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!FormInitialised)
				return;

			if (ComboBox0101.SelectedIndex < 0)
				ComboBox0101.SelectedIndex = 0;

			double fTmp;

			if (!double.TryParse(TextBox0102.Tag.ToString(), out fTmp))
				return;

			if (ComboBox0101.SelectedIndex == 0)
				TextBox0102.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fTmp + FicTHRprj.Z, eRoundMode.NEAREST).ToString();
			else
				TextBox0102.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fTmp, eRoundMode.NEAREST).ToString();
		}

		private void ComboBox0102_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!FormInitialised)
				return;

			if (ComboBox0102.SelectedIndex < 0)
				ComboBox0102.SelectedIndex = 0;

			if (ComboBox0102.SelectedIndex == 0)
				TextBox0103.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_CurrFAPOCH + FicTHRprj.Z, eRoundMode.NEAREST).ToString();
			else
				TextBox0103.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_CurrFAPOCH, eRoundMode.NEAREST).ToString();
		}

		private void ComboBox0103_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void ComboBox0104_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!FormInitialised)
				return;

			if (ComboBox0104.SelectedIndex < 0)
				ComboBox0104.SelectedIndex = 0;

			double fTmp;

			if (!double.TryParse(TextBox0106.Tag.ToString(), out fTmp))
				return;

			if (ComboBox0104.SelectedIndex == 0)
				TextBox0106.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fTmp + FicTHRprj.Z, eRoundMode.NEAREST).ToString();
			else
				TextBox0106.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fTmp, eRoundMode.NEAREST).ToString();
		}

		private void ComboBox0105_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ComboBox0105.SelectedIndex < 0)
				return;

			NavaidType selected = (NavaidType)ComboBox0105.SelectedItem;

			if (!RadioButton0102.Checked)
				return;

			_IF2FAPdistance = ARANFunctions.ReturnDistanceInMeters(selected.pPtPrj, _ptFAPprj);
			TextBox0105.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_IF2FAPdistance).ToString();
			TextBox0105.Tag = TextBox0105.Text;
			CheckAndDrawIF(2);
		}

		private void RadioButton0101_CheckedChanged(object sender, EventArgs e)
		{
			RadioButton control = (RadioButton)sender;
			if (!control.Checked)
				return;

			if (control.Tag == null)
			{
				TextBox0105.ReadOnly = false;
				ComboBox0105.Enabled = false;
				TextBox0105_Validating(TextBox0105, null);
			}
			else
			{
				TextBox0105.ReadOnly = true;
				ComboBox0105.Enabled = true;
				ComboBox0105_SelectedIndexChanged(ComboBox0105, null);
			}
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

			fFAPDist = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

			fTmp = fFAPDist;
			if (fFAPDist < fMinFAPDist)
				fFAPDist = fMinFAPDist;
			if (fFAPDist > fMaxFAPDist)
				fFAPDist = fMaxFAPDist;

			//if (fTmp != fFAPDist)
			//	TextBox0101.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(fFAPDist, eRoundMode.NERAEST).ToString();

			_hFAP = FAPDist2hFAP(fFAPDist);

			Caller(out _CurrFAPOCH, ref _hFAP);
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

			fTmp = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);
			if (ComboBox0101.SelectedIndex == 0)
				fTmp -= FicTHRprj.Z;

			double fTmp1 = 0.0;

			if (TextBox0102.Tag != null && double.TryParse(TextBox0102.Tag.ToString(), out fTmp1) && fTmp == fTmp1)
				return;

			_hFAP = fTmp;

			if (_hFAP < GlobalVars.arHOASPlaneCat1)
				_hFAP = GlobalVars.arHOASPlaneCat1;		// arISegmentMOC.Value

			if (_hFAP == fTmp1)
				return;

			Caller(out _CurrFAPOCH, ref _hFAP);
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

			_IF2FAPdistance = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);
			fTmp = _IF2FAPdistance;
			/*=====================*/

			if (_IF2FAPdistance < _ImRange_Min)
				_IF2FAPdistance = _ImRange_Min;

			if (_IF2FAPdistance > GlobalVars.constants.Pansops[ePANSOPSData.arImRange_Max].Value)
				_IF2FAPdistance = GlobalVars.constants.Pansops[ePANSOPSData.arImRange_Max].Value;

			if (fTmp != _IF2FAPdistance)
				TextBox0105.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_IF2FAPdistance).ToString();

			TextBox0105.Tag = TextBox0105.Text;

			CheckAndDrawIF(2);
		}

		private void TextBox0106_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0106_Validating(TextBox0106, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0106.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0106_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;
			if (!double.TryParse(TextBox0106.Text, out fTmp))
				return;

			fTmp = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);

			if (ComboBox0104.SelectedIndex == 0)
				fTmp -= FicTHRprj.Z;

			double fTmp1;
			if (TextBox0106.Tag != null && double.TryParse(TextBox0106.Tag.ToString(), out fTmp1) && fTmp == fTmp1)
				return;

			_hIF = fTmp;

			if (_hIF < _hIF_Min)
				_hIF = _hIF_Min;

			if (_hIF > _hIF_Max)
				_hIF = _hIF_Max;

			if (fTmp != _hIF)
			{
				TextBox0106.Tag = _hIF.ToString();
				ComboBox0104_SelectedIndexChanged(ComboBox0104, null);
			}

			//CheckAndDrawIF(1);
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

			_InterDescGrad = 0.01 * fTmp;
			fTmp = _InterDescGrad;

			if (_InterDescGrad < 0.0)
				_InterDescGrad = 0.0;

			if (_InterDescGrad > TanGPA)
				_InterDescGrad = TanGPA;

			if (fTmp != _InterDescGrad)
				TextBox0107.Text = (100.0 * _InterDescGrad).ToString("0.0");

			TextBox0107.Tag = TextBox0107.Text;
			/*	==========	*/
		}

		private void TextBox0109_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0109_Validating(TextBox0109, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0109.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0109_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;
			if (!double.TryParse(TextBox0109.Text, out fTmp))
				return;

			if (TextBox0109.Tag != null && TextBox0109.Tag.ToString() == TextBox0109.Text)
				return;

			_IFIAS = fTmp = GlobalVars.unitConverter.SpeedToInternalUnits(fTmp);

			if (_IFIAS < GlobalVars.constants.AircraftCategory[aircraftCategoryData.ViafMin].Value[Category])
				_IFIAS = GlobalVars.constants.AircraftCategory[aircraftCategoryData.ViafMin].Value[Category];

			if (_IFIAS > GlobalVars.constants.AircraftCategory[aircraftCategoryData.ViafMax].Value[Category])
				_IFIAS = GlobalVars.constants.AircraftCategory[aircraftCategoryData.ViafMax].Value[Category];

			if (fTmp != _IFIAS)
				TextBox0109.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(_IFIAS, eRoundMode.NEAREST).ToString();

			TextBox0109.Tag = TextBox0109.Text;

			//OCHMin = GP_RDH + AlignP_THR * TanGPA;

			CheckAndDrawIF(3);
		}

		private void TextBox0110_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0110_Validating(TextBox0110, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0110.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0110_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;
			if (!double.TryParse(TextBox0110.Text, out fTmp))
				return;

			if (TextBox0110.Tag != null && TextBox0110.Tag.ToString() == TextBox0110.Text)
				return;

			_PlannedTurnAtIF = fTmp;

			if (_PlannedTurnAtIF < 0.0)
				_PlannedTurnAtIF = 0.0;

			if (_PlannedTurnAtIF > 90.0)
				_PlannedTurnAtIF = 90.0;

			if (fTmp != _PlannedTurnAtIF)
				TextBox0110.Text = _PlannedTurnAtIF.ToString("0.0");

			TextBox0110.Tag = TextBox0110.Text;
			_PlannedTurnAtIF = ARANMath.DegToRad(_PlannedTurnAtIF);

			CheckAndDrawIF(3);
		}

		#endregion

		#region Page III

		private bool preparePageIII()
		{
			mahfFix = new MATF(GlobalVars.gAranEnv);

			mahfFix.TurnAt = eTurnAt.TP;
			mahfFix.SensorType = eSensorType.GNSS;
			mahfFix.FlightPhase = eFlightPhase.MApLT28;
			mahfFix.PBNType = ePBNClass.RNP_APCH;
			mahfFix.Role = eFIXRole.MAHF_LE_56;
			mahfFix.ISAtC = GlobalVars.CurrADHP.ISAtC;

			mahfFix.EntryDirection = ArDir;
			mahfFix.OutDirection = ArDir;


			_maxMAHFDist = GlobalVars.MATMinRange;

			TextBox0201.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_maxMAHFDist).ToString();
			TextBox0201.Tag = TextBox0201.Text;

			CreateMAPolygon();
			TextBox0202.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_CurrFAPOCH, eRoundMode.NEAREST).ToString();
			TextBox0205.Text = TextBox0104.Text;

			//ArrivalProfile.MAPtIndex = ArrivalProfile.PointsNo

			return true;
		}

		private double CreateMAPolygon()
		{
			double CoTanGPA = 1.0 / TanGPA;
			double CoTanZ = 1.0 / fMissAprPDG;

			Ring pRing = Functions.ReArrangeRing(GlobalVars.SBASOASPlanes[(int)eOAS.ZPlane].Poly[0].ExteriorRing, FicTHRprj, ArDir);
			Ring ZContRing = new Ring();

			ZContRing.Add(pRing[0]);
			ZContRing.Add(ARANFunctions.LocalToPrj(pRing[0], ArDir, _maxMAHFDist));
			ZContRing.Add(ARANFunctions.LocalToPrj(pRing[pRing.Count - 1], ArDir, _maxMAHFDist));
			ZContRing.Add(pRing[pRing.Count - 1]);
			ZContRing.Add(pRing[0]);

			Polygon ppgon = new Polygon();
			ppgon.ExteriorRing = ZContRing;
			ZContinued = new MultiPolygon();
			ZContinued.Add(ppgon);

			//============================================================================================================
			GeometryOperators geomOp = new GeometryOperators();
			geomOp.CurrentGeometry = GlobalVars.SBASOASPlanes[(int)eOAS.CommonPlane].Poly[0];
			pFullPoly = (MultiPolygon)geomOp.UnionGeometry(GlobalVars.SBASOASPlanes[(int)eOAS.CommonPlane].Poly[0], ZContinued);

			LeftLine = Functions.ReturnPolygonPartAsPolyline(pFullPoly, FicTHRprj, ArDir, 1);
			RightLine = Functions.ReturnPolygonPartAsPolyline(pFullPoly, FicTHRprj, ArDir, -1);

			PtCoordCntr = new Point(0.5 * (LeftLine[0].X + RightLine[0].X), 0.5 * (LeftLine[0].Y + RightLine[0].Y));

			XptLH = ARANFunctions.ReturnDistanceInMeters(FicTHRprj, PtCoordCntr);
			//============================================================
			GlobalVars.gAranGraphics.SafeDeleteGraphic(ZContinuedElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(SOCElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(pMAPtElem);

			ZContinuedElem = GlobalVars.gAranGraphics.DrawMultiPolygon(ZContinued, AranEnvironment.Symbols.eFillStyle.sfsHollow, 0);

			Functions.GetIntermObstacleList(GlobalVars.ObstacleList, out MAObstacleList, FicTHRprj, ArDir, ZContinued);

			int i, MAPrCnt = MAObstacleList.Parts.Length,
				MAObCnt = MAObstacleList.Obstacles.Length;

			int ix = -1;
			fMisAprOCH = _CurrFAPOCH;

			for (i = 0; i < MAPrCnt; i++)
			{
				MAObstacleList.Parts[i].Plane = (int)eOAS.NonPrec;
				MAObstacleList.Parts[i].EffectiveHeight = (MAObstacleList.Parts[i].Height * CoTanZ + (_zSurfaceOrigin + MAObstacleList.Parts[i].Dist)) / (CoTanZ + CoTanGPA);
				MAObstacleList.Parts[i].hSurface = -(MAObstacleList.Parts[i].Dist + _zSurfaceOrigin) * fMissAprPDG;
				MAObstacleList.Parts[i].hPenet = MAObstacleList.Parts[i].Height - MAObstacleList.Parts[i].hSurface;
				MAObstacleList.Parts[i].Flags = 1;
				MAObstacleList.Parts[i].ReqOCH = Math.Min(MAObstacleList.Parts[i].Height, MAObstacleList.Parts[i].EffectiveHeight) + m_fMOC;

				if (MAObstacleList.Parts[i].hPenet <= 0.0 || MAObstacleList.Parts[i].ReqH > _hFAP || MAObstacleList.Parts[i].ReqOCH <= fMisAprOCH)
					continue;

				fMisAprOCH = MAObstacleList.Parts[i].ReqOCH;
				ix = i;
			}
			// /===========================================/ //
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

				if (MAObstacleList.Parts[MAPrCnt].hPenet > 0.0 && MAObstacleList.Parts[MAPrCnt].ReqH <= _hFAP)
				{
					MAObstacleList.Parts[MAPrCnt].EffectiveHeight = (MAObstacleList.Parts[MAPrCnt].Height * CoTanZ + (_zSurfaceOrigin + MAObstacleList.Parts[MAPrCnt].Dist)) / (CoTanZ + CoTanGPA);
					MAObstacleList.Parts[MAPrCnt].ReqOCH = Math.Min(MAObstacleList.Parts[MAPrCnt].Height, MAObstacleList.Parts[MAPrCnt].EffectiveHeight) + m_fMOC;

					if (MAObstacleList.Parts[MAPrCnt].ReqOCH > fMisAprOCH)
					{
						fMisAprOCH = MAObstacleList.Parts[MAPrCnt].ReqOCH;
						ix = MAPrCnt;
					}
				}

				MAPrCnt++;
			}

			if (MAPrCnt > 0)
			{
				Array.Resize<ObstacleData>(ref MAObstacleList.Parts, MAPrCnt);
				Array.Resize<Obstacle>(ref MAObstacleList.Obstacles, MAPrCnt);
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

			TextBox0204.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_SOC2THRDist, eRoundMode.NEAREST).ToString();

			_MAPt2THRDist = (fMisAprOCH - GP_RDH) * CoTanGPA;
			pMAPt = ARANFunctions.LocalToPrj(FicTHRprj, ArDir, -_MAPt2THRDist);
			pMAPt.Z = fMisAprOCH;
			pMAPt.M = ArDir;

			//maptFix.TurnAltitude = fMisAprOCH + FicTHRprj.Z;
			//maptFix.PrjPt = pMAPt;

			SOCElem = GlobalVars.gAranGraphics.DrawPointWithText(PtSOC, "SOC", GlobalVars.WPTColor);
			pMAPtElem = GlobalVars.gAranGraphics.DrawPointWithText(pMAPt, "MAPt", GlobalVars.WPTColor);

			TextBox0203.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fMisAprOCH, eRoundMode.NEAREST).ToString();

			if (fMisAprOCH > _CurrFAPOCH || fMisAprOCH > _hFAP)
				TextBox0203.ForeColor = System.Drawing.Color.Red;
			else
				TextBox0203.ForeColor = System.Drawing.Color.Black;

			if (ix >= 0)
				TextBox0206.Text = MAObstacleList.Obstacles[MAObstacleList.Parts[ix].Owner].UnicalName;
			else
				TextBox0206.Text = "-";

			reportForm.FillPage05(MAObstacleList);

			ComboBox0201_SelectedIndexChanged(ComboBox0201, null);
			return fMisAprOCH;
		}

		private void CreateMAHF(double MAHFDistance, NavaidType nav = default(NavaidType ), bool ByDistance = true)
		{
			//GlobalVars.gAranGraphics.SafeDeleteGraphic(MAHFElem);
			//GlobalVars.gAranGraphics.SafeDeleteGraphic(MAHFTolerAreaElem);
			if (ByDistance)
				ptMAHF = ARANFunctions.LocalToPrj(FicTHRprj, ArDir, MAHFDistance);
			else
				ptMAHF = (Point)nav.pPtPrj.Clone();

			ptMAHF.Z = PtSOC.Z + (MAHFDistance - _SOC2THRDist) * fMissAprPDG;

			_maxTermAlt = ptMAHF.Z + FicTHRprj.Z;
			if (_maxTermAlt < _minTermAlt)
				_maxTermAlt = _minTermAlt;

			mahfFix.NomLineAltitude = _maxTermAlt;
			mahfFix.PrjPt = ptMAHF;
			mahfFix.DeleteGraphics();
			mahfFix.RefreshGraphics();

			TextBox0208.Tag = null;
			TextBox0208_Validating(TextBox0208, null);
		}

		private void ComboBox0201_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (MultiPage1.SelectedIndex < 1 || !FormInitialised)
				return;

			if (ComboBox0201.SelectedIndex < 0)
			{
				ComboBox0201.SelectedIndex = 0;
				return;
			}

			_EnRoteMOC = GlobalVars.unitConverter.HeightToInternalUnits(double.Parse(ComboBox0201.Text));
			_straightMissedTermHeight = _EnRoteMOC;

			int n = MAObstacleList.Parts.Length;

			for (int i = 0; i < n; i++)
			{
				double fReqH = MAObstacleList.Parts[i].Height + _EnRoteMOC;

				if (fReqH > _straightMissedTermHeight)
					_straightMissedTermHeight = fReqH;
			}

			_minTermAlt = GlobalVars.unitConverter.HeightToDisplayUnits(_straightMissedTermHeight + FicTHRprj.Z, eRoundMode.SPECIAL_CEIL);
			TextBox0207.Text = _minTermAlt.ToString();
			_minTermAlt = GlobalVars.unitConverter.HeightToInternalUnits(_minTermAlt);

			_minMAHFDist = _SOC2THRDist + (_straightMissedTermHeight - PtSOC.Z) / fMissAprPDG;
			double fTmp;
			if (!double.TryParse(TextBox0209.Text, out fTmp))
				TextBox0209.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_minMAHFDist).ToString();

			if (!double.TryParse(TextBox0208.Text, out fTmp))
				TextBox0208.Text = TextBox0207.Text;
			else
			{
				TextBox0208.Tag = null;
				TextBox0208_Validating(TextBox0208, null);
			}

			Guid prevPt = default(Guid);
			if (ComboBox0202.SelectedIndex >= 0)
				prevPt = ((NavaidType)ComboBox0202.SelectedItem).Identifier;

			int previ = 0;
			n = InSectList.Length;
			ComboBox0202.Items.Clear();

			for (int i = 0; i < n; i++)
			{
				double X, Y;
				ARANFunctions.PrjToLocal(FicTHRprj, ArDir, InSectList[i].pPtPrj, out X, out Y);

				if (X < _minMAHFDist) continue;
				if (X > _maxMAHFDist) continue;

				ComboBox0202.Items.Add(InSectList[i]);
				if (InSectList[i].Identifier == prevPt)
					previ = i;
			}

			if (ComboBox0202.Items.Count > 0)
			{
				RadioButton0202.Enabled = true;
				ComboBox0202.SelectedIndex = previ;
			}
			else
			{
				RadioButton0202.Enabled = false;
				RadioButton0202.Checked = false;
			}

			if (!RadioButton0202.Checked)
			{
				TextBox0209.Tag = null;
				TextBox0209_Validating(TextBox0209, null);
			}
		}

		private void ComboBox0202_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!FormInitialised)
				return;

			int k = ComboBox0202.SelectedIndex;
			if (k < 0)
				return;

			NavaidType newMAHF = (NavaidType)ComboBox0202.SelectedItem;

			if (newMAHF.TypeCode == eNavaidType.NONE)
				_Label0201_19.Text = "WPT/FIX";
			else
				_Label0201_19.Text = newMAHF.TypeCode.ToString();

			CreateMAHF(0, newMAHF, false);
		}

		private void CheckBox0201_CheckedChanged(object sender, EventArgs e)
		{
			if (!FormInitialised)
				return;

			_Label0201_20.Enabled = !CheckBox0201.Checked;
			Frame0201.Enabled = !CheckBox0201.Checked;
			OkBtn.Enabled = !CheckBox0201.Checked;
			NextBtn.Enabled = CheckBox0201.Checked;
		}

		private void RadioButton0201_CheckedChanged(object sender, EventArgs e)
		{
			if (!FormInitialised) return;

			if (!((RadioButton)sender).Checked)
				return;

			if (RadioButton0201.Checked)
			{
				TextBox0209.ReadOnly = false;
				ComboBox0202.Enabled = false;
				TextBox0209_Validating(TextBox0209, null);
			}
			else
			{
				TextBox0209.ReadOnly = true;
				ComboBox0202.Enabled = true;
				ComboBox0202_SelectedIndexChanged(ComboBox0202, null);
			}
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
			double fTmp;

			if (!double.TryParse(TextBox0201.Text, out fTmp))
				return;

			if (TextBox0201.Tag != null && TextBox0201.Tag.ToString() == TextBox0201.Text)
				return;

			_maxMAHFDist = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

			fTmp = _maxMAHFDist;

			if (_maxMAHFDist < GlobalVars.MATMinRange)
				_maxMAHFDist = GlobalVars.MATMinRange;
			if (_maxMAHFDist > GlobalVars.ModellingRadius)
				_maxMAHFDist = GlobalVars.ModellingRadius;

			if (_maxMAHFDist != fTmp)
				TextBox0201.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_maxMAHFDist).ToString();

			TextBox0201.Tag = TextBox0201.Text;
			CreateMAPolygon();
		}

		private void TextBox0208_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0208_Validating(TextBox0208, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0208.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0208_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(TextBox0208.Text, out fTmp))
				return;

			if (TextBox0208.Tag != null && TextBox0208.Tag.ToString() == TextBox0208.Text)
				return;

			_MAHFTerminationAlt = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);

			fTmp = _MAHFTerminationAlt;
			if (_MAHFTerminationAlt < _minTermAlt)
				_MAHFTerminationAlt = _minTermAlt;
			if (_MAHFTerminationAlt > _maxTermAlt)
				_MAHFTerminationAlt = _maxTermAlt;

			if (fTmp != _MAHFTerminationAlt)
				TextBox0208.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_MAHFTerminationAlt).ToString();

			TextBox0208.Tag = TextBox0208.Text;
		}

		private void TextBox0209_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0209_Validating(TextBox0209, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0209.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0209_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(TextBox0209.Text, out fTmp))
				return;

			if (TextBox0209.Tag != null && TextBox0209.Tag.ToString() == TextBox0209.Text)
				return;

			_MAHF2THRDist = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

			fTmp = _MAHF2THRDist;
			if (_MAHF2THRDist < _minMAHFDist)
				_MAHF2THRDist = _minMAHFDist;
			if (_MAHF2THRDist > _maxMAHFDist)
				_MAHF2THRDist = _maxMAHFDist;

			if (fTmp != _MAHF2THRDist)
				TextBox0209.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_MAHF2THRDist).ToString();

			TextBox0209.Tag = TextBox0209.Text;

			CreateMAHF(_MAHF2THRDist);
		}

		#endregion

		#region Page IV

		private bool preparePageIV()
		{
			_TurnMoreThan15 = true;
			_PlannedTurnAtMATF = 0.5*Math.PI;
			TextBox0309.Text = ARANMath.RadToDeg(_PlannedTurnAtMATF).ToString("0");

			TextBox0302.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(GlobalVars.constants.AircraftCategory[aircraftCategoryData.VmaFaf].Value[Category], eRoundMode.SPECIAL_NEAREST).ToString();
			_Label0301_10.Text = "		Recomended range:\r\nfrom " +
				GlobalVars.unitConverter.SpeedToDisplayUnits(GlobalVars.constants.AircraftCategory[aircraftCategoryData.VmaInter].Value[Category], eRoundMode.SPECIAL_NEAREST).ToString() +
				" to " + TextBox0302.Text + " " + GlobalVars.unitConverter.SpeedUnit;

			_BankAngle = ARANMath.DegToRad(double.Parse(TextBox0301.Text));

			if (_TurnIAS < GlobalVars.constants.AircraftCategory[aircraftCategoryData.VmaFaf].Value[Category])
				_TurnIAS = GlobalVars.constants.AircraftCategory[aircraftCategoryData.VmaFaf].Value[Category];

			if (_TurnIAS > GlobalVars.constants.AircraftCategory[aircraftCategoryData.VmaInter].Value[Category])
				_TurnIAS = GlobalVars.constants.AircraftCategory[aircraftCategoryData.VmaInter].Value[Category];

			mahfFix.Role = eFIXRole.MATF_LE_56;
			mahfFix.IAS = _TurnIAS;

			//fIAS = 3.6 * DeConvertSpeed(CDbl(TextBox0402.Text))
			CalcTNHIntervals();
			Transition1();

			return true;
		}

		private void Transition1()
		{
			double fMinOCH;

			if (IxOCH > -1)
			{
				if (ARANMath.SideDef(PtCoordCntr, ArDir + ARANMath.C_PI_2, WorkObstacleList.Parts[IxOCH].pPtPrj) == SideDirection.sideLeft)
					fMinOCH = _CurrFAPOCH;
				else
					fMinOCH = m_fMOC > fRDHOCH ? m_fMOC : fRDHOCH;
			}
			else
				fMinOCH = m_fMOC > fRDHOCH ? m_fMOC : fRDHOCH;

			m_IxMinOCH = IxMaxOCH = -1;
			double CurrOCH, NextOCH = fMinOCH;

			do
			{
				CurrOCH = NextOCH;
				Interval OCHInterv = CalcOCHRange(ref CurrOCH, ref NextOCH, ref m_IxMinOCH, ref IxMaxOCH);
				m_TurnIntervals = CalcTurnInterval(ref CurrOCH, ref NextOCH, ref OCHInterv, ref m_IxMinOCH);
			} while (CurrOCH > NextOCH && IxMaxOCH >= 0);

			if (m_IxMinOCH > -1)
				TextBox0303.Text = DetTNHObstacles.Obstacles[DetTNHObstacles.Parts[m_IxMinOCH].Owner].UnicalName;
			else if (IxOCH > -1)
				TextBox0303.Text = WorkObstacleList.Obstacles[WorkObstacleList.Parts[IxOCH].Owner].UnicalName;
			else
				TextBox0303.Text = "";

			fMisAprOCH = CurrOCH;
			ComboBox0302_SelectedIndexChanged(ComboBox0302, null);

			TextBox0306.Text = GlobalVars.unitConverter.HeightToDisplayUnits(CurrOCH, eRoundMode.NEAREST).ToString();

			fHTurn = fTNH;
			DrawMAPtSOC(ref fMisAprOCH, ref NextOCH, ref m_TurnIntervals, ref IxMaxOCH);
		}

		private void CalcTurnRange(ref ObstacleContainer OCHObstacles)
		{
			double HAbsTurn = FicTHRprj.Z + GlobalVars.constants.Pansops[ePANSOPSData.arMATurnAlt].Value;

			_TurnTAS = ARANMath.IASToTASForRnav(_TurnIAS, HAbsTurn, GlobalVars.CurrADHP.ISAtC - GlobalVars.constants.Pansops[ePANSOPSData.arISAmax].Value);
			_TurnTurnR = ARANMath.BankToRadius(_BankAngle, _TurnTAS);

			double Rv = 1.76527777777777777777 * Math.Tan(_BankAngle) / (ARANMath.C_PI * _TurnTAS);
			if (Rv > 0.003) Rv = 0.003;

			double e = (180.0 * 0.001 / System.Math.PI) * GlobalVars.constants.PansopsCoreDB.DepWs.Value / Rv;
			//double e = 2.0 * GlobalVars.constants.PansopsCoreDB.DepWs.Value * _TurnTAS / (254.168 * System.Math.Tan(_BankAngle));
			int i, n = OCHObstacles.Parts.Length;

			for (i = 0; i < n; i++)
			{
				Interval TurnRange;

				if (TurnDir > 0)
					TurnRange = Functions.CalcSpiralStartPoint(RightLine, OCHObstacles.Parts[i], e, _TurnTurnR, ArDir, TurnDir);
				else
					TurnRange = Functions.CalcSpiralStartPoint(LeftLine, OCHObstacles.Parts[i], e, _TurnTurnR, ArDir, TurnDir);

				OCHObstacles.Parts[i].TurnDistL = TurnRange.Min;
				OCHObstacles.Parts[i].TurnAngleL = TurnRange.Max;
			}

			reportForm.FillPage06(OCHObstacles);
			Functions.Sort(ref OCHObstacles, 1);
		}

		private void CalcTNHIntervals()
		{
			double CoTanGPA = 1.0 / TanGPA;
			double CoTanZ = 1.0 / fMissAprPDG;
			double TurnMOC;

			if (_TurnMoreThan15)
				TurnMOC = GlobalVars.constants.Pansops[ePANSOPSData.arMA_FinalMOC].Value;
			else
				TurnMOC = GlobalVars.constants.Pansops[ePANSOPSData.arMA_InterMOC].Value;

			//=============================================================================
			Functions.GetIntermObstacleList(GlobalVars.ObstacleList, out DetTNHObstacles, FicTHRprj, ArDir, ZContinued);

			int i;
			int DtObCnt = DetTNHObstacles.Obstacles.Length;
			int DtPrCnt = DetTNHObstacles.Parts.Length;

			for (i = 0; i < DtPrCnt; i++)
			{
				DetTNHObstacles.Parts[i].ReqOCH = (DetTNHObstacles.Parts[i].Height * CoTanZ + (_zSurfaceOrigin - DetTNHObstacles.Parts[i].Dist)) / (CoTanZ + CoTanGPA) + m_fMOC;
				if (DetTNHObstacles.Parts[i].ReqOCH < 0.0)
					DetTNHObstacles.Parts[i].ReqOCH = 0.0;

				DetTNHObstacles.Parts[i].ReqH = DetTNHObstacles.Parts[i].Height + TurnMOC;
				DetTNHObstacles.Parts[i].Dist = XptLH + DetTNHObstacles.Parts[i].Dist;
				DetTNHObstacles.Parts[i].Plane = (int)eOAS.NonPrec;
			}

			int WrObCnt = WorkObstacleList.Obstacles.Length;
			int WrPrCnt = WorkObstacleList.Parts.Length;

			if (DtPrCnt > 0)
			{
				Array.Resize<Obstacle>(ref DetTNHObstacles.Obstacles, WrObCnt + DtObCnt);
				Array.Resize<ObstacleData>(ref DetTNHObstacles.Parts, WrPrCnt + DtPrCnt);
			}
			else if (WrPrCnt > 0)
			{
				DetTNHObstacles.Obstacles = new Obstacle[WrObCnt];
				DetTNHObstacles.Parts = new ObstacleData[WrPrCnt];
			}
			else
			{
				DetTNHObstacles.Obstacles = new Obstacle[0];
				DetTNHObstacles.Parts = new ObstacleData[0];
			}

			double fCutDist = ARANFunctions.ReturnDistanceInMeters(IFprj, FicTHRprj);

			GeometryOperators pYPlaneRelation = new GeometryOperators();
			pYPlaneRelation.CurrentGeometry = GlobalVars.SBASOASPlanes[(int)eOAS.ZPlane + TurnDir].Poly;

			for (i = 0; i < WrObCnt; i++)
				WorkObstacleList.Obstacles[i].NIx = -1;

			for (i = 0; i < WrPrCnt; i++)
			{
				//If (WorkObstacleList.Parts(I).Dist < XptLH) And ((Not pYPlaneRelation.Contains(WorkObstacleList.Parts(I).pPtPrj)) Or (WorkObstacleList.Parts(I).hPent > 0.0)) Then
				//If (Not pYPlaneRelation.Contains(WorkObstacleList.Parts(I).pPtPrj) Or (WorkObstacleList.Parts(I).hPenet > 0.0)) And (WorkObstacleList.Parts(I).ReqH < _hFAP) And (WorkObstacleList.Parts(I).Dist < fCutDist) Then

				if (((!pYPlaneRelation.Contains(WorkObstacleList.Parts[i].pPtPrj) || WorkObstacleList.Parts[i].hPenet > 0.0) && WorkObstacleList.Parts[i].ReqH < _hFAP && WorkObstacleList.Parts[i].Dist < fCutDist))
				{

				}
				else
					continue;

				DetTNHObstacles.Parts[DtPrCnt] = WorkObstacleList.Parts[i];

				if (WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].NIx < 0)
				{
					DetTNHObstacles.Obstacles[DtObCnt] = WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner];
					DetTNHObstacles.Obstacles[DtObCnt].PartsNum = 0;
					//DetTNHObstacles.Obstacles[DtObCnt].Parts = new int[WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].PartsNum];
					WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].NIx = DtObCnt;
					DtObCnt++;
				}

				DetTNHObstacles.Parts[DtPrCnt].Owner = WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].NIx;
				DetTNHObstacles.Parts[DtPrCnt].Index = DetTNHObstacles.Obstacles[DetTNHObstacles.Parts[DtPrCnt].Owner].PartsNum;
				//DetTNHObstacles.Obstacles[DetTNHObstacles.Parts[DtPrCnt].Owner].Parts[DetTNHObstacles.Obstacles[DetTNHObstacles.Parts[DtPrCnt].Owner].PartsNum] = DtPrCnt;
				DetTNHObstacles.Obstacles[DetTNHObstacles.Parts[DtPrCnt].Owner].PartsNum++;

				DetTNHObstacles.Parts[DtPrCnt].Dist = XptLH - DetTNHObstacles.Parts[DtPrCnt].Dist;

				if (DetTNHObstacles.Parts[DtPrCnt].hPenet > 0.0)
					DetTNHObstacles.Parts[DtPrCnt].ReqOCH = Math.Min(DetTNHObstacles.Parts[DtPrCnt].Height, DetTNHObstacles.Parts[DtPrCnt].EffectiveHeight) + m_fMOC;
				else
					DetTNHObstacles.Parts[DtPrCnt].ReqOCH = 0.0;

				DetTNHObstacles.Parts[DtPrCnt].ReqH = DetTNHObstacles.Parts[DtPrCnt].Height + TurnMOC;
				DtPrCnt++;
			}

			if (DtPrCnt == 0)
			{
				DetTNHObstacles.Obstacles = new Obstacle[0];
				DetTNHObstacles.Parts = new ObstacleData[0];
			}
			else
			{
				Array.Resize<ObstacleData>(ref DetTNHObstacles.Parts, DtPrCnt);
				Array.Resize<Obstacle>(ref DetTNHObstacles.Obstacles, DtObCnt);
			}

			Functions.Sort(ref DetTNHObstacles, 0);
			//=============================================================================

			CalcTurnRange(ref DetTNHObstacles);
		}

		private Interval CalcOCHRange(ref double OCH, ref double NextOCH, ref int IxMinOCH, ref int IxNextOCH)
		{
			double CurrMOC;

			if (_TurnMoreThan15)
				CurrMOC = GlobalVars.constants.Pansops[ePANSOPSData.arMA_FinalMOC].Value;
			else
				CurrMOC = GlobalVars.constants.Pansops[ePANSOPSData.arMA_InterMOC].Value;

			XptSOC = XptLH - (OCH - m_fMOC) / TanGPA + _zSurfaceOrigin;

			double fMinHDist = XptSOC;
			if (fMinHDist < 10.0)
				fMinHDist = 10.0;

			double L6Sec = GlobalVars.constants.Pansops[ePANSOPSData.arT_TechToleranc].Value * (_TurnTAS + GlobalVars.CurrADHP.WindSpeed) * 0.277777777777778;

			Interval Result = default(Interval);
			Result.Min = fMinHDist;
			Result.Max = GlobalVars.ModellingRadius;

			int n = DetTNHObstacles.Parts.Length;

			for (int i = 0; i < n; i++)
			{
				eOAS lFlag =(eOAS) (DetTNHObstacles.Parts[i].Plane & 0xF);

				if (!DetTNHObstacles.Obstacles[DetTNHObstacles.Parts[i].Owner].IgnoredByUser && (lFlag == eOAS.NonPrec || DetTNHObstacles.Parts[i].hPenet > 0.0) && DetTNHObstacles.Parts[i].ReqOCH > OCH && (DetTNHObstacles.Parts[i].Plane & 16) == 0)
				{
					double TurnDist = DetTNHObstacles.Parts[i].TurnDistL - L6Sec - mahfFix.ATT;
					if (TurnDist > fMinHDist)
					{
						Result.Min = fMinHDist;
						Result.Max = TurnDist;
						NextOCH = DetTNHObstacles.Parts[i].ReqOCH;
						IxNextOCH = i;
						return Result;
					}
					else
					{
						double fTmp = XptLH + _zSurfaceOrigin - TurnDist;
						double fNewOCH = fTmp * TanGPA + m_fMOC;

						if (fNewOCH > DetTNHObstacles.Parts[i].ReqOCH)
						{
							OCH = DetTNHObstacles.Parts[i].ReqOCH;
							XptSOC = XptLH - (OCH - m_fMOC) / TanGPA + _zSurfaceOrigin;
							fMinHDist = XptSOC;			//	XptLH - Math.Max((OCH - fMOC), CurrMOC) / TanGPA + _zSurfaceOrigin; //XptSOC
							IxMinOCH = i;
						}
						else
						{
							OCH = fNewOCH;

							double fTmpDist = (OCH - m_fMOC) / TanGPA - _zSurfaceOrigin;
							XptSOC = XptLH - fTmpDist;
							fMinHDist = XptLH - Math.Max(OCH - m_fMOC, CurrMOC) / TanGPA + _zSurfaceOrigin; //'XptSOC

							NextOCH = DetTNHObstacles.Parts[i].ReqOCH;
							IxNextOCH = i;

							Result.Min = TurnDist;
							Result.Max = TurnDist;
							IxMinOCH = i;

							return Result;
						}
					}
				}
			}

			return Result;
		}

		private Interval[] CalcTurnInterval(ref double OCH, ref double NextOCH, ref Interval OCHRange, ref int IxMinOCH)
		{
			double CurrMOC;

			if (_TurnMoreThan15)
				CurrMOC = GlobalVars.constants.Pansops[ePANSOPSData.arMA_FinalMOC].Value;
			else
				CurrMOC = GlobalVars.constants.Pansops[ePANSOPSData.arMA_InterMOC].Value;

			XptSOC = XptLH - (OCH - m_fMOC) / TanGPA + _zSurfaceOrigin;

			double CoTanGPA = 1.0 / TanGPA;
			double CoTanZ = 1.0 / fMissAprPDG;
			double L6Sec = GlobalVars.constants.Pansops[ePANSOPSData.arT_TechToleranc].Value * (_TurnTAS + GlobalVars.CurrADHP.WindSpeed) * 0.277777777777778 ;
			double fNewOCH = m_fMOC;
			int n = DetTNHObstacles.Parts.Length;

			Interval[] Result = new Interval[1];
			Result[0] = OCHRange;
			Result[0].Tag = 0;

			if (n == 0)
				return Result;

			Interval[,] ObsInterval = new Interval[2, n];

			int i, j = -1;
			int lIxOCH = -1;

			for (i = 0; i < n; i++)
			{
				if ((!DetTNHObstacles.Obstacles[DetTNHObstacles.Parts[i].Owner].IgnoredByUser) && (DetTNHObstacles.Parts[i].Flags & 16) == 0 && DetTNHObstacles.Parts[i].Dist > 0.0)
				{
					double TurnDist = DetTNHObstacles.Parts[i].TurnDistL - L6Sec - mahfFix.ATT;
					if (TurnDist < OCHRange.Max)
					{
						ObsInterval[1, i].Min = TanGPA * (XptLH - TurnDist + _zSurfaceOrigin) + m_fMOC;
						//if(ObsInterval[1, I].Min < 10.0)	ObsInterval[1, I].Min = 10.0;
						ObsInterval[1, i].Tag = i + 1;

						if (DetTNHObstacles.Parts[i].Dist < OCHRange.Max)
							ObsInterval[1, i].Max = 0.0;
						else
							ObsInterval[1, i].Max = (XptLH - DetTNHObstacles.Parts[i].Dist + _zSurfaceOrigin + DetTNHObstacles.Parts[i].Height * CoTanZ) / (CoTanZ + CoTanGPA) + m_fMOC;

						ObsInterval[1, i].Max = Math.Min(ObsInterval[1, i].Max, DetTNHObstacles.Parts[i].ReqOCH);

						//PrevH = DetTNHObstacles(I).Height
						double fTmp = Math.Min(ObsInterval[1, i].Min, ObsInterval[1, i].Max);
						if (fNewOCH < fTmp)
						{
							fNewOCH = fTmp;
							lIxOCH = i;
						}
						j = i;
					}
					else
					{
						ObsInterval[1, i].Tag = -1;
						ObsInterval[0, i].Tag = -1;
					}
				}
				else
				{
					ObsInterval[1, i].Tag = -1;
					ObsInterval[0, i].Tag = -1;
				}
			}

			n = j;

			//ReDim FullInterval(0 To 0)
			//FullInterval(0) = OCHRange
			//FullInterval(0).Tag = 0
			//CalcTurnInterval = FullInterval

			if (n == 0)
				return Result;

			//fNewOCH = 5.0 * Round(0.2 * fNewOCH + 0.4999)

			if (NextOCH > OCH && fNewOCH >= NextOCH)
			{
				OCH = fNewOCH;
				return Result;
			}

			ArrayExt.ResizeArray<Interval>(ref ObsInterval, 2, n);

			if (OCH < fNewOCH)
			{
				OCH = fNewOCH;
				IxMinOCH = lIxOCH;
				double fTmpDist = (OCH - m_fMOC) / TanGPA - _zSurfaceOrigin;
				XptSOC = XptLH - fTmpDist;
				OCHRange.Min = XptLH - Math.Max(OCH - m_fMOC, CurrMOC) / TanGPA + _zSurfaceOrigin;
			}

			for (i = 0; i < n; i++)
			{
				if ((!DetTNHObstacles.Obstacles[DetTNHObstacles.Parts[i].Owner].IgnoredByUser) && ObsInterval[1, i].Tag >= 0)
				{
					double TurnDist = DetTNHObstacles.Parts[i].TurnDistL - L6Sec - mahfFix.ATT;
					if (TurnDist < OCHRange.Max)
					{
						double fTmpDist = XptSOC + (DetTNHObstacles.Parts[i].ReqH - OCH + m_fMOC) * CoTanZ;

						if (fTmpDist < DetTNHObstacles.Parts[i].Dist)
							ObsInterval[0, i].Tag = -1;
						else
						{
							ObsInterval[0, i].Min = TurnDist;
							ObsInterval[0, i].Max = DetTNHObstacles.Parts[i].Dist + CoTanZ;
							ObsInterval[0, i].Tag = i + 1;
						}
					}
				}
			}

			//==================================================================================================

			for (i = 0; i < n; i++)
			{
				if (ObsInterval[0, i].Tag >= 0)
				{
					int m = Result.Length;
					j = 0;
					while (j < m)
					{
						Interval[] tmpResult = Interval.Difference(Result[j], ObsInterval[0, i]);
						int l = tmpResult.Length;

						if (l > 0)
							if (tmpResult[0].Max < Result[j].Min)
								tmpResult[0].Tag = i + 1;

						if (l == 1)
							Result[j] = tmpResult[0];
						//FullInterval[J].Tag = 1
						else if (l > 1)
						{
							Result[j] = tmpResult[0];
							//FullInterval[J].Tag = 1
							Array.Resize<Interval>(ref Result, m + l);

							for (int k = m; k < m + l; k++)
							{
								Result[k] = tmpResult[k - m];
								Result[k].Tag = i + 1;
							}
						}
						else if (l == 0)
						{
							if (m == 1)
							{
								NextOCH = OCH + 0.5;
								OCH = NextOCH + 0.5;
								return tmpResult;
							}
							else
							{
								for (int k = j + 1; k < m; k++)
									Result[k] = tmpResult[k + 1];

								Array.Resize<Interval>(ref Result, m);
								j--;
							}
						}

						m = Result.Length;
						j++;
					}
				}
			}

			return Result;
		}

		private void DrawMAPtSOC(ref double OCH, ref double NextOCH, ref Interval[] TurnIntervals, ref int IxNextOCH)	//, Optional fHTurn As Double
		{
			PtSOC = ARANFunctions.LocalToPrj(PtCoordCntr, ArDir, XptSOC);
			PtSOC.Z = OCH - m_fMOC;
			PtSOC.M = ArDir;

			double fTmp = (OCH - GP_RDH) / TanGPA;
			pMAPt = ARANFunctions.LocalToPrj(FicTHRprj, ArDir, -fTmp);
			pMAPt.Z = OCH; //PtSOC.z
			pMAPt.M = ArDir;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(SOCElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(pMAPtElem);

			SOCElem = GlobalVars.gAranGraphics.DrawPointWithText(PtSOC, "SOC", GlobalVars.WPTColor);
			pMAPtElem = GlobalVars.gAranGraphics.DrawPointWithText(pMAPt, "MAPt", GlobalVars.WPTColor);

			if (!double.TryParse(TextBox0310.Text, out fTmp))
				TextBox0310.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(XptLH - TurnIntervals[0].Min).ToString();
			//======================================================================

			if (IxNextOCH > -1)
			{
				TextBox0307.Text = GlobalVars.unitConverter.HeightToDisplayUnits(NextOCH, eRoundMode.NEAREST).ToString();
				TextBox0304.Text = DetTNHObstacles.Obstacles[DetTNHObstacles.Parts[IxNextOCH].Owner].UnicalName;
			}
			else
			{
				TextBox0307.Text = "-";
				TextBox0304.Text = "-";
			}

			int n = TurnIntervals.Length;

			if (n == 0)
			{
				MessageBox.Show("Solution does not exist.");	//"??????????????????????????????"
				return;
			}

			fTNH = (TurnIntervals[0].Min - XptSOC) * fMissAprPDG + fMisAprOCH - m_fMOC;
			if (fTNH < fMisAprOCH)
				fTNH = fMisAprOCH;

			ComboBox0304.Items.Clear();

			n = InSectList.Length;
			//for (int i = 0; i < n; i++)
			{

			}

			TextBox0310_Validating(TextBox0310, null);
			reportForm.FillPage06(DetTNHObstacles, 1);
		}

		private void ComboBox0301_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!FormInitialised)
				return;

			TurnDir = 1 - 2 * ComboBox0301.SelectedIndex;
		}

		private void ComboBox0302_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ComboBox0302.SelectedIndex < 0)
			{
				ComboBox0302.SelectedIndex = 0;
				return;
			}

			if (ComboBox0302.SelectedIndex == 0)
				TextBox0305.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fMisAprOCH + FicTHRprj.Z, eRoundMode.NEAREST).ToString();
			else
				TextBox0305.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fMisAprOCH, eRoundMode.NEAREST).ToString();
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

			_BankAngle = fTmp;

			if (_BankAngle < 15.0)
				_BankAngle = 15.0;
			if (_BankAngle > 25.0)
				_BankAngle = 25.0;

			if (fTmp != _BankAngle)
				TextBox0301.Text = _BankAngle.ToString("0.0");

			_BankAngle = ARANMath.DegToRad(_BankAngle);
			TextBox0301.Tag = TextBox0301.Text;
		}

		private void TextBox0302_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0302_Validating(TextBox0302, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0302.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0302_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;
			if (!double.TryParse(TextBox0302.Text, out fTmp))
				return;

			if (TextBox0302.Tag != null && TextBox0302.Tag.ToString() == TextBox0302.Text)
				return;

			_TurnIAS = fTmp = GlobalVars.unitConverter.SpeedToInternalUnits(fTmp);

			if (_TurnIAS < GlobalVars.constants.AircraftCategory[aircraftCategoryData.VmaInter].Value[Category])
				_TurnIAS = GlobalVars.constants.AircraftCategory[aircraftCategoryData.VmaInter].Value[Category];

			if (_TurnIAS > GlobalVars.constants.AircraftCategory[aircraftCategoryData.VmaFaf].Value[Category])
				_TurnIAS = GlobalVars.constants.AircraftCategory[aircraftCategoryData.VmaFaf].Value[Category];

			if (fTmp != _TurnIAS)
				TextBox0302.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(_TurnIAS, eRoundMode.NEAREST).ToString();

			mahfFix.IAS = _TurnIAS;

			//_TurnTAS = ARANMath.IASToTASForRnav(_TurnIAS, _hIF, GlobalVars.CurrADHP.ISAtC - GlobalVars.constants.Pansops[ePANSOPSData.arISAmax].Value);
			//_TurnTurnR = ARANMath.BankToRadius(ARANMath.DegToRad(25), _TurnTAS);

			TextBox0302.Tag = TextBox0302.Text;
		}

		private void TextBox0305_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0305_Validating(TextBox0305, null);
			else
				Functions.TextBoxFloat(ref eventChar, TextBox0305.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0305_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;
			if (!double.TryParse(TextBox0305.Text, out fTmp))
				return;

			if (TextBox0305.Tag != null && TextBox0305.Tag.ToString() == TextBox0305.Text)
				return;

			fMisAprOCH = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);

			if (ComboBox0302.SelectedIndex == 0)
				fMisAprOCH = fMisAprOCH - FicTHRprj.Z;

			//fTmp = fMisAprOCH;

			double fMinOCH;

			if (IxOCH > -1)
			{
				if (ARANMath.SideDef(PtCoordCntr, ArDir + ARANMath.C_PI_2, WorkObstacleList.Parts[IxOCH].pPtPrj) == SideDirection.sideLeft)
					fMinOCH = _CurrFAPOCH;
				else
					fMinOCH = m_fMOC > fRDHOCH ? m_fMOC : fRDHOCH;
			}
			else
				fMinOCH = m_fMOC > fRDHOCH ? m_fMOC : fRDHOCH;

			if (fMisAprOCH < fMinOCH) fMisAprOCH = fMinOCH;

			m_IxMinOCH = IxMaxOCH = -1;
			double CurrOCH, NextOCH = fMisAprOCH;

			do
			{
				CurrOCH = NextOCH;
				Interval OCHInterv = CalcOCHRange(ref CurrOCH, ref NextOCH, ref m_IxMinOCH, ref IxMaxOCH);
				m_TurnIntervals = CalcTurnInterval(ref CurrOCH, ref NextOCH, ref OCHInterv, ref m_IxMinOCH);
			} while (CurrOCH > NextOCH && IxMaxOCH >= 0);

			fMisAprOCH = CurrOCH;
			ComboBox0302_SelectedIndexChanged(ComboBox0302, null);

			//TextBox0505.Text = GlobalVars.unitConverter.HeightToDisplayUnits(CurrOCH, eRoundMode.NERAEST).ToString();

			DrawMAPtSOC(ref  fMisAprOCH, ref NextOCH, ref m_TurnIntervals, ref IxMaxOCH);	//	, fHTurn

			//======================================================================

			//int N = ArrivalProfile.MAPtIndex - 1;
			//While ArrivalProfile.PointsNo > N
			//	ArrivalProfile.RemovePoint()
			//End While

			////fMAPtDist = FAPDist - (hFAP - fMisAprOCH) / Tan(DegToRad(GPAngle))
			//fMAPtDist = (fMisAprOCH - GP_RDH) / System.Math.Tan(DegToRad(GPAngle))
			//ArrivalProfile.AddPoint(fMAPtDist, fMisAprOCH, ILS.Course - ILS.MagVar, RadToDeg(System.Math.Atan(fMissAprPDG)), CodeProcedureDistance.MAP)

			////================================================================
			//d0 = Point2LineDistancePrj(ptLHPrj, TurnFixPnt, ArDir + 90.0) * SideDef(ptLHPrj, ArDir - 90.0, TurnFixPnt)
			//ArrivalProfile.AddPoint(d0, TurnFixPnt.Z - ptLHPrj.Z, ILS.Course - ILS.MagVar, RadToDeg(System.Math.Atan(fMissAprPDG)), -1, 1)
			//======================================================================

			//TextBox0503.Text = CStr(ConvertHeight(fMisAprOCH, eRoundMode.rmNERAEST))

			TextBox0310.Tag = "";
			TextBox0310_Validating(TextBox0310, null);
		}

		private void TextBox0309_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0309_Validating(TextBox0309, null);
			else
				Functions.TextBoxFloatWithSign(ref eventChar, TextBox0309.Text);

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

			_PlannedTurnAtMATF = fTmp;

			if (_PlannedTurnAtMATF < 0.0)
				_PlannedTurnAtMATF = 0.0;

			if (_PlannedTurnAtMATF > 90.0)
				_PlannedTurnAtMATF = 90.0;

			bool moreThan15 = _PlannedTurnAtMATF > 15;

			if (fTmp != _PlannedTurnAtMATF)
				TextBox0309.Text = _PlannedTurnAtIF.ToString("0.0");

			TextBox0309.Tag = TextBox0309.Text;

			_PlannedTurnAtMATF = ARANMath.DegToRad(_PlannedTurnAtMATF);

			if (_TurnMoreThan15 != moreThan15)
			{
				CalcTNHIntervals();
				Transition1();
				_TurnMoreThan15 = moreThan15;
			}
		}

		private void TextBox0310_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox0310_Validating(TextBox0310, null);
			else
				Functions.TextBoxFloatWithSign(ref eventChar, TextBox0310.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox0310_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;
			if (!double.TryParse(TextBox0310.Text, out fTmp))
				return;

			if (TextBox0310.Tag != null && TextBox0310.Tag.ToString() == TextBox0310.Text)
				return;

			double oldFixDist = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);
			double FixDist = XptLH - oldFixDist;

			//==================== 
			int i, n = m_TurnIntervals.Length;
			if (n == 0)
				return;

			double InRange = FixDist;
			bool bInRange = false;

			if (FixDist > m_TurnIntervals[n - 1].Max)
				InRange = m_TurnIntervals[n - 1].Max;
			else if (FixDist < m_TurnIntervals[0].Min)
				InRange = m_TurnIntervals[0].Min;
			else
			{
				double minDx = GlobalVars.ModellingRadius;

				for (i = 0; i < n; i++)
				{
					bInRange = (FixDist >= m_TurnIntervals[i].Min && FixDist <= m_TurnIntervals[i].Max);
					if (bInRange)
						break;

					double dX = System.Math.Abs(FixDist - m_TurnIntervals[i].Min);
					if (dX < minDx)
					{
						InRange = m_TurnIntervals[i].Min;
						minDx = dX;
					}

					dX = System.Math.Abs(FixDist - m_TurnIntervals[i].Max);
					if (dX < minDx)
					{
						InRange = m_TurnIntervals[i].Max;
						minDx = dX;
					}
				}
			}

			if (!bInRange)
				FixDist = InRange;

			FixDist = XptLH - FixDist;

			//============================================================
			fHTurn = (XptLH - FixDist - XptSOC) * fMissAprPDG + fMisAprOCH - m_fMOC;

			double L0 = (fHTurn - PtSOC.Z) / fMissAprPDG;

			hTurn = fHTurn + FicTHRprj.Z;

			TurnFixPnt = ARANFunctions.LocalToPrj(PtSOC, ArDir, L0);
			TurnFixPnt.Z = hTurn;
			TurnFixPnt.M = ArDir;

			_TurnTAS = ARANMath.IASToTASForRnav(_TurnIAS, hTurn, GlobalVars.CurrADHP.ISAtC - GlobalVars.constants.Pansops[ePANSOPSData.arISAmax].Value);
			_TurnTurnR = ARANMath.BankToRadius(_BankAngle, _TurnTAS);

			//fTurnDist = FixDist

			TextBox0310.Tag = GlobalVars.unitConverter.DistanceToDisplayUnits(FixDist, eRoundMode.NEAREST).ToString();

			if (FixDist != oldFixDist)
				TextBox0310.Text = TextBox0310.Tag.ToString();

			mahfFix.NomLineAltitude = hTurn;
			mahfFix.PrjPt = TurnFixPnt;
			mahfFix.DeleteGraphics();
			mahfFix.RefreshGraphics();

			//While ArrivalProfile.PointsNo > ArrivalProfile.MAPtIndex + 1
			//	ArrivalProfile.RemovePoint()
			//End While

			//Dim dD As Double
			//dD = Point2LineDistancePrj(ptLHPrj, TurnFixPnt, ArDir + 90.0) * SideDef(ptLHPrj, ArDir - 90.0, TurnFixPnt)
			//ArrivalProfile.AddPoint(dD, TurnFixPnt.Z - ptLHPrj.Z, ILS.Course - ILS.MagVar, RadToDeg(System.Math.Atan(fMissAprPDG)), -1, 1)
		}

		#endregion

		#region Page V

		private bool preparePageV()
		{
			WPT_FIXType fix = new WPT_FIXType();

			//protected 
			WayPoint WPT_FAP;

			fix.Name = "FAP";
			fix.pPtPrj = _ptFAPprj;
			fix.pPtGeo = _ptFAPgeo;

			WPT_FAP = new WayPoint(eFIXRole.DEP_ST, fix, GlobalVars.gAranEnv);

			WPT_FAP.FlyMode = eFlyMode.Flyby;
			WPT_FAP.FlightPhase = eFlightPhase.FAFApch;

			WPT_FAP.IAS = _TurnIAS;
			WPT_FAP.ISAtC = GlobalVars.CurrADHP.ISAtC;

			WPT_FAP.EntryDirection = ArDir;
			WPT_FAP.OutDirection = ArDir;

			//========================================================
			//MAPt maptFix = new MAPt(eFIXRole.MAHF_LE_56, GlobalVars.gAranEnv);
			//maptFix.SOCDistance = _MAPt2THRDist - _SOC2THRDist;
			//maptFix.SensorType = eSensorType.GNSS;
			//maptFix.PBNType = ePBNClass.RNP_APCH;
			//maptFix.Role = eFIXRole.MAHF_LE_56;
			//maptFix.ISAtC = GlobalVars.CurrADHP.ISAtC;
			//maptFix.EntryDirection = ArDir;
			//maptFix.OutDirection = ArDir;
			//========================================================

			mahfFix.FlightPhase = eFlightPhase.MApLT28;
			mahfFix.FlyMode = eFlyMode.Flyover;		//??????????
			mahfFix.IAS = _TurnIAS;

			LegApch prevLeg = new LegApch(WPT_FAP, mahfFix, GlobalVars.gAranEnv);
			prevLeg.Altitude = _hFAP;
			prevLeg.CreateGeometry(null, GlobalVars.CurrADHP);

			GeometryOperators geoOp = new GeometryOperators();

			Point ptSt = ARANFunctions.LocalToPrj(_ptFAPprj, ArDir, 0, 20000.0);
			Point ptEnd = ARANFunctions.LocalToPrj(_ptFAPprj, ArDir, 0, -20000.0);

			LineString ls = new LineString();
			ls.Add(ptSt);
			ls.Add(ptEnd);

			Geometry outGeomLeft, outGeomRight;

			geoOp.Cut((Geometry)pFullPoly, ls, out outGeomLeft, out outGeomRight);

			if (mahfFix.FlyMode == eFlyMode.Flyover)
			{
				double dist = mahfFix.LPT + (mahfFix.ConstructTAS + GlobalVars.CurrADHP.WindSpeed)* (mahfFix.PilotTime + mahfFix.BankTime);

				ptSt = ARANFunctions.LocalToPrj(mahfFix.PrjPt, ArDir, dist, 20000.0);
				ptEnd = ARANFunctions.LocalToPrj(mahfFix.PrjPt, ArDir, dist, -20000.0);
			}
			else
			{
				double dist = mahfFix.LPT + (mahfFix.ConstructTAS + GlobalVars.CurrADHP.WindSpeed) * (mahfFix.PilotTime + mahfFix.BankTime);

				ptSt = ARANFunctions.LocalToPrj(mahfFix.PrjPt, ArDir, dist, 20000.0);
				ptEnd = ARANFunctions.LocalToPrj(mahfFix.PrjPt, ArDir, dist, -20000.0);
			}

			ls.Clear() ;
			ls.Add(ptSt);
			ls.Add(ptEnd);
			MultiPolygon pPoly1 = (MultiPolygon)outGeomLeft;

			geoOp.Cut((Geometry)pPoly1, ls, out outGeomLeft, out outGeomRight);
			MultiPolygon pPoly2 = (MultiPolygon)outGeomRight;

			prevLeg.FullArea = prevLeg.PrimaryArea = pPoly2;

			//if (comboBox205.SelectedIndex == 0)
			//	prevLeg.EndFIX.Name = textBox214.Text;
			//else
			//{
			//	WPT_FIXType SigPoint = (WPT_FIXType)comboBox205.SelectedItem;
			//	prevLeg.EndFIX.Name = SigPoint.Name;
			//	prevLeg.EndFIX.ID = SigPoint.Identifier.ToString();
			//}
			//prevLeg.Obstacles = segment1Term.InnerObstacleList;
			//prevLeg.DetObstacle = segment1Term.DetObs;

			Point ptTmp = ARANFunctions.LocalToPrj(prevLeg.EndFIX.PrjPt, prevLeg.EndFIX.EntryDirection, -prevLeg.EndFIX.EPT, 0);
			prevLeg.MinLegLength = 0.0;	// ARANFunctions.ReturnDistanceInMeters(prevLeg.StartFIX.PrjPt, ptTmp);

			//for (int i = 0; i < leg.Obstacles.Length; i++)
			//{
			//	leg.Obstacles[i].IsExcluded = leg.Obstacles[i].Ignored;
			//	leg.Obstacles[i].DistStar = leg.Obstacles[i].d0 = leg.Obstacles[i].X;
			//}

			ObstacleContainer prevLegObstacleList;
			ObstacleData prevLegDetObs = new ObstacleData();

			Functions.GetObstaclesByPolygon(MAObstacleList, out prevLegObstacleList, pPoly2);
			//int inx = Functions.GetLegAreaObstacles(GlobalVars.ObstacleList, out prevLegObstacleList, prevLeg, 0.0, FicTHRprj.Z, 0.0);
			prevLeg.Obstacles = prevLegObstacleList;
			prevLeg.DetObstacle_2 = prevLegDetObs;

			reportForm.FillPage07(prevLeg.Obstacles, prevLeg.DetObstacle_2);
			reportForm.AddPage08(prevLeg.Obstacles, prevLeg.DetObstacle_2);
			_haveReport = true;

			//ptMAHF.Z = PtSOC.Z + (MAHFDistance - _SOC2THRDist) * fMissAprPDG;

			//while (true)
			//	Application.DoEvents();
			//=====================================================================================================
			//comboBox409.SelectedIndex = comboBox206.SelectedIndex;
			//textBox407.Text = textBox205.Text;

			UpDown401Range.Circular = true;
			UpDown401Range.InDegree = true;
			radioButton401.Checked = true;
			radioButton403.Checked = true;

			//segment1Term.Clean();
			//segment1Term.TermWPT.IAS = _IAS;

			//if (_terminationType == TerminationType.AtHeight)
			//	segment1Term.TermWPT.TurnAltitude = segment1Term.TerminationAltitude;
			//else
			//	segment1Term.TermWPT.TurnAltitude = segment1Term.TurnAltitude;

			_SignificantPoints = new List<WayPoint>();
			for (int i = 0; i < GlobalVars.WPTList.Length; i++)
				_SignificantPoints.Add(new WayPoint(eFIXRole.TP_, GlobalVars.WPTList[i], GlobalVars.gAranEnv));

			transitions = new Transitions(_SignificantPoints, GlobalVars.CurrADHP, SelectedRWY, PtSOC, prevLeg, fMissAprPDG, _PlannedTurnAtMATF, 300000.0, (aircraftCategory)Category, GlobalVars.gAranEnv, InitNewPoint);
			transitions.OnFIXUpdated = FIXUpdated;
			transitions.OnDistanceChanged = OnLegDistanceChanged;
			transitions.OnAltitudeChanged = OnLegAltitudeChanged;
			//transitions.plannedTurnAngle = ARANMath.DegToRadValue();
			//transitions.OnDirectionChanged = OnLegDirectionChanged;
			transitions.BankAngle = _BankAngle;

			transitions.OnUpdateDirList = OnUpdateDirList;
			transitions.OnUpdateDistList = OnUpdateDistList;
			transitions.MOCLimit = GlobalVars.EnrouteMOCValues[comboBox408.SelectedIndex];

			UpDown401Range.Max = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Min, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			UpDown401Range.Min = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Max, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			transitions.Gradient = GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;

			comboBox406.Items.Clear();
			foreach (ePBNClass pbn in firstTurnPBNClass)
				comboBox406.Items.Add(pbn);

			comboBox407.Items.Clear();

			eFlyMode FlyMode = (eFlyMode)ComboBox0303.SelectedIndex;

			if (FlyMode == eFlyMode.Flyover)
				foreach (CodeSegmentPath pt in FlyOverPathAndTerminations)
					comboBox407.Items.Add(pt);
			else			//if (FlyMode == eFlyMode.FlyBy)
				foreach (CodeSegmentPath pt in FlyByPathAndTerminations)
					comboBox407.Items.Add(pt);

			comboBox403.SelectedIndex = 0;
			comboBox404.SelectedIndex = 0;
			comboBox405.SelectedIndex = 0;
			comboBox406.SelectedIndex = 0;

			comboBox407.SelectedIndex = 0;
			numericUpDown402.Value = (decimal)ARANMath.RadToDeg(transitions.plannedTurnAngle);

			textBox406.Text = transitions.FIXName;
			textBox402.Text = TextBox0302.Text;
			textBox402.Tag = null;

			double nnAzt = ARANFunctions.DirToAzimuth(transitions.ReferenceFIX.PrjPt, transitions.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			numericUpDown401.Value = (decimal)nnAzt;

			transitions.UpdateEnabled = true;

			//numericUpDown302.Value = (decimal)ARANMath.RadToDeg(transitions.plannedTurnAngle);

			textBox401.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(transitions.Distance, eRoundMode.NEAREST).ToString();

			double TAS = ARANMath.IASToTAS(transitions.IAS, transitions.Altitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);
			textBox403.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS, eRoundMode.SPECIAL_NEAREST).ToString();

			textBox404.Text = GlobalVars.unitConverter.HeightToDisplayUnits(transitions.Altitude, eRoundMode.NEAREST).ToString();
			textBox405.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.Gradient, eRoundMode.NEAREST).ToString();

			//textBox407.Text = Math.Round(ARANMath.RadToDeg(transitions.BankAngle)).ToString();//= ARANMath.DegToRad(bankAngle);
			if (ARANMath.RadToDeg(_BankAngle) >= 21.0)
				comboBox409.SelectedIndex = 2;
			else if (ARANMath.RadToDeg(_BankAngle) <= 19.0)
				comboBox409.SelectedIndex = 0;
			else
				comboBox409.SelectedIndex = 1;

			return true;
		}

		private void leavePageV()
		{
			if (transitions != null)
				transitions.Clean();
		}

		#region transitions events

		void InitNewPoint(object sender, WayPoint ReferenceFIX, WayPoint AddedFIX)
		{
			ReferenceFIX.ISAtC = GlobalVars.CurrADHP.ISAtC;
			AddedFIX.ISAtC = GlobalVars.CurrADHP.ISAtC;

			Transitions trans = (Transitions)sender;

			_AddedFIX = AddedFIX;

			UpDown401Range.Max = ARANFunctions.DirToAzimuth(trans.CourseOutConvertionPoint, trans.OutDirRange.Min, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			UpDown401Range.Min = ARANFunctions.DirToAzimuth(trans.CourseOutConvertionPoint, trans.OutDirRange.Max, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			textBox406.Text = trans.FIXName;
		}

		void FIXUpdated(object sender, WayPoint CurrFIX)
		{
			CurrFIX.ISAtC = GlobalVars.CurrADHP.ISAtC;

			reportForm.FillPage07(transitions.CurrentObstacleList, transitions.CurrentDetObs);
			_haveReport = true;

			if (ReportButton.Checked && !ReportButton.Visible)
				reportForm.Show(GlobalVars.Win32Window);
		}

		void OnLegAltitudeChanged(object sender, double value)
		{
			textBox404.Text = GlobalVars.unitConverter.HeightToDisplayUnits(value, eRoundMode.NEAREST).ToString();
			textBox404.Tag = textBox404.Text;
			textBox405.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.Gradient, eRoundMode.NEAREST).ToString();
		}

		void OnLegDistanceChanged(object sender, double value)
		{
			textBox401.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(value, eRoundMode.NEAREST).ToString();
			textBox401.Tag = textBox401.Text;
			textBox405.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.Gradient, eRoundMode.NEAREST).ToString();
		}

		void OnUpdateDirList(object sender, EventArgs e)
		{
			//if (comboBox302.SelectedIndex >= 0)
			//    OldWpt = (WayPoint)comboBox302.SelectedItem;

			if (transitions == null)
				return;

			transitions.turnDirection = comboBox403.SelectedIndex == 0 ? TurnDirection.CCW : TurnDirection.CW;

			WayPoint OldWpt = null;
			if (comboBox401.SelectedIndex >= 0)
				OldWpt = (WayPoint)comboBox401.Items[comboBox401.SelectedIndex];

			int wptIndex = -1;
			comboBox401.Items.Clear();

			foreach (WayPoint wpt in transitions.CourseSgfPoint)
			{
				if (wpt.Equals(OldWpt))
					wptIndex = comboBox401.Items.Count;
				try
				{
					comboBox401.Items.Add(wpt);
				}
				catch
				{
					break;
				}
			}

			radioButton402.Enabled = comboBox401.Items.Count > 0;
			if (radioButton402.Enabled)
			{
				if (wptIndex >= 0)
					comboBox401.SelectedIndex = wptIndex;
				else
					comboBox401.SelectedIndex = 0;
			}
			else
				radioButton401.Checked = true;

			//if (radioButton401.Checked)
			//	numericUpDown401_ValueChanged(numericUpDown401, new EventArgs());
		}

		void OnUpdateDistList(object sender, EventArgs e)
		{
			if (transitions == null)
				return;

			WayPoint OldWpt = null;
			int OldIndex = -1;

			if (comboBox402.SelectedIndex >= 0)
				OldWpt = (WayPoint)comboBox402.SelectedItem;

			comboBox402.Items.Clear();

			foreach (WayPoint wpt in transitions.DistanceSgfPoint)
			{
				if (OldWpt != null && wpt.Equals(OldWpt))
					OldIndex = comboBox402.Items.Count;
				comboBox402.Items.Add(wpt);
			}

			radioButton404.Enabled = comboBox402.Items.Count > 0;
			if (radioButton404.Enabled)
			{
				if (OldIndex >= 0)
					comboBox402.SelectedIndex = OldIndex;
				else
					comboBox402.SelectedIndex = 0;
			}
			else
				radioButton403.Checked = true;
		}

		#endregion

		private void radioButton401_CheckedChanged(object sender, EventArgs e)
		{
			if (transitions != null)
				transitions.DirectionIndex = -1;

			numericUpDown401.ReadOnly = !radioButton401.Checked;
			comboBox401.Enabled = !radioButton401.Checked;

			if (!radioButton401.Checked)
				comboBox401_SelectedIndexChanged(comboBox401, new EventArgs());

		}

		private bool PreventRecurse401 = false;
		private void numericUpDown401_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				if (numericUpDown401.Value == numericUpDown401.Minimum)
					numericUpDown401.Value = numericUpDown401.Maximum;
				else if (numericUpDown401.Value == numericUpDown401.Maximum)
					numericUpDown401.Value = numericUpDown401.Minimum;
			}
		}

		private void numericUpDown401_ValueChanged(object sender, EventArgs e)
		{
			if (!PreventRecurse401)
			{
				double dTmp = (double)numericUpDown401.Value;

				if (dTmp <= (double)numericUpDown401.Minimum)
					dTmp = (double)numericUpDown401.Maximum - 1;
				else if (dTmp >= (double)numericUpDown401.Maximum)
					dTmp = (double)numericUpDown401.Minimum + 1;

				double OldVal = dTmp;

				UpDown401Range.Max = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Min, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
				UpDown401Range.Min = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Max, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

				double newValue = UpDown401Range.CheckValue(dTmp);

				PreventRecurse401 = OldVal != newValue;
				numericUpDown401.Value = (decimal)newValue;
				if (PreventRecurse401)
					return;
			}
			else
				PreventRecurse401 = false;

			if (radioButton402.Checked)
				return;

			double newAzt = (double)numericUpDown401.Value;
			double Direction;

			if (_AddedFIX != null && _AddedFIX.GeoPt != null)
				Direction = ARANFunctions.AztToDirection(_AddedFIX.GeoPt, newAzt, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);
			else
				Direction = ARANFunctions.AztToDirection(FicTHRgeo, newAzt, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);

			//double _Distance = transitions.Distance;
			transitions.OutDirection = Direction;

			//if (transitions.Distance != _Distance)
			textBox401.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(transitions.Distance, eRoundMode.NEAREST).ToString();
			textBox401.Tag = textBox401.Text;

			double TAS = ARANMath.IASToTAS(transitions.IAS, transitions.Altitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);
			textBox403.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS, eRoundMode.SPECIAL_NEAREST).ToString();

			textBox404.Text = GlobalVars.unitConverter.HeightToDisplayUnits(transitions.Altitude, eRoundMode.NEAREST).ToString();
			textBox404.Tag = textBox404.Text;

			textBox405.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.Gradient, eRoundMode.NEAREST).ToString();

			if (ARANMath.RadToDeg(transitions.BankAngle) >= 21.0)
				comboBox409.SelectedIndex = 2;
			else if (ARANMath.RadToDeg(transitions.BankAngle) <= 19.0)
				comboBox409.SelectedIndex = 0;
			else
				comboBox409.SelectedIndex = 1;
		}

		private void comboBox401_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!comboBox401.Enabled)
				return;

			if (radioButton401.Checked)
				return;

			int k = comboBox401.SelectedIndex;
			if (k < 0)
				return;

			transitions.DirectionIndex = k;

			WayPoint sigPoint = (WayPoint)comboBox401.SelectedItem;

			label402.Text = sigPoint.Role.ToString();// AIXMTypeNames[sigPoint.AIXMType];
			double Direction;
			double newAzt;

			if (transitions.PathAndTermination == CodeSegmentPath.DF)
			{
				double fTurnDirection = -(int)(_AddedFIX.TurnDirection);
				double r = _AddedFIX.ConstructTurnRadius;
				Point ptCenter = ARANFunctions.LocalToPrj(_AddedFIX.PrjPt, _AddedFIX.EntryDirection, 0, -r * fTurnDirection);

				double dX = sigPoint.PrjPt.X - ptCenter.X;
				double dY = sigPoint.PrjPt.Y - ptCenter.Y;

				double distDest = ARANMath.Hypot(dX, dY);
				double dirDest = Math.Atan2(dY, dX);

				double TurnAngle = (_AddedFIX.EntryDirection - dirDest) * fTurnDirection + ARANMath.C_PI_2 - Math.Acos(r / distDest);
				Direction = ARANMath.Modulus(_AddedFIX.EntryDirection - TurnAngle * fTurnDirection, ARANMath.C_2xPI);
			}
			else
				Direction = Math.Atan2(sigPoint.PrjPt.Y - _AddedFIX.PrjPt.Y, sigPoint.PrjPt.X - _AddedFIX.PrjPt.X);

			if (_AddedFIX.PrjPt != null)
				newAzt = ARANFunctions.DirToAzimuth(_AddedFIX.PrjPt, Direction, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			else
				newAzt = ARANFunctions.DirToAzimuth(FicTHRprj, Direction, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			numericUpDown401.Value = (decimal)newAzt;
			newAzt = (double)numericUpDown401.Value;

			if (_AddedFIX != null && _AddedFIX.GeoPt != null)
				Direction = ARANFunctions.AztToDirection(_AddedFIX.GeoPt, newAzt, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);
			else
				Direction = ARANFunctions.AztToDirection(FicTHRgeo, newAzt, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);

			//double _Distance = transitions.Distance;

			transitions.OutDirection = Direction;

			//if (transitions.Distance != _Distance)
			textBox401.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(transitions.Distance, eRoundMode.NEAREST).ToString();
			textBox401.Tag = textBox401.Text;

			double TAS = ARANMath.IASToTAS(transitions.IAS, transitions.Altitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);
			textBox403.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS, eRoundMode.SPECIAL_NEAREST).ToString();

			textBox404.Text = GlobalVars.unitConverter.HeightToDisplayUnits(transitions.Altitude, eRoundMode.NEAREST).ToString();
			textBox404.Tag = textBox404.Text;

			textBox405.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.Gradient, eRoundMode.NEAREST).ToString();

			if (ARANMath.RadToDeg(transitions.BankAngle) >= 21.0)
				comboBox409.SelectedIndex = 2;
			else if (ARANMath.RadToDeg(transitions.BankAngle) <= 19.0)
				comboBox409.SelectedIndex = 0;
			else
				comboBox409.SelectedIndex = 1;
		}

		private void comboBox403_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (transitions == null)
				return;

			double fTmp = transitions.OutDirection;
			//transitions.OutDirection = Direction;

			transitions.turnDirection = comboBox403.SelectedIndex == 0 ? TurnDirection.CCW : TurnDirection.CW;
			if (fTmp != transitions.OutDirection)
			{
				double newAzt;
				if (_AddedFIX != null && _AddedFIX.PrjPt != null)
					newAzt = ARANFunctions.DirToAzimuth(_AddedFIX.PrjPt, transitions.OutDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
				else
					newAzt = ARANFunctions.DirToAzimuth(FicTHRprj, transitions.OutDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

				numericUpDown401.Value = (decimal)newAzt;
			}
			//if (radioButton401.Checked)	numericUpDown401_ValueChanged(numericUpDown401, new EventArgs());
		}

		private void textBox401_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox401_Validating(textBox401, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox401.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox401_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox401.Text, out fTmp))
				return;

			if (textBox401.Tag != null && textBox401.Tag.ToString() == textBox401.Text)
				return;

			double _Distance = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

			transitions.Distance = _Distance;

			//if (transitions.Distance != _Distance)
			textBox401.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(transitions.Distance, eRoundMode.NEAREST).ToString();
			textBox401.Tag = textBox401.Text;

			textBox404.Text = GlobalVars.unitConverter.HeightToDisplayUnits(transitions.Altitude, eRoundMode.NEAREST).ToString();
			textBox404.Tag = textBox404.Text;

			textBox405.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.Gradient, eRoundMode.NEAREST).ToString();

			if (ARANMath.RadToDeg(transitions.BankAngle) >= 21.0)
				comboBox409.SelectedIndex = 2;
			else if (ARANMath.RadToDeg(transitions.BankAngle) <= 19.0)
				comboBox409.SelectedIndex = 0;
			else
				comboBox409.SelectedIndex = 1;
		}

		private void comboBox402_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!comboBox402.Enabled)
				return;

			int k = comboBox402.SelectedIndex;
			if (k < 0)
				return;

			transitions.DistanceIndex = k;
			textBox406.Text = transitions.FIXName;

			WayPoint sigPoint = (WayPoint)comboBox402.SelectedItem;
			label404.Text = sigPoint.Role.ToString();

			if (radioButton403.Checked)
				return;

			double _Distance;

			if (transitions.PathAndTermination == CodeSegmentPath.DF)
			{
				double fTurnDirection = (int)(_AddedFIX.EffectiveTurnDirection);
				double r = _AddedFIX.ConstructTurnRadius;
				Point ptCenter = ARANFunctions.LocalToPrj(_AddedFIX.PrjPt, _AddedFIX.EntryDirection, 0, r * fTurnDirection);
				Point ptFrom = ARANFunctions.LocalToPrj(ptCenter, _AddedFIX.OutDirection, 0.0, -r * fTurnDirection);

				_Distance = ARANMath.Hypot(sigPoint.PrjPt.Y - ptFrom.Y, sigPoint.PrjPt.X - ptFrom.X);
			}
			else
				_Distance = ARANMath.Hypot(sigPoint.PrjPt.Y - _AddedFIX.PrjPt.Y, sigPoint.PrjPt.X - _AddedFIX.PrjPt.X);

			transitions.Distance = _Distance;

			textBox401.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(transitions.Distance, eRoundMode.NEAREST).ToString();
			textBox401.Tag = textBox401.Text;
		}

		private void textBox406_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
			{
				textBox406_Validating(textBox406, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		private void textBox406_Validating(object sender, CancelEventArgs e)
		{
			textBox406.Text = textBox406.Text.ToUpper();
			transitions.FIXName = textBox406.Text;
			textBox406.Tag = textBox406.Text;
		}

		private void textBox402_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox402_Validating(textBox402, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox402.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox402_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox402.Text, out fTmp))
				return;

			if (textBox402.Tag != null && textBox402.Tag.ToString() == textBox402.Text)
				return;

			double fIAS = GlobalVars.unitConverter.SpeedToInternalUnits(fTmp);

			transitions.IAS = fIAS;

			if (transitions.IAS != fIAS)
				textBox402.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(transitions.IAS, eRoundMode.NEAREST).ToString();

			double TAS = ARANMath.IASToTAS(transitions.IAS, transitions.Altitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);
			textBox403.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS, eRoundMode.NEAREST).ToString();

			textBox402.Tag = textBox402.Text;
		}

		private void textBox404_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox404_Validating(textBox404, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox404.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox404_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox404.Text, out fTmp))
				return;

			if (textBox404.Tag != null && textBox404.Tag.ToString() == textBox404.Text)
				return;

			double _Altitude = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);
			transitions.Altitude = _Altitude;

			//if (transitions.Altitude != _Altitude)
			textBox404.Text = GlobalVars.unitConverter.HeightToDisplayUnits(transitions.Altitude, eRoundMode.NEAREST).ToString();
			textBox404.Tag = textBox404.Text;

			double TAS = ARANMath.IASToTAS(transitions.IAS, transitions.Altitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);
			textBox403.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS, eRoundMode.SPECIAL_NEAREST).ToString();
			textBox405.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.Gradient, eRoundMode.NEAREST).ToString();

			if (ARANMath.RadToDeg(transitions.BankAngle) >= 21.0)
				comboBox409.SelectedIndex = 2;
			else if (ARANMath.RadToDeg(transitions.BankAngle) <= 19.0)
				comboBox409.SelectedIndex = 0;
			else
				comboBox409.SelectedIndex = 1;
		}

		private void comboBox409_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (transitions == null)
				return;

			double bankAngle;

			if (!double.TryParse(comboBox409.Text, out bankAngle))
				return;

			if (comboBox409.Tag != null && comboBox409.Tag.ToString() == comboBox409.Text)
				return;

			transitions.BankAngle = ARANMath.DegToRad(bankAngle);

			comboBox409.Tag = comboBox409.Text;
		}

		private void numericUpDown402_ValueChanged(object sender, EventArgs e)
		{
			//double _Distance = transitions.Distance;
			transitions.plannedTurnAngle = ARANMath.DegToRad((double)numericUpDown402.Value);

			//if (transitions.Distance != _Distance)
			textBox401.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(transitions.Distance, eRoundMode.NEAREST).ToString();
			textBox401.Tag = textBox401.Text;

			textBox404.Text = GlobalVars.unitConverter.HeightToDisplayUnits(transitions.Altitude, eRoundMode.NEAREST).ToString();
			textBox404.Tag = textBox404.Text;

			textBox405.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.Gradient, eRoundMode.NEAREST).ToString();

			if (ARANMath.RadToDeg(transitions.BankAngle) >= 21.0)
				comboBox409.SelectedIndex = 2;
			else if (ARANMath.RadToDeg(transitions.BankAngle) <= 19.0)
				comboBox409.SelectedIndex = 0;
			else
				comboBox409.SelectedIndex = 1;
		}

		private void comboBox405_SelectedIndexChanged(object sender, EventArgs e)
		{
			checkBox401.Enabled = comboBox405.SelectedIndex == 1;
			transitions.SensorType = (eSensorType)comboBox405.SelectedIndex;
		}

		private void checkBox401_CheckedChanged(object sender, EventArgs e)
		{
			transitions.MultiCoverage = checkBox401.Checked;
		}

		private void comboBox404_SelectedIndexChanged(object sender, EventArgs e)
		{
			transitions.FlyMode = (eFlyMode)comboBox404.SelectedIndex;
		}

		private void comboBox406_SelectedIndexChanged(object sender, EventArgs e)
		{
			transitions.PBNClass = (ePBNClass)comboBox406.SelectedItem;
		}

		private void comboBox407_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox407.SelectedIndex < 0)
				return;

			transitions.PathAndTermination = (CodeSegmentPath)comboBox407.SelectedItem;
			comboBox403.Visible = transitions.PathAndTermination == CodeSegmentPath.DF;
			label416.Visible = comboBox403.Visible;

			double nnAzt = ARANFunctions.DirToAzimuth(transitions.ReferenceFIX.PrjPt, transitions.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			if ((CodeSegmentPath)comboBox407.SelectedItem == CodeSegmentPath.DF)
			{
				transitions.turnDirection = comboBox403.SelectedIndex == 0 ? TurnDirection.CCW : TurnDirection.CW;
				UpDown401Range.Min = ARANMath.Modulus(nnAzt - 270.0, 360.0);
				UpDown401Range.Max = ARANMath.Modulus(nnAzt + 270.0, 360.0);
			}
			else
			{
				UpDown401Range.Min = ARANMath.Modulus(nnAzt - 120.0, 360.0);
				UpDown401Range.Max = ARANMath.Modulus(nnAzt + 120.0, 360.0);
			}
		}

		private void comboBox408_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (transitions == null)
				return;
			transitions.MOCLimit = GlobalVars.EnrouteMOCValues[comboBox408.SelectedIndex];
			reportForm.FillPage07(transitions.CurrentObstacleList, transitions.CurrentDetObs);
			_haveReport = true;
		}

		private void button402_Click(object sender, EventArgs e)
		{
			reportForm.AddPage08(transitions.CurrentObstacleList, transitions.CurrentDetObs);

			transitions.UpdateEnabled = false;
			bool transfered = transitions.transferedOver56;

			if (!transitions.Add())
			{
				transitions.UpdateEnabled = true;
				return;
			}

			_haveReport = true;

			FIX EndFIX = (FIX)transitions.Legs[transitions.Legs.Count - 1].EndFIX;

			UpDown401Range.Max = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Min, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			UpDown401Range.Min = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Max, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			if (transfered != transitions.transferedOver56)
			{
				comboBox406.Items.Clear();
				if(transitions.transferedOver56)
					foreach (ePBNClass pbn in regularTurnPBNClass)
						comboBox406.Items.Add(pbn);
				else
					foreach (ePBNClass pbn in firstTurnPBNClass)
						comboBox406.Items.Add(pbn);

				comboBox406.SelectedIndex = 0;
			}

			comboBox407.Items.Clear();
			if (EndFIX.FlyMode == eFlyMode.Flyby)
				foreach (CodeSegmentPath pt in FlyByPathAndTerminations)
					comboBox407.Items.Add(pt);
			else
				foreach (CodeSegmentPath pt in FlyOverPathAndTerminations)
					comboBox407.Items.Add(pt);

			comboBox407.SelectedIndex = 0;

			textBox406.Text = transitions.FIXName;
			transitions.FlyMode = (eFlyMode)comboBox404.SelectedIndex;
			transitions.SensorType = (eSensorType)comboBox405.SelectedIndex;

			transitions.PBNClass = (ePBNClass)comboBox406.SelectedItem;

			transitions.PathAndTermination = (CodeSegmentPath)comboBox407.SelectedItem;

			if (radioButton402.Checked)
				comboBox401_SelectedIndexChanged(comboBox401, new EventArgs());

			if (radioButton404.Checked)
				comboBox402_SelectedIndexChanged(comboBox402, new EventArgs());

			button403.Enabled = true;
			_haveReport = true;

			transitions.UpdateEnabled = true;
		}

		private void button403_Click(object sender, EventArgs e)
		{
			transitions.UpdateEnabled = false;
			bool transfered = transitions.transferedOver56;

			if (!transitions.Remove())
			{
				transitions.UpdateEnabled = true;
				return;
			}

			reportForm.RemoveLastLeg();			//+++++++++++++++++++++++++++++++++++++++
			//numericUpDown302.Value = (decimal)ARANMath.RadToDeg(transitions.plannedTurnAngle);
			//transitions.UpdateEnabled = true;		//????????????????????????????
			//_AddedFIX = transitions.LegPoints[transitions.LegPoints.Count - 1];

			UpDown401Range.Max = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Min, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			UpDown401Range.Min = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Max, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			//FIX tmpFix = new FIX(GlobalVars.gAranEnv);
			//tmpFix.Assign(transitions.Legs[transitions.Legs.Count - 1].EndFIX);
			_AddedFIX = transitions.LegPoints[transitions.LegPoints.Count - 1];
			int selectedInex, i;

			if (transfered != transitions.transferedOver56)
			{
				selectedInex = i = 0;
				comboBox406.Items.Clear();
				if (transitions.transferedOver56)
					foreach (ePBNClass pbn in regularTurnPBNClass)
					{
						//if(transitions.PBNClass == pbn)
						//	selectedInex = i;
						comboBox406.Items.Add(pbn);
						//i++;
					}
				else
					foreach (ePBNClass pbn in firstTurnPBNClass)
						comboBox406.Items.Add(pbn);

				comboBox406.SelectedIndex = selectedInex;
			}

			selectedInex = i = 0;
			comboBox407.Items.Clear();
			if (_AddedFIX.FlyMode == eFlyMode.Flyby)
				foreach (CodeSegmentPath pt in FlyByPathAndTerminations)
				{
					comboBox407.Items.Add(pt);
					if (transitions.PathAndTermination == pt)
						selectedInex = i;
					i++;
				}
			else
				foreach (CodeSegmentPath pt in FlyOverPathAndTerminations)
				{
					comboBox407.Items.Add(pt);
					if (transitions.PathAndTermination == pt)
						selectedInex = i;
					i++;
				}

			comboBox407.SelectedIndex = selectedInex;

			//double nnAzt = ARANFunctions.DirToAzimuth(transitions.ReferenceFIX.PrjPt, transitions.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefShp);
			//if ((ePathAndTermination)comboBox307.SelectedItem == ePathAndTermination.DF)
			//{
			//    transitions.turnDirection = comboBox303.SelectedIndex == 0 ? TurnDirection.CCW : TurnDirection.CW;
			//    UpDown301Range.Min = ARANMath.Modulus(nnAzt - 270.0, 360.0);
			//    UpDown301Range.Max = ARANMath.Modulus(nnAzt + 270.0, 360.0);
			//}
			//else
			//{
			//    UpDown301Range.Min = ARANMath.Modulus(nnAzt - 120.0, 360.0);
			//    UpDown301Range.Max = ARANMath.Modulus(nnAzt + 120.0, 360.0);
			//}

			UpDown401Range.Max = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Min, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			UpDown401Range.Min = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Max, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			double nnAzt = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			numericUpDown401.Value = (decimal)nnAzt;

			comboBox404.SelectedIndex = (int)transitions.FlyMode;
			comboBox405.SelectedIndex = (int)transitions.SensorType;
			comboBox406.SelectedItem = (int)transitions.PBNClass;
			comboBox407.SelectedItem = (int)transitions.PathAndTermination;

			//if (radioButton302.Checked)
			//    comboBox301_SelectedIndexChanged(comboBox301, new EventArgs());

			//if (radioButton304.Checked)
			//    comboBox302_SelectedIndexChanged(comboBox302, new EventArgs());

			transitions.UpdateEnabled = true;

			textBox401.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(transitions.Distance, eRoundMode.NEAREST).ToString();
			double TAS = ARANMath.IASToTAS(transitions.IAS, transitions.Altitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);
			textBox403.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS, eRoundMode.SPECIAL_NEAREST).ToString();

			textBox404.Text = GlobalVars.unitConverter.HeightToDisplayUnits(transitions.Altitude, eRoundMode.NEAREST).ToString();

			textBox405.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.Gradient, eRoundMode.NEAREST).ToString();
			textBox406.Text = transitions.FIXName;

			if (ARANMath.RadToDeg(transitions.BankAngle) >= 21.0)
				comboBox409.SelectedIndex = 2;
			else if (ARANMath.RadToDeg(transitions.BankAngle) <= 19.0)
				comboBox409.SelectedIndex = 0;
			else
				comboBox409.SelectedIndex = 1;
		}

		private void button401_Click(object sender, EventArgs e)
		{
			//FIXInfoForm.ShowFixInfo(Left + MultiPage1.Left + _MultiPage1_TabPage3.Left + button401.Left, Top + _MultiPage1_TabPage3.Top + button401.Top, transitions.LegPoints);
		}

		//void OnLegDirectionChanged(object sender, double value)
		//{
		//}


		#endregion

	}
}
