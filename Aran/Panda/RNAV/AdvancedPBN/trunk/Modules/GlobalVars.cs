// #define checkLicenze
using System;
using System.Threading;
using System.Windows.Forms;
using Aran.AranEnvironment;
using Aran.Geometries;
using Aran.Geometries.SpatialReferences;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
//using Aran.PANDA.RNAV.SGBAS.Properties;
using Microsoft.Win32;

namespace Aran.PANDA.RNAV.SGBAS
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public static class GlobalVars
	{
		#region "Public Constants"

		#region "Product releated"
		public const string PANSOPSVersion = "DOC 8168 OPS/611    Sixth Edition, Amendments 1 to 6 incorporated - 2016";
		public const string ModuleName = "AdvancedPBN";
		//public const string ModuleCategory = "RNAV";

		public const string HelpFile = "PANDA.chm";
		public const int ReportHelpIDRNAVDeparture = 0;
		#endregion

		#region "PANS-OPS Constants"    '   Срочно: внести в базу PANSOPS
		public static Constants.Constants constants = null;

		public const double SIDTerminationFIXToler = 5000.0;
		public const double SBASTransitionDistance = 2.0 * 1852.0;
		public const double SBASWidth = 1759.4;
		public const double MATMinRange = 25.0 * 1852.0;
		
		public const double THRAccuracy = 259.28;
		public const double arInitApprBank = 25.0;
		public const double arInitialApTurnRadius = 2500.0; //3704.0 //2 NM


		public const double ArPANSOPS_MaxNavDist = 46000.0;
		//public const double OASZOrigin = 900.0;
		public const double Cat1OASZOrigin = 900.0;

		public const double arHOASPlaneCat1 = 300.0;
		public const double arHOASPlaneCat23 = 150.0;
		public static double[] EnrouteMOCValues = new double[] { 300.0, 450.0, 600.0 };
		#endregion

		public const double f05 = 0.087266462599716;	//ARANMath.DegToRad(15.0);
		public const double f15 = 0.26179938779914941;	//ARANMath.DegToRad(15.0);
		public const double f120 = 2.1118483949131388;	//ARANMath.DegToRad(121);
		public const double f270 = 4.7123889803846897;	//ARANMath.DegToRad(270);


		//#region "Math releated"
		//public const double PI = 3.14159265358979;
		//public const double DegToRadValue = PI / 180.0;

		//public const double RadToDegValue = 180.0 / PI;
		//public const double mEps = 1E-12;
		//public const double distEps = 0.0001;
		//public const double PDGEps = 0.0001;

		//public const double degEps = 1.0 / 36000.0;
		//public const double radEps = degEps * DegToRadValue;

		//public const double NMCoeff = 1.852;
		//public const double FootCoeff = 0.3048;
		//#endregion

		#region "Model releated"
		public const short NO_VALUE = -9999;
		public const double MaxModelRadius = 150000.0;
		public const double MaxILSDist = 20000.0;
		public const double MaxNAVDist = 200000.0;

		public const double OffsetTreshold = 1.0;
		public const double MinGPIntersectHeight = 55.0;
		public const double MaxGPIntersectHeight = 150.0;
		public const double LocOffsetOCHAdd = 20.0;
		public const double MaxRefGPAngle = 3.5 * Math.PI / 180.0;
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
		// System menu
		public const int WM_SYSCOMMAND = 0x112;
		public const int MF_STRING = 0x0;
		public const int MF_SEPARATOR = 0x800;
		public const int SYSMENU_ABOUT_ID = 0x1;
		#endregion

		#endregion

		#region "Public Variables"

		#region "Public Public"
		public static string ConstDir;

		public static MultiPolygon p_LicenseRect;
		public static string UserName;		//public static string UserName;
		#endregion

		#region "Visibility manipulation"
		public static ToolbarForm VisibilityBar;

		//public static bool ButtonControl1State;
		//public static bool ButtonControl2State;
		//public static bool ButtonControl3State;
		//public static bool ButtonControl4State;
		//public static bool ButtonControl5State;
		//public static bool ButtonControl6State;
		//public static bool ButtonControl7State;
		//public static bool ButtonControl8State;


		//public static int CLElem;
		//public static int pCircleElem;
		//public static int StraightAreaFullElem;
		//public static int StraightAreaPrimElem;

		//public static int p120LineElem;
		//public static int StrTrackElem;

		//public static int KKElem;
		//public static int K1K1Elem;
		//public static int TerminationFIXElem;

		//public static int TurnAreaElem;
		//public static int NomTrackElem;
		//public static int PrimElem;
		//public static int SecRElem;
		//public static int SecLElem;
		//public static int FIXElem;


		public static int TurnAreaElemColor = ARANFunctions.RGB(255, 0, 0);
		
		public static int PrimElemColor = ARANFunctions.RGB(255, 0, 255);
		public static int SecRElemColor = ARANFunctions.RGB(0, 255, 0);
		public static int SecLElemColor = ARANFunctions.RGB(0, 0, 255);

		public static int StrTrackElemColor = ARANFunctions.RGB(0, 0, 0);
		public static int NomTrackElemColor = ARANFunctions.RGB(0, 0, 0);
		public static int KKElemColor = ARANFunctions.RGB(0, 0, 0);
		public static int K1K1ElemColor = ARANFunctions.RGB(0, 0, 0);

		public static int WPTColor = ARANFunctions.RGB(192, 127, 192);
		public static int ObstColor = ARANFunctions.RGB(255, 127, 127);
		//public const int ptColor = 0x1FFF1F;

		//public ArrivalToolbar CommandBar = null;

		public static int[] SBASOASPlanesElement = new int[9];
		public static bool SBASOASPlanesState;

		public static int[] OASPlanesCat1Element = new int[9];
		public static bool OASPlanesCat1State;

		//public static int[] OASPlanesCat23Element = new int[9];
		//public static bool OASPlanesCat23State;

		public static int[] ILSPlanesElement = new int[13];
		public static bool ILSPlanesState;

		public static int[] OFZPlanesElement = new int[8];
		public static bool OFZPlanesState;

		#endregion

		#region "Display && units managment"

		#endregion

		#region "Document, projection, e.t.c."

		private static bool _errorHandled = false;

		public static Settings settings;
		public static Aran.PANDA.Common.UnitConverter unitConverter;
		public static NavaidsConstant navaidsConstants;

		//public static IHookHelper gHookHelper;
		public static IAranEnvironment gAranEnv;
		public static IAranGraphics gAranGraphics;

		public static double ModellingRadius = 150000;
		public static double RModel;

		//public static IMap pMap;
		//public static IGeographicCoordinateSystem pGCS;
		//public static IProjectedCoordinateSystem pPCS;
		public static double MeanEarthRadius;

		public static SpatialReferenceOperation pspatialReferenceOperation;
		public static SpatialReference pSpRefPrj;
		public static SpatialReference pSpRefGeo;
		public static Ellipsoid pEllipsoid;

		#endregion

		#region "DB"
		public static ADHPType CurrADHP;
		public static RWYType[] RWYList;
		public static ObstacleContainer ObstacleList;

		public static NavaidType[] NavaidList;
		public static NavaidType[] DMEList;
		public static WPT_FIXType[] WPTList;
		#endregion

		#endregion

		#region "OAS & ILS planes"
		public static D3DPolygone[] SBASOASPlanes = new D3DPolygone[9];
		public static D3DPolygone[] wOASPlanes = new D3DPolygone[9];		//OASPlanesCat1
		//public static D3DPolygone[] OASPlanesCat23 = new D3DPolygone[9];
		public static D3DPolygone[] OFZPlanes = new D3DPolygone[8];
		public static D3DPolygone[] ILSPlanes = new D3DPolygone[13];

		public static string[] OASPlaneNames = new string[] { "Zero", "W", "X", "Y", "Z", "Y", "X", "W*", "Common", "Non Prec." };
		public static string[] OFZPlaneNames = new string[] { "Zero", "Inner Approach", "Inner transitional1", "Inner transitional2", "Balking landing", "Inner transitional2", "Inner transitional1", "Common" };
		public static string[] ILSPlaneNames = new string[] { "Zero", "Approach 1", "Approach 2", "Transitional A", "Transitional B", "Transitional C", "Transitional D", "Missed Approach", "Transitional D", "Transitional C", "Transitional B", "Transitional A", "Common" };
		public static string[] RNPARPlaneNames = new string[] { "Zero", "Approach", "Missed Approach"};

		#endregion

		public static IWin32Window Win32Window
		{
			get { return gAranEnv.Win32Window; }
		}

		public static void InitCommand()
		{
            //HandleThreadException();

            bool isExists;
            ConstDir = RegFuncs.GetConstantsDir(out isExists);

            if (!isExists)
                throw new Exception("Installation Path Not Exists.");

			settings = new Settings();
			settings.Load(gAranEnv);

			RModel = ModellingRadius = settings.Radius;

			unitConverter = new Aran.PANDA.Common.UnitConverter(settings);
			//Resources.Culture = Thread.CurrentThread.CurrentUICulture;

			//'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...

			if (constants == null)
				constants = new Constants.Constants();

			navaidsConstants = constants.NavaidConstants;

			//'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...

			pspatialReferenceOperation = new SpatialReferenceOperation(gAranEnv);
			pSpRefPrj = pspatialReferenceOperation.SpRefPrj;
			pSpRefGeo = pspatialReferenceOperation.SpRefGeo;
			pEllipsoid = pSpRefGeo.Ellipsoid;
			ARANFunctions.InitEllipsoid(pEllipsoid.SemiMajorAxis, 1.0 / pEllipsoid.Flattening);

			MeanEarthRadius = Math.Sqrt(1.0 - pEllipsoid.Flattening) * pEllipsoid.SemiMajorAxis;

			//'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...
#if checkLicenze
			string Achar = RegFuncs.GetRNAVAcar(ModuleName);

			p_LicenseRect = DecoderCode.DecodeLCode(Achar, ModuleName);

			if (DecoderCode.LstStDtWriter(Achar, ModuleName) != 0)  //RegFuncs.RNAV, 
				throw new Exception("CRITICAL ERROR!!!");

			if (p_LicenseRect.Count == 0)
			    throw new Exception("Empty region");
#endif
			//'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...

			CurrADHP.Identifier = settings.Aeroport;
			CurrADHP.pPtGeo = null;

			DBModule.InitModule();
			DBModule.FillADHPFields(ref CurrADHP);

			if (CurrADHP.pPtGeo == null)
				throw new Exception("Initialization of ADHP failed.");

			DBModule.FillRWYList(out RWYList, CurrADHP);

			if (RWYList.Length == 0)
				throw new Exception("RWY list is empty.");

			DBModule.FillNavaidList(out NavaidList, out DMEList, CurrADHP, settings.Radius);
			DBModule.FillWPT_FIXList(out WPTList, CurrADHP, settings.Radius);

			//double maxRange = Functions.CalcMaxRadius() + ModellingRadius;
			//DBModule.GetObstaclesByDist(out ObstacleList, CurrADHP.pPtPrj, settings.Radius);
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

		//public static string GetMapFileName()
		//{
		//    if (gHookHelper.Hook is ESRI.ArcGIS.Framework.IApplication)
		//    {
		//        ESRI.ArcGIS.Framework.IApplication app = (ESRI.ArcGIS.Framework.IApplication)gHookHelper.Hook;
		//        return app.Templates.get_Item(app.Templates.Count - 1);
		//    }
		//    else
		//        return gAranEnv.DocumentFileName;
		//}

	}
}
