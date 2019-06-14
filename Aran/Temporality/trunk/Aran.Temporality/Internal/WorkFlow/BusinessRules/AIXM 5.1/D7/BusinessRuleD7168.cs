using System;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRuleD7168 : AbstractBusinessRule
    {

        #region Overrides of AbstractBusinessRule

                public override string UID => "AIXM-5.1_RULE-D7168";

        public override bool CheckChild(object obj)
        {
            var item = obj as Airspace;
            if (item == null) return false;

            return item.GeometryComponent != null && item.GeometryComponent.Count <= 99;
        }

        public override Type GetApplicableType()
        {
            return typeof (Airspace);
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
            return "It is prohibited that an Airspace has at least 100 geometryComponent";
        }

        public override string Comments()
        {
            return "An Airspace cannot have more than 99 AirspaceVolume, in order to enable the automatic creation of the Airspace PART in case of backward mapping to AIXM 4.5.";
        }

        public override string Name()
        {
            return "Airspace aggregation - designator size";
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