using Aran.Aim.Enums;

namespace Aran.Aim.DataTypes
{
	public class ValLightIntensity : ValClass <UomLightIntensity, double>
	{
        public ValLightIntensity ()
        {
        }

        public ValLightIntensity (double value, UomLightIntensity uom)
            : base (value, uom)
        {
        }

		public override DataType DataType
		{
			get { return DataType.ValLightIntensity; }
		}
	}
}

namespace Aran.Aim.PropertyEnum
{
    public static class MetadataValLightIntensity
    {
        public static AimPropInfoList PropInfoList;

        static MetadataValLightIntensity ()
        {
            PropInfoList = MetadataValClassBase.PropInfoList.Clone ();

            PropInfoList.Add (PropertyValClassBase.Uom, (int) EnumType.UomLightIntensity);
        }
    }
}