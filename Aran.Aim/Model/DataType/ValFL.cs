using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.DataTypes
{
	public class ValFL : ADataType
	{
		public override DataType DataType
		{
			get { return DataType.ValFL; }
		}
		
		public uint Value
		{
			get { return GetFieldValue <uint> ((int) PropertyValFL.Value); }
			set { SetFieldValue <uint> ((int) PropertyValFL.Value, value); }
		}
		
		public UomFL Uom
		{
			get { return GetFieldValue <UomFL> ((int) PropertyValFL.Uom); }
			set { SetFieldValue <UomFL> ((int) PropertyValFL.Uom, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyValFL
	{
		Value,
		Uom,
		NEXT_CLASS
	}
	
	public static class MetadataValFL
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataValFL ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyValFL.Value, (int) AimFieldType.SysUInt32);
			PropInfoList.Add (PropertyValFL.Uom, (int) EnumType.UomFL);
		}
	}
}
