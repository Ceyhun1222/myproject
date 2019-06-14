
using System;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule830 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as AirportHeliport;
            if (item == null) return false;


            //"Each AirportHeliportTimeSlice that has a fieldElevation must have a verticalDatum";
     
            if (item.FieldElevation!=null)
            {
                return item.VerticalDatum != null;
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
            return "Each AirportHeliportTimeSlice that has a fieldElevation must have a verticalDatum";
        }

        public override string Comments()
        {
            return "Each AirportHeliportTimeSlice that has a fieldElevation must have a verticalDatum";
        }

        public override string Name()
        {
            return "generic rule 53 - AirportHeliportTimeSlice_fieldElevation_verticalDatum";
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
