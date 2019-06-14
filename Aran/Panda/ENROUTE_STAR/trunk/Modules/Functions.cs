using System;
//using System.Drawing;

using Microsoft.Win32;

//using ESRI.ArcGIS.Carto;
//using ESRI.ArcGIS.Display;
//using ESRI.ArcGIS.esriSystem;
//using ESRI.ArcGIS.Framework;
//using ESRI.ArcGIS.Geometry;
//using ESRI.ArcGIS.Catalog;

using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Geometries.SpatialReferences;
using Aran.Panda.Common;
using Aran.Panda.Constants;

namespace Aran.Panda.EnrouteStar
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public static class Shall_sort
	{
		public static void fSort(ObstacleType[] obsArray)
		{
			int Length = obsArray.Length;
			if (Length == 0)
				return;

			int GapSize = 0;
			do
				GapSize = GapSize * 3 + 1;
			while (GapSize <= Length);

			do
			{
				GapSize = GapSize / 3;
				for (int i = GapSize; i < Length; i++)
				{
					int CurPos = i;
					ObstacleType TempVal = obsArray[i];
					while (obsArray[CurPos - GapSize].fSort > TempVal.fSort)
					{
						obsArray[CurPos] = obsArray[CurPos - GapSize];
						CurPos = CurPos - GapSize;
						if (CurPos - GapSize < 0)
							break;
					}
					obsArray[CurPos] = TempVal;
				}
			}
			while (GapSize > 1);
		}

		public static void fSortD(ObstacleType[] obsArray)
		{
			int Length = obsArray.Length;
			if (Length == 0)
				return;

			int GapSize = 0;
			do
				GapSize = GapSize * 3 + 1;
			while (GapSize <= Length);

			do
			{
				GapSize = GapSize / 3;
				for (int i = GapSize; i < Length; i++)
				{
					int CurPos = i;
					ObstacleType TempVal = obsArray[i];
					while (obsArray[CurPos - GapSize].fSort < TempVal.fSort)
					{
						obsArray[CurPos] = obsArray[CurPos - GapSize];
						CurPos = CurPos - GapSize;
						if (CurPos - GapSize < 0)
							break;
					}
					obsArray[CurPos] = TempVal;
				}
			}
			while (GapSize > 1);
		}

		public static void sSort(ObstacleType[] obsArray)
		{
			int Length = obsArray.Length;
			if (Length == 0)
				return;

			int GapSize = 0;

			do
				GapSize = GapSize * 3 + 1;
			while (GapSize <= Length);

			do
			{
				GapSize = GapSize / 3;
				for (int i = GapSize; i < Length; i++)
				{
					int CurPos = i;
					ObstacleType TempVal = obsArray[i];
					while (String.Compare(obsArray[CurPos - GapSize].sSort, TempVal.sSort) > 0)
					{
						obsArray[CurPos] = obsArray[CurPos - GapSize];
						CurPos = CurPos - GapSize;
						if (CurPos - GapSize < 0)
							break;
					}
					obsArray[CurPos] = TempVal;
				}
			}
			while (GapSize > 1);
		}

		public static void sSortD(ObstacleType[] obsArray)
		{
			int Length = obsArray.Length;
			if (Length == 0)
				return;

			int GapSize = 0;

			do
				GapSize = GapSize * 3 + 1;
			while (GapSize <= Length);

			do
			{
				GapSize = GapSize / 3;
				for (int i = GapSize; i < Length; i++)
				{
					int CurPos = i;
					ObstacleType TempVal = obsArray[i];

					while (String.Compare(obsArray[CurPos - GapSize].sSort, TempVal.sSort) < 0)
					{
						obsArray[CurPos] = obsArray[CurPos - GapSize];
						CurPos = CurPos - GapSize;
						if (CurPos - GapSize < 0)
							break;
					}
					obsArray[CurPos] = TempVal;
				}
			}
			while (GapSize > 1);
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public static class Functions
	{

		public static void SetFormParented(IntPtr hWnd)
		{
			IntPtr hWndParent = GlobalVars.gAranEnv.Win32Window.Handle;
			Int32 hWndP = NativeMethods.GetWindowLong(hWnd, NativeMethods.GWL_HWNDPARENT);
			NativeMethods.SetWindowLong(hWnd, NativeMethods.GWL_HWNDPARENT, hWndParent);	//GetApplicationHWnd()
		}

		private static void QuickSort(ObstacleType[] A, int iLo, int iHi)
		{
			int Lo = iLo;
			int Hi = iHi;
			double Mid_Renamed = A[(Lo + Hi) / 2].X;

			do
			{
				while (A[Lo].X < Mid_Renamed)
					Lo++;

				while (A[Hi].X > Mid_Renamed)
					Hi--;

				if (Lo <= Hi)
				{
					ObstacleType t = A[Lo];
					A[Lo] = A[Hi];
					A[Hi] = t;
					Lo++;
					Hi--;
				}
			}
			while (!(Lo > Hi));

			if (Hi > iLo)
				QuickSort(A, iLo, Hi);

			if ((Lo < iHi))
				QuickSort(A, Lo, iHi);
		}

		public static void Sort(ObstacleType[] A)
		{
			int Lo = A.GetLowerBound(0);
			int Hi = A.GetUpperBound(0);

			if (Lo == Hi)
				return;

			QuickSort(A, Lo, Hi);
		}

		public static int SideDef(Point pointOnLine, double LineAngle, Point testPoint)
		{
			double fdY = testPoint.Y - pointOnLine.Y;
			double fdX = testPoint.X - pointOnLine.X;
			double fDist = fdY * fdY + fdX * fdX;

			if (fDist < ARANMath.Epsilon_2Distance)
				return 0;

			double Angle12 = ARANMath.RadToDegValue * System.Math.Atan2(testPoint.Y - pointOnLine.Y, testPoint.X - pointOnLine.X);
			double dAngle = NativeMethods.Modulus(LineAngle - Angle12, 360.0);

			if (dAngle == 0.0 || dAngle == 180.0)
				return 0;

			if (dAngle < 180.0)
				return 1;

			return -1;
		}

		public static int SideFrom2Angle(double Angle0, double Angle1)
		{
			double dAngle = SubtractAngles(Angle0, Angle1);

			if (180.0 - dAngle < GlobalVars.degEps || dAngle < GlobalVars.degEps)
				return 0;

			dAngle = NativeMethods.Modulus(Angle1 - Angle0, 360.0);

			if (dAngle < 180.0)
				return 1;

			return -1;
		}

		public static double SpiralTouchAngle(double r0, double coef0, double aztNominal, double aztTouch, int TurnDir)
		{
			double TouchAngle = NativeMethods.Modulus((aztTouch - aztNominal) * TurnDir, 360.0);
			TouchAngle = ARANMath.DegToRadValue * TouchAngle;

			double turnAngle = TouchAngle;
			double coef = ARANMath.RadToDegValue * coef0;

			for (int i = 0; i < 10; i++)
			{
				double d = coef / (r0 + coef * turnAngle);
				double delta = (turnAngle - TouchAngle - System.Math.Atan(d)) / (2.0 - 1.0 / (d * d + 1.0));
				turnAngle = turnAngle - delta;
				if ((System.Math.Abs(delta) < GlobalVars.radEps))
					break;
			}

			double spiralTouchAngleReturn = ARANMath.RadToDegValue * turnAngle;
			if (spiralTouchAngleReturn < 0.0)
				spiralTouchAngleReturn = NativeMethods.Modulus(spiralTouchAngleReturn, 360.0);

			return spiralTouchAngleReturn;
		}

		public static void CreateWindSpiral(Point pPtPrj, double aztNom, double AztSt, ref double AztEnd,
											double r0, double coef, int TurnDir, ref MultiPoint pPointCollection)
		{
			if (Functions.SubtractAngles(aztNom, AztEnd) < GlobalVars.degEps)
				AztEnd = aztNom;

			double dphi0 = (AztSt - aztNom) * TurnDir;
			dphi0 = NativeMethods.Modulus(dphi0, 360.0);

			if (dphi0 < 0.001)
				dphi0 = 0.0;
			else
				dphi0 = SpiralTouchAngle(r0, coef, aztNom, AztSt, TurnDir);

			double dphi = SpiralTouchAngle(r0, coef, aztNom, AztEnd, TurnDir);

			double TurnAng = dphi - dphi0;

			if (TurnAng < 0.0)
				return;

			double azt0 = aztNom + (dphi0 - 90.0) * TurnDir;
			azt0 = NativeMethods.Modulus(azt0, 360.0);

			double dAlpha = 1.0;
			int n = System.Convert.ToInt32(TurnAng / dAlpha);

			if (n < 1) n = 1;
			else if (n < 5) n = 5;
			else if (n < 10) n = 10;

			dAlpha = TurnAng / n;

			Point ptCnt = ARANFunctions.PointAlongPlane(pPtPrj, aztNom + 90.0 * TurnDir, r0);

			for (int i = 0; i <= n; i++)
			{
				double R = r0 + (dAlpha * coef * i) + dphi0 * coef;
				Point ptCur = ARANFunctions.PointAlongPlane(ptCnt, azt0 + (i * dAlpha) * TurnDir, R);
				pPointCollection.Add(ptCur);
			}
		}

		public static string MyDD2Str(double X, int Mode)
		{
			double xMin, xIMin, xSec;
			string sTmp;

			int lSign = System.Math.Sign(X);
			X = NativeMethods.Modulus(System.Math.Abs(X), 360.0);

			if (X == 0.0)
				X = 360.0;

			string sSign = "";
			if (lSign < 0)
				sSign = "-";

			double xDeg = System.Math.Floor((double)X);
			string sResult = "";

			switch (Mode)
			{
				case 0:
					sResult = sSign + System.Convert.ToString(System.Math.Round(X, 2)) + "°";
					break;
				case 1:
					sTmp = sSign + System.Convert.ToString(xDeg) + "°";
					xMin = System.Math.Round((X - xDeg) * 60.0, 4);
					sResult = sTmp + System.Convert.ToString(xMin) + "'";

					break;
				case 2:
					xDeg = System.Math.Floor((double)X);
					sTmp = sSign + System.Convert.ToString(xDeg) + "°";
					xMin = (X - xDeg) * 60.0;
					xIMin = System.Math.Floor((double)xMin);
					sTmp = sTmp + System.Convert.ToString(xIMin) + "'";
					xSec = (xMin - xIMin) * 60.0;
					sResult = sTmp + System.Convert.ToString(System.Math.Round(xSec, 2)) + @"""";

					break;
				case 3:
					xDeg = System.Math.Floor((double)X);
					sTmp = sSign + System.Convert.ToString(xDeg) + "°";
					xMin = (X - xDeg) * 60.0;
					xIMin = System.Math.Floor((double)xMin);
					sTmp = sTmp + System.Convert.ToString(xIMin) + "'";
					xSec = (xMin - xIMin) * 60.0;
					sResult = sTmp + System.Convert.ToString(System.Math.Round(xSec, 2)) + @"""";

					if (X > 0)
						sResult = sResult + "N";
					else
						sResult = sResult + "S";

					break;
				case 4:
					xDeg = System.Math.Floor((double)X);
					sTmp = sSign + System.Convert.ToString(xDeg) + "°";
					xMin = (X - xDeg) * 60.0;
					xIMin = System.Math.Floor((double)xMin);
					sTmp = sTmp + System.Convert.ToString(xIMin) + "'";
					xSec = (xMin - xIMin) * 60.0;
					sResult = sTmp + System.Convert.ToString(System.Math.Round(xSec, 2)) + @"""";

					if (X > 0)
						sResult = sResult + "E";
					else
						sResult = sResult + "W";

					break;
			}
			return sResult;
		}

		public static double Point2LineDistancePrj(Point PtTest, Point PtLine, double dirAngle)
		{
			//dirAngle = ARANMath.DegToRadValue * dirAngle;
			double CosA = System.Math.Cos(dirAngle);
			double SinA = System.Math.Sin(dirAngle);

			double dX = PtTest.X - PtLine.X;
			double dY = PtTest.Y - PtLine.Y;

			return System.Math.Abs(dY * CosA - dX * SinA);
		}

		public static double Point2LineDistancePrjSgn(Point PtTest, Point PtLine, double dirAngle)
		{
			//dirAngle = ARANMath.DegToRadValue * dirAngle;
			double CosA = System.Math.Cos(dirAngle);
			double SinA = System.Math.Sin(dirAngle);

			double dX = PtTest.X - PtLine.X;
			double dY = PtTest.Y - PtLine.Y;

			return dY * CosA - dX * SinA;
		}

		public static double SubtractAngles(double X, double Y)
		{
			double subtractAnglesReturn = 0;
			X = NativeMethods.Modulus(X, 360.0);
			Y = NativeMethods.Modulus(Y, 360.0);
			subtractAnglesReturn = NativeMethods.Modulus(X - Y, 360.0);
			if (subtractAnglesReturn > 180.0)
				subtractAnglesReturn = 360.0 - subtractAnglesReturn;

			return subtractAnglesReturn;
		}

		public static void SortIntervals(Interval[] Intervals, bool RightSide = false)
		{
			Interval Tmp;

			int n = Intervals.GetUpperBound(0);

			for (int i = 0; i <= n - 1; i++)
			{
				for (int j = i + 1; j <= n; j++)
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
					else
					{
						if (Intervals[i].Min > Intervals[j].Min)
						{
							Tmp = Intervals[i];
							Intervals[i] = Intervals[j];
							Intervals[j] = Tmp;
						}
					}
				}
			}
		}

		public static Interval[] IntervalsDifference(Interval A, Interval B)
		{
			Interval[] res = GlobalVars.InitArray<Interval>(1);    //--- Checked

			if ((B.Min == B.Max) || (B.Max < A.Min) || (A.Max < B.Min))
			{
				res[0] = A;
			}
			else if ((A.Min < B.Min) && (A.Max > B.Max))
			{
				res = GlobalVars.InitArray<Interval>(2);   //--- Checked
				res[0].Min = A.Min;
				res[0].Max = B.Min;
				res[1].Min = B.Max;
				res[1].Max = A.Max;
			}
			else if (A.Max > B.Max)
			{
				res[0].Min = B.Max;
				res[0].Max = A.Max;
			}
			else if ((A.Min < B.Min))
			{
				res[0].Min = A.Min;
				res[0].Max = B.Min;
			}
			else
			{
				res = new Interval[0];
			}

			return res;
		}

		public static void GetObstInRange(ObstacleType[] ObstSource, ref ObstacleType[] ObstDest, double Range)
		{
			int n = ObstSource.GetUpperBound(0);

			if (n >= 0)
				ObstDest = GlobalVars.InitArray<ObstacleType>(n + 1);    //--- Checked
			else
			{
				ObstDest = new ObstacleType[0];
				return;
			}

			int j = -1;
			for (int i = 0; i <= n; i++)
			{
				if (ObstSource[i].X > Range)
					break;

				j = j + 1;
				ObstDest[j] = ObstSource[i];
			}

			if (j < 0)
				ObstDest = new ObstacleType[0];
			else
				System.Array.Resize<ObstacleType>(ref ObstDest, j + 1);
		}


		public static double IAS2TAS(double IAS, double h, double dT)
		{
			if ((h >= 288.0 / 0.006496) || (h >= (288.0 + dT) / 0.006496))
				return 2.0 * IAS;
			//     h = Int(288.0 / 0.006496)

			return IAS * 171233.0 * System.Math.Sqrt(288.0 + dT - 0.006496 * h) / (Math.Pow((288.0 - 0.006496 * h), 2.628));
		}

		public static double Bank2Radius(double Bank, double v)
		{
			double Rv = 0;

			Rv = 6.355 * System.Math.Tan(ARANMath.DegToRadValue * Bank) / (ARANMath.C_PI * v);

			if (Rv > 0.003)
				Rv = 0.003;

			if (Rv > 0)
				return v / (20.0 * ARANMath.C_PI * Rv);

			return -1;
		}

		public static double Radius2Bank(double R, double v)
		{
			if (R > 0.0)
				return ARANMath.RadToDegValue * (System.Math.Atan(v * v / (20.0 * R * 6.355)));

			return -1;
		}

		//public static double CircleVectorIntersect(Point PtCent, double R, Point ptVect, double DirVect, out Point ptRes)
		//{
		//    double tanAlpha = Math.Tan(DirVect);
		//    double tanBetha = Math.Tan(DirVect + ARANMath.C_PI_2);

		//    double x = (tanBetha * PtCent.X - tanAlpha * ptVect.X - PtCent.Y + ptVect.Y);
		//    double y = tanBetha * (x - PtCent.X) + PtCent.Y;

		//    double DistCnt2Vect = ARANMath.Hypot(PtCent.X - x, PtCent.Y - y);

		//    if (DistCnt2Vect < R)
		//    {
		//        double CosA = Math.Cos(DirVect);
		//        double SinA = Math.Sin(DirVect);
		//        double d = System.Math.Sqrt(R * R - DistCnt2Vect * DistCnt2Vect);

		//        ptRes = new Point(x + d * CosA, y + d * SinA);
		//        return d;
		//    }

		//    ptRes = new Point();
		//    return 0.0;
		//}

		//public static double CircleVectorIntersect(Point PtCent, double R, Point ptVect, double DirVect)
		//{
		//    Point transTemp66;
		//    return CircleVectorIntersect(PtCent, R, ptVect, DirVect, out transTemp66);
		//}

		public static bool AngleInSector(double Angle, double X, double Y)
		{
			double Sector = Functions.SubtractAngles(X, Y);
			double Sub1 = Functions.SubtractAngles(X, Angle);
			double Sub2 = Functions.SubtractAngles(Angle, Y);

			return !((Sub1 + Sub2 > Sector + GlobalVars.degEps));
		}

#if GG
		public static MultiLineString ReturnPolygonPartAsPolyline(MultiPolygon pPolygon, Point PtDerL, double CLDir, int Turn)
		{
			MultiLineString returnPolygonPartAsPolylineReturn = null;
			int I = 0;
			int N = 0;
			int Side = 0;
			MultiLineString pLine = null;
			Point pPt = null;
			MultiPoint pTmpPoly = null;

			pTmpPoly = RemoveAgnails(pPolygon);
			pTmpPoly = ReArrangePolygon(pTmpPoly, PtDerL, CLDir, false);

			pPt = ARANFunctions.PointAlongPlane(PtDerL, CLDir + 180.0, 30000.0);
			returnPolygonPartAsPolylineReturn = new Polyline();
			N = pTmpPoly.PointCount - 1;

			for (I = 0; I <= N; I++)
			{
				Side = Functions.SideDef(pPt, CLDir, pTmpPoly.get_Point(I));
				if (Side == Turn)
				{
					returnPolygonPartAsPolylineReturn.AddPoint(pTmpPoly.get_Point(I));
				}
			}

			if (Turn < 0)
			{
				pLine = ((ESRI.ArcGIS.Geometry.IPolyline)(returnPolygonPartAsPolylineReturn));
				pLine.ReverseOrientation();
			}
			return returnPolygonPartAsPolylineReturn;
		}

		public static MultiPolygon ReArrangePolygon(MultiPolygon pPolygon, Point ptBase, double BaseDir, bool bFlag = false)
		{
			ESRI.ArcGIS.Geometry.IPointCollection reArrangePolygonReturn = null;
			int I = 0;
			int J = 0;

			int N = 0;
			int iStart = 0;
			int Side = 0;

			double dl = 0;
			double dm = 0;

			double dX0 = 0;
			double dY0 = 0;

			double dX1 = 0;
			double dY1 = 0;
			PointCollection pPoly = null;
			ITopologicalOperator2 pTopoOper = null;

			IPoint pPt = null;

			pTopoOper = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(pPolygon));
			pTopoOper.IsKnownSimple_2 = false;
			pTopoOper.Simplify();

			if (pPolygon.PointCount < 4)
			{
				return pPolygon;
			}

			pPt = Functions.PointAlongPlane(ptBase, BaseDir + 180.0, 30000.0);

			pPoly = new Polyline();
			pPoly.AddPointCollection(pPolygon);

			N = pPoly.PointCount - 1;
			pPoly.RemovePoints(N, 1);

			iStart = -1;

			Side = Functions.SideDef(pPt, BaseDir, pPoly.get_Point(0));
			if (Side < 0)
			{
				dm = Functions.Point2LineDistancePrj(pPoly.get_Point(0), pPt, BaseDir + 90.0);
				iStart = 0;
			}

			for (I = 1; I <= N - 1; I++)
			{
				Side = Functions.SideDef(pPt, BaseDir, pPoly.get_Point(I));
				if (Side < 0)
				{
					dl = Functions.Point2LineDistancePrj(pPoly.get_Point(I), pPt, BaseDir + 90.0);
					if ((dl < dm) || (iStart < 0))
					{
						dm = dl;
						iStart = I;
					}
				}
			}

			if (bFlag)
			{
				if (iStart == 0)
				{
					iStart = N - 1;
				}
				else
				{
					iStart = iStart - 1;
				}
			}

			dX0 = pPoly.get_Point(1).X - pPoly.get_Point(0).X;
			dY0 = pPoly.get_Point(1).Y - pPoly.get_Point(0).Y;
			I = 1;

			while (I < N)
			{
				J = (I + 1) % (N - 1);
				dX1 = pPoly.get_Point(J).X - pPoly.get_Point(I).X;
				dY1 = pPoly.get_Point(J).Y - pPoly.get_Point(I).Y;
				dl = Functions.ReturnDistanceInMeters(pPoly.get_Point(J), pPoly.get_Point(I));

				if (dl < 0.001)
				{
					pPoly.RemovePoints(I, 1);
					N = N - 1;
					J = System.Convert.ToInt32(NativeMethods.Modulus(I + 1, N));
					if (I <= iStart)
					{
						iStart = iStart - 1;
					}

					dX1 = dX0;
					dY1 = dY0;
				}
				else if ((dY0 != 0.0) && (I != iStart))
				{
					if (dY1 != 0.0)
					{
						if (System.Math.Abs(System.Math.Abs(dX0 / dY0) - System.Math.Abs(dX1 / dY1)) < 0.00001)
						{
							pPoly.RemovePoints(I, 1);
							N = N - 1;
							J = (I + 1) % N;
							if (I <= iStart)
							{
								iStart = iStart - 1;
							}

							dX1 = dX0;
							dY1 = dY0;
						}
						else
						{
							I = I + 1;
						}
					}
					else
					{
						I = I + 1;
					}
				}
				else if ((dX0 != 0.0) && (I != iStart))
				{
					if (dX1 != 0.0)
					{
						if (System.Math.Abs(System.Math.Abs(dY0 / dX0) - System.Math.Abs(dY1 / dX1)) < 0.00001)
						{
							pPoly.RemovePoints(I, 1);
							N = N - 1;
							J = (I + 1) % N;
							if (I <= iStart)
							{
								iStart = iStart - 1;
							}

							dX1 = dX0;
							dY1 = dY0;
						}
						else
						{
							I = I + 1;
						}
					}
					else
					{
						I = I + 1;
					}
				}
				else
				{
					I = I + 1;
				}
				dX0 = dX1;
				dY0 = dY1;
			}

			N = pPoly.PointCount;
			reArrangePolygonReturn = new Polygon();

			for (I = N - 1; I >= 0; I += -1)
			{
				J = System.Convert.ToInt32(NativeMethods.Modulus(I + iStart, N));
				reArrangePolygonReturn.AddPoint(pPoly.get_Point(J));
			}

			// DrawPolygon ReArrangePolygon, 255
			// Set pPoly = New Polyline
			// pPoly.re
			// pPoly.ReverseOrientation
			return reArrangePolygonReturn;
		}

		public static double CalcSpiralStartPoint(IPointCollection LinePoint, ref ObstacleHd DetObs, double coef, double r0, double DepDir, double AztDir, int TurnDir, bool bDerFlg)
		{
			double calcSpiralStartPointReturn = 0;
			IPoint[] BasePoints = null;
			IPoint ptTmp2 = null;
			IPoint PtTurn = null;
			IPoint ptCnt = null;
			IPoint ptTmp = null;
			ILine pLine = null;

			IConstructPoint pConstructor = null;
			IProximityOperator pProxi = null;

			double ASinAlpha = 0;
			double MaxTheta = 0;
			double dAlpha = 0;
			double alpha = 0;
			double Theta = 0;
			double hTMin = 0;
			double Dist = 0;
			double fTmp = 0;
			double hT = 0;

			int Offset = 0;
			int N = 0;
			int I = 0;
			int iMin = 0;
			int Side1 = 0;
			int Side = 0;


			if (bDerFlg)
			{
				Offset = 1;
			}
			else
			{
				Offset = 0;
			}

			pLine = new Line();
			pProxi = ((ESRI.ArcGIS.Geometry.IProximityOperator)(pLine));

			N = LinePoint.PointCount - Offset;

			if (N < 2)
			{
				DetObs.CourseAdjust = GlobalVars.NO_VALUE;
				return -100;
			}

			if (N > 0)
			{
				BasePoints = new ESRI.ArcGIS.Geometry.IPoint[N + 1 + 1]; //--- Checked
			}
			else
			{
				BasePoints = new ESRI.ArcGIS.Geometry.IPoint[0];
			}

			for (I = 0; I <= N - 1; I++)
			{
				BasePoints[I] = LinePoint.get_Point(I + Offset);

				if (I == N - 1)
				{
					BasePoints[I].M = BasePoints[I - 1].M;
				}
				else
				{
					BasePoints[I].M = Functions.ReturnAngleInDegrees(LinePoint.get_Point(I + Offset), LinePoint.get_Point(I + Offset + 1));
				}
			}

			ptCnt = new ESRI.ArcGIS.Geometry.Point();
			pConstructor = ((ESRI.ArcGIS.Geometry.IConstructPoint)(ptCnt));

			PtTurn = null;

			hTMin = GlobalVars.RModel;
			iMin = -1;

			MaxTheta = SpiralTouchAngle(r0, coef, DepDir, DepDir + 90.0 * TurnDir, TurnDir);
			if (MaxTheta > 180.0)
			{
				MaxTheta = 360.0 - MaxTheta;
			}

			for (I = 0; I <= N - 2; I++)
			{
				Side = Functions.SideDef(BasePoints[I], (BasePoints[I].M), DetObs.pPtPrj);
				alpha = DepDir + 90.0 * Side;

				if (System.Math.Abs(System.Math.Sin(ARANMath.DegToRadValue * (alpha - BasePoints[I].M))) > GlobalVars.degEps)
				{

					dAlpha = Functions.SubtractAngles(alpha, BasePoints[I].M);
					ptTmp = Functions.PointAlongPlane(BasePoints[I], DepDir - 90.0 * Side, r0);

					Dist = Functions.Point2LineDistancePrj(DetObs.pPtPrj, ptTmp, BasePoints[I].M);
					Side1 = Functions.SideDef(ptTmp, BasePoints[I].M, DetObs.pPtPrj);

					Theta = 0.5 * MaxTheta;
					do
					{
						fTmp = Theta;
						ASinAlpha = Dist / (r0 + Theta * coef);
						if (System.Math.Abs(ASinAlpha) <= 1.0)
							Theta = dAlpha - ARANMath.RadToDegValue * (Side1 * TurnDir * System.Math.Asin(ASinAlpha));
						else
						{
							Theta = MaxTheta;
							break;
						}
					}
					while (System.Math.Abs(fTmp - Theta) > GlobalVars.degEps);

					fTmp = System.Math.Sin(ARANMath.DegToRadValue * Theta) * (r0 + Theta * coef);

					if (Theta > MaxTheta)
					{
						hT = System.Math.Sin(ARANMath.DegToRadValue * MaxTheta) * (r0 + MaxTheta * coef);
						fTmp = (hT - fTmp);
						Theta = MaxTheta;
					}
					else
					{
						hT = fTmp;
						fTmp = 0.0;
					}

					ptTmp2 = Functions.PointAlongPlane(DetObs.pPtPrj, DepDir + 180.0, hT + fTmp);
					pConstructor.ConstructAngleIntersection(ptTmp2, ARANMath.DegToRadValue * (DepDir + 90.0), ptTmp, ARANMath.DegToRadValue * BasePoints[I].M);

					ptTmp = Functions.PointAlongPlane(ptCnt, DepDir - TurnDir * 90.0, r0);

					pLine.FromPoint = BasePoints[I];
					pLine.ToPoint = BasePoints[I + 1];

					fTmp = pProxi.ReturnDistance(ptTmp);

					if (fTmp < GlobalVars.distEps)
					{
						if (hT < hTMin)
						{
							hTMin = hT;
							iMin = I;
							PtTurn = ptTmp;
							PtTurn.M = Theta;
							PtTurn.Z = DetObs.Dist - hTMin;
							if ((PtTurn.Z < 0.0))
							{
								PtTurn.Z = 0.0;
							}
						}
					}
				}
			}

			if (iMin > -1)
			{
				calcSpiralStartPointReturn = PtTurn.Z;
				DetObs.CourseAdjust = PtTurn.M;
			}
			else
			{
				calcSpiralStartPointReturn = -100.0;
				DetObs.CourseAdjust = -9999.0;
			}
			return calcSpiralStartPointReturn;
		}

		public static double FixToTouchSpiral(IPoint ptBase, double coef0, double TurnR, int TurnDir,
												double Theta, IPoint FixPnt, double DepCourse)
		{
			double fixToTouchSpiralReturn = 0;
			int I = 0;

			double R = 0;
			double X1 = 0;
			double Y1 = 0;
			double Theta0 = 0;
			double Theta1 = 0;
			double fTmp1 = 0;
			double fTmp2 = 0;
			double Theta1New = 0;
			double coef = 0;
			double Dist = 0;
			double FixTheta = 0;
			double dTheta = 0;
			double CntTheta = 0;

			double f = 0;
			double F1 = 0;
			double SinT = 0;
			double CosT = 0;

			IPoint PtCntSpiral = null;
			IPoint ptOut = null;

			fixToTouchSpiralReturn = -1000;

			coef = ARANMath.RadToDegValue * coef0;
			Theta0 = NativeMethods.Modulus(90.0 * TurnDir + DepCourse + 180.0, 360.0);
			PtCntSpiral = Functions.PointAlongPlane(ptBase, DepCourse + 90.0 * TurnDir, TurnR);
			Dist = Functions.ReturnDistanceInMeters(PtCntSpiral, FixPnt);
			FixTheta = Functions.ReturnAngleInDegrees(PtCntSpiral, FixPnt);
			dTheta = NativeMethods.Modulus((FixTheta - Theta0) * TurnDir, 360.0);
			R = TurnR + dTheta * coef0;
			if (Dist < R)
			{
				return fixToTouchSpiralReturn;
			}

			X1 = FixPnt.X - PtCntSpiral.X;
			Y1 = FixPnt.Y - PtCntSpiral.Y;
			CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, Theta, TurnDir);
			CntTheta = NativeMethods.Modulus(Theta0 + CntTheta * TurnDir, 360.0);
			// ===============================Variant Fiddowsy ==================================

			Theta1 = CntTheta;
			for (I = 0; I <= 20; I++)
			{
				dTheta = NativeMethods.Modulus((Theta1 - Theta0) * TurnDir, 360.0);
				SinT = System.Math.Sin(ARANMath.DegToRadValue * Theta1);
				CosT = System.Math.Cos(ARANMath.DegToRadValue * Theta1);
				R = TurnR + dTheta * coef0;
				f = R * R - (Y1 * R + X1 * coef * TurnDir) * SinT - (X1 * R - Y1 * coef * TurnDir) * CosT;
				F1 = 2 * R * coef * TurnDir - (Y1 * R + 2 * X1 * coef * TurnDir) * CosT + (X1 * R - 2 * Y1 * coef * TurnDir) * SinT;
				Theta1New = Theta1 - ARANMath.RadToDegValue * (f / F1);

				fTmp1 = Functions.SubtractAngles(Theta1New, Theta1);
				if (fTmp1 < 0.0001)
				{
					Theta1 = Theta1New;
					break;
				}
				Theta1 = Theta1New;
			}

			dTheta = NativeMethods.Modulus((Theta1 - Theta0) * TurnDir, 360.0);
			R = TurnR + dTheta * coef0;

			ptOut = Functions.PointAlongPlane(PtCntSpiral, Theta1, R);
			fTmp1 = Functions.ReturnAngleInDegrees(ptOut, FixPnt);
			CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, fTmp1, TurnDir);
			CntTheta = NativeMethods.Modulus(Theta0 + CntTheta * TurnDir, 360.0);
			fTmp2 = Functions.SubtractAngles(CntTheta, Theta1);

			if (fTmp2 < 0.0001)
			{
				fixToTouchSpiralReturn = fTmp1;
				return fixToTouchSpiralReturn;
			}

			return fixToTouchSpiralReturn;
		}

		public static void CreateNavaidZone(IPoint ptNav, double dirAngle, eNavaidType pNavType, double Multiplicity,
											IPointCollection LPolygon, IPointCollection RPolygon, IPointCollection PrimPolygon)
		{
			int NavType = (int)pNavType;

			IPoint Pt0 = null;
			IPoint Pt1 = null;
			IPoint Pt2 = null;
			IPoint pt3 = null;
			IPoint pt4 = null;
			IPoint pt5 = null;
			double dZone = 0;
			double BaseLength = 0;
			double alpha = 0;
			double Betta = 0;
			double d0 = 0;

			if ((NavType == 2))
			{
				BaseLength = Navaids_DataBase.NDB.InitWidth * 0.5;
				alpha = Navaids_DataBase.NDB.SplayAngle;
				dZone = Navaids_DataBase.NDB.Range * Multiplicity;
			}
			else if ((NavType == 0))
			{
				BaseLength = 0.5 * Navaids_DataBase.VOR.InitWidth;
				alpha = Navaids_DataBase.VOR.SplayAngle;
				dZone = Navaids_DataBase.VOR.Range * Multiplicity;
			}
			else
			{
				return;
			}

			d0 = dZone / System.Math.Cos(ARANMath.DegToRadValue * alpha);
			Betta = 0.5 * System.Math.Tan(ARANMath.DegToRadValue * alpha);
			Betta = System.Math.Atan(Betta);
			Betta = ARANMath.RadToDegValue * Betta;

			if (LPolygon.PointCount > 0)
			{
				LPolygon.RemovePoints(0, LPolygon.PointCount);
			}
			if (RPolygon.PointCount > 0)
			{
				RPolygon.RemovePoints(0, RPolygon.PointCount);
			}
			if (PrimPolygon.PointCount > 0)
			{
				PrimPolygon.RemovePoints(0, PrimPolygon.PointCount);
			}

			// ==========LeftPolygon
			Pt0 = Functions.PointAlongPlane(ptNav, dirAngle + 90.0, BaseLength);
			pt3 = Functions.PointAlongPlane(ptNav, dirAngle + 90.0, 0.5 * BaseLength);
			Pt1 = Functions.PointAlongPlane(Pt0, dirAngle + alpha, d0);
			Pt2 = Functions.PointAlongPlane(pt3, dirAngle + Betta, d0);
			pt4 = Functions.PointAlongPlane(pt3, dirAngle + 180.0 - Betta, d0);
			pt5 = Functions.PointAlongPlane(Pt0, dirAngle + 180.0 - alpha, d0);

			LPolygon.AddPoint(Pt0);
			LPolygon.AddPoint(Pt1);
			LPolygon.AddPoint(Pt2);
			LPolygon.AddPoint(pt3);
			LPolygon.AddPoint(pt4);
			LPolygon.AddPoint(pt5);
			// LPolygon.AddPoint Pt0

			PrimPolygon.AddPoint(pt4);
			PrimPolygon.AddPoint(pt3);
			PrimPolygon.AddPoint(Pt2);

			// ==========RightPolygon
			Pt0 = Functions.PointAlongPlane(ptNav, dirAngle - 90.0, 0.5 * BaseLength);
			pt3 = Functions.PointAlongPlane(ptNav, dirAngle - 90.0, BaseLength);
			Pt1 = Functions.PointAlongPlane(Pt0, dirAngle - Betta, d0);
			Pt2 = Functions.PointAlongPlane(pt3, dirAngle - alpha, d0);
			pt4 = Functions.PointAlongPlane(pt3, dirAngle + 180.0 + alpha, d0);
			pt5 = Functions.PointAlongPlane(Pt0, dirAngle + 180.0 + Betta, d0);

			RPolygon.AddPoint(Pt0);
			RPolygon.AddPoint(Pt1);
			RPolygon.AddPoint(Pt2);
			RPolygon.AddPoint(pt3);
			RPolygon.AddPoint(pt4);
			RPolygon.AddPoint(pt5);
			// RPolygon.AddPoint Pt0

			PrimPolygon.AddPoint(Pt1);
			PrimPolygon.AddPoint(Pt0);
			PrimPolygon.AddPoint(pt5);
			// PrimPolygon.AddPoint PrimPolygon.Point(0)
		}

		public static bool RayPolylineIntersect(IPolyline pPolyline, IPoint RayPt, double RayDir, out IPoint InterPt)
		{
			double dMin = 5000000.0;

			IPolyline pLine = (ESRI.ArcGIS.Geometry.IPolyline)(new Polyline());
			pLine.FromPoint = RayPt;
			pLine.ToPoint = Functions.PointAlongPlane(RayPt, RayDir, dMin);

			ITopologicalOperator pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator)pPolyline;
			IPointCollection pPoints = (ESRI.ArcGIS.Geometry.IPointCollection)pTopo.Intersect(pLine, esriGeometryDimension.esriGeometry0Dimension);

			int n = pPoints.PointCount;
			InterPt = null;

			if (n == 0)
				return false;

			if (n == 1)
				InterPt = pPoints.get_Point(0);
			else
			{
				for (int i = 0; i < n; i++)
				{
					double d = Functions.ReturnDistanceInMeters(RayPt, pPoints.get_Point(i));
					if (d < dMin)
					{
						dMin = d;
						InterPt = pPoints.get_Point(i);
					}
				}
			}

			return InterPt != null;
		}

		public static bool RayPolylineIntersect(IPointCollection pPolyline, IPoint RayPt, double RayDir, out IPoint InterPt)
		{
			return RayPolylineIntersect(pPolyline as IPolyline, RayPt, RayDir, out InterPt);
		}

		public static Polygon RemoveFars(IPointCollection pPolygon, IPoint pPoint)
		{
			return RemoveFars(pPolygon as IPolygon, pPoint);
		}

		public static Polygon RemoveFars(IPolygon pPolygon, IPoint pPoint)
		{
			IClone pClone = (IClone)pPolygon;
			ESRI.ArcGIS.Geometry.Polygon result = (ESRI.ArcGIS.Geometry.Polygon)pClone.Clone();

			IGeometryCollection Geocollect = (ESRI.ArcGIS.Geometry.IGeometryCollection)result;
			int n = Geocollect.GeometryCount;

			if (n > 1)
			{
				int i;
				double OutDist = 20000000000.0, tmpDist;

				IGeometryCollection tmpCollect = (ESRI.ArcGIS.Geometry.IGeometryCollection)(new Polygon());
				IProximityOperator pProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)pPoint;

				for (i = 0; i < n; i++)
				{
					tmpCollect.AddGeometry(Geocollect.get_Geometry(i));
					tmpDist = pProxi.ReturnDistance(tmpCollect as IGeometry);
					if (OutDist > tmpDist)
						OutDist = tmpDist;
					tmpCollect.RemoveGeometries(0, 1);
				}

				i = 0;
				while (i < n)
				{
					tmpCollect.AddGeometry(Geocollect.get_Geometry(i));
					tmpDist = pProxi.ReturnDistance(tmpCollect as IGeometry);
					if (OutDist < tmpDist)
					{
						Geocollect.RemoveGeometries(i, 1);
						n--;
					}
					else
						i++;

					tmpCollect.RemoveGeometries(0, 1);
				}
			}
			return result;
		}

		public static IPolygon RemoveHoles(IPointCollection pPolygon)
		{
			return RemoveHoles(pPolygon as IPolygon);
		}

		public static IPolygon RemoveHoles(IPolygon pPolygon)
		{
			IClone pClone = (IClone)pPolygon;
			IPolygon result = (IPolygon)pClone.Clone();
			IGeometryCollection NewPolygon = (ESRI.ArcGIS.Geometry.IGeometryCollection)result;

			int i = 0;

			while (i < NewPolygon.GeometryCount)
			{
				IRing pInteriorRing = (ESRI.ArcGIS.Geometry.IRing)NewPolygon.get_Geometry(i);
				if (!pInteriorRing.IsExterior)
					NewPolygon.RemoveGeometries(i, 1);
				else
					i++;
			}
			return result;
		}

		private static double CalcNomPos(Point ptDMEprj, double Xs, double Ys, double d0,
										double BaseHeight, double fRefAltitude, double PDG, int AheadBehindSide, int NearSide)
		{
			double calcNomPosReturn = 0;
			double dNomPosDer = 0;
			double dNomPosDME = 0;
			double dOldPosDME = 0;
			double hMax = 0;
			int I = 0;

			I = 0;
			dNomPosDME = d0 + NearSide * Navaids_DataBase.DME.MinimalError;
			hMax = 0.0;

			do
			{
				dNomPosDer = Xs + AheadBehindSide * System.Math.Sqrt(dNomPosDME * dNomPosDME - Ys * Ys);
				hMax = BaseHeight + dNomPosDer * PDG + fRefAltitude - ptDMEprj.Z;

				dOldPosDME = dNomPosDME;
				dNomPosDME = (d0 + NearSide * Navaids_DataBase.DME.MinimalError) / (1.0 - NearSide * Navaids_DataBase.DME.ErrorScalingUp * System.Math.Sqrt(1.0 + hMax * hMax / (dNomPosDer * dNomPosDer)));

				I ++;
				if (I > 5)
					break;
			}
			while (System.Math.Abs(dOldPosDME - dNomPosDME) > GlobalVars.distEps);

			calcNomPosReturn = dNomPosDME;
			return calcNomPosReturn;
		}

		private static Interval CalcDMERange(Point ptBasePrj, double BaseHeight, double fRefAltitude, double NomDir,
											double PDG, Point ptDMEprj, LineString KKhMin, LineString KKhMax)
		{
			Interval calcDMERangeReturn = new Interval();
			int Side = 0;
			double d0 = 0;
			double d1 = 0;
			double Ys = 0;
			double Xs = 0;
			double Dist0 = 0;
			double Dist1 = 0;
			int LeftRightSide = 0;
			int AheadBehindSide = 0;

			AheadBehindSide = Functions.SideDef(KKhMin.FromPoint, NomDir + 90.0, ptDMEprj);
			LeftRightSide = Functions.SideDef(ptBasePrj, NomDir, ptDMEprj);

			Xs = Point2LineDistancePrj(ptDMEprj, ptBasePrj, NomDir + 90.0) * Functions.SideDef(ptBasePrj, NomDir + 90.0, ptDMEprj);
			Ys = Point2LineDistancePrj(ptDMEprj, ptBasePrj, NomDir);

			if (AheadBehindSide < 0)
			{
				if (LeftRightSide > 0)
				{
					d0 = ReturnDistanceInMeters(ptDMEprj, KKhMin.ToPoint);

					Side = Functions.SideDef(KKhMax.FromPoint, NomDir, ptDMEprj);
					if (Side < 0)
					{
						d1 = Point2LineDistancePrj(ptDMEprj, KKhMax.FromPoint, NomDir + 90.0);
					}
					else
					{
						d1 = ReturnDistanceInMeters(ptDMEprj, KKhMax.FromPoint);
					}
				}
				else
				{
					d0 = ReturnDistanceInMeters(ptDMEprj, KKhMin.FromPoint);

					Side = Functions.SideDef(KKhMax.ToPoint, NomDir, ptDMEprj);
					if (Side > 0)
					{
						d1 = Point2LineDistancePrj(ptDMEprj, KKhMax.ToPoint, NomDir + 90.0);
					}
					else
					{
						d1 = ReturnDistanceInMeters(ptDMEprj, KKhMax.ToPoint);
					}
				}
			}
			else
			{
				if (LeftRightSide > 0)
				{
					d0 = ReturnDistanceInMeters(ptDMEprj, KKhMax.ToPoint);

					Side = Functions.SideDef(KKhMin.FromPoint, NomDir, ptDMEprj);
					if (Side < 0)
					{
						d1 = Point2LineDistancePrj(ptDMEprj, KKhMin.ToPoint, NomDir + 90.0);
					}
					else
					{
						d1 = ReturnDistanceInMeters(ptDMEprj, KKhMin.FromPoint);
					}
				}
				else
				{
					d0 = ReturnDistanceInMeters(ptDMEprj, KKhMax.FromPoint);

					Side = Functions.SideDef(KKhMin.ToPoint, NomDir, ptDMEprj);
					if (Side > 0)
					{
						d1 = Point2LineDistancePrj(ptDMEprj, KKhMin.ToPoint, NomDir + 90.0);
					}
					else
					{
						d1 = ReturnDistanceInMeters(ptDMEprj, KKhMin.ToPoint);
					}
				}
			}

			Dist0 = CalcNomPos(ptDMEprj, Xs, Ys, d0, BaseHeight, fRefAltitude, PDG, AheadBehindSide, 1);
			Dist1 = CalcNomPos(ptDMEprj, Xs, Ys, d1, BaseHeight, fRefAltitude, PDG, AheadBehindSide, -1);

			calcDMERangeReturn.Left = Dist0;
			calcDMERangeReturn.Right = Dist1;
			return calcDMERangeReturn;
		}


		public static IPointCollection CalcTouchByFixDir(IPoint PtSt, IPoint PtFix, double TurnR, double DirCur,
														ref double DirFix, int TurnDir, double SnapAngle,
														ref double dDir, ref IPoint FlyBy)
		{
			ESRI.ArcGIS.Geometry.IPointCollection calcTouchByFixDirReturn = null;
			IConstructPoint Constructor = null;

			IPoint ptCnt1 = null;
			IPoint Pt1 = null;
			IPoint Pt10 = null;
			IPoint Pt11 = null;

			IPoint Pt2 = null;
			IPoint pt3 = null;

			int SideD = 0;
			int SideT = 0;

			double DeltaAngle = 0;
			double DeltaDist = 0;
			double distToTmp = 0;
			double dirToTmp = 0;
			double OutDir = 0;
			double OutDir0 = 0;
			double OutDir1 = 0;
			double Dist = 0;

			calcTouchByFixDirReturn = new Multipoint();

			if (Functions.SubtractAngles(DirCur, DirFix) < 0.5)
			{
				DirFix = DirCur;
				if (Functions.ReturnDistanceInMeters(PtFix, PtSt) < GlobalVars.distEps)
				{
					FlyBy = new ESRI.ArcGIS.Geometry.Point();
					FlyBy.PutCoords(PtFix.X, PtFix.Y);
					calcTouchByFixDirReturn.AddPoint(PtSt);
					calcTouchByFixDirReturn.AddPoint(PtSt);
					return calcTouchByFixDirReturn;
				}
			}

			ptCnt1 = Functions.PointAlongPlane(PtSt, DirCur + 90.0 * TurnDir, TurnR);
			PtSt.M = DirCur;

			OutDir0 = NativeMethods.Modulus(DirFix - SnapAngle * TurnDir, 360.0);
			OutDir1 = NativeMethods.Modulus(DirFix + SnapAngle * TurnDir, 360.0);

			Pt10 = Functions.PointAlongPlane(ptCnt1, OutDir0 - 90.0 * TurnDir, TurnR);
			Pt11 = Functions.PointAlongPlane(ptCnt1, OutDir1 - 90.0 * TurnDir, TurnR);

			SideT = Functions.SideDef(Pt10, DirFix, PtFix);
			SideD = Functions.SideDef(Pt10, DirFix, ptCnt1);

			if (SideT * SideD < 0)
			{
				Pt1 = Pt10;
				OutDir = OutDir0;
			}
			else
			{
				Pt1 = Pt11;
				OutDir = OutDir1;
			}

			Pt1.M = OutDir;

			FlyBy = new ESRI.ArcGIS.Geometry.Point();
			Constructor = ((ESRI.ArcGIS.Geometry.IConstructPoint)(FlyBy));

			Constructor.ConstructAngleIntersection(Pt1, ARANMath.DegToRadValue * OutDir, PtFix, ARANMath.DegToRadValue * DirFix);

			Dist = Functions.ReturnDistanceInMeters(Pt1, FlyBy);

			dirToTmp = Functions.ReturnAngleInDegrees(PtFix, FlyBy);
			distToTmp = Functions.ReturnDistanceInMeters(PtFix, FlyBy);

			SideT = AnglesSideDef(OutDir, DirFix);

			if (SideT > 0)
			{
				DeltaAngle = NativeMethods.Modulus(180.0 + DirFix - OutDir, 360.0);
			}
			else if (SideT < 0)
			{
				DeltaAngle = NativeMethods.Modulus(OutDir - 180.0 - DirFix, 360.0);
			}

			DeltaAngle = 0.5 * DeltaAngle;
			DeltaDist = TurnR / System.Math.Tan(ARANMath.DegToRadValue * DeltaAngle);

			dDir = Dist - DeltaDist;

			if (DeltaDist <= Dist)
			{
				Pt2 = Functions.PointAlongPlane(FlyBy, OutDir - 180.0, DeltaDist);
				pt3 = Functions.PointAlongPlane(FlyBy, DirFix, DeltaDist);
			}
			else
			{
				Pt2 = Functions.PointAlongPlane(FlyBy, OutDir, DeltaDist);
				pt3 = Functions.PointAlongPlane(FlyBy, DirFix - 180.0, DeltaDist);
			}

			Pt2.M = OutDir;
			pt3.M = DirFix;

			calcTouchByFixDirReturn.AddPoint(PtSt);
			calcTouchByFixDirReturn.AddPoint(Pt1);
			calcTouchByFixDirReturn.AddPoint(Pt2);
			calcTouchByFixDirReturn.AddPoint(pt3);
			return calcTouchByFixDirReturn;
		}

		public static int AnglesSideDef(double X, double Y)
		{
			double Z = NativeMethods.Modulus(X - Y, 360.0);
			if (Z > 360.0 - GlobalVars.degEps || Z < GlobalVars.degEps)
				return 0;

			if (Z > 180.0 - GlobalVars.degEps)
				return -1;

			if (Z < 180.0 + GlobalVars.degEps)
				return 1;

			return 2;
		}

		public static IPointCollection CalcTrajectoryFromMultiPoint(IPointCollection MultiPoint)
		{
			ESRI.ArcGIS.Geometry.IPointCollection calcTrajectoryFromMultiPointReturn = null;
			IConstructPoint ptConstr = null;
			IPoint FromPt = null;
			IPoint CntPt = null;
			IPoint ToPt = null;

			double fTmp = 0;
			double fE = 0;

			int Side = 0;
			int I = 0;
			int N = 0;

			CntPt = new ESRI.ArcGIS.Geometry.Point();
			ptConstr = ((ESRI.ArcGIS.Geometry.IConstructPoint)(CntPt));
			calcTrajectoryFromMultiPointReturn = new Polyline();
			fE = ARANMath.DegToRadValue * 0.5;

			N = MultiPoint.PointCount - 2;

			calcTrajectoryFromMultiPointReturn.AddPoint(MultiPoint.get_Point(0));

			for (I = 0; I <= N; I++)
			{
				FromPt = MultiPoint.get_Point(I);
				ToPt = MultiPoint.get_Point(I + 1);
				fTmp = ARANMath.DegToRadValue * (FromPt.M - ToPt.M);

				if ((System.Math.Abs(System.Math.Sin(fTmp)) <= fE) && (System.Math.Cos(fTmp) > 0.0))
				{
					calcTrajectoryFromMultiPointReturn.AddPoint(ToPt);
				}
				else
				{
					if (System.Math.Abs(System.Math.Sin(fTmp)) > fE)
					{
						ptConstr.ConstructAngleIntersection(FromPt, ARANMath.DegToRadValue * (NativeMethods.Modulus(FromPt.M + 90.0, 360.0)), ToPt, ARANMath.DegToRadValue * (NativeMethods.Modulus(ToPt.M + 90.0, 360.0)));
					}
					else
					{
						CntPt.PutCoords(0.5 * (FromPt.X + ToPt.X), 0.5 * (FromPt.Y + ToPt.Y));
					}
					Side = Functions.SideDef(FromPt, FromPt.M, ToPt);
					calcTrajectoryFromMultiPointReturn.AddPointCollection(CreateArcPrj(CntPt, FromPt, ToPt, -Side));
				}
			}
			return calcTrajectoryFromMultiPointReturn;
		}

		public static IPointCollection TurnToFixPrj(IPoint PtSt, double TurnR, int TurnDir, IPoint FixPnt)
		{
			ESRI.ArcGIS.Geometry.IPointCollection turnToFixPrjReturn = null;
			IPoint ptCnt = null;
			IPoint Pt1 = null;
			double DeltaAngle = 0;
			double DirFx2Cnt = 0;
			double DistFx2Cnt = 0;
			double DirCur = 0;

			DirCur = PtSt.M;

			turnToFixPrjReturn = new Multipoint();
			ptCnt = Functions.PointAlongPlane(PtSt, DirCur + 90.0 * TurnDir, TurnR);

			DistFx2Cnt = Functions.ReturnDistanceInMeters(ptCnt, FixPnt);


			// If DistFx2Cnt + distEps < TurnR Then
			//     TurnR = DistFx2Cnt
			//     Exit Function
			// End If

			// If DistFx2Cnt < TurnR Then
			//     DistFx2Cnt = TurnR
			// End If

			DirFx2Cnt = Functions.ReturnAngleInDegrees(ptCnt, FixPnt);
			DeltaAngle = -ARANMath.RadToDegValue * (System.Math.Acos(TurnR / DistFx2Cnt)) * TurnDir;

			if (double.IsNaN(DeltaAngle))
				DeltaAngle = 0.0;

			Pt1 = Functions.PointAlongPlane(ptCnt, DirFx2Cnt + DeltaAngle, TurnR);
			Pt1.M = Functions.ReturnAngleInDegrees(Pt1, FixPnt);

			turnToFixPrjReturn.AddPoint(PtSt);
			turnToFixPrjReturn.AddPoint(Pt1);
			return turnToFixPrjReturn;
		}

		public static IPointCollection CreateBasePoints(IPointCollection pPolygone, IPolyline K1K1, double lDepDir, int lTurnDir)
		{
			ESRI.ArcGIS.Geometry.IPointCollection createBasePointsReturn = null;
			IPointCollection TmpPoly = null;
			bool bFlg = false;
			int I = 0;
			int N = 0;
			int Side = 0;

			bFlg = false;
			N = pPolygone.PointCount;
			TmpPoly = new Polyline();
			createBasePointsReturn = new Polygon();

			if (lTurnDir > 0)
			{
				for (I = 0; I <= N - 1; I++)
				{
					Side = Functions.SideDef(K1K1.FromPoint, lDepDir + 90.0, pPolygone.get_Point(I));
					if ((Side < 0))
					{
						if (bFlg)
						{
							createBasePointsReturn.AddPoint(pPolygone.get_Point(I));
						}
						else
						{
							TmpPoly.AddPoint(pPolygone.get_Point(I));
						}
					}
					else if (!(bFlg))
					{
						bFlg = true;
						createBasePointsReturn.AddPoint(K1K1.FromPoint);
						createBasePointsReturn.AddPoint(K1K1.ToPoint);
					}
				}
			}
			else
			{
				for (I = N - 1; I >= 0; I += -1)
				{
					Side = Functions.SideDef(K1K1.FromPoint, lDepDir + 90.0, pPolygone.get_Point(I));
					if ((Side < 0))
					{
						if (bFlg)
						{
							createBasePointsReturn.AddPoint(pPolygone.get_Point(I));
						}
						else
						{
							TmpPoly.AddPoint(pPolygone.get_Point(I));
						}
					}
					else if (!(bFlg))
					{
						bFlg = true;
						createBasePointsReturn.AddPoint(K1K1.ToPoint);
						createBasePointsReturn.AddPoint(K1K1.FromPoint);
					}
				}
			}

			createBasePointsReturn.AddPointCollection(TmpPoly);
			return createBasePointsReturn;
		}

		public static IPolygon PolygonDifference(IPolygon Source, IPolygon Subtractor)
		{
			IPolygon polygonDifferenceReturn = null;
			ITopologicalOperator2 pTopo = null;

			pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(Source));
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();

			pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(Source));
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();

			try
			{
				polygonDifferenceReturn = pTopo.Difference(Subtractor as IGeometry) as IPolygon;
				pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(polygonDifferenceReturn));
				pTopo.IsKnownSimple_2 = false;
				pTopo.Simplify();
				return polygonDifferenceReturn;
			}
			catch
			{
				polygonDifferenceReturn = Subtractor;
			}

			return polygonDifferenceReturn;
		}

		public static IPolygon PolygonIntersection(IPointCollection pPoly1, IPointCollection pPoly2)
		{
			return PolygonIntersection(pPoly1 as IPolygon, pPoly2 as IPolygon);
		}

		public static IPolygon PolygonIntersection(IPolygon pPoly1, IPolygon pPoly2)
		{
			IPolygon polygonIntersectionReturn = null;
			ITopologicalOperator2 pTopo = null;
			Polygon pTmpPoly0 = null;
			Polygon pTmpPoly1 = null;

			pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(pPoly2));
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();

			pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(pPoly1));
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();

			try
			{
				polygonIntersectionReturn = pTopo.Intersect(pPoly2 as IGeometry, esriGeometryDimension.esriGeometry2Dimension) as IPolygon;
				pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(polygonIntersectionReturn));
				pTopo.IsKnownSimple_2 = false;
				pTopo.Simplify();
			}
			catch
			{
				try
				{
					pTmpPoly0 = pTopo.Union(pPoly2 as IGeometry) as Polygon;
					pTmpPoly1 = pTopo.SymmetricDifference(pPoly2 as IGeometry) as Polygon;

					pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(pTmpPoly1));
					pTopo.IsKnownSimple_2 = false;
					pTopo.Simplify();

					pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(pTmpPoly0));
					pTopo.IsKnownSimple_2 = false;
					pTopo.Simplify();
					polygonIntersectionReturn = pTopo.Difference(pTmpPoly1 as IGeometry) as IPolygon;

					pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(polygonIntersectionReturn));
					pTopo.IsKnownSimple_2 = false;
					pTopo.Simplify();
				}
				catch
				{
					polygonIntersectionReturn = pPoly2;
				}
			}
			return polygonIntersectionReturn;
		}

		public static bool PriorPostFixTolerance(IPointCollection pPolygon, IPoint pPtPrj, double fDir, out double PriorDist, out double PostDist)
		{
			int I, N;
			Double fDist, fMinDist, fMaxDist;

			IPoint pCurrPt;
			IPolyline pCutterPolyline;
			IPointCollection pIntersection;
			ITopologicalOperator2 pTopological;
			IPointCollection pPoints = pPolygon as IPointCollection;

			pCutterPolyline = new Polyline() as IPolyline;
			pCutterPolyline.FromPoint = PointAlongPlane(pPtPrj, fDir, 1000000.0);
			pCutterPolyline.ToPoint = PointAlongPlane(pPtPrj, fDir + 180.0, 1000000.0);
			pTopological = pPolygon as ITopologicalOperator2;

			PriorDist = -1.0;
			PostDist = -1.0;

			try
			{
				pIntersection = pTopological.Intersect(pCutterPolyline, esriGeometryDimension.esriGeometry0Dimension) as IPointCollection;
			}
			catch
			{
				return false;
			}

			N = pIntersection.PointCount;
			if (N == 0) return false;

			pCurrPt = pIntersection.Point[0];
			fDist = ReturnDistanceInMeters(pPtPrj, pCurrPt) * SideDef(pPtPrj, fDir + 90.0, pCurrPt);
			fMinDist = fDist;
			fMaxDist = fDist;

			for (I = 1; I < N; I++)
			{
				pCurrPt = pIntersection.Point[I];
				fDist = ReturnDistanceInMeters(pPtPrj, pCurrPt) * SideDef(pPtPrj, fDir + 90.0, pCurrPt);
				if (fDist < fMinDist) fMinDist = fDist;
				if (fDist > fMaxDist) fMaxDist = fDist;
			}

			PriorDist = fMinDist;
			PostDist = fMaxDist;

			return true;
		}

#endif

		public static string DegToStr(double value)
		{
			string result = value.ToString(); //System.Convert.ToString(value);

			int l = result.Length;

			for (int i = 1; i <= 3 - l; i++)
				result = "0" + result;

			return result + "°";
		}

		public static void shall_SortfSort(ObstacleType[] obsArray)
		{
			int LastRow = obsArray.GetUpperBound(0);

			if (LastRow < 0)
				return;

			int FirstRow = obsArray.GetLowerBound(0);
			int NumRows = LastRow - FirstRow + 1;
			int GapSize = 0;

			do
				GapSize = GapSize * 3 + 1;
			while (GapSize <= NumRows);

			do
			{
				GapSize = GapSize / 3;
				for (int I = (GapSize + FirstRow); I <= LastRow; I++)
				{
					int CurPos = I;
					ObstacleType TempVal = obsArray[I];
					while (obsArray[CurPos - GapSize].fSort > TempVal.fSort)
					{
						obsArray[CurPos] = obsArray[CurPos - GapSize];
						CurPos = CurPos - GapSize;
						if (CurPos - GapSize < FirstRow)
							break;
					}
					obsArray[CurPos] = TempVal;
				}
			}
			while (GapSize > 1);
		}

		public static void shall_SortfSortD(ObstacleType[] obsArray)
		{
			int LastRow = obsArray.GetUpperBound(0);
			if (LastRow < 0)
				return;

			int FirstRow = obsArray.GetLowerBound(0);
			int NumRows = LastRow - FirstRow + 1;
			int GapSize = 0;

			do
				GapSize = GapSize * 3 + 1;
			while (GapSize <= NumRows);

			do
			{
				GapSize = GapSize / 3;
				for (int I = GapSize + FirstRow; I <= LastRow; I++)
				{
					int CurPos = I;
					ObstacleType TempVal = obsArray[I];
					while (obsArray[CurPos - GapSize].fSort < TempVal.fSort)
					{
						obsArray[CurPos] = obsArray[CurPos - GapSize];
						CurPos = CurPos - GapSize;
						if (CurPos - GapSize < FirstRow)
							break;
					}
					obsArray[CurPos] = TempVal;
				}
			}
			while (GapSize > 1);
		}

		public static void shall_SortsSort(ObstacleType[] obsArray)
		{
			int LastRow = obsArray.GetUpperBound(0);

			if (LastRow < 0)
				return;

			int FirstRow = obsArray.GetLowerBound(0);
			int NumRows = LastRow - FirstRow + 1;
			int GapSize = 0;

			do
				GapSize = GapSize * 3 + 1;
			while (GapSize <= NumRows);

			do
			{
				GapSize = GapSize / 3;
				for (int I = (GapSize + FirstRow); I <= LastRow; I++)
				{
					int CurPos = I;
					ObstacleType TempVal = obsArray[I];
					while (String.Compare(obsArray[CurPos - GapSize].sSort, TempVal.sSort) > 0)
					{
						obsArray[CurPos] = obsArray[CurPos - GapSize];
						CurPos = CurPos - GapSize;
						if (CurPos - GapSize < FirstRow)
							break;
					}
					obsArray[CurPos] = TempVal;
				}
			}
			while (GapSize > 1);
		}

		public static void shall_SortsSortD(ObstacleType[] obsArray)
		{
			int LastRow = obsArray.GetUpperBound(0);

			if (LastRow < 0)
				return;

			int FirstRow = obsArray.GetLowerBound(0);
			int NumRows = LastRow - FirstRow + 1;
			int GapSize = 0;

			do
				GapSize = GapSize * 3 + 1;
			while (GapSize <= NumRows);

			do
			{
				GapSize = GapSize / 3;
				for (int I = (GapSize + FirstRow); I <= LastRow; I++)
				{
					int CurPos = I;
					ObstacleType TempVal = obsArray[I];

					while (String.Compare(obsArray[CurPos - GapSize].sSort, TempVal.sSort) < 0)
					{
						obsArray[CurPos] = obsArray[CurPos - GapSize];
						CurPos = CurPos - GapSize;
						if (CurPos - GapSize < FirstRow)
							break;
					}
					obsArray[CurPos] = TempVal;
				}
			}
			while (GapSize > 1);
		}

		public static double CalcTA_Hpenet(ObstacleType[] PtTrnList, double hTurn, double TA_PDG, double MaxDist, out int index, double iniH = 0)
		{
			index = -1;
			double result = iniH;
			int n = PtTrnList.GetUpperBound(0);

			if (n < 0)
				return result;

			for (int i = 0; i <= n; i++)
			{
				PtTrnList[i].hPent = PtTrnList[i].Height + PtTrnList[i].MOC - hTurn - PtTrnList[i].X * TA_PDG;

				if ((PtTrnList[i].X <= MaxDist) && (result < PtTrnList[i].hPent))
				{
					result = PtTrnList[i].hPent;
					index = i;
				}
			}
			return result;
		}

		public static double CalcTA_PDG(ObstacleType[] PtTrnList, double hTurn, double TA_PDG, double MaxDist, out int index)
		{
			index = -1;
			int n = PtTrnList.Length;

			if (n == 0)
				return TA_PDG;

			double result = 0.0;

			for (int i = 0; i < n; i++)
			{
				if ((PtTrnList[i].hPent > 0.0) && (PtTrnList[i].X <= MaxDist))
				{
					double PDGi = PtTrnList[i].hPent / PtTrnList[i].X;
					if (result < PDGi)
					{
						index = i;
						result = PDGi;
					}
				}
			}

			return System.Math.Round(result + TA_PDG + 0.0004999, 3);
		}

		public static void TextBoxFloat(ref char KeyChar, string BoxText)
		{
			if (KeyChar < 32)
				return;

			char DecSep = Convert.ToString(1.1).ToCharArray()[1];

			if ((KeyChar < '0' || KeyChar > '9') && KeyChar != DecSep)
				KeyChar = '\0';
			else if (KeyChar == DecSep && BoxText.Contains(DecSep.ToString()))
				KeyChar = '\0';
		}

		public static void TextBoxInteger(ref char KeyChar)
		{
			if (KeyChar < ' ')
				return;
			if (KeyChar < '0' || KeyChar > '9')
				KeyChar = '\0';
		}

		/*
		public static void SetThreadLocaleByConfig()
		{
			//GlobalVars.LangCode = Functions.RegRead<Int32>(Microsoft.Win32.Registry.CurrentUser, GlobalVars.PandaRegKey, "LanguageCode", GlobalVars.NeutralLangCode);
			//NativeMethods.SetThreadLocale(GlobalVars.LangCode);
			NativeMethods.SetThreadLocale(GlobalVars.LangCode);
		}
		*/
		public static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			System.Windows.Forms.MessageBox.Show(e.Exception.Message, "Error",
					System.Windows.Forms.MessageBoxButtons.OK,
					System.Windows.Forms.MessageBoxIcon.Error);
		}

		private static bool _errorHandled = false;

		public static void HandleThreadException()
		{
			if (_errorHandled)
				return;
			_errorHandled = true;
			System.Windows.Forms.Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Functions.Application_ThreadException);
		}


		private const double OverlapDist = 0.5;

		public const double PBNInternalTriggerDistance = 28000.0;
		public const double PBNTerminalTriggerDistance = 56000.0;
		public const double SBASTriggerDistance = 46000.0;
		//public const double GNSSTriggerDistance = 46000.0;

		static void JoinSegments(Leg leg, Ring LegRing, ADHPType ARP, Boolean IsOuter, Boolean IsPrimary)
		{
			int JointFlag;

			Double EntryDir, OutDir, BaseDir0, BaseDir1, ASW_0F, ASW_0C, ASW_1C,
				SplayAngle15, LegLenght, DivergenceAngle30, SpiralDivergenceAngle,
				fDistTreshold, DistToEndFIX, fSide, fTmp, DistToCenter, TransitionDist,
				Dist0, Dist1, dPhi1, Delta, dPhi2, Direction;//, DistToLPT, Dist56000;

			TurnDirection TurnDirO, TurnDir;

			Point ptBase0, ptBase1, ptTmp, ptCurr, ptCenter, ptInter;
			WayPoint StartFIX, EndFIX;

			//String txt;

			StartFIX = leg.StartFIX;
			EndFIX = leg.EndFIX;

			if (IsOuter)
				JointFlag = 1;
			else
				JointFlag = 2;

			//bool GlDRAWFLG = IsOuter && IsPrimary && (StartFIX.ConstMode == FAF_);

			EntryDir = StartFIX.EntryDirection;
			OutDir = StartFIX.OutDirection;

			TurnDir = ARANMath.SideToTurn(ARANMath.SideDef(StartFIX.PrjPt, EntryDir, EndFIX.PrjPt));
			TurnDirO = TurnDir;

			if (TurnDir == TurnDirection.NONE)
				TurnDir = StartFIX.EffectiveTurnDirection;

			//	fSide = (1.0 - 2.0 * ((Byte)IsOuter)) * ((int)TurnDir);

			fSide = IsOuter ? -((int)TurnDir) : ((int)TurnDir);

			SpiralDivergenceAngle = GlobalVars.constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;

			if (IsPrimary)
			{
				DivergenceAngle30 = Math.Atan(0.5 * Math.Tan(SpiralDivergenceAngle));
				fTmp = GlobalVars.constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
				SplayAngle15 = Math.Atan(0.5 * Math.Tan(fTmp));
			}
			else
			{
				SplayAngle15 = GlobalVars.constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
				DivergenceAngle30 = SpiralDivergenceAngle;//GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;
			}

			//	SpiralDivergenceAngle := DivergenceAngle30;

			ptTmp = new Point();
			ptCurr = new Point();
			ptCurr.Assign(LegRing[LegRing.Count - 1]);

			//if (GlDRAWFLG )
			//	GUI.DrawPointWithText(ptCurr, 0, 'ptCurr-0');

			ASW_0C = IsPrimary ? 0.5 * StartFIX.SemiWidth : StartFIX.SemiWidth;
			ASW_1C = IsPrimary ? 0.5 * EndFIX.SemiWidth : EndFIX.SemiWidth;

			//	if IsPrimary then	txt := ' Pr'
			//	else				txt := '     Se';

			//	if IsOuter then		txt := txt + ' Out'
			//	else				txt := txt + '      Inn';

			//=================================================================================
			//	if (RadToDeg(EndFIX.TurnAngle) <= 10)or((EndFIX.ConstMode <> FAF_)and (RadToDeg(EndFIX.TurnAngle) <= 30)) then
			//	if (EndFIX.ConstMode = FAF_)or(RadToDeg(EndFIX.TurnAngle) <= 30) then

			if (ARANMath.RadToDeg(EndFIX.TurnAngle) <= 10 ||
				//(EndFIX.FlyMode == eFlyMode.FlyBy && EndFIX.ConstMode != eFIXRole.FAF_ && ARANMath.RadToDeg(EndFIX.TurnAngle) <= 30))
				(EndFIX.FlyMode == eFlyMode.Flyby && EndFIX.Role != eFIXRole.FAF_ && ARANMath.RadToDeg(EndFIX.TurnAngle) <= 30))
			{
				if (EndFIX.TurnDirection == TurnDirection.NONE || (int)EndFIX.TurnDirection != Math.Round(fSide))
					DistToEndFIX = -OverlapDist;
				else
				{
					ptTmp = ARANFunctions.LocalToPrj(EndFIX.PrjPt, EndFIX.OutDirection - 0.5 * ARANMath.C_PI * fSide, ASW_1C, 0);
					//if Counter = 5 then

					//if GlDRAWFLG then
					//	GUI.DrawPointWithText(ptTmp, 0, 'ptTmp' + txt);
					DistToEndFIX = ARANFunctions.PointToLineDistance(ptTmp, EndFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI) - OverlapDist;
				}
			}
			else if ((int)EndFIX.TurnDirection == Math.Round(fSide))
				DistToEndFIX = EndFIX.EPT - OverlapDist;
			else
				DistToEndFIX = EndFIX.FlyMode == eFlyMode.Flyby ? EndFIX.LPT - OverlapDist : OverlapDist;

			ptCenter = ARANFunctions.LocalToPrj(EndFIX.PrjPt, OutDir + ARANMath.C_PI, DistToEndFIX, 0);

			//if GlDRAWFLG then
			//if GlDRAWFLG then

			//	txt := 'ptCenter-'+ IntToStr(Counter) + txt;

			//if Counter = 5 then
			//if Test then
			//	GUI.DrawPointWithText(ptCenter, 255, txt);
			//=================================================================================

			DistToCenter = ARANFunctions.PointToLineDistance(ptCurr, ptCenter, OutDir + 0.5 * ARANMath.C_PI);
			if (DistToCenter > ARANMath.EpsilonDistance)
			{
				LegLenght = ARANMath.Hypot(EndFIX.PrjPt.X - StartFIX.PrjPt.X, EndFIX.PrjPt.Y - StartFIX.PrjPt.Y);

				//if (StartFIX.ConstMode == eFIXRole.FAF_ && EndFIX.ConstMode == eFIXRole.MAPt_)
				if (StartFIX.Role == eFIXRole.FAF_ && EndFIX.Role == eFIXRole.MAPt_)
				{
					dPhi1 = Math.Atan2(ASW_0C - ASW_1C, LegLenght);
					if (dPhi1 > DivergenceAngle30) dPhi1 = DivergenceAngle30;

					BaseDir0 = OutDir + dPhi1 * fSide;
					ptBase0 = ARANFunctions.LocalToPrj(EndFIX.PrjPt, OutDir - 0.5 * ARANMath.C_PI * fSide, ASW_0C, 0);

					BaseDir1 = OutDir + dPhi1 * fSide;
					ptBase1 = ARANFunctions.LocalToPrj(EndFIX.PrjPt, OutDir - 0.5 * ARANMath.C_PI * fSide, ASW_1C, 0);
				}
				else
				{
					BaseDir0 = OutDir;
					ptBase0 = ARANFunctions.LocalToPrj(ptCenter, OutDir - 0.5 * ARANMath.C_PI * fSide, ASW_0C, 0);

					BaseDir1 = OutDir;
					ptBase1 = ARANFunctions.LocalToPrj(ptCenter, OutDir - 0.5 * ARANMath.C_PI * fSide, ASW_1C, 0);
				}

				//if GlDRAWFLG then
				//	GUI.DrawPointWithText(ptCurr, 255, 'ptCur-1');

				//if GlDRAWFLG then
				//	GUI.DrawPointWithText(ptBase1, RGB(0, 0, 255), 'ptBase1');
				//if GlDRAWFLG then
				//	GUI.DrawPointWithText(ptBase0, RGB(0, 0, 255), 'ptBase0');

				TransitionDist = -(LegLenght + 100) * 100;

				//if (EndFIX.SensorType == eSensorType.GNSS && EndFIX.ConstMode == eFIXRole.FAF_)
				if (EndFIX.SensorType == eSensorType.GNSS && EndFIX.Role == eFIXRole.FAF_)
					TransitionDist = 1.5 * (StartFIX.XXT - EndFIX.XXT) / Math.Tan(GlobalVars.constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value);

				fDistTreshold = PBNTerminalTriggerDistance;

				Dist0 = ARANMath.Hypot(ARP.pPtPrj.X - StartFIX.PrjPt.X, ARP.pPtPrj.Y - StartFIX.PrjPt.Y);
				Dist1 = ARANMath.Hypot(EndFIX.PrjPt.X - ARP.pPtPrj.X, EndFIX.PrjPt.Y - ARP.pPtPrj.Y);

				if (Dist0 >= fDistTreshold && Dist1 < fDistTreshold)
				{
					dPhi1 = Math.Atan2(ARP.pPtPrj.Y - EndFIX.PrjPt.Y, ARP.pPtPrj.X - EndFIX.PrjPt.X);
					dPhi2 = Math.Atan2(StartFIX.PrjPt.Y - EndFIX.PrjPt.Y, StartFIX.PrjPt.X - EndFIX.PrjPt.X);
					Direction = dPhi2 - dPhi1;
					Dist0 = Dist1 * Math.Cos(Direction) + Math.Sqrt(ARANMath.Sqr(fDistTreshold) - ARANMath.Sqr(Dist1 * Math.Sin(Direction)));

					if (Dist0 > TransitionDist)
						TransitionDist = Dist0;
				}

				if (TransitionDist > 0 && TransitionDist < DistToEndFIX)
					TransitionDist = DistToEndFIX;

				//		Delta := -fside * PointToLineDistance(ptCur, ptBase0, BaseDir0);
				ASW_0F = -fSide * ARANFunctions.PointToLineDistance(ptCurr, StartFIX.PrjPt, OutDir);	//Abs ????????
				if (IsOuter && (Math.Abs(ASW_0F - ASW_0C) > ARANMath.EpsilonDistance))
				{
					Dist0 = (LegLenght + 100) * 100;
					ptTmp = ARANFunctions.LocalToPrj(ptCenter, OutDir + ARANMath.C_PI, TransitionDist, fSide * ASW_0C);

					//if GlDRAWFLG then
					//	GUI.DrawPointWithText(ptTmp, 255, 'ptTmp');

					if (TransitionDist > DistToCenter)
					{
						if (ASW_0C > ASW_1C)
							Direction = OutDir + DivergenceAngle30 * fSide;
						else
							Direction = OutDir - SplayAngle15 * fSide;

						ptInter = (Point)ARANFunctions.LineLineIntersect(ptTmp, Direction, ptBase1, BaseDir1);

						//if GlDRAWFLG then
						//	GUI.DrawPointWithText(ptInter, 255, 'ptInter-M');

						Dist0 = ARANFunctions.PointToLineDistance(ptInter, ptCenter, OutDir + 0.5 * ARANMath.C_PI);
					}

					//if GlDRAWFLG then
					//	GUI.DrawPointWithText(ptBase0, 255, 'ptBase0-0');

					/*
								if( Dist0 < DistToCenter)
								{
									ptBase0.Assign(ptTmp);
									BaseDir0 = OutDir + DivergenceAngle30 * fSide;
					//if GlDRAWFLG then
					//	GUI.DrawPointWithText(ptBase0, 255, 'ptBase0-1');
								}
					*/
					if (ASW_0F > ASW_0C)
					{
						if (StartFIX.JointFlags != 0 || !IsPrimary)
							Direction = OutDir + SpiralDivergenceAngle * fSide;
						else
							Direction = OutDir + DivergenceAngle30 * fSide;

						//	Direction = OutDir + SpiralDivergenceAngle * fSide
					}
					else
						Direction = OutDir - SplayAngle15 * fSide;

					ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, Direction, ptBase0, BaseDir0);
					DistToCenter = ARANFunctions.PointToLineDistance(ptInter, ptCenter, OutDir + 0.5 * ARANMath.C_PI);

					//if GlDRAWFLG then
					//	GUI.DrawPointWithText(ptInter, 255, 'ptInter-1');

					Dist0 = ARANFunctions.PointToLineDistance(ptInter, ptCurr, OutDir - 0.5 * ARANMath.C_PI);
					if (DistToCenter < 0 || Dist0 < 0)
					{
						ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, Direction, ptCenter, OutDir + 0.5 * ARANMath.C_PI);
						DistToCenter = ARANFunctions.PointToLineDistance(ptInter, ptCenter, OutDir + 0.5 * ARANMath.C_PI);
					}
					else
						EndFIX.JointFlags = EndFIX.JointFlags & (~JointFlag);

					ptCurr.Assign(ptInter);

					//if GlDRAWFLG then
					//	GUI.DrawPointWithText(ptCurr, 255, 'ptCur-2');

					LegRing.Add(ptCurr);
					ASW_0F = -fSide * ARANFunctions.PointToLineDistance(ptCurr, StartFIX.PrjPt, OutDir);
				}

				Delta = -fSide * ARANFunctions.PointToLineDistance(ptCurr, ptBase1, BaseDir1);

				if (Math.Abs(Delta) > ARANMath.EpsilonDistance)
				{
					if (TransitionDist > 0)
					{
						if (TransitionDist < DistToEndFIX) TransitionDist = DistToEndFIX;

						if (TransitionDist < DistToCenter)
						{
							ptCurr = ARANFunctions.LocalToPrj(EndFIX.PrjPt, OutDir - ARANMath.C_PI, TransitionDist, fSide * ASW_0F);
							//if GlDRAWFLG then
							//	GUI.DrawPointWithText(ptCurr, 255, 'ptCur-3');

							LegRing.Add(ptCurr);

							DistToCenter = ARANFunctions.PointToLineDistance(ptCurr, ptCenter, OutDir + 0.5 * ARANMath.C_PI);
						}
					}

					if (DistToCenter > ARANMath.EpsilonDistance)
					{
						if (Delta > ARANMath.EpsilonDistance)
							Direction = OutDir + DivergenceAngle30 * fSide;
						else
							Direction = OutDir - SplayAngle15 * fSide;

						ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, Direction, ptBase1, BaseDir1);
						DistToCenter = ARANFunctions.PointToLineDistance(ptInter, ptCenter, OutDir + 0.5 * ARANMath.C_PI);
						Dist0 = ARANFunctions.PointToLineDistance(ptInter, ptCurr, OutDir - 0.5 * ARANMath.C_PI);

						if (DistToCenter < 0 && Dist0 < 0)			//if(DistToCenter < 0)
						{
							//if GlDRAWFLG then
							//	GUI.DrawPointWithText(ptInter, 255, 'ptInter-2');
							//					if IsPrimary then
							//						Leg.EndFIX.JointFlags := Leg.EndFIX.JointFlags or JointFlag;

							ptInter = (Point)ARANFunctions.LineLineIntersect(ptCurr, Direction, ptCenter, OutDir + 0.5 * ARANMath.C_PI);
							DistToCenter = ARANFunctions.PointToLineDistance(ptInter, ptCenter, OutDir + 0.5 * ARANMath.C_PI);
						}

						ptCurr.Assign(ptInter);

						//if GlDRAWFLG then
						//	GUI.DrawPointWithText(ptCurr, 255, 'ptCur-4');

						LegRing.Add(ptCurr);
					}
				}

				if (DistToCenter > ARANMath.EpsilonDistance)
				{
					ptCurr = ARANFunctions.LocalToPrj(ptCenter, OutDir, 0, -ASW_1C * fSide);

					//if GlDRAWFLG then
					//	GUI.DrawPointWithText(ptCurr, 255, 'ptCur-5');

					LegRing.Add(ptCurr);
				}
			}

			//GlDRAWFLG := False;
		}

		static Ring CreateOuterTurnAreaLT(Leg PrevLeg, Leg leg, ADHPType ARP, Boolean IsPrimary)
		{
			Double ptDir, fTmp, Dist0, Dist1, ptDist, EntryDir, OutDir, SplayAngle15,
				SpiralDivergenceAngle, DivergenceAngle30, fSide, ASW_OUT0F, ASW_OUT0C;

			TurnDirection TurnDir;
			Point ptCnt, ptTmp, ptFrom, ptTo;
			Geometry Geom0, Geom1;
			Ring tmpRing;
			WayPoint StartFIX, EndFIX;

			StartFIX = leg.StartFIX;
			EndFIX = leg.EndFIX;

			EntryDir = StartFIX.EntryDirection;
			OutDir = StartFIX.OutDirection;

			//	SplayAngle15 := GPANSOPSConstants.Constant[arafTrn_OSplay].Value;
			//	DivergenceAngle30 := GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;

			SpiralDivergenceAngle = GlobalVars.constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;

			if (IsPrimary)
			{
				//		fTmp := GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;
				DivergenceAngle30 = Math.Atan(0.5 * Math.Tan(SpiralDivergenceAngle));

				fTmp = GlobalVars.constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
				SplayAngle15 = Math.Atan(0.5 * Math.Tan(fTmp));
			}
			else
			{
				DivergenceAngle30 = SpiralDivergenceAngle;//GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;
				SplayAngle15 = GlobalVars.constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
			}

			TurnDir = ARANMath.SideToTurn(ARANMath.SideDef(StartFIX.PrjPt, EntryDir, EndFIX.PrjPt));
			//	if TurnDir = SideOn then TurnDir := SideLeft;
			if (TurnDir == TurnDirection.NONE)
				TurnDir = StartFIX.EffectiveTurnDirection;

			//	if TurnDir = SideOn then TurnDir := SideRight;

			fSide = (int)TurnDir;

			ptFrom = null;

			if (TurnDir == TurnDirection.CCW)
			{
				if (IsPrimary)
				{
					ASW_OUT0C = 0.5 * StartFIX.SemiWidth;
					ASW_OUT0F = StartFIX.ASW_2_R;
					if (PrevLeg != null)
					{

						//				ptFrom = ARANFunctions.RingVectorIntersect (PrevLeg.PrimaryArea.Ring[0], StartFIX.PrjPt, EntryDir - 0.5*PI, out fTmp);
						ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.PrimaryArea[0].ExteriorRing, StartFIX.PrjPt, OutDir - 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_OUT0F = fTmp;
					}
				}
				else
				{
					ASW_OUT0C = StartFIX.SemiWidth;
					ASW_OUT0F = StartFIX.ASW_R;
					if (PrevLeg != null)
					{
						//ptFrom = ARANFunctions.RingVectorIntersect (PrevLeg.FullArea.Ring[0], StartFIX.PrjPt, EntryDir - 0.5*PI,  out fTmp);
						ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, StartFIX.PrjPt, OutDir - 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_OUT0F = fTmp;
					}
				}
			}
			else
			{
				if (IsPrimary)
				{
					ASW_OUT0C = 0.5 * StartFIX.SemiWidth;
					ASW_OUT0F = StartFIX.ASW_2_L;
					if (PrevLeg != null)
					{
						//ptFrom = ARANFunctions.RingVectorIntersect (PrevLeg.PrimaryArea.Ring[0], StartFIX.PrjPt, EntryDir + 0.5*PI, fTmp);
						ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.PrimaryArea[0].ExteriorRing, StartFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_OUT0F = fTmp;
					}
				}
				else
				{
					ASW_OUT0C = StartFIX.SemiWidth;
					ASW_OUT0F = StartFIX.ASW_L;
					if (PrevLeg != null)
					{
						//ptFrom = ARANFunctions.RingVectorIntersect (PrevLeg.FullArea.Ring[0], StartFIX.PrjPt, EntryDir + 0.5*PI, fTmp);
						ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, StartFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_OUT0F = fTmp;
					}
				}
			}

			//if(not IsPrimary) and Assigned(ptFrom)and Assigned(PrevLeg) then
			//if GlDRAWFLG then
			//	GUI.DrawPointWithText(ptFrom, 0, 'ptFrom-O0');

			Ring result = new Ring();

			ptTmp = new Point();

			if (ptFrom == null)
			{
				ptTmp = ARANFunctions.LocalToPrj(StartFIX.PrjPt, EntryDir - ARANMath.C_PI, StartFIX.EPT + 10.0, -ASW_OUT0F * fSide);
				result.Add(ptTmp);
				ptFrom = ARANFunctions.LocalToPrj(StartFIX.PrjPt, EntryDir + 0.5 * ARANMath.C_PI * fSide, ASW_OUT0F, 0);
			}

			//if(not IsPrimary) and Assigned(ptFrom)and Assigned(PrevLeg) then
			//if GlDRAWFLG then
			//	GUI.DrawPointWithText(ptFrom, 0, 'ptFrom-O1');

			result.Add(ptFrom);

			ptTo = ARANFunctions.LocalToPrj(StartFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI * fSide, ASW_OUT0C, 0);

			ptDist = ARANMath.Hypot(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);
			ptDir = Math.Atan2(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);

			fTmp = ARANMath.Modulus((OutDir - ptDir) * fSide, ARANMath.C_2xPI);
			if (fTmp > ARANMath.C_PI)
				fTmp = fTmp - ARANMath.C_2xPI;

			if (ptDist > 1.0 && fTmp > -SplayAngle15 && fTmp < DivergenceAngle30)
			{
				ptTmp = ARANFunctions.LocalToPrj(ptFrom, ptDir, 0.5 * ptDist, 0);

				Geom0 = ARANFunctions.LineLineIntersect(ptTmp, ptDir + 0.5 * ARANMath.C_PI, ptFrom, EntryDir + 0.5 * ARANMath.C_PI);
				Geom1 = ARANFunctions.LineLineIntersect(ptTmp, ptDir + 0.5 * ARANMath.C_PI, ptTo, OutDir + 0.5 * ARANMath.C_PI);
				ptCnt = null;

				if (Geom0.Type == GeometryType.Point)
				{
					ptCnt = (Point)Geom0;
					if (Geom1.Type == GeometryType.Point)
					{
						Dist0 = ARANMath.Hypot(ptFrom.Y - ptCnt.Y, ptFrom.X - ptCnt.X);
						Dist1 = ARANMath.Hypot(ptFrom.Y - ((Point)Geom1).Y, ptFrom.X - ((Point)Geom1).X);
						if (ASW_OUT0F > ASW_OUT0C && Dist1 < Dist0)
							ptCnt.Assign(Geom1);
					}
				}
				else if (Geom1.Type == GeometryType.Point)
					ptCnt = (Point)Geom1;

				if (ptCnt != null)
				{
					tmpRing = ARANFunctions.CreateArcPrj(ptCnt, ptFrom, ptTo, TurnDir);
					result.AddMultiPoint(tmpRing);
				}

			}
			//=============================================================================
			JoinSegments(leg, result, ARP, true, IsPrimary);
			return result;
		}

		static Ring CreateInnerTurnAreaLT(Leg PrevLeg, Leg leg, ADHPType ARP, Boolean IsPrimary)
		{
			Double OutDir, EntryDir, TurnAng, SplayAngle15, DivergenceAngle30,
			SpiralDivergenceAngle, fSide, fTmp, ptDist, ptDir, ASW_IN0F, ASW_IN0C;

			TurnDirection TurnDir;
			WayPoint StartFIX, EndFIX;

			Point ptFrom, ptTo, ptTmp;

			//GlDRAWFLG := Counter mod 4 = 1;

			StartFIX = leg.StartFIX;
			EndFIX = leg.EndFIX;

			EntryDir = StartFIX.EntryDirection;
			OutDir = StartFIX.OutDirection;

			TurnDir = ARANMath.SideToTurn(ARANMath.SideDef(StartFIX.PrjPt, EntryDir, EndFIX.PrjPt));
			if (TurnDir == TurnDirection.NONE)
				TurnDir = StartFIX.EffectiveTurnDirection;
			// TurnDir = SideLeft;	//	if TurnDir = SideOn then TurnDir := SideRight;

			fSide = (int)TurnDir;
			TurnAng = ARANMath.Modulus((EntryDir - OutDir) * fSide, ARANMath.C_PI);

			//	DivergenceAngle30 := GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;
			//	SplayAngle15 := GPANSOPSConstants.Constant[arafTrn_OSplay].Value;

			SpiralDivergenceAngle = GlobalVars.constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;

			if (IsPrimary)
			{
				//		fTmp := GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;
				DivergenceAngle30 = Math.Atan(0.5 * Math.Tan(SpiralDivergenceAngle));

				fTmp = GlobalVars.constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
				SplayAngle15 = Math.Atan(0.5 * Math.Tan(fTmp));
			}
			else
			{
				SplayAngle15 = GlobalVars.constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
				DivergenceAngle30 = SpiralDivergenceAngle;//GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;
			}

			ptFrom = null;

			if (TurnDir == TurnDirection.CCW)
			{
				if (IsPrimary)
				{
					ASW_IN0C = 0.5 * StartFIX.SemiWidth;
					ASW_IN0F = StartFIX.ASW_2_R;
					if (PrevLeg != null)
					{
						//				ptFrom = ARANFunctions.RingVectorIntersect (PrevLeg.PrimaryArea.ExteriorRing, StartFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI, fTmp);
						ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.PrimaryArea[0].ExteriorRing, StartFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_IN0F = fTmp;
					}
				}
				else
				{
					ASW_IN0C = StartFIX.SemiWidth;
					ASW_IN0F = StartFIX.ASW_R;
					if (PrevLeg != null)
					{
						//				ptFrom = ARANFunctions.RingVectorIntersect (PrevLeg.FullArea.ExteriorRing, StartFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI, fTmp);
						ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, StartFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_IN0F = fTmp;
					}
				}
			}
			else
			{
				if (IsPrimary)
				{
					ASW_IN0C = 0.5 * StartFIX.SemiWidth;
					ASW_IN0F = StartFIX.ASW_2_L;
					if (PrevLeg != null)
					{
						//				ptFrom = ARANFunctions.RingVectorIntersect (PrevLeg.PrimaryArea.ExteriorRing, StartFIX.PrjPt, OutDir - 0.5 * ARANMath.C_PI, fTmp);
						ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.PrimaryArea[0].ExteriorRing, StartFIX.PrjPt, OutDir - 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_IN0F = fTmp;
					}
				}
				else
				{
					ASW_IN0C = StartFIX.SemiWidth;
					ASW_IN0F = StartFIX.ASW_L;
					if (PrevLeg != null)
					{
						//				ptFrom = ARANFunctions.RingVectorIntersect (PrevLeg.FullArea.ExteriorRing, StartFIX.PrjPt, OutDir - 0.5 * ARANMath.C_PI, fTmp);
						ptFrom = ARANFunctions.FindRingLatestPoint(PrevLeg.FullArea[0].ExteriorRing, StartFIX.PrjPt, OutDir - 0.5 * ARANMath.C_PI, OutDir, out fTmp);
						if (ptFrom != null)
							ASW_IN0F = fTmp;
					}
				}
			}

			//if GlDRAWFLG and Assigned(ptFrom) then
			//	GUI.DrawPointWithText(ptFrom, 0, 'ptFrom-I0');

			Ring result = new Ring();
			ptTmp = new Point();

			if (ptFrom == null)
			{
				ptTmp = ARANFunctions.LocalToPrj(StartFIX.PrjPt, EntryDir - ARANMath.C_PI, StartFIX.EPT + 10.0, ASW_IN0F * fSide);
				result.Add(ptTmp);

				ptFrom = ARANFunctions.LocalToPrj(StartFIX.PrjPt, OutDir - 0.5 * ARANMath.C_PI * fSide, ASW_IN0F, 0);
			}

			//if GlDRAWFLG then
			//	GUI.DrawPointWithText(ptFrom, 0, 'ptFrom-I1');

			result.Add(ptFrom);

			fTmp = ASW_IN0C / Math.Cos(TurnAng);
			ptTo = ARANFunctions.LocalToPrj(StartFIX.PrjPt, EntryDir - 0.5 * ARANMath.C_PI * fSide, fTmp, 0);//ASW_IN0C

			ptDist = ARANMath.Hypot(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);
			ptDir = Math.Atan2(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);

			fTmp = ARANMath.Modulus((ptDir - OutDir) * fSide, 2 * ARANMath.C_PI);
			if (fTmp > ARANMath.C_PI)
				fTmp = fTmp - 2 * ARANMath.C_PI;

			if (ptDist > 1.0 && fTmp > -SplayAngle15 && fTmp < DivergenceAngle30)
				result.Add(ptTo);
			/*
			if IsPrimary then
			begin
				GUI.DrawPointWithText(ptFrom, 255, 'ptFrom');
				GUI.DrawPointWithText(ptTo, 255, 'ptTo');
				GUI.DrawPointWithText(PtTmp, 0, 'ptTmp');
			end;
			*/
			JoinSegments(leg, result, ARP, false, IsPrimary);
			return result;
		}

		static Ring CreateOuterTurnArea(Leg leg, Double WSpeed, ADHPType ARP, Boolean IsPrimary)
		{
			int i, n;
			eFlyMode FlyMode;

			Double Rv, coef, R, K, dAlpha, AztEnd1, AztEnd2,
				SpAngle, fTmp, OutDir, EntryDir, SpStartDir, SpStartRad, SpTurnAng, SpFromAngle, SpToAngle,
				dPhi1, dPhi2, SplayAngle, CurWidth, CurDist, Dist0, Dist1, MaxDist, TransitionDist,
				Dist3700, Dist56000, LPTYDist, fDistTreshold, ptInterDist, TurnAng, fSide, TurnR, dRad,
				SpAbeamDist, SplayAngle15, CurrY, PrevX, PrevY, BulgeAngle, BaseDir, Delta, DivergenceAngle30,
				SpiralDivergenceAngle, ASW_OUT0C, ASW_OUT0F, ASW_OUTMax, ASW_OUT1;

			TurnDirection TurnDir;

			Point OuterBasePoint, InnerBasePoint, ptTmp, ptInter, ptBase,
				ptCut, ptCnt, ptCur, ptCur1;

			Boolean bFlag, IsMAPt, HaveSecondSP;
			WayPoint StartFIX, EndFIX;

			//Polygon polygon;

			StartFIX = leg.StartFIX;
			EndFIX = leg.EndFIX;

			FlyMode = StartFIX.FlyMode;
			EntryDir = StartFIX.EntryDirection;
			OutDir = StartFIX.OutDirection;

			SpiralDivergenceAngle = GlobalVars.constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;

			if (IsPrimary)
			{
				fTmp = GlobalVars.constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;
				DivergenceAngle30 = Math.Atan(0.5 * Math.Tan(SpiralDivergenceAngle));

				fTmp = GlobalVars.constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
				SplayAngle15 = Math.Atan(0.5 * Math.Tan(fTmp));
			}
			else
			{
				DivergenceAngle30 = SpiralDivergenceAngle;	//GlobalVars.constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;
				SplayAngle15 = GlobalVars.constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
			}

			TurnDir = ARANMath.SideToTurn(ARANMath.SideDef(StartFIX.PrjPt, EntryDir, EndFIX.PrjPt));
			fSide = (int)TurnDir;
			TurnAng = ARANMath.Modulus((EntryDir - OutDir) * fSide, ARANMath.C_PI);

			TurnR = ARANMath.BankToRadius(StartFIX.BankAngle, StartFIX.TAS);

			Rv = 1765.27777777777777777 * Math.Tan(StartFIX.BankAngle) / (ARANMath.C_PI * StartFIX.TAS);
			if (Rv > 3)
				Rv = 3;
			coef = WSpeed / ARANMath.DegToRad(Rv);

			//PtTmp = ARANFunctions.LocalToPrj(StartFIX.PrjPt, EntryDir - ARANMath.C_PI * BYTE(FlyMode = fmFlyBy), StartFIX.LPT, 0);

			ptTmp = ARANFunctions.LocalToPrj(StartFIX.PrjPt, FlyMode == eFlyMode.Flyby ? EntryDir - ARANMath.C_PI : EntryDir, StartFIX.LPT, 0);

			if (TurnDir == TurnDirection.CCW)
			{
				if (IsPrimary)
				{
					dRad = 0;
					ASW_OUT0F = StartFIX.ASW_2_R;
					ASW_OUT0C = 0.5 * StartFIX.SemiWidth;

					ASW_OUT1 = 0.5 * EndFIX.SemiWidth;
				}
				else
				{
					dRad = StartFIX.ASW_R - StartFIX.ASW_2_R;
					ASW_OUT0F = StartFIX.ASW_R;
					ASW_OUT0C = StartFIX.SemiWidth;

					ASW_OUT1 = EndFIX.SemiWidth;
				}
				InnerBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir + 0.5 * ARANMath.C_PI, StartFIX.ASW_2_L, 0);
				OuterBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir - 0.5 * ARANMath.C_PI, StartFIX.ASW_2_R, 0);
			}
			else
			{
				if (IsPrimary)
				{
					dRad = 0;
					ASW_OUT0F = StartFIX.ASW_2_L;
					ASW_OUT0C = 0.5 * StartFIX.SemiWidth;

					ASW_OUT1 = 0.5 * EndFIX.SemiWidth;
				}
				else
				{
					dRad = StartFIX.ASW_L - StartFIX.ASW_2_L;
					ASW_OUT0F = StartFIX.ASW_L;
					ASW_OUT0C = StartFIX.SemiWidth;

					ASW_OUT1 = EndFIX.SemiWidth;
				}

				InnerBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir - 0.5 * ARANMath.C_PI, StartFIX.ASW_2_R, 0);
				OuterBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir + 0.5 * ARANMath.C_PI, StartFIX.ASW_2_L, 0);
			}

			TurnR = TurnR + dRad;

			ASW_OUTMax = Math.Max(ASW_OUT0C, ASW_OUT1);

			Ring result = new Ring();

			ptCur = new Point();
			ptCur1 = new Point();

			//MAX DISTANCE =================================================================

			if (EndFIX.TurnDirection == TurnDir)
				LPTYDist = EndFIX.EPT - 0.5;
			else
				LPTYDist = (EndFIX.FlyMode == eFlyMode.Flyby ? EndFIX.LPT : -EndFIX.LPT) - 0.5;

			//	if EndFIX.SensorType = stGNSS then		fDistTreshold := GNSSTriggerDistance
			//	else									fDistTreshold := SBASTriggerDistance + GNSSTriggerDistance;
			fDistTreshold = PBNTerminalTriggerDistance;

			Dist0 = ARANMath.Hypot(ARP.pPtPrj.X - StartFIX.PrjPt.X, ARP.pPtPrj.Y - StartFIX.PrjPt.Y);
			Dist1 = ARANMath.Hypot(ARP.pPtPrj.X - EndFIX.PrjPt.X, ARP.pPtPrj.Y - EndFIX.PrjPt.Y);

			Dist56000 = LPTYDist;
			TransitionDist = LPTYDist;

			if (Dist0 >= fDistTreshold && Dist1 < fDistTreshold)
			{
				dPhi1 = Math.Atan2(ARP.pPtPrj.Y - EndFIX.PrjPt.Y, ARP.pPtPrj.X - EndFIX.PrjPt.X);
				dPhi2 = Math.Atan2(StartFIX.PrjPt.Y - EndFIX.PrjPt.Y, StartFIX.PrjPt.X - EndFIX.PrjPt.X);
				fTmp = dPhi2 - dPhi1;
				Dist56000 = Dist1 * Math.Cos(fTmp) + Math.Sqrt(ARANMath.Sqr(fDistTreshold) - ARANMath.Sqr(Dist1 * Math.Sin(fTmp)));
				TransitionDist = Dist56000;
			}

			//if (StartFIX.ConstMode == eFIXRole.IF_ && EndFIX.ConstMode == eFIXRole.FAF_)
			if (StartFIX.Role == eFIXRole.IF_ && EndFIX.Role == eFIXRole.FAF_)
			{
				Dist3700 = 1.5 * (StartFIX.XXT - EndFIX.XXT) / Math.Tan(GlobalVars.constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value);

				//		Dist3700 := (StartFIX.SemiWidth - EndFIX.SemiWidth)/Tan(GPANSOPSConstants.Constant[arSecAreaCutAngl].Value);
				//GPANSOPSConstants.Constant[rnvImMinDist].Value;
				TransitionDist = Math.Max(TransitionDist, Dist3700);
			}

			//IsMAPt = (StartFIX.ConstMode == eFIXRole.FAF_ && EndFIX.ConstMode == eFIXRole.MAPt_);
			IsMAPt = (StartFIX.Role == eFIXRole.FAF_ && EndFIX.Role == eFIXRole.MAPt_);
			//=============================================================================
			ptCnt = ARANFunctions.LocalToPrj(OuterBasePoint, EntryDir - 0.5 * ARANMath.C_PI * fSide, TurnR - dRad, 0);
			//if GlDRAWFLG then
			//	GUI.DrawPointWithText(ptCnt, 0, 'ptCnt-0');

			SpStartDir = EntryDir + ARANMath.C_PI_2 * fSide;
			SpStartRad = SpStartDir;
			BulgeAngle = Math.Atan2(coef, TurnR) * fSide;

			SpTurnAng = ARANFunctions.SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir + BulgeAngle, OutDir, TurnDir);
			//	SpTurnAng := SpiralTouchAngleOld(TurnR, coef, EntryDir, OutDir, TurnDir);

			Dist0 = TurnR + SpTurnAng * coef;
			ptTmp = ARANFunctions.LocalToPrj(ptCnt, SpStartDir - SpTurnAng * fSide, Dist0, 0);
			SpAbeamDist = fSide * ARANFunctions.PointToLineDistance(ptTmp, StartFIX.PrjPt, OutDir);

			HaveSecondSP = (ARANMath.RadToDeg(TurnAng) >= 105) || ((ARANMath.RadToDeg(TurnAng) >= 60) && (SpAbeamDist > ASW_OUT0C));
			SpFromAngle = 0;

			if (TurnAng > 0.5 * ARANMath.C_PI)
				AztEnd1 = EntryDir - 0.5 * ARANMath.C_PI * fSide;
			else
				AztEnd1 = OutDir;

			if (HaveSecondSP)
				AztEnd2 = EntryDir - 0.5 * ARANMath.C_PI * fSide;
			else
				AztEnd2 = AztEnd1;

			SpToAngle = ARANFunctions.SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir + BulgeAngle, AztEnd1, TurnDir);
			//	SpToAngle = ARANFunctions.SpiralTouchAngleOld(TurnR, coef, EntryDir, AztEnd1, TurnDir);

			if (FlyMode == eFlyMode.Flyby)
				SpTurnAng = ARANFunctions.SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir + BulgeAngle, EntryDir, TurnDir);
			//		SpTurnAng = SpiralTouchAngleOld(TurnR, coef, EntryDir, EntryDir, TurnDir)
			else
				SpTurnAng = SpToAngle;

			n = (int)Math.Round(ARANMath.RadToDeg(SpTurnAng));

			if (n < 1) n = 1;
			else if (n < 5) n = 5;
			else if (n < 10) n = 10;

			dAlpha = SpTurnAng / n;

			for (i = 0; i <= n; i++)
			{
				R = TurnR + i * dAlpha * coef;
				ptCur = ARANFunctions.LocalToPrj(ptCnt, SpStartDir - i * dAlpha * fSide, R, 0);
				result.Add(ptCur);
			}

			if (FlyMode == eFlyMode.Flyby)
			{
				Dist0 = TurnR + SpToAngle * coef;
				ptCur1 = ARANFunctions.LocalToPrj(ptCnt, EntryDir - (SpToAngle - 0.5 * ARANMath.C_PI) * fSide, Dist0, 0);
				ptInter = (Point)ARANFunctions.LineLineIntersect(ptCur, EntryDir, ptCur1, AztEnd1);

				ptCur.Assign(ptCur1);
				result.Add(ptInter);
				//if GlDRAWFLG then
				//	GUI.DrawPointWithText(ptInter, 0, 'ptInter');

				result.Add(ptCur);
				//if GlDRAWFLG then
				//	GUI.DrawPointWithText(ptCur, 0, 'ptCur-0');

			}

			//if (GlDRAWFLG )
			//{
			//    polygon = new Polygon();
			//    polygon.Add(result);
			//    GUI.DrawPolygon(polygon, 255, sfsSolid);
			//}

			ptBase = ARANFunctions.LocalToPrj(EndFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI * fSide, ASW_OUTMax, 0);
			BaseDir = OutDir;

			SpStartDir = SpStartDir - SpToAngle * fSide;
			SpFromAngle = SpToAngle;

			CurDist = ARANFunctions.PointToLineDistance(ptCur, EndFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI);
			CurWidth = ARANFunctions.PointToLineDistance(ptCur, StartFIX.PrjPt, OutDir) * fSide;
			MaxDist = Math.Max(LPTYDist, TransitionDist);

			if (IsMAPt)
			{
				Dist0 = ARANMath.Hypot(EndFIX.PrjPt.X - StartFIX.PrjPt.X, EndFIX.PrjPt.Y - StartFIX.PrjPt.Y);
				dPhi1 = Math.Atan2(ASW_OUT0C - ASW_OUT1, Dist0);
				if (dPhi1 > DivergenceAngle30)
					dPhi1 = DivergenceAngle30;

				SpAngle = OutDir - dPhi1 * fSide;

				ptBase = ARANFunctions.LocalToPrj(StartFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI * fSide, ASW_OUT0C, 0);

				if (CurWidth < ASW_OUTMax)
					SplayAngle = OutDir + SplayAngle15 * fSide;
				else
					SplayAngle = OutDir - DivergenceAngle30 * fSide;

				ptInter = (Point)ARANFunctions.LineLineIntersect(ptCur, SplayAngle, ptBase, SpAngle);
				ptInterDist = ARANFunctions.PointToLineDistance(ptInter, EndFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI);

				MaxDist = Math.Max(MaxDist, ptInterDist);

				BaseDir = SpAngle;
				ASW_OUTMax = fSide * ARANFunctions.PointToLineDistance(ptInter, EndFIX.PrjPt, OutDir);
			}

			//																	fTmp := Modulus((AztEnd2 - AztEnd1) * fSide, 2*PI);
			//	 (HaveSecondSP or ((fTmp > EpsilonRadian) and (fTmp < PI)))then
			if (CurDist - LPTYDist > ARANMath.EpsilonDistance && (HaveSecondSP || (ARANMath.Modulus(AztEnd2 - AztEnd1, ARANMath.C_2xPI) > ARANMath.EpsilonRadian)))
			{
				if (TransitionDist > CurDist)
					ASW_OUTMax = ASW_OUT1;

				if (CurWidth - ASW_OUTMax > ARANMath.EpsilonDistance)
				{
					if (ARANMath.Modulus(AztEnd2 - AztEnd1, ARANMath.C_2xPI) > ARANMath.EpsilonRadian)
					//			if (fTmp > EpsilonRadian) and (fTmp < PI) then
					{
						SpToAngle = ARANFunctions.SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir + BulgeAngle, AztEnd2, TurnDir);
						//				SpToAngle = SpiralTouchAngleOld(TurnR, coef, EntryDir, AztEnd2, TurnDir);

						SpTurnAng = SpToAngle - SpFromAngle;
						if (SpTurnAng >= 0)
						{
							n = (int)Math.Round(ARANMath.RadToDeg(SpTurnAng));
							if (n < 1) n = 1;
							else if (n < 5) n = 5;
							else if (n < 10) n = 10;

							dAlpha = SpTurnAng / n;
							bFlag = false;
							PrevY = 0;

							for (i = 0; i <= n; i++)
							{
								R = TurnR + (SpFromAngle + i * dAlpha) * coef;
								ptCur = ARANFunctions.LocalToPrj(ptCnt, SpStartDir - i * dAlpha * fSide, R, 0);

								CurrY = ARANFunctions.PointToLineDistance(ptCur, ptBase, BaseDir);

								if ((!bFlag) && CurrY * fSide >= 0) bFlag = true;

								if (bFlag && CurrY * fSide <= 0 && i > 0)
								{
									K = -PrevY / (CurrY - PrevY);
									ptCur.X = result[result.Count - 1].X + K * (ptCur.X - result[result.Count - 1].X);
									ptCur.Y = result[result.Count - 1].Y + K * (ptCur.Y - result[result.Count - 1].Y);
									result.Add(ptCur);
									break;
								}
								PrevY = CurrY;
								result.Add(ptCur);
							}
							SpStartDir = SpStartDir - SpTurnAng * fSide;
							SpFromAngle = SpToAngle;
						}
					}

					//if (GlDRAWFLG )
					//{
					//    polygon = new Polygon();
					//    polygon.Add(result);
					//    GUI.DrawPolygon(polygon, 0, sfsForwardDiagonal);
					//}

					if (HaveSecondSP)
					{
						ptCnt = ARANFunctions.LocalToPrj(InnerBasePoint, EntryDir - 0.5 * ARANMath.C_PI * fSide, TurnR - dRad, 0);
						//if GlDRAWFLG then
						//	GUI.DrawPointWithText(ptCnt, 0, 'ptCnt-1');

						R = TurnR + SpFromAngle * coef;
						ptCur1 = ARANFunctions.LocalToPrj(ptCnt, SpStartDir - SpFromAngle * fSide, R, 0);
						//if GlDRAWFLG then
						//	GUI.DrawPointWithText(ptCur1, 0, 'ptCurSp1');

						Delta = -fSide * ARANFunctions.PointToLineDistance(ptCur1, ptBase, BaseDir);

						fTmp = ARANMath.Modulus(EntryDir + 0.5 * ARANMath.C_PI - BaseDir, 2 * ARANMath.C_PI);

						if (Math.Abs(fTmp) < ARANMath.EpsilonRadian || Math.Abs(fTmp - ARANMath.C_2xPI) < ARANMath.EpsilonRadian || Math.Abs(fTmp - ARANMath.C_PI) < ARANMath.EpsilonRadian)
						{ }
						else if (Math.Abs(Delta) > ARANMath.EpsilonDistance)
						{
							ptInter = (Point)ARANFunctions.LineLineIntersect(ptCur, EntryDir + 0.5 * ARANMath.C_PI, ptBase, BaseDir);
							fTmp = ARANFunctions.PointToLineDistance(ptInter, EndFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI);
							Dist0 = ARANFunctions.PointToLineDistance(ptInter, ptCur, OutDir + 0.5 * ARANMath.C_PI);

							//if (GlDRAWFLG )
							//{
							//    GUI.DrawPointWithText(ptCur, 255, "ptCur-2");
							//    if (fTmp > -100000)
							//        GUI.DrawPointWithText(ptInter, 255, "ptInter-0");
							//    GUI.DrawPointWithText(ptBase, 255, "ptBase");
							//}

							if (fTmp > 0 && Dist0 < 0)
							{
								ptCur.Assign(ptInter);
								result.Add(ptCur);
							}

							/*
							if( fTmp < 0 )
							{
								ptInter = (Point)ARANFunctions.LineLineIntersect(ptCur, EntryDir + 0.5 * ARANMath.C_PI, EndFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI);
							if (GlDRAWFLG )
									GUI.DrawPointWithText(ptInter, 255, "ptInter--1");
							if GlDRAWFLG then
									GUI.DrawPointWithText(EndFIX.PrjPt, 255, "EndFIX.PrjPt");
							}

							fTmp = PointToLineDistance(ptInter, ptCur, OutDir + 0.5 * PI);
							if (false )
							//if (fTmp < 0 )
							{
									ptCur.Assign(ptInter);
									result.Add(ptCur);
							}

							if GlDRAWFLG then
								GUI.DrawPointWithText(ptCur, 0, "ptCur-3");
							*/
						}
					}
				}
			}

			//if (GlDRAWFLG )
			//{
			//    polygon = new Polygon();
			//    Polygon.Add(result);
			//    GUI.DrawPolygon(polygon, 0, sfsBackwardDiagonal);
			//}

			CurWidth = ARANFunctions.PointToLineDistance(ptCur, StartFIX.PrjPt, OutDir) * fSide;
			CurDist = ARANFunctions.PointToLineDistance(ptCur, EndFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI);

			if (IsMAPt)
			{
				//Dist0 = Hypot(EndFIX.PrjPt.X - StartFIX.PrjPt.X, EndFIX.PrjPt.Y - StartFIX.PrjPt.Y);
				//dPhi1 = ArcTan2(ASW_OUT0C - ASW_OUT1, Dist0);
				//if dPhi1 > DivergenceAngle30 then dPhi1 := DivergenceAngle30;

				//		SpAngle := OutDir - dPhi1 * fSide;
				//		LocalToPrj(StartFIX.PrjPt, OutDir + 0.5 * PI * fSide, ASW_OUT0C, 0, ptBase);

				//		if (CurWidth < ASW_OUTMax) then
				//			SplayAngle := OutDir + SplayAngle15 * fSide
				//		else
				//			SplayAngle := OutDir - DivergenceAngle30 * fSide;

				//		ptInter := LineLineIntersect(ptCur, SplayAngle, InnerBasePoint, SpAngle).AsPoint;
				//		ptInterDist := PointToLineDistance(ptInter, EndFIX.PrjPt, OutDir + 0.5 * PI);

				//		TransitionDist := Max(TransitionDist, ptInterDist);
				//		if TransitionDist = ptInterDist then
				//		begin
				//			IsMAPt := True;
				//			BaseDir := SpAngle;
				//			ASW_OUTMax := fSide * PointToLineDistance(ptInter, EndFIX.PrjPt, OutDir);
				//		end;
			}

			//=============================================================================
			if (TransitionDist > CurDist)
				ASW_OUTMax = ASW_OUT1;

			if (CurDist - LPTYDist > ARANMath.EpsilonDistance && Math.Abs(CurWidth - ASW_OUTMax) > ARANMath.EpsilonDistance)
			{
				if (CurWidth - ASW_OUTMax > ARANMath.EpsilonDistance)
				{
					SpToAngle = ARANFunctions.SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir + BulgeAngle, OutDir - SpiralDivergenceAngle * fSide, TurnDir);
					//SpToAngle = ARANFunctions.SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir + BulgeAngle, OutDir-DivergenceAngle30 * fSide, TurnDir);

					SpTurnAng = SpToAngle - SpFromAngle;
					if (SpTurnAng >= 0)
					{
						n = (int)Math.Round(ARANMath.RadToDeg(SpTurnAng));
						if (n < 1) n = 1;
						else if (n < 5) n = 5;
						else if (n < 10) n = 10;

						dAlpha = SpTurnAng / n;
						bFlag = false;
						PrevY = 0;

						//if GlDRAWFLG then
						//	GUI.DrawPointWithText(ptBase, 255, 'ptBase');

						for (i = 0; i <= n; i++)
						{
							R = TurnR + (SpFromAngle + i * dAlpha) * coef;
							ptCur = ARANFunctions.LocalToPrj(ptCnt, SpStartDir - i * dAlpha * fSide, R, 0);

							ptTmp = ARANFunctions.PrjToLocal(ptBase, BaseDir, ptCur);
							//					CurrY := PointToLineDistance(ptCur, ptBase, BaseDir);

							if ((!bFlag) && ptTmp.Y * fSide >= 0)
								bFlag = true;

							if (i > 0)
							{
								/*
								if (ptTmp.X > 0 )
								{
									K = -PrevX/(ptTmp.X-PrevX);
									ptCur.X = result[result.Count-1].X + K *(ptCur.X - result[result.Count-1].X);
									ptCur.Y = result[result.Count-1].Y + K *(ptCur.Y - result[result.Count-1].Y);
									result.Add(ptCur);
									break;
								}
								*/
								if (bFlag && ptTmp.Y * fSide <= 0)
								{
									K = -PrevY / (ptTmp.Y - PrevY);
									ptCur.X = result[result.Count - 1].X + K * (ptCur.X - result[result.Count - 1].X);
									ptCur.Y = result[result.Count - 1].Y + K * (ptCur.Y - result[result.Count - 1].Y);
									result.Add(ptCur);
									break;
								}
							}

							if (i == n && IsPrimary)
								leg.EndFIX.JointFlags = leg.EndFIX.JointFlags | 1;

							PrevX = ptTmp.X;
							PrevY = ptTmp.Y;
							result.Add(ptCur);
						}
					}
				}
				else
				{
					SplayAngle = OutDir + SplayAngle15 * fSide;
					ptInter = (Point)ARANFunctions.LineLineIntersect(ptCur, SplayAngle, ptBase, BaseDir);

					ptInterDist = ARANFunctions.PointToLineDistance(ptInter, EndFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI);

					if (ptInterDist < TransitionDist)
					{
						ptCut = ARANFunctions.LocalToPrj(EndFIX.PrjPt, OutDir + ARANMath.C_PI, TransitionDist, 0);
						ptInter = (Point)ARANFunctions.LineLineIntersect(ptCur, SplayAngle, ptCut, OutDir + 0.5 * ARANMath.C_PI);
						//if GlDRAWFLG then
						//		GUI.DrawPointWithText(ptInter, 0, 'ptInter - 2');
					}
					result.Add(ptInter);
				}
			}

			//if (GlDRAWFLG)
			//{
			//    polygon = new Polygon();
			//    polygon.Add(result);
			//    GUI.DrawPolygon(polygon, RGB(0, 0, 255), sfsDiagonalCross);
			//}

			//GlDRAWFLG = False;//(EndFIX.ConstMode = IF_) and IsPrimary;
			JoinSegments(leg, result, ARP, true, IsPrimary);
			//GlDRAWFLG = False;
			return result;
		}

		static Ring CreateInnerTurnArea(Leg leg, ADHPType ARP, Boolean IsPrimary)
		{
			Double fDistTreshold, OutDir, EntryDir, Dist0, Dist1, dPhi1, dPhi2, azt0, TurnAng,
				Dist56000, Dist3700, CurDist, MaxDist, TransitionDist, CurWidth, ptInterDist, LPTYDist,
				fTmp, DivergenceAngle30, SpiralDivergenceAngle, SplayAngle15, BaseDir, LegLenght,
				SplayAngle, fSide, ASW_INMax, ASW_IN0C, ASW_IN0F, ASW_IN1;	//, Dist2;

			TurnDirection TurnDir;
			WayPoint StartFIX, EndFIX;

			Point InnerBasePoint, ptTmp, ptBase, ptCur, ptCut, ptInter;

			//Boolean ReCalcASW_IN0C;

			SpiralDivergenceAngle = GlobalVars.constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;

			if (IsPrimary)
			{
				//fTmp = GlobalVars.constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;
				DivergenceAngle30 = Math.Atan(0.5 * Math.Tan(SpiralDivergenceAngle));

				fTmp = GlobalVars.constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
				SplayAngle15 = Math.Atan(0.5 * Math.Tan(fTmp));
			}
			else
			{
				DivergenceAngle30 = SpiralDivergenceAngle;//GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;
				SplayAngle15 = GlobalVars.constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
			}

			StartFIX = leg.StartFIX;
			EndFIX = leg.EndFIX;

			EntryDir = StartFIX.EntryDirection;
			OutDir = StartFIX.OutDirection;
			TurnDir = ARANMath.SideToTurn(ARANMath.SideDef(StartFIX.PrjPt, EntryDir, EndFIX.PrjPt));

			fSide = (int)TurnDir;
			TurnAng = ARANMath.Modulus((EntryDir - OutDir) * fSide, ARANMath.C_PI);

			ptTmp = ARANFunctions.LocalToPrj(StartFIX.PrjPt, EntryDir - ARANMath.C_PI, StartFIX.EPT, 0);
			if (TurnDir == TurnDirection.CCW)
			{
				if (IsPrimary)
				{
					ASW_IN0F = StartFIX.ASW_2_L;
					ASW_IN0C = 0.5 * StartFIX.SemiWidth;
					ASW_IN1 = 0.5 * EndFIX.SemiWidth;
				}
				else
				{
					ASW_IN0F = StartFIX.ASW_L;
					ASW_IN0C = StartFIX.SemiWidth;
					ASW_IN1 = EndFIX.SemiWidth;
				}
				InnerBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir + 0.5 * ARANMath.C_PI, ASW_IN0F, 0);
			}
			else
			{
				if (IsPrimary)
				{
					ASW_IN0F = StartFIX.ASW_2_R;
					ASW_IN0C = 0.5 * StartFIX.SemiWidth;
					ASW_IN1 = 0.5 * EndFIX.SemiWidth;
				}
				else
				{
					ASW_IN0F = StartFIX.ASW_R;
					ASW_IN0C = StartFIX.SemiWidth;
					ASW_IN1 = EndFIX.SemiWidth;
				}
				InnerBasePoint = ARANFunctions.LocalToPrj(ptTmp, EntryDir - 0.5 * ARANMath.C_PI, ASW_IN0F, 0);
			}

			ASW_INMax = Math.Max(ASW_IN0C, ASW_IN1);
			Ring result = new Ring();
			ptCur = InnerBasePoint;

			//if GlDRAWFLG then
			//	GUI.DrawPointWithText(ptCur, 0, 'ptCur-0');

			result.Add(ptCur);
			CurDist = ARANFunctions.PointToLineDistance(ptCur, EndFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI);

			//MAX DISTANCE =================================================================

			///if (EndFIX.ConstMode == eFIXRole.FAF_ || ARANMath.RadToDeg(EndFIX.TurnAngle) <= 30)
			if (EndFIX.Role == eFIXRole.FAF_ || ARANMath.RadToDeg(EndFIX.TurnAngle) <= 30)
				LPTYDist = -0.5;
			else if (EndFIX.TurnDirection == TurnDir)
				LPTYDist = EndFIX.EPT - 0.5;
			else
				//LPTYDist = (2*BYTE(EndFIX.FlyMode == fmFlyBy)-1)*EndFIX.LPT - 0.5;
				LPTYDist = (EndFIX.FlyMode == eFlyMode.Flyby ? EndFIX.LPT : -EndFIX.LPT) - 0.5;

			//if(EndFIX.SensorType == stGNSS)		fDistTreshold = GNSSTriggerDistance
			//else									fDistTreshold = SBASTriggerDistance + GNSSTriggerDistance;
			fDistTreshold = PBNTerminalTriggerDistance;

			Dist0 = ARANMath.Hypot(ARP.pPtPrj.X - StartFIX.PrjPt.X, ARP.pPtPrj.Y - StartFIX.PrjPt.Y);
			Dist1 = ARANMath.Hypot(ARP.pPtPrj.X - EndFIX.PrjPt.X, ARP.pPtPrj.Y - EndFIX.PrjPt.Y);

			Dist56000 = LPTYDist;
			TransitionDist = LPTYDist;

			if (Dist0 >= fDistTreshold && Dist1 < fDistTreshold)
			{
				dPhi1 = Math.Atan2(ARP.pPtPrj.Y - EndFIX.PrjPt.Y, ARP.pPtPrj.X - EndFIX.PrjPt.X);
				dPhi2 = Math.Atan2(StartFIX.PrjPt.Y - EndFIX.PrjPt.Y, StartFIX.PrjPt.X - EndFIX.PrjPt.X);
				azt0 = dPhi2 - dPhi1;
				Dist56000 = Dist1 * Math.Cos(azt0) + Math.Sqrt(ARANMath.Sqr(fDistTreshold) - ARANMath.Sqr(Dist1 * Math.Sin(azt0)));
				TransitionDist = Dist56000;
			}

			//if (StartFIX.ConstMode == eFIXRole.IF_ && EndFIX.ConstMode == eFIXRole.FAF_)
			if (StartFIX.Role == eFIXRole.IF_ && EndFIX.Role == eFIXRole.FAF_)
			{
				Dist3700 = 1.5 * (StartFIX.XXT - EndFIX.XXT) / Math.Tan(GlobalVars.constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value);

				//Dist3700 = (StartFIX.SemiWidth - EndFIX.SemiWidth)/Tan(GPANSOPSConstants.Constant[arSecAreaCutAngl].Value);
				//Dist3700 = GPANSOPSConstants.Constant[rnvImMinDist].Value;
				TransitionDist = Math.Max(Dist56000, Dist3700);
			}

			MaxDist = Math.Max(LPTYDist, TransitionDist);

			if (CurDist < TransitionDist)
				TransitionDist = CurDist;

			//	if (CurDist > MaxDist)
			{
				//==============================================================================
				LegLenght = ARANMath.Hypot(EndFIX.PrjPt.X - StartFIX.PrjPt.X, EndFIX.PrjPt.Y - StartFIX.PrjPt.Y);

				//if (StartFIX.ConstMode == eFIXRole.FAF_ && EndFIX.ConstMode == eFIXRole.MAPt_)
				if (StartFIX.Role == eFIXRole.FAF_ && EndFIX.Role == eFIXRole.MAPt_)
				{
					dPhi1 = Math.Atan2(ASW_IN0C - ASW_IN1, LegLenght);
					if (dPhi1 > DivergenceAngle30)
						dPhi1 = DivergenceAngle30;

					BaseDir = OutDir + dPhi1 * fSide;
					ptBase = ARANFunctions.LocalToPrj(EndFIX.PrjPt, OutDir - 0.5 * ARANMath.C_PI * fSide, ASW_IN1, 0);
				}
				else
				{
					BaseDir = OutDir;
					ptBase = ARANFunctions.LocalToPrj(EndFIX.PrjPt, OutDir - 0.5 * ARANMath.C_PI * fSide, ASW_IN0C, 0);
				}

				//if GlDRAWFLG then
				//	GUI.DrawPointWithText(ptBase, 0, 'ptBase');

				CurWidth = -fSide * ARANFunctions.PointToLineDistance(ptCur, ptBase, BaseDir);
				if (Math.Abs(CurWidth) > ARANMath.EpsilonDistance)
				{
					if (CurWidth > 0)
						SplayAngle = EntryDir - 0.5 * TurnAng * fSide;
					else
					{
						//SplayAngle15 = GlobalVars.constants.Pansops[ePANSOPSData.arafTrn_OSplay].Value;
						SplayAngle = OutDir - SplayAngle15 * fSide;
					}

					ptInter = (Point)ARANFunctions.LineLineIntersect(InnerBasePoint, SplayAngle, ptBase, BaseDir);
					//if GlDRAWFLG then
					//	GUI.DrawPointWithText(ptInter, 0, 'ptInter - 1');

					ptInterDist = ARANFunctions.PointToLineDistance(ptInter, EndFIX.PrjPt, OutDir + 0.5 * ARANMath.C_PI);

					if (ptInterDist < TransitionDist)
					{
						ptCut = ARANFunctions.LocalToPrj(EndFIX.PrjPt, OutDir + ARANMath.C_PI, TransitionDist, 0);

						ptInter = (Point)ARANFunctions.LineLineIntersect(InnerBasePoint, SplayAngle, ptCut, OutDir + 0.5 * ARANMath.C_PI);
						//if GlDRAWFLG then
						//	GUI.DrawPointWithText(ptInter, 0, 'ptInter - 2');

					}
					result.Add(ptInter);
				}
			}

			JoinSegments(leg, result, ARP, false, IsPrimary);
			return result;
		}

		public static bool CreateProtectionArea(Leg PrevLeg, Leg leg, ADHPType ARP)
		{
			Double WSpeed, OutDir, fSide, TurnAngle, ASW_OUT1F, ASW_IN1F;

			TurnDirection TurnDir;
			Point ptTmp, ptLPTMeasure, ptEPTMeasure;
			Ring InnerRingP, OuterRingP, InnerRingB, OuterRingB;
			MultiPolygon PrimaryArePolygon, BufferPolygon, FullArePolygon;
			WayPoint StartFIX, EndFIX;

			//Polygon TestPolygon;
			//bool Test = false

			//	if Assigned(PrevLeg) then
			//		Leg.StartFIX.JointFlags := PrevLeg.EndFIX.JointFlags;

			WSpeed = GlobalVars.constants.Pansops[ePANSOPSData.dpWind_Speed].Value;
			StartFIX = leg.StartFIX;
			EndFIX = leg.EndFIX;

			StartFIX.CalcTurnRangePoints();
			EndFIX.CalcTurnRangePoints();

			OutDir = EndFIX.EntryDirection;	//StartFIX.OutDirection;//
			TurnAngle = StartFIX.TurnAngle;

			if (ARANMath.RadToDeg(TurnAngle) <= 10 ||
			//(StartFIX.FlyMode == eFlyMode.FlyBy && StartFIX.ConstMode != eFIXRole.FAF_ && ARANMath.RadToDeg(TurnAngle) <= 30))
			(StartFIX.FlyMode == eFlyMode.Flyby && StartFIX.Role != eFIXRole.FAF_ && ARANMath.RadToDeg(TurnAngle) <= 30))
			{
				/*
				if RadToDeg(TurnAngle) <= 0.1 then
				begin
					OuterRingP := CreateOuterTurnAreaLT(PrevLeg, Leg, ARP, True);
					InnerRingP := CreateOuterTurnAreaLT(PrevLeg, Leg, ARP, True);

					OuterRingB := CreateOuterTurnAreaLT(PrevLeg, Leg, ARP, False);
					InnerRingB := CreateOuterTurnAreaLT(PrevLeg, Leg, ARP, False);


					OuterRingP := CreateInnerTurnAreaLT(PrevLeg, Leg, ARP, True);
					InnerRingP := CreateInnerTurnAreaLT(PrevLeg, Leg, ARP, True);

					OuterRingB := CreateInnerTurnAreaLT(PrevLeg, Leg, ARP, False);
					InnerRingB := CreateInnerTurnAreaLT(PrevLeg, Leg, ARP, False);
				end
				else*/
				{
					OuterRingP = CreateOuterTurnAreaLT(PrevLeg, leg, ARP, true);
					InnerRingP = CreateInnerTurnAreaLT(PrevLeg, leg, ARP, true);

					OuterRingB = CreateOuterTurnAreaLT(PrevLeg, leg, ARP, false);
					InnerRingB = CreateInnerTurnAreaLT(PrevLeg, leg, ARP, false);
				}

				ptTmp = new Point();
				if (PrevLeg != null)
				{
					ptTmp = ARANFunctions.LocalToPrj(StartFIX.PrjPt, StartFIX.OutDirection, -0.5, 0);
					OuterRingP.Insert(0, ptTmp);	//StartFIX.PrjPt
					OuterRingB.Insert(0, ptTmp);	//StartFIX.PrjPt
				}

				//if (EndFIX.ConstMode != eFIXRole.MAPt_ && (ARANMath.RadToDeg(EndFIX.TurnAngle) <= 10 ||
				//	(EndFIX.FlyMode == eFlyMode.FlyBy && EndFIX.ConstMode != eFIXRole.FAF_ && ARANMath.RadToDeg(EndFIX.TurnAngle) <= 30)))
				if (EndFIX.Role != eFIXRole.MAPt_ && (ARANMath.RadToDeg(EndFIX.TurnAngle) <= 10 ||
					(EndFIX.FlyMode == eFlyMode.Flyby && EndFIX.Role != eFIXRole.FAF_ && ARANMath.RadToDeg(EndFIX.TurnAngle) <= 30)))
				{
					ptTmp = ARANFunctions.LocalToPrj(EndFIX.PrjPt, OutDir, 0.5, 0);
					OuterRingP.Add(ptTmp);		//EndFIX.PrjPt
					OuterRingB.Add(ptTmp);		//EndFIX.PrjPt
				}
			}
			else
			{
				//GlDRAWFLG = EndFIX.ConstMode = IF_;
				OuterRingP = CreateOuterTurnArea(leg, WSpeed, ARP, true);
				//GlDRAWFLG = false;

				InnerRingP = CreateInnerTurnArea(leg, ARP, true);

				OuterRingB = CreateOuterTurnArea(leg, WSpeed, ARP, false);
				InnerRingB = CreateInnerTurnArea(leg, ARP, false);

				ptTmp = new Point();

				//if (EndFIX.ConstMode != eFIXRole.MAPt_ && (ARANMath.RadToDeg(EndFIX.TurnAngle) <= 10 ||
				//	(EndFIX.FlyMode == eFlyMode.FlyBy && EndFIX.ConstMode != eFIXRole.FAF_ &&
				if (EndFIX.Role != eFIXRole.MAPt_ && (ARANMath.RadToDeg(EndFIX.TurnAngle) <= 10 ||
					(EndFIX.FlyMode == eFlyMode.Flyby && EndFIX.Role != eFIXRole.FAF_ &&
						ARANMath.RadToDeg(EndFIX.TurnAngle) <= 30)))
				{
					ptTmp = ARANFunctions.LocalToPrj(EndFIX.PrjPt, OutDir, 0.5, 0);
					OuterRingP.Add(ptTmp);		//EndFIX.PrjPt
					OuterRingB.Add(ptTmp);		//EndFIX.PrjPt
				}

			}

			//if Counter mod 4 = 1 then
			//if (Test )
			//{
			//    TestPolygon = new Polygon();
			//    TestPolygon.ExteriorRing = OuterRingP;
			//    GUI.DrawPolygon(TestPolygon, ARANFunctions.RGB(0, 0, 255), sfsSolid);

			//    TestPolygon.ExteriorRing = InnerRingP;
			//    GUI.DrawPolygon(TestPolygon, 255, sfsSolid);
			//}

			OuterRingP.AddReverse(InnerRingP);
			OuterRingB.AddReverse(InnerRingB);

			PrimaryArePolygon = new MultiPolygon();
			Polygon tmpPolygon = new Polygon();
			tmpPolygon.ExteriorRing = OuterRingP;
			PrimaryArePolygon.Add(tmpPolygon);

			BufferPolygon = new MultiPolygon();
			tmpPolygon = new Polygon();
			tmpPolygon.ExteriorRing = OuterRingB;
			BufferPolygon.Add(tmpPolygon);


			//if Counter = 5 then
			//if (false )
			//{
			//    GUI.DrawPolygon(BufferPolygon, ARANFunctions.RGB(0, 0, 255), sfsSolid);
			//    GUI.DrawPolygon(PrimaryArePolygon, 255, sfsSolid);
			//}

			GeometryOperators GeoOperators = new GeometryOperators();
			FullArePolygon = (MultiPolygon)GeoOperators.UnionGeometry(BufferPolygon, PrimaryArePolygon);

			leg.PrimaryArea = PrimaryArePolygon;
			leg.FullArea = FullArePolygon;

			TurnDir = EndFIX.TurnDirection;
			fSide = (int)TurnDir;

			if (TurnDir == TurnDirection.NONE)
			{
				ptLPTMeasure = ARANFunctions.LocalToPrj(EndFIX.PrjPt, OutDir, 0, 0);
				ptEPTMeasure = ARANFunctions.LocalToPrj(EndFIX.PrjPt, OutDir, 0, 0);
			}
			else
			{
				ptLPTMeasure = ARANFunctions.LocalToPrj(EndFIX.PrjPt, OutDir - (EndFIX.FlyMode == eFlyMode.Flyby ? ARANMath.C_PI : 0), EndFIX.LPT, 0);
				ptEPTMeasure = ARANFunctions.LocalToPrj(EndFIX.PrjPt, OutDir - ARANMath.C_PI, EndFIX.EPT, 0);
			}

			//====================================================================================================================
			ptTmp = ARANFunctions.RingVectorIntersect(PrimaryArePolygon[0].ExteriorRing, ptEPTMeasure, OutDir - 0.5 * ARANMath.C_PI * fSide, out ASW_IN1F);

			//if EndFIX.Role = MAPt_ then
			//if Counter = 5 then
			//if Test then
			//if (false )
			//{
			//    GUI.DrawPolygon(PrimaryArePolygon, ARANFunctions.RGB(0,255,0), sfsCross);
			//    GUI.DrawPointWithText(ptEPTMeasure, 0, "ptEPTMeasure");
			//    GUI.DrawPointWithText(ptTmp, 0, "ptEPTMeasure");
			//}

			if (ptTmp != null)
			{
				//GUI.DrawPointWithText(ptTmp, 0, 'ptEPTM' + IntToStr(Counter));
				if (TurnDir == TurnDirection.CCW)
					EndFIX.ASW_2_L = ASW_IN1F;
				else
					EndFIX.ASW_2_R = ASW_IN1F;

			}

			ptTmp = ARANFunctions.RingVectorIntersect(PrimaryArePolygon[0].ExteriorRing, ptLPTMeasure, OutDir + 0.5 * ARANMath.C_PI * fSide, out ASW_OUT1F);
			if (ptTmp != null)
			{
				if (TurnDir == TurnDirection.CCW)
					EndFIX.ASW_2_R = ASW_OUT1F;
				else
					EndFIX.ASW_2_L = ASW_OUT1F;
			}
			//====================================================================================================================
			ptTmp = ARANFunctions.RingVectorIntersect(FullArePolygon[0].ExteriorRing, ptEPTMeasure, OutDir - 0.5 * ARANMath.C_PI * fSide, out ASW_IN1F);
			if (ptTmp != null)
			{
				if (TurnDir == TurnDirection.CCW)
					EndFIX.ASW_L = ASW_IN1F;
				else
					EndFIX.ASW_R = ASW_IN1F;
			}

			ptTmp = ARANFunctions.RingVectorIntersect(FullArePolygon[0].ExteriorRing, ptLPTMeasure, OutDir + 0.5 * ARANMath.C_PI * fSide, out ASW_OUT1F);
			if (ptTmp != null)
			{
				if (TurnDir == TurnDirection.CCW)
					EndFIX.ASW_R = ASW_OUT1F;
				else
					EndFIX.ASW_L = ASW_OUT1F;
			}
			//====================================================================================================================

			//Counter++;
			return true;
		}

		public static void CreateAssesmentArea(Leg PrevLeg, Leg leg)
		{
			Double TurnAng, theta, d, fSide, LineLen, Dir0, Dir2, EntryDir;

			TurnDirection TurnDir;

			MultiPolygon FullPoly, PrimPoly;
			Ring PrevPrimaryRing, PrevFullRing, PrimaryRing, FullRing;

			LineString NNLinePart;
			WayPoint StartFIX, EndFIX;

			Point PtTmp0,
			PtTmp1, PtTmp2;
			GeometryOperators GeoOperators = new GeometryOperators();

			//Boolean test := false;

			if (PrevLeg != null)
			{
				FullPoly = (MultiPolygon)GeoOperators.UnionGeometry(PrevLeg.FullAssesmentArea, leg.FullAssesmentArea);
				PrimPoly = (MultiPolygon)GeoOperators.UnionGeometry(PrevLeg.PrimaryAssesmentArea, leg.PrimaryAssesmentArea);
			}
			else
			{
				FullPoly = (MultiPolygon)GeoOperators.UnionGeometry(leg.FullAssesmentArea, leg.FullAssesmentArea);
				PrimPoly = (MultiPolygon)GeoOperators.UnionGeometry(leg.PrimaryAssesmentArea, leg.PrimaryAssesmentArea);
			}

			StartFIX = leg.StartFIX;
			EndFIX = leg.EndFIX;

			EntryDir = StartFIX.EntryDirection;
			TurnAng = StartFIX.TurnAngle;
			TurnDir = StartFIX.TurnDirection;
			fSide = (int)TurnDir;

			LineLen = 10 * EndFIX.ASW_R + EndFIX.ASW_L;

			if (TurnDir == TurnDirection.NONE)
			{
				PtTmp1 = ARANFunctions.LocalToPrj(StartFIX.PrjPt, EntryDir + ARANMath.C_PI, StartFIX.EPT, 0);
				Dir0 = EntryDir + 1.5 * ARANMath.C_PI;
				Dir2 = EntryDir + 0.5 * ARANMath.C_PI;
			}
			else
			{
				theta = 0.25 * TurnAng;
				d = StartFIX.ATT * Math.Tan(theta);
				PtTmp1 = ARANFunctions.LocalToPrj(StartFIX.PrjPt, EntryDir + ARANMath.C_PI, StartFIX.ATT, -d * fSide);

				if (TurnDir == TurnDirection.CCW)
				{
					Dir0 = EntryDir + 1.5 * ARANMath.C_PI;
					Dir2 = EntryDir + 0.5 * (ARANMath.C_PI + TurnAng);
				}
				else
				{
					Dir0 = EntryDir - 0.5 * (ARANMath.C_PI + TurnAng);
					Dir2 = EntryDir + 0.5 * ARANMath.C_PI;
				}
			}

			PtTmp0 = ARANFunctions.LocalToPrj(PtTmp1, Dir0, LineLen, 0);
			PtTmp2 = ARANFunctions.LocalToPrj(PtTmp1, Dir2, LineLen, 0);

			NNLinePart = new LineString();
			NNLinePart.Add(PtTmp0);
			NNLinePart.Add(PtTmp1);
			NNLinePart.Add(PtTmp2);

			ARANFunctions.CutRingByNNLine(PrimPoly[0].ExteriorRing, NNLinePart, out PrevPrimaryRing, out PrimaryRing);
			ARANFunctions.CutRingByNNLine(FullPoly[0].ExteriorRing, NNLinePart, out PrevFullRing, out FullRing);

			/*
			if test and (EndFIX.Role = MAPt_) then
			begin
			    GUI.DrawPolygon(FullPoly, 255, sfsSolid);
			    GUI.DrawPolygon(PrimPoly, 0, sfsSolid);

			    GUI.DrawRing(PrevFullRing, 0, sfsCross);
			    GUI.DrawRing(FullRing, 0, sfsCross);
			end;
			*/
			Polygon tmpPolygon;

			if (PrevLeg != null)
			{
				if (PrevFullRing != null)
				{
					//FullPoly.ExteriorRing = PrevFullRing;
					tmpPolygon = new Polygon();
					tmpPolygon.ExteriorRing = PrevFullRing;
					FullPoly.Add(tmpPolygon);

					PrevLeg.FullAssesmentArea = FullPoly;
				}

				if (PrevPrimaryRing != null)
				{
					//PrimPoly.ExteriorRing = PrevPrimaryRing;
					tmpPolygon = new Polygon();
					tmpPolygon.ExteriorRing = PrevPrimaryRing;
					PrimPoly.Add(tmpPolygon);

					PrevLeg.PrimaryAssesmentArea = PrimPoly;
				}
			}

			if (FullRing != null)
			{
				//FullPoly.ExteriorRing = FullRing;
				tmpPolygon = new Polygon();
				tmpPolygon.ExteriorRing = FullRing;
				FullPoly.Add(tmpPolygon);

				leg.FullAssesmentArea = FullPoly;
			}

			if (PrimaryRing != null)
			{
				//PrimPoly.ExteriorRing = PrimaryRing;
				tmpPolygon = new Polygon();
				tmpPolygon.ExteriorRing = PrimaryRing;
				PrimPoly.Add(tmpPolygon);

				leg.PrimaryAssesmentArea = PrimPoly;
			}
		}
	}

	//function CalcMAMinOCA(ObstacleList: TList; Gradiend: Double; MinValue: Double; var MaxIx: Integer; AndMask: Integer = -1): Double;
	//var
	//    I, N:		Integer;
	//    ObstacleInfo:	TObstacleInfo;
	//begin
	//    result := MinValue;

	//    N := ObstacleList.Count;
	//    MaxIx := -1;

	//    for I := 0 To N - 1 do
	//    begin
	//        ObstacleInfo := TObstacleInfo(ObstacleList.Items[I]);
	//        ObstacleInfo.Flags := ObstacleInfo.Flags and AndMask;

	//        if ObstacleInfo.Dist >= 0 then
	//            ObstacleInfo.ReqOCA := ObstacleInfo.ReqH - ObstacleInfo.Dist * Gradiend
	//        else
	//            ObstacleInfo.ReqOCA := Min(ObstacleInfo.ReqH - ObstacleInfo.Dist * Gradiend,
	//                                        ObstacleInfo.Elevation + FlightPhases[fpFinalApproach].MOC*ObstacleInfo.fTmp);

	//        if result < ObstacleInfo.ReqOCA then
	//        begin
	//            MaxIx := I;
	//            result := ObstacleInfo.ReqOCA;
	//        end;
	//    end;

	//    if MaxIx >= 0 then
	//    begin
	//        ObstacleInfo := TObstacleInfo(ObstacleList.Items[MaxIx]);
	//        ObstacleInfo.Flags := ObstacleInfo.Flags or 2;
	//    end;
	//end;
}


