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
    public partial class CDepartRouts : Form
    {
        #region declerations
        private const short noChange = 0;
        private const short goParalel = 1;
        private const short expandAngle = 2;

        private IGraphicsContainer pGraphics;
        private IScreenCapture screenCapture;

        //private ADHPType CurrADHP;

        private WPT_FIXType TurnDirector;
        private WPT_FIXType WPt602;

        private IPoint FlyBy;

        //private ICommandItem[] _mTool;

        private IPolygon pCircle;

        private IPointCollection Prim;
        private IPointCollection SecL;
        private IPointCollection SecR;

        private IPolygon pTermFIXTolerArea;

        private IPolygon SecPoly;

        private IPointCollection pFIXPoly;
        private IPointCollection pPolygon;

        private IPoint ptCenter;

        private IPointCollection BaseArea;
        private IPointCollection BasePoints;
        private IPointCollection ZNR_Poly;
        private IPointCollection TurnArea;

        private IPointCollection TracPoly;
        private IPointCollection MPtCollection;

        private IPoint OutPt;

        private IPoint NJoinPt;
        private IPoint FJoinPt;

        private IPoint NOutPt;
        private IPoint FOutPt;

        private ObstacleContainer oFullList;
        private ObstacleContainer oAllStraightList;
        private ObstacleContainer oZNRList;
        private ObstacleContainer oTrnAreaList;

        private ObstacleData CurrDetObs;
        private ObstacleData PrevDet;

        private bool CheckState;

        private int AirCat;
        private int minAdjustAngle;
        private int iDominicObst;
        private int TrackAdjust;
        private int AdjustDir;

        private int CurrPage;
        private int idPDGMax;
        private int TurnDir;

        private double RMin;
        private double MOCLimit;
        private double MaxReq;
        private double fReachedA;
        private double hKK;
        //private double ZRSegment;
        private double TurnAreaMaxd0;
        private double AltitudeAtTurnFixPnt;
        private double InitialTurnFixAltitude;

        private double fStraightDepartTermAlt;
        private double DirCourse;
        private double OutAzt;


        private double NReqCorrAngle;
        private double FReqCorrAngle;
        private double NCorrAngle;
        private double FCorrAngle;
        private double fBankAngle;
        private double TACurrPDG;

        private double DirToNav;
        private double hMinTurn;
        private double hMaxTurn;
        private double drPDGMax;
        private double CurrPDG;
        private double MinGPDG;
        private double RWYDir;
        private double DepDir;
        private double MinPDG;

        // private double DesPDG; 
        private double MinDR;
        private double fIAS;

        private double MinCurrPDG;

        private RWYType[] RWYList;
        private WPT_FIXType[] FixAngl;
        private WPT_FIXType[] FixWPT;
        private WPT_FIXType[] FixBox602;

        private string PrevPDG;
        private string RecommendStr;

        private IPoint TurnFixPnt;
        private NavaidType[] TurnInterDat;

        private IPoint TerFixPnt;
        private NavaidType[] TerInterNavDat;

        private IPolyline K1K1;
        private IPolyline KK;
        private IPolyline KKFixMax;

        private ReportPoint[] RoutsPoints;
        private double RoutsAllLen;
        private double fZNRLen;

        private StandardInstrumentDeparture _Procedure;

        private bool Report;
        private RWYType DER;
        private CReports ReportsFrm;
        private CNomInfo NomInfoFrm = new CNomInfo();
        private int ArrayIVariant = 1;
        private Label[] Label1;

        private int HelpContextID = 4100;

        ReportFile AccurRep;
        ReportFile RoutsLogRep;
        ReportFile RoutsProtRep;
        ReportFile RoutsGeomRep;

        #endregion

        public CDepartRouts()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            screenCapture = GlobalVars.gAranEnv.GetScreenCapture(FeatureType.StandardInstrumentDeparture.ToString());

            Report = false;
            MinCurrPDG = 0.0;

            //int i;
            //int n = GlobalVars.ADHPList.Length;
            //if (n <= 0)
            //{
            //	MessageBox.Show(Resources.str612, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    this.Close();
            //    return;
            //}

            //ComboBox004.Items.Clear();
            //for (i = 0; i < n; i++)
            //    ComboBox004.Items.Add(GlobalVars.ADHPList[i].Name);

            ToolTip1.SetToolTip(TextBox401, Resources.str15268 + PANS_OPS_DataBase.dpGui_Ar1.Value.ToString() + " ?"); // "??????????? ???????? ?? ?????????? PANS-OPS "" ?"
            ToolTip1.SetToolTip(TextBox402, ToolTip1.GetToolTip(SpinButton401));

            GlobalVars.ButtonControl1State = true;
            GlobalVars.ButtonControl2State = true;
            GlobalVars.ButtonControl3State = true;
            GlobalVars.ButtonControl4State = true;
            GlobalVars.ButtonControl5State = true;
            GlobalVars.ButtonControl6State = true;
            GlobalVars.ButtonControl7State = true;
            GlobalVars.ButtonControl8State = true;

            pGraphics = GlobalVars.GetActiveView().GraphicsContainer;

            //#warning How to find ICommandBars pCommandBars?
            //_mTool = new ICommandItem[8];
            //ICommandBars pCommandBars = GlobalVars.Application.Document.CommandBars;
            //_mTool[0] = pCommandBars.FindById(DepartureGuids.CircleVTool);
            //_mTool[1] = pCommandBars.FindById(DepartureGuids.CLTool);
            //_mTool[2] = pCommandBars.FindById(DepartureGuids.StraightVTool);
            //_mTool[3] = pCommandBars.FindById(DepartureGuids.TurnAreaVTool);
            //_mTool[4] = pCommandBars.FindById(DepartureGuids.SecondaryVTool);
            //_mTool[5] = pCommandBars.FindById(DepartureGuids.NomTrackVTool);
            //_mTool[6] = pCommandBars.FindById(DepartureGuids.KKVTool);
            //_mTool[7] = pCommandBars.FindById(DepartureGuids.FIXVTool);

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
            // RefreshCommandBar mTool

            NCorrAngle = PANS_OPS_DataBase.dpafTrn_ISplay.Value;
            FCorrAngle = PANS_OPS_DataBase.dpafTrn_OSplay.Value;

            BasePoints = new ESRI.ArcGIS.Geometry.Polygon();
            ZNR_Poly = new ESRI.ArcGIS.Geometry.Polygon();

            // ============================================
            CurrPage = 0;
            MultiPage1.SelectedIndex = CurrPage;
            GlobalVars.RModel = 0.0;
            TextBox102.Text = Math.Round(100.0 * PANS_OPS_DataBase.dpPDG_Nom.Value, 1).ToString();

            TextBox911.Left = TextBox901.Left;
            TextBox912.Left = TextBox902.Left;
            TextBox913.Left = TextBox903.Left;
            TextBox914.Left = TextBox904.Left;

            OptionButton904.Checked = true;

            // =============================================================================================

            fBankAngle = PANS_OPS_DataBase.dpT_Bank.Value;
            TextBox302.Text = fBankAngle.ToString();

            ComboBox003.Items.Clear();

            for (int i = 0; i < GlobalVars.EnrouteMOCValues.Length; i++)
                ComboBox003.Items.Add(Functions.ConvertHeight(GlobalVars.EnrouteMOCValues[i], eRoundMode.SPECIAL).ToString());

            ComboBox302.Items.Clear();
            ComboBox302.Items.Add(Resources.str03312);
            ComboBox302.Items.Add(Resources.str03313);

            // ComboBox002.SelectedIndex = 0
            // ComboBox003.SelectedIndex = 0

            ComboBox302.SelectedIndex = 0;
            ComboBox201.SelectedIndex = 0;

            pPolygon = new ESRI.ArcGIS.Geometry.Polygon();

            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            KK = new Polyline() as ESRI.ArcGIS.Geometry.IPolyline;
            K1K1 = new Polyline() as ESRI.ArcGIS.Geometry.IPolyline;

            ReportsFrm = new CReports();
            ReportsFrm.SetBtn(ReportBtn, GlobalVars.ReportHelpIDRouts);
            //GlobalVars.CurrCmd = 2;
            TextBox203.Text = (100 * PANS_OPS_DataBase.dpPDG_Nom.Value).ToString();

            TextBox405.Text = TextBox203.Text;
            TextBox912.Text = TextBox203.Text;

            // CreateLog this.Text
            // ==============================================================
            Text = Resources.str00033;
            MultiPage1.TabPages[0].Text = Resources.str00300;
            MultiPage1.TabPages[1].Text = Resources.str00310;
            MultiPage1.TabPages[2].Text = Resources.str00320;
            MultiPage1.TabPages[3].Text = Resources.str00330;
            MultiPage1.TabPages[4].Text = Resources.str00340;
            MultiPage1.TabPages[5].Text = Resources.str00350;
            MultiPage1.TabPages[6].Text = Resources.str00360;
            MultiPage1.TabPages[7].Text = Resources.str00370;
            MultiPage1.TabPages[8].Text = Resources.str00380;
            MultiPage1.TabPages[9].Text = Resources.str00390;

            // ===============================================================
            PrevBtn.Text = Resources.str00002;
            NextBtn.Text = Resources.str00003;
            OkBtn.Text = Resources.str00004;
            CancelBtn.Text = Resources.str00005;
            ReportBtn.Text = Resources.str00006;
            // ==================Page1=======================================
            Label001.Text = Resources.str03001;
            Label002.Text = Resources.str03002;
            Label003.Text = Resources.str03003;
            Label004.Text = Resources.str03004;
            Label005.Text = Resources.str03005;
            Label006.Text = Resources.str03006;
            //Label007.Text = LoadResString(3007)
            Label008.Text = Resources.str03008;
            Label013.Text = Resources.str01009;
            //Label014.Text = Resources.str73;

            Label2.Text = Resources.str03010;

            LabelA.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label009.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
            Label010.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label011.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
            Label012.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;

            Frame001.Text = Resources.str30001;
            Frame002.Text = Resources.str30002;
            Option3.Text = Resources.str30101;
            Option4.Text = Resources.str30102;
            // ==================Page2======================================
            Label101.Text = Resources.str03101;
            Label102.Text = Resources.str03102;
            Label103.Text = Resources.str03103;
            Label104.Text = Resources.str03104;

            Label108.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;

            AdjustTrack.Text = Resources.str31301;
            ComboBox101.Items.Clear();
            ComboBox101.Items.Add(Resources.str31302);
            ComboBox101.Items.Add(Resources.str31303);

            // ===================Page3======================================
            _Label200_0.Text = Resources.str03105;
            _Label200_2.Text = Resources.str03301;
            Label201.Text = Resources.str03201;
            Label202.Text = Resources.str03202;
            Label203.Text = Resources.str03203;
            // Label205.Caption = LoadResString(3205)
            Label207.Text = Resources.str03207;
            Label208.Text = Resources.str03208;
            Label209.Text = Resources.str03209;
            // Label210.Caption = LoadResString(3210)
            Label211.Text = Resources.str00000;

            _Label200_1.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label212.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;

            CheckBox201.Text = Resources.str32301;
            // ===================Page4=======================================
            Label302.Text = Resources.str03302;
            Label303.Text = Resources.str03303;
            Label306.Text = Resources.str03306;
            Label311.Text = Resources.str03311;

            Label304.Text = GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Unit;
            Label307.Text = Resources.str00902 + "(" + GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Unit + ")";
            Label308.Text = Resources.str00903 + "(" + GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Unit + ")";

            Frame301.Text = Resources.str33001;
            OptionButton301.Text = Resources.str33101;
            OptionButton302.Text = Resources.str33102;
            CheckBox301.Text = Resources.str33301;

            //ToolTip1.SetToolTip(Label307, "—корость промежуточного этапа ухода на второй круг, увеличенна€ на 10%");
            //ToolTip1.SetToolTip(Label308, "—корость конечного этапа ухода на второй круг, увеличенна€ на 10%");

            //ToolTip1.SetToolTip(Label309, "—корость промежуточного этапа ухода на второй круг, увеличенна€ на 10%");
            //ToolTip1.SetToolTip(Label310, "—корость конечного этапа ухода на второй круг, увеличенна€ на 10%");

            // ====================Page5======================================
            Label401.Text = Resources.str03401;
            Label402.Text = Resources.str03402;
            Label403.Text = Resources.str03403;
            Label404.Text = Resources.str03404;
            // Label405.Caption = LoadResString(3405)
            Label407.Text = Resources.str03407;
            Label408.Text = Resources.str03408;
            Label409.Text = Resources.str03409;
            Label412.Text = Resources.str03412;

            Label413.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label414.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;

            CheckBox401.Text = Resources.str34301;
            ComboBox401.Items.Clear();
            ComboBox401.Items.Add(Resources.str34302);
            ComboBox401.Items.Add(Resources.str34303);
            // ====================Page6======================================
            // Label505.Caption = LoadResString(3505)
            // Label507.Caption = LoadResString(3507)
            // Label508.Caption = LoadResString(3508)
            // Label510.Caption = LoadResString(3510)
            // Label512.Caption = LoadResString(3512)
            // Label514.Caption = LoadResString(3514)
            // Label516.Caption = LoadResString(3516)
            // Label518.Caption = LoadResString(3518)
            // Label520.Caption = LoadResString(3520)
            // Label522.Caption = LoadResString(3522)

            Label501.Text = Resources.str03501;
            Label504.Text = Resources.str03504;
            Label506.Text = Resources.str03506;
            Label509.Text = Resources.str03509;
            Label511.Text = Resources.str03511;
            Label513.Text = Resources.str15474 + PANS_OPS_DataBase.dpT_TechToleranc.Value.ToString() + " " + Resources.str00057 + "):";
            Label515.Text = Resources.str03515;
            Label517.Text = Resources.str03517;
            Label519.Text = Resources.str03519;
            Label521.Text = Resources.str03521;
            //Label522.Text = 
            Label523.Text = Resources.str03523;
            Frame501.Text = Resources.str35001;
            Frame502.Text = Resources.str35002;

            Label527.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label528.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label507.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;

            Label529.Text = GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Unit;
            Label530.Text = GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Unit;
            Label531.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
            Label532.Text = Resources.str00058;
            Label533.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
            Label534.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
            Label535.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label536.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            Label537.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;

            Option501.Text = Resources.str35101;
            Option502.Text = Resources.str35102;
            // =====================Page7======================================
            Label601.Text = Resources.str03601;
            Label603.Text = Resources.str03603;
            Label605.Text = Resources.str03605;
            Label607.Text = Resources.str03607;
            Label609.Text = Resources.str03609;
            Label613.Text = Resources.str03610;
            Label614.Text = Resources.str00057;
            Label610.Text = Resources.str03612;

            OptionButton601.Text = Resources.str36101;
            OptionButton602.Text = Resources.str36102;
            OptionButton603.Text = Resources.str36103;
            CheckBox601.Text = Resources.str36301;
            ComboBox603.Items.Clear();
            ComboBox603.Items.Add(Resources.str36304);
            ComboBox603.Items.Add(Resources.str36305);

            // =====================Page8======================================
            Label701.Text = Resources.str03701;
            Label702.Text = Resources.str03702;
            Frame701.Text = Resources.str37001;
            Frame702.Text = Resources.str37002;
            OptionButton701.Text = Resources.str37101;
            OptionButton702.Text = Resources.str37102;
            OptionButton703.Text = Resources.str37103;
            OptionButton704.Text = Resources.str37104;
            OptionButton705.Text = Resources.str37105;
            OptionButton706.Text = Resources.str37106;
            // =====================Page9======================================
            Frame801.Text = Resources.str38001;
            Frame802.Text = Resources.str38002;
            CheckBox801.Text = Resources.str38031;
            CheckBox802.Text = Resources.str38032;
            CheckBox803.Text = Resources.str38033;
            CheckBox804.Text = Resources.str38034;
            CheckBox805.Text = Resources.str38035;
            CheckBox806.Text = Resources.str38036;
            // =====================Page10=====================================
            Frame901.Text = Resources.str39004;
            Frame903.Text = Resources.str03903;

            OptionButton901.Text = Resources.str39011;
            OptionButton902.Text = Resources.str39012;
            OptionButton903.Text = Resources.str39013;
            OptionButton904.Text = Resources.str39001;
            OptionButton905.Text = Resources.str39002;
            OptionButton906.Text = Resources.str35101;
            OptionButton907.Text = Resources.str35102;

            Label901.Text = Resources.str03901;
            Label902.Text = Resources.str03902;
            Label904.Text = Resources.str03904;
            //Label905.Text = My.Resources.str1

            _Label910_0.Text = Resources.str39017;

            _Label910_2.Text = Resources.str39018;
            _Label910_4.Text = Resources.str39016;
            _Label910_6.Text = Resources.str39019;

            _Label911_0.Text = Resources.str03905;
            _Label911_2.Text = Resources.str03501;

            _Label903_0.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            _Label903_1.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            _Label910_1.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
            _Label910_3.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            _Label910_5.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            _Label910_7.Text = GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit;
            // ================================================================

            //  2007
            Label1 = new Label[10] { Label1_00, Label1_01, Label1_02, Label1_03, Label1_04, Label1_05, Label1_06, Label1_07, Label1_08, Label1_09 };

            Label1[0].Text = Resources.str00300;
            Label1[1].Text = Resources.str00310;
            Label1[2].Text = Resources.str00320;
            Label1[3].Text = Resources.str00330;
            Label1[4].Text = Resources.str00340;
            Label1[5].Text = Resources.str00350;
            Label1[6].Text = Resources.str00360;
            Label1[7].Text = Resources.str00370;
            Label1[8].Text = Resources.str00380;
            Label1[9].Text = Resources.str00390;

            FocusStepCaption(0);
            MultiPage1.Top = -21;
            Height = Height - 21;
            Frame1.Top = Frame1.Top - 21;

            ShowPanelBtn.Checked = false;
            this.Width = 534;

            //ComboBox004.SelectedIndex = 0;
            DBModule.FillRWYList(out RWYList, GlobalVars.CurrADHP);

            int n = RWYList.Length;

            if (n == 0)
            {
                MessageBox.Show(Resources.str15056, "PANDA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            for (int i = 0; i < n; i++)
                ComboBox001.Items.Add(RWYList[i].Name);

            DBModule.FillWPT_FIXList(out GlobalVars.WPTList, GlobalVars.CurrADHP, GlobalVars.MaxNAVDist);

            //RMin = 100.0 * System.Math.Round((TMA_Min - PtAirportPrj.Z) / (dpPDG_Nom.Value * 100.0) + 0.5) 
            RMin = 50.0 * System.Math.Round(0.02 * 600.0 / PANS_OPS_DataBase.dpPDG_Nom.Value + 0.4999);
            if (RMin < 20000.0) RMin = 20000.0;
            TextBox003.Text = Functions.ConvertDistance(RMin, eRoundMode.CEIL).ToString();
            //TextBox007.Text = Functions.ConvertHeight(600.0 + GlobalVars.CurrADHP.Elev, eRoundMode.rmNERAEST).ToString();
            //TextBox003_Validating(TextBox003, new System.ComponentModel.CancelEventArgs());

            ComboBox001.SelectedIndex = 0;
        }

        private void DepartRoutsFrm_FormClosed(System.Object eventSender, FormClosedEventArgs eventArgs)
        {
            screenCapture.Rollback();
            DBModule.CloseDB();

            Functions.DeleteGraphicsElement(GlobalVars.pCircleElem);
            GlobalVars.pCircleElem = null;

            Functions.DeleteGraphicsElement(GlobalVars.StraightAreaElem);
            GlobalVars.StraightAreaElem = null;

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
            //CloseLog();
        }

        private void CDepartRoutsFrm_KeyUp(object sender, KeyEventArgs e)
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
            Functions.AppendMenu(hSysMenu, GlobalVars.MF_STRING, GlobalVars.SYSMENU_ABOUT_ID, "&AboutЕ");
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
            double fWd = 0.5 * PANS_OPS_DataBase.dpNGui_Ar1_Wd.Value;

            IPoint pt0 = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], RWYDir - 90.0, fWd);
            IPoint pt5 = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], RWYDir + 90.0, fWd);

            IPoint pt1 = Functions.PointAlongPlane(pt0, RWYDir - PANS_OPS_DataBase.dpAr1_OB_TrAdj.Value - PANS_OPS_DataBase.dpAr1_IB_TrAdj.Value, d0);
            IPoint pt4 = Functions.PointAlongPlane(pt5, RWYDir + PANS_OPS_DataBase.dpAr1_OB_TrAdj.Value + PANS_OPS_DataBase.dpAr1_IB_TrAdj.Value, d0);

            pMaxPolygon.AddPoint(pt0);
            pMaxPolygon.AddPoint(pt1);
            pMaxPolygon.AddPoint(pt4);
            pMaxPolygon.AddPoint(pt5);

            pMaxPolygon = (IPointCollection)Functions.PolygonIntersection(pMaxPolygon, pCircle);

            Functions.GetObstListInPoly(oFullList, out oAllStraightList, (IPolygon)pMaxPolygon);
        }

        private int AdjustI_Zone(double PDG, int Angle = -100, bool DrawFlag = false)
        {
            IPolyline p120Line;

            double dDir;
            double d0;

            if (Angle != -100)
                DepDir = RWYDir + Angle;

            dDir = DepDir;

            Functions.CreateDeparturePolygon(ref pPolygon, GlobalVars.RModel, ArrayIVariant, DER, 0, dDir, RWYDir, PDG);
            p120Line = (ESRI.ArcGIS.Geometry.IPolyline)new Polyline();
            p120Line.FromPoint = pPolygon.Point[1];
            p120Line.ToPoint = pPolygon.Point[4];

            if (CheckBox301.Checked && OptionButton301.Checked)
            {
                d0 = DER.Length + DER.ClearWay - PANS_OPS_DataBase.dpT_Init.Value;
                IPoint Pt2 = Functions.PointAlongPlane(pPolygon.Point[0], RWYDir - 180.0, d0);
                IPoint pt3 = Functions.PointAlongPlane(pPolygon.Point[5], RWYDir + 180.0, d0);
                pPolygon.AddFirstPoint(Pt2);
                pPolygon.AddPoint(pt3);
            }

            IPolygon pPolygon1 = Functions.PolygonIntersection(pPolygon, pCircle);
            Functions.GetZNRObstList(oAllStraightList, out oZNRList, DER, dDir, PDG, pPolygon1);
            int result = Functions.CalcPDGToTop(oZNRList, pPolygon, ArrayIVariant, DER, 0.0, dDir, RWYDir, MOCLimit);

            if (DrawFlag)
            {
                Functions.DeleteGraphicsElement(GlobalVars.StraightAreaElem);

                IGroupElement pGroupElement = (IGroupElement)new GroupElement();
                pGroupElement.AddElement(Functions.DrawPolygon(pPolygon1, Functions.RGB(0, 255, 0), esriSimpleFillStyle.esriSFSNull, false));
                pGroupElement.AddElement(Functions.DrawPolyline(p120Line, Functions.RGB(255, 0, 0), 1, false));
                GlobalVars.StraightAreaElem = (IElement)pGroupElement;

                if (GlobalVars.ButtonControl2State)
                {
                    pGraphics.AddElement(GlobalVars.StraightAreaElem, 0);
                    GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                }
                //Functions.RefreshCommandBar(_mTool, 2);
            }

            return result;
        }

        private Interval CalcLocalRange(double rPDG, out double NewPDG, int AdjustANgle, bool drawFlg = false)
        {
            int indx;
            int iter;
            int m;
            int i;

            double TurnDepDist;
            double PrevRange = 0.0;
            double CurrRange;
            double PrevPDG = 0.0;
            double VTotal;
            double fTASl;
            double MinHd;
            double fHinR;
            double stPDG;
            double sPDG;
            double fTmp;
            double coef;

            double PDG;
            double Rv;
            double dl;

            ObstacleData PrevPrevDet = new ObstacleData();
            ObstacleContainer TmpInList;
            IPointCollection poly;

            // =============================================================================
            bool CanEvasion = true;

            DepDir = RWYDir + AdjustANgle;

            IPointCollection pLine = new Polyline();

            pLine.AddPoint(DER.pPtPrj[eRWY.PtDER]);
            pLine.AddPoint(Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir, 2.0 * GlobalVars.RModel));

            ITopologicalOperator2 pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)pLine;

            IPointCollection pRes = (IPointCollection)pTopoOper.Intersect(pCircle, esriGeometryDimension.esriGeometry0Dimension);
            CurrDetObs = PrevPrevDet;
            CurrDetObs.pPtPrj = pRes.Point[0]; // ????????????????????????
                                               // CurrDetObs.pPtPrj = PointAlongPlane(DER.pPtPrj(ptEnd), DepDir, R)
            CurrDetObs.PDG = PANS_OPS_DataBase.dpPDG_Nom.Value;
            double CurrDetObsHorAccuracy = 0.0;

            CurrDetObs.Dist = Functions.ReturnDistanceInMeters(CurrDetObs.pPtPrj, DER.pPtPrj[eRWY.PtDER]);
            CurrDetObs.Owner = -1;// Resources.str39014;

            iter = 0;
            do
            {
                PrevDet = CurrDetObs;
                double PrevDetHorAccuracy = CurrDetObsHorAccuracy;
                CurrRange = PrevDet.Dist - 5.0 - PrevDetHorAccuracy;

                AdjustI_Zone(PANS_OPS_DataBase.dpPDG_Nom.Value, AdjustANgle, false);
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
                    PrevPDG = PDG;
                    PrevRange = CurrRange;
                    PrevPrevDet = PrevDet;
                }

                if (indx > -1)
                {
                    CurrDetObs = TmpInList.Parts[indx];
                    CurrDetObsHorAccuracy = TmpInList.Obstacles[TmpInList.Parts[indx].Owner].HorAccuracy;
                }
                else
                {
                    CurrDetObs.Owner = -1;// Resources.str39014;
                    CurrDetObsHorAccuracy = 0.0;
                }

                // ====================================================================================
                if (OptionButton301.Checked)
                    fHinR = PDG * CurrRange + PANS_OPS_DataBase.dpH_abv_DER.Value + DER.pPtPrj[eRWY.PtDER].Z;
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

                poly = Functions.ReturnPolygonPartAsPolyline(pPolygon, DER.pPtPrj[eRWY.PtDER], DepDir, TurnDir);

                TurnDepDist = Functions.CalcSpiralStartPoint(poly, ref PrevDet, coef, r0, DepDir, RWYDir, TurnDir, CheckBox301.Checked && OptionButton301.Checked);

                if (TurnDepDist > 0.0)
                {
                    TurnDepDist = TurnDepDist - dl;
                    fTmp = System.Math.Round(TurnDepDist * PDG + PANS_OPS_DataBase.dpH_abv_DER.Value - 0.4999);
                    TurnDepDist = (fTmp - PANS_OPS_DataBase.dpH_abv_DER.Value) / PDG;
                }
                else
                {
                    // TurnDepDist = CurrRange + MinHd
                    break;
                }

                if (OptionButton301.Checked)
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

                PrevPDG = PDG;
                iter++;
            }
            while (true);

            // =============================================================================
            Interval result = new Interval();
            result.Left = 2.0 * GlobalVars.RModel;
            result.Right = result.Left;
            NewPDG = PrevPDG;

            if (CanEvasion)
            {
                result.Left = PrevRange;
                result.Right = PrevRange;

                if (PDG > rPDG)
                    stPDG = PDG;
                else
                    stPDG = rPDG;

                m = (int)System.Math.Round(1000.0 * (PrevPDG - stPDG));
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

                    AdjustI_Zone(sPDG, AdjustANgle, false);

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

                    if (indx > -1)
                        CurrDetObs = TmpInList.Parts[indx];
                    else
                        CurrDetObs.Owner = -1;      // Resources.str39014;
                                                    // ====================================================================================

                    if (OptionButton301.Checked)
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

                    poly = Functions.ReturnPolygonPartAsPolyline(pPolygon, DER.pPtPrj[eRWY.PtDER], DepDir, TurnDir);
                    TurnDepDist = Functions.CalcSpiralStartPoint(poly, ref PrevDet, coef, r0, DepDir, RWYDir, TurnDir, CheckBox301.Checked && OptionButton301.Checked);
                    // ====================================================================================
                    if (OptionButton301.Checked)
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
                            NewPDG = System.Math.Round(sPDG + 0.0004999, 3);
                            result.Left = MinHd;
                            result.Right = TurnDepDist;
                            break;
                        }
                    }
                    // ====================================================================================
                }
            }

            if (drawFlg)
                AdjustI_Zone(NewPDG, AdjustANgle, true);

            return result;
        }

        private Interval CalcGlobalRange(double rPDG, ref double NewPDG, ref int AdjustANgle, bool drawFlg = false)
        {
            int MinAngle = 0;
            int I;
            int cicles = (int)PANS_OPS_DataBase.dpTr_AdjAngle.Value;
            double MinPDG = 9999.0;
            double PDG;

            Interval CurrRange;
            Interval MinRange = new Interval();

            ObstacleData ppTmp = new ObstacleData();
            ObstacleData cTmp = new ObstacleData();

            MinRange.Left = 2.0 * GlobalVars.RModel;
            MinRange.Right = MinRange.Left;
            rPDG = rPDG - 0.9 * GlobalVars.mEps;


            for (I = 0; I <= cicles; I++)
            {
                CurrRange = CalcLocalRange(rPDG, out PDG, I, false);

                if ((PDG < MinPDG) && (CurrRange.Right < GlobalVars.RModel))
                {
                    MinPDG = PDG;
                    MinRange = CurrRange;
                    MinAngle = I;
                    ppTmp = PrevDet;
                    cTmp = CurrDetObs;
                    if (System.Math.Abs(MinPDG - rPDG) <= GlobalVars.mEps)
                        break;
                }
            }

            for (I = 1; I <= cicles; I++)
            {
                CurrRange = CalcLocalRange(rPDG, out PDG, -I, false);
                if ((PDG < MinPDG) && (CurrRange.Right < GlobalVars.RModel) || ((System.Math.Abs(MinPDG - PDG) < GlobalVars.mEps) && (I < MinAngle)))
                {
                    MinPDG = PDG;
                    MinRange = CurrRange;
                    MinAngle = -I;
                    ppTmp = PrevDet;
                    cTmp = CurrDetObs;
                    if (System.Math.Abs(MinPDG - rPDG) <= GlobalVars.mEps)
                        break;
                }
            }
            // ==============================
            Interval result = MinRange;

            //calcGlobalRangeReturn = MinRange;
            NewPDG = MinPDG;

            AdjustANgle = MinAngle;
            PrevDet = ppTmp;
            CurrDetObs = cTmp;

            AdjustI_Zone(NewPDG, AdjustANgle, drawFlg);

            TrackAdjust = AdjustANgle;
            AdjustDir = System.Math.Sign(TrackAdjust);

            ComboBox401.SelectedIndex = (1 - AdjustDir) >> 1;

            SpinButton401.Value = System.Math.Abs(TrackAdjust);

            ToolTip1.SetToolTip(SpinButton401, Resources.str15094 + Functions.DegToStr(TrackAdjust) + " " + ComboBox401.Text); // "??????????? ????????: "
            ToolTip1.SetToolTip(ComboBox401, ToolTip1.GetToolTip(SpinButton401));
            return result;
        }

        private IPointCollection CreateTurnArea(double WSpeed, double TurnR, double V, ref double AztEnter,
                                                ref double AztOut, int pTurnDir, IPointCollection BasePoints)
        {
            double Bank = Functions.Radius2Bank(TurnR, V);
            double Rv = 6355.0 * System.Math.Tan(GlobalVars.DegToRadValue * Bank) / (GlobalVars.PI * V);
            if (Rv > 3.0)
                Rv = 3.0;
            double coef = WSpeed / (Rv * 3.6);

            AztOut = NativeMethods.Modulus(AztOut);
            AztEnter = NativeMethods.Modulus(AztEnter);

            double wAztOut = AztOut;
            double TurnAng;

            if (Functions.SubtractAngles(wAztOut, AztEnter) < 1.0)
                TurnAng = 0.0;
            else
                TurnAng = NativeMethods.Modulus((wAztOut - AztEnter) * pTurnDir);

            double AztCur = AztEnter;
            double AztNext = Functions.ReturnAngleInDegrees(BasePoints.Point[0], BasePoints.Point[1]);

            IPointCollection TmpSpiral = new Polyline();
            IPointCollection result = new ESRI.ArcGIS.Geometry.Polygon();

            double dAng = 0.0;
            int i = 0, n = BasePoints.PointCount;

            do
            {
                if (OptionButton603.Checked && CheckBox601.Checked)
                {
                    double ThetaTouch = Functions.FixToTouchSpiral(BasePoints.Point[i], coef, TurnR, pTurnDir, AztOut, TurnDirector.pPtPrj, DepDir);
                    if (ThetaTouch < -370.0)
                    {
                        MessageBox.Show(Resources.str15095, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        wAztOut = AztOut;
                    }
                    else
                        wAztOut = ThetaTouch;

                    if (Functions.SubtractAngles(wAztOut, AztEnter) < 1.0)
                        TurnAng = 0.0;
                    else
                        TurnAng = NativeMethods.Modulus((wAztOut - AztEnter) * pTurnDir, 360.0);
                }

                double azt12 = Functions.ReturnAngleInDegrees(BasePoints.Point[i], BasePoints.Point[(i + 1) % n]);

                if (System.Math.Abs(NativeMethods.Modulus(azt12, 360.0) - NativeMethods.Modulus(AztCur, 360.0)) < 1.0)
                    AztNext = azt12;
                else if (Functions.SideFrom2Angle(AztNext, azt12) * TurnDir < 0)
                {
                    double dAng0 = NativeMethods.Modulus((AztCur - azt12) * pTurnDir);
                    dAng = dAng - dAng0;
                    AztNext = azt12;

                    Functions.CreateWindSpiral(BasePoints.Point[i], AztEnter, azt12 - 90.0 * TurnDir, ref azt12, TurnR, coef, pTurnDir, TmpSpiral);

                    IPoint PtIntersect = new ESRI.ArcGIS.Geometry.Point();
                    IConstructPoint Constructor = (ESRI.ArcGIS.Geometry.IConstructPoint)PtIntersect;
                    Constructor.ConstructAngleIntersection(result.Point[result.PointCount - 1], GlobalVars.DegToRadValue * AztCur, TmpSpiral.Point[TmpSpiral.PointCount - 1], GlobalVars.DegToRadValue * azt12);

                    result.AddPoint(PtIntersect);
                }
                else
                {
                    double dAng0 = NativeMethods.Modulus((azt12 - AztCur) * pTurnDir);
                    dAng += dAng0;

                    if (dAng < TurnAng)
                        AztNext = azt12;
                    else
                        AztNext = wAztOut;

                    Functions.CreateWindSpiral(BasePoints.Point[i], AztEnter, AztCur, ref AztNext, TurnR, coef, pTurnDir, result);
                }

                i = (i + 1) % n;
                AztCur = AztNext;
            }
            while (Functions.SubtractAngles(AztNext, wAztOut) > GlobalVars.degEps);

            if (OptionButton603.Checked && CheckBox601.Checked)
                AztOut = wAztOut;

            return result;
        }

        private void CalcJoiningParams(eNavaidType NavType, int pTurnDir, IPointCollection TurnArea, IPoint OutPt, double OutAzt)
        {
            IPointCollection AllPolys = null;
            IPoint ptCurr = null;
            IPoint votPovorot = null;
            IConstructPoint Construct = null;
            int Side = 0;
            int Side1 = 0;
            double fTmpAzt = 0;

            ptCurr = new ESRI.ArcGIS.Geometry.Point();
            Construct = ((ESRI.ArcGIS.Geometry.IConstructPoint)(ptCurr));

            Prim = new ESRI.ArcGIS.Geometry.Polygon();
            SecL = new ESRI.ArcGIS.Geometry.Polygon();
            SecR = new ESRI.ArcGIS.Geometry.Polygon();
            AllPolys = new ESRI.ArcGIS.Geometry.Polygon();

            Functions.CreateNavaidZone(TurnDirector.pPtPrj, OutAzt, NavType, 2.0, SecL, SecR, Prim);

            votPovorot = TurnArea.Point[TurnArea.PointCount - 1];

            AllPolys.AddPoint(SecL.Point[5]);
            AllPolys.AddPoint(SecL.Point[0]);
            AllPolys.AddPoint(SecL.Point[1]);

            AllPolys.AddPoint(SecR.Point[2]);
            AllPolys.AddPoint(SecR.Point[3]);
            AllPolys.AddPoint(SecR.Point[4]);

            TextBox701.Text = "";
            TextBox702.Text = "";

            NReqCorrAngle = -1.0;
            FReqCorrAngle = -1.0;

            if (pTurnDir < 0)
            { // sag

                // ================== xarici ========================
                fTmpAzt = Functions.ReturnAngleInDegrees(AllPolys.Point[1], AllPolys.Point[0]);
                Side = Functions.SideFrom2Angle(OutAzt, fTmpAzt);
                Side1 = Functions.SideDef(AllPolys.Point[1], fTmpAzt, votPovorot);

                if (Side * Side1 < 0)
                {
                    if (CheckState)
                    {
                        OptionButton704.Enabled = false;
                        OptionButton706.Enabled = false;
                        Frame702.Enabled = false;
                    }
                }
                else
                {
                    if (CheckState)
                    {
                        Frame702.Enabled = true;
                        OptionButton704.Enabled = true;
                        OptionButton706.Enabled = true;
                    }

                    FReqCorrAngle = Functions.ReturnAngleInDegrees(AllPolys.Point[1], votPovorot);
                    FReqCorrAngle = NativeMethods.Modulus(FReqCorrAngle - fTmpAzt, 360.0);
                    if (FReqCorrAngle > 180.0)
                    {
                        FReqCorrAngle = 360.0 - FReqCorrAngle;
                    }
                    TextBox702.Text = System.Math.Round(FReqCorrAngle + 0.04999, 1).ToString();

                    if (CheckState)
                    {
                        if (FReqCorrAngle > FCorrAngle)
                            OptionButton704.Checked = true;
                        else
                            OptionButton706.Checked = true;
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
                        OptionButton701.Enabled = false;
                        OptionButton703.Enabled = false;
                        Frame701.Enabled = false;
                    }
                }
                else
                {
                    if (CheckState)
                    {
                        Frame701.Enabled = true;
                        OptionButton701.Enabled = true;
                        OptionButton703.Enabled = true;
                    }
                    NReqCorrAngle = Functions.ReturnAngleInDegrees(AllPolys.Point[4], TurnArea.Point[0]);
                    NReqCorrAngle = NativeMethods.Modulus(fTmpAzt - NReqCorrAngle, 360.0);
                    if (NReqCorrAngle > 180.0)
                        NReqCorrAngle = 360.0 - NReqCorrAngle;

                    TextBox701.Text = System.Math.Round(NReqCorrAngle + 0.04999, 1).ToString();

                    if (CheckState)
                    {
                        if (NReqCorrAngle > NCorrAngle)
                            OptionButton701.Checked = true;
                        else
                            OptionButton703.Checked = true;
                    }
                }
            }
            else
            { // sol

                // ================== xarici ========================
                fTmpAzt = Functions.ReturnAngleInDegrees(AllPolys.Point[4], AllPolys.Point[5]);
                Side = Functions.SideFrom2Angle(OutAzt, fTmpAzt);
                Side1 = Functions.SideDef(AllPolys.Point[4], fTmpAzt, votPovorot);

                if (Side * Side1 < 0)
                {
                    if (CheckState)
                    {
                        OptionButton704.Enabled = false;
                        OptionButton706.Enabled = false;
                        Frame702.Enabled = false;
                    }
                }
                else
                {
                    if (CheckState)
                    {
                        Frame702.Enabled = true;
                        OptionButton704.Enabled = true;
                        OptionButton706.Enabled = true;
                    }

                    FReqCorrAngle = Functions.ReturnAngleInDegrees(AllPolys.Point[4], votPovorot);
                    FReqCorrAngle = NativeMethods.Modulus(FReqCorrAngle - fTmpAzt, 360.0);
                    if (FReqCorrAngle > 180.0)
                    {
                        FReqCorrAngle = 360.0 - FReqCorrAngle;
                    }
                    TextBox702.Text = System.Math.Round(FReqCorrAngle + 0.04999, 1).ToString();

                    if (CheckState)
                    {
                        if (FReqCorrAngle > FCorrAngle)
                        {
                            OptionButton704.Checked = true;
                        }
                        else
                        {
                            OptionButton706.Checked = true;
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
                        OptionButton701.Enabled = false;
                        OptionButton703.Enabled = false;
                        Frame701.Enabled = false;
                    }
                }
                else
                {
                    if (CheckState)
                    {
                        Frame701.Enabled = true;
                        OptionButton701.Enabled = true;
                        OptionButton703.Enabled = true;
                    }
                    NReqCorrAngle = Functions.ReturnAngleInDegrees(AllPolys.Point[1], TurnArea.Point[0]);
                    NReqCorrAngle = NativeMethods.Modulus(fTmpAzt - NReqCorrAngle, 360.0);
                    if (NReqCorrAngle > 180.0)
                        NReqCorrAngle = 360.0 - NReqCorrAngle;

                    TextBox701.Text = System.Math.Round(NReqCorrAngle + 0.04999, 1).ToString();

                    if (CheckState)
                    {
                        if (NReqCorrAngle > NCorrAngle)
                            OptionButton701.Checked = true;
                        else
                            OptionButton703.Checked = true;
                    }
                }
            }

        }

        private IPolygon ApplayJoining(eNavaidType NavType, int pTurnDir, IPointCollection TurnArea, IPolygon BasePoints,
                                        IPoint OutPt, double OutAzt, IPointCollection tmpPoly1)
        {
            IPolygon applayJoiningReturn = null;
            IPointCollection ApplayJoining1 = null;
            IPointCollection AllPolys = null;
            IPoint votPovorot = null;
            IPoint antiPovorot = null;
            IConstructPoint Construct = null;
            IClone Clone = null;
            int Side1 = 0;
            int Side = 0;
            double NavOuterAzt = 0;
            double fTmpAzt = 0;

            AllPolys = new ESRI.ArcGIS.Geometry.Polygon();

            Clone = ((ESRI.ArcGIS.esriSystem.IClone)(TurnArea));
            applayJoiningReturn = ((IPolygon)(Clone.Clone()));
            ApplayJoining1 = applayJoiningReturn as IPointCollection;

            votPovorot = TurnArea.Point[TurnArea.PointCount - 1];
            antiPovorot = TurnArea.Point[0];

            NJoinPt = antiPovorot;
            FJoinPt = votPovorot;

            Functions.CreateNavaidZone(TurnDirector.pPtPrj, OutAzt, TurnDirector.TypeCode, 2.0, SecL, SecR, Prim);

            AllPolys.AddPoint(SecL.Point[5]);
            AllPolys.AddPoint(SecL.Point[0]);
            AllPolys.AddPoint(SecL.Point[1]);

            AllPolys.AddPoint(SecR.Point[2]);
            AllPolys.AddPoint(SecR.Point[3]);
            AllPolys.AddPoint(SecR.Point[4]);

            NOutPt = new ESRI.ArcGIS.Geometry.Point();
            FOutPt = new ESRI.ArcGIS.Geometry.Point();

            Construct = ((ESRI.ArcGIS.Geometry.IConstructPoint)(FOutPt));

            if (pTurnDir < 0)           // sag
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
                    if (OptionButton704.Checked)
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
                Side1 = Functions.SideDef(AllPolys.Point[4], fTmpAzt, antiPovorot);

                if (Side * Side1 < 0)
                {
                    NavOuterAzt = OutAzt + PANS_OPS_DataBase.dpafTrn_ISplay.Value * pTurnDir;
                    Construct.ConstructAngleIntersection(antiPovorot, GlobalVars.DegToRadValue * NavOuterAzt, AllPolys.Point[4], GlobalVars.DegToRadValue * fTmpAzt);

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
                    if (OptionButton701.Checked)
                    {
                        fTmpAzt = Functions.ReturnAngleInDegrees(AllPolys.get_Point(4), AllPolys.get_Point(3));
                        Construct.ConstructAngleIntersection(antiPovorot, GlobalVars.DegToRadValue * OutAzt, AllPolys.get_Point(4), GlobalVars.DegToRadValue * fTmpAzt);
                        // DrawPoint NOutPt, 0

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
            else
            { // sol

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
                    if (OptionButton704.Checked)
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

                fTmpAzt = Functions.ReturnAngleInDegrees(AllPolys.get_Point(1), AllPolys.Point[0]);
                Side = Functions.SideFrom2Angle(OutAzt, fTmpAzt);
                Side1 = Functions.SideDef(AllPolys.Point[1], fTmpAzt, antiPovorot);
                Construct = (ESRI.ArcGIS.Geometry.IConstructPoint)NOutPt;

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
                    if (OptionButton701.Checked)
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

            applayJoiningReturn = ApplayJoining1 as IPolygon;

            return applayJoiningReturn;
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

            //IPointCollection pPoints = null;
            //IPointCollection TmpPoly = null;

            //IPoint PtInter15 = null;
            //IPoint ptTmp = null;
            //IPoint ptOut = null;
            //IPoint PtIn = null;

            //ITopologicalOperator2 TopoOper = null;

            //double SplayAngle = 0;
            //double OutDir = 0;
            //double InDir = 0;
            //double Dist1 = 0;
            //double Dist2 = 0;
            //double Dir2Int = 0;
            //double ExtAngle = 0;

            //int SideE = 0;
            //int SideP = 0;

            //int Side = 0;
            //int I = 0;

            LeftPolys.AddPoint(SecL.get_Point(5));
            LeftPolys.AddPoint(SecL.get_Point(0));
            LeftPolys.AddPoint(SecL.get_Point(1));

            RightPolys.AddPoint(SecR.get_Point(2));
            RightPolys.AddPoint(SecR.get_Point(3));
            RightPolys.AddPoint(SecR.get_Point(4));
            TextBox701.Text = "";
            TextBox702.Text = "";

            double InDir = OutAzt + PANS_OPS_DataBase.dpafTrn_OSplay.Value * TurnDir;
            double OutDir = OutAzt - PANS_OPS_DataBase.dpafTrn_OSplay.Value * TurnDir;

            IPoint PtIn = TurnArea.get_Point(0);
            IPoint ptOut = TurnArea.get_Point(TurnArea.PointCount - 1);
            IPoint ptTmp = Functions.PointAlongPlane(PtIn, InDir, 10.0 * GlobalVars.RModel);
            IPoint PtInter15 = null;

            InLine.FromPoint = PtIn;
            InLine.ToPoint = ptTmp;

            OutLine.FromPoint = ptOut;
            ptTmp = Functions.PointAlongPlane(ptOut, OutDir, 10.0 * GlobalVars.RModel);
            OutLine.ToPoint = ptTmp;


            eNavaidType NavType = TurnDirector.TypeCode;

            double SplayAngle;
            if (NavType == eNavaidType.NDB)
                SplayAngle = Navaids_DataBase.NDB.SplayAngle;
            else //if (NavType == eNavaidType.CodeVOR)
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
                        OptionButton701.Enabled = false;
                        OptionButton703.Enabled = false;
                        OptionButton702.Enabled = false;
                        Frame701.Enabled = false;
                    }

                    if (pPoints.PointCount > 1)
                    {
                        double minDist = 10.0 * GlobalVars.RModel;
                        for (i = 0; i <= pPoints.PointCount - 1; i++)
                        {
                            double currDist = Functions.ReturnDistanceInMeters(PtIn, pPoints.get_Point(i));
                            if (currDist < minDist)
                            {
                                minDist = currDist;
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
                        TmpPoly.AddFirstPoint(LeftPolys.get_Point(1));
                        ptTmp = Functions.PointAlongPlane(LeftPolys.get_Point(1), DirToNav + SplayAngle, 10.0 * GlobalVars.RModel);
                    }

                    TmpPoly.AddFirstPoint(ptTmp);
                }
                else
                {
                    if (CheckState)
                        Frame701.Enabled = true;

                    SideP = -1;
                    Side = -1;
                    SideE = -1;


                    if (Functions.RayPolylineIntersect(LeftPolys, PtIn, DirToNav, ref PtInterP))
                        SideP = Functions.SideDef(PtIn, DirToNav + 90.0, PtInterP);

                    double Dir2Int = Functions.ReturnAngleInDegrees(PtIn, LeftPolys.get_Point(1));
                    double ExtAngle = Functions.SubtractAngles(Dir2Int, DirToNav);
                    TextBox701.Text = System.Math.Round(ExtAngle, 1).ToString();

                    if (ExtAngle <= 90.0)
                    {
                        PtInterE = new ESRI.ArcGIS.Geometry.Point();
                        PtInterE.PutCoords(PtIn.X, PtIn.Y);
                        SideE = Functions.SideDef(PtIn, Dir2Int + 90.0, PtInterE);
                    }

                    if (Functions.RayPolylineIntersect(LeftPolys, PtIn, DirToNav + PANS_OPS_DataBase.dpafTrn_OSplay.Value, ref PtInter15))
                        Side = Functions.SideDef(PtIn, DirToNav + PANS_OPS_DataBase.dpafTrn_OSplay.Value + 90.0, PtInter15);

                    OptionButton701.Enabled = SideP > 0;
                    OptionButton702.Enabled = Side > 0;
                    OptionButton703.Enabled = SideE > 0;

                    //         Frame701.Enabled = OptionButton701.Enabled And OptionButton703.Enabled

                    // ======================================================================================
                    if (CheckState)
                    {
                        if (OptionButton703.Enabled)
                        {
                            OptionButton703.Checked = true;
                            ptTmp = PtInterE;
                        }
                        else if (OptionButton701.Enabled)
                        {
                            OptionButton701.Checked = true;
                            ptTmp = PtInterP;
                        }
                        else if (OptionButton702.Enabled)
                        {
                            OptionButton702.Checked = true;
                            ptTmp = PtInter15;
                        }
                    }
                    else
                    {
                        if (OptionButton703.Checked)
                            ptTmp = PtInterE;
                        else if (OptionButton701.Checked)
                            ptTmp = PtInterP;
                        else
                            ptTmp = PtInter15;
                    }

                    NJoinPt = new ESRI.ArcGIS.Geometry.Point();
                    NJoinPt.PutCoords(ptTmp.X, ptTmp.Y);

                    TmpPoly.AddFirstPoint(ptTmp);
                    Side = Functions.SideDef(ptTmp, DirToNav + 90.0, TurnDirector.pPtPrj);

                    if (Side < 0)
                        ptTmp = Functions.PointAlongPlane(ptTmp, DirToNav + SplayAngle, 10.0 * GlobalVars.RModel);
                    else
                    {
                        TmpPoly.AddFirstPoint(LeftPolys.get_Point(1));
                        ptTmp = Functions.PointAlongPlane(LeftPolys.get_Point(1), DirToNav + SplayAngle, 10.0 * GlobalVars.RModel);
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
                        OptionButton704.Enabled = false;
                        OptionButton705.Enabled = false;
                        Frame702.Enabled = false;
                    }

                    if (pPoints.PointCount > 1)
                    {
                        double minDist = 10.0 * GlobalVars.RModel;
                        for (i = 0; i <= pPoints.PointCount - 1; i++)
                        {
                            double currDist = Functions.ReturnDistanceInMeters(ptOut, pPoints.get_Point(i));
                            if (currDist < minDist)
                            {
                                minDist = currDist;
                                FJoinPt = pPoints.get_Point(i);
                            }
                        }
                    }
                    else
                        FJoinPt = pPoints.get_Point(0);

                    TmpPoly.AddPoint(FJoinPt);
                    Side = Functions.SideDef(FJoinPt, DirToNav + 90.0, TurnDirector.pPtPrj);

                    if (Side < 0)
                        ptTmp = Functions.PointAlongPlane(FJoinPt, DirToNav - SplayAngle, 10.0 * GlobalVars.RModel);
                    else
                    {
                        TmpPoly.AddPoint(RightPolys.get_Point(1));
                        ptTmp = Functions.PointAlongPlane(RightPolys.get_Point(1), DirToNav - SplayAngle, 10.0 * GlobalVars.RModel);
                    }

                    TmpPoly.AddPoint(ptTmp);
                }
                else
                {
                    if (CheckState)
                    {
                        Frame702.Enabled = true;
                        OptionButton704.Enabled = true;
                        OptionButton705.Enabled = true;
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

                    OptionButton704.Enabled = SideP > 0;
                    OptionButton705.Enabled = Side > 0;

                    //         Frame702.Enabled = OptionButton704.Enabled And OptionButton705.Enabled

                    // ======================================================================================
                    if (CheckState)
                    {
                        if (OptionButton705.Enabled)
                        {
                            OptionButton705.Checked = true;
                            ptTmp = PtInter15;
                        }
                        else if (OptionButton704.Enabled)
                        {
                            OptionButton704.Checked = true;
                            ptTmp = PtInterP;
                        }
                    }
                    else
                    {
                        if (OptionButton705.Checked)
                            ptTmp = PtInter15;
                        else if (OptionButton704.Checked)
                            ptTmp = PtInterP;
                    }

                    FJoinPt = new ESRI.ArcGIS.Geometry.Point();
                    FJoinPt.PutCoords(ptTmp.X, ptTmp.Y);

                    TmpPoly.AddPoint(ptTmp);
                    Side = Functions.SideDef(ptTmp, DirToNav + 90.0, TurnDirector.pPtPrj);

                    if (Side < 0)
                        ptTmp = Functions.PointAlongPlane(ptTmp, DirToNav - SplayAngle, 10.0 * GlobalVars.RModel);
                    else
                    {
                        TmpPoly.AddPoint(RightPolys.get_Point(1));
                        ptTmp = Functions.PointAlongPlane(RightPolys.get_Point(1), DirToNav - SplayAngle, 10.0 * GlobalVars.RModel);
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
                        OptionButton701.Enabled = false;
                        OptionButton702.Enabled = false;
                        OptionButton703.Enabled = false;
                        Frame701.Enabled = false;
                    }

                    if (pPoints.PointCount > 1)
                    {
                        double minDist = 10.0 * GlobalVars.RModel;
                        for (i = 0; i < pPoints.PointCount; i++)
                        {
                            double currDist = Functions.ReturnDistanceInMeters(PtIn, pPoints.Point[i]);
                            if (currDist < minDist)
                            {
                                minDist = currDist;
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
                        TmpPoly.AddFirstPoint(RightPolys.get_Point(1));
                        ptTmp = Functions.PointAlongPlane(RightPolys.get_Point(1), DirToNav - SplayAngle, 10.0 * GlobalVars.RModel);
                    }

                    TmpPoly.AddFirstPoint(ptTmp);
                }
                else
                {
                    if (CheckState)
                        Frame701.Enabled = true;

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
                    TextBox701.Text = System.Math.Round(ExtAngle, 1).ToString();

                    if (ExtAngle <= 90.0)
                    {
                        PtInterE = new ESRI.ArcGIS.Geometry.Point();
                        PtInterE.PutCoords(PtIn.X, PtIn.Y);
                        SideE = Functions.SideDef(PtIn, Dir2Int + 90.0, PtInterE);
                    }

                    if (Functions.RayPolylineIntersect(RightPolys, PtIn, DirToNav - PANS_OPS_DataBase.dpafTrn_OSplay.Value, ref PtInter15))
                        Side = Functions.SideDef(PtIn, DirToNav - PANS_OPS_DataBase.dpafTrn_OSplay.Value + 90.0, PtInter15);

                    OptionButton701.Enabled = SideP > 0;
                    OptionButton702.Enabled = Side > 0;
                    OptionButton703.Enabled = SideE > 0;
                    //         Frame701.Enabled = OptionButton701.Enabled And OptionButton703.Enabled
                    // ======================================================================================
                    if (CheckState)
                    {
                        if (OptionButton703.Enabled)
                        {
                            OptionButton703.Checked = true;
                            ptTmp = PtInterE;
                        }
                        else if (OptionButton701.Enabled)
                        {
                            OptionButton701.Checked = true;
                            ptTmp = PtInterP;
                        }
                        else if (OptionButton702.Enabled)
                        {
                            OptionButton702.Checked = true;
                            ptTmp = PtInter15;
                        }
                    }
                    else if (OptionButton703.Checked)
                        ptTmp = PtInterE;
                    else if (OptionButton701.Checked)
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
                        OptionButton704.Enabled = false;
                        OptionButton705.Enabled = false;
                        Frame702.Enabled = false;
                    }

                    if (pPoints.PointCount > 1)
                    {
                        double minDist = 10.0 * GlobalVars.RModel;
                        for (i = 0; i < pPoints.PointCount; i++)
                        {
                            double currDist = Functions.ReturnDistanceInMeters(ptOut, pPoints.Point[i]);
                            if (currDist < minDist)
                            {
                                minDist = currDist;
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
                        Frame702.Enabled = true;
                        OptionButton704.Enabled = true;
                        OptionButton705.Enabled = true;
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

                    OptionButton704.Enabled = SideP > 0;
                    OptionButton705.Enabled = Side > 0;

                    //         Frame702.Enabled = OptionButton704.Enabled And OptionButton705.Enabled

                    // ======================================================================================
                    if (CheckState)
                    {
                        if (OptionButton705.Enabled)
                        {
                            OptionButton705.Checked = true;
                            ptTmp = PtInter15;
                        }
                        else if (OptionButton704.Enabled)
                        {
                            OptionButton704.Checked = true;
                            ptTmp = PtInterP;
                        }
                    }
                    else if (OptionButton705.Checked)
                        ptTmp = PtInter15;
                    else if (OptionButton704.Checked)
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

        private void SecondArea(int pTurnDir, IPointCollection pTurnArea)
        {
            if (!(OptionButton603.Checked || OptionButton602.Checked))
            {
                MessageBox.Show(Resources.str00901, null, MessageBoxButtons.OK, MessageBoxIcon.Error);  //	MsgBox "Invalid options", vbExclamation, "PANDA"
                return;
            }

            IPoint ptOut = MPtCollection.get_Point(MPtCollection.PointCount - 1);
            double OutAzt = ptOut.M;

            Prim = new ESRI.ArcGIS.Geometry.Polygon();
            SecL = new ESRI.ArcGIS.Geometry.Polygon();
            SecR = new ESRI.ArcGIS.Geometry.Polygon();

            Functions.CreateNavaidZone(TurnDirector.pPtPrj, OutAzt, TurnDirector.TypeCode, 5.0, SecL, SecR, Prim);

            IClone Clone = ((ESRI.ArcGIS.esriSystem.IClone)(pTurnArea));
            IPointCollection GeneralArea = ((ESRI.ArcGIS.Geometry.IPointCollection)(Clone.Clone()));

            IPointCollection IArea = new ESRI.ArcGIS.Geometry.Polygon();
            IArea.AddPoint(SecR.get_Point(5));
            IArea.AddPoint(SecR.get_Point(0));
            IArea.AddPoint(SecR.get_Point(1));

            IArea.AddPoint(SecL.get_Point(2));
            IArea.AddPoint(SecL.get_Point(3));
            IArea.AddPoint(SecL.get_Point(4));

            ITopologicalOperator2 Topo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(IArea));
            Topo.IsKnownSimple_2 = false;
            Topo.Simplify();

            Topo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(GeneralArea));
            Topo.IsKnownSimple_2 = false;
            Topo.Simplify();

            IPolyline Cutter = ((ESRI.ArcGIS.Geometry.IPolyline)(new Polyline()));

            double Dist0;
            if (TurnDirector.TypeCode == eNavaidType.NDB)
                Dist0 = Navaids_DataBase.NDB.Range * 10.0;
            else //if (NavType == eNavaidType.CodeVOR)
                Dist0 = Navaids_DataBase.VOR.Range * 10.0;

            IPoint ptFar = Functions.PointAlongPlane(TurnDirector.pPtPrj, OutAzt, Dist0);
            Cutter.ToPoint = ptFar;
            Cutter.FromPoint = Functions.PointAlongPlane(TurnDirector.pPtPrj, OutAzt + 180.0, Dist0);

            IPointCollection TmpPoly;
            PolyCut.ClipByLine(GeneralArea, Cutter, out SecL, out SecR, out TmpPoly);

            Topo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(IArea));
            Topo.IsKnownSimple_2 = false;
            Topo.Simplify();

            Topo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(SecR));
            Topo.IsKnownSimple_2 = false;
            Topo.Simplify();

            SecR = Topo.Difference(IArea) as IPointCollection;

            Topo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(SecL));
            Topo.IsKnownSimple_2 = false;
            Topo.Simplify();

            SecL = Topo.Difference(IArea) as IPointCollection;

            SecR = Functions.RemoveFars(SecR, ptFar);
            SecL = Functions.RemoveFars(SecL, ptFar);

            IConstructPoint Construct;
            IPolygon pTmpPoly;
            IPoint ptCut;
            double TmpDir;

            if (CheckBox801.Checked)
            {
                if (OptionButton603.Checked)
                { // Na FIX

                    // ==================== Outer ==================
                    IPoint ptCutOuter = new ESRI.ArcGIS.Geometry.Point(), ptCutInner;
                    Construct = ((ESRI.ArcGIS.Geometry.IConstructPoint)(ptCutOuter));
                    Construct.ConstructAngleIntersection(ptOut, GlobalVars.DegToRadValue * OutAzt, FOutPt, GlobalVars.DegToRadValue * (OutAzt + 90.0));
                    int SideIn = Functions.SideDef(ptOut, OutAzt + 90.0, ptCutOuter);

                    if (SideIn < 0)
                        ptCutOuter.PutCoords(ptOut.X, ptOut.Y);

                    double OutDist = SideIn * Functions.ReturnDistanceInMeters(ptOut, ptCutOuter);
                    // ==================== Inner ==================
                    IPoint ptCutInner0 = new ESRI.ArcGIS.Geometry.Point();
                    IPoint ptCutInner1 = new ESRI.ArcGIS.Geometry.Point();

                    Construct = ((ESRI.ArcGIS.Geometry.IConstructPoint)(ptCutInner0));
                    Construct.ConstructAngleIntersection(ptOut, GlobalVars.DegToRadValue * OutAzt, NOutPt, GlobalVars.DegToRadValue * (OutAzt + 90.0));

                    if (ptCutInner0.IsEmpty)
                        ptCutInner0.PutCoords(ptCutOuter.X, ptCutOuter.Y);

                    Construct = ((ESRI.ArcGIS.Geometry.IConstructPoint)(ptCutInner1));
                    Construct.ConstructAngleIntersection(ptOut, GlobalVars.DegToRadValue * OutAzt, NOutPt, GlobalVars.DegToRadValue * DepDir);

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
                    Cutter.FromPoint = ptCut;
                    Cutter.ToPoint = Functions.PointAlongPlane(ptCut, OutAzt - 90.0 * TurnDir, 2.0 * GlobalVars.RModel);

                    if (pTurnDir > 0)
                    {
                        PolyCut.ClipByLine(SecR, Cutter, out pTmpPoly, out PolyCut.MissingPolygon, out PolyCut.MissingPolygon);
                        if (!(pTmpPoly.IsEmpty))
                            SecR = Functions.RemoveFars(pTmpPoly, ptFar);
                    }
                    else
                    {
                        PolyCut.ClipByLine(SecL, Cutter, out PolyCut.MissingPolygon, out pTmpPoly, out PolyCut.MissingPolygon);
                        if (!(pTmpPoly.IsEmpty))
                            SecL = Functions.RemoveFars(pTmpPoly, ptFar);
                    }

                    // ==================== Inner ==================
                    if (bFlg)
                        Cutter.ToPoint = Functions.PointAlongPlane(ptCut, OutAzt + 90.0 * TurnDir, 2.0 * GlobalVars.RModel);
                    else
                    {
                        Cutter.ToPoint = Functions.PointAlongPlane(ptCut, DepDir + 180.0, 2.0 * GlobalVars.RModel);
                        Cutter.FromPoint = Functions.PointAlongPlane(ptCut, DepDir, 2.0 * GlobalVars.RModel);

                        if (Functions.SideDef(Cutter.ToPoint, OutAzt, Cutter.FromPoint) != TurnDir)
                            Cutter.ReverseOrientation();
                    }

                    if (pTurnDir > 0)
                    {
                        PolyCut.ClipByLine(SecL, Cutter, out PolyCut.MissingPolygon, out pTmpPoly, out PolyCut.MissingPolygon);
                        if (!(pTmpPoly.IsEmpty))
                            SecL = Functions.RemoveFars(pTmpPoly, ptFar);
                    }
                    else
                    {
                        PolyCut.ClipByLine(SecR, Cutter, out pTmpPoly, out PolyCut.MissingPolygon, out PolyCut.MissingPolygon);
                        if (!(pTmpPoly.IsEmpty))
                            SecR = Functions.RemoveFars(pTmpPoly, ptFar);
                    }
                }
                else
                { // Na curs FIX
                  // ==================== Outer ==================
                    Cutter.FromPoint = FlyBy;
                    double DrDir = MPtCollection.Point[1].M;

                    if (Functions.SideDef(FlyBy, DrDir, ptOut) == TurnDir)
                        Cutter.ToPoint = Functions.PointAlongPlane(FlyBy, OutAzt - 90.0 * TurnDir, 2.0 * GlobalVars.RModel);
                    else
                        Cutter.ToPoint = Functions.PointAlongPlane(FlyBy, DrDir - 90.0 * TurnDir, 2.0 * GlobalVars.RModel);

                    if (pTurnDir > 0)
                    {
                        PolyCut.ClipByLine(SecR, Cutter, out pTmpPoly, out PolyCut.MissingPolygon, out PolyCut.MissingPolygon);
                        if (!pTmpPoly.IsEmpty)
                            SecR = Functions.RemoveFars(pTmpPoly, ptFar);
                    }
                    else
                    {
                        PolyCut.ClipByLine(SecL, Cutter, out PolyCut.MissingPolygon, out pTmpPoly, out PolyCut.MissingPolygon);
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

                    Cutter.ToPoint = Functions.PointAlongPlane(FlyBy, TmpDir, 2.0 * GlobalVars.RModel);

                    if (pTurnDir > 0)
                    {
                        PolyCut.ClipByLine(SecL, Cutter, out PolyCut.MissingPolygon, out pTmpPoly, out PolyCut.MissingPolygon);
                        if (!pTmpPoly.IsEmpty)
                            SecL = Functions.RemoveFars(pTmpPoly, ptFar);
                    }
                    else
                    {
                        PolyCut.ClipByLine(SecR, Cutter, out pTmpPoly, out PolyCut.MissingPolygon, out PolyCut.MissingPolygon);
                        if (!pTmpPoly.IsEmpty)
                            SecR = Functions.RemoveFars(pTmpPoly, ptFar);
                    }
                }
            }

            if (CheckBox802.Checked)
            {
                ptCut = new ESRI.ArcGIS.Geometry.Point();
                Construct = ((ESRI.ArcGIS.Geometry.IConstructPoint)(ptCut));

                TmpDir = OutAzt - PANS_OPS_DataBase.dpSecAreaCutAngl.Value * TurnDir;
                Construct.ConstructAngleIntersection(TurnDirector.pPtPrj, GlobalVars.DegToRadValue * OutAzt, NJoinPt, GlobalVars.DegToRadValue * TmpDir);

                Cutter.FromPoint = ptCut;
                Cutter.ToPoint = Functions.PointAlongPlane(ptCut, TmpDir + 180.0, 2.0 * GlobalVars.RModel);

                if (pTurnDir > 0)
                {
                    PolyCut.ClipByLine(SecL, Cutter, out PolyCut.MissingPolygon, out pTmpPoly, out PolyCut.MissingPolygon);
                    if (!pTmpPoly.IsEmpty)
                        SecL = Functions.RemoveFars(pTmpPoly, ptFar);
                }
                else
                {
                    PolyCut.ClipByLine(SecR, Cutter, out pTmpPoly, out PolyCut.MissingPolygon, out PolyCut.MissingPolygon);
                    if (!pTmpPoly.IsEmpty)
                        SecR = Functions.RemoveFars(pTmpPoly, ptFar);
                }
            }

            if (CheckBox805.Checked)
            {
                ptCut = new ESRI.ArcGIS.Geometry.Point();
                Construct = (ESRI.ArcGIS.Geometry.IConstructPoint)ptCut;

                TmpDir = OutAzt + PANS_OPS_DataBase.dpSecAreaCutAngl.Value * TurnDir;
                Construct.ConstructAngleIntersection(TurnDirector.pPtPrj, GlobalVars.DegToRadValue * OutAzt, FJoinPt, GlobalVars.DegToRadValue * TmpDir);

                Cutter.FromPoint = ptCut;
                Cutter.ToPoint = Functions.PointAlongPlane(ptCut, TmpDir + 180.0, 2.0 * GlobalVars.RModel);

                if (pTurnDir > 0)
                {
                    PolyCut.ClipByLine(SecR, Cutter, out pTmpPoly, out PolyCut.MissingPolygon, out PolyCut.MissingPolygon);
                    if (!pTmpPoly.IsEmpty)
                        SecR = Functions.RemoveFars(pTmpPoly, ptFar);
                }
                else
                {
                    PolyCut.ClipByLine(SecL, Cutter, out PolyCut.MissingPolygon, out pTmpPoly, out PolyCut.MissingPolygon);
                    if (!pTmpPoly.IsEmpty)
                        SecL = Functions.RemoveFars(pTmpPoly, ptFar);
                }
            }

            ptCut = TurnDirector.pPtPrj;

            if (CheckBox803.Checked)
            {
                TmpDir = Functions.ReturnAngleInDegrees(ptCut, NJoinPt);

                Cutter.FromPoint = ptCut;
                Cutter.ToPoint = Functions.PointAlongPlane(ptCut, TmpDir, 2.0 * GlobalVars.RModel);

                if (pTurnDir > 0)
                {
                    PolyCut.ClipByLine(SecL, Cutter, out PolyCut.MissingPolygon, out pTmpPoly, out PolyCut.MissingPolygon);
                    if (!pTmpPoly.IsEmpty)
                        SecL = Functions.RemoveFars(pTmpPoly, ptFar);
                }
                else
                {
                    PolyCut.ClipByLine(SecR, Cutter, out pTmpPoly, out PolyCut.MissingPolygon, out PolyCut.MissingPolygon);
                    if (!pTmpPoly.IsEmpty)
                        SecR = Functions.RemoveFars(pTmpPoly, ptFar);
                }
            }

            if (CheckBox806.Checked)
            {
                TmpDir = Functions.ReturnAngleInDegrees(ptCut, FJoinPt);

                Cutter.FromPoint = ptCut;
                Cutter.ToPoint = Functions.PointAlongPlane(ptCut, TmpDir, 2.0 * GlobalVars.RModel);

                if (pTurnDir > 0)
                {
                    PolyCut.ClipByLine(SecR, Cutter, out pTmpPoly, out PolyCut.MissingPolygon, out PolyCut.MissingPolygon);
                    if (!pTmpPoly.IsEmpty)
                        SecR = Functions.RemoveFars(pTmpPoly, ptFar);
                }
                else
                {
                    PolyCut.ClipByLine(SecL, Cutter, out PolyCut.MissingPolygon, out pTmpPoly, out PolyCut.MissingPolygon);
                    if (!pTmpPoly.IsEmpty)
                        SecL = Functions.RemoveFars(pTmpPoly, ptFar);
                }
            }

            if (!(OptionButton301.Checked))
            {
                Topo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)SecR;
                SecR = Topo.Difference(pFIXPoly) as IPointCollection;

                Topo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)SecL;
                SecL = Topo.Difference(pFIXPoly) as IPointCollection;
            }
            // ============================================
            Clone = (ESRI.ArcGIS.esriSystem.IClone)TurnArea;
            TmpPoly = (ESRI.ArcGIS.Geometry.IPointCollection)Clone.Clone();
            Topo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)TmpPoly;
            Topo.IsKnownSimple_2 = false;
            Topo.Simplify();

            Topo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)SecL;
            SecL = Topo.Difference(TmpPoly) as IPointCollection;

            Topo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)SecR;
            SecR = Topo.Difference(TmpPoly) as IPointCollection;
            // ============================================

            SecL = Functions.RemoveFars(SecL, ptFar);
            SecR = Functions.RemoveFars(SecR, ptFar);

            SecL = Functions.RemoveAgnails(SecL);
            SecR = Functions.RemoveAgnails(SecR);

            Functions.DeleteGraphicsElement(GlobalVars.PrimElem);
            GlobalVars.PrimElem = null;

            Functions.DeleteGraphicsElement(GlobalVars.SecRElem);
            Functions.DeleteGraphicsElement(GlobalVars.SecLElem);

            GlobalVars.SecLElem = Functions.DrawPolygon(SecL, Functions.RGB(0, 0, 255));
            GlobalVars.SecLElem.Locked = true;

            GlobalVars.SecRElem = Functions.DrawPolygon(SecR, GlobalVars.SecRElemColor);
            GlobalVars.SecRElem.Locked = true;

            Topo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(SecR));
            Topo.IsKnownSimple_2 = false;
            Topo.Simplify();

            Topo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(SecL));
            Topo.IsKnownSimple_2 = false;
            Topo.Simplify();

            Topo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(SecR));
            SecPoly = (IPolygon)Topo.Union(SecL);
        }

        private void AdjustTrack_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            double fTmp = 0;
            int I = 0;
            ObstacleData DetObs;

            if (!AdjustTrack.Checked)
            {
                Label101.Visible = true;

                TrackAdjust = minAdjustAngle;
                DepDir = RWYDir + TrackAdjust;
                iDominicObst = AdjustI_Zone(MinPDG, TrackAdjust, false);

                if (iDominicObst > -1)
                {
                    DetObs = oZNRList.Parts[iDominicObst];
                    fTmp = System.Math.Round(DetObs.PDG + 0.0004999, 3);
                }
                else
                    fTmp = PANS_OPS_DataBase.dpPDG_Nom.Value;

                CurrPDG = Functions.CalcLocalPDG(oZNRList.Parts, out I);
                if (I > -1)
                    CurrDetObs = oZNRList.Parts[I];
                else
                    CurrDetObs.Owner = -1;// Resources.str39014;

                SpinButton101.Value = System.Math.Abs(TrackAdjust);

                ComboBox101.SelectedIndex = (1 - AdjustDir) / 2;
                TextBox102.Text = System.Math.Round(CurrPDG * 100.0 + 0.049999, 1).ToString();
                TextBox103.Text = System.Math.Round(fTmp * 100.0, 1).ToString();

                drPDGMax = Functions.dPDGMax(oZNRList.Parts, CurrPDG, out idPDGMax);
                TextBox104.Text = Functions.ConvertHeight(drPDGMax * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value, eRoundMode.CEIL).ToString();

                //iDominicObst = AdjustI_Zone(MinPDG, TrackAdjust, True)
                //Functions.CalcObstaclesReqTNAH(oZNRList, CurrPDG);

                ReportsFrm.FillPage1(oZNRList, CurrPDG, CurrDetObs, ComboBox001.Text, 2);
                Report = true;
            }
            else
                Label101.Visible = false;

            SpinButton101.Enabled = AdjustTrack.Checked;
            ComboBox101.Enabled = AdjustTrack.Checked;
        }

        private void CancelBtn_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            this.Close();
        }

        private void ReportBtn_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!Report)
                return;

            if (ReportBtn.Checked)
                ReportsFrm.Show(GlobalVars.Win32Window);
            else
                ReportsFrm.Hide();
        }

        private void CheckBox201_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            Label211.Visible = !(CheckBox201.Checked);
            OkBtn.Enabled = !(CheckBox201.Checked);
            NextBtn.Enabled = CheckBox201.Checked;

            // MultiPage1.TabPages.Item(3).Visible = CheckBox201.Checked
            // MultiPage1.TabPages.Item(4).Visible = CheckBox201.Checked
            // MultiPage1.TabPages.Item(5).Visible = CheckBox201.Checked
            // MultiPage1.TabPages.Item(6).Visible = CheckBox201.Checked
            // MultiPage1.TabPages.Item(7).Visible = CheckBox201.Checked
            // MultiPage1.TabPages.Item(8).Visible = CheckBox201.Checked
            // MultiPage1.TabPages.Item(9).Visible = CheckBox201.Checked

            // 	FocusStepCaption((MultiPage1.SelectedIndex))

            //     If CheckBox201 = 1 Then
            //         TrackAdjust = minAdjustAngle
            //         DepDir = RWYDir + TrackAdjust
            //         iDominicObst = AdjustI_Zone(MinPDG, TrackAdjust, True)

            //         CurrPDG = CalcLocalPDG(PtInList, I)
            //         If I > -1 Then
            //             CurrDetObs = PtInList(I)
            //         Else
            //             CurrDetObs.ID = LoadResString(39014)
            //         End If

            //         TextBox101 = System.Math.Abs(SpinButton1.Value)
            //         SpinButton1.Value = System.Math.Abs(TrackAdjust)
            // 
            //         ComboBox101.ListIndex = 0.5 * (1 - AdjustDir)
            //         TextBox102 = CurrPDG * 100.0

            //     If iDominicObst > -1 Then
            //         fTmp = -Int(-PtInList(iDominicObst).PDG * 1000) * 0.001
            //     Else
            //         fTmp = dpPDG_Nom.Value
            //     End If

            //         TextBox103 = fTmp * 100.0
            // 
            //         drPDGMax = dPDGMax(PtInList, CurrPDG, idPDGMax)
            //         TextBox104 = -Int(-(drPDGMax * CurrPDG + dpH_abv_DER.Value))

            // Dim nstrs() As Variant
            // nstrs = Array("??????", "???????")

            //         TextBox202 = System.Math.Abs(TrackAdjust)
            //         Label205.Caption = "? " + nstrs(ComboBox101.ListIndex)

            //         TextBox203 = CurrPDG * 100.0
            //         TextBox206 = CurrDetObs.ID
            //         TextBox201 = NativeMethods.Modulus (System.Math.Round(DER.pPtGeo(ptEnd).M - TrackAdjust), 360.0)
            //         TextBox204 = TextBox104
            //     End If
        }

        private void CheckBox401_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            CheckBox401.Enabled = !(CheckBox401.Checked);
            Label412.Visible = CheckBox401.Enabled;

            // If CheckBox401 = 0 Then
            //     TrackAdjust = minAdjustAngle
            //     DepDir = RWYDir + TrackAdjust
            //     iDominicObst = AdjustI_Zone(MinPDG, TrackAdjust, True)

            //     CurrPDG = CalcLocalPDG(PtInList, I)
            //     CurrDetObs = PtInList(I)

            //     TextBox101 = System.Math.Abs(TrackAdjust)
            //     SpinButton1.Value = System.Math.Abs(TrackAdjust)
            //     ComboBox101.ListIndex = 0.5 * (1 - AdjustDir)
            //     TextBox102 = CurrPDG * 100.0

            //     If iDominicObst > -1 Then
            //         fTmp = -Int(-PtInList(iDominicObst).PDG * 1000) * 0.001
            //     Else
            //         fTmp = dpPDG_Nom.Value
            //     End If

            //     TextBox103 = fTmp * 100.0

            //     drPDGMax = dPDGMax(PtInList, CurrPDG, idPDGMax)
            //     TextBox104 = -Int(-(drPDGMax * CurrPDG + dpH_abv_DER.Value))

            //     Report = True
            //     ReportsFrm.FillPage1 PtInList, CurrPDG, iDominicObst, ComboBox001.text, 2
            //     ReportsFrm.FillAdjustAngles DER.pPtGeo(ptEnd).m-MagVar, -TrackAdjust * AdjustDir
            // End If

            SpinButton401.Enabled = CheckBox401.Checked;
            ComboBox401.Enabled = CheckBox401.Checked;
        }

        private void CheckBox805_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            SecondArea(TurnDir, BaseArea);
        }

        private void CheckBox806_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            SecondArea(TurnDir, BaseArea);
        }

        private void CheckBox601_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            ComboBox601_SelectedIndexChanged(ComboBox601, new System.EventArgs());
        }

        private void CheckBox801_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            CheckBox804.Checked = CheckBox801.Checked;
            SecondArea(TurnDir, BaseArea);
        }

        private void CheckBox802_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            SecondArea(TurnDir, BaseArea);
        }

        private void CheckBox803_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            SecondArea(TurnDir, BaseArea);
        }

        private void CheckBox804_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            CheckBox801.Checked = CheckBox804.Checked;
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
            int K = ComboBox002.SelectedIndex;
            if (K < 0)
                K = 0;

            TextBox001.Text = Functions.Degree2String(DER.pPtGeo[eRWY.PtDER].M, (Degree2StringMode)K);
            double fTmp = NativeMethods.Modulus(DER.pPtGeo[eRWY.PtDER].M - GlobalVars.CurrADHP.MagVar);
            TextBox004.Text = Functions.Degree2String(fTmp, (Degree2StringMode)K);
        }

        private void ComboBox003_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            int k = ComboBox003.SelectedIndex;
            if (k < 0)
                return;
            MOCLimit = GlobalVars.EnrouteMOCValues[k];// Functions.DeConvertHeight(double.Parse(ComboBox003.Text));

            RMin = 50.0 * System.Math.Round(0.02 * 600.0 / PANS_OPS_DataBase.dpPDG_Nom.Value + 0.4999);

            double minR = 50.0 * System.Math.Round(0.02 * MOCLimit / PANS_OPS_DataBase.dpMOC.Value + 0.4999);
            if (RMin < minR)
                RMin = minR;

            double OldR;
            if (double.TryParse(TextBox003.Text, out OldR))
                OldR = Functions.DeConvertDistance(OldR);

            if (OldR < RMin)
            {
                TextBox003.Tag = "";
                TextBox003.Text = Functions.ConvertDistance(RMin, eRoundMode.CEIL).ToString();
                TextBox003_Validating(TextBox003, new System.ComponentModel.CancelEventArgs());
            }
        }

        private void ComboBox101_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (ComboBox101.Enabled)
                SpinButton101_ValueChanged(SpinButton101, new System.EventArgs());
        }

        private void ComboBox201_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            AirCat = ComboBox201.SelectedIndex;
            Label309.Text = Functions.ConvertSpeed(1.1 * Categories_DATABase.cVmaInter.Values[AirCat], eRoundMode.NEAREST).ToString();
            Label310.Text = Functions.ConvertSpeed(1.1 * Categories_DATABase.cVmaFaf.Values[AirCat], eRoundMode.NEAREST).ToString();
            TextBox301.Text = Functions.ConvertSpeed(1.1 * Categories_DATABase.cVmaFaf.Values[AirCat], eRoundMode.NEAREST).ToString();
            TextBox301_Changed();
        }

        private void ComboBox401_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            SpinButton401_ValueChanged(SpinButton401, new System.EventArgs());
        }

        private void ComboBox302_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            TurnDir = 1 - 2 * ComboBox302.SelectedIndex;
        }

        private void ComboBox601_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (ComboBox601.Items.Count < 1)
                return;

            if (MultiPage1.SelectedIndex < 4)
                return;

            if (OptionButton603.Checked)
                TurnDirector = FixWPT[ComboBox601.SelectedIndex];
            else if (OptionButton602.Checked)
                TurnDirector = FixAngl[ComboBox601.SelectedIndex];
            else
                return;

            Label602.Text = Navaids_DataBase.GetNavTypeName(TurnDirector.TypeCode);

            if (OptionButton603.Checked && (TurnDirector.TypeCode == eNavaidType.VOR || TurnDirector.TypeCode == eNavaidType.NDB))
                CheckBox601.Enabled = true;
            else
            {
                CheckBox601.Checked = false;
                CheckBox601.Enabled = false;
            }

            if (TurnDirector.TypeCode == eNavaidType.VOR)
                ComboBox603.Items[0] = Resources.str36302; // "?? ???????? ??????"
            else
                ComboBox603.Items[0] = Resources.str36303; // "?? ???????? ??????"

            if (OptionButton602.Checked)
            {
                UpdateIntervals(true);

                if (ComboBox603.SelectedIndex == 0)
                    ComboBox603_SelectedIndexChanged(ComboBox603, new System.EventArgs());
                else
                    ComboBox603.SelectedIndex = 0;
            }
            else if (OptionButton603.Checked)
                UpdateToFix();
        }

        private void ComboBox501_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            int k = ComboBox501.SelectedIndex;
            string tipStr;

            if (OptionButton301.Checked || k < 0)
                return;

            Label503.Text = Navaids_DataBase.GetNavTypeName(TurnInterDat[k].TypeCode);

            if (TurnInterDat[k].TypeCode == eNavaidType.DME)
            {
                int n = TurnInterDat[k].ValMin.GetUpperBound(0);

                Option501.Visible = Option502.Visible = true;
                Option501.Enabled = Option502.Enabled = n > 0;

                Label526.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;

                if (Option501.Checked || (n == 0))
                    TextBox501.Text = Functions.ConvertDistance(TurnInterDat[k].ValMin[0], eRoundMode.NEAREST).ToString();
                else
                    TextBox501.Text = Functions.ConvertDistance(TurnInterDat[k].ValMin[1], eRoundMode.NEAREST).ToString();

                if (n == 0)
                {
                    if (TurnInterDat[k].ValCnt > 0)
                        Option501.Checked = true;
                    else
                        Option502.Checked = true;
                }


                Label502.Text = Resources.str15096; // "??????????:"
                tipStr = Resources.str15097 + "\r\n"; // "?????????? ???????? ??????????:"

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

                Option501.Visible = Option502.Visible = false;
                //Option501.Enabled = Option502.Enabled = false;

                Label526.Text = "∞";

                if (TurnInterDat[k].TypeCode == eNavaidType.VOR)
                {
                    Label502.Text = Resources.str15101; // "??????:"
                    Kmax = NativeMethods.Modulus(TurnInterDat[k].ValMax[0] - GlobalVars.CurrADHP.MagVar);
                    Kmin = NativeMethods.Modulus(TurnInterDat[k].ValMin[0] - GlobalVars.CurrADHP.MagVar);
                    tipStr = Resources.str15102; // "?????????? ???????? ????????: ?? "
                }
                else
                {
                    Label502.Text = Resources.str15104;
                    Kmax = NativeMethods.Modulus(TurnInterDat[k].ValMax[0] + 180.0 - GlobalVars.CurrADHP.MagVar);
                    Kmin = NativeMethods.Modulus(TurnInterDat[k].ValMin[0] + 180.0 - GlobalVars.CurrADHP.MagVar);
                    tipStr = Resources.str15100; // "?????????? ???????? ????????: ?? "
                }

                if (TurnInterDat[k].ValCnt > 0)
                    TextBox501.Text = Math.Round(Kmin, 1).ToString();
                else
                    TextBox501.Text = Math.Round(Kmax, 1).ToString();

                tipStr = tipStr + Functions.DegToStr(Kmin) + Resources.str15103 + Functions.DegToStr(Kmax); // " ? ?? "
            }

            Label508.Text = tipStr;
            tipStr = tipStr.Replace("\r\n", ";  ");
            ToolTip1.SetToolTip(TextBox501, tipStr);

            TextBox501_Validating(TextBox501, new System.ComponentModel.CancelEventArgs());
        }

        private void ComboBox602_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            int k = ComboBox602.SelectedIndex;
            if (k < 0 || k == System.Convert.ToInt32(ComboBox602.Tag))
                return;

            WPt602 = FixBox602[k];

            DirCourse = Functions.ReturnAngleInDegrees(TurnDirector.pPtPrj, WPt602.pPtPrj);
            bool BoolRes = UpdateToNavCurs(DirCourse) == -1;

            if (!BoolRes)
            {
                TextBox602.Tag = System.Math.Round(NativeMethods.Modulus(Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], DirCourse) - GlobalVars.CurrADHP.MagVar)).ToString();
                TextBox602.Text = TextBox602.Tag.ToString();
            }

            if ((!BoolRes) || (System.Convert.ToInt32(ComboBox602.Tag) == -1))
                ComboBox602.Tag = ComboBox602.SelectedIndex;
            else
                ComboBox602.SelectedIndex = System.Convert.ToInt32(ComboBox602.Tag);
        }

        private void ComboBox603_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (OptionButton602.Checked)
            {
                if (ComboBox603.SelectedIndex == 0)
                {
                    TextBox602.ReadOnly = false;

                    ComboBox602.Enabled = false;
                    TextBox602.Tag = "a";
                    TextBox602_Validating(TextBox602, new System.ComponentModel.CancelEventArgs());
                }
                else
                {
                    int n = GlobalVars.WPTList.Length;
                    if (n > 0)
                    {
                        TextBox602.ReadOnly = true;
                        ComboBox602.Enabled = true;

                        ComboBox602.Tag = -1;
                        ComboBox602.Items.Clear();

                        FixBox602 = new WPT_FIXType[n];
                        int j = -1;
                        for (int i = 0; i < n; i++)
                        {
                            if (GlobalVars.WPTList[i].Name != ComboBox601.Text)
                            {
                                j++;
                                FixBox602[j] = GlobalVars.WPTList[i];
                                ComboBox602.Items.Add(GlobalVars.WPTList[i].Name);
                            }
                        }

                        if (j >= 0)
                        {
                            System.Array.Resize<WPT_FIXType>(ref FixBox602, j + 1);
                            ComboBox602.SelectedIndex = 0;
                        }
                        else
                        {
                            ComboBox603.Enabled = false;
                            ComboBox603.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        ComboBox603.Enabled = false;
                        ComboBox603.SelectedIndex = 0;
                    }
                }
            }
        }

        private void InfoBtn_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            System.Drawing.Point InfoFormOrigin = NomInfoBtn.PointToScreen(new System.Drawing.Point(0, NomInfoBtn.Height));
            NomInfoFrm.ShowMessage(InfoFormOrigin);
        }

        private void HelpBtn_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            NativeMethods.HtmlHelp(0, GlobalVars.HelpFile, GlobalVars.HH_HELP_CONTEXT, HelpContextID);
        }

        // ---------------------------------------------------------------------------------------
        //  Procedure : ShowPanelBtn_CheckedChanged
        //  DateTime  : 13.06.2007 10:04
        //  Author    : RuslanA
        //  Purpose   :
        // ---------------------------------------------------------------------------------------

        private void ShowPanelBtn_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (ShowPanelBtn.Checked)
            {
                this.Width = 683;
                ShowPanelBtn.Image = Resources.bmpHIDE_INFO;
            }
            else
            {
                this.Width = 534;
                ShowPanelBtn.Image = Resources.bmpSHOW_INFO;
            }

            //if(NextBtn.Enabled)
            //	NextBtn.Focus();
            //else
            //	PrevBtn.Focus();
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

            // If MultiPage1.Tab >= 4 Then UpDown1.Value = MultiPage1.Tab
        }

        private void FillFixWPT()
        {
            double fTASl = Functions.IAS2TAS(fIAS, TurnFixPnt.Z, GlobalVars.CurrADHP.ISAtC); //Round(IAS2TAS(fIAS, TurnFixPnt.Z, CurrADHP.ISAtC));
            double TurnR = Functions.Bank2Radius(fBankAngle, fTASl);
            IPoint ptCnt = Functions.PointAlongPlane(TurnFixPnt, DepDir + 90.0 * TurnDir, TurnR);

            int j = -1, n = GlobalVars.WPTList.Length;
            System.Array.Resize<WPT_FIXType>(ref FixWPT, n);

            for (int i = 0; i < n; i++)
            {
                double fDist = Functions.ReturnDistanceInMeters(GlobalVars.WPTList[i].pPtPrj, ptCnt);
                if (fDist > TurnR && fDist < GlobalVars.RModel)
                {
                    j++;
                    FixWPT[j] = GlobalVars.WPTList[i];
                }
            }

            OptionButton603.Enabled = j >= 0;
            System.Array.Resize<WPT_FIXType>(ref FixWPT, j + 1);
        }

        private void NextBtn_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            int i, ix, j, k, n, m;
            double NewTA_PDG, DepDist, fReqH, hTurn, fTmp, PDG, mk;

            IPointCollection pPoly;
            Interval mIntr;
            NativeMethods.ShowPandaBox(this.Handle.ToInt32());

            bool pageChanged = false;

            switch (MultiPage1.SelectedIndex)
            {
                case 0:
                    #region Page - 0

                    if (!double.TryParse(TextBox003.Text, out fTmp))
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15105, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextBox003.Focus();
                        return;
                    }

                    if (Functions.DeConvertDistance(fTmp) + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Rounding / GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Multiplier < RMin)
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15105, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextBox003.Focus();
                        return;
                    }

                    MOCLimit = Functions.DeConvertHeight(double.Parse(ComboBox003.Text));

                    // ====================================================================

                    n = GlobalVars.WPTList.Length;
                    j = -1;

                    if (n > 0)
                    {
                        FixAngl = new WPT_FIXType[n];

                        for (i = 0; i < n; i++)
                            if ((GlobalVars.WPTList[i].TypeCode == eNavaidType.VOR) || (GlobalVars.WPTList[i].TypeCode == eNavaidType.NDB))
                                FixAngl[++j] = GlobalVars.WPTList[i];
                    }

                    if (j >= 0)
                    {
                        System.Array.Resize<WPT_FIXType>(ref FixAngl, j + 1);
                        OptionButton602.Enabled = true;
                    }
                    else
                    {
                        FixAngl = new WPT_FIXType[0];
                        OptionButton602.Enabled = false;
                    }
                    // ====================================================================

                    AdjustTrack.Checked = false;
                    ReportsFrm.SetTabVisible(-10, false);

                    FillAllStraightList();

                    MinPDG = 9999.0;
                    int cicles = (int)PANS_OPS_DataBase.dpTr_AdjAngle.Value;
                    for (i = 0; i <= cicles; i++)
                    {
                        AdjustI_Zone(PANS_OPS_DataBase.dpPDG_Nom.Value, i, false);
                        PDG = Functions.CalcLocalPDG(oZNRList.Parts, out m);
                        if (PDG < MinPDG)
                        {
                            MinPDG = PDG;
                            minAdjustAngle = i;
                        }
                    }

                    for (i = 1; i <= cicles; i++)
                    {
                        AdjustI_Zone(PANS_OPS_DataBase.dpPDG_Nom.Value, -i, false);
                        PDG = Functions.CalcLocalPDG(oZNRList.Parts, out m);
                        if ((PDG < MinPDG) || ((PDG == MinPDG) && (i < minAdjustAngle)))
                        {
                            MinPDG = PDG;
                            minAdjustAngle = -i;
                        }
                    }

                    CurrPDG = MinPDG;
                    TrackAdjust = minAdjustAngle;
                    DepDir = RWYDir + TrackAdjust;
                    AdjustDir = System.Math.Sign(TrackAdjust);

                    TextBox102.Text = Math.Round(100.0 * CurrPDG + 0.0499999999, 1).ToString();

                    iDominicObst = AdjustI_Zone(CurrPDG, minAdjustAngle, true);

                    if (iDominicObst > -1)
                    {
                        fTmp = oZNRList.Parts[iDominicObst].PDG;            // System.Math.Round(oZNRList[iDominicObst].PDG + 0.0004999, 3);
                        TextBox205.Text = oZNRList.Obstacles[oZNRList.Parts[iDominicObst].Owner].UnicalName;
                    }
                    else
                    {
                        fTmp = PANS_OPS_DataBase.dpPDG_Nom.Value;
                        TextBox205.Text = Resources.str39014;
                    }

                    PDG = Functions.CalcLocalPDG(oZNRList.Parts, out k);

                    if (k >= 0)
                    {
                        CurrDetObs = oZNRList.Parts[k];
                        TextBox206.Text = oZNRList.Obstacles[CurrDetObs.Owner].UnicalName;
                    }
                    else
                    {
                        CurrDetObs.Owner = -1;//Resources.str39014;
                        TextBox206.Text = Resources.str39014;
                    }

                    TextBox103.Text = System.Math.Round(100.0 * fTmp + 0.0499999999, 1).ToString();

                    if (fTmp > CurrPDG)
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str00071, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }

                    drPDGMax = Functions.dPDGMax(oZNRList.Parts, CurrPDG, out idPDGMax);
                    TextBox104.Text = Functions.ConvertHeight(drPDGMax * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value, eRoundMode.CEIL).ToString();

                    iDominicObst = AdjustI_Zone(MinPDG, minAdjustAngle, false);

                    //Functions.CalcObstaclesReqTNAH(oZNRList, CurrPDG);

                    ReportsFrm.FillPage1(oZNRList, CurrPDG, CurrDetObs, ComboBox001.Text, 2);

                    Report = true;

                    ComboBox101.SelectedIndex = (1 - AdjustDir) >> 1;
                    SpinButton101.Value = System.Math.Abs(TrackAdjust);

                    ToolTip1.SetToolTip(SpinButton101, Resources.str15106 + Functions.DegToStr(TrackAdjust) + " " + ComboBox101.Text); // "??????????? ????????: "
                    ToolTip1.SetToolTip(ComboBox101, ToolTip1.GetToolTip(SpinButton101));

                    // ==============================================================
                    pPoly = new Polyline();
                    pPoly.AddPoint(DER.pPtPrj[eRWY.PtDER]);
                    pPoly.AddPoint(Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir, GlobalVars.RModel));

                    Functions.DeleteGraphicsElement(GlobalVars.StrTrackElem);
                    GlobalVars.StrTrackElem = Functions.DrawPolyline(pPoly, GlobalVars.StrTrackElemColor, 1.0, false);

                    if (GlobalVars.ButtonControl5State)
                    {
                        pGraphics.AddElement(GlobalVars.StrTrackElem, 0);
                        GlobalVars.StrTrackElem.Locked = true;
                    }

                    //Functions.RefreshCommandBar(_mTool, 2);
                    GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

                    break;
                #endregion
                case 1:
                    #region Page - 1

                    n = oZNRList.Parts.Length;

                    fStraightDepartTermAlt = MOCLimit;
                    ix = -1;

                    for (i = 0; i < n; i++)
                    {
                        fReqH = oZNRList.Parts[i].Height + MOCLimit; // * PtInList(I).fTmp

                        if (fReqH > fStraightDepartTermAlt)
                        {
                            fStraightDepartTermAlt = fReqH;
                            ix = i;
                        }
                    }

                    if (ix >= 0)
                        ToolTip1.SetToolTip(TextBox207, Resources.str03106 + oZNRList.Obstacles[oZNRList.Parts[ix].Owner].UnicalName);
                    else
                        ToolTip1.SetToolTip(TextBox207, "");

                    TextBox207.Text = Functions.ConvertHeight(fStraightDepartTermAlt + DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.NEAREST).ToString();
                    // ===========================================================================================================
                    OkBtn.Enabled = true;
                    TextBox202.Text = System.Math.Abs(TrackAdjust).ToString();
                    Label205.Text = "∞ " + ComboBox101.Text;
                    TextBox203.Text = TextBox103.Text;

                    mk = DER.pPtGeo[eRWY.PtDER].M - TrackAdjust - GlobalVars.CurrADHP.MagVar;

                    TextBox201.Text = NativeMethods.Modulus(Functions.RoundAngle(mk)).ToString();
                    TextBox204.Text = TextBox104.Text;
                    ComboBox201_SelectedIndexChanged(ComboBox201, new System.EventArgs());
                    // =================== Start Log ================================
                    //     LogStr LoadResString(15118) + MultiPage1.TabCaption(1)                   '"???????? 2 - "
                    //     LogStr LoadResString(15119) + TextBox101.Text + "? " + ComboBox101.Text  '"    ???????? ?????: "
                    //     LogStr LoadResString(15120) + TextBox102.Text + "%"                      '"    PDG ?????:     "
                    //     LogStr LoadResString(15121) + TextBox103.Text + "%"                      '"    ???????? ?????????? 0,8 % ??????: "
                    //     LogStr LoadResString(15122) + TextBox104.Text + LoadResString(15028)                    '"    ?????? ??????????? ? PDG 3,3% ??? DER:         " " m"
                    // =================== End Log ================================
                    break;

                #endregion
                case 2:
                    #region Page - 2
                    CheckBox301.Checked = true;
                    TextBox302_Validating(TextBox302, new System.ComponentModel.CancelEventArgs());

                    if (OptionButton301.Checked)
                        OptionButton301_CheckedChanged(OptionButton301, new System.EventArgs());
                    else if (OptionButton302.Checked)
                        OptionButton302_CheckedChanged(OptionButton302, new System.EventArgs());
                    else
                        OptionButton301.Checked = true;

                    // =================== Start Log ================================
                    //     LogStr LoadResString(15124) + MultiPage1.TabCaption(2)                         '"???????? 3 - "
                    //     LogStr LoadResString(15125) + TextBox201.Text + "∞"                             ' "    ??:  "
                    //     LogStr LoadResString(15126) + TextBox202.Text + Label205.Caption                '"    ???????? ?????: "
                    //     LogStr LoadResString(15127) + TextBox203.Text + "%"                             ' "    PDG ?????:      "
                    //     LogStr LoadResString(15128) + TextBox204.Text + LoadResString(15129)            '"    ?????? ??????????? ? ???. ?????????:         "" ??????"
                    //     LogStr LoadResString(15130) + TextBox205.Text                                   '"    ??????????? ????????? ????: "
                    //     LogStr LoadResString(15131) + TextBox206.Text                                   '"    ??????????????????? ???????????:            "
                    // =================== End Log ================================
                    break;

                #endregion
                case 3:
                    #region Page - 3

                    if (!double.TryParse(TextBox301.Text, out fTmp))
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15132, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextBox301.Focus();
                        return;
                    }

                    if (!double.TryParse(TextBox302.Text, out fTmp))
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15133, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextBox302.Focus();
                        return;
                    }

                    CheckBox401.Checked = false;
                    mIntr = CalcGlobalRange(PANS_OPS_DataBase.dpPDG_Nom.Value - 0.01, ref CurrPDG, ref TrackAdjust, true);
                    //Functions.CalcObstaclesReqTNAH(oZNRList, CurrPDG);

                    DepDist = mIntr.Right;

                    hMinTurn = System.Math.Round(mIntr.Left * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value - 0.4999);
                    hMaxTurn = System.Math.Round(DepDist * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value + 0.4999);

                    //     Label405.Width = 3435

                    //if (PrevDet.ID != Resources.str39014)
                    if (PrevDet.Owner != -1)
                    {
                        Label405.Text = Resources.str15134 + "\r\n" +
                            " ID:                         " + oZNRList.Obstacles[PrevDet.Owner].UnicalName + ";" + "\r\n" +
                            Resources.str15135 + System.Math.Round(100.0 * PrevDet.PDG, 1).ToString() + " %;" + "\r\n" +
                            Resources.str15136 + Functions.ConvertDistance(PrevDet.Dist, eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit + ";" + "\r\n" +
                        Resources.str15138 + Functions.DegToStr(System.Math.Round(PrevDet.CourseAdjust)); //  "??????????? ??????????????? ????????: "
                    }
                    else
                        Label405.Text = Resources.str15139 + Math.Round(100.0 * CurrPDG, 1).ToString() + Resources.str15140; // " PDG ??????? ??? ???:        "

                    Label405.Width = 229; // " ???????? ?? DER:            "

                    TextBox405.Text = Math.Round(100.0 * CurrPDG, 1).ToString(); // " ?;"

                    mk = Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], DepDir) - GlobalVars.CurrADHP.MagVar;
                    TextBox403.Text = NativeMethods.Modulus(Functions.RoundAngle(mk)).ToString();
                    //  " ??????????? ???? ?????????: "
                    if (hMaxTurn < hMinTurn)
                        hMaxTurn = hMinTurn;

                    TextBox401.Text = Functions.ConvertHeight(hMinTurn, eRoundMode.NEAREST).ToString(); //	"? ?????????? "
                    if (hMinTurn < PANS_OPS_DataBase.dpGui_Ar1.Value)
                        TextBox401.ForeColor = System.Drawing.Color.Red;        //	"% ?????????????? ?????? ????? ?? ???? ????"
                    else
                        TextBox401.ForeColor = System.Drawing.Color.Black;

                    TextBox402.Text = Functions.ConvertHeight(hMaxTurn, eRoundMode.NEAREST).ToString();
                    if (hMaxTurn < PANS_OPS_DataBase.dpGui_Ar1.Value)
                        TextBox402.ForeColor = System.Drawing.Color.Red;
                    else
                        TextBox402.ForeColor = System.Drawing.Color.Black;

                    PrevPDG = TextBox405.Text;
                    MinGPDG = CurrPDG;
                    // ==============================================================
                    pPoly = new Polyline();
                    pPoly.AddPoint(DER.pPtPrj[eRWY.PtDER]);
                    pPoly.AddPoint(Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir, GlobalVars.RModel));

                    Functions.DeleteGraphicsElement(GlobalVars.StrTrackElem);
                    GlobalVars.StrTrackElem = Functions.DrawPolyline(pPoly, GlobalVars.StrTrackElemColor, 1.0, false);

                    if (GlobalVars.ButtonControl5State)
                    {
                        pGraphics.AddElement(GlobalVars.StrTrackElem, 0);
                        GlobalVars.StrTrackElem.Locked = true;
                    }

                    //Functions.RefreshCommandBar(_mTool, 2);

                    GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                    break;

                #endregion
                case 4:
                    #region Page - 4
                    if (!double.TryParse(TextBox405.Text, out fTmp))
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15152, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextBox405.Focus();
                        return;
                    }

                    CheckState = true;
                    if (PrevPDG != TextBox405.Text)
                    {
                        NewTA_PDG = 0.001 * System.Math.Round(double.Parse(TextBox405.Text) * 10.0 + 0.4999);
                        // =================================================
                        if (CheckBox401.Checked)
                            mIntr = CalcLocalRange(NewTA_PDG, out CurrPDG, TrackAdjust, true);
                        else
                            mIntr = CalcGlobalRange(NewTA_PDG, ref CurrPDG, ref TrackAdjust, true);

                        DepDist = mIntr.Right;

                        hMinTurn = System.Math.Round(mIntr.Left * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value - 0.4999);
                        hMaxTurn = System.Math.Round(DepDist * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value + 0.4999);

                        Label405.Width = 229;
                        //if (PrevDet.ID != Resources.str39014)
                        if (PrevDet.Owner != -1)
                        {
                            Label405.Text = Resources.str15154 + "\r\n" +
                                " ID:                         " + oZNRList.Obstacles[PrevDet.Owner].UnicalName + ";" + "\r\n" +
                                Resources.str15155 + (System.Math.Round(PrevDet.PDG + 0.0004999, 3) * 100.0).ToString() + "%;" + "\r\n" +
                                Resources.str15156 + Functions.ConvertDistance(PrevDet.Dist, eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit + ";" + "\r\n" +
                            Resources.str15157 + Functions.DegToStr(System.Math.Round(PrevDet.CourseAdjust)); // "??????????? ??????????????? ????????: "" PDG ??????? ??? ???:        "" ???????? ?? DER:            "" ?;"" ??????????? ???? ?????????: "
                        }
                        else
                            Label405.Text = Resources.str15139 + Math.Round(100.0 * CurrPDG, 1).ToString() + Resources.str15140; // "? ?????????? ""% ?????????????? ?????? ????? ?? ???? ????"

                        Label405.Width = 229;

                        TextBox405.Text = Math.Round(100.0 * CurrPDG, 1).ToString();

                        mk = Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], DepDir) - GlobalVars.CurrADHP.MagVar;
                        TextBox403.Text = NativeMethods.Modulus(Functions.RoundAngle(mk)).ToString();

                        if (hMaxTurn < hMinTurn)
                            hMaxTurn = hMinTurn;

                        TextBox401.Text = Functions.ConvertHeight(hMinTurn, eRoundMode.NEAREST).ToString();

                        if (hMinTurn < PANS_OPS_DataBase.dpGui_Ar1.Value)
                            TextBox401.ForeColor = System.Drawing.Color.FromArgb(0XFF0000);
                        else
                            TextBox401.ForeColor = System.Drawing.Color.FromArgb(0);

                        TextBox402.Text = Functions.ConvertHeight(hMaxTurn, eRoundMode.NEAREST).ToString();
                        if (hMaxTurn < PANS_OPS_DataBase.dpGui_Ar1.Value)
                            TextBox402.ForeColor = System.Drawing.Color.FromArgb(0XFF0000);
                        else
                            TextBox402.ForeColor = System.Drawing.Color.FromArgb(0);

                        PrevPDG = TextBox405.Text;
                        NativeMethods.HidePandaBox();
                        return;
                    }

                    MinCurrPDG = 0.01 * double.Parse(TextBox405.Text);
                    // =================================================

                    Functions.DeleteGraphicsElement(GlobalVars.FIXElem);
                    GlobalVars.FIXElem = null;

                    DBModule.FillNavaidList(out GlobalVars.NavaidList, out GlobalVars.DMEList, GlobalVars.CurrADHP, GlobalVars.MaxNAVDist);

                    // ====================================================================
                    ComboBox501.Items.Clear();

                    TextBox502.Tag = "a";
                    TextBox503.Tag = "a";
                    if (OptionButton301.Checked)
                    {
                        Label508.Text = Resources.str15161 + TextBox401.Text + " " + Label413.Text + Resources.str15162 + " " + TextBox402.Text + " " + Label413.Text;

                        ToolTip1.SetToolTip(TextBox502, Label508.Text); // "?????????? ???????? ?????: ?? "" ? ?? "" ?"

                        TextBox502.Text = TextBox401.Text;
                        TextBox503.Text = Functions.ConvertHeight(Functions.DeConvertHeight(double.Parse(TextBox401.Text)) + DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.NEAREST).ToString();
                    }
                    else // if (OptionButton302.Checked)
                    {
                        TurnInterDat = Functions.GetValidNavs(pPolygon as IPolygon, DER, DepDir, hMinTurn, hMaxTurn, CurrPDG);
                        //System.Diagnostics.Debug.WriteLine("TurnNavDat.count = " + TurnNavDat.Length);
                        n = TurnInterDat.Length - 1;
                        if (n >= 0)
                        {
                            for (i = 0; i <= n; i++)
                                ComboBox501.Items.Add(TurnInterDat[i].CallSign);

                            ComboBox501.SelectedIndex = 0;
                            //ComboBox501_SelectedIndexChanged(ComboBox501, new System.EventArgs());
                        }
                        else
                        {
                            NativeMethods.HidePandaBox();
                            MessageBox.Show(Resources.str15163, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    TextBox502_Validating(TextBox502, new System.ComponentModel.CancelEventArgs());

                    // =================== Start Log ================================
                    //     LogStr LoadResString(15164) + MultiPage1.TabCaption(4)                     '"???????? 5 - "
                    //     LogStr LoadResString(15165)                                                '"    H ????????? ??? DER, min(m) "
                    //     LogStr LoadResString(15167) + TextBox402.Text                              '"    H ????????? ??? DER, max(m) "
                    //     LogStr LoadResString(15168) + TextBox403.Text + "∞"                        '"    ?? ??????: "
                    //     LogStr "    " + Label405.Caption
                    //     LogStr LoadResString(15169)                                                '"    ?????????? ??????????? ???? 1 :"
                    //     LogStr LoadResString(15170) + TextBox404.Text + "? " + ComboBox401.Text    '"    ???????? ????? ?????? ?? ????? ???: "
                    //     LogStr LoadResString(15171) + TextBox405.Text + "%"                        '"    PDG ? ???? 1, ?????????? ????? ??:  "
                    // 
                    //     If Label412.Visible Then
                    //         LogStr LoadResString(15172)                                            '"    ??????????? ??????? ?????(??????????? PDG ? ???????? ????? ??? ???? 1 ?????? ? ??????????)"
                    //     End If
                    // =================== End Log ================================
                    break;
                #endregion
                case 5:
                    #region Page - 5
                    if (OptionButton301.Checked && !double.TryParse(TextBox502.Text, out fTmp))
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15173, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextBox502.Focus();
                        return;
                    }

                    if ((!OptionButton301.Checked) && !double.TryParse(TextBox501.Text, out fTmp))
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15174 + Label502.Text + Resources.str15175, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextBox501.Focus();
                        return;
                    }
                    // =============================================
                    FillFixWPT();

                    fTmp = PANS_OPS_DataBase.dpT_Gui_dist.Value / (Functions.IAS2TAS(fIAS, TurnFixPnt.Z, GlobalVars.CurrADHP.ISAtC) + GlobalVars.CurrADHP.WindSpeed) * 3.6;
                    ToolTip1.SetToolTip(TextBox601, Resources.str15176 + System.Math.Round(fTmp).ToString() + " " + Resources.str00057); // "???????????? ????????: "" ???"

                    TextBox601.Text = "0";
                    TextBox601.Tag = "0";

                    TextBox603.Text = "30";
                    TextBox603.Tag = "30";

                    CheckState = true;

                    //Functions.DeleteGraphicsElement(GlobalVars.StraightAreaElem);
                    // =============================================

                    //OptionButton706.Visible = false;
                    //OptionButton702.Visible = false;
                    //OptionButton705.Visible = false;
                    //OptionButton702.Checked = false;
                    //OptionButton705.Checked = false;
                    //CheckBox806.Visible = false;

                    if (OptionButton601.Checked)
                        OptionButton601_CheckedChanged(OptionButton601, new EventArgs());
                    else if (OptionButton602.Checked)
                        OptionButton602_CheckedChanged(OptionButton602, new EventArgs());
                    else if (OptionButton603.Checked)
                        OptionButton603_CheckedChanged(OptionButton603, new EventArgs());
                    else
                        OptionButton601.Checked = true;

                    //GlobalVars.ButtonControl2State = false;
                    //Functions.RefreshCommandBar(_mTool, 2);
                    break;
                #endregion
                case 6:
                    #region Page - 6
                    if (!((OptionButton603.Checked || OptionButton602.Checked || OptionButton601.Checked)))
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15184, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //         OptionButton601.SetFocus
                        return;
                    }

                    if ((OptionButton602.Checked || OptionButton601.Checked) && !double.TryParse(TextBox602.Text, out fTmp))
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15185, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TextBox602.Focus();
                        return;
                    }

                    if (OptionButton602.Checked)
                    {
                        if (!double.TryParse(TextBox603.Text, out fTmp))
                        {
                            NativeMethods.HidePandaBox();
                            MessageBox.Show(Resources.str15186, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            TextBox603.Focus();
                            return;
                        }

                        if (!double.TryParse(TextBox601.Text, out fTmp))
                        {
                            NativeMethods.HidePandaBox();
                            MessageBox.Show(Resources.str15187, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            TextBox601.Focus();
                            return;
                        }
                    }

                    CheckState = false;
                    if (OptionButton601.Checked || (TurnDirector.TypeCode <= eNavaidType.NONE))
                    {

                        screenCapture.Save(this);
                        pageChanged = true;
                        CurrPage = MultiPage1.SelectedIndex + 2;
                        MultiPage1.SelectedIndex = CurrPage;

                        SecPoly = (IPolygon)new ESRI.ArcGIS.Geometry.Polygon();

                        hTurn = CalcTAParams();
                        //TerminationParams(hTurn);
                        Frame903.Visible = false;
                    }
                    break;
                #endregion
                case 7:
                    #region Page - 7
                    CheckBox803.Enabled = OptionButton703.Checked && OptionButton703.Enabled;
                    if (!CheckBox803.Enabled)
                        CheckBox803.Checked = false;

                    CheckBox806.Enabled = OptionButton706.Checked && OptionButton706.Enabled;
                    if (!CheckBox806.Enabled)
                        CheckBox806.Checked = false;

                    SecondArea(TurnDir, BaseArea);
                    // =================== Start Log ================================
                    //     LogStr LoadResString(15198) + MultiPage1.TabCaption(7)                      '"???????? 8 - "
                    // 
                    //     If Frame701.Enabled Then
                    //         LogStr LoadResString(15199)                                             '"    ?? ?????????? ??????? ?????????:"
                    //         If OptionButton701 Then
                    //             LogStr LoadResString(15200)                                         '"    ?????????? ????????? ?????"
                    //         End If
                    //         If OptionButton702 Then
                    //             LogStr LoadResString(15201)                                         '"    ????????? ???? ?? 15? ?? ????????? ?????"
                    //         End If
                    // 
                    //         If OptionButton703 Then
                    //             LogStr LoadResString(15202)                                         '"    ????????? ???? ????????"
                    //         End If
                    //     Else
                    //         LogStr LoadResString(15203)                                             '"    ?? ?????????? ??????? ????????? ???????? ????????? ?? ???????? PANS-OPS"
                    //     End If
                    // 
                    //     If Frame702.Enabled Then
                    //         LogStr LoadResString(15204)                                             '"    ?? ??????? ??????? ?????????:"
                    //         If OptionButton704 Then
                    //             LogStr LoadResString(15200)                                         '"    ?????????? ????????? ?????"
                    //         End If
                    //         If OptionButton705 Then
                    //             LogStr LoadResString(15201)                                         '"    ????????? ???? ?? 15? ?? ????????? ?????"
                    //         End If
                    // 
                    //         If OptionButton706 Then
                    //             LogStr LoadResString(15202)                                         '"    ????????? ???? ????????"
                    //         End If
                    //     Else
                    //         LogStr LoadResString(15208)                                             '"    ?? ??????? ??????? ????????? ???????? ????????? ?? ???????? PANS-OPS"
                    //     End If
                    // =================== End Log ================================
                    break;
                #endregion
                case 8:
                    #region Page - 8

                    if (!((CheckBox801.Checked || CheckBox802.Checked || CheckBox803.Checked)))
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15209, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!((CheckBox804.Checked || CheckBox805.Checked || CheckBox806.Checked)))
                    {
                        NativeMethods.HidePandaBox();
                        MessageBox.Show(Resources.str15209, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    hTurn = CalcTAParams();
                    //TerminationParams(hTurn);

                    //CalcNewStraightAreaWithFixedLength(ref NewTIA_PDG, ref TACurrPDG);

                    Frame903.Visible = OptionButton602.Checked;

                    //     OkBtn.Enabled = True
                    // =================== Start Log ================================
                    //     LogStr LoadResString(15210) + MultiPage1.TabCaption(8)             '"???????? 9 - "
                    //     LogStr LoadResString(15211)                                        '"    ????????? ?? ?????????? ???????:"
                    //     If CheckBox801.Value = 1 Then
                    //         LogStr LoadResString(15212)                                    '"    ?? ??????? ???? ??????? ?????????"
                    //     End If
                    //     If CheckBox802.Value = 1 Then
                    //         LogStr LoadResString(15213)                                    '"    ?? ??????? ? 30?"
                    //     End If
                    //     If CheckBox803.Value = 1 Then
                    //         LogStr LoadResString(15214)                                    '"    ?? ????? ???-??????? ??"
                    //     End If
                    // 
                    //     LogStr LoadResString(15215)                                        '"    ????????? ?? ??????? ???????:"
                    //     If CheckBox804.Value = 1 Then
                    //         LogStr LoadResString(15212)                                    '"    ?? ??????? ???? ??????? ?????????"
                    //     End If
                    //     If CheckBox805.Value = 1 Then
                    //         LogStr LoadResString(15213)                                    '"    ?? ??????? ? 30?"
                    //     End If
                    //     If CheckBox806.Value = 1 Then
                    //         LogStr LoadResString(15214)                                    '"    ?? ????? ???-??????? ??"
                    //     End If
                    // =================== End Log ================================
                    break;
                    #endregion
            }

            // ===================================================================
            if (!pageChanged)
                screenCapture.Save(this);

            CurrPage = MultiPage1.SelectedIndex + 1;
            MultiPage1.SelectedIndex = CurrPage;

            //  2007
            FocusStepCaption(MultiPage1.SelectedIndex);

            NextBtn.Enabled = MultiPage1.SelectedIndex < MultiPage1.TabPages.Count - 1;

            if ((CurrPage == 2) && (!CheckBox201.Checked))
                NextBtn.Enabled = false;

            if (CurrPage == MultiPage1.TabPages.Count - 1)
                OkBtn.Enabled = true;

            PrevBtn.Enabled = MultiPage1.SelectedIndex > 0;

            this.HelpContextID = 4000 + 100 * (MultiPage1.SelectedIndex + 1);
            NativeMethods.HidePandaBox();

            this.Visible = false;
            this.Show(GlobalVars.Win32Window);

            this.Activate();
        }

        private void ApplayOptions()
        {
            if (CheckState || OptionButton601.Checked)
                return;

            Functions.CreateNavaidZone(TurnDirector.pPtPrj, OutAzt, TurnDirector.TypeCode, 2.0, SecL, SecR, Prim);

            if (OptionButton603.Checked && (TurnDirector.TypeCode == eNavaidType.VOR || TurnDirector.TypeCode == eNavaidType.NDB))
            {
                IPointCollection tmpPoly1 = new ESRI.ArcGIS.Geometry.Polygon();

                tmpPoly1.AddPoint(TurnArea.Point[0]);
                tmpPoly1.AddPoint(TurnArea.Point[TurnArea.PointCount - 1]);

                IPointCollection NewPoly = (IPointCollection)ApplayJoining(TurnDirector.TypeCode, TurnDir, TurnArea, BasePoints as IPolygon, OutPt, OutAzt, tmpPoly1);
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

                IGeometryCollection pBag = (ESRI.ArcGIS.Geometry.IGeometryCollection)(new GeometryBag());
                pBag.AddGeometry((IGeometry)NewPoly);
                pTopoOper.ConstructUnion((IEnumGeometry)pBag);
                BaseArea = (IPointCollection)Functions.RemoveHoles(NewPoly);

                if (OptionButton301.Checked)
                {
                    pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)BaseArea;
                    BaseArea = (IPointCollection)pTopoOper.Difference(ZNR_Poly);
                }

                BaseArea = (IPointCollection)Functions.PolygonIntersection(pCircle, BaseArea);
                //     Set pTopoOper = pCircle
                //     Set BaseArea = pTopoOper.Intersect(BaseArea, esriGeometry2Dimension)
                // ================================================================

                Functions.DeleteGraphicsElement(GlobalVars.TurnAreaElem);
                Functions.DeleteGraphicsElement(GlobalVars.PrimElem);
                Functions.DeleteGraphicsElement(GlobalVars.SecLElem);
                Functions.DeleteGraphicsElement(GlobalVars.SecRElem);

                GlobalVars.TurnAreaElem = Functions.DrawPolygon(BaseArea, 255, esriSimpleFillStyle.esriSFSNull, false);

                tmpPoly1 = (IPointCollection)Functions.PolygonIntersection(pCircle, Prim);
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
            else if (OptionButton602.Checked)
                UpdateToNavCurs(DirCourse);
        }

        private void Option501_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (((RadioButton)eventSender).Checked)
            {
                TextBox501.Tag = "a";
                TextBox501_Validating(TextBox501, new System.ComponentModel.CancelEventArgs());
            }
        }

        private void Option502_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (((RadioButton)eventSender).Checked)
            {
                TextBox501.Tag = "a";
                TextBox501_Validating(TextBox501, new System.ComponentModel.CancelEventArgs());
            }
        }

        private void Option3_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (((RadioButton)eventSender).Checked)
                ArrayIVariant = 1;
        }

        private void Option4_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (((RadioButton)eventSender).Checked)
                ArrayIVariant = 2;
        }

        private void OptionButton301_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!((RadioButton)eventSender).Checked)
                return;

            Label504.Text = Resources.str15221;
            Label525.Text = Resources.str02621; // "???. ?????? ?????????:"
            Label506.Text = Resources.str15220; // "????????????? ???:"
            Label907.Text = Resources.str03909;

            CheckBox301.Enabled = true;
            Frame501.Visible = false;
            Frame502.Visible = true;

            TextBox502.ReadOnly = false;
            TextBox503.ReadOnly = false;

            Text508.Visible = false;
            Text509.Visible = false;
            Text510.Visible = false;

            Label521.Visible = false;
            Label522.Visible = false;
            Label523.Visible = false;
            Label535.Visible = false;
            Label536.Visible = false;
            Label537.Visible = false;
        }

        private void OptionButton302_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!((RadioButton)eventSender).Checked)
                return;

            Label504.Text = Resources.str00904; // "???. ?????? ?? KK:"
            Label525.Text = Resources.str00905; // "???. ?????? ?? KK:"                       '"???. ?????? ?????????:"
            Label506.Text = Resources.str00906; // "???????? KK ?? DER:"
            Label907.Text = Resources.str40101;

            CheckBox301.Enabled = false;
            Frame501.Visible = true;
            Frame502.Visible = false;

            TextBox502.ReadOnly = true;

            TextBox503.ReadOnly = true;

            Text508.Visible = true;
            Text509.Visible = true;
            Text510.Visible = true;

            Label521.Visible = true;
            Label522.Visible = true;
            Label523.Visible = true;
            Label535.Visible = true;
            Label536.Visible = true;
            Label537.Visible = true;
        }

        private void OptionButton901_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!((RadioButton)eventSender).Checked)
                return;

            TextBox911.ReadOnly = false;
            TextBox912.ReadOnly = true;
        }

        private void OptionButton902_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!((RadioButton)eventSender).Checked)
                return;

            TextBox911.ReadOnly = true;
            TextBox912.ReadOnly = false;
        }

        private void OptionButton903_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!((RadioButton)eventSender).Checked)
                return;

            TextBox911.ReadOnly = true;
            TextBox912.ReadOnly = true;

            int indx;
            double Old_TAPDG = CurrPDG;
            double Old_TACurrPDG = TACurrPDG;
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
                    MessageBox.Show(Resources.str15224, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            while (CurrPDG + GlobalVars.PDGEps < TACurrPDG);

            // =====================================================================================
            IPoint ptCnt;

            if (OptionButton601.Checked)
            {
                IPoint ptTmp = MPtCollection.Point[MPtCollection.PointCount - 1];
                Functions.CircleVectorIntersect(GlobalVars.CurrADHP.pPtPrj, GlobalVars.RModel, ptTmp, ptTmp.M, out ptCnt);
            }
            else if (OptionButton602.Checked)
                ptCnt = TerFixPnt;
            else
                ptCnt = TurnDirector.pPtPrj;

            IProximityOperator pProxi;
            if (OptionButton301.Checked)
                pProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)ZNR_Poly;
            else
                pProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)KK;

            double MaxDist = pProxi.ReturnDistance(ptCnt);

            // =====================================================================================

            TACurrPDG = CurrPDG;
            TextBox911.Text = Math.Round(100.0 * CurrPDG, 1).ToString();
            TextBox911.Tag = TextBox911.Text;

            TextBox912.Text = Math.Round(100.0 * TACurrPDG, 1).ToString();
            TextBox912.Tag = TextBox912.Text;

            double hTurn = fZNRLen * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value;
            hKK = hTurn + DER.pPtPrj[eRWY.PtDER].Z;
            fReachedA = hKK + MaxDist * TACurrPDG;

            TextBox914.Text = Functions.ConvertHeight(hKK, eRoundMode.NEAREST).ToString();

            double L1 = Functions.ReturnDistanceInMeters(DER.pPtPrj[eRWY.PtDER], TurnFixPnt);
            AltitudeAtTurnFixPnt = CurrPDG * L1 + PANS_OPS_DataBase.dpH_abv_DER.Value + DER.pPtPrj[eRWY.PtDER].Z;

            TextBox906.Text = Functions.ConvertHeight(fReachedA, eRoundMode.NEAREST).ToString();
            //TextBox916.Text = TextBox906.Text;
            TextBox916_Validating(TextBox916, null);
            // =====================================================================================

            IPointCollection mPoly = (IPointCollection)Functions.CalcTrajectoryFromMultiPoint(MPtCollection);
            mPoly.AddPoint(ptCnt);
            NomInfoFrm.SetInfo(mPoly, hKK, TACurrPDG);
        }

        private void OptionButton701_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (((RadioButton)eventSender).Checked)
                ApplayOptions();
        }

        private void OptionButton703_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (((RadioButton)eventSender).Checked)
                ApplayOptions();
        }

        private void OptionButton704_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (((RadioButton)eventSender).Checked)
                ApplayOptions();
        }

        private void OptionButton706_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (((RadioButton)eventSender).Checked)
                ApplayOptions();
        }

        private void OptionButton702_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (((RadioButton)eventSender).Checked)
                ApplayOptions();
        }

        private void OptionButton705_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (((RadioButton)eventSender).Checked)
                ApplayOptions();
        }

        private void OptionButton603_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!((RadioButton)eventSender).Checked)
                return;

            ComboBox601.Visible = true;
            CheckBox601.Visible = true;
            // =====
            Label601.Text = Resources.str03613;
            Label601.Visible = true;
            Label602.Visible = true;
            Label607.Visible = false;
            Label609.Visible = false;
            Label603.Visible = false;
            Label604.Visible = false;
            Label613.Visible = false;
            Label614.Visible = false;

            Label610.Visible = false;
            Label611.Visible = false;

            ComboBox602.Visible = false;
            ComboBox603.Visible = false;

            TextBox601.Visible = false;
            TextBox603.Visible = false;
            Label608.Visible = false;
            TextBox602.ReadOnly = true;

            ToolTip1.SetToolTip(TextBox602, "");
            TextBox602.Tag = "a";
            // ==
            CheckBox806.Visible = true;
            OptionButton706.Visible = true;
            OptionButton702.Visible = false;
            OptionButton705.Visible = false;
            OptionButton702.Checked = false;
            OptionButton705.Checked = false;

            Label702.Visible = true;
            TextBox702.Visible = true;
            // ==
            ComboBox601.Items.Clear();

            for (int i = 0; i < FixWPT.Length; i++)
                ComboBox601.Items.Add(FixWPT[i].ToString());

            ComboBox601.SelectedIndex = 0;
            //ComboBox601_SelectedIndexChanged(ComboBox601, New System.EventArgs())
        }

        private void OptionButton602_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!((RadioButton)eventSender).Checked)
                return;

            Label601.Text = Resources.str03614;
            Label601.Visible = true;
            Label602.Visible = true;
            Label607.Visible = true;
            //     Label605.Visible = True
            Label609.Visible = true;
            Label603.Visible = true;
            Label604.Visible = true;

            Label610.Visible = true;
            Label611.Visible = true;
            ComboBox602.Visible = true;
            ComboBox603.Visible = true;
            ComboBox603.Enabled = GlobalVars.WPTList.GetUpperBound(0) > 0;

            Label613.Visible = true;
            Label614.Visible = true;


            ComboBox601.Visible = true;

            TextBox603.Visible = true;
            TextBox602.ReadOnly = false;

            ToolTip1.SetToolTip(TextBox602, RecommendStr);
            TextBox602.Tag = "a";
            Label608.Visible = true;

            TextBox601.Visible = true;
            CheckBox601.Visible = false;
            OptionButton702.Visible = true;
            // =======
            CheckBox806.Visible = false;
            CheckBox806.Checked = false;

            OptionButton706.Visible = false;
            CheckBox806.Checked = false;
            OptionButton702.Visible = true;
            OptionButton705.Visible = true;

            Label702.Visible = false;
            TextBox702.Visible = false;
            // =======
            ComboBox601.Items.Clear();
            ComboBox602.Items.Clear();

            for (int i = 0; i < FixAngl.Length; i++)
                ComboBox601.Items.Add(FixAngl[i].Name);

            ComboBox601.SelectedIndex = 0;
            ComboBox603.SelectedIndex = 0;

            //ComboBox601_SelectedIndexChanged(ComboBox601, New System.EventArgs())
        }

        private void OptionButton601_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!((RadioButton)eventSender).Checked)
                return;

            TextBox602.ReadOnly = false;

            ToolTip1.SetToolTip(TextBox602, "");
            TextBox602.Tag = "a";

            Label601.Visible = false;
            Label602.Visible = false;
            Label607.Visible = false;

            Label609.Visible = false;
            Label603.Visible = false;
            Label604.Visible = false;
            Label611.Visible = false;
            Label613.Visible = false;
            Label614.Visible = false;

            Label610.Visible = false;
            ComboBox602.Visible = false;
            ComboBox603.Visible = false;

            ComboBox601.Visible = false;
            TextBox603.Visible = false;
            Label608.Visible = false;
            TextBox601.Visible = false;
            CheckBox601.Visible = false;
            OptionButton702.Visible = false;
            TextBox602_Validating(TextBox602, new System.ComponentModel.CancelEventArgs());
        }

        private void OptionButton904_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!((RadioButton)eventSender).Checked)
                return;

            TextBox901.Visible = true;
            TextBox902.Visible = true;
            TextBox903.Visible = true;
            TextBox904.Visible = true;

            TextBox911.Visible = false;
            TextBox912.Visible = false;
            TextBox913.Visible = false;
            TextBox914.Visible = false;
        }

        private void OptionButton905_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!((RadioButton)eventSender).Checked)
                return;

            TextBox901.Visible = false;
            TextBox902.Visible = false;
            TextBox903.Visible = false;
            TextBox904.Visible = false;

            TextBox911.Visible = true;
            TextBox912.Visible = true;
            TextBox913.Visible = true;
            TextBox914.Visible = true;
        }

        private void OptionButton906_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (!((RadioButton)eventSender).Checked)
                return;

            TextBox907.Tag = "";
            TextBox907_Validating(TextBox907, new System.ComponentModel.CancelEventArgs());
        }

        private void OptionButton907_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (((RadioButton)eventSender).Checked)
            {
                TextBox907.Tag = "";
                TextBox907_Validating(TextBox907, new System.ComponentModel.CancelEventArgs());
            }
        }

        private void PrevBtn_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            // LogStr LoadResString(15225)                                      '"<-?????? ???????? ????"
            switch (MultiPage1.SelectedIndex)
            {
                case 1:
                    Functions.DeleteGraphicsElement(GlobalVars.StraightAreaElem);
                    Functions.DeleteGraphicsElement(GlobalVars.StrTrackElem);

                    GlobalVars.StraightAreaElem = null;
                    GlobalVars.StrTrackElem = null;

                    GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

                    //Functions.RefreshCommandBar(_mTool, 2);
                    if (ReportsFrm.Visible)
                        ReportsFrm.Hide();

                    GlobalVars.RModel = 0.0;
                    TextBox003.Tag = "a";
                    TextBox003_Validating(TextBox003, new System.ComponentModel.CancelEventArgs());

                    break;
                case 2:
                    CheckBox201.Checked = false;
                    OkBtn.Enabled = false;
                    break;
                case 3:
                    CheckBox301.Checked = false;
                    break;
                case 4:
                    break;
                case 5:
                    hMinTurn = Functions.DeConvertHeight(double.Parse(TextBox401.Text));
                    hMaxTurn = Functions.DeConvertHeight(double.Parse(TextBox402.Text));

                    Functions.DeleteGraphicsElement(GlobalVars.FIXElem);
                    GlobalVars.FIXElem = null;

                    GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                    //Functions.RefreshCommandBar(_mTool, 128);
                    break;
                case 6:
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

                    //if (GlobalVars.StraightAreaElem != null)
                    //{
                    //    if (GlobalVars.StraightAreaElem is IGroupElement)
                    //    {
                    //        IGroupElement pGroupElement = ((ESRI.ArcGIS.Carto.IGroupElement)(GlobalVars.StraightAreaElem));
                    //        for (int i = 0; i < pGroupElement.ElementCount; i++)
                    //        {
                    //            pGraphics.AddElement(pGroupElement.Element[i], 0);
                    //            pGroupElement.Element[i].Locked = true;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        pGraphics.AddElement(GlobalVars.StraightAreaElem, 0);
                    //        GlobalVars.StraightAreaElem.Locked = true;
                    //    }
                    //}

                    Functions.DeleteGraphicsElement(GlobalVars.FIXElem);

                    GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                    //Functions.RefreshCommandBar(_mTool, 65535);
                    break;
                case 7:
                    CheckState = true;
                    break;
                case 9:
                    if (OptionButton601.Checked || (TurnDirector.TypeCode <= eNavaidType.NONE))
                    {
                        CurrPage = MultiPage1.SelectedIndex - 2;
                        MultiPage1.SelectedIndex = CurrPage;
                        CheckState = true;
                    }
                    else
                        CheckState = false;

                    OkBtn.Enabled = false;

                    if (OptionButton602.Checked)
                        Functions.DeleteGraphicsElement(GlobalVars.TerminationFIXElem);

                    break;
            }

            screenCapture.Delete();

            CurrPage = MultiPage1.SelectedIndex - 1;
            MultiPage1.SelectedIndex = CurrPage;

            // 2007
            FocusStepCaption((MultiPage1.SelectedIndex));

            NextBtn.Enabled = MultiPage1.SelectedIndex < MultiPage1.TabPages.Count - 1;
            PrevBtn.Enabled = MultiPage1.SelectedIndex > 0;

            this.HelpContextID = 4000 + 100 * (MultiPage1.SelectedIndex + 1);
            // Me.Activate()
        }

        private void OkBtn_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            string RepFileName, RepFileTitle;

            if (!Functions.ShowSaveDialog(out RepFileName, out RepFileTitle))
                return;

            ConvertTracToPoints();
            ReportHeader pReport;

            //string sProcName;   // Must be FIX Name
            //if (CheckBox201.Checked)
            //{
            //	if (OptionButton601.Checked)
            //		sProcName = "VM" + " RWY " + DER.Name;
            //	else
            //		sProcName = "F" + TextBox916.Text + " RWY" + DER.Name;
            //}
            //else
            //	sProcName = "VA" + " RWY" + DER.Name;           // .Identifier;
            //pReport.Procedure = sProcName;

            pReport.Procedure = RepFileTitle;

            //pReport.EffectiveDate = _Procedure.TimeSlice.ValidTime.BeginPosition;
            pReport.Database = GlobalVars.gAranEnv.ConnectionInfo.Database;
            pReport.Aerodrome = GlobalVars.CurrADHP.Name;
            //pReport.RWY = ComboBox001.Text;

            if (CheckBox201.Checked)
                pReport.Category = ComboBox201.Text;
            else
                pReport.Category = "";

            SaveAccuracy(RepFileName, RepFileTitle, pReport);
            SaveRoutsLog(RepFileName, RepFileTitle, pReport);
            SaveProtocol(RepFileName, RepFileTitle, pReport);

            if (CurrPage > 2)
                RoutsGeomRep = ReportFile.SaveGeometry(RepFileName, RepFileTitle, pReport, DER, RoutsPoints, RoutsAllLen, false);
			//SaveGeometry(RepFileName, RepFileTitle, pReport, DER, RoutsPoints, RoutsAllLen, false);

			DBModule.pObjectDir.SaveAsXml(RepFileName + "_InputData.xml");

			if (!SaveProcedure(RepFileTitle))
                return;

            screenCapture.Save(this);
            saveReportToDB();
            saveScreenshotToDB();

            this.Close();

            //screenCapture.Delete();
        }

        private void saveReportToDB()
        {
            saveReportToDB(RoutsLogRep, FeatureReportType.Log);
            saveReportToDB(RoutsProtRep, FeatureReportType.Protocol);
            saveReportToDB(RoutsGeomRep, FeatureReportType.Geometry);
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

        private void SpinButton101_ValueChanged(System.Object eventSender, System.EventArgs e)
        {
            int iDir = 1 - 2 * ComboBox101.SelectedIndex;
            int newAdjust = System.Convert.ToInt32(SpinButton101.Value * iDir);

            if (TrackAdjust == newAdjust)
                return;

            TrackAdjust = newAdjust;

            DepDir = RWYDir + TrackAdjust;
            AdjustI_Zone(PANS_OPS_DataBase.dpPDG_Nom.Value, TrackAdjust, false);

            int indx;
            CurrPDG = Functions.CalcLocalPDG(oZNRList.Parts, out indx);
            if (indx > -1)
            {
                CurrDetObs = oZNRList.Parts[indx];
                TextBox206.Text = oZNRList.Obstacles[CurrDetObs.Owner].UnicalName;
            }
            else
            {
                CurrDetObs.Owner = -1;
                TextBox206.Text = Resources.str39014;
            }

            iDominicObst = AdjustI_Zone(CurrPDG, TrackAdjust, true);
            double fTmp;
            if (iDominicObst > -1)
            {
                fTmp = System.Math.Round(oZNRList.Parts[iDominicObst].PDG + 0.0004999, 3);
                TextBox205.Text = oZNRList.Obstacles[oZNRList.Parts[iDominicObst].Owner].UnicalName;
            }
            else
            {
                fTmp = PANS_OPS_DataBase.dpPDG_Nom.Value;
                TextBox205.Text = Resources.str39014;
            }

            drPDGMax = Functions.dPDGMax(oZNRList.Parts, CurrPDG, out idPDGMax);
            TextBox104.Text = Functions.ConvertHeight(drPDGMax * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value, eRoundMode.CEIL).ToString();

            //PtLReportList[0].ID = -1;		// Resources.str39014;
            //PtLReportList[1].ID = -1;		// Resources.str39014;
            //PtLReportList[2].ID = CurrDetObs.ID;

            TextBox102.Text = Math.Round(CurrPDG * 100.0 + 0.0499999, 1).ToString();

            //if (CurrPDG < fTmp)	MessageBox.Show(Resources.str71, null, MessageBoxButtons.OK, MessageBoxIcon.Error);

            TextBox103.Text = System.Math.Round(fTmp * 100.0, 1).ToString();
            //Functions.CalcObstaclesReqTNAH(oZNRList, CurrPDG);
            ReportsFrm.FillPage1(oZNRList, CurrPDG, CurrDetObs, ComboBox001.Text, 2);
            Report = true;

            IPointCollection pPoly = new Polyline();
            pPoly.AddPoint(DER.pPtPrj[eRWY.PtDER]);
            pPoly.AddPoint(Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir, GlobalVars.RModel));

            Functions.DeleteGraphicsElement(GlobalVars.StrTrackElem);
            GlobalVars.StrTrackElem = Functions.DrawPolyline(pPoly, GlobalVars.StrTrackElemColor, 1.0, GlobalVars.ButtonControl5State);

            //Functions.RefreshCommandBar(_mTool, 2);
        }

        private void SpinButton401_ValueChanged(System.Object sender, System.EventArgs e)
        {
            int NewAdjust;
            double NewPDG;
            double DepDist;
            Interval mIntr;
            IPointCollection pPoly;

            if (!(SpinButton401.Enabled))
                return;

            AdjustDir = 1 - 2 * ComboBox401.SelectedIndex;
            NewAdjust = System.Convert.ToInt32(SpinButton401.Value * AdjustDir);

            if (NewAdjust == TrackAdjust)
                return;

            TrackAdjust = NewAdjust;
            NewPDG = PANS_OPS_DataBase.dpPDG_Nom.Value;

            // =============================================================
            mIntr = CalcLocalRange(NewPDG * 0.99, out CurrPDG, TrackAdjust, true);
            DepDist = mIntr.Right;
            hMinTurn = System.Math.Round(mIntr.Left * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value - 0.4999);
            hMaxTurn = System.Math.Round(DepDist * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value + 0.4999);

            Label405.Width = 229;
            //if (PrevDet.ID != Resources.str39014)
            if (PrevDet.Owner != -1)
            {
                Label405.Text = Resources.str15241 + "\r\n" +
                    " ID:                         " + oZNRList.Obstacles[PrevDet.Owner].UnicalName + ";" + "\r\n" +
                    Resources.str15242 + (System.Math.Round(PrevDet.PDG + 0.0004999, 3) * 100.0).ToString() + "%;" + "\r\n" +
                    Resources.str15243 + Functions.ConvertDistance(PrevDet.Dist, eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit + ";" + "\r\n" +
                Resources.str15245 + Functions.DegToStr(System.Math.Round(PrevDet.CourseAdjust)); // "??????????? ??????????????? ????????: "" PDG ??????? ??? ???:        "" ???????? ?? DER:  " ?;"          "" ??????????? ???? ?????????: "
            }
            else
                Label405.Text = Resources.str15246 + Math.Round(100.0 * CurrPDG, 1).ToString() + Resources.str15247; // "? ?????????? ""% ?????????????? ?????? ????? ?? ???? ????"

            Label405.Width = 229;

            // ================================================================
            AdjustI_Zone(CurrPDG, TrackAdjust, true);
            PrevPDG = TextBox405.Text;

            // ============================================================

            if (hMaxTurn < hMinTurn)
                hMaxTurn = hMinTurn;

            TextBox402.Text = Functions.ConvertHeight(hMaxTurn, eRoundMode.NEAREST).ToString();
            TextBox401.Text = Functions.ConvertHeight(hMinTurn, eRoundMode.NEAREST).ToString();

            if (hMinTurn < PANS_OPS_DataBase.dpGui_Ar1.Value)
                TextBox401.ForeColor = System.Drawing.Color.FromArgb(0XFF0000);
            else
                TextBox401.ForeColor = System.Drawing.Color.FromArgb(0);

            if (hMaxTurn < PANS_OPS_DataBase.dpGui_Ar1.Value)
                TextBox402.ForeColor = System.Drawing.Color.FromArgb(0XFF0000);
            else
                TextBox402.ForeColor = System.Drawing.Color.FromArgb(0);

            // =============================================================
            pPoly = new Polyline();
            pPoly.AddPoint(DER.pPtPrj[eRWY.PtDER]);
            pPoly.AddPoint(Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir, GlobalVars.RModel));

            Functions.DeleteGraphicsElement(GlobalVars.StrTrackElem);
            GlobalVars.StrTrackElem = Functions.DrawPolyline(pPoly, GlobalVars.StrTrackElemColor, 1.0, false);

            if (GlobalVars.ButtonControl5State)
            {
                pGraphics.AddElement(GlobalVars.StrTrackElem, 0);
                GlobalVars.StrTrackElem.Locked = true;
            }

            GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            //Functions.RefreshCommandBar(_mTool, 2);

            // ===============================================================
            TextBox405.Text = Math.Round(100.0 * CurrPDG, 1).ToString();
            TextBox403.Text = NativeMethods.Modulus(Functions.RoundAngle(Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], DepDir) - GlobalVars.CurrADHP.MagVar)).ToString();
            // ============================================================

            if (double.Parse(TextBox502.Text) < double.Parse(TextBox402.Text))
                TextBox502.Text = TextBox402.Text;

            if (double.Parse(TextBox502.Text) > double.Parse(TextBox401.Text))
                TextBox502.Text = TextBox401.Text;

            ToolTip1.SetToolTip(TextBox502, Resources.str15248 + TextBox401.Text + " " + Label413.Text + Resources.str15162 + TextBox402.Text + " " + Label413.Text); // "?????????? ???????? ?????: ?? "" ? ?? "" ?"
            Label508.Text = ToolTip1.GetToolTip(TextBox502);
        }

        private void TextBox301_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char KeyAscii = eventArgs.KeyChar;
            bool Cancel = false;

            if (KeyAscii == 13)
                TextBox301_Validating(TextBox301, new System.ComponentModel.CancelEventArgs(Cancel));
            else
                Functions.TextBoxInteger(ref KeyAscii);

            eventArgs.KeyChar = KeyAscii;
            if (KeyAscii == 0)
                eventArgs.Handled = true;
        }

        private void TextBox301_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            if (TextBox301.Tag.ToString() == TextBox301.Text)
                return;

            if (TextBox301_Changed())
                TextBox301.Tag = TextBox301.Text;
        }

        private bool TextBox301_Changed()
        {
            double speed;

            if (!double.TryParse(TextBox301.Text, out speed))
                return false;

            speed = Functions.DeConvertSpeed(speed);

            fIAS = speed;

            if (fIAS < 1.1 * Categories_DATABase.cVmaInter.Values[AirCat])
                fIAS = 1.1 * Categories_DATABase.cVmaInter.Values[AirCat];
            else if (fIAS > 1.1 * Categories_DATABase.cVmaFaf.Values[AirCat])
                fIAS = 1.1 * Categories_DATABase.cVmaFaf.Values[AirCat];

            if (speed != fIAS)
                TextBox301.Text = Functions.ConvertSpeed(fIAS, eRoundMode.NEAREST).ToString();

            return true;
        }

        private void TextBox302_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char KeyAscii = eventArgs.KeyChar;
            bool Cancel = false;
            if (KeyAscii == 13)
                TextBox302_Validating(TextBox302, new System.ComponentModel.CancelEventArgs(Cancel));
            else
                Functions.TextBoxInteger(ref KeyAscii);

            eventArgs.KeyChar = KeyAscii;
            if (KeyAscii == 0)
                eventArgs.Handled = true;
        }

        private void TextBox302_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            double fTmp;
            if (!double.TryParse(TextBox302.Text, out fTmp))
                return;

            if (TextBox302.Tag.ToString() == TextBox302.Text)
                return;

            fBankAngle = fTmp;
            if (fBankAngle < 1.0)
                fBankAngle = 1.0;

            if (fBankAngle < PANS_OPS_DataBase.dpT_Bank.Value - 5)
                fBankAngle = PANS_OPS_DataBase.dpT_Bank.Value - 5;

            if (fBankAngle > PANS_OPS_DataBase.dpT_Bank.Value + 10)
                fBankAngle = PANS_OPS_DataBase.dpT_Bank.Value + 10;

            if (fTmp != fBankAngle)
                TextBox302.Text = fBankAngle.ToString();

            TextBox302.Tag = TextBox302.Text;
        }

        private void TextBox405_KeyPress(object sender, KeyPressEventArgs e)
        {
            char eventChar = e.KeyChar;

            if (eventChar == 13)
                TextBox405_Validating(TextBox405, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox405.Text);

            e.KeyChar = eventChar;
            if (eventChar == 0)
                e.Handled = true;
        }

        private void TextBox405_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            double fTmp;

            if (!double.TryParse(TextBox405.Text, out fTmp))
            {
                if (double.TryParse(TextBox405.Tag.ToString(), out fTmp))
                    TextBox405.Text = TextBox405.Tag.ToString();
                return;
            }

            fTmp *= 0.01;

            if (fTmp > PANS_OPS_DataBase.dpMaxPosPDG.Value * 50.0)
            {
                fTmp = PANS_OPS_DataBase.dpMaxPosPDG.Value * 50.0;
                TextBox405.Text = Math.Round(100.0 * fTmp, 1).ToString();
            }

            if (fTmp < PANS_OPS_DataBase.dpPDG_Nom.Value)
            {
                fTmp = PANS_OPS_DataBase.dpPDG_Nom.Value;
                TextBox405.Text = Math.Round(100.0 * fTmp, 1).ToString();
            }
        }

        private void TextBox503_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char eventChar = eventArgs.KeyChar;
            if (eventChar == 13)
                TextBox503_Validating(TextBox503, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox503.Text);

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        private void TextBox503_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            double fTmp, hTurn, fTASl, Range = (hMaxTurn - PANS_OPS_DataBase.dpH_abv_DER.Value) / CurrPDG;

            // ===============================
            if (double.TryParse(TextBox503.Text, out fTmp) && OptionButton301.Checked)
            {
                if (TextBox503.Tag.ToString() == TextBox503.Text)
                    return;

                double iniH = Functions.DeConvertHeight(fTmp) - DER.pPtPrj[eRWY.PtDER].Z;

                if (iniH < hMinTurn)
                    iniH = hMinTurn;

                if (iniH > hMaxTurn)
                    iniH = hMaxTurn;

                ObstacleContainer TmpInList;

                if (Range == GlobalVars.RModel)
                {
                    int n = oZNRList.Obstacles.Length;
                    TmpInList.Obstacles = new Obstacle[n];
                    System.Array.Copy(oZNRList.Obstacles, TmpInList.Obstacles, n);

                    n = oZNRList.Parts.Length;
                    TmpInList.Parts = new ObstacleData[n];
                    System.Array.Copy(oZNRList.Parts, TmpInList.Parts, n);
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

                hTurn = newH + DER.pPtPrj[eRWY.PtDER].Z;
                Range = (newH - PANS_OPS_DataBase.dpH_abv_DER.Value) / CurrPDG;

                Functions.GetObstInRange(oZNRList, out TmpInList, Range);
                ReportsFrm.FillPage4(TmpInList, OptionButton301.Checked);
                // ============================================================
                fTASl = Functions.IAS2TAS(fIAS, hTurn, GlobalVars.CurrADHP.ISAtC);
                double VTotal = fTASl + GlobalVars.CurrADHP.WindSpeed;
                double Ls = VTotal * PANS_OPS_DataBase.dpT_TechToleranc.Value * 0.277777777777778;

                IClone pSource = (ESRI.ArcGIS.esriSystem.IClone)pPolygon;
                IPointCollection pPolyClone = (IPointCollection)pSource.Clone();

                ITopologicalOperator2 pTopo = (ITopologicalOperator2)pPolyClone;
                pTopo.IsKnownSimple_2 = false;
                pTopo.Simplify();
                // ====================
                double L0 = (newH - PANS_OPS_DataBase.dpOIS_abv_DER.Value) / CurrPDG;
                IPoint ptTmp = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir, L0);

                IPointCollection pLine = new Polyline();
                pLine.AddPoint(Functions.PointAlongPlane(ptTmp, DepDir - 90.0, GlobalVars.RModel));
                pLine.AddPoint(Functions.PointAlongPlane(ptTmp, DepDir + 90.0, GlobalVars.RModel));

                KK = pTopo.Intersect(pLine, esriGeometryDimension.esriGeometry1Dimension) as IPolyline;
                if (Functions.SideDef(KK.ToPoint, DepDir, KK.FromPoint) < 0)
                    KK.ReverseOrientation();

                // ==========================
                TurnFixPnt = new ESRI.ArcGIS.Geometry.Point();
                IConstructPoint pConstruct = (IConstructPoint)TurnFixPnt;
                pConstruct.ConstructAngleIntersection(DER.pPtPrj[eRWY.PtDER], GlobalVars.DegToRadValue * DepDir, KK.ToPoint, GlobalVars.DegToRadValue * (DepDir + 90.0));

                TurnFixPnt.Z = hTurn;
                TurnFixPnt.M = DepDir;

                ptTmp = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir, L0 + Ls);
                pLine.RemovePoints(0, 2);
                pLine.AddPoint(Functions.PointAlongPlane(ptTmp, DepDir - 90.0, GlobalVars.RModel));
                pLine.AddPoint(Functions.PointAlongPlane(ptTmp, DepDir + 90.0, GlobalVars.RModel));

                K1K1 = pTopo.Intersect(pLine, esriGeometryDimension.esriGeometry1Dimension) as IPolyline;
                if (Functions.SideDef(K1K1.ToPoint, DepDir, K1K1.FromPoint) < 0)
                    K1K1.ReverseOrientation();

                // ============================================================
                TextBox502.Text = Functions.ConvertHeight(newH, eRoundMode.NEAREST).ToString();
                TextBox502.Tag = TextBox502.Text;

                TextBox503.Text = Functions.ConvertHeight(newH + DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.NEAREST).ToString();
                TextBox503.Tag = TextBox503.Text;
                // ============================================================
                IPointCollection pPoly = new Polyline();
                pPoly.AddPoint(DER.pPtPrj[eRWY.PtDER]);
                pPoly.AddPoint(TurnFixPnt);


                Functions.DeleteGraphicsElement(GlobalVars.StrTrackElem);
                GlobalVars.StrTrackElem = Functions.DrawPolyline(pPoly, GlobalVars.StrTrackElemColor, 1.0, false);

                Functions.DeleteGraphicsElement(GlobalVars.FIXElem);

                if ((GlobalVars.FIXElem != null) && GlobalVars.ButtonControl8State)
                {
                    if (GlobalVars.FIXElem is IGroupElement)
                    {
                        IGroupElement pGroupElement = (IGroupElement)GlobalVars.FIXElem;
                        for (int i = 0; i < pGroupElement.ElementCount; i++)
                        {
                            pGraphics.AddElement(pGroupElement.get_Element(i), 0);
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

                if (GlobalVars.ButtonControl5State)
                {
                    pGraphics.AddElement(GlobalVars.StrTrackElem, 0);
                    GlobalVars.StrTrackElem.Locked = true;
                }
                //Functions.RefreshCommandBar(_mTool, 128);
            }

            fTASl = Functions.IAS2TAS(fIAS, TurnFixPnt.Z, GlobalVars.CurrADHP.ISAtC);

            Text502.Text = Functions.ConvertSpeed(fIAS, eRoundMode.NEAREST).ToString();
            Text503.Text = Functions.ConvertSpeed(fTASl, eRoundMode.NEAREST).ToString();
            Text504.Text = Functions.ConvertDistance(PANS_OPS_DataBase.dpT_TechToleranc.Value / 3.6 * (GlobalVars.CurrADHP.WindSpeed + fTASl), eRoundMode.NEAREST).ToString();

            double Rv = 6355.0 * System.Math.Tan(GlobalVars.DegToRadValue * fBankAngle) / (GlobalVars.PI * fTASl);
            if (Rv > 3.0)
                Rv = 3.0;

            Text505.Text = System.Math.Round(Rv, 2).ToString();

            if (Rv > 0.0)
            {
                double r0 = fTASl / (0.02 * GlobalVars.PI * Rv);
                Text506.Text = Functions.ConvertDistance(r0, eRoundMode.NEAREST).ToString();
            }
            else
                Text506.Text = "-";

            double E = 25.0 * GlobalVars.CurrADHP.WindSpeed / Rv;
            Text507.Text = Functions.ConvertDistance(E, eRoundMode.NEAREST).ToString();

            Text508.Text = Functions.ConvertHeight(TurnFixPnt.Z, eRoundMode.NEAREST).ToString();

            if (OptionButton301.Checked)
                Text501.Text = Functions.ConvertDistance(Range, eRoundMode.NEAREST).ToString();
            else
            {
                IProximityOperator pProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)KK;
                Text501.Text = Functions.ConvertDistance(pProxi.ReturnDistance(DER.pPtPrj[eRWY.PtDER]), eRoundMode.NEAREST).ToString();

                double L1 = Functions.ReturnDistanceInMeters(DER.pPtPrj[eRWY.PtDER], TurnFixPnt);
                double TurnFixPntAltitude = CurrPDG * L1 + DER.pPtPrj[eRWY.PtDER].Z + PANS_OPS_DataBase.dpH_abv_DER.Value;
                Text509.Text = Functions.ConvertHeight(TurnFixPntAltitude, eRoundMode.NEAREST).ToString();

                Text510.Text = Functions.ConvertDistance(pProxi.ReturnDistance(KKFixMax), eRoundMode.NEAREST).ToString();
            }
        }

        private void TextBox603_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char KeyAscii = eventArgs.KeyChar;
            if (KeyAscii == 13)
                TextBox603_Validating(TextBox603, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxInteger(ref KeyAscii);

            eventArgs.KeyChar = KeyAscii;
            if (KeyAscii == 0)
                eventArgs.Handled = true;
        }

        private void TextBox603_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            double fTmp;
            if (!double.TryParse(TextBox603.Text, out fTmp))
            {
                if (double.TryParse(TextBox603.Tag.ToString(), out fTmp))
                    TextBox603.Text = TextBox603.Tag.ToString();
                else
                    TextBox603.Text = PANS_OPS_DataBase.dpInterMinAngle.Value.ToString();
            }

            fTmp = double.Parse(TextBox603.Text);

            if (fTmp < PANS_OPS_DataBase.dpInterMinAngle.Value - PANS_OPS_DataBase.dpFlightTechTol.Value)
                TextBox603.Text = (PANS_OPS_DataBase.dpInterMinAngle.Value - PANS_OPS_DataBase.dpFlightTechTol.Value).ToString();
            else if (fTmp > PANS_OPS_DataBase.dpInterMaxAngle.Value + PANS_OPS_DataBase.dpFlightTechTol.Value)
                TextBox603.Text = (PANS_OPS_DataBase.dpInterMaxAngle.Value + PANS_OPS_DataBase.dpFlightTechTol.Value).ToString();

            if (TextBox603.Tag.ToString() == TextBox603.Text)
                return;

            if (MultiPage1.SelectedIndex >= 4)
            {
                UpdateIntervals(false);
                TextBox602.Tag = "a";
                TextBox602_Validating(TextBox602, new System.ComponentModel.CancelEventArgs());
                TextBox603.Tag = TextBox603.Text;
            }
        }

        private void UpdateIntervals(bool ChangeCurDir = false)
        {
            if (MultiPage1.SelectedIndex < 4)
                return;

            double Snap = double.Parse(TextBox603.Text);
            double tStabl = double.Parse(TextBox601.Text);
            double fTASl = Functions.IAS2TAS(fIAS, (TurnFixPnt.Z), GlobalVars.CurrADHP.ISAtC);
            double L0, VTotal = fTASl + GlobalVars.CurrADHP.WindSpeed;

            if (OptionButton301.Checked)
                L0 = Functions.Point2LineDistancePrj(DER.pPtPrj[eRWY.PtDER], KK.FromPoint, DepDir + 90.0);
            else
                L0 = Functions.Point2LineDistancePrj(DER.pPtPrj[eRWY.PtDER], KKFixMax.FromPoint, DepDir + 90.0);

            IPoint CurrPnt;
            if (OptionButton301.Checked)
                CurrPnt = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir, L0);
            else
            {
                CurrPnt = new ESRI.ArcGIS.Geometry.Point();
                CurrPnt.PutCoords(TurnFixPnt.X, TurnFixPnt.Y);
            }

            double TurnR = Functions.Bank2Radius(fBankAngle, fTASl);
            // ==========================================
            double ddr = TurnR / (System.Math.Tan(GlobalVars.DegToRadValue * ((180.0 - Snap) * 0.5)));
            double dr = PANS_OPS_DataBase.dpT_Gui_dist.Value + ddr;
            double lMinDR = VTotal * tStabl * 0.277777777777778 + ddr;
            MinDR = System.Math.Round(lMinDR - ddr);
            ToolTip1.SetToolTip(TextBox601, Resources.str15250 + Functions.ConvertDistance(MinDR, eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit); // "??????????? ????? DR = "" ????"
                                                                                                                                                                                                      // ==========================================

            IPoint ptCnt = Functions.PointAlongPlane(CurrPnt, DepDir + 90.0 * TurnDir, TurnR);
            double bAz = Functions.ReturnAngleInDegrees(TurnDirector.pPtPrj, ptCnt);
            double d = Functions.ReturnDistanceInMeters(ptCnt, TurnDirector.pPtPrj);

            double R = System.Math.Sqrt(dr * dr + TurnR * TurnR);
            double delta = GlobalVars.RadToDegValue * (System.Math.Atan(TurnR / dr));
            double alpha = Snap - TurnDir * delta;

            double xMin, xMax, fTmp = R * System.Math.Sin(GlobalVars.DegToRadValue * alpha) / d;
            if (fTmp > 1.0)
                xMin = 90.0;
            else if (fTmp < -1.0)
                xMin = -90.0;
            else
                xMin = GlobalVars.RadToDegValue * Functions.ArcSin(fTmp);

            alpha = Snap + TurnDir * delta;
            fTmp = R * System.Math.Sin(GlobalVars.DegToRadValue * alpha) / d;
            if (fTmp > 1.0)
                xMax = 90.0;
            else if (fTmp < -1.0)
                xMax = -90.0;
            else
                xMax = GlobalVars.RadToDegValue * Functions.ArcSin(fTmp);

            Interval[] Intervals = new Interval[4];

            TextBox403.Text = NativeMethods.Modulus(Functions.RoundAngle(Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], DepDir) - GlobalVars.CurrADHP.MagVar)).ToString();
            Intervals[0].Left = NativeMethods.Modulus(Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], bAz + xMin) - GlobalVars.CurrADHP.MagVar);
            Intervals[1].Right = NativeMethods.Modulus(Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], bAz - xMax) - GlobalVars.CurrADHP.MagVar);
            Intervals[2].Left = NativeMethods.Modulus(Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], bAz + xMax) + 180.0 - GlobalVars.CurrADHP.MagVar);
            Intervals[3].Right = NativeMethods.Modulus(Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], bAz - xMin) + 180.0 - GlobalVars.CurrADHP.MagVar);

            R = System.Math.Sqrt(lMinDR * lMinDR + TurnR * TurnR);

            delta = GlobalVars.RadToDegValue * (System.Math.Atan(TurnR / lMinDR));
            alpha = Snap - TurnDir * delta;

            fTmp = R * System.Math.Sin(GlobalVars.DegToRadValue * alpha) / d;
            if (fTmp > 1.0)
                xMin = 90.0;
            else if (fTmp < -1.0)
                xMin = -90.0;
            else
                xMin = GlobalVars.RadToDegValue * Functions.ArcSin(fTmp);

            alpha = Snap + TurnDir * delta;
            fTmp = R * System.Math.Sin(GlobalVars.DegToRadValue * alpha) / d;
            if (fTmp > 1.0)
                xMax = 90.0;
            else if (fTmp < -1.0)
                xMax = -90.0;
            else
                xMax = GlobalVars.RadToDegValue * Functions.ArcSin(fTmp);

            Intervals[0].Right = NativeMethods.Modulus(Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], bAz + xMin) - GlobalVars.CurrADHP.MagVar);
            Intervals[1].Left = NativeMethods.Modulus(Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], bAz - xMax) - GlobalVars.CurrADHP.MagVar);
            Intervals[2].Right = NativeMethods.Modulus(Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], bAz + xMax) + 180.0 - GlobalVars.CurrADHP.MagVar);
            Intervals[3].Left = NativeMethods.Modulus(Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], bAz - xMin) + 180.0 - GlobalVars.CurrADHP.MagVar);

            int i, j = 0, n = Intervals.Length;
            // SortIntervals Intervals

            while (j < n - 1)
            {
                if (Functions.SubtractAngles(Intervals[j].Right, Intervals[j + 1].Left) <= 1.0)
                {
                    Intervals[j].Right = Intervals[j + 1].Right;

                    n--;
                    for (i = j + 1; i < n; i++)
                        Intervals[i] = Intervals[i + 1];
                }
                else
                    j++;
            }

            if (n > 1)
            {
                if (Functions.SubtractAngles(Intervals[0].Left, Intervals[n - 1].Right) <= 1.0)
                {
                    //     if (Intervals(0).Left = Intervals(N).Right)
                    n--;
                    Intervals[0].Left = Intervals[n].Left;
                }
            }

            if (n >= 0)
                System.Array.Resize<Interval>(ref Intervals, n);
            else
                Intervals = new Interval[0];

            Functions.SortIntervals(Intervals, false);

            RecommendStr = Resources.str15252; // "????????????? ????????? ?????: "
            string tmpStr = RecommendStr + "\r\n";

            // CurVal = -1 ∞
            // If (IsNumeric(TextBox602.Text)) Then
            //     CurVal = CInt(TextBox602.Text)
            // End If

            for (i = 0; i < n; i++)
            {
                if (Functions.SubtractAngles(System.Math.Round(Intervals[i].Left), System.Math.Round(Intervals[i].Right)) <= GlobalVars.degEps)
                {
                    RecommendStr = RecommendStr + Functions.DegToStr(System.Math.Round(Intervals[i].Left));
                    tmpStr = tmpStr + Functions.DegToStr(System.Math.Round(Intervals[i].Left));
                    if (i == 0 && ChangeCurDir)
                        TextBox602.Text = System.Math.Round(Intervals[0].Left).ToString();
                }
                else
                {
                    RecommendStr = RecommendStr + Resources.str15098 + Functions.DegToStr(System.Math.Round(Intervals[i].Left + 0.4999)) + Resources.str15103 + Functions.DegToStr(System.Math.Round(Intervals[i].Right - 0.4999)); // "?? "" ?? "
                    tmpStr = tmpStr + Resources.str15098 + Functions.DegToStr(System.Math.Round(Intervals[i].Left + 0.4999)) + Resources.str15103 + Functions.DegToStr(System.Math.Round(Intervals[i].Right - 0.4999)); // "?? ""? ?? "
                    if ((i == 0) && ChangeCurDir)
                        TextBox602.Text = System.Math.Round(Intervals[0].Left + 0.4999).ToString();
                }

                if (i != n - 1)
                {
                    RecommendStr = RecommendStr + "; ";
                    tmpStr = tmpStr + "\r\n";
                }
            }

            if (ChangeCurDir)
                DirCourse = Functions.Azt2Dir(DER.pPtGeo[eRWY.PtDER], double.Parse(TextBox602.Text) + GlobalVars.CurrADHP.MagVar);

            Label609.Text = tmpStr;

            if (OptionButton602.Checked)
                ToolTip1.SetToolTip(TextBox602, RecommendStr);
            else
                ToolTip1.SetToolTip(TextBox602, "");
        }

        private int UpdateToNavCurs(double OutAzt)
        {
            double fTASl = Functions.IAS2TAS(fIAS, TurnFixPnt.Z, GlobalVars.CurrADHP.ISAtC);
            double TurnR = Functions.Bank2Radius(fBankAngle, fTASl);
            double dDir, InterAngle = double.Parse(TextBox603.Text);

            MPtCollection = Functions.CalcTouchByFixDir(TurnFixPnt, TurnDirector.pPtPrj, TurnR, DepDir, ref OutAzt, TurnDir, InterAngle, out dDir, out FlyBy);

            //if (dDir > GlobalVars.RModel)
            //{
            //	MessageBox.Show(Resources.str15256, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //	return -1;
            //}

            IPoint ptTmp, ptCurr = MPtCollection.Point[MPtCollection.PointCount - 1];
            TracPoly = (IPointCollection)Functions.CalcTrajectoryFromMultiPoint(MPtCollection);
            IProximityOperator pProxi;

            if (ComboBox603.SelectedIndex == 1)
            {
                if (Functions.SideDef(ptCurr, OutAzt + 90.0, WPt602.pPtPrj) > 0)
                    TracPoly.AddPoint(WPt602.pPtPrj);
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

            Label604.Text = Functions.ConvertDistance(dDir, eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit; // " ??????"

            if (dDir > PANS_OPS_DataBase.dpT_Gui_dist.Value || dDir < MinDR)
                Label604.ForeColor = System.Drawing.Color.Red;
            else
                Label604.ForeColor = System.Drawing.Color.Black;

            OutAzt = MPtCollection.Point[1].M;

            if (BasePoints.PointCount > 0)
                BasePoints.RemovePoints(0, BasePoints.PointCount);

            if (ZNR_Poly.PointCount > 0)
                ZNR_Poly.RemovePoints(0, ZNR_Poly.PointCount);

            if (TurnDir > 0)
            {
                BasePoints.AddPoint(K1K1.FromPoint);
                BasePoints.AddPoint(K1K1.ToPoint);

                if (OptionButton301.Checked)
                { // ?? ??? FIX
                    if (AdjustDir < 0)
                    {
                        if (CheckBox301.Checked && OptionButton301.Checked)
                            BasePoints.AddPoint(pPolygon.Point[5]);
                        else
                            BasePoints.AddPoint(pPolygon.Point[4]);
                    }

                    if (CheckBox301.Checked && OptionButton301.Checked)
                    {
                        BasePoints.AddPoint(pPolygon.Point[6]);
                        BasePoints.AddPoint(pPolygon.Point[7]);
                        BasePoints.AddPoint(pPolygon.Point[0]);
                        BasePoints.AddPoint(pPolygon.Point[1]);
                    }
                    else
                    {
                        BasePoints.AddPoint(pPolygon.Point[5]);
                        BasePoints.AddPoint(pPolygon.Point[0]);
                    }

                    if (AdjustDir > 0)
                    {
                        if (CheckBox301.Checked && OptionButton301.Checked)
                            BasePoints.AddPoint(pPolygon.Point[2]);
                        else
                            BasePoints.AddPoint(pPolygon.Point[1]);
                    }
                }
                else
                {
                    BasePoints.AddPoint(KK.ToPoint);
                    BasePoints.AddPoint(KK.FromPoint);
                }
            }
            else
            {
                BasePoints.AddPoint(K1K1.ToPoint);
                BasePoints.AddPoint(K1K1.FromPoint);

                if (OptionButton301.Checked)
                { // ?? ??? FIX
                    if (AdjustDir > 0)
                    {
                        if (CheckBox301.Checked && OptionButton301.Checked)
                            BasePoints.AddPoint(pPolygon.get_Point(2));
                        else
                            BasePoints.AddPoint(pPolygon.get_Point(1));
                    }

                    if (CheckBox301.Checked && OptionButton301.Checked)
                    {
                        BasePoints.AddPoint(pPolygon.get_Point(1));
                        BasePoints.AddPoint(pPolygon.get_Point(0));
                        BasePoints.AddPoint(pPolygon.get_Point(7));
                        BasePoints.AddPoint(pPolygon.get_Point(6));
                    }
                    else
                    {
                        BasePoints.AddPoint(pPolygon.get_Point(0));
                        BasePoints.AddPoint(pPolygon.get_Point(5));
                    }

                    if (AdjustDir < 0)
                    {
                        if (CheckBox301.Checked && OptionButton301.Checked)
                            BasePoints.AddPoint(pPolygon.get_Point(5));
                        else
                            BasePoints.AddPoint(pPolygon.get_Point(4));
                    }
                }
                else
                {
                    BasePoints.AddPoint(KK.FromPoint);
                    BasePoints.AddPoint(KK.ToPoint);
                }
            }

            ZNR_Poly.AddPoint(pPolygon.get_Point(0));
            ZNR_Poly.AddPoint(pPolygon.get_Point(1));

            if (CheckBox301.Checked && OptionButton301.Checked)
            {
                ZNR_Poly.AddPoint(pPolygon.get_Point(2));
                ZNR_Poly.AddPoint(KK.FromPoint);
                ZNR_Poly.AddPoint(KK.ToPoint);
                ZNR_Poly.AddPoint(pPolygon.get_Point(pPolygon.PointCount - 3));
            }
            else
            {
                ZNR_Poly.AddPoint(KK.FromPoint);
                ZNR_Poly.AddPoint(KK.ToPoint);
            }

            ZNR_Poly.AddPoint(pPolygon.Point[pPolygon.PointCount - 2]);
            ZNR_Poly.AddPoint(pPolygon.Point[pPolygon.PointCount - 1]);

            ITopologicalOperator2 pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)ZNR_Poly;
            pTopoOper.IsKnownSimple_2 = false;
            pTopoOper.Simplify();

            TurnArea = CreateTurnArea(3.6 * PANS_OPS_DataBase.dpWind_Speed.Value, TurnR, fTASl, ref DepDir, ref OutAzt, TurnDir, BasePoints);

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

            if (CheckBox301.Checked && OptionButton301.Checked)
            {
                if (TurnDir > 0)
                    TurnArea.AddFirstPoint(pPolygon.Point[pPolygon.PointCount - 2]);
                else
                    TurnArea.AddFirstPoint(pPolygon.Point[1]);
            }

            IPointCollection TmpPoly;
            if (OptionButton301.Checked)
                TmpPoly = ZNR_Poly;
            else
                TmpPoly = pFIXPoly;
            ptCurr = TurnArea.Point[TurnArea.PointCount - 1];

            double maxDist = 0.0, AztCurr = OutAzt + PANS_OPS_DataBase.dpafTrn_OSplay.Value * TurnDir;
            int n = TmpPoly.PointCount, iMax = 0;
            for (int i = 0; i < n; i++)
            {
                double currDist = Functions.Point2LineDistancePrj(TmpPoly.Point[i], ptCurr, AztCurr);
                if (maxDist < currDist)
                {
                    iMax = i;
                    maxDist = currDist;
                }
            }

            ptTmp = TmpPoly.Point[iMax];

            TurnArea.AddFirstPoint(ptTmp);
            TmpPoly = new ESRI.ArcGIS.Geometry.Polygon();
            TmpPoly.AddPoint(ptTmp);
            TmpPoly.AddPoint(ptCurr);

            Prim = new ESRI.ArcGIS.Geometry.Polygon();
            SecL = new ESRI.ArcGIS.Geometry.Polygon();
            SecR = new ESRI.ArcGIS.Geometry.Polygon();

            DirToNav = MPtCollection.Point[MPtCollection.PointCount - 1].M;

            Functions.CreateNavaidZone(TurnDirector.pPtPrj, DirToNav, TurnDirector.TypeCode, 2.0, SecL, SecR, Prim);
            IPointCollection NewPoly = CalcAreaIntersection(TurnArea, OutAzt, DirToNav, TurnDir);

            pTopoOper = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(BasePoints));
            pTopoOper.IsKnownSimple_2 = false;
            pTopoOper.Simplify();

            pTopoOper = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(NewPoly));
            pTopoOper.IsKnownSimple_2 = false;
            pTopoOper.Simplify();

            BaseArea = Functions.RemoveAgnails(Functions.RemoveHoles((IPolygon)pTopoOper.Union(BasePoints)));

            if (OptionButton301.Checked)
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

            IPointCollection DrawPoly = Functions.PolygonIntersection(pCircle, Prim) as IPointCollection;
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
            //Functions.RefreshCommandBar(_mTool, 122);

            return 0;
        }

        private int UpdateToFix()
        {
            double fTASl = Functions.IAS2TAS(fIAS, TurnFixPnt.Z, GlobalVars.CurrADHP.ISAtC);
            double TurnR = Functions.Bank2Radius(fBankAngle, fTASl);
            MPtCollection = Functions.TurnToFixPrj(TurnFixPnt, TurnR, TurnDir, TurnDirector.pPtPrj);

            if (MPtCollection.PointCount < 2)
            {
                MessageBox.Show(Resources.str15257 + "\r\n" +
                    Resources.str15258 + "\r\n" +
                    Resources.str15259 + Functions.ConvertDistance(TurnR, eRoundMode.NEAREST).ToString() + " " +
                    GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

            OutPt = MPtCollection.get_Point(1);

            DirToNav = MPtCollection.get_Point(MPtCollection.PointCount - 1).M;
            OutAzt = MPtCollection.get_Point(1).M;
            DirCourse = OutAzt;

            TextBox602.Text = NativeMethods.Modulus(System.Math.Round(Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], OutAzt) - GlobalVars.CurrADHP.MagVar)).ToString();
            TextBox602.Tag = TextBox602.Text;

            if ((TurnDirector.TypeCode != eNavaidType.VOR) && (TurnDirector.TypeCode != eNavaidType.NDB))
            {
                UpdateToCourse(OutAzt);
                return 0;
            }
            // ====================================
            if (BasePoints.PointCount > 0)
                BasePoints.RemovePoints(0, BasePoints.PointCount);

            if (ZNR_Poly.PointCount > 0)
                ZNR_Poly.RemovePoints(0, ZNR_Poly.PointCount);

            if (TurnDir > 0)
            {
                BasePoints.AddPoint(K1K1.FromPoint);
                BasePoints.AddPoint(K1K1.ToPoint);

                if (OptionButton301.Checked)
                {
                    if (AdjustDir < 0)
                    {
                        if (CheckBox301.Checked && OptionButton301.Checked)
                            BasePoints.AddPoint(pPolygon.get_Point(5));
                        else
                            BasePoints.AddPoint(pPolygon.get_Point(4));
                    }

                    if (CheckBox301.Checked && OptionButton301.Checked)
                    {
                        BasePoints.AddPoint(pPolygon.get_Point(6));
                        BasePoints.AddPoint(pPolygon.get_Point(7));
                        BasePoints.AddPoint(pPolygon.get_Point(0));
                        BasePoints.AddPoint(pPolygon.get_Point(1));
                    }
                    else
                    {
                        BasePoints.AddPoint(pPolygon.get_Point(5));
                        BasePoints.AddPoint(pPolygon.get_Point(0));
                    }

                    if (AdjustDir > 0)
                    {
                        if (CheckBox301.Checked && OptionButton301.Checked)
                            BasePoints.AddPoint(pPolygon.get_Point(2));
                        else
                            BasePoints.AddPoint(pPolygon.get_Point(1));
                    }
                }
                else
                {
                    BasePoints.AddPoint(KK.ToPoint);
                    BasePoints.AddPoint(KK.FromPoint);
                }
            }
            else
            {
                BasePoints.AddPoint(K1K1.ToPoint);
                BasePoints.AddPoint(K1K1.FromPoint);

                if (OptionButton301.Checked)
                {
                    if (AdjustDir > 0)
                    {
                        if (CheckBox301.Checked && OptionButton301.Checked)
                            BasePoints.AddPoint(pPolygon.get_Point(2));
                        else
                            BasePoints.AddPoint(pPolygon.get_Point(1));
                    }

                    if (CheckBox301.Checked && OptionButton301.Checked)
                    {
                        BasePoints.AddPoint(pPolygon.get_Point(1));
                        BasePoints.AddPoint(pPolygon.get_Point(0));
                        BasePoints.AddPoint(pPolygon.get_Point(7));
                        BasePoints.AddPoint(pPolygon.get_Point(6));
                    }
                    else
                    {
                        BasePoints.AddPoint(pPolygon.get_Point(0));
                        BasePoints.AddPoint(pPolygon.get_Point(5));
                    }

                    if (AdjustDir < 0)
                    {
                        if (CheckBox301.Checked && OptionButton301.Checked)
                            BasePoints.AddPoint(pPolygon.get_Point(5));
                        else
                            BasePoints.AddPoint(pPolygon.get_Point(4));
                    }
                }
                else
                {
                    BasePoints.AddPoint(KK.FromPoint);
                    BasePoints.AddPoint(KK.ToPoint);
                }
            }

            ZNR_Poly.AddPoint(pPolygon.get_Point(0));
            ZNR_Poly.AddPoint(pPolygon.get_Point(1));

            if (CheckBox301.Checked && OptionButton301.Checked)
            {
                ZNR_Poly.AddPoint(pPolygon.get_Point(2));
                ZNR_Poly.AddPoint(KK.FromPoint);
                ZNR_Poly.AddPoint(KK.ToPoint);
                ZNR_Poly.AddPoint(pPolygon.get_Point(pPolygon.PointCount - 3));
            }
            else
            {
                ZNR_Poly.AddPoint(KK.FromPoint);
                ZNR_Poly.AddPoint(KK.ToPoint);
            }

            ZNR_Poly.AddPoint(pPolygon.get_Point(pPolygon.PointCount - 2));
            ZNR_Poly.AddPoint(pPolygon.get_Point(pPolygon.PointCount - 1));

            ITopologicalOperator2 pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)ZNR_Poly;
            pTopoOper.IsKnownSimple_2 = false;
            pTopoOper.Simplify();

            TurnArea = CreateTurnArea(3.6 * PANS_OPS_DataBase.dpWind_Speed.Value, TurnR, fTASl, ref DepDir, ref OutAzt, TurnDir, BasePoints);
            // ===========================================
            TracPoly = (IPointCollection)Functions.CalcTrajectoryFromMultiPoint(MPtCollection);
            //     If CheckBox601.Value = 0 Then
            //         Set ptCur = MPtCollection.Point(MPtCollection.PointCount - 1)
            //         Dim pProxi As IProximityOperator
            //         Set pProxi = pCircle
            //         If pProxi.ReturnDistance(ptCur) = 0.0 Then
            //             CircleVectorIntersect PtAirportPrj, R, ptCur, ptCur.M, ptTmp
            // 
            //             TracPoly.AddPoint ptTmp
            //         End If
            //     End If

            TracPoly.AddPoint(TurnDirector.pPtPrj);
            // ====================================================

            MPtCollection.get_Point(1).M = OutAzt;

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

            if (CheckBox301.Checked && OptionButton301.Checked)
            {
                if (TurnDir > 0)
                    TurnArea.AddFirstPoint(pPolygon.get_Point(pPolygon.PointCount - 2));
                else
                    TurnArea.AddFirstPoint(pPolygon.get_Point(1));
            }

            IPoint ptCurr = TurnArea.get_Point(TurnArea.PointCount - 1);
            double AztCur = OutAzt + PANS_OPS_DataBase.dpafTrn_OSplay.Value * TurnDir;

            IPointCollection TmpPoly;
            if (OptionButton301.Checked)
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

            IPoint ptTmp = TmpPoly.get_Point(iMax);
            TurnArea.AddFirstPoint(ptTmp);

            TmpPoly = new ESRI.ArcGIS.Geometry.Polygon();
            TmpPoly.AddPoint(ptTmp);
            TmpPoly.AddPoint(TurnArea.get_Point(TurnArea.PointCount - 1));
            // ============= To FIX ============

            CalcJoiningParams(TurnDirector.TypeCode, TurnDir, TurnArea, OutPt, OutAzt);
            IPointCollection NewPoly = ApplayJoining(TurnDirector.TypeCode, TurnDir, TurnArea, BasePoints as IPolygon, OutPt, OutAzt, TmpPoly) as IPointCollection;
            double fTmp = Functions.ReturnAngleInDegrees(TurnArea.get_Point(TurnArea.PointCount - 1), TurnArea.get_Point(TurnArea.PointCount - 2));
            // ==============
            NewPoly = Functions.RemoveAgnails(NewPoly);

            pTopoOper = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(TmpPoly));
            pTopoOper.IsKnownSimple_2 = false;
            pTopoOper.Simplify();

            pTopoOper = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(BasePoints));
            pTopoOper.IsKnownSimple_2 = false;
            pTopoOper.Simplify();

            NewPoly = pTopoOper.Union(NewPoly) as IPointCollection;
            pTopoOper = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(NewPoly));
            pTopoOper.IsKnownSimple_2 = false;
            pTopoOper.Simplify();

            if (OptionButton301.Checked)
                NewPoly = pTopoOper.Difference(ZNR_Poly) as IPointCollection;
            else
                NewPoly = pTopoOper.Difference(pFIXPoly) as IPointCollection;

            pTopoOper = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(NewPoly));

            //     If SideFrom2Angle(DepDir + 90.0 * TurnDir, fTmp) * TurnDir < 0 Then
            if (NativeMethods.Modulus((OutAzt - DepDir) * TurnDir, 360.0) > 90.0)
            {
                NewPoly = pTopoOper.Union(TmpPoly) as IPointCollection;
                pTopoOper = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(NewPoly));
            }

            BaseArea = Functions.RemoveAgnails(Functions.RemoveHoles(NewPoly));

            if (OptionButton301.Checked)
            {
                pTopoOper = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(BaseArea));
                BaseArea = pTopoOper.Difference(ZNR_Poly) as IPointCollection;
            }
            else
                BaseArea = pTopoOper.Union(pFIXPoly) as IPointCollection;

            BaseArea = Functions.PolygonIntersection(pCircle, BaseArea) as IPointCollection;
            //     Set pTopoOper = pCircle
            //     Set BaseArea = pTopoOper.Intersect(BaseArea, esriGeometry2Dimension)

            Functions.DeleteGraphicsElement(GlobalVars.KKElem);
            Functions.DeleteGraphicsElement(GlobalVars.K1K1Elem);
            Functions.DeleteGraphicsElement(GlobalVars.TurnAreaElem);
            Functions.DeleteGraphicsElement(GlobalVars.PrimElem);
            Functions.DeleteGraphicsElement(GlobalVars.SecRElem);
            Functions.DeleteGraphicsElement(GlobalVars.SecLElem);
            Functions.DeleteGraphicsElement(GlobalVars.NomTrackElem);

            GlobalVars.KKElem = Functions.DrawPolyline(KK, 0, 1.0, false);
            GlobalVars.K1K1Elem = Functions.DrawPolyline(K1K1, 0, 1.0, false);

            IPointCollection DrawPoly = Functions.PolygonIntersection(pCircle, Prim) as IPointCollection;
            GlobalVars.PrimElem = Functions.DrawPolygon(DrawPoly, GlobalVars.PrimElemColor, esriSimpleFillStyle.esriSFSNull, false);

            DrawPoly = Functions.PolygonIntersection(pCircle, SecL) as IPointCollection;
            GlobalVars.SecLElem = Functions.DrawPolygon(DrawPoly, GlobalVars.SecLElemColor, esriSimpleFillStyle.esriSFSNull, false);

            DrawPoly = Functions.PolygonIntersection(pCircle, SecR) as IPointCollection;
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

            //Functions.RefreshCommandBar(_mTool, 122);
            return 1;
        }

        private int UpdateToCourse(double OutAzt)
        {
            double fTASl = Functions.IAS2TAS(fIAS, (TurnFixPnt.Z), GlobalVars.CurrADHP.ISAtC);
            double TurnR = Functions.Bank2Radius(fBankAngle, fTASl);

            IPoint ptTmp = Functions.PointAlongPlane(TurnFixPnt, DepDir + 90.0 * TurnDir, TurnR);
            IPoint Pt0 = Functions.PointAlongPlane(ptTmp, OutAzt - 90.0 * TurnDir, TurnR);
            Pt0.M = OutAzt;

            MPtCollection = new Multipoint();
            MPtCollection.AddPoint(TurnFixPnt);
            MPtCollection.AddPoint(Pt0);
            TracPoly = (IPointCollection)Functions.CalcTrajectoryFromMultiPoint(MPtCollection);

            if (OptionButton603.Checked)
                TracPoly.AddPoint(TurnDirector.pPtPrj);
            else
            {
                IProximityOperator pProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)pCircle;
                if (pProxi.ReturnDistance(Pt0) == 0.0)
                {
                    Functions.CircleVectorIntersect(GlobalVars.CurrADHP.pPtPrj, GlobalVars.RModel, Pt0, Pt0.M, out ptTmp);
                    TracPoly.AddPoint(ptTmp);
                }
            }

            if (BasePoints.PointCount > 0)
                BasePoints.RemovePoints(0, BasePoints.PointCount);

            if (ZNR_Poly.PointCount > 0)
                ZNR_Poly.RemovePoints(0, ZNR_Poly.PointCount);

            if (TurnDir > 0)
            {
                BasePoints.AddPoint(K1K1.FromPoint);
                BasePoints.AddPoint(K1K1.ToPoint);

                if (OptionButton301.Checked)
                { // ?? ??? FIX

                    if (AdjustDir < 0)
                    {
                        if (CheckBox301.Checked && OptionButton301.Checked)
                            BasePoints.AddPoint(pPolygon.get_Point(5));
                        else
                            BasePoints.AddPoint(pPolygon.get_Point(4));
                    }

                    if (CheckBox301.Checked && OptionButton301.Checked)
                    {
                        BasePoints.AddPoint(pPolygon.get_Point(6));
                        BasePoints.AddPoint(pPolygon.get_Point(7));
                        BasePoints.AddPoint(pPolygon.get_Point(0));
                        BasePoints.AddPoint(pPolygon.get_Point(1));
                    }
                    else
                    {
                        BasePoints.AddPoint(pPolygon.get_Point(5));
                        BasePoints.AddPoint(pPolygon.get_Point(0));
                    }

                    if (AdjustDir > 0)
                    {
                        if (CheckBox301.Checked && OptionButton301.Checked)
                            BasePoints.AddPoint(pPolygon.get_Point(2));
                        else
                            BasePoints.AddPoint(pPolygon.get_Point(1));
                    }
                }
                else
                {
                    BasePoints.AddPoint(KK.ToPoint);
                    BasePoints.AddPoint(KK.FromPoint);
                }
            }
            else
            {
                BasePoints.AddPoint(K1K1.ToPoint);
                BasePoints.AddPoint(K1K1.FromPoint);

                if (OptionButton301.Checked)
                {   // ?? ??? FIX
                    if (AdjustDir > 0)
                    {
                        if (CheckBox301.Checked && OptionButton301.Checked)
                            BasePoints.AddPoint(pPolygon.get_Point(2));
                        else
                            BasePoints.AddPoint(pPolygon.get_Point(1));
                    }

                    if (CheckBox301.Checked && OptionButton301.Checked)
                    {
                        BasePoints.AddPoint(pPolygon.get_Point(1));
                        BasePoints.AddPoint(pPolygon.get_Point(0));
                        BasePoints.AddPoint(pPolygon.get_Point(7));
                        BasePoints.AddPoint(pPolygon.get_Point(6));
                    }
                    else
                    {
                        BasePoints.AddPoint(pPolygon.get_Point(0));
                        BasePoints.AddPoint(pPolygon.get_Point(5));
                    }

                    if (AdjustDir < 0)
                    {
                        if (CheckBox301.Checked && OptionButton301.Checked)
                            BasePoints.AddPoint(pPolygon.get_Point(5));
                        else
                            BasePoints.AddPoint(pPolygon.get_Point(4));
                    }
                }
                else
                {
                    BasePoints.AddPoint(KK.FromPoint);
                    BasePoints.AddPoint(KK.ToPoint);
                }
            }

            ZNR_Poly.AddPoint(pPolygon.get_Point(0));
            ZNR_Poly.AddPoint(pPolygon.get_Point(1));

            if (CheckBox301.Checked && OptionButton301.Checked)
            {
                ZNR_Poly.AddPoint(pPolygon.get_Point(2));
                ZNR_Poly.AddPoint(KK.FromPoint);
                ZNR_Poly.AddPoint(KK.ToPoint);
                ZNR_Poly.AddPoint(pPolygon.get_Point(pPolygon.PointCount - 3));
            }
            else
            {
                ZNR_Poly.AddPoint(KK.FromPoint);
                ZNR_Poly.AddPoint(KK.ToPoint);
            }

            ZNR_Poly.AddPoint(pPolygon.get_Point(pPolygon.PointCount - 2));
            ZNR_Poly.AddPoint(pPolygon.get_Point(pPolygon.PointCount - 1));

            ITopologicalOperator2 pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)ZNR_Poly;
            pTopoOper.IsKnownSimple_2 = false;
            pTopoOper.Simplify();

            TurnArea = CreateTurnArea(3.6 * PANS_OPS_DataBase.dpWind_Speed.Value, TurnR, fTASl, ref DepDir, ref OutAzt, TurnDir, BasePoints);

            //Functions.DrawPolygon(TurnArea, Functions.RGB(50, 170, 200));

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

            if (CheckBox301.Checked && OptionButton301.Checked)
            {
                if (TurnDir > 0)
                    TurnArea.AddFirstPoint(pPolygon.get_Point(pPolygon.PointCount - 2));
                else
                    TurnArea.AddFirstPoint(pPolygon.get_Point(1));
            }

            IPoint ptCurr = TurnArea.get_Point(TurnArea.PointCount - 1);
            double AztCurr = OutAzt + PANS_OPS_DataBase.dpafTrn_OSplay.Value * TurnDir;

            IPointCollection TmpPoly;
            if (OptionButton301.Checked)
                TmpPoly = ZNR_Poly;
            else
                TmpPoly = pFIXPoly;

            double maxDist = 0.0, currDist;
            int n = TmpPoly.PointCount, iMax = 0;

            for (int i = 0; i < n; i++)
            {
                currDist = Functions.Point2LineDistancePrj(TmpPoly.Point[i], ptCurr, AztCurr);
                if ((maxDist < currDist))
                {
                    iMax = i;
                    maxDist = currDist;
                }
            }

            ptTmp = TmpPoly.Point[iMax];
            TurnArea.AddFirstPoint(ptTmp);

            TmpPoly = new ESRI.ArcGIS.Geometry.Polygon();
            TmpPoly.AddPoint(ptTmp);
            TmpPoly.AddPoint(ptCurr);

            // ============= To Course ============

            IPoint ptTmp1;
            currDist = Functions.CircleVectorIntersect(ptTmp, GlobalVars.RModel + GlobalVars.RModel, ptCurr, AztCurr, out ptTmp1);

            ptCurr = Functions.PointAlongPlane(ptCurr, OutAzt - PANS_OPS_DataBase.dpafTrn_OSplay.Value * TurnDir, currDist);
            TmpPoly.AddPoint(ptCurr);
            TurnArea.AddPoint(ptCurr);

            ptCurr = Functions.PointAlongPlane(ptTmp, OutAzt + PANS_OPS_DataBase.dpafTrn_ISplay.Value * TurnDir, GlobalVars.RModel + GlobalVars.RModel);
            TmpPoly.AddPoint(ptCurr);
            TurnArea.AddPoint(ptCurr);

            //
            double fTmp = Functions.ReturnAngleInDegrees(TurnArea.get_Point(0), ptCurr);

            pTopoOper = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(TmpPoly));
            pTopoOper.IsKnownSimple_2 = false;
            pTopoOper.Simplify();

            pTopoOper = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(BasePoints));
            pTopoOper.IsKnownSimple_2 = false;
            pTopoOper.Simplify();

            if (NativeMethods.Modulus((OutAzt - DepDir) * TurnDir, 360.0) > 90.0)
                BasePoints = (IPointCollection)pTopoOper.Union(TmpPoly);

            IClone pSource = (ESRI.ArcGIS.esriSystem.IClone)TurnArea;
            TmpPoly = (ESRI.ArcGIS.Geometry.IPointCollection)pSource.Clone();

            pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)TmpPoly;
            pTopoOper.IsKnownSimple_2 = false;
            pTopoOper.Simplify();

            BaseArea = Functions.RemoveAgnails(Functions.RemoveHoles(pTopoOper.Union(BasePoints) as IPolygon));

            if (OptionButton301.Checked)
            {
                pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)BaseArea;
                BaseArea = (IPointCollection)pTopoOper.Difference(ZNR_Poly);
            }

            IPointCollection DrawPoly = Functions.PolygonIntersection(pCircle, BaseArea) as IPointCollection;

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

            GlobalVars.KKElem = Functions.DrawPolyline(KK, GlobalVars.KKElemColor, 1.0, false);
            GlobalVars.K1K1Elem = Functions.DrawPolyline(K1K1, GlobalVars.K1K1ElemColor, 1.0, false);

            GlobalVars.TurnAreaElem = Functions.DrawPolygon(DrawPoly, GlobalVars.TurnAreaElemColor, esriSimpleFillStyle.esriSFSNull, false);
            GlobalVars.NomTrackElem = Functions.DrawPolyline(TracPoly, GlobalVars.NomTrackElemColor, 1.0, false);

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

            //Functions.RefreshCommandBar(_mTool, 122);
            return 0;
        }

        private void TextBox602_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char KeyAscii = eventArgs.KeyChar;
            if (KeyAscii == 13)
                TextBox602_Validating(TextBox602, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxInteger(ref KeyAscii);

            eventArgs.KeyChar = KeyAscii;
            if (KeyAscii == 0)
                eventArgs.Handled = true;
        }

        private void TextBox602_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            if (ComboBox603.SelectedIndex == 0 || !OptionButton602.Checked)
            {
                double fTmp;
                if (!double.TryParse(TextBox602.Text, out fTmp))
                {
                    if (double.TryParse(TextBox602.Tag.ToString(), out fTmp))
                        TextBox602.Text = TextBox602.Tag.ToString();
                    return;
                }

                if (MultiPage1.SelectedIndex < 5 || TextBox602.Tag.ToString() == TextBox602.Text)
                    return;

                DirCourse = Functions.Azt2Dir(DER.pPtGeo[eRWY.PtDER], fTmp + GlobalVars.CurrADHP.MagVar);
            }

            if (OptionButton602.Checked)
            {
                if (UpdateToNavCurs(DirCourse) == -1)
                {
                    TextBox602.Text = TextBox602.Tag.ToString();
                    eventArgs.Cancel = true;
                }
                else
                {
                    TextBox602.Tag = TextBox602.Text;
                    eventArgs.Cancel = false;
                }
            }
            else if (OptionButton601.Checked)
            {
                UpdateToCourse(DirCourse);
                TextBox602.Tag = TextBox602.Text;
                eventArgs.Cancel = false;
            }
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

                if (NewR < RMin)
                {
                    NewR = RMin;
                    TextBox003.Text = Functions.ConvertDistance(NewR, eRoundMode.CEIL).ToString();
                }

                TextBox003.Tag = TextBox003.Text;
                GlobalVars.RModel = NewR;

                double NewH = NewR * PANS_OPS_DataBase.dpPDG_Nom.Value;
                if (NewH < 600.0)
                    NewH = 600.0;

                //TextBox007.Text = Functions.ConvertHeight(NewH + DER.pPtGeo[eRWY.PtDER].Z, eRoundMode.rmNERAEST).ToString();//GlobalVars.CurrADHP.Elev + 
                //TextBox007.Text = Functions.ConvertHeight(NewH + PANS_OPS_DataBase.dpH_abv_DER.Value + DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.rmNERAEST).ToString();

                //         If NewR > 60000# Then NewR = 60000#

                Functions.DeleteGraphicsElement(GlobalVars.pCircleElem);
                Functions.DeleteGraphicsElement(GlobalVars.CLElem);

                //         If R = NewR Then Return
                pCircle = Functions.CreatePrjCircle(ptCenter, GlobalVars.RModel);

                // =====================================================================
                ISimpleFillSymbol pEmptyFillSym = new SimpleFillSymbol();
                IRgbColor pRGB = new RgbColor();
                pRGB.RGB = Functions.RGB(255, 0, 0);
                ISimpleLineSymbol pLineSym = new SimpleLineSymbol();
                pLineSym.Color = pRGB;
                pLineSym.Width = 2.0;

                pEmptyFillSym.Style = esriSimpleFillStyle.esriSFSNull;
                pEmptyFillSym.Outline = pLineSym; // pRedLineSymbol

                GlobalVars.pCircleElem = Functions.DrawPolygonSFS(pCircle as IPolygon, pEmptyFillSym, false);
                if (GlobalVars.ButtonControl1State)
                {
                    pGraphics.AddElement(GlobalVars.pCircleElem, 0);
                    GlobalVars.pCircleElem.Locked = true;
                }
                // =====================================================================
                IPolyline pLine = (ESRI.ArcGIS.Geometry.IPolyline)(new Polyline());
                pLine.FromPoint = DER.pPtPrj[eRWY.PtDER];
                pLine.ToPoint = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], RWYDir, GlobalVars.RModel);

                ITopologicalOperator pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator)pCircle;
                pLine = (ESRI.ArcGIS.Geometry.IPolyline)pTopo.Intersect(pLine, esriGeometryDimension.esriGeometry1Dimension);
                // =====================================================================
                //         Set pRGB = New RgbColor
                pRGB.RGB = 0; // RGB(178, 138, 64)
                              //         Set pLineSym = New SimpleLineSymbol
                pLineSym.Color = pRGB;
                pLineSym.Style = esriSimpleLineStyle.esriSLSDash;
                pLineSym.Width = 1.0;

                GlobalVars.CLElem = Functions.DrawPolylineSFS(pLine, pLineSym, false);

                if (GlobalVars.ButtonControl7State)
                {
                    pGraphics.AddElement(GlobalVars.CLElem, 0);
                    GlobalVars.CLElem.Locked = true;
                }

                if (GlobalVars.ButtonControl1State || GlobalVars.ButtonControl7State)
                    GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                // =====================================================================
                //Functions.RefreshCommandBar(_mTool, 65);

                double MaxDist = DBModule.GetObstListInPoly(out oFullList, ptCenter, GlobalVars.RModel, DER.pPtGeo[eRWY.PtDER].Z);    // = Functions.MaxObstacleDist(oFullList, ptCenter);

                Label007.Text = Resources.str15261 + Functions.ConvertDistance(GlobalVars.RModel, eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit + ":";
                TextBox005.Text = oFullList.Obstacles.Length.ToString();
                TextBox006.Text = Functions.ConvertDistance(MaxDist, eRoundMode.FLOOR).ToString();
            }
            else if (double.TryParse(TextBox003.Tag.ToString(), out fTmp))
                TextBox003.Text = TextBox003.Tag.ToString();
            else
                TextBox003.Text = Functions.ConvertDistance(GlobalVars.RModel, eRoundMode.CEIL).ToString();
        }

        private void TextBox502_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char eventChar = eventArgs.KeyChar;

            if (eventChar == 13)
                TextBox502_Validating(TextBox502, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox502.Text);

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        private void TextBox502_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            double iniH, fTASl, Range = (hMaxTurn - PANS_OPS_DataBase.dpH_abv_DER.Value) / CurrPDG;
            // ===============================
            if (double.TryParse(TextBox502.Text, out iniH) && OptionButton301.Checked)
            {
                if (TextBox502.Tag.ToString() == TextBox502.Text)
                    return;

                iniH = Functions.DeConvertHeight(iniH);
                //         iniH = iniH + ptThrPrj.Z - DER.pPtPrj(ptEnd).Z

                if (iniH < hMinTurn)
                    iniH = hMinTurn;

                if (iniH > hMaxTurn)
                    iniH = hMaxTurn;

                ObstacleContainer TmpInList;

                if (Range == GlobalVars.RModel)
                {
                    int n = oZNRList.Obstacles.Length;
                    TmpInList.Obstacles = new Obstacle[n];
                    System.Array.Copy(oZNRList.Obstacles, TmpInList.Obstacles, n);

                    n = oZNRList.Parts.Length;
                    TmpInList.Parts = new ObstacleData[n];
                    System.Array.Copy(oZNRList.Parts, TmpInList.Parts, n);
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
                ReportsFrm.FillPage4(TmpInList, OptionButton301.Checked);
                // ============================================================
                fTASl = Functions.IAS2TAS(fIAS, hTurn, GlobalVars.CurrADHP.ISAtC);
                double VTotal = fTASl + GlobalVars.CurrADHP.WindSpeed;
                double Ls = VTotal * PANS_OPS_DataBase.dpT_TechToleranc.Value * 0.277777777777778;

                IClone pSource = (ESRI.ArcGIS.esriSystem.IClone)pPolygon;
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
                KK = pTopo.Intersect(pLine, esriGeometryDimension.esriGeometry1Dimension) as IPolyline;
                if (Functions.SideDef(KK.ToPoint, DepDir, KK.FromPoint) < 0)
                    KK.ReverseOrientation();

                // ==========================
                TurnFixPnt = new ESRI.ArcGIS.Geometry.Point();
                IConstructPoint pConstruct = (ESRI.ArcGIS.Geometry.IConstructPoint)TurnFixPnt;
                pConstruct.ConstructAngleIntersection(DER.pPtPrj[eRWY.PtDER], GlobalVars.DegToRadValue * DepDir, KK.ToPoint, GlobalVars.DegToRadValue * (DepDir + 90.0));

                TurnFixPnt.Z = hTurn;
                TurnFixPnt.M = DepDir;

                ptTmp = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir, L0 + Ls);
                pLine.RemovePoints(0, 2);
                pLine.AddPoint(Functions.PointAlongPlane(ptTmp, DepDir - 90.0, GlobalVars.RModel));
                pLine.AddPoint(Functions.PointAlongPlane(ptTmp, DepDir + 90.0, GlobalVars.RModel));

                K1K1 = pTopo.Intersect(pLine, esriGeometryDimension.esriGeometry1Dimension) as IPolyline;

                if (Functions.SideDef(K1K1.ToPoint, DepDir, K1K1.FromPoint) < 0)
                    K1K1.ReverseOrientation();

                // ============================================================
                TextBox502.Text = Functions.ConvertHeight(newH, eRoundMode.NEAREST).ToString();
                TextBox502.Tag = TextBox502.Text;

                TextBox503.Text = Functions.ConvertHeight(newH + DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.NEAREST).ToString();
                TextBox503.Tag = TextBox503.Text;
                // ============================================================
                IPointCollection pPoly = new Polyline();
                pPoly.AddPoint(DER.pPtPrj[eRWY.PtDER]);
                pPoly.AddPoint(TurnFixPnt);

                Functions.DeleteGraphicsElement(GlobalVars.StrTrackElem);
                GlobalVars.StrTrackElem = Functions.DrawPolyline(pPoly, GlobalVars.StrTrackElemColor, 1.0, false);

                Functions.DeleteGraphicsElement(GlobalVars.FIXElem);

                if ((GlobalVars.FIXElem != null) && GlobalVars.ButtonControl8State)
                {
                    if (GlobalVars.FIXElem is IGroupElement)
                    {
                        IGroupElement pGroupElement = (ESRI.ArcGIS.Carto.IGroupElement)GlobalVars.FIXElem;
                        for (int i = 0; i < pGroupElement.ElementCount; i++)
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

                if (GlobalVars.ButtonControl5State)
                {
                    pGraphics.AddElement(GlobalVars.StrTrackElem, 0);
                    GlobalVars.StrTrackElem.Locked = true;
                }

                //Functions.RefreshCommandBar(_mTool, 128);
            }

            fTASl = Functions.IAS2TAS(fIAS, TurnFixPnt.Z, GlobalVars.CurrADHP.ISAtC);
            Text502.Text = Functions.ConvertSpeed(fIAS, eRoundMode.NEAREST).ToString();
            Text503.Text = Functions.ConvertSpeed(fTASl, eRoundMode.NEAREST).ToString();
            Text504.Text = Functions.ConvertDistance(PANS_OPS_DataBase.dpT_TechToleranc.Value * (fTASl + GlobalVars.CurrADHP.WindSpeed) / 3.6).ToString();

            double Rv = 6355.0 * System.Math.Tan(GlobalVars.DegToRadValue * fBankAngle) / (GlobalVars.PI * fTASl);
            if (Rv > 3.0)
                Rv = 3.0;

            Text505.Text = System.Math.Round(Rv, 2).ToString();

            if (Rv > 0.0)
            {
                double r0 = fTASl / (0.02 * GlobalVars.PI * Rv);
                Text506.Text = Functions.ConvertDistance(r0, eRoundMode.NEAREST).ToString();
            }
            else
                Text506.Text = "-";

            double E = 25.0 * GlobalVars.CurrADHP.WindSpeed / Rv;
            Text507.Text = Functions.ConvertDistance(E, eRoundMode.NEAREST).ToString();

            Text508.Text = Functions.ConvertHeight(TurnFixPnt.Z, eRoundMode.NEAREST).ToString();

            if (OptionButton301.Checked)
                Text501.Text = Functions.ConvertDistance(Range, eRoundMode.NEAREST).ToString();
            else
            {
                IProximityOperator pProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)KK;
                Text501.Text = Functions.ConvertDistance(pProxi.ReturnDistance(DER.pPtPrj[eRWY.PtDER]), eRoundMode.NEAREST).ToString();

                double L1 = Functions.ReturnDistanceInMeters(DER.pPtPrj[eRWY.PtDER], TurnFixPnt);
                double TurnFixPntAltitude = CurrPDG * L1 + DER.pPtPrj[eRWY.PtDER].Z + PANS_OPS_DataBase.dpH_abv_DER.Value;
                Text509.Text = Functions.ConvertHeight(TurnFixPntAltitude, eRoundMode.NEAREST).ToString();

                Text510.Text = Functions.ConvertDistance(pProxi.ReturnDistance(KKFixMax), eRoundMode.NEAREST).ToString();
            }
        }

        private void TextBox601_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char eventChar = eventArgs.KeyChar;

            if (eventChar == 13)
                TextBox601_Validating(TextBox601, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox601.Text);

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        private void TextBox601_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            double fTmp;
            if (!double.TryParse(TextBox601.Text, out fTmp))
            {
                if (double.TryParse(TextBox601.Tag.ToString(), out fTmp))
                    TextBox601.Text = TextBox601.Tag.ToString();
                else
                {
                    fTmp = 0.0;
                    TextBox601.Text = "0";
                }
            }

            if (TextBox601.Tag.ToString() == TextBox601.Text)
                return;

            if (fTmp > 2.5 * PANS_OPS_DataBase.dpT_TechToleranc.Value)
            {
                TextBox601.Text = (2.5 * PANS_OPS_DataBase.dpT_TechToleranc.Value).ToString();
                TextBox601.Tag = TextBox601.Text;
            }

            TextBox603.Tag = "a";
            TextBox603_Validating(TextBox603, new System.ComponentModel.CancelEventArgs());
            TextBox601.Tag = TextBox601.Text;
        }

        private void TextBox501_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            if (OptionButton301.Checked)
                return;

            char eventChar = eventArgs.KeyChar;

            if (eventChar == 13)
                TextBox501_Validating(TextBox501, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox501.Text);

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        private void TextBox501_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            IPointCollection pPolyClone;
            IPointCollection pPoly;
            IPointCollection pSect1;
            IPointCollection pLine;
            IPointCollection pSect0 = null;

            IClone Clone;

            ITopologicalOperator2 pTopo;
            IConstructPoint pConstruct;
            IPolyline pCutter;

            IPoint pt1, pt2;

            double VTotal;
            double hTurn;
            double fTASl;
            double hFix;
            double fDir;
            double dMin;
            double dMax;
            double fTmp;
            double Ls;
            double d0;
            double d;

            double DepDistMax = GlobalVars.RModel;
            double DepDistMin = 0.0;
            double InterToler = 0;
            double fDis = 0;

            int I;
            int N;

            if (!double.TryParse(TextBox501.Text, out fDir))
                return;

            int K = ComboBox501.SelectedIndex;

            if (TurnInterDat[K].TypeCode == eNavaidType.DME)
            {
                fDir = Functions.DeConvertDistance(fDir);
                fTmp = fDir;

                N = TurnInterDat[K].ValMin.GetUpperBound(0);
                if (Option501.Checked || (N == 0))
                {
                    dMin = TurnInterDat[K].ValMin[0];
                    dMax = TurnInterDat[K].ValMax[0];
                }
                else
                {
                    dMin = TurnInterDat[K].ValMin[1];
                    dMax = TurnInterDat[K].ValMax[1];
                }
                if (fTmp < dMin)
                    fTmp = dMin;

                if (fTmp > dMax)
                    fTmp = dMax;

                if (fTmp != fDir)
                {
                    fDir = fTmp;
                    TextBox501.Text = Functions.ConvertDistance(fDir, eRoundMode.NEAREST).ToString();
                }
            }
            else
            {
                fDir += GlobalVars.CurrADHP.MagVar;
                if (TurnInterDat[K].TypeCode == eNavaidType.VOR)
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

                if (!Functions.AngleInSector(fDir, TurnInterDat[K].ValMin[0], TurnInterDat[K].ValMax[0]))
                {
                    double da0 = Functions.SubtractAngles(fDir, TurnInterDat[K].ValMin[0]);
                    double da1 = Functions.SubtractAngles(fDir, TurnInterDat[K].ValMax[0]);

                    if (da0 < da1)
                        fDir = TurnInterDat[K].ValMin[0];
                    else
                        fDir = TurnInterDat[K].ValMax[0];

                    fTmp = fDir;
                    if (TurnInterDat[K].TypeCode == eNavaidType.NDB)
                        fTmp = fTmp + 180.0;

                    TextBox501.Text = NativeMethods.Modulus(fTmp - GlobalVars.CurrADHP.MagVar).ToString();
                }
            }

            IGroupElement pGroupElem = ((ESRI.ArcGIS.Carto.IGroupElement)(new GroupElement()));

            Functions.DeleteGraphicsElement(GlobalVars.FIXElem);

            if (GlobalVars.FIXElem is IGroupElement)
                pGroupElem.AddElement((GlobalVars.FIXElem as IGroupElement).Element[0]);
            else
                pGroupElem.AddElement(GlobalVars.FIXElem);

            switch (TurnInterDat[K].TypeCode)
            {
                case Departure.eNavaidType.VOR:
                case Departure.eNavaidType.NDB:
                    fDir = Functions.Azt2Dir(DER.pPtGeo[eRWY.PtDER], fDir);

                    pSect0 = new Polyline();
                    pt1 = Functions.PointAlongPlane(TurnInterDat[K].pPtPrj, fDir + InterToler, fDis);
                    pt2 = Functions.PointAlongPlane(TurnInterDat[K].pPtPrj, fDir - InterToler, fDis);
                    TurnFixPnt = new ESRI.ArcGIS.Geometry.Point();
                    pConstruct = ((ESRI.ArcGIS.Geometry.IConstructPoint)(TurnFixPnt));

                    pConstruct.ConstructAngleIntersection(TurnInterDat[K].pPtPrj, GlobalVars.DegToRadValue * fDir, DER.pPtPrj[eRWY.PtDER], GlobalVars.DegToRadValue * DepDir);
                    pSect0.AddPoint(pt1);
                    pSect0.AddPoint(TurnInterDat[K].pPtPrj);
                    pSect0.AddPoint(pt2);

                    pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(pCircle));
                    pSect0 = pTopo.Intersect(pSect0, esriGeometryDimension.esriGeometry1Dimension) as IPointCollection;
                    pGroupElem.AddElement(Functions.DrawPolyline(pSect0, Functions.RGB(0, 0, 255), 1, false));
                    break;
                case Departure.eNavaidType.DME:
                    if ((TurnInterDat[K].ValCnt < 0) || (Option502.Enabled && Option502.Checked))
                        Functions.CircleVectorIntersect(TurnInterDat[K].pPtPrj, fDir, DER.pPtPrj[eRWY.PtDER], DepDir, out TurnFixPnt);
                    else
                        Functions.CircleVectorIntersect(TurnInterDat[K].pPtPrj, fDir, DER.pPtPrj[eRWY.PtDER], DepDir + 180.0, out TurnFixPnt);

                    fDis = Functions.Point2LineDistancePrj(TurnFixPnt, DER.pPtPrj[eRWY.PtDER], DepDir + 90.0);
                    hFix = fDis * PANS_OPS_DataBase.dpOv_Nav_PDG.Value + PANS_OPS_DataBase.dpH_abv_DER.Value - TurnInterDat[K].pPtPrj.Z + DER.pPtPrj[eRWY.PtDER].Z;

                    d0 = System.Math.Sqrt(fDir * fDir + hFix * hFix) * Navaids_DataBase.DME.ErrorScalingUp + Navaids_DataBase.DME.MinimalError;

                    d = fDir + d0;
                    pSect0 = (IPointCollection)Functions.CreatePrjCircle(TurnInterDat[K].pPtPrj, d);

                    d = fDir - d0;
                    pSect1 = (IPointCollection)Functions.CreatePrjCircle(TurnInterDat[K].pPtPrj, d);

                    pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(pSect0));
                    pPoly = pTopo.Difference(pSect1) as IPointCollection;

                    pCutter = ((ESRI.ArcGIS.Geometry.IPolyline)(new Polyline()));
                    pCutter.FromPoint = Functions.PointAlongPlane(TurnInterDat[K].pPtPrj, DepDir - 90.0, fDir + fDir);
                    pCutter.ToPoint = Functions.PointAlongPlane(TurnInterDat[K].pPtPrj, DepDir + 90.0, fDir + fDir);

                    if (Functions.SideDef(pCutter.FromPoint, DepDir, pCutter.ToPoint) > 0)
                        pCutter.ReverseOrientation();

                    pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(pPoly));

                    if ((TurnInterDat[K].ValCnt < 0) || (Option502.Enabled && Option502.Checked))
                        pTopo.Cut(pCutter, out pSect1, out pSect0);

                    else
                        pTopo.Cut(pCutter, out pSect0, out pSect1);

                    pGroupElem.AddElement(Functions.DrawPolygon(pSect0, Functions.RGB(0, 0, 255), esriSimpleFillStyle.esriSFSNull, false));
                    DepDistMax = (hMaxTurn - PANS_OPS_DataBase.dpH_abv_DER.Value) / CurrPDG + 10.0;
                    DepDistMin = (hMinTurn - PANS_OPS_DataBase.dpH_abv_DER.Value) / CurrPDG - 10.0;
                    break;
            }

            Clone = ((ESRI.ArcGIS.esriSystem.IClone)(pPolygon));
            pPolyClone = ((ESRI.ArcGIS.Geometry.IPointCollection)(Clone.Clone()));
            pFIXPoly = ((ESRI.ArcGIS.Geometry.IPointCollection)(Clone.Clone()));

            pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(pPolyClone));
            pTopo.IsKnownSimple_2 = false;
            pTopo.Simplify();

            pLine = pTopo.Intersect(pSect0, esriGeometryDimension.esriGeometry1Dimension) as IPointCollection;

            if (pLine.PointCount == 0 && TurnInterDat[K].TypeCode == eNavaidType.DME)
                pLine = pTopo.Intersect(pSect0, esriGeometryDimension.esriGeometry2Dimension) as IPointCollection;

            dMin = 5 * GlobalVars.RModel;
            dMax = -dMin;

            pt1 = pt2 = null;

            for (I = 0; I < pLine.PointCount; I++)
            {
                d = Functions.Point2LineDistancePrj(DER.pPtPrj[eRWY.PtDER], pLine.get_Point(I), DepDir + 90.0);
                if ((d >= DepDistMin) && (d < dMin))
                {
                    dMin = d;
                    pt1 = pLine.get_Point(I);
                }
                if ((d <= DepDistMax) && (d > dMax))
                {
                    dMax = d;
                    pt2 = pLine.get_Point(I);
                }
            }

            if (dMax <= dMin)
            {
                MessageBox.Show(Resources.str15265, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextBox501.Text = TextBox501.Tag.ToString();
                TextBox501_Validating(TextBox501, new System.ComponentModel.CancelEventArgs());
                return;
            }

            pLine = new Polyline();

            pLine.AddPoint(Functions.PointAlongPlane(pt1, DepDir + 90.0, GlobalVars.RModel));
            pLine.AddPoint(Functions.PointAlongPlane(pt1, DepDir - 90.0, GlobalVars.RModel));

            KK = pTopo.Intersect(pLine, esriGeometryDimension.esriGeometry1Dimension) as IPolyline;

            if (Functions.SideDef(DER.pPtPrj[eRWY.PtDER], DepDir, KK.FromPoint) < 0)
                KK.ReverseOrientation();

            // ==========================
            fDis = Functions.Point2LineDistancePrj(DER.pPtPrj[eRWY.PtDER], pt1, DepDir + 90.0);

            d = Functions.Point2LineDistancePrj(DER.pPtPrj[eRWY.PtDER], TurnFixPnt, DepDir + 90.0);
            hTurn = d * PANS_OPS_DataBase.dpOv_Nav_PDG.Value + PANS_OPS_DataBase.dpOIS_abv_DER.Value + DER.pPtPrj[eRWY.PtDER].Z;

            TextBox502.Text = Functions.ConvertHeight(PANS_OPS_DataBase.dpOIS_abv_DER.Value + fDis * CurrPDG, eRoundMode.NEAREST).ToString();
            TextBox503.Text = Functions.ConvertHeight(PANS_OPS_DataBase.dpOIS_abv_DER.Value + fDis * CurrPDG + DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.NEAREST).ToString();

            TurnFixPnt.Z = hTurn;
            TurnFixPnt.M = DepDir;

            Functions.CutPoly(ref pFIXPoly, pLine, 1);

            pLine.RemovePoints(0, pLine.PointCount);
            pLine.AddPoint(Functions.PointAlongPlane(pt2, DepDir + 90.0, GlobalVars.RModel));
            pLine.AddPoint(Functions.PointAlongPlane(pt2, DepDir - 90.0, GlobalVars.RModel));

            KKFixMax = pTopo.Intersect(pLine, esriGeometryDimension.esriGeometry1Dimension) as IPolyline;
            if (Functions.SideDef(DER.pPtPrj[eRWY.PtDER], DepDir, KKFixMax.FromPoint) < 0)
                KKFixMax.ReverseOrientation();

            Functions.CutPoly(ref pFIXPoly, pLine, -1);
            // ====
            fTASl = Functions.IAS2TAS(fIAS, (TurnFixPnt.Z), GlobalVars.CurrADHP.ISAtC);
            VTotal = fTASl + GlobalVars.CurrADHP.WindSpeed;
            Ls = VTotal * PANS_OPS_DataBase.dpT_TechToleranc.Value * 0.277777777777778;

            d0 = Functions.Point2LineDistancePrj(DER.pPtPrj[eRWY.PtDER], KKFixMax.FromPoint, DepDir + 90.0);
            pt1 = Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir, d0 + Ls);

            pLine.RemovePoints(0, pLine.PointCount);

            pLine.AddPoint(Functions.PointAlongPlane(pt1, DepDir + 90.0, GlobalVars.RModel));
            pLine.AddPoint(Functions.PointAlongPlane(pt1, DepDir - 90.0, GlobalVars.RModel));

            K1K1 = pTopo.Intersect(pLine, esriGeometryDimension.esriGeometry1Dimension) as IPolyline;

            if (Functions.SideDef(K1K1.ToPoint, DepDir, K1K1.FromPoint) < 0)
                K1K1.ReverseOrientation();

            // ==========================
            pGroupElem.AddElement(Functions.DrawPolygon(pFIXPoly, Functions.RGB(255, 255, 0), esriSimpleFillStyle.esriSFSNull, false));
            pGroupElem.AddElement(Functions.DrawPoint(TurnFixPnt, 255, false));
            GlobalVars.FIXElem = ((ESRI.ArcGIS.Carto.IElement)(pGroupElem));

            pPoly = new Polyline();
            pPoly.AddPoint(DER.pPtPrj[eRWY.PtDER]);
            pPoly.AddPoint(TurnFixPnt);

            Functions.DeleteGraphicsElement(GlobalVars.StrTrackElem);
            GlobalVars.StrTrackElem = Functions.DrawPolyline(pPoly, GlobalVars.StrTrackElemColor, 1.0, false);

            if (GlobalVars.ButtonControl8State)
                pGraphics.AddElement(pGroupElem as IElement, 0);

            if (GlobalVars.ButtonControl5State)
            {
                pGraphics.AddElement(GlobalVars.StrTrackElem, 0);
                GlobalVars.StrTrackElem.Locked = true;
            }

            GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            //Functions.RefreshCommandBar(_mTool, 128);

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

            ReportsFrm.FillPage4(TmpInList, OptionButton301.Checked);
            // ==========================================================================
            TextBox501.Tag = TextBox501.Text;
            TextBox502_Validating(TextBox502, new System.ComponentModel.CancelEventArgs());
        }

        private int CalcNewStraightAreaWithFixedLength(ref double NewPDG, ref double NewTAPDG)
        {
            Functions.DeleteGraphicsElement(GlobalVars.FIXElem);
            GlobalVars.FIXElem = null;

            int i, n, k = ComboBox501.SelectedIndex; ;
            double Intersect = 0.0;

            if (OptionButton302.Checked)
            {
                Intersect = double.Parse(TextBox501.Text);

                if (TurnInterDat[k].TypeCode == eNavaidType.NDB)
                    Intersect += 180.0;
                else if (TurnInterDat[k].TypeCode == eNavaidType.DME)
                    Intersect = Functions.DeConvertDistance(Intersect);
            }

            double NewHMinTurn, NewHMaxTurn, NewCurrPDG;

            do
            {
                double NewHTurn;
                double DepDist = Functions.Point2LineDistancePrj(DER.pPtPrj[eRWY.PtDER], KK.FromPoint, DepDir + 90.0);
                Interval mIntr = CalcLocalRange(NewPDG, out NewCurrPDG, TrackAdjust, true);
                if (NewCurrPDG < MinCurrPDG)
                    NewCurrPDG = MinCurrPDG;

                NewHMinTurn = System.Math.Round(mIntr.Left * NewCurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value - 0.4999);
                NewHMaxTurn = System.Math.Round(mIntr.Right * NewCurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value + 0.4999);
                NewHTurn = System.Math.Round(DepDist * NewCurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value + 0.49);

                if (!((NewHTurn > NewHMaxTurn) || (NewHTurn < NewHMinTurn)))
                {
                    if (OptionButton302.Checked)
                    {
                        NavaidType[] NavDatL = Functions.GetValidNavs(pPolygon as IPolygon, DER, DepDir, NewHMinTurn, NewHMaxTurn, NewCurrPDG);
                        n = NavDatL.Length;
                        int j = -1;
                        for (i = 0; i < n; i++)
                            if ((NavDatL[i].Name == TurnInterDat[k].Name) && (NavDatL[i].TypeCode == TurnInterDat[k].TypeCode))
                            {
                                j = i;
                                break;
                            }

                        if (j >= 0)
                        {
                            if (NavDatL[j].TypeCode == eNavaidType.VOR || NavDatL[j].TypeCode == eNavaidType.TACAN || NavDatL[j].TypeCode == eNavaidType.NDB)
                            {
                                //if (NavDatL[j].TypeCode == eNavaidType.CodeNDB)
                                //    Intersect += 180.0;

                                if (!Functions.AngleInSector(Intersect + GlobalVars.CurrADHP.MagVar, NavDatL[j].ValMin[0], NavDatL[j].ValMax[0]))
                                    goto ContinueL;
                            }
                            else if (NavDatL[j].TypeCode == eNavaidType.DME)
                            {
                                //Intersect = Functions.DeConvertDistance(Intersect);

                                if (Option502.Enabled && Option502.Checked)
                                {
                                    if (Intersect < NavDatL[j].ValMin[1] || Intersect > NavDatL[j].ValMax[1])
                                        goto ContinueL;
                                }
                                else if (Intersect < NavDatL[j].ValMin[0] || Intersect > NavDatL[j].ValMax[0])
                                    goto ContinueL;
                            }

                            TextBox501.Tag = "a";
                            TextBox501_Validating(TextBox501, new System.ComponentModel.CancelEventArgs());
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
                    MessageBox.Show(Resources.str15266, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                }
            }
            while (true);

            hMaxTurn = NewHMaxTurn;
            hMinTurn = NewHMinTurn;
            CurrPDG = NewCurrPDG;

            // ===================================================================
            if (OptionButton603.Checked)
            {
                if (UpdateToFix() == 1)
                    SecondArea(TurnDir, BaseArea);
            }
            else if (OptionButton602.Checked)
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

            if (OptionButton301.Checked)
                pDistPoly = (ESRI.ArcGIS.Geometry.IGeometry)ZNR_Poly;
            else
                pDistPoly = KK;

            if (OptionButton601.Checked)
            {
                IPoint ptTmp = MPtCollection.Point[MPtCollection.PointCount - 1];
                Functions.CircleVectorIntersect(GlobalVars.CurrADHP.pPtPrj, GlobalVars.RModel, ptTmp, ptTmp.M, out ptCnt);
            }
            else if (OptionButton602.Checked)
                ptCnt = TerFixPnt;
            else
                ptCnt = TurnDirector.pPtPrj;

            //if (OptionButton602.Checked || (TurnWPT.TypeCode > eNavaidType.CodeNONE ))
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
            if (!double.TryParse(TextBox905.Text, out dBuffer))
                dBuffer = -1.0;
            else
                dBuffer = Functions.DeConvertDistance(dBuffer);

            if (dBuffer < PANS_OPS_DataBase.dpTermMinBuffer.Value)
            {
                dBuffer = PANS_OPS_DataBase.dpTermMinBuffer.Value;
                if (!OptionButton601.Checked)
                    TextBox905.Text = Functions.ConvertDistance(dBuffer, eRoundMode.NEAREST).ToString();
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

            // =========================================================================================================

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
                {
                    double CurrReq = oTrnAreaList.Parts[i].Height + oTrnAreaList.Parts[i].CLShift * MOCLimit;
                    if (MaxReq < CurrReq)
                    {
                        MaxReq = CurrReq;
                        IxMaxReq = i;
                    }
                }
            }

            TextBox915.Text = Functions.ConvertHeight(MaxReq + DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.NEAREST).ToString();
            //TextBox916.Text = TextBox915.Text;
            TextBox916_Validating(TextBox915, null);

            ToolTip1.SetToolTip(TextBox915, "");
            if (IxMaxReq >= 0)
                ToolTip1.SetToolTip(TextBox915, oTrnAreaList.Obstacles[oTrnAreaList.Parts[IxMaxReq].Owner].UnicalName);

            return 0;
        }

        private void TextBox905_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char eventChar = eventArgs.KeyChar;

            if (eventChar == 13)
                TextBox905_Validating(TextBox905, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox905.Text);

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        //private void TextBox905_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        //{
        //    double fTmp;
        //    //if (OptionButton603.Checked)				return;

        //    if (!double.TryParse(TextBox905.Text, out fTmp))
        //        return;

        //    double D0 = Functions.DeConvertDistance(fTmp);
        //    if (D0 < PANS_OPS_DataBase.dpTermMinBuffer.Value)
        //    {
        //        D0 = PANS_OPS_DataBase.dpTermMinBuffer.Value;
        //        TextBox905.Text = Functions.ConvertDistance(D0, eRoundMode.rmNERAEST).ToString();
        //    }

        //    if (OptionButton901.Checked)
        //    {
        //        TextBox911.Tag = "";
        //        TextBox911_Validating(TextBox911, new System.ComponentModel.CancelEventArgs());
        //    }
        //    else if (OptionButton902.Checked)
        //    {
        //        TextBox912.Tag = "";
        //        TextBox912_Validating(TextBox912, new System.ComponentModel.CancelEventArgs());
        //    }
        //    else if (OptionButton903.Checked)
        //    {
        //        OptionButton903_CheckedChanged(OptionButton903, new System.EventArgs());
        //    }

        //    double hTurn = fZNRLen * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value;
        //    fReachedA = hTurn + D0 * TACurrPDG + DER.pPtPrj[eRWY.PtDER].Z;
        //    TextBox906.Text = Functions.ConvertHeight(fReachedA, eRoundMode.rmNERAEST).ToString();

        //    // =====================================================================================
        //    IPointCollection mPoly = (IPointCollection)Functions.CalcTrajectoryFromMultiPoint(MPtCollection);
        //    if (!OptionButton602.Checked)
        //    {
        //        if (OptionButton603.Checked)
        //            mPoly.AddPoint(TurnWPT.pPtPrj);
        //        else
        //        {
        //            IPoint pTurnComplitPt = MPtCollection.Point[MPtCollection.PointCount - 1];
        //            ITopologicalOperator2 pTopo;

        //            if (OptionButton301.Checked)
        //                pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)ZNR_Poly;
        //            else
        //                pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)KK;

        //            ESRI.ArcGIS.Geometry.Polygon pBufferPolygon = (ESRI.ArcGIS.Geometry.Polygon)pTopo.Buffer(D0);

        //            pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)pBufferPolygon;
        //            pTopo.IsKnownSimple_2 = false;
        //            pTopo.Simplify();

        //            mPoly.AddPoint(Functions.PointAlongPlane(pTurnComplitPt, (pTurnComplitPt.M), 10.0 * GlobalVars.RModel));

        //            IPolyline pPoly = (IPolyline)pTopo.Intersect(mPoly, esriGeometryDimension.esriGeometry1Dimension);

        //            if (Functions.ReturnDistanceInMeters(pPoly.FromPoint, mPoly.Point[0]) > GlobalVars.distEps)
        //                pPoly.ReverseOrientation();

        //            mPoly = (ESRI.ArcGIS.Geometry.IPointCollection)pPoly;
        //        }
        //    }
        //    else
        //        mPoly.AddPoint(TerFixPnt);

        //    NomInfoFrm.SetInfo(mPoly, hKK, TACurrPDG);
        //    // =====================================================================================
        //}

        private void TextBox905_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            if (OptionButton601.Checked) return;

            if (TextBox905.Tag != null && TextBox905.Tag.ToString() == TextBox905.Text)
                return;
            TextBox905.Tag = TextBox905.Text;

            double dBuffer;

            if (!double.TryParse(TextBox905.Text, out dBuffer))
                dBuffer = -1.0;
            else
                dBuffer = Functions.DeConvertDistance(dBuffer);

            if (dBuffer < PANS_OPS_DataBase.dpTermMinBuffer.Value)
            {
                dBuffer = PANS_OPS_DataBase.dpTermMinBuffer.Value;
                TextBox905.Text = Functions.ConvertDistance(dBuffer, eRoundMode.NEAREST).ToString();
            }
            else if (dBuffer > TurnAreaMaxd0)
            {
                dBuffer = TurnAreaMaxd0;
                TextBox905.Text = Functions.ConvertDistance(dBuffer, eRoundMode.NEAREST).ToString();
            }

            if (OptionButton901.Checked)
            {
                TextBox911.Tag = "";
                TextBox911_Validating(TextBox911, new System.ComponentModel.CancelEventArgs());
            }
            else if (OptionButton902.Checked)
            {
                TextBox912.Tag = "";
                TextBox912_Validating(TextBox912, new System.ComponentModel.CancelEventArgs());
            }
            else if (OptionButton903.Checked)
                OptionButton903_CheckedChanged(OptionButton903, new System.EventArgs());
        }

        private void TextBox907_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char eventChar = eventArgs.KeyChar;

            if (eventChar == 13)
                TextBox907_Validating(TextBox907, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox907.Text);

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        private void TextBox907_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            double InterToler = 0.0, fDirl = 0.0;
            double d, d0, TrackToler,
                hFix, fTmp, fDis, dMin, dMax;

            IClone Clone;
            IPolygon pPoly;
            IPolyline pCutter;
            IProximityOperator pProxi;
            IConstructPoint pConstruct;
            IGroupElement pGroupElement;
            ITopologicalOperator2 pTopo;
            IPointCollection pLine, pSect0 = null, pSect1, pPolyClone;
            IPoint pt1, pt2;

            NavaidType TurnNav;

            if (!OptionButton602.Checked)
                return;

            int K = ComboBox902.SelectedIndex;
            if (K < 0)
                return;

            IPoint pInterceptPt = MPtCollection.Point[MPtCollection.PointCount - 1];

            if (TerInterNavDat[K].ValCnt != -2)
            {
                if (!double.TryParse(TextBox907.Text, out fDirl))
                    return;

                if (TextBox907.Tag.ToString() == TextBox907.Text)
                    return;             //else if (TerInterNavDat[K].ValCnt != -2 || TerInterNavDat[K].TypeCode == eNavaidType.NONE) 
            }

            Functions.DeleteGraphicsElement(GlobalVars.TerminationFIXElem);

            pGroupElement = (ESRI.ArcGIS.Carto.IGroupElement)new GroupElement();

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

                        if ((TerInterNavDat[K].TypeCode == eNavaidType.VOR))
                            InterToler = Navaids_DataBase.VOR.IntersectingTolerance;
                        else if (TerInterNavDat[K].TypeCode == eNavaidType.LLZ)
                            InterToler = Navaids_DataBase.LLZ.IntersectingTolerance;
                        else if (TerInterNavDat[K].TypeCode == eNavaidType.NDB)
                        {
                            fDirl += 180.0;
                            InterToler = Navaids_DataBase.NDB.IntersectingTolerance;
                        }

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
                            if (TerInterNavDat[K].TypeCode == eNavaidType.NDB)
                                TextBox907.Text = System.Math.Round(NativeMethods.Modulus(fDirl + 180.0 - TerInterNavDat[K].MagVar)).ToString();
                            else
                                TextBox907.Text = System.Math.Round(NativeMethods.Modulus(fDirl - TerInterNavDat[K].MagVar)).ToString();
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

                    if (OptionButton906.Checked || (N == 0))
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
                        TextBox907.Text = Functions.ConvertDistance(fDirl).ToString();

                    if ((TerInterNavDat[K].ValCnt < 0) || (OptionButton906.Enabled && OptionButton906.Checked))
                        Functions.CircleVectorIntersect(TerInterNavDat[K].pPtPrj, fDirl, pInterceptPt, pInterceptPt.M, out TerFixPnt);
                    else
                        Functions.CircleVectorIntersect(TerInterNavDat[K].pPtPrj, fDirl, pInterceptPt, pInterceptPt.M + 180.0, out TerFixPnt);
                    break;
            }

            // ==================================================================================================================
            if (OptionButton301.Checked)
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

                    pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)pSect0;
                    pPoly = (IPolygon)pTopo.Difference(pSect1);

                    pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)pPoly;
                    pTopo.IsKnownSimple_2 = false;
                    pTopo.Simplify();

                    if (Functions.SideDef(pCutter.FromPoint, pInterceptPt.M, pCutter.ToPoint) > 0)
                        pCutter.ReverseOrientation();

                    if ((TerInterNavDat[K].ValCnt < 0) || (OptionButton906.Enabled && OptionButton906.Checked))
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
            Clone = (ESRI.ArcGIS.esriSystem.IClone)Prim;
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
            TextBox907.Tag = TextBox907.Text;
            // ==========================================================================

            if (OptionButton901.Checked)
            {
                TextBox911.Tag = "";
                TextBox911_Validating(TextBox911, new System.ComponentModel.CancelEventArgs());
            }
            else if (OptionButton902.Checked)
            {
                TextBox912.Tag = "";
                TextBox912_Validating(TextBox912, new System.ComponentModel.CancelEventArgs());
            }
            else if (OptionButton903.Checked)
                OptionButton903_CheckedChanged(OptionButton903, new System.EventArgs());
        }

        private void TextBox911_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char eventChar = eventArgs.KeyChar;

            if (eventChar == 13)
                TextBox911_Validating(TextBox911, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox911.Text);

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        private void TextBox912_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char eventChar = eventArgs.KeyChar;

            if (eventChar == 13)
                TextBox912_Validating(TextBox912, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox912.Text);

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        private void TextBox911_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            if (TextBox911.ReadOnly)
                return;

            double NewTIA_PDG;
            if (!double.TryParse(TextBox911.Text, out NewTIA_PDG))
            {
                MessageBox.Show(Resources.str15267, "PANDA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (TextBox911.Tag.ToString() == TextBox911.Text)
                return;

            NewTIA_PDG *= 0.01;
            if (NewTIA_PDG < MinCurrPDG)
            {
                NewTIA_PDG = MinCurrPDG;
                TextBox911.Text = TextBox405.Text;
            }

            CalcNewStraightAreaWithFixedLength(ref NewTIA_PDG, ref TACurrPDG);

            TextBox911.Text = Math.Round(100.0 * CurrPDG, 1).ToString();
            TextBox911.Tag = TextBox911.Text;
            TextBox912.Text = Math.Round(100.0 * TACurrPDG, 1).ToString();
            //     TextBox912.Tag = TextBox912.Text   '????
            // =====================================================================================
            IProximityOperator pProxi;
            IPoint ptCnt;

            if (OptionButton301.Checked)
                pProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)ZNR_Poly;
            else
                pProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)KK;

            if (OptionButton601.Checked)
            {
                IPoint ptTmp = MPtCollection.Point[MPtCollection.PointCount - 1];
                Functions.CircleVectorIntersect(GlobalVars.CurrADHP.pPtPrj, GlobalVars.RModel, ptTmp, ptTmp.M, out ptCnt);
            }
            else if (OptionButton602.Checked)
                ptCnt = TerFixPnt;
            else
                ptCnt = TurnDirector.pPtPrj;

            // =====================================================================================
            double DistToFix = pProxi.ReturnDistance(ptCnt);
            double hTurn = fZNRLen * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value;
            hKK = hTurn + DER.pPtPrj[eRWY.PtDER].Z;
            fReachedA = hKK + DistToFix * TACurrPDG;

            TextBox914.Text = Functions.ConvertHeight(hKK, eRoundMode.NEAREST).ToString();

            double L1 = Functions.ReturnDistanceInMeters(DER.pPtPrj[eRWY.PtDER], TurnFixPnt);
            AltitudeAtTurnFixPnt = CurrPDG * L1 + PANS_OPS_DataBase.dpH_abv_DER.Value + DER.pPtPrj[eRWY.PtDER].Z;

            TextBox906.Text = Functions.ConvertHeight(fReachedA, eRoundMode.NEAREST).ToString();
            //TextBox916.Text = TextBox906.Text;
            TextBox916_Validating(TextBox916, null);
            // =====================================================================================

            IPointCollection mPoly = (IPointCollection)Functions.CalcTrajectoryFromMultiPoint(MPtCollection);
            mPoly.AddPoint(ptCnt);
            NomInfoFrm.SetInfo(mPoly, hKK, TACurrPDG);
        }

        private void TextBox912_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            if (TextBox912.ReadOnly)
                return;

            double newTAPDG;
            if (!double.TryParse(TextBox912.Text, out newTAPDG))
            {
                MessageBox.Show(Resources.str15267, "PANDA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (TextBox912.Tag.ToString() == TextBox912.Text)
                return;

            TACurrPDG = 0.01 * newTAPDG;
            if (TACurrPDG < PANS_OPS_DataBase.dpPDG_Nom.Value)
                TACurrPDG = PANS_OPS_DataBase.dpPDG_Nom.Value;

            int indx;
            double hTurn = fZNRLen * CurrPDG + PANS_OPS_DataBase.dpH_abv_DER.Value;
            hKK = hTurn + DER.pPtPrj[eRWY.PtDER].Z;

            TextBox914.Text = Functions.ConvertHeight(hKK, eRoundMode.NEAREST).ToString();

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

            TextBox911.Text = Math.Round(100.0 * CurrPDG, 1).ToString();
            TextBox912.Text = Math.Round(100.0 * TACurrPDG, 1).ToString();
            TextBox912.Tag = TextBox912.Text;

            // =====================================================================================
            IPoint ptCnt;
            if (OptionButton601.Checked)
            {
                IPoint ptTmp = MPtCollection.Point[MPtCollection.PointCount - 1];
                Functions.CircleVectorIntersect(GlobalVars.CurrADHP.pPtPrj, GlobalVars.RModel, ptTmp, ptTmp.M, out ptCnt);
            }
            else if (OptionButton602.Checked)
                ptCnt = TerFixPnt;
            else
                ptCnt = TurnDirector.pPtPrj;

            IProximityOperator pProxi;
            if (OptionButton301.Checked)
                pProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)ZNR_Poly;
            else
                pProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)KK;

            double DistToFix = pProxi.ReturnDistance(ptCnt);

            fReachedA = hKK + DistToFix * TACurrPDG;
            TextBox906.Text = Functions.ConvertHeight(fReachedA, eRoundMode.NEAREST).ToString();
            //TextBox916.Text = TextBox906.Text;
            TextBox916_Validating(TextBox916, null);
            // =====================================================================================

            IPointCollection mPoly = (IPointCollection)Functions.CalcTrajectoryFromMultiPoint(MPtCollection);
            mPoly.AddPoint(ptCnt);
            NomInfoFrm.SetInfo(mPoly, hKK, TACurrPDG);
        }

        private void ConvertTracToPoints()
        {
            if (CurrPage < 3)
                return;

            int n = MPtCollection.PointCount + 1;
            double PDGznr = double.Parse(TextBox911.Text) * 0.01;
            double PDGzr = double.Parse(TextBox912.Text) * 0.01;

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
            IConstructPoint ptConstr = (ESRI.ArcGIS.Geometry.IConstructPoint)pPtCross;

            //double fZLen = 0.0;
            double OverallLength = 0.0;
            for (int i = 1; i <= n; i++)
            {
                IPoint pPtGeo, pPtPrev = pPtCurr;

                RoutsPoints[i].Radius = -1;
                if (i < n)
                {
                    pPtCurr = MPtCollection.get_Point(i - 1);

                    if ((i & 1) == 1)
                    {
                        // =========================================================================
                        IPoint FromPt = MPtCollection.Point[i - 1];
                        IPoint ToPt = MPtCollection.Point[i];

                        //Functions.DrawPointWithText(FromPt, "pt00");
                        //Functions.DrawPointWithText(ToPt, "pt01");
                        //Application.DoEvents();

                        double fTmp = GlobalVars.DegToRadValue * (FromPt.M - ToPt.M);

                        if ((System.Math.Abs(System.Math.Sin(fTmp)) <= fE) && (System.Math.Cos(fTmp) > 0.0))
                        {
                        }
                        else
                        {
                            if (System.Math.Abs(System.Math.Sin(fTmp)) > fE)
                                ptConstr.ConstructAngleIntersection(FromPt, GlobalVars.DegToRadValue * (NativeMethods.Modulus(FromPt.M + 90.0)), ToPt, GlobalVars.DegToRadValue * (NativeMethods.Modulus(ToPt.M + 90.0)));
                            else
                                pPtCross.PutCoords(0.5 * (FromPt.X + ToPt.X), 0.5 * (FromPt.Y + ToPt.Y));

                            RoutsPoints[i].Radius = Functions.ReturnDistanceInMeters(pPtCross, FromPt);
                            RoutsPoints[i].Turn = Functions.SideDef(FromPt, FromPt.M, ToPt);

                            RoutsPoints[i].turnAngle = NativeMethods.Modulus((FromPt.M - ToPt.M) * RoutsPoints[i].Turn);
                            RoutsPoints[i].TurnArcLen = RoutsPoints[i].turnAngle * GlobalVars.DegToRadValue * RoutsPoints[i].Radius;

                            pPtGeo = (ESRI.ArcGIS.Geometry.IPoint)(Functions.ToGeo(pPtCross));
                            RoutsPoints[i].CenterLat = pPtGeo.Y;
                            RoutsPoints[i].CenterLon = pPtGeo.X;

                            RoutsPoints[i].Direction = -1;
                        }
                        // =========================================================================
                        RoutsPoints[i].Description = Resources.str00513 + GlobalVars.RomanFigures[(i + 1) / 2 - 1] + Resources.str00515;
                    }
                    else
                    {
                        RoutsPoints[i].Direction = NativeMethods.Modulus(Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], pPtCurr.M));
                        RoutsPoints[i].Description = Resources.str00514 + GlobalVars.RomanFigures[(i + 1) / 2 - 1] + Resources.str00515;
                    }
                }
                else
                {
                    pPtCurr = TracPoly.get_Point(TracPoly.PointCount - 1);
                    pPtCurr.M = pPtPrev.M;
                    RoutsPoints[i].Description = Resources.str00516;
                    //RoutsPoints[i].Direction = RoutsPoints[i - 1].Direction
                    RoutsPoints[i].Direction = -1;
                }

                IPolyline pPolyline;
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

                //Functions.DrawPolyline(pPolyline, -1,2);
                //Application.DoEvents();

                OverallLength = OverallLength + pPolyline.Length;
                RoutsPoints[i - 1].DistToNext = pPolyline.Length;
                RoutsPoints[i].DistToNext = 0.0;

                if (i == 1)
                    RoutsPoints[i].Height = RoutsPoints[i - 1].Height + RoutsPoints[i - 1].DistToNext * PDGznr;
                else
                    RoutsPoints[i].Height = RoutsPoints[i - 1].Height + RoutsPoints[i - 1].DistToNext * PDGzr;

                pPtGeo = (IPoint)Functions.ToGeo(pPtCurr);

                RoutsPoints[i].Lat = pPtGeo.Y;
                RoutsPoints[i].Lon = pPtGeo.X;
            }

            RoutsAllLen = OverallLength;
        }

        private void SaveAccuracy(string RepFileName, string RepFileTitle, ReportHeader pReport)
        {
            AccurRep = new ReportFile();

            AccurRep.OpenFile(RepFileName + "_Accuracy", RepFileTitle + ": " + Resources.str00173);
            //AccurRep.H1(My.Resources.str15271 + " - " + RepFileTitle + ": " + My.Resources.str00173);
            AccurRep.WriteString(Resources.str15271 + " - " + RepFileTitle + ": " + Resources.str00173, true);

            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            AccurRep.WriteHeader(pReport);
            AccurRep.Param("Distance accuracy", GlobalVars.settings.DistancePrecision.ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);
            AccurRep.Param("Angle accuracy", GlobalVars.settings.AnglePrecision.ToString(), "degrees");
            double horAccuracy = Functions.CalDERcHorisontalAccuracy(DER);
            Functions.SaveDerAccurasyInfo(AccurRep, DER, horAccuracy);

            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            if (!CheckBox201.Checked)
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
            GuidNav.TypeCode = eNavaidType.NDB;
            GuidNav.HorAccuracy = Functions.CalDERcHorisontalAccuracy(DER);

            NavaidType IntersectNav;

            // =============================================================================================================

            if (OptionButton302.Checked)
            {
                IntersectNav = TurnInterDat[ComboBox501.SelectedIndex];
                if (IntersectNav.ValCnt != -2)
                    Functions.SaveFixAccurasyInfo(AccurRep, TurnFixPnt, "TP", GuidNav, IntersectNav);
            }

            // =============================================================================================================

            if (OptionButton602.Checked)
            {
                IntersectNav = TerInterNavDat[ComboBox902.SelectedIndex];
                if (IntersectNav.ValCnt != -2)
                {
                    GuidNav = Navaids_DataBase.WPT_FIXToNavaid(TurnDirector);
                    Functions.SaveFixAccurasyInfo(AccurRep, TerFixPnt, "", GuidNav, IntersectNav, true);
                }
            }

            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            AccurRep.CloseFile();
        }

        private void SaveProtocol(string RepFileName, string RepFileTitle, ReportHeader pReport)
        {
            ReportsFrm.SortForSave();

            RoutsProtRep = new ReportFile();

            RoutsProtRep.ThrPtPrj = DER.pPtPrj[eRWY.PtDER];

            RoutsProtRep.OpenFile(RepFileName + "_Protocol", RepFileTitle + ": " + Resources.str00701);

            RoutsProtRep.WriteString(Resources.str15271 + " - " + RepFileTitle + ": " + Resources.str00701, true);
            RoutsProtRep.WriteString("");
            RoutsProtRep.WriteString(RepFileTitle, true);

            RoutsProtRep.WriteHeader(pReport);
            RoutsProtRep.WriteString("");
            RoutsProtRep.WriteString("");

            RoutsProtRep.lListView = ReportsFrm.listView1;
            RoutsProtRep.WriteTab(ReportsFrm.GetTabPageText(0));

            RoutsProtRep.lListView = ReportsFrm.listView4;
            RoutsProtRep.WriteTab(ReportsFrm.GetTabPageText(3));

            RoutsProtRep.lListView = ReportsFrm.listView5;
            RoutsProtRep.WriteTab(ReportsFrm.GetTabPageText(4));

            RoutsProtRep.CloseFile();
        }

        //private void SaveGeometry(string RepFileName, string RepFileTitle, ReportHeader pReport)
        //{
        //	CReportFile RoutsGeomRep = new CReportFile();
        //	RoutsGeomRep.ThrPtPrj = DER.pPtPrj[eRWY.PtDER];

        //	RoutsGeomRep.OpenFile(RepFileName + "_Geometry", Resources.str517);

        //	RoutsGeomRep.WriteString(Resources.str15271 + " - " + Resources.str517);
        //	RoutsGeomRep.WriteString("");
        //	RoutsGeomRep.WriteString(RepFileTitle);

        //	RoutsGeomRep.WriteHeader(pReport);
        //	//RoutsGeomRep.WriteParam LoadResString(518), CStr(Date) + " - " + CStr(Time)

        //	RoutsGeomRep.WriteString("");
        //	RoutsGeomRep.WriteString("");

        //	int n = RoutsPoints.Length;
        //	for (int i = 0; i < n; i++)
        //		RoutsGeomRep.WritePoint(RoutsPoints[i]);

        //	RoutsGeomRep.WriteString("");

        //	RoutsGeomRep.WriteParam(Resources.str519, Functions.ConvertDistance(RoutsAllLen, eRoundMode.rmNERAEST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);

        //	RoutsGeomRep.CloseFile();
        //	RoutsGeomRep = null;
        //}

        //private void SaveGeometry(string RepFileName, string RepFileTitle, ReportHeader pReport, RWYType DER, ReportPoint[] reportPoints, double allRoutsLen, bool guaded)
        //{
        //	RoutsGeomRep = new CReportFile();
        //	RoutsGeomRep.ThrPtPrj = DER.pPtPrj[eRWY.PtDER];

        //	RoutsGeomRep.OpenFile(RepFileName + "_Geometry", RepFileTitle + ": " + Resources.str00517);
        //	if (guaded)
        //		RoutsGeomRep.WriteString(System.Net.WebUtility.HtmlEncode(Resources.str15479 + " - " + RepFileTitle + ": " + Resources.str00517));
        //	else
        //		RoutsGeomRep.WriteString(System.Net.WebUtility.HtmlEncode(Resources.str15271 + " - " + RepFileTitle + ": " + Resources.str00517));

        //	RoutsGeomRep.WriteString("");
        //	RoutsGeomRep.WriteString(System.Net.WebUtility.HtmlEncode(RepFileTitle));
        //	RoutsGeomRep.WriteHeader(pReport);

        //	RoutsGeomRep.WriteString("");
        //	RoutsGeomRep.WriteString("");

        //	int n = reportPoints.Length;
        //	for (int i = 0; i < n; i++)
        //		RoutsGeomRep.WritePoint(reportPoints[i]);

        //	RoutsGeomRep.WriteString("");

        //	RoutsGeomRep.WriteParam(Resources.str00519, Functions.ConvertDistance(allRoutsLen, eRoundMode.rmNERAEST).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);

        //	RoutsGeomRep.CloseFile();
        //}

        private void SaveRoutsLog(string RepFileName, string RepFileTitle, ReportHeader pReport)
        {
            RoutsLogRep = new ReportFile();
            RoutsLogRep.ThrPtPrj = ((ESRI.ArcGIS.Geometry.IPoint)(DER.pPtPrj[eRWY.PtDER]));

            RoutsLogRep.OpenFile(RepFileName + "_Log", RepFileTitle + ": " + Resources.str00520);

            RoutsLogRep.WriteString(Resources.str15271 + " - " + RepFileTitle + ": " + Resources.str00520, true);
            RoutsLogRep.WriteString("");
            RoutsLogRep.WriteString(RepFileTitle, true);

            RoutsLogRep.WriteHeader(pReport);

            RoutsLogRep.WriteString("");
            RoutsLogRep.WriteString("");

            RoutsLogRep.ExH2(MultiPage1.TabPages[0].Text);
            RoutsLogRep.HTMLString("[ " + System.Net.WebUtility.HtmlEncode(MultiPage1.TabPages[0].Text) + " ]", false, false);
            RoutsLogRep.WriteString("");

            RoutsLogRep.Param(Label001.Text, ComboBox001.Text, "");
            RoutsLogRep.Param(Label002.Text, TextBox001.Text, "");
            RoutsLogRep.Param(Label006.Text, TextBox004.Text, "");
            RoutsLogRep.Param(Label013.Text, TextBox007.Text, Label010.Text);
            RoutsLogRep.Param(Label003.Text, TextBox002.Text, Label011.Text);
            RoutsLogRep.WriteString("");

            RoutsLogRep.Param(Label004.Text, TextBox003.Text, Label009.Text);
            if (Option4.Checked)
                RoutsLogRep.Param(Frame001.Text, Option4.Text, "");
            else
                RoutsLogRep.Param(Frame001.Text, Option3.Text, "");

            RoutsLogRep.WriteString("");

            RoutsLogRep.WriteString(Frame002.Text, true);
            RoutsLogRep.Param(Label007.Text, TextBox005.Text, "");
            RoutsLogRep.Param(Label008.Text, TextBox006.Text, Label012.Text);
            RoutsLogRep.WriteString("");
            RoutsLogRep.WriteString("");

            RoutsLogRep.ExH2(MultiPage1.TabPages[1].Text);
            RoutsLogRep.HTMLString("[ " + System.Net.WebUtility.HtmlEncode(MultiPage1.TabPages[1].Text) + " ]");
            RoutsLogRep.WriteString("");

            RoutsLogRep.Param(Label202.Text, SpinButton101.Value.ToString(), Label105.Text + " " + ComboBox101.Text);
            RoutsLogRep.Param(Label102.Text, TextBox102.Text, "%");
            RoutsLogRep.Param(Label103.Text, TextBox103.Text, "%");
            RoutsLogRep.Param(Label104.Text, TextBox104.Text, Label108.Text);
            RoutsLogRep.WriteString("");
            RoutsLogRep.WriteString("");

            RoutsLogRep.ExH2(MultiPage1.TabPages[2].Text);
            RoutsLogRep.WriteString("[ " + System.Net.WebUtility.HtmlEncode(MultiPage1.TabPages[2].Text) + " ]");
            RoutsLogRep.WriteString("");

            RoutsLogRep.Param(Label201.Text, TextBox201.Text, "∞");
            RoutsLogRep.Param(Label202.Text, TextBox202.Text, Label205.Text);
            RoutsLogRep.Param(Label203.Text, TextBox203.Text, Label206.Text);
            RoutsLogRep.Param(Label207.Text, TextBox204.Text, Label212.Text);
            RoutsLogRep.Param(Label208.Text, TextBox205.Text, "");
            RoutsLogRep.Param(Label209.Text, TextBox206.Text, "");
            RoutsLogRep.Param(_Label200_2.Text, ComboBox201.Text, "");

            RoutsLogRep.WriteString("");
            RoutsLogRep.WriteString("");

            if (CurrPage <= 2)
            {
                RoutsLogRep.CloseFile();
                return;
            }
            RoutsLogRep.ExH2(MultiPage1.TabPages[3].Text);
            RoutsLogRep.WriteString("[ " + System.Net.WebUtility.HtmlEncode(MultiPage1.TabPages[3].Text) + " ]");
            RoutsLogRep.WriteString("");

            RoutsLogRep.Param(Resources.str00902, Label309.Text, Label304.Text);
            RoutsLogRep.Param(Resources.str00903, Label310.Text, Label304.Text);
            RoutsLogRep.Param(Label302.Text, TextBox301.Text, Label304.Text);
            RoutsLogRep.Param(Label303.Text, TextBox302.Text, "∞");
            RoutsLogRep.Param(Label311.Text, ComboBox302.Text, "");
            RoutsLogRep.WriteString("");

            if (OptionButton301.Checked)
                RoutsLogRep.Param(Frame301.Text, OptionButton301.Text, "");
            else
                RoutsLogRep.Param(Frame301.Text, OptionButton302.Text, "");

            if (OptionButton301.Checked)
            {
                if (CheckBox301.Checked)
                    RoutsLogRep.Param(CheckBox301.Text, Resources.str39015, "");
                else
                    RoutsLogRep.Param(CheckBox301.Text, Resources.str39014, "");
            }

            RoutsLogRep.WriteString("");
            RoutsLogRep.WriteString("");

            RoutsLogRep.ExH2(MultiPage1.TabPages[4].Text);
            RoutsLogRep.WriteString("[ " + System.Net.WebUtility.HtmlEncode(MultiPage1.TabPages[4].Text) + " ]");
            RoutsLogRep.WriteString("");

            RoutsLogRep.Param(Label404.Text, TextBox403.Text, "∞");
            RoutsLogRep.WriteString("");

            RoutsLogRep.WriteString(Label401.Text, true);
            RoutsLogRep.Param(Label402.Text, TextBox401.Text, Label413.Text);
            RoutsLogRep.Param(Label403.Text, TextBox402.Text, Label414.Text);
            RoutsLogRep.WriteString("");

            RoutsLogRep.WriteString(Label407.Text, true);
            RoutsLogRep.Param(Label408.Text, SpinButton401.Value.ToString(), Label410.Text + " " + ComboBox401.Text);
            RoutsLogRep.Param(Label409.Text, TextBox405.Text, "%");
            RoutsLogRep.WriteString("");
            RoutsLogRep.WriteString("");

            RoutsLogRep.ExH2(MultiPage1.TabPages[5].Text);
            RoutsLogRep.WriteString("[ " + System.Net.WebUtility.HtmlEncode(MultiPage1.TabPages[5].Text) + " ]");
            RoutsLogRep.WriteString("");

            if (OptionButton302.Checked)
            {
                RoutsLogRep.WriteString(Frame501.Text, true);
                RoutsLogRep.Param(Label501.Text, ComboBox501.Text + " (" + Label503.Text + ")", "");
                RoutsLogRep.Param(Label502.Text, TextBox501.Text, Label526.Text);

                if (TurnInterDat[ComboBox501.SelectedIndex].TypeCode == eNavaidType.DME)
                {
                    if (Option501.Checked)
                        RoutsLogRep.Param(Resources.str00521, Option501.Text, "");
                    else
                        RoutsLogRep.Param(Resources.str00521, Option502.Text, "");
                }
            }
            else
                RoutsLogRep.WriteString(Frame502.Text, true);

            RoutsLogRep.WriteString("");

            RoutsLogRep.Param((Label525.Text), (TextBox503.Text), (Label527.Text));
            RoutsLogRep.Param((Label504.Text), (TextBox502.Text), (Label528.Text));
            RoutsLogRep.Param((Label506.Text), (Text501.Text), (Label507.Text));
            RoutsLogRep.WriteString("");

            RoutsLogRep.Param(Label509.Text, Text502.Text, Label529.Text);
            RoutsLogRep.Param(Label511.Text, Text503.Text, Label530.Text);
            RoutsLogRep.Param(Label513.Text, Text504.Text, Label531.Text);
            RoutsLogRep.Param(Label515.Text, Text505.Text, Label532.Text);
            RoutsLogRep.Param(Label517.Text, Text506.Text, Label533.Text);
            RoutsLogRep.Param(Label519.Text, Text507.Text, Label534.Text);

            if (OptionButton302.Checked)
            {
                RoutsLogRep.Param(Label521.Text, Text508.Text, Label535.Text);
                RoutsLogRep.Param(Label522.Text, Text509.Text, Label536.Text);
                RoutsLogRep.Param(Label523.Text, Text510.Text, Label537.Text);
            }

            RoutsLogRep.WriteString("");
            RoutsLogRep.WriteString("");

            RoutsLogRep.ExH2(MultiPage1.TabPages[6].Text);
            RoutsLogRep.WriteString("[ " + System.Net.WebUtility.HtmlEncode(MultiPage1.TabPages[6].Text) + " ]");
            RoutsLogRep.WriteString("");

            if (OptionButton601.Checked)
            {
                RoutsLogRep.WriteString(OptionButton601.Text, true);
                RoutsLogRep.Param(Label605.Text, TextBox602.Text, Label612.Text);
                RoutsLogRep.WriteString("");
            }
            else if (OptionButton602.Checked)
            {
                RoutsLogRep.WriteString(OptionButton602.Text, true);
                RoutsLogRep.Param(Label601.Text, ComboBox601.Text + " (" + Label602.Text + ")", "");
                RoutsLogRep.Param(Label605.Text, TextBox602.Text, Label612.Text);
                RoutsLogRep.WriteString("");

                RoutsLogRep.Param(Label613.Text, TextBox601.Text, Label614.Text);
                RoutsLogRep.Param(Label607.Text, TextBox603.Text, Label611.Text);
                RoutsLogRep.Param(Label603.Text, Label604.Text, GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);
                RoutsLogRep.WriteString("");
            }
            else if (OptionButton603.Checked)
            {
                RoutsLogRep.WriteString(OptionButton603.Text, true);
                RoutsLogRep.Param(Label601.Text, ComboBox601.Text + " (" + Label602.Text + ")", "");
                RoutsLogRep.Param(Label605.Text, TextBox602.Text, Label612.Text);
                RoutsLogRep.WriteString("");
            }

            RoutsLogRep.WriteString("");

            if (OptionButton602.Checked)
            {
                RoutsLogRep.ExH2(MultiPage1.TabPages[8].Text);
                RoutsLogRep.WriteString("[ " + System.Net.WebUtility.HtmlEncode(MultiPage1.TabPages[8].Text) + " ]");
                RoutsLogRep.WriteString("");

                RoutsLogRep.WriteString(Frame801.Text);
                if (CheckBox801.Checked)
                    RoutsLogRep.Param(CheckBox801.Text, Resources.str39015, "");
                else
                    RoutsLogRep.Param(CheckBox801.Text, Resources.str39014, "");

                if (CheckBox802.Checked)
                    RoutsLogRep.Param(CheckBox802.Text, Resources.str39015, "");
                else
                    RoutsLogRep.Param(CheckBox802.Text, Resources.str39014, "");

                if (CheckBox803.Checked)
                    RoutsLogRep.Param(CheckBox803.Text, Resources.str39015, "");
                else
                    RoutsLogRep.Param(CheckBox803.Text, Resources.str39014, "");

                RoutsLogRep.WriteString("");

                RoutsLogRep.WriteString(Frame802.Text);
                if (CheckBox804.Checked)
                    RoutsLogRep.Param(CheckBox804.Text, Resources.str39015, "");
                else
                    RoutsLogRep.Param(CheckBox804.Text, Resources.str39014, "");

                if (CheckBox805.Checked)
                    RoutsLogRep.Param(CheckBox805.Text, Resources.str39015, "");
                else
                    RoutsLogRep.Param(CheckBox805.Text, Resources.str39014, "");

                if (CheckBox806.Checked)
                    RoutsLogRep.Param(CheckBox806.Text, Resources.str39015, "");
                else
                    RoutsLogRep.Param(CheckBox806.Text, Resources.str39014, "");

                RoutsLogRep.WriteString("");
                RoutsLogRep.WriteString("");
            }

            RoutsLogRep.ExH2(MultiPage1.TabPages[9].Text);
            RoutsLogRep.WriteString("[ " + System.Net.WebUtility.HtmlEncode(MultiPage1.TabPages[9].Text) + " ]");
            RoutsLogRep.WriteString("");

            RoutsLogRep.Param(_Label910_0.Text, TextBox905.Text, _Label910_1.Text);
            RoutsLogRep.Param(_Label910_6.Text, TextBox916.Text, _Label910_7.Text);
            RoutsLogRep.Param(_Label910_2.Text, TextBox915.Text, _Label910_3.Text);
            RoutsLogRep.Param(_Label910_4.Text, TextBox906.Text, _Label910_5.Text);
            RoutsLogRep.WriteString("");

            if (OptionButton602.Checked)
            {
                RoutsLogRep.WriteString(Frame903.Text, true);

                RoutsLogRep.Param(_Label911_2.Text, ComboBox902.Text + " (" + _Label911_3.Text + ")", "");
                RoutsLogRep.Param(_Label911_4.Text, TextBox907.Text, _Label911_5.Text);

                if (TerInterNavDat[ComboBox902.SelectedIndex].TypeCode == eNavaidType.DME)
                {
                    if (OptionButton906.Checked)
                        RoutsLogRep.Param(Resources.str00521, OptionButton906.Text, "");
                    else
                        RoutsLogRep.Param(Resources.str00521, OptionButton907.Text, "");
                }

                RoutsLogRep.WriteString("");
            }

            RoutsLogRep.WriteString(Resources.str39001, true);
            RoutsLogRep.Param(Label901.Text, TextBox901.Text, "%");
            RoutsLogRep.Param(Label902.Text, TextBox902.Text, "%");
            RoutsLogRep.Param(Label907.Text, TextBox904.Text, GlobalVars.HeightConverter[GlobalVars.HeightUnit].Unit);
            RoutsLogRep.WriteString("");

            RoutsLogRep.WriteString(Resources.str39002, true);
            RoutsLogRep.Param(OptionButton901.Text, TextBox911.Text, "%");
            RoutsLogRep.Param(OptionButton902.Text, TextBox912.Text, "%");
            RoutsLogRep.WriteString("");

            RoutsLogRep.WriteString("");
            RoutsLogRep.WriteString("");

            RoutsLogRep.CloseFile();
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

            this.Text = Resources.str00033 + "  [" + MultiPage1.TabPages[StIndex].Text + "]";
        }

        void FillCombo901()
        {
            ComboBox901.Items.Clear();
            if (!OptionButton602.Checked)
                return;

            ComboBox901.Items.Add("Create new");

            int k = ComboBox902.SelectedIndex;
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

                fDist = Functions.ReturnDistanceInMeters(TerInterNavDat[k].pPtPrj, GlobalVars.WPTList[i].pPtPrj);
                int iSide = Functions.SideDef(TerInterNavDat[k].pPtPrj, pInterceptPt.M + 90, GlobalVars.WPTList[i].pPtPrj);
                int nN = TerInterNavDat[k].ValMin.Length;

                if (iSide < 0 || nN == 1)
                {
                    if (fDist >= TerInterNavDat[k].ValMin[0] && fDist < TerInterNavDat[k].ValMax[0])
                        ComboBox901.Items.Add(GlobalVars.WPTList[i]);
                    else if (fDist >= TerInterNavDat[k].ValMin[1] && fDist < TerInterNavDat[k].ValMax[1])
                        ComboBox901.Items.Add(GlobalVars.WPTList[i]);
                }
                else if (TerInterNavDat[k].IntersectionType == eIntersectionType.ByAngle)
                {
                    fDir = Functions.ReturnAngleInDegrees(TerInterNavDat[k].pPtPrj, GlobalVars.WPTList[i].pPtPrj);

                    if (Functions.AngleInSector(fDir, TerInterNavDat[k].ValMin[0], TerInterNavDat[k].ValMax[0]))
                        ComboBox901.Items.Add(GlobalVars.WPTList[i]);
                }
            }

            ComboBox901.SelectedIndex = 0;
        }

        private void ComboBox901_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (! bFormInitialised)				return;
            if (!OptionButton602.Checked)
                return;

            if (ComboBox901.SelectedIndex < 1)
                TextBox907.ReadOnly = false;
            else
            {
                TextBox907.ReadOnly = true;

                if (TerInterNavDat.Length <= 0)
                    return;

                int k = ComboBox902.SelectedIndex;
                if (k < 0)
                    return;

                WPT_FIXType selectedWPT = (WPT_FIXType)ComboBox901.SelectedItem;
                _Label911_1.Text = Navaids_DataBase.GetNavTypeName(selectedWPT.TypeCode);

                //IPolyline pPoly = CalcTrajectoryFromMultiPoint(MPtCollection);
                //DrawPolyLine(pPoly, RGB(255, 0, 0), 2);
                //Application.DoEvents();

                IPoint pInterceptPt = MPtCollection.Point[MPtCollection.PointCount - 1];

                if (TerInterNavDat[k].IntersectionType == eIntersectionType.ByDistance || TerInterNavDat[k].IntersectionType == eIntersectionType.RadarFIX)
                {
                    double fDist = Functions.ReturnDistanceInMeters(TerInterNavDat[k].pPtPrj, selectedWPT.pPtPrj);

                    TextBox907.Text = Functions.ConvertDistance(fDist - TerInterNavDat[k].Disp, eRoundMode.NONE).ToString("0.0000");
                    if (Functions.SideDef(TerInterNavDat[k].pPtPrj, pInterceptPt.M + 90.0, selectedWPT.pPtPrj) < 0)
                        OptionButton906.Checked = true;
                    else
                        OptionButton907.Checked = true;
                }
                else
                {
                    double fDir = Functions.ReturnAngleInDegrees(TerInterNavDat[k].pPtPrj, selectedWPT.pPtPrj);
                    double fAzt = Functions.Dir2Azt(TerInterNavDat[k].pPtPrj, fDir);
                    if (TerInterNavDat[k].TypeCode == eNavaidType.NDB)
                        TextBox907.Text = Functions.RoundAngle(NativeMethods.Modulus(fAzt + 180.0 - TerInterNavDat[k].MagVar), eRoundMode.NONE).ToString("0.00");
                    else
                        TextBox907.Text = Functions.RoundAngle(NativeMethods.Modulus(fAzt - TerInterNavDat[k].MagVar), eRoundMode.NONE).ToString("0.00");
                }
            }

            TextBox907.Tag = "-";
            TextBox907_Validating(TextBox907, null);
        }

        private void ComboBox902_SelectedIndexChanged(object sender, EventArgs e)
        {
            int n = TerInterNavDat.Length;
            int k = ComboBox902.SelectedIndex;

            if (n == 0 || k < 0 || !OptionButton602.Checked)
                return;

            _Label911_3.Text = Navaids_DataBase.GetNavTypeName(TerInterNavDat[k].TypeCode);

            string tipStr;

            if (TerInterNavDat[k].TypeCode == eNavaidType.DME)
            {
                TextBox907.Visible = true;

                _Label911_5.Text = GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;

                n = TerInterNavDat[k].ValMin.GetUpperBound(0);

                OptionButton906.Enabled = n > 0;
                OptionButton907.Enabled = n > 0;

                if (n == 0)
                {
                    if (TerInterNavDat[k].ValCnt > 0)
                        OptionButton906.Checked = true;
                    else
                        OptionButton907.Checked = true;
                }

                if (OptionButton906.Checked || (n == 0))
                    TextBox907.Text = Functions.ConvertDistance(TerInterNavDat[k].ValMin[0], eRoundMode.NEAREST).ToString();
                else
                    TextBox907.Text = Functions.ConvertDistance(TerInterNavDat[k].ValMin[1], eRoundMode.NEAREST).ToString();


                _Label911_4.Text = Resources.str15096;          // "??????????:"
                tipStr = Resources.str15097;                    // "?????????? ???????? ??????????:"

                for (int i = 0; i <= n; i++)
                {
                    tipStr = tipStr + Resources.str15098 + Functions.ConvertDistance(TerInterNavDat[k].ValMin[i], eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit +
                                    Resources.str15099 + Functions.ConvertDistance(TerInterNavDat[k].ValMax[i], eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit;
                    if (i < n)
                        tipStr = Resources.str26101 + " - " + tipStr + "\r\n" + Resources.str26102 + " - ";
                }
            }
            else
            {
                // =============================================================================================
                OptionButton906.Enabled = false;
                OptionButton907.Enabled = false;

                TextBox907.Visible = TerInterNavDat[k].ValCnt != -2;
                if (TerInterNavDat[k].ValCnt == -2)
                {
                    TextBox907.Text = "";
                    tipStr = "FIX " + Resources.str00106;
                    _Label911_4.Text = tipStr;
                    _Label911_5.Text = "";
                }
                else
                {
                    double Kmin, Kmax;
                    _Label911_5.Text = "∞";
                    if (TerInterNavDat[k].TypeCode == eNavaidType.VOR)
                    {
                        _Label911_4.Text = Resources.str15101;
                        Kmax = NativeMethods.Modulus(TerInterNavDat[k].ValMax[0] - TerInterNavDat[k].MagVar);
                        Kmin = NativeMethods.Modulus(TerInterNavDat[k].ValMin[0] - TerInterNavDat[k].MagVar);
                        tipStr = Resources.str15102; // "?????????? ???????? ????????: ?? "
                    }
                    else
                    {
                        _Label911_4.Text = Resources.str15104;
                        Kmax = NativeMethods.Modulus(TerInterNavDat[k].ValMax[0] + 180.0 - TerInterNavDat[k].MagVar);
                        Kmin = NativeMethods.Modulus(TerInterNavDat[k].ValMin[0] + 180.0 - TerInterNavDat[k].MagVar);
                        tipStr = Resources.str15100; // "?????????? ???????? ????????: ?? "
                    }

                    if (TerInterNavDat[k].ValCnt > 0)
                        TextBox907.Text = Math.Round(Kmin, 1).ToString();
                    else
                        TextBox907.Text = Math.Round(Kmax, 1).ToString();

                    tipStr = tipStr + Functions.DegToStr(Kmin) + Resources.str15103 + Functions.DegToStr(Kmax);
                }
            }

            if ((TerInterNavDat[k].TypeCode == eNavaidType.DME) || (TerInterNavDat[k].ValCnt != -2))
            {
                tipStr = tipStr.Replace("\r\n", "   ");
                ToolTip1.SetToolTip(TextBox907, tipStr);
            }

            TextBox907.Tag = "-";
            TextBox907_Validating(TextBox907, new System.ComponentModel.CancelEventArgs());
            FillCombo901();
        }

        private void TextBox916_KeyPress(System.Object eventSender, KeyPressEventArgs eventArgs)
        {
            char eventChar = eventArgs.KeyChar;

            if (eventChar == 13)
                TextBox916_Validating(TextBox916, new System.ComponentModel.CancelEventArgs());
            else
                Functions.TextBoxFloat(ref eventChar, TextBox916.Text);

            eventArgs.KeyChar = eventChar;
            if (eventChar == 0)
                eventArgs.Handled = true;
        }

        private void TextBox916_Validating(System.Object eventSender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            double fTmp;
            if (!double.TryParse(TextBox916.Text, out fTmp))
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
                TextBox916.Text = Functions.ConvertHeight(NevVal).ToString();
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

            if (OptionButton301.Checked)
                pDistPoly = (ESRI.ArcGIS.Geometry.IGeometry)ZNR_Poly;
            else
                pDistPoly = KK;

            IProximityOperator pProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)pDistPoly;
            double dBuffer = PANS_OPS_DataBase.dpTermMinBuffer.Value;

            //if (OptionButton603.Checked)
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
            if (OptionButton601.Checked)
            {
                d0 = TurnAreaMaxd0;
                dBuffer = TurnAreaMaxd0;
            }
            else if (OptionButton602.Checked)
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

            //if (MultiPage1.SelectedIndex <= 8)
            TextBox905.Text = Functions.ConvertDistance(dBuffer, eRoundMode.NEAREST).ToString();
            //else
            //{
            //    if (!double.TryParse(TextBox905.Text, out dBuffer))
            //        dBuffer = -1.0;
            //    else
            //        dBuffer = Functions.DeConvertDistance(dBuffer);

            //    if (dBuffer < PANS_OPS_DataBase.dpTermMinBuffer.Value)
            //    {
            //        dBuffer = PANS_OPS_DataBase.dpTermMinBuffer.Value;
            //        if (!OptionButton601.Checked)
            //            TextBox905.Text = Functions.ConvertDistance(dBuffer, eRoundMode.rmNERAEST).ToString();
            //    }
            //}

            //if (OptionButton602.Checked || (OptionButton603.Checked && (TurnWPT.TypeCode > eNavaidType.CodeNONE)))
            if (!OptionButton603.Checked)
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
            TextBox906.Text = Functions.ConvertHeight(fReachedA, eRoundMode.NEAREST).ToString();
            TextBox916.Text = TextBox906.Text;
            TextBox916_Validating(TextBox916, null);
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

                //	if (oTrnAreaList.Parts[i].hPenet > MaxDh)
                //	{
                //		MaxDh = oTrnAreaList.Parts[i].hPenet;
                //		IxDh = i;
                //	}
                //}
            }

            TextBox915.Text = Functions.ConvertHeight(MaxReq + DER.pPtPrj[eRWY.PtDER].Z, eRoundMode.NEAREST).ToString();
            //TextBox916.Text = TextBox915.Text;
            TextBox916_Validating(TextBox915, null);

            ToolTip1.SetToolTip(TextBox915, "");
            if (IxMaxReq >= 0)
                ToolTip1.SetToolTip(TextBox915, oTrnAreaList.Obstacles[oTrnAreaList.Parts[IxMaxReq].Owner].UnicalName);
            // =========================================================================================================

            TextBox901.Text = Math.Round(100.0 * CurrPDG, 1).ToString();
            TextBox902.Text = Math.Round(100.0 * TACurrPDG, 1).ToString();
            TextBox903.Text = Functions.ConvertHeight(dTNAh, eRoundMode.CEIL).ToString();

            hKK = hTurn + DER.pPtPrj[eRWY.PtDER].Z;

            TextBox904.Text = Functions.ConvertHeight(hKK, eRoundMode.NEAREST).ToString();

            TACurrPDG = Functions.CalcTA_PDG(oTrnAreaList.Parts, hTurn, TACurrPDG, out i);

            TextBox911.Text = TextBox901.Text;
            TextBox912.Text = Math.Round(100.0 * TACurrPDG, 1).ToString();
            //TextBox913.Text = "0"
            TextBox914.Text = TextBox904.Text;

            double L1 = Functions.ReturnDistanceInMeters(DER.pPtPrj[eRWY.PtDER], TurnFixPnt);
            AltitudeAtTurnFixPnt = CurrPDG * L1 + PANS_OPS_DataBase.dpH_abv_DER.Value + DER.pPtPrj[eRWY.PtDER].Z;
            InitialTurnFixAltitude = AltitudeAtTurnFixPnt;

            OptionButton901.Checked = true;

            //ConvertTracToPoints();
            ReportsFrm.FillPage5(oTrnAreaList);

            // =====================================================================================
            IPointCollection mPoly = (IPointCollection)Functions.CalcTrajectoryFromMultiPoint(MPtCollection);
            mPoly.AddPoint(ptCnt);

            if (!OptionButton601.Checked)
            {
                TextBox905.BackColor = SystemColors.Window;
                TextBox905.ReadOnly = false;
            }
            else
            {
                TextBox905.BackColor = SystemColors.ButtonFace;
                TextBox905.ReadOnly = true;
            }

            NomInfoFrm.SetInfo(mPoly, hKK, TACurrPDG);
            // =====================================================================================

            if (OptionButton602.Checked)
            {
                double DistMin = 0.0;
                double DistMax = Functions.ReturnDistanceInMeters(pTurnComplitPt, ptAreaEnd);
                //Functions.DrawPointWithText(ptAreaEnd, "ptAreaEnd");				//Functions.DrawPointWithText(pTurnComplitPt, "pTurnComplitPt");				//Application.DoEvents();
                TerInterNavDat = Functions.GetValidTurnTermNavs(pTurnComplitPt, DER.pPtPrj[eRWY.PtDER].Z, DistMin, DistMax, pTurnComplitPt.M, TACurrPDG, TurnDirector.TypeCode, TurnDirector.pPtPrj);

                ComboBox902.Items.Clear();
                n = TerInterNavDat.Length;

                for (i = 0; i < n; i++)
                    ComboBox902.Items.Add(TerInterNavDat[i].CallSign);

                if (Functions.SideDef(pTurnComplitPt, pTurnComplitPt.M + 90.0, TurnDirector.pPtGeo) > 0)
                {
                    System.Array.Resize<NavaidType>(ref TerInterNavDat, n + 1);
                    TerInterNavDat[n] = Navaids_DataBase.WPT_FIXToNavaid(TurnDirector);
                    n++;
                    ComboBox902.Items.Add(TurnDirector.Name);
                }

                if (n == 0)
                {
                    TerInterNavDat = new NavaidType[0];
                    TerInterNavDat[0].CallSign = "Radar FIX";
                    TerInterNavDat[0].Identifier = Guid.Empty;
                    TerInterNavDat[0].NAV_Ident = Guid.Empty;
                    TerInterNavDat[0].Name = "Radar FIX";

                    TerInterNavDat[0].TypeCode = eNavaidType.DME;

                    TerInterNavDat[0].MagVar = TurnDirector.MagVar;
                    TerInterNavDat[0].Range = Navaids_DataBase.DME.Range;

                    TerInterNavDat[0].pPtGeo = TurnDirector.pPtGeo;
                    TerInterNavDat[0].pPtPrj = TurnDirector.pPtPrj;
                    // ===============================================================

                    IPoint Pt0 = Functions.PointAlongPlane(pTurnComplitPt, pTurnComplitPt.M, DistMin);
                    IPoint Pt1 = Functions.PointAlongPlane(pTurnComplitPt, pTurnComplitPt.M, TurnAreaMaxd0);

                    int Side0 = Functions.SideDef(Pt0, pTurnComplitPt.M + 90.0, TurnDirector.pPtPrj);
                    int Side1 = Functions.SideDef(Pt1, pTurnComplitPt.M + 90.0, TurnDirector.pPtPrj);

                    double Dist0 = Functions.ReturnDistanceInMeters(Pt0, TurnDirector.pPtPrj);
                    double Dist1 = Functions.ReturnDistanceInMeters(Pt1, TurnDirector.pPtPrj);

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

                    ComboBox902.Items.Add("Radar FIX");
                }

                ComboBox902.SelectedIndex = 0;
            }

            return hTurn;
        }

        private DepartureLeg StraightDepartureLeg(ref StandardInstrumentDeparture pProcedure, AircraftCharacteristic IsLimitedTo, out TerminalSegmentPoint pEndPoint)
        {
            double Angle, fDist, fDir1, fDir;
            double PriorFixTolerance, PostFixTolerance;
            double fSegmentLength, fDistToNav, fAltitude;

            DepartureLeg pDepartureLeg;
            SegmentLeg pSegmentLeg;
            DistanceIndication pDistanceIndication;
            Feature pFixDesignatedPoint;

            SignificantPoint pFIXSignPt;
            TerminalSegmentPoint pStartPoint;
            SegmentPoint pSegmentPoint;

            ValDistance pDistance;
            ValDistanceVertical pDistanceVertical;
            ValSpeed pSpeed;

            UomDistance mUomHDistance;
            UomDistanceVertical mUomVDistance;
            UomSpeed mUomSpeed;

            UomDistance[] uomDistHorzTab;
            UomDistanceVertical[] uomDistVerTab;
            UomSpeed[] uomSpeedTab;

            NavaidType IntersectNav;
            SignificantPoint pInterNavSignPt;

            IPoint ptTmp;
            bool HaveTP = CheckBox201.Checked;

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

            pSegmentLeg.UpperLimitReference = CodeVerticalReference.MSL;
            pSegmentLeg.LowerLimitReference = CodeVerticalReference.MSL;
            pSegmentLeg.CourseType = CodeCourse.TRUE_TRACK;
            pSegmentLeg.ProcedureTurnRequired = false;
            //pSegmentLeg.TurnDirection = CodeDirectionTurn.EITHER;
            pSegmentLeg.LegPath = CodeTrajectory.STRAIGHT;

            if (HaveTP && OptionButton302.Checked)
            {
                IntersectNav = TurnInterDat[ComboBox501.SelectedIndex];

                if (IntersectNav.TypeCode == eNavaidType.DME)
                    pSegmentLeg.LegTypeARINC = CodeSegmentPath.VD;
                else
                    pSegmentLeg.LegTypeARINC = CodeSegmentPath.VR;
            }
            else
            {
                pSegmentLeg.LegTypeARINC = CodeSegmentPath.VA;
                pSegmentLeg.EndConditionDesignator = CodeSegmentTermination.ALTITUDE;
                IntersectNav = default(NavaidType);
            }


            // =======================================================================================
            //if (Functions.SideDef(PtDerShift, DepDir + 90.0, GuidNav.pPtPrj) < 0)
            //    pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.FROM;
            //else
            //    pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO;

            pSegmentLeg.Course = Functions.Dir2Azt(DER.pPtPrj[eRWY.PtDER], DepDir);
            //pSegmentLeg.Course = NativeMethods.Modulus(System.Math.Round(DER.pPtGeo[eRWY.PtDER].M - TrackAdjust), 360.0);

            // =======================================================================================
            pDistanceVertical = new ValDistanceVertical();
            pDistanceVertical.Uom = mUomVDistance;

            if (HaveTP)
                pDistanceVertical.Value = Functions.ConvertHeight(AltitudeAtTurnFixPnt);
            else
                pDistanceVertical.Value = double.Parse(TextBox207.Text);

            pSegmentLeg.LowerLimitAltitude = pDistanceVertical;
            pSegmentLeg.AltitudeInterpretation = CodeAltitudeUse.AT_LOWER;
            // =================
            if (HaveTP)
                fSegmentLength = Functions.ReturnDistanceInMeters(DER.pPtPrj[eRWY.PtDER], TurnFixPnt);
            else
                fSegmentLength = (fStraightDepartTermAlt - PANS_OPS_DataBase.dpH_abv_DER.Value) / CurrPDG;

            pDistance = new ValDistance();
            pDistance.Uom = mUomHDistance;
            pDistance.Value = Functions.ConvertDistance(fSegmentLength, eRoundMode.NEAREST);
            pSegmentLeg.Length = pDistance;
            // =================
            pSegmentLeg.VerticalAngle = Functions.RadToDeg(System.Math.Atan(CurrPDG));
            // =================
            pSpeed = new ValSpeed();
            pSpeed.Uom = mUomSpeed;
            pSpeed.Value = double.Parse(TextBox301.Text);
            pSegmentLeg.SpeedLimit = pSpeed;
            pSegmentLeg.SpeedReference = CodeSpeedReference.IAS;

            // Start Point ========================

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
            // ==
            SignificantPoint derEnd = new SignificantPoint();
            derEnd.RunwayPoint = new FeatureRef(DER.pSignificantPointID[eRWY.PtDER]);
            pSegmentPoint.PointChoice = derEnd;
            //pSignificantPoint = DER.pSignificantPoint[eRWY.PtDER];
            //pSegmentPoint.PointChoice = pSignificantPoint;

            pSegmentLeg.StartPoint = pStartPoint;
            //  End Of Start Point ========================

            double horAccuracy = 0.0;

            NavaidType GuidNav = default(NavaidType);
            GuidNav.pPtPrj = DER.pPtPrj[eRWY.PtDER];
            GuidNav.Identifier = new Guid("C3E1A55A-490B-4230-AB1E-D60DB17C7E7C");
            GuidNav.TypeCode = eNavaidType.NDB;
            GuidNav.HorAccuracy = Functions.CalDERcHorisontalAccuracy(DER);

            //  EndPoint ========================
            pEndPoint = null;

            if (HaveTP && !OptionButton301.Checked)
            {
                pInterNavSignPt = IntersectNav.GetSignificantPoint();

                pEndPoint = new TerminalSegmentPoint();
                //         pEndPoint.IndicatorFACF =      ??????????
                //         pEndPoint.LeadDME =            ??????????
                //         pEndPoint.LeadRadial =         ??????????
                pEndPoint.Role = CodeProcedureFixRole.TP;

                pSegmentPoint = pEndPoint;

                //pSegmentPoint.FlyOver = false;
                pSegmentPoint.RadarGuidance = false;

                pSegmentPoint.ReportingATC = CodeATCReporting.NO_REPORT;
                pSegmentPoint.Waypoint = false;
                // =====
                PointReference pPointReference;
                AngleUse pAngleUse;
                AngleIndication pStAngleIndication;
                ValDistanceSigned pDistanceSigned;

                pPointReference = new PointReference();

                if (IntersectNav.TypeCode != eNavaidType.NONE)
                    horAccuracy = Functions.CalcHorisontalAccuracy(TurnFixPnt, GuidNav, IntersectNav);

				fDir1 = Functions.Azt2DirPrj(TurnFixPnt, pSegmentLeg.Course.Value);
				pFixDesignatedPoint = DBModule.CreateDesignatedPoint(TurnFixPnt, "TP", fDir1);
				pFIXSignPt = new SignificantPoint();

                if (pFixDesignatedPoint is DesignatedPoint)
                    pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef();
                else if (pFixDesignatedPoint is Navaid)
                    pFIXSignPt.NavaidSystem = pFixDesignatedPoint.GetFeatureRef();

                // ========================

                if (IntersectNav.TypeCode == eNavaidType.DME)
                {
                    fAltitude = TurnFixPnt.Z - IntersectNav.pPtPrj.Z;
                    fDistToNav = Functions.ReturnDistanceInMeters(IntersectNav.pPtPrj, TurnFixPnt);
                    fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitude * fAltitude);
                    pDistanceIndication = DBModule.CreateDistanceIndication(Functions.ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, pInterNavSignPt);
                    pDistanceIndication.Fix = pFIXSignPt.GetFeatureRef();
                    // ======================== pStDistanceIndication.

                    pPointReference.FacilityDistance.Add(pDistanceIndication.GetFeatureRefObject());
                    pPointReference.Role = CodeReferenceRole.RAD_DME;
                }
                else
                {
                    pAngleUse = new AngleUse();
                    fDir = Functions.ReturnAngleInDegrees(IntersectNav.pPtPrj, TurnFixPnt);

                    Angle = NativeMethods.Modulus(Functions.Dir2Azt(IntersectNav.pPtPrj, fDir) - IntersectNav.MagVar, 360.0);
                    pStAngleIndication = DBModule.CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pInterNavSignPt);
                    pStAngleIndication.TrueAngle = Functions.Dir2Azt(TurnFixPnt, fDir);

                    pStAngleIndication.Fix = pFIXSignPt.GetFeatureRef();
                    pAngleUse.TheAngleIndication = pStAngleIndication.GetFeatureRef();
                    pAngleUse.AlongCourseGuidance = false;
                    // ========================

                    pPointReference.FacilityAngle.Add(pAngleUse);
                    pPointReference.Role = CodeReferenceRole.INTERSECTION;
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
                //Functions.DrawPolygon(pFIXPoly, 0, true, (int)ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSCross );

                pPointReference.FixToleranceArea = Converters.ESRIPolygonToAIXMSurface((IPolygon)Functions.ToGeo((IGeometry)pFIXPoly));
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

            pLocation = Converters.ESRIPointToARANPoint(DER.pPtGeo[eRWY.PtDER]);
            pLineStringSegment.Add(pLocation);

            if (HaveTP)
                ptTmp = (IPoint)Functions.ToGeo(TurnFixPnt);
            else
                ptTmp = (IPoint)Functions.ToGeo(Functions.PointAlongPlane(DER.pPtPrj[eRWY.PtDER], DepDir, fSegmentLength));

            pLocation = Converters.ESRIPointToARANPoint(ptTmp);
            pLineStringSegment.Add(pLocation);

            pCurve = new Curve();
            pCurve.Geo.Add(pLineStringSegment);
            pSegmentLeg.Trajectory = pCurve;
            //===========================================================
            // I protected Area =======================================================

            IPolygon pPolygon1 = Functions.PolygonIntersection(pPolygon, pCircle);
            Functions.GetZNRObstList(oAllStraightList, out oZNRList, DER, DepDir, CurrPDG, pPolygon1);
            int result = Functions.CalcPDGToTop(oZNRList, pPolygon, ArrayIVariant, DER, 0.0, DepDir, RWYDir, MOCLimit);

            //Functions.DrawPolygon(ZNR_Poly, -1, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSCross);
            //while(true)
            //Application.DoEvents();

            if (HaveTP)
                pPolygon1 = (IPolygon)ZNR_Poly;
            else
                pPolygon1 = Functions.PolygonIntersection(pPolygon, pCircle);

            ObstacleAssessmentArea pPrimProtectedArea = new ObstacleAssessmentArea();
            pPrimProtectedArea.Surface = Converters.ESRIPolygonToAIXMSurface(Functions.ToGeo(pPolygon1 as IGeometry) as IPolygon);
            pPrimProtectedArea.SectionNumber = 0;
            //pPrimProtectedArea.StartingCurve = pCurve;
            pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY;
            pPrimProtectedArea.SurfaceZone = CodeObstructionIdSurfaceZone.PRIMARY;
            //=====================================================================

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
            //		pPrimProtectedArea.SignificantObstacle.Add(obs);

            //	//if (pSecProtectedArea != null && (isPrimary & 2) != 0)
            //	//	pSecProtectedArea.SignificantObstacle.Add(obs);
            //}
            //  END ======================================================

            //==========================================================================
            /*
			// II protected Area =======================================================
			ITopologicalOperator2 pTopo = (ITopologicalOperator2)(currLeg.FullAssesmentArea);
			IPolygon pPolygon1 = (IPolygon)pTopo.Difference( currLeg.PrimaryAssesmentArea);
			pTopo = (ITopologicalOperator2)pPolygon1;
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();

			if (pPolygon1 != null)
			{
				ObstacleAssessmentArea pSecProtectedArea = new ObstacleAssessmentArea();
				pSecProtectedArea.Surface = Converters.ESRIPolygonToAIXMSurface(Functions.ToGeo(pPolygon1 as IGeometry) as IPolygon);
				pSecProtectedArea.SectionNumber = 1;
				pSecProtectedArea.Type = CodeObstacleAssessmentSurface.SECONDARY;
				//pPrimProtectedArea.SurfaceZone = CodeObstructionIdSurfaceZone.PRIMARY;

				pPrimProtectedArea.StartingCurve = pCurve;

				pSegmentLeg.DesignSurface.Add(pSecProtectedArea);
			}
			*/

            pSegmentLeg.DesignSurface.Add(pPrimProtectedArea);
            //if (pSecProtectedArea != null)
            //	pSegmentLeg.DesignSurface.Add(pSecProtectedArea);

            //  END =====================================================
            return pDepartureLeg;
        }

        private DepartureLeg TurningDepartureLeg(ref StandardInstrumentDeparture pProcedure, AircraftCharacteristic IsLimitedTo, ref TerminalSegmentPoint pEndPoint)
        {
            DepartureLeg result;
            SegmentLeg pSegmentLeg;

            //FIXableNavaidType GuidNav;
            //SignificantPoint pGuidNavSignPt;

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

            //string sTermAlt;
            bool HaveIntercept = OptionButton602.Checked;

            double fTermAlt, fDir, fAlt;
            double PriorFixTolerance, PostFixTolerance;

            PointReference pPointReference;
            //SignificantPoint pFIXSignPt;
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
                fTermAlt = double.Parse(TextBox916.Text);

            pDistanceVertical = new ValDistanceVertical();
            pDistanceVertical.Uom = mUomVDistance;

            pDistanceVertical.Value = fTermAlt;
            pSegmentLeg.LowerLimitAltitude = pDistanceVertical;
            // ======
            pSegmentLeg.VerticalAngle = Functions.RadToDeg(System.Math.Atan(TACurrPDG));
            // ======
            pSpeed = new ValSpeed();
            pSpeed.Uom = mUomSpeed;

            pSpeed.Value = double.Parse(TextBox301.Text);
            pSegmentLeg.SpeedLimit = pSpeed;
            pSegmentLeg.SpeedReference = CodeSpeedReference.IAS;

            // StartPoint ========================
            //pSegmentLeg.StartPoint = pEndPoint;
            // End Of StartPoint =================

            // ====================================================================================================
            if (OptionButton601.Checked)
            {
                pSegmentLeg.LegTypeARINC = CodeSegmentPath.VM;
                pSegmentLeg.Course = NativeMethods.Modulus(double.Parse(TextBox602.Text) + GlobalVars.CurrADHP.MagVar, 360.0);
                pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.ALTITUDE;
                pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT;
            }
            else if (OptionButton602.Checked)
            {
                pSegmentLeg.LegTypeARINC = CodeSegmentPath.VI;
                //         fTmp = CDbl(TextBox603.Text)
                //         pDouble.Value = NativeMethods.Modulus (CDbl(TextBox602.Text) + CurrADHP.MagVar + fTmp * TurnDir, 360.0)
                pSegmentLeg.Course = System.Math.Round(Functions.Dir2Azt(MPtCollection.Point[1], MPtCollection.Point[1].M), 1);
                pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT;
                pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT;
            }
            else// if (OptionButton603.Checked)
            {
                pSegmentLeg.LegPath = CodeTrajectory.STRAIGHT;
                pSegmentLeg.BankAngle = double.Parse(TextBox302.Text);

                //GuidNav = Navaids_DataBase.WPT_FIXToFixableNavaid(TurnDirector);
                //pGuidNavSignPt = TurnDirector.GetSignificantPoint();				// new SignificantPoint();
                //pGuidNavSignPt.NavaidSystem = TurnDirector.GetFeatureRef();

                //else if ()
                //{

                //    pSegmentLeg.LegTypeARINC = CodeSegmentPath.DF;
                //    pSegmentLeg.Course = NativeMethods.Modulus(double.Parse(TextBox602.Text) + GlobalVars.CurrADHP.MagVar, 360.0);

                //    //if (Functions.SideDef(TurnFixPnt, DepDir + 90.0, GuidNav.pPtPrj) < 0)
                //    //    pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.FROM;
                //    //else
                //    pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO;
                //}
                //else
                //{
                if (CheckBox601.Checked || TurnDirector.TypeCode == eNavaidType.NONE)
                    pSegmentLeg.LegTypeARINC = CodeSegmentPath.DF;
                else
                    pSegmentLeg.LegTypeARINC = CodeSegmentPath.CF;

                pSegmentLeg.Course = NativeMethods.Modulus(double.Parse(TextBox602.Text) + GlobalVars.CurrADHP.MagVar, 360.0);

                //if (Functions.SideDef(TurnFixPnt, DepDir + 90.0, GuidNav.pPtPrj) < 0)
                //    pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.FROM;
                //else
                pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO;
                //}

                // StartPoint ========================
                //pSegmentLeg.StartPoint = pEndPoint;
                // End Of StartPoint =================

                // EndPoint ==========================
                pEndPoint = new TerminalSegmentPoint();
                // pEndPoint.IndicatorFACF =      ??????????
                // pEndPoint.LeadDME =            ??????????
                // pEndPoint.LeadRadial =         ??????????
                // pEndPoint.Role = CodeProcedureFixRole.VDP;
                pSegmentPoint = pEndPoint;

                //pSegmentPoint.FlyOver = true;
                pSegmentPoint.RadarGuidance = false;
                pSegmentPoint.ReportingATC = CodeATCReporting.NO_REPORT;
                pSegmentPoint.Waypoint = false;
                // =======================================================================

                pPointReference = new PointReference();

                // bOnNav = TurnWPT.TypeCode != eNavaidType.CodeNONE;
                ////if (TurnWPT.TypeCode != eNavaidClass.CodeNONE)
                //if (!CheckBox601.Checked)
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
                //IntersectNav = GuidNav;
                //pInterNavSignPt = pGuidNavSignPt;
                //pInterNavSignPt;

                if (TurnDirector.TypeCode != eNavaidType.NONE)
                {
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
                    pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.ALTITUDE;

                pSegmentPoint.PointChoice = TurnDirector.GetSignificantPoint();
                pSegmentLeg.EndPoint = pEndPoint;
            }

            // End Of EndPoint ========================
            // pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT;
            // =======================================================================================
            IPointCollection mPoly = (IPointCollection)Functions.CalcTrajectoryFromMultiPoint(MPtCollection);


            //Functions.DrawPolyline(mPoly, -1, 2);
            //Functions.DrawPointWithText(TurnDirector.pPtPrj, "LOSEN");
            //Application.DoEvents();

            if (!HaveIntercept)
            {
                IPoint ptCnt;

                if (OptionButton601.Checked)
                {
                    IPoint ptTmp = MPtCollection.Point[MPtCollection.PointCount - 1];
                    Functions.CircleVectorIntersect(GlobalVars.CurrADHP.pPtPrj, GlobalVars.RModel, ptTmp, ptTmp.M, out ptCnt);
                }
                else if (OptionButton602.Checked)
                    ptCnt = TerFixPnt;
                else
                    ptCnt = TurnDirector.pPtPrj;

                mPoly.AddPoint(ptCnt);
            }

            //Functions.DrawPolyline(mPoly, -1, 2);
            //Application.DoEvents();

            IPolyline pPoly = (IPolyline)mPoly;

            //  Length =====================================================
            pDistance = new ValDistance();
            pDistance.Uom = mUomHDistance;

            pDistance.Value = Functions.ConvertDistance(pPoly.Length, eRoundMode.NEAREST);
            pSegmentLeg.Length = pDistance;
            //  Trajectory =====================================================

            //Functions.DrawPolyline(pPoly, -1, 2);
            //Application.DoEvents();

            IGeometryCollection pPolyline = (IGeometryCollection)pPoly;

            Curve pCurve = new Curve();

            for (int j = 0; j < pPolyline.GeometryCount; j++)
            {
                IPointCollection pPath = (IPointCollection)pPolyline.Geometry[j];
                LineString pLineStringSegment = new LineString();

                for (int i = 0; i < pPath.PointCount; i++)
                {
                    Aran.Geometries.Point pLocation = Converters.ESRIPointToARANPoint((IPoint)Functions.ToGeo(pPath.Point[i]));

                    //Functions.DrawPointWithText(pPath.Point[i], (i + 1).ToString());
                    //Application.DoEvents();

                    pLineStringSegment.Add(pLocation);
                }
                pCurve.Geo.Add(pLineStringSegment);
            }
            pSegmentLeg.Trajectory = pCurve;

            //===========================================================
            // I protected Area =======================================================
            IPolygon pPolygon1;

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

            pTopo = (ITopologicalOperator2)pCircle;
            pPolygon1 = (IPolygon)pTopo.Intersect(pPolygon1, esriGeometryDimension.esriGeometry2Dimension);
            pTopo = (ITopologicalOperator2)pPolygon1;
            pTopo.IsKnownSimple_2 = false;
            pTopo.Simplify();

            //Functions.DrawPolygon(pCircle, -1, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSHorizontal);
            //Functions.DrawPolygon(pPolygon1, -1, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal);
            //Functions.DrawPolygon(SecPoly, -1, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal);
            ////while (true)
            //Application.DoEvents();

            ObstacleAssessmentArea pPrimProtectedArea = new ObstacleAssessmentArea();
            pPrimProtectedArea.Surface = Converters.ESRIPolygonToAIXMSurface(Functions.ToGeo(pPolygon1 as IGeometry) as IPolygon);
            pPrimProtectedArea.SectionNumber = 0;

            if (!OptionButton301.Checked)
            {
                pCurve = new Curve();
                MultiLineString mls = Converters.ESRIPolylineToARANPolyline((IPolyline)Functions.ToGeo((IGeometry)KK));
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
                pTopo = (ITopologicalOperator2)pCircle;
                IPolygon pPolygon2 = (IPolygon)pTopo.Intersect(SecPoly, esriGeometryDimension.esriGeometry2Dimension);
                pTopo = (ITopologicalOperator2)pPolygon2;
                pTopo.IsKnownSimple_2 = false;
                pTopo.Simplify();

                pSecProtectedArea = new ObstacleAssessmentArea();
                pSecProtectedArea.Surface = Converters.ESRIPolygonToAIXMSurface(Functions.ToGeo(pPolygon2 as IGeometry) as IPolygon);
                pSecProtectedArea.SectionNumber = 1;
                pSecProtectedArea.Type = CodeObstacleAssessmentSurface.SECONDARY;
                //pPrimProtectedArea.SurfaceZone = CodeObstructionIdSurfaceZone.PRIMARY;

                //pSecProtectedArea.StartingCurve = pCurve;

                if (!OptionButton301.Checked)
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

            //for (int i = 0; i < ostacles.Obstacles.Length; i++)
            //{
            //	Obstruction obs = new Obstruction();
            //	obs.VerticalStructureObstruction = new FeatureRef(ostacles.Obstacles[i].Identifier);

            //	double RequiredClearance = 0;
            //	int isPrimary = 0;

            //	for (int j = 0; j < ostacles.Obstacles[i].PartsNum; j++)
            //	{

            //		//MinimumAltitude = Math.Max(MinimumAltitude, ostacles.Parts[ostacles.Obstacles[i].Parts[j]].ReqH);
            //		RequiredClearance = Math.Max(RequiredClearance, ostacles.Parts[ostacles.Obstacles[i].Parts[j]].MOC);

            //		if (ostacles.Parts[ostacles.Obstacles[i].Parts[j]].Prima)			isPrimary |= 1;
            //		else																isPrimary |= 2;
            //	}

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

            //	if ((isPrimary & 1) != 0)
            //		pPrimProtectedArea.SignificantObstacle.Add(obs);

            //	if (pSecProtectedArea != null && (isPrimary & 2) != 0)
            //		pSecProtectedArea.SignificantObstacle.Add(obs);
            //}

            //  END ======================================================

            pSegmentLeg.DesignSurface.Add(pPrimProtectedArea);
            if (pSecProtectedArea != null)
                pSegmentLeg.DesignSurface.Add(pSecProtectedArea);

            //  END of MissedApproachTermination =================================================
            return result;
        }

        private DepartureLeg DepartureTerminationContinued(ref StandardInstrumentDeparture pProcedure, AircraftCharacteristic IsLimitedTo, ref TerminalSegmentPoint pEndPoint)
        {
            DepartureLeg result;
            SegmentLeg pSegmentLeg;

            AngleIndication pAngleIndication;
            Feature pFixDesignatedPoint;

            //TerminalSegmentPoint pEndPoint;
            SegmentPoint pSegmentPoint;

            ValDistance pDistance;
            ValDistanceVertical pDistanceVertical;
            ValSpeed pSpeed;

            UomDistance mUomHDistance;
            UomDistanceVertical mUomVDistance;
            UomSpeed mUomSpeed;

            UomDistance[] uomDistHorzTab;
            UomDistanceVertical[] uomDistVerTab;
            UomSpeed[] uomSpeedTab;

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

            result = DBModule.pObjectDir.CreateFeature<DepartureLeg>();
            result.Departure = pProcedure.GetFeatureRef();
            result.AircraftCategory.Add(IsLimitedTo);
            pSegmentLeg = result;

            GuidNav = Navaids_DataBase.WPT_FIXToNavaid(TurnDirector);
            IntersectNav = TerInterNavDat[ComboBox902.SelectedIndex];

            pInterceptPt = MPtCollection.Point[MPtCollection.PointCount - 1];

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

            pSegmentLeg.Course = NativeMethods.Modulus(double.Parse(TextBox602.Text) + GuidNav.MagVar, 360.0);

            //  LowerLimitAltitude ========================================================
            pDistanceVertical = new ValDistanceVertical();
            pDistanceVertical.Uom = mUomVDistance;

            pDistanceVertical.Value = double.Parse(TextBox916.Text);
            pSegmentLeg.LowerLimitAltitude = pDistanceVertical;
            //  UpperLimitAltitude ========================================================

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

            //  VerticalAngle ==========================================================
            pSegmentLeg.VerticalAngle = Functions.RadToDeg(System.Math.Atan(TACurrPDG));
            //  SpeedLimit ======
            pSpeed = new ValSpeed();
            pSpeed.Uom = mUomSpeed;

            pSpeed.Value = double.Parse(TextBox301.Text);
            pSegmentLeg.SpeedLimit = pSpeed;
            pSegmentLeg.SpeedReference = CodeSpeedReference.IAS;

            // Start Point ========================
            //pSegmentLeg.StartPoint = pEndPoint;
            // End Of Start Point ========================

            pGuidNavSignPt = GuidNav.GetSignificantPoint();

            //  EndPoint ========================
            pEndPoint = new TerminalSegmentPoint();

            //         pTerminalSegmentPoint.IndicatorFACF =      ??????????
            //         pTerminalSegmentPoint.LeadDME =            ??????????
            // pTerminalSegmentPoint.LeadRadial =         ??????????
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

                //Angle = NativeMethods.Modulus((double)pSegmentLeg.Course + 90.0 - IntersectNav.MagVar, 360.0);
                //pStAngleIndication = DBModule.CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pFIXSignPt);
                //pStAngleIndication.TrueAngle = NativeMethods.Modulus((double)pSegmentLeg.Course + 90.0, 360.0);

                //pStAngleIndication.Fix = pFIXSignPt.GetFeatureRef();

                //pAngleUse = new AngleUse();
                //pAngleUse.TheAngleIndication = pStAngleIndication.GetFeatureRef();
                //pAngleUse.AlongCourseGuidance = false;
                //// ========================
                //pPointReference.FacilityAngle.Add(pAngleUse);
                //pPointReference.Role = CodeReferenceRole.INTERSECTION;
            }
            else
            {
                double horAccuracy = 0.0;

                if (GuidNav.Identifier != Guid.Empty && GuidNav.TypeCode != eNavaidType.NONE && IntersectNav.TypeCode != eNavaidType.NONE)
                    horAccuracy = Functions.CalcHorisontalAccuracy(TerFixPnt, GuidNav, IntersectNav);

				fDir = Functions.Azt2DirPrj(TerFixPnt, (double)pSegmentLeg.Course);
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
                    pAngleUse = new AngleUse();
                    fDir = Functions.ReturnAngleInDegrees(IntersectNav.pPtPrj, TerFixPnt);

                    Angle = NativeMethods.Modulus(Functions.Dir2Azt(IntersectNav.pPtPrj, fDir) - IntersectNav.MagVar, 360.0);
                    AngleIndication pStAngleIndication = DBModule.CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pInterNavSignPt);
                    pStAngleIndication.TrueAngle = Functions.Dir2Azt(TerFixPnt, fDir);

                    pStAngleIndication.Fix = pFIXSignPt.GetFeatureRef();
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
            //  End Of EndPoint ========================

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
            //  END of Trajectory =====================================================
            return result;
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

            FeatureRef featureRef = new FeatureRef(GlobalVars.CurrADHP.Identifier);

            FeatureRefObject featureRefObject = new FeatureRefObject();
            featureRefObject.Feature = featureRef;
            _Procedure.AirportHeliport.Add(featureRefObject);

            //pGuidanceServiceChose.Navaid = FinalNav.pFeature.GetFeatureRef();
            //_Procedure.GuidanceFacility.Add(pGuidanceServiceChose);

               // Must be FIX Name

            //if (CheckBox201.Checked)
            //{
            //    if (OptionButton601.Checked)
            //        sProcName = "VM" + " RWY" + DER.Name;
            //    else
            //        sProcName = "F" + TextBox916.Text + " RWY" + DER.Name;
            //}
            //else
            //    sProcName = "VA" + " RWY" + DER.Name;// .Identifier;

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
            if (CheckBox201.Checked)
            {
                // Leg 2 Standart Departure ===============================================================================
                NO_SEQ++;
                pSegmentLeg = TurningDepartureLeg(ref _Procedure, IsLimitedTo, ref pEndPoint);

                ptl = new ProcedureTransitionLeg();
                ptl.SeqNumberARINC = NO_SEQ;
                ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef<AbstractSegmentLegRef>();
                pTransition.TransitionLeg.Add(ptl);

                // Leg 3 Standart Departure ===============================================================================
                if (OptionButton602.Checked)
                {
                    NO_SEQ++;
                    pSegmentLeg = DepartureTerminationContinued(ref _Procedure, IsLimitedTo, ref pEndPoint);

                    ptl = new ProcedureTransitionLeg();
                    ptl.SeqNumberARINC = NO_SEQ;
                    ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef<AbstractSegmentLegRef>();
                    pTransition.TransitionLeg.Add(ptl);
                }
            }

            //=============================================================================
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
