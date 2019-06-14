using System;
using System.Collections.Generic;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Abstract.State;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.MetaData;
using Aran.Temporality.Common.OperationResult;

namespace Aran.Temporality.Common.Interface
{
    public interface IStorage<T> : IDisposable where T : class //T is feature type
    {
        #region Properties

        TemporalityLogicOptions Options { get; set; }
        string Path { get; set; }

        #endregion

        #region Event operations

        //cancel
        CommonOperationResult CancelEvent(TimeSliceId myEvent, DateTime? cancelDate = null);

        //commit
        CommonOperationResult CommitEvent(AbstractEvent<T> myEvent);

        FeatureOperationResult CommitNewEvent(AbstractEvent<T> myEvent);

        //correct
        FeatureOperationResult CommitCorrection(AbstractEvent<T> myEvent);

        //read
        AbstractEvent<T> GetEvent(IFeatureId featureId, TimeSliceVersion version);

        IList<AbstractEventMetaData> GetActualEventMeta(IFeatureId featureId, TimeSlice impactInterval, TimeSlice submitInterval);
        IList<AbstractEventMetaData> GetCancelledEventMeta(IFeatureId featureId, TimeSlice impactInterval, TimeSlice submitInterval);
        
        #endregion

        #region State operations

        AbstractState<T> GetSnapshotByVersion(IFeatureId featureId, TimeSliceVersion version, bool ignoreTemporal = false);
        AbstractState<T> GetSnapshotByActualDate(IFeatureId featureId, DateTime dateTime, bool ignoreTemporal = false);
        IList<AbstractState<T>> GetSnapshotByFeatureName(IFeatureId featureId, DateTime dateTime,
                                                         bool ignoreTemporal = false);

        AbstractState<T> GetBaselineByVersion(IFeatureId featureId, TimeSliceVersion version, bool ignoreTemporal = false);
        AbstractState<T> GetBaselineByActualDate(IFeatureId featureId, DateTime dateTime, bool ignoreTemporal = false);
        IList<AbstractState<T>> GetBaselineByFeatureName(IFeatureId featureId, DateTime dateTime,
                                                         bool ignoreTemporal = false);

        bool Optimize();

        #endregion

        #region Version operation


        TimeSliceVersion CreateNewVersion(IFeatureId featureId);

        #endregion

        #region WorkPackage operation

        Guid CreateNewWorkPackage(string name, bool isSafe);

        #endregion

        #region Utils and global methods

        DateTime GetServerTime();

        void Truncate();

        #endregion

        //---
        #region Query operation

        //IList<Type> GetSupportedQueryTypes();

        //QueryResult<AbstractEvent<T>> QueryEvent(AbstractQuery query, bool ignoreTemporal = false);
        //QueryResult<TimeSliceId> QueryEventId(AbstractQuery query, bool ignoreTemporal = false);

        //QueryResult<AbstractState<T>> QueryState(AbstractQuery query, DateTime actualDate, bool ignoreTemporal = false);

        #endregion
        //---
        #region Dependency

        //IList<AbstractEvent<T>> GetRelatedEvents(IList<TimeSliceId> featureIds);
        //IList<TimeSliceId> GetRelatedFeatures(IList<TimeSliceId> featureIds);

        #endregion
        //---

        //---

        //---

        //---

        //---
    }
}