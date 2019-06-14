using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Aran.Geometries;
using Aran.Omega.Enums;

namespace Europe_ICAO015
{
    public static class Common
    {
        public static double ConvertDistance(double valRenamed, RoundType roundMode)
        {
            TypeConvert distanceConvert = Starter.DistanceConverter;
            if (((int)roundMode < 0) | ((int)roundMode > 3))
                roundMode = 0;
            switch ((int)roundMode)
            {
                case 0:
                    return valRenamed * distanceConvert.MultiPlier;
                case 1:
                    return System.Math.Round(valRenamed * distanceConvert.MultiPlier / distanceConvert.Rounding - 0.4999) * distanceConvert.Rounding;
                case 2:
                    {
                        return System.Math.Round(valRenamed * distanceConvert.MultiPlier / distanceConvert.Rounding) * distanceConvert.Rounding;
                    }
                case 3:
                    return System.Math.Round(valRenamed * distanceConvert.MultiPlier / distanceConvert.Rounding + 0.4999) * distanceConvert.Rounding;
            }
            return valRenamed;
        }

        public static double ConvertHeight(double valRenamed, RoundType roundMode)
        {
            TypeConvert heightConvert = Starter.HeightConverter;
            if (((int)roundMode < 0) | ((int)roundMode > 3))
                roundMode = 0;
            switch ((int)roundMode)
            {
                case 0:
                    return valRenamed * heightConvert.MultiPlier;
                case 1:
                    return System.Math.Round(valRenamed * heightConvert.MultiPlier / heightConvert.Rounding - 0.4999) * heightConvert.Rounding;
                case 2:
                    return System.Math.Round(valRenamed * heightConvert.MultiPlier / heightConvert.Rounding) * heightConvert.Rounding;
                case 3:
                    return System.Math.Round(valRenamed * heightConvert.MultiPlier / heightConvert.Rounding + 0.4999) * heightConvert.Rounding;
            }
            return valRenamed;
        }

        public static double ConvertAccuracy(double valRenamed, RoundType roundMode, TypeConvert typeConvert)
        {
            if (((int)roundMode < 0) | ((int)roundMode > 3))
                roundMode = 0;
            switch ((int)roundMode)
            {
                case 0:
                    return valRenamed * typeConvert.MultiPlier;
                case 1:
                    return System.Math.Round(valRenamed / typeConvert.Rounding - 0.4999) * typeConvert.Rounding;
                case 2:
                    return System.Math.Round(valRenamed / typeConvert.Rounding) * typeConvert.Rounding;
                case 3:
                    return System.Math.Round(valRenamed / typeConvert.Rounding + 0.4999) * typeConvert.Rounding;
            }
            return valRenamed;
        }

        public static double ConvertSpeed_(double valRenamed, RoundType roundMode)
        {
            TypeConvert speedConvert = Starter.SpeedConverter;
            if ((roundMode < 0) | ((int)roundMode > 3))
                roundMode = 0;
            switch ((int)roundMode)
            {
                case 0:
                    return valRenamed * speedConvert.MultiPlier;
                case 1:
                    return System.Math.Round(valRenamed * speedConvert.MultiPlier / speedConvert.Rounding - 0.4999) * speedConvert.Rounding;
                case 2:
                    return System.Math.Round(valRenamed * speedConvert.MultiPlier / speedConvert.Rounding) * speedConvert.Rounding;
                case 3:
                    return System.Math.Round(valRenamed * speedConvert.MultiPlier / speedConvert.Rounding + 0.4999) * speedConvert.Rounding;
            }
            return valRenamed;
        }

        public static double DeConvertDistance(double valRenamed)
        {
            return valRenamed / Starter.DistanceConverter.MultiPlier;
        }

        public static double DeConvertHeight(double valRenamed)
        {
            return valRenamed / Starter.HeightConverter.MultiPlier;
        }

        public static double DeConvertSpeed(double valRenamed)
        {
            return valRenamed / Starter.SpeedConverter.MultiPlier;
        }
    }
}
