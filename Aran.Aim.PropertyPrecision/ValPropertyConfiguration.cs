using System.Collections.Generic;
using Aran.Package;

namespace Aran.Aim.PropertyPrecision
{
    public class ValPropertyConfiguration : PropertyConfiguration
    {
        public SortedList<int, EnumSubProperty> EnumProperties = new SortedList<int, EnumSubProperty>();

        public override void Pack(PackageWriter writer)
        {
            writer.PutInt32(EnumProperties.Count);
            foreach (KeyValuePair<int, EnumSubProperty> pair in EnumProperties)
            {
                writer.PutInt32(pair.Key);
                pair.Value.Pack(writer);
            }
        }

        public override void Unpack(PackageReader reader)
        {
            EnumProperties.Clear();
            int count = reader.GetInt32();
            for (int i = 0; i < count; i++)
            {
                var key = reader.GetInt32();
                var value = new EnumSubProperty();
                value.Unpack(reader);
                EnumProperties[key] = value;
            }
        }

        public override int PropertyType
        {
            get { return (int)PropertyTypes.ValProperty; }
        }
    }
}