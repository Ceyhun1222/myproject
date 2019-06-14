
using System;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule890 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as Runway;

            //"Each Runway must have lengthAccuracy equal to 1 M";

             if (item?.LengthAccuracy != null)
            {
                return item.LengthAccuracy.Uom == UomDistance.M && Math.Abs(item.LengthAccuracy.Value - 1) < Epsilon();
            }
            return false;
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
            return "Each Runway must have lengthAccuracy equal to 1 M";
        }

        public override string Comments()
        {
            return "Aeronautical Data requirements Runway.lengthAccuracy 1 M";
        }

        public override string Name()
        {
            return "Template - Annex 14 Vol1 Table A5.5 AccuracyDecl/MagVar/Bear";
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
