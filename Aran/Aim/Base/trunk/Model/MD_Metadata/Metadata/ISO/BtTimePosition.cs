using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;
using Aran.Aim.Enums;

namespace Aran.Aim.Metadata.ISO
{
	public class BtTimePosition : BtAbstractTimePrimitive
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.BtTimePosition; }
		}
		
		public DateTime? Value
		{
			get { return GetNullableFieldValue <DateTime> ((int) PropertyBtTimePosition.Value); }
			set { SetNullableFieldValue <DateTime> ((int) PropertyBtTimePosition.Value, value); }
		}
		
		public MdTimeIndeterminateValueType? IndeterminatePosition
		{
			get { return GetNullableFieldValue <MdTimeIndeterminateValueType> ((int) PropertyBtTimePosition.IndeterminatePosition); }
			set { SetNullableFieldValue <MdTimeIndeterminateValueType> ((int) PropertyBtTimePosition.IndeterminatePosition, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyBtTimePosition
	{
		Value = PropertyBtAbstractTimePrimitive.NEXT_CLASS,
		IndeterminatePosition,
		NEXT_CLASS
	}
	
	public static class MetadataBtTimePosition
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataBtTimePosition ()
		{
			PropInfoList = MetadataBtAbstractTimePrimitive.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyBtTimePosition.Value, (int) AimFieldType.SysDateTime, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyBtTimePosition.IndeterminatePosition, (int) EnumType.MdTimeIndeterminateValueType, PropertyTypeCharacter.Nullable);
		}
	}
}
