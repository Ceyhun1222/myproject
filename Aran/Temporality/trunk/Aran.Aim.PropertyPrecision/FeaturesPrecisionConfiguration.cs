using System.Collections.Generic;
using System.IO;
using Aran.Aim.Package;
using Aran.Package;

namespace Aran.Aim.PropertyPrecision
{
    public class FeaturesPrecisionConfiguration : IPackable
    {
        public string ConfigurationName;
        public SortedList<int, ComplexPropertyConfiguration> FeatureConfigurations = new SortedList<int, ComplexPropertyConfiguration>();

        public void Pack(PackageWriter writer)
        {
            if (string.IsNullOrEmpty(ConfigurationName))
            {
                ConfigurationName = "noname";
            }
            writer.PutString(ConfigurationName);
            writer.PutInt32(FeatureConfigurations.Count);
            foreach (KeyValuePair<int, ComplexPropertyConfiguration> pair in FeatureConfigurations)
            {
                writer.PutInt32(pair.Key);
                pair.Value.Pack(writer);
            }
        }

        public void Unpack(PackageReader reader)
        {
            ConfigurationName=reader.GetString();
            FeatureConfigurations.Clear();
            int count = reader.GetInt32();
            for (int i = 0; i < count; i++)
            {
                var key=reader.GetInt32();
                var value = new ComplexPropertyConfiguration();
                value.Unpack(reader);
                FeatureConfigurations[key] = value;
            }
        }

        public void FromBytes(byte[] bytes)
        {
            if (bytes == null) return;
            if (bytes.Length == 0) return;

            using (var memoryStream = new MemoryStream(bytes))
            {
                using (var reader = new AranPackageReader(memoryStream))
                {
                     Unpack(reader);
                }
            }
        }

        public byte[] ToBytes()
        {
            byte[] result;
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new AranPackageWriter(memoryStream))
                {
                    Pack(writer);
                    result = memoryStream.ToArray();
                }
            }
            return result;
        }
    }
}
