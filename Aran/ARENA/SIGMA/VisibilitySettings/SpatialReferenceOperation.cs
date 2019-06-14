using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace VisibilityTool
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
            ISpatialReference pSpRefPrj = pMap.SpatialReference;
            IProjectedCoordinateSystem pPCS = pMap.SpatialReference as IProjectedCoordinateSystem;
            ISpheroid pSpheroid = pPCS.GeographicCoordinateSystem.Datum.Spheroid;

            ISpatialReference pSpRefShp = _geoSpReference;

            ((IGeometry)pGeo).SpatialReference = pSpRefShp;
            ((IGeometry)pGeo).Project(pSpRefPrj);
            return pGeo;
        }

        public void CreateGeoSpReference()
        {
            //var layerName = "Airspace_C";
            //var upLayerName = layerName.ToUpper();
            //IFeatureLayer featureLayer = null;
            //var map = GlobalParams.HookHelper.FocusMap;
            //for (int i = 0; i < map.LayerCount; i++)
            //{
            //    var layer = map.Layer[i];

            //    if (layer.Name.ToUpper() == upLayerName)
            //    {
            //        featureLayer = (IFeatureLayer)layer;
            //        break;
            //    }
            //}
            ISpatialReferenceFactory spatFactor = new SpatialReferenceEnvironment();
            _geoSpReference = (ISpatialReference)spatFactor.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);
        }
    }
}