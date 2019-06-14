using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using AIXM.Features;
using ESRI.ArcGIS.Carto;

namespace EOSID
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public struct TypeConvert
	{
		public double Multiplier;
		public double Rounding;
		public string Unit;
	}

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
	public class SideDirection
	{
		public const int
			rightSide = -1,
			onSide = 0,
			leftSide = 1;
	}

	//public enum SideDirection
	//{
	//    sideRight = -1,
	//    sideOn = 0,
	//    sideLeft = 1
	//}

	[System.Runtime.InteropServices.ComVisible(false)]
	public class TurnDirection
	{
		public const int
		CW = -1,
		NONE = 0,
		CCW = 1,
		CoLinear = 2;
	}

	//public enum TurnDirection
	//{
	//    CW = -1,
	//    NONE = 0,
	//    CCW = 1,
	//    CoLinear = 2
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

	[System.Runtime.InteropServices.ComVisible(false)]
	public class Win32Window : System.Windows.Forms.IWin32Window
	{
		private IntPtr _handle;

		public Win32Window(Int32 handle)
		{
			_handle = new IntPtr(handle);
		}

		public IntPtr Handle
		{
			get { return _handle; }
		}
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
	public struct ADHPData
	{
		public IPoint pPtGeo;
		public IPoint pPtPrj;
		public string Name;
		public string ID;
		public string OrgID;
		public double MagVar;
		public double Elev;
		public double WindSpeed;
		public double ISAtC;
		public double TransitionLevel;
		public int index;

		public override string ToString()
		{
			return Name;
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public enum eRWY
	{
		PtStart = 0,
		PtTHR = 1,
		PtDER = 2,
		PtEnd = 3
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct RWYData
	{
		public EnumArray<IPoint, eRWY> pPtGeo;
		public EnumArray<IPoint, eRWY> pPtPrj;
		public EnumArray<string, eRWY> ptID;
		public IRunwayDirection pRunwayDir;
		public string Name;
		public string ID;

		public string ADHP_ID;
		public string ILSID;
		public double TrueBearing;
		public double Length;
		public double TORA;
		public double TODA;
		public double ASDA;
		public bool Selected;
		public int index;

		public void Initialize()
		{
			pPtGeo = new EnumArray<IPoint, eRWY>();
			pPtPrj = new EnumArray<IPoint, eRWY>();
			ptID = new EnumArray<string, eRWY>();
		}
		public override string ToString()
		{
			return Name;
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct WPT_FIXData
	{
		public IPoint pPtGeo;
		public IPoint pPtPrj;
		//public Feature pFeature;
		public string Name;
		public string ID;

		public eNavaidClass TypeCode;
		public double MagVar;
		public long Tag;

		public override string ToString()
		{
			return Name;
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ILSData
	{
		public IPoint pPtGeo;
		public IPoint pPtPrj;
		//public Feature pFeature;
		public string RWY_ID;
		public string ID;
		public string CallSign;
		public int Category;
		public double GPAngle;
		public double GP_RDH;
		public double MagVar;
		public double Course;
		public double LLZ_THR;
		public double AngleWidth;
		public double SecWidth;
		public int index;
		public long Tag;

		public override string ToString()
		{
			return CallSign;
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct NavaidData
	{
		public IPoint pPtGeo;
		public IPoint pPtPrj;
		//public Feature pFeature;
		public string Name;
		public string ID;
		public string CallSign;
		public double MagVar;

		public eNavaidClass TypeCode;
		public double Range;
		public long PairNavaidIndex;
		public long index;

		public double Course;
		public double GP_RDH;
		public double LLZ_THR;
		public double Sec_Width;

		public long ValCnt;
		public double[] ValMin;
		public double[] ValMax;

		public long Tag;
		public override string ToString()
		{
			return CallSign;
			//if (CallSign == "")
			//    return Name;
			//else
			//    return CallSign;
		}

		//public SignificantPoint GetSignificantPoint()
		//{
		//    SignificantPoint sp = new SignificantPoint();
		//    sp.NavaidSystem = pFeature.GetFeatureRef();
		//    return sp;
		//}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ObstacleData
	{
		public IPoint pPtGeo;
		public IPoint pPtPrj;

		public string Name;
		public string ID;

		public double TotalDist;
		public double X;
		public double Y;
		public double Height;	//Z

		public double MOC;
		public double MOCH;

		public double PDG;
		public double ReqNetGradient;
		public double ApplNetGradient2;
		public double ApplNetGradient4;
		public double AcceleStartDist;
		public double AcceleEndDist;
		public double ActualHeight;
		public int Phase;

		public double HorAccuracy;
		public double VertAccuracy;

		public double fTmp;
		public double fSort;
		public string sSort;
		public int index;

		public override string ToString()
		{
			return Name;
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct CLPoint
	{
		public IRunwayCentrelinePoint pCLPoint;
		public IPoint pPtGeo;
		public IPoint pPtPrj;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct Interval
	{
		public double Left, Right;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public enum eLegType
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
	public struct PointData
	{
		public IPoint pPoint;

		public double Direction;
		public double Width;

		public double NetHeight;
		public double GrossHeight;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct LegPoint
	{
		public PointData BestCase;
		public PointData TraceCase;
		public PointData WorstCase;
		public Boolean atHeight;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct TurnData
	{
		public IPoint ptCenter;
		public IPoint ptStart;
		public IPoint ptEnd;

		public int TurnDir;
		public double Radius;
		public double Angle;
		public double StartDist;
		public double Length;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct LegData
	{
		public double Length;
		public double PrevTotalLength;
		public double FlightTime;
		public double PrevTotalFlightTime;
		public double NetGrd;

		//=========================
		public int turns;
		public TurnData[] Turn;
		public double DRLength;
		public double DRDirection;
		//=========================
		public IPolyline pNominalPoly;
		public IElement pNominalElement;

		public void Initialize()
		{
			Turn = new TurnData[3];
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct TrackLeg
	{
		public LegPoint ptStart;
		public LegPoint ptEnd;
		public double BankAngle;
		public double PlannedEndWidth;
		public double OutMagVar;
		public double AcceleDist;
		public int AcceleLeg;
		public eLegType SegmentCode;
		public AIXM.DataTypes.Enums.SegmentPathType PathCode;

		public bool bOnWPTFIX;
		public WPT_FIXData WptFix;
		public NavaidData GuidanceNav;
		public NavaidData InterceptionNav;

		//=========================
		public ObstacleData[] ObstacleList;
		public int ObsMaxNetGrd;
		public int ObsMaxAcceleDist;
		//=========================
		public IPolygon pProtectionArea;
		public IElement pProtectionElement;
		//=======================================================
		//bool Guidance;
		//=========================
		public LegData BestCase;
		public LegData TraceCase;
		public LegData WorstCase;

		public string Comment;
		//=======================================================
		public Object Tag;

		public void Initialize()
		{
			BestCase.Initialize();
			WorstCase.Initialize();
		}
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
		public double Raidus;
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
		public string RWY;
		public string Database;
		public string Aerodrome;
		public string EffectiveDate;
	}

}

