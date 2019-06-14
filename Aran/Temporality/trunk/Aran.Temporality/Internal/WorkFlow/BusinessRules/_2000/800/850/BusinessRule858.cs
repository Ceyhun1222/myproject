
using System;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule858 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as Glidepath;
            if (item == null) return false;

            //"Each GlidePath that has rdhAccuracy must have rdh and rdh.uom = rdhAccuracy.uom";

            if (item.RdhAccuracy!=null)
            {
                return item.Rdh != null && item.Rdh.Uom == item.RdhAccuracy.Uom;
            }

            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof (Glidepath);
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
            return "Each GlidePath that has rdhAccuracy must have rdh and rdh.uom = rdhAccuracy.uom";
        }

        public override string Comments()
        {
            return
                "GlidePath that has a value in rdhAccuracy must have a value in the corresponding rdh. uom must match.";
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
