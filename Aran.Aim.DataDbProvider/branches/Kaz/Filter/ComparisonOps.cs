
namespace Aran.Aim.Data.Filters
{
    public class ComparisonOps
    {
        public ComparisonOps ()
        {
        }

        public ComparisonOps (ComparisonOpType operationType, 
            string propertyName, object value = null)
        {
            OperationType = operationType;
            PropertyName = propertyName;
            Value = value;
        }

        public ComparisonOpType OperationType
        {
            get;
            set;
        }

        public string PropertyName
        {
            get;
            set;
        }

        public object Value
        {
            get;
            set;
        }
        
    }

    public enum ComparisonOpType
    {
        EqualTo,
        NotEqualTo,
        LessThan,
        GreaterThan,
        LessThanOrEqualTo,
        GreaterThanOrEqualTo,
        Null,
        NotNull,
        Like,
        NotLike,
        Is
    }
}
