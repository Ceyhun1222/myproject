using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AIP.GUI;
using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.Features;

namespace AIP.GUI.Classes
{
    public static class Cache
    {
        internal static Dictionary<DateTime, CacheData> CachedData =
            new Dictionary<DateTime, CacheData>();
        private static string cachePath = Directory.GetCurrentDirectory() + @"\Cache\";
        private static CacheType cacheType;

        static Cache()
        {
            cacheType = Properties.Settings.Default.UseCache ? 
                CacheType.Memory : 
                CacheType.Disabled;
        }

        internal static void Enable(bool isEnable)
        {
            cacheType = isEnable ? CacheType.Memory: CacheType.Disabled;
        }

        internal static void Clear()
        {
            CachedData.Clear();
        }
        

        internal static List<Feature> Get(FeatureType featType)
        {
            try
            {
                if (cacheType.HasFlag(CacheType.Memory) && Contains(featType))
                    return 
                        CachedData[Lib.CurrentAIP.Effectivedate].Data[featType];
                if (cacheType.HasFlag(CacheType.Disk) && Contains(featType))
                    return
                        ReadFromBinaryFile<List<Feature>>(featType);
                else
                    return null;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }
        

        internal static Feature GetById(FeatureType featType, Guid guid)
        {
            try
            {
                if (cacheType.HasFlag(CacheType.Memory) && Contains(featType, CacheType.Memory))
                    return CachedData[Lib.CurrentAIP.Effectivedate]?.Data[featType]?.FirstOrDefault(x => x.Identifier == guid);
                else
                if(cacheType.HasFlag(CacheType.Disk) && Contains(featType, CacheType.Memory))
                return CachedData[Lib.CurrentAIP.Effectivedate]?.Data[featType]?.FirstOrDefault(x => x.Identifier == guid);
                else
                    return null;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        internal static bool Contains(FeatureType featType, CacheType checkCache = CacheType.Memory)
        {
            try
            {
                if (cacheType.HasFlag(CacheType.Disabled))
                    return false;

                bool memory = cacheType.HasFlag(CacheType.Memory) && CachedData != null && CachedData.Count > 0 && CachedData.ContainsKey(Lib.CurrentAIP.Effectivedate) && Cache.CachedData[Lib.CurrentAIP.Effectivedate].Data.ContainsKey(featType);
                bool disk = cacheType.HasFlag(CacheType.Disk) && File.Exists(FilePath(featType));
                if (memory || disk)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return false;
            }
        }



        internal static void Add(FeatureType featType, List<Feature> lst)
        {
            try
            {
                if (cacheType.HasFlag(CacheType.Disabled)) return;
                if (Lib.CurrentAIP.Effectivedate != default(DateTime))
                {
                    if (cacheType.HasFlag(CacheType.Memory))
                    {
                        if (Cache.CachedData == null || Cache.CachedData.Count == 0 ||
                            !Cache.CachedData.ContainsKey(Lib.CurrentAIP.Effectivedate))
                        {
                            CacheData cache = new CacheData
                            {
                                Data = new Dictionary<FeatureType, List<Feature>> { { featType, lst } }
                            };
                            Cache.CachedData?.Add(Lib.CurrentAIP.Effectivedate, cache);
                        }
                        else if (Cache.CachedData.ContainsKey(Lib.CurrentAIP.Effectivedate))
                        {
                            Cache.CachedData[Lib.CurrentAIP.Effectivedate].Data.Add(featType, lst);
                        }
                    }
                    if (cacheType.HasFlag(CacheType.Disk))
                    {
                        WriteToBinaryFile(featType, lst);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private static void WriteToBinaryFile<T>(FeatureType featType, T objectToWrite)
        {
            try
            {
                string filePath = FilePath(featType);
                using (Stream stream = File.Open(filePath, FileMode.Create))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    binaryFormatter.Serialize(stream, objectToWrite);
                }
                //File.WriteAllBytes(filePath, ToByteArray(objectToWrite));

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static byte[] ToByteArray(object obj)
        {
            try
            {
                var size = Marshal.SizeOf(obj);
                var bytes = new byte[size];
                var ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(obj, ptr, false);
                Marshal.Copy(ptr, bytes, 0, size);
                Marshal.FreeHGlobal(ptr);
                return bytes;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        private static T ReadFromBinaryFile<T>(FeatureType featType)
        {
            try
            {
                string filePath = FilePath(featType);
                using (Stream stream = File.Open(filePath, FileMode.Open))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    if (cacheType.HasFlag(CacheType.Memory) && !Contains(featType, CacheType.Memory))
                    {
                        Add(featType, (List<Feature>)binaryFormatter.Deserialize(stream));
                    }
                    return (T)binaryFormatter.Deserialize(stream);
                }
                //byte[] obj = File.ReadAllBytes(filePath);
                //return ToObject<T>(obj);

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return default(T);
            }
        }

        public static T ToObject<T>(byte[] byteArray)
        {
            try
            {
                var size = Marshal.SizeOf(byteArray);
                var bytes = new byte[size];
                var ptr = Marshal.AllocHGlobal(size);
                Marshal.Copy(bytes, 0, ptr, size);
                var obj = (T)Marshal.PtrToStructure(ptr, typeof(T));
                Marshal.FreeHGlobal(ptr);
                return obj;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return default(T);
            }
        }

        private static string FilePath(FeatureType featType)
        {
            try
            {
                return $@"{cachePath}data_{Lib.CurrentAIP.Effectivedate.ToString("yyyy-MM-dd")}_{featType}.tmp";
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

    }
    public class CacheData
    {
        public Dictionary<FeatureType, List<Feature>> Data;
    }

    /// <summary>
    /// Cache type
    /// If disabled - original data will be returned
    /// If disk - only data from disk will be returned
    /// If memory - only memory data will be returned
    /// If memory & disk - first from memory, then from disk
    /// </summary>
    [Flags]
    internal enum CacheType
    {
        None = 0x0, // cache not selected
        Disabled = 0x1, // cache disabled
        Disk = 0x2, // cache on the disk - not completed, do not use
        Memory = 0x4 // cache in the memory
    }
}
