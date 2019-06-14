using System;
using System.Linq;
using Aran.Aim;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRuleCF080 : AbstractBusinessRule
    {

        #region Overrides of AbstractBusinessRule

        
        public override bool CheckChild(object obj)
        {
            var item = obj as Route;
            if (item == null) return false;

            if (item.TimeSlice.SequenceNumber == 1)
                return true;

            var all = LoadAll(FeatureType.Route, item.Identifier).Cast<Route>().ToList();
            var minSequence = all.Select(t => t.TimeSlice.SequenceNumber).Min();
            var first = all.First(t => t.TimeSlice.SequenceNumber == minSequence);
            if (first == null)
                return false;

            return first.Type != CodeRoute.NAT || all.TrueForAll(t => t.Type == CodeRoute.NAT);
        }

        public override Type GetApplicableType()
        {
            return typeof (Route);
        }

        public override string GetApplicableProperty()
        {
            return null;
        }

        public override string Source()
        {
            return RuleSource.AIXMModel;
        }

        public override string Svbr()
        {
            return "It is prohibited that a Route has timeSlice[1].RouteTimeSlice.type equal-to ('NAT') and has timeSlice[2].RouteTimeSlice.type not equal-to  ('NAT')";
        }

        public override string Comments()
        {
            return "The Route type cannot change to a value that is not backwards mapped in EAD";
        }

        public override string Name()
        {
            return "Route type change (in EAD)";
        }

        public override string Category()
        {
            return RuleCategory.DataConsistencyRule;
        }

        public override string Level()
        {
            return ErrorLevel.Error;
        }

        #endregion
    }
}