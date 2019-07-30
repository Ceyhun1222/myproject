using System.Collections.Generic;

namespace Aran.Aim.Data.Filters
{
    public abstract class LogicOps
    {
    }

    public class BinaryLogicOp : LogicOps
    {
        public BinaryLogicOp ()
        {
            OperationList = new List<OperationChoice> ();
        }

        public BinaryLogicOpType Type { get; set; }

        public List<OperationChoice> OperationList { get; private set; }
    }

    public enum BinaryLogicOpType
    {
        And, 
        Or
    }
}
