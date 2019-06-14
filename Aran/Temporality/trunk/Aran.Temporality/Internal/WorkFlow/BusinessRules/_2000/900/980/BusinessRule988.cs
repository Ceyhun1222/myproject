
using System;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule988 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as RadioFrequencyArea;
            if (item == null) return false;

            //"Each RadioFrequencyArea that has a type equal to 'SCL' must have an angleScallop";

            if (item.Type==CodeRadioFrequencyArea.SCL)
            {
                return item.AngleScallop != null;
            }

            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof (RadioFrequencyArea);
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
            return "Each RadioFrequencyArea that has a type equal to 'SCL' must have an angleScallop";
        }

        public override string Comments()
        {
            return "If CODE_TYPE='SCL' ('scalloping'), then VAL_ANGLE_SCALLOP is mandatory";
        }

        public override string Name()
        {
            return "RFA_SCL_TYPE_REQ_ANGLE_SCALLOP";
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
