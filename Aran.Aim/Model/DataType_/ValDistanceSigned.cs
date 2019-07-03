using Aran.Aim.Enums;

namespace Aran.Aim.DataTypes
{
	public class ValDistanceSigned : ValClass <UomDistance, double>
	{
        public ValDistanceSigned ()
        {
        }

        public ValDistanceSigned (double value, UomDistance uom)
            : base (value, uom)
        {
        }

		public override DataType DataType
		{
			get { return DataType.ValDistanceSigned; }
		}
	}
}

namespace Aran.Aim.PropertyEnum
{
    public static class MetadataValDistanceSigned
    {
        public static AimPropInfoList PropInfoList;

        static MetadataValDistanceSigned ()
        {
            PropInfoList = MetadataValClassBase.PropInfoList.Clone ();

            PropInfoList.Add (PropertyValClassBase.Uom, (int) EnumType.UomDistance);
        }
    }
}