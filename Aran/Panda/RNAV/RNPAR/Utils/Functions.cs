using System;

using Aran.AranEnvironment;
using Aran.AranEnvironment.Symbols;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.PANDA.Common;

//using Aran.PANDA.RNAV.SGBAS.Properties;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Aran.Panda.RNAV.RNPAR.Context;
using Aran.Panda.RNAV.RNPAR.Core.Model;
using Aran.Panda.RNAV.RNPAR.UI.ViewModel.Procedure;
using Env = Aran.Panda.RNAV.RNPAR.Context.AppEnvironment;


namespace Aran.Panda.RNAV.RNPAR.Utils
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

            r = 2 * q;          //r = Math.Sqrt(q);
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

            if (m == 0 || n == 0 || pPoly.IsEmpty)
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

        public static void CreateOFZPlanes(Point ptLHPrj, double ArDir, double H, ref D3DPolygone[] OFZPlanes)
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
            lineStrGeoOp.CurrentGeometry = CurrLeg.FullProtectionAreaOutline(); //ARANFunctions.PolygonToPolyLine(CurrLeg.FullAssesmentArea[0]);

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


        public static bool IsInSector(Point point, Point center, Point intersec1, Point intersec2)
        {
            double az, invAz;
            ARANFunctions.ReturnGeodesicAzimuth(center, point, out az, out invAz);

            double az1, invAz1;
            ARANFunctions.ReturnGeodesicAzimuth(center, intersec1, out az1, out invAz1);

            double az2, invAz2;
            ARANFunctions.ReturnGeodesicAzimuth(center, intersec2, out az2, out invAz2);

            return ARANFunctions.AngleInSector(az, az1, az2);
        }

        public static bool IsInCircle(Point point, Point center, double radius)
        {
            return ARANFunctions.ReturnDistanceInMeters(point, center) <= radius;
        }

        public static List<WPT_FIXType> GetRfWptList(RFLeg currLeg, WPT_FIXType[] wpts, double minR, double maxR, double exitCourse, int turnDir)
        {
            var result = new List<WPT_FIXType>();
            var centerMax = ARANFunctions.LocalToPrj(currLeg.RollOutPrj, currLeg.RollOutDir, 0.0, turnDir * maxR);
            var centerMin = ARANFunctions.LocalToPrj(currLeg.RollOutPrj, currLeg.RollOutDir, 0.0, turnDir * minR);

            var exitDir = NativeMethods.Modulus(currLeg.RollOutDir + Math.PI, Math.PI * 2);
            var startDir = ARANFunctions.AztToDirection(currLeg.RollOutGeo, NativeMethods.Modulus(exitCourse + turnDir * 240), AppEnvironment.Current.SpatialContext.SpatialReferenceGeo, AppEnvironment.Current.SpatialContext.SpatialReferenceProjection);
            var maxPoint = ARANFunctions.LocalToPrj(centerMax, startDir, 0.0, -turnDir * maxR);
            var maxDir = ARANFunctions.ReturnAngleInRadians(currLeg.RollOutPrj, maxPoint);

            List<WPT_FIXType> wptList = wpts.ToList();
            foreach (var wpt in wptList)
            {
                var dir = ARANFunctions.ReturnAngleInRadians(currLeg.RollOutPrj, wpt.pPtPrj);


                if (ARANFunctions.AngleInSector(dir, turnDir > 0 ? maxDir : exitDir, turnDir > 0 ? exitDir : maxDir))
                    result.Add(wpt);
            }

            return result.Where(t => Functions.IsInCircle(t.pPtPrj, centerMax, maxR) && !Functions.IsInCircle(t.pPtPrj, centerMin, minR)).ToList(); ;
        }

        public static List<WPT_FIXType> GetStraightWptList(Point endPoint, double endDir, WPT_FIXType[] wpts, double minDistance, double maxDistance)
        {
            double fE = Env.Current.RNPContext.ErrorAngle;
            List<WPT_FIXType> wptList = wpts.ToList();
            var result = new List<WPT_FIXType>();

            foreach (var wpt in wptList)
            {
                var dir = ARANFunctions.ReturnAngleInRadians(wpt.pPtPrj, endPoint);
                double fTmp = endDir - dir;

                if ((System.Math.Abs(Math.Sin(fTmp)) <= fE) && (Math.Cos(fTmp) > 0))
                {
                    var distance = ARANFunctions.ReturnDistanceInMeters(endPoint, wpt.pPtPrj);
                    if (distance < minDistance || distance > maxDistance)
                        continue;
                    result.Add(wpt);
                }

            }
            return result;
        }

        
        public static double Dir2Azt(Point pPtPrj, double dir)
        {
            return ARANFunctions.DirToAzimuth(pPtPrj, dir, Env.Current.SpatialContext.SpatialReferenceProjection, Env.Current.SpatialContext.SpatialReferenceGeo);
        }

        public static double ConvertHeight(double Val_Renamed, eRoundMode RoundMode = eRoundMode.NEAREST)
        {
            //special changed to special_floor
            if (RoundMode < eRoundMode.NONE || RoundMode > eRoundMode.SPECIAL_FLOOR) RoundMode = eRoundMode.NONE;

            var heightConverter = Env.Current.UnitContext.UnitConverter.HeightConverter[Env.Current.UnitContext.UnitConverter.HeightUnitIndex];

            switch (RoundMode)
            {
                case eRoundMode.NONE:
                    return Val_Renamed * heightConverter.Multiplier;
                case eRoundMode.FLOOR:
                //return System.Math.Round(Val_Renamed * heightConverter.Multiplier / heightConverter.Rounding - 0.4999) * heightConverter.Rounding;
                case eRoundMode.CEIL:
                //return System.Math.Round(Val_Renamed * heightConverter.Multiplier / heightConverter.Rounding + 0.4999) * heightConverter.Rounding;
                case eRoundMode.NEAREST:
                    return System.Math.Round(Val_Renamed * heightConverter.Multiplier / heightConverter.Rounding) * heightConverter.Rounding;
                case eRoundMode.SPECIAL_FLOOR:
                    if (Env.Current.UnitContext.UnitConverter.HeightUnitIndex == 0)
                        return System.Math.Round(Val_Renamed * Env.Current.UnitContext.UnitConverter.HeightConverter[Env.Current.UnitContext.UnitConverter.HeightUnitIndex].Multiplier / 50.0) * 50.0;
                    else if (Env.Current.UnitContext.UnitConverter.HeightUnitIndex == 1)
                        return System.Math.Round(Val_Renamed * heightConverter.Multiplier / 100.0) * 100.0;
                    else
                        return System.Math.Round(Val_Renamed * heightConverter.Multiplier / heightConverter.Rounding) * heightConverter.Rounding;
            }

            return Val_Renamed;
        }

        public static double ConvertDistance(double Val_Renamed, eRoundMode RoundMode = eRoundMode.NEAREST)
        {
            if (RoundMode < eRoundMode.NONE || RoundMode > eRoundMode.CEIL) RoundMode = eRoundMode.NONE;

            var distanceConverter = Env.Current.UnitContext.UnitConverter.DistanceConverter[Env.Current.UnitContext.UnitConverter.DistanceUnitIndex];

            switch (RoundMode)
            {
                case eRoundMode.NONE:
                    return Val_Renamed * distanceConverter.Multiplier;
                case eRoundMode.FLOOR:
                //return System.Math.Round(Val_Renamed * distanceConverter.Multiplier / distanceConverter.Rounding - 0.4999) * distanceConverter.Rounding;
                case eRoundMode.CEIL:
                //return System.Math.Round(Val_Renamed * distanceConverter.Multiplier / distanceConverter.Rounding + 0.4999) * distanceConverter.Rounding;
                case eRoundMode.NEAREST:
                    return System.Math.Round(Val_Renamed * distanceConverter.Multiplier / distanceConverter.Rounding) * distanceConverter.Rounding;
            }
            return Val_Renamed;
        }

        public static bool ShowSaveDialog(out string FileName, out string FileTitle)
        {
            System.Windows.Forms.SaveFileDialog SaveDialog1 = new System.Windows.Forms.SaveFileDialog();
            //TODO RNPAR get project file location
            string ProjectPath = Env.Current.AranEnv.DocumentFileName; 
            int pos = ProjectPath.LastIndexOf('\\');
            int pos2 = ProjectPath.LastIndexOf('.');

            SaveDialog1.DefaultExt = "";
            SaveDialog1.InitialDirectory = ProjectPath.Substring(0, pos);
            //SaveDialog1.Title = Aran.PANDA.Departure.Properties.Resources.str00511;
            SaveDialog1.FileName = ProjectPath.Substring(0, pos2) + ".htm";
            SaveDialog1.Filter = "PANDA Report File (*.htm)|*.htm|All Files (*.*)|*.*";

            FileTitle = "";
            FileName =
                $"RNP AR APCH{Env.Current.RNPContext.PreFinalPhase.CurrentState._SelectedRWY.Name}_{((AircraftCategory) Env.Current.RNPContext.PreFinalPhase.CurrentState._Category)}";

            if (SaveDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileName = SaveDialog1.FileName;
                FileTitle = FileName;

                pos = FileTitle.LastIndexOf('.');
                if (pos >= 0)
                    FileTitle = FileTitle.Substring(0, pos);

                pos2 = FileTitle.LastIndexOf('\\');
                if (pos2 >= 0)
                    FileTitle = FileTitle.Substring(pos2 + 1);

                return true;
            }

            return false;
        }

        public static void SaveFixAccurasyInfo(ReportFile reportFile, Point ptFix, string FixRole, bool isFinal = false)
        {
            //Fix Role				{ FAF, If... }
            //Calculated horizontal accuracy at FIX - in meters

            //TODO RNPAR calculate actual accuracy
            double HorAccuracy = 0.0;//CalcHorisontalAccuracy(ptFix, GuidanceNav, IntersectNav);

            reportFile.Param("Fix Role", FixRole);
            reportFile.Param("Calculated horizontal accuracy at FIX", HorAccuracy.ToString("0.00"), "meters");
            reportFile.WriteMessage();

            if (!isFinal)
            {
                reportFile.WriteMessage("=================================================");
                reportFile.WriteMessage();
            }
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
                xSec = (xMin - xIMin) * 60.0;   //		xSec = System.Math.Round((xMin - xIMin) * 60.0, 2);
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
    }
}
