#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.Utilities;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Aim.Extension.Message;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.Util;
using Aran.Temporality.Internal.Implementation.Storage;
using Aran.Temporality.Internal.Remote.Util;

#endregion

namespace Aran.Temporality.Common.Aim.MetaData
{
    [Serializable]
    public class AimEvent : AbstractEvent<AimFeature>
    {
        #region Ctor

        public AimEvent()
        {
        }

        public AimEvent(AbstractEventMetaData other) : base(other)
        {
        }

        public AimEvent(AbstractEvent<AimFeature> other) : base(other)
        {
        }

        public AimEvent(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt)
        {
        }

        #endregion

        public override string ToString()
        {
            if (TimeSlice == null) return base.ToString();
            return Version + (IsCanceled ? " cancelled" : "");
        }

        #region Overrides of AbstractMetaData

        private Guid? _guid;
        public override Guid? Guid
        {
            get => Data == null || Data.Identifier == System.Guid.Empty ? _guid : Data?.Identifier;
            set => _guid = value;
        }

        private int _featureTypeId = -1;
        public override int FeatureTypeId
        {
            get => Data == null || Data.Identifier == System.Guid.Empty ? _featureTypeId : (int)Data.FeatureType;
            set => _featureTypeId = value;
        }


        #endregion

        #region Overrides of AbstractEvent<Feature>

        //private static readonly IList<string> SystemNames =
        //    new List<string> {"Guid", "FeatureTypeId", "FeatureType", "Identifier", "Id"};


        protected override void SerializeData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Data", FormatterUtil.ObjectToBytes(Data), typeof(byte[]));
        }

        protected override void DeserializeData(SerializationInfo info, StreamingContext context)
        {
            object bytes = info.GetValue("Data", typeof(byte[]));
            Data = FormatterUtil.ObjectFromBytes<AimFeature>((byte[])bytes);
        }


        protected override void ApplyToDataInternal(AimFeature data, DateTime actualDateTime)
        {
            ApplyToDataInternal(data.Feature);
            ApplyToExtensionInternal(data);
            ApplyToMetaDataInternal(data);
        }

        private void ApplyToMetaDataInternal(AimFeature data)
        {
            if (Data.Feature == null) return;
            if (data.Feature == null) return;

            if (Data.Feature.TimeSliceMetadata != null)
            {
                data.Feature.TimeSliceMetadata = Data.Feature.TimeSliceMetadata;
            }

        }

        private void ApplyToExtensionInternal(AimFeature data)
        {
            if (Data.Feature == null) return;

            var classInfo = AimMetadata.GetClassInfoByIndex(Data.Feature);

            foreach (var propInfo in classInfo.Properties)
            {
                var extensionsToBeApplied = Data.PropertyExtensions.Where(t => t.PropertyIndex == propInfo.Index).ToList();
                if (extensionsToBeApplied.Count <= 0) continue;
                data.PropertyExtensions.RemoveAll(t => t.PropertyIndex == propInfo.Index);
                data.PropertyExtensions.AddRange(extensionsToBeApplied);
            }
        }


        private void ApplyInterpretation(Feature data)
        {
            switch (Interpretation)
            {
                case Enum.Interpretation.Snapshot:
                    data.TimeSlice.Interpretation = TimeSliceInterpretationType.SNAPSHOT;
                    break;
                case Enum.Interpretation.PermanentDelta:
                    data.TimeSlice.Interpretation = TimeSliceInterpretationType.PERMDELTA;
                    break;
                case Enum.Interpretation.TempDelta:
                    data.TimeSlice.Interpretation = TimeSliceInterpretationType.TEMPDELTA;
                    break;
                case Enum.Interpretation.BaseLine:
                    data.TimeSlice.Interpretation = TimeSliceInterpretationType.BASELINE;
                    break;
            }
        }

        private void ApplyToDataInternal(IAimObject data)
        {
            var classInfo = AimMetadata.GetClassInfoByIndex(data);
            var eventData = Data.Feature as IAimObject;

            var feature = data as Feature;
            if (feature != null)
            {
                if (feature.TimeSlice == null)
                {
                    feature.TimeSlice = new TimeSlice();
                }

                if (feature.TimeSlice.FeatureLifetime == null)
                {
                    feature.TimeSlice.FeatureLifetime = new TimePeriod();
                }



                if (LifeTimeBeginSet)
                {
                    feature.TimeSlice.FeatureLifetime.BeginPosition = (DateTime)LifeTimeBegin;
                }

                if (LifeTimeEndSet)
                {
                    feature.TimeSlice.FeatureLifetime.EndPosition = LifeTimeEnd;
                }

                ApplyInterpretation(feature);

                if (Data.Feature != null)
                {
                    foreach (var propInfo in classInfo.Properties)
                    {

                        var nilReason = Data.Feature.GetNilReason(propInfo.Index);
                        if (nilReason != null)
                        {
                            data.SetValue(propInfo.Index, null);
                            feature.SetNilReason(propInfo.Index, nilReason);
                            continue;
                        }

                        var propVal = eventData.GetValue(propInfo.Index);
                        if (propVal != null)
                        {
                            feature.SetNilReason(propInfo.Index, null);

                            if (propVal is IList)
                            {
                                var newList = propVal as IList;
                                var oldList = data.GetValue(propInfo.Index) as IList;

                                //Debug.Assert(oldList != null, "oldList != null");
                                if (oldList != null)
                                {
                                    oldList.Clear();
                                    foreach (var item in newList)
                                    {
                                        oldList.Add(item);
                                    }
                                }
                                else
                                {
                                    data.SetValue(propInfo.Index, propVal);
                                }

                            }
                            else
                            {
                                data.SetValue(propInfo.Index, propVal);
                            }

                        }
                    }
                }
            }
        }

        public override IDictionary<int, List<IFeatureId>> GetRelatedFeatures()
        {
            var result = new SortedList<int, List<IFeatureId>>();

            if (Data == null) return result;
            if (Data.Feature == null) return result;

            AimEvent myEvent = this;

            var featurePropList = new List<RefFeatureProp>();
            AimMetadataUtility.GetReferencesFeatures(Data.Feature, featurePropList);
            foreach (var featureProp in featurePropList)
            {
                var featureTypeId = (int)featureProp.FeatureType;
                var featureId = new FeatureId
                {
                    Guid = featureProp.RefIdentifier,
                    FeatureTypeId = featureTypeId,
                    WorkPackage = myEvent.WorkPackage
                };

                List<IFeatureId> featureIds;
                if (!result.TryGetValue(featureTypeId, out featureIds))
                {
                    featureIds = new List<IFeatureId>();
                    result[featureTypeId] = featureIds;
                }

                featureIds.Add(featureId);
            }

            return result;
        }

        public override void CreateExtensions()
        {
            Data.InitEsriExtension();

            var identity = StorageService.CurrentIdentity();

            string user = null, application = null;

            if (identity?.IsAuthenticated == true)
            {
                var userLogin = identity.Name;
                var i = userLogin.IndexOf("\\", StringComparison.Ordinal);
                var userId = userLogin.Substring(0, i);
                application = userLogin.Substring(i + 1);

                user = StorageService.GetUserById(Convert.ToInt32(userId))?.Name;
            }
            else if (ConfigUtil.ExternalApplication != null)
            {
                user = ConfigUtil.ExternalApplicationUserName;
                application = ConfigUtil.ExternalApplication;
            }
            else
            {
                throw new Exception("Access denied");
            }

            Data.MessageExtensions.RemoveAll(t => t is UserExtension);
            Data.MessageExtensions.Add(new UserExtension
            {
                User = user,
                Application = application
            });
        }

        #endregion
    }
}