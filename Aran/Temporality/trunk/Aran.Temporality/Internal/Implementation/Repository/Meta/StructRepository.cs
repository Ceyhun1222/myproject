#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Aran.Aim.Data;
using Aran.Temporality.Internal.Abstract.Repository;

#endregion

namespace Aran.Temporality.Internal.Implementation.Repository.Meta
{
    internal class StructRepository<TDataType> : AbstractFileDataRepository<TDataType, Int32>
        where TDataType : struct
    {
        private readonly byte[] _buffer = new byte[Marshal.SizeOf(typeof(TDataType))];
        private readonly OffsetRepository _deletedIndices;
        private readonly IntPtr _pointer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TDataType)));
        private readonly int _structSize = Marshal.SizeOf(typeof(TDataType));

        #region Overrides of AbstractDataRepository<TDataType>



        //private FileStream _stream;

        public sealed override void Open(bool rewrite = false)
        {
            string folder = FileName.Substring(0, FileName.LastIndexOf("\\", StringComparison.Ordinal));
            Directory.CreateDirectory(folder);

            if (rewrite && File.Exists(FileName))
            {
                File.Delete(FileName);
            }

            //_stream = File.Open(FileName, rewrite ? FileMode.Create : FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            _deletedIndices.Open(rewrite);
        }

        public override void Close()
        {
            _deletedIndices.Close();
            //_stream.Close();
        }

        public override void RemoveAll()
        {
            Close();
            Open(true);
        }

        private TDataType FromBuffer()
        {
            var result = Activator.CreateInstance<TDataType>();
            Marshal.Copy(_buffer, 0, _pointer, _structSize);
            result = (TDataType)Marshal.PtrToStructure(_pointer, result.GetType());
            return result;
        }

        private void ToBuffer(TDataType item)
        {
            Marshal.StructureToPtr(item, _pointer, true);
            Marshal.Copy(_pointer, _buffer, 0, _structSize);
        }

        public List<TDataType?> LoadAll()
        {
            var result = new List<TDataType?>();

            using (FileStream stream = File.Open(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                List<int> deleted = _deletedIndices.LoadAll();

                result.Clear();
                stream.Position = 0;
                int index = 0;
                while (true)
                {
                    int read = stream.Read(_buffer, 0, _structSize);
                    if (read != _structSize) break;
                    if (!deleted.Contains(index))
                    {
                        result.Add(FromBuffer());
                    }
                    else
                    {
                        result.Add(null);
                    }
                    index++;
                }
            }

            return result;

            //var result = new List<TDataType?>();

            //var deleted =  _deletedIndices.LoadAll();

            //result.Clear();
            //_stream.Position = 0;
            //var index = 0;
            //while (true)
            //{
            //    var read = _stream.Read(_buffer, 0, _structSize);
            //    if (read != _structSize) break;
            //    if (!deleted.Contains(index))
            //    {
            //        result.Add(FromBuffer());
            //    }
            //    else
            //    {
            //        result.Add(null);
            //    }
            //    index++;
            //}
            //return result;
        }

        public void Add(TDataType item, int index)
        {
            //TryActionOnFile.TryToOpenAndPerformAction(FileName, stream =>
            //{

            //        if (index > -1)
            //        {
            //            stream.Position = index * _structSize;
            //        }
            //        else
            //        {
            //            int? deleted = _deletedIndices.PokeInvalid();
            //            if (deleted != null)
            //            {
            //                Add(item, (int)deleted);
            //                return;
            //            }
            //            stream.Position = stream.Length;
            //        }

            //        ToBuffer(item);
            //        stream.Position = stream.Length;
            //        stream.Write(_buffer, 0, _buffer.Length);
            //    stream.Close();

            //}, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);


            using (
                FileStream stream = File.Open(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)
                )
            {
                if (index > -1)
                {
                    stream.Position = index * _structSize;
                }
                else
                {
                    int? deleted = _deletedIndices.Poke();
                    if (deleted != null)
                    {
                        Add(item, (int)deleted);
                        return;
                    }
                    stream.Position = stream.Length;
                }

                ToBuffer(item);
                stream.Position = stream.Length;
                stream.Write(_buffer, 0, _buffer.Length);
            }

            //if (index > -1)
            //{
            //    _stream.Position = index*_structSize;
            //}
            //else
            //{
            //    int? deleted = _deletedIndices.PokeInvalid();
            //    if (deleted != null)
            //    {
            //        Add(item, (int)deleted);
            //        return;
            //    }
            //    _stream.Position = _stream.Length;
            //}

            // ToBuffer(item);
            //_stream.Position = _stream.Length;
            //_stream.Write(_buffer, 0, _buffer.Length);
        }

        public override int Add(TDataType item)
        {
            Add(item, -1);
            return 0;
        }

        public override void Remove(TDataType item)
        {
            throw new Exception("can not call Remove");
        }

        public void RemoveByIndex(int index)
        {
            _deletedIndices.Add(index);
        }

        public TDataType? PokeNullable()
        {
            using (FileStream stream = File.Open(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                if (stream.Length < _structSize) return null;

                stream.Position = stream.Length - _structSize;
                int read = stream.Read(_buffer, 0, _structSize);
                if (read != _structSize) throw new Exception("wrong data length");
                stream.SetLength(stream.Length - _structSize);
            }

            //if (_stream.Length < _structSize) return null;
            //_stream.Position = _stream.Length - _structSize;
            //var read = _stream.Read(_buffer, 0, _structSize);
            //if (read != _structSize) throw new Exception("wrong data length");
            //_stream.SetLength(_stream.Length - _structSize);

            return FromBuffer();
        }


        public override void Dispose()
        {
            _deletedIndices.Dispose();
            Marshal.FreeHGlobal(_pointer);
            //_stream.Dispose();
        }

        public override IEnumerable<TDataType> Where(Func<TDataType, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public override TDataType Poke()
        {
            throw new NotImplementedException();
        }

        public override IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override TDataType Get(int key, int featureTypeId, Projection projection = null)
        {
            throw new NotImplementedException();
        }

        public override void RemoveByKey(int key, int featureTypeId)
        {
            throw new NotImplementedException();
        }

        #endregion

        private StructRepository()
        {
        }

        public StructRepository(string path, string marker) : this()
        {
            RepositoryName = path;
            Marker = marker;
            _deletedIndices = new OffsetRepository(RepositoryName, Marker + "_di");
            Open();
        }
    }
}