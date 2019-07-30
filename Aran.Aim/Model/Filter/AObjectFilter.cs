using System;
using System.Runtime.Serialization;
using Aran.Aim.Package;

namespace Aran.Aim
{
    [Serializable]
    public class AObjectFilter : ISerializable
    {
        public AObjectFilter ()
        {
        }

        public AObjectFilter (DBEntity dbEntity, int property)
        {
            Id = dbEntity.Id;
            Type = AimMetadata.GetAimTypeIndex (dbEntity);
            Property = property;
        }

        public AObjectFilter (long id, int type, int property)
        {
            Id = id;
            Type = type;
            Property = property;
        }

        public long Id { get; set; }
        public int Type { get; set; }
        public int Property { get; set; }

        #region ISerializable Members

        protected AObjectFilter (SerializationInfo info, StreamingContext context)
        {
            AranPackageReader reader = CommonFunctions.Desirialize (info);

            Id = reader.GetInt64 ();
            Type = reader.GetInt32 ();
            Property = reader.GetInt32 ();

            reader.Dispose ();
        }

        void ISerializable.GetObjectData (SerializationInfo info, StreamingContext context)
        {
            AranPackageWriter writer = new AranPackageWriter ();

            writer.PutInt64 (Id);
            writer.PutInt32 (Type);
            writer.PutInt32 (Property);

            CommonFunctions.Serialize (writer, info);
        }

        #endregion
    }
}
