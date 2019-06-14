
using System;
using Aran.Aim;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule955 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as TouchDownLiftOff;
            if (item == null) return false;

            //"Each TouhcDownLiftOff must have associatedAirportHeliport.type not equal to 'AD'.";

            if (item.AssociatedAirportHeliport!=null)
            {
                var airport = Load(FeatureType.AirportHeliport, item.AssociatedAirportHeliport) as AirportHeliport;
                if (airport==null) return false;
                return airport.Type != CodeAirportHeliport.AD;
            }
            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof (TouchDownLiftOff);
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
            return "Each TouhcDownLiftOff must have associatedAirportHeliport.type not equal to 'AD'.";
        }

        public override string Comments()
        {
            return "A TLOF cannot be attached to an AirportHeliport of type 'AD' [JLL] Replaces Rule 63";
        }

        public override string Name()
        {
            return "TLOF_AHP_AD";
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
