
using System;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule950 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as AirportHeliport;

            //"Each AirportHeliport must have a referenceTemperature.";

            return item?.ReferenceTemperature != null;
        }

        public override Type GetApplicableType()
        {
            return typeof (AirportHeliport);
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
            return "Each AirportHeliport must have a referenceTemperature.";
        }

        public override string Comments()
        {
            return "An aerodrome reference temperature shall be determined for an aerodrome in degrees Celsius";
        }

        public override string Name()
        {
            return "AHP_refTemp_mandatory";
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
