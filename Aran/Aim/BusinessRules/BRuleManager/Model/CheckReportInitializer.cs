using Aran.Aim;
using BusinessRules.Data;
using MvvmCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRuleManager.Model
{
    class CheckReportInitializer : BaseInitializer
    {
        private string _prevSearchText;

        public CheckReportInitializer(object model) : base(model)
        {
            DefaultSize = new System.Windows.Size(900, 600);
            WindowTitle = "Report";
            ShowInTaskbar = false;

            _model.Fill = new Action<object, Dictionary<string, string>, Dictionary<string, int>>(FillReport);
            _model.Items = new List<ExpandoObject>();
            _model.ReportSelectedItem = null;
            _model.SearchTextChanged = new Action<object>(OnSearchTextChanged);
            _model.SearchNextPrevCommand = new RelayCommand(OnSearchNextPrev);
            _model.SaveReport = new Action<string>(OnSaveReport);
            _model.RuleCount = 0;
        }

        public void FillReport(object arg, Dictionary<string, string> textualDescriptionDict, Dictionary<string, int> checkCountDict)
        {
            var reportList = arg as List<Tuple<BRuleInfo, List<Tuple<FeatureType, Guid>>>>;

            var list = new List<ExpandoObject>();

            foreach(var bruleFeatList in reportList)
            {
                foreach(var featTypeGuid in bruleFeatList.Item2)
                {
                    dynamic item = new ExpandoObject();
                    item.Type = featTypeGuid.Item1;
                    item.Identifier = featTypeGuid.Item2;
                    item.BRule = bruleFeatList.Item1.Uid;
                    item.CheckCount = checkCountDict[item.BRule];
                    item.BRuleAndProfile = bruleFeatList.Item1.Uid + " / " +
                        (bruleFeatList.Item1.Profile ?? "***") + " / " +
                        ("Checked: " + item.CheckCount);
                    item.Sbvr = textualDescriptionDict[item.BRule];
                    item.BRuleProfile = bruleFeatList.Item1.Profile;
                    list.Add(item);
                }
            }

            _model.Items = list;
            _model.RuleCount = reportList.Count;
        }

        private void OnSearchTextChanged(object arg)
        {
            if (_prevSearchText != _model.SearchText)
            {
                _prevSearchText = _model.SearchText;
                FindIdentifier(1);
            }
        }

        private void OnSearchNextPrev(object obj)
        {
            FindIdentifier("next".Equals(obj) ? 1 : -1);
        }

        private void OnSaveReport(string fileName)
        {
            try
            {
                var model = GetRazorModel();
                var fileContext = Razor.Run<RzReportModel>("ReportTemplate", model);

                File.WriteAllText(fileName, fileContext);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FindIdentifier(int step)
        {
            if (_prevSearchText == null)
                return;

            dynamic startIndex = _model.GetListSelectedIndex();

            for(var i = startIndex + step; i < _model.Items.Count; i += step)
            {
                var item = _model.Items[i];

                if (item.Identifier.ToString().IndexOf(_prevSearchText, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _model.ReportSelectedItem = item;
                    break;
                }
            }
        }

        private RzReportModel GetRazorModel()
        {
            var profileDict = new Dictionary<
                string /*Profile*/, 
                Dictionary<
                    string /*Rule uid*/, 
                    List<RzReportFeature>>>();

            var sbvrDict = new Dictionary<string /*uid*/, string /*sbvr*/>();
            var checkCountDict = new Dictionary<string, int>();

            foreach(dynamic item in _model.Items)
            {
                if (!profileDict.TryGetValue(item.BRuleProfile, out Dictionary<string, List<RzReportFeature>> profile))
                {
                    profile = new Dictionary<string, List<RzReportFeature>>();
                    profileDict.Add(item.BRuleProfile, profile);
                }

                if (!profile.TryGetValue(item.BRule, out List<RzReportFeature> features))
                {
                    features = new List<RzReportFeature>();
                    profile.Add(item.BRule, features);
                    sbvrDict[item.BRule] = item.Sbvr;
                    checkCountDict[item.BRule] = item.CheckCount;
                }

                features.Add(new RzReportFeature { Type = item.Type.ToString(), Identifier = item.Identifier.ToString() });
            }

            var model = new RzReportModel();

            foreach(var profilePair in profileDict)
            {
                var rzProfile = new RzReportProfile { Type = profilePair.Key };
                model.Profiles.Add(rzProfile);

                foreach(var rulePair in profilePair.Value)
                {
                    var rzRule = new RzReportRule { Uid = rulePair.Key };
                    rzRule.Sbvr = sbvrDict[rzRule.Uid];
                    rzRule.CheckCount = checkCountDict[rzRule.Uid];
                    rzProfile.Rules.Add(rzRule);
                    rzRule.Features.AddRange(rulePair.Value);
                }
            }

            return model;
        }
    }

    public class RzReportModel
    {
        public List<RzReportProfile> Profiles { get; } = new List<RzReportProfile>();
    }

    public class RzReportProfile
    {
        public string Type { get; set; }
        public List<RzReportRule> Rules { get; } = new List<RzReportRule>();
    }

    public class RzReportRule
    {
        public string Uid { get; set; }
        public string Sbvr { get; set; }
        public int CheckCount { get; set; }
        public List<RzReportFeature> Features { get; } = new List<RzReportFeature>();
    }

    public class RzReportFeature
    {
        public string Type { get; set; }
        public string Identifier { get; set; }
    }
}
