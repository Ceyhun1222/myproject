using System;
using System.Runtime.Serialization;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Abstract.MetaData;

namespace Aran.Temporality.Common.Abstract.State
{
    [Serializable]
    public abstract class AbstractState<T> : AbstractStateMetaData, ISerializable where T : class //T is feature type
    {
        #region Ctor

        protected AbstractState()
        {
        }

        protected AbstractState(AbstractStateMetaData other) : base(other)
        {
        }

        #endregion

        #region State related properties

        public T Data { get; set; }

        #endregion

        #region Conversions

        public abstract AbstractEvent<T> EventFromState();

        #endregion

        public override bool Equals(object obj)
        {
            if (obj is AbstractMetaData)
            {
                var meta = obj as AbstractMetaData;
                return meta.WorkPackage == WorkPackage &&
                       meta.Version == Version &&
                       meta.Guid == Guid;
            }

            return false;
        }

        public override int GetHashCode()
        {
            var result = WorkPackage*500;
         
            if (Version != null)
                result += Version.SequenceNumber* 100 + Version.CorrectionNumber;
            if (Guid != null)
                result += Guid.GetHashCode();

            return result;
        }

        protected abstract void SerializeData(SerializationInfo info, StreamingContext context);
        protected abstract void DeserializeData(SerializationInfo info, StreamingContext context);

        #region Implementation of ISerializable

        protected AbstractState(SerializationInfo info, StreamingContext context)
        {
            var meta = (AbstractStateMetaData) info.GetValue("Meta", typeof (AbstractStateMetaData));
            InitFromMeta(meta);
            DeserializeData(info, context);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Meta", this, typeof (AbstractStateMetaData));
            SerializeData(info, context);
        }

        #endregion
    }
}