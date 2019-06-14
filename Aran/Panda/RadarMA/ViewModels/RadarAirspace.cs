using ESRI.ArcGIS.Geometry;

namespace Aran.Panda.RadarMA.ViewModels
{
    public class RadarAirspace
    {
        public IPolygon Geo { get; internal set; }
        public int Id { get; internal set; }
        public string Name { get; internal set; }
        public string Type { get; internal set; }
    }
}