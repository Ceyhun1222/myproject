using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.FeatureInfo;
using Aran.Aim.Features;
using Aran.AranEnvironment;
using Aran.AranEnvironment.Symbols;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Geometries.SpatialReferences;
using Aran.PANDA.Common;
using Aran.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Aran.PANDA.Vss
{
    public static class Globals
    {
        private static Settings _pandaSettings;
        private static UnitConverter _unitConverter;
        private static SymbolSettings _symbols;
        private static GeometryOperators _geomOpers;
        private static VssQPI _qpi;

        static Globals()
        {
            Const_CourseDrawDistance = 12000;
            Const_StandardSplayAngleTan = 0.15;
            Const_StandardSplayAngle = Math.Atan(0.15);
            Const_SlopeOffset = ARANMath.DegToRad(1.12);
        }

        public static void ClearSingletoneValues()
        {
            _pandaSettings = null;
            _unitConverter = null;
            _symbols = null;
            _geomOpers = null;
        }

        public static IAranEnvironment AranEnv { get; set; }

        public static DbProvider DbPro
        {
            get { return AranEnv.DbProvider as DbProvider; }
        }

        public static MainForm MainForm { get; set; }

        public static Settings PandaSettings
        {
            get
            {
                if (_pandaSettings == null) {
                    _pandaSettings = new Settings();
                    _pandaSettings.Load(AranEnv);

                    if (_pandaSettings.Aeroport == Guid.Empty)
                        ShowError(
                            "Airport not defined in PANDA Settings!\n" +
                            "Please, define first in Options menu.");
                }

                return _pandaSettings;
            }
        }

        public static UnitConverter UnitConverter
        {
            get
            {
                if (_unitConverter == null)
                    _unitConverter = new UnitConverter(PandaSettings);

                return _unitConverter;
            }
        }

        public static SymbolSettings Symbols
        {
            get
            {
                if (_symbols == null) {
                    _symbols = new SymbolSettings();
                }

                return _symbols;
            }
        }

        public static GeometryOperators GeomOpers
        {
            get
            {
                if (_geomOpers == null)
                    _geomOpers = new GeometryOperators();
                
                return _geomOpers;
            }
        }

        public static LineSymbol LineSymbol(DrawElementType elementType)
        {
            return Symbols[elementType] as LineSymbol;
        }

        public static PointSymbol PointSymbol(DrawElementType elementType)
        {
            return Symbols[elementType] as PointSymbol;
        }

        public static FillSymbol PolygonSymbol(DrawElementType elementType)
        {
            return Symbols[elementType] as FillSymbol;
        }

        public static void ShowError(string message, bool isWarning = false)
        {
            System.Windows.Forms.MessageBox.Show(message, "Error", System.Windows.Forms.MessageBoxButtons.OK,
                    (isWarning ? System.Windows.Forms.MessageBoxIcon.Warning : System.Windows.Forms.MessageBoxIcon.Error));

            if (!isWarning)
                MainForm.Close();
        }

        public static void ShowFeatureInfo(Feature feature)
        {
            if (feature == null)
                return;

            var _featureInfo = new ROFeatureViewer();
            _featureInfo.GettedFeature = Qpi.GetFeature;
            _featureInfo.SetOwner(Globals.MainForm);

            var featList = new List<Feature>();
            featList.Add(feature);

            _featureInfo.ShowFeaturesForm(featList, false);
        }

        public static VssQPI Qpi
        {
            get
            {
                if (_qpi == null) {
                    _qpi = new VssQPI();
                    _qpi.Open(DbPro);

                    var terrainDataReaderHandler = AranEnv.CommonData.GetObject("terrainDataReader") as TerrainDataReaderEventHandler;
                    if (terrainDataReaderHandler != null)
                        _qpi.TerrainDataReader += terrainDataReaderHandler;
                }

                return _qpi;
            }
        }

        public static string GetTextOrEmpty(object value)
        {
            if (value == null)
                return string.Empty;
            return value.ToString();
        }

        public static string GetDoubleText(double? value, int precision = 2)
        {
            if (value == null)
                return string.Empty;

            var precFormat = new string('#', precision);
            return value.Value.ToString("0." + precFormat);
        }

        public static string GetValDistanceText(ValDistance valDistance, eRoundMode roundMode = eRoundMode.NEAREST)
        {
            if (valDistance == null)
                return string.Empty;

            var precision = PandaSettings.DistancePrecision;
            var hdt = PandaSettings.DistanceUnit;

            double val = valDistance.Value;

            if ((hdt == HorizantalDistanceType.KM && valDistance.Uom != UomDistance.KM) ||
                (hdt == HorizantalDistanceType.NM && valDistance.Uom != UomDistance.NM)) {
                    val = Aran.Converters.ConverterToSI.Convert(valDistance, 0d);
            }

            val = UnitConverter.DistanceToDisplayUnits(val, roundMode);

            return string.Format("{0} {1}", val, UnitConverter.DistanceUnit);
        }

        public static string DistanceFormat(double value)
        {
            return UnitConverter.DistanceToDisplayUnits(value) + "  " + UnitConverter.DistanceUnit;
        }

        public static Geometry ToProject(Geometry geom)
        {
            return GeomOpers.GeoTransformations(geom, AranEnv.Graphics.WGS84SR, AranEnv.Graphics.ViewProjection);
        }

        public static double Const_CourseDrawDistance { get; private set; }

        public static double Const_StandardSplayAngle { get; private set; }

        public static double Const_StandardSplayAngleTan { get; private set; }

        public static double Const_SlopeOffset { get; private set; }

        public static SpatialReference Wgs84SpRef
        {
            get { return AranEnv.Graphics.WGS84SR; }
        }

        public static SpatialReference ViewSpRef
        {
            get { return AranEnv.Graphics.ViewProjection; }
        }

		public static SpatialReferenceOperation SpatRefOperation
		{
			get;
			internal set;
		}

		public static void ClearDrawedGraphics(object obj)
        {
            var methods = obj.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods) {
                var attrs = method.GetCustomAttributes(typeof(DrawElementMethodAttribute), true);
                if (attrs != null && attrs.Length > 0) {
                    method.Invoke(obj, new object [] { DrawOperType.Clear });
                }
            }
        }
    }

    public class ComboBoxItem
    {
        public ComboBoxItem(string text, object tag)
        {
            Text = text ?? string.Empty;
            Tag = tag;
        }

        public string Text { get; private set; }

        public object Tag { get; private set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
