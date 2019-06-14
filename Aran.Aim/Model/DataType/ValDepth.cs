using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.DataTypes
{
	public class ValDepth : ADataType
	{
		public override DataType DataType
		{
			get { return DataType.ValDepth; }
		}
		
		public double Value
		{
			get { return GetFieldValue <double> ((int) PropertyValDepth.Value); }
			set { SetFieldValue <double> ((int) PropertyValDepth.Value, value); }
		}
		
		public UomDepth Uom
		{
			get { return GetFieldValue <UomDepth> ((int) PropertyValDepth.Uom); }
			set { SetFieldValue <UomDepth> ((int) PropertyValDepth.Uom, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyValDepth
	{
		Value,
		Uom,
		NEXT_CLASS
	}
	
	public static class MetadataValDepth
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataValDepth ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyValDepth.Value, (int) AimFieldType.SysDouble);
			PropInfoList.Add (PropertyValDepth.Uom, (int) EnumType.UomDepth);
		}
	}
}
