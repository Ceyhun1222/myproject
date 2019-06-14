#region

using System;
using System.Threading;
using Aran.Temporality.Internal.Interface.Util;

#endregion

namespace Aran.Temporality.Internal.Util.Lock
{
    internal class MonitorUtil : ILockUtil
    {
        private static readonly Object Obj = new Object();

        #region ILockUtil Members

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
          int priority = 0
          )
        {
            bool lockTaken = false;
            try
            {
                Monitor.Enter(Obj, ref lockTaken);
                //Console.WriteLine("taken " + priority);
                return action(param1, param2, param3, param4, param5, param6, param7, param8);
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(Obj);
                    //Console.WriteLine("released " + priority);
                }
            }
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
           int priority = 0
           )
        {
            bool lockTaken = false;
            try
            {
                Monitor.Enter(Obj, ref lockTaken);
                //Console.WriteLine("taken " + priority);
                return action(param1, param2, param3, param4, param5, param6, param7);
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(Obj);
                    //Console.WriteLine("released " + priority);
                }
            }
        }


        public TResultType ManipulateWithData<TParamType1, TParamType2, TParamType3, TParamType4, TParamType5, TParamType6, TResultType>(
           Func<TParamType1, TParamType2, TParamType3, TParamType4, TParamType5, TParamType6, TResultType> action,
           TParamType1 param1,
           TParamType2 param2,
           TParamType3 param3,
           TParamType4 param4,
           TParamType5 param5,
           TParamType6 param6,
           int priority = 0
           )
        {
            bool lockTaken = false;
            try
            {
                Monitor.Enter(Obj, ref lockTaken);
                //Console.WriteLine("taken " + priority);
                return action(param1, param2, param3, param4, param5, param6);
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(Obj);
                    //Console.WriteLine("released " + priority);
                }
            }
        }


        public TResultType ManipulateWithData<TParamType1, TParamType2, TParamType3, TParamType4, TParamType5, TResultType>(
            Func<TParamType1, TParamType2, TParamType3, TParamType4, TParamType5, TResultType> action,
            TParamType1 param1,
            TParamType2 param2,
            TParamType3 param3,
            TParamType4 param4, 
            TParamType5 param5, 
            int priority = 0
            )
        {
            bool lockTaken = false;
            try
            {
                Monitor.Enter(Obj, ref lockTaken);
                //Console.WriteLine("taken " + priority);
                return action(param1, param2, param3, param4, param5);
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(Obj);
                    //Console.WriteLine("released " + priority);
                }
            }
        }

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
                Monitor.Enter(Obj, ref lockTaken);
                //Console.WriteLine("taken " + priority);
                return action(param1, param2, param3, param4);
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(Obj);
                    //Console.WriteLine("released " + priority);
                }
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
                Monitor.Enter(Obj, ref lockTaken);
                //Console.WriteLine("taken " + priority);
                return action(param1, param2, param3);
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(Obj);
                    //Console.WriteLine("released " + priority);
                }
            }
        }

        public TResultType ManipulateWithData<TParamType1, TParamType2, TResultType>(
            Func<TParamType1, TParamType2, TResultType> action,
            TParamType1 param1, TParamType2 param2, int priority = 0)
        {
            bool lockTaken = false;
            try
            {
                Monitor.Enter(Obj, ref lockTaken);
                //Console.WriteLine("taken " + priority);
                return action(param1, param2);
            }
            finally
            {
                if (lockTaken) Monitor.Exit(Obj);
            }
        }

        public TResultType ManipulateWithData<TParamType, TResultType>(Func<TParamType, TResultType> action,
                                                                       TParamType param, int priority = 0)
        {
            bool lockTaken = false;
            try
            {
                Monitor.Enter(Obj, ref lockTaken);
                //Console.WriteLine("taken " + priority);
                return action(param);
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(Obj);
                    //Console.WriteLine("released " + priority);
                }
            }
        }

        public TResultType ManipulateWithData<TResultType>(Func<TResultType> action, int priority = 0)
        {
            bool lockTaken = false;
            try
            {
                Monitor.Enter(Obj, ref lockTaken);
                //Console.WriteLine("taken " + priority);
                return action();
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(Obj);
                    //Console.WriteLine("released " + priority);
                }
            }
        }

        public void ManipulateWithData(Action action, int priority = 0)
        {
            bool lockTaken = false;
            try
            {
                Monitor.Enter(Obj, ref lockTaken);
                //Console.WriteLine("taken " + priority);
                action();
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(Obj);
                    //Console.WriteLine("released " + priority);
                }
            }
        }

        #endregion
    }
}