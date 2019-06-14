using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Enums;
using Aran.Geometries;
using Aran.PANDA.Common;
using Aran.PANDA.RNAV.EnRoute.Modules;

namespace Aran.PANDA.RNAV.EnRoute
{

	//[System.Runtime.InteropServices.ComVisible(false)]
	//[FlagsAttribute]	//[Flags]
	//public enum SegmentDirection
	//{
	//	Empty = 0,
	//	Forward = 1,
	//	Backward = 2,
	//	Both = 3
	//}


	[System.Runtime.InteropServices.ComVisible(false)]
	public enum Degree2StringMode
	{
		DD,
		DM,
		DMS,
		DMSLat, //NS
		DMSLon	//EW
	}

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct ReportHeader
	//{
	//	public string Aerodrome;
	//	public string Procedure;
	//	public string Category;
	//	public string Database;
	//}

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
		public double TrueTrack;
		public double MagnTrack;

		public double ReverseTrueTrack;
		public double ReverseMagnTrack;

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
	//    public Point ptOut;			//pPtEnd;
	//    public Polyline PathPrj;		//pNominalPoly;
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

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct Segment
	{
		public FIX Start;
		public FIX End;
		public MultiLineString NominalTrack;

		public EnRouteLeg Forwardleg;
		public EnRouteLeg Backwardleg;
		public ObstacleContainer MergedObstacles;

		public CodeDirection Granted;
		public CodeDirection Direction;
		public ePBNClass PBNType;
		public eSensorType SensorType;

		public int UpperLimit;

		public double Altitude;
		public double TAS;
		public double IAS;
		public double MOC;
		public double MOCA;

		public double Dir;
		public double MaxTurnAtStart;
		public double MaxTurnAtEnd;
		public double TrueTrack;
		public double ReverseTrueTrack;
		public double Length;

		public int NominalTracktElem;
		public int PrimaryProtectionAreatElem;
		public int SecondaryProtectionAreatElem;

	    public List<ObstacleReport> ObstacleList { get; set; }
	}

    public enum Side
    {
        Forward,
        BackWard,
        Mix
    }
}
