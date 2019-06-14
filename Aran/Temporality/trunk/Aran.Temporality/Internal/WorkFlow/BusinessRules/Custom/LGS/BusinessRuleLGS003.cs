using System;
using System.Linq;
using Aran.Aim;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Dictionary;
using NHibernate.Util;

namespace Aran.Temporality.Internal.WorkFlow.BusinessRules
{
    internal class BusinessRuleLGS003 : AbstractBusinessRule
    {

        #region Overrides of AbstractBusinessRule

        public override string UID => "LGS003";

        public override bool CheckParent(object obj)
        {
            var item = obj as IAimObject;
            if (item == null) return false;
            var properties = GetProperties(item);
            foreach (var propInfo in properties)
            {
                var classInfo = AimMetadata.GetClassInfoByIndex(item);
                var conjProp = classInfo.Properties
                    .Where(t => t.Name == propInfo.Name.Substring(0, propInfo.Name.IndexOf("Accuracy", StringComparison.Ordinal)))
                    .ToList().FirstOrNull() as AimPropInfo;

                if (conjProp != null)
                {
                    dynamic cProp = item.GetValue(conjProp.Index);
                    dynamic prop = item.GetValue(propInfo.Index);
                    return (cProp != null && prop != null) || (cProp == null && prop == null);
                }

                
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
            return "It is prohibited that one of the properties {property} and {property}Accuracy has value, but other not.";
        }

        public override string Comments()
        {
            return "{property} and {property}Accuracy must both have value or both be empty";
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