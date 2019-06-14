using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;

namespace EnrouteIntersect
{
    class GlobalParams
    {
        public static IHookHelper HookHelper { get; set; }

        public static SpatialReferenceOperation SpatialOperation { get; set; }

        public static Graphics Graphics { get; set; }

	    public static ILayer RouteSegmentLayer { get; set; }

	    public static double Radius;
    }
}
