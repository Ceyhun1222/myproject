
using System;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule825 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as DepartureLeg;
            if (item == null) return false;

            //"Each DepartureLeg.length must have a resolution matching 1/100 KM or  NM";
       
            if (item.Length!=null)
            {
                if (item.Length.Uom==UomDistance.KM)
                {
                    return Math.Abs(item.Length.Value*100%1 - 0) < Epsilon();
                }
                if (item.Length.Uom == UomDistance.NM)
                {
                    return Math.Abs(item.Length.Value * 100 % 1 - 0) < Epsilon();
                }
                return false;
            }

            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof (DepartureLeg);
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
            return "Each DepartureLeg.length must have a resolution matching 1/100 KM or  NM";
        }

        public override string Comments()
        {
            return "Each DepartureLeg.length must have a resolution matching 1/100 KM or  NM";
        }

        public override string Name()
        {
            return "Template - Annex 15 Table A7-5 - fract resolution - Length/distance/dimension";
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
