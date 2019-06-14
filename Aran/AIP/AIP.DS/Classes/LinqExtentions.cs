using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim;
using Aran.Aim.Features;

namespace AIP.DataSet.Classes
{
    class FeatureComparer : IEqualityComparer<Feature>
    {
        public static readonly FeatureComparer Instance = new FeatureComparer();

        public bool Equals(Feature x, Feature y)
        {
            if (x == null || y == null) return false;
            return x.Identifier.Equals(y.Identifier);
        }

        public int GetHashCode(Feature obj)
        {
            return obj.Identifier.GetHashCode();
        }
    }
}
