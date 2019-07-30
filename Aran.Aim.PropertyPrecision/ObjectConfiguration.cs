using System;
using System.Collections.Generic;
using Aran.Package;

namespace Aran.Aim.PropertyPrecision
{
    public class ObjectConfiguration : IPackable
    {
        public SortedList<int, PropertyConfiguration> Properties = new SortedList<int, PropertyConfiguration>();

        public void Pack(PackageWriter writer)
        {
            writer.PutInt32(Properties.Count);
            foreach (KeyValuePair<int, PropertyConfiguration> pair in Properties)
            {
                writer.PutInt32(pair.Key);
                writer.PutInt32(pair.Value.PropertyType);
                pair.Value.Pack(writer);
            }
        }

        public void Unpack(PackageReader reader)
        {
            int count = reader.GetInt32();
            for (int i = 0; i < count; i++)
            {
                var key = reader.GetInt32();
                var propertyType = (PropertyTypes)reader.GetInt32();
                PropertyConfiguration value = null;
                switch (propertyType)
                {
                    case PropertyTypes.DoubleProperty:
                        value=new DoublePropertyConfiguration();
                        break;
                    case PropertyTypes.ValProperty:
                        value = new ValPropertyConfiguration();
                        break;
                    case PropertyTypes.ComplexProperty:
                        value = new ComplexPropertyConfiguration();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                value.Unpack(reader);
                Properties[key] = value;
            }
        }
    }
}