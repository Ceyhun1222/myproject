using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Util;
using Aran.Temporality.Internal.Interface.Storage;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Abstract;
using Aran.Temporality.Internal.WorkFlow.BusinessRules.Util;
using BusinessRules.Data;

namespace Aran.Temporality.Internal.Implementation.Storage.NHibernateStorage
{
    internal class BusinessRulesStorage : CrudStorageTemplate<BusinessRule>, IBusinessRulesStorage
    {
        private static IList<BusinessRuleUtil>  _rules;

        public IList<BusinessRuleUtil> GetBusinessRules()
        {
            if (_rules==null)
            {
                var rulesActiveList=GetAllEntities();
                _rules = new List<BusinessRuleUtil>();

                var protoType = typeof (AbstractBusinessRule);
                var types = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(type => protoType.IsAssignableFrom(type) && !type.IsAbstract);

                using (var ruleDb = new RulesDb())
                {
                    ruleDb.Open();

                    var commandInfos = ruleDb.GetRuleAndCommandInfos(new RuleFilterValues {
                        { RuleFilterType.Profile, RuleFilterProfileType.Both}
                    });

                    foreach (var cmdInfoTuple in commandInfos)
                    {
                        var bRule = cmdInfoTuple.Rule;

                        var id = (int) Crc32.Instance.ComputeChecksum(Encoding.UTF8.GetBytes(bRule.Uid));
                        var correspondingEntity = rulesActiveList.FirstOrDefault(t => t.RuleId == id);
                        if (correspondingEntity == null)
                        {
                            correspondingEntity = new BusinessRule
                            {
                                IsActive = false,
                                RuleId = id
                            };

                            var newId = CreateEntity(correspondingEntity);
                            correspondingEntity.Id = newId;
                        }

                        _rules.Add(new BusinessRuleUtil
                        {
                            Level = bRule.Profile,
                            Name = bRule.Name,
                            Comments = bRule.Comment,
                            UID = bRule.Uid,
                            RuleEntityId = correspondingEntity.Id,
                            IsActive = correspondingEntity.IsActive,
                            Source = bRule.Source,
                            Category = bRule.Category,
                            Id = id
                        });
                    }
                }
                
                foreach (var type in types)
                {
                    var rule = Activator.CreateInstance(type) as AbstractBusinessRule;
                    if (rule==null) continue;

                    if(_rules.Any(t => t.UID.Equals(rule.UID)))
                        continue;

                    var id = rule.Id;

                    var correspondingEntity=rulesActiveList.FirstOrDefault(t => t.RuleId == id);
                    if (correspondingEntity == null)
                    {
                        correspondingEntity = new BusinessRule
                        {
                            IsActive = false,
                            RuleId = id
                        };
                        
                        var newId=CreateEntity(correspondingEntity);
                        correspondingEntity.Id = newId;
                    }

                    _rules.Add(new BusinessRuleUtil
                                   {
                                       ApplicableProperty = rule.GetApplicableProperty(),
                                       ApplicableType = rule.GetApplicableType() == null ? null : rule.GetApplicableType().Name,
                                       Category = rule.Category(),
                                       Comments = rule.Comments(),
                                       Level = rule.Level(),
                                       Svbr = rule.Svbr(),
                                       Source = rule.Source(),
                                       Name = rule.Name(),
                                       IsActive = correspondingEntity.IsActive,
                                       RuleEntityId = correspondingEntity.Id,
                                       Id=id,
                                       UID = rule.UID
                                   });


                }
            }
            return _rules;
        }

        public void ActivateRule(int ruleId, bool active)
        {
            using (var session = Repository.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var rule = session.Get<BusinessRule>(ruleId); 
                    if (rule != null)
                    {
                        rule.IsActive = active;
                        session.Update(rule);
                    }
                    else
                    {
                        session.Save(new BusinessRule
                                         {
                                             Id = ruleId,
                                             IsActive = active
                                         });
                    }
                    transaction.Commit();

                    var ruleActive=GetBusinessRules().FirstOrDefault(t => t.RuleEntityId == ruleId);
                    if (ruleActive!=null)
                    {
                        ruleActive.IsActive = active;
                    }
                    
                }
            }
        }

    }
}
