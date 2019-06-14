using System;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRuleD6D8A : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule


        public override bool CheckChild(object obj)
        {
            var item = obj as RunwayCentrelinePoint;
            if (item == null) return false;

            //"Each RunwayCentreLinePoint.location.ElevatedPoint must have verticalAccuracy less or equal to 0.25 M";

            if (item.Location!=null)
            {
                if (item.Location.VerticalAccuracy != null)
                {
                    return item.Location.VerticalAccuracy.Uom == UomDistance.M
                           && Math.Abs(item.Location.VerticalAccuracy.Value) - 0.25 < Epsilon();
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
            return RuleSource.ICAOAnnex11;
        }

        public override string Svbr()
        {
            return "It is prohibited that RunwayCentrelinePoint with assigned location.ElevatedPoint.verticalAccuracy value and location.ElevatedPoint.verticalAccuracy.uom equal-to 'M' has location.ElevatedPoint.verticalAccuracy value higher-than 0.25";
        }

        public override string Comments()
        {
            return "The accuracy of the RunwayCentrelinePoint location.ElevatedPoint.verticalAccuracy shall be not less than 0.25";
        }

        public override string Name()
        {
            return "Accuracy (vertical) - RunwayCentrelinePoint";
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