using MvvmCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRuleManager.Model
{
    class VocabularyInitializer : BaseInitializer
    {
        public VocabularyInitializer(object model) : base(model)
        {
            _model.KeywordItems = VocabularyItems.GetKeywords();
            _model.VerbItems = VocabularyItems.GetVerbs();
            _model.LastAddedAimObjectName = null;
            _model.OtherKeyword = string.Empty;

            var featList = new List<string>();
            foreach (var item in AimTypeItems.ClassInfoList)
            {
                if (item.AimObjectType == Aran.Aim.AimObjectType.Feature)
                    featList.Add(item.AixmName);
            }

            var objList = new List<string>();
            foreach (var item in AimTypeItems.ClassInfoList)
            {
                if (item.AimObjectType == Aran.Aim.AimObjectType.Object)
                    objList.Add(item.AixmName);
            }

            _model.FeatureTypes = featList;
            _model.ObjectTypes = objList;

            _model.AddPropertyCommand = new RelayCommand(AddPropertyCommand);
        }

        private void AddPropertyCommand(object arg)
        {
            dynamic propSelModel = ModelFactory.Create(ModelType.PropertySelector);
            propSelModel.OkAction = new Action<string>(PropertySelectorOkAction);

            string rootFeatureName = _model.LastAddedAimObjectName;
            if (rootFeatureName != null)
                propSelModel.AddRootFeature(rootFeatureName);

            WindowService.Instance.ShowWindow(propSelModel, _model._window);
        }

        private void PropertySelectorOkAction(string pathText)
        {
            Action<string> propertySelectedAction = _model.PropertySelectedAction;
            propertySelectedAction?.Invoke(pathText);
        }
    }

    static class VocabularyItems
    {
        public static List<string> GetKeywords()
        {
            var list = new List<string>
            {
                "It is prohibited that",
                "It is obligatory that",
                "Each",

                "a",
                "an",

                "shall",
                "or",
                "and",
                "not",

                "assigned",

                "with",
                "value",

                "at least",
                "at least one",
                "at most",
                "exactly one",
                "more than",
                "more than one",

                "equal-to",
                "higher-than",
                "higher-or-equal-to",
                "lower-than",
                "lower-or-equal-to",
                "resolved-into",
                "other-than"
            };
            list.Sort();
            return list;
        }

        public static List<string> GetVerbs()
        {
            var list = new List<string>
            {
                "specialisation",
                "have",
                "has"
            };
            list.Sort();
            return list;
        }

    }
}
