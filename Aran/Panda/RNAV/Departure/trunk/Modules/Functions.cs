using System;

using Aran.AranEnvironment;
using Aran.AranEnvironment.Symbols;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.PANDA.Common;

using Aran.PANDA.RNAV.Departure.Properties;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Aran.PANDA.Constants;

namespace Aran.PANDA.RNAV.Departure
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


		//public static double CalcMaxRadius()
		//{
		//	GeometryOperators pGeoOp = new GeometryOperators();
		//	MultiPolygon Poly = (MultiPolygon)pGeoOp.ConvexHull(pLineStr);
		//	Polygon sumPoly = Poly[0];
		//	Ring pExtRing = sumPoly.ExteriorRing;

		//	Ring pExtRing = ConvexHull.CalcConvexHull(pLineStr);
		//	Polygon sumPoly = new Polygon();
		//	sumPoly.ExteriorRing = pExtRing;
		//	MultiPolygon Poly = new MultiPolygon();
		//	Poly.Add(sumPoly);

		//	ptCentr = pExtRing.Centroid;
		//	GlobalVars.gAranGraphics.DrawPointWithText(ptCentr, 255, "");

		//	n = pExtRing.Count;

		//	double x, y, distance = 0;

		//	GlobalVars.gAranGraphics.DrawMultiPolygon(Poly, 255, eFillStyle.sfsCross);
		//	GlobalVars.gAranGraphics.DrawPointWithText(ptCentr, 255, "Centroid");

		//	for (i = 0; i < n; i++)
		//	{
		//		distance = ARANFunctions.ReturnDistanceInMeters(ptCentr, sumPoly.ExteriorRing[i]);
		//		if (result < distance)
		//			result = distance;
		//	}
		//	MultiPolygon Poly = new MultiPolygon();
		//	Poly.Add(new Polygon());

		//	Poly[0].ExteriorRing = ARANFunctions.CreateCirclePrj(ptCentr, result);
		//	GlobalVars.gAranGraphics.DrawMultiPolygon(Poly, 255, eFillStyle.sfsCross);

		//}

		public static double CalcMaxRadius()
		{
			int i, n = GlobalVars.RWYList.Length;
			if (n <= 1)
				return 0;

			Point ptCentr;
			List<Point> pLineStr = new List<Point>();

			for (i = 0; i < n; i++)
			{
				ptCentr = GlobalVars.RWYList[i].pPtPrj[eRWY.ptTHR];
				pLineStr.Add(ptCentr);
			}

			if (pLineStr.Count < 3)
				return ARANMath.Hypot(pLineStr[0].X - pLineStr[1].X, pLineStr[0].Y - pLineStr[1].Y);

			double result;
			minCircle.minCircleAroundPoints(pLineStr, out ptCentr, out result);

			return 2 * result;
		}

		//public static void GetObstaclesByPolygon(Obstacle[] LocalObstacles, out Obstacle[] ObstacleList, MultiPolygon pPoly)
		//{
		//	int n = LocalObstacles.Length;
		//	ObstacleList = new Obstacle[n];
		//	if (n == 0)
		//		return;

		//	//GeometryOperators pGeoOp = new GeometryOperators();

		//	int j = -1;
		//	for (int i = 0; i < n; i++)
		//	{
		//		Point ptCurr = (Point)LocalObstacles[i].pGeomPrj;
		//		//if (pGeoOp.Contains(pPoly, ptCurr))
		//		if (pPoly.IsPointInside(ptCurr))
		//			ObstacleList[++j] = LocalObstacles[i];
		//	}

		//	System.Array.Resize<Obstacle>(ref ObstacleList, j + 1);
		//}

		//private static void QuickSort(ObstacleType[] obstacleArray, int iLo, int iHi)
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
		//			ObstacleType t = obstacleArray[Lo];
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

		//public static void SortByDist(ObstacleType[] obstacleArray)
		//{
		//	int Lo = obstacleArray.GetLowerBound(0);
		//	int Hi = obstacleArray.GetUpperBound(0);

		//	if (Lo >= Hi)
		//		return;

		//	QuickSort(obstacleArray, Lo, Hi);
		//}

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

		public static void SortByDist(ref ObstacleContainer A)
		{
			int Lo = 0;
			int Hi = A.Parts.Length - 1;

			if (Lo >= Hi) return;
			SortByDist(ref A, Lo, Hi);
		}

		public static bool PriorPostFixTolerance(MultiPolygon pPolygon, Point pPtPrj, double fDir, out double PriorDist, out double PostDist)
		{
			PriorDist = -1.0;
			PostDist = -1.0;
			MultiLineString ptIntersection;

			LineString pCutterPolyline = new LineString();
			pCutterPolyline.Add(ARANFunctions.LocalToPrj(pPtPrj, fDir, 1000000.0, 0));
			pCutterPolyline.Add(ARANFunctions.LocalToPrj(pPtPrj, fDir, -1000000.0, 0));

			try
			{
				GeometryOperators pTopological = new GeometryOperators();
				Geometry pIntersection = pTopological.Intersect(pCutterPolyline, pPolygon);
				if (pIntersection.IsEmpty)
					return false;
				ptIntersection = (MultiLineString)pIntersection;
			}
			catch
			{
				return false;
			}

			Point ptDist = ARANFunctions.PrjToLocal(pPtPrj, fDir, ptIntersection[0][0]);

			double fMinDist = ptDist.X;
			double fMaxDist = ptDist.X;
			int n = ptIntersection.Count;

			for (int j = 0; j < n; j++)
			{
				LineString ls = ptIntersection[j];
				int m = ls.Count;

				for (int i = 0; i < m; i++)
				{
					ptDist = ARANFunctions.PrjToLocal(pPtPrj, fDir, ls[i]);

					if (ptDist.X < fMinDist) fMinDist = ptDist.X;
					if (ptDist.X > fMaxDist) fMaxDist = ptDist.X;
				}
			}
			PriorDist = fMinDist;
			PostDist = fMaxDist;

			return true;
		}

		public static int GetObstaclesByDistance(out ObstacleContainer obstList, Point ptCenter, double radius, out double maxDist)
		{
			int n = GlobalVars.GObstacleList.Obstacles.Length;
			obstList.Obstacles = new Obstacle[n];
			obstList.Parts = new ObstacleData[0];

			maxDist = 0.0;

			if (n == 0)
				return 0;

			if (radius == 0)
			{
				obstList.Obstacles = new Obstacle[0];
				return 0;
			}

			int j = 0;
			GeometryOperators pTopooper = new GeometryOperators();
			pTopooper.CurrentGeometry = ptCenter;

			foreach (var obst in GlobalVars.GObstacleList.Obstacles)
			{
				double dist = pTopooper.GetDistance(obst.pGeomPrj);
				if (dist <= radius)
				{
					obstList.Obstacles[j++] = obst;
					if (dist > maxDist)
						maxDist = dist;
				}
			}

			System.Array.Resize<Obstacle>(ref obstList.Obstacles, j);
			return j;
		}

		static Point GetNearestPoint(MultiPoint pPoints, Point PtEnd, double depDir)
		{
			double minDist = double.MaxValue;
			Point result = null;

			foreach (Point pCurrPt in pPoints)
			{
				ARANFunctions.PrjToLocal(PtEnd, depDir, pCurrPt, out double dist, out double y);

				if (dist < minDist)
				{
					minDist = dist;
					result = pCurrPt;
				}
			}

			return result;
		}

		static Point GetNearestPoint(Geometry pKKGeom, MultiPoint pPoints)
		{
			double minDist = double.MaxValue;
			Point result = null;

			GeometryOperators pGeoOp = new GeometryOperators { CurrentGeometry = pKKGeom };

			foreach (Point pCurrPt in pPoints)
			{
				double dist = pGeoOp.GetDistance(pCurrPt);

				if (dist < minDist)
				{
					minDist = dist;
					result = pCurrPt;
				}
			}

			return result;
		}

		public static ObstacleContainer GetLegAreaObstacles(out ObstacleContainer OutObstList, LegBase CurrLeg, double distFromPrevs, double hDER, double MOCLimit, WayPoint PtPrevFIX = null)
		{
			int m = GlobalVars.ObstacleList.Obstacles.Length;
			ObstacleContainer result;
			result.Obstacles = new Obstacle[0];
			result.Parts = new ObstacleData[0];

			if (m == 0)
			{
				OutObstList.Obstacles = new Obstacle[0];
				OutObstList.Parts = new ObstacleData[0];
				return result;
			}

			GeometryOperators fullGeoOp = new GeometryOperators
			{
				CurrentGeometry = CurrLeg.FullAssesmentArea
			};

			GeometryOperators primaryGeoOp = new GeometryOperators
			{
				CurrentGeometry = CurrLeg.PrimaryAssesmentArea
			};

			MultiPolygon pSecondaryPoly = (MultiPolygon)primaryGeoOp.Difference(CurrLeg.FullAssesmentArea);

			GeometryOperators secondGeoOp = new GeometryOperators
			{
				CurrentGeometry = pSecondaryPoly
			};

			GeometryOperators lineStrGeoOp = new GeometryOperators
			{
				CurrentGeometry = CurrLeg.FullProtectionAreaOutline()  // ARANFunctions.PolygonToPolyLine(CurrLeg.FullAssesmentArea[0])
			};

			GeometryOperators KKLineGeoOp = new GeometryOperators
			{
				CurrentGeometry = CurrLeg.KKLine
			};

			double LowerMOCLimit = GlobalVars.constants.Pansops[ePANSOPSData.dpObsClr].Value;

			int c = 10 * m;
			int k = 0;
			int l = 0;

			OutObstList.Obstacles = new Obstacle[m];
			OutObstList.Parts = new ObstacleData[c];

			MultiPoint pObstPoints = new MultiPoint();
			LineString pPart = new LineString();

			//MultiPolygon pTmpPoly = (MultiPolygon)fullGeoOp.Difference(CurrLeg.FullAssesmentArea, CurrLeg.PrimaryAssesmentArea);
			double maxO = double.MinValue;
			int resultObst = -1;
			int resultPart = -1;

			for (int i = 0; i < m; i++)
			{
				Geometry pCurrGeom = GlobalVars.ObstacleList.Obstacles[i].pGeomPrj;

				if (pCurrGeom.IsEmpty)
					continue;

				double distToFullPoly = fullGeoOp.GetDistance(pCurrGeom);
				if (distToFullPoly > GlobalVars.ObstacleList.Obstacles[i].HorAccuracy)
					continue;

				pObstPoints.Clear();

				if (pCurrGeom.Type == GeometryType.Point)
					pObstPoints.Add((Point)pCurrGeom);
				else
				{
					Geometry pTmpPoints = primaryGeoOp.Intersect(pCurrGeom);
					MultiPoint polyPoints = pTmpPoints.ToMultiPoint();
					Point ptTmp = GetNearestPoint(CurrLeg.KKLine, polyPoints);

					if (ptTmp != null)
						pObstPoints.Add(ptTmp);

					pTmpPoints = secondGeoOp.Intersect(pCurrGeom);

					polyPoints = pTmpPoints.ToMultiPoint();
					ptTmp = GetNearestPoint(CurrLeg.KKLine, polyPoints);

					if (ptTmp != null)
						pObstPoints.Add(ptTmp);

					//Geometry pTmpPoints = fullGeoOp.Intersect(pCurrGeom);
					//pObstPoints.AddMultiPoint(pTmpPoints.ToMultiPoint());

					//pTmpPoints = primaryGeoOp.Intersect(pCurrGeom);
					//pObstPoints.AddMultiPoint(pTmpPoints.ToMultiPoint());

					//RemoveSeamPoints(ref pObstPoints);
				}

				int n = pObstPoints.Count;
				if (n <= 0)
					continue;

				OutObstList.Obstacles[l] = GlobalVars.ObstacleList.Obstacles[i];
				OutObstList.Obstacles[l].PartsNum = n;
				//OutObstList.Obstacles[l].Parts = new int[n];

				for (int j = 0; j < n; j++)
				{
					Point pCurrPt = pObstPoints[j];

					if (k >= c)
					{
						c += m;
						Array.Resize<ObstacleData>(ref OutObstList.Parts, c);
					}

					//OutObstList.Obstacles[l].Parts[j] = k;
					OutObstList.Parts[k].pPtPrj = pCurrPt;
					OutObstList.Parts[k].Owner = l;
					OutObstList.Parts[k].Index = j;

					OutObstList.Parts[k].Elev = OutObstList.Obstacles[l].Height;
					OutObstList.Parts[k].Height = OutObstList.Parts[k].Elev - hDER;
					OutObstList.Parts[k].d0 = KKLineGeoOp.GetDistance(pCurrPt);

					double distToPrimaryPoly = primaryGeoOp.GetDistance(pCurrPt);
					OutObstList.Parts[k].Prima = distToPrimaryPoly <= OutObstList.Obstacles[l].HorAccuracy;

					OutObstList.Parts[k].fSecCoeff = 1.0;
					OutObstList.Parts[k].Flags = 0;

					if (!OutObstList.Parts[k].Prima)
					{
						double d1 = lineStrGeoOp.GetDistance(pCurrPt);
						double d = distToPrimaryPoly + d1;

						OutObstList.Parts[k].fSecCoeff = d1 / d;
						OutObstList.Parts[k].Flags = 1;
					}

					OutObstList.Parts[k].DistStar = OutObstList.Parts[k].d0 + distFromPrevs;

					double dPDG = OutObstList.Parts[k].DistStar;

					if (PtPrevFIX != null && CurrLeg.StartFIX.FlyMode == eFlyMode.Atheight)
					{
						Point ptNearest = KKLineGeoOp.GetNearestPoint(pCurrPt);

						double distFromS = ARANFunctions.Point2LineDistancePrj(ptNearest, PtPrevFIX.PrjPt, CurrLeg.StartFIX.EntryDirection + ARANMath.C_PI_2);
						dPDG = OutObstList.Parts[k].d0 + Math.Abs(distFromS);
					}

					double tmpMOC = dPDG * GlobalVars.constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.dpMOC].Value;
					if (tmpMOC > MOCLimit)
						tmpMOC = MOCLimit;

					if (tmpMOC < LowerMOCLimit)
						tmpMOC = LowerMOCLimit;

					OutObstList.Parts[k].MOC = tmpMOC * OutObstList.Parts[k].fSecCoeff;
					OutObstList.Parts[k].ReqH = OutObstList.Parts[k].MOC + OutObstList.Parts[k].Height;
					//OutObstList.Parts[k].ReqOCH = OutObstList.Parts[k].MOC + OutObstList.Parts[k].Height;

					OutObstList.Parts[k].PDG = (OutObstList.Parts[k].ReqH - GlobalVars.constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.dpH_abv_DER].Value) / OutObstList.Parts[k].DistStar;

					if (maxO < OutObstList.Parts[k].PDG)
					{
						maxO = OutObstList.Parts[k].PDG;

						resultObst = l;
						resultPart = k;
					}

					OutObstList.Parts[k].Ignored = false;
					k++;
				}
				l++;
			}

			if (k >= 0)
			{
				Array.Resize<Obstacle>(ref OutObstList.Obstacles, l);
				Array.Resize<ObstacleData>(ref OutObstList.Parts, k);

				if (resultPart >= 0)
				{
					result.Obstacles = new Obstacle[1];
					result.Parts = new ObstacleData[1];

					result.Obstacles[0] = OutObstList.Obstacles[resultObst];
					result.Parts[0] = OutObstList.Parts[resultPart];
				}
			}
			else
			{
				OutObstList.Obstacles = new Obstacle[0];
				OutObstList.Parts = new ObstacleData[0];
			}

			return result;
		}

		public static void GetStrightAreaObstacles(out ObstacleContainer OutObstList, MultiPolygon pFullPoly, MultiPolygon pPrimaryPoly, RWYType Rwy, double depDir)
		{
			int m = GlobalVars.ObstacleList.Obstacles.Length;

			if (m == 0)
			{
				OutObstList.Obstacles = new Obstacle[0];
				OutObstList.Parts = new ObstacleData[0];
				return;
			}

			GeometryOperators fullGeoOp = new GeometryOperators
			{
				CurrentGeometry = pFullPoly
			};

			GeometryOperators primaryGeoOp = new GeometryOperators
			{
				CurrentGeometry = pPrimaryPoly
			};

			MultiPolygon pSecondaryPoly = (MultiPolygon)primaryGeoOp.Difference(pFullPoly);
			GeometryOperators secondGeoOp = null;

			if (pSecondaryPoly != null)
				secondGeoOp = new GeometryOperators
				{
					CurrentGeometry = pSecondaryPoly
				};

			//GeometryOperators lineStrGeoOp = new GeometryOperators
			//{
			//	CurrentGeometry = pFullPoly		//ARANFunctions.FullProtectionAreaBorder(pFullPoly, depDir)
			//};

			//GlobalVars.gAranGraphics.DrawMultiLineString((MultiLineString)lineStrGeoOp.CurrentGeometry, 2);
			//LegBase.ProcessMessages();

			int c = 10 * m;
			int k = 0;
			int l = 0;

			OutObstList.Obstacles = new Obstacle[m];
			OutObstList.Parts = new ObstacleData[c];

			MultiPoint pObstPoints = new MultiPoint();
			LineString pPart = new LineString();

			Point PtEnd = Rwy.pPtPrj[eRWY.ptDER];
			LineString ls = new LineString();

			for (int i = 0; i < m; i++)
			{
				Geometry pCurrGeom = GlobalVars.ObstacleList.Obstacles[i].pGeomPrj;

				if (pCurrGeom.IsEmpty)
					continue;

				double distToFullPoly = fullGeoOp.GetDistance(pCurrGeom);
				if (distToFullPoly > GlobalVars.ObstacleList.Obstacles[i].HorAccuracy)
					continue;

				pObstPoints.Clear();

				if (pCurrGeom.Type == GeometryType.Point)
					pObstPoints.Add((Point)pCurrGeom);
				else
				{
					Geometry pTmpPoints = primaryGeoOp.Intersect(pCurrGeom);
					MultiPoint polyPoints = pTmpPoints.ToMultiPoint();
					Point ptTmp = GetNearestPoint(polyPoints, PtEnd, depDir);

					if (ptTmp != null)
						pObstPoints.Add(ptTmp);

					if (secondGeoOp != null)
					{
						pTmpPoints = secondGeoOp.Intersect(pCurrGeom);

						polyPoints = pTmpPoints.ToMultiPoint();
						ptTmp = GetNearestPoint(polyPoints, PtEnd, depDir);

						if (ptTmp != null)
							pObstPoints.Add(ptTmp);
					}
				}

				int n = pObstPoints.Count;
				if (n <= 0)
					continue;

				OutObstList.Obstacles[l] = GlobalVars.ObstacleList.Obstacles[i];
				OutObstList.Obstacles[l].PartsNum = n;
				//OutObstList.Obstacles[l].Parts = new int[n];

				for (int j = 0; j < n; j++)
				{
					Point pCurrPt = pObstPoints[j];

					if (k >= c)
					{
						c += m;
						Array.Resize<ObstacleData>(ref OutObstList.Parts, c);
					}

					//OutObstList.Obstacles[l].Parts[j] = k;
					OutObstList.Parts[k].pPtPrj = pCurrPt;
					OutObstList.Parts[k].Owner = l;
					OutObstList.Parts[k].Index = j;

					OutObstList.Parts[k].Elev = OutObstList.Obstacles[l].Height;
					OutObstList.Parts[k].Height = OutObstList.Parts[k].Elev - PtEnd.Z;

					ARANFunctions.PrjToLocal(PtEnd, depDir, pCurrPt, out OutObstList.Parts[k].Dist, out OutObstList.Parts[k].CLShift);

					//int sign = Math.Sign(OutObstList.Parts[k].Dist);
					//OutObstList.Parts[k].Rmin = Math.Abs(OutObstList.Parts[k].Dist) - OutObstList.Obstacles[l].HorAccuracy;
					//if (OutObstList.Parts[k].Rmin < 0.0)
					//	OutObstList.Parts[k].Rmin = 0.0;
					//OutObstList.Parts[k].Rmin *= sign;

					//sign = Math.Sign(OutObstList.Parts[k].CLShift);
					//OutObstList.Parts[k].Rmax = Math.Abs(OutObstList.Parts[k].CLShift) - OutObstList.Obstacles[l].HorAccuracy;
					//if (OutObstList.Parts[k].Rmax < 0.0)
					//	OutObstList.Parts[k].Rmax = 0.0;
					//OutObstList.Parts[k].Rmax *= sign;

					double distToPrimaryPoly = primaryGeoOp.GetDistance(pCurrPt);
					OutObstList.Parts[k].Prima = distToPrimaryPoly <= OutObstList.Obstacles[l].HorAccuracy;
					OutObstList.Parts[k].fSecCoeff = 1.0;
					OutObstList.Parts[k].Flags = 0;
					OutObstList.Parts[k].Ignored = false;

					if (!OutObstList.Parts[k].Prima)
					{
						ls.Clear();
						ls.Add(pCurrPt);
						ls.Add(ARANFunctions.LocalToPrj(PtEnd, depDir, OutObstList.Parts[k].Dist, Math.Sign(OutObstList.Parts[k].CLShift) * 30000.0));

						MultiLineString mls = (MultiLineString)fullGeoOp.Intersect(ls);

						double d1 = mls.Length;		//lineStrGeoOp.GetDistance(pCurrPt);
						double d = distToPrimaryPoly + d1;

						OutObstList.Parts[k].fSecCoeff = d1 / d;
						OutObstList.Parts[k].Flags = 1;
					}

					k++;
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
		}

		//static void RemoveSeamPoints(ref MultiPoint pPoints)
		//{
		//	const double eps2 = ARANMath.EpsilonDistance * ARANMath.EpsilonDistance;
		//	int n = pPoints.Count;
		//	int j = 0;

		//	while (j < n - 1)
		//	{
		//		Point pCurrPt = pPoints[j];
		//		int i = j + 1;
		//		while (i < n)
		//		{
		//			double dx = pCurrPt.X - pPoints[i].X;
		//			double dy = pCurrPt.Y - pPoints[i].Y;
		//			double fDist2 = dx * dx + dy * dy;

		//			if (fDist2 < eps2)
		//			{
		//				pPoints.Remove(i);
		//				n--;
		//			}
		//			else
		//				i++;
		//		}
		//		j++;
		//	}
		//}

		//public static ObstacleContainer GetLegAreaObstacles_Old(out ObstacleContainer OutObstList, LegBase CurrLeg, double distFromPrevs, double hDER, double MOCLimit, WayPoint PtPrevFIX = null)
		//{
		//	int m = GlobalVars.ObstacleList.Obstacles.Length;
		//	ObstacleContainer result;
		//	result.Obstacles = new Obstacle[0];
		//	result.Parts = new ObstacleData[0];

		//	if (m == 0)
		//	{
		//		OutObstList.Obstacles = new Obstacle[0];
		//		OutObstList.Parts = new ObstacleData[0];
		//		return result;
		//	}

		//	GeometryOperators primaryGeoOp = new GeometryOperators
		//	{
		//		CurrentGeometry = CurrLeg.PrimaryAssesmentArea
		//	};

		//	GeometryOperators fullGeoOp = new GeometryOperators
		//	{
		//		CurrentGeometry = CurrLeg.FullAssesmentArea
		//	};

		//	GeometryOperators lineStrGeoOp = new GeometryOperators
		//	{
		//		CurrentGeometry = ARANFunctions.PolygonToPolyLine(CurrLeg.FullAssesmentArea[0])
		//	};

		//	GeometryOperators KKLineGeoOp = new GeometryOperators
		//	{
		//		CurrentGeometry = CurrLeg.KKLine
		//	};

		//	double LowerMOCLimit = GlobalVars.constants.Pansops[ePANSOPSData.dpObsClr].Value;

		//	int c = 10 * m;
		//	int k = 0;
		//	int l = 0;

		//	OutObstList.Obstacles = new Obstacle[m];
		//	OutObstList.Parts = new ObstacleData[c];

		//	MultiPoint pObstPoints = new MultiPoint();
		//	LineString pPart = new LineString();

		//	//MultiPolygon pTmpPoly = (MultiPolygon)fullGeoOp.Difference(CurrLeg.FullAssesmentArea, CurrLeg.PrimaryAssesmentArea);
		//	double maxO = double.MinValue;
		//	int resultObst = -1;
		//	int resultPart = -1;

		//	for (int i = 0; i < m; i++)
		//	{
		//		Geometry pCurrGeom = GlobalVars.ObstacleList.Obstacles[i].pGeomPrj;

		//		if (pCurrGeom.IsEmpty )
		//			continue;

		//		double distToFullPoly = fullGeoOp.GetDistance(pCurrGeom);
		//		if (distToFullPoly > GlobalVars.ObstacleList.Obstacles[i].HorAccuracy)
		//			continue;

		//		pObstPoints.Clear();

		//		if (pCurrGeom.Type == GeometryType.Point)
		//			pObstPoints.Add((Point)pCurrGeom);
		//		else
		//		{
		//			Geometry pTmpPoints = fullGeoOp.Intersect(pCurrGeom);
		//			pObstPoints.AddMultiPoint(pTmpPoints.ToMultiPoint());

		//			pTmpPoints = primaryGeoOp.Intersect(pCurrGeom);
		//			pObstPoints.AddMultiPoint(pTmpPoints.ToMultiPoint());

		//			RemoveSeamPoints(ref pObstPoints);
		//		}

		//		int n = pObstPoints.Count;
		//		if (n <= 0)
		//			continue;

		//		OutObstList.Obstacles[l] = GlobalVars.ObstacleList.Obstacles[i];
		//		OutObstList.Obstacles[l].PartsNum = n;
		//		OutObstList.Obstacles[l].Parts = new int[n];

		//		for (int j = 0; j < n; j++)
		//		{
		//			Point pCurrPt = pObstPoints[j];

		//			if (k >= c)
		//			{
		//				c += m;
		//				Array.Resize<ObstacleData>(ref OutObstList.Parts, c);
		//			}

		//			OutObstList.Obstacles[l].Parts[j] = k;
		//			OutObstList.Parts[k].pPtPrj = pCurrPt;
		//			OutObstList.Parts[k].Owner = l;
		//			OutObstList.Parts[k].Index = j;

		//			OutObstList.Parts[k].Elev = OutObstList.Obstacles[l].Height;
		//			OutObstList.Parts[k].Height = OutObstList.Parts[k].Elev - hDER;
		//			OutObstList.Parts[k].d0 = KKLineGeoOp.GetDistance(pCurrPt);

		//			double distToPrimaryPoly = primaryGeoOp.GetDistance(pCurrPt);
		//			OutObstList.Parts[k].Prima = distToPrimaryPoly <= OutObstList.Obstacles[l].HorAccuracy;

		//			OutObstList.Parts[k].fSecCoeff = 1.0;
		//			OutObstList.Parts[k].Flags = 0;

		//			if (!OutObstList.Parts[k].Prima)
		//			{
		//				double d1 = lineStrGeoOp.GetDistance(pCurrPt);
		//				double d = distToPrimaryPoly + d1;

		//				OutObstList.Parts[k].fSecCoeff = d1 / d;
		//				OutObstList.Parts[k].Flags = 1;
		//			}

		//			OutObstList.Parts[k].DistStar = OutObstList.Parts[k].d0 + distFromPrevs;

		//			double dPDG = OutObstList.Parts[k].DistStar;

		//			if (PtPrevFIX != null && CurrLeg.StartFIX.FlyMode == eFlyMode.Atheight)
		//			{
		//				Point ptNearest = KKLineGeoOp.GetNearestPoint(pCurrPt);

		//				double distFromS = ARANFunctions.Point2LineDistancePrj(ptNearest, PtPrevFIX.PrjPt, CurrLeg.StartFIX.EntryDirection + ARANMath.C_PI_2);
		//				dPDG = OutObstList.Parts[k].d0 + Math.Abs(distFromS);
		//			}

		//			double tmpMOC = dPDG * GlobalVars.constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.dpMOC].Value;
		//			if (tmpMOC > MOCLimit)
		//				tmpMOC = MOCLimit;

		//			if (tmpMOC < LowerMOCLimit)
		//				tmpMOC = LowerMOCLimit;

		//			OutObstList.Parts[k].MOC = tmpMOC * OutObstList.Parts[k].fSecCoeff;
		//			OutObstList.Parts[k].ReqH = OutObstList.Parts[k].MOC + OutObstList.Parts[k].Height;
		//			//OutObstList.Parts[k].ReqOCH = OutObstList.Parts[k].MOC + OutObstList.Parts[k].Height;

		//			OutObstList.Parts[k].PDG = (OutObstList.Parts[k].ReqH - GlobalVars.constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.dpH_abv_DER].Value) / OutObstList.Parts[k].DistStar;

		//			if (maxO < OutObstList.Parts[k].PDG)
		//			{
		//				maxO = OutObstList.Parts[k].PDG;

		//				resultObst = l;
		//				resultPart = k;
		//			}

		//			OutObstList.Parts[k].Ignored = false;
		//			k++;
		//		}
		//		l++;
		//	}

		//	if (k >= 0)
		//	{
		//		Array.Resize<Obstacle>(ref OutObstList.Obstacles, l);
		//		Array.Resize<ObstacleData>(ref OutObstList.Parts, k);

		//		if (resultPart >= 0)
		//		{
		//			result.Obstacles = new Obstacle[1];
		//			result.Parts = new ObstacleData[1];

		//			result.Obstacles[0] = OutObstList.Obstacles[resultObst];
		//			result.Parts[0] = OutObstList.Parts[resultPart];
		//		}
		//	}
		//	else
		//	{
		//		OutObstList.Obstacles = new Obstacle[0];
		//		OutObstList.Parts = new ObstacleData[0];
		//	}

		//	return result;
		//}

		//public static void GetStrightAreaObstacles_Old(ObstacleContainer LocalObstacles, out ObstacleContainer innerObstacleList, MultiPolygon pFullPoly, MultiPolygon pPrimaryPoly, RWYType Rwy, double depDir)
		//{
		//	int m = GlobalVars.ObstacleList.Obstacles.Length;

		//	if (m == 0)
		//	{
		//		innerObstacleList.Obstacles = new Obstacle[0];
		//		innerObstacleList.Parts = new ObstacleData[0];
		//		return;
		//	}

		//	GeometryOperators primaryGeoOp = new GeometryOperators
		//	{
		//		CurrentGeometry = pPrimaryPoly
		//	};

		//	GeometryOperators fullGeoOp = new GeometryOperators
		//	{
		//		CurrentGeometry = pFullPoly
		//	};

		//	GeometryOperators lineStrGeoOp = new GeometryOperators
		//	{
		//		CurrentGeometry = ARANFunctions.PolygonToPolyLine(pFullPoly[0])
		//	};

		//	int c = 10 * m;
		//	int k = 0;
		//	int l = 0;

		//	innerObstacleList.Obstacles = new Obstacle[m];
		//	innerObstacleList.Parts = new ObstacleData[c];

		//	MultiPoint pObstPoints = new MultiPoint();
		//	LineString pPart = new LineString();

		//	Point PtEnd = Rwy.pPtPrj[eRWY.ptDER];

		//	for (int i = 0; i < m; i++)
		//	{
		//		Geometry pCurrGeom = GlobalVars.ObstacleList.Obstacles[i].pGeomPrj;

		//		if (pCurrGeom.IsEmpty)
		//			continue;

		//		double distToFullPoly = fullGeoOp.GetDistance(pCurrGeom);
		//		if (distToFullPoly > GlobalVars.ObstacleList.Obstacles[i].HorAccuracy)
		//			continue;

		//		pObstPoints.Clear();

		//		if (pCurrGeom.Type == GeometryType.Point)
		//			pObstPoints.Add((Point)pCurrGeom);
		//		else
		//		{
		//			Geometry pTmpPoints = fullGeoOp.Intersect(pCurrGeom);
		//			pObstPoints.AddMultiPoint(pTmpPoints.ToMultiPoint());

		//			pTmpPoints = primaryGeoOp.Intersect(pCurrGeom);
		//			pObstPoints.AddMultiPoint(pTmpPoints.ToMultiPoint());

		//			RemoveSeamPoints(ref pObstPoints);
		//		}

		//		int n = pObstPoints.Count;
		//		if (n <= 0)
		//			continue;

		//		innerObstacleList.Obstacles[l] = GlobalVars.ObstacleList.Obstacles[i];
		//		innerObstacleList.Obstacles[l].PartsNum = n;
		//		innerObstacleList.Obstacles[l].Parts = new int[n];

		//		for (int j = 0; j < n; j++)
		//		{
		//			Point pCurrPt = pObstPoints[j];

		//			if (k >= c)
		//			{
		//				c += m;
		//				Array.Resize<ObstacleData>(ref innerObstacleList.Parts, c);
		//			}

		//			innerObstacleList.Obstacles[l].Parts[j] = k;
		//			innerObstacleList.Parts[k].pPtPrj = pCurrPt;
		//			innerObstacleList.Parts[k].Owner = l;
		//			innerObstacleList.Parts[k].Index = j;

		//			innerObstacleList.Parts[k].Elev = innerObstacleList.Obstacles[l].Height;
		//			innerObstacleList.Parts[k].Height = innerObstacleList.Parts[k].Elev - PtEnd.Z;

		//			ARANFunctions.PrjToLocal(PtEnd, depDir, pCurrPt, out innerObstacleList.Parts[k].Dist, out innerObstacleList.Parts[k].CLShift);

		//			int sign = Math.Sign(innerObstacleList.Parts[k].Dist);
		//			innerObstacleList.Parts[k].Rmin = Math.Abs(innerObstacleList.Parts[k].Dist) - innerObstacleList.Obstacles[l].HorAccuracy;
		//			if (innerObstacleList.Parts[k].Rmin < 0.0)
		//				innerObstacleList.Parts[k].Rmin = 0.0;
		//			innerObstacleList.Parts[k].Rmin *= sign;

		//			sign = Math.Sign(innerObstacleList.Parts[k].CLShift);
		//			innerObstacleList.Parts[k].Rmax = Math.Abs(innerObstacleList.Parts[k].CLShift) - innerObstacleList.Obstacles[l].HorAccuracy;
		//			if (innerObstacleList.Parts[k].Rmax < 0.0)
		//				innerObstacleList.Parts[k].Rmax = 0.0;
		//			innerObstacleList.Parts[k].Rmax *= sign;

		//			double distToPrimaryPoly = primaryGeoOp.GetDistance(pCurrPt);
		//			innerObstacleList.Parts[k].Prima = distToPrimaryPoly <= innerObstacleList.Obstacles[l].HorAccuracy;     //pPrimaryPoly.IsPointInside(ptCurr);
		//			innerObstacleList.Parts[k].fSecCoeff = 1.0;
		//			innerObstacleList.Parts[k].Flags = 0;

		//			if (!innerObstacleList.Parts[k].Prima)
		//			{
		//				double d1 = lineStrGeoOp.GetDistance(pCurrPt);
		//				double d = distToPrimaryPoly + d1;
		//				innerObstacleList.Parts[k].fSecCoeff = d1 / d;
		//				innerObstacleList.Parts[k].Flags = 1;
		//			}

		//			k++;
		//		}
		//		l++;
		//	}

		//	if (k >= 0)
		//	{
		//		Array.Resize<Obstacle>(ref innerObstacleList.Obstacles, l);
		//		Array.Resize<ObstacleData>(ref innerObstacleList.Parts, k);
		//	}
		//	else
		//	{
		//		innerObstacleList.Obstacles = new Obstacle[0];
		//		innerObstacleList.Parts = new ObstacleData[0];
		//	}
		//}

		//public static int GetLegAreaObstacles(out ObstacleContainer obstList, LegDep CurrLeg, double distFromPrevs, double hDER, double MOCLimit, WayPoint PtPrevFIX = null)
		//{
		//	int n = GlobalVars.ObstacleList.Obstacles.Length;
		//	obstList.Obstacles = new Obstacle[n];
		//	obstList.Parts = new ObstacleData[0];

		//	if (n == 0)
		//		return -1;

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

		//	int j = -1, k = -1;
		//	double maxO = double.MinValue;

		//	for (int i = 0; i < n; i++)
		//	{
		//		Point pCurrPt = (Point)GlobalVars.ObstacleList[i].pPrjGeometry;
		//		double distToFullPoly = fullGeoOp.GetDistance(pCurrPt);

		//		//if (GlobalVars.ObstacleList[i].ID == 1193)
		//		//{
		//		////    //GlobalVars.gAranGraphics.DrawPointWithText(ptCurr, 255, "pt" + GlobalVars.ObstacleList[i].ID.ToString());
		//		//    Leg.ProcessMessages();
		//		//}

		//		if (distToFullPoly > GlobalVars.ObstacleList[i].HorAccuracy)
		//			continue;

		//		obstList[++j] = GlobalVars.ObstacleList[i];

		//		//ObstacleList[j].Elev = ObstacleList[j].Height;
		//		obstList[j].Height = obstList[j].Elev - hDER;
		//		obstList[j].d0 = KKLineGeoOp.GetDistance(pCurrPt);

		//		//if (maxO < ObstacleList[j].Height)
		//		//{
		//		//	maxO = ObstacleList[j].Height;
		//		//	k = j;
		//		//}

		//		double distToPrimaryPoly = primaryGeoOp.GetDistance(pCurrPt);
		//		obstList[j].Prima = distToPrimaryPoly <= obstList[j].HorAccuracy;
		//		obstList[j].fTmp = 1.0;

		//		if (!obstList[j].Prima)
		//		{
		//			double d1 = lineStrGeoOp.GetDistance(pCurrPt);
		//			double d = distToPrimaryPoly + d1;
		//			obstList[j].fTmp = d1 / d;
		//		}

		//		obstList[j].DistStar = obstList[j].d0 + distFromPrevs;

		//		double dPDG = obstList[j].DistStar;

		//		if (PtPrevFIX != null && CurrLeg.StartFIX.FlyMode == eFlyMode.Atheight)
		//		{
		//			Point ptNearest = KKLineGeoOp.GetNearestPoint(pCurrPt);

		//			double distFromS = ARANFunctions.Point2LineDistancePrj(ptNearest, PtPrevFIX.PrjPt, CurrLeg.StartFIX.EntryDirection + ARANMath.C_PI_2);
		//			dPDG = obstList[j].d0 + Math.Abs(distFromS);
		//		}

		//		double tmpMOC = dPDG * GlobalVars.constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.dpMOC].Value;

		//		//ObstacleList[j].MOC08 = tmpMOC;

		//		if (tmpMOC > MOCLimit)
		//			tmpMOC = MOCLimit;
		//		if (tmpMOC < LowerMOCLimit)
		//			tmpMOC = LowerMOCLimit;

		//		obstList[j].MOC = tmpMOC * obstList[j].fTmp;
		//		obstList[j].ReqH = obstList[j].MOC + obstList[j].Height;
		//		obstList[j].PDG = (obstList[j].ReqH - GlobalVars.constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.dpH_abv_DER].Value) / obstList[j].DistStar;

		//		//if (PtPrevFIX != null )
		//		//{
		//		//CurrLeg.EndFIX.en
		//		//ObstacleList[j].PDG = (ObstacleList[j].ReqH - GlobalVars.constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.dpH_abv_DER].Value) / ObstacleList[j].d0;
		//		//}

		//		//double ReachedH = ObstacleList[j].DistStar

		//		if (maxO < obstList[j].PDG)
		//		{
		//			maxO = obstList[j].PDG;
		//			k = j;
		//		}

		//		obstList[j].Ignored = false;
		//	}

		//	System.Array.Resize<ObstacleType>(ref obstList, j + 1);
		//	return k;
		//}

		//public static void GetStrightAreaObstacles12(ObstacleContainer LocalObstacles, out ObstacleContainer innerObstacleList,
		//	 MultiPolygon pFullPoly, MultiPolygon pPrimaryPoly, RWYType Rwy, double depDir)
		//{
		//	int n = LocalObstacles.Length;
		//	innerObstacleList = new ObstacleType[n];

		//	if (n == 0)
		//		return;

		//	int j = -1;

		//	GeometryOperators fullGeoOp = new GeometryOperators();
		//	fullGeoOp.CurrentGeometry = pFullPoly;

		//	GeometryOperators primaryGeoOp = new GeometryOperators();
		//	primaryGeoOp.CurrentGeometry = pPrimaryPoly;

		//	GeometryOperators lineStrGeoOp = new GeometryOperators();
		//	lineStrGeoOp.CurrentGeometry = ARANFunctions.PolygonToPolyLine(pFullPoly[0]);

		//	Point PtEnd = Rwy.pPtPrj[eRWY.ptDER];

		//	for (int i = 0; i < n; i++)
		//	{
		//		//if ((i & 0x3FF) == 0)
		//		//	System.Windows.Forms.Application.DoEvents();

		//		Point ptCurr = (Point)LocalObstacles[i].pPrjGeometry;
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
		//			innerObstacleList[j].Prima = distToPrimaryPoly <= innerObstacleList[j].HorAccuracy;     //pPrimaryPoly.IsPointInside(ptCurr);
		//			innerObstacleList[j].fTmp = 1.0;

		//			if (!innerObstacleList[j].Prima)
		//			{
		//				//double d0 = pGeoOp.GetDistance(pPrimaryPoly, ptCurr);
		//				double d1 = lineStrGeoOp.GetDistance(ptCurr);
		//				double d = distToPrimaryPoly + d1;
		//				innerObstacleList[j].fTmp = d1 / d;
		//			}
		//		}
		//		//else
		//		//{
		//		//	outerObstacleList[++k] = LocalObstacles[i];

		//		//	Point ptTmp = fullGeoOp.GetNearestPoint(ptCurr);

		//		//	outerObstacleList[k].DistStar = ARANFunctions.PointToLineDistance(ptTmp, PtEnd, depDir + ARANMath.C_PI_2);

		//		//	outerObstacleList[k].d0 = distToFullPoly - outerObstacleList[k].HorAccuracy;
		//		//	if (outerObstacleList[k].d0 <= 0.0)
		//		//		outerObstacleList[k].d0 = 0.0;
		//		//}
		//	}

		//	//System.Windows.Forms.Application.DoEvents();		//|?????????????????????????????????
		//	//System.Array.Resize<ObstacleType>(ref outerObstacleList, k + 1);

		//	System.Array.Resize<ObstacleType>(ref innerObstacleList, j + 1);
		//}

		public static void TextBoxFloat(ref char KeyChar, string BoxText)
		{
			if (KeyChar < 32)
				return;

			char DecSep = (1.1).ToString().ToCharArray()[1];

			if (((KeyChar < '0') || KeyChar > '9') && KeyChar != DecSep)
				KeyChar = '\0';
			else if (KeyChar == DecSep)
			{
				if (BoxText.Contains(DecSep.ToString()))
					KeyChar = '\0';
			}
		}

		public static void TextBoxInteger(ref char KeyChar)
		{
			if (KeyChar < ' ')
				return;
			if ((KeyChar < '0') || (KeyChar > '9'))
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

		public static double CalcDERHorisontalAccuracy(RWYType rwy)
		{
			double bearingAccurasy = GlobalVars.settings.AnglePrecision * ARANMath.DegToRadValue;
			double D = rwy.TODA;
			double sigP = Math.Sqrt(rwy.StartHorAccuracy * rwy.StartHorAccuracy + rwy.TODAAccuracy * rwy.TODAAccuracy + D * D * bearingAccurasy * bearingAccurasy);   //(33)
			return sigP;

			//double theta = rwy.pPtPrj[eRWY.ptStart].M * ARANMath.DegToRadValue;
			//double cosTh = Math.Cos(theta);
			//double sinTh = Math.Sin(theta);

			//Проекционные координаты новой точки определяются следующими выражениям:
			//double X = D * cosTh;       // (25)
			//double Y = D * sinTh;       // (26)

			//расстояния D и угла θ.
			//double dXdD = cosTh;		//(27)
			//double dYdD = sinTh;		//(29)	
			//double dXdTh = -D * sinTh;	//(28)
			//double dYdTh = D * cosTh;	//(30)

			//Среднеквадратичное отклонение по проекционным координатам определяется следующими выражениями:

			//double sigX = Math.Sqrt(dXdD * dXdD * rwy.TODAAccuracy * rwy.TODAAccuracy + dXdTh * dXdTh * bearingAccurasy * bearingAccurasy);
			//double sigX_1 = Math.Sqrt(cosTh * cosTh * rwy.TODAAccuracy * rwy.TODAAccuracy + Y * Y * bearingAccurasy * bearingAccurasy);			//(31)

			//double sigY = Math.Sqrt(dYdD* dYdD * rwy.TODAAccuracy * rwy.TODAAccuracy + dYdTh* dYdTh * bearingAccurasy * bearingAccurasy);
			//double sigY_1 = Math.Sqrt(sinTh * sinTh * rwy.TODAAccuracy * rwy.TODAAccuracy + X * X * bearingAccurasy * bearingAccurasy);			//(32)

			//Общая ошибка определяется выражением:
			//double sigP = Math.Sqrt(rwy.StartHorAccuracy * rwy.StartHorAccuracy + sigX * sigX + sigY * sigY);
			//return sigP;
		}

		public static double CalcHorisontalAccuracy(WayPoint prevWPT, WayPoint wpt)
		{
			double bearingAccurasy = GlobalVars.settings.AnglePrecision * ARANMath.DegToRadValue;
			double distanceAccurasy = GlobalVars.settings.DistancePrecision;

			double D = ARANFunctions.ReturnDistanceInMeters(prevWPT.PrjPt, wpt.PrjPt);
			double sigP;

			if (prevWPT.FlyMode == eFlyMode.Atheight)
				sigP = Math.Sqrt(Math.Max(1, Math.Sqrt(0)) + D * D * bearingAccurasy * bearingAccurasy);
			else
				sigP = Math.Sqrt(distanceAccurasy * distanceAccurasy + prevWPT.HorAccuracy * prevWPT.HorAccuracy + D * D * bearingAccurasy * bearingAccurasy);   //(33)

			wpt.HorAccuracy = sigP;
			return sigP;
		}

		public static double CalcHorisontalAccuracy(Point ptFix, NavaidType GuidanceNav, NavaidType IntersectNav)
		{
			if (GuidanceNav.TypeCode == eNavaidType.DME || IntersectNav.Identifier == Guid.Empty)
				return 0;
			if (GuidanceNav.Identifier == Guid.Empty) return 0;

			if (IntersectNav.TypeCode == eNavaidType.NONE) return 0;
			if (IntersectNav.ValCnt == -2) return 0;

			const double distEps = 0.0001;
			double sqrt1_2 = 0.5 * Math.Sqrt(2.0);

			double sigL2;

			double GuidDir = ARANFunctions.ReturnAngleInRadians(GuidanceNav.pPtPrj, ptFix);
			double dNavNav, LNavNav = ARANFunctions.ReturnDistanceInMeters(GuidanceNav.pPtPrj, IntersectNav.pPtPrj);

			if (LNavNav < distEps * distEps)
			{
				sigL2 = 0.5 * (GuidanceNav.HorAccuracy * GuidanceNav.HorAccuracy + IntersectNav.HorAccuracy * IntersectNav.HorAccuracy);
				dNavNav = GuidDir;
			}
			else
			{
				dNavNav = ARANFunctions.ReturnAngleInRadians(GuidanceNav.pPtPrj, IntersectNav.pPtPrj);

				double dX = IntersectNav.pPtPrj.X - GuidanceNav.pPtPrj.X;
				double dY = IntersectNav.pPtPrj.Y - GuidanceNav.pPtPrj.Y;

				double sigX = 0.5 * dX * dX / (LNavNav * LNavNav) * (GuidanceNav.HorAccuracy * GuidanceNav.HorAccuracy + IntersectNav.HorAccuracy * IntersectNav.HorAccuracy);
				double sigY = 0.5 * dY * dY / (LNavNav * LNavNav) * (GuidanceNav.HorAccuracy * GuidanceNav.HorAccuracy + IntersectNav.HorAccuracy * IntersectNav.HorAccuracy);

				sigL2 = sigX + sigY;
			}

			double sigT2 = GlobalVars.settings.AnglePrecision * ARANMath.DegToRadValue;
			sigT2 = sigT2 * sigT2;

			double ted1 = ARANMath.SubtractAngles(dNavNav, GuidDir);// * ARANMath.DegToRadValue;
			double sinT1 = Math.Sin(ted1);
			double cosT1 = Math.Cos(ted1);
			double dY3dX3 = Math.Tan(ted1);

			double dX3dT1, dY3dT1;
			double sigY3, sigX3;

			if (IntersectNav.TypeCode == eNavaidType.DME)
			{
				double sigD2 = GlobalVars.unitConverter.DistanceToInternalUnits(GlobalVars.settings.DistancePrecision);
				sigD2 = sigD2 * sigD2;

				double GuidDist = ARANFunctions.ReturnDistanceInMeters(IntersectNav.pPtPrj, ptFix);
				double sqRoot = Math.Sqrt(GuidDist * GuidDist - LNavNav * LNavNav * sinT1 * sinT1);
				double recip = 1.0 / sqRoot;

				double dX3dL = cosT1 * cosT1 + LNavNav * cosT1 * sinT1 * sinT1 * recip;    //(14)
				double dX3dD = GuidDist * cosT1 * recip;                                   //(15)
				dX3dT1 = 2.0 * LNavNav * cosT1 * sinT1 + sinT1 * sqRoot + cosT1 * cosT1 * sinT1 * LNavNav * LNavNav * recip;    //(16)

				double sigX3_2 = dX3dL * dX3dL * sigL2 + dX3dD * dX3dD * sigD2 + dX3dT1 * dX3dT1 * sigT2;
				sigX3 = Math.Sqrt(sigX3_2);                                         //(17)

				double X3 = LNavNav * cosT1 * cosT1 + cosT1 * sqRoot;                      //(13)
				dY3dT1 = X3 / (cosT1 * cosT1);

				sigY3 = Math.Sqrt(dY3dX3 * dY3dX3 * sigX3_2 + dY3dT1 * dY3dT1 * sigT2);
			}
			else
			{
				double IntersectDir = ARANFunctions.ReturnAngleInRadians(IntersectNav.pPtPrj, ptFix);
				double ted2 = ARANMath.SubtractAngles(dNavNav, IntersectDir);	// * ARANMath.DegToRadValue;

				double dX3dY3 = 1.0 / dY3dX3;                                              //(7)
				double ctT1T2 = dX3dY3 + 1.0 / Math.Tan(ted2);

				double dY3dL = 1.0 / ctT1T2;
				double Y3 = LNavNav * dY3dL;

				dX3dT1 = -Y3 / (sinT1 * sinT1);                                       //(8)

				double fTmp = sinT1 * ctT1T2;
				dY3dT1 = -LNavNav / (fTmp * fTmp);

				fTmp = Math.Sin(ted2) * ctT1T2;
				double dY3dT2 = -LNavNav / (fTmp * fTmp);

				double sigY3_2 = dY3dL * dY3dL * sigL2 + dY3dT1 * dY3dT1 * sigT2 + dY3dT2 * dY3dT2 * sigT2;
				sigY3 = Math.Sqrt(sigY3_2);
				sigX3 = Math.Sqrt(dX3dY3 * dX3dY3 * sigY3_2 + dX3dT1 * dX3dT1 * sigT2);
			}

			double result = Math.Sqrt(sigX3 * sigX3 + sigY3 * sigY3);
			return result;
		}

		public static void SaveDerAccurasyInfo(ReportFile reportFile, RWYType rwy)
		{
			reportFile.Param("Calculated horizontal accuracy at DER", rwy.DERHorAccuracy.ToString("0.00"), "meters");
			reportFile.WriteMessage();
		}

		public static void SaveFixAccurasyInfo(ReportFile reportFile, WayPoint prevWPT, WayPoint wpt, string FixRole, bool isCalculated, bool isFinal = false)
		{
			//Fix Role				{ FAF, If... }
			//Calculated horizontal accuracy at FIX - in meters
			double distance = ARANFunctions.ReturnDistanceInMeters(prevWPT.PrjPt, wpt.PrjPt);
			double direction = ARANFunctions.ReturnAngleInRadians(prevWPT.PrjPt, wpt.PrjPt);
			double azimutht = ARANFunctions.DirToAzimuth(wpt.PrjPt, direction, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			reportFile.Param("WPT", wpt.CallSign);
			reportFile.Param("WPT Role", FixRole);

			if (prevWPT.FlyMode != eFlyMode.Atheight)
			{
				reportFile.Param("Distance from previos WPT", GlobalVars.unitConverter.DistanceToDisplayUnits(distance).ToString(), GlobalVars.unitConverter.DistanceUnit);
				reportFile.Param("Azimutht from previos WPT", GlobalVars.unitConverter.AzimuthToDisplayUnits(azimutht).ToString(), GlobalVars.unitConverter.AzimuthUnit);
			}

			if (!isCalculated)
				reportFile.Param("Horizontal accuracy at WPT", wpt.HorAccuracy.ToString("0.00"), "meters");
			else
			{

				reportFile.Param("Calculated horizontal accuracy at WPT", wpt.HorAccuracy.ToString("0.00"), "meters");
			}
			reportFile.WriteMessage();

			if (!isFinal)
			{
				reportFile.WriteMessage("=================================================");
				reportFile.WriteMessage();
			}
		}


		public static bool ShowSaveDialog(out string FileName, out string FileTitle)
		{
			System.Windows.Forms.SaveFileDialog saveDialog = new System.Windows.Forms.SaveFileDialog();

			//string ProjectPath = GlobalVars.GetMapFileName();
			//int pos = ProjectPath.LastIndexOf('\\');
			//int pos2 = ProjectPath.LastIndexOf('.');

			//SaveDialog1.DefaultExt = "";
			//SaveDialog1.InitialDirectory = ProjectPath.Substring(0, pos);
			//SaveDialog1.Title = Properties.Resources.str00511;
			//SaveDialog1.FileName = ProjectPath.Substring(0, pos2 - 1) + ".htm";

			saveDialog.FileName = "";
			saveDialog.Filter = "PANDA Report File (*.htm)|*.htm|All Files (*.*)|*.*";
			saveDialog.AddExtension = false;

			FileTitle = "";
			FileName = "";

			if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				FileName = saveDialog.FileName;

				int pos = FileName.LastIndexOf('.');
				if (pos > 0)
					FileName = FileName.Substring(0, pos);

				FileTitle = FileName;
				int pos2 = FileTitle.LastIndexOf('\\');
				if (pos2 > 0)
					FileTitle = FileTitle.Substring(pos2 + 1);

				return true;
			}

			return false;
		}

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

		//internal static void shall_SortsSort(Obstacle[] obstacles)
		//{
		//	int lastRow = obstacles.GetUpperBound(0);
		//	if (lastRow < 0)
		//		return;

		//	int firstRow = obstacles.GetLowerBound(0);
		//	int numRows = lastRow - firstRow + 1;

		//	int gapSize = 0;
		//	do
		//		gapSize = gapSize * 3 + 1;
		//	while (gapSize <= numRows);

		//	do
		//	{
		//		gapSize = gapSize / 3;
		//		for (int i = gapSize + firstRow; i <= lastRow; i++)
		//		{
		//			int curPos = i;
		//			Obstacle tmpVal = obstacles[i];

		//			while (String.Compare(obstacles[curPos - gapSize].sSort, tmpVal.sSort) > 0)
		//			{
		//				obstacles[curPos] = obstacles[curPos - gapSize];
		//				curPos = curPos - gapSize;
		//				if (curPos - gapSize < firstRow)
		//					break;
		//			}
		//			obstacles[curPos] = tmpVal;
		//		}
		//	}
		//	while (gapSize > 1);
		//}

		//internal static void shall_SortsSortD(Obstacle[] obstacles)
		//{
		//	int lastRow = obstacles.GetUpperBound(0);
		//	if (lastRow < 0)
		//		return;

		//	int firstRow = obstacles.GetLowerBound(0);
		//	int numRows = lastRow - firstRow + 1;

		//	int gapSize = 0;
		//	do
		//		gapSize = gapSize * 3 + 1;
		//	while (gapSize <= numRows);

		//	do
		//	{
		//		gapSize = gapSize / 3;
		//		for (int i = gapSize + firstRow; i <= lastRow; i++)
		//		{
		//			int curPos = i;
		//			Obstacle tmpVal = obstacles[i];

		//			while (String.Compare(obstacles[curPos - gapSize].sSort, tmpVal.sSort) < 0)
		//			{
		//				obstacles[curPos] = obstacles[curPos - gapSize];
		//				curPos = curPos - gapSize;
		//				if (curPos - gapSize < firstRow)
		//					break;
		//			}
		//			obstacles[curPos] = tmpVal;
		//		}
		//	}
		//	while (gapSize > 1);
		//}

		//internal static void shall_SortfSort(Obstacle[] obstacles)
		//{
		//	int lastRow = obstacles.GetUpperBound(0);
		//	if (lastRow < 0)
		//		return;

		//	int firstRow = obstacles.GetLowerBound(0);
		//	int numRows = lastRow - firstRow + 1;

		//	int gapSize = 0;
		//	do
		//		gapSize = gapSize * 3 + 1;
		//	while (gapSize <= numRows);

		//	do
		//	{
		//		gapSize = gapSize / 3;
		//		for (int i = gapSize + firstRow; i <= lastRow; i++)
		//		{
		//			int curPos = i;
		//			Obstacle tmpVal = obstacles[i];

		//			while (obstacles[curPos - gapSize].fSort > tmpVal.fSort)
		//			{
		//				obstacles[curPos] = obstacles[curPos - gapSize];
		//				curPos = curPos - gapSize;
		//				if (curPos - gapSize < firstRow)
		//					break;
		//			}
		//			obstacles[curPos] = tmpVal;
		//		}
		//	}
		//	while (gapSize > 1);
		//}

		//internal static void shall_SortfSortD(Obstacle[] obstacles)
		//{
		//	int lastRow = obstacles.GetUpperBound(0);
		//	if (lastRow < 0)
		//		return;

		//	int firstRow = obstacles.GetLowerBound(0);
		//	int numRows = lastRow - firstRow + 1;

		//	int gapSize = 0;
		//	do
		//		gapSize = gapSize * 3 + 1;
		//	while (gapSize <= numRows);

		//	do
		//	{
		//		gapSize = gapSize / 3;
		//		for (int i = gapSize + firstRow; i <= lastRow; i++)
		//		{
		//			int curPos = i;
		//			Obstacle tmpVal = obstacles[i];
		//			while (obstacles[curPos - gapSize].fSort < tmpVal.fSort)
		//			{
		//				obstacles[curPos] = obstacles[curPos - gapSize];
		//				curPos = curPos - gapSize;
		//				if (curPos - gapSize < firstRow)
		//					break;
		//			}
		//			obstacles[curPos] = tmpVal;
		//		}
		//	}
		//	while (gapSize > 1);
		//}

		public static double MinFlybyDistByHeightAtFIX(double dT, double derElev, double grd, double plannedAng, WayPoint wpt, double hHeight )
		{
			if (wpt.FlyMode != eFlyMode.Flyby)
			{
				if (wpt.FlyMode == eFlyMode.Atheight)
					return 0.0;

				return wpt.ATT;
			}

			if (hHeight < GlobalVars.constants.Pansops[ePANSOPSData.dpNGui_Ar1].Value)
				hHeight = GlobalVars.constants.Pansops[ePANSOPSData.dpNGui_Ar1].Value;				//120 m

			double BankInRadian = GlobalVars.constants.Pansops[ePANSOPSData.rnvFlyOInterBank].Value;
			double hAbovDer = GlobalVars.constants.Pansops[ePANSOPSData.dpH_abv_DER].Value;			//005 m
			double maxPDG = GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;

			double kTurn = Math.Tan(0.5 * plannedAng);
			double IASInMetrsInSec = wpt.IAS;

			double DistFixToFix = (hHeight - hAbovDer) / grd;
			double Altitude = derElev + hAbovDer + DistFixToFix * maxPDG;
			//double RTurn = ARANMath.BankToRadiusForRnav(BankInRadian, IASInMetrsInSec, Altitude, dT);
			double TASInMetrsInSec = ARANMath.IASToTAS(IASInMetrsInSec, Altitude, dT);
			double RTurn = ARANMath.BankToRadius(BankInRadian, TASInMetrsInSec);

			return RTurn * kTurn + wpt.ATT;
		}

		public static double MinFlybyDistByHeightAtKKLine(double dT, double derElev, double grd, double plannedAng, WayPoint wpt, double hHeight = -1)
		{
			if (wpt.FlyMode != eFlyMode.Flyby)
			{
				if (wpt.FlyMode == eFlyMode.Atheight)
					return 0.0;

				return wpt.ATT;
			}

			if (hHeight < GlobalVars.constants.Pansops[ePANSOPSData.dpNGui_Ar1].Value)
				hHeight = GlobalVars.constants.Pansops[ePANSOPSData.dpNGui_Ar1].Value;				//120 m

			double BankInRadian = GlobalVars.constants.Pansops[ePANSOPSData.rnvFlyOInterBank].Value;
			double hAbovDer = GlobalVars.constants.Pansops[ePANSOPSData.dpH_abv_DER].Value;			//005 m
			double maxPDG = GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;

			double kTurn = Math.Tan(0.5 * plannedAng);
			double IASInMetrsInSec = wpt.IAS;

			double d2KK = (hHeight - hAbovDer) / grd;

			double DistFixToFix = d2KK + wpt.ATT + 6071.35061;	//+ 6071.3506099238484;
			double Altitude = derElev + hAbovDer + DistFixToFix * maxPDG;

			double RTurn = ARANMath.BankToRadiusForRnav(BankInRadian, IASInMetrsInSec, Altitude, dT);

			double L1 = RTurn * kTurn;

			int io = 100;
			while (io >= 0)
			{
				double L1_Old = L1;

				DistFixToFix = d2KK + L1 + wpt.ATT;
				Altitude = derElev + hAbovDer + DistFixToFix * maxPDG;
				//RTurn = ARANMath.BankToRadiusForRnav(BankInRadian, IASInMetrsInSec, Altitude, dT);
				double TASInMetrsInSec = ARANMath.IASToTAS(IASInMetrsInSec, Altitude, dT);
				RTurn = ARANMath.BankToRadius(BankInRadian, TASInMetrsInSec);

				L1 = RTurn * kTurn;

				if (Math.Abs(L1 - L1_Old) < ARANMath.EpsilonDistance)
					break;
				io--;
			}
			return L1 + wpt.ATT;
		}
	}
}

/*
─────────────────────────▄▀▄  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─▀█▀█▄  
─────────────────────────█──█──█  
─────────────────────────█▄▄█──▀█  
────────────────────────▄█──▄█▄─▀█  
────────────────────────█─▄█─█─█─█  
────────────────────────█──█─█─█─█  
────────────────────────█──█─█─█─█  
────▄█▄──▄█▄────────────█──▀▀█─█─█  
──▄█████████────────────▀█───█─█▄▀  
─▄███████████────────────██──▀▀─█  
▄█████████████────────────█─────█  
██████████───▀▀█▄─────────▀█────█  
████████───▀▀▀──█──────────█────█  
██████───────██─▀█─────────█────█  
████──▄──────────▀█────────█────█ Look dude,
███──█──────▀▀█───▀█───────█────█ a good code!
███─▀─██──────█────▀█──────█────█  
███─────────────────▀█─────█────█  
███──────────────────█─────█────█  
███─────────────▄▀───█─────█────█  
████─────────▄▄██────█▄────█────█  
████────────██████────█────█────█  
█████────█──███████▀──█───▄█▄▄▄▄█  
██▀▀██────▀─██▄──▄█───█───█─────█  
██▄──────────██████───█───█─────█  
─██▄────────────▄▄────█───█─────█  
─███████─────────────▄█───█─────█  
──██████─────────────█───█▀─────█  
──▄███████▄─────────▄█──█▀──────█  
─▄█─────▄▀▀▀█───────█───█───────█  
▄█────────█──█────▄███▀▀▀▀──────█  
█──▄▀▀────────█──▄▀──█──────────█  
█────█─────────█─────█──────────█  
█────────▀█────█─────█─────────██  
█───────────────█──▄█▀─────────█  
█──────────██───█▀▀▀───────────█  
█───────────────█──────────────█  
█▄─────────────██──────────────█  
─█▄────────────█───────────────█  
──██▄────────▄███▀▀▀▀▀▄────────█  
─█▀─▀█▄────────▀█──────▀▄──────█  
─█────▀▀▀▀▄─────█────────▀─────█
*/