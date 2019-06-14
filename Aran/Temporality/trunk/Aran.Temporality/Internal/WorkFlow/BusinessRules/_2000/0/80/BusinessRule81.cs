
using System;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule81 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as FASDataBlock;
            if (item == null) return false;

            //"Each UomDistanceType should be 'KM' or 'M' or 'CM'";
     
            if (item.LengthOffset!=null)
            {
                return item.LengthOffset.Uom == UomDistance.M ||
                       item.LengthOffset.Uom == UomDistance.KM ||
                       item.LengthOffset.Uom == UomDistance.CM ;
            }

            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof (FASDataBlock);
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
            return "Each UomDistanceType should be 'KM' or 'M' or 'CM'";
        }

        public override string Comments()
        {
            return
                "As a standardization requirement, the metric system shall be used for linear measurements. UOM shall be KM or M[ Standard Source : EUROCAE ED-99A, paragraph 3.1.5]";
        }

        public override string Name()
        {
            return "Linear_Measurements_in_M_or_KM";
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
