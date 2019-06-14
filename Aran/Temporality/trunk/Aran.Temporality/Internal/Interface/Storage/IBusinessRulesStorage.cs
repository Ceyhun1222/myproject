using System.Collections.Generic;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Util;

namespace Aran.Temporality.Internal.Interface.Storage
{
    internal interface IBusinessRulesStorage : ICrudStorage<BusinessRule>
    {
        IList<BusinessRuleUtil> GetBusinessRules();
        void ActivateRule(int ruleId, bool active);
    }
}
