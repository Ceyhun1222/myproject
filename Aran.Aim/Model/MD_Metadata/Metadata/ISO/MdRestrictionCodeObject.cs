using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;

namespace Aran.Aim.Metadata.ISO
{
	public class MdRestrictionCodeObject : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.MdRestrictionCodeObject; }
		}
		
		public MdRestrictionCode? Value
		{
			get { return GetNullableFieldValue <MdRestrictionCode> ((int) PropertyMdRestrictionCodeObject.Value); }
			set { SetNullableFieldValue <MdRestrictionCode> ((int) PropertyMdRestrictionCodeObject.Value, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyMdRestrictionCodeObject
	{
		Value = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataMdRestrictionCodeObject
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataMdRestrictionCodeObject ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyMdRestrictionCodeObject.Value, (int) EnumType.MdRestrictionCode, PropertyTypeCharacter.Nullable);
		}
	}
}
