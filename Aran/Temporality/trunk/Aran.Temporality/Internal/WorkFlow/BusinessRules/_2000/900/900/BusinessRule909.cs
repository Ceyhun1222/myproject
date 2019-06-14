
using System;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule909 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as SegmentLeg;
            if (item == null) return false;

            //"Each SegmentLeg that has an altitudeInterpretation equal to 'AT_LOWER' or 'ABOVE_LOWER' or 'BETWEEN' or 'RECOMMENDED' or 'EXPECT_LOWER' must have a lowerLimitAltitude";
     
            if (item.AltitudeInterpretation==CodeAltitudeUse.AT_LOWER || 
                item.AltitudeInterpretation==CodeAltitudeUse.ABOVE_LOWER ||
                item.AltitudeInterpretation==CodeAltitudeUse.BETWEEN ||
                item.AltitudeInterpretation==CodeAltitudeUse.RECOMMENDED ||
                item.AltitudeInterpretation==CodeAltitudeUse.EXPECT_LOWER 
                )
            {
                return item.LowerLimitAltitude != null;
            }

            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof (SegmentLeg);
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
                "Each SegmentLeg that has an altitudeInterpretation equal to 'AT_LOWER' or 'ABOVE_LOWER' or 'BETWEEN' or 'RECOMMENDED' or 'EXPECT_LOWER' must have a lowerLimitAltitude";
        }

        public override string Comments()
        {
            return "If CODE_DESCR_DIST_VER = 'L', 'LA' or 'B' then VAL_DIST_VER_LOWER is mandatory Note: AIXM5.1 also adds \"Recommended\" and \"Expect lower\" values";
        }

        public override string Name()
        {
            return "PROCEDURE_LEG_CODE_DESCR_DIST_VER_L_LA_B_REQ_VAL_DIST_VER_LOWER";
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
