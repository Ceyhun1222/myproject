using System;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule7 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckParent(object obj)
        {
            //Each  [...].upperLimit that has an uom equal to 'FL' should have 2 or 3 digits.
            var item = (dynamic) obj;
            var upperLimit = item.UpperLimit;
            if (upperLimit != null)
            {
                if (upperLimit.Uom.ToString() == "FL")
                {
                    var value = upperLimit.Value;
                    return value >= 10 && value <= 999;
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
            return "UpperLimit";
        }

        public override string Source()
        {
            return RuleSource.AIXM45BR;
        }

        public override string Svbr()
        {
            return "Each  [...].upperLimit that has an uom equal to 'FL' should have 2 or 3 digits.";
        }

        public override string Comments()
        {
            return "If upperLimit.UOM = 'FL' (flight level in hundreds of feet)  then it should have 2 or 3 digits.";
        }

        public override string Name()
        {
            return "generic rule 24 - Upper_Limit_UOM_FL_3_digits";
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