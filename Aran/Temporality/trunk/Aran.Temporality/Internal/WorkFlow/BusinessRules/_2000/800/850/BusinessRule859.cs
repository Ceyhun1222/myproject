
using System;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule859 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as PrecisionApproachRadar;
            if (item == null) return false;

            //"Each PrecisionApproachRadar that has magneticVariationAccuracy 
            
            if (item.MagneticVariationAccuracy!=null)
            {
            //must have magneticVariation and magneticVariation.uom = magneticVariationAccuracy.uom";

                return item.MagneticVariation != null;
            }

            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof (PrecisionApproachRadar);
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
                "Each PrecisionApproachRadar that has magneticVariationAccuracy must have magneticVariation and magneticVariation.uom = magneticVariationAccuracy.uom";
        }

        public override string Comments()
        {
            return
                "PrecisionApproachRadar that has a value in magneticVariationAccuracy must have a value in the corresponding magneticVariation. uom must match.";
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