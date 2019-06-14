namespace ChartPApproachTerrain  
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
        public const double xStart = 3.5;
        public const double yStart = 12;
        public const double horScale = 2500;
        public const double verScale = 500;
        public const double areaWidth = 900;
        public const double areaHeight = 120;
        public const double offsetValueInM = 3;
        
        public static double ConvertDistance(double Val_Renamed, roundType roundMode)
        {
            TypeConvert distanceConvert = InitChartPAT.DistanceConverter;
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

        public static double DeConvertDistance(double Val_Renamed)
        {
            return Val_Renamed / InitChartPAT.DistanceConverter.MultiPlier;
        }

        public static double DeConvertHeight(double Val_Renamed)
        {
            return Val_Renamed / InitChartPAT.HeightConverter.MultiPlier;
        }


    }
}
