using System.Collections.Generic;
using Aran.Aim.Features;

namespace Holding
{
    public class DmeCoverageParam:IEqualityComparer<DmeCoverageParam>
    {
        public DmeCoverageType DmeCovType { get; set; }
        public List<Navaid> NavList { get; set; }
        public Aran.Geometries.Point RefPoint { get; set; }
        public double Doc { get; set; }
        public List<DmeCoverage> DmeCoverageList { get; set; }

        public Aran.Geometries.MultiPolygon TwoDmeGeom { get; set; }
        public Aran.Geometries.MultiPolygon ThreeDmeGeom { get; set; }     

        public bool Equals(DmeCoverageParam x, DmeCoverageParam y)
        {
            int count = 0;
            if (x.NavList.Count != y.NavList.Count)
                return false;
            
            foreach (Navaid nav1 in x.NavList)
            {
                foreach (Navaid nav2 in y.NavList)
                {
                    if (nav1 == nav2)
                        count++;
                }
            }
            if (x.NavList.Count != count)
                return false ;

            if (x.Doc != y.Doc)
                return false;
            
            return true;
        }

        public int GetHashCode(DmeCoverageParam obj)
        {
            return obj.GetHashCode();
        }

    }
}