using System;
using System.IO;
using System.Runtime.Serialization;
using Aran.Geometries;
using Aran.Package;

namespace Aran.Geometries
{
    [Serializable]
    public class GeometryData : ISerializable
    {
        public GeometryData (Geometry geometry)
        {
            _geometry = geometry;

            if (_geometry == null)
                throw new Exception ("Geometry is null");
        }

        protected GeometryData (SerializationInfo info, StreamingContext context)
        {
            string s = info.GetString ("Value");
            char [] ca = s.ToCharArray ();
            byte [] buffer = new byte [ca.Length];
            for (int i = 0; i < ca.Length; i++)
                buffer [i] = (byte) ca [i];

            BinaryPackageReader reader = new BinaryPackageReader (buffer);
            _geometry = Geometry.UnpackGeometry (reader);
            reader.Dispose ();
        }

        public Geometry Geometry
        {
            get { return _geometry; }
        }

        void ISerializable.GetObjectData (SerializationInfo info, StreamingContext context)
        {
            BinaryPackageWriter writer = new BinaryPackageWriter ();

            Geometry.PackGeometry (writer, _geometry);

            byte [] buffer = (writer.Writer.BaseStream as MemoryStream).ToArray ();
            writer.Dispose ();

            char [] ca = new char [buffer.Length];
            for (int i = 0; i < ca.Length; i++)
                ca [i] = (char) buffer [i];
            string s = new string (ca);
            info.AddValue ("Value", s);
        }

        private Geometry _geometry;
    }
}
