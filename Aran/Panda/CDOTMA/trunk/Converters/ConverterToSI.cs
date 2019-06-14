using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PDM;

namespace Converter
{
	public static class ConverterToSI
	{
		//public static double Convert(object valueType, double defaultValue)
		//{
		//    if (valueType == null)
		//        return defaultValue;

		//    try
		//    {
		//        if (valueType is ValWeight)
		//            return ToWeightKg((ValWeight)valueType);

		//        if (valueType is ValTemperature)
		//            return ToTemperatureC((ValTemperature)valueType);

		//        if (valueType is ValSpeed)
		//            return ToSpeedM_SEC((ValSpeed)valueType);

		//        if (valueType is ValPressure)
		//            return ToPressurePA((ValPressure)valueType);

		//        if (valueType is ValLightIntensity)
		//            return ToLightIntensityCD((ValLightIntensity)valueType);

		//        if (valueType is ValFrequency)
		//            return ToFrequencyHZ((ValFrequency)valueType);

		//        if (valueType is ValFL)
		//            return ToTransitionLevelSM((ValFL)valueType);

		//        if (valueType is ValDuration)
		//            return ToDurationSEC((ValDuration)valueType);

		//        if (valueType is ValDistanceVertical)
		//            return ToDistanceVerticalM((ValDistanceVertical)valueType);

		//        if (valueType is ValDistanceSigned)
		//            return ToDistanceSignedM((ValDistanceSigned)valueType);

		//        if (valueType is ValDistance)
		//            return ToDistanceM((ValDistance)valueType);

		//        return defaultValue;
		//    }
		//    catch
		//    {
		//        return defaultValue;
		//    }
		//}

		public static double Convert(double value, object valueType, double defaultValue = 0.0)
		{
			if (valueType == null)
				return defaultValue;

			try
			{
				//if (valueType is ValWeight)
				//    return ToWeightKg((ValWeight)valueType);

				//if (valueType is ValTemperature)
				//    return ToTemperatureC((ValTemperature)valueType);

				if (valueType is SpeedType)
					return ToSpeedM_SEC(value, (SpeedType)valueType);

				//if (valueType is ValPressure)
				//    return ToPressurePA((ValPressure)valueType);

				//if (valueType is ValLightIntensity)
				//    return ToLightIntensityCD((ValLightIntensity)valueType);

				if (valueType is UOM_FREQ)
					return ToFrequencyHZ(value, (UOM_FREQ)valueType);

				//if (valueType is ValFL)
				//    return ToTransitionLevelSM((ValFL)valueType);

				//if (valueType is ValDuration)
				//    return ToDurationSEC((ValDuration)valueType);

				if (valueType is UOM_DIST_VERT)
					return ToDistanceVerticalM(value, (UOM_DIST_VERT)valueType);

				if (valueType is UOM_DIST_HORZ)
					return ToDistanceSignedM(value, (UOM_DIST_HORZ)valueType);

				//if (valueType is ValDistance)
				//    return ToDistanceM((ValDistance)valueType);

				return defaultValue;
			}
			catch
			{
				return defaultValue;
			}
		}

		//private static double ToWeightKg(ValWeight valWeight)
		//{
		//    double source = valWeight.Value;
		//    switch (valWeight.Uom)
		//    {
		//        case UomWeight.KG:
		//            return source;

		//        case UomWeight.T:
		//            return (source * 1000);

		//        case UomWeight.LB:
		//            return (source * 0.4535924);

		//        case UomWeight.TON:
		//            return (source * 907.1847);

		//        default:
		//            throw new Exception("ValWeight Uom type is not implemented !");
		//    }
		//}

		//private static double ToTemperatureC(ValTemperature valtemperature)
		//{
		//    double source = valtemperature.Value;
		//    switch (valtemperature.Uom)
		//    {
		//        case UomTemperature.C:
		//            return source;

		//        case UomTemperature.F:
		//            return (double)(((source - 32.0) * 5.0) / 9.0);

		//        case UomTemperature.K:
		//            return (source - 273.15);

		//        default:
		//            throw new Exception("ValTemperature Uom type is not implemented !");
		//    }
		//}

		private static double ToSpeedM_SEC(double valSpeed, SpeedType Uom)
		{
			double source = valSpeed;
			switch (Uom)
			{
				case SpeedType.KM_H:
					return (source * 0.2777778);

				case SpeedType.KT:
					return (source * 0.5144444);

				case SpeedType.MACH:
					return (source * 331.46);

				case SpeedType.M_MIN:
					return (source * 0.01666667);

				case SpeedType.FT_MIN:
					return (source * 0.00508);

				case SpeedType.M_SEC:
					return source;

				case SpeedType.FT_SEC:
					return (source * 0.3048);

				case SpeedType.MPH:
					return (source * 0.44704);

				default:
					throw new Exception("ValSpeed Uom type is not implemented !");
			}
		}

		//private static double ToPressurePA(ValPressure valPressure)
		//{
		//    double source = valPressure.Value;
		//    switch (valPressure.Uom)
		//    {
		//        case UomPressure.PA:
		//            return source;

		//        case UomPressure.MPA:
		//            return (source * 1000000);

		//        case UomPressure.PSI:
		//            return (source * 6894.757);

		//        case UomPressure.BAR:
		//            return (source * 100000);

		//        case UomPressure.TORR:
		//            return (source * 133.3224);

		//        case UomPressure.ATM:
		//            return (source * 101325);

		//        case UomPressure.HPA:
		//            return (source * 100);

		//        default:
		//            throw new Exception("ValPressure Uom type is not implemented !");
		//    }
		//}

		//private static double ToLightIntensityCD(ValLightIntensity valLightIntensity)
		//{
		//    double source = valLightIntensity.Value;
		//    switch (valLightIntensity.Uom)
		//    {
		//        case UomLightIntensity.CD:
		//            return source;
		//        default:
		//            throw new Exception("ValLightIntensity Uom type is not implemented !");
		//    }
		//}

		private static double ToFrequencyHZ(double FreqVal, UOM_FREQ Uom)
		{
			switch (Uom)
			{
				case UOM_FREQ.HZ:
					return FreqVal;

				case UOM_FREQ.KHZ:
					return (FreqVal * 1000.0);

				case UOM_FREQ.MHZ:
					return (FreqVal * 1000000.0);

				case UOM_FREQ.GHZ:
					return (FreqVal * 1000000000.0);
				default:
					throw new Exception("ValFrequency UOM type is not implemented !");
			}
		}

		//private static double ToTransitionLevelSM(ValFL valTransition)
		//{
		//    uint source = valTransition.Value;
		//    switch (valTransition.Uom)
		//    {
		//        case UomFL.FL:
		//            return (source * 3.048);

		//        case UomFL.SM:
		//            return source;

		//        default:
		//            throw new Exception("ValFL Uom type is not implemented !");
		//    }
		//}

		//private static double ToDurationSEC(ValDuration valDuration)
		//{
		//    double source = valDuration.Value;
		//    switch (valDuration.Uom)
		//    {
		//        case UomDuration.HR:
		//            return (source * 3600);

		//        case UomDuration.MIN:
		//            return (source * 60);

		//        case UomDuration.SEC:
		//            return source;

		//        default:
		//            throw new Exception("ValDuration Uom type is not implemented !");
		//    }
		//}

		private static double ToDistanceVerticalM(double distVertVal, UOM_DIST_VERT Uom)
		{
			switch (Uom)
			{
				case UOM_DIST_VERT.FT:
					return (distVertVal * 0.3048);

				case UOM_DIST_VERT.M:
					return distVertVal;

				case UOM_DIST_VERT.FL:
					return (distVertVal * 30.48);

				case UOM_DIST_VERT.SM:
					return (distVertVal * 10.0);

				case UOM_DIST_VERT.KM:
					return (distVertVal * 1000.0);
				default:
					throw new Exception("ValDistanceVertical Uom type is not implemented !");
			}
		}

		private static double ToDistanceSignedM(double valDistanceSigned, UOM_DIST_HORZ Uom)
		{
			return ToDistanceValClass(valDistanceSigned, Uom);
		}

		private static double ToDistanceM(double distanceValue, UOM_DIST_HORZ Uom)
		{
			return ToDistanceValClass(distanceValue, Uom);
		}

		private static double ToDistanceValClass(double valClass, UOM_DIST_HORZ Uom)
		{
			double source = valClass;
			switch (Uom)
			{
				case UOM_DIST_HORZ.NM:
					return (source * 1852.2);

				case UOM_DIST_HORZ.KM:
					return (source * 1000.0);

				case UOM_DIST_HORZ.M:
					return source;

				case UOM_DIST_HORZ.FT:
					return (source * 0.3048);

				case UOM_DIST_HORZ.MI:
					return (source * 1609.344);

				case UOM_DIST_HORZ.SM:
				    return (source * 0.01);

				default:
					throw new Exception("Uom type is not implemented !");
			}
		}

		//private static double ToDepthCM(double valDepth, UOM_DEPTH Uom)
		//{
		//    double source = valDepth;
		//    switch (Uom)
		//    {
		//        case UOM_DEPTH.MM:
		//            return (source * 0.1);

		//        case UOM_DEPTH.CM:
		//            return source;

		//        case UOM_DEPTH.IN:
		//            return (source * 2.54);

		//        case UOM_DEPTH.FT:
		//            return (source * 30.48);

		//        default:
		//            throw new Exception("ValDepth Uom type is not implemented !");
		//    }
		//}
	}
}
