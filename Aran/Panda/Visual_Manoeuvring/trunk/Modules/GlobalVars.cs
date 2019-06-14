using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Win32;

using Aran.Package;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Geometries.SpatialReferences;
using Aran.AranEnvironment;
using Aran.Panda.Common;
using Aran.Panda.Constants;
using Aran.Panda.VisualManoeuvring.Properties;
using Aran.Aim.Features;

namespace Aran.Panda.VisualManoeuvring
{
    [System.Runtime.InteropServices.ComVisible(false)]
    public static class GlobalVars
    {
        #region "Public Constants"
        #region "PANS-OPS Constants"    '   Срочно: внести в базу PANSOPS

        //   Срочно: внести в базу ArPANSOPS
        public const double THRAccuracy = 259.28;
        public const double arInitApprBank = 25.0;
        public const double arInitialApTurnRadius = 3704.0; //2 NM

        public const double ArPANSOPS_MaxNavDist = 46000.0;
        public const double OASZOrigin = 900.0;
        public const double arHOASPlaneCat1 = 300.0;
        public const double arHOASPlaneCat23 = 150.0;
        //public const double[] EnrouteMOCValues = new double[] { 300.0, 450.0, 600.0 };

        public const double SIDTerminationFIXToler = 5000.0;

        public static Constants.Constants constants = null;
        #endregion

        #region "Product releated"
        public const string ModuleName = "Arrival";
        public const string PANSOPSVersion = "DOC 8168 OPS/611    Sixth Edition - 2014";
        public const string ModuleRegKey = RegFuncs.Conventional;
        public const string Acar = "Acar";

        public const string HelpFile = "PANDA.chm";
        public const int ReportHelpIDRNAVDeparture = 0;
        #endregion

        #region "Math releated"
        public const double PI = 3.14159265358979;
        public const double DegToRadValue = PI / 180.0;

        public const double RadToDegValue = 180.0 / PI;
        public const double mEps = 1E-12;
        public const double distEps = 0.0001;
        public const double PDGEps = 0.0001;

        public const double degEps = 1.0 / 36000.0;
        public const double radEps = degEps * DegToRadValue;

        public const double NMCoeff = 1.852;
        public const double FootCoeff = 0.3048;
        #endregion

        #region "Model releated"
        public const short NO_VALUE = -9999;
        public const double MaxModelRadius = 150000.0;
        public const double MaxILSDist = 20000.0;
        public const double MaxNAVDist = 200000.0;

        public const double OffsetTreshold = 1.0;
        public const double MinGPIntersectHeight = 55.0;
        public const double MaxGPIntersectHeight = 150.0;
        public const double LocOffsetOCHAdd = 20.0;
        public const double MaxRefGPAngle = 3.5;
        #endregion

        #region "Neutral Language"
        public const short NeutralLangCode = 1033;
        #endregion

        #region "MS Windows"
        public const int GWL_WNDPROC = -4;
        public const int GWL_HINSTANCE = -6;
        public const int GWL_HWNDPARENT = -8;
        public const int GWL_STYLE = -16;
        public const int GWL_EXSTYLE = -20;
        public const int GWL_USERDATA = -21;

        public const int GWL_ID = -12;
        // select last opened tab, [display a specified topic]
        public const int HH_DISPLAY_TOPIC = 0x0;
        // select contents tab, [display a specified topic]
        public const int HH_DISPLAY_TOC = 0x1;
        // select index tab and searches for a keyword
        public const int HH_DISPLAY_INDEX = 0x2;
        // select search tab and perform a search
        public const int HH_DISPLAY_SEARCH = 0x3;
        public const int HH_SET_WIN_TYPE = 0x4;
        public const int HH_GET_WIN_TYPE = 0x5;
        public const int HH_GET_WIN_HANDLE = 0x6;
        // Display string resource ID or text in a pop-up window.
        public const int HH_DISPLAY_TEXT_POPUP = 0xe;
        // Display mapped numeric value in dwData.
        public const int HH_HELP_CONTEXT = 0xf;
        // Text pop-up help, similar to WinHelp's HELP_CONTEXTMENU.
        public const int HH_TP_HELP_CONTEXTMENU = 0x10;
        // text pop-up help, similar to WinHelp's HELP_WM_HELP.
        public const int HH_TP_HELP_WM_HELP = 0x11;
        public const int HH_CLOSE_ALL = 0x12;
        #endregion

        #endregion

        #region Public Variables

        #region "Public Public"
        public static string InstallDir;
        public static int CurrCmd = -1;
        //public static string UserName;
        public static MultiPolygon p_LicenseRect;
        //public static IPandaAranExtension PandaExt;
        #endregion

        #region "Visibility manipulation"
        public static bool ButtonControl1State;
        public static bool ButtonControl2State;
        public static bool ButtonControl3State;
        public static bool ButtonControl4State;
        public static bool ButtonControl5State;
        public static bool ButtonControl6State;
        public static bool ButtonControl7State;
        public static bool ButtonControl8State;

        public static int FIXElem;
        public static int CLElem;
        public static int pCircleElem;
        public static int StraightAreaFullElem;
        public static int StraightAreaPrimElem;

        public static int p120LineElem;
        public static int TurnAreaElem;

        public static int PrimElem;
        public static int SecRElem;
        public static int SecLElem;
        public static int StrTrackElem;
        public static int NomTrackElem;

        public static int KKElem;
        public static int K1K1Elem;
        public static int TerminationFIXElem;

        public static int TurnAreaElemColor = (int)ARANFunctions.RGB(255, 0, 0);
        public static int PrimElemColor = (int)ARANFunctions.RGB(255, 255, 0);
        public static int SecRElemColor = (int)ARANFunctions.RGB(0, 0, 255);
        public static int SecLElemColor = (int)ARANFunctions.RGB(0, 0, 255);
        public static int StrTrackElemColor = (int)ARANFunctions.RGB(0, 0, 0);
        public static int NomTrackElemColor = (int)ARANFunctions.RGB(0, 0, 0);
        public static int KKElemColor = (int)ARANFunctions.RGB(0, 0, 0);
        public static int K1K1ElemColor = (int)ARANFunctions.RGB(0, 0, 0);
        #endregion

        #region "Display && units managment"

        #endregion

        #region "Document, projection, e.t.c."

        private static bool _errorHandled = false;

        public static Aran.Panda.Common.Settings settings;
        public static Aran.Panda.Common.UnitConverter unitConverter;
        public static NavaidsConstant navaidsConstants;
        public static PansOpsCoreDatabase pansopsCoreDatabase;

        //public static IHookHelper gHookHelper;
        public static IAranEnvironment gAranEnv;
        public static IAranGraphics gAranGraphics;

        public static double ModellingRadius = 150000;
        public static double RModel;

        //public static IMap pMap;
        //public static IGeographicCoordinateSystem pGCS;
        //public static IProjectedCoordinateSystem pPCS;

        public static SpatialReferenceOperation pspatialReferenceOperation;
        public static SpatialReference pSpRefPrj;
        public static SpatialReference pSpRefShp;
        public static Ellipsoid pSpheroid;

        #endregion

        #region "DB"
        public static ADHPType CurrADHP;
        public static RWYType[] RWYList;
        public static ObstacleType[] ObstacleList;
        public static VerticalStructure[] VerticalStructureList;

        public static NavaidType[] NavaidList;
        public static NavaidType[] DMEList;

        public static WPT_FIXType[] WPTList;
        #endregion

        #endregion

        public static IWin32Window Win32Window
        {
            get { return gAranEnv.Win32Window; }
        }

        public static void InitCommand()
        {
            HandleThreadException();

            InstallDir = RegFuncs.RegRead<string>(Registry.CurrentUser, RegFuncs.Panda, RegFuncs.InstallDirKeyName, null);
            if (InstallDir == null)
                throw new Exception("Intallation Path Not Exists.");

            ARANFunctions.InitEllipsoid();

            settings = new Aran.Panda.Common.Settings();
            settings.Load(gAranEnv);

            ModellingRadius = settings.Radius;
            unitConverter = new Aran.Panda.Common.UnitConverter(settings);
            Resources.Culture = Thread.CurrentThread.CurrentUICulture;

            //'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...

            if (constants == null)
                constants = new Constants.Constants();

            navaidsConstants = constants.NavaidConstants;
            pansopsCoreDatabase = constants.PansopsCoreDB;

            //'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...

            pspatialReferenceOperation = new SpatialReferenceOperation(gAranEnv);
            pSpRefPrj = pspatialReferenceOperation.SpRefPrj;
            pSpRefShp = pspatialReferenceOperation.SpRefGeo;

            //'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...

            string Achar = RegFuncs.RegRead<String>(Registry.CurrentUser, ModuleRegKey + "\\" + ModuleName, RegFuncs.LicenseKeyName, "");
            p_LicenseRect = DecoderCode.DecodeLCode(Achar, ModuleName);

            if (DecoderCode.LstStDtWriter(ModuleName) != 0)
                throw new Exception("CRITICAL ERROR!!!");

            if (p_LicenseRect.Count == 0)
                throw new Exception("Empty region");

            //'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...

			DBModule.InitModule();
			
			CurrADHP.Identifier = settings.Aeroport;
            CurrADHP.pPtGeo = null;
            DBModule.FillADHPFields(ref CurrADHP);

            if (CurrADHP.pPtGeo == null)
                throw new Exception("Initialization of ADHP failed.");

            //    '==============================================================

            //if (!PANS_OPS_DataBase.InitModule) {
            //    MsgBox(ErrorStr, MsgBoxStyle.Critical, "PANS-OPS DataBase");
            //    return -1;
            //}

            //if(!Navaids_DataBase.InitModule) {
            //    MsgBox(ErrorStr, MsgBoxStyle.Critical, "Navaids DataBase");
            //    return -1;
            //}

            //if(!Categories_DATABase.InitModule) {
            //    MsgBox(ErrorStr, MsgBoxStyle.Critical, "Categories DataBase");
            //    return -1;
            //}

            //if (!PANS_OPS_Core_DataBase.InitModule) {
            //    MsgBox(ErrorStr, MsgBoxStyle.Critical, "PANS-OPS Core DataBase");
            //    return -1;
            //}

            //'=============================================================


            DBModule.FillRWYList(out RWYList, CurrADHP);
            //DBModule.FillWPT_FIXList(out WPTList, CurrADHP, MaxNAVDist);   //!!! Do I need this in Visual Manoeuvring???
            DBModule.FillNavaidList(out NavaidList, out DMEList, CurrADHP, MaxNAVDist);

            //DBModule.GetObstaclesByDist(out ObstacleList, CurrADHP.pPtPrj, ModellingRadius);
        }

        

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show(e.Exception.Message, "Error",
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Error);
        }

        private static void HandleThreadException()
        {
            if (_errorHandled)
                return;
            _errorHandled = true;
            System.Windows.Forms.Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
        }
    }
}
