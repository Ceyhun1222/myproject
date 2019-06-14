using Aran.Geometries;
using Aran.Panda.RNAV.RNPAR.Core.Intefrace;
using Aran.Panda.RNAV.RNPAR.Model;
using Aran.Panda.RNAV.RNPAR.Rules.Core;
using Aran.PANDA.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Panda.RNAV.RNPAR.Context;
using Aran.Panda.RNAV.RNPAR.Utils;

namespace Aran.Panda.RNAV.RNPAR.Core.Model
{
    abstract class State<T> : IState<T>
    {
        protected StateType _type;
        protected T _next;
        protected T _previous;
        protected IList<IRule> _rules = new List<IRule>();

        

        public StateType Type => _type;

        
        public virtual bool HasNext() => _next != null;
        public virtual bool HasPrevoius() => _previous != null;
        public virtual T Next() => _next;
        public virtual T Previous() => _previous;
        public virtual void SetNext(T next)
        {
            _next = next;
        }

        public virtual void SetPrevious(T previous)
        {
            _previous = previous;
        }

        protected abstract T CreateNewState();

        public virtual T Copy()
        {
            return BaseCopy();
        }

        protected T BaseCopy()
        {
            var state = CreateNewState();
            (state as State<T>)._rules = new List<IRule>();
            foreach (var rule in _rules)
            {
                (state as State<T>).Rules.Add(rule);
            }

            return state;
        }

        public virtual void Commit()
        {
            
        }



        public IList<IRule> Rules => _rules;
        public virtual void Clear()
        {
           
        }

        public virtual void ReCreate()
        {
           
        }

        
    }
}
