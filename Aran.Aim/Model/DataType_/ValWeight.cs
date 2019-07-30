using Aran.Aim.Enums;

namespace Aran.Aim.DataTypes
{
	public class ValWeight : ValClass <UomWeight, double>
	{
        public ValWeight ()
        {
        }

        public ValWeight (double value, UomWeight uom)
            : base (value, uom)
        {
        }

		public override DataType DataType
		{
			get { return DataType.ValWeight; }
		}
	}
}

namespace Aran.Aim.PropertyEnum
{
    public static class MetadataValWeight
    {
        public static AimPropInfoList PropInfoList;

        static MetadataValWeight ()
        {
            PropInfoList = MetadataValClassBase.PropInfoList.Clone ();

            PropInfoList.Add (PropertyValClassBase.Uom, (int) EnumType.UomWeight);
        }
    }
}