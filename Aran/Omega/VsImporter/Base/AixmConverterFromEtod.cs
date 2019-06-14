using ESRI.ArcGIS.esriSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Geometries;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS;

namespace Aran.Omega.VSImporter
{
    public class AixmConverterFromEtod
    {
        private const string VsPointName = "PointObstacle";
        private const string VsPolylineName = "PolylineObstacle";
        private const string VsPolygonName = "PolygonObstacle";
        private const string VsName = "Obstacle";
        private readonly string _fileName;
        private Dictionary<string, VerticalStructure> _verticalStructures;
        private IFeatureWorkspace _featureWorkspace;
        private IFeatureClass _ptFeatClass;
        private IFeatureClass _polygonFeatClass;
        private IFeatureClass _polylineFeatClass;

        public AixmConverterFromEtod(string fileName)
        {
            _fileName = fileName;
            RuntimeManager.Bind(ProductCode.Desktop);
            AoInitialize ao = new AoInitialize();
            ao.Initialize(esriLicenseProductCode.esriLicenseProductCodeBasic);
        }

        public void InitializeWorkspace()
        {
            var workspaceFactory = (IWorkspaceFactory2)new AccessWorkspaceFactoryClass();
            _featureWorkspace = workspaceFactory.OpenFromFile(_fileName, 0) as IFeatureWorkspace;
        }

        public void CreateFeatureClasses()
        {
            if (_featureWorkspace == null) throw new ArgumentException("Workspace is null");

            _ptFeatClass = _featureWorkspace.OpenFeatureClass(VsPointName);
            _polygonFeatClass = _featureWorkspace.OpenFeatureClass(VsPolygonName);
            _polylineFeatClass = _featureWorkspace.OpenFeatureClass(VsPolylineName);
        }

        public List<VerticalStructure> GetVerticalStructures()
        {
            InitializeWorkspace();
            CreateFeatureClasses();

            _verticalStructures = new Dictionary<string, VerticalStructure>();

            LoadVsByFeatName(VsName);
            return _verticalStructures.Values.ToList();
        }

        private void LoadVsByFeatName(string featName)
        {
            ITable featClass = _featureWorkspace.OpenTable(featName);

            var cursor = featClass.Search (null, false);
            IRow esriFeature = cursor.NextRow();
            while (esriFeature != null)
            {
                var id = esriFeature.GetValue<string>(FieldNames.ObsId);
                if (id != null)
                {
                    VerticalStructure vs;
                    if (!_verticalStructures.TryGetValue(id, out vs))
                        vs = new VerticalStructure();

                    vs.Identifier =new Guid(id);

                    AssignParts(vs);

                    vs.Lighted = esriFeature.CheckAvaliablity(FieldNames.Lighting);

                    vs.MarkingICAOStandard = esriFeature.CheckAvaliablity(FieldNames.Marking);

                    vs.Group = esriFeature.IsGroup(FieldNames.Group);

                    vs.LightingICAOStandard = esriFeature.CheckAvaliablity(FieldNames.LightingICAOStandard);

                    var width = esriFeature.GetValue<double>(FieldNames.Width);
                    if (Math.Abs(width - default(double)) > 0.0001)
                    {
                        vs.Width = new ValDistance(width, UomDistance.M);
                    }

                    var length = esriFeature.GetValue<double>(FieldNames.Length);
                    if (Math.Abs(length - default(double)) > 0.0001)
                    {
                        vs.Width = new ValDistance(length, UomDistance.M);
                    }

                    var radius = esriFeature.GetValue<double>(FieldNames.Radius);
                    if (Math.Abs(radius - default(double)) > 0.0001)
                    {
                        vs.Width = new ValDistance(radius, UomDistance.M);
                    }


                    if (Enum.TryParse(esriFeature.GetValue<string>(FieldNames.ObsType).ToUpper(), out CodeVerticalStructure obsType))
                        vs.Type = obsType;

                    vs.Name = esriFeature.GetValue<string>(FieldNames.SourceId);

                    vs.TimeSlice = new TimeSlice();

                    var beginValidTime = esriFeature.GetValue<DateTime>(FieldNames.StValid);
                    vs.TimeSlice.ValidTime = new TimePeriod(beginValidTime);
                    var endValidTime = esriFeature.GetValue<DateTime>(FieldNames.EndValid);
                    if (endValidTime != default(DateTime))
                        vs.TimeSlice.ValidTime.EndPosition = endValidTime;

                    var featLifeTimeBegin = esriFeature.GetValue<DateTime>(FieldNames.StFeat);
                    var featLifeTimeEnd = esriFeature.GetValue<DateTime>(FieldNames.EndFeat);
                    
                    vs.TimeSlice.FeatureLifetime = new TimePeriod(featLifeTimeBegin);
                    if (featLifeTimeEnd != default(DateTime))
                        vs.TimeSlice.FeatureLifetime.EndPosition = featLifeTimeEnd;

                    vs.TimeSlice.Interpretation = TimeSliceInterpretationType.PERMDELTA;

                    if (!_verticalStructures.ContainsKey(vs.Identifier.ToString()))
                        _verticalStructures.Add(vs.Identifier.ToString(),vs);
                }

                esriFeature = cursor.NextRow();
                
            }
        }

        private void AssignParts(VerticalStructure vs)
        {
            IFeatureCursor featCursor = GetFeatCursor(vs, _ptFeatClass);
            AssignPart(vs, featCursor);

            featCursor = GetFeatCursor(vs, _polygonFeatClass);
            AssignPart(vs, featCursor);

            featCursor = GetFeatCursor(vs, _polylineFeatClass);
            AssignPart(vs, featCursor);
        }

        private void AssignPart(VerticalStructure vs, IFeatureCursor featCursor)
        {
            IFeature esriVsPart = featCursor.NextFeature();
            while (esriVsPart != null)
            {
                var vsPart = CreatePart(esriVsPart);
                vs.Part.Add(vsPart);

                esriVsPart = featCursor.NextFeature();
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(featCursor);
        }

        private IFeatureCursor GetFeatCursor(VerticalStructure vs,IFeatureClass featureClass)
        {
            IQueryFilter queryFiler = new QueryFilterClass();
            queryFiler.WhereClause = $"VerticalStructure_Id ='{vs.Identifier}'";
            var featCursor = featureClass.Search(queryFiler, false);
            return featCursor;
        }

        private VerticalStructurePart CreatePart(IFeature esriFeature)
        {
            var geo = esriFeature.Shape;
            if (geo == null)
                return null;

            var vsPart = new VerticalStructurePart();
            vsPart.HorizontalProjection = new VerticalStructurePartGeometry();
            dynamic partExtent = GetPartWithGeometry(geo, vsPart);

            var elevation = esriFeature.GetValue<double>(FieldNames.Elevation);
            if (Math.Abs(elevation - default(double)) > 0.0001)
            {
                partExtent.Elevation =
                    new ValDistanceVertical(elevation, UomDistanceVertical.M);
            }

            var height = esriFeature.GetValue<double>(FieldNames.Height);
            if (Math.Abs(height - default(double)) > 0.0001)
            {
                vsPart.VerticalExtent = new ValDistance(height, UomDistance.M);
            }

            var horAccuracy = esriFeature.GetValue<double>(FieldNames.HAcc);
            if (Math.Abs(horAccuracy - default(double)) > 0.0001)
            {
                partExtent.HorizontalAccuracy =
                    new Aran.Aim.DataTypes.ValDistance(horAccuracy, UomDistance.M);
            }

            var verAccuracy = esriFeature.GetValue<double>(FieldNames.VAcc);
            if (Math.Abs(verAccuracy - default(double)) > 0.0001)
            {
                partExtent.VerticalAccuracy =
                    new Aran.Aim.DataTypes.ValDistance(verAccuracy, UomDistance.M);
            }

            partExtent.VerticalDatum = esriFeature.VerticalDatum();

            vsPart.ConstructionStatus = esriFeature.GetConstructionStatus();
            return vsPart;
        }

        private static dynamic GetPartWithGeometry(IGeometry geo, VerticalStructurePart vsPart)
        {
            dynamic partExtent;
            if (geo.GeometryType == esriGeometryType.esriGeometryPolygon)
            {
                partExtent = new ElevatedSurface();
                vsPart.HorizontalProjection.SurfaceExtent = partExtent;
                Geometry convertedGeo = ConvertFromEsriGeom.ToPolygonGeo(geo as IPolygon);

                if (convertedGeo.Type == GeometryType.Polygon)
                    vsPart.HorizontalProjection.SurfaceExtent.Geo.Add(convertedGeo as Aran.Geometries.Polygon);
                else
                {
                    foreach (Aran.Geometries.Polygon poly in (MultiPolygon) convertedGeo)
                    {
                        vsPart.HorizontalProjection.SurfaceExtent.Geo.Add(poly);
                    }
                }

            }
            else if (geo.GeometryType == esriGeometryType.esriGeometryPolyline)
            {
                partExtent = new ElevatedCurve();
                vsPart.HorizontalProjection.LinearExtent = partExtent;

                Aran.Geometries.MultiLineString mltLineString =
                    (Aran.Geometries.MultiLineString)ConvertFromEsriGeom.ToPolyline(geo as IPolyline, true);

                foreach (var lineString in mltLineString)
                {
                    vsPart.HorizontalProjection.LinearExtent.Geo.Add(lineString);
                }
            }
            else if (geo.GeometryType == esriGeometryType.esriGeometryPoint)
            {
                partExtent = new ElevatedPoint();
                vsPart.HorizontalProjection.Location = partExtent;
                if (geo is IPoint pt)
                {
                    vsPart.HorizontalProjection.Location.Geo.X = pt.X;
                    vsPart.HorizontalProjection.Location.Geo.Y = pt.Y;
                }
            }
            else
            {
                throw new ArgumentException("Geometry format is not correct");
            }

            return partExtent;
        }
    }
}
