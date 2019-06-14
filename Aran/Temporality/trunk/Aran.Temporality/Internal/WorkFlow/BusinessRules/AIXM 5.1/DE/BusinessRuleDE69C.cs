using System;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRuleDE69C : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        
        public override bool CheckChild(object obj)
        {
            var item = obj as RunwayCentrelinePoint;
            if (item == null) return false;

            //"Each RunwayCentreLinePoint.location.ElevatedPoint must have horizontalAccuracy less or equal to 1.0 M";

            if (item.Location!=null)
            {
                if (item.Location.HorizontalAccuracy != null)
                {
                    return item.Location.HorizontalAccuracy.Uom == UomDistance.M
                           && Math.Abs(item.Location.HorizontalAccuracy.Value) - 1.0 < Epsilon();
                }
                return false;
            }

            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof (RunwayCentrelinePoint);
        }

        public override string GetApplicableProperty()
        {
            return null;
        }

        public override string Source()
        {
            return RuleSource.ICAOAnnex14;
        }

        public override string Svbr()
        {
            return "It is prohibited that an RunwayCentrelinePoint with assigned location.ElevatedPoint.horizontalAccuracy has location.ElevatedPoint.horizontalAccuracy.uom equal-to 'M' and location.ElevatedPoint.horizontalAccuracy higher-than 1.0";
        }

        public override string Comments()
        {
            return "The accuracy of the RunwayCentrelinePoint location shall be better than 1.0 M";
        }

        public override string Name()
        {
            return "Accuracy (horizontal) - RunwayCentrelinePoint location (Annex 14)";
        }

        public override string Category()
        {
            return RuleCategory.Standard;
        }

        public override string Level()
        {
            return ErrorLevel.Warning;
        }

        #endregion
    }
}