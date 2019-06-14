using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Geometries.SpatialReferences;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Geometry;

namespace SigmaChart
{
     public class SpatialReferenceOperation
    {
        private ISpatialReference _geoSpatialReference;
        private ISpatialReference _prjSpatialReference;
        private SpatialReference _aranSpPrjReference;
        private SpatRefConverter _spRefConverter;
        private SpatialReference _aranSpGeoReference;

        public SpatialReferenceOperation(double centralMeridian=0)
        {
            //  CreateGeoSpatialReference();
            _aranSpPrjReference = CreateSpRef(centralMeridian);
            _aranSpGeoReference = CreateGeoSpatialReference();

            _spRefConverter = new SpatRefConverter();
            _prjSpatialReference = _spRefConverter.ToEsriSpatRef(_aranSpPrjReference);
            _geoSpatialReference = _spRefConverter.ToEsriSpatRef(_aranSpGeoReference);

            
            CreateGeoSpatialReference();
        }

        public SpatialReference CreateGeoSpatialReference()
        {
            var result = new SpatialReference { Name = "WGS1984" };
            result.Ellipsoid.SrGeoType = SpatialReferenceGeoType.srgtWGS1984;
            result.Ellipsoid.SemiMajorAxis = 6378137.0;
            result.Ellipsoid.Flattening = 1 / 298.25722356300003;

            result.SpatialReferenceUnit = SpatialReferenceUnit.sruMeter;
            result.SpatialReferenceType = SpatialReferenceType.srtGeographic;
            return result;
        }

         public void ChangeCentralMeridian(double centralMeridian)
         {
             foreach (var spatialReferenceParam in _aranSpPrjReference.ParamList)
             {
                 if (spatialReferenceParam.SRParamType == SpatialReferenceParamType.srptCentralMeridian)
                     spatialReferenceParam.Value = centralMeridian;
             }

             _prjSpatialReference = _spRefConverter.ToEsriSpatRef(_aranSpPrjReference);
         }

         private SpatialReference CreateSpRef(double centralMeridian)
         {
             var result = new SpatialReference {Name = "WGS1984"};
             result.Ellipsoid.SrGeoType = SpatialReferenceGeoType.srgtWGS1984;
             result.Ellipsoid.SemiMajorAxis = 6378137.0;
             result.Ellipsoid.Flattening = 1/298.25722356300003;

             result.SpatialReferenceUnit = SpatialReferenceUnit.sruMeter;

             result.SpatialReferenceType = SpatialReferenceType.srtTransverse_Mercator;
             result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptFalseEasting, 500000.0));
             result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptFalseNorthing, 0));
             result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptCentralMeridian,
                 centralMeridian));
             result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptLatitudeOfOrigin, 0));
             result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptScaleFactor, 0.9996));

             return result;
         }

         public IGeometry ToEsriPrj(IGeometry geom)
        {
            ESRI.ArcGIS.esriSystem.IClone clone = geom as ESRI.ArcGIS.esriSystem.IClone;
            IGeometry geom2 = clone.Clone() as IGeometry;
            geom2.SpatialReference = _geoSpatialReference;
            geom2.Project(_prjSpatialReference);
            
            return geom2;
        }

        public IGeometry ToEsriGeo(ISpatialReference prjSp,IGeometry geom)
        {
            ESRI.ArcGIS.esriSystem.IClone clone = geom as ESRI.ArcGIS.esriSystem.IClone;
            IGeometry geom2 = clone.Clone() as IGeometry;
            geom2.SpatialReference = prjSp;
            geom2.Project(_geoSpatialReference);

            return geom2;
        }

        public T ToEsriGeo<T>(T geom) where T : IGeometry
        {
            ESRI.ArcGIS.esriSystem.IClone clone = geom as ESRI.ArcGIS.esriSystem.IClone;
            IGeometry geom2 = clone.Clone() as IGeometry;
            geom2.SpatialReference = _prjSpatialReference;
            geom2.Project(_geoSpatialReference);
            return (T)geom2;
        }

         public void ChangeSpatialReference(ISpatialReference spReference)
         {
             _prjSpatialReference = spReference;
         }
    }
    
}
