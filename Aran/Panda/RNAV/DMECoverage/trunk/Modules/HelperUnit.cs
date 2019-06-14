using Aran.PANDA.Common;
using System;
using System.Runtime.InteropServices;

namespace Aran.PANDA.RNAV.DMECoverage
{
	public static class HelperUnit
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

		public static string[] NavTypeNames = new string[] { "VOR", "DME", "NDB", "LOC", "TACAN", "Radar FIX" };

		public static string Tostring(this eNavaidType navType)
		{
			if (navType == eNavaidType.NONE)
				return "WPT";
			else
				return NavTypeNames[(int)navType];
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

		public static double RealMod(double X, int __base)
		{
			int N = (int)X;
			double dx = X - N;
			double result = N % __base + dx;

			if (result < 0.0)
				result +=  __base;

			if (result > __base / 2)
				result = result - __base;

			return result;
		}

		public static void DD2DMS(double Val, out int xDeg, out int xMin, out double xSec, int nMod, int decimals)
		{
			double X = Val;

			if (nMod > 0)
				X = RealMod(X, nMod);

			xDeg = (int)X;
			double dx = (X - xDeg) * 60.0;

			xMin = (int)dx;
			xSec = (dx - xMin) * 60.0;
			double factor = Math.Pow(10.0, 10.0);

			xSec = Math.Round(xSec * factor) / factor;
			if (xSec >= 60.0)
			{
				xSec = 0.0;
				xMin++;
				if (xMin == 60)
				{
					xMin = 0;
					xDeg++;
				}
			}
		}

		public static void DMS2Str(int xDeg, int xMin, double xSec, int yDeg, int yMin, double ySec, out string Xstr, out string Ystr, string LonSide, string LatSide)
		{
			string xDegStr = (xDeg).ToString();
			while (xDegStr.Length < 3)
				xDegStr = '0' + xDegStr;
			xDegStr = xDegStr + '°';

			string xMinStr = (xMin).ToString();
			while (xMinStr.Length < 2)
				xMinStr = '0' + xMinStr;
			xMinStr = xMinStr + '\'';

			string xSecStr = xSec.ToString("00.00");
			xSecStr = xSecStr + '\"' + LonSide;
			Xstr = xDegStr + xMinStr + xSecStr;
			// =================================================
			string yDegStr = (yDeg).ToString();
			while (yDegStr.Length < 2)
				yDegStr = '0' + yDegStr;
			yDegStr = yDegStr + '°';

			string yMinStr = (yMin).ToString();
			while (yMinStr.Length < 2)
				yMinStr = '0' + yMinStr;
			yMinStr = yMinStr + '\'';

			string ySecStr = ySec.ToString("00.00");
			ySecStr = ySecStr + '\"' + LatSide;
			Ystr = yDegStr + yMinStr + ySecStr;
		}

		public static void DD2Str(double xDeg, double yDeg, out string Xstr, out string Ystr, out string LonSide, out string LatSide)
		{
			if (xDeg >= 0.0)
				LonSide = "E";
			else
			{
				xDeg = -xDeg;
				LonSide = "W";
			}

			if (yDeg >= 0.0)
				LatSide = "N";
			else
			{
				yDeg = -yDeg;
				LatSide = "S";
			}

			double xSec, ySec;
			int ixDeg, iyDeg, xMin, yMin;
			DD2DMS(xDeg, out ixDeg, out xMin, out xSec, 360, 2);
			DD2DMS(yDeg, out iyDeg, out yMin, out ySec, 0, 2);
			DMS2Str(ixDeg, xMin, xSec, iyDeg, yMin, ySec, out Xstr, out Ystr, LonSide, LatSide);
		}

		public static double DMS2DD(double Deg, double Min, double Sec, int Sign, int nMod)
		{
			double result = (Math.Abs(Deg) + Math.Abs(Min / 60.0) + Math.Abs(Sec / 3600.0)) * Sign;

			result = RoundTo(result, -10);
			if ((nMod > 0))
				result = RealMod(result, nMod);

			return result;
		}

		//public static double IntPower(double Base, int Exponent)
		//{
		//	double result = 1.0;

		//	if (Exponent < 0)
		//	{
		//		Base = 1.0 / Base;
		//		Exponent = -Exponent;
		//	}

		//	for (int i = 1; i <= Exponent; i++)
		//		result *= Base;

		//	return result;
		//}

		public static double RoundTo(double AValue, int ADigit)
		{
			double LFactor = Math.Pow(10, ADigit);	//IntPower(10, ADigit);		//
			return Math.Round(AValue / LFactor) * LFactor;
		}

		/*
		public static int GetDMEList(string orgID, ADHPType ahp, Point ptCentre, double Range, List<NavaidType> DMEList)
        {
            int I, N;

            TPandaCollection  SignificantPoints = ARANGlobals.Units.ARANGlobals.GObjectDirectory.SignificantPoints[orgID, ahp, ptCentre, Range];
			TAIXM Item;

			DMEList.Clear();
            N = SignificantPoints.Count;

            for (I = 0; I < N; I ++ )
            {
                Item = SignificantPoints[I];
                if (Item.AIXMType == AIXMTypes.TAIXMType.atDME)
                    DMEList.Add(Item);
            }

            return DMEList.Count;
        }
		 * */
	} // end HelperUnit

}

