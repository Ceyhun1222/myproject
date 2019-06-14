using MvvmCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessRules.Data;
using System.ComponentModel;
using Aran.Aim.BusinessRules.SbvrParser;
using Aran.Aim.BusinessRules;
using Aran.Aim;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Reflection;

namespace BRuleManager.Model
{
    class RulesInitializer : BaseInitializer
    {
        private RulesDb _rulesDb;
        private string _prevSearchText;
        private List<ExpandoObject> _rules;

        public RulesInitializer(object model) : base(model)
        {
            DefaultSize = new System.Windows.Size(900, 600);
            WindowTitle = "Business Rules Manager";

            _model.RuleSelectedItem = null;
            _model.Items = null;
            _model.NewOrEditCommand = new RelayCommand(OnNewOrEdit, OnNewOrEditCanExecute);
            _model.DeleteCommand = new RelayCommand(OnDelete, DeleteCanExecute);
            _model.CloseCommand = new RelayCommand((a) => { _model._window.Close(); });
            _model.ActivateSelectedCommand = new RelayCommand(OnActivateSelected);
            _model.AboutCommand = new RelayCommand(OnAbout);
            _model.ViewHelpCommand = new RelayCommand(OnViewHelp);
            _model.CheckAixmMessageFile = new Action<string>(CheckAixmMessageFile);
            _model.RequestDelete = null;
            _prevSearchText = string.Empty;
            _model.SearchText = _prevSearchText;
            _model.SearchTextChanged = new Action<object>(OnSearchTextChanged);
            _model.RuleGroup = ModelFactory.Create(ModelType.RulesGroup);
            _model.RuleGroup.GroupSelected = new Action<string, string>(OnGroupSelected);
            _model.IsGroupPanelVisible = false;

            Load();

            _model.RuleGroup.SetRules(_rules);
        }

        private void Load()
        {
            _prevSearchText = string.Empty;
            _model.SearchText = _prevSearchText;

            _rules = new List<ExpandoObject>();
            GetRules(_rules);
            _model.Items = new ObservableCollection<ExpandoObject>(_rules);
            _model.RuleSelectedItem = (_model.Items as ObservableCollection<ExpandoObject>).FirstOrDefault();
        }

        private void OnGroupSelected(string propName, string filterText)
        {
            var isIndexOf = propName == "Tags";

            var obsCol = _model.Items as ObservableCollection<ExpandoObject>;
            obsCol.Clear();

            if (filterText == null)
            {
                foreach (var item in _rules)
                    obsCol.Add(item);
            }
            else
            {
                foreach (IDictionary<string, object> item in _rules)
                {
                    if (item.TryGetValue(propName, out object propValue) && propValue != null)
                    {
                        if ((isIndexOf && (propValue.ToString().IndexOf(filterText, StringComparison.InvariantCultureIgnoreCase) >=0 )) || 
                            (!isIndexOf && propValue.ToString().Equals(filterText, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            obsCol.Add(item as ExpandoObject);
                        }
                    }
                }
            }

            _model.RuleSelectedItem = (_model.Items as ObservableCollection<ExpandoObject>).FirstOrDefault();
        }

        private void OnNewOrEdit(object arg)
        {
            var isNew = (arg.Equals("new"));
            dynamic rule = null;

            if (isNew)
            {
                rule = ModelFactory.Create(ModelType.RuleEdit);

                var propInfos = typeof(BRuleInfo).GetProperties();
                foreach (var propInfo in propInfos)
                {
                    (rule as IDictionary<string, object>)[propInfo.Name] = (propInfo.PropertyType.IsValueType ?
                        Activator.CreateInstance(propInfo.PropertyType) : null);
                }

                rule.IsActive = true;
                rule.Profile = "Error";
            }
            else
            {
                rule = _model.RuleSelectedItem;
            }

            rule.SbvrEdit = ModelFactory.Create(ModelType.SbvrEdit);
            rule.SbvrEdit.Vocabulary = ModelFactory.Create(ModelType.Vocabulary);

            if (rule.Saved == null)
                rule.Saved = new Func<object, string>(RuleEdit_Saved);

            WindowService.Instance.ShowWindow(rule, _model._window);

            rule.SbvrEdit.Vocabulary._window = rule._window;
        }

        private bool OnNewOrEditCanExecute(object arg)
        {
            if ("edit".Equals(arg))
            {
                return _model.RuleSelectedItem != null && _model.RuleSelectedItem.IsCustom;
            }
            return true;
        }

        private void OnDelete(object arg)
        {
            if (_model.RequestDelete is Func<object, bool> requestDeleteHandler)
            {
                if (requestDeleteHandler?.Invoke(_model))
                {
                    (_rulesDb as IRuleDbSetter)?.DeleteCustomRule(_model.RuleSelectedItem.DbItemId);

                    Load();
                }
            }
        }

        private void OnAbout(object arg)
        {
            var about = ModelFactory.Create(ModelType.About);
            WindowService.Instance.ShowWindow(about, _model._window);
        }

        private void OnViewHelp(object arg)
        {
            var helpPdfFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Help/BRules/Business Rules Manager.pdf");
            if (!File.Exists(helpPdfFileName))
            {
                System.Windows.MessageBox.Show("Help file not found", WindowTitle, 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }
            System.Diagnostics.Process.Start(helpPdfFileName);
        }

        private bool DeleteCanExecute(object arg)
        {
            return _model.RuleSelectedItem != null && _model.RuleSelectedItem.IsCustom;
        }

        private void OnActivateSelected(object arg)
        {
            var isActive = "activate".Equals(arg);
            var list = _model.GetSelectedRules();

            var idList = new List<long>();

            foreach (dynamic rule in list)
            {
                idList.Add(rule.DbItemId);

                //*** Remove IsActivateChanged handler (which updates db). Use bulk update
                object isActiveChangedAction = rule.IsActiveChanged;
                rule.IsActiveChanged = null;

                rule.IsActive = isActive;

                rule.IsActiveChanged = isActiveChangedAction;
            }

            (_rulesDb as IRuleDbSetter).ActivateRule(idList, isActive);
        }

        private void CheckAixmMessageFile(string fileName)
        {
            try
            {
                WindowIsLoading = true;

                var featProvider = new Aran.Aim.BRules.Data.AixmMessageProvider();
                featProvider.Open(fileName);

                var result = new List<Tuple<BRuleInfo, List<Tuple<FeatureType, Guid>>>>();
                var uids = new List<string>();
                var checkCountDist = new Dictionary<string, int>();

                using (var searcher = new FeatureSearcher())
                {
                    //*** Interesting error in register SqliteFunctioins. Works after reopen.
                    searcher.Open(featProvider);
                    searcher.Close();

                    searcher.Open(featProvider);

                    var commandInfos = _rulesDb.GetRuleAndCommandInfos(new RuleFilterValues {
                            { RuleFilterType.Profile, RuleFilterProfileType.Both },
                            { RuleFilterType.IsActive, true }});

                    foreach (var ruleCmdInfoPair in commandInfos)
                    {
                        var cil = CommandInfoList.FromJson(ruleCmdInfoPair.CommandsJson);
                        var resIdentifiers = searcher.Check(cil.Items, out int checkCount);

                        if (resIdentifiers != null && resIdentifiers.Count > 0)
                        {
                            result.Add(new Tuple<BRuleInfo, List<Tuple<FeatureType, Guid>>>(ruleCmdInfoPair.Rule, resIdentifiers));
                            uids.Add(ruleCmdInfoPair.Rule.Uid);
                            checkCountDist.Add(ruleCmdInfoPair.Rule.Uid, checkCount);
                        }
                    }
                }

                var textualDescriptions = _rulesDb.GetTextualDescription(uids);

                dynamic checkReportModel = ModelFactory.Create(ModelType.CheckReport);
                checkReportModel.Fill(result, textualDescriptions, checkCountDist);
                checkReportModel.GetRuleInfo = new Func<string, ExpandoObject>(CheckReportModel_GetRuleInfo);
                WindowService.Instance.ShowWindow(checkReportModel, _model._window);

                WindowIsLoading = false;

            }
            finally
            {
                WindowIsLoading = false;
            }
        }

        private void OnSearchTextChanged(object model)
        {
            if (_model.SearchText != _prevSearchText)
            {
                _prevSearchText = _model.SearchText;

                var obsCol = _model.Items as ObservableCollection<ExpandoObject>;
                obsCol.Clear();

                if (_prevSearchText == string.Empty)
                {
                    foreach(var item in _rules)
                        obsCol.Add(item);
                }
                else
                {
                    foreach (dynamic item in _rules)
                    {
                        if (item.Uid.IndexOf(_prevSearchText, StringComparison.CurrentCultureIgnoreCase) >= 0)
                            obsCol.Add(item);
                    }
                }

                _model.RuleSelectedItem = (_model.Items as ObservableCollection<ExpandoObject>).FirstOrDefault();
            }
        }

        private ExpandoObject CheckReportModel_GetRuleInfo(string uid)
        {
            var pair = _rulesDb.GetRuleAndCommandInfos(
                new RuleFilterValues { { RuleFilterType.UID, new string[] { uid } } })
                .FirstOrDefault();

            if (pair != null)
            {
                PropertyInfo[] propInfos = null;
                return BRuleInfoToModel(pair.Rule, ref propInfos);
            }

            return null;
        }

        static BRuleReportList WriteToTextFile(List<Tuple<BRuleInfo, List<Tuple<FeatureType, Guid>>>> report)
        {
            var ruleReportlist = new BRuleReportList();

            foreach (var item in report)
            {
                var ruleInfo = item.Item1;
                var features = item.Item2;

                var ruleReport = new BRuleReport
                {
                    Uid = ruleInfo.Uid,
                    Name = ruleInfo.Name,
                    Profile = ruleInfo.Profile,
                    Description = ruleInfo.Comment
                };

                var dict = new Dictionary<FeatureType, List<Guid>>();

                foreach (var featTypeGuid in features)
                {
                    if (!dict.TryGetValue(featTypeGuid.Item1, out var guidList))
                    {
                        guidList = new List<Guid>();
                        dict.Add(featTypeGuid.Item1, guidList);
                    }
                    guidList.Add(featTypeGuid.Item2);
                }

                foreach (var featType in dict.Keys)
                {
                    var rfr = new BRuleFeatureReport { FeatureType = featType.ToString() };
                    rfr.Identifiers.AddRange(dict[featType]);
                    ruleReport.Features.Add(rfr);
                }

                ruleReportlist.Items.Add(ruleReport);
            }

            return ruleReportlist;
        }


        private void GetRules(List<ExpandoObject> list)
        {
            if (_rulesDb == null)
            {
                _rulesDb = new RulesDb();
                _rulesDb.Open();
            }

            var ruleCmdInfos = _rulesDb.GetRuleAndCommandInfos(null);

            PropertyInfo[] propInfos = null;

            foreach (var ruleCmdInfo in ruleCmdInfos)
                list.Add(BRuleInfoToModel(ruleCmdInfo.Rule, ref propInfos));
        }

        private ExpandoObject BRuleInfoToModel(BRuleInfo brule, ref PropertyInfo[] propInfos)
        {
            if (propInfos == null)
                propInfos = typeof(BRuleInfo).GetProperties();

            var ruleEditModel = ModelFactory.Create(ModelType.RuleEdit) as IDictionary<string, object>;

            foreach (var propInfo in propInfos)
                ruleEditModel[propInfo.Name] = propInfo.GetValue(brule);

            ruleEditModel["Saved"] = null;
            ruleEditModel["IsActiveChanged"] = new Action<object>(RuleEdit_IsActivateChanged);

            return ruleEditModel as ExpandoObject;
        }

        private string RuleEdit_Saved(object arg)
        {
            try
            {
                var uid = SetRule(arg);

                _model.RuleSelectedItem = null;

                Load();

                foreach (dynamic item in _model.Items)
                {
                    if (item.Uid == uid)
                    {
                        _model.RuleSelectedItem = item;
                        break;
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private void RuleEdit_IsActivateChanged(object sender)
        {
            dynamic ruleEdit = sender;
            if (ruleEdit?.DbItemId > 0)
                (_rulesDb as IRuleDbSetter).ActivateRule(new long[] { ruleEdit.DbItemId }, ruleEdit.IsActive);
        }

        private string SetRule(dynamic ruleModel)
        {
            if (_rulesDb == null)
                throw new Exception("DB is not initialized");

            var cmdInfoJsonText = string.Empty;

            #region Convert CommandInfo text

            string taggedDescription = ruleModel.TaggedDescription;

            var td = new TaggedDocument();
            td.Init(taggedDescription);
            td.Next();

            var brule = new BRule();
            brule.Parse(td, out var isIncomplate);

            if (isIncomplate)
                throw new Exception("Incomplate SBVR text");

            var cmdInfoList = BRuleSqlConverter.ToSqlCommand(brule);
            if (cmdInfoList.Count > 0)
                cmdInfoJsonText = new CommandInfoList(cmdInfoList).ToJson();

            #endregion

            var ruleInfo = new BRuleInfo
            {
                DbItemId = ruleModel.DbItemId,
                Uid = ruleModel.Uid,
                Name = ruleModel.Name,
                Profile = ruleModel.Profile,
                Comment = ruleModel.Comment,
                Category = ruleModel.Category,
                Source = ruleModel.Source,
                TaggedDescription = ruleModel.TaggedDescription,
                IsCustom = true,
                IsActive = ruleModel.IsActive,
                Tags = ruleModel.Tags
            };

            (_rulesDb as IRuleDbSetter).SetCustomRule(ruleInfo, cmdInfoJsonText);

            return ruleInfo.Uid;
        }


        protected override void OnPropertyChanged(string propertyName)
        {
            if (propertyName == "IsGroupPanelVisible")
            {
                if (!_model.IsGroupPanelVisible)
                {
                    _model.RuleGroup.ClearSelectedGroup();
                }
            }
        }

    }
}
