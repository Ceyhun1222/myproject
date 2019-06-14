using System;
using GeoAPI.Geometries;

namespace ObstacleCalculator.Domain.Models
{
    public class PlaneParam
    {
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }
        public double D { get; set; }

        public double GetZ(Coordinate pt)
        {
            if (pt != null)
                return (-(A * pt.X + B * pt.Y + D));
            return 0;
        }

        public double GetZ2(Coordinate pt)
        {
            if (pt != null)
                return (-(A * pt.X + B * pt.Y + D)/C);
            return 0;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is  PlaneParam planeParam))
                return false;

            var isEqual = Math.Abs(A - planeParam.A) < 0.1
                          && Math.Abs(B - planeParam.B) < 0.1
                          && Math.Abs(C - planeParam.C) < 0.1;
            return ReferenceEquals(this, obj) ||isEqual;
        }
    }
}
