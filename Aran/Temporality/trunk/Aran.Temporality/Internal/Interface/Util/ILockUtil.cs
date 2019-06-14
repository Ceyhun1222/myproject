#region

using System;

#endregion

namespace Aran.Temporality.Internal.Interface.Util
{
    internal interface ILockUtil
    {
        TResultType ManipulateWithData<TParamType1, TParamType2, TParamType3, TParamType4, TResultType>(
            Func<TParamType1, TParamType2, TParamType3, TParamType4, TResultType> action,
            TParamType1 param1,
            TParamType2 param2,
            TParamType3 param3,
            TParamType4 param4,
            int priority = 0
            );

        TResultType ManipulateWithData<TParamType1, TParamType2, TParamType3, TParamType4, TParamType5, TResultType>(
           Func<TParamType1, TParamType2, TParamType3, TParamType4, TParamType5, TResultType> action,
           TParamType1 param1,
           TParamType2 param2,
           TParamType3 param3,
           TParamType4 param4,
           TParamType5 param5,
           int priority = 0
           );

        TResultType ManipulateWithData<TParamType1, TParamType2, TParamType3, TParamType4, TParamType5, TParamType6, TResultType>(
          Func<TParamType1, TParamType2, TParamType3, TParamType4, TParamType5, TParamType6, TResultType> action,
          TParamType1 param1,
          TParamType2 param2,
          TParamType3 param3,
          TParamType4 param4,
          TParamType5 param5,
          TParamType6 param6,
          int priority = 0
          );

        TResultType ManipulateWithData<TParamType1, TParamType2, TParamType3, TParamType4, TParamType5, TParamType6, TParamType7, TParamType8, TResultType>(
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
         );

        TResultType ManipulateWithData<TParamType1, TParamType2, TParamType3, TParamType4, TParamType5, TParamType6, TParamType7, TResultType>(
          Func<TParamType1, TParamType2, TParamType3, TParamType4, TParamType5, TParamType6, TParamType7, TResultType> action,
          TParamType1 param1,
          TParamType2 param2,
          TParamType3 param3,
          TParamType4 param4,
          TParamType5 param5,
          TParamType6 param6,
          TParamType7 param7,
          int priority = 0
          );

        TResultType ManipulateWithData<TParamType1, TParamType2, TParamType3, TResultType>(
            Func<TParamType1, TParamType2, TParamType3, TResultType> action,
            TParamType1 param1,
            TParamType2 param2,
            TParamType3 param3,
            int priority = 0);

        TResultType ManipulateWithData<TParamType1, TParamType2, TResultType>(
            Func<TParamType1, TParamType2, TResultType> action,
            TParamType1 param1, TParamType2 param2, int priority = 0);

        TResultType ManipulateWithData<TParamType, TResultType>(
            Func<TParamType, TResultType> action,
            TParamType param,
            int priority = 0);

        TResultType ManipulateWithData<TResultType>(
            Func<TResultType> action,
            int priority = 0);

        void ManipulateWithData(Action action, int priority = 0);
    }
}