
using System;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule25 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckParent(object obj)
        {
            //Each  [...] that has a width and a length should have [...].width less than [...].length
            var item = (dynamic)obj;
            try
            {
                if (item.Length != null && item.Width != null)
                {
                    return item.Width < item.Length;
                }
            }
            catch 
            {
            }
            
            return true;
        }

        public override Type GetApplicableType()
        {
            return null;
        }

        public override string GetApplicableProperty()
        {
            return "Width";
        }

        public override string Source()
        {
            return RuleSource.AIXM45BR;
        }

        public override string Svbr()
        {
            return "Each  [...] that has a width and a length should have [...].width less than [...].length";
        }

        public override string Comments()
        {
            return "If both VAL_WID and VAL_LEN are specified, VAL_WID should be lower than VAL_LEN";
        }

        public override string Name()
        {
            return "check_length_greater_than_width";
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
