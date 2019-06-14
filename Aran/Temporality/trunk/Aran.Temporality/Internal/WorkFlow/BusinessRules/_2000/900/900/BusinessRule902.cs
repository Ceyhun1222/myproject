
using System;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule902 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as RunwayDirection;

            //"Each RunwayDirection.designator must start with the 2 most significant digits of RunwayDirection.magneticBearing";
        
            if (item?.MagneticBearing == null) return false;
            if (item.MagneticBearing.ToString().Length<2) return false;

            var stringValue=((int)((item.MagneticBearing+5)/10)).ToString();
            if (stringValue.Length<2)
            {
                stringValue = "0"+stringValue;
            }
            if (stringValue == "00") stringValue = "36";

            return item.Designator.StartsWith(stringValue.Substring(0, 2));
        }

        public override Type GetApplicableType()
        {
            return typeof (RunwayDirection);
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
                "Each RunwayDirection.designator must start with the 2 most significant digits of RunwayDirection.magneticBearing";
        }

        public override string Comments()
        {
            return "The 2 digits used for TXT_DESIG must match the VAL_MAG_BRG value if specified.";
        }

        public override string Name()
        {
            return "Rule 644";
        }

        public override string Category()
        {
            return "Coding rule";
        }

        public override string Level()
        {
            return ErrorLevel.Error;
        }

        #endregion
    }
}
