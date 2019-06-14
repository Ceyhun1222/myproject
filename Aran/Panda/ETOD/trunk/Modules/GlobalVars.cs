using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

using Microsoft.Win32;
using Microsoft.VisualBasic;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Controls;

using Aran.Interfaces;
//using Aran.Package;
using Aran.AranEnvironment;
using Aran.Panda.Common;

using ETOD.Properties;

namespace ETOD
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public static class GlobalVars
	{
		#region "Public Constants"
		#region "PANS-OPS Constants"    '   Срочно: внести в базу PANSOPS
		//public static double[] VDepartMin = new double[] {
		//    204.0,
		//    264.0,
		//    325.0,
		//    380.0,
		//    468.0 };
		//public static double[] VDepartMax = new double[] {
		//    226.0,
		//    308.0,
		//    490.0,
		//    539.0,
		//    561.0 };
		//public const double SIDTerminationFIXToler = 5000.0;
		#endregion

		#region "Product releated"
		public const string PANSOPSVersion = "DOC 8168 OPS/611    Fifth Edition - 2006";
		public const string PandaRegKey = "SOFTWARE\\RISK\\Panda";

		public const string HelpFile = "PANDA.chm";
		//public const short ReportHelpIDOmni = 3300;
		//public const short ReportHelpIDRouts = 5200;
		//public const short ReportHelpIDGuidance = 7300;
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

		#region "Public Variables"

		#region "Public Public"
		public static int CurrCmd = 0;

		public static IPolygon p_LicenseRect;
		public static double RModel;
		public static string UserName;
		public static string InstallDir;
		public static int LangCode;
		public static Win32Window m_Win32Window;
		#endregion

		#region "Visibility manipulation"

		public static bool Area1State = true;
		public static bool Area2State = true;
		public static bool Area3State = true;
		public static bool Area4State = true;

		//Graphic Elements====================================
		public static IElement Area1Elem = null;
		public static List<IElement> Area2Elem = new List<IElement>();
		public static IElement Area2dElem = null;
		public static IElement Area3Elem = null;
		public static IElement RWYElem = null;
		public static List<IElement> Area4Elem = new List<IElement>();

		public static int Area1Color = Functions.RGB(255, 255, 0);
		public static int Area2Color = Functions.RGB(0, 0, 255);
		public static int Area3Color = Functions.RGB(0, 255, 0);
		public static int Area4Color = Functions.RGB(0, 0, 0);

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

		#region "Display && units managment"
		public static int DistanceUnit;
		public static int HeightUnit;
		public static int SpeedUnit;

		public static TypeConvert[] DistanceConverter = InitArray<TypeConvert>(2);    //--- Checked
		public static TypeConvert[] HeightConverter = InitArray<TypeConvert>(2);      //--- Checked
		public static TypeConvert[] SpeedConverter = InitArray<TypeConvert>(2);       //--- Checked

		#endregion

		#region "Document, Map, projection, e.t.c."
		public static IHookHelper gHookHelper;
		public static Aran.AranEnvironment.IAranEnvironment gAranEnv;

		//public static IApplication Application;
		public static IMap pMap;

		public static IGeographicCoordinateSystem pGCS;
		public static IProjectedCoordinateSystem pPCS;

		public static ISpatialReference pSpRefPrj;
		public static ISpatialReference pSpRefShp;
		public static LocalCoordinatSystem pLocalCoord;

		public static ISpheroid pSpheroid;
		#endregion

		#region "DB"
		public static ADHPType[] ADHPList;
		public static NavaidType[] NavaidList;
		public static NavaidType[] DMEList;
		public static WPT_FIXType[] WPTList;
		#endregion

		#endregion

		public static IWin32Window Win32Window
		{
			get { return gAranEnv.Win32Window; }
		}

		static IPandaAranExtension PandaExt;

		static bool Initalize_PANDA()
		{
			try
			{
				if (PandaExt == null)
					PandaExt = GetPandaAranExt();

				if (PandaExt == null)
				{
					throw new Exception(Resources.Str200);
					//MessageBox.Show(Resources.str70, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
					//return false;
				}
			}
			catch
			{
				throw new Exception(Resources.Str200);
				//MessageBox.Show(Resources.str70, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
				//return false;
			}
			return true;
		}

		public static void InitCommand()
		{
			Functions.HandleThreadException();

			InstallDir = Functions.RegRead<String>(Registry.CurrentUser, PandaRegKey, "Path", null);
			if (InstallDir == null)
				throw new Exception("Intallation Path Not Exists.");

			Initalize_PANDA();
			Settings _settings;

			_settings = new Settings();
			_settings.Load(PandaExt);

			LangCode = _settings.Language;

			NativeMethods.SetThreadLocale(LangCode);
			Resources.Culture = new System.Globalization.CultureInfo(LangCode);

			//========================================================================
			pMap = GetMap();
			pSpRefPrj = pMap.SpatialReference;

			if (pSpRefPrj == null)
				throw new Exception("Map projection is not defined.");

			pPCS = pSpRefPrj as IProjectedCoordinateSystem;

			if (pPCS == null)
				pGCS = pSpRefPrj as IGeographicCoordinateSystem;
			else
				pGCS = pPCS.GeographicCoordinateSystem;

			if (pGCS == null)
				throw new Exception("Invalid Map projection.");

			pSpheroid = pGCS.Datum.Spheroid;
			NativeMethods.InitEllipsoid(pSpheroid.SemiMajorAxis, 1.0 / pSpheroid.Flattening);

			if ((pPCS != null))
				NativeMethods.InitProjection(pPCS.get_CentralMeridian(true), 0.0, pPCS.ScaleFactor, pPCS.FalseEasting, pPCS.FalseNorthing);
			else
				throw new Exception("Invalid Map projection.");

			IGeographicCoordinateSystem pGeoCoordSys = default(IGeographicCoordinateSystem);
			ISpatialReferenceFactory2 pSpatRefFact = default(ISpatialReferenceFactory2);

			UserName = "";
			DBGDBModule.InitModule();

			//DBGDBModule.InitModule();
			//DBModule.InitModule();

			//IWorkspace pWorkspace = default(IWorkspace);
			//IEnumDataset pDatasetsEn = default(IEnumDataset);
			//IGeoDataset pGeoDataset = default(IGeoDataset);

			//pWorkspace = DBModule.pObjectDir.Workspace as IWorkspace;

			//if (pWorkspace != null)
			//{
			//    pDatasetsEn = pWorkspace.get_Datasets(esriDatasetType.esriDTAny);
			//    pDatasetsEn.Reset();

			//    pGeoDataset = pDatasetsEn.Next() as IGeoDataset;
			//    pSpRefShp = pGeoDataset.SpatialReference;
			//}
			pSpRefShp = null;

			if (pSpRefShp == null || pSpRefShp.Name == "Unknown")
			{
				pSpatRefFact = new SpatialReferenceEnvironment() as ISpatialReferenceFactory2;
				pGeoCoordSys = pSpatRefFact.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984) as IGeographicCoordinateSystem;
				pSpRefShp = pGeoCoordSys;
				GetActiveView().Refresh();
			}

			pSpRefShp.SetZDomain(-2000.0, 14000.0);
			pSpRefShp.SetMDomain(-2000.0, 14000.0);
			pSpRefShp.SetDomain(-360.0, 360.0, -360.0, 360.0);

			///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
			//string Acar = Functions.RegRead<String>(Registry.CurrentUser, PandaRegKey + "\\Departure", "Acar", "");
			//p_LicenseRect = DecoderCode.DecodeLCode(Acar, "Departure");

			//string Acar = Functions.RegRead<String>(Registry.CurrentUser, PandaRegKey + "\\ETOD", "Acar", "");
			//p_LicenseRect = DecoderCode.DecodeLCode(Acar, "ETOD");

			//if (DecoderCode.LstStDtWriter() != 0)
			//    throw new Exception("CRITICAL ERROR!!");

			//if (p_LicenseRect.IsEmpty)
			//    throw new Exception("ERROR #10LR512");

			//==============================================================
			//DistanceUnit = Functions.RegRead<Int32>(Registry.CurrentUser, PandaRegKey, "Distance", 0);
			//if (DistanceUnit < 0 || DistanceUnit > 1)
			//    DistanceUnit = 0;

			//double DistancePrecision = Functions.RegRead<Double>(Registry.CurrentUser, PandaRegKey, "DistancePrecision", 0.1);
			//double HeightPrecision = Functions.RegRead<Double>(Registry.CurrentUser, PandaRegKey, "HeightPrecision", 1.0);
			//double DSpeedPrecision = Functions.RegRead<Double>(Registry.CurrentUser, PandaRegKey, "DSpeedPrecision", 1);
			//double SpeedPrecision = Functions.RegRead<Double>(Registry.CurrentUser, PandaRegKey, "SpeedPrecision", 1);
			//HeightUnit = Functions.RegRead<Int32>(Registry.CurrentUser, PandaRegKey, "Height", 0);
			//SpeedUnit = Functions.RegRead<Int32>(Registry.CurrentUser, PandaRegKey, "Speed", 0);
			//========================================================================
			//DistanceConverter[0].Multiplier = 0.001;
			//DistanceConverter[0].Rounding = DistancePrecision;
			//DistanceConverter[0].Unit = Resources.Str10;			//"км" '"kM"

			//DistanceConverter[1].Multiplier = 1.0 / 1852.0;
			//DistanceConverter[1].Rounding = DistancePrecision;
			//DistanceConverter[1].Unit = Resources.Str11;			//"ММ" '"NM"

			//HeightConverter[0].Multiplier = 1.0;
			//HeightConverter[0].Rounding = HeightPrecision;
			//HeightConverter[0].Unit = Resources.Str12;				//"meter"

			//HeightConverter[1].Multiplier = 1.0 / 0.3048;
			//HeightConverter[1].Rounding = HeightPrecision;
			//HeightConverter[1].Unit = Resources.Str13;				//"фт" '"feet"

			//SpeedConverter[0].Multiplier = 1.0;
			//SpeedConverter[0].Rounding = SpeedPrecision;
			//SpeedConverter[0].Unit = Resources.Str14;				//"км/ч" '"km/h"

			//SpeedConverter[1].Multiplier = 1.0 / 1.852;
			//SpeedConverter[1].Rounding = SpeedPrecision;
			//SpeedConverter[1].Unit = Resources.Str15;				//"узлы" '"Kt"

			HeightUnit = _settings.HeightUnitIndex;						// Functions.RegRead<Int32>(Registry.CurrentUser, PandaRegKey, "Height", 0);
			SpeedUnit = _settings.SpeedUnitIndex;						// Functions.RegRead<Int32>(Registry.CurrentUser, PandaRegKey, "Speed", 0);
			DistanceUnit = _settings.DistanceUnitIndex;

			//========================================================================

			DistanceConverter[0].Multiplier = 0.001;
			DistanceConverter[0].Rounding = _settings.DistancePrecision;
			DistanceConverter[0].Unit = Resources.Str10;
			//"км" '"kM"
			DistanceConverter[1].Multiplier = 1.0 / 1852.0;
			DistanceConverter[1].Rounding = _settings.DistancePrecision;
			DistanceConverter[1].Unit = Resources.Str11;
			//"ММ" '"NM"

			HeightConverter[0].Multiplier = 1.0;
			HeightConverter[0].Rounding = _settings.HeightPrecision;
			HeightConverter[0].Unit = Resources.Str12;
			//"meter"
			HeightConverter[1].Multiplier = 1.0 / 0.3048;
			HeightConverter[1].Rounding = _settings.HeightPrecision;
			HeightConverter[1].Unit = Resources.Str13;
			//"фт" '"feet"

			SpeedConverter[0].Multiplier = 1.0;
			SpeedConverter[0].Rounding = _settings.SpeedPrecision;
			SpeedConverter[0].Unit = Resources.Str14;
			//"км/ч" '"km/h"
			SpeedConverter[1].Multiplier = 1.0 / 1.852;
			SpeedConverter[1].Rounding = _settings.SpeedPrecision;
			SpeedConverter[1].Unit = Resources.Str15;
			//"узлы" '"Kt"

			//==============================================================
			//PANS_OPS_DataBase.InitModule();
			//Navaids_DataBase.InitModule();
			//==============================================================

			//List<string> icaoPrefixList = new List<string>();
			//icaoPrefixList.Add("-1");
			//DBModule.FillADHPList(ref ADHPList, icaoPrefixList);

			//Settings _settings;
			//_settings = new Settings(DateTime.Now);
			//Aran.Package.IPackable packable = _settings as Aran.Package.IPackable;
			//GlobalVars.PandaExt.GetData("Project", ref packable);
			//DBModule.FillADHPList(ref ADHPList, _settings.DB.Organisation.Identifier); //, CurrCmd = 2

			m_Win32Window = new Win32Window(GetApplicationHWnd());
		}

		private static void FillLayerInfo(ref LayerInfo pLayerInfo, string LayerName, IFeatureClass pFeatureClass)
		{
			IDataset pDataset = default(IDataset);
			IWorkspace pWs = default(IWorkspace);
			System.DateTime tmpVar = default(System.DateTime);

			pDataset = pFeatureClass as IDataset;
			pWs = pDataset.Workspace;
			pLayerInfo.WorkspaceType = (int)pWs.WorkspaceFactory.WorkspaceType;

			if (pLayerInfo.WorkspaceType == (int)esriWorkspaceType.esriFileSystemWorkspace)
			{
				pLayerInfo.Source = pWs.PathName + "\\" + pDataset.BrowseName;

				tmpVar = File.GetCreationTime(pLayerInfo.Source + ".shp");
				pLayerInfo.FileDate = File.GetCreationTime(pLayerInfo.Source + ".dbf");

				if (tmpVar > pLayerInfo.FileDate)
					pLayerInfo.FileDate = tmpVar;

				pLayerInfo.Source = pLayerInfo.Source + ".shp";
			}
			else
			{
				pLayerInfo.Source = pWs.PathName;
				pLayerInfo.FileDate = File.GetCreationTime(pLayerInfo.Source);
			}

			pLayerInfo.LayerName = LayerName;
			pLayerInfo.Initialised = true;
		}

		public static IMap GetMap()
		{
			//return (Application.Document as IMxDocument).FocusMap;
			return gHookHelper.FocusMap;
		}

		public static IActiveView GetActiveView()
		{
			//ESRI.ArcGIS.ArcMapUI.IMxDocument pDocument = default(ESRI.ArcGIS.ArcMapUI.IMxDocument);
			//pDocument = Application.Document as IMxDocument;
			//return pDocument.ActiveView;
			return gHookHelper.ActiveView;
		}

		public static string GetMapFileName()
		{
//			return Application.Templates.get_Item(Application.Templates.Count - 1);
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
//			return Application.hWnd;
			if (gHookHelper.Hook is ESRI.ArcGIS.Framework.IApplication)
				return ((ESRI.ArcGIS.Framework.IApplication)gHookHelper.Hook).hWnd;
			else
				return gAranEnv.Win32Window.Handle.ToInt32();
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
