using ESRI.ArcGIS.Geodatabase;

namespace TOSSM.Util.GeoUtil
{
    public class RelationCreationArgs
    {
        public ClassCreationArgs Source { get; set; }
        public ClassCreationArgs Target { get; set; }
        public esriRelCardinality Cardinality { get; set; }
        public string KeyInSource { get; set; }
        public string KeyInTarget { get; set; } //for many to many only
        public string Name { get; set; }
        public string Description { get; set; }
        public string ReversedDescription { get; set; }
    }
}