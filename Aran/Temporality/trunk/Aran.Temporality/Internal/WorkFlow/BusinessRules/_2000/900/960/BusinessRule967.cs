
using System;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule967 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as Azimuth;
            if (item == null) return false;

            //"Each Azimuth.ElevatedPoint.horizontalAccuracy should be at most 1/10sec";


            if (item.Location?.HorizontalAccuracy != null)
            {
                switch (item.Location.HorizontalAccuracy.Uom)
                {
                    case UomDistance.M:
                        return item.Location.HorizontalAccuracy.Value < 3;
                    case UomDistance.FT:
                        return item.Location.HorizontalAccuracy.Value < 10;
                }
                return false;
            }
            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof (Azimuth);
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
            return "Each Azimuth.ElevatedPoint.horizontalAccuracy should be at most 1/10sec";
        }

        public override string Comments()
        {
            return "MLS_AZIMUTH VAL_GEO_ACCURACY should  be of at least 1/10 sec[Standard - Source: ICAO annex  15, Appendix 7 - 1]";
        }

        public override string Name()
        {
            return "MLS_AZIMUTH_GEO_ACCURACY_1_TENTH_SEC";
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
