using System;
using System.Collections.Generic;

namespace OASCalculator
{
	//public enum eGPAngle : int
	//{
	//    g25 = 0,
	//    g30 = 1,
	//    g35 = 2
	//}

	//public enum eMissGrad : int
	//{
	//    m20 = 0,
	//    m25 = 1,
	//    m30 = 2,
	//    m40 = 3,
	//    m50 = 4
	//}

	//public enum eLocTHR : int
	//{
	//    m20 = 0,
	//    m30 = 1,
	//    m38 = 2,
	//    m45 = 3
	//}

	[System.Runtime.InteropServices.ComVisible(false)]
    public class Point
	{
		public Point(double x, double y)
		{
			X = x;
			Y = y;
			Z = 0.0;
		}

		public Point(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public double X, Y, Z;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public class Line
	{
		public Line()
		{
			Start = null;
			End = null;
		}

		public Line(Point pt1, Point pt2)
		{
			Start = pt1;
			End = pt2;
		}

		public Point Start, End;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public class Polygon
	{
		public Polygon()
		{
			Points = new List<Point>();
		}

		public void Add(Point pt)
		{
			Points.Add(pt);
		}
		public List<Point> Points;

		internal void Insert(int i, Point point)
		{
			Points.Insert(i, point);
		}

		public int Count { get { return Points.Count; } }

		public Point this[int i]
		{
			get { return Points[i]; }
			set { Points[i] = value; }
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public static class eOAS
	{
		public const int
		ZeroPlane = 0,
		WPlane = 1,
		XlPlane = 2,
		YlPlane = 3,
		ZPlane = 4,
		YrPlane = 5,
		XrPlane = 6,
		WsPlane = 7,
		CommonPlane = 8,
		NonPrec = 9,

		YPlane = 10,
		XPlane = 11;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public enum eFlightCategory
	{
		CategoryI = 0,
		CategoryISBAS = 1,
		CategoryII = 2,
		CategoryIIAuto = 3,
		APVI = 4,
		APVII = 5
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct D3Point
	{
		public double X;
		public double Y;
		public double Z;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct D3Line
	{
		public double A;
		public double B;
		public double C;
		//public D3Point origin;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct D3Plane
	{
		public double U, V, W;
		public double A;
		public double B;
		public double C;
		public double D;
		public D3Point origin;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct D3DPolygone
	{
		public Polygon Poly;
		public D3Plane Plane;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public static class Common
	{
		static double RadToDeg(double x)
		{
			return x * Math.PI / 180.0;
		}

		static Point LocalToPrj(Point center, double dirInRadian, Point ptPrj)
		{
			double SinA = Math.Sin(dirInRadian);
			double CosA = Math.Cos(dirInRadian);
			double Xnew = center.X + ptPrj.X * CosA - ptPrj.Y * SinA;
			double Ynew = center.Y + ptPrj.X * SinA + ptPrj.Y * CosA;
			return new Point(Xnew, Ynew);
		}

		public const double catOASTermHeight = 300.0;
		public const double baseRDHHeight = 15.0;

		public static double[] GPNodes = new double[3] { 2.5, 3.0, 3.5 };
		public static double[] LocTHRDists = new double[4] { 2000.0, 3000.0, 3800.0, 4500.0 };
		public static double[] MisAprGr = new double[5] { 2.0, 2.5, 3.0, 4.0, 5.0 };
		public static double[] WANodes = new double[3] { 0.0239, 0.0285, 0.0331 };

		#region category I, category I-APV, APV-I, APV-II
		public static double[,] XBNodesI = new double[4, 3]
		{
			{0.1500, 0.1688, 0.1975},		// 2000
			{0.1625, 0.1825, 0.2138},		// 3000
			{0.1681, 0.1888, 0.2188},		// 3800
			{0.1738, 0.1900, 0.2200},		// 4500
		};

		public static D3Point[] CSNodesI = new D3Point[3] { new D3Point { X = 394.0, Y = 33.0, Z = 0.0 }, new D3Point { X = 281.0, Y = 49.0, Z = 0.0 }, new D3Point { X = 195.0, Y = 68.0, Z = 0.0 } };
		public static D3Point[] DSNodesI = new D3Point[3] { new D3Point { X = -344.0, Y = 135.0, Z = 0.0 }, new D3Point { X = -286.0, Y = 135.0, Z = 0.0 }, new D3Point { X = -245.0, Y = 135.0, Z = 0.0 } };

		public static D3Point[,] ESNodesI = new D3Point[5, 3]
		{
			{new D3Point { X = -900.0, Y = 201.0, Z = 0.0 }, new D3Point { X = -900.0, Y = 213.0, Z = 0.0 }, new D3Point { X = -900.0, Y = 223.0, Z = 0.0 } },	// 2.0
			{new D3Point { X = -900.0, Y = 193.0, Z = 0.0 }, new D3Point { X = -900.0, Y = 205.0, Z = 0.0 }, new D3Point { X = -900.0, Y = 216.0, Z = 0.0 } },	// 2.5
			{new D3Point { X = -900.0, Y = 187.0, Z = 0.0 }, new D3Point { X = -900.0, Y = 198.0, Z = 0.0 }, new D3Point { X = -900.0, Y = 209.0, Z = 0.0 } },	// 3.0
			{new D3Point { X = -900.0, Y = 177.0, Z = 0.0 }, new D3Point { X = -900.0, Y = 187.0, Z = 0.0 }, new D3Point { X = -900.0, Y = 197.0, Z = 0.0 } },	// 4.0
			{new D3Point { X = -900.0, Y = 169.0, Z = 0.0 }, new D3Point { X = -900.0, Y = 178.0, Z = 0.0 }, new D3Point { X = -900.0, Y = 188.0, Z = 0.0 } },	// 5.0
		};
		#endregion

		#region category II
		//public static double[,] XBNodesII = new double[4, 3]
		//{
		//    {0.1500, 0.1688, 0.1975},		// 2000
		//    {0.1625, 0.1825, 0.2138},		// 3000
		//    {0.1681, 0.1888, 0.2188},		// 3800
		//    {0.1738, 0.1900, 0.2200},		// 4500
		//};

		//public static D3Point[] CSNodesII = new D3Point[3] { new D3Point { X = 394.0, Y = 33.0, Z = 0.0 }, new D3Point { X = 281.0, Y = 49.0, Z = 0.0 }, new D3Point { X = 195.0, Y = 68.0, Z = 0.0 } };
		//public static D3Point[] DSNodesII = new D3Point[3] { new D3Point { X = -344.0, Y = 135.0, Z = 0.0 }, new D3Point { X = -286.0, Y = 135.0, Z = 0.0 }, new D3Point { X = -245.0, Y = 135.0, Z = 0.0 } };

		//public static D3Point[,] ESNodesII = new D3Point[5, 3]
		//{
		//    {new D3Point { X = -900.0, Y = 201.0, Z = 0.0 }, new D3Point { X = -900.0, Y = 213.0, Z = 0.0 }, new D3Point { X = -900.0, Y = 223.0, Z = 0.0 } },	// 2.0
		//    {new D3Point { X = -900.0, Y = 193.0, Z = 0.0 }, new D3Point { X = -900.0, Y = 205.0, Z = 0.0 }, new D3Point { X = -900.0, Y = 216.0, Z = 0.0 } },	// 2.5
		//    {new D3Point { X = -900.0, Y = 187.0, Z = 0.0 }, new D3Point { X = -900.0, Y = 198.0, Z = 0.0 }, new D3Point { X = -900.0, Y = 209.0, Z = 0.0 } },	// 3.0
		//    {new D3Point { X = -900.0, Y = 177.0, Z = 0.0 }, new D3Point { X = -900.0, Y = 187.0, Z = 0.0 }, new D3Point { X = -900.0, Y = 197.0, Z = 0.0 } },	// 4.0
		//    {new D3Point { X = -900.0, Y = 169.0, Z = 0.0 }, new D3Point { X = -900.0, Y = 178.0, Z = 0.0 }, new D3Point { X = -900.0, Y = 188.0, Z = 0.0 } },	// 5.0
		//};
		#endregion


		//category I
		//public static double[] CSxNodes = new double[3] { 394.0, 281.0, 195.0 };
		//public static double[] CSyNodes = new double[3] { 33.0, 49.0, 68.0 };
		//public static double[] DSxNodes = new double[3] { -344.0, -286.0, -245.0 };
		//public static double[] DSyNodes = new double[3] { 135.0, 135.0, 135.0 };

		public static double Det2(double a1X, double a1Y, double a2X, double a2Y)
		{
			return a1X * a2Y - a2X * a1Y;
		}

		//public static double Det2(D3Point a1, D3Point a2)
		//{
		//	return a1.X * a2.Y - a1.Y * a2.X;
		//}

		public static double WW1Inter(D3Plane w, D3Plane w1)
		{
			double result = (w.D - w1.D) / (w1.A - w.A);
			return result;
		}

		public static D3Line IntersectPlanes(D3Plane pl0, D3Plane pl1)
		{
			D3Line result;

			result.A = pl1.A - pl0.A;
			result.B = pl1.B - pl0.B;
			result.C = pl1.D - pl0.D;

			return result;
		}

		//public static D3Point IntersectPlanes(double p1A, double p1B, double p1C, double p2A, double p2B, double p2C, double H)
		//{
		//	double d = p1A * p2B - p1B * p2A;

		//	if (d == 0.0)
		//		return default(D3Point);

		//	d = 1.0 / d;

		//	D3Point result;

		//	result.X = Det2(H - p1C, p1B, H - p2C, p2B) * d;
		//	result.Y = Det2(p1A, H - p1C, p2A, H - p2C) * d;
		//	result.Z = H;

		//	return result;
		//}

		public static D3Point IntersectPlanes(D3Plane pl0, D3Plane pl1, double H)
		{
			double d = pl0.A * pl1.B - pl0.B * pl1.A;
			if (d == 0.0)
				return default(D3Point);

			d = 1.0 / d;

			D3Point result;

			result.X = Det2(-pl0.C * H - pl0.D, pl0.B, -pl0.C * H - pl1.D, pl1.B) * d;
			result.Y = Det2(pl0.A, -pl0.C * H - pl0.D, pl1.A, -pl0.C * H - pl1.D) * d;
			result.Z = H;

			return result;
		}

		public static Line IntersectPlanes(D3Plane pl0, D3Plane pl1, double hMin, double hMax)
		{
			double d = pl0.A * pl1.B - pl0.B * pl1.A;
			if (d == 0.0)
				return new Line();

			d = 1.0 / d;

			double x = Det2(-(pl0.C * hMin + pl0.D), pl0.B, -(pl1.C * hMin + pl1.D), pl1.B) * d;
			double y = Det2(pl0.A, -(pl0.C * hMin + pl0.D), pl1.A, -(pl1.C * hMin + pl1.D)) * d;
			Point pt0 = new Point(x, y, hMin);

			x = Det2(-(pl0.C * hMax + pl0.D), pl0.B, -(pl1.C * hMax + pl1.D), pl1.B) * d;
			y = Det2(pl0.A, -(pl0.C * hMax + pl0.D), pl1.A, -(pl1.C * hMax + pl1.D)) * d;
			Point pt1 = new Point(x, y, hMax);

			Line result = new Line(pt0, pt1);
			return result;
		}

		public static D3Plane PlaneFrom3Pt(D3Point pt0, D3Point pt1, D3Point pt2)
		{
			double d = (pt2.X - pt0.X) * (pt1.Y - pt0.Y) - (pt1.X - pt0.X) * (pt2.Y - pt0.Y);

			if (d == 0.0)
				return default(D3Plane);
			d = 1.0 / d;

			D3Plane result;

			result.A = d * ((pt1.Y - pt0.Y) * (pt2.Z - pt0.Z) - (pt2.Y - pt0.Y) * (pt1.Z - pt0.Z));
			result.B = d * ((pt1.Z - pt0.Z) * (pt2.X - pt0.X) - (pt2.Z - pt0.Z) * (pt1.X - pt0.X));
			result.C = -1.0;
			result.D = pt0.Z - result.A * pt0.X - result.B * pt0.Y;

			result.U = result.V = result.W = 0.0;
			result.origin = pt0;

			return result;
		}

		public static double LagrangeI(double x, double[] xNods, double[] yNods)
		{
			int n = xNods.Length;

			if (xNods.Length != n)
				throw new Exception("X[] and Y[] arrays have different sizes in \"LagrangeI\".");

			double result = 0.0;

			for (int i = 0; i < n; i++)
			{
				double lx = 1.0;

				for (int j = 0; j < n; j++)
				{
					if (i == j)
						continue;

					lx *= (x - xNods[j]) / (xNods[i] - xNods[j]);
				}

				result += yNods[i] * lx;
			}

			return result;
		}

		public static D3Point LagrangeI(double x, double[] xNods, D3Point[] yNods)
		{
			int n = xNods.Length;

			if (xNods.Length != n)
				throw new Exception("X[] and Y[] arrays have different sizes in \"LagrangeI\".");

			D3Point result;
			result.X = 0.0; result.Y = 0.0; result.Z = 0.0;

			for (int i = 0; i < n; i++)
			{
				double lx = 1.0;

				for (int j = 0; j < n; j++)
					if (i != j)
						lx *= (x - xNods[j]) / (xNods[i] - xNods[j]);

				result.X += yNods[i].X * lx;
				result.Y += yNods[i].Y * lx;
				result.Z += yNods[i].Z * lx;
			}

			return result;
		}

		public static double LagrangeII(double x, double y, double[,] zNods, double[] xNods, double[] yNods)
		{
			int n = zNods.GetUpperBound(1) + 1;
			int m = zNods.GetUpperBound(0) + 1;

			if (xNods.Length != n || yNods.Length != m)
				throw new Exception("X[] and Y[] arrays have different sizes in \"LagrangeII\".");

			double[] lx = new double[n];
			double[] ly = new double[m];

			for (int i = 0; i < n; i++)
			{
				double lx1 = 1.0;

				for (int j = 0; j < n; j++)
				{
					if (i == j)
						continue;

					lx1 *= (x - xNods[j]) / (xNods[i] - xNods[j]);
				}

				lx[i] = lx1;
			}

			for (int i = 0; i < m; i++)
			{
				double ly1 = 1.0;

				for (int j = 0; j < m; j++)
				{
					if (i == j)
						continue;

					ly1 *= (y - yNods[j]) / (yNods[i] - yNods[j]);
				}

				ly[i] = ly1;
			}

			double result = 0.0;

			for (int i = 0; i < n; i++)
			{
				double partialSum = 0.0;

				for (int j = 0; j < m; j++)
					partialSum += zNods[j, i] * ly[j];

				result += partialSum * lx[i];
			}

			return result;
		}

		public static D3Point LagrangeII(double x, double y, D3Point[,] zNods, double[] xNods, double[] yNods)
		{
			int n = zNods.GetUpperBound(1) + 1;
			int m = zNods.GetUpperBound(0) + 1;

			if (xNods.Length != n || yNods.Length != m)
				throw new Exception("X[] and Y[] arrays have different sizes in \"LagrangeII\".");

			double[] lx = new double[n];
			double[] ly = new double[m];


			for (int i = 0; i < n; i++)
			{
				double lx1 = 1.0;

				for (int j = 0; j < n; j++)
				{
					if (i == j)
						continue;

					lx1 *= (x - xNods[j]) / (xNods[i] - xNods[j]);
				}

				lx[i] = lx1;
			}

			for (int i = 0; i < m; i++)
			{
				double ly1 = 1.0;

				for (int j = 0; j < m; j++)
				{
					if (i == j)
						continue;

					ly1 *= (y - yNods[j]) / (yNods[i] - yNods[j]);
				}

				ly[i] = ly1;
			}

			D3Point result;
			result.X = 0.0; result.Y = 0.0; result.Z = 0.0;

			for (int i = 0; i < n; i++)
			{
				double partialSumX = 0.0;
				double partialSumY = 0.0;
				double partialSumZ = 0.0;

				for (int j = 0; j < m; j++)
				{
					partialSumX += zNods[j, i].X * ly[j];
					partialSumY += zNods[j, i].Y * ly[j];
					partialSumZ += zNods[j, i].Z * ly[j];
				}

				result.X += partialSumX * lx[i];
				result.Y += partialSumY * lx[i];
				result.Z += partialSumZ * lx[i];
			}

			return result;
		}

		public static void CalcOASPlanesForValues(Point ptLHPrj, double ArDir, double hMax, eFlightCategory flightCat, double LLZTHRDist, double GPAvalue, double MAGvalue, double rdhValue, double Ss, double St, ref D3DPolygone[] OASPlanes)
		{
			D3Point PointC, PointD, PointE, PointD2;

			double RDHCorr = rdhValue - 15.0;
			double CwCorr = St - 6.0 - RDHCorr;
			double Cw_Corr = St - 6.0 - RDHCorr;
			double TanGPA = Math.Tan(GPAvalue);
			double GPAinDeg = RadToDeg(GPAvalue);
			MAGvalue *= 100.0;

			OASPlanes[eOAS.ZeroPlane].Plane.A = 0.0;
			OASPlanes[eOAS.ZeroPlane].Plane.B = 0.0;
			OASPlanes[eOAS.ZeroPlane].Plane.C = -1.0;
			OASPlanes[eOAS.ZeroPlane].Plane.D = 0.0;

			OASPlanes[eOAS.CommonPlane].Plane.A = 0.0;
			OASPlanes[eOAS.CommonPlane].Plane.B = 0.0;
			OASPlanes[eOAS.CommonPlane].Plane.C = -1.0;
			OASPlanes[eOAS.CommonPlane].Plane.D = 300.0;

			PointC = Common.LagrangeI(GPAinDeg, Common.GPNodes, Common.CSNodesI);

			OASPlanes[eOAS.WPlane].Plane.A = Common.LagrangeI(GPAinDeg, Common.GPNodes, Common.WANodes);
			OASPlanes[eOAS.WPlane].Plane.B = 0.0;
			OASPlanes[eOAS.WPlane].Plane.C = -1.0;
			OASPlanes[eOAS.WPlane].Plane.D = -PointC.X * OASPlanes[eOAS.WPlane].Plane.A;

			PointD = Common.LagrangeI(GPAinDeg, Common.GPNodes, Common.DSNodesI);

			OASPlanes[eOAS.XPlane].Plane.B = Common.LagrangeII(GPAinDeg, LLZTHRDist, Common.XBNodesI, Common.GPNodes, Common.LocTHRDists);
			OASPlanes[eOAS.XPlane].Plane.A = -OASPlanes[eOAS.XPlane].Plane.B * (PointD.Y - PointC.Y) / (PointD.X - PointC.X);
			OASPlanes[eOAS.XPlane].Plane.C = -1.0;
			OASPlanes[eOAS.XPlane].Plane.D = OASPlanes[eOAS.XPlane].Plane.B * (PointD.Y * PointC.X - PointC.Y * PointD.X) / (PointD.X - PointC.X);

			PointD2.Z = Common.catOASTermHeight;
			PointD2.X = (PointD2.Z - Common.baseRDHHeight) / TanGPA;
			PointD2.Y = (PointD2.Z - OASPlanes[eOAS.XPlane].Plane.A * PointD2.X - OASPlanes[eOAS.XPlane].Plane.D) / OASPlanes[eOAS.XPlane].Plane.B;

			PointE = Common.LagrangeII(GPAinDeg, MAGvalue, Common.ESNodesI, Common.GPNodes, Common.MisAprGr);
			OASPlanes[eOAS.YPlane].Plane = Common.PlaneFrom3Pt(PointD, PointE, PointD2);

			OASPlanes[eOAS.ZPlane].Plane.A = -0.01 * MAGvalue;
			OASPlanes[eOAS.ZPlane].Plane.B = 0.0;
			OASPlanes[eOAS.ZPlane].Plane.C = -1.0;
			OASPlanes[eOAS.ZPlane].Plane.D = -OASPlanes[eOAS.ZPlane].Plane.A * PointE.X;

			// Region "W*"
			//_PointE2 = Common.Calc2Equation(_PlaneY, _PlaneZ, Common.catOASTermHeight);

			if (flightCat >= eFlightCategory.APVI)
			{
				double dd = 50.0;
				if (flightCat == eFlightCategory.APVII)
					dd = 20.0;

				OASPlanes[eOAS.WsPlane].Plane.A = Math.Tan(0.75 * GPAvalue);
				OASPlanes[eOAS.WsPlane].Plane.B = 0.0;
				OASPlanes[eOAS.WsPlane].Plane.C = -1.0;
				OASPlanes[eOAS.WsPlane].Plane.D = OASPlanes[eOAS.WsPlane].Plane.A * rdhValue / TanGPA - dd;
			}

			double APVCorr = 38.0;
			if (flightCat == eFlightCategory.APVII)
				APVCorr = 8.0;

			PointE.X = -900.0 - APVCorr / TanGPA;
			OASPlanes[eOAS.ZPlane].Plane.D = -OASPlanes[eOAS.ZPlane].Plane.A * PointE.X;

			double dRDH = rdhValue - Common.baseRDHHeight;
			OASPlanes[eOAS.WPlane].Plane.D += dRDH;
			OASPlanes[eOAS.WsPlane].Plane.D += dRDH;
			OASPlanes[eOAS.XPlane].Plane.D += dRDH;
			OASPlanes[eOAS.YPlane].Plane.D += dRDH;

			OASPlanes[eOAS.XPlane].Plane.D -= APVCorr;
			OASPlanes[eOAS.YPlane].Plane.D -= APVCorr;

			//==============================================================================//

			Line ResLine;

			int i, n = OASPlanes.Length;

			for (i = 0; i < n; i++)
				OASPlanes[i].Poly = new Polygon();

			double hXY, hYZ;
			if (flightCat == eFlightCategory.CategoryISBAS || flightCat == eFlightCategory.APVI || flightCat == eFlightCategory.APVII)
			{
				D3Point pt3Tmp;
				D3Line xyLine = IntersectPlanes(OASPlanes[eOAS.YlPlane].Plane, OASPlanes[eOAS.XlPlane].Plane);
				pt3Tmp.Y = -1759.4;
				pt3Tmp.X = -(pt3Tmp.Y * xyLine.B + xyLine.C) / xyLine.A;
				hXY = OASPlanes[eOAS.XlPlane].Plane.A * pt3Tmp.X + OASPlanes[eOAS.XlPlane].Plane.B * pt3Tmp.Y + OASPlanes[eOAS.XlPlane].Plane.D;
				//hXY = GlobalVars.arHOASPlaneCat1;

				D3Line yzLine = IntersectPlanes(OASPlanes[eOAS.YlPlane].Plane, OASPlanes[eOAS.ZPlane].Plane);
				pt3Tmp.Y = -1759.4;
				pt3Tmp.X = -(pt3Tmp.Y * yzLine.B + yzLine.C) / yzLine.A;
				hYZ = OASPlanes[eOAS.ZPlane].Plane.A * pt3Tmp.X + OASPlanes[eOAS.ZPlane].Plane.D;
			}
			else
			{
				if (flightCat == eFlightCategory.CategoryI)
					hXY = hYZ = 300.0;		//	hXY = GlobalVars.arHOASPlaneCat1;
				else
					hXY = hYZ = 300.0;		//	hXY = GlobalVars.arHOASPlaneCat2;
			}

			for (i = eOAS.XlPlane; i < eOAS.XrPlane; i++)
			{
				if (i == eOAS.ZPlane || i + 1 == eOAS.ZPlane)
					ResLine = IntersectPlanes(OASPlanes[i].Plane, OASPlanes[i + 1].Plane, 0.0, hYZ);
				else
					ResLine = IntersectPlanes(OASPlanes[i].Plane, OASPlanes[i + 1].Plane, 0.0, hXY);

				RotateAndOffset(ArDir + Math.PI, ptLHPrj, ResLine);
				OASPlanes[0].Poly.Add(ResLine.Start);
				OASPlanes[n - 1].Poly.Add(ResLine.End);
			}

			D3Line WxWs = IntersectPlanes(OASPlanes[eOAS.WPlane].Plane, OASPlanes[eOAS.WsPlane].Plane);
			double xWWs = -WxWs.C / WxWs.A;
			double hWWs = OASPlanes[eOAS.WPlane].Plane.A * xWWs + OASPlanes[eOAS.WPlane].Plane.D;
			bool haveOmegaS = hWWs > 0.0;

			if (haveOmegaS)
			{
				ResLine = IntersectPlanes(OASPlanes[eOAS.XrPlane].Plane, OASPlanes[eOAS.WsPlane].Plane, 0.0, hWWs);
				RotateAndOffset(ArDir + Math.PI, ptLHPrj, ResLine);

				OASPlanes[0].Poly.Add(ResLine.Start);
				OASPlanes[eOAS.WsPlane].Poly.Add(ResLine.Start);
				OASPlanes[eOAS.WsPlane].Poly.Add(ResLine.End);

				ResLine = IntersectPlanes(OASPlanes[eOAS.XlPlane].Plane, OASPlanes[eOAS.WsPlane].Plane, 0.0, hWWs);
				RotateAndOffset(ArDir + Math.PI, ptLHPrj, ResLine);

				OASPlanes[0].Poly.Insert(0, ResLine.Start);

				OASPlanes[eOAS.WsPlane].Poly.Add(ResLine.End);
				OASPlanes[eOAS.WsPlane].Poly.Add(ResLine.Start);
			}
			else
				hWWs = 0.0;

			ResLine = IntersectPlanes(OASPlanes[eOAS.XrPlane].Plane, OASPlanes[eOAS.WPlane].Plane, hWWs, hMax);
			RotateAndOffset(ArDir + Math.PI, ptLHPrj, ResLine);

			OASPlanes[eOAS.WPlane].Poly.Add(ResLine.Start);
			OASPlanes[eOAS.WPlane].Poly.Add(ResLine.End);
			if (!haveOmegaS)
				OASPlanes[0].Poly.Add(ResLine.Start);
			OASPlanes[n - 1].Poly.Add(ResLine.End);

			ResLine = IntersectPlanes(OASPlanes[eOAS.XlPlane].Plane, OASPlanes[eOAS.WPlane].Plane, hWWs, hMax);
			RotateAndOffset(ArDir + Math.PI, ptLHPrj, ResLine);

			OASPlanes[eOAS.WPlane].Poly.Add(ResLine.End);
			OASPlanes[eOAS.WPlane].Poly.Add(ResLine.Start);
			if (!haveOmegaS)
				OASPlanes[0].Poly.Insert(0, ResLine.Start);

			OASPlanes[n - 1].Poly.Insert(0, ResLine.End);

			int j, k, m = OASPlanes[0].Poly.Count;
			for (j = 0, i = eOAS.XlPlane; i < eOAS.WsPlane; i++, j++)
			{
				k = (j + 1) % m;

				if (haveOmegaS && i == eOAS.XlPlane)
					OASPlanes[i].Poly.Add(OASPlanes[eOAS.WsPlane].Poly[2]);

				OASPlanes[i].Poly.Add(OASPlanes[0].Poly[j]);
				OASPlanes[i].Poly.Add(OASPlanes[0].Poly[k]);

				if (haveOmegaS && i == eOAS.XrPlane)
					OASPlanes[i].Poly.Add(OASPlanes[eOAS.WsPlane].Poly[1]);

				if (i == eOAS.XlPlane)
				{
					ResLine = IntersectPlanes(OASPlanes[eOAS.XlPlane].Plane, OASPlanes[eOAS.YlPlane].Plane, hWWs, hMax);
					RotateAndOffset(ArDir + Math.PI, ptLHPrj, ResLine);
					OASPlanes[i].Poly.Add(ResLine.End);
				}
				else
					OASPlanes[i].Poly.Add(OASPlanes[n - 1].Poly[k]);

				if (i == eOAS.XrPlane)
				{
					ResLine = IntersectPlanes(OASPlanes[eOAS.XrPlane].Plane, OASPlanes[eOAS.YrPlane].Plane, hWWs, hMax);
					RotateAndOffset(ArDir + Math.PI, ptLHPrj, ResLine);
					OASPlanes[i].Poly.Add(ResLine.End);
				}
				else
					OASPlanes[i].Poly.Add(OASPlanes[n - 1].Poly[j]);
			}

			ResLine = null;

			//=================================================================

		}

		#region Planes calculation & creation

		public static void CalcOASPlanes(eFlightCategory flightCat, double LLZTHRDist, double GPAvalue, double MAGvalue, double rdhValue, double Ss, double St, ref D3DPolygone[] OASPlanes)
		{
			D3Point PointC, PointD, PointE, PointD2;
			MAGvalue *= 100.0;

			double RDHCorr = rdhValue - 15.0;	// GlobalVars.constants.Pansops[Panda.Constants.ePANSOPSData.arAbv_Treshold].Value;

			double CwCorr = St - 6.0 - RDHCorr;
			double Cw_Corr = St - 6.0 - RDHCorr;
			double TanGPA = Math.Tan(GPAvalue);
			double GPAinDeg = RadToDeg(GPAvalue);

			OASPlanes[eOAS.ZeroPlane].Plane.A = 0.0;
			OASPlanes[eOAS.ZeroPlane].Plane.B = 0.0;
			OASPlanes[eOAS.ZeroPlane].Plane.C = -1.0;
			OASPlanes[eOAS.ZeroPlane].Plane.D = 0.0;

			OASPlanes[eOAS.CommonPlane].Plane.A = 0.0;
			OASPlanes[eOAS.CommonPlane].Plane.B = 0.0;
			OASPlanes[eOAS.CommonPlane].Plane.C = -1.0;
			OASPlanes[eOAS.CommonPlane].Plane.D = 300.0;

			PointC = Common.LagrangeI(GPAinDeg, Common.GPNodes, Common.CSNodesI);

			OASPlanes[eOAS.WPlane].Plane.A = Common.LagrangeI(GPAinDeg, Common.GPNodes, Common.WANodes);
			OASPlanes[eOAS.WPlane].Plane.B = 0.0;
			OASPlanes[eOAS.WPlane].Plane.C = -1.0;
			OASPlanes[eOAS.WPlane].Plane.D = -PointC.X * OASPlanes[eOAS.WPlane].Plane.A;

			PointD = Common.LagrangeI(GPAinDeg, Common.GPNodes, Common.DSNodesI);

			OASPlanes[eOAS.XrPlane].Plane.B = Common.LagrangeII(GPAinDeg, LLZTHRDist, Common.XBNodesI, Common.GPNodes, Common.LocTHRDists);
			OASPlanes[eOAS.XrPlane].Plane.A = -OASPlanes[eOAS.XrPlane].Plane.B * (PointD.Y - PointC.Y) / (PointD.X - PointC.X);
			OASPlanes[eOAS.XrPlane].Plane.C = -1.0;
			OASPlanes[eOAS.XrPlane].Plane.D = OASPlanes[eOAS.XrPlane].Plane.B * (PointD.Y * PointC.X - PointC.Y * PointD.X) / (PointD.X - PointC.X);

			//_PointC2.Z = Common.catOASTermHeight;
			//_PointC2.X = (_PointC2.Z - _PlaneW.C) / _PlaneW.A;
			//_PointC2.Y = (_PointC2.Z - _PlaneX.A * _PointC2.X - _PlaneX.C) / _PlaneX.B;

			PointD2.Z = Common.catOASTermHeight;
			PointD2.X = (PointD2.Z - Common.baseRDHHeight) / TanGPA;
			PointD2.Y = (PointD2.Z - OASPlanes[eOAS.XrPlane].Plane.A * PointD2.X - OASPlanes[eOAS.XrPlane].Plane.D) / OASPlanes[eOAS.XrPlane].Plane.B;

			PointE = Common.LagrangeII(GPAinDeg, MAGvalue, Common.ESNodesI, Common.GPNodes, Common.MisAprGr);
			OASPlanes[eOAS.YrPlane].Plane = Common.PlaneFrom3Pt(PointD, PointE, PointD2);

			OASPlanes[eOAS.ZPlane].Plane.A = -0.01 * MAGvalue;
			OASPlanes[eOAS.ZPlane].Plane.B = 0.0;
			OASPlanes[eOAS.ZPlane].Plane.C = -1.0;
			OASPlanes[eOAS.ZPlane].Plane.D = -OASPlanes[eOAS.ZPlane].Plane.A * PointE.X;

			/// Region "W*"
			//_PointE2 = Common.Calc2Equation(_PlaneY, _PlaneZ, Common.catOASTermHeight);

			if (flightCat >= eFlightCategory.APVI)
			{
				double dd = 50.0;
				if (flightCat == eFlightCategory.APVII)
					dd = 20.0;

				OASPlanes[eOAS.WsPlane].Plane.A = Math.Tan(0.75 * GPAvalue);
				OASPlanes[eOAS.WsPlane].Plane.B = 0.0;
				OASPlanes[eOAS.WsPlane].Plane.C = -1.0;
				OASPlanes[eOAS.WsPlane].Plane.D = OASPlanes[eOAS.WsPlane].Plane.A * rdhValue / TanGPA - dd;
			}

			double APVCorr = 38.0;
			if (flightCat == eFlightCategory.APVII)
				APVCorr = 8.0;

			PointE.X = -900.0 - APVCorr / TanGPA;
			OASPlanes[eOAS.ZPlane].Plane.D = -OASPlanes[eOAS.ZPlane].Plane.A * PointE.X;

			double dRDH = rdhValue - Common.baseRDHHeight;
			OASPlanes[eOAS.WPlane].Plane.D += dRDH;
			OASPlanes[eOAS.WsPlane].Plane.D += dRDH;
			OASPlanes[eOAS.XrPlane].Plane.D += dRDH;
			OASPlanes[eOAS.YrPlane].Plane.D += dRDH;

			OASPlanes[eOAS.XrPlane].Plane.D -= APVCorr;
			OASPlanes[eOAS.YrPlane].Plane.D -= APVCorr;

			//==============================================================================//

			OASPlanes[eOAS.YlPlane].Plane.A = OASPlanes[eOAS.YrPlane].Plane.A;
			OASPlanes[eOAS.YlPlane].Plane.B = -OASPlanes[eOAS.YrPlane].Plane.B;
			OASPlanes[eOAS.YlPlane].Plane.C = -1.0;
			OASPlanes[eOAS.YlPlane].Plane.D = OASPlanes[eOAS.YrPlane].Plane.D;

			OASPlanes[eOAS.XlPlane].Plane.A = OASPlanes[eOAS.XrPlane].Plane.A;
			OASPlanes[eOAS.XlPlane].Plane.B = -OASPlanes[eOAS.XrPlane].Plane.B;
			OASPlanes[eOAS.XlPlane].Plane.C = -1.0;
			OASPlanes[eOAS.XlPlane].Plane.D = OASPlanes[eOAS.XrPlane].Plane.D;
		}

		public static void CreateOASPlanes(Point ptLHPrj, double ArDir, double hMax, ref D3DPolygone[] OASPlanes)
		{
			Line ResLine;

			int i, n = OASPlanes.Length;

			for (i = 0; i < n; i++)
				OASPlanes[i].Poly = new Polygon();

			D3Point pt3Tmp;
			D3Line xyLine = IntersectPlanes(OASPlanes[eOAS.YlPlane].Plane, OASPlanes[eOAS.XlPlane].Plane);
			pt3Tmp.Y = -1759.4;
			pt3Tmp.X = -(pt3Tmp.Y * xyLine.B + xyLine.C) / xyLine.A;
			double hXY = OASPlanes[eOAS.XlPlane].Plane.A * pt3Tmp.X + OASPlanes[eOAS.XlPlane].Plane.B * pt3Tmp.Y + OASPlanes[eOAS.XlPlane].Plane.D;

			D3Line yzLine = IntersectPlanes(OASPlanes[eOAS.YlPlane].Plane, OASPlanes[eOAS.ZPlane].Plane);
			pt3Tmp.Y = -1759.4;
			pt3Tmp.X = -(pt3Tmp.Y * yzLine.B + yzLine.C) / yzLine.A;
			double hYZ = OASPlanes[eOAS.ZPlane].Plane.A * pt3Tmp.X + OASPlanes[eOAS.ZPlane].Plane.D;

			for (i = eOAS.XlPlane; i < eOAS.XrPlane; i++)
			{
				if (i == eOAS.ZPlane || i + 1 == eOAS.ZPlane)
					ResLine = IntersectPlanes(OASPlanes[i].Plane, OASPlanes[i + 1].Plane, 0.0, hYZ);
				else
					ResLine = IntersectPlanes(OASPlanes[i].Plane, OASPlanes[i + 1].Plane, 0.0, hXY);

				RotateAndOffset(ArDir + Math.PI, ptLHPrj, ResLine);
				OASPlanes[0].Poly.Add(ResLine.Start);
				OASPlanes[n - 1].Poly.Add(ResLine.End);
			}

			D3Line WxWs = IntersectPlanes(OASPlanes[eOAS.WPlane].Plane, OASPlanes[eOAS.WsPlane].Plane);
			double xWWs = -WxWs.C / WxWs.A;
			double hWWs = OASPlanes[eOAS.WPlane].Plane.A * xWWs + OASPlanes[eOAS.WPlane].Plane.D;
			bool haveOmegaS = hWWs > 0.0;

			if (haveOmegaS)
			{
				ResLine = IntersectPlanes(OASPlanes[eOAS.XrPlane].Plane, OASPlanes[eOAS.WsPlane].Plane, 0.0, hWWs);
				RotateAndOffset(ArDir + Math.PI, ptLHPrj, ResLine);

				OASPlanes[0].Poly.Add(ResLine.Start);
				OASPlanes[eOAS.WsPlane].Poly.Add(ResLine.Start);
				OASPlanes[eOAS.WsPlane].Poly.Add(ResLine.End);

				ResLine = IntersectPlanes(OASPlanes[eOAS.XlPlane].Plane, OASPlanes[eOAS.WsPlane].Plane, 0.0, hWWs);
				RotateAndOffset(ArDir + Math.PI, ptLHPrj, ResLine);

				OASPlanes[0].Poly.Insert(0, ResLine.Start);

				OASPlanes[eOAS.WsPlane].Poly.Add(ResLine.End);
				OASPlanes[eOAS.WsPlane].Poly.Add(ResLine.Start);
			}
			else
				hWWs = 0.0;

			ResLine = IntersectPlanes(OASPlanes[eOAS.XrPlane].Plane, OASPlanes[eOAS.WPlane].Plane, hWWs, hMax);
			RotateAndOffset(ArDir + Math.PI, ptLHPrj, ResLine);

			OASPlanes[eOAS.WPlane].Poly.Add(ResLine.Start);
			OASPlanes[eOAS.WPlane].Poly.Add(ResLine.End);
			if (!haveOmegaS)
				OASPlanes[0].Poly.Add(ResLine.Start);
			OASPlanes[n - 1].Poly.Add(ResLine.End);

			ResLine = IntersectPlanes(OASPlanes[eOAS.XlPlane].Plane, OASPlanes[eOAS.WPlane].Plane, hWWs, hMax);
			RotateAndOffset(ArDir + Math.PI, ptLHPrj, ResLine);

			OASPlanes[eOAS.WPlane].Poly.Add(ResLine.End);
			OASPlanes[eOAS.WPlane].Poly.Add(ResLine.Start);
			if (!haveOmegaS)
				OASPlanes[0].Poly.Insert(0, ResLine.Start);

			OASPlanes[n - 1].Poly.Insert(0, ResLine.End);

			int j, k, m = OASPlanes[0].Poly.Count;
			for (j = 0, i = eOAS.XlPlane; i < eOAS.WsPlane; i++, j++)
			{
				k = (j + 1) % m;

				if (haveOmegaS && i == eOAS.XlPlane)
					OASPlanes[i].Poly.Add(OASPlanes[eOAS.WsPlane].Poly[2]);

				OASPlanes[i].Poly.Add(OASPlanes[0].Poly[j]);
				OASPlanes[i].Poly.Add(OASPlanes[0].Poly[k]);

				if (haveOmegaS && i == eOAS.XrPlane)
					OASPlanes[i].Poly.Add(OASPlanes[eOAS.WsPlane].Poly[1]);

				if (i == eOAS.XlPlane)
				{
					ResLine = IntersectPlanes(OASPlanes[eOAS.XlPlane].Plane, OASPlanes[eOAS.YlPlane].Plane, hWWs, hMax);
					RotateAndOffset(ArDir + Math.PI, ptLHPrj, ResLine);
					OASPlanes[i].Poly.Add(ResLine.End);
				}
				else
					OASPlanes[i].Poly.Add(OASPlanes[n - 1].Poly[k]);

				if (i == eOAS.XrPlane)
				{
					ResLine = IntersectPlanes(OASPlanes[eOAS.XrPlane].Plane, OASPlanes[eOAS.YrPlane].Plane, hWWs, hMax);
					RotateAndOffset(ArDir + Math.PI, ptLHPrj, ResLine);
					OASPlanes[i].Poly.Add(ResLine.End);
				}
				else
					OASPlanes[i].Poly.Add(OASPlanes[n - 1].Poly[j]);
			}

			ResLine = null;

			//=================================================================

			//for (i = 0; i < n; i++)
			//{
			//    if (OASPlanes[i].Count == 0)
			//        continue;

			//    Polygon pPoly = new Polygon();
			//    pPoly.ExteriorRing = pWorkRings[i];
			//    OASPlanes[i].Poly.Add(pPoly);
			//}
		}

		private static void RotateAndOffset(double dir, Point ptLHPrj, Line pPolyline)
		{
			Point ptOrigin = new Point(0, 0);

			Point ptTmpStart = LocalToPrj(ptOrigin, dir, pPolyline.Start);
			Point ptTmpEnd = LocalToPrj(ptOrigin, dir, pPolyline.End);

			ptTmpStart.X += ptLHPrj.X;
			ptTmpStart.Y += ptLHPrj.Y;

			ptTmpEnd.X += ptLHPrj.X;
			ptTmpEnd.Y += ptLHPrj.Y;

			pPolyline.Start = ptTmpStart;
			pPolyline.End = ptTmpEnd;
		}

		#endregion
	}
}
