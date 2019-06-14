using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Panda.RNAV.RNPAR.Core.Intefrace;
using Aran.Panda.RNAV.RNPAR.Model;

namespace Aran.Panda.RNAV.RNPAR.Core.Model
{
    class IntermediatePhase : Phase<IntermediateState>
    {
        public override PhaseType PhaseType => PhaseType.Intermediate;

        public override void Init()
        {
            CurrentState = new IntermediateState();
            CurrentState.Init();
        }
    }
}
