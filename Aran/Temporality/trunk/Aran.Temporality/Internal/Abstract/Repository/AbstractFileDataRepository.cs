#region

using System;
using System.Collections;
using System.Collections.Generic;
using Aran.Temporality.Common.Config;

#endregion

namespace Aran.Temporality.Internal.Abstract.Repository
{
    internal abstract class AbstractFileDataRepository<TDataType, TKey> : AbstractLinqDataRepository<TDataType, TKey>
    {
        public static string FilePath(string repositoryName, string marker)
        {
            return ConfigUtil.StoragePath + "\\" + repositoryName + "\\" + marker + ".fdb";
        }

        protected string FileName
        {
            get { return FilePath(RepositoryName, Marker); }
        }
    }
}