using Aran.Aim.DataTypes;

namespace Aran.Aim.Data.Filters
{
    public class DWithin : SpatialOps
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
        public ValDistance Distance
        {
            get;
            set;
        }
    }
}
