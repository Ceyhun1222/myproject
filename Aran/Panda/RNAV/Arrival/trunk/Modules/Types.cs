using Aran.Aim.Features;
using Aran.Geometries;
using Aran.PANDA.Common;

namespace Aran.PANDA.RNAV.Arrival
{
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

	//public enum PBN_Spec
	//{
	//	RNAV_1
	//	RNAV_2,
	//	RNP_4,
	//	RNAV_5,
	//}

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

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct Procedure
	{
		public Route pFeature;

		override public string ToString()
		{
			if (pFeature == null)
				return "";

			return string.Format("{0}", pFeature.Name);
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct Segment
	{
		public RouteSegment pFeature;
		public Route proc;

		public FIX Start;
		public FIX End;
		public MultiLineString NominalTrack;

		public LegArrival Forwardleg;
		public LegArrival Backwardleg;

		public ePBNClass PBNType;
		//public string RouteName;

		public int UpperLimit;

		public double Altitude;
		public double TAS;
		public double IAS;
		public double MOC;

		public double WindSpeed;
		public double Dir;
		public double MaxTurnAtStart;
		public double MaxTurnAtEnd;
		public double TrueCourse;
		public double Lenght;

		public int NominalTracktElem;
		public int PrimaryProtectionAreatElem;
		public int SecondaryProtectionAreatElem;
		override public string ToString()
		{
			string result = "";

			if (Start != null && Start.Name != null)
			{
				result = Start.Name;

				if (End != null && End.Name != null)
					result = Start.Name + " - " + End.Name;
			}
			else if (End != null || End.Name != null)
				result = End.Name;

			return result;
		}
	}
}
