
using System;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule4 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule
        //Each ElevatedPoint that has a geoidUndulation must have an elevation
        public override bool CheckChild(object obj)
        {
            var item = obj as ElevatedPoint;
            if (item == null) return false;

            if (item.GeoidUndulation != null)
                return item.Elevation!=null;
            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof(ElevatedPoint);
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
            return "Each ElevatedPoint that has a geoidUndulation must have an elevation";
        }

        public override string Comments()
        {
            return "VAL_GEOID_UNDULATION may be specified only if VAL_ELEV has been specified";
        }

        public override string Name()
        {
            return "generic rule 06 - EP_GeoidUndulation_Elevation";
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