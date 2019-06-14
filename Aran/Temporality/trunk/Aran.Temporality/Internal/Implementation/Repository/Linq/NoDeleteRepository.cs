#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aran.Aim.Data;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.Internal.Abstract.Repository;
using Aran.Temporality.Internal.Util;
using FileStream = System.IO.FileStream;

#endregion

namespace Aran.Temporality.Internal.Implementation.Repository.Linq
{
    internal class NoDeleteRepository<TDataType> : AbstractFileDataRepository<TDataType, long>
        where TDataType : class
    {

        #region Properties


        private readonly IList<FContainer<TDataType>> _cache = new List<FContainer<TDataType>>();
        protected long CachedOffset;
        public int MaxSubResultSize = 10 * 1024; //10 kb
        public int MaximumCacheSize = 10 * 1024 * 1024; //10 mb
        private readonly ILogger _logger = LogManager.GetLogger(typeof(NoDeleteRepository<TDataType>));

        #endregion

        #region Constructors

        private NoDeleteRepository()
        {
        }

        public NoDeleteRepository(string path, string marker, bool rewrite = false)
            : this()
        {
            RepositoryName = path;
            Marker = marker;
            Open(rewrite);
        }

        #endregion

        #region Methods

        public void PrepareIndexes()
        {
            PreloadCache();
        }

        public void PreloadCache()
        {
            TryActionOnFile.TryToOpenAndPerformAction(FileName,
                                                      f =>
                                                      {
                                                          if (CachedOffset > 0)
                                                          {
                                                              f.Position = CachedOffset;
                                                          }

                                                          FContainer<TDataType> container =
                                                              FContainerFile<TDataType>.GetNextContainer(f);
                                                          while (container != null)
                                                          {
                                                              if (CachedOffset < MaximumCacheSize)
                                                              //cache allowed
                                                              {
                                                                  _cache.Add(container);
                                                                  CachedOffset = f.Position;
                                                              }
                                                              else
                                                              {
                                                                  break;
                                                              }

                                                              //next
                                                              container =
                                                                  FContainerFile<TDataType>.GetNextContainer(f);
                                                          }

                                                          f.Close();
                                                      },
                                                      FileMode.Open,
                                                      FileAccess.Read,
                                                      FileShare.Read, 100);
        }

        #endregion

        #region Implementation of AbstractDataRepository

        public sealed override void Open(bool rewrite = false)
        {
            string folder = FileName.Substring(0, FileName.LastIndexOf("\\", StringComparison.Ordinal));

            Directory.CreateDirectory(folder);

            if (rewrite && File.Exists(FileName))
                File.Delete(FileName);
        }

        public override void Close()
        {
        }

        public override long Add(TDataType item)
        {
            long result = 0;
            TryActionOnFile.TryToOpenAndPerformAction(FileName,
                                                      f =>
                                                      {
                                                          result = f.Position;
                                                          _logger.Trace($"Updating file {FileName} offset: {result}");
                                                          var container = new FContainer<TDataType>
                                                          { Object = item };
                                                          FContainerFile<TDataType>.WriteContainer(container, f);
                                                          f.Close();
                                                          _logger.Trace($"File {FileName} has been updated.");
                                                      },
                                                      FileMode.Append,
                                                      FileAccess.Write,
                                                      FileShare.None);

            return result;
        }

        public override TDataType Get(long offset, int featureTypeId, Projection projection = null)
        {
            TDataType result = null;
            TryActionOnFile.TryToOpenAndPerformAction(FileName,
                f =>
                {
                    f.Position = offset;

                    FContainer<TDataType> container =
                        FContainerFile<TDataType>.GetNextContainer(f);
                    f.Close();
                    //if (container == null) 
                    //{
                    //result = null;
                    //}
                    //else
                    //{
                    result = (TDataType)container.Object;
                    //}

                },
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read);

            return result;
        }

        public override List<TDataType> Get(IList<long> offsets, int featureTypeId, Projection projection = null)
        {
            List<TDataType> result = null;

            TryActionOnFile.TryToOpenAndPerformAction(FileName,
                                                      f =>
                                                      {
                                                          result = new List<TDataType>();
                                                          foreach (var offset in offsets)
                                                          {
                                                              // ReSharper disable RedundantCheckBeforeAssignment
                                                              if (f.Position != offset)
                                                              // ReSharper restore RedundantCheckBeforeAssignment
                                                              {
                                                                  f.Position = offset;
                                                              }

                                                              FContainer<TDataType> container =
                                                                  FContainerFile<TDataType>.GetNextContainer(f);
                                                              result.Add((TDataType)container.Object);
                                                          }
                                                          f.Close();
                                                      },
                                                      FileMode.Open,
                                                      FileAccess.Read,
                                                      FileShare.Read);

            return result;
        }

        public override void Remove(TDataType item)
        {
            throw new NotImplementedException();
        }

        public override void RemoveByKey(long key, int featureTypeId)
        {
            throw new NotImplementedException();
        }

        public override void RemoveAll()
        {
            TryActionOnFile.TryToDelete(FileName);
        }

        #endregion

        #region Implementation of AbstractLinqDataRepository

        public FContainer<TDataType> FindContainer(TDataType item)
        {
            return WhereContainer(t => t.Equals(item)).FirstOrDefault();
        }

        public IEnumerable<FContainer<TDataType>> WhereContainer(Func<TDataType, bool> predicate)
        {
            var result = new List<FContainer<TDataType>>();


            TryActionOnFile.TryToOpenAndPerformAction(FileName,
                                                      f =>
                                                      {
                                                          var subResult = new List<FContainer<TDataType>>();
                                                          long subResultSize = 0;

                                                          if (CachedOffset > 0)
                                                          {
                                                              result.AddRange(
                                                                  _cache.Where(
                                                                      t => predicate((TDataType)t.Object)));
                                                              f.Position = CachedOffset;
                                                          }

                                                          FContainer<TDataType> container =
                                                              FContainerFile<TDataType>.GetNextContainer(f);
                                                          while (container != null)
                                                          {
                                                              //accumulate readed features
                                                              subResult.Add(container);
                                                              subResultSize += container.Size;

                                                              if (CachedOffset < MaximumCacheSize)
                                                              //cache allowed
                                                              {
                                                                  _cache.Add(container);
                                                                  CachedOffset = f.Position;
                                                              }

                                                              //when there are a lot of features
                                                              if (subResultSize > MaxSubResultSize)
                                                              {
                                                                  //process them
                                                                  result.AddRange(
                                                                      subResult.Where(
                                                                          t => predicate((TDataType)t.Object)));
                                                                  subResult.Clear();
                                                                  subResultSize = 0;
                                                              }

                                                              //next
                                                              container =
                                                                  FContainerFile<TDataType>.GetNextContainer(f);
                                                          }

                                                          if (subResult.Count > 0)
                                                          {
                                                              //process last
                                                              result.AddRange(
                                                                  subResult.Where(
                                                                      t => predicate((TDataType)t.Object)));
                                                              subResult.Clear();
                                                          }

                                                          f.Close();
                                                      },
                                                      FileMode.OpenOrCreate,
                                                      FileAccess.Read,
                                                      FileShare.Read);

            return result;
        }

        public override IEnumerable<TDataType> Where(Func<TDataType, bool> predicate)
        {
            IEnumerable<FContainer<TDataType>> containers = WhereContainer(predicate);
            return containers.Select(container => (TDataType)container.Object).ToList();
        }

        public override TDataType Poke()
        {
            throw new NotImplementedException();
        }

        public override IEnumerator GetEnumerator()
        {
            _logger.Info($"Reading file {FileName} .");
            FileStream f = null;
            if (File.Exists(FileName))
            {
                f = File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            }

            if (f != null)
            {
                FContainer<TDataType> container = FContainerFile<TDataType>.GetNextContainer(f);
                while (container != null)
                {
                    yield return container.Object;
                    container = FContainerFile<TDataType>.GetNextContainer(f);
                }
                f.Close();
                _logger.Info($"File {FileName} has been read.");
            }
            else
                _logger.Warn($"File {FileName} does not exist.");
        }

        #endregion

        #region Implementation of IDisposable

        public override void Dispose()
        {
            Close();
        }

        #endregion
    }
}