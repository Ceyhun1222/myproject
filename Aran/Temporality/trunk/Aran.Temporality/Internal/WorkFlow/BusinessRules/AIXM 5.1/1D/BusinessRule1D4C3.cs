using System;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule1D4C3 : AbstractBusinessRule
    {

        public override bool CheckParent(object obj)
        {
            var item = (dynamic)obj;
            var airSpaceVolume = item.TheAirspaceVolume as AirspaceVolume;
            if (airSpaceVolume?.ContributorAirspace?.Dependency == CodeAirspaceDependency.FULL_GEOMETRY)
            {
                return airSpaceVolume.LowerLimit == null;
            }
            return true;
        }


        public override Type GetApplicableType()
        {
            return null;
        }

        public override string GetApplicableProperty()
        {
            return nameof(AirspaceGeometryComponent.TheAirspaceVolume);
        }

        public override string Source()
        {
            return RuleSource.AIXM45BR;
        }

        public override string Svbr()
        {
            return "Each AirspaceVolume with contributorAirspace.AirspaceVolumeDependency.dependency value equal-to 'FULL_GEOMETRY' shall not have assigned lowerLimit";
        }

        public override string Comments()
        {
            return "An AirspaceVolume that has a geometry defined as a full copy of the geometry of another Airspace shall not have vertical limits (because it re-uses the vertical limits of the contributor Airspace).";
        }

        public override string Name()
        {
            return "AirspaceVolume with full geometry dependency - no vertical limits";
        }

        public override string Category()
        {
            return RuleCategory.DataConsistencyRule;
        }

        public override string Level()
        {
            return ErrorLevel.Error;
        }

    }
}