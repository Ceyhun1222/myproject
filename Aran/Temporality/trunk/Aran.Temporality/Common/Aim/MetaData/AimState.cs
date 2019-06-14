#region

using System;
using System.Runtime.Serialization;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Abstract.State;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.MetaData;
using Aran.Temporality.Common.Util;

#endregion

namespace Aran.Temporality.Common.Aim.MetaData
{
    [Serializable]
    public class AimState : AbstractState<AimFeature>
    {
        #region Ctor

        public AimState()
        {
        }

        public AimState(AbstractStateMetaData other) : base(other)
        {
        }

        protected AimState(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt)
        {
        }

        #endregion

        #region Overrides of AbstractMetaData

        public override Guid? Guid
        {
            get
            {
                return Data?.Identifier;
            }
            set { }
        }

        public override int FeatureTypeId
        {
            get
            {
                if (Data != null) return (int) Data.FeatureType;
                return -1;
            }
            set { }
        }


        #endregion

        #region Overrides of AbstractState<Feature>

        public override AbstractEvent<AimFeature> EventFromState()
        {
            AbstractState<AimFeature> state = this;

            var result = new AimEvent
                             {
                                 Interpretation = Interpretation.BaseLine,
                                 TimeSlice = new TimeSlice(state.TimeSlice),
                                 Data = state.Data,
                                 Version = new TimeSliceVersion(state.Version),
                                 ApplyTimeSliceToFeatureLifeTime = Version == TimeSliceVersion.First
                             };

            return result;
        }

        protected override void SerializeData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Data", FormatterUtil.ObjectToBytes(Data), typeof (byte[]));
        }

        protected override void DeserializeData(SerializationInfo info, StreamingContext context)
        {
            object bytes = info.GetValue("Data", typeof (byte[]));
            Data = FormatterUtil.ObjectFromBytes<AimFeature>((byte[])bytes);
        }

        #endregion

        public AimState Clone()
        {
            AimState state = this;
            var result = new AimState(state)
                             {
                                 Data = CloneUtil.DeepClone(state.Data)
                             };
            return result;
        }
    }
}