
using System;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule882 : AbstractBusinessRule
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
            return RuleSource.AIXM45BR;
        }

        public override string Svbr()
        {
            return "Each RunwayCentreLinePoint.location.ElevatedPoint must have verticalAccuracy equal to 0.25 M";
        }

        public override string Comments()
        {
            return "Aeronautical Data requirements RunwayCentreLinePoint.location.ElevatedPoint.verticalAccuracy 0.25 M";
        }

        public override string Name()
        {
            return "Template - Annex 14 Vol1 Table A5-2 Accuracy Lat/long/Alt/Elev/Height ";
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
