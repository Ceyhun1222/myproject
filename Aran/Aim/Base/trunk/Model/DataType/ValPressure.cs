using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.DataTypes
{
	public class ValPressure : ADataType
	{
		public override DataType DataType
		{
			get { return DataType.ValPressure; }
		}
		
		public double Value
		{
			get { return GetFieldValue <double> ((int) PropertyValPressure.Value); }
			set { SetFieldValue <double> ((int) PropertyValPressure.Value, value); }
		}
		
		public UomPressure Uom
		{
			get { return GetFieldValue <UomPressure> ((int) PropertyValPressure.Uom); }
			set { SetFieldValue <UomPressure> ((int) PropertyValPressure.Uom, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyValPressure
	{
		Value,
		Uom,
		NEXT_CLASS
	}
	
	public static class MetadataValPressure
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataValPressure ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyValPressure.Value, (int) AimFieldType.SysDouble);
			PropInfoList.Add (PropertyValPressure.Uom, (int) EnumType.UomPressure);
		}
	}
}
