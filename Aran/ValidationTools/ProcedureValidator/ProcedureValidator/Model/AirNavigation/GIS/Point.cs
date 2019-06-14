using Aran.Aim.Features;

namespace PVT.Model
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Point()
        {

        }

        public Point(AixmPoint point)
        {
            X = point.Geo.X;
            Y = point.Geo.Y;
            Z = point.Geo.Z;
        }

        public Point(Aran.Geometries.Point point)
        {
            X = point.X;
            Y = point.Y;
            Z = point.Z;
        }

        public Aran.Geometries.Point ToAranPoint()
        {
            return new Aran.Geometries.Point(X, Y, Z);
        }
    }
    
}
