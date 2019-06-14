using System;
using System.Runtime.Serialization;
using Aran.Aim.Package;

namespace Aran.Aim
{
    [Serializable]
    public class AimSerialization : ISerializable
    {
        public AimSerialization (AimObject aimObject)
        {
            _aimObject = aimObject;
        }

        protected AimSerialization (SerializationInfo info, StreamingContext context)
        {
            AranPackageReader reader = CommonFunctions.Desirialize (info);
            //_aranObject = reader.GetObject () as AimObject;
            reader.Dispose ();
        }

        public T Cast<T> () where T : AimObject
        {
            return (T) _aimObject;
        }
        
        void ISerializable.GetObjectData (SerializationInfo info, StreamingContext context)
        {
            AranPackageWriter writer = new AranPackageWriter ();
            //writer.PutObject (_aranObject);
            CommonFunctions.Serialize (writer, info);
        }

        private AimObject _aimObject;
    }
}
