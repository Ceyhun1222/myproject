
using System;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule0 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckParent(object obj)
        {
            var item = (dynamic)obj;
            var srsName = item.SrsName;
            if (srsName != null)
            {
                return srsName == "urn:ogc:def:crs:OGC:1.3:CRS84";
            }
            return true;
        }

        public override Type GetApplicableType()
        {
            return null;
        }

        public override string GetApplicableProperty()
        {
            return "SrsName";
        }

        public override string Source()
        {
            return RuleSource.AIXM45BR;
        }

        public override string Svbr()
        {
            return "Each [...] that has an srsName must have srsName = 'urn:ogc:def:crs:OGC:1.3:CRS84'";
        }

        public override string Comments()
        {
            return "All geographical coordinates should be expressed in the WGS 84 system [Standard - Source: ICAO Annex 15, item 3.7.1.1]";
        }

        public override string Name()
        {
            return "generic rule 01 - Geographic System WGS84";
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
