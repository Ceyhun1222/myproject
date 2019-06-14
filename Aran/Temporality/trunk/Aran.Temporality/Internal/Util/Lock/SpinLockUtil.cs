#region

using System;
using System.Threading;
using Aran.Temporality.Internal.Interface.Util;

#endregion

namespace Aran.Temporality.Internal.Util.Lock
{
    internal class SpinLockUtil : ILockUtil
    {
        private SpinLock _spinlock = new SpinLock(true);

        #region ILockUtil Members

        public TResultType ManipulateWithData<TParamType1, TParamType2, TParamType3, TParamType4, TResultType>(
            Func<TParamType1, TParamType2, TParamType3, TParamType4, TResultType> action,
            TParamType1 param1,
            TParamType2 param2,
            TParamType3 param3,
            TParamType4 param4, int priority = 0
            )
        {
            bool lockTaken = false;
            try
            {
                _spinlock.Enter(ref lockTaken);

                return action(param1, param2, param3, param4);
            }
            finally
            {
                if (lockTaken) _spinlock.Exit(false);
            }
        }

        public TResultType ManipulateWithData<TParamType1, TParamType2, TParamType3, TResultType>(
            Func<TParamType1, TParamType2, TParamType3, TResultType> action,
            TParamType1 param1,
            TParamType2 param2,
            TParamType3 param3, int priority = 0
            )
        {
            bool lockTaken = false;
            try
            {
                _spinlock.Enter(ref lockTaken);

                return action(param1, param2, param3);
            }
            finally
            {
                if (lockTaken) _spinlock.Exit(false);
            }
        }

        public TResultType ManipulateWithData<TParamType1, TParamType2, TResultType>(
            Func<TParamType1, TParamType2, TResultType> action,
            TParamType1 param1, TParamType2 param2, int priority = 0)
        {
            bool lockTaken = false;
            try
            {
                _spinlock.Enter(ref lockTaken);

                return action(param1, param2);
            }
            finally
            {
                if (lockTaken) _spinlock.Exit(false);
            }
        }

        public TResultType ManipulateWithData<TParamType, TResultType>(Func<TParamType, TResultType> action,
                                                                       TParamType param, int priority = 0)
        {
            bool lockTaken = false;
            try
            {
                _spinlock.Enter(ref lockTaken);

                return action(param);
            }
            finally
            {
                if (lockTaken) _spinlock.Exit(false);
            }
        }

        public TResultType ManipulateWithData<TResultType>(Func<TResultType> action, int priority = 0)
        {
            bool lockTaken = false;
            try
            {
                _spinlock.Enter(ref lockTaken);

                return action();
            }
            finally
            {
                if (lockTaken) _spinlock.Exit(false);
            }
        }

        public void ManipulateWithData(Action action, int priority = 0)
        {
            bool lockTaken = false;
            try
            {
                _spinlock.Enter(ref lockTaken);

                action();
            }
            finally
            {
                if (lockTaken) _spinlock.Exit(false);
            }
        }

        #endregion
    }
}