using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using EsriWorkEnvironment;

namespace EnrouteChartCompare.Helper
{
    public class SpatialReferenceOperation
    {
        private ISpatialReference _geoSpReference;

        public SpatialReferenceOperation()
        {
            CreateGeoSpReference();
        }

        public IGeometry ToProject(IGeometry pGeo)
        {
            var pMap = GlobalParams.HookHelper.FocusMap;
            var pSpRefPrj = pMap.SpatialReference;
            var pPcs = (IProjectedCoordinateSystem) pMap.SpatialReference;

            var pSpRefShp = _geoSpReference;

            ((IGeometry) pGeo).SpatialReference = pSpRefShp;
            ((IGeometry) pGeo).Project(pSpRefPrj);
            return pGeo;
        }

        private void CreateGeoSpReference()
        {
            IFeatureLayer featureLayer = null;
            var map = GlobalParams.HookHelper.FocusMap;

            var layer = EsriUtils.getLayerByName(map, "AirportHeliport") ?? EsriUtils.getLayerByName(map, "AirportCartography");
            featureLayer = (IFeatureLayer)layer;
            _geoSpReference = ((IGeoDataset) featureLayer).SpatialReference;
        }
    }
}