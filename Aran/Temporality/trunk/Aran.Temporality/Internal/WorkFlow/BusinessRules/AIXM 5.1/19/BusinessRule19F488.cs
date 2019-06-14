using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;
using System;
using Aran.Aim.Features;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule19F488 : AbstractBusinessRule
    {

                public override string UID => "AIXM-5.1_RULE-19F488";


        public override bool CheckChild(object obj)
        {
            var item = obj as Airspace;
            if (item == null) return false;

            if ((item.GeometryComponent?.Count ?? 0) <= 1) return true;
            return !item.GeometryComponent.Exists(s => s.OperationSequence == null);
        }
        
        public override string GetApplicableProperty()
        {
            return null;
        }

        public override Type GetApplicableType()
        {
            return typeof (Airspace);
        }

        public override string Level()
        {
            return ErrorLevel.Error;
        }

        public override string Name()
        {
            return "Airspace aggregation - sequence mandatory";
        }

        public override string Source()
        {
            return RuleSource.AIXMModel;
        }

        public override string Svbr()
        {
            return "It is prohibited that Airspace with more than one geometryComponent has not assigned geometryComponent.AirspaceGeometryComponent.operationSequence value";
        }

        public override string Category()
        {
            return RuleCategory.DataConsistencyRule;
        }

        public override string Comments()
        {
            return "If Airspace has more than one geometryComponent, then the operation seqeunce for each AirspaceGeometryComponent must be defined";
        }
    }
}
