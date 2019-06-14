using System;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule1B1D6D : AbstractBusinessRule
    {
                public override string UID => "AIXM-5.1_RULE-1B1D6D";

        public override bool CheckChild(object obj)
        {
            var item = obj as DME;
            if (item == null) return false;

            var navaids = Load(FeatureType.Navaid);
            return
                navaids.Exists(
                    feature =>
                        (feature as Navaid)?.NavaidEquipment?.Exists(
                            navaidComponent => navaidComponent?.TheNavaidEquipment?.Identifier == item.Identifier) ?? false);

        }

        public override string Category()
        {
            return  RuleCategory.Standard;
        }

        public override string Comments()
        {
            return "A Navaid service must be defined for each DME equipment";
        }

        public override string GetApplicableProperty()
        {
            return null;
        }

        public override Type GetApplicableType()
        {
            return typeof (DME);
        }

        public override string Level()
        {
            return ErrorLevel.Error;
        }

        public override string Name()
        {
            return "NavaidEquipment mandatory used by Navaid";
        }

        public override string Source()
        {
            return RuleSource.AIXMModel;
        }

        public override string Svbr()
        {
            return
                "It is prohibited that at least one DME and not Navaid.navaidEquipment.NavaidComponent.theNavaidEquipment resolves-into NavaidEquipment specialisation DME";
        }
    }
}