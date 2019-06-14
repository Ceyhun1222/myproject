#region

using System;
using System.Collections.Generic;

#endregion

namespace Aran.Temporality._Unused
{
    internal interface IHasMetaDataRepository<out TDataType, TMetaType>
    {
        int RemoveAll(Predicate<TMetaType> predicate);
        IEnumerable<TDataType> Where(Func<TMetaType, bool> predicate);
        IEnumerable<TDataType> GetByMeta(IEnumerable<TMetaType> metaList);
        TDataType GetByMeta(TMetaType meta);
    }
}