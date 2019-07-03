using Aran.Aim.DataTypes;

namespace Aran.Aim.Objects
{
    public class AbstractGroundLightSystemRefObject : AbstractFeatureRefObject <AbstractGroundLightSystemRef>
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AbstractGroundLightSystemRefObject; }
		}
	}
}
