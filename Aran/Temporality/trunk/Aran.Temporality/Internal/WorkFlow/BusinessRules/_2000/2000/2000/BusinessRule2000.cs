using System;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules._2000._2000._2000
{
    internal class BusinessRule2000 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as RouteSegment;
            if (item == null) return false;

            return item.End != null && item.Start != null;
        }

        public override Type GetApplicableType()
        {
            return typeof(RouteSegment);
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
            return "Each RouteSegment must have Start and End points";
        }

        public override string Comments()
        {
            return "Each RouteSegment must have Start and End points";
        }

        public override string Name()
        {
            return "RouteSegment Start+End";
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
