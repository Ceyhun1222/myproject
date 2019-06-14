
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule126 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            //"Each AirportHeliport that has a type equal to 'AD' or 'AH' must have a controlType"
            var item = obj as AirportHeliport;
            if (item == null) return false;
            if (item.Type.ToString() == "AD" || item.Type.ToString() == "AH")
            {
                return item.ControlType != null;
            }
            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof(AirportHeliport);
        }

        public override string GetApplicableProperty()
        {
            return "controlType";
        }

        public override string Source()
        {
            return "AIXM 4.5 Business Rules";
        }

        public override string Svbr()
        {
            return "Each AirportHeliport that has a type equal to 'AD' or 'AH' must have a controlType";
        }

        public override string Comments()
        {
            return "If CODE_TYPE='AD' (aerodrome only) or 'AH' (aerodrome with helipads), then CODE_TYPE_MIL_OPS is mandatory ";
        }

        public override string Name()
        {
            return "AHP_AH_or_AD_req_CODE_TYPE_MIL_OPS";
        }

        public override string Category()
        {
            return "Data consistency rule";
        }

        public override string Level()
        {
            return "Error";
        }

        #endregion
    }
}
