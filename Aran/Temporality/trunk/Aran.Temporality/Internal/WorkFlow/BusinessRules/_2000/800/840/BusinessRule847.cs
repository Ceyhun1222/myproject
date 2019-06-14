
using System;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule847 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as Localizer;
            if (item == null) return false;

            //"Each Localizer that has magneticBearingAccuracy must have magneticBearing and magneticBearing.uom = magneticBearingAccuracy.uom";
       
            if (item.MagneticBearingAccuracy!=null)
            {
                return item.MagneticBearing != null;
            }

            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof(Localizer);
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
                "Each Localizer that has magneticBearingAccuracy must have magneticBearing and magneticBearing.uom = magneticBearingAccuracy.uom";
        }

        public override string Comments()
        {
            return
                "Localizer that has a value in magneticBearingAccuracy must have a value in the corresponding magneticBearing. uom must match.";
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
