
namespace Aran.Aim.Data.Filters
{
    public class Filter
    {
        public Filter ( OperationChoice operationChoice)
        {
            Operation = operationChoice;
        }

        public OperationChoice Operation
        {
            get;
            set;
        }
    }
}
