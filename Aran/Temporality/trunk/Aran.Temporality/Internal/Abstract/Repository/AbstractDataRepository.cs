#region

using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;

#endregion

namespace Aran.Temporality.Internal.Abstract.Repository
{
    internal abstract class AbstractDataRepository<TDataType, TKeyType> : IDisposable
    {
        #region Properties

        public string RepositoryName { get; set; } //folder path

        public string Marker { get; set; } //unique marker indicating what kind of objects to be stored

        #endregion

        #region Abstract methods

        public abstract void Open(bool rewrite = false);

        public abstract void Close();

        public abstract TKeyType Add(TDataType item);

        public abstract TDataType Get(TKeyType key, int featureTypeId, Projection projection = null);

        public virtual List<TDataType> Get(IList<TKeyType> keys, int featureTypeId, Projection projection = null)
        {
            return keys.Select(key => Get(key, featureTypeId)).ToList();
        }

        public virtual List<TDataType> Get(Filter filter, int featureTypeId, Projection projection = null)
        {
            return new List<TDataType>();
        }

        public abstract void Remove(TDataType item);

        public abstract void RemoveByKey(TKeyType key, int featureTypeId);

        public abstract void RemoveAll();

        #endregion

        #region Implementation of IDisposable

        public abstract void Dispose();

        #endregion
    }
}