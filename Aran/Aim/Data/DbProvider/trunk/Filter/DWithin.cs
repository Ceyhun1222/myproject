using Aran.Aim.DataTypes;
using Aran.Package;
using System;

namespace Aran.Aim.Data.Filters
{
    [Serializable]
    public class DWithin : SpatialOps
    {
        public Aran.Geometries.Point Point { get; set; }

        public ValDistance Distance { get; set; }

        public override void Pack(PackageWriter writer)
        {
            base.Pack(writer);

            var isDistanceNull = (Distance == null);
            writer.PutBool(isDistanceNull);
            
            if (!isDistanceNull)
                (Distance as IPackable).Pack(writer);

            var isGeomNull = (Point == null);
            writer.PutBool(isGeomNull);
            
            if (!isGeomNull) {
                writer.PutEnum<Aran.Geometries.GeometryType>(Point.Type);
                Point.Pack(writer);
            }
        }

        public override void Unpack(PackageReader reader)
        {
            base.Unpack(reader);

            var isDistanceNull = reader.GetBool();
            if (!isDistanceNull) {
                Distance = new Aran.Aim.DataTypes.ValDistance();
                (Distance as IPackable).Unpack(reader);
            }

            var isGeomNull = reader.GetBool();
            if (!isGeomNull) {
                var geomType = reader.GetEnum<Aran.Geometries.GeometryType>();
                //Geometry = Aran.Geometries.Geometry.Create(geomType);
                Point = new Geometries.Point();
                Point.Unpack(reader);
            }
        }
    }
}
