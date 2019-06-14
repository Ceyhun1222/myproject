using System;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Interface;

namespace Aran.Temporality.Internal.Service.Validation
{
    internal class TemporalityValidation : ValidationBase
    {
        public TemporalityValidation(AimTemporalityService aimTemporalityService) : base(aimTemporalityService)
        {
        }


        public void IsCorrectionValid(AbstractEvent<AimFeature> abstractEvent)
        {
            //Temporary commented
            //  IsCorrectionVersionValid(abstractEvent);
            var correction = AimTemporalityService.GetLastCorrection(abstractEvent);
            if (correction == null)
                Error("Previous version of event was not found while performing correction");

            IsLifeTimesEqual(abstractEvent, correction);
            IsInterpretationsEqual(abstractEvent, correction);
            IsValidTimeBeginsEqual(abstractEvent, correction);
            if (abstractEvent.Interpretation == Interpretation.TempDelta)
            {
                IsValidTimeEndsEqual(abstractEvent, correction);
            }
        }

        public void IsNewEventValid(AbstractEvent<AimFeature> abstractEvent)
        {

            
            //IsEffectiveDateCoincide(abstractEvent); temporary commented due to customer's desire 
            if (abstractEvent.Version.SequenceNumber == 1)
            {
                //Temporary commented
                // IsComissionVersionValid(abstractEvent);
                // CommissionValidation(abstractEvent);

            }
            else
            {
                IsNewSequenceVersionValid(abstractEvent);
                IsComissioned(abstractEvent);
                //var commission = AimTemporalityService.GetCommission(abstractEvent);
                //if (commission == null)
                //    Error("Commission version of event was not found");
                //IsLifeTimesEqual(abstractEvent, commission);
            }
        }


        public void IsEffectiveDateCoincide(AbstractEvent<AimFeature> abstractEvent)
        {
            var slot = AimTemporalityService.GetPrivateSlotById(abstractEvent.WorkPackage);
            if (slot.PublicSlot.SlotType == 0)
            {
                if (abstractEvent.Interpretation == Interpretation.TempDelta)
                {
                    if (abstractEvent.TimeSlice.BeginPosition < slot.PublicSlot.EffectiveDate)
                        Error("Begin of valid time is less than slot effective date");
                }
                if (abstractEvent.Interpretation == Interpretation.PermanentDelta)
                {
                    if (abstractEvent.TimeSlice.BeginPosition != slot.PublicSlot.EffectiveDate)
                        Error("Begin of valid is not equal to slot effective date");
                }
            }
        }

        public void IsDecomissionValid(IFeatureId meta)
        {
            IsComissioned(meta);
        }

        public void IsCancelValid(IFeatureId meta)
        {
            IsComissioned(meta);
        }

        private void IsValidTimeEndsEqual(AbstractEvent<AimFeature> event1, AbstractEventMetaData event2)
        {
            if (event2.TimeSlice.EndPosition != event1.TimeSlice.EndPosition)
                Error("ValidTime End is not correct");
        }

        private void IsValidTimeBeginsEqual(AbstractEvent<AimFeature> event1, AbstractEventMetaData event2)
        {
            if (event2.TimeSlice.BeginPosition != event1.TimeSlice.BeginPosition)
                Error("ValidTime Begin is not correct");
        }

        private void IsInterpretationsEqual(AbstractEvent<AimFeature> event1, AbstractEventMetaData event2)
        {
            if (event2.Interpretation != event1.Interpretation)
                Error("Interpretation is not correct");
        }

        private void IsLifeTimesEqual(AbstractEvent<AimFeature> event1, AbstractEventMetaData event2)
        {
            if (event2.LifeTimeBegin != event1.LifeTimeBegin)
                Error("LifeTime Begin is not correct");
            if (event2.LifeTimeEndSet != event1.LifeTimeEndSet)
                Error("LifeTime End is not correct");
            if (event2.LifeTimeEnd != event1.LifeTimeEnd)
                Error("LifeTime End is not correct");
        }

        private void IsCorrectionVersionValid(AbstractEvent<AimFeature> abstractEvent)
        {
            if (abstractEvent.Version.CorrectionNumber == 0)
                Error("Correction number can't be 0 for this method, try CommitNewEvent");

        }

        private void IsEventNew(AbstractEvent<AimFeature> abstractEvent)
        {
            if (abstractEvent.Version.CorrectionNumber != 0)
                Error("Correction number should be 0 for this method, try CommitCorrection");
        }


        private void IsComissionVersionValid(AbstractEvent<AimFeature> abstractEvent)
        {
            if (abstractEvent.Version.SequenceNumber != 1 || abstractEvent.Version.CorrectionNumber != 0)
                Error("Sequence and correction numbers should be 1 and 0  for this method");
        }

        private void IsNewSequenceVersionValid(AbstractEvent<AimFeature> abstractEvent)
        {
            if (abstractEvent.Version.CorrectionNumber != 0)
                Error("Correction numbers should be 0 for this method");
            if (abstractEvent.Version.SequenceNumber <= 1)
                Error("Sequence should be greater than 1 for this method");
        }

        private void IsNotComissioned(IFeatureId abstractEvent)
        {
            if (AimTemporalityService.IsComissioned(abstractEvent))
                Error("Feature is already comissioned");
        }

        private void IsComissioned(IFeatureId abstractEvent)
        {
            if (!AimTemporalityService.IsComissioned(abstractEvent))
                Error("Feature should be comissioned");
        }





        private void CommissionValidation(AbstractEvent<AimFeature> abstractEvent)
        {
            if (abstractEvent.Interpretation == Interpretation.TempDelta)
                IsComissioned(abstractEvent);
            if (abstractEvent.Interpretation == Interpretation.PermanentDelta)
                IsNotComissioned(abstractEvent);
        }


    }
}