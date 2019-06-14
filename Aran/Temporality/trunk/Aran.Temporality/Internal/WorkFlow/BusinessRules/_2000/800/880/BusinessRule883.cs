
using System;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule883 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as CheckpointINS;
            if (item == null) return false;

            //"Each CheckPointINS.position.ElevatedPoint must have horizontalAccuracy equal to 0.5 M";

            if (item.Position!=null)
            {
                if (item.Position.HorizontalAccuracy!=null)
                {
                    return item.Position.HorizontalAccuracy.Uom == UomDistance.M &&
                           Math.Abs(item.Position.HorizontalAccuracy.Value - 0.5) < Epsilon();
                }
                return false;
            }

            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof (CheckpointINS);
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
            return "Each CheckPointINS.position.ElevatedPoint must have horizontalAccuracy equal to 0.5 M";
        }

        public override string Comments()
        {
            return "Aeronautical Data requirements CheckPointINS.position.ElevatedPoint.horizontalAccuracy 0.5 M";
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
