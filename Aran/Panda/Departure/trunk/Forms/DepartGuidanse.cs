using System;
using System.Drawing;
using System.Windows.Forms;
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
using Aran.Aim;
using Aran.Aim.Data;

namespace Aran.PANDA.Departure
{
    [System.Runtime.InteropServices.ComVisible(false)]
    public partial class CDepartGuidanse : Form
    {
        #region declerations
        private const short noChange = 0;
        private const short goParalel = 1;
        private const short expandAngle = 2;
        //private const double NMCoeff = 1.852;

        private IGraphicsContainer pGraphics;
        private IScreenCapture screenCapture;
        //private ADHPType CurrADHP;

        private IPointCollection MPtCollection;
        private IPointCollection TracPoly;

        private IPointCollection UnitedPolygon;
        private IPointCollection BasePoints;
        private IPointCollection BaseArea;
        private IPointCollection pPolygon;
        private IPointCollection ZNR_Poly;
        private IPointCollection TurnArea;
        private IPointCollection pFIXPoly;

        private IPolygon StraightPrimPoly;
        private IPolygon StraightSecPoly;

        private IPoint ptCenter;
        private IPolygon pCircle;

        private IPointCollection PrimPoly;
        private IPointCollection SecL;
        private IPointCollection SecR;
        private IPolygon SecPoly;

        private IPolygon pTermFIXTolerArea;

        private IPoint PtDerShift;
        private IPoint TurnFixPnt;
        private IPoint NJoinPt;
        private IPoint FJoinPt;

        private IPoint NOutPt;
        private IPoint FOutPt;

        private WPT_FIXType TurnDirector;
        private WPT_FIXType WPt702;

        private IPoint OutPt;
        private IPoint FlyBy;

        //private ICommandItem[] _mTool;

        private ObstacleContainer oFullList;
        private ObstacleContainer oZNRList;
        private ObstacleContainer oAllStraightList;
        private ObstacleContainer oTrnAreaList;

        private ObstacleData PrevDet;
        private ObstacleData CurrDetObs;

        private IPoint TerFixPnt;
        private NavaidType[] TerInterNavDat;
        private NavaidType[] FrontList;
        private NavaidType[] TurnInterDat;

        private double DirCourse;

        private int AirCat;
        private int RightMinAngle;
        private int RightMaxAngle;
        private int LeftMinAngle;
        private int LeftMaxAngle;
        private int iDominicObst;

        private int AdjustDir;
        private int CurrPage;
        private int idPDGMax;

        private int TurnDir;
        //private int NState = 0; 
        //private int FState = 0; 
        private int TrackAdjust;

        private double RMin;
        private double MOCLimit;
        //private double ZRSegment;
        private double TurnAreaMaxd0;
        private double fReachedA;
        private double hKK;
        private double MaxReq;


        private double fStraightDepartTermAlt;
        private double InitialTurnFixAltitude;
        private double AltitudeAtTurnFixPnt;
        private double NReqCorrAngle;
        private double FReqCorrAngle;
        private double FCorrAngle = 0;
        private double NCorrAngle = 0;
        private double fBankAngle;

        private double DirToNav;
        private double drPDGMax;
        private double DerShift;
        private double MinAngle;
        private double hMinTurn;
        private double hMaxTurn;
        private double OutAzt;
        private double MinDR;

        private double RWYDir;
        private double DepDir;
        private double fIAS;

        private double CurrPDG;
        private double TACurrPDG;

        private double MinGPDG;
        private double MinPDG;

        private double MinCurrPDG;

        private IPolyline KKFixMax;
        private IPolyline K1K1;
        private IPolyline KK;

        private string RecommendStr;
        private string PrevPDG;

        private bool CheckState;
        private bool Report;
        private StandardInstrumentDeparture _Procedure;

        private RWYType[] RWYList;
        private WPT_FIXType[] FixAngl;
        private WPT_FIXType[] FixWPT;
        private WPT_FIXType[] FixBox702;

        private ReportPoint[] RoutsPoints;
        private double GuidAllLen;
        private double fZNRLen;
        private RWYType DER;

        private CReports ReportsFrm;
        private CNomInfo NomInfoFrm;
        private int ArrayIVariant = 1;
        private Label[] Label1;

        private int HelpContextID;
        private bool bFormInitialised = false;

        ReportFile AccurRep;
        ReportFile GuidLogRep;
        ReportFile GuidProtRep;
        ReportFile GuidGeomRep;
        #endregion

        public CDepartGuidanse()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();
            screenCapture = GlobalVars.gAranEnv.GetScreenCapture(FeatureType.StandardInstrumentDeparture.ToString());

            bFormInitialised = true;

            Report = false;
            MinCurrPDG = 0.0;
            //ComboBox004.Items.Clear();

            int i, n;// = GlobalVars.ADHPList.Length;

            //if (n <= 0)
            //{
            //	MessageBox.Show(Resources.str612, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    this.Close();
            //    return;
            //}

            //for (i = 0; i < n; i++)
            //    ComboBox004.Items.Add(GlobalVars.ADHPList[i].Name);

            GlobalVars.ButtonControl1State = true;
            GlobalVars.ButtonControl2State = true;
            GlobalVars.ButtonControl3State = true;
            GlobalVars.ButtonControl4State = true;
            GlobalVars.ButtonControl5State = true;
            GlobalVars.ButtonControl6State = true;
            GlobalVars.ButtonControl7State = true;
            GlobalVars.ButtonControl8State = true;

            ToolTip1.SetToolTip(TextBox501, Resources.str15463 + Functions.ConvertHeight(PANS_OPS_DataBase.dpGui_Ar1.Value, eRoundMode.NEAREST).ToString() + " " + GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit); // "??????????? ???????? ?? ?????????? PANS-OPS "" ?"
            ToolTip1.SetToolTip(TextBox502, ToolTip1.GetToolTip(TextBox501));

            pGraphics = GlobalVars.GetActiveView().GraphicsContainer;

            HelpContextID = 6100;

            //#warning How to find ICommandBars pCommandBars?
            //_mTool = new ICommandItem[8];
            //ICommandBars pCommandBars = GlobalVars.Application.Document.CommandBars;
            //_mTool[0] = pCommandBars.Find("Depart.CircleVTool", false, false);
            //_mTool[1] = pCommandBars.Find("Depart.CLTool", false, false);
            //_mTool[2] = pCommandBars.Find("Depart.StraightVTool", false, false);
            //_mTool[3] = pCommandBars.Find("Depart.TurnAreaVTool", false, false);
            //_mTool[4] = pCommandBars.Find("Depart.SecondaryVTool", false, false);
            //_mTool[5] = pCommandBars.Find("Depart.NomTrackVTool", false, false);
            //_mTool[6] = pCommandBars.Find("Depart.KKVTool", false, false);
            //_mTool[7] = pCommandBars.Find("Depart.FIXVTool", false, false);

            GlobalVars.pCircleElem = null;
            GlobalVars.StraightAreaElem = null;

            GlobalVars.TurnAreaElem = null;

            GlobalVars.PrimElem = null;
            GlobalVars.SecRElem = null;
            GlobalVars.SecLElem = null;

            GlobalVars.NomTrackElem = null;
            GlobalVars.StrTrackElem = null;

            GlobalVars.KKElem = null;
            GlobalVars.K1K1Elem = null;
            GlobalVars.CLElem = null;
            GlobalVars.FIXElem = null;

            CurrPage = 0;
            MultiPage1.SelectedIndex = CurrPage;

            GlobalVars.RModel = 0.0;

            TextBox1011.Left = TextBox1001.Left;
            TextBox1012.Left = TextBox1002.Left;
            TextBox1013.Left = TextBox1003.Left;
            TextBox1014.Left = TextBox1004.Left;

            OptionButton1004.Checked = true;

            StraightPrimPoly = (IPolygon)new ESRI.ArcGIS.Geometry.Polygon();
            pPolygon = new ESRI.ArcGIS.Geometry.Polygon();
            // ========================================================
            ToolTip1.SetToolTip(TextBox205, Resources.str15465 + (PANS_OPS_DataBase.dpGui_Ar1_Wd.Value).ToString() + " " + PANS_OPS_DataBase.dpGui_Ar1_Wd.Unit); // "?????????? ???????? "
            ToolTip1.SetToolTip(ComboBox101, Resources.str15466 + PANS_OPS_DataBase.dpGui_Ar1_Wd.Value.ToString() + Resources.str15467 + PANS_OPS_DataBase.dpTr_AdjAngle.Value.ToString() + "°"); // "??? ??????????????? ??????????? ?"" ? ? ?"
                                                                                                                                                                                                  // "?????????? PANS-OPS: ?? ????? 20 ??"
            ToolTip1.SetToolTip(TextBox204, Resources.str15468 + (Functions.ConvertDistance(PANS_OPS_DataBase.dpStr_Gui_dist.Value, eRoundMode.NEAREST)).ToString() + " " + PANS_OPS_DataBase.dpStr_Gui_dist.Unit); // "?????????? ???????? "

            // '=============================================================================================

            fBankAngle = PANS_OPS_DataBase.dpT_Bank.Value;
            TextBox402.Text = fBankAngle.ToString();

            pGraphics = GlobalVars.GetActiveView().GraphicsContainer;

            ComboBox003.Items.Clear();
            for (i = 0; i < GlobalVars.EnrouteMOCValues.Length; i++)
                ComboBox003.Items.Add(Functions.ConvertHeight(GlobalVars.EnrouteMOCValues[i], eRoundMode.SPECIAL).ToString());

            ComboBox401.Items.Clear();
            ComboBox401.Items.Add(Resources.str03312);
            ComboBox401.Items.Add(Resources.str03313);

            ComboBox501.Items.Clear();
            ComboBox501.Items.Add(Resources.str34302);
            ComboBox501.Items.Add(Resources.str34303);

            // ============================================

            ComboBox701.Items.Clear();
            // ------------___________________________---------------------

            NomInfoFrm = new CNomInfo();
            ReportsFrm = new CReports();
            ReportsFrm.SetBtn(ReportBtn, GlobalVars.ReportHelpIDGuidance);

            TextBox1012.Text = (3.3).ToString();

            // CreateLog Me.Caption
            // ===========================================================
            this.Text = Resources.str00022;

            MultiPage1.TabPages[0].Text = Resources.str00200;
            MultiPage1.TabPages[1].Text = Resources.str00210;
            MultiPage1.TabPages[2].Text = Resources.str00220;
            MultiPage1.TabPages[3].Text = Resources.str00230;
            MultiPage1.TabPages[4].Text = Resources.str00240;
            MultiPage1.TabPages[5].Text = Resources.str00250;
            MultiPage1.TabPages[6].Text = Resources.str00260;
            MultiPage1.TabPages[7].Text = Resources.str00270;
            MultiPage1.TabPages[8].Text = Resources.str00280;
            MultiPage1.TabPages[9].Text = Resources.str00290;
            MultiPage1.TabPages[10].Text = Resources.str00291;

            // ===============================================================
            PrevBtn.Text = Resources.str00002;
            NextBtn.Text = Resources.str00003;
            OkBtn.Text = Resources.str00004;
            CancelBtn.Text = Resources.str00005;
            ReportBtn.Text = Resources.str00006;
            // =====================Page1=================================
            Label001.Text = Resources.str02001;
            Label002.Text = Resources.str02002;
            Label003.Text = Resources.str02003;
            Label004.Text = Resources.str02004;
            Label005.Text = Resources.str02005;
            Label006.Text = Resources.str02006;
            // Label007.Caption = LoadResString(2007)
            Label008.Text = Resources.str02008;
            Label013.Text = Resources.str01009;
            //Label016.Text = Resources.str73;

            Label2.Text = Resources.str03010;

            LabelA.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label009.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
            Label010.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label011.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
            Label012.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;

            Frame001.Text = Resources.str20010;
            Frame002.Text = Resources.str20011;
            Option001.Text = Resources.str20111;
            Option002.Text = Resources.str20112;
            // =====================Page2==================================
            Label101.Text = Resources.str02101;
            Label104.Text = Resources.str15276;
            Frame101.Text = Resources.str21101;

            // ======================Page3==================================
            Label203.Text = Resources.str02203;
            Label204.Text = Resources.str02204;
            Label205.Text = Resources.str02205;
            Label206.Text = Resources.str02206;

            Label209.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label210.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
            Label211.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;

            // Label212.Caption = LoadResString(2212)
            // =====================Page4===================================
            _Label300_0.Text = Resources.str03105;
            _Label300_2.Text = Resources.str02401;
            Label301.Text = Resources.str02301;
            Label303.Text = Resources.str02303;
            Label305.Text = Resources.str02305;
            Label307.Text = Resources.str02307;
            Label309.Text = Resources.str02309;
            Label310.Text = Resources.str02310;
            Label311.Text = Resources.str00000;

            _Label300_1.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label308.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            CheckBox301.Text = Resources.str23301;
            // ======================Page5====================================
            Label402.Text = Resources.str02402;
            Label403.Text = GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Unit;
            Label404.Text = Resources.str02404;
            Label406.Text = Resources.str02406;
            Label407.Text = Resources.str00902 + "(" + GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Unit + ")";
            Label408.Text = Resources.str00903 + "(" + GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Unit + ")";

            Label411.Text = Resources.str02411;

            OptionButton401.Text = Resources.str24011;
            OptionButton402.Text = Resources.str24012;
            OptionButton403.Text = Resources.str24013;
            CheckBox401.Text = Resources.str24401;
            Frame401.Text = Resources.str33001;
            // =====================Page6====================================
            Label501.Text = Resources.str02501;
            Label502.Text = Resources.str02502;
            Label503.Text = Resources.str02503;
            Label504.Text = Resources.str02504;
            Label507.Text = Resources.str02507;
            Label508.Text = Resources.str02508;
            Label510.Text = Resources.str02510;
            Label512.Text = Resources.str02512;

            Label513.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label514.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;

            CheckBox501.Text = Resources.str25301;
            // ======================Page7===================================
            Label601.Text = Resources.str02601;
            Label604.Text = Resources.str02604;


            Label609.Text = Resources.str02609;
            Label611.Text = Resources.str02611;
            Label613.Text = Resources.str15474 + PANS_OPS_DataBase.dpT_TechToleranc.Value.ToString() + " " + Resources.str00057 + "):"; // "???. ???. ???. "" ???.:"

            Label615.Text = Resources.str02615;
            Label617.Text = Resources.str02617;
            Label619.Text = Resources.str02619;
            Label621.Text = Resources.str02621;
            //Label622.Text =
            Label623.Text = Resources.str02623;
            Label606.Text = Resources.str02625;

            Label627.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label628.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label607.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;

            Label629.Text = GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Unit;
            Label630.Text = GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Unit;
            Label631.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;

            Label632.Text = Resources.str00058;
            Label633.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
            Label634.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
            Label635.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label636.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label637.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;

            Option601.Text = Resources.str26101;
            Option602.Text = Resources.str26102;
            Frame601.Text = Resources.str26001;
            Frame602.Text = Resources.str26002;
            // ======================Page8=====================================
            Label701.Text = Resources.str02701;

            // ????????????????????????????????????????
            Label703.Text = Resources.str03605;
            // ????????????????????????????????????????

            Label705.Text = Resources.str00057;
            Label706.Text = Resources.str02706;
            Label708.Text = Resources.str02708;
            Label709.Text = Resources.str02709;
            Label58.Text = Resources.str03610;
            Label711.Text = Resources.str03612;
            OptionButton701.Text = Resources.str27101;
            OptionButton702.Text = Resources.str27102;
            OptionButton703.Text = Resources.str27103;
            CheckBox701.Text = Resources.str27301;

            ComboBox703.Items.Clear();
            ComboBox703.Items.Add(Resources.str36304);
            ComboBox703.Items.Add(Resources.str36305);
            // =======================Page9=====================================
            Label801.Text = Resources.str02801;
            Label802.Text = Resources.str02802;
            Frame801.Text = Resources.str28001;
            Frame802.Text = Resources.str28002;
            OptionButton801.Text = Resources.str28101;
            OptionButton802.Text = Resources.str28102;
            OptionButton803.Text = Resources.str28103;
            OptionButton804.Text = Resources.str28104;
            OptionButton805.Text = Resources.str28105;
            OptionButton806.Text = Resources.str28106;
            // =======================Page10====================================
            _Frame901_0.Text = Resources.str29001;
            _Frame901_1.Text = Resources.str29002;

            CheckBox901.Text = Resources.str29301;
            CheckBox902.Text = Resources.str29302;
            CheckBox903.Text = Resources.str29303;
            CheckBox904.Text = Resources.str29304;
            CheckBox905.Text = Resources.str29305;
            CheckBox906.Text = Resources.str29306;

            // =======================Page11====================================
            Label1001_01.Text = Resources.str39017;
            Label1001_03.Text = Resources.str39018;
            Label1001_05.Text = Resources.str39016;
            Label1001_07.Text = Resources.str39019;
            Label1001_09.Text = Resources.str02911;
            Label1001_11.Text = Resources.str02912;
            Label1001_13.Text = Resources.str02914;
            Label1001_17.Text = Resources.str03905;
            Label1001_19.Text = Resources.str03501;

            Label1001_02.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
            Label1001_04.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label1001_06.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label1001_08.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label1001_14.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label1001_16.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;

            Frame1001.Text = Resources.str39004;
            Frame1003.Text = Resources.str03903;

            OptionButton1001.Text = Resources.str29111;
            OptionButton1002.Text = Resources.str29112;
            OptionButton1003.Text = Resources.str29113;
            OptionButton1004.Text = Resources.str39001;
            OptionButton1005.Text = Resources.str39002;
            OptionButton1006.Text = Resources.str35101;
            OptionButton1007.Text = Resources.str35102;

            // =================================================================

            // 2007
            Label1 = new Label[11] { Label1_00, Label1_01, Label1_02, Label1_03, Label1_04, Label1_05, Label1_06, Label1_07, Label1_08, Label1_09, Label1_10 };
            Label1[0].Text = Resources.str00200;
            Label1[1].Text = Resources.str00210;
            Label1[2].Text = Resources.str00220;
            Label1[3].Text = Resources.str00230;
            Label1[4].Text = Resources.str00240;
            Label1[5].Text = Resources.str00250;
            Label1[6].Text = Resources.str00260;
            Label1[7].Text = Resources.str00270;
            Label1[8].Text = Resources.str00280;
            Label1[9].Text = Resources.str00290;
            Label1[10].Text = Resources.str00291;
            // 'jamil 2007

            FocusStepCaption(0);

            MultiPage1.Top = -21;
            Height = Height - 21;
            Frame1.Top = Frame1.Top - 21;

            ShowPanelBtn.Checked = false;
            this.Width = 545;

            ComboBox501.SelectedIndex = 0;
            ComboBox301.SelectedIndex = 0;
            ComboBox401.SelectedIndex = 0;
            //ComboBox004.SelectedIndex = 0;
            ComboBox001.Items.Clear();
            ComboBox101.Items.Clear();

            DBModule.FillRWYList(out RWYList, GlobalVars.CurrADHP);

            n = RWYList.Length;

            if (n == 0)
            {
                MessageBox.Show(Resources.str15056, "PANDA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            for (i = 0; i < n; i++)
                ComboBox001.Items.Add(RWYList[i].Name);

            DBModule.FillWPT_FIXList(out GlobalVars.WPTList, GlobalVars.CurrADHP, GlobalVars.MaxNAVDist);
            DBModule.FillNavaidList(out GlobalVars.NavaidList, out GlobalVars.DMEList, GlobalVars.CurrADHP, GlobalVars.MaxNAVDist);

            // ====================================================================
            n = GlobalVars.WPTList.Length;
            if (n > 0)
            {
                FixAngl = new WPT_FIXType[n];

                int j = 0;
                for (i = 0; i < n; i++)
                {
                    if (GlobalVars.WPTList[i].TypeCode == eNavaidType.VOR || GlobalVars.WPTList[i].TypeCode == eNavaidType.NDB)
                        FixAngl[j++] = GlobalVars.WPTList[i];
                }

                if (j > 0)
                    System.Array.Resize<WPT_FIXType>(ref FixAngl, j);
                else
                {
                    FixAngl = new WPT_FIXType[0];
                    OptionButton702.Enabled = false;
                }
            }
            else
            {
                FixAngl = new WPT_FIXType[0];
                OptionButton703.Enabled = false;
                OptionButton702.Enabled = false;
                ComboBox701.Enabled = false;
            }
            // ====================================================================
            RMin = 50.0 * System.Math.Round(0.02 * 600.0 / PANS_OPS_DataBase.dpPDG_Nom.Value + 0.4999);
            if (RMin < 20000.0)
                RMin = 20000.0;
            TextBox003.Text = Functions.ConvertDistance(RMin, eRoundMode.CEIL).ToString();
            //TextBox007.Text = Functions.ConvertHeight(600.0 + GlobalVars.CurrADHP.Elev, eRoundMode.rmNERAEST).ToString();
            //TextBox003_Validating(TextBox003, new System.ComponentModel.CancelEventArgs());

            ComboBox001.SelectedIndex = 0;
            //GlobalVars.CurrCmd = 3;
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

        private void FillAllStraightList()
        {
            IPointCollection pMaxPolygon = new ESRI.ArcGIS.Geometry.Polygon();

            double d0 = (Functions.ReturnDistanceInMeters(DER.pPtPrj[eRWY.PtDER], GlobalVars.CurrADHP.pPtPrj) + GlobalVars.RModel) * 1.18;
            double fWd = 0.5 * (PANS_OPS_DataBase.dpNGui_Ar1_Wd.Value + PANS_OPS_DataBase.dpGui_Ar1_Wd.Value);

            IPoint pt0 = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], RWYDir - 90.0, fWd);
            IPoint pt5 = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], RWYDir + 90.0, fWd);

            IPoint pt1 = Functions.PointAlongPlane(pt0, RWYDir - PANS_OPS_DataBase.dpAr1_OB_TrAdj.Value - PANS_OPS_DataBase.dpAr1_IB_TrAdj.Value, d0);
            IPoint pt4 = Functions.PointAlongPlane(pt5, RWYDir + PANS_OPS_DataBase.dpAr1_OB_TrAdj.Value + PANS_OPS_DataBase.dpAr1_IB_TrAdj.Value, d0);

            pMaxPolygon.AddPoint(pt0);
            pMaxPolygon.AddPoint(pt1);
            pMaxPolygon.AddPoint(pt4);
            pMaxPolygon.AddPoint(pt5);

            pMaxPolygon = (IPointCollection)Functions.PolygonIntersection(pMaxPolygon, pCircle);
            //while(true)
            //Functions.DrawPolygon(pMaxPolygon, -1, esriSimpleFillStyle.esriSFSDiagonalCross);
            //Application.DoEvents();

            Functions.GetObstListInPoly(oFullList, out oAllStraightList, (IPolygon)pMaxPolygon);
        }

        private double CalcShift()
        {
            int k = ComboBox101.SelectedIndex;

            PtDerShift = new ESRI.ArcGIS.Geometry.Point();
            IConstructPoint ptConstr = (ESRI.ArcGIS.Geometry.IConstructPoint)PtDerShift;

            ptConstr.ConstructAngleIntersection(DER.pPtPrj[eRWY.PtDER], GlobalVars.DegToRadValue * (RWYDir + 90.0), FrontList[k].pPtPrj, GlobalVars.DegToRadValue * DepDir);
            //Functions.DrawPoint(PtDerShift, 0);

            DerShift = Functions.ReturnDistanceInMeters(PtDerShift, DER.pPtPrj[eRWY.PtDER]) * Functions.SideDef(DER.pPtPrj[eRWY.PtDER], RWYDir, PtDerShift);
            return DerShift;
        }

        private int CreateStraightZones(double PDG, double Angle = -100, bool DrawFlag = false)
        {
            if (Angle != -100.0)
                DepDir = RWYDir + Angle;

            DerShift = CalcShift();

            int k = ComboBox101.SelectedIndex;
            Functions.CreateDeparturePolygon(ref pPolygon, GlobalVars.RModel, ArrayIVariant, DER, DerShift, DepDir, RWYDir, PDG, FrontList[k].TypeCode, FrontList[k].pPtPrj);

            IPolyline p120Line = (ESRI.ArcGIS.Geometry.IPolyline)(new Polyline());
            p120Line.FromPoint = pPolygon.Point[1];
            p120Line.ToPoint = pPolygon.Point[4];

            IPointCollection lPoly = new ESRI.ArcGIS.Geometry.Polygon();
            IPointCollection rPoly = new ESRI.ArcGIS.Geometry.Polygon();
            IPolygon prmPoly = (IPolygon)new ESRI.ArcGIS.Geometry.Polygon();

            Functions.CreateNavaidZone(FrontList[k].pPtPrj, DepDir, FrontList[k].TypeCode, 1.3, lPoly, rPoly, prmPoly as IPointCollection);

            IPoint LPt = lPoly.Point[0];
            IPoint RPt = rPoly.Point[3];

            double LfFw = Functions.ReturnAngleInDegrees(LPt, lPoly.Point[1]);
            double LfBw = Functions.ReturnAngleInDegrees(LPt, lPoly.Point[5]);
            double RtFw = Functions.ReturnAngleInDegrees(RPt, rPoly.Point[2]);
            double RtBw = Functions.ReturnAngleInDegrees(RPt, rPoly.Point[4]);

            IPointCollection AllPolys = new ESRI.ArcGIS.Geometry.Polygon();
            AllPolys.AddPoint(lPoly.Point[5]);
            AllPolys.AddPoint(lPoly.Point[0]);
            AllPolys.AddPoint(lPoly.Point[1]);

            AllPolys.AddPoint(rPoly.Point[2]);
            AllPolys.AddPoint(rPoly.Point[3]);
            AllPolys.AddPoint(rPoly.Point[4]);
            AllPolys.AddPoint(lPoly.Point[5]);

            ITopologicalOperator2 pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)lPoly;
            pTopoOper.IsKnownSimple_2 = false;
            pTopoOper.Simplify();

            pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)rPoly;
            pTopoOper.IsKnownSimple_2 = false;
            pTopoOper.Simplify();

            StraightSecPoly = (IPolygon)pTopoOper.Union(lPoly as IGeometry);
            pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)StraightSecPoly;
            pTopoOper.IsKnownSimple_2 = false;
            pTopoOper.Simplify();

            IClone pSource = (ESRI.ArcGIS.esriSystem.IClone)pPolygon;
            IPointCollection pPolygon1 = (ESRI.ArcGIS.Geometry.IPointCollection)pSource.Clone();

            StraightPrimPoly = Functions.PolygonIntersection((IPolygon)pPolygon1, (IPolygon)prmPoly);
            StraightSecPoly = Functions.PolygonIntersection((IPolygon)pPolygon1, (IPolygon)StraightSecPoly);

            //Functions.DrawPolygon(StraightPrimPoly, -1, esriSimpleFillStyle.esriSFSCross);
            //Functions.DrawPolygon(StraightSecPoly, -1, esriSimpleFillStyle.esriSFSBackwardDiagonal);
            //Application.DoEvents();

            Functions.GetZNRObstList(oAllStraightList, out oZNRList, DER, DepDir, PDG, StraightPrimPoly, StraightSecPoly, FrontList[k].pPtPrj);
            int result = Functions.CalcPDGToTop(oZNRList, pPolygon, ArrayIVariant, DER, DerShift, DepDir, RWYDir, MOCLimit, FrontList[k].TypeCode, FrontList[k].pPtPrj);

            bool haveTail = (CurrPage >= 4) && CheckBox401.Checked && OptionButton401.Checked;

            if (haveTail)
            {
                double d0 = DER.Length + DER.ClearWay - PANS_OPS_DataBase.dpT_Init.Value;
                IPoint pt0 = Functions.PointAlongPlane(pPolygon.Point[0], RWYDir + 180.0, d0);
                IPoint ptN = Functions.PointAlongPlane(pPolygon.Point[5], RWYDir + 180.0, d0);
                pPolygon.AddPoint(pt0, 0);
                pPolygon.AddPoint(ptN);
            }

            pSource = (ESRI.ArcGIS.esriSystem.IClone)pPolygon;
            pPolygon1 = (ESRI.ArcGIS.Geometry.IPointCollection)pSource.Clone();

            StraightPrimPoly = Functions.PolygonIntersection((IPolygon)pPolygon1, (IPolygon)prmPoly);
            UnitedPolygon = Functions.PolygonIntersection((IPolygon)AllPolys, (IPolygon)pPolygon1) as IPointCollection;
            UnitedPolygon = Functions.ReArrangePolygon(UnitedPolygon, DER.pPtPrj[eRWY.PtDER], RWYDir, haveTail);

            if (DrawFlag)
            {
                IGeometryCollection geoColl = (ESRI.ArcGIS.Geometry.IGeometryCollection)StraightSecPoly;
                IPolyline pLine = null;

                if (geoColl.GeometryCount > 1)
                {
                    double dlMax = 1000000.0;
                    double drMax = 1000000.0;
                    IPoint pt0 = null, pt1 = null;
                    IPointCollection scPoly = (IPointCollection)StraightSecPoly;

                    for (int i = 0; i < scPoly.PointCount; i++)
                    {
                        double d0 = Functions.ReturnDistanceInMeters(DER.pPtPrj[eRWY.PtDER], scPoly.Point[i]);
                        if (d0 >= dlMax)
                            continue;

                        double fTmp = Functions.Point2LineDistancePrj(scPoly.Point[i], LPt, LfFw);
                        if (fTmp < 0.001)
                        {
                            dlMax = d0;
                            pt0 = scPoly.Point[i];
                        }

                        fTmp = Functions.Point2LineDistancePrj(scPoly.Point[i], LPt, LfBw);
                        if (fTmp < 0.001)
                        {
                            dlMax = d0;
                            pt0 = scPoly.Point[i];
                        }

                        fTmp = Functions.Point2LineDistancePrj(scPoly.Point[i], RPt, RtFw);
                        if (fTmp < 0.001)
                        {
                            drMax = d0;
                            pt1 = scPoly.Point[i];
                        }

                        fTmp = Functions.Point2LineDistancePrj(scPoly.Point[i], RPt, RtBw);
                        if (fTmp < 0.001)
                        {
                            drMax = d0;
                            pt1 = scPoly.Point[i];
                        }
                    }

                    if (pt0 == null || pt1 == null)
                        TextBox204.Text = Resources.str15272;
                    else
                    {
                        pLine = (ESRI.ArcGIS.Geometry.IPolyline)(new Polyline());
                        pLine.FromPoint = pt0;
                        pLine.ToPoint = pt1;

                        double Azt = Functions.ReturnAngleInDegrees(pt0, pt1);

                        IPoint pt2 = new ESRI.ArcGIS.Geometry.Point();
                        IConstructPoint Constructor = (ESRI.ArcGIS.Geometry.IConstructPoint)pt2;
                        Constructor.ConstructAngleIntersection(pt0, GlobalVars.DegToRadValue * Azt, DER.pPtPrj[eRWY.PtDER], GlobalVars.DegToRadValue * RWYDir);

                        double d0 = Functions.Point2LineDistancePrj(pt2, DER.pPtPrj[eRWY.PtDER], RWYDir + 90.0);
                        TextBox204.Text = Functions.ConvertDistance(d0, eRoundMode.NEAREST).ToString();
                    }
                }
                else
                    TextBox204.Text = Resources.str15272; // "????????? ?? ??????????????"

                if (GlobalVars.StraightAreaElem != null)
                {
                    if (GlobalVars.StraightAreaElem is IGroupElement)
                        for (int i = 0; i < (GlobalVars.StraightAreaElem as IGroupElement).ElementCount; i++)
                            Functions.DeleteGraphicsElement((GlobalVars.StraightAreaElem as IGroupElement).Element[i]);
                    else
                        Functions.DeleteGraphicsElement(GlobalVars.StraightAreaElem);
                    GlobalVars.StraightAreaElem = null;
                }

                IGroupElement pGroupElement = (ESRI.ArcGIS.Carto.IGroupElement)(new GroupElement());
                pPolygon1 = Functions.PolygonIntersection(pCircle as IPolygon, StraightPrimPoly as IPolygon) as IPointCollection;
                pGroupElement.AddElement(Functions.DrawPolygon(pPolygon1 as IPolygon, 255, esriSimpleFillStyle.esriSFSNull, false));

                pPolygon1 = Functions.PolygonIntersection(pCircle as IPolygon, StraightSecPoly as IPolygon) as IPointCollection;
                pGroupElement.AddElement(Functions.DrawPolygon(pPolygon1, Functions.RGB(0, 0, 255), esriSimpleFillStyle.esriSFSNull, false));

                if (pLine != null)
                    pGroupElement.AddElement(Functions.DrawPolyline(pLine, Functions.RGB(0, 0, 255), 1, false));
                pGroupElement.AddElement(Functions.DrawPolyline(p120Line, Functions.RGB(255, 0, 0), 1, false));

                GlobalVars.StraightAreaElem = (ESRI.ArcGIS.Carto.IElement)pGroupElement;

                if (GlobalVars.ButtonControl2State)
                {
                    pGraphics.AddElement(pGroupElement.Element[0], 0);
                    pGraphics.AddElement(pGroupElement.Element[1], 0);
                    pGraphics.AddElement(pGroupElement.Element[2], 0);

                    pGroupElement.Element[0].Locked = true;
                    pGroupElement.Element[1].Locked = true;
                    pGroupElement.Element[2].Locked = true;

                    GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                }

                //Functions.RefreshCommandBar(_mTool, 2);
            }
            return result;
        }

        private Interval CalcLocalRange(double rPDG, out double NewPDG, int AdjustANgle, bool drawFlg = false)
        {
            double VTotal, fTASl, MinHd, fHinR, stPDG,
                sPDG, fTmp, coef, TurnDepDist, Rv, dl;

            DepDir = RWYDir + AdjustANgle;

            IPointCollection pLine = new Polyline();
            pLine.AddPoint(DER.pPtPrj[eRWY.PtDER]);
            pLine.AddPoint(Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir, 2.0 * GlobalVars.RModel));

            IPolygon lPoly = ((IPolygon)(new ESRI.ArcGIS.Geometry.Polygon()));
            IPolygon rPoly = ((IPolygon)(new ESRI.ArcGIS.Geometry.Polygon()));
            IPolygon prmPoly = ((IPolygon)(new ESRI.ArcGIS.Geometry.Polygon()));

            int k = ComboBox101.SelectedIndex;
            IPoint pPtGNavPrj = FrontList[k].pPtPrj;

            Functions.CreateNavaidZone(pPtGNavPrj, DepDir, FrontList[k].TypeCode, 1.3, lPoly as IPointCollection, rPoly as IPointCollection, prmPoly as IPointCollection);

            double fDist = Functions.ReturnDistanceInMeters(pPtGNavPrj, DER.pPtPrj[eRWY.PtDER]);
            // Set pPtGNavPrj = Functions.PointAlongPlane(DER.pPtPrj(ptEnd), DepDir + 180#, fDist)
            pPtGNavPrj = Functions.PointAlongPlane(pPtGNavPrj, DepDir + 180.0, fDist);

            ITopologicalOperator2 pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)prmPoly;
            pTopoOper.IsKnownSimple_2 = false;
            pTopoOper.Simplify();

            pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)pLine;
            IPointCollection pRes = (ESRI.ArcGIS.Geometry.IPointCollection)pTopoOper.Intersect(prmPoly, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension);

            int i, indx, n = pRes.PointCount;
            k = -1;
            for (i = 0; i < n; i++)
            {
                fTmp = Functions.ReturnDistanceInMeters(DER.pPtPrj[eRWY.PtDER], pRes.Point[i]);
                if (fTmp > fDist)
                {
                    fDist = fTmp;
                    k = i;
                }
            }

            ObstacleData PrevPrevDet = new ObstacleData();
            CurrDetObs = PrevPrevDet;

            if ((k < 0) || (fDist > GlobalVars.RModel))
                CurrDetObs.pPtPrj = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir, GlobalVars.RModel);
            else
                CurrDetObs.pPtPrj = pRes.Point[k];

            //CurrDetObs.pPtPrj = Functions.PointAlongPlane(DER.pPtPrj(ptEnd), DepDir, RModel)
            // DrawPoint CurrDetObs.pPtPrj, 255

            CurrDetObs.Owner = -1;                  // Resources.str39014;
            CurrDetObs.Dist = Functions.ReturnDistanceInMeters(CurrDetObs.pPtPrj, DER.pPtPrj[eRWY.PtDER]);
            CurrDetObs.PDG = PANS_OPS_DataBase.dpPDG_Nom.Value;

            double CurrDetObsHorAccuracy = 0.0;


            double CurrRange, PDG, fPrevPDG = 0, PrevRange = 0;
            int iter = 0;
            bool CanEvasion = true;
            // MinHd = (dpNGui_Ar1.Value - dpH_abv_DER.value) / dpPDG_Nom.value

            do
            {
                PrevDet = CurrDetObs;
                double PrevDetHorAccuracy = CurrDetObsHorAccuracy;
                CurrRange = PrevDet.Dist - 5.0 - PrevDetHorAccuracy;

                CreateStraightZones(PANS_OPS_DataBase.dpPDG_Nom.Value, AdjustANgle, false);

                ObstacleContainer TmpInList;
                if (CurrRange == GlobalVars.RModel)
                {
                    int nN = oZNRList.Obstacles.Length;
                    TmpInList.Obstacles = new Obstacle[nN];
                    System.Array.Copy(oZNRList.Obstacles, TmpInList.Obstacles, nN);

                    nN = oZNRList.Parts.Length;
                    TmpInList.Parts = new ObstacleData[nN];
                    System.Array.Copy(oZNRList.Parts, TmpInList.Parts, nN);

                }
                else
                    Functions.GetObstInRange(oZNRList, out TmpInList, CurrRange);

                PDG = Functions.CalcLocalPDG(TmpInList.Parts, out indx);
                if (iter == 0)
                {
                    fPrevPDG = PDG;
                    PrevRange = CurrRange;
                    PrevPrevDet = PrevDet;
                }

                if (indx > -1)
                {
                    CurrDetObs = TmpInList.Parts[indx];
                    CurrDetObsHorAccuracy = TmpInList.Obstacles[CurrDetObs.Owner].HorAccuracy;
                }
                else
                {
                    CurrDetObs.Owner = -1;// Resources.str39014;
                    CurrDetObsHorAccuracy = 0.0;
                }

                // ====================================================================================
                if (OptionButton401.Checked)
                    fHinR = PDG * CurrRange + PANS_OPS_DataBase.dpH_abv_DER.Value + DER.pPtPrj[eRWY.PtDER].Z;
                else
                    fHinR = PANS_OPS_DataBase.dpOv_Nav_PDG.Value * CurrRange + PANS_OPS_DataBase.dpH_abv_DER.Value + DER.pPtPrj[eRWY.PtDER].Z;

                fTASl = Functions.IAS2TAS(fIAS, fHinR, GlobalVars.CurrADHP.ISAtC);
                VTotal = fTASl + GlobalVars.CurrADHP.WindSpeed;

                double r0 = Functions.Bank2Radius(fBankAngle, fTASl);

                Rv = 6355.0 * System.Math.Tan(GlobalVars.DegToRadValue * fBankAngle) / (GlobalVars.PI * fTASl);
                if ((Rv > 3.0))
                    Rv = 3.0;

                coef = GlobalVars.CurrADHP.WindSpeed / (Rv * 3.6);
                dl = PANS_OPS_DataBase.dpT_TechToleranc.Value * VTotal * 0.277777777777778;

                IPointCollection poly = Functions.ReturnPolygonPartAsPolyline(UnitedPolygon, pPtGNavPrj, DepDir, TurnDir);
                TurnDepDist = Functions.CalcSpiralStartPoint(poly, ref PrevDet, coef, r0, DepDir, RWYDir, TurnDir, CheckBox401.Checked && OptionButton401.Checked);

                if (TurnDepDist > 0.0)
                {
                    TurnDepDist = TurnDepDist - dl;
                    fTmp = System.Math.Round(TurnDepDist * PDG + PANS_OPS_DataBase.dpH_abv_DER.Value - 0.4999);
                    TurnDepDist = (fTmp - PANS_OPS_DataBase.dpH_abv_DER.Value) / PDG;
                }
                else
                    break;
                // TurnDepDist = CurrRange + MinHd

                if (OptionButton401.Checked)
                    Functions.CalcTIAMinTNAH(TmpInList.Parts, PDG, out MinHd, 0);
                else
                    MinHd = (PANS_OPS_DataBase.dpNGui_Ar1.Value - PANS_OPS_DataBase.dpH_abv_DER.Value) / PDG;

                if (MinHd > TurnDepDist)
                    break;

                if (PDG + GlobalVars.mEps < rPDG)
                    break;

                PrevRange = CurrRange;
                PrevPrevDet = PrevDet;
                CanEvasion = true;

                if ((CurrDetObs.Owner == -1) || (System.Math.Abs(PDG - rPDG) < GlobalVars.mEps))
                    break;

                fPrevPDG = PDG;
                iter = iter + 1;
            }
            while (true);

            // =============================================================================
            Interval result = new Interval();

            result.Left = 2.0 * GlobalVars.RModel;
            result.Right = result.Left;
            NewPDG = fPrevPDG;

            if (CanEvasion)
            {
                result.Left = PrevRange;
                result.Right = PrevRange;

                if (PDG > rPDG)
                    stPDG = PDG;
                else
                    stPDG = rPDG;

                int m = System.Convert.ToInt32(System.Math.Round((fPrevPDG - stPDG) * 1000.0));
                if (m < 0)
                    m = 0;

                for (i = 0; i <= m; i++)
                {
                    sPDG = stPDG + i * 0.001;
                    if (i == m)
                    {
                        CurrRange = PrevRange;
                        PrevDet = PrevPrevDet;
                    }

                    CreateStraightZones(sPDG, AdjustANgle, false);

                    ObstacleContainer TmpInList;
                    if (CurrRange == GlobalVars.RModel)
                    {
                        int nN = oZNRList.Obstacles.Length;
                        TmpInList.Obstacles = new Obstacle[nN];
                        System.Array.Copy(oZNRList.Obstacles, TmpInList.Obstacles, nN);

                        nN = oZNRList.Parts.Length;
                        TmpInList.Parts = new ObstacleData[nN];
                        System.Array.Copy(oZNRList.Parts, TmpInList.Parts, nN);
                    }
                    else
                        Functions.GetObstInRange(oZNRList, out TmpInList, CurrRange);

                    PDG = Functions.CalcLocalPDG(TmpInList.Parts, out indx);
                    //if(iter == 0) PrevPDG = PDG
                    if (indx > -1)
                        CurrDetObs = TmpInList.Parts[indx];
                    else
                        CurrDetObs.Owner = -1;// Resources.str39014;

                    // ====================================================================================

                    if (OptionButton401.Checked)
                        fHinR = sPDG * CurrRange + PANS_OPS_DataBase.dpH_abv_DER.Value + DER.pPtPrj[eRWY.PtDER].Z;
                    else
                        fHinR = PANS_OPS_DataBase.dpOv_Nav_PDG.Value * CurrRange + PANS_OPS_DataBase.dpH_abv_DER.Value + DER.pPtPrj[eRWY.PtDER].Z;

                    fTASl = Functions.IAS2TAS(fIAS, fHinR, GlobalVars.CurrADHP.ISAtC);
                    VTotal = fTASl + GlobalVars.CurrADHP.WindSpeed;

                    double r0 = Functions.Bank2Radius(fBankAngle, fTASl);

                    Rv = 6355.0 * System.Math.Tan(GlobalVars.DegToRadValue * fBankAngle) / (GlobalVars.PI * fTASl);
                    if (Rv > 3.0)
                        Rv = 3.0;

                    coef = GlobalVars.CurrADHP.WindSpeed / (Rv * 3.6);
                    dl = PANS_OPS_DataBase.dpT_TechToleranc.Value * VTotal * 0.277777777777778;
                    // ====================================================================================
                    IPointCollection poly = Functions.ReturnPolygonPartAsPolyline(UnitedPolygon, pPtGNavPrj, DepDir, TurnDir);
                    TurnDepDist = Functions.CalcSpiralStartPoint(poly, ref PrevDet, coef, r0, DepDir, RWYDir, TurnDir, CheckBox401.Checked && OptionButton401.Checked);

                    if (OptionButton401.Checked)
                        Functions.CalcTIAMinTNAH(TmpInList.Parts, sPDG, out MinHd, 0);
                    else
                        MinHd = (PANS_OPS_DataBase.dpNGui_Ar1.Value - PANS_OPS_DataBase.dpH_abv_DER.Value) / sPDG;

                    if (TurnDepDist > 0.0)
                    {
                        TurnDepDist = TurnDepDist - dl;
                        fTmp = System.Math.Round(TurnDepDist * sPDG + PANS_OPS_DataBase.dpH_abv_DER.Value - 0.4999);
                        TurnDepDist = (fTmp - PANS_OPS_DataBase.dpH_abv_DER.Value) / sPDG;

                        if (MinHd <= TurnDepDist)
                        {
                            NewPDG = System.Math.Round(sPDG + 0.0004999, 3); // NewPDG = 0.001 * System.Math.Round(sPDG * 1000 + 0.4)
                            result.Left = MinHd;
                            result.Right = TurnDepDist;
                            break;
                        }
                    }
                }
                // ====================================================================================
            }

            if (drawFlg)
                CreateStraightZones(NewPDG, AdjustANgle, true);

            return result;
        }

        private Interval CalcGlobalRange(double rPDG, out double NewPDG, out int AdjustANgle, bool drawFlg = false)
        {
            ObstacleData ppTmp = new ObstacleData();
            ObstacleData cTmp = ppTmp;

            Interval MinRange = new Interval();
            MinRange.Left = (PANS_OPS_DataBase.dpNGui_Ar1.Value - PANS_OPS_DataBase.dpH_abv_DER.Value) / rPDG; // = 2*R
            MinRange.Right = GlobalVars.RModel;         // MinRange.Left
            rPDG = rPDG - 0.9 * GlobalVars.mEps;

            int MinAngle = 0, i;
            double PDG, fMinPDG = 9999.0;

            for (i = LeftMinAngle; i <= LeftMaxAngle; i++)
            {
                Interval CurrRange = CalcLocalRange(rPDG, out PDG, i, false);
                if ((PDG < fMinPDG) && (CurrRange.Right < GlobalVars.RModel))
                {
                    fMinPDG = PDG;
                    MinRange = CurrRange;
                    MinAngle = i;
                    ppTmp = PrevDet;
                    cTmp = CurrDetObs;
                    if (System.Math.Abs(fMinPDG - rPDG) <= GlobalVars.mEps)
                        break;
                }
            }

            for (i = RightMinAngle; i <= RightMaxAngle; i++)
            {
                Interval CurrRange = CalcLocalRange(rPDG, out PDG, -i, false);
                if ((PDG < fMinPDG) && (CurrRange.Right < GlobalVars.RModel) || ((System.Math.Abs(fMinPDG - PDG) < GlobalVars.mEps) && (i < MinAngle)))
                {
                    fMinPDG = PDG;
                    MinRange = CurrRange;
                    MinAngle = -i;
                    ppTmp = PrevDet;
                    cTmp = CurrDetObs;
                    if (System.Math.Abs(fMinPDG - rPDG) <= GlobalVars.mEps)
                        break;
                }
            }

            NewPDG = fMinPDG;

            AdjustANgle = MinAngle;
            PrevDet = ppTmp;
            CurrDetObs = cTmp;
            CreateStraightZones(NewPDG, AdjustANgle, drawFlg);
            //Functions.CalcObstaclesReqTNAH(oZNRList, NewPDG);		//???

            TrackAdjust = AdjustANgle;
            AdjustDir = System.Math.Sign(TrackAdjust);

            ComboBox501.SelectedIndex = (1 - AdjustDir) >> 1;

            SpinButton501.Value = System.Math.Abs(TrackAdjust);

            ToolTip1.SetToolTip(SpinButton501, Resources.str15273 + TrackAdjust.ToString() + "? " + ComboBox501.Text); // "??????????? ????????: "
            ToolTip1.SetToolTip(ComboBox501, ToolTip1.GetToolTip(SpinButton501));
            return MinRange;
        }

        private double CalcRNELocalRange(out double NewPDG, int AdjustANgle, out double CircleR, bool drawFlg = false, double CheckPDG = -1.0)
        {
            if (CheckPDG < 0.0)
                CheckPDG = PANS_OPS_DataBase.dpPDG_Nom.Value;

            //if (CheckPDG < PANS_OPS_DataBase.dpPDG_Nom.Value)
            //    CheckPDG = PANS_OPS_DataBase.dpPDG_Nom.Value;

            int k = ComboBox101.SelectedIndex;

            NewPDG = MinCurrPDG; // dpPDG_Nom.Value    '

            double Range = Functions.Point2LineDistancePrj(FrontList[k].pPtPrj, DER.pPtPrj[eRWY.PtDER], RWYDir + AdjustANgle + 90.0);
            double hCone = PANS_OPS_DataBase.dpH_abv_DER.Value + PANS_OPS_DataBase.dpOv_Nav_PDG.Value * Range + DER.pPtPrj[eRWY.PtDER].Z - FrontList[k].pPtPrj.Z;

            if (FrontList[k].TypeCode == eNavaidType.VOR)
                CircleR = hCone * System.Math.Tan(GlobalVars.DegToRadValue * Navaids_DataBase.VOR.ConeAngle);
            else
                CircleR = hCone * System.Math.Tan(GlobalVars.DegToRadValue * Navaids_DataBase.NDB.ConeAngle);

            double CurrRange = Range - CircleR;

            CreateStraightZones(NewPDG, AdjustANgle, false);

            ObstacleContainer TmpInList;

            if (CurrRange == GlobalVars.RModel)
            {
                int nN = oZNRList.Obstacles.Length;
                TmpInList.Obstacles = new Obstacle[nN];
                System.Array.Copy(oZNRList.Obstacles, TmpInList.Obstacles, nN);

                nN = oZNRList.Parts.Length;
                TmpInList.Parts = new ObstacleData[nN];
                System.Array.Copy(oZNRList.Parts, TmpInList.Parts, nN);
            }
            else
                Functions.GetObstInRange(oZNRList, out TmpInList, CurrRange);

            int indx;
            NewPDG = Functions.CalcLocalPDG(TmpInList.Parts, out indx);
            if (NewPDG < CheckPDG)
                NewPDG = CheckPDG;

            //CreateStraightZones(NewPDG, AdjustANgle, drawFlg);

            return CurrRange;
        }

        private double CalcRNEGlobalRange(out double NewPDG, int AdjustANgle, bool drawFlg = false, double CheckPDG = -1.0)
        {
            double PDG, CurrRange, CircleR;
            double MinPDG = 9999.0, MinRange = 2.0 * GlobalVars.RModel;
            int i, MinAngle = 0;

            for (i = LeftMinAngle; i <= LeftMaxAngle; i++)
            {
                CurrRange = CalcRNELocalRange(out PDG, i, out CircleR, false, CheckPDG);
                if (PDG < MinPDG)
                {
                    MinPDG = PDG;
                    MinRange = CurrRange;
                    MinAngle = i;
                }
            }

            for (i = RightMinAngle; i <= RightMaxAngle; i++)
            {
                CurrRange = CalcRNELocalRange(out PDG, -i, out CircleR, false, CheckPDG);
                if (PDG < MinPDG)
                {
                    MinPDG = PDG;
                    MinRange = CurrRange;
                    MinAngle = -i;
                }
            }

            // CalcRNEGlobalRange = CalcRNELocalRange(PDG, MinAngle,CircleR)

            NewPDG = MinPDG;
            AdjustANgle = MinAngle;

            int k = ComboBox101.SelectedIndex;

            //CurrRange = Point2LineDistancePrj(FrontList(K).pPtPrj, DER.pPtPrj(ptEnd), RWYDir + AdjustANgle + 90#)
            //hTurn = dpH_abv_DER.Value + dpOv_Nav_PDG.Value * CurrRange + DER.pPtPrj(ptEnd).Z - FrontList(K).pPtGeo.Z

            CreateStraightZones(NewPDG, AdjustANgle, drawFlg);
            //Functions.CalcObstaclesReqTNAH(oZNRList, NewPDG);		//???

            //    PrevPrevDet = ppTmp
            //    CurrDetObs = cTmp
            TrackAdjust = AdjustANgle;
            AdjustDir = System.Math.Sign(TrackAdjust);

            //ComboBox501.SelectedIndex = System.Convert.ToInt32(0.5 * (1 - AdjustDir));
            ComboBox501.SelectedIndex = ((1 - AdjustDir) >> 1);

            SpinButton501.Value = System.Math.Abs(TrackAdjust);

            ToolTip1.SetToolTip(SpinButton501, Resources.str15273 + TrackAdjust.ToString() + "? " + ComboBox501.Text); // "??????????? ????????: "
            ToolTip1.SetToolTip(ComboBox501, ToolTip1.GetToolTip(SpinButton501));
            return MinRange;
        }

        private void ReportBtn_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            if (!Report)
                return;

            if (ReportBtn.Checked)
                ReportsFrm.Show(GlobalVars.Win32Window);
            else
                ReportsFrm.Hide();
        }

        private void CheckBox301_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            OkBtn.Enabled = !CheckBox301.Checked;
            NextBtn.Enabled = CheckBox301.Checked;
            Label311.Visible = !CheckBox301.Checked;
        }

        private void CheckBox905_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            SecondArea(TurnDir, BaseArea);
        }

        private void CheckBox906_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            SecondArea(TurnDir, BaseArea);
        }

        private void CheckBox701_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            ComboBox701_SelectedIndexChanged(ComboBox701, new System.EventArgs());
        }

        private void CheckBox901_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            CheckBox904.Checked = CheckBox901.Checked;
            SecondArea(TurnDir, BaseArea);
        }

        private void CheckBox902_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            SecondArea(TurnDir, BaseArea);
        }

        private void CheckBox903_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            SecondArea(TurnDir, BaseArea);
        }

        private void CheckBox904_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            CheckBox901.Checked = CheckBox904.Checked;
        }

        private void FillFrontList()
        {
            ComboBox101.Items.Clear();

            int n = GlobalVars.NavaidList.Length;
            FrontList = new NavaidType[n];

            if (n == 0)
                return;

            IPoint Pt300R = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], RWYDir - 90.0, PANS_OPS_DataBase.dpGui_Ar1_Wd.Value);
            IPoint Pt300L = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], RWYDir + 90.0, PANS_OPS_DataBase.dpGui_Ar1_Wd.Value);

            //Functions.DrawPointWithText(Pt300R, "300-R");
            //Functions.DrawPointWithText(Pt300L, "300-L");
            //Functions.DrawPointWithText(DER.pPtPrj[eRWY.PtDER], "DER");
            //Application.DoEvents();

            double Dist = PANS_OPS_DataBase.dpGui_Ar1_Wd.Value / System.Math.Tan(GlobalVars.DegToRadValue * PANS_OPS_DataBase.dpTr_AdjAngle.Value);

            IPoint ptAheadDer = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], RWYDir, Dist);
            IPoint ptBehindDer = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], RWYDir + 180.0, Dist);

            //Functions.DrawPointWithText(ptAheadDer, "Ahead Der");
            //Functions.DrawPointWithText(ptBehindDer, "Behind Der");
            //Application.DoEvents();

            IPoint ptTmp = new ESRI.ArcGIS.Geometry.Point();
            IConstructPoint ptConstr = (ESRI.ArcGIS.Geometry.IConstructPoint)ptTmp;

            int j = -1;
            for (int i = 0; i < n; i++)
            {
                IPoint ptNavPrj = GlobalVars.NavaidList[i].pPtPrj;
                Dist = Functions.ReturnDistanceInMeters(ptNavPrj, DER.pPtPrj[eRWY.PtDER]);
                if (Dist > 31000.0)
                    continue;

                double aztTmp = Functions.ReturnAngleInDegrees(ptBehindDer, ptNavPrj);

                double dAzt = NativeMethods.Modulus(RWYDir - aztTmp, 360.0);
                if (dAzt > 180.0)
                    dAzt = 360.0 - dAzt;

                if ((dAzt <= PANS_OPS_DataBase.dpTr_AdjAngle.Value - 1.0) && (Functions.SideDef(DER.pPtPrj[eRWY.PtDER], RWYDir - 90.0, ptNavPrj) < 0))
                {
                    j++;
                    FrontList[j] = GlobalVars.NavaidList[i];

                    // ===============
                    ptConstr.ConstructAngleIntersection(DER.pPtPrj[eRWY.PtDER], GlobalVars.DegToRadValue * RWYDir, ptNavPrj, GlobalVars.DegToRadValue * (RWYDir - 90.0));
                    FrontList[j].Dist = Functions.ReturnDistanceInMeters(ptTmp, DER.pPtPrj[eRWY.PtDER]);
                    FrontList[j].CLShift = Functions.ReturnDistanceInMeters(ptTmp, ptNavPrj);
                    FrontList[j].CLShift = FrontList[j].CLShift * Functions.SideDef(DER.pPtPrj[eRWY.PtDER], RWYDir, ptNavPrj);

                    FrontList[j].index = i;
                    FrontList[j].Front = true;

                    FrontList[j].ValMin = new double[1];
                    FrontList[j].ValMax = new double[1];

                    FrontList[j].ValMin[0] = Functions.Dir2Azt(Pt300R, Functions.ReturnAngleInDegrees(Pt300R, ptNavPrj));
                    FrontList[j].ValMax[0] = Functions.Dir2Azt(Pt300L, Functions.ReturnAngleInDegrees(Pt300L, ptNavPrj));

                    if (Functions.SubtractAngles(FrontList[j].ValMin[0], DER.pPtGeo[eRWY.PtDER].M) > PANS_OPS_DataBase.dpTr_AdjAngle.Value)
                        FrontList[j].ValMin[0] = NativeMethods.Modulus(DER.pPtGeo[eRWY.PtDER].M - PANS_OPS_DataBase.dpTr_AdjAngle.Value, 360.0);

                    if (Functions.SubtractAngles(FrontList[j].ValMax[0], DER.pPtGeo[eRWY.PtDER].M) > PANS_OPS_DataBase.dpTr_AdjAngle.Value)
                        FrontList[j].ValMax[0] = NativeMethods.Modulus(DER.pPtGeo[eRWY.PtDER].M + PANS_OPS_DataBase.dpTr_AdjAngle.Value, 360.0);
                }

                aztTmp = Functions.ReturnAngleInDegrees(ptNavPrj, ptAheadDer);
                dAzt = NativeMethods.Modulus(RWYDir - aztTmp, 360.0);
                if (dAzt > 180.0)
                    dAzt = 360.0 - dAzt;

                if ((dAzt <= PANS_OPS_DataBase.dpTr_AdjAngle.Value - 1.0) && (Functions.SideDef(DER.pPtPrj[eRWY.PtDER], RWYDir - 90.0, ptNavPrj) > 0))
                {
                    j++;
                    FrontList[j] = GlobalVars.NavaidList[i];

                    // ===============
                    ptConstr.ConstructAngleIntersection(DER.pPtPrj[eRWY.PtDER], GlobalVars.DegToRadValue * RWYDir, ptNavPrj, GlobalVars.DegToRadValue * (RWYDir - 90.0));

                    FrontList[j].Dist = Functions.ReturnDistanceInMeters(ptTmp, DER.pPtPrj[eRWY.PtDER]);
                    FrontList[j].CLShift = Functions.ReturnDistanceInMeters(ptTmp, ptNavPrj);
                    FrontList[j].CLShift = FrontList[j].CLShift * Functions.SideDef(DER.pPtPrj[eRWY.PtDER], RWYDir, ptNavPrj);

                    FrontList[j].index = i;
                    FrontList[j].Front = false;

                    FrontList[j].ValMin = new double[1];
                    FrontList[j].ValMax = new double[1];

                    FrontList[j].ValMin[0] = Functions.Dir2Azt(ptNavPrj, Functions.ReturnAngleInDegrees(ptNavPrj, Pt300L));
                    FrontList[j].ValMax[0] = Functions.Dir2Azt(ptNavPrj, Functions.ReturnAngleInDegrees(ptNavPrj, Pt300R));

                    if (Functions.SubtractAngles(FrontList[j].ValMin[0], DER.pPtGeo[eRWY.PtDER].M) > PANS_OPS_DataBase.dpTr_AdjAngle.Value)
                        FrontList[j].ValMin[0] = NativeMethods.Modulus(DER.pPtGeo[eRWY.PtDER].M - PANS_OPS_DataBase.dpTr_AdjAngle.Value, 360.0);

                    if (Functions.SubtractAngles(FrontList[j].ValMax[0], DER.pPtGeo[eRWY.PtDER].M) > PANS_OPS_DataBase.dpTr_AdjAngle.Value)
                        FrontList[j].ValMax[0] = NativeMethods.Modulus(DER.pPtGeo[eRWY.PtDER].M + PANS_OPS_DataBase.dpTr_AdjAngle.Value, 360.0);
                }

                FrontList[j].IntersectionType = eIntersectionType.OnNavaid;
            }

            ComboBox101.Enabled = j >= 0;
            System.Array.Resize<NavaidType>(ref FrontList, j + 1);

            for (int i = 0; i <= j; i++)
                ComboBox101.Items.Add(FrontList[i].CallSign);

            if (j >= 0)
                ComboBox101.SelectedIndex = 0;
        }

        private void ComboBox001_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            int RWYIndex = ComboBox001.SelectedIndex;
            if (RWYIndex < 0)
                return;

            DER = RWYList[RWYIndex];

            TextBox007.Text = Functions.ConvertHeight(DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.NEAREST).ToString();

            RWYDir = DER.pPtPrj[eRWY.PtDER].M;

            TextBox002.Text = Functions.ConvertDistance(DER.Length, eRoundMode.NEAREST).ToString();

            double d0 = DER.Length + DER.ClearWay - PANS_OPS_DataBase.dpT_Init.Value;

            ptCenter = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], RWYDir + 180.0, d0);

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
            double fTmp = NativeMethods.Modulus(DER.pPtGeo[eRWY.PtDER].M - GlobalVars.CurrADHP.MagVar);

            if (ComboBox002.SelectedIndex >= 0)
            {
                TextBox001.Text = Functions.Degree2String(DER.pPtGeo[eRWY.PtDER].M, (Degree2StringMode)ComboBox002.SelectedIndex);
                TextBox004.Text = Functions.Degree2String(fTmp, (Degree2StringMode)ComboBox002.SelectedIndex);
            }
            else
            {
                TextBox001.Text = Functions.Degree2String((DER.pPtGeo[eRWY.PtDER].M), Degree2StringMode.DD);
                TextBox004.Text = Functions.Degree2String(fTmp, Degree2StringMode.DD);
            }
        }

        private void ComboBox003_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            RMin = 50.0 * System.Math.Round(0.02 * 600.0 / PANS_OPS_DataBase.dpPDG_Nom.Value + 0.4999);

            // If (ComboBox003.ListIndex = 0) Then
            //     MOCLimit = 100000.0
            // Else
            MOCLimit = Functions.DeConvertHeight(double.Parse(ComboBox003.Text));
            double minR = 50.0 * System.Math.Round(0.02 * MOCLimit / PANS_OPS_DataBase.dpMOC.Value + 0.4999);
            if (RMin < minR)
                RMin = minR;

            // End If

            double OldR;

            if (!double.TryParse(TextBox003.Text, out OldR))
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
            //int i = ComboBox004.SelectedIndex;
            //if (i < 0)
            //    return;

            //CurrADHP = GlobalVars.ADHPList[i];

            //DBModule.FillADHPFields(ref CurrADHP);
            //if (CurrADHP.pPtGeo == null)
            //{
            //		MessageBox.Show("Error reading ADHP.", "PANDA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            //ComboBox001.Items.Clear();
            //ComboBox101.Items.Clear();

            //DBModule.FillRWYList(out RWYList, GlobalVars.CurrADHP);

            //int i, n = RWYList.Length;

            //if (n <= 0)
            //{
            //		MessageBox.Show(Resources.str15056, "PANDA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            //for (i = 0; i < n; i++)
            //    ComboBox001.Items.Add(RWYList[i].Name);

            //DBModule.FillWPT_FIXList(out GlobalVars.WPTList, GlobalVars.CurrADHP, GlobalVars.MaxNAVDist);
            //DBModule.FillNavaidList(out GlobalVars.NavaidList, out GlobalVars.DMEList, GlobalVars.CurrADHP, GlobalVars.MaxNAVDist);

            //RMin = 50.0 * System.Math.Round(0.02 * 600.0 / PANS_OPS_DataBase.dpPDG_Nom.Value + 0.4999);
            //if (RMin < 20000.0)
            //    RMin = 20000.0;

            //TextBox007.Text = Functions.ConvertHeight(600.0 + GlobalVars.CurrADHP.Elev, eRoundMode.rmNERAEST).ToString();
            //TextBox003.Text = Functions.ConvertDistance(RMin, eRoundMode.rmCEIL).ToString();
            //// ====================================================================
            //n = GlobalVars.WPTList.Length;
            //if (n > 0)
            //{
            //    FixAngl = new WPT_FIXType[n];

            //    int  j = -1;
            //    for (i = 0; i < n; i++)
            //    {
            //        if (GlobalVars.WPTList[i].TypeCode == eNavaidType.CodeVOR || GlobalVars.WPTList[i].TypeCode == eNavaidType.CodeNDB)
            //        {
            //            j ++;
            //            FixAngl[j] = GlobalVars.WPTList[i];
            //        }
            //    }

            //    if (j >= 0)
            //        System.Array.Resize<WPT_FIXType>(ref FixAngl, j + 1);
            //    else
            //    {
            //        FixAngl = new WPT_FIXType[0];
            //        OptionButton702.Enabled = false;
            //    }
            //}
            //else
            //{
            //    FixAngl = new WPT_FIXType[0];
            //    OptionButton703.Enabled = false;
            //    OptionButton702.Enabled = false;
            //    ComboBox701.Enabled = false;
            //}
            //// ====================================================================

            //ComboBox001.SelectedIndex = 0;
            //TextBox003_Validating(TextBox003, new System.ComponentModel.CancelEventArgs());
        }

        private void ComboBox101_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            int k, l, m;

            double d;
            double VMin;
            double VMax;

            double AztMin;
            double AztMax;
            bool bFlag;
            IProximityOperator pProxi;
            string RadPel = "";

            if (MultiPage1.SelectedIndex != 1)
                return;

            k = ComboBox101.SelectedIndex;

            Label103.Text = Navaids_DataBase.GetNavTypeName(FrontList[k].TypeCode);

            VMin = NativeMethods.Modulus(System.Math.Round(FrontList[k].ValMin[0] - FrontList[k].MagVar + 0.4999), 360.0);
            VMax = NativeMethods.Modulus(System.Math.Round(FrontList[k].ValMax[0] - FrontList[k].MagVar - 0.4999), 360.0);

            Label105.Text = Resources.str15098 + Functions.DegToStr(VMin) + Resources.str15103 + Functions.DegToStr(VMax);

            bFlag = FrontList[k].Front;

            if (FrontList[k].TypeCode == eNavaidType.VOR)
            {
                Label108.Text = Resources.str02104 + Resources.str02105;
                if (FrontList[k].Front)
                {
                    VMin = NativeMethods.Modulus(System.Math.Round(FrontList[k].ValMin[0] - FrontList[k].MagVar + 180.0 + 0.4999), 360.0);
                    VMax = NativeMethods.Modulus(System.Math.Round(FrontList[k].ValMax[0] - FrontList[k].MagVar + 180.0 - 0.4999), 360.0);
                }
                else
                {
                    VMin = NativeMethods.Modulus(System.Math.Round(FrontList[k].ValMin[0] - FrontList[k].MagVar + 0.4999), 360.0);
                    VMax = NativeMethods.Modulus(System.Math.Round(FrontList[k].ValMax[0] - FrontList[k].MagVar - 0.4999), 360.0);
                }
                RadPel = Resources.str02201;
            }
            else if (FrontList[k].TypeCode == eNavaidType.NDB)
            {
                if (FrontList[k].Front)
                    Label108.Text = Resources.str02104 + Resources.str02106;
                else
                    Label108.Text = Resources.str02104 + Resources.str02107;

                VMin = NativeMethods.Modulus(System.Math.Round(FrontList[k].ValMin[0] - FrontList[k].MagVar + 0.4999), 360.0);
                VMax = NativeMethods.Modulus(System.Math.Round(FrontList[k].ValMax[0] - FrontList[k].MagVar - 0.4999), 360.0);
                RadPel = Resources.str02202;
            }

            Label109.Text = Resources.str15098 + Functions.DegToStr(VMin) + Resources.str15103 + Functions.DegToStr(VMax);

            if (FrontList[k].Front)
            {
                Label107.Text = Functions.ConvertDistance(FrontList[k].Dist, eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit + Resources.str15279;
                pProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)pCircle;

                d = pProxi.ReturnDistance(FrontList[k].pPtPrj);
                bFlag = d <= GlobalVars.distEps;
                if (FrontList[k].TypeCode == eNavaidType.NDB)
                    RadPel = RadPel + Resources.str15255;   // Verify
            }
            else
            {
                Label107.Text = Functions.ConvertDistance(FrontList[k].Dist, eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit + Resources.str15282; // " ?????? ????? ?? DER"
                if (FrontList[k].TypeCode == eNavaidType.NDB)
                    RadPel = RadPel + Resources.str15098;
            }
            RadPel = RadPel + ComboBox101.Text;

            // Label301.Width = 1330
            Label201.Text = Resources.str02207 + RadPel; // Label301.Caption

            TextBox201.Text = VMin.ToString();

            OptionButton403.Enabled = bFlag;

            if ((!(bFlag)) && OptionButton403.Checked)
            {
                OptionButton401.Checked = true;
                OptionButton403.Checked = false;
            }

            if (FrontList[k].CLShift > 0.0)
                Label106.Text = Functions.ConvertDistance(FrontList[k].CLShift, eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit + Resources.str15283; // " ?????? ?????? ?? CL"
            else
                Label106.Text = Functions.ConvertDistance(-FrontList[k].CLShift, eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit + Resources.str15284; // " ?????? ????? ?? CL"

            AztMin = NativeMethods.Modulus(System.Math.Round(FrontList[k].ValMin[0] + 0.4999));
            AztMax = NativeMethods.Modulus(System.Math.Round(FrontList[k].ValMax[0] - 0.4999));

            k = System.Convert.ToInt32(Functions.SubtractAngles(AztMax, AztMin));
            l = System.Convert.ToInt32(Functions.SubtractAngles(RWYDir, Functions.Azt2Dir(DER.pPtGeo[eRWY.PtDER], AztMin)));
            m = System.Convert.ToInt32(Functions.SubtractAngles(RWYDir, Functions.Azt2Dir(DER.pPtGeo[eRWY.PtDER], AztMax)));

            bool bRight = false;


            // MsgBox "ComboBox501.ListCount = " && ComboBox501.ListCount && vbCrLf && "ComboBox501.ListIndex = " && ComboBox501.ListIndex

            if ((k == 0) || (k < l + m))
            {
                ComboBox501.Enabled = false;
                bRight = (l < m) || (NativeMethods.Modulus(Functions.Azt2Dir(DER.pPtGeo[eRWY.PtDER], AztMax) - RWYDir, 360.0) >= 180.0);

                if (bRight)
                {
                    ComboBox501.SelectedIndex = 1;
                    RightMinAngle = l;
                    RightMaxAngle = m;

                    LeftMinAngle = 0;
                    LeftMaxAngle = -1;

                    SpinButton501.Minimum = l;
                    SpinButton501.Maximum = m;
                }
                else
                {
                    ComboBox501.SelectedIndex = 0;
                    RightMinAngle = 0;
                    RightMaxAngle = -1;

                    LeftMinAngle = m;
                    LeftMaxAngle = l;

                    SpinButton501.Minimum = m;
                    SpinButton501.Maximum = l;
                }
            }
            else
            {
                ComboBox501.Enabled = true;
                RightMinAngle = 0;
                RightMaxAngle = m;

                LeftMinAngle = 0;
                LeftMaxAngle = l;
                ComboBox501.SelectedIndex = 0;
                SpinButton501.Minimum = 0;
                SpinButton501.Maximum = l;
            }

        }

        private void ComboBox301_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            AirCat = ComboBox301.SelectedIndex;

            Label409.Text = Functions.ConvertSpeed(1.1 * Categories_DATABase.cVmaInter.Values[AirCat], eRoundMode.NEAREST).ToString();
            Label410.Text = Functions.ConvertSpeed(1.1 * Categories_DATABase.cVmaFaf.Values[AirCat], eRoundMode.NEAREST).ToString();
            TextBox401.Text = Functions.ConvertSpeed(1.1 * Categories_DATABase.cVmaFaf.Values[AirCat], eRoundMode.NEAREST).ToString();
            TextBox401_Validating(TextBox401, new System.ComponentModel.CancelEventArgs());
        }

        private void ComboBox401_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            TurnDir = 1 - 2 * ComboBox401.SelectedIndex;
        }

        private void ComboBox501_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (ComboBox501.SelectedIndex == 0)
            {
                SpinButton501.Minimum = LeftMinAngle;
                SpinButton501.Maximum = LeftMaxAngle;

                if (SpinButton501.Value < LeftMinAngle)
                    SpinButton501.Value = RightMinAngle;
                else if (SpinButton501.Value > LeftMaxAngle)
                    SpinButton501.Value = LeftMaxAngle;
            }
            else
            {
                SpinButton501.Minimum = RightMinAngle;
                SpinButton501.Maximum = RightMaxAngle;

                if (SpinButton501.Value < RightMinAngle)
                    SpinButton501.Value = RightMinAngle;
                else if (SpinButton501.Value > RightMaxAngle)
                    SpinButton501.Value = RightMaxAngle;
            }
            SpinButton501_Change(SpinButton501, new System.EventArgs());
        }

        private void ComboBox701_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (ComboBox701.Items.Count < 1 || MultiPage1.SelectedIndex < 5)
                return;

            // CurrPDG = CDbl(TextBox505.Text) * 0.01
            // hTurn = CDbl(TextBox602.Text)

            if (OptionButton703.Checked)
                TurnDirector = FixWPT[ComboBox701.SelectedIndex];
            else if (OptionButton702.Checked)
                TurnDirector = FixAngl[ComboBox701.SelectedIndex];
            else
                return;

            Label702.Text = Navaids_DataBase.GetNavTypeName(TurnDirector.TypeCode);

            if (OptionButton703.Checked && (TurnDirector.TypeCode == eNavaidType.VOR || TurnDirector.TypeCode == eNavaidType.NDB))
                CheckBox701.Enabled = true;
            else
            {
                CheckBox701.Checked = false;
                CheckBox701.Enabled = false;
            }

            if ((TurnDirector.TypeCode == eNavaidType.VOR))
            {
                ComboBox703.Items[0] = Resources.str36302; // "?? ???????? ??????"
                                                           // VB6.SetItemString(ComboBox703, 0, My.Resources.str36302) '"?? ???????? ??????"
            }
            else
            {
                ComboBox703.Items[0] = Resources.str36303; // "?? ???????? ??????"
                                                           // VB6.SetItemString(ComboBox703, 0, My.Resources.str36303) '"?? ???????? ??????"
            }

            if (OptionButton702.Checked)
            {
                UpdateIntervals((true));
                if (ComboBox703.SelectedIndex == 0)
                    ComboBox703_SelectedIndexChanged(ComboBox703, new System.EventArgs());
                else
                    ComboBox703.SelectedIndex = 0;
            }
            else if (OptionButton703.Checked)
                UpdateToFix();
        }

        private void ComboBox601_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            int k = ComboBox601.SelectedIndex;
            string tipStr;

            if (OptionButton401.Checked || k < 0)
                return;

            Label602.Text = Navaids_DataBase.GetNavTypeName(TurnInterDat[k].TypeCode);

            if (TurnInterDat[k].TypeCode == eNavaidType.DME)
            {
                int n = TurnInterDat[k].ValMin.Length - 1;

                Option601.Enabled = n > 0;
                Option602.Enabled = n > 0;
                Label626.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;

                if (Option601.Checked || (n == 0))
                    TextBox601.Text = Functions.ConvertDistance(TurnInterDat[k].ValMin[0], eRoundMode.NEAREST).ToString();
                else
                    TextBox601.Text = Functions.ConvertDistance(TurnInterDat[k].ValMin[1], eRoundMode.NEAREST).ToString();

                if (n == 0)
                {
                    if (TurnInterDat[k].ValCnt > 0)
                        Option601.Checked = true;
                    else
                        Option602.Checked = true;
                }

                Label603.Text = Resources.str15096;
                tipStr = Resources.str15097 + "\r\n";

                for (int i = 0; i <= n; i++)
                {
                    tipStr = tipStr + Resources.str15098 + Functions.ConvertDistance(TurnInterDat[k].ValMin[i], eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit +
                                        Resources.str15099 + Functions.ConvertDistance(TurnInterDat[k].ValMax[i], eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
                    if (i < n)
                        tipStr = tipStr + "\r\n";
                }
            }
            else
            {
                double Kmin, Kmax;

                Option601.Enabled = false;
                Option602.Enabled = false;
                Label626.Text = "°";

                if (TurnInterDat[k].TypeCode == eNavaidType.VOR)
                {
                    Label603.Text = Resources.str15101;
                    Kmax = NativeMethods.Modulus(TurnInterDat[k].ValMax[0] - GlobalVars.CurrADHP.MagVar);
                    Kmin = NativeMethods.Modulus(TurnInterDat[k].ValMin[0] - GlobalVars.CurrADHP.MagVar);
                    tipStr = Resources.str15102;
                }
                else
                {
                    Label603.Text = Resources.str15104;
                    Kmax = NativeMethods.Modulus(TurnInterDat[k].ValMax[0] + 180.0 - GlobalVars.CurrADHP.MagVar);
                    Kmin = NativeMethods.Modulus(TurnInterDat[k].ValMin[0] + 180.0 - GlobalVars.CurrADHP.MagVar);
                    tipStr = Resources.str15100;
                }

                if (TurnInterDat[k].ValCnt > 0)
                    TextBox601.Text = Math.Round(Kmin, 1).ToString();
                else
                    TextBox601.Text = Math.Round(Kmax, 1).ToString();

                tipStr = tipStr + Functions.DegToStr(Kmin) + Resources.str15103 + Functions.DegToStr(Kmax); // " ? ?? "
            }

            Label608.Text = tipStr;
            tipStr = tipStr.Replace("\r\n", ";  ");
            ToolTip1.SetToolTip(TextBox601, tipStr);
            TextBox601_Validating(TextBox601, new System.ComponentModel.CancelEventArgs());
        }

        private void ComboBox702_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            int k = ComboBox702.SelectedIndex;
            if (k < 0 || k == System.Convert.ToInt32(ComboBox702.Tag))
                return;

            WPt702 = FixBox702[k];

            DirCourse = Functions.ReturnAngleInDegrees(TurnDirector.pPtPrj, WPt702.pPtPrj);
            bool BoolRes = UpdateToNavCurs(DirCourse) == -1;

            if (!BoolRes)
            {
                TextBox702.Tag = System.Math.Round(NativeMethods.Modulus(Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], DirCourse) - GlobalVars.CurrADHP.MagVar)).ToString();
                TextBox702.Text = TextBox702.Tag.ToString();
            }

            if ((!BoolRes) || System.Convert.ToInt32(ComboBox702.Tag) == -1)
                ComboBox702.Tag = ComboBox702.SelectedIndex;
            else
                ComboBox702.SelectedIndex = System.Convert.ToInt32(ComboBox702.Tag);
        }

        private void ComboBox703_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (OptionButton702.Checked)
            {
                if (ComboBox703.SelectedIndex == 0)
                {
                    TextBox702.ReadOnly = false;

                    ComboBox702.Enabled = false;
                    TextBox702.Tag = "a";
                    TextBox702_Validating(TextBox702, new System.ComponentModel.CancelEventArgs());
                }
                else
                {
                    int n = GlobalVars.WPTList.Length;
                    int j = -1;

                    if (n > 0)
                    {
                        TextBox702.ReadOnly = true;
                        ComboBox702.Enabled = true;

                        ComboBox702.Tag = -1;
                        ComboBox702.Items.Clear();

                        FixBox702 = new WPT_FIXType[n];
                        for (int i = 0; i < n; i++)
                        {
                            if (GlobalVars.WPTList[i].Name == ComboBox701.Text)
                                continue;

                            FixBox702[++j] = GlobalVars.WPTList[i];
                            ComboBox702.Items.Add(GlobalVars.WPTList[i].Name);
                        }

                        if (j >= 0)
                        {
                            System.Array.Resize<WPT_FIXType>(ref FixBox702, j + 1);
                            ComboBox702.SelectedIndex = 0;
                        }
                    }

                    if (j < 0)
                    {
                        ComboBox703.Enabled = false;
                        ComboBox703.SelectedIndex = 0;
                    }
                }
            }
        }

        private void HelpBtn_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            NativeMethods.HtmlHelp(0, GlobalVars.HelpFile, GlobalVars.HH_HELP_CONTEXT, HelpContextID);
        }

        private void NomInfoBtn_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            System.Drawing.Point InfoFormOrigin = NomInfoBtn.PointToScreen(new System.Drawing.Point(0, NomInfoBtn.Height));
            NomInfoFrm.ShowMessage(InfoFormOrigin);
        }


        // ---------------------------------------------------------------------------------------
        //  Procedure : InfoBtn_Click
        //  DateTime  : 19.09.2007 15:23
        //  Author    : RuslanA
        //  Purpose   :
        // ---------------------------------------------------------------------------------------
        // 
        private void InfoBtn_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!bFormInitialised)
                return;

            if (ShowPanelBtn.Checked)
            {
                this.Width = 705;
                ShowPanelBtn.Image = Resources.bmpHIDE_INFO;
            }
            else
            {
                this.Width = 545;
                ShowPanelBtn.Image = Resources.bmpSHOW_INFO;
            }

            if (NextBtn.Enabled)
                NextBtn.Focus();
            else
                PrevBtn.Focus();
        }

        private void OkBtn_Click(object eventSender, System.EventArgs eventArgs)
        {
            string RepFileName, RepFileTitle;

            if (!Functions.ShowSaveDialog(out RepFileName, out RepFileTitle))
                return;

            ConvertTracToPoints();
            ReportHeader pReport;

            pReport.Procedure = RepFileTitle;
            pReport.Database = GlobalVars.gAranEnv.ConnectionInfo.Database;
            pReport.Aerodrome = GlobalVars.CurrADHP.Name;
            //pReport.RWY = ComboBox001.Text;

            pReport.Category = CheckBox301.Checked ? ComboBox301.Text : "";

            //if (CheckBox301.Checked)
            //	pReport.Category = ComboBox301.Text;
            //else
            //	pReport.Category = "";

            SaveAccuracy(RepFileName, RepFileTitle, pReport);
            SaveLog(RepFileName, RepFileTitle, pReport);
            SaveProtocol(RepFileName, RepFileTitle, pReport);

            if (CurrPage > 3)
                GuidGeomRep = ReportFile.SaveGeometry(RepFileName, RepFileTitle, pReport, DER, RoutsPoints, GuidAllLen, true);

			DBModule.pObjectDir.SaveAsXml(RepFileName + "_InputData.xml");

			if (!SaveProcedure(RepFileTitle))
                return;

            screenCapture.Save(this);
            saveReportToDB();
            saveScreenshotToDB();
            this.Close();
        }

        private void saveReportToDB()
        {
            saveReportToDB(GuidLogRep, FeatureReportType.Log);
            saveReportToDB(GuidProtRep, FeatureReportType.Protocol);
            saveReportToDB(GuidGeomRep, FeatureReportType.Geometry);
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
            this.Close();
        }

        private void Option601_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (((RadioButton)eventSender).Checked)
            {
                TextBox601.Tag = "a";
                TextBox601_Validating(TextBox601, new System.ComponentModel.CancelEventArgs());
            }
        }

        private void Option602_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (((RadioButton)eventSender).Checked)
            {
                TextBox601.Tag = "a";
                TextBox601_Validating(TextBox601, new System.ComponentModel.CancelEventArgs());
            }
        }

        private void Option001_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (((RadioButton)eventSender).Checked)
                ArrayIVariant = 1;
        }

        private void Option002_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (((RadioButton)eventSender).Checked)
                ArrayIVariant = 2;
        }

        private void OptionButton1001_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!((RadioButton)eventSender).Checked)
                return;

            TextBox1011.ReadOnly = false;
            TextBox1012.ReadOnly = true;
        }

        private void OptionButton1002_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!((RadioButton)eventSender).Checked)
                return;

            TextBox1011.ReadOnly = true;
            TextBox1012.ReadOnly = false;
        }

        private void OptionButton1003_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!((RadioButton)eventSender).Checked)
                return;

            TextBox1011.ReadOnly = true;
            TextBox1012.ReadOnly = true;

            TextBox1011.BackColor = SystemColors.Control;
            TextBox1012.BackColor = SystemColors.Control;

            double Old_TAPDG = CurrPDG;
            double Old_TACurrPDG = TACurrPDG;

            int indx;
            double Common_PDG = Functions.CalcCommon_PDG(oTrnAreaList.Parts, fZNRLen, out indx);

            if (Common_PDG < MinCurrPDG)
                Common_PDG = MinCurrPDG;

            do
            {
                int FuncRes = CalcNewStraightAreaWithFixedLength(ref Common_PDG, ref TACurrPDG);
                if (FuncRes > 0)
                {
                    CurrPDG = Old_TAPDG;
                    TACurrPDG = Old_TACurrPDG;
                    return;
                }

                Common_PDG = TACurrPDG;
                if (CurrPDG > PANS_OPS_DataBase.dpMaxPosPDG.Value * 50.0)
                {
                    CurrPDG = Old_TAPDG;
                    TACurrPDG = Old_TACurrPDG;
                    MessageBox.Show(Resources.str15313, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            while (CurrPDG + GlobalVars.PDGEps < TACurrPDG);

            // =====================================================================================
            IPoint ptCnt;

            if (OptionButton701.Checked)
            {
                IPoint ptTmp = MPtCollection.Point[MPtCollection.PointCount - 1];
                Functions.CircleVectorIntersect(GlobalVars.CurrADHP.pPtPrj, GlobalVars.RModel, ptTmp, ptTmp.M, out ptCnt);
            }
            else if (OptionButton702.Checked)
                ptCnt = TerFixPnt;
            else
                ptCnt = TurnDirector.pPtPrj;

            IProximityOperator pProxi;
            if (OptionButton401.Checked)
                pProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)ZNR_Poly;
            else
                pProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)KK;

            double MaxDist = pProxi.ReturnDistance(ptCnt);

            // =====================================================================================

            TACurrPDG = CurrPDG;
            TextBox1011.Text = Math.Round(100.0 * CurrPDG, 1).ToString();
            TextBox1011.Tag = TextBox1011.Text;

            TextBox1012.Text = Math.Round(100.0 * TACurrPDG, 1).ToString();
            TextBox1012.Tag = TextBox1012.Text;

            double hTurn = fZNRLen * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value;
            hKK = hTurn + DER.pPtPrj[eRWY.PtDER].Z;
            fReachedA = hKK + MaxDist * TACurrPDG;

            TextBox1014.Text = Functions.ConvertHeight(hKK, eRoundMode.NEAREST).ToString();

            double L1 = Functions.ReturnDistanceInMeters(DER.pPtPrj[eRWY.PtDER], TurnFixPnt);
            AltitudeAtTurnFixPnt = CurrPDG * L1 + PANS_OPS_DataBase.dpH_abv_DER.Value + DER.pPtPrj[eRWY.PtDER].Z;

            TextBox1006.Text = Functions.ConvertHeight(fReachedA, eRoundMode.NEAREST).ToString();
            //TextBox1016.Text = TextBox1006.Text;
            TextBox1016_Validating(TextBox1016, null);
            // =====================================================================================

            IPointCollection mPoly = (IPointCollection)Functions.CalcTrajectoryFromMultiPoint(MPtCollection);
            mPoly.AddPoint(ptCnt);
            NomInfoFrm.SetInfo(mPoly, hKK, TACurrPDG);
        }

        private void MultiPage1_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (System.Convert.ToInt32(MultiPage1.Tag) == 1)
                return;

            MultiPage1.Tag = 1;

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

            MultiPage1.Tag = 0;
        }

        private void FillFixWPT()
        {
            double fTASl = Functions.IAS2TAS(fIAS, TurnFixPnt.Z, GlobalVars.CurrADHP.ISAtC); //Round(IAS2TAS(fIAS, TurnFixPnt.Z, CurrADHP.ISAtC));
            double TurnR = Functions.Bank2Radius(fBankAngle, fTASl);
            IPoint ptCnt = Functions.PointAlongPlane(TurnFixPnt, DepDir + 90.0 * TurnDir, TurnR);

            int i, j = -1, n = GlobalVars.WPTList.Length;
            System.Array.Resize<WPT_FIXType>(ref FixWPT, n);

            for (i = 0; i < n; i++)
            {
                double fDist = Functions.ReturnDistanceInMeters(GlobalVars.WPTList[i].pPtPrj, ptCnt);
                if (fDist > TurnR && fDist < GlobalVars.RModel)
                {
                    j++;
                    FixWPT[j] = GlobalVars.WPTList[i];
                }
            }

            OptionButton703.Enabled = j >= 0;
            System.Array.Resize<WPT_FIXType>(ref FixWPT, j + 1);
        }

        private void NextBtn_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            int i, k, n;

            double CurrAngle, NewTA_PDG, fMagVar, CircleR;
            double DepDist, AztMin, AztMax;
            double hTurn, fTmp;

            Interval mIntr;
            IPointCollection mPoly;
            ObstacleContainer TmpInList;

            NativeMethods.ShowPandaBox(this.Handle.ToInt32());

            switch (MultiPage1.SelectedIndex)
            {
                case 0:
                    FillFrontList();

                    if (FrontList.Length <= 0)
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15314, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!double.TryParse(TextBox003.Text, out fTmp))
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15315, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextBox003.Focus();
                        return;
                    }

                    if (Functions.DeConvertDistance(fTmp) + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Rounding / GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Multiplier < RMin)
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15315, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextBox003.Focus();
                        return;
                    }

                    // If ComboBox003.ListIndex = 0 Then
                    //     MOCLimit = 100000.0
                    // Else
                    MOCLimit = Functions.DeConvertHeight(double.Parse(ComboBox003.Text));
                    // End If

                    // =================== Start Log ================================
                    //     LogStr LoadResString(15316) + MultiPage1.TabCaption(0)              '"???????? 1 - "
                    //     LogStr LoadResString(15317) + ComboBox001.Text                      '"    ???:           "
                    //     LogStr LoadResString(15318) + TextBox001.Text                       '"    ?? ???:       "
                    //     LogStr LoadResString(15319) + TextBox004.Text                       '"    ?? ???:       "
                    //     LogStr LoadResString(15320) + TextBox002.Text                       '"    ????? ??? (?): "
                    //     If Option001 Then
                    //         sTmp = Option001.Caption
                    //     Else
                    //         sTmp = Option002.Caption
                    //     End If
                    //     LogStr LoadResString(15321) + sTmp                                  '"    ??????? ??????????? ??????? ???? 1: "
                    // 
                    //     LogStr LoadResString(15322) + TextBox003.Text                       '"    ?????? ?????? ?????????? ?? ??? (?): "
                    //     LogStr LoadResString(15323)                                         '"    ?????? ? ????????????:"
                    //     LogStr LoadResString(15324) + TextBox006.Text                       '"    ???????????? ???????? (?):       "
                    //     LogStr LoadResString(15325) + TextBox005.Text                       '"    ?????????? ? ???????? ???????:   "

                    // =================== End Log ================================
                    break;
                case 1:
                    ReportsFrm.SetTabVisible(-1, false);

                    Label202.Text = Label109.Text;
                    k = ComboBox101.SelectedIndex;
                    fMagVar = FrontList[k].MagVar;
                    AztMin = System.Math.Round(FrontList[k].ValMin[0] - fMagVar + 0.4999);
                    AztMax = System.Math.Round(FrontList[k].ValMax[0] - fMagVar - 0.4999);

                    FillAllStraightList();

                    SpinButton201.Enabled = false;

                    SpinButton201.Minimum = System.Convert.ToDecimal(AztMin);
                    SpinButton201.Value = System.Convert.ToDecimal(AztMin);

                    if (AztMin >= 330 && AztMax <= 30)
                        SpinButton201.Maximum = System.Convert.ToDecimal(AztMax + 360.0);
                    else
                        SpinButton201.Maximum = System.Convert.ToDecimal(AztMax);

                    SpinButton201.Enabled = true;

                    MinPDG = 9999.0;
                    MinAngle = MinPDG;

                    int iMin = (int)SpinButton201.Minimum;
                    int iMax = (int)SpinButton201.Maximum;

                    for (i = iMin; i <= iMax; i++)
                    {
                        DepDir = Functions.Azt2Dir(DER.pPtGeo[eRWY.PtDER], (double)i + fMagVar);

                        //         CurrAngle = NativeMethods.Modulus (DepDir - RWYDir, 360.0)
                        CurrAngle = DepDir - RWYDir;
                        DerShift = CalcShift();

                        CreateStraightZones(PANS_OPS_DataBase.dpPDG_Nom.Value, -100, false);
                        CurrPDG = Functions.CalcLocalPDG(oZNRList.Parts, out iDominicObst);

                        if ((CurrPDG < MinPDG) || ((CurrPDG == MinPDG) && (System.Math.Abs(CurrAngle) < System.Math.Abs(MinAngle))))
                        {
                            MinPDG = CurrPDG;
                            MinAngle = CurrAngle;
                            k = i;
                        }
                    }

                    if (SpinButton201.Value == k)
                        SpinButton201_ValueChanged(SpinButton201, null);
                    else
                        SpinButton201.Value = k;

                    // =================== Start Log ================================
                    //     LogStr LoadResString(15326) + MultiPage1.TabCaption(1)     '"???????? 2 - "
                    //     LogStr LoadResString(15327) + ComboBox101.Text             '"    ???????? ?????????:   "
                    //     LogStr LoadResString(15328) + Label103.Caption             '"    ??? ????????:         "
                    //     LogStr LoadResString(15329) + Label105.Caption             '"    ???????? ??????????????? ???????/???????: "
                    //     LogStr LoadResString(15330)                                '"    ???????????? ???:"
                    //     LogStr "    " + Label106.Caption
                    //     LogStr "    " + Label107.Caption
                    // =================== End Log ================================
                    break;
                case 2:
                    if (!double.TryParse(TextBox201.Text, out fTmp))
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15331, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextBox201.Focus();
                        return;
                    }

                    OkBtn.Enabled = true;
                    //     TextBox301 = TextBox201.Text
                    //     TextBox301 = CStr(Modulus(CDbl(TextBox201.Text) + 90.0 * FrontList(ComboBox101.ListIndex).TypeCode, 360.0))

                    if ((FrontList[ComboBox101.SelectedIndex].TypeCode == eNavaidType.VOR) && FrontList[ComboBox101.SelectedIndex].Front)
                        TextBox301.Text = NativeMethods.Modulus(fTmp + 180.0, 360.0).ToString();
                    else
                        TextBox301.Text = TextBox201.Text;

                    //     CurrAngle = System.Math.Round(Modulus(DepDir - RWYDir, 360.0))
                    CurrAngle = System.Math.Round(DepDir - RWYDir);

                    TextBox302.Text = System.Math.Abs(CurrAngle).ToString();
                    if (CurrAngle < 0)
                        Label304.Text = Resources.str15332; // "? ???????"
                    else
                        Label304.Text = Resources.str15333; // "? ??????"

                    TextBox303.Text = TextBox202.Text; // CStr(CurrPDG * 100.0)
                    TextBox304.Text = TextBox203.Text;

                    // ===========================================================================================================
                    n = oZNRList.Parts.Length;

                    //     fEnrouteMOC = DeConvertHeight(CDbl(ComboBox003.Text))

                    fStraightDepartTermAlt = MOCLimit;
                    int ix = -1;

                    for (i = 0; i < n; i++)
                    {
                        double fReqH = oZNRList.Parts[i].Height + MOCLimit * oZNRList.Parts[i].fTmp;

                        if (fReqH > fStraightDepartTermAlt)
                        {
                            fStraightDepartTermAlt = fReqH;
                            ix = i;
                        }
                    }

                    ToolTip1.SetToolTip(TextBox307, "");

                    if (ix >= 0)
                        ToolTip1.SetToolTip(TextBox307, Resources.str03106 + oZNRList.Obstacles[oZNRList.Parts[ix].Owner].UnicalName);

                    TextBox307.Text = Functions.ConvertHeight(fStraightDepartTermAlt + DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.NEAREST).ToString();
                    ComboBox301_SelectedIndexChanged(ComboBox301, new System.EventArgs());
                    // ===========================================================================================================

                    // =================== Start Log ================================
                    //     LogStr LoadResString(15334) + MultiPage1.TabCaption(2)                                       '"???????? 3 - "
                    //     LogStr LoadResString(15335) + TextBox201.Text + "°"                                          '"    ??????/??????: "
                    //     LogStr LoadResString(15336) + TextBox202.Text + "%"                                          '"    PDG ?????:     "
                    //     LogStr LoadResString(15337) + TextBox203.Text + LoadResString(15028)                         '"    ?????? ??????????? ? PDG 3,3% ??? DER: " m"        "
                    //     LogStr LoadResString(15339) + TextBox204.Text + LoadResString(15028)                         '"    ????????? ?????????????? ??????? ? ??????????: "" m"
                    //     LogStr LoadResString(15340) + TextBox205.Text + LoadResString(15028) + Label212.Caption      '"    ???????? DER: "" m "
                    // =================== End Log ================================
                    break;
                case 3:
                    // ===================================================================
                    CheckBox401.Checked = true;
                    //     hTurn = dpGui_Ar1.Value + DER.pPtPrj(ptEnd).Z
                    TextBox402_Validating(TextBox402, new System.ComponentModel.CancelEventArgs());

                    if (OptionButton401.Checked)
                        OptionButton401_CheckedChanged(OptionButton401, new System.EventArgs());
                    else if (OptionButton402.Checked)
                        OptionButton402_CheckedChanged(OptionButton402, new System.EventArgs());
                    else if (OptionButton403.Checked)
                        OptionButton403_CheckedChanged(OptionButton403, new System.EventArgs());
                    else
                        OptionButton401.Checked = true;

                    // =================== Start Log ================================
                    //     LogStr LoadResString(15342) + MultiPage1.TabCaption(3)                          '"???????? 4 - "
                    //     LogStr LoadResString(15343) + TextBox301.Text + "°"                             '"    ??:  "
                    //     LogStr LoadResString(15344) + TextBox302.Text + Label304.Caption                '"    ???????? ?????: "
                    //     LogStr LoadResString(15345) + TextBox303.Text + "%"                             '"    PDG ?????:      "
                    //     LogStr LoadResString(15346) + TextBox304.Text + LoadResString(15129)            '"    ?????? ??????????? ? ???. ?????????:         "" ??????"
                    //     LogStr LoadResString(15347) + TextBox305.Text                                   '"    ??????????? ????????? ????: "
                    //     LogStr LoadResString(15348) + TextBox306.Text                                   '"    ??????????????????? ???????????:            "
                    // =================== End Log ================================
                    break;
                case 4:
                    // ===================================================================
                    if (!double.TryParse(TextBox401.Text, out fTmp))
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15349, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextBox401.Focus();
                        return;
                    }

                    if (!double.TryParse(TextBox402.Text, out fTmp))
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15350, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextBox402.Focus();
                        return;
                    }

                    CheckBox501.Checked = false;
                    if (OptionButton403.Checked)
                    {
                        DepDist = CalcRNEGlobalRange(out CurrPDG, TrackAdjust, true);

                        k = ComboBox101.SelectedIndex;
                        hMinTurn = System.Math.Round(DepDist * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value - 0.4999);
                        if (FrontList[k].TypeCode == eNavaidType.VOR)
                            CircleR = hMinTurn * System.Math.Tan(GlobalVars.DegToRadValue * Navaids_DataBase.VOR.ConeAngle);
                        else
                            CircleR = hMinTurn * System.Math.Tan(GlobalVars.DegToRadValue * Navaids_DataBase.NDB.ConeAngle);
                        hMaxTurn = System.Math.Round((DepDist + 2.0 * CircleR) * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value + 0.4999);

                        Label506.Text = "";
                        Label506.Width = 229;
                    }
                    else
                    {
                        mIntr = CalcGlobalRange(PANS_OPS_DataBase.dpPDG_Nom.Value - 0.01, out CurrPDG, out TrackAdjust, true);

                        hMinTurn = System.Math.Round(mIntr.Left * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value - 0.4999);
                        hMaxTurn = System.Math.Round(mIntr.Right * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value + 0.4999);

                        Label506.Width = 229;

                        if (PrevDet.Owner != -1/*Resources.str39014*/)
                        {
                            Label506.Text = Resources.str15351 + "\r\n" +
                                " ID:                         " + oZNRList.Obstacles[PrevDet.Owner].UnicalName + ";" + "\r\n" +
                                Resources.str15352 + (System.Math.Round(PrevDet.PDG + 0.0004999, 3) * 100.0).ToString() + "%;" + "\r\n" +
                                Resources.str15353 + Functions.ConvertDistance(PrevDet.Dist, eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit + ";" + "\r\n" +
                                Resources.str15354 + Functions.DegToStr(System.Math.Round(PrevDet.CourseAdjust));               // "??????????? ??????????????? ????????: "" PDG ??????? ??? ???:        "" ???????? ?? DER:            "" ?;"" ??????????? ???? ?????????: "
                        }
                        else
                            Label506.Text = Resources.str15355 + Math.Round(100.0 * CurrPDG, 1).ToString() + Resources.str15356;    // "? ?????????? ""% ?????????????? ?????? ????? ?? ???? ????"

                        Label506.Width = 229;
                    }

                    TextBox505.Text = Math.Round(100.0 * CurrPDG, 1).ToString();
                    TextBox503.Text = NativeMethods.Modulus(Functions.RoundAngle(Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], DepDir) - GlobalVars.CurrADHP.MagVar)).ToString();

                    if (hMaxTurn < hMinTurn)
                        hMaxTurn = hMinTurn;

                    TextBox501.Text = Functions.ConvertHeight(hMinTurn, eRoundMode.NEAREST).ToString();
                    if (hMinTurn < PANS_OPS_DataBase.dpGui_Ar1.Value)
                        TextBox501.ForeColor = System.Drawing.Color.FromArgb(0XFF0000);
                    else
                        TextBox501.ForeColor = System.Drawing.Color.FromArgb(0);

                    TextBox502.Text = Functions.ConvertHeight(hMaxTurn, eRoundMode.NEAREST).ToString();
                    if (hMaxTurn < PANS_OPS_DataBase.dpGui_Ar1.Value)
                        TextBox502.ForeColor = System.Drawing.Color.FromArgb(0XFF0000);
                    else
                        TextBox502.ForeColor = System.Drawing.Color.FromArgb(0);

                    PrevPDG = TextBox505.Text;
                    MinGPDG = CurrPDG;
                    // ==============================================================
                    mPoly = new Polyline();
                    mPoly.AddPoint(PtDerShift);
                    mPoly.AddPoint(Functions.PointAlongPlane(PtDerShift, DepDir, GlobalVars.RModel));

                    Functions.DeleteGraphicsElement(GlobalVars.StrTrackElem);
                    GlobalVars.StrTrackElem = Functions.DrawPolyline(mPoly, GlobalVars.StrTrackElemColor, 1.0, false);

                    if (GlobalVars.ButtonControl5State)
                    {
                        pGraphics.AddElement(GlobalVars.StrTrackElem, 0);
                        GlobalVars.StrTrackElem.Locked = true;
                    }

                    //Functions.RefreshCommandBar(_mTool, 2);
                    GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                    break;
                case 5:
                    if (!double.TryParse(TextBox505.Text, out fTmp))
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15370, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextBox505.Focus();
                        return;
                    }

                    CheckState = true;
                    if (PrevPDG != TextBox505.Text)
                    {
                        NewTA_PDG = System.Math.Round(0.01 * double.Parse(TextBox505.Text) + 0.0004999, 3);
                        // =================================================
                        if (OptionButton403.Checked)
                        {
                            if (CheckBox501.Checked)
                                DepDist = CalcRNELocalRange(out CurrPDG, TrackAdjust, out CircleR, true, NewTA_PDG);
                            else
                                DepDist = CalcRNEGlobalRange(out CurrPDG, TrackAdjust, true, NewTA_PDG);

                            k = ComboBox101.SelectedIndex;
                            hMinTurn = System.Math.Round(DepDist * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value - 0.4999);

                            if (FrontList[k].TypeCode == eNavaidType.VOR)
                                CircleR = hMinTurn * System.Math.Tan(GlobalVars.DegToRadValue * Navaids_DataBase.VOR.ConeAngle);
                            else
                                CircleR = hMinTurn * System.Math.Tan(GlobalVars.DegToRadValue * Navaids_DataBase.NDB.ConeAngle);

                            hMaxTurn = System.Math.Round((DepDist + 2.0 * CircleR) * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value + 0.4999);

                            Label506.Text = "";
                            Label506.Width = 229;
                        }
                        else
                        {
                            if (CheckBox501.Checked)
                                mIntr = CalcLocalRange(NewTA_PDG, out CurrPDG, TrackAdjust, true);
                            else
                                mIntr = CalcGlobalRange(NewTA_PDG, out CurrPDG, out TrackAdjust, true);

                            hMinTurn = System.Math.Round(mIntr.Left * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value - 0.4999);
                            hMaxTurn = System.Math.Round(mIntr.Right * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value + 0.4999);

                            Label506.Width = 229;

                            //if (PrevDet.Owner != Resources.str39014)
                            if (PrevDet.Owner != -1)
                            {
                                Label506.Text = Resources.str15372 + "\r\n" +
                                    " ID:                         " + oZNRList.Obstacles[PrevDet.Owner].UnicalName + ";" + "\r\n" +
                                    Resources.str15373 + (System.Math.Round(PrevDet.PDG + 0.0004999, 3) * 100.0).ToString() + "%;" + "\r\n" +
                                    Resources.str15374 + Functions.ConvertDistance(PrevDet.Dist, eRoundMode.NEAREST).ToString() + " "
                                    + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit + ";" + "\r\n" + Resources.str15376 +
                                    Functions.DegToStr(System.Math.Round(PrevDet.CourseAdjust)); // "??????????? ??????????????? ????????: "" PDG ??????? ??? ???:        "" ???????? ?? DER:            "" ?;"" ??????????? ???? ?????????: "
                            }
                            else
                                Label506.Text = Resources.str15377 + Math.Round(100.0 * CurrPDG, 1).ToString() + Resources.str15378; // "? ?????????? ""% ?????????????? ?????? ????? ?? ???? ????"

                            Label506.Width = 229;
                        }

                        TextBox505.Text = Math.Round(100.0 * CurrPDG, 1).ToString();
                        TextBox503.Text = NativeMethods.Modulus(Functions.RoundAngle(Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], DepDir) - GlobalVars.CurrADHP.MagVar)).ToString();

                        if (hMaxTurn < hMinTurn)
                            hMaxTurn = hMinTurn;

                        TextBox501.Text = Functions.ConvertHeight(hMinTurn, eRoundMode.NEAREST).ToString();
                        if (hMinTurn < PANS_OPS_DataBase.dpGui_Ar1.Value)
                            TextBox501.ForeColor = System.Drawing.Color.FromArgb(0xFF0000);
                        else
                            TextBox501.ForeColor = System.Drawing.Color.FromArgb(0);

                        TextBox502.Text = Functions.ConvertHeight(hMaxTurn, eRoundMode.NEAREST).ToString();
                        if (hMaxTurn < PANS_OPS_DataBase.dpGui_Ar1.Value)
                            TextBox502.ForeColor = System.Drawing.Color.FromArgb(0xFF0000);
                        else
                            TextBox502.ForeColor = System.Drawing.Color.FromArgb(0);

                        PrevPDG = TextBox505.Text;
                        NativeMethods.HidePandaBox();
                        return;
                    }

                    MinCurrPDG = 0.01 * double.Parse(TextBox505.Text);

                    // =================================================

                    Functions.DeleteGraphicsElement(GlobalVars.FIXElem);
                    GlobalVars.FIXElem = null;

                    ComboBox601.Items.Clear();

                    if (!OptionButton403.Checked) //(OptionButton401.Checked || OptionButton402.Checked)
                    {
                        //     ComboBox603.ListIndex = 0
                        //     ComboBox603.Tag = "0"

                        //     ComboBox604.ListIndex = 0
                        //     ComboBox604.Tag = "0"

                        TextBox602.Tag = "a";
                        TextBox603.Tag = "a";
                        if (OptionButton401.Checked)
                        {
                            Label608.Text = Resources.str15380 + TextBox501.Text + " " + Label513.Text + Resources.str15162 + " " + TextBox502.Text + " " + Label513.Text;
                            ToolTip1.SetToolTip(TextBox602, Label608.Text);

                            TextBox602.Text = TextBox501.Text;
                            TextBox603.Text = Functions.ConvertHeight(Functions.DeConvertHeight(double.Parse(TextBox501.Text)) + DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.NEAREST).ToString();
                            TextBox603_Validating(TextBox603, new System.ComponentModel.CancelEventArgs());
                        }
                        else
                        {
                            TurnInterDat = Functions.GetValidNavs(UnitedPolygon as IPolygon, DER, DepDir, hMinTurn, hMaxTurn, CurrPDG);

                            n = TurnInterDat.GetUpperBound(0);
                            if (n >= 0)
                            {
                                for (i = 0; i <= n; i++)
                                    ComboBox601.Items.Add(TurnInterDat[i].CallSign);

                                ComboBox601.SelectedIndex = 0;
                                //ComboBox601_SelectedIndexChanged(ComboBox601, new System.EventArgs());
                            }
                            else //if (OptionButton402.Checked)
                            {
                                NativeMethods.HidePandaBox();
                                MessageBox.Show(Resources.str15383, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            TextBox602_Validating(TextBox602, new System.ComponentModel.CancelEventArgs());
                        }
                    }
                    else //if (OptionButton403.Checked)
                    {
                        CurrPage = MultiPage1.SelectedIndex + 1;
                        MultiPage1.SelectedIndex = CurrPage;

                        if (GlobalVars.StraightAreaElem != null)
                        {
                            if (GlobalVars.StraightAreaElem is IGroupElement)
                                for (i = 0; i < (GlobalVars.StraightAreaElem as IGroupElement).ElementCount; i++)
                                    Functions.DeleteGraphicsElement((GlobalVars.StraightAreaElem as IGroupElement).Element[i]);
                            else
                                Functions.DeleteGraphicsElement(GlobalVars.StraightAreaElem);
                            GlobalVars.StraightAreaElem = null;
                        }

                        //GlobalVars.ButtonControl2State = false;

                        DepDist = CalcRNELocalRange(out CurrPDG, TrackAdjust, out CircleR, false, CurrPDG);
                        k = ComboBox101.SelectedIndex;

                        FIXOnRNE(DepDist, CircleR);
                        // ===============================================
                        // ==========================================================================
                        if (DepDist == GlobalVars.RModel)
                        {
                            int nN = oZNRList.Obstacles.Length;
                            TmpInList.Obstacles = new Obstacle[nN];
                            System.Array.Copy(oZNRList.Obstacles, TmpInList.Obstacles, nN);

                            nN = oZNRList.Parts.Length;
                            TmpInList.Parts = new ObstacleData[nN];
                            System.Array.Copy(oZNRList.Parts, TmpInList.Parts, nN);
                        }
                        else
                            Functions.GetObstInRange(oZNRList, out TmpInList, DepDist);

                        ReportsFrm.FillPage4(TmpInList, OptionButton401.Checked);
                        //===========================================================
                        FillFixWPT();

                        fTmp = PANS_OPS_DataBase.dpT_Gui_dist.Value / (Functions.IAS2TAS(fIAS, (TurnFixPnt.Z), GlobalVars.CurrADHP.ISAtC) + GlobalVars.CurrADHP.WindSpeed) * 3.6;
                        ToolTip1.SetToolTip(TextBox701, Resources.str15395 + System.Math.Round(fTmp).ToString() + " " + Resources.str00057); // "???????????? ????????: "" ???"

                        TextBox701.Text = "0";
                        TextBox701.Tag = "0";
                        TextBox703.Text = "30";
                        TextBox703.Tag = "30";

                        CheckState = true;

                        if (GlobalVars.StraightAreaElem != null)
                        {
                            if (GlobalVars.StraightAreaElem is IGroupElement)
                                for (i = 0; i < (GlobalVars.StraightAreaElem as IGroupElement).ElementCount; i++)
                                    Functions.DeleteGraphicsElement((GlobalVars.StraightAreaElem as IGroupElement).Element[i]);
                            else
                                Functions.DeleteGraphicsElement(GlobalVars.StraightAreaElem);
                            GlobalVars.StraightAreaElem = null;
                        }

                        if (OptionButton701.Checked)
                            OptionButton701_CheckedChanged(OptionButton701, new EventArgs());
                        else if (OptionButton702.Checked)
                            OptionButton702_CheckedChanged(OptionButton702, new EventArgs());
                        else if (OptionButton703.Checked)
                            OptionButton703_CheckedChanged(OptionButton703, new EventArgs());
                        else
                            OptionButton701.Checked = true;
                    }
                    // ==========================================================================
                    // =================== Start Log ================================
                    //     LogStr LoadResString(15384) + MultiPage1.TabCaption(5)                   '"???????? 6 - "
                    //     LogStr LoadResString(15385)                                              '"    ?????????? ?????? ???????????? ??:"
                    //     LogStr LoadResString(15386) + TextBox501.Text                            '"    H ????????? ??? DER, min(m) "
                    //     LogStr LoadResString(15387) + TextBox502.Text                            '"    H ????????? ??? DER, max(m) "
                    //     LogStr LoadResString(15388) + TextBox503.Text + "°"                      '"    ?? ??????: "
                    //     LogStr "    " + Label506.Caption
                    //     LogStr LoadResString(15389)                                              '"    ?????????? ??????????? ???? 1 :"
                    //     LogStr LoadResString(15390) + TextBox504.Text + "? " + ComboBox501.Text  '"    ???????? ????? ?????? ?? ????? ???: "
                    //     LogStr LoadResString(15391) + TextBox505.Text + "%"                      '"    PDG ? ???? 1, ?????????? ????? ??:  "
                    // 
                    //     If Label512.Visible Then
                    //         LogStr LoadResString(15392)                                          '"    ??????????? ??????? ?????(??????????? PDG ? ???????? ????? ??? ???? 1 ?????? ? ??????????)"
                    //     End If
                    // =================== End Log ================================
                    break;
                case 6:
                    if (OptionButton401.Checked && !double.TryParse(TextBox602.Text, out fTmp))
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15393, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextBox602.Focus();
                        return;
                    }

                    if ((!OptionButton401.Checked) && !double.TryParse(TextBox601.Text, out fTmp))
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15394 + Label603.Text + Resources.str15382, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextBox601.Focus();
                        return;
                    }

                    // ===============================================
                    FillFixWPT();
                    //     TextBox702.Text = CStr(System.Math.Round(Modulus(Dir2Azt(DER.pPtprj(ptEnd), DepDir), 360.0)) - MagVar)
                    // ===============================================

                    fTmp = PANS_OPS_DataBase.dpT_Gui_dist.Value / (Functions.IAS2TAS(fIAS, (TurnFixPnt.Z), GlobalVars.CurrADHP.ISAtC) + GlobalVars.CurrADHP.WindSpeed) * 3.6;
                    ToolTip1.SetToolTip(TextBox701, Resources.str15395 + System.Math.Round(fTmp).ToString() + " " + Resources.str00057); // "???????????? ????????: "" ???"

                    TextBox701.Text = "0";
                    TextBox701.Tag = "0";
                    TextBox703.Text = "30";
                    TextBox703.Tag = "30";

                    CheckState = true;

                    if (GlobalVars.StraightAreaElem != null)
                    {
                        if (GlobalVars.StraightAreaElem is IGroupElement)
                            for (i = 0; i < (GlobalVars.StraightAreaElem as IGroupElement).ElementCount; i++)
                                Functions.DeleteGraphicsElement((GlobalVars.StraightAreaElem as IGroupElement).Element[i]);
                        else
                            Functions.DeleteGraphicsElement(GlobalVars.StraightAreaElem);
                        GlobalVars.StraightAreaElem = null;
                    }

                    if (OptionButton701.Checked)
                        OptionButton701_CheckedChanged(OptionButton701, new EventArgs());
                    else if (OptionButton702.Checked)
                        OptionButton702_CheckedChanged(OptionButton702, new EventArgs());
                    else if (OptionButton703.Checked)
                        OptionButton703_CheckedChanged(OptionButton703, new EventArgs());
                    else
                        OptionButton701.Checked = true;

                    //GlobalVars.ButtonControl2State = false;
                    //Functions.RefreshCommandBar(_mTool, 2);
                    break;
                case 7:
                    if (!((OptionButton703.Checked || OptionButton702.Checked || OptionButton701.Checked)))
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15404, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if ((OptionButton702.Checked || OptionButton701.Checked) && !double.TryParse(TextBox702.Text, out fTmp))
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15405, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextBox702.Focus();
                        return;
                    }

                    if (OptionButton702.Checked)
                    {
                        if (!double.TryParse(TextBox703.Text, out fTmp))
                        {
                            NativeMethods.HidePandaBox();
                            MessageBox.Show(Resources.str15403, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            TextBox703.Focus();
                            return;
                        }

                        if (!double.TryParse(TextBox701.Text, out fTmp))
                        {
                            NativeMethods.HidePandaBox();
                            MessageBox.Show(Resources.str15406, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            TextBox701.Focus();
                            return;
                        }
                    }

                    CheckState = false;
                    if (OptionButton701.Checked || (TurnDirector.TypeCode < 0))
                    {
                        CurrPage = MultiPage1.SelectedIndex + 2;
                        MultiPage1.SelectedIndex = CurrPage;

                        SecPoly = (IPolygon)new ESRI.ArcGIS.Geometry.Polygon();

                        hTurn = CalcTAParams();
                        //TerminationParams(hTurn);
                        Frame1003.Visible = false;
                    }

                    // =================== Start Log ================================
                    //     LogStr LoadResString(15407) + MultiPage1.TabCaption(7)                              '"???????? 8 - "
                    //     If OptionButton701 Then
                    //        LogStr "    " + OptionButton701.Caption
                    //        LogStr LoadResString(15408) + TextBox702.Text + "°"                              '"    ???? ?????????? ???????? :"
                    //     ElseIf OptionButton702 Then
                    //         LogStr "    " + OptionButton702.Caption
                    //         LogStr LoadResString(15409) + ComboBox701.Text + "  " + Label702.Caption        '"    ???: "
                    //         LogStr LoadResString(15410) + TextBox701.Text + LoadResString(15411)            '"    ????? ????????????:"" ???"
                    //         LogStr LoadResString(15412) + TextBox702.Text + "°"                             '"    ???? ?????????? ????????: "
                    //         LogStr "    Intercept Angle: " + TextBox703.Text + "°"
                    //         LogStr LoadResString(15413) + Label710.Caption + LoadResString(15028)           '"    ????? ????? ????????? ????: "" m"
                    //         LogStr "    " + Label709.Caption
                    //     Else
                    //         LogStr "    " + OptionButton703.Caption
                    //         LogStr LoadResString(15415) + ComboBox701.Text + " " + Label702.Caption         '"    ?? ???/FIX/WPT"
                    //         LogStr LoadResString(15416) + TextBox702.Text + "°"                             '"    ???? ?????????? ????????: "
                    //         If CheckBox701.Value = 1 Then
                    //         LogStr LoadResString(15417)                                                     '"    ??????????? ???? ?? ???????????"
                    //         End If
                    //     End If
                    // =================== End Log ================================
                    break;
                case 8:
                    CheckBox903.Enabled = OptionButton803.Checked && OptionButton803.Enabled;
                    if (!(CheckBox903.Enabled))
                    {
                        CheckBox903.Checked = false;
                    }

                    CheckBox906.Enabled = OptionButton806.Checked && OptionButton806.Enabled;
                    if (!(CheckBox906.Enabled))
                    {
                        CheckBox906.Checked = false;
                    }

                    SecondArea(TurnDir, BaseArea);
                    //     Dbug = True
                    // =================== Start Log ================================
                    //     LogStr LoadResString(15418) + MultiPage1.TabCaption(8)                              '"???????? 9 - "
                    // '    LogStr "Page 9 - C??????? ???"
                    //     If Frame801.Enabled Then
                    //         LogStr LoadResString(15419)                                                     '"    ?? ?????????? ??????? ?????????:"
                    //         If OptionButton801 Then
                    //             LogStr LoadResString(15420)                                                 '"    ?????????? ????????? ?????"
                    //         End If
                    //         If OptionButton802 Then
                    //             LogStr LoadResString(15421)                                                 '"    ????????? ???? ?? 15? ?? ????????? ?????"
                    //         End If
                    // 
                    //         If OptionButton803 Then
                    //             LogStr LoadResString(15422)                                                 '"    ????????? ???? ????????"
                    //         End If
                    //     Else
                    //         LogStr LoadResString(15423)                                                     '"    ?? ?????????? ??????? ????????? ???????? ????????? ?? ???????? PANS-OPS"
                    //     End If
                    // 
                    //     If Frame802.Enabled Then
                    //         LogStr LoadResString(15424)                                                     '"    ?? ??????? ??????? ?????????:"
                    //         If OptionButton804 Then
                    //             LogStr LoadResString(15420)                                                 '"    ?????????? ????????? ?????"
                    //         End If
                    //         If OptionButton805 Then
                    //             LogStr LoadResString(15421)                                                 '"    ????????? ???? ?? 15? ?? ????????? ?????"
                    //         End If
                    // 
                    //         If OptionButton806 Then
                    //             LogStr LoadResString(15422)                                                 '"    ????????? ???? ????????"
                    //         End If
                    //     Else
                    //         LogStr LoadResString(15414)                                                     '"    ?? ??????? ??????? ????????? ???????? ????????? ?? ???????? PANS-OPS"
                    //     End If
                    // =================== End Log ================================
                    break;
                case 9:
                    if (!((CheckBox901.Checked || CheckBox902.Checked || CheckBox903.Checked)))
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15425, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!((CheckBox904.Checked || CheckBox905.Checked || CheckBox906.Checked)))
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15425, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    hTurn = CalcTAParams();
                    //TerminationParams(hTurn);
                    Frame1003.Visible = OptionButton702.Checked;
                    // =================== Start Log ================================
                    //     LogStr LoadResString(15426) + MultiPage1.TabCaption(9)                    '"???????? 10 - "
                    //     LogStr LoadResString(15427)                                               '"    ????????? ?? ?????????? ???????:"
                    //     If CheckBox901.Value = 1 Then
                    //         LogStr LoadResString(15428)                                           '"    ?? ??????? ???? ??????? ?????????"
                    //     End If
                    //     If CheckBox902.Value = 1 Then
                    //         LogStr LoadResString(15429)                                           '"    ?? ??????? ? 30?"
                    //     End If
                    //     If CheckBox903.Value = 1 Then
                    //         LogStr LoadResString(15430)                                           '"    ?? ????? ???-??????? ??"
                    //     End If
                    // 
                    //     LogStr LoadResString(15431)                                               '"    ????????? ?? ??????? ???????:"
                    //     If CheckBox904.Value = 1 Then
                    //         LogStr LoadResString(15432)                                           '"    ?? ??????? ???? ??????? ?????????"
                    //     End If
                    //     If CheckBox905.Value = 1 Then
                    //         LogStr LoadResString(15429)                                           '"    ?? ??????? ? 30?"
                    //     End If
                    //     If CheckBox906.Value = 1 Then
                    //         LogStr LoadResString(15433)                                           '"    ?? ????? ???-??????? ??"
                    //     End If
                    // =================== End Log ================================
                    break;
            }

            // ===================================================================
            screenCapture.Save(this);
            CurrPage = MultiPage1.SelectedIndex + 1;

            // MsgBox "1 - CurrPage = " + CStr(CurrPage)
            // MsgBox "2 - MultiPage1.Tabs = " + CStr(MultiPage1.Tabs)

            MultiPage1.SelectedIndex = CurrPage;

            //  2007
            FocusStepCaption((MultiPage1.SelectedIndex));

            NextBtn.Enabled = MultiPage1.SelectedIndex < MultiPage1.TabPages.Count - 1;
            PrevBtn.Enabled = MultiPage1.SelectedIndex > 0;

            if (MultiPage1.SelectedIndex == 1)
                ComboBox101_SelectedIndexChanged(ComboBox101, new System.EventArgs());

            if (CurrPage == MultiPage1.TabPages.Count - 1)
                OkBtn.Enabled = true;

            // OkBtn.Enabled = CurrPage = MultiPage1.Tabs - 1

            if ((CurrPage == 3) && !CheckBox301.Checked)
                NextBtn.Enabled = false;

            NativeMethods.HidePandaBox();

            this.HelpContextID = 6000 + 100 * (MultiPage1.SelectedIndex + 1);

            this.Visible = false;
            this.Show(GlobalVars.Win32Window);

            this.Activate();
            // Me.Activate()
        }

        private void OptionButton804_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!bFormInitialised)
                return;

            if (((RadioButton)eventSender).Checked)
                ApplayOptions();
        }


        private void OptionButton806_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!bFormInitialised)
                return;

            if (((RadioButton)eventSender).Checked)
                ApplayOptions();
        }


        private void OptionButton802_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!bFormInitialised)
                return;

            if (((RadioButton)eventSender).Checked)
                ApplayOptions();
        }


        private void OptionButton805_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!bFormInitialised)
                return;

            if (((RadioButton)eventSender).Checked)
                ApplayOptions();
        }

        private void OptionButton401_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!bFormInitialised)
                return;

            if (!((RadioButton)eventSender).Checked)
                return;

            Label604.Text = Resources.str01410;
            Label606.Text = Resources.str15434; // "????????????? ???:"

            Label625.Text = Resources.str02621;

            Label1001_15.Text = Resources.str03909;

            CheckBox401.Enabled = true;
            //TextBox505.ReadOnly = false;

            Frame601.Visible = false;
            Frame602.Visible = true;
            TextBox602.ReadOnly = false;
            TextBox603.ReadOnly = false;
            Text608.Visible = false;
            Text609.Visible = false;
            Text610.Visible = false;
            // =====

            Label602.Text = "";
            Label603.Text = "";
            Label608.Text = "";

            Label621.Visible = false;
            Label622.Visible = false;
            Label623.Visible = false;
            Label635.Visible = false;
            Label636.Visible = false;
            Label637.Visible = false;
        }

        private void OptionButton402_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!bFormInitialised)
                return;

            if (!((RadioButton)eventSender).Checked)
                return;

            Label604.Text = Resources.str00904; // "???. ?????? ?? KK:"
            Label606.Text = Resources.str00906; // "???????? KK ?? DER:"

            Label621.Text = Resources.str02621;
            Label625.Text = Resources.str00905; // "???. ?????? ?? KK:"

            Label1001_15.Text = Resources.str40101;

            CheckBox401.Enabled = false;
            //TextBox505.ReadOnly = false;

            TextBox602.ReadOnly = true;
            TextBox603.ReadOnly = true;

            Frame601.Visible = true;
            Frame602.Visible = false;

            Text608.Visible = true;
            Text609.Visible = true;
            Text610.Visible = true;

            Label621.Visible = true;
            Label622.Visible = true;
            Label623.Visible = true;
            Label635.Visible = true;
            Label636.Visible = true;
            Label637.Visible = true;
        }

        private void OptionButton403_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!bFormInitialised)
                return;

            if (!((RadioButton)eventSender).Checked)
                return;

            Label604.Text = Resources.str15435; // "???. ?????? ?????????:"
            Label606.Text = Resources.str15437; // "???????? ??? ?? DER:"

            Label621.Text = Resources.str15436; // "???. ?????? ?????????:"

            Label1001_15.Text = Resources.str40101;

            CheckBox401.Enabled = false;
            //TextBox505.ReadOnly = true;

            TextBox602.ReadOnly = true;
            TextBox603.ReadOnly = true;

            Frame601.Visible = false;
            Frame602.Visible = true;

            Text608.Visible = true;
            Text609.Visible = true;
            Text610.Visible = true;

            Label621.Visible = true;
            Label622.Visible = true;
            Label623.Visible = true;
            Label635.Visible = true;
            Label636.Visible = true;
            Label637.Visible = true;
        }

        private void OptionButton703_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (((RadioButton)eventSender).Checked)
            {
                int I = 0;
                ComboBox701.Visible = true;
                CheckBox701.Visible = true;
                // =====
                Label701.Text = Resources.str15375; // "???/FIX/WPT"
                Label701.Visible = true;
                Label702.Visible = true;
                Label706.Visible = false;
                Label709.Visible = false;
                Label708.Visible = false;
                Label58.Visible = false;

                Label705.Visible = false;
                Label707.Visible = false;

                Label710.Visible = false;
                Label712.Visible = false;

                TextBox703.Visible = false;
                TextBox701.Visible = false;
                //     TextBox702.Visible = False
                //     Label703.Visible = False
                //     Label704.Visible = False
                TextBox702.ReadOnly = true;

                ToolTip1.SetToolTip(TextBox702, "");
                TextBox702.Tag = "a";


                Label711.Visible = false;
                ComboBox702.Visible = false;
                ComboBox703.Visible = false;

                // ==
                CheckBox906.Visible = true;
                OptionButton806.Visible = true;
                OptionButton802.Visible = false;
                OptionButton805.Visible = false;
                OptionButton802.Checked = false;
                OptionButton805.Checked = false;

                Label802.Visible = true;
                TextBox802.Visible = true;
                // ==

                ComboBox701.Items.Clear();

                for (I = 0; I <= FixWPT.GetUpperBound(0); I++)
                    ComboBox701.Items.Add(FixWPT[I].Name);

                ComboBox701.SelectedIndex = 0;
                //ComboBox701_SelectedIndexChanged(ComboBox701, new System.EventArgs());
            }
        }


        // TRANSMISSINGCOMMENT: Method OptionButton702_CheckedChanged
        private void OptionButton702_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!bFormInitialised)
                return;

            if (((RadioButton)eventSender).Checked)
            {
                Label701.Text = Resources.str15438; // "???"
                Label701.Visible = true;
                Label702.Visible = true;
                Label706.Visible = true;
                Label709.Visible = true;
                Label708.Visible = true;
                Label58.Visible = true;
                Label705.Visible = true;
                Label707.Visible = true;
                Label710.Visible = true;
                Label711.Visible = true;
                Label712.Visible = true;

                ComboBox701.Visible = true;
                TextBox703.Visible = true;
                //     TextBox702.Visible = True
                //     Label704.Visible = True
                //     Label703.Visible = True
                TextBox702.ReadOnly = false;

                ToolTip1.SetToolTip(TextBox702, RecommendStr);
                TextBox702.Tag = "a";
                TextBox701.Visible = true;
                CheckBox701.Visible = false;

                ComboBox702.Visible = true;
                ComboBox703.Visible = true;
                ComboBox703.Enabled = GlobalVars.WPTList.Length > 0;

                // =================================
                CheckBox906.Visible = false;
                CheckBox906.Checked = false;

                OptionButton806.Visible = false;
                CheckBox906.Checked = false;
                OptionButton802.Visible = true;
                OptionButton805.Visible = true;

                Label802.Visible = false;
                TextBox802.Visible = false;
                // ====================================
                //ComboBox701.Items = FixAngl;
                ComboBox701.Items.Clear();

                for (int I = 0; I < FixAngl.Length; I++)
                    ComboBox701.Items.Add(FixAngl[I].Name);

                ComboBox701.SelectedIndex = 0;
                //ComboBox701_SelectedIndexChanged(ComboBox701, new System.EventArgs());
            }
        }

        private void OptionButton701_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!bFormInitialised)
                return;

            if (((RadioButton)eventSender).Checked)
            {
                TextBox702.ReadOnly = false;

                ToolTip1.SetToolTip(TextBox702, "");
                TextBox702.Tag = "a";

                Label701.Visible = false;
                Label702.Visible = false;
                Label706.Visible = false;
                Label709.Visible = false;
                Label708.Visible = false;
                Label58.Visible = false;
                Label705.Visible = false;
                Label707.Visible = false;
                Label710.Visible = false;
                Label711.Visible = false;
                Label712.Visible = false;

                ComboBox702.Visible = false;
                ComboBox703.Visible = false;

                ComboBox701.Visible = false;
                TextBox703.Visible = false;

                TextBox701.Visible = false;
                CheckBox701.Visible = false;
                OptionButton802.Visible = false;
                TextBox702_Validating(TextBox702, new System.ComponentModel.CancelEventArgs());
            }
        }

        private void OptionButton801_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!bFormInitialised)
                return;

            if (((RadioButton)eventSender).Checked)
                ApplayOptions();
        }

        private void OptionButton803_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!bFormInitialised)
                return;

            if (((RadioButton)eventSender).Checked)
                ApplayOptions();
        }

        private void PrevBtn_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            // LogStr LoadResString(15439)                          '"<-?????? ???????? ????"
            screenCapture.Delete();
            switch (MultiPage1.SelectedIndex)
            {
                case 2:
                    if (GlobalVars.StraightAreaElem != null)
                    {
                        if (GlobalVars.StraightAreaElem is IGroupElement)
                            for (int i = 0; i < (GlobalVars.StraightAreaElem as IGroupElement).ElementCount; i++)
                                Functions.DeleteGraphicsElement((GlobalVars.StraightAreaElem as IGroupElement).Element[i]);
                        else
                            Functions.DeleteGraphicsElement(GlobalVars.StraightAreaElem);
                        GlobalVars.StraightAreaElem = null;
                    }

                    Functions.DeleteGraphicsElement(GlobalVars.StrTrackElem);

                    GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

                    if (ReportsFrm.Visible)
                        ReportsFrm.Hide();

                    GlobalVars.StrTrackElem = null;
                    //Functions.RefreshCommandBar(_mTool, 2);

                    GlobalVars.RModel = 0.0;
                    TextBox003.Tag = "a";
                    TextBox003_Validating(TextBox003, new System.ComponentModel.CancelEventArgs());

                    break;
                case 3:
                    CheckBox301.Checked = false;
                    OkBtn.Enabled = false;
                    break;
                case 4:
                    CheckBox401.Checked = false;
                    break;
                case 6:

                    Functions.DeleteGraphicsElement(GlobalVars.FIXElem);
                    GlobalVars.FIXElem = null;
                    GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

                    //Functions.RefreshCommandBar(_mTool, 128);

                    break;
                case 7:
                    OkBtn.Enabled = false;

                    Functions.DeleteGraphicsElement(GlobalVars.TurnAreaElem);
                    GlobalVars.TurnAreaElem = null;

                    Functions.DeleteGraphicsElement(GlobalVars.PrimElem);
                    GlobalVars.PrimElem = null;

                    Functions.DeleteGraphicsElement(GlobalVars.SecRElem);
                    GlobalVars.SecRElem = null;

                    Functions.DeleteGraphicsElement(GlobalVars.SecLElem);
                    GlobalVars.SecLElem = null;

                    Functions.DeleteGraphicsElement(GlobalVars.NomTrackElem);
                    GlobalVars.NomTrackElem = null;

                    Functions.DeleteGraphicsElement(GlobalVars.KKElem);
                    GlobalVars.KKElem = null;

                    Functions.DeleteGraphicsElement(GlobalVars.K1K1Elem);
                    GlobalVars.K1K1Elem = null;

                    if (GlobalVars.StraightAreaElem != null)
                    {
                        if (GlobalVars.StraightAreaElem is IGroupElement)
                            for (int i = 0; i < (GlobalVars.StraightAreaElem as IGroupElement).ElementCount; i++)
                                Functions.DeleteGraphicsElement((GlobalVars.StraightAreaElem as IGroupElement).Element[i]);
                        else
                            Functions.DeleteGraphicsElement(GlobalVars.StraightAreaElem);
                        GlobalVars.StraightAreaElem = null;
                    }

                    //pGroupElement = ((ESRI.ArcGIS.Carto.IGroupElement)(GlobalVars.StraightAreaElem));
                    //pGraphics.AddElement(pGroupElement.get_Element(0), 0);
                    //pGraphics.AddElement(pGroupElement.get_Element(1), 0);
                    //pGraphics.AddElement(pGroupElement.get_Element(2), 0);
                    //pGraphics.AddElement(pGroupElement.get_Element(3), 0);

                    //pGroupElement.get_Element(0).Locked = true;
                    //pGroupElement.get_Element(1).Locked = true;
                    //pGroupElement.get_Element(2).Locked = true;
                    //pGroupElement.get_Element(3).Locked = true;

                    Functions.DeleteGraphicsElement(GlobalVars.FIXElem);
                    GlobalVars.FIXElem = null;

                    GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

                    //Functions.RefreshCommandBar(_mTool, 58);

                    if (OptionButton403.Checked)
                    {
                        CurrPage = MultiPage1.SelectedIndex - 1;
                        MultiPage1.SelectedIndex = CurrPage;
                    }
                    break;
                case 8:
                    CheckState = true;
                    break;
                case 10:
                    if (OptionButton701.Checked || (OptionButton703.Checked && (TurnDirector.TypeCode != eNavaidType.VOR) && (TurnDirector.TypeCode != eNavaidType.NDB)))
                    {
                        CurrPage = MultiPage1.SelectedIndex - 2;
                        MultiPage1.SelectedIndex = CurrPage;
                        CheckState = true;
                    }
                    else
                        CheckState = false;

                    OkBtn.Enabled = false;

                    if (OptionButton702.Checked)
                        Functions.DeleteGraphicsElement(GlobalVars.TerminationFIXElem);

                    break;
            }

            CurrPage = MultiPage1.SelectedIndex - 1;
            MultiPage1.SelectedIndex = CurrPage;

            //  2007
            FocusStepCaption((MultiPage1.SelectedIndex));

            NextBtn.Enabled = MultiPage1.SelectedIndex < MultiPage1.TabPages.Count - 1;
            PrevBtn.Enabled = MultiPage1.SelectedIndex > 0;

            this.HelpContextID = 6000 + 100 * (MultiPage1.SelectedIndex + 1);
            this.Activate();
        }

        private void SpinButton201_ValueChanged(System.Object sender, System.EventArgs e)
        {
            Label212.Text = "";

            if (!SpinButton201.Enabled || MultiPage1.SelectedIndex < 1)
                return;

            IPointCollection mPoly;
            ObstacleData DetObs = new ObstacleData();
            DetObs.Owner = -1;

            int K = ComboBox101.SelectedIndex;
            double Angle = NativeMethods.Modulus((double)SpinButton201.Value);

            if (FrontList[K].TypeCode == eNavaidType.VOR && FrontList[K].Front)
                TextBox201.Text = NativeMethods.Modulus(Angle + 180.0).ToString();
            else
                TextBox201.Text = NativeMethods.Modulus(Angle).ToString();

            DepDir = Functions.Azt2Dir(DER.pPtGeo[eRWY.PtDER], (double)SpinButton201.Value + FrontList[K].MagVar);
            DerShift = CalcShift();

            if (DerShift > 0)
            {
                TextBox205.Text = Functions.ConvertHeight(DerShift, eRoundMode.NEAREST).ToString();
                Label212.Text = Resources.str15440;     // "???????"
            }
            else
            {
                TextBox205.Text = Functions.ConvertHeight(-DerShift, eRoundMode.NEAREST).ToString();
                Label212.Text = Resources.str15441;     // "??????"
            }

            CreateStraightZones(PANS_OPS_DataBase.dpPDG_Nom.Value, -100, false);
            CurrPDG = Functions.CalcLocalPDG(oZNRList.Parts, out iDominicObst);

            if (iDominicObst > -1)
            {
                DetObs = oZNRList.Parts[iDominicObst];
                TextBox306.Text = oZNRList.Obstacles[DetObs.Owner].UnicalName;
            }
            else
            {
                DetObs.Owner = -1;
                TextBox306.Text = Resources.str39014;
            }

            TextBox202.Text = Math.Round(100.0 * CurrPDG + 0.049999, 1).ToString();

            int I = CreateStraightZones(CurrPDG, -100.0, true);
            if (I > -1)
                TextBox305.Text = oZNRList.Obstacles[oZNRList.Parts[I].Owner].UnicalName;
            else
                TextBox305.Text = Resources.str39014;

            drPDGMax = Functions.dPDGMax(oZNRList.Parts, CurrPDG, out idPDGMax);
            //Functions.CalcObstaclesReqTNAH(oZNRList, CurrPDG);

            TextBox203.Text = Functions.ConvertHeight(drPDGMax * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value, eRoundMode.CEIL).ToString();
            ReportsFrm.FillPage1(oZNRList, CurrPDG, DetObs, ComboBox001.Text, 3);
            Report = true;
            //     ReportsFrm.FillPrimaries PtInList

            mPoly = new Polyline();
            mPoly.AddPoint(PtDerShift);
            mPoly.AddPoint(Functions.PointAlongPlane(PtDerShift, DepDir, GlobalVars.RModel));

            Functions.DeleteGraphicsElement(GlobalVars.StrTrackElem);
            GlobalVars.StrTrackElem = Functions.DrawPolyline(mPoly, GlobalVars.StrTrackElemColor, 1.0, false);

            if (GlobalVars.ButtonControl5State)
            {
                pGraphics.AddElement(GlobalVars.StrTrackElem, 0);
                GlobalVars.StrTrackElem.Locked = true;
                GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }

            //Functions.RefreshCommandBar(_mTool, 2);
        }

        private void SpinButton501_Change(System.Object sender, System.EventArgs e)
        {
            if (!SpinButton501.Enabled)
                return;

            int NewAdjust, K;
            double DepDist, CircleR;
            Interval mIntr;

            AdjustDir = 1 - 2 * ComboBox501.SelectedIndex;
            NewAdjust = System.Convert.ToInt32(SpinButton501.Value * AdjustDir);
            if (NewAdjust == TrackAdjust)
                return;

            TrackAdjust = NewAdjust;
            double NewPDG = PANS_OPS_DataBase.dpPDG_Nom.Value;

            if (OptionButton403.Checked)
            {
                // ============================================================
                DepDist = CalcRNELocalRange(out CurrPDG, TrackAdjust, out CircleR, false);
                K = ComboBox101.SelectedIndex;

                //     Range = Functions.Point2LineDistancePrj(FrontList(K).pPtPrj, DER.pPtPrj(ptEnd), DepDir + 90.0)
                //     hTurn = dpH_abv_DER.Value + dpOv_Nav_PDG.Value * DepDist + DER.pPtPrj(ptEnd).Z - FrontList(K).pPtPrj.Z

                //     If FrontList(K).TypeCode = 0 Then
                //         CircleR = hTurn * System.Math.Tan(DegToRadValue * VOR.ConeAngle)
                //     Else
                //         CircleR = hTurn * System.Math.Tan(DegToRadValue * NDB.ConeAngle)
                //     End If

                hMinTurn = System.Math.Round(DepDist * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value - 0.4999);
                hMaxTurn = System.Math.Round((DepDist + 2.0 * CircleR) * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value + 0.4999);
            }
            else    // =============================================================
            {
                mIntr = CalcLocalRange(NewPDG * 0.99, out CurrPDG, TrackAdjust, true);
                hMinTurn = System.Math.Round(mIntr.Left * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value - 0.4999);
                hMaxTurn = System.Math.Round(mIntr.Right * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value + 0.4999);

                Label506.Width = 229;
                //if (PrevDet.ID != Resources.str39014)

                if (PrevDet.Owner != -1)
                {
                    Label506.Text = Resources.str15442 + "\r\n" +
                        Resources.str15444 + Functions.ConvertDistance(PrevDet.Dist, eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit + ";" + "\r\n" +
                        Resources.str15446 + Functions.DegToStr(System.Math.Round(PrevDet.CourseAdjust));
                }
                else
                    Label506.Text = Resources.str15447 + Math.Round(100.0 * CurrPDG, 1).ToString() + Resources.str15448;

                Label506.Width = 229;   // ============================================================
            }

            CreateStraightZones(CurrPDG, TrackAdjust, true);
            PrevPDG = TextBox505.Text;
            // ============================================================

            if (hMaxTurn < hMinTurn)
                hMaxTurn = hMinTurn;

            TextBox502.Text = Functions.ConvertHeight(hMaxTurn, eRoundMode.NEAREST).ToString();
            TextBox501.Text = Functions.ConvertHeight(hMinTurn, eRoundMode.NEAREST).ToString();

            if (hMinTurn < PANS_OPS_DataBase.dpGui_Ar1.Value)
                TextBox501.ForeColor = System.Drawing.Color.FromArgb(0XFF0000);
            else
                TextBox501.ForeColor = System.Drawing.Color.FromArgb(0);

            if (hMaxTurn < PANS_OPS_DataBase.dpGui_Ar1.Value)
                TextBox502.ForeColor = System.Drawing.Color.FromArgb(0XFF0000);
            else
                TextBox502.ForeColor = System.Drawing.Color.FromArgb(0);

            // =============================================================
            IPointCollection mPoly = new Polyline();
            mPoly.AddPoint(PtDerShift);
            mPoly.AddPoint(Functions.PointAlongPlane(PtDerShift, DepDir, GlobalVars.RModel));

            Functions.DeleteGraphicsElement(GlobalVars.StrTrackElem);
            GlobalVars.StrTrackElem = Functions.DrawPolyline(mPoly, GlobalVars.StrTrackElemColor, 1.0, false);

            if (GlobalVars.ButtonControl5State)
            {
                pGraphics.AddElement(GlobalVars.StrTrackElem, 0);
                GlobalVars.StrTrackElem.Locked = true;
            }
            GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            //Functions.RefreshCommandBar(_mTool, 2);
            // ===============================================================
            TextBox505.Text = Math.Round(100.0 * CurrPDG, 1).ToString();
            TextBox503.Text = NativeMethods.Modulus(Functions.RoundAngle(Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], DepDir) - GlobalVars.CurrADHP.MagVar)).ToString();
            // ============================================================
            if (double.Parse(TextBox602.Text) < double.Parse(TextBox501.Text))
                TextBox602.Text = TextBox501.Text;

            if (double.Parse(TextBox602.Text) > double.Parse(TextBox502.Text))
                TextBox602.Text = TextBox502.Text;

            Label608.Text = Resources.str15380 + TextBox501.Text + " " + Label513.Text +
                            Resources.str15162 + " " + TextBox502.Text + " " + Label513.Text;
            ToolTip1.SetToolTip(TextBox602, Label608.Text);
        }

        private void UpdateIntervals(bool ChangeCurDir)// = false
        {
            if (MultiPage1.SelectedIndex < 5)
                return;

            //     CurrPDG = CDbl(TextBox505.Text) * 0.01
            //     hTurn = CDbl(TextBox602.Text)

            double Snap = double.Parse(TextBox703.Text);
            double tStabl = double.Parse(TextBox701.Text);
            double fTASl = Functions.IAS2TAS(fIAS, TurnFixPnt.Z, GlobalVars.CurrADHP.ISAtC);
            double VTotal = fTASl + GlobalVars.CurrADHP.WindSpeed;

            double L0;
            if (OptionButton401.Checked)
                L0 = (TurnFixPnt.Z - PANS_OPS_DataBase.dpOIS_abv_DER.Value) / CurrPDG;
            else
                L0 = Functions.Point2LineDistancePrj(DER.pPtPrj[eRWY.PtDER], KKFixMax.FromPoint, DepDir + 90.0);

            double TurnR = Functions.Bank2Radius(fBankAngle, fTASl);
            // ==========================================
            double ddr = TurnR / (System.Math.Tan(GlobalVars.DegToRadValue * (180.0 - Snap) * 0.5));
            double dr = PANS_OPS_DataBase.dpT_Gui_dist.Value + ddr;
            double lMinDR = VTotal * tStabl * 0.277777777777778 + ddr;
            MinDR = System.Math.Round(lMinDR - ddr);
            ToolTip1.SetToolTip(TextBox701, Resources.str15452 + Functions.ConvertDistance(MinDR, eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit); // "??????????? ????? DR = "
                                                                                                                                                                                                      // ==========================================

            IPoint ptCnt = Functions.PointAlongPlane(TurnFixPnt, DepDir + 90.0 * TurnDir, TurnR);
            double bAz = Functions.ReturnAngleInDegrees(TurnDirector.pPtPrj, ptCnt);
            double d = Functions.ReturnDistanceInMeters(TurnDirector.pPtPrj, ptCnt);

            double r_ = System.Math.Sqrt(dr * dr + TurnR * TurnR);

            double delta = GlobalVars.RadToDegValue * (System.Math.Atan(TurnR / dr));
            double alpha = Snap - TurnDir * delta;

            double xMin, xMax;
            double fTmp = r_ * System.Math.Sin(GlobalVars.DegToRadValue * alpha) / d;
            if (fTmp > 1.0)
                xMin = 90.0;
            else if (fTmp < -1.0)
                xMin = -90.0;
            else
                xMin = GlobalVars.RadToDegValue * Functions.ArcSin(fTmp);

            alpha = Snap + TurnDir * delta;
            fTmp = r_ * System.Math.Sin(GlobalVars.DegToRadValue * alpha) / d;
            if (fTmp > 1.0)
                xMax = 90.0;
            else if (fTmp < -1.0)
                xMax = -90.0;
            else
                xMax = GlobalVars.RadToDegValue * Functions.ArcSin(fTmp);

            Interval[] Intervals = new Interval[4];

            Intervals[0].Left = NativeMethods.Modulus(Functions.Dir2Azt(ptCnt, bAz + xMin) - GlobalVars.CurrADHP.MagVar, 360.0);
            Intervals[1].Right = NativeMethods.Modulus(Functions.Dir2Azt(ptCnt, bAz - xMax) - GlobalVars.CurrADHP.MagVar, 360.0);
            Intervals[2].Left = NativeMethods.Modulus(Functions.Dir2Azt(ptCnt, bAz + xMax) + 180.0 - GlobalVars.CurrADHP.MagVar, 360.0);
            Intervals[3].Right = NativeMethods.Modulus(Functions.Dir2Azt(ptCnt, bAz - xMin) + 180.0 - GlobalVars.CurrADHP.MagVar, 360.0);

            r_ = System.Math.Sqrt(lMinDR * lMinDR + TurnR * TurnR);

            delta = GlobalVars.RadToDegValue * (System.Math.Atan(TurnR / lMinDR));

            alpha = Snap - TurnDir * delta;
            fTmp = r_ * System.Math.Sin(GlobalVars.DegToRadValue * alpha) / d;
            if (fTmp > 1.0)
                xMin = 90.0;
            else if (fTmp < -1.0)
                xMin = -90.0;
            else
                xMin = GlobalVars.RadToDegValue * Functions.ArcSin(fTmp);

            alpha = Snap + TurnDir * delta;
            fTmp = r_ * System.Math.Sin(GlobalVars.DegToRadValue * alpha) / d;
            if (fTmp > 1.0)
                xMax = 90.0;
            else if (fTmp < -1.0)
                xMax = -90.0;
            else
                xMax = GlobalVars.RadToDegValue * Functions.ArcSin(fTmp);

            Intervals[0].Right = NativeMethods.Modulus(Functions.Dir2Azt(ptCnt, bAz + xMin) - GlobalVars.CurrADHP.MagVar, 360.0);
            Intervals[1].Left = NativeMethods.Modulus(Functions.Dir2Azt(ptCnt, bAz - xMax) - GlobalVars.CurrADHP.MagVar, 360.0);
            Intervals[2].Right = NativeMethods.Modulus(Functions.Dir2Azt(ptCnt, bAz + xMax) + 180.0 - GlobalVars.CurrADHP.MagVar, 360.0);
            Intervals[3].Left = NativeMethods.Modulus(Functions.Dir2Azt(ptCnt, bAz - xMin) + 180.0 - GlobalVars.CurrADHP.MagVar, 360.0);

            int i, j = 0, n = Intervals.GetUpperBound(0);
            // SortIntervals Intervals

            while (j < n)
            {
                if (Functions.SubtractAngles(Intervals[j].Right, Intervals[j + 1].Left) <= 1.0)
                {
                    Intervals[j].Right = Intervals[j + 1].Right;
                    for (i = j + 1; i <= n - 1; i++)
                        Intervals[i] = Intervals[i + 1];

                    n--;
                    if (n > -1)
                        System.Array.Resize<Interval>(ref Intervals, n + 1);
                    else
                        Intervals = new Interval[0];
                }
                else
                    j++;
            }

            n = Intervals.GetUpperBound(0);

            if (n > 0)
            {
                if (Functions.SubtractAngles(Intervals[0].Left, Intervals[n].Right) <= 1.0)
                {
                    //	If Intervals(0).Left = Intervals(N).Right Then
                    Intervals[0].Left = Intervals[n].Left;
                    n--;
                    System.Array.Resize<Interval>(ref Intervals, n + 1);
                }
            }

            Functions.SortIntervals(Intervals, false);

            RecommendStr = Resources.str15454;
            string tmpStr = RecommendStr + "\r\n";

            for (i = 0; i <= n; i++)
            {
                if (Functions.SubtractAngles(System.Math.Round(Intervals[i].Left), System.Math.Round(Intervals[i].Right)) <= GlobalVars.degEps)
                {
                    RecommendStr = RecommendStr + " " + Functions.DegToStr(System.Math.Round(Intervals[i].Left));
                    tmpStr = tmpStr + " " + Functions.DegToStr(System.Math.Round(Intervals[i].Left));
                    if ((i == 0) && ChangeCurDir)
                        TextBox702.Text = System.Math.Round(Intervals[0].Left).ToString();
                }
                else
                {
                    RecommendStr = RecommendStr + Resources.str15455 + " " + Functions.DegToStr(System.Math.Round(Intervals[i].Left + 0.4999)) + Resources.str15456 + Functions.DegToStr(System.Math.Round(Intervals[i].Right - 0.4999)); // "?? ""? ?? "
                    tmpStr = tmpStr + Resources.str15455 + " " + Functions.DegToStr(System.Math.Round(Intervals[i].Left + 0.4999)) + Resources.str15456 + Functions.DegToStr(System.Math.Round(Intervals[i].Right - 0.4999)); // "?? ""? ?? "
                    if ((i == 0) && ChangeCurDir)
                        TextBox702.Text = System.Math.Round(Intervals[0].Left + 0.4999).ToString();
                }

                if (i != n)
                {
                    RecommendStr = RecommendStr + "; ";
                    tmpStr = tmpStr + "\r\n";
                }
            }

            //     I = Label709.Width
            Label709.Text = tmpStr;
            //     Label709.Width = I

            //     TextBox702.Text = CStr(System.Math.Round(Intervals(0).Left + 0.4999))

            if (OptionButton702.Checked)
                ToolTip1.SetToolTip(TextBox702, RecommendStr);
            else
                ToolTip1.SetToolTip(TextBox702, "");

            //     TextBox702_Validate False
        }

        private void UpdateIntervals()
        {
            UpdateIntervals(false);
        }

        private int UpdateToNavCurs(double OutAzt)
        {
            double fTASl = Functions.IAS2TAS(fIAS, TurnFixPnt.Z, GlobalVars.CurrADHP.ISAtC);
            double TurnR = Functions.Bank2Radius(fBankAngle, fTASl);
            double dDir, InterAngle = double.Parse(TextBox703.Text);

            MPtCollection = Functions.CalcTouchByFixDir(TurnFixPnt, TurnDirector.pPtPrj, TurnR, DepDir, ref OutAzt, TurnDir, InterAngle, out dDir, out FlyBy);

            //if (dDir > GlobalVars.RModel)
            //{
            //	MessageBox.Show(Resources.str15457, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //	return -1;
            //}

            IClone pSource = ((ESRI.ArcGIS.esriSystem.IClone)(UnitedPolygon));
            IPointCollection TmpPoly, pRPolygon = ((ESRI.ArcGIS.Geometry.IPointCollection)(pSource.Clone()));

            if (!OptionButton401.Checked) // ?? ??????
            {
                TmpPoly = new Polyline();
                TmpPoly.AddPoint(Functions.PointAlongPlane(KK.ToPoint, DepDir - 90.0, GlobalVars.RModel));
                TmpPoly.AddPoint(Functions.PointAlongPlane(KK.FromPoint, DepDir + 90.0, GlobalVars.RModel));

                Functions.CutPoly(ref pRPolygon, TmpPoly, -1);
                pRPolygon = Functions.ReArrangePolygon(pRPolygon, DER.pPtPrj[eRWY.PtDER], RWYDir);
            }

            IPoint ptTmp, ptCurr = MPtCollection.Point[MPtCollection.PointCount - 1];
            TracPoly = (IPointCollection)Functions.CalcTrajectoryFromMultiPoint(MPtCollection);
            IProximityOperator pProxi;

            if (ComboBox703.SelectedIndex == 1)
            {
                if (Functions.SideDef(ptCurr, OutAzt + 90.0, WPt702.pPtPrj) > 0)
                    TracPoly.AddPoint(WPt702.pPtPrj);
                else
                {
                    pProxi = ((ESRI.ArcGIS.Geometry.IProximityOperator)(pCircle));
                    if (pProxi.ReturnDistance(ptCurr) == 0.0)
                    {
                        Functions.CircleVectorIntersect(GlobalVars.CurrADHP.pPtPrj, GlobalVars.RModel, ptCurr, ptCurr.M, out ptTmp);
                        TracPoly.AddPoint(ptTmp);
                    }
                }
            }
            else
            {
                pProxi = ((ESRI.ArcGIS.Geometry.IProximityOperator)(pCircle));
                if (pProxi.ReturnDistance(ptCurr) == 0.0)
                {
                    Functions.CircleVectorIntersect(GlobalVars.CurrADHP.pPtPrj, GlobalVars.RModel, ptCurr, ptCurr.M, out ptTmp);
                    TracPoly.AddPoint(ptTmp);
                }
            }

            Label710.Text = Functions.ConvertDistance(dDir, eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;

            if (dDir > PANS_OPS_DataBase.dpT_Gui_dist.Value || dDir < MinDR)
                Label710.ForeColor = System.Drawing.Color.Red;
            else
                Label710.ForeColor = System.Drawing.Color.Black;

            OutAzt = MPtCollection.Point[1].M;

            BasePoints = Functions.CreateBasePoints(pRPolygon, K1K1, DepDir, TurnDir);
            ZNR_Poly = Functions.CreateBasePoints(UnitedPolygon, KK, DepDir, TurnDir);

            ITopologicalOperator2 pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)ZNR_Poly;
            pTopoOper.IsKnownSimple_2 = false;
            pTopoOper.Simplify();

            TurnArea = CreateTurnArea(3.6 * PANS_OPS_DataBase.dpWind_Speed.Value, TurnR, fTASl, ref DepDir, ref OutAzt, TurnDir, BasePoints);

            int i;
            if (OptionButton401.Checked)
            {
                if (TurnDir > 0)
                {
                    if (TurnArea.PointCount > 0)
                    {
                        TurnArea.AddPoint(KK.FromPoint, 0);
                        TurnArea.AddPoint(KK.ToPoint, 0);
                    }
                    else
                    {
                        TurnArea.AddPoint(KK.ToPoint);
                        TurnArea.AddPoint(KK.FromPoint);
                    }
                }
                else
                {
                    if (TurnArea.PointCount > 0)
                    {
                        TurnArea.AddPoint(KK.ToPoint, 0);
                        TurnArea.AddPoint(KK.FromPoint, 0);
                    }
                    else
                    {
                        TurnArea.AddPoint(KK.FromPoint);
                        TurnArea.AddPoint(KK.ToPoint);
                    }
                }

                if (CheckBox401.Checked)
                {
                    if (TurnDir > 0)
                        TurnArea.AddFirstPoint(UnitedPolygon.Point[UnitedPolygon.PointCount - 2]);
                    else
                        TurnArea.AddFirstPoint(UnitedPolygon.Point[1]);
                }
            }
            else
            {
                int k = BasePoints.PointCount;

                if (TurnArea.PointCount > 0)
                {
                    IPointCollection pLPolygon = new ESRI.ArcGIS.Geometry.Polygon();
                    for (i = 1; i < k; i++)
                    {
                        int Side = Functions.SideDef(DER.pPtPrj[eRWY.PtDER], DepDir, BasePoints.Point[i]);
                        if (Side == TurnDir)
                            pLPolygon.AddPoint(BasePoints.Point[i]);
                    }
                    pLPolygon.AddPointCollection(TurnArea);
                    TurnArea = pLPolygon;
                    //ptCurr = TurnArea.Point[TurnArea.PointCount - 1]
                }
                else
                {
                    ptCurr = BasePoints.Point[0];
                    for (i = 1; i < k; i++)
                    {
                        int Side = Functions.SideDef(DER.pPtPrj[eRWY.PtDER], DepDir, BasePoints.Point[i]);
                        if (Side == TurnDir)
                            TurnArea.AddPoint(BasePoints.Point[i]);
                    }
                    TurnArea.AddPoint(ptCurr);
                }
            }

            IPointCollection tmpPoly;
            if (OptionButton401.Checked)
                tmpPoly = ZNR_Poly;
            else
                tmpPoly = pFIXPoly;
            ptCurr = TurnArea.Point[TurnArea.PointCount - 1];

            double maxDist = 0.0, AztCurr = OutAzt + PANS_OPS_DataBase.dpafTrn_OSplay.Value * TurnDir;
            int n = tmpPoly.PointCount, iMax = 0;
            for (i = 0; i < n; i++)
            {
                double currDist = Functions.Point2LineDistancePrj(tmpPoly.Point[i], ptCurr, AztCurr);
                if (maxDist < currDist)
                {
                    iMax = i;
                    maxDist = currDist;
                }
            }

            ptTmp = tmpPoly.Point[iMax];
            TurnArea.AddFirstPoint(ptTmp);

            tmpPoly = new ESRI.ArcGIS.Geometry.Polygon();
            tmpPoly.AddPoint(ptTmp);
            tmpPoly.AddPoint(ptCurr);

            PrimPoly = new ESRI.ArcGIS.Geometry.Polygon();
            SecL = new ESRI.ArcGIS.Geometry.Polygon();
            SecR = new ESRI.ArcGIS.Geometry.Polygon();

            DirToNav = MPtCollection.Point[MPtCollection.PointCount - 1].M;

            Functions.CreateNavaidZone(TurnDirector.pPtPrj, DirToNav, TurnDirector.TypeCode, 2.0, SecL, SecR, PrimPoly);
            IPointCollection NewPoly = CalcAreaIntersection(TurnArea, OutAzt, DirToNav, TurnDir);

            pTopoOper = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(BasePoints));
            pTopoOper.IsKnownSimple_2 = false;
            pTopoOper.Simplify();

            pTopoOper = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(NewPoly));
            pTopoOper.IsKnownSimple_2 = false;
            pTopoOper.Simplify();

            BaseArea = Functions.RemoveAgnails(Functions.RemoveHoles((IPolygon)pTopoOper.Union(BasePoints)));

            if (OptionButton401.Checked)
            {
                pTopoOper = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(BaseArea));
                BaseArea = (IPointCollection)pTopoOper.Difference(ZNR_Poly);
            }

            BaseArea = (IPointCollection)Functions.PolygonIntersection(pCircle, BaseArea);

            Functions.DeleteGraphicsElement(GlobalVars.KKElem);
            Functions.DeleteGraphicsElement(GlobalVars.K1K1Elem);
            Functions.DeleteGraphicsElement(GlobalVars.TurnAreaElem);
            Functions.DeleteGraphicsElement(GlobalVars.PrimElem);
            Functions.DeleteGraphicsElement(GlobalVars.SecRElem);
            Functions.DeleteGraphicsElement(GlobalVars.SecLElem);
            Functions.DeleteGraphicsElement(GlobalVars.NomTrackElem);

            IPointCollection DrawPoly = Functions.PolygonIntersection(pCircle, PrimPoly) as IPointCollection;
            GlobalVars.PrimElem = Functions.DrawPolygon(DrawPoly, GlobalVars.PrimElemColor, esriSimpleFillStyle.esriSFSNull, false);

            DrawPoly = Functions.PolygonIntersection(pCircle, SecL) as IPointCollection;
            GlobalVars.SecLElem = Functions.DrawPolygon(DrawPoly, GlobalVars.SecLElemColor, esriSimpleFillStyle.esriSFSNull, false);

            DrawPoly = Functions.PolygonIntersection(pCircle, SecR) as IPointCollection;
            GlobalVars.SecRElem = Functions.DrawPolygon(DrawPoly, GlobalVars.SecRElemColor, esriSimpleFillStyle.esriSFSNull, false);

            GlobalVars.TurnAreaElem = Functions.DrawPolygon(BaseArea, GlobalVars.TurnAreaElemColor, esriSimpleFillStyle.esriSFSNull, false);

            GlobalVars.KKElem = Functions.DrawPolyline(KK, GlobalVars.KKElemColor, 1.0, false);
            GlobalVars.K1K1Elem = Functions.DrawPolyline(K1K1, GlobalVars.K1K1ElemColor, 1.0, false);
            GlobalVars.NomTrackElem = Functions.DrawPolyline(TracPoly, GlobalVars.NomTrackElemColor, 1.0, false);

            if (GlobalVars.ButtonControl3State)
            {
                pGraphics.AddElement(GlobalVars.TurnAreaElem, 0);
                GlobalVars.TurnAreaElem.Locked = true;
            }

            if (GlobalVars.ButtonControl4State)
            {
                pGraphics.AddElement(GlobalVars.PrimElem, 0);
                GlobalVars.PrimElem.Locked = true;
                pGraphics.AddElement(GlobalVars.SecLElem, 0);
                GlobalVars.SecLElem.Locked = true;
                pGraphics.AddElement(GlobalVars.SecRElem, 0);
                GlobalVars.SecRElem.Locked = true;
            }

            if (GlobalVars.ButtonControl6State)
            {
                pGraphics.AddElement(GlobalVars.KKElem, 0);
                GlobalVars.KKElem.Locked = true;
                pGraphics.AddElement(GlobalVars.K1K1Elem, 0);
                GlobalVars.K1K1Elem.Locked = true;
            }

            if (GlobalVars.ButtonControl5State)
            {
                pGraphics.AddElement(GlobalVars.NomTrackElem, 0);
                GlobalVars.NomTrackElem.Locked = true;
            }

            GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            //Functions.RefreshCommandBar(_mTool, 124);

            return 0;
        }

        private int UpdateToFix()
        {
            double fTASl = Functions.IAS2TAS(fIAS, (TurnFixPnt.Z), GlobalVars.CurrADHP.ISAtC);
            double TurnR = Functions.Bank2Radius(fBankAngle, fTASl);

            MPtCollection = Functions.TurnToFixPrj(TurnFixPnt, TurnR, TurnDir, TurnDirector.pPtPrj);

            if (MPtCollection.PointCount == 0)
            {
                MessageBox.Show(Resources.str15458 + "\r\n" +
                    Resources.str15459 + "\r\n" +
                    Resources.str15460 + Functions.ConvertDistance(TurnR, eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit,
                    null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

            OutPt = MPtCollection.Point[1];

            DirToNav = MPtCollection.Point[MPtCollection.PointCount - 1].M;
            OutAzt = MPtCollection.Point[1].M;
            DirCourse = OutAzt;

            TextBox702.Text = System.Math.Round(NativeMethods.Modulus(Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], OutAzt) - GlobalVars.CurrADHP.MagVar, 360.0)).ToString();
            TextBox702.Tag = TextBox702.Text;

            if ((TurnDirector.TypeCode != eNavaidType.VOR) && (TurnDirector.TypeCode != eNavaidType.NDB))
            {
                UpdateToCourse(OutAzt);
                return 0;
            }

            IClone pSource = ((ESRI.ArcGIS.esriSystem.IClone)(UnitedPolygon));
            IPointCollection TmpPoly, pRPolygon = ((ESRI.ArcGIS.Geometry.IPointCollection)(pSource.Clone()));

            if (!(OptionButton401.Checked))
            { // ?? ??????
                TmpPoly = new Polyline();
                TmpPoly.AddPoint(Functions.PointAlongPlane(KK.ToPoint, DepDir - 90.0, GlobalVars.RModel));
                TmpPoly.AddPoint(Functions.PointAlongPlane(KK.FromPoint, DepDir + 90.0, GlobalVars.RModel));

                Functions.CutPoly(ref pRPolygon, TmpPoly, -1);
                pRPolygon = Functions.ReArrangePolygon(pRPolygon, DER.pPtPrj[eRWY.PtDER], RWYDir);
            }

            BasePoints = Functions.CreateBasePoints(pRPolygon, K1K1, DepDir, TurnDir);
            ZNR_Poly = Functions.CreateBasePoints(UnitedPolygon, KK, DepDir, TurnDir);

            ITopologicalOperator2 pTopoOper = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(ZNR_Poly));
            pTopoOper.IsKnownSimple_2 = false;
            pTopoOper.Simplify();

            TurnArea = CreateTurnArea(3.6 * PANS_OPS_DataBase.dpWind_Speed.Value, TurnR, fTASl, ref DepDir, ref OutAzt, TurnDir, BasePoints);
            TracPoly = (IPointCollection)Functions.CalcTrajectoryFromMultiPoint(MPtCollection);
            TracPoly.AddPoint(TurnDirector.pPtPrj);
            MPtCollection.Point[1].M = OutAzt;

            IPoint ptCurr;
            int Side = 0;
            if (OptionButton401.Checked)
            {
                if (TurnDir > 0)
                {
                    if (TurnArea.PointCount > 0)
                    {
                        TurnArea.AddFirstPoint(KK.FromPoint);
                        TurnArea.AddFirstPoint(KK.ToPoint);
                    }
                    else
                    {
                        TurnArea.AddPoint(KK.ToPoint);
                        TurnArea.AddPoint(KK.FromPoint);
                    }
                }
                else
                {
                    if (TurnArea.PointCount > 0)
                    {
                        TurnArea.AddFirstPoint(KK.ToPoint);
                        TurnArea.AddFirstPoint(KK.FromPoint);
                    }
                    else
                    {
                        TurnArea.AddPoint(KK.FromPoint);
                        TurnArea.AddPoint(KK.ToPoint);
                    }
                }

                if (CheckBox401.Checked && OptionButton401.Checked)
                {
                    if (TurnDir > 0)
                    {
                        TurnArea.AddFirstPoint(pPolygon.Point[pPolygon.PointCount - 2]);
                    }
                    else
                    {
                        TurnArea.AddFirstPoint(pPolygon.Point[1]);
                    }
                }
                //     Set ptCur = TurnArea.Point(TurnArea.PointCount - 1)
            }
            else
            {
                int k = BasePoints.PointCount;

                if (TurnArea.PointCount > 0)
                {
                    pRPolygon = new ESRI.ArcGIS.Geometry.Polygon();
                    for (int i = 1; i < k; i++)
                    {
                        Side = Functions.SideDef(DER.pPtPrj[eRWY.PtDER], DepDir, BasePoints.Point[i]);
                        if (Side == TurnDir)
                            pRPolygon.AddPoint(BasePoints.Point[i]);
                    }
                    pRPolygon.AddPointCollection(TurnArea);
                    TurnArea = pRPolygon;
                    //         Set ptCur = TurnArea.Point(TurnArea.PointCount - 1)
                }
                else
                {
                    ptCurr = BasePoints.Point[0];
                    for (int i = 1; i < k; i++)
                    {
                        Side = Functions.SideDef(DER.pPtPrj[eRWY.PtDER], DepDir, BasePoints.Point[i]);
                        if (Side == TurnDir)
                            TurnArea.AddPoint(BasePoints.Point[i]);
                    }
                    TurnArea.AddPoint(ptCurr);
                }
            }

            ptCurr = TurnArea.Point[TurnArea.PointCount - 1];
            double AztCur = OutAzt + PANS_OPS_DataBase.dpafTrn_OSplay.Value * TurnDir;

            if (OptionButton401.Checked)
                TmpPoly = ZNR_Poly;
            else
                TmpPoly = pFIXPoly;

            double maxDist = 0.0;
            int iMax = 0, n = TmpPoly.PointCount;

            for (int i = 0; i < n; i++)
            {
                double currDist = Functions.Point2LineDistancePrj(TmpPoly.Point[i], ptCurr, AztCur);
                if (maxDist < currDist)
                {
                    iMax = i;
                    maxDist = currDist;
                }
            }

            IPoint ptTmp = TmpPoly.Point[iMax];

            TurnArea.AddFirstPoint(ptTmp);

            pSource = (ESRI.ArcGIS.esriSystem.IClone)TurnArea;
            pRPolygon = (ESRI.ArcGIS.Geometry.IPointCollection)pSource.Clone();

            TmpPoly = new ESRI.ArcGIS.Geometry.Polygon();
            TmpPoly.AddPoint(ptTmp);
            TmpPoly.AddPoint(TurnArea.Point[TurnArea.PointCount - 1]);

            // ============= To FIX ============
            CalcJoiningParams(TurnDirector.TypeCode, TurnDir, TurnArea, OutPt, OutAzt);

            IPointCollection NewPoly = ApplayJoining(TurnDirector.TypeCode, TurnDir, TurnArea, (IPolygon)BasePoints, OutPt, OutAzt, TmpPoly);
            double fTmp = Functions.ReturnAngleInDegrees(TurnArea.Point[TurnArea.PointCount - 1], TurnArea.Point[TurnArea.PointCount - 2]);
            // ================
            NewPoly = Functions.RemoveAgnails(NewPoly);

            pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)TmpPoly;
            pTopoOper.IsKnownSimple_2 = false;
            pTopoOper.Simplify();

            pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)pRPolygon;
            pTopoOper.IsKnownSimple_2 = false;
            pTopoOper.Simplify();

            pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)BasePoints;
            pTopoOper.IsKnownSimple_2 = false;
            pTopoOper.Simplify();

            //     Set pTopoOper = NewPoly

            NewPoly = pTopoOper.Union(NewPoly as IGeometry) as IPointCollection;
            pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)NewPoly;

            NewPoly = pTopoOper.Union(pRPolygon as IGeometry) as IPointCollection;
            pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)NewPoly;

            if (NativeMethods.Modulus((OutAzt - DepDir) * TurnDir) > 90.0)
            {
                //     If SideFrom2Angle(DepDir + 90.0 * TurnDir, fTmp) * TurnDir < 0 Then
                NewPoly = pTopoOper.Union(TmpPoly as IGeometry) as IPointCollection;
                pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)NewPoly;
            }

            TmpPoly = new ESRI.ArcGIS.Geometry.Polygon();
            TmpPoly.AddPointCollection(TurnArea);

            pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)TmpPoly;
            pTopoOper.IsKnownSimple_2 = false;
            pTopoOper.Simplify();

            BaseArea = Functions.RemoveHoles(pTopoOper.Union(NewPoly as IGeometry) as IPolygon) as IPointCollection;

            if (OptionButton401.Checked)
            {
                pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)BaseArea;
                BaseArea = pTopoOper.Difference(ZNR_Poly as IGeometry) as IPointCollection;
            }

            //     Set pTopoOper = pCircle
            //     Set BaseArea = pTopoOper.Intersect(BaseArea, esriGeometry2Dimension)
            BaseArea = Functions.PolygonIntersection(pCircle as IPolygon, BaseArea as IPolygon) as IPointCollection;

            Functions.DeleteGraphicsElement(GlobalVars.KKElem);
            Functions.DeleteGraphicsElement(GlobalVars.K1K1Elem);
            Functions.DeleteGraphicsElement(GlobalVars.TurnAreaElem);
            Functions.DeleteGraphicsElement(GlobalVars.PrimElem);
            Functions.DeleteGraphicsElement(GlobalVars.SecRElem);
            Functions.DeleteGraphicsElement(GlobalVars.SecLElem);
            Functions.DeleteGraphicsElement(GlobalVars.NomTrackElem);

            GlobalVars.KKElem = Functions.DrawPolyline(KK, 0, 1.0, false);
            GlobalVars.K1K1Elem = Functions.DrawPolyline(K1K1, 0, 1.0, false);

            IPointCollection DrawPoly = Functions.PolygonIntersection(pCircle as IPolygon, PrimPoly as IPolygon) as IPointCollection;
            GlobalVars.PrimElem = Functions.DrawPolygon(DrawPoly, GlobalVars.PrimElemColor, esriSimpleFillStyle.esriSFSNull, false);

            DrawPoly = Functions.PolygonIntersection(pCircle as IPolygon, SecL as IPolygon) as IPointCollection;
            GlobalVars.SecLElem = Functions.DrawPolygon(DrawPoly, GlobalVars.SecLElemColor, esriSimpleFillStyle.esriSFSNull, false);

            DrawPoly = Functions.PolygonIntersection(pCircle as IPolygon, SecR as IPolygon) as IPointCollection;
            GlobalVars.SecRElem = Functions.DrawPolygon(DrawPoly, GlobalVars.SecRElemColor, esriSimpleFillStyle.esriSFSNull, false);

            GlobalVars.TurnAreaElem = Functions.DrawPolygon(BaseArea, GlobalVars.TurnAreaElemColor, esriSimpleFillStyle.esriSFSNull, false);
            GlobalVars.NomTrackElem = Functions.DrawPolyline(TracPoly, GlobalVars.NomTrackElemColor, 1.0, false);

            if (GlobalVars.ButtonControl3State)
            {
                pGraphics.AddElement(GlobalVars.TurnAreaElem, 0);
                GlobalVars.TurnAreaElem.Locked = true;
            }

            if (GlobalVars.ButtonControl4State)
            {
                pGraphics.AddElement(GlobalVars.PrimElem, 0);
                GlobalVars.PrimElem.Locked = true;
                pGraphics.AddElement(GlobalVars.SecLElem, 0);
                GlobalVars.SecLElem.Locked = true;
                pGraphics.AddElement(GlobalVars.SecRElem, 0);
                GlobalVars.SecRElem.Locked = true;
            }

            if (GlobalVars.ButtonControl6State)
            {
                pGraphics.AddElement(GlobalVars.KKElem, 0);
                GlobalVars.KKElem.Locked = true;
                pGraphics.AddElement(GlobalVars.K1K1Elem, 0);
                GlobalVars.K1K1Elem.Locked = true;
            }

            if (GlobalVars.ButtonControl5State)
            {
                pGraphics.AddElement(GlobalVars.NomTrackElem, 0);
                GlobalVars.NomTrackElem.Locked = true;
            }

            GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            //Functions.RefreshCommandBar(_mTool, 124);
            // =========== End To FIX ==========
            return 1;
        }

        private int UpdateToCourse(double OutAzt)
        {
            int updateToCourseReturn = 0;

            IPoint ptCur = null;
            IPoint ptTmp = null;
            IPoint Pt0 = null;
            double curDist0 = 0;
            double curDist = 0;
            double AztCur = 0;
            double TurnR;
            double fTASl;
            double fTmp;

            ITopologicalOperator2 pTopoOper = null;
            IPointCollection pLPolygon = null;
            IPointCollection pRPolygon = null;
            IPointCollection tmpPoly1 = null;
            IPointCollection TmpPoly = null;
            IClone pSource = null;
            int iMax = 0;
            int K = 0;

            pSource = ((ESRI.ArcGIS.esriSystem.IClone)(UnitedPolygon));
            pRPolygon = ((ESRI.ArcGIS.Geometry.IPointCollection)(pSource.Clone()));

            if (!(OptionButton401.Checked))
            { // ?? ??????

                TmpPoly = new Polyline();
                TmpPoly.AddPoint(Functions.PointAlongPlane(KK.ToPoint, DepDir - 90.0, GlobalVars.RModel));
                TmpPoly.AddPoint(Functions.PointAlongPlane(KK.FromPoint, DepDir + 90.0, GlobalVars.RModel));

                Functions.CutPoly(ref pRPolygon, TmpPoly, -1);
                pRPolygon = Functions.ReArrangePolygon(pRPolygon, DER.pPtPrj[eRWY.PtDER], RWYDir);
            }

            fTASl = Functions.IAS2TAS(fIAS, (TurnFixPnt.Z), GlobalVars.CurrADHP.ISAtC);
            TurnR = Functions.Bank2Radius(fBankAngle, fTASl);
            // '=========  End Params

            IPointCollection DrawPoly = null;

            ptTmp = Functions.PointAlongPlane(TurnFixPnt, DepDir + 90.0 * TurnDir, TurnR);
            Pt0 = Functions.PointAlongPlane(ptTmp, OutAzt - 90.0 * TurnDir, TurnR);
            Pt0.M = OutAzt;

            MPtCollection = new Multipoint();
            MPtCollection.AddPoint(TurnFixPnt);
            MPtCollection.AddPoint(Pt0);
            TracPoly = (IPointCollection)Functions.CalcTrajectoryFromMultiPoint(MPtCollection);

            ptCur = MPtCollection.Point[MPtCollection.PointCount - 1];

            IProximityOperator pProxi = null;
            if (OptionButton703.Checked)
                TracPoly.AddPoint(TurnDirector.pPtPrj);
            else
            {
                pProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)pCircle;
                if (pProxi.ReturnDistance(ptCur) == 0.0)
                {
                    Functions.CircleVectorIntersect(GlobalVars.CurrADHP.pPtPrj, GlobalVars.RModel, ptCur, ptCur.M, out ptTmp);
                    TracPoly.AddPoint(ptTmp);
                }
            }

            BasePoints = Functions.CreateBasePoints(pRPolygon, K1K1, DepDir, TurnDir);
            ZNR_Poly = Functions.CreateBasePoints(UnitedPolygon, KK, DepDir, TurnDir);

            pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)ZNR_Poly;
            pTopoOper.IsKnownSimple_2 = false;
            pTopoOper.Simplify();

            TurnArea = CreateTurnArea(3.6 * PANS_OPS_DataBase.dpWind_Speed.Value, TurnR, fTASl, ref DepDir, ref OutAzt, TurnDir, BasePoints);

            // =========================================
            // Functions.DrawPolygon TurnArea, RGB(0, 255, 255), , esriSFSDiagonalCross
            // Functions.DrawPolygon BasePoints, 0, , esriSFSDiagonalCross
            // =========================================

            int Side = 0;
            if (OptionButton401.Checked)
            {
                if (TurnDir > 0)
                {
                    if (TurnArea.PointCount > 0)
                    {
                        TurnArea.AddFirstPoint(KK.FromPoint);
                        TurnArea.AddFirstPoint(KK.ToPoint);
                    }
                    else
                    {
                        TurnArea.AddPoint(KK.ToPoint);
                        TurnArea.AddPoint(KK.FromPoint);
                    }
                }
                else
                {
                    if (TurnArea.PointCount > 0)
                    {
                        TurnArea.AddFirstPoint(KK.ToPoint);
                        TurnArea.AddFirstPoint(KK.FromPoint);
                    }
                    else
                    {
                        TurnArea.AddPoint(KK.FromPoint);
                        TurnArea.AddPoint(KK.ToPoint);
                    }
                }

                if (CheckBox401.Checked)
                {
                    if (TurnDir > 0)
                        TurnArea.AddFirstPoint(pPolygon.Point[pPolygon.PointCount - 2]);
                    else
                        TurnArea.AddFirstPoint(pPolygon.Point[1]);
                }
                //         Set ptCur = TurnArea.Point(TurnArea.PointCount - 1)
            }
            else
            {
                K = BasePoints.PointCount;

                if (TurnArea.PointCount > 0)
                {
                    pLPolygon = new ESRI.ArcGIS.Geometry.Polygon();
                    for (int i = 1; i < K; i++)
                    {
                        Side = Functions.SideDef(DER.pPtPrj[eRWY.PtDER], DepDir, BasePoints.Point[i]);
                        if (Side == TurnDir)
                            pLPolygon.AddPoint(BasePoints.Point[i]);
                    }
                    pLPolygon.AddPointCollection(TurnArea);
                    TurnArea = pLPolygon;
                    //             Set ptCur = TurnArea.Point(TurnArea.PointCount - 1)
                }
                else
                {
                    ptCur = BasePoints.Point[0];
                    for (int i = 1; i < K; i++)
                    {
                        Side = Functions.SideDef(DER.pPtPrj[eRWY.PtDER], DepDir, BasePoints.Point[i]);
                        if (Side == TurnDir)
                            TurnArea.AddPoint(BasePoints.Point[i]);
                    }
                    TurnArea.AddPoint(ptCur);
                }
            }

            ptCur = TurnArea.Point[TurnArea.PointCount - 1];
            AztCur = OutAzt + PANS_OPS_DataBase.dpafTrn_OSplay.Value * TurnDir;

            if (OptionButton401.Checked)
                tmpPoly1 = ZNR_Poly;
            else
                tmpPoly1 = pFIXPoly;

            curDist0 = 0.0;
            K = tmpPoly1.PointCount;
            for (int i = 0; i < K; i++)
            {
                curDist = Functions.Point2LineDistancePrj(tmpPoly1.Point[i], ptCur, AztCur);
                if ((curDist0 < curDist))
                {
                    iMax = i;
                    curDist0 = curDist;
                }
            }
            ptTmp = tmpPoly1.Point[iMax];

            TurnArea.AddFirstPoint(ptTmp);
            // Functions.DrawPolygon TurnArea, 255

            tmpPoly1 = new ESRI.ArcGIS.Geometry.Polygon();
            tmpPoly1.AddPoint(ptTmp);
            tmpPoly1.AddPoint(TurnArea.Point[TurnArea.PointCount - 1]);

            // ============= To Course ============

            ESRI.ArcGIS.Geometry.IPoint transTemp254 = null;
            curDist0 = Functions.CircleVectorIntersect(ptTmp, GlobalVars.RModel + GlobalVars.RModel, ptCur, AztCur, out transTemp254);

            ptCur = Functions.PointAlongPlane(ptCur, OutAzt - PANS_OPS_DataBase.dpafTrn_OSplay.Value * TurnDir, curDist0);
            tmpPoly1.AddPoint(ptCur);
            TurnArea.AddPoint(ptCur);

            ptCur = Functions.PointAlongPlane(ptTmp, OutAzt + PANS_OPS_DataBase.dpafTrn_ISplay.Value * TurnDir, GlobalVars.RModel + GlobalVars.RModel);
            tmpPoly1.AddPoint(ptCur);
            TurnArea.AddPoint(ptCur);

            //
            fTmp = Functions.ReturnAngleInDegrees(TurnArea.Point[0], ptCur);

            // ================================================================

            pTopoOper = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(tmpPoly1));
            pTopoOper.IsKnownSimple_2 = false;
            pTopoOper.Simplify();

            pTopoOper = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(BasePoints));
            pTopoOper.IsKnownSimple_2 = false;
            pTopoOper.Simplify();

            BaseArea = BasePoints;

            //     If SideFrom2Angle(DepDir + 90.0 * TurnDir, fTmp) * TurnDir < 0 Then
            if (NativeMethods.Modulus((OutAzt - DepDir) * TurnDir) > 90.0)
                BaseArea = pTopoOper.Union(tmpPoly1 as IGeometry) as IPointCollection;

            tmpPoly1 = new ESRI.ArcGIS.Geometry.Polygon();
            tmpPoly1.AddPointCollection(TurnArea);

            pTopoOper = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(tmpPoly1));
            pTopoOper.IsKnownSimple_2 = false;
            pTopoOper.Simplify();

            BaseArea = Functions.RemoveHoles(pTopoOper.Union(BaseArea as IGeometry) as IPolygon) as IPointCollection;

            if (OptionButton401.Checked)
            {
                pTopoOper = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(BaseArea));
                BaseArea = pTopoOper.Difference(ZNR_Poly as IGeometry) as IPointCollection;
            }

            pTopoOper = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(pCircle));

            DrawPoly = Functions.PolygonIntersection(pCircle as IPolygon, BaseArea as IPolygon) as IPointCollection;
            // ================================================================

            Functions.DeleteGraphicsElement(GlobalVars.KKElem);
            Functions.DeleteGraphicsElement(GlobalVars.K1K1Elem);
            Functions.DeleteGraphicsElement(GlobalVars.TurnAreaElem);
            Functions.DeleteGraphicsElement(GlobalVars.PrimElem);
            Functions.DeleteGraphicsElement(GlobalVars.SecRElem);
            Functions.DeleteGraphicsElement(GlobalVars.SecLElem);
            Functions.DeleteGraphicsElement(GlobalVars.NomTrackElem);

            GlobalVars.PrimElem = null;
            GlobalVars.SecRElem = null;
            GlobalVars.SecLElem = null;

            GlobalVars.KKElem = Functions.DrawPolyline(KK, 0, 1.0, false);
            GlobalVars.K1K1Elem = Functions.DrawPolyline(K1K1, 0, 1.0, false);
            GlobalVars.TurnAreaElem = Functions.DrawPolygon(DrawPoly, 255, esriSimpleFillStyle.esriSFSNull, false);
            GlobalVars.NomTrackElem = Functions.DrawPolyline(TracPoly, 0, 1.0, false);

            if (GlobalVars.ButtonControl3State)
            {
                pGraphics.AddElement(GlobalVars.TurnAreaElem, 0);
                GlobalVars.TurnAreaElem.Locked = true;
            }

            if (GlobalVars.ButtonControl6State)
            {
                pGraphics.AddElement(GlobalVars.KKElem, 0);
                GlobalVars.KKElem.Locked = true;
                pGraphics.AddElement(GlobalVars.K1K1Elem, 0);
                GlobalVars.K1K1Elem.Locked = true;
            }

            if (GlobalVars.ButtonControl5State)
            {
                pGraphics.AddElement(GlobalVars.NomTrackElem, 0);
                GlobalVars.NomTrackElem.Locked = true;
            }

            GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            //Functions.RefreshCommandBar(_mTool, 124);
            return updateToCourseReturn;
        }

        private void FIXOnRNE(double DepDist, double CircleR)
        {
            IPointCollection pPolyClone;
            IPointCollection pLine;
            IPolygon pPoly;
            IPointCollection mPoly;

            IGroupElement pGroupElement;
            ITopologicalOperator2 pTopo;
            IElement pElement;
            IClone Clone;
            IPoint Pt1;
            IPoint Pt2;

            double VTotal;
            double fTASl;
            double hTurn;
            double Ls;
            double d;
            int I;
            int K;

            Functions.DeleteGraphicsElement(GlobalVars.FIXElem);

            Clone = ((ESRI.ArcGIS.esriSystem.IClone)(UnitedPolygon));
            pPolyClone = ((ESRI.ArcGIS.Geometry.IPointCollection)(Clone.Clone()));
            pFIXPoly = ((ESRI.ArcGIS.Geometry.IPointCollection)(Clone.Clone()));

            pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(pPolyClone));
            pTopo.IsKnownSimple_2 = false;
            pTopo.Simplify();

            Pt1 = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir, DepDist);
            Pt2 = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir, DepDist + 2.0 * CircleR);

            pLine = new Polyline();
            pLine.AddPoint(Functions.PointAlongPlane(Pt1, DepDir + 90.0, GlobalVars.RModel));
            pLine.AddPoint(Functions.PointAlongPlane(Pt1, DepDir - 90.0, GlobalVars.RModel));
            Functions.CutPoly(ref pFIXPoly, pLine, 1);

            KK = pTopo.Intersect(pLine as IGeometry, esriGeometryDimension.esriGeometry1Dimension) as IPolyline;

            if (Functions.SideDef(DER.pPtPrj[eRWY.PtDER], DepDir, KK.FromPoint) < 0)
                KK.ReverseOrientation();

            // ==========================
            pLine.RemovePoints(0, pLine.PointCount);

            pLine.AddPoint(Functions.PointAlongPlane(Pt2, DepDir + 90.0, GlobalVars.RModel));
            pLine.AddPoint(Functions.PointAlongPlane(Pt2, DepDir - 90.0, GlobalVars.RModel));
            Functions.CutPoly(ref pFIXPoly, pLine, -1);

            KKFixMax = pTopo.Intersect(pLine as IGeometry, esriGeometryDimension.esriGeometry1Dimension) as IPolyline;

            if (Functions.SideDef(DER.pPtPrj[eRWY.PtDER], DepDir, KKFixMax.FromPoint) < 0)
                KKFixMax.ReverseOrientation();

            //     DrawPolyLine KKFixMax, 255

            K = ComboBox101.SelectedIndex;

            TurnFixPnt = new ESRI.ArcGIS.Geometry.Point();
            TurnFixPnt.PutCoords(FrontList[K].pPtPrj.X, FrontList[K].pPtPrj.Y);
            // ==========================
            d = Functions.Point2LineDistancePrj(DER.pPtPrj[eRWY.PtDER], TurnFixPnt, DepDir + 90.0);
            hTurn = d * PANS_OPS_DataBase.dpOv_Nav_PDG.Value + PANS_OPS_DataBase.dpOIS_abv_DER.Value + DER.pPtPrj[eRWY.PtDER].Z;

            TurnFixPnt.Z = hTurn;
            TurnFixPnt.M = DepDir;

            fTASl = Functions.IAS2TAS(fIAS, (TurnFixPnt.Z), GlobalVars.CurrADHP.ISAtC);
            VTotal = fTASl + GlobalVars.CurrADHP.WindSpeed;
            Ls = VTotal * PANS_OPS_DataBase.dpT_TechToleranc.Value * 0.277777777777778;

            Pt1 = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir, d + Ls + CircleR);

            pLine.RemovePoints(0, pLine.PointCount);

            pLine.AddPoint(Functions.PointAlongPlane(Pt1, DepDir + 90.0, GlobalVars.RModel));
            pLine.AddPoint(Functions.PointAlongPlane(Pt1, DepDir - 90.0, GlobalVars.RModel));

            K1K1 = pTopo.Intersect(pLine as IGeometry, esriGeometryDimension.esriGeometry1Dimension) as IPolyline;

            if (Functions.SideDef(K1K1.ToPoint, DepDir, K1K1.FromPoint) < 0)
                K1K1.ReverseOrientation();

            //     DrawPolyLine K1K1, 255
            // ============================================================
            pPoly = Functions.CreatePrjCircle(FrontList[K].pPtPrj, CircleR);
            pGroupElement = ((ESRI.ArcGIS.Carto.IGroupElement)(new GroupElement()));

            pGroupElement.AddElement(Functions.DrawPolygon(pFIXPoly, Functions.RGB(255, 255, 0), esriSimpleFillStyle.esriSFSNull, false));
            pGroupElement.AddElement(Functions.DrawPolygon(pPoly, Functions.RGB(255, 0, 255), esriSimpleFillStyle.esriSFSNull, false));

            mPoly = new Polyline();
            mPoly.AddPoint(PtDerShift);
            mPoly.AddPoint(TurnFixPnt);

            Functions.DeleteGraphicsElement(GlobalVars.StrTrackElem);
            GlobalVars.StrTrackElem = Functions.DrawPolyline(mPoly, GlobalVars.StrTrackElemColor, 1.0, false);

            if (GlobalVars.ButtonControl8State)
            {
                for (I = 0; I < pGroupElement.ElementCount; I++)
                {
                    pElement = pGroupElement.get_Element(I);
                    pGraphics.AddElement(pElement, 0);
                    pElement.Locked = true;
                }
            }

            if (GlobalVars.ButtonControl5State)
            {
                pGraphics.AddElement(GlobalVars.StrTrackElem, 0);
                GlobalVars.StrTrackElem.Locked = true;
            }

            GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            //Functions.RefreshCommandBar(_mTool, 128);

            GlobalVars.FIXElem = ((ESRI.ArcGIS.Carto.IElement)(pGroupElement));
        }

        private int CalcNewStraightAreaWithFixedLength(ref double NewPDG, ref double NewTAPDG)
        {
            Functions.DeleteGraphicsElement(GlobalVars.FIXElem);
            GlobalVars.FIXElem = null;

            int i, n, k = ComboBox601.SelectedIndex;
            double Intersect = 0;

            if (OptionButton402.Checked)
            {
                Intersect = double.Parse(TextBox601.Text);

                if (TurnInterDat[k].TypeCode == eNavaidType.NDB)
                    Intersect += 180.0;
                else if (TurnInterDat[k].TypeCode == eNavaidType.DME)
                    Intersect = Functions.DeConvertDistance(Intersect);
            }

            double NewHMinTurn, NewHMaxTurn, NewCurrPDG;

            do
            {
                double NewHTurn;

                if (OptionButton403.Checked)
                {
                    double CircleR, DepDist = CalcRNELocalRange(out NewCurrPDG, TrackAdjust, out CircleR, true, NewPDG);

                    NewHMinTurn = System.Math.Round(DepDist * NewCurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value - 0.4999);
                    NewHMaxTurn = System.Math.Round((DepDist + 2.0 * CircleR) * NewCurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value + 0.4999);

                    FIXOnRNE(DepDist, CircleR);
                    NewHTurn = NewHMinTurn;
                }
                else
                {
                    double DepDist = Functions.Point2LineDistancePrj(DER.pPtPrj[eRWY.PtDER], KK.FromPoint, DepDir + 90.0);
                    Interval mIntr = CalcLocalRange(NewPDG, out NewCurrPDG, TrackAdjust, true);

                    NewHMinTurn = System.Math.Round(mIntr.Left * NewCurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value - 0.4999);
                    NewHMaxTurn = System.Math.Round(mIntr.Right * NewCurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value + 0.4999);
                    NewHTurn = System.Math.Round(DepDist * NewCurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value + 0.49);
                }

                if (!(NewHTurn > NewHMaxTurn || NewHTurn < NewHMinTurn))
                {
                    if (OptionButton402.Checked)
                    {
                        NavaidType[] NavDatL = Functions.GetValidNavs(UnitedPolygon as IPolygon, DER, DepDir, NewHMinTurn, NewHMaxTurn, NewCurrPDG);

                        n = NavDatL.Length;
                        int j = -1;
                        for (i = 0; i < n; i++)
                        {
                            if ((NavDatL[i].Name == TurnInterDat[k].Name) && (NavDatL[i].TypeCode == TurnInterDat[k].TypeCode))
                            {
                                j = i;
                                break;
                            }
                        }

                        if (j >= 0)
                        {
                            if (NavDatL[j].TypeCode == eNavaidType.DME)
                            {
                                if (Option602.Enabled && Option602.Checked)
                                {
                                    if ((Intersect < NavDatL[j].ValMin[1]) || (Intersect > NavDatL[j].ValMax[1]))
                                        goto ContinueL;
                                }
                                else if ((Intersect < NavDatL[j].ValMin[0]) || (Intersect > NavDatL[j].ValMax[0]))
                                    goto ContinueL;

                            }
                            else if (NavDatL[j].TypeCode == eNavaidType.VOR || NavDatL[j].TypeCode == eNavaidType.NDB)
                            {
                                if (!Functions.AngleInSector(Intersect + GlobalVars.CurrADHP.MagVar, NavDatL[j].ValMin[0], NavDatL[j].ValMax[0]))
                                    goto ContinueL;
                            }
                            else
                                goto ContinueL;


                            TextBox601.Tag = "a";
                            TextBox601_Validating(TextBox601, new System.ComponentModel.CancelEventArgs());
                            break;
                        }
                    }
                    else
                        break;
                }
            ContinueL:

                NewPDG = NewPDG + 0.001;

                if (NewPDG > PANS_OPS_DataBase.dpMaxPosPDG.Value * 50.0)
                {
                    MessageBox.Show(Resources.str15462, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                }
            }
            while (true);

            hMaxTurn = NewHMaxTurn;
            hMinTurn = NewHMinTurn;
            CurrPDG = NewCurrPDG;

            // ===================================================================
            if (OptionButton703.Checked)
            {
                if (UpdateToFix() == 1)
                    SecondArea(TurnDir, BaseArea);
            }
            else if (OptionButton702.Checked)
            {
                UpdateToNavCurs(DirCourse);
                SecondArea(TurnDir, BaseArea);
            }
            else
                UpdateToCourse(DirCourse);

            // ===================================================================
            IGeometry pDistPoly;
            IPolyline pCutter;
            IGeometry pFullArea, pSecArea, pTmpPoly;
            ITopologicalOperator2 pTopo;
            IPoint ptCnt;

            if (OptionButton401.Checked)
                pDistPoly = (ESRI.ArcGIS.Geometry.IGeometry)ZNR_Poly;
            else
                pDistPoly = KK;

            if (OptionButton701.Checked)
            {
                IPoint ptTmp = MPtCollection.Point[MPtCollection.PointCount - 1];
                Functions.CircleVectorIntersect(GlobalVars.CurrADHP.pPtPrj, GlobalVars.RModel, ptTmp, ptTmp.M, out ptCnt);
            }
            else if (OptionButton702.Checked)
                ptCnt = TerFixPnt;
            else
                ptCnt = TurnDirector.pPtPrj;

            //if (!OptionButton701.Checked || TurnWPT.TypeCode > eNavaidType.CodeNONE)
            {
                IPoint nPt1 = TurnArea.Point[0];
                IPoint nPt2 = TurnArea.Point[TurnArea.PointCount - 1];
                double fX, fY;

                Functions.PrjToLocal(ptCnt, DirCourse, nPt1, out fX, out fY);
                if (fX > 0)
                    ptCnt = nPt1;

                Functions.PrjToLocal(ptCnt, DirCourse, nPt2, out fX, out fY);
                if (fX > 0)
                    ptCnt = nPt2;
            }

            // ==========================================================================================================
            double dBuffer;
            if (!double.TryParse(TextBox1005.Text, out dBuffer))
                dBuffer = -1.0;
            else
                dBuffer = Functions.DeConvertDistance(dBuffer);

            if (dBuffer < PANS_OPS_DataBase.dpTermMinBuffer.Value)
            {
                dBuffer = PANS_OPS_DataBase.dpTermMinBuffer.Value;
                if (!OptionButton701.Checked)
                    TextBox1005.Text = Functions.ConvertDistance(dBuffer, eRoundMode.NEAREST).ToString();
            }

            pCutter = (IPolyline)(new Polyline());
            pCutter.FromPoint = Functions.LocalToPrj(ptCnt, DirCourse, dBuffer, 100000.0);
            pCutter.ToPoint = Functions.LocalToPrj(ptCnt, DirCourse, dBuffer, -100000.0);

            IRelationalOperator pRelation = (IRelationalOperator)pCutter;

            if (pRelation.Disjoint((IGeometry)BaseArea) || pRelation.Disjoint(pCircle))
                pFullArea = (IGeometry)BaseArea;
            else
            {
                pTopo = (ITopologicalOperator2)BaseArea;
                pTopo.Cut(pCutter, out pTmpPoly, out pFullArea);
            }

            if (pRelation.Disjoint(SecPoly) || pRelation.Disjoint(pCircle))
                pSecArea = (IGeometry)SecPoly;
            else
            {
                pTopo = (ITopologicalOperator2)SecPoly;
                pTopo.Cut(pCutter, out pTmpPoly, out pSecArea);
            }

            // ==========================================================================================================

            NewTAPDG = PANS_OPS_DataBase.dpPDG_Nom.Value;
            //Functions.GetTurnAreaObstacles(oFullList, out oTrnAreaList, (IPolygon)BaseArea, SecPoly, pDistPoly, DER, DepDir, DirToNav, TurnWPT.pPtPrj, MOCLimit);
            Functions.GetTurnAreaObstacles(oFullList, out oTrnAreaList, (IPolygon)pFullArea, (IPolygon)pSecArea, pDistPoly, DER, DepDir, DirToNav, TurnDirector.pPtPrj, MOCLimit);

            double hTurn = Functions.Point2LineDistancePrj(DER.pPtPrj[eRWY.PtDER], KK.ToPoint, DepDir + 90.0) * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value;
            double dTNAh = Functions.CalcTA_Hpenet(oTrnAreaList.Parts, hTurn, NewTAPDG, out i);
            NewTAPDG = Functions.CalcTA_PDG(oTrnAreaList.Parts, hTurn, NewTAPDG, out i);

            ReportsFrm.FillPage5(oTrnAreaList);
            // =========================================================================================================

            MaxReq = MOCLimit;
            int IxMaxReq = -1;

            n = oTrnAreaList.Parts.Length;

            for (i = 0; i < n; i++)
            {
                //if (oTrnAreaList.Parts[i].Dist < MaxDist)
                {                   //double CurrReq = oTrnAreaList.Parts[i].Height + oTrnAreaList.Parts[i].MOC;
                    double CurrReq = oTrnAreaList.Parts[i].Height + oTrnAreaList.Parts[i].CLShift * MOCLimit;
                    if (MaxReq < CurrReq)
                    {
                        MaxReq = CurrReq;
                        IxMaxReq = i;
                    }
                }
            }

            TextBox1015.Text = Functions.ConvertHeight(MaxReq + DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.NEAREST).ToString();
            //TextBox1016.Text = TextBox1015.Text;
            TextBox1016_Validating(TextBox1016, null);

            ToolTip1.SetToolTip(TextBox1015, "");
            if (IxMaxReq >= 0)
                ToolTip1.SetToolTip(TextBox1015, oTrnAreaList.Obstacles[oTrnAreaList.Parts[IxMaxReq].Owner].UnicalName);

            return 0;
        }

        private void CDepartGuidanseFrm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                NativeMethods.HtmlHelp(0, GlobalVars.HelpFile, GlobalVars.HH_HELP_CONTEXT, HelpContextID);
                e.Handled = true;
            }
        }

        private void CDepartGuidanseFrm_FormClosed(System.Object eventSender, FormClosedEventArgs eventArgs)
        {
            screenCapture.Rollback();
            DBModule.CloseDB();
            Functions.DeleteGraphicsElement(GlobalVars.pCircleElem);
            GlobalVars.pCircleElem = null;

            if (GlobalVars.StraightAreaElem != null)
            {
                if (GlobalVars.StraightAreaElem is IGroupElement)
                    for (int i = 0; i < (GlobalVars.StraightAreaElem as IGroupElement).ElementCount; i++)
                        Functions.DeleteGraphicsElement((GlobalVars.StraightAreaElem as IGroupElement).Element[i]);
                else
                    Functions.DeleteGraphicsElement(GlobalVars.StraightAreaElem);
                GlobalVars.StraightAreaElem = null;
            }

            if (OptionButton702.Checked)
                Functions.DeleteGraphicsElement(GlobalVars.TerminationFIXElem);
            GlobalVars.TerminationFIXElem = null;

            Functions.DeleteGraphicsElement(GlobalVars.TurnAreaElem);
            GlobalVars.TurnAreaElem = null;

            Functions.DeleteGraphicsElement(GlobalVars.PrimElem);
            GlobalVars.PrimElem = null;

            Functions.DeleteGraphicsElement(GlobalVars.SecRElem);
            GlobalVars.SecRElem = null;

            Functions.DeleteGraphicsElement(GlobalVars.SecLElem);
            GlobalVars.SecLElem = null;

            Functions.DeleteGraphicsElement(GlobalVars.NomTrackElem);
            GlobalVars.NomTrackElem = null;

            Functions.DeleteGraphicsElement(GlobalVars.StrTrackElem);
            GlobalVars.StrTrackElem = null;

            Functions.DeleteGraphicsElement(GlobalVars.KKElem);
            GlobalVars.KKElem = null;

            Functions.DeleteGraphicsElement(GlobalVars.K1K1Elem);
            GlobalVars.K1K1Elem = null;

            Functions.DeleteGraphicsElement(GlobalVars.CLElem);
            GlobalVars.CLElem = null;

            Functions.DeleteGraphicsElement(GlobalVars.FIXElem);
            GlobalVars.FIXElem = null;

            //Functions.RefreshCommandBar(_mTool, 65535);

            GlobalVars.GetActiveView().Refresh();
            ReportsFrm.Close();

            //GlobalVars.CurrCmd = 0;
            //LogModule.CloseLog(); 
        }

        private IPointCollection CreateTurnArea(double WSpeed, double TurnR, double V, ref double AztEnter, ref double AztOut, int pTurnDir, IPointCollection BasePoints)
        {
            double Bank = Functions.Radius2Bank(TurnR, V);
            double Rv = 6355.0 * System.Math.Tan(GlobalVars.DegToRadValue * Bank) / (GlobalVars.PI * V);
            if (Rv > 3.0)
                Rv = 3.0;

            double coef = WSpeed / (Rv * 3.6);

            AztOut = NativeMethods.Modulus(AztOut);
            double wAztOut = AztOut;

            AztEnter = NativeMethods.Modulus(AztEnter, 360.0);
            ESRI.ArcGIS.Geometry.IPointCollection result = new ESRI.ArcGIS.Geometry.Polygon();

            double TurnAng;

            if (Functions.SubtractAngles(wAztOut, AztEnter) < 1.0)
                return result;
            else
                TurnAng = NativeMethods.Modulus((wAztOut - AztEnter) * pTurnDir, 360.0);

            IPointCollection TmpSpiral = new Polyline();
            double dAng = 0, AztCurr = AztEnter,
                AztNext = Functions.ReturnAngleInDegrees(BasePoints.Point[0], BasePoints.Point[1]);
            int i = 0, k = 0, n = BasePoints.PointCount;
            do
            {
                // ======================================
                if (OptionButton703.Checked && CheckBox701.Checked)
                {
                    double ThetaTouch = Functions.FixToTouchSpiral(BasePoints.Point[i], coef, TurnR, pTurnDir, AztOut, TurnDirector.pPtPrj, DepDir);
                    if (ThetaTouch < -370.0)
                    {
                        MessageBox.Show(Resources.str15470, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        wAztOut = AztOut;
                    }
                    else
                        wAztOut = ThetaTouch;

                    if (Functions.SubtractAngles(wAztOut, AztEnter) < 1.0)
                        TurnAng = 0.0;
                    else
                        TurnAng = NativeMethods.Modulus((wAztOut - AztEnter) * pTurnDir);
                }
                // ======================================

                double azt12 = Functions.ReturnAngleInDegrees(BasePoints.Point[i], BasePoints.Point[(i + 1) % n]);

                if (System.Math.Abs(NativeMethods.Modulus(azt12, 360.0) - NativeMethods.Modulus(AztCurr)) < 1.0)
                    AztNext = azt12;
                else if (Functions.SideFrom2Angle(AztNext, azt12) * TurnDir < 0)
                {
                    double dAng0 = NativeMethods.Modulus((AztCurr - azt12) * pTurnDir);
                    dAng = dAng - dAng0;
                    AztNext = azt12;

                    Functions.CreateWindSpiral(BasePoints.Point[i], AztEnter, azt12 - 90.0 * TurnDir, ref azt12, TurnR, coef, pTurnDir, TmpSpiral);

                    IPoint ptIntersect = new ESRI.ArcGIS.Geometry.Point();
                    IConstructPoint Constructor = (ESRI.ArcGIS.Geometry.IConstructPoint)ptIntersect;
                    Constructor.ConstructAngleIntersection(result.Point[result.PointCount - 1], GlobalVars.DegToRadValue * AztCurr, TmpSpiral.Point[TmpSpiral.PointCount - 1], GlobalVars.DegToRadValue * azt12);
                    result.AddPoint(ptIntersect);
                }
                else
                {
                    double dAng0 = NativeMethods.Modulus((azt12 - AztCurr) * pTurnDir, 360.0);
                    dAng = dAng + dAng0;

                    if (dAng < TurnAng)
                        AztNext = azt12;
                    else
                        AztNext = wAztOut;

                    Functions.CreateWindSpiral(BasePoints.Point[i], AztEnter, AztCurr, ref AztNext, TurnR, coef, pTurnDir, result);
                }

                AztCurr = AztNext;
                i = (i + 1) % n;
                k++;
            }
            while (Functions.SubtractAngles(AztNext, wAztOut) > GlobalVars.degEps);

            if (OptionButton703.Checked && CheckBox701.Checked)
                AztOut = wAztOut;

            return result;
        }

        private IPointCollection CalcAreaIntersection(IPointCollection TurnArea, double OutAzt, double DirToNav, int pTurnDir)
        {
            IPointCollection result = new ESRI.ArcGIS.Geometry.Polygon();
            result.AddPointCollection(TurnArea);

            IPointCollection LeftPolys = new Polyline();
            IPointCollection RightPolys = new Polyline();

            IPolyline OutLine = ((ESRI.ArcGIS.Geometry.IPolyline)(new Polyline()));
            IPolyline InLine = ((ESRI.ArcGIS.Geometry.IPolyline)(new Polyline()));

            IPoint PtInterE = new ESRI.ArcGIS.Geometry.Point();
            IPoint PtInterP = new ESRI.ArcGIS.Geometry.Point();

            TextBox801.Text = "";
            TextBox802.Text = "";

            LeftPolys.AddPoint(SecL.Point[5]);
            LeftPolys.AddPoint(SecL.Point[0]);
            LeftPolys.AddPoint(SecL.Point[1]);

            RightPolys.AddPoint(SecR.Point[2]);
            RightPolys.AddPoint(SecR.Point[3]);
            RightPolys.AddPoint(SecR.Point[4]);

            double InDir = OutAzt + PANS_OPS_DataBase.dpafTrn_OSplay.Value * TurnDir;
            double OutDir = OutAzt - PANS_OPS_DataBase.dpafTrn_OSplay.Value * TurnDir;

            IPoint PtIn = TurnArea.Point[0];
            IPoint ptOut = TurnArea.Point[TurnArea.PointCount - 1];
            IPoint ptTmp = Functions.PointAlongPlane(PtIn, InDir, 10.0 * GlobalVars.RModel);
            IPoint PtInter15 = null;

            InLine.FromPoint = PtIn;
            InLine.ToPoint = ptTmp;

            OutLine.FromPoint = ptOut;
            ptTmp = Functions.PointAlongPlane(ptOut, OutDir, 10.0 * GlobalVars.RModel);
            OutLine.ToPoint = ptTmp;


            double SplayAngle;
            if (TurnDirector.TypeCode == eNavaidType.NDB)
                SplayAngle = Navaids_DataBase.NDB.SplayAngle;
            else //if (TurnWPT.TypeCode == eNavaidType.CodeVOR)
                SplayAngle = Navaids_DataBase.VOR.SplayAngle;

            ITopologicalOperator2 TopoOper;
            IPointCollection TmpPoly = new ESRI.ArcGIS.Geometry.Polygon();
            int SideE, SideP, Side, i;

            if (pTurnDir > 0)
            {
                TmpPoly.AddPoint(PtIn);
                TmpPoly.AddPoint(ptOut);

                TopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)LeftPolys;
                IPointCollection pPoints = ((ESRI.ArcGIS.Geometry.IPointCollection)(TopoOper.Intersect(InLine, esriGeometryDimension.esriGeometry0Dimension)));

                if (pPoints.PointCount > 0)
                {
                    if (CheckState)
                    {
                        OptionButton801.Enabled = false;
                        OptionButton802.Enabled = false;
                        OptionButton803.Enabled = false;
                        Frame801.Enabled = false;
                    }

                    if (pPoints.PointCount > 1)
                    {
                        double minDist = 10.0 * GlobalVars.RModel;
                        for (i = 0; i < pPoints.PointCount; i++)
                        {
                            double CurrDist = Functions.ReturnDistanceInMeters(PtIn, pPoints.Point[i]);
                            if (CurrDist < minDist)
                            {
                                minDist = CurrDist;
                                NJoinPt = pPoints.Point[i];
                            }
                        }
                    }
                    else
                        NJoinPt = pPoints.Point[0];

                    TmpPoly.AddFirstPoint(NJoinPt);

                    Side = Functions.SideDef(NJoinPt, DirToNav + 90.0, TurnDirector.pPtPrj);
                    if (Side < 0)
                        ptTmp = Functions.PointAlongPlane(NJoinPt, DirToNav + SplayAngle, 10.0 * GlobalVars.RModel);
                    else
                    {
                        TmpPoly.AddFirstPoint(LeftPolys.Point[1]);
                        ptTmp = Functions.PointAlongPlane(LeftPolys.Point[1], DirToNav + SplayAngle, 10.0 * GlobalVars.RModel);
                    }

                    TmpPoly.AddFirstPoint(ptTmp);
                }
                else
                {
                    if (CheckState)
                        Frame801.Enabled = true;

                    SideE = -1;
                    Side = -1;
                    SideP = -1;

                    if (Functions.RayPolylineIntersect(LeftPolys, PtIn, DirToNav, ref PtInterP))
                        SideP = Functions.SideDef(PtIn, DirToNav + 90.0, PtInterP);

                    double Dir2Int = Functions.ReturnAngleInDegrees(PtIn, LeftPolys.Point[1]);
                    double ExtAngle = Functions.SubtractAngles(Dir2Int, DirToNav);
                    TextBox801.Text = System.Math.Round(ExtAngle).ToString();

                    if (ExtAngle <= 90.0)
                    {
                        PtInterE = new ESRI.ArcGIS.Geometry.Point();
                        PtInterE.PutCoords(PtIn.X, PtIn.Y);
                        SideE = Functions.SideDef(PtIn, Dir2Int + 90.0, PtInterE);
                    }

                    if (Functions.RayPolylineIntersect(LeftPolys, PtIn, DirToNav + PANS_OPS_DataBase.dpafTrn_OSplay.Value, ref PtInter15))
                        Side = Functions.SideDef(PtIn, DirToNav + PANS_OPS_DataBase.dpafTrn_OSplay.Value + 90.0, PtInter15);

                    OptionButton801.Enabled = (SideP > 0);
                    OptionButton802.Enabled = (Side > 0);
                    OptionButton803.Enabled = (SideE > 0);

                    //         MsgBox  "OptionButton801.Enabled = " + CStr(OptionButton801.Enabled) + Chr(13) + _
                    // '                              "OptionButton803.Enabled = " + CStr(OptionButton803.Enabled) + Chr(13) + _
                    // '                              "OptionButton802.Enabled = " + CStr(OptionButton802.Enabled)
                    //         Frame801.Enabled = OptionButton801.Enabled And OptionButton803.Enabled
                    // ======================================================================================
                    if (CheckState)
                    {
                        if (OptionButton803.Enabled)
                        {
                            OptionButton803.Checked = true;
                            ptTmp = PtInterE;
                        }
                        else if (OptionButton801.Enabled)
                        {
                            OptionButton801.Checked = true;
                            ptTmp = PtInterP;
                        }
                        else if (OptionButton802.Enabled)
                        {
                            OptionButton802.Checked = true;
                            ptTmp = PtInter15;
                        }
                    }
                    else if (OptionButton803.Checked)
                        ptTmp = PtInterE;
                    else if (OptionButton801.Checked)
                        ptTmp = PtInterP;
                    else
                        ptTmp = PtInter15;

                    NJoinPt = new ESRI.ArcGIS.Geometry.Point();
                    NJoinPt.PutCoords(ptTmp.X, ptTmp.Y);

                    TmpPoly.AddFirstPoint(ptTmp);
                    Side = Functions.SideDef(ptTmp, DirToNav + 90.0, TurnDirector.pPtPrj);

                    if (Side < 0)
                        ptTmp = Functions.PointAlongPlane(ptTmp, DirToNav + SplayAngle, 10.0 * GlobalVars.RModel);
                    else
                    {
                        TmpPoly.AddFirstPoint(LeftPolys.Point[1]);
                        ptTmp = Functions.PointAlongPlane(LeftPolys.Point[1], DirToNav + SplayAngle, 10.0 * GlobalVars.RModel);
                    }

                    TmpPoly.AddFirstPoint(ptTmp);
                }
                // ======================================================================================

                TopoOper = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(RightPolys));
                pPoints = ((ESRI.ArcGIS.Geometry.IPointCollection)(TopoOper.Intersect(OutLine, esriGeometryDimension.esriGeometry0Dimension)));
                if (pPoints.PointCount > 0)
                {
                    if (CheckState)
                    {
                        Frame802.Enabled = false;
                        OptionButton804.Enabled = false;
                        OptionButton805.Enabled = false;
                    }

                    if (pPoints.PointCount > 1)
                    {
                        double minDist = 10.0 * GlobalVars.RModel;
                        for (i = 0; i < pPoints.PointCount; i++)
                        {
                            double CurrDist = Functions.ReturnDistanceInMeters(ptOut, pPoints.Point[i]);
                            if (CurrDist < minDist)
                            {
                                minDist = CurrDist;
                                FJoinPt = pPoints.Point[i];
                            }
                        }
                    }
                    else
                        FJoinPt = pPoints.Point[0];

                    TmpPoly.AddPoint(FJoinPt);
                    Side = Functions.SideDef(FJoinPt, DirToNav + 90.0, TurnDirector.pPtPrj);

                    if (Side < 0)
                        ptTmp = Functions.PointAlongPlane(FJoinPt, DirToNav - SplayAngle, 10.0 * GlobalVars.RModel);
                    else
                    {
                        TmpPoly.AddPoint(RightPolys.Point[1]);
                        ptTmp = Functions.PointAlongPlane(RightPolys.Point[1], DirToNav - SplayAngle, 10.0 * GlobalVars.RModel);
                    }

                    TmpPoly.AddPoint(ptTmp);
                }
                else
                {
                    if (CheckState)
                    {
                        Frame802.Enabled = true;
                        OptionButton804.Enabled = true;
                        OptionButton805.Enabled = true;
                    }

                    SideP = -1;
                    Side = -1;

                    if (Functions.RayPolylineIntersect(RightPolys, ptOut, DirToNav, ref PtInterP))
                    {
                        //Dist1 = Functions.ReturnDistanceInMeters(PtInterP, ptOut);
                        SideP = Functions.SideDef(ptOut, DirToNav + 90.0, PtInterP);
                    }

                    if (Functions.RayPolylineIntersect(RightPolys, ptOut, DirToNav - PANS_OPS_DataBase.dpafTrn_OSplay.Value, ref PtInter15))
                    {
                        //Dist2 = Functions.ReturnDistanceInMeters(PtInter15, ptOut);
                        Side = Functions.SideDef(ptOut, DirToNav - PANS_OPS_DataBase.dpafTrn_OSplay.Value + 90.0, PtInter15);
                    }

                    OptionButton804.Enabled = SideP > 0;
                    OptionButton805.Enabled = Side > 0;

                    //         Frame802.Enabled = OptionButton804.Enabled And OptionButton806.Enabled

                    // ======================================================================================
                    if (CheckState)
                    {
                        if (OptionButton805.Enabled)
                        {
                            OptionButton805.Checked = true;
                            ptTmp = PtInter15;
                        }
                        else if (OptionButton804.Enabled)
                        {
                            OptionButton804.Checked = true;
                            ptTmp = PtInterP;
                        }
                    }
                    else if (OptionButton805.Checked)
                        ptTmp = PtInter15;
                    else if (OptionButton804.Checked)
                        ptTmp = PtInterP;

                    FJoinPt = new ESRI.ArcGIS.Geometry.Point();
                    FJoinPt.PutCoords(ptTmp.X, ptTmp.Y);

                    TmpPoly.AddPoint(ptTmp);
                    Side = Functions.SideDef(ptTmp, DirToNav + 90.0, TurnDirector.pPtPrj);

                    if (Side < 0)
                        ptTmp = Functions.PointAlongPlane(ptTmp, DirToNav - SplayAngle, 10.0 * GlobalVars.RModel);
                    else
                    {
                        TmpPoly.AddPoint(RightPolys.Point[1]);
                        ptTmp = Functions.PointAlongPlane(RightPolys.Point[1], DirToNav - SplayAngle, 10.0 * GlobalVars.RModel);
                    }

                    TmpPoly.AddPoint(ptTmp);
                }
                // ======================================================================================
            }
            else
            {
                TmpPoly.AddPoint(PtIn);
                TmpPoly.AddPoint(ptOut);

                TopoOper = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(RightPolys));
                IPointCollection pPoints = ((ESRI.ArcGIS.Geometry.IPointCollection)(TopoOper.Intersect(InLine, esriGeometryDimension.esriGeometry0Dimension)));
                if (pPoints.PointCount > 0)
                {
                    if (CheckState)
                    {
                        OptionButton801.Enabled = false;
                        OptionButton802.Enabled = false;
                        OptionButton803.Enabled = false;
                        Frame801.Enabled = false;
                    }

                    if (pPoints.PointCount > 1)
                    {
                        double minDist = 10.0 * GlobalVars.RModel;
                        for (i = 0; i < pPoints.PointCount; i++)
                        {
                            double CurrDist = Functions.ReturnDistanceInMeters(PtIn, pPoints.Point[i]);
                            if (CurrDist < minDist)
                            {
                                minDist = CurrDist;
                                NJoinPt = pPoints.Point[i];
                            }
                        }
                    }
                    else
                        NJoinPt = pPoints.Point[0];

                    TmpPoly.AddFirstPoint(NJoinPt);
                    Side = Functions.SideDef(NJoinPt, DirToNav + 90.0, TurnDirector.pPtPrj);

                    if (Side < 0)
                        ptTmp = Functions.PointAlongPlane(NJoinPt, DirToNav - SplayAngle, 10.0 * GlobalVars.RModel);
                    else
                    {
                        TmpPoly.AddFirstPoint(RightPolys.Point[1]);
                        ptTmp = Functions.PointAlongPlane(RightPolys.Point[1], DirToNav - SplayAngle, 10.0 * GlobalVars.RModel);
                    }

                    TmpPoly.AddFirstPoint(ptTmp);
                }
                else
                {
                    if (CheckState)
                        Frame801.Enabled = true;

                    SideE = -1;
                    Side = -1;
                    SideP = -1;

                    if (Functions.RayPolylineIntersect(RightPolys, PtIn, DirToNav, ref PtInterP))
                    {
                        //Dist1 = Functions.ReturnDistanceInMeters(PtInterP, PtIn);
                        SideP = Functions.SideDef(PtIn, DirToNav + 90.0, PtInterP);
                    }

                    double Dir2Int = Functions.ReturnAngleInDegrees(PtIn, RightPolys.Point[1]);
                    double ExtAngle = Functions.SubtractAngles(Dir2Int, DirToNav);
                    TextBox801.Text = System.Math.Round(ExtAngle, 1).ToString();

                    if (ExtAngle <= 90.0)
                    {
                        PtInterE = new ESRI.ArcGIS.Geometry.Point();
                        PtInterE.PutCoords(PtIn.X, PtIn.Y);
                        SideE = Functions.SideDef(PtIn, Dir2Int + 90.0, PtInterE);
                    }

                    if (Functions.RayPolylineIntersect(RightPolys, PtIn, DirToNav - PANS_OPS_DataBase.dpafTrn_OSplay.Value, ref PtInter15))
                        Side = Functions.SideDef(PtIn, DirToNav - PANS_OPS_DataBase.dpafTrn_OSplay.Value + 90.0, PtInter15);

                    OptionButton801.Enabled = SideP > 0;
                    OptionButton802.Enabled = Side > 0;
                    OptionButton803.Enabled = SideE > 0;
                    //         Frame801.Enabled = OptionButton801.Enabled And OptionButton803.Enabled
                    // ======================================================================================
                    if (CheckState)
                    {
                        if (OptionButton803.Enabled)
                        {
                            OptionButton803.Checked = true;
                            ptTmp = PtInterE;
                        }
                        else if (OptionButton801.Enabled)
                        {
                            OptionButton801.Checked = true;
                            ptTmp = PtInterP;
                        }
                        else if (OptionButton802.Enabled)
                        {
                            OptionButton802.Checked = true;
                            ptTmp = PtInter15;
                        }
                    }
                    else if (OptionButton803.Checked)
                        ptTmp = PtInterE;
                    else if (OptionButton801.Checked)
                        ptTmp = PtInterP;
                    else
                        ptTmp = PtInter15;

                    NJoinPt = new ESRI.ArcGIS.Geometry.Point();
                    NJoinPt.PutCoords(ptTmp.X, ptTmp.Y);

                    TmpPoly.AddFirstPoint(ptTmp);
                    Side = Functions.SideDef(ptTmp, DirToNav + 90.0, TurnDirector.pPtPrj);

                    if (Side < 0)
                        ptTmp = Functions.PointAlongPlane(ptTmp, DirToNav - SplayAngle, 10.0 * GlobalVars.RModel);
                    else
                    {
                        TmpPoly.AddFirstPoint(RightPolys.Point[1]);
                        ptTmp = Functions.PointAlongPlane(RightPolys.Point[1], DirToNav - SplayAngle, 10.0 * GlobalVars.RModel);
                    }

                    TmpPoly.AddFirstPoint(ptTmp);
                }
                // ======================================================================================

                TopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)LeftPolys;
                pPoints = (ESRI.ArcGIS.Geometry.IPointCollection)TopoOper.Intersect(OutLine, esriGeometryDimension.esriGeometry0Dimension);
                if (pPoints.PointCount > 0)
                {
                    if (CheckState)
                    {
                        OptionButton804.Enabled = false;
                        OptionButton805.Enabled = false;
                        Frame802.Enabled = false;
                    }

                    if (pPoints.PointCount > 1)
                    {
                        double minDist = 10.0 * GlobalVars.RModel;
                        for (i = 0; i < pPoints.PointCount; i++)
                        {
                            double CurrDist = Functions.ReturnDistanceInMeters(ptOut, pPoints.Point[i]);
                            if (CurrDist < minDist)
                            {
                                minDist = CurrDist;
                                FJoinPt = pPoints.Point[i];
                            }
                        }
                    }
                    else
                        FJoinPt = pPoints.Point[0];

                    TmpPoly.AddPoint(pPoints.Point[0]);
                    Side = Functions.SideDef(pPoints.Point[0], DirToNav + 90.0, TurnDirector.pPtPrj);

                    if (Side < 0)
                        ptTmp = Functions.PointAlongPlane(pPoints.Point[0], DirToNav + SplayAngle, 10.0 * GlobalVars.RModel);
                    else
                    {
                        TmpPoly.AddPoint(LeftPolys.Point[1]);
                        ptTmp = Functions.PointAlongPlane(LeftPolys.Point[1], DirToNav + SplayAngle, 10.0 * GlobalVars.RModel);
                    }

                    TmpPoly.AddPoint(ptTmp);
                }
                else
                {
                    if (CheckState)
                    {
                        Frame802.Enabled = true;
                        OptionButton804.Enabled = true;
                        OptionButton805.Enabled = true;
                    }

                    SideP = -1;
                    Side = -1;

                    if (Functions.RayPolylineIntersect(LeftPolys, ptOut, DirToNav, ref PtInterP))
                    {
                        //Dist1 = Functions.ReturnDistanceInMeters(PtInterP, ptOut);
                        SideP = Functions.SideDef(ptOut, DirToNav + 90.0, PtInterP);
                    }

                    if (Functions.RayPolylineIntersect(LeftPolys, ptOut, DirToNav + PANS_OPS_DataBase.dpafTrn_OSplay.Value, ref PtInter15))
                    {
                        //Dist2 = Functions.ReturnDistanceInMeters(PtInter15, ptOut);
                        Side = Functions.SideDef(ptOut, DirToNav + PANS_OPS_DataBase.dpafTrn_OSplay.Value + 90.0, PtInter15);
                    }

                    OptionButton804.Enabled = SideP > 0;
                    OptionButton805.Enabled = Side > 0;

                    //         Frame802.Enabled = OptionButton804.Enabled And OptionButton806.Enabled

                    // ======================================================================================
                    if (CheckState)
                    {
                        if (OptionButton805.Enabled)
                        {
                            OptionButton805.Checked = true;
                            ptTmp = PtInter15;
                        }
                        else if (OptionButton804.Enabled)
                        {
                            OptionButton804.Checked = true;
                            ptTmp = PtInterP;
                        }
                    }
                    else if (OptionButton805.Checked)
                        ptTmp = PtInter15;
                    else if (OptionButton804.Checked)
                        ptTmp = PtInterP;

                    FJoinPt = new ESRI.ArcGIS.Geometry.Point();
                    FJoinPt.PutCoords(ptTmp.X, ptTmp.Y);

                    TmpPoly.AddPoint(ptTmp);
                    Side = Functions.SideDef(ptTmp, DirToNav + 90.0, TurnDirector.pPtPrj);

                    if (Side < 0)
                        ptTmp = Functions.PointAlongPlane(ptTmp, DirToNav + SplayAngle, 10.0 * GlobalVars.RModel);
                    else
                    {
                        TmpPoly.AddPoint(LeftPolys.Point[1]);
                        ptTmp = Functions.PointAlongPlane(LeftPolys.Point[1], DirToNav + SplayAngle, 10.0 * GlobalVars.RModel);
                    }

                    TmpPoly.AddPoint(ptTmp);
                }
                // ======================================================================================
            }

            TopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)TmpPoly;
            TopoOper.IsKnownSimple_2 = false;
            TopoOper.Simplify();

            TopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)result;
            TopoOper.IsKnownSimple_2 = false;
            TopoOper.Simplify();

            result = (IPointCollection)TopoOper.Union(TmpPoly);
            TopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)result;
            TopoOper.IsKnownSimple_2 = false;
            TopoOper.Simplify();

            return result;
        }

        private void CalcJoiningParams(eNavaidType NavType, int iTurnDir, IPointCollection TurnArea, IPoint OutPt, double OutAzt)
        {
            IPoint ptCurr = new ESRI.ArcGIS.Geometry.Point();
            IConstructPoint Construct = ((ESRI.ArcGIS.Geometry.IConstructPoint)(ptCurr));

            PrimPoly = new ESRI.ArcGIS.Geometry.Polygon();
            SecL = new ESRI.ArcGIS.Geometry.Polygon();
            SecR = new ESRI.ArcGIS.Geometry.Polygon();
            IPointCollection AllPolys = new ESRI.ArcGIS.Geometry.Polygon();

            Functions.CreateNavaidZone(TurnDirector.pPtPrj, OutAzt, NavType, 2.0, SecL, SecR, PrimPoly);

            IPoint votPovorot = TurnArea.Point[TurnArea.PointCount - 1];

            AllPolys.AddPoint(SecL.Point[5]);
            AllPolys.AddPoint(SecL.Point[0]);
            AllPolys.AddPoint(SecL.Point[1]);

            AllPolys.AddPoint(SecR.Point[2]);
            AllPolys.AddPoint(SecR.Point[3]);
            AllPolys.AddPoint(SecR.Point[4]);

            TextBox801.Text = "";
            TextBox802.Text = "";

            NReqCorrAngle = -1.0;
            FReqCorrAngle = -1.0;

            int Side, Side1;
            double fTmpAzt;

            if (iTurnDir < 0)       // sag
            {
                // ================== xarici ========================
                fTmpAzt = Functions.ReturnAngleInDegrees(AllPolys.Point[1], AllPolys.Point[0]);
                Side = Functions.SideFrom2Angle(OutAzt, fTmpAzt);
                Side1 = Functions.SideDef(AllPolys.Point[1], fTmpAzt, votPovorot);

                if (Side * Side1 < 0)
                {
                    if (CheckState)
                    {
                        Frame802.Enabled = false;
                        //FState = noChange; 
                        OptionButton804.Enabled = false;
                        OptionButton806.Enabled = false;
                    }
                }
                else
                {
                    if (CheckState)
                    {
                        Frame802.Enabled = true;
                        OptionButton804.Enabled = true;
                        OptionButton806.Enabled = true;
                    }

                    FReqCorrAngle = Functions.ReturnAngleInDegrees(AllPolys.Point[1], votPovorot);
                    FReqCorrAngle = NativeMethods.Modulus(FReqCorrAngle - fTmpAzt);
                    if (FReqCorrAngle > 180.0)
                        FReqCorrAngle = 360.0 - FReqCorrAngle;

                    TextBox802.Text = System.Math.Round(FReqCorrAngle + 0.04999, 1).ToString();

                    if (FReqCorrAngle > FCorrAngle)
                    {
                        //FState = goParalel; 
                        OptionButton804.Checked = true;
                    }
                    else
                    {
                        //FState = expandAngle; 
                        OptionButton806.Checked = true;
                    }
                }
                // ================== daxili ========================
                fTmpAzt = Functions.ReturnAngleInDegrees(AllPolys.Point[4], AllPolys.Point[5]);
                Side = Functions.SideFrom2Angle(OutAzt, fTmpAzt);
                Side1 = Functions.SideDef(AllPolys.Point[4], fTmpAzt, TurnArea.Point[0]);

                if (Side * Side1 < 0)
                {
                    if (CheckState)
                    {
                        //NState = noChange; 
                        OptionButton801.Enabled = false;
                        OptionButton803.Enabled = false;
                        Frame801.Enabled = false;
                    }
                }
                else
                {
                    if (CheckState)
                    {
                        Frame801.Enabled = true;
                        OptionButton801.Enabled = true;
                        OptionButton803.Enabled = true;
                    }

                    NReqCorrAngle = Functions.ReturnAngleInDegrees(AllPolys.Point[4], TurnArea.Point[0]);
                    NReqCorrAngle = NativeMethods.Modulus(fTmpAzt - NReqCorrAngle);
                    if (NReqCorrAngle > 180.0)
                        NReqCorrAngle = 360.0 - NReqCorrAngle;

                    TextBox801.Text = System.Math.Round(NReqCorrAngle + 0.04999, 1).ToString();

                    if (CheckState)
                    {
                        if (NReqCorrAngle > NCorrAngle)
                        {
                            //NState = goParalel; 
                            OptionButton801.Checked = true;
                        }
                        else
                        {
                            //NState = expandAngle; 
                            OptionButton803.Checked = true;
                        }
                    }
                }
            }
            else            // sol
            {
                // ================== xarici ========================
                fTmpAzt = Functions.ReturnAngleInDegrees(AllPolys.Point[4], AllPolys.Point[5]);
                Side = Functions.SideFrom2Angle(OutAzt, fTmpAzt);
                Side1 = Functions.SideDef(AllPolys.Point[4], fTmpAzt, votPovorot);

                if (Side * Side1 < 0)
                {
                    if (CheckState)
                    {
                        //FState = noChange; 
                        OptionButton804.Enabled = false;
                        OptionButton806.Enabled = false;
                        Frame802.Enabled = false;
                    }
                }
                else
                {
                    if (CheckState)
                    {
                        Frame802.Enabled = true;
                        OptionButton804.Enabled = true;
                        OptionButton806.Enabled = true;
                    }

                    FReqCorrAngle = Functions.ReturnAngleInDegrees(AllPolys.Point[4], votPovorot);
                    FReqCorrAngle = NativeMethods.Modulus(FReqCorrAngle - fTmpAzt);
                    if (FReqCorrAngle > 180.0)
                        FReqCorrAngle = 360.0 - FReqCorrAngle;

                    TextBox802.Text = System.Math.Round(FReqCorrAngle + 0.04999, 1).ToString();

                    if (CheckState)
                    {
                        if (FReqCorrAngle > FCorrAngle)
                        {
                            //FState = goParalel; 
                            OptionButton804.Checked = true;
                        }
                        else
                        {
                            //FState = expandAngle; 
                            OptionButton806.Checked = true;
                        }
                    }
                }
                // ================== daxili ========================
                fTmpAzt = Functions.ReturnAngleInDegrees(AllPolys.Point[1], AllPolys.Point[0]);
                Side = Functions.SideFrom2Angle(OutAzt, fTmpAzt);
                Side1 = Functions.SideDef(AllPolys.Point[1], fTmpAzt, TurnArea.Point[0]);

                if (Side * Side1 < 0)
                {
                    if (CheckState)
                    {
                        //NState = noChange; 
                        OptionButton801.Enabled = false;
                        OptionButton803.Enabled = false;
                        Frame801.Enabled = false;
                    }
                }
                else
                {
                    if (CheckState)
                    {
                        Frame801.Enabled = true;
                        OptionButton801.Enabled = true;
                        OptionButton803.Enabled = true;
                    }

                    NReqCorrAngle = Functions.ReturnAngleInDegrees(AllPolys.Point[1], TurnArea.Point[0]);
                    NReqCorrAngle = NativeMethods.Modulus(fTmpAzt - NReqCorrAngle);
                    if (NReqCorrAngle > 180.0)
                        NReqCorrAngle = 360.0 - NReqCorrAngle;

                    TextBox801.Text = System.Math.Round(NReqCorrAngle + 0.04999, 1).ToString();

                    if (CheckState)
                    {
                        if (NReqCorrAngle > NCorrAngle)
                        {
                            //NState = goParalel; 
                            OptionButton801.Checked = true;
                        }
                        else
                        {
                            //NState = expandAngle; 
                            OptionButton803.Checked = true;
                        }
                    }
                }
            }
        }

        private ESRI.ArcGIS.Geometry.Polygon ApplayJoining(eNavaidType NavType, int pTurnDir, IPointCollection TurnArea, IPolygon BasePoints, IPoint OutPt, double OutAzt, IPointCollection tmpPoly1)
        {
            IClone pClone = (ESRI.ArcGIS.esriSystem.IClone)TurnArea;
            ESRI.ArcGIS.Geometry.Polygon result = (ESRI.ArcGIS.Geometry.Polygon)pClone.Clone();
            IPointCollection ApplayJoining1 = result;

            IPoint votPovorot = TurnArea.Point[TurnArea.PointCount - 1];
            IPoint antiPovorot = TurnArea.Point[0];

            NJoinPt = antiPovorot;
            FJoinPt = votPovorot;

            Functions.CreateNavaidZone(TurnDirector.pPtPrj, OutAzt, NavType, 2.0, SecL, SecR, PrimPoly);
            IPointCollection AllPolys = new ESRI.ArcGIS.Geometry.Polygon();

            AllPolys.AddPoint(SecL.Point[5]);
            AllPolys.AddPoint(SecL.Point[0]);
            AllPolys.AddPoint(SecL.Point[1]);

            AllPolys.AddPoint(SecR.Point[2]);
            AllPolys.AddPoint(SecR.Point[3]);
            AllPolys.AddPoint(SecR.Point[4]);

            NOutPt = new ESRI.ArcGIS.Geometry.Point();
            FOutPt = new ESRI.ArcGIS.Geometry.Point();
            IConstructPoint Construct = (ESRI.ArcGIS.Geometry.IConstructPoint)FOutPt;

            int Side1, Side;
            double NavOuterAzt, fTmpAzt;

            if (pTurnDir < 0)               // sag
            {
                // ================== xarici ========================
                fTmpAzt = Functions.ReturnAngleInDegrees(AllPolys.Point[1], AllPolys.Point[0]);
                Side = Functions.SideFrom2Angle(OutAzt, fTmpAzt);
                Side1 = Functions.SideDef(AllPolys.Point[1], fTmpAzt, votPovorot);

                if (Side * Side1 < 0)
                {
                    NavOuterAzt = OutAzt - PANS_OPS_DataBase.dpafTrn_OSplay.Value * pTurnDir;
                    Construct.ConstructAngleIntersection(votPovorot, GlobalVars.DegToRadValue * NavOuterAzt, AllPolys.Point[1], GlobalVars.DegToRadValue * fTmpAzt);
                    fTmpAzt = Functions.ReturnAngleInDegrees(TurnDirector.pPtPrj, AllPolys.Point[1]);
                    Side = Functions.SideDef(TurnDirector.pPtPrj, fTmpAzt, FOutPt);

                    if ((Side < 0))
                    {
                        ApplayJoining1.AddPoint(FOutPt);
                        ApplayJoining1.AddPoint(AllPolys.Point[1]);
                        tmpPoly1.AddPoint(FOutPt);
                        tmpPoly1.AddPoint(AllPolys.Point[1]);
                    }
                    else
                    {
                        fTmpAzt = Functions.ReturnAngleInDegrees(AllPolys.Point[0], AllPolys.Point[2]);
                        Construct.ConstructAngleIntersection(votPovorot, GlobalVars.DegToRadValue * NavOuterAzt, AllPolys.Point[0], GlobalVars.DegToRadValue * fTmpAzt);
                        ApplayJoining1.AddPoint(FOutPt);
                        tmpPoly1.AddPoint(FOutPt);
                    }

                    ApplayJoining1.AddPoint(AllPolys.Point[2]);
                    tmpPoly1.AddPoint(AllPolys.Point[2]);
                }
                else
                {
                    if (OptionButton804.Checked)
                    {
                        fTmpAzt = Functions.ReturnAngleInDegrees(AllPolys.Point[1], AllPolys.Point[2]);
                        Construct.ConstructAngleIntersection(votPovorot, GlobalVars.DegToRadValue * OutAzt, AllPolys.Point[1], GlobalVars.DegToRadValue * fTmpAzt);

                        ApplayJoining1.AddPoint(FOutPt);
                        tmpPoly1.AddPoint(FOutPt);

                        Side = Functions.SideDef(FOutPt, OutAzt, AllPolys.Point[2]);
                        if (Side < 0)
                        {
                            ApplayJoining1.AddPoint(AllPolys.Point[2]);
                            tmpPoly1.AddPoint(AllPolys.Point[2]);
                        }
                    }
                    else
                    { // Genishlanir

                        FOutPt.PutCoords(votPovorot.X, votPovorot.Y);

                        ApplayJoining1.AddPoint(AllPolys.Point[1]);
                        ApplayJoining1.AddPoint(AllPolys.Point[2]);
                        tmpPoly1.AddPoint(AllPolys.Point[1]);
                        tmpPoly1.AddPoint(AllPolys.Point[2]);
                    }
                }
                // ================== daxili ========================
                Construct = ((ESRI.ArcGIS.Geometry.IConstructPoint)(NOutPt));

                fTmpAzt = Functions.ReturnAngleInDegrees(AllPolys.Point[4], AllPolys.Point[5]);

                Side = Functions.SideFrom2Angle(OutAzt, fTmpAzt);
                Side1 = Functions.SideDef(AllPolys.get_Point(4), fTmpAzt, antiPovorot);

                if (Side * Side1 < 0)
                {
                    NavOuterAzt = OutAzt + PANS_OPS_DataBase.dpafTrn_ISplay.Value * pTurnDir;
                    Construct.ConstructAngleIntersection(antiPovorot, GlobalVars.DegToRadValue * NavOuterAzt, AllPolys.get_Point(4), GlobalVars.DegToRadValue * fTmpAzt);

                    ApplayJoining1.AddPoint(AllPolys.get_Point(3));
                    tmpPoly1.AddPoint(AllPolys.get_Point(3));

                    fTmpAzt = Functions.ReturnAngleInDegrees(TurnDirector.pPtPrj, AllPolys.get_Point(4));
                    Side = Functions.SideDef(TurnDirector.pPtPrj, fTmpAzt, NOutPt);
                    if ((Side * pTurnDir < 0))
                    {
                        ApplayJoining1.AddPoint(AllPolys.get_Point(4));
                        tmpPoly1.AddPoint(AllPolys.get_Point(4));
                    }
                    else
                    {
                        fTmpAzt = Functions.ReturnAngleInDegrees(AllPolys.get_Point(4), AllPolys.get_Point(3));
                        Construct.ConstructAngleIntersection(antiPovorot, GlobalVars.DegToRadValue * NavOuterAzt, AllPolys.get_Point(4), GlobalVars.DegToRadValue * fTmpAzt);
                    }

                    ApplayJoining1.AddPoint(NOutPt);
                    tmpPoly1.AddPoint(NOutPt);
                }
                else
                {
                    if (OptionButton801.Checked)
                    {
                        fTmpAzt = Functions.ReturnAngleInDegrees(AllPolys.get_Point(4), AllPolys.get_Point(3));
                        Construct.ConstructAngleIntersection(antiPovorot, GlobalVars.DegToRadValue * OutAzt, AllPolys.get_Point(4), GlobalVars.DegToRadValue * fTmpAzt);

                        Side = Functions.SideDef(NOutPt, OutAzt, AllPolys.get_Point(3));
                        if (Side * pTurnDir < 0)
                        {
                            ApplayJoining1.AddPoint(AllPolys.get_Point(3));
                            tmpPoly1.AddPoint(AllPolys.get_Point(3));
                        }
                        ApplayJoining1.AddPoint(NOutPt);
                        tmpPoly1.AddPoint(NOutPt);
                    }
                    else
                    {
                        NOutPt.PutCoords(antiPovorot.X, antiPovorot.Y);

                        ApplayJoining1.AddPoint(AllPolys.get_Point(3));
                        ApplayJoining1.AddPoint(AllPolys.get_Point(4));
                        tmpPoly1.AddPoint(AllPolys.get_Point(3));
                        tmpPoly1.AddPoint(AllPolys.get_Point(4));
                    }
                }
            }
            else                // sol
            {
                // ================== xarici ========================
                fTmpAzt = Functions.ReturnAngleInDegrees(AllPolys.get_Point(4), AllPolys.get_Point(5));
                Side = Functions.SideFrom2Angle(OutAzt, fTmpAzt);
                Side1 = Functions.SideDef(AllPolys.get_Point(4), fTmpAzt, votPovorot);

                if (Side * Side1 < 0)
                {
                    NavOuterAzt = OutAzt - PANS_OPS_DataBase.dpafTrn_OSplay.Value * pTurnDir;
                    Construct.ConstructAngleIntersection(votPovorot, GlobalVars.DegToRadValue * NavOuterAzt, AllPolys.get_Point(4), GlobalVars.DegToRadValue * fTmpAzt);

                    fTmpAzt = Functions.ReturnAngleInDegrees(TurnDirector.pPtPrj, AllPolys.get_Point(4));
                    Side = Functions.SideDef(TurnDirector.pPtPrj, fTmpAzt, FOutPt);
                    if ((Side * pTurnDir > 0))
                    {
                        ApplayJoining1.AddPoint(FOutPt);
                        ApplayJoining1.AddPoint(AllPolys.get_Point(4));
                        tmpPoly1.AddPoint(FOutPt);
                        tmpPoly1.AddPoint(AllPolys.get_Point(4));
                    }
                    else
                    {
                        fTmpAzt = Functions.ReturnAngleInDegrees(AllPolys.get_Point(4), AllPolys.get_Point(3));
                        Construct.ConstructAngleIntersection(votPovorot, GlobalVars.DegToRadValue * NavOuterAzt, AllPolys.get_Point(4), GlobalVars.DegToRadValue * fTmpAzt);
                        ApplayJoining1.AddPoint(FOutPt);
                        tmpPoly1.AddPoint(FOutPt);
                    }

                    ApplayJoining1.AddPoint(AllPolys.get_Point(3));
                    tmpPoly1.AddPoint(AllPolys.get_Point(3));
                }
                else
                {
                    if (OptionButton804.Checked)
                    { // Genishlanmir

                        fTmpAzt = Functions.ReturnAngleInDegrees(AllPolys.get_Point(4), AllPolys.get_Point(3));
                        Construct.ConstructAngleIntersection(votPovorot, GlobalVars.DegToRadValue * OutAzt, AllPolys.get_Point(4), GlobalVars.DegToRadValue * fTmpAzt);

                        ApplayJoining1.AddPoint(FOutPt);
                        tmpPoly1.AddPoint(FOutPt);
                        Side = Functions.SideDef(FOutPt, OutAzt, AllPolys.get_Point(3));
                        if (Side > 0)
                        {
                            ApplayJoining1.AddPoint(AllPolys.get_Point(3));
                            tmpPoly1.AddPoint(AllPolys.get_Point(3));
                        }
                    }
                    else
                    { // Genishlanir

                        FOutPt.PutCoords(votPovorot.X, votPovorot.Y);

                        ApplayJoining1.AddPoint(AllPolys.get_Point(4));
                        ApplayJoining1.AddPoint(AllPolys.get_Point(3));
                        tmpPoly1.AddPoint(AllPolys.get_Point(4));
                        tmpPoly1.AddPoint(AllPolys.get_Point(3));
                    }
                }
                // ================== daxili ========================

                fTmpAzt = Functions.ReturnAngleInDegrees(AllPolys.get_Point(1), AllPolys.get_Point(0));
                Side = Functions.SideFrom2Angle(OutAzt, fTmpAzt);
                Side1 = Functions.SideDef(AllPolys.get_Point(1), fTmpAzt, antiPovorot);
                Construct = ((ESRI.ArcGIS.Geometry.IConstructPoint)(NOutPt));

                if (Side * Side1 < 0)
                {
                    NavOuterAzt = OutAzt + PANS_OPS_DataBase.dpafTrn_ISplay.Value * pTurnDir;
                    Construct.ConstructAngleIntersection(antiPovorot, GlobalVars.DegToRadValue * NavOuterAzt, AllPolys.get_Point(1), GlobalVars.DegToRadValue * fTmpAzt);

                    ApplayJoining1.AddPoint(AllPolys.get_Point(2));
                    tmpPoly1.AddPoint(AllPolys.get_Point(2));
                    fTmpAzt = Functions.ReturnAngleInDegrees(TurnDirector.pPtPrj, AllPolys.get_Point(1));
                    Side = Functions.SideDef(TurnDirector.pPtPrj, fTmpAzt, NOutPt);
                    if ((Side * pTurnDir < 0))
                    {
                        ApplayJoining1.AddPoint(AllPolys.get_Point(1));
                        tmpPoly1.AddPoint(AllPolys.get_Point(1));
                    }
                    else
                    {
                        fTmpAzt = Functions.ReturnAngleInDegrees(AllPolys.get_Point(1), AllPolys.get_Point(2));
                        Construct.ConstructAngleIntersection(antiPovorot, GlobalVars.DegToRadValue * NavOuterAzt, AllPolys.get_Point(1), GlobalVars.DegToRadValue * fTmpAzt);
                    }

                    ApplayJoining1.AddPoint(NOutPt);
                    tmpPoly1.AddPoint(NOutPt);
                }
                else
                {
                    if (OptionButton801.Checked)
                    { // Genishlanmir

                        fTmpAzt = Functions.ReturnAngleInDegrees(AllPolys.get_Point(1), AllPolys.get_Point(2));
                        Construct.ConstructAngleIntersection(antiPovorot, GlobalVars.DegToRadValue * OutAzt, AllPolys.get_Point(1), GlobalVars.DegToRadValue * fTmpAzt);

                        Side = Functions.SideDef(NOutPt, OutAzt, AllPolys.get_Point(2));
                        if (Side * pTurnDir < 0)
                        {
                            ApplayJoining1.AddPoint(AllPolys.get_Point(2));
                            tmpPoly1.AddPoint(AllPolys.get_Point(2));
                        }
                        ApplayJoining1.AddPoint(NOutPt);
                        tmpPoly1.AddPoint(NOutPt);
                    }
                    else
                    { // Genishlanir

                        NOutPt.PutCoords(antiPovorot.X, antiPovorot.Y);

                        ApplayJoining1.AddPoint(AllPolys.get_Point(2));
                        ApplayJoining1.AddPoint(AllPolys.get_Point(1));
                        tmpPoly1.AddPoint(AllPolys.get_Point(2));
                        tmpPoly1.AddPoint(AllPolys.get_Point(1));
                    }
                }
            }

            ApplayJoining1.AddPoint(ApplayJoining1.get_Point(0));
            tmpPoly1.AddPoint(tmpPoly1.get_Point(0));

            return result;
        }

        private void ApplayOptions()
        {
            if (MultiPage1.SelectedIndex != 8 || OptionButton701.Checked)
                return;

            Functions.CreateNavaidZone(TurnDirector.pPtPrj, OutAzt, TurnDirector.TypeCode, 2.0, SecL, SecR, PrimPoly);

            if (OptionButton703.Checked && (TurnDirector.TypeCode == eNavaidType.VOR || TurnDirector.TypeCode == eNavaidType.NDB))
            {
                IPointCollection tmpPoly1 = new ESRI.ArcGIS.Geometry.Polygon();

                tmpPoly1.AddPoint(TurnArea.get_Point(0));
                tmpPoly1.AddPoint(TurnArea.get_Point(TurnArea.PointCount - 1));

                IPointCollection NewPoly = ApplayJoining(TurnDirector.TypeCode, TurnDir, TurnArea, BasePoints as IPolygon, OutPt, OutAzt, tmpPoly1);
                NewPoly = Functions.RemoveAgnails(NewPoly);

                ITopologicalOperator2 pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)tmpPoly1;
                pTopoOper.IsKnownSimple_2 = false;
                pTopoOper.Simplify();

                //double fTmp = Functions.ReturnAngleInDegrees(TurnArea.get_Point(TurnArea.PointCount - 1), TurnArea.get_Point(TurnArea.PointCount - 2));

                pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)NewPoly;
                pTopoOper.IsKnownSimple_2 = false;
                pTopoOper.Simplify();

                NewPoly = (IPointCollection)pTopoOper.Union(BasePoints);
                pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)NewPoly;
                pTopoOper.IsKnownSimple_2 = false;
                pTopoOper.Simplify();

                if (NativeMethods.Modulus((OutAzt - DepDir) * TurnDir) > 90.0)
                {
                    NewPoly = (IPointCollection)pTopoOper.Union(tmpPoly1);
                    pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)NewPoly;
                    pTopoOper.IsKnownSimple_2 = false;
                    pTopoOper.Simplify();
                }

                tmpPoly1 = new ESRI.ArcGIS.Geometry.Polygon();
                tmpPoly1.AddPointCollection(TurnArea);

                pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)tmpPoly1;
                pTopoOper.IsKnownSimple_2 = false;
                pTopoOper.Simplify();

                BaseArea = (IPointCollection)Functions.RemoveHoles((IPolygon)pTopoOper.Union(NewPoly));

                if (OptionButton401.Checked)
                {
                    pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)BaseArea;
                    BaseArea = (IPointCollection)pTopoOper.Difference(ZNR_Poly);
                }

                BaseArea = (IPointCollection)Functions.PolygonIntersection(pCircle, BaseArea);
                // ================================================================
                Functions.DeleteGraphicsElement(GlobalVars.TurnAreaElem);
                Functions.DeleteGraphicsElement(GlobalVars.PrimElem);
                Functions.DeleteGraphicsElement(GlobalVars.SecLElem);
                Functions.DeleteGraphicsElement(GlobalVars.SecRElem);

                GlobalVars.TurnAreaElem = Functions.DrawPolygon(BaseArea, 255, esriSimpleFillStyle.esriSFSNull, false);

                tmpPoly1 = (IPointCollection)Functions.PolygonIntersection(pCircle, PrimPoly);
                GlobalVars.PrimElem = Functions.DrawPolygon(tmpPoly1, GlobalVars.PrimElemColor, esriSimpleFillStyle.esriSFSNull, false);

                tmpPoly1 = (IPointCollection)Functions.PolygonIntersection(pCircle, SecL);
                GlobalVars.SecLElem = Functions.DrawPolygon(tmpPoly1, GlobalVars.SecLElemColor, esriSimpleFillStyle.esriSFSNull, false);

                tmpPoly1 = (IPointCollection)Functions.PolygonIntersection(pCircle, SecR);
                GlobalVars.SecRElem = Functions.DrawPolygon(tmpPoly1, GlobalVars.SecRElemColor, esriSimpleFillStyle.esriSFSNull, false);

                if (GlobalVars.ButtonControl3State)
                {
                    pGraphics.AddElement(GlobalVars.TurnAreaElem, 0);
                    GlobalVars.TurnAreaElem.Locked = true;
                }

                if (GlobalVars.ButtonControl4State)
                {
                    pGraphics.AddElement(GlobalVars.PrimElem, 0);
                    GlobalVars.PrimElem.Locked = true;
                    pGraphics.AddElement(GlobalVars.SecLElem, 0);
                    GlobalVars.SecLElem.Locked = true;
                    pGraphics.AddElement(GlobalVars.SecRElem, 0);
                    GlobalVars.SecRElem.Locked = true;
                }

                GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                //Functions.RefreshCommandBar(_mTool, 4);
            }
            else if (OptionButton702.Checked)
                UpdateToNavCurs(DirCourse);
        }

        private void SecondArea(int pTurnDir, IPointCollection pTurnArea)
        {
            if (!(OptionButton703.Checked || OptionButton702.Checked))
            {
                MessageBox.Show(Resources.str00901, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            IPoint ptOut = MPtCollection.Point[MPtCollection.PointCount - 1];
            double OutAzt = ptOut.M;

            PrimPoly = new ESRI.ArcGIS.Geometry.Polygon();
            SecL = new ESRI.ArcGIS.Geometry.Polygon();
            SecR = new ESRI.ArcGIS.Geometry.Polygon();

            Functions.CreateNavaidZone(TurnDirector.pPtPrj, OutAzt, TurnDirector.TypeCode, 5.0, SecL, SecR, PrimPoly);

            IClone pClone = (ESRI.ArcGIS.esriSystem.IClone)pTurnArea;
            IPointCollection pGeneralArea = (ESRI.ArcGIS.Geometry.IPointCollection)pClone.Clone();

            IPointCollection pIArea = new ESRI.ArcGIS.Geometry.Polygon();
            pIArea.AddPoint(SecR.Point[5]);
            pIArea.AddPoint(SecR.Point[0]);
            pIArea.AddPoint(SecR.Point[1]);

            pIArea.AddPoint(SecL.Point[2]);
            pIArea.AddPoint(SecL.Point[3]);
            pIArea.AddPoint(SecL.Point[4]);

            ITopologicalOperator2 pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)pIArea;
            pTopo.IsKnownSimple_2 = false;
            pTopo.Simplify();

            pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)pGeneralArea;
            pTopo.IsKnownSimple_2 = false;
            pTopo.Simplify();

            IPolyline pCutter = ((ESRI.ArcGIS.Geometry.IPolyline)(new Polyline()));

            double Dist0;
            if (TurnDirector.TypeCode == eNavaidType.NDB)
                Dist0 = Navaids_DataBase.NDB.Range * 10.0;
            else //if (TurnWPT.TypeCode == eNavaidType.CodeVOR )
                Dist0 = Navaids_DataBase.VOR.Range * 10.0;

            IPoint ptFar = Functions.PointAlongPlane(TurnDirector.pPtPrj, OutAzt, Dist0);
            pCutter.ToPoint = ptFar;
            pCutter.FromPoint = Functions.PointAlongPlane(TurnDirector.pPtPrj, OutAzt + 180.0, Dist0);

            IPointCollection pTmpPoly0;
            PolyCut.ClipByLine(pGeneralArea, pCutter, out SecL, out SecR, out pTmpPoly0);

            pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(SecR));
            SecR = pTopo.Difference(pIArea as IGeometry) as IPointCollection;

            pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(SecL));
            SecL = pTopo.Difference(pIArea as IGeometry) as IPointCollection;

            SecR = Functions.RemoveFars(SecR, ptFar);
            SecL = Functions.RemoveFars(SecL, ptFar);

            IConstructPoint pConstruct;
            IPolygon pTmpPoly;
            IPoint ptCut;
            double TmpDir;

            if (CheckBox901.Checked)
            {
                if (OptionButton703.Checked)                    // Na FIX
                {
                    // ==================== Outer ==================
                    IPoint ptCutOuter = new ESRI.ArcGIS.Geometry.Point(), ptCutInner;

                    pConstruct = ((ESRI.ArcGIS.Geometry.IConstructPoint)(ptCutOuter));
                    pConstruct.ConstructAngleIntersection(ptOut, GlobalVars.DegToRadValue * OutAzt, FOutPt, GlobalVars.DegToRadValue * (OutAzt + 90.0));
                    int SideIn = Functions.SideDef(ptOut, OutAzt + 90.0, ptCutOuter);

                    if (SideIn < 0)
                        ptCutOuter.PutCoords(ptOut.X, ptOut.Y);

                    double OutDist = SideIn * Functions.ReturnDistanceInMeters(ptOut, ptCutOuter);
                    // ==================== Inner ==================
                    IPoint ptCutInner0 = new ESRI.ArcGIS.Geometry.Point();
                    IPoint ptCutInner1 = new ESRI.ArcGIS.Geometry.Point();

                    pConstruct = ((ESRI.ArcGIS.Geometry.IConstructPoint)(ptCutInner0));
                    pConstruct.ConstructAngleIntersection(ptOut, GlobalVars.DegToRadValue * OutAzt, NOutPt, GlobalVars.DegToRadValue * (OutAzt + 90.0));

                    if (ptCutInner0.IsEmpty)
                        ptCutInner0.PutCoords(ptCutOuter.X, ptCutOuter.Y);

                    pConstruct = ((ESRI.ArcGIS.Geometry.IConstructPoint)(ptCutInner1));
                    pConstruct.ConstructAngleIntersection(ptOut, GlobalVars.DegToRadValue * OutAzt, NOutPt, GlobalVars.DegToRadValue * DepDir);

                    if (ptCutInner1.IsEmpty)
                        ptCutInner1.PutCoords(ptCutInner0.X, ptCutInner0.Y);

                    int Side0 = Functions.SideDef(ptOut, OutAzt + 90.0, ptCutInner0);
                    Dist0 = Side0 * Functions.ReturnDistanceInMeters(ptOut, ptCutInner0);

                    int Side1 = Functions.SideDef(ptOut, OutAzt + 90.0, ptCutInner1);
                    double Dist1 = Side1 * Functions.ReturnDistanceInMeters(ptOut, ptCutInner1);

                    TmpDir = Functions.ReturnAngleInDegrees(ptCutInner0, ptCutInner1);

                    if (Side0 < 0)
                        ptCutInner0.PutCoords(ptOut.X, ptOut.Y);

                    if (Side1 < 0)
                        ptCutInner1.PutCoords(ptOut.X, ptOut.Y);

                    bool bFlg = Functions.SubtractAngles(OutAzt, TmpDir) > 0.5;
                    double InDist;

                    if (bFlg)
                    {
                        ptCutInner = ptCutInner0; // outazt+90
                        InDist = Dist0;
                    }
                    else
                    {
                        ptCutInner = ptCutInner1; // depdir
                        InDist = Dist1;
                    }
                    // ==================== Select suitable point ==================
                    if (OutDist > InDist)
                        ptCut = ptCutInner;
                    else
                        ptCut = ptCutOuter;
                    // ==================== Outer ==================
                    pCutter.FromPoint = ptCut;
                    pCutter.ToPoint = Functions.PointAlongPlane(ptCut, OutAzt - 90.0 * TurnDir, 2.0 * GlobalVars.RModel);

                    if (pTurnDir > 0)
                    {
                        PolyCut.ClipByLine(SecR, pCutter, out pTmpPoly, out PolyCut.MissingPolygon, out PolyCut.MissingPolygon);
                        if (!pTmpPoly.IsEmpty)
                            SecR = Functions.RemoveFars(pTmpPoly, ptFar);
                    }
                    else
                    {
                        PolyCut.ClipByLine(SecL, pCutter, out PolyCut.MissingPolygon, out pTmpPoly, out PolyCut.MissingPolygon);
                        if (!pTmpPoly.IsEmpty)
                            SecL = Functions.RemoveFars(pTmpPoly, ptFar);
                    }
                    // ==================== Inner ==================
                    if (bFlg)
                        pCutter.ToPoint = Functions.PointAlongPlane(ptCut, OutAzt + 90.0 * TurnDir, 2.0 * GlobalVars.RModel);
                    else
                    {
                        pCutter.ToPoint = Functions.PointAlongPlane(ptCut, DepDir + 180.0, 2.0 * GlobalVars.RModel);
                        pCutter.FromPoint = Functions.PointAlongPlane(ptCut, DepDir, 2.0 * GlobalVars.RModel);
                        if (Functions.SideDef(pCutter.ToPoint, OutAzt, pCutter.FromPoint) != TurnDir)
                            pCutter.ReverseOrientation();
                    }

                    if (pTurnDir > 0)
                    {
                        PolyCut.ClipByLine(SecL, pCutter, out PolyCut.MissingPolygon, out pTmpPoly, out PolyCut.MissingPolygon);
                        if (!pTmpPoly.IsEmpty)
                            SecL = Functions.RemoveFars(pTmpPoly, ptFar);
                    }
                    else
                    {
                        PolyCut.ClipByLine(SecR, pCutter, out pTmpPoly, out PolyCut.MissingPolygon, out PolyCut.MissingPolygon);
                        if (!pTmpPoly.IsEmpty)
                            SecR = Functions.RemoveFars(pTmpPoly, ptFar);
                    }
                }
                else
                { // Na curs FIX

                    // ==================== Outer ==================
                    //     DrawPoint FlyBy, RGB(255, 0, 255)
                    pCutter.FromPoint = FlyBy;
                    double DrDir = MPtCollection.Point[1].M;

                    if (Functions.SideDef(FlyBy, DrDir, ptOut) == TurnDir)
                        pCutter.ToPoint = Functions.PointAlongPlane(FlyBy, OutAzt - 90.0 * TurnDir, 2.0 * GlobalVars.RModel);
                    else
                        pCutter.ToPoint = Functions.PointAlongPlane(FlyBy, DrDir - 90.0 * TurnDir, 2.0 * GlobalVars.RModel);

                    if (pTurnDir > 0)
                    {
                        PolyCut.ClipByLine(SecR, pCutter, out pTmpPoly, out PolyCut.MissingPolygon, out PolyCut.MissingPolygon);
                        if (!pTmpPoly.IsEmpty)
                            SecR = Functions.RemoveFars(pTmpPoly, ptFar);
                    }
                    else
                    {
                        PolyCut.ClipByLine(SecL, pCutter, out PolyCut.MissingPolygon, out pTmpPoly, out PolyCut.MissingPolygon);
                        if (!pTmpPoly.IsEmpty)
                            SecL = Functions.RemoveFars(pTmpPoly, ptFar);
                    }
                    // ==================== Inner ==================
                    IPoint ptTmp = Functions.PointAlongPlane(FlyBy, DepDir, 2.0 * GlobalVars.RModel);
                    double TmpDepDir;

                    if (Functions.SideDef(FlyBy, OutAzt, ptTmp) == TurnDir)
                        TmpDepDir = DepDir + 180.0;
                    else
                        TmpDepDir = DepDir;

                    if (Functions.SideDef(FlyBy, DrDir, ptOut) != TurnDir)
                        TmpDir = OutAzt + 90.0 * TurnDir;
                    else
                        TmpDir = DrDir + 90.0 * TurnDir;

                    if (Functions.SideFrom2Angle(TmpDir, TmpDepDir) == TurnDir)
                        TmpDir = TmpDepDir;

                    pCutter.ToPoint = Functions.PointAlongPlane(FlyBy, TmpDir, 2.0 * GlobalVars.RModel);

                    if (pTurnDir > 0)
                    {
                        PolyCut.ClipByLine(SecL, pCutter, out PolyCut.MissingPolygon, out pTmpPoly, out PolyCut.MissingPolygon);
                        if (!pTmpPoly.IsEmpty)
                            SecL = Functions.RemoveFars(pTmpPoly, ptFar);
                    }
                    else
                    {
                        PolyCut.ClipByLine(SecR, pCutter, out pTmpPoly, out PolyCut.MissingPolygon, out PolyCut.MissingPolygon);
                        if (!pTmpPoly.IsEmpty)
                            SecR = Functions.RemoveFars(pTmpPoly, ptFar);
                    }
                }
            }

            if (CheckBox902.Checked)
            {
                ptCut = new ESRI.ArcGIS.Geometry.Point();
                pConstruct = ((ESRI.ArcGIS.Geometry.IConstructPoint)(ptCut));

                TmpDir = OutAzt - PANS_OPS_DataBase.dpSecAreaCutAngl.Value * TurnDir;
                pConstruct.ConstructAngleIntersection(TurnDirector.pPtPrj, GlobalVars.DegToRadValue * OutAzt, NJoinPt, GlobalVars.DegToRadValue * TmpDir);

                pCutter.FromPoint = ptCut;
                pCutter.ToPoint = Functions.PointAlongPlane(ptCut, TmpDir + 180.0, 2.0 * GlobalVars.RModel);

                if (pTurnDir > 0)
                {
                    PolyCut.ClipByLine(SecL, pCutter, out PolyCut.MissingPolygon, out pTmpPoly, out PolyCut.MissingPolygon);
                    if (!pTmpPoly.IsEmpty)
                        SecL = Functions.RemoveFars(pTmpPoly, ptFar);
                }
                else
                {
                    PolyCut.ClipByLine(SecR, pCutter, out pTmpPoly, out PolyCut.MissingPolygon, out PolyCut.MissingPolygon);
                    if (!pTmpPoly.IsEmpty)
                        SecR = Functions.RemoveFars(pTmpPoly, ptFar);
                }
            }

            if (CheckBox905.Checked)
            {
                ptCut = new ESRI.ArcGIS.Geometry.Point();
                pConstruct = ((ESRI.ArcGIS.Geometry.IConstructPoint)(ptCut));

                TmpDir = OutAzt + PANS_OPS_DataBase.dpSecAreaCutAngl.Value * TurnDir;
                pConstruct.ConstructAngleIntersection(TurnDirector.pPtPrj, GlobalVars.DegToRadValue * OutAzt, FJoinPt, GlobalVars.DegToRadValue * TmpDir);

                pCutter.FromPoint = ptCut;
                pCutter.ToPoint = Functions.PointAlongPlane(ptCut, TmpDir + 180.0, 2.0 * GlobalVars.RModel);

                if (pTurnDir > 0)
                {
                    PolyCut.ClipByLine(SecR, pCutter, out pTmpPoly, out PolyCut.MissingPolygon, out PolyCut.MissingPolygon);
                    if (!pTmpPoly.IsEmpty)
                        SecR = Functions.RemoveFars(pTmpPoly, ptFar);
                }
                else
                {
                    PolyCut.ClipByLine(SecL, pCutter, out PolyCut.MissingPolygon, out pTmpPoly, out PolyCut.MissingPolygon);
                    if (!pTmpPoly.IsEmpty)
                        SecL = Functions.RemoveFars(pTmpPoly, ptFar);
                }
            }

            ptCut = TurnDirector.pPtPrj;

            if (CheckBox903.Checked)
            {
                TmpDir = Functions.ReturnAngleInDegrees(ptCut, NJoinPt);

                pCutter.FromPoint = ptCut;
                pCutter.ToPoint = Functions.PointAlongPlane(ptCut, TmpDir, 2.0 * GlobalVars.RModel);

                if (pTurnDir > 0)
                {
                    PolyCut.ClipByLine(SecL, pCutter, out PolyCut.MissingPolygon, out pTmpPoly, out PolyCut.MissingPolygon);
                    if (!pTmpPoly.IsEmpty)
                        SecL = Functions.RemoveFars(pTmpPoly, ptFar);
                }
                else
                {
                    PolyCut.ClipByLine(SecR, pCutter, out pTmpPoly, out PolyCut.MissingPolygon, out PolyCut.MissingPolygon);
                    if (!pTmpPoly.IsEmpty)
                        SecR = Functions.RemoveFars(pTmpPoly, ptFar);
                }
            }

            if (CheckBox906.Checked)
            {
                TmpDir = Functions.ReturnAngleInDegrees(ptCut, FJoinPt);

                pCutter.FromPoint = ptCut;
                pCutter.ToPoint = Functions.PointAlongPlane(ptCut, TmpDir, 2.0 * GlobalVars.RModel);

                if (pTurnDir > 0)
                {
                    PolyCut.ClipByLine(SecR, pCutter, out pTmpPoly, out PolyCut.MissingPolygon, out PolyCut.MissingPolygon);
                    if (!pTmpPoly.IsEmpty)
                        SecR = Functions.RemoveFars(pTmpPoly, ptFar);
                }
                else
                {
                    PolyCut.ClipByLine(SecL, pCutter, out PolyCut.MissingPolygon, out pTmpPoly, out PolyCut.MissingPolygon);
                    if (!pTmpPoly.IsEmpty)
                        SecL = Functions.RemoveFars(pTmpPoly, ptFar);
                }
            }

            if (!(OptionButton401.Checked))
            {
                pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(SecR));
                SecR = pTopo.Difference(pFIXPoly as IGeometry) as IPointCollection;

                pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(SecL));
                SecL = pTopo.Difference(pFIXPoly as IGeometry) as IPointCollection;
            }

            // ==========================================
            pClone = ((ESRI.ArcGIS.esriSystem.IClone)(TurnArea));
            pTmpPoly0 = ((ESRI.ArcGIS.Geometry.IPointCollection)(pClone.Clone()));
            pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(pTmpPoly0));
            pTopo.IsKnownSimple_2 = false;
            pTopo.Simplify();

            pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(SecL));
            SecL = pTopo.Difference(pTmpPoly0 as IGeometry) as IPointCollection;

            pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(SecR));
            SecR = pTopo.Difference(pTmpPoly0 as IGeometry) as IPointCollection;
            // ===========================================
            SecL = Functions.RemoveFars(SecL, ptFar);
            SecR = Functions.RemoveFars(SecR, ptFar);

            SecL = Functions.RemoveAgnails(SecL);
            SecR = Functions.RemoveAgnails(SecR);

            Functions.DeleteGraphicsElement(GlobalVars.PrimElem);
            GlobalVars.PrimElem = null;
            Functions.DeleteGraphicsElement(GlobalVars.SecRElem);
            Functions.DeleteGraphicsElement(GlobalVars.SecLElem);

            GlobalVars.SecLElem = Functions.DrawPolygon(SecL, GlobalVars.SecLElemColor);
            GlobalVars.SecLElem.Locked = true;

            GlobalVars.SecRElem = Functions.DrawPolygon(SecR, GlobalVars.SecRElemColor);
            GlobalVars.SecRElem.Locked = true;

            pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)SecR;
            SecPoly = (IPolygon)pTopo.Union(SecL);
            pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(SecPoly));
            pTopo.IsKnownSimple_2 = false;
            pTopo.Simplify();
        }

        private void TextBox201_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char eventChar = eventArgs.KeyChar;
            if (eventChar == 13)
                TextBox201_Validating(TextBox201, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox201.Text);

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        private void TextBox201_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            int k = ComboBox101.SelectedIndex;
            double fTmp;

            if (double.TryParse(TextBox201.Text, out fTmp))
            {
                if (TextBox201.Tag.ToString() == TextBox201.Text)
                    return;

                if (FrontList[k].TypeCode == eNavaidType.VOR && FrontList[k].Front)
                    fTmp = NativeMethods.Modulus(fTmp + 180.0);
                else
                    fTmp = NativeMethods.Modulus(fTmp);

                double fTmpMin = NativeMethods.Modulus((double)SpinButton201.Minimum);
                double fTmpMax = NativeMethods.Modulus((double)SpinButton201.Maximum);
                double fdMinMax = Functions.SubtractAngles(fTmpMax, fTmpMin);
                double fdAngMin = Functions.SubtractAngles(fTmp, fTmpMin);
                double fdAngMax = Functions.SubtractAngles(fTmp, fTmpMax);

                if ((fdAngMin > fdMinMax) || (fdAngMax > fdMinMax))
                {
                    if (fdAngMin > fdAngMax)
                        fTmp = fTmpMax;
                    else
                        fTmp = fTmpMin;
                }

                decimal decTmp = System.Convert.ToDecimal(fTmp);
                if (decTmp < SpinButton201.Minimum)
                    decTmp += 360;

                if (SpinButton201.Value != decTmp)
                    SpinButton201.Value = decTmp;
                else if (FrontList[k].TypeCode == eNavaidType.VOR && FrontList[k].Front)
                    TextBox201.Text = NativeMethods.Modulus((double)decTmp + 180.0).ToString();//- GlobalVars.CurrADHP.MagVar 
                else
                    TextBox201.Text = NativeMethods.Modulus((double)decTmp).ToString();//- GlobalVars.CurrADHP.MagVar 

                SpinButton201.Value = decTmp;
            }
            else
            {
                if (FrontList[k].TypeCode == eNavaidType.VOR && FrontList[k].Front)
                    TextBox201.Text = NativeMethods.Modulus(Functions.RoundAngle((double)SpinButton201.Value + 180.0)).ToString();
                else
                    TextBox201.Text = NativeMethods.Modulus(Functions.RoundAngle((double)SpinButton201.Value)).ToString();
            }

            TextBox201.Tag = TextBox201.Text;
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
            double NewR;

            if (double.TryParse(TextBox003.Text, out NewR))
            {
                if (TextBox003.Tag.ToString() == TextBox003.Text)
                    return;

                NewR = Functions.DeConvertDistance(NewR);

                //         If NewR > 60000# Then NewR = 60000#
                if (NewR < RMin)
                {
                    NewR = RMin;
                    TextBox003.Text = Functions.ConvertDistance(NewR, eRoundMode.NEAREST).ToString();
                }
                TextBox003.Tag = TextBox003.Text;
                //if(GlobalVars.RModel == NewR) return
                GlobalVars.RModel = NewR;

                double NewH;
                NewH = NewR * PANS_OPS_DataBase.dpPDG_Nom.Value;
                if (NewH < 600.0) NewH = 600.0;
                //TextBox007.Text = Functions.ConvertHeight(NewH + GlobalVars.CurrADHP.Elev, eRoundMode.rmNERAEST).ToString();
                //TextBox007.Text = Functions.ConvertHeight(NewH + PANS_OPS_DataBase.dpH_abv_DER.Value + DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.rmNERAEST).ToString();

                pCircle = Functions.CreatePrjCircle(ptCenter, GlobalVars.RModel);

                ISimpleFillSymbol pEmptyFillSym = new SimpleFillSymbol();
                IRgbColor pRGB = new RgbColor();
                pRGB.RGB = Functions.RGB(255, 0, 0);

                ISimpleLineSymbol pLineSym = new SimpleLineSymbol();
                pLineSym.Color = pRGB;
                pLineSym.Width = 2.0;

                pEmptyFillSym.Style = esriSimpleFillStyle.esriSFSNull;
                pEmptyFillSym.Outline = pLineSym; // pRedLineSymbol

                Functions.DeleteGraphicsElement(GlobalVars.pCircleElem);
                Functions.DeleteGraphicsElement(GlobalVars.CLElem);

                GlobalVars.pCircleElem = Functions.DrawPolygonSFS(pCircle as IPolygon, pEmptyFillSym, false);
                // ================================
                IPolyline pLine = ((ESRI.ArcGIS.Geometry.IPolyline)(new Polyline()));
                pLine.FromPoint = ((ESRI.ArcGIS.Geometry.IPoint)(DER.pPtPrj[eRWY.PtDER]));
                pLine.ToPoint = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], RWYDir, Functions.DeConvertDistance(double.Parse(TextBox003.Text)));

                ITopologicalOperator pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator)(pCircle));
                pLine = ((ESRI.ArcGIS.Geometry.IPolyline)(pTopo.Intersect(pLine, esriGeometryDimension.esriGeometry1Dimension)));
                // ================================
                pRGB.RGB = 0; // RGB(178, 138, 64)
                              //         pLineSym = New SimpleLineSymbol
                pLineSym.Color = pRGB;
                pLineSym.Style = esriSimpleLineStyle.esriSLSDash;
                pLineSym.Width = 1.0;

                GlobalVars.CLElem = Functions.DrawPolylineSFS(pLine, pLineSym, false);

                if (GlobalVars.ButtonControl1State)
                {
                    pGraphics.AddElement(GlobalVars.pCircleElem, 0);
                    GlobalVars.pCircleElem.Locked = true;
                }

                if (GlobalVars.ButtonControl7State)
                {
                    pGraphics.AddElement(GlobalVars.CLElem, 0);
                    GlobalVars.CLElem.Locked = true;
                }

                if (GlobalVars.ButtonControl1State || GlobalVars.ButtonControl7State)
                    GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                // ================================
                //Functions.RefreshCommandBar(_mTool, 65);

                double MaxDist = DBModule.GetObstListInPoly(out oFullList, ptCenter, GlobalVars.RModel, DER.pPtGeo[eRWY.PtDER].Z);

                Label007.Text = Resources.str15471 + Functions.ConvertDistance(GlobalVars.RModel, eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit + ":";
                TextBox005.Text = oFullList.Obstacles.Length.ToString();

                TextBox006.Text = Functions.ConvertDistance(MaxDist, eRoundMode.FLOOR).ToString();
            }
            else if (double.TryParse(TextBox003.Tag.ToString(), out NewR))
                TextBox003.Text = TextBox003.Tag.ToString();
            else
                TextBox003.Text = Functions.ConvertDistance(GlobalVars.RModel, eRoundMode.NEAREST).ToString();
        }

        private void TextBox1012_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char eventChar = eventArgs.KeyChar;

            if (eventChar == 13)
                TextBox1012_Validating(TextBox1012, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox1012.Text);

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        private void TextBox1012_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            if (TextBox1012.ReadOnly)
                return;

            double newTAPDG;
            if (!double.TryParse(TextBox1012.Text, out newTAPDG))
            {
                MessageBox.Show(Resources.str15473, "PANDA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (TextBox1012.Tag.ToString() == TextBox1012.Text)
                return;

            TACurrPDG = 0.01 * newTAPDG;
            if (TACurrPDG < PANS_OPS_DataBase.dpPDG_Nom.Value)
                TACurrPDG = PANS_OPS_DataBase.dpPDG_Nom.Value;

            int indx;
            double hTurn = fZNRLen * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value;
            hKK = hTurn + DER.pPtPrj[eRWY.PtDER].Z;

            TextBox1014.Text = Functions.ConvertHeight(hKK, eRoundMode.NEAREST).ToString();

            double L1 = Functions.ReturnDistanceInMeters(DER.pPtPrj[eRWY.PtDER], TurnFixPnt);
            AltitudeAtTurnFixPnt = CurrPDG * L1 + PANS_OPS_DataBase.dpH_abv_DER.Value + DER.pPtPrj[eRWY.PtDER].Z;

            double dTNAh = Functions.CalcTA_Hpenet(oTrnAreaList.Parts, hTurn, TACurrPDG, out indx, -hTurn + PANS_OPS_DataBase.dpObsClr.Value);

            // If dTNAh < 0 Then dTNAh = 0
            double NewTIA_PDG = (hTurn + dTNAh - PANS_OPS_DataBase.dpH_abv_DER.Value) / (hTurn - PANS_OPS_DataBase.dpH_abv_DER.Value) * CurrPDG;
            if (NewTIA_PDG < MinCurrPDG)
                NewTIA_PDG = MinCurrPDG;

            CalcNewStraightAreaWithFixedLength(ref NewTIA_PDG, ref newTAPDG);

            if (TACurrPDG < newTAPDG)
                TACurrPDG = newTAPDG;

            TextBox1011.Text = Math.Round(100.0 * CurrPDG, 1).ToString();
            TextBox1012.Text = Math.Round(100.0 * TACurrPDG, 1).ToString();
            TextBox1012.Tag = TextBox1012.Text;

            // =====================================================================================
            IPoint ptCnt;
            if (OptionButton701.Checked)
            {
                IPoint ptTmp = MPtCollection.Point[MPtCollection.PointCount - 1];
                Functions.CircleVectorIntersect(GlobalVars.CurrADHP.pPtPrj, GlobalVars.RModel, ptTmp, ptTmp.M, out ptCnt);
            }
            else if (OptionButton702.Checked)
                ptCnt = TerFixPnt;
            else
                ptCnt = TurnDirector.pPtPrj;

            IProximityOperator pProxi;
            if (OptionButton401.Checked)
                pProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)ZNR_Poly;
            else
                pProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)KK;

            double DistToFix = pProxi.ReturnDistance(ptCnt);

            fReachedA = hKK + DistToFix * TACurrPDG;
            TextBox1006.Text = Functions.ConvertHeight(fReachedA, eRoundMode.NEAREST).ToString();
            //TextBox1016.Text = TextBox1006.Text;
            TextBox1016_Validating(TextBox1016, null);
            // =====================================================================================

            IPointCollection mPoly = (IPointCollection)Functions.CalcTrajectoryFromMultiPoint(MPtCollection);
            mPoly.AddPoint(ptCnt);
            NomInfoFrm.SetInfo(mPoly, hKK, TACurrPDG);
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
            double fTmp;
            if (!double.TryParse(TextBox401.Text, out fTmp))
                return;

            if (TextBox401.Tag.ToString() == TextBox401.Text)
                return;

            double speed = Functions.DeConvertSpeed(fTmp);
            fIAS = speed;

            if (fIAS < 1.1 * Categories_DATABase.cVmaInter.Values[AirCat])
                fIAS = 1.1 * Categories_DATABase.cVmaInter.Values[AirCat];
            else if (fIAS > 1.1 * Categories_DATABase.cVmaFaf.Values[AirCat])
                fIAS = 1.1 * Categories_DATABase.cVmaFaf.Values[AirCat];

            if (fIAS != speed)
                TextBox401.Text = Functions.ConvertSpeed(fIAS, eRoundMode.NEAREST).ToString();

            TextBox401.Tag = TextBox401.Text;
        }

        private void TextBox402_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char eventChar = eventArgs.KeyChar;

            if (eventChar == 13)
                TextBox402_Validating(TextBox402, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox402.Text);

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        private void TextBox402_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            double fTmp;
            if (!double.TryParse(TextBox402.Text, out fTmp))
                return;

            if (TextBox402.Tag.ToString() == TextBox402.Text)
                return;

            fBankAngle = fTmp;
            if (fBankAngle < 1.0)
                fBankAngle = 1.0;

            if (fBankAngle < PANS_OPS_DataBase.dpT_Bank.Value - 5)
                fBankAngle = PANS_OPS_DataBase.dpT_Bank.Value - 5;

            if (fBankAngle > PANS_OPS_DataBase.dpT_Bank.Value + 10)
                fBankAngle = PANS_OPS_DataBase.dpT_Bank.Value + 10;

            if (fTmp != fBankAngle)
                TextBox402.Text = fBankAngle.ToString();
            TextBox402.Tag = TextBox402.Text;
        }

        private void TextBox505_KeyPress(System.Object sender, KeyPressEventArgs e)
        {
            char eventChar = e.KeyChar;

            if (eventChar == 13)
                TextBox505_Validating(TextBox505, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox505.Text);

            e.KeyChar = eventChar;
            if (eventChar == 0)
                e.Handled = true;
        }

        private void TextBox505_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            double fTmp;

            if (!double.TryParse(TextBox505.Text, out fTmp))
            {
                if (double.TryParse(TextBox505.Tag.ToString(), out fTmp))
                    TextBox505.Text = TextBox505.Tag.ToString();
                return;
            }

            fTmp *= 0.01;

            if (fTmp > PANS_OPS_DataBase.dpMaxPosPDG.Value * 50.0)
            {
                fTmp = PANS_OPS_DataBase.dpMaxPosPDG.Value * 50.0;
                TextBox505.Text = Math.Round(100.0 * fTmp, 1).ToString();
            }

            if (fTmp < PANS_OPS_DataBase.dpPDG_Nom.Value)
            {
                fTmp = PANS_OPS_DataBase.dpPDG_Nom.Value;
                TextBox505.Text = Math.Round(100.0 * fTmp, 1).ToString();
            }
        }

        private void TextBox602_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char eventChar = eventArgs.KeyChar;

            if (eventChar == 13)
                TextBox602_Validating(TextBox602, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox602.Text);

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        private void TextBox602_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            double iniH, fTASl, Range = (hMaxTurn - PANS_OPS_DataBase.dpH_abv_DER.Value) / CurrPDG;

            if (double.TryParse(TextBox602.Text, out iniH) && OptionButton401.Checked)
            {
                if (TextBox602.Tag.ToString() == TextBox602.Text)
                    return;

                iniH = Functions.DeConvertHeight(iniH);

                //         If (ComboBox603.ListIndex = 1) Then
                //             iniH = iniH * FootCoeff
                //         End If
                //         iniH = iniH + ptThrPrj.Z - DER.pPtPrj(ptEnd).Z

                if (iniH > hMaxTurn)
                    iniH = hMaxTurn;

                if (iniH < 120.0)
                    iniH = 120.0;

                if (iniH < hMinTurn)
                    iniH = hMinTurn;

                ObstacleContainer TmpInList;

                if (Range == GlobalVars.RModel)
                {
                    int nN = oZNRList.Obstacles.Length;
                    TmpInList.Obstacles = new Obstacle[nN];
                    System.Array.Copy(oZNRList.Obstacles, TmpInList.Obstacles, nN);

                    nN = oZNRList.Parts.Length;
                    TmpInList.Parts = new ObstacleData[nN];
                    System.Array.Copy(oZNRList.Parts, TmpInList.Parts, nN);
                }
                else
                    Functions.GetObstInRange(oZNRList, out TmpInList, Range);

                double newH = Functions.CalcTIAMinTNAH(TmpInList.Parts, CurrPDG, out Range, iniH);

                if (newH > hMaxTurn)
                {
                    newH = Functions.CalcTIAMaxTNAH(TmpInList.Parts, CurrPDG, ref Range, iniH);
                    if (newH < hMinTurn)
                        newH = hMinTurn;
                }

                double hTurn = newH + DER.pPtPrj[eRWY.PtDER].Z;
                Range = (newH - PANS_OPS_DataBase.dpH_abv_DER.Value) / CurrPDG;
                Functions.GetObstInRange(oZNRList, out TmpInList, Range);
                ReportsFrm.FillPage4(TmpInList, OptionButton401.Checked);

                // ============================================================
                fTASl = Functions.IAS2TAS(fIAS, hTurn, GlobalVars.CurrADHP.ISAtC);
                double VTotal = fTASl + GlobalVars.CurrADHP.WindSpeed;
                double Ls = VTotal * PANS_OPS_DataBase.dpT_TechToleranc.Value * 0.277777777777778;

                IClone pSource = (ESRI.ArcGIS.esriSystem.IClone)UnitedPolygon;
                IPointCollection pPolyClone = (ESRI.ArcGIS.Geometry.IPointCollection)pSource.Clone();
                ITopologicalOperator2 pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)pPolyClone;
                pTopo.IsKnownSimple_2 = false;
                pTopo.Simplify();

                // ====================
                double L0 = (newH - PANS_OPS_DataBase.dpOIS_abv_DER.Value) / CurrPDG;
                IPoint ptTmp = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir, L0);

                IPointCollection pLine = new Polyline();
                pLine.AddPoint(Functions.PointAlongPlane(ptTmp, DepDir - 90.0, GlobalVars.RModel));
                pLine.AddPoint(Functions.PointAlongPlane(ptTmp, DepDir + 90.0, GlobalVars.RModel));
                KK = pTopo.Intersect(pLine as IGeometry, esriGeometryDimension.esriGeometry1Dimension) as IPolyline;

                if (Functions.SideDef(KK.ToPoint, DepDir, KK.FromPoint) < 0)
                    KK.ReverseOrientation();

                // ==========================
                TurnFixPnt = new ESRI.ArcGIS.Geometry.Point();
                IConstructPoint pConstruct = ((ESRI.ArcGIS.Geometry.IConstructPoint)(TurnFixPnt));
                pConstruct.ConstructAngleIntersection(PtDerShift, GlobalVars.DegToRadValue * DepDir, KK.ToPoint, GlobalVars.DegToRadValue * (DepDir + 90.0));
                TurnFixPnt.Z = hTurn;
                TurnFixPnt.M = DepDir;

                ptTmp = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir, L0 + Ls);
                pLine.RemovePoints(0, 2);
                pLine.AddPoint(Functions.PointAlongPlane(ptTmp, DepDir - 90.0, GlobalVars.RModel));
                pLine.AddPoint(Functions.PointAlongPlane(ptTmp, DepDir + 90.0, GlobalVars.RModel));

                K1K1 = pTopo.Intersect(pLine as IGeometry, esriGeometryDimension.esriGeometry1Dimension) as IPolyline;

                if (Functions.SideDef(K1K1.ToPoint, DepDir, K1K1.FromPoint) < 0)
                    K1K1.ReverseOrientation();

                // ============================================================
                TextBox602.Text = Functions.ConvertHeight(newH, eRoundMode.NEAREST).ToString();
                TextBox602.Tag = TextBox602.Text;

                TextBox603.Text = Functions.ConvertHeight(newH + DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.NEAREST).ToString();
                TextBox603.Tag = TextBox603.Text;
                // ============================================================
                IPointCollection mPoly = new Polyline();
                mPoly.AddPoint(PtDerShift);
                mPoly.AddPoint(TurnFixPnt);

                Functions.DeleteGraphicsElement(GlobalVars.StrTrackElem);
                GlobalVars.StrTrackElem = Functions.DrawPolyline(mPoly, GlobalVars.StrTrackElemColor, 1.0, false);

                // ============================================================
                if (GlobalVars.FIXElem != null)
                {
                    Functions.DeleteGraphicsElement(GlobalVars.FIXElem);

                    if (GlobalVars.ButtonControl8State)
                    {
                        if (GlobalVars.FIXElem is IGroupElement)
                        {
                            IGroupElement pGroupElement = (ESRI.ArcGIS.Carto.IGroupElement)GlobalVars.FIXElem;
                            for (int i = 0; i < pGroupElement.ElementCount; i++)
                            {
                                pGraphics.AddElement(pGroupElement.Element[i], 0);
                                pGroupElement.Element[i].Locked = true;
                            }
                        }
                        else
                        {
                            pGraphics.AddElement(GlobalVars.FIXElem, 0);
                            GlobalVars.FIXElem.Locked = true;
                        }
                        GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                    }
                }

                if (GlobalVars.ButtonControl5State)
                {
                    pGraphics.AddElement(GlobalVars.StrTrackElem, 0);
                    GlobalVars.StrTrackElem.Locked = true;
                }
                //Functions.RefreshCommandBar(_mTool, 128);
            }

            fTASl = Functions.IAS2TAS(fIAS, TurnFixPnt.Z, GlobalVars.CurrADHP.ISAtC);
            Text602.Text = Functions.ConvertSpeed(fIAS, eRoundMode.NEAREST).ToString();
            Text603.Text = Functions.ConvertSpeed(fTASl, eRoundMode.NEAREST).ToString();
            Text604.Text = Functions.ConvertDistance(PANS_OPS_DataBase.dpT_TechToleranc.Value / 3.6 * (GlobalVars.CurrADHP.WindSpeed + fTASl), eRoundMode.NEAREST).ToString();

            double Rv = 6355.0 * System.Math.Tan(GlobalVars.DegToRadValue * fBankAngle) / (GlobalVars.PI * fTASl);
            if (Rv > 3.0)
                Rv = 3.0;

            Text605.Text = System.Math.Round(Rv, 2).ToString();

            if (Rv > 0.0)
            {
                double r0 = fTASl / (0.02 * GlobalVars.PI * Rv);
                Text606.Text = Functions.ConvertDistance(r0, eRoundMode.NEAREST).ToString();
            }
            else
                Text606.Text = "-";

            double E = 25.0 * GlobalVars.CurrADHP.WindSpeed / Rv;
            Text607.Text = Functions.ConvertDistance(E, eRoundMode.NEAREST).ToString();

            if (OptionButton401.Checked)
                Text601.Text = Functions.ConvertDistance(Range, eRoundMode.NEAREST).ToString();
            else
            {
                IProximityOperator pProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)KK;
                Text601.Text = Functions.ConvertDistance(pProxi.ReturnDistance(DER.pPtPrj[eRWY.PtDER]), eRoundMode.NEAREST).ToString();

                //Dist = Functions.Point2LineDistancePrj(DER.pPtPrj[eRWY.PtDER], TurnFixPnt, DepDir + 90.0);
                //hTurn = PANS_OPS_DataBase.dpH_abv_DER.Value + PANS_OPS_DataBase.dpOv_Nav_PDG.Value * Dist + DER.pPtPrj[eRWY.PtDER].Z;
                Text608.Text = Functions.ConvertHeight(TurnFixPnt.Z, eRoundMode.NEAREST).ToString();

                double L1 = Functions.ReturnDistanceInMeters(DER.pPtPrj[eRWY.PtDER], TurnFixPnt);
                double TurnFixPntAltitude = CurrPDG * L1 + DER.pPtPrj[eRWY.PtDER].Z + PANS_OPS_DataBase.dpH_abv_DER.Value;
                Text609.Text = Functions.ConvertHeight(TurnFixPntAltitude, eRoundMode.NEAREST).ToString();

                Text610.Text = Functions.ConvertDistance(pProxi.ReturnDistance(KKFixMax), eRoundMode.NEAREST).ToString();
            }
        }

        private void TextBox603_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char eventChar = eventArgs.KeyChar;

            if (eventChar == 13)
                TextBox603_Validating(TextBox603, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox603.Text);

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        private void TextBox603_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            double iniH, fTASl, Range = (hMaxTurn - PANS_OPS_DataBase.dpH_abv_DER.Value) / CurrPDG;

            if (double.TryParse(TextBox603.Text, out iniH) && OptionButton401.Checked)
            {
                if (TextBox603.Tag.ToString() == TextBox603.Text)
                    return;

                iniH = Functions.DeConvertHeight(iniH);

                //         If (ComboBox604.ListIndex = 1) Then
                //             iniH = iniH * FootCoeff
                //         End If

                iniH = iniH - DER.pPtPrj[eRWY.PtDER].Z;

                if (iniH > hMaxTurn)
                    iniH = hMaxTurn;

                if (iniH < 120.0)
                    iniH = 120.0;

                if (iniH < hMinTurn)
                    iniH = hMinTurn;

                ObstacleContainer TmpInList;

                if (Range == GlobalVars.RModel)
                {
                    int nN = oZNRList.Obstacles.Length;
                    TmpInList.Obstacles = new Obstacle[nN];
                    System.Array.Copy(oZNRList.Obstacles, TmpInList.Obstacles, nN);

                    nN = oZNRList.Parts.Length;
                    TmpInList.Parts = new ObstacleData[nN];
                    System.Array.Copy(oZNRList.Parts, TmpInList.Parts, nN);
                }
                else
                    Functions.GetObstInRange(oZNRList, out TmpInList, Range);

                double newH = Functions.CalcTIAMinTNAH(TmpInList.Parts, CurrPDG, out Range, iniH);

                if (newH > hMaxTurn)
                {
                    newH = Functions.CalcTIAMaxTNAH(TmpInList.Parts, CurrPDG, ref Range, iniH);
                    if (newH < hMinTurn)
                        newH = hMinTurn;
                }

                double hTurn = newH + DER.pPtPrj[eRWY.PtDER].Z;
                Range = (newH - PANS_OPS_DataBase.dpH_abv_DER.Value) / CurrPDG;
                Functions.GetObstInRange(oZNRList, out TmpInList, Range);
                ReportsFrm.FillPage4(TmpInList, OptionButton401.Checked);
                // ============================================================

                fTASl = Functions.IAS2TAS(fIAS, hTurn, GlobalVars.CurrADHP.ISAtC);
                double VTotal = fTASl + GlobalVars.CurrADHP.WindSpeed;
                double Ls = VTotal * PANS_OPS_DataBase.dpT_TechToleranc.Value * 0.277777777777778;

                IClone pSource = (ESRI.ArcGIS.esriSystem.IClone)UnitedPolygon;
                IPointCollection pPolyClone = (ESRI.ArcGIS.Geometry.IPointCollection)pSource.Clone();
                ITopologicalOperator2 pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)pPolyClone;
                pTopo.IsKnownSimple_2 = false;
                pTopo.Simplify();

                // ====================
                double L0 = (newH - PANS_OPS_DataBase.dpOIS_abv_DER.Value) / CurrPDG;
                IPoint ptTmp = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir, L0);

                IPointCollection pLine = new Polyline();
                pLine.AddPoint(Functions.PointAlongPlane(ptTmp, DepDir - 90.0, GlobalVars.RModel));
                pLine.AddPoint(Functions.PointAlongPlane(ptTmp, DepDir + 90.0, GlobalVars.RModel));

                KK = (IPolyline)pTopo.Intersect(pLine, esriGeometryDimension.esriGeometry1Dimension);
                if (Functions.SideDef(KK.ToPoint, DepDir, KK.FromPoint) < 0)
                    KK.ReverseOrientation();
                // ==========================

                TurnFixPnt = new ESRI.ArcGIS.Geometry.Point();
                IConstructPoint pConstruct = (ESRI.ArcGIS.Geometry.IConstructPoint)TurnFixPnt;
                pConstruct.ConstructAngleIntersection(PtDerShift, GlobalVars.DegToRadValue * DepDir, KK.ToPoint, GlobalVars.DegToRadValue * (DepDir + 90.0));
                TurnFixPnt.Z = hTurn;
                TurnFixPnt.M = DepDir;

                ptTmp = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir, L0 + Ls);
                pLine.RemovePoints(0, 2);
                pLine.AddPoint(Functions.PointAlongPlane(ptTmp, DepDir - 90.0, GlobalVars.RModel));
                pLine.AddPoint(Functions.PointAlongPlane(ptTmp, DepDir + 90.0, GlobalVars.RModel));

                K1K1 = (IPolyline)pTopo.Intersect(pLine, esriGeometryDimension.esriGeometry1Dimension);

                if (Functions.SideDef(K1K1.ToPoint, DepDir, K1K1.FromPoint) < 0)
                    K1K1.ReverseOrientation();

                // ============================================================
                TextBox602.Text = Functions.ConvertHeight(newH, eRoundMode.NEAREST).ToString();
                TextBox602.Tag = TextBox602.Text;

                TextBox603.Text = Functions.ConvertHeight(newH + DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.NEAREST).ToString();
                TextBox603.Tag = TextBox603.Text;
                // ============================================================
                IPointCollection mPoly = new Polyline();
                mPoly.AddPoint(PtDerShift);
                mPoly.AddPoint(TurnFixPnt);

                Functions.DeleteGraphicsElement(GlobalVars.StrTrackElem);
                GlobalVars.StrTrackElem = Functions.DrawPolyline(mPoly, GlobalVars.StrTrackElemColor, 1.0, false);
                // ============================================================

                Functions.DeleteGraphicsElement(GlobalVars.FIXElem);

                if (GlobalVars.FIXElem != null)
                {
                    if (GlobalVars.ButtonControl8State)
                    {
                        if (GlobalVars.FIXElem is IGroupElement)
                        {
                            IGroupElement pGroupElement = ((ESRI.ArcGIS.Carto.IGroupElement)(GlobalVars.FIXElem));
                            for (int i = 0; i <= pGroupElement.ElementCount - 1; i++)
                            {
                                pGraphics.AddElement(pGroupElement.get_Element(i), 0);
                                pGroupElement.get_Element(i).Locked = true;
                            }
                        }
                        else
                        {
                            pGraphics.AddElement(GlobalVars.FIXElem, 0);
                            GlobalVars.FIXElem.Locked = true;
                        }
                        GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                    }
                }

                if (GlobalVars.ButtonControl5State)
                {
                    pGraphics.AddElement(GlobalVars.StrTrackElem, 0);
                    GlobalVars.StrTrackElem.Locked = true;
                }

                //Functions.RefreshCommandBar(_mTool, 128);
            }

            fTASl = Functions.IAS2TAS(fIAS, TurnFixPnt.Z, GlobalVars.CurrADHP.ISAtC);

            Text602.Text = Functions.ConvertSpeed(fIAS, eRoundMode.NEAREST).ToString();
            Text603.Text = Functions.ConvertSpeed(fTASl, eRoundMode.NEAREST).ToString();
            Text604.Text = Functions.ConvertDistance(PANS_OPS_DataBase.dpT_TechToleranc.Value / 3.6 * (GlobalVars.CurrADHP.WindSpeed + fTASl), eRoundMode.NEAREST).ToString();

            double Rv = 6355.0 * System.Math.Tan(GlobalVars.DegToRadValue * fBankAngle) / (GlobalVars.PI * fTASl);
            if (Rv > 3.0)
                Rv = 3.0;

            Text605.Text = System.Math.Round(Rv, 2).ToString();

            if (Rv > 0.0)
            {
                double r0 = fTASl / (0.02 * GlobalVars.PI * Rv);
                Text606.Text = Functions.ConvertDistance(r0, eRoundMode.NEAREST).ToString();
            }
            else
                Text606.Text = "-";

            double E = 25.0 * GlobalVars.CurrADHP.WindSpeed / Rv;
            Text607.Text = Functions.ConvertDistance(E, eRoundMode.NEAREST).ToString();

            if (OptionButton401.Checked)
                Text601.Text = Functions.ConvertDistance(Range, eRoundMode.NEAREST).ToString();
            else
            {
                IProximityOperator pProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)KK;
                Text601.Text = Functions.ConvertDistance(pProxi.ReturnDistance(DER.pPtPrj[eRWY.PtDER]), eRoundMode.NEAREST).ToString();

                //Dist = Functions.Point2LineDistancePrj(DER.pPtPrj[eRWY.PtDER], TurnFixPnt, DepDir + 90.0);
                //hTurn = PANS_OPS_DataBase.dpH_abv_DER.Value + PANS_OPS_DataBase.dpOv_Nav_PDG.Value * Dist + DER.pPtPrj[eRWY.PtDER].Z;
                Text608.Text = Functions.ConvertHeight(TurnFixPnt.Z, eRoundMode.NEAREST).ToString();

                double L1 = Functions.ReturnDistanceInMeters(DER.pPtPrj[eRWY.PtDER], TurnFixPnt);
                double TurnFixPntAltitude = CurrPDG * L1 + DER.pPtPrj[eRWY.PtDER].Z + PANS_OPS_DataBase.dpH_abv_DER.Value;
                Text609.Text = Functions.ConvertHeight(TurnFixPntAltitude, eRoundMode.NEAREST).ToString();

                Text610.Text = Functions.ConvertDistance(pProxi.ReturnDistance(KKFixMax), eRoundMode.NEAREST).ToString();
            }
        }

        private void TextBox701_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char eventChar = eventArgs.KeyChar;

            if (eventChar == 13)
                TextBox701_Validating(TextBox701, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox701.Text);

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        private void TextBox701_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            if (TextBox701.Tag.ToString() == TextBox701.Text)
                return;

            double fTmp;
            if (!double.TryParse(TextBox701.Text, out fTmp))
            {
                if (double.TryParse(TextBox701.Tag.ToString(), out fTmp))
                    TextBox701.Text = TextBox701.Tag.ToString();
                else
                {
                    TextBox701.Text = "0";
                    fTmp = 0.0;
                }
            }

            if (fTmp > 2.5 * PANS_OPS_DataBase.dpT_TechToleranc.Value)
            {
                TextBox701.Text = (2.5 * PANS_OPS_DataBase.dpT_TechToleranc.Value).ToString();
                TextBox701.Tag = TextBox701.Text;
            }

            TextBox703.Tag = "a";
            TextBox703_Validating(TextBox703, new System.ComponentModel.CancelEventArgs());
            TextBox701.Tag = TextBox701.Text;
        }

        private void TextBox703_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char eventChar = eventArgs.KeyChar;

            if (eventChar == 13)
                TextBox703_Validating(TextBox703, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox703.Text);

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        private void TextBox703_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            double fTmp;
            if (!double.TryParse(TextBox703.Text, out fTmp))
            {
                if (double.TryParse(TextBox703.Tag.ToString(), out fTmp))
                    TextBox703.Text = TextBox703.Tag.ToString();
                else
                {
                    fTmp = PANS_OPS_DataBase.dpInterMinAngle.Value;
                    TextBox703.Text = fTmp.ToString();
                }
            }

            if (fTmp < PANS_OPS_DataBase.dpInterMinAngle.Value - PANS_OPS_DataBase.dpFlightTechTol.Value)
                TextBox703.Text = (PANS_OPS_DataBase.dpInterMinAngle.Value - PANS_OPS_DataBase.dpFlightTechTol.Value).ToString();
            else if (fTmp > PANS_OPS_DataBase.dpInterMaxAngle.Value + PANS_OPS_DataBase.dpFlightTechTol.Value)
                TextBox703.Text = (PANS_OPS_DataBase.dpInterMaxAngle.Value + PANS_OPS_DataBase.dpFlightTechTol.Value).ToString();

            if (MultiPage1.SelectedIndex >= 5)
            {
                if (TextBox703.Tag.ToString() == TextBox703.Text)
                    return;

                UpdateIntervals(false);

                TextBox702.Tag = "a";
                TextBox702_Validating(TextBox702, new System.ComponentModel.CancelEventArgs());

                TextBox703.Tag = TextBox703.Text;
            }
        }

        private void TextBox702_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char eventChar = eventArgs.KeyChar;

            if (eventChar == 13)
                TextBox702_Validating(TextBox702, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox702.Text);

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        private void TextBox702_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            if (ComboBox703.SelectedIndex == 0 || !OptionButton702.Checked)
            {
                double fTmp;
                if (!double.TryParse(TextBox702.Text, out fTmp))
                {
                    if (double.TryParse(TextBox702.Tag.ToString(), out fTmp))
                        TextBox702.Text = TextBox702.Tag.ToString();
                    return;
                }

                if (MultiPage1.SelectedIndex < 6 || TextBox702.Tag.ToString() == TextBox702.Text)
                    return;

                DirCourse = Functions.Azt2Dir(DER.pPtGeo[eRWY.PtDER], fTmp + GlobalVars.CurrADHP.MagVar);
            }

            if (OptionButton702.Checked)
            {
                if (UpdateToNavCurs(DirCourse) == -1)
                {
                    TextBox702.Text = TextBox702.Tag.ToString();
                    eventArgs.Cancel = true;
                }
                else
                {
                    TextBox702.Tag = TextBox702.Text;
                    eventArgs.Cancel = false;
                }
            }
            else if (OptionButton701.Checked)
            {
                UpdateToCourse(DirCourse);
                TextBox702.Tag = TextBox702.Text;
                eventArgs.Cancel = false;
            }
        }

        private void TextBox601_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char eventChar = eventArgs.KeyChar;

            if (OptionButton401.Checked)
                goto EventExitSub;

            if (eventChar == 13)
                TextBox601_Validating(TextBox601, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox601.Text);

            EventExitSub:

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        private void TextBox601_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            IPointCollection pPolyClone = null;
            IPolygon pPoly = null;
            IPointCollection pSect0 = null;
            IPointCollection pSect1 = null;
            IPointCollection pLine = null;

            IGroupElement pGroupElement = null;
            IGroupElement pGroupElem = null;
            IElement pElement = null;
            IClone Clone = null;

            ITopologicalOperator2 pTopo = null;
            IConstructPoint pConstruct = null;
            IPolyline pCutter = null;

            IPoint Pt1 = null;
            IPoint Pt2 = null;

            double DepDistMax = 0;
            double DepDistMin = 0;
            double InterToler = 0;
            double VTotal;
            double hTurn;
            double fTASl;
            double hFix;
            double fTmp;
            double fDir;
            double fDis = 0;
            double dMin;
            double dMax;
            double Ls;
            double d0;
            double d;

            int i, k, l, n;

            if (!double.TryParse(TextBox601.Text, out fDir))
                return;

            //     If TextBox601.Tag = TextBox601.Text Then Return

            k = ComboBox601.SelectedIndex;
            l = ComboBox101.SelectedIndex;

            if (TurnInterDat[k].TypeCode == eNavaidType.DME)
            {
                fDir = Functions.DeConvertDistance(fDir);
                fTmp = fDir;
                System.Array transTemp78 = TurnInterDat[k].ValMin;
                n =  /* TRANSINFO: .NET Equivalent of Microsoft.VisualBasic NameSpace */ transTemp78.GetUpperBound(0);
                if (Option601.Checked || n == 0)
                {
                    dMin = TurnInterDat[k].ValMin[0];
                    dMax = TurnInterDat[k].ValMax[0];
                }
                else
                {
                    dMin = TurnInterDat[k].ValMin[1];
                    dMax = TurnInterDat[k].ValMax[1];
                }

                if (fTmp < dMin)
                    fTmp = dMin;

                if (fTmp > dMax)
                    fTmp = dMax;

                if (fTmp != fDir)
                {
                    fDir = fTmp;
                    TextBox601.Text = Functions.ConvertDistance(fDir, eRoundMode.NEAREST).ToString();
                }
            }
            else
            {
                fDir += GlobalVars.CurrADHP.MagVar;
                if (TurnInterDat[k].TypeCode == eNavaidType.VOR)
                {
                    InterToler = Navaids_DataBase.VOR.IntersectingTolerance;
                    fDis = Navaids_DataBase.VOR.Range;
                }
                else
                {
                    InterToler = Navaids_DataBase.NDB.IntersectingTolerance;
                    fDis = Navaids_DataBase.NDB.Range;
                    fDir = fDir + 180.0;
                }

                if (!(Functions.AngleInSector(fDir, TurnInterDat[k].ValMin[0], TurnInterDat[k].ValMax[0])))
                {
                    if (TurnInterDat[k].ValCnt > 0)
                        fDir = TurnInterDat[k].ValMin[0];
                    else
                        fDir = TurnInterDat[k].ValMax[0];

                    fTmp = fDir;
                    if (TurnInterDat[k].TypeCode == eNavaidType.NDB)
                        fTmp = fTmp + 180.0;

                    TextBox601.Text = NativeMethods.Modulus(fTmp - GlobalVars.CurrADHP.MagVar).ToString();
                }
            }

            pGroupElem = ((ESRI.ArcGIS.Carto.IGroupElement)(new GroupElement()));

            if (GlobalVars.FIXElem != null)
            {
                Functions.DeleteGraphicsElement(GlobalVars.FIXElem);

                if (GlobalVars.FIXElem is IGroupElement)
                {
                    pGroupElement = ((ESRI.ArcGIS.Carto.IGroupElement)(GlobalVars.FIXElem));
                    pGroupElem.AddElement(pGroupElement.get_Element(0));
                }
                else
                    pGroupElem.AddElement(GlobalVars.FIXElem);
            }

            switch (TurnInterDat[k].TypeCode)
            {
                case eNavaidType.VOR:
                case eNavaidType.NDB:
                    fDir = Functions.Azt2Dir(DER.pPtGeo[eRWY.PtDER], fDir);

                    pSect0 = new Polyline();
                    Pt1 = Functions.PointAlongPlane(TurnInterDat[k].pPtPrj, fDir + InterToler, fDis);
                    Pt2 = Functions.PointAlongPlane(TurnInterDat[k].pPtPrj, fDir - InterToler, fDis);
                    TurnFixPnt = new ESRI.ArcGIS.Geometry.Point();
                    pConstruct = ((ESRI.ArcGIS.Geometry.IConstructPoint)(TurnFixPnt));

                    pConstruct.ConstructAngleIntersection(TurnInterDat[k].pPtPrj, GlobalVars.DegToRadValue * fDir, FrontList[l].pPtPrj, GlobalVars.DegToRadValue * DepDir);
                    pSect0.AddPoint(Pt1);
                    pSect0.AddPoint(TurnInterDat[k].pPtPrj);
                    pSect0.AddPoint(Pt2);

                    pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(pCircle));
                    pSect0 = pTopo.Intersect(pSect0 as IGeometry, esriGeometryDimension.esriGeometry1Dimension) as IPointCollection;
                    pGroupElem.AddElement(Functions.DrawPolyline(pSect0, Functions.RGB(0, 0, 255), 1, false));

                    DepDistMax = GlobalVars.RModel;
                    DepDistMin = 0.0;
                    break;
                case eNavaidType.DME:
                    if ((TurnInterDat[k].ValCnt < 0) || (Option602.Enabled && Option602.Checked))
                        Functions.CircleVectorIntersect(TurnInterDat[k].pPtPrj, fDir, FrontList[l].pPtPrj, DepDir, out TurnFixPnt);
                    else
                        Functions.CircleVectorIntersect(TurnInterDat[k].pPtPrj, fDir, FrontList[l].pPtPrj, DepDir + 180.0, out TurnFixPnt);

                    fDis = Functions.Point2LineDistancePrj(TurnFixPnt, DER.pPtPrj[eRWY.PtDER], DepDir + 90.0);
                    hFix = fDis * PANS_OPS_DataBase.dpOv_Nav_PDG.Value + PANS_OPS_DataBase.dpH_abv_DER.Value - TurnInterDat[k].pPtPrj.Z + DER.pPtPrj[eRWY.PtDER].Z;

                    d0 = System.Math.Sqrt(fDir * fDir + hFix * hFix) * Navaids_DataBase.DME.ErrorScalingUp + Navaids_DataBase.DME.MinimalError;

                    d = fDir + d0;
                    pSect0 = (IPointCollection)Functions.CreatePrjCircle(TurnInterDat[k].pPtPrj, d);

                    d = fDir - d0;
                    pSect1 = (IPointCollection)Functions.CreatePrjCircle(TurnInterDat[k].pPtPrj, d);

                    pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(pSect0));
                    pPoly = (IPolygon)pTopo.Difference(pSect1 as IGeometry);

                    pCutter = ((ESRI.ArcGIS.Geometry.IPolyline)(new Polyline()));
                    pCutter.FromPoint = Functions.PointAlongPlane(TurnInterDat[k].pPtPrj, DepDir - 90.0, fDir + fDir);
                    pCutter.ToPoint = Functions.PointAlongPlane(TurnInterDat[k].pPtPrj, DepDir + 90.0, fDir + fDir);

                    if (Functions.SideDef(pCutter.FromPoint, DepDir, pCutter.ToPoint) > 0)
                        pCutter.ReverseOrientation();

                    pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(pPoly));

                    if ((TurnInterDat[k].ValCnt < 0) || (Option602.Enabled && Option602.Checked))
                        pTopo.Cut(pCutter, out pSect1, out pSect0);
                    else
                        pTopo.Cut(pCutter, out pSect0, out pSect1);

                    pGroupElem.AddElement(Functions.DrawPolygon(pSect0, Functions.RGB(0, 0, 255), esriSimpleFillStyle.esriSFSNull, false));
                    DepDistMax = (hMaxTurn - PANS_OPS_DataBase.dpH_abv_DER.Value) / CurrPDG + 10.0;
                    DepDistMin = (hMinTurn - PANS_OPS_DataBase.dpH_abv_DER.Value) / CurrPDG - 10.0;
                    break;
            }


            Clone = ((ESRI.ArcGIS.esriSystem.IClone)(UnitedPolygon));
            pPolyClone = ((ESRI.ArcGIS.Geometry.IPointCollection)(Clone.Clone()));
            pFIXPoly = ((ESRI.ArcGIS.Geometry.IPointCollection)(Clone.Clone()));

            pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(pPolyClone));
            pTopo.IsKnownSimple_2 = false;
            pTopo.Simplify();

            pLine = pTopo.Intersect(pSect0 as IGeometry, esriGeometryDimension.esriGeometry1Dimension) as IPointCollection;

            if ((pLine.PointCount == 0) && (TurnInterDat[k].TypeCode == eNavaidType.DME))
                pLine = pTopo.Intersect(pSect0 as IGeometry, esriGeometryDimension.esriGeometry2Dimension) as IPointCollection;

            dMax = -5 * GlobalVars.RModel;
            dMin = 5 * GlobalVars.RModel;

            for (i = 0; i <= pLine.PointCount - 1; i++)
            {
                d = Functions.Point2LineDistancePrj(DER.pPtPrj[eRWY.PtDER], pLine.get_Point(i), DepDir + 90.0);
                if ((d >= DepDistMin) && (d < dMin))
                {
                    dMin = d;
                    Pt1 = pLine.get_Point(i);
                }

                if ((d <= DepDistMax) && (d > dMax))
                {
                    dMax = d;
                    Pt2 = pLine.get_Point(i);
                }
            }

            if (dMax <= dMin)
            {
                MessageBox.Show(Resources.str15476, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextBox601.Text = TextBox601.Tag.ToString();
                TextBox601_Validating(TextBox601, new System.ComponentModel.CancelEventArgs());
                return;
            }

            pLine = new Polyline();

            pLine.AddPoint(Functions.PointAlongPlane(Pt1, DepDir + 90.0, GlobalVars.RModel));
            pLine.AddPoint(Functions.PointAlongPlane(Pt1, DepDir - 90.0, GlobalVars.RModel));

            KK = pTopo.Intersect(pLine as IGeometry, esriGeometryDimension.esriGeometry1Dimension) as IPolyline;

            if (Functions.SideDef(DER.pPtPrj[eRWY.PtDER], DepDir, KK.FromPoint) < 0)
                KK.ReverseOrientation();
            // ==========================
            fDis = Functions.Point2LineDistancePrj(DER.pPtPrj[eRWY.PtDER], Pt1, DepDir + 90.0);
            fTmp = System.Math.Round(PANS_OPS_DataBase.dpOIS_abv_DER.Value + fDis * CurrPDG + 0.4999);
            if (fTmp < PANS_OPS_DataBase.dpGui_Ar1.Value)
                fTmp = PANS_OPS_DataBase.dpGui_Ar1.Value;

            d = Functions.Point2LineDistancePrj(DER.pPtPrj[eRWY.PtDER], TurnFixPnt, DepDir + 90.0);
            hTurn = d * PANS_OPS_DataBase.dpOv_Nav_PDG.Value + PANS_OPS_DataBase.dpOIS_abv_DER.Value + DER.pPtPrj[eRWY.PtDER].Z;

            //     If (ComboBox603.ListIndex = 0) Then
            TextBox602.Text = Functions.ConvertHeight(PANS_OPS_DataBase.dpOIS_abv_DER.Value + fDis * CurrPDG, eRoundMode.NEAREST).ToString();
            //     Else
            //         TextBox602.Text = CStr(System.Math.Round((dpOIS_abv_DER.Value + fDis * CurrPDG ) / FootCoeff + 0.4999))
            //     End If

            //     If (ComboBox604.ListIndex = 0) Then
            TextBox603.Text = Functions.ConvertHeight(PANS_OPS_DataBase.dpOIS_abv_DER.Value + fDis * CurrPDG + DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.NEAREST).ToString();
            //     Else
            //         TextBox603.Text = CStr(System.Math.Round((dpOIS_abv_DER.Value + fDis * CurrPDG + DER.pPtPrj(ptEnd).Z) / FootCoeff + 0.4999))
            //     End If


            TurnFixPnt.Z = hTurn;
            TurnFixPnt.M = DepDir;

            Functions.CutPoly(ref pFIXPoly, pLine, 1);

            pLine.RemovePoints(0, pLine.PointCount);
            pLine.AddPoint(Functions.PointAlongPlane(Pt2, DepDir + 90.0, GlobalVars.RModel));
            pLine.AddPoint(Functions.PointAlongPlane(Pt2, DepDir - 90.0, GlobalVars.RModel));

            KKFixMax = pTopo.Intersect(pLine as IGeometry, esriGeometryDimension.esriGeometry1Dimension) as IPolyline;
            if (Functions.SideDef(DER.pPtPrj[eRWY.PtDER], DepDir, KKFixMax.FromPoint) < 0)
                KKFixMax.ReverseOrientation();

            Functions.CutPoly(ref pFIXPoly, pLine, -1);
            // ====
            fTASl = Functions.IAS2TAS(fIAS, hTurn, GlobalVars.CurrADHP.ISAtC);
            VTotal = fTASl + GlobalVars.CurrADHP.WindSpeed;
            Ls = VTotal * PANS_OPS_DataBase.dpT_TechToleranc.Value * 0.277777777777778;

            d0 = Functions.Point2LineDistancePrj(DER.pPtPrj[eRWY.PtDER], KKFixMax.FromPoint, DepDir + 90.0);
            Pt1 = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir, d0 + Ls);

            pLine.RemovePoints(0, pLine.PointCount);

            pLine.AddPoint(Functions.PointAlongPlane(Pt1, DepDir + 90.0, GlobalVars.RModel));
            pLine.AddPoint(Functions.PointAlongPlane(Pt1, DepDir - 90.0, GlobalVars.RModel));

            K1K1 = pTopo.Intersect(pLine as IGeometry, esriGeometryDimension.esriGeometry1Dimension) as IPolyline;

            if (Functions.SideDef(K1K1.ToPoint, DepDir, K1K1.FromPoint) < 0)
                K1K1.ReverseOrientation();

            // ==========================
            pGroupElem.AddElement(Functions.DrawPolygon(pFIXPoly, Functions.RGB(255, 255, 0), esriSimpleFillStyle.esriSFSNull, false));
            pGroupElem.AddElement(Functions.DrawPoint(TurnFixPnt, 255, false));
            GlobalVars.FIXElem = ((ESRI.ArcGIS.Carto.IElement)(pGroupElem));

            IPointCollection mPoly = null;
            mPoly = new Polyline();
            mPoly.AddPoint(PtDerShift);
            mPoly.AddPoint(TurnFixPnt);

            Functions.DeleteGraphicsElement(GlobalVars.StrTrackElem);
            GlobalVars.StrTrackElem = Functions.DrawPolyline(mPoly, GlobalVars.StrTrackElemColor, 1.0, false);

            if (GlobalVars.ButtonControl8State)
            {
                for (i = 0; i <= pGroupElem.ElementCount - 1; i++)
                {
                    pElement = pGroupElem.get_Element(i);
                    pGraphics.AddElement(pElement, 0);
                    pElement.Locked = true;
                }
            }

            if (GlobalVars.ButtonControl5State)
            {
                pGraphics.AddElement(GlobalVars.StrTrackElem, 0);
                GlobalVars.StrTrackElem.Locked = true;
            }

            //Functions.RefreshCommandBar(_mTool, 128);

            GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            // ==========================================================================
            ObstacleContainer TmpInList;
            if (fDis == GlobalVars.RModel)
            {
                int nN = oZNRList.Obstacles.Length;
                TmpInList.Obstacles = new Obstacle[nN];
                System.Array.Copy(oZNRList.Obstacles, TmpInList.Obstacles, nN);

                nN = oZNRList.Parts.Length;
                TmpInList.Parts = new ObstacleData[nN];
                System.Array.Copy(oZNRList.Parts, TmpInList.Parts, nN);
            }
            else
                Functions.GetObstInRange(oZNRList, out TmpInList, fDis);

            ReportsFrm.FillPage4(TmpInList, OptionButton401.Checked);
            // ==========================================================================
            TextBox601.Tag = TextBox601.Text;
            TextBox602_Validating(TextBox602, new System.ComponentModel.CancelEventArgs());
        }

        private void TextBox1011_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char eventChar = eventArgs.KeyChar;

            if (eventChar == 13)
                TextBox1011_Validating(TextBox1011, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox1011.Text);

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        private void TextBox1011_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            if (TextBox1011.ReadOnly)
                return;

            double NewTIA_PDG;
            if (!double.TryParse(TextBox1011.Text, out NewTIA_PDG))
            {
                MessageBox.Show(Resources.str15473, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (TextBox1011.Tag.ToString() == TextBox1011.Text)
                return;

            NewTIA_PDG *= 0.01;
            if (NewTIA_PDG < MinCurrPDG)
            {
                NewTIA_PDG = MinCurrPDG;
                TextBox1011.Text = System.Math.Round(100.0 * MinCurrPDG, 1).ToString();
            }

            CalcNewStraightAreaWithFixedLength(ref NewTIA_PDG, ref TACurrPDG);

            TextBox1011.Text = Math.Round(100.0 * CurrPDG, 1).ToString();
            TextBox1011.Tag = TextBox1011.Text;
            TextBox1012.Text = Math.Round(100.0 * TACurrPDG, 1).ToString();
            //     TextBox1012.Tag = TextBox1012.Text  '????
            // =====================================================================================
            IProximityOperator pProxi;
            IPoint ptCnt;

            if (OptionButton401.Checked)
                pProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)ZNR_Poly;
            else
                pProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)KK;

            if (OptionButton701.Checked)
            {
                IPoint ptTmp = MPtCollection.Point[MPtCollection.PointCount - 1];
                Functions.CircleVectorIntersect(GlobalVars.CurrADHP.pPtPrj, GlobalVars.RModel, ptTmp, ptTmp.M, out ptCnt);
            }
            else if (OptionButton702.Checked)
                ptCnt = TerFixPnt;
            else
                ptCnt = TurnDirector.pPtPrj;

            // =====================================================================================
            double DistToFix = pProxi.ReturnDistance(ptCnt);
            double hTurn = fZNRLen * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value;
            hKK = hTurn + DER.pPtPrj[eRWY.PtDER].Z;
            fReachedA = hKK + DistToFix * TACurrPDG;

            TextBox1014.Text = Functions.ConvertHeight(hKK, eRoundMode.NEAREST).ToString();

            double L1 = Functions.ReturnDistanceInMeters(DER.pPtPrj[eRWY.PtDER], TurnFixPnt);
            AltitudeAtTurnFixPnt = CurrPDG * L1 + PANS_OPS_DataBase.dpH_abv_DER.Value + DER.pPtPrj[eRWY.PtDER].Z;

            TextBox1006.Text = Functions.ConvertHeight(fReachedA, eRoundMode.NEAREST).ToString();
            //TextBox1016.Text = TextBox1006.Text;
            TextBox1016_Validating(TextBox1016, null);
            // =====================================================================================

            IPointCollection mPoly = (IPointCollection)Functions.CalcTrajectoryFromMultiPoint(MPtCollection);
            mPoly.AddPoint(ptCnt);
            NomInfoFrm.SetInfo(mPoly, hKK, TACurrPDG);
        }

        private void ConvertTracToPoints()
        {
            if (CurrPage <= 3)
                return;

            IPoint pPtGeo;
            IPolyline pPolyline;

            //double fZLen = 0.0;
            double OverallLength = 0.0;
            int n = MPtCollection.PointCount + 1;

            double PDGznr = double.Parse(TextBox1011.Text) * 0.01;
            double PDGzr = double.Parse(TextBox1012.Text) * 0.01;

            RoutsPoints = new ReportPoint[n + 1];

            RoutsPoints[0].Description = Resources.str00512;
            RoutsPoints[0].Lat = DER.pPtGeo[eRWY.PtDER].Y;
            RoutsPoints[0].Lon = DER.pPtGeo[eRWY.PtDER].X;
            RoutsPoints[0].Direction = NativeMethods.Modulus(Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], RWYDir));
            RoutsPoints[0].Height = PANS_OPS_DataBase.dpH_abv_DER.Value + DER.pPtPrj[eRWY.PtDER].Z;
            RoutsPoints[0].Radius = -1;

            IPoint pPtCurr = ((ESRI.ArcGIS.Geometry.IPoint)(DER.pPtPrj[eRWY.PtDER]));
            pPtCurr.M = RWYDir;

            double fE = GlobalVars.DegToRadValue * 0.5;

            IPoint pPtCross = new ESRI.ArcGIS.Geometry.Point();
            IConstructPoint ptConstr = ((ESRI.ArcGIS.Geometry.IConstructPoint)(pPtCross));

            for (int i = 1; i <= n; i++)
            {
                IPoint pPtPrev = pPtCurr;

                RoutsPoints[i].Radius = -1;
                if (i < n)
                {
                    pPtCurr = MPtCollection.get_Point(i - 1);

                    if ((i & 1) == 1)
                    {
                        // =========================================================================
                        IPoint FromPt = pPtCurr;
                        IPoint ToPt = MPtCollection.get_Point(i);
                        double fTmp = GlobalVars.DegToRadValue * (FromPt.M - ToPt.M);

                        if ((System.Math.Abs(System.Math.Sin(fTmp)) <= fE) && (System.Math.Cos(fTmp) > 0.0))
                        {
                        }
                        else
                        {
                            if (System.Math.Abs(System.Math.Sin(fTmp)) > fE)
                                ptConstr.ConstructAngleIntersection(FromPt, GlobalVars.DegToRadValue * (NativeMethods.Modulus(FromPt.M + 90.0, 360.0)), ToPt, GlobalVars.DegToRadValue * (NativeMethods.Modulus(ToPt.M + 90.0, 360.0)));
                            else
                                pPtCross.PutCoords(0.5 * (FromPt.X + ToPt.X), 0.5 * (FromPt.Y + ToPt.Y));

                            RoutsPoints[i].Radius = Functions.ReturnDistanceInMeters(pPtCross, FromPt);
                            RoutsPoints[i].Turn = Functions.SideDef(FromPt, FromPt.M, ToPt);

                            RoutsPoints[i].turnAngle = NativeMethods.Modulus((FromPt.M - ToPt.M) * RoutsPoints[i].Turn);
                            RoutsPoints[i].TurnArcLen = RoutsPoints[i].turnAngle * GlobalVars.DegToRadValue * RoutsPoints[i].Radius;

                            pPtGeo = (ESRI.ArcGIS.Geometry.IPoint)Functions.ToGeo(pPtCross);
                            RoutsPoints[i].CenterLat = pPtGeo.Y;
                            RoutsPoints[i].CenterLon = pPtGeo.X;

                            RoutsPoints[i].Direction = -1;
                        }
                        // =========================================================================
                        RoutsPoints[i].Description = Resources.str00513 + GlobalVars.RomanFigures[(i + 1) / 2 - 1] + Resources.str00515;
                    }
                    else
                    {
                        RoutsPoints[i].Description = Resources.str00514 + GlobalVars.RomanFigures[(i + 1) / 2 - 1] + Resources.str00515;
                        RoutsPoints[i].Direction = NativeMethods.Modulus(Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], pPtCurr.M));
                    }
                }
                else
                {
                    pPtCurr = TracPoly.get_Point(TracPoly.PointCount - 1);
                    pPtCurr.M = pPtPrev.M;
                    RoutsPoints[i].Description = Resources.str00516;
                    //GuidPoints[i].Direction = GuidPoints[i - 1].Direction
                    RoutsPoints[i].Direction = -1.0;
                }

                if (RoutsPoints[i - 1].Radius > 0)
                {
                    IPointCollection pMultiPoint = new Multipoint();
                    pMultiPoint.AddPoint(pPtPrev);
                    pMultiPoint.AddPoint(pPtCurr);
                    pPolyline = Functions.CalcTrajectoryFromMultiPoint(pMultiPoint);
                }
                else
                {
                    pPolyline = (ESRI.ArcGIS.Geometry.IPolyline)(new Polyline());
                    pPolyline.FromPoint = pPtPrev;
                    pPolyline.ToPoint = pPtCurr;
                }

                OverallLength += pPolyline.Length;
                RoutsPoints[i - 1].DistToNext = pPolyline.Length;
                RoutsPoints[i].DistToNext = 0.0;

                if (i == 1)
                {
                    RoutsPoints[i].Height = RoutsPoints[i - 1].Height + pPolyline.Length * PDGznr;
                    //fZLen = pPolyline.Length;
                }
                else
                    RoutsPoints[i].Height = RoutsPoints[i - 1].Height + pPolyline.Length * PDGzr;

                pPtGeo = (ESRI.ArcGIS.Geometry.IPoint)Functions.ToGeo(pPtCurr);
                RoutsPoints[i].Lat = pPtGeo.Y;
                RoutsPoints[i].Lon = pPtGeo.X;
            }

            //ZRSegment = OverallLength - fZLen;
            GuidAllLen = OverallLength;
        }

        /*
		private void SaveGeometry(string RepFileName, string RepFileTitle, ReportHeader pReport)
		{
			CReportFile GuidGeomRep = new CReportFile();
			GuidGeomRep.DerPtPrj = DER.pPtPrj[eRWY.PtDER];

			GuidGeomRep.OpenFile(RepFileName + "_Geometry", Resources.str517);

			GuidGeomRep.WriteString(Resources.str15479 + " - " + Resources.str517);
			GuidGeomRep.WriteString("");
			GuidGeomRep.WriteString(RepFileTitle);

			GuidGeomRep.WriteHeader(pReport);
			//     GuidGeomRep.WriteParam LoadResString(518), CStr(Date) + " - " + CStr(Time)
			//     If AIRLayerInfo.Initialised Then GuidGeomRep.WriteLayerInfo AIRLayerInfo
			//     If RWYLayerInfo.Initialised Then GuidGeomRep.WriteLayerInfo RWYLayerInfo
			//     If NAVLayerInfo.Initialised Then GuidGeomRep.WriteLayerInfo NAVLayerInfo
			//     If ObsLayerInfo.Initialised Then GuidGeomRep.WriteLayerInfo ObsLayerInfo
			//     If FIXLayerInfo.Initialised Then GuidGeomRep.WriteLayerInfo FIXLayerInfo
			//     If WARNINGLayerInfo.Initialised Then GuidGeomRep.WriteLayerInfo WARNINGLayerInfo

			GuidGeomRep.WriteString("");
			GuidGeomRep.WriteString("");

			int n = GuidPoints.Length;
			for (int i = 0; i < n; i++)
				GuidGeomRep.WritePoint(GuidPoints[i]);

			GuidGeomRep.WriteString("");

			GuidGeomRep.WriteParam(Resources.str519, Functions.ConvertDistance(GuidAllLen, eRoundMode.rmNERAEST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);

			GuidGeomRep.CloseFile();
			GuidGeomRep = null;
		}
		*/

        private void SaveAccuracy(string RepFileName, string RepFileTitle, ReportHeader pReport)
        {
            AccurRep = new ReportFile();

            AccurRep.OpenFile(RepFileName + "_Accuracy", RepFileTitle + ": " + Resources.str00173);
            //AccurRep.H1(My.Resources.str15479 + " - " + RepFileTitle + ": " + My.Resources.str00173);
            AccurRep.WriteString(Resources.str15479 + " - " + RepFileTitle + ": " + Resources.str00173, true);

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

            NavaidType GuidNav = FrontList[ComboBox101.SelectedIndex];
            NavaidType IntersectNav;

            // =============================================================================================================
            if (OptionButton402.Checked)
            {
                IntersectNav = TurnInterDat[ComboBox601.SelectedIndex];
                Functions.SaveFixAccurasyInfo(AccurRep, TurnFixPnt, "TP", GuidNav, IntersectNav);
            }

            // =============================================================================================================

            if (OptionButton702.Checked)
            {
                IntersectNav = TerInterNavDat[ComboBox1002.SelectedIndex];
                if (IntersectNav.ValCnt != -2)
                {
                    GuidNav = Navaids_DataBase.WPT_FIXToNavaid(TurnDirector);
                    Functions.SaveFixAccurasyInfo(AccurRep, TerFixPnt, "", GuidNav, IntersectNav, true);
                }
            }

            // =============================================================================================================
            //if (CheckBox1501.Checked)
            //{
            //	int i = 1;

            //	while (i < TSC)
            //	{
            //		TraceSegment currSegment = Trace[i];
            //		if (i < TSC - 1)
            //		{
            //			TraceSegment nextSegment = Trace[i + 1];
            //			if (nextSegment.SegmentCode == eSegmentType.straight && currSegment.LegType == CodeSegmentPath.DF)
            //			{
            //				currSegment = nextSegment;
            //				i++;
            //			}
            //		}
            //		i++;

            //		IntersectNav = currSegment.InterceptionNav;
            //		GuidNav = currSegment.GuidanceNav;
            //		SaveFixAccurasyInfo(AccurRep, currSegment.ptOut, IIf(i < TSC, "MATF", "MAHF"), GuidNav, IntersectNav, i >= TSC);
            //	}
            //}
            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            AccurRep.CloseFile();
        }

        private void SaveProtocol(string RepFileName, string RepFileTitle, ReportHeader pReport)
        {
            ReportsFrm.SortForSave();

            GuidProtRep = new ReportFile();

            GuidProtRep.ThrPtPrj = DER.pPtPrj[eRWY.PtDER];

            GuidProtRep.OpenFile(RepFileName + "_Protocol", RepFileTitle + ": " + Resources.str00170);

            GuidProtRep.WriteString(Resources.str15479 + " - " + RepFileTitle + ": " + Resources.str00170, true);
            GuidProtRep.WriteString("");
            GuidProtRep.WriteString(RepFileTitle, true);

            GuidProtRep.WriteHeader(pReport);
            GuidProtRep.WriteString("");
            GuidProtRep.WriteString("");

            GuidProtRep.lListView = ReportsFrm.listView1;
            GuidProtRep.WriteTab(ReportsFrm.GetTabPageText(0));

            GuidProtRep.lListView = ReportsFrm.listView4;
            GuidProtRep.WriteTab(ReportsFrm.GetTabPageText(3));

            GuidProtRep.lListView = ReportsFrm.listView5;
            GuidProtRep.WriteTab(ReportsFrm.GetTabPageText(4));

            GuidProtRep.CloseFile();
        }

        private void SaveGeometry(string RepFileName, string RepFileTitle, ReportHeader pReport, RWYType DER, ReportPoint[] reportPoints, double allRoutsLen, bool guaded)
        {
            GuidGeomRep = new ReportFile();
            GuidGeomRep.ThrPtPrj = DER.pPtPrj[eRWY.PtDER];

            GuidGeomRep.OpenFile(RepFileName + "_Geometry", RepFileTitle + ": " + Resources.str00517);
            if (guaded)
                GuidGeomRep.WriteString(Resources.str15479 + " - " + RepFileTitle + ": " + Resources.str00517, true);
            else
                GuidGeomRep.WriteString(Resources.str15271 + " - " + RepFileTitle + ": " + Resources.str00517, true);

            GuidGeomRep.WriteString("");
            GuidGeomRep.WriteString(RepFileTitle, true);
            GuidGeomRep.WriteHeader(pReport);

            GuidGeomRep.WriteString("");
            GuidGeomRep.WriteString("");

            int n = reportPoints.Length;
            for (int i = 0; i < n; i++)
                GuidGeomRep.WritePoint(reportPoints[i]);

            GuidGeomRep.WriteString("");

            GuidGeomRep.Param(Resources.str00519, Functions.ConvertDistance(allRoutsLen, eRoundMode.NEAREST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);

            GuidGeomRep.CloseFile();
        }
        private void SaveLog(string RepFileName, string RepFileTitle, ReportHeader pReport)
        {
            GuidLogRep = new ReportFile();
            GuidLogRep.ThrPtPrj = DER.pPtPrj[eRWY.PtDER];

            GuidLogRep.OpenFile(RepFileName + "_Log", RepFileTitle + ": " + Resources.str00520);

            GuidLogRep.WriteString(Resources.str15479 + " - " + RepFileTitle + ": " + Resources.str00520, true);
            GuidLogRep.WriteString("");
            GuidLogRep.WriteString(RepFileTitle, true);

            GuidLogRep.WriteHeader(pReport);

            //     GuidLogRep.WriteParam LoadResString(518), CStr(Date) + " - " + CStr(Time)
            //     If AIRLayerInfo.Initialised Then GuidLogRep.WriteLayerInfo AIRLayerInfo
            //     If RWYLayerInfo.Initialised Then GuidLogRep.WriteLayerInfo RWYLayerInfo
            //     If NAVLayerInfo.Initialised Then GuidLogRep.WriteLayerInfo NAVLayerInfo
            //     If ObsLayerInfo.Initialised Then GuidLogRep.WriteLayerInfo ObsLayerInfo
            //     If FIXLayerInfo.Initialised Then GuidLogRep.WriteLayerInfo FIXLayerInfo
            //     If WARNINGLayerInfo.Initialised Then GuidLogRep.WriteLayerInfo WARNINGLayerInfo

            GuidLogRep.WriteString("");
            GuidLogRep.WriteString("");


            GuidLogRep.ExH2(MultiPage1.TabPages[0].Text);

            GuidLogRep.HTMLMessage("[ " + MultiPage1.TabPages[0].Text + " ]");
            GuidLogRep.WriteString("");

            GuidLogRep.Param(Label001.Text, ComboBox001.Text, "");
            GuidLogRep.Param(Label002.Text, TextBox001.Text, "");
            GuidLogRep.Param(Label006.Text, TextBox004.Text, "");
            GuidLogRep.Param(Label013.Text, TextBox007.Text, Label010.Text);
            GuidLogRep.Param(Label003.Text, TextBox002.Text, Label011.Text);
            GuidLogRep.WriteString("");

            GuidLogRep.Param(Label004.Text, TextBox003.Text, Label009.Text);
            if (Option001.Checked)
                GuidLogRep.Param(Frame001.Text, Option001.Text, "");
            else
                GuidLogRep.Param(Frame001.Text, Option002.Text, "");

            GuidLogRep.WriteString("");

            GuidLogRep.WriteString(Frame002.Text, true);
            GuidLogRep.Param(Label007.Text, TextBox005.Text, "");
            GuidLogRep.Param(Label008.Text, TextBox006.Text, Label012.Text);
            GuidLogRep.WriteString("");
            GuidLogRep.WriteString("");

            GuidLogRep.ExH2(MultiPage1.TabPages[1].Text);
            GuidLogRep.HTMLMessage("[ " + MultiPage1.TabPages[1].Text + " ]");
            GuidLogRep.WriteString("");

            GuidLogRep.Param(Label101.Text, ComboBox101.Text, "");
            GuidLogRep.Param(Resources.str02102, Label103.Text, "");
            GuidLogRep.Param(Label108.Text, Label109.Text, "");
            GuidLogRep.Param(Label104.Text, Label105.Text, "");
            GuidLogRep.WriteString("");

            GuidLogRep.WriteString(Frame101.Text, true);
            GuidLogRep.WriteString(Label106.Text, true);
            GuidLogRep.WriteString(Label107.Text, true);
            GuidLogRep.WriteString("");
            GuidLogRep.WriteString("");


            GuidLogRep.ExH2(MultiPage1.TabPages[2].Text);
            GuidLogRep.HTMLMessage("[ " + MultiPage1.TabPages[2].Text + " ]");
            GuidLogRep.WriteString("");

            GuidLogRep.Param(Label201.Text, TextBox201.Text, "°");
            GuidLogRep.Param(Label203.Text, TextBox202.Text, "%");
            GuidLogRep.Param(Label204.Text, TextBox203.Text, Label209.Text);
            GuidLogRep.Param(Label205.Text, TextBox204.Text, Label210.Text);
            GuidLogRep.Param(Label206.Text, TextBox205.Text, Label211.Text + " " + Label212.Text);
            GuidLogRep.WriteString("");
            GuidLogRep.WriteString("");


            GuidLogRep.ExH2(MultiPage1.TabPages[3].Text);
            GuidLogRep.HTMLMessage("[ " + MultiPage1.TabPages[3].Text + " ]");
            GuidLogRep.WriteString("");

            GuidLogRep.Param(Label301.Text, TextBox301.Text, "°");
            GuidLogRep.Param(Label303.Text, TextBox302.Text, Label304.Text);
            GuidLogRep.Param(Label305.Text, TextBox303.Text, "%");
            GuidLogRep.Param(Label307.Text, TextBox304.Text, Label308.Text);
            GuidLogRep.Param(Label309.Text, TextBox305.Text, "");
            GuidLogRep.Param(Label310.Text, TextBox306.Text, "");
            GuidLogRep.WriteString("");
            GuidLogRep.WriteString("");

            if (CurrPage > 3)
            {
                GuidLogRep.ExH2(MultiPage1.TabPages[4].Text);
                GuidLogRep.HTMLMessage("[ " + MultiPage1.TabPages[4].Text + " ]");
                GuidLogRep.WriteString("");

                GuidLogRep.Param(_Label300_2.Text, ComboBox301.Text, "");
                GuidLogRep.Param(Resources.str00902, Label409.Text, Label403.Text);
                GuidLogRep.Param(Resources.str00903, Label410.Text, Label403.Text);
                GuidLogRep.Param(Label402.Text, TextBox401.Text, Label403.Text);
                GuidLogRep.Param(Label404.Text, TextBox402.Text, "°");
                GuidLogRep.Param(Label411.Text, ComboBox401.Text, "");
                GuidLogRep.WriteString("");

                if (OptionButton401.Checked)
                    GuidLogRep.Param(Frame401.Text, OptionButton401.Text, "");

                if (OptionButton402.Checked)
                    GuidLogRep.Param(Frame401.Text, OptionButton402.Text, "");

                if (OptionButton403.Checked)
                    GuidLogRep.Param(Frame401.Text, OptionButton403.Text, "");
                if (OptionButton401.Checked)
                {
                    if (CheckBox401.Checked)
                        GuidLogRep.Param(CheckBox401.Text, Resources.str39015, "");
                    else
                        GuidLogRep.Param((CheckBox401.Text), Resources.str39014, "");
                }

                GuidLogRep.WriteString("");
                GuidLogRep.WriteString("");

                GuidLogRep.ExH2(MultiPage1.TabPages[5].Text);
                GuidLogRep.HTMLMessage("[ " + MultiPage1.TabPages[5].Text + " ]");
                GuidLogRep.WriteString("");

                GuidLogRep.Param(Label504.Text, TextBox503.Text, "°");
                GuidLogRep.WriteString("");

                GuidLogRep.WriteString(Label501.Text, true);
                GuidLogRep.Param(Label502.Text, TextBox501.Text, Label513.Text);
                GuidLogRep.Param(Label503.Text, TextBox502.Text, Label514.Text);
                GuidLogRep.WriteString("");

                GuidLogRep.WriteString(Label507.Text, true);
                GuidLogRep.Param(Label508.Text, SpinButton501.Value.ToString(), "? " + ComboBox501.Text);
                GuidLogRep.Param(Label510.Text, TextBox505.Text, "%");
                GuidLogRep.WriteString("");
                GuidLogRep.WriteString("");

                if (!(OptionButton403.Checked))
                {
                    GuidLogRep.ExH2(MultiPage1.TabPages[6].Text);
                    GuidLogRep.HTMLMessage("[ " + MultiPage1.TabPages[6].Text + " ]");
                    GuidLogRep.WriteString("");

                    GuidLogRep.WriteString(Frame601.Text, true);
                    if (OptionButton402.Checked)
                    {
                        GuidLogRep.Param(Label601.Text, ComboBox601.Text + " (" + Label602.Text + ")", "");
                        GuidLogRep.Param(Label603.Text, TextBox601.Text, Label626.Text);

                        if (TurnInterDat[ComboBox601.SelectedIndex].TypeCode == eNavaidType.DME)
                        {
                            if (Option601.Checked)
                                GuidLogRep.Param(Resources.str00521, Option601.Text, "");
                            else
                                GuidLogRep.Param(Resources.str00521, Option602.Text, "");
                        }
                    }
                    GuidLogRep.WriteString("");

                    GuidLogRep.Param(Label625.Text, TextBox603.Text, Label627.Text);
                    GuidLogRep.Param(Label604.Text, TextBox602.Text, Label628.Text);
                    GuidLogRep.Param(Label606.Text, Text601.Text, Label607.Text);
                    GuidLogRep.WriteString("");

                    GuidLogRep.Param(Label609.Text, Text602.Text, Label629.Text);
                    GuidLogRep.Param(Label611.Text, Text603.Text, Label630.Text);
                    GuidLogRep.Param(Label613.Text, Text604.Text, Label631.Text);
                    GuidLogRep.Param(Label615.Text, Text605.Text, Label632.Text);
                    GuidLogRep.Param(Label617.Text, Text606.Text, Label633.Text);
                    GuidLogRep.Param(Label619.Text, Text607.Text, Label634.Text);

                    if (!OptionButton401.Checked)
                    {
                        GuidLogRep.Param(Label621.Text, Text608.Text, Label635.Text);
                        GuidLogRep.Param(Label622.Text, Text609.Text, Label636.Text);
                        GuidLogRep.Param(Label623.Text, Text610.Text, Label637.Text);
                    }

                    GuidLogRep.WriteString("");
                    GuidLogRep.WriteString("");
                }

                GuidLogRep.ExH2(MultiPage1.TabPages[7].Text);
                GuidLogRep.HTMLMessage("[ " + MultiPage1.TabPages[7].Text + " ]");
                GuidLogRep.WriteString("");

                if (OptionButton701.Checked)
                {
                    GuidLogRep.WriteString(OptionButton701.Text, true);
                    GuidLogRep.Param(Label703.Text, TextBox702.Text, "");
                    GuidLogRep.WriteString("");
                }

                if (OptionButton702.Checked)
                {
                    GuidLogRep.WriteString(OptionButton702.Text, true);
                    GuidLogRep.Param(Label701.Text, ComboBox701.Text + " (" + Label702.Text + ")", "");
                    GuidLogRep.Param(Label703.Text, TextBox702.Text, "");
                    GuidLogRep.WriteString("");

                    GuidLogRep.Param(Label58.Text, TextBox701.Text, Label705.Text);
                    GuidLogRep.Param(Label706.Text, TextBox703.Text, "°");
                    GuidLogRep.Param(Label708.Text, Label710.Text, "");
                    GuidLogRep.WriteString("");
                }

                if (OptionButton703.Checked)
                {
                    GuidLogRep.WriteString(OptionButton703.Text, true);
                    GuidLogRep.Param(Label701.Text, ComboBox701.Text + " (" + Label702.Text + ")", "");
                    GuidLogRep.Param(Label703.Text, TextBox702.Text, "");
                    GuidLogRep.WriteString("");
                }

                GuidLogRep.WriteString("");

                if (OptionButton702.Checked)
                {
                    GuidLogRep.ExH2(MultiPage1.TabPages[9].Text);
                    GuidLogRep.HTMLMessage("[ " + MultiPage1.TabPages[9].Text + " ]");
                    GuidLogRep.WriteString("");

                    GuidLogRep.WriteString(_Frame901_0.Text, true);
                    if (CheckBox901.Checked)
                        GuidLogRep.Param(CheckBox901.Text, Resources.str39015, "");
                    else
                        GuidLogRep.Param(CheckBox901.Text, Resources.str39014, "");

                    if (CheckBox902.Checked)
                        GuidLogRep.Param(CheckBox902.Text, Resources.str39015, "");
                    else
                        GuidLogRep.Param(CheckBox902.Text, Resources.str39014, "");

                    if (CheckBox903.Checked)
                        GuidLogRep.Param(CheckBox903.Text, Resources.str39015, "");
                    else
                        GuidLogRep.Param(CheckBox903.Text, Resources.str39014, "");

                    GuidLogRep.WriteString("");

                    GuidLogRep.WriteString(_Frame901_1.Text, true);
                    if (CheckBox904.Checked)
                        GuidLogRep.Param(CheckBox904.Text, Resources.str39015, "");
                    else
                        GuidLogRep.Param(CheckBox904.Text, Resources.str39014, "");

                    if (CheckBox905.Checked)
                        GuidLogRep.Param(CheckBox905.Text, Resources.str39015, "");
                    else
                        GuidLogRep.Param(CheckBox905.Text, Resources.str39014, "");

                    if (CheckBox906.Checked)
                        GuidLogRep.Param(CheckBox906.Text, Resources.str39015, "");
                    else
                        GuidLogRep.Param((CheckBox906.Text), Resources.str39014, "");

                    GuidLogRep.WriteString("");
                    GuidLogRep.WriteString("");
                }

                GuidLogRep.ExH2(MultiPage1.TabPages[10].Text);
                GuidLogRep.HTMLMessage("[ " + MultiPage1.TabPages[10].Text + " ]");
                GuidLogRep.WriteString("");

                GuidLogRep.Param(Label1001_01.Text, TextBox1005.Text, Label1001_02.Text);
                GuidLogRep.Param(Label1001_07.Text, TextBox1016.Text, Label1001_08.Text);
                GuidLogRep.Param(Label1001_03.Text, TextBox1015.Text, Label1001_04.Text);
                GuidLogRep.Param(Label1001_05.Text, TextBox1006.Text, Label1001_06.Text);
                GuidLogRep.WriteString("");

                if (OptionButton702.Checked)
                {
                    GuidLogRep.WriteString(Frame1003.Text, true);

                    GuidLogRep.Param(Label1001_19.Text, ComboBox1002.Text + " (" + Label1001_20.Text + ")", "");
                    GuidLogRep.Param(Label1001_21.Text, TextBox1007.Text, Label1001_22.Text);

                    if (TerInterNavDat[ComboBox1002.SelectedIndex].TypeCode == eNavaidType.DME)
                    {
                        if (OptionButton1006.Checked)
                            GuidLogRep.Param(Resources.str00521, OptionButton1006.Text, "");
                        else
                            GuidLogRep.Param(Resources.str00521, OptionButton1007.Text, "");
                    }

                    GuidLogRep.WriteString("");
                }


                GuidLogRep.WriteString(Resources.str39001, true);
                GuidLogRep.Param(Label1001_09.Text, TextBox1001.Text, "%");
                GuidLogRep.Param(Label1001_11.Text, TextBox1002.Text, "%");
                GuidLogRep.Param(Label1001_15.Text, TextBox1004.Text, GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);
                GuidLogRep.WriteString("");

                GuidLogRep.WriteString(Resources.str39002, true);
                GuidLogRep.Param(OptionButton1001.Text, TextBox1011.Text, "%");
                GuidLogRep.Param(OptionButton1002.Text, TextBox1012.Text, "%");
                GuidLogRep.WriteString("");

                GuidLogRep.WriteString("");
                GuidLogRep.WriteString("");
            }

            GuidLogRep.CloseFile();
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

            this.Text = Resources.str00022 + "  [" + MultiPage1.TabPages[StIndex].Text + "]";
        }

        private DepartureLeg StraightDepartureLeg(ref StandardInstrumentDeparture pProcedure, AircraftCharacteristic IsLimitedTo, out TerminalSegmentPoint pEndPoint)
        {
            double fSegmentLength;
            double fDistToNav, fAltitude;
            double Angle, fDist, fDir1, fDir;
            double PriorFixTolerance, PostFixTolerance;

            SegmentLeg pSegmentLeg;
            DepartureLeg pDepartureLeg;
            TerminalSegmentPoint pStartPoint;

            SegmentPoint pSegmentPoint;
            SignificantPoint pGuidNavSignPt;

            AngleUse pAngleUse;
            PointReference pPointReference;
            AngleIndication pAngleIndication;
            AngleIndication pStAngleIndication;
            DistanceIndication pDistanceIndication;

            ValSpeed pSpeed;
            ValDistance pDistance;
            ValDistanceSigned pDistanceSigned;
            ValDistanceVertical pDistanceVertical;

            UomSpeed mUomSpeed;
            UomDistance mUomHDistance;
            UomDistanceVertical mUomVDistance;

            UomSpeed[] uomSpeedTab;
            UomDistance[] uomDistHorTab;
            UomDistanceVertical[] uomDistVerTab;

            IPoint ptTmp;

            NavaidType GuidNav;
            NavaidType IntersectNav;
            SignificantPoint pInterNavSignPt;

            bool HaveTP = CheckBox301.Checked;

            uomSpeedTab = new UomSpeed[] { UomSpeed.KM_H, UomSpeed.KT, UomSpeed.M_SEC, UomSpeed.MACH, UomSpeed.M_MIN, UomSpeed.FT_MIN, UomSpeed.FT_SEC };
            uomDistHorTab = new UomDistance[] { UomDistance.KM, UomDistance.NM, UomDistance.M, UomDistance.FT, UomDistance.MI };
            uomDistVerTab = new UomDistanceVertical[] { UomDistanceVertical.M, UomDistanceVertical.FT };

            mUomSpeed = uomSpeedTab[GlobalVars.SpeedUnit];
            mUomVDistance = uomDistVerTab[GlobalVars.HeightUnit];
            mUomHDistance = uomDistHorTab[GlobalVars.DistanceUnit];

            pDepartureLeg = DBModule.pObjectDir.CreateFeature<DepartureLeg>();
            pDepartureLeg.Departure = pProcedure.GetFeatureRef();
            pDepartureLeg.AircraftCategory.Add(IsLimitedTo);
            pSegmentLeg = pDepartureLeg;

            //SegmentLeg
            pSegmentLeg.UpperLimitReference = CodeVerticalReference.MSL;
            pSegmentLeg.LowerLimitReference = CodeVerticalReference.MSL;
            //pSegmentLeg.TurnDirection = CodeDirectionTurn.EITHER;
            pSegmentLeg.LegPath = CodeTrajectory.STRAIGHT;
            pSegmentLeg.CourseType = CodeCourse.TRUE_TRACK;

            if (HaveTP && !OptionButton401.Checked)
            {
                pSegmentLeg.LegTypeARINC = CodeSegmentPath.CF;
                pSegmentLeg.EndConditionDesignator = CodeSegmentTermination.INTERCEPT;
            }
            else
            {
                pSegmentLeg.LegTypeARINC = CodeSegmentPath.CA;
                pSegmentLeg.EndConditionDesignator = CodeSegmentTermination.ALTITUDE;
            }

            pSegmentLeg.ProcedureTurnRequired = false;

            //     pSegmentLeg.AltitudeOverrideATC =
            //     pSegmentLeg.AltitudeOverrideReference =
            //     pSegmentLeg.Duration = '???????????????????????????????? pLegs(I).valDur
            //     pSegmentLeg.Note
            //     pSegmentLeg.ReqNavPerformance
            //     pSegmentLeg.SpeedInterpretation =
            //     pSegmentLeg.ReqNavPerformance

            //     pSegmentLeg.CourseDirection =
            //     pSegmentLeg.ProcedureTransition

            GuidNav = FrontList[ComboBox101.SelectedIndex];
            pGuidNavSignPt = GuidNav.GetSignificantPoint();

            // ====================================================================
            if (Functions.SideDef(PtDerShift, DepDir + 90.0, GuidNav.pPtPrj) < 0)
                pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.FROM;
            else
                pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO;

            pSegmentLeg.Course = Functions.Dir2Azt(PtDerShift, DepDir);
            //pSegmentLeg.Course = NativeMethods.Modulus(System.Math.Round(DER.pPtGeo[eRWY.PtDER].M - TrackAdjust), 360.0);

            // ====================================================================
            pDistanceVertical = new ValDistanceVertical();
            pDistanceVertical.Uom = mUomVDistance;

            if (HaveTP)
                pDistanceVertical.Value = Functions.ConvertHeight(AltitudeAtTurnFixPnt);
            else
                pDistanceVertical.Value = double.Parse(TextBox307.Text);

            pSegmentLeg.LowerLimitAltitude = pDistanceVertical;
            pSegmentLeg.AltitudeInterpretation = CodeAltitudeUse.AT_LOWER;
            // ====================================================================
            //	fTmp = DeConvertHeight(CDbl(TextBox0404.Text))
            //	Set pDistance = New Distance
            //	pDistance.Uom = mVUomDistance
            //	fTmp = ConvertHeight(fTmp, 2)
            //	pDistance.Value = CStr(fTmp)
            //	Set pSegmentLeg.UpperLimitAltitude = pDistance
            // ====================================================================
            if (HaveTP)
                fSegmentLength = Functions.ReturnDistanceInMeters(PtDerShift, TurnFixPnt);
            else
                fSegmentLength = (fStraightDepartTermAlt - PANS_OPS_DataBase.dpH_abv_DER.Value) / CurrPDG;

            pDistance = new ValDistance();
            pDistance.Uom = mUomHDistance;
            pDistance.Value = Functions.ConvertDistance(fSegmentLength);
            pSegmentLeg.Length = pDistance;
            // ====================================================================
            pSegmentLeg.VerticalAngle = Functions.RadToDeg(System.Math.Atan(CurrPDG));
            // ====================================================================
            pSpeed = new ValSpeed();
            pSpeed.Uom = mUomSpeed;
            pSpeed.Value = double.Parse(TextBox401.Text);
            pSegmentLeg.SpeedLimit = pSpeed;
            pSegmentLeg.SpeedReference = CodeSpeedReference.IAS;

            // Start Point ========================================================

            pStartPoint = new TerminalSegmentPoint();
            //        pStartPoint.IndicatorFACF =      ??????????
            //        pStartPoint.LeadDME =            ??????????
            //        pStartPoint.LeadRadial =         ??????????
            //pStartPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.

            pSegmentPoint = pStartPoint;

            //pSegmentPoint.FlyOver = true;
            pSegmentPoint.RadarGuidance = false;

            pSegmentPoint.ReportingATC = CodeATCReporting.NO_REPORT;
            pSegmentPoint.Waypoint = false;

            //== Angle indication ========================================================
            fDir = Functions.ReturnAngleInDegrees(GuidNav.pPtPrj, PtDerShift);
            Angle = NativeMethods.Modulus(Functions.Dir2Azt(GuidNav.pPtPrj, fDir) - GuidNav.MagVar, 360.0);

            pAngleIndication = DBModule.CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt);
            pAngleIndication.TrueAngle = Functions.Dir2Azt(PtDerShift, fDir);

            pAngleIndication.IndicationDirection = CodeDirectionReference.FROM;
            pSegmentLeg.Angle = pAngleIndication.GetFeatureRef();
            //== Angle indication ========================================================
            double horAccuracy = Functions.CalDERcHorisontalAccuracy(DER);

			fDir1 = Functions.Azt2DirPrj(PtDerShift, pSegmentLeg.Course.Value);
			Feature pFixDesignatedPoint = DBModule.CreateDesignatedPoint(PtDerShift, "DER", fDir1);

            SignificantPoint pFIXSignPt = new SignificantPoint();

            if (pFixDesignatedPoint is DesignatedPoint)
                pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef();
            else if (pFixDesignatedPoint is Navaid)
                pFIXSignPt.NavaidSystem = pFixDesignatedPoint.GetFeatureRef();

            //== Angle Use ========================================================
            pAngleUse = new AngleUse();
            pAngleIndication.Fix = pFIXSignPt.GetFeatureRef();
            pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef();
            pAngleUse.AlongCourseGuidance = true;

            pPointReference = new PointReference();
            pPointReference.FacilityAngle.Add(pAngleUse);

            pStartPoint.FacilityMakeup.Add(pPointReference);
            //== Angle Use ========================================================

            pSegmentPoint.PointChoice = pFIXSignPt;

            //SignificantPoint derEnd = new SignificantPoint();
            //derEnd.RunwayPoint = new FeatureRef(DER.pSignificantPointID[eRWY.PtDER]);
            //pSegmentPoint.PointChoice = derEnd;

            pSegmentLeg.StartPoint = pStartPoint;
            //  End Of Start Point ================================================

            //=====================================================================

            //  EndPoint ==========================================================
            pEndPoint = null;

            if (HaveTP && !OptionButton401.Checked)
            {
                if (OptionButton402.Checked)
                    IntersectNav = TurnInterDat[ComboBox601.SelectedIndex];
                else
                    IntersectNav = FrontList[ComboBox101.SelectedIndex];

                pInterNavSignPt = IntersectNav.GetSignificantPoint();

                pEndPoint = new TerminalSegmentPoint();
                //        pEndPoint.IndicatorFACF =      ??????????
                //        pEndPoint.LeadDME =            ??????????
                //        pEndPoint.LeadRadial =         ??????????
                pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.TP;

                pSegmentPoint = pEndPoint;

                //pSegmentPoint.FlyOver = true;
                pSegmentPoint.RadarGuidance = false;

                pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT;
                pSegmentPoint.Waypoint = false;

                pPointReference = new PointReference();

                if (OptionButton403.Checked)
                {
                    pFIXSignPt = pInterNavSignPt;
                    pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD;
                }
                else
                {
                    horAccuracy = 0.0;

                    if (GuidNav.Identifier != Guid.Empty && GuidNav.TypeCode != eNavaidType.NONE && IntersectNav.TypeCode != eNavaidType.NONE)
                        horAccuracy = Functions.CalcHorisontalAccuracy(TurnFixPnt, GuidNav, IntersectNav);

					fDir = Functions.Azt2DirPrj(TurnFixPnt, pSegmentLeg.Course.Value);
					pFixDesignatedPoint = DBModule.CreateDesignatedPoint(TurnFixPnt, "TP", fDir);
					pFIXSignPt = new SignificantPoint();

                    if (pFixDesignatedPoint is DesignatedPoint)
                        pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef();
                    else if (pFixDesignatedPoint is Navaid)
                        pFIXSignPt.NavaidSystem = pFixDesignatedPoint.GetFeatureRef();

                    //== Angle indication ========================================================
                    fDir = Functions.ReturnAngleInDegrees(GuidNav.pPtPrj, TurnFixPnt);
                    Angle = NativeMethods.Modulus(Functions.Dir2Azt(GuidNav.pPtPrj, fDir) - GuidNav.MagVar, 360.0);

                    pAngleIndication = DBModule.CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt);
                    pAngleIndication.TrueAngle = Functions.Dir2Azt(TurnFixPnt, fDir);
                    pAngleIndication.IndicationDirection = CodeDirectionReference.FROM;
                    pAngleIndication.Fix = pFIXSignPt.GetFeatureRef();

                    //== Angle indication ========================================================
                    //== Angle Use ==========================================================
                    pAngleUse = new AngleUse();
                    pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef();
                    pAngleUse.AlongCourseGuidance = true;

                    pPointReference.FacilityAngle.Add(pAngleUse);
                    //== Angle Use ==========================================================

                    // ========================

                    if (IntersectNav.TypeCode == eNavaidType.DME)
                    {
                        fDistToNav = Functions.ReturnDistanceInMeters(IntersectNav.pPtPrj, TurnFixPnt);
                        fAltitude = TurnFixPnt.Z - IntersectNav.pPtPrj.Z;
                        fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitude * fAltitude);

                        pDistanceIndication = DBModule.CreateDistanceIndication(Functions.ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, pInterNavSignPt);
                        pDistanceIndication.Fix = pFIXSignPt.GetFeatureRef();

                        // ======================== pStDistanceIndication.
                        pPointReference.FacilityDistance.Add(pDistanceIndication.GetFeatureRefObject());
                        pPointReference.Role = CodeReferenceRole.RAD_DME;
                    }
                    else
                    {
                        fDir = Functions.ReturnAngleInDegrees(IntersectNav.pPtPrj, TurnFixPnt);

                        Angle = NativeMethods.Modulus(Functions.Dir2Azt(IntersectNav.pPtPrj, fDir) - IntersectNav.MagVar, 360.0);
                        pStAngleIndication = DBModule.CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pInterNavSignPt);
                        pStAngleIndication.TrueAngle = Functions.Dir2Azt(TurnFixPnt, fDir);
                        pStAngleIndication.Fix = pFIXSignPt.GetFeatureRef();

                        pAngleUse = new AngleUse();
                        pAngleUse.TheAngleIndication = pStAngleIndication.GetFeatureRef();
                        pAngleUse.AlongCourseGuidance = false;
                        // ========================
                        pPointReference.FacilityAngle.Add(pAngleUse);
                        pPointReference.Role = CodeReferenceRole.INTERSECTION;
                    }
                }
                //=======================

                Functions.PriorPostFixTolerance(pFIXPoly, TurnFixPnt, DepDir, out PriorFixTolerance, out PostFixTolerance);

                pDistanceSigned = new ValDistanceSigned();
                pDistanceSigned.Uom = mUomHDistance;
                pDistanceSigned.Value = Math.Abs(Functions.ConvertDistance(PriorFixTolerance, eRoundMode.NEAREST));
                pPointReference.PriorFixTolerance = pDistanceSigned;

                pDistanceSigned = new Aran.Aim.DataTypes.ValDistanceSigned();
                pDistanceSigned.Uom = mUomHDistance;
                pDistanceSigned.Value = Math.Abs(Functions.ConvertDistance(PostFixTolerance, eRoundMode.NEAREST));

                pPointReference.PostFixTolerance = pDistanceSigned;
                pPointReference.FixToleranceArea = Converters.ESRIPolygonToAIXMSurface(Functions.ToGeo(pFIXPoly as IGeometry) as IPolygon);

                // ========
                pEndPoint.FacilityMakeup.Add(pPointReference);
                pSegmentPoint.PointChoice = pFIXSignPt;
                pSegmentLeg.EndPoint = pEndPoint;
            }
            // End of EndPoint ========================

            // Trajectory =====================================================
            Aran.Geometries.Point pLocation;
            Curve pCurve;
            LineString pLineStringSegment;

            pLineStringSegment = new LineString();
            pLocation = Converters.ESRIPointToARANPoint(Functions.ToGeo(PtDerShift) as IPoint);
            pLineStringSegment.Add(pLocation);

            if (HaveTP && OptionButton403.Checked)
                ptTmp = (IPoint)Functions.ToGeo(TurnFixPnt);
            else
                ptTmp = (IPoint)Functions.ToGeo(Functions.PointAlongPlane(PtDerShift, DepDir, fSegmentLength));

            pLocation = Converters.ESRIPointToARANPoint(ptTmp);
            pLineStringSegment.Add(pLocation);

            pCurve = new Curve();
            pCurve.Geo.Add(pLineStringSegment);
            pSegmentLeg.Trajectory = pCurve;
            //==========================================================================
            // I protected Area ========================================================
            //pPointReference.FixToleranceArea = Converters.ESRIPolygonToAIXMSurface(Functions.ToGeo(currLeg.PrimaryAssesmentArea as IGeometry) as IPolygon);

            int k = ComboBox101.SelectedIndex;
            Functions.GetZNRObstList(oAllStraightList, out oZNRList, DER, DepDir, CurrPDG, StraightPrimPoly, StraightSecPoly, FrontList[k].pPtPrj);
            int result = Functions.CalcPDGToTop(oZNRList, pPolygon, ArrayIVariant, DER, DerShift, DepDir, RWYDir, MOCLimit, FrontList[k].TypeCode, FrontList[k].pPtPrj);

            IPolygon pPolygon1, pPolygon2;

            if (HaveTP)
            {
                pPolygon1 = Functions.PolygonIntersection(ZNR_Poly as IPolygon, StraightPrimPoly as IPolygon) as IPolygon;
                pPolygon2 = Functions.PolygonIntersection(ZNR_Poly as IPolygon, StraightSecPoly as IPolygon) as IPolygon;
            }
            else
            {
                pPolygon1 = Functions.PolygonIntersection(pCircle as IPolygon, StraightPrimPoly as IPolygon) as IPolygon;
                pPolygon2 = Functions.PolygonIntersection(pCircle as IPolygon, StraightSecPoly as IPolygon) as IPolygon;
            }

            //Functions.DrawPolygon(pPolygon1, -1, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal);
            //Functions.DrawPolygon(pPolygon2, -1, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal);
            //Functions.DrawPolygon(ZNR_Poly, -1, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal);
            //Functions.DrawPolygon(StraightSecPoly, -1, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal);

            ////while(true)
            //Application.DoEvents();


            ObstacleAssessmentArea pPrimProtectedArea = new ObstacleAssessmentArea();
            pPrimProtectedArea.Surface = Converters.ESRIPolygonToAIXMSurface(Functions.ToGeo(pPolygon1 as IGeometry) as IPolygon);
            pPrimProtectedArea.SectionNumber = 0;
            pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY;
            //pPrimProtectedArea.SurfaceZone = CodeObstructionIdSurfaceZone.PRIMARY;

            // II protected Area =======================================================
            ObstacleAssessmentArea pSecProtectedArea = null;
            if (pPolygon2 != null && !pPolygon2.IsEmpty)
            {
                pSecProtectedArea = new ObstacleAssessmentArea();
                pSecProtectedArea.Surface = Converters.ESRIPolygonToAIXMSurface(Functions.ToGeo(pPolygon2 as IGeometry) as IPolygon);
                pSecProtectedArea.SectionNumber = 1;
                pSecProtectedArea.Type = CodeObstacleAssessmentSurface.SECONDARY;
                //pPrimProtectedArea.SurfaceZone = CodeObstructionIdSurfaceZone.PRIMARY;

                //pPrimProtectedArea.StartingCurve = pCurve;
            }

            // Protection Area Obstructions list ==================================================
            ObstacleContainer ostacles = oZNRList;

            IRelationalOperator relation = (IRelationalOperator)pPolygon1;

            ObstacleData[] MaxPDGArray = new ObstacleData[GlobalVars.ArraySize];
            ObstacleData[] MaxTnaHArray = new ObstacleData[GlobalVars.ArraySize];
            int MaxPDGArrayCnt = 0;
            int MaxTnaHArrayCnt = 0;

            if (ostacles.Parts.Length > 0)
            {
                //TIARequiredTNAH = CalcStraightReqTNAH(CurrPDG, drPDGMax, ref iTnaH, CheckBox101.Checked);

                MaxPDGArray[0] = ostacles.Parts[0];
                MaxTnaHArray[0] = ostacles.Parts[0];

                double MaxPDGValue = MaxPDGArray[0].PDG;
                double MinPDGValue = MaxPDGArray[0].PDG;

                double MaxTnaHValue = MaxTnaHArray[0].ReqTNH;
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
                int isPrimary = 0;
                //double MinimumAltitude = 0;
                double RequiredClearance = 0;
                bool CloseIn = true;

                obs.VerticalStructureObstruction = new FeatureRef(ostacles.Obstacles[owner].Identifier);    //ostacles.Obstacles[i].pFeature.GetFeatureRef();

                for (int j = 0; j < ostacles.Obstacles[owner].PartsNum; j++)
                {
                    if (relation.Disjoint(ostacles.Parts[ostacles.Obstacles[owner].Parts[j]].pPtPrj))
                        continue;

                    //MinimumAltitude = Math.Max(MinimumAltitude, ostacles.Parts[ostacles.Obstacles[owner].Parts[j]].hPenet);
                    RequiredClearance = Math.Max(RequiredClearance, ostacles.Parts[ostacles.Obstacles[owner].Parts[j]].MOC);


                    if (!ostacles.Parts[ostacles.Obstacles[owner].Parts[j]].Ignored)
                        CloseIn = false;

                    if (pSecProtectedArea == null)
                        isPrimary = 1;
                    else if (ostacles.Parts[ostacles.Obstacles[owner].Parts[j]].Prima)
                        isPrimary |= 1;
                    else
                        isPrimary |= 2;
                }

                if (isPrimary == 0)
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

                if ((isPrimary & 1) != 0)
                    pPrimProtectedArea.SignificantObstacle.Add(obs);
                if ((isPrimary & 2) != 0)
                    pSecProtectedArea.SignificantObstacle.Add(obs);
            }

            //pPrimProtectedArea.StartingCurve = pCurve;
            pSegmentLeg.DesignSurface.Add(pPrimProtectedArea);

            if (pSecProtectedArea != null)
                pSegmentLeg.DesignSurface.Add(pSecProtectedArea);

            //  END =====================================================
            return pDepartureLeg;
        }

        private DepartureLeg TurningDepartureLeg(ref StandardInstrumentDeparture pProcedure, AircraftCharacteristic IsLimitedTo, ref TerminalSegmentPoint pEndPoint)
        {
            DepartureLeg result;
            SegmentLeg pSegmentLeg;

            SegmentPoint pSegmentPoint;

            ValDistance pDistance;
            ValDistanceSigned pDistanceSigned;
            ValDistanceVertical pDistanceVertical;
            ValSpeed pSpeed;

            UomDistance mUomHDistance;
            UomDistanceVertical mUomVDistance;
            UomSpeed mUomSpeed;

            UomDistance[] uomDistHorzTab;
            UomDistanceVertical[] uomDistVerTab;
            UomSpeed[] uomSpeedTab;

            bool HaveIntercept = OptionButton702.Checked;
            double fTermAlt;
            double fDir, fAlt;
            double PriorFixTolerance, PostFixTolerance;

            PointReference pPointReference;
            ITopologicalOperator2 pTopo;

            uomDistHorzTab = new UomDistance[] { UomDistance.KM, UomDistance.NM, UomDistance.M, UomDistance.FT, UomDistance.MI };
            uomDistVerTab = new UomDistanceVertical[] { UomDistanceVertical.M, UomDistanceVertical.FT };
            uomSpeedTab = new UomSpeed[] { UomSpeed.KM_H, UomSpeed.KT, UomSpeed.M_SEC, UomSpeed.MACH, UomSpeed.M_MIN, UomSpeed.FT_MIN, UomSpeed.FT_SEC };

            mUomHDistance = uomDistHorzTab[GlobalVars.DistanceUnit];
            mUomVDistance = uomDistVerTab[GlobalVars.HeightUnit];
            mUomSpeed = uomSpeedTab[GlobalVars.SpeedUnit];

            result = DBModule.pObjectDir.CreateFeature<DepartureLeg>();
            result.Departure = pProcedure.GetFeatureRef();
            result.AircraftCategory.Add(IsLimitedTo);
            pSegmentLeg = result;

            pSegmentLeg.AltitudeInterpretation = CodeAltitudeUse.ABOVE_LOWER;
            pSegmentLeg.UpperLimitReference = CodeVerticalReference.MSL;
            pSegmentLeg.LowerLimitReference = CodeVerticalReference.MSL;
            pSegmentLeg.CourseType = CodeCourse.TRUE_TRACK;
            //pSegmentLeg.TurnDirection = CodeDirectionTurn.EITHER;
            pSegmentLeg.ProcedureTurnRequired = false;

            //  ======================================================================

            if (HaveIntercept)
                fTermAlt = Functions.ConvertHeight(InitialTurnFixAltitude);
            else
                fTermAlt = double.Parse(TextBox1016.Text);

            pDistanceVertical = new ValDistanceVertical();
            pDistanceVertical.Uom = mUomVDistance;

            pDistanceVertical.Value = fTermAlt;
            pSegmentLeg.LowerLimitAltitude = pDistanceVertical;
            // ======
            pSegmentLeg.VerticalAngle = Functions.RadToDeg(System.Math.Atan(TACurrPDG));
            // ======
            pSpeed = new ValSpeed();
            pSpeed.Uom = mUomSpeed;

            pSpeed.Value = double.Parse(TextBox401.Text);
            pSegmentLeg.SpeedLimit = pSpeed;
            pSegmentLeg.SpeedReference = CodeSpeedReference.IAS;

            // StartPoint ========================
            //pSegmentLeg.StartPoint = pEndPoint;
            // End Of StartPoint =================

            if (OptionButton701.Checked)
            {
                pSegmentLeg.LegTypeARINC = CodeSegmentPath.VA;
                pSegmentLeg.Course = NativeMethods.Modulus(double.Parse(TextBox702.Text) + GlobalVars.CurrADHP.MagVar, 360.0);
                pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.ALTITUDE;
                pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT;
            }
            else if (OptionButton702.Checked)
            {
                //fTmp = CDbl(TextBox703.Text)
                //pDouble.Value = NativeMethods.Modulus (CDbl(TextBox702.Text) + CurrADHP.MagVar + fTmp * TurnDir, 360.0)
                pSegmentLeg.LegTypeARINC = CodeSegmentPath.VI;
                pSegmentLeg.Course = System.Math.Round(Functions.Dir2Azt(MPtCollection.get_Point(1), MPtCollection.get_Point(1).M), 1);
                pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT;
                pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT;
            }
            else // If OptionButton703.Value Then
            {
                pSegmentLeg.LegPath = CodeTrajectory.STRAIGHT;
                pSegmentLeg.BankAngle = double.Parse(TextBox402.Text);

                //GuidNav = Navaids_DataBase.WPT_FIXToFixableNavaid(TurnDirector);
                //pGuidNavSignPt = TurnDirector.GetSignificantPoint();

                //{

                if (CheckBox701.Checked || TurnDirector.TypeCode == eNavaidType.NONE)
                    pSegmentLeg.LegTypeARINC = CodeSegmentPath.DF;
                else
                    pSegmentLeg.LegTypeARINC = CodeSegmentPath.CF;

                pSegmentLeg.Course = NativeMethods.Modulus(double.Parse(TextBox702.Text) + GlobalVars.CurrADHP.MagVar, 360.0);

                if (Functions.SideDef(TurnFixPnt, DepDir + 90.0, TurnDirector.pPtPrj) < 0)
                    pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.FROM;
                else
                    pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO;
                //}

                // EndPoint ========================
                pEndPoint = new TerminalSegmentPoint();
                //	pEndPoint.IndicatorFACF =	??????????
                //	pEndPoint.LeadDME =			??????????
                //	pEndPoint.LeadRadial =		??????????
                pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.VDP;

                pSegmentPoint = pEndPoint;
                //pSegmentPoint.FlyOver = true;
                pSegmentPoint.RadarGuidance = false;
                pSegmentPoint.ReportingATC = CodeATCReporting.NO_REPORT;
                pSegmentPoint.Waypoint = false;
                //=======================================================================

                pPointReference = new PointReference();

                //IntersectNav = GuidNav;
                //pInterNavSignPt = pGuidNavSignPt;
                //pFIXSignPt = pInterNavSignPt;


                ////if( TurnWPT.TypeCode != eNavaidType.CodeNONE)

                //if (!CheckBox701.Checked)
                //{
                //    Angle = NativeMethods.Modulus((double)pSegmentLeg.Course - TurnWPT.MagVar, 360.0);
                //    //fDir = Functions.Azt2Dir(TurnWPT.pPtGeo, Angle);

                //    if (Functions.ReturnDistanceInMeters(TurnWPT.pPtPrj, TerFixPnt) < 2.0)
                //    {
                //        bOnNav = true;
                //        fDir = Functions.Azt2Dir(TurnWPT.pPtGeo, Angle);
                //    }
                //    else
                //    {
                //        fDir = Functions.ReturnAngleInDegrees(TurnWPT.pPtPrj, TerFixPnt);
                //    }

                //    //    Angle = NativeMethods.Modulus(Functions.Dir2Azt(TurnWPT.pPtPrj, fDir) - TurnWPT.MagVar, 360.0);

                //    pAngleIndication = DBModule.CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt);
                //    pAngleIndication.TrueAngle = Angle;		// Functions.Dir2Azt(TerFixPnt, fDir);
                //    pAngleIndication.IndicationDirection = CodeDirectionReference.FROM;

                //    pAngleUse = new AngleUse();
                //    pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef();
                //    pAngleUse.AlongCourseGuidance = true;

                //    pPointReference.FacilityAngle.Add(pAngleUse);
                //}
                //=======================================================================

                if (TurnDirector.TypeCode != eNavaidType.NONE)
                {
                    //pFIXSignPt.NavaidSystem = TurnDirector.GetFeatureRef();
                    pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT;
                    //pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.CF;
                    pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD;
                    //=======================================================
                    fDir = Functions.Azt2DirPrj(TurnDirector.pPtPrj, (double)pSegmentLeg.Course);
                    fAlt = Functions.DeConvertHeight(fTermAlt);

                    IPointCollection pTermFIXTolerArea;

                    if (TurnDirector.TypeCode == eNavaidType.VOR)
                        Navaids_DataBase.VORFIXTolerArea(TurnDirector.pPtPrj, fDir, fAlt, out pTermFIXTolerArea);
                    else
                        Navaids_DataBase.NDBFIXTolerArea(TurnDirector.pPtPrj, fDir, fAlt, out pTermFIXTolerArea);

                    pTopo = pTermFIXTolerArea as ESRI.ArcGIS.Geometry.ITopologicalOperator2;
                    pTopo.IsKnownSimple_2 = false;
                    pTopo.Simplify();

                    Functions.PriorPostFixTolerance(pTermFIXTolerArea, TurnDirector.pPtPrj, fDir, out PriorFixTolerance, out PostFixTolerance);

                    pDistanceSigned = new Aran.Aim.DataTypes.ValDistanceSigned();
                    pDistanceSigned.Uom = mUomHDistance;
                    pDistanceSigned.Value = Math.Abs(Functions.ConvertDistance(PriorFixTolerance, eRoundMode.NEAREST));
                    pPointReference.PriorFixTolerance = pDistanceSigned;

                    pDistanceSigned = new Aran.Aim.DataTypes.ValDistanceSigned();
                    pDistanceSigned.Uom = mUomHDistance;
                    pDistanceSigned.Value = Math.Abs(Functions.ConvertDistance(PostFixTolerance, eRoundMode.NEAREST));

                    pPointReference.PostFixTolerance = pDistanceSigned;
                    pPointReference.FixToleranceArea = Converters.ESRIPolygonToAIXMSurface(Functions.ToGeo(pTermFIXTolerArea as IGeometry) as IPolygon);
                    // ========================
                    pEndPoint.FacilityMakeup.Add(pPointReference);
                }
                else
                {
                    //pFIXSignPt.FixDesignatedPoint = TurnDirector.GetFeatureRef();
                    pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.ALTITUDE;
                }

                pSegmentPoint.PointChoice = TurnDirector.GetSignificantPoint();
                pSegmentLeg.EndPoint = pEndPoint;
            }

            // End Of EndPoint ========================
            // =======================================================================================

            IPointCollection mPoly = (IPointCollection)Functions.CalcTrajectoryFromMultiPoint(MPtCollection);

            if (!HaveIntercept)
            {
                IPoint ptCnt;

                if (OptionButton701.Checked)
                {
                    IPoint ptTmp = MPtCollection.Point[MPtCollection.PointCount - 1];
                    Functions.CircleVectorIntersect(GlobalVars.CurrADHP.pPtPrj, GlobalVars.RModel, ptTmp, ptTmp.M, out ptCnt);
                }
                else if (OptionButton702.Checked)
                    ptCnt = TerFixPnt;
                else
                    ptCnt = TurnDirector.pPtPrj;

                mPoly.AddPoint(ptCnt);
            }

            IPolyline pPoly = (IPolyline)mPoly;

            //  Length =====================================================
            pDistance = new ValDistance();
            pDistance.Uom = mUomHDistance;

            pDistance.Value = Functions.ConvertDistance(pPoly.Length, eRoundMode.NEAREST);
            pSegmentLeg.Length = pDistance;

            // Trajectory =====================================================
            IGeometryCollection pPolyline = (IGeometryCollection)pPoly;

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

            //===========================================================
            // I protected Area =======================================================
            IPolygon pPolygon1;
            //pPolygon1 = (IPolygon)BaseArea;

            if (!SecPoly.IsEmpty)
            {
                pTopo = (ITopologicalOperator2)(BaseArea);
                pPolygon1 = (IPolygon)pTopo.Difference(SecPoly);
                pTopo = (ITopologicalOperator2)pPolygon1;
                pTopo.IsKnownSimple_2 = false;
                pTopo.Simplify();
            }
            else
                pPolygon1 = (IPolygon)BaseArea;

            //Functions.DrawPolygon(pPolygon1, -1, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal);
            //Functions.DrawPolygon(SecPoly, -1, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal);
            ////while (true)
            //	Application.DoEvents();

            ObstacleAssessmentArea pPrimProtectedArea = new ObstacleAssessmentArea();
            pPrimProtectedArea.Surface = Converters.ESRIPolygonToAIXMSurface(Functions.ToGeo(pPolygon1 as IGeometry) as IPolygon);
            pPrimProtectedArea.SectionNumber = 0;

            if (!OptionButton401.Checked)
            {
                MultiLineString mls = Converters.ESRIPolylineToARANPolyline((IPolyline)Functions.ToGeo((IGeometry)KK));
                pCurve = new Curve();
                pCurve.Geo.Assign(mls);
                pPrimProtectedArea.StartingCurve = pCurve;
            }
            else
                pPrimProtectedArea.StartingCurve = null;

            pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY;
            pPrimProtectedArea.SurfaceZone = CodeObstructionIdSurfaceZone.PRIMARY;

            // II protected Area =======================================================
            //ITopologicalOperator2 
            ObstacleAssessmentArea pSecProtectedArea = null;

            if (!SecPoly.IsEmpty)
            {
                pSecProtectedArea = new ObstacleAssessmentArea();
                pSecProtectedArea.Surface = Converters.ESRIPolygonToAIXMSurface(Functions.ToGeo(SecPoly as IGeometry) as IPolygon);
                pSecProtectedArea.SectionNumber = 1;
                pSecProtectedArea.Type = CodeObstacleAssessmentSurface.SECONDARY;
                //pPrimProtectedArea.SurfaceZone = CodeObstructionIdSurfaceZone.PRIMARY;

                //pSecProtectedArea.StartingCurve = pCurve;
                if (!OptionButton401.Checked)
                {
                    pCurve = new Curve();
                    MultiLineString mls = Converters.ESRIPolylineToARANPolyline((IPolyline)Functions.ToGeo((IGeometry)KK));
                    pCurve.Geo.Assign(mls);
                    pSecProtectedArea.StartingCurve = pCurve;
                }
                else
                    pSecProtectedArea.StartingCurve = null;
            }

            // Protection Area Obstructions list ==================================================
            ObstacleContainer ostacles = oTrnAreaList;
            IRelationalOperator relation = (IRelationalOperator)pPolygon1;
            ObstacleData[] MaxHPenetArray = new ObstacleData[GlobalVars.ArraySize];

            int MaxHPenetArrayCnt = 0;

            if (ostacles.Parts.Length > 0)
            {
                MaxHPenetArray[0] = ostacles.Parts[0];

                double MaxHPenetValue = MaxHPenetArray[0].hPenet;
                double MinHPenetValue = MaxHPenetValue;

                MaxHPenetArrayCnt = 1;

                for (int i = 1; i < ostacles.Parts.Length; i++)
                {
                    int stVal = MaxHPenetArrayCnt < GlobalVars.ArraySize ? MaxHPenetArrayCnt : GlobalVars.ArraySize - 1;
                    int j = stVal;
                    double currPenet = ostacles.Parts[i].hPenet;

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
                        for (; j > 0 && currPenet > MaxHPenetArray[j - 1].hPenet; j--)
                            MaxHPenetArray[j] = MaxHPenetArray[j - 1];
                        MaxHPenetArray[j] = ostacles.Parts[i];

                        if (MaxHPenetArrayCnt < GlobalVars.ArraySize)
                            MaxHPenetArrayCnt++;
                        MinHPenetValue = MaxHPenetArray[MaxHPenetArrayCnt - 1].hPenet;
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
                int isPrimary = 0;
                //double MinimumAltitude = 0;
                double RequiredClearance = 0;
                bool CloseIn = true;

                obs.VerticalStructureObstruction = new FeatureRef(ostacles.Obstacles[owner].Identifier);    //ostacles.Obstacles[i].pFeature.GetFeatureRef();

                for (int j = 0; j < ostacles.Obstacles[owner].PartsNum; j++)
                {
                    if (relation.Disjoint(ostacles.Parts[ostacles.Obstacles[owner].Parts[j]].pPtPrj))
                        continue;

                    //MinimumAltitude = Math.Max(MinimumAltitude, ostacles.Parts[ostacles.Obstacles[owner].Parts[j]].hPenet);
                    RequiredClearance = Math.Max(RequiredClearance, ostacles.Parts[ostacles.Obstacles[owner].Parts[j]].MOC);

                    if (!ostacles.Parts[ostacles.Obstacles[owner].Parts[j]].Ignored)
                        CloseIn = false;

                    if (pSecProtectedArea == null)
                        isPrimary = 1;
                    else if (ostacles.Parts[ostacles.Obstacles[owner].Parts[j]].Prima)
                        isPrimary |= 1;
                    else
                        isPrimary |= 2;
                }

                if (isPrimary == 0)
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

                if ((isPrimary & 1) != 0)
                    pPrimProtectedArea.SignificantObstacle.Add(obs);

                if ((isPrimary & 2) != 0)
                    pSecProtectedArea.SignificantObstacle.Add(obs);
            }

            // Protection Area Obstructions list ==================================================

            pSegmentLeg.DesignSurface.Add(pPrimProtectedArea);

            if (!SecPoly.IsEmpty)
                pSegmentLeg.DesignSurface.Add(pSecProtectedArea);

            //  END of MissedApproachTermination =================================================
            return result;
        }

        private DepartureLeg DepartureTerminationContinued(ref StandardInstrumentDeparture pProcedure, AircraftCharacteristic IsLimitedTo, ref TerminalSegmentPoint pStartPoint)
        {
            DepartureLeg pDepartureLeg;
            SegmentLeg pSegmentLeg;

            TerminalSegmentPoint pEndPoint;
            ValDistance pDistance;

            AngleIndication pAngleIndication;
            SegmentPoint pSegmentPoint;
            Feature pFixDesignatedPoint;
            ValDistanceVertical pDistanceVertical;

            ValSpeed pSpeed;
            UomDistance mUomHDistance;
            UomDistanceVertical mUomVDistance;
            UomSpeed mUomSpeed;

            UomDistance[] uomDistHorzTab = null;
            UomDistanceVertical[] uomDistVerTab = null;
            UomSpeed[] uomSpeedTab = null;

            IPoint pInterceptPt;

            NavaidType GuidNav;
            NavaidType IntersectNav;
            SignificantPoint pGuidNavSignPt;

            uomDistHorzTab = new UomDistance[] { UomDistance.KM, UomDistance.NM, UomDistance.M, UomDistance.FT, UomDistance.MI };
            uomDistVerTab = new UomDistanceVertical[] { UomDistanceVertical.M, UomDistanceVertical.FT };
            uomSpeedTab = new UomSpeed[] { UomSpeed.KM_H, UomSpeed.KT, UomSpeed.M_SEC, UomSpeed.MACH, UomSpeed.M_MIN, UomSpeed.FT_MIN, UomSpeed.FT_SEC };

            mUomHDistance = uomDistHorzTab[GlobalVars.DistanceUnit];
            mUomVDistance = uomDistVerTab[GlobalVars.HeightUnit];
            mUomSpeed = uomSpeedTab[GlobalVars.SpeedUnit];

            pDepartureLeg = DBModule.pObjectDir.CreateFeature<DepartureLeg>();
            pDepartureLeg.Departure = pProcedure.GetFeatureRef();
            pDepartureLeg.AircraftCategory.Add(IsLimitedTo);
            pSegmentLeg = pDepartureLeg;

            GuidNav = Navaids_DataBase.WPT_FIXToNavaid(TurnDirector);
            IntersectNav = TerInterNavDat[ComboBox1002.SelectedIndex];

            pInterceptPt = MPtCollection.get_Point(MPtCollection.PointCount - 1);

            pSegmentLeg.AltitudeInterpretation = CodeAltitudeUse.ABOVE_LOWER;
            pSegmentLeg.UpperLimitReference = CodeVerticalReference.MSL;
            pSegmentLeg.LowerLimitReference = CodeVerticalReference.MSL;
            //pSegmentLeg.TurnDirection = CodeDirectionTurn.EITHER;
            pSegmentLeg.CourseType = CodeCourse.TRUE_TRACK;
            //  ======================================================================
            pSegmentLeg.LegTypeARINC = CodeSegmentPath.CF;

            if (Functions.SideDef(pInterceptPt, pInterceptPt.M + 90.0, GuidNav.pPtPrj) < 0)
                pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.FROM;
            else
                pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO;

            pSegmentLeg.Course = NativeMethods.Modulus(double.Parse(TextBox702.Text) + GuidNav.MagVar, 360.0);

            //  LowerLimitAltitude =======================================================================================
            pDistanceVertical = new ValDistanceVertical();
            pDistanceVertical.Uom = mUomVDistance;

            pDistanceVertical.Value = double.Parse(TextBox1016.Text);
            pSegmentLeg.LowerLimitAltitude = pDistanceVertical;

            //  UpperLimitAltitude =======================================================================================
            //     Set pDistance = New Distance
            //     pDistance.Uom = mVUomDistance
            //     fTmp = ConvertHeight(fTmp, 2)
            //     pDistance.Value = CStr(fTmp)
            //     Set pSegmentLeg.UpperLimitAltitude = pDistance

            //  Length ======
            pDistance = new ValDistance();
            pDistance.Uom = mUomHDistance;

            pDistance.Value = Functions.ConvertDistance(Functions.ReturnDistanceInMeters(pInterceptPt, TerFixPnt), eRoundMode.NEAREST);
            pSegmentLeg.Length = pDistance;

            //  VerticalAngle ====================================================================================================
            pSegmentLeg.VerticalAngle = Functions.RadToDeg(System.Math.Atan(TACurrPDG));
            //  SpeedLimit ======
            pSpeed = new ValSpeed();
            pSpeed.Uom = mUomSpeed;

            pSpeed.Value = double.Parse(TextBox401.Text);
            pSegmentLeg.SpeedLimit = pSpeed;
            pSegmentLeg.SpeedReference = CodeSpeedReference.IAS;

            // Start Point ========================
            //pSegmentLeg.StartPoint = pStartPoint
            // End Of Start Point ========================

            pGuidNavSignPt = GuidNav.GetSignificantPoint();

            //  EndPoint ========================
            pEndPoint = new TerminalSegmentPoint();
            // pEndPoint.IndicatorFACF =      ??????????
            // pEndPoint.LeadDME =            ??????????
            // pEndPoint.LeadRadial =         ??????????
            // pEndPoint.Role = CodeProcedureFixRole.MAHF;
            // ==

            pSegmentPoint = pEndPoint;
            //pSegmentPoint.FlyOver = false;
            pSegmentPoint.RadarGuidance = false;
            pSegmentPoint.ReportingATC = CodeATCReporting.NO_REPORT;
            pSegmentPoint.Waypoint = true;
            // =======================================================================
            double fDir = Functions.ReturnAngleInDegrees(GuidNav.pPtPrj, TerFixPnt);
            double Angle = NativeMethods.Modulus(Functions.Dir2Azt(GuidNav.pPtPrj, fDir) - GuidNav.MagVar, 360.0);

            pAngleIndication = DBModule.CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt);
            pAngleIndication.TrueAngle = Functions.Dir2Azt(TerFixPnt, fDir);
            pAngleIndication.IndicationDirection = CodeDirectionReference.FROM;

            pSegmentLeg.Angle = pAngleIndication.GetFeatureRef();

            // =======================================================================

            PointReference pPointReference = new PointReference();
            SignificantPoint pInterNavSignPt = IntersectNav.GetSignificantPoint();
            SignificantPoint pFIXSignPt;

            //============================================
            if (IntersectNav.ValCnt == -2)
            {
                pFIXSignPt = pInterNavSignPt;
                pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD;
            }
            else
            {
                fDir = Functions.Azt2DirPrj(TerFixPnt, (double)pSegmentLeg.Course);

                double horAccuracy = 0.0;

                if (GuidNav.Identifier != Guid.Empty && GuidNav.TypeCode != eNavaidType.NONE && IntersectNav.TypeCode != eNavaidType.NONE)
                    horAccuracy = Functions.CalcHorisontalAccuracy(TerFixPnt, GuidNav, IntersectNav);

				pFixDesignatedPoint = DBModule.CreateDesignatedPoint(TerFixPnt, "", fDir);

                pFIXSignPt = new SignificantPoint();
                if (pFixDesignatedPoint is DesignatedPoint)
                    pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef();
                else if (pFixDesignatedPoint is Navaid)
                    pFIXSignPt.NavaidSystem = pFixDesignatedPoint.GetFeatureRef();

                pAngleIndication.Fix = pFIXSignPt.GetFeatureRef();
                AngleUse pAngleUse = new AngleUse();
                pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef();
                pAngleUse.AlongCourseGuidance = true;

                pPointReference.FacilityAngle.Add(pAngleUse);

                // ========================

                if (IntersectNav.TypeCode == eNavaidType.DME)
                {
                    double fDistToNav = Functions.ReturnDistanceInMeters(IntersectNav.pPtPrj, TerFixPnt);
                    double fAltitude = TerFixPnt.Z - IntersectNav.pPtPrj.Z + DER.pPtPrj[eRWY.PtDER].Z;
                    double fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitude * fAltitude);
                    DistanceIndication pStDistanceIndication = DBModule.CreateDistanceIndication(Functions.ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, pInterNavSignPt);
                    pStDistanceIndication.Fix = pFIXSignPt.GetFeatureRef();

                    // ========================pStDistanceIndication.
                    pPointReference.FacilityDistance.Add(pStDistanceIndication.GetFeatureRefObject());
                    pPointReference.Role = CodeReferenceRole.RAD_DME;
                }
                else
                {
                    fDir = Functions.ReturnAngleInDegrees(IntersectNav.pPtPrj, TerFixPnt);
                    Angle = NativeMethods.Modulus(Functions.Dir2Azt(IntersectNav.pPtPrj, fDir) - IntersectNav.MagVar, 360.0);

                    AngleIndication pStAngleIndication = DBModule.CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pInterNavSignPt);
                    pStAngleIndication.TrueAngle = Functions.Dir2Azt(TerFixPnt, fDir);
                    pStAngleIndication.Fix = pFIXSignPt.GetFeatureRef();

                    pAngleUse = new AngleUse();
                    pAngleUse.TheAngleIndication = pStAngleIndication.GetFeatureRef();
                    pAngleUse.AlongCourseGuidance = false;
                    // ========================
                    pPointReference.FacilityAngle.Add(pAngleUse);
                    pPointReference.Role = CodeReferenceRole.INTERSECTION;
                }
            }

            double PriorFixTolerance, PostFixTolerance;
            fDir = Functions.Azt2DirPrj(TerFixPnt, (double)pSegmentLeg.Course);
            Functions.PriorPostFixTolerance(pTermFIXTolerArea as IPointCollection, TerFixPnt, fDir, out PriorFixTolerance, out PostFixTolerance);

            ValDistanceSigned pDistanceSigned = new Aran.Aim.DataTypes.ValDistanceSigned();
            pDistanceSigned.Uom = mUomHDistance;
            pDistanceSigned.Value = Math.Abs(Functions.ConvertDistance(PriorFixTolerance, eRoundMode.NEAREST));
            pPointReference.PriorFixTolerance = pDistanceSigned;

            pDistanceSigned = new Aran.Aim.DataTypes.ValDistanceSigned();
            pDistanceSigned.Uom = mUomHDistance;
            pDistanceSigned.Value = Math.Abs(Functions.ConvertDistance(PostFixTolerance, eRoundMode.NEAREST));

            pPointReference.PostFixTolerance = pDistanceSigned;
            pPointReference.FixToleranceArea = Converters.ESRIPolygonToAIXMSurface(Functions.ToGeo(pTermFIXTolerArea) as IPolygon);
            // ========================
            pEndPoint.FacilityMakeup.Add(pPointReference);
            pSegmentPoint.PointChoice = pFIXSignPt;
            pSegmentLeg.EndPoint = pEndPoint;
            // End Of EndPoint ========================

            //  Trajectory =====================================================

            Aran.Geometries.Point pLocation;
            Curve pCurve;
            LineString pLineStringSegment;

            pLineStringSegment = new LineString();

            pLocation = Converters.ESRIPointToARANPoint((IPoint)Functions.ToGeo(pInterceptPt));
            pLineStringSegment.Add(pLocation);

            pLocation = Converters.ESRIPointToARANPoint((IPoint)Functions.ToGeo(TerFixPnt));
            pLineStringSegment.Add(pLocation);

            pCurve = new Curve();
            pCurve.Geo.Add(pLineStringSegment);

            pSegmentLeg.Trajectory = pCurve;
            //  END of MissedApproachTermination =================================================
            return pDepartureLeg;
        }

        private bool SaveProcedure(string sProcName)
        {
            DBModule.pObjectDir.ClearAllFeatures();

            //  Procedure =================================================================================================
            _Procedure = DBModule.pObjectDir.CreateFeature<StandardInstrumentDeparture>();
            //pProcedure.CommunicationFailureDescription
            //pProcedure.Description =
            //pProcedure.ID
            //pProcedure.Note =
            //pProcedure.ProtectsSafeAltitudeAreaId =
            //GuidanceService pGuidanceServiceChose = new GuidanceService();

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
            }
            _Procedure.AircraftCharacteristic.Add(IsLimitedTo);

            FeatureRef featureRef = new FeatureRef();
            featureRef.Identifier = GlobalVars.CurrADHP.Identifier;

            FeatureRefObject featureRefObject = new FeatureRefObject();
            featureRefObject.Feature = featureRef;
            _Procedure.AirportHeliport.Add(featureRefObject);

            //pGuidanceServiceChose.Navaid = FinalNav.pFeature.GetFeatureRef();
            //pProcedure.GuidanceFacility.Add(pGuidanceServiceChose);

            //string sProcName;       // Must be FIX Name sProcName = TurnWPT.Name + " RWY" + DER.Identifier;

            //if (CheckBox301.Checked)
            //{
            //    if (OptionButton701.Checked)
            //        sProcName = "VM" + " RWY" + DER.Name;
            //    else
            //        sProcName = "F" + TextBox1016.Text + " RWY" + DER.Name;
            //}
            //else
            //    sProcName = "VA" + " RWY" + DER.Name;

            _Procedure.Name = sProcName;

            // Transition ==========================================================================
            ProcedureTransition pTransition = new ProcedureTransition();
            pTransition.DepartureRunwayTransition = pLandingTakeoffAreaCollection;
            pTransition.Type = CodeProcedurePhase.RWY;

            //     pTransition.Description =
            //     pTransition.ID =
            //     pTransition.Procedure =
            //     pTransition.TransitionId = TextBox0???.Text

            // Legs ======================================================================================================
            TerminalSegmentPoint pEndPoint;
            SegmentLeg pSegmentLeg;
            ProcedureTransitionLeg ptl;

            // Leg 1 Straight Departure ===============================================================================

            uint NO_SEQ = 1;
            pSegmentLeg = StraightDepartureLeg(ref _Procedure, IsLimitedTo, out pEndPoint);

            ptl = new ProcedureTransitionLeg();
            ptl.SeqNumberARINC = NO_SEQ;
            ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef<AbstractSegmentLegRef>();
            pTransition.TransitionLeg.Add(ptl);

            // Standart Departure ===============================================================================
            if (CheckBox301.Checked)
            {
                // Leg 2 Straight Continued ===============================================================================
                //if (! OptionButton403.Checked)
                //{
                //    NO_SEQ++;
                //    pSegmentLeg = StraightDepartureLegContinued(ref pProcedure, IsLimitedTo, pEndPoint, out pEndPoint);

                //    ptl = new ProcedureTransitionLeg();
                //    ptl.SeqNumberARINC = NO_SEQ;
                //    ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef<AbstractSegmentLegRef>();
                //    pTransition.TransitionLeg.Add(ptl);
                //}
                // Leg 2 Standart Departure ===============================================================================
                NO_SEQ++;
                pSegmentLeg = TurningDepartureLeg(ref _Procedure, IsLimitedTo, ref pEndPoint);

                ptl = new ProcedureTransitionLeg();
                ptl.SeqNumberARINC = NO_SEQ;
                ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef<AbstractSegmentLegRef>();
                pTransition.TransitionLeg.Add(ptl);

                // Leg 3 Standart Departure ===============================================================================
                if (OptionButton702.Checked)
                {
                    NO_SEQ++;
                    pSegmentLeg = DepartureTerminationContinued(ref _Procedure, IsLimitedTo, ref pEndPoint);    //, ref pEndPoint

                    ptl = new ProcedureTransitionLeg();
                    ptl.SeqNumberARINC = NO_SEQ;
                    ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef<AbstractSegmentLegRef>();
                    pTransition.TransitionLeg.Add(ptl);
                }
            }

            //=============================================================================
            _Procedure.FlightTransition.Add(pTransition);

            //DBModule.pObjectDir.Store(pProcedure);
            try
            {
                DBModule.pObjectDir.SetRootFeatureType(Aim.FeatureType.StandardInstrumentDeparture);

                bool saveRes = DBModule.pObjectDir.Commit(new Aim.FeatureType[]{
                        Aim.FeatureType.DesignatedPoint,
                        Aim.FeatureType.AngleIndication,
                        Aim.FeatureType.DistanceIndication,
                        Aim.FeatureType.StandardInstrumentDeparture,
                        Aim.FeatureType.StandardInstrumentArrival,
                        Aim.FeatureType.InstrumentApproachProcedure,
                        Aim.FeatureType.ArrivalFeederLeg,
                        Aim.FeatureType.ArrivalLeg,
                        Aim.FeatureType.DepartureLeg,
                        Aim.FeatureType.FinalLeg,
                        Aim.FeatureType.InitialLeg,
                        Aim.FeatureType.IntermediateLeg,
                        Aim.FeatureType.MissedApproachLeg});

                GlobalVars.gAranEnv.RefreshAllAimLayers();

                return saveRes;
            }
            catch (Exception ex)
            {
                throw new Exception("Error on commit." + "\r\n" + ex.Message);
            }

            //return false;
        }

        private void TextBox1005_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char eventChar = eventArgs.KeyChar;

            if (eventChar == 13)
                TextBox1005_Validating(TextBox1005, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox1005.Text);

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        //private void TextBox1005_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        //{
        //    if (OptionButton703.Checked) return;

        //    double d0;
        //    if (!double.TryParse(TextBox1005.Text, out d0))
        //        return;

        //    d0 = Functions.DeConvertDistance(d0);
        //    if (d0 < PANS_OPS_DataBase.dpTermMinBuffer.Value)
        //    {
        //        d0 = PANS_OPS_DataBase.dpTermMinBuffer.Value;
        //        TextBox1005.Text = Functions.ConvertDistance(d0, eRoundMode.rmNERAEST).ToString();
        //    }
        //    else if (d0 > TurnAreaMaxd0)
        //    {
        //        d0 = TurnAreaMaxd0;
        //        TextBox1005.Text = Functions.ConvertDistance(d0, eRoundMode.rmNERAEST).ToString();
        //    }

        //    if (OptionButton1001.Checked)
        //    {
        //        TextBox1011.Tag = "";
        //        TextBox1011_Validating(TextBox1011, new System.ComponentModel.CancelEventArgs());
        //    }
        //    else if (OptionButton1002.Checked)
        //    {
        //        TextBox1012.Tag = "";
        //        TextBox1012_Validating(TextBox1012, new System.ComponentModel.CancelEventArgs());
        //    }
        //    else if (OptionButton1003.Checked)
        //        OptionButton1003_CheckedChanged(OptionButton1003, new System.EventArgs());

        //    double hTurn = fZNRLen * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value;
        //    double fReachedA = hTurn + d0 * TACurrPDG + DER.pPtPrj[eRWY.PtDER].Z;
        //    TextBox1006.Text = Functions.ConvertHeight(fReachedA, eRoundMode.rmNERAEST).ToString();
        //    // =====================================================================================

        //    IPointCollection mPoly = (IPointCollection)Functions.CalcTrajectoryFromMultiPoint(MPtCollection);

        //    if (OptionButton701.Checked)
        //    {
        //        IPoint pTurnComplitPt = MPtCollection.get_Point(MPtCollection.PointCount - 1);
        //        ITopologicalOperator2 pTopo;

        //        if (OptionButton401.Checked)
        //            pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)ZNR_Poly;
        //        else
        //            pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)KK;

        //        ESRI.ArcGIS.Geometry.Polygon pBufferPolygon = (ESRI.ArcGIS.Geometry.Polygon)pTopo.Buffer(d0);

        //        pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)pBufferPolygon;
        //        pTopo.IsKnownSimple_2 = false;
        //        pTopo.Simplify();
        //        mPoly.AddPoint(Functions.PointAlongPlane(pTurnComplitPt, (pTurnComplitPt.M), 10 * GlobalVars.RModel));

        //        IPolyline pPoly = pTopo.Intersect(mPoly as IGeometry, esriGeometryDimension.esriGeometry1Dimension) as IPolyline;
        //        if (Functions.ReturnDistanceInMeters(pPoly.FromPoint, mPoly.get_Point(0)) > GlobalVars.distEps)
        //            pPoly.ReverseOrientation();

        //        mPoly = (ESRI.ArcGIS.Geometry.IPointCollection)pPoly;
        //    }
        //    else if (OptionButton702.Checked)
        //        mPoly.AddPoint(TerFixPnt);
        //    else if (OptionButton703.Checked)
        //        mPoly.AddPoint(TurnWPT.pPtPrj);

        //    NomInfoFrm.SetInfo(mPoly, hTurn + DER.pPtPrj[eRWY.PtDER].Z, TACurrPDG);
        //}

        private void TextBox1005_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            if (OptionButton701.Checked) return;

            if (TextBox1005.Tag != null && TextBox1005.Tag.ToString() == TextBox1005.Text)
                return;
            TextBox1005.Tag = TextBox1005.Text;

            double dBuffer;
            if (!double.TryParse(TextBox1005.Text, out dBuffer))
                dBuffer = -1.0;
            else
                dBuffer = Functions.DeConvertDistance(dBuffer);

            if (dBuffer < PANS_OPS_DataBase.dpTermMinBuffer.Value)
            {
                dBuffer = PANS_OPS_DataBase.dpTermMinBuffer.Value;
                TextBox1005.Text = Functions.ConvertDistance(dBuffer, eRoundMode.NEAREST).ToString();
            }
            else if (dBuffer > TurnAreaMaxd0)
            {
                dBuffer = TurnAreaMaxd0;
                TextBox1005.Text = Functions.ConvertDistance(dBuffer, eRoundMode.NEAREST).ToString();
            }

            if (OptionButton1001.Checked)
            {
                TextBox1011.Tag = "";
                TextBox1011_Validating(TextBox1011, new System.ComponentModel.CancelEventArgs());
            }
            else if (OptionButton1002.Checked)
            {
                TextBox1012.Tag = "";
                TextBox1012_Validating(TextBox1012, new System.ComponentModel.CancelEventArgs());
            }
            else if (OptionButton1003.Checked)
                OptionButton1003_CheckedChanged(OptionButton1003, new System.EventArgs());
        }

        private void TextBox1007_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char eventChar = eventArgs.KeyChar;

            if (eventChar == 13)
                TextBox1007_Validating(TextBox1007, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox1007.Text);

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        private void TextBox1007_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            double InterToler = 0.0, fDirl = 0.0;
            double TrackToler, hFix, fTmp,
                fDis, dMin, dMax, d0, d;

            IPointCollection pPolyClone, pPoly, pSect1, pLine;
            IPointCollection pSect0 = null;

            IGroupElement pGroupElement;
            ITopologicalOperator2 pTopo;
            IConstructPoint pConstruct;
            IProximityOperator pProxi;
            IPolyline pCutter;
            IPoint pt1, pt2;
            IClone Clone;
            NavaidType TurnNav;

            if (!OptionButton702.Checked)
                return;

            int K = ComboBox1002.SelectedIndex;
            if (K < 0)
                return;

            IPoint pInterceptPt = MPtCollection.Point[MPtCollection.PointCount - 1];

            if (TerInterNavDat[K].ValCnt != -2)
            {
                if (!double.TryParse(TextBox1007.Text, out fDirl))
                    return;

                if (TextBox1007.Tag.ToString() == TextBox1007.Text)
                    return;
            }

            Functions.DeleteGraphicsElement(GlobalVars.TerminationFIXElem);

            pGroupElement = (ESRI.ArcGIS.Carto.IGroupElement)(new GroupElement());

            switch (TerInterNavDat[K].TypeCode)
            {
                case eNavaidType.VOR:
                case eNavaidType.NDB:
                case eNavaidType.LLZ:
                case eNavaidType.TACAN:
                    if (TerInterNavDat[K].ValCnt == -2)
                    {
                        Clone = ((ESRI.ArcGIS.esriSystem.IClone)(TerInterNavDat[K].pPtPrj));
                        TerFixPnt = ((ESRI.ArcGIS.Geometry.IPoint)(Clone.Clone()));
                    }
                    else
                    {
                        fDirl += TerInterNavDat[K].MagVar;

                        if (TerInterNavDat[K].TypeCode == eNavaidType.VOR)
                            InterToler = Navaids_DataBase.VOR.IntersectingTolerance;
                        else if (TerInterNavDat[K].TypeCode == eNavaidType.NDB)
                        {
                            InterToler = Navaids_DataBase.NDB.IntersectingTolerance;
                            fDirl = fDirl - 180.0;
                        }
                        else if (TerInterNavDat[K].TypeCode == eNavaidType.LLZ)
                            InterToler = Navaids_DataBase.LLZ.IntersectingTolerance;

                        fTmp = fDirl;

                        if (!Functions.AngleInSector(fDirl, TerInterNavDat[K].ValMin[0], TerInterNavDat[K].ValMax[0]))
                        {
                            if (Functions.SubtractAngles(fDirl, TerInterNavDat[K].ValMin[0]) < Functions.SubtractAngles(fDirl, TerInterNavDat[K].ValMax[0]))
                                fDirl = TerInterNavDat[K].ValMin[0];
                            else
                                fDirl = TerInterNavDat[K].ValMax[0];
                        }

                        if (fTmp != fDirl)
                        {
                            if ((TerInterNavDat[K].TypeCode == eNavaidType.NDB))
                                TextBox1007.Text = NativeMethods.Modulus(System.Math.Round(fDirl + 180.0 - TerInterNavDat[K].MagVar)).ToString();
                            else
                                TextBox1007.Text = NativeMethods.Modulus(System.Math.Round(fDirl - TerInterNavDat[K].MagVar)).ToString();
                        }

                        fDirl = Functions.Azt2Dir(TerInterNavDat[K].pPtGeo, fDirl);

                        TerFixPnt = new ESRI.ArcGIS.Geometry.Point();
                        pConstruct = (ESRI.ArcGIS.Geometry.IConstructPoint)TerFixPnt;
                        pConstruct.ConstructAngleIntersection(TerInterNavDat[K].pPtPrj, Functions.DegToRad(fDirl), pInterceptPt, Functions.DegToRad(pInterceptPt.M));
                    }
                    break;
                case eNavaidType.DME:
                    fDirl = Functions.DeConvertDistance(fDirl);
                    fTmp = fDirl;

                    int N = TerInterNavDat[K].ValMin.GetUpperBound(0);

                    if (OptionButton1006.Checked || (N == 0))
                    {
                        if (fDirl < TerInterNavDat[K].ValMin[0])
                            fDirl = TerInterNavDat[K].ValMin[0];
                        else if (fDirl > TerInterNavDat[K].ValMax[0])
                            fDirl = TerInterNavDat[K].ValMax[0];
                    }
                    else if (fDirl < TerInterNavDat[K].ValMin[1])
                        fDirl = TerInterNavDat[K].ValMin[1];
                    else if (fDirl > TerInterNavDat[K].ValMax[1])
                        fDirl = TerInterNavDat[K].ValMax[1];

                    if (fTmp != fDirl)
                        TextBox1007.Text = Functions.ConvertDistance(fDirl).ToString();     //				if (fDirl < 1)	fDirl = 1;

                    if ((TerInterNavDat[K].ValCnt < 0) || (OptionButton1006.Enabled && OptionButton1006.Checked))
                        Functions.CircleVectorIntersect(TerInterNavDat[K].pPtPrj, fDirl, pInterceptPt, pInterceptPt.M, out TerFixPnt);
                    else
                        Functions.CircleVectorIntersect(TerInterNavDat[K].pPtPrj, fDirl, pInterceptPt, pInterceptPt.M + 180, out TerFixPnt);
                    break;
            }

            // ==================================================================================================================
            if (OptionButton401.Checked)
                pProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)ZNR_Poly;
            else
                pProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)KK;

            fDis = pProxi.ReturnDistance(TerFixPnt);

            TerFixPnt.M = pInterceptPt.M;

            // ==================================================================================================================

            TerFixPnt.Z = hKK + fDis * TACurrPDG;

            switch (TerInterNavDat[K].TypeCode)
            {
                case eNavaidType.VOR:
                case eNavaidType.NDB:
                case eNavaidType.LLZ:
                case eNavaidType.TACAN:
                    if (TerInterNavDat[K].ValCnt == -2)
                    {
                        if (TerInterNavDat[K].TypeCode == eNavaidType.VOR)
                            Navaids_DataBase.VORFIXTolerArea(TerInterNavDat[K].pPtPrj, pInterceptPt.M, TerFixPnt.Z, out pSect0);
                        else
                            Navaids_DataBase.NDBFIXTolerArea(TerInterNavDat[K].pPtPrj, pInterceptPt.M, TerFixPnt.Z, out pSect0);

                        pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)pSect0;
                        pTopo.IsKnownSimple_2 = false;
                        pTopo.Simplify();
                        pGroupElement.AddElement(Functions.DrawPolygon(pSect0, Functions.RGB(0, 0, 255), esriSimpleFillStyle.esriSFSNull, false));
                    }
                    else
                    {
                        fDis = TerInterNavDat[K].Range;

                        pSect0 = new Polyline();
                        pt1 = Functions.PointAlongPlane(TerInterNavDat[K].pPtPrj, fDirl + InterToler, fDis);
                        pt2 = Functions.PointAlongPlane(TerInterNavDat[K].pPtPrj, fDirl - InterToler, fDis);

                        pSect0.AddPoint(pt1);
                        pSect0.AddPoint(TerInterNavDat[K].pPtPrj);
                        pSect0.AddPoint(pt2);
                        pGroupElement.AddElement(Functions.DrawPolyline(pSect0, Functions.RGB(0, 0, 255), 1, false));
                    }
                    break;
                case eNavaidType.DME:
                    hFix = TerFixPnt.Z - TerInterNavDat[K].pPtPrj.Z;

                    d0 = System.Math.Sqrt(fDirl * fDirl + hFix * hFix) * Navaids_DataBase.DME.ErrorScalingUp + Navaids_DataBase.DME.MinimalError;

                    d = fDirl + d0;
                    pSect0 = (IPointCollection)Functions.CreatePrjCircle(TerInterNavDat[K].pPtPrj, d);

                    pCutter = ((ESRI.ArcGIS.Geometry.IPolyline)(new Polyline()));
                    pCutter.FromPoint = Functions.PointAlongPlane(TerInterNavDat[K].pPtPrj, pInterceptPt.M + 90.0, d + d);
                    pCutter.ToPoint = Functions.PointAlongPlane(TerInterNavDat[K].pPtPrj, pInterceptPt.M - 90.0, d + d);

                    d = fDirl - d0;
                    pSect1 = (IPointCollection)Functions.CreatePrjCircle(TerInterNavDat[K].pPtPrj, d);

                    pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(pSect0));
                    pPoly = pTopo.Difference(pSect1 as IGeometry) as IPointCollection;

                    pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(pPoly));
                    pTopo.IsKnownSimple_2 = false;
                    pTopo.Simplify();

                    if (Functions.SideDef(pCutter.FromPoint, pInterceptPt.M, pCutter.ToPoint) > 0)
                        pCutter.ReverseOrientation();

                    if ((TerInterNavDat[K].ValCnt < 0) || (OptionButton1006.Enabled && OptionButton1006.Checked))
                        pTopo.Cut(pCutter, out pSect1, out pSect0);
                    else
                        pTopo.Cut(pCutter, out pSect0, out pSect1);

                    pGroupElement.AddElement(Functions.DrawPolygon(pSect0, Functions.RGB(0, 0, 255), esriSimpleFillStyle.esriSFSNull, false));
                    break;
            }

            pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)pSect0;
            pTopo.IsKnownSimple_2 = false;
            pTopo.Simplify();

            TurnNav = Navaids_DataBase.WPT_FIXToNavaid(TurnDirector);
            Clone = (ESRI.ArcGIS.esriSystem.IClone)PrimPoly;
            if (TurnNav.TypeCode > eNavaidType.NONE)
            {
                TrackToler = 0.0;

                if (TurnNav.TypeCode == eNavaidType.VOR)
                    TrackToler = Navaids_DataBase.VOR.TrackingTolerance;
                else if (TurnNav.TypeCode == eNavaidType.NDB)
                    TrackToler = Navaids_DataBase.NDB.TrackingTolerance;
                else if (TurnNav.TypeCode == eNavaidType.LLZ)
                    TrackToler = Navaids_DataBase.LLZ.TrackingTolerance;

                pPolyClone = new ESRI.ArcGIS.Geometry.Polygon();
                pPolyClone.AddPoint(TurnNav.pPtPrj);
                pPolyClone.AddPoint(Functions.PointAlongPlane(TurnNav.pPtPrj, pInterceptPt.M + 180.0 - TrackToler, 3.0 * GlobalVars.RModel));
                pPolyClone.AddPoint(Functions.PointAlongPlane(TurnNav.pPtPrj, pInterceptPt.M + 180.0 + TrackToler, 3.0 * GlobalVars.RModel));
                pPolyClone.AddPoint(TurnNav.pPtPrj);
                pPolyClone.AddPoint(Functions.PointAlongPlane(TurnNav.pPtPrj, pInterceptPt.M - TrackToler, 3.0 * GlobalVars.RModel));
                pPolyClone.AddPoint(Functions.PointAlongPlane(TurnNav.pPtPrj, pInterceptPt.M + TrackToler, 3.0 * GlobalVars.RModel));
                pPolyClone.AddPoint(TurnNav.pPtPrj);
                //     Set pFIXPoly__ = Clone.Clone
            }
            else
                pPolyClone = (ESRI.ArcGIS.Geometry.IPointCollection)Clone.Clone();

            pTermFIXTolerArea = (ESRI.ArcGIS.Geometry.IPolygon)Clone.Clone();

            pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)pPolyClone;
            pTopo.IsKnownSimple_2 = false;
            pTopo.Simplify();

            pLine = (IPointCollection)pTopo.Intersect(pSect0, esriGeometryDimension.esriGeometry1Dimension);

            if ((pLine.PointCount == 0) && ((TerInterNavDat[K].TypeCode == eNavaidType.DME) || (TerInterNavDat[K].ValCnt == -2)))
                pLine = (IPointCollection)pTopo.Intersect(pSect0, esriGeometryDimension.esriGeometry2Dimension);

            dMin = 5 * GlobalVars.RModel;
            dMax = -dMin;
            pt1 = pt2 = null;

            for (int i = 0; i < pLine.PointCount; i++)
            {
                d = Functions.Point2LineDistancePrj(pInterceptPt, pLine.Point[i], pInterceptPt.M - 90.0) * Functions.SideDef(pInterceptPt, pInterceptPt.M - 90.0, pLine.Point[i]);
                if (d < dMin)
                {
                    dMin = d;
                    pt1 = pLine.Point[i];
                }

                if (d > dMax)
                {
                    dMax = d;
                    pt2 = pLine.Point[i];
                }
            }

            pLine = new Polyline();

            pLine.AddPoint(Functions.PointAlongPlane(pt1, pInterceptPt.M - 90.0, GlobalVars.RModel));
            pLine.AddPoint(Functions.PointAlongPlane(pt1, pInterceptPt.M + 90.0, GlobalVars.RModel));

            Functions.CutPoly(ref pTermFIXTolerArea, pLine as IPolyline, 1);

            pLine.RemovePoints(0, pLine.PointCount);
            pLine.AddPoint(Functions.PointAlongPlane(pt2, pInterceptPt.M - 90.0, GlobalVars.RModel));
            pLine.AddPoint(Functions.PointAlongPlane(pt2, pInterceptPt.M + 90.0, GlobalVars.RModel));

            Functions.CutPoly(ref pTermFIXTolerArea, pLine as IPolyline, -1);
            // =============================================
            pGroupElement.AddElement(Functions.DrawPolygon(pTermFIXTolerArea, 0, esriSimpleFillStyle.esriSFSNull, false));
            pGroupElement.AddElement(Functions.DrawPointWithText(TerFixPnt, "TerPt", Functions.RGB(0, 0, 255), false));
            GlobalVars.TerminationFIXElem = (ESRI.ArcGIS.Carto.IElement)pGroupElement;

            pGraphics.AddElement(GlobalVars.TerminationFIXElem, 0);
            GlobalVars.TerminationFIXElem.Locked = true;

            GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            TextBox1007.Tag = TextBox1007.Text;
            // ==========================================================================

            if (OptionButton1001.Checked)
            {
                TextBox1011.Tag = "";
                TextBox1011_Validating(TextBox1011, new System.ComponentModel.CancelEventArgs());
            }
            else if (OptionButton1002.Checked)
            {
                TextBox1012.Tag = "";
                TextBox1012_Validating(TextBox1012, new System.ComponentModel.CancelEventArgs());
            }
            else if (OptionButton1003.Checked)
                OptionButton1003_CheckedChanged(OptionButton1003, new System.EventArgs());
        }

        private void TextBox1016_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char eventChar = eventArgs.KeyChar;

            if (eventChar == 13)
                TextBox1016_Validating(TextBox1016, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox1016.Text);

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        private void TextBox1016_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            double fTmp;
            if (!double.TryParse(TextBox1016.Text, out fTmp))
                fTmp = -1;
            //return;

            double NevVal = Functions.DeConvertHeight(fTmp);
            fTmp = NevVal;

            double MinVal = Math.Max(MaxReq + DER.pPtPrj[eRWY.PtDER].Z, hKK);
            double MaxVal = fReachedA;

            //if (MaxVal < MinVal) MaxVal = MinVal;

            if (NevVal < MinVal)
                NevVal = MinVal;

            if (NevVal > MaxVal)
                NevVal = MaxVal;

            if (fTmp != NevVal)
                TextBox1016.Text = Functions.ConvertHeight(NevVal).ToString();
        }

        private void FillCombo1001()
        {
            ComboBox1001.Items.Clear();
            if (!OptionButton702.Checked)
                return;

            ComboBox1001.Items.Add("Create new");

            int k = ComboBox1002.SelectedIndex;
            if (k < 0) return;

            IPoint pInterceptPt = MPtCollection.Point[MPtCollection.PointCount - 1];

            int n = GlobalVars.WPTList.Length;

            for (int i = 0; i < n; i++)
            {
                if (Functions.SideDef(pInterceptPt, pInterceptPt.M + 90, GlobalVars.WPTList[i].pPtPrj) <= 0)
                    continue;

                double fDir = Functions.ReturnAngleInDegrees(pInterceptPt, GlobalVars.WPTList[i].pPtPrj);

                if (Functions.SubtractAngles(fDir, pInterceptPt.M) >= 0.5)
                    continue;

                double fDist = Functions.ReturnDistanceInMeters(pInterceptPt, GlobalVars.WPTList[i].pPtPrj);
                if (fDist >= GlobalVars.RModel)
                    continue;

                if (TerInterNavDat[k].IntersectionType != eIntersectionType.ByDistance && TerInterNavDat[k].IntersectionType != eIntersectionType.RadarFIX)
                    continue;

                if (TerInterNavDat[k].IntersectionType == eIntersectionType.ByDistance)
                {
                    fDist = Functions.ReturnDistanceInMeters(TerInterNavDat[k].pPtPrj, GlobalVars.WPTList[i].pPtPrj);
                    int iSide = Functions.SideDef(TerInterNavDat[k].pPtPrj, pInterceptPt.M + 90, GlobalVars.WPTList[i].pPtPrj);
                    int nN = TerInterNavDat[k].ValMin.Length;

                    if (iSide < 0 || nN == 1)
                    {
                        if (fDist >= TerInterNavDat[k].ValMin[0] && fDist < TerInterNavDat[k].ValMax[0])
                            ComboBox1001.Items.Add(GlobalVars.WPTList[i]);
                    }
                    else if (fDist >= TerInterNavDat[k].ValMin[1] && fDist < TerInterNavDat[k].ValMax[1])
                        ComboBox1001.Items.Add(GlobalVars.WPTList[i]);
                }
                else if (TerInterNavDat[k].IntersectionType == eIntersectionType.ByAngle)
                {
                    fDir = Functions.ReturnAngleInDegrees(TerInterNavDat[k].pPtPrj, GlobalVars.WPTList[i].pPtPrj);

                    if (Functions.AngleInSector(fDir, TerInterNavDat[k].ValMin[0], TerInterNavDat[k].ValMax[0]))
                        ComboBox1001.Items.Add(GlobalVars.WPTList[i]);
                }
            }

            ComboBox1001.SelectedIndex = 0;
        }

        private void ComboBox1001_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (! bFormInitialised)				return;
            if (!OptionButton702.Checked)
                return;

            if (ComboBox1001.SelectedIndex < 1)
                TextBox1007.ReadOnly = false;
            else
            {
                TextBox1007.ReadOnly = true;

                if (TerInterNavDat.Length <= 0)
                    return;

                int k = ComboBox1002.SelectedIndex;

                if (k < 0)
                    return;

                WPT_FIXType selectedWPT = (WPT_FIXType)ComboBox1001.SelectedItem;
                Label1001_18.Text = Navaids_DataBase.GetNavTypeName(selectedWPT.TypeCode);

                //IPolyline pPoly = CalcTrajectoryFromMultiPoint(MPtCollection);
                //DrawPolyLine(pPoly, RGB(255, 0, 0), 2);
                //Application.DoEvents();

                IPoint pInterceptPt = MPtCollection.Point[MPtCollection.PointCount - 1];

                if (TerInterNavDat[k].IntersectionType == eIntersectionType.ByDistance || TerInterNavDat[k].IntersectionType == eIntersectionType.RadarFIX)
                {
                    double fDist = Functions.ReturnDistanceInMeters(TerInterNavDat[k].pPtPrj, selectedWPT.pPtPrj);

                    TextBox1007.Text = Functions.ConvertDistance(fDist - TerInterNavDat[k].Disp, eRoundMode.NONE).ToString("0.0000");
                    if (Functions.SideDef(TerInterNavDat[k].pPtPrj, pInterceptPt.M + 90.0, selectedWPT.pPtPrj) < 0)
                        OptionButton1006.Checked = true;
                    else
                        OptionButton1007.Checked = true;
                }
                else
                {
                    double fDir = Functions.ReturnAngleInDegrees(TerInterNavDat[k].pPtPrj, selectedWPT.pPtPrj);
                    double fAzt = Functions.Dir2Azt(TerInterNavDat[k].pPtPrj, fDir);
                    if (TerInterNavDat[k].TypeCode == eNavaidType.NDB)
                        TextBox1007.Text = Functions.RoundAngle(NativeMethods.Modulus(fAzt + 180.0 - TerInterNavDat[k].MagVar), eRoundMode.NONE).ToString("0.00");
                    else
                        TextBox1007.Text = Functions.RoundAngle(NativeMethods.Modulus(fAzt - TerInterNavDat[k].MagVar), eRoundMode.NONE).ToString("0.00");
                }
            }

            TextBox1007.Tag = "-";
            TextBox1007_Validating(TextBox1007, null);
        }

        private void ComboBox1002_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!OptionButton702.Checked)
                return;

            int k = ComboBox1002.SelectedIndex;
            if (k < 0)
                return;

            int n = TerInterNavDat.Length;

            if (n <= 0)
                return;

            string tipStr;

            Label1001_20.Text = Navaids_DataBase.GetNavTypeName(TerInterNavDat[k].TypeCode);

            if (TerInterNavDat[k].TypeCode == eNavaidType.DME)
            {
                TextBox1007.Visible = true;
                Label1001_22.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;

                n = TerInterNavDat[k].ValMin.Length;

                OptionButton1006.Enabled = n > 1;
                OptionButton1007.Enabled = n > 1;

                if (OptionButton1006.Checked || (n == 1))
                    TextBox1007.Text = Functions.ConvertDistance(TerInterNavDat[k].ValMin[0], eRoundMode.NEAREST).ToString();
                else
                    TextBox1007.Text = Functions.ConvertDistance(TerInterNavDat[k].ValMin[1], eRoundMode.NEAREST).ToString();

                if (n == 1)
                {
                    if (TerInterNavDat[k].ValCnt > 0)
                        OptionButton1006.Checked = true;
                    else
                        OptionButton1007.Checked = true;
                }

                Label1001_21.Text = Resources.str15096; // "??????????:"
                tipStr = Resources.str15097; // "?????????? ???????? ??????????:"

                for (int i = 0; i < n; i++)
                {
                    tipStr = tipStr + Resources.str15098 + Functions.ConvertDistance(TerInterNavDat[k].ValMin[i], eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit +
                                        Resources.str15099 + Functions.ConvertDistance(TerInterNavDat[k].ValMax[i], eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
                    if (i < n - 1)
                        tipStr = Resources.str26101 + " - " + tipStr + "\r\n" +
                            Resources.str26102 + " - ";
                }
            }
            else
            {
                // =============================================================================================
                OptionButton1006.Enabled = false;
                OptionButton1007.Enabled = false;

                TextBox1007.Visible = TerInterNavDat[k].ValCnt != -2;
                if (TerInterNavDat[k].ValCnt == -2)
                {
                    TextBox1007.Text = "";
                    tipStr = "FIX " + Resources.str00106;
                    Label1001_21.Text = tipStr;
                    Label1001_22.Text = "";
                }
                else
                {
                    double Kmin, Kmax;

                    Label1001_22.Text = "°";
                    if (TerInterNavDat[k].TypeCode == eNavaidType.VOR)
                    {
                        Label1001_21.Text = Resources.str15101;
                        Kmax = NativeMethods.Modulus(TerInterNavDat[k].ValMax[0] - TerInterNavDat[k].MagVar);
                        Kmin = NativeMethods.Modulus(TerInterNavDat[k].ValMin[0] - TerInterNavDat[k].MagVar);
                        tipStr = Resources.str15102; // "?????????? ???????? ????????: ?? "
                    }
                    else
                    {
                        Label1001_21.Text = Resources.str15104;
                        Kmax = NativeMethods.Modulus(TerInterNavDat[k].ValMax[0] + 180.0 - TerInterNavDat[k].MagVar);
                        Kmin = NativeMethods.Modulus(TerInterNavDat[k].ValMin[0] + 180.0 - TerInterNavDat[k].MagVar);
                        tipStr = Resources.str15100; // "?????????? ???????? ????????: ?? "
                    }

                    if (TerInterNavDat[k].ValCnt > 0)
                        TextBox1007.Text = Math.Round(Kmin, 1).ToString();
                    else
                        TextBox1007.Text = Math.Round(Kmax, 1).ToString();

                    tipStr = tipStr + Functions.DegToStr(Kmin) + Resources.str15103 + Functions.DegToStr(Kmax);
                }
            }

            if ((TerInterNavDat[k].TypeCode == eNavaidType.DME) || (TerInterNavDat[k].ValCnt != -2))
            {
                tipStr = tipStr.Replace("\r\n", "   ");
                ToolTip1.SetToolTip(TextBox1007, tipStr);
            }

            TextBox1007.Tag = "-";
            TextBox1007_Validating(TextBox1007, new System.ComponentModel.CancelEventArgs());

            FillCombo1001();
        }

        private void OptionButton1004_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!bFormInitialised)
                return;

            if (((RadioButton)eventSender).Checked)
            {
                TextBox1001.Visible = true;
                TextBox1002.Visible = true;
                TextBox1003.Visible = true;
                TextBox1004.Visible = true;

                TextBox1011.Visible = false;
                TextBox1012.Visible = false;
                TextBox1013.Visible = false;
                TextBox1014.Visible = false;
            }
        }

        private void OptionButton1005_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!bFormInitialised)
                return;

            if (((RadioButton)eventSender).Checked)
            {
                TextBox1001.Visible = false;
                TextBox1002.Visible = false;
                TextBox1003.Visible = false;
                TextBox1004.Visible = false;

                TextBox1011.Visible = true;
                TextBox1012.Visible = true;
                TextBox1013.Visible = true;
                TextBox1014.Visible = true;
            }
        }

        private void OptionButton1006_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!bFormInitialised)
                return;

            if (((RadioButton)eventSender).Checked)
            {
                TextBox1007.Tag = "";
                TextBox1007_Validating(TextBox1007, new System.ComponentModel.CancelEventArgs());
            }
        }

        private void OptionButton1007_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!bFormInitialised)
                return;

            if (((RadioButton)eventSender).Checked)
            {
                TextBox1007.Tag = "";
                TextBox1007_Validating(TextBox1007, new System.ComponentModel.CancelEventArgs());
            }
        }

        private double CalcTAParams()
        {
            int i;

            IPoint ptCnt;
            IPolyline pCutter;
            IGeometry pDistPoly;
            ITopologicalOperator2 pTopo;
            IGeometry pFullArea, pSecArea, pTmpPoly;
            IPoint pTurnComplitPt = MPtCollection.Point[MPtCollection.PointCount - 1];

            if (OptionButton401.Checked)
                pDistPoly = (ESRI.ArcGIS.Geometry.IGeometry)ZNR_Poly;
            else
                pDistPoly = KK;

            IProximityOperator pProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)pDistPoly;
            double dBuffer = PANS_OPS_DataBase.dpTermMinBuffer.Value;

            //if (OptionButton703.Checked)
            //{
            //    ptCnt = TurnWPT.pPtPrj;
            //    TurnAreaMaxd0 = pProxi.ReturnDistance(TurnWPT.pPtPrj);
            //    d0 = TurnAreaMaxd0;
            //}
            //else
            //{
            IPoint ptAreaEnd;
            Functions.CircleVectorIntersect(GlobalVars.CurrADHP.pPtPrj, GlobalVars.RModel, pTurnComplitPt, pTurnComplitPt.M, out ptAreaEnd);
            TurnAreaMaxd0 = pProxi.ReturnDistance(ptAreaEnd);

            ptCnt = ptAreaEnd;
            double d0;
            if (OptionButton701.Checked)
            {
                d0 = TurnAreaMaxd0;
                dBuffer = TurnAreaMaxd0;
            }
            else if (OptionButton702.Checked)
            {
                d0 = pProxi.ReturnDistance(pTurnComplitPt);
                if (TurnAreaMaxd0 < d0)
                    TurnAreaMaxd0 = d0;
            }
            else
            {
                //TurnAreaMaxd0 = Functions.ReturnDistanceInMeters(ptCnt, TurnDirector.pPtPrj);
                TurnAreaMaxd0 = pProxi.ReturnDistance(TurnDirector.pPtPrj);

                ptCnt = TurnDirector.pPtPrj;
                //d0 = Functions.ReturnDistanceInMeters(ptCnt, TurnDirector.pPtPrj);
                d0 = TurnAreaMaxd0;
                //0.0;// 
                //ptCnt = TurnDirector.pPtPrj;
                //TurnAreaMaxd0 = Functions.ReturnDistanceInMeters(ptCnt, TurnDirector.pPtPrj);
            }
            //}

            //if (MultiPage1.SelectedIndex < 9)
            TextBox1005.Text = Functions.ConvertDistance(dBuffer, eRoundMode.NEAREST).ToString();
            //else
            //{
            //    if (!double.TryParse(TextBox1005.Text, out dBuffer))
            //        dBuffer = -1.0;
            //    else
            //        dBuffer = Functions.DeConvertDistance(dBuffer);

            //    if (dBuffer < PANS_OPS_DataBase.dpTermMinBuffer.Value)
            //    {
            //        dBuffer = PANS_OPS_DataBase.dpTermMinBuffer.Value;
            //        if (!OptionButton701.Checked)
            //            TextBox1005.Text = Functions.ConvertDistance(dBuffer, eRoundMode.rmNERAEST).ToString();
            //    }
            //}

            //if (OptionButton702.Checked || (OptionButton703.Checked && (TurnWPT.TypeCode > eNavaidType.CodeNONE)))
            if (!OptionButton703.Checked)
            {
                IPoint nPt1 = TurnArea.Point[0];
                IPoint nPt2 = TurnArea.Point[TurnArea.PointCount - 1];
                double fX, fY;

                Functions.PrjToLocal(ptCnt, DirCourse, nPt1, out fX, out fY);
                if (fX > 0)
                    ptCnt = nPt1;

                Functions.PrjToLocal(ptCnt, DirCourse, nPt2, out fX, out fY);
                if (fX > 0)
                    ptCnt = nPt2;
            }

            fZNRLen = Functions.Point2LineDistancePrj(DER.pPtPrj[eRWY.PtDER], KK.ToPoint, DepDir + 90.0);
            double hTurn = fZNRLen * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value;

            TACurrPDG = PANS_OPS_DataBase.dpPDG_Nom.Value;
            pTurnComplitPt.Z = hTurn + d0 * TACurrPDG;

            fReachedA = pTurnComplitPt.Z + DER.pPtPrj[eRWY.PtDER].Z;
            TextBox1006.Text = Functions.ConvertHeight(fReachedA, eRoundMode.NEAREST).ToString();
            TextBox1016.Text = TextBox1006.Text;
            TextBox1016_Validating(TextBox1016, null);
            // =====================================================================================

            pCutter = (IPolyline)(new Polyline());
            pCutter.FromPoint = Functions.LocalToPrj(ptCnt, DirCourse, dBuffer, 100000.0);      //2.0 * GlobalVars.RModel
            pCutter.ToPoint = Functions.LocalToPrj(ptCnt, DirCourse, dBuffer, -100000.0);

            IRelationalOperator pRelation = (IRelationalOperator)pCutter;

            if (pRelation.Disjoint((IGeometry)BaseArea) || pRelation.Disjoint(pCircle))
                pFullArea = (IGeometry)BaseArea;
            else
            {
                pTopo = (ITopologicalOperator2)BaseArea;
                pTopo.Cut(pCutter, out pTmpPoly, out pFullArea);
            }

            if (pRelation.Disjoint(SecPoly) || pRelation.Disjoint(pCircle))
                pSecArea = (IGeometry)SecPoly;
            else
            {
                pTopo = (ITopologicalOperator2)SecPoly;
                pTopo.Cut(pCutter, out pTmpPoly, out pSecArea);
            }

            // =========================================================================================================
            Functions.GetTurnAreaObstacles(oFullList, out oTrnAreaList, (IPolygon)pFullArea, (IPolygon)pSecArea, pDistPoly, DER, DepDir, DirToNav, TurnDirector.pPtPrj, MOCLimit);
            double dTNAh = Functions.CalcTA_Hpenet(oTrnAreaList.Parts, hTurn, TACurrPDG, out i);

            // =========================================================================================================

            MaxReq = MOCLimit;
            int IxMaxReq = -1, n = oTrnAreaList.Parts.Length;

            for (i = 0; i < n; i++)
            {
                //if (oTrnAreaList.Parts[i].Dist < TurnAreaMaxd0)
                //{
                //double CurrReq = oTrnAreaList.Parts[i].Height + oTrnAreaList.Parts[i].MOC;
                double CurrReq = oTrnAreaList.Parts[i].Height + oTrnAreaList.Parts[i].CLShift * MOCLimit;

                if (MaxReq < CurrReq)
                {
                    MaxReq = CurrReq;
                    IxMaxReq = i;
                }

                //if (oTrnAreaList.Parts[i].hPenet > MaxDh)
                //{
                //    MaxDh = oTrnAreaList.Parts[i].hPenet;
                //    IxDh = i;
                //}
                //}
            }

            TextBox1015.Text = Functions.ConvertHeight(MaxReq + DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.NEAREST).ToString();
            //TextBox1016.Text = TextBox1015.Text;
            TextBox1016_Validating(TextBox1016, null);

            ToolTip1.SetToolTip(TextBox1015, "");
            if (IxMaxReq >= 0)
                ToolTip1.SetToolTip(TextBox1015, oTrnAreaList.Obstacles[oTrnAreaList.Parts[IxMaxReq].Owner].UnicalName);
            // =========================================================================================================

            TextBox1001.Text = Math.Round(100.0 * CurrPDG, 1).ToString();
            TextBox1002.Text = Math.Round(100.0 * TACurrPDG, 1).ToString();
            TextBox1003.Text = Functions.ConvertHeight(dTNAh, eRoundMode.CEIL).ToString();

            hKK = hTurn + DER.pPtPrj[eRWY.PtDER].Z;

            TextBox1004.Text = Functions.ConvertHeight(hKK, eRoundMode.NEAREST).ToString();

            TACurrPDG = Functions.CalcTA_PDG(oTrnAreaList.Parts, hTurn, TACurrPDG, out i);

            TextBox1011.Text = TextBox1001.Text;
            TextBox1012.Text = Math.Round(100.0 * TACurrPDG, 1).ToString();
            //TextBox1013.Text = "0"
            TextBox1014.Text = TextBox1004.Text;

            double L1 = Functions.ReturnDistanceInMeters(DER.pPtPrj[eRWY.PtDER], TurnFixPnt);
            AltitudeAtTurnFixPnt = CurrPDG * L1 + PANS_OPS_DataBase.dpH_abv_DER.Value + DER.pPtPrj[eRWY.PtDER].Z;
            InitialTurnFixAltitude = AltitudeAtTurnFixPnt;

            OptionButton1001.Checked = true;

            //ConvertTracToPoints();
            ReportsFrm.FillPage5(oTrnAreaList);

            //============================================================================================================
            IPointCollection mPoly = (IPointCollection)Functions.CalcTrajectoryFromMultiPoint(MPtCollection);
            mPoly.AddPoint(ptCnt);

            if (!OptionButton701.Checked)
            {
                TextBox1005.BackColor = SystemColors.Window;
                TextBox1005.ReadOnly = false;
            }
            else
            {
                TextBox1005.BackColor = SystemColors.ButtonFace;
                TextBox1005.ReadOnly = true;
            }

            NomInfoFrm.SetInfo(mPoly, hTurn + DER.pPtPrj[eRWY.PtDER].Z, TACurrPDG);
            //============================================================================================================

            if (OptionButton702.Checked)
            {
                double DistMin = 0;
                double DistMax = Functions.ReturnDistanceInMeters(ptAreaEnd, pTurnComplitPt);

                TerInterNavDat = Functions.GetValidTurnTermNavs(pTurnComplitPt, DER.pPtPrj[eRWY.PtDER].Z, DistMin, DistMax, pTurnComplitPt.M, TACurrPDG, TurnDirector.TypeCode, TurnDirector.pPtPrj);

                ComboBox1002.Items.Clear();
                n = TerInterNavDat.Length;

                for (i = 0; i < n; i++)
                    ComboBox1002.Items.Add(TerInterNavDat[i].CallSign);

                if (Functions.SideDef(pTurnComplitPt, pTurnComplitPt.M + 90.0, TurnDirector.pPtGeo) > 0)
                {
                    System.Array.Resize<NavaidType>(ref TerInterNavDat, n + 1);
                    TerInterNavDat[n] = Navaids_DataBase.WPT_FIXToNavaid(TurnDirector);
                    n++;
                    ComboBox1002.Items.Add(TurnDirector.Name);
                }

                if (n == 0)
                {
                    TerInterNavDat = new NavaidType[0];
                    TerInterNavDat[0].CallSign = "Radar FIX";
                    TerInterNavDat[0].NAV_Ident = Guid.Empty;
                    TerInterNavDat[0].Identifier = Guid.Empty;
                    TerInterNavDat[0].Name = "Radar FIX";

                    TerInterNavDat[0].TypeCode = eNavaidType.DME;

                    TerInterNavDat[0].MagVar = TurnDirector.MagVar;
                    TerInterNavDat[0].Range = Navaids_DataBase.DME.Range;

                    TerInterNavDat[0].pPtGeo = TurnDirector.pPtGeo;
                    TerInterNavDat[0].pPtPrj = TurnDirector.pPtPrj;
                    // ===============================================================

                    IPoint pt0 = Functions.PointAlongPlane(pTurnComplitPt, pTurnComplitPt.M, DistMin);
                    IPoint pt1 = Functions.PointAlongPlane(pTurnComplitPt, pTurnComplitPt.M, TurnAreaMaxd0);

                    int Side0 = Functions.SideDef(pt0, pTurnComplitPt.M + 90.0, TurnDirector.pPtPrj);
                    int Side1 = Functions.SideDef(pt1, pTurnComplitPt.M + 90.0, TurnDirector.pPtPrj);

                    double Dist0 = Functions.ReturnDistanceInMeters(pt0, TurnDirector.pPtPrj);
                    double Dist1 = Functions.ReturnDistanceInMeters(pt1, TurnDirector.pPtPrj);

                    if (Side0 != Side1)
                    {
                        TerInterNavDat[0].ValCnt = 0;
                        TerInterNavDat[0].ValMax = new double[2];
                        TerInterNavDat[0].ValMin = new double[2];

                        TerInterNavDat[0].ValMin[0] = 1;
                        TerInterNavDat[0].ValMin[1] = 1;

                        if (Side0 > 0)
                        {
                            TerInterNavDat[0].ValMax[0] = Dist0;
                            TerInterNavDat[0].ValMax[1] = Dist1;
                        }
                        else
                        {
                            TerInterNavDat[0].ValMax[0] = Dist1;
                            TerInterNavDat[0].ValMax[1] = Dist0;
                        }
                    }
                    else
                    {
                        TerInterNavDat[0].ValCnt = Side0;
                        TerInterNavDat[0].ValMax = new double[1];
                        TerInterNavDat[0].ValMin = new double[1];

                        TerInterNavDat[0].ValMin[0] = Functions.Min(Dist0, Dist1);
                        TerInterNavDat[0].ValMax[0] = Functions.Max(Dist0, Dist1);
                    }

                    ComboBox1002.Items.Add("Radar FIX");
                }

                ComboBox1002.SelectedIndex = 0;
            }

            return hTurn;
        }

        private void CheckBox501_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox501.Enabled = !(CheckBox501.Checked);
            Label512.Visible = CheckBox501.Enabled;

            SpinButton501.Enabled = CheckBox501.Checked;
            ComboBox501.Enabled = CheckBox501.Checked;
        }
    }
}
