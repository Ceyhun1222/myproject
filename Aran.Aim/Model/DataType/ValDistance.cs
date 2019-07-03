using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.DataTypes
{
	public class ValDistance : ADataType
	{
		public override DataType DataType
		{
			get { return DataType.ValDistance; }
		}
		
		public double Value
		{
			get { return GetFieldValue <double> ((int) PropertyValDistance.Value); }
			set { SetFieldValue <double> ((int) PropertyValDistance.Value, value); }
		}
		
		public UomDistance Uom
		{
			get { return GetFieldValue <UomDistance> ((int) PropertyValDistance.Uom); }
			set { SetFieldValue <UomDistance> ((int) PropertyValDistance.Uom, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyValDistance
	{
		Value,
		Uom,
		NEXT_CLASS
	}
	
	public static class MetadataValDistance
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataValDistance ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyValDistance.Value, (int) AimFieldType.SysDouble);
			PropInfoList.Add (PropertyValDistance.Uom, (int) EnumType.UomDistance);
		}
	}
}
