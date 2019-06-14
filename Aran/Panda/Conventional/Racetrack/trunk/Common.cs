using System;
using Aran.Geometries;

namespace Aran.PANDA.Conventional.Racetrack
{
	public struct SpeedInterval
    {
        public Double Min, Max;
        public object Tag;
    }
	public enum Categories
	{
		A = 0,
		B = 1,
		C = 2,
		D = 3,
		E = 4
	}

	public enum ObstactleReportType
	{
		None,
		BasicArea,
		ProtectSect1,
		ProtectSect2,
		ProtectRecipDir,
		ProtectIntersectRecipEntry,
		SecondaryArea,
		Buffer
	}

    public enum FlightPhase
    { 
        TerminalNormal,
        TerminalTurbulence,
        TerminalInitialApproach,
        Enroute
    }

	public enum PointChoice
	{
		None = 0,
		Create = 1,
		Select = 2
	}

	public enum NavType
	{
		None,
		Vor,
		Dme,
		Ndb
	}

	public enum DsgPntDefinition
	{
		ViaVorVorRadial,
		ViaSelectionFromDb
	}

	public enum ProcedureTypeConv
	{
		None = -1,
		Vordme = 0,
		VorNdb = 1,
		Vorvor = 2,
	}

	public enum ModulType
	{
		Holding,
		Racetrack
	}

	public enum EntryDirection
	{
		None = 0,
		Toward = 1,
		Away = 2
	}

	public class NavaidPntPrj
	{
		public NavaidPntPrj ( Point pnt, NavType navType )
		{
			Value = pnt;
			Type = navType;
		}

		public Point Value
		{
			get; private set;
		}

		public NavType Type
		{
			get; private set;
		}
	}

	public static class Common
	{
		public const double MinAltitude = 300;
		public const double SecondAltitude = 4250;
		public const double ThirdAltitude = 6100;
		public const double MaxAltitude = 10350;
		public const double MinEnrouteDistance = 300;
		public const double MaxEnrouteDistance = 300000;//Max 300 km 
		public const double MaxStarDownTo30Distance = 56000;//Max 56 km in StarDownTo30Distance
		public const double ConstDoc = 370400;

		public static double AdaptToInterval ( double realValue, double minVal, double maxVal, double increment )
		{
			if ( realValue <= minVal )
				return minVal;

			if ( realValue >= maxVal )
				return maxVal;

			if ( increment == 0 )
				return realValue;

			double quotient = Math.IEEERemainder ( realValue, increment );
			if ( quotient < increment / 2 )
			{
				return realValue - quotient;
			}
			else
			{
				return realValue + ( increment - quotient );
			}

		}
	}
}