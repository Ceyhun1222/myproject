using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.DataTypes
{
	public class ValDuration : ADataType
	{
		public override DataType DataType
		{
			get { return DataType.ValDuration; }
		}
		
		public double Value
		{
			get { return GetFieldValue <double> ((int) PropertyValDuration.Value); }
			set { SetFieldValue <double> ((int) PropertyValDuration.Value, value); }
		}
		
		public UomDuration Uom
		{
			get { return GetFieldValue <UomDuration> ((int) PropertyValDuration.Uom); }
			set { SetFieldValue <UomDuration> ((int) PropertyValDuration.Uom, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyValDuration
	{
		Value,
		Uom,
		NEXT_CLASS
	}
	
	public static class MetadataValDuration
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataValDuration ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyValDuration.Value, (int) AimFieldType.SysDouble);
			PropInfoList.Add (PropertyValDuration.Uom, (int) EnumType.UomDuration);
		}
	}
}
