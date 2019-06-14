
using System;
using Aran.Aim;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule952 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as Runway;
            if (item == null) return false;

            //"Each Runway that has type='RWY' must have associatedAirportHeliport.type not equal to 'HP'";

            if (item.Type==CodeRunway.RWY)
            {
                var airport = Load(FeatureType.AirportHeliport, item.AssociatedAirportHeliport) as AirportHeliport;
                if (airport == null) return false;
                return airport.Type != CodeAirportHeliport.HP;
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
            return "Each Runway that has type='RWY' must have associatedAirportHeliport.type not equal to 'HP'";
        }

        public override string Comments()
        {
            return "If CODE_TYPE = 'HP', there may not exist any runway specified for the heliport (no relationship to RWY is allowed). [JLL]Replaces rule 64";
        }

        public override string Name()
        {
            return "RWY_not_related_to_HP";
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
