
using System;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule855 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as AirportHeliport;
            if (item == null) return false;

            //"Each AirportHeliport that has fieldElevationAccuracy must have fieldElevation and fieldElevation.uom = fieldElevationAccuracy.uom";
            if (item.FieldElevationAccuracy!=null)
            {
                return item.FieldElevation != null && item.FieldElevation.Uom == item.FieldElevationAccuracy.Uom;
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
                "Each AirportHeliport that has fieldElevationAccuracy must have fieldElevation and fieldElevation.uom = fieldElevationAccuracy.uom";
        }

        public override string Comments()
        {
            return
                "AirportHeliport that has a value in fieldElevationAccuracy must have a value in the corresponding fieldElevation. uom must match.";
        }

        public override string Name()
        {
            return "Template - Mandatory value when accuracy present";
        }

        public override string Category()
        {
            return "Recommended practice";
        }

        public override string Level()
        {
            return ErrorLevel.Error;
        }

        #endregion
    }
}
