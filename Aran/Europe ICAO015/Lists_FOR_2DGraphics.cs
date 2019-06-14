using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAO015
{
    public class Lists_FOR_2DGraphics
    {
        string parenttext;
        string navaidtxt;
        double distance;
        long id;
        string obstacle;
        double elevation;
        double penetrate;
        string typegeo;
        string polygon_type;

        public string ParentTxt
        {
            get { return parenttext; }
            set { parenttext = value; }
        }
        public string ChildTxt
        {
            get { return navaidtxt; }
            set { navaidtxt = value; }
        }
        public long ID
        {
            get { return id; }
            set { id = value; }
        }
        public double Distance
        {
            get { return distance; }
            set { distance = value; }
        }
        public string Obstacle
        {
            get { return obstacle; }
            set { obstacle = value; }
        }
        public double Elevation
        {
            get { return elevation; }
            set { elevation = value; }
        }
        public double Penetrate
        {
            get { return penetrate; }
            set { penetrate = value; }
        }
        public string TypeGeo
        {
            get { return typegeo; }
            set { typegeo = value; }
        }
        public string Polygon_Type
        {
            get { return polygon_type; }
            set { polygon_type = value; }
        }

    }
}
