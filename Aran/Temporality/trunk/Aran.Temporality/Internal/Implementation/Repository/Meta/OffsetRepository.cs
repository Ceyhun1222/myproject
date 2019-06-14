#region

using System;
using System.Collections.Generic;
using System.IO;
using Aran.Aim.Data;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Internal.Abstract.Repository;

#endregion

namespace Aran.Temporality.Internal.Implementation.Repository.Meta
{
    internal class OffsetRepository : AbstractDataRepository<Int32, Int32>
    {
        #region Overrides of AbstractDataRepository<int>

        public OffsetRepository(string path, string marker)
        {
            RepositoryName = path;
            Marker = marker;
        }


        public override void Open(bool rewrite = false)
        {
            string folder = FileName.Substring(0, FileName.LastIndexOf("\\", StringComparison.Ordinal));
            Directory.CreateDirectory(folder);

            if (rewrite && File.Exists(FileName))
            {
                File.Delete(FileName);
            }
        }

        public override void Close()
        {
        }

        public override void RemoveAll()
        {
            Close();
            Open(true);
        }

        public override int Add(int item)
        {
            using (FileStream stream = File.Open(FileName, FileMode.Append, FileAccess.Write, FileShare.None))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(item);
                }
                stream.Close();
            }

            return 0;
        }

        public int? Poke()
        {
            int? result = null;

            using (FileStream stream = File.Open(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                if (stream.Length >= 4)
                {
                    stream.Position = stream.Length - 4;
                    var reader = new BinaryReader(stream);
                    result = reader.ReadInt32();
                    stream.SetLength(stream.Length - 4);
                    reader.Close();
                }
                stream.Close();
            }

            return result;
        }

        public override void Remove(int item)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
        }

        #endregion

        public static string FilePath(string repositoryName, string marker)
        {
            return ConfigUtil.StoragePath + "\\" + repositoryName + "\\" + marker + ".fdb";
        }

        protected string FileName
        {
            get { return FilePath(RepositoryName, Marker); }
        }
        public List<int> LoadAll()
        {
            var result = new List<int>();

            using (FileStream stream = File.Open(FileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
            {
                using (var reader = new BinaryReader(stream))
                {
                    while (stream.Position < stream.Length)
                    {
                        result.Add(reader.ReadInt32());
                    }
                }
                stream.Close();
            }

            return result;
        }

        public override void RemoveByKey(int key, int featureTypeId)
        {
            throw new NotImplementedException();
        }

        public override int Get(int key, int featureTypeId, Projection projection = null)
        {
            throw new NotImplementedException();
        }
    }
}