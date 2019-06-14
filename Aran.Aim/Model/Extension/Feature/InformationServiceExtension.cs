using System.Collections.ObjectModel;

namespace Aran.Aim.Features
{
	public static class InformationServiceExtension
	{
		public static ReadOnlyCollection<VOR> GetNavaidBroadcast (this InformationService thisValue)
		{
			return null;
		}
		public static ReadOnlyCollection<Airspace> GetClientAirspace (this InformationService thisValue)
		{
			return null;
		}
		public static ReadOnlyCollection<AirportHeliport> GetClientAirport (this InformationService thisValue)
		{
			return null;
		}
		public static ReadOnlyCollection<HoldingPattern> GetClientHolding (this InformationService thisValue)
		{
			return null;
		}
		public static ReadOnlyCollection<AerialRefuelling> GetClientAerialRefuelling (this InformationService thisValue)
		{
			return null;
		}
	}
}
