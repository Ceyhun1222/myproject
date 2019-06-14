using Aran.Panda.RNAV.RNPAR.Model;
using Aran.Panda.RNAV.RNPAR.Rules.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Panda.RNAV.RNPAR.Core.Intefrace
{
    interface IState<T>
    {
        void Commit();
        StateType Type { get; }
        T Next();
        T Previous();
        bool HasNext();
        bool HasPrevoius();
        T Copy();
        void SetNext(T next);
        void SetPrevious(T previous);
        IList<IRule> Rules { get; }
        void Clear();
        void ReCreate();

    }
}
