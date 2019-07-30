using Aran.Package;

namespace Aran.Aim.PropertyPrecision
{
    public class ComplexPropertyConfiguration : PropertyConfiguration
    {
        public ObjectConfiguration ObjectConfiguration=new ObjectConfiguration();
       
        public override void Pack(PackageWriter writer)
        {
            ObjectConfiguration.Pack(writer);
        }

        public override void Unpack(PackageReader reader)
        {
            ObjectConfiguration.Properties.Clear();
            ObjectConfiguration.Unpack(reader);
        }

        public override int PropertyType
        {
            get { return (int)PropertyTypes.ComplexProperty; }
        }
    }
}