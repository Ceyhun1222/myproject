using Aran.Geometries;
using System.Collections.Generic;

namespace Aran.PANDA.Conventional.Racetrack
{
    public static class ChainHullAlgorithm
    {
		
        private static double IsLeft( Point p0, Point p1, Point p2 )
        {
            // isLeft(): tests if a point is Left|On|Right of an infinite line.
            //    Input:  three points P0, P1, and P2
            //    Return: >0 for P2 left of the line through P0 and P1
            //            =0 for P2 on the line
            //            <0 for P2 right of the line
            //    See: the January 2001 Algorithm on Area of Triangles
            return (p1.X - p0.X) * (p2.Y - p0.Y) - (p2.X - p0.X) * (p1.Y - p0.Y);
        }

		public static Polygon ConvexHull(params Geometry[] geomCollection)
		{
			_pointList = new List<Point> ( );
			foreach (Geometry geometry in geomCollection)
			{
				if (geometry != null)
				{
					switch (geometry.Type)
					{
						case GeometryType.Point:
							_pointList.Add ( ( Point ) geometry );
							break;

						case GeometryType.MultiPoint:
						case GeometryType.LineString:
						case GeometryType.Ring:
							AddListMultipoint ( ( MultiPoint ) geometry );
							break;

						case GeometryType.Polygon:
							AddListPolygon ( ( Polygon ) geometry );
							break;

						case GeometryType.MultiPolygon:
							MultiPolygon multiPolygon = ( MultiPolygon ) geometry;
							foreach ( var item in multiPolygon )
							{
								AddListPolygon ( ( Polygon ) item );
							}
							break;

						case GeometryType.MultiLineString:
							AddListPolyLine((MultiLineString)geometry);
							break;
					}
				}
			}

			return ConvexHull(_pointList);
		}
		
		private static int Compare(Point x, Point y)
		{

			int res = 0;
			Point pt1 = x;
			Point pt2 = y;
			if (pt1.X < pt2.X) res = -1;
			if (pt1.X == pt2.X)
			{
				if (pt1.Y < pt2.Y) res = -1;
				if (pt1.Y == pt2.Y) res = 0;
				if (pt1.Y > pt2.Y) res = 1;
			}
			if (pt1.X > pt2.X) res = 1;

			return res;
		}
	
		private static Polygon ConvexHull(List<Point> p)
		{
			
			int n = p.Count;
			Point [] h = new Point [ n + 1 ];
			int top = (-1);  // indices for bottom and top of the stack
			int i;                // array scan index
			int pntCnt;
			Polygon res = new Polygon();
			Ring rng = new Ring();
			p.Sort(Compare);
					
			// Get the indices of points with min x-coord and min|max y-coord
			int minmin = 0;
			double xmin = p[0].X;
			for (i = 1; i < n; i++)
				if (p[i].X != xmin) break;
			var minmax = i - 1;
			if (minmax == n - 1)
			{       // degenerate case: all x-coords == xmin
				h[++top] = p[minmin];
				if (p[minmax].Y != p[minmin].Y) // a nontrivial segment
					h[++top] = p[minmax];
				h[++top] = p[minmin];           // add polygon endpoint
				pntCnt = top + 1;
			}
			else
			{
				// Get the indices of points with max x-coord and min|max y-coord
				int maxmax = n - 1;
				double xmax = p[n - 1].X;
				for (i = n - 2; i >= 0; i--)
					if (p[i].X != xmax) break;
				var maxmin = i + 1;

				// Compute the lower hull on the stack H
				h[++top] = p[minmin];      // push minmin point onto stack
				i = minmax;
				while (++i <= maxmin)
				{
					// the lower line joins P[minmin] with P[maxmin]
					if (IsLeft(p[minmin], p[maxmin], p[i]) >= 0 && i < maxmin)
						continue;          // ignore P[i] above or on the lower line

					while (top > 0)        // there are at least 2 points on the stack
					{
						// test if P[i] is left of the line at the stack top
						if (IsLeft(h[top - 1], h[top], p[i]) > 0)
							break;         // P[i] is a new hull vertex
						else
							top--;         // pop top point off stack
					}
					h[++top] = p[i];       // push P[i] onto stack
				}

				// Next, compute the upper hull on the stack H above the bottom hull
				if (maxmax != maxmin)      // if distinct xmax points
					h[++top] = p[maxmax];  // push maxmax point onto stack
				var bot = top;  // indices for bottom and top of the stack
				i = maxmin;
				while (--i >= minmax)
				{
					// the upper line joins P[maxmax] with P[minmax]
					if (IsLeft(p[maxmax], p[minmax], p[i]) >= 0 && i > minmax)
						continue;          // ignore P[i] below or on the upper line

					while (top > bot)    // at least 2 points on the upper stack
					{
						// test if P[i] is left of the line at the stack top
						if (IsLeft(h[top - 1], h[top], p[i]) > 0)
							break;         // P[i] is a new hull vertex
						else
							top--;         // pop top point off stack
					}
					h[++top] = p[i];       // push P[i] onto stack
				}
				if (minmax != minmin)
					h[++top] = p[minmin];  // push joining endpoint onto stack

				pntCnt = top + 1;
			}


			for ( int k = 0; k <= pntCnt - 1; k++ )
			{
				rng.Add ( h [ k ] );
			}

			res.ExteriorRing = rng;

			return res;

		}

		private static void AddListMultipoint(MultiPoint mp)
		{
			foreach ( Point point in mp )
			{
				_pointList.Add ( point );
			}
		}

		private static void AddListPolygon(Polygon complexGeom)
		{
			AddListMultipoint ( complexGeom.ToMultiPoint ( ) );
		}

		private static void AddListPolyLine(MultiLineString complexGeom)
		{
			AddListMultipoint ( complexGeom.ToMultiPoint ( ) );
		}
	

		private static List<Point> _pointList;
    }
}
