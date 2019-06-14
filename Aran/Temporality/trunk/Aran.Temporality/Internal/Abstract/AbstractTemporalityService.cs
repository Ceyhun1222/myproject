#region Namespaces

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using Aran.Aim;
using Aran.Aim.Data.Filters;
using Aran.Converters;
using Aran.Geometries;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Abstract.State;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Enum;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Exceptions;
using Aran.Temporality.Common.Extensions;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.Common.MetaData;
using Aran.Temporality.Common.OperationResult;
using Aran.Temporality.Common.Util;
using Aran.Temporality.Internal.Abstract.Storage;
using Aran.Temporality.Internal.Attribute;
using Aran.Temporality.Internal.Config;
using Aran.Temporality.Internal.Implementation.Storage;
using Aran.Temporality.Internal.Interface.Util;
using Aran.Temporality.Internal.MetaData;
using Aran.Temporality.Internal.Util.Lock;
using Aran.Temporality.Internal.WorkFlow;
using ESRI.ArcGIS.Geometry;
using FluentNHibernate.Conventions;
using Projection = Aran.Aim.Data.Projection;
#if TEST
using OperationContext = System.ServiceModel.Web.MockedOperationContext;
#endif

#endregion

namespace Aran.Temporality.Internal.Abstract
{
    internal abstract class AbstractTemporalityService<T> : NoAixmDataService, ITemporalityService<T> where T : class //T is feature type
    {
        #region Abstract methods

        public abstract AbstractEvent<T> FormCancelSequence(TimeSliceId timeSliceId, Interpretation interpretation, DateTime? cancelDate = null);

        #endregion

        #region Constructor, Destructor, options and handlers

        private Thread _workflowThread;
        private Thread _optimizerThread;
        private TemporalityLogicOptions _options;

        private AbstractTemporalityService()
        {
            Options = new TemporalityLogicOptions();
        }

        protected AbstractTemporalityService(string path) : this()
        {
            StorageName = path;
            //TODO: optimizer
#warning optimizer cause problems
            InitOptimizer();
            InitWorkflow();
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


        public int StorageId { get; set; }

        private string _storageName;
        public string StorageName
        {
            get { return _storageName; }
            set
            {
                _storageName = value;
                if (AbstractEventStorage != null) AbstractEventStorage.StorageName = _storageName;
                if (AbstractStateStorage != null) AbstractStateStorage.StorageName = _storageName;
            }
        }

        private void InitWorkflow()
        {
            _workflowThread = new Thread(
                ()
                    =>
                    {
                        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                        StorageService.ResetSlotStatus();
                        while (!_disposingNow)
                        {
                            //if (!Workflow())
                            Workflow();
                                Thread.Sleep(ConfigUtil.WorkflowSleepTime);
                        }
                        //Console.WriteLine("optimizer exited");
                    });

            //_workflowThread.Priority=ThreadPriority.Highest;
            _workflowThread.Start();
        }


        private void InitOptimizer()
        {
            _optimizerThread = new Thread(() =>
                                              {
                                                  Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                                                  Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                                                  while (!_disposingNow)
                                                  {
                                                      if (!Optimize()) Thread.Sleep(ConfigUtil.OptimizerSleepTime);
                                                  }
                                                  //Console.WriteLine("optimizer exited");
                                              });

            _optimizerThread.Start();
        }

        ~AbstractTemporalityService()
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

        internal abstract AbstractEventStorage<T> AbstractEventStorageInternal { get; }
        internal abstract AbstractStateStorage<T> AbstractStateStorageInternal { get; }

        private AbstractEventStorage<T> _abstractEventStorage;
        private AbstractStateStorage<T> _abstractStateStorage;

        internal AbstractEventStorage<T> AbstractEventStorage
        {
            get
            {
                if (_abstractEventStorage == null)
                {
                    _abstractEventStorage = AbstractEventStorageInternal;
                    _abstractEventStorage.TemporalityService = this;
                }
                return _abstractEventStorage;
            }
        }
        internal AbstractStateStorage<T> AbstractStateStorage
        {
            get
            {
                if (_abstractStateStorage == null)
                {
                    _abstractStateStorage = AbstractStateStorageInternal;
                    _abstractStateStorage.TemporalityService = this;
                }
                return _abstractStateStorage;
            }
        }



        #endregion

        #region Lock util

        private readonly ILockUtil _lockUtil = new MonitorUtil();
        private readonly ILogger _logger = LogManager.GetLogger(typeof(AbstractTemporalityService<T>));

        #endregion

        #region Private methods

        private CommonOperationResult CancelSequenceInternal(TimeSliceId timeSliceId, Interpretation interpretation, DateTime? cancelDate = null)
        {
            _logger.Debug("Cancelling sequence.");
            if (timeSliceId.Version.SequenceNumber == 1 && interpretation != Interpretation.TempDelta)
            {
                _logger.Debug("Sequence number equal 1");
                if (AbstractEventStorage.GetActualEventMeta(timeSliceId, null, null).Any(t => (t.Interpretation == Interpretation.TempDelta || t.Version.SequenceNumber != 1) && !t.IsCanceled))
                {
                    _logger.Warn($"Can not cancel sequence 1 when there is other active sequences in the same workpackage. {timeSliceId.Dump()} cancelDate: {cancelDate}");
                    _logger.Trace(() => AbstractEventStorage.GetActualEventMeta(timeSliceId, null, null).Where(t => t.Version.SequenceNumber != 1).ToList().Dump());
                    return new CommonOperationResult().ReportError("can not cancel sequence 1 when there is other active sequences in the same workpackage");
                }

                if (timeSliceId.WorkPackage != 0)
                {
                    //data thaty was cancelled in wp
                    var cancelledMeta = AbstractEventStorage.GetCancelledEventMeta(timeSliceId, null, null);
                    _logger.Trace(() => cancelledMeta.Dump());
                    //actual data from default wp with sequence number greater than 1
                    var defaultMeta = AbstractEventStorage.GetActualEventMeta(new FeatureId(timeSliceId) { WorkPackage = 0 }, null, null).
                        Where(metaData => metaData.Version.SequenceNumber != 1 && !metaData.IsCanceled).ToList();
                    _logger.Trace(() => defaultMeta.Dump());

                    //if there is actual data that was not cancelled
                    foreach (var metaData in defaultMeta)
                    {
                        if (!cancelledMeta.Any(t => t.Version.SequenceNumber == metaData.Version.SequenceNumber && t.Version.CorrectionNumber == metaData.Version.CorrectionNumber))
                        {
                            //report
                            _logger.Warn($"Can not cancel sequence 1 when there is other active sequences in default workpackage. {timeSliceId.Dump()} cancelDate: {cancelDate}");
                            _logger.Debug(() => metaData.Dump());
                            _logger.Trace(() => cancelledMeta.Where(
                                           t =>
                                               t.Version.SequenceNumber == metaData.Version.SequenceNumber &&
                                               t.Version.CorrectionNumber == metaData.Version.CorrectionNumber)
                                           .ToList()
                                           .Dump());

                            return new CommonOperationResult().ReportError("can not cancel sequence 1 when there is other active sequences in default workpackage");
                        }
                    }
                }
            }

            var myEvent = FormCancelSequence(timeSliceId, interpretation, cancelDate);

            return CommitCorrection(myEvent);
        }


        private static List<AbstractEvent<T>> ApplyCorrectionsWithoutInterpretation(IEnumerable<AbstractEvent<T>> list, bool includeCanceled = false)
        {
            var groups = list.GroupBy(t => t.Version.SequenceNumber);

            var result = new List<AbstractEvent<T>>();
            foreach (IGrouping<int, AbstractEvent<T>> myGroup in groups)
            {
                var correction = myGroup.Max(t => t.Version.CorrectionNumber);
                var actualEvent = myGroup.First(t => t.Version.CorrectionNumber == correction);
                if (!actualEvent.IsCanceled || includeCanceled)
                {
                    result.Add(actualEvent);
                }
            }
            return result;
        }

        private static List<AbstractEvent<T>> ApplyCorrections(IEnumerable<AbstractEvent<T>> list, bool includeCancelled = false)
        {
            var temp = list.Where(t => t.Interpretation == Interpretation.TempDelta);
            var perm = list.Where(t => t.Interpretation != Interpretation.TempDelta);
            var result = ApplyCorrectionsWithoutInterpretation(perm, includeCancelled);
            result.AddRange(ApplyCorrectionsWithoutInterpretation(temp, includeCancelled));
            return result;
        }

        private List<AbstractEvent<T>> SortEvents(IEnumerable<AbstractEvent<T>> events)
        {
            EnumComparer<Interpretation> comparer = new EnumComparer<Interpretation> { Interpretation.PermanentDelta, Interpretation.BaseLine, Interpretation.Snapshot, Interpretation.TempDelta };

            if (Options.SortingEventBy == TemporalityLogicOptions.SortingEventField.PublicDate)
            {
                return events.OrderBy(t => t.Interpretation, comparer).ThenBy(t => t.SubmitDate).ThenBy(t => t.TimeSlice.BeginPosition).ToList();
            }
            if (Options.SortingEventBy == TemporalityLogicOptions.SortingEventField.ValidStartDate)
            {
                return events.OrderBy(t => t.Interpretation, comparer).ThenBy(t => t.TimeSlice.BeginPosition).ThenBy(t => t.Version.SequenceNumber).ThenBy(t => t.SubmitDate).ToList();
            }

            throw new Exception("wrong option");
        }

        private List<AbstractEvent<T>> SortEventsWithoutInterpretation(IEnumerable<AbstractEvent<T>> events)
        {
            EnumComparer<Interpretation> comparer = new EnumComparer<Interpretation> { Interpretation.PermanentDelta, Interpretation.BaseLine, Interpretation.Snapshot, Interpretation.TempDelta };

            if (Options.SortingEventBy == TemporalityLogicOptions.SortingEventField.PublicDate)
            {
                return events.OrderBy(t => t.SubmitDate).ThenBy(t => t.TimeSlice.BeginPosition).ToList();
            }
            if (Options.SortingEventBy == TemporalityLogicOptions.SortingEventField.ValidStartDate)
            {
                return events.OrderBy(t => t.TimeSlice.BeginPosition).ThenBy(t => t.Version.SequenceNumber).ThenBy(t => t.SubmitDate).ToList();
            }

            throw new Exception("wrong option");
        }

        //lastEventCorrection != null only for correction
        private CommonOperationResult CommitEventInternal(AbstractEvent<T> myEvent, AbstractEventMetaData lastEventCorrection = null)
        {
            if (myEvent.WorkPackage != 0)
            {
                AbstractEventStorage.LoadMeta(myEvent.WorkPackage);
            }

            //set user extension 
            _logger.Debug("Committing event process");
            var result = new CommonOperationResult();
            try
            {
                _logger.Trace("Creation event extensions.");
                myEvent.CreateExtensions();
                _logger.Trace(() => $"Event extensions has been created. {myEvent.Dump()}");
                //check version
                if (AbstractEventStorage.Contains(myEvent))
                {
                    throw new Exception("Event with same id and version already exists");
                }

                //if (myEvent.TimeSlice!=null && !AbstractEventStorage.IsValidTimeUnique(myEvent))
                //{
                //    throw new Exception("Event with same start of valid time already exists");
                //}
                var newVersion = CreateNewVersion(myEvent, myEvent.Interpretation);

                if (myEvent.Version.SequenceNumber > newVersion.SequenceNumber)
                {
                    throw new Exception($"Wrong sequence number, should be not greater than {newVersion.SequenceNumber}");
                }

                //save event
                AbstractEventStorage.SaveEventInternal(myEvent);

                //clear all states after old version or new version  - which is earlier
                var date = myEvent.TimeSlice.BeginPosition;
                if (lastEventCorrection?.TimeSlice != null && lastEventCorrection.TimeSlice.BeginPosition < date)
                {
                    date = lastEventCorrection.TimeSlice.BeginPosition;
                }

                AbstractStateStorage.ClearStatesAfter(myEvent, date);

                _logger.Debug("Event has been commited.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error on committing event interval.");
                result.ReportError(ex.Message);
            }

            return result;
        }

        private static void SetUser(AbstractEventMetaData myEventParam)
        {
            LogManager.GetLogger(typeof(AbstractTemporalityService<T>)).Trace("Setting user parameters.");
            string user = OperationContext.Current?.ServiceSecurityContext?.PrimaryIdentity?.Name;

            if (ConfigUtil.ExternalApplication != null)
                user = ConfigUtil.ExternalApplicationUserName;

            myEventParam.User = user;

            LogManager.GetLogger(typeof(AbstractTemporalityService<T>)).Trace($"User parameters has been set. User: {user}");
        }




        private List<AbstractEvent<T>> GetEventsInternal(IFeatureId featureId, DateTime fromDate, DateTime? toDate, bool andOneMore = true, Interpretation? interpretation = null, DateTime? currentDate = null)
        {
            var events = GetEventsAffectingDateInterval(featureId, fromDate, toDate, andOneMore, false, list =>
           {
               var result = list;
               switch (interpretation)
               {
                   case Interpretation.PermanentDelta:
                       result = result.Where(t => t.Interpretation == Interpretation.PermanentDelta).ToList();
                       break;
                   case Interpretation.TempDelta:
                       result = result.Where(t => t.Interpretation == Interpretation.TempDelta).ToList();
                       break;
               }

               if (currentDate != null)
                   result.RemoveAll(t => t.SubmitDate > currentDate);

               return result;
           });

            if (events.IsNotEmpty())
            {
                events = SortEvents(events);
                events.ForEach(ApplyTimeSlice);
            }

            return events;
        }

        //currentDate is null means get normal
        //else get data as it was at currentDate (ignore all events submitted after)
        private IList<AbstractState<T>> GetActualDataInternal(IFeatureId featureId, DateTime dateTime, Interpretation
            interpretation = Interpretation.Snapshot, DateTime? endDate = null, Filter filter = null, Aim.Data.Projection projection = null)
        {
            if (featureId.WorkPackage != 0)
            {
                AbstractEventStorage.LoadMeta(featureId.WorkPackage);
            }

            projection?.IncludeFilterPaths(filter);

            var result = new List<AbstractState<T>>();
            var resultInterpretation = Interpretation.BaseLine;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            //get last state in current workpackage
            //or null if currentDate is set
            AbstractState<T> lastState = null;

            if (endDate == null)
            {
                lastState = AbstractStateStorage.GetLastKnownState(featureId, dateTime, true);
                stopwatch.Stop();
                //Console.WriteLine("last state in " +stopwatch.ElapsedMilliseconds);
                stopwatch.Restart();
            }


            if (lastState == null)
            {
                //if no state is found use default data
                lastState = AbstractStateStorage.GetByDefaultData(new StateMetaData(featureId));
            }

            //check if it is out-of-date
            if (lastState.TimeSlice.BeginPosition < dateTime)
            {
                var events = GetEventsAffectingDateInterval(featureId, lastState.TimeSlice.BeginPosition, dateTime, true, false,
                    list =>
                    {
                        list.RemoveAll(t => t.Interpretation == Interpretation.TempDelta && t.TimeSlice.EndPosition != null && t.TimeSlice.EndPosition < dateTime);
                        if (endDate != null && endDate != default(DateTime))
                        {

                            list.RemoveAll(t => t.Interpretation == Interpretation.TempDelta
                            && (t.TimeSlice.EndPosition != endDate || t.TimeSlice.BeginPosition != dateTime));
                        }
                        return list;
                    }, projection);





                //sort events if needed
                if (events.Count > 1) events = SortEvents(events);

                //apply events if any

                if (events.Count > 0)
                {
                    var nextDate = default(DateTime);
                    //make clone in order to protect state from further changes
                    lastState = CloneUtil.DeepClone(lastState);

                    //enum all appropriate (not ignored) events

                    foreach (var currentEvent in events.Where(currentEvent => !(Interpretation.BaseLine == interpretation && currentEvent.Interpretation == Interpretation.TempDelta)))
                    {
                        if (currentEvent.TimeSlice.BeginPosition <= dateTime &&
                            ((currentEvent.Interpretation == Interpretation.TempDelta && currentEvent.TimeSlice.EndPosition >= dateTime) ||
                            (currentEvent.Interpretation == Interpretation.PermanentDelta)))
                        {
                            ApplyEvent(currentEvent, lastState, featureId, dateTime);

                            if (currentEvent.Interpretation == Interpretation.TempDelta)
                                resultInterpretation = Interpretation.TempDelta;

                            if (currentEvent.TimeSlice.EndPosition != null)
                            {
                                lastState.TimeSlice.EndPosition = currentEvent.TimeSlice.EndPosition;
                            }

                            if (currentEvent.LifeTimeEndSet)
                            {
                                if (currentEvent.LifeTimeEnd < dateTime)
                                {
                                    return result;
                                }
                                else
                                    break;
                            }
                        }
                        else
                        {
                            if (currentEvent.Interpretation == Interpretation.PermanentDelta)
                            {
                                if (nextDate == default(DateTime))
                                    nextDate = currentEvent.TimeSlice.BeginPosition;
                            }
                            continue;
                        }
                    }

                    //update actual date
#warning There is no need to update as i think :)
                    //lastState.TimeSlice.BeginPosition = dateTime;
                    //set end position for baseline interpretation
                    if (nextDate != default(DateTime))
                    {
                        if (lastState.TimeSlice.EndPosition == null || lastState.TimeSlice.EndPosition > nextDate)
                            lastState.TimeSlice.EndPosition = nextDate;
                    }




                }
            }

            stopwatch.Stop();

            if (lastState.Version != new TimeSliceVersion(0, 0))
            {
                ApplyTimeSlice(lastState);
                ApplyInterpretation(lastState, resultInterpretation);
                //if at least any event were actually found
                
                if (AbstractEventStorage.CheckFilterRuntime(lastState?.Data, filter))
                    result.Add(lastState);
            }

            return result;
        }


        private List<AbstractEvent<T>> GetEventsAffectingDateInterval(IFeatureId featureId, DateTime fromDate, DateTime? toDate, bool andOneMore = true, bool includeCancelled = false, Func<List<AbstractEvent<T>>, List<AbstractEvent<T>>> filterFunc = null, Aim.Data.Projection projection = null)
        {
            var defaultFeatureId = new FeatureId(featureId) { WorkPackage = 0 };
            var events = new List<AbstractEvent<T>>();

            //get all known events from both current and default workpackage

            if (featureId.WorkPackage != 0)
            {
                //events from default workpackage
                events.AddRange(AbstractEventStorage.GetEventsAffectingDateInterval(
                    defaultFeatureId, fromDate, toDate, andOneMore));
            }

            //combine with events in current workpackage
            events.AddRange(AbstractEventStorage.GetEventsAffectingDateInterval(
                featureId, fromDate, toDate, andOneMore, projection));

            if (filterFunc != null)
                events = filterFunc(events);

            //apply cancellation and correction events
            events = ApplyCorrections(events, includeCancelled);

            return events;
        }

        public abstract void ApplyTimeSlice(AbstractState<T> state);

        public abstract void ApplyInterpretation(AbstractState<T> state, Interpretation tossInterpretation);

        public abstract void ApplyTimeSlice(AbstractEvent<T> abstractEvent);

        private IList<AbstractState<T>> GetActualDataByFeatureType(IFeatureId featureId, bool slotOnly, DateTime dateTime, Interpretation interpretation = Interpretation.Snapshot, DateTime? currentDate = null, Filter filter = null, Projection projection = null)
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            if (featureId.WorkPackage != 0)
            {
                AbstractEventStorage.LoadMeta(featureId.WorkPackage);
            }

            //TODO: use currentDate
#warning use currentDate

            //TODO: make it faster
#warning optimize
            var result = new List<AbstractState<T>>();

            var ids = AbstractEventStorage.GetActualIds(featureId, dateTime).ToList();
            var deletedIds = AbstractEventStorage.GetDeletedIds(featureId, dateTime).ToList();

            ids = ids.Except(deletedIds).ToList();
            ids = AbstractEventStorage.GetFilteredIds(featureId, filter, true, ids).ToList();

            var defaultIds = new List<Guid>();

            if (featureId.WorkPackage != 0)
            {
                if (!slotOnly)
                {
                    //get default ids except selected wp
                    defaultIds = AbstractEventStorage.GetActualIds(
                        new FeatureId(featureId)
                        {
                            WorkPackage = 0
                        },
                        dateTime).ToList();


                    defaultIds = defaultIds.Except(deletedIds).ToList();
                    defaultIds = defaultIds.Except(ids).ToList();
                    defaultIds = AbstractEventStorage.GetFilteredIds(new FeatureId(featureId)
                    {
                        WorkPackage = 0
                    }, filter, true, defaultIds).ToList();
                }
            }

            ids.ForEach(id =>
            {
                result.AddRange(GetActualDataInternal(new FeatureId(featureId)
                    {
                        Guid = id
                    },
                    dateTime,
                    interpretation,
                    currentDate,
                    filter,
                    projection));
            });



            if (featureId.WorkPackage != 0)
            {
                if (!slotOnly)
                {
                    //add data from default wp
                    defaultIds.ForEach(id =>
                    {
                        result.AddRange(GetActualDataInternal(new FeatureId(featureId)
                            {
                                Guid = id,
                                WorkPackage = 0
                            },
                            dateTime,
                            interpretation,
                            currentDate,
                            filter,
                            projection));
                    });
                }
            }

            s.Stop();
            var ms = s.ElapsedMilliseconds;

            return result;
        }


        private IList<AbstractEvent<T>> GetEventsByFeatureType(IFeatureId featureId, bool slotOnly, DateTime fromDateTime, DateTime toDateTime, Interpretation? interpretation = null, DateTime? currentDate = null)
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            if (featureId.WorkPackage != 0)
            {
                AbstractEventStorage.LoadMeta(featureId.WorkPackage);
            }

            var result = new List<AbstractEvent<T>>();

            var ids = AbstractEventStorage.GetActualIds(featureId, toDateTime).ToList();
            var deletedIds = AbstractEventStorage.GetDeletedIds(featureId, toDateTime).ToList();

            ids = ids.Except(deletedIds).ToList();

            var defaultIds = new List<Guid>();

            if (featureId.WorkPackage != 0)
            {
                if (!slotOnly)
                {
                    //get default ids except selected wp
                    defaultIds = AbstractEventStorage.GetActualIds(
                        new FeatureId(featureId)
                        {
                            WorkPackage = 0
                        },
                        toDateTime).ToList();


                    defaultIds = defaultIds.Except(deletedIds).ToList();
                    defaultIds = defaultIds.Except(ids).ToList();
                }
            }

            ids.ForEach(id =>
            {
                result.AddRange(GetEventsInternal(new FeatureId(featureId)
                {
                    Guid = id
                },
                    fromDateTime,
                    toDateTime,
                    false,
                    interpretation,
                    currentDate));
            });



            if (featureId.WorkPackage != 0)
            {
                if (!slotOnly)
                {
                    //add data from default wp
                    defaultIds.ForEach(id =>
                    {
                        result.AddRange(GetEventsInternal(new FeatureId(featureId)
                        {
                            Guid = id,
                            WorkPackage = 0
                        },
                            fromDateTime,
                            toDateTime,
                            false,
                            interpretation,
                            currentDate));
                    });
                }
            }

            s.Stop();
            var ms = s.ElapsedMilliseconds;

            return result;
        }

        #endregion

        #region Important data manipulation methods (using Lock)

        #region Write operations
        [SecureOperation(DataOperation.WriteData)]
        public CommonOperationResult CancelSequence(TimeSliceId myEvent, Interpretation interpretation, DateTime? cancelDate = null)
        {
            return _lockUtil.ManipulateWithData(CancelSequenceInternal, myEvent, interpretation, cancelDate);
        }

        // [SecureOperation(DataOperation.WriteData)]
        public CommonOperationResult CommitEvent(AbstractEvent<T> myEventParam)
        {
            _logger.Debug("Committing event");
            SetUser(myEventParam);

            //check all parameter set
            _logger.Debug("Validating meta data");
            _logger.Debug("Meta data has been validated");
            return _lockUtil.ManipulateWithData(myEvent => CommitEventInternal(myEvent), myEventParam);
        }

        [SecureOperation(DataOperation.WriteData)]
        public FeatureOperationResult CommitNewEvent(AbstractEvent<T> myEventParam)
        {
            _logger.Debug("Committing new event");
            return _lockUtil.ManipulateWithData(myEvent =>
            {
                var result = new FeatureOperationResult();
                try
                {

                    _logger.Debug("Creating new version");
                    myEvent.Version = CreateNewVersion(myEvent, myEvent.Interpretation);
                    _logger.Debug(() => $"New version has been created. Version: {myEvent.Version.Dump()}");

                    result.Add(CommitEvent(myEvent));
                    result.TimeSliceVersion = new TimeSliceVersion(myEvent.Version);
                    _logger.Debug("New event event has been committed");
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error on committing new event.");
                    result.ReportError(ex.Message);
                }

                return result;
            },
            myEventParam);
        }

        [SecureOperation(DataOperation.WriteData)]
        public FeatureOperationResult CommitCorrection(AbstractEvent<T> myEventParam)
        {
            _logger.Debug("Commiting correction.");

            SetUser(myEventParam);

            _logger.Trace(myEventParam.Dump);

            return _lockUtil.ManipulateWithData(myEvent =>
            {
                var result = new FeatureOperationResult();

                var lastEventCorrection = AbstractEventStorage.GetLastCorrection(myEvent);


                _logger.Trace(() => $"Last event correction: {lastEventCorrection.Dump()}");
                //form correct new version
                _logger.Debug("Creating new correction.");
                var correctNewVersion = new TimeSliceVersion(myEventParam.Version.SequenceNumber, lastEventCorrection.Version.CorrectionNumber);
                correctNewVersion.CorrectionNumber++;
                _logger.Debug($"New correction has been created. Sequence: {correctNewVersion.SequenceNumber} Corection: {correctNewVersion.CorrectionNumber}");
                //check versions
                _logger.Debug("Checking versions.");
                if (myEvent.Version != null && myEvent.Version.CorrectionNumber != -1)
                {
                    if (myEvent.Version != correctNewVersion)
                    {

                        var error = "Version of event is wrong. " +
                                    $"Expected: {correctNewVersion}, actual: {myEvent.Version}";
                        _logger.Warn(error);
                        result.ReportError(error);
                    }
                }
                _logger.Debug("Versions has been checked.");

                result.TimeSliceVersion = new TimeSliceVersion(correctNewVersion);
                if (result.IsOk) myEvent.Version = correctNewVersion;
                if (result.IsOk) result.Add(CommitEventInternal(myEvent, lastEventCorrection));
                _logger.Debug("Correction has been commited. ");


                return result;
            },
            myEventParam);
        }


        public bool IsComissioned(IFeatureId myEventParam)
        {
            return _lockUtil.ManipulateWithData(myEvent => AbstractEventStorage.Contains(myEvent, Interpretation.PermanentDelta), myEventParam);
        }

        public bool Contains(IFeatureId myEventParam)
        {
            return _lockUtil.ManipulateWithData(myEvent => AbstractEventStorage.Contains(myEvent, Interpretation.PermanentDelta) || AbstractEventStorage.Contains(myEvent, Interpretation.TempDelta), myEventParam);
        }

        public AbstractEventMetaData GetCommission(AbstractEvent<T> myEventParam)
        {
            return _lockUtil.ManipulateWithData(myEvent => AbstractEventStorage.GetCommissionEventMeta(myEvent), myEventParam);
        }

        public AbstractEventMetaData GetLastCorrection(AbstractEvent<T> myEventParam)
        {
            return _lockUtil.ManipulateWithData(myEvent => AbstractEventStorage.GetLastCorrection(myEvent), myEventParam);
        }

        [SecureOperation(DataOperation.WriteData)]
        public bool Decommission(IFeatureId featureIdParam, DateTime airacDateParam)
        {
            return _lockUtil.ManipulateWithData(DecommissionInternal, featureIdParam, airacDateParam);
        }

        #endregion

        #region Read operations


        [SecureOperation(DataOperation.ReadData)]
        public TimeSliceVersion CreateNewVersion(IFeatureId featureIdParam, Interpretation interpretationParam)
        {
            return _lockUtil.ManipulateWithData((featureId, interpretation) => AbstractEventStorage.CreateNewVersion(featureId, interpretation),
                                                featureIdParam, interpretationParam);
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


        [SecureOperation(DataOperation.ReadData)]
        public IList<AbstractEvent<T>> GetEventsByDate(IFeatureId featureIdParam, bool slotOnly, DateTime fromDateTimeParam, DateTime toDateTimeParam, Interpretation? interpretationParam = null, DateTime? currentDateParam = null)
        {
            if (featureIdParam.FeatureTypeId == 0 && featureIdParam.Guid != null)
            {
                return new List<AbstractEvent<T>>();
            }

            return _lockUtil.ManipulateWithData((featureId, fromDateTime, toDateTime, interpretation, currentDate) =>
                {

                    if (toDateTime == default(DateTime))
                    {
                        throw new Exception("Correct actual date or version should be specified");
                    }

                    return featureId.Guid == null ? GetEventsByFeatureType(featureId, slotOnly, fromDateTime, toDateTime, interpretation, currentDate)
                        : GetEventsInternal(featureId, fromDateTime, toDateTime, false, interpretation, currentDate);
                },
                featureIdParam,
                fromDateTimeParam,
                toDateTimeParam,
                interpretationParam,
                currentDateParam);
        }

        [SecureOperation(DataOperation.ReadData)]
        public IList<AbstractState<T>> GetActualDataByDate(IFeatureId featureIdParam, bool slotOnly, DateTime dateTimeParam, Interpretation interpretationParam = Interpretation.Snapshot, DateTime? currentDateParam = null, Filter filterParam = null, Projection projectionParam = null)
        {
            if (featureIdParam.FeatureTypeId == 0 && featureIdParam.Guid != null)
            {
                return new List<AbstractState<T>>();
            }

            return _lockUtil.ManipulateWithData((featureId, dateTime, interpretation, currentDate, filter, projection) =>
                {

                    if (dateTime == default(DateTime))
                    {
                        throw new Exception("Correct actual date or version should be specified");
                    }

                    return featureId.Guid == null ? GetActualDataByFeatureType(featureId, slotOnly, dateTime, interpretation, currentDate, filter, projection)
                        : GetActualDataInternal(featureId, dateTime, interpretation, currentDate, filter, projection);
                },
                featureIdParam,
                dateTimeParam,
                interpretationParam,
                currentDateParam,
                filterParam,
                projectionParam);
        }


        [SecureOperation(DataOperation.ReadData)]
        public IList<AbstractState<T>> GetActualDataByGeo(IFeatureId maskParam, bool slotOnlyParam, DateTime effectiveDateParam, Geometry geoParam, double distanceParam)
        {
            return _lockUtil.ManipulateWithData((mask, slotOnly, effectiveDate, geo, distance)
                => GetActualDataByGeoInternal(mask, slotOnly, effectiveDate, geo, distance, Interpretation.BaseLine),
            maskParam,
            slotOnlyParam,
            effectiveDateParam,
            geoParam,
            distanceParam);
        }

        [SecureOperation(DataOperation.ReadData)]
        public IList<AbstractState<T>> GetStatesInRange(IFeatureId featureIdParam, bool slotOnlyParam, DateTime dateTimeStartParam,
            DateTime dateTimeEndParam)
        {
            return GetStatesInRangeByInterpretation(featureIdParam, slotOnlyParam, dateTimeStartParam, dateTimeEndParam,
                Interpretation.BaseLine);
        }

        [SecureOperation(DataOperation.ReadData)]
        public virtual IList<AbstractState<T>> GetStatesInRangeByInterpretation(IFeatureId featureIdParam, bool slotOnlyParam, DateTime dateTimeStartParam,
            DateTime dateTimeEndParam, Interpretation interpretation)
        {
            return _lockUtil.ManipulateWithData((featureId, slotOnly, dateTimeStart, dateTimeEnd) =>
                {

                    if (dateTimeStart == default(DateTime))
                    {
                        throw new Exception("Correct actual date or version should be specified");
                    }

                    if (dateTimeEnd == default(DateTime))
                    {
                        throw new Exception("Correct actual date or version should be specified");
                    }
                    return featureId.Guid == null ? GetFeaturesStatesByRangeInternal(featureId, slotOnly, dateTimeStart, dateTimeEnd, interpretation)
                        : GetFeatureStatesByRangeInternal(featureId, dateTimeStart, dateTimeEnd, interpretation);
                },
                featureIdParam,
                slotOnlyParam,
                dateTimeStartParam,
                dateTimeEndParam);
        }
        #endregion

    
        [SecureOperation(StorageOperation.TruncateStorage)]
        public void Truncate()
        {
            _lockUtil.ManipulateWithData(() =>
                                             {
                                                 AbstractEventStorage.Truncate();
                                                 AbstractStateStorage.Truncate();
                                             });
        }

        public void Setup()
        {
            StorageService.NeedSetup = true;
        }

        public bool Workflow()
        {
            return _lockUtil.ManipulateWithData<bool>(DoWorkflow);
        }


        public abstract bool DoWorkflow();


        //enabled for everyone
        public bool Optimize()
        {
            return _lockUtil.ManipulateWithData<bool>(AbstractStateStorage.Optimize, 1);
        }


        private List<AbstractEvent<T>> SelectAppliedEvents(List<AbstractEvent<T>> eventsSorted, DateTime actualDate)
        {
            List<AbstractEvent<T>> result = new List<AbstractEvent<T>>();

            foreach (var currentEvent in eventsSorted)
            {

                if (currentEvent.TimeSlice.BeginPosition <= actualDate &&
                    (currentEvent.TimeSlice.EndPosition == null || currentEvent.TimeSlice.EndPosition > actualDate))
                {

                    result.Add(currentEvent);
                }
                else if (currentEvent.TimeSlice.BeginPosition > actualDate)
                {
                    break;
                }
            }

            return result;
        }

        private AbstractState<T> ApplyEvents(AbstractState<T> lastStateOrigin, List<AbstractEvent<T>> eventsSorted, DateTime actualDate, DateTime endDate)
        {
            List<AbstractEvent<T>> tempEvents = new List<AbstractEvent<T>>();

            //make clone in order to protect state from further changes
            var lastState = CloneUtil.DeepClone(lastStateOrigin);

            var allPermEvents = SortEvents(eventsSorted.Where(t => t.Interpretation != Interpretation.TempDelta));
            var allTempEvents = SortEvents(eventsSorted.Where(t => t.Interpretation == Interpretation.TempDelta));


            var permEvents = SelectAppliedEvents(allPermEvents, actualDate);
            if (permEvents.IsNotEmpty() && !permEvents.Last().LifeTimeEndSet)
                tempEvents = SelectAppliedEvents(allTempEvents, actualDate);


            // At first apply permanent eventsSorted
            foreach (var currentEvent in permEvents)
            {
                ApplyEvent(currentEvent, lastState);
            }

            // then temporary eventsSorted
            foreach (var currentEvent in tempEvents)
            {
                ApplyEvent(currentEvent, lastState);
            }


            lastState.TimeSlice.BeginPosition = actualDate;
            lastState.TimeSlice.EndPosition = (DateTime?)(endDate == default(DateTime) ? (ValueType)null : endDate);

            ApplyTimeSlice(lastState);

            ApplyInterpretation(lastState, tempEvents.IsNotEmpty() ? Interpretation.TempDelta : Interpretation.BaseLine);


            return CloneUtil.DeepClone(lastState);
        }

        private static void ApplyEvent(AbstractEvent<T> currentEvent, AbstractState<T> lastState)
        {
            currentEvent.ApplyToData(lastState.Data, currentEvent.TimeSlice.BeginPosition);

            //new version will be equal to event's version
            lastState.Version = new TimeSliceVersion(currentEvent.Version);
        }


        private static void ApplyEvent(AbstractEvent<T> currentEvent, AbstractState<T> lastState, IFeatureId featureId,
            DateTime dateTime)
        {
            //apply event
            currentEvent.ApplyToData(lastState.Data, dateTime);

            //new states will be created in specified workpackage
            lastState.WorkPackage = featureId.WorkPackage;

            //new version will be equal to event's version
            lastState.Version = new TimeSliceVersion(currentEvent.Version);

            //timeslice will be equal to event's timeslice
            lastState.TimeSlice.BeginPosition = currentEvent.TimeSlice.BeginPosition;

        }


        private IList<AbstractState<T>> GetFeatureStatesByRangeInternal(IFeatureId featureId,
            DateTime dateTimeStart, DateTime dateTimeEnd, Interpretation interpretation)
        {
            if (featureId.WorkPackage != 0)
            {
                AbstractEventStorage.LoadMeta(featureId.WorkPackage);
            }


            var stopwatch = new Stopwatch();
            stopwatch.Start();

            //get last state in current workpackage
            //or null if currentDate is set
            AbstractState<T> lastState = null;


            {
                lastState = AbstractStateStorage.GetLastKnownState(featureId, dateTimeStart, true);
                stopwatch.Stop();
                //Console.WriteLine("last state in " +stopwatch.ElapsedMilliseconds);
                stopwatch.Restart();
            }

            if (lastState == null)
            {
                //if no state is found use default data
                lastState = AbstractStateStorage.GetByDefaultData(new StateMetaData(featureId));
            }

            var events = GetEventsAffectingDateInterval(featureId, lastState.TimeSlice.BeginPosition, dateTimeEnd, true);


            //sort events if needed
            if (events.Count > 1) events = SortEventsWithoutInterpretation(events);

            if (interpretation == Interpretation.PermanentDelta || interpretation == Interpretation.BaseLine)
                events = events.Where(t => t.Interpretation != Interpretation.TempDelta).ToList();

            var keyDates = new SortedSet<DateTime>() { dateTimeStart };
            bool stop = false;
            events.ForEach(t =>
            {
                if (stop)
                    return;
                if (t.TimeSlice.BeginPosition >= dateTimeStart && t.TimeSlice.BeginPosition <= dateTimeEnd)
                {
                    if (t.LifeTimeEndSet)
                    {
                        stop = true;
                        keyDates.Remove(t.TimeSlice.BeginPosition);
                        return;
                    }
                    keyDates.Add(t.TimeSlice.BeginPosition);
                }
                if (t.TimeSlice.EndPosition != null)
                {
                    if ((DateTime)t.TimeSlice.EndPosition >= dateTimeStart && (DateTime)t.TimeSlice.EndPosition <= dateTimeEnd)
                    {
                        keyDates.Add((DateTime)t.TimeSlice.EndPosition);
                    }
                }
            });


            var result = new List<AbstractState<T>>();
            DateTime prevDate = default(DateTime);
            foreach (var nextDate in keyDates)
            {
                if (prevDate != default(DateTime))
                {
                    result.Add(ApplyEvents(lastState, events, prevDate, nextDate));
                }
                prevDate = nextDate;
            }
            if (prevDate != default(DateTime) && !prevDate.Equals(dateTimeEnd))
                result.Add(ApplyEvents(lastState, events, prevDate, /*default(DateTime)*/dateTimeEnd));

            stopwatch.Stop();
            return result.Where(t => t.Version.SequenceNumber > 0).ToList();
        }

        private IList<AbstractState<T>> GetFeaturesStatesByRangeInternal(IFeatureId featureId, bool slotOnly, DateTime dateTimeStart, DateTime dateTimeEnd, Interpretation interpretation)
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            if (featureId.WorkPackage != 0)
            {
                AbstractEventStorage.LoadMeta(featureId.WorkPackage);
            }

            //TODO: use currentDate
#warning use currentDate

            //TODO: make it faster
#warning optimize
            var result = new List<AbstractState<T>>();

            var ids = AbstractEventStorage.GetActualIds(featureId, dateTimeStart, dateTimeEnd).ToList();
            var deletedIds = AbstractEventStorage.GetDeletedIds(featureId, dateTimeStart).ToList();

            ids = ids.Except(deletedIds).ToList();

            var defaultIds = new List<Guid>();

            if (featureId.WorkPackage != 0)
            {
                if (!slotOnly)
                {
                    //get default ids except selected wp
                    defaultIds = AbstractEventStorage.GetActualIds(
                        new FeatureId(featureId)
                        {
                            WorkPackage = 0
                        },
                        dateTimeStart, dateTimeEnd).ToList();


                    defaultIds = defaultIds.Except(deletedIds).ToList();
                    defaultIds = defaultIds.Except(ids).ToList();
                }
            }


            ids.ForEach(id =>
            {
                result.AddRange(GetFeatureStatesByRangeInternal(new FeatureId(featureId)
                {
                    Guid = id
                }, dateTimeStart, dateTimeEnd, interpretation));
            });



            if (featureId.WorkPackage != 0)
            {
                if (!slotOnly)
                {
                    //add data from default wp
                    defaultIds.ForEach(id =>
                    {
                        result.AddRange(GetFeatureStatesByRangeInternal(new FeatureId(featureId)
                        {
                            Guid = id,
                            WorkPackage = 0,
                        }, dateTimeStart, dateTimeEnd, interpretation));
                    });
                }
            }

            s.Stop();
            var ms = s.ElapsedMilliseconds;

            return result;
        }


        private IList<AbstractState<T>> GetActualDataByGeoInternal(IFeatureId featureId, bool slotOnly, DateTime dateTime, Geometry aranGeometry, double distance,
            Interpretation interpretation, DateTime? currentDate = null)
        {
            var geo = ConvertToEsriGeom.FromGeometry(aranGeometry);


            Stopwatch s = new Stopwatch();
            s.Start();

            if (featureId.WorkPackage != 0)
            {
                AbstractEventStorage.LoadMeta(featureId.WorkPackage);
            }

            //TODO: use currentDate
#warning use currentDate

            //TODO: make it faster
#warning optimize
            var result = new List<AbstractState<T>>();

            var ids = AbstractEventStorage.GetActualIds(featureId, dateTime).ToList();
            var deletedIds = AbstractEventStorage.GetDeletedIds(featureId, dateTime).ToList();

            ids = ids.Except(deletedIds).ToList();

            var defaultIds = new List<Guid>();

            if (featureId.WorkPackage != 0)
            {
                if (!slotOnly)
                {
                    //get default ids except selected wp
                    defaultIds = AbstractEventStorage.GetActualIds(
                        new FeatureId(featureId)
                        {
                            WorkPackage = 0
                        },
                        dateTime).ToList();


                    defaultIds = defaultIds.Except(deletedIds).ToList();
                    defaultIds = defaultIds.Except(ids).ToList();
                }
            }


            ids.ForEach(id =>
            {
                var stateList = GetActualDataInternal(new FeatureId(featureId)
                {
                    Guid = id
                },
                    dateTime,
                    interpretation,
                    currentDate);

                if (stateList == null || stateList.Count == 0) return;
                var state = stateList.First();

                if (IsGeoIntersect(state, geo, distance))
                {
                    result.Add(state);
                }
            });



            if (featureId.WorkPackage != 0)
            {
                if (!slotOnly)
                {
                    //add data from default wp
                    defaultIds.ForEach(id =>
                    {
                        var stateList = GetActualDataInternal(new FeatureId(featureId)
                        {
                            Guid = id,
                            WorkPackage = 0
                        },
                    dateTime,
                    interpretation,
                    currentDate);

                        if (stateList == null || stateList.Count == 0) return;
                        var state = stateList.First();

                        if (IsGeoIntersect(state, geo, distance))
                        {
                            result.Add(state);
                        }
                    });
                }
            }

            s.Stop();
            var ms = s.ElapsedMilliseconds;

            return result;
        }

        public abstract bool IsGeoIntersect(AbstractState<T> state, IGeometry geo, double distance);

        #endregion

        #region Slot publishing
        public void PublishPublicSlot(PublicSlot publicSlot)
        {
            try
            {

                _logger.Info($"Publishing public slot. Id:{publicSlot.Id} Name:{publicSlot.Name} " +
                                                     $"Effective Date: {publicSlot.EffectiveDate}.");

                if (!AiracCycle.CanPublish(publicSlot.EffectiveDate, publicSlot.SlotType == 0))
                    if (publicSlot.SlotType == 0)
                        throw new OperationException("Public slot must be published at least today."/*"Public slot must be published at least 42 days in advance of the effective date."*/);
                    else
                        throw new OperationException("Public slot must be published at least today.");


                var privateSlots = GetPrivateSlots(publicSlot.Id, -1);

                if (privateSlots.Count != 1)
                    throw new OperationException("Public slot must have only one private slot.");

                var privateSlotIds = privateSlots.Select(t => t.Id).ToList();

                //CheckEffectiveDate(publicSlot, privateSlotIds); temporary commented due to customer's desire 


                Stopwatch stopwatch = Stopwatch.StartNew();
                if (AbstractEventStorage.CanCommitWorkPackage(privateSlotIds))
                {

                    foreach (var workPackage in privateSlots)
                    {
                        _logger.Info($"Publishing private slot. Public slot id:{publicSlot.Id} " +
                                                        $"id:{workPackage.Id} name: {workPackage.Name} Creation date:{workPackage.CreationDate}.");
                        AbstractEventStorage.CommitWorkPackage(workPackage.Id);
                        _logger.Info($"Private slot {workPackage.Id} published.");
                    }
                    stopwatch.Stop();
                    _logger.Info($"Public slot {publicSlot.Id} published.");
                    _logger.Debug($"Publishing time: {stopwatch.ElapsedMilliseconds} ms and {stopwatch.ElapsedTicks} ticks.");



                    foreach (var privateSlot in privateSlots)
                    {
                        _logger.Info($"Updating private slot status to published. Id:{privateSlot.Id} Name: {privateSlot.Name}.");
                        privateSlot.Status = SlotStatus.Published;
                        UpdatePrivateSlot(privateSlot);
                        _logger.Info($"Private slot {privateSlot.Id} status changed.");
                    }
                    _logger.Info($"Updating public slot status to published. Id:{publicSlot.Id} Name: {publicSlot.Name}.");
                    publicSlot.Status = SlotStatus.Published;
                    UpdatePublicSlot(publicSlot);
                    _logger.Info($"Public slot {publicSlot.Id} status changed.");

                    //TODO:Debug and uncomment
#warning Debug and uncomment
                    //CreateStates(publicSlot);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error while publishing public slot. Id:{publicSlot.Id} Message:{ex.Message}");
                var errorMessage = "Error while publishing: " + ex.Message + ". Stacktrace: " + ex.StackTrace;
                var report = new ProblemReport
                {
                    DateTime = DateTime.Now,
                    PublicSlotId = publicSlot.Id,
                    ReportType = (int)ReportType.PublishingError,
                    ReportData = GetBytes(errorMessage)
                };

                UpdateProblemReport(report);

                _logger.Info($"Updating public slot status to publishing failed. Id:{publicSlot.Id} Name: {publicSlot.Name}.");
                publicSlot.Status = SlotStatus.PublishingFailed;
                UpdatePublicSlot(publicSlot);
                _logger.Info($"Public slot {publicSlot.Id} status changed.");
            }

        }

        private void CheckEffectiveDate(PublicSlot publicSlot, List<int> privateSlotIds)
        {
            foreach (var workPackage in privateSlotIds)
            {
                AbstractEventStorage.LoadMeta(workPackage);
                var metas = AbstractEventStorage.GetWorkPackageMeta(workPackage);
                foreach (var meta in metas)
                {
                    if (meta.Interpretation == Interpretation.TempDelta)
                    {
                        if (meta.TimeSlice.BeginPosition < publicSlot.EffectiveDate)
                            throw new OperationException(
                                $"Begin of valid time is less than slot effective date. {GetMetaDescription(meta)}");
                    }
                    else if (meta.Interpretation == Interpretation.PermanentDelta && !meta.IsCanceled)
                    {
                        if (meta.TimeSlice.BeginPosition != publicSlot.EffectiveDate)
                            throw new OperationException(
                                $"Begin of valid time is not equal to slot effective date. {GetMetaDescription(meta)}");
                    }
                    else if (meta.Interpretation != Interpretation.PermanentDelta && meta.Interpretation != Interpretation.TempDelta)
                        throw new OperationException(
                            $"Interpretation is incorrect. {GetMetaDescription(meta)}");
                }
            }
        }

        private static string GetMetaDescription(AbstractEventMetaData meta)
        {
            return $"Uuid: {meta.Guid}, FeatureType: {(FeatureType)meta.FeatureTypeId}, Interpretation: {meta.Interpretation} Version: {meta.Version}, Valid date: {meta.TimeSlice}";
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

        #region Slot

        public IList<int> GetPrivateSlotFeatureTypes(int slotIdParam)
        {
            return _lockUtil.ManipulateWithData(GetSlotFeatureTypesInternal, slotIdParam);
        }

        private IList<int> GetSlotFeatureTypesInternal(int slotId)
        {
            return AbstractEventStorage.GetSlotFeatureTypesInternal(slotId);
        }

        #endregion


        public StateWithDelta<T> GetActualDataForEditing(IFeatureId featureIdParam, DateTime actualDateParam, Interpretation interpretationParam = Interpretation.Snapshot, DateTime? endDateParam = null)
        {
            return _lockUtil.ManipulateWithData((featureId, actualDate, interpretation, endDate) =>
            {

                if (actualDate == default(DateTime))
                {
                    throw new Exception("Correct actual date should be specified");
                }

                return GetActualDataForEditingInternal(featureId, actualDate, interpretationParam, endDate);
            },
           featureIdParam,
           actualDateParam,
           interpretationParam,
           endDateParam);
        }

        private StateWithDelta<T> GetActualDataForEditingInternal(IFeatureId featureId, DateTime dateTime, Interpretation interpretation = Interpretation.Snapshot, DateTime? endDate = null)
        {

            if (featureId.WorkPackage != 0)
            {
                AbstractEventStorage.LoadMeta(featureId.WorkPackage);
            }

            var defaultFeatureId = new FeatureId(featureId) { WorkPackage = 0 };

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            //get last state in current workpackage
            //or null if currentDate is set
            AbstractState<T> lastState = null;


            lastState = AbstractStateStorage.GetLastKnownState(featureId, dateTime, false);
            stopwatch.Stop();
            //Console.WriteLine("last state in " +stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();


            if (lastState == null)
            {
                //if no state is found use default data
                lastState = AbstractStateStorage.GetByDefaultData(new StateMetaData(featureId));
            }


            var delta = AbstractStateStorage.GetByDefaultData(new StateMetaData(featureId));
            AbstractState<T> stateAfterDelta = null;

            //check if it is out-of-date
            if (lastState.TimeSlice.BeginPosition < dateTime)
            {

                var events = GetEventsAffectingDateInterval(featureId, lastState.TimeSlice.BeginPosition, dateTime, false);


                //TODO: check lifetime
                //DateTime lifeTimeBegin = GetLifeTimeBegin(events);


                if (interpretation == Interpretation.BaseLine)
                    events = events.Where(currentEvent => currentEvent.Interpretation != Interpretation.TempDelta).ToList();
                else if (interpretation == Interpretation.Snapshot)
                {
                    if (endDate != null && endDate != default(DateTime))
                    {
                        events = events
                            .Where(currentEvent => currentEvent.Interpretation != Interpretation.TempDelta ||
                                                   (currentEvent.TimeSlice.BeginPosition == dateTime &&
                                                    currentEvent.TimeSlice.EndPosition == endDate))
                            .ToList();
                    }
                }
                //sort events if needed
                if (events.Count > 1) events = SortEvents(events);

                //apply events if any
                if (events.Count > 0)
                {
                    var nextDate = default(DateTime);

                    //make clone in order to protect state from further changes
                    lastState = CloneUtil.DeepClone(lastState);

                    //enum all appropriate (not ignored) events
                    foreach (var currentEvent in events)
                    {
                        if (currentEvent.TimeSlice.BeginPosition < dateTime)
                        {
                            ApplyEvent(currentEvent, lastState, featureId, dateTime);
                        }
                        else if (currentEvent.TimeSlice.BeginPosition == dateTime)
                        {
                            //apply event
                            currentEvent.ApplyToData(delta.Data, dateTime);
                            if (stateAfterDelta == null)
                                stateAfterDelta = CloneUtil.DeepClone(lastState);

                            currentEvent.ApplyToData(stateAfterDelta.Data, dateTime);
                            stateAfterDelta.Version = new TimeSliceVersion(currentEvent.Version);
                            stateAfterDelta.TimeSlice.BeginPosition = currentEvent.TimeSlice.BeginPosition;

                            //new states will be created in specified workpackage
                            delta.WorkPackage = featureId.WorkPackage;

                            //new version will be equal to event's version
                            delta.Version = new TimeSliceVersion(currentEvent.Version);

                            //timeslice will be equal to event's timeslice
                            delta.TimeSlice.BeginPosition = currentEvent.TimeSlice.BeginPosition;

                            //TODO return meaningfull value
                            if (currentEvent.LifeTimeEndSet)
                                return null;


                        }
                        else
                        {
                            break;
                        }
                    }

                    //update actual date
                    lastState.TimeSlice.BeginPosition = dateTime;
                    //set end position for baseline interpretation
                    if (nextDate != default(DateTime))
                    {
                        lastState.TimeSlice.EndPosition = nextDate;
                    }


                }
            }

            stopwatch.Stop();

            if (lastState.Version != new TimeSliceVersion(0, 0) || delta.Version != new TimeSliceVersion(0, 0))
            {
                if (delta.Version == new TimeSliceVersion(0, 0))
                {
                    stateAfterDelta = CloneUtil.DeepClone(lastState);
                }

                ApplyTimeSlice(lastState);
                ApplyTimeSlice(delta);
                ApplyTimeSlice(stateAfterDelta);

                //if at least any event were actually found
                return new StateWithDelta<T>
                {
                    StateBeforeDelta = lastState.Version == new TimeSliceVersion(0, 0) ? null : lastState.Data,
                    Delta = delta.Version == new TimeSliceVersion(0, 0) ? null : delta.Data,
                    StateAfterDelta = stateAfterDelta?.Data
                };
            }

            return null;
        }


        public IList<AbstractEvent<T>> GetEvolution(IFeatureId featureIdParam)
        {
            return _lockUtil.ManipulateWithData(GetEvolutionInternal, featureIdParam);
        }

        private IList<AbstractEvent<T>> GetEvolutionInternal(IFeatureId featureId)
        {
            if (featureId.WorkPackage != 0)
            {
                AbstractEventStorage.LoadMeta(featureId.WorkPackage);
            }


            var events = GetEventsAffectingDateInterval(featureId, new DateTime(), null, true, true);

            //sort events if needed
            if (events.Count > 1) events = SortEvents(events);

            foreach (var abstractEvent in events)
            {
                ApplyTimeSlice(abstractEvent);
            }

            return events;
        }

        public void GetConfigurationData(int id)
        {
            new FeatureDependencyService<T>().GetConfigurationData(id, this);
        }

        public IList<AbstractEvent<T>> GetChangesInInterval(IFeatureId featureIdParam, DateTime startDateParam, DateTime endDateParam, bool slotOnlyParam)
        {
            return _lockUtil.ManipulateWithData(GetChangesInIntervalInternal,
                                                featureIdParam,
                                                startDateParam,
                                                endDateParam,
                                                slotOnlyParam);
        }

        private IList<AbstractEvent<T>> GetChangesInIntervalInternal(IFeatureId featureId, DateTime startDate, DateTime endDate, bool slotOnly)
        {
            var currentEvents = AbstractEventStorage.GetChangesInIntervalInternal(featureId, startDate, endDate).ToList();
            if (featureId.WorkPackage != 0 && !slotOnly)
            {
                var defaultChanges = AbstractEventStorage.GetChangesInIntervalInternal(
                    new FeatureId(featureId) { WorkPackage = 0 }, startDate, endDate);

                currentEvents.AddRange(defaultChanges);
            }

            return currentEvents;
        }

        public IList<AbstractEventMetaData> GetChangesMetaInInterval(IFeatureId featureIdParam, DateTime startDateParam, DateTime endDateParam, bool slotOnlyParam)
        {
            return _lockUtil.ManipulateWithData(GetChangesMetaInIntervalInternal,
                                                featureIdParam,
                                                startDateParam,
                                                endDateParam,
                                                slotOnlyParam);
        }

        private IList<AbstractEventMetaData> GetChangesMetaInIntervalInternal(IFeatureId featureId, DateTime startDate, DateTime endDate, bool slotOnly)
        {
            var currentEvents = AbstractEventStorage.GetChangesMetaInIntervalInternal(featureId, startDate, endDate).ToList();
            if (featureId.WorkPackage != 0 && !slotOnly)
            {
                var defaultChanges = AbstractEventStorage.GetChangesMetaInIntervalInternal(
                    new FeatureId(featureId) { WorkPackage = 0 }, startDate, endDate);

                currentEvents.AddRange(defaultChanges);
            }

            return currentEvents;
        }

        public OperationStatus GetCurrentOperationStatus()
        {
            return CurrentOperationStatus.Status();
        }

        public bool DeletePrivateSlot(int id)
        {
            StorageService.DeletePrivateSlot(id);
            AbstractEventStorage.DeleteWorkPackage(id);
            AbstractStateStorage.DeleteWorkPackage(id);
            return true;
        }

        private bool DecommissionInternal(IFeatureId featureId, DateTime airacDate)
        {
            _logger.Debug("Decommisioning feature");
            if (featureId.WorkPackage != 0)
            { AbstractEventStorage.LoadMeta(featureId.WorkPackage); }


            var myEvents = GetEventsAffectingDateInterval(featureId, airacDate, airacDate, true, false, list =>
            {
                return list.Where(t => t.Interpretation != Interpretation.TempDelta).ToList();
            });


            if (myEvents.Count > 0)
            {
                _logger.Debug("Creating new correction.");
                var lastEvent = myEvents.First();

                //we should correct last event
                var lastEventCorrection = AbstractEventStorage.GetLastCorrection(lastEvent);
                if (lastEventCorrection == null)
                {
                    //result.ReportError("Previous version of event was not found while performing correction");
                    _logger.Warn("Previous version of event was not found while performing correction");
                    return false;
                }

                //form correct new version
                var correctNewVersion = new TimeSliceVersion(lastEvent.Version.SequenceNumber,
                                                             lastEventCorrection.Version.CorrectionNumber);
                correctNewVersion.CorrectionNumber++;

                lastEvent.Version = correctNewVersion;

                lastEvent.LifeTimeEnd = airacDate;
                lastEvent.TimeSlice.EndPosition = airacDate;

                ApplyTimeSlice(lastEvent);

                //it should be in current wp anyway
                lastEvent.WorkPackage = featureId.WorkPackage;
                _logger.Debug("Correction has been formed");
                _logger.Trace(lastEvent.Dump);

                CommitEventInternal(lastEvent, lastEventCorrection);

            }
            else
            {

                myEvents = GetEventsAffectingDateInterval(featureId, new DateTime(), airacDate, true, false, list =>
                {
                    return list.Where(t => t.Interpretation != Interpretation.TempDelta).ToList();
                });

                if (myEvents.IsEmpty())
                {
                    throw new Exception($"Feature was not comissioned or comissioned after {airacDate:G}");
                }

                _logger.Debug("No events has been found. Creating new event");
                var timeSlice = new TimeSliceId
                {
                    FeatureTypeId = featureId.FeatureTypeId,
                    Guid = featureId.Guid,
                    WorkPackage = featureId.WorkPackage
                };
                var newEvent = FormCancelSequence(timeSlice, Interpretation.PermanentDelta, airacDate);

                newEvent.TimeSlice = new TimeSlice(airacDate);
                newEvent.FeatureTypeId = featureId.FeatureTypeId;
                newEvent.Guid = featureId.Guid;
                newEvent.WorkPackage = featureId.WorkPackage;
                newEvent.IsCanceled = false;
                newEvent.Version = CreateNewVersion(newEvent, Interpretation.PermanentDelta);
                newEvent.LifeTimeEnd = airacDate;
                newEvent.LifeTimeBegin = myEvents.First(t => t.LifeTimeBegin != null)?.LifeTimeBegin;

                ApplyTimeSlice(newEvent);

                //it should be in current wp anyway
                newEvent.WorkPackage = featureId.WorkPackage;
                _logger.Debug("New event has been created.");
                _logger.Trace(newEvent.Dump);

                CommitEvent(newEvent);
            }
            _logger.Debug("Feature has been decommisioned");
            return true;
        }

        private void CreateStates(PublicSlot publicSlot)
        {
            foreach (var item in Enum.GetValues(typeof(FeatureType)))
            {

                var stateList = GetActualDataByFeatureType(new FeatureId
                {
                    FeatureTypeId = (int)item
                }, false, publicSlot.EffectiveDate);

                foreach (var state in stateList)
                {
                    _abstractStateStorage.ClearStatesAfter(state, publicSlot.EffectiveDate);
                    _abstractStateStorage.SaveStateInternal(state);
                }
            }
        }

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public int GetFeatureTypeById(Guid id)
        {
            return AbstractEventStorage.GetFeatureTypeById(id);
        }


        public DateTime GetServerTime()
        {
            return DateTime.Now;
        }

        public SystemType GetServerType()
        {
            try
            {
                return ServerSettings.Settings.System;
            }
            catch
            {
                return SystemType.Test;
            }
        }
    }
}