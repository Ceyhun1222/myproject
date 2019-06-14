using System;
using System.Collections.Generic;
using Aran.PANDA.Common;

namespace ChartPApproachTerrain  
{
    internal class InitChartPAT
    {
        public static bool InitCommand()
        {
            try
            {
                 
                ARANFunctions.InitEllipsoid();
                if (GlobalParams.HookHelper.FocusMap.SpatialReference == null) return false;
                GlobalParams.SpatialRefOperation =
                    new SpatialReferenceOperation(GlobalParams.HookHelper.FocusMap.SpatialReference);

                if (GlobalParams.UI == null)
                    GlobalParams.UI = new Graphics();

                double multiplier = 1.0;
                string unit = "M";
                _distancePrecision = 1;
                _heightPrecision = 1;

                DistanceConverterList = new List<TypeConvert> { new TypeConvert { MultiPlier = multiplier, Rounding = _distancePrecision, Unit = unit } };
                DistanceConverter = DistanceConverterList[0];

                HeightConverterList = new List<TypeConvert>();
                multiplier = 1.0;
                unit = "M";
                HeightConverterList.Add(new TypeConvert { MultiPlier = multiplier, Rounding = _heightPrecision, Unit = unit });

                multiplier = 1.0 / 0.3048;
                unit = "FT";
                HeightConverterList.Add(new TypeConvert { MultiPlier = multiplier, Rounding = _heightPrecision, Unit = unit });

                HeightConverter = HeightConverterList[0];// new TypeConvert { MultiPlier = multiplier, Rounding = _heightPrecision, Unit = unit };

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static TypeConvert DistanceConverter { get;set; }

        public static TypeConvert HeightConverter { get; set; }

        public static List<TypeConvert> HeightConverterList { get; set; }
        public static List<TypeConvert> DistanceConverterList { get; set; }

        private static double _distancePrecision;
        private static double _heightPrecision;
        private static double _speedPrecision;
        private static double _dSpeedPrecision;
        private static HorizantalDistanceType _distanceUnit;
        private static VerticalDistanceType _heightUnit;

    }

    public struct TypeConvert
    {
        public double MultiPlier { get; set; }
        public double Rounding { get; set; }
        public string Unit { get; set; }
    }
}
