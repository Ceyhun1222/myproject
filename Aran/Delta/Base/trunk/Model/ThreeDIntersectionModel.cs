using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Geometries;

namespace Aran.Delta.Model
{
    class ThreeDIntersectionModel
    {
        public string Layer
        {
            get
            {
                if (IntersectedAirspace != null)
                    return IntersectedAirspace.GetLayerName();
                else if (IntersectedRoute != null)
                    return IntersectedRoute.GetLayerName();

                return "";

            }
        }
        public MultiPolygon AirspaceGeo { get; set; }
        public MultiLineString RouteGeo { get; set; }
        public Geometry IntersectGeo { get; set; }
        public MultiPolygon BufferGeo { get; set; }
        public bool IsIntersect { get; set; }
        public double MinIntersectValue { get; set; }
        public string MinIntersectUnit { get; set; }
        public double MaxIntersectValue { get; set; }
        public string MaxIntersectUnit { get; set; }
        public bool IntersectedIn3D { get; set; }

        public Airspace IntersectedAirspace { get; set; }
        public Route IntersectedRoute { get; set; }
        public ValDistanceVertical LowerLimit { get; set; }
        public ValDistanceVertical UpperLimit { get; set; }
        public double BufferWidth { get; set; }

        public string Name { get; set; }
    }
}
