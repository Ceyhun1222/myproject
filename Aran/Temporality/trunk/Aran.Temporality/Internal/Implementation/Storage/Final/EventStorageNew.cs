#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.Common.MetaData;
using Aran.Temporality.Common.OperationResult;
using Aran.Temporality.Common.Util;
using Aran.Temporality.Internal.Abstract.Repository;
using Aran.Temporality.Internal.Abstract.Storage.Cached;
using Aran.Temporality.Internal.MetaData.Offset;
using Aran.Temporality.Internal.MetaData.Util;
using Aran.Temporality.Internal.Util;

#endregion

namespace Aran.Temporality.Internal.Implementation.Storage.Final
{
    internal abstract class EventStorageNew<TEventKeyType, TMetaKeyType> : CachedEventStorageNew<TEventKeyType, TMetaKeyType>
    {

        #region Feature dependency

        //feature dependency check is based on the following state:
        //if main feature A has link to subfeature B then if A actual B should be actual as well
        private void CheckFeatureDependency(IList<FeatureDependencyException> list,
            IFeatureId mainFeatureId, TimeSlice mainLifeTime,
            IFeatureId featureId, TimeSlice lifeTime)
        {
            if (lifeTime.BeginPosition > mainLifeTime.BeginPosition)
            {
                list.Add(new FeatureDependencyException((FeatureType)mainFeatureId.FeatureTypeId + ": " + mainFeatureId.Guid +
                                                    " begins its life at " +
                                                    mainLifeTime.BeginPosition.ToShortDateString() +
                                                    " and has link to " +
                                                    (FeatureType)featureId.FeatureTypeId + ": " + featureId.Guid +
                                                    " which life is started later at " +
                                                    lifeTime.BeginPosition.ToShortDateString())
                {
                    SourceFeatureId = new FeatureId(mainFeatureId),
                    TargetFeatureId = new FeatureId(featureId)
                });
            }
            if (lifeTime.EndPosition != null)
            {
                if (mainLifeTime.EndPosition != null || mainLifeTime.EndPosition > lifeTime.EndPosition)
                {
#warning implement it
                    //TODO: get data actual after end of lifetime.EndPosition
                    //if link is still exist - notify that link should be removed

                }

            }
        }

        TimeSlice GetLifeTime(IFeatureId id)
        {
            if (id.WorkPackage == 0)
            {
                var defaultMeta = MetaCacheActual.GetByFeatureId(id);
                var begin = defaultMeta.LifeTimeBegin;
                var end = defaultMeta.LifeTimeEnd;

                return begin == null ? null : new TimeSlice((DateTime)begin, end);
            }
            else
            {
                var meta = MetaCacheActual.GetByFeatureId(id);
                var defaultMeta = MetaCacheActual.GetByFeatureId(new FeatureId(id) { WorkPackage = 0 });
                var begin = defaultMeta.LifeTimeBegin;
                var end = defaultMeta.LifeTimeEnd;

                if (meta.LifeTimeBeginSet)
                {
                    begin = meta.LifeTimeBegin;
                }

                if (meta.LifeTimeEndSet)
                {
                    end = meta.LifeTimeEnd;
                }

                return begin == null ? null : new TimeSlice((DateTime)begin, end);
            }
        }

        private IEnumerable<FeatureDependencyException> CheckReverseRelations(AbstractEventMetaData myEvent, IEnumerable<FeatureId> featuresLinkedToCurrent)
        {

            var lifeTime = GetLifeTime(myEvent);
            if (lifeTime == null)
            {
                if (myEvent.LifeTimeBegin == null) throw new Exception("feature lifetime should not be null for a first event");
                lifeTime = new TimeSlice((DateTime)myEvent.LifeTimeBegin, myEvent.LifeTimeEnd);
            }

            var list = new List<FeatureDependencyException>();

            foreach (var mainFeatureId in featuresLinkedToCurrent)
            {
                CheckFeatureDependency(list,
                    mainFeatureId,
                    GetLifeTime(mainFeatureId),
                    myEvent,
                    lifeTime);
            }

            return list;
        }

        private List<FeatureDependencyException> CheckRelatedFeatures(AbstractEvent<AimFeature> myEvent, IEnumerable<KeyValuePair<int, List<IFeatureId>>> relatedFeatures)
        {
            var mainLifeTime = GetLifeTime(myEvent);

            if (mainLifeTime == null)
            {
                if (myEvent.LifeTimeBegin == null) throw new Exception("feature lifetime should not be null for a first event");
                mainLifeTime = new TimeSlice((DateTime)myEvent.LifeTimeBegin, myEvent.LifeTimeEnd);
            }

            var list = new List<FeatureDependencyException>();

            foreach (var featureId in relatedFeatures.SelectMany(pair => pair.Value))
            {
                CheckFeatureDependency(list, myEvent, mainLifeTime, featureId, GetLifeTime(featureId));
            }

            return list;
        }

        public IList<FeatureId> GetReverseRelations(IFeatureId id, DateTime actualDate)
        {
            var result = new SortedSet<FeatureId>();

            var metaPackage = MetaCacheActual.GetByWorkPackage(id.WorkPackage);

            foreach (var dict in metaPackage.Values)
            {
                foreach (var meta in dict.Values)
                {
                    foreach (var myEvent in meta.Where(metaData => metaData.SubmitDate <= actualDate &&    //prior events
                                            metaData.RelatedFeatures.Values.Any(             //having link to 
                                                relatedList => relatedList.Any(              //specified featureId
                                                    t => t.WorkPackage == id.WorkPackage && t.Guid == id.Guid))))
                    {
                        result.Add(new FeatureId(myEvent));
                    }

                }
            }

            return result.ToList();
        }

        #endregion

        #region Events repository

        private readonly ILogger _logger = LogManager.GetLogger(typeof(EventStorageNew<TEventKeyType, TMetaKeyType>));

        #endregion

        #region Overrides of AbstractEventStorage<AimFeature>

        public override void SaveEventInternal(AbstractEvent<AimFeature> myEvent)
        {
            _logger.Debug("Saving event to storage.");
            if (myEvent.TimeSlice == null)
            {
                throw new Exception("Feature lifetime can not be null");
            }


            if (myEvent.LifeTimeBeginSet)
            {
                if (myEvent.TimeSlice != null && myEvent.TimeSlice.BeginPosition < myEvent.LifeTimeBegin)
                {
                    throw new Exception("Feature lifetime can not be started after event's actual date");
                }
            }



            var relatedFeatures = myEvent.GetRelatedFeatures();


#warning Implement feature dependency
            //TODO: implement it

            //for now we skip the check for FeatureDependency
            //if (workPackageIsSafe && false)
            //{

            //    var list=CheckRelatedFeatures(myEvent, relatedFeatures);
            //    list.AddRange(CheckReverseRelations(myEvent, GetReverseRelations(myEvent, myEvent.TimeSlice.BeginPosition)));

            //    if (list.Count > 0)
            //    {
            //        throw new FeatureDependencyListException("Feature dependency violated, see list for details") { ExceptionList = list };
            //    }
            //}

            _logger.Debug("Saving event to storage");
            var meta = new OffsetEventMetaData<TEventKeyType>(myEvent)
            {
                Offset = Repository(myEvent.WorkPackage).Add(new AimEvent(myEvent)),
                RelatedFeatures = relatedFeatures
            };

            AddEventMetaData(meta);

            _logger.Debug("Event has been saved to storage.");

        }

        public override IList<Guid> GetFilteredIds(IFeatureId featureId, Filter filter, bool slotOnly, IList<Guid> ids = null)
        {
            var result = Repository(featureId.WorkPackage).GetFilteredId(filter, featureId.FeatureTypeId, ids);
            if (featureId.WorkPackage != 0 && !slotOnly)
                result = result.Union(Repository(0).GetFilteredId(filter, featureId.FeatureTypeId, ids)).ToList();
            return result;
        }

        public override IList<AbstractEvent<AimFeature>> GetEventsAffectingDateInterval(IFeatureId featureId, DateTime fromDate, DateTime? toDate, bool andOneMore = false, Projection projection = null)
        {
            EventMetaSet<OffsetEventMetaData<TEventKeyType>> metaset = MetaCacheActual.GetByFeatureId(featureId);
            ICollection<OffsetEventMetaData<TEventKeyType>> subset = metaset.GetViewBetween(fromDate, toDate, andOneMore);
            //manipulation with subset here

            //if there is no Data return empty list
            if (subset.Count == 0) return new List<AbstractEvent<AimFeature>>();

            subset = subset.OrderBy(t => t.Offset).ToList(); //for better performance

            AbstractAimRepository<TEventKeyType> wpEvents = Repository(featureId.WorkPackage);

            var datas = wpEvents?.Get(subset.Select(x => x.Offset).ToList(), featureId.FeatureTypeId, projection);
            return datas?.Cast<AbstractEvent<AimFeature>>().ToList() ?? new List<AbstractEvent<AimFeature>>();

            //return (from metaData in subset
            //        let data = wpEvents.Get(metaData.Offset)
            //        select GetEventFromData(data, metaData)).ToList();
        }

        public override void Dispose()
        {
        }


        public override AbstractEvent<AimFeature> GetEventByVersion(IFeatureId featureId, TimeSliceVersion version)
        {
            var metaset = MetaCacheActual.GetByFeatureId(featureId);

            //it does not contain IsCanceled because it is MetaCacheActual
            //var data = metaset.Where(t => t.IsCanceled == false &&
            //                              t.Version.SequenceNumber == version.SequenceNumber).ToList();

            var data = metaset.Where(t => t.Version.SequenceNumber == version.SequenceNumber).ToList();
            if (data.Count == 0) return null;

            var metaitem = data.First();
            return GetEventFromData(Repository(featureId.WorkPackage).Get(metaitem.Offset).Data, metaitem);
        }


        public override void Truncate()
        {
            base.Truncate();
            MetaCacheActual.Clear();
            MetaCacheCancelled.Clear();
            FileUtil.DeleteDirectory(StorageName + "\\" + "events");
        }

        #endregion

        #region Overrides of AbstractEventStorage<Feature>


        public override bool CanCommitWorkPackage(List<int> workPackageIds)
        {
            var result = true;

            //var defaultWorkPackageMeta=MetaCacheActual.GetByWorkPackage(0);
            //var currentWorkPackageMeta=MetaCacheActual.GetByWorkPackage(workpackage);
            //var currentCancelledWorkPackageMeta = MetaCacheCancelled.GetByWorkPackage(workpackage);

            //foreach (var pair in currentWorkPackageMeta)
            //{
            //    var featureType = pair.Key;

            //    SortedList<Guid, EventMetaSet<OffsetEventMetaData<TEventKeyType>>> defaultFeatureTypeData=null;
            //    if (defaultWorkPackageMeta!=null)
            //    if (!defaultWorkPackageMeta.TryGetValue(featureType, out defaultFeatureTypeData))
            //    {
            //        defaultFeatureTypeData = null;//may be we should init with instance?
            //    }

            //    SortedList<Guid, EventMetaSet<OffsetEventMetaData<TEventKeyType>>> cancelledFeatureTypeData=null;
            //    if (currentCancelledWorkPackageMeta!=null)
            //    if (!currentCancelledWorkPackageMeta.TryGetValue(featureType, out cancelledFeatureTypeData))
            //    {
            //        cancelledFeatureTypeData = null;//may be we should init with instance?
            //    }


            //    foreach (var pair2 in pair.Value)
            //    {
            //        var featureId = pair2.Key;
            //        var currentEventMetaSet=pair2.Value;

            //        //get similar data from default workpackage
            //        EventMetaSet<OffsetEventMetaData<TEventKeyType>> defaultEventMetaSet = null;
            //        if (defaultFeatureTypeData!=null)
            //        {
            //            if(!defaultFeatureTypeData.TryGetValue(featureId, out defaultEventMetaSet))
            //            {
            //                defaultEventMetaSet = null;
            //            }
            //        }

            //        if (defaultEventMetaSet == null)
            //        {
            //            //this feature is absent in default package, so no conflicts possible
            //            continue;
            //        }


            //        var cancelCommission = defaultEventMetaSet.Values.Where(t => t.Value.IsCancelCommission).Any();
            //        if (cancelCommission)
            //        {
            //            ReportFeatureCommisionWasCancelled(featureType, featureId);
            //            result = false;
            //            continue;
            //        }

            //        var decommisionList=defaultEventMetaSet.Values.Where(t => t.Value.IsDecommission).ToList();
            //        if (decommisionList.Count>0)
            //        {
            //            foreach (var valuePair in decommisionList)
            //            {
            //                if (!currentEventMetaSet.Where(t => t.Version.SequenceNumber == valuePair.Value.Version.SequenceNumber
            //                    && t.Version.CorrectionNumber == valuePair.Value.Version.CorrectionNumber+1).Any())
            //                {
            //                    ReportDecommisionWithoutCancellation(featureType, featureId);
            //                    result = false;
            //                    continue;
            //                }
            //            }
            //        }

            //        var hasFirst=currentEventMetaSet.Values.Where(t => t.Value.Version.SequenceNumber==1).Any();
            //        if (hasFirst)
            //        {
            //            //this feature was committed in workpakage, but the same feature is present in default workpackage
            //            ReportSameFeatureWasAlreadyCommited(featureType,featureId);
            //            result = false;
            //            continue;
            //        }

            //        //get similar cancelled data
            //        EventMetaSet<OffsetEventMetaData<TEventKeyType>> cancelledEventMetaSet = null;
            //        if (cancelledFeatureTypeData != null)
            //        {
            //            if (!cancelledFeatureTypeData.TryGetValue(featureId, out cancelledEventMetaSet))
            //            {
            //                cancelledEventMetaSet = null;
            //            }
            //        }

            //        var currentCorrections = 
            //            currentEventMetaSet.Values.Where(t => t.Value.Version.CorrectionNumber > 0).
            //            Select(t=>t.Value.Version).ToList();

            //        var defaultCorrections =
            //            defaultEventMetaSet.Values.Where(t => t.Value.Version.CorrectionNumber > 0).
            //            Select(t => t.Value.Version).ToList();

            //        var intersection=currentCorrections.Intersect(defaultCorrections).ToList();
            //        var problems = (cancelledEventMetaSet==null)?intersection: 
            //            intersection.Where(version => !cancelledEventMetaSet.Where(
            //                t => t.Version.SequenceNumber == version.SequenceNumber && 
            //                    t.Version.CorrectionNumber == 0).Any()).ToList();

            //        if (problems.Count > 0)
            //        {
            //            ReportSameCorrectionNumbers(problems, featureId, intersection);
            //            result = false;
            //            continue;
            //        }

            //    }
            //}

            return result;
        }

        public override bool CommitWorkPackage(int workpackage)
        {
            LoadMeta(workpackage);

            int actualMetaCount = 0;
            int cancelledMetaCount = 0;


            _logger.Info($"Comminting workpackage {workpackage}.");

            Stopwatch stopwatch = Stopwatch.StartNew();
            var defaultActualWorkPackageMeta = MetaCacheActual.GetByWorkPackage(0);
            var defaultCancelledWorkPackageMeta = MetaCacheCancelled.GetByWorkPackage(0);

            var currentActualWorkPackageMeta = MetaCacheActual.GetByWorkPackage(workpackage);
            var currentCancelledWorkPackageMeta = MetaCacheCancelled.GetByWorkPackage(workpackage);

            List<OffsetEventMetaData<TEventKeyType>> savedWorkPackageMeta = new List<OffsetEventMetaData<TEventKeyType>>();
            List<OffsetEventMetaData<TEventKeyType>> savedCancelledWorkPackageMeta = new List<OffsetEventMetaData<TEventKeyType>>();

            foreach (var pair in currentActualWorkPackageMeta)
            {

                int actualFeatureMetaCount = 0;
                int cancelledFeatureMetaCount = 0;

                var featureType = pair.Key;
                _logger.Debug($"Comminting feature type. id:{featureType} name:{(FeatureType)featureType}.");
                //read data corresponding to FeatureType

                SortedList<Guid, EventMetaSet<OffsetEventMetaData<TEventKeyType>>> defaultActualFeatureTypeData = null;
                defaultActualWorkPackageMeta?.TryGetValue(featureType, out defaultActualFeatureTypeData);

                SortedList<Guid, EventMetaSet<OffsetEventMetaData<TEventKeyType>>> defaultCancelledFeatureTypeData = null;
                defaultCancelledWorkPackageMeta?.TryGetValue(featureType, out defaultCancelledFeatureTypeData);

                SortedList<Guid, EventMetaSet<OffsetEventMetaData<TEventKeyType>>> currentCancelledFeatureTypeData = null;
                currentCancelledWorkPackageMeta?.TryGetValue(featureType, out currentCancelledFeatureTypeData);

                foreach (var pair2 in pair.Value)
                {
                    var guid = pair2.Key;
                    var currentActualMetaSet = pair2.Value;

                    _logger.Debug($"Comminting feature {guid}.");


                    EventMetaSet<OffsetEventMetaData<TEventKeyType>> currentCancelledMetaSet = null;
                    currentCancelledFeatureTypeData?.TryGetValue(guid, out currentCancelledMetaSet);

                    CommitFeature(new FeatureId { FeatureTypeId = featureType, Guid = guid, WorkPackage = workpackage }, Interpretation.PermanentDelta, currentActualMetaSet, currentCancelledMetaSet, savedWorkPackageMeta, savedCancelledWorkPackageMeta);
                    CommitFeature(new FeatureId { FeatureTypeId = featureType, Guid = guid, WorkPackage = workpackage }, Interpretation.TempDelta, currentActualMetaSet, currentCancelledMetaSet, savedWorkPackageMeta, savedCancelledWorkPackageMeta);

                    _logger.Debug($"{guid} feature's sequences not created in workpackage {workpackage} has been commited.");

                    actualFeatureMetaCount += savedWorkPackageMeta.Count;
                    cancelledFeatureMetaCount += savedCancelledWorkPackageMeta.Count;

                    stopwatch.Stop();
                    _logger.Debug($"Feature {guid} has been committed. " +
                                                                       $"Committed {savedWorkPackageMeta.Count} actual events and " +
                                                                       $"{savedCancelledWorkPackageMeta.Count} non-actual events. " +
                                                                       $"Time: {stopwatch.ElapsedMilliseconds} ms and {stopwatch.ElapsedTicks}.");

                    savedWorkPackageMeta.Clear();
                    savedCancelledWorkPackageMeta.Clear();
                }

                actualMetaCount += actualFeatureMetaCount;
                cancelledMetaCount += cancelledFeatureMetaCount;

                _logger.Debug($"Feature type {(FeatureType)featureType} has been committed. " +
                                                                       $"Committed {actualFeatureMetaCount} actual events and " +
                                                                       $"{cancelledFeatureMetaCount} non-actual events.");
            }

            _logger.Info($"Workpackage {workpackage} has been committed. " +
                                                                      $"Committed {actualMetaCount} actual events and " +
                                                                      $"{cancelledMetaCount} non-actual events.");
            return true;
        }

        private void CommitFeature(IFeatureId featureId, Interpretation interpretation, EventMetaSet<OffsetEventMetaData<TEventKeyType>> currentActualMetaSet,
            EventMetaSet<OffsetEventMetaData<TEventKeyType>> currentCancelledMetaSet, List<OffsetEventMetaData<TEventKeyType>> savedWorkPackageMeta, List<OffsetEventMetaData<TEventKeyType>> savedCancelledWorkPackageMeta)
        {
            //move all data which sequence completelly belongs to current package
            var sequenceStarts = currentActualMetaSet.Where(t => t.Version.CorrectionNumber == 0 && t.Interpretation == interpretation)
                .Select(t => t.Version.SequenceNumber)
                .ToList();
            if (currentCancelledMetaSet != null)
            {
                sequenceStarts.AddRange(currentCancelledMetaSet.Where(t => t.Version.CorrectionNumber == 0 && t.Interpretation == interpretation)
                    .Select(t => t.Version.SequenceNumber));
            }

            sequenceStarts = sequenceStarts.Distinct().OrderBy(f => f).ToList();

            _logger.Debug($"Commiting {featureId.Guid} feature's sequences created in workpackage {featureId.WorkPackage}.");
            foreach (var sequenceStart in sequenceStarts)
            {
                var version = CreateNewVersion(new FeatureId { FeatureTypeId = featureId.FeatureTypeId, Guid = featureId.Guid },
                    interpretation);
                var start = sequenceStart;

                if (currentCancelledMetaSet != null)
                {
                    CommitNewSequence(featureId, interpretation, currentCancelledMetaSet, start, savedCancelledWorkPackageMeta, version);
                }
                CommitNewSequence(featureId, interpretation, currentActualMetaSet, start, savedWorkPackageMeta, version, true);

            }
            _logger.Debug($"{featureId.Guid} feature's sequences created in workpackage {featureId.WorkPackage} has been commited.");


            //save other as is
            _logger.Debug($"Commiting {featureId.Guid} feature's sequences not created in workpackage {featureId.WorkPackage}.");
            if (currentCancelledMetaSet != null)
            {
                CommitCorrections(featureId, interpretation, currentCancelledMetaSet, savedCancelledWorkPackageMeta, sequenceStarts, false);
            }

            CommitCorrections(featureId, interpretation, currentActualMetaSet, savedWorkPackageMeta, sequenceStarts, true);
        }

        private void CommitCorrections(IFeatureId featureId, Interpretation interpretation, EventMetaSet<OffsetEventMetaData<TEventKeyType>> currentMetaSet,
            List<OffsetEventMetaData<TEventKeyType>> savedWorkPackageMeta, List<int> sequenceStarts, bool actual = false)
        {
            string type = actual ? "actual" : "non-actual";
            foreach (var meta in currentMetaSet.Where(
                t => !sequenceStarts.Contains(t.Version.SequenceNumber) && t.Interpretation == interpretation))
            {
                if (!savedWorkPackageMeta.Contains(meta))
                {

                    _logger.Debug($"Commiting {type} feature {featureId.Guid}. Version: {meta.Version}. Interpretation: {interpretation}");
                    MoveMetaToDefaultWorkPackage(meta);
                    savedWorkPackageMeta.Add(meta);
                    _logger.Debug($"Feature {featureId.Guid}  version: {meta.Version}  interpretation: {interpretation} has been committed.");
                }
            }
        }

        private void CommitNewSequence(IFeatureId featureId, Interpretation interpretation, EventMetaSet<OffsetEventMetaData<TEventKeyType>> currentMetaSet,
            int start, List<OffsetEventMetaData<TEventKeyType>> savedWorkPackageMeta, TimeSliceVersion version, bool actual = false)
        {
            string type = actual ? "actual" : "non-actual";
            foreach (var meta in currentMetaSet.Where(
                t => t.Version.SequenceNumber == start && t.Interpretation == interpretation))
            {
                if (!savedWorkPackageMeta.Contains(meta))
                {

                    _logger.Debug($"Commiting {type} feature {featureId.Guid}. Version: {meta.Version}. New Version: {version}. Interpretation: {interpretation}");
                    MoveMetaToDefaultWorkPackage(meta, version.SequenceNumber);
                    savedWorkPackageMeta.Add(meta);
                    _logger.Debug($"Feature {featureId.Guid}  version: {meta.Version}  interpretation: {interpretation} has been committed.");
                }
            }
        }

        protected void MoveMetaToDefaultWorkPackage(OffsetEventMetaData<TEventKeyType> meta, int sequenceStart = 0)
        {
            if (meta == null) return;
            var metaByWorkPackage = Repository(meta.WorkPackage);
            var data = metaByWorkPackage?.Get(meta.Offset);
            if (data == null) return;

            if (sequenceStart != 0) meta.Version.SequenceNumber = sequenceStart;//set new sequence if needed
            meta.WorkPackage = 0;//set default workpackage

            var myEvent = GetEventFromData(data.Data, meta);
            SaveEventInternal(myEvent);
        }




        public override void DeleteWorkPackage(int workpackage)
        {
            //remove event repository
            RemoveRepository(workpackage);

            //remove from cache
            MetaCacheActual.Remove(workpackage);
            MetaCacheCancelled.Remove(workpackage);
        }
        #endregion

        #region Report

        private void ReportSameCorrectionNumbers(List<TimeSliceVersion> problems, Guid featureId, List<TimeSliceVersion> intersection)
        {
            //throw new Exception("ReportSameCorrectionNumbers");
        }

        private void ReportDecommisionWithoutCancellation(int featureType, Guid featureId)
        {
            //throw new Exception("ReportDecommisionWithoutCancellation");
        }

        private void ReportFeatureCommisionWasCancelled(int featureType, Guid featureId)
        {
            //throw new Exception("ReportFeatureCommisionWasCancelled");
        }


        private void ReportSameFeatureWasAlreadyCommited(int featureType, Guid featureId)
        {
            //throw new Exception("ReportSameFeatureWasAlreadyCommited");
        }


        #endregion

        public override IList<AbstractEvent<AimFeature>> GetChangesInIntervalInternal(IFeatureId featureId,
                                                                             DateTime startDate,
                                                                             DateTime endDate)
        {
            if (featureId.WorkPackage != 0)
            {
                LoadMeta(featureId.WorkPackage);
            }

            var byWorkPackage = MetaCacheActual.GetByWorkPackage(featureId.WorkPackage);
            if (byWorkPackage == null) return new List<AbstractEvent<AimFeature>>();


            var metaSets = new List<EventMetaSet<OffsetEventMetaData<TEventKeyType>>>();


            if (featureId.FeatureTypeId == 0 || featureId.FeatureTypeId == -1)
            {
                //add all types
                if (featureId.Guid == null)
                {
                    //add all features
                    foreach (var wpData in byWorkPackage.Values)
                    {
                        metaSets.AddRange(wpData.Values);
                    }
                }
                else
                {
                    //add specific feature
                    foreach (var wpData in byWorkPackage.Values)
                    {
                        EventMetaSet<OffsetEventMetaData<TEventKeyType>> metaSet;
                        if (wpData.TryGetValue((Guid)featureId.Guid, out metaSet))
                        {
                            metaSets.Add(metaSet);
                        }
                    }
                }

            }
            else if (byWorkPackage.ContainsKey(featureId.FeatureTypeId))
            {
                //add specific type
                SortedList<Guid, EventMetaSet<OffsetEventMetaData<TEventKeyType>>> metaParent;
                if (byWorkPackage.TryGetValue(featureId.FeatureTypeId, out metaParent))
                {

                    if (featureId.Guid == null)
                    {
                        //add all features
                        metaSets.AddRange(metaParent.Values);
                    }
                    else
                    {
                        //add specific feature
                        EventMetaSet<OffsetEventMetaData<TEventKeyType>> metaSet;
                        if (metaParent.TryGetValue((Guid)featureId.Guid, out metaSet))
                        {
                            metaSets.Add(metaSet);
                        }
                    }
                }
            }

            if (metaSets.Count == 0) return new List<AbstractEvent<AimFeature>>();

            var result = new List<AbstractEvent<AimFeature>>();

            foreach (var featureMetaSet in metaSets)
            {
                var subset = featureMetaSet.GetViewBetween(startDate, endDate);
                if (subset.Count == 0) continue;
                subset = subset.OrderBy(t => t.Offset).ToList(); //for better performance

                var datas = Repository(featureId.WorkPackage).Get(subset.Select(metaData => metaData.Offset).ToList());
                var events = datas?.Cast<AbstractEvent<AimFeature>>().ToList() ?? new List<AbstractEvent<AimFeature>>();

                //var events =
                //    subset.Select(
                //        metaData =>
                //        GetEventFromData(Events(featureId.WorkPackage).Get(metaData.Offset), metaData)).
                //        ToList();
                result.AddRange(events);
            }


            return result;

        }


        public override IList<AbstractEventMetaData> GetChangesMetaInIntervalInternal(IFeatureId featureId,
                                                                             DateTime startDate,
                                                                             DateTime endDate)
        {
            if (featureId.WorkPackage != 0)
            {
                LoadMeta(featureId.WorkPackage);
            }
            var byWorkPackage = MetaCacheActual.GetByWorkPackage(featureId.WorkPackage);
            if (byWorkPackage == null) return new List<AbstractEventMetaData>();

            var featureTypeValues = new List<SortedList<Guid, EventMetaSet<OffsetEventMetaData<TEventKeyType>>>>();
            if (featureId.FeatureTypeId == 0 || featureId.FeatureTypeId == -1)
            {
                //add all
                featureTypeValues.AddRange(byWorkPackage.Values);
            }
            else if (byWorkPackage.ContainsKey(featureId.FeatureTypeId))
            {
                //add specified
                featureTypeValues.Add(byWorkPackage[featureId.FeatureTypeId]);
            }

            if (featureTypeValues.Count == 0) return new List<AbstractEventMetaData>();

            var result = new List<AbstractEventMetaData>();

            foreach (var featureTypeMetaSet in featureTypeValues)
            {
                foreach (var featureMetaSet in featureTypeMetaSet.Values)
                {
                    ICollection<OffsetEventMetaData<TEventKeyType>> subset = featureMetaSet.GetViewBetween(startDate, endDate);
                    result.AddRange(subset);
                }
            }

            return result;
        }
    }
}