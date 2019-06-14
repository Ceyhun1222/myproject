
using System;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule879 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as AirportHeliport;
            if (item == null) return false;

            //"Each AirportHeliport.ARP.ElevatedPoint must have horizontalAccuracy greater or equal to 30 M";
            if (item.ARP!=null)
            {
                if (item.ARP.HorizontalAccuracy!=null)
                {
                    return item.ARP.HorizontalAccuracy.Uom == UomDistance.M
                           && Math.Abs(item.ARP.HorizontalAccuracy.Value) - 30 > -Epsilon();
                }
                return false;
            }

            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof (AirportHeliport);
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
            return "Each AirportHeliport.ARP.ElevatedPoint must have horizontalAccuracy equal to 30 M";
        }

        public override string Comments()
        {
            return "Aeronautical Data requirements AirportHeliport.ARP.ElevatedPoint.horizontalAccuracy 30 M";
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
