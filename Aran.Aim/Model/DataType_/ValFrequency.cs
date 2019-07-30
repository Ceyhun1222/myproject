using Aran.Aim.Enums;

namespace Aran.Aim.DataTypes
{
	public class ValFrequency : ValClass <UomFrequency, double>
	{
        public ValFrequency ()
        {
        }

        public ValFrequency (double value, UomFrequency uom)
            : base (value, uom)
        {
        }

		public override DataType DataType
		{
			get { return DataType.ValFrequency; }
		}
	}
}

namespace Aran.Aim.PropertyEnum
{
    public static class MetadataValFrequency
    {
        public static AimPropInfoList PropInfoList;

        static MetadataValFrequency ()
        {
            PropInfoList = MetadataValClassBase.PropInfoList.Clone ();

            PropInfoList.Add (PropertyValClassBase.Uom, (int) EnumType.UomFrequency);
        }
    }
}