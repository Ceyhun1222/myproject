using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.PANDA.RNAV.Approach
{
	enum eApproachType
	{
		LNAV = 0,
		VNAV = 1
	}

	enum eLandingType
	{
		StraightIn = 0,
		Circkling = 1
	}

	//static class cFlightProcedure
	//{
	//	public const int fpEnroute = 0;
	//	public const int fpDeparture = 1;
	//	public const int fpArrival = 2;
	//	public const int fpInitialApproach = 3;
	//	public const int fpIntermediateApproach = 4;
	//	public const int fpFinalApproach = 5;
	//	public const int fpInitialMissedApproach = 6;
	//	public const int fpIntermediateMissedApproach = 7;
	//	public const int fpFinalMissedApproach = 8;
	//}

	enum eFlightProcedure
	{
		fpEnroute, fpDeparture, fpArrival, fpInitialApproach, fpIntermediateApproach,
		fpFinalApproach, fpInitialMissedApproach, fpIntermediateMissedApproach,
		fpFinalMissedApproach
	}

}
