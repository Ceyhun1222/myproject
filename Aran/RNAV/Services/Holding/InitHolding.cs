using System;
using Microsoft.Win32;
using System.Runtime.InteropServices;
//using Holding.Convential;
using System.Windows.Forms;
using Aran.AranEnvironment;
using Aran.Geometries;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
using Aran.Aim.Features;
using Aran.Panda.Rnav.Holding.Properties;
using Aran.PANDA.Rnav.Holding.Properties;

namespace Holding
{

    public static class InitHolding
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetThreadLocale(int dwLangID);

        [DllImport("MathRNAV.dll", EntryPoint = "_InitAll@0")]
        static extern void InitAll();

        [DllImport("hhctrl.ocx", EntryPoint = "HtmlHelpA")]
        public static extern int HtmlHelp(int hwndCaller, string pszFile, int uCommand, int dwData);

        public const string pANSOPSVersion = "DOC 8168 OPS/611    Fifth Edition - 2006";
        //public const string pandaRegKey = "SOFTWARE\\RISK\\Panda";
        //public const string rnavRegKey = "SOFTWARE\\RISK\\RNAV";
        public const string HelpFile = "RNAV.chm";
        public const string ModulName = "HoldingR";
        public const string KeyName = "Acar";

        public const int MaxAbCatAltitudeVal = 4250;
        public const int MaxCdeCatAltitudeVal = 103500;
        public const int MinCatAltitudeVal = 300;

        public static Win32Windows Win32Window;
        public const double SysCompTolerance = 463;

        public const int HhHelpContext = 0xf;
        // Text pop-up help, similar to WinHelp's HELP_CONTEXTMENU.


        public static bool InitCommand()
        {
            try
            {
                NativeMethods.InitAll();

                if (GlobalParams.AranSettings == null)
                    GlobalParams.AranSettings = new Aran.PANDA.Common.Settings();
                GlobalParams.AranSettings.Load(GlobalParams.AranEnvironment);
                var settings = GlobalParams.AranSettings;
                GlobalParams.UI = GlobalParams.AranEnvironment.Graphics;
                GlobalParams.Radius = settings.Radius;

                if (GlobalParams.Constant_G == null)
                    GlobalParams.Constant_G = new Constants();

                if (GlobalParams.GeomOperators == null)
                    GlobalParams.GeomOperators = new Aran.Geometries.Operators.GeometryOperators();

                GlobalParams.SpatialRefOperation = new SpatialReferenceOperation(GlobalParams.AranEnvironment);
                try
                {
                    if (GlobalParams.Database == null)
                        GlobalParams.Database = new DBModule();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Database error"+e.Message, Resources.Holding_Caption);
                    return false;
                }

                //CurOrganization = GlobalParams.Database.HoldingQpi.GetOrganisation(GlobalParams.AranSettings.Organization);
                //CurAdhp =(AirportHeliport) GlobalParams.Database.HoldingQpi.GetFeature(Aran.Aim.FeatureType.AirportHeliport, GlobalParams.AranSettings.Aeroport);

                CurAdhp = GlobalParams.Database.HoldingQpi.GetAirportHeliport(GlobalParams.AranSettings.Aeroport);

                //if (_speedCategories == null)
                //    _speedCategories = new SpeedCategories();
                if (_navaid_database == null)
                    _navaid_database = new Navaids_DataBase();
                ////string a = _AssemblyName;

                Langcode = settings.Language;
                SetThreadLocale((int)Langcode);
                //Holding.Properties.Resources.Culture = new System.Globalization.CultureInfo((int)Langcode);
                _distanceUnit = settings.DistanceUnit;

                _distancePrecision = settings.DistancePrecision;

                _heightUnit = settings.HeightUnit;

                _heightPrecision = settings.HeightPrecision;

                _speedUnit = settings.SpeedUnit;

                //_dSpeedUnit = settings.Get;

                _speedPrecision = settings.SpeedPrecision;

                _dSpeedPrecision = _speedPrecision;

				//Acar = RegKeys.RegRead<string>(Microsoft.Win32.Registry.CurrentUser, RegKeys.RNAV + "\\" + ModulName, KeyName, null);
				//LicenseRectGeo = DecoderCode.DecodeLCode(Acar, ModulName, GlobalParams.SpatialRefOperation.SpRefGeo, GlobalParams.SpatialRefOperation.SpRefPrj);

				//LicenseRectPrj = GlobalParams.SpatialRefOperation.ToPrj<Aran.Geometries.MultiPolygon>(LicenseRectGeo);
                if (LicenseRectGeo!=null && !LicenseRectGeo.IsEmpty)
                    LicenseRectPrj = GlobalParams.SpatialRefOperation.ToPrj<Aran.Geometries.MultiPolygon>(LicenseRectGeo);

				//if (DecoderCode.LstStDtWriter(RegKeys.RNAV + "\\" + ModulName) != 0)
				//    throw new Exception("CRITICAL ERROR!!");

				//if (LicenseRectGeo == null || LicenseRectGeo.Count == 0)
				//    throw new Exception("ERROR #10LR512");


                double multiplier;
                string unit = "";
                if (_distanceUnit == HorizantalDistanceType.KM)
                {
                    multiplier = 0.001;
                    unit = Resources.str519;
                }
                else
                {
                    multiplier = 1.0 / 1852.0;
                    unit = Resources.str520;
                }
                DistanceConverter = new TypeConvert { MultiPlier = multiplier, Rounding = _distancePrecision, Unit = unit };

                if (_heightUnit == VerticalDistanceType.M)
                {
                    multiplier = 1.0;
                    unit = Resources.str111;
                }
                else if (_heightUnit == VerticalDistanceType.Ft)
                {
                    multiplier = 1.0 / 0.3048;
                    unit = Resources.str112;
                }

                HeightConverter = new TypeConvert { MultiPlier = multiplier, Rounding = _heightPrecision, Unit = unit };

                if (_speedUnit == HorizantalSpeedType.KMInHour)
                {
                    multiplier = 3.6;
                    unit = Resources.str125;//"km/h";
                }
                else if (_speedUnit == HorizantalSpeedType.Knot)
                {
                    multiplier = 3.6 / 1.852;
                    unit = Resources.str126; // '"Kt"
                }

                SpeedConverter = new TypeConvert { MultiPlier = multiplier, Rounding = _speedPrecision, Unit = unit };

                if (_dSpeedUnit == VerticalSpeedType.MeterInMin)
                {
                    multiplier = 60;
                    unit = Resources.str149;
                }
                else if (_dSpeedUnit == VerticalSpeedType.FeetInMin)
                {
                    multiplier = 60.0 / 0.3048;
                    unit = Resources.str150;
                }

                DSpeedConverter = new TypeConvert { MultiPlier = multiplier, Rounding = _dSpeedPrecision, Unit = unit };

                FlightRecieverValue = new EnumArray<string, flightReciever>();
                FlightRecieverValue[flightReciever.GNSS] = "GNSS";
                FlightRecieverValue[flightReciever.VORDME] = "VOR-DME";
                FlightRecieverValue[flightReciever.DMEDME] = "DME-DME";

                FlightPhaseValue = new EnumArray<string, flightPhase>();
                FlightPhaseValue[flightPhase.Enroute] = "Enroute";
                FlightPhaseValue[flightPhase.STARUpTo30] = "STARUpTo30";
                FlightPhaseValue[flightPhase.STARDownTo30] = "STARDownTo30";
                FlightPhaseValue[flightPhase.IAFDownTo30] = "IAFDownTo30";
                FlightPhaseValue[flightPhase.IFDownTo30] = "IFDownTo30";
                FlightPhaseValue[flightPhase.MissAprchDownTo30] = "MissAprchDownTo30";
                FlightPhaseValue[flightPhase.MissAprchDownTo15] = "MissAprchDownTo15";
                return true;
            }
            catch (Exception e)
            {
                GlobalParams.AranEnvironment.GetLogger("Rnav Holding").Error(e);
                MessageBox.Show("Error loading init parametrs", Resources.Holding_Caption);
                return false;
            }

            // ConvRacetrackModel = new ConventialRacetrack (); 
        }

        public static Navaids_DataBase Navaid_Database 
        {
            get{return _navaid_database;}
        }

        //public static SpeedCategories SpeedCategories
        //{
        //    get { return _speedCategories; }

        //}

        public static EnumArray<string, flightReciever> FlightRecieverValue;
        public static EnumArray<string, flightPhase> FlightPhaseValue;

        public static TypeConvert DistanceConverter { get; private set; }

        public static TypeConvert HeightConverter { get; private set; }

        public static TypeConvert SpeedConverter { get; private set; }

        public static TypeConvert DSpeedConverter { get; private set; }

        public static int Langcode { get; private set; }

        public static string RNAVInstalDir { get; private set; }

        public static string PandaInstalDir { get; set; }

        public static string Acar { get; private set; }

        public static MultiPolygon LicenseRectGeo { get; set; }

        public static MultiPolygon LicenseRectPrj { get; set; }

        public static HorizantalDistanceType DistanceUnit { get { return _distanceUnit; } }

        public static VerticalDistanceType HeightUnit { get { return _heightUnit; } }

        public static HorizantalSpeedType SpeedUnit { get { return _speedUnit; } }

        public static AircraftCategoryList AircraftCtList { get; private set; }

        public static double SpeedPrecision { get { return _speedPrecision; } }

        public static double HeightPrecision { get { return _heightPrecision; } }

        public static double DistancePrecision { get { return _distancePrecision; } }

        public static IAranEnvironment Env { get; set; }

        public static OrganisationAuthority CurOrganization { get; private set; }

        public static AirportHeliport CurAdhp { get; private set; }


        private static Navaids_DataBase _navaid_database;
        private static double _distancePrecision;
        private static double _heightPrecision;
        private static double _speedPrecision;
        private static double _dSpeedPrecision;
        private static HorizantalDistanceType _distanceUnit;
        private static VerticalDistanceType _heightUnit;
        private static HorizantalSpeedType _speedUnit;
        private static VerticalSpeedType _dSpeedUnit;
      //  private static SpeedCategories _speedCategories;

        //private  static TypeConvert[] distanceParam;
        //private static TypeConvert[] heightParam;
        //private static TypeConvert[] speedParam;
        //private static TypeConvert[] dSpeedParam;


    }
}
