#region

using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Abstract.State;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.ArcGis;
using Aran.Temporality.Common.Entity.Enum;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Extensions;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.Internal.Abstract;
using Aran.Temporality.Internal.Abstract.Storage;
using Aran.Temporality.Internal.Attribute;
using Aran.Temporality.Internal.MetaData;
using Aran.Temporality.Internal.MetaStorage;
using Aran.Temporality.Internal.Util;
using Aran.Temporality.Internal.WorkFlow;
using ESRI.ArcGIS.Geometry;

#endregion

namespace Aran.Temporality.Internal.Service
{
    //public class DummyFeature : Feature
    //{
    //    protected override void Pack(PackageWriter writer)
    //    {
    //        if (TimeSlice == null) TimeSlice = new Aim.DataTypes.TimeSlice();

    //        if (TimeSlice != null)
    //        {
    //            var aimPropVal = GetValue((int)PropertyFeature.TimeSlice);
    //            ((IPackable)aimPropVal).Pack(writer);
    //        }

    //        writer.PutString(Identifier.ToString());
    //    }

    //    protected override void Unpack(PackageReader reader)
    //    {
    //        TimeSlice = new Aim.DataTypes.TimeSlice();
    //        (TimeSlice as IPackable).Unpack(reader);
    //        var s = reader.GetString();
    //        Identifier = Guid.Parse(s);
    //    }

    //    #region Overrides of Feature

    //    public override FeatureType FeatureType
    //    {
    //        get { return FeatureType.UnknownFeature; }
    //    }

    //    #endregion
    //}

    internal class AimTemporalityService : AbstractTemporalityService<AimFeature>
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(AimTemporalityService));


        public AimTemporalityService(String path) : base(path)
        {
        }

        public override void ApplyTimeSlice(AbstractEvent<AimFeature> abstractEvent)
        {
            if (abstractEvent?.Version == null) return;

            if (abstractEvent.Guid != null)
            {
                var id = (Guid)abstractEvent.Guid;

                if (abstractEvent.Data == null)
                {
                    abstractEvent.Data = new AimFeature {Identifier = id};
                }
                if (abstractEvent.Data.Feature == null)
                { 
                    var f=(Feature)AimObjectFactory.Create(abstractEvent.FeatureTypeId);
                    f.Identifier = id;
                    abstractEvent.Data.Feature = f;
                }
            }
            if (abstractEvent.Data.Feature.TimeSlice == null) abstractEvent.Data.Feature.TimeSlice=new TimeSlice();
            if (abstractEvent.Data.Feature.TimeSlice.ValidTime == null) abstractEvent.Data.Feature.TimeSlice.ValidTime=new TimePeriod();
            if (abstractEvent.Data.Feature.TimeSlice.FeatureLifetime == null) abstractEvent.Data.Feature.TimeSlice.FeatureLifetime = new TimePeriod();
          
            abstractEvent.Data.Feature.TimeSlice.SequenceNumber = abstractEvent.Version.SequenceNumber;
            abstractEvent.Data.Feature.TimeSlice.CorrectionNumber = abstractEvent.Version.CorrectionNumber;

            abstractEvent.Data.Feature.TimeSlice.ValidTime.BeginPosition = abstractEvent.TimeSlice.BeginPosition;
            abstractEvent.Data.Feature.TimeSlice.ValidTime.EndPosition = abstractEvent.TimeSlice.EndPosition;

            if (abstractEvent.LifeTimeBeginSet)
            {
                abstractEvent.Data.Feature.TimeSlice.FeatureLifetime.BeginPosition = (DateTime)abstractEvent.LifeTimeBegin;
            }

            if (abstractEvent.LifeTimeEndSet)
            {
                abstractEvent.Data.Feature.TimeSlice.FeatureLifetime.EndPosition = abstractEvent.LifeTimeEnd;
            }


        }

        public override void ApplyTimeSlice(AbstractState<AimFeature> state)
        {
            if (state?.Version == null) return;
            if (state.Data?.Feature == null) return;
            
            if (state.Data.Feature.TimeSlice == null)
            {
                state.Data.Feature.TimeSlice = new TimeSlice();
            }
            if (state.Data.Feature.TimeSlice.ValidTime == null)
            {
               state.Data.Feature.TimeSlice.ValidTime=new TimePeriod();
            }
            
            state.Data.Feature.TimeSlice.SequenceNumber = state.Version.SequenceNumber;
            state.Data.Feature.TimeSlice.CorrectionNumber = state.Version.CorrectionNumber;

            state.Data.Feature.TimeSlice.ValidTime.BeginPosition = state.TimeSlice.BeginPosition;
            state.Data.Feature.TimeSlice.ValidTime.EndPosition = state.TimeSlice.EndPosition;

            //state.Data.Feature.TimeSlice.Interpretation =
            //    state.Data.Feature.TimeSlice.Interpretation == TimeSliceInterpretationType.TEMPDELTA
            //        ? TimeSliceInterpretationType.TEMPDELTA
            //        : TimeSliceInterpretationType.BASELINE;

        }

        public override void ApplyInterpretation(AbstractState<AimFeature> state, Interpretation tossInterpretation)
        {
            switch (tossInterpretation)
            {
                case Interpretation.Snapshot:
                    state.Data.Feature.TimeSlice.Interpretation = TimeSliceInterpretationType.SNAPSHOT;
                    break;
                case Interpretation.PermanentDelta:
                    state.Data.Feature.TimeSlice.Interpretation = TimeSliceInterpretationType.PERMDELTA;
                    break;
                case Interpretation.TempDelta:
                    state.Data.Feature.TimeSlice.Interpretation = TimeSliceInterpretationType.TEMPDELTA;
                    break;
                case Interpretation.BaseLine:
                    state.Data.Feature.TimeSlice.Interpretation = TimeSliceInterpretationType.BASELINE;
                    break;
            }
        }

        public override bool DoWorkflow()
        {
            return WorkFlowImplementation.DoWorkflow(this);
        }

        public override bool IsGeoIntersect(AbstractState<AimFeature> state, IGeometry geo, double distance)
        {
            var aimState = state as AimState;
            if (aimState?.Data == null) return false;

            GeometryFormatter.PrepareGeometry(aimState.Data);
            return GeometryFormatter.HasIntersection(geo, aimState.Data);
        }

        #region Overrides of Storage<DummyFeature>

        public override AbstractEvent<AimFeature> FormCancelSequence(TimeSliceId timeSliceId, Interpretation interpretation, DateTime? cancelDate = null)
        {
            _logger.Trace("Forming cancel event.");

            var meta = new EventMetaData();
            meta.InitFromFeatureId(timeSliceId);

            var effectiveDate = cancelDate ?? DateTime.Now;

            //set lifetime according to deletetime
            meta.LifeTimeBegin = effectiveDate;
            meta.TimeSlice = new Common.MetaData.TimeSlice(effectiveDate);

            //we do not care about correction number
            meta.Version = new TimeSliceVersion(timeSliceId.Version) { CorrectionNumber = -1 };
            meta.IsCanceled = true;

            meta.TimeSlice = new Common.MetaData.TimeSlice(DateTime.Now);
            meta.Interpretation = interpretation;
            //build event from empty data
            var myEvent = AbstractEventStorage.GetEventFromData(new AimFeature(timeSliceId), meta);
            myEvent.TimeSlice = new Common.MetaData.TimeSlice(effectiveDate);

            _logger.Trace("Cancel event has been formed.");
            _logger.Trace(() => myEvent.Dump());
            return myEvent;
        }


        internal override AbstractEventStorage<AimFeature> AbstractEventStorageInternal { get; } = StorageManager<AimFeature>.GetAbstractEventStorage();

        internal override AbstractStateStorage<AimFeature> AbstractStateStorageInternal { get; } = new AimStateStorage();

        #endregion


        #region Override AbstractTemporalityService

        [SecureOperation(DataOperation.ReadData)]
        public override IList<AbstractState<AimFeature>> GetStatesInRangeByInterpretation(IFeatureId featureId, bool slotOnly, DateTime dateTimeStart,
            DateTime dateTimeEnd, Interpretation interpretation)
        {
            var result = base.GetStatesInRangeByInterpretation(featureId, slotOnly, dateTimeStart, dateTimeEnd, interpretation);
            if (interpretation == Interpretation.TempDelta)
                return result.Where(t => t.Data?.Feature?.TimeSlice?.Interpretation == TimeSliceInterpretationType.TEMPDELTA)
                    .ToList();
            return result;
        }
        

        #endregion

    }
}