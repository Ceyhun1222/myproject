using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.DataTypes
{
	public class ValFrequency : ADataType
	{
		public override DataType DataType
		{
			get { return DataType.ValFrequency; }
		}
		
		public double Value
		{
			get { return GetFieldValue <double> ((int) PropertyValFrequency.Value); }
			set { SetFieldValue <double> ((int) PropertyValFrequency.Value, value); }
		}
		
		public UomFrequency Uom
		{
			get { return GetFieldValue <UomFrequency> ((int) PropertyValFrequency.Uom); }
			set { SetFieldValue <UomFrequency> ((int) PropertyValFrequency.Uom, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyValFrequency
	{
		Value,
		Uom,
		NEXT_CLASS
	}
	
	public static class MetadataValFrequency
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataValFrequency ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyValFrequency.Value, (int) AimFieldType.SysDouble);
			PropInfoList.Add (PropertyValFrequency.Uom, (int) EnumType.UomFrequency);
		}
	}
}
