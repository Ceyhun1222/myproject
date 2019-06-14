using Aran.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Temporality.Common.Mongo
{
    public class PointEqualityComparer : EqualityComparer<Point>
    {
        public override bool Equals(Point x, Point y)
        {
            return x.Equals2D(y, 0.000001);
        }

        public override int GetHashCode(Point obj)
        {
            int hash = 17;

            hash = hash * 23 + obj.X.GetHashCode();
            hash = hash * 23 + obj.Y.GetHashCode();

            return hash;
        }
    }
}
