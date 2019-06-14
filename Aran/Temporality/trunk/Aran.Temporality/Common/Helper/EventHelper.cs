using System;
using System.Collections.Generic;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.MetaData;
using Aran.Temporality.Common.OperationResult;

namespace Aran.Temporality.Common.Helper
{
    public class EventHelper
    {
        private readonly ITemporalityService<AimFeature> _service;

        public EventHelper(ITemporalityService<AimFeature> service)
        {
            _service = service;
        }


        //cancel
        public CommonOperationResult CancelSequence(TimeSliceId myEvent, Interpretation interpretation, DateTime? cancelDate = null)
        {
            return _service.CancelSequence(myEvent, interpretation, cancelDate);
        }

        //commit
        //public CommonOperationResult CommitEvent(AbstractEvent<AimFeature> myEvent)
        //{
        //    return _service.CommitEvent(myEvent);
        //}

        public FeatureOperationResult CommitNewEvent(AbstractEvent<AimFeature> myEvent)
        {
            return _service.CommitNewEvent(myEvent);
        }

        //correct
        public FeatureOperationResult CommitCorrection(AbstractEvent<AimFeature> myEvent)
        {
            return _service.CommitCorrection(myEvent);
        }


        //read
        public AbstractEvent<AimFeature> GetEvent(IFeatureId featureId, TimeSliceVersion version)
        {
            return _service.GetEvent(featureId,version);
        }

        public IList<AbstractEventMetaData> GetActualEventMeta(IFeatureId featureId, 
            TimeSlice impactInterval, 
            TimeSlice submitInterval)
        {
            return _service.GetActualEventMeta(featureId, impactInterval, submitInterval);
        }

        public IList<AbstractEventMetaData> GetCancelledEventMeta(IFeatureId featureId, 
            TimeSlice impactInterval, 
            TimeSlice submitInterval)
        {
            return _service.GetCancelledEventMeta(featureId, impactInterval, submitInterval);
        }

    }
}
