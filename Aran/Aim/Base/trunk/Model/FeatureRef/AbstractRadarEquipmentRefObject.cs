using Aran.Aim.DataTypes;

namespace Aran.Aim.Objects
{
    public class AbstractRadarEquipmentRefObject : AbstractFeatureRefObject<AbstractRadarEquipmentRef>
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AbstractRadarEquipmentRefObject; }
		}
	}
}
