using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Geometry;

namespace SigmaChart
{
    public enum ObstaclePartType
    {
        Point,
        PolyLine,
        Polygon
    }

    public class Obstacle
    {
        public Obstacle()
        {
            ObstacleParts = new List<ObstaclePart>();
        }

        public int ID { get; set; }
        public string Identifier { get; set; }
        public string Name { get; set; }
        public IGeometry Geo { get; set; }
        public List<ObstaclePart> ObstacleParts { get; set; }
    }

    public class ObstaclePart
    {
        public string Identifier{ get; set; }
        public string ObstacleID { get; set; }
        public double Elevation { get; set; }
        public string Uom { get; set; }
        public ObstaclePartType ObstacleType { get; set; }
        
        public Obstacle_Geometry Geometry { get; set; }
    }

    public class Obstacle_Geometry
    {
        public string PartID { get; set; }
        public IGeometry Shape { get; set; }
    }

}
