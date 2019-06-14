using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using System;
using System.Collections.Generic;
using ESRI.ArcGIS.Framework;
using PDM;
using ESRI.ArcGIS.Geometry;

namespace ChartPApproachTerrain  
{
    class GlobalParams
    {
        public static IHookHelper HookHelper { get; set; }

        public static IActiveView ActiveView => HookHelper.ActiveView;
        public static IMap Map { get; set; }
       

        public static SpatialReferenceOperation SpatialRefOperation { get; set; }

        public static int HWND { get; set; }

        public static Graphics UI { get; internal set; }
        
        public static List<VerticalStructure> ObstacleList { get; set; }

        public static double RotateVal { get; set; }

        public static IApplication Application { get; set; }

        public static Runway SelectedRunway { get; set; }

        public static RunwayDirection SelectedRwyDirection { get; set; }

        public static AirportHeliport SelectedAirport { get; set; }

        public static IGeometry Area4Rectangle { get; set; }

        public static DateTime EffectiveDate { get; set; }

        public static bool IsOpened { get; set; }

        public static double Step { get; set; }



    }
}
