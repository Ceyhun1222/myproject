using Aran.Package;

namespace Aran.Aim.PropertyPrecision
{
    public class DoublePropertyConfiguration : PropertyConfiguration
    {
        public int PrecisionFormat;

        public override void Pack(PackageWriter writer)
        {
            writer.PutInt32(PrecisionFormat);
        }

        public override void Unpack(PackageReader reader)
        {
            PrecisionFormat=reader.GetInt32();
        }

        public override int PropertyType
        {
            get { return (int) PropertyTypes.DoubleProperty; }
        }
    }
}