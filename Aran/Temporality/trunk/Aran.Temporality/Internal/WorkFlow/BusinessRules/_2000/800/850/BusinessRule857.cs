
using System;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule857 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as RadarEquipment;
            if (item == null) return false;

            //"Each RadarEquipment that has rangeAccuracy must have range and range.uom = rangeAccuracy.uom";

            if (item.RangeAccuracy!=null)
            {
                return item.Range != null && item.Range.Uom == item.RangeAccuracy.Uom;
            }

            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof (RadarEquipment);
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
            return "Each RadarEquipment that has rangeAccuracy must have range and range.uom = rangeAccuracy.uom";
        }

        public override string Comments()
        {
            return
                "RadarEquipment that has a value in rangeAccuracy must have a value in the corresponding range. uom must match.";
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