using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using Aran.Geometries;
using Aran.Package;

namespace Aran.Aim.Package
{
    public class AranPackageWriter :
        PackageWriter,
        IDisposable
    {
        public AranPackageWriter (Stream stream)
        {
            _writer = new BinaryWriter (stream);
            _disposeStream = false;
        }

        public AranPackageWriter ()
        {
            MemoryStream ms = new MemoryStream ();
            _writer = new BinaryWriter (ms);
            _disposeStream = true;
        }

        public override void PutByte (byte value)
        {
            _writer.Write (value);
        }

        public override void PutBool (bool value)
        {
            _writer.Write (value);
        }

        public override void PutInt16 (short value)
        {
            _writer.Write (value);
        }

        public override void PutInt32 (int value)
        {
            _writer.Write (value);
        }

        public override void PutInt64 (long value)
        {
            _writer.Write (value);
        }

        public override void PutUInt32 (uint value)
        {
            _writer.Write (value);
        }

        public override void PutDouble (double value)
        {
            _writer.Write (value);
        }

        public override void PutString (string value)
        {
            _writer.Write (value);
        }

        private void PutSystemType (IConvertible value)
        {
            TypeCode typeCode = value.GetTypeCode ();

            switch (typeCode)
            {
                case TypeCode.Byte:
                    PutByte ((byte) value);
                    return;
                case TypeCode.Boolean:
                    PutBool ((bool) value);
                    return;
                case TypeCode.Int32:
                    PutInt32 ((Int32) value);
                    return;
                case TypeCode.Int64:
                    PutInt64 ((Int64) value);
                    return;
                case TypeCode.Double:
                    PutDouble ((double) value);
                    return;
                case TypeCode.String:
                    PutString ((string) value);
                    return;
                case TypeCode.DateTime:
                    PutDateTime ((DateTime) value);
                    return;
                default:
                    throw new Exception ("System type not supported for pack.");
            }
        }

        private void PutList (IList list)
        {
            PutByte ((byte) PackedTypeInfo.List);
            PutInt32 (list.Count);

            if (list.Count > 0)
            {
                object listItem = list [0];

                if (listItem is IConvertible)
                {
                    PutByte ((byte) (PackedTypeInfo) (listItem as IConvertible).GetTypeCode ());

                    for (int i = 0; i < list.Count; i++)
                    {
                        PutSystemType (list [i] as IConvertible);
                    }
                }
                else if (listItem is AimObject)
                {
                    PutByte ((byte) PackedTypeInfo.AranObject);
                    PutInt32 (AimMetadata.GetAimTypeIndex (listItem as AimObject));

                    for (int i = 0; i < list.Count; i++)
                    {
                        (list [i] as IPackable).Pack (this);
                    }
                }
                else if (listItem is Geometry)
                {
                    PutByte ((byte) PackedTypeInfo.Geometry);
                    PutEnum<GeometryType> ((listItem as Geometry).Type);
                    
                    for (int i = 0; i < list.Count; i++)
                    {
                        (list [i] as Geometry).Pack (this);
                    }
                }
                else
                {
                    throw new Exception ("List item type not supported for package.");
                }
            }
        }

        public byte [] GetBytes ()
        {
            return (_writer.BaseStream as MemoryStream).ToArray ();
        }

        public void Dispose ()
        {
            if (_disposeStream)
                _writer.BaseStream.Dispose ();
        }

        private BinaryWriter _writer;
        private bool _disposeStream;
    }
}
