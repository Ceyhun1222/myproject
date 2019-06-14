using Aran.Aim.Enums;

namespace Aran.Aim.DataTypes
{
	public class ValPressure : ValClass <UomPressure, double>
	{
        public ValPressure ()
        {
        }

        public ValPressure (double value, UomPressure uom)
            : base (value, uom)
        {
        }

		public override DataType DataType
		{
			get { return DataType.ValPressure; }
		}
	}
}

namespace Aran.Aim.PropertyEnum
{
    public static class MetadataValPressure
    {
        public static AimPropInfoList PropInfoList;

        static MetadataValPressure ()
        {
            PropInfoList = MetadataValClassBase.PropInfoList.Clone ();

            PropInfoList.Add (PropertyValClassBase.Uom, (int) EnumType.UomPressure);
        }
    }
}