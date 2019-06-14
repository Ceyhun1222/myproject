using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Geometries;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;

namespace Aran.PANDA.RNAV.SGBAS
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

		public static void CalcOASPlanes(eSBASCat flightCat, double LLZTHRDist, double GPAvalue, double MAGvalue, double rdhValue, double Ss, double St, ref D3DPolygone[] OASPlanes)
		{
			D3Point PointC, PointD, PointE/*, PointC2*/, PointD2/*, PointE2*/;
			MAGvalue *= 100.0;

			double RDHCorr = rdhValue - GlobalVars.constants.Pansops[PANDA.Constants.ePANSOPSData.arAbv_Treshold].Value;

			double CwCorr = St - 6.0 - RDHCorr;
			double Cw_Corr = St - 6.0 - RDHCorr;
			double TanGPA = Math.Tan(GPAvalue);
			double GPAinDeg = ARANMath.RadToDeg(GPAvalue);

			OASPlanes[(int)eOAS.ZeroPlane].Plane.A = 0.0;
			OASPlanes[(int)eOAS.ZeroPlane].Plane.B = 0.0;
			OASPlanes[(int)eOAS.ZeroPlane].Plane.C = -1.0;
			OASPlanes[(int)eOAS.ZeroPlane].Plane.D = 0.0;

			OASPlanes[(int)eOAS.CommonPlane].Plane.A = 0.0;
			OASPlanes[(int)eOAS.CommonPlane].Plane.B = 0.0;
			OASPlanes[(int)eOAS.CommonPlane].Plane.C = -1.0;
			OASPlanes[(int)eOAS.CommonPlane].Plane.D = 300.0;

			PointC = Common.LagrangeI(GPAinDeg, Common.GPNodes, Common.CSNodesI);

			OASPlanes[(int)eOAS.WPlane].Plane.A = Common.LagrangeI(GPAinDeg, Common.GPNodes, Common.WANodes);
			OASPlanes[(int)eOAS.WPlane].Plane.B = 0.0;
			OASPlanes[(int)eOAS.WPlane].Plane.C = -1.0;
			OASPlanes[(int)eOAS.WPlane].Plane.D = -PointC.X * OASPlanes[(int)eOAS.WPlane].Plane.A;

			PointD = Common.LagrangeI(GPAinDeg, Common.GPNodes, Common.DSNodesI);

			OASPlanes[(int)eOAS.XrPlane].Plane.B = Common.LagrangeII(GPAinDeg, LLZTHRDist, Common.XBNodesI, Common.GPNodes, Common.LocTHRDists);
			OASPlanes[(int)eOAS.XrPlane].Plane.A = -OASPlanes[(int)eOAS.XrPlane].Plane.B * (PointD.Y - PointC.Y) / (PointD.X - PointC.X);
			OASPlanes[(int)eOAS.XrPlane].Plane.C = -1.0;
			OASPlanes[(int)eOAS.XrPlane].Plane.D = OASPlanes[(int)eOAS.XrPlane].Plane.B * (PointD.Y * PointC.X - PointC.Y * PointD.X) / (PointD.X - PointC.X);

			//_PointC2.Z = Common.catOASTermHeight;
			//_PointC2.X = (_PointC2.Z - _PlaneW.C) / _PlaneW.A;
			//_PointC2.Y = (_PointC2.Z - _PlaneX.A * _PointC2.X - _PlaneX.C) / _PlaneX.B;

			PointD2.Z = Common.catOASTermHeight;
			PointD2.X = (PointD2.Z - Common.baseRDHHeight) / TanGPA;
			PointD2.Y = (PointD2.Z - OASPlanes[(int)eOAS.XrPlane].Plane.A * PointD2.X - OASPlanes[(int)eOAS.XrPlane].Plane.D) / OASPlanes[(int)eOAS.XrPlane].Plane.B;

			PointE = Common.LagrangeII(GPAinDeg, MAGvalue, Common.ESNodesI, Common.GPNodes, Common.MisAprGr);
			OASPlanes[(int)eOAS.YrPlane].Plane = Common.PlaneFrom3Pt(PointD, PointE, PointD2);

			OASPlanes[(int)eOAS.ZPlane].Plane.A = -0.01 * MAGvalue;
			OASPlanes[(int)eOAS.ZPlane].Plane.B = 0.0;
			OASPlanes[(int)eOAS.ZPlane].Plane.C = -1.0;
			OASPlanes[(int)eOAS.ZPlane].Plane.D = -OASPlanes[(int)eOAS.ZPlane].Plane.A * PointE.X;

			/// Region "W*"
			//_PointE2 = Common.Calc2Equation(_PlaneY, _PlaneZ, Common.catOASTermHeight);

			if (flightCat >= eSBASCat.APVI)
			{
				double dd = 50.0;
				if (flightCat == eSBASCat.APVII)
					dd = 20.0;

				OASPlanes[(int)eOAS.WsPlane].Plane.A = Math.Tan(0.75 * GPAvalue);
				OASPlanes[(int)eOAS.WsPlane].Plane.B = 0.0;
				OASPlanes[(int)eOAS.WsPlane].Plane.C = -1.0;
				OASPlanes[(int)eOAS.WsPlane].Plane.D = OASPlanes[(int)eOAS.WsPlane].Plane.A * rdhValue / TanGPA - dd;
			}

			double APVCorr = 38.0;

			if (flightCat == eSBASCat.CategoryI)
				APVCorr = 0.0;
			else if (flightCat == eSBASCat.APVII)
				APVCorr = 8.0;

			PointE.X = -900.0 - APVCorr / TanGPA;
			OASPlanes[(int)eOAS.ZPlane].Plane.D = -OASPlanes[(int)eOAS.ZPlane].Plane.A * PointE.X;

			double dRDH = rdhValue - Common.baseRDHHeight;
			OASPlanes[(int)eOAS.WPlane].Plane.D += dRDH;
			OASPlanes[(int)eOAS.WsPlane].Plane.D += dRDH;
			OASPlanes[(int)eOAS.XrPlane].Plane.D += dRDH;
			OASPlanes[(int)eOAS.YrPlane].Plane.D += dRDH;

			OASPlanes[(int)eOAS.XrPlane].Plane.D -= APVCorr;
			OASPlanes[(int)eOAS.YrPlane].Plane.D -= APVCorr;

			//==============================================================================//

			OASPlanes[(int)eOAS.YlPlane].Plane.A = OASPlanes[(int)eOAS.YrPlane].Plane.A;
			OASPlanes[(int)eOAS.YlPlane].Plane.B = -OASPlanes[(int)eOAS.YrPlane].Plane.B;
			OASPlanes[(int)eOAS.YlPlane].Plane.C = -1.0;
			OASPlanes[(int)eOAS.YlPlane].Plane.D = OASPlanes[(int)eOAS.YrPlane].Plane.D;

			OASPlanes[(int)eOAS.XlPlane].Plane.A = OASPlanes[(int)eOAS.XrPlane].Plane.A;
			OASPlanes[(int)eOAS.XlPlane].Plane.B = -OASPlanes[(int)eOAS.XrPlane].Plane.B;
			OASPlanes[(int)eOAS.XlPlane].Plane.C = -1.0;
			OASPlanes[(int)eOAS.XlPlane].Plane.D = OASPlanes[(int)eOAS.XrPlane].Plane.D;
		}

		public static void CreateOASPlanes(Aran.Geometries.Point ptLHPrj, double ArDir, double hMax, ref D3DPolygone[] OASPlanes)
		{
			Aran.Geometries.LineSegment ResLine;

			int i, n = OASPlanes.Length;
			Ring[] pWorkRings = new Ring[n];

			for (i = 0; i < n; i++)
			{
				OASPlanes[i].Poly = new MultiPolygon();
				pWorkRings[i] = new Ring();
			}

			D3Point pt3Tmp;

			D3Line yzLine = IntersectPlanes(OASPlanes[(int)eOAS.YlPlane].Plane, OASPlanes[(int)eOAS.ZPlane].Plane);
			pt3Tmp.Y = -GlobalVars.SBASWidth;
			pt3Tmp.X = -(pt3Tmp.Y * yzLine.B + yzLine.C) / yzLine.A;
			double hYZ  = OASPlanes[(int)eOAS.ZPlane].Plane.A * pt3Tmp.X + OASPlanes[(int)eOAS.ZPlane].Plane.D;

			pt3Tmp = IntersectPlanes(OASPlanes[(int)eOAS.YrPlane].Plane, OASPlanes[(int)eOAS.XrPlane].Plane, hMax - GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value);
			bool yLtSBASWidth = pt3Tmp.Y < GlobalVars.SBASWidth;

			if (yLtSBASWidth)
			{
				pt3Tmp.Z = hMax - GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value;
				pt3Tmp.Y = -GlobalVars.SBASWidth;
				pt3Tmp.X = (pt3Tmp.Z - OASPlanes[(int)eOAS.XlPlane].Plane.B * pt3Tmp.Y - OASPlanes[(int)eOAS.XlPlane].Plane.D) / OASPlanes[(int)eOAS.XlPlane].Plane.A;

				double hYY = OASPlanes[(int)eOAS.YlPlane].Plane.A * pt3Tmp.X + OASPlanes[(int)eOAS.YlPlane].Plane.B * pt3Tmp.Y + OASPlanes[(int)eOAS.YlPlane].Plane.D;

				for (i = (int)eOAS.XlPlane; i < (int)eOAS.XrPlane; i++)
				{
					if ((eOAS)i == eOAS.ZPlane || (eOAS)(i + 1) == eOAS.ZPlane)
						ResLine = IntersectPlanes(OASPlanes[i].Plane, OASPlanes[i + 1].Plane, 0.0, hYZ);
					else
						ResLine = IntersectPlanes(OASPlanes[i].Plane, OASPlanes[i + 1].Plane, 0.0, pt3Tmp.Z);

					RotateAndOffset(ArDir + Math.PI, ptLHPrj, ResLine);

					pWorkRings[0].Add(ResLine.Start);

					if ((eOAS)i == eOAS.XlPlane )
					{
						Point ptEnd = new Point(pt3Tmp.X, pt3Tmp.Y, hYY);
						RotateAndOffset(ArDir + Math.PI, ptLHPrj, ptEnd);

						pWorkRings[n - 1].Add(ResLine.End);
						pWorkRings[n - 1].Add(ptEnd);
						pt3Tmp.Y = -pt3Tmp.Y;
					}
					else if ((eOAS)i == eOAS.YrPlane)
					{
						Point ptEnd = new Point(pt3Tmp.X, pt3Tmp.Y, hYY);
						RotateAndOffset(ArDir + Math.PI, ptLHPrj, ptEnd);

						pWorkRings[n - 1].Add(ptEnd);
						pWorkRings[n - 1].Add(ResLine.End);

						pt3Tmp.Y = -pt3Tmp.Y;
					}
					else
						pWorkRings[n - 1].Add(ResLine.End);
				}
			}
			else
			{
				D3Line xyLine = IntersectPlanes(OASPlanes[(int)eOAS.YlPlane].Plane, OASPlanes[(int)eOAS.XlPlane].Plane);
				pt3Tmp.Y = -GlobalVars.SBASWidth;
				pt3Tmp.X = -(pt3Tmp.Y * xyLine.B + xyLine.C) / xyLine.A;
				double hXY = OASPlanes[(int)eOAS.XlPlane].Plane.A * pt3Tmp.X + OASPlanes[(int)eOAS.XlPlane].Plane.B * pt3Tmp.Y + OASPlanes[(int)eOAS.XlPlane].Plane.D;

				for (i = (int)eOAS.XlPlane; i < (int)eOAS.XrPlane; i++)
				{
					if ((eOAS)i == eOAS.ZPlane || (eOAS)(i + 1) == eOAS.ZPlane)
						ResLine = IntersectPlanes(OASPlanes[i].Plane, OASPlanes[i + 1].Plane, 0.0, hYZ);
					else
						ResLine = IntersectPlanes(OASPlanes[i].Plane, OASPlanes[i + 1].Plane, 0.0, hXY);

					RotateAndOffset(ArDir + Math.PI, ptLHPrj, ResLine);

					pWorkRings[0].Add(ResLine.Start);
					pWorkRings[n - 1].Add(ResLine.End);
				}
			}

			D3Line WxWs = IntersectPlanes(OASPlanes[(int)eOAS.WPlane].Plane, OASPlanes[(int)eOAS.WsPlane].Plane);
			double xWWs = -WxWs.C / WxWs.A;
			double hWWs = OASPlanes[(int)eOAS.WPlane].Plane.A * xWWs + OASPlanes[(int)eOAS.WPlane].Plane.D;
			bool haveOmegaS = hWWs > 0.0;

			if (haveOmegaS)
			{
				ResLine = IntersectPlanes(OASPlanes[(int)eOAS.XrPlane].Plane, OASPlanes[(int)eOAS.WsPlane].Plane, 0.0, hWWs);
				RotateAndOffset(ArDir + Math.PI, ptLHPrj, ResLine);

				pWorkRings[0].Add(ResLine.Start);
				pWorkRings[(int)eOAS.WsPlane].Add(ResLine.Start);
				pWorkRings[(int)eOAS.WsPlane].Add(ResLine.End);

				ResLine = IntersectPlanes(OASPlanes[(int)eOAS.XlPlane].Plane, OASPlanes[(int)eOAS.WsPlane].Plane, 0.0, hWWs);
				RotateAndOffset(ArDir + Math.PI, ptLHPrj, ResLine);

				pWorkRings[0].Insert(0, ResLine.Start);

				pWorkRings[(int)eOAS.WsPlane].Add(ResLine.End);
				pWorkRings[(int)eOAS.WsPlane].Add(ResLine.Start);
			}
			else
				hWWs = 0.0;

			ResLine = IntersectPlanes(OASPlanes[(int)eOAS.XrPlane].Plane, OASPlanes[(int)eOAS.WPlane].Plane, hWWs, hMax - GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value);
			RotateAndOffset(ArDir + Math.PI, ptLHPrj, ResLine);

			pWorkRings[(int)eOAS.WPlane].Add(ResLine.Start);
			pWorkRings[(int)eOAS.WPlane].Add(ResLine.End);
			if (!haveOmegaS)
				pWorkRings[0].Add(ResLine.Start);
			pWorkRings[n - 1].Add(ResLine.End);

			ResLine = IntersectPlanes(OASPlanes[(int)eOAS.XlPlane].Plane, OASPlanes[(int)eOAS.WPlane].Plane, hWWs, hMax - GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value);
			RotateAndOffset(ArDir + Math.PI, ptLHPrj, ResLine);

			pWorkRings[(int)eOAS.WPlane].Add(ResLine.End);
			pWorkRings[(int)eOAS.WPlane].Add(ResLine.Start);
			if (!haveOmegaS)
				pWorkRings[0].Insert(0, ResLine.Start);

			pWorkRings[n - 1].Insert(0, ResLine.End);

			int j, k, m = pWorkRings[0].Count;

			if (yLtSBASWidth)
			{
				int h = 0;

				for (j = 0, i = (int)eOAS.XlPlane; i < (int)eOAS.WsPlane; i++, j++)
				{
					k = (j + 1) % m;

					if (haveOmegaS && (eOAS)i == eOAS.XlPlane)
						pWorkRings[i].Add(pWorkRings[(int)eOAS.WsPlane][2]);

					pWorkRings[i].Add(pWorkRings[0][j]);
					pWorkRings[i].Add(pWorkRings[0][k]);


					if (haveOmegaS && (eOAS)i == eOAS.XrPlane)
						pWorkRings[i].Add(pWorkRings[(int)eOAS.WsPlane][1]);

					if ((eOAS)i == eOAS.YlPlane)
					{
						pWorkRings[i].Add(pWorkRings[n - 1][k + h + 1]);
						pWorkRings[i].Add(pWorkRings[n - 1][k + h]);
						pWorkRings[i].Add(pWorkRings[n - 1][j + h]);
						h++;
					}
					else if ((eOAS)i == eOAS.YrPlane)
					{
						pWorkRings[i].Add(pWorkRings[n - 1][k + h + 1]);
						pWorkRings[i].Add(pWorkRings[n - 1][k + h]);
						pWorkRings[i].Add(pWorkRings[n - 1][j + h]);
						h++;
					}
					else
					{
						pWorkRings[i].Add(pWorkRings[n - 1][k + h]);
						pWorkRings[i].Add(pWorkRings[n - 1][j + h]);
					}
				}
			}
			else
			{
				Point pt1 = null, pt6 = null;

				for (j = 0, i = (int)eOAS.XlPlane; i < (int)eOAS.WsPlane; i++, j++)
				{
					k = (j + 1) % m;

					if (haveOmegaS && (eOAS)i == eOAS.XlPlane)
						pWorkRings[i].Add(pWorkRings[(int)eOAS.WsPlane][2]);

					pWorkRings[i].Add(pWorkRings[0][j]);
					pWorkRings[i].Add(pWorkRings[0][k]);

					if (haveOmegaS && (eOAS)i == eOAS.XrPlane)
						pWorkRings[i].Add(pWorkRings[(int)eOAS.WsPlane][1]);

					if ((eOAS)i == eOAS.XlPlane)
					{
						ResLine = IntersectPlanes(OASPlanes[(int)eOAS.XlPlane].Plane, OASPlanes[(int)eOAS.YlPlane].Plane, hWWs, hMax - GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value);
						RotateAndOffset(ArDir + Math.PI, ptLHPrj, ResLine);

						pWorkRings[i].Add(ResLine.End);
						pt1 = ResLine.End;
					}
					else
						pWorkRings[i].Add(pWorkRings[n - 1][k]);

					if ((eOAS)i == eOAS.XrPlane)
					{
						ResLine = IntersectPlanes(OASPlanes[(int)eOAS.XrPlane].Plane, OASPlanes[(int)eOAS.YrPlane].Plane, hWWs, hMax - GlobalVars.constants.Pansops[ePANSOPSData.arISegmentMOC].Value);
						RotateAndOffset(ArDir + Math.PI, ptLHPrj, ResLine);

						pWorkRings[i].Add(ResLine.End);
						pt6 = ResLine.End;
					}
					else
						pWorkRings[i].Add(pWorkRings[n - 1][j]);
				}

				pWorkRings[n - 1].Insert(1, pt1);
				pWorkRings[n - 1].Insert(pWorkRings[n - 1].Count - 1, pt6);
			}
			//=================================================================

			ResLine = null;

			for (i = 0; i < n; i++)
			{
				if (pWorkRings[i].Count == 0)
					continue;

				Polygon pPoly = new Polygon();
				pPoly.ExteriorRing = pWorkRings[i];
				OASPlanes[i].Poly.Add(pPoly);
			}
		}

		public static void CreateOASPlanes(Aran.Geometries.Point ptLHPrj, double ArDir, double hMax, ref D3DPolygone[] OASPlanes, int ilsCat)
		{
			double hCons;

			if (ilsCat == 0)
				hCons = hMax;
			else if (ilsCat == 1)
				hCons = GlobalVars.arHOASPlaneCat1;
			else
				hCons = GlobalVars.arHOASPlaneCat23;

			int i, n = OASPlanes.Length;
			Ring[] pWorkRings = new Ring[n];

			for (i = 0; i < n; i++)
			{
				OASPlanes[i].Poly = new MultiPolygon();
				pWorkRings[i] = new Ring();
			}

			Aran.Geometries.LineSegment ResLine=null;

			for (i = (int)eOAS.XlPlane; i < (int)eOAS.XrPlane; i++)
			{
				ResLine = IntersectPlanes(OASPlanes[i].Plane, OASPlanes[i + 1].Plane, 0.0, hCons);
				RotateAndOffset(ArDir + Math.PI, ptLHPrj, ResLine);

				//LineString ls = new LineString();
				//ls.Add(ResLine.Start);
				//ls.Add(ResLine.End); 

				//GlobalVars.gAranGraphics.DrawLineString(ls, -1, 2);
				//System.Windows.Forms.Application.DoEvents();

				pWorkRings[0].Add(ResLine.Start);
				pWorkRings[n - 1].Add(ResLine.End);
			}

			int j, k, l;
			for (i = 0; i < 4; i++)
			{
				j = 1 + (i + 4) % 6;
				k = 1 + (i + 5) % 6;

				ResLine = IntersectPlanes(OASPlanes[j].Plane, OASPlanes[k].Plane, 0.0, hMax);
				RotateAndOffset(ArDir + Math.PI, ptLHPrj, ResLine);

				pWorkRings[0].Add(ResLine.Start);
				pWorkRings[n - 1].Add(ResLine.End);
			}

			int m = pWorkRings[0].Count;
			j = 6;

			for (i = 1; i < 7; i++)
			{
				k = j % m;
				l = (j + m - 1) % m;

				pWorkRings[i].Add(pWorkRings[0][k]);
				pWorkRings[i].Add(pWorkRings[0][l]);
				pWorkRings[i].Add(pWorkRings[n-1][l]);
				pWorkRings[i].Add(pWorkRings[n-1][k]);
				OASPlanes[i].Plane.pPt = pWorkRings[0][l];

				j++;
				if (j == 4) j = 5;
				if (j == 8) j = 1;
			}

			//=================================================================

			ResLine = null;

			for (i = 0; i < n; i++)
			{
				if (pWorkRings[i].Count == 0)
					continue;

				Polygon pPoly = new Polygon();
				pPoly.ExteriorRing = pWorkRings[i];
				OASPlanes[i].Poly.Add(pPoly);
			}
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
