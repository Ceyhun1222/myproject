
using System;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule880 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as AircraftStand;
            if (item == null) return false;
            //"Each AircraftStand.location.ElevatedPoint must have horizontalAccuracy greater or equal to 0.5 M";

            if (item.Location!=null)
            {
                if (item.Location.HorizontalAccuracy!=null)
                {
                    return item.Location.HorizontalAccuracy.Uom == UomDistance.M
                           && Math.Abs(item.Location.HorizontalAccuracy.Value) - 0.5 > -Epsilon();
                }
                return false;
            }
            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof (AircraftStand);
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
            return "Each AircraftStand.location.ElevatedPoint must have horizontalAccuracy equal to 0.5 M";
        }

        public override string Comments()
        {
            return "Aeronautical Data requirements AircraftStand.location.ElevatedPoint.horizontalAccuracy 0.5 M";
        }

        public override string Name()
        {
            return "Template - Annex 14 Vol1 Table A5-1 Accuracy Lat/long/Alt/Elev/Height ";
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
