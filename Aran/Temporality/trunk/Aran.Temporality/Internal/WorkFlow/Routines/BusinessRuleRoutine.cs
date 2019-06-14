using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim;
using Aran.Aim.BusinessRules;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Util;
using Aran.Temporality.Common.Util;
using Aran.Temporality.Internal.WorkFlow.BusinessRules;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Util;
using BusinessRules.Data;
using NHibernate.Linq;

namespace Aran.Temporality.Internal.WorkFlow.Routines
{
    internal class BusinessRuleRoutine : AbstractCheckRoutine
    {
        private static readonly Type[] BusinessRulesTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(type => typeof(AbstractBusinessRule).IsAssignableFrom(type) && !type.IsAbstract).ToArray();

        #region Implementation of ICheckPublicSlotRoutine
       
        private AbstractBusinessRule[] _rules;
        public override void InitPrivateSlot()
        {
            if (_rules == null)
            {
                _rules = BusinessRulesTypes.Select(t => (AbstractBusinessRule)Activator.CreateInstance(t)).ToArray();
            }
            foreach (var rule in _rules)
            {
                rule.Context = Context;
            }
        }

        public override int GetReportType()
        {
            return (int) ReportType.BusinessRulesReport;
        }

        public override bool CheckFeature(AimFeature aimFeature, List<ProblemReportUtil> problems)
        {
            CurrentOperationStatus.NextOperation();
            NextFeature();
            var result = true;
            var feature = aimFeature.Feature;
            if (_rules.Length == 0)
                return true;
            var activeRules = _rules.First().Context.Service.GetBusinessRules().Where(r => r.IsActive).ToList();
            
            var selectedRules = _rules.Where(rule => activeRules
                .Any(r => (r.Id == rule.Id && r.Id != 0 && rule.Id != 0 ) || rule.UID == null)) ;
            (selectedRules.Where(rule => !rule.CheckFeature(feature))
                .Select(rule => new BusinessRuleProblemReportUtil
                {
                    FeatureType = feature.FeatureType,
                    Guid = feature.Identifier,
                    RuleId = rule.Id 
                })).ForEach(problem =>
                {
                    problems.Add(problem);
                    result = false;
                });
            if (!result)
            {
                NextError();
            }
            return result;
        }

        public override bool CheckPrivateSlot()
        {
            FeaturesProcessed = 0;
            ErrorFound = 0;
            //
            InitPrivateSlot();
            //calculated amount of features to process
            var union = AllFeatureTypes.Cast<FeatureType>().Aggregate((IEnumerable<AimFeature>)new List<AimFeature>(),
                    (current, featureType) => current.Union(Context.LoadStates(featureType)));

            var totalCount = union.Count();
            CurrentOperationStatus.NextTask(totalCount);
            //union all features

            var problems = new ConcurrentBag<List<ProblemReportUtil>>();
            var result = true;


            // New business rules
            var errorList = new List<System.Tuple<BRuleInfo, List<System.Tuple<FeatureType, Guid>>>>();


            var provider = new TemporalityFeatureProvider(Context);
            using (var searcher = new FeatureSearcher())
            {
                searcher.Open(provider);

                using (var ruleDb = new RulesDb())
                {
                    ruleDb.Open();

                    var commandInfos = ruleDb.GetRuleAndCommandInfos(new RuleFilterValues {
                        { RuleFilterType.Profile, RuleFilterProfileType.Both},
                        { RuleFilterType.IsActive, true }
                    });

                    foreach (var cmdInfoTuple in commandInfos)
                    {
                        var cil = CommandInfoList.FromJson(cmdInfoTuple.CommandsJson);
                        var resIdentifiers = searcher.Check(cil.Items, out int checkCount);

                        if (resIdentifiers != null && resIdentifiers.Count > 0)
                            errorList.Add(new System.Tuple<BRuleInfo, List<System.Tuple<FeatureType, Guid>>>(cmdInfoTuple.Rule, resIdentifiers));
                    }
                }
            }

            var subres = new List<ProblemReportUtil>();
            foreach (var brError in errorList)
            {
                foreach (var error in brError.Item2)
                {
                    subres.Add(new BusinessRuleProblemReportUtil { FeatureType = error.Item1, Guid = error.Item2, RuleId = unchecked((int)Crc32.Instance.ComputeChecksum(Encoding.UTF8.GetBytes(brError.Item1.Uid))) });
                }

            }


            // Old business rules
            union.ForEach(t => CheckFeature(t, subres));
            problems.Add(subres);

            result = problems.Count > 0;
            result = !result;

            ReleasePrivateSlot();
            //save result
            Context.Service.UpdateProblemReport(new ProblemReport
            {
                PrivateSlotId = Context.PrivateSlot.Id,
                PublicSlotId = 0,//just private slot
                ReportType = GetReportType(),
                ReportData = FormatterUtil.MultiCollectionToBytes(problems.ToArray(), false)
            });
            return result;
        }

        #endregion
    }
}
