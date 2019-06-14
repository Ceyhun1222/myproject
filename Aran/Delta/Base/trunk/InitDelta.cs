using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Aran.AranEnvironment;
using Aran.Delta.Classes;
using Aran.Delta.Model;
using Aran.Delta.Properties;
using Aran.PANDA.Common;
using Aran.Aim.Features;
using System.Runtime.InteropServices;
using Aran.Aim.Data;
using Aran.Geometries.SpatialReferences;
using Aran.Queries.DeltaQPI;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using NetTopologySuite.Utilities;

namespace Aran.Delta
{
    public static class InitDelta
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetThreadLocale(int dwLangID);

        public static int ObstacleDistanceFromArp = 20000;

        public static bool InitCommand()
        {
            try
            {
                NativeMethods.InitAll();

                GlobalParams.LogList = new List<string>();

                if (GlobalParams.AranEnv == null)
                {
                    if (!Functions.InitalizeExtension())
                    {
                        Messages.Error("Cannot Initialize extension");
                        return false;
                    }

                    if (GlobalParams.Map.SpatialReference == null)
                    {
                        Messages.Error("Spatial reference can not be null!");
                        return false;
                    }

                    GlobalParams.SpatialRefOperation =
                        new Aran.Delta.Model.SpatialReferenceOperation(GlobalParams.Map.SpatialReference);

                    GlobalParams.Settings = GlobalParams.DeltaExt.Settings;
                    
                    GlobalParams.Database = new ArenaDBModule();
                    GlobalParams.DesigningAreaReader = new DesigningLayerDbModule();

                    EsriFunctions.AddDeltaLayersToMap(GlobalParams.Map);
                }
                else
                {
                    GlobalParams.SpatialRefOperation =
                        new Aran.Delta.Model.SpatialReferenceOperation(GlobalParams.AranEnv.Graphics.ViewProjection);

                    if (GlobalParams.Settings == null)
                        GlobalParams.Settings = new Aran.Delta.Settings.DeltaSettings();

                    GlobalParams.Settings.Load(GlobalParams.AranEnv);
                    var deltaQpi = DeltaQpiFactory.Create();
                    var dbProvider = GlobalParams.AranEnv.DbProvider as DbProvider;
                    deltaQpi.Open(dbProvider);

                    GlobalParams.Database = new DbModule(deltaQpi,GlobalParams.Settings);
                }

                var settings = GlobalParams.Settings;
                if (settings == null)
                    throw new Exception("Settings is empty!Please first save the settings");

                if (GlobalParams.UI == null)
                    GlobalParams.UI = new Model.Graphics();


                GlobalParams.GeometryOperators = new Geometries.Operators.JtsGeometryOperators();

                Langcode = 1093;
                SetThreadLocale((int) Langcode);

                _distanceUnit = settings.DeltaInterface.DistanceUnit;
                _distancePrecision = settings.DeltaInterface.DistancePrecision;

                _heightUnit = settings.DeltaInterface.HeightUnit;
                _heightPrecision = settings.DeltaInterface.HeightPrecision;

                _anglePrecision = settings.DeltaInterface.AnglePrecision;

                double multiplier;
                string unit;
                if (_distanceUnit == HorizantalDistanceType.KM)
                {
                    multiplier = 0.001;
                    unit = Delta.Properties.Resources.str519;
                }
                else
                {
                    multiplier = 1.0/1852.0;
                    unit = Delta.Properties.Resources.str520;
                }
                DistanceConverter = new TypeConvert
                {
                    MultiPlier = multiplier,
                    Rounding = _distancePrecision,
                    Unit = unit
                };

                if (_heightUnit == VerticalDistanceType.M)
                {
                    multiplier = 1.0;
                    unit = Delta.Properties.Resources.str111;
                }
                else if (_heightUnit == VerticalDistanceType.Ft)
                {
                    multiplier = 1.0/0.3048;
                    unit = Delta.Properties.Resources.str112;
                }

                AngleConverter = new TypeConvert {MultiPlier = 1, Rounding = _anglePrecision, Unit = "°"};

                HeightConverter = new TypeConvert {MultiPlier = multiplier, Rounding = _heightPrecision, Unit = unit};

                // if (GlobalParams.AranMapToolMenuItem == null)
                //{
                if (GlobalParams.AranEnv != null)
                {
                    GlobalParams.Tool = new AranEnvironment.AranTool();
                    GlobalParams.Tool.Cursor = System.Windows.Forms.Cursors.Cross;
                    GlobalParams.Tool.Visible = true;
                    GlobalParams.AranEnv.AranUI.AddMapTool(GlobalParams.Tool);
                }
                else
                {
                    var aranTool = new AranTool();
                    aranTool.Cursor = System.Windows.Forms.Cursors.Cross;
                    aranTool.Visible = true;

                    ESRI.ArcGIS.Framework.ICommandBars commandBars = GlobalParams.Application.Document.CommandBars;
                    ESRI.ArcGIS.esriSystem.UID commandID = new ESRI.ArcGIS.esriSystem.UIDClass();
                    commandID.Value = "Aran.Delta.Classes.ByClickTool"; // example: "Aran.PANDA.RadarMA.Tool1";
					var drawLineCommandItem = commandBars.Find(commandID, false, false);

                    if (drawLineCommandItem != null)
                    {
                        var byClickTool = drawLineCommandItem.Command as ByClickTool;
                        if (byClickTool != null)
                            byClickTool.SetAranTool(aranTool);

                        GlobalParams.Tool = aranTool;
                        GlobalParams.ByClickToolCommand = drawLineCommandItem;
                        //(commandItem as DrawLineTool).SetAranTool(aranTool);
                    }
                }

                GlobalParams.FetureSnap = new Model.SnapClass();
                //}
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error init default parametrs!" + e.Message, "Delta", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }

        }

        static void InitializeTool() 
        {
            
           
        
        }

        public static int Langcode { get; private set; }

        public static TypeConvert DistanceConverter { get; private set; }

        public static TypeConvert HeightConverter { get; private set; }

        public static TypeConvert SpeedConverter { get; private set; }

        public static TypeConvert DSpeedConverter { get; private set; }

        public static TypeConvert AngleConverter {get;private set;}

        private static double _distancePrecision;
        private static double _heightPrecision;
        private static double _anglePrecision;
        private static HorizantalDistanceType _distanceUnit;
        private static VerticalDistanceType _heightUnit;
    }

    public struct TypeConvert
    {
        public double MultiPlier { get; set; }
        public double Rounding { get; set; }
        public string Unit { get; set; }
    }
}
