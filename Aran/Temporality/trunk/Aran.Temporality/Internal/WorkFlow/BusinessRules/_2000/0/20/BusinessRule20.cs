
using System;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule20 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckParent(object obj)
        {
            var item = (dynamic) obj;

            // "Each [...] that has a maximumLimit.uom equal to 'FL' or 'SM' must have a maximumLimitReference equal to 'STD'."

            var maximumLimit=item.MaximumLimit;
            if (maximumLimit!=null)
            {
                if (maximumLimit.Uom.ToString()=="FL" || maximumLimit.Uom.ToString()=="SM")
                {
                    return item.maximumLimitReference.ToString() == "STD";
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
            return
                "Each [...] that has a maximumLimit.uom equal to 'FL' or 'SM' must have a maximumLimitReference equal to 'STD'.";
        }

        public override string Comments()
        {
            return
                "If the unit of measurement as the value 'FL' or 'SM', then the attribute CODE_DIST_VER_MAX must have the value 'STD' (standard pressure).";
        }

        public override string Name()
        {
            return "generic rule 40 - maximumLimit_vs_Reference";
        }

        public override string Category()
        {
            return RuleCategory.Standard;
        }

        public override string Level()
        {
            return ErrorLevel.Error;
        }

        #endregion
    }
}
