
using System;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule22 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule
        //Each [...] that has a minimumLimitReference must have a minimumLimit
        public override bool CheckParent(object obj)
        {
            var item = (dynamic)obj;
            if (item.MinimumLimitReference!=null)
            {
                return item.MinimumLimit != null;
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
            return "Each [...] that has a minimumLimitReference must have a minimumLimit";
        }

        public override string Comments()
        {
            return "If CODE_DIST_VER_MNM is specified, then UOM_DIST_VER_MNM and VAL_DIST_VER_MNM are mandatory. ";
        }

        public override string Name()
        {
            return "gen_Reference_vs_minimumLimit";
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
