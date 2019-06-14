
using System;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule984 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as TouchDownLiftOffSafeArea;
            if (item == null) return false;

            if (item.Width!=null)
            {
                return (item.Width.Uom == UomDistance.M || item.Width.Uom == UomDistance.FT) &&
                    //no fractional part
                    item.Width.Value % 1< Epsilon();
            }
            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof (TouchDownLiftOffSafeArea);
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
            return "Each AirportHeliportProtectionArea.width should have a resolution matching 1m";
        }

        public override string Comments()
        {
            return "LANDING_PROTECTION_AREA VAL_WID should be published with a resolution of 1 m or 1 ft[Standard - Source: ICAO annex 15, Appendix 7 - 4]";
        }

        public override string Name()
        {
            return "LANDING_PROTECTION_AREA_RES_1M_1FT";
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
