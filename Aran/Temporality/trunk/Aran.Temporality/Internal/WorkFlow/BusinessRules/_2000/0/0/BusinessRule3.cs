
using System;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule3 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as ElevatedCurve;
            if (item == null) return false;
            if (item.GeoidUndulation != null)
            {
                return item.Elevation != null;
            }
            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof(ElevatedCurve);
        }

        public override string GetApplicableProperty()
        {
            return null;
        }

        public override string Source()
        {
            return RuleSource.AIXM45BR;
        }

        public override string Svbr()
        {
            return "Each ElevatedCurve that has a geoidUndulation must have an elevation";
        }

        public override string Comments()
        {
            return "VAL_GEOID_UNDULATION may be specified only if VAL_ELEV has been specified";
        }

        public override string Name()
        {
            return "generic rule 05 - EC_GeoidUndulation_Elevation";
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
