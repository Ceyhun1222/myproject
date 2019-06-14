
using System;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule16 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule
        //"Each [...] that has an upperLevel must have an upperLevelReference.";
        public override bool CheckParent(object obj)
        {
            var item = (dynamic)obj;
            var upperLevel = item.UpperLevel;
            if (upperLevel != null)
            {
                return item.UpperLevelReference != null;
            }
            return true;
        }

        public override Type GetApplicableType()
        {
            return null;
        }

        public override string GetApplicableProperty()
        {
            return "UpperLevel";
        }

        public override string Source()
        {
            return RuleSource.AIXM45BR;
        }

        public override string Svbr()
        {
            return "Each [...] that has an upperLevel must have an upperLevelReference.";
        }

        public override string Comments()
        {
            return "If VAL_DIST_VER_UPPER is specified, then UOM_DIST_VER_UPPER and CODE_DIST_VER_UPPER are mandatoryIf UOM_DIST_VER_UPPER is specified, then VAL_DIST_VER_UPPER and CODE_DIST_VER_UPPER are mandatory.";
        }

        public override string Name()
        {
            return "generic rule 34 - gen_upperLevelUom_vs_Reference";
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
