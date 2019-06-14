using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using Microsoft.Win32;

namespace EOSID
{
	[System.Runtime.InteropServices.ComVisible(false)]
	internal static class Functions
	{
#if DEBUG
		public static void ProcessMessages(bool continuous = false)
		{
			do
			{
				System.Windows.Forms.Application.DoEvents();
				//continuous = false;
			}
			while (continuous);
		}
#endif

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
					return sSign + X.ToString("0.0000") + "°";
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

					sTmp = xMin.ToString("00.0000");
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

		internal static void TextBoxFloat(ref char KeyChar, string BoxText)
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

		internal static void TextBoxInteger(ref char KeyChar)
		{
			if (KeyChar < ' ')
				return;
			if ((KeyChar < '0') || (KeyChar > '9'))
				KeyChar = '\0';
		}

		internal static T RegRead<T>(RegistryKey hKey, string key, string valueName, T defaultValue)
		{
			try
			{
				RegistryKey regKey = hKey.OpenSubKey(key, false);
				if (regKey != null)
				{
					object value = regKey.GetValue(valueName);
					if (value != null)
					{
						try
						{
							return (T)Convert.ChangeType(value, typeof(T));
						}
						catch { }
					}
				}
			}
			catch { }

			return defaultValue;
		}

		internal static int RegWrite(Microsoft.Win32.RegistryKey HKey, string Key, string ValueName, object Value)
		{
			try
			{
				Microsoft.Win32.RegistryKey regKey = HKey.OpenSubKey(Key, true);
				if (regKey == null)
					return -1;

				regKey.SetValue(ValueName, Value);
				return 0;
			}
			catch { }

			return -1;
		}

		internal static double DegToRad(double Val_Renamed)
		{
			return Val_Renamed * GlobalVars.DegToRadValue;
		}

		internal static double RadToDeg(double Val_Renamed)
		{
			return Val_Renamed * GlobalVars.RadToDegValue;
		}

		internal static int RGB(int r, int g, int b)
		{
			return (((b << 8) | g) << 8) | r;
		}

		internal static IGeometry ToGeo(IGeometry prjGeometry)
		{
			IClone pClone = (IClone)prjGeometry;
			IGeometry toGeoReturn = (IGeometry)pClone.Clone();

			toGeoReturn.SpatialReference = GlobalVars.pSpRefPrj;
			toGeoReturn.Project(GlobalVars.pSpRefShp);
			return toGeoReturn;
		}

		internal static IGeometry ToPrj(IGeometry geoGeometry)
		{
			IClone pClone = (IClone)geoGeometry;
			IGeometry toPrjReturn = (IGeometry)pClone.Clone();

			toPrjReturn.SpatialReference = GlobalVars.pSpRefShp;
			toPrjReturn.Project(GlobalVars.pSpRefPrj);
			return toPrjReturn;
		}

		internal static double Azt2Dir(IPoint ptGeo, decimal Azt)
		{
			return Azt2Dir(ptGeo, (double)Azt);
		}

		internal static double Azt2Dir(IPoint ptGeo, double Azt)
		{
			double ResX, ResY;
			NativeMethods.PointAlongGeodesic(ptGeo.X, ptGeo.Y, 10.0, Azt, out ResX, out ResY);

			IPoint Pt10 = new Point();
			Pt10.PutCoords(ResX, ResY);

			ILine pLine = new Line();
			pLine.PutCoords(ptGeo, Pt10);
			pLine = (ILine)ToPrj(pLine);

			return NativeMethods.Modulus(GlobalVars.RadToDegValue * pLine.Angle, 360.0);
		}

		internal static double Dir2Azt(IPoint pPtPrj, double Dir)
		{
			double resD, resI;

			IPoint PtN = LocalToPrj(pPtPrj, Dir, 10.0, 0.0);
			IPoint Pt10 = (IPoint)ToGeo(PtN);
			PtN = (IPoint)ToGeo(pPtPrj);

			NativeMethods.ReturnGeodesicAzimuth(PtN.X, PtN.Y, Pt10.X, Pt10.Y, out resD, out resI);
			return resD;
		}

		internal static int SideDef(IPoint pointOnLine, double lineAngleInDegree, IPoint testPoint)
		{
			double fdY = testPoint.Y - pointOnLine.Y;
			double fdX = testPoint.X - pointOnLine.X;
			double fDist = fdY * fdY + fdX * fdX;

			if (fDist < GlobalVars.distEps2)
				return SideDirection.onSide;

			double Angle12 = GlobalVars.RadToDegValue * Math.Atan2(fdY, fdX);
			double rAngle = NativeMethods.Modulus(lineAngleInDegree - Angle12);

			if (rAngle < GlobalVars.degEps || 360.0 - rAngle < GlobalVars.degEps || Math.Abs(rAngle - 180.0) < GlobalVars.degEps)
				return SideDirection.onSide;

			//if ((Math.Abs(rAngle) < GlobalVars.degEps) || (Math.Abs(rAngle - 90) < GlobalVars.degEps))
			//    return SideDirection.sideOn;

			if (rAngle < 180.0)
				return SideDirection.rightSide;

			return SideDirection.leftSide;
		}

		//internal static int SideDef(IPoint PtInLine, double LineAngle, IPoint PtOutLine)
		//{
		//    double Angle12 = ReturnAngleInDegrees(PtInLine, PtOutLine);
		//    double dAngle = NativeMethods.Modulus(LineAngle - Angle12, 360.0);

		//    if (dAngle == 0.0 || dAngle == 180.0)
		//        return 0;

		//    if (dAngle < 180.0)
		//        return 1;

		//    return -1;
		//}

		internal static double IAS2TAS(double IAS, double h, double dT)
		{
			if ((h >= 288.0 / 0.006496) || (h >= (288.0 + dT) / 0.006496))
				return 2.0 * IAS;				//     h = Int(288.0 / 0.006496)
			else
				return IAS * 171233.0 * System.Math.Sqrt(288.0 + dT - 0.006496 * h) / (Math.Pow((288.0 - 0.006496 * h), 2.628));
		}

		internal static double Bank2Radius(double Bank, double V)
		{
			double Rv = 6.355 * System.Math.Tan(GlobalVars.DegToRadValue * Bank) / (Math.PI * V);

			if (Rv > 0.003)
				Rv = 0.003;

			if (Rv > 0.0)
				return V / (20.0 * Math.PI * Rv);

			return -1.0;
		}

		internal static double Radius2Bank(double R, double V)
		{
			if (R > 0.0)
				return GlobalVars.RadToDegValue * (System.Math.Atan(V * V / (20.0 * R * 6.355)));

			return -1.0;
		}

		internal static double PointToLineDistance(IPoint refPoint, IPoint linePoint, double lineDirInDeg)
		{
			double lineDirInRadian = GlobalVars.DegToRadValue * lineDirInDeg;
			double CosA = Math.Cos(lineDirInRadian);
			double SinA = Math.Sin(lineDirInRadian);
			return (refPoint.Y - linePoint.Y) * CosA - (refPoint.X - linePoint.X) * SinA;
		}

		internal static double ReturnAngleInDegrees(IPoint ptFrom, IPoint ptTo)
		{
			double fdX = ptTo.X - ptFrom.X;
			double fdY = ptTo.Y - ptFrom.Y;
			return NativeMethods.Modulus(RadToDeg(NativeMethods.ATan2(fdY, fdX)), 360.0);
		}

		internal static double ReturnDistanceInMeters(IPoint ptFrom, IPoint ptTo)
		{
			double fdX = ptTo.X - ptFrom.X;
			double fdY = ptTo.Y - ptFrom.Y;
			return System.Math.Sqrt(fdX * fdX + fdY * fdY);
		}

		// ==================================
		internal static Point LocalToPrj(IPoint center, double dirInDeg, double x, double y = 0.0)
		{
			double dirInRadian = GlobalVars.DegToRadValue * dirInDeg;
			double SinA = Math.Sin(dirInRadian);
			double CosA = Math.Cos(dirInRadian);
			double Xnew = center.X + x * CosA - y * SinA;
			double Ynew = center.Y + x * SinA + y * CosA;

			Point result = new Point();
			result.PutCoords(Xnew, Ynew);
			return result;
		}

		internal static Point LocalToPrj(IPoint center, double dirInDeg, IPoint ptPrj)
		{
			double dirInRadian = GlobalVars.DegToRadValue * dirInDeg;
			double SinA = Math.Sin(dirInRadian);
			double CosA = Math.Cos(dirInRadian);
			double Xnew = center.X + ptPrj.X * CosA - ptPrj.Y * SinA;
			double Ynew = center.Y + ptPrj.X * SinA + ptPrj.Y * CosA;

			Point result = new Point();
			result.PutCoords(Xnew, Ynew);
			result.Z = center.Z;
			return result;
		}
		// ==================================
		internal static Point PrjToLocal(IPoint center, double dirInDeg, double x, double y)
		{
			double dirInRadian = GlobalVars.DegToRadValue * dirInDeg;
			double SinA = Math.Sin(dirInRadian);
			double CosA = Math.Cos(dirInRadian);
			double dX = x - center.X;
			double dY = y - center.Y;
			double Xnew = dX * CosA + dY * SinA;
			double Ynew = -dX * SinA + dY * CosA;

			Point result = new Point();
			result.PutCoords(Xnew, Ynew);
			result.Z = center.Z;
			return result;
		}

		internal static void PrjToLocal(IPoint center, double dirInDeg, double x, double y, out double resX, out double resY)
		{
			double dirInRadian = GlobalVars.DegToRadValue * dirInDeg;
			double SinA = Math.Sin(dirInRadian);
			double CosA = Math.Cos(dirInRadian);
			double dX = x - center.X;
			double dY = y - center.Y;

			resX = dX * CosA + dY * SinA;
			resY = -dX * SinA + dY * CosA;
		}

		internal static Point PrjToLocal(IPoint center, double dirInDeg, IPoint ptPrj)
		{
			double dirInRadian = GlobalVars.DegToRadValue * dirInDeg;
			double SinA = Math.Sin(dirInRadian);
			double CosA = Math.Cos(dirInRadian);
			double dX = ptPrj.X - center.X;
			double dY = ptPrj.Y - center.Y;

			double Xnew = dX * CosA + dY * SinA;
			double Ynew = -dX * SinA + dY * CosA;

			Point result = new Point();
			result.PutCoords(Xnew, Ynew);
			result.Z = ptPrj.Z;
			return result;
		}

		internal static void PrjToLocal(IPoint center, double dirInDeg, IPoint ptPrj, out double resX, out double resY)
		{
			double dirInRadian = GlobalVars.DegToRadValue * dirInDeg;
			double SinA = Math.Sin(dirInRadian);
			double CosA = Math.Cos(dirInRadian);
			double dX = ptPrj.X - center.X;
			double dY = ptPrj.Y - center.Y;

			resX = dX * CosA + dY * SinA;
			resY = -dX * SinA + dY * CosA;
		}

		internal static IPoint LineLineIntersect(IPoint ptLine1, double dirLine1, IPoint ptLine2, double dirLine2)
		{
			const double Eps = 0.0001;
			double d, Ua, Ub, k,
				cosF1, sinF1,
				cosF2, sinF2;
			dirLine1 = GlobalVars.DegToRadValue * dirLine1;
			dirLine2 = GlobalVars.DegToRadValue * dirLine2;

			cosF1 = Math.Cos(dirLine1);
			sinF1 = Math.Sin(dirLine1);

			cosF2 = Math.Cos(dirLine2);
			sinF2 = Math.Sin(dirLine2);

			d = sinF2 * cosF1 - cosF2 * sinF1;

			Ua = cosF2 * (ptLine1.Y - ptLine2.Y) -
				  sinF2 * (ptLine1.X - ptLine2.X);

			Ub = cosF1 * (ptLine1.Y - ptLine2.Y) -
				  sinF1 * (ptLine1.X - ptLine2.X);

			if (Math.Abs(d) < Eps)
				return null;

			k = Ua / d;
			IPoint result = new Point();
			result.PutCoords(ptLine1.X + k * cosF1, ptLine1.Y + k * sinF1);
			return result;
		}

		// ==================================

		internal static Interval[] IntervalsDifference(Interval A, Interval B)
		{
			Interval[] res = null;


			if (B.Left == B.Right || B.Right < A.Left || A.Right < B.Left)
			{
				res = new Interval[1];
				res[0] = A;
			}
			else if (A.Left < B.Left && A.Right > B.Right)
			{
				res = new Interval[2];
				res[0].Left = A.Left;
				res[0].Right = B.Left;
				res[1].Left = B.Right;
				res[1].Right = A.Right;
			}
			else if (A.Right > B.Right)
			{
				res = new Interval[1];
				res[0].Left = B.Right;
				res[0].Right = A.Right;
			}
			else if (A.Left < B.Left)
			{
				res = new Interval[1];
				res[0].Left = A.Left;
				res[0].Right = B.Left;
			}
			else
				res = new Interval[0];

			return res;
		}

		internal static void SortIntervals(Interval[] Intervals, bool RightSide = false)
		{
			int N = Intervals.Length;

			for (int I = 0; I < N - 1; I++)
			{
				for (int J = I + 1; J < N; J++)
				{
					if (RightSide)
					{
						if (Intervals[I].Right > Intervals[J].Right)
						{
							Interval Tmp = Intervals[I];
							Intervals[I] = Intervals[J];
							Intervals[J] = Tmp;
						}
					}
					else
					{
						if (Intervals[I].Left > Intervals[J].Left)
						{
							Interval Tmp = Intervals[I];
							Intervals[I] = Intervals[J];
							Intervals[J] = Tmp;
						}
					}
				}
			}
		}

		internal static Interval[] CiclicIntervalsIntersection(Interval A, Interval B)
		{
			Interval[] res;

			if (A.Right == 360.0)
			{
				res = new Interval[0];
				res[0] = A;
				return res;
			}
			if (B.Right == 360.0)
			{
				res = new Interval[0];
				res[0] = B;
				return res;
			}

			double Left1, Left2;
			double Right1, Right2;

			int cnt = 0, i;

			double dA = NativeMethods.Modulus(A.Right - A.Left);
			double dB = NativeMethods.Modulus(B.Right - B.Left);

			double dBL = NativeMethods.Modulus(B.Left - A.Left);
			double dAR = NativeMethods.Modulus(A.Right - B.Left);
			Left1 = Left2 = Right1 = Right2 = 0.0;

			if (dBL <= dA)
			{
				cnt = 1;
				Left1 = B.Left;

				if (dAR < dB)
					Right1 = A.Right;
				else
					Right1 = B.Right;
			}

			double dAL = 360 - dBL;// NativeMethods.Modulus(A.Left - B.Left);
			double dBR = NativeMethods.Modulus(B.Right - A.Left);

			if (dAL <= dB)
			{
				cnt |= 2;
				Left2 = A.Left;

				if (dBR < dA)
					Right2 = B.Right;
				else
					Right2 = A.Right;
			}

			if (cnt == 0)
			{
				res = new Interval[0];
				return res;
			}

			i = 0;
			if (cnt == 3)
				res = new Interval[2];
			else
				res = new Interval[1];

			if ((cnt & 1) != 0)
			{
				res[i].Left = Left1;
				res[i].Right = Right1;
				i++;
			}

			if ((cnt & 2) != 0)
			{
				res[i].Left = Left2;
				res[i].Right = Right2;
			}

			return res;
		}

		// ==================================

		internal static Polyline CreateArcPrj(IPoint ptCnt, IPoint ptFrom, IPoint ptTo, int ClWise)
		{
			double dX = ptFrom.X - ptCnt.X;
			double dY = ptFrom.Y - ptCnt.Y;
			double R = Math.Sqrt(dX * dX + dY * dY);

			double dirFrom = NativeMethods.Modulus(GlobalVars.RadToDegValue * Math.Atan2(dY, dX), 360.0);
			double dirTo = NativeMethods.Modulus(GlobalVars.RadToDegValue * Math.Atan2(ptTo.Y - ptCnt.Y, ptTo.X - ptCnt.X), 360.0);

			double dDir = NativeMethods.Modulus((dirTo - dirFrom) * ClWise, 360.0);

			int n = (int)(dDir);	// + 0.5

			if (n < 5)
				n = 5;
			else if (n < 10)
				n = 10;

			double angStep = dDir / n;

			IPointCollection result = new ESRI.ArcGIS.Geometry.Polyline();

			result.AddPoint(ptFrom);

			IPoint ptCurr = new ESRI.ArcGIS.Geometry.Point();

			for (int i = 1; i < n; i++)
			{
				double iInRad = GlobalVars.DegToRadValue * (dirFrom + i * angStep * ClWise);
				ptCurr.X = ptCnt.X + R * System.Math.Cos(iInRad);
				ptCurr.Y = ptCnt.Y + R * System.Math.Sin(iInRad);
				result.AddPoint(ptCurr);
			}

			result.AddPoint(ptTo);
			return (Polyline)result;
		}

		internal static IPolygon CreateCirclePrj(IPoint pPoint1, double Radius)
		{
			IPointCollection result = new Polygon();
			IPoint ptCurr = new ESRI.ArcGIS.Geometry.Point();

			for (int i = 0; i <= 359; i++)
			{
				double iInRad = i * GlobalVars.DegToRadValue;
				ptCurr.X = pPoint1.X + Radius * System.Math.Cos(iInRad);
				ptCurr.Y = pPoint1.Y + Radius * System.Math.Sin(iInRad);
				result.AddPoint(ptCurr);
			}

			ITopologicalOperator2 pTopo = result as ITopologicalOperator2;
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();
			return (IPolygon)result;
		}

		internal static IPolyline CalcTrajectoryFromMultiPoint(IPointCollection MultiPoint)
		{
			IPolyline result = (IPolyline)new Polyline();
			IPointCollection ptColl = (IPointCollection)result;

			double fE = 0.5 * GlobalVars.DegToRadValue;

			int n = MultiPoint.PointCount - 1;

			IPoint CntPt = new Point();
			IConstructPoint ptConstr = (IConstructPoint)CntPt;

			ptColl.AddPoint(MultiPoint.Point[0]);

			for (int i = 0; i < n; i++)
			{
				IPoint FromPt = MultiPoint.Point[i];
				IPoint ToPt = MultiPoint.Point[i + 1];
				double fTmp = GlobalVars.DegToRadValue * (FromPt.M - ToPt.M);

				if (System.Math.Abs(System.Math.Sin(fTmp)) <= fE && System.Math.Cos(fTmp) > 0.0)
					ptColl.AddPoint(ToPt);
				else
				{
					if (System.Math.Abs(System.Math.Sin(fTmp)) > fE)
						ptConstr.ConstructAngleIntersection(FromPt, GlobalVars.DegToRadValue * (NativeMethods.Modulus(FromPt.M + 90.0, 360.0)),
															ToPt, GlobalVars.DegToRadValue * (NativeMethods.Modulus(ToPt.M + 90.0, 360.0)));
					else
						CntPt.PutCoords(0.5 * (FromPt.X + ToPt.X), 0.5 * (FromPt.Y + ToPt.Y));

					//Graphics.DrawPoint(CntPt, RGB(0, 0, 255));
					//Graphics.DrawPoint(ToPt, RGB(0, 255, 0));

					int Side = (int)SideDef(FromPt, (FromPt.M), ToPt);
					ptColl.AddPointCollection(CreateArcPrj(CntPt, FromPt, ToPt, Side));
				}
				//Graphics.DrawPoint(CntPt, RGB(0, 255, 0));
			}
			return result;
		}

		internal static IPointCollection TurnToFixPrj(IPoint PtSt, double TurnR, int TurnDir, IPoint FixPnt)
		{
			IPoint ptCnt = LocalToPrj(PtSt, PtSt.M + 90.0 * TurnDir, TurnR);

			double DistFx2Cnt = ReturnDistanceInMeters(ptCnt, FixPnt);
			double DirFx2Cnt = ReturnAngleInDegrees(ptCnt, FixPnt);

			double DeltaAngle = -GlobalVars.RadToDegValue * (Math.Acos(TurnR / DistFx2Cnt)) * TurnDir;

			IPoint Pt1 = LocalToPrj(ptCnt, DirFx2Cnt + DeltaAngle, TurnR);
			Pt1.M = ReturnAngleInDegrees(Pt1, FixPnt);

			IPointCollection result = new ESRI.ArcGIS.Geometry.Multipoint();
			result.AddPoint(PtSt);
			result.AddPoint(Pt1);
			return result;
		}

		internal static int AnglesSideDef(double X, double Y)
		{
			double Z = NativeMethods.Modulus(X - Y, 360.0);

			if (Z > 360.0 - GlobalVars.degEps || Z < GlobalVars.degEps)
				return SideDirection.onSide;

			if (Z > 180.0 - GlobalVars.degEps)
				return SideDirection.rightSide;

			if (Z < 180.0 + GlobalVars.degEps)
				return SideDirection.leftSide;

			return 2;
		}

		// ==================================

		internal static Point CircleVectorIntersect(IPoint centerPoint, double radius, IPoint ptVector, double dirVector, out double d)
		{
			dirVector *= GlobalVars.DegToRadValue;
			double v1x = Math.Cos(dirVector);
			double v1y = Math.Sin(dirVector);

			double v2x = centerPoint.X - ptVector.X;
			double v2y = centerPoint.Y - ptVector.Y;

			double distToIntersect = v1x * v2x + v1y * v2y;

			Point ptIntersect = new Point();
			ptIntersect.PutCoords(ptVector.X + v1x * distToIntersect, ptVector.Y + v1y * distToIntersect);

			double dx = centerPoint.X - ptIntersect.X;
			double dy = centerPoint.Y - ptIntersect.Y;

			double distCenterToIntersect2 = dx * dx + dy * dy;
			double radius2 = radius * radius;

			if (distCenterToIntersect2 < radius2)
			{
				d = Math.Sqrt(radius2 - distCenterToIntersect2);

				double xNew = ptVector.X + (d + distToIntersect) * v1x;
				double yNew = ptVector.Y + (d + distToIntersect) * v1y;
				Point result = new Point();
				result.PutCoords(xNew, yNew);
				return result;
			}

			d = -1.0;
			return new Point();	// null;
		}

		internal static Point CircleVectorIntersect(IPoint centerPoint, double radius, IPoint ptVector, double dirVector)
		{
			double fTemp;
			return CircleVectorIntersect(centerPoint, radius, ptVector, dirVector, out fTemp);
		}

		//internal static double CircleVectorIntersect(Point centerPoint, double radius, Point ptVector, double dirVector)
		//{
		//    double fTemp;
		//    CircleVectorIntersect(centerPoint, radius, ptVector, dirVector, out fTemp);
		//    return fTemp;
		//}

		internal static Interval CalcNavInterval(IPoint pPtRef, double refDirection, int TurnDir, IPoint pPtNav, double fIAS, double turnAlt, double bankAngle, double gradient)
		{
			const double maxTurn = GlobalVars.DegToRadValue * GlobalVars.MaxInterceptAngle;

			Interval ResInterval;
			ResInterval.Left = -1.0;
			ResInterval.Right = -1.0;

			double CotanMaxTurn = 1.0 / Math.Tan(maxTurn);
			double fRefX, fRefY;
			Functions.PrjToLocal(pPtRef, refDirection, pPtNav, out fRefX, out fRefY);

			double fTAS0 = Functions.IAS2TAS(fIAS, turnAlt, GlobalVars.m_CurrADHP.ISAtC);
			double fTurnR0 = Functions.Bank2Radius(bankAngle, fTAS0);

			double fDistToIntersect = fRefX - TurnDir * fRefY * CotanMaxTurn;
			double fToIntersect120 = fDistToIntersect;
			double dist120 = fTurnR0 * Math.Tan(GlobalVars.DegToRadValue * 60.0);
			double fDistToTurn = fDistToIntersect - dist120;

			if (fDistToTurn < 0.0)
				fDistToTurn = 0.0;

			turnAlt += fDistToTurn * gradient;

			double fTAS120 = Functions.IAS2TAS(fIAS, turnAlt, GlobalVars.m_CurrADHP.ISAtC);
			double fTurnR120 = Functions.Bank2Radius(bankAngle, fTAS120);

			if (Math.Abs(fRefY) < GlobalVars.distEps)
			{
				if (fRefX < 0.0)
					return ResInterval;

				ResInterval.Left = 0.0;
				ResInterval.Right = Math.Min(2.0 * GlobalVars.RadToDegValue * Math.Atan2(fRefX, fTurnR120), GlobalVars.MaxInterceptAngle);	//SubtractAngles(dirMin, refDirection);
				return ResInterval;
			}

			double denom, D;
			double distMn, distMx;
			double fCorrection = 0.0;

			if (TurnDir * fRefY > 0.0)		//eyni teref
			{
				denom = TurnDir * fRefY - 2.0 * fTurnR120;
				D = fRefX * fRefX + TurnDir * fRefY * denom;

				if (D < 0.0)
					return ResInterval;

				D = Math.Sqrt(D);

				double alpha_1 = 2.0 * Math.Atan2(-fRefX - D, denom);		// +2 * Math.PI;
				double alpha_2 = 2.0 * Math.Atan2(-fRefX + D, denom);		// -2 * Math.PI;

				double fDistToIntersect1 = fRefX - fRefY * TurnDir / Math.Tan(alpha_1);		//TurnDir*
				double fDistToIntersect2 = fRefX - fRefY * TurnDir / Math.Tan(alpha_2);		//TurnDir*

				if (fDistToIntersect1 <= 0.0 && fDistToIntersect2 <= 0.0)
					return ResInterval;

				if (fDistToIntersect1 * fDistToIntersect2 < 0.0)
				{
					distMn = Math.Max(fDistToIntersect1, fDistToIntersect2);
					distMx = Math.Min(fToIntersect120, 0.2 * GlobalVars.MaxNAVDist);
				}
				else
				{
					distMn = Math.Min(fDistToIntersect1, fDistToIntersect2);
					distMx = Math.Min(fToIntersect120, Math.Max(fDistToIntersect1, fDistToIntersect2));
				}

				if (distMx > distMn)
				{
					IPoint ptMin = Functions.LocalToPrj(pPtRef, refDirection, distMn);
					IPoint ptMax = Functions.LocalToPrj(pPtRef, refDirection, distMx);

					double dirMin = fCorrection + Functions.ReturnAngleInDegrees(ptMin, pPtNav);
					double dirMax = fCorrection + Functions.ReturnAngleInDegrees(ptMax, pPtNav);

					double TurnAngle1 = NativeMethods.Modulus(TurnDir * (dirMin - refDirection));	//SubtractAngles(dirMin, refDirection);
					double TurnAngle2 = NativeMethods.Modulus(TurnDir * (dirMax - refDirection));	//SubtractAngles(dirMax, refDirection);

					ResInterval.Left = Math.Min(TurnAngle1, TurnAngle2);
					ResInterval.Right = Math.Max(TurnAngle1, TurnAngle2);
				}
			}
			else						// eks teref
			{
				fCorrection = 180.0;
				denom = -fRefY * TurnDir;
				D = fRefX * fRefX + denom * (denom + 2.0 * fTurnR120);

				D = Math.Sqrt(D);

				double alpha_1 = 2.0 * Math.Atan2(-fRefX - D, denom);		// +2 * Math.PI;
				double alpha_2 = 2.0 * Math.Atan2(-fRefX + D, denom);		// -2 * Math.PI;

				double fDistToIntersect1 = fRefX + fRefY * TurnDir / Math.Tan(alpha_1);		//TurnDir*
				double fDistToIntersect2 = fRefX + fRefY * TurnDir / Math.Tan(alpha_2);		//TurnDir*

				if (fDistToIntersect1 <= 0.0 && fDistToIntersect2 <= 0.0)
					return ResInterval;

				if (fDistToIntersect1 * fDistToIntersect2 < 0.0)
					distMn = Math.Max(Math.Max(fDistToIntersect1, fDistToIntersect2), fToIntersect120);
				else
					distMn = Math.Max(Math.Min(fDistToIntersect1, fDistToIntersect2), fToIntersect120);

				distMx = 0.2 * GlobalVars.MaxNAVDist;

				if (distMx > distMn)
				{
					IPoint ptMin = Functions.LocalToPrj(pPtRef, refDirection, distMn);
					IPoint ptMax = Functions.LocalToPrj(pPtRef, refDirection, distMx);

					double dirMin = Functions.ReturnAngleInDegrees(pPtNav, ptMin);
					double dirMax = Functions.ReturnAngleInDegrees(pPtNav, ptMax);
					//double dirMin = fCorrection + Functions.ReturnAngleInDegrees(ptMin, pPtNav);
					//double dirMax = fCorrection + Functions.ReturnAngleInDegrees(ptMax, pPtNav);

					double TurnAngle1 = NativeMethods.Modulus(TurnDir * (dirMin - refDirection));	//SubtractAngles(dirMin, refDirection);
					double TurnAngle2 = NativeMethods.Modulus(TurnDir * (dirMax - refDirection));	//SubtractAngles(dirMax, refDirection);

					ResInterval.Left = Math.Min(TurnAngle1, TurnAngle2);
					ResInterval.Right = Math.Max(TurnAngle1, TurnAngle2);
				}
			}
			return ResInterval;
		}

		internal static int GetObstaclesInPoly(ObstacleData[] AllObsstacles, out ObstacleData[] resultObsstacles, IPolygon testPoly)
		{
			int j = 0, n = AllObsstacles.Length;
			resultObsstacles = new ObstacleData[n];

			IRelationalOperator pReleation = (IRelationalOperator)testPoly;

			for (int i = 0; i < n; i++)
				if (pReleation.Contains(AllObsstacles[i].pPtPrj))
					resultObsstacles[j++] = AllObsstacles[i];

			System.Array.Resize<ObstacleData>(ref resultObsstacles, j);
			return j;
		}

		internal static int GetCourseInterceptLegObstacles(ObstacleData[] AllObsstacles, ref TrackLeg leg)
		{
			//double PDGNet = leg.WorstCase.NetGrd;
			//IConstructPoint pConstructPoint;
			//IPoint P0, P1, P2;//, pTch, ptTmp;
			//P0 = new Point();
			//P1 = leg.BestCase.Turn[0].ptCenter;
			//P2 = leg.WorstCase.Turn[0].ptCenter;

			//double currDir = leg.ptStart.BestCase.Direction;
			//int TurnDir = leg.BestCase.Turn[0].TurnDir, bestSide = (int)SideDef(leg.ptStart.BestCase.pPoint, currDir, leg.ptStart.WorstCase.pPoint);

			//double R = leg.BestCase.Turn[0].Radius, Rb, b;
			//Rb = R + b;

			//double x0, y0,  ycSign, xc1;//xc, 
			//R = 0.5 * (leg.BestCase.Turn1.Radius + leg.WorstCase.Turn1.Radius);
			//double xDir = ReturnAngleInDegrees(P1, P2);
			//double xDist = ReturnDistanceInMeters(P1, P2);
			//PrjToLocal(P1, xDir, leg.ptStart.BestCase.pPoint, out x0, out ycSign);
			//double alpha, betha, theta, fTmpd, x, ;		//, betha = leg.ptStart.BestCase.Direction; // leg.ptStart.WorstCase.Direction;
			//double Pb = BestGr;
			//double dHb = leg.ptEnd.BestCase.NetHeight - leg.ptStart.BestCase.NetHeight;
			//double dHw = leg.ptEnd.WorstCase.NetHeight - leg.ptStart.WorstCase.NetHeight;

			//double SinD = Math.Sin(GlobalVars.DegToRadValue * SubtractAngles(currDir, xDir));
			//if (SinD > 0.0)
			//    SinD = 1.0 / SinD;
			//else
			//    SinD = 2.0;

			//double BaseDir = ReturnAngleInDegrees(leg.ptStart.BestCase.pPoint, leg.ptStart.WorstCase.pPoint);
			//double BaseDist = ReturnDistanceInMeters(leg.ptStart.BestCase.pPoint, leg.ptStart.WorstCase.pPoint);
			//double InvBaseDist = 0.0;
			//if (BaseDist > 0)
			//    InvBaseDist = 1.0 / BaseDist;
			//else
			//    BaseDir = currDir + 90 * TurnDir;


			IRelationalOperator pReleation = (IRelationalOperator)(leg.pProtectionArea);
			double b = 0.5 * (leg.ptEnd.BestCase.Width + leg.ptEnd.WorstCase.Width);
			double BestGr = leg.BestCase.NetGrd;
			double WorststGr = leg.WorstCase.NetGrd;
			double dGr = WorststGr - BestGr, dDst = leg.WorstCase.PrevTotalLength - leg.BestCase.PrevTotalLength;

			//=================================================================

			IClone pClone = (IClone)leg.BestCase.pNominalPoly;
			IPolyline pBestPolyline = (IPolyline)pClone.Clone();

			ITopologicalOperator2 pBestTopo = (ITopologicalOperator2)pBestPolyline;
			pBestTopo.IsKnownSimple_2 = false;
			pBestTopo.Simplify();

			IMAware pMAware = (IMAware)pBestPolyline;
			pMAware.MAware = true;

			IMSegmentation pBestMSegmentation = (IMSegmentation)pBestPolyline;
			pBestMSegmentation.CalculateNonSimpleMs();
			pBestMSegmentation.SetAndInterpolateMsBetween(0, pBestPolyline.Length);

			IProximityOperator pBestProxi = (IProximityOperator)pBestPolyline;
			IProximityOperator pBestNomReleation = (IProximityOperator)(pBestPolyline.FromPoint);
			//=================================================================
			pClone = (IClone)leg.WorstCase.pNominalPoly;
			IPolyline pWorstPolyline = (IPolyline)pClone.Clone();

			ITopologicalOperator2 pWorstTopo = (ITopologicalOperator2)pWorstPolyline;
			pWorstTopo.IsKnownSimple_2 = false;
			pWorstTopo.Simplify();

			pMAware = (IMAware)pWorstPolyline;
			pMAware.MAware = true;

			IMSegmentation pWorstMSegmentation = (IMSegmentation)pWorstPolyline;
			pWorstMSegmentation.CalculateNonSimpleMs();
			//pWorstMSegmentation.SetMsAsDistance(true);
			pWorstMSegmentation.SetAndInterpolateMsBetween(0, pWorstPolyline.Length);

			IProximityOperator pWorstProxi = (IProximityOperator)(pWorstPolyline);
			IProximityOperator pWorstNomReleation = (IProximityOperator)(pWorstPolyline.FromPoint);
			//=================================================================

			//IPolyline pPolyline = (IPolyline)new Polyline();
			//pPolyline.FromPoint = leg.BestCase.Turn1.ptCenter;
			//pPolyline.ToPoint = leg.WorstCase.Turn1.ptCenter;

			//Graphics.DrawPolyline(pPolyline, -1);
			//Graphics.DrawPointWithText(leg.BestCase.Turn1.ptCenter, "O1");
			//Graphics.DrawPointWithText(leg.WorstCase.Turn1.ptCenter, "O2");
			//ProcessMessages();

			//==========================================================================================

			leg.ObsMaxNetGrd = -1;
			leg.ObsMaxAcceleDist = -1;

			int j = 0, n = AllObsstacles.Length;
			leg.ObstacleList = new ObstacleData[n];

			for (int i = 0; i < n; i++)
			{
				IPoint pCurrPt = AllObsstacles[i].pPtPrj;

				if (pReleation.Disjoint(pCurrPt))
					continue;

				leg.ObstacleList[j] = AllObsstacles[i];

				IPoint ptBestNearest = pBestProxi.ReturnNearestPoint(pCurrPt, esriSegmentExtension.esriNoExtension);
				IPoint ptWorstNearest = pWorstProxi.ReturnNearestPoint(pCurrPt, esriSegmentExtension.esriNoExtension);
				//IPoint ptBestNearest = pBestProxi.ReturnNearestPoint(pCurrPt, esriSegmentExtension.esriExtendEmbedded);
				//IPoint ptWorstNearest = pWorstProxi.ReturnNearestPoint(pCurrPt, esriSegmentExtension.esriExtendEmbedded);

				double BestDist = ReturnDistanceInMeters(ptBestNearest, pCurrPt);
				double WorstDist = ReturnDistanceInMeters(ptWorstNearest, pCurrPt);
				double TotalDist = ReturnDistanceInMeters(ptBestNearest, ptWorstNearest);

				//Graphics.DrawPointWithText(ptBestNearest, "pt-Best");
				//Graphics.DrawPointWithText(ptWorstNearest, "pt-Worst");
				//ProcessMessages();

				//if (AllObsstacles[i].ID == "Ter-7443")
				//{
				//    ArcLen = 0.0;
				//}

				//double xc = dSum;
				//double ArcLen = 0.0;
				//xf = TotalDist > GlobalVars.distEps ? x0 : BaseDist;

				double dSum = WorstDist + BestDist;
				double k = 1.0;
				double L = ptWorstNearest.M;
				double Lb = ptBestNearest.M;
				bool onCircumCircle = ((L > leg.WorstCase.Turn[0].StartDist) || (Lb > leg.BestCase.Turn[0].StartDist)) &&
										(L < leg.WorstCase.Turn[0].StartDist + leg.WorstCase.Turn[0].Length) &&
										(Lb < leg.BestCase.Turn[0].StartDist + leg.BestCase.Turn[0].Length);


				if (WorstDist < leg.ptEnd.WorstCase.Width)	//BaseDist
				{
					//xc = xDist;
					//theta = leg.ptEnd.WorstCase.Direction;
				}
				else if (WorstDist > TotalDist || BestDist > TotalDist)//BaseDist
				{
					if (TotalDist < GlobalVars.distEps)
					{
						//xc = xDist;
						//theta = leg.ptEnd.WorstCase.Direction;
					}
					else if (WorstDist > BestDist)
					{
						//xc = 0.0;
						//theta = leg.ptEnd.BestCase.Direction;
						k = 0.0;
						L = ptBestNearest.M;
						onCircumCircle = L > leg.BestCase.Turn[0].StartDist;
					}
					else
					{
						//xc = xDist;
						//theta = leg.ptEnd.WorstCase.Direction;
					}
				}
				else
				{
					//theta = ReturnAngleInDegrees(pCurrPt, leg.ptEnd.BestCase.pPoint);
					//alpha = DegToRad(NativeMethods.Modulus(theta - xDir));
					//fTmp = y0 / Math.Tan(alpha) - Rb / Math.Sin(alpha) * TurnDir;
					//xc = x0 - fTmp;
					//fTmp = ReturnDistanceInMeters(pCurrPt, leg.ptEnd.BestCase.pPoint);
					//if (xc < 0 || xc >= xDist || fTmp < leg.ptEnd.WorstCase.Width)
					//x0 = BestDist;
					//double fTmp = BestGr + dGr * BestDist / TotalDist;
					//theta = ReturnAngleInDegrees(pCurrPt, leg.ptEnd.BestCase.pPoint);
					//xc = xDist *

					k = BestDist / dSum;
					L = ptBestNearest.M + (ptWorstNearest.M - ptBestNearest.M) * k;
					double StartDist = leg.BestCase.Turn[0].StartDist + (leg.WorstCase.Turn[0].StartDist - leg.BestCase.Turn[0].StartDist) * k;

					onCircumCircle = L > StartDist;
				}

#if experti
				if (false)
				{
					ptTmp = LocalToPrj(pCurrPt, currDir - 90.0 * TurnDir, Rb);
					PrjToLocal(P1, xDir, ptTmp, out xc1, out fTmp);

					//onCircumCircle = ycSign * y0 < 0;
					//Graphics.DrawPointWithText(ptTmp, AllObsstacles[i].ID + "-t");
					//Graphics.DrawPointWithText(pCurrPt, "pCurrPt");
					//ProcessMessages();

					//onCircumCircle = false;
					ArcLen = 0.0;
					pTch = pCurrPt;

					if (ycSign * fTmp < 0)
					{
						PrjToLocal(P1, xDir, pCurrPt, out x0, out y0);

						d = Rb * Rb - y0 * y0;
						x = Math.Sqrt(d);

						xc = (x0 - x);
						fTmp = x0 + x;

						if (xc < -SinD * leg.ptStart.BestCase.Width || xc > BaseDist + SinD * leg.ptStart.WorstCase.Width)
						{
							if (fTmp >= -SinD * leg.ptStart.BestCase.Width && fTmp <= BaseDist + SinD * leg.ptStart.WorstCase.Width)
								xc = fTmp;
						}
						else
						{
							if (fTmp >= -SinD * leg.ptStart.BestCase.Width && fTmp <= BaseDist + SinD * leg.ptStart.WorstCase.Width)
								xc = Math.Max(xc, fTmp);
						}

						if (xc >= -SinD * leg.ptStart.BestCase.Width && xc <= BaseDist + SinD * leg.ptStart.WorstCase.Width)
						{
							//ptTmp = LocalToPrj(P1, xDir, Math.Max(xc, fTmp));
							//pTch = LocalToPrj(ptTmp, leg.ptStart.BestCase.Direction + 90.0 * TurnDir, Rb);
							//Graphics.DrawPointWithText(pTch, "1");
							//Graphics.DrawPolygon(CreateCirclePrj(ptTmp, Rb));
							//Graphics.DrawPointWithText(ptTmp, "1");
							//ProcessMessages();

							P0 = LocalToPrj(P1, xDir, xc);
							pTch = LocalToPrj(P0, currDir + 90.0 * TurnDir, Rb);

							//Graphics.DrawPointWithText(pTch, "pTch");
							//Graphics.DrawPolygon(CreateCirclePrj(P0, Rb));
							//Graphics.DrawPointWithText(P0, "cnt1");
							//Graphics.DrawPolyline(leg.BestCase.pNominalPoly);
							//Graphics.DrawPolyline(leg.WorstCase.pNominalPoly);
							//ProcessMessages();

							double fdX = pCurrPt.X - P0.X;
							double fdY = pCurrPt.Y - P0.Y;
							betha = RadToDeg(Math.Atan2(fdY, fdX));
							theta = betha - 90.0 * TurnDir;

							alpha = NativeMethods.Modulus((currDir - theta) * TurnDir);
							ArcLen = R * DegToRad(alpha);
						}
					}

					//BaseDir = currDir + 90 * TurnDir;
					pConstructPoint = (IConstructPoint)P0;
					pConstructPoint.ConstructAngleIntersection(leg.ptStart.BestCase.pPoint, GlobalVars.DegToRadValue * BaseDir, pTch, GlobalVars.DegToRadValue * currDir);


					if (P0.IsEmpty)
					{
						PrjToLocal(leg.ptStart.BestCase.pPoint, BaseDir, pCurrPt, out x0, out y0);
						L = ReturnDistanceInMeters(leg.ptStart.BestCase.pPoint, pCurrPt);
					}
					else
					{
						PrjToLocal(leg.ptStart.BestCase.pPoint, BaseDir, P0, out x0, out y0);
						L = ReturnDistanceInMeters(pTch, P0);
					}
				}
#endif

				//if (onCircumCircle)
				//{
				//    Graphics.DrawPointWithText(P0, AllObsstacles[i].ID);
				//    ProcessMessages();
				//}


				leg.ObstacleList[j].X = L ;
				leg.ObstacleList[j].ApplNetGradient2 = BestGr + dGr * k;

				//Graphics.DrawPolyline(pLinePart, 255, 2);
				//Graphics.DrawPointWithText(leg.ObstacleList[j].pPtPrj, "leg.Obst");
				//PrjToLocal(SelectedRWY.pPtPrj[eRWY.PtDER], SelectedRWY.pPtPrj[eRWY.PtDER].M, leg.ObstacleList[j].pPtPrj, out leg.ObstacleList[j].X, out leg.ObstacleList[j].Y);

				leg.ObstacleList[j].Y = b;
				//if(prevlrgOnHeight)
				//	leg.ObstacleList[j].TotalDist = leg.BestCase.PrevTotalLength + leg.ObstacleList[j].X;
				//else

				leg.ObstacleList[j].TotalDist = leg.BestCase.PrevTotalLength + dDst * k + leg.ObstacleList[j].X;

				leg.ObstacleList[j].MOC = GlobalVars.NetMOC;
				if (onCircumCircle)
					leg.ObstacleList[j].MOC = GlobalVars.TurnMOC;				//

				leg.ObstacleList[j].MOCH = leg.ObstacleList[j].Height + leg.ObstacleList[j].MOC;

				double LostH = leg.ObstacleList[j].MOCH - GlobalVars.heightAboveDERMax;
				leg.ObstacleList[j].ReqNetGradient = LostH / leg.ObstacleList[j].TotalDist;

				double PDGNet = leg.ObstacleList[j].ApplNetGradient2;		//????????????????????????????????????????????????????????????
				leg.ObstacleList[j].AcceleStartDist = LostH / PDGNet;

				//Graphics.DrawPolyline(pLinePart, 255, 2);
				/*{
					if (ReturnDistanceInMeters(pLinePart.FromPoint, leg.pNominalPoly.FromPoint) > ReturnDistanceInMeters(pLinePart.ToPoint, leg.pNominalPoly.FromPoint))
						pLinePart.ReverseOrientation();
				}*/

				if (leg.ObsMaxNetGrd < 0)
					leg.ObsMaxNetGrd = j;
				else if (leg.ObstacleList[j].ReqNetGradient > leg.ObstacleList[leg.ObsMaxNetGrd].ReqNetGradient)
					leg.ObsMaxNetGrd = j;

				if (leg.ObsMaxAcceleDist < 0)
					leg.ObsMaxAcceleDist = j;
				else if (leg.ObstacleList[j].AcceleStartDist > leg.ObstacleList[leg.ObsMaxAcceleDist].AcceleStartDist)
					leg.ObsMaxAcceleDist = j;

				j++;
			}

			System.Array.Resize<ObstacleData>(ref leg.ObstacleList, j);
			return j;
		}

		internal static int GetLegObstaclesNoTurn(ObstacleData[] AllObsstacles, ref TrackLeg leg)
		{
			double PDGNet = leg.WorstCase.NetGrd;
			IRelationalOperator pAreaReleation = (IRelationalOperator)(leg.pProtectionArea);
			//=================================================================

			IClone pClone = (IClone)leg.BestCase.pNominalPoly;
			IPolyline pBestPolyline = (IPolyline)pClone.Clone();

			ITopologicalOperator2 pBestTopo = (ITopologicalOperator2)pBestPolyline;
			pBestTopo.IsKnownSimple_2 = false;
			pBestTopo.Simplify();

			IMAware pMAware = (IMAware)pBestPolyline;
			pMAware.MAware = true;

			IMSegmentation pBestMSegmentation = (IMSegmentation)pBestPolyline;
			pBestMSegmentation.CalculateNonSimpleMs();
			pBestMSegmentation.SetAndInterpolateMsBetween(0, pBestPolyline.Length);

			IProximityOperator pBestProxi = (IProximityOperator)pBestPolyline;
			IProximityOperator pBestNomReleation = (IProximityOperator)(pBestPolyline.FromPoint);
			//=================================================================
			pClone = (IClone)leg.WorstCase.pNominalPoly;
			IPolyline pWorstPolyline = (IPolyline)pClone.Clone();

			ITopologicalOperator2 pWorstTopo = (ITopologicalOperator2)pWorstPolyline;
			pWorstTopo.IsKnownSimple_2 = false;
			pWorstTopo.Simplify();

			pMAware = (IMAware)pWorstPolyline;
			pMAware.MAware = true;

			IMSegmentation pWorstMSegmentation = (IMSegmentation)pWorstPolyline;
			pWorstMSegmentation.CalculateNonSimpleMs();
			//pWorstMSegmentation.SetMsAsDistance(true);
			pWorstMSegmentation.SetAndInterpolateMsBetween(0, pWorstPolyline.Length);

			IProximityOperator pWorstProxi = (IProximityOperator)(pWorstPolyline);
			IProximityOperator pWorstNomReleation = (IProximityOperator)(pWorstPolyline.FromPoint);
			//=================================================================

			double BestStartDist1, BestStartDist2 = 0;
			double BestEndDist1, BestEndDist2;

			if (leg.SegmentCode == eLegType.arcPath)
			{
				BestStartDist1 = leg.BestCase.Turn[0].StartDist;
				BestEndDist1 = BestStartDist1 + leg.BestCase.Turn[0].Length;
				BestStartDist2 = BestEndDist1;		// leg.BestCase.Turn[1].StartDist;
				BestEndDist2 = BestStartDist2 + leg.BestCase.Turn[1].Length;
			}

			leg.ObsMaxNetGrd = -1;
			leg.ObsMaxAcceleDist = -1;

			IPoint P1 = leg.ptStart.BestCase.pPoint;
			IPoint P2 = leg.ptStart.WorstCase.pPoint;

			double BaseDir = ReturnAngleInDegrees(P1, P2);
			double BaseDist = ReturnDistanceInMeters(P1, P2);

			double InvBaseDist = 0.0;
			if (BaseDist > 0)
				InvBaseDist = 1.0 / BaseDist;

			double BestGr = leg.BestCase.NetGrd;
			double WorstGr = leg.WorstCase.NetGrd;
			double dGr = WorstGr - BestGr, dDst = leg.WorstCase.PrevTotalLength - leg.BestCase.PrevTotalLength;

			//double x0, y0;
			//double xDir = ReturnAngleInDegrees(P1, P2);
			//double xDist = ReturnDistanceInMeters(P1, P2);
			//PrjToLocal(P1, xDir, leg.ptStart.BestCase.pPoint, out x0, out ycSign);

			int j = 0, n = AllObsstacles.Length;
			leg.ObstacleList = new ObstacleData[n];

			for (int i = 0; i < n; i++)
			{
				IPoint pCurrPt = AllObsstacles[i].pPtPrj;

				if (pAreaReleation.Disjoint(pCurrPt))
					continue;

				leg.ObstacleList[j] = AllObsstacles[i];

				IPoint ptBestNearest = pBestProxi.ReturnNearestPoint(pCurrPt, esriSegmentExtension.esriNoExtension);
				IPoint ptWorstNearest = pWorstProxi.ReturnNearestPoint(pCurrPt, esriSegmentExtension.esriNoExtension);

				double BestDist = ReturnDistanceInMeters(ptBestNearest, pCurrPt);
				double WorstDist = ReturnDistanceInMeters(ptWorstNearest, pCurrPt);
				double TotalDist = ReturnDistanceInMeters(ptBestNearest, ptWorstNearest);


				if (TotalDist < 40.0)
					leg.ObstacleList[j].ApplNetGradient2 = WorstGr;
				else if (WorstDist > TotalDist || BestDist > TotalDist)//BaseDist
				{
					if (BaseDist < GlobalVars.distEps)
						leg.ObstacleList[j].ApplNetGradient2 = WorstGr;
					else
						leg.ObstacleList[j].ApplNetGradient2 = WorstDist > BestDist ? BestGr : WorstGr;
				}
				else
				{
					leg.ObstacleList[j].ApplNetGradient2 = BestGr + dGr * BestDist / TotalDist; //BestGr + dGr * BestDist * InvBaseDist;
				}

				leg.ObstacleList[j].X = Math.Max(ptBestNearest.M, ptWorstNearest.M);			// pBestLinePart.Length;
				leg.ObstacleList[j].Y = pBestProxi.ReturnDistance(pCurrPt);

				leg.ObstacleList[j].TotalDist = leg.BestCase.PrevTotalLength + leg.ObstacleList[j].X;

				leg.ObstacleList[j].MOC = GlobalVars.NetMOC;

				if (leg.SegmentCode == eLegType.arcPath && leg.ObstacleList[j].X > BestStartDist2)
					leg.ObstacleList[j].MOC = GlobalVars.TurnMOC;

				leg.ObstacleList[j].MOCH = leg.ObstacleList[j].Height + leg.ObstacleList[j].MOC;

				double LostH = leg.ObstacleList[j].MOCH - GlobalVars.heightAboveDERMax;
				leg.ObstacleList[j].ReqNetGradient = LostH / leg.ObstacleList[j].TotalDist;

				PDGNet = leg.ObstacleList[j].ApplNetGradient2;			// ????????????????????
				leg.ObstacleList[j].AcceleStartDist = LostH / PDGNet;

				//Graphics.DrawPointWithText(pCurrPt, "Obst");
				//Graphics.DrawPointWithText(ptBestNearest, "Bestpt");
				//Graphics.DrawPointWithText(ptWorstNearest, "Worstpt");
				//ProcessMessages();

				if (leg.ObsMaxNetGrd < 0)
					leg.ObsMaxNetGrd = j;
				else if (leg.ObstacleList[j].ReqNetGradient > leg.ObstacleList[leg.ObsMaxNetGrd].ReqNetGradient)
					leg.ObsMaxNetGrd = j;

				if (leg.ObsMaxAcceleDist < 0)
					leg.ObsMaxAcceleDist = j;
				else if (leg.ObstacleList[j].AcceleStartDist > leg.ObstacleList[leg.ObsMaxAcceleDist].AcceleStartDist)
					leg.ObsMaxAcceleDist = j;

				j++;
			}

			System.Array.Resize<ObstacleData>(ref leg.ObstacleList, j);
			return j;
		}

		//internal static int GetDMEArcLegObstacles(ObstacleData[] AllObsstacles, ref TrakceLeg leg)
		//{
		//    double PDGNet = leg.WorstCase.NetGrd;
		//    IRelationalOperator pReleation = (IRelationalOperator)(leg.pProtectionArea);

		//    leg.ObsMaxNetGrd = -1;
		//    leg.ObsMaxAcceleDist = -1;

		//    int j = 0, n = AllObsstacles.Length;

		//    leg.ObstacleList = new ObstacleData[n];

		//    for (int i = 0; i < n; i++)
		//    {
		//        IPoint pCurrPt = AllObsstacles[i].pPtPrj;
		//        if (pReleation.Disjoint(pCurrPt))
		//            continue;

		//        leg.ObstacleList[j] = AllObsstacles[i];


		//        ptTmp = LocalToPrj(pCurrPt, currDir2 - 90.0 * TurnDir2, Rb2);
		//        PrjToLocal(P11, xDir2, ptTmp, out xc, out fTmp);

		//        onCircumCircle2 = ycSign * fTmp < 0;

		//        //onCircumCircle = ycSign * y0 < 0;
		//        //Graphics.DrawPointWithText(ptTmp, AllObsstacles[i].ID + "-t");
		//        //Graphics.DrawPointWithText(pCurrPt, "pCurrPt");
		//        //ProcessMessages();

		//        if (onCircumCircle2)
		//        {
		//            PrjToLocal(P11, xDir2, pCurrPt, out x0, out y0);

		//            d = Rb2 * Rb2 - y0 * y0;
		//            x = Math.Sqrt(d);

		//            xc = (x0 - x);
		//            fTmp = x0 + x;

		//            if (xc < 0.0)
		//                xc = fTmp;
		//            else if (fTmp < 1.01 * xDist2)
		//            {
		//                if (bestSide2 * TurnDir2 >= 0)
		//                    xc = Math.Min(xc, fTmp);
		//                else
		//                    xc = Math.Max(xc, fTmp);

		//                //ptTmp = LocalToPrj(P1, xDir, Math.Max(xc, fTmp));
		//                //pTch = LocalToPrj(ptTmp, leg.ptStart.BestCase.Direction + 90.0 * TurnDir, Rb);
		//                //Graphics.DrawPointWithText(pTch, "1");
		//                //Graphics.DrawPolygon(CreateCirclePrj(ptTmp, Rb));
		//                //Graphics.DrawPointWithText(ptTmp, "1");
		//                //ProcessMessages();
		//            }

		//            P0 = LocalToPrj(P11, xDir2, xc);
		//            pTch = LocalToPrj(P0, currDir2 + 90.0 * TurnDir2, Rb2);

		//            //Graphics.DrawPointWithText(pTch, "2");
		//            //Graphics.DrawPolygon(CreateCirclePrj(P0, Rb2));
		//            //Graphics.DrawPointWithText(P0, "2");
		//            //ProcessMessages();

		//            double fdX = pCurrPt.X - P0.X;
		//            double fdY = pCurrPt.Y - P0.Y;
		//            betha = RadToDeg(Math.Atan2(fdY, fdX));
		//            theta = betha - 90.0 * TurnDir2;

		//            alpha = NativeMethods.Modulus((currDir2 - theta) * TurnDir2);
		//            ArcLen2 = R2 * DegToRad(alpha);
		//        }
		//        else
		//        {
		//            pTch = pCurrPt;
		//            ArcLen2 = 0.0;
		//        }

		//        //IConstructPoint pConstructPoint = (IConstructPoint)P0;
		//        //pConstructPoint.ConstructAngleIntersection(leg.ptStart.BestCase.pPoint, GlobalVars.DegToRadValue * BaseDir, pTch, GlobalVars.DegToRadValue * currDir2);

		//        ////if (onCircumCircle)
		//        ////{
		//        ////    Graphics.DrawPointWithText(P0, AllObsstacles[i].ID);
		//        ////    ProcessMessages();
		//        ////}

		//        //PrjToLocal(leg.ptStart.BestCase.pPoint, BaseDir, P0, out x0, out y0);
		//        //L = ReturnDistanceInMeters(pTch, P0);

		//        PrjToLocal(P01, xDir1, pTch, out x0, out y0);

		//        d = Rb1 * Rb1 - y0 * y0;
		//        onCircumCircle1 = d >= 0;

		//        if (onCircumCircle1)
		//        {
		//            x = Math.Sqrt(d);

		//            xc = (x0 - x);
		//            fTmp = x0 + x;

		//            if (xc < 0.0)
		//                xc = fTmp;
		//            else if (fTmp < 1.01 * xDist1)
		//            {
		//                if (bestSide1 >= 0)
		//                    xc = Math.Min(xc, fTmp);
		//                else
		//                    xc = Math.Max(xc, fTmp);

		//                //ptTmp = LocalToPrj(P1, xDir, Math.Max(xc, fTmp));
		//                //pTch = LocalToPrj(ptTmp, leg.ptStart.BestCase.Direction + 90.0 * TurnDir, Rb);
		//                //Graphics.DrawPointWithText(pTch, "1");
		//                //Graphics.DrawPolygon(CreateCirclePrj(ptTmp, Rb));
		//                //Graphics.DrawPointWithText(ptTmp, "1");
		//                //ProcessMessages();
		//            }

		//            //xc = x0 - x;			//, если x0 - L ≥ 0, в противном случае,
		//            //if (xc < 0)
		//            //    xc = x0 + x;

		//            double dirInRadian = GlobalVars.DegToRadValue * xDir1;
		//            double fdX = pTch.X - P01.X - xc * Math.Cos(dirInRadian);
		//            double fdY = pTch.Y - P01.Y - xc * Math.Sin(dirInRadian);
		//            betha = RadToDeg(Math.Atan2(fdY, fdX));
		//            theta = betha - 90.0 * TurnDir1;


		//            P0 = LocalToPrj(P01, xDir1, xc);
		//            ptTmp = LocalToPrj(P0, leg.ptStart.BestCase.Direction + 90.0 * TurnDir1, Rb1);
		//            //Graphics.DrawPointWithText(ptTmp, "1");
		//            //Graphics.DrawPolygon(CreateCirclePrj(P0, Rb1));
		//            //Graphics.DrawPointWithText(P0, "1");
		//        }
		//        else
		//        {
		//            theta = ReturnAngleInDegrees(pTch, leg.ptEnd.BestCase.pPoint);
		//            alpha = DegToRad(NativeMethods.Modulus(theta - xDir1));
		//            fTmp = y0 / Math.Tan(alpha) - Rb1 / Math.Sin(alpha) * TurnDir1;
		//            xc = x0 - fTmp;
		//        }

		//        //if (onCircumCircle1)
		//        //{
		//        //    P0 = LocalToPrj(P1, xDir, xc);
		//        //    IPoint pTch = LocalToPrj(P0, leg.ptStart.BestCase.Direction + 90.0 * TurnDir, Rb);

		//        //    Graphics.DrawPointWithText(pTch, "");
		//        //    Graphics.DrawPolygon(CreateCirclePrj(P0, Rb));
		//        //    Graphics.DrawPointWithText(P0, "");
		//        //    ProcessMessages();
		//        //}

		//        alpha = NativeMethods.Modulus((leg.ptStart.BestCase.Direction - theta) * TurnDir1);
		//        ArcLen1 = R1 * DegToRad(alpha);

		//        L = 0.0;
		//        if (!onCircumCircle1)
		//        {
		//            P0 = LocalToPrj(P01, xDir1, xc);

		//            x = ReturnDistanceInMeters(P0, pTch);
		//            L = Math.Sqrt(x * x - Rb1 * Rb1);
		//        }

		//        leg.ObstacleList[j].X = L + ArcLen1 + ArcLen2;
		//        leg.ObstacleList[j].ApplNetGradient = BestGr + dGr * xc * InvXdist;

		//        leg.ObstacleList[j].Y = b;
		//        //if(prevlrgOnHeight)
		//        //	leg.ObstacleList[j].TotalDist = leg.BestCase.PrevTotalLength + leg.ObstacleList[j].X;
		//        //else

		//        leg.ObstacleList[j].TotalDist = leg.BestCase.PrevTotalLength + dDst * xc * InvXdist + leg.ObstacleList[j].X;

		//        leg.ObstacleList[j].MOC = GlobalVars.TurnMOC;				//GlobalVars.NetMOC;
		//        leg.ObstacleList[j].MOCH = leg.ObstacleList[j].Height + leg.ObstacleList[j].MOC;

		//        double LostH = leg.ObstacleList[j].MOCH - GlobalVars.heightAboveDERMax;
		//        leg.ObstacleList[j].ReqNetGradient = LostH / leg.ObstacleList[j].TotalDist;
		//		PDGNet = leg.ObstacleList[j].ApplNetGradient;		//????????????????????????????????????????????????????????????
		//        leg.ObstacleList[j].AcceleDist = LostH / PDGNet;

		//        if (leg.ObsMaxNetGrd < 0)
		//            leg.ObsMaxNetGrd = j;
		//        else if (leg.ObstacleList[j].ReqNetGradient > leg.ObstacleList[leg.ObsMaxNetGrd].ReqNetGradient)
		//            leg.ObsMaxNetGrd = j;

		//        if (leg.ObsMaxAcceleDist < 0)
		//            leg.ObsMaxAcceleDist = j;
		//        else if (leg.ObstacleList[j].AcceleDist > leg.ObstacleList[leg.ObsMaxAcceleDist].AcceleDist)
		//            leg.ObsMaxAcceleDist = j;

		//        j++;
		//    }

		//    System.Array.Resize<ObstacleData>(ref leg.ObstacleList, j);
		//    return j;
		//}

		internal static int GetLegObstacles1TurnS(ObstacleData[] AllObsstacles, ref TrackLeg leg)
		{
			double PDGNet = leg.WorstCase.NetGrd;

			IRelationalOperator pReleation = (IRelationalOperator)(leg.pProtectionArea);

			IPoint pCurrPt, P1, P2;//, P0;
			P1 = leg.BestCase.Turn[0].ptCenter;
			P2 = leg.WorstCase.Turn[0].ptCenter;

			double x0, y0, R = leg.BestCase.Turn[0].Radius, Rb, b, L, xc, ArcLen;
			double currDir = leg.ptStart.BestCase.Direction;
			double xDir = ReturnAngleInDegrees(P1, P2);
			double BaseDir = ReturnAngleInDegrees(leg.ptStart.BestCase.pPoint, leg.ptStart.WorstCase.pPoint);

			double xDist = ReturnDistanceInMeters(P1, P2);

			double InvXdist = 0.0;	// ReturnDistanceInMeters(P1, P2);
			if (xDist > 0)
				InvXdist = 1.0 / xDist;

			//R = 0.5 * (leg.BestCase.Turn1.Radius + leg.WorstCase.Turn1.Radius);
			b = 0.5 * (leg.ptEnd.BestCase.Width + leg.ptEnd.WorstCase.Width);
			Rb = R + b;

			//double alpha, betha, theta, fTmp;		//, betha = leg.ptStart.BestCase.Direction; // leg.ptStart.WorstCase.Direction;
			bool onCircumCircle;

			double BestGr = leg.BestCase.NetGrd;
			double WorststGr = leg.WorstCase.NetGrd;
			double dGr = WorststGr - BestGr, dDst = leg.WorstCase.PrevTotalLength - leg.BestCase.PrevTotalLength;//, d, x;

			//double Pb = BestGr;
			//double dHb = leg.ptEnd.BestCase.NetHeight - leg.ptStart.BestCase.NetHeight;
			//double dHw = leg.ptEnd.WorstCase.NetHeight - leg.ptStart.WorstCase.NetHeight;

			int TurnDir = leg.BestCase.Turn[0].TurnDir;
			PrjToLocal(leg.ptStart.BestCase.pPoint, currDir, leg.ptStart.WorstCase.pPoint, out x0, out y0);

			int bestSide = Math.Sign(x0);

			IClone pClone = (IClone)leg.BestCase.pNominalPoly;
			IPolyline pBestPolyline = (IPolyline)pClone.Clone();

			ITopologicalOperator2 pBestTopo = (ITopologicalOperator2)pBestPolyline;
			pBestTopo.IsKnownSimple_2 = false;
			pBestTopo.Simplify();

			IMAware pMAware = (IMAware)pBestPolyline;
			pMAware.MAware = true;

			IMSegmentation pBestMSegmentation = (IMSegmentation)pBestPolyline;
			pBestMSegmentation.CalculateNonSimpleMs();
			pBestMSegmentation.SetAndInterpolateMsBetween(0, pBestPolyline.Length);

			IProximityOperator pBestProxi = (IProximityOperator)pBestPolyline;
			IProximityOperator pBestNomReleation = (IProximityOperator)(pBestPolyline.FromPoint);
			//=================================================================
			pClone = (IClone)leg.WorstCase.pNominalPoly;
			IPolyline pWorstPolyline = (IPolyline)pClone.Clone();

			ITopologicalOperator2 pWorstTopo = (ITopologicalOperator2)pWorstPolyline;
			pWorstTopo.IsKnownSimple_2 = false;
			pWorstTopo.Simplify();

			pMAware = (IMAware)pWorstPolyline;
			pMAware.MAware = true;

			IMSegmentation pWorstMSegmentation = (IMSegmentation)pWorstPolyline;
			pWorstMSegmentation.CalculateNonSimpleMs();
			//pWorstMSegmentation.SetMsAsDistance(true);
			pWorstMSegmentation.SetAndInterpolateMsBetween(0, pWorstPolyline.Length);

			IProximityOperator pWorstProxi = (IProximityOperator)(pWorstPolyline);
			IProximityOperator pWorstNomReleation = (IProximityOperator)(pWorstPolyline.FromPoint);
			//==========================================================================================

			leg.ObsMaxNetGrd = -1;
			leg.ObsMaxAcceleDist = -1;

			int j = 0, n = AllObsstacles.Length;
			leg.ObstacleList = new ObstacleData[n];

			for (int i = 0; i < n; i++)
			{
				pCurrPt = AllObsstacles[i].pPtPrj;
				if (pReleation.Disjoint(pCurrPt))
					continue;

				leg.ObstacleList[j] = AllObsstacles[i];

				//if (AllObsstacles[i].ID == "Ter-2582")
				//{
				//    xc = -1.0;
				//}

				/*
				PrjToLocal(P1, xDir, pCurrPt, out x0, out y0);
				xc = -1.0;

				d = Rb * Rb - y0 * y0;
				if (d >= 0)
				{
					x = Math.Sqrt(d);
					xc = (x0 - x);
					fTmp = x0 + x;

					if (xc < 0.0)
						xc = fTmp;
					else if (fTmp < 1.01 * xDist)
					{
						if (bestSide >= 0)
							xc = Math.Min(xc, fTmp);
						else
							xc = Math.Max(xc, fTmp);

						//IPoint ptTmp = LocalToPrj(P1, xDir, Math.Max(xc, fTmp));
						//IPoint pTch = LocalToPrj(ptTmp, leg.ptStart.BestCase.Direction + 90.0 * TurnDir, Rb);
						//Graphics.DrawPolygon(CreateCirclePrj(ptTmp, Rb));
						//Graphics.DrawPointWithText(pCurrPt, "odst");
						//Graphics.DrawPointWithText(ptTmp, "cnt");
						//Graphics.DrawPointWithText(pTch, "tch");
						//ProcessMessages();
					}
				}

				onCircumCircle = xc >= 0 && xc <1.01 * xDist;

				if (onCircumCircle)
				{
					//xc = x0 - x;			//, если x0 - L ≥ 0, в противном случае,
					//if (xc < 0)
					//    xc = x0 + x;
					//Graphics.DrawPointWithText(pCurrPt, "odst");
					//ProcessMessages(true);

					double dirInRadian = GlobalVars.DegToRadValue * xDir;
					double fdX = pCurrPt.X - P1.X - xc * Math.Cos(dirInRadian);
					double fdY = pCurrPt.Y - P1.Y - xc * Math.Sin(dirInRadian);
					betha = RadToDeg(Math.Atan2(fdY, fdX));
					theta = betha - 90.0 * TurnDir;
				}
				else*/
				{
					IPoint ptBestNearest = pBestProxi.ReturnNearestPoint(pCurrPt, esriSegmentExtension.esriNoExtension);
					IPoint ptWorstNearest = pWorstProxi.ReturnNearestPoint(pCurrPt, esriSegmentExtension.esriNoExtension);

					double BestDist = ReturnDistanceInMeters(ptBestNearest, pCurrPt);
					double WorstDist = ReturnDistanceInMeters(ptWorstNearest, pCurrPt);
					double TotalDist = ReturnDistanceInMeters(ptBestNearest, ptWorstNearest);

					L = ptWorstNearest.M;
					ArcLen = 0.0;

					onCircumCircle = L < leg.WorstCase.Turn[0].Length;
					xc = xDist;
					//theta = leg.ptEnd.WorstCase.Direction;

					if (WorstDist < leg.ptEnd.WorstCase.Width)	//BaseDist
					{
					}
					else if (WorstDist > TotalDist || BestDist > TotalDist)//BaseDist
					{
						if (TotalDist < GlobalVars.distEps)
						{
						}
						else if (WorstDist > BestDist)
						{
							xc = 0.0;
							//theta = leg.ptEnd.BestCase.Direction;
							L = ptBestNearest.M;
							onCircumCircle = L < leg.BestCase.Turn[0].Length;
						}
						else
						{
						}
					}
					else
					{
						//theta = ReturnAngleInDegrees(pCurrPt, leg.ptEnd.BestCase.pPoint);
						//alpha = DegToRad(NativeMethods.Modulus(theta - xDir));
						//fTmp = y0 / Math.Tan(alpha) - Rb / Math.Sin(alpha) * TurnDir;
						//xc = x0 - fTmp;
						//fTmp = ReturnDistanceInMeters(pCurrPt, leg.ptEnd.BestCase.pPoint);

						//if (xc < 0 || xc >= xDist || fTmp < leg.ptEnd.WorstCase.Width)
						{
							double k = BestDist / (WorstDist + BestDist);
							//x0 = BestDist;
							//double fTmp = BestGr + dGr * BestDist / TotalDist;
							xc = xDist * k;	//TotalDist;
							L = ptBestNearest.M + (ptWorstNearest.M - ptBestNearest.M) * k;
							double tL = leg.BestCase.Turn[0].Length + (leg.WorstCase.Turn[0].Length - leg.BestCase.Turn[0].Length) * k;
							onCircumCircle = L < tL;

							//theta = ReturnAngleInDegrees(pCurrPt, leg.ptEnd.BestCase.pPoint);
						}
					}
				}

				//if (onCircumCircle)
				//{
				//	IPoint ptTmp = LocalToPrj(P1, xDir, xc);
				//	IPoint pTch = LocalToPrj(ptTmp, leg.ptStart.BestCase.Direction + 90.0 * TurnDir, Rb);

				//	Graphics.DrawPointWithText(pTch, "tch");
				//	Graphics.DrawPolygon(CreateCirclePrj(ptTmp, Rb));
				//    Graphics.DrawPointWithText(ptTmp, "cnt");
				//    ProcessMessages();
				//}

				//Graphics.DrawPolyline(leg.BestCase.pNominalPoly);
				//Graphics.DrawPolyline(leg.WorstCase.pNominalPoly);
				//Graphics.DrawPointWithText(pCurrPt, "odst");
				//ProcessMessages();

				//alpha = NativeMethods.Modulus((leg.ptStart.BestCase.Direction - theta) * TurnDir);
				//ArcLen = R * DegToRad(alpha);


				//if (!onCircumCircle)
				//{
				//    P0 = LocalToPrj(P1, xDir, xc);

				//    x = ReturnDistanceInMeters(P0, pCurrPt);
				//    L = Math.Sqrt(x * x - Rb * Rb);
				//}

				leg.ObstacleList[j].X = L + ArcLen;
				leg.ObstacleList[j].ApplNetGradient2 = BestGr + dGr * xc * InvXdist;

				leg.ObstacleList[j].Y = b;
				//if(prevlrgOnHeight)
				//	leg.ObstacleList[j].TotalDist = leg.BestCase.PrevTotalLength + leg.ObstacleList[j].X;
				//else

				leg.ObstacleList[j].TotalDist = leg.BestCase.PrevTotalLength + dDst * xc * InvXdist + leg.ObstacleList[j].X;

				leg.ObstacleList[j].MOC = GlobalVars.NetMOC;
				if (onCircumCircle)
					leg.ObstacleList[j].MOC = GlobalVars.TurnMOC;				//GlobalVars.NetMOC;

				leg.ObstacleList[j].MOCH = leg.ObstacleList[j].Height + leg.ObstacleList[j].MOC;

				double LostH = leg.ObstacleList[j].MOCH - GlobalVars.heightAboveDERMax;
				leg.ObstacleList[j].ReqNetGradient = LostH / leg.ObstacleList[j].TotalDist;

				PDGNet = leg.ObstacleList[j].ApplNetGradient2;		//????????????????????????????????????????????????????????????
				leg.ObstacleList[j].AcceleStartDist = LostH / PDGNet;

				if (leg.ObsMaxNetGrd < 0)
					leg.ObsMaxNetGrd = j;
				else if (leg.ObstacleList[j].ReqNetGradient > leg.ObstacleList[leg.ObsMaxNetGrd].ReqNetGradient)
					leg.ObsMaxNetGrd = j;

				if (leg.ObsMaxAcceleDist < 0)
					leg.ObsMaxAcceleDist = j;
				else if (leg.ObstacleList[j].AcceleStartDist > leg.ObstacleList[leg.ObsMaxAcceleDist].AcceleStartDist)
					leg.ObsMaxAcceleDist = j;

				j++;
			}

			System.Array.Resize<ObstacleData>(ref leg.ObstacleList, j);
			return j;
		}

		internal static int GetLegObstaclesS1Turn(ObstacleData[] AllObsstacles, ref TrackLeg leg)
		{
			double PDGNet = leg.WorstCase.NetGrd;

			IRelationalOperator pReleation = (IRelationalOperator)(leg.pProtectionArea);

			IPoint pCurrPt, P0, P1, P2;
			P1 = leg.BestCase.Turn[0].ptCenter;
			P2 = leg.WorstCase.Turn[0].ptCenter;

			double x0, y0, R = leg.BestCase.Turn[0].Radius, Rb, b, L, xc, ArcLen;
			double currDir = leg.ptStart.BestCase.Direction;
			double xDir = ReturnAngleInDegrees(P1, P2);
			double BaseDir = ReturnAngleInDegrees(leg.ptStart.BestCase.pPoint, leg.ptStart.WorstCase.pPoint);

			double xDist = ReturnDistanceInMeters(P1, P2);

			double InvXdist = 0.0;	// ReturnDistanceInMeters(P1, P2);
			if (xDist > 0)
				InvXdist = 1.0 / xDist;

			//R = 0.5 * (leg.BestCase.Turn1.Radius + leg.WorstCase.Turn1.Radius);
			b = 0.5 * (leg.ptEnd.BestCase.Width + leg.ptEnd.WorstCase.Width);
			Rb = R + b;

			double alpha, betha, theta, fTmp;		//, betha = leg.ptStart.BestCase.Direction; // leg.ptStart.WorstCase.Direction;
			bool onCircumCircle;

			double BestGr = leg.BestCase.NetGrd;
			double WorststGr = leg.WorstCase.NetGrd;
			double dGr = WorststGr - BestGr, d, x, dDst = leg.WorstCase.PrevTotalLength - leg.BestCase.PrevTotalLength;

			//double Pb = BestGr;
			//double dHb = leg.ptEnd.BestCase.NetHeight - leg.ptStart.BestCase.NetHeight;
			//double dHw = leg.ptEnd.WorstCase.NetHeight - leg.ptStart.WorstCase.NetHeight;

			int TurnDir = leg.BestCase.Turn[0].TurnDir;
			PrjToLocal(leg.ptStart.BestCase.pPoint, currDir, leg.ptStart.WorstCase.pPoint, out x0, out y0);

			int bestSide = Math.Sign(x0);

			IClone pClone = (IClone)leg.BestCase.pNominalPoly;
			IPolyline pBestPolyline = (IPolyline)pClone.Clone();

			ITopologicalOperator2 pBestTopo = (ITopologicalOperator2)pBestPolyline;
			pBestTopo.IsKnownSimple_2 = false;
			pBestTopo.Simplify();

			IMAware pMAware = (IMAware)pBestPolyline;
			pMAware.MAware = true;

			IMSegmentation pBestMSegmentation = (IMSegmentation)pBestPolyline;
			pBestMSegmentation.CalculateNonSimpleMs();
			pBestMSegmentation.SetAndInterpolateMsBetween(0, pBestPolyline.Length);

			IProximityOperator pBestProxi = (IProximityOperator)pBestPolyline;
			IProximityOperator pBestNomReleation = (IProximityOperator)(pBestPolyline.FromPoint);
			//=================================================================
			pClone = (IClone)leg.WorstCase.pNominalPoly;
			IPolyline pWorstPolyline = (IPolyline)pClone.Clone();

			ITopologicalOperator2 pWorstTopo = (ITopologicalOperator2)pWorstPolyline;
			pWorstTopo.IsKnownSimple_2 = false;
			pWorstTopo.Simplify();

			pMAware = (IMAware)pWorstPolyline;
			pMAware.MAware = true;

			IMSegmentation pWorstMSegmentation = (IMSegmentation)pWorstPolyline;
			pWorstMSegmentation.CalculateNonSimpleMs();
			//pWorstMSegmentation.SetMsAsDistance(true);
			pWorstMSegmentation.SetAndInterpolateMsBetween(0, pWorstPolyline.Length);

			IProximityOperator pWorstProxi = (IProximityOperator)(pWorstPolyline);
			IProximityOperator pWorstNomReleation = (IProximityOperator)(pWorstPolyline.FromPoint);
			//==========================================================================================
			double ycSign;
			PrjToLocal(P1, xDir, leg.BestCase.Turn[0].ptEnd, out x0, out ycSign);

			leg.ObsMaxNetGrd = -1;
			leg.ObsMaxAcceleDist = -1;

			int j = 0, n = AllObsstacles.Length;
			leg.ObstacleList = new ObstacleData[n];

			for (int i = 0; i < n; i++)
			{
				pCurrPt = AllObsstacles[i].pPtPrj;
				if (pReleation.Disjoint(pCurrPt))
					continue;

				leg.ObstacleList[j] = AllObsstacles[i];

				IPoint pTch;
				IPoint ptTmp = LocalToPrj(pCurrPt, currDir - 90.0 * TurnDir, Rb);
				PrjToLocal(P1, xDir, ptTmp, out xc, out fTmp);

				onCircumCircle = ycSign * fTmp < 0;

				//onCircumCircle = ycSign * y0 < 0;
				//Graphics.DrawPointWithText(ptTmp, AllObsstacles[i].ID + "-t");
				//Graphics.DrawPointWithText(pCurrPt, "pCurrPt");
				//Graphics.DrawPolyline(leg.BestCase.pNominalPoly);
				//ProcessMessages();

				if (onCircumCircle)
				{
					PrjToLocal(P1, xDir, pCurrPt, out x0, out y0);

					d = Rb * Rb - y0 * y0;
					x = Math.Sqrt(d);

					xc = (x0 - x);
					fTmp = x0 + x;

					if (xc < 0.0)
						xc = fTmp;
					else if (fTmp < 1.01 * xDist)
					{
						if (bestSide * TurnDir >= 0)
							xc = Math.Min(xc, fTmp);
						else
							xc = Math.Max(xc, fTmp);

						//ptTmp = LocalToPrj(P1, xDir, Math.Max(xc, fTmp));
						//pTch = LocalToPrj(ptTmp, leg.ptStart.BestCase.Direction + 90.0 * TurnDir, Rb);
						//Graphics.DrawPointWithText(pTch, "1");
						//Graphics.DrawPolygon(CreateCirclePrj(ptTmp, Rb));
						//Graphics.DrawPointWithText(ptTmp, "1");
						//ProcessMessages();
					}

					P0 = LocalToPrj(P1, xDir, xc);
					pTch = LocalToPrj(P0, currDir + 90.0 * TurnDir, Rb);

					//Graphics.DrawPointWithText(pTch, "2");
					//Graphics.DrawPolygon(CreateCirclePrj(P0, Rb2));
					//Graphics.DrawPointWithText(P0, "2");
					//ProcessMessages();

					double fdX = pCurrPt.X - P0.X;
					double fdY = pCurrPt.Y - P0.Y;
					betha = RadToDeg(Math.Atan2(fdY, fdX));
					theta = betha - 90.0 * TurnDir;

					alpha = NativeMethods.Modulus((currDir - theta) * TurnDir);
					ArcLen = R * DegToRad(alpha);
				}
				else
				{
					pTch = pCurrPt;
					ArcLen = 0.0;
				}

				//IConstructPoint pConstructPoint = (IConstructPoint)P0;
				//pConstructPoint.ConstructAngleIntersection(leg.ptStart.BestCase.pPoint, GlobalVars.DegToRadValue * BaseDir, pTch, GlobalVars.DegToRadValue * currDir2);

				////if (onCircumCircle)
				////{
				////    Graphics.DrawPointWithText(P0, AllObsstacles[i].ID);
				////    ProcessMessages();
				////}

				//PrjToLocal(leg.ptStart.BestCase.pPoint, BaseDir, P0, out x0, out y0);
				//L = ReturnDistanceInMeters(pTch, P0);

				PrjToLocal(P1, xDir, pTch, out x0, out y0);
				////=-=================================================================
				//if (AllObsstacles[i].ID == "Ter-2582")
				//{
				//    xc = -1.0;
				//}

				PrjToLocal(P1, xDir, pCurrPt, out x0, out y0);
				xc = -1.0;

				d = Rb * Rb - y0 * y0;
				if (d >= 0)
				{
					x = Math.Sqrt(d);
					xc = (x0 - x);
					fTmp = x0 + x;

					if (xc < 0.0)
						xc = fTmp;
					else if (fTmp < 1.01 * xDist)
					{
						if (bestSide >= 0)
							xc = Math.Min(xc, fTmp);
						else
							xc = Math.Max(xc, fTmp);

						//IPoint ptTmp = LocalToPrj(P1, xDir, Math.Max(xc, fTmp));
						//IPoint pTch = LocalToPrj(ptTmp, leg.ptStart.BestCase.Direction + 90.0 * TurnDir, Rb);
						//Graphics.DrawPolygon(CreateCirclePrj(ptTmp, Rb));
						//Graphics.DrawPointWithText(pCurrPt, "odst");
						//Graphics.DrawPointWithText(ptTmp, "cnt");
						//Graphics.DrawPointWithText(pTch, "tch");
						//ProcessMessages();
					}
				}

				onCircumCircle = xc >= 0 && xc < 1.01 * xDist;

				if (onCircumCircle)
				{
					//xc = x0 - x;			//, если x0 - L ≥ 0, в противном случае,
					//if (xc < 0)
					//    xc = x0 + x;
					//Graphics.DrawPointWithText(pCurrPt, "odst");
					//ProcessMessages(true);

					double dirInRadian = GlobalVars.DegToRadValue * xDir;
					double fdX = pCurrPt.X - P1.X - xc * Math.Cos(dirInRadian);
					double fdY = pCurrPt.Y - P1.Y - xc * Math.Sin(dirInRadian);
					betha = RadToDeg(Math.Atan2(fdY, fdX));
					theta = betha - 90.0 * TurnDir;
				}
				else
				{
					IPoint ptBestNearest = pBestProxi.ReturnNearestPoint(pCurrPt, esriSegmentExtension.esriNoExtension);
					IPoint ptWorstNearest = pWorstProxi.ReturnNearestPoint(pCurrPt, esriSegmentExtension.esriNoExtension);

					double BestDist = ReturnDistanceInMeters(ptBestNearest, pCurrPt);
					double WorstDist = ReturnDistanceInMeters(ptWorstNearest, pCurrPt);
					double TotalDist = ReturnDistanceInMeters(ptBestNearest, ptWorstNearest);

					if (WorstDist < leg.ptEnd.WorstCase.Width)	//BaseDist
					{
						xc = xDist;
						theta = leg.ptEnd.WorstCase.Direction;
					}
					else if (WorstDist > TotalDist || BestDist > TotalDist)//BaseDist
					{
						if (TotalDist < GlobalVars.distEps)
						{
							xc = xDist;
							theta = leg.ptEnd.WorstCase.Direction;
						}
						else if (WorstDist > BestDist)
						{
							xc = 0.0;
							theta = leg.ptEnd.BestCase.Direction;
						}
						else
						{
							xc = xDist;
							theta = leg.ptEnd.WorstCase.Direction;
						}
					}
					else
					{
						theta = ReturnAngleInDegrees(pCurrPt, leg.ptEnd.BestCase.pPoint);
						alpha = DegToRad(NativeMethods.Modulus(theta - xDir));
						fTmp = y0 / Math.Tan(alpha) - Rb / Math.Sin(alpha) * TurnDir;
						xc = x0 - fTmp;
						fTmp = ReturnDistanceInMeters(pCurrPt, leg.ptEnd.BestCase.pPoint);

						if (xc < 0 || xc >= xDist || fTmp < leg.ptEnd.WorstCase.Width)
						{
							//x0 = BestDist;
							//double fTmp = BestGr + dGr * BestDist / TotalDist;
							xc = xDist * BestDist / (WorstDist + BestDist);//TotalDist;
							//theta = ReturnAngleInDegrees(pCurrPt, leg.ptEnd.BestCase.pPoint);
						}
					}
				}

				//if (onCircumCircle)
				//{
				//	IPoint ptTmp = LocalToPrj(P1, xDir, xc);
				//	IPoint pTch = LocalToPrj(ptTmp, leg.ptStart.BestCase.Direction + 90.0 * TurnDir, Rb);

				//	Graphics.DrawPointWithText(pTch, "tch");
				//	Graphics.DrawPolygon(CreateCirclePrj(ptTmp, Rb));
				//    Graphics.DrawPointWithText(ptTmp, "cnt");
				//    ProcessMessages();
				//}

				//Graphics.DrawPolyline(leg.BestCase.pNominalPoly);
				//Graphics.DrawPolyline(leg.WorstCase.pNominalPoly);
				//Graphics.DrawPointWithText(pCurrPt, "odst");
				//ProcessMessages();

				alpha = NativeMethods.Modulus((leg.ptStart.BestCase.Direction - theta) * TurnDir);
				ArcLen = R * DegToRad(alpha);

				L = 0.0;
				if (!onCircumCircle)
				{
					P0 = LocalToPrj(P1, xDir, xc);

					x = ReturnDistanceInMeters(P0, pCurrPt);
					L = Math.Sqrt(x * x - Rb * Rb);
				}

				leg.ObstacleList[j].X = L + ArcLen;
				leg.ObstacleList[j].ApplNetGradient2 = BestGr + dGr * xc * InvXdist;

				leg.ObstacleList[j].Y = b;
				//if(prevlrgOnHeight)
				//	leg.ObstacleList[j].TotalDist = leg.BestCase.PrevTotalLength + leg.ObstacleList[j].X;
				//else

				leg.ObstacleList[j].TotalDist = leg.BestCase.PrevTotalLength + dDst * xc * InvXdist + leg.ObstacleList[j].X;

				leg.ObstacleList[j].MOC = GlobalVars.TurnMOC;				//GlobalVars.NetMOC;
				leg.ObstacleList[j].MOCH = leg.ObstacleList[j].Height + leg.ObstacleList[j].MOC;

				double LostH = leg.ObstacleList[j].MOCH - GlobalVars.heightAboveDERMax;
				leg.ObstacleList[j].ReqNetGradient = LostH / leg.ObstacleList[j].TotalDist;

				PDGNet = leg.ObstacleList[j].ApplNetGradient2;		//????????????????????????????????????????????????????????????
				leg.ObstacleList[j].AcceleStartDist = LostH / PDGNet;

				if (leg.ObsMaxNetGrd < 0)
					leg.ObsMaxNetGrd = j;
				else if (leg.ObstacleList[j].ReqNetGradient > leg.ObstacleList[leg.ObsMaxNetGrd].ReqNetGradient)
					leg.ObsMaxNetGrd = j;

				if (leg.ObsMaxAcceleDist < 0)
					leg.ObsMaxAcceleDist = j;
				else if (leg.ObstacleList[j].AcceleStartDist > leg.ObstacleList[leg.ObsMaxAcceleDist].AcceleStartDist)
					leg.ObsMaxAcceleDist = j;

				j++;
			}

			System.Array.Resize<ObstacleData>(ref leg.ObstacleList, j);
			return j;
		}

		internal static int GetLegObstacles2Turn0(ObstacleData[] AllObsstacles, ref TrackLeg leg)
		{
			double PDGNet = leg.WorstCase.NetGrd;

			IRelationalOperator pReleation = (IRelationalOperator)(leg.pProtectionArea);
			IPoint pCurrPt, P0, P11, P12, pTch, ptTmp;
			P11 = leg.BestCase.Turn[1].ptCenter;
			P12 = leg.WorstCase.Turn[1].ptCenter;
			IConstructPoint pConstructPoint;

			double x0, y0, R2 = leg.BestCase.Turn[1].Radius, Rb2, b, L, xc, ycSign2, ArcLen2, xc1;
			//R = 0.5 * (leg.BestCase.Turn1.Radius + leg.WorstCase.Turn1.Radius);
			b = 0.5 * (leg.ptEnd.BestCase.Width + leg.ptEnd.WorstCase.Width);
			Rb2 = R2 + b;
			double currDir2 = leg.ptStart.BestCase.Direction;
			double xDir2 = ReturnAngleInDegrees(P11, P12);
			double xDist2 = ReturnDistanceInMeters(P11, P12);
			PrjToLocal(P11, xDir2, leg.ptStart.BestCase.pPoint, out x0, out ycSign2);

			double BaseDir = ReturnAngleInDegrees(leg.ptStart.BestCase.pPoint, leg.ptStart.WorstCase.pPoint);
			double InvBaseDist = ReturnDistanceInMeters(leg.ptStart.BestCase.pPoint, leg.ptStart.WorstCase.pPoint);
			if (InvBaseDist > 0)
				InvBaseDist = 1.0 / InvBaseDist;

			double alpha, betha, theta, fTmp;		//, betha = leg.ptStart.BestCase.Direction; // leg.ptStart.WorstCase.Direction;

			double BestGr = leg.BestCase.NetGrd;
			double WorststGr = leg.WorstCase.NetGrd;
			double dGr = WorststGr - BestGr, d, x, dDst = leg.WorstCase.PrevTotalLength - leg.BestCase.PrevTotalLength;

			//double Pb = BestGr;
			//double dHb = leg.ptEnd.BestCase.NetHeight - leg.ptStart.BestCase.NetHeight;
			//double dHw = leg.ptEnd.WorstCase.NetHeight - leg.ptStart.WorstCase.NetHeight;

			int TurnDir2 = leg.BestCase.Turn[0].TurnDir, bestSide2 = (int)SideDef(leg.ptStart.BestCase.pPoint, currDir2, leg.ptStart.WorstCase.pPoint);
			bool onCircumCircle2;

			//IPolyline pPolyline = (IPolyline)new Polyline();
			//pPolyline.FromPoint = leg.BestCase.Turn1.ptCenter;
			//pPolyline.ToPoint = leg.WorstCase.Turn1.ptCenter;

			//Graphics.DrawPolyline(pPolyline, -1);
			//Graphics.DrawPointWithText(leg.BestCase.Turn1.ptCenter, "O1");
			//Graphics.DrawPointWithText(leg.WorstCase.Turn1.ptCenter, "O2");
			//ProcessMessages();

			//==========================================================================================

			leg.ObsMaxNetGrd = -1;
			leg.ObsMaxAcceleDist = -1;

			int j = 0, n = AllObsstacles.Length;
			leg.ObstacleList = new ObstacleData[n];
			P0 = new Point();

			for (int i = 0; i < n; i++)
			{
				pCurrPt = AllObsstacles[i].pPtPrj;

				if (pReleation.Disjoint(pCurrPt))
					continue;

				leg.ObstacleList[j] = AllObsstacles[i];

				ptTmp = LocalToPrj(pCurrPt, currDir2 - 90.0 * TurnDir2, Rb2);
				PrjToLocal(P11, xDir2, ptTmp, out xc1, out fTmp);

				onCircumCircle2 = ycSign2 * fTmp < 0;

				//onCircumCircle = ycSign * y0 < 0;
				//Graphics.DrawPointWithText(ptTmp, AllObsstacles[i].ID + "-t");
				//Graphics.DrawPointWithText(pCurrPt, "pCurrPt");
				//ProcessMessages();

				if (onCircumCircle2)
				{
					PrjToLocal(P11, xDir2, pCurrPt, out x0, out y0);

					d = Rb2 * Rb2 - y0 * y0;
					x = Math.Sqrt(d);

					xc = (x0 - x);
					fTmp = x0 + x;

					if (xc < 0.0)
						xc = fTmp;
					else if (fTmp < 1.01 * xDist2)
					{
						if (bestSide2 * TurnDir2 >= 0)
							xc = Math.Min(xc, fTmp);
						else
							xc = Math.Max(xc, fTmp);

						//ptTmp = LocalToPrj(P1, xDir, Math.Max(xc, fTmp));
						//pTch = LocalToPrj(ptTmp, leg.ptStart.BestCase.Direction + 90.0 * TurnDir, Rb);
						//Graphics.DrawPointWithText(pTch, "1");
						//Graphics.DrawPolygon(CreateCirclePrj(ptTmp, Rb));
						//Graphics.DrawPointWithText(ptTmp, "1");
						//ProcessMessages();
					}

					P0 = LocalToPrj(P11, xDir2, xc);
					pTch = LocalToPrj(P0, currDir2 + 90.0 * TurnDir2, Rb2);

					//Graphics.DrawPointWithText(pTch, "");
					//Graphics.DrawPolygon(CreateCirclePrj(P0, Rb));
					//Graphics.DrawPointWithText(P0, "");
					//ProcessMessages();

					double fdX = pCurrPt.X - P0.X;
					double fdY = pCurrPt.Y - P0.Y;
					betha = RadToDeg(Math.Atan2(fdY, fdX));
					theta = betha - 90.0 * TurnDir2;

					alpha = NativeMethods.Modulus((currDir2 - theta) * TurnDir2);
					ArcLen2 = R2 * DegToRad(alpha);
				}
				else
				{
					pTch = pCurrPt;
					ArcLen2 = 0.0;
				}

				pConstructPoint = (IConstructPoint)P0;
				pConstructPoint.ConstructAngleIntersection(leg.ptStart.BestCase.pPoint, GlobalVars.DegToRadValue * BaseDir, pTch, GlobalVars.DegToRadValue * currDir2);

				//if (onCircumCircle)
				//{
				//    Graphics.DrawPointWithText(P0, AllObsstacles[i].ID);
				//    ProcessMessages();
				//}

				PrjToLocal(leg.ptStart.BestCase.pPoint, BaseDir, P0, out x0, out y0);
				L = ReturnDistanceInMeters(pTch, P0);

				leg.ObstacleList[j].X = L + ArcLen2;
				leg.ObstacleList[j].ApplNetGradient2 = BestGr + dGr * x0 * InvBaseDist;

				//Graphics.DrawPolyline(pLinePart, 255, 2);
				//Graphics.DrawPointWithText(leg.ObstacleList[j].pPtPrj, "leg.Obst");
				//PrjToLocal(SelectedRWY.pPtPrj[eRWY.PtDER], SelectedRWY.pPtPrj[eRWY.PtDER].M, leg.ObstacleList[j].pPtPrj, out leg.ObstacleList[j].X, out leg.ObstacleList[j].Y);

				leg.ObstacleList[j].Y = b;
				//if(prevlrgOnHeight)
				//	leg.ObstacleList[j].TotalDist = leg.BestCase.PrevTotalLength + leg.ObstacleList[j].X;
				//else

				leg.ObstacleList[j].TotalDist = leg.BestCase.PrevTotalLength + dDst * x0 * InvBaseDist + leg.ObstacleList[j].X;

				leg.ObstacleList[j].MOC = GlobalVars.TurnMOC;				//GlobalVars.NetMOC;
				leg.ObstacleList[j].MOCH = leg.ObstacleList[j].Height + leg.ObstacleList[j].MOC;

				double LostH = leg.ObstacleList[j].MOCH - GlobalVars.heightAboveDERMax;
				leg.ObstacleList[j].ReqNetGradient = LostH / leg.ObstacleList[j].TotalDist;

				PDGNet = leg.ObstacleList[j].ApplNetGradient2;		//????????????????????????????????????????????????????????????
				leg.ObstacleList[j].AcceleStartDist = LostH / PDGNet;

				//Graphics.DrawPolyline(pLinePart, 255, 2);

				/*{
					if (ReturnDistanceInMeters(pLinePart.FromPoint, leg.pNominalPoly.FromPoint) > ReturnDistanceInMeters(pLinePart.ToPoint, leg.pNominalPoly.FromPoint))
						pLinePart.ReverseOrientation();
				}*/

				if (leg.ObsMaxNetGrd < 0)
					leg.ObsMaxNetGrd = j;
				else if (leg.ObstacleList[j].ReqNetGradient > leg.ObstacleList[leg.ObsMaxNetGrd].ReqNetGradient)
					leg.ObsMaxNetGrd = j;

				if (leg.ObsMaxAcceleDist < 0)
					leg.ObsMaxAcceleDist = j;
				else if (leg.ObstacleList[j].AcceleStartDist > leg.ObstacleList[leg.ObsMaxAcceleDist].AcceleStartDist)
					leg.ObsMaxAcceleDist = j;

				j++;
			}

			System.Array.Resize<ObstacleData>(ref leg.ObstacleList, j);
			return j;
		}

		internal static int GetLegObstacles2Turn1(ObstacleData[] AllObsstacles, ref TrackLeg leg)
		{
			double PDGNet = leg.WorstCase.NetGrd;

			IRelationalOperator pReleation = (IRelationalOperator)(leg.pProtectionArea);

			IPoint pCurrPt, P0, P01, P02;
			P01 = leg.BestCase.Turn[0].ptCenter;
			P02 = leg.WorstCase.Turn[0].ptCenter;

			IPoint P11, P12, pTch, ptTmp;
			P11 = leg.BestCase.Turn[1].ptCenter;
			P12 = leg.WorstCase.Turn[1].ptCenter;

			double x0, y0, R1 = leg.BestCase.Turn[0].Radius, Rb1, b, L, xc, ArcLen1;
			double R2 = leg.BestCase.Turn[1].Radius, Rb2, ycSign, ArcLen2;

			double currDir1 = leg.ptStart.BestCase.Direction;
			double currDir2 = leg.BestCase.DRDirection;
			double xDir1 = ReturnAngleInDegrees(P01, P02);
			double xDir2 = ReturnAngleInDegrees(P11, P12);
			double BaseDir = ReturnAngleInDegrees(leg.ptStart.BestCase.pPoint, leg.ptStart.WorstCase.pPoint);

			double xDist1 = ReturnDistanceInMeters(P01, P02);
			double xDist2 = ReturnDistanceInMeters(P11, P12);

			if (xDist1 + xDist2 < GlobalVars.distEps)
				return GetLegObstaclesNoTurn(AllObsstacles, ref leg);	// GetDMEArcLegObstacles(AllObsstacles, ref leg);

			PrjToLocal(P11, xDir2, leg.BestCase.Turn[0].ptEnd, out x0, out ycSign);

			double InvXdist = 0.0;
			if (xDist1 > 0.0)
				InvXdist = 1.0 / xDist1;

			//R = 0.5 * (leg.BestCase.Turn1.Radius + leg.WorstCase.Turn1.Radius);
			b = 0.5 * (leg.ptEnd.BestCase.Width + leg.ptEnd.WorstCase.Width);
			Rb1 = R1 + b;
			Rb2 = R2 + b;

			double alpha, betha, theta, fTmp;		//, betha = leg.ptStart.BestCase.Direction; // leg.ptStart.WorstCase.Direction;
			bool onCircumCircle1;
			bool onCircumCircle2;

			double BestGr = leg.BestCase.NetGrd;
			double WorststGr = leg.WorstCase.NetGrd;
			double dGr = WorststGr - BestGr, d, x, dDst = leg.WorstCase.PrevTotalLength - leg.BestCase.PrevTotalLength;

			//double Pb = BestGr;
			//double dHb = leg.ptEnd.BestCase.NetHeight - leg.ptStart.BestCase.NetHeight;
			//double dHw = leg.ptEnd.WorstCase.NetHeight - leg.ptStart.WorstCase.NetHeight;

			PrjToLocal(leg.ptStart.BestCase.pPoint, currDir1, leg.ptStart.WorstCase.pPoint, out x0, out y0);
			int TurnDir1 = leg.BestCase.Turn[0].TurnDir, bestSide1 = Math.Sign(x0);
			int TurnDir2 = leg.BestCase.Turn[1].TurnDir, bestSide2 = (int)SideDef(leg.BestCase.Turn[0].ptEnd, currDir2, leg.WorstCase.Turn[0].ptEnd);

			//==========================================================================================
			IClone pClone = (IClone)leg.BestCase.pNominalPoly;
			IPolyline pBestPolyline = (IPolyline)pClone.Clone();

			ITopologicalOperator2 pBestTopo = (ITopologicalOperator2)pBestPolyline;
			pBestTopo.IsKnownSimple_2 = false;
			pBestTopo.Simplify();

			IMAware pMAware = (IMAware)pBestPolyline;
			pMAware.MAware = true;

			IMSegmentation pBestMSegmentation = (IMSegmentation)pBestPolyline;
			pBestMSegmentation.CalculateNonSimpleMs();
			pBestMSegmentation.SetAndInterpolateMsBetween(0, pBestPolyline.Length);

			IProximityOperator pBestProxi = (IProximityOperator)pBestPolyline;
			IProximityOperator pBestNomReleation = (IProximityOperator)(pBestPolyline.FromPoint);
			//=================================================================
			pClone = (IClone)leg.WorstCase.pNominalPoly;
			IPolyline pWorstPolyline = (IPolyline)pClone.Clone();

			ITopologicalOperator2 pWorstTopo = (ITopologicalOperator2)pWorstPolyline;
			pWorstTopo.IsKnownSimple_2 = false;
			pWorstTopo.Simplify();

			pMAware = (IMAware)pWorstPolyline;
			pMAware.MAware = true;

			IMSegmentation pWorstMSegmentation = (IMSegmentation)pWorstPolyline;
			pWorstMSegmentation.CalculateNonSimpleMs();
			//pWorstMSegmentation.SetMsAsDistance(true);
			pWorstMSegmentation.SetAndInterpolateMsBetween(0, pWorstPolyline.Length);

			IProximityOperator pWorstProxi = (IProximityOperator)(pWorstPolyline);
			IProximityOperator pWorstNomReleation = (IProximityOperator)(pWorstPolyline.FromPoint);
			//=================================================================

			leg.ObsMaxNetGrd = -1;
			leg.ObsMaxAcceleDist = -1;

			int j = 0, n = AllObsstacles.Length;

			leg.ObstacleList = new ObstacleData[n];

			for (int i = 0; i < n; i++)
			{
				pCurrPt = AllObsstacles[i].pPtPrj;
				if (pReleation.Disjoint(pCurrPt))
					continue;

				leg.ObstacleList[j] = AllObsstacles[i];

				IPoint ptBestNearest = pBestProxi.ReturnNearestPoint(pCurrPt, esriSegmentExtension.esriNoExtension);
				IPoint ptWorstNearest = pWorstProxi.ReturnNearestPoint(pCurrPt, esriSegmentExtension.esriNoExtension);

				double BestDist = ReturnDistanceInMeters(ptBestNearest, pCurrPt);
				double WorstDist = ReturnDistanceInMeters(ptWorstNearest, pCurrPt);
				double TotalDist = ReturnDistanceInMeters(ptBestNearest, ptWorstNearest);

				if (WorstDist < leg.ptEnd.WorstCase.Width)	//BaseDist
				{
					xc = xDist1;
					theta = leg.ptEnd.WorstCase.Direction;
				}

				ptTmp = LocalToPrj(pCurrPt, currDir2 - 90.0 * TurnDir2, Rb2);
				PrjToLocal(P11, xDir2, ptTmp, out xc, out fTmp);

				onCircumCircle2 = ycSign * fTmp < 0;

				//onCircumCircle = ycSign * y0 < 0;
				//Graphics.DrawPointWithText(ptTmp, AllObsstacles[i].ID + "-t");
				//Graphics.DrawPointWithText(pCurrPt, "pCurrPt");
				//Graphics.DrawPolyline(leg.BestCase.pNominalPoly);
				//ProcessMessages();

				if (onCircumCircle2)
				{
					PrjToLocal(P11, xDir2, pCurrPt, out x0, out y0);

					d = Rb2 * Rb2 - y0 * y0;
					x = Math.Sqrt(d);

					xc = (x0 - x);
					fTmp = x0 + x;

					if (xc < 0.0)
						xc = fTmp;
					else if (fTmp < 1.01 * xDist2)
					{
						if (bestSide2 * TurnDir2 >= 0)
							xc = Math.Min(xc, fTmp);
						else
							xc = Math.Max(xc, fTmp);

						//ptTmp = LocalToPrj(P1, xDir, Math.Max(xc, fTmp));
						//pTch = LocalToPrj(ptTmp, leg.ptStart.BestCase.Direction + 90.0 * TurnDir, Rb);
						//Graphics.DrawPointWithText(pTch, "1");
						//Graphics.DrawPolygon(CreateCirclePrj(ptTmp, Rb));
						//Graphics.DrawPointWithText(ptTmp, "1");
						//ProcessMessages();
					}

					P0 = LocalToPrj(P11, xDir2, xc);
					pTch = LocalToPrj(P0, currDir2 + 90.0 * TurnDir2, Rb2);

					//Graphics.DrawPointWithText(pTch, "2");
					//Graphics.DrawPolygon(CreateCirclePrj(P0, Rb2));
					//Graphics.DrawPointWithText(P0, "2");
					//ProcessMessages();

					double fdX = pCurrPt.X - P0.X;
					double fdY = pCurrPt.Y - P0.Y;
					betha = RadToDeg(Math.Atan2(fdY, fdX));
					theta = betha - 90.0 * TurnDir2;

					alpha = NativeMethods.Modulus((currDir2 - theta) * TurnDir2);
					ArcLen2 = R2 * DegToRad(alpha);
				}
				else
				{
					pTch = pCurrPt;
					ArcLen2 = 0.0;
				}

				//IConstructPoint pConstructPoint = (IConstructPoint)P0;
				//pConstructPoint.ConstructAngleIntersection(leg.ptStart.BestCase.pPoint, GlobalVars.DegToRadValue * BaseDir, pTch, GlobalVars.DegToRadValue * currDir2);

				////if (onCircumCircle)
				////{
				////    Graphics.DrawPointWithText(P0, AllObsstacles[i].ID);
				////    ProcessMessages();
				////}

				//PrjToLocal(leg.ptStart.BestCase.pPoint, BaseDir, P0, out x0, out y0);
				//L = ReturnDistanceInMeters(pTch, P0);

				PrjToLocal(P01, xDir1, pTch, out x0, out y0);

				d = Rb1 * Rb1 - y0 * y0;
				onCircumCircle1 = d >= 0;

				if (onCircumCircle1)
				{
					x = Math.Sqrt(d);

					xc = (x0 - x);
					fTmp = x0 + x;

					if (xc < 0.0)
						xc = fTmp;
					else if (fTmp < 1.01 * xDist1)
					{
						if (bestSide1 >= 0)
							xc = Math.Min(xc, fTmp);
						else
							xc = Math.Max(xc, fTmp);

						//ptTmp = LocalToPrj(P1, xDir, Math.Max(xc, fTmp));
						//pTch = LocalToPrj(ptTmp, leg.ptStart.BestCase.Direction + 90.0 * TurnDir, Rb);
						//Graphics.DrawPointWithText(pTch, "1");
						//Graphics.DrawPolygon(CreateCirclePrj(ptTmp, Rb));
						//Graphics.DrawPointWithText(ptTmp, "1");
						//ProcessMessages();
					}

					//xc = x0 - x;			//, если x0 - L ≥ 0, в противном случае,
					//if (xc < 0)
					//    xc = x0 + x;

					double dirInRadian = GlobalVars.DegToRadValue * xDir1;
					double fdX = pTch.X - P01.X - xc * Math.Cos(dirInRadian);
					double fdY = pTch.Y - P01.Y - xc * Math.Sin(dirInRadian);
					betha = RadToDeg(Math.Atan2(fdY, fdX));
					theta = betha - 90.0 * TurnDir1;


					P0 = LocalToPrj(P01, xDir1, xc);
					ptTmp = LocalToPrj(P0, leg.ptStart.BestCase.Direction + 90.0 * TurnDir1, Rb1);
					//Graphics.DrawPointWithText(ptTmp, "1");
					//Graphics.DrawPolygon(CreateCirclePrj(P0, Rb1));
					//Graphics.DrawPointWithText(P0, "1");
				}
				else
				{
					theta = ReturnAngleInDegrees(pTch, leg.ptEnd.BestCase.pPoint);
					alpha = DegToRad(NativeMethods.Modulus(theta - xDir1));
					fTmp = y0 / Math.Tan(alpha) - Rb1 / Math.Sin(alpha) * TurnDir1;
					xc = x0 - fTmp;
				}

				//if (onCircumCircle1)
				//{
				//    P0 = LocalToPrj(P1, xDir, xc);
				//    IPoint pTch = LocalToPrj(P0, leg.ptStart.BestCase.Direction + 90.0 * TurnDir, Rb);

				//    Graphics.DrawPointWithText(pTch, "");
				//    Graphics.DrawPolygon(CreateCirclePrj(P0, Rb));
				//    Graphics.DrawPointWithText(P0, "");
				//    ProcessMessages();
				//}

				alpha = NativeMethods.Modulus((leg.ptStart.BestCase.Direction - theta) * TurnDir1);
				ArcLen1 = R1 * DegToRad(alpha);

				L = 0.0;
				if (!onCircumCircle1)
				{
					P0 = LocalToPrj(P01, xDir1, xc);

					x = ReturnDistanceInMeters(P0, pTch);
					L = Math.Sqrt(x * x - Rb1 * Rb1);
				}

				leg.ObstacleList[j].X = L + ArcLen1 + ArcLen2;
				leg.ObstacleList[j].ApplNetGradient2 = BestGr + dGr * xc * InvXdist;

				leg.ObstacleList[j].Y = b;
				//if(prevlrgOnHeight)
				//	leg.ObstacleList[j].TotalDist = leg.BestCase.PrevTotalLength + leg.ObstacleList[j].X;
				//else

				leg.ObstacleList[j].TotalDist = leg.BestCase.PrevTotalLength + dDst * xc * InvXdist + leg.ObstacleList[j].X;

				leg.ObstacleList[j].MOC = GlobalVars.TurnMOC;				//GlobalVars.NetMOC;
				leg.ObstacleList[j].MOCH = leg.ObstacleList[j].Height + leg.ObstacleList[j].MOC;

				double LostH = leg.ObstacleList[j].MOCH - GlobalVars.heightAboveDERMax;
				leg.ObstacleList[j].ReqNetGradient = LostH / leg.ObstacleList[j].TotalDist;

				PDGNet = leg.ObstacleList[j].ApplNetGradient2;		//????????????????????????????????????????????????????????????
				leg.ObstacleList[j].AcceleStartDist = LostH / PDGNet;

				if (leg.ObsMaxNetGrd < 0)
					leg.ObsMaxNetGrd = j;
				else if (leg.ObstacleList[j].ReqNetGradient > leg.ObstacleList[leg.ObsMaxNetGrd].ReqNetGradient)
					leg.ObsMaxNetGrd = j;

				if (leg.ObsMaxAcceleDist < 0)
					leg.ObsMaxAcceleDist = j;
				else if (leg.ObstacleList[j].AcceleStartDist > leg.ObstacleList[leg.ObsMaxAcceleDist].AcceleStartDist)
					leg.ObsMaxAcceleDist = j;

				j++;
			}

			System.Array.Resize<ObstacleData>(ref leg.ObstacleList, j);
			return j;
		}

		internal static int GetLegObstacles2Turn(ObstacleData[] AllObsstacles, ref TrackLeg leg)
		{
			double PDGNet = leg.WorstCase.NetGrd;

			IRelationalOperator pReleation = (IRelationalOperator)(leg.pProtectionArea);

			IPoint pCurrPt, P01, P02;//, P0;
			P01 = leg.BestCase.Turn[0].ptCenter;
			P02 = leg.WorstCase.Turn[0].ptCenter;

			IPoint P11, P12;//, pTch, ptTmp;
			P11 = leg.BestCase.Turn[1].ptCenter;
			P12 = leg.WorstCase.Turn[1].ptCenter;

			double x0, y0, R1 = leg.BestCase.Turn[0].Radius, Rb1, b, xc;//, ArcLen1, L;
			double R2 = leg.BestCase.Turn[1].Radius, Rb2, ycSign;//, ArcLen2;

			double currDir1 = leg.ptStart.BestCase.Direction;
			double currDir2 = leg.BestCase.DRDirection;
			double xDir1 = ReturnAngleInDegrees(P01, P02);
			double xDir2 = ReturnAngleInDegrees(P11, P12);
			double BaseDir = ReturnAngleInDegrees(leg.ptStart.BestCase.pPoint, leg.ptStart.WorstCase.pPoint);

			double xDist1 = ReturnDistanceInMeters(P01, P02);
			double xDist2 = ReturnDistanceInMeters(P11, P12);

			if (xDist1 + xDist2 < GlobalVars.distEps) return GetLegObstaclesNoTurn(AllObsstacles, ref leg);	// GetDMEArcLegObstacles(AllObsstacles, ref leg);

			PrjToLocal(P11, xDir2, leg.BestCase.Turn[0].ptEnd, out x0, out ycSign);

			double InvXdist = 0.0;
			if (xDist1 > 0.0)
				InvXdist = 1.0 / xDist1;

			//R = 0.5 * (leg.BestCase.Turn1.Radius + leg.WorstCase.Turn1.Radius);
			b = 0.5 * (leg.ptEnd.BestCase.Width + leg.ptEnd.WorstCase.Width);
			Rb1 = R1 + b;
			Rb2 = R2 + b;

			//double alpha, betha, theta, fTmp;		//, betha = leg.ptStart.BestCase.Direction; // leg.ptStart.WorstCase.Direction;
			//bool onCircumCircle1;
			//bool onCircumCircle2;

			double BestGr = leg.BestCase.NetGrd;
			double WorstGr = leg.WorstCase.NetGrd;
			double dGr = WorstGr - BestGr, dDst = leg.WorstCase.PrevTotalLength - leg.BestCase.PrevTotalLength;//, d, x;

			//double Pb = BestGr;
			//double dHb = leg.ptEnd.BestCase.NetHeight - leg.ptStart.BestCase.NetHeight;
			//double dHw = leg.ptEnd.WorstCase.NetHeight - leg.ptStart.WorstCase.NetHeight;

			PrjToLocal(leg.ptStart.BestCase.pPoint, currDir1, leg.ptStart.WorstCase.pPoint, out x0, out y0);
			int TurnDir1 = leg.BestCase.Turn[0].TurnDir, bestSide1 = Math.Sign(x0);
			int TurnDir2 = leg.BestCase.Turn[1].TurnDir, bestSide2 = (int)SideDef(leg.BestCase.Turn[0].ptEnd, currDir2, leg.WorstCase.Turn[0].ptEnd);

			//==========================================================================================
			IClone pClone = (IClone)leg.BestCase.pNominalPoly;
			IPolyline pBestPolyline = (IPolyline)pClone.Clone();

			ITopologicalOperator2 pBestTopo = (ITopologicalOperator2)pBestPolyline;
			pBestTopo.IsKnownSimple_2 = false;
			pBestTopo.Simplify();

			IMAware pMAware = (IMAware)pBestPolyline;
			pMAware.MAware = true;

			IMSegmentation pBestMSegmentation = (IMSegmentation)pBestPolyline;
			pBestMSegmentation.CalculateNonSimpleMs();
			pBestMSegmentation.SetAndInterpolateMsBetween(0, pBestPolyline.Length);

			IProximityOperator pBestProxi = (IProximityOperator)pBestPolyline;
			IProximityOperator pBestNomReleation = (IProximityOperator)(pBestPolyline.FromPoint);
			//=================================================================
			pClone = (IClone)leg.WorstCase.pNominalPoly;
			IPolyline pWorstPolyline = (IPolyline)pClone.Clone();

			ITopologicalOperator2 pWorstTopo = (ITopologicalOperator2)pWorstPolyline;
			pWorstTopo.IsKnownSimple_2 = false;
			pWorstTopo.Simplify();

			pMAware = (IMAware)pWorstPolyline;
			pMAware.MAware = true;

			IMSegmentation pWorstMSegmentation = (IMSegmentation)pWorstPolyline;
			pWorstMSegmentation.CalculateNonSimpleMs();
			//pWorstMSegmentation.SetMsAsDistance(true);
			pWorstMSegmentation.SetAndInterpolateMsBetween(0, pWorstPolyline.Length);

			IProximityOperator pWorstProxi = (IProximityOperator)(pWorstPolyline);
			IProximityOperator pWorstNomReleation = (IProximityOperator)(pWorstPolyline.FromPoint);
			//=================================================================

			leg.ObsMaxNetGrd = -1;
			leg.ObsMaxAcceleDist = -1;

			int j = 0, n = AllObsstacles.Length;

			leg.ObstacleList = new ObstacleData[n];

			for (int i = 0; i < n; i++)
			{
				pCurrPt = AllObsstacles[i].pPtPrj;
				if (pReleation.Disjoint(pCurrPt))
					continue;

				leg.ObstacleList[j] = AllObsstacles[i];

				IPoint ptBestNearest = pBestProxi.ReturnNearestPoint(pCurrPt, esriSegmentExtension.esriNoExtension);
				IPoint ptWorstNearest = pWorstProxi.ReturnNearestPoint(pCurrPt, esriSegmentExtension.esriNoExtension);

				double BestDist = ReturnDistanceInMeters(ptBestNearest, pCurrPt);
				double WorstDist = ReturnDistanceInMeters(ptWorstNearest, pCurrPt);
				double TotalDist = ReturnDistanceInMeters(ptBestNearest, ptWorstNearest);

				if (WorstDist < leg.ptEnd.WorstCase.Width)	//BaseDist
				{
					xc = ptWorstNearest.M;
				}
				else
				{
					xc = ptBestNearest.M + BestDist * (ptWorstNearest.M - ptBestNearest.M) / (BestDist + WorstDist);
				}

				//Graphics.DrawPointWithText(ptBestNearest, "NBest");
				//Graphics.DrawPointWithText(ptWorstNearest, "NWorst");
				//ProcessMessages();

				leg.ObstacleList[j].X = ptWorstNearest.M;	//		leg.ObstacleList[j].X = Math.Max(ptBestNearest.M, ptWorstNearest.M);

				leg.ObstacleList[j].ApplNetGradient2 = WorstGr;

				leg.ObstacleList[j].Y = b;
				//if(prevlrgOnHeight)
				//	leg.ObstacleList[j].TotalDist = leg.BestCase.PrevTotalLength + leg.ObstacleList[j].X;
				//else

				leg.ObstacleList[j].TotalDist = leg.BestCase.PrevTotalLength + leg.ObstacleList[j].X;

				leg.ObstacleList[j].MOC = GlobalVars.TurnMOC;				//GlobalVars.NetMOC;
				leg.ObstacleList[j].MOCH = leg.ObstacleList[j].Height + leg.ObstacleList[j].MOC;

				double LostH = leg.ObstacleList[j].MOCH - GlobalVars.heightAboveDERMax;
				leg.ObstacleList[j].ReqNetGradient = LostH / leg.ObstacleList[j].TotalDist;

				PDGNet = leg.ObstacleList[j].ApplNetGradient2;		//????????????????????????????????????????????????????????????
				leg.ObstacleList[j].AcceleStartDist = LostH / PDGNet;

				if (leg.ObsMaxNetGrd < 0)
					leg.ObsMaxNetGrd = j;
				else if (leg.ObstacleList[j].ReqNetGradient > leg.ObstacleList[leg.ObsMaxNetGrd].ReqNetGradient)
					leg.ObsMaxNetGrd = j;

				if (leg.ObsMaxAcceleDist < 0)
					leg.ObsMaxAcceleDist = j;
				else if (leg.ObstacleList[j].AcceleStartDist > leg.ObstacleList[leg.ObsMaxAcceleDist].AcceleStartDist)
					leg.ObsMaxAcceleDist = j;

				j++;
			}

			System.Array.Resize<ObstacleData>(ref leg.ObstacleList, j);
			return j;
		}

		internal static int GetLegObstacles3Turn(ObstacleData[] AllObsstacles, double PDGNet, double PrevTotalLength, IPolygon pProtectionArea, IPolyline pNominalPoly, IPointCollection MyPC, ref ObstacleData[] CaseObstacles)
		{
			IRelationalOperator pProtReleation = (IRelationalOperator)(pProtectionArea);
			IProximityOperator pNomProxi = (IProximityOperator)(pNominalPoly);
			IProximityOperator pFromReleation = (IProximityOperator)(pNominalPoly.FromPoint);
			ITopologicalOperator2 pNomTopo = (ITopologicalOperator2)pNominalPoly;

			pNomTopo.IsKnownSimple_2 = false;
			pNomTopo.Simplify();

			IPolyline pObstacleLine = (IPolyline)new Polyline();
			IGeometry pPart1, pPart2;
			IPolyline pLinePart;

			//leg.ObsMaxNetGrd = -1;
			//leg.ObsMaxAcceleDist = -1;

			int j = 0, n = AllObsstacles.Length;
			CaseObstacles = new ObstacleData[n];

			for (int i = 0; i < n; i++)
			{
				if (!pProtReleation.Contains(AllObsstacles[i].pPtPrj))
					continue;

				CaseObstacles[j] = AllObsstacles[i];

				IPoint ptNearest = pNomProxi.ReturnNearestPoint(CaseObstacles[j].pPtPrj, esriSegmentExtension.esriNoExtension);

				//if (CaseObstacles[j].ID == "Ter-9115" || CaseObstacles[j].ID == "Ter-9116")
				//{
				//    Graphics.DrawPointWithText(CaseObstacles[j].pPtPrj, CaseObstacles[j].ID);
				//    Graphics.DrawPointWithText(ptNearest, "Nearest-" + CaseObstacles[j].ID);
				//    Graphics.DrawPolyline(pNominalPoly);
				//    ProcessMessages();
				//}

				//if (CaseObstacles[j].ID == "Ter-1978")
				//{
				//Graphics.DrawPointWithText(CaseObstacles[j].pPtPrj, "Ter-1978");
				//Graphics.DrawPointWithText(ptNearest, "Nearest-1978");
				//Graphics.DrawPolyline(pNominalPoly);
				//ProcessMessages();
				//}

				//Graphics.DrawPointWithText(CaseObstacles[j].pPtPrj, "Obst-1");
				//Graphics.DrawPointWithText(ptNearest, "Nearest-1");
				//Graphics.DrawPolyline(pNominalPoly);
				//ProcessMessages();

				double dir = ReturnAngleInDegrees(ptNearest, CaseObstacles[j].pPtPrj);
				pObstacleLine.FromPoint = LocalToPrj(ptNearest, dir, 100000.0);
				pObstacleLine.ToPoint = LocalToPrj(ptNearest, dir, -100000.0);

				pNomTopo.Cut(pObstacleLine, out pPart1, out pPart2);

				//Graphics.DrawPolyline(pNominalPoly, Functions.RGB(0, 255, 0));
				//Graphics.DrawPolyline(pObstacleLine, Functions.RGB(0, 0, 255));
				//ProcessMessages();

				//Graphics.DrawPolyline((IPolyline)pPart1, Functions.RGB(255, 0, 0),2);
				//Graphics.DrawPolyline((IPolyline)pPart2, Functions.RGB(0, 0, 0),2);
				//ProcessMessages();

				if (pPart1.IsEmpty && pPart2.IsEmpty)
					continue;

				if (pPart1.IsEmpty)
					pLinePart = (IPolyline)pPart2;
				else if (pPart2.IsEmpty)
					pLinePart = (IPolyline)pPart1;
				else if (pFromReleation.ReturnDistance(pPart1) < pFromReleation.ReturnDistance(pPart2))
					pLinePart = (IPolyline)pPart1;
				else
					pLinePart = (IPolyline)pPart2;

				//Graphics.DrawPolyline(pLinePart, 255, 2);
				//ProcessMessages();
				//Graphics.DrawPointWithText(CaseObstacles[j].pPtPrj, "leg.Obst");

				IGeometryCollection pGeoColl = (IGeometryCollection)pLinePart, pTmpGeoColl = (IGeometryCollection)new Polyline();
				int g = 0;
				while (pGeoColl.GeometryCount > 1)
				{
					pTmpGeoColl.RemoveGeometries(0, pTmpGeoColl.GeometryCount);
					pTmpGeoColl.AddGeometry(pGeoColl.Geometry[g]);
					Double fDist = pFromReleation.ReturnDistance((IGeometry)pTmpGeoColl);

					if (fDist * fDist > GlobalVars.distEps)
						pGeoColl.RemoveGeometries(g, 1);
					else
						g++;
				}

				//Graphics.DrawPolyline(pLinePart, 0, 2);
				//ProcessMessages();

				CaseObstacles[j].X = pLinePart.Length;

				CaseObstacles[j].ApplNetGradient2 = PDGNet;

				//if (CaseObstacles[j].ID == "Ter-9115" || CaseObstacles[j].ID == "Ter-9116")
				//    ProcessMessages();


				CaseObstacles[j].Y = pNomProxi.ReturnDistance(CaseObstacles[j].pPtPrj);
				CaseObstacles[j].TotalDist = PrevTotalLength + CaseObstacles[j].X;

				//MyPC
				bool inTurnArea = true;

				IPoint pt0 = MyPC.Point[0];

				for (int k = 1; k < MyPC.PointCount; k++)
				{
					IPoint pt1 = MyPC.Point[k];
					if (SubtractAngles(pt1.M, pt0.M) < GlobalVars.degEps)
					{
						double dx1 = ptNearest.X - pt0.X, dy1 = ptNearest.Y - pt0.Y;
						double dx2 = pt1.X - ptNearest.X, dy2 = pt1.Y - ptNearest.Y;

						//Graphics.DrawPointWithText(pt0, "pt0");
						//Graphics.DrawPointWithText(pt1, "pt1");
						//ProcessMessages();

						if (Math.Abs(dy2 * dx1 - dx2 * dy1) < 0.5)
						{
							inTurnArea = false;
							break;
						}
					}
					pt0 = pt1;
				}

				CaseObstacles[j].MOC = GlobalVars.NetMOC;
				if (inTurnArea)
					CaseObstacles[j].MOC = GlobalVars.TurnMOC;

				CaseObstacles[j].MOCH = CaseObstacles[j].Height + CaseObstacles[j].MOC;

				double LostH = CaseObstacles[j].MOCH - GlobalVars.heightAboveDERMax;
				CaseObstacles[j].ReqNetGradient = LostH / CaseObstacles[j].TotalDist;

				PDGNet = CaseObstacles[j].ApplNetGradient2;		//????????????????????????????????????????????????????????????
				CaseObstacles[j].AcceleStartDist = LostH / PDGNet;

				//Graphics.DrawPolyline(pLinePart, 255, 2);

				//{
				//	if (ReturnDistanceInMeters(pLinePart.FromPoint, leg.pNominalPoly.FromPoint) > ReturnDistanceInMeters(pLinePart.ToPoint, leg.pNominalPoly.FromPoint))
				//		pLinePart.ReverseOrientation();
				//}
				//PrjToLocal(SelectedRWY.pPtPrj[eRWY.PtDER], SelectedRWY.pPtPrj[eRWY.PtDER].M, CaseObstacles[j].pPtPrj, out CaseObstacles[j].X, out CaseObstacles[j].Y);

				//if (leg.ObsMaxNetGrd < 0)
				//    leg.ObsMaxNetGrd = j;
				//else if (CaseObstacles[j].ReqNetGradient > CaseObstacles[leg.ObsMaxNetGrd].ReqNetGradient)
				//    leg.ObsMaxNetGrd = j;

				//if (leg.ObsMaxAcceleDist < 0)
				//    leg.ObsMaxAcceleDist = j;
				//else if (CaseObstacles[j].AcceleDist > CaseObstacles[leg.ObsMaxAcceleDist].AcceleDist)
				//    leg.ObsMaxAcceleDist = j;

				j++;
			}

			System.Array.Resize<ObstacleData>(ref CaseObstacles, j);
			return j;
		}

		internal static int GetLegObstacles(ObstacleData[] AllObsstacles, ref TrackLeg leg)
		{
			int nTurns = Math.Max(leg.WorstCase.turns, leg.BestCase.turns);
			if (nTurns != 0)
			{
				if (leg.SegmentCode == eLegType.courseIntercept)
					return GetCourseInterceptLegObstacles(AllObsstacles, ref leg);

				if (nTurns == 1)
				{
					if (leg.BestCase.Turn[0].StartDist > 0.0)
						return GetCourseInterceptLegObstacles(AllObsstacles, ref leg);
					else
						return GetLegObstacles1TurnS(AllObsstacles, ref leg);
				}

				if (nTurns == 2)
					return GetLegObstacles2Turn(AllObsstacles, ref leg);
			}

			return GetLegObstaclesNoTurn(AllObsstacles, ref leg);

			//double PDGNet = leg.WorstCase.NetGrd;

			//IRelationalOperator pReleation = (IRelationalOperator)(leg.pProtectionArea);

			//IProximityOperator pBestProxi = (IProximityOperator)(leg.BestCase.pNominalPoly);
			//IProximityOperator pBestNomReleation = (IProximityOperator)(leg.BestCase.pNominalPoly.FromPoint);

			//IProximityOperator pWorstProxi = (IProximityOperator)(leg.WorstCase.pNominalPoly);
			//IProximityOperator pWorstNomReleation = (IProximityOperator)(leg.WorstCase.pNominalPoly.FromPoint);

			//ITopologicalOperator2 pBestTopo = (ITopologicalOperator2)leg.BestCase.pNominalPoly;
			//pBestTopo.IsKnownSimple_2 = false;
			//pBestTopo.Simplify();

			//ITopologicalOperator2 pWorstTopo = (ITopologicalOperator2)leg.WorstCase.pNominalPoly;
			//pWorstTopo.IsKnownSimple_2 = false;
			//pWorstTopo.Simplify();

			//IPolyline pObstacleLine = (IPolyline)new Polyline();
			//IGeometry pBestPart1, pBestPart2;
			//IPolyline pBestLinePart;

			//IGeometry pWorstPart1, pWorstPart2;
			//IPolyline pWorstLinePart;

			//double BestStartDist1 = 0, BestStartDist2 = 0;
			//double BestEndDist1 = 0, BestEndDist2 = 0;


			//leg.ObsMaxNetGrd = -1;
			//leg.ObsMaxAcceleDist = -1;

			//int j = 0, n = AllObsstacles.Length;
			//leg.ObstacleList = new ObstacleData[n];

			//for (int i = 0; i < n; i++)
			//{
			//    if (!pReleation.Contains(AllObsstacles[i].pPtPrj))
			//        continue;

			//    leg.ObstacleList[j] = AllObsstacles[i];

			//    inLine = true;
			//    //if (nTurns > 0)
			//    //{
			//    //    double dir1 = ReturnAngleInDegrees(leg.BestCase.Turn[0].ptStart, leg.WorstCase.Turn[0].ptStart);
			//    //    double dir2 = ReturnAngleInDegrees(leg.BestCase.Turn[0].ptCenter, leg.WorstCase.Turn[0].ptCenter);
			//    //    double dir3 = ReturnAngleInDegrees(leg.BestCase.Turn[0].ptEnd, leg.WorstCase.Turn[0].ptEnd);
			//    //}

			//    if (inLine)
			//    {
			//        IPoint ptBestNearest = pBestProxi.ReturnNearestPoint(leg.ObstacleList[j].pPtPrj, esriSegmentExtension.esriNoExtension);
			//        IPoint ptWorstNearest = pWorstProxi.ReturnNearestPoint(leg.ObstacleList[j].pPtPrj, esriSegmentExtension.esriNoExtension);

			//        //Graphics.DrawPolyline(leg.WorstCase.pNominalPoly, -1, 2);
			//        //Graphics.DrawPolyline(leg.BestCase.pNominalPoly, -1, 2);
			//        //Graphics.DrawPointWithText(leg.ObstacleList[j].pPtPrj, "Obst");
			//        //Graphics.DrawPointWithText(ptBestNearest, "Bestpt");
			//        //Graphics.DrawPointWithText(ptWorstNearest, "Worstpt");
			//        //Graphics.DrawPolygon(leg.pProtectionArea);
			//        //ProcessMessages();

			//        double dir = ReturnAngleInDegrees(ptBestNearest, leg.ObstacleList[j].pPtPrj);
			//        pObstacleLine.FromPoint = LocalToPrj(ptBestNearest, dir, 100000.0);
			//        pObstacleLine.ToPoint = LocalToPrj(ptBestNearest, dir, -100000.0);

			//        pBestTopo.Cut(pObstacleLine, out pBestPart1, out pBestPart2);

			//        //Graphics.DrawPointWithText(ptNearest, "ptNearest");
			//        //Graphics.DrawPolyline(pObstacleLine, Functions.RGB(0, 0, 255));
			//        //Graphics.DrawPolyline(leg.pNominalPoly, Functions.RGB(0, 255, 0));


			//        //Graphics.DrawPolyline((IPolyline)pPart1, Functions.RGB(255, 0, 0),2);
			//        //Graphics.DrawPolyline((IPolyline)pPart2, Functions.RGB(0, 0, 0),2);
			//        //ProcessMessages();

			//        if (pBestPart1.IsEmpty && pBestPart2.IsEmpty)
			//            continue;

			//        dir = ReturnAngleInDegrees(ptWorstNearest, leg.ObstacleList[j].pPtPrj);
			//        pObstacleLine.FromPoint = LocalToPrj(ptWorstNearest, dir, 100000.0);
			//        pObstacleLine.ToPoint = LocalToPrj(ptWorstNearest, dir, -100000.0);
			//        pWorstTopo.Cut(pObstacleLine, out pWorstPart1, out pWorstPart2);

			//        if (pWorstPart1.IsEmpty && pWorstPart2.IsEmpty)
			//            continue;

			//        if (pBestPart1.IsEmpty)
			//            pBestLinePart = (IPolyline)pBestPart2;
			//        else if (pBestPart2.IsEmpty)
			//            pBestLinePart = (IPolyline)pBestPart1;
			//        else if (pBestNomReleation.ReturnDistance(pBestPart1) < pBestNomReleation.ReturnDistance(pBestPart2))
			//            pBestLinePart = (IPolyline)pBestPart1;
			//        else
			//            pBestLinePart = (IPolyline)pBestPart2;


			//        if (pWorstPart1.IsEmpty)
			//            pWorstLinePart = (IPolyline)pWorstPart2;
			//        else if (pWorstPart2.IsEmpty)
			//            pWorstLinePart = (IPolyline)pWorstPart1;
			//        else if (pWorstNomReleation.ReturnDistance(pWorstPart1) < pWorstNomReleation.ReturnDistance(pWorstPart2))
			//            pWorstLinePart = (IPolyline)pWorstPart1;
			//        else
			//            pWorstLinePart = (IPolyline)pWorstPart2;

			//        //Graphics.DrawPolyline(pLinePart, 255, 2);
			//        //Graphics.DrawPointWithText(leg.ObstacleList[j].pPtPrj, "leg.Obst");

			//        leg.ObstacleList[j].X = pBestLinePart.Length;
			//        leg.ObstacleList[j].Y = pBestProxi.ReturnDistance(leg.ObstacleList[j].pPtPrj);
			//        leg.ObstacleList[j].TotalDist = leg.BestCase.PrevTotalLength + leg.ObstacleList[j].X;

			//        leg.ObstacleList[j].MOC = GlobalVars.NetMOC;

			//        if (leg.BestCase.turns >= 0)
			//        {
			//            if ((leg.type != LegType.arcPath && leg.ObstacleList[j].X > BestStartDist1 && leg.ObstacleList[j].X < BestEndDist1) || (leg.BestCase.turns == 2 && leg.ObstacleList[j].X > BestStartDist2 && leg.ObstacleList[j].X < BestEndDist2))
			//                leg.ObstacleList[j].MOC = GlobalVars.TurnMOC;
			//        }
			//    }

			//    leg.ObstacleList[j].MOCH = leg.ObstacleList[j].Height + leg.ObstacleList[j].MOC;

			//    double LostH = leg.ObstacleList[j].MOCH - GlobalVars.heightAboveDERMax;
			//    leg.ObstacleList[j].ReqNetGradient = LostH / leg.ObstacleList[j].TotalDist;
			//		PDGNet = leg.ObstacleList[j].ApplNetGradient;		//????????????????????????????????????????????????????????????
			//    leg.ObstacleList[j].AcceleDist = LostH / PDGNet;

			//    //Graphics.DrawPolyline(pLinePart, 255, 2);
			//    /*{
			//        if (ReturnDistanceInMeters(pLinePart.FromPoint, leg.pNominalPoly.FromPoint) > ReturnDistanceInMeters(pLinePart.ToPoint, leg.pNominalPoly.FromPoint))
			//            pLinePart.ReverseOrientation();
			//    }*/
			//    //PrjToLocal(SelectedRWY.pPtPrj[eRWY.PtDER], SelectedRWY.pPtPrj[eRWY.PtDER].M, leg.ObstacleList[j].pPtPrj, out leg.ObstacleList[j].X, out leg.ObstacleList[j].Y);

			//    if (leg.ObsMaxNetGrd < 0)
			//        leg.ObsMaxNetGrd = j;
			//    else if (leg.ObstacleList[j].ReqNetGradient > leg.ObstacleList[leg.ObsMaxNetGrd].ReqNetGradient)
			//        leg.ObsMaxNetGrd = j;

			//    if (leg.ObsMaxAcceleDist < 0)
			//        leg.ObsMaxAcceleDist = j;
			//    else if (leg.ObstacleList[j].AcceleDist > leg.ObstacleList[leg.ObsMaxAcceleDist].AcceleDist)
			//        leg.ObsMaxAcceleDist = j;

			//    j++;
			//}

			//System.Array.Resize<ObstacleData>(ref leg.ObstacleList, j);
			//return j;
		}

		//internal static int GetSegmentObstacles(ObstacleType[] AllObsstacles, out ObstacleType[] resultObstacles, IPolygon testPoly)
		//{
		//    int j = 0, n = AllObsstacles.Length;
		//    resultObstacles = new ObstacleType[n];

		//    IRelationalOperator pReleation = (IRelationalOperator)testPoly;

		//    for (int i = 0; i < n; i++)
		//        if (pReleation.Contains(AllObsstacles[i].pPtPrj))
		//            resultObstacles[j++] = AllObsstacles[i];

		//    System.Array.Resize<ObstacleType>(ref resultObstacles, j);
		//    return j;
		//}

		internal static bool AngleInSector(double Angle, double X, double Y)
		{
			double Sector = NativeMethods.Modulus(Y - X);	//SubtractAngles(X, Y);
			double Sub1 = NativeMethods.Modulus(Angle - X);
			double Sub2 = NativeMethods.Modulus(Y - Angle);

			return !(Sub1 + Sub2 > Sector + GlobalVars.degEps);
		}

		internal static bool AngleInInterval(double Ang, Interval inter)
		{
			if (inter.Left == -2)
				return false;

			if (inter.Right == -1)
			{
				if (inter.Left == Math.Round(Ang, 1))
					return true;
				return false;
			}

			inter.Left = NativeMethods.Modulus(inter.Left, 360.0);
			inter.Right = NativeMethods.Modulus(inter.Right, 360.0);
			Ang = NativeMethods.Modulus(Ang, 360.0);

			if (inter.Left > inter.Right)
			{
				if (Ang >= inter.Left || Ang <= inter.Right)
					return true;
			}
			else
				if (Ang >= inter.Left && Ang <= inter.Right)
					return true;

			return false;
		}

		internal static double SubtractAngles(double X, double Y)
		{
			X = NativeMethods.Modulus(X, 360.0);
			Y = NativeMethods.Modulus(Y, 360.0);
			double result = NativeMethods.Modulus(X - Y, 360.0);
			if (result > 180.0)
				return 360.0 - result;
			return result;
		}

		internal static IPoint CenterOfTurn(IPoint FromPt, IPoint ToPt)
		{
			double fTmp = GlobalVars.DegToRadValue * (FromPt.M - ToPt.M);
			IPoint pPtX = new Point();

			if (Math.Abs(Math.Sin(fTmp)) > GlobalVars.DegToRadValue * 0.5)
			{
				IConstructPoint ptConstr = (IConstructPoint)pPtX;
				ptConstr.ConstructAngleIntersection(FromPt, GlobalVars.DegToRadValue * (NativeMethods.Modulus(FromPt.M + 90.0, 360.0)), ToPt, GlobalVars.DegToRadValue * (NativeMethods.Modulus(ToPt.M + 90.0, 360.0)));
			}
			else
				pPtX.PutCoords(0.5 * (FromPt.X + ToPt.X), 0.5 * (FromPt.Y + ToPt.Y));

			return pPtX;
		}

		internal static string FormatInterval(Interval iv)
		{
			if (iv.Right == -1)
				return (iv.Left).ToString() + "°";

			return "от " + NativeMethods.Modulus(iv.Left).ToString() + "° до " + NativeMethods.Modulus(iv.Right).ToString() + "°";
		}

		public static bool ShowSaveDialog(out string FileName, out string FileTitle)
		{
			System.Windows.Forms.SaveFileDialog SaveDialog1 = new System.Windows.Forms.SaveFileDialog();
			string ProjectPath = GlobalVars.GetMapFileName();
			int pos = ProjectPath.LastIndexOf('\\');
			int pos2 = ProjectPath.LastIndexOf('.');

			SaveDialog1.DefaultExt = "";
			SaveDialog1.InitialDirectory = ProjectPath.Substring(0, pos);
			//SaveDialog1.Title = Aran.Panda.Departure.Properties.Resources.str0511;
			SaveDialog1.FileName = ProjectPath.Substring(0, pos2 - 1) + ".htm";
			SaveDialog1.Filter = "PANDA Report File (*.htm)|*.htm|All Files (*.*)|*.*";

			FileTitle = "";
			FileName = "";

			if (SaveDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				FileName = SaveDialog1.FileName;

				pos = FileName.LastIndexOf('.');
				if (pos > 0)
					FileName = FileName.Substring(0, pos);

				FileTitle = FileName;
				pos2 = FileTitle.LastIndexOf('\\');
				if (pos2 > 0)
					FileTitle = FileTitle.Substring(pos2 + 1);

				return true;
			}

			return false;
		}

#if experimental
		#region Event types
		struct IntersectData
		{
		    public IPoint pPoint;
		    public bool IsPoint;
		    public double leftDir, rightDir;
		}

		enum NominalEventType
		{
		    point,
		    turn,
		    intersect,
		    changeWidth
		}

		struct NominalEvent
		{
		    public NominalEventType eventType;
		    public LegPoint point;
		    public TurnData turn;
		    public IntersectData intersect;
		    public ChangeWidthData changeWidth;
		    public double distFromLeftOrigin;
		    public double distFromRightOrigin;
		    public double LeftWidth;
		    public double RightWidth;
		}
		#endregion

		internal static void CreateSideLine_New(ref TrackLeg segment, ref IPointCollection ptColl, int Side)
		{
			IPolyline pLeftNominal;
			IPolyline pRightNominal;

			ITopologicalOperator2 pTopo = (ITopologicalOperator2)segment.BestCase.pNominalPoly;

			IPointCollection pPoints = (IPointCollection)pTopo.Intersect(segment.WorstCase.pNominalPoly, esriGeometryDimension.esriGeometry0Dimension);
			IPointCollection pLines = (IPointCollection)pTopo.Intersect(segment.WorstCase.pNominalPoly, esriGeometryDimension.esriGeometry1Dimension);
			IPointCollection pCommon = new Multipoint();

			NominalEvent[] leftEvents, rightEvents;
			//IMCollection  imColl = (IMCollection)segment.BestCase.pNominalPoly;
			//IMSegmentation imSeg = (IMSegmentation)segment.BestCase.pNominalPoly;


			int leftCnt = 0, rightCnt = 0;
			leftEvents = new NominalEvent[pPoints.PointCount + pLines.PointCount + 4];
			rightEvents = new NominalEvent[pPoints.PointCount + pLines.PointCount + 4];

			IPoint LeftOrigin, RightOrigin;
			double dist = ReturnDistanceInMeters(segment.BestCase.pNominalPoly.FromPoint, segment.WorstCase.pNominalPoly.FromPoint);

			if (dist > GlobalVars.distEps)
			{
				SideDirection wichSide = SideDef(segment.BestCase.pNominalPoly.FromPoint, segment.BestCase.pNominalPoly.FromPoint.M, segment.WorstCase.pNominalPoly.FromPoint);
				if (wichSide == SideDirection.sideOn)
				{
					wichSide = SideDef(segment.BestCase.pNominalPoly.FromPoint, segment.BestCase.pNominalPoly.FromPoint.M + 90.0, segment.WorstCase.pNominalPoly.FromPoint);

					if (segment.BestCase.turns > 0 && segment.BestCase.Turn.Length > GlobalVars.distEps)
					{
						if (segment.BestCase.Turn[0].TurnDir * (int)wichSide < 0)
						{
							pRightNominal = segment.BestCase.pNominalPoly;
							pLeftNominal = segment.WorstCase.pNominalPoly;

							RightOrigin = segment.BestCase.pNominalPoly.FromPoint;
							LeftOrigin = segment.WorstCase.pNominalPoly.FromPoint;
						}
						else
						{
							pRightNominal = segment.WorstCase.pNominalPoly;
							pLeftNominal = segment.BestCase.pNominalPoly;

							RightOrigin = segment.WorstCase.pNominalPoly.FromPoint;
							LeftOrigin = segment.BestCase.pNominalPoly.FromPoint;
						}
					}
					else
					{
						if (wichSide == SideDirection.sideLeft)
						{
							pRightNominal = pLeftNominal = segment.WorstCase.pNominalPoly;
							LeftOrigin = RightOrigin = segment.WorstCase.pNominalPoly.FromPoint;
						}
						else
						{
							pRightNominal = pLeftNominal = segment.BestCase.pNominalPoly;
							LeftOrigin = RightOrigin = segment.BestCase.pNominalPoly.FromPoint;
						}
					}
				}
				else if (wichSide == SideDirection.sideLeft)
				{
					pRightNominal = segment.BestCase.pNominalPoly;
					pLeftNominal = segment.WorstCase.pNominalPoly;

					RightOrigin = segment.BestCase.pNominalPoly.FromPoint;
					LeftOrigin = segment.WorstCase.pNominalPoly.FromPoint;
				}
				else
				{
					pRightNominal = segment.WorstCase.pNominalPoly;
					pLeftNominal = segment.BestCase.pNominalPoly;

					RightOrigin = segment.WorstCase.pNominalPoly.FromPoint;
					LeftOrigin = segment.BestCase.pNominalPoly.FromPoint;
				}
			}
			else
			{
				pRightNominal = pLeftNominal = segment.BestCase.pNominalPoly;
				LeftOrigin = RightOrigin = segment.BestCase.pNominalPoly.FromPoint;
			}

			ICurve worstCurve = segment.WorstCase.pNominalPoly, bestCurve = segment.BestCase.pNominalPoly;
			ICurve leftCurve = pLeftNominal, rightCurve = pRightNominal;
			IPoint pPtTmp = new Point();
			ILine worstTangent = new Line();
			ILine bestTangent = new Line();

			double distAlong = 0, distFrom = 0;
			bool bRightSide = false;

			//if (Side == -1)
			//{
			//    Graphics.DrawPointWithText(RightOrigin, "RightOrigin");
			//    Graphics.DrawPointWithText(LeftOrigin, "LeftOrigin");
			//}

			//if (pPoints.PointCount ==0)				pPoints = pLines;

			for (int i = 0; i < pPoints.PointCount; i++)
			{
				leftEvents[leftCnt].eventType = NominalEventType.intersect;

				worstCurve.QueryTangent(esriSegmentExtension.esriNoExtension, distAlong, false, 1, worstTangent);
				bestCurve.QueryTangent(esriSegmentExtension.esriNoExtension, distAlong, false, 1, bestTangent);

				leftCurve.QueryPointAndDistance(esriSegmentExtension.esriNoExtension, pPoints.Point[i], false, pPtTmp, ref distAlong, ref distFrom, ref bRightSide);


				leftEvents[leftCnt].distFromLeftOrigin = distAlong;

				rightCurve.QueryPointAndDistance(esriSegmentExtension.esriNoExtension, pPoints.Point[i], false, pPtTmp, ref distAlong, ref distFrom, ref bRightSide);
				leftEvents[leftCnt].distFromRightOrigin = distAlong;

				leftEvents[leftCnt].LeftWidth = 0.0;
				leftEvents[leftCnt].RightWidth = 0.0;

				leftEvents[leftCnt].intersect.pPoint = pPoints.Point[i];
				leftEvents[leftCnt].intersect.IsPoint = true;
				leftEvents[leftCnt].intersect.leftDir = 0.0;

				leftEvents[leftCnt].intersect.rightDir = 0.0;

				rightEvents[rightCnt].eventType = NominalEventType.intersect;
				rightEvents[rightCnt].distFromLeftOrigin = leftEvents[leftCnt].distFromLeftOrigin;
				rightEvents[rightCnt].distFromRightOrigin = leftEvents[leftCnt].distFromRightOrigin;
				rightEvents[rightCnt].LeftWidth = 0.0;
				rightEvents[rightCnt].RightWidth = 0.0;

				rightEvents[rightCnt].intersect.pPoint = pPoints.Point[i];
				rightEvents[rightCnt].intersect.IsPoint = true;
				rightEvents[rightCnt].intersect.leftDir = 0.0;
				rightEvents[rightCnt].intersect.rightDir = 0.0;

				//Graphics.DrawPointWithText(pPoints.Point[i], "pt-" + (i + 1).ToString());
				leftCnt++;
				rightCnt++;
			}

			for (int i = 0; i < pLines.PointCount; i++)
			{
				leftEvents[leftCnt].eventType = NominalEventType.intersect;

				leftCurve.QueryPointAndDistance(esriSegmentExtension.esriNoExtension, pPoints.Point[i], false, pPtTmp, ref distAlong, ref distFrom, ref bRightSide);
				leftEvents[leftCnt].distFromLeftOrigin = distAlong;

				rightCurve.QueryPointAndDistance(esriSegmentExtension.esriNoExtension, pPoints.Point[i], false, pPtTmp, ref distAlong, ref distFrom, ref bRightSide);
				leftEvents[leftCnt].distFromRightOrigin = distAlong;
				leftEvents[leftCnt].LeftWidth = 0.0;
				leftEvents[leftCnt].RightWidth = 0.0;

				leftEvents[leftCnt].intersect.pPoint = pLines.Point[i];
				leftEvents[leftCnt].intersect.IsPoint = false;
				leftEvents[leftCnt].intersect.leftDir = 0.0;
				leftEvents[leftCnt].intersect.rightDir = 0.0;


				rightEvents[rightCnt].eventType = NominalEventType.intersect;
				rightEvents[rightCnt].distFromLeftOrigin = leftEvents[leftCnt].distFromLeftOrigin;
				rightEvents[rightCnt].distFromRightOrigin = leftEvents[leftCnt].distFromRightOrigin;
				rightEvents[rightCnt].LeftWidth = 0.0;
				rightEvents[rightCnt].RightWidth = 0.0;

				rightEvents[rightCnt].intersect.pPoint = pLines.Point[i];
				rightEvents[rightCnt].intersect.IsPoint = false;
				rightEvents[rightCnt].intersect.leftDir = 0.0;
				rightEvents[rightCnt].intersect.rightDir = 0.0;

				//Graphics.DrawPointWithText(pPoints.Point[i], "ln-" + (i + 1).ToString());
				leftCnt++;
				rightCnt++;
			}

			double splayRate = GlobalVars.SplayRate, currLength = 0.0,
				currWidth = segment.ptStart.BestCase.Width, currDir = segment.ptStart.BestCase.Direction,
				endWidth = segment.PlannedEndWidth;

			if (segment.SegmentCode == eLegType.arcPath)
			{
				if (currWidth < GlobalVars.ArcProtectWidth)
					currWidth = GlobalVars.ArcProtectWidth;

				if (endWidth < GlobalVars.ArcProtectWidth)
					endWidth = GlobalVars.ArcProtectWidth;
			}

			double splayLen = (endWidth - currWidth) / splayRate;
			double toSplay = splayLen;

			if (currWidth >= endWidth)
				splayRate = 0.0;


			IPoint ptCurr = LocalToPrj(segment.ptStart.BestCase.pPoint, currDir, 0.0, Side * currWidth);
			IPoint ptEnd = segment.BestCase.Turn[0].ptEnd;

			ptColl.AddPoint(ptCurr);

			double straigtLen;
			if (segment.BestCase.turns > 0)
			{
				straigtLen = ReturnDistanceInMeters(segment.ptStart.BestCase.pPoint, segment.BestCase.Turn[0].ptStart);

				if (Side * segment.BestCase.Turn[0].TurnDir > 0.0)
					straigtLen += ReturnDistanceInMeters(segment.ptStart.BestCase.pPoint, segment.ptStart.WorstCase.pPoint);
			}
			else
				straigtLen = ReturnDistanceInMeters(segment.ptStart.BestCase.pPoint, segment.ptEnd.BestCase.pPoint);

			if (straigtLen > GlobalVars.distEps && currWidth < endWidth)
			{
				if (splayLen < straigtLen)
				{
					currWidth = endWidth;
					ptCurr = LocalToPrj(segment.ptStart.BestCase.pPoint, currDir, splayLen, Side * currWidth);
					ptColl.AddPoint(ptCurr);

					splayRate = 0.0;
				}
				else
					currWidth += straigtLen * splayRate;

			}

			ptCurr = LocalToPrj(segment.ptStart.BestCase.pPoint, currDir, straigtLen, Side * currWidth);
			ptColl.AddPoint(ptCurr);
			currLength = straigtLen;
			toSplay = splayLen - currLength;

			if (segment.BestCase.turns > 0)
			{
				double turnAngle = segment.BestCase.Turn[0].Angle;
				double arcLen = segment.BestCase.Turn[0].Radius * DegToRad(turnAngle);

				if (arcLen > GlobalVars.distEps)
				{
					if (currWidth < endWidth)
					{
						if (toSplay < arcLen)
							turnAngle = RadToDeg(toSplay / segment.BestCase.Turn[0].Radius);

						int n = (int)turnAngle;

						if (n < 1) n = 1;
						else if (n < 5) n = 5;
						else if (n < 10) n = 10;

						double angStep = turnAngle / n;

						IPoint ptCenter = segment.BestCase.Turn[0].ptCenter;
						if (Side * segment.BestCase.Turn[0].TurnDir > 0.0)
							ptCenter = LocalToPrj(ptCenter, currDir, ReturnDistanceInMeters(segment.ptStart.BestCase.pPoint, segment.ptStart.WorstCase.pPoint), 0.0);

						ptCurr = new Point();

						for (int i = 0; i < n; i++)
						{
							double angleInRad = GlobalVars.DegToRadValue * (currDir + (90.0 - i * angStep) * segment.BestCase.Turn[0].TurnDir);
							double currRadius = segment.BestCase.Turn[0].Radius + currWidth * Side * segment.BestCase.Turn[0].TurnDir;

							ptCurr.X = ptCenter.X + currRadius * System.Math.Cos(angleInRad);
							ptCurr.Y = ptCenter.Y + currRadius * System.Math.Sin(angleInRad);
							ptColl.AddPoint(ptCurr);

							currWidth += segment.BestCase.Turn[0].Radius * GlobalVars.DegToRadValue * angStep * splayRate;
						}

						currDir -= turnAngle * segment.BestCase.Turn[0].TurnDir;
						turnAngle = segment.BestCase.Turn[0].Angle - turnAngle;
					}

					if (toSplay < arcLen)
					{
						int n = (int)turnAngle;

						if (n < 1) n = 1;
						else if (n < 5) n = 5;
						else if (n < 10) n = 10;

						double angStep = turnAngle / n;

						IPoint ptCenter = segment.BestCase.Turn[0].ptCenter;
						if (Side * segment.BestCase.Turn[0].TurnDir > 0.0)
							ptCenter = LocalToPrj(ptCenter, currDir, ReturnDistanceInMeters(segment.ptStart.BestCase.pPoint, segment.ptStart.WorstCase.pPoint), 0.0);

						ptCurr = new Point();

						for (int i = 0; i < n; i++)
						{
							double angleInRad = GlobalVars.DegToRadValue * (currDir + (90.0 - i * angStep) * segment.BestCase.Turn[0].TurnDir);
							double currRadius = segment.BestCase.Turn[0].Radius + currWidth * Side * segment.BestCase.Turn[0].TurnDir;

							ptCurr.X = ptCenter.X + currRadius * System.Math.Cos(angleInRad);
							ptCurr.Y = ptCenter.Y + currRadius * System.Math.Sin(angleInRad);
							ptColl.AddPoint(ptCurr);
						}

						currDir -= turnAngle * segment.BestCase.Turn[0].TurnDir;
					}
				}

				ptCurr = LocalToPrj(ptEnd, currDir, 0.0, Side * currWidth);
				ptColl.AddPoint(ptCurr);

				//Graphics.DrawPolyline(ptColl, 255, 2);
				//Graphics.DrawPolyline(ptColl, 0, 1);
				//ProcessMessages();

				currLength = arcLen;
				toSplay = splayLen - currLength;
			}

			if (segment.BestCase.turns > 1)
			{
				straigtLen = ReturnDistanceInMeters(ptEnd, segment.BestCase.Turn[1].ptStart);


				if (straigtLen > GlobalVars.distEps && currWidth < endWidth)
				{
					if (splayLen < straigtLen)
					{
						currWidth = endWidth;
						splayRate = 0.0;

						ptCurr = LocalToPrj(ptEnd, currDir, splayLen, Side * currWidth);
						ptColl.AddPoint(ptCurr);
					}
					else
						currWidth += straigtLen * splayRate;

					ptCurr = LocalToPrj(segment.BestCase.Turn[0].ptEnd, currDir, straigtLen, Side * currWidth);
					ptColl.AddPoint(ptCurr);
				}
				currLength = straigtLen;
				toSplay = splayLen - currLength;

				double turnAngle = segment.BestCase.Turn[1].Angle;
				double arcLen = segment.BestCase.Turn[1].Radius * DegToRad(turnAngle);

				if (arcLen > GlobalVars.distEps)
				{
					if (currWidth < endWidth)
					{
						if (toSplay < arcLen)
							turnAngle = RadToDeg(toSplay / segment.BestCase.Turn[1].Radius);

						int n = (int)turnAngle;

						if (n < 1) n = 1;
						else if (n < 5) n = 5;
						else if (n < 10) n = 10;

						double angStep = turnAngle / n;

						ptCurr = new Point();

						for (int i = 0; i < n; i++)
						{
							double angleInRad = GlobalVars.DegToRadValue * (currDir + (90.0 - i * angStep) * segment.BestCase.Turn[1].TurnDir);
							double currRadius = segment.BestCase.Turn[1].Radius + currWidth * Side * segment.BestCase.Turn[1].TurnDir;

							ptCurr.X = segment.BestCase.Turn[1].ptCenter.X + currRadius * System.Math.Cos(angleInRad);
							ptCurr.Y = segment.BestCase.Turn[1].ptCenter.Y + currRadius * System.Math.Sin(angleInRad);
							ptColl.AddPoint(ptCurr);

							currWidth += segment.BestCase.Turn[1].Radius * GlobalVars.DegToRadValue * angStep * splayRate;
						}

						currDir -= turnAngle * segment.BestCase.Turn[1].TurnDir;
						turnAngle = segment.BestCase.Turn[1].Angle - turnAngle;
					}

					if (toSplay < arcLen)
					{
						int n = (int)turnAngle;

						if (n < 1) n = 1;
						else if (n < 5) n = 5;
						else if (n < 10) n = 10;

						double angStep = turnAngle / n;

						ptCurr = new Point();

						for (int i = 0; i < n; i++)
						{
							double angleInRad = GlobalVars.DegToRadValue * (currDir + (90.0 - i * angStep) * segment.BestCase.Turn[1].TurnDir);
							double currRadius = segment.BestCase.Turn[1].Radius + currWidth * Side * segment.BestCase.Turn[1].TurnDir;

							ptCurr.X = segment.BestCase.Turn[1].ptCenter.X + currRadius * System.Math.Cos(angleInRad);
							ptCurr.Y = segment.BestCase.Turn[1].ptCenter.Y + currRadius * System.Math.Sin(angleInRad);
							ptColl.AddPoint(ptCurr);
						}

						currDir -= turnAngle * segment.BestCase.Turn[1].TurnDir;
					}
				}

				ptCurr = LocalToPrj(segment.BestCase.Turn[1].ptEnd, currDir, 0.0, currWidth * Side);
				//Graphics.DrawPoint(ptCurr);
				//ProcessMessages();

				ptColl.AddPoint(ptCurr);

				currLength = arcLen;
				toSplay = splayLen - currLength;
			}

			if (segment.BestCase.turns > 0)
			{
				if (segment.BestCase.turns < 2)
					straigtLen = ReturnDistanceInMeters(ptEnd, segment.ptEnd.BestCase.pPoint);
				else
					straigtLen = ReturnDistanceInMeters(segment.BestCase.Turn[1].ptEnd, segment.ptEnd.BestCase.pPoint);

				if (straigtLen > GlobalVars.distEps && currWidth < endWidth)
				{
					if (splayLen < straigtLen)
					{
						currWidth = endWidth;
						splayRate = 0.0;

						ptCurr = LocalToPrj(segment.ptStart.BestCase.pPoint, currDir, splayLen, Side * currWidth);
						ptColl.AddPoint(ptCurr);

					}
					else
						currWidth += straigtLen * splayRate;

					ptCurr = LocalToPrj(segment.ptStart.BestCase.pPoint, currDir, straigtLen, Side * currWidth);
					ptColl.AddPoint(ptCurr);
				}
				currLength = straigtLen;
				toSplay = splayLen - currLength;
			}

			segment.ptEnd.BestCase.Width = currWidth;
		}
#endif

#if OldFashioned
		internal static IPointCollection CalcTouchByFixDir(IPoint PtSt, IPoint PtFix, double TurnR, double DirCur, double DirFix, int TurnDir, double SnapAngle, out double deadReck, out IPoint FlyBy)
		{
			IPointCollection result = new ESRI.ArcGIS.Geometry.Multipoint();

			if (SubtractAngles(DirCur, DirFix) < 0.5)
			{
				DirFix = DirCur;
				if (ReturnDistanceInMeters(PtFix, PtSt) < GlobalVars.distEps)
				{
					FlyBy = new ESRI.ArcGIS.Geometry.Point();
					FlyBy.PutCoords(PtFix.X, PtFix.Y);
					result.AddPoint(PtSt);
					result.AddPoint(PtSt);
					deadReck = 0.0;
					return result;
				}
			}

			IPoint ptCnt1 = LocalToPrj(PtSt, DirCur + 90.0 * TurnDir, TurnR);
			PtSt.M = DirCur;

			//double OutDir0 = NativeMethods.Modulus(DirFix - SnapAngle * TurnDir);
			//double OutDir1 = NativeMethods.Modulus(DirFix + SnapAngle * TurnDir);

			double OutDir0 = DirFix - SnapAngle * TurnDir;
			double OutDir1 = DirFix + SnapAngle * TurnDir;

			IPoint Pt10 = LocalToPrj(ptCnt1, OutDir0 - 90.0 * TurnDir, TurnR);
			IPoint Pt11 = LocalToPrj(ptCnt1, OutDir1 - 90.0 * TurnDir, TurnR);

			int SideT = (int)SideDef(Pt10, DirFix, PtFix);
			int SideD = (int)SideDef(Pt10, DirFix, ptCnt1);

			IPoint pt1;
			double OutDir;

			if (SideT * SideD < 0)
			{
				pt1 = Pt10;
				OutDir = NativeMethods.Modulus(OutDir0);
			}
			else
			{
				pt1 = Pt11;
				OutDir = NativeMethods.Modulus(OutDir1);
			}

			pt1.M = OutDir;

			FlyBy = new ESRI.ArcGIS.Geometry.Point();

			IConstructPoint Constructor = (IConstructPoint)FlyBy;
			Constructor.ConstructAngleIntersection(pt1, GlobalVars.DegToRadValue * OutDir, PtFix, GlobalVars.DegToRadValue * DirFix);

			double Dist = ReturnDistanceInMeters(pt1, FlyBy);
			double dirToTmp = ReturnAngleInDegrees(PtFix, FlyBy);
			double distToTmp = ReturnDistanceInMeters(PtFix, FlyBy);

			SideT = AnglesSideDef(OutDir, DirFix);

			double DeltaAngle = 0.0;

			if (SideT > 0)
				DeltaAngle = 0.5 * NativeMethods.Modulus(180.0 + DirFix - OutDir, 360.0);
			else if (SideT < 0)
				DeltaAngle = 0.5 * NativeMethods.Modulus(OutDir - 180.0 - DirFix, 360.0);

			double DeltaDist = TurnR / System.Math.Tan(GlobalVars.DegToRadValue * DeltaAngle);

			deadReck = Dist - DeltaDist;

			IPoint pt2, pt3;
			if (DeltaDist <= Dist)
			{
				pt2 = LocalToPrj(FlyBy, OutDir - 180.0, DeltaDist);
				pt3 = LocalToPrj(FlyBy, DirFix, DeltaDist);
			}
			else
			{
				pt2 = LocalToPrj(FlyBy, OutDir, DeltaDist);
				pt3 = LocalToPrj(FlyBy, DirFix - 180.0, DeltaDist);
			}

			pt2.M = OutDir;
			pt3.M = DirFix;

			result.AddPoint(PtSt);
			result.AddPoint(pt1);
			result.AddPoint(pt2);
			result.AddPoint(pt3);
			return result;
		}

		internal static void CreateSideLine(ref TrakceLeg segment, ref IPointCollection ptColl, bool bestCase, int Side)
		{
			PointData ptStart, ptEnd;
			LegData data;// = new LegData();
			//data.Initialize();

			if (bestCase)
			{
				data = segment.BestCase;
				ptStart = segment.ptStart.BestCase;
				ptEnd = segment.ptEnd.BestCase;
			}
			else
			{
				data = segment.WorstCase;
				ptStart = segment.ptStart.WorstCase;
				ptEnd = segment.ptEnd.WorstCase;
			}

			double splayRate = GlobalVars.SplayRate, currLength = 0.0,
				currWidth = ptStart.Width, currDir = ptStart.Direction,
				endWidth = segment.PlannedEndWidth;

			if (segment.type == LegType.arcPath)
			{
				if (currWidth < GlobalVars.ArcProtectWidth)
					currWidth = GlobalVars.ArcProtectWidth;

				if (endWidth < GlobalVars.ArcProtectWidth)
					endWidth = GlobalVars.ArcProtectWidth;
			}

			double splayLen = (endWidth - currWidth) / splayRate;
			double toSplay = splayLen;

			if (currWidth >= endWidth)
				splayRate = 0.0;


			IPoint pPtCurr = LocalToPrj(ptStart.pPoint, currDir, 0.0, Side * currWidth);
			IPoint pPtEnd = data.Turn[0].ptEnd;

			ptColl.AddPoint(pPtCurr);

			double straigtLen;
			if (data.turns > 0)
			{
				straigtLen = ReturnDistanceInMeters(ptStart.pPoint, data.Turn[0].ptStart);
			}
			else
				straigtLen = ReturnDistanceInMeters(ptStart.pPoint, ptEnd.pPoint);

			if (straigtLen > GlobalVars.distEps && currWidth < endWidth)
			{
				if (splayLen < straigtLen)
				{
					currWidth = endWidth;
					pPtCurr = LocalToPrj(ptStart.pPoint, currDir, splayLen, Side * currWidth);
					ptColl.AddPoint(pPtCurr);

					splayRate = 0.0;
				}
				else
					currWidth += straigtLen * splayRate;

			}

			pPtCurr = LocalToPrj(ptStart.pPoint, currDir, straigtLen, Side * currWidth);
			ptColl.AddPoint(pPtCurr);
			currLength = straigtLen;
			toSplay = splayLen - currLength;

			if (data.turns > 0)
			{
				double turnAngle = data.Turn[0].Angle;
				double arcLen = data.Turn[0].Radius * DegToRad(turnAngle);

				if (arcLen > GlobalVars.distEps)
				{
					if (currWidth < endWidth)
					{
						if (toSplay < arcLen)
							turnAngle = RadToDeg(toSplay / data.Turn[0].Radius);

						int n = (int)turnAngle;

						if (n < 1) n = 1;
						else if (n < 5) n = 5;
						else if (n < 10) n = 10;

						double angStep = turnAngle / n;

						IPoint pPtCenter = data.Turn[0].ptCenter;

						pPtCurr = new Point();

						for (int i = 0; i < n; i++)
						{
							double angleInRad = GlobalVars.DegToRadValue * (currDir + (90.0 - i * angStep) * data.Turn[0].TurnDir);
							double currRadius = data.Turn[0].Radius + currWidth * Side * data.Turn[0].TurnDir;

							pPtCurr.X = pPtCenter.X + currRadius * System.Math.Cos(angleInRad);
							pPtCurr.Y = pPtCenter.Y + currRadius * System.Math.Sin(angleInRad);
							ptColl.AddPoint(pPtCurr);

							currWidth += data.Turn[0].Radius * GlobalVars.DegToRadValue * angStep * splayRate;
						}

						currDir -= turnAngle * data.Turn[0].TurnDir;
						turnAngle = data.Turn[0].Angle - turnAngle;
					}

					if (toSplay < arcLen)
					{
						int n = (int)turnAngle;

						if (n < 1) n = 1;
						else if (n < 5) n = 5;
						else if (n < 10) n = 10;

						double angStep = turnAngle / n;

						IPoint pPtCenter = data.Turn[0].ptCenter;

						pPtCurr = new Point();

						for (int i = 0; i < n; i++)
						{
							double angleInRad = GlobalVars.DegToRadValue * (currDir + (90.0 - i * angStep) * data.Turn[0].TurnDir);
							double currRadius = data.Turn[0].Radius + currWidth * Side * data.Turn[0].TurnDir;

							pPtCurr.X = pPtCenter.X + currRadius * System.Math.Cos(angleInRad);
							pPtCurr.Y = pPtCenter.Y + currRadius * System.Math.Sin(angleInRad);
							ptColl.AddPoint(pPtCurr);
						}

						currDir -= turnAngle * data.Turn[0].TurnDir;
					}
				}

				//Graphics.DrawPolyline(ptColl, 255, 2);
				//ProcessMessages(true);

					pPtCurr = LocalToPrj(pPtEnd, currDir, 0.0, Side * currWidth);
					ptColl.AddPoint(pPtCurr);

				//Graphics.DrawPolyline(ptColl, 0, 1);
				//ProcessMessages();
				//ptColl.RemovePoints(0, ptColl.PointCount);
				currLength = arcLen;
				toSplay = splayLen - currLength;
			}

			if (data.turns > 1)
			{
				straigtLen = ReturnDistanceInMeters(pPtEnd, data.Turn[1].ptStart);


				if (straigtLen > GlobalVars.distEps && currWidth < endWidth)
				{
					if (splayLen < straigtLen)
					{
						currWidth = endWidth;
						splayRate = 0.0;

						pPtCurr = LocalToPrj(pPtEnd, currDir, splayLen, Side * currWidth);
						ptColl.AddPoint(pPtCurr);
					}
					else
						currWidth += straigtLen * splayRate;

					pPtCurr = LocalToPrj(data.Turn[0].ptEnd, currDir, straigtLen, Side * currWidth);
					ptColl.AddPoint(pPtCurr);
				}
				currLength = straigtLen;
				toSplay = splayLen - currLength;

				double turnAngle = data.Turn[1].Angle;
				double arcLen = data.Turn[1].Radius * DegToRad(turnAngle);

				if (arcLen > GlobalVars.distEps)
				{
					if (currWidth < endWidth)
					{
						if (toSplay < arcLen)
							turnAngle = RadToDeg(toSplay / data.Turn[1].Radius);

						int n = (int)turnAngle;

						if (n < 1) n = 1;
						else if (n < 5) n = 5;
						else if (n < 10) n = 10;

						double angStep = turnAngle / n;

						pPtCurr = new Point();

						for (int i = 0; i < n; i++)
						{
							double angleInRad = GlobalVars.DegToRadValue * (currDir + (90.0 - i * angStep) * data.Turn[1].TurnDir);
							double currRadius = data.Turn[1].Radius + currWidth * Side * data.Turn[1].TurnDir;

							pPtCurr.X = data.Turn[1].ptCenter.X + currRadius * System.Math.Cos(angleInRad);
							pPtCurr.Y = data.Turn[1].ptCenter.Y + currRadius * System.Math.Sin(angleInRad);
							ptColl.AddPoint(pPtCurr);

							currWidth += data.Turn[1].Radius * GlobalVars.DegToRadValue * angStep * splayRate;
						}

						currDir -= turnAngle * data.Turn[1].TurnDir;
						turnAngle = data.Turn[1].Angle - turnAngle;
					}

					if (toSplay < arcLen)
					{
						int n = (int)turnAngle;

						if (n < 1) n = 1;
						else if (n < 5) n = 5;
						else if (n < 10) n = 10;

						double angStep = turnAngle / n;

						pPtCurr = new Point();

						for (int i = 0; i < n; i++)
						{
							double angleInRad = GlobalVars.DegToRadValue * (currDir + (90.0 - i * angStep) * data.Turn[1].TurnDir);
							double currRadius = data.Turn[1].Radius + currWidth * Side * data.Turn[1].TurnDir;

							pPtCurr.X = data.Turn[1].ptCenter.X + currRadius * System.Math.Cos(angleInRad);
							pPtCurr.Y = data.Turn[1].ptCenter.Y + currRadius * System.Math.Sin(angleInRad);
							ptColl.AddPoint(pPtCurr);
						}

						currDir -= turnAngle * data.Turn[1].TurnDir;
					}
				}

				pPtCurr = LocalToPrj(data.Turn[1].ptEnd, currDir, 0.0, currWidth * Side);
				//Graphics.DrawPoint(ptCurr);
				//ProcessMessages();

				ptColl.AddPoint(pPtCurr);

				currLength = arcLen;
				toSplay = splayLen - currLength;
			}

			if (data.turns > 0)
			{
				if (data.turns == 2)
					pPtEnd = data.Turn[1].ptEnd;
				straigtLen = ReturnDistanceInMeters(pPtEnd, ptEnd.pPoint);

				//if (data.turns < 2)
				//    straigtLen = ReturnDistanceInMeters(pPtEnd, ptEnd.pPoint);
				//else
				//    straigtLen = ReturnDistanceInMeters(data.Turn2.ptEnd, ptEnd.pPoint);

				if (straigtLen > GlobalVars.distEps && currWidth < endWidth)
				{
					if (splayLen < straigtLen)
					{
						currWidth = endWidth;
						splayRate = 0.0;

						pPtCurr = LocalToPrj(ptStart.pPoint, currDir, splayLen, Side * currWidth);
						ptColl.AddPoint(pPtCurr);

					}
					else
						currWidth += straigtLen * splayRate;

					pPtCurr = LocalToPrj(ptStart.pPoint, currDir, straigtLen, Side * currWidth);
					ptColl.AddPoint(pPtCurr);
				}

				if (segment.type == LegType.directToFIX)
				{
					pPtCurr = LocalToPrj(pPtEnd, currDir, straigtLen, Side * currWidth);
					ptColl.AddPoint(pPtCurr);
				}

				currLength = straigtLen;
				toSplay = splayLen - currLength;
			}

			if (bestCase)
				segment.ptEnd.BestCase.Width = currWidth;
			else
				segment.ptEnd.WorstCase.Width = currWidth;
		}

#else
		internal static IPointCollection CalcTouchByFixDir(ref LegData CaseData, PointData CasePoint, IPoint PtFix, double TurnR, double DirNav, int TurnDir, double SnapAngle, out double deadReck)
		{
			const double tan15 = 0.2679491924311227;
			double Sin30 = 0.5;
			double SinSnap = Math.Sin(GlobalVars.DegToRadValue * SnapAngle);
			IPointCollection result = new ESRI.ArcGIS.Geometry.Multipoint();
			//CasePoint.pPoint.M = CasePoint.Direction;
			//int navSide = (int)SideDef(PtFix, DirNav, CasePoint.pPoint);

			CaseData.Turn[0].ptStart = CasePoint.pPoint;
			CaseData.Turn[0].ptStart.M = CasePoint.Direction;

			CaseData.Turn[0].ptCenter = LocalToPrj(CaseData.Turn[0].ptStart, CasePoint.Direction - 90.0 * TurnDir, TurnR);

			double turnAngle = NativeMethods.Modulus((CasePoint.Direction - DirNav) * TurnDir + SnapAngle);
			//turnAngle = Math.Min(turnAngle, NativeMethods.Modulus((CasePoint.Direction - DirNav) * TurnDir - SnapAngle));

			double OutDir1 = CasePoint.Direction - turnAngle * TurnDir;

			CaseData.Turn[0].ptEnd = LocalToPrj(CaseData.Turn[0].ptCenter, OutDir1 + 90.0 * TurnDir, TurnR);
			CaseData.Turn[0].ptEnd.M = OutDir1;

			IPoint ptInters1 = new Point();
			IConstructPoint pConstructor = (IConstructPoint)ptInters1;
			pConstructor.ConstructAngleIntersection(CaseData.Turn[0].ptEnd, GlobalVars.DegToRadValue * OutDir1, PtFix, GlobalVars.DegToRadValue * DirNav);

			//Graphics.DrawPointWithText(ptInters1, "ptInters1-0");
			//Graphics.DrawPointWithText(CaseData.Turn[0].ptCenter, "ptCnt1-0");
			//Graphics.DrawPointWithText(CaseData.Turn[0].ptEnd, "ptOut1-0");
			//IPolyline pPolyline = (IPolyline)new Polyline();
			//pPolyline.FromPoint = PtFix;
			//pPolyline.ToPoint = ptInters1;
			//Graphics.DrawPolyline(pPolyline, -1);
			//ProcessMessages();

			if (SideDef(CaseData.Turn[0].ptEnd, OutDir1 + 90.0, ptInters1) == SideDirection.leftSide)
			{
				double tmpTurnAngle = NativeMethods.Modulus((CasePoint.Direction - DirNav) * TurnDir - SnapAngle);
				if (tmpTurnAngle < GlobalVars.MaxTurnAngle)
				{
					turnAngle = tmpTurnAngle;
					OutDir1 = CasePoint.Direction - turnAngle * TurnDir;

					CaseData.Turn[0].ptEnd = LocalToPrj(CaseData.Turn[0].ptCenter, OutDir1 + 90.0 * TurnDir, TurnR);
					CaseData.Turn[0].ptEnd.M = OutDir1;

					pConstructor = (IConstructPoint)ptInters1;
					pConstructor.ConstructAngleIntersection(CaseData.Turn[0].ptEnd, GlobalVars.DegToRadValue * OutDir1, PtFix, GlobalVars.DegToRadValue * DirNav);
				}
			}

			CaseData.Turn[0].Radius = TurnR;
			CaseData.Turn[0].TurnDir = TurnDir;
			CaseData.Turn[0].Angle = turnAngle;

			CaseData.Turn[0].StartDist = 0.0;
			CaseData.Turn[0].Length = GlobalVars.DegToRadValue * turnAngle * TurnR;

			//Graphics.DrawPointWithText(ptInters1, "ptInters1");
			//Graphics.DrawPointWithText(CaseData.Turn[0].ptCenter, "ptCnt1");
			//Graphics.DrawPointWithText(CaseData.Turn[0].ptEnd, "ptOut1");

			//IPolyline pPolyline = (IPolyline)new Polyline();
			//pPolyline.FromPoint = PtFix;
			//pPolyline.ToPoint = ptInters1;
			//Graphics.DrawPolyline(pPolyline, -1);
			//ProcessMessages();

			double s, sAngle, X, Y;
			double TanSnap30 = Math.Tan(0.5 * GlobalVars.DegToRadValue * (SnapAngle + 30.0));

			sAngle = TurnR * Math.Tan(0.5 * GlobalVars.DegToRadValue * SnapAngle);
			PrjToLocal(CaseData.Turn[0].ptEnd, OutDir1, ptInters1, out X, out Y);

			int nTurns = 2;

			if (X < sAngle)
			{
				sAngle = TurnR * ((TanSnap30 + tan15) * Sin30 / SinSnap - TanSnap30);
				s = Math.Max(sAngle + X, 0.0);
				nTurns = 3;
			}
			else
				s = X - sAngle;

			CaseData.turns = nTurns;
			deadReck = s;

			//II Turn ==========================================================================================================

			int TurnDir2 = SideDirection.leftSide;
			double turnAngle2 = NativeMethods.Modulus(OutDir1 - DirNav);
			if (turnAngle2 > 180.0)
			{
				turnAngle2 = 360.0 - turnAngle2;
				TurnDir2 = SideDirection.rightSide;
			}

			double OutDir2;		// = OutDir1 + turnAngle2  * TurnDir2;
			if (nTurns == 2)
				OutDir2 = DirNav;
			else
			{
				turnAngle2 = SnapAngle + 30.0;
				OutDir2 = DirNav - 30.0 * TurnDir2;
			}

			CaseData.Turn[1].Radius = TurnR;
			CaseData.Turn[1].TurnDir = TurnDir2;
			CaseData.Turn[1].Angle = turnAngle2;

			CaseData.Turn[1].StartDist = s;
			CaseData.Turn[1].Length = GlobalVars.DegToRadValue * turnAngle2 * TurnR;

			CaseData.Turn[1].ptStart = LocalToPrj(CaseData.Turn[0].ptEnd, OutDir1, s);
			CaseData.Turn[1].ptStart.M = OutDir1;

			CaseData.Turn[1].ptCenter = LocalToPrj(CaseData.Turn[1].ptStart, OutDir1 - 90.0 * TurnDir2, TurnR);

			CaseData.Turn[1].ptEnd = LocalToPrj(CaseData.Turn[1].ptCenter, OutDir2 + 90.0 * TurnDir2, TurnR);
			CaseData.Turn[1].ptEnd.M = OutDir2;

			//Graphics.DrawPointWithText(CaseData.Turn[1].ptCenter, "ptCnt2");
			//Graphics.DrawPointWithText(CaseData.Turn[1].ptStart, "ptOut2");
			//Graphics.DrawPointWithText(CaseData.Turn[1].ptEnd, "ptOut3");
			//ProcessMessages();

			result.AddPoint(CaseData.Turn[0].ptStart);
			result.AddPoint(CaseData.Turn[0].ptEnd);
			if (s > GlobalVars.distEps)
				result.AddPoint(CaseData.Turn[1].ptStart);
			result.AddPoint(CaseData.Turn[1].ptEnd);

			//III Turn ==========================================================================================================
			if (nTurns == 3)
			{
				IPoint ptInters2 = new Point();
				pConstructor = (IConstructPoint)ptInters2;
				pConstructor.ConstructAngleIntersection(CaseData.Turn[1].ptEnd, GlobalVars.DegToRadValue * OutDir2, PtFix, GlobalVars.DegToRadValue * DirNav);

				//Graphics.DrawPointWithText(ptInters2, "ptInters2");
				//ProcessMessages();

				sAngle = TurnR * tan15;
				PrjToLocal(CaseData.Turn[1].ptEnd, OutDir2, ptInters2, out X, out Y);
				s = X - sAngle;
				double OutDir3 = DirNav;
				//double turnAngle3 = NativeMethods.Modulus(DirNav - OutDir2);

				CaseData.Turn[2].Radius = TurnR;
				CaseData.Turn[2].TurnDir = -TurnDir2;
				CaseData.Turn[2].Angle = 30.0;

				CaseData.Turn[2].StartDist = s;
				CaseData.Turn[2].Length = 30.0 * GlobalVars.DegToRadValue * TurnR;

				CaseData.Turn[2].ptStart = LocalToPrj(CaseData.Turn[1].ptEnd, OutDir2, s);
				CaseData.Turn[2].ptStart.M = OutDir2;

				CaseData.Turn[2].ptCenter = LocalToPrj(CaseData.Turn[2].ptStart, OutDir2 + 90.0 * TurnDir2, TurnR);

				CaseData.Turn[2].ptEnd = LocalToPrj(CaseData.Turn[2].ptCenter, OutDir3 - 90.0 * TurnDir2, TurnR);
				CaseData.Turn[2].ptEnd.M = OutDir3;

				//Graphics.DrawPointWithText(CaseData.Turn[2].ptStart, "ptOut4");
				//Graphics.DrawPointWithText(CaseData.Turn[2].ptCenter, "ptCnt3");
				//Graphics.DrawPointWithText(CaseData.Turn[2].ptEnd, "ptOut5");
				//ProcessMessages();

				result.AddPoint(CaseData.Turn[2].ptStart);
				result.AddPoint(CaseData.Turn[2].ptEnd);

				deadReck += s;
			}

			return result;
		}

		internal static void CreateTurnAndInterceptSideLine(LegData data, PointData StartFIX, PointData EndFIX, double PlannedEndWidth, ref IPointCollection ptColl, out double currWidth, int Side)
		{
			//CreateTurnAndInterceptSideLine
			#region  Head

			double splayRate = GlobalVars.SplayRate, currLength = 0.0,
				currDir = StartFIX.Direction,
				endWidth = PlannedEndWidth;

			currWidth = StartFIX.Width;

			//Graphics.DrawPolyline(segment.BestCase.pNominalPoly, -1);
			//ProcessMessages();

			double splayLen = (endWidth - currWidth) / splayRate;
			double toSplay = splayLen;

			if (currWidth >= endWidth)
				splayRate = 0.0;

			IPoint pPtRefer = StartFIX.pPoint;
			IPoint pPtCurr = LocalToPrj(pPtRefer, currDir, 0.0, Side * currWidth);

			ptColl.AddPoint(pPtCurr);

			//Graphics.DrawPointWithText(pPtCurr,"c0");
			//ProcessMessages();
			#endregion

			#region  Turns

			double straigtLen;

			for (int iTurn = 0; iTurn < data.turns; iTurn++)
			{
				straigtLen = ReturnDistanceInMeters(pPtRefer, data.Turn[iTurn].ptStart);


				if (straigtLen > GlobalVars.distEps && currWidth < endWidth)	//???????????????????
				{
					if (toSplay < straigtLen)
					{
						currWidth = endWidth;
						splayRate = 0.0;
						if (toSplay > GlobalVars.distEps)
						{
							pPtCurr = LocalToPrj(pPtRefer, currDir, toSplay, Side * currWidth);
							ptColl.AddPoint(pPtCurr);
						}
					}
					else
						currWidth += straigtLen * splayRate;
				}

				//if (straigtLen > 0.0)
				//{
				//pPtCurr = LocalToPrj(pPtRefer, currDir, straigtLen, Side * currWidth);
				//	ptColl.AddPoint(pPtCurr);
				//Graphics.DrawPointWithText(pPtCurr,"c1");
				//Graphics.DrawPointWithText(pPtRefer, "pPtRefer");
				//ProcessMessages();
				//}

				currLength += straigtLen;
				toSplay -= currLength;

				double turnAngle = data.Turn[iTurn].Angle;
				double arcLen = data.Turn[iTurn].Radius * DegToRad(turnAngle);

				if (arcLen > GlobalVars.distEps)
				{
					if (currWidth < endWidth)
					{
						if (toSplay < arcLen)
						{
							turnAngle = RadToDeg(toSplay / data.Turn[iTurn].Radius);
							arcLen = GlobalVars.DegToRadValue * turnAngle * data.Turn[iTurn].Radius;
						}

						int n = (int)turnAngle;

						if (n < 1) n = 1;
						else if (n < 5) n = 5;
						else if (n < 10) n = 10;

						double angStep = turnAngle / n;
						IPoint pPtCenter = data.Turn[iTurn].ptCenter;

						pPtCurr = new Point();

						for (int i = 0; i < n; i++)
						{
							double angleInRad = GlobalVars.DegToRadValue * (currDir + (90.0 - i * angStep) * data.Turn[iTurn].TurnDir);
							double currRadius = data.Turn[iTurn].Radius + currWidth * Side * data.Turn[iTurn].TurnDir;

							pPtCurr.X = pPtCenter.X + currRadius * System.Math.Cos(angleInRad);
							pPtCurr.Y = pPtCenter.Y + currRadius * System.Math.Sin(angleInRad);
							ptColl.AddPoint(pPtCurr);

							currWidth += data.Turn[iTurn].Radius * GlobalVars.DegToRadValue * angStep * splayRate;
						}

						toSplay -= arcLen;
						currLength += arcLen;
						currDir -= turnAngle * data.Turn[iTurn].TurnDir;

						turnAngle = data.Turn[iTurn].Angle - turnAngle;
						arcLen = data.Turn[iTurn].Radius * DegToRad(turnAngle);
					}

					if (arcLen > 0.0)
					{
						int n = (int)turnAngle;

						if (n < 1) n = 1;
						else if (n < 5) n = 5;
						else if (n < 10) n = 10;

						double angStep = turnAngle / n;

						IPoint pPtCenter = data.Turn[iTurn].ptCenter;

						//Graphics.DrawPointWithText(data.Turn[iTurn].ptStart, "Start-" + iTurn.ToString());
						//Graphics.DrawPointWithText(data.Turn[iTurn].ptEnd, "End-" + iTurn.ToString());
						//Graphics.DrawPointWithText(pPtCenter, "Center-" + iTurn.ToString());
						//ProcessMessages();

						pPtCurr = new Point();

						for (int i = 0; i < n; i++)
						{
							double angleInRad = GlobalVars.DegToRadValue * (currDir + (90.0 - i * angStep) * data.Turn[iTurn].TurnDir);
							double currRadius = data.Turn[iTurn].Radius + currWidth * Side * data.Turn[iTurn].TurnDir;

							pPtCurr.X = pPtCenter.X + currRadius * System.Math.Cos(angleInRad);
							pPtCurr.Y = pPtCenter.Y + currRadius * System.Math.Sin(angleInRad);

							//Graphics.DrawPointWithText(pPtCurr, "Curr");
							//ProcessMessages();

							ptColl.AddPoint(pPtCurr);
						}

						currDir -= turnAngle * data.Turn[iTurn].TurnDir;
						currLength += arcLen;
					}
				}

				pPtRefer = data.Turn[iTurn].ptEnd;
				pPtCurr = LocalToPrj(pPtRefer, currDir, 0.0, Side * currWidth);
				ptColl.AddPoint(pPtCurr);

				//Graphics.DrawPolyline(ptColl, RGB(0, 0, 255), 2);
				//Graphics.DrawPolyline(ptColl, 0, 1);
				//ProcessMessages();

				//toSplay = splayLen - currLength;
			}
			#endregion

			#region Trail

			straigtLen = ReturnDistanceInMeters(pPtRefer, EndFIX.pPoint);

			if (straigtLen > GlobalVars.distEps)	// && currWidth < endWidth
			{
				if (toSplay < straigtLen)
				{
					currWidth = endWidth;
					splayRate = 0.0;

					if (toSplay > GlobalVars.distEps)
					{
						if (toSplay > 0.5 * straigtLen)
							pPtCurr = LocalToPrj(EndFIX.pPoint, currDir, straigtLen - toSplay, Side * currWidth);
						else
							pPtCurr = LocalToPrj(pPtRefer, currDir, toSplay, Side * currWidth);

						ptColl.AddPoint(pPtCurr);
					}
				}
				else
					currWidth += straigtLen * splayRate;

				pPtCurr = LocalToPrj(EndFIX.pPoint, currDir, 0.0, Side * currWidth);
				ptColl.AddPoint(pPtCurr);
			}

			#endregion
		}

		internal static void CreateSideLine(ref TrackLeg segment, ref IPointCollection ptColl, bool bestCase, int Side)
		{
			PointData StartFIX, EndFIX;
			LegData data;

			if (bestCase)
			{
				data = segment.BestCase;
				StartFIX = segment.ptStart.BestCase;
				EndFIX = segment.ptEnd.BestCase;
			}
			else
			{
				data = segment.WorstCase;
				StartFIX = segment.ptStart.WorstCase;
				EndFIX = segment.ptEnd.WorstCase;
			}

			#region  Head

			double splayRate = GlobalVars.SplayRate, currLength = 0.0,
				currWidth = StartFIX.Width, currDir = StartFIX.Direction,
				endWidth = segment.PlannedEndWidth;

			if (segment.SegmentCode == eLegType.arcPath)
			{
				if (currWidth < GlobalVars.ArcProtectWidth)
					currWidth = GlobalVars.ArcProtectWidth;

				if (endWidth < GlobalVars.ArcProtectWidth)
					endWidth = GlobalVars.ArcProtectWidth;
			}

			//Graphics.DrawPolyline(segment.BestCase.pNominalPoly, -1);
			//ProcessMessages();

			double splayLen = (endWidth - currWidth) / splayRate;
			double toSplay = splayLen;

			if (currWidth >= endWidth)
				splayRate = 0.0;

			IPoint pPtRefer = StartFIX.pPoint;
			IPoint pPtCurr = LocalToPrj(pPtRefer, currDir, 0.0, Side * currWidth);

			ptColl.AddPoint(pPtCurr);

			//Graphics.DrawPointWithText(pPtCurr,"c0");
			//ProcessMessages();

			#endregion

			#region  Turns

			double straigtLen;

			for (int iTurn = 0; iTurn < data.turns; iTurn++)
			{
				straigtLen = ReturnDistanceInMeters(pPtRefer, data.Turn[iTurn].ptStart);

				if (straigtLen > GlobalVars.distEps && currWidth < endWidth)	//???????????????????
				{
					if (toSplay < straigtLen)
					{
						currWidth = endWidth;
						splayRate = 0.0;
						if (toSplay > GlobalVars.distEps)
						{
							pPtCurr = LocalToPrj(pPtRefer, currDir, toSplay, Side * currWidth);
							ptColl.AddPoint(pPtCurr);
						}
					}
					else
						currWidth += straigtLen * splayRate;
				}

				//if (straigtLen > 0.0)
				//{
				//pPtCurr = LocalToPrj(pPtRefer, currDir, straigtLen, Side * currWidth);
				//	ptColl.AddPoint(pPtCurr);
				//Graphics.DrawPointWithText(pPtCurr,"c1");
				//Graphics.DrawPointWithText(pPtRefer, "pPtRefer");
				//ProcessMessages();
				//}

				currLength += straigtLen;
				toSplay -= currLength;

				double turnAngle = data.Turn[iTurn].Angle;
				double arcLen = data.Turn[iTurn].Radius * DegToRad(turnAngle);

				if (arcLen > GlobalVars.distEps)
				{
					if (currWidth < endWidth)
					{
						if (toSplay < arcLen)
						{
							turnAngle = RadToDeg(toSplay / data.Turn[iTurn].Radius);
							arcLen = GlobalVars.DegToRadValue * turnAngle * data.Turn[iTurn].Radius;
						}

						int n = (int)turnAngle;

						if (n < 1) n = 1;
						else if (n < 5) n = 5;
						else if (n < 10) n = 10;

						double angStep = turnAngle / n;
						IPoint pPtCenter = data.Turn[iTurn].ptCenter;

						pPtCurr = new Point();

						for (int i = 0; i < n; i++)
						{
							double angleInRad = GlobalVars.DegToRadValue * (currDir + (90.0 - i * angStep) * data.Turn[iTurn].TurnDir);
							double currRadius = data.Turn[iTurn].Radius + currWidth * Side * data.Turn[iTurn].TurnDir;

							pPtCurr.X = pPtCenter.X + currRadius * System.Math.Cos(angleInRad);
							pPtCurr.Y = pPtCenter.Y + currRadius * System.Math.Sin(angleInRad);
							ptColl.AddPoint(pPtCurr);

							currWidth += data.Turn[iTurn].Radius * GlobalVars.DegToRadValue * angStep * splayRate;
						}

						toSplay -= arcLen;
						currLength += arcLen;
						currDir -= turnAngle * data.Turn[iTurn].TurnDir;

						turnAngle = data.Turn[iTurn].Angle - turnAngle;
						arcLen = data.Turn[iTurn].Radius * DegToRad(turnAngle);
					}

					if (arcLen > 0.0)
					{
						int n = (int)turnAngle;

						if (n < 1) n = 1;
						else if (n < 5) n = 5;
						else if (n < 10) n = 10;

						double angStep = turnAngle / n;

						IPoint pPtCenter = data.Turn[iTurn].ptCenter;

						//Graphics.DrawPointWithText(data.Turn[iTurn].ptStart, "Start-" + iTurn.ToString());
						//Graphics.DrawPointWithText(data.Turn[iTurn].ptEnd, "End-" + iTurn.ToString());
						//Graphics.DrawPointWithText(pPtCenter, "Center-" + iTurn.ToString());
						//ProcessMessages();

						pPtCurr = new Point();

						for (int i = 0; i < n; i++)
						{
							double angleInRad = GlobalVars.DegToRadValue * (currDir + (90.0 - i * angStep) * data.Turn[iTurn].TurnDir);
							double currRadius = data.Turn[iTurn].Radius + currWidth * Side * data.Turn[iTurn].TurnDir;

							pPtCurr.X = pPtCenter.X + currRadius * System.Math.Cos(angleInRad);
							pPtCurr.Y = pPtCenter.Y + currRadius * System.Math.Sin(angleInRad);

							//Graphics.DrawPointWithText(pPtCurr, "Curr");
							//ProcessMessages();

							ptColl.AddPoint(pPtCurr);
						}

						currDir -= turnAngle * data.Turn[iTurn].TurnDir;
						currLength += arcLen;
					}
				}

				pPtRefer = data.Turn[iTurn].ptEnd;
				pPtCurr = LocalToPrj(pPtRefer, currDir, 0.0, Side * currWidth);
				ptColl.AddPoint(pPtCurr);

				//Graphics.DrawPolyline(ptColl, RGB(0, 0, 255), 2);
				//Graphics.DrawPolyline(ptColl, 0, 1);
				//ProcessMessages();
			}
			#endregion

			#region Trail

			straigtLen = ReturnDistanceInMeters(pPtRefer, EndFIX.pPoint);

			if (straigtLen > GlobalVars.distEps)	// && currWidth < endWidth)
			{
				if (toSplay < straigtLen)
				{
					currWidth = endWidth;
					splayRate = 0.0;
					if (toSplay > GlobalVars.distEps)
					{
						if (toSplay > 0.5 * straigtLen)
							pPtCurr = LocalToPrj(EndFIX.pPoint, currDir, straigtLen - toSplay, Side * currWidth);
						else
							pPtCurr = LocalToPrj(pPtRefer, currDir, toSplay, Side * currWidth);
						//pPtCurr = LocalToPrj(pPtRefer, currDir, splayLen, Side * currWidth);

						ptColl.AddPoint(pPtCurr);
					}
				}
				else
					currWidth += straigtLen * splayRate;

				//pPtCurr = LocalToPrj(pPtRefer, currDir, straigtLen, Side * currWidth);
				pPtCurr = LocalToPrj(EndFIX.pPoint, currDir, 0.0, Side * currWidth);
				ptColl.AddPoint(pPtCurr);
			}
			else if (segment.SegmentCode == eLegType.directToFIX)			//????????????????????
			{
				pPtCurr = LocalToPrj(EndFIX.pPoint, currDir, 0.0, Side * currWidth);
				//pPtCurr = LocalToPrj(pPtRefer, currDir, straigtLen, Side * currWidth);
				ptColl.AddPoint(pPtCurr);
			}

			#endregion

			if (bestCase)
				segment.ptEnd.BestCase.Width = currWidth;
			else
				segment.ptEnd.WorstCase.Width = currWidth;
		}

#endif

		internal static void CreateProtectionArea(ref TrackLeg segment)
		{
			IPointCollection ptCollBestLeft = (IPointCollection)new Polyline();
			IPointCollection ptCollBestRight = (IPointCollection)new Polyline();
			IPointCollection ptCollWorstLeft = (IPointCollection)new Polyline();
			IPointCollection ptCollWorstRight = (IPointCollection)new Polyline();

			CreateSideLine(ref segment, ref ptCollBestLeft, true, SideDirection.rightSide);
			//Graphics.DrawPolyline(ptCollBestLeft, RGB(255, 0, 155), 2);
			//ProcessMessages();

			CreateSideLine(ref segment, ref ptCollBestRight, true, SideDirection.leftSide);
			//Graphics.DrawPolyline(ptCollBestRight, RGB(255, 0, 255), 2);
			//ProcessMessages();

			CreateSideLine(ref segment, ref ptCollWorstLeft, false, SideDirection.rightSide);
			//Graphics.DrawPolyline(ptCollWorstLeft, RGB(0, 255, 155), 2);
			//ProcessMessages();

			CreateSideLine(ref segment, ref ptCollWorstRight, false, SideDirection.leftSide);
			//Graphics.DrawPolyline(ptCollWorstRight, RGB(0, 255, 255), 2);
			//ProcessMessages();

			IPointCollection ptColl;
			ITopologicalOperator2 pTopo;

			IPolygon[] src = new IPolygon[6];


			//1=========================================================
			((IPolyline)ptCollBestRight).ReverseOrientation();

			IPolygon pTmpPoly = (IPolygon)new Polygon();
			ptColl = (IPointCollection)pTmpPoly;
			ptColl.AddPointCollection(ptCollWorstRight);
			ptColl.AddPointCollection(ptCollBestRight);
			pTmpPoly.Close();
			pTopo = (ITopologicalOperator2)pTmpPoly;
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();
			src[0] = pTmpPoly;

			//Graphics.DrawPolygon(pTmpPoly, -1, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal);
			//Functions.ProcessMessages();
			//2=========================================================
			pTmpPoly = (IPolygon)new Polygon();
			ptColl = (IPointCollection)pTmpPoly;
			ptColl.AddPointCollection(ptCollBestLeft);
			ptColl.AddPointCollection(ptCollBestRight);
			pTmpPoly.Close();
			pTopo = (ITopologicalOperator2)pTmpPoly;
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();
			src[1] = pTmpPoly;

			//Graphics.DrawPolygon(pTmpPoly, -1, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal);
			//Functions.ProcessMessages();
			//3=========================================================
			pTmpPoly = (IPolygon)new Polygon();
			ptColl = (IPointCollection)pTmpPoly;
			ptColl.AddPointCollection(ptCollWorstLeft);
			ptColl.AddPointCollection(ptCollBestRight);
			pTmpPoly.Close();
			pTopo = (ITopologicalOperator2)pTmpPoly;
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();

			src[2] = pTmpPoly;

			//Graphics.DrawPolygon(pTmpPoly, -1, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSHorizontal);
			//Functions.ProcessMessages();
			//4=========================================================
			((IPolyline)ptCollWorstRight).ReverseOrientation();

			pTmpPoly = (IPolygon)new Polygon();
			ptColl = (IPointCollection)pTmpPoly;
			ptColl.AddPointCollection(ptCollBestLeft);
			ptColl.AddPointCollection(ptCollWorstRight);
			pTmpPoly.Close();
			pTopo = (ITopologicalOperator2)pTmpPoly;
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();
			src[3] = pTmpPoly;

			//Graphics.DrawPolygon(pTmpPoly, -1, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSVertical);
			//Functions.ProcessMessages();
			//5=========================================================
			pTmpPoly = (IPolygon)new Polygon();
			ptColl = (IPointCollection)pTmpPoly;
			ptColl.AddPointCollection(ptCollWorstLeft);
			ptColl.AddPointCollection(ptCollWorstRight);
			pTmpPoly.Close();
			pTopo = (ITopologicalOperator2)pTmpPoly;
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();
			src[4] = pTmpPoly;

			//Graphics.DrawPolygon(pTmpPoly, -1, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSCross);
			//Functions.ProcessMessages();
			//6=========================================================
			((IPolyline)ptCollWorstLeft).ReverseOrientation();

			pTmpPoly = (IPolygon)new Polygon();
			ptColl = (IPointCollection)pTmpPoly;
			ptColl.AddPointCollection(ptCollBestLeft);
			ptColl.AddPointCollection(ptCollWorstLeft);
			pTmpPoly.Close();
			pTopo = (ITopologicalOperator2)pTmpPoly;
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();
			src[5] = pTmpPoly;

			//Graphics.DrawPolygon(pTmpPoly, -1, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross);
			//Functions.ProcessMessages();

			//Graphics.DrawPolygon(src[0], -1, (ESRI.ArcGIS.Display.esriSimpleFillStyle)1);
			//Functions.ProcessMessages();

			int i = 0;
			IGeometryCollection pGeoms = (IGeometryCollection)src[0];
			int n = pGeoms.GeometryCount;
			while (i < pGeoms.GeometryCount)
			{
				IArea pArea = (IArea)pGeoms.Geometry[i];
				if (pArea.Area < 15.0)
					pGeoms.RemoveGeometries(i, 1);
				else
					i++;
			}

			if (n != pGeoms.GeometryCount)
			{
				pTopo = (ITopologicalOperator2)src[0];
				pTopo.IsKnownSimple_2 = false;
				pTopo.Simplify();
			}

			i = 0;
			pGeoms = (IGeometryCollection)src[5];
			n = pGeoms.GeometryCount;

			while (i < pGeoms.GeometryCount)
			{
				IArea pArea = (IArea)pGeoms.Geometry[i];
				if (pArea.Area < 15.0)
					pGeoms.RemoveGeometries(i, 1);
				else
					i++;
			}

			if (n != pGeoms.GeometryCount)
			{
				pTopo = (ITopologicalOperator2)src[5];
				pTopo.IsKnownSimple_2 = false;
				pTopo.Simplify();
			}

			IPolygon pProtectionArea = null;
			pTopo = (ITopologicalOperator2)src[0];

			for (i = 1; i < 6; i++)
			{
				//Graphics.DrawPolygon(pProtectionArea, -1, (ESRI.ArcGIS.Display.esriSimpleFillStyle)(i ));
				//Graphics.DrawPolygon(src[i], -1, (ESRI.ArcGIS.Display.esriSimpleFillStyle)(i + 1));
				//Functions.ProcessMessages();
				//pArea = (IArea)src[i];
				pProtectionArea = (IPolygon)pTopo.Union(src[i]);
				pTopo = (ITopologicalOperator2)pProtectionArea;
				pTopo.IsKnownSimple_2 = false;
				pTopo.Simplify();
			}

			n = Math.Min(segment.BestCase.turns, segment.WorstCase.turns);

			for (i = 0; i < n; i++)
			{
				double dir, dist, r1, r2, turnAngle;
				double touchAngle12, touchAngle21;//, centrDir12, centrDir21;
				double entryDir1, entryDir2, turnAngle1, turnAngle2;
				IPoint ptCenter1, ptCenter2;

				int turnDir;	//, turnDir2;

				ptCenter1 = segment.BestCase.Turn[i].ptCenter;
				ptCenter2 = segment.WorstCase.Turn[i].ptCenter;

				turnAngle = segment.BestCase.Turn[i].Angle;

				dir = ReturnAngleInDegrees(ptCenter1, ptCenter2);
				dist = ReturnDistanceInMeters(ptCenter1, ptCenter2);
				r1 = segment.BestCase.Turn[i].Radius;
				r2 = segment.WorstCase.Turn[i].Radius;

				if (i == 0)
				{
					turnDir = segment.BestCase.Turn[i].TurnDir;

					entryDir1 = segment.ptStart.BestCase.Direction;
					entryDir2 = segment.ptStart.WorstCase.Direction;
				}
				else
				{
					turnDir = segment.WorstCase.Turn[i].TurnDir;
					entryDir1 = segment.BestCase.Turn[i].ptStart.M;
					//segment.BestCase.DRDirection;
					entryDir2 = segment.WorstCase.Turn[i].ptStart.M;
					//segment.WorstCase.DRDirection;
				}

				touchAngle12 = dir - RadToDeg(Math.Atan((r1 - r2) / dist)) * turnDir;
				touchAngle21 = dir - RadToDeg(Math.Atan((r2 - r1) / dist)) * turnDir + 180.0;

				//centrDir12 = touchAngle12 - 90.0 * turnDir;
				//centrDir21 = touchAngle21 - 90.0 * turnDir;

				turnAngle1 = NativeMethods.Modulus((entryDir1 - touchAngle12) * turnDir);
				turnAngle2 = NativeMethods.Modulus((entryDir1 - touchAngle21) * turnDir);

				IPoint ptTmp1, ptTmp2, ptTmp3, ptTmp4;
				IPolyline line1 = (IPolyline)new Polyline(), line2 = (IPolyline)new Polyline();
				IPointCollection ptcol1, ptcol2;

				if (turnAngle1 < turnAngle)
				{
					pTmpPoly = (IPolygon)new Polygon();
					ptColl = (IPointCollection)pTmpPoly;

					ptTmp1 = LocalToPrj(ptCenter1, touchAngle12, 0.0, r1 * turnDir);

					//Graphics.DrawPointWithText(ptTmp1, "ptTmp1-1");
					//Graphics.DrawPointWithText(ptCenter1, "ptCenter1-1");
					//ProcessMessages();

					line1.FromPoint = ptTmp1;
					line1.ToPoint = LocalToPrj(ptCenter1, touchAngle12, 0.0, 100000000.0 * turnDir);
					ptcol1 = (IPointCollection)pTopo.Intersect(line1, esriGeometryDimension.esriGeometry1Dimension);

					//Graphics.DrawPolyline(line1, 0, 2);
					//Graphics.DrawPolygon(pProtectionArea, -1, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross);
					//ProcessMessages();

					if (ptcol1.PointCount > 1)
					{
						if (ReturnDistanceInMeters(ptTmp1, ptcol1.Point[0]) > GlobalVars.distEps)
							ptTmp4 = ptcol1.Point[0];
						else
							ptTmp4 = ptcol1.Point[1];
					}
					else
						ptTmp4 = ptTmp1;

					ptTmp2 = LocalToPrj(ptCenter2, touchAngle12, 0.0, r2 * turnDir);
					//Graphics.DrawPointWithText(ptTmp2, "ptTmp2");
					//ProcessMessages();

					line2.FromPoint = ptTmp2;
					line2.ToPoint = LocalToPrj(ptCenter2, touchAngle12, 0.0, 100000000.0 * turnDir);
					ptcol2 = (IPointCollection)pTopo.Intersect(line2, esriGeometryDimension.esriGeometry1Dimension);

					//Graphics.DrawPolyline(line2, 255, 2);
					//Graphics.DrawPolygon(pProtectionArea, -1, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross);
					//ProcessMessages();
					if (ptcol2.PointCount > 1)
					{
						if (ReturnDistanceInMeters(ptTmp2, ptcol2.Point[0]) > GlobalVars.distEps)
							ptTmp3 = ptcol2.Point[0];
						else
							ptTmp3 = ptcol2.Point[1];
					}
					else
						ptTmp3 = ptTmp2;

					//Graphics.DrawPointWithText(ptTmp3, "ptTmp3");
					//ProcessMessages();

					ptColl.AddPoint(ptTmp1);
					ptColl.AddPoint(ptTmp2);
					ptColl.AddPoint(ptTmp3);
					ptColl.AddPoint(ptTmp4);

					pTmpPoly.Close();
					pTopo = (ITopologicalOperator2)pTmpPoly;
					pTopo.IsKnownSimple_2 = false;
					pTopo.Simplify();

					//Graphics.DrawPolygon(pTmpPoly, -1, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSCross);
					//ProcessMessages();

					pProtectionArea = (IPolygon)pTopo.Union(pProtectionArea);
					pTopo = (ITopologicalOperator2)pProtectionArea;
					pTopo.IsKnownSimple_2 = false;
					pTopo.Simplify();
				}

				if (turnAngle2 < turnAngle)
				{
					pTmpPoly = (IPolygon)new Polygon();
					ptColl = (IPointCollection)pTmpPoly;

					ptTmp1 = LocalToPrj(ptCenter1, touchAngle21, 0.0, r1 * turnDir);
					//ptTmp1 = LocalToPrj(ptCenter1, touchAngle21 + 90.0 * turnDir, r1);

					//Graphics.DrawPointWithText(ptTmp1, "ptTmp1");
					//Graphics.DrawPointWithText(ptCenter1, "ptCenter1");
					//Graphics.DrawPolygon(pProtectionArea, -1, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross);
					//ProcessMessages();

					line1.FromPoint = ptTmp1;
					line1.ToPoint = LocalToPrj(ptCenter1, touchAngle21, 0.0, 100000000.0 * turnDir);
					//line1.ToPoint = LocalToPrj(ptCenter1, touchAngle21 + 90 * turnDir, 100000000.0);

					ptcol1 = (IPointCollection)pTopo.Intersect(line1, esriGeometryDimension.esriGeometry1Dimension);
					if (ReturnDistanceInMeters(ptTmp1, ptcol1.Point[0]) > GlobalVars.distEps)
						ptTmp4 = ptcol1.Point[0];
					else
						ptTmp4 = ptcol1.Point[1];

					//Graphics.DrawPointWithText(ptTmp4, "ptTmp4");

					ptTmp2 = LocalToPrj(ptCenter2, touchAngle21, 0.0, r2 * segment.WorstCase.Turn[0].TurnDir);
					//ptTmp2 = LocalToPrj(ptCenter2, touchAngle21 + 90.0 * segment.WorstCase.Turn1.TurnDir, r2);
					//Graphics.DrawPointWithText(ptTmp2, "ptTmp2");

					line2.FromPoint = ptTmp2;
					line2.ToPoint = LocalToPrj(ptCenter2, touchAngle21, 0.0, 100000000.0 * segment.WorstCase.Turn[0].TurnDir);
					//line2.ToPoint = LocalToPrj(ptCenter2, touchAngle21 + 90.0 * segment.WorstCase.Turn1.TurnDir, 100000000.0);
					ptcol2 = (IPointCollection)pTopo.Intersect(line2, esriGeometryDimension.esriGeometry1Dimension);

					//Graphics.DrawPolyline(line2, 255, 2);
					//Graphics.DrawPolygon(pProtectionArea, -1, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross);
					//ProcessMessages();
					if (ptcol2.PointCount > 1)
					{
						if (ReturnDistanceInMeters(ptTmp2, ptcol2.Point[0]) > GlobalVars.distEps)
							ptTmp3 = ptcol2.Point[0];
						else
							ptTmp3 = ptcol2.Point[1];

						//Graphics.DrawPointWithText(ptTmp3, "ptTmp3");
						//ProcessMessages();
						//ptTmp3 = line2.ToPoint;

						ptColl.AddPoint(ptTmp1);
						ptColl.AddPoint(ptTmp2);
						ptColl.AddPoint(ptTmp3);
						ptColl.AddPoint(ptTmp4);
					}
					else
					{
						ptColl.AddPoint(ptTmp1);
						ptColl.AddPoint(ptTmp2);
						ptColl.AddPoint(ptTmp4);
					}

					pTmpPoly.Close();
					pTopo = (ITopologicalOperator2)pTmpPoly;
					pTopo.IsKnownSimple_2 = false;
					pTopo.Simplify();

					//Graphics.DrawPolygon(pTmpPoly, -1, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSCross);
					//ProcessMessages();

					pProtectionArea = (IPolygon)pTopo.Union(pProtectionArea);
					pTopo = (ITopologicalOperator2)pProtectionArea;
					pTopo.IsKnownSimple_2 = false;
					pTopo.Simplify();
				}

				//Graphics.DrawPointWithText(ptTmp1, "I-1-2");
				//Graphics.DrawPointWithText(ptTmp2, "II-1-2");

				//Graphics.DrawPointWithText(ptTmp1, "I-2-1");
				//Graphics.DrawPointWithText(ptTmp2, "II-2-1");
				//Graphics.DrawPointWithText(segment.BestCase.Turn1.ptCenter, "ptCenter-1");
				//Graphics.DrawPointWithText(segment.WorstCase.Turn1.ptCenter, "ptCenter-2");
				//Functions.ProcessMessages();
			}

			//Graphics.DrawPolygon(pProtectionArea, -1, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross);
			//ProcessMessages();

			IGeometryCollection pGeoColl = (IGeometryCollection)pProtectionArea;
			int k = 0;
			while (k < pGeoColl.GeometryCount)
			{
				IRing pRing = (IRing)pGeoColl.Geometry[k];
				if (!pRing.IsExterior)
					pGeoColl.RemoveGeometries(k, 1);
				else
					k++;
			}

			pTopo = (ITopologicalOperator2)pProtectionArea;
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();

			//=========================================================
			segment.pProtectionArea = pProtectionArea;
		}
	}
}
