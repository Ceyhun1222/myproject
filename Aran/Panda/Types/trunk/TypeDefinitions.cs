using System;
using System.Collections.Generic;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Geometries;
using Aran.Queries;

namespace Aran.PANDA.Common
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ADHPType
	{
		public Point pPtGeo;
		public Point pPtPrj;
		public AirportHeliport pAirportHeliport;
		public List<FeatureRefObject> AltimeterSource;

		public string Name;
		public double MagVar;
		public double Elev;
		public double WindSpeed;
		public double ISAtC;
		public double LowestTemperature;
		//public double MaximumTemperature;
		public double TransitionLevel;
		public Guid Identifier;
		public Guid OrgID;
		public long index;

		public override string ToString()
		{
			if (Name != null)
				return Name;

			return "";
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct RWYType
	{
		public EnumArray<Point, eRWY> pPtGeo;
		public EnumArray<Point, eRWY> pPtPrj;
		public EnumArray<Guid, eRWY> clptIdent;
		public double StartHorAccuracy;
		public double DERHorAccuracy;

		public string Name;
		public string PairName;
		public double TrueBearing;
		public double MagneticBearing;
		public double Length;

		public double ClearWay;
		public double TODA;
		public double TODAAccuracy;
		public double ASDA;

		public Guid Identifier;
		public Guid PairID;
		public Guid ADHP_ID;

		public long index;
		public bool Selected;

		public FeatureRefObject GetFeatureRefObject()
		{
			FeatureRefObject fro = new FeatureRefObject();
			fro.Feature = new FeatureRef(Identifier);
			return fro;
		}

		public void Initialize()
		{
			pPtGeo = new EnumArray<Point, eRWY>();
			pPtPrj = new EnumArray<Point, eRWY>();
			clptIdent = new EnumArray<Guid, eRWY>();
		}

		public override string ToString()
		{
			return Name;
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ILSType
	{
		public Point pPtGeo;
		public Point pPtPrj;

		public string CallSign;

		public Guid RWY_ID;
		public Guid NAV_Ident;
		public Guid Identifier;

		public double MagVar;
		public double HorAccuracy;

		public double GPAngle;
		public double GP_RDH;
		public double LLZ_THR;
		public double SecWidth;
		public double AngleWidth;

		public int index;
		public int Category;

		public long Tag;

		public NavaidType ToNavaid()
		{
			NavaidType result = new NavaidType();

			result.pPtGeo = this.pPtGeo;
			result.pPtPrj = this.pPtPrj;

			result.Name = this.CallSign;
			result.CallSign = this.CallSign;

			result.NAV_Ident = this.NAV_Ident;
			result.Identifier = this.Identifier;

			result.MagVar = this.MagVar;
			result.HorAccuracy = HorAccuracy;
			result.Range = 40000.0;

			result.GPAngle = this.GPAngle;
			result.GP_RDH = this.GP_RDH;

			result.LLZ_THR = this.LLZ_THR;
			result.SecWidth = this.SecWidth;
			result.AngleWidth = this.AngleWidth;

			result.ValCnt = -1;
			result.Tag = this.Tag;
			//result.index = this.index;
			result.PairNavaidIndex = -1;
			result.TypeCode = eNavaidType.LLZ;
			result.PairNavaidType = eNavaidType.NONE;
			result.IntersectionType = eIntersectionType.ByAngle;

			return result;
		}

		public FeatureRef GetFeatureRef()
		{
			return new FeatureRef(Identifier);
		}

		public SignificantPoint GetSignificantPoint()
		{
			SignificantPoint sp = new SignificantPoint();
			sp.NavaidSystem = new FeatureRef(NAV_Ident);
			return sp;
		}

		public override string ToString()
		{
			if (!string.IsNullOrWhiteSpace(CallSign))
				return CallSign;

			return string.Empty;
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct NavaidType
	{
		public Point pPtGeo;
		public Point pPtPrj;

		public string Name;
		public string CallSign;

		public Guid NAV_Ident;
		public Guid Identifier;

		public double MagVar;
		public double Disp;			//DME Displacement
		public double HorAccuracy;
		public double Range;

		public double GPAngle;
		public double GP_RDH;
		public double LLZ_THR;
		public double SecWidth;
		public double AngleWidth;

		public double[] ValMin;
		public double[] ValMax;

		public long ValCnt;
		public long Tag;
		public long index;
		public long PairNavaidIndex;
		public eNavaidType TypeCode;
		public eNavaidType PairNavaidType;
		public eIntersectionType IntersectionType;

		public FeatureRef GetFeatureRef()
		{
			return new FeatureRef(Identifier);
		}

		public SignificantPoint GetSignificantPoint()
		{
			SignificantPoint sp = new SignificantPoint();
			sp.NavaidSystem = new FeatureRef(NAV_Ident);
			return sp;
		}

		//public static bool operator ==(FIXableNavaidType item1, FIXableNavaidType item2)
		//{
		//    return item1.Name == item2.Name && item1.CallSign == item2.CallSign;
		//}

		public override string ToString()
		{
			if (!string.IsNullOrWhiteSpace(CallSign))
				return CallSign;

			if (!string.IsNullOrWhiteSpace(Name))
				return Name;

			return string.Empty;
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct WPT_FIXType
	{
		private static Constants.Constants constants;

		public Point pPtGeo;
		public Point pPtPrj;

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
			//DesignatedPoint dp = new DesignatedPoint();

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

		public NavaidType ToNavaid()
		{
			if (constants == null)
				constants = new Constants.Constants();

			NavaidType result = new NavaidType();

			result.pPtGeo = this.pPtGeo;
			result.pPtPrj = this.pPtPrj;

			result.NAV_Ident = this.NAV_Ident;
			result.Identifier = this.Identifier;
			result.Name = this.Name;
			result.CallSign = this.CallSign;

			result.TypeCode = this.TypeCode;
			result.MagVar = this.MagVar;

			if (this.TypeCode == eNavaidType.VOR)
				result.Range = constants.NavaidConstants.VOR.Range;
			else if (this.TypeCode == eNavaidType.NDB)
				result.Range = constants.NavaidConstants.NDB.Range;
			else if (this.TypeCode == eNavaidType.DME)
				result.Range = constants.NavaidConstants.DME.Range;
			else
				result.Range = constants.NavaidConstants.LLZ.Range;

			result.index = -1;
			result.PairNavaidIndex = -1;

			//result.GP_RDH = 0;
			//result.Course = 0;
			//result.LLZ_THR = 0;
			//result.SecWidth = 0;

			result.Tag = this.Tag;
			result.ValCnt = -2;
			return result;
		}

		public override string ToString()
		{
			if (!string.IsNullOrWhiteSpace(CallSign))
				return CallSign;

			if (!string.IsNullOrWhiteSpace(Name))
				return Name;

			return string.Empty;
		}

		//public static bool operator ==(WPT_FIXType item1, WPT_FIXType item2)
		//{
		//    return item1.Identifier == item2.Identifier;
		//}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ObstacleData
	{
		public Point pPtGeo;
		public Point pPtPrj;

		public double TurnDistL;
		public double TurnAngleL;

		/// <summary>
		/// X
		/// </summary>
		public double Dist;			//X
		/// <summary>
		/// Y
		/// </summary>
		public double CLShift;		//Y

		public double d0;
		public double DistStar;
		public double Height;
		public double Elev;
		public double EffectiveHeight;

		public double ReqH;
		public double ReqOCH;   //ReqOCA
		public double hSurface;
		public double hPenet;

		public double Rmin;
		public double Rmax;
		public double MOC;		//MOCA
		public double fSecCoeff;
		//=========================
		public double PDG;

		public double NomPDGDist;
		public double NomPDGHeight;

		public double PDGToTop;
		public double PDGAvoid;

		public double ReqTNH;   //ReqTNA
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
		public int Index;			//?????????
		public double minZPlane;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct Obstacle
	{
		public Geometry pGeomGeo;
		public Geometry pGeomPrj;

		public Guid Identifier;

		public string UnicalName;
		public string TypeName;

		public long ID;
		public bool IgnoredByUser;
		/// <summary>
		/// Actually is Elevation
		/// </summary>
		public double Height;		//Elevation
		public double HorAccuracy;
		public double VertAccuracy;

		//public int[] Parts;
		public int PartsNum;	//????????????
		public int NIx;
		public int Index;

		override public string ToString()
		{
			return string.Format("{0} / {1}", TypeName, UnicalName);
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ObstacleContainer
	{
		public Obstacle[] Obstacles;
		public ObstacleData[] Parts;

		public void Clear()
		{
			Obstacles = new Obstacle[0];
			Parts = new ObstacleData[0];
		}

		void AddPart(ref int Last, int Dest, ObstacleContainer from, ObstacleData Src)
		{
			Parts[Dest] = Src;

			if (from.Obstacles[Src.Owner].NIx < 0)
			{
				Last += 1;
				Obstacles[Last] = from.Obstacles[Src.Owner];
				Obstacles[Last].PartsNum = 0;
				from.Obstacles[Src.Owner].NIx = Last;
				//Array.Resize<int>(ref Obstacles[Last].Parts, from.Obstacles[Src.Owner].PartsNum - 1);
			}

			Parts[Dest].Owner = from.Obstacles[Src.Owner].NIx;
			Parts[Dest].Index = Obstacles[Parts[Dest].Owner].PartsNum;
			Obstacles[Parts[Dest].Owner].PartsNum++;
			//Obstacles[Parts[Dest].Owner].Parts[Parts[Dest].Index] = Dest;
		}

		//void AddPart(ref int Last, int Dest, ObstacleContainer from, ObstacleData Src)
		//{
		//	Parts[Dest] = Src;

		//	if (from.Obstacles[Src.Owner].NIx < 0)
		//	{
		//		Last += 1;
		//		Obstacles[Last] = from.Obstacles[Src.Owner];
		//		Obstacles[Last].PartsNum = 0;
		//		from.Obstacles[Src.Owner].NIx = Last;
		//		Array.Resize<int>(ref Obstacles[Last].Parts, from.Obstacles[Src.Owner].PartsNum - 1);
		//	}

		//	Parts[Dest].Owner = from.Obstacles[Src.Owner].NIx;
		//	Parts[Dest].Index = Obstacles[Parts[Dest].Owner].PartsNum;
		//	Obstacles[Parts[Dest].Owner].PartsNum ++;
		//	Obstacles[Parts[Dest].Owner].Parts[Parts[Dest].Index] = Dest;
		//}

		void MovePart(int Dest, int Src)
		{
			int L, K;
			L = Parts[Dest].Owner;
			K = Parts[Dest].Index;

			Obstacles[L].PartsNum -= 1;
			//Obstacles[L].Parts[K] = Obstacles[L].Parts[Obstacles[L].PartsNum];
			//Parts[Obstacles[L].Parts[K]].Index = K;

			Parts[Dest] = Parts[Src];
			//Obstacles[Parts[Dest].Owner].Parts[Parts[Dest].Index] = Dest;
			//N -= 1;
		}

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
		public void SortByGUID()
		{
			Array.Sort(Parts, ComparePartsByGUID);
		}

		public int ComparePartsByGUID(ObstacleData d0, ObstacleData d1)
		{
			int res = Obstacles[d0.Owner].Identifier.CompareTo(Obstacles[d1.Owner].Identifier);
			if (res != 0)
				return res;

			if (d0.pPtPrj.X > d1.pPtPrj.X)
				return 1;

			if (d0.pPtPrj.X < d1.pPtPrj.X)
				return -1;

			if (d0.pPtPrj.Y > d1.pPtPrj.Y)
				return 1;

			if (d0.pPtPrj.Y < d1.pPtPrj.Y)
				return -1;

			return 0;
		}
	}

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct PDGLevel
	//{
	//    public double PDG;
	//    public double Range;
	//    public long AdjustANgle;
	//}

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct ReportType
	//{
	//    public string Procedure;
	//    public string Category;
	//    public string RWY;
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

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct CLPoint
	{
		public RunwayCentrelinePoint pCLPoint;
		public Point pPtGeo;
		public Point pPtPrj;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public class CBWPT_FixItem
	{
		public WPT_FIXType WPT_FIX { get; private set; }

		public CBWPT_FixItem(WPT_FIXType wptFix)
		{
			WPT_FIX = wptFix;
		}

		public override string ToString()
		{
			return WPT_FIX.ToString();
		}
	}
}
