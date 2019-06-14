
using System;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule938 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as ProcedureAvailability;

            //"Each ProcedureAvailability must have status";
            return item?.Status != null;

        }

        public override Type GetApplicableType()
        {
            return typeof (ProcedureAvailability);
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
            return "Each ProcedureAvailability must have status";
        }

        public override string Comments()
        {
            return "ProcedureAvailability has mandatory attribute status";
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
