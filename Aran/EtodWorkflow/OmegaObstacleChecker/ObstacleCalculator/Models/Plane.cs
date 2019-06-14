using GeoAPI.Geometries;

namespace ObstacleCalculator.Domain.Models
{
    public class Plane
    {
        public Plane()
        {
            CalcType = CalculationType.ByCoord;
        }
        public PlaneParam Param { get; set; }

        public IPolygon Geo { get; set; }

        public CalculationType CalcType { get; set; }
    }
}
