using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;
using Aran.Geometries;
using Aran.Delta.Model;

namespace Aran.Delta
{
    public enum RoundType
    {
        RealValue = 0,
        ToDown = 1,
        ToNearest = 2,
        ToUp = 3,
    }

    public static class Common
    {
        public static double ConvertDistance(double valRenamed, RoundType roundMode)
        {
            TypeConvert distanceConvert = InitDelta.DistanceConverter;
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
            TypeConvert heightConvert = InitDelta.HeightConverter;
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
            TypeConvert speedConvert = InitDelta.SpeedConverter;
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
            return valRenamed / InitDelta.DistanceConverter.MultiPlier;
        }

        public static double DeConvertHeight(double valRenamed)
        {
            return valRenamed / InitDelta.HeightConverter.MultiPlier;
        }

        public static double DeConvertSpeed(double valRenamed)
        {
            return valRenamed / InitDelta.SpeedConverter.MultiPlier;
        }

        public static double ConvertAngle(double valRenamed, RoundType roundMode)
        {
            TypeConvert angleConverter = InitDelta.AngleConverter;
            if (((int)roundMode < 0) | ((int)roundMode > 3))
                roundMode = 0;
            switch ((int)roundMode)
            {
                case 0:
                    return valRenamed * angleConverter.MultiPlier;
                case 1:
                    return System.Math.Round(valRenamed * angleConverter.MultiPlier / angleConverter.Rounding - 0.4999) * angleConverter.Rounding;
                case 2:
                    {
                        return System.Math.Round(valRenamed * angleConverter.MultiPlier / angleConverter.Rounding) * angleConverter.Rounding;
                    }
                case 3:
                    return System.Math.Round(valRenamed * angleConverter.MultiPlier / angleConverter.Rounding + 0.4999) * angleConverter.Rounding;
            }
            return valRenamed;
        }

        public static double DeConvertAngle(double valRenamed)
        {
            return valRenamed / InitDelta.AngleConverter.MultiPlier;
        }

        public static double Convert(double valRenamed, double rounding, RoundType roundMode)
        {
            switch ((int)roundMode)
            {
                case 0:
                    return valRenamed * 1;
                case 1:
                    return System.Math.Round(valRenamed * 1 / rounding - 0.4999) * rounding;
                case 2:
                    {
                        return System.Math.Round(valRenamed * 1 / rounding) * rounding;
                    }
                case 3:
                    return System.Math.Round(valRenamed * 1 / rounding + 0.4999) * rounding;
            }
            return valRenamed;
        }

        public static bool TryCalculateResolutionBasedOnAccuracy(double accuracy, out double resolution)
        {
            resolution = 0;

            if (accuracy > 30 && accuracy <= 2000)
                resolution = 60;
            else if (accuracy > 3 && accuracy <= 30)
                resolution = 1;
            else if (accuracy > 0.3 && accuracy <= 3)
                resolution = 0.1;
            else if (accuracy <= 0.3)
                resolution = 0.01;

            if (resolution > 0)
                return true;

            return false;
        }

        public static double CalculateResolutionBasedOnPoint(double lat, double lon)
        {
            Functions.DD2DMS(lat, out double latDeg, out double latMin, out double latSec, 1);
            Functions.DD2DMS(lon, out double lonDeg, out double lonMin, out double lonSec, 1);

            if (latSec == 0 || lonSec == 0)
                return 60;
            else
            {
                var latResolution = latSec.ToString().SkipWhile(x => x != '.').Skip(1).Count();
                var lonResolution = lonSec.ToString().SkipWhile(x => x != '.').Skip(1).Count();

                if (latResolution == 0 || lonResolution == 0)
                    return 1;
                else if (latResolution == 1 || lonResolution == 1)
                    return 0.1;
                else if (latResolution == 2 || lonResolution == 2)
                    return 0.01;
            }

            throw new Exception("Wrong accuracy entered");
        }

        public static int CalculateResolutionDecimalCount(double resolution)
        {

            //todo write common formula
            if (resolution == 60)
                return -1;
            else if (resolution == 1)
                return 0;
            else if (resolution == 0.1)
                return 1;
            else if (resolution == 0.01)
                return 2;

            throw new Exception("Wrong resolution entered");
        }

        public static void CalculateSuggestedScaleBasedOnResolution(double resolution, out double? fromScale, out double? toScale)
        {
            fromScale = toScale = null;
            if (resolution == 60)
            {
                fromScale = 30 / 0.0005;
                toScale = 2000 / 0.0005;
            }
            else if (resolution == 1)
            {
                fromScale = 3 / 0.0005;
                toScale = 30 / 0.0005;
            }
            else if (resolution == 0.1)
            {
                fromScale = 3 / 0.0005;
                toScale = 0.3 / 0.0005;
            }
            else if (resolution == 0.01)
            {
                fromScale = 0.3 / 0.0005;
            }
        }
    }
}
