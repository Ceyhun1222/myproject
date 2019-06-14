using System;
using System.Collections.Generic;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Geometries;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Abstract.State;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.MetaData;
using Aran.Temporality.Common.OperationResult;
using Aran.Temporality.Common.Util;
using Aran.Temporality.Internal.WorkFlow;

namespace Aran.Temporality.Common.Interface
{
    public interface ITemporalityService<T> : IDisposable where T : class //T is feature type
    {

        OperationStatus GetCurrentOperationStatus();

        #region Properties

        TemporalityLogicOptions Options { get; set; }
        string StorageName { get; set; }

        #endregion

        #region Event operations

        //cancel
        CommonOperationResult CancelSequence(TimeSliceId myEvent, Interpretation interpretation, DateTime? cancelDate = null);

        //commit
        //CommonOperationResult CommitEvent(AbstractEvent<T> myEvent);

        FeatureOperationResult CommitNewEvent(AbstractEvent<T> myEvent);

        //correct
        FeatureOperationResult CommitCorrection(AbstractEvent<T> myEvent);

        //read
        AbstractEvent<T> GetEvent(IFeatureId featureId, TimeSliceVersion version);


        IList<AbstractEventMetaData> GetActualEventMeta(IFeatureId featureId, TimeSlice impactInterval, TimeSlice submitInterval);
        IList<AbstractEventMetaData> GetCancelledEventMeta(IFeatureId featureId, TimeSlice impactInterval, TimeSlice submitInterval);

        #endregion

        #region State operations


        IList<AbstractState<T>> GetActualDataByDate(IFeatureId featureId, bool slotOnly, DateTime dateTime, Interpretation interpretation = Interpretation.Snapshot, DateTime? endDate = null, Filter filter = null, Projection projection = null);
        IList<AbstractEvent<T>> GetEventsByDate(IFeatureId featureId, bool slotOnly, DateTime fromDateTime, DateTime toDateTime, Interpretation? interpretation = null, DateTime? currentDate = null);
        IList<AbstractState<T>> GetStatesInRange(IFeatureId featureId, bool slotOnly, DateTime dateTimeStart, DateTime dateTimeEnd);
        IList<AbstractState<T>> GetStatesInRangeByInterpretation(IFeatureId featureId, bool slotOnly, DateTime dateTimeStart, DateTime dateTimeEnd, Interpretation interpretation);

        bool Optimize();

        #endregion

        #region Version operation

        TimeSliceVersion CreateNewVersion(IFeatureId featureId, Interpretation interpretation);

        #endregion

        #region Utils and global methods

        DateTime GetServerTime();
        SystemType GetServerType();
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

        #region Slot

        IList<int> GetPrivateSlotFeatureTypes(int slotId);

        void PublishPublicSlot(PublicSlot publicSlot);
        bool DeletePrivateSlot(int id);


        #endregion


        StateWithDelta<T> GetActualDataForEditing(IFeatureId featureId, DateTime actualDate, Interpretation interpretation = Interpretation.Snapshot, DateTime? endDate = null);
        IList<AbstractEvent<T>> GetEvolution(IFeatureId featureId);

        void GetConfigurationData(int id);

        #region Changes

        IList<AbstractEvent<T>> GetChangesInInterval(IFeatureId featureId, DateTime startDate, DateTime endDate, bool slotOnly);
        IList<AbstractEventMetaData> GetChangesMetaInInterval(IFeatureId featureIdParam, DateTime startDateParam,
                                                               DateTime endDateParam, bool slotOnlyParam);

        #endregion

        List<string> GetUsersOfPrivateSlot(int id);
        bool Decommission(IFeatureId featureId, DateTime airacDate);
        IList<AbstractState<T>> GetActualDataByGeo(IFeatureId mask, bool b, DateTime effectiveDate, Geometry geo, double distance);

        int GetFeatureTypeById(Guid id);
    }
}