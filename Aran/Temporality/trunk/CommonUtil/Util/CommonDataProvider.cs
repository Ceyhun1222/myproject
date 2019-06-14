using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Aim;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.Utilities;
using Aran.Metadata.Utils;
using Aran.Temporality.Common.Abstract.State;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Util;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.MetaData;
using Aran.Temporality.Common.Util;
using Aran.Temporality.CommonUtil.Context;
using UMCommonModels.Dto;
using WebApiClient;

namespace Aran.Temporality.CommonUtil.Util
{
    public class CommonDataProvider
    {


        private static List<BusinessRuleUtil> _businessRules;
        public static IEnumerable<BusinessRuleUtil> BisinessRules
        {
            get
            {
                return _businessRules ??
                       (_businessRules =
                        CurrentDataContext.GetBusinessRules().OrderBy(t => t.ApplicableType).
                            ThenBy(t => t.Name).ToList());
            }
        }

        public static bool ActivatePrivateSlot(int privateSlotId)
        {
            if (CurrentDataContext.CurrentUser == null) return false;

            if (privateSlotId == 0)
            {
                CurrentDataContext.CurrentUser.ActivePrivateSlot = null;
                CurrentDataContext.CurrentNoAixmDataService.SetUserActiveSlotId(CurrentDataContext.CurrentUser.Id, 0);
            }
            else
            {
                var privateSlot = GetPrivateSlotById(privateSlotId);
                CurrentDataContext.CurrentUser.ActivePrivateSlot = privateSlot;
                CurrentDataContext.CurrentNoAixmDataService.SetUserActiveSlotId(CurrentDataContext.CurrentUser.Id, privateSlotId);
            }

            return true;
        }

        public static PrivateSlot GetActivePrivateSlot()
        {
            if (CurrentDataContext.CurrentUser == null) return null;
            return CurrentDataContext.CurrentUser.ActivePrivateSlot;
        }

        public static void DeactivatePrivateSlot(int privateSlotId)
        {
            if (CurrentDataContext.CurrentUser == null) return;
            CurrentDataContext.CurrentUser.ActivePrivateSlot = null;
            CurrentDataContext.CurrentNoAixmDataService.SetUserActiveSlotId(CurrentDataContext.CurrentUser.Id, 0);
        }

        public static bool DeletePrivateSlot(int id)
        {
            return CurrentDataContext.CurrentService.DeletePrivateSlot(id);
        }

        public static bool UpdatePrivateSlot(PrivateSlot privateSlot)
        {
            return CurrentDataContext.CurrentNoAixmDataService.UpdatePrivateSlot(privateSlot);
        }

        public static PrivateSlot GetPrivateSlotById(int id)
        {
            return CurrentDataContext.CurrentNoAixmDataService.GetPrivateSlotById(id);
        }

        public static int CreatePrivateSlot(PrivateSlot privateSlot)
        {
            return CurrentDataContext.CurrentNoAixmDataService.CreatePrivateSlot(privateSlot);
        }

        public static bool UpdatePublicSlot(PublicSlot publicSlot)
        {
            return CurrentDataContext.CurrentNoAixmDataService.UpdatePublicSlot(publicSlot);
        }

        public static PublicSlot GetPublicSlotById(int id)
        {
            return CurrentDataContext.CurrentNoAixmDataService.GetPublicSlotById(id);
        }

        public static bool DeletePublicSlot(int id)
        {
            return CurrentDataContext.CurrentNoAixmDataService.DeletePublicSlot(id);
        }

        public static int CreatePublicSlot(PublicSlot publicSlot)
        {
            return CurrentDataContext.CurrentNoAixmDataService.CreatePublicSlot(publicSlot);
        }

        public static IList<PrivateSlot> GetPrivateSlots(int id)
        {
            var userId = -1;
            if (CurrentDataContext.CurrentUser != null)
            {
                userId = CurrentDataContext.CurrentUser.Id;
            }
            return CurrentDataContext.CurrentNoAixmDataService.GetPrivateSlots(id, userId);
        }

        public static IList<PublicSlot> GetPublicSlots()
        {
            if (CurrentDataContext.CurrentNoAixmDataService == null) return new List<PublicSlot>();

            return CurrentDataContext.CurrentNoAixmDataService.GetPublicSlots();
        }



        public static bool Decomission(Feature feature, int workpackage = 0)
        {
            if (feature.TimeSlice.FeatureLifetime.EndPosition == null)
                throw new Exception("Feature lifetime endposition is null");

            return CurrentDataContext.CurrentService.Decommission(new FeatureId
            {
                Guid = feature.Identifier,
                FeatureTypeId = (int)feature.FeatureType,
                WorkPackage = workpackage
            }, (DateTime)feature.TimeSlice.FeatureLifetime.EndPosition);
        }

        public static void CommitAsCorrection(Feature feature, int workpackage = 0)
        {
            var aimEvent = CreateNewEvent(feature, workpackage);

            aimEvent.Version = new TimeSliceVersion(feature.TimeSlice.SequenceNumber, -1);

            feature.SetMetadataIdentificationInfo(CurrentDataContext.UserDto, CiRoleCode.PointOfContact);
            feature.SetMetadataEffectiveDate();
            feature.SetMetadataAmendments(false);

            var result = CurrentDataContext.CurrentService.CommitCorrection(aimEvent);

            if (!result.IsOk) throw new Exception(result.ErrorMessage);
        }



        public static void Commission(Feature feature, int workpackage = 0)
        {
            var aimEvent = CreateNewEvent(feature, workpackage);

            feature.SetMetadataIdentificationInfo(CurrentDataContext.UserDto, CiRoleCode.PointOfContact);
            feature.SetMetadataEffectiveDate();
            feature.SetMetadataAmendments(true);

            var result = CurrentDataContext.CurrentService.CommitNewEvent(aimEvent);

            if (!result.IsOk) throw new Exception(result.ErrorMessage);
        }

        private static AimEvent CreateNewEvent(Feature feature, int workpackage = 0)
        {
            var aimEvent = new AimEvent
            {
                TimeSlice = new TimeSlice
                {
                    BeginPosition = feature.TimeSlice.ValidTime.BeginPosition,
                    EndPosition = feature.TimeSlice.ValidTime.EndPosition,
                },

                Interpretation = (feature.TimeSlice.Interpretation == TimeSliceInterpretationType.PERMDELTA
                                  || feature.TimeSlice.Interpretation == TimeSliceInterpretationType.BASELINE)
                    ? Interpretation.PermanentDelta
                    : Interpretation.TempDelta,

                Version = new TimeSliceVersion(1, 0),
                Data = feature,
                WorkPackage = workpackage
            };

            if (feature.TimeSlice.FeatureLifetime != null)
            {
                aimEvent.LifeTimeBegin = feature.TimeSlice.FeatureLifetime.BeginPosition;
                aimEvent.LifeTimeEnd = feature.TimeSlice.FeatureLifetime.EndPosition;
            }
            return aimEvent;
        }

        public static void CommitAsNewSequence(Feature feature, int workpackage = 0)
        {
            CommitNewEvent(feature, workpackage);
        }

        private static void CommitNewEvent(Feature feature, int workpackage = 0, TimeSliceVersion version = null)
        {
            var aimEvent = new AimEvent
            {
                TimeSlice = new TimeSlice
                {
                    BeginPosition = feature.TimeSlice.ValidTime.BeginPosition,
                    EndPosition = feature.TimeSlice.ValidTime.EndPosition,
                },

                Interpretation = (feature.TimeSlice.Interpretation == TimeSliceInterpretationType.PERMDELTA
                                  || feature.TimeSlice.Interpretation == TimeSliceInterpretationType.BASELINE)
                    ? Interpretation.PermanentDelta
                    : Interpretation.TempDelta,

                Version = (version == null) ? new TimeSliceVersion(feature.TimeSlice.SequenceNumber, feature.TimeSlice.CorrectionNumber) : version,
                Data = feature,
                WorkPackage = workpackage
            };

            if (feature.TimeSlice.FeatureLifetime != null)
            {
                aimEvent.LifeTimeBegin = feature.TimeSlice.FeatureLifetime.BeginPosition;
                aimEvent.LifeTimeEnd = feature.TimeSlice.FeatureLifetime.EndPosition;
            }

            feature.SetMetadataIdentificationInfo(CurrentDataContext.UserDto, CiRoleCode.PointOfContact);
            feature.SetMetadataEffectiveDate();
            feature.SetMetadataAmendments(true);

            var result = CurrentDataContext.CurrentService.CommitNewEvent(aimEvent);

            if (!result.IsOk) throw new Exception(result.ErrorMessage);
        }

        public static AimFeature GetState(FeatureType featureType, Guid id, DateTime actualDate)
        {
            var states = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId { FeatureTypeId = (int)featureType, Guid = id },
                      false,
                      actualDate
                      );

            if (states != null && states.Count > 0)
            {
                return states[0].Data;
            }
            return null;
        }

        public static StateWithDelta<AimFeature> GetDataForEditor(FeatureType featureType, Guid id, DateTime actualDate, int workpackage = 0, Interpretation interpretation = Interpretation.Snapshot, DateTime? endDate = null)
        {
            return CurrentDataContext.CurrentService.GetActualDataForEditing(
                       new FeatureId
                       {
                           FeatureTypeId = (int)featureType,
                           Guid = id,
                           WorkPackage = workpackage
                       },
                       actualDate,
                       interpretation,
                       endDate
                       );
        }



        private static DateTime cacheDate;
        private static Dictionary<FeatureType, IList<AbstractState<AimFeature>>> cachedStates =
            new Dictionary<FeatureType, IList<AbstractState<AimFeature>>>();

        public static void ClearCache()
        {
            cachedStates.Clear();
        }


        public static bool HasLinkTo(AimFeature feature2, AimFeature feature1)
        {
            var featurePropList = new List<RefFeatureProp>();
            AimMetadataUtility.GetReferencesFeatures(feature2.Feature, featurePropList);
            return featurePropList.Any(t => t.FeatureType == feature1.FeatureType && t.RefIdentifier == feature1.Identifier);
        }




        public static List<FeatureDependencyConfiguration> GetFeatureDependencies()
        {
            return CurrentDataContext.GetFeatureDependencies();
        }


        public static List<UserDto> GetUsers()
        {
            if (!ConfigUtil.UseWebApiForMetadata)
                return new List<UserDto>();

            return new UserClient().GetUsers();
        }
    }
}
