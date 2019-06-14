using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.DataTypes
{
	public class ValLightIntensity : ADataType
	{
		public override DataType DataType
		{
			get { return DataType.ValLightIntensity; }
		}
		
		public double Value
		{
			get { return GetFieldValue <double> ((int) PropertyValLightIntensity.Value); }
			set { SetFieldValue <double> ((int) PropertyValLightIntensity.Value, value); }
		}
		
		public UomLightIntensity Uom
		{
			get { return GetFieldValue <UomLightIntensity> ((int) PropertyValLightIntensity.Uom); }
			set { SetFieldValue <UomLightIntensity> ((int) PropertyValLightIntensity.Uom, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyValLightIntensity
	{
		Value,
		Uom,
		NEXT_CLASS
	}
	
	public static class MetadataValLightIntensity
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataValLightIntensity ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyValLightIntensity.Value, (int) AimFieldType.SysDouble);
			PropInfoList.Add (PropertyValLightIntensity.Uom, (int) EnumType.UomLightIntensity);
		}
	}
}
