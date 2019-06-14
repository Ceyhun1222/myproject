using System;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    class BusinessRule1A336B : AbstractBusinessRule
    {
                public override string UID => "AIXM-5.1_RULE-1A336B";

        public override bool CheckChild ( object obj )
        {
            var item = obj as RunwayDirection;
            return !string.IsNullOrWhiteSpace(item?.Designator);
        }

        public override string Category ( )
        {
            return RuleCategory.MinimalDataRule;
        }

        public override string Comments ( )
        {
            return "For each instance of a feature/object, some properties are mandatory for backwards compatibility reasons with the previous AIXM 4.5 version.";
        }

        public override string GetApplicableProperty ( )
        {
            return null;
        }

        public override Type GetApplicableType ( )
        {
            return typeof (RunwayDirection);
        }

        public override string Level ( )
        {
            return ErrorLevel.Error;
        }

        public override string Name ( )
        {
            return "Minimal feature properties for AIXM 4.5 backwards compatibility";
        }

        public override string Source ( )
        {
            return RuleSource.AIXM45;
        }

        public override string Svbr ( )
        {
            return
                "Each RunwayDirection shall have assigned designator value";
        }
    }
}