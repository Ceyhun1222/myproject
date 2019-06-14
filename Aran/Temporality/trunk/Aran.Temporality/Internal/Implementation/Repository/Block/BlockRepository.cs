#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Aran.Aim.Data;
using Aran.Temporality.Common.Util;
using Aran.Temporality.Internal.Abstract.Repository;
using Aran.Temporality.Internal.Implementation.Repository.Meta;
using Aran.Temporality.Internal.MetaData;
using Aran.Temporality.Internal.Struct;
using Aran.Temporality.Internal.Util;
using ESRI.ArcGIS.esriSystem;

#endregion

namespace Aran.Temporality.Internal.Implementation.Repository.Block
{
    internal class BlockRepository<TDataType> : AbstractFileDataRepository<TDataType, BlockOffsetStructure>
        where TDataType : class
    {
        #region Holes storage

        private readonly SortedList<int, OffsetRepository> _holeRepositories = new SortedList<int, OffsetRepository>();

        private OffsetRepository HoleRepository(int length)
        {
            int sizeInBits = BlockOffsetStructure.Log2(length);
            if (sizeInBits < BlockOffsetStructure.MinBlockBits)
            {
                sizeInBits = 0;
            }

            if (!_holeRepositories.ContainsKey(sizeInBits))
            {
                _holeRepositories[sizeInBits] = new OffsetRepository(RepositoryName, Marker + "_h" + sizeInBits);
            }

            return _holeRepositories[sizeInBits];
        }

        public void AddHole(DataSegment hole)
        {
            HoleRepository(hole.Length).Add(hole.Offset);
        }

        private DataSegment GetHoleByLength(int length)
        {
            int? hole = HoleRepository(length).Poke();
            return hole != null ? new DataSegment { Offset = (int)hole, Length = length } : null;
        }

        #endregion

        private BlockRepository()
        {
        }

        public BlockRepository(string path, string marker)
            : this()
        {
            RepositoryName = path;
            Marker = marker;

            Open();
        }

        #region Overrides of AbstractDataRepository<TDataType>

        public override void Open(bool rewrite = false)
        {
            string folder = FileName.Substring(0, FileName.LastIndexOf("\\", StringComparison.Ordinal));
            Directory.CreateDirectory(folder);
        }

        public override void Close()
        {
            foreach (var repo in _holeRepositories.Values)
            {
                repo.Close();
            }
        }

        public override void RemoveAll()
        {
            TryActionOnFile.TryToDelete(FileName);

            foreach (var repository in _holeRepositories.Values)
            {
                repository.RemoveAll();
            }
        }

        public override TDataType Get(BlockOffsetStructure offset, int featureTypeId, Projection projection = null)
        {
            TDataType result = null;

            TryActionOnFile.TryToOpenAndPerformAction(FileName,
                                                      f =>
                                                      {
                                                          var bytes = new byte[offset.BlockLength];
                                                          IList<DataSegment> segments = offset.GetSegments();
                                                          int pos = offset.BlockLength;
                                                          foreach (var segment in segments)
                                                          {
                                                              segment.LoadData(bytes, f);
                                                              pos -= segment.Length;
                                                              Buffer.BlockCopy(segment.Data, 0, bytes, pos,
                                                                               segment.Length);
                                                          }
                                                          f.Close();

                                                          result =
                                                              FormatterUtil.ObjectFromBytes<TDataType>(bytes);
                                                      },
                                                      FileMode.Open,
                                                      FileAccess.Read,
                                                      FileShare.Read);

            return result;
        }

        private void WriteSegment(Stream f, DataSegment segment)
        {
            DataSegment hole = GetHoleByLength(segment.Length);
            if (hole != null)
            {
                segment.Offset = hole.Offset;
                f.Position = segment.Offset;
                f.Write(segment.Data, 0, segment.Length);
            }
            else
            {
                segment.Offset = (int)f.Length;
                f.Position = segment.Offset;
                f.Write(segment.Data, 0, segment.Length);

                //align minimal size
                int remain = (1 << BlockOffsetStructure.MinBlockBits) - segment.Length;
                if (remain > 0)
                {
                    f.SetLength(f.Length + remain);
                }
            }
        }

        public override BlockOffsetStructure Add(TDataType item)
        {
            var result = new BlockOffsetStructure();
            TryActionOnFile.TryToOpenAndPerformAction(FileName,
                                                      f =>
                                                      {
                                                          byte[] dataBytes = FormatterUtil.ObjectToBytes(item);
                                                          IList<DataSegment> segments =
                                                              BlockOffsetStructure.FromData(dataBytes);

                                                          foreach (var segment in segments)
                                                          {
                                                              WriteSegment(f, segment);
                                                          }
                                                          f.Close();

                                                          result = BlockOffsetStructure.FromSegments(segments);
                                                      },
                                                      FileMode.OpenOrCreate,
                                                      FileAccess.Write,
                                                      FileShare.None);

            return result;
        }

        public override IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override void Remove(TDataType item)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<TDataType> Where(Func<TDataType, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public override TDataType Poke()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            foreach (var repo in _holeRepositories.Values)
            {
                repo.Dispose();
            }
        }

        public override void RemoveByKey(BlockOffsetStructure key, int featureTypeId)
        {
            throw new NotImplementedException();
        }

        #endregion


    }
}