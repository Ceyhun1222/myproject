
using System;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule10 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckParent(object obj)
        {
            var item = (dynamic) obj;
            var maximumLimit = item.MaximumLimit;
            if (maximumLimit!=null)
            {
                if (maximumLimit.Uom.ToString()=="FL")
                {
                    return maximumLimit.Value >= 10 && maximumLimit.Value <= 999;
                }
            }
            return true;
        }

        public override Type GetApplicableType()
        {
            return null;
        }

        public override string GetApplicableProperty()
        {
            return "MaximumLimit";
        }

        public override string Source()
        {
            return RuleSource.AIXM45BR;
        }

        public override string Svbr()
        {
            return "Each  [...].maximumLimit that has an uom equal to 'FL' should have 2 or 3 digits.";
        }

        public override string Comments()
        {
            return "If maximumLimit.UOM = 'FL' (flight level in hundreds of feet)  then it should have 2 or 3 digits.";
        }

        public override string Name()
        {
            return "generic rule 28 - Maximum_Limit_UOM_FL_2_3_digits";
        }

        public override string Category()
        {
            return "Recommended practice";
        }

        public override string Level()
        {
            return ErrorLevel.Warning;
        }

        #endregion
    }
}
