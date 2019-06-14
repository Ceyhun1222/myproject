using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules.Parser
{
    public class SbvrCondition
    {

    }

    public class SbvrConditionList
    {
        public SbvrConditionList()
        {
            Conditions = new List<SbvrCondition>();
        }

        public SbvrLogicType LogicType { get; set; }

        public List<SbvrCondition> Conditions { get; private set; }
    }

    public enum SbvrLogicType { And, Or }
}
