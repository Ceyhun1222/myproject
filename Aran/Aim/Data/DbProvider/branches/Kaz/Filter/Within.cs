
namespace Aran.Aim.Data.Filters
{
    public class Within : SpatialOps
    {
        public string PropertyName
        {
            get;
            set;
        }
        public Aran.Geometries.Geometry Geometry
        {
            get;
            set;
        }
    }
}
