using System.Collections.Generic;
using System.Windows.Input;
using Aran.AranEnvironment;
using Aran.Geometries.Operators;
using Aran.Omega.View;
using Aran.PANDA.Constants;
using Aran.PANDA.Common;
using Aran.Aim.Features;
using System.Windows.Threading;

namespace Aran.Omega
{
    public class GlobalParams
    {
        public static IAranGraphics UI { get; set; }
        public static Aran.Geometries.Operators.IGeometryOperators GeomOperators { get; set; }
       // public static GeometryOperators GeomOperators { get; set; }
        public static DbModule Database { get; set; }
        public static Constants Constant { get; set; }
        public static IAranEnvironment AranEnvironment { get; set; }
        public static Aran.Omega.SettingsUI.OmegaSettings Settings { get; set; }
        public static SpatialReferenceOperation SpatialRefOperation { get; set; }
        public static OmegaMainForm OLSWindow { get; set; }
        //public static EtodForm ETODWindow { get; set; }
        public static List<VerticalStructure> ObstacleList { get; set; }
        public static List<VerticalStructure> AdhpObstacleList { get; set; }

        public static AranTool AranMapToolMenuItem { get; set; }
        public static RunwayCentrelinePoint RefPoint { get; set; }
        public static int HWND { get; set; }
        public static ILogger Logger { get; set; }

        public static ViewModels.OLSViewModel OlsViewModel { get; set; }

        public static EtodForm ETODWindow { get; set; }

        public static Geometries.MultiPolygon FirGeomPrj { get; set; }
        public static Dispatcher Dispatcher { get; set; }
    }
}
