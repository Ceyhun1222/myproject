
using System;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule12 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckParent(object obj)
        {
            //Each  [...] that has a lowerLimitAltitude must have a lowerLimitReference
            var item = (dynamic)obj;
            var lowerLimitAltitude = item.LowerLimitAltitude;
            if (lowerLimitAltitude != null)
                return item.LowerLimitReference != null;
            return true;
        }

        public override Type GetApplicableType()
        {
            return null;
        }

        public override string GetApplicableProperty()
        {
            return "LowerLimitAltitude";
        }

        public override string Source()
        {
            return RuleSource.AIXM45BR;
        }

        public override string Svbr()
        {
            return "Each  [...] that has a lowerLimitAltitude must have a lowerLimitReference";
        }

        public override string Comments()
        {
            return "If VAL_DIST_VER_LOWER is specified, then UOM_DIST_VER_LOWER and CODE_DIST_VER_LOWER are mandatoryIf UOM_DIST_VER_LOWER is specified, then VAL_DIST_VER_LOWER and CODE_DIST_VER_LOWER are mandatory. ";
        }

        public override string Name()
        {
            return "generic rule 30 - lowerLimitAltitudeUom_vs_Reference";
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
