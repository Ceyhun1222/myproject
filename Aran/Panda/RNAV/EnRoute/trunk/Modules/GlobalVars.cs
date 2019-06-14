using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Aran.Aim;
using Aran.AranEnvironment;
using Aran.Geometries;
using Aran.Geometries.SpatialReferences;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
using Aran.PANDA.RNAV.EnRoute;
using Microsoft.Win32;
using Aran.PANDA.RNAV.EnRoute.Properties;

namespace Aran.PANDA.RNAV.EnRoute
{
	public static class GlobalVars
	{
		#region "Product releated"
		public const string PANSOPSVersion = "DOC 8168 OPS/611    Sixth Edition, Amendments 1 to 6 incorporated - 2016";
		public const string ModuleName = "En-Route";
		//public const string ModuleCategory = "RNAV";

		public const string HelpFile = "PANDA.chm";
		public const int ReportHelpIDRNAVEnRoute = 0;
		#endregion

		#region "Model releated"
		public const short NO_VALUE = -9999;
		public const double MaxModelRadius = 150000.0;
		public const double MaxILSDist = 20000.0;
		public const double MaxNAVDist = 200000.0;
		#endregion

		// System menu
		public const int WM_SYSCOMMAND = 0x112;
		public const int MF_STRING = 0x0;
		public const int MF_SEPARATOR = 0x800;
		public const int SYSMENU_ABOUT_ID = 0x1;

		#region "Public Variables"

		#region "Public Public"

		public static string ConstDir;

		public static MultiPolygon p_LicenseRect;
		public static string UserName;
		#endregion

		#region "Visibility manipulation"
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

		public static Settings settings;
		public static UnitConverter unitConverter;
		public static NavaidsConstant navaidsConstants;
		public static Constants.Constants constants = null;

		public static IAranEnvironment gAranEnv;
		public static IAranGraphics gAranGraphics;

		//private static bool _errorHandled = false;

		public static double ModellingRadius = 150000;
		public static double RModel;

		public static SpatialReferenceOperation pspatialReferenceOperation;
		public static SpatialReference pSpRefPrj;
		public static SpatialReference pSpRefGeo;
		public static Ellipsoid pSpheroid;

		#endregion

		#region "DB"
		public static ADHPType CurrADHP;
		//public static RWYType[] RWYList;
		//public static ObstacleType[] ObstacleList;
		//public static NavaidType[] NavaidList;
		//public static NavaidType[] DMEList;

		public static WPT_FIXType[] WPTList;
		#endregion

		#endregion

		public static IWin32Window Win32Window
		{
			get { return gAranEnv.Win32Window; }
		}

		public static void InitCommand()
		{
			bool isExists;
			ConstDir = RegFuncs.GetConstantsDir(out isExists);

			if (!isExists)
				throw new Exception("Installation Path Not Exists.");

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

			UserName = DBModule.InitModule();
			DBModule.FillADHPFields(ref CurrADHP);
			if (CurrADHP.pPtGeo == null)
				throw new Exception("Initialization of ADHP failed.");

			//DBModule.FillRWYList(out RWYList, CurrADHP);
			DBModule.FillWPT_FIXList(out WPTList, p_LicenseRect);

			//double maxRange = Functions.CalcMaxRadius() + ModellingRadius;
			//DBModule.GetObstaclesByDist(out ObstacleList, CurrADHP.pPtPrj, settings.Radius);

			if (WPTList.Length == 0)
				throw new Exception("'WPT/Designated points' list is empty!");
			//Aran.PANDA.Common.NativeMethods.HidePandaBox();
			//MessageBox.Show(GlobalVars.Win32Window, "'WPT/Designated points' list is empty!", "PBN En-Route");
		}

		public static string GetMapFileName()
		{
			return gAranEnv.DocumentFileName;
		}

	}
}
