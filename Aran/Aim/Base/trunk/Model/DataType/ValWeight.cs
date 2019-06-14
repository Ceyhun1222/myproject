using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.DataTypes
{
	public class ValWeight : ADataType
	{
		public override DataType DataType
		{
			get { return DataType.ValWeight; }
		}
		
		public double Value
		{
			get { return GetFieldValue <double> ((int) PropertyValWeight.Value); }
			set { SetFieldValue <double> ((int) PropertyValWeight.Value, value); }
		}
		
		public UomWeight Uom
		{
			get { return GetFieldValue <UomWeight> ((int) PropertyValWeight.Uom); }
			set { SetFieldValue <UomWeight> ((int) PropertyValWeight.Uom, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyValWeight
	{
		Value,
		Uom,
		NEXT_CLASS
	}
	
	public static class MetadataValWeight
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataValWeight ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyValWeight.Value, (int) AimFieldType.SysDouble);
			PropInfoList.Add (PropertyValWeight.Uom, (int) EnumType.UomWeight);
		}
	}
}
