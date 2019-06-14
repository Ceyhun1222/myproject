using System;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Queries;
using ESRI.ArcGIS.Geometry;

namespace Aran.PANDA.Departure
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public enum eRoundMode
	{
		NONE = 0,
		FLOOR = 1,
		NEAREST = 2,
		CEIL = 3,
		SPECIAL = 4
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
		PtDER = 2,
		//PtEnd = 3,
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
	};

	[System.Runtime.InteropServices.ComVisible(false)]
	public enum eIntersectionType
	{
		ByDistance = 1,
		ByAngle = 2,
		OnNavaid = 3,
		RadarFIX = 4
	};

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ADHPType
	{
		public IPoint pPtGeo;
		public IPoint pPtPrj;
		//public AirportHeliport pAirportHeliport;
		public string Name;
		public double MagVar;
		public double Elev;
		public double WindSpeed;
		public double ISAtC;
		public double TransitionLevel;
		public Guid Identifier;
		public Guid OrgID;
		public long index;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct RWYType
	{
		public EnumArray<IPoint, eRWY> pPtGeo;
		public EnumArray<IPoint, eRWY> pPtPrj;
		public EnumArray<Guid, eRWY> pSignificantPointID;		//public EnumArray<double, eRWY> HorAccuracy;
		public double StartHorAccuracy;

		public string Name;
		public string PairName;
		public double TrueBearing;
		//public double MagneticBearing;
		public double Length;

		public double ClearWay;
		public double TODA;
		public double TODAAccuracy;
		//public double ASDA;

		public Guid Identifier;
		public Guid PairID;
		public Guid ADHP_ID;

		public long index;
		public bool Selected;

		public FeatureRefObject GetFeatureRefObject()
		{
			FeatureRefObject fro = new FeatureRefObject();
			fro.Feature = new FeatureRef(Identifier);				//'Feature.GetFeatureRef()
			return fro;
		}

		public void Initialize()
		{
			pPtGeo = new EnumArray<IPoint, eRWY>();
			pPtPrj = new EnumArray<IPoint, eRWY>();
			pSignificantPointID = new EnumArray<Guid, eRWY>();
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ILSType
	{
		public IPoint pPtGeo;
		public IPoint pPtPrj;

		public Guid RWY_ID;
		public Guid NAV_Ident;
		public Guid Identifier;

		public string CallSign;
		public double MagVar;
		public double Course;
		public double HorAccuracy;

		public double GPAngle;
		public double GP_RDH;

		public double LLZ_THR;
		public double AngleWidth;
		public double SecWidth;
		public long Category;
		public long index;
		public long Tag;

		public FeatureRef GetFeatureRef()
		{
			return new FeatureRef(NAV_Ident);
		}

		public SignificantPoint GetSignificantPoint()
		{
			SignificantPoint sp = new SignificantPoint();
			sp.NavaidSystem = new FeatureRef(NAV_Ident);
			return sp;
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct NavaidType
	{
		public IPoint pPtGeo;
		public IPoint pPtPrj;

		public Guid NAV_Ident;
		public Guid Identifier;

		public string Name;
		public string CallSign;

		public eNavaidType TypeCode;
		public eNavaidType PairNavaidType;

		public long index;
		public long PairNavaidIndex;

		public double MagVar;
		public double Range;
		public double Course;
		public double GPAngle;
		public double GP_RDH;
		public double LLZ_THR;
		public double SecWidth;
		public double AngleWidth;
		public double HorAccuracy;
		public double Disp;

		public double Dist;
		public double CLShift;

		public bool Front;
		public eIntersectionType IntersectionType;

		public long ValCnt;
		public double[] ValMin;
		public double[] ValMax;

		public long Tag;

		public FeatureRef GetFeatureRef()
		{
			if (TypeCode > eNavaidType.NONE && TypeCode <= eNavaidType.TACAN)
				return new FeatureRef(NAV_Ident);

			return new FeatureRef(Identifier);
		}

		public SignificantPoint GetSignificantPoint()
		{
			SignificantPoint sp = new SignificantPoint();

			if (TypeCode > eNavaidType.NONE && TypeCode <= eNavaidType.TACAN)
				sp.NavaidSystem = new FeatureRef(NAV_Ident);
			else
				sp.FixDesignatedPoint = new FeatureRef(Identifier);

			return sp;
		}

		override public string ToString()
		{
			if (!string.IsNullOrWhiteSpace(CallSign))
				return CallSign;

			if (!string.IsNullOrWhiteSpace(Name))
				return Name;

			return string.Empty;
		}
	}

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct NavaidType
	//{
	//	public IPoint pPtGeo;
	//	public IPoint pPtPrj;

	//	public Guid NAV_Ident;
	//	public Guid Identifier;
	//	public string Name;
	//	public string CallSign;
	//	public double MagVar;
	//	public eNavaidType TypeCode;
	//	public double Range;
	//	public long PairNavaidIndex;
	//	public eNavaidType PairNavaidType;
	//	public long index;
	//	public bool Front;
	//	public double Dist;
	//	public double CLShift;
	//	public long ValCnt;
	//	public double[] ValMin;
	//	public double[] ValMax;
	//	public long Tag;

	//	public SignificantPoint GetSignificantPoint()
	//	{
	//		SignificantPoint sp = new SignificantPoint();
	//		if (TypeCode > eNavaidType.NONE)
	//			sp.NavaidSystem = new FeatureRef(NAV_Ident);
	//		else
	//			sp.FixDesignatedPoint = new FeatureRef(Identifier);

	//		return sp;
	//	}

	//	override public string ToString()
	//	{
	//		if (CallSign != null && CallSign != "")
	//			return CallSign;
	//		if (Name != null && Name != "")
	//			return Name;
	//		return "";
	//	}
	//}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct WPT_FIXType
	{
		public IPoint pPtGeo;
		public IPoint pPtPrj;

		public Guid NAV_Ident;
		public Guid Identifier;

		public string Name;
		public string CallSign;
		public double MagVar;
		public double HorAccuracy;

		public eNavaidType TypeCode;
		public long Tag;

		public FeatureRef GetFeatureRef()
		{
			if (TypeCode > eNavaidType.NONE)
				return new FeatureRef(NAV_Ident);

			return new FeatureRef(Identifier);
		}

		public SignificantPoint GetSignificantPoint()
		{
			SignificantPoint sp = new SignificantPoint();

			if (TypeCode > eNavaidType.NONE)
				sp.NavaidSystem = new FeatureRef(NAV_Ident);
			else
				sp.FixDesignatedPoint = new FeatureRef(Identifier);

			return sp;
		}

		public Feature GetFeature()
		{
			Feature result;

			if (TypeCode <= eNavaidType.NONE)
			{
				result = new DesignatedPoint();
				result.Identifier = Identifier;
			}
			else
			{
				result = new Navaid();
				result.Identifier = NAV_Ident;
			}

			return result;
		}

		override public string ToString()
		{
			if (!string.IsNullOrWhiteSpace(CallSign))
				return CallSign;

			if (!string.IsNullOrWhiteSpace(Name))
				return Name;

			return string.Empty;
		}
	}

	//==========================================================================================
	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ObstacleData
	{
		public ESRI.ArcGIS.Geometry.IPoint pPtGeo;
		public ESRI.ArcGIS.Geometry.IPoint pPtPrj;
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
		public ESRI.ArcGIS.Geometry.IGeometry pGeomGeo;
		public ESRI.ArcGIS.Geometry.IGeometry pGeomPrj;

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
		public FeatureRef GetFeatureRef()
		{
			return new FeatureRef(Identifier);
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ObstacleContainer
	{
		public Obstacle[] Obstacles;
		public ObstacleData[] Parts;
		//public int SortOrder;

		//void AddPart(ref int Last, int Dest, ObstacleContainer from, ObstacleData Src)
		//{
		//	Parts[Dest] = Src;

		//	if (from.Obstacles[Src.Owner].NIx < 0)
		//	{
		//		Last += 1;
		//		Obstacles[Last] = from.Obstacles[Src.Owner];
		//		Obstacles[Last].PartsNum = 0;
		//		System.Array.Resize<int>(ref Obstacles[Last].Parts, from.Obstacles[Src.Owner].PartsNum - 1);
		//		from.Obstacles[Src.Owner].NIx = Last;
		//	}

		//	Parts[Dest].Owner = from.Obstacles[Src.Owner].NIx;
		//	Parts[Dest].Index = Obstacles[Parts[Dest].Owner].PartsNum;
		//	Obstacles[Parts[Dest].Owner].Parts[Obstacles[Parts[Dest].Owner].PartsNum] = Dest;
		//	Obstacles[Parts[Dest].Owner].PartsNum += 1;
		//}

		//void MovePart(int Dest, int Src)
		//{
		//	int L, K;
		//	L = Parts[Dest].Owner;
		//	K = Parts[Dest].Index;

		//	Obstacles[L].PartsNum -= 1;
		//	Obstacles[L].Parts[K] = Obstacles[L].Parts[Obstacles[L].PartsNum];
		//	Parts[Obstacles[L].Parts[K]].Index = K;

		//	Parts[Dest] = Parts[Src];
		//	Obstacles[Parts[Dest].Owner].Parts[Parts[Dest].Index] = Dest;
		//	//N -= 1;
		//}

		//void MoveObst(int Dest, int Src)
		//{
		//}
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
	//public struct ObstacleTA
	//{
	//    public string Name;
	//    public long ID;
	//    public long index;
	//    public double Height;
	//    public double Dist;
	//    public double DistStar;
	//    public double PDG;
	//    public double dPDG;
	//    public double MOC;
	//    public double PDGToTop;
	//    public double CLShift;
	//    public double TrackAdjust;
	//    public double TrackAdjustPDG;
	//    public bool Prima;
	//}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ProhibitedSector
	{
		public double LeftAngle;
		public double RightAngle;
		public ObstacleData Obstacle;
		public IPointCollection SectorGraphics;
		public int SectorColor;
	}

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

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct TraceSegment
	{
		public IPoint ptIn;			//pPtStart;
		public IPoint ptOut;		//pPtEnd;
		public IPolyline PathPrj;	//pNominalPoly;
		public IPolygon pProtectArea;

		public eSegmentType SegmentCode;
		public CodeSegmentPath LegType;

		public NavaidType GuidanceNav;
		public NavaidType InterceptionNav;
		public double SeminWidth;

		public double HStart;
		public double HFinish;
		public double H1, H2;

		public double Length;
		public double PDG;
		public double BankAngle;
		public double DirIn;
		public double DirOut;

		//=========================
		public double DirBetween;
		public string StCoords;
		public string FinCoords;
		//=========================
		public IPoint PtCenter1;
		public string Center1Coords;
		public int Turn1Dir;
		public double Turn1R;
		public double Turn1Angle;
		public double Turn1Length;
		public string St1Coords;
		public string Fin1Coords;
		//=========================
		public IPoint PtCenter2;
		public string Center2Coords;
		public int Turn2Dir;
		public double Turn2R;
		public double Turn2Angle;
		public double Turn2Length;
		public string St2Coords;
		public string Fin2Coords;
		//=========================
		public double BetweenTurns;
		public string Comment;
		public string RepComment;
	}

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

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct LayerInfo
	//{
	//    public bool Initialised;
	//    public long WorkspaceType;
	//    public string Source;
	//    public string LayerName;
	//    public DateTime FileDate;
	//}

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
		public RunwayCentrelinePoint pCLPoint;
		public IPoint pPtGeo;
		public IPoint pPtPrj;
	}
}
