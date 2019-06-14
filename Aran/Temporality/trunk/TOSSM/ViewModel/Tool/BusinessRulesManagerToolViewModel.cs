using System;
using System.Linq;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.ViewModel;
using TOSSM.ViewModel.Document.Slot.Problems.ProblemList;
using TOSSM.ViewModel.Document.Slot.Problems.Single;
using TOSSM.ViewModel.Pane.Base;

namespace TOSSM.ViewModel.Tool
{
    public class BusinessRulesManagerToolViewModel : ToolViewModel
    {

        protected override void OnDispose()
        {
            ErrorCategoryViewModel.IsTerminated = true;
            base.OnDispose();
        }

        public BusinessRulesCategoryViewModel ErrorCategoryViewModel { get; set; }
        public BusinessRulesCategoryViewModel WarningCategoryViewModel { get; set; }

        #region Ctor

        public static string ToolContentId = "Business Rules";

        public BusinessRulesManagerToolViewModel() : base(ToolContentId)
        {
            ContentId = ToolContentId;

            ErrorCategoryViewModel=new BusinessRulesCategoryViewModel(true);
            WarningCategoryViewModel = new BusinessRulesCategoryViewModel(true);
        }

        public override Uri IconSource => new Uri("pack://application:,,,/Resources/Images/rules.png", UriKind.RelativeOrAbsolute);

        public override void Load()
        {
            if (IsLoaded) return;
            IsLoaded = true;
            ReloadData();
        }

        #endregion


        private BlockerModel _blockerModel;
        public BlockerModel BlockerModel
        {
            get => _blockerModel ?? (_blockerModel = new BlockerModel { ActivatingObject = this });
            set => _blockerModel = value;
        }

        public void ReloadData()
        {
            BlockerModel.BlockForAction(
               () =>
               {
                   var rules = CurrentDataContext.GetBusinessRules().ToList();

                   rules = rules.OrderByDescending(t=>t.IsActive).ThenBy(t => t.Name).ToList();

                   ErrorCategoryViewModel.Rules.Clear();
                   WarningCategoryViewModel.Rules.Clear();



                   foreach (var rule in rules)
                   {
                       if (rule.Level == "Error")
                       {
                           ErrorCategoryViewModel.Rules.Add(new RuleViewModel(rule) { OnChanged = ErrorCategoryViewModel.OnChanged });
                       }
                       if (rule.Level == "Warning")
                       {
                           WarningCategoryViewModel.Rules.Add(new RuleViewModel(rule) { OnChanged = WarningCategoryViewModel.OnChanged });
                       }
                   }

                   ErrorCategoryViewModel.UpdateRulesFiltered();
                   WarningCategoryViewModel.UpdateRulesFiltered();

               });

        }
    }
}
