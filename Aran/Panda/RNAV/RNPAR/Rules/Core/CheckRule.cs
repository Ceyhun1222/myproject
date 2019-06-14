using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Panda.RNAV.RNPAR.Core.Model;
using Aran.Panda.RNAV.RNPAR.Model;

namespace Aran.Panda.RNAV.RNPAR.Rules.Core
{
    abstract class CheckRule: Rule, ICheckRule 
    {
        protected CheckRule(SegmentType phase, string comment) : base(phase, comment)
        {
        }

        public bool Check(params object[] list)
        {
            CheckParameters(list);
            return GetResult(list);
        }

        protected abstract bool GetResult(object[] list);
    }
}
