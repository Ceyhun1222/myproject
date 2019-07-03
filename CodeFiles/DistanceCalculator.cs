using System;

namespace DistanceCalculator
{
	static class DistanceCalculator
	{
		private const double DegToRad = Math.PI * (1.0 / 180.0);
		private const double InvPIx2 = 2.0 / Math.PI;
		private const double r00 = 6356824.05;
		private const double r45 = 6374541.883;
		private const double r90 = 6399592.574;
		private const double d2 = 4.0 * (r90 - 2.0 * r45 + r00);
		private const double d1 = r90 - r00;

		static private double m_R = 6371010.0;

		static public void setNewR(double x)
		{
			x = InvPIx2 * Math.Abs(x);
			double dx = x - 0.5;
			m_R = r45 + dx * (d1 + 0.5 * d2 * dx);
			setR = false;
		}

		static public Boolean setR { get; set; }

		static public double CalcDistance(double lon1, double lat1, double lon2, double lat2)
		{
			double lon1r, lat1r, lon2r, lat2r;

			lon1r = lon1 * DegToRad;
			lat1r = lat1 * DegToRad;
			lon2r = lon2 * DegToRad;
			lat2r = lat2 * DegToRad;

			if (setR)
				setNewR(0.5 * (lat1r + lat2r));

			return m_R * Math.Acos(Math.Sin(lat1r) * Math.Sin(lat2r) + Math.Cos(lat1r) * Math.Cos(lat2r) * Math.Cos(lon2r - lon1r));
		}
	}
}
