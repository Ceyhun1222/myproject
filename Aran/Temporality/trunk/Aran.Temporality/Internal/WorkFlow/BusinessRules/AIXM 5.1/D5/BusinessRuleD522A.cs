using System;
using Aran.Aim;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRuleD522A : AbstractBusinessRule
    {

        #region Overrides of AbstractBusinessRule


        public override bool CheckChild(object obj)
        {
            var item = obj as Navaid;
            if (item == null) return false;

            if (item.Type == CodeNavaidService.DME)
            {
                if (item.NavaidEquipment == null)
                    return true;
                foreach (var equipment in item.NavaidEquipment)
                {
                    if (equipment.ProvidesNavigableLocation == true)
                    {
                        var navaid = Load(FeatureType.DME, equipment.TheNavaidEquipment);
                        if (navaid == null) return false;
                    }
                }
            }
            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof (Navaid);
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
            return "It is prohibited that a NavaidComponent is-property-of Navaid with type equal-to 'DME' and NavaidComponent has providesNavigableLocation equal-to 'YES' and not NavaidComponent.theNavaidEquipment resolves-into NavaidEquipment specialisation DME";
        }

        public override string Comments()
        {
            return "Navaid with type 'DME' cannot have its navigable location provided by a NavaidEquipment different from DME";
        }

        public override string Name()
        {
            return "Navaid navigable location equipment";
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