using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Geometry;

namespace Aran.Panda.RadarMA.Models
{
    public class ObstacleReport
    {
        public ObstacleReport(string name, 
            IGeometry geo,
            IGeometry geoPrj,
            double elevation,
            string source)
        {
            Name = name;
            Geo = geo;
            GeoPrj = geoPrj;
            Elevation =elevation;
            DataSource = source;

            if (!(Geo is IPoint pt)) return;

            ARANFunctions.Dd2DmsStr(pt.X, pt.Y, ",", "E", "N", 1, 2, out var longtitudeStr, out var latitudeStr);

            Latitude = latitudeStr;
            Longtitude = longtitudeStr;
        }

        public string DataSource { get;}

        public string Name { get; }

        public IGeometry Geo { get;}

        public IGeometry GeoPrj { get; }

        public double Elevation { get;}

        public string Latitude { get; set; }

        public string Longtitude { get; set; }
    }
}
