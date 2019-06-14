using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.PANDA.Common
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public enum eRoundMode
	{
		NONE = 0,
		FLOOR = 1,
		NEAREST = 2,
		CEIL = 3,
		SPECIAL_FLOOR = 4,
		SPECIAL_NEAREST = 5,
		SPECIAL_CEIL = 6,
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public enum eDegree2StringMode
	{
		DD,
		DM,
		DMS,
		DMSLat, //NS
		DMSLon  //EW
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public enum SideDirection
	{
		sideRight = -1,
		sideOn = 0,
		sideLeft = 1
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public enum EntryDirection
	{
		None = 0,
		Toward = 1,
		Away = 2
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public enum TurnDirection : int
	{
		CW = -1,
		NONE = 0,
		CCW = 1,
		CoLinear = 2
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public enum eOAS
	{
		ZeroPlane = 0,
		WPlane = 1,
		XlPlane = 2,
		YlPlane = 3,
		ZPlane = 4,
		YrPlane = 5,
		XrPlane = 6,
		WsPlane = 7,
		CommonPlane = 8,
		NonPrec = 9
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public enum eSensorType
	{ GNSS, DME_DME, VOR_DME };


	/*

	(Enroute, SIDGE56,STARGE56) > 56 km
	(SIDGE28, STAR, IIAP, MApGE28) < 56 km
	(SID, MApLT28) < 28 km
	FAFApch

	Departure	(SID,SIDGE28, SIDGE56)
	Approach	(IIAP, FAFApch, MApLT28, MApGE28)
	Arrival		(STARGE56, STAR)
	Enroute		(Enroute)

		public enum eFlightPhase
		{
			Enroute, SIDGE56, SIDGE28, SID, STARGE56, STAR, IIAP, FAFApch, MApLT28, MApGE28
		}
			SID, MApLT28, FAFApch, SIDGE28, MApGE28, STAR, IIAP, SIDGE56, STARGE56, Enroute

	*/

	[System.Runtime.InteropServices.ComVisible(false)]
	public enum eFlightPhase
	{
		SID, MApLT28, FAFApch,              //	< 28 km
		SIDGE28, MApGE28, STAR, IIAP,       //	< 56 km
		SIDGE56, STARGE56, Enroute,         //	> 56 km
											//FAFApch
	};

	[System.Runtime.InteropServices.ComVisible(false)]
	public enum ePBNClass
	{
		RNP_APCH, RNAV1, RNAV2,
		RNP03, RNP1, RNP4, RNAV5,
		GNSS, RNP_APCH_Final
	};

	[System.Runtime.InteropServices.ComVisible(false)]
	public enum eFIXRole
	{
		IAF_GT_56_, IAF_LE_56_, IF_, FAF_, MAPt_,
		MATF_LE_56, MATF_GT_56, MAHF_LE_56,
		MAHF_GT_56, IDEP_, DEP_, TP_, PBN_IAF,
		PBN_IF, PBN_FAF, PBN_MAPt, PBN_MATF_LT_28,
		PBN_MATF_GE_28, DEP_ST, SDF                         /*Departure Start point, Step Down FIX*/
	};

	//[System.Runtime.InteropServices.ComVisible(false)]
	//public enum ePathAndTermination
	//{
	//	AF, HF, HA, HM, IF, PI, PT, TF, CA, CD, CI, CR, CF,
	//	DF, FA, FC, FT, FM, VM, FD, VR, VD, VI, VA, RF
	//};

	[System.Runtime.InteropServices.ComVisible(false)]
	public enum eFlyMode { Flyby = 0, Flyover = 1, Atheight = 2 };

	[System.Runtime.InteropServices.ComVisible(false)]
	public enum eFlyPath { DirectToFIX, CourseToFIX, TrackToFIX };

	[System.Runtime.InteropServices.ComVisible(false)]
	public enum eNavaidType
	{
		NONE = -1,
		VOR = 0,
		DME = 1,
		NDB = 2,
		LLZ = 3,
		TACAN = 4,
		RadarFIX = 5,
		Dopler = 16
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public enum eIntersectionType
	{
		//NONE = 0,
		ByDistance = 1,
		ByAngle = 2,
		OnNavaid = 3,
		RadarFIX = 4,
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public enum eRWY
	{
		ptStart = 0,
		ptTHR = 1,
		ptDER = 2,
		ptEnd = 3
	}

	public enum InterfaceUnitType
	{
		UI,
		Report
	}
}

