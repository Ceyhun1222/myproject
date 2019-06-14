using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Carto;

namespace ChartTypeA.Utils
{
    class ProjectionUtil
    {
        public static void ChangeProjectionAndMeredian(double CMeridian, IMap pMap)
        {
            //IMap pMap = pDocument.FocusMap;

            ISpatialReferenceFactory2 pSpatRefFact = new SpatialReferenceEnvironmentClass();
            IProjectionGEN pProjection = pSpatRefFact.CreateProjection((int)esriSRProjectionType.esriSRProjection_TransverseMercator) as IProjectionGEN;

            IGeographicCoordinateSystem pGCS = pSpatRefFact.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);
            ILinearUnit pLinearUnit = pSpatRefFact.CreateUnit((int)esriSRUnitType.esriSRUnit_Meter) as ILinearUnit;
            IProjectedCoordinateSystemEdit pProjCoordSysEdit = new ProjectedCoordinateSystemClass();
            IParameter[] pParams = pProjection.GetDefaultParameters();
            pParams[0].Value = 500000;
            pParams[1].Value = 0;
            pParams[2].Value = Math.Round(CMeridian, 6);
            pParams[3].Value = (double)0.9996;

            object name = "Transverse_Mercator";
            object alias = "UserDefinedAlias";
            object abbreviation = "Trans_Merc";
            object remarks = "ARAN coordinate system.";
            object usage = "";
            object CS = pGCS;
            object LU = pLinearUnit;
            object PRJ = pProjection;
            object PARAMS = pParams;

            pProjCoordSysEdit.Define(ref name, ref alias, ref abbreviation, ref remarks, ref usage, ref CS, ref LU, ref PRJ, ref PARAMS);

            ISpatialReference pPCS = (ESRI.ArcGIS.Geometry.IProjectedCoordinateSystem)pProjCoordSysEdit; // pRJ
            if (pMap != null)
            {
                pMap.SpatialReference = pPCS;
            }

            GlobalParams.SpatialRefOperation = new SpatialReferenceOperation(pPCS);

        }

    }
}
