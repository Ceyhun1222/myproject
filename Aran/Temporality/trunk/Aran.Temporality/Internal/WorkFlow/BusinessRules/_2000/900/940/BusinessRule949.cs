
using System;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule949 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as AirportHeliport;
            if (item == null) return false;

            //"Each AirportHeliport that has a referenceTemperature must have referenceTemperature.uom equal to 'C'";

            if (item.ReferenceTemperature!=null)
            {
                return item.ReferenceTemperature.Uom == UomTemperature.C;
            }

            return true;
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
            return
                "Each AirportHeliport that has a referenceTemperature must have referenceTemperature.uom equal to 'C'";
        }

        public override string Comments()
        {
            return "An aerodrome reference temperature shall be determined for an aerodrome in degrees Celsius";
        }

        public override string Name()
        {
            return "AHP_refTemp_in_degree_C";
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
