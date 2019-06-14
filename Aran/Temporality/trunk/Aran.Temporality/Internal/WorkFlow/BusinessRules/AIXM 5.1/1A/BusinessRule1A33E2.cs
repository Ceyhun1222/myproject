using System;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
	class BusinessRule1A33E2 : AbstractBusinessRule
	{
				public override string UID => "AIXM-5.1_RULE-1A33E2";

		public override bool CheckParent(object obj)
		{
			var item = obj as AirspaceVolume;
			if (item == null)
				return false;
		    if (item.ContributorAirspace != null)
		    {
		        return item.ContributorAirspace.TheAirspace != null &&
		               Load(FeatureType.Airspace, item.ContributorAirspace.TheAirspace) != null;

		    }
			return true;
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
			return nameof(AirspaceVolume.ContributorAirspace);
		}

		public override Type GetApplicableType()
		{
		    return null;
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
				"Each AirspaceVolumeDependency shall have assigned theAirspace value";
		}
	}
}