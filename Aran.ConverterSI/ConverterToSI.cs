using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using System;
using Aran.Aim;

namespace Aran.Converters
{
	public static class ConverterToSI
	{
		public static double Convert(object valueType, double defaultValue)
		{
			if(valueType == null)
				return defaultValue;

            var dataTypeVal = valueType as ADataType;
            if (dataTypeVal == null)
                return defaultValue;

            switch (dataTypeVal.DataType)
            {
                case DataType.ValWeight:
                    return ToWeightKg((ValWeight)dataTypeVal);
                case DataType.ValTemperature:
                    return ToTemperatureC((ValTemperature)dataTypeVal);
                case DataType.ValSpeed:
                    return ToSpeedM_SEC((ValSpeed)dataTypeVal);
                case DataType.ValPressure:
                    return ToPressurePA((ValPressure)dataTypeVal);
                case DataType.ValLightIntensity:
                    return ToLightIntensityCD((ValLightIntensity)dataTypeVal);
                case DataType.ValFrequency:
                    return ToFrequencyHZ((ValFrequency)dataTypeVal);
                case DataType.ValFL:
                    return ToTransitionLevelSM((ValFL)dataTypeVal);
                case DataType.ValDuration:
                    return ToDurationSEC((ValDuration)dataTypeVal);
                case DataType.ValDistanceVertical:
                    return ToDistanceVerticalM((ValDistanceVertical)dataTypeVal);
                case DataType.ValDistanceSigned:
                    return ToDistanceSignedM((ValDistanceSigned)dataTypeVal);
                case DataType.ValDistance:
                    return ToDistanceM((ValDistance)dataTypeVal);
                default:
                    return defaultValue;
            }
		}

		private static double ToWeightKg(ValWeight valWeight)
		{
			double source = valWeight.Value;
			switch (valWeight.Uom)
			{
				case UomWeight.KG:
					return source;

				case UomWeight.T:
					return (source * 1000);

				case UomWeight.LB:
					return (source * 0.4535924);

				case UomWeight.TON:
					return (source * 907.1847);

				default:
					throw new Exception("ValWeight Uom type is not implemented !");
			}
		}

		private static double ToTemperatureC(ValTemperature valtemperature)
		{
			double source = valtemperature.Value;
			switch (valtemperature.Uom)
			{
				case UomTemperature.C:
					return source;

				case UomTemperature.F:
					return (double)(((source - 32.0) * 5.0) / 9.0);

				case UomTemperature.K:
					return (source - 273.15);

				default:
					throw new Exception("ValTemperature Uom type is not implemented !");
			}
		}

		private static double ToSpeedM_SEC(ValSpeed valSpeed)
		{
			double source = valSpeed.Value;
			switch (valSpeed.Uom)
			{
				case UomSpeed.KM_H:
					return (source * 0.2777778);

				case UomSpeed.KT:
					return (source * 0.5144444);

				case UomSpeed.MACH:
					return (source * 331.46);

				case UomSpeed.M_MIN:
					return (source * 0.01666667);

				case UomSpeed.FT_MIN:
					return (source * 0.00508);

				case UomSpeed.M_SEC:
					return source;

				case UomSpeed.FT_SEC:
					return (source * 0.3048);

				case UomSpeed.MPH:
					return (source * 0.44704);

				default:
					throw new Exception("ValSpeed Uom type is not implemented !");
			}
		}

		private static double ToPressurePA(ValPressure valPressure)
		{
			double source = valPressure.Value;
			switch (valPressure.Uom)
			{
				case UomPressure.PA:
					return source;

				case UomPressure.MPA:
					return (source * 1000000);

				case UomPressure.PSI:
					return (source * 6894.757);

				case UomPressure.BAR:
					return (source * 100000);

				case UomPressure.TORR:
					return (source * 133.3224);

				case UomPressure.ATM:
					return (source * 101325);

				case UomPressure.HPA:
					return (source * 100);

				default:
					throw new Exception("ValPressure Uom type is not implemented !");
			}
		}

		private static double ToLightIntensityCD(ValLightIntensity valLightIntensity)
		{
			double source = valLightIntensity.Value;
			switch (valLightIntensity.Uom)
			{
				case UomLightIntensity.CD:
					return source;
				default:
					throw new Exception("ValLightIntensity Uom type is not implemented !");
			}
		}

		private static double ToFrequencyHZ(ValFrequency valFrequency)
		{
			double source = valFrequency.Value;
			switch (valFrequency.Uom)
			{
				case UomFrequency.HZ:
					return source;

				case UomFrequency.KHZ:
					return (source * 1000);

				case UomFrequency.MHZ:
					return (source * 1000000);

				case UomFrequency.GHZ:
					return (source * 1000000000);
				default:
					throw new Exception("ValFrequency UOM type is not implemented !");
			}
		}

		private static double ToTransitionLevelSM(ValFL valTransition)
		{
			uint source = valTransition.Value;
			switch (valTransition.Uom)
			{
				case UomFL.FL:
					return (source * 3.048);

				case UomFL.SM:
					return source;

				default:
					throw new Exception("ValFL Uom type is not implemented !");
			}
		}

		private static double ToDurationSEC(ValDuration valDuration)
		{
			double source = valDuration.Value;
			switch (valDuration.Uom)
			{
				case UomDuration.HR:
					return (source * 3600);

				case UomDuration.MIN:
					return (source * 60);

				case UomDuration.SEC:
					return source;

				default:
					throw new Exception("ValDuration Uom type is not implemented !");
			}
		}

		private static double ToDistanceVerticalM(ValDistanceVertical distanceVerticalValue)
		{
			double source = distanceVerticalValue.Value;
			
            switch (distanceVerticalValue.Uom)
			{
				case UomDistanceVertical.FT:
					return (source * 0.3048);

				case UomDistanceVertical.M:
					return source;

				case UomDistanceVertical.FL:
					return (source * 30.48);

				case UomDistanceVertical.SM:
					return (source * 10);

				default:
					throw new Exception("ValDistanceVertical Uom type is not implemented !");
			}
		}

		private static double ToDistanceSignedM(ValDistanceSigned valDistanceSigned)
		{
			return ToDistanceValClass(valDistanceSigned);
		}

		private static double ToDistanceM(ValDistance distanceValue)
		{
			return ToDistanceValClass(distanceValue);
		}

		private static double ToDistanceValClass(IEditValClass valClass)
		{
			double source = valClass.Value;
			switch (valClass.Uom)
			{
				case (int)UomDistance.NM:
					return (source * 1852.0);

				case (int)UomDistance.KM:
					return (source * 1000.0);

				case (int)UomDistance.M:
					return source;

				case (int)UomDistance.FT:
					return (source * 0.3048);

				case (int)UomDistance.MI:
					return (source * 1609.344);

				case (int)UomDistance.CM:
					return (source * 0.01);

				default:
					throw new Exception("Uom type is not implemented !");
			}
		}

		private static double ToDepthCM(ValDepth valDepth)
		{
			double source = valDepth.Value;
			switch (valDepth.Uom)
			{
				case UomDepth.MM:
					return (source * 0.1);

				case UomDepth.CM:
					return source;

				case UomDepth.IN:
					return (source * 2.54);

				case UomDepth.FT:
					return (source * 30.48);

				default:
					throw new Exception("ValDepth Uom type is not implemented !");
			}
		}
	}
}
