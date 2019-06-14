#region

using System;
using System.Collections.Generic;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.MetaData;
using Aran.Temporality.Internal.Interface.Storage;

#endregion

namespace Aran.Temporality.Internal.Abstract.Storage
{
    internal abstract class AbstractEventStorage<T> : IEventStorage<T> where T : class //T is feature type
    {
        public AbstractTemporalityService<T> TemporalityService { get; set; }

        #region Abstract Implementation of IEventStorage<T>

        public abstract void Truncate();
        public abstract AbstractEventMetaData GetLastCorrection(AbstractEvent<T> myEvent);
        public abstract void SaveEventInternal(AbstractEvent<T> myEvent);
        public abstract IList<Guid> GetActualIds(IFeatureId meta, DateTime? dateStart = null, DateTime? dateEnd = null);

        public abstract IList<Guid> GetDeletedIds(IFeatureId meta, DateTime? dateStart = null, DateTime? dateEnd = null);
        public abstract IList<Guid> GetFilteredIds(IFeatureId featureId, Filter filter, bool slotOnly, IList<Guid> ids = null);
        public abstract bool CheckFilterRuntime(T abstractStateData, Filter filter);

        public abstract void DeleteWorkPackage(int workpackage);
        public abstract bool CommitWorkPackage(int workPackage);
        public abstract AbstractEvent<T> GetEventFromData(T data, AbstractEventMetaData meta);
        public abstract IList<AbstractEventMetaData> GetCancelledEventMeta(IFeatureId featureId, TimeSlice impactInterval, TimeSlice submitInterval);
        public abstract IList<AbstractEventMetaData> GetActualEventMeta(IFeatureId featureId, TimeSlice impactInterval, TimeSlice submitInterval);
        public abstract IList<AbstractEventMetaData> GetWorkPackageMeta(int workpackage);
        public abstract AbstractEvent<T> GetEventByVersion(IFeatureId featureId, TimeSliceVersion version);
        public abstract IList<AbstractEvent<T>> GetEventsAffectingDateInterval(IFeatureId featureId, DateTime fromDate, DateTime? toDate, bool andOneMore=false, Projection projection = null);
        public abstract TimeSliceVersion CreateNewVersion(IFeatureId featureId, Interpretation interpretation);
        public abstract bool Contains(AbstractEvent<T> myEventParam);
        public abstract bool Contains(IFeatureId meta, Interpretation interpretation);
        public abstract AbstractEventMetaData GetCommissionEventMeta(IFeatureId meta);
        public abstract bool IsValidTimeUnique(AbstractEvent<T> myEvent);

        public abstract IList<int> GetSlotFeatureTypesInternal(int slotId);
        #endregion

        #region Implementation of IEventStorage<T>

        private string _storageName;
        public abstract void Load();
        public abstract void LoadMeta(int wp);

        public string StorageName
        {
            get { return _storageName; }
            set
            {
                _storageName = value;
                Load();
            }
        }

        public TemporalityLogicOptions Options { get; set; }

        #endregion

        #region Implementation of IDisposable

        public abstract void Dispose();

        #endregion

        public abstract IList<AbstractEvent<T>> GetChangesInIntervalInternal(IFeatureId featureId, DateTime startDate,
                                                                             DateTime endDate);

        public abstract IList<AbstractEventMetaData> GetChangesMetaInIntervalInternal(IFeatureId featureId,
                                                                                      DateTime startDate,
                                                                                      DateTime endDate);


        public abstract bool CanCommitWorkPackage(List<int> workPackageIds);

        public abstract int GetFeatureTypeById(Guid id);
    }
}