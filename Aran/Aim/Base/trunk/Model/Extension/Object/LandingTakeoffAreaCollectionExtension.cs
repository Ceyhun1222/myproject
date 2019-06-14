using System.Collections.ObjectModel;

namespace Aran.Aim.Features
{
	public static class LandingTakeoffAreaCollectionExtension
	{
		public static ReadOnlyCollection<RunwayDirection> GetRunway (this LandingTakeoffAreaCollection thisValue)
		{
			return null;
		}
		public static ReadOnlyCollection<TouchDownLiftOff> GetTLOF (this LandingTakeoffAreaCollection thisValue)
		{
			return null;
		}
	}
}
