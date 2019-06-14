using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Panda.RNAV.RNPAR.Core.Model
{
    class InitialState: State<InitialState>
    {
        public InitialState()
        {
            _type = RNPAR.Model.StateType.Initial;
        }

        protected override InitialState CreateNewState()
        {
            return new InitialState();
        }
    }
}
