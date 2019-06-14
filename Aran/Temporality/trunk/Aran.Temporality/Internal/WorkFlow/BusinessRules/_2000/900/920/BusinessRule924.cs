
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule924 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as VerticalStructurePartGeometry;
            if (item == null) return false;

            //"Each VerticalStructurePartGeometry must have location";
            return item.Location != null;

        }

        public override Type GetApplicableType()
        {
            return typeof (VerticalStructurePartGeometry);
        }

        public override string GetApplicableProperty()
        {
            return null;
        }

        public override string Source()
        {
            return "AIXM 4.5 Business Rules";
        }

        public override string Svbr()
        {
            return "Each VerticalStructurePartGeometry must have location";
        }

        public override string Comments()
        {
            return "VerticalStructurePartGeometry has mandatory attribute location";
        }

        public override string Name()
        {
            return "Template - Mandatory objects from AIXM4.5";
        }

        public override string Category()
        {
            return "Minimal data rule";
        }

        public override string Level()
        {
            return "Error";
        }

        #endregion
    }
}
