using System;
using System.Linq;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules._2000._2000._2000
{
    class BusinessRule2003 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as Navaid;
            if (item == null) return false;

            var segmentsStarting = Context.GetLinks(false, FeatureType.RouteSegment, item, "Start/PointChoice/NavaidSystem").Cast<RouteSegment>().ToList();
            var segmentsEnding = Context.GetLinks(false, FeatureType.RouteSegment, item, "End/PointChoice/NavaidSystem").Cast<RouteSegment>().ToList();

            var atc = segmentsStarting.Where(t => t.Start?.ReportingATC != null).Select(t => t.Start.ReportingATC).ToList();
            atc.AddRange(segmentsEnding.Where(t => t.End?.ReportingATC != null).Select(t => t.End.ReportingATC));

            return atc.Distinct().Count() < 2;

        }


        public override Type GetApplicableType()
        {
            return typeof(Navaid);
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
            return "When Start point of some route segment matches with End point of some route segment they must share same ReportingATC parameter";
        }

        public override string Comments()
        {
            return "When Start point of some route segment matches with End point of some route segment they must share same ReportingATC parameter";
        }

        public override string Name()
        {
            return "Route segment point ReportingATC";
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
