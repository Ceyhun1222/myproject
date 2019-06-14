using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Aran.Aim.BusinessRules
{
    public enum AbstractOperationType
    {
        Operation,
        Group
    }

    public enum LogicType
    {
        And,
        Or
    }

    public enum OperationType
    {
        Assigned,
        Equal,
        Higher,
        HigherEqual,
        Lower,
        LowerEqual,
        ResolvedInto,
        OtherThan,
        AtLeast,
        AtMost,
        ExactlyOne,
        MoreThanOne,
        Expressed,
        Undefined
    }

    public enum ExpressedType
    {
        None,
        More,
        Less,
        Exactly
    }

    public enum InnerOperationType
    {
        None,
        Equal
    }
}
