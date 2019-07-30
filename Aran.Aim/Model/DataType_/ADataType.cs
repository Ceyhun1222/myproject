
namespace Aran.DataTypes
{
    public abstract class ADataType : AranObject
    {
        public override AranObjectType AranObjectType
        {
            get { return AranObjectType.DataType; }
        }

        public abstract DataType DataType { get; }

        //public abstract void AssignFrom (string [] values, int startIndex);
    }
}
