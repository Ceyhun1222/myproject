using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.DataTypes
{
	public class ValDistanceSigned : ADataType
	{
		public override DataType DataType
		{
			get { return DataType.ValDistanceSigned; }
		}
		
		public double Value
		{
			get { return GetFieldValue <double> ((int) PropertyValDistanceSigned.Value); }
			set { SetFieldValue <double> ((int) PropertyValDistanceSigned.Value, value); }
		}
		
		public UomDistance Uom
		{
			get { return GetFieldValue <UomDistance> ((int) PropertyValDistanceSigned.Uom); }
			set { SetFieldValue <UomDistance> ((int) PropertyValDistanceSigned.Uom, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyValDistanceSigned
	{
		Value,
		Uom,
		NEXT_CLASS
	}
	
	public static class MetadataValDistanceSigned
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataValDistanceSigned ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyValDistanceSigned.Value, (int) AimFieldType.SysDouble);
			PropInfoList.Add (PropertyValDistanceSigned.Uom, (int) EnumType.UomDistance);
		}
	}
}
