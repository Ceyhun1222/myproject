
using Aran.Aim.DataTypes;

namespace Aran.Aim.Objects
{
    public class AbstractNavaidEquipmentRefObject : AbstractFeatureRefObject <AbstractNavaidEquipmentRef>
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AbstractNavaidEquipmentRefObject; }
		}
	}
}
