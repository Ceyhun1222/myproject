using ChartTypeA.Models;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Framework;
using ChartTypeA.CmdsMenu;

namespace ChartTypeA
{
    class GlobalParams
    {
        private object _hookHelper;

        public static IHookHelper HookHelper { get; set; }

        public static IActiveView ActiveView { get { return HookHelper.ActiveView; } }
        public static IMap Map
        {
            get { return HookHelper.FocusMap; }
        }

        public static SpatialReferenceOperation SpatialRefOperation { get; set; }

        public static int HWND { get; set; }

        //public static Aran.Panda.Common.UnitConverter HeightUnitConverter { get; set; }

        //public static Aran.Panda.Common.UnitConverter DistanceUnitConverter { get; set; }
        public static Graphics UI { get; internal set; }
        public static ArenaDbModule DbModule { get; set; }

       // public static Aran.Panda.Constants.Constants Constant { get; set; }

        public static List<PDM.VerticalStructure> ObstacleList { get; set; }

        public static double RotateVal { get; set; }

        public static AbstractGridCreater GrCreater { get; set; }

        public static IApplication Application { get; set; }

        public static ChartParams TypeAChartParams { get; set; }


        public static ViewModels.SelectRunwayViewModel RModel { get; set; }
        public static Extension TypeAExtension { get; internal set; }
        public static int Version { get; internal set; }
        public static VerticalObstacleCreater VerticalObstacleCreater { get; internal set; }
        public static ScaleElement ScaleElement { get; internal set; }
        public static string ProjectName { get; internal set; }
    }
}
