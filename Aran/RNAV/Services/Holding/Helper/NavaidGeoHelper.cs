using Aran.Aim.Features;
using Holding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Panda.Rnav.Holding.Helper
{
    public static class NavaidGeoHelper
    {
        public static Aran.Geometries.Point ComponentGeometry(this Navaid navaid)
        {
            if (navaid == null)
                throw new ArgumentNullException("Navaid cannot be null");

            if (navaid.Location != null)
                return navaid.Location.Geo;

            foreach (var navComponent in navaid.NavaidEquipment)
            {
                var navaidEquipment = GlobalParams.Database.HoldingQpi.GetAbstractFeature(navComponent.TheNavaidEquipment)
                     as NavaidEquipment;
                return navaidEquipment.Location?.Geo;
            }
            return null;
        }
    }
}
