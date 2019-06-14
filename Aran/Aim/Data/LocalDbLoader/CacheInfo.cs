using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Package;

namespace Aran.Aim.Data.LocalDbLoader
{
    class CacheInfo : IPackable
    {
        public CacheInfo()
        {
            FeatureTypes = new List<FeatureType>();
        }

        public List<FeatureType> FeatureTypes { get; private set; }

        public void Pack(PackageWriter writer)
        {
            var count = FeatureTypes.Count;
            writer.PutInt32(count);
            foreach (var featType in FeatureTypes)
                writer.PutEnum<FeatureType>(featType);
        }

        public void Unpack(PackageReader reader)
        {
            FeatureTypes.Clear();
            var count = reader.GetInt32();
            for (int i = 0; i < count; i++)
                FeatureTypes.Add(reader.GetEnum<FeatureType>());
        }
    }
}
