#region Assembly GeoAPI, Version=1.7.4.0, Culture=neutral, PublicKeyToken=a1a0da7def465678
// D:\AirNav\RefFiles\JTS\GeoAPI.dll
#endregion

using System;
using GeoAPI.Operation.Buffer;

namespace Europe_ICAO015
{
    public interface IGeometry : ICloneable, IComparable, IComparable<IGeometry>, IEquatable<IGeometry>
    {
        bool IsValid { get; }
        GeoAPI.Geometries.Dimension Dimension { get; set; }
        int NumGeometries { get; }
        GeoAPI.Geometries.Coordinate Coordinate { get; }
        GeoAPI.Geometries.IPoint Centroid { get; }
        GeoAPI.Geometries.Dimension BoundaryDimension { get; }
        bool IsSimple { get; }
        IGeometry Envelope { get; }
        IGeometry Boundary { get; }
        GeoAPI.Geometries.IPrecisionModel PrecisionModel { get; }
        int SRID { get; set; }
        string GeometryType { get; }
        GeoAPI.Geometries.OgcGeometryType OgcGeometryType { get; }
        double Area { get; }
        double Length { get; }
        GeoAPI.Geometries.IGeometryFactory Factory { get; }
        GeoAPI.Geometries.Envelope EnvelopeInternal { get; }
        GeoAPI.Geometries.Coordinate[] Coordinates { get; }
        int NumPoints { get; }
        bool IsRectangle { get; }
        bool IsEmpty { get; }
        object UserData { get; set; }
        GeoAPI.Geometries.IPoint InteriorPoint { get; }
        GeoAPI.Geometries.IPoint PointOnSurface { get; }

        void Apply(GeoAPI.Geometries.IGeometryComponentFilter filter);
        void Apply(GeoAPI.Geometries.IGeometryFilter filter);
        void Apply(GeoAPI.Geometries.ICoordinateSequenceFilter filter);
        void Apply(GeoAPI.Geometries.ICoordinateFilter filter);
        byte[] AsBinary();
        string AsText();
        IGeometry Buffer(double distance);
        IGeometry Buffer(double distance, IBufferParameters bufferParameters);
        [Obsolete]
        IGeometry Buffer(double distance, BufferStyle endCapStyle);
        IGeometry Buffer(double distance, int quadrantSegments, EndCapStyle endCapStyle);
        [Obsolete]
        IGeometry Buffer(double distance, int quadrantSegments, BufferStyle endCapStyle);
        IGeometry Buffer(double distance, int quadrantSegments);
        bool Contains(IGeometry g);
        IGeometry ConvexHull();
        bool CoveredBy(IGeometry g);
        bool Covers(IGeometry g);
        bool Crosses(IGeometry g);
        IGeometry Difference(IGeometry other);
        bool Disjoint(IGeometry g);
        double Distance(IGeometry g);
        bool EqualsExact(IGeometry other, double tolerance);
        bool EqualsExact(IGeometry other);
        bool EqualsNormalized(IGeometry g);
        bool EqualsTopologically(IGeometry other);
        void GeometryChanged();
        void GeometryChangedAction();
        IGeometry GetGeometryN(int n);
        double[] GetOrdinates(GeoAPI.Geometries.Ordinate ordinate);
        IGeometry Intersection(IGeometry other);
        bool Intersects(IGeometry g);
        bool IsWithinDistance(IGeometry geom, double distance);
        void Normalize();
        IGeometry Normalized();
        bool Overlaps(IGeometry g);
        bool Relate(IGeometry g, string intersectionPattern);
        GeoAPI.Geometries.IntersectionMatrix Relate(IGeometry g);
        IGeometry Reverse();
        IGeometry SymmetricDifference(IGeometry other);
        bool Touches(IGeometry g);
        IGeometry Union();
        IGeometry Union(IGeometry other);
        bool Within(IGeometry g);
    }
}