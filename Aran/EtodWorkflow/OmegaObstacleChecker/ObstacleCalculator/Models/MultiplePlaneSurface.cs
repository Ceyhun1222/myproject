using System.Collections.Generic;

namespace ObstacleCalculator.Domain.Models
{
    class MultiplePlaneSurface:SurfaceBase
    {
        public List<Plane> Planes { get; set; }
    }
}
