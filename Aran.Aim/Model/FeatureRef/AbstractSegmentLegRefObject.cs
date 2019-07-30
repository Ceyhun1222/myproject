using Aran.Aim.DataTypes;

namespace Aran.Aim.Objects
{
    public class AbstractSegmentLegRefObject : AbstractFeatureRefObject <AbstractSegmentLegRef>
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AbstractSegmentLegRefObject; }
		}
	}
}
