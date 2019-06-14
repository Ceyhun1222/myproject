using System;
using Aran.Geometries;
using Aran.PANDA.Common;

namespace Aran.PANDA.RNAV.SGBAS
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
		public double Direction;
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

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct TraceSegment
	//{
	//    public Point ptIn;			//pPtStart;
	//    public Point ptOut;		//pPtEnd;
	//    public Polyline PathPrj;	//pNominalPoly;
	//    public Polygon pProtectArea;

	//    public eSegmentType SegmentCode;
	//    public CodeSegmentPath LegType;

	//    public NavaidType GuidanceNav;
	//    public NavaidType InterceptionNav;

	//    public double HStart;
	//    public double HFinish;
	//    public double H1, H2;

	//    public double Length;
	//    public double PDG;
	//    public double BankAngle;
	//    public double DirIn;
	//    public double DirOut;

	//    //=========================
	//    public double DirBetween;
	//    public string StCoords;
	//    public string FinCoords;
	//    //=========================
	//    public Point PtCenter1;
	//    public string Center1Coords;
	//    public int Turn1Dir;
	//    public double Turn1R;
	//    public double Turn1Angle;
	//    public double Turn1Length;
	//    public string St1Coords;
	//    public string Fin1Coords;
	//    //=========================
	//    public Point PtCenter2;
	//    public string Center2Coords;
	//    public int Turn2Dir;
	//    public double Turn2R;
	//    public double Turn2Angle;
	//    public double Turn2Length;
	//    public string St2Coords;
	//    public string Fin2Coords;
	//    //=========================
	//    public double BetweenTurns;
	//    public string Comment;
	//    public string RepComment;
	//}

}
