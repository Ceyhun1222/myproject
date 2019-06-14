using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Data;
using Aran.AranEnvironment;
using Aran.Delta.Model;
using Aran.Delta.ViewModels;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Framework;

namespace Aran.Delta
{
    public class GlobalParams
    {
        private static IHookHelper _hookHelper;

        public static IAranEnvironment AranEnv { get; set; }
        public static IApplication Application { get; set; }
        public static IDBModule Database { get; set; }
        public static Aran.Delta.Model.SpatialReferenceOperation SpatialRefOperation { get; set; }
        public static Aran.Delta.Settings.DeltaSettings Settings { get; set; }
        public static AranTool Tool { get; set; }
        public static Aran.PANDA.Common.UnitConverter UnitConverter { get; set; }
        
        public static SnapClass FetureSnap { get; set; }
        public static RouteBufferViewModel RouteBuffer { get; set; }
        public static Aran.Geometries.Operators.JtsGeometryOperators GeometryOperators { get; set; }
        public static List<string> LogList { get; set; }

        public static IHookHelper HookHelper
        {
            get
            {
                if (_hookHelper == null)
                {
                    _hookHelper = new HookHelper();
                    _hookHelper.Hook = AranEnv.HookObject ;

                }
                return _hookHelper;
            }
            set { _hookHelper = value; }
        }

        public static IActiveView ActiveView { get { return HookHelper.ActiveView; } }
        public static IMap Map {
            get { return _hookHelper.FocusMap; }
        }

        public static int HWND { get; set; }

        public static Model.Graphics UI{ get; set; }

        public static ICommandItem ByClickToolCommand { get; set; }

        public static Delta.Settings.DeltaExtension DeltaExt { get; set; }

        public static DesigningLayerDbModule DesigningAreaReader { get; set; }

        public static double Accuracy {get; set;}
    }
}
