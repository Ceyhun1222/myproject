using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Geometries;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;

namespace Aran.Panda.RNAV.RNPAR.Utils
{
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
		public double X, Y, Z;
		public double A;
		public double B;
		public double C;
		public double D;
		public Point pPt;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct D3DPolygone
	{
		public Aran.Geometries.MultiPolygon Poly;
		public D3Plane Plane;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public static class Common
	{
		public const double catOASTermHeight = 300.0;
		public const double baseRDHHeight = 15.0;

		public static double[] GPNodes = new double[3] { 2.5, 3.0, 3.5 };
		public static double[] LocTHRDists = new double[4] { 2000.0, 3000.0, 3800.0, 4500.0 };
		public static double[] MisAprGr = new double[5] { 2.0, 2.5, 3.0, 4.0, 5.0 };
		public static double[] WANodes = new double[3] { 0.0239, 0.0285, 0.0331 };

		#region category I, APV-I, APV-II
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

		public static D3Line IntersectPlanes(D3Plane p1, D3Plane p2)
		{
			D3Line result;

			result.A = p2.A - p1.A;
			result.B = p2.B - p1.B;
			result.C = p2.D - p1.D;

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

		public static LineSegment IntersectPlanes(D3Plane pl0, D3Plane pl1, double hMin, double hMax)
		{
			double d = pl0.A * pl1.B - pl0.B * pl1.A;

			if (d == 0.0)
				return new LineSegment();
			d = 1.0 / d;

			double x = Det2(-(pl0.C * hMin + pl0.D), pl0.B, -(pl1.C * hMin + pl1.D), pl1.B) * d;
			double y = Det2(pl0.A, -(pl0.C * hMin + pl0.D), pl1.A, -(pl1.C * hMin + pl1.D)) * d;
			Point pt0 = new Point(x, y, hMin);

			x = Det2(-(pl0.C * hMax + pl0.D), pl0.B, -(pl1.C * hMax + pl1.D), pl1.B) * d;
			y = Det2(pl0.A, -(pl0.C * hMax + pl0.D), pl1.A, -(pl1.C * hMax + pl1.D)) * d;
			Point pt1 = new Point(x, y, hMax);

			LineSegment result = new LineSegment(pt0, pt1);
			return result;
		}

		public static D3Plane PlaneFrom3Pt(D3Point pt0, D3Point pt1, D3Point pt2)
		{
			double dx0 = pt1.X - pt0.X;
			double dy0 = pt1.Y - pt0.Y;

			double dx1 = pt2.X - pt0.X;
			double dy1 = pt2.Y - pt0.Y;

			double d = dx1 * dy0 - dx0 * dy1;
			if (d == 0.0)
				return default(D3Plane);

			D3Plane result;

			double dz0 = pt1.Z - pt0.Z;
			double dz1 = pt2.Z - pt0.Z;

			double dInv = 1.0 / d;

			result.A = dInv * (dy0 * dz1 - dy1 * dz0);
			result.B = dInv * (dz0 * dx1 - dz1 * dx0);
			result.C = -1.0;
			result.D = pt0.Z - result.A * pt0.X - result.B * pt0.Y;

			result.X = result.Y = result.Z = 0.0;
			result.pPt = null;

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

		public static void RotateAndOffset(double dir, Point ptOffset, Point pt0)
		{
			Point ptOrigin = new Point(0, 0);
			Point result = ARANFunctions.LocalToPrj(ptOrigin, dir, pt0);
			pt0.X = result.X;
			pt0.Y = result.Y; 

			pt0.X += ptOffset.X;
			pt0.Y += ptOffset.Y;
		}

		public static void RotateAndOffset(double dir, Point ptOffset, LineSegment pPolyline)
		{
			Point ptOrigin = new Point(0, 0);

			Point ptTmpStart = ARANFunctions.LocalToPrj(ptOrigin, dir, pPolyline.Start);
			Point ptTmpEnd = ARANFunctions.LocalToPrj(ptOrigin, dir, pPolyline.End);

			ptTmpStart.X += ptOffset.X;
			ptTmpStart.Y += ptOffset.Y;

			ptTmpEnd.X += ptOffset.X;
			ptTmpEnd.Y += ptOffset.Y;

			pPolyline.Start = ptTmpStart;
			pPolyline.End = ptTmpEnd;
		}

	}
}
