
using System;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule885 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as Localizer;

            //"Each Localizer must have trueBearingAccuracy equal to 0.01 degree";

            if (item?.TrueBearingAccuracy != null)
            {
                return Math.Abs(item.TrueBearingAccuracy.Value - 0.01) < Epsilon();
            }
            return false;
        }

        public override Type GetApplicableType()
        {
            return typeof (Localizer);
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
            return "Each Localizer must have trueBearingAccuracy equal to 0.01 degree";
        }

        public override string Comments()
        {
            return "Aeronautical Data requirements Localizer.trueBearingAccuracy 0.01 degree";
        }

        public override string Name()
        {
            return "Template - Annex 14 Vol1 Table A5.4 AccuracyDecl/MagVar/Bear";
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
