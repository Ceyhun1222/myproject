using System;
using System.Linq;
using NetTopologySuite.Geometries;
using CDOTMA.Geometries;

namespace CDOTMA
{
	public static class Functions
	{

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

		public static double DegToRad(double val)
		{
			return val * GlobalVars.DegToRadValue;
		}

		public static double RadToDeg(double val)
		{
			return val * GlobalVars.RadToDegValue;
		}

		public static void TextBoxFloat(ref char KeyChar, string BoxText)
		{
			if (KeyChar < 32)
				return;

			char DecSep = (1.1).ToString().ToCharArray()[1];

			if ((KeyChar < '0' || KeyChar > '9') && KeyChar != DecSep && KeyChar != '-')
				KeyChar = '\0';
			else if (KeyChar == DecSep && BoxText.Contains(DecSep))
				KeyChar = '\0';
		}

		public static Geometry ToGeo(Geometry prjGeometry)
		{
			if (prjGeometry == null)
				return null;

			if (prjGeometry.GetSpatialReference() == null)
				prjGeometry.SetSpatialReference(GlobalVars.pSpRefPrj);

			Geometry result = (Geometry)prjGeometry.CloneGeom();
			result.Project(GlobalVars.pSpRefGeo);

			return result;
		}

		public static Geometry ToPrj(Geometry geoGeometry)
		{
			if (geoGeometry == null)
				return null;

			if (geoGeometry.GetSpatialReference() == null)
				geoGeometry.SetSpatialReference(GlobalVars.pSpRefGeo);

			Geometry result = (Geometry)geoGeometry.CloneGeom();

			result.Project(GlobalVars.pSpRefPrj);
			return result;
		}

		public static double Azt2Dir(Point ptGeo, decimal Azt)
		{
			return Azt2Dir(ptGeo, (double)Azt);
		}

		public static double Azt2Dir(Point ptGeo, double Azt)
		{
			double ResX, ResY;
			NativeMethods.PointAlongGeodesic(ptGeo.X, ptGeo.Y, 10.0, Azt, out ResX, out ResY);

			Point Pt10 = (Point)Functions.ToPrj(ptGeo);
			Point Pt11 = (Point)Functions.ToPrj(new Point(ResX, ResY));

			return ReturnAngleInDegrees(Pt10, Pt11);
		}

		public static double Azt2DirPrj(Point ptPrj, double Azt)
		{
			Point Pt10 = (Point)ToGeo(ptPrj);

			double ResX, ResY;
			NativeMethods.PointAlongGeodesic(Pt10.X, Pt10.Y, 10.0, Azt, out ResX, out ResY);

			Point Pt11 = (Point)Functions.ToPrj(new Point(ResX, ResY));
			return ReturnAngleInDegrees(ptPrj, Pt11);
		}

		public static double Dir2Azt(Point pPtPrj, double Dir_Renamed)
		{
			double resD;
			double resI;
			Point PtN = Functions.PointAlongPlane(pPtPrj, Dir_Renamed, 10.0);
			Point Pt10 = (Point)Functions.ToGeo(PtN);
			PtN = (Point)Functions.ToGeo(pPtPrj);

			NativeMethods.ReturnGeodesicAzimuth(PtN.X, PtN.Y, Pt10.X, Pt10.Y, out resD, out resI);
			return resD;
		}

		public static double Dir2AztGeo(Point pPtGeo, double Dir_Renamed)
		{
			double resD;
			double resI;

			Point pPtPrj = ToPrj(pPtGeo) as Point;
			Point PtN = Functions.PointAlongPlane(pPtPrj, Dir_Renamed, 10.0);

			Point Pt10 = (Point)Functions.ToGeo(PtN);
			PtN = (Point)Functions.ToGeo(pPtPrj);

			NativeMethods.ReturnGeodesicAzimuth(PtN.X, PtN.Y, Pt10.X, Pt10.Y, out resD, out resI);
			return resD;
		}

		public static Point PointAlongPlane(Point ptFrom, double dirAngle, double Dist)
		{
			dirAngle = GlobalVars.DegToRadValue * dirAngle;
			double CosA = System.Math.Cos(dirAngle);
			double SinA = System.Math.Sin(dirAngle);

			double X = ptFrom.X + Dist * CosA;
			double Y = ptFrom.Y + Dist * SinA;

			return new Point(X, Y);
		}

		public static double ReturnDistanceInMeters(Point ptFrom, Point ptTo)
		{
			double fdX = ptTo.X - ptFrom.X;
			double fdY = ptTo.Y - ptFrom.Y;
			return System.Math.Sqrt(fdX * fdX + fdY * fdY);
		}

		public static double ReturnAngleInDegrees(Point ptFrom, Point ptTo)
		{
			double fdX = ptTo.X - ptFrom.X;
			double fdY = ptTo.Y - ptFrom.Y;
			return NativeMethods.Modulus(RadToDeg(NativeMethods.ATan2(fdY, fdX)));
		}

		public static double SubtractAzimuths(double X, double Y)
		{
			X = NativeMethods.Modulus(X);
			Y = NativeMethods.Modulus(Y);
			double result = NativeMethods.Modulus(X - Y);
			if (result > 180.0)
				result = 360.0 - result;

			return result;
		}

		public static int RGB(uint red, uint green, uint blue)
		{
			//red &= 255;		green &= 255;	blue &= 255;
			//return (int)((blue << 16) | (green << 8) | red);

			return (int)(((byte)blue) << 16) | (((byte)green) << 8) | ((byte)red);
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
				xSec = (xMin - xIMin) * 60.0;	//	xSec = System.Math.Round((xMin - xIMin) * 60.0, 2);

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

	}
}
