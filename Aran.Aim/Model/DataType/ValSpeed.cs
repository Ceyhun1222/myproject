using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.DataTypes
{
	public class ValSpeed : ADataType
	{
		public override DataType DataType
		{
			get { return DataType.ValSpeed; }
		}
		
		public double Value
		{
			get { return GetFieldValue <double> ((int) PropertyValSpeed.Value); }
			set { SetFieldValue <double> ((int) PropertyValSpeed.Value, value); }
		}
		
		public UomSpeed Uom
		{
			get { return GetFieldValue <UomSpeed> ((int) PropertyValSpeed.Uom); }
			set { SetFieldValue <UomSpeed> ((int) PropertyValSpeed.Uom, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyValSpeed
	{
		Value,
		Uom,
		NEXT_CLASS
	}
	
	public static class MetadataValSpeed
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataValSpeed ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyValSpeed.Value, (int) AimFieldType.SysDouble);
			PropInfoList.Add (PropertyValSpeed.Uom, (int) EnumType.UomSpeed);
		}
	}
}
