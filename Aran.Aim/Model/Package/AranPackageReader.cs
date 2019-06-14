using System;
using System.Collections;
using System.IO;
using Aran.Geometries;
using Aran.Package;

namespace Aran.Aim.Package
{
    public class AranPackageReader : 
        PackageReader,
        IDisposable
    {
        public AranPackageReader (Stream stream)
        {
            _binaryReader = new BinaryReader (stream);
            _destroyStream = false;
        }

        public AranPackageReader (byte [] buffer)
        {
            MemoryStream ms = new MemoryStream (buffer);
            _binaryReader = new BinaryReader (ms);
            _destroyStream = true;
        }

        public override byte GetByte ()
        {
            return _binaryReader.ReadByte ();
        }

        public override bool GetBool ()
        {
            return _binaryReader.ReadBoolean ();
        }

        public override short GetInt16 ()
        {
            return _binaryReader.ReadInt16 ();
        }

        public override int GetInt32 ()
        {
            return _binaryReader.ReadInt32 ();
        }

        public override long GetInt64 ()
        {
            return _binaryReader.ReadInt64 ();
        }

        public override uint GetUInt32 ()
        {
            return _binaryReader.ReadUInt32 ();
        }

        public override double GetDouble ()
        {
            return _binaryReader.ReadDouble ();
        }

        public override string GetString ()
        {
            return _binaryReader.ReadString ();
        }


        private IConvertible GetSystemType (TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Byte:
                    return GetByte ();
                case TypeCode.Boolean:
                    return GetBool ();
                case TypeCode.Int32:
                    return GetInt32 ();
                case TypeCode.Int64:
                    return GetInt64 ();
                case TypeCode.Double:
                    return GetDouble ();
                case TypeCode.String:
                    return GetString ();
                case TypeCode.DateTime:
                    return GetDateTime ();
                default:
                    throw new Exception ("System type not supported for unpack.");
            }
        }

        private IList GetObjectArray ()
        {
            return null;

            //int listCount = GetInt32 ();
            //if (listCount == 0)
            //    return null;

            //PackedTypeInfo itemTypeInfo = (PackedTypeInfo) GetByte ();

            //if (itemTypeInfo == PackedTypeInfo.AranObject)
            //{
            //    int aranTypeIndex = GetInt32 ();
            //    IList aranList = AranObjectFactory.CreateList (aranTypeIndex);

            //    for (int i = 0; i < listCount; i++)
            //    {
            //        AranObject aranObject = AranObjectFactory.Create (aranTypeIndex);
            //        (aranObject as IPackable).Unpack (this);
            //        aranList.Add (aranObject);
            //    }
            //    return aranList;
            //}
            //else if (itemTypeInfo == PackedTypeInfo.Geometry)
            //{
            //    GeometryType geomType = GetEnum<GeometryType> ();
            //    IList geomList = GeometryFactory.CreateList (geomType);
                
            //    for (int i = 0; i < listCount; i++)
            //    {
            //        Geometry geometry = GeometryFactory.Create (geomType);
            //        geometry.Unpack (this);
            //        geomList.Add (geometry);
            //    }
            //    return geomList;
            //}
            //else
            //{
            //    IList systemTypeList = AranObjectFactory.CreateSystemTypeList (itemTypeInfo);

            //    for (int i = 0; i < listCount; i++)
            //    {
            //        systemTypeList.Add (GetSystemType ((TypeCode) itemTypeInfo));
            //    }

            //    return systemTypeList;
            //}
        }

        public void Dispose ()
        {
            if (_destroyStream)
                _binaryReader.BaseStream.Dispose ();
        }

        private BinaryReader _binaryReader;
        private bool _destroyStream;
    }

    
}
