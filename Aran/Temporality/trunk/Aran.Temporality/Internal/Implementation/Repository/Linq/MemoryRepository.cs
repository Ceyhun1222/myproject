#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aran.Aim.Data;
using Aran.Temporality.Common.Util;
using Aran.Temporality.Internal.Abstract.Repository;
using FluentNHibernate.Utils;

#endregion

namespace Aran.Temporality.Internal.Implementation.Repository.Linq
{
    internal class MemoryRepository<T> : AbstractFileDataRepository<T, long> where T : class
    {
        #region Properties

        private List<T> Items { get; set; } = new List<T>();

        #endregion

        #region Constructors

        private MemoryRepository()
        {
        }

        public MemoryRepository(string path, string marker) : this()
        {
            RepositoryName = path;
            Marker = marker;
            Open();
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return Items.Count + " items";
        }

        #endregion

        #region Implementation of AbstractDataRepository

        public sealed override void Open(bool rewrite = false)
        {
            Items = new List<T>();

            if (File.Exists(FileName) && !rewrite)
            {
                using (FileStream fs = File.Open(FileName, FileMode.Open))
                {
                    try
                    {
                        Items = (List<T>)FormatterUtil.Formatter.Deserialize(fs);
                    }
                    catch
                    {
                        //ignored
                    }
                    if (Items == null) Items = new List<T>();
                }
            }
        }

        public override void Close()
        {
            string dir = FileName.Substring(0, FileName.LastIndexOf("\\", StringComparison.Ordinal));
            Directory.CreateDirectory(dir);
            using (FileStream fs = File.Open(FileName, FileMode.Create))
            {
                FormatterUtil.Formatter.Serialize(fs, Items);
            }
        }

        public override long Add(T item)
        {
            Items.Add(item.DeepClone());
            return Items.Count - 1;
        }

        public override T Get(long offset, int featureTypeId, Projection projection = null)
        {
            return Items[(int)offset].DeepClone();
        }

        public override void Remove(T item)
        {
            Items.Remove(item);
        }

        public override void RemoveByKey(long key, int featureTypeId)
        {
            Items.RemoveAt((int)key);
        }

        public override void RemoveAll()
        {
            Items.Clear();
        }

        #endregion

        #region Implementation of AbstractLinqDataRepository

        public override IEnumerable<T> Where(Func<T, bool> predicate)
        {
            return Items.DeepClone().Where(predicate);
        }

        public override T Poke()
        {
            if (Items.Count == 0) return null;
            T item = Items.Last();
            Items.Remove(item);
            return item;
        }

        public override IEnumerator GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        #endregion

        #region Implementation of IDisposable

        public override void Dispose()
        {
            Close();
            Items.Clear();
            Items = null;
        }

        #endregion
    }
}