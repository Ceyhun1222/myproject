using Aran.Geometries;
using System.Collections.Generic;

namespace Aran.Omega.TypeB   
{
    public static class ChainHullAlgorithm
    {
        private static double isLeft(Point P0, Point P1, Point P2)
        {
            // isLeft(): tests if a point is Left|On|Right of an infinite line.
            //    Input:  three points P0, P1, and P2
            //    Return: >0 for P2 left of the line through P0 and P1
            //            =0 for P2 on the line
            //            <0 for P2 right of the line
            //    See: the January 2001 Algorithm on Area of Triangles
            return (P1.X - P0.X) * (P2.Y - P0.Y) - (P2.X - P0.X) * (P1.Y - P0.Y);
        }

        public static Polygon ConvexHull(params Geometry[] geomCollection)
        {
            pointList = new List<Point>();
            foreach (Geometry geometry in geomCollection)
            {
                if (geometry != null)
                {
                    switch (geometry.Type)
                    {
                        case GeometryType.Point:
                            pointList.Add((Point)geometry);
                            break;

                        case GeometryType.MultiPoint:
                        case GeometryType.LineString:
                        case GeometryType.Ring:
                            AddListMultipoint((MultiPoint)geometry);
                            break;

                        case GeometryType.Polygon:
                            AddListPolygon((Polygon)geometry);
                            break;

                        case GeometryType.MultiPolygon:
                            MultiPolygon multiPolygon = (MultiPolygon)geometry;
                            foreach (var item in multiPolygon)
                            {
                                AddListPolygon((Polygon)item);
                            }
                            break;

                        case GeometryType.MultiLineString:
                            AddListPolyLine((MultiLineString)geometry);
                            break;

                        default:
                            break;
                    }
                }
            }

            return ConvexHull(pointList);
        }

        private static int Compare(Point x, Point y)
        {

            int res = 0;
            Point Pt1 = (Point)x;
            Point Pt2 = (Point)y;
            if (Pt1.X < Pt2.X) res = -1;
            if (Pt1.X == Pt2.X)
            {
                if (Pt1.Y < Pt2.Y) res = -1;
                if (Pt1.Y == Pt2.Y) res = 0;
                if (Pt1.Y > Pt2.Y) res = 1;
            }
            if (Pt1.X > Pt2.X) res = 1;

            return res;
        }

        private static Polygon ConvexHull(List<Point> P)
        {

            int n = P.Count;
            Point[] H = new Point[n + 1];
            int bot = 0, top = (-1);  // indices for bottom and top of the stack
            int i;                // array scan index
            int PntCnt;
            Polygon res = new Polygon();
            Ring rng = new Ring();
            P.Sort(Compare);

            // Get the indices of points with min x-coord and min|max y-coord
            int minmin = 0, minmax;
            double xmin = P[0].X;
            for (i = 1; i < n; i++)
                if (P[i].X != xmin) break;
            minmax = i - 1;
            if (minmax == n - 1)
            {       // degenerate case: all x-coords == xmin
                H[++top] = P[minmin];
                if (P[minmax].Y != P[minmin].Y) // a nontrivial segment
                    H[++top] = P[minmax];
                H[++top] = P[minmin];           // add polygon endpoint
                PntCnt = top + 1;
            }
            else
            {
                // Get the indices of points with max x-coord and min|max y-coord
                int maxmin, maxmax = n - 1;
                double xmax = P[n - 1].X;
                for (i = n - 2; i >= 0; i--)
                    if (P[i].X != xmax) break;
                maxmin = i + 1;

                // Compute the lower hull on the stack H
                H[++top] = P[minmin];      // push minmin point onto stack
                i = minmax;
                while (++i <= maxmin)
                {
                    // the lower line joins P[minmin] with P[maxmin]
                    if (isLeft(P[minmin], P[maxmin], P[i]) >= 0 && i < maxmin)
                        continue;          // ignore P[i] above or on the lower line

                    while (top > 0)        // there are at least 2 points on the stack
                    {
                        // test if P[i] is left of the line at the stack top
                        if (isLeft(H[top - 1], H[top], P[i]) > 0)
                            break;         // P[i] is a new hull vertex
                        else
                            top--;         // pop top point off stack
                    }
                    H[++top] = P[i];       // push P[i] onto stack
                }

                // Next, compute the upper hull on the stack H above the bottom hull
                if (maxmax != maxmin)      // if distinct xmax points
                    H[++top] = P[maxmax];  // push maxmax point onto stack
                bot = top;                 // the bottom point of the upper hull stack
                i = maxmin;
                while (--i >= minmax)
                {
                    // the upper line joins P[maxmax] with P[minmax]
                    if (isLeft(P[maxmax], P[minmax], P[i]) >= 0 && i > minmax)
                        continue;          // ignore P[i] below or on the upper line

                    while (top > bot)    // at least 2 points on the upper stack
                    {
                        // test if P[i] is left of the line at the stack top
                        if (isLeft(H[top - 1], H[top], P[i]) > 0)
                            break;         // P[i] is a new hull vertex
                        else
                            top--;         // pop top point off stack
                    }
                    H[++top] = P[i];       // push P[i] onto stack
                }
                if (minmax != minmin)
                    H[++top] = P[minmin];  // push joining endpoint onto stack

                PntCnt = top + 1;
            }


            for (int k = 0; k <= PntCnt - 1; k++)
            {
                rng.Add(H[k]);
            }

            res.ExteriorRing = rng;

            return res;

        }

        private static void AddListMultipoint(MultiPoint mp)
        {
            foreach (Point point in mp)
            {
                pointList.Add(point);
            }
        }

        private static void AddListPolygon(Polygon complexGeom)
        {
            AddListMultipoint(complexGeom.ToMultiPoint());
        }

        private static void AddListPolyLine(MultiLineString complexGeom)
        {
            AddListMultipoint(complexGeom.ToMultiPoint());
        }

        private static List<Point> pointList;
    }
}
