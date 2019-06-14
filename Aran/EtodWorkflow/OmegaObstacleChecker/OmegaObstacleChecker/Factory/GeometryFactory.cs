using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoAPI.CoordinateSystems.Transformations;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using ObstacleCalculator.Domain.Enums;
using ObstacleChecker.API.Model;

namespace ObstacleChecker.API.Factory
{
    public class GeometryFactory
    {
        private readonly IMathTransform _mathTransform;

        public GeometryFactory(IMathTransform mathTransform)
        {
            _mathTransform = mathTransform;
        }

        public IGeometry CreateGeometryInPrj(ObstacleReportRequest obstacleReportRequest)
        {
            if (obstacleReportRequest.GeometryType == ObsacleGeometryType.Point)
            {
                var point = obstacleReportRequest.Points.FirstOrDefault();
                if (point == null)
                    throw new ArgumentException("Points cannot be empty!");

                var obstaclePrj = _mathTransform.Transform(new Coordinate(point.Longitude, point.Latitude));
                return new NetTopologySuite.Geometries.Point(obstaclePrj);
            }

            var coordinates = new Coordinate[obstacleReportRequest.Points.Count()];

            int i = 0;
            foreach (var point in obstacleReportRequest.Points)
            {
                coordinates[i++] = _mathTransform.Transform(new Coordinate(point.Longitude, point.Latitude));
            }

            if (obstacleReportRequest.GeometryType == ObsacleGeometryType.Polyline)
            {
                return new MultiLineString(new ILineString[] { new LineString(coordinates)});
            }

            return new Polygon(new LinearRing(coordinates));
        }
    }
}
