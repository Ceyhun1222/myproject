
using System;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule843 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as Azimuth;
            if (item == null) return false;

            //"Each Azimuth that has trueBearingAccuracy must have trueBearing and trueBearing.uom = trueBearingAccuracy.uom";
  
            if (item.TrueBearingAccuracy!=null)
            {
                return item.TrueBearing != null;
            }

            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof(Azimuth);
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
                "Each Azimuth that has trueBearingAccuracy must have trueBearing and trueBearing.uom = trueBearingAccuracy.uom";
        }

        public override string Comments()
        {
            return
                "Azimuth that has a value in trueBearingAccuracy must have a value in the corresponding trueBearing. uom must match.";
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