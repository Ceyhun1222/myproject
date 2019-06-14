
using System;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule881 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as DeicingArea;
            if (item == null) return false;

            //"Each DeicingArea.extent.ElevatedSurface must have horizontalAccuracy equal to 1 M";

            if (item.Extent!=null)
            {
                if (item.Extent.HorizontalAccuracy!=null)
                 {
                     return item.Extent.HorizontalAccuracy.Uom == UomDistance.M
                            && Math.Abs(item.Extent.HorizontalAccuracy.Value - 1) < Epsilon();
                 }
                return false;
            }


            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof (DeicingArea);
        }

        public override string GetApplicableProperty()
        {
            return null;
        }

        public override string Source()
        {
            return RuleSource.AIXM45BR;
        }

        public override string Svbr()
        {
            return "Each DeicingArea.extent.ElevatedSurface must have horizontalAccuracy equal to 1 M";
        }

        public override string Comments()
        {
            return "Aeronautical Data requirements DeicingArea.extent.ElevatedSurface.horizontalAccuracy 1 M";
        }

        public override string Name()
        {
            return "Template - Annex 14 Vol1 Table A5-1 Accuracy Lat/long/Alt/Elev/Height ";
        }

        public override string Category()
        {
            return RuleCategory.Standard;
        }

        public override string Level()
        {
            return ErrorLevel.Error;
        }

        #endregion
    }
}
