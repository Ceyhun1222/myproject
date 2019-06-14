
using System;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule875 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as SpecialNavigationStation;
            if (item == null) return false;

            //"Each SpecialNavigationStation.ElevatedPoint.horizontalAccuracy should be at most 1/100 sec";

            if (item.Position!=null)
            {
                if (item.Position.HorizontalAccuracy!=null)
                {
                    if (item.Position.HorizontalAccuracy.Uom==UomDistance.M)
                    {
                        return item.Position.HorizontalAccuracy.Value <= 0.3;
                    }
                    if (item.Position.HorizontalAccuracy.Uom==UomDistance.FT)
                    {
                        return item.Position.HorizontalAccuracy.Value <= 1;
                    }
                    return false;
                }
                return false;
            }
            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof (SpecialNavigationStation);
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
            return "Each SpecialNavigationStation.ElevatedPoint.horizontalAccuracy should be at most 1/100 sec";
        }

        public override string Comments()
        {
            return "VAL_GEO_ACCURACY should be of at least 1/100 sec ";
        }

        public override string Name()
        {
            return "Rule 711";
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
