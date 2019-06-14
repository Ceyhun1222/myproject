using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Geometries.SpatialReferences;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Geometry;

namespace Aran.Panda.RadarMA
{
     public class SpatialReferenceOperation
    {
        private ISpatialReference _geoSpatialReference;
        private ISpatialReference _prjSpatialReference;

        public SpatialReferenceOperation(ISpatialReference prjSpatialReference)
        {
            _prjSpatialReference = prjSpatialReference;
          //  CreateGeoSpatialReference();
            var spRefConverter = new SpatRefConverter();
            SpRefPrj = spRefConverter.FromEsriSpatRef(_prjSpatialReference);
            SpRefGeo = CreateSpRef(SpatialReferenceType.srtGeographic);//spRefConverter.FromEsriSpatRef(_geoSpatialReference);
            _geoSpatialReference = spRefConverter.ToEsriSpatRef(SpRefGeo);

        }

        public SpatialReferenceOperation(SpatialReference prjSpatialReference)
        {
            var spRefConverter = new SpatRefConverter();
            _prjSpatialReference = spRefConverter.ToEsriSpatRef(prjSpatialReference);
         //   CreateGeoSpatialReference();
            SpRefPrj = spRefConverter.FromEsriSpatRef(_prjSpatialReference);
            SpRefGeo = CreateSpRef(SpatialReferenceType.srtGeographic);
            _geoSpatialReference = spRefConverter.ToEsriSpatRef(SpRefGeo);
        }

        public void CreateGeoSpatialReference()
        {
            ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironment();
            //Create a geographic coordinate system using the available geographic 
            //coordinate systems. These can be found in the esriGeometry esriSRGeoCSType
            //enumeration. 
            _geoSpatialReference = (ISpatialReference)spatialReferenceFactory.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);
        }

        private SpatialReference CreateSpRef(SpatialReferenceType spatialReferenceType)
        {
            var result = new SpatialReference {Name = "WGS1984"};
            result.Ellipsoid.SrGeoType = SpatialReferenceGeoType.srgtWGS1984;
            result.Ellipsoid.SemiMajorAxis = 6378137.0;
            result.Ellipsoid.Flattening = 1 / 298.25722356300003;

            result.SpatialReferenceUnit = SpatialReferenceUnit.sruMeter;

            switch (spatialReferenceType)
            {
                case SpatialReferenceType.srtGeographic:
                    result.SpatialReferenceType = SpatialReferenceType.srtGeographic;
                    break;
                case SpatialReferenceType.srtMercator:
                    result.SpatialReferenceType = SpatialReferenceType.srtMercator;
                    result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptFalseEasting, SpRefPrj[SpatialReferenceParamType.srptFalseEasting]));
                    result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptFalseNorthing, SpRefPrj[SpatialReferenceParamType.srptFalseNorthing]));
                    result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptCentralMeridian, SpRefPrj[SpatialReferenceParamType.srptCentralMeridian]));
                    result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptLatitudeOfOrigin, SpRefPrj[SpatialReferenceParamType.srptStandardParallel1]));
                    break;
                case SpatialReferenceType.srtTransverse_Mercator:
                    result.SpatialReferenceType = SpatialReferenceType.srtTransverse_Mercator;
                    result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptFalseEasting, SpRefPrj[SpatialReferenceParamType.srptFalseEasting]));
                    result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptFalseNorthing, SpRefPrj[SpatialReferenceParamType.srptFalseNorthing]));
                    result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptCentralMeridian, SpRefPrj[SpatialReferenceParamType.srptCentralMeridian]));
                    result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptLatitudeOfOrigin, SpRefPrj[SpatialReferenceParamType.srptLatitudeOfOrigin]));
                    result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptScaleFactor, SpRefPrj[SpatialReferenceParamType.srptScaleFactor]));
                    break;
                case SpatialReferenceType.srtGauss_Krueger:
                    result.SpatialReferenceType = SpatialReferenceType.srtGauss_Krueger;
                    result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptFalseEasting, SpRefPrj[SpatialReferenceParamType.srptFalseEasting]));
                    result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptFalseNorthing, SpRefPrj[SpatialReferenceParamType.srptFalseNorthing]));
                    result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptCentralMeridian, SpRefPrj[SpatialReferenceParamType.srptCentralMeridian]));
                    result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptLatitudeOfOrigin, SpRefPrj[SpatialReferenceParamType.srptLatitudeOfOrigin]));
                    result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptScaleFactor, SpRefPrj[SpatialReferenceParamType.srptScaleFactor]));
                    break;
                default:
                    throw new NotImplementedException();
            }
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

        public T ToEsriGeo<T>(T geom) where T : IGeometry
        {
            ESRI.ArcGIS.esriSystem.IClone clone = geom as ESRI.ArcGIS.esriSystem.IClone;
            IGeometry geom2 = clone.Clone() as IGeometry;
            geom2.SpatialReference = _prjSpatialReference;
            geom2.Project(_geoSpatialReference);
            return (T)geom2;
        }

        public T ToGeo<T>(T ptPrj) where T : Geometry
        {
            if (ptPrj == null)
                return null;
            IGeometry esriPrjGeo = Aran.Converters.ConvertToEsriGeom.FromGeometry(ptPrj);
            IGeometry esriGeo = ToEsriGeo(esriPrjGeo);

            var geometry = Aran.Converters.ConvertFromEsriGeom.ToGeometry(esriGeo,true);

            if (ptPrj.Type == GeometryType.Polygon)
                return (geometry as MultiPolygon)[0] as T;
            if (ptPrj.Type == GeometryType.LineString)
                return (geometry as MultiLineString)[0] as T;
            return geometry as T; ;
        }

        public T ToPrj<T>(T ptGeo) where T : Geometry
        {
            if (ptGeo == null)
                return null;

            IGeometry esriPrjGeo = Aran.Converters.ConvertToEsriGeom.FromGeometry(ptGeo);
            IGeometry esriGeo = ToEsriPrj(esriPrjGeo);

            var geometry = Aran.Converters.ConvertFromEsriGeom.ToGeometry(esriGeo);

            return geometry as T;
        }

        public SpatialReference SpRefPrj { get; private set; }
        public SpatialReference SpRefGeo { get; private set; }

        public double AztToDirGeo(Aran.Geometries.Point ptGeo, double azimuthInDeg)
        {
            return ARANFunctions.AztToDirection(ptGeo, azimuthInDeg, SpRefGeo, SpRefPrj);
        }

        public double AztToDirPrj(Aran.Geometries.Point ptPrj, double azimuthInDeg)
        {
            var ptGeo = ToGeo<Aran.Geometries.Point>(ptPrj);
            return ARANFunctions.AztToDirection(ptGeo, azimuthInDeg, SpRefGeo, SpRefPrj);
        }

        public double DirToAztPrj(Aran.Geometries.Point ptPrj, double directionInRad)
        {
            return ARANFunctions.DirToAzimuth(ptPrj, directionInRad, SpRefPrj, SpRefGeo);
        }

        public double DirToAztGeo(Aran.Geometries.Point ptGeo, double directionInRad)
        {
            var ptPrj = ToPrj<Aran.Geometries.Point>(ptGeo);
            return ARANFunctions.DirToAzimuth(ptPrj, directionInRad, SpRefPrj, SpRefGeo);
        }
    }
    
}
