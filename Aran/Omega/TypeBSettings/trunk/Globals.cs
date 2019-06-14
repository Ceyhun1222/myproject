using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.esriSystem;

namespace Aran.Omega.TypeB.Settings
{
    public class Globals
    {
        public static TypeBSettings Settings { get; set; }

        public static IVariantStream Stream { get; set; }
    }
}
