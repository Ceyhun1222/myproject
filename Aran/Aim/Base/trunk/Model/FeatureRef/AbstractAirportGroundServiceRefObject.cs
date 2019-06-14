
using Aran.Aim.DataTypes;

namespace Aran.Aim.Objects
{
	public class AbstractAirportGroundServiceRefObject : AbstractFeatureRefObject <AbstractAirportGroundServiceRef>
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AbstractAirportGroundServiceRefObject; }
		}
	}
}
