using System.Collections.Generic;
using System.Windows.Media;

namespace PVT.Model.Geometry
{
    public class Line2D: PointGeometry2D
    {
        public Line2D()
        {

        }

        private Line2D(Line3D line)
        {
            Start = new Point2D(line.Start);
            End = new Point2D(line.End);
        }

        public Point2D Start { get; set; }
        public Point2D End { get; set; }

        public override List<Point2D> Points
        {
            get
            {
                return new List<Point2D>() { new Point2D(Start.X, Start.Y), new Point2D(End.X, End.Y) };
            }
        }

        public override Geometry3D Transform()
        {
            return new Line3D{ Start = (Point3D)Start.Transform(), End = (Point3D)End.Transform()};
        }

        public override System.Windows.Media.Geometry Convert()
        {
            var line = new LineGeometry();
            line.StartPoint = new System.Windows.Point(Start.X, Start.Y);
            line.EndPoint = new System.Windows.Point(End.X, End.Y);
            return line;
        }
    }

    public class Line3D: PointGeometry3D
    {
        public Line3D()
        {

        }

        public Line3D(Line2D line)
        {
            Start = new Point3D(line.Start);
            End = new Point3D(line.End);
        }

        public Point3D Start { get; set; }
        public Point3D End { get; set; }

        public override List<Point3D> Points
        {
            get
            {
                return new List<Point3D>() { new Point3D(Start.X, Start.Y, Start.Z), new Point3D(End.X, End.Y, End.Z) };
            }
        }

        public override Geometry2D Transform()
        {
            return new Line2D { Start = (Point2D)Start.Transform(), End = (Point2D)End.Transform() };
        }
    }
}
