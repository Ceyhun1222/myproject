using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartTypeA
{

    public enum roundType
    {
        realValue = 0,
        toDown = 1,
        toNearest = 2,
        toUp = 3,
    }

    public static class Common
    {
        public const double MinAltitude = 300;
        public const double SecondAltitude = 4250;
        public const double ThirdAltitude = 6100;
        public const double MaxAltitude = 10350;
        public const double MinEnrouteDistance = 0;
        public const double MaxEnrouteDistance = 300000;//Max 300 km 
        public const double MaxStarDownTo30Distance = 56000;//Max 56 km in StarDownTo30Distance
        public const double constDoc = 370400;
        public const double MinStarUpTO30Distance = 56005;
        public const double MaxMissAprchDownTo15 = 28002;

        public static double ConvertDistance(double Val_Renamed, roundType roundMode)
        {
            TypeConvert distanceConvert = InitChartTypeA.DistanceConverter;
            if (((int)roundMode < 0) | ((int)roundMode > 3))
                roundMode = 0;
            switch ((int)roundMode)
            {
                case 0:
                    return Val_Renamed * distanceConvert.MultiPlier;
                case 1:
                    return System.Math.Round(Val_Renamed * distanceConvert.MultiPlier / distanceConvert.Rounding - 0.4999) * distanceConvert.Rounding;
                case 2:
                    {
                        return System.Math.Round(Val_Renamed * distanceConvert.MultiPlier / distanceConvert.Rounding) * distanceConvert.Rounding;
                    }
                case 3:
                    return System.Math.Round(Val_Renamed * distanceConvert.MultiPlier / distanceConvert.Rounding + 0.4999) * distanceConvert.Rounding;
            }
            return Val_Renamed;
        }

        public static double ConvertHeight(double Val_Renamed, roundType RoundMode)
        {
            TypeConvert heightConvert = InitChartTypeA.HeightConverter;
            if (((int)RoundMode < 0) | ((int)RoundMode > 3))
                RoundMode = 0;
            switch ((int)RoundMode)
            {
                case 0:
                    return Val_Renamed * heightConvert.MultiPlier;
                case 1:
                    return System.Math.Round(Val_Renamed * heightConvert.MultiPlier / heightConvert.Rounding - 0.4999) * heightConvert.Rounding;
                case 2:
                    return System.Math.Round(Val_Renamed * heightConvert.MultiPlier / heightConvert.Rounding) * heightConvert.Rounding;
                case 3:
                    return System.Math.Round(Val_Renamed * heightConvert.MultiPlier / heightConvert.Rounding + 0.4999) * heightConvert.Rounding;
            }
            return Val_Renamed;
        }
        
      
        public static double DeConvertDistance(double Val_Renamed)
        {
            return Val_Renamed / InitChartTypeA.DistanceConverter.MultiPlier;
        }

        public static double DeConvertHeight(double Val_Renamed)
        {
            return Val_Renamed / InitChartTypeA.HeightConverter.MultiPlier;
        }


        public static double RoundByDistance(double Val_Renamed, roundType RoundMode,double rounding)
        {
            if (((int)RoundMode < 0) | ((int)RoundMode > 3))
                RoundMode = 0;
            switch ((int)RoundMode)
            {
                case 0:
                    return Val_Renamed ;
                case 1:
                    return System.Math.Round(Val_Renamed  / rounding - 0.4999) * rounding;
                case 2:
                    return System.Math.Round(Val_Renamed  / rounding) * rounding;
                case 3:
                    return System.Math.Round(Val_Renamed  / rounding + 0.4999) * rounding;
            }
            return Val_Renamed;
        }


    }
}
