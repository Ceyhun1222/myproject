
using System;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule826 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as RunwayCentrelinePoint;
            if (item == null) return false;

            //"Each RunwayCentreLinePoint.location.verticalAccuracy must have a resolution matching 1/10 M or FT";

            if (item.Location != null)
            {
                if (item.Location.VerticalAccuracy != null)
                {
                    if (item.Location.VerticalAccuracy.Uom == UomDistance.M)
                    {
                        return Math.Abs(item.Location.VerticalAccuracy.Value * 10 % 1 - 0) < Epsilon();
                    }
                    if (item.Location.VerticalAccuracy.Uom == UomDistance.FT)
                    {
                        return Math.Abs(item.Location.VerticalAccuracy.Value % 1 - 0) < Epsilon();
                    }
                    return false;
                }
                return false;
            }

            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof(RunwayCentrelinePoint);
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
            return "Each RunwayCentreLinePoint.location.verticalAccuracy must have a resolution matching 1/10 M or FT";
        }

        public override string Comments()
        {
            return "Each RunwayCentreLinePoint.verticalAccuracy must have a resolution matching 1/10 M or FT";
        }

        public override string Name()
        {
            return "Template - Annex 15 Table A7-2 - Decimal Elevation/altitude";
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
