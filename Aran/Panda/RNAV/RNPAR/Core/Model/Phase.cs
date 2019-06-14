using Aran.Panda.RNAV.RNPAR.Core.Intefrace;
using Aran.Panda.RNAV.RNPAR.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Panda.RNAV.RNPAR.Core.Model
{ 
    abstract class Phase<T> where T: IState<T> 
    {
        public abstract PhaseType PhaseType { get; }

        public bool IsCommitted => CommittedState != null;

        protected T CommittedState { get; set; }
        public T CurrentState { get;  protected set; }

        public virtual void Init()
        {

        }

        public virtual void Init(T state)
        {

        }

        public virtual void ReInit()
        {

        }

        public virtual void Commit()
        {

            var tmp = CommittedState;
            CommittedState = CurrentState.Copy();
            CurrentState.Commit();
            CurrentState.SetPrevious(CommittedState);
            CommittedState.SetNext(CurrentState);
            if (tmp != null)
            {
                tmp.SetNext(CommittedState);
                CommittedState.SetPrevious(tmp);
            }
        }



        public virtual void Rollback()
        {
            if (CommittedState == null)
                throw new InvalidOperationException();

            CurrentState.Clear();
            CurrentState = CommittedState.Copy();
            if (CommittedState.HasPrevoius())
            {
                var previousState = CommittedState.Previous();
                CommittedState = previousState;
            }
            else
            {
                CommittedState = default(T);
            }
            CurrentState.SetPrevious(CommittedState);
            CommittedState?.SetNext(CurrentState);
            CurrentState.ReCreate();
        }

        public virtual void Clear()
        {
            CurrentState.Clear();
        }

    }
}
