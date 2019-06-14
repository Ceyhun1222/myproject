using System.Collections.Generic;
using Aran.AranEnvironment;
using Aran.Interfaces;
using Aran.Omega.TypeB.View;
using Aran.Panda.Constants;
using Aran.Panda.Common;
using Aran.Aim.Features;

namespace Aran.Omega.TypeB
{
    public  class GlobalParams
    {
        static GlobalParams ()
	    {
            Logs = "";
	    }
        
        public static IAranGraphics UI { get; set; }
        public static Topology.GeometryOperators GeomOperators { get; set; }
        //public static Aran.Geometries.Operators.GeometryOperators GeomOperators { get; set; }
       // public static Aran.Panda.SettingsUI.SettingsUI AranSettings { get; set; }
        public static IPandaAranExtension AranExtension { get; set; }
        public static DbModule Database { get; set; }
        public static Constants Constant { get; set; }
        public static IAranEnvironment AranEnvironment { get; set; }
        public static Aran.Omega.SettingsUI.OmegaSettings Settings { get; set; }
        public static SpatialReferenceOperation SpatialRefOperation { get; set; }
        public static TypeBView TypeBView { get; set; }
        //public static EtodForm ETODWindow { get; set; }
        public static List<VerticalStructure> ObstacleList { get; set; }
        public static List<VerticalStructure> AdhpObstacleList { get; set; }

        public static AranTool AranMapToolMenuItem { get; set; }

        public static RunwayCentrelinePoint RefPoint { get; set; }
        public static int HWND { get; set; }
        public static string Logs;

        public static ViewModels.TypeBViewModel TypeBViewModel { get; set; }
    }
}
