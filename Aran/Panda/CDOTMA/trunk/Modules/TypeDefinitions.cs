using System;
using System.Collections.Generic;
using PDM;
using CDOTMA.CoordinatSystems;
using NetTopologySuite.Geometries;
using GeoAPI.Geometries;

namespace CDOTMA
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public enum eRoundMode
	{
		NONE = 0,
		FLOOR = 1,
		NERAEST = 2,
		CEIL = 3,
		SPECIAL_FLOOR = 4,
		SPECIAL_NERAEST = 5,
		SPECIAL_CEIL = 6,
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
	public enum eRWY
	{
		PtStart = 0,
		PtTHR = 1,
		PtEnd = 2,
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public enum eNavaidType
	{
		CodeNONE = -1,
		CodeVOR = 0,
		CodeDME = 1,
		CodeNDB = 2,
		CodeLLZ = 3,
		CodeTACAN = 4
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public enum eSegmentType
	{
		NONE,
		straight,
		toHeading,
		courseIntercept,
		directToFIX,
		turnAndIntercept,
		arcIntercept,
		arcPath
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public enum eIntersectionType
	{
		ByDistance = 1,
		ByAngle = 2,
		OnNavaid = 3,
		RadarFIX = 4
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	[FlagsAttribute()]
	public enum eAreaType : int
	{
		PrimaryArea = 4,
		BufferArea = 8
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	[FlagsAttribute()]
	public enum RouteSegmentDir
	{
		NONE = 0,
		Forward = 1,
		Backward = 2,
		Both = 3
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ADHPType
	{
		public Point pPtGeo;
		public Point pPtPrj;

		public string Name;
		public string Identifier;
		public string OrgID;
		//public Guid Identifier;
		//public Guid OrgID;

		public double MagVar;
		public double Elev;
		public double WindSpeed;
		public double ISAtC;
		public double TransitionLevel;
		public long index;
		public AirportHeliport pAirportHeliport;
		public override string ToString()
		{
			if (Name != null)
				return Name;
			return Identifier;
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct RWYType
	{
		public EnumArray<Point, eRWY> pPtGeo;
		public EnumArray<Point, eRWY> pPtPrj;
		//public EnumArray<SignificantPoint, eRWY> pSignificantPoint;

		public string Name;
		public string PairName;
		public Guid Identifier;
		public Guid PairID;
		public Guid ADHP_ID;

		public double TrueBearing;
		public double ClearWay;
		public double Length;

		public long index;
		public bool Selected;

		public RunwayDirection pRunwayDir;
		public void Initialize()
		{
			pPtGeo = new EnumArray<Point, eRWY>();
			pPtPrj = new EnumArray<Point, eRWY>();
			//pSignificantPoint = new EnumArray<SignificantPoint, eRWY>();
		}
	}

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct ILSType
	//{
	//    public Point pPtGeo;
	//    public Point pPtPrj;
	//    public Feature pFeature;
	//    public Guid RWY_ID;
	//    public Guid Identifier;
	//    public string CallSign;
	//    public long Category;
	//    public double GPAngle;
	//    public double GP_RDH;
	//    public double MagVar;
	//    public double Course;
	//    public double LLZ_THR;
	//    public double AngleWidth;
	//    public double SecWidth;
	//    public long index;
	//    public long Tag;

	//    public SignificantPoint GetSignificantPoint()
	//    {
	//        SignificantPoint sp = new SignificantPoint();
	//        sp.NavaidSystem = pFeature.GetFeatureRef();
	//        return sp;
	//    }
	//}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct NavaidType
	{
		public Point pPtGeo;
		public Point pPtPrj;

		//public Feature pFeature;
		//public NavaidComponent pFeature;
		public PDMObject pFeature;

		//public Guid Identifier;
		public string ID;
		public string Name;
		public string CallSign;
		public double MagVar;
		public eNavaidType TypeCode;
		public double Range;
		public long PairNavaidIndex;
		public eNavaidType PairNavaidType;
		public long index;
		public double Course;
		public double GPAngle;
		public double GP_RDH;
		public double LLZ_THR;
		public double SecWidth;
		public double AngleWidth;
		public long ValCnt;
		public double[] ValMin;
		public double[] ValMax;
		public long Tag;

		//public SignificantPoint GetSignificantPoint()
		//{
		//    SignificantPoint sp = new SignificantPoint();
		//    sp.NavaidSystem = pFeature.GetFeatureRef();
		//    return sp;
		//}

		override public string ToString()
		{
			if (Name != null && Name != "")
				return Name;
			if (CallSign != null && CallSign != "")
				return CallSign;
			return "";
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct WPT_FIXType
	{
		public Point pPtGeo;
		public Point pPtPrj;

		//public Feature pFeature;
		//public WayPoint pFeature;
		public PDMObject pFeature;

		//public Guid Identifier;
		public string ID;

		public string Name;
		public string CallSign;
		public double MagVar;
		public eNavaidType TypeCode;
		public long Tag;

		override public string ToString()
		{
			if (Name != null && Name != "")
				return Name;
			if (CallSign != null && CallSign != "")
				return CallSign;
			return "";
		}
	}

	//==========================================================================================
	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ObstacleData
	{
		public Point pPtGeo;
		public Point pPtPrj;
		public double Dist;
		public double CLShift;
		public double DistStar;

		public double Height;
		public double EffectiveHeight;
		public double ReqH;
		public double hPenet;

		public double MOC;
		public double fTmp;
		//=========================
		public double PDG;
		public double NomPDGDist;
		public double PDGToTop;
		public double PDGAvoid;

		public double ReqTNH;
		public double TrackAdjust;
		public double CourseAdjust;
		public double SectorAngle;
		public double Range;

		public bool Prima;
		public bool Ignored;
		public bool IsExcluded;
		public bool IsInteresting;
		//=========================
		public int Plane;

		public int Flags;
		public int AreaFlags;

		public double fSort;
		public string sSort;

		public int Owner;
		public int Index;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct Obstacle
	{
		public Geometry pGeomGeo;
		public Geometry pGeomPrj;

		public string UnicalName;
		public string TypeName;

		public Guid Identifier;
		public long ID;
		//public bool IgnoredByUser;

		public double Height;
		public double HorAccuracy;
		public double VertAccuracy;

		public int[] Parts;
		public int PartsNum;
		public int NIx;
		public int Index;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ObstacleContainer
	{
		public Obstacle[] Obstacles;
		public ObstacleData[] Parts;

		void AddPart(ref int Last, int Dest, ObstacleContainer from, ObstacleData Src)
		{
			Parts[Dest] = Src;

			if (from.Obstacles[Src.Owner].NIx < 0)
			{
				Last += 1;
				Obstacles[Last] = from.Obstacles[Src.Owner];
				Obstacles[Last].PartsNum = 0;
				System.Array.Resize<int>(ref Obstacles[Last].Parts, from.Obstacles[Src.Owner].PartsNum - 1);
				from.Obstacles[Src.Owner].NIx = Last;
			}

			Parts[Dest].Owner = from.Obstacles[Src.Owner].NIx;
			Parts[Dest].Index = Obstacles[Parts[Dest].Owner].PartsNum;
			Obstacles[Parts[Dest].Owner].Parts[Obstacles[Parts[Dest].Owner].PartsNum] = Dest;
			Obstacles[Parts[Dest].Owner].PartsNum += 1;
		}

		void MovePart(int Dest, int Src)
		{
			int L, K;
			L = Parts[Dest].Owner;
			K = Parts[Dest].Index;

			Obstacles[L].PartsNum -= 1;
			Obstacles[L].Parts[K] = Obstacles[L].Parts[Obstacles[L].PartsNum];
			Parts[Obstacles[L].Parts[K]].Index = K;

			Parts[Dest] = Parts[Src];
			Obstacles[Parts[Dest].Owner].Parts[Parts[Dest].Index] = Dest;
			//N -= 1;
		}

		void MoveObst(int Dest, int Src)
		{
		}
	}

	//==========================================================================================

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct ObstacleType
	//{
	//    public IPoint pPtGeo;
	//    public IPoint pPtPrj;
	//    public string Name;
	//    public string Type;
	//    public long ID;
	//    public Guid Identifier;
	//    public long index;
	//    public double Height;
	//    public double Dist;
	//    public double DistStar;
	//    public double PDG;
	//    public double NomPDGDist;
	//    public double PDGToTop;
	//    public double PDGAvoid;
	//    public double MOC;
	//    public double hPenet;
	//    public double ReqTNH;
	//    public double CLShift;
	//    public double TrackAdjust;
	//    public double CourseAdjust;
	//    public double SectorAngle;
	//    public double Range;
	//    public double HorAccuracy;
	//    public double VertAccuracy;
	//    public double fTmp;
	//    public double fSort;
	//    public string sSort;
	//    public bool Prima;
	//    public bool Ignored;
	//    public bool IsExcluded;
	//    public bool IsInteresting;
	//}

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct ProhibitedSector
	//{
	//    public double LeftAngle;
	//    public double RightAngle;
	//    public ObstacleData Obstacle;
	//    public MultiPoint SectorGraphics;
	//    public int SectorColor;
	//}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct PDGLevel
	{
		public double PDG;
		public double Range;
		public long AdjustANgle;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct Interval
	{
		public double Left;
		public double Right;
	}

	//=======================================================

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct AirspaceVolumeType
	{
		public AirspaceVolume pAirspaceVolume;
		public MultiPolygon geometryGeo;
		public MultiPolygon geometryPrj;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct AirspaceType
	{
		public string Name;
		public Airspace pAirspace;
		public List<AirspaceVolumeType> AsVT;

		override public string ToString()
		{
			if (Name != null && Name != "")
				return Name;
			return "";
		}
	}

	//=======================================================

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ProcedureType
	{
		public string Name;
		public PROC_TYPE_code procType;
		public PDMObject pProcedure;
		public List<Transition> procTransitions;

		public MultiPolygon RouteBuffer;
		public MultiLineString NominalLine;
		public object Tag;

		override public string ToString()
		{
			if (Name != null && Name != "")
				return Name;
			return "";
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct Transition
	{
		public string Name;
		public ProcedureTransitions pProcTransition;
		public List<Leg> procLegs;
		public ProcedureType Owner;

		public MultiPolygon RouteBuffer;
		public MultiLineString NominalLine;

		override public string ToString()
		{
			if (Name != null && Name != "")
				return Name;
			return "";
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct Leg
	{
		public string Name;
		public PDMObject pProcLeg;
		public SegmentPoint ptStart;
		public SegmentPoint ptEnd;

		public MultiLineString PathGeomGeo;
		public MultiLineString PathGeomPrj;
		public CodeSegmentPath PathCode;
		//==================================
		public Point ptStartGeo;
		public Point ptEndGeo;

		public Point ptStartPrj;
		public Point ptEndPrj;

		public ProcedureType Owner; 
		//public Transition Owner;

		public MultiPolygon pProtectArea;
		public NavaidType GuidanceNav;
		public NavaidType InterceptionNav;
		public double HStart;
		public double HFinish;
		public double Length;
		public double PDG;
		public double BankAngle;
		public double DirIn;
		public double DirOut;
		public double AztIn;
		public double AztOut;

		override public string ToString()
		{
			if (Name != null && Name != "")
				return Name;
			return "";
		}
	}

	//=======================================================

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct TraceSegment
	//{
	//    public Point ptIn;					//pPtStart;
	//    public Point ptOut;					//pPtEnd;
	//    public MultiLineString PathPrj;		//pNominalPoly;
	//    public MultiPolygon pProtectArea;

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

	//=======================================================

	[System.Runtime.InteropServices.ComVisible(false)]
	public class LegPoint
	{
		public LegPoint(WPT_FIXType from)
		{
			_parentWpt = from;
			_parentNav.TypeCode = (eNavaidType)(-2);

			pPtGeo = from.pPtGeo;

			pPtPrj = from.pPtPrj;
			_pPtPrj = (Point)pPtPrj.Clone();

			pFeature = from.pFeature;
			ID = from.ID;

			Name = from.Name;
			CallSign = from.CallSign;
			MagVar = from.MagVar;
			TypeCode = from.TypeCode;
			Tag = from.Tag;
			legs = new List<TraceLeg>();

			EnvelopeInternal = new Envelope();
			if (pPtPrj != null)
				EnvelopeInternal.ExpandToInclude(pPtPrj.Coordinate);
		}

		public LegPoint(NavaidType from)
		{
			_parentNav = from;
			_parentWpt.TypeCode = (eNavaidType)(-2);

			pPtGeo = from.pPtGeo;
			pPtPrj = from.pPtPrj;

			_pPtPrj = (Point)pPtPrj.Clone();

			pFeature = from.pFeature;
			ID = from.ID;

			Name = from.Name;
			CallSign = from.CallSign;
			MagVar = from.MagVar;
			TypeCode = from.TypeCode;
			Tag = from.Tag;
			legs = new List<TraceLeg>();

			EnvelopeInternal = new Envelope();
			if (pPtPrj != null)
				EnvelopeInternal.ExpandToInclude(pPtPrj.Coordinate);
		}

		public int CheckLeg(TraceLeg leg)
		{
			int result = 0;

			if (leg.ptStart != null && ID == leg.ptStart.PointChoiceID)
			{
				leg.StartPoint = this;
				result = 1;
			}

			if (result == 0 && leg.ptEnd != null && ID == leg.ptEnd.PointChoiceID)
			{
				leg.EndPoint = this;
				result = 2;
			}
			//dsend = Functions.ReturnDistanceInMeters(pPtPrj, leg.ptEndPrj);

			if (result == 0)
				return 0;

			legs.Add(leg);

			return result;
		}

		public bool Modified()
		{
			double dstst = Functions.ReturnDistanceInMeters(pPtPrj, _pPtPrj);

			if (dstst <= GlobalVars.distEps)
				return false;

			foreach (TraceLeg tlg in legs)
			{
				if (tlg.StartPoint != null && tlg.StartPoint.ID == this.ID)
				{
					tlg.StartPoint.pPtPrj.X = this.pPtPrj.X;
					tlg.StartPoint.pPtPrj.Y = this.pPtPrj.Y;
				}
				else
				{
					tlg.EndPoint.pPtPrj.X = this.pPtPrj.X;
					tlg.EndPoint.pPtPrj.Y = this.pPtPrj.Y;
				}

				if (tlg.StartPoint != null && tlg.EndPoint != null)
				{
					LineString[] ls = new LineString[1];
					ls[0] = new LineString(new Coordinate[2] { tlg.StartPoint.pPtPrj.Coordinate, tlg.EndPoint.pPtPrj.Coordinate });
					tlg.PathGeomPrj = new MultiLineString(ls);
					Geometry geom = (Geometry)tlg.PathGeomPrj.Buffer(9000.0);
					tlg.pProtectArea = (Polygon)geom;
				}
			}

			return true;
		}

		public LegPoint Clone()
		{
			LegPoint result;
			if (_parentWpt.pPtGeo == null)
				result = new LegPoint(_parentNav);
			else
				result = new LegPoint(_parentWpt);

			result.pFeature = pFeature;
			result._parentWpt = _parentWpt;
			result._parentNav = _parentNav;

			result._pPtPrj = (Point)_pPtPrj.Clone();

			result.pPtGeo = (Point)pPtGeo.Clone();
			result.pPtPrj = (Point)pPtPrj.Clone();

			result.ID=ID;

			result.Name = Name;
			result.CallSign = CallSign;
			result.MagVar = MagVar;
			result.TypeCode = TypeCode;


			result.Tag = Tag;
			result.EnvelopeInternal = EnvelopeInternal.Clone();
			result.legs.AddRange(legs);

			return result;
		}

		public PDMObject pFeature;
		WPT_FIXType _parentWpt;
		NavaidType _parentNav;

		Point _pPtPrj;

		public Point pPtGeo;
		public Point pPtPrj;

		public string ID;

		public string Name;
		public string CallSign;
		public double MagVar;
		public eNavaidType TypeCode;

		public WPT_FIXType parentWpt { get { return _parentWpt; } }
		public NavaidType parentNav { get { return _parentNav; } }

		public long Tag;
		public List<TraceLeg> legs;
		public List<TraceLeg> selctedLegs;

		public Envelope EnvelopeInternal;

		override public string ToString()
		{
			if (Name != null && Name != "")
				return Name;
			if (CallSign != null && CallSign != "")
				return CallSign;
			return "";
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public class TraceLeg 
	{
		public TraceLeg(Leg leg)
		{
			Name = leg.Name;
			pProcLeg = leg.pProcLeg;
			ptStart = leg.ptStart;
			ptEnd = leg.ptEnd;

			PathGeomGeo = leg.PathGeomGeo;
			PathGeomPrj = leg.PathGeomPrj;
			PathCode = leg.PathCode;
			//==================================
			ptStartGeo = leg.ptStartGeo;
			ptEndGeo = leg.ptEndGeo;

			ptStartPrj = leg.ptStartPrj;
			ptEndPrj = leg.ptEndPrj;

			Owner = leg.Owner;
			//pProtectArea = leg.pProtectArea;
			GuidanceNav = leg.GuidanceNav;
			InterceptionNav = leg.InterceptionNav;
			HStart = leg.HStart;
			HFinish = leg.HFinish;
			Length = leg.Length;
			PDG = leg.PDG;
			BankAngle = leg.BankAngle;

			AztIn = leg.AztIn;
			AztOut = leg.AztOut;

			DirIn = leg.DirIn;
			DirOut = leg.DirOut;

			Direction = RouteSegmentDir.NONE;

			EnvelopeInternal = new Envelope();
			if (ptStartPrj != null)
				EnvelopeInternal.ExpandToInclude(ptStartPrj.Coordinate);
			if (ptEndPrj != null)
				EnvelopeInternal.ExpandToInclude(ptEndPrj.Coordinate);
			if (PathGeomPrj != null)
				EnvelopeInternal.ExpandToInclude(PathGeomPrj.EnvelopeInternal);
		}

		public string Name;
		public PDMObject pProcLeg;
		public SegmentPoint ptStart;
		public SegmentPoint ptEnd;

		public MultiLineString PathGeomGeo;
		public MultiLineString PathGeomPrj;
		public CodeSegmentPath PathCode;
		//==================================
		public LegPoint StartPoint;
		public LegPoint EndPoint;

		public Point ptStartGeo;
		public Point ptEndGeo;

		public Point ptStartPrj;
		public Point ptEndPrj;

		public ProcedureType Owner;
		public Polygon pProtectArea;

		public NavaidType GuidanceNav;
		public NavaidType InterceptionNav;
		public RouteSegmentDir Direction;
		public double DirIn;
		public double DirOut;

		public double AztIn;
		public double AztOut;

		public double HStart;
		public double HFinish;

		public double BankAngle;
		public double PDG;
		public double Length;

		public Envelope EnvelopeInternal;

		override public string ToString()
		{
			if (Name != null && Name != "")
				return Name;
			return "";
		}
	}

	//=======================================================

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ReportPoint
	{
		public string Description;

		public double Lat;
		public double Lon;
		public double Direction;

		//public double PDG;
		public double Height;
		public double DistToNext;

		public int Turn;
		public double Radius;
		public double turnAngle;
		public double TurnArcLen;
		public double CenterLat;
		public double CenterLon;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ReportHeader
	{
		public string Procedure;
		public string Category;
		//public string RWY;
		public string Database;
		public string Aerodrome;
		//public DateTime EffectiveDate;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct TypeConvert
	{
		public double Multiplier;
		public double Rounding;
		public string Unit;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct SquareSolutionArea
	{
		public int Solutions;
		public double First;
		public double Second_Renamed;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct VORData
	{
		public double Range;
		public double FA_Range;
		public double InitWidth;
		public double SplayAngle;
		public double TrackingTolerance;
		public double IntersectingTolerance;
		public double ConeAngle;
		public double TrackAccuracy;
		public double LateralDeviationCoef;
		public double EnRouteTrackingToler;
		public double EnRouteTracking2Toler;
		public double EnRouteInterToler;
		public double EnRoutePrimAreaWith;
		public double EnRouteSecAreaWith;
		public double OnNAVRadius;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct NDBData
	{
		public double Range;
		public double FA_Range;
		public double InitWidth;
		public double SplayAngle;
		public double TrackingTolerance;
		public double IntersectingTolerance;
		public double ConeAngle;
		public double TrackAccuracy;
		public double Entry2ConeAccuracy;
		public double LateralDeviationCoef;
		public double EnRouteTrackingToler;
		public double EnRouteTracking2Toler;
		public double EnRouteInterToler;
		public double EnRoutePrimAreaWith;
		public double EnRouteSecAreaWith;
		public double OnNAVRadius;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct DMEData
	{
		public double Range;
		public double MinimalError;
		public double ErrorScalingUp;
		public double SlantAngle;
		public double TP_div;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct LLZData
	{
		public double Range;
		public double TrackingTolerance;
		public double IntersectingTolerance;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct CLPoint
	{
		//public RunwayCentrelinePoint pCLPoint;
		public Point pPtGeo;
		public Point pPtPrj;
	}
}
