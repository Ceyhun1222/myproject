#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Aran.Aim;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Extensions;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.Common.MetaData;
using Aran.Temporality.Internal.Abstract.Repository;
using Aran.Temporality.Internal.Implementation.Repository.Linq;
using Aran.Temporality.Internal.MetaData.Offset;
using Aran.Temporality.Internal.MetaData.Util;
using Aran.Temporality.Internal.Util;
using FluentNHibernate.Conventions;
using NHibernate.Linq;
using StackExchange.Redis;

#endregion

namespace Aran.Temporality.Internal.Abstract.Storage.Cached
{
    internal abstract class CachedEventStorage<T, TKeyType> : AbstractEventStorage<T> where T : class
    {


        #region Util

        private IList<AbstractEventMetaData> GetEventMeta(EventMetaSet<OffsetEventMetaData<TKeyType>> metaset, TimeSlice impactInterval, TimeSlice submitInterval)
        {
            var result = new List<AbstractEventMetaData>();

            if (impactInterval == null)
            {
                if (submitInterval == null)
                {
                    result.AddRange(metaset);
                }
                else
                {
                    result.AddRange(submitInterval.EndPosition == null
                                        ? metaset.Where(t => t.SubmitDate > submitInterval.BeginPosition)
                                        : metaset.Where(
                                            t =>
                                            t.SubmitDate > submitInterval.BeginPosition &&
                                            t.SubmitDate < submitInterval.EndPosition));
                }

            }
            else
            {
                if (submitInterval == null)
                {
                    result.AddRange(metaset.GetViewBetween(impactInterval.BeginPosition, impactInterval.EndPosition));
                }
                else
                {
                    var list = metaset.GetViewBetween(impactInterval.BeginPosition, impactInterval.EndPosition);
                    result.AddRange(submitInterval.EndPosition == null
                                        ? list.Where(t => t.SubmitDate > submitInterval.BeginPosition)
                                        : list.Where(
                                            t =>
                                            t.SubmitDate > submitInterval.BeginPosition &&
                                            t.SubmitDate < submitInterval.EndPosition));
                }
            }
            return result;
        }

        #endregion

        #region Logic


        public override IList<AbstractEventMetaData> GetActualEventMeta(IFeatureId featureId, TimeSlice impactInterval, TimeSlice submitInterval)
        {
            return GetEventMeta(MetaCacheActual.GetByFeatureId(featureId), impactInterval, submitInterval);
        }

        public override IList<AbstractEventMetaData> GetWorkPackageMeta(int workpackage)
        {
            var result = new List<AbstractEventMetaData>();
            var currentActualWorkPackageMeta = MetaCacheActual.GetByWorkPackage(workpackage);
            // var currentCancelledWorkPackageMeta = MetaCacheCancelled.GetByWorkPackage(workpackage);

            if (currentActualWorkPackageMeta != null)
                foreach (var pair in currentActualWorkPackageMeta)
                {

                    var featureType = pair.Key;
                    SortedList<Guid, EventMetaSet<OffsetEventMetaData<TKeyType>>> currentActualFeatureTypeData = null;
                    currentActualWorkPackageMeta.TryGetValue(featureType, out currentActualFeatureTypeData);
                    currentActualFeatureTypeData?.ForEach(t => result.AddRange(t.Value));
                }

            //if(currentCancelledWorkPackageMeta != null)
            //    foreach (var pair in currentCancelledWorkPackageMeta)
            //    {

            //        var featureType = pair.Key;
            //        SortedList<Guid, EventMetaSet<OffsetEventMetaData<TMetaKeyType>>> currentCancelledFeatureTypeData = null;
            //        currentCancelledWorkPackageMeta?.TryGetValue(featureType, out currentCancelledFeatureTypeData);
            //        currentCancelledFeatureTypeData?.ForEach(t => result.AddRange(t.Value));
            //    }

            return result;
        }

        public override IList<AbstractEventMetaData> GetCancelledEventMeta(IFeatureId featureId, TimeSlice impactInterval, TimeSlice submitInterval)
        {
            return GetEventMeta(MetaCacheCancelled.GetByFeatureId(featureId), impactInterval, submitInterval);
        }


        private AbstractEventMetaData GetCommissionEventInCurrentWorkPackage(IFeatureId ifeatureId)
        {
            var metaset = MetaCacheActual.GetByFeatureId(ifeatureId);
            var list = metaset.Where(t => t.Version.SequenceNumber == 1 && t.Interpretation == Interpretation.PermanentDelta).ToList();
            if (list.Count == 0) return null;
            return list.First();
        }



        private AbstractEventMetaData GetCommissionEventInDefaultWorkPackage(IFeatureId ifeatureId)
        {
            var metaset = MetaCacheActual.GetByFeatureId(new FeatureId(ifeatureId) { WorkPackage = 0 });
            var list = metaset.Where(t => t.Version.SequenceNumber == 1 && t.Interpretation == Interpretation.PermanentDelta).ToList();
            if (list.Count == 0) return null;
            return list.First();
        }

        private bool ContainsInCurrentWorkPackage(IFeatureId ifeatureId, Interpretation interpretation)
        {
            var metaset = MetaCacheActual.GetByFeatureId(ifeatureId);
            return metaset.Any(t => t.Interpretation == interpretation);
        }

        private bool ContainsInDefaultPackage(IFeatureId ifeatureId, Interpretation interpretation)
        {
            var metaset = MetaCacheActual.GetByFeatureId(new FeatureId(ifeatureId) { WorkPackage = 0 });
            return metaset.Any(t => t.Interpretation == interpretation);
        }

        private AbstractEventMetaData GetLastCorrectionInCurrentWorkPackage(AbstractEvent<T> myEvent)
        {
            var metaset = MetaCacheActual.GetByFeatureId(myEvent);
            var list = metaset.Where(t => t.Version.SequenceNumber == myEvent.Version.SequenceNumber && t.Interpretation == myEvent.Interpretation).ToList();
            if (list.Count == 0) return null;
            var lastCorrectionNumber = list.Max(t => t.Version.CorrectionNumber);
            return list.FirstOrDefault(t => t.Version.CorrectionNumber == lastCorrectionNumber);
        }

        private AbstractEventMetaData GetLastCorrectionInDefaultWorkPackage(AbstractEvent<T> myEvent)
        {
            var metaset = MetaCacheActual.GetByFeatureId(new FeatureId(myEvent) { WorkPackage = 0 });
            var list = metaset.Where(t => t.Version.SequenceNumber == myEvent.Version.SequenceNumber && t.Interpretation == myEvent.Interpretation).ToList();
            if (list.Count == 0) return null;
            var lastCorrectionNumber = list.Max(t => t.Version.CorrectionNumber);
            return list.FirstOrDefault(t => t.Version.CorrectionNumber == lastCorrectionNumber);
        }

        public override AbstractEventMetaData GetLastCorrection(AbstractEvent<T> myEvent)
        {
            var meta = GetLastCorrectionInCurrentWorkPackage(myEvent);
            //if we are already in default workpackage
            if (myEvent.WorkPackage == 0) return meta;
            //if there is any data in current workpackage
            if (meta != null) return meta;
            //return data from default
            return GetLastCorrectionInDefaultWorkPackage(myEvent);
        }

        public override IList<Guid> GetActualIds(IFeatureId meta, DateTime? dateStart = null, DateTime? dateEnd = null)
        {
            var metaList = MetaCacheActual.GetPairsByFeatureTypeName(meta);

            if (dateStart == null) return metaList.Keys;


            var result = new HashSet<Guid>();

            foreach (var item in metaList.Where(t2 => t2.Value.IsActual && t2.Value.Values.Count > 0))
            {
                var start = item.Value.Values.Where(t => t.Value.LifeTimeBegin != null).ToList();
                var end = item.Value.Values.Where(t => t.Value.LifeTimeEnd != null).ToList();
                var minDate = item.Value.Values.Min(t => t.Key);

                var featureStartDate = start.IsEmpty() ? minDate : start.First().Value.LifeTimeBegin;
                DateTime? featureEndDate = end.IsEmpty() ? null : end.First().Value.LifeTimeEnd;

                if (featureEndDate == null)
                {
                    if (dateEnd == null)
                    {
                        //they intersect in infinity
                        result.Add(item.Key);
                    }
                    else if (featureStartDate <= dateEnd)
                    {
                        result.Add(item.Key);
                    }
                }
                else
                {
                    if (dateEnd == null)
                    {
                        if (dateStart <= featureEndDate)
                        {
                            result.Add(item.Key);
                        }
                    }
                    else
                    {
                        if (dateStart <= featureEndDate && featureStartDate <= dateEnd)
                        {
                            result.Add(item.Key);
                        }
                    }
                }
            }

            return new List<Guid>(result);
        }

        public override IList<Guid> GetDeletedIds(IFeatureId meta, DateTime? dateStart = null, DateTime? dateEnd = null)
        {
            //TODO: fix this, change logic according to interval
            var metaList = MetaCacheActual.GetPairsByFeatureTypeName(meta);

            if (dateStart == null) return new List<Guid>();

            var actualDate = (DateTime)dateStart;
            var result = metaList.Where(t => t.Value.HasDecomissioningOn(actualDate)).Select(t => t.Key).ToList();

            return result;
        }

        public override bool Contains(AbstractEvent<T> myEvent)
        {
            var list = MetaCacheActual.GetByFeatureId(myEvent);
            if (list == null) return false;
            return list.ContainsVersion(myEvent.Version, myEvent.Interpretation);
        }

        public override bool Contains(IFeatureId meta, Interpretation interpretation)
        {
            if (!ContainsInCurrentWorkPackage(meta, interpretation))
                return ContainsInDefaultPackage(meta, interpretation);
            return true;
        }

        public override AbstractEventMetaData GetCommissionEventMeta(IFeatureId myEvent)
        {
            var meta = GetCommissionEventInCurrentWorkPackage(myEvent);
            //if we are already in default workpackage
            if (myEvent.WorkPackage == 0) return meta;
            //if there is any data in current workpackage
            if (meta != null) return meta;
            //return data from default
            return GetCommissionEventInDefaultWorkPackage(myEvent);
        }

        public override TimeSliceVersion CreateNewVersion(IFeatureId featureId, Interpretation interpretation)
        {
            var sequenceNumber = 1;

            EventMetaSet<OffsetEventMetaData<TKeyType>> metaset = MetaCacheActual.GetByFeatureId(featureId);
            if (metaset.Any(t => t.Interpretation == interpretation))
            {
                var lastNumber = metaset.GetLastSequenceNumber(interpretation) + 1;
                if (sequenceNumber < lastNumber)
                    sequenceNumber = lastNumber;
            }
            if (featureId.WorkPackage != 0)
            {
                metaset = MetaCacheActual.GetByFeatureId(new FeatureId(featureId) { WorkPackage = 0 });
                if (metaset.Any(t => t.Interpretation == interpretation))
                {
                    var lastNumber = metaset.GetLastSequenceNumber(interpretation) + 1;
                    if (sequenceNumber < lastNumber)
                        sequenceNumber = lastNumber;
                }
            }

            return new TimeSliceVersion { SequenceNumber = sequenceNumber };
        }

        public override bool IsValidTimeUnique(AbstractEvent<T> myEvent)
        {
            var metaset = MetaCacheActual.GetByFeatureId(myEvent);
            return metaset.All(t => t.TimeSlice.BeginPosition != myEvent.TimeSlice.BeginPosition);
        }



        public override void Truncate()
        {
            _repositories.Clear();
        }

        #endregion

        #region Metadata

        protected readonly MetaCache<EventMetaSet<OffsetEventMetaData<TKeyType>>> MetaCacheActual =
            new MetaCache<EventMetaSet<OffsetEventMetaData<TKeyType>>> { IsActual = true };

        protected readonly MetaCache<EventMetaSet<OffsetEventMetaData<TKeyType>>> MetaCacheCancelled =
           new MetaCache<EventMetaSet<OffsetEventMetaData<TKeyType>>> { IsActual = false };

        private readonly SortedList<int, IEnumerable> _repositories =
            new SortedList<int, IEnumerable>();

        protected AbstractOffsetDataRepository<T, TKeyType> Repository(int workpackage)
        {
            if (!_repositories.TryGetValue(workpackage, out var result))
            {
                var marker = "events\\events_" + workpackage;
                result = StorageManager<T>.GetEventRepository(StorageName, marker);
                _repositories[workpackage] = result;
            }

            return (AbstractOffsetDataRepository<T, TKeyType>)result;
        }

        protected void RemoveRepository(int workpackage)
        {
            var metaRepository = Repository(workpackage);
            _repositories.Remove(workpackage);
            metaRepository.RemoveAll();

            //remove from cache
            MetaCacheActual.Remove(workpackage);
            MetaCacheCancelled.Remove(workpackage);

            _loadedSlots.Remove(workpackage);
        }

        private ConnectionMultiplexer _redis;
        private ISubscriber _sub;
        private BinaryFormatter _binaryFormatter = new BinaryFormatter();
        private string _publishChannelName;

        protected CachedEventStorage()
        {
            if (ConfigUtil.RemoteChannelName == "" || ConfigUtil.RepositoryType == RepositoryType.NoDeleteRepository)
                ConfigUtil.UseRedisForMetaCache = false;

            if (ConfigUtil.UseRedisForMetaCache)
            {
                _redis = ConnectionMultiplexer.Connect(ConfigUtil.RedisConnectionString);
                _sub = _redis.GetSubscriber();
            }
        }

        private void RedisCallback(RedisChannel channel, RedisValue message)
        {
            using (var memStream = new MemoryStream())
            {
                memStream.Write(message, 0, ((byte[])message).Length);
                memStream.Seek(0, SeekOrigin.Begin);

                if (_binaryFormatter.Deserialize(memStream) is OffsetEventMetaData<TKeyType> meta)
                {
                    if (_loadedSlots.Contains(meta.WorkPackage))
                    {
                        AddEventMetaData(meta, false);
                    }
                }
            }
        }

#warning actually we should return false in case of error
        //TODO:actually we should return false in case of error
        protected bool AddEventMetaData(OffsetEventMetaData<TKeyType> meta, bool publish = true)
        {
            if (ConfigUtil.UseRedisForMetaCache && publish)
            {
                using (var ms = new MemoryStream())
                {
                    _binaryFormatter.Serialize(ms, meta);
                    _sub.Publish(_publishChannelName, ms.ToArray());
                    _logger.Debug($"Metadata has been sent to Redis channel: {_publishChannelName}");
                }
            }

            // Temporary fix. Must be deleted in future.
            if (meta.LifeTimeBegin == null)
                meta.LifeTimeBegin = meta.TimeSlice.BeginPosition;

            // Temporary fix. Must be deleted in future.
            if (meta.LifeTimeEndSet && meta.TimeSlice.EndPosition == null)
                meta.TimeSlice.EndPosition = meta.LifeTimeEnd;
            // Temporary fix. Must be deleted in future. Some data was written with tempdelta instead of permdelta
            if (meta.Interpretation == Interpretation.TempDelta && meta.TimeSlice.EndPosition == null && !meta.IsCanceled)
                meta.Interpretation = Interpretation.PermanentDelta;

            // Temporary fix. Must be deleted in future.
            if (meta.Interpretation == Interpretation.BaseLine)
                meta.Interpretation = Interpretation.PermanentDelta;

            _logger.Trace($"Placing meta {meta.Guid} type: {(FeatureType)meta.FeatureTypeId} version:{meta.Version}  in cache.");
            _logger.Trace(meta.Dump);
            if (MetaCacheActual.GetByFeatureId(meta)
                .Any(t => t.Version.SequenceNumber == meta.Version.SequenceNumber && t.Interpretation == meta.Interpretation))
            {
                _logger
                    .Trace($"Meta {meta.Guid} with sequence {meta.Version.SequenceNumber} is contained in cache.");
                var deleted = MetaCacheActual.GetByFeatureId(meta).DeleteEventBySequence(meta);
                if (deleted != null)
                {
                    _logger
                        .Trace($"Meta {meta.Guid} {meta.Version} is the newest.");
                    //if it was deleted, place it in cancelled
                    MetaCacheCancelled.GetByFeatureId(deleted).Add(deleted);
                    _logger
                        .Trace($"Meta {deleted.Guid} {deleted.Version} {meta.Version.SequenceNumber} has been placed in non-actual cache.");
                    MetaCacheActual.GetByFeatureId(meta).Add(meta);
                    _logger
                        .Trace($"Meta {meta.Guid} {meta.Version} has been placed in actual cache.");

                }
                else
                {
                    _logger
                        .Trace($"Meta {meta.Guid} {meta.Version} is not the newest.");
                    MetaCacheCancelled.GetByFeatureId(meta).Add(meta);
                    _logger
                        .Trace($"Meta {meta.Guid} {meta.Version} has been placed in non-actual cache.");
                }
            }
            else
            {
                _logger
                    .Trace($"Meta {meta.Guid} with sequence {meta.Version.SequenceNumber} is not contained in cache.");
                MetaCacheActual.GetByFeatureId(meta).Add(meta);
                _logger
                        .Trace($"Meta {meta.Guid} {meta.Version} has been placed in actual cache.");
            }

            return true;
        }

        public override void Load()
        {
            if (ConfigUtil.UseRedisForMetaCache)
            {
                var subChannelName = "Channel_" + ConfigUtil.OwnChannelName + "_" + StorageName + "_" + typeof(TKeyType).Name;
                _publishChannelName = "Channel_" + ConfigUtil.RemoteChannelName + "_" + StorageName + "_" + typeof(TKeyType).Name;
                _sub.UnsubscribeAll();
                _sub.Subscribe(subChannelName, RedisCallback);
                _logger.Info($"Has been subscribed to Redis channel: {subChannelName}");
            }

            //load default wp
            LoadMeta(0);
        }

        private readonly HashSet<int> _loadedSlots = new HashSet<int>();
        private readonly ILogger _logger = LogManager.GetLogger(typeof(CachedEventStorage<T, TKeyType>));

        public override void LoadMeta(int wp)
        {
            if (_loadedSlots.Contains(wp)) return;

            _logger.Info($"Loading workpackage {wp}");
            Stopwatch watch = Stopwatch.StartNew();
            var count = 0;
            foreach (var data in Repository(wp).GetMetaDatas())
            {
                AddEventMetaData((OffsetEventMetaData<TKeyType>)data, false);
                count++;
            }
            watch.Stop();
            _logger.Info($"Workpackage {wp} has been loaded. Loaded {count} metas.");
            _logger.Debug($"Loading time: { watch.ElapsedMilliseconds} ms or {watch.ElapsedTicks} ticks.");

            _loadedSlots.Add(wp);

        }

        #endregion


        public override IList<int> GetSlotFeatureTypesInternal(int slotId)
        {
            LoadMeta(slotId);

            var byWp = MetaCacheActual.GetByWorkPackage(slotId);
            if (byWp == null || byWp.Count == 0) return new List<int>();

            var actualChangesMeta = byWp.Where(t => t.Value.Values.Count > 0).ToList();
            if (actualChangesMeta.Count == 0) return new List<int>();

            var actualChangesMetaKeys = actualChangesMeta.Select(t => t.Key).ToList();
            return actualChangesMetaKeys;
        }

        public override int GetFeatureTypeById(Guid id)
        {
            return MetaCacheActual.GetFeatureTypeById(id);
        }


    }
}