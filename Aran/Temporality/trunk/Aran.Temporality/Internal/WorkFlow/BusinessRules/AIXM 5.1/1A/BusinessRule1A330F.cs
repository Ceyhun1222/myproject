﻿using System;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule1A330F : AbstractBusinessRule
    {
                public override string UID => "AIXM-5.1_RULE-1A330F";

        public override bool CheckChild(object obj)
        {
            var item = obj as GeoBorder;
            return item?.Border?.Geo?.IsEmpty??false;
        }

        public override string Category()
        {
            return  RuleCategory.MinimalDataRule;
        }

        public override string Comments()
        {
            return "For each instance of a feature/object, some properties are mandatory for backwards compatibility reasons with the previous AIXM 4.5 version.";
        }

        public override string GetApplicableProperty()
        {
            return null;
        }

        public override Type GetApplicableType()
        {
            return typeof (GeoBorder);
        }

        public override string Level()
        {
            return ErrorLevel.Error;
        }

        public override string Name()
        {
            return "Minimal feature properties for AIXM 4.5 backwards compatibility";
        }

        public override string Source()
        {
            return RuleSource.AIXM45;
        }

        public override string Svbr()
        {
            return
                "Each GeoBorder shall have assigned border.Curve value";
        }
    }
}