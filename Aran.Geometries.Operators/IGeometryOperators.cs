using GeoAPI.Operation.Buffer;

namespace Aran.Geometries.Operators
{
    public interface IGeometryOperators
    {
        Geometry CurrentGeometry { get; set; }

        Geometry Boundary(Geometry geom);
        Geometry Buffer(Geometry geom, double width);
        Geometry Buffer(Geometry geom, double width, int quadrantSegments);
        Geometry BufferWithCapStyle(Geometry geom, double width, EndCapStyle bufferStyle = EndCapStyle.Round);
        bool Contains(Geometry other);
        bool Contains(Geometry geom, Geometry other);
        Geometry ConvexHull(Geometry geom);
        bool Crosses(Geometry other);
        bool Crosses(Geometry geom, Geometry other);
        Geometry Difference(Geometry geom1);
        Geometry Difference(Geometry geom1, Geometry geom2);
        bool Disjoint(Geometry other);
        bool Disjoint(Geometry geom, Geometry other);
        double GetDistance(Geometry other);
        double GetDistance(Geometry geom, Geometry other);
        Geometry Intersect(Geometry geom);
        Geometry Intersect(Geometry geom1, Geometry geom2);
        Geometry SimplifyGeometry(Geometry geom, double tolerance = 0.001);
        Geometry UnionGeometry(Geometry geom1);
        Geometry UnionGeometry(Geometry geom1, Geometry geom2);
    }
}