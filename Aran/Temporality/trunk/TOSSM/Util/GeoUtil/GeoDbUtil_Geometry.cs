using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace TOSSM.Util
{
    public partial class GeoDbUtil
    {
        #region Geometry

        public static IGeometryDef CreateGeometryDef(esriGeometryType shapeType, ISpatialReference spatialReference=null)
        {
            if (spatialReference == null) spatialReference = Wgs1984;
            IGeometryDef geometryDef = new GeometryDefClass();
            IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;
            geometryDefEdit.GeometryType_2 = shapeType;
            geometryDefEdit.SpatialReference_2 = spatialReference;
            geometryDefEdit.HasZ_2 = true;
            geometryDefEdit.HasM_2 = true;
            return geometryDef;
        }

        public static ISpatialReference Wgs1984 = CreateGeographicCoordinateSpatialReference(esriSRGeoCSType.esriSRGeoCS_WGS1984);

        public static ISpatialReference CreateGeographicCoordinateSpatialReference(esriSRGeoCSType geoCsType)
        {
            ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
            ISpatialReference spatialReference = spatialReferenceFactory.CreateGeographicCoordinateSystem((int)geoCsType);
            ISpatialReferenceResolution spatialReferenceResolution = (ISpatialReferenceResolution)spatialReference;
            spatialReferenceResolution.ConstructFromHorizon();
            spatialReferenceResolution.SetDefaultXYResolution();
            ISpatialReferenceTolerance spatialReferenceTolerance = (ISpatialReferenceTolerance)spatialReference;
            spatialReferenceTolerance.SetDefaultXYTolerance();
            return spatialReference;
        }

        public static ISpatialReference CreateProjectedCoordinateSpatialReference(esriSRProjCSType projCsType)
        {
            ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
            ISpatialReference spatialReference = spatialReferenceFactory.CreateProjectedCoordinateSystem((int)projCsType);
            ISpatialReferenceResolution spatialReferenceResolution = (ISpatialReferenceResolution)spatialReference;
            spatialReferenceResolution.ConstructFromHorizon();
            spatialReferenceResolution.SetDefaultXYResolution();
            ISpatialReferenceTolerance spatialReferenceTolerance = (ISpatialReferenceTolerance)spatialReference;
            spatialReferenceTolerance.SetDefaultXYTolerance();
            return spatialReference;
        }

        public static IGeometry SimplifyGeometry(IGeometry geometry)
        {
            if (geometry == null)
                return null;

            //Set the IsKnownSimple property to false, otherwise simplification will not take place.
            ITopologicalOperator2 topoOp = geometry as ITopologicalOperator2;
            if (topoOp != null)
            {
                topoOp.IsKnownSimple_2 = true;

                switch (geometry.GeometryType)
                {
                    case esriGeometryType.esriGeometryMultipoint:
                        topoOp.Simplify();
                        break;
                    case esriGeometryType.esriGeometryPolygon:
                        ((IPolygon)geometry).SimplifyPreserveFromTo();
                        break;
                    case esriGeometryType.esriGeometryPolyline:
                        var polyline6 = geometry as IPolyline6;
                        if (polyline6 != null)
                            polyline6.SimplifyNonPlanar();
                        else
                            ((IPolyline)geometry).SimplifyNetwork();
                        break;
                }
            }

            return geometry;
        }

        #endregion
    }
}
