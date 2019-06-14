#region

using System.Collections.Generic;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Internal.Abstract.Repository;

#endregion

namespace Aran.Temporality._Unused
{
    internal abstract class AbstractByIdDataRepository<TDataType> : AbstractDataRepository<TDataType>
        where TDataType : IFeatureId
    {
        #region Abstract methods

        public abstract IList<TDataType> GetById(IFeatureId fId);

        #endregion
    }
}