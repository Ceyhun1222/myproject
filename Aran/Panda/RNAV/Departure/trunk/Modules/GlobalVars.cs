using System;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

using Aran.AranEnvironment;
using Aran.Geometries;
using Aran.Geometries.SpatialReferences;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
using Aran.PANDA.RNAV.Departure.Properties;

namespace Aran.PANDA.RNAV.Departure
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public static class GlobalVars
	{
		#region "Public Constants"
		#region "PANS-OPS Constants"	'	Срочно: внести в базу PANSOPS

		public const double SIDTerminationFIXToler = 5000.0;
		public static Constants.Constants constants = null;

		public const double MinBankAngle = 15;
		public const double MaxBankAngle = 25;
		//public const double MaxAltitude = 12800.0;
		public const double MaxAltitude = 12800.0;
		public const double NomLineGrd = 0.07;

		public const double f05 = ARANMath.DegToRadValue;	    //0.087266462599716;	//ARANMath.DegToRad(  5.0);
        public const double f15 = 0.26179938779914941;          //ARANMath.DegToRad( 15.0);
        public const double f120 = ARANMath.DegToRadValue * 135.0;    //2.1118483949131388;		//ARANMath.DegToRad(121.0);
		public const double f270 = 4.7123889803846897;		//ARANMath.DegToRad(270.0);

		#endregion

		#region "Product releated"
		public const string PANSOPSVersion = "DOC 8168 OPS/611    Sixth Edition, Amendments 1 to 6 incorporated - 2016";
		public const string ModuleName = "Departure";
		//public const string ModuleCategory = "RNAV";

		public const string HelpFile = "PANDA.chm";
		public const int ReportHelpIDRNAVDeparture = 0;
		#endregion

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
		//public static System.Reflection.AssemblyName thisAssemName;
		public static int CurrCmd = -1;
		public static string ConstDir;

		public static MultiPolygon p_LicenseRect;
		public static string UserName;
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

		public static int TurnAreaElemColor = ARANFunctions.RGB(255, 0, 0);
		public static int PrimElemColor = ARANFunctions.RGB(255, 255, 0);
		public static int SecRElemColor = ARANFunctions.RGB(0, 0, 255);
		public static int SecLElemColor = ARANFunctions.RGB(0, 0, 255);
		public static int StrTrackElemColor = ARANFunctions.RGB(0, 0, 0);
		public static int NomTrackElemColor = ARANFunctions.RGB(0, 0, 0);
		public static int KKElemColor = ARANFunctions.RGB(0, 0, 0);
		public static int K1K1ElemColor = ARANFunctions.RGB(0, 0, 0);
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

		public static SpatialReferenceOperation pspatialReferenceOperation;
		public static SpatialReference pSpRefPrj;
		public static SpatialReference pSpRefGeo;
		public static Ellipsoid pSpheroid;

		#endregion

		#region "DB"
		public static ADHPType CurrADHP;
		public static RWYType[] RWYList;

		//public static ObstacleType[] ObstacleList;
		public static ObstacleContainer GObstacleList;
		public static ObstacleContainer ObstacleList;

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
			//HandleThreadException();

            bool isExists;
            ConstDir = RegFuncs.GetConstantsDir(out isExists);
			
            if (!isExists)
				throw new Exception("Installation Path Not Exists.");

			//thisAssemName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
			ARANFunctions.InitEllipsoid();

			settings = new Settings();
			settings.Load(gAranEnv);

			ModellingRadius = settings.Radius;
			unitConverter = new Aran.PANDA.Common.UnitConverter(settings);
			Resources.Culture = Thread.CurrentThread.CurrentUICulture;

			//'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...

			if (constants == null)
				constants = new Constants.Constants();

			navaidsConstants = constants.NavaidConstants;

			//'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...

			pspatialReferenceOperation = new SpatialReferenceOperation(gAranEnv);
			pSpRefPrj = pspatialReferenceOperation.SpRefPrj;
			if (pSpRefPrj.SpatialReferenceType == SpatialReferenceType.srtGeographic)
				throw new Exception("Invalid Map projection.");

			pSpRefGeo = pspatialReferenceOperation.SpRefGeo;

			//'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...

			string Achar = RegFuncs.GetRNAVAcar(ModuleName);
			p_LicenseRect = DecoderCode.DecodeLCode(Achar, ModuleName);

			if (DecoderCode.LstStDtWriter(Achar, ModuleName) != 0)
				throw new Exception("CRITICAL ERROR!!!");

			if (p_LicenseRect.Count == 0)
				throw new Exception("Empty region");

			//'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...

			CurrADHP.Identifier = settings.Aeroport;
			CurrADHP.pPtGeo = null;

			DBModule.InitModule();
			DBModule.FillADHPFields(ref CurrADHP);

			if (CurrADHP.pPtGeo == null)
				throw new Exception("Initialization of ADHP failed.");

			DBModule.FillRWYList(out RWYList, CurrADHP);
			DBModule.FillWPT_FIXList(out WPTList, CurrADHP, settings.Radius);

			double maxRange = Functions.CalcMaxRadius() + ModellingRadius;
            if (settings.AnnexObstalce)
                DBModule.GetAnnexObstacle(out GObstacleList, CurrADHP.pPtPrj, CurrADHP.pAirportHeliport);
            else
                DBModule.GetObstaclesByDist(out GObstacleList, CurrADHP.pPtPrj, maxRange);//settings.Radius
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

/*
─────────────────────────▄▀▄  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─▀█▀█▄  
─────────────────────────█──█──█  
─────────────────────────█▄▄█──▀█  
────────────────────────▄█──▄█▄─▀█  
────────────────────────█─▄█─█─█─█  
────────────────────────█──█─█─█─█  
────────────────────────█──█─█─█─█  
────▄█▄──▄█▄────────────█──▀▀█─█─█  
──▄█████████────────────▀█───█─█▄▀  
─▄███████████────────────██──▀▀─█  
▄█████████████────────────█─────█  
██████████───▀▀█▄─────────▀█────█  
████████───▀▀▀──█──────────█────█  
██████───────██─▀█─────────█────█  
████──▄──────────▀█────────█────█ Look dude,
███──█──────▀▀█───▀█───────█────█ a good code!
███─▀─██──────█────▀█──────█────█  
███─────────────────▀█─────█────█  
███──────────────────█─────█────█  
███─────────────▄▀───█─────█────█  
████─────────▄▄██────█▄────█────█  
████────────██████────█────█────█  
█████────█──███████▀──█───▄█▄▄▄▄█  
██▀▀██────▀─██▄──▄█───█───█─────█  
██▄──────────██████───█───█─────█  
─██▄────────────▄▄────█───█─────█  
─███████─────────────▄█───█─────█  
──██████─────────────█───█▀─────█  
──▄███████▄─────────▄█──█▀──────█  
─▄█─────▄▀▀▀█───────█───█───────█  
▄█────────█──█────▄███▀▀▀▀──────█  
█──▄▀▀────────█──▄▀──█──────────█  
█────█─────────█─────█──────────█  
█────────▀█────█─────█─────────██  
█───────────────█──▄█▀─────────█  
█──────────██───█▀▀▀───────────█  
█───────────────█──────────────█  
█▄─────────────██──────────────█  
─█▄────────────█───────────────█  
──██▄────────▄███▀▀▀▀▀▄────────█  
─█▀─▀█▄────────▀█──────▀▄──────█  
─█────▀▀▀▀▄─────█────────▀─────█
*/