
using System;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule26 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule
        //Each [...] that has a dateMagneticVariation must have a magneticVariation
        public override bool CheckParent(object obj)
        {
            var item = (dynamic)obj;
            if (item.DateMagneticVariation != null)
            {
                return item.MagneticVariation != null;
            }
            return true;
        }

        public override Type GetApplicableType()
        {
            return null;
        }

        public override string GetApplicableProperty()
        {
            return "DateMagneticVariation";
        }

        public override string Source()
        {
            return RuleSource.AIXM45BR;
        }

        public override string Svbr()
        {
            return "Each [...] that has a dateMagneticVariation must have a magneticVariation";
        }

        public override string Comments()
        {
            return "If DATE_MAG_VAR is specified, then VAL_MAG_VAR is mandatory";
        }

        public override string Name()
        {
            return "generic rule 51 - check_mandatory_dateMagneticVariation_magneticVariation";
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
