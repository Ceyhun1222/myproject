#region

using System;
using Aran.Temporality.Internal.Interface.Util;

#endregion

namespace Aran.Temporality.Internal.Util.Lock
{
    internal class LockUtil : ILockUtil
    {
        public static bool Locked;

        private static int _lastPriority;
        private readonly object _lockObj = new object();

        public static void SetPriority(int p)
        {
            _lastPriority = p;
            if (p != 0)
            {
            }
        }

        #region Implementation of ILockUtil


       public TResultType ManipulateWithData<TParamType1, TParamType2, TParamType3, TParamType4, TParamType5, TParamType6, TParamType7, TParamType8, TResultType>(
       Func<TParamType1, TParamType2, TParamType3, TParamType4, TParamType5, TParamType6, TParamType7, TParamType8, TResultType> action,
       TParamType1 param1,
       TParamType2 param2,
       TParamType3 param3,
       TParamType4 param4,
       TParamType5 param5,
       TParamType6 param6,
       TParamType7 param7,
       TParamType8 param8,
       int priority = 0)
        {
            TResultType result;
            lock (_lockObj)
            {
                SetPriority(priority);
                Locked = true;
                result = action(param1, param2, param3, param4, param5, param6, param7, param8);
            }
            Locked = false;
            return result;
        }

        public TResultType ManipulateWithData<TParamType1, TParamType2, TParamType3, TParamType4, TParamType5, TParamType6, TParamType7, TResultType>(
         Func<TParamType1, TParamType2, TParamType3, TParamType4, TParamType5, TParamType6, TParamType7, TResultType> action,
         TParamType1 param1,
         TParamType2 param2,
         TParamType3 param3,
         TParamType4 param4,
         TParamType5 param5,
         TParamType6 param6,
         TParamType7 param7,
         int priority = 0)
        {
            TResultType result;
            lock (_lockObj)
            {
                SetPriority(priority);
                Locked = true;
                result = action(param1, param2, param3, param4, param5, param6, param7);
            }
            Locked = false;
            return result;
        }

        public TResultType ManipulateWithData<TParamType1, TParamType2, TParamType3, TParamType4, TParamType5, TParamType6, TResultType>(
         Func<TParamType1, TParamType2, TParamType3, TParamType4, TParamType5, TParamType6, TResultType> action,
         TParamType1 param1,
         TParamType2 param2,
         TParamType3 param3,
         TParamType4 param4,
         TParamType5 param5,
         TParamType6 param6,
         int priority = 0)
        {
            TResultType result;
            lock (_lockObj)
            {
                SetPriority(priority);
                Locked = true;
                result = action(param1, param2, param3, param4, param5, param6);
            }
            Locked = false;
            return result;
        }

        public TResultType ManipulateWithData<TParamType1, TParamType2, TParamType3, TParamType4, TParamType5, TResultType>(
           Func<TParamType1, TParamType2, TParamType3, TParamType4, TParamType5, TResultType> action,
           TParamType1 param1,
           TParamType2 param2,
           TParamType3 param3,
           TParamType4 param4, 
           TParamType5 param5, 
           int priority = 0)
        {
            TResultType result;
            lock (_lockObj)
            {
                SetPriority(priority);
                Locked = true;
                result = action(param1, param2, param3, param4, param5);
            }
            Locked = false;
            return result;
        }

        public TResultType ManipulateWithData<TParamType1, TParamType2, TParamType3, TParamType4, TResultType>(
            Func<TParamType1, TParamType2, TParamType3, TParamType4, TResultType> action,
            TParamType1 param1,
            TParamType2 param2,
            TParamType3 param3,
            TParamType4 param4, int priority = 0)
        {
            TResultType result;
            lock (_lockObj)
            {
                SetPriority(priority);
                Locked = true;
                result = action(param1, param2, param3, param4);
            }
            Locked = false;
            return result;
        }

        public TResultType ManipulateWithData<TParamType1, TParamType2, TParamType3, TResultType>(
            Func<TParamType1, TParamType2, TParamType3, TResultType> action,
            TParamType1 param1,
            TParamType2 param2,
            TParamType3 param3, int priority = 0)
        {
            TResultType result;
            lock (_lockObj)
            {
                SetPriority(priority);
                Locked = true;
                result = action(param1, param2, param3);
            }
            Locked = false;
            return result;
        }

        public TResultType ManipulateWithData<TParamType1, TParamType2, TResultType>(
            Func<TParamType1, TParamType2, TResultType> action,
            TParamType1 param1,
            TParamType2 param2, int priority = 0)
        {
            TResultType result;
            lock (_lockObj)
            {
                SetPriority(priority);
                Locked = true;
                result = action(param1, param2);
            }
            Locked = false;
            return result;
        }

        public TResultType ManipulateWithData<TParamType, TResultType>(
            Func<TParamType, TResultType> action,
            TParamType param, int priority = 0)
        {
            TResultType result;
            lock (_lockObj)
            {
                SetPriority(priority);
                Locked = true;
                result = action(param);
            }
            Locked = false;
            return result;
        }

        public TResultType ManipulateWithData<TResultType>(Func<TResultType> action, int priority = 0)
        {
            TResultType result;
            lock (_lockObj)
            {
                SetPriority(priority);
                Locked = true;
                result = action();
            }
            Locked = false;
            return result;
        }

        public void ManipulateWithData(Action action, int priority = 0)
        {
            lock (_lockObj)
            {
                SetPriority(priority);
                Locked = true;
                action();
            }
            Locked = false;
        }

        #endregion
    }
}