using Aran.Aim.Enums;

namespace Aran.Aim.DataTypes
{
	public class ValDuration : ValClass <UomDuration, double>
	{
        public ValDuration ()
        {
        }

        public ValDuration (double value, UomDuration uom)
            : base (value, uom)
        {
        }

		public override DataType DataType
		{
			get { return DataType.ValDuration; }
		}
	}
}

namespace Aran.Aim.PropertyEnum
{
    public static class MetadataValDuration
    {
        public static AimPropInfoList PropInfoList;

        static MetadataValDuration ()
        {
            PropInfoList = MetadataValClassBase.PropInfoList.Clone ();

            PropInfoList.Add (PropertyValClassBase.Uom, (int) EnumType.UomDuration);
        }
    }
}