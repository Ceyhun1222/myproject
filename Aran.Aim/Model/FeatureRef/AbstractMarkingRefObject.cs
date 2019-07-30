
using Aran.Aim.DataTypes;

namespace Aran.Aim.Objects
{
    public class AbstractMarkingRefObject : AbstractFeatureRefObject<AbstractMarkingRef>
    {
        public override ObjectType ObjectType
        {
            get { return ObjectType.AbstractMarkingRefObject; }
        }
    }
}
