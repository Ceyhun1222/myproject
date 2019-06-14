
using System;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule959 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as CircleSector;
            if (item == null) return false;

            //"Each CircleSector that has a lowerLimit and that has an upperLimit " +
            if (item.UpperLimit != null && item.LowerLimit != null)
            {
                //"must have CircleSector.lowerLimit that is at most CircleSector.upperLimit.";
            
                return ConverterToSI.Convert(item.LowerLimit, 0) <=
                       ConverterToSI.Convert(item.UpperLimit, 0);
            }

            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof (CircleSector);
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
                "Each CircleSector that has a lowerLimit and that has an upperLimit must have CircleSector.lowerLimit that is at most CircleSector.upperLimit.";
        }

        public override string Comments()
        {
            return
                "If both VAL_DIST_VER_LOWER and VAL_DIST_VER_UPPER are specified, then the value of the lower limit must be smaller than or equal to the value of the upper limit (when converted to a common unit of measurement and reference system)";
        }

        public override string Name()
        {
            return "CIRCLE_SECTOR_LOWER_BELOW_UPPER_LIMIT";
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
