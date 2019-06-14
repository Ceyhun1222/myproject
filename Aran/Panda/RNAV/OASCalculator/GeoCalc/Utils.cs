using System;

namespace GeoCalc
{
	public struct TGCoordinate
	{
		public double X;
		public double Y;
	}

	public static class Utils
	{
		public static double Sqr(double x)
		{
			return x * x;
		}

		public static double Hypot(double x, double y)
		{
			return Math.Sqrt(x * x + y * y);
		}

		public static double Mod2PI(double x)
		{
			double k = 2.0 * Math.PI;

			x -= Math.Floor(x / k) * k;

			if (x < 0.0)
				x += k;

			if (x == k)
				x = 0.0;
			return x;
		}
		public static double Mod360(double x)
		{
			x -= Math.Floor(x / 360.0) * 360.0;

			if (x < 0.0)
				x += 360.0;

			if (x == 360.0)
				x = 0.0;
			return x;
		}

		public static double DegToRad(double x)
		{
			return x * (Math.PI / 180.0);
		}

		public static double RadToDeg(double x)
		{
			return x * (180.0 / Math.PI);
		}
	}
}
