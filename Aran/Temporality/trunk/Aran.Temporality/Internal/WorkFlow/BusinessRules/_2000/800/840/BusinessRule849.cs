
using System;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule849 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as PrecisionApproachRadar;
            if (item == null) return false;

            //"Each PrecisionApproachRadar that has slopeAccuracy must have slope and slope.uom = slopeAccuracy.uom";
            if (item.SlopeAccuracy!=null)
            {
                return item.Slope != null;
            }

            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof(PrecisionApproachRadar);
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
            return
                "Each PrecisionApproachRadar that has slopeAccuracy must have slope and slope.uom = slopeAccuracy.uom";
        }

        public override string Comments()
        {
            return
                "PrecisionApproachRadar that has a value in slopeAccuracy must have a value in the corresponding slope. uom must match.";
        }

        public override string Name()
        {
            return "Template - Mandatory value when accuracy present";
        }

        public override string Category()
        {
            return "Recommended practice";
        }

        public override string Level()
        {
            return ErrorLevel.Error;
        }

        #endregion
    }
}
