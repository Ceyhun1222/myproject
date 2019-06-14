using System;

using Aran.AranEnvironment;
using Aran.AranEnvironment.Symbols;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.PANDA.Common;

//using Aran.PANDA.RNAV.SGBAS.Properties;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Aran.PANDA.Constants;


namespace Aran.PANDA.RNAV.SGBAS
{
	[System.Runtime.InteropServices.ComVisible(false)]
	class minCircle
	{
		static bool circlePPP(Point a, Point b, Point c, out Point o, out double r)
		{
			Point ba = new Point(b.X - a.X, b.Y - a.Y);
			Point ca = new Point(c.X - a.X, c.Y - a.Y);
			double p = ba.X * ca.Y - ba.Y * ca.X;

			o = new Point();

			if (p == 0)
			{
				r = 0;
				return false;
			}

			p += p;
			double a2 = a.X * a.X + a.Y * a.Y;
			double b2 = b.X * b.X + b.Y * b.Y - a2;
			double c2 = c.X * c.X + c.Y * c.Y - a2;

			o.X = (b2 * ca.Y - c2 * ba.Y) / p;
			o.Y = (c2 * ba.X - b2 * ca.X) / p;
			r = ARANMath.Hypot(a.X - o.X, a.Y - o.Y);
			return true;
		}

		public static Ring circlePPP(Point a, Point b, Point c)
		{
			Point ba = new Point(b.X - a.X, b.Y - a.Y);
			Point ca = new Point(c.X - a.X, c.Y - a.Y);
			double p = ba.X * ca.Y - ba.Y * ca.X;

			if (p == 0)
				return new Ring();

			p += p;
			double a2 = a.X * a.X + a.Y * a.Y;
			double b2 = b.X * b.X + b.Y * b.Y - a2;
			double c2 = c.X * c.X + c.Y * c.Y - a2;

			Point o = new Point();
			o.X = (b2 * ca.Y - c2 * ba.Y) / p;
			o.Y = (c2 * ba.X - b2 * ca.X) / p;

			double r = ARANMath.Hypot(a.X - o.X, a.Y - o.Y);
			return ARANFunctions.CreateCirclePrj(o, r);
		}

		public static bool minCircleAroundPoints(List<Point> P, out Point o, out double r)
		{
			int n = P.Count;

			if (n < 3)
			{
				r = 0.0;
				if (n == 0)
				{
					o = null;
					return false;
				}

				if (n == 1)
				{
					o = (Point)P[0].Clone();
					return true;
				}

				o = new Point(0.5 * (P[0].X + P[1].X), 0.5 * (P[0].Y + P[1].Y));
				r = ARANMath.Hypot(P[0].X - P[1].X, P[0].Y - P[1].Y);
				return true;
			}

			int i, im = 0;

			double max = 0, t;
			Point p0 = P[0];

			for (i = 1; i < n; ++i)
			{
				t = ARANMath.Hypot(P[i].X - p0.X, P[i].Y - p0.Y);

				if (max < t)
				{
					max = t;
					im = i;
				}
			}

			if (im == 0)
			{
				o = p0;
				r = 0;
				return true;
			}

			int np = 2;
			int[] ip = new int[3];

			ip[0] = 0;
			ip[1] = im;

			o = new Point();
			o.X = 0.5 * (p0.X + P[im].X);
			o.Y = 0.5 * (p0.Y + P[im].Y);

			double q = 0.25 * max, s;

			for (; ; )
			{
				max = 0.0;
				for (i = 0; i < n; ++i)
				{
					t = ARANMath.Hypot(P[i].X - o.X, P[i].Y - o.Y);

					if (max < t)
					{
						max = t;
						im = i;
					}
				}

				if (max <= q || im == ip[0] || im == ip[1])
					break;

				Point pm = P[im];
				int km = 0;

				s = ARANMath.Hypot(pm.X - P[ip[0]].X, pm.Y - P[ip[0]].Y);
				t = ARANMath.Hypot(pm.X - P[ip[1]].X, pm.Y - P[ip[1]].Y);

				if (s < t)
				{
					s = t;
					km = 1;
				}

				Point v = new Point();
				if (np == 2)
				{
					s *= 0.25;
					int iTmp = ip[km];
					ip[km] = ip[0];
					ip[0] = iTmp;

					v.X = 0.5 * (pm.X + P[ip[0]].X);
					v.Y = 0.5 * (pm.Y + P[ip[0]].Y);

					if (ARANMath.Hypot(v.X - P[ip[1]].X, v.Y - P[ip[1]].Y) > s)
					{
						np = 3;
						ip[2] = im;
						circlePPP(P[ip[0]], P[ip[1]], pm, out v, out s);
					}
					else
					{
						ip[1] = im;
					}
				}
				else
				{
					if (im == ip[2])
						break;
					t = ARANMath.Hypot(pm.X - P[ip[2]].X, pm.Y - P[ip[2]].Y);

					if (s < t)
					{
						s = t;
						km = 2;
					}

					s *= 0.25;
					int iTmp = ip[km];
					ip[km] = ip[0];
					ip[0] = iTmp;

					v.X = 0.5 * (pm.X + P[ip[0]].X);
					v.Y = 0.5 * (pm.Y + P[ip[0]].Y);

					double q1 = ARANMath.Hypot(v.X - P[ip[1]].X, v.Y - P[ip[1]].Y);
					double q2 = ARANMath.Hypot(v.X - P[ip[2]].X, v.Y - P[ip[2]].Y);
					if (q1 < q2)
					{
						ip[1] = ip[2];
						q1 = q2;
					}

					if (q1 > s)
					{
						circlePPP(P[ip[0]], P[ip[1]], pm, out v, out s);
						ip[2] = im;
					}
					else
					{
						np = 2;
						ip[1] = im;
					}
				}

				if (s <= q)
					break;

				q = s;
				o = v;
			}

			r = 2 * q;			//r = Math.Sqrt(q);
			return true;
		}

		public static Ring minCircleAroundPoints(List<Point> P)
		{
			int n = P.Count;

			if (n < 3)
			{
				if (n == 0) return new Ring();
				if (n == 1) return ARANFunctions.CreateCirclePrj(P[0], 0);
				Point ptCenter = new Point(0.5 * (P[0].X + P[1].X), 0.5 * (P[0].Y + P[1].Y));

				return ARANFunctions.CreateCirclePrj(ptCenter, ARANMath.Hypot(P[0].X - P[1].X, P[0].Y - P[1].Y));
			}

			Point p0 = P[0];
			int i, im = 0;
			double max = 0, t;

			for (i = 1; i < n; ++i)
			{
				Point pI = P[i];
				t = ARANMath.Hypot(pI.X - p0.X, pI.Y - p0.Y); // qmod ( pT );

				if (max < t)
				{
					max = t;
					im = i;
				}
			}

			if (im == 0)
				return ARANFunctions.CreateCirclePrj(p0, 0);

			int np = 2;
			int[] ip = new int[3];

			ip[0] = 0;
			ip[1] = im;

			Point o = new Point();
			o.X = 0.5 * (p0.X + P[im].X);
			o.Y = 0.5 * (p0.Y + P[im].Y);

			double q = 0.25 * max, s;

			for (; ; )
			{
				max = 0;
				for (i = 0; i < n; ++i)
				{
					Point pI = P[i];
					t = ARANMath.Hypot(pI.X - o.X, pI.Y - o.Y);

					if (max < t)
					{
						max = t;
						im = i;
					}
				}

				if (max <= q || im == ip[0] || im == ip[1])
					break;

				Point pm = P[im];
				int km = 0;

				s = ARANMath.Hypot(pm.X - P[ip[0]].X, pm.Y - P[ip[0]].Y);
				t = ARANMath.Hypot(pm.X - P[ip[1]].X, pm.Y - P[ip[1]].Y);
				if (s < t)
				{
					s = t;
					km = 1;
				}

				Point v = new Point();
				if (np == 2)
				{
					s *= 0.25;
					int iTmp = ip[km];
					ip[km] = ip[0];
					ip[0] = iTmp;

					v.X = 0.5 * (pm.X + P[ip[0]].X);
					v.Y = 0.5 * (pm.Y + P[ip[0]].Y);

					if (ARANMath.Hypot(v.X - P[ip[1]].X, v.Y - P[ip[1]].Y) > s)
					{
						np = 3;
						ip[2] = im;
						circlePPP(P[ip[0]], P[ip[1]], pm, out v, out s);
					}
					else
					{
						ip[1] = im;
					}
				}
				else
				{
					if (im == ip[2]) break;
					t = ARANMath.Hypot(pm.X - P[ip[2]].X, pm.Y - P[ip[2]].Y);

					if (s < t)
					{
						s = t;
						km = 2;
					}

					s *= 0.25;
					int iTmp = ip[km];
					ip[km] = ip[0];
					ip[0] = iTmp;

					v.X = 0.5 * (pm.X + P[ip[0]].X);
					v.Y = 0.5 * (pm.Y + P[ip[0]].Y);

					double q1 = ARANMath.Hypot(v.X - P[ip[1]].X, v.Y - P[ip[1]].Y);
					double q2 = ARANMath.Hypot(v.X - P[ip[2]].X, v.Y - P[ip[2]].Y);
					if (q1 < q2)
					{
						ip[1] = ip[2];
						q1 = q2;
					}

					if (q1 > s)
					{
						circlePPP(P[ip[0]], P[ip[1]], pm, out v, out s);
						ip[2] = im;
					}
					else
					{
						np = 2;
						ip[1] = im;
					}
				}
				if (s <= q) break;
				q = s;
				o = v;
			}

			return ARANFunctions.CreateCirclePrj(o, Math.Sqrt(q));
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	class ConvexHull
	{
		// 2D cross product of OA and OB vectors, i.e. z-component of their 3D cross product.
		// Returns a positive value, if OAB makes a counter-clockwise turn,
		// negative for clockwise turn, and zero if the points are collinear.
		static double cross(Point O, Point A, Point B)
		{
			return (A.X - O.X) * (B.Y - O.Y) - (A.Y - O.Y) * (B.X - O.X);
		}

		static int ComparePointsByLexi(Point p1, Point p2)
		{
			if (p1.X < p2.X)
				return -1;
			if (p1.X > p2.X)
				return 1;

			if (p1.Y < p2.Y)
				return -1;
			if (p1.Y > p2.Y)
				return 1;

			return 0;
		}

		// Returns a list of points on the convex hull in counter-clockwise order.
		// Note: the last point in the returned list is the same as the first one.
		public static Ring CalcConvexHull(List<Point> P)
		{
			// Sort points lexicographically
			P.Sort(ComparePointsByLexi);

			Ring H = new Ring();
			int n = P.Count, k = 0;

			// Build lower hull
			for (int i = 0; i < n; i++)
			{
				while (k >= 2 && cross(H[k - 2], H[k - 1], P[i]) <= 0)
				{
					H.Remove(k);
					k--;
				}
				H.Add(P[i]);
				k++;
			}

			// Build upper hull
			for (int i = n - 2, t = k + 1; i >= 0; i--)
			{
				while (k >= t && cross(H[k - 2], H[k - 1], P[i]) <= 0)
				{
					H.Remove(k);
					k--;
				}
				H.Add(P[i]);
				k++;
			}

			return H;
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public static class ArrayExt
	{
		public static T[,] ResizeArray<T>(ref T[,] original, int newCols, int newRows)
		{
			T[,] newArray = new T[newCols, newRows];

			int oldCoNum = original.GetLength(0);
			int oldRoNum = original.GetLength(1);

			int minCols = Math.Min(oldCoNum, newCols);
			int minRows = Math.Min(oldRoNum, newRows);

			for (int i = 0; i < minCols; i++)
				System.Array.Copy(original, i * oldRoNum, newArray, i * newRows, minRows);

			original = newArray;
			return newArray;
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public static class Functions
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

		[DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
		public static extern IntPtr GetWindow(IntPtr hWnd, int uCmd);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIDNewItem, string lpNewItem);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool InsertMenu(IntPtr hMenu, int uPosition, int uFlags, int uIDNewItem, string lpNewItem);

		public static DateTime RetrieveLinkerTimestamp()
		{
			const int c_PeHeaderOffset = 60;
			const int c_LinkerTimestampOffset = 8;

			byte[] b = new byte[2048];
			System.IO.Stream s = null;

			try
			{
				string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;
				s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
				s.Read(b, 0, 2048);
			}
			finally
			{
				if (s != null)
					s.Close();
			}

			int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
			int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);

			DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

			dt = dt.AddSeconds(secondsSince1970);
			dt = dt.ToLocalTime();
			return dt;
		}

		//private static void QuickSort(Obstacle[] obstacleArray, int iLo, int iHi)
		//{
		//	int Lo = iLo;
		//	int Hi = iHi;
		//	double midDist = obstacleArray[(Lo + Hi) / 2].X;

		//	do
		//	{
		//		while (obstacleArray[Lo].X < midDist)
		//			Lo++;

		//		while (obstacleArray[Hi].X > midDist)
		//			Hi--;

		//		if (Lo <= Hi)
		//		{
		//			Obstacle t = obstacleArray[Lo];
		//			obstacleArray[Lo] = obstacleArray[Hi];
		//			obstacleArray[Hi] = t;
		//			Lo++;
		//			Hi--;
		//		}
		//	}
		//	while (Lo <= Hi);

		//	if (Hi > iLo)
		//		QuickSort(obstacleArray, iLo, Hi);

		//	if (Lo < iHi)
		//		QuickSort(obstacleArray, Lo, iHi);
		//}

		//public static void SortByDist(Obstacle[] obstacleArray)
		//{
		//	int Lo = obstacleArray.GetLowerBound(0);
		//	int Hi = obstacleArray.GetUpperBound(0);

		//	if (Lo >= Hi)
		//		return;

		//	QuickSort(obstacleArray, Lo, Hi);
		//}

		//public static bool PriorPostFixTolerance(Polygon pPolygon, Point pPtPrj, double fDir, out double PriorDist, out double PostDist)
		//{
		//	PriorDist = -1.0;
		//	PostDist = -1.0;
		//	MultiLineString ptIntersection;

		//	LineString pCutterPolyline = new LineString();
		//	pCutterPolyline.Add(ARANFunctions.LocalToPrj(pPtPrj, fDir, 1000000.0, 0));
		//	pCutterPolyline.Add(ARANFunctions.LocalToPrj(pPtPrj, fDir, -1000000.0, 0));

		//	try
		//	{
		//		GeometryOperators pTopological = new GeometryOperators();
		//		Geometry pIntersection = pTopological.Intersect(pCutterPolyline, pPolygon);
		//		if (pIntersection.IsEmpty)
		//			return false;
		//		ptIntersection = (MultiLineString)pIntersection;
		//	}
		//	catch
		//	{
		//		return false;
		//	}

		//	Point ptDist = ARANFunctions.PrjToLocal(pPtPrj, fDir, ptIntersection[0][0]);

		//	double fMinDist = ptDist.X;
		//	double fMaxDist = ptDist.X;
		//	int n = ptIntersection.Count;

		//	for (int j = 0; j < n; j++)
		//	{
		//		LineString ls = ptIntersection[j];
		//		int m = ls.Count;

		//		for (int i = 0; i < m; i++)
		//		{
		//			ptDist = ARANFunctions.PrjToLocal(pPtPrj, fDir, ls[i]);

		//			if (ptDist.X < fMinDist) fMinDist = ptDist.X;
		//			if (ptDist.X > fMaxDist) fMaxDist = ptDist.X;
		//		}
		//	}
		//	PriorDist = fMinDist;
		//	PostDist = fMaxDist;

		//	return true;
		//}

		//public static void GetStrightAreaObstacles(Obstacle[] LocalObstacles, out Obstacle[] innerObstacleList,
		//	out Obstacle[] outerObstacleList, MultiPolygon pFullPoly, MultiPolygon pPrimaryPoly, RWYType Rwy, double depDir)
		//{
		//	int n = LocalObstacles.Length;
		//	innerObstacleList = new Obstacle[n];
		//	outerObstacleList = new Obstacle[n];

		//	if (n == 0)
		//		return;

		//	int j = -1, k = -1;

		//	GeometryOperators fullGeoOp = new GeometryOperators();
		//	fullGeoOp.CurrentGeometry = pFullPoly;

		//	GeometryOperators primaryGeoOp = new GeometryOperators();
		//	primaryGeoOp.CurrentGeometry = pPrimaryPoly;

		//	GeometryOperators lineStrGeoOp = new GeometryOperators();
		//	lineStrGeoOp.CurrentGeometry = ARANFunctions.PolygonToPolyLine(pFullPoly[0]);

		//	Point PtEnd = Rwy.pPtPrj[eRWY.ptDER];

		//	for (int i = 0; i < n; i++)
		//	{
		//		Point ptCurr = (Point)LocalObstacles[i].GeompPrj;

		//		ARANFunctions.PrjToLocal(PtEnd, depDir, ptCurr, out LocalObstacles[i].X, out LocalObstacles[i].Y);

		//		int sign = Math.Sign(LocalObstacles[i].X);
		//		LocalObstacles[i].Xmin = Math.Abs(LocalObstacles[i].X) - LocalObstacles[i].HorAccuracy;
		//		if (LocalObstacles[i].Xmin < 0.0)
		//			LocalObstacles[i].Xmin = 0.0;
		//		LocalObstacles[i].Xmin *= sign;

		//		sign = Math.Sign(LocalObstacles[i].Y);
		//		LocalObstacles[i].Ymin = Math.Abs(LocalObstacles[i].Y) - LocalObstacles[i].HorAccuracy;
		//		if (LocalObstacles[i].Ymin < 0.0)
		//			LocalObstacles[i].Ymin = 0.0;
		//		LocalObstacles[i].Ymin *= sign;

		//		double distToFullPoly = fullGeoOp.GetDistance(ptCurr);

		//		if (distToFullPoly <= LocalObstacles[i].HorAccuracy) //if (pFullPoly.IsPointInside(ptCurr))
		//		{
		//			innerObstacleList[++j] = LocalObstacles[i];

		//			double distToPrimaryPoly = primaryGeoOp.GetDistance(ptCurr);
		//			innerObstacleList[j].Prima = distToPrimaryPoly <= innerObstacleList[j].HorAccuracy;//pPrimaryPoly.IsPointInside(ptCurr);
		//			innerObstacleList[j].fTmp = 1.0;

		//			if (!innerObstacleList[j].Prima)
		//			{
		//				//double d0 = pGeoOp.GetDistance(pPrimaryPoly, ptCurr);
		//				double d1 = lineStrGeoOp.GetDistance(ptCurr);
		//				double d = distToPrimaryPoly + d1;
		//				innerObstacleList[j].fTmp = d1 / d;
		//			}
		//		}
		//		else
		//		{
		//			outerObstacleList[++k] = LocalObstacles[i];

		//			Point ptTmp = fullGeoOp.GetNearestPoint(ptCurr);

		//			outerObstacleList[k].DistStar = ARANFunctions.PointToLineDistance(ptTmp, PtEnd, depDir + ARANMath.C_PI_2);

		//			outerObstacleList[k].d0 = distToFullPoly - outerObstacleList[k].HorAccuracy;
		//			if (outerObstacleList[k].d0 <= 0.0)
		//				outerObstacleList[k].d0 = 0.0;
		//		}
		//	}

		//	System.Array.Resize<ObstacleType>(ref innerObstacleList, j + 1);
		//	System.Array.Resize<ObstacleType>(ref outerObstacleList, k + 1);
		//}

		//public static void GetObstaclesByDistance(ObstacleContainer ObstacleList, out ObstacleContainer OutObstacleList, Point ptCenter, double radius)
		//{
		//	int m = ObstacleList.Obstacles.Length;
		//	int n = ObstacleList.Parts.Length;

		//	OutObstacleList.Obstacles = new Obstacle[m];
		//	OutObstacleList.Parts = new ObstacleData[n];

		//	if (n > 0 && m > 0)
		//	{
		//		OutObstacleList.Obstacles = new Obstacle[m];
		//		OutObstacleList.Parts = new ObstacleData[n];
		//	}
		//	else
		//	{
		//		OutObstacleList.Obstacles = new Obstacle[0];
		//		OutObstacleList.Parts = new ObstacleData[0];
		//		return;
		//	}

		//	for (int i = 0; i < m; i++)
		//		ObstacleList.Obstacles[i].NIx = -1;

		//	int k = 0, l = 0;

		//	for (int i = 0; i < n; i++)
		//	{
		//		double dist = ARANFunctions.ReturnDistanceInMeters(ptCenter, ObstacleList.Parts[i].pPtPrj);
		//		if (dist > radius)
		//			continue;

		//		OutObstacleList.Parts[k] = ObstacleList.Parts[i];

		//		if (ObstacleList.Obstacles[ObstacleList.Parts[i].Owner].NIx < 0)
		//		{
		//			OutObstacleList.Obstacles[l] = ObstacleList.Obstacles[ObstacleList.Parts[i].Owner];
		//			OutObstacleList.Obstacles[l].PartsNum = 0;
		//			OutObstacleList.Obstacles[l].Parts = new int[ObstacleList.Obstacles[ObstacleList.Parts[i].Owner].PartsNum];
		//			ObstacleList.Obstacles[ObstacleList.Parts[i].Owner].NIx = l;
		//			l++;
		//		}

		//		OutObstacleList.Parts[k].Owner = ObstacleList.Obstacles[ObstacleList.Parts[i].Owner].NIx;
		//		OutObstacleList.Parts[k].Index = OutObstacleList.Obstacles[OutObstacleList.Parts[k].Owner].PartsNum;

		//		OutObstacleList.Obstacles[OutObstacleList.Parts[k].Owner].Parts[OutObstacleList.Obstacles[OutObstacleList.Parts[k].Owner].PartsNum] = k;
		//		OutObstacleList.Obstacles[OutObstacleList.Parts[k].Owner].PartsNum++;

		//		k++;
		//	}

		//	if (k > 0)
		//	{
		//		Array.Resize<Obstacle>(ref OutObstacleList.Obstacles, l);
		//		Array.Resize<ObstacleData>(ref OutObstacleList.Parts, k);
		//	}
		//	else
		//	{
		//		OutObstacleList.Obstacles = new Obstacle[0];
		//		OutObstacleList.Parts = new ObstacleData[0];
		//	}
		//}

		public static int GetObstaclesByPolygon(ObstacleContainer ObstacleList, out ObstacleContainer OutObstacleList, MultiPolygon pPoly)
		{
			int m = ObstacleList.Obstacles.Length;
			int n = ObstacleList.Parts.Length;

			if (m == 0 ||n ==0 || pPoly.IsEmpty)
			{
				OutObstacleList.Obstacles = new Obstacle[0];
				OutObstacleList.Parts = new ObstacleData[0];
				return 0;
			}

			int c = Math.Max(n, m);
			OutObstacleList.Obstacles = new Obstacle[m];
			OutObstacleList.Parts = new ObstacleData[c];

			for (int i = 0; i < m; i++)
				ObstacleList.Obstacles[i].NIx = -1;

			int k = 0, l = 0;

			for (int i = 0; i < n; i++)
			{
				Point ptCurr = ObstacleList.Parts[i].pPtPrj;

				if (!pPoly.IsPointInside(ptCurr))
					continue;

				OutObstacleList.Parts[k] = ObstacleList.Parts[i];

				if (ObstacleList.Obstacles[ObstacleList.Parts[i].Owner].NIx < 0)
				{
					OutObstacleList.Obstacles[l] = ObstacleList.Obstacles[ObstacleList.Parts[i].Owner];
					OutObstacleList.Obstacles[l].PartsNum = 0;
					//OutObstacleList.Obstacles[l].Parts = new int[ObstacleList.Obstacles[ObstacleList.Parts[i].Owner].PartsNum];
					ObstacleList.Obstacles[ObstacleList.Parts[i].Owner].NIx = l;
					l++;
				}

				OutObstacleList.Parts[k].Owner = ObstacleList.Obstacles[ObstacleList.Parts[i].Owner].NIx;
				OutObstacleList.Parts[k].Index = OutObstacleList.Obstacles[OutObstacleList.Parts[k].Owner].PartsNum;

				//OutObstacleList.Obstacles[OutObstacleList.Parts[k].Owner].Parts[OutObstacleList.Parts[k].Index].PartsNum] = k;
				OutObstacleList.Obstacles[OutObstacleList.Parts[k].Owner].PartsNum++;

				k++;
				if (k >= c)
				{
					c += n;
					Array.Resize<ObstacleData>(ref OutObstacleList.Parts, c);
				}
			}

			if (k > 0)
			{
				Array.Resize<Obstacle>(ref OutObstacleList.Obstacles, l);
				Array.Resize<ObstacleData>(ref OutObstacleList.Parts, k);
			}
			else
			{
				OutObstacleList.Obstacles = new Obstacle[0];
				OutObstacleList.Parts = new ObstacleData[0];
			}
			return k;
		}

		public static int GetObstaclesByPolygonWithDecomposition(ObstacleContainer ObstacleList, out ObstacleContainer OutObstList, MultiPolygon pPlane)
		{
			int m = ObstacleList.Obstacles.Length;

			if (m == 0 || pPlane.IsEmpty)
			{
				OutObstList.Obstacles = new Obstacle[0];
				OutObstList.Parts = new ObstacleData[0];
				return 0;
			}

			int n = Math.Max(ObstacleList.Parts.Length, m);
			int i, c = n, l = 0, k = 0;

			OutObstList.Obstacles = new Obstacle[m];
			OutObstList.Parts = new ObstacleData[c];

			GeometryOperators pGeomOper = new GeometryOperators();
			pGeomOper.CurrentGeometry = pPlane;

			MultiPoint pObstPoints;// = new MultiPoint();

			for (i = 0; i < m; i++)
			{
				Geometry pCurrGeom = ObstacleList.Obstacles[i].pGeomPrj;
				if (pGeomOper.Disjoint(pCurrGeom))
					continue;

				if (pCurrGeom.Type == GeometryType.Point)
				{
					pObstPoints = new MultiPoint();
					pObstPoints.Add((Point)pCurrGeom);
				}
				else
				{
					pObstPoints = pGeomOper.Intersect(pPlane, pCurrGeom).ToMultiPoint();
					Geometry pTmpPoints = pGeomOper.Intersect(pPlane, pCurrGeom);
					pObstPoints.AddMultiPoint(pTmpPoints.ToMultiPoint());

					RemoveSeamPoints(ref pObstPoints);
				}

				int p = pObstPoints.Count;
				if (p <= 0)
					continue;

				OutObstList.Obstacles[l] = ObstacleList.Obstacles[i];
				OutObstList.Obstacles[l].PartsNum = p;
				//OutObstList.Obstacles[l].Parts = new int[p];

				for (int j = 0; j < p; j++)
				{
					Point pCurrPt = pObstPoints[j];

					OutObstList.Parts[k].pPtPrj = pCurrPt;
					OutObstList.Parts[k].Owner = l;
					OutObstList.Parts[k].Height = OutObstList.Obstacles[l].Height;
					OutObstList.Parts[k].Index = j;
					//OutObstList.Obstacles[l].Parts[j] = k;

					k++;
					if (k >= c)
					{
						c += n;
						Array.Resize<ObstacleData>(ref OutObstList.Parts, c);
					}
				}

				l++;
			}

			if (k > 0)
			{
				Array.Resize<Obstacle>(ref OutObstList.Obstacles, l);
				Array.Resize<ObstacleData>(ref OutObstList.Parts, k);
			}
			else
			{
				OutObstList.Obstacles = new Obstacle[0];
				OutObstList.Parts = new ObstacleData[0];
			}

			return k;
		}

		public static int GetLegAreaObstacles(ObstacleContainer ObstacleList, out ObstacleContainer OutObstacleList, LegApch CurrLeg, double distFromPrevs, double hTHR, double hKK, double MOC, double PDG)
		{
			int m = ObstacleList.Obstacles.Length;

			if (m <= 0)
			{
				OutObstacleList.Obstacles = new Obstacle[0];
				OutObstacleList.Parts = new ObstacleData[0];
				return 0;
			}

			GeometryOperators fullGeoOp = new GeometryOperators();
			fullGeoOp.CurrentGeometry = CurrLeg.FullAssesmentArea;

			GeometryOperators primaryGeoOp = new GeometryOperators();
			primaryGeoOp.CurrentGeometry = CurrLeg.PrimaryAssesmentArea;

			GeometryOperators lineStrGeoOp = new GeometryOperators();
			lineStrGeoOp.CurrentGeometry = CurrLeg.FullProtectionAreaOutline();	//ARANFunctions.PolygonToPolyLine(CurrLeg.FullAssesmentArea[0]);

			GeometryOperators KKLineGeoOp = new GeometryOperators();
			KKLineGeoOp.CurrentGeometry = CurrLeg.KKLine;

			int c = 10 * m;
			OutObstacleList.Obstacles = new Obstacle[m];
			OutObstacleList.Parts = new ObstacleData[c];

			double maxO = double.MinValue;
			int ix = -1;
			int k = 0, l = 0;

			Aran.Geometries.MultiPoint pObstPoints = new MultiPoint();

			for (int i = 0; i < m; i++)
			{
				Geometry pCurrGeom = ObstacleList.Obstacles[i].pGeomPrj;

				if (pCurrGeom.IsEmpty || fullGeoOp.Disjoint(pCurrGeom))
					continue;

				pObstPoints.Clear();

				if (pCurrGeom.Type == GeometryType.Point)
					pObstPoints.Add((Point)pCurrGeom);
				else
				{
					var pTmpPoints = fullGeoOp.Intersect(pCurrGeom);
					pObstPoints.AddMultiPoint(pTmpPoints.ToMultiPoint());

					pTmpPoints = primaryGeoOp.Intersect(pCurrGeom);
					pObstPoints.AddMultiPoint(pTmpPoints.ToMultiPoint());

					RemoveSeamPoints(ref pObstPoints);
				}

				int n = pObstPoints.Count;
				if (n <= 0)
					continue;

				OutObstacleList.Obstacles[l] = ObstacleList.Obstacles[i];
				OutObstacleList.Obstacles[l].PartsNum = n;
				//OutObstacleList.Obstacles[l].Parts = new int[n];

				int j = 0;
				for (int jr = 0; jr < n; jr++)
				{
					Point ptCurr = pObstPoints[jr];

					if (k >= c)
					{
						c += m;
						Array.Resize<ObstacleData>(ref OutObstacleList.Parts, c);
					}

					OutObstacleList.Parts[k].pPtPrj = ptCurr;
					OutObstacleList.Parts[k].Owner = l;
					OutObstacleList.Parts[k].Height = OutObstacleList.Obstacles[l].Height;
					OutObstacleList.Parts[k].Index = j;
					//OutObstacleList.Obstacles[l].Parts[j] = k;
					j++;
					/*/==============================================================/*/
					OutObstacleList.Parts[k].Elev = ObstacleList.Obstacles[i].Height + hTHR;
					OutObstacleList.Parts[k].d0 = KKLineGeoOp.GetDistance(ptCurr);

					if (maxO < OutObstacleList.Parts[k].Height)
					{
						maxO = OutObstacleList.Parts[k].Height;
						ix = k;
					}

					double distToPrimaryPoly = primaryGeoOp.GetDistance(ptCurr);
					OutObstacleList.Parts[k].Prima = distToPrimaryPoly <= OutObstacleList.Obstacles[OutObstacleList.Parts[k].Owner].HorAccuracy;
					OutObstacleList.Parts[k].fSecCoeff = 1.0;

					if (!OutObstacleList.Parts[k].Prima)
					{
						double d1 = lineStrGeoOp.GetDistance(ptCurr);
						double d = distToPrimaryPoly + d1;
						OutObstacleList.Parts[k].fSecCoeff = d1 / d;
					}

					OutObstacleList.Parts[k].DistStar = OutObstacleList.Parts[k].d0 + distFromPrevs;

					//double dPDG = OutObstacleList.Parts[k].DistStar;
					//if (PtPrevFIX != null && PtPrevFIX.FlyMode == eFlyMode.Atheight)
					//{
					//	Point ptNearest = KKLineGeoOp.GetNearestPoint(ptCurr);

					//	double distFromS = ARANFunctions.Point2LineDistancePrj(ptNearest, PtPrevFIX.PrjPt, CurrLeg.StartFIX.EntryDirection + ARANMath.C_PI_2);
					//	dPDG = OutObstacleList.Parts[k].d0 + Math.Abs(distFromS);
					//}
					//double tmpMOC = dPDG * GlobalVars.constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.dpMOC].Value;
					//OutObstacleList.Parts[k].MOC08 = tmpMOC;
					//if (tmpMOC > MOCLimit)
					//	tmpMOC = MOCLimit;
					//if (tmpMOC < LowerMOCLimit)
					//	tmpMOC = LowerMOCLimit;
					//MOC = GlobalVars.constants.Pansops[ePANSOPSData.arMA_FinalMOC].Value;

					OutObstacleList.Parts[k].MOC = MOC * OutObstacleList.Parts[k].fSecCoeff;
					OutObstacleList.Parts[k].ReqH = OutObstacleList.Parts[k].MOC + OutObstacleList.Parts[k].Height;
					OutObstacleList.Parts[k].hPenet = OutObstacleList.Parts[k].ReqH - OutObstacleList.Parts[k].d0 * PDG - hKK;
					OutObstacleList.Parts[k].Ignored = false;

					k++;
					if (k >= c)
					{
						c += n;
						Array.Resize<ObstacleData>(ref OutObstacleList.Parts, c);
					}
				}

				//if (j > 0 && j != n)		Array.Resize(ref OutObstacleList.Obstacles[l].Parts, j);

				OutObstacleList.Obstacles[l].PartsNum = j;
				l++;
			}

			if (k > 0)
			{
				Array.Resize<Obstacle>(ref OutObstacleList.Obstacles, l);
				Array.Resize<ObstacleData>(ref OutObstacleList.Parts, k);
			}
			else
			{
				OutObstacleList.Obstacles = new Obstacle[0];
				OutObstacleList.Parts = new ObstacleData[0];
			}

			//GlobalVars.gAranGraphics.DrawLineString((LineString)CurrLeg.KKLine, 255, 2);
			//Leg.ProcessMessages();

			return ix;
		}

		//public static int GetLegAreaObstacles_0(out ObstacleContainer OutObstacleList, Leg CurrLeg, double distFromPrevs, double hDER, double MOCLimit, WayPoint PtPrevFIX = null)
		//{
		//	int m = GlobalVars.ObstacleList.Obstacles.Length;
		//	int n = GlobalVars.ObstacleList.Parts.Length;

		//	OutObstacleList.Obstacles = new Obstacle[m];
		//	OutObstacleList.Parts = new ObstacleData[n];

		//	if (n > 0 && m > 0)
		//	{
		//		OutObstacleList.Obstacles = new Obstacle[m];
		//		OutObstacleList.Parts = new ObstacleData[n];
		//	}
		//	else
		//	{
		//		OutObstacleList.Obstacles = new Obstacle[0];
		//		OutObstacleList.Parts = new ObstacleData[0];
		//		return -1;
		//	}

		//	//================================================================================================

		//	double LowerMOCLimit = GlobalVars.constants.Pansops[ePANSOPSData.dpObsClr].Value;
		//	GeometryOperators fullGeoOp = new GeometryOperators();
		//	fullGeoOp.CurrentGeometry = CurrLeg.FullAssesmentArea;

		//	GeometryOperators primaryGeoOp = new GeometryOperators();
		//	primaryGeoOp.CurrentGeometry = CurrLeg.PrimaryAssesmentArea;

		//	GeometryOperators lineStrGeoOp = new GeometryOperators();
		//	lineStrGeoOp.CurrentGeometry = ARANFunctions.PolygonToPolyLine(CurrLeg.FullAssesmentArea[0]);

		//	GeometryOperators KKLineGeoOp = new GeometryOperators();
		//	KKLineGeoOp.CurrentGeometry = CurrLeg.KKLine;

		//	for (int i = 0; i < m; i++)
		//		GlobalVars.ObstacleList.Obstacles[i].NIx = -1;

		//	double maxO = double.MinValue;
		//	int ix = -1;
		//	int k = 0, l = 0;

		//	for (int i = 0; i < n; i++)
		//	{
		//		Point ptCurr = GlobalVars.ObstacleList.Parts[i].pPtPrj;
		//		double distToFullPoly = fullGeoOp.GetDistance(ptCurr);

		//		if (distToFullPoly > GlobalVars.ObstacleList.Obstacles[GlobalVars.ObstacleList.Parts[i].Owner].HorAccuracy)
		//			continue;

		//		//if (pCommonContur.Disjoint(WorkObstacleList.Parts[i].pPtPrj))
		//		//	continue;
		//		//if (pPrecisionContur.Disjoint(WorkObstacleList.Parts[i].pPtPrj))
		//		//	continue;

		//		OutObstacleList.Parts[k] = GlobalVars.ObstacleList.Parts[i];

		//		if (GlobalVars.ObstacleList.Obstacles[GlobalVars.ObstacleList.Parts[i].Owner].NIx < 0)
		//		{
		//			OutObstacleList.Obstacles[l] = GlobalVars.ObstacleList.Obstacles[GlobalVars.ObstacleList.Parts[i].Owner];
		//			OutObstacleList.Obstacles[l].PartsNum = 0;
		//			OutObstacleList.Obstacles[l].Parts = new int[GlobalVars.ObstacleList.Obstacles[GlobalVars.ObstacleList.Parts[i].Owner].PartsNum];
		//			GlobalVars.ObstacleList.Obstacles[GlobalVars.ObstacleList.Parts[i].Owner].NIx = l;
		//			l++;
		//		}

		//		OutObstacleList.Parts[k].Owner = GlobalVars.ObstacleList.Obstacles[GlobalVars.ObstacleList.Parts[i].Owner].NIx;
		//		OutObstacleList.Parts[k].Index = OutObstacleList.Obstacles[OutObstacleList.Parts[k].Owner].PartsNum;

		//		OutObstacleList.Obstacles[OutObstacleList.Parts[k].Owner].Parts[OutObstacleList.Obstacles[OutObstacleList.Parts[k].Owner].PartsNum] = k;
		//		OutObstacleList.Obstacles[OutObstacleList.Parts[k].Owner].PartsNum++;

		//		//ARANFunctions.PrjToLocal(ptFAF, ArDir + Math.PI, OutObstacleList.Parts[k].pPtPrj, out x, out y);
		//		//OutObstacleList.Parts[k].Dist = x;
		//		//OutObstacleList.Parts[k].DistStar = y;

		//		OutObstacleList.Parts[k].Elev = OutObstacleList.Parts[k].Height;
		//		OutObstacleList.Parts[k].Height -= hDER;
		//		OutObstacleList.Parts[k].d0 = KKLineGeoOp.GetDistance(ptCurr);

		//		if (maxO < OutObstacleList.Parts[k].Height)
		//		{
		//			maxO = OutObstacleList.Parts[k].Height;
		//			ix = k;
		//		}

		//		double distToPrimaryPoly = primaryGeoOp.GetDistance(ptCurr);
		//		OutObstacleList.Parts[k].Prima = distToPrimaryPoly <= OutObstacleList.Obstacles[OutObstacleList.Parts[k].Owner].HorAccuracy;
		//		OutObstacleList.Parts[k].fTmp = 1.0;

		//		if (!OutObstacleList.Parts[k].Prima)
		//		{
		//			double d1 = lineStrGeoOp.GetDistance(ptCurr);
		//			double d = distToPrimaryPoly + d1;
		//			OutObstacleList.Parts[k].fTmp = d1 / d;
		//		}

		//		OutObstacleList.Parts[k].DistStar = OutObstacleList.Parts[k].d0 + distFromPrevs;

		//		double dPDG = OutObstacleList.Parts[k].DistStar;

		//		if (PtPrevFIX != null && PtPrevFIX.FlyMode == eFlyMode.Atheight)
		//		{
		//			Point ptNearest = KKLineGeoOp.GetNearestPoint(ptCurr);

		//			double distFromS = ARANFunctions.Point2LineDistancePrj(ptNearest, PtPrevFIX.PrjPt, CurrLeg.StartFIX.EntryDirection + ARANMath.C_PI_2);
		//			dPDG = OutObstacleList.Parts[k].d0 + Math.Abs(distFromS);
		//		}

		//		double tmpMOC = dPDG * GlobalVars.constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.dpMOC].Value;

		//		//OutObstacleList.Parts[k].MOC08 = tmpMOC;

		//		if (tmpMOC > MOCLimit)
		//			tmpMOC = MOCLimit;
		//		if (tmpMOC < LowerMOCLimit)
		//			tmpMOC = LowerMOCLimit;

		//		OutObstacleList.Parts[k].MOC = tmpMOC * OutObstacleList.Parts[k].fTmp;
		//		OutObstacleList.Parts[k].ReqH = OutObstacleList.Parts[k].MOC + OutObstacleList.Parts[k].Height;
		//		OutObstacleList.Parts[k].PDG = (OutObstacleList.Parts[k].ReqH - GlobalVars.constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.arAbv_Treshold].Value) / OutObstacleList.Parts[k].DistStar;
		//		OutObstacleList.Parts[k].Ignored = false;

		//		k++;
		//	}

		//	if (k > 0)
		//	{
		//		Array.Resize<Obstacle>(ref OutObstacleList.Obstacles, l);
		//		Array.Resize<ObstacleData>(ref OutObstacleList.Parts, k);
		//	}
		//	else
		//	{
		//		OutObstacleList.Obstacles = new Obstacle[0];
		//		OutObstacleList.Parts = new ObstacleData[0];
		//	}

		//	return ix;
		//}

		public static int GetIntermObstacleList(ObstacleContainer ObstacleList, out ObstacleContainer OutObstList, Point ptLHPrj, double ArDir, MultiPolygon pFullPlane, MultiPolygon pPrimePlane = null)
		{
			const double farDistance = 1000000.0;
			int m = ObstacleList.Obstacles.Length;

			if (m < 0 || pFullPlane.IsEmpty)
			{
				OutObstList.Obstacles = new Obstacle[0];
				OutObstList.Parts = new ObstacleData[0];
				return 0;
			}

			OutObstList.Obstacles = new Obstacle[m];

			int n = Math.Max(ObstacleList.Parts.Length, m);
			int i, c = n, l = 0, k = 0;

			OutObstList.Parts = new ObstacleData[c];

			GeometryOperators pFullGeomOper = new GeometryOperators();
			pFullGeomOper.CurrentGeometry = pFullPlane;

			GeometryOperators pPrimGeomOper = null;

			if (pPrimePlane != null)
			{
				pPrimGeomOper = new GeometryOperators();
				pPrimGeomOper.CurrentGeometry = pPrimePlane;
			}

			MultiPoint pObstPoints = new MultiPoint();

			for (i = 0; i < m; i++)
			{
				Geometry pCurrGeom = ObstacleList.Obstacles[i].pGeomPrj;
				if (pFullGeomOper.Disjoint(pCurrGeom))
					continue;

				pObstPoints.Clear();

				if (pCurrGeom.Type == GeometryType.Point)
					pObstPoints.Add((Point)pCurrGeom);
				else
				{
					Geometry pTmpPoints = pFullGeomOper.Intersect(pCurrGeom);
					pObstPoints.AddMultiPoint(pTmpPoints.ToMultiPoint());
					
					if (pPrimePlane != null)
					{
						pTmpPoints = pFullGeomOper.Intersect(pCurrGeom);
						pObstPoints.AddMultiPoint(pTmpPoints.ToMultiPoint());
					}

					RemoveSeamPoints(ref pObstPoints);
				}

				int p = pObstPoints.Count;
				if (p <= 0)
					continue;

				OutObstList.Obstacles[l] = ObstacleList.Obstacles[i];
				OutObstList.Obstacles[l].PartsNum = p;
				//OutObstList.Obstacles[l].Parts = new int[p];

				for (int j = 0; j < p; j++)
				{
					Point pCurrPt = pObstPoints[j];

					OutObstList.Parts[k].pPtPrj = pCurrPt;
					OutObstList.Parts[k].Owner = l;
					OutObstList.Parts[k].Height = OutObstList.Obstacles[l].Height;
					OutObstList.Parts[k].Index = j;
					//OutObstList.Obstacles[l].Parts[j] = k;

					//pZv = ObstacleList.Obstacles[i].pGeomPrj
					//OutObstList.Obstacles[L].Height = pZv.ZMax - ptLHPrj.Z

					OutObstList.Parts[k].Height = OutObstList.Parts[k].pPtPrj.Z - ptLHPrj.Z;
					ARANFunctions.PrjToLocal(ptLHPrj, ArDir, OutObstList.Parts[k].pPtPrj, out  OutObstList.Parts[k].Dist, out OutObstList.Parts[k].DistStar);
					OutObstList.Parts[k].DistStar = Math.Abs(OutObstList.Parts[k].DistStar);
					OutObstList.Parts[k].ReqH = OutObstList.Parts[j].Height + GlobalVars.constants.Pansops[ePANSOPSData.arMA_FinalMOC].Value;
					OutObstList.Parts[k].fSecCoeff = 1.0;

					if (pPrimePlane != null)
					{
						OutObstList.Parts[k].Prima = !pPrimGeomOper.Disjoint(pCurrPt);
						//double distToPrimaryPoly = pPrimGeomOper.GetDistance(pCurrPt);
						//distToPrimaryPoly <= OutObstList.Obstacles[OutObstList.Parts[k].Owner].HorAccuracy;

						if (!OutObstList.Parts[k].Prima)
						{
							double x, y;
							double distToPrimaryPoly = pPrimGeomOper.GetDistance(pCurrPt);
							//GlobalVars.gAranGraphics.DrawPointWithText(pCurrPt, -1, "O_1");

							ARANFunctions.PrjToLocal(ptLHPrj, ArDir, pCurrPt, out x, out y);

							LineString pls = new LineString();
							pls.Add(pCurrPt);
							pls.Add(ARANFunctions.LocalToPrj(ptLHPrj, ArDir, x, 100000.0 * Math.Sign(y)));

							var mls = pFullGeomOper.Intersect(pls);

							Point ptTmp = ARANFunctions.LocalToPrj(ptLHPrj, ArDir, x, farDistance * Math.Sign(y));

							double distToFullPoly = farDistance - pFullGeomOper.GetDistance(ptTmp) - Math.Abs(y);//lineStrGeoOp.GetDistance(pls);
							double dFull = distToPrimaryPoly + distToFullPoly;

							OutObstList.Parts[k].fSecCoeff = distToFullPoly / dFull;
						}
					}

					k++;
					if (k >= c)
					{
						c += n;
						Array.Resize<ObstacleData>(ref OutObstList.Parts, c);
					}
				}

				l++;
			}

			if (k > 0)
			{
				Array.Resize<Obstacle>(ref OutObstList.Obstacles, l);
				Array.Resize<ObstacleData>(ref OutObstList.Parts, k);
			}
			else
			{
				OutObstList.Obstacles = new Obstacle[0];
				OutObstList.Parts = new ObstacleData[0];
			}

			return k - 1;
		}

		public static bool AngleInSectorDeg(double angle, double X, double Y)
		{
			X = NativeMethods.Modulus(X);
			Y = NativeMethods.Modulus(Y);
			angle = NativeMethods.Modulus(angle);

			if (X > Y)
			{
				if (angle >= X || angle <= Y) return true;
			}
			else if (angle >= X && angle <= Y) return true;

			return false;
		}

		//public static void ConsiderObs(ref Obstacle[] ObstList, Point ptCenter, RWYType Rwy, double fRadius, out double maxDist, out int obsNum)
		//{
		//	maxDist = 0.0;
		//	obsNum = 0;

		//	if (fRadius == 0)
		//		return;

		//	int n = ObstList.Length;
		//	Point PtEnd = Rwy.pPtPrj[eRWY.ptDER];
		//	double CLDir = PtEnd.M;

		//	for (int i = 0; i < n; i++)
		//	{
		//		Point ptCurr = (Point)ObstList[i].pGeomPrj;

		//		ObstList[i].Height = ptCurr.Z - PtEnd.Z;
		//		ObstList[i].HeightMax = ObstList[i].Height + ObstList[i].VertAccuracy;

		//		ARANFunctions.PrjToLocal(PtEnd, CLDir, ptCurr, out ObstList[i].X, out ObstList[i].Y);

		//		int sign = Math.Sign(ObstList[i].X);
		//		ObstList[i].Xmin = Math.Abs(ObstList[i].X) - ObstList[i].HorAccuracy;
		//		if (ObstList[i].Xmin < 0.0)
		//			ObstList[i].Xmin = 0.0;
		//		ObstList[i].Xmin *= sign;

		//		sign = Math.Sign(ObstList[i].Y);
		//		ObstList[i].Ymin = Math.Abs(ObstList[i].Y) - ObstList[i].HorAccuracy;
		//		if (ObstList[i].Ymin < 0.0)
		//			ObstList[i].Ymin = 0.0;
		//		ObstList[i].Ymin *= sign;

		//		double fDist = ARANFunctions.ReturnDistanceInMeters(ptCurr, ptCenter);
		//		if (fDist <= fRadius)
		//		{
		//			obsNum++;
		//			if (fDist > maxDist)
		//				maxDist = fDist;
		//		}
		//	}
		//}

		public static void TextBoxFloat(ref char KeyChar, string BoxText)
		{
			if (KeyChar < 32)
				return;

			char DecSep = (1.1).ToString().ToCharArray()[1];

			if (((KeyChar < '0') || KeyChar > '9') && KeyChar != DecSep)
				KeyChar = '\0';
			else if (KeyChar == DecSep && BoxText.IndexOf(DecSep) >= 0)
				KeyChar = '\0';
		}

		public static void TextBoxFloatWithSign(ref char KeyChar, string BoxText)
		{
			if (KeyChar < 32)
				return;

			char DecSep = (1.1).ToString().ToCharArray()[1];

			if (((KeyChar < '0') || KeyChar > '9') && KeyChar != DecSep && KeyChar != '-' && KeyChar != '+')
				KeyChar = '\0';
			else if (KeyChar == DecSep && BoxText.IndexOf(DecSep) >= 0)
				KeyChar = '\0';
			else if (KeyChar == '-' && BoxText.IndexOf('-') >= 0)
				KeyChar = '\0';
			else if (KeyChar == '+' && BoxText.IndexOf('+') >= 0)
				KeyChar = '\0';
		}

		public static void TextBoxFloatWithMinus(ref char KeyChar, string BoxText)
		{
			if (KeyChar < 32)
				return;

			char DecSep = (1.1).ToString().ToCharArray()[1];

			if (((KeyChar < '0') || KeyChar > '9') && KeyChar != DecSep && KeyChar != '-' && KeyChar != '+')
				KeyChar = '\0';
			else if (KeyChar == DecSep && BoxText.IndexOf(DecSep) >= 0)
				KeyChar = '\0';
			else if (KeyChar == '-' && BoxText != "")
				KeyChar = '\0';
		}

		public static void TextBoxInteger(ref char KeyChar)
		{
			if (KeyChar < ' ')
				return;
			if ((KeyChar < '0') || (KeyChar > '9'))
				KeyChar = '\0';
		}

		public static void TextBoxLimitCount(ref char KeyChar, string BoxText, int n)
		{
			if (KeyChar < 32)
				return;
			if (BoxText.Length >= n)
				KeyChar = '\0';
		}

		public static string[] NavTypeNames = new string[] { "VOR", "DME", "NDB", "LLZ", "TACAN", "Radar FIX" };

		public static string GetNavTypeName(eNavaidType navType)
		{
			if (navType == eNavaidType.NONE)
				return "WPT";
			else
				return NavTypeNames[(int)navType];
		}

		public static void CreateOFZPlanes(Point ptLHPrj, double ArDir, double H, ref  D3DPolygone[] OFZPlanes)
		{
            // PANSOPS 5.6 ANNEX 14 chapter 4
            Point pt1, pt2, pt3, Pt4;

			pt1 = ARANFunctions.PointAlongPlane(ptLHPrj, ArDir + Math.PI, (H - 27.0 + 1.2) * 50.0);
			pt2 = ARANFunctions.PointAlongPlane(ptLHPrj, ArDir + Math.PI, 60.0);
			pt3 = ARANFunctions.PointAlongPlane(ptLHPrj, ArDir, 1800.0);
			Pt4 = ARANFunctions.PointAlongPlane(ptLHPrj, ArDir, (H + 59.94) * 30.03);

			pt2.Z = 0.0;
			pt3.Z = 0.0;

			int i, n = OFZPlanes.Length;
			Ring[] pWorkRings = new Ring[n];

			for (i = 0; i < n; i++)
			{
				OFZPlanes[i].Poly = new MultiPolygon();
				pWorkRings[i] = new Ring();
			}
			//=================================================================
			pWorkRings[0].Add(ARANFunctions.PointAlongPlane(pt2, ArDir - ARANMath.C_PI_2, 60.0));
			pWorkRings[0].Add(ARANFunctions.PointAlongPlane(pt2, ArDir + ARANMath.C_PI_2, 60.0));
			pWorkRings[0].Add(ARANFunctions.PointAlongPlane(pt3, ArDir + ARANMath.C_PI_2, 60.0));
			pWorkRings[0].Add(ARANFunctions.PointAlongPlane(pt3, ArDir - ARANMath.C_PI_2, 60.0));

			OFZPlanes[0].Plane.pPt = pt2;
			OFZPlanes[0].Plane.X = 0.0;
			OFZPlanes[0].Plane.Y = 0.0;
			OFZPlanes[0].Plane.Z = 1.0;

			OFZPlanes[0].Plane.A = 0.0;
			OFZPlanes[0].Plane.B = 0.0;
			OFZPlanes[0].Plane.C = -1.0;
			OFZPlanes[0].Plane.D = 0.0;

			//=================================================================
			pWorkRings[1].Add(ARANFunctions.PointAlongPlane(pt1, ArDir - ARANMath.C_PI_2, 60.0));
			pWorkRings[1].Add(ARANFunctions.PointAlongPlane(pt1, ArDir + ARANMath.C_PI_2, 60.0));
			pWorkRings[1].Add(pWorkRings[0][1]);
			pWorkRings[1].Add(pWorkRings[0][0]);

			OFZPlanes[1].Plane.pPt = pt2;
			OFZPlanes[1].Plane.A = 0.02;
			OFZPlanes[1].Plane.B = 0.0;
			OFZPlanes[1].Plane.C = -1.0;
			OFZPlanes[1].Plane.D = -1.2;

			//=================================================================
			pWorkRings[2].Add(pWorkRings[1][1]);
			pWorkRings[2].Add(ARANFunctions.PointAlongPlane(pt1, ArDir + ARANMath.C_PI_2, 3.003 * (21.18 - 1.2 + 27.0)));
			pWorkRings[2].Add(ARANFunctions.PointAlongPlane(pt2, ArDir + ARANMath.C_PI_2, 3.003 * (H + 21.18 - 0.02 * 60.0)));
			pWorkRings[2].Add(pWorkRings[1][2]);

			OFZPlanes[2].Plane.pPt = pWorkRings[2][3];
			OFZPlanes[2].Plane.pPt.Z = 0.0;

			OFZPlanes[2].Plane.A = 0.02;
			OFZPlanes[2].Plane.B = -0.333;
			OFZPlanes[2].Plane.C = -1.0;
			OFZPlanes[2].Plane.D = -21.18;

			//=================================================================
			pWorkRings[3].Add(pWorkRings[0][1]);
			pWorkRings[3].Add(pWorkRings[2][2]);
			pWorkRings[3].Add(ARANFunctions.PointAlongPlane(Pt4, ArDir + ARANMath.C_PI_2, 3.003 * (H + 19.98)));
			pWorkRings[3].Add(pWorkRings[0][2]);

			OFZPlanes[3].Plane.pPt = pWorkRings[3][3];
			OFZPlanes[3].Plane.pPt.Z = 0.0;

			OFZPlanes[3].Plane.A = 0.0;
			OFZPlanes[3].Plane.B = -0.333;
			OFZPlanes[3].Plane.C = -1.0;
			OFZPlanes[3].Plane.D = -19.98;

			//=================================================================
			pWorkRings[4].Add(pWorkRings[3][3]);
			pWorkRings[4].Add(pWorkRings[3][2]);
			pWorkRings[4].Add(ARANFunctions.PointAlongPlane(Pt4, ArDir - ARANMath.C_PI_2, 3.003 * (H + 19.98)));
			pWorkRings[4].Add(pWorkRings[0][3]);

			OFZPlanes[4].Plane.pPt = pWorkRings[4][3];
			OFZPlanes[4].Plane.pPt.Z = 0.0;

			OFZPlanes[4].Plane.A = -0.0333;
			OFZPlanes[4].Plane.B = 0.0;
			OFZPlanes[4].Plane.C = -1.0;
			OFZPlanes[4].Plane.D = -59.94;

			//=================================================================
			pWorkRings[5].Add(pWorkRings[0][0]);
			pWorkRings[5].Add(pWorkRings[0][3]);
			pWorkRings[5].Add(pWorkRings[4][2]);
			pWorkRings[5].Add(ARANFunctions.PointAlongPlane(pt2, ArDir - ARANMath.C_PI_2, 3.003 * (H + 21.18 - 0.02 * 60.0)));

			OFZPlanes[5].Plane.pPt = pWorkRings[5][0];
			OFZPlanes[5].Plane.pPt.Z = 0.0;

			OFZPlanes[5].Plane.A = 0.0;
			OFZPlanes[5].Plane.B = 0.333;
			OFZPlanes[5].Plane.C = -1.0;
			OFZPlanes[5].Plane.D = -19.98;

			//=================================================================
			pWorkRings[6].Add(pWorkRings[1][0]);
			pWorkRings[6].Add(pWorkRings[1][3]);
			pWorkRings[6].Add(pWorkRings[5][3]);
			pWorkRings[6].Add(ARANFunctions.PointAlongPlane(pt1, ArDir - ARANMath.C_PI_2, 3.003 * (21.18 - 1.2 + 27.0)));

			OFZPlanes[6].Plane.pPt = pWorkRings[6][1];
			OFZPlanes[6].Plane.pPt.Z = 0.0;

			OFZPlanes[6].Plane.A = 0.02;
			OFZPlanes[6].Plane.B = 0.333;
			OFZPlanes[6].Plane.C = -1.0;
			OFZPlanes[6].Plane.D = -21.18;

			//=================================================================
			GeometryOperators geometryOperators = new GeometryOperators();

			for (i = 0; i < n - 1; i++)
			{
				Polygon pPoly = new Polygon();
				pPoly.ExteriorRing = pWorkRings[i];
				OFZPlanes[i].Poly.Add(pPoly);
				OFZPlanes[n - 1].Poly = (MultiPolygon)geometryOperators.UnionGeometry(OFZPlanes[n - 1].Poly, OFZPlanes[i].Poly);
			}
		}

		public static void CreateILSPlanes(Point ptTHRPrj, double ArDir, ref D3DPolygone[] ILSPlanes)
		{
			Point ptD = ARANFunctions.PointAlongPlane(ptTHRPrj, ArDir + Math.PI, 12660.0);
			Point ptH = ARANFunctions.PointAlongPlane(ptTHRPrj, ArDir + Math.PI, 3060.0);
			Point PtG = ARANFunctions.PointAlongPlane(ptTHRPrj, ArDir + Math.PI, 60.0);
			Point ptF = ARANFunctions.PointAlongPlane(ptTHRPrj, ArDir, 900.0);
			Point ptE = ARANFunctions.PointAlongPlane(ptTHRPrj, ArDir, 2700.0);
			Point ptA = ARANFunctions.PointAlongPlane(ptTHRPrj, ArDir, 12900.0);

			ptF.Z = 0.0;
			PtG.Z = 0.0;

			int i, n = ILSPlanes.Length;
			Ring[] pWorkRings = new Ring[n];

			for (i = 0; i < n; i++)
			{
				ILSPlanes[i].Poly = new MultiPolygon();
				pWorkRings[i] = new Ring();
			}

			//=================================================================
			pWorkRings[0].Add(ARANFunctions.PointAlongPlane(ptF, ArDir - ARANMath.C_PI_2, 150.0));
			pWorkRings[0].Add(ARANFunctions.PointAlongPlane(PtG, ArDir - ARANMath.C_PI_2, 150.0));
			pWorkRings[0].Add(ARANFunctions.PointAlongPlane(PtG, ArDir + ARANMath.C_PI_2, 150.0));
			pWorkRings[0].Add(ARANFunctions.PointAlongPlane(ptF, ArDir + ARANMath.C_PI_2, 150.0));

			ILSPlanes[0].Plane.pPt = ptF;
			ILSPlanes[0].Plane.X = 0.0;
			ILSPlanes[0].Plane.Y = 0.0;
			ILSPlanes[0].Plane.Z = 1.0;

			ILSPlanes[0].Plane.A = 0.0;
			ILSPlanes[0].Plane.B = 0.0;
			ILSPlanes[0].Plane.C = -1.0;
			ILSPlanes[0].Plane.D = 0.0;

			//=================================================================
			pWorkRings[1].Add(pWorkRings[0][1]);
			pWorkRings[1].Add(ARANFunctions.PointAlongPlane(ptH, ArDir - ARANMath.C_PI_2, 600.0));
			pWorkRings[1].Add(ARANFunctions.PointAlongPlane(ptH, ArDir + ARANMath.C_PI_2, 600.0));
			pWorkRings[1].Add(pWorkRings[0][2]);

			ILSPlanes[1].Plane.pPt = ptH;
			ILSPlanes[1].Plane.A = 0.02;
			ILSPlanes[1].Plane.B = 0.0;
			ILSPlanes[1].Plane.C = -1.0;
			ILSPlanes[1].Plane.D = -1.2;

			//=================================================================
			pWorkRings[2].Add(pWorkRings[1][1]);
			pWorkRings[2].Add(ARANFunctions.PointAlongPlane(ptD, ArDir - ARANMath.C_PI_2, 2040.0));
			pWorkRings[2].Add(ARANFunctions.PointAlongPlane(ptD, ArDir + ARANMath.C_PI_2, 2040.0));
			pWorkRings[2].Add(pWorkRings[1][2]);

			ILSPlanes[2].Plane.pPt = ptD;
			ILSPlanes[2].Plane.pPt.Z = 0.0;

			ILSPlanes[2].Plane.A = 0.025;
			ILSPlanes[2].Plane.B = 0.0;
			ILSPlanes[2].Plane.C = -1.0;
			ILSPlanes[2].Plane.D = -16.5;

			//=================================================================
			pWorkRings[3].Add(pWorkRings[2][3]);
			pWorkRings[3].Add(pWorkRings[2][2]);
			pWorkRings[3].Add(ARANFunctions.PointAlongPlane(ptH, ArDir + ARANMath.C_PI_2, 2278.0));

			ILSPlanes[3].Plane.pPt = ptH;
			ILSPlanes[3].Plane.pPt.Z = 0.0;

			ILSPlanes[3].Plane.A = 0.00355;
			ILSPlanes[3].Plane.B = -0.143;
			ILSPlanes[3].Plane.C = -1.0;
			ILSPlanes[3].Plane.D = -36.66;

			//=================================================================
			pWorkRings[11].Add(pWorkRings[2][0]);
			pWorkRings[11].Add(ARANFunctions.PointAlongPlane(ptH, ArDir - ARANMath.C_PI_2, 2278.0));
			pWorkRings[11].Add(pWorkRings[2][1]);

			ILSPlanes[11].Plane.pPt = ptH;

			ILSPlanes[11].Plane.A = 0.00355;
			ILSPlanes[11].Plane.B = 0.143;
			ILSPlanes[11].Plane.C = -1.0;
			ILSPlanes[11].Plane.D = -36.66;

			//=================================================================
			pWorkRings[4].Add(pWorkRings[1][3]);
			pWorkRings[4].Add(pWorkRings[1][2]);
			pWorkRings[4].Add(pWorkRings[3][2]);
			pWorkRings[4].Add(ARANFunctions.PointAlongPlane(PtG, ArDir + ARANMath.C_PI_2, 2248.0));

			ILSPlanes[4].Plane.pPt = PtG;

			ILSPlanes[4].Plane.A = -0.00145;
			ILSPlanes[4].Plane.B = -0.143;
			ILSPlanes[4].Plane.C = -1.0;
			ILSPlanes[4].Plane.D = -21.36;

			//=================================================================
			pWorkRings[10].Add(pWorkRings[1][0]);
			pWorkRings[10].Add(ARANFunctions.PointAlongPlane(PtG, ArDir - ARANMath.C_PI_2, 2248.0));
			pWorkRings[10].Add(pWorkRings[11][1]);
			pWorkRings[10].Add(pWorkRings[1][1]);

			ILSPlanes[10].Plane.pPt = PtG;

			ILSPlanes[10].Plane.A = -0.00145;
			ILSPlanes[10].Plane.B = 0.143;
			ILSPlanes[10].Plane.C = -1.0;
			ILSPlanes[10].Plane.D = -21.36;

			//=================================================================
			pWorkRings[5].Add(pWorkRings[0][3]);
			pWorkRings[5].Add(pWorkRings[0][2]);
			pWorkRings[5].Add(pWorkRings[4][3]);
			pWorkRings[5].Add(ARANFunctions.PointAlongPlane(ptE, ArDir + ARANMath.C_PI_2, 2248.0));
			pWorkRings[5].Add(ARANFunctions.PointAlongPlane(ptE, ArDir + ARANMath.C_PI_2, 465.0));

			ILSPlanes[5].Plane.pPt = ptE;

			ILSPlanes[5].Plane.A = 0.0;
			ILSPlanes[5].Plane.B = -0.143;
			ILSPlanes[5].Plane.C = -1.0;
			ILSPlanes[5].Plane.D = -21.45;

			//=================================================================
			pWorkRings[9].Add(ARANFunctions.PointAlongPlane(ptE, ArDir - ARANMath.C_PI_2, 465.0));
			pWorkRings[9].Add(ARANFunctions.PointAlongPlane(ptE, ArDir - ARANMath.C_PI_2, 2248.0));
			pWorkRings[9].Add(pWorkRings[10][1]);
			pWorkRings[9].Add(pWorkRings[0][1]);
			pWorkRings[9].Add(pWorkRings[0][0]);

			ILSPlanes[9].Plane.pPt = ptE;

			ILSPlanes[9].Plane.A = 0.0;
			ILSPlanes[9].Plane.B = 0.143;
			ILSPlanes[9].Plane.C = -1.0;
			ILSPlanes[9].Plane.D = -21.45;

			//=================================================================
			pWorkRings[6].Add(pWorkRings[5][4]);
			pWorkRings[6].Add(pWorkRings[5][3]);
			pWorkRings[6].Add(ARANFunctions.PointAlongPlane(ptA, ArDir + ARANMath.C_PI_2, 3015.0));

			ILSPlanes[6].Plane.pPt = ptA;

			ILSPlanes[6].Plane.A = 0.01075;
			ILSPlanes[6].Plane.B = -0.143;
			ILSPlanes[6].Plane.C = -1.0;
			ILSPlanes[6].Plane.D = 7.58;

			//=================================================================
			pWorkRings[8].Add(pWorkRings[9][0]);
			pWorkRings[8].Add(ARANFunctions.PointAlongPlane(ptA, ArDir - ARANMath.C_PI_2, 3015.0));
			pWorkRings[8].Add(pWorkRings[9][1]);

			ILSPlanes[8].Plane.pPt = ptA;

			ILSPlanes[8].Plane.A = 0.01075;
			ILSPlanes[8].Plane.B = 0.143;
			ILSPlanes[8].Plane.C = -1.0;
			ILSPlanes[8].Plane.D = 7.58;

			//=================================================================
			pWorkRings[7].Add(pWorkRings[0][0]);
			pWorkRings[7].Add(pWorkRings[0][3]);
			pWorkRings[7].Add(pWorkRings[6][0]);
			pWorkRings[7].Add(pWorkRings[6][2]);
			pWorkRings[7].Add(pWorkRings[8][1]);
			pWorkRings[7].Add(pWorkRings[8][0]);

			ILSPlanes[7].Plane.pPt = ptA;

			ILSPlanes[7].Plane.A = -0.025;
			ILSPlanes[7].Plane.B = 0.0;
			ILSPlanes[7].Plane.C = -1.0;
			ILSPlanes[7].Plane.D = -22.5;

			//=================================================================
			double fAlpha = System.Math.Atan(1440.0 / 9600.0);
			double fDistPl2 = (18500.0 - 3060.0) / System.Math.Cos(fAlpha);

			double fAztPl2 = ARANFunctions.ReturnAngleInRadians(pWorkRings[2][0], pWorkRings[2][1]);
			//pWorkRings[2].ReplacePoint(1, ARANFunctions.PointAlongPlane(pWorkRings[2][0], fAztPl2, fDistPl2));
			pWorkRings[2][1].Assign(ARANFunctions.PointAlongPlane(pWorkRings[2][0], fAztPl2, fDistPl2));

			fAztPl2 = ARANFunctions.ReturnAngleInRadians(pWorkRings[2][3], pWorkRings[2][2]);
			//pWorkRings[2].ReplacePoint(2, ARANFunctions.PointAlongPlane(pWorkRings[2][3], fAztPl2, fDistPl2));
			pWorkRings[2][2].Assign(ARANFunctions.PointAlongPlane(pWorkRings[2][3], fAztPl2, fDistPl2));

			//=================================================================
			GeometryOperators geometryOperators = new GeometryOperators();

			for (i = 0; i < n - 1; i++)
			{
				Polygon pPoly = new Polygon();
				pPoly.ExteriorRing = pWorkRings[i];
				ILSPlanes[i].Poly.Add(pPoly);

				ILSPlanes[n - 1].Poly = (MultiPolygon)geometryOperators.UnionGeometry(ILSPlanes[n - 1].Poly, ILSPlanes[i].Poly);
			}
		}

		static void RemoveSeamPoints(ref MultiPoint pPoints)
		{
			const double eps2 = ARANMath.EpsilonDistance * ARANMath.EpsilonDistance;
			int n = pPoints.Count;
			int j = 0;

			while (j < n - 1)
			{
				Point pCurrPt = pPoints[j];
				int i = j + 1;
				while (i < n)
				{
					double dx = pCurrPt.X - pPoints[i].X;
					double dy = pCurrPt.Y - pPoints[i].Y;
					double fDist2 = dx * dx + dy * dy;

					if (fDist2 < eps2)
					{
						pPoints.Remove(i);
						n--;
					}
					else
						i++;
				}
				j++;
			}
		}

		public static int GetPrecisionAndIntermetiatObstacles(ObstacleContainer WorkObstacleList, out ObstacleContainer PrecisionObstacles, ObstacleContainer ObstacleList, out ObstacleContainer IntermetiatObstacles, Point ptFAF, double ArDir, double refHeight, MultiPolygon pPrecisionArea, MultiPolygon IntermSecondaryArea, MultiPolygon IntermPrimaryArea)
		{
			GeometryOperators pCommonContur = new GeometryOperators();
			pCommonContur.CurrentGeometry = IntermSecondaryArea;

			GeometryOperators pPrecisionContur = new GeometryOperators();
			pPrecisionContur.CurrentGeometry = pPrecisionArea;

			int i;
			int m = WorkObstacleList.Obstacles.Length;
			int n = WorkObstacleList.Parts.Length;
			double x, y;

			if (n > 0 && m > 0)
			{
				PrecisionObstacles.Obstacles = new Obstacle[m];
				PrecisionObstacles.Parts = new ObstacleData[n];
			}
			else
			{
				PrecisionObstacles.Obstacles = new Obstacle[0];
				PrecisionObstacles.Parts = new ObstacleData[0];
			}

			for (i = 0; i < m; i++)
				WorkObstacleList.Obstacles[i].NIx = -1;

			int k = 0, l = 0;

			for (i = 0; i < n; i++)
			{
				if (pCommonContur.Disjoint(WorkObstacleList.Parts[i].pPtPrj))
					continue;
				if (pPrecisionContur.Disjoint(WorkObstacleList.Parts[i].pPtPrj))
					continue;

				PrecisionObstacles.Parts[k] = WorkObstacleList.Parts[i];

				if (WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].NIx < 0)
				{
					PrecisionObstacles.Obstacles[l] = WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner];
					PrecisionObstacles.Obstacles[l].PartsNum = 0;
					//PrecisionObstacles.Obstacles[l].Parts = new int[WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].PartsNum];
					WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].NIx = l;
					l++;
				}

				PrecisionObstacles.Parts[k].Owner = WorkObstacleList.Obstacles[WorkObstacleList.Parts[i].Owner].NIx;
				PrecisionObstacles.Parts[k].Index = PrecisionObstacles.Obstacles[PrecisionObstacles.Parts[k].Owner].PartsNum;

				//PrecisionObstacles.Obstacles[PrecisionObstacles.Parts[k].Owner].Parts[PrecisionObstacles.Obstacles[PrecisionObstacles.Parts[k].Owner].PartsNum] = k;
				PrecisionObstacles.Obstacles[PrecisionObstacles.Parts[k].Owner].PartsNum++;

				//ARANFunctions.PrjToLocal(ptFAF, ArDir + Math.PI, PrecisionObstacles.Parts[k].pPtPrj, out x, out y);
				//PrecisionObstacles.Parts[k].Dist = x;
				//PrecisionObstacles.Parts[k].DistStar = y;

				k++;
			}

			if (k > 0)
			{
				Array.Resize<Obstacle>(ref PrecisionObstacles.Obstacles, l);
				Array.Resize<ObstacleData>(ref PrecisionObstacles.Parts, k);
			}
			else
			{
				PrecisionObstacles.Obstacles = new Obstacle[0];
				PrecisionObstacles.Parts = new ObstacleData[0];
			}

			//==========================================================================================================================
			double FAFWidth;
			LineString ls = new LineString();
			ls.Add(ARANFunctions.LocalToPrj(ptFAF, ArDir, 0.0, 100000.0));
			ls.Add(ARANFunctions.LocalToPrj(ptFAF, ArDir, 0.0, -100000.0));

			Geometry geom = pCommonContur.Intersect(IntermSecondaryArea, ls);
			if (geom.Type == GeometryType.LineString)
				FAFWidth = 0.5 * ((LineString)geom).Length;
			else
				FAFWidth = 0.5 * ((MultiLineString)geom).Length;

			//ARANFunctions.PrjToLocal(ptFAF, ArDir + Math.PI, IntermetiatObstacles.Parts[k].pPtPrj, out x, out y);

			GeometryOperators pPrimaryContur = new GeometryOperators();
			pPrimaryContur.CurrentGeometry = IntermPrimaryArea;

			m = ObstacleList.Obstacles.Length;
			int c = 10 * m;
			IntermetiatObstacles.Parts = new ObstacleData[c];

			//n = ObstacleList.Parts.Length;

			if (m > 0)
			{
				IntermetiatObstacles.Obstacles = new Obstacle[m];
				IntermetiatObstacles.Parts = new ObstacleData[c];
			}
			else
			{
				IntermetiatObstacles.Obstacles = new Obstacle[0];
				IntermetiatObstacles.Parts = new ObstacleData[0];
				return 0;
			}

			k = 0;
			l = 0;
			Aran.Geometries.MultiPoint pObstPoints;

			for (i = 0; i < m; i++)
			{
				Geometry pCurrGeom = ObstacleList.Obstacles[i].pGeomPrj;

				if (pCurrGeom.IsEmpty || pCommonContur.Disjoint(pCurrGeom))
					continue;

				pObstPoints = new MultiPoint();

				if (pCurrGeom.Type == GeometryType.Point)
				{
					if (!pPrecisionContur.Disjoint(pCurrGeom))
						continue;

					pObstPoints.Add((Point)pCurrGeom);
				}
				else
				{
					var pTmpPoints = pCommonContur.Intersect(pCurrGeom, IntermSecondaryArea);
					pObstPoints.AddMultiPoint(pTmpPoints.ToMultiPoint());

					pTmpPoints = pCommonContur.Intersect(pCurrGeom, IntermPrimaryArea);
					pObstPoints.AddMultiPoint(pTmpPoints.ToMultiPoint());

					pTmpPoints = pCommonContur.Intersect(pCurrGeom, pPrecisionArea);
					pObstPoints.AddMultiPoint(pTmpPoints.ToMultiPoint());

					RemoveSeamPoints(ref pObstPoints);
				}

				n = pObstPoints.Count;
				if (n <= 0)
					continue;

				IntermetiatObstacles.Obstacles[l] = ObstacleList.Obstacles[i];
				IntermetiatObstacles.Obstacles[l].PartsNum = n;
				//IntermetiatObstacles.Obstacles[l].Parts = new int[n];

				if (ObstacleList.Obstacles[i].pGeomPrj.Type == GeometryType.Point)
					IntermetiatObstacles.Obstacles[l].Height = ((Point)ObstacleList.Obstacles[i].pGeomPrj).Z - refHeight;
				//else
				//{
					//pZv = ObstacleList.Obstacles[i].pGeomPrj;
					//OutObstList.Obstacles[L].Height = pZv.ZMax - ptLHPrj.Z;
				//}

				int j = 0;
				for (int jr = 0; jr < n; jr++)
				{
					Point pCurrPt = pObstPoints[jr];

					if (!pPrecisionContur.Disjoint(pCurrPt))
						continue;

					//GlobalVars.gAranGraphics.DrawPointWithText(pCurrPt, -1, OutObstList.Obstacles[l].TypeName + "/" + OutObstList.Obstacles[l].UnicalName);
					//System.Windows.Forms.Application.DoEvents();

					if (k >= c)
					{
						c += m;
						Array.Resize<ObstacleData>(ref IntermetiatObstacles.Parts, c);
					}

					IntermetiatObstacles.Parts[k].pPtPrj = pCurrPt;
					IntermetiatObstacles.Parts[k].Owner = l;
					IntermetiatObstacles.Parts[k].Height = IntermetiatObstacles.Obstacles[l].Height;
					IntermetiatObstacles.Parts[k].Index = j;
					//IntermetiatObstacles.Obstacles[l].Parts[j] = k;
					j++;

					IntermetiatObstacles.Parts[k].Prima = !pPrimaryContur.Disjoint(IntermetiatObstacles.Parts[k].pPtPrj);
					IntermetiatObstacles.Parts[k].MOC = GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value;
					IntermetiatObstacles.Parts[k].fSecCoeff = 1.0;

					ARANFunctions.PrjToLocal(ptFAF, ArDir + Math.PI, IntermetiatObstacles.Parts[k].pPtPrj, out x, out y);
					IntermetiatObstacles.Parts[k].Dist = x;
					IntermetiatObstacles.Parts[k].DistStar = y;

					if (!IntermetiatObstacles.Parts[k].Prima)
					{
						//FAFWidth						//GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value
						double kf;
						y = Math.Abs(y);

						if (x < GlobalVars.SBASTransitionDistance)
						{
							double tana = (2.5 * 1852.0 - FAFWidth) / GlobalVars.SBASTransitionDistance;
							kf = 2.0 - 2.0 * y / (FAFWidth + x * tana);//GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value
						}
						else
							kf = 2.0 - 2.0 * y / (2.5 * 1852.0);

						if (kf > 1.0) kf = 1.0;
						if (kf < 0.0) kf = 0.0;

						IntermetiatObstacles.Parts[k].fSecCoeff = kf;
						IntermetiatObstacles.Parts[k].MOC = kf * GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value;
					}

					IntermetiatObstacles.Parts[k].ReqH = IntermetiatObstacles.Obstacles[l].Height + IntermetiatObstacles.Parts[k].MOC;
					k++;
				}

				//if (j > 0 && j != n)					Array.Resize<int>(ref IntermetiatObstacles.Obstacles[l].Parts, j);

				IntermetiatObstacles.Obstacles[l].PartsNum = j;
				l++;
			}

			if (k > 0)
			{
				Array.Resize<Obstacle>(ref IntermetiatObstacles.Obstacles, l);
				Array.Resize<ObstacleData>(ref IntermetiatObstacles.Parts, k);
			}
			else
			{
				IntermetiatObstacles.Obstacles = new Obstacle[0];
				IntermetiatObstacles.Parts = new ObstacleData[0];
			}

			//==========================================================================================================================
			return 0;
		}

		static MultiPoint ToMultiPoint(Geometry geom)
		{
			MultiPoint result = new MultiPoint();
			Polygon poly;
			MultiLineString mls;

			switch (geom.Type)
			{
				case GeometryType.Point:
					result.Add((Point)geom);
					break;
        case GeometryType.MultiPoint:
        case GeometryType.LineString:
		//case GeometryType.LineSegment:
        case GeometryType.Ring:
					Ring ring = (Ring )geom;

					result.AddMultiPoint((MultiPoint) ring );
					break;
        case GeometryType.Polygon:
					poly = (Polygon) geom;
					return  poly.ToMultiPoint();

        case GeometryType.MultiLineString:
					mls=(MultiLineString )geom;
					return  mls.ToMultiPoint() ;

        case GeometryType.MultiPolygon:
									MultiPolygon	mlp=(MultiPolygon	)geom;
					return  mlp.ToMultiPoint() ;

		//case GeometryType.GeometryCollection,
			//break;
			}

			return result ;
		}

		public static int AnaliseObstacles(ObstacleContainer ObstacleList, out ObstacleContainer OutObstList, Point ptLHPrj, double ArDir, D3DPolygone[] Planes)
		{
			//Dim pZv As ESRI.ArcGIS.Geometry.IZ
			//Aran.Geometries.MultiPoint pTmpPoints ;

			int m = ObstacleList.Obstacles.Length;

			if (m == 0)
			{
				OutObstList.Obstacles = new Obstacle[0];
				OutObstList.Parts = new ObstacleData[0];
				return 0;
			}

			int c, i, j, k, l, n, o, p, result;
			double x, y, z, abscissa = ArDir + Math.PI;

			Point pCurrPt;
			Geometry pCurrGeom;

			Aran.Geometries.MultiPoint pObstPoints;

			GeometryOperators pTopoOper = new GeometryOperators();
			GeometryOperators pRelation = new GeometryOperators();
			GeometryOperators pRelationFull = new GeometryOperators();

			p = Planes.Length;
			pRelationFull.CurrentGeometry = Planes[p - 1].Poly;
			OutObstList.Obstacles = new Obstacle[m];

			c = 10 * m;
			OutObstList.Parts = new ObstacleData[c];

			k = 0;
			l = 0;
			result = 0;

			for (i = 0; i < m; i++)
			{
				pCurrGeom = ObstacleList.Obstacles[i].pGeomPrj;

				if (pCurrGeom.IsEmpty || pRelationFull.Disjoint(pCurrGeom))
					continue;

				pObstPoints = new MultiPoint();

				if (pCurrGeom.Type == GeometryType.Point)
					pObstPoints.Add((Point)pCurrGeom);
				else
				{
					for (o = 0; o < p - 1; o++)
					{
						pRelation.CurrentGeometry = Planes[o].Poly;
						if (pRelation.Disjoint(pCurrGeom))
							continue;

						var pTmpPoints = pTopoOper.Intersect(pCurrGeom, Planes[o].Poly);
						pObstPoints.AddMultiPoint(pTmpPoints.ToMultiPoint());
					}

					RemoveSeamPoints(ref pObstPoints);
				}

				n = pObstPoints.Count;
				if (n <= 0)
					continue;

				OutObstList.Obstacles[l] = ObstacleList.Obstacles[i];
				OutObstList.Obstacles[l].PartsNum = n;
				//OutObstList.Obstacles[l].Parts = new int[n];

				if (ObstacleList.Obstacles[i].pGeomPrj.Type == GeometryType.Point)
					OutObstList.Obstacles[l].Height = ((Point)ObstacleList.Obstacles[i].pGeomPrj).Z - ptLHPrj.Z;
				else
				{
					//pZv = ObstacleList.Obstacles[i].pGeomPrj;
					//OutObstList.Obstacles[L].Height = pZv.ZMax - ptLHPrj.Z;
				}

				for (j = 0; j < n; j++)
				{
					pCurrPt = pObstPoints[j];

					//GlobalVars.gAranGraphics.DrawPointWithText(pCurrPt, -1, OutObstList.Obstacles[l].TypeName + "/" + OutObstList.Obstacles[l].UnicalName);
					//System.Windows.Forms.Application.DoEvents();

					if (k >= c)
					{
						c += m;
						Array.Resize<ObstacleData>(ref OutObstList.Parts, c);
					}

					OutObstList.Parts[k].pPtPrj = pCurrPt;
					OutObstList.Parts[k].Owner = l;
					OutObstList.Parts[k].Height = OutObstList.Obstacles[l].Height;
					OutObstList.Parts[k].Index = j;
					//OutObstList.Obstacles[l].Parts[j] = k;

					ARANFunctions.PrjToLocal(ptLHPrj, abscissa, OutObstList.Parts[k].pPtPrj, out x, out y);

					OutObstList.Parts[k].Dist = x;
					OutObstList.Parts[k].DistStar = y;

					//GlobalVars.gAranGraphics.DrawPointWithText(pCurrPt, -1, OutObstList.Obstacles[l].TypeName + "/" + OutObstList.Obstacles[l].UnicalName);
					//System.Windows.Forms.Application.DoEvents();

					for (o = 0; o < p - 1; o++)
					{
						pRelation.CurrentGeometry = Planes[o].Poly;
						if (pRelation.Disjoint(OutObstList.Parts[k].pPtPrj))
							continue;

						z = Planes[o].Plane.A * x + Planes[o].Plane.B * y + Planes[o].Plane.D;

						OutObstList.Parts[k].hSurface = z;
						OutObstList.Parts[k].hPenet = OutObstList.Parts[k].Height - z;

						OutObstList.Parts[k].Plane = o;
						OutObstList.Parts[k].minZPlane = z;

						if (OutObstList.Parts[k].hPenet > 0.0)
							result++;
						k++;
						break;
					}
				}

				l++;
			}

			if (k >= 0)
			{
				Array.Resize<Obstacle>(ref OutObstList.Obstacles, l);
				Array.Resize<ObstacleData>(ref OutObstList.Parts, k);
			}
			else
			{
				OutObstList.Obstacles = new Obstacle[0];
				OutObstList.Parts = new ObstacleData[0];
			}

			return result;
		}

		public static void CalcEffectiveHeights(ref ObstacleContainer ObstList, double fGPAngle, double fMAPDG, double fRDH, double ZSurfaceOrigin, D3DPolygone[] OASWorkPlanes, bool ILSObs = false)
		{
			double CoTanZ = 1.0 / fMAPDG;

			double TanGPA = System.Math.Tan(fGPAngle);
			double CoTanGPA = 1.0 / TanGPA;

			int n = ObstList.Parts.Length;

			for (int i = 0; i < n; i++)
			{
				if (ILSObs)
				{
					if (ObstList.Parts[i].Dist < 0)
						ObstList.Parts[i].ReqH = fRDH;
					else
						ObstList.Parts[i].ReqH = ObstList.Parts[i].Dist * TanGPA + fRDH;

				}
				else switch ((eOAS)ObstList.Parts[i].Plane)
					{
						case eOAS.ZeroPlane:
							ObstList.Parts[i].ReqH = fRDH;
							break;
						case eOAS.ZPlane:
							ObstList.Parts[i].ReqH = 0.0;
							break;
						case eOAS.WPlane:
						case eOAS.XlPlane:
						case eOAS.XrPlane:
							ObstList.Parts[i].ReqH = OASWorkPlanes[(int)ObstList.Parts[i].Plane].Plane.A * ObstList.Parts[i].Dist +
													OASWorkPlanes[(int)ObstList.Parts[i].Plane].Plane.B * ObstList.Parts[i].DistStar +
													OASWorkPlanes[(int)ObstList.Parts[i].Plane].Plane.D + GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value;
							break;
						case eOAS.YlPlane:
						case eOAS.YrPlane:
							if (ObstList.Parts[i].Dist < 0)
								ObstList.Parts[i].ReqH = fRDH;
							else
								ObstList.Parts[i].ReqH = ObstList.Parts[i].Dist * TanGPA + fRDH;
							break;
					}

				if (ObstList.Parts[i].ReqH < GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value)
					ObstList.Parts[i].ReqH = GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value;

				double fTmp = (ObstList.Parts[i].Height * CoTanZ + (ZSurfaceOrigin + ObstList.Parts[i].Dist)) / (CoTanZ + CoTanGPA);
				ObstList.Parts[i].EffectiveHeight = fTmp;

				//if( ObstList[i].Dist < -ZSurfaceOrigin )
				if (ObstList.Parts[i].EffectiveHeight < ObstList.Parts[i].Height)
					ObstList.Parts[i].Flags = 1;
				else
					ObstList.Parts[i].Flags = 0;
			}
		}

		public static double StartPointDist(D3DPolygone A, D3DPolygone B, double hFAP, double FAPDist)
		{
			double D = Common.Det2(A.Plane.A, A.Plane.B, B.Plane.A, B.Plane.B);
			if (D == 0.0)
				return -1.0;

			double dX = Common.Det2(-(A.Plane.D + A.Plane.C * hFAP), A.Plane.B, -(B.Plane.D + B.Plane.C * hFAP), B.Plane.B);

			return System.Math.Abs(dX / D) - FAPDist;
		}

		public static void SortIntervals(ref Interval[] Intervals, bool RightSide = false)
		{
			Interval Tmp;

			int n = Intervals.Length;

			for (int i = 0; i < n - 1; i++)
			{
				for (int j = i + 1; j < n; j++)
				{
					if (RightSide)
					{
						if (Intervals[i].Max > Intervals[j].Max)
						{
							Tmp = Intervals[i];
							Intervals[i] = Intervals[j];
							Intervals[j] = Tmp;
						}
					}
					else if (Intervals[i].Min > Intervals[j].Min)
					{
						Tmp = Intervals[i];
						Intervals[i] = Intervals[j];
						Intervals[j] = Tmp;
					}
				}
			}
		}

		public static void SortByDist(ref ObstacleContainer A, int iLo, int iHi)
		{
			int Lo = iLo;
			int Hi = iHi;
			double midValue = A.Parts[(Lo + Hi) / 2].Dist;

			do
			{
				while (A.Parts[Lo].Dist < midValue)
					Lo++;

				while (A.Parts[Hi].Dist > midValue)
					Hi--;

				if (Lo <= Hi)
				{
					ObstacleData T = A.Parts[Lo];
					A.Parts[Lo] = A.Parts[Hi];
					A.Parts[Hi] = T;
					Lo++;
					Hi--;
				}
			}
			while (Lo <= Hi);

			if (Hi > iLo) SortByDist(ref A, iLo, Hi);
			if (Lo < iHi) SortByDist(ref A, Lo, iHi);
		}

		public static void SortByTurnDist(ref ObstacleContainer A, int iLo, int iHi)
		{
			int Lo = iLo;
			int Hi = iHi;
			double midValue = A.Parts[(Lo + Hi) / 2].TurnDistL;

			do
			{
				while (A.Parts[Lo].TurnDistL < midValue)
					Lo++;

				while (A.Parts[Hi].TurnDistL > midValue)
					Hi--;

				if (Lo <= Hi)
				{
					ObstacleData T = A.Parts[Lo];
					A.Parts[Lo] = A.Parts[Hi];
					A.Parts[Hi] = T;
					Lo++;
					Hi--;
				}
			}
			while (Lo <= Hi);

			if (Hi > iLo) SortByTurnDist(ref A, iLo, Hi);
			if (Lo < iHi) SortByTurnDist(ref A, Lo, iHi);
		}

		public static void SortByReqH(ref ObstacleContainer A, int iLo, int iHi)
		{
			int Lo = iLo;
			int Hi = iHi;
			double midValue = A.Parts[(Lo + Hi) / 2].ReqH;

			do
			{
				while (A.Parts[Lo].ReqH > midValue)
					Lo++;

				while (A.Parts[Hi].ReqH < midValue)
					Hi--;

				if (Lo <= Hi)
				{
					ObstacleData T = A.Parts[Lo];
					A.Parts[Lo] = A.Parts[Hi];
					A.Parts[Hi] = T;
					Lo++;
					Hi--;
				}
			}
			while (Lo <= Hi);

			if (Hi > iLo) SortByReqH(ref A, iLo, Hi);
			if (Lo < iHi) SortByReqH(ref A, Lo, iHi);
		}

		public static void SortByReqOCH(ref ObstacleContainer A, int iLo, int iHi)
		{
			int Lo = iLo;
			int Hi = iHi;
			double midValue = A.Parts[(Lo + Hi) / 2].ReqOCH;

			do
			{
				while (A.Parts[Lo].ReqOCH > midValue)
					Lo++;

				while (A.Parts[Hi].ReqOCH < midValue)
					Hi--;

				if (Lo <= Hi)
				{
					ObstacleData T = A.Parts[Lo];
					A.Parts[Lo] = A.Parts[Hi];
					A.Parts[Hi] = T;
					Lo++;
					Hi--;
				}
			}
			while (Lo <= Hi);

			if (Hi > iLo) SortByReqOCH(ref A, iLo, Hi);
			if (Lo < iHi) SortByReqOCH(ref A, Lo, iHi);
		}

		public static void SortByfSort(ref ObstacleContainer A, int iLo, int iHi)
		{
			int Lo = iLo;
			int Hi = iHi;
			double midValue = A.Parts[(Lo + Hi) / 2].fSort;

			do
			{
				while (A.Parts[Lo].fSort > midValue)
					Lo++;

				while (A.Parts[Hi].fSort < midValue)
					Hi--;

				if (Lo <= Hi)
				{
					ObstacleData T = A.Parts[Lo];
					A.Parts[Lo] = A.Parts[Hi];
					A.Parts[Hi] = T;
					Lo++;
					Hi--;
				}
			}
			while (Lo <= Hi);

			if (Hi > iLo) SortByfSort(ref A, iLo, Hi);
			if (Lo < iHi) SortByfSort(ref A, Lo, iHi);
		}

		public static void Sort(ref ObstacleContainer A, int SortIx)
		{
			int Lo = 0;
			int Hi = A.Parts.Length - 1;

			if (Lo >= Hi) return;

			switch (SortIx)
			{
				case 0:
					SortByDist(ref A, Lo, Hi);
					break;
				case 1:
					SortByTurnDist(ref A, Lo, Hi);
					break;
				case 2:
					SortByReqH(ref A, Lo, Hi);
					break;
				case 3:
					SortByReqOCH(ref A, Lo, Hi);
					break;
				//case 4:
				//	SortByfTmp(ref A, Lo, Hi);
				//	break;
				case 100:
					SortByfSort(ref A, Lo, Hi);
					break;
				//case 101:
				//	SortBysSort(ref A, Lo, Hi);
				//	break;
			}
		}

		public static double PosibleMaxIFaltitude(double IF2FAPdistance, double InterDescGrad, double maxPlannedAngle, double hFAP, double IFIAS)
		{
			double result = IF2FAPdistance * InterDescGrad + hFAP;
			if (InterDescGrad <= 0.0)
				return result;

			double TAS = ARANMath.IASToTASForRnav(IFIAS, result, GlobalVars.CurrADHP.ISAtC);
			double RTurn = ARANMath.BankToRadius(ARANMath.DegToRad(25), TAS);
			double realDist = RTurn * Math.Tan(0.5 * maxPlannedAngle) + GlobalVars.SBASTransitionDistance;

			while (realDist > IF2FAPdistance + ARANMath.EpsilonDistance)
			{
				double dH = (realDist - IF2FAPdistance) * InterDescGrad;

				result -= dH;
				if (result < hFAP)
					return hFAP;

				TAS = ARANMath.IASToTASForRnav(IFIAS, result, GlobalVars.CurrADHP.ISAtC);
				RTurn = ARANMath.BankToRadius(ARANMath.DegToRad(25), TAS);

				realDist = RTurn * Math.Tan(0.5 * maxPlannedAngle) + GlobalVars.SBASTransitionDistance;
			}

			return result;
		}

		public static double VORFIXTolerArea(Point ptVor, double Aztin, double AbsH, out Polygon TolerArea)
		{
			double vORFIXTolerAreaReturn;

			Point ptCurr;
			double fTmp;
			double fTmpH = AbsH - ptVor.Z;
			double R = fTmpH * Math.Tan(GlobalVars.navaidsConstants.VOR.ConeAngle);

			TolerArea = new Polygon();
			TolerArea.ExteriorRing = new Ring();

			Point ptTmp = ARANFunctions.PointAlongPlane(ptVor, Aztin - (ARANMath.C_PI_2 + GlobalVars.navaidsConstants.VOR.TrackAccuracy), GlobalVars.navaidsConstants.VOR.LateralDeviationCoef * fTmpH);

			ptCurr = ARANFunctions.CircleVectorIntersect(ptVor, R, ptTmp, Aztin - GlobalVars.navaidsConstants.VOR.TrackAccuracy, out fTmp);
			TolerArea.ExteriorRing.Add(ptCurr);
			ptCurr = ARANFunctions.CircleVectorIntersect(ptVor, R, ptTmp, ARANMath.C_PI + Aztin - GlobalVars.navaidsConstants.VOR.TrackAccuracy, out fTmp);
			TolerArea.ExteriorRing.Add(ptCurr);
			ptTmp = ARANFunctions.PointAlongPlane(ptVor, Aztin + ARANMath.C_PI_2 + GlobalVars.navaidsConstants.VOR.TrackAccuracy, GlobalVars.navaidsConstants.VOR.LateralDeviationCoef * fTmpH);
			ptCurr = ARANFunctions.CircleVectorIntersect(ptVor, R, ptTmp, ARANMath.C_PI + Aztin + GlobalVars.navaidsConstants.VOR.TrackAccuracy, out fTmp);
			TolerArea.ExteriorRing.Add(ptCurr);

			ptCurr = ARANFunctions.CircleVectorIntersect(ptVor, R, ptTmp, Aztin + GlobalVars.navaidsConstants.VOR.TrackAccuracy, out fTmp);
			TolerArea.ExteriorRing.Add(ptCurr);

			vORFIXTolerAreaReturn = R;
			return vORFIXTolerAreaReturn;
		}

		public static double NDBFIXTolerArea(Point ptNDB, double Aztin, double AbsH, out Polygon TolerArea)
		{
			double nDBFIXTolerAreaReturn;
			double R;
			double qN;
			double fTmp;
			double fTmpH;

			Point ptTmp;
			Point ptCurr;

			fTmpH = AbsH - ptNDB.Z;
			R = fTmpH * Math.Tan(GlobalVars.navaidsConstants.NDB.ConeAngle);

			TolerArea = new Polygon();
			TolerArea.ExteriorRing = new Ring();

			qN = R * Math.Sin(ARANMath.DegToRad(GlobalVars.navaidsConstants.NDB.Entry2ConeAccuracy));
			ptTmp = ARANFunctions.PointAlongPlane(ptNDB, Aztin - ARANMath.C_PI_2, qN + Math.Sqrt(R * R - qN * qN) * Math.Tan(GlobalVars.navaidsConstants.NDB.TrackAccuracy));
			ptCurr = ARANFunctions.CircleVectorIntersect(ptNDB, R, ptTmp, Aztin - GlobalVars.navaidsConstants.NDB.TrackAccuracy, out fTmp);
			TolerArea.ExteriorRing.Add(ptCurr);
			ptCurr = ARANFunctions.CircleVectorIntersect(ptNDB, R, ptTmp, ARANMath.C_PI + Aztin - GlobalVars.navaidsConstants.NDB.TrackAccuracy, out fTmp);
			TolerArea.ExteriorRing.Add(ptCurr);
			ptTmp = ARANFunctions.PointAlongPlane(ptNDB, Aztin + ARANMath.C_PI_2, qN + Math.Sqrt(R * R - qN * qN) * Math.Tan(GlobalVars.navaidsConstants.NDB.TrackAccuracy));
			ptCurr = ARANFunctions.CircleVectorIntersect(ptNDB, R, ptTmp, ARANMath.C_PI + Aztin + GlobalVars.navaidsConstants.NDB.TrackAccuracy, out fTmp);
			TolerArea.ExteriorRing.Add(ptCurr);
			ptCurr = ARANFunctions.CircleVectorIntersect(ptNDB, R, ptTmp, Aztin + GlobalVars.navaidsConstants.NDB.TrackAccuracy, out fTmp);
			TolerArea.ExteriorRing.Add(ptCurr);

			nDBFIXTolerAreaReturn = R;
			return nDBFIXTolerAreaReturn;
		}

		//public static bool ShowSaveDialog(out string FileName, out string FileTitle)
		//{
		//	System.Windows.Forms.SaveFileDialog SaveDialog1 = new System.Windows.Forms.SaveFileDialog();

		//	//string ProjectPath = GlobalVars.GetMapFileName();
		//	//int pos = ProjectPath.LastIndexOf('\\');
		//	//int pos2 = ProjectPath.LastIndexOf('.');

		//	//SaveDialog1.DefaultExt = "";
		//	//SaveDialog1.InitialDirectory = ProjectPath.Substring(0, pos);
		//	//SaveDialog1.Title = Properties.Resources.str00511;
		//	//SaveDialog1.FileName = ProjectPath.Substring(0, pos2 - 1) + ".htm";

		//	SaveDialog1.FileName = "";
		//	SaveDialog1.Filter = "PANDA Report File (*.htm)|*.htm|All Files (*.*)|*.*";

		//	FileTitle = "";
		//	FileName = "";

		//	if (SaveDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
		//	{
		//		FileName = SaveDialog1.FileName;

		//		int pos = FileName.LastIndexOf('.');
		//		if (pos > 0)
		//			FileName = FileName.Substring(0, pos - 1);

		//		FileTitle = FileName;
		//		int pos2 = FileTitle.LastIndexOf('\\');
		//		if (pos2 > 0)
		//			FileTitle = FileTitle.Substring(pos2);

		//		return true;
		//	}

		//	return false;
		//}

		internal static string Degree2String(double X, Degree2StringMode Mode)
		{
			string sSign = "", sResult = "", sTmp;
			double xDeg, xMin, xIMin, xSec;
			bool lSign = false;

			if (Mode == Degree2StringMode.DMSLat)
			{
				lSign = Math.Sign(X) < 0;
				if (lSign)
					X = -X;

				xDeg = System.Math.Floor(X);
				xMin = (X - xDeg) * 60.0;
				xIMin = System.Math.Floor(xMin);
				xSec = (xMin - xIMin) * 60.0;	//		xSec = System.Math.Round((xMin - xIMin) * 60.0, 2);
				if (xSec >= 60.0)
				{
					xSec = 0.0;
					xIMin++;
				}

				if (xIMin >= 60.0)
				{
					xIMin = 0.0;
					xDeg++;
				}

				sTmp = xDeg.ToString("00");
				sResult = sTmp + "°";

				sTmp = xIMin.ToString("00");
				sResult = sResult + sTmp + "'";

				sTmp = xSec.ToString("00.00");
				sResult = sResult + sTmp + @"""";

				return sResult + (lSign ? "S" : "N");
			}

			if (Mode >= Degree2StringMode.DMSLon)
			{
				X = NativeMethods.Modulus(X);
				lSign = X > 180.0;
				if (lSign) X = 360.0 - X;

				xDeg = System.Math.Floor(X);
				xMin = (X - xDeg) * 60.0;
				xIMin = System.Math.Floor(xMin);
				xSec = (xMin - xIMin) * 60.0;
				if (xSec >= 60.0)
				{
					xSec = 0.0;
					xIMin++;
				}

				if (xIMin >= 60.0)
				{
					xIMin = 0.0;
					xDeg++;
				}

				sTmp = xDeg.ToString("000");
				sResult = sTmp + "°";

				sTmp = xIMin.ToString("00");
				sResult = sResult + sTmp + "'";

				sTmp = xSec.ToString("00.00");
				sResult = sResult + sTmp + @"""";

				return sResult + (lSign ? "W" : "E");
			}

			if (System.Math.Sign(X) < 0) sSign = "-";
			X = NativeMethods.Modulus(System.Math.Abs(X));

			switch (Mode)
			{
				case Degree2StringMode.DD:
					return sSign + X.ToString("#0.00##") + "°";
				case Degree2StringMode.DM:
					if (System.Math.Sign(X) < 0) sSign = "-";
					X = NativeMethods.Modulus(System.Math.Abs(X));

					xDeg = System.Math.Floor(X);
					xMin = (X - xDeg) * 60.0;
					if (xMin >= 60)
					{
						X++;
						xMin = 0;
					}

					sResult = sSign + xDeg.ToString() + "°";

					sTmp = xMin.ToString("00.00##");
					return sResult + sTmp + "'";
				case Degree2StringMode.DMS:
					if (System.Math.Sign(X) < 0) sSign = "-";
					X = NativeMethods.Modulus(System.Math.Abs(X));

					xDeg = System.Math.Floor(X);
					xMin = (X - xDeg) * 60.0;
					xIMin = System.Math.Floor(xMin);
					xSec = (xMin - xIMin) * 60.0;
					if (xSec >= 60.0)
					{
						xSec = 0.0;
						xIMin++;
					}

					if (xIMin >= 60.0)
					{
						xIMin = 0.0;
						xDeg++;
					}

					sResult = sSign + xDeg.ToString() + "°";

					sTmp = xIMin.ToString("00");
					sResult = sResult + sTmp + "'";

					sTmp = xSec.ToString("00.00");
					return sResult + sTmp + @"""";
			}
			return sResult;
		}

		internal static void shall_SortsSort(ObstacleContainer obstacles)
		{
			int lastRow = obstacles.Parts.GetUpperBound(0);
			if (lastRow < 0)
				return;

			int firstRow = obstacles.Parts.GetLowerBound(0);
			int numRows = lastRow - firstRow + 1;
			if (numRows == 0)
				return;

			int gapSize = 0;
			do
				gapSize = gapSize * 3 + 1;
			while (gapSize <= numRows);

			do
			{
				gapSize = gapSize / 3;
				for (int i = gapSize + firstRow; i <= lastRow; i++)
				{
					int curPos = i;
					ObstacleData tmpVal = obstacles.Parts[i];

					while (String.Compare(obstacles.Parts[curPos - gapSize].sSort, tmpVal.sSort) > 0)
					{
						obstacles.Parts[curPos] = obstacles.Parts[curPos - gapSize];
						curPos = curPos - gapSize;
						if (curPos - gapSize < firstRow)
							break;
					}
					obstacles.Parts[curPos] = tmpVal;
				}
			}
			while (gapSize > 1);
		}

		internal static void shall_SortsSortD(ObstacleContainer obstacles)
		{
			int lastRow = obstacles.Parts.GetUpperBound(0);
			if (lastRow < 0)
				return;

			int firstRow = obstacles.Parts.GetLowerBound(0);
			int numRows = lastRow - firstRow + 1;
			if (numRows == 0)
				return;

			int gapSize = 0;
			do
				gapSize = gapSize * 3 + 1;
			while (gapSize <= numRows);

			do
			{
				gapSize = gapSize / 3;
				for (int i = gapSize + firstRow; i <= lastRow; i++)
				{
					int curPos = i;
					ObstacleData tmpVal = obstacles.Parts[i];

					while (String.Compare(obstacles.Parts[curPos - gapSize].sSort, tmpVal.sSort) < 0)
					{
						obstacles.Parts[curPos] = obstacles.Parts[curPos - gapSize];
						curPos = curPos - gapSize;
						if (curPos - gapSize < firstRow)
							break;
					}
					obstacles.Parts[curPos] = tmpVal;
				}
			}
			while (gapSize > 1);
		}

		internal static void shall_SortfSort(ObstacleContainer obstacles)
		{
			int lastRow = obstacles.Parts.GetUpperBound(0);
			if (lastRow < 0)
				return;

			int firstRow = obstacles.Parts.GetLowerBound(0);
			int numRows = lastRow - firstRow + 1;
			if (numRows == 0)
				return;

			int gapSize = 0;
			do
				gapSize = gapSize * 3 + 1;
			while (gapSize <= numRows);

			do
			{
				gapSize = gapSize / 3;
				for (int i = gapSize + firstRow; i <= lastRow; i++)
				{
					int curPos = i;
					ObstacleData tmpVal = obstacles.Parts[i];

					while (obstacles.Parts[curPos - gapSize].fSort > tmpVal.fSort)
					{
						obstacles.Parts[curPos] = obstacles.Parts[curPos - gapSize];
						curPos = curPos - gapSize;
						if (curPos - gapSize < firstRow)
							break;
					}
					obstacles.Parts[curPos] = tmpVal;
				}
			}
			while (gapSize > 1);
		}

		internal static void shall_SortfSortD(ObstacleContainer obstacles)
		{
			int lastRow = obstacles.Parts.GetUpperBound(0);
			if (lastRow < 0)
				return;

			int firstRow = obstacles.Parts.GetLowerBound(0);
			int numRows = lastRow - firstRow + 1;
			if (numRows == 0)
				return;


			int gapSize = 0;
			do
				gapSize = gapSize * 3 + 1;
			while (gapSize <= numRows);

			do
			{
				gapSize = gapSize / 3;
				for (int i = gapSize + firstRow; i <= lastRow; i++)
				{
					int curPos = i;
					ObstacleData tmpVal = obstacles.Parts[i];
					while (obstacles.Parts[curPos - gapSize].fSort < tmpVal.fSort)
					{
						obstacles.Parts[curPos] = obstacles.Parts[curPos - gapSize];
						curPos = curPos - gapSize;
						if (curPos - gapSize < firstRow)
							break;
					}
					obstacles.Parts[curPos] = tmpVal;
				}
			}
			while (gapSize > 1);
		}

        public static bool ShowSaveDialog(out string FileName, out string FileTitle)
        {
            System.Windows.Forms.SaveFileDialog SaveDialog1 = new System.Windows.Forms.SaveFileDialog();

            //string ProjectPath = GlobalVars.GetMapFileName();
            //int pos = ProjectPath.LastIndexOf('\\');
            //int pos2 = ProjectPath.LastIndexOf('.');

            //SaveDialog1.DefaultExt = "";
            //SaveDialog1.InitialDirectory = ProjectPath.Substring(0, pos);
            //SaveDialog1.Title = Properties.Resources.str00511;
            //SaveDialog1.FileName = ProjectPath.Substring(0, pos2 - 1) + ".htm";

            SaveDialog1.FileName = "";
            SaveDialog1.Filter = "PANDA Report File (*.htm)|*.htm|All Files (*.*)|*.*";

            FileTitle = "";
            FileName = "";

            if (SaveDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileName = SaveDialog1.FileName;

                int pos = FileName.LastIndexOf('.');
                if (pos > 0)
                    FileName = FileName.Substring(0, pos - 1);

                FileTitle = FileName;
                int pos2 = FileTitle.LastIndexOf('\\');
                if (pos2 > 0)
                    FileTitle = FileTitle.Substring(pos2);

                return true;
            }

            return false;
        }

		public static double MinFlybyDist(double BankInRadian, double IASInMetrsInSec, double derElev, double dT, double grd, double plannedAng, double ATT)
		{
			double hAbovDer = GlobalVars.constants.Pansops[ePANSOPSData.dpH_abv_DER].Value;		//5 m
			double hMinHeight = GlobalVars.constants.Pansops[ePANSOPSData.dpNGui_Ar1].Value;	//120 m
			double maxPDG = GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;
			double kTurn = Math.Tan(0.5 * plannedAng);
			double hM = hMinHeight - hAbovDer;													//115 m

			double Altitude = derElev + hM;
			double RTurn = ARANMath.BankToRadiusForRnav(BankInRadian, IASInMetrsInSec, Altitude, dT);

			double EPT_h = RTurn * kTurn;
			double LGRD_h = hM / grd + ATT;

			int io = 100;
			while (io >= 0)
			{
				double EPT_Old = EPT_h;

				Altitude = derElev + hAbovDer + (EPT_h + LGRD_h) * maxPDG;

				RTurn = ARANMath.BankToRadiusForRnav(BankInRadian, IASInMetrsInSec, Altitude, dT);
				EPT_h = RTurn * kTurn;

				if (Math.Abs(EPT_h - EPT_Old) < ARANMath.EpsilonDistance)
					break;
				io--;
			}
			return EPT_h + ATT;
		}

		public static Interval CalcSpiralStartPoint(LineString LinePoint, ObstacleData detObs, double coef, double r0, double ArDir, int TurnDir)
		{
			int Offset = 0;
			int n = LinePoint.Count - Offset;

			Point[] BasePoints = new Point[n];

			//GlobalVars.gAranGraphics.DrawLineString(LinePoint, 255, 2);
			//GlobalVars.gAranGraphics.DrawPointWithText(detObs.pPtPrj, -1,"O1");
			//System.Windows.Forms.Application.DoEvents();

			LineString pLine = new LineString();
			GeometryOperators pProxi = new GeometryOperators();
			pProxi.CurrentGeometry = pLine;

			for (int i = 0; i < n; i++)
			{
				BasePoints[i] = LinePoint[i + Offset];
				if (i == n - 1)
					BasePoints[i].M = BasePoints[i - 1].M;
				else
					BasePoints[i].M = ARANFunctions.ReturnAngleInRadians(LinePoint[i + Offset], LinePoint[i + Offset + 1]);
			}

			Point ptCnt, ptTurn = null;

			int iMin = -1;
			double hTMin = GlobalVars.MaxModelRadius;
			double MaxTheta = ARANFunctions.SpiralTouchAngle(r0, coef, ArDir, ArDir + ARANMath.C_PI_2 * TurnDir, (SideDirection)TurnDir);

			if (MaxTheta > Math.PI)
				MaxTheta = 2.0 * Math.PI - MaxTheta;

			for (int i = 0; i < n - 1; i++)
			{
				Point ptCurr = detObs.pPtPrj;
				SideDirection Side = ARANMath.SideDef(BasePoints[i], BasePoints[i].M, ptCurr);
				double Alpha = ArDir - ARANMath.C_PI_2 * (int)Side;

				if (System.Math.Abs(System.Math.Sin(Alpha - BasePoints[i].M)) <= ARANMath.EpsilonRadian)
					continue;

				Point ptTmp = ARANFunctions.LocalToPrj(BasePoints[i], ArDir + ARANMath.C_PI_2 * (int)Side, r0);
				SideDirection Side1 = ARANMath.SideDef(ptTmp, BasePoints[i].M, ptCurr);

				//while(true)
				//System.Windows.Forms.Application.DoEvents();

				//GlobalVars.gAranGraphics.DrawPointWithText(ptTmp, -1, "ptTmp_01");
				//System.Windows.Forms.Application.DoEvents();

				double Dist = ARANFunctions.Point2LineDistancePrj(ptCurr, ptTmp, BasePoints[i].M);

				double dAlpha = ARANMath.SubtractAngles(Alpha, BasePoints[i].M);
				double Theta = 0.5 * MaxTheta;
				double fTmp;

				do
				{
					fTmp = Theta;
					double ASinAlpha = Dist / (r0 + Theta * coef);

					if (System.Math.Abs(ASinAlpha) <= 1.0)
						Theta = dAlpha + (int)Side1 * TurnDir * Math.Asin(ASinAlpha);
					else
					{
						Theta = MaxTheta;
						break;
					}
				}
				while (System.Math.Abs(fTmp - Theta) > ARANMath.EpsilonRadian);

				fTmp = System.Math.Sin(Theta) * (r0 + Theta * coef);
				double hT;
				if (Theta > MaxTheta)
				{
					hT = System.Math.Sin(MaxTheta) * (r0 + MaxTheta * coef);
					fTmp = (hT - fTmp);
					Theta = MaxTheta;
				}
				else
				{
					hT = fTmp;
					fTmp = 0.0;
				}

				Point ptTmp2 = ARANFunctions.LocalToPrj(ptCurr, ArDir + Math.PI, hT + fTmp);
				Geometry geom = ARANFunctions.LineLineIntersect(ptTmp2, ArDir + ARANMath.C_PI_2, ptTmp, BasePoints[i].M);
				ptCnt = (Point)geom;
				ptTmp = ARANFunctions.LocalToPrj(ptCnt, ArDir - ARANMath.C_PI_2 * TurnDir, r0);

				//GlobalVars.gAranGraphics.DrawPointWithText(ptTmp2, -1, "ptTmp_02");
				//GlobalVars.gAranGraphics.DrawPointWithText(ptCnt, -1, "ptCnt");
				//System.Windows.Forms.Application.DoEvents();

				pLine.Clear();
				pLine.Add(BasePoints[i]);
				pLine.Add(BasePoints[i + 1]);

				//fTmp = pProxi.GetDistance(ptTmp);
				fTmp = pProxi.GetDistance(ptTmp, pLine);

				if (fTmp >= ARANMath.EpsilonDistance)
					continue;

				if (hT >= hTMin)
					continue;

				hTMin = hT;
				iMin = i;
				ptTurn = ptTmp;
				ptTurn.M = Theta;
				ptTurn.Z = detObs.Dist - hTMin;
				if (ptTurn.Z < 0.0)
					ptTurn.Z = 0.0;
			}

			Interval Result = default(Interval);

			if (iMin > -1)
			{
				Result.Min = ptTurn.Z;
				Result.Max = ptTurn.M;
			}
			else
			{
				Result.Min = -9999.0;
				Result.Max = -9999.0;
			}

			return Result;
		}

		public static LineString ReturnPolygonPartAsPolyline(MultiPolygon pPolygon, Point ptCenter, double CLDir, int Turn)
		{
			LineString Result = new LineString();
			if (pPolygon.IsEmpty)
				return Result;

			Ring pTmpPoly = ReArrangeRing(pPolygon[0].ExteriorRing, ptCenter, CLDir);

			int n = pTmpPoly.Count;

			for (int i = 0; i < n; i++)
			{
				double x, y;
				ARANFunctions.PrjToLocal(ptCenter, CLDir, pTmpPoly[i], out x, out y);
				if (y * Turn > 0.0)
					Result.Add(pTmpPoly[i]);
			}

			if (Turn > 0)
				Result.Reverse();

			return Result;
		}

		public static Ring RemoveAgnails(Ring pPolygon)
		{
			Ring result = (Ring)pPolygon.Clone();
			result.Close();

			int n = result.Count - 1;

			if (n <= 3) return result;

			result.Remove(n);

			int j = 0;
			while (j < n)
			{
				if (n < 4)
					break;

				int k = (j + 1) % n;
				int l = (j + 2) % n;

				double dX0 = result[k].X - result[j].X;
				double dY0 = result[k].Y - result[j].Y;

				double dX1 = result[l].X - result[k].X;
				double dY1 = result[l].Y - result[k].Y;

				double dl = dX1 * dX1 + dY1 * dY1;

				if (dl < 0.00001)
				{
					result.Remove(k);
					n--;
					if (j >= n) j = n - 1;
				}
				else if (dY0 != 0.0)
				{
					if (dY1 != 0.0)
					{
						if (System.Math.Abs(dX0 / dY0 - dX1 / dY1) < 0.0001)
						{
							result.Remove(k);
							n--;
							j = (j - 2) % n;
							if (j < 0) j = 0; //j += n;
						}
						else
							j++;
					}
					else
						j++;
				}
				else if (dX0 != 0.0)
				{
					if (dX1 != 0.0)
					{
						if (System.Math.Abs(dY0 / dX0 - dY1 / dX1) < 0.0001)
						{
							result.Remove(k);
							n--;
							j = (j - 2) % n;
							if (j < 0) j = 0; //j += n;
						}
						else
							j++;
					}
					else
						j++;
				}
				else
					j++;
			}

			result.Close();

			return result;
		}

		public static Polygon RemoveAgnails(Polygon pPolygon)
		{
			Polygon result = new Polygon();
			result.ExteriorRing = RemoveAgnails(pPolygon.ExteriorRing);

			int n = pPolygon.InteriorRingList.Count;

			for (int i = 0; i < n; i++)
				result.InteriorRingList.Add(RemoveAgnails(pPolygon.InteriorRingList[i]));

			return result;
		}

		internal static MultiPolygon RemoveAgnails(MultiPolygon pMultiPolygon)
		{
			MultiPolygon result = new MultiPolygon();
			int n = pMultiPolygon.Count;

			for (int i = 0; i < n; i++)
				result.Add(RemoveAgnails(pMultiPolygon[i]));

			return result;
		}

		internal static Ring ReArrangeRing(Ring ring, Point ptCenter, double CLDir, bool bFlag = false)
		{
			const double epsilon = 0.00001;

			MultiPoint pPoints = new MultiPoint();
			pPoints.AddMultiPoint(ring);

			if (ARANFunctions.ReturnDistanceInMeters(pPoints[0], pPoints[pPoints.Count - 1]) == 0.0)
				pPoints.Remove(pPoints.Count - 1);

			if (pPoints.Count < 3)
				return new Ring();

			double x0 = double.MinValue, y0, x1, y1;
			int i, iStart = -1, n = pPoints.Count;

			for (i = 0; i < n; i++)
			{
				//GlobalVars.gAranGraphics.DrawPointWithText(pPoints[i], -1, "pt-"+i.ToString());

				ARANFunctions.PrjToLocal(ptCenter, CLDir, pPoints[i], out x1, out y1);
				if (y1 > 0.0 && x1 > x0)
				{
					y0 = y1;
					x0 = x1;
					iStart = i;
				}
			}

			//GlobalVars.gAranGraphics.DrawRing(ring, -1, eFillStyle.sfsBackwardDiagonal);
			//while(true)
			//System.Windows.Forms.Application.DoEvents();

			//if (bFlag)
			//{
			//	if (iStart == 0)
			//		iStart = n - 1;
			//	else
			//		iStart = (iStart - 1) % n;
			//}

			double dX0 = pPoints[1].X - pPoints[0].X;
			double dY0 = pPoints[1].Y - pPoints[0].Y;

			i = 1;
			while (i < n)
			{
				int j = (i + 1) % n;
				double dX1 = pPoints[j].X - pPoints[i].X;
				double dY1 = pPoints[j].Y - pPoints[i].Y;
				double dl = ARANFunctions.ReturnDistanceInMeters(pPoints[j], pPoints[i]);

				if (dl < epsilon)
				{
					pPoints.Remove(i);
					n--;
					j = (i + 1) % n;

					if (i <= iStart)
						iStart--;

					dX1 = dX0;
					dY1 = dY0;
				}
				else if (i == iStart)
					i++;
				else if (System.Math.Abs(dY0) > System.Math.Abs(dX0))
				{
					if (dY0 != 0.0 && dY1 != 0.0)
					{
						if (System.Math.Abs(System.Math.Abs(dX0 / dY0) - System.Math.Abs(dX1 / dY1)) < epsilon)
						{
							pPoints.Remove(i);
							n--;
							j = (i + 1) % n;

							if (i <= iStart)
								iStart--;

							dX1 = dX0;
							dY1 = dY0;
						}
						else
							i++;
					}
					else
						i++;
				}
				else if (dX0 != 0.0 && dX1 != 0.0)
				{
					if (System.Math.Abs(System.Math.Abs(dY0 / dX0) - System.Math.Abs(dY1 / dX1)) < epsilon)
					{
						pPoints.Remove(i);
						n--;
						j = (i + 1) % n;

						if (i <= iStart)
							iStart--;

						dX1 = dX0;
						dY1 = dY0;
					}
					else
						i++;
				}
				else
					i++;

				dX0 = dX1;
				dY0 = dY1;
			}

			n = pPoints.Count;
			Ring Result = new Ring();

			if (ring.SignedArea > 0.0)	//if(ring.IsExterior)
				for (i = n; i > 0; i--)
					Result.Add(pPoints[(i + iStart) % n]);
			else
				for (i = 0; i < n; i++)
					Result.Add(pPoints[(i + iStart) % n]);

			return Result;
		}

		internal static int Quadric(double A, double B, double C, out double X0, out double X1)
		{
			double D = B * B - 4 * A * C;

			if (D < 0.0)
			{
				X0 = X1 = 0.0;
				return 0;
			}

			if (D == 0.0)
			{
				X0 = X1 = -0.5 * B / A;
				return 1;
			}

			if (A == 0.0)
			{
				X0 = X1 = -C / B;
				return 1;
			}

			double fTmp = 0.5 / A;
			if (fTmp > 0.0)
			{
				X0 = (-B - System.Math.Sqrt(D)) * fTmp;
				X1 = (-B + System.Math.Sqrt(D)) * fTmp;
			}
			else
			{
				X0 = (-B + System.Math.Sqrt(D)) * fTmp;
				X1 = (-B - System.Math.Sqrt(D)) * fTmp;
			}

			return 2;
		}

		internal static Interval[] IntervalsDifference(Interval A, Interval B)
		{
			Interval[] Res = new Interval[1];

			if (B.Min == B.Max || B.Max < A.Min || A.Max < B.Min)
				Res[0] = A;
			else if (A.Min < B.Min && A.Max > B.Max)
			{
				Res = new Interval[2];
				Res[0].Min = A.Min;
				Res[0].Max = B.Min;
				Res[1].Min = B.Max;
				Res[1].Max = A.Max;
			}
			else if (A.Max > B.Max)
			{
				Res[0].Min = B.Max;
				Res[0].Max = A.Max;
			}
			else if (A.Min < B.Min)
			{
				Res[0].Min = A.Min;
				Res[0].Max = B.Min;
			}
			else
				Res = new Interval[0];


			return Res;
		}

		internal static NavaidType[] GetValidFAPNavs(Point PtTHR, double MaxDist, double NomDir, Point ptFAP, double hFAP, eNavaidType GuidType, Point GuidNav)
		{
			int nNav = GlobalVars.DMEList.Length;
			if (nNav == 0)
				return new NavaidType[0];

			double TrackToler;
			double navRange;
			//===========================================================================
			if (GuidType == eNavaidType.VOR || GuidType == eNavaidType.TACAN)
			{
				TrackToler = GlobalVars.constants.NavaidConstants.VOR.TrackingTolerance;
				navRange = GlobalVars.constants.NavaidConstants.VOR.Range;
			}
			else if (GuidType == eNavaidType.NDB)
			{
				TrackToler = GlobalVars.constants.NavaidConstants.NDB.TrackingTolerance;
				navRange = GlobalVars.constants.NavaidConstants.NDB.Range;
			}
			else if (GuidType == eNavaidType.LLZ)
			{
				TrackToler = GlobalVars.constants.NavaidConstants.LLZ.TrackingTolerance;
				navRange = GlobalVars.constants.NavaidConstants.LLZ.Range;
			}
			else
				return new NavaidType[0];


			Ring pGuidRing = new Ring();
			pGuidRing.Add(GuidNav);

			SideDirection AheadBehindSide = ARANMath.SideDef(ptFAP, NomDir + ARANMath.C_PI_2, GuidNav);

			if (AheadBehindSide == SideDirection.sideRight)
			{
				pGuidRing.Add(ARANFunctions.LocalToPrj(GuidNav, NomDir + TrackToler + Math.PI, navRange));
				pGuidRing.Add(ARANFunctions.LocalToPrj(GuidNav, NomDir - TrackToler + Math.PI, navRange));
			}
			else if (AheadBehindSide == SideDirection.sideLeft)
			{
				pGuidRing.Add(ARANFunctions.LocalToPrj(GuidNav, NomDir + TrackToler, navRange));
				pGuidRing.Add(ARANFunctions.LocalToPrj(GuidNav, NomDir - TrackToler, navRange));
			}
			else
			{
				pGuidRing.Add(ARANFunctions.LocalToPrj(GuidNav, NomDir + TrackToler + Math.PI, navRange));
				pGuidRing.Add(ARANFunctions.LocalToPrj(GuidNav, NomDir - TrackToler + Math.PI, navRange));
				pGuidRing.Add(GuidNav);
				pGuidRing.Add(ARANFunctions.LocalToPrj(GuidNav, NomDir + TrackToler, navRange));
				pGuidRing.Add(ARANFunctions.LocalToPrj(GuidNav, NomDir - TrackToler, navRange));
			}
			pGuidRing.Add(GuidNav);
			Polygon pGuidPoly = new Polygon();
			pGuidPoly.ExteriorRing = pGuidRing;

			LineString pNominalLine = new LineString();
			pNominalLine.Add(ARANFunctions.LocalToPrj(PtTHR, NomDir + Math.PI, 2.0 * MaxDist));
			pNominalLine.Add(ARANFunctions.LocalToPrj(PtTHR, NomDir, 2.0 * MaxDist));

			NavaidType[] ValidNavs = new NavaidType[nNav];

			Polygon pExternCircle = new Polygon();
			Polygon pInnerCircle = new Polygon();

			GeometryOperators pTopoOper = new GeometryOperators();
			MultiPolygon pTolerPoly = new MultiPolygon();
			//pTopoOper.CurrentGeometry = pGuidPoly;
			int j = 0;

			//========================================================================================

			for (int i = 0; i < nNav; i++)
			{
				ValidNavs[j] = GlobalVars.DMEList[i];

				Point ptFNav = GlobalVars.DMEList[i].pPtGeo;
				Point ptFNavPrj = GlobalVars.DMEList[i].pPtPrj;

				double DirNAV2FAF = ARANFunctions.ReturnAngleInRadians(ptFNavPrj, ptFAP);
				double d0 = ARANFunctions.ReturnDistanceInMeters(ptFNavPrj, ptFAP);
				double fTmp = ARANMath.SubtractAngles(DirNAV2FAF, NomDir);

				if (fTmp > ARANMath.C_PI_2) fTmp = Math.PI - fTmp;
				if (fTmp > GlobalVars.constants.Pansops[ePANSOPSData.arTP_by_DME_div].Value)
					continue;

				if (d0 + 0.5 * GlobalVars.constants.Pansops[ePANSOPSData.arFAFTolerance].Value > GlobalVars.constants.NavaidConstants.DME.Range)
					continue;

				double Hequ = hFAP + PtTHR.Z - ptFNavPrj.Z;
				double Alpha = System.Math.Atan(Hequ / d0);
				if (Alpha > ARANMath.C_PI_2 - GlobalVars.constants.NavaidConstants.DME.SlantAngle)
					continue;

				double Dist0 = System.Math.Sqrt(Hequ * Hequ + d0 * d0);
				double dMin = (Dist0 - GlobalVars.constants.NavaidConstants.DME.MinimalError) / (1.0 + GlobalVars.constants.NavaidConstants.DME.ErrorScalingUp);
				double dMax = (Dist0 + GlobalVars.constants.NavaidConstants.DME.MinimalError) / (1.0 - GlobalVars.constants.NavaidConstants.DME.ErrorScalingUp);

				SideDirection Side = ARANMath.SideDef(ptFAP, NomDir + ARANMath.C_PI_2, ptFNavPrj);

				if (Side == SideDirection.sideLeft)
				{
					Point ptTmp = ARANFunctions.CircleVectorIntersect(ptFNavPrj, dMin, ptFAP, NomDir);
					if (ptTmp.IsEmpty)
						continue;
					if (ARANFunctions.ReturnDistanceInMeters(ptFAP, ptTmp) > 0.5 * GlobalVars.constants.Pansops[ePANSOPSData.arFAFTolerance].Value)
						continue;

					ptTmp = ARANFunctions.CircleVectorIntersect(ptFNavPrj, dMax, ptFAP, NomDir);
					if (ARANFunctions.ReturnDistanceInMeters(ptFAP, ptTmp) > 0.5 * GlobalVars.constants.Pansops[ePANSOPSData.arFAFTolerance].Value)
						continue;
				}
				else
				{
					Point ptTmp = ARANFunctions.CircleVectorIntersect(ptFNavPrj, dMin, ptFAP, NomDir + Math.PI);
					if (ptTmp.IsEmpty)
						continue;
					if (ARANFunctions.ReturnDistanceInMeters(ptFAP, ptTmp) > 0.5 * GlobalVars.constants.Pansops[ePANSOPSData.arFAFTolerance].Value)
						continue;

					ptTmp = ARANFunctions.CircleVectorIntersect(ptFNavPrj, dMax, ptFAP, NomDir + Math.PI);
					if (ARANFunctions.ReturnDistanceInMeters(ptFAP, ptTmp) > 0.5 * GlobalVars.constants.Pansops[ePANSOPSData.arFAFTolerance].Value)
						continue;
				}

				Ring pTmpRing = ARANFunctions.CreateCirclePrj(ptFNavPrj, dMax);
				pExternCircle.ExteriorRing = pTmpRing;

				pTmpRing = ARANFunctions.CreateCirclePrj(ptFNavPrj, dMin);
				pInnerCircle.ExteriorRing = pTmpRing;

				MultiPolygon pBublic = (MultiPolygon)pTopoOper.Difference(pExternCircle, pInnerCircle);
				MultiPolygon pTmpMultiPoly = (MultiPolygon)pTopoOper.Intersect(pBublic, pGuidPoly);

				int k;
				if (pTmpMultiPoly.Count == 1)
					pTolerPoly.Add(pTmpMultiPoly[0]);
				else for (k = 0; k < pTolerPoly.Count; k++)
					{
						pTolerPoly.Clear();
						pTolerPoly.Add(pTmpMultiPoly[k]);
						if (pTopoOper.GetDistance(ptFAP) == 0.0)
							break;
					}

				Geometry pGeom = pTopoOper.Intersect(pTolerPoly, pNominalLine);
				if (pGeom.IsEmpty)
					continue;

				LineString pPLine = ((MultiLineString)pGeom)[0];
				bool bUseful = true;

				for (k = 0; k < pPLine.Count; k++)
					if (ARANFunctions.ReturnDistanceInMeters(ptFAP, pPLine[k]) > GlobalVars.constants.Pansops[ePANSOPSData.arFAFTolerance].Value)
					{
						bUseful = false;
						break;
					}

				if (bUseful)
				{
					ValidNavs[j].IntersectionType = eIntersectionType.ByDistance;
					j++;
				}
			}
			//========================================================================================
			if (j > 0)
				Array.Resize<NavaidType>(ref ValidNavs, j);
			else
				ValidNavs = new NavaidType[0];

			return ValidNavs;
		}

		private static double CalcNomPos(Point ptDMEprj, double Xs, double Ys, double d0, double BaseHeight, double fRefAltitude, double PDG, SideDirection AheadBehindSide, SideDirection NearSide)
		{
			double dOldPosDME, dNomPosDME = d0 - (int)NearSide * GlobalVars.constants.NavaidConstants.DME.MinimalError;
			int i = 0;
			do
			{
				double nSqr = dNomPosDME * dNomPosDME - Ys * Ys;
				double nSign = System.Math.Sign(nSqr);

				double dNomPosDer = Xs - (int)AheadBehindSide * nSign * System.Math.Sqrt(System.Math.Abs(nSqr));	//dNomPosDer = Xs + AheadBehindSide * System.Math.Sqrt(nSqr)
				double hMax = dNomPosDer * PDG + BaseHeight + fRefAltitude - ptDMEprj.Z;
				dOldPosDME = dNomPosDME;
				dNomPosDME = (d0 - (int)NearSide * GlobalVars.constants.NavaidConstants.DME.MinimalError) /
					(1.0 + (int)NearSide * GlobalVars.constants.NavaidConstants.DME.ErrorScalingUp * System.Math.Sqrt(1.0 + hMax * hMax / (dNomPosDer * dNomPosDer)));

				i++;
				if (i > 5) return dNomPosDME;
			}
			while (System.Math.Abs(dOldPosDME - dNomPosDME) > 0.0001);

			return dNomPosDME;
		}

		private static Interval CalcDMERange(Point ptBasePrj, double BaseHeight, double fRefAltitude, double NomDir, double PDG, Point ptDMEprj, LineSegment KKhMin, LineSegment KKhMax)
		{
			SideDirection Side;
			double d0;
			double d1;

			SideDirection AheadBehindSide = ARANMath.SideDef(KKhMin.Start, NomDir + ARANMath.C_PI_2, ptDMEprj);
			SideDirection LeftRightSide = ARANMath.SideDef(ptBasePrj, NomDir, ptDMEprj);

			if (AheadBehindSide == SideDirection.sideLeft )
			{
				if (LeftRightSide == SideDirection.sideRight)
				{
					d0 = ARANFunctions.ReturnDistanceInMeters(ptDMEprj, KKhMin.End);

					Side = ARANMath.SideDef(KKhMax.Start, NomDir, ptDMEprj);
					if (Side == SideDirection.sideLeft)
						d1 = ARANFunctions.Point2LineDistancePrj(ptDMEprj, KKhMax.Start, NomDir + ARANMath.C_PI_2);
					else
						d1 = ARANFunctions.ReturnDistanceInMeters(ptDMEprj, KKhMax.Start);
				}
				else
				{
					d0 = ARANFunctions.ReturnDistanceInMeters(ptDMEprj, KKhMin.Start);

					Side = ARANMath.SideDef(KKhMax.End, NomDir, ptDMEprj);
					if (Side == SideDirection.sideRight)
						d1 = ARANFunctions.Point2LineDistancePrj(ptDMEprj, KKhMax.End, NomDir + ARANMath.C_PI_2);
					else
						d1 = ARANFunctions.ReturnDistanceInMeters(ptDMEprj, KKhMax.End);
				}
			}
			else if (LeftRightSide == SideDirection.sideRight)
			{
				d0 = ARANFunctions.ReturnDistanceInMeters(ptDMEprj, KKhMax.End);

				Side = ARANMath.SideDef(KKhMin.Start, NomDir, ptDMEprj);
				if (Side == SideDirection.sideLeft)
					d1 = ARANFunctions.Point2LineDistancePrj(ptDMEprj, KKhMin.End, NomDir + ARANMath.C_PI_2);
				else
					d1 = ARANFunctions.ReturnDistanceInMeters(ptDMEprj, KKhMin.Start);
			}
			else
			{
				d0 = ARANFunctions.ReturnDistanceInMeters(ptDMEprj, KKhMax.Start);

				Side = ARANMath.SideDef(KKhMin.End, NomDir, ptDMEprj);
				if (Side == SideDirection.sideRight)
					d1 = ARANFunctions.Point2LineDistancePrj(ptDMEprj, KKhMin.End, NomDir + ARANMath.C_PI_2);
				else
					d1 = ARANFunctions.ReturnDistanceInMeters(ptDMEprj, KKhMin.End);
			}

			double Xs = ARANFunctions.Point2LineDistancePrj(ptDMEprj, ptBasePrj, NomDir + ARANMath.C_PI_2) * (int)ARANMath.SideDef(ptBasePrj, NomDir - ARANMath.C_PI_2, ptDMEprj);
			double Ys = ARANFunctions.Point2LineDistancePrj(ptDMEprj, ptBasePrj, NomDir);

			double Dist0 = CalcNomPos(ptDMEprj, Xs, Ys, d0, BaseHeight, fRefAltitude, PDG, AheadBehindSide, SideDirection.sideRight);
			double Dist1 = CalcNomPos(ptDMEprj, Xs, Ys, d1, BaseHeight, fRefAltitude, PDG, AheadBehindSide, SideDirection.sideLeft);

			Interval result = default(Interval);
			result.Min = Dist0;
			result.Max = Dist1;
			return result;
		}

		internal static NavaidType[] GetValidIFNavs(Point PtFAF, double fRefH, double minDist, double MaxDist, double NomDir, double fPDG, eNavaidType GuidType, Point GuidNav)
		{
			int nr = GlobalVars.NavaidList.Length;
			int nd = GlobalVars.DMEList.Length;
			int nNav = nr + nd;

			if (nNav == 0)
				return new NavaidType[0];

			double TrackToler;

			if (GuidType == eNavaidType.VOR || GuidType == eNavaidType.TACAN)
				TrackToler = GlobalVars.constants.NavaidConstants.VOR.TrackingTolerance;
			else if (GuidType == eNavaidType.NDB)
				TrackToler = GlobalVars.constants.NavaidConstants.NDB.TrackingTolerance;
			else if (GuidType == eNavaidType.LLZ)
				TrackToler = GlobalVars.constants.NavaidConstants.LLZ.TrackingTolerance;
			else return new NavaidType[0];

			Ring pGuidRing = new Ring();

			//if (GuidType != eNavaidTupes.CodeLLZ){
			pGuidRing.Add(GuidNav);
			pGuidRing.Add(ARANFunctions.LocalToPrj(GuidNav, NomDir - TrackToler, 3.0 * GlobalVars.MaxModelRadius));
			pGuidRing.Add(ARANFunctions.LocalToPrj(GuidNav, NomDir + TrackToler, 3.0 * GlobalVars.MaxModelRadius));
			//}

			pGuidRing.Add(GuidNav);
			pGuidRing.Add(ARANFunctions.LocalToPrj(GuidNav, NomDir - TrackToler + Math.PI, 3.0 * GlobalVars.MaxModelRadius));
			pGuidRing.Add(ARANFunctions.LocalToPrj(GuidNav, NomDir + TrackToler + Math.PI, 3.0 * GlobalVars.MaxModelRadius));
			pGuidRing.Add(GuidNav);

			pGuidRing.Add(GuidNav);
			Polygon pGuidPoly = new Polygon();
			pGuidPoly.ExteriorRing = pGuidRing;

			//GlobalVars.gAranGraphics.DrawPolygon(pGuidPoly, -1, eFillStyle.sfsForwardDiagonal);
			//while(true)
			//System.Windows.Forms.Application.DoEvents();

			Point ptFar = ARANFunctions.LocalToPrj(PtFAF, NomDir + Math.PI, MaxDist);
			Point ptNear = ARANFunctions.LocalToPrj(PtFAF, NomDir + Math.PI, minDist);

			//GlobalVars.gAranGraphics.DrawPointWithText(ptFar, -1, "-Far");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptNear, -1, "-Near");
			////GlobalVars.gAranGraphics.DrawPointWithText(GuidNav, -1, "-GuidNav");
			//while(true)
			System.Windows.Forms.Application.DoEvents();

			LineSegment KKhMax = new LineSegment();

			Point ptTmp;

			KKhMax.Start = (Point)ARANFunctions.LineLineIntersect(GuidNav, NomDir - TrackToler, ptFar, NomDir + ARANMath.C_PI_2);
			KKhMax.End = (Point)ARANFunctions.LineLineIntersect(GuidNav, NomDir + TrackToler, ptFar, NomDir + ARANMath.C_PI_2);

			if (ARANMath.SideDef(KKhMax.End, NomDir, KKhMax.Start) == SideDirection.sideRight)
				KKhMax.Reverse();

			Point ptFNav, ptFNavPrj, ptFarD;
			NavaidType[] ValidNavs = new NavaidType[nNav];
			int i, j = 0;

			GeometryOperators pTopoOper = new GeometryOperators();
			pTopoOper.CurrentGeometry = pGuidPoly;

			for (i = 0; i < nr; i++)
			{
				ValidNavs[j] = GlobalVars.NavaidList[i];
				ptFNav = GlobalVars.NavaidList[i].pPtGeo;
				ptFNavPrj = GlobalVars.NavaidList[i].pPtPrj;

				if (pTopoOper.GetDistance(ptFNavPrj) == 0.0)
					continue;

				eNavaidType navType = GlobalVars.NavaidList[i].TypeCode;

				if (navType != eNavaidType.VOR && navType != eNavaidType.NDB && navType != eNavaidType.TACAN)
					continue;

				SideDirection LeftRightSide = ARANMath.SideDef(ptNear, NomDir + Math.PI, ptFNavPrj);
				//SideDirection AheadBehindSide = ARANMath.SideDef(ptNear, NomDir - ARANMath.C_PI_2, ptFNavPrj);	//ptFar

				double InterToler;

				if (navType == eNavaidType.NDB)
					InterToler = GlobalVars.constants.NavaidConstants.NDB.IntersectingTolerance;
				else
					InterToler = GlobalVars.constants.NavaidConstants.VOR.IntersectingTolerance;

				SideDirection Side = ARANMath.SideDef(KKhMax.Start, NomDir + (int)LeftRightSide * ARANMath.C_PI_2, ptFNavPrj);
				if (Side ==  SideDirection.sideLeft)
					ptFarD = KKhMax.End;
				else
					ptFarD = KKhMax.Start;

				//if(ERange < ReturnDistanceInMeters(ptFNavPrj, ptFarD))  continue;
				if (ValidNavs[j].Range < ARANFunctions.ReturnDistanceInMeters(ptFNavPrj, ptFarD))
					continue;

				double azt_Far = ARANFunctions.ReturnAngleInRadians(ptFNavPrj, ptFarD);
				double azt_Near = ARANFunctions.ReturnAngleInRadians(ptFNavPrj, ptNear);

				if (ARANMath.SubtractAngles(azt_Near, azt_Far) < 2.0 * InterToler)
					continue;

				double D = ARANFunctions.Point2LineDistancePrj(ptFNavPrj, PtFAF, NomDir);
				if (System.Math.Atan(GlobalVars.constants.Pansops[ePANSOPSData.arIFTolerance].Value / D) < InterToler)
					continue;

				double Betta = 0.5 * (System.Math.Acos(2.0 * D * System.Math.Sin(InterToler) / GlobalVars.constants.Pansops[ePANSOPSData.arIFTolerance].Value -
						System.Math.Cos(InterToler)) - InterToler);

				double Dist0, Dist1, fTmp;
				ptTmp = (Point)ARANFunctions.LineLineIntersect(GuidNav, NomDir, ptFNavPrj, NomDir + Betta + ARANMath.C_PI_2);
				//DrawPoint ptTmp, RGB(0, 0, 255)
				if (ARANMath.SideDef(PtFAF, NomDir + ARANMath.C_PI_2, ptTmp) == SideDirection.sideRight)
					Dist0 = 0.0;
				else
					Dist0 = ARANFunctions.ReturnDistanceInMeters(PtFAF, ptTmp);

				ptTmp = (Point)ARANFunctions.LineLineIntersect(GuidNav, NomDir, ptFNavPrj, NomDir - Betta + ARANMath.C_PI_2);
				//DrawPoint ptTmp, RGB(0, 0, 255)
				if (ARANMath.SideDef(PtFAF, NomDir + ARANMath.C_PI_2, ptTmp) == SideDirection.sideRight)
					Dist1 = 0.0;
				else
					Dist1 = ARANFunctions.ReturnDistanceInMeters(PtFAF, ptTmp);

				if (Dist1 < Dist0)
				{
					fTmp = Dist1;
					Dist1 = Dist0;
					Dist0 = fTmp;
				}

				if (Dist1 == 0)
					continue;
				//if(Dist1 < minDist) continue;
				//if(Dist0 > MaxDist) continue;

				ptTmp = (Point)ARANFunctions.LineLineIntersect(GuidNav, NomDir, ptFNavPrj, azt_Far - (int)LeftRightSide * InterToler);
				double d0 = minDist;
				double d1 = ARANFunctions.ReturnDistanceInMeters(PtFAF, ptTmp);

				if (d0 < Dist0) d0 = Dist0;
				if (d1 > Dist1) d1 = Dist1;
				if (d1 < d0) continue;

				Point ptNearD = ARANFunctions.LocalToPrj(PtFAF, NomDir + Math.PI, d0);
				ptFarD = ARANFunctions.LocalToPrj(PtFAF, NomDir + Math.PI, d1);

				azt_Far = ARANFunctions.ReturnAngleInRadians(ptFNavPrj, ptFarD);
				azt_Near = ARANFunctions.ReturnAngleInRadians(ptFNavPrj, ptNearD);

				ValidNavs[j].ValMax = new double[1];
				ValidNavs[j].ValMin = new double[1];
				ValidNavs[j].ValCnt = -(int)LeftRightSide;

				if (LeftRightSide == SideDirection.sideRight)
				{
					ValidNavs[j].ValMax[0] = System.Math.Round(ARANFunctions.DirToAzimuth(ptFNavPrj, azt_Far, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo) - 0.4999);
					ValidNavs[j].ValMin[0] = System.Math.Round(ARANFunctions.DirToAzimuth(ptFNavPrj, azt_Near, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo) + 0.4999);
				}
				else
				{
					ValidNavs[j].ValMin[0] = System.Math.Round(ARANFunctions.DirToAzimuth(ptFNavPrj, azt_Far, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo) + 0.4999);
					ValidNavs[j].ValMax[0] = System.Math.Round(ARANFunctions.DirToAzimuth(ptFNavPrj, azt_Near, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo) - 0.4999);
				}

				if (ARANMath.SubtractAngles(ValidNavs[j].ValMax[0] + InterToler, ValidNavs[j].ValMin[0] - InterToler) < InterToler)
					continue;

				ValidNavs[j].IntersectionType = eIntersectionType.ByAngle;
				j++;
			}

			double Hequip = PtFAF.Z + fRefH;
			LineSegment KKhMinDME = new LineSegment();
			LineSegment KKhMaxDME = new LineSegment();

			for (i = 0; i < nd; i++)
			{
				ValidNavs[j] = GlobalVars.DMEList[i];
				ptFNav = GlobalVars.DMEList[i].pPtGeo;
				ptFNavPrj = GlobalVars.DMEList[i].pPtPrj;

				SideDirection LeftRightSide = ARANMath.SideDef(ptNear, NomDir + Math.PI, ptFNavPrj);
				SideDirection AheadBehindSide = ARANMath.SideDef(ptNear, NomDir - ARANMath.C_PI_2, ptFNavPrj);		//ptFar

				Interval IntrH = default(Interval);
				double fTmp;
				IntrH.Min = minDist;
				IntrH.Max = MaxDist;

				if (AheadBehindSide == SideDirection.sideLeft )
					fTmp = ARANFunctions.ReturnDistanceInMeters(ptFNavPrj, ptNear);
				else if (LeftRightSide == SideDirection.sideRight)
					fTmp = ARANFunctions.ReturnDistanceInMeters(ptFNavPrj, KKhMax.End);
				else
					fTmp = ARANFunctions.ReturnDistanceInMeters(ptFNavPrj, KKhMax.Start);

				if (fTmp > GlobalVars.constants.NavaidConstants.DME.Range)		//   Range checking
					continue;

				Point ptMin23, ptMax23;
				if (LeftRightSide !=  SideDirection.sideOn)
				{
					ptMin23 = (Point)ARANFunctions.LineLineIntersect(PtFAF, NomDir, ptFNavPrj, NomDir + (int)LeftRightSide * GlobalVars.constants.Pansops[ePANSOPSData.arTP_by_DME_div].Value);
					ptMax23 = (Point)ARANFunctions.LineLineIntersect(PtFAF, NomDir, ptFNavPrj, NomDir - (int)LeftRightSide * GlobalVars.constants.Pansops[ePANSOPSData.arTP_by_DME_div].Value);
				}
				else
				{
					ptMin23 = ptFNavPrj;
					ptMax23 = ptFNavPrj;
				}

				Interval Intr23 = default(Interval);

				Intr23.Min = ARANFunctions.Point2LineDistancePrj(PtFAF, ptMin23, NomDir + ARANMath.C_PI_2) * (int)ARANMath.SideDef(PtFAF, NomDir + ARANMath.C_PI_2, ptMin23);
				Intr23.Max = ARANFunctions.Point2LineDistancePrj(PtFAF, ptMax23, NomDir + ARANMath.C_PI_2) * (int)ARANMath.SideDef(PtFAF, NomDir + ARANMath.C_PI_2, ptMax23);

				fTmp = ARANFunctions.Point2LineDistancePrj(ptFNavPrj, GuidNav, NomDir + TrackToler) + 5.0;
				double A, B, C, D = ARANFunctions.Point2LineDistancePrj(ptFNavPrj, GuidNav, NomDir - TrackToler) + 5.0;
				if (D < fTmp) D = fTmp;

				double fMinDMEDist = (GlobalVars.constants.NavaidConstants.DME.MinimalError + D) / (1.0 - GlobalVars.constants.NavaidConstants.DME.ErrorScalingUp);
				double vecDist;
				ptMin23 = ARANFunctions.CircleVectorIntersect(ptFNavPrj, fMinDMEDist, PtFAF, NomDir + Math.PI, out vecDist);

				if (vecDist > 0.0)
				{
					ptMax23 = ARANFunctions.CircleVectorIntersect(ptFNavPrj, fMinDMEDist, PtFAF, NomDir);
					A = ARANFunctions.Point2LineDistancePrj(PtFAF, ptMax23, NomDir + ARANMath.C_PI_2) * (int)ARANMath.SideDef(PtFAF, NomDir + ARANMath.C_PI_2, ptMax23);
					B = ARANFunctions.Point2LineDistancePrj(PtFAF, ptMin23, NomDir + ARANMath.C_PI_2) * (int)ARANMath.SideDef(PtFAF, NomDir + ARANMath.C_PI_2, ptMin23);
					if (Intr23.Min > A) Intr23.Min = A;
					if (Intr23.Max < B) Intr23.Max = B;
				}

				Interval[] IntrRes = IntervalsDifference(IntrH, Intr23);
				if (IntrRes.Length == 0)
					continue;

				//fTmp = CircleVectorIntersect(ptFNavPrj, maxDist, ptFAF, NomDir + 180.0, ptTmp)

				double Xs = ARANFunctions.Point2LineDistancePrj(ptFNavPrj, PtFAF, NomDir + ARANMath.C_PI_2) * (int)ARANMath.SideDef(PtFAF, NomDir + ARANMath.C_PI_2, ptFNavPrj);
				double Ys = ARANFunctions.Point2LineDistancePrj(ptFNavPrj, PtFAF, NomDir);

				//Hequip1 = Hequip - ptFNavPrj.Z;
				//if(Hequip1 < 0) Hequip1 = 0;
				//======================= 3700 m ==============================================================
				Interval Intr3700 = default(Interval);
				A = 1.0 + GlobalVars.constants.Pansops[ePANSOPSData.arImDescent_Max].Value * GlobalVars.constants.Pansops[ePANSOPSData.arImDescent_Max].Value;
				B = 2.0 * GlobalVars.constants.Pansops[ePANSOPSData.arImDescent_Max].Value * Hequip - Xs;
				fTmp = (GlobalVars.constants.Pansops[ePANSOPSData.arIFTolerance].Value * System.Math.Cos(GlobalVars.constants.Pansops[ePANSOPSData.arTP_by_DME_div].Value) -
						GlobalVars.constants.NavaidConstants.DME.MinimalError) / GlobalVars.constants.NavaidConstants.DME.ErrorScalingUp;

				C = Hequip * Hequip + Xs * Xs + Ys * Ys - fTmp * fTmp;
				D = B * B - 4.0 * A * C;

				if (D <= 0.0)
					continue;

				D = System.Math.Sqrt(D);
				if (A > 0.0)
				{
					Intr3700.Min = 0.5 * (-B - D) / A;
					Intr3700.Max = 0.5 * (-B + D) / A;
				}
				else
				{
					Intr3700.Min = 0.5 * (-B + D) / A;
					Intr3700.Max = 0.5 * (-B - D) / A;
				}

				if (IntrH.Min < Intr3700.Min) IntrH.Min = Intr3700.Min;
				if (IntrH.Max > Intr3700.Max) IntrH.Max = Intr3700.Max;

				if (IntrH.Min >= IntrH.Max) continue;
				//========================= 55 deg ==================================================================
				int ii, jj, n, m;
				Interval[] IntrRes1;
				Interval Intr55 = default(Interval);

				fTmp = 1.0 / System.Math.Tan(GlobalVars.constants.NavaidConstants.DME.SlantAngle);
				fTmp = fTmp * fTmp;

				A = GlobalVars.constants.Pansops[ePANSOPSData.arImDescent_Max].Value * GlobalVars.constants.Pansops[ePANSOPSData.arImDescent_Max].Value - fTmp;
				B = 2.0 * ((Hequip - ptFNavPrj.Z) * GlobalVars.constants.Pansops[ePANSOPSData.arImDescent_Max].Value + Xs * fTmp);
				C = (Hequip - ptFNavPrj.Z) * (Hequip - ptFNavPrj.Z) - (Xs * Xs + Ys * Ys) * fTmp;
				D = B * B - 4.0 * A * C;

				if (D > 0.0)
				{
					D = System.Math.Sqrt(D);
					if (A > 0.0)
					{
						Intr55.Min = 0.5 * (-B - D) / A;
						Intr55.Max = 0.5 * (-B + D) / A;
					}
					else
					{
						Intr55.Min = 0.5 * (-B + D) / A;
						Intr55.Max = 0.5 * (-B - D) / A;
					}

					//ptTmp = PointAlongPlane(ptFAF, NomDir + 180.0, Intr55.Min);
					//DrawPoint(ptTmp, 0);
					//ptTmp = PointAlongPlane(ptFAF, NomDir + 180.0, Intr55.Max);
					//DrawPoint(ptTmp, 255);

					n = IntrRes.Length;
					IntrRes1 = new Interval[0];

					for (ii = 0; ii < n; ii++)
					{
						Interval[] IntrRes2 = IntervalsDifference(IntrRes[ii], Intr55);

						if (IntrRes1.Length == 0)
							IntrRes1 = IntrRes2;
						else
						{
							m = IntrRes2.Length;
							if (m > 0)
							{
								int L = IntrRes1.Length;
								Array.Resize<Interval>(ref IntrRes1, L + m);

								for (jj = 0; jj < m; jj++)
									IntrRes1[L + jj] = IntrRes2[jj];
							}
						}
					}

					IntrRes = IntrRes1;
				}

				n = IntrRes.Length;

				ii = 0;
				if (n > 0)
					do
					{
						if (IntrRes[ii].Min == IntrRes[ii].Max)
						{
							n--;
							for (jj = ii; jj < n; jj++)
								IntrRes[jj] = IntrRes[jj + 1];
						}
						else
							ii++;

					} while (ii < n);


				ii = 0;
				while (ii < n - 1)
				{
					if (IntrRes[ii].Max == IntrRes[ii + 1].Min)
					{
						IntrRes[ii].Max = IntrRes[ii + 1].Max;
						n--;
						for (jj = ii + 1; jj < n; jj++)
							IntrRes[jj] = IntrRes[jj + 1];
					}
					else
						ii++;
				}

				if (n == 0) continue;

				//========================= ==================================================================
				IntrRes1 = new Interval[n];//+1
				m = 0;

				for (ii = 0; ii < n; ii++)
				{
					Point ptNearD = ARANFunctions.LocalToPrj(PtFAF, NomDir + Math.PI, IntrRes[ii].Min);
					ptFarD = ARANFunctions.LocalToPrj(PtFAF, NomDir + Math.PI, IntrRes[ii].Max);
					double d1 = ARANFunctions.ReturnDistanceInMeters(ptNearD, ptFNavPrj);
					KKhMinDME.Start = ptNearD;
					KKhMinDME.End = ptNearD;

					//DrawPointWithText(ptFarD, "ptFarD", 255);
					//DrawPointWithText(ptNearD, "ptNearD", 0);

					KKhMaxDME.Start = ARANFunctions.LocalToPrj(ptFarD, NomDir - ARANMath.C_PI_2, GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value);
					KKhMaxDME.End = ARANFunctions.LocalToPrj(ptFarD, NomDir + ARANMath.C_PI_2, GlobalVars.constants.Pansops[ePANSOPSData.arIFHalfWidth].Value);
					LineString lst = new LineString();
					lst.Add(KKhMaxDME.Start);
					lst.Add(KKhMaxDME.End);

					MultiLineString mlst = (MultiLineString)  pTopoOper.Intersect(lst);

					if (mlst.IsEmpty)
					{
						KKhMaxDME.Start = ptFarD;
						KKhMaxDME.End = ptFarD;
					}
					else
					{
						KKhMaxDME.Start = mlst[0][0];
						KKhMaxDME.End = mlst[0][mlst[0].Count - 1];

						if (ARANMath.SideDef(ptFarD, NomDir + Math.PI, KKhMaxDME.Start) == SideDirection.sideLeft)
							KKhMaxDME.Reverse();
					}

					IntrRes1[m] = CalcDMERange(PtFAF, PtFAF.Z, fRefH, NomDir + Math.PI, GlobalVars.constants.Pansops[ePANSOPSData.arImDescent_Max].Value, ptFNavPrj, KKhMinDME, KKhMaxDME);
					if (ii == 0)
					{
						if (AheadBehindSide == SideDirection.sideLeft)
						{
							if (IntrRes1[m].Min > d1)
								IntrRes1[m].Min = d1;
						}
						else if (IntrRes1[m].Max < d1)
							IntrRes1[m].Max = d1;
					}

					if (IntrRes1[m].Min < IntrRes1[m].Max)
					{
						m++;
						ValidNavs[j].ValCnt = (int)ARANMath.SideDef(KKhMinDME.Start, NomDir - ARANMath.C_PI_2, ptFNavPrj);
					}
				}

				//M--;
				if (m == 0) continue;

				if (m > 1) ValidNavs[j].ValCnt = 0;

				ValidNavs[j].ValMax = new double[m];
				ValidNavs[j].ValMin = new double[m];

				for (ii = 0; ii < m; ii++)
				{
					ValidNavs[j].ValMin[ii] = IntrRes1[ii].Min;
					ValidNavs[j].ValMax[ii] = IntrRes1[ii].Max;
				}

				if (ValidNavs[j].ValMax[0] < ValidNavs[j].ValMin[0])
					continue;

				ValidNavs[j].IntersectionType = eIntersectionType.ByDistance;
				j++;
			}

			//========================================================================================
			if (j > 0)
				Array.Resize<NavaidType>(ref 	ValidNavs, j);
			else
				ValidNavs = new NavaidType[0];

			return ValidNavs;
		}

		internal static bool PriorPostFixTolerance(MultiPolygon pPolygon, Point pPtPrj, double fDir, out double PriorDist, out double PostDist)
		{
			LineString pCutterPolyline = new LineString();
			pCutterPolyline.Add(ARANFunctions.LocalToPrj(pPtPrj, fDir, 1000000.0));
			pCutterPolyline.Add(ARANFunctions.LocalToPrj(pPtPrj, fDir + 180.0, 1000000.0));

			PriorDist = -1.0;
			PostDist = -1.0;

			GeometryOperators geomOperators = new GeometryOperators();
			geomOperators.CurrentGeometry = pPolygon;
			LineString pIntersection;

			try
			{
				pIntersection = (LineString)geomOperators.Intersect(pCutterPolyline);
			}
			catch
			{
				return false;
			}

			int n = pIntersection.Count;

			if (n == 0)
				return false;

			Point pCurrPt = pIntersection[0];

			double fDist = ARANFunctions.ReturnDistanceInMeters(pPtPrj, pCurrPt) * (int)ARANMath.SideDef(pPtPrj, fDir + ARANMath.C_PI_2, pCurrPt);
			double fMinDist = fDist;
			double fMaxDist = fDist;

			for (int i = 1; i < n; i++)
			{
				pCurrPt = pIntersection[i];
				fDist = ARANFunctions.ReturnDistanceInMeters(pPtPrj, pCurrPt) * (int)ARANMath.SideDef(pPtPrj, fDir + ARANMath.C_PI_2, pCurrPt);

				if (fDist < fMinDist) fMinDist = fDist;
				if (fDist > fMaxDist) fMaxDist = fDist;
			}

			PostDist = fMinDist;
			PriorDist = fMaxDist;

			return true;
		}
	}
}
