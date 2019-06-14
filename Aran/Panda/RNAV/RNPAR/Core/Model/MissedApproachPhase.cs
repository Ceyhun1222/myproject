using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Panda.RNAV.RNPAR.Core.Intefrace;
using Aran.Panda.RNAV.RNPAR.Model;

namespace Aran.Panda.RNAV.RNPAR.Core.Model
{
    class MissedApproachPhase : Phase<MissedApproachState>
    {
        public override PhaseType PhaseType => PhaseType.MissedApproach;

        public override void Init()
        {
            CurrentState = new MissedApproachState();
            CurrentState.Init();
        }
    }
}
