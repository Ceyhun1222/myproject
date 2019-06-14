using Aran.AranEnvironment;
using Aran.PANDA.Common;

namespace Aran.PANDA.LegCreator
{
	class GlobalParams
	{
		public static IAranEnvironment AranEnvironment
		{
			get;
			internal set;
		}
		public static SpatialReferenceOperation SpatialRefOperation
		{
			get;
			internal set;
		}
	}
}
