using System;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRuleLGS001 : AbstractBusinessRule
    {

        #region Overrides of AbstractBusinessRule

        public override bool CheckParent(object obj)
        {
            var item = obj as IAimObject;
            if (item == null) return false;
            var properties = GetProperties(item);
            foreach (var propInfo in properties)
            {
                var prop  = item.GetValue(propInfo.Index) as ValClass<UomDistance, double>;
                if (prop?.Uom == UomDistance.NM)
                    return false;


            }
            return true;
        }


        public override Type GetApplicableType()
        {
            return null;
        }

        public override string GetApplicableProperty()
        {
            return "^\\w*Accuracy$";
        }

        public override string Source()
        {
            return RuleSource.LGS;
        }

        public override string Svbr()
        {
            return "It is prohibited that an *Accuracy property has *Accuracy.uom equal to 'NM'";
        }

        public override string Comments()
        {
            return "An *Accuracy property can not have *Accuracy.uom equal to 'NM'.";
        }

        public override string Name()
        {
            return "Accuracy";
        }

        public override string Category()
        {
            return RuleCategory.RecommendedPractice;
        }

        public override string Level()
        {
            return ErrorLevel.Error;
        }

        #endregion
    }
}