using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim.Enums;

namespace Aran.Delta
{
    class DeltaHeightConverters
    {
        public static double ConvertTo(double value, UomDistanceVertical fromUnit, UomDistanceVertical toUnit)
        {
            double mVal = FromDistanceVerticalM(fromUnit, value);
            double result = ToDistanceVertical(toUnit, mVal);
            return result;
        }

        public static double FromDistanceVerticalM(UomDistanceVertical uom, double source)
        {
            switch (uom)
            {
                case UomDistanceVertical.FT:
                    return (source*0.3048);

                case UomDistanceVertical.M:
                    return source;

                case UomDistanceVertical.FL:
                    return (source * 30.48);

                case UomDistanceVertical.SM:
                    return (source * 0.1);

                default:
                    throw new Exception("UomDistanceVertical is not implemented !");
            }
        }

        public static double ToDistanceVertical(UomDistanceVertical uom, double sourceInM)
        {
            switch (uom)
            {
                case UomDistanceVertical.FT:
                    return (sourceInM / 0.3048);

                case UomDistanceVertical.M:
                    return sourceInM;

                case UomDistanceVertical.FL:
                    return (sourceInM / 30.48);

                case UomDistanceVertical.SM:
                    return (sourceInM * 10);

                default:
                    throw new Exception("UomDistanceVertical is not implemented !");
            }
        }
    }
}
