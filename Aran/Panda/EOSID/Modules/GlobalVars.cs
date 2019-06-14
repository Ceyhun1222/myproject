using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using Microsoft.Win32;
using ESRI.ArcGIS.ArcMapUI;

namespace EOSID
{
	static class GlobalVars
	{
		#region "Product releated"
		//public const string PANSOPSVersion = "DOC 8168 OPS/611    Fifth Edition - 2006";
		public const string PandaRegKey = "SOFTWARE\\RISK\\PANDA";
		public const string ModuleRegKey = PandaRegKey + "\\Conventional";
		public const string ModuleName = "EOSID";
		public const string LicenseKeyName = "Acar";

		public static RegistryKey PANDARootKey = Registry.CurrentUser;

		//public const string HelpFile = "PANDA.chm";
		//public const short ReportHelpID1 = 3300;
		//public const short ReportHelpID2 = 5200;
		//public const short ReportHelpID3 = 7300;
		#endregion

		#region "Math releated"
		public const double DegToRadValue = Math.PI / 180.0;
		public const double RadToDegValue = 180.0 / Math.PI;

		public const double mEps = 1E-12;
		public const double distEps = 0.0001;
		public const double distEps2 = distEps * distEps;

		public const double PDGEps = 0.0001;

		public const double degEps = 1.0 / 36000.0;
		public const double radEps = degEps * DegToRadValue;

		//public const double NMCoeff = 1.852;
		//public const double FootCoeff = 0.3048;
		#endregion

		#region "Model releated"
#if AN124
#endif
		public static double[] mass_AN124 = { 240, 300, 365, 392 };
		public static double[] v2_AN124 = { 270, 280, 290, 295 };
		public static double[] v4_AN124 = { 420, 420, 440, 440 };

		//public static double[] vRotation = { 235, 245, 250, 260 };
		//public static double[] vLiftOff = { 250, 260, 270, 275 };
		//public static double[] vAccentToCircle = { 310, 310, 310, 310 };
		//public static double[] v15start = { 330, 330, 330, 340 };
		//public static double[] v15end = { 370, 370, 370, 380 };

		//public static double[] v2start = { 390, 390, 390, 410 };
		//public static double[] v2end = { 420, 420, 420, 440 };
		//public static double[] vCircling = { 450, 450, 450, 470 };
#if AN124
Взлетная масса, тонны					| 240 | 300 | 365 | 392 |
Скорость (V), км/ч						+-----------------------+
Скорость подъема носовой ноги Vп.оп		| 235 | 245 | 250 | 260 |
Vотр									| 250 | 260 | 270 | 275 |
Безопасная скорость взлета V2			| 270 | 280 | 290 | 295 |
V набора высоты круга					|			310			|
V уборки закрылков до 15	начало		|		330		  | 340 |
							конец		|		370		  | 380 |
V уборки закрылков до 2	начало		|		390		  | 410 |
							конец		|		420		  | 440 |
V полной уборки механизации крыла  V4	|	420		|	440		|
V полета по кругу						|	450		|	470		|
#endif

#if IL76
#endif
		public static double[] mass_IL76 = { 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 195 };
		public static double[] v2_IL76 = { 280, 280, 280, 280, 280, 280, 280, 280, 280, 285, 290 };
		public static double[] v4_IL76 = { 280, 290, 305, 315, 330, 340, 350, 360, 375, 385, 390 };

#if IL76

Вес, тонны								| 100 | 110 | 120 | 130 | 140 | 150 | 160 | 170 | 180 | 190 | 195 |
Скорость (V), км/ч						+-----------------------------------------------------------------+
Скорость подъема носовой ноги Vп.оп		|							230						  | 235 | 240 |		vRotation
Безопасная скорость взлета V2			|							280						  | 285 | 290 |		v2
Скорость начала уборки закрылков		|		290		  |		 320		|	340		| 360 |		370	  |		v15start
Скорость начала уборки предкрылков		|					350				|		370		  |		390	  |
V полной уборки механизации крыла V4	| 280 | 290 | 305 | 315 | 330 | 340 | 350 | 360 | 375 | 385 | 390 |

#endif

		public const short NO_VALUE = -9999;
		public const double MaxModelRadius = 150000.0;
		public const double MaxILSDist = 20000.0;
		public const double MaxNAVDist = 200000.0;

		public const double RMin = 30000;
		public const double RMax = 100000;

		public const double heightAboveDERMin = 5;
		public const double heightAboveDERMax = 10.668;

		public static double[] ProtectionWidt = { 300.0, 600.0, 900.0, 1200.0 };

		public const double BaseSemiWidth = 90;
		public const double MandatorySemiWidth = 300;
		public const double SplayRate = 0.125;				//	12.5%

		public const double NetMOC = 10.668;				//	10.67	35
		public const double TurnMOC = 15.24;				//	15.24	50
		public const double ArcProtectWidth = 1852.0;
		public const double minAccelerationHeight = 120;	//121.92
		public const double MaxTrackAbeam = 5556.0;
		public const double MinTurnAngle = 1.0;
		public const double MaxTurnAngle = 210.0;
		public const double MaxInterceptAngle = 120.0;

		#endregion

		#region "Public Variables"
		public static int CurrCmd = 0;

		//public static IPolygon p_LicenseRect;
		public static double RModel = 50000;
		public static double heightAboveDER;

		public static string UserName;
		public static string ConstDir;
		//public static int LangCode;
		public static Win32Window win32Window;
		#endregion

		#region "Map"
		public static IApplication Application;
		public static IMap pMap;
		public static IGeographicCoordinateSystem pGCS;
		public static IProjectedCoordinateSystem pPCS;
		public static ISpatialReference pSpRefPrj;
		public static ISpatialReference pSpRefShp;
		public static ISpheroid pSpheroid;
		#endregion

		#region "DB"
		public static ADHPData[] ADHPList;
		public static NavaidData[] NavaidList;
		public static NavaidData[] DMEList;
		public static WPT_FIXData[] WPTList;
		#endregion

		public static ADHPData m_CurrADHP;
		public static RWYData m_SelectedRWY;


		#region "Display && units managment"
		public static int DistanceUnitIndex;
		public static int HeightUnitIndex;
		public static int SpeedUnitIndex;

		public static TypeConvert[] DistanceConverter = new TypeConvert[2];
		public static TypeConvert[] HeightConverter = new TypeConvert[2];
		public static TypeConvert[] SpeedConverter = new TypeConvert[2];
		public static TypeConvert[] DSpeedConverter = new TypeConvert[2];

		#endregion

		static public void InitUnits()
		{
			double DistancePrecision = Functions.RegRead<Double>(Registry.CurrentUser, PandaRegKey, "DistancePrecision", 0.1);
			double HeightPrecision = Functions.RegRead<Double>(Registry.CurrentUser, PandaRegKey, "HeightPrecision", 1.0);
			double DSpeedPrecision = Functions.RegRead<Double>(Registry.CurrentUser, PandaRegKey, "DSpeedPrecision", 1);
			double SpeedPrecision = Functions.RegRead<Double>(Registry.CurrentUser, PandaRegKey, "SpeedPrecision", 1);

			DistanceUnitIndex = Functions.RegRead<Int32>(Registry.CurrentUser, PandaRegKey, "Distance", 0);
			if (DistanceUnitIndex < 0 || DistanceUnitIndex > 1) DistanceUnitIndex = 0;

			HeightUnitIndex = Functions.RegRead<Int32>(Registry.CurrentUser, PandaRegKey, "Height", 0);
			if (HeightUnitIndex < 0 || HeightUnitIndex > 1) HeightUnitIndex = 0;

			SpeedUnitIndex = Functions.RegRead<Int32>(Registry.CurrentUser, PandaRegKey, "Speed", 0);
			if (SpeedUnitIndex < 0 || SpeedUnitIndex > 1) SpeedUnitIndex = 0;
			//========================================================================

			DistanceConverter[0].Multiplier = 0.001;
			DistanceConverter[0].Rounding = DistancePrecision;
			DistanceConverter[0].Unit = UnitConverter.DistanceUnitKm;
			//"км" '"kM"
			DistanceConverter[1].Multiplier = 1.0 / 1852.0;
			DistanceConverter[1].Rounding = DistancePrecision;
			DistanceConverter[1].Unit = UnitConverter.DistanceUnitNM;
			//"ММ" '"NM"

			HeightConverter[0].Multiplier = 1.0;
			HeightConverter[0].Rounding = HeightPrecision;
			HeightConverter[0].Unit = UnitConverter.HeightUnitM;
			//"meter"
			HeightConverter[1].Multiplier = 1.0 / 0.3048;
			HeightConverter[1].Rounding = HeightPrecision;
			HeightConverter[1].Unit = UnitConverter.HeightUnitFt;
			//"фт" '"feet"

			SpeedConverter[0].Multiplier = 1.0;
			SpeedConverter[0].Rounding = SpeedPrecision;
			SpeedConverter[0].Unit = UnitConverter.SpeedUnitKm_H;
			//"км/ч" '"km/h"
			SpeedConverter[1].Multiplier = 1.0 / 1.852;
			SpeedConverter[1].Rounding = SpeedPrecision;
			SpeedConverter[1].Unit = UnitConverter.SpeedUnitKt;
			//"узлы" '"Kt"

			DSpeedConverter[0].Multiplier = 1.0;
			DSpeedConverter[0].Rounding = SpeedPrecision;
			DSpeedConverter[0].Unit = UnitConverter.DSpeedUnitM_Min;
			//"m/min" '"m/min"
			DSpeedConverter[1].Multiplier = 1.0 / 0.3048;
			DSpeedConverter[1].Rounding = SpeedPrecision;
			DSpeedConverter[1].Unit = UnitConverter.DSpeedUnitFt_Min;
			//"ft/min" '"ft/min"
		}

		public static bool InitCommand(int cmd)
		{
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

			IWorkspace pWorkspace = default(IWorkspace);
			IEnumDataset pDatasetsEn = default(IEnumDataset);
			IGeoDataset pGeoDataset = default(IGeoDataset);

			IGeographicCoordinateSystem pGeoCoordSys = default(IGeographicCoordinateSystem);
			ISpatialReferenceFactory2 pSpatRefFact = default(ISpatialReferenceFactory2);

			UserName = DBModule.InitModule();

			pWorkspace = DBModule.pObjectDir.Workspace as IWorkspace;

			if (pWorkspace != null)
			{
				pDatasetsEn = pWorkspace.get_Datasets(esriDatasetType.esriDTAny);
				pDatasetsEn.Reset();

				pGeoDataset = pDatasetsEn.Next() as IGeoDataset;
				pSpRefShp = pGeoDataset.SpatialReference;
			}

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
			/*
			string Achar = Functions.RegRead<String>(Registry.CurrentUser, ModuleRegKey + "\\" + ModuleName, "Acar", "");
			p_LicenseRect = DecoderCode.DecodeLCode(Achar, ModuleName);

			if (DecoderCode.LstStDtWriter(ModuleName) != 0)
				throw new Exception("CRITICAL ERROR!!");

			if (p_LicenseRect.IsEmpty)
				throw new Exception("ERROR #10LR512");
			*/
			//==============================================================
			ConstDir = Functions.RegRead<String>(PANDARootKey, PandaRegKey, "ConstDir", null);
			if (ConstDir == null)
				throw new Exception("Constants path not exists.");

			InitUnits();

			//==============================================================
			PANS_OPS_DataBase.InitModule();
			Navaids_DataBase.InitModule();
			//==============================================================

			AIXM.IObjectList icaoPrefixList = new AIXM.ObjectList();
			icaoPrefixList.Add("-1");

			DBModule.FillADHPList(out ADHPList, icaoPrefixList);

			win32Window = new Win32Window(Application.hWnd);
			return true;
		}

		public static string GetMapFileName()
		{
			return Application.Templates.get_Item(Application.Templates.Count - 1);
		}

		public static IMap GetMap()
		{
			return (Application.Document as IMxDocument).FocusMap;
		}

		public static IActiveView GetActiveView()
		{
			ESRI.ArcGIS.ArcMapUI.IMxDocument pDocument = default(ESRI.ArcGIS.ArcMapUI.IMxDocument);
			pDocument = Application.Document as IMxDocument;
			return pDocument.ActiveView;
		}

		public static int GetApplicationHWnd()
		{
			return Application.hWnd;
		}
	}
}
