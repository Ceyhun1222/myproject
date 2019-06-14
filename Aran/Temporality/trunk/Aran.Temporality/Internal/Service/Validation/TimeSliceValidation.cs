using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Enum;
using NHibernate.Event;

namespace Aran.Temporality.Internal.Service.Validation
{
    internal class TimeSliceValidation : ValidationBase
    {
        public TimeSliceValidation(AimTemporalityService aimTemporalityService) : base(aimTemporalityService)
        {
        }


        public void IsInterpretationCorrect(AbstractEvent<AimFeature> abstractEvent)
        {
            if (abstractEvent.Interpretation != Interpretation.PermanentDelta && abstractEvent.Interpretation != Interpretation.TempDelta)
                Error("Interpretation can be only Permanent or Temporary");
        }

        public void IsTimeSliceCorrect(AbstractEvent<AimFeature> abstractEvent, bool decomission = false)
        {
            IsNull(abstractEvent);
            IsTimeSliceNull(abstractEvent);
            IsVersionNull(abstractEvent);
            IsVersionCorrect(abstractEvent);
            IsFeatureLifeTimeBeginSet(abstractEvent);
            IsInterpretationCorrect(abstractEvent);
            IsValidTimeCorrect(abstractEvent);
            if (decomission) IsFeatureLifeTimeEndCorrect(abstractEvent);
        }

        private void IsFeatureLifeTimeBeginSet(AbstractEvent<AimFeature> abstractEvent)
        {
            if (!abstractEvent.LifeTimeBeginSet || abstractEvent.LifeTimeBegin == default(DateTime))
                Error("Begin of feature life time can't be null");
        }

        private void IsFeatureLifeTimeEndCorrect(AbstractEvent<AimFeature> abstractEvent)
        {
            IsFeatureLifeTimeEndSet(abstractEvent);
            if (abstractEvent.LifeTimeEnd <= abstractEvent.LifeTimeBegin)
                Error("End of feature life time can't be less or equal to begin of feature life time");
        }

        private void IsFeatureLifeTimeEndSet(AbstractEvent<AimFeature> abstractEvent)
        {
            if (!abstractEvent.LifeTimeEndSet || abstractEvent.LifeTimeEnd == default(DateTime))
                Error("End of feature life time can't be null");
        }


        private void IsValidTimeCorrect(AbstractEvent<AimFeature> abstractEvent)
        {
            if (abstractEvent.TimeSlice.BeginPosition == default(DateTime))
                Error("Begin of valid time can't be null");
            switch (abstractEvent.Interpretation)
            {
                case Interpretation.PermanentDelta:
                    IsPermDeltaValidTimeCorrect(abstractEvent);
                    break;
                case Interpretation.TempDelta:
                    IsTempDeltaValidTimeCorrect(abstractEvent);
                    break;
            }
        }

        private void IsPermDeltaValidTimeCorrect(AbstractEvent<AimFeature> abstractEvent)
        {

            if (!(abstractEvent.TimeSlice.EndPosition == null || abstractEvent.TimeSlice.EndPosition == default(DateTime)))
                Error("End of valid time can't be set for PermDelta");
        }

        private void IsTempDeltaValidTimeCorrect(AbstractEvent<AimFeature> abstractEvent)
        {
            if (abstractEvent.TimeSlice.EndPosition == null || abstractEvent.TimeSlice.EndPosition == default(DateTime))
                Error("End of valid time can't be null for TempDelta");
            if (abstractEvent.TimeSlice.EndPosition <= abstractEvent.TimeSlice.BeginPosition)
                Error("End of valid time should be greater than begin of valid time for TempDelta");
        }

        private void IsNull(AbstractEvent<AimFeature> abstractEvent)
        {
            if (abstractEvent == null)
                Error("Event can't be null.");
        }

        private void IsTimeSliceNull(AbstractEvent<AimFeature> abstractEvent)
        {
            if (abstractEvent.TimeSlice == null)
                Error("TimeSlice can't be null.");
        }

        private void IsVersionNull(AbstractEvent<AimFeature> abstractEvent)
        {
            if (abstractEvent.Version == null)
                Error("Version can't be null.");
        }

        public void IsVersionCorrect(AbstractEvent<AimFeature> abstractEvent)
        {
            if (abstractEvent.Version.SequenceNumber < 0)
                Error("Sequence number should be greater than 0.");
            if (abstractEvent.Version.CorrectionNumber < -1)
                Error("CorrectionNumber number should be greater than -1.");

        }
    }
}
