using Aran.Aim.Enums;

namespace Aran.Aim.DataTypes
{
	public class ValSpeed : ValClass <UomSpeed, double>
	{
        public ValSpeed ()
        {
        }

        public ValSpeed (double value, UomSpeed uom)
            : base (value, uom)
        {
        }

		public override DataType DataType
		{
			get { return DataType.ValSpeed; }
		}		
	}
}

namespace Aran.Aim.PropertyEnum
{
    public static class MetadataValSpeed
    {
        public static AimPropInfoList PropInfoList;

        static MetadataValSpeed ()
        {
            PropInfoList = MetadataValClassBase.PropInfoList.Clone ();

            PropInfoList.Add (PropertyValClassBase.Uom, (int) EnumType.UomSpeed);
        }
    }
}