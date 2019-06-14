using System;
using System.Linq;
using Aran.Aim;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule38A40 : AbstractBusinessRule
    {

        #region Overrides of AbstractBusinessRule

        
        public override bool CheckChild(object obj)
        {
            var item = obj as Runway;
            if (item == null) return false;

            if (item.TimeSlice.SequenceNumber == 1)
                return true;

            var all = LoadAll(FeatureType.Runway, item.Identifier).Cast<Runway>().ToList();
            var minSequence = all.Select(t => t.TimeSlice.SequenceNumber).Min();
            var first = all.First(t => t.TimeSlice.SequenceNumber == minSequence);
            if (first == null)
                return false;

            return first.Type != CodeRunway.RWY || all.TrueForAll(t => t.Type == CodeRunway.RWY);
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
            return RuleSource.AIXMModel;
        }

        public override string Svbr()
        {
            return "It is prohibited that a Runway has timeSlice[1].RunwayTimeSlice.type equal-to 'RWY' and has timeSlice[2].RunwayTimeSlice.type not equal-to  ('RWY')";
        }

        public override string Comments()
        {
            return "The Runway type cannot change to a value that is not backwards mapped in EAD";
        }

        public override string Name()
        {
            return "Runway type change (in EAD)";
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