
using System;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule11 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule
        //"Each [...] that has a lowerLimit must have a lowerLimitReference."

        public override bool CheckParent(object obj)
        {
            var item = (dynamic)obj;
            var lowerLimit = item.LowerLimit;
            if (lowerLimit!=null)
            {
                var lowerLimitReference = item.LowerLimitReference;
                return lowerLimitReference != null;
            }
            return true;
        }

        public override Type GetApplicableType()
        {
            return null;
        }

        public override string GetApplicableProperty()
        {
            return "LowerLimit";
        }

        public override string Source()
        {
            return RuleSource.AIXM45BR;
        }

        public override string Svbr()
        {
            return "Each [...] that has a lowerLimit must have a lowerLimitReference.";
        }

        public override string Comments()
        {
            return "If VAL_DIST_VER_LOWER is specified, then UOM_DIST_VER_LOWER and CODE_DIST_VER_LOWER are mandatory.If UOM_DIST_VER_LOWER is specified, then VAL_DIST_VER_LOWER and CODE_DIST_VER_LOWER are mandatory.";
        }

        public override string Name()
        {
            return "generic rule 29 - lowerLimitUom_vs_Reference";
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