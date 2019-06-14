
using System;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule918 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as CallsignDetail;

            //"Each CallsignDetail must have callsign";
            return item?.CallSign != null;

        }

        public override Type GetApplicableType()
        {
            return typeof (CallsignDetail);
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
            return "Each CallsignDetail must have callsign";
        }

        public override string Comments()
        {
            return "CallsignDetail has mandatory attribute callsign";
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
