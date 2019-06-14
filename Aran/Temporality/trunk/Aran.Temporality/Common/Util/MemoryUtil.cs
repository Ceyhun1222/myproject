using System;
using System.IO;
using System.Reflection;
using System.Runtime;
using Aran.Temporality.Internal.Util;

namespace Aran.Temporality.Common.Util
{
    public enum MemoryFactoryType
    {
        Classic,
        MsRecycled,
    }

    public class MemoryUtil
    {
        private static readonly string Tag = "TOSS";
        public static MemoryFactoryType FactoryType = MemoryFactoryType.Classic;
        private static RecyclableMemoryStreamManager _manager;

        private static RecyclableMemoryStreamManager MsManager
        {
            get
            {
                return _manager ?? (_manager = new RecyclableMemoryStreamManager
                {
                    GenerateCallStacks = false

                    //int blockSize = 1024;
                    //int largeBufferMultiple = 1024*1024;
                    //int maxBufferSize = 16*largeBufferMultiple;

                    //var manager = new RecyclableMemoryStreamManager();//blockSize, largeBufferMultiple, maxBufferSize);

                    //manager.AggressiveBufferReturn = true;
                    //manager.MaximumFreeLargePoolBytes = maxBufferSize*4;
                    //manager.MaximumFreeSmallPoolBytes = 100*blockSize;
                });
            }
        }


        public static void CompactLoh()
        {
            var piLohcm = typeof (GCSettings).GetProperty("LargeObjectHeapCompactionMode", BindingFlags.Static | BindingFlags.Public);

            if (null != piLohcm)
            {
                var miSetter = piLohcm.GetSetMethod();
                miSetter.Invoke(null, new object[] {/* GCLargeObjectHeapCompactionMode.CompactOnce */ 2});
            }
            GC.Collect(); // This will cause the LOH to be compacted (once).
        }

        public static MemoryStream GetMemoryStream()
        {
            switch (FactoryType)
            {
                case MemoryFactoryType.Classic:
                    return new MemoryStream();
                case MemoryFactoryType.MsRecycled:
                    return MsManager.GetStream(Tag);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        public static MemoryStream GetMemoryStream(byte[] data)
        {
            switch (FactoryType)
            {
                case MemoryFactoryType.Classic:
                    return new MemoryStream(data);
                case MemoryFactoryType.MsRecycled:
                    return MsManager.GetStream(Tag, data, 0, data.Length);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static MemoryStream GetMemoryStream(byte[] data, int offset, int length)
        {
            switch (FactoryType)
            {
                case MemoryFactoryType.Classic:
                    return new MemoryStream(data,offset, length);
                case MemoryFactoryType.MsRecycled:
                    return MsManager.GetStream(Tag, data, offset, length);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}
