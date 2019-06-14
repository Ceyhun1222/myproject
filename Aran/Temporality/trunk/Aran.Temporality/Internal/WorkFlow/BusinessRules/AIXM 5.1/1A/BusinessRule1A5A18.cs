using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Aim;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule1A5A18 : AbstractBusinessRule
    {

        #region Overrides of AbstractBusinessRule

        
        public override bool CheckParent(object obj)
        {
            var item = (dynamic)obj;
            var geometryComponent = item.GeometryComponent as List<AirspaceGeometryComponent>;
            if (geometryComponent != null )
            {
                return geometryComponent.All(component => component.Operation != CodeAirspaceAggregation.BASE || component.OperationSequence == 1);
            }
            return true;
        }


        public override Type GetApplicableType()
        {
            return null;
        }

        public override string GetApplicableProperty()
        {
            return nameof(Airspace.GeometryComponent);
        }

        public override string Source()
        {
            return RuleSource.AIXM45BR;
        }

        public override string Svbr()
        {
            return " Each AirspaceGeometryComponent with operation equal-to 'BASE' shall have operationSequence equal-to 1";
        }

        public override string Comments()
        {
            return "An AirspaceGeometryComponent that is used as BASE for an aggregation shall have operationSequence equal to 1";
        }

        public override string Name()
        {
            return "Airspace aggregation - BASE component sequence";
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