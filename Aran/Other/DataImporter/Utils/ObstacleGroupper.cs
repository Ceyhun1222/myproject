using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aran.PANDA.Common;

namespace DataImporter.Utils
{
    public class ObstacleGroupper:IObstacleGroupper
    {
        public ObstacleGroupper()
        {
        }

        public async Task<List<Obstacle>> GroupAsync(List<Obstacle> obstacles)
        {
            var result = new List<Obstacle>();
            var lines = await GroupLinesAsync(obstacles).ConfigureAwait(false);
            var polygons = await GroupPolygonesAsync(obstacles).ConfigureAwait(false);
            var circles = await GroupCirclesAsync(obstacles).ConfigureAwait(false);

            result.AddRange(lines);
            result.AddRange(polygons);
            result.AddRange(circles);

            obstacles.ForEach(obs =>
            {
                if (obs.GeoType == ObstacleGeoType.Point)
                    result.Add(obs);
            });

            return result;
        }

        public Task<List<Obstacle>> GroupLinesAsync(List<Obstacle> obstacles)
        {
            return Task.Factory.StartNew(() =>
            {
                var result = new List<Obstacle>();

                dynamic groupedObstacles = GroupObstaclesByGeoType(ObstacleGeoType.PolyLine, obstacles);

                foreach (var groupObstacles in groupedObstacles)
                {
                    if (groupObstacles.Values?.Count > 1)
                    {
                        List<Obstacle> groupedObstaclesValues = groupedObstacles.Values;

                        var lineString = new Aran.Geometries.LineString();
                        var geo = new Aran.Geometries.MultiLineString {lineString};
                        foreach (var obstacle in groupedObstaclesValues)
                            lineString.Add(obstacle.Geo as Aran.Geometries.Point);

                        var addedObstacle = groupObstacles.Values[0];
                        addedObstacle.Geo = geo;
                        result.Add(addedObstacle);
                    }
                }
                return result;
            });
        }

        public Task<List<Obstacle>> GroupPolygonesAsync(List<Obstacle> obstacles)
        {
            return Task.Factory.StartNew(() =>
            {
                var result = new List<Obstacle>();
                dynamic groupedObstacles = GroupObstaclesByGeoType(ObstacleGeoType.Polygon,obstacles);
                foreach (var groupObstacle in groupedObstacles)
                {
                    if (groupObstacle.Values?.Count > 1)
                    {
                        List <Obstacle> groupedObstaclesValues= groupObstacle.Values;

                        var poly = new Aran.Geometries.Polygon { ExteriorRing = new Aran.Geometries.Ring() };
                        var geo = new Aran.Geometries.MultiPolygon { poly };
                        foreach (var obstacle in groupedObstaclesValues)
                            poly.ExteriorRing.Add(obstacle.Geo as Aran.Geometries.Point);


                        var addedObstacle = groupObstacle.Values[0];
                        addedObstacle.Geo = geo;
                        result.Add(addedObstacle);
                    }
                }
                return result;
            });
        }

        public Task<List<Obstacle>> GroupCirclesAsync(List<Obstacle> obstacles)
        {
            return Task.Factory.StartNew(() =>
            {
                var result = new List<Obstacle>();

                dynamic groupedObstacles = GroupObstaclesByGeoType(ObstacleGeoType.Circle,obstacles);
                foreach (var groupObstacles in groupedObstacles)
                {
                    if (groupObstacles.Values?.Count == 1)
                    {
                        var obstacle = groupObstacles.Values[0];

                        if (obstacle.Geo != null && Math.Abs(obstacle.Radius) > ARANMath.Epsilon)
                        {
                            var geo = ARANFunctions.CreateCircleGeo(obstacle.Geo as Aran.Geometries.Point, obstacle.Radius);
                            obstacle.Geo = new Aran.Geometries.MultiPolygon { geo };
                            result.Add(obstacle);
                        }
                    }
                }
                return result;
            });
        }

        private object GroupObstaclesByGeoType(ObstacleGeoType geoType, List<Obstacle> obstacles)
        {
            var groupedObstacles = from obstacle in obstacles
                where obstacle.GeoType == ObstacleGeoType.PolyLine
                group obstacle by obstacle.Name
                into g
                select new { Id = g.Key, Values = g.ToList() };

            return groupedObstacles;
        }
      
    }
}
