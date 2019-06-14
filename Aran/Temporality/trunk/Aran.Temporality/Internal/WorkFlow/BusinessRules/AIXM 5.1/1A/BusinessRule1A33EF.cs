using System;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
	class BusinessRule1A33EF : AbstractBusinessRule
	{
				public override string UID => "AIXM-5.1_RULE-1A33EF";

		public override bool CheckChild ( object obj)
		{
			var item = obj as RouteSegment;
			return item?.RouteFormed != null &&
			       Load ( FeatureType.Route, item.RouteFormed ) != null;
		}

		public override string Category()
		{
			return RuleCategory.MinimalDataRule;
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
			return typeof(RouteSegment);
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
				"Each RouteSegment shall have assigned routeFormed value";
		}
	}
}