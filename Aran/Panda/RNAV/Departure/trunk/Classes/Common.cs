using System;
using Aran.Geometries;
using Aran.PANDA.Common;

namespace Aran.PANDA.RNAV.Departure
{
	[Serializable]
	[System.Runtime.InteropServices.ComVisible(false)]
	public delegate void DoubleValEventHandler(object sender, double value);

	[System.Runtime.InteropServices.ComVisible(false)]
	public enum TerminationType
	{
		AtHeight = 0, AtWPT = 1, Intercept = 2
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public enum Degree2StringMode
	{
		DD,
		DM,
		DMS,
		DMSLat, //NS
		DMSLon	//EW
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ReportHeader
	{
		public string Aerodrome;
		public string Procedure;
		public string Category;
		public string Database;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ReportPoint
	{
		public string Description;
		public double TrueCourse;
		public double turnAngle;
		public double Height;
		public double Radius;
		public double TurnArcLen;
		public double DistToNext;

		public double Lat;
		public double Lon;

		public double CenterLat;
		public double CenterLon;

		public int Turn;
	}
}
