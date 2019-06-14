using System;
using System.Collections.Generic;
//using Aran.PANDA.CRMWall.Properties;
using System.Runtime.InteropServices;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Panda.Common;

namespace Aran.PANDA.CRMWall
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
				ptCentr = GlobalVars.RWYList[i].pPtPrj[eRWY.PtTHR];
				pLineStr.Add(ptCentr);
			}

			if (pLineStr.Count < 3)
				return ARANMath.Hypot(pLineStr[0].X - pLineStr[1].X, pLineStr[0].Y - pLineStr[1].Y);

			double result;
			minCircle.minCircleAroundPoints(pLineStr, out ptCentr, out result);

			return 2 * result;
		}

		public static void GetObstaclesByPolygon(ObstacleType[] LocalObstacles, out ObstacleType[] ObstacleList, MultiPolygon pPoly)
		{
			int n = LocalObstacles.Length;
			ObstacleList = new ObstacleType[n];
			if (n == 0)
				return;

			//GeometryOperators pGeoOp = new GeometryOperators();

			int j = -1;
			for (int i = 0; i < n; i++)
			{
				Point ptCurr = LocalObstacles[i].pPtPrj;
				//if (pGeoOp.Contains(pPoly, ptCurr))
				if (pPoly.IsPointInside(ptCurr))
					ObstacleList[++j] = LocalObstacles[i];
			}

			System.Array.Resize<ObstacleType>(ref ObstacleList, j + 1);
		}

		private static void QuickSort(ObstacleType[] obstacleArray, int iLo, int iHi)
		{
			int Lo = iLo;
			int Hi = iHi;
			double midDist = obstacleArray[(Lo + Hi) / 2].X;

			do
			{
				while (obstacleArray[Lo].X < midDist)
					Lo++;

				while (obstacleArray[Hi].X > midDist)
					Hi--;

				if (Lo <= Hi)
				{
					ObstacleType t = obstacleArray[Lo];
					obstacleArray[Lo] = obstacleArray[Hi];
					obstacleArray[Hi] = t;
					Lo++;
					Hi--;
				}
			}
			while (Lo <= Hi);

			if (Hi > iLo)
				QuickSort(obstacleArray, iLo, Hi);

			if (Lo < iHi)
				QuickSort(obstacleArray, Lo, iHi);
		}

		public static void SortByDist(ObstacleType[] obstacleArray)
		{
			int Lo = obstacleArray.GetLowerBound(0);
			int Hi = obstacleArray.GetUpperBound(0);

			if (Lo >= Hi)
				return;

			QuickSort(obstacleArray, Lo, Hi);
		}

		public static bool PriorPostFixTolerance(Polygon pPolygon, Point pPtPrj, double fDir, out double PriorDist, out double PostDist)
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

		public static void GetStrightAreaObstacles(ObstacleType[] LocalObstacles, out ObstacleType[] innerObstacleList,
			out ObstacleType[] outerObstacleList, MultiPolygon pFullPoly, MultiPolygon pPrimaryPoly, RWYType Rwy, double depDir)
		{
			int n = LocalObstacles.Length;
			innerObstacleList = new ObstacleType[n];
			outerObstacleList = new ObstacleType[n];

			if (n == 0)
				return;

			int j = -1, k = -1;

			GeometryOperators fullGeoOp = new GeometryOperators();
			fullGeoOp.CurrentGeometry = pFullPoly;

			GeometryOperators primaryGeoOp = new GeometryOperators();
			primaryGeoOp.CurrentGeometry = pPrimaryPoly;

			GeometryOperators lineStrGeoOp = new GeometryOperators();
			lineStrGeoOp.CurrentGeometry = ARANFunctions.PolygonToPolyLine(pFullPoly[0]);

			Point PtEnd = Rwy.pPtPrj[eRWY.PtDER];

			for (int i = 0; i < n; i++)
			{
				Point ptCurr = LocalObstacles[i].pPtPrj;

				ARANFunctions.PrjToLocal(PtEnd, depDir, ptCurr, out LocalObstacles[i].X, out LocalObstacles[i].Y);

				int sign = Math.Sign(LocalObstacles[i].X);
				LocalObstacles[i].Xmin = Math.Abs(LocalObstacles[i].X) - LocalObstacles[i].HorAccuracy;
				if (LocalObstacles[i].Xmin < 0.0)
					LocalObstacles[i].Xmin = 0.0;
				LocalObstacles[i].Xmin *= sign;

				sign = Math.Sign(LocalObstacles[i].Y);
				LocalObstacles[i].Ymin = Math.Abs(LocalObstacles[i].Y) - LocalObstacles[i].HorAccuracy;
				if (LocalObstacles[i].Ymin < 0.0)
					LocalObstacles[i].Ymin = 0.0;
				LocalObstacles[i].Ymin *= sign;

				double distToFullPoly = fullGeoOp.GetDistance(ptCurr);

				//if (pFullPoly.IsPointInside(ptCurr))
				//{
				//     //&& distToFullPoly > LocalObstacles[i].HorAccuracy
				//    distToFullPoly = distToFullPoly;
				//}

				if (distToFullPoly <= LocalObstacles[i].HorAccuracy) //if (pFullPoly.IsPointInside(ptCurr))
				{
					innerObstacleList[++j] = LocalObstacles[i];

					//GlobalVars.gAranGraphics.DrawPointWithText(ptCurr, 255, "ptCurr+8");
					//double clShift = ARANFunctions.PointToLineDistance(ptCurr, PtEnd, depDir);
					//double sign = Math.Sign(clShift);
					//clShift = Math.Abs(clShift) - innerObstacleList[j].HorAccuracy;
					//if (clShift < 0.0)
					//    clShift = ARANMath.Epsilon_2Distance;
					//innerObstacleList[j].CLShift = sign * clShift;

					double distToPrimaryPoly = primaryGeoOp.GetDistance(ptCurr);
					innerObstacleList[j].Prima = distToPrimaryPoly <= innerObstacleList[j].HorAccuracy;//pPrimaryPoly.IsPointInside(ptCurr);
					innerObstacleList[j].fTmp = 1.0;

					if (!innerObstacleList[j].Prima)
					{
						//double d0 = pGeoOp.GetDistance(pPrimaryPoly, ptCurr);
						double d1 = lineStrGeoOp.GetDistance(ptCurr);
						double d = distToPrimaryPoly + d1;
						innerObstacleList[j].fTmp = d1 / d;
					}
				}
				else
				{
					outerObstacleList[++k] = LocalObstacles[i];

					Point ptTmp = fullGeoOp.GetNearestPoint(ptCurr);

					//GlobalVars.gAranGraphics.DrawMultiPolygon(pFullPoly, 0, eFillStyle.sfsCross);

					outerObstacleList[k].DistStar = ARANFunctions.PointToLineDistance(ptTmp, PtEnd, depDir + ARANMath.C_PI_2);
					// -outerObstacleList[k].HorAccuracy;
					//if (outerObstacleList[k].DistStar <= 0.0)
					//    outerObstacleList[k].DistStar = ARANMath.Epsilon_2Distance;

					//GlobalVars.gAranGraphics.DrawPointWithText(ptTmp,255, "-3-ptTmp");

					//double d = ARANFunctions.ReturnDistanceInMeters(ptTmp, ptCurr);
					// d = pGeoOp.GetDistance(pFullPoly, ptCurr);

					outerObstacleList[k].d0 = distToFullPoly - outerObstacleList[k].HorAccuracy;
					if (outerObstacleList[k].d0 <= 0.0)
						outerObstacleList[k].d0 = 0.0;
				}
			}

			System.Array.Resize<ObstacleType>(ref innerObstacleList, j + 1);
			System.Array.Resize<ObstacleType>(ref outerObstacleList, k + 1);
		}

		public static void GetObstaclesByDistance(out ObstacleType[] ObstacleList, Point ptCenter, double radius)
		{
			int n = GlobalVars.ObstacleList.Length;
			ObstacleList = new ObstacleType[n];
			if (n == 0)
				return;

			int j = 0;

			for (int i = 0; i < n; i++)
			{
				Point ptCurr = GlobalVars.ObstacleList[i].pPtPrj;
				double dist = ARANFunctions.ReturnDistanceInMeters(ptCenter, ptCurr);
				if (dist <= radius)
					ObstacleList[j++] = GlobalVars.ObstacleList[i];
			}

			System.Array.Resize<ObstacleType>(ref ObstacleList, j);
		}

		public static void ConsiderObs(ref ObstacleType[] ObstList, Point ptCenter, RWYType Rwy, double fRadius, out double maxDist, out int obsNum)
		{
			maxDist = 0.0;
			obsNum = 0;

			if (fRadius == 0)
				return;

			int n = ObstList.Length;
			Point PtEnd = Rwy.pPtPrj[eRWY.PtDER];
			double CLDir = PtEnd.M;

			for (int i = 0; i < n; i++)
			{
				Point ptCurr = ObstList[i].pPtPrj;

				ObstList[i].Height = ptCurr.Z - PtEnd.Z;
				ObstList[i].HeightMax = ObstList[i].Height + ObstList[i].VertAccuracy;

				ARANFunctions.PrjToLocal(PtEnd, CLDir, ptCurr, out ObstList[i].X, out ObstList[i].Y);

				int sign = Math.Sign(ObstList[i].X);
				ObstList[i].Xmin = Math.Abs(ObstList[i].X) - ObstList[i].HorAccuracy;
				if (ObstList[i].Xmin < 0.0)
					ObstList[i].Xmin = 0.0;
				ObstList[i].Xmin *= sign;

				sign = Math.Sign(ObstList[i].Y);
				ObstList[i].Ymin = Math.Abs(ObstList[i].Y) - ObstList[i].HorAccuracy;
				if (ObstList[i].Ymin < 0.0)
					ObstList[i].Ymin = 0.0;
				ObstList[i].Ymin *= sign;

				double fDist = ARANFunctions.ReturnDistanceInMeters(ptCurr, ptCenter);
				if (fDist <= fRadius)
				{
					obsNum++;
					if (fDist > maxDist)
						maxDist = fDist;
				}
			}
		}

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
			if (navType == eNavaidType.CodeNONE)
				return "WPT";
			else
				return NavTypeNames[(int)navType];
		}

		internal static void shall_SortsSort(ObstacleType[] obstacles)
		{
			int lastRow = obstacles.GetUpperBound(0);
			if (lastRow < 0)
				return;

			int firstRow = obstacles.GetLowerBound(0);
			int numRows = lastRow - firstRow + 1;

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
					ObstacleType tmpVal = obstacles[i];

					while (String.Compare(obstacles[curPos - gapSize].sSort, tmpVal.sSort) > 0)
					{
						obstacles[curPos] = obstacles[curPos - gapSize];
						curPos = curPos - gapSize;
						if (curPos - gapSize < firstRow)
							break;
					}
					obstacles[curPos] = tmpVal;
				}
			}
			while (gapSize > 1);
		}

		internal static void shall_SortsSortD(ObstacleType[] obstacles)
		{
			int lastRow = obstacles.GetUpperBound(0);
			if (lastRow < 0)
				return;

			int firstRow = obstacles.GetLowerBound(0);
			int numRows = lastRow - firstRow + 1;

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
					ObstacleType tmpVal = obstacles[i];

					while (String.Compare(obstacles[curPos - gapSize].sSort, tmpVal.sSort) < 0)
					{
						obstacles[curPos] = obstacles[curPos - gapSize];
						curPos = curPos - gapSize;
						if (curPos - gapSize < firstRow)
							break;
					}
					obstacles[curPos] = tmpVal;
				}
			}
			while (gapSize > 1);
		}

		internal static void shall_SortfSort(ObstacleType[] obstacles)
		{
			int lastRow = obstacles.GetUpperBound(0);
			if (lastRow < 0)
				return;

			int firstRow = obstacles.GetLowerBound(0);
			int numRows = lastRow - firstRow + 1;

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
					ObstacleType tmpVal = obstacles[i];

					while (obstacles[curPos - gapSize].fSort > tmpVal.fSort)
					{
						obstacles[curPos] = obstacles[curPos - gapSize];
						curPos = curPos - gapSize;
						if (curPos - gapSize < firstRow)
							break;
					}
					obstacles[curPos] = tmpVal;
				}
			}
			while (gapSize > 1);
		}

		internal static void shall_SortfSortD(ObstacleType[] obstacles)
		{
			int lastRow = obstacles.GetUpperBound(0);
			if (lastRow < 0)
				return;

			int firstRow = obstacles.GetLowerBound(0);
			int numRows = lastRow - firstRow + 1;

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
					ObstacleType tmpVal = obstacles[i];
					while (obstacles[curPos - gapSize].fSort < tmpVal.fSort)
					{
						obstacles[curPos] = obstacles[curPos - gapSize];
						curPos = curPos - gapSize;
						if (curPos - gapSize < firstRow)
							break;
					}
					obstacles[curPos] = tmpVal;
				}
			}
			while (gapSize > 1);
		}

	}
}
