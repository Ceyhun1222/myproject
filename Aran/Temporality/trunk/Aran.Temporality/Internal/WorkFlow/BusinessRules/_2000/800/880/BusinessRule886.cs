
using System;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule886 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as AirportHeliport;

            //"Each AirportHeliport must have magneticVariationAccuracy equal to 1 degree";

            if (item?.MagneticVariationAccuracy != null)
            {
                return Math.Abs(item.MagneticVariationAccuracy.Value - 1) < Epsilon();
            }
            return false;
        }

        public override Type GetApplicableType()
        {
            return typeof(AirportHeliport);
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
            return "Each AirportHeliport must have magneticVariationAccuracy equal to 1 degree";
        }

        public override string Comments()
        {
            return "Aeronautical Data requirements AirportHeliport.magneticVariationAccuracy 1 degree";
        }

        public override string Name()
        {
            return "Template - Annex 14 Vol1 Table A5.3 AccuracyDecl/MagVar/Bear";
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
