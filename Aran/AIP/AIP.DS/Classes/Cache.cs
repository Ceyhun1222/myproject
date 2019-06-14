using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AIP.DataSet.Lib;
using Aran.Aim;
using Aran.Aim.Features;

namespace AIP.DataSet.Classes
{
    public static class Cache
    {
        internal static Dictionary<DateTime, CacheData> CachedData =
            new Dictionary<DateTime, CacheData>();

        internal static void Clear()
        {
            CachedData.Clear();
        }

        internal static List<Feature> Get(FeatureType featType)
        {
            try
            {
                    if (Contains(featType))
                        return CachedData[AIP.EffectiveDate].Data[featType];
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
                if (Contains(featType))
                {
                    return CachedData[AIP.EffectiveDate]?.Data[featType]?.FirstOrDefault(x => x.Identifier == guid);
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        internal static bool Contains(FeatureType featType)
        {
            try
            {
                if (CachedData != null && CachedData.Count > 0 && CachedData.ContainsKey(AIP.EffectiveDate) && Cache.CachedData[AIP.EffectiveDate].Data.ContainsKey(featType))
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
                if (AIP.EffectiveDate != default(DateTime))
                {
                    if (Cache.CachedData == null || Cache.CachedData.Count == 0 || !Cache.CachedData.ContainsKey(AIP.EffectiveDate))
                    {
                        CacheData cache = new CacheData { Data = new Dictionary<FeatureType, List<Feature>> { { featType, lst } } };
                        Cache.CachedData?.Add(AIP.EffectiveDate, cache);
                    }
                    else if (Cache.CachedData.ContainsKey(AIP.EffectiveDate))
                    {
                        Cache.CachedData[AIP.EffectiveDate].Data.Add(featType, lst);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

    }
    public class CacheData
    {
        public Dictionary<FeatureType, List<Feature>> Data;
    }
}
