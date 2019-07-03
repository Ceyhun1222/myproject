using System.Collections.ObjectModel;

namespace Aran.Aim.Features
{
	public static class StandardLevelSectorExtension
	{
		public static ReadOnlyCollection<Airspace> GetApplicableAirspace (this StandardLevelSector thisValue)
		{
			return null;
		}
		public static StandardLevelColumn GetApplicableLevelColumn (this StandardLevelSector thisValue)
		{
			return null;
		}
	}
}
