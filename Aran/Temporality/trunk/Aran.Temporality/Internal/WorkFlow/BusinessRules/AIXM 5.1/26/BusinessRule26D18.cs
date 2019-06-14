using System;
using System.Linq;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    class BusinessRule26D18 : AbstractBusinessRule
    {

        public override bool CheckChild ( object obj )
        {
            var item = obj as Airspace;
            if ( item == null )
                return false;

            if ((item.GeometryComponent?.Count ?? 0) > 1)
                return item.GeometryComponent.Sum(t => (t.Operation == CodeAirspaceAggregation.BASE) ? 1 : 0) == 1;
            return true;
        }

        public override string Category ( )
        {
            return RuleCategory.DataConsistencyRule;
        }

        public override string Comments ( )
        {
            return "If Airspace has more than one geometryComponent, then there must exist exactly one AirspacegeometryComponent with operation = 'BASE'";
        }

        public override string GetApplicableProperty ( )
        {
            return null;
        }

        public override Type GetApplicableType ( )
        {
            return typeof ( Airspace );
        }

        public override string Level ( )
        {
            return ErrorLevel.Error;
        }

        public override string Name ( )
        {
            return "Airspace aggregation - one BASE component";
        }

        public override string Source ( )
        {
            return RuleSource.AIXM45;
        }

        public override string Svbr ( )
        {
            return
                "It is obligatory that each Airspace with more than one geometryComponent has exactly one geometryComponent.AirspaceGeometryComponent.operation equal-to 'BASE'";
        }
    }
}