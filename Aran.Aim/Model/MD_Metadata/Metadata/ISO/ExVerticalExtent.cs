using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Metadata.ISO
{
	public class ExVerticalExtent : BtAbstractObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.ExVerticalExtent; }
		}
		
		public double? MinimumValue
		{
			get { return GetNullableFieldValue <double> ((int) PropertyExVerticalExtent.MinimumValue); }
			set { SetNullableFieldValue <double> ((int) PropertyExVerticalExtent.MinimumValue, value); }
		}
		
		public double? MaximumValue
		{
			get { return GetNullableFieldValue <double> ((int) PropertyExVerticalExtent.MaximumValue); }
			set { SetNullableFieldValue <double> ((int) PropertyExVerticalExtent.MaximumValue, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyExVerticalExtent
	{
		MinimumValue = PropertyBtAbstractObject.NEXT_CLASS,
		MaximumValue,
		NEXT_CLASS
	}
	
	public static class MetadataExVerticalExtent
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataExVerticalExtent ()
		{
			PropInfoList = MetadataBtAbstractObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyExVerticalExtent.MinimumValue, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyExVerticalExtent.MaximumValue, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
		}
	}
}
