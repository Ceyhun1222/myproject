
namespace Aran.Aim.DataTypes
{
    public class AbstractServiceRef : AbstractFeatureRef<ServiceType>
    {
        public override DataType DataType
        {
            get { return DataType.AbstractServiceRef; }
        }
    }
}
