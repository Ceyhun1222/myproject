using System.Collections.Generic;
using Aran.Aim.Features;
using Aran.AranEnvironment.Symbols;
using Aran.Geometries;
using Aran.PANDA.Common;

namespace Aran.PANDA.RNAV.DMECoverage
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public enum Degree2StringMode
	{
		DD,
		DM,
		DMS,
		DMSLat, //NS
		DMSLon  //EW
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct CriticalDME
	{
		public NavaidType DMEStation;
		public List<CDMECoverage> Coverages;

		public MultiPolygon CoverArea;
		public MultiPolygon ExemptArea;
		public MultiPolygon CriticalArea;

		private int _CoverAreaElem;
		private int _ExemptAreaElem;
		private int _CriticalAreaElem;

		public void Initialize(NavaidType dme)
		{
			DMEStation = dme;

			if (Coverages == null)
				Coverages = new List<CDMECoverage>();
			else
				Coverages.Clear();

			_CoverAreaElem = -1;
			_ExemptAreaElem = -1;
			_CriticalAreaElem = -1;
		}

		public void DrawPolygons(int flags)
		{

			if (CoverArea != null && (flags & 1) != 0)
				_CoverAreaElem = GlobalVars.gAranGraphics.DrawMultiPolygon(CoverArea, eFillStyle.sfsCross, ARANFunctions.RGB(0, 255, 0));
			else
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_CoverAreaElem);
				_CoverAreaElem = -1;
			}

			if (ExemptArea != null && (flags & 2) != 0)
				_ExemptAreaElem = GlobalVars.gAranGraphics.DrawMultiPolygon(ExemptArea, eFillStyle.sfsDiagonalCross, ARANFunctions.RGB(0, 255, 255));
			else
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_ExemptAreaElem);
				_ExemptAreaElem = -1;
			}

			if (CriticalArea != null && (flags & 4) != 0)
				_CriticalAreaElem = GlobalVars.gAranGraphics.DrawMultiPolygon(CriticalArea, eFillStyle.sfsDiagonalCross, ARANFunctions.RGB(255, 0, 0));
			else
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_CriticalAreaElem);
				_CriticalAreaElem = -1;
			}
		}

		public void ClearImages()
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_CoverAreaElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_ExemptAreaElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_CriticalAreaElem);

			_CoverAreaElem = -1;
			_ExemptAreaElem = -1;
			_CriticalAreaElem = -1;
		}

		override public string ToString()
		{
			if (!string.IsNullOrWhiteSpace(DMEStation.CallSign))
				return DMEStation.CallSign;

			if (!string.IsNullOrWhiteSpace(DMEStation.Name))
				return DMEStation.Name;

			return "";
		}
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

	public struct Procedure
	{
		public int startLeg;
		public int endleg;
		public string Name;
		override public string ToString()
		{
			if (Name == null)
				return "";

			return Name;
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct Segment
	{
		//public RouteSegment pFeature;
		//public Route proc;

		public FIX Start;
		public FIX End;
		public MultiLineString NominalTrack;
		public MultiPolygon ProtectionArea;

		public LegApch Forwardleg;
		public LegApch Backwardleg;

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
			if (Start != null && Start.Name != null)
			{
				if (End != null && End.Name != null)
					return Start.Name + " - " + End.Name;

				return Start.Name;
			}

			if (End == null || End.Name == null)
				return End.Name;

			return "";
		}
	}
}
