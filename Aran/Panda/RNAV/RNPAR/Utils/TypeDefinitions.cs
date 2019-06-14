using System;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Geometries;
using Aran.Queries;

namespace Aran.Panda.RNAV.RNPAR.Utils
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public struct TraceSegment
	{
		public Point ptIn;			//pPtStart;
		public Point ptOut;		//pPtEnd;
		public MultiLineString PathPrj;	//pNominalPoly;
		public MultiPolygon pProtectArea;

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
		public Point PtCenter1;
		public string Center1Coords;
		public int Turn1Dir;
		public double Turn1R;
		public double Turn1Angle;
		public double Turn1Length;
		public string St1Coords;
		public string Fin1Coords;
		//=========================
		public Point PtCenter2;
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
		public double Altitude;
		public double DistToNext;

		public int Turn;
		public double Radius;
		public double turnAngle;
		public double TurnArcLen;
		public double CenterLat;
		public double CenterLon;
        public double TrueCourse;
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
    public struct NavaidType
    {
        public Point pPtGeo;
        public Point pPtPrj;

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

    [System.Runtime.InteropServices.ComVisible(false)]
    public enum eNavaidType
    {
        NONE = -1,
        VOR = 0,
        DME = 1,
        NDB = 2,
        LLZ = 3,
        TACAN = 4,
        //StartPoint = 5
    }

    [System.Runtime.InteropServices.ComVisible(false)]
    public enum eIntersectionType
    {
        ByDistance = 1,
        ByAngle = 2,
        OnNavaid = 3,
        RadarFIX = 4
    };
}
