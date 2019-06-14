using Aran.Package;
using System;

namespace Aran.Aim.Data.Filters
{
    [Serializable]
    public class Within : SpatialOps
    {
        public Aran.Geometries.Geometry Geometry { get; set; }

        public override void Pack(PackageWriter writer)
        {
            base.Pack(writer);

            var isGeomNull = (Geometry == null);
            writer.PutBool(isGeomNull);
            if (!isGeomNull) {
                writer.PutEnum<Aran.Geometries.GeometryType>(Geometry.Type);
                Geometry.Pack(writer);
            }
        }

        public override void Unpack(PackageReader reader)
        {
            base.Unpack(reader);

            var isGeomNull = reader.GetBool();
            if (!isGeomNull) {
                var geomType = reader.GetEnum<Aran.Geometries.GeometryType>();
                Geometry = Aran.Geometries.Geometry.Create(geomType);
                Geometry.Unpack(reader);
            }
        }
    }
}
