using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Collections;

namespace Holding
{
    //public enum procedureType
    //{
    //    withHoldingFunctionality = 0,
    //    withoutHoldingFunctiolity = 1,
    //    rNPHolding = 2
    //}

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
			TypeConvert distanceConvert = InitHolding.DistanceConverter;
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
			TypeConvert heightConvert = InitHolding.HeightConverter;
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

		public static double ConvertSpeed_(double Val_Renamed, roundType RoundMode)
		{
			TypeConvert speedConvert = InitHolding.SpeedConverter;
			if ((RoundMode < 0) | ((int)RoundMode > 3))
				RoundMode = 0;
			switch ((int)RoundMode)
			{
				case 0:
					return Val_Renamed * speedConvert.MultiPlier;
				case 1:
					return System.Math.Round(Val_Renamed * speedConvert.MultiPlier / speedConvert.Rounding - 0.4999) * speedConvert.Rounding;
				case 2:
					return System.Math.Round(Val_Renamed * speedConvert.MultiPlier / speedConvert.Rounding) * speedConvert.Rounding;
				case 3:
					return System.Math.Round(Val_Renamed * speedConvert.MultiPlier / speedConvert.Rounding + 0.4999) * speedConvert.Rounding;
			}
			return Val_Renamed;
		}

		public static double ConvertDSpeed(double Val_Renamed, roundType RoundMode)
		{
			TypeConvert dSpeedConvert = InitHolding.DSpeedConverter;
			if ((RoundMode < 0) | ((int)RoundMode > 3))
				RoundMode = 0;
			switch ((int)RoundMode)
			{
				case 0:
					return Val_Renamed * dSpeedConvert.MultiPlier;
				case 1:
					return System.Math.Round(Val_Renamed * dSpeedConvert.MultiPlier / dSpeedConvert.Rounding - 0.4999) * dSpeedConvert.Rounding;
				case 2:
					return System.Math.Round(Val_Renamed * dSpeedConvert.MultiPlier / dSpeedConvert.Rounding) * dSpeedConvert.Rounding;
				case 3:
					return System.Math.Round(Val_Renamed * dSpeedConvert.MultiPlier / dSpeedConvert.Rounding + 0.4999) * dSpeedConvert.Rounding;
			}
			return Val_Renamed;
		}

		public static double DeConvertDistance(double Val_Renamed)
		{
			return  Val_Renamed / InitHolding.DistanceConverter.MultiPlier;
		}

		public static double DeConvertHeight(double Val_Renamed)
		{
			return  Val_Renamed / InitHolding.HeightConverter.MultiPlier;
		}

		public static double DeConvertSpeed(double Val_Renamed)
		{
			return Val_Renamed / InitHolding.SpeedConverter.MultiPlier;
		}

		public static double DeConvertDSpeed(double Val_Renamed)
		{
			return Val_Renamed / InitHolding.DSpeedConverter.MultiPlier;
		}

		public static double AdaptToInterval(double realValue, double minVal, double maxVal, double increment)
		{
			if (realValue <= minVal)
				return minVal;
			if (realValue >= maxVal)
				return maxVal;
			if (increment == 0)
			{
				return realValue;
			}
			else
			{
				double quotient;
				quotient = Math.IEEERemainder(realValue, increment);
				if (quotient < increment / 2)
				{
					return realValue - quotient;
				}
				else
				{
					return realValue + (increment - quotient);
				}
			}

		}

        //public static void Dd2Dms(double longtitude,double latitude, string decSep,string lonSide,string latSide,
        //    int sign,out string longtitudeStr,out string latitudeStr)
        //{

        //    double x = System.Math.Abs(System.Math.Round(System.Math.Abs(longtitude) * sign, 10));

        //    double xDeg = Math.Truncate(x);
        //    double dx = (x - xDeg) * 60;
        //    dx = System.Math.Round(dx, 8);
        //    double xMin = Math.Truncate(dx);
        //    double xSec = (dx - xMin) * 60;
        //    xSec = System.Math.Round(xSec, 6);

        //    int Res = 10;
        //    string xDegStr;
        //    if (xDeg < 10) 
        //        xDegStr = "00" + xDeg;
        //    else if (xDeg < 100)
        //        xDegStr = "0" + xDeg;
        //    else
        //        xDegStr = xDeg.ToString(CultureInfo.InvariantCulture);

        //    xDegStr = xDegStr + "°";

        //    string xMinStr = xMin.ToString(CultureInfo.InvariantCulture);
        //    if (xMin < 10)
        //        xMinStr = "0" + xMin;

        //    xMinStr = xMinStr + "'";

        //    xSec = Math.Round(xSec*Res)/Res;
        //    string xSecStr = Math.Truncate(xSec).ToString(CultureInfo.InvariantCulture);

        //    xSecStr = xSecStr + decSep;

        //    xSec = Math.Round((xSec - Math.Truncate(xSec))*Res);

        //    xSecStr = xSecStr + xSec;

        //    xSecStr = xSecStr + "''" + lonSide;
        //    //xSecStr = xSecStr + lonSide;
        //    longtitudeStr = xDegStr + xMinStr + xSecStr;


        //    double y = System.Math.Abs(System.Math.Round(System.Math.Abs(latitude) * sign, 10));

        //    double yDeg = Math.Truncate(y);
        //    double dy = (y - yDeg) * 60;
        //    dy = System.Math.Round(dy, 8);
        //    double yMin = Math.Truncate(dy);
        //    double ySec = (dy - yMin) * 60;
        //    ySec = System.Math.Round(ySec, 6);

        //    string yDegStr = yDeg.ToString(CultureInfo.InvariantCulture);
        //    if (yDeg < 10)
        //        yDegStr = "0" + yDeg.ToString(CultureInfo.InvariantCulture);

        //    yDegStr = yDegStr + "°";

        //    string yMinStr = yMin.ToString(CultureInfo.InvariantCulture);
        //    if (yMin < 10)
        //            yMinStr = "0" + yMin.ToString(CultureInfo.InvariantCulture);
        //    yMinStr = yMinStr + "'";

        //    ySec = Math.Round(ySec*Res)/Res;

        //    string ySecStr = Math.Truncate(ySec).ToString(CultureInfo.InvariantCulture);
        //    ySecStr = ySecStr + decSep;
        //    ySec = Math.Round((ySec - Math.Truncate(ySec))*Res);

        //    ySecStr = ySecStr + ySec.ToString(CultureInfo.InvariantCulture);
        //    ySecStr = ySecStr + "''" + latSide;
        //    //ySecStr = ySecStr + latSide;
        //    latitudeStr = yDegStr + yMinStr + ySecStr;
        //}

	}

	//public static class FeatureExtension
	//{
	//    private static Hashtable _tagList;

	//    private static Hashtable TagList
	//    {
	//        get
	//        {
	//            if (_tagList == null)
	//            {
	//                _tagList = new Hashtable();
	//            }
	//            return _tagList;
	//        }
	//    }

	//    private static object getPropertyValue(this Delib.Classes.Feature feature, string propertyName)
	//    {
	//        if (TagList.ContainsKey(feature))
	//        {
	//            Hashtable propList = (Hashtable)TagList[feature];
	//            if (propList.ContainsKey(propertyName))
	//                return propList[propertyName];
	//        }
	//        return null;
	//    }

	//    private static void setPropertyValue(this Delib.Classes.Feature feature, string propertyName, object value)
	//    {
	//        if (TagList.ContainsKey(feature))
	//        {
	//            Hashtable propList = (Hashtable)TagList[feature];
	//            if (propList.ContainsKey(propertyName))
	//                propList[propertyName] = value;
	//            else
	//                propList.Add(propertyName, value);
	//        }
	//        else
	//        {
	//            Hashtable propList = new Hashtable();
	//            propList.Add(propertyName, GeomFunctions.GmlToAranPoint((Delib.Classes.Feature)value));
	//            TagList.Add(feature, propList);
	//        }
	//    }

	//    public static Point GetProjectPoint(this Delib.Classes.Feature feature)
	//    {
	//        return getPropertyValue(feature, "ProjectPoint") as Point;
	//    }

	//    public static void SetProjectPoint(this Delib.Classes.Feature feature, Aran.Geometries.Point value)
	//    {
	//        setPropertyValue(feature, "ProjectPoint", value);
	//    }

	//    //public static Delib.Classes.GeomObjects.Point GetGeoPoint(this Delib.Classes.Feature feature)
	//    //{
	//    //    return getPropertyValue(feature, "GeoPoint") as Delib.Classes.GeomObjects.Point;
	//    //}

	//    //public static void SetGeoPoint(this Delib.Classes.Feature feature, Delib.Classes.GeomObjects.Point value)
	//    //{
	//    //    setPropertyValue(feature, "GeoPoint", value);
	//    //}
	//}

	
}
