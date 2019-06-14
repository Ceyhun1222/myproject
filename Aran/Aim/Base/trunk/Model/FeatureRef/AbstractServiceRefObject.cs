using Aran.Aim.DataTypes;

namespace Aran.Aim.Objects
{
    public class AbstractServiceRefObject : AbstractFeatureRefObject <AbstractServiceRef>
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AbstractServiceRefObject; }
		}
	}
}
