using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using System.Runtime.ExceptionServices;

namespace ChartTypeA
{
    public class SpatialReferenceOperation
    {
        private ISpatialReference _geoSpatialReference;
        private ISpatialReference _prjSpatialReference;

        public SpatialReferenceOperation(ISpatialReference prjSpatialReference)
        {
            _prjSpatialReference = prjSpatialReference;
            CreateGeoSpatialReference();
        }

        public void CreateGeoSpatialReference()
        {
            ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironment();
            //Create a geographic coordinate system using the available geographic 
            //coordinate systems. These can be found in the esriGeometry esriSRGeoCSType
            //enumeration. 
            _geoSpatialReference = (ISpatialReference)spatialReferenceFactory.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);

        }


        [HandleProcessCorruptedStateExceptions]
        public IGeometry ToEsriPrj(IGeometry geom)
        {
            try
            {
                ESRI.ArcGIS.esriSystem.IClone clone = geom as ESRI.ArcGIS.esriSystem.IClone;
                IGeometry geom2 = clone.Clone() as IGeometry;
                geom2.SpatialReference = _geoSpatialReference;
                geom2.Project(_prjSpatialReference);
                return geom2;
            }
            catch (System.AccessViolationException)
            {
                return null;
            }
        }

        public T ToEsriGeo<T>(T geom) where T : IGeometry
        {
            ESRI.ArcGIS.esriSystem.IClone clone = geom as ESRI.ArcGIS.esriSystem.IClone;
            IGeometry geom2 = clone.Clone() as IGeometry;
            geom2.SpatialReference = _prjSpatialReference;
            geom2.Project(_geoSpatialReference);
            return (T)geom2;
        }

        public ISpatialReference PrjSpatialReference { get {return _prjSpatialReference; } }
    }
}
