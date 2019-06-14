using System;
using System.Drawing;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Geometries;
using Aran.PANDA.Departure.Properties;
using Aran.Queries;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using Aran.AranEnvironment;
using Aran.Aim.Data;

namespace Aran.PANDA.Departure
{
    [System.Runtime.InteropServices.ComVisible(false)]
    public partial class CDepartOmniDirect : Form
    {
        #region declerations
        private const double MinSectAngle = 16;
        private const double MaxSectAngle = 165;
        private const double SectorBufferAngle = 15;

        private const int MaxTraceSegments = 100;

        private IGraphicsContainer pGraphics;
        private IScreenCapture screenCapture;

        private IElement pCircleElem;
        private IElement pTraceElem;
        private IElement pProtectElem;
        private IElement[] pSectorElems;
        private IElement StraightAreaElem;
        private IElement pTraceSelectElem;
        private IElement pProtectSelectElem;

        private IPoint ptCenter;
        private IPolygon pCircle;
        private IPointCollection pPolygon;
        //private ISimpleFillSymbol pSectorFS;

        private ObstacleContainer oFullList;
        private ObstacleContainer oInnerList;
        private ObstacleContainer oOuterList;

        private double RMin;
        private double fIAS;
        private double dAr1;
        private double dAr2;
        private double TNAH;
        private double dTNAh;
        private double DepDir;
        private double MinPDG;
        private double drPDGMax;
        private double MOCLimit;
        private double dAr2addend;
        private double ModellingTNA;
        private double TIARequiredTNAH;
        private double OTARequiredTNAH;

        private double segPDG;
        private double CurrPDG;
        private double TACurrPDG;
        private double appliedPDG;

        private int LeftSectorsCount;
        private int RightSectorsCount;

        private RWYType[] RWYList;
        private ProhibitedSector[] LeftSectors;
        private ProhibitedSector[] RightSectors;
        private SquareSolutionArea[] Solutions;
        private TraceSegment[] Trace = new TraceSegment[MaxTraceSegments + 1];

        private int TSC;
        private int iTnaH;
        private int AirCat;
        private int CurrPage;
        private int idPDGMax;
        private int iPDGToTop;
        private int iShortage;

        private bool IsSectorized;
        private double SectorRightDir;
        private double SectorLeftDir;
        private double SectorHeight;
        private IPointCollection SectorPoly;
        private StandardInstrumentDeparture _Procedure;
        private bool MultiPageBusy;
        private bool Report;
        private RWYType DER;
        private CReports ReportsFrm;
        private CAddSegment AddSegmentFrm;
        private Label[] Label1;

        private int HelpContextID;
        private bool bFormInitialised = false;
        public bool IsClosing = false;

        ReportFile AccurRep;
        ReportFile OmniProtRep;
        ReportFile OmniLogRep;
        ReportFile OmniGeomRep;

        #endregion

        #region Form

        public CDepartOmniDirect()
        {
            //  This call is required by the Windows Form Designer.
            InitializeComponent();
            screenCapture = GlobalVars.gAranEnv.GetScreenCapture(FeatureType.StandardInstrumentDeparture.ToString());

            bFormInitialised = true;

            pGraphics = GlobalVars.GetActiveView().GraphicsContainer;

            MultiPageBusy = false;
            Report = false;
            MultiPage1.SelectedIndex = 0;
            CurrPage = 0;
            TSC = 0;
            IsSectorized = false;

            HelpContextID = 2100;

            TextBox101.Text = (100.0 * PANS_OPS_DataBase.dpPDG_Nom.Value).ToString("0.0");
            TextBox206.Text = TextBox101.Text;

            // =============================================================================================

            // Dim MOCArray() As String
            //     If HeightUnit = 0 Then
            //         MOCArray = New String() {"300", "450", "600"}
            //     Else
            //         MOCArray = New String() {"1000", "1500", "2000"}
            //     End If
            //     ComboBox003.Items.Clear()
            //     ComboBox003.Items.Add(My.Resources.str1012)
            //     ComboBox003.Items.Add(MOCArray(0))
            //     ComboBox003.Items.Add(MOCArray(1))
            //     ComboBox003.Items.Add(MOCArray(2))


            ComboBox003.Items.Clear();
            //  ComboBox003.Items.Add(My.Resources.str1012)

            int i, n = GlobalVars.EnrouteMOCValues.Length;
            for (i = 0; i < n; i++)
                ComboBox003.Items.Add(Functions.ConvertHeight(GlobalVars.EnrouteMOCValues[i], eRoundMode.SPECIAL).ToString());

            pPolygon = new ESRI.ArcGIS.Geometry.Polygon();

            TextBox003.Text = Functions.ConvertDistance(RMin, eRoundMode.CEIL).ToString();
            TextBox007.Text = Functions.ConvertHeight(600.0 + GlobalVars.CurrADHP.Elev, eRoundMode.NEAREST).ToString();

            MultiPage1.TabPages[4].Visible = false;
            MultiPage1.TabPages[5].Visible = false;

            AddSegmentFrm = new CAddSegment(screenCapture);
            ReportsFrm = new CReports();

            ReportsFrm.SetBtn(ReportBtn, GlobalVars.ReportHelpIDOmni);
            // CreateLog Me.Caption
            // ===============================================================
            this.Text = Resources.str00011;
            MultiPage1.TabPages[0].Text = Resources.str00100;
            MultiPage1.TabPages[1].Text = Resources.str00110;
            MultiPage1.TabPages[2].Text = Resources.str00120;
            MultiPage1.TabPages[3].Text = Resources.str00130;
            MultiPage1.TabPages[4].Text = Resources.str00140;
            MultiPage1.TabPages[5].Text = Resources.str00150;

            // ===============================================================
            PrevBtn.Text = Resources.str00002;
            NextBtn.Text = Resources.str00003;
            OkBtn.Text = Resources.str00004;
            CancelBtn.Text = Resources.str00005;
            ReportBtn.Text = Resources.str00006;
            // ===========Page1===============================================
            Label001.Text = Resources.str01001;
            Label002.Text = Resources.str01002;
            Label003.Text = Resources.str01003;
            Label004.Text = Resources.str01004;
            Label005.Text = Resources.str01005;
            Label006.Text = Resources.str01006;
            // Label007.Caption = LoadResString(1007)
            Label008.Text = Resources.str01008;
            Label009.Text = Resources.str01010;
            Label014.Text = Resources.str01009;
            Label016.Text = Resources.str01011;
            Label017.Text = Resources.str00073;

            Label111.Text = Resources.str02621;
            Label112.Text = Resources.str12020;

            Label2.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label010.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
            Label011.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
            Label012.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label013.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
            Label015.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;

            Frame001.Text = Resources.str10001;

            // ===========Page2===============================================
            Label101.Text = Resources.str01101;
            Label102.Text = Resources.str02511;
            Label103.Text = Resources.str01103;
            // Label105.Caption = LoadResString(1105)
            Label107.Text = Resources.str01107;
            Label108.Text = Resources.str01108;
            Label109.Text = Resources.str01109;
            Label110.Text = Resources.str01110;

            Label104.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
            // Label106.Caption = HeightConverter(HeightUnit).Unit
            Label113.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label114.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;

            CheckBox101.Text = Resources.str11013;

            // ===========Page3================================================
            Label201.Text = Resources.str01201;
            Label202.Text = Resources.str01202;
            Label203.Text = Resources.str01203;
            Label204.Text = Resources.str01204;
            Label211.Text = Resources.str02212;
            Label212.Text = Resources.str02212;
            Label213.Text = Resources.str12020;
            Label209.Text = Resources.str12015;
            Label210.Text = Resources.str12016;
            Label205.Text = Resources.str12017;
            Label206.Text = Resources.str12018;

            Label214.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label215.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label216.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
            Label218.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
            Label220.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label221.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;

            Frame201.Text = Resources.str12010;
            Frame202.Text = Resources.str12014;
            Frame203.Text = Resources.str12019;
            OptionButton201.Text = Resources.str12011;
            OptionButton202.Text = Resources.str12012;
            CheckBox201.Text = Resources.str12013;

            // ===========Page4=================================================
            Label301.Text = Resources.str01301;
            Label302.Text = Resources.str02511;
            Label303.Text = Resources.str01303;
            Label304.Text = Resources.str02511;
            Label305.Text = Resources.str01305;
            Label306.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label307.Text = Resources.str01307;
            Label308.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
            Label309.Text = Resources.str12023;
            Label310.Text = Resources.str12024;
            Label311.Text = Resources.str02212;
            Label312.Text = Resources.str02212;
            Label313.Text = Resources.str12025;
            Label314.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label315.Text = Resources.str00001;
            Label316.Text = Resources.str12020;
            // Label317.Caption = LoadResString(2209)
            Label317.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;

            CheckBox301.Text = Resources.str12021;
            Frame301.Text = Resources.str12022;

            // =======  Page5====================================================

            Label411.Text = Resources.str01403;
            Label413.Text = Resources.str01409;
            Label415.Text = Resources.str01404;
            Label416.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label417.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label418.Text = GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Unit;
            Label419.Text = Resources.str01401;
            Label420.Text = Resources.str01406;
            Label421.Text = Resources.str00902 + "(" + GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Unit + ")";
            Label422.Text = Resources.str00903 + "(" + GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Unit + ")";
            Label423.Text = Resources.str01410;
            Label424.Text = Resources.str01402;

            // =======Page6=================================================
            AddSegmentBtn.Text = Resources.str01501;
            RemoveSegmentBtn.Text = Resources.str01502;
            SaveGeometryBtn.Text = Resources.str01503;
            // =============================================================
            for (i = 0; i < 12; i++)
                ListView501.Columns[i].Text = Resources.ResourceManager.GetString("str" + (12602 + i).ToString());

            // 2007
            Label1 = new Label[6] { Label1_00, Label1_01, Label1_02, Label1_03, Label1_04, Label1_05 };

            Label1[0].Text = Resources.str00100;
            Label1[1].Text = Resources.str00110;
            Label1[2].Text = Resources.str00120;
            Label1[3].Text = Resources.str00130;
            Label1[4].Text = Resources.str00140;
            Label1[5].Text = Resources.str00150;

            FocusStepCaption(0);
            MultiPage1.Top = -MultiPage1.ItemSize.Height - 3; // -MultiPage1.ItemSize.Height-3
            Height = Height - MultiPage1.ItemSize.Height - 3;
            Frame1.Top = Frame1.Top - MultiPage1.ItemSize.Height - 3;

            ShowPanelBtn.Checked = false;
            this.Width = 505;

            //ComboBox004.SelectedIndex = 0;
            //GlobalVars.CurrCmd = 1;


            DBModule.FillRWYList(out RWYList, GlobalVars.CurrADHP);

            n = RWYList.Length;

            if (n == 0)
            {
                MessageBox.Show(Resources.str15056, "PANDA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            DBModule.FillWPT_FIXList(out GlobalVars.WPTList, GlobalVars.CurrADHP, GlobalVars.MaxNAVDist);
            DBModule.FillNavaidList(out GlobalVars.NavaidList, out GlobalVars.DMEList, GlobalVars.CurrADHP, GlobalVars.MaxNAVDist);

            TextBox007.Text = Functions.ConvertHeight(600.0 + GlobalVars.CurrADHP.Elev, eRoundMode.NEAREST).ToString();

            for (i = 0; i < n; i++)
                ComboBox001.Items.Add(RWYList[i].Name);

            GlobalVars.RModel = 0.0;
            RMin = 50.0 * System.Math.Round(0.02 * 600.0 / PANS_OPS_DataBase.dpPDG_Nom.Value + 0.4999);
            if (RMin < 20000.0) RMin = 20000.0;

            //TextBox003.Text = "0";	// Functions.ConvertDistance(RMin, eRoundMode.rmCEIL).ToString();
            //TextBox003_Validating(TextBox003, new System.ComponentModel.CancelEventArgs());
            //RMax = 50.0 * System.Math.Round((0.02 * 2400.0) / PANS_OPS_DataBase.dpMOC.Value + 0.4999);
            //if (RMin+5000.0 < RMax)			RMax = RMin + 5000.0;

            ComboBox001.SelectedIndex = 0;
        }

        private void DepartOmniDirectFrm_FormClosed(System.Object eventSender, FormClosedEventArgs eventArgs)
        {
            IsClosing = true;

            screenCapture.Rollback();

            pCircle = null;
            pPolygon = null;
            //pSectorFS = null;

            oInnerList.Obstacles = null;
            oInnerList.Parts = null;

            oFullList.Obstacles = null;
            oFullList.Parts = null;

            RightSectors = null;
            LeftSectors = null;
            System.Array.Clear(Trace, 0, Trace.Length);

            Functions.DeleteGraphicsElement(pCircleElem);
            pCircleElem = null;

            Functions.DeleteGraphicsElement(StraightAreaElem);
            StraightAreaElem = null;

            Functions.DeleteGraphicsElement(pTraceElem);
            pTraceElem = null;

            Functions.DeleteGraphicsElement(pTraceSelectElem);
            pTraceSelectElem = null;

            Functions.DeleteGraphicsElement(pProtectElem);
            pProtectElem = null;

            Functions.DeleteGraphicsElement(pProtectSelectElem);
            pProtectSelectElem = null;

            if (IsSectorized)
                ClearSectorElems();

            if (ReportsFrm != null)
                ReportsFrm.Close();

            if (AddSegmentFrm != null)
                AddSegmentFrm.Close();
            //GlobalVars.GetActiveView().Refresh();
        }

        private void CDepartOmniDirectFrm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.F1)
                return;

            NativeMethods.HtmlHelp(0, GlobalVars.HelpFile, GlobalVars.HH_HELP_CONTEXT, HelpContextID);
            e.Handled = true;
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

        protected override void WndProc(ref System.Windows.Forms.Message m)
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

        #region Main Form controls

        // ---------------------------------------------------------------------------------------
        //  Procedure : InfoBtn_Click
        //  DateTime  : 13.06.2007 10:05
        //  Author    : RuslanA
        //  Purpose   :
        // ---------------------------------------------------------------------------------------
        // 
        private void ShowPanelBtn_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            if (!bFormInitialised)
                return;

            if (ShowPanelBtn.Checked)
            {
                this.Width = 655;
                ShowPanelBtn.Image = Resources.bmpHIDE_INFO;
            }
            else
            {
                this.Width = 505;
                ShowPanelBtn.Image = Resources.bmpSHOW_INFO;
            }
        }

        private void HelpBtn_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            NativeMethods.HtmlHelp(0, GlobalVars.HelpFile, GlobalVars.HH_HELP_CONTEXT, HelpContextID);
        }

        private void PrevBtn_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            double fTmp;
            // Dim Cancel As Boolean
            // LogStr LoadResString(15044)                                                      '"<-?????? ???????? ????"

            switch (MultiPage1.SelectedIndex)
            {
                case 1:
                    GlobalVars.RModel = 0.0;
                    TextBox003.Tag = "a";
                    Functions.DeleteGraphicsElement(StraightAreaElem);
                    GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

                    TextBox003_Validating(TextBox003, new System.ComponentModel.CancelEventArgs());
                    ReportsFrm.Hide();
                    Report = false;
                    break;
                case 2:
                    //     CurrPDG = MinPDG
                    //     IncludeObs PtOutList
                    CheckBox201.Checked = false;

                    TextBox101.Tag = "a";
                    TextBox101.Text = (100.0 * MinPDG + 0.0499999).ToString("0.00");

                    fTmp = MinPDG;
                    dAr2addend = 0.0;
                    AdjustI_Zone(fTmp);
                    iPDGToTop = CalcZoneIIPDG(out fTmp);
                    dAr2 = 0.0;

                    OptionButton201.Checked = true;
                    TextBox205.Text = "";
                    //     TextBox206.Text = CStr(dpPDG_Nom.Value * 100.0)
                    ReportsFrm.SetTabVisible(1, false);

                    //     ClearSectorElems
                    //     IncludeObs PtOutList

                    TextBox101.Tag = "a";
                    TextBox101_Validating(TextBox101, new System.ComponentModel.CancelEventArgs());
                    //     CheckBox201.Value = False
                    //     UpdateTAData
                    break;
                case 3:
                    OkBtn.Enabled = false;

                    //     dAr2addend = 0.0
                    //     AdjustII_Zone()
                    //     CheckBox201_Click
                    //     if Frame201.Enabled then TextBox206_Validate false
                    // Dim ObsDPDG As ObstacleHd
                    //     If iPDGToTop > -1 Then
                    //         ObsDPDG = PtInList(iPDGToTop)
                    //     Else
                    //         ObsDPDG.ID = LoadResString(39014)
                    //     End If
                    //     ReportsFrm.FillPage1 PtInList, CurrPDG, ObsDPDG, ComboBox001.Text, 1
                    break;
                case 4:
                    //dAr2addend = 0.0;
                    //AdjustI_Zone(MinPDG);

                    IPolygon pPolygon1 = Functions.PolygonIntersection(pPolygon, pCircle);
                    Functions.DeleteGraphicsElement(StraightAreaElem);
                    StraightAreaElem = Functions.DrawPolygon(pPolygon1, Functions.RGB(0, 0, 255));
                    StraightAreaElem.Locked = true;

                    if (IsSectorized)
                        DrawExcludeSector();

                    break;
                case 5:
                    TSC = 0;

                    if (pTraceElem != null)
                    {
                        Functions.DeleteGraphicsElement(pTraceElem);
                        pTraceElem = null;
                    }

                    if (pTraceSelectElem != null)
                    {
                        Functions.DeleteGraphicsElement(pTraceSelectElem);
                        pTraceSelectElem = null;
                    }

                    if (pProtectElem != null)
                    {
                        Functions.DeleteGraphicsElement(pProtectElem);
                        pProtectElem = null;
                    }

                    if (pProtectSelectElem != null)
                    {
                        Functions.DeleteGraphicsElement(pProtectSelectElem);
                        pProtectSelectElem = null;
                    }

                    GlobalVars.GetActiveView().Refresh();
                    break;
            }

            screenCapture.Delete();
            CurrPage = MultiPage1.SelectedIndex - 1;
            MultiPage1.SelectedIndex = CurrPage;

            // 2007
            FocusStepCaption((MultiPage1.SelectedIndex));

            NextBtn.Enabled = MultiPage1.SelectedIndex < MultiPage1.TabPages.Count - 1;
            PrevBtn.Enabled = MultiPage1.SelectedIndex > 0;

            if (CurrPage == 2)
            {
                MultiPage1.TabPages[4].Visible = false;
                MultiPage1.TabPages[5].Visible = false;
            }

            HelpContextID = 2000 + 100 * (MultiPage1.SelectedIndex + 1);
            this.Activate();
        }

        private void NextBtn_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            double TmpPDG;
            double NewPDG;
            double DistMax;
            double dAr12;
            double fTmp;
            IPolygon pPolygon1;
            ObstacleData ObsDPDG;

            NativeMethods.ShowPandaBox(this.Handle.ToInt32());

            switch (MultiPage1.SelectedIndex)
            {
                case 0:
                    if (!double.TryParse(TextBox003.Text, out fTmp))
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str00152, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextBox003.Focus();
                        return;
                    }
                    else if (Functions.DeConvertDistance(fTmp) + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Rounding / GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Multiplier < RMin)
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15009, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextBox003.Focus();
                        return;
                    }

                    MOCLimit = Functions.DeConvertHeight(double.Parse(ComboBox003.Text));

                    ReportsFrm.SetTabVisible(-1, false);
                    NewPDG = PANS_OPS_DataBase.dpPDG_Nom.Value;

                    do
                    {
                        MinPDG = NewPDG;
                        iPDGToTop = CreatePolygon(GlobalVars.RModel, ref NewPDG);
                    }
                    while (NewPDG < PANS_OPS_DataBase.dpMaxPosPDG.Value * 50.0 && MinPDG < NewPDG);

                    if (NewPDG > PANS_OPS_DataBase.dpMaxPosPDG.Value * 50.0)
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15010 + ComboBox001.Text + Resources.str15011 + "\r\n" +
                            Resources.str15012 + (5000.0 * PANS_OPS_DataBase.dpMaxPosPDG.Value).ToString("0.0") + "%",
                                null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    CurrPDG = MinPDG;
                    TextBox101.Text = (CurrPDG * 100.0 + 0.0499999).ToString("0.0");
                    // =================================================================
                    //RadialateObs(oOuterList.Parts);
                    drPDGMax = Functions.dPDGMax(oInnerList.Parts, CurrPDG, out idPDGMax);

                    TIARequiredTNAH = CalcStraightReqTNAH(CurrPDG, drPDGMax, ref iTnaH, false);

                    if (iTnaH > oInnerList.Parts.Length)
                        iTnaH = idPDGMax;

                    //TextBox103.Text = CStr(ConvertHeight(TIARequiredTNAH, 3))

                    TextBox107.Text = Functions.ConvertHeight(TIARequiredTNAH + DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.CEIL).ToString();
                    TextBox108.Text = Functions.ConvertHeight(TIARequiredTNAH, eRoundMode.CEIL).ToString();

                    if (iPDGToTop > -1)
                        TextBox104.Text = oInnerList.Obstacles[oInnerList.Parts[iPDGToTop].Owner].UnicalName;
                    else
                        TextBox104.Text = Resources.str39014;

                    if (idPDGMax > -1)
                        TextBox105.Text = oInnerList.Obstacles[oInnerList.Parts[idPDGMax].Owner].UnicalName;
                    else
                        TextBox105.Text = Resources.str39014;

                    if (iTnaH > -1)
                        TextBox106.Text = oInnerList.Obstacles[oInnerList.Parts[iTnaH].Owner].UnicalName;
                    else
                        TextBox106.Text = Resources.str39014;

                    TextBox102.Text = Functions.ConvertDistance(drPDGMax, eRoundMode.CEIL).ToString();
                    TextBox204.Text = (100.0 * CurrPDG + 0.0499999).ToString("0.0");

                    if (iPDGToTop > -1)
                        ObsDPDG = oInnerList.Parts[iPDGToTop];
                    else
                    {
                        ObsDPDG = new ObstacleData();
                        ObsDPDG.Owner = -1;             // Resources.str39014;
                    }

                    ReportsFrm.FillPage1(oInnerList, CurrPDG, ObsDPDG, ComboBox001.Text, 1);
                    Report = true;
                    // =================================================================
                    pPolygon1 = Functions.PolygonIntersection(pPolygon, pCircle);
                    Functions.DeleteGraphicsElement(StraightAreaElem);
                    StraightAreaElem = Functions.DrawPolygon(pPolygon1, Functions.RGB(0, 0, 255));
                    StraightAreaElem.Locked = true;

                    // =================== Start Log ================================
                    //     LogStr LoadResString(15014) + MultiPage1.TabCaption(0)   '"???????? 1 - "
                    //     LogStr LoadResString(15015) + ComboBox001.Text           '"    ???:           "
                    //     LogStr LoadResString(15016) + TextBox001.Text            '"    ?? ???:       "
                    //     LogStr LoadResString(15017) + TextBox004.Text            '"    ?? ???:       "
                    //     LogStr LoadResString(15018) + TextBox002.Text            '"    ????? ??? (?): "
                    //     LogStr LoadResString(15019) + TextBox003.Text            '"    ?????? ?????? ?????????? ?? ??? (?): "
                    //     LogStr LoadResString(15020)                             ' "    ?????? ? ????????????:"
                    //     LogStr LoadResString(15021) + TextBox006.Text            '"    ???????????? ???????? (?):       "
                    //     LogStr LoadResString(15022) + TextBox005.Text            '"    ?????????? ? ???????? ???????:   "
                    // =================== End Log ================================
                    break;
                case 1:
                    if (!double.TryParse(TextBox101.Text, out fTmp))
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15023, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextBox101.Focus();
                        return;
                    }

                    dAr2 = 0.0;
                    dAr2addend = 0.0;
                    NewPDG = 0.01 * fTmp;

                    do
                    {
                        TmpPDG = NewPDG;
                        AdjustI_Zone(NewPDG);
                        iPDGToTop = CalcZoneIIPDG(out NewPDG);
                    }
                    while (NewPDG < PANS_OPS_DataBase.dpMaxPosPDG.Value * 50.0 && TmpPDG < NewPDG);

                    if (NewPDG >= PANS_OPS_DataBase.dpMaxPosPDG.Value * 50.0)
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15024 + TextBox101.Text + "%", null, MessageBoxButtons.OK, MessageBoxIcon.Error);

                        // "????? ? ????? ??????????? ?????????? ??? ???????? ?????????? ????????? >= "
                        return;
                    }

                    CurrPDG = TmpPDG;
                    TextBox204.Text = (100.0 * CurrPDG + 0.0499999).ToString("0.0");

                    // =================================================================
                    drPDGMax = Functions.dPDGMax(oInnerList.Parts, CurrPDG, out idPDGMax);
                    TIARequiredTNAH = CalcStraightReqTNAH(CurrPDG, drPDGMax, ref iTnaH, CheckBox101.Checked);

                    if (iTnaH > oInnerList.Parts.Length)
                        iTnaH = idPDGMax;

                    if (iPDGToTop > -1)
                        ObsDPDG = oInnerList.Parts[iPDGToTop];
                    else
                    {
                        ObsDPDG = new ObstacleData();
                        ObsDPDG.Owner = -1;             // Resources.str39014;
                    }

                    ReportsFrm.FillPage1(oInnerList, CurrPDG, ObsDPDG, ComboBox001.Text, 1);
                    Report = true;
                    // =================================================================
                    AdjustII_Zone();
                    TIARequiredTNAH = CalcStraightReqTNAH(CurrPDG, drPDGMax, ref iTnaH, CheckBox101.Checked);

                    TextBox203.Text = Functions.ConvertHeight(TIARequiredTNAH + DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.CEIL).ToString();
                    TextBox211.Text = Functions.ConvertHeight(TIARequiredTNAH, eRoundMode.CEIL).ToString();

                    TACurrPDG = 0.01 * double.Parse(TextBox206.Text);
                    dTNAh = CalcTNAHshortTA(TIARequiredTNAH, ref iShortage);

                    if (iShortage >= 0)
                    {
                        TextBox207.Text = oOuterList.Obstacles[oOuterList.Parts[iShortage].Owner].UnicalName;
                        TextBox208.Text = Functions.ConvertHeight(oOuterList.Parts[iShortage].Height + oOuterList.Parts[iShortage].MOC, eRoundMode.NEAREST).ToString() + " / " + Functions.ConvertHeight(TIARequiredTNAH + oOuterList.Parts[iShortage].Dist * TACurrPDG, eRoundMode.NEAREST).ToString();
                    }
                    else
                    {
                        TextBox207.Text = Resources.str39014;
                        TextBox208.Text = "-";
                    }

                    TextBox202.Text = Functions.ConvertDistance(dAr1 + dAr2, eRoundMode.CEIL).ToString();
                    TextBox201.Text = Functions.ConvertHeight(dTNAh, eRoundMode.NEAREST).ToString();

                    Frame201.Enabled = dTNAh > 0.0;
                    int tmp;
                    dAr2addend = CalcTIARange(out tmp);

                    CheckBox201.Enabled = dTNAh > 0.0;

                    DistMax = dAr1 + dAr2 + dAr2addend;
                    OTARequiredTNAH = CurrPDG * DistMax + PANS_OPS_DataBase.dpOIS_abv_DER.Value;
                    TNAH = OTARequiredTNAH;

                    SortObsByRadial(oOuterList.Parts);
                    SelectInterestingObs(oOuterList.Parts);
                    FindProSectors(oOuterList.Parts);
                    pSectorElems = new ESRI.ArcGIS.Carto.IElement[LeftSectorsCount + RightSectorsCount + 2];   //--- Checked

                    ReportsFrm.FillPage2(oOuterList, TACurrPDG, TIARequiredTNAH, MOCLimit);   //, dAr1 + dAr2 + dAr2addend
                    ReportsFrm.FillPage6(oOuterList, TIARequiredTNAH, TACurrPDG, DepDir);

                    if (CheckBox201.Enabled && CheckBox201.Checked)
                        CheckBox201_CheckedChanged(CheckBox201, new System.EventArgs());

                    TextBox205.Text = Functions.ConvertDistance(dAr2addend, eRoundMode.CEIL).ToString();
                    TextBox205.Tag = "";

                    CheckBox101_CheckedChanged(CheckBox101, new System.EventArgs());
                    TextBox205_Validating(TextBox205, new System.ComponentModel.CancelEventArgs());

                    pPolygon1 = Functions.PolygonIntersection(pPolygon, pCircle);
                    Functions.DeleteGraphicsElement(StraightAreaElem);
                    StraightAreaElem = Functions.DrawPolygon(pPolygon1, Functions.RGB(0, 0, 255));
                    StraightAreaElem.Locked = true;

                    break;
                case 2:
                    if (OptionButton201.Checked && !double.TryParse(TextBox205.Text, out fTmp))
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15035, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextBox205.Focus();
                        return;
                    }

                    if (OptionButton202.Checked && !double.TryParse(TextBox206.Text, out fTmp))
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15036, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextBox206.Focus();
                        return;
                    }

                    CheckBox301.Checked = false;
                    CheckBox301_CheckedChanged(CheckBox301, new System.EventArgs());

                    DistMax = dAr1 + dAr2 + dAr2addend;
                    TNAH = CurrPDG * DistMax + PANS_OPS_DataBase.dpOIS_abv_DER.Value;

                    AdjustII_Zone();
                    dTNAh = CalcTNAHshortTA(TNAH, ref iShortage);

                    if (dTNAh > 0.00001)
                    {
                        NativeMethods.HidePandaBox();
                        return;
                    }

                    TextBox301.Text = (100.0 * CurrPDG).ToString("0.0");
                    TextBox302.Text = (100.0 * TACurrPDG).ToString("0.0");
                    TextBox303.Text = Functions.ConvertHeight(TNAH + DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.CEIL).ToString();
                    TextBox308.Text = Functions.ConvertHeight(TNAH, eRoundMode.CEIL).ToString();
                    TextBox304.Text = Functions.ConvertDistance(DistMax, eRoundMode.CEIL).ToString();

                    OkBtn.Enabled = true;
                    NextBtn.Enabled = false;

                    if (CheckBox201.Checked)
                    {
                        Frame301.Enabled = System.Math.Abs(OTARequiredTNAH - TNAH) >= GlobalVars.distEps;
                        Label309.Enabled = Frame301.Enabled;
                        Label310.Enabled = Frame301.Enabled;
                        Label311.Enabled = Frame301.Enabled;
                        Label312.Enabled = Frame301.Enabled;
                        Label313.Enabled = Frame301.Enabled;
                        Label314.Enabled = Frame301.Enabled;
                        TextBox305.Enabled = Frame301.Enabled;
                        TextBox306.Enabled = Frame301.Enabled;
                        TextBox307.Enabled = Frame301.Enabled;

                        SectorHeight = FindSectorHeight(oOuterList.Parts, TNAH, TACurrPDG);
                        Frame301.Visible = true;
                        TextBox305.Text = UpDown209.Value.ToString();
                        TextBox306.Text = UpDown210.Value.ToString();
                        TextBox307.Text = Functions.ConvertHeight(OTARequiredTNAH, eRoundMode.CEIL).ToString();
                    }
                    else
                        Frame301.Visible = false;

                    dAr12 = dAr1 + dAr2 + dAr2addend;
                    for (int i = 0; i < oInnerList.Parts.Length; i++)
                    {
                        if (oInnerList.Parts[i].Dist <= dAr12)
                        {
                            fTmp = oInnerList.Parts[i].Height + PANS_OPS_DataBase.dpObsClr.Value;
                            if (fTmp > PANS_OPS_DataBase.dpNGui_Ar1.Value)
                                oInnerList.Parts[i].ReqTNH = fTmp;
                        }
                    }

                    ObsDPDG = new ObstacleData();
                    ObsDPDG.Owner = -1;

                    ReportsFrm.FillPage1(oInnerList, CurrPDG, ObsDPDG, ComboBox001.Text, 1, true);
                    // =================== Start Log ================================
                    //     LogStr LoadResString(15037) + MultiPage1.TabCaption(2)       '"???????? 3 - "
                    //     LogStr LoadResString(15038) + TextBox201.Text + LoadResString(15028)         '"    ????????? TNA/H:   "" ?"
                    //     LogStr LoadResString(15039) + TextBox202.Text + LoadResString(15028)         '"    ????????????? ???:     "" ?"
                    //     LogStr "    TNA/H:             " + TextBox203.Text + LoadResString(15028)    '" ?"
                    //     LogStr LoadResString(15041) + TextBox204.Text + "%"                          '"    PDG ? ???:         "
                    //     LogStr LoadResString(15042)                                                  '"    ?????? ????????? TNA/H ? ?? ??????????? ?????????? :"
                    //     If OptionButton201 Then
                    //         LogStr LoadResString(15040) + TextBox205.Text + LoadResString(15028)     '"    ??????? ? ????? ???: " " m"
                    //     Else
                    //         LogStr LoadResString(15043) + TextBox206.Text + "%"                      '"    PDG ? ??: "
                    //     End If
                    // =================== End Log ================================
                    break;
                case 3:
                    Functions.DeleteGraphicsElement(StraightAreaElem);
                    StraightAreaElem = null;

                    ClearSectorElems();

                    if (ComboBox401.SelectedIndex == 0)
                        ComboBox401_SelectedIndexChanged(ComboBox401, null);
                    else
                        ComboBox401.SelectedIndex = 0;

                    TextBox402.Text = (CurrPDG * 100).ToString("0.0");

                    TextBox403_Validating(TextBox403, new System.ComponentModel.CancelEventArgs());

                    ModellingTNA = TNAH + DER.pPtPrj[eRWY.PtDER].Z;
                    TextBox404.Text = Functions.ConvertHeight(ModellingTNA, eRoundMode.CEIL).ToString();
                    TextBox405.Text = Functions.ConvertHeight(TNAH, eRoundMode.CEIL).ToString();

                    //     TextBox404_Validate False
                    //     TextBox405_Validate False

                    if (GlobalVars.NavaidList.Length == 0)
                    {
                        //         HidePandaBox
                        MessageBox.Show(Resources.str00159, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //         Return
                    }
                    break;
                case 4:
                    //result.HStart = DER.pPtPrj[eRWY.PtDER].Z + PANS_OPS_DataBase.dpH_abv_DER.Value;
                    //result.HFinish = ModellingTNA;
                    //result.ptIn = DER.pPtPrj[eRWY.PtDER];

                    IPoint ptOut = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir, (ModellingTNA - DER.pPtPrj[eRWY.PtDER].Z - PANS_OPS_DataBase.dpNGui_Ar1.Value) / appliedPDG);

                    //Functions.DrawPointWithText(ptOut, "ptOut");
                    //Application.DoEvents();

                    double Dist = Functions.ReturnDistanceInMeters(DER.pPtPrj[eRWY.PtDER], ptOut);

                    CreatePolygon(Dist, ref appliedPDG);
                    //AdjustI_Zone(appliedPDG);

                    segPDG = appliedPDG;

                    //dAr2addend = 0.0;
                    //AdjustI_Zone(appliedPDG);

                    if (IsSectorized)
                        DrawExcludeSector();

                    AddSegmentFrm.IsSectorized = IsSectorized;
                    AddSegmentFrm.SectorRightDir = SectorRightDir;
                    AddSegmentFrm.SectorLeftDir = SectorLeftDir;
                    AddSegmentFrm.SectorHeight = SectorHeight;
                    AddSegmentFrm.SectorPoly = SectorPoly;

                    Trace[0] = AddSegmentFrm.CreateInitialSegment(DER, (IPolygon)pPolygon, appliedPDG, DepDir, TNAH);
                    TSC = 1;

                    RemoveSegmentBtn.Enabled = false;

                    ReDrawTrace();
                    ReListTrace();
                    break;
            }

            screenCapture.Save(this);
            CurrPage = MultiPage1.SelectedIndex + 1;
            MultiPage1.SelectedIndex = CurrPage;

            //  2007
            FocusStepCaption((MultiPage1.SelectedIndex));

            NextBtn.Enabled = MultiPage1.SelectedIndex < MultiPage1.TabPages.Count - 1;
            PrevBtn.Enabled = MultiPage1.SelectedIndex > 0;
            NativeMethods.HidePandaBox();

            if (CurrPage == 3)
                NextBtn.Enabled = false;

            HelpContextID = 2000 + 100 * (MultiPage1.SelectedIndex + 1);

            this.Visible = false;
            this.Show(GlobalVars.Win32Window);

            this.Activate();
        }

        private void ReportBtn_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            if (!bFormInitialised)
                return;

            if (!Report)
                return;

            if (ReportBtn.Checked)
                ReportsFrm.Show(GlobalVars.Win32Window);
            else
                ReportsFrm.Hide();
        }

        private void OkBtn_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            string RepFileName, RepFileTitle;
            screenCapture.Save(this);

            if (!Functions.ShowSaveDialog(out RepFileName, out RepFileTitle))
                return;

            ReportHeader pReport;
            //pReport.Procedure = "OD RWY" + DER.Name;	// _Procedure.Name;
            pReport.Procedure = RepFileTitle;

            //pReport.EffectiveDate = _Procedure.TimeSlice.ValidTime.BeginPosition;

            pReport.Database = GlobalVars.gAranEnv.ConnectionInfo.Database;
            pReport.Aerodrome = GlobalVars.CurrADHP.Name;
            //pReport.RWY = ComboBox001.Text;

            if (CheckBox301.Checked)
                pReport.Category = ComboBox401.Text;
            else
                pReport.Category = "";

            SaveAccuracy(RepFileName, RepFileTitle, pReport);
            SaveLog(RepFileName, RepFileTitle, pReport);
            SaveProtocol(RepFileName, RepFileTitle, pReport);

            if (!CheckBox301.Checked)               //if (CurrPage > 3)
            {
                ModellingTNA = TNAH + DER.pPtPrj[eRWY.PtDER].Z;
                appliedPDG = CurrPDG;

                Trace[0] = AddSegmentFrm.CreateInitialSegment(DER, (IPolygon)pPolygon, appliedPDG, DepDir, TNAH);
                TSC = 1;
            }

            SaveGeometry(RepFileName, RepFileTitle, pReport);

			DBModule.pObjectDir.SaveAsXml(RepFileName + "_InputData.xml");

			if (!SaveProcedure(RepFileTitle))
                return;

            saveReportToDB();
            saveScreenshotToDB();
            this.Close();
        }

        private void saveReportToDB()
        {
            saveReportToDB(OmniLogRep, FeatureReportType.Log);
            saveReportToDB(OmniProtRep, FeatureReportType.Protocol);
            saveReportToDB(OmniGeomRep, FeatureReportType.Geometry);
        }

        private void saveReportToDB(ReportFile rp, FeatureReportType type)
        {
            if (rp.IsFinished)
            {
                FeatureReport report = new FeatureReport();
                report.Identifier = _Procedure.Identifier;
                report.ReportType = type;
                report.HtmlZipped = rp.Report;
                DBModule.pObjectDir.SetFeatureReport(report);
            }
        }

        private void saveScreenshotToDB()
        {
            Screenshot screenshot = new Screenshot();
            screenshot.DateTime = DateTime.Now;
            screenshot.Identifier = _Procedure.Identifier;
            screenshot.Images = screenCapture.Commit(_Procedure.Identifier);
            DBModule.pObjectDir.SetScreenshot(screenshot);
        }

        private void CancelBtn_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            IsClosing = true;
            this.Close();
        }

        #endregion

        int CalcZoneIIPDG(out double PDG)
        {
            //IClone pClone = (IClone)pPolygon;
            //IPolygon pPolygon1 = (IPolygon)pClone.Clone();
            //ITopologicalOperator2 pTopo = (ITopologicalOperator2)pPolygon1;
            //pTopo.IsKnownSimple_2 = false;
            //pTopo.Simplify();

            //Functions.ClassifyObstacles(oFullList, out oInnerList, out oOuterList, pPolygon1, DER, AztDir);

            PDG = PANS_OPS_DataBase.dpPDG_Nom.Value;

            int result = -1;
            int n = oInnerList.Parts.Length;

            for (int i = 0; i < n; i++)
            {
                if ((oInnerList.Parts[i].Dist > 0.0) && (Functions.SideDef(oInnerList.Parts[i].pPtPrj, DepDir - 90.0, DER.pPtPrj[eRWY.PtDER]) < 0))
                    oInnerList.Parts[i].MOC = 0.0;
                else
                    oInnerList.Parts[i].MOC = oInnerList.Parts[i].Dist * PANS_OPS_DataBase.dpMOC.Value;

                if (oInnerList.Parts[i].MOC > MOCLimit)
                    oInnerList.Parts[i].MOC = MOCLimit;

                double rH = oInnerList.Parts[i].Height + oInnerList.Parts[i].MOC - PANS_OPS_DataBase.dpOIS_abv_DER.Value;

                oInnerList.Parts[i].PDGToTop = (rH - oInnerList.Parts[i].MOC) / oInnerList.Parts[i].Dist;
                oInnerList.Parts[i].PDG = rH / oInnerList.Parts[i].Dist;
                oInnerList.Parts[i].Ignored = rH + PANS_OPS_DataBase.dpOIS_abv_DER.Value <= PANS_OPS_DataBase.dpPDG_60.Value;

                if (oInnerList.Parts[i].PDG > PDG && !oInnerList.Parts[i].Ignored)
                {
                    PDG = oInnerList.Parts[i].PDG;
                    result = i;
                }
            }
            return result;
        }

        private int CreatePolygon(double Dist, ref double PDG)
        {
            double d0 = DER.Length + DER.ClearWay - PANS_OPS_DataBase.dpT_Init.Value;
            dAr1 = (PANS_OPS_DataBase.dpNGui_Ar1.Value - PANS_OPS_DataBase.dpH_abv_DER.Value) / PDG;

            double d1_ = dAr1 / System.Math.Cos(GlobalVars.DegToRadValue * PANS_OPS_DataBase.dpOD1_ZoneAdjA.Value);
            double d2_ = Dist / System.Math.Cos(GlobalVars.DegToRadValue * PANS_OPS_DataBase.dpOD2_ZoneAdjA.Value);

            IPoint ptB = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir + 180.0, d0);

            //Functions.DrawPointWithText(ptB, "ptb-0");
            //ptB = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtStart], DepDir, PANS_OPS_DataBase.dpT_Init.Value);
            //Functions.DrawPointWithText(ptB, "ptb-1");
            //Application.DoEvents();

            IPoint pt0 = Functions.PointAlongPlane(ptB, DepDir - 90.0, PANS_OPS_DataBase.dpT_Init_Wd.Value);
            IPoint pt7 = Functions.PointAlongPlane(ptB, DepDir + 90.0, PANS_OPS_DataBase.dpT_Init_Wd.Value);

            ptB = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir - 90.0, PANS_OPS_DataBase.dpT_Init_Wd.Value);
            IPoint pt6 = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir + 90.0, PANS_OPS_DataBase.dpT_Init_Wd.Value);

            IPoint pt2 = Functions.PointAlongPlane(ptB, DepDir - PANS_OPS_DataBase.dpOD1_ZoneAdjA.Value, d1_);
            IPoint pt5 = Functions.PointAlongPlane(pt6, DepDir + PANS_OPS_DataBase.dpOD1_ZoneAdjA.Value, d1_);

            IPoint pt3 = Functions.PointAlongPlane(pt2, DepDir - PANS_OPS_DataBase.dpOD2_ZoneAdjA.Value, d2_);
            IPoint pt4 = Functions.PointAlongPlane(pt5, DepDir + PANS_OPS_DataBase.dpOD2_ZoneAdjA.Value, d2_);

            if (pPolygon.PointCount > 0)
                pPolygon.RemovePoints(0, pPolygon.PointCount);

            pPolygon.AddPoint(pt0);
            pPolygon.AddPoint(ptB);
            pPolygon.AddPoint(pt2);
            pPolygon.AddPoint(pt3);
            pPolygon.AddPoint(pt4);
            pPolygon.AddPoint(pt5);
            pPolygon.AddPoint(pt6);
            pPolygon.AddPoint(pt7);

            IClone pClone = (IClone)pPolygon;
            IPolygon pPolygon1 = (IPolygon)pClone.Clone();
            ITopologicalOperator2 pTopo = (ITopologicalOperator2)pPolygon1;
            pTopo.IsKnownSimple_2 = false;
            pTopo.Simplify();
            if (CurrPage > 3)
                return 0;

            Functions.ClassifyObstacles(oFullList, out oInnerList, out oOuterList, pPolygon1, DER, DepDir);
            return CalcZoneIIPDG(out PDG);
        }

        private void AdjustI_Zone(double PDG)
        {
            dAr1 = (PANS_OPS_DataBase.dpNGui_Ar1.Value - PANS_OPS_DataBase.dpOIS_abv_DER.Value) / PDG;
            dAr2 = GlobalVars.RModel;
            double d0 = dAr1 / System.Math.Cos(GlobalVars.DegToRadValue * PANS_OPS_DataBase.dpOD1_ZoneAdjA.Value);

            IPoint pt2 = Functions.PointAlongPlane(pPolygon.Point[1], DepDir - PANS_OPS_DataBase.dpOD1_ZoneAdjA.Value, d0);
            IPoint pt5 = Functions.PointAlongPlane(pPolygon.Point[6], DepDir + PANS_OPS_DataBase.dpOD1_ZoneAdjA.Value, d0);

            pPolygon.ReplacePoints(2, 1, 1, ref pt2);
            pPolygon.ReplacePoints(5, 1, 1, ref pt5);
            AdjustII_Zone();
        }

        private void AdjustII_Zone()
        {
            double d0 = (dAr2 + dAr2addend) / System.Math.Cos(GlobalVars.DegToRadValue * PANS_OPS_DataBase.dpOD2_ZoneAdjA.Value);

            IPoint pt3 = Functions.PointAlongPlane(pPolygon.Point[2], DepDir - PANS_OPS_DataBase.dpOD2_ZoneAdjA.Value, d0);
            IPoint pt4 = Functions.PointAlongPlane(pPolygon.Point[5], DepDir + PANS_OPS_DataBase.dpOD2_ZoneAdjA.Value, d0);

            pPolygon.ReplacePoints(3, 1, 1, ref pt3);
            pPolygon.ReplacePoints(4, 1, 1, ref pt4);

            IPolygon pPolygon1 = Functions.PolygonIntersection(pPolygon, pCircle);
            Functions.DeleteGraphicsElement(StraightAreaElem);
            StraightAreaElem = Functions.DrawPolygon(pPolygon1, Functions.RGB(0, 0, 255));
            StraightAreaElem.Locked = true;

            IClone pClone = (IClone)pPolygon;
            pPolygon1 = (IPolygon)pClone.Clone();
            ITopologicalOperator2 pTopo = (ITopologicalOperator2)pPolygon1;
            pTopo.IsKnownSimple_2 = false;
            pTopo.Simplify();

            double PDG;
            Functions.ClassifyObstacles(oFullList, out oInnerList, out oOuterList, pPolygon1, DER, DepDir);
            CalcZoneIIPDG(out PDG);

            RadialateObs(oOuterList.Parts);

            if (IsSectorized && CurrPage > 1)
                ExcludeObs(oOuterList.Parts);
        }

        public double CalcStraightReqTNAH(double PDG, double DistRet33, ref int index, bool EntireFlag = false)
        {
            index = -1;
            dAr2 = 0.0;

            int n = oInnerList.Parts.Length;
            if (n <= 0)
                return PANS_OPS_DataBase.dpNGui_Ar1.Value;

            double fTmp, fTmpH;
            double X, TNIA_Bound;
            double TNIA_MOC_Bound = PANS_OPS_DataBase.dpObsClr.Value / PANS_OPS_DataBase.dpMOC.Value;
            double lhMax = 0.0;

            for (int i = 0; i < n; i++)
            {
                if (oInnerList.Parts[i].Dist > TNIA_MOC_Bound)
                    oInnerList.Parts[i].ReqTNH = PANS_OPS_DataBase.dpNGui_Ar1.Value;
                else
                {
                    if ((PDG == PANS_OPS_DataBase.dpPDG_Nom.Value) || EntireFlag)
                    {
                        fTmp = oInnerList.Parts[i].Height + PANS_OPS_DataBase.dpObsClr.Value;
                        fTmpH = PANS_OPS_DataBase.dpOIS_abv_DER.Value + PDG * oInnerList.Parts[i].Dist;
                        if (fTmpH >= fTmp)
                            fTmp = 0.0;
                    }
                    else
                    {
                        X = (PANS_OPS_DataBase.dpObsClr.Value - PANS_OPS_DataBase.dpOIS_abv_DER.Value + oInnerList.Parts[i].Height - PANS_OPS_DataBase.dpPDG_Nom.Value * oInnerList.Parts[i].Dist) / (PDG - PANS_OPS_DataBase.dpPDG_Nom.Value);

                        if (X >= oInnerList.Parts[i].Dist)
                            fTmp = oInnerList.Parts[i].Height + PANS_OPS_DataBase.dpObsClr.Value;
                        else
                            fTmp = PANS_OPS_DataBase.dpOIS_abv_DER.Value + PDG * X;
                    }

                    oInnerList.Parts[i].ReqTNH = Math.Max(fTmp, PANS_OPS_DataBase.dpNGui_Ar1.Value);

                    if (lhMax < fTmp)
                    {
                        lhMax = fTmp;
                        index = i;
                    }
                }
            }

            double H233 = PANS_OPS_DataBase.dpOIS_abv_DER.Value + DistRet33 * PDG;

            if (lhMax < PANS_OPS_DataBase.dpNGui_Ar1.Value)
            {
                lhMax = PANS_OPS_DataBase.dpNGui_Ar1.Value;
                index = -1;
            }

            if ((!EntireFlag) && (lhMax < H233))
            {
                lhMax = H233;
                index = n + 2;
            }

            TNIA_Bound = (lhMax - PANS_OPS_DataBase.dpOIS_abv_DER.Value) / PDG;
            dAr2 = TNIA_Bound - dAr1;
            return lhMax;
        }

        private double CalcTIARange(out int index)
        {
            SelectInterestingObs(oOuterList.Parts);
            //if( dTNAh <= 0.0)
            //    return 0.0;

            int i, j, n = oOuterList.Parts.Length;
            Solutions = new SquareSolutionArea[n];

            double MinFirst = GlobalVars.RModel + GlobalVars.RModel;
            double dAr12 = dAr1 + dAr2;
            //double dAr12 = dAr1 + dAr2 + dAr2addend;

            double DistMax = 0.0;
            double dr, alpha;

            SectorHeight = TIARequiredTNAH;
            index = j = -1;

            //FindSectorHeight = TIA_TNAH
            //For I = 0 To N
            //	If (ObsList(I).IsExcluded = True) And (ObsList(I).IsInteresting = True) And _
            //		//((PDG_TA * (ObsList(I).Height + dpMOC.Value * ObsList(I).Solution.Second) - dpMOC.Value * TIA_TNAH) / (PDG_TA - dpMOC.Value) > FindSectorHeight) Then
            //		FindSectorHeight = (PDG_TA * (ObsList(I).Height + dpMOC.Value * ObsList(I).Solution.Second) - dpMOC.Value * TIA_TNAH) / (PDG_TA - dpMOC.Value)
            //	End If
            //Next I

            for (i = 0; i < n; i++)
            {
                IPoint ptCurr = oOuterList.Parts[i].pPtPrj;

                //IPolyline pPolyline = new PolylineClass();
                //IPointCollection pPoints = (IPointCollection)pPolyline;
                //pPoints.AddPoint(Functions.PointAlongPlane(pPolygon.Point[3], DepDir - PANS_OPS_DataBase.dpOD2_ZoneAdjA.Value, 10000.0));
                //pPoints.AddPoint(pPolygon.Point[3]);
                //pPoints.AddPoint(Functions.PointAlongPlane(pPolygon.Point[3], DepDir - PANS_OPS_DataBase.dpOD2_ZoneAdjA.Value - 90.0, 10000.0));

                //Functions.DrawPointWithText(pPolygon.Point[4], "Point[4]");
                //Functions.DrawPolyline(pPolyline, 255,2);
                //Application.DoEvents();


                if ((Functions.SideDef(pPolygon.Point[3], DepDir - PANS_OPS_DataBase.dpOD2_ZoneAdjA.Value, ptCurr) > 0) && (Functions.SideDef(pPolygon.Point[3], DepDir - PANS_OPS_DataBase.dpOD1_ZoneAdjA.Value - 90.0, ptCurr) < 0))
                {
                    dr = Functions.ReturnDistanceInMeters(pPolygon.Point[3], ptCurr);
                    //Functions.DrawPointWithText(ptCurr, "Obst");
                    //Functions.DrawPointWithText(pPolygon.Point[3], "Point[3]");
                    //do
                    //Application.DoEvents();

                    alpha = (DepDir - PANS_OPS_DataBase.dpOD2_ZoneAdjA.Value) - Functions.ReturnAngleInDegrees(pPolygon.Point[3], ptCurr);
                    Solutions[i] = Functions.CalcZeroDTNAH(oOuterList.Parts[i].hPenet, CurrPDG, TACurrPDG, alpha, PANS_OPS_DataBase.dpOD2_ZoneAdjA.Value, dr, oOuterList.Parts[i].MOC, dAr12, MOCLimit);
                }
                else if ((Functions.SideDef(pPolygon.Point[4], DepDir + PANS_OPS_DataBase.dpOD2_ZoneAdjA.Value, ptCurr) < 0) && (Functions.SideDef(pPolygon.Point[4], DepDir + PANS_OPS_DataBase.dpOD1_ZoneAdjA.Value + 90.0, ptCurr) > 0))
                {
                    //Functions.DrawPointWithText(ptCurr, "Obst");
                    //Functions.DrawPointWithText(pPolygon.Point[4], "Point[4]");
                    //do
                    //Application.DoEvents();
                    //while (true) ;

                    dr = Functions.ReturnDistanceInMeters(pPolygon.Point[4], ptCurr);
                    alpha = Functions.ReturnAngleInDegrees(pPolygon.Point[4], ptCurr) - (DepDir + PANS_OPS_DataBase.dpOD2_ZoneAdjA.Value);
                    Solutions[i] = Functions.CalcZeroDTNAH(oOuterList.Parts[i].hPenet, CurrPDG, TACurrPDG, alpha, PANS_OPS_DataBase.dpOD2_ZoneAdjA.Value, dr, oOuterList.Parts[i].MOC, dAr12, MOCLimit);
                }
                else if (Functions.SideDef(pPolygon.Point[4], DepDir - 90.0, ptCurr) > 0)
                {
                    double Dist = oOuterList.Parts[i].hPenet / CurrPDG;
                    double ObsMOC = PANS_OPS_DataBase.dpMOC.Value * (oOuterList.Parts[i].Dist + oOuterList.Parts[i].DistStar);

                    if (ObsMOC > MOCLimit)
                        ObsMOC = MOCLimit;

                    Solutions[i].Second_Renamed = Dist;
                    Solutions[i].Solutions = 2;

                    if (ObsMOC > PANS_OPS_DataBase.dpObsClr.Value)
                        Solutions[i].Second_Renamed = (oOuterList.Parts[i].Height + ObsMOC - TIARequiredTNAH - oOuterList.Parts[i].Dist * TACurrPDG) / CurrPDG;
                }

                if (!oOuterList.Parts[i].IsExcluded)
                {
                    if (Solutions[i].Solutions == 3 && Solutions[i].First < MinFirst)
                    {
                        MinFirst = Solutions[i].First;
                        j = i;
                    }

                    if (Solutions[i].Second_Renamed > DistMax)
                    {
                        DistMax = Solutions[i].Second_Renamed;
                        index = i;
                    }
                }
                else if (oOuterList.Parts[i].IsInteresting)
                {
                    double fTmp = (TACurrPDG * (oOuterList.Parts[i].Height + PANS_OPS_DataBase.dpMOC.Value * Solutions[i].Second_Renamed) - PANS_OPS_DataBase.dpMOC.Value * TIARequiredTNAH) / (TACurrPDG - PANS_OPS_DataBase.dpMOC.Value);
                    if (fTmp > SectorHeight)
                        SectorHeight = fTmp;
                }
            }

            bool MaxSolution = MinFirst > GlobalVars.RModel;
            if (!MaxSolution)
            {
                for (i = 0; i < n; i++)
                {
                    if (Solutions[i].Solutions == 2 && !oOuterList.Parts[i].IsExcluded)
                    {
                        MaxSolution = Solutions[i].Second_Renamed > MinFirst;
                        if (MaxSolution)
                            break;
                    }
                }
            }

            if (!MaxSolution)
            {
                DistMax = MinFirst;
                index = j;
            }

            return DistMax;
        }

        private double CalcTNAHshortTA(double lhMax, ref int index)
        {
            double maxTNAHShortage = 0.0;
            index = -1;

            int n = oOuterList.Parts.Length;
            for (int i = 0; i < n; i++)
            {
                if (!oOuterList.Parts[i].IsExcluded)
                {
                    double ObsMOC = PANS_OPS_DataBase.dpMOC.Value * (oOuterList.Parts[i].DistStar + oOuterList.Parts[i].Dist);

                    if (ObsMOC > MOCLimit)
                        ObsMOC = MOCLimit;

                    if (ObsMOC < PANS_OPS_DataBase.dpObsClr.Value)
                        ObsMOC = PANS_OPS_DataBase.dpObsClr.Value;

                    double TNAHShortage = oOuterList.Parts[i].Height + ObsMOC - TACurrPDG * oOuterList.Parts[i].Dist - lhMax;

                    oOuterList.Parts[i].MOC = ObsMOC;
                    oOuterList.Parts[i].hPenet = TNAHShortage;

                    //         If TNAHShortage > 0.0 Then
                    //             MsgBox  "dAr12     = " + CStr(System.Math.Round(dAr12)) + vbCrLf + _
                    // '                                  "TACurrPDG = " + CStr(System.Math.Round(100# * TACurrPDG, 2)) + vbCrLf + _
                    // '                                  "ID        = " + PtOutList(I).ID + vbCrLf + _
                    // '                                  "Dist      = " + CStr(System.Math.Round(PtOutList(I).Dist))
                    //         End If

                    if (TNAHShortage > maxTNAHShortage)
                    {
                        maxTNAHShortage = TNAHShortage;
                        index = i;
                    }

                    oOuterList.Parts[i].PDGToTop = TNAHShortage / oOuterList.Parts[i].Dist;
                }
            }

            return maxTNAHShortage;
        }

        private void ComboBox003_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!bFormInitialised)
                return;

            int k = ComboBox003.SelectedIndex;
            if (k < 0)
                return;
            MOCLimit = GlobalVars.EnrouteMOCValues[k];// 			MOCLimit = Functions.DeConvertHeight(double.Parse(ComboBox003.Text));

            RMin = 50.0 * System.Math.Round(0.02 * 600.0 / PANS_OPS_DataBase.dpPDG_Nom.Value + 0.4999);

            double minR = 50.0 * System.Math.Round(0.02 * MOCLimit / PANS_OPS_DataBase.dpMOC.Value + 0.4999);
            if (RMin < minR)
                RMin = minR;

            double OldR;

            if (double.TryParse(TextBox003.Text, out OldR))
                OldR = Functions.DeConvertDistance(OldR);
            else
                OldR = -1.0;

            if (OldR < RMin)
            {
                TextBox003.Tag = "";
                TextBox003.Text = Functions.ConvertDistance(RMin, eRoundMode.CEIL).ToString();
                TextBox003_Validating(TextBox003, new System.ComponentModel.CancelEventArgs());
            }
        }

        private void ComboBox004_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            //if (!(bFormInitialised))
            //    return;

            //ComboBox001.Items.Clear();
            //I = ComboBox004.SelectedIndex;

            //if (I < 0)
            //	return;

            //CurrADHP = GlobalVars.ADHPList[i];

            //DBModule.FillADHPFields(ref CurrADHP);
            //if (CurrADHP.pPtGeo == null)
            //{
            //	MessageBox.Show("Error reading ADHP.", "PANDA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            //int i, n;

            //GlobalVars.RModel = 0.0;
            //RMin = 50.0 * System.Math.Round(0.02 * 600.0 / PANS_OPS_DataBase.dpPDG_Nom.Value + 0.4999);
            //if (RMin < 20000.0)
            //    RMin = 20000.0;

            //RMax = 50.0 * System.Math.Round((0.02 * 800.0) / PANS_OPS_DataBase.dpMOC.Value + 0.4999);
            //if (RMin > RMax)
            //    RMax = RMin + 5000.0;

            //DBModule.FillRWYList(out RWYList, GlobalVars.CurrADHP);

            //n = RWYList.GetUpperBound(0);

            //if (n < 0)
            //{
            //	MessageBox.Show(Resources.str15056, "PANDA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            //DBModule.FillWPT_FIXList(out GlobalVars.WPTList, GlobalVars.CurrADHP, GlobalVars.MaxNAVDist);
            //DBModule.FillNavaidList(out GlobalVars.NavaidList, out GlobalVars.DMEList, GlobalVars.CurrADHP, GlobalVars.MaxNAVDist);

            //for (i = 0; i <= n; i++)
            //    ComboBox001.Items.Add(RWYList[i].Name);

            //ComboBox001.SelectedIndex = 0;
        }

        private void CheckBox101_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!bFormInitialised)
                return;

            string oldVal = TextBox101.Text;

            if (CheckBox101.Checked)
            {
                TextBox206.Text = TextBox101.Text;
                TextBox206.ReadOnly = false;

                TextBox101.ReadOnly = true;
                TextBox101.Text = (MinPDG * 100.0 + 0.0499999).ToString("0.0");

                Label101.Text = Resources.str15001;     // "PDG ???? ?????:"
                OptionButton202.Text = Label101.Text;
                OptionButton202.Checked = true;
                OptionButton201.Enabled = false;
            }
            else
            {
                TextBox206.ReadOnly = true;
                TextBox206.Text = (PANS_OPS_DataBase.dpPDG_Nom.Value * 100.0).ToString();

                TextBox101.ReadOnly = false;

                Label101.Text = Resources.str15002;         // "PDG ? ???:"
                OptionButton202.Text = Resources.str15003;  // "PDG ? ??:"
                OptionButton201.Checked = true;
                OptionButton201.Enabled = true;
            }

            ObstacleData ObsDPDG = new ObstacleData();

            if (oldVal != TextBox101.Text)
            {
                drPDGMax = Functions.dPDGMax(oInnerList.Parts, CurrPDG, out idPDGMax);

                TIARequiredTNAH = CalcStraightReqTNAH(CurrPDG, drPDGMax, ref iTnaH, false);

                if (iTnaH > oInnerList.Parts.Length)
                    iTnaH = idPDGMax;

                //     TextBox103.Text = CStr(ConvertHeight(TIARequiredTNAH, 3))
                TextBox107.Text = Functions.ConvertHeight(TIARequiredTNAH + DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.CEIL).ToString();
                TextBox108.Text = Functions.ConvertHeight(TIARequiredTNAH, eRoundMode.CEIL).ToString();

                if (iPDGToTop > -1)
                    TextBox104.Text = oInnerList.Obstacles[oInnerList.Parts[iPDGToTop].Owner].UnicalName;
                else
                    TextBox104.Text = Resources.str39014;

                if (idPDGMax > -1)
                    TextBox105.Text = oInnerList.Obstacles[oInnerList.Parts[idPDGMax].Owner].UnicalName;
                else
                    TextBox105.Text = Resources.str39014;

                if (iTnaH > -1)
                    TextBox106.Text = oInnerList.Obstacles[oInnerList.Parts[iTnaH].Owner].UnicalName;
                else
                    TextBox106.Text = Resources.str39014;

                TextBox102.Text = Functions.ConvertDistance(drPDGMax, eRoundMode.CEIL).ToString();
                //     TextBox204 = CStr(CurrPDG * 100.0)
                if (iPDGToTop > -1)
                    ObsDPDG = oInnerList.Parts[iPDGToTop];
                else
                    ObsDPDG.Owner = -1; // Resources.str39014;

                ReportsFrm.FillPage1(oInnerList, CurrPDG, ObsDPDG, ComboBox001.Text, 1);
                Report = true;
                // =================================================================
                IPolygon pPolygon1 = Functions.PolygonIntersection(pPolygon, pCircle);
                Functions.DeleteGraphicsElement(StraightAreaElem);
                StraightAreaElem = Functions.DrawPolygon(pPolygon1, Functions.RGB(0, 0, 255));
                StraightAreaElem.Locked = true;
            }
        }

        private void CheckBox201_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            if (!bFormInitialised)
                return;

            IRgbColor pRGB = new RgbColor();
            ISimpleLineSymbol pLS = new SimpleLineSymbol();

            pRGB.RGB = Functions.RGB(0, 0, 255);
            pLS.Color = pRGB;
            pLS.Width = 1;
            pLS.Style = esriSimpleLineStyle.esriSLSSolid;

            ISimpleFillSymbol pSectorFS = new SimpleFillSymbol();
            pSectorFS.Outline = pLS;
            pSectorFS.Color = pRGB;
            IsSectorized = CheckBox201.Checked;

            if (IsSectorized)
            {
                if (LeftSectorsCount > 0)
                    SectorLeftDir = System.Math.Round(LeftSectors[LeftSectorsCount - 1].RightAngle + 0.4999);
                if (RightSectorsCount > 0)
                    SectorRightDir = System.Math.Round(RightSectors[RightSectorsCount - 1].LeftAngle - 0.4999);
                //         FindMaxSector PtOutList
                SectorPoly = SectorBetweenAngles(SectorRightDir, SectorLeftDir);

                Frame202.Enabled = true;
                UpDown209.Value = (decimal)Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], DepDir - SectorLeftDir);//System.Math.Round(
                UpDown210.Value = (decimal)Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], DepDir - SectorRightDir);//System.Math.Round(

                ExcludeObs(oOuterList.Parts);
                DrawExcludeSector();
            }
            else
            {
                Frame202.Enabled = false;
                //         TextBox209.Text = ""
                //         TextBox210.Text = ""

                IncludeObs(oOuterList.Parts);
                ClearSectorElems();
            }

            TACurrPDG = 0.0;
            TextBox206.Tag = "";
            OptionButton202.Checked = true;
            TextBox206_Validating(TextBox206, new System.ComponentModel.CancelEventArgs());
        }

        private void CheckBox301_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!bFormInitialised)
                return;

            Label315.Visible = !CheckBox301.Checked;

            if (CheckBox301.Checked)
            {
                MultiPage1.TabPages[4].Visible = true;
                MultiPage1.TabPages[5].Visible = true;
                NextBtn.Enabled = true;
            }
            else
            {
                MultiPage1.TabPages[4].Visible = false;
                MultiPage1.TabPages[5].Visible = false;
                NextBtn.Enabled = false;
            }

            FocusStepCaption((MultiPage1.SelectedIndex));
        }

        private void ComboBox001_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!bFormInitialised)
                return;

            int RWYIndex = ComboBox001.SelectedIndex;

            if (RWYIndex < 0)
                return;

            DER = RWYList[RWYIndex];

            TextBox010.Text = Functions.ConvertHeight(DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.NEAREST).ToString();
            DepDir = DER.pPtPrj[eRWY.PtDER].M;

            TextBox002.Text = Functions.ConvertDistance(DER.Length, eRoundMode.NEAREST).ToString();

            double d0 = DER.Length + DER.ClearWay - PANS_OPS_DataBase.dpT_Init.Value;
            ptCenter = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir + 180.0, d0);

            // ================================
            if (ComboBox002.SelectedIndex < 0)
                ComboBox002.SelectedIndex = 0;
            else
                ComboBox002_SelectedIndexChanged(ComboBox002, new System.EventArgs());

            TextBox003.Text = "0";
            if (ComboBox003.SelectedIndex < 0)
                ComboBox003.SelectedIndex = 0;
            else
                ComboBox003_SelectedIndexChanged(ComboBox003, new System.EventArgs());
        }

        private void ComboBox002_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!bFormInitialised)
                return;

            int dmsMode = ComboBox002.SelectedIndex;
            if (dmsMode < 0)
                dmsMode = 0;

            double fTmp = NativeMethods.Modulus(DER.pPtGeo[eRWY.PtDER].M - GlobalVars.CurrADHP.MagVar);

            TextBox001.Text = Functions.Degree2String(DER.pPtGeo[eRWY.PtDER].M, (Degree2StringMode)dmsMode);
            TextBox004.Text = Functions.Degree2String(fTmp, (Degree2StringMode)dmsMode);
        }

        private void ComboBox401_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!bFormInitialised)
                return;
            if (ComboBox401.SelectedIndex < 0)
                return;

            AirCat = ComboBox401.SelectedIndex;

            Label409.Text = Functions.ConvertSpeed(1.1 * Categories_DATABase.cVmaInter.Values[AirCat], eRoundMode.NEAREST).ToString();
            Label410.Text = Functions.ConvertSpeed(1.1 * Categories_DATABase.cVmaFaf.Values[AirCat], eRoundMode.NEAREST).ToString();
            TextBox401.Text = Functions.ConvertSpeed(1.1 * Categories_DATABase.cVmaFaf.Values[AirCat], eRoundMode.NEAREST).ToString();
            TextBox401_Validating(TextBox401, new System.ComponentModel.CancelEventArgs());
        }

        private void AddSegmentBtn_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            for (int i = 0; i < ListView501.Items.Count; i++)
                ListView501.Items[i].Selected = false;

            ReSelectTrace();

            if (TSC == MaxTraceSegments)
            {
                MessageBox.Show(Resources.str00151, null, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            this.Hide();

            AddSegmentFrm.CreateNextSegment(GlobalVars.CurrADHP, DER, fIAS, segPDG, DepDir, Trace[TSC - 1], this);

            //if (AddSegmentFrm.CreateNextSegment(GlobalVars.CurrADHP, DER, fIAS, segPDG, AztDir, Trace[TSC - 1], ref Trace[TSC]))
            //{
            //    TSC++;
            //    ReDrawTrace();
            //    ReListTrace();
            //    RemoveSegmentBtn.Enabled = true;
            //}

            //this.Show(GlobalVars.Win32Window);
        }

        public void DialogHook(int result, ref TraceSegment NSegment, double NewPDG = 0)
        {
            if (result == 1)
            {
                //NSegment.
                RemoveSegmentBtn.Enabled = true;
                Trace[TSC++] = NSegment;

                segPDG = NewPDG;
                ReDrawTrace();
                ReListTrace();
            }

            this.Show(GlobalVars.Win32Window);
        }

        private void RemoveSegmentBtn_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            screenCapture.Delete();
            for (int i = 0; i < ListView501.Items.Count; i++)
                ListView501.Items[i].Selected = false;

            ReSelectTrace();

            if (TSC > 1)
            {
                TSC--;

                ReDrawTrace();
                ReListTrace();
            }

            RemoveSegmentBtn.Enabled = TSC > 1;
        }

        private void SaveGeometryBtn_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            string RepFileName, RepFileTitle;
            ReportHeader pReport = new ReportHeader();

            if (Functions.ShowSaveDialog(out RepFileName, out RepFileTitle))
            {
                //pReport.Procedure = Resources.str11;
                pReport.Procedure = RepFileTitle;

                //pReport.RWY = ComboBox001.Text;
                pReport.Category = ComboBox401.Text;
                SaveGeometry(RepFileName, RepFileTitle, pReport);
            }
        }

        private void ListView501_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            ReSelectTrace();
        }

        private void MultiPage1_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (MultiPageBusy)
                return;
            MultiPageBusy = true;

            if (CurrPage > MultiPage1.SelectedIndex)
            {
                MultiPage1.SelectedIndex = CurrPage;
                PrevBtn_Click(PrevBtn, new System.EventArgs());
            }
            else if (CurrPage < MultiPage1.SelectedIndex)
            {
                MultiPage1.SelectedIndex = CurrPage;
                NextBtn_Click(NextBtn, new System.EventArgs());
            }

            MultiPageBusy = false;
        }


        private void OptionButton201_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!bFormInitialised)
                return;

            if (((RadioButton)eventSender).Checked)
            {
                TextBox206.ReadOnly = true;
                TextBox205.ReadOnly = false;

                Label204.Enabled = true;
                Label217.Enabled = true;
                TextBox204.Enabled = true;
            }
        }

        private void OptionButton202_CheckedChanged(object sender, EventArgs e)
        {
            if (!bFormInitialised)
                return;

            if (((RadioButton)sender).Checked)
            {
                TextBox206.ReadOnly = false;
                TextBox205.ReadOnly = true;

                Label204.Enabled = false;
                Label217.Enabled = false;
                TextBox204.Enabled = false;
            }
        }

        private void TextBox101_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            double fTmp;

            ObstacleData ObsDPDG;
            if (!double.TryParse(TextBox101.Text, out fTmp))
                return;

            if (TextBox101.Tag.ToString() == TextBox101.Text)
                return;

            fTmp *= 0.01;
            double fNew = fTmp;

            if (fNew < MinPDG)
                fNew = MinPDG;
            else if (fNew > PANS_OPS_DataBase.dpMaxPosPDG.Value * 50)
                fNew = PANS_OPS_DataBase.dpMaxPosPDG.Value * 50;

            if (fNew != fTmp)
                TextBox101.Text = (100.0 * fNew + 0.0499999).ToString("0.0");

            drPDGMax = Functions.dPDGMax(oInnerList.Parts, fNew, out idPDGMax);
            TIARequiredTNAH = CalcStraightReqTNAH(fNew, drPDGMax, ref iTnaH, false);

            if (iTnaH > oInnerList.Parts.Length)
                iTnaH = idPDGMax;

            //     TextBox103.Text = CStr(ConvertHeight(TIARequiredTNAH, 3))
            TextBox107.Text = Functions.ConvertHeight(TIARequiredTNAH + DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.CEIL).ToString();
            TextBox108.Text = Functions.ConvertHeight(TIARequiredTNAH, eRoundMode.CEIL).ToString();

            if (idPDGMax > -1)
                TextBox105.Text = oInnerList.Obstacles[oInnerList.Parts[idPDGMax].Owner].UnicalName;
            else
                TextBox105.Text = Resources.str39014;

            if (iTnaH > -1)
                TextBox106.Text = oInnerList.Obstacles[oInnerList.Parts[iTnaH].Owner].UnicalName;
            else
                TextBox106.Text = Resources.str39014;

            TextBox102.Text = Functions.ConvertDistance(drPDGMax, eRoundMode.CEIL).ToString();
            //     UpdateTAData
            //     UpdateSectorPoly

            if (iPDGToTop > -1)
                ObsDPDG = oInnerList.Parts[iPDGToTop];
            else
            {
                ObsDPDG = new ObstacleData();
                ObsDPDG.Owner = -1; // Resources.str39014;
            }

            ReportsFrm.FillPage1(oInnerList, CurrPDG, ObsDPDG, ComboBox001.Text, 1);
            Report = true;
            // =================================================================
            IPolygon pPolygon1 = Functions.PolygonIntersection(pPolygon, pCircle);
            Functions.DeleteGraphicsElement(StraightAreaElem);
            StraightAreaElem = Functions.DrawPolygon(pPolygon1, Functions.RGB(0, 0, 255));
            StraightAreaElem.Locked = true;

            TextBox101.Tag = TextBox101.Text;
        }

        private void TextBox101_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char eventChar = eventArgs.KeyChar;

            if (eventChar == 13)
                TextBox101_Validating(TextBox101, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, (TextBox101.Text));

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        private void TextBox205_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char eventChar = eventArgs.KeyChar;

            if (eventChar == 13)
                TextBox205_Validating(TextBox205, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox205.Text);

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        private void TextBox205_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            double d0, MaxReqPDG, ReqPDGi, fTmp;

            if (!double.TryParse(TextBox205.Text, out fTmp) || !OptionButton201.Checked)
                return;

            if (TextBox205.Tag.ToString() == TextBox205.Text)
                return;

            dAr2addend = Functions.DeConvertDistance(fTmp);
            AdjustII_Zone();
            CalcZoneIIPDG(out d0);

            int n = oOuterList.Parts.Length;
            d0 = dAr1 + dAr2 + dAr2addend;
            TNAH = CurrPDG * d0 + PANS_OPS_DataBase.dpOIS_abv_DER.Value;
            dTNAh = CalcTNAHshortTA(TNAH, ref iShortage);

            MaxReqPDG = PANS_OPS_DataBase.dpPDG_Nom.Value;
            int indx = -1;
            for (int i = 0; i < n; i++)
            {
                if (!oOuterList.Parts[i].IsExcluded)
                {
                    ReqPDGi = oOuterList.Parts[i].hPenet / oOuterList.Parts[i].Dist + TACurrPDG;
                    if (MaxReqPDG < ReqPDGi)
                    {
                        MaxReqPDG = ReqPDGi;
                        indx = i;
                    }
                }
            }

            TACurrPDG = MaxReqPDG;
            //TextBox206.Text = (0.1 * System.Math.Ceiling(1000.0 * TACurrPDG)).ToString();
            TextBox206.Text = (100.0 * TACurrPDG).ToString("0.0");

            //     dAr2addend = 0.0
            //     AdjustII_Zone()
            //     dAr2addend = DeConvertDistance(CDbl(TextBox205.Text))

            ReportsFrm.FillPage3(oOuterList, TACurrPDG, TNAH, MOCLimit);  //d0, 
            TextBox205.Tag = TextBox205.Text;
        }

        private void TextBox003_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char eventChar = eventArgs.KeyChar;

            if (eventChar == 13)
                TextBox003_Validating(TextBox003, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox003.Text);

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        private void TextBox003_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            double NewR, fTmp;

            if (double.TryParse(TextBox003.Text, out NewR))
            {
                if (TextBox003.Tag.ToString() == TextBox003.Text)
                    return;

                NewR = Functions.DeConvertDistance(NewR);
                fTmp = NewR;

                if (NewR < RMin) NewR = RMin;

                if (fTmp != NewR)
                    TextBox003.Text = Functions.ConvertDistance(NewR, eRoundMode.CEIL).ToString();

                TextBox003.Tag = TextBox003.Text;
                GlobalVars.RModel = NewR;

                double NewH;
                NewH = NewR * PANS_OPS_DataBase.dpPDG_Nom.Value;
                if (NewH < 600.0) NewH = 600.0;
                TextBox007.Text = Functions.ConvertHeight(NewH + PANS_OPS_DataBase.dpH_abv_DER.Value + DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.NEAREST).ToString();

                Functions.DeleteGraphicsElement(pCircleElem);

                //         If R = NewR Then Return
                pCircle = Functions.CreatePrjCircle(ptCenter, GlobalVars.RModel);
                //         GetObstacles PtInList, PtOutList, pCircle

                ISimpleFillSymbol pEmptyFillSym;
                ILineSymbol pRedLineSymbol;
                IColor pRedColor;

                pEmptyFillSym = new SimpleFillSymbol();
                pRedColor = new RgbColor();
                pRedColor.RGB = Functions.RGB(255, 0, 0);

                pRedLineSymbol = new SimpleLineSymbol();
                pRedLineSymbol.Color = pRedColor;
                pRedLineSymbol.Width = 2;

                pEmptyFillSym.Style = esriSimpleFillStyle.esriSFSNull;
                pEmptyFillSym.Outline = pRedLineSymbol;

                pCircleElem = Functions.DrawPolygonSFS(pCircle, pEmptyFillSym);
                pCircleElem.Locked = true;

                double MaxDist = DBModule.GetObstListInPoly(out oFullList, ptCenter, GlobalVars.RModel, DER.pPtPrj[eRWY.PtDER].Z);

                Label007.Text = Resources.str15052 + Functions.ConvertDistance(GlobalVars.RModel, eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit + ":";
                TextBox005.Text = oFullList.Obstacles.Length.ToString();
                TextBox006.Text = Functions.ConvertDistance(MaxDist, eRoundMode.FLOOR).ToString();
            }
            else if (double.TryParse(TextBox003.Tag.ToString(), out fTmp))
                TextBox003.Text = TextBox003.Tag.ToString();
            else
                TextBox003.Text = Functions.ConvertDistance(GlobalVars.RModel, eRoundMode.CEIL).ToString();
        }

        private void TextBox206_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char eventChar = eventArgs.KeyChar;

            if (eventChar == 13)
                TextBox206_Validating(TextBox206, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox206.Text);

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        bool TextBox206Busy = false;
        private void TextBox206_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            if (TextBox206Busy)
                return;

            //if (MultiPage1.SelectedIndex < 2 || (!OptionButton202.Checked && !IsSectorized))
            if (MultiPage1.SelectedIndex < 2 || !OptionButton202.Checked)
                return;

            double d0;
            if (!double.TryParse(TextBox206.Text, out d0))
                return;

            if (TextBox206.Tag.ToString() == TextBox206.Text)
                return;

            d0 *= 0.01;
            if (d0 == TACurrPDG)
                return;

            if (d0 > PANS_OPS_DataBase.dpMaxPosPDG.Value * 50.0)
            {
                d0 = PANS_OPS_DataBase.dpMaxPosPDG.Value * 50.0;

                if (TextBox206.Tag.ToString() == (100.0 * d0).ToString("0.0"))
                {
                    TextBox206.Text = TextBox206.Tag.ToString();
                    return;
                }

                TextBox206.Tag = (100.0 * d0).ToString("0.0");
                TextBox206.Text = TextBox206.Tag.ToString();
            }

            TextBox206Busy = true;
            TACurrPDG = d0;
            dAr2addend = 0.0;

            //int N = PtOutList.Length;

            if (CheckBox101.Checked)
            {
                if (TACurrPDG < MinPDG)
                {
                    TACurrPDG = MinPDG;
                    TextBox206.Text = (100.0 * TACurrPDG).ToString("0.0");
                }
                // =================================================================
                double NewPDG = TACurrPDG;

                do
                {
                    CurrPDG = NewPDG;
                    iPDGToTop = CreatePolygon(GlobalVars.RModel, ref NewPDG);
                }
                while (NewPDG < PANS_OPS_DataBase.dpMaxPosPDG.Value * 50.0 && CurrPDG < NewPDG);

                if (NewPDG >= PANS_OPS_DataBase.dpMaxPosPDG.Value * 50.0)
                {
                    MessageBox.Show(Resources.str15051 + (100.0 * TACurrPDG).ToString("0.0") + "%", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TextBox206Busy = false;
                    return;
                }

                TACurrPDG = CurrPDG;
                TextBox206.Text = (100.0 * TACurrPDG).ToString("0.0"); // + 0.0499999

                drPDGMax = Functions.dPDGMax(oInnerList.Parts, CurrPDG, out idPDGMax);
                TIARequiredTNAH = CalcStraightReqTNAH(CurrPDG, drPDGMax, ref iTnaH, CheckBox101.Checked);

                if (iTnaH > oInnerList.Parts.Length)
                    iTnaH = idPDGMax;

                //     TextBox103.Text = CStr(ConvertHeight(TIARequiredTNAH, 3))
                TextBox107.Text = Functions.ConvertHeight(TIARequiredTNAH + DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.CEIL).ToString();
                TextBox108.Text = Functions.ConvertHeight(TIARequiredTNAH, eRoundMode.CEIL).ToString();     // + DER.pPtPrj(ptEnd).Z - ptThrPrj.Z

                if (iPDGToTop > -1)
                    TextBox104.Text = oInnerList.Obstacles[oInnerList.Parts[iPDGToTop].Owner].UnicalName;
                else
                    TextBox104.Text = Resources.str39014;

                if (idPDGMax > -1)
                    TextBox105.Text = oInnerList.Obstacles[oInnerList.Parts[idPDGMax].Owner].UnicalName;
                else
                    TextBox105.Text = Resources.str39014;

                if (iTnaH > -1)
                    TextBox106.Text = oInnerList.Obstacles[oInnerList.Parts[iTnaH].Owner].UnicalName;
                else
                    TextBox106.Text = Resources.str39014;

                TextBox202.Text = Functions.ConvertDistance(dAr1 + dAr2, eRoundMode.CEIL).ToString();

                AdjustII_Zone();
                iPDGToTop = CalcZoneIIPDG(out NewPDG);

                dTNAh = CalcTNAHshortTA(TIARequiredTNAH, ref iShortage);
                ObstacleData ObsDPDG;

                if (iPDGToTop > -1)
                    ObsDPDG = oInnerList.Parts[iPDGToTop];
                else
                {
                    ObsDPDG = new ObstacleData();
                    ObsDPDG.Owner = -1; // Resources.str39014;
                }

                ReportsFrm.FillPage1(oInnerList, CurrPDG, ObsDPDG, ComboBox001.Text, 1);
                Report = true;
                // =================================================================
            }
            else
            {
                if (TACurrPDG < PANS_OPS_DataBase.dpPDG_Nom.Value)
                {
                    TACurrPDG = PANS_OPS_DataBase.dpPDG_Nom.Value;
                    TextBox206.Text = (TACurrPDG * 100.0).ToString("0.0"); // + 0.04999
                }
                AdjustII_Zone();
                dTNAh = CalcTNAHshortTA(TIARequiredTNAH, ref iShortage);
            }

            TextBox201.Text = Functions.ConvertHeight(dTNAh, eRoundMode.NEAREST).ToString();
            TextBox203.Text = Functions.ConvertHeight(TIARequiredTNAH + DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.CEIL).ToString();
            TextBox211.Text = Functions.ConvertHeight(TIARequiredTNAH, eRoundMode.CEIL).ToString(); // + DER.pPtPrj(ptEnd).Z - ptThrPrj.Z

            if (iShortage >= 0)
            {
                TextBox207.Text = oOuterList.Obstacles[oOuterList.Parts[iShortage].Owner].UnicalName;
                TextBox208.Text = Functions.ConvertHeight(oOuterList.Parts[iShortage].Height + oOuterList.Parts[iShortage].MOC, eRoundMode.NEAREST).ToString() + " / "
                                + Functions.ConvertHeight(TIARequiredTNAH + oOuterList.Parts[iShortage].Dist * TACurrPDG, eRoundMode.NEAREST).ToString();
            }
            else
            {
                TextBox207.Text = Resources.str39014;
                TextBox208.Text = "-";
            }

            if (CheckBox201.Checked)
                Frame201.Enabled = dTNAh > 0.0;
            else
                Frame201.Enabled = true;

            int i;
            dAr2addend = CalcTIARange(out i);
            TextBox205.Text = Functions.ConvertDistance(dAr2addend, eRoundMode.CEIL).ToString();

            AdjustII_Zone();
            d0 = dAr1 + dAr2 + dAr2addend;
            TNAH = CurrPDG * d0 + PANS_OPS_DataBase.dpOIS_abv_DER.Value;
            dTNAh = CalcTNAHshortTA(TNAH, ref iShortage);

            ReportsFrm.FillPage3(oOuterList, TACurrPDG, TNAH, MOCLimit, i);   //, d0
            TextBox206.Tag = TextBox206.Text;

            TextBox206Busy = false;
        }

        private void TextBox401_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char eventChar = eventArgs.KeyChar;

            if (eventChar == 13)
                TextBox401_Validating(TextBox401, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox401.Text);

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        private void TextBox401_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            double F;

            if (double.TryParse(TextBox401.Text, out F))
            {
                F = Functions.DeConvertSpeed(F);

                if (F < 1.1 * Categories_DATABase.cVmaInter.Values[AirCat])
                    F = 1.1 * Categories_DATABase.cVmaInter.Values[AirCat];
                else if (F > 1.1 * Categories_DATABase.cVmaFaf.Values[AirCat])
                    F = 1.1 * Categories_DATABase.cVmaFaf.Values[AirCat];

                fIAS = F;
                TextBox401.Text = Functions.ConvertSpeed(F, eRoundMode.NEAREST).ToString();
            }
            else if (double.TryParse(TextBox401.Tag.ToString(), out F))
                TextBox401.Text = TextBox401.Tag.ToString();
            else
                TextBox401.Text = Functions.ConvertSpeed(1.1 * Categories_DATABase.cVmaFaf.Values[AirCat], eRoundMode.NEAREST).ToString();

            TextBox401.Tag = TextBox401.Text;
        }

        private void TextBox403_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char eventChar = eventArgs.KeyChar;

            if (eventChar == 13)
                TextBox403_Validating(TextBox403, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox403.Text);

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        private void TextBox403_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            double fTmp;
            if (double.TryParse(TextBox403.Text, out fTmp))
            {
                if (TextBox403.Tag != null && (TextBox403.Tag.ToString() == TextBox403.Text))
                    return;

                fTmp *= 0.01;
                appliedPDG = fTmp;

                if (appliedPDG > PANS_OPS_DataBase.dpMaxPosPDG.Value * 50.0)
                    appliedPDG = PANS_OPS_DataBase.dpMaxPosPDG.Value * 50.0;

                if (appliedPDG < CurrPDG)
                    appliedPDG = CurrPDG;

                if (appliedPDG < TACurrPDG)
                    appliedPDG = TACurrPDG;

                if (fTmp != appliedPDG)
                    TextBox403.Text = (100 * appliedPDG).ToString("0.0");

                TextBox403.Tag = TextBox403.Text;
            }
            else if (double.TryParse(TextBox403.Tag.ToString(), out fTmp))
                TextBox403.Text = TextBox403.Tag.ToString();
            else if (TACurrPDG > CurrPDG)
                TextBox403.Text = (TACurrPDG * 100.0).ToString("0.0");
            else
                TextBox403.Text = (CurrPDG * 100.0).ToString("0.0");
        }

        private void UpDown209_ValueChanged(System.Object sender, System.EventArgs e)
        {
            if (!bFormInitialised)
                return;

            if (UpDown209.Tag != null && UpDown209.Tag.ToString() == "a")
                return;

            UpDown209.Tag = "a";

            double Min = NativeMethods.Modulus(DER.pPtGeo[eRWY.PtDER].M - MinSectAngle);
            double Max = NativeMethods.Modulus(DER.pPtGeo[eRWY.PtDER].M - MaxSectAngle);

            if (UpDown209.Value == 360)
                UpDown209.Value = 0;
            if (UpDown209.Value == -1)
                UpDown209.Value = 359;
            if (UpDown209.Value == (decimal)NativeMethods.Modulus(Max - 1))
                UpDown209.Value = System.Convert.ToDecimal(Max);
            if (UpDown209.Value == (decimal)NativeMethods.Modulus(Min + 1))
                UpDown209.Value = System.Convert.ToDecimal(Min);

            double fValue = DepDir - Functions.Azt2Dir(DER.pPtGeo[eRWY.PtDER], UpDown209.Value);
            if (fValue > 180.0)
                fValue = fValue - 360.0;

            if (fValue != SectorLeftDir)
            {
                SectorLeftDir = fValue;
                SectorPoly = SectorBetweenAngles(SectorRightDir, SectorLeftDir);

                ExcludeObs(oOuterList.Parts);
                DrawExcludeSector();
                //         UpdateTAData
                //         UpdateSectorPoly
                TACurrPDG = 0.0;
                TextBox206.Tag = "";
                TextBox206_Validating(TextBox206, new System.ComponentModel.CancelEventArgs());
            }

            UpDown209.Tag = "";
        }

        private void UpDown210_ValueChanged(System.Object sender, System.EventArgs e)
        {
            if (!bFormInitialised)
                return;

            if (UpDown210.Tag != null && UpDown210.Tag.ToString() == "a")
                return;

            UpDown210.Tag = "a";

            double Min = NativeMethods.Modulus(DER.pPtGeo[eRWY.PtDER].M + MinSectAngle);
            double Max = NativeMethods.Modulus(DER.pPtGeo[eRWY.PtDER].M + MaxSectAngle);

            if (UpDown210.Value == 360)
                UpDown210.Value = 0;
            if (UpDown210.Value == -1)
                UpDown210.Value = 359;
            if (UpDown210.Value == (decimal)NativeMethods.Modulus(Max + 1))
                UpDown210.Value = (decimal)Max;
            if (UpDown210.Value == (decimal)NativeMethods.Modulus(Min - 1))
                UpDown210.Value = (decimal)Min;

            double fValue = NativeMethods.Modulus(360.0 - Functions.Azt2Dir(DER.pPtGeo[eRWY.PtDER], UpDown210.Value) + DepDir);
            if (Math.Abs(fValue - SectorRightDir) < GlobalVars.degEps)
            {
                // MsgBox  "RightDir = " + CStr(fValue)
                SectorRightDir = fValue;
                SectorPoly = SectorBetweenAngles(SectorRightDir, SectorLeftDir);

                ExcludeObs(oOuterList.Parts);
                DrawExcludeSector();

                //         UpdateTAData
                //         UpdateSectorPoly

                TACurrPDG = 0.0;
                TextBox206.Tag = "";
                TextBox206_Validating(TextBox206, new System.ComponentModel.CancelEventArgs());
            }

            UpDown210.Tag = "";
        }

        private void IncludeObs(ObstacleData[] ObsList)
        {
            int n = ObsList.Length;
            for (int i = 0; i < n; i++)
                ObsList[i].IsExcluded = false;
        }

        private void ExcludeObs(ObstacleData[] ObsList)
        {
            int n = ObsList.Length;
            for (int i = 0; i < n; i++)
                ObsList[i].IsExcluded = (ObsList[i].SectorAngle >= SectorRightDir + SectorBufferAngle) || (ObsList[i].SectorAngle <= SectorLeftDir - SectorBufferAngle);
        }

        private IPointCollection SectorBetweenAngles(double Ang1, double Ang2)
        {
            IPoint Pt1 = Functions.PointAlongPlane(ptCenter, DepDir - Ang1, GlobalVars.RModel);
            IPoint Pt2 = Functions.PointAlongPlane(ptCenter, DepDir - Ang2, GlobalVars.RModel);

            IPointCollection result = Functions.CreateArcPrj(ptCenter, Pt2, Pt1, -1);
            result.AddPoint(ptCenter);
            result.AddFirstPoint(ptCenter);

            ITopologicalOperator2 pTopo = (ITopologicalOperator2)result;
            pTopo.IsKnownSimple_2 = false;
            pTopo.Simplify();
            return result;
        }

        private void DrawExcludeSector()
        {
            ClearSectorElems();

            int i;

            IRgbColor pRGB = new RgbColor();
            ISimpleLineSymbol pLS = new SimpleLineSymbol();

            pRGB.RGB = Functions.RGB(0, 0, 255);
            pLS.Color = pRGB;
            pLS.Width = 1;
            pLS.Style = esriSimpleLineStyle.esriSLSSolid;

            ISimpleFillSymbol pSectorFS = new SimpleFillSymbol();
            pSectorFS.Outline = pLS;
            pSectorFS.Color = pRGB;
            pSectorFS.Style = esriSimpleFillStyle.esriSFSHorizontal;

            for (i = 0; i < LeftSectorsCount; i++)
            {
                pRGB.RGB = LeftSectors[i].SectorColor;
                pSectorFS.Color = pRGB;

                if ((LeftSectors[i].RightAngle < SectorLeftDir))
                    pSectorElems[i] = Functions.DrawPolygonSFS(LeftSectors[i].SectorGraphics, pSectorFS);
                else
                {
                    pSectorElems[i] = Functions.DrawPolygonSFS(SectorBetweenAngles(SectorLeftDir, LeftSectors[i].LeftAngle), pSectorFS);
                    pSectorElems[i].Locked = true;
                    break;
                }

                pSectorElems[i].Locked = true;
            }

            for (i = 0; i < RightSectorsCount; i++)
            {
                pRGB.RGB = RightSectors[i].SectorColor;
                pSectorFS.Color = pRGB;

                if ((RightSectors[i].LeftAngle > SectorRightDir))
                    pSectorElems[LeftSectorsCount + i] = Functions.DrawPolygonSFS(RightSectors[i].SectorGraphics, pSectorFS);
                else
                {
                    pSectorElems[LeftSectorsCount + i] = Functions.DrawPolygonSFS(SectorBetweenAngles(RightSectors[i].RightAngle, SectorRightDir), pSectorFS);
                    pSectorElems[LeftSectorsCount + i].Locked = true;
                    break;
                }

                pSectorElems[LeftSectorsCount + i].Locked = true;
            }

            pSectorFS.Style = esriSimpleFillStyle.esriSFSNull;
            if (LeftSectorsCount > 0)
                if ((LeftSectors[LeftSectorsCount - 1].RightAngle < SectorLeftDir))
                {
                    pSectorElems[RightSectorsCount + LeftSectorsCount] = Functions.DrawPolygonSFS(SectorBetweenAngles(SectorLeftDir, LeftSectors[LeftSectorsCount - 1].RightAngle), pSectorFS);
                    pSectorElems[RightSectorsCount + LeftSectorsCount].Locked = true;
                }

            if (RightSectorsCount > 0)
                if ((RightSectors[RightSectorsCount - 1].LeftAngle > SectorRightDir))
                {
                    pSectorElems[RightSectorsCount + LeftSectorsCount + 1] = Functions.DrawPolygonSFS(SectorBetweenAngles(RightSectors[RightSectorsCount - 1].LeftAngle, SectorRightDir), pSectorFS);
                    pSectorElems[RightSectorsCount + LeftSectorsCount + 1].Locked = true;
                }
        }

        private void ClearSectorElems()
        {
            int n = pSectorElems.Length;
            for (int i = 0; i < n; i++)
            {
                if (pSectorElems[i] != null)
                {
                    Functions.DeleteGraphicsElement(pSectorElems[i]);
                    pSectorElems[i] = null;
                }
            }

            GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        public void SortObsByRadial(ObstacleData[] ObsList)
        {
            int n = ObsList.Length;
            if (n == 0)
                return;

            for (int i = 0; i < n - 1; i++)
                for (int j = i + 1; j < n; j++)
                    if ((ObsList[i].SectorAngle > ObsList[j].SectorAngle))
                    {
                        ObstacleData ObsTmp = ObsList[i];
                        ObsList[i] = ObsList[j];
                        ObsList[j] = ObsTmp;
                    }
        }

        public void SelectInterestingObs(ObstacleData[] ObsList)
        {
            int i, i1 = -1, i2 = -1, n = ObsList.Length;

            for (i = 0; i < n; i++)
            {
                ObsList[i].IsInteresting = false;
                if ((ObsList[i].SectorAngle > -MinSectAngle - SectorBufferAngle) && (ObsList[i].SectorAngle < 0.0))
                {
                    if (i1 == -1)
                        i1 = i - 1;
                }
                else if (ObsList[i].SectorAngle > MinSectAngle + SectorBufferAngle)
                {
                    i2 = i;
                    break;
                }
            }

            if (i2 >= 0)
                for (i = i2; i < n; i++)
                    ObsList[i].IsInteresting = false;

            if (i1 >= 0)
            {
                double MinPent = 0.0;
                for (i = i1; i >= 0; i--)
                {
                    if (ObsList[i].hPenet > MinPent)
                    {
                        MinPent = ObsList[i].hPenet;
                        ObsList[i].IsInteresting = true;
                    }
                }
            }

            if (i2 >= 0)
            {
                double MinPent = 0.0;
                for (i = i2; i < n; i++)
                {
                    if (ObsList[i].hPenet > MinPent)
                    {
                        MinPent = ObsList[i].hPenet;
                        ObsList[i].IsInteresting = true;
                    }
                }
            }
        }

        public void RadialateObs(ObstacleData[] ObsList)
        {
            int n = ObsList.Length;

            for (int i = 0; i < n; i++)
            {
                ObsList[i].SectorAngle = NativeMethods.Modulus(DepDir - Functions.ReturnAngleInDegrees(ptCenter, ObsList[i].pPtPrj));
                if ((ObsList[i].SectorAngle > 180.0))
                    ObsList[i].SectorAngle = ObsList[i].SectorAngle - 360.0;
            }
        }

        public double FindSectorHeight(ObstacleData[] ObsList, double TIA_TNAH, double PDG_TA)
        {
            int n = ObsList.Length;
            double result = TIA_TNAH;

            for (int i = 0; i < n; i++)
            {
                if (ObsList[i].IsExcluded && ObsList[i].IsInteresting)
                {
                    double fTmp = (PDG_TA * (ObsList[i].Height + PANS_OPS_DataBase.dpMOC.Value * Solutions[i].Second_Renamed) - PANS_OPS_DataBase.dpMOC.Value * TIA_TNAH) / (PDG_TA - PANS_OPS_DataBase.dpMOC.Value);
                    if (fTmp > result)
                        result = fTmp;
                }
            }
            return result;
        }

        public void FindProSectors(ObstacleData[] ObsList)
        {
            LeftSectorsCount = RightSectorsCount = 0;
            int n = ObsList.Length;

            if (n == 0)
            {
                LeftSectors = new ProhibitedSector[0];
                RightSectors = new ProhibitedSector[0];
                return;
            }

            LeftSectors = new ProhibitedSector[n + 1];
            RightSectors = new ProhibitedSector[n + 1];

            LeftSectors[0].LeftAngle = -180.0;
            LeftSectors[0].RightAngle = -MaxSectAngle;

            RightSectors[0].LeftAngle = MaxSectAngle;
            RightSectors[0].RightAngle = 180.0;

            LeftSectorsCount = RightSectorsCount = 1;

            for (int i = 0; i < n; i++)
            {
                if ((ObsList[i].IsInteresting) && (ObsList[i].SectorAngle + SectorBufferAngle < -MinSectAngle))
                {
                    LeftSectors[LeftSectorsCount].RightAngle = System.Math.Round(ObsList[i].SectorAngle + SectorBufferAngle + 0.4999);
                    LeftSectors[LeftSectorsCount].LeftAngle = LeftSectors[LeftSectorsCount - 1].RightAngle;
                    LeftSectors[LeftSectorsCount].Obstacle = ObsList[i];
                    LeftSectorsCount++;
                }
            }

            for (int i = n - 1; i >= 0; i--)
            {
                if ((ObsList[i].IsInteresting) && (ObsList[i].SectorAngle - SectorBufferAngle > MinSectAngle))
                {
                    RightSectors[RightSectorsCount].LeftAngle = System.Math.Round(ObsList[i].SectorAngle - SectorBufferAngle - 0.4999);
                    RightSectors[RightSectorsCount].RightAngle = RightSectors[RightSectorsCount - 1].LeftAngle;
                    RightSectors[RightSectorsCount].Obstacle = ObsList[i];
                    RightSectorsCount++;
                }
            }

            if (LeftSectorsCount == 1 && RightSectorsCount == 1)
            {
                LeftSectorsCount = RightSectorsCount = 0;
                LeftSectors = new ProhibitedSector[0];
                RightSectors = new ProhibitedSector[0];
                return;
            }

            System.Array.Resize(ref LeftSectors, LeftSectorsCount);
            System.Array.Resize(ref RightSectors, RightSectorsCount);

            for (int i = 0; i < LeftSectorsCount; i++)
            {
                LeftSectors[i].SectorGraphics = SectorBetweenAngles(LeftSectors[i].RightAngle, LeftSectors[i].LeftAngle);
                LeftSectors[i].SectorColor = Functions.RGB(255 * (uint)(i & 1), 255 * (uint)(i & 2), 255 * (uint)(i & 4));
            }

            for (int i = 0; i < RightSectorsCount; i++)
            {
                RightSectors[i].SectorGraphics = SectorBetweenAngles(RightSectors[i].RightAngle, RightSectors[i].LeftAngle);
                RightSectors[i].SectorColor = Functions.RGB(255 * (uint)(i & 1), 255 * (uint)(i & 2), 255 * (uint)(i & 4));
            }
        }

        //private void UpdateSectorPoly(){
        //	SectorPoly = SectorBetweenAngles(SectorRightDir, SectorLeftDir);
        //}

        //private TraceSegment CreateInitialSegment(RWYType DER, double fPDG, double DepDir, double TNAH, IPolygon pPolygon)
        //{
        //	TraceSegment result = new TraceSegment();

        //	result.HStart = DER.pPtPrj[eRWY.PtDER].Z + PANS_OPS_DataBase.dpH_abv_DER.Value;
        //	result.HFinish = ModellingTNA;
        //	result.PDG = fPDG;
        //	result.DirIn = DepDir;
        //	result.DirOut = result.DirIn;

        //	result.ptIn = DER.pPtPrj[eRWY.PtDER];
        //	result.ptOut = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir, (result.HFinish - result.HStart) / fPDG);

        //	//Functions.DrawPointWithText(result.ptIn, "ptIn");
        //	//Functions.DrawPointWithText(result.ptOut, "ptOut");
        //	//Application.DoEvents();

        //	result.PathPrj = (IPolyline)new Polyline();
        //	((IPointCollection)result.PathPrj).AddPoint(result.ptIn);
        //	((IPointCollection)result.PathPrj).AddPoint(result.ptOut);

        //	IPolyline pPolyline = result.PathPrj as IPolyline;
        //	result.Length = pPolyline.Length;

        //	//Functions.DrawPolyline(result.PathPrj, 255,2);
        //	//Application.DoEvents();

        //	IClone pClone = (IClone)pPolygon;
        //	result.pProtectArea = (IPolygon)pClone.Clone();
        //	ITopologicalOperator2 pTopo = (ITopologicalOperator2)result.pProtectArea;
        //	pTopo.IsKnownSimple_2 = false;
        //	pTopo.Simplify();
        //	result.Comment = Resources.str00153; // "Начальный участок"
        //	result.RepComment = result.Comment;

        //	//Functions.DrawPolygon(result.pProtectArea, -1, esriSimpleFillStyle.esriSFSCross);
        //	//Application.DoEvents();

        //	result.SegmentCode = eSegmentType.straight;
        //	result.LegType = Aim.Enums.CodeSegmentPath.VA;
        //	result.GuidanceNav.TypeCode = eNavaidType.NONE;
        //	result.InterceptionNav.TypeCode = eNavaidType.NONE;

        //	IPoint PtGeoSt = Functions.ToGeo(result.ptIn) as IPoint;
        //	IPoint PtGeoFin = Functions.ToGeo(result.ptOut) as IPoint;

        //	result.StCoords = Functions.Degree2String(System.Math.Abs(PtGeoSt.Y), Degree2StringMode.DMSLat) + ", " + Functions.Degree2String(System.Math.Abs(PtGeoSt.X), Degree2StringMode.DMSLon);
        //	result.FinCoords = Functions.Degree2String(System.Math.Abs(PtGeoFin.Y), Degree2StringMode.DMSLat) + ", " + Functions.Degree2String(System.Math.Abs(PtGeoFin.X), Degree2StringMode.DMSLon);

        //	return result;
        //}

        private void ReDrawTrace()
        {
            Functions.DeleteGraphicsElement(pTraceElem);
            Functions.DeleteGraphicsElement(pProtectElem);

            pTraceElem = null;
            pProtectElem = null;

            IPointCollection pAllTracks = new ESRI.ArcGIS.Geometry.Polyline();
            IPolygon pAllProtections = (IPolygon)new ESRI.ArcGIS.Geometry.Polygon();

            for (int i = 0; i < TSC; i++)
            {
                ITopologicalOperator2 pTopo = (ITopologicalOperator2)pAllProtections;
                IPolygon pTmpPoly = (IPolygon)pTopo.Union(Trace[i].pProtectArea);

                pTopo = (ITopologicalOperator2)pTmpPoly;
                pTopo.IsKnownSimple_2 = false;
                pTopo.Simplify();
                pAllProtections = pTmpPoly;

                pAllTracks.AddPointCollection((IPointCollection)Trace[i].PathPrj);
            }

            pTraceElem = Functions.DrawPolyline(pAllTracks, Functions.RGB(0, 0, 255), 1);
            pTraceElem.Locked = true;

            pProtectElem = Functions.DrawPolygon(pAllProtections, Functions.RGB(0, 255, 0));
            pProtectElem.Locked = true;
        }

        private void ReListTrace()
        {
            ListView501.Items.Clear();

            for (int i = 0; i < TSC; i++)
            {
                ListViewItem itmX = ListView501.Items.Add((i + 1).ToString());

                switch (Trace[i].SegmentCode)
                {
                    case eSegmentType.straight:                         // "Прямой сегмент"
                        itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, Resources.str00154));
                        break;
                    case eSegmentType.toHeading:                        // "На заданный курс"
                        itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, Resources.str00155));
                        break;
                    case eSegmentType.courseIntercept:                  // "На заданную WPT"
                        itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, Resources.str00156));
                        break;
                    case eSegmentType.directToFIX:                      // "???????? ?????"
                        itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, Resources.str00157));
                        break;
                    case eSegmentType.turnAndIntercept:                 // "На курс заданной РНС"
                        itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, Resources.str00158));
                        break;
                    case eSegmentType.arcIntercept:
                        itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, "Arc intercept"));
                        break;
                    case eSegmentType.arcPath:
                        itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, "Arc path"));    //	Resources.str160
                        break;
                }

                if (i == 0)
                    itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(Trace[i].HStart, eRoundMode.NEAREST).ToString() + " / " + Functions.ConvertHeight(Trace[i].HStart - DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.NEAREST).ToString()));
                else
                    itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(Trace[i].HStart, eRoundMode.NEAREST).ToString() + " / " + Functions.ConvertHeight(Trace[i].HStart - DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.NEAREST).ToString()));

                itmX.SubItems[2].Text = itmX.SubItems[2].Text + " " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;

                itmX.SubItems.Insert(3, new ListViewItem.ListViewSubItem(null, Functions.ConvertHeight(Trace[i].HFinish, eRoundMode.NEAREST).ToString() + " / " + Functions.ConvertHeight(Trace[i].HFinish - DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.NEAREST).ToString()));
                itmX.SubItems[3].Text = itmX.SubItems[3].Text + " " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;

                itmX.SubItems.Insert(4, new ListViewItem.ListViewSubItem(null, Functions.ConvertDistance(Trace[i].Length, eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit));
                itmX.SubItems.Insert(5, new ListViewItem.ListViewSubItem(null, Functions.ConvertDistance(Trace[i].Turn1R, eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit));
                itmX.SubItems.Insert(6, new ListViewItem.ListViewSubItem(null, (100.0 * Trace[i].PDG).ToString("0.00")));

                //itmX.SubItems.Insert(7, new ListViewItem.ListViewSubItem(null, NativeMethods.Modulus(Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], Trace[i].DirIn) - GlobalVars.CurrADHP.MagVar).ToString("0.00")));
                //itmX.SubItems.Insert(8, new ListViewItem.ListViewSubItem(null, NativeMethods.Modulus(Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], Trace[i].DirOut) - GlobalVars.CurrADHP.MagVar).ToString("0.00")));

                itmX.SubItems.Insert(7, new ListViewItem.ListViewSubItem(null, NativeMethods.Modulus(Functions.Dir2Azt(Trace[i].ptIn, Trace[i].DirIn) - GlobalVars.CurrADHP.MagVar).ToString("0.00")));
                itmX.SubItems.Insert(8, new ListViewItem.ListViewSubItem(null, NativeMethods.Modulus(Functions.Dir2Azt(Trace[i].ptOut, Trace[i].DirOut) - GlobalVars.CurrADHP.MagVar).ToString("0.00")));
                itmX.SubItems.Insert(9, new ListViewItem.ListViewSubItem(null, Trace[i].StCoords));
                itmX.SubItems.Insert(10, new ListViewItem.ListViewSubItem(null, Trace[i].FinCoords));
                itmX.SubItems.Insert(11, new ListViewItem.ListViewSubItem(null, Trace[i].Comment));
            }
        }

        private void ReSelectTrace()
        {
            Functions.DeleteGraphicsElement(pTraceSelectElem);
            Functions.DeleteGraphicsElement(pProtectSelectElem);
            pTraceSelectElem = null;
            pProtectSelectElem = null;

            int j = -1;

            for (int i = 0; i < ListView501.Items.Count; i++)
                if (ListView501.Items[i].Selected)
                {
                    j = i;
                    break;
                }

            if (j > -1)
            {
                pTraceSelectElem = Functions.DrawPolyline(Trace[j].PathPrj, 255);       // Functions.RGB(0, 255, 255)
                pTraceSelectElem.Locked = true;

                pProtectSelectElem = Functions.DrawPolygon(Trace[j].pProtectArea, 255); // Functions.RGB(0, 255, 255))
                pProtectSelectElem.Locked = true;
            }
        }

        //		private void UpdateTAData() 
        //		{ 
        //			int I = 0; 
        //			
        //			dTNAh = 0; 
        //			iShortage = -1; 
        //			dAr2addend = 0; 
        //			
        //			TACurrPDG = PANS_OPS_DataBase.dpPDG_Nom.Value; 
        //			dTNAh = CalcTNAHshortTA( TIARequiredTNAH, ref iShortage ); 
        //			
        //			if ( iShortage >= 0 ) 
        //			{ 
        //				TextBox207.Text = PtOutList[ iShortage ].ID; 
        //				TextBox208.Text = Functions.ConvertHeight( PtOutList[ iShortage ].Height + PtOutList[ iShortage ].MOC, 2 ).ToString() + " / " + Functions.ConvertHeight( TIARequiredTNAH + PtOutList[ iShortage ].Dist * CurrPDG, 2 ).ToString(); 
        //			} 
        //			else 
        //			{ 
        //				TextBox207.Text = Resources.str160; // loadresstring(39014)
        //				TextBox208.Text = "-"; 
        //			} 
        //			
        //			TextBox202.Text = Functions.ConvertDistance( dAr1 + dAr2, 3 ).ToString(); 
        //			TextBox201.Text = Functions.ConvertHeight( dTNAh, 2 ).ToString(); 
        //			
        //			dAr2addend = CalcTIARange(out I);
        //			
        //			Frame201.Enabled = dTNAh > 0.0; 
        //			
        //			//     SectorHeight = FindSectorHeight(PtOutList, TIARequiredTNAH, TACurrPDG)
        //			TextBox307.Text = Functions.ConvertHeight( SectorHeight, 3 ).ToString(); 
        //			TextBox205.Text = Functions.ConvertDistance( dAr2addend, 3 ).ToString(); 
        //			
        //			CheckBox101_CheckedChanged( CheckBox101, new System.EventArgs() ); 
        //			TextBox205_Validating( TextBox205, new System.ComponentModel.CancelEventArgs( false ) ); 
        //		} 


        private void SaveAccuracy(string RepFileName, string RepFileTitle, ReportHeader pReport)
        {
            AccurRep = new ReportFile();

            AccurRep.OpenFile(RepFileName + "_Accuracy", RepFileTitle + ": " + Resources.str00173);
            //AccurRep.H1(My.Resources.str00162 + " - " + RepFileTitle + ": " + My.Resources.str00173);
            AccurRep.WriteString(Resources.str00162 + " - " + RepFileTitle + ": " + Resources.str00173, true);

            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            AccurRep.WriteHeader(pReport);
            AccurRep.Param("Distance accuracy", GlobalVars.settings.DistancePrecision.ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);
            AccurRep.Param("Angle accuracy", GlobalVars.settings.AnglePrecision.ToString(), "degrees");
            double horAccuracy = Functions.CalDERcHorisontalAccuracy(DER);
            Functions.SaveDerAccurasyInfo(AccurRep, DER, horAccuracy);

            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            if (!CheckBox301.Checked)
            {
                AccurRep.CloseFile();
                return;
            }

            //AccurRep.WriteMessage();
            AccurRep.WriteMessage("=================================================");
            AccurRep.WriteMessage();

            NavaidType GuidNav = default(NavaidType);
            GuidNav.pPtPrj = DER.pPtPrj[eRWY.PtDER];
            GuidNav.Identifier = new Guid("C3E1A55A-490B-4230-AB1E-D60DB17C7E7C");
            GuidNav.TypeCode = eNavaidType.NONE;
            GuidNav.HorAccuracy = Functions.CalDERcHorisontalAccuracy(DER);

            NavaidType IntersectNav;

            // =============================================================================================================

            int i = 1;

            while (i < TSC)
            {
                TraceSegment currSegment = Trace[i];
                if (i < TSC - 1)
                {
                    TraceSegment nextSegment = Trace[i + 1];
                    if (nextSegment.SegmentCode == eSegmentType.straight && currSegment.LegType == CodeSegmentPath.DF)
                    {
                        currSegment = nextSegment;
                        i++;
                    }
                }
                i++;

                if (currSegment.LegType != CodeSegmentPath.CI)
                {
                    IntersectNav = currSegment.InterceptionNav;
                    GuidNav = currSegment.GuidanceNav;
                    Functions.SaveFixAccurasyInfo(AccurRep, currSegment.ptOut, "Departure FIX", GuidNav, IntersectNav, i >= TSC);
                }
            }

            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            AccurRep.CloseFile();
        }

        private void SaveGeometry(string RepFileName, string RepFileTitle, ReportHeader pReport)
        {
            OmniGeomRep = new ReportFile();
            OmniGeomRep.ThrPtPrj = DER.pPtPrj[eRWY.PtDER];

            OmniGeomRep.OpenFile(RepFileName + "_Geometry", RepFileTitle + ": " + Resources.str00517);

            OmniGeomRep.WriteString(Resources.str00162 + " - " + RepFileTitle + ": " + Resources.str00517, true);

            OmniGeomRep.WriteString("");
            OmniGeomRep.WriteString(RepFileTitle, true);
            OmniGeomRep.WriteHeader(pReport);

            OmniGeomRep.WriteString("");
            //OmniGeomRep.WriteString("");

            //OmniGeomRep.WriteParam(Resources.str164, ComboBox401.Text, "");
            OmniGeomRep.Param(Resources.str00165, TextBox403.Text, "%");
            OmniGeomRep.Param(Resources.str00166, TextBox401.Text, GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Unit);

            OmniGeomRep.WriteString("");
            OmniGeomRep.WriteString("");

            double TraceLen = 0;
            int i = 0;

            if (TSC > 1 && Trace[1].SegmentCode == eSegmentType.straight)
            {
                TraceSegment tmpSegment = Trace[1];
                tmpSegment.StCoords = Trace[0].StCoords;
                tmpSegment.HStart = Trace[0].HStart;

                tmpSegment.Length = Trace[0].Length + Trace[1].Length;

                OmniGeomRep.WriteTraceSegment(tmpSegment, 2 == TSC);
                OmniGeomRep.WriteString();

                TraceLen += tmpSegment.Length;
                i = 2;
            }

            for (; i < TSC; i++)
            {
                OmniGeomRep.WriteTraceSegment(Trace[i], i == TSC - 1);
                TraceLen = TraceLen + Trace[i].Length;
                OmniGeomRep.WriteString();
            }

            OmniGeomRep.Param(Resources.str00168, Functions.ConvertDistance(TraceLen, eRoundMode.NEAREST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);

            OmniGeomRep.CloseFile();
        }

        private void SaveProtocol(string RepFileName, string RepFileTitle, ReportHeader pReport)
        {
            ReportsFrm.SortForSave();

            OmniProtRep = new ReportFile();

            OmniProtRep.ThrPtPrj = DER.pPtPrj[eRWY.PtDER];

            OmniProtRep.OpenFile(RepFileName + "_Protocol", RepFileTitle + ": " + Resources.str00170);

            OmniProtRep.WriteString(Resources.str00171, true);
            OmniProtRep.WriteString("");
            OmniProtRep.WriteString(RepFileTitle, true);

            OmniProtRep.WriteHeader(pReport);
            OmniProtRep.WriteString("");
            OmniProtRep.WriteString("");

            OmniProtRep.lListView = ReportsFrm.listView1;
            OmniProtRep.WriteTab(ReportsFrm.GetTabPageText(0));

            OmniProtRep.lListView = ReportsFrm.listView2;
            OmniProtRep.WriteTab(ReportsFrm.GetTabPageText(1));

            OmniProtRep.lListView = ReportsFrm.listView3;
            OmniProtRep.WriteTab(ReportsFrm.GetTabPageText(2));

            OmniProtRep.CloseFile();
        }

        private void SaveLog(string RepFileName, string RepFileTitle, ReportHeader pReport)
        {
            OmniLogRep = new ReportFile();

            OmniLogRep.ThrPtPrj = ((ESRI.ArcGIS.Geometry.IPoint)(DER.pPtPrj[eRWY.PtDER]));

            OmniLogRep.OpenFile(RepFileName + "_Log", RepFileTitle + ": " + Resources.str00520);

            OmniLogRep.WriteString(Resources.str00172, true);
            OmniLogRep.WriteString("");
            OmniLogRep.WriteString(RepFileTitle, true);

            OmniLogRep.WriteHeader(pReport);

            OmniLogRep.WriteString("");
            OmniLogRep.WriteString("");

            OmniLogRep.ExH2(MultiPage1.TabPages[0].Text);
            OmniLogRep.HTMLString("[ " + MultiPage1.TabPages[0].Text + " ]", true, false);
            OmniLogRep.WriteString("");

            OmniLogRep.Param(Label001.Text, ComboBox001.Text, "");
            OmniLogRep.Param(Label002.Text, TextBox001.Text, "");
            OmniLogRep.Param(Label006.Text, TextBox004.Text, "");
            OmniLogRep.Param(Label003.Text, TextBox002.Text, Label010.Text);
            OmniLogRep.Param(Label014.Text, TextBox010.Text, Label015.Text);
            OmniLogRep.WriteString("");

            OmniLogRep.Param(Label004.Text, TextBox003.Text, Label011.Text);
            OmniLogRep.Param(Label009.Text, TextBox007.Text, Label012.Text);
            OmniLogRep.WriteString("");

            OmniLogRep.WriteString(Frame001.Text, true);
            OmniLogRep.Param(Label007.Text, TextBox005.Text, "");
            OmniLogRep.Param(Label008.Text, TextBox006.Text, Label013.Text);
            OmniLogRep.WriteString("");
            OmniLogRep.WriteString("");

            OmniLogRep.ExH2(MultiPage1.TabPages[1].Text);
            OmniLogRep.HTMLString("[ " + MultiPage1.TabPages[1].Text + " ]", true, false);
            OmniLogRep.WriteString("");

            OmniLogRep.Param(Label101.Text, TextBox101.Text, "%");
            OmniLogRep.Param(Label103.Text, TextBox102.Text, Label104.Text);
            //     OmniLogRep.WriteParam Label105.Caption, TextBox103.Text, Label106.Caption
            OmniLogRep.WriteString("");

            OmniLogRep.Param(Label111.Text, TextBox107.Text, Label113.Text);
            OmniLogRep.Param(Label112.Text, TextBox108.Text, Label114.Text);
            OmniLogRep.WriteString("");

            OmniLogRep.WriteString(Label107.Text, true);
            OmniLogRep.Param(Label108.Text, TextBox104.Text, "");
            OmniLogRep.Param(Label109.Text, TextBox105.Text, "");
            OmniLogRep.Param(Label110.Text, TextBox106.Text, "");
            OmniLogRep.WriteString("");
            OmniLogRep.WriteString("");

            OmniLogRep.ExH2(MultiPage1.TabPages[2].Text);
            OmniLogRep.HTMLString("[ " + MultiPage1.TabPages[2].Text + " ]", true, false);
            OmniLogRep.WriteString("");

            OmniLogRep.WriteString(Frame203.Text, true);
            OmniLogRep.Param(Label203.Text, TextBox203.Text, Label214.Text);
            OmniLogRep.Param(Label213.Text, TextBox211.Text, Label215.Text);
            OmniLogRep.Param(Label202.Text, TextBox202.Text, Label216.Text);
            OmniLogRep.Param(Label204.Text, TextBox204.Text, "%");
            OmniLogRep.WriteString("");

            OmniLogRep.Param(Label201.Text, TextBox201.Text, Label220.Text);
            OmniLogRep.Param(Label205.Text, TextBox207.Text, "");
            OmniLogRep.Param(Label206.Text, TextBox208.Text, Label221.Text);
            OmniLogRep.WriteString("");

            if (Frame202.Enabled)
            {
                OmniLogRep.WriteString(Frame202.Text, true);
                OmniLogRep.Param(Label209.Text, UpDown209.Value.ToString(), "°");
                OmniLogRep.Param(Label210.Text, UpDown210.Value.ToString(), "°");
                OmniLogRep.WriteString("");
            }

            if (Frame201.Enabled)
            {
                OmniLogRep.WriteString(Frame201.Text, true);
                OmniLogRep.Param(OptionButton201.Text, TextBox205.Text, Label218.Text);
                OmniLogRep.Param(OptionButton202.Text, TextBox206.Text, "%");
                OmniLogRep.WriteString("");
            }

            OmniLogRep.WriteString("");

            OmniLogRep.ExH2(MultiPage1.TabPages[3].Text);
            OmniLogRep.HTMLString("[ " + MultiPage1.TabPages[3].Text + " ]", true, false);
            OmniLogRep.WriteString("");

            OmniLogRep.Param(Label301.Text, TextBox301.Text, "%");
            OmniLogRep.Param(Label303.Text, TextBox302.Text, "%");
            OmniLogRep.Param(Label305.Text, TextBox303.Text, Label306.Text);
            OmniLogRep.Param(Label316.Text, TextBox308.Text, Label317.Text);
            OmniLogRep.Param(Label307.Text, TextBox304.Text, Label308.Text);
            OmniLogRep.WriteString("");

            if (Frame301.Visible)
            {
                OmniLogRep.WriteString(Frame301.Text, true);
                OmniLogRep.Param(Label309.Text, TextBox305.Text, "°");
                OmniLogRep.Param(Label310.Text, TextBox306.Text, "°");
                OmniLogRep.Param(Label313.Text, TextBox307.Text, Label314.Text);
                OmniLogRep.WriteString("");
            }

            OmniLogRep.WriteString("");

            if (CurrPage > 3)
            {
                OmniLogRep.ExH2(MultiPage1.TabPages[4].Text);
                OmniLogRep.WriteString("[ " + MultiPage1.TabPages[4].Text + " ]");
                OmniLogRep.WriteString("");

                OmniLogRep.Param(Label419.Text, ComboBox401.Text, "");
                OmniLogRep.Param(Label424.Text, TextBox401.Text, Label418.Text);
                OmniLogRep.Param(Label411.Text, TextBox402.Text, "%");
                OmniLogRep.Param(Label413.Text, TextBox403.Text, "%");
                OmniLogRep.Param(Label415.Text, TextBox404.Text, Label416.Text);
                OmniLogRep.Param(Label423.Text, TextBox405.Text, Label417.Text);
                OmniLogRep.WriteString("");
                OmniLogRep.WriteString("");
            }

            if (CurrPage > 4)
            {
                OmniLogRep.ExH2(MultiPage1.TabPages[5].Text);
                OmniLogRep.WriteString("[ " + MultiPage1.TabPages[5].Text + " ]");
                OmniLogRep.WriteString("");

                OmniLogRep.lListView = ListView501;
                OmniLogRep.WriteTab();
                OmniLogRep.WriteString("");
                OmniLogRep.WriteString("");
            }

            OmniLogRep.CloseFile();
        }

        private void FocusStepCaption(int StIndex)
        {
            for (int i = 0; i < Label1.Length; i++)
            {
                Label1[i].ForeColor = System.Drawing.Color.FromArgb(0XC0C0C0);
                Label1[i].Font = new Font(Label1[i].Font, FontStyle.Regular);
            }

            Label1[StIndex].ForeColor = System.Drawing.Color.FromArgb(0XFF8000);
            Label1[StIndex].Font = new Font(Label1[StIndex].Font, FontStyle.Bold);

            this.Text = Resources.str00011 + "  [" + MultiPage1.TabPages[StIndex].Text + "]";
        }

        UomDistance[] uomDistHorzTab = new UomDistance[] { UomDistance.KM, UomDistance.NM, UomDistance.M, UomDistance.FT, UomDistance.MI };
        UomDistanceVertical[] uomDistVerTab = new UomDistanceVertical[] { UomDistanceVertical.M, UomDistanceVertical.FT };
        UomSpeed[] uomSpeedTab = new UomSpeed[] { UomSpeed.KM_H, UomSpeed.KT, UomSpeed.M_SEC, UomSpeed.MACH, UomSpeed.M_MIN, UomSpeed.FT_MIN, UomSpeed.FT_SEC };

        UomDistance mUomHDistance;
        UomDistanceVertical mUomVDistance;
        UomSpeed mUomSpeed;

        private DepartureLeg StraightDepartureLeg(ref StandardInstrumentDeparture pProcedure, AircraftCharacteristic IsLimitedTo)// , TerminalSegmentPoint pEndPoint
        {
            DepartureLeg result = DBModule.pObjectDir.CreateFeature<DepartureLeg>();
            result.Departure = pProcedure.GetFeatureRef();
            result.AircraftCategory.Add(IsLimitedTo);

            SegmentLeg pSegmentLeg = result;
            pSegmentLeg.UpperLimitReference = CodeVerticalReference.MSL;
            pSegmentLeg.LowerLimitReference = CodeVerticalReference.MSL;
            pSegmentLeg.CourseType = CodeCourse.TRUE_TRACK;

            //pSegmentLeg.TurnDirection = CodeDirectionTurn.EITHER;
            pSegmentLeg.LegPath = CodeTrajectory.STRAIGHT;

            pSegmentLeg.LegTypeARINC = CodeSegmentPath.VA;
            pSegmentLeg.EndConditionDesignator = CodeSegmentTermination.ALTITUDE;

            pSegmentLeg.ProcedureTurnRequired = false;
            pSegmentLeg.Course = Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], DepDir);

            // =======================================================================================

            ValDistanceVertical pDistanceVertical = new ValDistanceVertical();
            pDistanceVertical.Uom = mUomVDistance;

            //if (CheckBox301.Checked)
            //	pDistanceVertical.Value = double.Parse(TextBox404.Text);
            //else
            pDistanceVertical.Value = Functions.ConvertHeight(Trace[0].HFinish);

            pSegmentLeg.LowerLimitAltitude = pDistanceVertical;
            pSegmentLeg.AltitudeInterpretation = CodeAltitudeUse.ABOVE_LOWER;
            // =================
            double fSegmentLength;
            //if (CheckBox301.Checked)
            //	fSegmentLength = (Functions.DeConvertHeight(double.Parse(TextBox405.Text)) - PANS_OPS_DataBase.dpH_abv_DER.Value) / CurrPDG;
            //else
            fSegmentLength = Trace[0].Length;

            ValDistance pDistance = new ValDistance();
            pDistance.Uom = mUomHDistance;
            pDistance.Value = Functions.ConvertDistance(fSegmentLength, eRoundMode.NEAREST);
            pSegmentLeg.Length = pDistance;
            // =================
            pSegmentLeg.VerticalAngle = Functions.RadToDeg(System.Math.Atan(appliedPDG));
            // =================
            ValSpeed pSpeed = new ValSpeed();
            pSpeed.Uom = mUomSpeed;
            pSpeed.Value = double.Parse(TextBox401.Text);
            pSegmentLeg.SpeedLimit = pSpeed;
            pSegmentLeg.SpeedReference = CodeSpeedReference.IAS;

            // Start Point ========================

            TerminalSegmentPoint pStartPoint = new TerminalSegmentPoint();
            //        pStartPoint.IndicatorFACF =      ??????????
            //        pStartPoint.LeadDME =            ??????????
            //        pStartPoint.LeadRadial =         ??????????
            //pStartPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.

            SegmentPoint pSegmentPoint = pStartPoint;
            //pSegmentPoint.FlyOver = true;
            pSegmentPoint.RadarGuidance = false;
            pSegmentPoint.ReportingATC = CodeATCReporting.NO_REPORT;
            pSegmentPoint.Waypoint = false;

            SignificantPoint derEnd = new SignificantPoint();
            derEnd.RunwayPoint = new FeatureRef(DER.pSignificantPointID[eRWY.PtDER]);
            pSegmentPoint.PointChoice = derEnd;
            //pSegmentPoint.PointChoice = DER.pSignificantPoint[eRWY.PtDER];

            pSegmentLeg.StartPoint = pStartPoint;
            //  End Of Start Point ========================

            // EndPoint ========================
            // End of EndPoint ========================

            //===========================================================
            // protected Area =======================================================


            //Functions.DrawPolygon(ZNR_Poly, -1, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSCross);
            //while(true)
            //Application.DoEvents();

            IPolygon pPolygon1 = Functions.PolygonIntersection(Trace[0].pProtectArea, pCircle);

            ObstacleAssessmentArea pPrimProtectedArea = new ObstacleAssessmentArea();
            pPrimProtectedArea.Surface = Converters.ESRIPolygonToAIXMSurface(Functions.ToGeo(pPolygon1 as IGeometry) as IPolygon);
            pPrimProtectedArea.SectionNumber = 0;
            //pPrimProtectedArea.StartingCurve = pCurve;
            pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY;
            pPrimProtectedArea.SurfaceZone = CodeObstructionIdSurfaceZone.PRIMARY;

            //=====================================================================
            // Protection Area Obstructions list ==================================================
            ObstacleContainer ostacles = oInnerList;
            IRelationalOperator relation = (IRelationalOperator)pPolygon1;

            ObstacleData[] MaxPDGArray = new ObstacleData[GlobalVars.ArraySize];
            ObstacleData[] MaxTnaHArray = new ObstacleData[GlobalVars.ArraySize];
            int MaxPDGArrayCnt = 0;
            int MaxTnaHArrayCnt = 0;

            if (ostacles.Parts.Length > 0)
            {
                TIARequiredTNAH = CalcStraightReqTNAH(CurrPDG, drPDGMax, ref iTnaH, CheckBox101.Checked);

                MaxPDGArray[0] = ostacles.Parts[0];
                MaxTnaHArray[0] = ostacles.Parts[0];

                double MaxPDGValue = MaxPDGArray[0].PDG;
                double MaxTnaHValue = MaxTnaHArray[0].ReqTNH;

                double MinPDGValue = MaxPDGArray[0].PDG;
                double MinTnaHValue = MaxTnaHArray[0].ReqTNH;

                double PDGMaxTnaH = MaxTnaHArray[0].ReqTNH;
                double PDGMinTnaH = MaxTnaHArray[0].ReqTNH;

                MaxPDGArrayCnt = 1;
                MaxTnaHArrayCnt = 1;

                for (int i = 1; i < ostacles.Parts.Length; i++)
                {
                    int stVal = MaxPDGArrayCnt < GlobalVars.ArraySize ? MaxPDGArrayCnt : GlobalVars.ArraySize - 1;
                    int j = stVal;
                    double currPdg = ostacles.Parts[i].PDG;

                    if (currPdg > MaxPDGValue)
                    {
                        for (; j > 0; j--)
                            MaxPDGArray[j] = MaxPDGArray[j - 1];

                        MaxPDGArray[0] = ostacles.Parts[i];
                        MaxPDGValue = currPdg;

                        if (MaxPDGArrayCnt < GlobalVars.ArraySize)
                            MaxPDGArrayCnt++;

                        if (PDGMaxTnaH < ostacles.Parts[i].ReqTNH)
                            PDGMaxTnaH = ostacles.Parts[i].ReqTNH;

                        if (PDGMinTnaH > ostacles.Parts[i].ReqTNH)
                            PDGMinTnaH = ostacles.Parts[i].ReqTNH;

                    }
                    else if (currPdg > MinPDGValue)
                    {
                        for (; j > 0 && currPdg > MaxPDGArray[j - 1].PDG; j--)
                            MaxPDGArray[j] = MaxPDGArray[j - 1];
                        MaxPDGArray[j] = ostacles.Parts[i];

                        if (MaxPDGArrayCnt < GlobalVars.ArraySize)
                            MaxPDGArrayCnt++;
                        MinPDGValue = MaxPDGArray[MaxPDGArrayCnt - 1].PDG;

                        if (PDGMaxTnaH < ostacles.Parts[i].ReqTNH)
                            PDGMaxTnaH = ostacles.Parts[i].ReqTNH;

                        if (PDGMinTnaH > ostacles.Parts[i].ReqTNH)
                            PDGMinTnaH = ostacles.Parts[i].ReqTNH;

                    }
                    else if (MaxPDGArrayCnt < GlobalVars.ArraySize)
                    {
                        MaxPDGArray[MaxPDGArrayCnt] = ostacles.Parts[i];
                        MinPDGValue = currPdg;
                        MaxPDGArrayCnt++;

                        if (PDGMaxTnaH < ostacles.Parts[i].ReqTNH)
                            PDGMaxTnaH = ostacles.Parts[i].ReqTNH;

                        if (PDGMinTnaH > ostacles.Parts[i].ReqTNH)
                            PDGMinTnaH = ostacles.Parts[i].ReqTNH;
                    }

                    //==========================================================

                    double currTnaH = ostacles.Parts[i].ReqTNH;

                    if (currTnaH > MaxTnaHValue)
                    {
                        for (; j > 0; j--)
                            MaxTnaHArray[j] = MaxTnaHArray[j - 1];

                        MaxTnaHArray[0] = ostacles.Parts[i];
                        MaxTnaHValue = currTnaH;

                        if (MaxTnaHArrayCnt < GlobalVars.ArraySize)
                            MaxTnaHArrayCnt++;
                    }
                    else if (currTnaH > MinTnaHValue)
                    {
                        for (; j > 0 && currTnaH > MaxTnaHArray[j - 1].ReqTNH; j--)
                            MaxTnaHArray[j] = MaxTnaHArray[j - 1];
                        MaxTnaHArray[j] = ostacles.Parts[i];

                        if (MaxTnaHArrayCnt < GlobalVars.ArraySize)
                            MaxTnaHArrayCnt++;
                        MinTnaHValue = MaxTnaHArray[MaxTnaHArrayCnt - 1].ReqTNH;
                    }
                    else if (MaxTnaHArrayCnt < GlobalVars.ArraySize)
                    {
                        MaxTnaHArray[MaxTnaHArrayCnt] = ostacles.Parts[i];
                        MinTnaHValue = currTnaH;
                        MaxTnaHArrayCnt++;
                    }

                }
            }

            System.Array.Resize<ObstacleData>(ref MaxPDGArray, MaxPDGArrayCnt + MaxTnaHArrayCnt);
            System.Array.Copy(MaxTnaHArray, 0, MaxPDGArray, MaxPDGArrayCnt, MaxTnaHArrayCnt);
            MaxPDGArrayCnt += MaxTnaHArrayCnt;

            for (int i = 0; i < MaxPDGArrayCnt - 1; i++)
            {
                for (int j = i + 1; j < MaxPDGArrayCnt; j++)
                {
                    if (MaxPDGArray[i].Owner == MaxPDGArray[j].Owner)
                    {
                        if (MaxPDGArray[i].MOC < MaxPDGArray[j].MOC)
                            MaxPDGArray[i] = MaxPDGArray[j];

                        MaxPDGArray[j] = MaxPDGArray[--MaxPDGArrayCnt];
                    }
                }
            }

            //for (int i = 0; i < MaxTnaHArrayCnt - 1; i++)
            //{
            //	for (int j = i + 1; j < MaxTnaHArrayCnt; j++)
            //	{
            //		if (MaxTnaHArray[i].Owner == MaxTnaHArray[j].Owner)
            //		{
            //			MaxTnaHArray[j] = MaxTnaHArray[--MaxTnaHArrayCnt];
            //		}
            //	}
            //}

            for (int i = 0; i < MaxPDGArrayCnt; i++)
            {
                Obstruction obs = new Obstruction();
                int owner = MaxPDGArray[i].Owner;
                //double MinimumAltitude = 0;
                double RequiredClearance = 0;
                bool CloseIn = true;
                bool inArea = false;

                obs.VerticalStructureObstruction = new FeatureRef(ostacles.Obstacles[owner].Identifier);    //ostacles.Obstacles[i].pFeature.GetFeatureRef();

                for (int j = 0; j < ostacles.Obstacles[owner].PartsNum; j++)
                {
                    if (relation.Disjoint(ostacles.Parts[ostacles.Obstacles[owner].Parts[j]].pPtPrj))
                        continue;

                    //MinimumAltitude = Math.Max(MinimumAltitude, ostacles.Parts[ostacles.Obstacles[owner].Parts[j]].hPenet);
                    RequiredClearance = Math.Max(RequiredClearance, ostacles.Parts[ostacles.Obstacles[owner].Parts[j]].MOC);
                    inArea = true;

                    if (!ostacles.Parts[ostacles.Obstacles[owner].Parts[j]].Ignored)
                        CloseIn = false;
                    //if (ostacles.Parts[ostacles.Obstacles[owner].Parts[j]].Prima)		isPrimary |= 1;
                    //else																isPrimary |= 2;
                }

                if (!inArea)
                    continue;

                obs.CloseIn = CloseIn;

                //ReqH
                double MinimumAltitude = ostacles.Obstacles[owner].Height + RequiredClearance + DER.pPtPrj[eRWY.PtDER].Z;

                pDistanceVertical = new ValDistanceVertical();
                pDistanceVertical.Uom = mUomVDistance;
                pDistanceVertical.Value = Functions.ConvertHeight(MinimumAltitude);
                obs.MinimumAltitude = pDistanceVertical;

                //MOC
                pDistance = new ValDistance();
                pDistance.Uom = UomDistance.M;
                pDistance.Value = RequiredClearance;
                obs.RequiredClearance = pDistance;

                //if ((isPrimary & 1) != 0)
                pPrimProtectedArea.SignificantObstacle.Add(obs);
            }

            //  END ======================================================

            //==========================================================================

            pSegmentLeg.DesignSurface.Add(pPrimProtectedArea);

            // Trajectory =====================================================
            Aran.Geometries.Point pLocation;
            Curve pCurve;
            LineString pLineStringSegment;

            pLineStringSegment = new LineString();

            pLocation = Converters.ESRIPointToARANPoint(DER.pPtGeo[eRWY.PtDER]);
            pLineStringSegment.Add(pLocation);

            IPoint ptTmp;

            //if (CheckBox301.Checked)
            //	ptTmp = (IPoint)Functions.ToGeo(Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir, fSegmentLength));
            //else
            ptTmp = (IPoint)Functions.ToGeo(Trace[0].PathPrj.ToPoint);

            pLocation = Converters.ESRIPointToARANPoint(ptTmp);
            pLineStringSegment.Add(pLocation);

            pCurve = new Curve();
            pCurve.Geo.Add(pLineStringSegment);
            pSegmentLeg.Trajectory = pCurve;
            //  END =====================================================
            return result;
        }

        private DepartureLeg StandartDepartureLeg(TraceSegment segment, ref StandardInstrumentDeparture pProcedure, AircraftCharacteristic IsLimitedTo, ref TerminalSegmentPoint pEndPoint, bool first)
        {
            DepartureLeg result = DBModule.pObjectDir.CreateFeature<DepartureLeg>();
            result.Departure = pProcedure.GetFeatureRef();
            result.AircraftCategory.Add(IsLimitedTo);

            SegmentLeg pSegmentLeg = result;

            pSegmentLeg.AltitudeInterpretation = CodeAltitudeUse.ABOVE_LOWER;
            pSegmentLeg.UpperLimitReference = CodeVerticalReference.MSL;
            pSegmentLeg.LowerLimitReference = CodeVerticalReference.MSL;
            pSegmentLeg.CourseType = CodeCourse.TRUE_TRACK;

            //=================================
            //FIXableNavaidType IntersectNav;
            NavaidType IntersectNav = segment.InterceptionNav;

            //NavaidType EndIntersectNav;
            ESRI.ArcGIS.Geometry.IPoint ptStart = segment.ptIn;
            ESRI.ArcGIS.Geometry.IPoint ptEnd = segment.ptOut;

            //  ======================================================================
            pSegmentLeg.LegTypeARINC = segment.LegType;

            //if (Functions.SideDef(pInterceptPt, pInterceptPt.M + 90.0, GuidNav.pPtPrj) < 0)
            //    pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.FROM;
            //else
            //    pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO;

            double fCourse, fCourseDir;

            switch (segment.SegmentCode)
            {
                //case eSegmentType.straight:					// "Прямой сегмент"
                case eSegmentType.courseIntercept:              // "На заданную WPT"
                case eSegmentType.arcIntercept:
                    fCourseDir = segment.DirIn;
                    fCourse = Functions.Dir2Azt(segment.ptIn, fCourseDir);
                    break;
                case eSegmentType.turnAndIntercept:
                    fCourseDir = segment.DirBetween;
                    fCourse = Functions.Dir2Azt(segment.PtCenter1, fCourseDir);
                    break;
                default:
                    fCourseDir = segment.DirOut;
                    fCourse = Functions.Dir2Azt(segment.ptOut, fCourseDir);
                    break;
            }

            pSegmentLeg.Course = fCourse;

            //  LowerLimitAltitude ========================================================
            ValDistanceVertical pDistanceVertical = new ValDistanceVertical();
            pDistanceVertical.Uom = mUomVDistance;
            pDistanceVertical.Value = Functions.ConvertHeight(segment.HFinish, eRoundMode.NEAREST); //Trace[index].HStart
            pSegmentLeg.LowerLimitAltitude = pDistanceVertical;

            //  UpperLimitAltitude ========================================================
            //pDistanceVertical = new ValDistanceVertical();
            //pDistanceVertical.Uom = mUomVDistance;
            //pDistanceVertical.Value = Functions.ConvertHeight(Trace[index].HFinish, eRoundMode.rmNERAEST);
            //pSegmentLeg.UpperLimitAltitude = pDistanceVertical;
            //=================================
            pSegmentLeg.BankAngle = segment.BankAngle;
            pSegmentLeg.VerticalAngle = Functions.RadToDeg(System.Math.Atan(segment.PDG));
            // =================

            ValDistance pDistance = new ValDistance();
            pDistance.Uom = mUomHDistance;
            pDistance.Value = Functions.ConvertDistance(segment.Length, eRoundMode.NEAREST);
            pSegmentLeg.Length = pDistance;

            ValSpeed pSpeed = new ValSpeed();
            pSpeed.Uom = mUomSpeed;
            pSpeed.Value = double.Parse(TextBox401.Text);
            pSegmentLeg.SpeedLimit = pSpeed;
            pSegmentLeg.SpeedReference = CodeSpeedReference.IAS;

            // Start Point ========================
            pSegmentLeg.StartPoint = pEndPoint;

            // End Of Start Point ====================================================

            // Course Indication =====================================================
            NavaidType GuidNav = segment.GuidanceNav;
            double fDistToNav, fAltitudeMin;
            double Angle, fDist, fDir;

            AngleIndication pAngleIndication = null;
            DistanceIndication pDistanceIndication = null;

            if (GuidNav.TypeCode > eNavaidType.NONE && segment.SegmentCode != eSegmentType.turnAndIntercept)
            {
                if (GuidNav.TypeCode == eNavaidType.DME)
                {
                    fDistToNav = Functions.ReturnDistanceInMeters(GuidNav.pPtPrj, ptEnd);
                    fAltitudeMin = segment.HFinish - GuidNav.pPtPrj.Z;
                    fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitudeMin * fAltitudeMin);

                    pDistanceIndication = DBModule.CreateDistanceIndication(Functions.ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, GuidNav.GetSignificantPoint());
                    pSegmentLeg.Distance = pDistanceIndication.GetFeatureRef();
                }
                else //if (GuidNav.TypeCode != eNavaidType.NONE)
                {
                    fDir = Functions.ReturnAngleInDegrees(GuidNav.pPtPrj, ptEnd);
                    Angle = NativeMethods.Modulus(fCourse - GuidNav.MagVar);

                    pAngleIndication = DBModule.CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, GuidNav.GetSignificantPoint());
                    pAngleIndication.TrueAngle = pSegmentLeg.Course;
                    pAngleIndication.IndicationDirection = CodeDirectionReference.FROM;

                    pSegmentLeg.Angle = pAngleIndication.GetFeatureRef();
                }
            }
            // End Of Indication ======================================================================

            // Leg Points ========================
            // EndPoint =============================================
            pEndPoint = null;

            if (segment.LegType == CodeSegmentPath.AF || segment.LegType == CodeSegmentPath.HF || segment.LegType == CodeSegmentPath.IF ||
                segment.LegType == CodeSegmentPath.TF || segment.LegType == CodeSegmentPath.CF || segment.LegType == CodeSegmentPath.DF ||
                segment.LegType == CodeSegmentPath.RF || segment.LegType == CodeSegmentPath.CD || segment.LegType == CodeSegmentPath.CR ||
                segment.LegType == CodeSegmentPath.VD || segment.LegType == CodeSegmentPath.VR)
            {
                pEndPoint = new TerminalSegmentPoint();
                pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.OTHER_WPT;
                SegmentPoint pSegmentPoint = pEndPoint;
                //pSegmentPoint.FlyOver = true;

                pSegmentPoint.RadarGuidance = false;
                pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT;
                pSegmentPoint.Waypoint = false;

                //pEndPoint.IndicatorFACF =      ??????????
                //pEndPoint.LeadDME =            ??????????
                //pEndPoint.LeadRadial =         ??????????

                //if (GuidNav.TypeCode == eNavaidType.DME)
                //	fCourseDir = Functions.ReturnAngleInDegrees(GuidNav.pPtPrj, ptEnd) +
                //		90 * (1 + 2 * (pSegmentLeg.CourseDirection == Aran.Aim.Enums.CodeDirectionReference.OTHER_CW ? 1 : 0));
                //fCourseDir = Azt2DirPrj(ptStart, pSegmentLeg.Course)

                bool bOnNav = false;
                SignificantPoint pInterNavSignPt = null;

                if (IntersectNav.TypeCode > eNavaidType.NONE && IntersectNav.NAV_Ident != Guid.Empty)
                {
                    pInterNavSignPt = IntersectNav.GetSignificantPoint();

                    //if(SttIntersectNav.Identifier == GuidNav.Identifier)
                    if (IntersectNav.IntersectionType == eIntersectionType.OnNavaid)
                        bOnNav = true;
                }

                // Guidance Indication =====================================================================

                AngleUse pAngleUse;
                PointReference pPointReference = new PointReference();
                SignificantPoint pFIXSignPt;

                if (bOnNav)
                {
                    pFIXSignPt = pInterNavSignPt;
                    pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD;
                }
                else
                {
                    double horAccuracy = 0.0;

                    if (GuidNav.Identifier != Guid.Empty && GuidNav.TypeCode != eNavaidType.NONE && IntersectNav.TypeCode != eNavaidType.NONE)
                        horAccuracy = Functions.CalcHorisontalAccuracy(ptEnd, GuidNav, IntersectNav);

					Feature pFixDesignatedPoint = DBModule.CreateDesignatedPoint(ptEnd, "COORD", fCourseDir);

                    if (GuidNav.NAV_Ident != Guid.Empty)
                    {
                        if (GuidNav.TypeCode == eNavaidType.DME)
                        {
                            pDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef();
                            pPointReference.FacilityDistance.Add(pDistanceIndication.GetFeatureRefObject());
                        }
                        else if (GuidNav.TypeCode != eNavaidType.NONE)
                        {
                            pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef();

                            pAngleUse = new AngleUse();
                            pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef();
                            pAngleUse.AlongCourseGuidance = true;

                            pPointReference.FacilityAngle.Add(pAngleUse);
                        }
                    }

                    // End Of Indication ============================================

                    pFIXSignPt = new SignificantPoint();

                    if (pFixDesignatedPoint is DesignatedPoint)
                        pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef();
                    else if (pFixDesignatedPoint is Navaid)
                        pFIXSignPt.NavaidSystem = pFixDesignatedPoint.GetFeatureRef();

                    if (IntersectNav.TypeCode == eNavaidType.DME)
                    {
                        fDistToNav = Functions.ReturnDistanceInMeters(IntersectNav.pPtPrj, ptEnd);
                        fAltitudeMin = segment.HFinish - IntersectNav.pPtPrj.Z;

                        fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitudeMin * fAltitudeMin);
                        pDistanceIndication = DBModule.CreateDistanceIndication(Functions.ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, pInterNavSignPt);
                        pDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef();

                        pPointReference.FacilityDistance.Add(pDistanceIndication.GetFeatureRefObject());
                        pPointReference.Role = CodeReferenceRole.RAD_DME;
                    }
                    else if (IntersectNav.TypeCode != eNavaidType.NONE)
                    {
                        pAngleUse = new AngleUse();
                        fDir = Functions.ReturnAngleInDegrees(IntersectNav.pPtPrj, ptEnd);

                        Angle = NativeMethods.Modulus(Functions.Dir2Azt(IntersectNav.pPtPrj, fDir) - IntersectNav.MagVar, 360.0);
                        pAngleIndication = DBModule.CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pInterNavSignPt);
                        pAngleIndication.TrueAngle = Functions.Dir2Azt(ptEnd, fDir);
                        pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef();
                        pAngleIndication.IndicationDirection = CodeDirectionReference.FROM;

                        pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef();
                        pAngleUse.AlongCourseGuidance = false;

                        pPointReference.FacilityAngle.Add(pAngleUse);
                        pPointReference.Role = CodeReferenceRole.INTERSECTION;
                    }
                }

                pSegmentPoint.PointChoice = pFIXSignPt;
                pEndPoint.FacilityMakeup.Add(pPointReference);
            }

            pSegmentLeg.EndPoint = pEndPoint;

            // End of EndPoint ========================
            // protected Area =======================================================

            if (first)
            {
                IPolygon pPolygon1 = Functions.PolygonIntersection(segment.pProtectArea, pCircle);

                ObstacleAssessmentArea pPrimProtectedArea = new ObstacleAssessmentArea();
                pPrimProtectedArea.Surface = Converters.ESRIPolygonToAIXMSurface(Functions.ToGeo(pPolygon1 as IGeometry) as IPolygon);
                pPrimProtectedArea.SectionNumber = 0;
                //pPrimProtectedArea.StartingCurve = pCurve;
                pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY;
                pPrimProtectedArea.SurfaceZone = CodeObstructionIdSurfaceZone.PRIMARY;
                //=====================================================================
                // Protection Area Obstructions list ==================================================
                ObstacleContainer ostacles = oInnerList;
                IRelationalOperator relation = (IRelationalOperator)pPolygon1;

                ObstacleData[] MaxPDGArray = new ObstacleData[GlobalVars.ArraySize];
                ObstacleData[] MaxTnaHArray = new ObstacleData[GlobalVars.ArraySize];
                int MaxPDGArrayCnt = 0;
                int MaxTnaHArrayCnt = 0;

                if (ostacles.Parts.Length > 0)
                {
                    TIARequiredTNAH = CalcStraightReqTNAH(CurrPDG, drPDGMax, ref iTnaH, CheckBox101.Checked);

                    MaxPDGArray[0] = ostacles.Parts[0];
                    MaxTnaHArray[0] = ostacles.Parts[0];

                    double MaxPDGValue = MaxPDGArray[0].PDG;
                    double MaxTnaHValue = MaxTnaHArray[0].ReqTNH;

                    double MinPDGValue = MaxPDGArray[0].PDG;
                    double MinTnaHValue = MaxTnaHArray[0].ReqTNH;

                    double PDGMaxTnaH = MaxTnaHArray[0].ReqTNH;
                    double PDGMinTnaH = MaxTnaHArray[0].ReqTNH;

                    MaxPDGArrayCnt = 1;
                    MaxTnaHArrayCnt = 1;

                    for (int i = 1; i < ostacles.Parts.Length; i++)
                    {
                        int stVal = MaxPDGArrayCnt < GlobalVars.ArraySize ? MaxPDGArrayCnt : GlobalVars.ArraySize - 1;
                        int j = stVal;
                        double currPdg = ostacles.Parts[i].PDG;

                        if (currPdg > MaxPDGValue)
                        {
                            for (; j > 0; j--)
                                MaxPDGArray[j] = MaxPDGArray[j - 1];

                            MaxPDGArray[0] = ostacles.Parts[i];
                            MaxPDGValue = currPdg;

                            if (MaxPDGArrayCnt < GlobalVars.ArraySize)
                                MaxPDGArrayCnt++;

                            if (PDGMaxTnaH < ostacles.Parts[i].ReqTNH)
                                PDGMaxTnaH = ostacles.Parts[i].ReqTNH;

                            if (PDGMinTnaH > ostacles.Parts[i].ReqTNH)
                                PDGMinTnaH = ostacles.Parts[i].ReqTNH;

                        }
                        else if (currPdg > MinPDGValue)
                        {
                            for (; j > 0 && currPdg > MaxPDGArray[j - 1].PDG; j--)
                                MaxPDGArray[j] = MaxPDGArray[j - 1];
                            MaxPDGArray[j] = ostacles.Parts[i];

                            if (MaxPDGArrayCnt < GlobalVars.ArraySize)
                                MaxPDGArrayCnt++;
                            MinPDGValue = MaxPDGArray[MaxPDGArrayCnt - 1].PDG;

                            if (PDGMaxTnaH < ostacles.Parts[i].ReqTNH)
                                PDGMaxTnaH = ostacles.Parts[i].ReqTNH;

                            if (PDGMinTnaH > ostacles.Parts[i].ReqTNH)
                                PDGMinTnaH = ostacles.Parts[i].ReqTNH;

                        }
                        else if (MaxPDGArrayCnt < GlobalVars.ArraySize)
                        {
                            MaxPDGArray[MaxPDGArrayCnt] = ostacles.Parts[i];
                            MinPDGValue = currPdg;
                            MaxPDGArrayCnt++;

                            if (PDGMaxTnaH < ostacles.Parts[i].ReqTNH)
                                PDGMaxTnaH = ostacles.Parts[i].ReqTNH;

                            if (PDGMinTnaH > ostacles.Parts[i].ReqTNH)
                                PDGMinTnaH = ostacles.Parts[i].ReqTNH;
                        }

                        //==========================================================

                        double currTnaH = ostacles.Parts[i].ReqTNH;

                        if (currTnaH > MaxTnaHValue)
                        {
                            for (; j > 0; j--)
                                MaxTnaHArray[j] = MaxTnaHArray[j - 1];

                            MaxTnaHArray[0] = ostacles.Parts[i];
                            MaxTnaHValue = currTnaH;

                            if (MaxTnaHArrayCnt < GlobalVars.ArraySize)
                                MaxTnaHArrayCnt++;
                        }
                        else if (currTnaH > MinTnaHValue)
                        {
                            for (; j > 0 && currTnaH > MaxTnaHArray[j - 1].ReqTNH; j--)
                                MaxTnaHArray[j] = MaxTnaHArray[j - 1];
                            MaxTnaHArray[j] = ostacles.Parts[i];

                            if (MaxTnaHArrayCnt < GlobalVars.ArraySize)
                                MaxTnaHArrayCnt++;
                            MinTnaHValue = MaxTnaHArray[MaxTnaHArrayCnt - 1].ReqTNH;
                        }
                        else if (MaxTnaHArrayCnt < GlobalVars.ArraySize)
                        {
                            MaxTnaHArray[MaxTnaHArrayCnt] = ostacles.Parts[i];
                            MinTnaHValue = currTnaH;
                            MaxTnaHArrayCnt++;
                        }

                    }
                }

                System.Array.Resize<ObstacleData>(ref MaxPDGArray, MaxPDGArrayCnt + MaxTnaHArrayCnt);
                System.Array.Copy(MaxTnaHArray, 0, MaxPDGArray, MaxPDGArrayCnt, MaxTnaHArrayCnt);
                MaxPDGArrayCnt += MaxTnaHArrayCnt;

                for (int i = 0; i < MaxPDGArrayCnt - 1; i++)
                {
                    for (int j = i + 1; j < MaxPDGArrayCnt; j++)
                    {
                        if (MaxPDGArray[i].Owner == MaxPDGArray[j].Owner)
                        {
                            if (MaxPDGArray[i].MOC < MaxPDGArray[j].MOC)
                                MaxPDGArray[i] = MaxPDGArray[j];

                            MaxPDGArray[j] = MaxPDGArray[--MaxPDGArrayCnt];
                        }
                    }
                }

                for (int i = 0; i < MaxPDGArrayCnt; i++)
                {
                    Obstruction obs = new Obstruction();
                    int owner = MaxPDGArray[i].Owner;
                    //double MinimumAltitude = 0;
                    double RequiredClearance = 0;
                    bool CloseIn = true;
                    bool inArea = false;

                    obs.VerticalStructureObstruction = new FeatureRef(ostacles.Obstacles[owner].Identifier);    //ostacles.Obstacles[i].pFeature.GetFeatureRef();


                    for (int j = 0; j < ostacles.Obstacles[owner].PartsNum; j++)
                    {
                        if (relation.Disjoint(ostacles.Parts[ostacles.Obstacles[owner].Parts[j]].pPtPrj))
                            continue;

                        //MinimumAltitude = Math.Max(MinimumAltitude, ostacles.Parts[ostacles.Obstacles[owner].Parts[j]].hPenet);
                        RequiredClearance = Math.Max(RequiredClearance, ostacles.Parts[ostacles.Obstacles[owner].Parts[j]].MOC);
                        inArea = true;

                        if (!ostacles.Parts[ostacles.Obstacles[owner].Parts[j]].Ignored)
                            CloseIn = false;
                        //if (ostacles.Parts[ostacles.Obstacles[owner].Parts[j]].Prima)		isPrimary |= 1;
                        //else																isPrimary |= 2;
                    }

                    if (!inArea)
                        continue;

                    obs.CloseIn = CloseIn;

                    //ReqH
                    double MinimumAltitude = ostacles.Obstacles[owner].Height + RequiredClearance + DER.pPtPrj[eRWY.PtDER].Z;

                    pDistanceVertical = new ValDistanceVertical();
                    pDistanceVertical.Uom = mUomVDistance;
                    pDistanceVertical.Value = Functions.ConvertHeight(MinimumAltitude);
                    obs.MinimumAltitude = pDistanceVertical;

                    //MOC
                    pDistance = new ValDistance();
                    pDistance.Uom = UomDistance.M;
                    pDistance.Value = RequiredClearance;
                    obs.RequiredClearance = pDistance;

                    //if ((isPrimary & 1) != 0)
                    pPrimProtectedArea.SignificantObstacle.Add(obs);
                }

                //for (int i = 0; i < ostacles.Obstacles.Length; i++)
                //{
                //	Obstruction obs = new Obstruction();
                //	obs.VerticalStructureObstruction = new FeatureRef(ostacles.Obstacles[i].Identifier);	//ostacles.Obstacles[i].pFeature.GetFeatureRef();

                //	//double MinimumAltitude = 0;
                //	double RequiredClearance = 0;
                //	bool CloseIn = true;
                //	bool inArea = false;

                //	for (int j = 0; j < ostacles.Obstacles[i].PartsNum; j++)
                //	{
                //		if (relation.Disjoint(ostacles.Parts[ostacles.Obstacles[i].Parts[j]].pPtPrj))
                //			continue;

                //		//MinimumAltitude = Math.Max(MinimumAltitude, ostacles.Parts[ostacles.Obstacles[i].Parts[j]].hPenet);
                //		RequiredClearance = Math.Max(RequiredClearance, ostacles.Parts[ostacles.Obstacles[i].Parts[j]].MOC);
                //		inArea = true;

                //		if (!ostacles.Parts[ostacles.Obstacles[i].Parts[j]].Ignored)
                //			CloseIn = false;
                //		//if (ostacles.Parts[ostacles.Obstacles[i].Parts[j]].Prima)			isPrimary |= 1;
                //		//else																isPrimary |= 2;
                //	}

                //	if (!inArea)
                //		continue;

                //	obs.CloseIn = CloseIn;

                //	//ReqH
                //	double MinimumAltitude = ostacles.Obstacles[i].Height + RequiredClearance + DER.pPtPrj[eRWY.PtDER].Z;

                //	pDistanceVertical = new ValDistanceVertical();
                //	pDistanceVertical.Uom = mUomVDistance;
                //	pDistanceVertical.Value = Functions.ConvertHeight(MinimumAltitude);
                //	obs.MinimumAltitude = pDistanceVertical;

                //	//MOC
                //	pDistance = new ValDistance();
                //	pDistance.Uom = UomDistance.M;
                //	pDistance.Value = RequiredClearance;
                //	obs.RequiredClearance = pDistance;

                //	//if ((isPrimary & 1) != 0)
                //	pPrimProtectedArea.SignificantObstacle.Add(obs);

                //	//if (pSecProtectedArea != null && (isPrimary & 2) != 0)
                //	//	pSecProtectedArea.SignificantObstacle.Add(obs);
                //}
                //  END ======================================================
                //=====================================================================
                pSegmentLeg.DesignSurface.Add(pPrimProtectedArea);
            }
            else
            {
                //Functions.DrawPolygon(ZNR_Poly, -1, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSCross);
                //while(true)
                //Application.DoEvents();

                IPolygon pPolygon1 = pCircle;

                ObstacleAssessmentArea pPrimProtectedArea = new ObstacleAssessmentArea();
                pPrimProtectedArea.Surface = Converters.ESRIPolygonToAIXMSurface(Functions.ToGeo(pPolygon1 as IGeometry) as IPolygon);
                pPrimProtectedArea.SectionNumber = 0;
                //pPrimProtectedArea.StartingCurve = pCurve;
                pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY;
                pPrimProtectedArea.SurfaceZone = CodeObstructionIdSurfaceZone.PRIMARY;
                //=====================================================================
                // Protection Area Obstructions list ==================================================
                ObstacleContainer ostacles = oOuterList;
                IRelationalOperator relation = (IRelationalOperator)pPolygon1;

                //============================================================================================================================
                ObstacleData[] MaxHPenetArray = new ObstacleData[GlobalVars.ArraySize];

                int MaxHPenetArrayCnt = 0;

                if (ostacles.Parts.Length > 0)
                {
                    MaxHPenetArray[0] = ostacles.Parts[0];

                    double MaxHPenetValue = MaxHPenetArray[0].CourseAdjust;
                    double MinHPenetValue = MaxHPenetValue;

                    MaxHPenetArrayCnt = 1;

                    for (int i = 1; i < ostacles.Parts.Length; i++)
                    {
                        int stVal = MaxHPenetArrayCnt < GlobalVars.ArraySize ? MaxHPenetArrayCnt : GlobalVars.ArraySize - 1;
                        int j = stVal;
                        double currPenet = ostacles.Parts[i].CourseAdjust;

                        if (currPenet > MaxHPenetValue)
                        {
                            for (; j > 0; j--)
                                MaxHPenetArray[j] = MaxHPenetArray[j - 1];

                            MaxHPenetArray[0] = ostacles.Parts[i];
                            MaxHPenetValue = currPenet;

                            if (MaxHPenetArrayCnt < GlobalVars.ArraySize)
                                MaxHPenetArrayCnt++;
                        }
                        else if (currPenet > MinHPenetValue)
                        {
                            for (; j > 0 && currPenet > MaxHPenetArray[j - 1].CourseAdjust; j--)
                                MaxHPenetArray[j] = MaxHPenetArray[j - 1];
                            MaxHPenetArray[j] = ostacles.Parts[i];

                            if (MaxHPenetArrayCnt < GlobalVars.ArraySize)
                                MaxHPenetArrayCnt++;
                            MinHPenetValue = MaxHPenetArray[MaxHPenetArrayCnt - 1].CourseAdjust;
                        }
                        else if (MaxHPenetArrayCnt < GlobalVars.ArraySize)
                        {
                            MaxHPenetArray[MaxHPenetArrayCnt] = ostacles.Parts[i];
                            MinHPenetValue = currPenet;
                            MaxHPenetArrayCnt++;
                        }

                    }
                }

                for (int i = 0; i < MaxHPenetArrayCnt - 1; i++)
                {
                    for (int j = i + 1; j < MaxHPenetArrayCnt; j++)
                    {
                        if (MaxHPenetArray[i].Owner == MaxHPenetArray[j].Owner)
                        {
                            if (MaxHPenetArray[i].MOC < MaxHPenetArray[j].MOC)
                                MaxHPenetArray[i] = MaxHPenetArray[j];

                            MaxHPenetArray[j] = MaxHPenetArray[--MaxHPenetArrayCnt];
                        }
                    }
                }

                for (int i = 0; i < MaxHPenetArrayCnt; i++)
                {
                    Obstruction obs = new Obstruction();
                    int owner = MaxHPenetArray[i].Owner;
                    //double MinimumAltitude = 0;
                    double RequiredClearance = 0;
                    bool CloseIn = true;
                    bool inArea = false;

                    obs.VerticalStructureObstruction = new FeatureRef(ostacles.Obstacles[owner].Identifier);    //ostacles.Obstacles[i].pFeature.GetFeatureRef();


                    for (int j = 0; j < ostacles.Obstacles[owner].PartsNum; j++)
                    {
                        if (relation.Disjoint(ostacles.Parts[ostacles.Obstacles[owner].Parts[j]].pPtPrj))
                            continue;

                        //MinimumAltitude = Math.Max(MinimumAltitude, ostacles.Parts[ostacles.Obstacles[owner].Parts[j]].hPenet);
                        RequiredClearance = Math.Max(RequiredClearance, ostacles.Parts[ostacles.Obstacles[owner].Parts[j]].MOC);
                        inArea = true;

                        if (!ostacles.Parts[ostacles.Obstacles[owner].Parts[j]].Ignored)
                            CloseIn = false;
                        //if (ostacles.Parts[ostacles.Obstacles[owner].Parts[j]].Prima)		isPrimary |= 1;
                        //else																isPrimary |= 2;
                    }

                    if (!inArea)
                        continue;

                    obs.CloseIn = CloseIn;

                    //ReqH
                    double MinimumAltitude = ostacles.Obstacles[owner].Height + RequiredClearance + DER.pPtPrj[eRWY.PtDER].Z;

                    pDistanceVertical = new ValDistanceVertical();
                    pDistanceVertical.Uom = mUomVDistance;
                    pDistanceVertical.Value = Functions.ConvertHeight(MinimumAltitude);
                    obs.MinimumAltitude = pDistanceVertical;

                    //MOC
                    pDistance = new ValDistance();
                    pDistance.Uom = UomDistance.M;
                    pDistance.Value = RequiredClearance;
                    obs.RequiredClearance = pDistance;

                    //if ((isPrimary & 1) != 0)
                    pPrimProtectedArea.SignificantObstacle.Add(obs);
                }

                //============================================================================================================================

                //for (int i = 0; i < ostacles.Obstacles.Length; i++)
                //{
                //	Obstruction obs = new Obstruction();
                //	obs.VerticalStructureObstruction = new FeatureRef(ostacles.Obstacles[i].Identifier);	//ostacles.Obstacles[i].pFeature.GetFeatureRef();

                //	//double MinimumAltitude = 0;
                //	double RequiredClearance = 0;
                //	bool CloseIn = true;
                //	bool inArea = false;

                //	for (int j = 0; j < ostacles.Obstacles[i].PartsNum; j++)
                //	{
                //		if (relation.Disjoint(ostacles.Parts[ostacles.Obstacles[i].Parts[j]].pPtPrj))
                //			continue;

                //		//MinimumAltitude = Math.Max(MinimumAltitude, ostacles.Parts[ostacles.Obstacles[i].Parts[j]].hPenet);
                //		RequiredClearance = Math.Max(RequiredClearance, ostacles.Parts[ostacles.Obstacles[i].Parts[j]].MOC);
                //		inArea = true;

                //		if (!ostacles.Parts[ostacles.Obstacles[i].Parts[j]].Ignored)
                //			CloseIn = false;
                //		//if (ostacles.Parts[ostacles.Obstacles[i].Parts[j]].Prima)			isPrimary |= 1;
                //		//else																isPrimary |= 2;
                //	}

                //	if (!inArea)
                //		continue;

                //	obs.CloseIn = CloseIn;

                //	//ReqH
                //	double MinimumAltitude = ostacles.Obstacles[i].Height + RequiredClearance + DER.pPtPrj[eRWY.PtDER].Z;

                //	pDistanceVertical = new ValDistanceVertical();
                //	pDistanceVertical.Uom = mUomVDistance;
                //	pDistanceVertical.Value = Functions.ConvertHeight(MinimumAltitude);
                //	obs.MinimumAltitude = pDistanceVertical;

                //	//MOC
                //	pDistance = new ValDistance();
                //	pDistance.Uom = UomDistance.M;
                //	pDistance.Value = RequiredClearance;
                //	obs.RequiredClearance = pDistance;

                //	//if ((isPrimary & 1) != 0)
                //	pPrimProtectedArea.SignificantObstacle.Add(obs);

                //	//if (pSecProtectedArea != null && (isPrimary & 2) != 0)
                //	//	pSecProtectedArea.SignificantObstacle.Add(obs);
                //}
                //  END ======================================================

                //==========================================================================
                pSegmentLeg.DesignSurface.Add(pPrimProtectedArea);
            }

            // Trajectory ===========================================
            IGeometryCollection pPolyline = (IGeometryCollection)segment.PathPrj;

            Curve pCurve = new Curve();

            for (int j = 0; j < pPolyline.GeometryCount; j++)
            {
                IPointCollection pPath = (IPointCollection)pPolyline.Geometry[j];
                LineString pLineStringSegment = new LineString();

                for (int i = 0; i < pPath.PointCount; i++)
                {
                    Aran.Geometries.Point pLocation = Converters.ESRIPointToARANPoint((IPoint)Functions.ToGeo(pPath.Point[i]));
                    pLineStringSegment.Add(pLocation);
                }
                pCurve.Geo.Add(pLineStringSegment);
            }

            pSegmentLeg.Trajectory = pCurve;

            // Trajectory =============================================
            // END ====================================================

            return result;
        }

        private bool SaveProcedure(string procName)
        {
            DBModule.pObjectDir.ClearAllFeatures();

            mUomHDistance = uomDistHorzTab[GlobalVars.DistanceUnit];
            mUomVDistance = uomDistVerTab[GlobalVars.HeightUnit];
            mUomSpeed = uomSpeedTab[GlobalVars.SpeedUnit];

            //  Procedure =================================================================================================
            _Procedure = DBModule.pObjectDir.CreateFeature<StandardInstrumentDeparture>();

            //pProcedure.CommunicationFailureDescription
            //pProcedure.Description =
            //pProcedure.ID
            //pProcedure.Note =
            //pProcedure.ProtectsSafeAltitudeAreaId =

            _Procedure.CodingStandard = CodeProcedureCodingStandard.PANS_OPS;
            _Procedure.DesignCriteria = CodeDesignStandard.PANS_OPS;
            _Procedure.RNAV = false;
            _Procedure.FlightChecked = false;

            LandingTakeoffAreaCollection pLandingTakeoffAreaCollection = new LandingTakeoffAreaCollection();
            pLandingTakeoffAreaCollection.Runway.Add(DER.GetFeatureRefObject());
            _Procedure.Takeoff = pLandingTakeoffAreaCollection;

            AircraftCharacteristic IsLimitedTo = new AircraftCharacteristic();

            switch (AirCat)
            {
                case 0:
                    IsLimitedTo.AircraftLandingCategory = CodeAircraftCategory.A;
                    break;
                case 1:
                    IsLimitedTo.AircraftLandingCategory = CodeAircraftCategory.B;
                    break;
                case 2:
                    IsLimitedTo.AircraftLandingCategory = CodeAircraftCategory.C;
                    break;
                case 3:
                    IsLimitedTo.AircraftLandingCategory = CodeAircraftCategory.D;
                    break;
                case 4:
                    IsLimitedTo.AircraftLandingCategory = CodeAircraftCategory.E;
                    break;
                case 5:
                    IsLimitedTo.AircraftLandingCategory = CodeAircraftCategory.H;
                    break;
            }
            _Procedure.AircraftCharacteristic.Add(IsLimitedTo);

            FeatureRef featureRef = new FeatureRef();
            featureRef.Identifier = GlobalVars.CurrADHP.Identifier;

            FeatureRefObject featureRefObject = new FeatureRefObject();
            featureRefObject.Feature = featureRef;
            _Procedure.AirportHeliport.Add(featureRefObject);

			//pGuidanceServiceChose.Navaid = FinalNav.pFeature.GetFeatureRef();
			//pProcedure.GuidanceFacility.Add(pGuidanceServiceChose);

			_Procedure.Name = procName;// "OD RWY" + DER.Name;
			//_Procedure.Designator = procName;// "OD RWY";

            // Transition ==========================================================================
            ProcedureTransition pTransition = new ProcedureTransition();
            pTransition.DepartureRunwayTransition = pLandingTakeoffAreaCollection;
            pTransition.Type = CodeProcedurePhase.RWY;

            //     pTransition.Description =
            //     pTransition.ID =
            //     pTransition.Procedure =
            //     pTransition.TransitionId = TextBox0???.Text
            // Legs ======================================================================================================

            SegmentLeg pSegmentLeg;
            ProcedureTransitionLeg ptl;

            // Leg 1 Straight Departure ==================================================================================
            if (!CheckBox301.Checked)
            {
                pSegmentLeg = StraightDepartureLeg(ref _Procedure, IsLimitedTo);

                ptl = new ProcedureTransitionLeg();
                ptl.SeqNumberARINC = 1;
                ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef<AbstractSegmentLegRef>();
                pTransition.TransitionLeg.Add(ptl);
            }
            else             //if (CheckBox301.Checked)
            {
                // Standart Departure =====================================================================================
                // Leg 2 Standart Departure ===============================================================================
                // Start Point ============================================================================================

                //TerminalSegmentPoint pEndPoint = new TerminalSegmentPoint();

                ////        pStartPoint.IndicatorFACF =      ??????????
                ////        pStartPoint.LeadDME =            ??????????
                ////        pStartPoint.LeadRadial =         ??????????
                ////pStartPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.

                //SegmentPoint pSegmentPoint = pEndPoint;
                //pSegmentPoint.FlyOver = true;
                //pSegmentPoint.RadarGuidance = false;
                //pSegmentPoint.ReportingATC = CodeATCReporting.NO_REPORT;
                //pSegmentPoint.Waypoint = false;
                //pSegmentPoint.PointChoice = DER.pSignificantPoint[eRWY.PtDER];

                TerminalSegmentPoint pEndPoint = null;  //??????????????????????????????????
                                                        //  End Of Start Point ====================================================================================
                int i = 0;
                uint SeqNumberARINC = 0;

                while (i < TSC)
                {
                    bool first = i == 0;
                    TraceSegment currSegment = Trace[i];

                    if (i < TSC - 1)
                    {
                        TraceSegment nextSegment = Trace[i + 1];

                        if (nextSegment.SegmentCode == eSegmentType.straight && ((i == 0 && currSegment.LegType == CodeSegmentPath.VA) || (currSegment.LegType == CodeSegmentPath.DF)))
                        {
                            nextSegment.HStart = currSegment.HStart;

                            nextSegment.ptIn = currSegment.ptIn;
                            nextSegment.DirIn = currSegment.DirIn;
                            nextSegment.Length += currSegment.Length;

                            nextSegment.StCoords = currSegment.StCoords;

                            if (nextSegment.PathPrj != null && currSegment.PathPrj != null)
                            {
                                ITopologicalOperator2 pTopo = (ITopologicalOperator2)currSegment.PathPrj;
                                pTopo.IsKnownSimple_2 = false;
                                pTopo.Simplify();

                                nextSegment.PathPrj = (IPolyline)pTopo.Union(nextSegment.PathPrj);

                                pTopo = (ITopologicalOperator2)nextSegment.PathPrj;
                                pTopo.IsKnownSimple_2 = false;
                                pTopo.Simplify();

                                if (!nextSegment.PathPrj.IsEmpty)
                                {
                                    nextSegment.pProtectArea = (IPolygon)pTopo.Buffer(nextSegment.SeminWidth);
                                    pTopo = (ITopologicalOperator2)nextSegment.pProtectArea;
                                    pTopo.IsKnownSimple_2 = false;
                                    pTopo.Simplify();
                                }
                            }
                            currSegment = nextSegment;
                            i++;
                        }
                    }

                    pSegmentLeg = StandartDepartureLeg(currSegment, ref _Procedure, IsLimitedTo, ref pEndPoint, first);

                    ptl = new ProcedureTransitionLeg();

                    ptl.SeqNumberARINC = ++SeqNumberARINC;
                    ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef<AbstractSegmentLegRef>();
                    pTransition.TransitionLeg.Add(ptl);

                    i++;
                }
            }
            //============================================================================================================
            _Procedure.FlightTransition.Add(pTransition);

            try
            {
                DBModule.pObjectDir.SetRootFeatureType(FeatureType.StandardInstrumentDeparture);
                //DBModule.pObjectDir.Commit();

                bool saveRes = DBModule.pObjectDir.Commit(new FeatureType[]{
                        FeatureType.DesignatedPoint,
                        FeatureType.AngleIndication,
                        FeatureType.DistanceIndication,
                        FeatureType.StandardInstrumentDeparture,
                        FeatureType.StandardInstrumentArrival,
                        FeatureType.InstrumentApproachProcedure,
                        FeatureType.ArrivalFeederLeg,
                        FeatureType.ArrivalLeg,
                        FeatureType.DepartureLeg,
                        FeatureType.FinalLeg,
                        FeatureType.InitialLeg,
                        FeatureType.IntermediateLeg,
                        FeatureType.MissedApproachLeg});

                GlobalVars.gAranEnv.RefreshAllAimLayers();

                return saveRes;
            }
            catch (Exception ex)
            {
                throw new Exception("Error on commit." + "\r\n" + ex.Message);
            }
            //return false;
        }

    }
}
