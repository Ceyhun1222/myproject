
using System;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule930 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as FlightRestrictionLevel;

            //"Each FlightRestrictionLevel must have lowerLevelReference";
            return item?.LowerLevelReference != null;

        }

        public override Type GetApplicableType()
        {
            return typeof (FlightRestrictionLevel);
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
            return "Each FlightRestrictionLevel must have lowerLevelReference";
        }

        public override string Comments()
        {
            return "FlightRestrictionLevel has mandatory attribute lowerLevelReference";
        }

        public override string Name()
        {
            return "Template - Mandatory objects from AIXM4.5";
        }

        public override string Category()
        {
            return "Minimal data rule";
        }

        public override string Level()
        {
            return ErrorLevel.Error;
        }

        #endregion
    }
}
