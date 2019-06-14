using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Panda.RNAV.RNPAR.Core.Model;
using Aran.Panda.RNAV.RNPAR.Model;

namespace Aran.Panda.RNAV.RNPAR.Rules.Core
{
    abstract class CalculationRule : Rule, ICalculationRule
    {



        protected CalculationRule(SegmentType phase, string comment) : base(phase, comment)
        {
        }

        public object Calculate(params object[] list)
        {
            Params = list;
            CheckParameters(Params);
            Result = GetResult(Params);
            return Result;
        }

        protected abstract object GetResult(object[]  list);
    }
}
