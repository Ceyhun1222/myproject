using Aran.Package;

namespace Aran.Aim.PropertyPrecision
{
    public class EnumSubProperty : IPackable
    {
        public int PrecisionFormat;
        public int Enum;

        public void Pack(PackageWriter writer)
        {
            writer.PutInt32(PrecisionFormat);
            writer.PutInt32(Enum);
        }

        public void Unpack(PackageReader reader)
        {
            PrecisionFormat = reader.GetInt32();
            Enum = reader.GetInt32();
        }
    }
}