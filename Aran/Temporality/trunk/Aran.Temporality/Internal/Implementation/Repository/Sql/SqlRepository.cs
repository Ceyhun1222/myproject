using System;
using System.Collections;
using System.Collections.Generic;
using Aran.Aim.Data;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Util;
using Aran.Temporality.Internal.Abstract.Repository;
using Aran.Temporality.Internal.Implementation.Storage;
using Aran.Temporality.Internal.Implementation.Storage.NHibernateStorage;

namespace Aran.Temporality.Internal.Implementation.Repository.Sql
{
    internal class SqlRepository<TDataType> : AbstractLinqDataRepository<TDataType, long>
        where TDataType : class
    {
        //init this somehow
        private EventEntityStorage _eventEntityStorage;
        private EventEntityStorage EventEntityStorage
        {
            get { return _eventEntityStorage ?? (_eventEntityStorage = new EventEntityStorage { Repository = StorageService.HibernateRepository }); }
            set { _eventEntityStorage = value; }
        }

        #region Overrides of AbstractDataRepository<TDataType>

        public override void Open(bool rewrite = false)
        {
            var t = EventEntityStorage.GetType();
            if (rewrite)
            {
                RemoveAll();
            }
        }

        public override void Close()
        {
        }

        public override void RemoveAll()
        {
            EventEntityStorage.DeleteAll();
        }

        public override void Remove(TDataType item)
        {
            throw new NotImplementedException();
        }

        public override void RemoveByKey(long key, int featureTypeId)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
        }

        #endregion

        #region Overrides of AbstractLinqDataRepository<TDataType>

        public override IEnumerable<TDataType> Where(Func<TDataType, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public override TDataType Poke()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Overrides of AbstractByOffsetDataRepository<TDataType,long>

        public override TDataType Get(long offset, int featureTypeId, Projection projection = null)
        {
            return DataFromEventEntity(EventEntityStorage.GetEntityById((int)offset));
        }

        public override long Add(TDataType item)
        {
            return EventEntityStorage.CreateEntity(EventEntityFromData(item));
        }

        public override IEnumerator GetEnumerator()
        {
            return EventEntityStorage.GetEnumerator();
        }

        #endregion


        private static TDataType DataFromEventEntity(EventEntity eventEntity)
        {
            if (eventEntity?.Data == null) return null;

            return FormatterUtil.ObjectFromBytes<TDataType>(eventEntity.Data);
        }

        private static EventEntity EventEntityFromData(TDataType data)
        {
            var eventEntity = new EventEntity { Data = FormatterUtil.ObjectToBytes(data) };
            //TODO:

            return eventEntity;
        }
    }
}
