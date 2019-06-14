using System;
//using System.Collections;
//using System.Diagnostics;

//using System.Windows;
//using System.Windows.Forms;

using Aran.Aim.Features;
using Aran.Queries;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Geometries.SpatialReferences;

using Aran.Panda.Common;

namespace Aran.Panda.EnrouteStar
{
	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct ADHPType
	//{
	//    public Point pPtGeo;
	//    public Point pPtPrj;
	//    public AirportHeliport pAirportHeliport;
	//    public string Name;
	//    public double MagVar;
	//    public double Elev;
	//    public double WindSpeed;
	//    public double ISAtC;
	//    public double TransitionLevel;
	//    public Guid Identifier;
	//    public Guid OrgID;
	//    public long index;
	//}

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public enum eRWY
	//{
	//    PtStart = 0,
	//    PtTHR = 1,
	//    PtEnd = 2,
	//}

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct RWYType
	//{
	//    public EnumArray<Point, eRWY> pPtGeo;
	//    public EnumArray<Point, eRWY> pPtPrj;
	//    public EnumArray<SignificantPoint, eRWY> pSignificantPoint;
	//    //public EnumArray<string, eRWY> ptID;
	//    public RunwayDirection pRunwayDir;

	//    public string Name;
	//    public string PairName;
	//    public double TrueBearing;
	//    public double Length;
	//    public double ClearWay;
	//    public Guid Identifier;
	//    public Guid PairID;
	//    public Guid ADHP_ID;
	//    //public Guid ILSID;
	//    public long index;
	//    public bool Selected;

	//    public void Initialize()
	//    {
	//        pPtGeo = new EnumArray<Point, eRWY>();
	//        pPtPrj = new EnumArray<Point, eRWY>();
	//        pSignificantPoint = new EnumArray<SignificantPoint, eRWY>();
	//        //ptID = new EnumArray<string, eRWY>();
	//    }
	//}

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

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct NavaidType
	//{
	//    public Point pPtGeo;
	//    public Point pPtPrj;
	//    public Feature pFeature;
	//    public Guid Identifier;
	//    public string Name;
	//    public string CallSign;
	//    public double MagVar;

	//    public eNavaidType TypeCode;
	//    public double Range;
	//    public long PairNavaidIndex;
	//    public eNavaidType PairNavaidType;
	//    public long index;
	//    public double Course;
	//    public double GPAngle;
	//    public double GP_RDH;
	//    public double LLZ_THR;
	//    public double SecWidth;
	//    public double AngleWidth;
	//    public long Tag;

	//    public SignificantPoint GetSignificantPoint()
	//    {
	//        SignificantPoint sp = new SignificantPoint();
	//        sp.NavaidSystem = pFeature.GetFeatureRef();
	//        return sp;
	//    }
	//}

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct FIXableNavaidType
	//{
	//    public Point pPtGeo;
	//    public Point pPtPrj;
	//    public Feature pFeature;
	//    public Guid Identifier;
	//    public string Name;
	//    public string CallSign;
	//    public double MagVar;
	//    //public string TypeName_Renamed;
	//    public eNavaidType TypeCode;
	//    public double Range;
	//    public long PairNavaidIndex;
	//    public eNavaidType PairNavaidType;
	//    public long index;
	//    public bool Front;
	//    public double Dist;
	//    public double CLShift;
	//    public long ValCnt;
	//    public double[] ValMin;
	//    public double[] ValMax;
	//    public long Tag;

	//    public SignificantPoint GetSignificantPoint()
	//    {
	//        SignificantPoint sp = new SignificantPoint();
	//        sp.NavaidSystem = pFeature.GetFeatureRef();
	//        return sp;
	//    }
	//}

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct WPT_FIXType
	//{
	//    public Point pPtGeo;
	//    public Point pPtPrj;
	//    public Feature pFeature;
	//    public Guid Identifier;
	//    public string Name;
	//    public string CallSign;
	//    public double MagVar;

	//    public eNavaidType TypeCode;
	//    public long Tag;
	//}

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct ObstacleHd
	//{
	//    public Point pPtGeo;
	//    public Point pPtPrj;
	//    public string Name;
	//    public long ID;
	//    public Guid Identifier;
	//    public long index;
	//    public double Height;
	//    public double Dist;
	//    public double DistStar;
	//    public double PDG;
	//    public double dPDG;
	//    public double MOC;
	//    public double PDGToTop;
	//    public double hPent;
	//    public double ReqTNH;
	//    public double CLShift;
	//    public double TrackAdjust;
	//    public double TrackAdjustPDG;
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

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct ProhibitedSector
	//{
	//    public double LeftAngle;
	//    public double RightAngle;
	//    public ObstacleHd Obstacle;
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

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct TraceSegment
	//{
	//    public long SegmentType;
	//    public double HStart;
	//    public double HFinish;
	//    public double H1;
	//    public double H2;
	//    public double Length;
	//    public Point PtIn;
	//    public Point ptOut;
	//    public double DirIn;
	//    public double DirBetween;
	//    public double DirOut;
	//    public MultiPoint PathPrj;
	//    public string StCoords;
	//    public string FinCoords;
	//    public string Center1Coords;
	//    public string Center2Coords;
	//    public string St1Coords;
	//    public string Fin1Coords;
	//    public string St2Coords;
	//    public string Fin2Coords;
	//    public double TurnR;
	//    public double Turn1Dir;
	//    public double Turn2Dir;
	//    public double Turn1Angle;
	//    public double Turn2Angle;
	//    public double Turn1Length;
	//    public double Turn2Length;
	//    public double BetweenTurns;
	//    public string Comment;
	//    public string RepComment;
	//}

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct DepartPoint
	//{
	//    public string Description;
	//    public string Lat;
	//    public string Lon;
	//    public string Direction;
	//    public string PDG;
	//    public string Height;
	//    public string DistToNext;
	//    public long Raidus;
	//    public long Turn;
	//    public string CenterLat;
	//    public string CenterLon;
	//    public string turnAngle;
	//    public string TurnArcLen;
	//}

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct DepartReport
	//{
	//    public string OmniVPP;
	//    public string OmniIKVPP;
	//    public string OmniMKVPP;
	//    public string OmniVPPLen;
	//    public string OmniR;
	//    public string OmniTMA;
	//    public string OmniMinPdgObs;
	//    public string OmniNomPdgObs;
	//    public string OmniMinTurnObs;
	//    public string OmniZNRPdg;
	//    public string OmniZRPdg;
	//    public string OmniTNAH;
	//    public string OmniZNRLen;
	//    public string GuidVPP;
	//    public string GuidIKVPP;
	//    public string GuidMKVPP;
	//    public string GuidVPPLen;
	//    public string GuidR;
	//    public string GuidZone1Variant;
	//    public string GuidRNS;
	//    public string GuidRadial;
	//    public string GuidPdg;
	//    public string GuidReturnH;
	//    public string GuidStartGuid;
	//    public string GuidTrajDisp;
	//    public string GuidMKFly;
	//    public string GuidFlyDisp;
	//    public string GuidZNRPdg;
	//    public string GuidMaxPdgObs;
	//    public string GuidSchemePdgObs;
	//    public string GuidTurnWPTType;
	//    public DepartPoint[] GuidPoints;
	//    public string GuidAllLen;
	//    public string RoutsVPP;
	//    public string RoutsIKVPP;
	//    public string RoutsMKVPP;
	//    public string RoutsVPPLen;
	//    public string RoutsR;
	//    public string RoutsZone1Variant;
	//    public string RoutsPdg;
	//    public string RoutsReturnH;
	//    public string RoutsMKFly;
	//    public string RoutsFlyDisp;
	//    public string RoutsMaxPdgObs;
	//    public string RoutsSchemePdgObs;
	//    public string RoutsTurnWPTType;
	//    public DepartPoint[] RoutsPoints;
	//    public string RoutsAllLen;
	//    public string TracePdg;
	//    public string TraceIAS;
	//    public string TraceCategory;
	//}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ReportType
	{
		public string Procedure;
		public string Category;
		public string RWY;
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

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct TypeConvert
	//{
	//    public double Multiplier;
	//    public double Rounding;
	//    public string Unit;
	//}

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct SquareSolutionArea
	//{
	//    public int Solutions;
	//    public double First;
	//    public double Second;
	//}

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct VORData
	//{
	//    public double Range;
	//    public double FA_Range;
	//    public double InitWidth;
	//    public double SplayAngle;
	//    public double TrackingTolerance;
	//    public double IntersectingTolerance;
	//    public double ConeAngle;
	//    public double TrackAccuracy;
	//    public double LateralDeviationCoef;
	//    public double EnRouteTrackingToler;
	//    public double EnRouteTracking2Toler;
	//    public double EnRouteInterToler;
	//    public double EnRoutePrimAreaWith;
	//    public double EnRouteSecAreaWith;
	//    public double OnNAVRadius;
	//}

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct NDBData
	//{
	//    public double Range;
	//    public double FA_Range;
	//    public double InitWidth;
	//    public double SplayAngle;
	//    public double TrackingTolerance;
	//    public double IntersectingTolerance;
	//    public double ConeAngle;
	//    public double TrackAccuracy;
	//    public double Entry2ConeAccuracy;
	//    public double LateralDeviationCoef;
	//    public double EnRouteTrackingToler;
	//    public double EnRouteTracking2Toler;
	//    public double EnRouteInterToler;
	//    public double EnRoutePrimAreaWith;
	//    public double EnRouteSecAreaWith;
	//    public double OnNAVRadius;
	//}

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct DMEData
	//{
	//    public double Range;
	//    public double MinimalError;
	//    public double ErrorScalingUp;
	//    public double SlantAngle;
	//    public double TP_div;
	//}

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct LLZData
	//{
	//    public double Range;
	//    public double TrackingTolerance;
	//    public double IntersectingTolerance;
	//}

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct CLPoint
	//{
	//    public RunwayCentrelinePoint pCLPoint;
	//    public Point pPtGeo;
	//    public Point pPtPrj;
	//}

	[System.Runtime.InteropServices.ComVisible(false)]
	public class CBWPT_FixItem
	{
		public CBWPT_FixItem(WPT_FIXType wptFix)
		{
			WPT_FIX = wptFix;
		}

		public override string ToString()
		{
			return WPT_FIX.CallSign;
		}

		public WPT_FIXType WPT_FIX { get; private set; }
	}
}
