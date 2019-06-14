
using System;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule983 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as FinalLeg;
            if (item == null) return false;

            //"Each FinalLeg.SignificantPoint.Point.horizontalAccuracy should be at most 1/10 sec";

           if (item.FinalPathAlignmentPoint!=null)
           {
               if (item.FinalPathAlignmentPoint.Position?.HorizontalAccuracy == null) return false;

               switch (item.FinalPathAlignmentPoint.Position.HorizontalAccuracy.Uom)
               {
                   case UomDistance.M:
                       return item.FinalPathAlignmentPoint.Position.HorizontalAccuracy.Value <= 3;
                   case UomDistance.FT:
                       return item.FinalPathAlignmentPoint.Position.HorizontalAccuracy.Value <= 10;
               }
               return false;
           }

            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof (FinalLeg);
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
            return "Each FinalLeg.SignificantPoint.Point.horizontalAccuracy should be at most 1/10 sec";
        }

        public override string Comments()
        {
            return
                "VAL_GEO_ACCURACY should be of at least 1/10 sec if used in a IAP procedure_leg [ Standard  -  Source:   ICAO annex 15, Appendix 7-1]";
        }

        public override string Name()
        {
            return "DESIGNATED_POINT_SP_HOR_ACCURACY_1_TENTH_SEC";
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
