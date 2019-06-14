using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Aran.Aim;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Util;
using Aran.Temporality.Common.Util;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.View;
using Aran.Temporality.CommonUtil.ViewModel;
using TOSSM.Util;
using TOSSM.Util.Wrapper;
using TOSSM.ViewModel.Document.Relations.Util;
using TOSSM.ViewModel.Document.Slot.Problems.ProblemList;
using TOSSM.ViewModel.Document.Slot.Problems.Single;
using TOSSM.ViewModel.Pane.Base;

namespace TOSSM.ViewModel.Document.Slot
{
    public class SlotValidationReportViewModel : DocViewModel
    {
        #region Ctor

        public override Uri IconSource => new Uri("pack://application:,,,/Resources/Images/warning.png", UriKind.RelativeOrAbsolute);

        public override void OnClosed()
        {
            ErrorCategory.DataPresenter.IsTerminated = true;
            WarningCategory.DataPresenter.IsTerminated = true;
        }

        protected override void OnDispose()
        {
            ErrorCategory.DataPresenter.IsTerminated = true;
            WarningCategory.DataPresenter.IsTerminated = true;
        }

        public SlotValidationOverviewViewModel SlotValidationOverviewViewModel => _slotValidationOverviewViewModel ?? (_slotValidationOverviewViewModel = new SlotValidationOverviewViewModel());

        private readonly Dictionary<int, IList<ReadonlyFeatureWrapper>> _slotContent = new Dictionary<int, IList<ReadonlyFeatureWrapper>>();
        private IEnumerable<ReadonlyFeatureWrapper> GetSlotContent(FeatureType type)
        {
            IList<ReadonlyFeatureWrapper> features;
            if (!_slotContent.TryGetValue((int)type, out features))
            {
                features = DataProvider.GetSlotContent(EditedSlot, (int)type, false);
                _slotContent[(int)type] = features;
            }
            return features;
        }

        #region Init categories

        private void InitErrorCategory()
        {
            ErrorCategory.DataPresenter.BlockerModel = BlockerModel;
            ErrorCategory.OnSelectedFeatureChanged =
                     () =>
                     {
                         ErrorCategory.ProblemListViewModel.Clear();

                         var wrapper = ErrorCategory.DataPresenter.SelectedFeature as ReadonlyFeatureWrapper;
                         if (wrapper == null)
                         {
                             ErrorCategory.ProblemListViewModel.Update();
                             return;
                         }
                         var id = wrapper.Feature.Identifier;
                         var ruleIdsList = _businessRuleReportData.Where(t => t.Guid == id &&
                                                                              t.FeatureType ==
                                                                              ErrorCategory.SelectedRelation
                                                                                  .FeatureType)
                             .Select(t => t.RuleId)
                             .
                             ToList();
                         var errorIds =
                             DataProvider.BisinessRules.Where(
                                 t => ruleIdsList.Contains(t.Id) && t.Level == "Error").
                                 Select(t => t.Id).ToList();


                         var applicableRules =
                             DataProvider.BisinessRules.Where(t => errorIds.Contains(t.Id)).ToList();
                         foreach (var businessRuleUtil in applicableRules)
                         {
                             ErrorCategory.ProblemListViewModel.Add(new RuleViewModel(businessRuleUtil));
                         }

                         ErrorCategory.ProblemListViewModel.Update();
                     };

            ErrorCategory.OnReloadData =
                () =>
                {

                    if (ErrorCategory.SelectedRelation == null)
                    {
                        ErrorCategory.DataPresenter.FeatureData = new List<object>();
                        ErrorCategory.DataPresenter.UpdateFeatureDataFiltered();
                        return;
                    }
                    var type = ErrorCategory.SelectedRelation.FeatureType;
                    var ruleIds =
                        _businessRuleReportData.Where(t => t.FeatureType == type)
                            .Select(t => t.RuleId)
                            .Distinct()
                            .ToList();
                    var errorIds =
                        DataProvider.BisinessRules.Where(
                            t => t.IsActive && ruleIds.Contains(t.Id) && t.Level == "Error").
                            Select(t => t.Id).ToList();
                    var problemsFeatureIds =
                        _businessRuleReportData.Where(t => t.FeatureType == type && errorIds.Contains(t.RuleId))
                            .
                            Select(t => t.Guid).Distinct().ToList();
                    var list =
                        GetSlotContent(type)
                            .Where(t => problemsFeatureIds.Contains(t.Feature.Identifier))
                            .ToList();
                    ErrorCategory.DataPresenter.FeatureData = new List<object>(list);
                    ErrorCategory.DataPresenter.UpdateFeatureDataFiltered();
                    ErrorCategory.DataPresenter.SelectedFeature = null;

                };
        }

        private void InitWarningCategory()
        {
            WarningCategory.DataPresenter.BlockerModel = BlockerModel;

            WarningCategory.OnSelectedFeatureChanged =
             () =>
             {
                 WarningCategory.ProblemListViewModel.Clear();
                 var wrapper = WarningCategory.DataPresenter.SelectedFeature as ReadonlyFeatureWrapper;
                 if (wrapper == null)
                 {
                     WarningCategory.ProblemListViewModel.Update();
                     return;
                 }
                 var id = wrapper.Feature.Identifier;
                 var ruleIdsList = _businessRuleReportData.Where(t => t.Guid == id &&
                                                                      t.FeatureType ==
                                                                      WarningCategory.SelectedRelation
                                                                          .FeatureType).Select(t => t.RuleId).
                     ToList();
                 var errorIds =
                     DataProvider.BisinessRules.Where(t => ruleIdsList.Contains(t.Id) && t.Level == "Warning").
                         Select(t => t.Id).ToList();


                 var applicableRules = DataProvider.BisinessRules.Where(t => errorIds.Contains(t.Id)).ToList();
                 foreach (var businessRuleUtil in applicableRules)
                 {
                     WarningCategory.ProblemListViewModel.Add(new RuleViewModel(businessRuleUtil));
                 }

                 WarningCategory.ProblemListViewModel.Update();
             };

            WarningCategory.OnReloadData =
                () =>
                {
                    if (WarningCategory.SelectedRelation == null)
                    {
                        WarningCategory.DataPresenter.FeatureData = new List<object>();
                        WarningCategory.DataPresenter.UpdateFeatureDataFiltered();
                        return;
                    }
                    var type = WarningCategory.SelectedRelation.FeatureType;
                    var ruleIds =
                        _businessRuleReportData.Where(t => t.FeatureType == type)
                            .Select(t => t.RuleId)
                            .Distinct();
                    var errorIds =
                        DataProvider.BisinessRules.Where(t => ruleIds.Contains(t.Id) && t.Level == "Warning").
                            Select(t => t.Id).ToList();
                    var problemsFeatureIds =
                        _businessRuleReportData.Where(t => t.FeatureType == type && errorIds.Contains(t.RuleId))
                            .
                            Select(t => t.Guid).Distinct().ToList();
                    var list =
                        GetSlotContent(type)
                            .Where(t => problemsFeatureIds.Contains(t.Feature.Identifier))
                            .ToList();
                    WarningCategory.DataPresenter.FeatureData = new List<object>(list);
                    WarningCategory.DataPresenter.UpdateFeatureDataFiltered();
                    WarningCategory.DataPresenter.SelectedFeature = null;
                };
        }

        private void InitMissingLinkCategory()
        {
            MissingLinkCategory.DataPresenter.BlockerModel = BlockerModel;

            MissingLinkCategory.OnSelectedFeatureChanged =
                              () =>
                              {

                                  MissingLinkCategory.ProblemListViewModel.Clear();

                                  var wrapper =
                                      MissingLinkCategory.DataPresenter.SelectedFeature as
                                          ReadonlyFeatureWrapper;
                                  if (wrapper == null)
                                  {
                                      MissingLinkCategory.ProblemListViewModel.Update();
                                      return;
                                  }

                                  var id = wrapper.Feature.Identifier;
                                  var linkProblems = _linkProblemReportData.Where(t => t.Guid == id).ToList();
                                  foreach (var businessRuleUtil in linkProblems)
                                  {
                                      var ruleViewModel = new MissingLinkViewModel
                                      {
                                          PropertyPath = businessRuleUtil.PropertyPath,
                                          ReferenceFeatureIdentifier = businessRuleUtil.ReferenceFeatureIdentifier.ToString(),
                                          ReferenceFeatureType = businessRuleUtil.ReferenceFeatureType
                                      };
                                      MissingLinkCategory.ProblemListViewModel.Add(ruleViewModel);
                                  }

                                  MissingLinkCategory.ProblemListViewModel.Update();
                              };

            MissingLinkCategory.OnReloadData =
                () =>
                {
                    if (MissingLinkCategory.SelectedRelation == null)
                    {
                        MissingLinkCategory.DataPresenter.FeatureData = new List<object>();
                        MissingLinkCategory.DataPresenter.UpdateFeatureDataFiltered();
                        return;
                    }
                    var type = MissingLinkCategory.SelectedRelation.FeatureType;
                    var problemsFeatureIds = _linkProblemReportData.Where(t => t.FeatureType == type).Select(t => t.Guid).Distinct().ToList();
                    var list = GetSlotContent(type).Where(t => problemsFeatureIds.Contains(t.Feature.Identifier)).ToList();
                    MissingLinkCategory.DataPresenter.FeatureData = new List<object>(list);
                    MissingLinkCategory.DataPresenter.UpdateFeatureDataFiltered();
                    MissingLinkCategory.DataPresenter.SelectedFeature = null;

                };
        }

        private void InitSyntaxCategory()
        {
            SyntaxCategory.DataPresenter.BlockerModel = BlockerModel;

            SyntaxCategory.OnSelectedFeatureChanged =
                              () =>
                              {
                                  SyntaxCategory.ProblemListViewModel.Clear();

                                  var wrapper = SyntaxCategory.DataPresenter.SelectedFeature as ReadonlyFeatureWrapper;
                                  if (wrapper == null)
                                  {
                                      SyntaxCategory.ProblemListViewModel.Update();
                                      return;
                                  }

                                  var id = wrapper.Feature.Identifier;
                                  var linkProblems = _syntaxProblemReportData.Where(t => t.Guid == id).ToList();
                                  foreach (var businessRuleUtil in linkProblems)
                                  {
                                      var ruleViewModel = new SyntaxErrorViewModel
                                      {
                                          PropertyPath = businessRuleUtil.PropertyPath,
                                          Violation = businessRuleUtil.Violation,
                                          StringValue = businessRuleUtil.StringValue
                                      };
                                      SyntaxCategory.ProblemListViewModel.Add(ruleViewModel);
                                  }

                                  SyntaxCategory.ProblemListViewModel.Update();
                              };

            SyntaxCategory.OnReloadData =
                () =>
                {

                    if (SyntaxCategory.SelectedRelation == null)
                    {
                        SyntaxCategory.DataPresenter.FeatureData = new List<object>();
                        SyntaxCategory.DataPresenter.UpdateFeatureDataFiltered();
                        return;
                    }
                    var type = SyntaxCategory.SelectedRelation.FeatureType;
                    var problemsFeatureIds = _syntaxProblemReportData.Where(t => t.FeatureType == type).Select(t => t.Guid).Distinct().ToList();
                    var list = GetSlotContent(type).Where(t => problemsFeatureIds.Contains(t.Feature.Identifier)).ToList();

                    SyntaxCategory.DataPresenter.FeatureData = new List<object>(list);
                    SyntaxCategory.DataPresenter.UpdateFeatureDataFiltered();
                    SyntaxCategory.DataPresenter.SelectedFeature = null;

                };
        }

        #endregion

        private readonly int _slotId;

        public SlotValidationReportViewModel(int privateSlotId)
        {
            _slotId = privateSlotId;
            CanClose = true;
            StatisticsBusinessRulesViewModel.FilterChanged = true;



            InitErrorCategory();
            InitWarningCategory();
            InitMissingLinkCategory();
            InitSyntaxCategory();
        }

        #endregion

        #region DataContext models

        private BusinessRulesCategoryViewModel _statisticsBusinessRulesViewModel;
        public BusinessRulesCategoryViewModel StatisticsBusinessRulesViewModel => _statisticsBusinessRulesViewModel ?? (_statisticsBusinessRulesViewModel = new BusinessRulesCategoryViewModel(true));

        private SlotValidationCategoryViewModel _errorCategory;
        public SlotValidationCategoryViewModel ErrorCategory => _errorCategory ?? (_errorCategory = new SlotValidationCategoryViewModel("Business Rules Violating Features:", new BusinessRulesCategoryViewModel()));

        private SlotValidationCategoryViewModel _warningCategory;
        public SlotValidationCategoryViewModel WarningCategory => _warningCategory ?? (_warningCategory = new SlotValidationCategoryViewModel("Business Rules Violating Features:", new BusinessRulesCategoryViewModel()));

        private SlotValidationCategoryViewModel _missingLinkCategory;
        public SlotValidationCategoryViewModel MissingLinkCategory => _missingLinkCategory ?? (_missingLinkCategory = new SlotValidationCategoryViewModel("Missing Link Features:", new MissingLinksProblemListViewModel()));

        private SlotValidationCategoryViewModel _syntaxCategory;
        public SlotValidationCategoryViewModel SyntaxCategory => _syntaxCategory ?? (_syntaxCategory = new SlotValidationCategoryViewModel("Syntax Violating Features:", new SyntaxProblemListViewModel()));

        private BlockerModel _blockerModel;
        public BlockerModel BlockerModel
        {
            get => _blockerModel ?? (_blockerModel = new BlockerModel { ActivatingObject = this });
            set => _blockerModel = value;
        }

        #endregion

        #region Load

        private List<LinkProblemReportUtil> _linkProblemReportData;
        private List<SyntaxProblemReportUtil> _syntaxProblemReportData;
        private List<BusinessRuleProblemReportUtil> _businessRuleReportData;

        private void LoadRuleStatistics()
        {
            if (_businessRuleReportData == null) return;

            var ruleIds = _businessRuleReportData.Select(t => t.RuleId).Distinct();
            var applicableRules = DataProvider.BisinessRules.Where(t => ruleIds.Contains(t.Id)).ToList();
            var statistics = applicableRules.Select(businessRuleUtil =>
                new RuleViewModel(businessRuleUtil)
                {
                    Count = _businessRuleReportData.Count(t => t.RuleId == businessRuleUtil.Id)
                }).ToList();

            NumberOfViolatedRules = statistics.Count(t => t.IsActive);

            StatisticsBusinessRulesViewModel.Rules = statistics.OrderByDescending(t => t.IsActive).
                ThenByDescending(t => t.Count).ThenBy(t => t.Name).ToList();
            StatisticsBusinessRulesViewModel.RulesFiltered = new List<RuleViewModel>(StatisticsBusinessRulesViewModel.Rules);
            StatisticsBusinessRulesViewModel.FilterChanged = true;
        }

      

        private async void LoadErrors()
        {
            if (_businessRuleReportData == null) return;

            {
                var ruleIds = _businessRuleReportData.Select(t => t.RuleId).Distinct();
                var errorIds = DataProvider.BisinessRules.Where(t => ruleIds.Contains(t.Id) && t.IsActive && t.Level == "Error").
                                Select(t => t.Id).ToList();
                NumberOfErrors = _businessRuleReportData.Where(t => errorIds.Contains(t.RuleId)).Select(t => t.Guid).Distinct().Count();
            }

            Application.Current.Dispatcher.Invoke(
                                    DispatcherPriority.Background,
                                    (Action)(() =>
                                                 {
                                                     ErrorCategory.RelationsData.Clear();
                                                     if (ErrorCategory.DataPresenter.FeatureData != null)
                                                     {
                                                         ErrorCategory.DataPresenter.FeatureData.Clear();
                                                     }
                                                     ErrorCategory.DataPresenter.UpdateFeatureDataFiltered();

                                                     ErrorCategory.ProblemListViewModel.Clear();
                                                     ErrorCategory.ProblemListViewModel.Update();
                                                 }));

            var featureTypes = _businessRuleReportData.Select(t => t.FeatureType).Distinct().ToList();
            await Util.Parallel.All(featureTypes, (featureType) =>
            {
                var type = featureType;
                var ruleIds = _businessRuleReportData.Where(t => t.FeatureType == type).Select(t => t.RuleId).Distinct();
                var errorIds = DataProvider.BisinessRules.Where(t => ruleIds.Contains(t.Id) && t.IsActive && t.Level == "Error").
                                Select(t => t.Id).ToList();
                if (errorIds.Count > 0)
                {
                    Application.Current.Dispatcher.Invoke(
                                     DispatcherPriority.Background,
                                     (Action)(() => ErrorCategory.RelationsData.Add(
                                        new SingleTypeRelationViewModel
                                        {
                                            FeatureType = type,
                                            Count = _businessRuleReportData.Where(t => t.FeatureType == type && errorIds.Contains(t.RuleId)).
                                                        Select(t => t.Guid).Distinct().Count()
                                        })));
                }
            });
        }



        private bool _isSyntaxLoaded;
        private async void LoadSyntax()
        {
            if (_isSyntaxLoaded) return;
            _isSyntaxLoaded = true;
            if (_syntaxProblemReportData == null) return;


            var featTypes = _syntaxProblemReportData.Select(t => t.FeatureType).Distinct().ToList();
            NumberOfSyntaxErrors = _syntaxProblemReportData.Select(t => t.Guid).Distinct().Count();

            Application.Current.Dispatcher.Invoke(
                                    DispatcherPriority.Background,
                                    (Action)(() =>
                                    {
                                        SyntaxCategory.RelationsData.Clear();
                                        if (SyntaxCategory.DataPresenter.FeatureData != null)
                                        {
                                            SyntaxCategory.DataPresenter.FeatureData.Clear();
                                        }
                                        SyntaxCategory.DataPresenter.UpdateFeatureDataFiltered();

                                        SyntaxCategory.ProblemListViewModel.Clear();
                                        SyntaxCategory.ProblemListViewModel.Update();
                                    }));

            await Util.Parallel.All(featTypes, (featType) =>
            {
                var type = featType;
                Application.Current.Dispatcher.Invoke(
                                 DispatcherPriority.Background,
                                 (Action)(() => SyntaxCategory.RelationsData.Add(
                                    new SingleTypeRelationViewModel
                                    {
                                        FeatureType = type,
                                        Count = _syntaxProblemReportData.Where(t => t.FeatureType == type).
                                                    Select(t => t.Guid).Distinct().Count()
                                    })));
            });
        }

        private bool _isMissingLinksLoaded;
        private async void LoadMissingLinks()
        {
            if (_isMissingLinksLoaded) return;
            _isMissingLinksLoaded = true;

            if (_linkProblemReportData == null) return;

            var featTypes = _linkProblemReportData.Select(t => t.FeatureType).Distinct().ToList();
            NumberOfMissingLinks = _linkProblemReportData.Select(t => t.Guid).Distinct().Count();


            Application.Current.Dispatcher.Invoke(
                                    DispatcherPriority.Background,
                                    (Action)(() =>
                                {
                                    MissingLinkCategory.RelationsData.Clear();
                                    if (MissingLinkCategory.DataPresenter.FeatureData != null)
                                    {
                                        MissingLinkCategory.DataPresenter.FeatureData.Clear();
                                    }
                                    MissingLinkCategory.DataPresenter.UpdateFeatureDataFiltered();

                                    MissingLinkCategory.ProblemListViewModel.Clear();
                                    MissingLinkCategory.ProblemListViewModel.Update();
                                }));

            await Util.Parallel.All(featTypes, (featType) =>
            {
                var type = featType;
                Application.Current.Dispatcher.Invoke(
                                 DispatcherPriority.Background,
                                 (Action)(() => MissingLinkCategory.RelationsData.Add(
                                new SingleTypeRelationViewModel
                                {
                                    FeatureType = type,
                                    Count = _linkProblemReportData.Where(t => t.FeatureType == type).
                                                Select(t => t.Guid).Distinct().Count()
                                })));
            });
        }

        private async void LoadWarnings()
        {
            if (_businessRuleReportData == null) return;

            {
                var ruleIds = _businessRuleReportData.Select(t => t.RuleId).Distinct();
                var errorIds = DataProvider.BisinessRules.Where(t => ruleIds.Contains(t.Id) && t.IsActive && t.Level == "Warning").
                                Select(t => t.Id).ToList();
                NumberOfWarnings = _businessRuleReportData.Where(t => errorIds.Contains(t.RuleId)).Select(t => t.Guid).Distinct().Count();
            }

            Application.Current.Dispatcher.Invoke(
                                   DispatcherPriority.Background,
                                   (Action)(() =>
                                   {
                                       WarningCategory.RelationsData.Clear();
                                       if (WarningCategory.DataPresenter.FeatureData != null)
                                       {
                                           WarningCategory.DataPresenter.FeatureData.Clear();
                                       }

                                       WarningCategory.DataPresenter.UpdateFeatureDataFiltered();

                                       WarningCategory.ProblemListViewModel.Clear();
                                       WarningCategory.ProblemListViewModel.Update();
                                   }));


            var featureTypes = _businessRuleReportData.Select(t => t.FeatureType).Distinct().ToList();
            await Util.Parallel.All(featureTypes, (featureType) =>
            {
                var type = featureType;
                var ruleIds = _businessRuleReportData.Where(t => t.FeatureType == type).Select(t => t.RuleId).Distinct();
                var warningIds = DataProvider.BisinessRules.Where(t => ruleIds.Contains(t.Id) && t.IsActive && t.Level == "Warning").
                                Select(t => t.Id).ToList();
                if (warningIds.Count > 0)
                {
                    Application.Current.Dispatcher.Invoke(
                                     DispatcherPriority.Background,
                                     (Action)(() => WarningCategory.RelationsData.Add(
                                         new SingleTypeRelationViewModel
                                         {
                                             FeatureType = type,
                                             Count = _businessRuleReportData.Where(t => t.FeatureType == type && warningIds.Contains(t.RuleId)).
                                                         Select(t => t.Guid).Distinct().Count()
                                         })));
                }
            });
        }


        public FeatureDependencyReportDocViewModel FeatureDependencyReportDocViewModel
        {
            get
            {
                return _featureDependencyReportDocViewModel ?? (_featureDependencyReportDocViewModel = new FeatureDependencyReportDocViewModel(_slotId)
                {
                    OnLoaded = () =>
                    {
                        var toBeDeleted = SlotValidationOverviewViewModel.Data.Where(t => t.ReportType == ReportType.FeatureDependencyReport).ToList();
                        foreach (var item in toBeDeleted)
                        {
                            SlotValidationOverviewViewModel.Data.Remove(item);
                        }
                        foreach (var configuration in FeatureDependencyReportDocViewModel.Configurations)
                        {
                            SlotValidationOverviewViewModel.Data.Add(new OverviewViewModel
                            {
                                ReportType = ReportType.FeatureDependencyReport,
                                Name = configuration.Name + " Dataset Validation Result:",
                                Description = configuration.ErrorsMessage + " Issues",
                                Date = GetDate(configuration.Date),
                                PictureId = (configuration.ErrorNumber == 0 ? OverviewViewModel.IsOk : OverviewViewModel.IsIssue)
                            });
                        }

                    }
                });
            }
        }

        private void LoadDependencies()
        {
            FeatureDependencyReportDocViewModel.LoadInternal();
        }

        public async void ReloadData()
        {
            await BlockerModel.BlockForAction(() =>
                                            {
                                                //gather rule statistics
                                                LoadRuleStatistics();

                                                //init errors and warnings by feature types
                                                LoadErrors();
                                                LoadWarnings();

                                                //
                                                LoadMissingLinks();

                                                LoadSyntax();

                                                LoadOverView();

                                                LoadDependencies();

                                            });

        }

        private bool isOverviewLoaded;
        private void LoadOverView()
        {
            if (isOverviewLoaded) return;
            isOverviewLoaded = true;

            SlotValidationOverviewViewModel.Data.Add(new OverviewViewModel
            {
                ReportType = ReportType.SyntaxReport,
                Name = "Syntax Validation Result:",
                Description = NumberOfSyntaxErrors + " Syntax Errors",
                Date = GetDate(SyntaxCheckDate),
                PictureId = NumberOfSyntaxErrors == 0 ? OverviewViewModel.IsOk : OverviewViewModel.IsIssue
            });

            SlotValidationOverviewViewModel.Data.Add(new OverviewViewModel
            {
                ReportType = ReportType.MissingLinkReport,
                Name = "Missed Links Validation Result:",
                Description = NumberOfMissingLinks + " Missed Links",
                Date = GetDate(LinksCheckDate),
                PictureId = NumberOfMissingLinks == 0 ? OverviewViewModel.IsOk : OverviewViewModel.IsIssue
            });

            SlotValidationOverviewViewModel.Data.Add(new OverviewViewModel
            {
                ReportType = ReportType.BusinessRulesReport,
                Name = "Business Rules Validation Result:",
                Description = NumberOfViolatedRules + " Rules Violated, " + NumberOfErrors + " Errors",
                Date = GetDate(BusinessRuleCheckDate),
                PictureId = NumberOfViolatedRules == 0 ? OverviewViewModel.IsOk : OverviewViewModel.IsIssue
            });
        }

        public DateTime SyntaxCheckDate { get; set; }
        public DateTime LinksCheckDate { get; set; }
        public DateTime BusinessRuleCheckDate { get; set; }

        private string GetDate(DateTime date)
        {
            return date == default(DateTime) ? "Was never performed" : "Performed " + DateTimeUtil.TimeAgo(date);
        }

        public static string GetString(byte[] bytes)
        {
            if (bytes == null) return "";
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        private void LoadRelations()
        {
            var publishingErrorReport = CurrentDataContext.CurrentNoAixmDataService.GetProblemReport(EditedSlot.PublicSlot.Id, 0, 0, ReportType.PublishingError);
            if (publishingErrorReport != null)
            {
                var errorMessage = GetString(publishingErrorReport.ReportData);
                SlotValidationOverviewViewModel.MoreDetailedMessage = errorMessage;
            }

            var problemReportEntity = CurrentDataContext.CurrentNoAixmDataService.GetProblemReport(0, _slotId, 0, ReportType.BusinessRulesReport);

            if (problemReportEntity != null)
            {
                BusinessRuleCheckDate = problemReportEntity.DateTime;
                //load report data
                _businessRuleReportData = FormatterUtil.ObjectFromBytes<List<ProblemReportUtil>>(problemReportEntity.ReportData).Cast<BusinessRuleProblemReportUtil>().ToList();
            }

            var missingLinkProblemReportEntity = CurrentDataContext.CurrentNoAixmDataService.GetProblemReport(0, _slotId, 0, ReportType.MissingLinkReport);
            if (missingLinkProblemReportEntity != null)
            {
                LinksCheckDate = missingLinkProblemReportEntity.DateTime;
                _linkProblemReportData = FormatterUtil.ObjectFromBytes<List<ProblemReportUtil>>(missingLinkProblemReportEntity.ReportData).Cast<LinkProblemReportUtil>().ToList();
            }

            var syntaxProblemReportEntity = CurrentDataContext.CurrentNoAixmDataService.GetProblemReport(0, _slotId, 0, ReportType.SyntaxReport);
            if (syntaxProblemReportEntity != null)
            {
                SyntaxCheckDate = syntaxProblemReportEntity.DateTime;
                _syntaxProblemReportData = FormatterUtil.ObjectFromBytes<List<ProblemReportUtil>>(syntaxProblemReportEntity.ReportData).Cast<SyntaxProblemReportUtil>().ToList();
            }

            //update UI
            ReloadData();
        }

        public PrivateSlot EditedSlot { get; set; }

        private int _numberOfViolatedRules;
        public int NumberOfViolatedRules
        {
            get => _numberOfViolatedRules;
            set
            {
                _numberOfViolatedRules = value;
                OnPropertyChanged("NumberOfViolatedRules");

                UpdateOverview();
            }
        }

        private void UpdateOverview()
        {
            var related = SlotValidationOverviewViewModel.Data.FirstOrDefault(t => t.Name == "Business Rules Validation Result:");
            if (related != null)
            {
                related.Description = NumberOfViolatedRules + " Rules Violated, " + NumberOfErrors + " Errors";
                related.PictureId = NumberOfViolatedRules == 0 ? OverviewViewModel.IsOk : OverviewViewModel.IsIssue;
            }
        }


        private int _numberOfErrors;
        public int NumberOfErrors
        {
            get => _numberOfErrors;
            set
            {
                _numberOfErrors = value;
                OnPropertyChanged("NumberOfErrors");
                UpdateOverview();
            }
        }

        private int _numberOfWarnings;
        public int NumberOfWarnings
        {
            get => _numberOfWarnings;
            set
            {
                _numberOfWarnings = value;
                OnPropertyChanged("NumberOfWarnings");
            }
        }



        private int _numberOfSyntaxErrors;
        public int NumberOfSyntaxErrors
        {
            get => _numberOfSyntaxErrors;
            set
            {
                _numberOfSyntaxErrors = value;
                OnPropertyChanged("NumberOfSyntaxErrors");
            }
        }


        private int _numberOfMissingLinks;
        private SlotValidationOverviewViewModel _slotValidationOverviewViewModel;
        private FeatureDependencyReportDocViewModel _featureDependencyReportDocViewModel;

        public int NumberOfMissingLinks
        {
            get => _numberOfMissingLinks;
            set
            {
                _numberOfMissingLinks = value;
                OnPropertyChanged("NumberOfMissingLinks");
            }
        }

        public override async void Load()
        {
            if (IsLoaded) return;

            await BlockerModel.BlockForAction(
                    () =>
                    {
                        EditedSlot = CurrentDataContext.CurrentNoAixmDataService.GetPrivateSlotById(_slotId);

                        Title = "Validation Report for space " +
                                EditedSlot.Name +
                                AiracSelectorViewModel.AiracMessage(EditedSlot.PublicSlot.EffectiveDate);

                        ErrorCategory.AiracDate = EditedSlot.PublicSlot.EffectiveDate;
                        MissingLinkCategory.AiracDate = EditedSlot.PublicSlot.EffectiveDate;
                        WarningCategory.AiracDate = EditedSlot.PublicSlot.EffectiveDate;

                        ErrorCategory.DataPresenter.EffectiveDate = EditedSlot.PublicSlot.EffectiveDate;
                        MissingLinkCategory.DataPresenter.EffectiveDate = EditedSlot.PublicSlot.EffectiveDate;
                        WarningCategory.DataPresenter.EffectiveDate = EditedSlot.PublicSlot.EffectiveDate;

                        AiracDate = EditedSlot.PublicSlot.EffectiveDate;


                        LoadRelations();

                        IsLoaded = true;
                    });
        }

        #endregion
    }
}
