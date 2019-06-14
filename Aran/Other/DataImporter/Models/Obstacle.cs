using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Geometries;

namespace DataImporter
{
    public enum ObstacleGeoType
    {
        Point,
        Polygon,
        PolyLine,
        Circle
    }

    public class Obstacle
    {
        public Obstacle()
        {
            Checked = true;
        }

        public bool Checked { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public double X { get; set; }
        public double Y { get; set; }
        public double Elev { get; set; }
        public string RMK { get; set; }
        public bool? CodeGrp { get; set; }
        public bool? Lght { get; set; }
        public bool? Markings { get; set; }

        public string Lat { get; set; }
        public string Long { get; set; }
        public ObstacleGeoType GeoType { get; set; }
        public Aran.Geometries.Geometry Geo { get; internal set; }
        public Aran.Geometries.Geometry GeoPrj { get; internal set; }
        public double Radius { get; internal set; }
    }
}
