
using System;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule14 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule
        //Each [...] that has an upperLimit must have an upperLimitReference
        public override bool CheckParent(object obj)
        {
            var item = (dynamic)obj;
            var upperLimit = item.UpperLimit;
            if (upperLimit != null)
                return item.UpperLimitReference != null;
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
            return "Each [...] that has an upperLimit must have an upperLimitReference.";
        }

        public override string Comments()
        {
            return "If VAL_DIST_VER_UPPER is specified, then UOM_DIST_VER_UPPER and CODE_DIST_VER_UPPER are mandatoryIf UOM_DIST_VER_UPPER is specified, then VAL_DIST_VER_UPPER and CODE_DIST_VER_UPPER are mandatory.";
        }

        public override string Name()
        {
            return "generic rule 32 - gen_upperLimitUom_vs_Reference";
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