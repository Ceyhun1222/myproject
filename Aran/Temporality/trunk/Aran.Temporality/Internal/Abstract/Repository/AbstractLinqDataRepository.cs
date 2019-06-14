#region

using System;
using System.Collections;
using System.Collections.Generic;

#endregion

namespace Aran.Temporality.Internal.Abstract.Repository
{
    internal abstract class AbstractLinqDataRepository<TDataType, TKey> : AbstractDataRepository<TDataType, TKey>, IEnumerable
    {
        public abstract IEnumerable<TDataType> Where(Func<TDataType, bool> predicate);
        public abstract TDataType Poke();
        public abstract IEnumerator GetEnumerator();
    }
}