using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATRACC.Converter.Template.AtsGeoPoints
{
    public class VorDme
    {
        public string Designator { get; set; }

        public string MagneticVariation { get; set; }
 
    }

    public class GeoPoint
    {
        public string Designator { get; set; }

        public string Coordinates { get; set; }
        public string More { get; set; }
    }

    partial class AtsGeoPointsTemplate
    {
        private List<VorDme> _vorDme=new List<VorDme>();
        private List<GeoPoint> _geoPoints=new List<GeoPoint>();

        public List<VorDme> VorDme
        {
            get { return _vorDme; }
            set { _vorDme = value; }
        }

        public List<GeoPoint> GeoPoints
        {
            get { return _geoPoints; }
            set { _geoPoints = value; }
        }
    }
}
