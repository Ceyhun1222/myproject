using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.DataTypes
{
	public class ValDistanceVertical : ADataType
	{
		public override DataType DataType
		{
			get { return DataType.ValDistanceVertical; }
		}
		
		public string Value
		{
			get { return GetFieldValue <string> ((int) PropertyValDistanceVertical.Value); }
			set { SetFieldValue <string> ((int) PropertyValDistanceVertical.Value, value); }
		}
		
		public UomDistanceVertical Uom
		{
			get { return GetFieldValue <UomDistanceVertical> ((int) PropertyValDistanceVertical.Uom); }
			set { SetFieldValue <UomDistanceVertical> ((int) PropertyValDistanceVertical.Uom, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyValDistanceVertical
	{
		Value,
		Uom,
		NEXT_CLASS
	}
	
	public static class MetadataValDistanceVertical
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataValDistanceVertical ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyValDistanceVertical.Value, (int) AimFieldType.SysString);
			PropInfoList.Add (PropertyValDistanceVertical.Uom, (int) EnumType.UomDistanceVertical);
		}
	}
}
