using System.Collections.Generic;

namespace Holding.Models
{
    public class HoldingNavComparer : IEqualityComparer<HoldingNavaid>
    {
        public bool Equals(HoldingNavaid x, HoldingNavaid y)
        {
            if (x.Designator == y.Designator)
                return true;
            else
                return false;
        }

        public int GetHashCode(HoldingNavaid obj)
        {
            return obj.GetHashCode();
        }
    }
}