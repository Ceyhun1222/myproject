
using System;
using Aran.Aim;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule954 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as Runway;
            if (item == null) return false;

            if (item.AssociatedAirportHeliport != null)
            {
                var airport = Load(FeatureType.AirportHeliport, item.AssociatedAirportHeliport) as AirportHeliport;
                if (airport == null) return false;
                return airport.Type != CodeAirportHeliport.AD;
            }

            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof (Runway);
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
            return
                "Each Runway that has type equal to 'FATO' must have associatedAirportHeliport.type not equal to 'AD'.";
        }

        public override string Comments()
        {
            return "A FATO cannot be attached to an AirportHeliport of type 'AD' [JLL] Replaces Rule 63";
        }

        public override string Name()
        {
            return "FATO_AHP_AD";
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
