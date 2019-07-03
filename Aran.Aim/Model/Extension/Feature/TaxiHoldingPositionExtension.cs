using System.Collections.ObjectModel;

namespace Aran.Aim.Features
{
	public static class TaxiHoldingPositionExtension
	{
		public static GuidanceLine GetAssociatedGuidanceLine (this TaxiHoldingPosition thisValue)
		{
			return null;
		}
		public static ReadOnlyCollection<Runway> GetProtectedRunway (this TaxiHoldingPosition thisValue)
		{
			return null;
		}
	}
}
