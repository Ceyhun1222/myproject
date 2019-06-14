#region

using System;
using System.Collections.Generic;
using Aran.Aim.Data;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.MetaData;

#endregion

namespace Aran.Temporality.Internal.Interface.Storage
{
    internal interface IEventStorage<T> : IDisposable where T : class //T is feature type
    {
        #region Properties

        TemporalityLogicOptions Options { get; set; }
        string StorageName { get; set; }

        #endregion


        IList<int> GetSlotFeatureTypesInternal(int slotId);

        bool IsValidTimeUnique(AbstractEvent<T> myEvent);
        
        void Truncate();
        void Load();
        void SaveEventInternal(AbstractEvent<T> myEvent);

        bool Contains(AbstractEvent<T> myEventParam);
        bool Contains(IFeatureId meta, Interpretation interpretation);
        AbstractEventMetaData GetCommissionEventMeta(IFeatureId meta);

        AbstractEventMetaData GetLastCorrection(AbstractEvent<T> myEvent);

        AbstractEvent<T> GetEventByVersion(IFeatureId featureId, TimeSliceVersion version);
        IList<AbstractEventMetaData> GetActualEventMeta(IFeatureId featureId, TimeSlice impactInterval, TimeSlice submitInterval);
        IList<AbstractEventMetaData> GetCancelledEventMeta(IFeatureId featureId, TimeSlice impactInterval, TimeSlice submitInterval);
        IList<AbstractEventMetaData> GetWorkPackageMeta(int workpackage);


        IList<AbstractEvent<T>> GetEventsAffectingDateInterval(IFeatureId featureId,
                                                               DateTime fromDate,
                                                               DateTime? toDate,
                                                               bool andOneMore = false, 
                                                               Projection projection = null);

        TimeSliceVersion CreateNewVersion(IFeatureId featureId, Interpretation interpretation);

        IList<Guid> GetActualIds(IFeatureId meta, DateTime? dateStart = null, DateTime? dateEnd = null);
        IList<Guid> GetDeletedIds(IFeatureId meta, DateTime? dateStart = null, DateTime? dateEnd = null);


        #region Workpackage operations

        void DeleteWorkPackage(int workpackage);
        bool CommitWorkPackage(int workPackage);
       
        #endregion




        AbstractEvent<T> GetEventFromData(T data, AbstractEventMetaData meta);
    }
}