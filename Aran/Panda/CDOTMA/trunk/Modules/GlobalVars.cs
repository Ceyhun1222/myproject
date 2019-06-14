using Microsoft.Win32;
using System;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

using NetTopologySuite.Geometries;

using CDOTMA.CoordinatSystems;
using Converters;
//using Aran.PANDA.Common;

namespace CDOTMA
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public static class GlobalVars
	{
		#region "Public Constants"
		#region "PANS-OPS Constants"    '   Срочно: внести в базу PANSOPS

		public const double SIDTerminationFIXToler = 5000.0;
		//public static Constants constants = null;

        public const double f05 = 0.087266462599716;	//ARANMath.DegToRad(15.0);
        public const double f15 = 0.26179938779914941;	//ARANMath.DegToRad(15.0);
		public const double f120 = 2.2689280275926285;	//ARANMath.DegToRad(130);
		public const double f270 = 4.7123889803846897;	//ARANMath.DegToRad(270);

		#endregion

		#region "Product releated"
		public const string PANSOPSVersion = "DOC 8168 OPS/611    Fifth Edition - 2006";
		public const string ModuleName = "Departure";
		//public const string ModuleCategory = "RNAV";

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
		public static int CurrCmd = -1;
		//public static string ConstDir;

		public static MultiPolygon p_LicenseRect;
		public static string UserName;		//public static string UserName;
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

		#endregion

		#region "Document, projection, e.t.c."

		private static bool _errorHandled = false;
		public static Settings settings;
		public static UnitConverter unitConverter;

		//public static NavaidsConstant navaidsConstants;
		//public static IHookHelper gHookHelper;
		//public static IAranEnvironment gAranEnv;
		//public static IAranGraphics gAranGraphics;
		//public static IMap pMap;
		//public static IGeographicCoordinateSystem pGCS;
		//public static IProjectedCoordinateSystem pPCS;
		//public static SpatialReferenceOperation pspatialReferenceOperation;

		public static MainForm mainForm;

		public static double ModellingRadius = 150000;
		public static double RModel;

		static CoordinatSystem _pSpRefPrj;			//viewCS
		public static CoordinatSystem pSpRefPrj		//viewCS
		{
			get { return _pSpRefPrj; }
			set
			{
				_pSpRefPrj = value;

				if (_pSpRefPrj != null)// && _pSpRefPrj is ProjectedCoordinatSystem)
				{
					ProjectedCoordinatSystem pPCS = (ProjectedCoordinatSystem)_pSpRefPrj;
					NativeMethods.InitProjection(pPCS.CentralMeridian, pPCS.LatitudeOfOrigin, pPCS.ScaleFactor, pPCS.FalseEasting, pPCS.FalseNorthing);

					//Point pPtPrj1 = new Point(634665.06, 4476214.35);
					//Point pPtPrj2 = new Point(634633.58, 4476196.27);
					//Point pPtPrj3 = new Point(634607.15, 4476238.98);
					//Point pPtPrj4 = new Point(634640.97, 4476252.44);

					//Point pPtGeo1 = Functions.ToGeo(pPtPrj1) as Point;
					//Point pPtGeo2 = Functions.ToGeo(pPtPrj2) as Point;
					//Point pPtGeo3 = Functions.ToGeo(pPtPrj3) as Point;
					//Point pPtGeo4 = Functions.ToGeo(pPtPrj4) as Point;
				}
			}
		}

		public static CoordinatSystem _pSpRefGeo;
		public static CoordinatSystem pSpRefGeo
		{
			get { return _pSpRefGeo; }
			set
			{
				_pSpRefGeo = value;

				if (_pSpRefGeo != null)
					NativeMethods.InitEllipsoid(((GeographicCoordinatSystem)_pSpRefGeo).datum.spheroid.SemiMajorAxis,
						((GeographicCoordinatSystem)_pSpRefGeo).datum.spheroid.InversFlattening);
			}
		}

		public static Spheroid pSpheroid;

		#endregion

		#region "DB"
		public static ADHPType CurrADHP;
		public static RWYType[] RWYList;

		//public static NavaidType[] NavaidList;
		//public static NavaidType[] DMEList;
		//public static WPT_FIXType[] WPTList;

		public static List<ADHPType> ADHPList;

		public static List<NavaidType> NavaidList;
		public static List<NavaidType> DMEList;
		public static List<WPT_FIXType> WPTList;
		//public static List<ProcedureType> Procedures;

		public static List<ProcedureType> ApproachProcedures;
		public static List<ProcedureType> ArrivalProcedures;
		public static List<ProcedureType> DepartureProcedures;
		public static List<ProcedureType> Routs;

		public static List<AirspaceType> AstList;
		public static List<AirspaceVolumeType> AirspaceVolumeList;

		public static List<TraceLeg> TraceLegs;
		public static List<LegPoint> LegPoints;

		//public static ObstacleType[] ObstacleList;
		#endregion

		#endregion

		public static void InitCommand()
		{
			//HandleThreadException();

			//bool isExists;
			//ConstDir = RegFuncs.GetConstantsDir(out isExists);

			//if (!isExists)
			//	throw new Exception("Installation path not exists.");

			pSpRefGeo = new GeographicCoordinatSystem();
			_pSpRefPrj = null;
		}

        public static string ApplicationDir
        {
            get { return System.IO.Path.GetDirectoryName(typeof(GlobalVars).Assembly.Location); }
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

			System.Windows.Forms.Application.ThreadException += 
				new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

			_errorHandled = true;
		}

		//public static string GetMapFileName()
		//{
		//	return ??????????????????;
		//}

	}
}
