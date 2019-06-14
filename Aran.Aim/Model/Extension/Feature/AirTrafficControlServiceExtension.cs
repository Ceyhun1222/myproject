using System.Collections.ObjectModel;

namespace Aran.Aim.Features
{
	public static class AirTrafficControlServiceExtension
	{
		public static ReadOnlyCollection<AirportHeliport> GetClientAirport (this AirTrafficControlService thisValue)
		{
			return null;
		}
		public static ReadOnlyCollection<Airspace> GetClientAirspace (this AirTrafficControlService thisValue)
		{
			return null;
		}
		public static ReadOnlyCollection<HoldingPattern> GetClientHolding (this AirTrafficControlService thisValue)
		{
			return null;
		}
		public static ReadOnlyCollection<AerialRefuelling> GetClientAerialRefuelling (this AirTrafficControlService thisValue)
		{
			return null;
		}
		public static DirectionFinder GetAircraftLocator (this AirTrafficControlService thisValue)
		{
			return null;
		}
	}
}
