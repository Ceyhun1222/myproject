using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAO015
{
    public class IlyasMclass
    {
        Aran.Geometries.Point pt;
        long id;       
        double elevation;
        //double uom;

        public long ID
        {
            get { return id; }
            set { id = value; }
        }        
        public double Elevation
        {
            get { return elevation; }
            set { elevation = value; }
        }
        public Aran.Geometries.Point Point
        {
            get { return pt; }
            set { pt = value; }
        }
    }
}
