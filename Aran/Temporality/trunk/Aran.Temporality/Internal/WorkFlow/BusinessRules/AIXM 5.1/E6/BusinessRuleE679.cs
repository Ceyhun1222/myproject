using System;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRuleE679 : AbstractBusinessRule
    {

        #region Overrides of AbstractBusinessRule


        public override bool CheckChild(object obj)
        {
            var item = obj as AirportHeliport;
            if (item == null) return false;

            //"Each AirportHeliport.ARP.ElevatedPoint must have horizontalAccuracy greater or equal to 30 M";
            if (item.ARP?.HorizontalAccuracy != null)
            {
                return item.ARP.HorizontalAccuracy.Uom != UomDistance.M
                       || Math.Abs(item.ARP.HorizontalAccuracy.Value) - 30 < Epsilon();
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
            return "It is prohibited that a AirportHeliport with ARP.ElevatedPoint.horizontalAccuracy.uom equal-to 'M' has ARP.ElevatedPoint.horizontalAccuracy value higher-than 30M";
        }

        public override string Comments()
        {
            return "The horizontal accuracy of the AirportHeliport ARP shall be better than 30 M";
        }

        public override string Name()
        {
            return "Accuracy (horizontal) AirportHeliport location";
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