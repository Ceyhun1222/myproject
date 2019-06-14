using Aran.PANDA.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.PANDA.RNAV.Approach
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
		DMSLon  //EW
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

	public class Comparers
	{
		public static int PartComparerReqH(ObstacleData o0, ObstacleData o1)
		{
			if (o0.ReqH < o1.ReqH) return 1;
			if (o0.ReqH > o1.ReqH) return -1;

			return 0;
		}

		public static int PartComparerDist(ObstacleData o0, ObstacleData o1)
		{
			return o0.Dist.CompareTo(o1.Dist);
		}
	}

	//public class DistanceComparer : IComparer
	//{
	//	int IComparer.Compare(Object x, Object y)
	//	{
	//		return ((ObstacleData)x).Dist.CompareTo (((ObstacleData)y).Dist);
	//	}
	//}
}
