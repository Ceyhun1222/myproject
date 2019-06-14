#region

using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Internal.Interface.Util;

#endregion

namespace Aran.Temporality.Internal.MetaData.Util
{
    internal class MetaCache<TContainerType> : IHasActual
        where TContainerType : IHasActual
    {
        public bool IsActual { get; set; }

        //feature id (WorkPackage + FeatureName + Guid) => T mapping
        protected readonly SortedList<int,
            SortedList<int, SortedList<Guid, TContainerType>>> MetaByWorkPackages =
                new SortedList<int, SortedList<int, SortedList<Guid, TContainerType>>>();



        public SortedList<int, SortedList<Guid, TContainerType>> GetByWorkPackage(int workpackage)
        {
            SortedList<int, SortedList<Guid, TContainerType>> result;
            return !MetaByWorkPackages.TryGetValue(workpackage, out result) ? null : result;
        }

        public IList<int> GetWorkPackages()
        {
            return MetaByWorkPackages.Keys;
        }

        public IList<TContainerType> GetByFeatureTypeName(IFeatureId meta)
        {
            int workpackage = meta.WorkPackage;
            int featureName = meta.FeatureTypeId;

            //get cache by feature name
            SortedList<int, SortedList<Guid, TContainerType>> metaByFeatureName;
            if (!MetaByWorkPackages.TryGetValue(workpackage, out metaByFeatureName))
            {
                metaByFeatureName = new SortedList<int, SortedList<Guid, TContainerType>>();
                MetaByWorkPackages[workpackage] = metaByFeatureName;
            }

            //get cache by guid
            SortedList<Guid, TContainerType> metaByGuid;
            if (!metaByFeatureName.TryGetValue(featureName, out metaByGuid))
            {
                metaByGuid = new SortedList<Guid, TContainerType>();
                metaByFeatureName[featureName] = metaByGuid;
            }

            return metaByGuid.Values;
        }

        public SortedList<Guid, TContainerType> GetPairsByFeatureTypeName(IFeatureId meta)
        {
            int workpackage = meta.WorkPackage;
            int featureName = meta.FeatureTypeId;

            //get cache by feature name
            SortedList<int, SortedList<Guid, TContainerType>> metaByFeatureName;
            if (!MetaByWorkPackages.TryGetValue(workpackage, out metaByFeatureName))
            {
                metaByFeatureName = new SortedList<int, SortedList<Guid, TContainerType>>();
                MetaByWorkPackages[workpackage] = metaByFeatureName;
            }

            //get cache by guid
            SortedList<Guid, TContainerType> metaByGuid;
            if (!metaByFeatureName.TryGetValue(featureName, out metaByGuid))
            {
                metaByGuid = new SortedList<Guid, TContainerType>();
                metaByFeatureName[featureName] = metaByGuid;
            }

            return metaByGuid;
        }

        public TContainerType GetByFeatureId(IFeatureId meta)
        {
            if (meta.Guid == null) throw new Exception("meta with null Guid in GetByFeatureId");

            int workpackage = meta.WorkPackage;
            var guid = (Guid) meta.Guid;
            int featureName = meta.FeatureTypeId;

            //get cache by feature name
            SortedList<int, SortedList<Guid, TContainerType>> metaByFeatureName;
            if (!MetaByWorkPackages.TryGetValue(workpackage, out metaByFeatureName))
            {
                metaByFeatureName = new SortedList<int, SortedList<Guid, TContainerType>>();
                MetaByWorkPackages[workpackage] = metaByFeatureName;
            }

            //get cache by guid
            SortedList<Guid, TContainerType> metaByGuid;
            if (!metaByFeatureName.TryGetValue(featureName, out metaByGuid))
            {
                metaByGuid = new SortedList<Guid, TContainerType>();
                metaByFeatureName[featureName] = metaByGuid;
            }

            //get data
            TContainerType item;
            if (!metaByGuid.TryGetValue(guid, out item))
            {
                item = Activator.CreateInstance<TContainerType>();
                item.IsActual = IsActual;
                metaByGuid[guid] = item;
            }

            return item;
        }

        public void Clear()
        {
            MetaByWorkPackages.Clear();
        }

        public void Remove(int workpackage)
        {
            MetaByWorkPackages.Remove(workpackage);
        }


        public int GetFeatureTypeById(Guid id)
        {
            foreach (var pair in MetaByWorkPackages.Values.SelectMany(list => list.Where(pair => pair.Value.Keys.Contains(id))))
            {
                return pair.Key;
            }
            return -1;
        }
    }
}