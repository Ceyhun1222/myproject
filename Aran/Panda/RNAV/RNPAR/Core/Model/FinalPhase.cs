using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Geometries;
using Aran.Panda.RNAV.RNPAR.Core.Intefrace;
using Aran.Panda.RNAV.RNPAR.Model;
using Aran.PANDA.Common;

namespace Aran.Panda.RNAV.RNPAR.Core.Model
{
    class FinalPhase : Phase<FinalState>
    {
        public override PhaseType PhaseType => PhaseType.Final;

        public override void Init()
        {
            CurrentState = new FinalState();
            CurrentState.Init();
        }

        public override void ReInit()
        {
            Rollback();
        }
    }


}
