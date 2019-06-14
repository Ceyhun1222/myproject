
using System;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule21 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckParent(object obj)
        {
            //Each [...] that has a lowerLimitReference must have a lowerLimit or a lowerLimitAltitude.";
            var item = (dynamic)obj;
            if (item.LowerLimitReference != null)
            {
                return item.LowerLimit != null || item.LowerLimitAltitude != null;
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
            return "Each [...] that has a lowerLimitReference must have a lowerLimit or a lowerLimitAltitude.";
        }

        public override string Comments()
        {
            return "If CODE_DIST_VER_LOWER is specified, then UOM_DIST_VER_LOWER and VAL_DIST_VER_LOWER are mandatory.";
        }

        public override string Name()
        {
            return "generic rule 41 - Reference_vs_lowerLimitUom";
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