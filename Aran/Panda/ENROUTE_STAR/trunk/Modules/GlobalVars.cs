using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;

using Microsoft.Win32;

using ESRI.ArcGIS.Controls;

using Aran.Interfaces;
using Aran.Package;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Geometries.SpatialReferences;
using Aran.AranEnvironment;
using Aran.Panda.Common;
using Aran.Panda.Constants;
using Aran.Panda.EnrouteStar.Properties;

namespace Aran.Panda.EnrouteStar
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public static class GlobalVars
	{
		#region "Public Constants"
		#region "PANS-OPS Constants"    '   Срочно: внести в базу PANSOPS
		public static double[] VDepartMin = new double[] {
            204.0,
            264.0,
            325.0,
            380.0,
            468.0 };
		public static double[] VDepartMax = new double[] {
            226.0,
            308.0,
            490.0,
            539.0,
            561.0 };
		public const double SIDTerminationFIXToler = 5000.0;

		public static Constants.Constants constants = null;
		#endregion

		#region "Product releated"
		public const string PANSOPSVersion = "DOC 8168 OPS/611    Sixth Edition - 2014";

		public const string ModuleName = "EnroteRNAV";
		public const string PandaRegKey = "SOFTWARE\\RISK\\Panda";
		public const string ModuleRegKey = "SOFTWARE\\RISK\\RNAV";
		public const string Acar = "Acar";

		public const string HelpFile = "PANDA.chm";
		//public const short ReportHelpIDOmni = 3300;
		//public const short ReportHelpIDRouts = 5200;
		//public const short ReportHelpIDGuidance = 7300;
		#endregion

		#region "Math releated"

		public const double mEps = 1E-12;
		public const double distEps = 0.0001;
		public const double PDGEps = 0.0001;

		public const double degEps = 1.0 / 36000.0;
		public const double radEps = degEps * ARANMath.DegToRadValue;

		public const double NMCoeff = 1.852;
		public const double FootCoeff = 0.3048;
		#endregion

		#region "Model releated"
		public const short NO_VALUE = -9999;
		public const double MaxModelRadius = 150000.0;
		public const double MaxILSDist = 20000.0;
		public const double MaxNAVDist = 200000.0;
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

		#region "Public Variables"

		#region "Public Public"
		public static string InstallDir;
		public static int CurrCmd = -1;
		public static string UserName;
		public static MultiPolygon p_LicenseRect;
		public static IPandaAranExtension PandaExt;
		#endregion

		#region "Visibility manipulation"
		//public static bool ButtonControl1State;
		//public static bool ButtonControl2State;
		//public static bool ButtonControl3State;
		//public static bool ButtonControl4State;
		//public static bool ButtonControl5State;
		//public static bool ButtonControl6State;
		//public static bool ButtonControl7State;
		//public static bool ButtonControl8State;

		//public static IElement FIXElem;
		//public static IElement CLElem;
		//public static IElement pCircleElem;
		//public static IElement StraightAreaElem;
		//public static IElement TurnAreaElem;

		//public static IElement PrimElem;
		//public static IElement SecRElem;
		//public static IElement SecLElem;
		//public static IElement StrTrackElem;
		//public static IElement NomTrackElem;

		//public static IElement KKElem;
		//public static IElement K1K1Elem;
		//public static IElement TerminationFIXElem;

		//public static int TurnAreaElemColor = Functions.RGB(255, 0, 0);
		//public static int PrimElemColor = Functions.RGB(255, 255, 0);
		//public static int SecRElemColor = Functions.RGB(0, 0, 255);
		//public static int SecLElemColor = Functions.RGB(0, 0, 255);
		//public static int StrTrackElemColor = Functions.RGB(0, 0, 0);
		//public static int NomTrackElemColor = Functions.RGB(0, 0, 0);

		//public static int KKElemColor = Functions.RGB(0, 0, 0);
		//public static int K1K1ElemColor = Functions.RGB(0, 0, 0);
		#endregion

		#region "Display & Settings & units managment"
		public static Common.Settings settings;
		public static Aran.Panda.Common.UnitConverter unitConverter;

		#endregion

		#region "Document, projection, e.t.c."
		public static IHookHelper gHookHelper;
		public static IAranEnvironment gAranEnv;
		public static IAranGraphics gAranGraphics;

		public static double ModellingRadius = 150000;
		public static double RModel;

		public static SpatialReferenceOperation pspatialReferenceOperation;
		public static SpatialReference pSpRefPrj;
		public static SpatialReference pSpRefShp;
		public static Ellipsoid pSpheroid;

		#endregion

		#region "DB"
		public static ADHPType CurrADHP;
		public static NavaidType[] NavaidList;
		public static NavaidType[] DMEList;
		public static WPT_FIXType[] WPTList;
		#endregion

		#endregion

        public static IWin32Window Win32Window
        {
            get { return  gAranEnv.Win32Window; }
        }

		static bool Initalize_PANDA()
		{
			try
			{
				if (PandaExt == null)
					PandaExt = GetPandaAranExt();

				if (PandaExt == null)
					throw new Exception(Resources.str70);
			}
			catch
			{
				throw new Exception(Resources.str70);
			}
			return true;
		}

		public static void InitCommand()
		{
			Functions.HandleThreadException();

			InstallDir = RegFuncs.RegRead<String> ( Registry.CurrentUser, PandaRegKey, "Path", null );
			if (InstallDir == null)
				throw new Exception("Intallation Path Not Exists.");

			Initalize_PANDA();

			settings = new Common.Settings();
			settings.Load(PandaExt);

			unitConverter = new Aran.Panda.Common.UnitConverter(settings);

			Resources.Culture = Thread.CurrentThread.CurrentUICulture;
			//========================================================================

			pspatialReferenceOperation = new SpatialReferenceOperation(gAranEnv);
			pSpRefPrj = pspatialReferenceOperation.SpRefPrj;
			pSpRefShp = pspatialReferenceOperation.SpRefGeo;

			///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
			string Achar = RegFuncs.RegRead<String> ( Registry.CurrentUser, ModuleRegKey + "\\" + ModuleName, Acar, "" );
			p_LicenseRect = DecoderCode.DecodeLCode(Achar, ModuleName);

			if (DecoderCode.LastStartWriter(ModuleName) != 0)
				throw new Exception("CRITICAL ERROR!!");

			if (p_LicenseRect.Count == 0)
				throw new Exception("ERROR #10LR512");

			//==============================================================
			ARANFunctions.InitEllipsoid();
			UserName = DBModule.InitModule();
			//PANS_OPS_DataBase.InitModule();

			Navaids_DataBase.InitModule();
			if (constants == null)
				constants = new Constants.Constants();
			//===============================================================
			//m_Win32Window = new Win32Window(GetApplicationHWnd());
			//DBModule.FillADHPList(ref ADHPList, _settings.Organization); //, CurrCmd = 2 _settings.DB.Organisation.Identifier
			//_settings.Aeroport
			ModellingRadius = settings.Radius;

			CurrADHP.Identifier = settings.Aeroport;
			CurrADHP.pPtGeo = null;

			DBModule.FillADHPFields(ref CurrADHP);

			if (CurrADHP.pPtGeo == null)
				throw new Exception("Initialization of ADHP failed.");
		}

		//public static IMap GetMap()
		//{
		//    return gHookHelper.FocusMap;
		//}

		//public static IActiveView GetActiveView()
		//{
		//    return gHookHelper.ActiveView;
		//}

		public static string GetMapFileName()
		{
			if (gHookHelper.Hook is ESRI.ArcGIS.Framework.IApplication)
			{
				ESRI.ArcGIS.Framework.IApplication app =
					(ESRI.ArcGIS.Framework.IApplication)gHookHelper.Hook;

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

		static IPandaAranExtension GetPandaAranExt()
		{
			if (gHookHelper.Hook is ESRI.ArcGIS.Framework.IApplication)
			{
				ESRI.ArcGIS.Framework.IApplication app =
					(ESRI.ArcGIS.Framework.IApplication)gHookHelper.Hook;

				ESRI.ArcGIS.esriSystem.UID pID = new ESRI.ArcGIS.esriSystem.UID();
				pID.Value = "PandaAranExt.PandaAranExt";
				IPandaAranExtension ext = (IPandaAranExtension)app.FindExtensionByCLSID(pID);
				return ext;
			}
			else
				return gAranEnv.PandaAranExt;
		}

		public static T[] InitArray<T>(int count) where T : struct
		{
			return new T[count];
		}

		public static T[] CopyArray<T>(T[] source) where T : struct
		{
			T[] ta = new T[source.Length];
			for (int i = 0; i < ta.Length; i++)
				ta[i] = source[i];
			return ta;
		}
	}
}
