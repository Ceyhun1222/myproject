#region

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.Metadata;
using Aran.Aim.Objects;
using Aran.Geometries;
using Aran.Package;
using Aran.Temporality.Common.Logging;

#endregion

namespace Aran.Temporality.Common.Util
{
    public class ArrayHolder
    {
        public byte[] Array;
    }

    public class FormatterUtil
    {
        #region static properties
        public static readonly IFormatter Formatter = new BinaryFormatter();
        #endregion

        #region Combine
        private static byte[] Combine(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }
        #endregion

        #region Colection
        public static byte[] MultiCollectionToBytes<T>(List<T>[] collection, bool isComunications)
        {
            var resultBag = new ConcurrentBag<byte[]>();

            ParallelUtil.ProcessCollection(collection,
                () => new ArrayHolder(),
                (list, arrayHolder) =>
                {
                    arrayHolder.Array = SingleObjectToBytes(list, isComunications);
                },
                subArray =>
                {
                    resultBag.Add(subArray.Array);
                });

            var resultList = resultBag.ToList();

            Int32[] lengths = resultList.Select(t => t.Length).ToArray();

            byte[] lengthsSerialized = new byte[lengths.Length * sizeof(Int32)];
            Buffer.BlockCopy(lengths, 0, lengthsSerialized, 0, lengthsSerialized.Length);

            var paramList = new List<byte[]>
            {
                MagicUtil.Collection, 
                BitConverter.GetBytes(lengths.Length),
                lengthsSerialized
            };
            paramList.AddRange(resultList);

            return Combine(paramList.ToArray());
        }


        public static byte[] CollectionToBytes(IList collection, bool isComunications=false)
        {
            return MultiCollectionToBytes(CollectionUtil.SplitList(collection.Cast<object>().ToList()), isComunications);
        }
        public static IList CollectionFromBytes(byte[] bytes, Type type)
        {
            byte[] lengthBytes = new byte[4];
            lengthBytes[0] = bytes[2];
            lengthBytes[1] = bytes[3];
            lengthBytes[2] = bytes[4];
            lengthBytes[3] = bytes[5];

            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(lengthBytes);
            }


            Int32 collectionCount = BitConverter.ToInt32(lengthBytes, 0);

            Int32[] lengths = new Int32[collectionCount];
            byte[] lengthsBytes = new byte[collectionCount * sizeof(Int32)];
            Buffer.BlockCopy(bytes, 6, //2 for magic 4 for length
               lengthsBytes, 0, collectionCount * sizeof(Int32));

            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(lengthsBytes);
            }

            Buffer.BlockCopy(lengthsBytes, 0, lengths, 0, collectionCount * sizeof(Int32));


            Buffer.BlockCopy(bytes,
                6, //2 for magic 4 for length
                lengths, 0, collectionCount * sizeof(Int32));


            var bag = new ConcurrentBag<IList>();

            Parallel.For(0, collectionCount, (i, state) =>
            {
                var blockLength = lengths[i];
                var blockOffset = 6 + collectionCount * sizeof(Int32);

                for (var j = 0; j < i; j++)
                {
                    blockOffset += lengths[j];
                }

                bag.Add((IList)SingleObjectFromBytes(bytes, blockOffset, blockLength));
            });


            //make list of exact type
            IList list=null;
            if (type.IsGenericType)
            {
                var genericArguments = type.GetGenericArguments();
                if (genericArguments.Length > 0)
                {
                    var genericType = genericArguments[0];
                    list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(genericType));
                }
            }
            else
            {
                
            }

            foreach (var item in bag.SelectMany(packet => packet.Cast<object>().Where(item => item != null)))
            {
                if (list == null)
                {
                    list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(item.GetType()));
                }
                list.Add(item);
            }

            return list??new List<object>();
            //(IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));

        }
        #endregion


        #region Object
        public static object ObjectFromBytes(byte[] bytes, Type type = null)
        {
            if (bytes == null) return null;
            if (bytes.Length < 2) return SingleObjectFromBytes(bytes, type);//no magic

            if (bytes[0] == MagicUtil.Collection[0] && bytes[1] == MagicUtil.Collection[1])//collection magic
            {
                return CollectionFromBytes(bytes, type);
            }

            if (bytes[0] == MagicUtil.ByteArray[0] && bytes[1] == MagicUtil.ByteArray[1])//ByteArray magic
            {
                var subBytes = new byte[bytes.Length - 2];
                Buffer.BlockCopy(bytes, 2, subBytes, 0, subBytes.Length);
                return subBytes;
            }

            return SingleObjectFromBytes(bytes, type);//gz magic or no magic
        }

        public static byte[] ObjectToBytes(object from, bool honorLists=false, bool isComunications=false)
        {
            var fromBytes = from as byte[];
            if (fromBytes != null)
            {
                return Combine(MagicUtil.ByteArray, fromBytes);
            }

            var collection = from as IList;
            if (collection != null && honorLists && collection.Count>0)
            {
                return CollectionToBytes(collection, isComunications);//put collection magic
            }
            return SingleObjectToBytes(from, isComunications);//put gz magic or no magic
        }
        #endregion


        #region Single Object
        private static byte[] SingleObjectToBytes(object from, bool isComunications)
        {
            if (from == null) return null;
            using (var stream = SingleThreadSerializableStream.OpenForWrite(from, isComunications))
            {
                WriteDataToStream(from, stream);
                return stream.ToArray();
            }
        }


        private static void WriteDataToStream(object from, Stream stream)
        {
            var packable = from as IPackable;
            if (packable != null)
            {
                using (var packageWriter = new BinaryPackageWriter(stream))
                {
                    if (from is Geometry)
                    {
                        var geometry = from as Geometry;
                        var geometryType = geometry.Type;

                        packageWriter.PutByte(0);
                        //0 - means geometry
                        //1 - means other

                        //write type, then data
                        packageWriter.PutInt32((int)geometryType);
                        geometry.Pack(packageWriter);
                    }
                    else if (from is IAimObject)
                    {
                        //0 - means geometry
                        //1 - means other (old)
                        //2 - new type serialization (with metadata)

                        //packageWriter.PutByte(1);

                        //int featureTypeId = -1;

                        ////write type, then data
                        //var feature = from as IAimObject;
                        //switch (feature.AimObjectType)
                        //{
                        //    case AimObjectType.DataType:
                        //        featureTypeId = (int)((ADataType)feature).DataType;
                        //        break;
                        //    case AimObjectType.Feature:
                        //        featureTypeId = (int)((Feature)feature).FeatureType;
                        //        break;
                        //    case AimObjectType.Field:
                        //        featureTypeId = (int)((AimField)feature).FieldType;
                        //        break;
                        //    case AimObjectType.Object:
                        //        featureTypeId = (int)((AObject)feature).ObjectType;
                        //        break;
                        //}

                        //packageWriter.PutInt32(featureTypeId);
                        //packable.Pack(packageWriter);


                        
                        int featureTypeId = -1;

                        //write type, then data
                        var feature = from as IAimObject;
                        switch (feature.AimObjectType)
                        {
                            case AimObjectType.DataType:
                                packageWriter.PutByte(1);

                                featureTypeId = (int)((ADataType)feature).DataType;
                                break;
                            case AimObjectType.Feature:
                                
                                var metadata=((Feature) feature).TimeSliceMetadata as IPackable;
                                if (metadata != null)
                                {
                                    packageWriter.PutByte(2);
                                    metadata.Pack(packageWriter);
                                }
                                else
                                {
                                    packageWriter.PutByte(1);
                                }

                                featureTypeId = (int)((Feature)feature).FeatureType;
                                break;
                            case AimObjectType.Field:
                                packageWriter.PutByte(1);
                
                                featureTypeId = (int)((AimField)feature).FieldType;
                                break;
                            case AimObjectType.Object:
                                packageWriter.PutByte(1);

                                featureTypeId = (int)((AObject)feature).ObjectType;
                                break;
                        }

                        packageWriter.PutInt32(featureTypeId);
                        packable.Pack(packageWriter);
                    }
                    else
                    {
                        throw new Exception("Unsupported type for serialization");
                    }
                }
            }
            else
            {
                Formatter.Serialize(stream, from);
            }
        }

        private static object SingleObjectFromBytes(byte[] bytes, int offset, int length, Type t=null)
        {
            if (bytes == null) return null;

            using (var s = SingleThreadSerializableStream.OpenForRead(bytes, offset, length))
            {
                if (t!=null && typeof(IPackable).IsAssignableFrom(t))
                {
                    using (var packageReader = new BinaryPackageReader(s))
                    {
                        //archiType
                        //0 - means geometry
                        //1 - means other
                        var archiType = packageReader.GetByte();

                      
                        if (archiType == 0)
                        {
                            //subtype (feature/object type)
                            var featureTypeId = packageReader.GetInt32();

                            IPackable geometry = Geometry.Create((GeometryType) featureTypeId);
                            geometry.Unpack(packageReader);
                            return geometry;
                        }

                        if (archiType == 1)
                        {
                            //subtype (feature/object type)
                            var featureTypeId = packageReader.GetInt32();

                            //read type, then data
                            IPackable feature = AimObjectFactory.Create(featureTypeId);
                            feature.Unpack(packageReader);
                            return feature;
                        }

                        if (archiType == 2)
                        {
                            var metadata = new FeatureTimeSliceMetadata();
                            ((IPackable) metadata).Unpack(packageReader);

                            var featureTypeId = packageReader.GetInt32();

                            IPackable feature = AimObjectFactory.Create(featureTypeId);
                            feature.Unpack(packageReader);
                            ((Feature) feature).TimeSliceMetadata = metadata;
                            return feature;
                        }
                        
                    }
                }

                return Formatter.Deserialize(s);
            }
        }
     
        private static object SingleObjectFromBytes(byte[] bytes, Type t = null)
        {
            return SingleObjectFromBytes(bytes, 0, bytes.Length, t);
        }
        #endregion

        #region public helpers

        public static T ObjectFromBytes<T>(byte[] bytes) where T : class
        {

            if (bytes == null) return null;
            var type = typeof (T);
            var obj = ObjectFromBytes(bytes, type);
            if (obj == null) return null;
            var typedObject = obj as T;
            if (typedObject != null) return typedObject;

            throw new Exception("Serialization exception, type mismatch "+typeof(T).Name+" != "+obj.GetType().Name);
            var newList = Activator.CreateInstance(type, obj);
            return (T)newList;
        }

        public static object Clone(object source)
        {
            var list = source as IList;
            if (list != null)
            {
                return new ArrayList(list);//fast clone lists
            }
            return source == null ? null : ObjectFromBytes(ObjectToBytes(source),source.GetType());
        }
        #endregion

        public static byte[] ComunicationObjectToBytes(object communicationObject)
        {
            return ObjectToBytes(communicationObject, true, true);
        }

        public static byte[] CompressMaximumObjectToBytes(object from)
        {
            using (var stream = SingleThreadSerializableStream.OpenForWrite(from, false, true))
            {
                WriteDataToStream(from, stream);
                return stream.ToArray();
            }
        }
    }
}