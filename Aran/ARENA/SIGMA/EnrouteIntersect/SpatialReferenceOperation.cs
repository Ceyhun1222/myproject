using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace EnrouteIntersect
{
    public class SpatialReferenceOperation
    {
        private ISpatialReference _geoSpReference;

        public SpatialReferenceOperation()
        {
			ISpatialReferenceFactory spatFactor = new SpatialReferenceEnvironment ( );
			_geoSpReference = ( ISpatialReference ) spatFactor.CreateGeographicCoordinateSystem ( ( int ) esriSRGeoCSType.esriSRGeoCS_WGS1984 );
		}

        public IGeometry ToProject(IGeometry pGeo)
        {
            var pMap = GlobalParams.HookHelper.FocusMap;
            ISpatialReference pSpRefPrj = pMap.SpatialReference;

            ISpatialReference pSpRefShp = _geoSpReference;

            ((IGeometry)pGeo).SpatialReference = pSpRefShp;
            ((IGeometry)pGeo).Project(pSpRefPrj);
            return pGeo;
        }

		public IGeometry ToGeo ( IGeometry geometry )
		{
			var pMap = GlobalParams.HookHelper.FocusMap;
			ISpatialReference pSpRefPrj = pMap.SpatialReference;

			ISpatialReference pSpRefShp = _geoSpReference;

			( ( IGeometry ) geometry ).SpatialReference = pSpRefPrj;
			( ( IGeometry ) geometry ).Project ( pSpRefShp );
			return geometry;
		}
	}
}