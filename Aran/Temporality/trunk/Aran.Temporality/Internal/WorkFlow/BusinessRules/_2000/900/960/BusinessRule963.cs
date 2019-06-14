
using System;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule963 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as InitialLeg;
            if (item == null) return false;

            //"Each SegmentLeg that has an upperLimitAltitude and a lowerLimitAltitude 
            if (item.UpperLimitAltitude != null && item.LowerLimitAltitude != null)
            {
                //must have SegmentLeg.lowerLimitAltitude less than SegmentLeg.upperLimitAltitude";

                return ConverterToSI.Convert(item.LowerLimitAltitude, 0) <=
                       ConverterToSI.Convert(item.UpperLimitAltitude, 0);

            }
            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof (InitialLeg);
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
                "Each SegmentLeg that has an upperLimitAltitude and a lowerLimitAltitude must have SegmentLeg.lowerLimitAltitude less than SegmentLeg.upperLimitAltitude";
        }

        public override string Comments()
        {
            return
                "When translated to use the same unit of measurement and the same vertical reference, VAL_DIST_VER_LOWER must be lower than VAL_DIST_VER_UPPER [ Data plausibility rule  -  Source: AIXM ]";
        }

        public override string Name()
        {
            return "PROCEDURE_LEG_LOWER_LIMIT_ALT_BELOW_UPPER_LIMIT_ALT";
        }

        public override string Category()
        {
            return "Data plausibility rule";
        }

        public override string Level()
        {
            return ErrorLevel.Error;
        }

        #endregion
    }
}
