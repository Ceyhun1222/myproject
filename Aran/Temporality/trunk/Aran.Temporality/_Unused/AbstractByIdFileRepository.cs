#region

using System;
using System.IO;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.Util;

#endregion

namespace Aran.Temporality._Unused
{
    internal abstract class AbstractByIdFileRepository<TDataType> : AbstractByIdDataRepository<TDataType>
        where TDataType : IFeatureId
    {
        #region Overrides of AbstractDataRepository<TDataType>

        public override void Open(bool rewrite = false)
        {
            Directory.CreateDirectory(FileName);
        }

        public override void Close()
        {
        }


        public override void RemoveAll()
        {
            FileUtil.DeleteDirectory(FileName);
        }

        public override void Remove(TDataType item)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
        }

        #endregion
    }
}