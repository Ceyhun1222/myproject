using System;
using System.Linq;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules._2000._2000._2000
{
    class BusinessRule2001 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as Route;
            if (item == null) return false;

            var segments=Context.GetLinks(false, FeatureType.RouteSegment, item, "RouteFormed");
            if (segments == null || segments.Count == 0)
            {
                return false;
            }

            return segments.Cast<RouteSegment>().All(segment => segment.End != null && segment.Start != null);
        }

        public override Type GetApplicableType()
        {
            return typeof(Route);
        }

        public override string GetApplicableProperty()
        {
            return null;
        }

        public override string Source()
        {
            return "Data integrity";
        }

        public override string Svbr()
        {
            return "Each Route must have RouteSegments with Start and End points";
        }

        public override string Comments()
        {
            return "Each Route must have RouteSegments with Start and End points";
        }

        public override string Name()
        {
            return "Route segments";
        }

        public override string Category()
        {
            return "Special rule";
        }

        public override string Level()
        {
            return ErrorLevel.Error;
        }

        #endregion
    }

}
