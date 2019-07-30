
using Aran.Aim.DataTypes;

namespace Aran.Aim.Objects
{
    public class AbstractApproachLegRefObject : AbstractFeatureRefObject <AbstractApproachLegRef>
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AbstractApproachLegRefObject; }
		}
	}
}
