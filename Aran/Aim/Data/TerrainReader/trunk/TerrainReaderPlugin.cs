using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.AranEnvironment;
using Aran.Queries;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF = Aran.Aim.Features;
using AG = Aran.Geometries;
using EG = ESRI.ArcGIS.Geometry;

namespace Aran.Aim.Data.TerrainReader
{
    public class TerrainReaderPlugin : AranPlugin
    {
        private IAranEnvironment _aranEnv;
        private Guid _id;


        public TerrainReaderPlugin()
        {
            _id = new Guid("f3d11f81-9c38-4ce0-ab80-10477af5f0d4");
            IsSystemPlugin = true;
        }


        public override Guid Id { get { return _id; } }

        public override void Startup(IAranEnvironment aranEnv)
        {
            _aranEnv = aranEnv;
            _aranEnv.CommonData.AddObject("terrainDataReader", new TerrainDataReaderEventHandler(OnTerrainDataReader));
        }

        public override string Name
        {
            get { return "TerrainDataReader"; }
        }

        private void OnTerrainDataReader(object sender, TerrainDataReaderEventArgs e)
        {
            IFeatureClass featClass = null;
            var layerName = "";
            var slp = new SelectedLayerPackable();
            if (_aranEnv.GetExtData("selectedTerrainData", slp))
                layerName = slp.LayerName;

            if (string.IsNullOrWhiteSpace(layerName))
                return;

            var esriMapControl = _aranEnv.MapControl as AxMapControl;

            for (int i = 0; i < esriMapControl.LayerCount; i++) {
                var featLayer = esriMapControl.get_Layer(i) as IFeatureLayer;
                if (featLayer != null) {
                    if (featLayer.Name == layerName) {
                        featClass = featLayer.FeatureClass;
                        break;
                    }
                }
            }

            if (featClass == null)
                return;

            ISpatialFilter sf = null;

            if (e.FilterPolygon != null) {
                sf = new SpatialFilter();
                sf.Geometry = Aran.Converters.ConvertToEsriGeom.FromMultiPolygon(e.FilterPolygon);
                sf.GeometryField = featClass.ShapeFieldName;
                //sf.SpatialRel = esriSpatialRelEnum.esriSpatialRelWithin;
                sf.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
            }
            
            IFeatureCursor cursor = featClass.Search(sf, false);
            IFeature esriFeature;

            var enumItems = Enum.GetValues(typeof(VerticalStructureField));
            int[] fieldIndexes = new int[enumItems.Length];

            foreach (object enumItem in enumItems) {
                fieldIndexes[(int)enumItem] = featClass.FindField(enumItem.ToString());
            }

            var valueGetter = new EsriFieldValueGetter<VerticalStructureField>();
            valueGetter.FieldIndexes = fieldIndexes;

            while ((esriFeature = cursor.NextFeature()) != null) {
                valueGetter.CurrentRowItem = esriFeature;

                try {
                    var vs = ToVerticalStructure(valueGetter);
                    if (vs != null)
                        e.Result.Add(vs);
                }
                catch (Exception ex) {
                    e.Errors.Add("Error, ID: " + valueGetter.GetId() + "\r\n" + ex.Message);
                }
            }
        }

        private static AF.VerticalStructure ToVerticalStructure(EsriFieldValueGetter<VerticalStructureField> valueGetter)
        {
            var aranPoint = valueGetter.GetGeometry() as Aran.Geometries.Point;
            if (aranPoint == null)
                return null;

            string s;
            double d;

            var vs = new AF.VerticalStructure();
            FillTimeSlice(vs);

            vs.Name = valueGetter[VerticalStructureField.txtName];
            s = valueGetter[VerticalStructureField.codeGroup];
            if (s != null)
                vs.Group = (s == "Y");

            s = valueGetter[VerticalStructureField.codeLgt];
            if (s != null)
                vs.Lighted = (s == "Y");

            var part = new AF.VerticalStructurePart();
            part.Designator = valueGetter[VerticalStructureField.txtDescrType];

            if (part.Designator != null && part.Designator.Length >= 16)
                part.Designator = part.Designator.Substring(0, 15);

            part.HorizontalProjection = new AF.VerticalStructurePartGeometry();

            var ep = new AF.ElevatedPoint();
            ep.Geo.Assign(aranPoint);
            part.HorizontalProjection.Location = ep;

            s = valueGetter[VerticalStructureField.uomDistVer];
            d = valueGetter[VerticalStructureField.valElev];
            UomDistanceVertical udv;
            if (s != null && Enum.TryParse<UomDistanceVertical>(s, true, out udv)) {
                ep.Elevation = new ValDistanceVertical(d, udv);
            }

            vs.Part.Add(part);

            return vs;
        }

        private static void FillTimeSlice(AF.Feature feature)
        {
            feature.Identifier = Guid.NewGuid();
            feature.TimeSlice = new TimeSlice();
            feature.TimeSlice.Interpretation = TimeSliceInterpretationType.BASELINE;
            feature.TimeSlice.ValidTime = new TimePeriod(DateTime.Now);
            feature.TimeSlice.FeatureLifetime = feature.TimeSlice.ValidTime;
        }
    }

    internal class EsriFieldValueGetter<TEnum> 
    {
        public int[] FieldIndexes { get; set; }

        public object CurrentRowItem
        {
            get { return _esriFeature; }
            set { _esriFeature = value as IFeature; }
        }

        public dynamic this[TEnum fieldEnum]
        {
            get
            {
                object val = _esriFeature.get_Value(FieldIndexes[(int)(object)fieldEnum]);

                if (System.DBNull.Value.Equals(val))
                    return null;

                return val;
            }
        }

        public object[] GetValues(params TEnum[] fieldEnumArr)
        {
            var values = new object[fieldEnumArr.Length];
            for (int i = 0; i < fieldEnumArr.Length; i++) {
                values[i] = this[fieldEnumArr[i]];
            }
            return values;
        }

        public Aran.Geometries.Geometry GetGeometry()
        {
            var esriGeom = _esriFeature.Shape as ESRI.ArcGIS.Geometry.IGeometry;
            if (esriGeom == null || esriGeom.IsEmpty)
                return null;

            var aranGeom = Aran.Converters.ConvertFromEsriGeom.ToGeometry(esriGeom, true);
            return aranGeom;
        }

        public long GetId()
        {
            if (_esriFeature.HasOID)
                return _esriFeature.OID;
            return -1;
        }

        public string GetMid()
        {
            var index = _esriFeature.Fields.FindField("R_mid");
            return _esriFeature.get_Value(index).ToString();
        }

        private IFeature _esriFeature;
    }

    public enum VerticalStructureField
    {
        txtName,
        txtDescrType,
        codeGroup,
        codeLgt,
        valElev,
        valElevAccuracy,
        uomDistVer
    }
}
