using System;
using System.Windows.Forms;
using Aran.PANDA.Departure.Properties;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using Aran.PANDA.Common;
using Microsoft.Win32;

namespace Aran.PANDA.Departure
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public static class GlobalVars
	{
		#region "Public Constants"
		#region "PANS-OPS Constants"    '   Срочно: внести в базу PANSOPS

		public static double[] EnrouteMOCValues = new double[] { 300.0, 450.0, 600.0 };

		public const double SIDTerminationFIXToler = 5000.0;
		#endregion

		#region "Product releated"
		public const string PANSOPSVersion = "DOC 8168 OPS/611    Sixth Edition, Amendments 1 to 6 incorporated - 2016";

		public const string PandaRegKey = "SOFTWARE\\RISK\\PANDA";
		public const string ModuleRegKey = PandaRegKey + "\\Conventional";
        public const string ModuleCategory = "Conventional";
        public const string ModuleName = "Departure";
		public const string LicenseKeyName = "Acar";
		//public static RegistryKey PANDARootKey = Registry.CurrentUser;

		public const string HelpFile = "PANDA.chm";
		public const int ReportHelpIDOmni = 3300;
		public const int ReportHelpIDRouts = 5200;
		public const int ReportHelpIDGuidance = 7300;
		public const int ArraySize = 100;
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
		#endregion

		#region "Model releated"
		public const short NO_VALUE = -9999;
		public const double MaxModelRadius = 150000.0;
		public const double MaxILSDist = 20000.0;
		public const double MaxNAVDist = 700000.0;

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

		public static int CurrCmd = 0;

		public static IPolygon p_LicenseRect;
		public static double RModel;
		public static string ConstDir;
		public static string PANS_OPSdb;
		public static string UserName;
		public static int LangCode;

		public static Settings settings;
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

		public static IElement FIXElem;
		public static IElement CLElem;
		public static IElement pCircleElem;
		public static IElement StraightAreaElem;
		public static IElement TurnAreaElem;

		public static IElement PrimElem;
		public static IElement SecRElem;
		public static IElement SecLElem;
		public static IElement StrTrackElem;
		public static IElement NomTrackElem;

		public static IElement KKElem;
		public static IElement K1K1Elem;
		public static IElement TerminationFIXElem;

		public static int TurnAreaElemColor = Functions.RGB(255, 0, 0);

		public static int PrimElemColor = Functions.RGB(255, 255, 0);
		public static int SecRElemColor = Functions.RGB(0, 0, 255);
		public static int SecLElemColor = Functions.RGB(0, 0, 255);
		public static int StrTrackElemColor = Functions.RGB(0, 0, 0);
		public static int NomTrackElemColor = Functions.RGB(0, 0, 0);

		public static int KKElemColor = Functions.RGB(0, 0, 0);
		public static int K1K1ElemColor = Functions.RGB(0, 0, 0);
		#endregion

		#region "Display && units managment"
		public static int DistanceUnit;
		public static int HeightUnit;
		public static int SpeedUnit;
		public static int AngleUnit;

		public static TypeConvert[] DistanceConverter = new TypeConvert[2];
		public static TypeConvert[] HeightConverter = new TypeConvert[2];
		public static TypeConvert[] SpeedConverter = new TypeConvert[2];
		public static TypeConvert[] AngleConverter = new TypeConvert[1];	//

		public static double AnglePrecision;
		public static double GradientPrecision;

		public static string[] RomanFigures = new string[]
		{   "I", "II", "III", "IV",
            "V", "VI", "VII", "VIII",
            "IX", "X", "XI", "XII" };

		#endregion

		#region "Document, projection, e.t.c."
		//public static IApplication Application;

		public static IHookHelper gHookHelper;
		public static Aran.AranEnvironment.IAranEnvironment gAranEnv;

		public static IMap pMap;
		public static IGeographicCoordinateSystem pGCS;
		public static IProjectedCoordinateSystem pPCS;
		public static ISpatialReference pSpRefPrj;
		public static ISpatialReference pSpRefShp;
		public static ISpheroid pSpheroid;
		#endregion

		#region "DB"
		public static ADHPType CurrADHP;
		public static NavaidType[] NavaidList;
		public static NavaidType[] DMEList;
		public static WPT_FIXType[] WPTList;
		#endregion

        public static IWin32Window Win32Window
        {
            get { return  gAranEnv.Win32Window; }
        }

		#endregion

		static void InitUnits(Settings settings)
		{
			HeightUnit = (int)settings.HeightUnit;
			SpeedUnit = (int)settings.SpeedUnit;
			DistanceUnit = (int)settings.DistanceUnit;
			AngleUnit = 0;

			AnglePrecision = settings.AnglePrecision;
			GradientPrecision = settings.GradientPrecision;

			//========================================================================

			DistanceConverter[0].Multiplier = 0.001;
			DistanceConverter[0].Rounding = settings.DistancePrecision;
			DistanceConverter[0].Unit = Resources.str00053;			//"км" '"kM"
			DistanceConverter[1].Multiplier = 1.0 / 1852.0;
			DistanceConverter[1].Rounding = settings.DistancePrecision;
			DistanceConverter[1].Unit = Resources.str00054;			//"ММ" '"NM"

			HeightConverter[0].Multiplier = 1.0;
			HeightConverter[0].Rounding = settings.HeightPrecision;
			HeightConverter[0].Unit = Resources.str00051;				//"meter"
			HeightConverter[1].Multiplier = 1.0 / 0.3048;
			HeightConverter[1].Rounding = settings.HeightPrecision;
			HeightConverter[1].Unit = Resources.str00052;				//"фт" '"feet"

			SpeedConverter[0].Multiplier = 1.0;
			SpeedConverter[0].Rounding = settings.SpeedPrecision;
			SpeedConverter[0].Unit = Resources.str00055;				//"км/ч" '"km/h"
			SpeedConverter[1].Multiplier = 1.0 / 1.852;
			SpeedConverter[1].Rounding = settings.SpeedPrecision;
			SpeedConverter[1].Unit = Resources.str00056;                //"узлы" '"Kt"

			AngleConverter[0].Multiplier = 1.0;
			AngleConverter[0].Rounding = settings.AnglePrecision;
			AngleConverter[0].Unit = "°";
		}

		public static void InitCommand()
		{
            bool isExists;
			ConstDir = RegFuncs.GetConstantsDir(out isExists);
			if (!isExists)
				throw new Exception("Constants path not exists.");

			PANS_OPSdb = RegFuncs.ReadConfig<string>(ModuleCategory + "\\" + ModuleName + "\\PANS_OPSdb", "pansops");

			settings = new Settings();

			settings.Load(GlobalVars.gAranEnv);

			LangCode = settings.Language;

			NativeMethods.SetThreadLocale(LangCode);
			Resources.Culture = new System.Globalization.CultureInfo(LangCode);

			//========================================================================
			pMap = GetMap();

			pSpRefPrj = pMap.SpatialReference;
			if (pSpRefPrj == null)
				throw new Exception("Map projection is not defined.");

			pSpRefPrj.SetZDomain(-2000.0, 14000.0);
			pSpRefPrj.SetMDomain(-2000.0, 14000.0);

			pPCS = pSpRefPrj as IProjectedCoordinateSystem;

			if (pPCS == null)
				pGCS = pSpRefPrj as IGeographicCoordinateSystem;
			else
				pGCS = pPCS.GeographicCoordinateSystem;

			if (pGCS == null)
				throw new Exception("Invalid Map projection.");

			pSpheroid = pGCS.Datum.Spheroid;
			NativeMethods.InitEllipsoid(pSpheroid.SemiMajorAxis, 1.0 / pSpheroid.Flattening);

			if (pPCS != null)
				NativeMethods.InitProjection(pPCS.get_CentralMeridian(true), 0.0, pPCS.ScaleFactor, pPCS.FalseEasting, pPCS.FalseNorthing);
			else
				throw new Exception("Invalid Map projection.");

			ISpatialReferenceFactory2 pSpatRefFact = new SpatialReferenceEnvironment() as ISpatialReferenceFactory2;
			IGeographicCoordinateSystem pGeoCoordSys = pSpatRefFact.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984) as IGeographicCoordinateSystem;
			pSpRefShp = pGeoCoordSys;

			pSpRefShp.SetZDomain(-2000.0, 14000.0);
			pSpRefShp.SetMDomain(-2000.0, 14000.0);
			pSpRefShp.SetDomain(-360.0, 360.0, -360.0, 360.0);

			///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            var Achar = RegFuncs.ReadConfig<string>(ModuleCategory + "/" + ModuleName + "/" + LicenseKeyName, string.Empty);
			p_LicenseRect = DecoderCode.DecodeLCode(Achar, ModuleName);

			//Functions.DrawPolygon(p_LicenseRect, -1, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSCross);
			//System.Windows.Forms.Application.DoEvents();

			if (DecoderCode.LastStartWriter(Achar,  ModuleName) != 0)
				throw new Exception("CRITICAL ERROR!!");

			if (p_LicenseRect.IsEmpty)
				throw new Exception("ERROR #10LR512");

			//==============================================================
			InitUnits(settings);

			//==============================================================
			CurrADHP.Identifier = settings.Aeroport;
			CurrADHP.pPtGeo = null;

			//UserName = "";

			DBModule.InitModule();
			DBModule.FillADHPFields(ref CurrADHP);

			if (CurrADHP.pPtGeo == null)
				throw new Exception("Initialization of ADHP failed.");

			PANS_OPS_DataBase.InitModule();
			Categories_DATABase.InitModule();
			Navaids_DataBase.InitModule();
			//==============================================================
			//m_Win32Window = new Win32Window(GetApplicationHWnd());
		}

		//private static void FillLayerInfo(ref LayerInfo pLayerInfo, string LayerName, IFeatureClass pFeatureClass)
		//{
		//    IDataset pDataset = default(IDataset);
		//    IWorkspace pWs = default(IWorkspace);
		//    System.DateTime tmpVar = default(System.DateTime);

		//    pDataset = pFeatureClass as IDataset;
		//    pWs = pDataset.Workspace;
		//    pLayerInfo.WorkspaceType = (int)pWs.WorkspaceFactory.WorkspaceType;

		//    if (pLayerInfo.WorkspaceType == (int)esriWorkspaceType.esriFileSystemWorkspace)
		//    {
		//        pLayerInfo.Source = pWs.PathName + "\\" + pDataset.BrowseName;

		//        tmpVar = System.IO.File.GetLastWriteTime(pLayerInfo.Source + ".shp");
		//        pLayerInfo.FileDate = System.IO.File.GetLastWriteTime(pLayerInfo.Source + ".dbf");

		//        if (tmpVar > pLayerInfo.FileDate)
		//            pLayerInfo.FileDate = tmpVar;

		//        pLayerInfo.Source = pLayerInfo.Source + ".shp";
		//    }
		//    else
		//    {
		//        pLayerInfo.Source = pWs.PathName;
		//        pLayerInfo.FileDate = System.IO.File.GetLastWriteTime(pLayerInfo.Source);
		//    }

		//    pLayerInfo.LayerName = LayerName;
		//    pLayerInfo.Initialised = true;
		//}

		public static IMap GetMap()
		{
			return gHookHelper.FocusMap;
		}

		public static IActiveView GetActiveView()
		{
			return gHookHelper.ActiveView;
		}

		public static string GetMapFileName()
		{
			if (gHookHelper.Hook is ESRI.ArcGIS.Framework.IApplication)
			{
				ESRI.ArcGIS.Framework.IApplication app = (ESRI.ArcGIS.Framework.IApplication)gHookHelper.Hook;
				return app.Templates.get_Item(app.Templates.Count - 1);
			}
			else
				return gAranEnv.DocumentFileName;
		}

		public static int GetApplicationHWnd()
		{
			if (gHookHelper.Hook is ESRI.ArcGIS.Framework.IApplication)
				return ((ESRI.ArcGIS.Framework.IApplication)gHookHelper.Hook).hWnd;
			else
				return gAranEnv.Win32Window.Handle.ToInt32 ();
		}
	}
}
