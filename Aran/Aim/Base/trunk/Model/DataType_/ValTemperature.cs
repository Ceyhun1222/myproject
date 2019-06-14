using Aran.Aim.Enums;

namespace Aran.Aim.DataTypes
{
	public class ValTemperature : ValClass <UomTemperature, double>
	{
        public ValTemperature ()
        {
        }

        public ValTemperature (double value, UomTemperature uom)
            : base (value, uom)
        {
        }

		public override DataType DataType
		{
			get { return DataType.ValTemperature; }
		}
	}
}

namespace Aran.Aim.PropertyEnum
{
    public static class MetadataValTemperature
    {
        public static AimPropInfoList PropInfoList;

        static MetadataValTemperature ()
        {
            PropInfoList = MetadataValClassBase.PropInfoList.Clone ();

            PropInfoList.Add (PropertyValClassBase.Uom, (int) EnumType.UomTemperature);
        }
    }
}