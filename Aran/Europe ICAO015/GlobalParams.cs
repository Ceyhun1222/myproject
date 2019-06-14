using System.Collections.Generic;
using System.Windows.Input;
using Aran.AranEnvironment;
using Aran.Geometries.Operators;
using Aran.Omega.View;
using Aran.PANDA.Constants;
using Aran.PANDA.Common;
using Aran.Aim.Features;

namespace Europe_ICAO015
{
    public class GlobalParams
    {
        public static IAranGraphics UI { get; set; }
        public static Aran.Geometries.Operators.JtsGeometryOperators GeomOperators { get; set; }
        // public static GeometryOperators GeomOperators { get; set; }
        public static DBModule Database { get; set; }
        public static Constants Constant { get; set; }
        public static IAranEnvironment AranEnvironment { get; internal set; }
        public static Aran.Omega.SettingsUI.OmegaSettings Settings { get; set; }
        public static SpatialReferenceOperation SpatialRefOperation { get; set; }
        //public static OmegaMainForm OLSWindow { get; set; }
        //public static EtodForm ETODWindow { get; set; }
        public static List<VerticalStructure> ObstacleList { get; set; }
      
        //public static Aran.Omega.ViewModels.OLSViewModel OlsViewModel { get; set; }
    }
}
