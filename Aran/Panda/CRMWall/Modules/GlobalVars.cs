using System;
using System.Windows.Forms;
using Aran.AranEnvironment;
using Aran.Geometries;
using Aran.Geometries.SpatialReferences;
using Aran.PANDA.Common;
using Microsoft.Win32;

namespace Aran.PANDA.CRMWall
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public static class GlobalVars
	{
		#region "Public Constants"

		#region "Product releated"
		public const string PANSOPSVersion = "DOC 8168 OPS/611    Sixth Edition, Amendments 1 to 6 incorporated - 2016";
		public const string PandaRegKey = "SOFTWARE\\RISK\\PANDA";
		public const string ModuleRegKey = PandaRegKey + "\\Conventional";
		public const string ModuleName = "CRMObstacle";
		public const string LicenseKeyName = "Acar";
		public static RegistryKey PANDARootKey = Registry.CurrentUser;

		public const string HelpFile = "PANDA.chm";
		public const short ReportHelpIDCRMWall = 0;
		#endregion

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
		public static System.Reflection.AssemblyName thisAssemName;
		public static int CurrCmd = 0;
		public static string ConstDir;
		public static MultiPolygon p_LicenseRect;

		public static string UserName;
		public static int LangCode;
		#endregion

		#region "Visibility manipulation"
		public static bool ButtonControl1State;
		public static bool ButtonControl2State;
		public static int pAreaElem;
		public static int ZeroLineElem;
		public static int CLElem;
		public static int AreaElemColor = ARANFunctions.RGB(255, 0, 0);
		#endregion

		#region "Document, projection, e.t.c."
		//private static bool _errorHandled = false;

		public static Settings settings;
		public static Aran.PANDA.Common.UnitConverter unitConverter;
		//public static NavaidsConstant navaidsConstants;

		public static Aran.AranEnvironment.IAranEnvironment gAranEnv;
		public static IAranGraphics gAranGraphics;

		public static double ModellingRadius = 150000.0;
		public static double RModel;

		public static SpatialReferenceOperation pspatialReferenceOperation;
		public static SpatialReference pSpRefPrj;
		public static SpatialReference pSpRefShp;
		public static Ellipsoid pSpheroid;
		#endregion

		#region "DB"
		public static ADHPType CurrADHP;
		public static RWYType[] RWYList;
		public static ObstacleContainer ObstacleList;
		//public static NavaidType[] NavaidList;
		//public static NavaidType[] DMEList;
		//public static WPT_FIXType[] WPTList;
		#endregion

		public static IWin32Window Win32Window { get { return gAranEnv.Win32Window; } }

		#endregion

		public static void InitCommand()
		{
			//HandleThreadException();
			//ConstDir = RegFuncs.RegRead<string>(Registry.CurrentUser, RegFuncs.Panda, RegFuncs.ConstKeyName, null);

            bool isExists;
            ConstDir = RegFuncs.GetConstantsDir(out isExists);

            if (!isExists)
				throw new Exception("Intallation Path Not Exists.");

			thisAssemName = System.Reflection.Assembly.GetExecutingAssembly().GetName();

			ARANFunctions.InitEllipsoid();

			settings = new Settings();
			settings.Load(gAranEnv);

			ModellingRadius = settings.Radius;
			unitConverter = new Aran.PANDA.Common.UnitConverter(settings);
			//Resources.Culture = Thread.CurrentThread.CurrentUICulture;

			//'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...

			//if (constants == null)
			//    constants = new Constants();
			//navaidsConstants = constants.NavaidConstants;

			//'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...

			pspatialReferenceOperation = new SpatialReferenceOperation(gAranEnv);
			pSpRefPrj = pspatialReferenceOperation.SpRefPrj;
			pSpRefShp = pspatialReferenceOperation.SpRefGeo;

			//'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...
            string Achar = RegFuncs.GetConventionalAcar(ModuleName);

			p_LicenseRect = DecoderCode.DecodeLCode(Achar, ModuleName);

			if (DecoderCode.LstStDtWriter(Achar, RegFuncs.Conventional, ModuleName) != 0)
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

			double maxRange = Functions.CalcMaxRadius() + ModellingRadius;
			DBModule.GetObstaclesByDist(out ObstacleList, CurrADHP.pPtPrj, maxRange);

		}
	}
}
