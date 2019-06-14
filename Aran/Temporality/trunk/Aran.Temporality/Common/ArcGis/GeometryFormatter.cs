using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aran.Aim;
using Aran.Aim.Objects;
using Aran.Converters;
using Aran.Geometries;
using Aran.Temporality.Common.Aim.Extension.Property;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Logging;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using Polygon = ESRI.ArcGIS.Geometry.Polygon;

namespace Aran.Temporality.Common.ArcGis
{
    public class GeometryFormatter
    {
        #region Intersection



        private static ISpatialReference CreateSpatialreference()
        {
            ISpatialReferenceFactory spatRefFactory = new SpatialReferenceEnvironment();
            IGeographicCoordinateSystem geogCoordSystem = spatRefFactory.
                CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);
            spatRefFactory = null;
            return geogCoordSystem;
        }

        private static ISpatialReference _wgs1984Reference;
        public static ISpatialReference Wgs1984Reference()
        {
            return _wgs1984Reference ?? (_wgs1984Reference = CreateSpatialreference());
        }


        private static bool Intersect(IGeometry esriGeom1, IGeometry esriGeom2)
        {
            var relationalOperator = esriGeom1 as IRelationalOperator;
            if (relationalOperator==null) throw new Exception("can not cast to operator");
            return !relationalOperator.Disjoint(esriGeom2);


            //var topologicalOperator = esriGeom1 as ITopologicalOperator;
            //if (topologicalOperator == null)
            //{
            //    topologicalOperator = esriGeom2 as ITopologicalOperator;
                
            //    if (topologicalOperator == null) throw new Exception("both geometry can not be topo operator");
               
            //    return Intersect(esriGeom2, esriGeom1);
            //}

            //IGeometry intersectGeom = null;
            //var geomType1 = esriGeom1.GeometryType;
            //var geomType2 = esriGeom2.GeometryType;
            //switch (geomType1)
            //{
            //    case esriGeometryType.esriGeometryPolyline:
            //        switch (geomType2)
            //        {
            //            case esriGeometryType.esriGeometryPolygon:
            //                intersectGeom = topologicalOperator.Intersect(esriGeom2, esriGeometryDimension.esriGeometry1Dimension);
            //                break;
            //            default:
            //                intersectGeom = topologicalOperator.Intersect(esriGeom2, esriGeometryDimension.esriGeometry0Dimension);
            //                break;
            //        }
            //        break;
            //    case esriGeometryType.esriGeometryPolygon:
            //        switch (geomType2)
            //        {
            //            case esriGeometryType.esriGeometryPolyline:
            //                intersectGeom = topologicalOperator.Intersect(esriGeom2, esriGeometryDimension.esriGeometry1Dimension);
            //                break;
            //            case esriGeometryType.esriGeometryPolygon:
            //                intersectGeom = topologicalOperator.Intersect(esriGeom2, esriGeometryDimension.esriGeometry2Dimension);
            //                break;
            //            default:
            //                intersectGeom = topologicalOperator.Intersect(esriGeom2, esriGeometryDimension.esriGeometry0Dimension);
            //                break;
            //        }
            //        break;
            //    default:
            //        intersectGeom = topologicalOperator.Intersect(esriGeom2, esriGeometryDimension.esriGeometry0Dimension);
            //        break;
            //}
            //return intersectGeom != null && !intersectGeom.IsEmpty;
        }

        public static bool HasIntersection(AimFeature feature1, AimFeature feature2)
        {
            PrepareGeometry(feature1);
            PrepareGeometry(feature2);

            var list1 = feature1.PropertyExtensions.Where(extension => extension is EsriPropertyExtension).
                Cast<EsriPropertyExtension>().
                Select(t => t.EsriObject).ToList();

            var list2 = feature2.PropertyExtensions.Where(extension => extension is EsriPropertyExtension).
               Cast<EsriPropertyExtension>().
               Select(t => t.EsriObject).ToList();

            return list1.Any(esriGeom1 => list2.Any(esriGeom2 => Intersect(esriGeom1, esriGeom2)));
        }

        public static bool HasIntersection(IGeometry esriGeom1, AimFeature feature2)
        {
            PrepareGeometry(feature2);

            var list2 = feature2.PropertyExtensions.Where(extension => extension is EsriPropertyExtension).
               Cast<EsriPropertyExtension>().
               Select(t => t.EsriObject).ToList();

            return list2.Any(esriGeom2 => Intersect(esriGeom1, esriGeom2));
        }

        #endregion

        #region Load esri if necessary

        public static void PrepareGeometry(AimFeature parent, bool force=false)
        {
            foreach (var extension in parent.PropertyExtensions.Where(extension => extension is EsriPropertyExtension).
                Cast<EsriPropertyExtension>().
                Where(extension => (extension.EsriObject == null || force) && 
                    extension.EsriData!=null))
            {
                extension.EsriObject = (IGeometry) EsriFromBytes(extension.EsriData);
            }
        }

        #endregion

        #region Init esri geometry

        public static void InitEsriExtension(AimFeature parent)
        {
            InitEsriExtension(parent, parent.Feature);
        }

        public static IGeometry MakeZAwareGeometry(IGeometry geometry)
        {
            IGeometryBridge2 geometryBridge = new GeometryEnvironmentClass();

            switch (geometry.GeometryType)
            {
                case esriGeometryType.esriGeometryPoint:
                    var point = geometry as IPoint;
                    if (point != null && point.IsEmpty)
                    {
                        geometry = new PointClass { X = 0, Y = 0, ZAware = true, MAware = true, Z = 0 };
                    }
                    else
                    {
                        geometry = new PointClass { X = point.X, Y = point.Y, ZAware = true, MAware = true, Z = 0 };
                    }
                    break;
                
                case esriGeometryType.esriGeometryPolygon:
                    {
                        var polygon = geometry as Polygon;
                        if (polygon == null)
                        {
                            return geometry;
                        }

                        (polygon as IZAware).ZAware = true;
                        (polygon as IMAware).MAware = true;
                        (polygon as IZ).SetConstantZ(0D);

                        //var newPolygon = new PolygonClass();
                        //var wksPoints = new List<WKSPoint>();

                        //for (var i = 0; i < polygon.PointCount; i++)
                        //{
                        //    var oldPoint = polygon.Point[i];
                        //    wksPoints.Add(new WKSPoint {X = oldPoint.X, Y = oldPoint.Y});
                        //}

                        //var pointArray = wksPoints.ToArray();

                        //geometryBridge.AddWKSPoints(newPolygon, ref pointArray);
                        ////newPolygon.Simplify();

                        //newPolygon.ZAware = true;
                        //newPolygon.MAware = true;

                        //newPolygon.SetConstantZ(0);

                        //geometry = newPolygon;
                    }
                    break;

                case esriGeometryType.esriGeometryPolyline:
                {
                    var polyline = geometry as Polyline;
                    if (polyline == null)
                    {
                        return geometry;
                    }

                    (polyline as IZAware).ZAware = true;
                    (polyline as IMAware).MAware = true;
                    (polyline as IZ).SetConstantZ(0D);

                    break;


                }


                    break;

            }

            return geometry;
        }

        private static void InitEsriExtension(AimFeature parent, object currentObject, 
            List<int> propertyPath = null, HashSet<object> processed = null)
        {
            if (processed == null)
            {
                processed = new HashSet<object>();
            }
            if (processed.Contains(currentObject)) return;
            processed.Add(currentObject);

            if (propertyPath==null) propertyPath=new List<int>();
            
            if (currentObject is Geometry)
            {
                //we should not be at root
                if (propertyPath.Count==0) throw new Exception("root geometry");

                //add geometry to extension
                var aranGeometry = currentObject as Geometry;
                bool doSimplify = true;
                if (aranGeometry is LineString || aranGeometry is MultiLineString)
                    doSimplify = false;
                var esriGeometry = ConvertToEsriGeom.FromGeometry(aranGeometry,false, Wgs1984Reference(),doSimplify);               
                
                //if (aranGeometry.Type == GeometryType.MultiPolygon)//it is zaware by default
                //{
                //}
                //else
                //{

                //   // esriGeometry = MakeZAwareGeometry(esriGeometry);
              
                //}

                esriGeometry = MakeZAwareGeometry(esriGeometry);
 
                parent.PropertyExtensions.Add(new EsriPropertyExtension
                                                  {
                                                      EsriObject = esriGeometry,
                                                      EsriData = EsriToBytes(esriGeometry),
                                                      PropertyIndex = propertyPath[0],
                                                      PropertyPath = propertyPath.ToArray()
                                                  });
                return;
            }

            if (currentObject is IAimObject)
            {
                var aimObject = currentObject as IAimObject;
                var classInfo = AimMetadata.GetClassInfoByIndex(aimObject);

                foreach (var propInfo in classInfo.Properties)
                {
                    object aimPropVal = aimObject.GetValue(propInfo.Index);
                    if (aimPropVal == null) continue;

                    if (aimPropVal is IEditAimField)
                    {
                        aimPropVal = (aimPropVal as IEditAimField).FieldValue;
                    }

                    if (aimPropVal is IEditChoiceClass)
                    {
                        aimPropVal = (aimPropVal as IEditChoiceClass).RefValue;
                    }

                   

                    if (aimPropVal is IList)
                    {
                        var list = aimPropVal as IList;
                        for (var index = 0; index < list.Count; index++)
                        {
                            var subValue = list[index];
                            var newPropertyPath = new List<int>(propertyPath) {propInfo.Index, index};
                            InitEsriExtension(parent, subValue, newPropertyPath, processed);
                        }
                    }
                    else
                    {
                        var newPropertyPath = new List<int>(propertyPath) {propInfo.Index};
                        InitEsriExtension(parent, aimPropVal, newPropertyPath, processed);
                    }
                }
            }
        }

        #endregion

        #region esri serialization

        private const string PropertyName = "esri";


        public static byte[] EsriToBytes(object esriGeometry)
        {
            var memBlb = new MemoryBlobStream();
            IObjectStream objStr = new ObjectStream();
            objStr.Stream = memBlb;

            IPropertySet propertySet = new PropertySetClass();
            
            var perStr = (IPersistStream)propertySet;
            propertySet.SetProperty(PropertyName, esriGeometry);
            perStr.Save(objStr, 0);

            object obj;
            ((IMemoryBlobStreamVariant)memBlb).ExportToVariant(out obj);

            return (byte[])obj;
        }

        public static object EsriFromBytes(byte[] bytes)
        {
            try
            {
                var memBlobStream = new MemoryBlobStream();

                var varBlobStream = (IMemoryBlobStreamVariant)memBlobStream;

                varBlobStream.ImportFromVariant(bytes);

                var anObjectStream = new ObjectStreamClass {Stream = memBlobStream};

                IPropertySet aPropSet = new PropertySetClass();

                var aPersistStream = (IPersistStream)aPropSet;
                aPersistStream.Load(anObjectStream);

                return aPropSet.GetProperty(PropertyName);
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(typeof(GeometryFormatter)).Error(ex, ex.Message);
                return null;
            }
        }

        #endregion

        
    }
}
