#region

using System;
using System.Collections;
using System.Collections.Generic;
using Aran.Aim.Data.Filters;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Internal.MetaData.Offset;

#endregion

namespace Aran.Temporality.Internal.Abstract.Repository
{
    internal abstract class AbstractOffsetDataRepository<TDataType, TKeyType> : AbstractFileDataRepository<TDataType, TKeyType>
    {
        public abstract IEnumerable GetMetaDatas();
        public abstract TKeyType Add(TDataType item, OffsetEventMetaData<TKeyType> meta);
        public abstract List<Guid> GetFilteredId(Filter filter, int featureTypeId, IList<Guid> ids);
    }
}