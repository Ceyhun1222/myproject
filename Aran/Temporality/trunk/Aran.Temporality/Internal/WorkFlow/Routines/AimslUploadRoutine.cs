using System;
using System.Collections.Generic;
using AIMSLServiceClient.Config;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Internal.Service;
using AimslSettings = AIMSLServiceClient.Config.AimslSettings;

namespace Aran.Temporality.Internal.WorkFlow.Routines
{
    internal class AimslUploadRoutine
    {

        public static void Proccess(IList<AimslOperation> operations, AimTemporalityService aimTemporalityService)
        {
            foreach (var operation in operations)
            {
                if (operation.CreationTime.AddMinutes(AimslSettings.Settings.ExpectationTimeout) <= DateTime.Now)
                {
                    if (operation.InternalStatus == AimslOperationStatusType.Opened)
                    {
                        if (operation.PullPoint == null)
                        {
                            aimTemporalityService.AimslDestroy(operation, true);
                            continue;
                        }
                    }

                    aimTemporalityService.AimslDestroyPullPoint(operation, true);
                    continue;
                }

                if (operation.InternalStatus == AimslOperationStatusType.Opened)
                {
                    if (operation.PullPoint == null)
                        aimTemporalityService.AimslAddPullPoint(operation);
                    else if (operation.Subscription == null)
                        aimTemporalityService.AimslAddSubscription(operation);
                    else
                        aimTemporalityService.AimslGetMessages(operation);
                    continue;
                }

                if (operation.InternalStatus == AimslOperationStatusType.Closed)
                {
                    aimTemporalityService.AimslDestroyPullPoint(operation);
                    continue;
                }
            }
        }

      


    }
}