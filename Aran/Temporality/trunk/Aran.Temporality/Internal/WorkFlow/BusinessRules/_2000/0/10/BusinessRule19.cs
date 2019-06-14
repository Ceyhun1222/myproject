
using System;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule19 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule
        //"Each [...] that has a minimumLimit.uom equal to 'FL' or 'SM' must have a minimumLimitReference equal to 'STD'."
        public override bool CheckParent(object obj)
        {
            var item = (dynamic)obj;
            if (item.MinimumLimit != null)
            {
                if (item.MinimumLimit.Uom.ToString() == "FL" || item.MinimumLimit.Uom.ToString() == "SM")
                    return item.MinimumLimitReference.ToString() == "STD";
            }
            return true;
        }

        public override Type GetApplicableType()
        {
            return null;
        }

        public override string GetApplicableProperty()
        {
            return "MinimimLimit";
        }

        public override string Source()
        {
            return RuleSource.AIXM45BR;
        }

        public override string Svbr()
        {
            return "Each [...] that has a minimumLimit.uom equal to 'FL' or 'SM' must have a minimumLimitReference equal to 'STD'.";
        }

        public override string Comments()
        {
            return "If the unit of measurement has the value 'FL' or 'SM', then the attribute CODE_DIST_VER_MNM must have the value 'STD' (standard pressure).";
        }

        public override string Name()
        {
            return "generic rule 39 - minimumLimit_vs_Reference";
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
