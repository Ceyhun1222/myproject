#region

using System;
using Aran.Temporality.Common.Util;
using System.Runtime.CompilerServices;

#endregion

[assembly: InternalsVisibleTo("TemporalityTest")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Aran.Temporality.Internal.Util
{
    internal class FContainer<T> where T : class
    {
        #region Fields

        public byte[] Data;
        public bool Delete;
        public Guid Id = Guid.NewGuid();
        public long Size;

        #endregion

        #region Properties and util methods

        private object _object;

        public Object Object
        {
            get { return _object ?? (_object = FormatterUtil.ObjectFromBytes<T>(Data)); }
            set
            {
                _object = value;
                Data = FormatterUtil.CompressMaximumObjectToBytes(_object);
            }
        }

        public long GetSize()
        {
            return 16 + //id
                   sizeof (long) + //size
                   1 + //Delete
                   (Delete || Data == null ? 0 : Data.Length); //Data if any
        }

        #endregion
    }
}