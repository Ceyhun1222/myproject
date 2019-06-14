
using System;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule872 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as DME;
            if (item == null) return false;

            //"Each DME.ElevatedPoint.verticalAccuracy should be at most 3m if DME.type is equal to 'PRECISION'";

            if (item.Type==CodeDME.PRECISION)
            {
                if (item.Location.VerticalAccuracy!=null)
                {
                    if (item.Location.VerticalAccuracy.Uom==UomDistance.M)
                    {
                        return item.Location.VerticalAccuracy.Value <= 3;
                    }
                    if (item.Location.VerticalAccuracy.Uom == UomDistance.FT)
                    {
                        return item.Location.VerticalAccuracy.Value <= 10;
                    }
                    return false;
                }
                return false;
            }

            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof (DME);
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
            return "Each DME.ElevatedPoint.verticalAccuracy should be at most 3m if DME.type is equal to 'PRECISION'";
        }

        public override string Comments()
        {
            return
                "VAL_ELEV_ACCURACY should be equal or better than 3 m or 10 ft for CODE_TYPE =A?A?A?PA?A?A? and equal or better than 30 m (100 ft) for other CODE_TYPE. [ Standard  -  Source:   ICAO annex 15, Appendix 7-2]";
        }

        public override string Name()
        {
            return "DME_PRECISION_VerticalAccuracy";
        }

        public override string Category()
        {
            return "Recommended practice";
        }

        public override string Level()
        {
            return ErrorLevel.Warning;
        }

        #endregion
    }
}
