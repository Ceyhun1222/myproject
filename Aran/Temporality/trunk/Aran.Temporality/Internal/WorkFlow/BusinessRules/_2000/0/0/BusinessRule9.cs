
using System;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule9 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckParent(object obj)
        {
            var item = (dynamic) obj;

            //"Each  [...].minimumLimit that has an uom equal to 'FL' should have 2 or 3 digits.";
            
            if (item.MinimumLimit.Uom.ToString()=="FL")
            {
                return item.MinimumLimit.Value >= 10 && item.MinimumLimit.Value <= 999;
            }
            return true;
        }

        public override Type GetApplicableType()
        {
            return null;
        }

        public override string GetApplicableProperty()
        {
            return "MinimimLimit";
        }

        public override string Source()
        {
            return RuleSource.AIXM45BR;
        }

        public override string Svbr()
        {
            return "Each  [...].minimumLimit that has an uom equal to 'FL' should have 2 or 3 digits.";
        }

        public override string Comments()
        {
            return "If minimumLimit.UOM = 'FL' (flight level in hundreds of feet)  then it should have 2 or 3 digits.";
        }

        public override string Name()
        {
            return "generic rule 27 - Minimum_Limit_UOM_FL_2_3_digits";
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
