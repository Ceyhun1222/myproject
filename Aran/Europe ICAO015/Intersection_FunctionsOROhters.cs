using Aran.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAO015
{
    public class Intersection_FunctionsOROhters
    {
        public Aran.Geometries.Point ClosestIntersection(double cx, double cy, double radius, Aran.Geometries.Point lineStart, Aran.Geometries.Point lineEnd)
        {
            Aran.Geometries.Point intersection1;
            Aran.Geometries.Point intersection2;
            int intersections = FindLineCircleIntersections(cx, cy, radius, lineStart, lineEnd, out intersection1, out intersection2);

            if (intersections == 1)
                return intersection1;//one intersection

            if (intersections == 2)
            {
                double dist1 = Distance(intersection1, lineStart);
                double dist2 = Distance(intersection2, lineStart);

                if (dist1 < dist2)
                    return intersection1;
                else
                    return intersection2;
            }

            return intersection1;// no intersections at all
        }

        private double Distance(Aran.Geometries.Point p1, Aran.Geometries.Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        // Find the points of intersection.
        private int FindLineCircleIntersections(double cx, double cy, double radius,
            Aran.Geometries.Point point1, Aran.Geometries.Point point2, out Aran.Geometries.Point intersection1, out Aran.Geometries.Point intersection2)
        {
            double dx, dy, A, B, C, det, t;

            dx = point2.X - point1.X;
            dy = point2.Y - point1.Y;

            A = dx * dx + dy * dy;
            B = 2 * (dx * (point1.X - cx) + dy * (point1.Y - cy));
            C = (point1.X - cx) * (point1.X - cx) + (point1.Y - cy) * (point1.Y - cy) - radius * radius;

            det = B * B - 4 * A * C;
            if ((A <= 0.0000001) || (det < 0))
            {
                // No real solutions.
                intersection1 = new Aran.Geometries.Point(float.NaN, float.NaN);
                intersection2 = new Aran.Geometries.Point(float.NaN, float.NaN);
                return 0;
            }
            else if (det == 0)
            {
                // One solution.
                t = -B / (2 * A);
                intersection1 = new Aran.Geometries.Point(point1.X + t * dx, point1.Y + t * dy);
                intersection2 = new Aran.Geometries.Point(float.NaN, float.NaN);
                return 1;
            }
            else
            {
                // Two solutions.
                t = (float)((-B + Math.Sqrt(det)) / (2 * A));
                intersection1 = new Aran.Geometries.Point(point1.X + t * dx, point1.Y + t * dy);
                t = (float)((-B - Math.Sqrt(det)) / (2 * A));
                intersection2 = new Aran.Geometries.Point(point1.X + t * dx, point1.Y + t * dy);
                return 2;
            }
        }
        public static Aran.Geometries.Point CircleVectorIntersect(Aran.Geometries.Point centerPoint, double radius, Aran.Geometries.Point ptVector, double dirVector)
        {

            double d;

            double v1x = Math.Cos(dirVector);
            double v1y = Math.Sin(dirVector);

            double v2x = centerPoint.X - ptVector.X;
            double v2y = centerPoint.Y - ptVector.Y;

            double distToIntersect = v1x * v2x + v1y * v2y;

            Aran.Geometries.Point ptIntersect = new Aran.Geometries.Point(ptVector.X + v1x * distToIntersect, ptVector.Y + v1y * distToIntersect);

            double dx = centerPoint.X - ptIntersect.X;
            double dy = centerPoint.Y - ptIntersect.Y;

            double distCenterToIntersect2 = dx * dx + dy * dy;
            double radius2 = radius * radius;

            if (distCenterToIntersect2 < radius2)
            {
                d = Math.Sqrt(radius2 - distCenterToIntersect2);

                double xNew = ptVector.X + (d + distToIntersect) * v1x;
                double yNew = ptVector.Y + (d + distToIntersect) * v1y;
                return new Aran.Geometries.Point(xNew, yNew);

                //return LocalToPrj(ptIntersect, dirVector, d, 0);
            }

            d = -1.0;
            return new Aran.Geometries.Point(); // null;
        }
        public Aran.Geometries.Point GetLinesIntersection(Aran.Geometries.Point line1P1, Aran.Geometries.Point line1P2, Aran.Geometries.Point line2P1, Aran.Geometries.Point line2P2)
        {
            //Line 1 (p1, p2)  
            var A1 = line1P2.Y - line1P1.Y;
            var B1 = line1P1.X - line1P2.X;
            var C1 = A1 * line1P1.X + B1 * line1P1.Y;

            //Line 2 (p3,  p4)  
            var A2 = line2P2.Y - line2P1.Y;
            var B2 = line2P1.X - line2P2.X;
            var C2 = A2 * line2P1.X + B2 * line2P1.Y;

            var determinate = A1 * B2 - A2 * B1;

            Aran.Geometries.Point intersectionPoint;
            if (determinate != 0)
            {
                double x = (B2 * C1 - B1 * C2) / determinate;
                double y = (A1 * C2 - A2 * C1) / determinate;
                intersectionPoint = new Aran.Geometries.Point();
                intersectionPoint.X = x;
                intersectionPoint.Y = y;
            }
            else //lines are parrallel  
                intersectionPoint = null;

            return intersectionPoint;
        }
        public static Geometry CustomLineLineIntersect(Aran.Geometries.Line line1, Aran.Geometries.Line line2)
        {
            const double Eps = 0.0001;

            double cosF1 = line1.DirVector.GetComponent(0);
            double sinF1 = line1.DirVector.GetComponent(1);

            double cosF2 = line2.DirVector.GetComponent(0);
            double sinF2 = line2.DirVector.GetComponent(1);

            double d = sinF2 * cosF1 - cosF2 * sinF1;

            double Ua = cosF2 * (line1.RefPoint.Y - line2.RefPoint.Y) -
                  sinF2 * (line1.RefPoint.X - line2.RefPoint.X);

            double Ub = cosF1 * (line1.RefPoint.Y - line2.RefPoint.Y) -
                  sinF1 * (line1.RefPoint.X - line2.RefPoint.X);

            if (Math.Abs(d) < 10e-7)
            {
                if (System.Math.Abs(Ua) + System.Math.Abs(Ub) < 2.0 * Eps)
                    return (Geometry)line1.Clone();

                return null;
            }

            double k = Ua / d;
            return new Aran.Geometries.Point(line1.RefPoint.X + k * cosF1, line1.RefPoint.Y + k * sinF1);

        }
    }
}
