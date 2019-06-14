#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Abstract.State;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.MetaData;
using Aran.Temporality.Common.OperationResult;
using Aran.Temporality.Internal.Attribute;
using Aran.Temporality.Internal.Implementation.Repository.NHibernate.Entity;
using Aran.Temporality.Internal.Interface.Storage;
using Aran.Temporality.Internal.Interface.Util;
using Aran.Temporality.Internal.MetaData;
using Aran.Temporality.Internal.Util.Lock;

#endregion

namespace Aran.Temporality.Internal.Abstract.Storage
{
    internal abstract class AbstractStorage<T> : IStorage<T> where T : class //T is feature type
    {
        #region Constructor, Destructor, options and handlers

        private Thread _optimizerThread;
        private TemporalityLogicOptions _options;

        private string _path;

        private AbstractStorage()
        {
            Options = new TemporalityLogicOptions();
        }

        protected AbstractStorage(string path) : this()
        {
            Path = path;
            InitOptimizer();
        }

        public TemporalityLogicOptions Options
        {
            get { return _options; }
            set
            {
                _options = value;
                //share options
                if (AbstractEventStorage != null) AbstractEventStorage.Options = Options;
                if (AbstractStateStorage != null) AbstractStateStorage.Options = Options;
            }
        }

        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;

                
                //share Path
                if (AbstractEventStorage != null) AbstractEventStorage.Path = _path;
                if (AbstractStateStorage != null) AbstractStateStorage.Path = _path;
            }
        }

        private void InitOptimizer()
        {
            _optimizerThread = new Thread(() =>
                                              {
                                                  while (!_disposingNow)
                                                  {
                                                      if (!Optimize()) Thread.Sleep(ConfigUtil.OptimizerSleepTime);
                                                  }
                                                  //Console.WriteLine("optimizer exited");
                                              });

            _optimizerThread.Start();
        }

        ~AbstractStorage()
        {
            Dispose(false);
        }

        #endregion

        #region Implementation of IDisposable

        // Track whether Dispose has been called.
        private bool _disposed;
        private bool _disposingNow;

        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose(bool disposing)
        {
            _disposingNow = true;
            // Check to see if Dispose has already been called.
            if (!_disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    AbstractEventStorage.Dispose();
                    AbstractStateStorage.Dispose();
                }

                if (_optimizerThread != null)
                {
                    _optimizerThread.Join();
                    _optimizerThread = null;
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.

                // Disposing has been done.
                _disposed = true;
            }
        }

        #endregion

        #region Storage properties

        internal abstract AbstractEventStorage<T> AbstractEventStorage { get; }
        internal abstract AbstractStateStorage<T> AbstractStateStorage { get; }

        #endregion

        #region Lock util

        private readonly ILockUtil _lockUtil = new LockUtil();

        #endregion

        #region Util methods 

        public DateTime GetServerTime()
        {
            return DateTime.Now;
        }

        #endregion

        #region Private methods

        private List<AbstractEvent<T>> SortEvents(IEnumerable<AbstractEvent<T>> events)
        {
            if (Options.SortingEventBy == TemporalityLogicOptions.SortingEventField.PublicDate)
            {
                return events.OrderBy(t => t.SubmitDate).ThenBy(t => t.TimeSlice.StartDate).ToList();
            }
            if (Options.SortingEventBy == TemporalityLogicOptions.SortingEventField.ValidStartDate)
            {
                return events.OrderBy(t => t.TimeSlice.StartDate).ThenBy(t => t.SubmitDate).ToList();
            }

            throw new Exception("wrong option");
        }

        private CommonOperationResult CommitEventInternal(AbstractEvent<T> myEvent)
        {
            var result = new CommonOperationResult();
            try
            {
                //check version
                if (AbstractEventStorage.Contains(myEvent))
                {
                    throw new Exception("Event with same id and version already exists");
                }

                if (!AbstractEventStorage.IsValidTimeUnique(myEvent))
                {
                    throw new Exception("Event with same start of valid time already exists");
                }

                TimeSliceVersion newVersion = CreateNewVersion(myEvent);
                if (myEvent.Version.SequenceNumber > newVersion.SequenceNumber)
                {
                    throw new Exception("Wrong sequence number, should be not greater than " + newVersion.SequenceNumber);
                }

                //save event
                AbstractEventStorage.SaveEventInternal(myEvent);

                //clear all states affected by event
                AbstractStateStorage.ClearStatesAfter(myEvent);
            }
            catch (Exception exception)
            {
                result.ReportError(exception.Message);
            }

            return result;
        }

        private void SetUser(AbstractEvent<T> myEventParam)
        {
            string user = null;
            if (OperationContext.Current != null &&
                OperationContext.Current.ServiceSecurityContext != null &&
                OperationContext.Current.ServiceSecurityContext.PrimaryIdentity != null)
            {
                user = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
            }
            myEventParam.User = user;
        }

        private CommonOperationResult CancelEventInternal(TimeSliceId myEvent, DateTime? cancelDate = null)
        {
            var result = new CommonOperationResult();
            try
            {
                AbstractEventStorage.CancelEvent(myEvent, cancelDate);
            }
            catch (Exception exception)
            {
                result.ReportError(exception.Message);
            }

            return result;
        }

        #endregion

        #region Important data manipulation methods (using Lock)

        [SecureOperation(PackageOperation.CreatePackage)]
        public Guid CreateNewWorkPackage(string name, bool isSafe)
        {
            return AbstractEventStorage.CreateNewWorkPackage(name, isSafe);
        }

        [SecureOperation(DataOperation.WriteData)]
        public CommonOperationResult CancelEvent(TimeSliceId myEvent, DateTime? cancelDate = null)
        {
            return _lockUtil.ManipulateWithData(CancelEventInternal, myEvent, cancelDate);
        }


        [SecureOperation(DataOperation.ReadData)]
        public TimeSliceVersion CreateNewVersion(IFeatureId featureIdParam)
        {
            return _lockUtil.ManipulateWithData(featureId => AbstractEventStorage.CreateNewVersion(featureId),
                                                featureIdParam);
        }


        [SecureOperation(StorageOperation.TruncateStorage)]
        public void Truncate()
        {
            _lockUtil.ManipulateWithData(() =>
                                             {
                                                 AbstractEventStorage.Truncate();
                                                 AbstractStateStorage.Truncate();
                                             });
        }


        [SecureOperation(DataOperation.ReadData)]
        public AbstractState<T> GetBaselineByVersion(IFeatureId featureId, TimeSliceVersion version, bool ignoreTemporal = false)
        {
            //TODO: fix
            #warning fix
            return GetSnapshotByVersion(featureId,version,ignoreTemporal);
        }

        [SecureOperation(DataOperation.ReadData)]
        public AbstractState<T> GetBaselineByActualDate(IFeatureId featureId, DateTime dateTime, bool ignoreTemporal = false)
        {
            //TODO: fix
            #warning fix
            return GetSnapshotByActualDate(featureId, dateTime, ignoreTemporal);
        }

        [SecureOperation(DataOperation.ReadData)]
        public IList<AbstractState<T>> GetBaselineByFeatureName(IFeatureId featureId, DateTime dateTime, bool ignoreTemporal = false)
        {
            //TODO: fix
            return GetSnapshotByFeatureName(featureId, dateTime, ignoreTemporal);
        }

        //enabled for everyone
        public bool Optimize()
        {
            return _lockUtil.ManipulateWithData<bool>(AbstractStateStorage.Optimize, 1);
        }

        [SecureOperation(DataOperation.ReadData)]
        public AbstractEvent<T> GetEvent(IFeatureId featureIdParam, TimeSliceVersion versionParam)
        {
            return
                _lockUtil.ManipulateWithData(
                    (featureId, version) => AbstractEventStorage.GetEventByVersion(featureId, version),
                    featureIdParam, versionParam);
        }

        [SecureOperation(DataOperation.ReadData)]
        public IList<AbstractEventMetaData> GetActualEventMeta(IFeatureId featureIdParam, TimeSlice impactIntervalParam, TimeSlice submitIntervalParam)
        {
            return _lockUtil.ManipulateWithData(
            (featureId, impactInterval, submitInterval) =>
                AbstractEventStorage.GetActualEventMeta(featureId, impactInterval, submitInterval),
                featureIdParam, impactIntervalParam, submitIntervalParam);
        }

        [SecureOperation(DataOperation.ReadData)]
        public IList<AbstractEventMetaData> GetCancelledEventMeta(IFeatureId featureIdParam, TimeSlice impactIntervalParam, TimeSlice submitIntervalParam)
        {
            return _lockUtil.ManipulateWithData(
            (featureId, impactInterval, submitInterval) =>
                AbstractEventStorage.GetCancelledEventMeta(featureId, impactInterval, submitInterval),
                featureIdParam, impactIntervalParam, submitIntervalParam);
        }

        [SecureOperation(DataOperation.WriteData)]
        public CommonOperationResult CommitEvent(AbstractEvent<T> myEventParam)
        {
            SetUser(myEventParam);

            //check all parameter set
            if (myEventParam.Version.CorrectionNumber != 0)
            {
                return
                    new CommonOperationResult().ReportError(
                        "Correction number should be 0 for this method, try CommitCorrection");
            }
            if (myEventParam.Interpretation == Interpretation.BaseLine)
            {
                return new CommonOperationResult().ReportError("Interpretation.BaseLine is not supported");
            }
            if (myEventParam.TimeSlice == null)
            {
                return new CommonOperationResult().ReportError("Timeslice should not be null");
            }
            if (myEventParam.Interpretation == Interpretation.TempDelta && myEventParam.TimeSlice.EndDate == null)
            {
                return new CommonOperationResult().ReportError("EndDate should not be null for Interpretation.TempDelta");
            }
            if (myEventParam.Interpretation == Interpretation.PermanentDelta && myEventParam.TimeSlice.EndDate != null)
            {
                return
                    new CommonOperationResult().ReportError("EndDate should be null for Interpretation.PermanentDelta");
            }
            if (myEventParam.TimeSlice.EndDate != null &&
                myEventParam.TimeSlice.EndDate < myEventParam.TimeSlice.StartDate)
            {
                return new CommonOperationResult().ReportError("EndDate should be greater or equal than StartDate");
            }

            return _lockUtil.ManipulateWithData(CommitEventInternal, myEventParam);
        }

        [SecureOperation(DataOperation.WriteData)]
        public FeatureOperationResult CommitNewEvent(AbstractEvent<T> myEventParam)
        {
            return _lockUtil.ManipulateWithData(myEvent =>
                                                    {
                                                        myEvent.Version = CreateNewVersion(myEvent);
                                                        var result = new FeatureOperationResult();
                                                        result.Add(CommitEvent(myEvent));
                                                        result.TimeSliceVersion = new TimeSliceVersion(myEvent.Version);
                                                        return result;
                                                    },
                                                myEventParam);
        }

        [SecureOperation(DataOperation.WriteData)]
        public FeatureOperationResult CommitCorrection(AbstractEvent<T> myEventParam)
        {
            SetUser(myEventParam);

            return _lockUtil.ManipulateWithData(myEvent =>
                {
                    var result = new FeatureOperationResult();

                    int oldEventCorrectionNumber =
                        AbstractEventStorage.GetLastCorrectionNumber(myEvent);
                    if (oldEventCorrectionNumber == -1)
                    {
                        result.ReportError(
                            "Previous version of event was not found while performing correction");
                    }
                    else
                    {
                        //form correct new version
                        var correctNewVersion =
                            new TimeSliceVersion(
                                myEventParam.Version.SequenceNumber,
                                oldEventCorrectionNumber);
                        correctNewVersion.CorrectionNumber++;

                        //check versions
                        if (myEvent.Version != null)
                        {
                            if (myEvent.Version != correctNewVersion)
                            {
                                result.ReportError("Version of event is wrong. " +
                                                    "Expected: " + correctNewVersion +
                                                    ", actual: " + myEvent.Version);
                            }
                        }

                        result.TimeSliceVersion =
                            new TimeSliceVersion(correctNewVersion);
                        if (result.IsOk) myEvent.Version = correctNewVersion;
                        if (result.IsOk)
                            result.Add(
                                CancelEvent(
                                    new TimeSliceId
                                        {
                                            FeatureTypeId =
                                                myEventParam.FeatureTypeId,
                                            Guid = myEventParam.Guid,
                                            WorkPackage = myEventParam.WorkPackage,
                                            Version =
                                                new TimeSliceVersion(
                                                myEventParam.Version.SequenceNumber,
                                                oldEventCorrectionNumber)
                                        }));

                        if (result.IsOk) result.Add(CommitEventInternal(myEvent));
                    }

                    return result;
                },
            myEventParam);
        }

        [SecureOperation(DataOperation.ReadData)]
        public AbstractState<T> GetSnapshotByVersion(IFeatureId featureIdParam, TimeSliceVersion versionParam,
                                                     bool ignoreTemporalParam)
        {
            return _lockUtil.ManipulateWithData((featureId, version, ignoreTemporal) =>
                                                    {
                                                        AbstractEvent<T> myEvent = GetEvent(featureId, version);
                                                        return myEvent == null
                                                                   ? null
                                                                   : GetSnapshotByActualDate(featureId,
                                                                                             myEvent.TimeSlice.StartDate,
                                                                                             ignoreTemporal);
                                                    },
                                                featureIdParam,
                                                versionParam,
                                                ignoreTemporalParam);
        }

        [SecureOperation(DataOperation.ReadData)]
        public IList<AbstractState<T>> GetSnapshotByFeatureName(IFeatureId featureIdParam, DateTime dateTimeParam,
                                                                bool ignoreTemporalParam = false)
        {
            return _lockUtil.ManipulateWithData((featureId, dateTime, ignoreTemporal) =>
                                                    {
                                                        var stopwatch = new Stopwatch();
                                                        stopwatch.Start();
                                                        //get last state
                                                        var lastStates =
                                                            AbstractStateStorage.GetLastKnownStatesByFeatureName(
                                                                featureId, dateTime, true);
                                                        stopwatch.Stop();
                                                        //Console.WriteLine("last states in " + stopwatch.ElapsedMilliseconds);
                                                        stopwatch.Restart();


                                                        var lastStateIds = new List<Guid>();
                                                        foreach (AbstractState<T> t in lastStates)
                                                        {
                                                            if (t.Guid != null)
                                                                lastStateIds.Add((Guid) t.Guid);
                                                        }

                                                        List<Guid> missingIds =
                                                            AbstractEventStorage.GetIds(featureId, dateTimeParam).Except
                                                                (lastStateIds).ToList();

                                                        foreach (var id in missingIds)
                                                        {
                                                            var meta = new StateMetaData(featureId) {Guid = id};
                                                            lastStates.Add(AbstractStateStorage.GetByDefaultData(meta));
                                                        }
                                                        stopwatch.Stop();
                                                        //Console.WriteLine("created new default states in " + stopwatch.ElapsedMilliseconds);
                                                        stopwatch.Restart();


                                                        List<AbstractState<T>> clones = lastStates.ToList();

                                                        //int total = 0;
                                                        foreach (var lastState in clones)
                                                        {
                                                            //check if it is out-of-date
                                                            if (lastState.TimeSlice.StartDate < dateTime)
                                                            {
                                                                List<AbstractEvent<T>> events =
                                                                    AbstractEventStorage.GetEventsAffectingDateInterval(
                                                                        lastState, lastState.TimeSlice.StartDate,
                                                                        dateTime).ToList();

                                                                //get only actual
                                                                events.RemoveAll(t => t.IsCanceled);

                                                                if (events.Count > 1) events = SortEvents(events);

                                                               
                                                                //apply events if any
                                                                if (events.Count > 0)
                                                                {
                                                                    //total += events.Count;
                                                                    //Console.WriteLine(events.Count + " events found");

                                                                    foreach (var currentEvent in events)
                                                                    {
                                                                        if (
                                                                            !(ignoreTemporal &&
                                                                              currentEvent.Interpretation ==
                                                                              Interpretation.TempDelta))
                                                                        {
                                                                            currentEvent.ApplyToData(lastState.Data,
                                                                                                     dateTime);
                                                                            lastState.Version =
                                                                                new TimeSliceVersion(
                                                                                    currentEvent.Version);
                                                                        }


                                                                        lastState.TimeSlice.StartDate =
                                                                            currentEvent.TimeSlice.StartDate;
                                                                        //Console.WriteLine("saving with " + lastState.TimeSlice.StartDate);
                                                                        //var clone = CloneUtil.DeepClone(lastState);
                                                                        AbstractStateStorage.SaveStateInternalLater(
                                                                            lastState);
                                                                    }

                                                                    //update actual date
                                                                    lastState.TimeSlice.StartDate = dateTime;
                                                                }
                                                            }
                                                        }


                                                        stopwatch.Stop();
                                                        //if (total>0)
                                                        //Console.WriteLine("applied " + total + " events in " + stopwatch.ElapsedMilliseconds);

                                                        //remove non actual
                                                        var zero = new TimeSliceVersion(0, 0);
                                                        clones.RemoveAll(t => t.Version == zero);

                                                        return clones;
                                                    },
                                                featureIdParam,
                                                dateTimeParam,
                                                ignoreTemporalParam);
        }


        [SecureOperation(DataOperation.ReadData)]
        public AbstractState<T> GetSnapshotByActualDate(IFeatureId featureIdParam, DateTime dateTimeParam,
                                                        bool ignoreTemporalParam)
        {
            return _lockUtil.ManipulateWithData((featureId, dateTime, ignoreTemporal) =>
                                                    {
                                                        var stopwatch = new Stopwatch();
                                                        stopwatch.Start();
                                                        //get last state
                                                        var lastState = AbstractStateStorage.GetLastKnownState(
                                                            featureId, dateTime, true);
                                                        stopwatch.Stop();
                                                        //Console.WriteLine("last state in " +stopwatch.ElapsedMilliseconds);
                                                        stopwatch.Restart();

                                                        if (lastState == null)
                                                        {
                                                            lastState =
                                                                AbstractStateStorage.GetByDefaultData(
                                                                    new StateMetaData(featureId));
                                                        }
                                                        //make sure we will not harm stored object by futher manipulations
                                                        //var lastStateClone = CloneUtil.DeepClone(lastState);
                                                        AbstractState<T> lastStateClone = lastState;

                                                        //int total = 0;

                                                        //check if it is out-of-date
                                                        if (lastState.TimeSlice.StartDate < dateTime)
                                                        {
                                                            var events =
                                                                AbstractEventStorage.GetEventsAffectingDateInterval(
                                                                    featureId, lastState.TimeSlice.StartDate, dateTime).
                                                                    ToList();
                                                            //get only actual
                                                            events.RemoveAll(t => t.IsCanceled);

                                                            if (events.Count > 1) events = SortEvents(events);

                                                            //apply events if any
                                                            if (events.Count > 0)
                                                            {
                                                                //total = events.Count;

                                                                foreach (var currentEvent in events)
                                                                {
                                                                    if (
                                                                        !(ignoreTemporal &&
                                                                          currentEvent.Interpretation ==
                                                                          Interpretation.TempDelta))
                                                                    {
                                                                        currentEvent.ApplyToData(lastStateClone.Data,
                                                                                                 dateTime);
                                                                        lastStateClone.Version =
                                                                            new TimeSliceVersion(currentEvent.Version);


                                                                        lastStateClone.TimeSlice.StartDate =
                                                                            currentEvent.TimeSlice.StartDate;
                                                                        //Console.WriteLine("saving with " + lastState.TimeSlice.StartDate);
                                                                        //var clone = CloneUtil.DeepClone(lastStateClone);

                                                                        AbstractStateStorage.SaveStateInternalLater(
                                                                            lastStateClone);
                                                                        
                                                                    }
                                                                }

                                                                //update actual date
                                                                lastStateClone.TimeSlice.StartDate = dateTime;
                                                            }
                                                        }

                                                        stopwatch.Stop();
                                                        //Console.WriteLine("applied " + total + " events in " + stopwatch.ElapsedMilliseconds);

                                                        if (lastStateClone.Version == new TimeSliceVersion(0, 0))
                                                        {
                                                            return null;
                                                        }

                                                        return lastStateClone;
                                                    },
                                                featureIdParam,
                                                dateTimeParam,
                                                ignoreTemporalParam);
        }

       
       

        #endregion

        #region Query 

        //public QueryResult<AbstractEvent<T>> QueryEvent(AbstractQuery query, bool ignoreTemporal = false)
        //{
        //    var result = new QueryResult<AbstractEvent<T>>();

        //    //process LogicalQuery
        //    if (query is LogicalQuery)
        //    {
        //        var logicalQuery = query as LogicalQuery;
        //        foreach (AbstractQuery q in logicalQuery.Queries)
        //        {
        //            QueryResult<AbstractEvent<T>> subResult = QueryEvent(q, ignoreTemporal);
        //            result.Add(subResult);
        //            if (!result.IsOk) return result;

        //            result.Combine(subResult, logicalQuery.LogicalCondition);

        //            //no need to perform all queries when no results are found in AND logic
        //            if (logicalQuery.LogicalCondition == LogicalCondition.And && result.Count() == 0) break;
        //        }
        //        return result;
        //    }

        //    //check support
        //    IList<Type> supportedTypes = GetSupportedQueryTypes();
        //    if (supportedTypes != null && !supportedTypes.Contains(query.GetType()))
        //    {
        //        result.ReportError("Query type " + query.GetType().Name + " is not supported by this storage");
        //        return result;
        //    }

        //    return AbstractEventStorage.Query(query, ignoreTemporal);
        //}

        //public QueryResult<TimeSliceId> QueryEventId(AbstractQuery query, bool ignoreTemporal = false)
        //{
        //    var result = new QueryResult<TimeSliceId>();

        //    //process LogicalQuery
        //    if (query is LogicalQuery)
        //    {
        //        var logicalQuery = query as LogicalQuery;
        //        foreach (AbstractQuery q in logicalQuery.Queries)
        //        {
        //            QueryResult<TimeSliceId> subResult = QueryEventId(q, ignoreTemporal);
        //            result.Add(subResult);
        //            if (!result.IsOk) return result;

        //            result.Combine(subResult, logicalQuery.LogicalCondition);

        //            //no need to perform all queries when no results are found in AND logic
        //            if (logicalQuery.LogicalCondition == LogicalCondition.And && result.Count() == 0) break;
        //        }
        //        return result;
        //    }

        //    //check support
        //    IList<Type> supportedTypes = GetSupportedQueryTypes();
        //    if (supportedTypes != null && !supportedTypes.Contains(query.GetType()))
        //    {
        //        result.ReportError("Query type " + query.GetType().Name + " is not supported by this storage");
        //        return result;
        //    }

        //    QueryResult<AbstractEvent<T>> resultEvent = AbstractEventStorage.Query(query, ignoreTemporal);

        //    if (resultEvent.IsOk)
        //    {
        //        result.Results = new SortedDictionary<int, IList<TimeSliceId>>();
        //        foreach (var pair in resultEvent.Results)
        //        {
        //            result.Results[pair.Key] = new List<TimeSliceId>();
        //            foreach (var abstractEvent in pair.Value)
        //            {
        //                var id = new TimeSliceId(abstractEvent) {Version = abstractEvent.Version};
        //                if (!result.Results[pair.Key].Contains(id))
        //                {
        //                    result.Results[pair.Key].Add(id);
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        result.ReportError(resultEvent.ErrorMessage);
        //    }

        //    return result;
        //}

        //public QueryResult<AbstractState<T>> QueryState(AbstractQuery query, DateTime actualDate,
        //                                                bool ignoreTemporal = false)
        //{
        //    var result = new QueryResult<AbstractState<T>>();

        //    //process LogicalQuery
        //    if (query is LogicalQuery)
        //    {
        //        var logicalQuery = query as LogicalQuery;
        //        foreach (AbstractQuery q in logicalQuery.Queries)
        //        {
        //            QueryResult<AbstractState<T>> subResult = QueryState(q, actualDate, ignoreTemporal);
        //            result.Add(subResult);
        //            if (!result.IsOk) return result;

        //            result.Combine(subResult, logicalQuery.LogicalCondition);

        //            //no need to perform all queries when no results are found in AND logic
        //            if (logicalQuery.LogicalCondition == LogicalCondition.And && result.Count() == 0) break;
        //        }
        //        return result;
        //    }

        //    //check support
        //    IList<Type> supportedTypes = GetSupportedQueryTypes();
        //    if (supportedTypes != null && !supportedTypes.Contains(query.GetType()))
        //    {
        //        result.ReportError("Query type " + query.GetType().Name + " is not supported by this storage");
        //        return result;
        //    }

        //    QueryResult<AbstractEvent<T>> resultEvent = QueryEvent(query, ignoreTemporal);

        //    if (resultEvent.IsOk)
        //    {
        //        result.Results = new SortedDictionary<int, IList<AbstractState<T>>>();
        //        foreach (var pair in resultEvent.Results)
        //        {
        //            var idList = new List<FeatureId>();
        //            foreach (var abstractEvent in pair.Value)
        //                if (abstractEvent.HasImpact(actualDate))
        //                {
        //                    var id = new FeatureId(abstractEvent);
        //                    if (!idList.Contains(id))
        //                    {
        //                        idList.Add(id);
        //                    }
        //                }

        //            if (idList.Count > 0)
        //            {
        //                result.Results[pair.Key] = new List<AbstractState<T>>();

        //                foreach (FeatureId featureId in idList)
        //                {
        //                    result.Results[pair.Key].Add(GetSnapshotByActualDate(featureId, actualDate, ignoreTemporal));
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        result.ReportError(resultEvent.ErrorMessage);
        //    }

        //    return result;
        //}

        #endregion

        #region Other

        //public IList<TimeSliceId> GetRelatedFeatures(IList<TimeSliceId> featureIdsParam)
        //{
        //    return _lockUtil.ManipulateWithData(featureIds =>
        //    {
        //        var relatedEvents = GetRelatedEvents(featureIds);
        //        var result = new SortedSet<TimeSliceId>();
        //        foreach (var relatedEvent in relatedEvents)
        //        {
        //            result.Add(new TimeSliceId(relatedEvent));
        //        }
        //        return result.ToList();
        //    },
        //    featureIdsParam);
        //}


        //public IList<AbstractEvent<T>> GetRelatedEvents(IList<TimeSliceId> featureIdsParam)
        //{
        //    return _lockUtil.ManipulateWithData(featureIds => AbstractEventStorage.GetRelatedEvents(featureIds),
        //                                        featureIdsParam);
        //}

        #endregion
    }
}