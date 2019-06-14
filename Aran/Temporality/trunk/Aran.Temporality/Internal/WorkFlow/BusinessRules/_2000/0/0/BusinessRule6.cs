
using System;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule6 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckParent(object obj)
        {
            var item = (dynamic)obj;
            //"If lowerLimit.UOM = 'FL' (flight level in hundreds of feet)  then it should have 2 or 3 digits.";

            var lowerLimit = item.LowerLimit;
            if (lowerLimit != null)
            {
                if (lowerLimit.Uom.ToString() == "FL")
                {
                    var value = lowerLimit.Value;
                    return value >= 10 && value <= 999;
                }
            }

            return true;
        }

        public override Type GetApplicableType()
        {
            return null;
        }

        public override string GetApplicableProperty()
        {
            return "LowerLevel";
        }

        public override string Source()
        {
            return RuleSource.AIXM45BR;
        }

        public override string Svbr()
        {
            return "Each  [...].lowerLevel that has an uom equal to 'FL' should have 2 or 3 digits.";
        }

        public override string Comments()
        {
            return "If lowerLimit.UOM = 'FL' (flight level in hundreds of feet)  then it should have 2 or 3 digits.";
        }

        public override string Name()
        {
            return "generic rule 21 -  Lower_Limit_UOM_FL_2_3_digits";
        }

        public override string Category()
        {
            return "Recommended practice";
        }

        public override string Level()
        {
            return ErrorLevel.Warning;
        }

        #endregion
    }
}
