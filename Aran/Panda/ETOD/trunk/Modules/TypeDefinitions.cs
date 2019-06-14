using System;
using System.Collections.Generic;

using ESRI.ArcGIS.Geometry;

using Aran.Aim.Features;
using Aran.Aim.Enums;
//using Aran.Queries;

namespace ETOD
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public struct OrganisationType
	{
		public System.Guid Identifier;
		public string Name;
		public long DbID;
		public long Index;
		public OrganisationAuthority pOrganisationAuthority;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ADHPType
	{
		public IPoint pPtGeo;
		public IPoint pPtPrj;
		public string Name;
		public System.Guid ID;
		public System.Guid OrgID;

		public double MagVar;
		public double Elev;
		public double WindSpeed;
		public double ISAtC;
		public double TransitionLevel;
		public int Index;
		public AirportHeliport pAirportHeliport;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public enum eRWY
	{
		PtStart = 0,
		PtTHR = 1,
		PtEnd = 2,
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct RWYCLPoint
	{
		public RunwayCentrelinePoint pCLPoint;
		public IPoint pPtGeo;
		public IPoint pPtPrj;
		public double fTmp;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct RWYType
	{
		public EnumArray<IPoint, eRWY> pPtGeo;
		public EnumArray<IPoint, eRWY> pPtPrj;
		//public EnumArray<string, eRWY> ptID;
		public EnumArray<RunwayCentrelinePoint, eRWY> pSignificantPoint;
		public RunwayDirection pRunwayDir;
		public RWYCLPoint[] CLPointArray;

		public string Name;
		public string PairName;
		public int NameID;

		public double TrueBearing;
		public double Length;
		public double ClearWay;

		public System.Guid ID;
		public System.Guid PairID;
		public System.Guid ADHP_ID;

		public int ILSID;
		public int index;
		public bool Selected;

		public void Initialize()
		{
			pPtGeo = new EnumArray<IPoint, eRWY>();
			pPtPrj = new EnumArray<IPoint, eRWY>();
			//ptID = new EnumArray<string, eRWY>();
			pSignificantPoint = new EnumArray<RunwayCentrelinePoint, eRWY>();
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct WPT_FIXType
	{
		public IPoint pPtGeo;
		public IPoint pPtPrj;
		public Feature pSignificantPoint;
		public string Name;
		public System.Guid ID;

		public string TypeName_Renamed;
		public eNavaidClass TypeCode;
		public double MagVar;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ILSType
	{
		public IPoint pPtGeo;
		public IPoint pPtPrj;
		public Feature pSignificantPoint;
		public System.Guid RWY_ID;
		public System.Guid ID;
		public string CallSign;
		public int Category;
		public double GPAngle;
		public double GP_RDH;
		public double MagVar;
		public double Course;
		public double LLZ_THR;
		public double AngleWidth;
		public double SecWidth;
		public int Index;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct NavaidType
	{
		public IPoint pPtGeo;
		public IPoint pPtPrj;
		public Feature pSignificantPoint;
		public string Name;
		public System.Guid ID;
		public string CallSign;
		public double MagVar;
		public string TypeName_Renamed;
		public eNavaidClass TypeCode;
		public double Range;
		public int PairNavaidIndex;
		public int index;
		public double Course;
		public double GPAngle;
		public double GP_RDH;
		public double LLZ_THR;
		public double SecWidth;
		public double AngleWidth;
		public int Tag;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct FIXableNavaidType
	{
		public IPoint pPtGeo;
		public IPoint pPtPrj;
		//public Delib.Classes.Features.Navaid.ISignificantPoint pSignificantPoint;
		public string Name;
		public int ID;
		public string CallSign;
		public double MagVar;
		public string TypeName_Renamed;
		public eNavaidClass TypeCode;
		public double Range;
		public int index;
		public int PairNavaidIndex;
		public string PairNavaidType;
		public bool Front;
		public double Dist;
		public double CLShift;
		public int ValCnt;
		public double[] ValMin;
		public double[] ValMax;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ObstacleType
	{
		public IGeometry pGeoGeo;
		public IGeometry pGeoPrj;
		public string Name;
		public string PartName;

		public long ID;
		public System.Guid Identifier;
		public double Height;
		public double Elevation;
		public double HorAccuracy;
		public double VertAccuracy;

		public double Hsurface;
		public double hPent;
		public List<ObstaclePlaneReleation> Releation;
		public ObstacleArea2 obstacleArea2;

		//public D3DPlane LocalPlane;
		//public double Hsurface;
		//public double hPent;
		//public ObstacleArea2 obstacleArea2;

		public CodeObstacleArea codeObstacleArea;
		public bool Ignored;

		public double fTmp;
		public double fSort;
		public string sSort;
		public object Tag;
		public int Group;
		public int index;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ObstacleTA
	{
		public string Name;
		public string ID;
		public int index;
		public double Height;
		public double Dist;
		public double DistStar;
		public double PDG;
		public double dPDG;
		public double MOC;
		public double PDGToTop;
		public double CLShift;
		public double TrackAdjust;
		public double TrackAdjustPDG;
		public bool Prima;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ProhibitedSector
	{
		public double LeftAngle;
		public double RightAngle;
		public ObstacleType Obstacle;
		public IPointCollection SectorGraphics;
		public int SectorColor;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct PDGLevel
	{
		public double PDG;
		public double Range;
		public int AdjustANgle;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct Interval
	{
		public double Left_Renamed;
		public double Right_Renamed;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct TraceSegment
	{
		public int SegmentType;
		public double HStart;
		public double HFinish;
		public double H1;
		public double H2;
		public double Length;
		public IPoint PtIn;
		public IPoint ptOut;
		public double DirIn;
		public double DirBetween;
		public double DirOut;
		public IPointCollection PathPrj;
		public string StCoords;
		public string FinCoords;
		public string Center1Coords;
		public string Center2Coords;
		public string St1Coords;
		public string Fin1Coords;
		public string St2Coords;
		public string Fin2Coords;
		public double TurnR;
		public double Turn1Dir;
		public double Turn2Dir;
		public double Turn1Angle;
		public double Turn2Angle;
		public double Turn1Length;
		public double Turn2Length;
		public double BetweenTurns;
		public string Comment;
		public string RepComment;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct DepartPoint
	{
		public string Description;
		public string Lat;
		public string Lon;
		public string Direction;
		public string PDG;
		public string Height;
		public string DistToNext;
		public int Raidus;
		public int Turn;
		public string CenterLat;
		public string CenterLon;
		public string turnAngle;
		public string TurnArcLen;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct DepartReport
	{
		public string OmniVPP;
		public string OmniIKVPP;
		public string OmniMKVPP;
		public string OmniVPPLen;
		public string OmniR;
		public string OmniTMA;
		public string OmniMinPdgObs;
		public string OmniNomPdgObs;
		public string OmniMinTurnObs;
		public string OmniZNRPdg;
		public string OmniZRPdg;
		public string OmniTNAH;
		public string OmniZNRLen;
		public string GuidVPP;
		public string GuidIKVPP;
		public string GuidMKVPP;
		public string GuidVPPLen;
		public string GuidR;
		public string GuidZone1Variant;
		public string GuidRNS;
		public string GuidRadial;
		public string GuidPdg;
		public string GuidReturnH;
		public string GuidStartGuid;
		public string GuidTrajDisp;
		public string GuidMKFly;
		public string GuidFlyDisp;
		public string GuidZNRPdg;
		public string GuidMaxPdgObs;
		public string GuidSchemePdgObs;
		public string GuidTurnWPTType;
		public DepartPoint[] GuidPoints;
		public string GuidAllLen;
		public string RoutsVPP;
		public string RoutsIKVPP;
		public string RoutsMKVPP;
		public string RoutsVPPLen;
		public string RoutsR;
		public string RoutsZone1Variant;
		public string RoutsPdg;
		public string RoutsReturnH;
		public string RoutsMKFly;
		public string RoutsFlyDisp;
		public string RoutsMaxPdgObs;
		public string RoutsSchemePdgObs;
		public string RoutsTurnWPTType;
		public DepartPoint[] RoutsPoints;
		public string RoutsAllLen;
		public string TracePdg;
		public string TraceIAS;
		public string TraceCategory;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ReportType
	{
		public string Procedure;
		public string Category;
		public string RWY;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct LayerInfo
	{
		public bool Initialised;
		public int WorkspaceType;
		public string Source;
		public string LayerName;
		public DateTime FileDate;
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
	public enum eNavaidClass
	{
		CodeNONE = -1,
		CodeVOR = 0,
		CodeDME = 1,
		CodeNDB = 2,
		CodeLLZ = 3,
		CodeTACAN = 4,
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
	[Flags]
	public enum ObstacleArea2
	{
		NotArea2 = 0,
		Area2A = 1,
		Area2B = 2,
		Area2C = 4,
		Area2D = 8,
	}
}
