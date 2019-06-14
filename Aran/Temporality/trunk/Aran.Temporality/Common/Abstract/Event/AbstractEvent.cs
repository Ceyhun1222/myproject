using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Internal.MetaData;

namespace Aran.Temporality.Common.Abstract.Event
{
    [Serializable]
    public abstract class AbstractEvent<T> : AbstractEventMetaData, ISerializable where T : class //T is feature type
    {
        #region Ctor

        protected AbstractEvent()
        {
        }

        protected AbstractEvent(AbstractEventMetaData other) : base(other)
        {
        }

        protected AbstractEvent(AbstractEvent<T> other) : base(other)
        {
            Data = other.Data;
        }

        #endregion

        #region Event related properties

        public T Data { get; set; }

        #endregion

        #region Logic

        protected abstract void SerializeData(SerializationInfo info, StreamingContext context);
        protected abstract void DeserializeData(SerializationInfo info, StreamingContext context);

        protected abstract void ApplyToDataInternal(T data, DateTime actualDateTime);
        public abstract IDictionary<int, List<IFeatureId>> GetRelatedFeatures();

        public void ApplyToData(T data, DateTime actualDateTime, bool newBaseLine = false)
        {
            //TODO: add interpretation into account
#warning fix
            if (Data == null || IsCanceled || (newBaseLine && Interpretation == Interpretation.TempDelta))
                return; //if it can be ignored - ignore it
            if (HasImpact(actualDateTime)) //actual by date
            {
                ApplyToDataInternal(data, actualDateTime);
            }
        }

        #endregion

        #region Implementation of ISerializable

        protected AbstractEvent(SerializationInfo info, StreamingContext context)
        {
            var meta = (EventMetaData)info.GetValue("Meta", typeof(EventMetaData));
            InitFromEventMeta(meta);
            DeserializeData(info, context);
        }


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            var meta=new EventMetaData(this);
            info.AddValue("Meta", meta, typeof(EventMetaData));
            
            SerializeData(info, context);
        }

        #endregion

        public abstract void CreateExtensions();
    }
}