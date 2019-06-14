using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Enum;
using Aran.Temporality.Common.Util;
using Aran.Temporality.Internal.Service;

namespace Aran.Temporality.Internal.WorkFlow.Routines
{
    internal class CheckRoutine
    {
        public static SyntaxCheckRoutine SyntaxCheckRoutine=new SyntaxCheckRoutine();
        public static BusinessRuleRoutine BusinessRuleRoutine = new BusinessRuleRoutine();
        public static MissingLinkRoutine MissingLinkRoutine = new MissingLinkRoutine();
        public static FeatureDependencyCheckRoutine FeatureDependencyCheckRoutine = new FeatureDependencyCheckRoutine();


        private static List<AbstractCheckRoutine> _routines; 
        public static List<AbstractCheckRoutine> Routines
        {
			get
			{
				return _routines ?? ( _routines = new List<AbstractCheckRoutine>
                                                     {
                                                         SyntaxCheckRoutine,
                                                         BusinessRuleRoutine,
                                                         MissingLinkRoutine,
                                                         FeatureDependencyCheckRoutine
                                                     } );
			}
        }

        public static void CheckPrivateSlot(PrivateSlot slot, AimTemporalityService service)
        {
            CurrentOperationStatus.PrivateSlotId = slot.Id;
            CurrentOperationStatus.PublicSlotId = slot.PublicSlot.Id;


            var option = service.GetSlotValidationOption(slot.Id);

            var performedRoutines = Routines;
            if (option != null)
            {
                performedRoutines=new List<AbstractCheckRoutine>();
                if ((option.Flag & (int) ValidationOption.CheckSyntax) != 0)
                {
                    performedRoutines.Add(SyntaxCheckRoutine);
                }
                if ((option.Flag & (int)ValidationOption.CheckLinks) != 0)
                {
                    performedRoutines.Add(MissingLinkRoutine);
                }
                if ((option.Flag & (int)ValidationOption.CheckBusinessRules) != 0)
                {
                    performedRoutines.Add(BusinessRuleRoutine);
                }

				FeatureDependencyCheckRoutine.DependencyIdsToProcess = FormatterUtil.ObjectFromBytes<int[]> ( option.MoreOptions );
				performedRoutines.Add ( FeatureDependencyCheckRoutine );
			}

            //TODO: make list of effective dates
            using (var routineContext = new RoutineContext { Service = service , PrivateSlot = slot, EffectiveDate = slot.PublicSlot.EffectiveDate})
            {
                //load all in memory
                CurrentOperationStatus.NewJob(performedRoutines.Count + 1);
                var types = Enum.GetValues(typeof (FeatureType));
                CurrentOperationStatus.NextTask(types.Length);
                foreach (FeatureType featureType in types)
                {
                    routineContext.LoadStates(featureType);
                    CurrentOperationStatus.NextOperation();
                }

               
                var result = false;
                foreach (var routine in performedRoutines)
                {
                    routine.Context = routineContext;
                    result |= routine.CheckPrivateSlot();
                }

                slot.Status = result ? SlotStatus.CheckFailed: SlotStatus.CheckOk;
				service.UpdatePrivateSlot(slot);
                CurrentOperationStatus.EndJob();
            }
        }
    }
}
