
using System;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule17 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule
        //"Each [...] that has a minimumLimit must have a minimumLimitReference."
        public override bool CheckParent(object obj)
        {
            var item = (dynamic)obj;
            var minimumLimit = item.MinimumLimit;
            if (minimumLimit != null)
            {
                return item.MinimumLimitReference != null;
            }
            return true;
        }


        public override Type GetApplicableType()
        {
            return null;
        }

        public override string GetApplicableProperty()
        {
            return "MinimumLimit";
        }

        public override string Source()
        {
            return RuleSource.AIXM45BR;
        }

        public override string Svbr()
        {
            return "Each [...] that has a minimumLimit must have a minimumLimitReference.";
        }

        public override string Comments()
        {
            return "If VAL_DIST_VER_MNM is specified, then UOM_DIST_VER_MNM and CODE_DIST_VER_MNM are mandatoryIf UOM_DIST_VER_MNM is specified, then VAL_DIST_VER_MNM and CODE_DIST_VER_MNM are mandatory.";
        }

        public override string Name()
        {
            return "generic rule 35 - gen_minimumLimitUom_vs_Reference";
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
