
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule127 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            //"Each AirportHeliport.ARP must have horizontalAccuracy and AirportHeliport.ARP.horizontalAccuracy  should be at most 1sec"
            var item = obj as AirportHeliport;
            if (item == null) return false;
            if (item.ARP != null)
            {
                if (item.ARP.HorizontalAccuracy != null)
                {
                    return Math.Abs(ConverterToSI.Convert(item.ARP.HorizontalAccuracy,0)- 30)<Epsilon();
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof(AirportHeliport);
        }

        public override string GetApplicableProperty()
        {
            return "horizontalAccuracy";
        }

        public override string Source()
        {
            return "AIXM 4.5 Business Rules";
        }

        public override string Svbr()
        {
            return "Each AirportHeliport.ARP must have horizontalAccuracy and AirportHeliport.ARP.horizontalAccuracy  should be at most 1sec";
        }

        public override string Comments()
        {
            return "VAL_GEO_ACCURACY should be of at least 1 sec ";
        }

        public override string Name()
        {
            return "AHP_ARP_horizontalAccuracy";
        }

        public override string Category()
        {
            return "Recommended practice";
        }

        public override string Level()
        {
            return "Warning";
        }

        #endregion
    }
}
