using Aran.Aim.DataTypes;

namespace Aran.Aim.Objects
{
    public class AbstractSurveillanceRadarRefObject : AbstractFeatureRefObject<AbstractSurveillanceRadarRef>
    {
        public override ObjectType ObjectType
        {
            get { return ObjectType.AbstractSurveillanceRadarRefObject; }
        }
    }
}
