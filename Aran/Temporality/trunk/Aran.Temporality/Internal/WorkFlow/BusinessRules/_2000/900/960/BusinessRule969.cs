
using System;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule969 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as Runway;
            if (item == null) return false;

            //"Each Runway.widthStrip must have a resolution matching 1 M or  FT";

            if (item.WidthStrip!=null)
            {
                if (item.WidthStrip.Uom==UomDistance.M)
                {
                    return item.WidthStrip.Value%1 < Epsilon();
                }
                if (item.WidthStrip.Uom==UomDistance.FT)
                {
                    return item.WidthStrip.Value%1 < Epsilon();
                }
                return false;
            }
            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof (Runway);
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
            return "Each Runway.widthStrip must have a resolution matching 1 M or FT";
        }

        public override string Comments()
        {
            return "Each Runway.widthStrip must have a resolution matching 1 M or FT";
        }

        public override string Name()
        {
            return "Template - Annex 15 Table A7-5 - int resolution - Length/distance/dimension";
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
