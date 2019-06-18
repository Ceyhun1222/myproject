using System;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule426A : AbstractBusinessRule
    {

        #region Overrides of AbstractBusinessRule

                public override string UID => "AIXM-5.1_RULE-426A";

        public override bool CheckChild(object obj)
        {
            var item = obj as Airspace;
            if (item == null) return false;

            if (item.GeometryComponent != null && item.GeometryComponent.Count >= 10
                && item.GeometryComponent.Count <= 99)
            {
                return item.Designator != null && item.Designator.Length <= 9;
            }
            return true;
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
            return "It is prohibited that an Airspace has at least 10  geometryComponent and Airspace has at most 99 geometryComponent and Airspace has designator longer than 8 characters";
        }

        public override string Comments()
        {
            return "In order to enable the automatic creation of PART type Airspace in AIXM 4.5, for an Airspace that has between 10 and 99 AirspaceVolume, the Airspace designator cannot have more than 8 characters.";
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