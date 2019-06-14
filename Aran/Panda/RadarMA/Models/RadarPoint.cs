using ESRI.ArcGIS.Geometry;

namespace Aran.Panda.RadarMA.Models
{
    public class RadarPoint
    {
        private IPoint _geo;
        public int Id { get; set; }

        public ESRI.ArcGIS.Geometry.IPoint Geo
        {
            get { return _geo; }
            set
            {
                _geo = value;
                if (_geo!=null)
                GeoPrj =(IPoint) GlobalParams.SpatialRefOperation.ToEsriPrj(_geo);
            }
        }

        public double Range { get; set; }
        public string RangeUOM { get; set; }
        public double MagVar { get; set; }

        public ESRI.ArcGIS.Geometry.IPoint GeoPrj { get; set; }

        public string Name { get; set; }
        public RadarPointChoiceType Type { get; set; }
    }
}
