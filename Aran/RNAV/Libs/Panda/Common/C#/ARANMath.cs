using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;


namespace ARAN.Common
{

	public enum SideDirection
	{
		sideRight = -1,
		sideOn = 0,
		sideLeft = 1
	}

	public static class TurnDirection
	{
		public const int CW = -1;
		public const int CCW = 1;
	}

	public static class ARANMath
	{

		public const double C_E = 2.71828182845904523536;
		private const double C_LOG2E = 1.44269504088896340736;
		public const double C_LOG10E = 0.434294481903251827651;
		public const double C_LN2 = 0.693147180559945309417;
		public const double C_LN10 = 2.30258509299404568402;

		public const double C_PI = 3.1415926535897932384626433832795;
		public const double C_2xPI = 2 * C_PI;
		public const double C_PI_2 = C_PI * 0.5;
		public const double C_1_PI = 0.318309886183790671538;
		public const double C_2_PI = 0.636619772367581343076;
		public const double C_1_SQRTPI = 0.564189583547756286948;
		public const double C_2_SQRTPI = 1.12837916709551257390;
		public const double C_SQRT2 = 1.41421356237309504880;
		public const double C_SQRT_2 = 0.707106781186547524401;

		public const double C_SQRT3 = 1.7320508075688772;
		public const double C_SQRT_3 = 0.57735026918962584;

		public const double Epsilon = 10e-8;
		public const double EpsilonDistance = 0.001;
		public const double Epsilon_2Distance = EpsilonDistance * EpsilonDistance;
		public const double EpsilonDegree = 1.0 / 3600.0;
		public const double EpsilonRadian = EpsilonDegree / 360;

		public enum RoundTo
		{
			rtNone,
			rtFloor,
			rtNear,
			rtCeil
		}

		public static double Modulus(double X, double Y)
		{
			double Result;
			X = X - Math.Floor(X / Y) * Y;
			if (X < 0.0) X = X + Y;
			if (X == Y) X = 0.0;
			Result = X;
			return Result;
		}

		public static double AdvancedRound(double Value, double Accuracy, RoundTo RoundMode)
		{
			double nValue;
			double Result;
			Result = Value;

			if ((RoundMode == RoundTo.rtFloor) || (RoundMode == RoundTo.rtCeil) || (RoundMode == RoundTo.rtNear))
			{
				nValue = Value / Accuracy;
				if (System.Math.Abs(nValue - System.Math.Round(nValue)) < Epsilon)
					RoundMode = RoundTo.rtNear;
				switch (RoundMode)
				{
					case RoundTo.rtFloor: Result = Math.Floor(nValue) * Accuracy; break;
					case RoundTo.rtNear: Result = System.Math.Round(nValue) * Accuracy; break;
					case RoundTo.rtCeil: Result = Math.Ceiling(nValue) * Accuracy; break;
				}
			}
			return Result;
		}

		public static string RoundToStr(double Value, double Accuracy)
		{
			return RoundToStr(Value, Accuracy, RoundTo.rtNear);
		}

		public static string RoundToStr(double Value, double Accuracy, RoundTo roundMode)
		{
			string s;
			double n;
			string Result;

			s = "0";
			n = Math.Log10(Accuracy);
			if (n < 0)
				s = "0." + new String('#', (int)Math.Ceiling(-n));
			Result = Convert.ToDouble(AdvancedRound(Value, Accuracy, roundMode)).ToString(s);
			return Result;
		}

		public static SideDirection InversDirection(SideDirection Direction)
		{
			return (SideDirection)Enum.ToObject(typeof(SideDirection), -(int)Direction);
		}

		public static SideDirection MultiplyDirections(SideDirection Direction1, SideDirection Direction2)
		{
			SideDirection Result;

			Result = (SideDirection)Enum.ToObject(typeof(SideDirection), (int)(Direction1) * (int)(Direction2));//SideDirection((int)(Direction1)*(int)(Direction2));
			return Result;
		}

		public static double IASToTAS(double IAS, double H, double dT)
		{
			return IAS * 171233.0 * Math.Sqrt(288.0 + dT - 0.006496 * H) / Math.Pow(288.0 - 0.006496 * H, 2.628);
		}

		public static double IASToTASForRnav(double ias, double H)
		{
			double k, P, TH;
			k = -5.2558761132785;
			P = Math.Pow(1013.25 * (288 / (288 - 0.0065 * H)), k);//					= 700.94615421975
			TH = 288 - 0.0065 * H + 15;//					= 283.5
			return 102.06 * Math.Sqrt(TH) * Math.Sqrt(Math.Sqrt(1 + (0.00067515 * ias * ias / P) * (1 + ias * ias / 6003025)) - 1);//					= 374.32914869661
		}

		public static double BankToRadius(double radianBank, double VInMetrsInSec)
		{
			double Rv;
			double Result;

			Rv = 1.76527777777777777777 * Math.Tan(radianBank) / (180 * VInMetrsInSec);
			if (Rv > 0.003) Rv = 0.003;

			if (Rv > 0.0)
				Result = VInMetrsInSec / (5.555555555555555555555 * 180 * Rv);
			else
				Result = -1;
			return Result;
		}

		public static double RadiusToBank(double radius, double VInMetrsInSec)
		{
			double Result;
			if (radius > 0.0)
				Result = Math.Atan(Math.Pow((VInMetrsInSec) / (9.807098765432098 * radius), 2));
			else
				Result = -1;
			return Result;
		}

		public static int SideFrom2Angle(double DirInRadian0, double DirInRadian1)
		{
			double rAngle;
			int Result;

			rAngle = SubtractAngles(DirInRadian0, DirInRadian1);

			if (((360 - rAngle) < EpsilonRadian) || (rAngle < EpsilonRadian))
				Result = 0;
			else
			{
				rAngle = Modulus(DirInRadian1 - DirInRadian0, 360);
				if (rAngle < 180)
					Result = 1;
				else
					Result = -1;
			}
			return Result;
		}

		public static SideDirection SideFrom2Angle_new(double DirInRadian0, double DirInRadian1)
		{
			double rAngle;
			SideDirection Result;

			rAngle = SubtractAngles(DirInRadian0, DirInRadian1);

			if (((360 - rAngle) < EpsilonRadian) || (rAngle < EpsilonRadian))
				Result = SideDirection.sideOn;
			else
			{
				rAngle = Modulus(DirInRadian1 - DirInRadian0, 360);
				if (rAngle < 180)
					Result = SideDirection.sideRight;
				else
					Result = SideDirection.sideLeft;
			}
			return Result;
		}

		public static double SubtractAngles(double X, double Y)
		{
			double Result;

			X = Modulus(X, C_2xPI);
			Y = Modulus(Y, C_2xPI);
			Result = Modulus(X - Y, C_2xPI);
			if (Result > C_PI)
				Result = C_2xPI - Result;

			return Result;
		}

		public static double SubtractAnglesWithSign(double startRad, double endRad, SideDirection side)
		{
			double Result;
			Result = Modulus((endRad - startRad) * (int)(side), 2 * Math.PI);
			if (Result > Math.PI)
				Result = Result - 2 * Math.PI;
			return Result;
		}

		public static SideDirection ChangeDirection(SideDirection dir)
		{
			if (dir == SideDirection.sideLeft)
				return SideDirection.sideRight;
			if (dir == SideDirection.sideRight)
				return SideDirection.sideLeft;
			return dir;

		}
	}
}
