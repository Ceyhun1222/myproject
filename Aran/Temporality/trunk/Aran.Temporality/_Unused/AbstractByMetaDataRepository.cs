#region

using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Abstract.State;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Internal.Abstract.Repository;
using Aran.Temporality.Internal.Abstract.Repository.By;
using Aran.Temporality.Internal.Interface.Util;
using Aran.Temporality.Internal.MetaData.Offset;

#endregion

namespace Aran.Temporality._Unused
{
    internal abstract class AbstractByMetaDataRepository<TDataType, TMetaType, TDataRepositoryType,
                                                         TMetaDataRepositoryType, TOffsetType>
        : AbstractByOffsetDataRepository<TDataType, TOffsetType>, IHasMetaDataRepository<TDataType, TMetaType>
        where TDataRepositoryType : AbstractDataRepository<TDataType>
        where TMetaDataRepositoryType : AbstractDataRepository<TMetaType>
        where TDataType : class, IFeatureId
        where TMetaType : class, IHasOffset<TOffsetType>, IFeatureId
    {
        #region Repository properties

        private AbstractByOffsetDataRepository<TDataType, TOffsetType> _dataRepository;

        private AbstractLinqDataRepository<TMetaType> _metaDataRepository;

        public AbstractByOffsetDataRepository<TDataType, TOffsetType> DataRepository
        {
            get
            {
                return _dataRepository ?? (_dataRepository = (AbstractByOffsetDataRepository<TDataType, TOffsetType>)
                                                             Activator.CreateInstance(typeof (TDataRepositoryType), RepositoryName, Marker + "_dt"));
            }
            set { _dataRepository = value; }
        }

        public AbstractLinqDataRepository<TMetaType> MetaDataRepository
        {
            get
            {
                return _metaDataRepository ?? (_metaDataRepository = (AbstractLinqDataRepository<TMetaType>)
                                                                     Activator.CreateInstance(
                                                                         typeof (TMetaDataRepositoryType), RepositoryName, Marker + "_mt"));
            }
            set { _metaDataRepository = value; }
        }

        #endregion

        #region Constructors

        private AbstractByMetaDataRepository()
        {
        }

        protected AbstractByMetaDataRepository(string path, string marker) : this()
        {
            RepositoryName = path;
            Marker = marker;
            Open();
        }

        #endregion

        #region Overrides of AbstractDataRepository<TDataType>

        public override void Open(bool rewrite = false)
        {
            MetaDataRepository.Open(rewrite);
            DataRepository.Open(rewrite);
        }

        public override void Close()
        {
            MetaDataRepository.Close();
            DataRepository.Close();
        }

        public override void RemoveAll()
        {
            MetaDataRepository.RemoveAll();
            DataRepository.RemoveAll();
        }

        public override TOffsetType AddItemAndGetOffset(TDataType item)
        {
            TOffsetType offset = DataRepository.AddItemAndGetOffset(item);
            TMetaType meta = GetMetaData(item);
            meta.Offset = offset;
            MetaDataRepository.Add(meta);
            return offset;
        }

        public override void Remove(TDataType item)
        {
            MetaDataRepository.Remove(GetMetaData(item));
            DataRepository.Remove(item);
        }

        public override IEnumerable<TDataType> Where(Func<TDataType, bool> predicate)
        {
            return DataRepository.Where(predicate);
        }

        public override void Dispose()
        {
            MetaDataRepository.Dispose();
            DataRepository.Dispose();
        }

        #endregion

        #region Implementation of IHasMetaDataRepository

        public int RemoveAll(Predicate<TMetaType> predicate)
        {
            IEnumerable<TMetaType> toBeDeletedMeta = MetaDataRepository.Where(new Func<TMetaType, bool>(predicate));
            IEnumerable<TDataType> toBeDeleted = GetByMeta(toBeDeletedMeta);
            int count = 0;
            foreach (var data in toBeDeleted)
            {
                count++;
                Remove(data);
            }
            return count;
        }

        public IEnumerable<TDataType> Where(Func<TMetaType, bool> predicate)
        {
            return GetByMeta(MetaDataRepository.Where(predicate));
        }

        public IEnumerable<TDataType> GetByMeta(IEnumerable<TMetaType> metaList)
        {
            return metaList.Select(GetByMeta).ToList();
        }

        public TDataType GetByMeta(TMetaType meta)
        {
            if (meta is OffsetEventMetaData<TOffsetType>)
            {
                return GetByOffset((meta as OffsetEventMetaData<TOffsetType>).Offset);
            }

            if (meta is OffsetStateMetaData<TOffsetType>)
            {
                return GetByOffset((meta as OffsetStateMetaData<TOffsetType>).Offset);
            }

            throw new Exception("not supported argument in GetByMeta, should be EventMetaData or StateMetaData only");
        }

        public override TDataType GetByOffset(TOffsetType offset)
        {
            return DataRepository.GetByOffset(offset);
        }

        public TMetaType GetMetaData(TDataType data)
        {
            if (data is AbstractEventMetaData)
            {
                return
                    (TMetaType)
                    Activator.CreateInstance(typeof (TMetaType), data as AbstractEventMetaData);
            }
            if (data is AbstractStateMetaData)
            {
                return
                    (TMetaType)
                    Activator.CreateInstance(typeof (TMetaType), data as AbstractStateMetaData);
            }

            throw new Exception(
                "unsupported argument in GetMetaData, must be AbstractEventMetaData or AbstractStateMetaData only");
        }

        #endregion
    }
}