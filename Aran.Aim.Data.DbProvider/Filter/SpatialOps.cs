using Aran.Package;
using System;

namespace Aran.Aim.Data.Filters
{
    [Serializable]
    public abstract class SpatialOps : IPackable
    {
        public string PropertyName { get; set; }

        public virtual void Pack(PackageWriter writer)
        {
            writer.PutString(PropertyName);
        }

        public virtual void Unpack(PackageReader reader)
        {
            PropertyName = reader.GetString();
        }
    }
}
