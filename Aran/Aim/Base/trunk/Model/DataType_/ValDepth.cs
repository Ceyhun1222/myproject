using Aran.Aim.Enums;

namespace Aran.Aim.DataTypes
{
    public class ValDepth : ValClass<UomDepth, double>
	{
        public ValDepth ()
        {
        }

        public ValDepth (double value, UomDepth uom)
            : base (value, uom)
        {
        }

		public override DataType DataType
		{
			get { return DataType.ValDepth; }
		}
	}
}

namespace Aran.Aim.PropertyEnum
{
    public static class MetadataValDepth
    {
        public static AimPropInfoList PropInfoList;

        static MetadataValDepth ()
        {
            PropInfoList = MetadataValClassBase.PropInfoList.Clone ();

            PropInfoList.Add (PropertyValClassBase.Uom, (int) EnumType.UomDepth);
        }
    }
}