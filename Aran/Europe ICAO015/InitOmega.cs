using System;
using System.Linq;
using System.Windows.Forms;
using Aran.Omega.Properties;
using Aran.Omega.SettingsUI;
using Aran.PANDA.Common;
using Aran.Geometries.Operators;
using Aran.Aim.Features;
using System.Runtime.InteropServices;
using Aran.Geometries.SpatialReferences;

namespace Europe_ICAO015
{
    public static class InitOmega
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetThreadLocale(int dwLangID);

        public static int ObstacleDistanceFromArp = 20000;

        public static bool InitCommand()
        {
            try
            {
                //GlobalParams.Logs = "";

                if (GlobalParams.Settings == null)
                    GlobalParams.Settings = new OmegaSettings();

                GlobalParams.Settings.Load(GlobalParams.AranEnvironment);
                var settings = GlobalParams.Settings;

                if (settings == null)
                    throw new Exception("Settings is empty!Please first save the settings");

                if (GlobalParams.Constant == null)
                    GlobalParams.Constant = new Aran.PANDA.Constants.Constants();

                GlobalParams.UI = GlobalParams.AranEnvironment.Graphics;

                if (GlobalParams.GeomOperators == null)
                {
                    // GlobalParams.GeomOperators = new Aran.Geometries.Operators.GeometryOperators();
                    GlobalParams.GeomOperators = new Aran.Geometries.Operators.JtsGeometryOperators();
                }

                GlobalParams.SpatialRefOperation = new SpatialReferenceOperation(GlobalParams.AranEnvironment);

                GlobalParams.Database = new DBModule();

                //                var obstacleArea = GlobalParams.Database.OmegaQPI.GetObstacleArea();

                AirportHeliport adhp = GlobalParams.Database.AirportHeliport;
                InitOmega.ObstacleDistanceFromArp = Convert.ToInt32(settings.OLSQuery.Radius);

                if (adhp != null)
                {

                    SpatialReferenceParam centralMeridianParam = GlobalParams.SpatialRefOperation.SpRefPrj
                                        .ParamList.FirstOrDefault(param => param.SRParamType == SpatialReferenceParamType.srptCentralMeridian);
                    double centralMeridian = centralMeridianParam.Value;

                    if (Math.Abs(adhp.ARP.Geo.X - centralMeridian) > 3)
                    {
                        System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show(ICAO015.Properties.Resources.Central_meridian_is_not_correct, "Omega", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        return false;
                        //if (result== System.Windows.Forms.DialogResult.OK)
                        //    GlobalParams.SpatialRefOperation.ChangeCentralMeridian(adhp.ARP.Geo);
                    }
                }

                Langcode = 1093;
                SetThreadLocale((int)Langcode);
                ICAO015.Properties.Resources.Culture = new System.Globalization.CultureInfo((int)Langcode);


                _distanceUnit = settings.OLSInterface.DistanceUnit;
                _distancePrecision = settings.OLSInterface.DistancePrecision;

                _heightUnit = settings.OLSInterface.HeightUnit;
                _heightPrecision = settings.OLSInterface.HeightPrecision;

                double multiplier;
                string unit;
                if (_distanceUnit == VerticalDistanceType.M)
                {
                    multiplier = 1;
                    unit = ICAO015.Properties.Resources.str111;
                }
                else
                {
                    multiplier = 1.0 / 1852.0;
                    unit = ICAO015.Properties.Resources.str520;
                }
                DistanceConverter = new TypeConvert { MultiPlier = multiplier, Rounding = _distancePrecision, Unit = unit };

                if (_heightUnit == VerticalDistanceType.M)
                {
                    multiplier = 1.0;
                    unit = ICAO015.Properties.Resources.str111;
                }
                else if (_heightUnit == VerticalDistanceType.Ft)
                {
                    multiplier = 1.0 / 0.3048;
                    unit = ICAO015.Properties.Resources.str112;
                }

                HeightConverter = new TypeConvert { MultiPlier = multiplier, Rounding = _heightPrecision, Unit = unit };

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error init loading parametrs!", "Omega", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }

        public static int Langcode { get; private set; }

        public static TypeConvert DistanceConverter { get; private set; }

        public static TypeConvert HeightConverter { get; private set; }

        public static TypeConvert SpeedConverter { get; private set; }

        public static TypeConvert DSpeedConverter { get; private set; }

        private static double _distancePrecision;
        private static double _heightPrecision;
        private static VerticalDistanceType _distanceUnit;
        private static VerticalDistanceType _heightUnit;
    }

    public struct TypeConvert
    {
        public double MultiPlier { get; set; }
        public double Rounding { get; set; }
        public string Unit { get; set; }
    }
}
