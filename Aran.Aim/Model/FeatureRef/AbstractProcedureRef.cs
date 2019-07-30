
namespace Aran.Aim.DataTypes
{
    public class AbstractProcedureRef : AbstractFeatureRef<ProcedureType>
    {
        public override DataType DataType
        {
            get { return DataType.AbstractProcedureRef; }
        }
    }
}
