using Aran.Package;

namespace Aran.Aim.PropertyPrecision
{
    public abstract class PropertyConfiguration : IPackable
    {
        public abstract void Pack(PackageWriter writer);
        public abstract void Unpack(PackageReader reader);
        public abstract int PropertyType { get; }
    }
}