using System;

namespace Aran.PANDA.Common
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public struct FlightPhase
	{
		public String Name;
		public eFlightPhase flightPhase;
		public double MOC;
		public Interval IASRange, GradientRange, TurnRange, DistRange;
	};
}
