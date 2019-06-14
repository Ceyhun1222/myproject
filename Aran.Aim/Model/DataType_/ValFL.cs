using Aran.Aim.Enums;

namespace Aran.Aim.DataTypes
{
	public class ValFL : ValClass <UomFL, uint>
	{
        public ValFL ()
        {
        }

        public ValFL (uint value, UomFL uom)
            : base (value, uom)
        {
        }

		public override DataType DataType
		{
			get { return DataType.ValFL; }
		}
	}
}

namespace Aran.Aim.PropertyEnum
{
    public static class MetadataValFL
    {
        public static AimPropInfoList PropInfoList;

        static MetadataValFL ()
        {
            PropInfoList = MetadataAimObject.PropInfoList.Clone ();

            PropInfoList.Add (PropertyValClassBase.Value, (int) AimFieldType.SysUInt32);
            PropInfoList.Add (PropertyValClassBase.Uom, (int) EnumType.UomFL);
        }
    }
}