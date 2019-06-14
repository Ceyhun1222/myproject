using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Controls;

namespace VisibilityTool
{
    class GlobalParams
    {
        public static  IHookHelper HookHelper { get; set; }

        public static SpatialReferenceOperation SpatialOperation { get; set; }

        public static Graphics Graphics { get; set; }
    }
}
