using System;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule1A0BFA : AbstractBusinessRule
    {

        #region Overrides of AbstractBusinessRule

        
        public override bool CheckParent(object obj)
        {
            var item = (dynamic)obj;
            var airSpaceVolume = item.TheAirspaceVolume as AirspaceVolume;
            if (airSpaceVolume?.HorizontalProjection != null)
            {
                return airSpaceVolume.ContributorAirspace == null;
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
            return "It is prohibited that AirspaceVolume with assigned horizontalProjection value has assigned contributorAirspace value";
        }

        public override string Comments()
        {
            return "An AirspaceVolume cannot have in the same time horizontalProjection, centreline and contributorAirspace";
        }

        public override string Name()
        {
            return "AirspaceVolume unique geometry";
        }

        public override string Category()
        {
            return RuleCategory.DataConsistencyRule;
        }

        public override string Level()
        {
            return ErrorLevel.Error;
        }

        #endregion
    }
}