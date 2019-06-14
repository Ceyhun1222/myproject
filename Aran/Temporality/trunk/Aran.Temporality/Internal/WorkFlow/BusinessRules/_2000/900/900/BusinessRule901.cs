
using System;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule901 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as VisualGlideSlopeIndicator;
            if (item==null) return false;

            //"Each VisualGlideSlopeIndicator that has a RunwayDirection must have no type equal to 'HAPI'.";

            if (item.RunwayDirection!=null)
            {
                return item.Type != CodeVASIS.HAPI;
            }

            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof (VisualGlideSlopeIndicator);
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
            return "Each VisualGlideSlopeIndicator that has a RunwayDirection must have no type equal to 'HAPI'.";
        }

        public override string Comments()
        {
            return "The value 'HAPI' (helicopter approach path indicator) cannot be used here";
        }

        public override string Name()
        {
            return "VisualGlideSlopeIndicator_No_RunwayDirection";
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
