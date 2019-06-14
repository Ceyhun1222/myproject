
using System;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule23 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule
        //"Each [...] that has a maximumLimitReference must have a maximumLimit"
        public override bool CheckParent(object obj)
        {
            var item = (dynamic)obj;
            if (item.MaximumLimitReference!=null)
            {
                return item.MaximumLimit != null;
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
            return "Each [...] that has a maximumLimitReference must have a maximumLimit";
        }

        public override string Comments()
        {
            return "If CODE_DIST_VER_MAX is specified, then VAL_DIST_VER_MAX and UOM_DIST_VER_MAX are mandatory. ";
        }

        public override string Name()
        {
            return "gen_Reference_vs_maximumLimit";
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
