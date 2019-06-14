using System;
using System.Linq;

using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Geometries.SpatialReferences;
using Aran.AranEnvironment;

namespace Aran.PANDA.Common
{
	public class SpatialReferenceOperation
	{
		private GeometryOperators _geometryOperators;
		private IAranEnvironment _environment;
		private SpatialReference _viewProjection;

		public SpatialReference SpRefPrj { get; private set; }
		public SpatialReference SpRefGeo { get; private set; }

		public SpatialReferenceOperation(IAranEnvironment environment)
		{
			_environment = environment;
			_viewProjection = _environment.Graphics.ViewProjection;

			if (_environment.Graphics.ViewProjection == null)
				throw new Exception("Spatial Reference is null");

			_geometryOperators = new GeometryOperators();


			SpRefGeo = CreateSpRef(SpatialReferenceType.srtGeographic);
		    SpRefPrj = _viewProjection; //CreateSpRef(_viewProjection.SpatialReferenceType);
		}

        public SpatialReferenceOperation(SpatialReference viewProjection)
        {
            _viewProjection = viewProjection;

            _geometryOperators = new GeometryOperators();

            SpRefGeo = CreateSpRef(SpatialReferenceType.srtGeographic);
            SpRefPrj = _viewProjection; //CreateSpRef(_viewProjection.SpatialReferenceType);
        }

		private SpatialReference CreateSpRef(SpatialReferenceType spatialReferenceType)
		{
			SpatialReference result = new SpatialReference();
			result.Name = "WGS1984";
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
					result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptFalseEasting, _viewProjection[SpatialReferenceParamType.srptFalseEasting]));
					result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptFalseNorthing, _viewProjection[SpatialReferenceParamType.srptFalseNorthing]));
					result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptCentralMeridian, _viewProjection[SpatialReferenceParamType.srptCentralMeridian]));
					result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptLatitudeOfOrigin, _viewProjection[SpatialReferenceParamType.srptStandardParallel1]));
					break;
				case SpatialReferenceType.srtTransverse_Mercator:
					result.SpatialReferenceType = SpatialReferenceType.srtTransverse_Mercator;
					result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptFalseEasting, _viewProjection[SpatialReferenceParamType.srptFalseEasting]));
					result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptFalseNorthing, _viewProjection[SpatialReferenceParamType.srptFalseNorthing]));
					result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptCentralMeridian, _viewProjection[SpatialReferenceParamType.srptCentralMeridian]));
					result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptLatitudeOfOrigin, _viewProjection[SpatialReferenceParamType.srptLatitudeOfOrigin]));
					result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptScaleFactor, _viewProjection[SpatialReferenceParamType.srptScaleFactor]));
					break;
				case SpatialReferenceType.srtGauss_Krueger:
					result.SpatialReferenceType = SpatialReferenceType.srtGauss_Krueger;
					result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptFalseEasting, _viewProjection[SpatialReferenceParamType.srptFalseEasting]));
					result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptFalseNorthing, _viewProjection[SpatialReferenceParamType.srptFalseNorthing]));
					result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptCentralMeridian, _viewProjection[SpatialReferenceParamType.srptCentralMeridian]));
					result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptLatitudeOfOrigin, _viewProjection[SpatialReferenceParamType.srptLatitudeOfOrigin]));
					result.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptScaleFactor, _viewProjection[SpatialReferenceParamType.srptScaleFactor]));
					break;
				default:
					throw new NotImplementedException();
			}
			return result;
		}

		public T ToGeo<T>(T prjGeom) where T : Geometry
		{
			if (prjGeom == null)
				return null;

			Geometry geometry = _geometryOperators.GeoTransformations(prjGeom, SpRefPrj, SpRefGeo);


			if (prjGeom.Type == GeometryType.Polygon)
			{
				if (geometry.IsEmpty)
					return new Polygon() as T;

				return (geometry as MultiPolygon)[0] as T;
			}

			if (prjGeom.Type == GeometryType.LineString)
			{
				if (geometry.IsEmpty)
					return new LineString() as T;

				return (geometry as MultiLineString)[0] as T;
			}

			return geometry as T;
		}

		public T ToPrj<T>(T geoGeom) where T : Geometry
		{
			if (geoGeom == null)
				return null;
			return _geometryOperators.GeoTransformations(geoGeom, SpRefGeo, SpRefPrj) as T;
		}

		public double AztToDirGeo(Point ptGeo, double azimuthInDeg)
		{
			return ARANFunctions.AztToDirection(ptGeo, azimuthInDeg, SpRefGeo, SpRefPrj);
		}

		public double AztToDirPrj(Point ptPrj, double azimuthInDeg)
		{
			Point ptGeo = ToGeo<Point>(ptPrj);
			return ARANFunctions.AztToDirection(ptGeo, azimuthInDeg, SpRefGeo, SpRefPrj);
		}

		public double DirToAztPrj(Point ptPrj, double directionInRad)
		{
			return ARANFunctions.DirToAzimuth(ptPrj, directionInRad, SpRefPrj, SpRefGeo);
		}

		public double DirToAztGeo(Point ptGeo, double directionInRad)
		{
			Point ptPrj = ToPrj<Point>(ptGeo);
			return ARANFunctions.DirToAzimuth(ptPrj, directionInRad, SpRefPrj, SpRefGeo);
		}

        public Point ChangeCentralMeridian(Point ptGeo)
        {
            if (ptGeo == null)
                return null;

            double centralMeridian = ptGeo.X;
            foreach (SpatialReferenceParam item in SpRefPrj.ParamList)
            {
                if (item.SRParamType == SpatialReferenceParamType.srptCentralMeridian)
                    item.Value = Math.Round(centralMeridian);
            }

            _environment.Graphics.ViewProjection = SpRefPrj;
            double xMin, yMin, xMax, yMax;
            _environment.Graphics.GetExtent(out xMin, out yMin, out xMax, out yMax);

            Aran.Geometries.Point ptPrj = ToPrj(ptGeo);

            double radX = (xMax - xMin) / 2, radY = (yMax - yMin) / 2;

            _environment.Graphics.SetExtent(ptPrj.X - radX, ptPrj.Y - radY, ptPrj.X + radX, ptPrj.Y + radY);
            return ptPrj;
        }
	}
}
