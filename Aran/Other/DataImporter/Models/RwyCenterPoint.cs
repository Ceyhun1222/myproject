using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Geometries;

namespace DataImporter
{
    public class RwyCenterPoint
    {
        public RwyCenterPoint()
        {
            Checked = true;
        }
        public bool Checked { get; set; }
        public string ID { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Elev { get; set; }
        public string Name { get; set; }
        public string RMK { get; set; }
        public double Geoid { get; set; }

        public string Lat { get; set; }
        public string Long { get; set; }
        public Point Geo { get; internal set; }
        public Point GeoPrj { get; internal set; }
    }
}
