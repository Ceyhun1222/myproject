using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace OIS
{
    public class SpatialReferenceOperation
    {
		private IMap _map;

        private ISpatialReference _geoSpReference;

        public SpatialReferenceOperation(IMap map)
        {
			_map = map;
            ISpatialReferenceFactory spatFactor = new SpatialReferenceEnvironment();
            _geoSpReference = (ISpatialReference)spatFactor.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);
        }

        public IGeometry ToProject(IGeometry pGeo)
        {
            ISpatialReference pSpRefPrj = _map.SpatialReference;
            IProjectedCoordinateSystem pPCS = _map.SpatialReference as IProjectedCoordinateSystem;
            ISpheroid pSpheroid = pPCS.GeographicCoordinateSystem.Datum.Spheroid;

            ISpatialReference pSpRefShp = _geoSpReference;

            ((IGeometry)pGeo).SpatialReference = pSpRefShp;
            ((IGeometry)pGeo).Project(pSpRefPrj);
            return pGeo;
        }
    }
}