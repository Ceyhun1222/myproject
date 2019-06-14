using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.DataTypes
{
	public class ValTemperature : ADataType
	{
		public override DataType DataType
		{
			get { return DataType.ValTemperature; }
		}
		
		public double Value
		{
			get { return GetFieldValue <double> ((int) PropertyValTemperature.Value); }
			set { SetFieldValue <double> ((int) PropertyValTemperature.Value, value); }
		}
		
		public UomTemperature Uom
		{
			get { return GetFieldValue <UomTemperature> ((int) PropertyValTemperature.Uom); }
			set { SetFieldValue <UomTemperature> ((int) PropertyValTemperature.Uom, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyValTemperature
	{
		Value,
		Uom,
		NEXT_CLASS
	}
	
	public static class MetadataValTemperature
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataValTemperature ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyValTemperature.Value, (int) AimFieldType.SysDouble);
			PropInfoList.Add (PropertyValTemperature.Uom, (int) EnumType.UomTemperature);
		}
	}
}
