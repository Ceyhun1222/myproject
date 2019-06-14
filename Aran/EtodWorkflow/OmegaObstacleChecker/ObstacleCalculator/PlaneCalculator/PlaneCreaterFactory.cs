using System;
using System.Collections.Generic;
using GeoAPI.CoordinateSystems.Transformations;
using ObstacleCalculator.Domain.Models;
using ObstacleCalculator.Domain.PlaneCalculator;
using ObstacleCalculator.Domain.Utils;

namespace ObstacleCalculator.Domain.PlaneCalculator
{
    public class PlaneCreaterFactory
    {
        private readonly IEnumerable<SurfaceBase> _surfaces;
        private readonly GeoJsonToJtsGeo _geoJsonToJtsGeo;

        /// <exception cref="ArgumentNullException"><paramref name="surfaces"/> is <see langword="null"/></exception>
        public PlaneCreaterFactory(IEnumerable<SurfaceBase> surfaces,IMathTransform mathTranform)
        {
            _surfaces = surfaces;
            if (_surfaces==null)
                throw new ArgumentNullException(nameof(surfaces));

            var mathTranform1 = mathTranform;
            if(mathTranform1==null)
                throw new ArgumentNullException(nameof(mathTranform));

            _geoJsonToJtsGeo = new GeoJsonToJtsGeo(mathTranform1);
        }

        public IEnumerable<IPlaneSurface> GetPlanes()
        {
            foreach (var surface in _surfaces)
            {
                switch (surface.Type)
                {
                    case  CodeObstacleArea.AREA1:
                        yield return new Area1Surface(surface);
                        break;
                    case CodeObstacleArea.AREA3:
                        yield return new Area3Surface(surface, _geoJsonToJtsGeo);
                        break;
                    case CodeObstacleArea.OTHER_AREA2C:
                        yield return new Area2CSurface(surface,_geoJsonToJtsGeo);
                        break;
                    case CodeObstacleArea.OTHER_AREA2D:
                        yield return new Area2DSurface(surface,_geoJsonToJtsGeo);
                        break;
                    case CodeObstacleArea.OTHER_CONICAL:
                        yield return new ConicalSurface(surface, _geoJsonToJtsGeo); ;
                        break;
                    default:
                        yield return new PlaneSurface(surface, _geoJsonToJtsGeo); ;
                        break;
                }
            }
        }
    }
}
