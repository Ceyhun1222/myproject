
using System;
using Aran.Aim.Features;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRule851 : AbstractBusinessRule
    {
        #region Overrides of AbstractBusinessRule

        public override bool CheckChild(object obj)
        {
            var item = obj as Localizer;
            if (item == null) return false;

            //"Each Localizer that has widthCourseAccuracy must have widthCourse and widthCourse.uom = widthCourseAccuracy.uom";
     
            if (item.WidthCourseAccuracy!=null)
            {
                return item.WidthCourse != null;
            }
            return true;
        }

        public override Type GetApplicableType()
        {
            return typeof (Localizer);
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
            return
                "Each Localizer that has widthCourseAccuracy must have widthCourse and widthCourse.uom = widthCourseAccuracy.uom";
        }

        public override string Comments()
        {
            return
                "Localizer that has a value in widthCourseAccuracy must have a value in the corresponding widthCourse. uom must match.";
        }

        public override string Name()
        {
            return "Template - Mandatory value when accuracy present";
        }

        public override string Category()
        {
            return "Recommended practice";
        }

        public override string Level()
        {
            return ErrorLevel.Error;
        }

        #endregion
    }
}
