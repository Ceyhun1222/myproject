using System;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Panda.Common;
using System.Threading;
using Aran.Aim.Data;
using System.Reflection;
using Aran.Queries;
using Aran.Panda.Constants;

namespace Aran.Panda.Conventional.Racetrack
{
//    public static class InitHolding
//    {
//        //public const string pANSOPSVersion = "DOC 8168 OPS/611    Fifth Edition - 2006";
//        //public const string helpFile = "PANDA.chm";
//        public const string ModulName = "Racetrack";
//        //public const int MaxABCatAltitudeVal = 4250;
//        //public const int MaxCDECatAltitudeVal = 10350;
//        //public const int MinCatAltitudeVal = 300;
//        //public static MainController ConvRacetrackModel;
//        //public static FormMain FrmConventialInitial;
//        //public static FormReport FrmReport;
//        //public static Win32Windows win32Window;
//        //public const double SysCompTolerance = 463;

////		private static Navaids_DataBase _navaid_database;
//        //private static string _pandaInstallDir;
//        //private static SpeedCategories _speedCategories;

//        //public static bool InitCommand ( )
//        //{
//        //    try
//        //    {
//        //        GlobalParams.Settings = new Settings();
//        //        GlobalParams.Settings.Load(GlobalParams.AranEnvironment.PandaAranExt);
//        //        GlobalParams.UnitConverter = new UnitConverter(GlobalParams.Settings);

//        //        //ModuleInstallDir = ARANFunctions.RegRead<string>(Registry.CurrentUser, RegKeys.Panda, RegKeys.InstallDirKeyName, Assembly.GetExecutingAssembly().Location);

//        //        if (GlobalParams.Constant_G == null)
//        //            GlobalParams.Constant_G = new Aran.Panda.Constants.Constants();

//        //        if (GlobalParams.GeomOperators == null)
//        //            GlobalParams.GeomOperators = new GeometryOperators();

//        //        GlobalParams.SpatialRefOperation = new SpatialReferenceOperation(GlobalParams.AranEnvironment);

//        //        GlobalParams.Database = new DBModule((IDbProvider)GlobalParams.AranEnvironment.DbProvider);
//        //        ExtensionFeature.CommonQPI = GlobalParams.Database.HoldingQpi;

//        //        if (_speedCategories == null)
//        //            _speedCategories = new SpeedCategories();
//        //        if (_navaid_database == null)
//        //            _navaid_database = new Navaids_DataBase(GlobalParams.Constant_G.InstallDir);

//        //        Racetrack.Properties.Resources.Culture = Thread.CurrentThread.CurrentUICulture;
//        //        Acar = ARANFunctions.RegRead<string>(Microsoft.Win32.Registry.CurrentUser, RegKeys.Conventional + "\\" + ModulName, RegKeys.LicenseKeyName, null);

//        //        //LicenseRectGeo = DecoderCode.DecodeLCode(Acar, modulName, GlobalParams.SpatialRefOperation.SpRefGeo, GlobalParams.SpatialRefOperation.SpRefPrj);
//        //        //LicenseRectPrj = GlobalParams.SpatialRefOperation.ToPrj<Aran.Geometries.MultiPolygon>(LicenseRectGeo););				
//        //        //if ( DecoderCode.LstStDtWriter ( RegKeys.Conventional + "\\" + modulName ) != 0 )
//        //        //    throw new Exception ( "CRITICAL ERROR!!" );

//        //        //if ( LicenseRectGeo == null || LicenseRectGeo.Count == 0 )
//        //        //    throw new Exception ( "ERROR #10LR512" );

//        //        //FlightRecieverValue = new EnumArray<string, flightReciever>();
//        //        //FlightRecieverValue[flightReciever.GNSS] = "GNSS";
//        //        //FlightRecieverValue[flightReciever.VORDME] = "VOR-DME";
//        //        //FlightRecieverValue[flightReciever.DMEDME] = "DME-DME";

//        //        //FlightPhaseValue = new EnumArray<string, flightPhase>();
//        //        //FlightPhaseValue[flightPhase.Enroute] = "Enroute";
//        //        //FlightPhaseValue[flightPhase.STARUpTo30] = "STARUpTo30";
//        //        //FlightPhaseValue[flightPhase.STARDownTo30] = "STARDownTo30";

//        //        ARANFunctions.InitEllipsoid();
//        //        //ConvRacetrackModel = new MainController();
//        //        return true;
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        MessageBox.Show(e.Message);
//        //        return false;
//        //    }
//        //}

//        //public static Navaids_DataBase Navaid_Database
//        //{
//        //    get
//        //    {
//        //        return _navaid_database;
//        //    }

//        //}

//        //public static SpeedCategories SpeedCategories
//        //{
//        //    get
//        //    {
//        //        return _speedCategories;
//        //    }

//        //}

//        //public static string RNAVInstalDir
//        //{
//        //    get;
//        //    private set;
//        //}

//        //public static string ModuleInstallDir
//        //{
//        //    get
//        //    {
//        //        return _pandaInstallDir;
//        //    }
//        //    set
//        //    {
//        //        _pandaInstallDir = value;
//        //        int constantsIndex = _pandaInstallDir.IndexOf(@"\constants");
//        //        if (constantsIndex != -1)
//        //        {
//        //            _pandaInstallDir = _pandaInstallDir.Remove(constantsIndex);
//        //        }
//        //    }
//        //}

//        public static string Acar
//        {
//            get;
//            private set;
//        }

//        public static MultiPolygon LicenseRectGeo
//        {
//            get;
//            set;
//        }

//        public static MultiPolygon LicenseRectPrj
//        {
//            get;
//            set;
//        }

//        public static AircraftCategoryList AircraftCtList
//        {
//            get;
//            private set;
//        }
//    }
}