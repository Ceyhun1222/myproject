using Aran.Aim.Enums;

namespace Aran.Aim.DataTypes
{
	public class ValDistance : ValClass <UomDistance, double>
	{
        public ValDistance ()
        {
        }

        public ValDistance (double value, UomDistance uom)
            : base (value, uom)
        {
        }

		public override DataType DataType
		{
			get { return DataType.ValDistance; }
		}
	}
}

namespace Aran.Aim.PropertyEnum
{
    public static class MetadataValDistance
    {
        public static AimPropInfoList PropInfoList;

        static MetadataValDistance ()
        {
            PropInfoList = MetadataValClassBase.PropInfoList.Clone ();

            PropInfoList.Add (PropertyValClassBase.Uom, (int) EnumType.UomDistance);
        }
    }
}