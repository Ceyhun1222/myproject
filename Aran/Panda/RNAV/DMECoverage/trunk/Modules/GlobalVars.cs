using System;
using System.Threading;
using System.Windows.Forms;
using Aran.AranEnvironment;
using Aran.Geometries;
using Aran.Geometries.SpatialReferences;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
using Aran.PANDA.RNAV.DMECoverage.Properties;
using Microsoft.Win32;

namespace Aran.PANDA.RNAV.DMECoverage
{
	internal static class GlobalVars
	{
		#region "Public Constants"
		#region "PANS-OPS Constants"	'	Срочно: внести в базу PANSOPS

		//public const double SIDTerminationFIXToler = 5000.0;
		public static Constants.Constants constants = null;

		//public const double f05 = ARANMath.DegToRadValue;	//0.087266462599716;	//ARANMath.DegToRad(  5.0);
		//public const double f15 = 0.26179938779914941;		//ARANMath.DegToRad( 15.0);
		//public const double f120 = 2.1118483949131388;		//ARANMath.DegToRad(121.0);
		//public const double f270 = 4.7123889803846897;		//ARANMath.DegToRad(270.0);

		#endregion

		#region "Product releated"
		public const string PANSOPSVersion = "DOC 8168 OPS/611    Sixth Edition, Amendments 1 to 6 incorporated - 2016";
		public const string ModuleName = "DMECoverage";

		//public const string ModuleCategory = "RNAV";
		//public const string HelpFile = "PANDA.chm";
		//public const int ReportHelpIDRNAVDeparture = 0;
		#endregion
		// System menu
		public const int WM_SYSCOMMAND = 0x112;
		public const int MF_STRING = 0x0;
		public const int MF_SEPARATOR = 0x800;
		public const int SYSMENU_ABOUT_ID = 0x1;
		#endregion

		#region "Public Variables"

		#region "Public Public"
		//public static int CurrCmd = -1;
		public static string ConstDir;

		public static MultiPolygon p_LicensePoly;
		public static string UserName;
		#endregion

		#endregion

		#region "Document, projection, e.t.c."

		//private static bool _errorHandled = false;

		public static Settings settings;
		public static Aran.PANDA.Common.UnitConverter unitConverter;
		public static NavaidsConstant navaidsConstants;

		public static IAranEnvironment gAranEnv;
		public static IAranGraphics gAranGraphics;

		public static double ModellingRadius = 150000;
		//public static double RModel;

		public static SpatialReferenceOperation pspatialReferenceOperation;
		public static SpatialReference pSpRefPrj;
		public static SpatialReference pSpRefGeo;
		//public static Ellipsoid pSpheroid;

		#endregion

		#region "DB"
		public static Aran.PANDA.Common.ADHPType CurrADHP;
		//public static Aran.PANDA.Common.RWYType[] RWYList;
		//public static ObstacleType[] ObstacleList;
		//public static Aran.PANDA.Common.ObstacleContainer ObstacleList;

		public static NavaidType[] NavaidList;
		public static NavaidType[] DMEList;

		public static WPT_FIXType[] WPTList;
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
			pSpRefGeo = pspatialReferenceOperation.SpRefGeo;

			//'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...
			string Achar = RegFuncs.GetRNAVAcar(ModuleName);

			p_LicensePoly = DecoderCode.DecodeLCode(Achar, ModuleName);

			if (DecoderCode.LstStDtWriter(Achar, ModuleName) != 0)
				throw new Exception("CRITICAL ERROR!!!");

			if (p_LicensePoly.Count == 0)
				throw new Exception("Empty region");

			//'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...

			CurrADHP.Identifier = settings.Aeroport;
			CurrADHP.pPtGeo = null;

			DBModule.InitModule();
			DBModule.FillADHPFields(ref CurrADHP);

			//GlobalVars.gAranGraphics.DrawMultiPolygon(p_LicenseRect, -1, AranEnvironment.Symbols.eFillStyle.sfsCross);
			//Application.DoEvents();

			if (CurrADHP.pPtGeo == null)
				throw new Exception("Initialization of ADHP failed.");

			DBModule.FillWPT_FIXList(out WPTList, p_LicensePoly);
			DBModule.FillNavaidList(out NavaidList, out DMEList, p_LicensePoly);

			//DBModule.FillRWYList(out RWYList, CurrADHP);
			//DBModule.GetObstaclesByDist(out ObstacleList, CurrADHP.pPtPrj, settings.Radius);
			//double maxRange = Functions.CalcMaxRadius() + ModellingRadius;
		}
	}
}
