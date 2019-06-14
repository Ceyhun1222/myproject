
using System;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule18 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule
        //"Each [...] that has a lowerLimit.uom equal to 'FL' or 'SM' or a lowerLevel.uom equal to 'FL' or 'SM' or a lowerLimitAltitude.uom equal to 'FL' or 'SM' must have a lowerLimitReference equal to 'STD'."
        public override bool CheckParent(object obj)
        {
            var item = (dynamic)obj;

            if (obj.GetType().GetProperty("LowerLimit")!=null && item.LowerLimit != null)
            {
                if (item.LowerLimit.Uom.ToString() == "FL" || item.LowerLimit.Uom.ToString() == "SM")
                    return item.LowerLimitReference.ToString() == "STD";
            }
            else if (obj.GetType().GetProperty("LowerLevel") != null && item.LowerLevel != null)
            {
                if (item.LowerLevel.Uom.ToString() == "FL" || item.LowerLevel.Uom.ToString() == "SM")
                    return item.LowerLimitReference.ToString() == "STD";
            }
            else if (obj.GetType().GetProperty("LowerLimitAltitude") != null && item.LowerLimitAltitude != null)
            {
                if (item.LowerLimitAltitude.Uom.ToString() == "FL" || item.LowerLimitAltitude.Uom.ToString() == "SM")
                    return item.LowerLimitReference.ToString() == "STD";
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
            return "Each [...] that has a lowerLimit.uom equal to 'FL' or 'SM' or a lowerLevel.uom equal to 'FL' or 'SM' or a lowerLimitAltitude.uom equal to 'FL' or 'SM' must have a lowerLimitReference equal to 'STD'.";
        }

        public override string Comments()
        {
            return "If the unit of measurement has the value 'FL' or 'SM', then the attribute CODE_DIST_VER_LOWER must have the value 'STD' (standard pressure).";
        }

        public override string Name()
        {
            return "standard_lower_limit_reference_for_FL_SM";
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
