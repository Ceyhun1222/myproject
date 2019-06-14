using System;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
	class BusinessRule19F870 : AbstractBusinessRule
	{
				public override string UID => "AIXM-5.1_RULE-19F870";

		public override bool CheckChild ( object obj )
		{
			var item = obj as Airspace;
			if ( item == null )
				return false;

			if ( ( item.GeometryComponent?.Count ?? 0 ) <= 1 )
				return true;
			return !item.GeometryComponent.Exists ( s => s.Operation == null );
		}

		public override string Category ( )
		{
			return RuleCategory.MinimalDataRule;
		}

		public override string Comments ( )
		{
			return "If Airspace has more than one geometryComponent, then the operation for each AirspaceGeometryComponent must be defined";
		}

		public override string GetApplicableProperty ( )
		{
			return null;
		}

		public override Type GetApplicableType ( )
		{
			return typeof ( Airspace );
		}

		public override string Level ( )
		{
			return ErrorLevel.Error;
		}

		public override string Name ( )
		{
			return "Airspace aggregation - operation mandatory";
		}

		public override string Source ( )
		{
			return RuleSource.AIXM45;
		}

		public override string Svbr ( )
		{
			return
				"It is prohibited that Airspace with more than one geometryComponent has not assigned geometryComponent.AirspaceGeometryComponent.operation value";
		}
	}
}
