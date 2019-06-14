#region

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.Common.Util;

#endregion

namespace Aran.Temporality.Internal.Util
{
    internal class TryActionOnFile
    {
        private static bool TryToPerformArbitraryAction(String path, Action action, int milliSecondMax = Timeout.Infinite)
        {
            bool result;
            DateTime dateTimestart = DateTime.Now;
            Tuple<AutoResetEvent, FileSystemWatcher> tuple = null;

            while (true)
            {
                try
                {
                    action();

                    result = true;
                    break;
                }
                catch (IOException ex)
                {
                    // Init only once and only if needed. Prevent against many instantiation in case of multhreaded 
                    // file access concurrency (if file is frequently accessed by someone else). Better memory usage.
                    LogManager.GetLogger(typeof(TryActionOnFile)).Error(ex, ex.Message);
                    if (tuple == null)
                    {
                        var autoResetEvent = new AutoResetEvent(true);
                        var fileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(path))
                        {
                            EnableRaisingEvents = true
                        };

                        fileSystemWatcher.Changed +=
                            (o, e) =>
                            {
                                if (Path.GetFullPath(e.FullPath) == Path.GetFullPath(path))
                                {
                                    autoResetEvent.Set();
                                }
                            };

                        tuple = new Tuple<AutoResetEvent, FileSystemWatcher>(autoResetEvent, fileSystemWatcher);
                    }

                    int milliSecond = Timeout.Infinite;
                    if (milliSecondMax != Timeout.Infinite)
                    {
                        milliSecond = (int)(DateTime.Now - dateTimestart).TotalMilliseconds;
                        if (milliSecond >= milliSecondMax)
                        {
                            result = false;
                            break;
                        }
                    }

                    tuple.Item1.WaitOne(milliSecond);
                }
            }

            if (tuple?.Item1 != null) // Dispose of resources now (don't wait the GC).
            {
                tuple.Item1.Dispose();
                tuple.Item2.Dispose();
            }

            return result;
        }

        public static bool TryToDelete(string path, int milliSecondMax = Timeout.Infinite)
        {
            return TryToPerformArbitraryAction(path, () => { if (File.Exists(path)) File.Delete(path); }, milliSecondMax);
        }

        private static SortedList<string, ByteArrayStreamFactory> _optimizer = new SortedList<string, ByteArrayStreamFactory>();


        public static bool TryToOpenAndPerformAction(string path, Action<Stream> action,
            FileMode fileMode = FileMode.OpenOrCreate,
            FileAccess fileAccess = FileAccess.ReadWrite,
            FileShare fileShare = FileShare.None,
            int milliSecondMax = Timeout.Infinite)
        {
            //ByteArrayStreamFactory factory = null;
            //if (!_optimizer.TryGetValue(path, out factory))
            //{
            //    lock (_optimizer)
            //    {
            //        if (!_optimizer.TryGetValue(path, out factory))
            //        {
            //            //prepare plexer
            //            factory = new ByteArrayStreamFactory();
            //            FileInfo info = new FileInfo(path);
            //            int length = (int)info.Length;
            //            byte[] data = new byte[length];
            //            using (var fileReader = new FileStream(path, FileMode.Open))
            //            {
            //                fileReader.Read(data, 0, length);
            //            }
            //            factory.SetData(data);
            //            //add it
            //            _optimizer.Add(path, factory);
            //        }
            //    }
            //}

            //if (factory != null)
            //{
            //    using (var reader = factory.GetReader())
            //    {
            //        action(reader);
            //    }
            //    return true;
            //}

            return TryToPerformArbitraryAction(path, () =>
            {
                using (FileStream file = File.Open(path, fileMode, fileAccess, fileShare))
                {
                    action(file);
                }
            }, milliSecondMax);
        }

        public static bool TryToOpenAndPerformActionOverMmf(string path, Action<Stream> action,
    FileMode fileMode = FileMode.OpenOrCreate,
    FileAccess fileAccess = FileAccess.ReadWrite,
    FileShare fileShare = FileShare.None,
    int milliSecondMax = Timeout.Infinite)
        {
            if (fileAccess == FileAccess.Read)
            {
                FileInfo fInfo = new FileInfo(path);
                int length = (int)fInfo.Length;

                var simplePath = Path.GetFileName(path);
                var mapName = "mmf_" + simplePath;//Global\\ or Local\\
                var mutexName = "mut_" + simplePath;
                MemoryMappedFile mmFile = null;
                Mutex mmMutex = null;
                try
                {
                    mmFile = MemoryMappedFile.OpenExisting(mapName);
                    //check for lock and wait
                    try
                    {
                        mmMutex = Mutex.OpenExisting(mutexName);
                        mmMutex.WaitOne();
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(typeof(TryActionOnFile)).Error(ex, ex.Message);
                        bool IsmutexCreated;
                        mmMutex = new Mutex(true, mutexName, out IsmutexCreated);
                    }
                    using (var accessor = mmFile.CreateViewStream(0, length, MemoryMappedFileAccess.Read))
                    {
                        try
                        {
                            action(accessor);
                        }
                        catch (Exception ex)
                        {
                            LogManager.GetLogger(typeof(TryActionOnFile)).Error(ex, ex.Message);
                        }
                    }
                    mmMutex.ReleaseMutex();
                    return true;
                }
                catch (Exception ex)
                {
                    LogManager.GetLogger(typeof(TryActionOnFile)).Error(ex, ex.Message);
                    bool ismutexCreated;
                    mmMutex = new Mutex(true, mutexName, out ismutexCreated);

                    //no such memory map

                    byte[] data = new byte[length];
                    //create mmf
                    mmFile = MemoryMappedFile.CreateNew(mapName, length, MemoryMappedFileAccess.ReadWrite);
                    //copy from file to mmf
                    using (var streamReader = new FileStream(path, FileMode.Open))
                    using (var accessor = mmFile.CreateViewAccessor(0, length, MemoryMappedFileAccess.Write))
                    {
                        streamReader.Read(data, 0, length);
                        accessor.WriteArray(0, data, 0, length);
                    }
                    mmMutex.ReleaseMutex();

                    TryToOpenAndPerformAction(path, action, fileMode, fileAccess, fileShare, milliSecondMax);
                }
            }

            if (fileAccess != FileAccess.Read)
            {

            }

            return true;
        }

    }
}