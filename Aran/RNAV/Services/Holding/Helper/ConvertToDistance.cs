using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Holding
{
    public static class ConvertToDistance
    {

        public static double ConvertDistance(ValDistance fromDistance)
        { 
            double distance = (double)fromDistance.Value;
            if (fromDistance.Uom == UomDistance.CM)
                distance /= 10;
            else if (fromDistance.Uom== UomDistance.FT)
                distance *= 0.3048;
            else if (fromDistance.Uom == UomDistance.KM)
                distance *= 1000;
            else if (fromDistance.Uom == UomDistance.MI)
                distance*=1609.344;
            else if (fromDistance.Uom == UomDistance.NM)
                distance*=1852;
            return Common.ConvertDistance(distance, roundType.toNearest);
        }
    }
}
