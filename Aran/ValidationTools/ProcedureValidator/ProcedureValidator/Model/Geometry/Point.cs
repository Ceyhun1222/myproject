using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace PVT.Model.Geometry
{
    public class Point2D:PointGeometry2D
    {
        public static Box GetBox(List<Point2D> points)
        {
            double maxX, minX;
            double maxY, minY;
            if (points.Count == 0)
                return null;

            maxX = minX = points[0].X;
            maxY = minY = points[0].Y;

            if (points.Count == 1)
                return new Box(new Point2D(maxX, maxY), new Point2D(maxX, maxY));

            for (var i = 1; i < points.Count; i++)
            {
                maxX = Math.Max(maxX, points[i].X);
                maxY = Math.Max(maxY, points[i].Y);

                minX = Math.Min(minX, points[i].X);
                minY = Math.Min(minY, points[i].Y);
            }

            return new Box(new Point2D(minX, minY), new Point2D(maxX, maxY));
        }
        public Point2D()
        {

        }

        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }



        public Point2D(Point3D point)
        {
            X = point.X;
            Y = point.Y;
        }

        public double X{ get; set; }
        public double Y { get; set; }

        public override List<Point2D> Points
        {
            get
            {
                return new List<Point2D>() { new Point2D(X, Y) };
            }
        }

        public override Geometry3D Transform()
        {
            return new Point3D(this);
        }

        public override System.Windows.Media.Geometry Convert()
        {
            var ellipseGeometry = new EllipseGeometry();
            ellipseGeometry.Center = new System.Windows.Point(X, Y);
            ellipseGeometry.RadiusX = 1;
            ellipseGeometry.RadiusY = 1;
            return ellipseGeometry;
        }
    }

    public class Point3D: PointGeometry3D
    {
        public static Box3D GetBox(List<Point3D> points)
        {
            double maxX, minX;
            double maxY, minY;
            double maxZ, minZ;
            if(points.Count == 0)
                return null;
             
            maxX = minX =points[0].X;
            maxY = minY =points[0].Y;
            maxZ = minZ = points[0].Z;

            if (points.Count == 1)
                return new Box3D(new Point3D(maxX,maxY, maxZ), new Point3D(maxX, maxY, maxZ));

            for (var i = 1; i < points.Count; i++)
            {
                maxX = Math.Max(maxX, points[i].X);
                maxY = Math.Max(maxY, points[i].Y);
                maxZ = Math.Max(maxZ, points[i].Z);

                minX = Math.Min(minX, points[i].X);
                minY = Math.Min(minY, points[i].Y);
                minZ = Math.Min(maxZ, points[i].Z);
            }

            return new Box3D(new Point3D(minX, minY, minZ), new Point3D(maxX, maxY, maxZ));
        }

        public Point3D()
        {

        }

        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point3D(Point2D point)
        {
            X = point.X;
            Y = point.Y;
            Z = 0;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public override List<Point3D> Points
        {
            get
            {
                return new List<Point3D>() { new Point3D(X, Y, Z) };
            }
        }

        public override Geometry2D Transform()
        {
            return new Point2D(this);
        }
    }
}
