
using System;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule2 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as ElevatedPoint;
            if (item == null) return false;

            //"Each ElevatedPoint that has a verticalAccuracy must have an elevation";

            var verticalAccuracy = item.VerticalAccuracy;
            if (verticalAccuracy != null)
            {
                return item.Elevation != null;
            }
            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof (ElevatedPoint);
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
            return "Each ElevatedPoint that has a verticalAccuracy must have an elevation";
        }

        public override string Comments()
        {
            return "VAL_ELEV_ACCURACY may be specified only if VAL_ELEV has been specified";
        }

        public override string Name()
        {
            return "generic rule 03 - EP_VerticalAccuracy_Elevation";
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
